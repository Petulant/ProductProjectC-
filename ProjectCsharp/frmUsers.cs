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
    public partial class frmUsers : frmInheritance
    {
        bool boolUserExist=false;
        int intUserID = 0;

        string strAccessConnectionString= "Driver={Microsoft Access Driver (*.mdb)}; Dbq=products.mdb; Uid=Admin; Pwd=;";
       

        public frmUsers()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            controlsLoad();
            loadUsers();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmProducts frm4 = new frmProducts();
            frm4.Show();
            this.Hide();
        }
        private void btnCreate_Click(object sender, EventArgs e)
        {
            if(btnCreate.Text=="Save")
            {
                if(txtfirstName.Text=="")
                {
                    MessageBox.Show("FirstName field cannot be left empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if(txtLastName.Text=="")
                {
                    MessageBox.Show("LastName field cannot be left empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if(txtUserName.Text=="")
                {
                    MessageBox.Show("UserName field cannot be left empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if(txtPassword.Text=="")
                {
                    MessageBox.Show("Password field cannot be left empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    checkIfUserExist();
                    if (boolUserExist == false)
                    {
                        createUser();
                        controlsLoad();
                        clearTextBoxes();
                        loadUsers();
                    }
                    else if(boolUserExist==true)
                    {
                        MessageBox.Show("User already exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
           else if(btnCreate.Text=="Create")
            {
                controlCreate();
            }
                
            

        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            controlsEdit();
            editUser();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            updateUser();
            controlsLoad();
            clearTextBoxes();
            loadUsers();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            deleteUser();
            controlsLoad();
            clearTextBoxes();
            loadUsers();
        }
        private void controlsLoad()
        {
            txtfirstName.Enabled = false;
            txtLastName.Enabled = false;
            txtUserName.Enabled = false;
            txtPassword.Enabled = false;

            cboUsers.Enabled = true;

            btnCreate.Enabled = true;
            btnUpdate.Enabled = false;
            btnEdit.Enabled = true;
            btnDelete.Enabled = false;
            button1.Enabled = true;

            btnCreate.Text = "Create";
        }
        private void controlCreate()
        {
            txtfirstName.Enabled = true;
            txtLastName.Enabled = true;
            txtUserName.Enabled = true;
            txtPassword.Enabled = true;

            cboUsers.Enabled = false;

            btnCreate.Enabled = true;
            btnUpdate.Enabled = false;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            button1.Enabled = false;

            btnCreate.Text = "Save";
        }
        private void controlsEdit()
        {
            txtfirstName.Enabled = true;
            txtLastName.Enabled = true;
            txtUserName.Enabled = true;
            txtPassword.Enabled = true;

            cboUsers.Enabled = false;


            btnCreate.Enabled = false;
            btnUpdate.Enabled = true;
            btnEdit.Enabled = false;
            btnDelete.Enabled = true;
            button1.Enabled = false;
        }
        private void clearTextBoxes()
        {
            txtfirstName.Text = "";
            txtLastName.Text = "";
            txtPassword.Text = "";
            txtUserName.Text = "";
        }
        private void loadUsers()
        {
            cboUsers.DataSource = null;
            cboUsers.Items.Clear();

            OdbcConnection odbcConnection = new OdbcConnection();
            odbcConnection.ConnectionString = strAccessConnectionString;

            string query = "select UserName from Users";
            OdbcCommand cmd = new OdbcCommand(query, odbcConnection);

            odbcConnection.Open();
            OdbcDataReader dr = cmd.ExecuteReader();
            AutoCompleteStringCollection UserCollection = new AutoCompleteStringCollection();

            while(dr.Read())
            {
                UserCollection.Add(dr.GetString(0));
            }
            odbcConnection.Close();

            cboUsers.DataSource = UserCollection;
            
        }

        private void createUser()
        {
            string query = "select * from Users where ID=0";

            OdbcConnection odbcConnection = new OdbcConnection();
            OdbcDataAdapter da = new OdbcDataAdapter(query, odbcConnection);

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataRow dr;

            odbcConnection.ConnectionString = strAccessConnectionString;

            da.Fill(ds, "Users");
            dt = ds.Tables["Users"];

            try
            {
                dr = dt.NewRow();
                dr["FirstName"] = txtfirstName.Text;
                dr["LastName"] = txtLastName.Text;
                dr["UserName"] = txtUserName.Text;
                dr["Password"] = txtPassword.Text;

                dt.Rows.Add(dr);
                OdbcCommandBuilder cmd = new OdbcCommandBuilder(da);

                da.Update(ds, "Users");
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

        private void checkIfUserExist()
        {
            string query = "select * from Users where UserName='" + txtUserName.Text + "'";
            OdbcConnection odbcConnection = new OdbcConnection();

            OdbcCommand cmd;
            OdbcDataReader dr;
            odbcConnection.ConnectionString = strAccessConnectionString;

            odbcConnection.Open();
            cmd = new OdbcCommand(query, odbcConnection);
            dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                boolUserExist = true;
            }
            dr.Close();
            odbcConnection.Close();
            dr.Dispose();
            odbcConnection.Dispose();

        }
        private void editUser()
        {
            string query = "select * from Users where UserName ='" + cboUsers.Text + "'";

            
            OdbcConnection odbcConnection = new OdbcConnection();
            OdbcCommand cmd;
            OdbcDataReader dr;

            odbcConnection.ConnectionString = strAccessConnectionString;
            odbcConnection.Open();
            cmd = new OdbcCommand(query, odbcConnection);
            dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                intUserID = dr.GetInt32(0);
                txtfirstName.Text= dr.GetString(1);
                txtLastName.Text = dr.GetString(2);
                txtUserName.Text = dr.GetString(3);
                txtPassword.Text = dr.GetString(4);

            }
            dr.Close();
            odbcConnection.Close();
            dr.Dispose();
            odbcConnection.Dispose();
        }
        private void updateUser()
        {
            string query = "select * from Users where ID=" + intUserID;
            OdbcConnection odbcConnection = new OdbcConnection();

            odbcConnection.ConnectionString = strAccessConnectionString;

            OdbcDataAdapter da = new OdbcDataAdapter(query, odbcConnection);
            DataSet ds = new DataSet("Users");

            da.FillSchema(ds, SchemaType.Source, "Users");
            da.Fill(ds, "Users");
            DataTable dt;

            dt = ds.Tables["Users"];
            DataRow dr;
            dr = dt.NewRow();

            try
            {
                dr = dt.Rows.Find(intUserID);
                dr.BeginEdit();

                dr["FirstName"] = txtfirstName.Text;
                dr["LastName"] = txtLastName.Text;
                dr["Username"] = txtUserName.Text;
                dr["Password"] = txtPassword.Text;

                dr.EndEdit();
                OdbcCommandBuilder cmd = new OdbcCommandBuilder(da);
                da.Update(ds, "Users");

            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message.ToString());
            }
            finally
            {
                odbcConnection.Close();
                odbcConnection.Dispose();

            }
        }
        private void deleteUser()
       {
            string query = "delete * from Users Where ID=" + intUserID;
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
             




    }
}
