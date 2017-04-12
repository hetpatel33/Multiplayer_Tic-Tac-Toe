# Multiplayer Tic-Tac-Toe

A multiplayer Tic-Tac-Toe game using C socket programming in which a Server creates the game in Local Area Network. The players in LAN can connect to the server using IP address of server provided. The connection to the server will be a client-server approach whereas the players will be playing with each other in a Peer-to-Peer fashion.


# Server Side (server.c) :
Server listens for incoming connections from players.When a connection arrives, new thread will be created. To handle this connection maintaining the concurrency of the server, each connection is opened in a new socket. New socket descriptor is created which will be passed to our thread. When players are connected, they can query to join, leave, list and challenge other players. Program store a list of all players that have joined which is protected by a mutex to prevent race conditions from taking place between threads.

# Client Side (client.c)  :
Client/Peer program opens a connection with the server program on launch. Creates a new thread to handle communication with the server. Binds to a port and starts listening for incoming connections. Player can query the server to join, leave, list or challenge other players. If ‘invite {playername}’ query is entered, then the server returns the IP of this player from the hashmap. The client program closes the thread with the server but maintains the connection and uses the supplied IP address to open a direct connection with the peer associated with this IP in a new thread. This connection is accepted on the port that invited player is listening to. On the other side, the invited player can refuse or accept to play a game. The players can keep playing until one of them declines.When the game finishes, the inviting player closes his thread and socket. It then recreates the thread to start querying the server again.The invited player on the other hand goes back to listening for other connections and recreates its server thread as well.
 
# Hash Table (strmap)  :
ANSI C hash table for strings is used to save the list of active clients connected to the server. For this strmap which is an open source is used with some modifications as needed.

