using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Library_Otomation
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            // Hash the password before sending it to the database for comparison
            string hashedPassword = HashPassword(password.Trim());
            MessageBox.Show(hashedPassword);


            DatabaseHelper db = new DatabaseHelper();
            SqlParameter[] parameters = new SqlParameter[]
            {
        new SqlParameter("@Username", username),
        new SqlParameter("@Password", hashedPassword)
            };

            DataTable result = db.ExecuteQuery("AuthenticateUser", parameters);

            if (result.Rows.Count > 0)
            {
                int userId = (int)result.Rows[0]["UserID"];
                string role = result.Rows[0]["Role"].ToString();

                this.Hide();
                AdminHomeForm adminHomeForm = new AdminHomeForm();
                adminHomeForm.Show();
            }
            else
            {
                lblLoginError.Text = "Invalid username or password.";
                lblLoginError.ForeColor = Color.Red;
            }
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (int b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private void lblLoginError_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
