using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;
using System.Drawing.Printing;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace WindowsFormsApplication1
{
  
    public partial class Viewpage : Form
    {
        OleDbConnection conn;
        int key=1;     
        int id=1;
        double d;
        int credit_No = 1;
        string update_key,update_No,update_credit_No, bpCharge, bwCharge,bpCharge1,bwCharge1;
        int indexRow;
        public Viewpage()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            panel1.Visible = false;
            panel2.Visible = false;
            panel3.Visible = false;
            resultBox.Visible = true;
            conn = new OleDbConnection("Provider=Microsoft.Jet.OleDb.4.0;"
                     + "Data Source=" + Application.StartupPath + "\\PERFECT DATABASE.mdb");
            conn.Open();
            clear_all();
            using (DataTable dt = new DataTable())
            {
                using (OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM Customer_Details order by [Customer Name] ASC", conn))
                {
                    adapter.Fill(dt);
                }
                dataGridView1.DataSource = dt;
               
            }
            
            for (int i = 0; i < dataGridView1.RowCount - 1; i++)
            {
                if (key <= int.Parse(dataGridView1.Rows[i].Cells[5].Value.ToString()))
                    key = int.Parse(dataGridView1.Rows[i].Cells[5].Value.ToString())+1;
            }
            using (DataTable dt = new DataTable())
            {
                using (OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM Sub_Entry", conn))
                {

                    adapter.Fill(dt);
                }
             
                dataGridView2.DataSource = dt;
            }
            for (int i = 0; i < dataGridView2.RowCount - 1; i++)
            {
                if (id <= int.Parse(dataGridView2.Rows[i].Cells[12].Value.ToString()))
                    id = int.Parse(dataGridView2.Rows[i].Cells[12].Value.ToString()) + 1;
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.A) && panel1.Visible==false)
            {
                button2.PerformClick();
                return true;
            }
            if (keyData == (Keys.Control | Keys.A) && panel1.Visible == true && panel2.Visible == false)
            {
                button11.PerformClick();
                return true;
            }
            if (keyData == (Keys.Control | Keys.A) &&  panel2.Visible == true && panel3.Visible==false)
            {
                button15.PerformClick();
                return true;
            }
            if (keyData == (Keys.Control | Keys.A) && panel3.Visible == true)
            {
                add_panel3.PerformClick();
                return true;
            }
            if (keyData == (Keys.Control | Keys.C) && panel2.Visible == true && panel3.Visible == false)
            {
                button14.PerformClick();
                return true;
            }
            if (keyData == (Keys.Control | Keys.S) && panel2.Visible == true && panel3.Visible == false)
            {
                save_butt.PerformClick();
                return true;
            }
            if (keyData == (Keys.Control | Keys.U) && panel1.Visible == false)
            {
                button1.PerformClick();
                return true;
            }
            if (keyData == (Keys.Control | Keys.U) && panel3.Visible == true)
            {
                update_panel3.PerformClick();
                return true;
            }
             if (keyData == (Keys.Escape) && panel1.Visible == true && panel2.Visible == false)
            {
                button7.PerformClick();
                return true;
            }
            if (keyData == (Keys.Escape) && panel3.Visible == true)
            {
                back_Panel3.PerformClick();
                return true;
            }
            if (keyData == (Keys.Escape) && panel2.Visible == true && panel3.Visible == false)
            {
                button13.PerformClick();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
       
        private void customer_DetailsBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {   
            this.Validate();
            this.customer_DetailsBindingSource.EndEdit();
            tableAdapterManager.UpdateAll(pERFECT_DATABASEDataSet);

        }

        private void Viewpage_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'pERFECT_DATABASEDataSet1.Sub_Entry' table. You can move, or remove it, as needed.
            this.sub_EntryTableAdapter1.Fill(this.pERFECT_DATABASEDataSet1.Sub_Entry);
            // TODO: This line of code loads data into the 'pERFECT_DATABASEDataSet.debit_details' table. You can move, or remove it, as needed.
            this.debit_detailsTableAdapter.Fill(this.pERFECT_DATABASEDataSet.debit_details);
            // TODO: This line of code loads data into the 'pERFECT_DATABASEDataSet.Sub_Entry' table. You can move, or remove it, as needed.
            this.sub_EntryTableAdapter.Fill(this.pERFECT_DATABASEDataSet.Sub_Entry);
            // TODO: This line of code loads data into the 'pERFECT_DATABASEDataSet.Customer_Details' table. You can move, or remove it, as needed.
            this.customer_DetailsTableAdapter.Fill(this.pERFECT_DATABASEDataSet.Customer_Details);
            /* DataGridViewRow row = dataGridView1.Rows[dataGridView1.RowCount - 1];
             if (dataGridView1.RowCount > 1)
                 key = int.Parse(row.Cells[5].Value.ToString());
             else
                 key = 1;
             clear_all();
           */
            clear_all();
        }


       
        private void customer_DetailsDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           

        }
       
        private void button2_Click(object sender, EventArgs e)
        {
            if (customer_NameTextBox.Text != "")
            {   if (adderessTextBox.Text == "")
                    adderessTextBox.Text = " ";
                if (mobile_NumberTextBox.Text == "")
                    mobile_NumberTextBox.Text = "0";
                if (b_P_ChargeTextBox.Text == "")
                    b_P_ChargeTextBox.Text = "0";
                if (b_W_ChargeTextBox.Text == "")
                    b_W_ChargeTextBox.Text = "0";
                OleDbCommand cmd = new OleDbCommand("INSERT INTO Customer_Details VALUES(@sitid1, @incident1, @nature1, @name1, @charges1, @charges2)", conn);
                cmd.Parameters.AddWithValue("@sitid1", customer_NameTextBox.Text);
                cmd.Parameters.AddWithValue("@incident1", adderessTextBox.Text);
                cmd.Parameters.AddWithValue("@nature1", mobile_NumberTextBox.Text);
                cmd.Parameters.AddWithValue("@name1", b_P_ChargeTextBox.Text);
                cmd.Parameters.AddWithValue("@charges1", b_W_ChargeTextBox.Text);
                cmd.Parameters.AddWithValue("@charges2", key);

                cmd.ExecuteNonQuery();
                using (DataTable dt = new DataTable())
                {
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM Customer_Details order by [Customer Name] ASC", conn))
                    {
                        adapter.Fill(dt);
                    }
                    dataGridView1.DataSource = dt;
                }
                key = key + 1;
                clear_all();

            }
            
        }
        private void clear_all()
        {
            customer_NameTextBox.Text = "";
            adderessTextBox.Text = "";
            mobile_NumberTextBox.Text = "";
            b_P_ChargeTextBox.Text = "";
            b_W_ChargeTextBox.Text = "";
            textBox12.Text = "";
            textBox11.Text = "";
            textBox10.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox8.Text = "";
            textBox7.Text = "";
            textBox6.Text = "";
            textBox9.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";

        }
        private void Viewpage_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

      

        private void b_P_ChargeTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void b_P_ChargeLabel_Click(object sender, EventArgs e)
        {

        }

        private void b_W_ChargeTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void mobile_NumberTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void adderessLabel_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            indexRow = e.RowIndex; // get the selected Row Index
            if (e.RowIndex >= 0 && e.RowIndex < dataGridView1.RowCount - 1)
            {
                DataGridViewRow row = dataGridView1.Rows[indexRow];
            
                customer_NameTextBox.Text = row.Cells[0].Value.ToString();
                adderessTextBox.Text = row.Cells[1].Value.ToString();
                mobile_NumberTextBox.Text = row.Cells[2].Value.ToString();
                b_P_ChargeTextBox.Text = row.Cells[3].Value.ToString();
                b_W_ChargeTextBox.Text = row.Cells[4].Value.ToString();
                update_key = row.Cells[5].Value.ToString();
            }
        }
       
       

        private void button1_Click(object sender, EventArgs e)
        {
            if (customer_NameTextBox.Text != "")
            {
                if (adderessTextBox.Text == "")
                    adderessTextBox.Text = " ";
                if (mobile_NumberTextBox.Text == "")
                    mobile_NumberTextBox.Text = "0";
                if (b_P_ChargeTextBox.Text == "")
                    b_P_ChargeTextBox.Text = "0";
                if (b_W_ChargeTextBox.Text == "")
                    b_W_ChargeTextBox.Text = "0";
                OleDbCommand cmd = new OleDbCommand("UPDATE Customer_Details SET [Customer Name]=@sitid1, [Adderess]=@incident1, [Mobile Number]=@nature1, [B/P Charge]=@name1,[B/W Charge]= @charges1 WHERE KEY=@charges2", conn);
                cmd.Parameters.AddWithValue("@sitid1", customer_NameTextBox.Text);
                cmd.Parameters.AddWithValue("@incident1", adderessTextBox.Text);
                cmd.Parameters.AddWithValue("@nature1", mobile_NumberTextBox.Text);
                cmd.Parameters.AddWithValue("@name1", b_P_ChargeTextBox.Text);
                cmd.Parameters.AddWithValue("@charges1", b_W_ChargeTextBox.Text);
                cmd.Parameters.AddWithValue("@charges2", update_key);

                cmd.ExecuteNonQuery();
                using (DataTable dt = new DataTable())
                {
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM Customer_Details order by [Customer Name] ASC", conn))
                    {
                        adapter.Fill(dt);
                    }
                    dataGridView1.DataSource = dt;
                }

                clear_all();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Are you sure to delete?");
            OleDbCommand cmd = new OleDbCommand("delete from Customer_Details where Key=@charges2 ", conn);
            cmd.Parameters.AddWithValue("@charges2", update_key);
            cmd.ExecuteNonQuery();

            OleDbCommand cmd1 = new OleDbCommand("delete from Sub_Entry where Key=@charges2 ", conn);
            cmd1.Parameters.AddWithValue("@charges2",int.Parse(update_key));
            cmd1.ExecuteNonQuery();
            using (DataTable dt = new DataTable())
            {
                using (OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM Customer_Details order by [Customer Name] ASC", conn))
                {
                    adapter.Fill(dt);
                }
                dataGridView1.DataSource = dt;
            }
           
            clear_all();
        }

        private void menuStrip1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            indexRow = e.RowIndex; // get the selected Row Index
            if (e.RowIndex >= 0 && e.RowIndex < dataGridView1.RowCount - 1)
            {
                DataGridViewRow row = dataGridView1.Rows[indexRow];
                update_key = row.Cells[5].Value.ToString();
                bpCharge = row.Cells[3].Value.ToString();
                bwCharge = row.Cells[4].Value.ToString();
                name_label.Text = row.Cells[0].Value.ToString();
                int uk = int.Parse(update_key);
                panel1.Visible = true;
                panel2.Visible = false;
                using (DataTable dt = new DataTable())
                {
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM Sub_Entry WHERE Key='" + uk + "' order by Date desc", conn))
                    {

                        adapter.Fill(dt);
                    }
                    //     DataTable tempDT = new DataTable();
                    //     tempDT = dt.DefaultView.ToTable(true,"No","Date","FIle Name","Drawing Work","Curior Charge","Blue Print","Black And White Print","Length","Width","Size","BP Charge","BW Charge","Total","Key");
                    dataGridView2.DataSource = dt;
                }
                using (DataTable dt = new DataTable())
                {
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM debit_details WHERE Key='" + uk + "' order by Date desc", conn))
                    {

                        adapter.Fill(dt);
                    }
                    DataTable tempDT = new DataTable();
                    tempDT = dt.DefaultView.ToTable(true, "Rupees", "Receipt No");
                    dataGridView3.DataSource = dt;
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            panel2.Visible = false;
            totalTextbox.Text = "";
            creditTextbox.Text = "";
            resultBox.Text = "";
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

       
      

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button11_Click(object sender, EventArgs e)
        {
            bpCharge1 = bpCharge;
            bwCharge1 = bwCharge;
            panel2.Visible = true;
            panel3.Visible = false;
            resultBox.Visible = false;
            bpc_textBox.Text = bpCharge;
            bwc_Textbox.Text = bwCharge;
        }
        private void button14_Click(object sender, EventArgs e)
        {
            if (textBox11.Text == "")
                textBox11.Text = "0";
            if (textBox6.Text == "")
                textBox6.Text = "0";
            if (textBox8.Text == "")
                textBox8.Text = "0";
            if (textBox7.Text == "")
                textBox7.Text = "0";
            if (textBox10.Text == "")
                textBox10.Text = "0";
            if (textBox4.Text == "")
                textBox4.Text = "0";
            if (textBox5.Text == "")
                textBox5.Text = "0";
            if (textBox9.Text == "")
                textBox9.Text = "0";
            double dw,cc,bw,bp;
            double bwc = double.Parse(bwc_Textbox.Text);
            double bpc = double.Parse(bpc_textBox.Text);
            if(double.TryParse(textBox11.Text, out dw) && double.TryParse(textBox6.Text, out cc) && double.TryParse(textBox7.Text, out bw) && double.TryParse(textBox8.Text, out bp) ){
                double total;
                total = (d * bw * bwc) + (d * bp * bpc)+ dw + cc;
                textBox9.Text = total.ToString("0.######");
               
            }
            bpc_textBox.Enabled = false;
            bwc_Textbox.Enabled = false;
        }

        private void button15_Click(object sender, EventArgs e)
        {
           
            if (textBox12.Text != "")
            {
                if (textBox11.Text == "")
                    textBox11.Text = "0";
                if (textBox6.Text == "")
                    textBox6.Text = "0";
                if (textBox8.Text == "")
                    textBox8.Text = "0";
                if (textBox7.Text == "")
                    textBox7.Text = "0";
                if (textBox10.Text == "")
                    textBox10.Text = "0";
                if (textBox4.Text == "")
                    textBox4.Text = "0";
                if (textBox5.Text == "")
                    textBox5.Text = "0";
                if (textBox9.Text == "")
                    textBox9.Text = "0";
                bpCharge = bpc_textBox.Text;
                bwCharge = bwc_Textbox.Text;
                OleDbCommand cmd = new OleDbCommand("INSERT INTO Sub_Entry VALUES(@charges,@charges2,@sitid1, @incident1, @nature1, @name1, @charges1, @charges3,@charges4,@charges5,@charges6,@charges7,@charges8,@charges9,@charges0)", conn);
                DateTime result = dateTimePicker1.Value;
                cmd.Parameters.AddWithValue("@charges", 0);
                cmd.Parameters.AddWithValue("@charges2", id);
                cmd.Parameters.AddWithValue("@sitid1", result.Date.ToString());
                cmd.Parameters.AddWithValue("@incident1", textBox12.Text);
                cmd.Parameters.AddWithValue("@nature1", textBox11.Text);
                cmd.Parameters.AddWithValue("@name1", textBox6.Text);
                cmd.Parameters.AddWithValue("@charges1", textBox8.Text);
                cmd.Parameters.AddWithValue("@charges3", textBox7.Text);
                cmd.Parameters.AddWithValue("@charges4", textBox10.Text);
                cmd.Parameters.AddWithValue("@charges5", textBox4.Text);
                cmd.Parameters.AddWithValue("@charges6", textBox5.Text);
                cmd.Parameters.AddWithValue("@charges7", bpCharge);
                cmd.Parameters.AddWithValue("@charges8", bwCharge);
                double t = double.Parse(textBox9.Text);
                cmd.Parameters.AddWithValue("@charges9", t.ToString("0.00"));
                cmd.Parameters.AddWithValue("@charges0", int.Parse(update_key));
                cmd.ExecuteNonQuery();
                int uk = int.Parse(update_key);
                using (DataTable dt = new DataTable())
                {
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM Sub_Entry WHERE Key='" + uk + "' order by Date desc", conn))
                    {
                        adapter.Fill(dt);
                    }
                 //   DataTable tempDT = new DataTable();
                  //  tempDT = dt.DefaultView.ToTable(true, "No", "Date", "FIle Name", "Drawing Work", "Curior Charge", "Blue Print", "Black And White Print", "Length", "Width", "Size", "BP Charge", "BW Charge", "Total", "Key");
                    dataGridView2.DataSource = dt;
                }
                id = id + 1;
                clear_all();
                bpCharge = bpCharge1;
                bwCharge = bwCharge1;
                bpc_textBox.Text = bpCharge;
                bwc_Textbox.Text = bwCharge;
                bpc_textBox.Enabled = false;
                bwc_Textbox.Enabled = false;
            }
        }
    
        private void panel2_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void DeletePanel1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Are you sure to delete?");
            OleDbCommand cmd1 = new OleDbCommand("delete from Sub_Entry where ID=@charges2 ", conn);
            cmd1.Parameters.AddWithValue("@charges2", update_No);
            cmd1.ExecuteNonQuery();
            int uk = int.Parse(update_key);
            using (DataTable dt = new DataTable())
            {
                using (OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM Sub_Entry WHERE Key='" + uk + "' order by Date desc", conn))
                {
                    adapter.Fill(dt);
                }
                dataGridView2.DataSource = dt;
            }
            clear_all();
        }
        double sum;
        private void Total_Click(object sender, EventArgs e)
        {
            sum = 0;
            for (int i = 0; i < dataGridView2.Rows.Count-1; i++)
            {
                //sum += Convert.ToInt32(dataGridView2.Rows[i].Cells[13].Value);
                sum += double.Parse(dataGridView2.Rows[i].Cells[11].Value.ToString());
            }
         
            totalTextbox.Text = sum.ToString("0.00");
            int sum1 = 0;
            for (int i = 0; i < dataGridView3.Rows.Count - 1; i++)
            {
                sum1 += Convert.ToInt32(dataGridView3.Rows[i].Cells[1].Value);
            }
            if(sum1>0)
            creditTextbox.Text = sum1.ToString("0.00");
            resultBox.Text = (sum - sum1).ToString("0.00");
        }

        private void Credit_button_Click(object sender, EventArgs e)
        {
           int uk = int.Parse(update_key);
            panel1.Visible = true;
            panel2.Visible = true;
            panel3.Visible = true;
            panel4.Visible = false;
            resultBox.Visible = false;
            using (DataTable dt = new DataTable())
            {
                using (OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM debit_details WHERE Key='" + uk + "' order by Date desc", conn))
                {

                    adapter.Fill(dt);
                }
                DataTable tempDT = new DataTable();
                tempDT = dt.DefaultView.ToTable(true, "Date", "Rupees", "Receipt No", "Key");
                dataGridView3.DataSource = tempDT;
            }

        }

        private void edit_panel1_Click(object sender, EventArgs e)
        {

        }

        private void button13_Click(object sender, EventArgs e)
        {
            panel2.Visible = false;
            panel1.Visible = true;
            resultBox.Visible = true;
            clear_all();
            save_butt.Visible = false;
        }
       
        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dataGridView2.RowCount - 1)
            {
                bpc_textBox.Text = bpCharge;
                bwc_Textbox.Text = bwCharge;
                save_butt.Visible = true;
                panel2.Visible = true;
                panel3.Visible = false;
                resultBox.Visible = false;
                indexRow = e.RowIndex; // get the selected Row Index
                DataGridViewRow row = dataGridView2.Rows[indexRow];
                dateTimePicker1.Value = DateTime.Parse(row.Cells[0].Value.ToString());
                textBox12.Text = row.Cells[1].Value.ToString();
                textBox11.Text = row.Cells[2].Value.ToString();
                textBox6.Text = row.Cells[3].Value.ToString();
                textBox8.Text = row.Cells[4].Value.ToString();
                textBox7.Text = row.Cells[5].Value.ToString();
                textBox10.Text = row.Cells[6].Value.ToString();
                textBox4.Text = row.Cells[7].Value.ToString();
                textBox5.Text = row.Cells[8].Value.ToString();
                textBox9.Text = row.Cells[11].Value.ToString();
                update_No = row.Cells[12].Value.ToString();
            }
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            indexRow = e.RowIndex; // get the selected Row Index
            if (e.RowIndex >= 0 && e.RowIndex < dataGridView2.RowCount - 1)
            {
                DataGridViewRow row = dataGridView2.Rows[indexRow];
            
                update_No = row.Cells[12].Value.ToString();
            }
        }

        private void panel2_Paint_2(object sender, PaintEventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }
        int num1,num2;
        private void textBox10_TextChanged(object sender, EventArgs e)
        {   
            
            if (int.TryParse(textBox10.Text, out num1) && int.TryParse(textBox4.Text, out num2))
            {   if(num1>0 & num2>0 )
                multiply();
            }
            else if (textBox4.Text == "")
            {

            }
            else
            {
               // MessageBox.Show("Invalid Input");
            }


        }
        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            
            if (int.TryParse(textBox10.Text, out num1) && int.TryParse(textBox4.Text, out num2))
            {
                if (num1 > 0 & num2 > 0)
                    multiply();
            }
            else if(textBox4.Text=="")
            {
                
            }else
            {
               // MessageBox.Show("Invalid Input");
            }

        }
        private void multiply()
        {
          
            double a1 = num2;
            double a2 = num1;
            d = (a1 * a2)/144;

            textBox5.Text = d.ToString("0.########");
        }

       

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

       

        private void back_Panel3_Click_1(object sender, EventArgs e)
        {
            panel3.Visible = false;
            panel2.Visible = false;
            resultBox.Visible = true;
            totalTextbox.Text = "";
            creditTextbox.Text = "";
            resultBox.Text = "";
        }

        
       
        private void panel3_Paint_1(object sender, PaintEventArgs e)
        {
            clear_all();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1 f1 = new Form1();
            this.Hide();
            f1.Show();
        }

        private void viewToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            indexRow = e.RowIndex; // get the selected Row Index
            if (e.RowIndex >= 0 && e.RowIndex < dataGridView1.RowCount)
            {
                DataGridViewRow row = dataGridView1.Rows[indexRow];
                customer_NameTextBox.Text = row.Cells[0].Value.ToString();
                adderessTextBox.Text = row.Cells[1].Value.ToString();
                mobile_NumberTextBox.Text = row.Cells[2].Value.ToString();
                b_P_ChargeTextBox.Text = row.Cells[3].Value.ToString();
                b_W_ChargeTextBox.Text = row.Cells[4].Value.ToString();
                update_key = row.Cells[5].Value.ToString();
            }
        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

       

        private void dataGridView3_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void change_button_Click(object sender, EventArgs e)
        {   
            bpc_textBox.Enabled = true;
            bwc_Textbox.Enabled = true;
            bpCharge1 = bpCharge;
            bwCharge1 = bwCharge;
            bpCharge = bpc_textBox.Text;
            bwCharge = bwc_Textbox.Text;
        }

        private void bwc_Textbox_TextChanged(object sender, EventArgs e)
        {

        }

        private void update_panel3_Click(object sender, EventArgs e)
        {
            DateTime result = dateTimePicker2.Value;
            OleDbCommand cmd = new OleDbCommand("UPDATE debit_details SET [Date]=@sitid1, [Rupees]=@incident1, [Receipt No]=@nature1 WHERE [Receipt No]=@charges", conn);
            cmd.Parameters.AddWithValue("@sitid1", result.ToString());
            cmd.Parameters.AddWithValue("@incident1",textBox2.Text);
            cmd.Parameters.AddWithValue("@nature1", textBox3.Text);
            cmd.Parameters.AddWithValue("@charges", update_credit_No);

            cmd.ExecuteNonQuery();
            int uk = int.Parse(update_key);
            using (DataTable dt = new DataTable())
            {
                using (OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM debit_details WHERE Key='" + uk + "' order by Date desc", conn))
                {

                    adapter.Fill(dt);
                }
                DataTable tempDT = new DataTable();
                tempDT = dt.DefaultView.ToTable(true, "Date", "Rupees", "Receipt No", "Key");
                dataGridView3.DataSource = tempDT;
            }

            clear_all();

        }

        private void delete_Panel3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Are you sure to delete?");
            OleDbCommand cmd1 = new OleDbCommand("delete from debit_details where [Receipt No]=@charges2 ", conn);
            cmd1.Parameters.AddWithValue("@charges2", textBox3.Text);
            cmd1.ExecuteNonQuery();
            int uk = int.Parse(update_key);
            using (DataTable dt = new DataTable())
            {
                using (OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM debit_details WHERE Key='" + uk + "' order by Date desc", conn))
                {

                    adapter.Fill(dt);
                }
                DataTable tempDT = new DataTable();
                tempDT = dt.DefaultView.ToTable(true, "Date", "Rupees", "Receipt No", "Key");
                dataGridView3.DataSource = tempDT;
            }

            clear_all();
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int uk = int.Parse(update_key);
            indexRow = e.RowIndex; // get the selected Row Index
            if (e.RowIndex >= 0 && e.RowIndex < dataGridView3.RowCount - 1)
            {
                DataGridViewRow row = dataGridView3.Rows[indexRow];
                dateTimePicker2.Value = DateTime.Parse(row.Cells[0].Value.ToString());
                textBox2.Text = row.Cells[1].Value.ToString();
                textBox3.Text = row.Cells[2].Value.ToString();
                update_credit_No = textBox3.Text;
                using (DataTable dt = new DataTable())
                {
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM debit_details WHERE Key='" + uk + "' order by Date desc", conn))
                    {

                        adapter.Fill(dt);
                    }
                    DataTable tempDT = new DataTable();
                    tempDT = dt.DefaultView.ToTable(true, "Date", "Rupees", "Receipt No", "Key");
                    dataGridView3.DataSource = tempDT;
                }
            }
        }

        private void developerToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void dataGridView2_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int uk = int.Parse(update_key);
            using (DataTable dt = new DataTable())
            {
                using (OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM Sub_Entry WHERE Key='" + uk + "' order by Date desc", conn))
                {
                    adapter.Fill(dt);
                }
                dataGridView2.DataSource = dt;
            }
        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void save_butt_Click(object sender, EventArgs e)
        {

            if (textBox12.Text != "")
            {
                if (textBox11.Text == "")
                    textBox11.Text = "0";
                if (textBox6.Text == "")
                    textBox6.Text = "0";
                if (textBox8.Text == "")
                    textBox8.Text = "0";
                if (textBox7.Text == "")
                    textBox7.Text = "0";
                if (textBox10.Text == "")
                    textBox10.Text = "0";
                if (textBox4.Text == "")
                    textBox4.Text = "0";
                if (textBox5.Text == "")
                    textBox5.Text = "0";
                if (textBox9.Text == "")
                    textBox9.Text = "0";
                panel3.Visible = false;
                panel2.Visible = false;
                panel1.Visible = true;
                resultBox.Visible = true;
                bpCharge = bpc_textBox.Text;
                bwCharge = bwc_Textbox.Text;
                int un = int.Parse(update_No);
                int uk = int.Parse(update_key);
                DateTime result = dateTimePicker1.Value;
                OleDbCommand cmd = new OleDbCommand("UPDATE Sub_Entry SET [Date]=@sitid1, [FIle Name]=@incident1, [Drawing Work]=@nature1, [Curior Charge]=@name1,[Blue Print]= @charges1,[Black And White Print]= @charges3,[Length]= @charges4,[Width]= @charges5,[Size]= @charges6,[BP Charge]= @charges7,[BW Charge]= @charges8,[Total]= @charges9 WHERE ID='" + un + "'", conn);
                cmd.Parameters.AddWithValue("@sitid1", result.ToString());
                cmd.Parameters.AddWithValue("@incident1", textBox12.Text);
                cmd.Parameters.AddWithValue("@nature1", textBox11.Text);
                cmd.Parameters.AddWithValue("@name1", textBox6.Text);
                cmd.Parameters.AddWithValue("@charges1", textBox8.Text);
                cmd.Parameters.AddWithValue("@charges3", textBox7.Text);
                cmd.Parameters.AddWithValue("@charges4", textBox10.Text);
                cmd.Parameters.AddWithValue("@charges5", textBox4.Text);
                cmd.Parameters.AddWithValue("@charges6", textBox5.Text);
                cmd.Parameters.AddWithValue("@charges7", bpCharge);
                cmd.Parameters.AddWithValue("@charges8", bwCharge);
                cmd.Parameters.AddWithValue("@charges9", double.Parse(textBox9.Text).ToString("0.00"));
                cmd.Parameters.AddWithValue("@charges10", int.Parse(update_No));
                cmd.ExecuteNonQuery();

                using (DataTable dt = new DataTable())
                {
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM Sub_Entry WHERE Key='" + uk + "' order by Date desc", conn))
                    {
                        adapter.Fill(dt);
                    }
                    dataGridView2.DataSource = dt;
                }
                clear_all();
                bpCharge = bpCharge1;
                bwCharge = bwCharge1;
                bpc_textBox.Text = bpCharge;
                bwc_Textbox.Text = bwCharge;
                bpc_textBox.Enabled = false;
                bwc_Textbox.Enabled = false;
            }
            }

        private void back_Panel3_Click(object sender, EventArgs e)
        {
            panel3.Visible = false;
            panel2.Visible = false;
            resultBox.Visible = true;
        }

        private void dataGridView3_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
             Properties.Settings.Default.Save();
        }

        private void dataGridView3_CellClick_2(object sender, DataGridViewCellEventArgs e)
        {
            int uk = int.Parse(update_key);
            indexRow = e.RowIndex; // get the selected Row Index
            if (e.RowIndex >= 0 && e.RowIndex<dataGridView3.RowCount-1)
            {
                DataGridViewRow row = dataGridView3.Rows[indexRow];
                dateTimePicker2.Value = DateTime.Parse(row.Cells[0].Value.ToString());
                textBox2.Text = row.Cells[1].Value.ToString();
                textBox3.Text = row.Cells[2].Value.ToString();
                update_credit_No = textBox3.Text;
                using (DataTable dt = new DataTable())
                {
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM debit_details WHERE Key='" + uk + "' order by Date desc", conn))
                    {

                        adapter.Fill(dt);
                    }
                    DataTable tempDT = new DataTable();
                    tempDT = dt.DefaultView.ToTable(true, "Date", "Rupees", "Receipt No", "Key");
                    dataGridView3.DataSource = tempDT;
                }
            }
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void printPreviewDialog1_Load(object sender, EventArgs e)
        {

        }

        private void panel3_Paint_2(object sender, PaintEventArgs e)
        {
          //  clear_all();
        }

        private void dataGridView4_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView4_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
        }

        private void dataGridView4_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void add_panel3_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != "" && textBox3.Text != "")
            {

                OleDbCommand cmd = new OleDbCommand("INSERT INTO debit_details VALUES(@charges,@charges2,@sitid1, @incident1,@charges0)", conn);
                DateTime result = dateTimePicker2.Value;
                cmd.Parameters.AddWithValue("@charges", credit_No);
                cmd.Parameters.AddWithValue("@charges2", result.ToString());
                cmd.Parameters.AddWithValue("@sitid1", textBox2.Text);
                cmd.Parameters.AddWithValue("@incident1", textBox3.Text);
                cmd.Parameters.AddWithValue("@charges0", int.Parse(update_key));
                cmd.ExecuteNonQuery();
                int uk = int.Parse(update_key);
                using (DataTable dt = new DataTable())
                {
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM debit_details WHERE Key='" + uk + "' order by Date desc", conn))
                    {

                        adapter.Fill(dt);
                    }
                    DataTable tempDT = new DataTable();
                    tempDT = dt.DefaultView.ToTable(true, "Date", "Rupees", "Receipt No");
                    dataGridView3.DataSource = tempDT;
                }

                credit_No = credit_No + 1;
                clear_all();

            }
        }

        private void dataGridView3_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

        }

        private void dataGridView3_ColumnHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {

        }

        private void dataGridView3_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var grid = sender as DataGridView;
            var rowIdx = (e.RowIndex + 1).ToString();

            var centerFormat = new StringFormat()
            {
                // right alignment might actually make more sense for numbers
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            var headerBounds = new System.Drawing.Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height);
            e.Graphics.DrawString(rowIdx, this.Font, SystemBrushes.ControlText, headerBounds, centerFormat);
        }

        private void dataGridView2_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var grid = sender as DataGridView;
            var rowIdx = (e.RowIndex + 1).ToString();

            var centerFormat = new StringFormat()
            {
                // right alignment might actually make more sense for numbers
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            var headerBounds = new System.Drawing.Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height);
            e.Graphics.DrawString(rowIdx, this.Font, SystemBrushes.ControlText, headerBounds, centerFormat);
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var grid = sender as DataGridView;
            var rowIdx = (e.RowIndex + 1).ToString();

            var centerFormat = new StringFormat()
            {
                // right alignment might actually make more sense for numbers
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            var headerBounds = new System.Drawing.Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height);
            e.Graphics.DrawString(rowIdx, this.Font, SystemBrushes.ControlText, headerBounds, centerFormat);
        }

        private void label15_Click_1(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void label14_Click_1(object sender, EventArgs e)
        {

        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void fileToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void grandTotalToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void recordsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
        }

        private void grandTotalToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            using (DataTable dt = new DataTable())
            {
                using (OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM Sub_Entry", conn))
                {
                    adapter.Fill(dt);
                }
                dataGridView4.DataSource = dt;
            }
            using (DataTable dt = new DataTable())
            {
                using (OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM debit_details", conn))
                {

                    adapter.Fill(dt);
                }
                DataTable tempDT = new DataTable();
                tempDT = dt.DefaultView.ToTable(true, "Date", "Rupees", "Receipt No");
                dataGridView5.DataSource = tempDT;
            }
            sum = 0;
            for (int i = 0; i < dataGridView4.Rows.Count - 1; i++)
            {
                //sum += Convert.ToInt32(dataGridView2.Rows[i].Cells[13].Value);
                sum += double.Parse(dataGridView4.Rows[i].Cells[11].Value.ToString());
            }
            int sum1 = 0;
            for (int i = 0; i < dataGridView5.Rows.Count - 1; i++)
            {
                sum1 += Convert.ToInt32(dataGridView5.Rows[i].Cells[1].Value);
            }
            MessageBox.Show("Grand Total: "+ sum.ToString("0.00")+" - "+ sum1.ToString("0.00")+" = "+ (sum - sum1).ToString("0.00"));
        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel6_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

       

        private void panel6_Paint_2(object sender, PaintEventArgs e)
        {

        }

        private void dateTimePicker3_ValueChanged(object sender, EventArgs e)
        {

        }

       

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void grandtotal_box_TextChanged(object sender, EventArgs e)
        {
                    }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            this.Hide();
            f2.Show();
        }

        private void dateTimePicker3_ValueChanged_1(object sender, EventArgs e)
        {

        }

        private void dataGridView3_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView3_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView3_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

        }

        private void dataGridView3_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {

        }

        private void dataGridView5_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void print_Panel3_Click(object sender, EventArgs e)
        {
            dateTimePicker3.Visible = true;
            dateTimePicker4.Visible = true;
            Ok_button.Visible = true;
            panel4.Visible = true;
        }
        private void Ok_button_Click(object sender, EventArgs e)
        {   
            string dt_start = dateTimePicker3.Value.ToShortDateString();
            dateTimePicker4.Value = dateTimePicker4.Value.AddDays(1);
            string dt_end = dateTimePicker4.Value.ToShortDateString();
            int uk = int.Parse(update_key);
            using (DataTable dt = new DataTable())
            {
                using (OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM debit_details WHERE [Date] Between #" + dt_start + "# and #" + dt_end + "# AND Key='" + uk + "' order by Date ASC", conn))
                {

                    adapter.Fill(dt);
                }
                DataTable tempDT = new DataTable();
                tempDT = dt.DefaultView.ToTable(true, "Date", "Rupees", "Receipt No");
                dataGridView5.DataSource = tempDT;
            }
            string pdfname = "Credits/" + name_label.Text + ".pdf";
            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 2, 2);
            PdfWriter wri = PdfWriter.GetInstance(doc, new FileStream(pdfname, FileMode.Create));
            doc.Open();

            iTextSharp.text.Font contentFont = iTextSharp.text.FontFactory.GetFont("Webdings", 20, iTextSharp.text.Font.BOLD, BaseColor.RED);
            Paragraph para = new Paragraph("PERFECT XEROX\n", contentFont);
            para.Alignment = Element.ALIGN_CENTER;
            doc.Add(para);
            iTextSharp.text.Font contentFont1 = iTextSharp.text.FontFactory.GetFont("Courier", 12, iTextSharp.text.Font.UNDERLINE, BaseColor.BLACK);
            iTextSharp.text.Font contentFont4 = iTextSharp.text.FontFactory.GetFont("Courier", 12, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            Paragraph email = new Paragraph("email: perfectxerox53@gmail.com\n", contentFont4);
            email.Alignment = Element.ALIGN_CENTER;
            doc.Add(email);
            Paragraph para1 = new Paragraph("Alkesh:98252-74649            Office:0285-2653161           Jignesh:97255-27727\n", contentFont1);
            para1.Alignment = Element.ALIGN_CENTER;
            doc.Add(para1);
            iTextSharp.text.Font contentFont2 = iTextSharp.text.FontFactory.GetFont("Segoe UI", 16, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
            Paragraph para2 = new Paragraph("       " + name_label.Text + "\n\n", contentFont2);
            doc.Add(para2);
            int[] wid = { 100, 100, 100 };
            PdfPTable table = new PdfPTable(dataGridView5.Columns.Count);
            table.WidthPercentage = 80f;
            table.SetWidths(wid);
            PdfPCell cell;
            for (int j = 0; j < dataGridView5.Columns.Count; j++)
            {
                cell = new PdfPCell(new Phrase(dataGridView5.Columns[j].HeaderText));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.VerticalAlignment = 1;
                table.AddCell(cell);
            }
            table.HeaderRows = 1;
            for (int i = 0; i < dataGridView5.Rows.Count; i++)
            {
                for (int k = 0; k < dataGridView5.Columns.Count; k++)
                {
                    if (dataGridView5[k, i].Value != null)
                    {
                        if (k == 0)
                        {
                            DateTime dt = DateTime.Parse(dataGridView5[k, i].Value.ToString());
                            cell = new PdfPCell(new Phrase(dt.ToString("dd/MM/yy")));
                        }
                        else
                            cell = new PdfPCell(new Phrase(dataGridView5[k, i].Value.ToString()));
                        cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        cell.VerticalAlignment = 1;
                        table.AddCell(cell);
                    }
                }
            }
            doc.Add(table);
            int sum1 = 0;
            for (int i = 0; i < dataGridView5.Rows.Count - 1; i++)
            {
                sum1 += Convert.ToInt32(dataGridView5.Rows[i].Cells[1].Value);
            }

            Paragraph para3 = new Paragraph("Total: " + sum1.ToString("0.00") + "              ", contentFont2);
            para3.Alignment = Element.ALIGN_RIGHT;
            doc.Add(para3);
            doc.Close();
            pdfname = "\\Credits\\" + name_label.Text + ".pdf";
            System.Diagnostics.Process.Start(Application.StartupPath + pdfname);
            dateTimePicker3.Visible = false;
            dateTimePicker4.Visible = false;
            Ok_button.Visible = false;
            panel4.Visible = false;
        }
        private void button9_Click(object sender, EventArgs e)
        {
            dateTimePicker3.Visible = true;
            dateTimePicker4.Visible = true;
            ok_panel1.Visible = true;
            panel2.Visible = true;
            panel3.Visible = true;
            panel4.Visible = true;
            resultBox.Visible = false;

        }
        private void ok_panel1_Click(object sender, EventArgs e)
        {
            string dt_start = dateTimePicker3.Value.ToShortDateString();
            dateTimePicker4.Value = dateTimePicker4.Value.AddDays(1);
            string dt_end = dateTimePicker4.Value.ToShortDateString();
            int uk = int.Parse(update_key);
            using (DataTable dt = new DataTable())
            {
                using (OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM Sub_Entry WHERE [Date] Between #" + dt_start + "# and #" + dt_end + "# AND Key='" + uk + "' order by Date ASC", conn))
                {
                    adapter.Fill(dt);
                }
                dataGridView4.DataSource = dt;
            }

            string pdfname = "Bills/"+name_label.Text + ".pdf";
            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 5, 5);
            PdfWriter wri = PdfWriter.GetInstance(doc, new FileStream(pdfname, FileMode.Create));
            doc.Open();

            iTextSharp.text.Font contentFont = iTextSharp.text.FontFactory.GetFont("Webdings", 20, iTextSharp.text.Font.BOLD, BaseColor.RED);
            Paragraph para = new Paragraph("PERFECT XEROX\n", contentFont);
            para.Alignment = Element.ALIGN_CENTER;
            doc.Add(para);
            iTextSharp.text.Font contentFont4 = iTextSharp.text.FontFactory.GetFont("Courier", 12, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            Paragraph email = new Paragraph("email: perfectxerox53@gmail.com\n", contentFont4);
            email.Alignment = Element.ALIGN_CENTER;
            doc.Add(email);
            iTextSharp.text.Font contentFont1 = iTextSharp.text.FontFactory.GetFont("Courier", 12, iTextSharp.text.Font.UNDERLINE, BaseColor.BLACK);
            Paragraph para1 = new Paragraph("Alkesh:98252-74649            Office:0285-2653161           Jignesh:97255-27727\n", contentFont1);
            para1.Alignment = Element.ALIGN_CENTER;
            doc.Add(para1);
            iTextSharp.text.Font contentFont2 = iTextSharp.text.FontFactory.GetFont("Segoe UI", 16, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
            Paragraph para2 = new Paragraph(name_label.Text + "\n\n", contentFont2);
            doc.Add(para2);
            int[] wid = { 25, 75, 13, 10, 10, 15, 10, 10, 20, 10, 13, 20, 0 };
            PdfPTable table = new PdfPTable(dataGridView4.Columns.Count);
            table.WidthPercentage = 100f;
            table.SetWidths(wid);
            PdfPCell cell;
            for (int j = 0; j < dataGridView4.Columns.Count; j++)
            {
                cell = new PdfPCell(new Phrase(dataGridView4.Columns[j].HeaderText));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.VerticalAlignment = 1;
                table.AddCell(cell);
            }
            table.HeaderRows = 1;
           
            for (int i = 0; i < dataGridView4.Rows.Count; i++)
            {
                for (int k = 0; k < dataGridView4.Columns.Count; k++)
                {
                    if (dataGridView4[k, i].Value != null)
                    {
                        if (k == 0)
                        {
                            DateTime dt = DateTime.Parse(dataGridView4[k, i].Value.ToString());
                            cell = new PdfPCell(new Phrase(dt.ToString("dd/MM/yy")));
                        }else if (k == 11)
                        {
                            double s = double.Parse((dataGridView4[k, i].Value.ToString()));
                            cell = new PdfPCell(new Phrase(s.ToString("0.00")));
                        }
                        else
                            cell = new PdfPCell(new Phrase(dataGridView4[k, i].Value.ToString()));
                        if (k != 1 && k!=0)
                        {
                            cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                            cell.VerticalAlignment = 1;
                        }        
                        table.AddCell(cell);
                    }
                }
            }
            doc.Add(table);
            sum = 0;
            for (int i = 0; i < dataGridView4.Rows.Count - 1; i++)
            {
                sum += double.Parse(dataGridView4.Rows[i].Cells[11].Value.ToString());
            }

            Paragraph para3 = new Paragraph("Grand Total: " + sum.ToString("0.00"), contentFont2);
            para3.Alignment = Element.ALIGN_RIGHT;
            doc.Add(para3);
            doc.Close();
            pdfname = "\\Bills\\" + name_label.Text + ".pdf";
            System.Diagnostics.Process.Start(Application.StartupPath+pdfname);
            dateTimePicker3.Visible = false;
            dateTimePicker4.Visible = false;
            ok_panel1.Visible = false;
            panel4.Visible = false;
            panel3.Visible = false;
            panel2.Visible = false;
            resultBox.Visible = true;
        }
        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
        
      

       


    }

}
