/* To compile this file in Linux, type:   gcc -o client client.c -lpthread -w*/

/*************************************************************************

DA-IICT, Gandhinagar
@author Jugal Kalal and Het Patel

**************************************************************************
*/


#include <stdio.h> /*Standard Input/Output library*/
#include <stdlib.h> /*Standard C library includes functions like type conversion, memory allocation, proccess control*/
#include <string.h> /*Include String functions*/
#include <unistd.h> /*Includes all definitions like null pointer exceptions*/
#include <arpa/inet.h> /*in_addr_t and in_port_t related definitions*/
#include <sys/types.h> /*It enables us to use Inbuilt functions such as clocktime, offset, ptrdiff_t for substracting two pointers*/
#include <netinet/in.h> /*Contains constants and structures needed for internet domain addresses*/
#include <sys/socket.h> /*Socket operations, definitions and its structures*/
#include <pthread.h> /* For creation and management of threads*/
#include <stdbool.h> /*For using boolean data type*/
 
#define MAXRCVLEN 200 /*Maximum size of buffer*/
#define STRLEN 200 /*Maximum String length*/

#define SERVERPORTNUM 27428  /* Port number of the server  */

#define HOSTPORTNUM 27429  /* Local port number of the client machine that peers will use to connect  */
#define PEERPORTNUM 27429	/* Peer(Opponent) port number */

char name[STRLEN],	/* name used to join the server  */ 
opponent[STRLEN];	/* name of the peer(Opponent)  */ 
pthread_t  tid; /*Thread ID*/  
int yours=0; /* User's score*/
int oppos=0; /* Peer's(Opponent) Score*/
/*
 * Logic for the tic tac toe game.
 * Takes the socket of the connection, a char pointer for communication
 * and the player number to distinguish between players.
 */
void playgame(int  socket, char * buffer, int playerID);

void * serverthread(void * parm);	/* Thread function to handle communication with the server */
void * peerthread(char * ip); 		/* Thread function used when we want to open a connection with a peer */

int serversocket, peersocket, mysocket, consocket; /* All socket descriptors are stored as global variables */
int ingame, turn; /* Some variables to handle game logic. */

int main(int argc, char *argv[])
{
	printf("Enter Server's IP: ");
	const char server[30]; /* To store IP address of server */
	scanf("%s",server);
	
	/* Establish connection with the server */
	struct sockaddr_in dest;
	serversocket = socket(AF_INET, SOCK_STREAM, 0);	/* Use a TCP connection */ /*create an unbound socket in a communications domain*/
	memset(&dest, 0, sizeof(dest));           
	dest.sin_family = AF_INET;
	dest.sin_addr.s_addr = htonl(inet_network(server)); /*function converts the unsigned integer hostlong from host byte order to network byte order. */ 
	dest.sin_port = htons(SERVERPORTNUM);               
	connect(serversocket, (struct sockaddr *)&dest, sizeof(struct sockaddr));
	/* The connection is established. Handle communication in a separate thread. */
	pthread_create(&tid, NULL, serverthread, (void*) &serversocket);   
    
    /* For Invitation */ 
    /* Bind to a socket and start listening for incomming connections. */
	struct sockaddr_in peer;
	struct sockaddr_in serv;
	socklen_t socksize = sizeof(struct sockaddr_in);
	memset(&serv, 0, sizeof(serv)); 
    serv.sin_family = AF_INET;   
    serv.sin_addr.s_addr = htonl(INADDR_ANY); 
    serv.sin_port = htons(HOSTPORTNUM);
	mysocket = socket(AF_INET, SOCK_STREAM, 0); /* Use a TCP connection */
	bind(mysocket, (struct sockaddr *)&serv, sizeof(struct sockaddr));
	listen(mysocket, 1); /* Only accept one connection */
	consocket = accept(mysocket, (struct sockaddr *)&peer, &socksize);    
	
	char buffer[MAXRCVLEN];	/* Buffer to hold data exchanged */
    int len;
	
	/* If a connection is received then run the following logic for the duration of the connection. */
	while(consocket)
	{
		/* Since the server thread is constantly reading from input stream for user commands, 
		 * we need to cancel this thread so we can redirect input to the peer.
		 * This does not cancel the connection as it was established outside the thread.
		 */
		pthread_cancel(tid);
		
		/* Receive peer's name and store it as current opponent */
		len = recv(consocket, buffer, MAXRCVLEN, 0); 
		buffer[len] = '\0';
		strcpy(opponent, buffer);	
		printf("\n%s has challenged you. Accept challenge? (y/n) ", opponent);
		
		/* Ask for approval */
		fgets(buffer, sizeof(buffer), stdin);
		buffer[strlen(buffer)-1] = '\0';
		if (strcmp(buffer, "y") == 0)
		{	
			yours=0;
			oppos=0;
			printf("\nYour Score: %d", yours);
			printf("\nOpponent's Score: %d",oppos);
			ingame = 1;
			send(consocket, buffer, strlen(buffer), 0);
		}
		else
		{
			yours=0;
			oppos=0;
			send(consocket, buffer, strlen(buffer), 0);
			printf("\nYou declined...\n");
			ingame = 0; 
		} 
		
		/* Play game until one of the player declines. */
		turn = 0;		
		int playerID = 2;
		while(ingame){
			playgame(consocket, buffer, playerID); 
		}
		/* After playing game, recreate thread to start communicating with server
		 * and keep socket open for connections from other peers.
		 */
		yours=0;
		oppos=0;
		opponent[0] = '\0';
		pthread_create(&tid, NULL, serverthread, (void*) &serversocket); 
		consocket = accept(mysocket, (struct sockaddr *)&peer, &socksize);  
	}

	/* Close all sockets before the program exits */
	close(mysocket);
	close(consocket);
	close(peersocket);
	close(serversocket);
			
	return EXIT_SUCCESS;   
}

/* Thread function to handle communication with the server */
void * serverthread(void *obj)
{
	bool checkflag=false;
	/* Set cancel state to enable so we can cancel this thread from outside. */
	pthread_setcancelstate(PTHREAD_CANCEL_ENABLE, NULL);

	/* Buffer to hold data exchanged */
	char buffer[MAXRCVLEN + 1]; 
	int len;
	
	/* Print use instructions */
	printf("\nUsage: \n\n join {name}\n list\n invite {player}\n leave\n\n");

	/* Runs while loop until user enters the leave command or the thread is cancelled.*/
	while(1)
	{
		/* Read input from stream and split into two arguements. */
		fgets(buffer, sizeof(buffer), stdin);
		buffer[strlen(buffer)-1] = '\0';		
		char arg1[STRLEN], arg2[STRLEN];
		int n = sscanf(buffer, "%s %s", arg1, arg2);
		
		/* Handle request to retrieve and print the list of players when user enters 'list'. */
		if(strcmp(buffer, "list") == 0)
		{
			send(serversocket, buffer, strlen(buffer), 0);
			len = recv(serversocket, buffer, MAXRCVLEN, 0);
			buffer[len] = '\0';
			printf("\n%s\n", buffer);
		}
		/* Handle request to join the player's list in the server when user enters 'join'. */
		else if((strcmp(arg1, "join") == 0) && n > 1)
		{
			if(name == NULL || strlen(name) < 1) /* Internal check to see whether user is already joined. */
			{
				send(serversocket, buffer, strlen(buffer), 0);
				len = recv(serversocket, buffer, MAXRCVLEN, 0);
				buffer[len] = '\0';
				printf("\n%s\n", buffer);
				strcpy(name, arg2);	/* Store this name */
			}
			else printf("\nYou are already joined by the following name: %s\n", name);
			
		}
		/* Handle request to invite a player to play game when user enters 'invite {playername}. */
		else if(strcmp(arg1, "invite") == 0 && n > 1)
		{
			if(name == NULL || strlen(name) < 1)
			{
				printf("\nYou must join before inviting...\n");
			}
			else
			{
				/* Retrieves IP address of the player and open a connection in a new thread.
				 */ 
				strcpy(opponent, arg2);
				send(serversocket, buffer, strlen(buffer), 0);
				len = recv(serversocket, buffer, MAXRCVLEN, 0);
				buffer[len] = '\0';
				pthread_create(&tid, NULL, (void*)peerthread, buffer); 
				pthread_exit(0); /* Exit this thread. */
			}
			
		}
		/* Handle request when user enter 'leave'. Closes all sockets and exits the program. */
		else if(strcmp(buffer, "leave") == 0)
		{
			name[0] = '\0';
			printf("\nGoodbye!\n");
			close(mysocket);
			close(consocket);
			close(peersocket);
			close(serversocket);
			exit(1);			
		}
		/* For all other inputs, spam usage instructions. */
		else{
			if(!checkflag){
				checkflag=true;
			}else
				printf("\nUsage: \n\n join {name}\n list\n invite {player}\n leave\n\n");
		}		
	}
}

/* Thread function used when we want to open a connection with a peer */
void * peerthread(char * ip)
{
	/* Buffer to hold data exchange. */
	char buffer[MAXRCVLEN + 1]; 
	int len;	

	/* Open connection with the IP address passed as arguement. */
	struct sockaddr_in dest;
	peersocket = socket(AF_INET, SOCK_STREAM, 0);	
	memset(&dest, 0, sizeof(dest));               
	dest.sin_family = AF_INET;
	dest.sin_addr.s_addr = htonl(inet_network(ip)); 
	dest.sin_port = htons(PEERPORTNUM); 
	printf("\nSending invite...\n");	
	connect(peersocket, (struct sockaddr *)&dest, sizeof(struct sockaddr));

	/* Send your name. */
	send(peersocket, name, strlen(name), 0);

	/* Receive acknowledgement from peer. */
	len = recv(peersocket, buffer, MAXRCVLEN, 0);
	buffer[len] = '\0';
	if (strcmp(buffer, "y") == 0)
	{
		yours=0;
		oppos=0;
		printf("\nYour Score: %d", yours);
		printf("\nOpponent's Score: %d",oppos);
		ingame = 1;
		
	}
	else
	{	
		yours=0;
		oppos=0;
		ingame = 0;
		printf("\n%s declined...\n", opponent);
	} 

	/* Play game until one of the player declines. */
	turn = 0;	
	int playerID = 1;
	while(ingame) playgame(peersocket, buffer, playerID);

	/* After game end, close the connection, recreate thread to start communication
	 * with the server and close this thread. 
	 */
	yours=0;
	oppos=0;
	opponent[0] = '\0';
	close(peersocket);
	pthread_create(&tid, NULL, serverthread, (void*) &serversocket);  
	pthread_exit(0);	
}

void playgame(int socket, char * buffer, int playerID)
{
	int len, datasocket = socket;

	printf("\nSTARTING GAME\n");

	int i = 0;                                   /* Loop counter                         */
	int player = 0;                              /* Player number - 1 or 2               */  
	int go = 0;                                  /* Square selection number for turn     */
	int row = 0;                                 /* Row index for a square               */  
	int column = 0;                              /* Column index for a square            */
	int line = 0;                                /* Row or column index in checking loop */
	int winner = 0;                              /* The winning player                   */
	char board[3][3] = {                         /* The board                            */
					   {'1','2','3'},          /* Initial values are reference numbers */
					   {'4','5','6'},          /* used to select a vacant square for   */
					   {'7','8','9'}           /* a turn.                              */
					 };
				 

	/* The main game loop. The game continues for up to 9 turns */
	/* As long as there is no winner                            */
	for( i = (0 + turn); i<(9 + turn) && winner==0; i++)
	{
		/* Display the board */
		printf("\n\n");
		printf(" %c | %c | %c\n", board[0][0], board[0][1], board[0][2]);
		printf("---+---+---\n");
		printf(" %c | %c | %c\n", board[1][0], board[1][1], board[1][2]);
		printf("---+---+---\n");
		printf(" %c | %c | %c\n", board[2][0], board[2][1], board[2][2]);
	  
		player = i%2 + 1;                           /* Select player */

		/* Take appropriate actions based on turn. */
		do
		{
			if (player == playerID)
			{
				printf("\n%s, please enter the number of the square "
				"where you want to place your %c: ", name,(player==1)?'X':'O');
				scanf("%d", &go);
				send(datasocket, &go, sizeof(go), 0); /* Send your selection */
			}
			else 
			{
				printf("\nWaiting for %s...\n", opponent);
				len = recv(datasocket, &go, MAXRCVLEN, 0); /* Receive peer's selection */
				printf("%s chose %d\n", opponent, go);
			}

			row = --go/3;                                 /* Get row index of square      */
			column = go%3;                                /* Get column index of square   */
		} while(go<0 || go>9 || board[row][column]>'9');

		board[row][column] = (player == 1) ? 'X' : 'O';        /* Insert player symbol   */

		/* Check for a winning line - diagonals first */     
		if((board[0][0] == board[1][1] && board[0][0] == board[2][2]) ||
		 (board[0][2] == board[1][1] && board[0][2] == board[2][0]))
			winner = player;
		else
		/* Check rows and columns for a winning line */
		for(line = 0; line <= 2; line ++)
			if((board[line][0] == board[line][1] && board[line][0] == board[line][2])||
			 (board[0][line] == board[1][line] && board[0][line] == board[2][line]))
				winner = player;

	}
	/* Game is over so display the final board */
	printf("\n\n");
	printf(" %c | %c | %c\n", board[0][0], board[0][1], board[0][2]);
	printf("---+---+---\n");
	printf(" %c | %c | %c\n", board[1][0], board[1][1], board[1][2]);
	printf("---+---+---\n");
	printf(" %c | %c | %c\n", board[2][0], board[2][1], board[2][2]);

	/* Display result message */
	if(winner == 0)
		printf("\nHow boring, it is a draw.\n");
	else if (winner == playerID){
		printf("\nCongratulations %s, YOU ARE THE WINNER!\n", name);
		yours++;
	}
	else{
		oppos++;
		printf("\n%s wins this round...\n", opponent);
	}
	printf("\nYour Score: %d", yours);
	printf("\nOpponent's Score: %d",oppos);
	/* Switch first turn */
	if (turn == 0 ) turn++;
	else turn--;

	/* Ask to play another round */
	printf("\nPlay another round? (y/n) ");
	fgetc(stdin);
	fgets(buffer, sizeof buffer, stdin);
	buffer[strlen(buffer)-1] = '\0';
	printf("\nWating for %s to acknowledge...\n", opponent);

	/* Set while loop flag to true if both players agree to play again otherwise set it to false */
	ingame=0;
	if (strcmp(buffer, "y") == 0) ingame = 1;
	send(datasocket, buffer, strlen(buffer), 0);
	len = recv(datasocket, buffer, MAXRCVLEN, 0);
	buffer[len] = '\0';
	if (strcmp(buffer, "y") != 0 && ingame==1)
	{
		printf("\n%s has declined...\n", opponent);
		ingame = 0;
		yours=0;
		oppos=0;
	}
}

