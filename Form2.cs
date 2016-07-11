using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form2 : Form
    {
       
        public static DataTable dt;
        public Form2()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            dt = new DataTable();
            dt.Columns.Add(new DataColumn("Num1", typeof(string)));
            dt.Columns.Add(new DataColumn("Num2", typeof(string)));
            dt.Columns.Add(new DataColumn("Num3", typeof(string)));
            dt.Columns.Add(new DataColumn("Result", typeof(string)));

        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

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
       
        private void button1_Click(object sender, EventArgs e)
        {   if (textBox1.Text == "")
                textBox1.Text = "0";
            if (textBox2.Text == "")
                textBox2.Text = "0";
            if (textBox3.Text == "")
                textBox3.Text = "0";
            double n1, n2, n3,result;
            n1 = double.Parse(textBox1.Text);
            n2 = double.Parse(textBox2.Text);
            n3 = double.Parse(textBox3.Text);
            result = ((n1 + n2) * n3) / 2.00;
            DataRow dr = null;
          
            dr = dt.NewRow();
            dr["Num1"] = n1.ToString("0.00##");
            dr["Num2"] = n2.ToString("0.00##");
            dr["Num3"] = n3.ToString("0.00##");
            dr["Result"] = result.ToString("0.00##");
            dt.Rows.Add(dr);
            dataGridView1.DataSource = dt;
            dataGridView1.DefaultCellStyle.Font = new Font("Tahoma", 15);
            clear_all();
        }

        private void fileToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Viewpage vp = new Viewpage();
            this.Hide();
            vp.Show();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1 f1 = new Form1();
            this.Hide();
            f1.Show();
        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
        private void clear_all()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dt.Rows.Count > 0)
            {
                dt.Rows.RemoveAt(dataGridView1.CurrentRow.Index);
                dataGridView1.DataSource = dt;
                dataGridView1.DefaultCellStyle.Font = new Font("Tahoma", 15);
                
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dt.Rows.Count > 0)
            {
                double sum = 0;
                for(int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {
                    sum += double.Parse(dataGridView1.Rows[i].Cells[3].Value.ToString());
                }
                textBox4.Text = sum.ToString("0.00");
            }
           
        }
    }
}
