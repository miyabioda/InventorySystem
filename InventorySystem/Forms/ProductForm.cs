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

namespace InventorySystem.Forms
{
    public partial class ProductForm : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB; AttachDbFilename=|DataDirectory|\Database.mdf;Integrated Security=True");
        public ProductForm()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            NewProduct np = new NewProduct();
            np.TopMost = true;
            np.Show();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            NewUnit nu = new NewUnit();
            nu.TopMost = true;
            nu.Show();
        }

        public void fill_product_name()
        {
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "Select * from productTable order by Product_Name";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                comboBox1.Items.Add(dr["Product_Name"].ToString());
            }
        }

        public void fill_product_units()
        {
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "Select * from unitsTable order by Unit_List";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                comboBox2.Items.Add(dr["Unit_List"].ToString());
            }
        }

        public void fill_dg()
        {
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from stockTable order by Product_Name";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }



        private void iconButton1_Click(object sender, EventArgs e)
        {
            int i;
            SqlCommand cmd1 = con.CreateCommand();
            cmd1.CommandType = CommandType.Text;
            cmd1.CommandText = "Select * from stockTable where Product_Name='" + comboBox1.Text + "' and Product_Unit='" + comboBox2.Text + "'";
            cmd1.ExecuteNonQuery();
            DataTable dt1 = new DataTable();
            SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
            da1.Fill(dt1);
            i = Convert.ToInt32(dt1.Rows.Count.ToString());

            if (i == 0)
            {
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "insert into quantityTable values('" + comboBox1.SelectedItem.ToString() + "','" + Convert.ToInt32(textBox1.Text) + "','" + comboBox2.SelectedItem.ToString() + "')";
                cmd.ExecuteNonQuery();

                SqlCommand cmd3 = con.CreateCommand();
                cmd3.CommandType = CommandType.Text;
                cmd3.CommandText = "insert into stockTable values('" + comboBox1.SelectedItem.ToString() + "','" + Convert.ToInt32(textBox1.Text) + "','" + comboBox2.SelectedItem.ToString() + "')";
                cmd3.ExecuteNonQuery();

                fill_dg();
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                MessageBox.Show("Item '" + comboBox1.SelectedItem.ToString() + "' '" + comboBox2.SelectedItem.ToString() + "' Successfully Updated!");
            }
            else
            {
                SqlCommand cmd2 = con.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.CommandText = "insert into quantityTable values('" + comboBox1.SelectedItem.ToString() + "','" + Convert.ToInt32(textBox1.Text) + "','" + comboBox2.SelectedItem.ToString() + "')";
                cmd2.ExecuteNonQuery();

                SqlCommand cmd4 = con.CreateCommand();
                cmd4.CommandType = CommandType.Text;
                cmd4.CommandText = "update stockTable set Product_Qty=Product_Qty + " + textBox1.Text + " where Product_Name='" + comboBox1.Text + "' and Product_Unit='" + comboBox2.Text + "'";
                cmd4.ExecuteNonQuery();

                fill_dg();
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                MessageBox.Show("Item '" + comboBox1.SelectedItem.ToString() + "' '" + comboBox2.SelectedItem.ToString() + "' Successfully Updated!");
            }
        }

        private void ProductForm_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'databaseDataSet.stockTable' table. You can move, or remove it, as needed.
            this.stockTableTableAdapter.Fill(this.databaseDataSet.stockTable);
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
            fill_product_name();
            fill_product_units();
            fill_dg();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        private void iconButton2_Click(object sender, EventArgs e)
        {
            int id;
            id = Convert.ToInt32(dataGridView1.SelectedCells[0].Value.ToString());
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "delete from stockTable where Id= " + id + "";
            cmd.ExecuteNonQuery();
            fill_dg();
        }
    }
}
