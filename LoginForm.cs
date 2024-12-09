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
        // Yapıcı metod / Constructor
        public LoginForm()
        {
            InitializeComponent(); // Form bileşenlerini başlat / Initialize form components
                                   // "Beni Hatırla" seçeneği kontrolü / Check "Remember Me" option
            if (Properties.Settings.Default.RememberMe)
            {
                txtUsername.Text = Properties.Settings.Default.Username; // Kullanıcı adını getir / Retrieve username
                txtPassword.Text = Properties.Settings.Default.Password; // Parolayı getir / Retrieve password
                chkRememberMe.Checked = true; // "Beni Hatırla" seçeneğini işaretle / Check "Remember Me" option
            }
        }

        // Giriş butonuna tıklama olayı / Login button click event
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim(); // Kullanıcı adını al / Get username
            string password = txtPassword.Text.Trim(); // Parolayı al / Get password

            // Kullanıcı adı veya parola boşsa hata mesajı göster / Show error message if username or password is empty
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                lblError.Text = "Kullanıcı Adı ve Parola gerekli."; // Hata mesajı / Error message
                lblError.ForeColor = Color.Red; // Hata mesajı rengi / Error message color
                return; // Metodu sonlandır / End method
            }

            try
            {
                // Parolayı hashle / Hash the password
                string hashedPassword = PasswordHelper.HashPassword(password);
                DatabaseHelper db = new DatabaseHelper(); // DatabaseHelper sınıfından nesne oluştur / Create an instance of DatabaseHelper
                                                          // Parametreleri tanımla / Define parameters
                SqlParameter[] parameters = {
                            new SqlParameter("@Username", username),
                            new SqlParameter("@Password", hashedPassword)
                        };

                // Kullanıcıyı doğrula / Authenticate user
                DataTable result = db.ExecuteQuery("AuthenticateUser", parameters, true);

                // Kullanıcı doğrulama sonucu kontrolü / Check user authentication result
                if (result.Rows.Count > 0)
                {
                    string role = result.Rows[0]["Role"].ToString(); // Kullanıcı rolü / User role
                    bool isActive = Convert.ToBoolean(result.Rows[0]["IsActive"]); // Kullanıcı aktif mi / Is user active?

                    // Kullanıcı aktif değilse hata mesajı göster / Show error message if user is not active
                    if (!isActive)
                    {
                        lblError.Text = "Hesabınız devre dışı. Lütfen yöneticiyle iletişime geçin."; // Hata mesajı / Error message
                        lblError.ForeColor = Color.Red; // Hata mesajı rengi / Error message color
                        return;
                    }

                    // "Beni Hatırla" seçeneği kontrolü / Check "Remember Me" option
                    if (chkRememberMe.Checked)
                    {
                        Properties.Settings.Default.Username = username; // Kullanıcı adını kaydet / Save username
                        Properties.Settings.Default.Password = password; // Parolayı kaydet / Save password
                        Properties.Settings.Default.RememberMe = true; // "Beni Hatırla" seçeneğini işaretle / Check "Remember Me" option
                        Properties.Settings.Default.Save(); // Ayarları kaydet / Save settings
                    }
                    else
                    {
                        Properties.Settings.Default.Reset(); // Ayarları sıfırla / Reset settings
                    }

                    this.Hide(); // Mevcut formu gizle / Hide current form
                                 // Giriş başarılı olduğunda MainForm'a geçiş / Transition to MainForm on successful login
                    MainForm mainForm = new MainForm(role, username);
                    mainForm.ShowDialog(); // MainForm'u göster / Show MainForm
                }
                else
                {
                    lblError.Text = "Kullanıcı Adı veya Parola yanlış."; // Hata mesajı / Error message
                    lblError.ForeColor = Color.Red; // Hata mesajı rengi / Error message color
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda mesaj kutusu göster / Show message box on error
                MessageBox.Show($"Bir Sorun Oluştu: {ex.Message}", "Hata"); // Error message
            }
        }
    }
}
