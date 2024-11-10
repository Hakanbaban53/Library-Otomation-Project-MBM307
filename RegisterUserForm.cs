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
    public partial class RegisterUserForm : Form
    {
        public RegisterUserForm()
        {
            InitializeComponent();
            cmbRole.Items.Add("Admin");
            cmbRole.Items.Add("Librarian");
            cmbRole.SelectedIndex = 0; // Default to first role

        }

        private void btnRegisterUser_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            string role = cmbRole.SelectedItem.ToString();

            // Hash the password before sending to the database
            string hashedPassword = HashPassword(password);

            DatabaseHelper db = new DatabaseHelper();
            SqlParameter[] parameters = new SqlParameter[]
            {
           new SqlParameter("@Username", username),
           new SqlParameter("@Password", hashedPassword),
           new SqlParameter("@Role", role)
            };

            try
            {
                db.ExecuteNonQuery("AddNewUser", parameters);
                MessageBox.Show("User registered successfully!");
            }
            catch (Exception ex)
            {
                lblRegisterError.Text = "Error: " + ex.Message;
                lblRegisterError.ForeColor = Color.Red;
            }
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }


    }
}
