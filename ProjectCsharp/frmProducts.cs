using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.Odbc;

namespace ProjectCsharp
{
    public partial class frmProducts : frmInheritance
    {
        //string ProductName;
        //string ProductDescription;
        //double ProductPrice;
        bool boolProductExists = false;
        int intProductID = 0;

        string strAccessConnectionString = "Driver={Microsoft Access Driver (*.mdb)}; Dbq=products.mdb; Uid=Admin; Pwd=;";

        public frmProducts()
        {
            InitializeComponent();
        }
        private void button5_Click(object sender, EventArgs e)
        {
            frmMain frm2 = new frmMain();
            frm2.Show();
            this.Hide();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            controlsLoad();
            loadProducts();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //create button
            if(button1.Text =="Save")
            {
                if(txtProductName.Text=="")
                {
                    MessageBox.Show("Product Name field cannot be left empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
                else if(txtProductDescription.Text=="")
                {
                    MessageBox.Show("Product Description field cannot be left empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (txtProductPrice.Text == "")
                {
                    MessageBox.Show("Product Price field cannot be left empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    checkIfProductsExists();
                    if (boolProductExists == false)
                    {
                        createProduct();
                        controlsLoad();
                        clearTextBoxes();
                        loadProducts();
                    }
                    else if (boolProductExists == true)
                    {
                        MessageBox.Show("Product already exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else if(button1.Text =="create")
            {
                controlsCreate();
            }
          
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //edit button
            editProduct();
            controlsEdit();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            // update button
            updateProduct();
            controlsLoad();
            clearTextBoxes();
            loadProducts();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            // delete button
            deleteProduct();
            controlsLoad();
            clearTextBoxes();
            loadProducts();

        }
        private void controlsLoad()
        {
            txtProductDescription.Enabled = false;
            txtProductName.Enabled = false;
            txtProductPrice.Enabled = false;

            cboProducts.Enabled = true;

            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = false;
            button4.Enabled = false; ;
            button5.Enabled = true;

            button1.Text = "create";
        }
        private  void controlsCreate()
        {
            txtProductDescription.Enabled = true;
            txtProductName.Enabled = true;
            txtProductPrice.Enabled = true;

            cboProducts.Enabled = false;

            button1.Enabled = true;
            button4.Enabled = false;
            button3.Enabled = false;
            button2.Enabled = false;
            button5.Enabled = false;

            button1.Text = "Save";

        }
        private void controlsEdit()
        {
            txtProductDescription.Enabled = true;
            txtProductName.Enabled = true;
            txtProductPrice.Enabled = true;

            cboProducts.Enabled = false;

            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = true;
            button4.Enabled = true;
            button5.Enabled = false;


        }
        private void clearTextBoxes()
        {
            txtProductDescription.Text = "";
            txtProductName.Text = "";
            txtProductPrice.Text = "";
        }
        private void loadProducts()
        {
            cboProducts.DataSource = null;
            cboProducts.Items.Clear();

            OdbcConnection odbcConnection = new OdbcConnection();
            odbcConnection.ConnectionString = strAccessConnectionString;

            string query= "Select ProductName from Product";

            OdbcCommand cmd = new OdbcCommand(query,odbcConnection);

            odbcConnection.Open();
            OdbcDataReader dr = cmd.ExecuteReader();
            AutoCompleteStringCollection ProductCollection = new AutoCompleteStringCollection();

            while (dr.Read())
            {
                ProductCollection.Add(dr.GetString(0));
            }
            cboProducts.DataSource = ProductCollection;
            odbcConnection.Close();
            }

        private void createProduct()
        {
            string query = "select * from Product where ProductID=0";

            OdbcConnection odbcConnection = new OdbcConnection();
            OdbcDataAdapter da = new OdbcDataAdapter(query, odbcConnection);

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataRow dr;

            odbcConnection.ConnectionString = strAccessConnectionString;

            da.Fill(ds,"Product");
            dt = ds.Tables["Product"];

            try
            {
                dr = dt.NewRow();
                dr["ProductName"] = txtProductName.Text;
                dr["ProductDescription"] = txtProductDescription.Text;
                dr["ProductPrice"]=txtProductPrice.Text;

                dt.Rows.Add(dr);
                OdbcCommandBuilder cmd = new OdbcCommandBuilder(da);

                da.Update(ds,"Product");
            }
            catch (Exception EX)
            {
                MessageBox.Show(EX.Message.ToString());
            }
            finally
            {
                odbcConnection.Close();
                odbcConnection.Dispose();
            }
           


        }
         private void checkIfProductsExists()
        {
            string query = "select * from Product where ProductName='" + txtProductName.Text + "'";
            OdbcConnection odbcConnection = new OdbcConnection();

            OdbcCommand cmd;
            OdbcDataReader dr;
            odbcConnection.ConnectionString = strAccessConnectionString;

            odbcConnection.Open();
            cmd = new OdbcCommand(query, odbcConnection);
            dr = cmd.ExecuteReader();

            if(dr.Read())
            {
                boolProductExists =true;
            }
            dr.Close();
            odbcConnection.Close();
            dr.Dispose();
            odbcConnection.Dispose();
        }
        private void editProduct()
         {
             string query = "select * from Product where ProductName ='" + cboProducts.Text + "'";

             OdbcConnection odbcConnection = new OdbcConnection();
             OdbcCommand cmd;
             OdbcDataReader dr;

             odbcConnection.ConnectionString = strAccessConnectionString;
             odbcConnection.Open();
             cmd = new OdbcCommand(query, odbcConnection);
             dr = cmd.ExecuteReader();

            if(dr.Read())
            {
                intProductID = dr.GetInt32(0);
                txtProductName.Text = dr.GetString(1);
                txtProductDescription.Text = dr.GetString(2);
                txtProductPrice.Text = dr.GetString(3);

            }
            dr.Close();
            odbcConnection.Close();
            dr.Dispose();
            odbcConnection.Dispose();
         }
        private void updateProduct()
        {
            string query = "select * from Product where ProductID=" + intProductID;
            OdbcConnection odbcConnection = new OdbcConnection();

            odbcConnection.ConnectionString = strAccessConnectionString;

            OdbcDataAdapter da = new OdbcDataAdapter(query, odbcConnection);
            DataSet ds = new DataSet("Product");

            da.FillSchema(ds,SchemaType.Source,"Product");
            da.Fill(ds, "Product");
            DataTable dt;

            dt = ds.Tables["Product"];
            DataRow dr;
            dr = dt.NewRow();

            try
            {
                dr = dt.Rows.Find(intProductID);
                dr.BeginEdit();

                dr["ProductName"] = txtProductName.Text;
                dr["ProductDescription"] = txtProductDescription.Text;
                dr["ProductPrice"] = txtProductPrice.Text;

                dr.EndEdit();
                OdbcCommandBuilder cmd = new OdbcCommandBuilder(da);
                da.Update(ds, "Product");

            }
            catch(Exception Ex)
            {
                MessageBox.Show(Ex.Message.ToString());
            }
            finally
            {
                odbcConnection.Close();
                odbcConnection.Dispose();

            }
        }
        private void deleteProduct()
        {
            string query = "delete * from Product Where ProductID=" + intProductID;
            OdbcConnection odbcConnection = new OdbcConnection();
            OdbcCommand cmd;
            OdbcDataReader dr;

            odbcConnection.ConnectionString = strAccessConnectionString;
            odbcConnection.Open();

            cmd = new OdbcCommand(query, odbcConnection);
            dr = cmd.ExecuteReader();

            if(dr.Read())
            {

            }
            dr.Close();
            odbcConnection.Close();
            dr.Dispose();
            odbcConnection.Dispose();

        }

        private void txtProductPrice_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
