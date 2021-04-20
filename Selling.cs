using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace SimpleSupermarketApp
{
    public partial class Selling : Form
    {
        //private string NameSeller = "";
        public Selling()
        {
            InitializeComponent();
        }

        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Acer\Documents\smarket.mdf;Integrated Security=True;Connect Timeout=30");

        private void populate()
        {
            Con.Open();
            string query = "select Name, Price from Product";
            SqlDataAdapter sda = new SqlDataAdapter(query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            SellerDGV.DataSource = ds.Tables[0];
            Con.Close();
        }

        private void populateBill()
        {
            Con.Open();
            string query = "select * from Bill where SellerName='"+Seller.Text+"'";
            SqlDataAdapter sda = new SqlDataAdapter(query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            BillDGV.DataSource = ds.Tables[0];
            Con.Close();
        }

        private void fillCombo()
        {
            //This Method will bind the Combobox with the Database
            Con.Open();
            string query = "select Name from Category";
            SqlCommand cmd = new SqlCommand(query, Con);
            SqlDataReader rdr;
            rdr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("Name", typeof(string));
            dt.Load(rdr);
            SelectCategory.ValueMember = "Name";
            SelectCategory.DataSource = dt;
            Con.Close();
        }


        private void SellerDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            ProdNameTB.Text = SellerDGV.SelectedRows[0].Cells[0].Value.ToString();
            PriceTB.Text = SellerDGV.SelectedRows[0].Cells[1].Value.ToString();
            
        }

        private void bunifuThinButton22_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Selling_Load(object sender, EventArgs e)
        {
            fillCombo();
            Seller.Text = Main.Sellername;
            populate();
            populateBill();
            
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            DateLabel.Text = DateTime.Today.Day.ToString() + "/" + DateTime.Today.Month.ToString() + "/" + DateTime.Today.Year.ToString();
        }


        

        private void OrderDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void AddButton_Click(object sender, EventArgs e)
        {

            try
            {
                if (BillIDTB.Text == "" || Seller.Text == "" || DateLabel.Text == "" || Rs.Text == "")
                {
                    MessageBox.Show("Missing Data");
                }
                else
                {
                    Con.Open();
                    string query = "insert into Bill values('" + BillIDTB.Text + "','" + Seller.Text + "','" + DateLabel.Text + "','" + Rs.Text + "')";
                    SqlCommand cmd = new SqlCommand(query, Con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Bill Added Successfully");
                    Con.Close();
                    populateBill();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }


        int n = 1, OrdTotal = 0;

        private void PrintButton_Click(object sender, EventArgs e)
        {
            if(printPreviewDialog1.ShowDialog() == DialogResult.OK)
            {
                printDocument1.Print();
            }
        }

        private void BillDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawString("MK-DRIAN SUPERMARKET", new Font("Century Gothic", 25, FontStyle.Bold),Brushes.Red, new Point(230));
            e.Graphics.DrawString("Bill ID: "+BillDGV.SelectedRows[0].Cells[0].Value.ToString(), new Font("Century Gothic", 20, FontStyle.Bold), Brushes.Blue, new Point(100, 70));
            e.Graphics.DrawString("Seller Name: " + BillDGV.SelectedRows[0].Cells[1].Value.ToString(), new Font("Century Gothic", 20, FontStyle.Bold), Brushes.Blue, new Point(100, 100));
            e.Graphics.DrawString("Date: " + BillDGV.SelectedRows[0].Cells[2].Value.ToString(), new Font("Century Gothic", 20, FontStyle.Bold), Brushes.Blue, new Point(100, 130));
            e.Graphics.DrawString("Total: " + BillDGV.SelectedRows[0].Cells[3].Value.ToString(), new Font("Century Gothic", 20, FontStyle.Bold), Brushes.Blue, new Point(100, 160));
            e.Graphics.DrawString("MK-DRIAN SUPERMARKET", new Font("Century Gothic", 25, FontStyle.Italic), Brushes.Red, new Point(230,190));
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            populate();
        }

        private void SelectCategory_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Con.Open();
            string query = "select Name, Price from Product where Category='" + SelectCategory.SelectedValue.ToString() + "'";
            SqlDataAdapter sda = new SqlDataAdapter(query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            SellerDGV.DataSource = ds.Tables[0];
            Con.Close();
        }

        private void SelectCategory_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Seller sell = new Seller();
            sell.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Category cat = new Category();
            cat.Show();
            cat.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Product prod = new Product();
            prod.Show();
            prod.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
            Main log = new Main();
            log.Show();
            this.Hide();
        }

        private void AddProdButton_Click(object sender, EventArgs e)
        {
            DataGridViewRow newRow = new DataGridViewRow();
            if (ProdNameTB.Text == "" || QtyTB.Text == "")
            {
                MessageBox.Show("Missing Data");
            }
            else
            {
                int total = Convert.ToInt32(PriceTB.Text) * Convert.ToInt32(QtyTB.Text);
                newRow.CreateCells(OrderDGV);
                newRow.Cells[0].Value = n;
                newRow.Cells[1].Value = ProdNameTB.Text;
                newRow.Cells[2].Value = PriceTB.Text;
                newRow.Cells[3].Value = QtyTB.Text;
                newRow.Cells[4].Value = total;
                OrderDGV.Rows.Add(newRow);
                OrdTotal = OrdTotal + total;
                Rs.Text = OrdTotal.ToString();
                n = n + 1;
            }
        }
    }
}
