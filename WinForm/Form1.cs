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

namespace WinForm
{
    public partial class Form1 : Form
    {
        private String constr = System.Configuration.ConfigurationManager.ConnectionStrings["WinForm.Properties.Settings.TestConnectionString"].ConnectionString;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!check_input())
            {
                MessageBox.Show("Please fill all the required fields");
                return;
            }
            SqlConnection conn = new SqlConnection(constr);
            
            try
            {
               
                String query = "INSERT INTO Customer(firstname,lastname,mname,home_add,phoneno,tinno)" +
                                "VALUES('" + fnametxt.Text + "','" + lnametxt.Text + "','" + mnametxt.Text + "','" + addresstxt.Text + "','" + phonetxt.Text + "','" + tintxt.Text +"')";
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = query;
                cmd.Connection = conn;
                conn.Open();
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                MessageBox.Show("New customer is added");
                try
                {
                   
                    cmd.CommandText = "SELECT * FROM Customer";
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = cmd;
                    DataTable dTable = new DataTable();
                    adapter.Fill(dTable);
                    dataGridView1.DataSource = dTable;
                    clearTexBox();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Exeption : " + ex.Message);
                }
                finally
                {
                    cmd.Dispose();
                }
            }catch(Exception ex)
            {
                MessageBox.Show("Exception : " +ex.Message);
            } finally
            {
                conn.Close();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'testDataSet.Customer' table. You can move, or remove it, as needed.
            this.customerTableAdapter.Fill(this.testDataSet.Customer);

        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            String id = dataGridView1.Rows[e.RowIndex].Cells["custid"].Value.ToString();
            String firstname = dataGridView1.Rows[e.RowIndex].Cells["firstname"].Value.ToString();
            String lastname = dataGridView1.Rows[e.RowIndex].Cells["lastname"].Value.ToString();
            String mname = dataGridView1.Rows[e.RowIndex].Cells["mname"].Value.ToString();
            String home_add = dataGridView1.Rows[e.RowIndex].Cells["home_add"].Value.ToString();
            String phoneno = dataGridView1.Rows[e.RowIndex].Cells["phoneno"].Value.ToString();
            String tinno = dataGridView1.Rows[e.RowIndex].Cells["tinno"].Value.ToString();

            //SET DATA FOR THE TEXTBOX
            ID.Text = id;
            fnametxt.Text = firstname;
            lnametxt.Text = lastname;
            mnametxt.Text = mname;
            addresstxt.Text = home_add;
            phonetxt.Text = phoneno;
            tintxt.Text = tinno;
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(ID.Text == "")
            {
                return;
            }
            if (!check_input())
            {
                MessageBox.Show("Please fill all the required fields");
                return;
            }
            SqlConnection conn = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand();
            String query = "UPDATE Customer " +
                           "SET firstname = '" + fnametxt.Text + "' " +
                           ",lastname = '" + lnametxt.Text + "' " +
                           ",mname = '" + mnametxt.Text + "' " +
                           ",home_add = '" + addresstxt.Text + "' " +
                           ",phoneno ='" + phonetxt.Text + "' " +
                           ",tinno = '" + tintxt.Text + "' " +
                           "WHERE custid = '" + ID.Text + "'";
            try
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = query;
                cmd.Connection = conn;
                conn.Open();
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                clearTexBox();
                ID.Text = "ID";
                MessageBox.Show("Current user is updated");
                try
                {

                    cmd.CommandText = "SELECT * FROM Customer";
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = cmd;
                    DataTable dTable = new DataTable();
                    adapter.Fill(dTable);
                    dataGridView1.DataSource = dTable;
                  
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Exeption : " + ex.Message);
                }
                finally
                {
                    cmd.Dispose();
                }
            } catch(SqlException ex)
            {
                MessageBox.Show("Exepction : " +ex.Message);
                conn.Close();
            }
            finally
            {
                conn.Close();
            }
                             
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(ID.Text == "")
            {
                return;
            }
            if (MessageBox.Show(
                "Yes or No",
                "Delete user?",
                MessageBoxButtons.YesNoCancel ,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1
                ) == System.Windows.Forms.DialogResult.Yes)
            {
                SqlConnection conn = new SqlConnection(this.constr);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                String query = "DELETE FROM Customer WHERE custid = '" + Convert.ToInt64(ID.Text) + "'";
                cmd.CommandText = query;
                cmd.Connection = conn;
                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    MessageBox.Show("User was deleted");
                    clearTexBox();
                    ID.Text = "ID";
                    try
                    {

                        cmd.CommandText = "SELECT * FROM Customer";
                        SqlDataAdapter adapter = new SqlDataAdapter();
                        adapter.SelectCommand = cmd;
                        DataTable dTable = new DataTable();
                        adapter.Fill(dTable);
                        dataGridView1.DataSource = dTable;
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Exeption : " + ex.Message);
                    }
                    finally
                    {
                        cmd.Dispose();
                    }
                } catch(SqlException ex)
                {
                    MessageBox.Show("Exception : " + ex.Message);
                    conn.Close();
                } finally
                {
                    conn.Close();
                }
            }
        }
        private void clearTexBox()
        {
            Action<Control.ControlCollection> func = null;
            func = (controls) =>
            {
                foreach (Control control in controls)
                {
                   
                    if (control is TextBox)
                    {
                        (control as TextBox).Clear();
                      
                    }
                    else
                    {
                        func(control.Controls);
                    }
                }
            };
            func(Controls); 
        }
        private bool check_input()
        {
            bool ok = true;
            Action<Control.ControlCollection> func = null;
            func = (controls) =>
            {
                foreach (Control control in controls)
                {

                    if (control is TextBox)
                    {
                        if ((control as TextBox).Text == "")
                        {
                            ok = false;
                        }
                    }
                    else
                    {
                        func(control.Controls);
                    }
                }
            };
            func(Controls);
            return ok;
        }
    }
}
