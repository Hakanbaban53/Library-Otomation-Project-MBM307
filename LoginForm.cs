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
        // Yapıcı metod
        public LoginForm()
        {
            InitializeComponent(); // Form bileşenlerini başlat
                                   // "Beni Hatırla" seçeneği kontrolü
            if (Properties.Settings.Default.RememberMe)
            {
                txtUsername.Text = Properties.Settings.Default.Username; // Kullanıcı adını getir
                txtPassword.Text = Properties.Settings.Default.Password; // Parolayı getir
                chkRememberMe.Checked = true; // "Beni Hatırla" seçeneğini işaretle
            }
        }

        // Giriş butonuna tıklama olayı
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim(); // Kullanıcı adını al
            string password = txtPassword.Text.Trim(); // Parolayı al

            // Kullanıcı adı veya parola boşsa hata mesajı göster
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                lblError.Text = "Kullanıcı Adı ve Parola gerekli."; // Hata mesajı
                lblError.ForeColor = Color.Red; // Hata mesajı rengi
                return; // Metodu sonlandır
            }

            try
            {
                // Parolayı hashle
                string hashedPassword = PasswordHelper.HashPassword(password);
                DatabaseHelper db = new DatabaseHelper(); // DatabaseHelper sınıfından nesne oluştur
                                                          // Parametreleri tanımla
                SqlParameter[] parameters = {
                        new SqlParameter("@Username", username),
                        new SqlParameter("@Password", hashedPassword)
                    };

                // Kullanıcıyı doğrula
                DataTable result = db.ExecuteQuery("AuthenticateUser", parameters, true);

                // Kullanıcı doğrulama sonucu kontrolü
                if (result.Rows.Count > 0)
                {
                    string role = result.Rows[0]["Role"].ToString(); // Kullanıcı rolü
                    bool isActive = Convert.ToBoolean(result.Rows[0]["IsActive"]); // Kullanıcı aktif mi

                    // Kullanıcı aktif değilse hata mesajı göster
                    if (!isActive)
                    {
                        lblError.Text = "Hesabınız devre dışı. Lütfen yöneticiyle iletişime geçin.";
                        lblError.ForeColor = Color.Red;
                        return;
                    }

                    // "Beni Hatırla" seçeneği kontrolü
                    if (chkRememberMe.Checked)
                    {
                        Properties.Settings.Default.Username = username; // Kullanıcı adını kaydet
                        Properties.Settings.Default.Password = password; // Parolayı kaydet
                        Properties.Settings.Default.RememberMe = true; // "Beni Hatırla" seçeneğini işaretle
                        Properties.Settings.Default.Save(); // Ayarları kaydet
                    }
                    else
                    {
                        Properties.Settings.Default.Reset(); // Ayarları sıfırla
                    }

                    this.Hide(); // Mevcut formu gizle
                                 // Giriş başarılı olduğunda MainForm'a geçiş
                    MainForm mainForm = new MainForm(role, username);
                    mainForm.ShowDialog(); // MainForm'u göster
                }
                else
                {
                    lblError.Text = "Kullanıcı Adı veya Parola yanlış."; // Hata mesajı
                    lblError.ForeColor = Color.Red; // Hata mesajı rengi
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda mesaj kutusu göster
                MessageBox.Show($"Bir Sorun Oluştu: {ex.Message}", "Hata");
            }
        }
    }
}
