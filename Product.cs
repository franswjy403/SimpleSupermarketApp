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
    public partial class Product : Form
    {
        public Product()
        {
            InitializeComponent();
        }
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Acer\Documents\smarket.mdf;Integrated Security=True;Connect Timeout=30");
        private void bunifuThinButton22_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void populate()
        {
            Con.Open();
            string query = "select * from Product";
            SqlDataAdapter sda = new SqlDataAdapter(query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            ProdDGV.DataSource = ds.Tables[0];
            Con.Close();
        }

        private void populate(string cat)
        {
            Con.Open();
            string query = "select * from Product where Category='"+cat+"'";
            SqlDataAdapter sda = new SqlDataAdapter(query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            ProdDGV.DataSource = ds.Tables[0];
            Con.Close();
        }

        private void fillcombo()
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
            comboBoxDown.ValueMember = "Name";
            comboBoxDown.DataSource = dt;
            comboBoxUp.ValueMember = "Name";
            comboBoxUp.DataSource = dt;
            Con.Close();
        }

        private void Product_Load(object sender, EventArgs e)
        {
            populate();
            fillcombo();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Category cat = new Category();
            cat.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Seller sell = new Seller();
            sell.Show();
            this.Hide();
        }

        private void ProdDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            ProdId.Text = ProdDGV.SelectedRows[0].Cells[0].Value.ToString();
            ProdName.Text = ProdDGV.SelectedRows[0].Cells[1].Value.ToString();
            ProdQty.Text = ProdDGV.SelectedRows[0].Cells[2].Value.ToString();
            ProdPrice.Text = ProdDGV.SelectedRows[0].Cells[3].Value.ToString();
            comboBoxDown.SelectedIndex = comboBoxDown.FindString(ProdDGV.SelectedRows[0].Cells[4].Value.ToString());
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            try
            {
                Con.Open();
                string query = "insert into Product values('" + ProdId.Text + "','" + ProdName.Text + "','" + ProdQty.Text + "','"+ProdPrice.Text+"','"+comboBoxDown.SelectedValue.ToString()+"')";
                SqlCommand cmd = new SqlCommand(query, Con);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Product Added Successfully");
                Con.Close();
                populate();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (ProdId.Text == "")
                {
                    MessageBox.Show("Select The Product to Delete");
                }
                else
                {
                    Con.Open();
                    string query = "delete from Product where Id=" + ProdId.Text + ";";
                    SqlCommand cmd = new SqlCommand(query, Con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Product Deleted Succesfully");
                    Con.Close();
                    populate();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (ProdId.Text == "" || ProdName.Text == "" || ProdQty.Text == "" || ProdPrice.Text == "" || comboBoxDown.SelectedValue.ToString() == "")
                {
                    MessageBox.Show("Missing Information");
                }
                else
                {
                    Con.Open();
                    string query = "update Product set Name='" + ProdName.Text + "', Qty='" + ProdQty.Text + "', Price='"+ProdPrice.Text+"', Category='"+comboBoxDown.SelectedValue.ToString()+"' where Id=" + ProdId.Text + ";";
                    SqlCommand cmd = new SqlCommand(query, Con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Category Edited Successfully");
                    Con.Close();
                    populate();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            if (comboBoxUp.SelectedIndex == -1)
            {
                populate();
            }
            else
            {
                populate(comboBoxUp.SelectedValue.ToString());
            }
        }

        private void comboBoxUp_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Main log = new Main();
            log.Show();
            this.Hide();
        }
    }
}
