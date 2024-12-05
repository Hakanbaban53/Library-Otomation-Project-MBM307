using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Library_Otomation
{
    public partial class ManageAccountForm : Form
    {
        // Kullanıcı bilgilerini saklamak için değişkenler
        private string currentUsername; // Mevcut kullanıcı adı
        private int userID; // Kullanıcı ID'si
        private string role; // Kullanıcı rolü
        private bool isActive; // Kullanıcının aktif olup olmadığı

        // Yapıcı metod, kullanıcı adını alır
        public ManageAccountForm(string username)
        {
            InitializeComponent(); // Form bileşenlerini başlat
            currentUsername = username; // Mevcut kullanıcı adını ayarla
        }

        // Form yüklendiğinde çağrılan metod
        private void ManageAccountForm_Load(object sender, EventArgs e)
        {
            try
            {
                DatabaseHelper db = new DatabaseHelper(); // Veritabanı yardımcı sınıfı
                SqlParameter[] parameters = { new SqlParameter("@Username", currentUsername) }; // Kullanıcı adı parametresi

                // Kullanıcı bilgilerini veritabanından al
                DataTable userData = db.ExecuteQuery("SELECT UserID, Role, IsActive, Email, Phone FROM Users WHERE Username = @Username", parameters, false);

                // Kullanıcı bilgileri varsa
                if (userData.Rows.Count > 0)
                {
                    // Kullanıcı bilgilerini değişkenlere ata
                    userID = Convert.ToInt32(userData.Rows[0]["UserID"]);
                    role = userData.Rows[0]["Role"].ToString();
                    isActive = Convert.ToBoolean(userData.Rows[0]["IsActive"]);

                    // Form bileşenlerine kullanıcı bilgilerini yerleştir
                    txtUsername.Text = currentUsername;
                    txtEmail.Text = userData.Rows[0]["Email"].ToString();
                    txtPhone.Text = userData.Rows[0]["Phone"].ToString();
                }
                else
                {
                    // Kullanıcı bilgileri yüklenemezse hata mesajı göster
                    MessageBox.Show("Kullanıcı bilgileri yüklenemedi.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda hata mesajı göster
                MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Alanları doğrulamak için metod
        private void ValidateFields()
        {
            // Boş alan kontrolü
            if (string.IsNullOrWhiteSpace(txtEmail.Text) || string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                throw new Exception("Tüm alanlar doldurulmalıdır."); // Hata fırlat
            }

            // E-posta formatını doğrula
            if (!System.Text.RegularExpressions.Regex.IsMatch(txtEmail.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                throw new Exception("Geçerli bir e-posta adresi giriniz."); // Hata fırlat
            }

            // Telefon numarası formatını doğrula (10 haneli, başında 0 olmadan)
            if (!System.Text.RegularExpressions.Regex.IsMatch(txtPhone.Text, @"^\d{10}$"))
            {
                throw new Exception("Telefon numarası 10 haneli olmalıdır (başında 0 olmadan)."); // Hata fırlat
            }

            // Eski şifre alanının boş olup olmadığını kontrol et
            if (string.IsNullOrEmpty(txtOldPassword.Text))
            {
                throw new Exception("Bilgilerde güncelleme yapmak için şifrenizi girmelisiniz."); // Hata fırlat
            }

            // Yeni şifre varsa kontrol et
            if (!string.IsNullOrEmpty(txtNewPassword.Text))
            {
                // Yeni şifre uzunluğunu kontrol et
                if (txtNewPassword.Text.Length < 8)
                    throw new Exception("Şifre en az 8 karakter uzunluğunda olmalıdır."); // Hata fırlat

                // Büyük harf kontrolü
                if (!txtNewPassword.Text.Any(char.IsUpper))
                    throw new Exception("Şifre en az bir büyük harf içermelidir."); // Hata fırlat

                // Küçük harf kontrolü
                if (!txtNewPassword.Text.Any(char.IsLower))
                    throw new Exception("Şifre en az bir küçük harf içermelidir."); // Hata fırlat

                // Rakam kontrolü
                if (!txtNewPassword.Text.Any(char.IsDigit))
                    throw new Exception("Şifre en az bir rakam içermelidir."); // Hata fırlat

                // Özel karakter kontrolü
                if (!txtNewPassword.Text.Any(ch => "!@#$%^&*()_+".Contains(ch)))
                    throw new Exception("Şifre en az bir özel karakter içermelidir (!@#$%^&*()_+)."); // Hata fırlat

                // Yeni şifrelerin eşleşip eşleşmediğini kontrol et
                if (txtNewPassword.Text != txtConfirmPassword.Text)
                    throw new Exception("Yeni şifreler uyuşmuyor."); // Hata fırlat
            }
        }

        // Hesap güncelleme butonuna tıklandığında çağrılan metod
        private void btnUpdateAccount_Click(object sender, EventArgs e)
        {
            try
            {
                ValidateFields(); // Alanları doğrula

                // Formdan alınan verileri değişkenlere ata
                string username = txtUsername.Text.Trim();
                string email = txtEmail.Text.Trim();
                string phone = txtPhone.Text.Trim();
                string oldPassword = txtOldPassword.Text.Trim();
                string newPassword = txtNewPassword.Text.Trim();

                DatabaseHelper db = new DatabaseHelper(); // Veritabanı yardımcı sınıfı

                // Eski şifreyi hashle
                string hashedPassword = PasswordHelper.HashPassword(oldPassword);
                SqlParameter[] authParameters = {
                        new SqlParameter("@Username", username),
                        new SqlParameter("@Password", hashedPassword) // Kullanıcı adı ve şifre parametreleri
                    };

                // Kullanıcıyı doğrula
                DataTable result = db.ExecuteQuery("AuthenticateUser", authParameters, true);

                // Kullanıcı doğrulandıysa
                if (result.Rows.Count > 0)
                {
                    // Güncelleme için parametreleri hazırla
                    SqlParameter[] updateParams = {
                            new SqlParameter("@Username", username),
                            new SqlParameter("@Email", email),
                            new SqlParameter("@Phone", phone)
                        };

                    // Kullanıcı bilgilerini güncelle
                    db.ExecuteNonQuery("UPDATE Users SET Email = @Email, Phone = @Phone WHERE Username = @Username", updateParams, false);

                    // Yeni şifre varsa güncelle
                    if (!string.IsNullOrEmpty(newPassword))
                    {
                        newPassword = PasswordHelper.HashPassword(newPassword); // Yeni şifreyi hashle
                        SqlParameter[] parameters = {
                                new SqlParameter("@UserID", userID),
                                new SqlParameter("@Username", currentUsername),
                                new SqlParameter("@PasswordHash", newPassword),
                                new SqlParameter("@Email", email),
                                new SqlParameter("@Phone", phone),
                                new SqlParameter("@Role", role),
                                new SqlParameter("@IsActive", isActive)
                            };

                        // Kullanıcıyı güncelle
                        db.ExecuteNonQuery("UpdateUser", parameters, true);

                        // Başarı mesajı fırlat
                        throw new Exception("Kullanıcı bilgileri başarıyla güncellendi.");
                    }
                }
                else
                {
                    // Eski şifre yanlışsa hata fırlat
                    throw new Exception("Eski şifrenizi yanlış girdiniz.");
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda hata mesajı göster
                MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // İptal butonuna tıklandığında çağrılan metod
        private void btnCancel_Click(object sender, EventArgs e)
        {
            FormHelper.NavigateBack(); // Geri git
        }

        // Geri butonuna tıklandığında çağrılan metod
        private void btnBack_Click(object sender, EventArgs e)
        {
            FormHelper.NavigateBack(); // Geri git
        }
    }
}
