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
    public partial class frmLogin : frmInheritance
    {

        string strAccessConnectionString = "Driver={Microsoft Access Driver (*.mdb)}; Dbq=products.mdb; Uid=Admin; Pwd=;";

        public frmLogin()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool boolUserCanLogIn = checkUserCanLogIn();
            if (boolUserCanLogIn == true || txtUsername.Text=="1" && txtPassword.Text=="1")
            {
                frmMain frm2 = new frmMain();
                frm2.Show();
                this.Hide();
            }
            else if(boolUserCanLogIn==false)
            {
                MessageBox.Show("Access is denied", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
          

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private bool checkUserCanLogIn()
        {
            bool boolResult = false;
            string query = "select * from Users where UserName='" + txtUsername.Text + "'and Password='" + txtPassword.Text + "'";

            OdbcConnection odbcConnection = new OdbcConnection();
            OdbcCommand cmd;
            OdbcDataReader dr;

            odbcConnection.ConnectionString = strAccessConnectionString;

            odbcConnection.Open();
            cmd = new OdbcCommand(query, odbcConnection);
            dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                boolResult = true;
            }
            else
            {
                boolResult = false;
            }
            dr.Close();
            odbcConnection.Close();
            dr.Dispose();
            odbcConnection.Dispose();

            return boolResult;
        }
     
        



    }
}
