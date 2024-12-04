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
            if (Properties.Settings.Default.RememberMe)
            {
                txtUsername.Text = Properties.Settings.Default.Username;
                txtPassword.Text = Properties.Settings.Default.Password;
                chkRememberMe.Checked = true;
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                lblError.Text = "Kullanıcı Adı veya Parola gerekli.";
                lblError.ForeColor = Color.Red;
                return;
            }

            try
            {
                string hashedPassword = PasswordHelper.HashPassword(password);
                DatabaseHelper db = new DatabaseHelper();
                SqlParameter[] parameters = {
                    new SqlParameter("@Username", username),
                    new SqlParameter("@Password", hashedPassword)
                };

                DataTable result = db.ExecuteQuery("AuthenticateUser", parameters, true);

                if (result.Rows.Count > 0)
                {
                    string role = result.Rows[0]["Role"].ToString();
                    bool isActive = Convert.ToBoolean(result.Rows[0]["IsActive"]);

                    if (!isActive)
                    {
                        lblError.Text = "Hesabınız devre dışı. Lütfen yöneticiyle iletişime geçin.";
                        lblError.ForeColor = Color.Red;
                        return;
                    }

                    if (chkRememberMe.Checked)
                    {
                        Properties.Settings.Default.Username = username;
                        Properties.Settings.Default.Password = password;
                        Properties.Settings.Default.RememberMe = true;
                        Properties.Settings.Default.Save();
                    }
                    else
                    {
                        Properties.Settings.Default.Reset();
                    }

                    this.Hide();
                    // Giriş başarılı olduğunda MainForm'a geçiş
                    MainForm mainForm = new MainForm(role, username);
                    mainForm.ShowDialog();
                    this.Show();
                }
                else
                {
                    lblError.Text = "Kullanıcı Adı veya Parola yanlış.";
                    lblError.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir Sorun Oluştu: {ex.Message}", "Hata");
            }
        }


    }
}
