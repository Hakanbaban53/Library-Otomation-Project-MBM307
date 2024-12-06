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
    public partial class UserManagementForm : Form
    {
        // Seçilen kullanıcı ID'si
        int selectedUserID = -1;

        // Formun yapıcı metodu
        public UserManagementForm()
        {
            InitializeComponent(); // Bileşenleri başlat
            LoadUsers(); // Kullanıcıları yükle
            UpdateUIState(); // UI durumunu güncelle
            cmbItemAdd(); // Kombinasyon kutusuna öğe ekle
        }

        // Kombinasyon kutusuna rol ve aktiflik durumunu ekler
        private void cmbItemAdd()
        {
            cmbRole.Items.Add("Admin"); // Admin rolü
            cmbRole.Items.Add("Librarian"); // Kütüphaneci rolü
            cmbIsActive.Items.Add("Aktif"); // 1
            cmbIsActive.Items.Add("Pasif"); // 0
        }

        // Veritabanından kullanıcıları yükler
        private void LoadUsers()
        {
            DatabaseHelper db = new DatabaseHelper(); // Veritabanı yardımcı sınıfı
            DataTable dt = db.ExecuteQuery("SELECT * FROM Users", null, false); // Kullanıcıları sorgula
            dataGridUsers.DataSource = dt; // Kullanıcıları veri ızgarasına ata
        }

        // Giriş alanlarını temizler
        private void clearFields()
        {
            txtUsername.Text = ""; // Kullanıcı adı
            txtPassword.Text = ""; // Şifre
            txtEmail.Text = ""; // E-posta
            txtPhone.Text = ""; // Telefon
            cmbRole.SelectedIndex = -1; // Rolü sıfırla
            cmbIsActive.SelectedIndex = -1; // Aktiflik durumunu sıfırla
        }

        // UI durumunu günceller
        private void UpdateUIState()
        {
            bool enableFields = chkNewUser.Checked || (chkUpdateUser.Checked && selectedUserID != -1); // Alanları etkinleştir
            txtUsername.Enabled = enableFields; // Kullanıcı adı alanı
            txtPassword.Enabled = enableFields; // Şifre alanı
            txtEmail.Enabled = enableFields; // E-posta alanı
            txtPhone.Enabled = enableFields; // Telefon alanı
            btnSave.Enabled = enableFields; // Kaydet butonu
            cmbRole.Enabled = enableFields; // Rol kombinasyon kutusu
            cmbIsActive.Enabled = enableFields; // Aktiflik kombinasyon kutusu
        }

        // Geri butonuna tıklandığında
        private void btnBack_Click(object sender, EventArgs e)
        {
            FormHelper.NavigateBack(); // Geri git
        }

        // Giriş alanlarını kontrol eder
        private void checkFiels()
        {
            // Tüm alanların dolu olup olmadığını kontrol et
            if (txtUsername.Text == "" || txtPassword.Text == "" || txtEmail.Text == "" || txtPhone.Text == "" || cmbRole.SelectedIndex == -1 || cmbIsActive.SelectedIndex == -1)
            {
                throw new Exception("Tüm alanlar doldurulmalıdır."); // Hata mesajı
            }
            // Şifre kurallarını kontrol et
            if (txtPassword.Text.Length < 8 || !txtPassword.Text.Any(char.IsUpper) || !txtPassword.Text.Any(char.IsLower) || !txtPassword.Text.Any(char.IsDigit) || !txtPassword.Text.Any(ch => "!@#$%^&*()_+".Contains(ch)))
            {
                throw new Exception("Şifre en az 8 karakter uzunluğunda olmalı, büyük harf, küçük harf, rakam ve özel karakter içermelidir."); // Hata mesajı
            }
            // Telefon numarasını kontrol et
            if (!System.Text.RegularExpressions.Regex.IsMatch(txtPhone.Text, @"^\d{10}$"))
            {
                throw new Exception("Geçerli bir telefon numararı giriniz (10 Haneli telefon numaranızı başında '0' olmadan girin.)."); // Hata mesajı
            }

            // E-posta adresini kontrol et
            if (!System.Text.RegularExpressions.Regex.IsMatch(txtEmail.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                throw new Exception("Geçerli bir e-posta adresi giriniz."); // Hata mesajı
            }
        }

        // Veri ızgarasında bir hücreye tıklandığında
        private void dataGridUsers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Geçerli bir satır seçildiyse
            {
                DataGridViewRow row = dataGridUsers.Rows[e.RowIndex]; // Seçilen satırı al
                selectedUserID = (int)row.Cells["UserID"].Value; // Kullanıcı ID'sini al
                txtUsername.Text = row.Cells["Username"].Value.ToString(); // Kullanıcı adını al
                txtPassword.Text = row.Cells["PasswordHash"].Value.ToString(); // Şifreyi al
                txtEmail.Text = row.Cells["Email"].Value.ToString(); // E-postayı al
                txtPhone.Text = row.Cells["Phone"].Value.ToString(); // Telefonu al
                cmbRole.Text = row.Cells["Role"].Value.ToString(); // Rolü al
                cmbIsActive.Text = row.Cells["IsActive"].Value.ToString() == "1" ? "Aktif" : "Pasif"; // Aktiflik durumunu al
            }
            UpdateUIState(); // UI durumunu güncelle
        }

        // Kaydet butonuna tıklandığında
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                checkFiels(); // Alanları kontrol et
                string username = txtUsername.Text; // Kullanıcı adı
                string password = txtPassword.Text; // Şifre
                string email = txtEmail.Text; // E-posta
                string phone = txtPhone.Text; // Telefon
                string role = cmbRole.Text.Trim(); // Rol
                string IsActive = cmbIsActive.Text.Trim() == "Aktif" ? "1" : "0"; // Aktiflik durumunu bit'e çevir

                DatabaseHelper db = new DatabaseHelper(); // Veritabanı yardımcı sınıfı
                SqlParameter[] parameters; // SQL parametreleri

                password = PasswordHelper.HashPassword(password); // Şifreyi hashle

                // Yeni kullanıcı ekleme
                if (chkNewUser.Checked)
                {
                    parameters = new SqlParameter[] {
                            new SqlParameter("username", username),
                            new SqlParameter("password", password),
                            new SqlParameter("email", email),
                            new SqlParameter("phone", phone),
                            new SqlParameter("role", role),
                            new SqlParameter("IsActive", IsActive)
                        };
                    db.ExecuteNonQuery("AddNewUser", parameters); // Yeni kullanıcı ekle
                }
                // Kullanıcı güncelleme
                else if (chkUpdateUser.Checked)
                {
                    int userID = selectedUserID; // Seçilen kullanıcı ID'si
                    parameters = new SqlParameter[] {
                            new SqlParameter("userID", userID),
                            new SqlParameter("username", username),
                            new SqlParameter("password", password),
                            new SqlParameter("email", email),
                            new SqlParameter("phone", phone),
                            new SqlParameter("role", role),
                            new SqlParameter("IsActive", IsActive)
                        };
                    db.ExecuteNonQuery("UpdateUser", parameters); // Kullanıcıyı güncelle
                }
                LoadUsers(); // Kullanıcıları yeniden yükle
                clearFields(); // Alanları temizle
                UpdateUIState(); // UI durumunu güncelle
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); // Hata mesajını göster
            }
        }

        // Kullanıcı silme butonuna tıklandığında
        private void btnDeleteUser_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedUserID == -1) // Kullanıcı seçilmediyse
                {
                    MessageBox.Show("Lütfen silmek istediğiniz kullanıcıyı seçiniz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning); // Uyarı mesajı
                    return; // Çık
                }

                string inputPassword = PromptForPassword(); // Şifreyi isteme
                inputPassword = PasswordHelper.HashPassword(inputPassword); // Şifreyi hashle
                if (string.IsNullOrEmpty(inputPassword)) // Şifre boşsa
                {
                    MessageBox.Show("Silme işlemi iptal edildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information); // Bilgi mesajı
                    return; // Çık
                }

                DatabaseHelper db = new DatabaseHelper(); // Veritabanı yardımcı sınıfı
                SqlParameter[] parameters = new SqlParameter[] // SQL parametreleri
                {
                        new SqlParameter("@UserID", selectedUserID), // Kullanıcı ID'si
                        new SqlParameter("@Username", txtUsername.Text), // Kullanıcı adı
                        new SqlParameter("@Password", inputPassword) // Şifre
                };

                db.ExecuteNonQuery("DeleteUser", parameters); // Kullanıcıyı sil
                LoadUsers(); // Kullanıcıları yeniden yükle
                clearFields(); // Alanları temizle
                UpdateUIState(); // UI durumunu güncelle
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); // Hata mesajını göster
            }
        }

        // Şifre girişi için formu açar
        private string PromptForPassword()
        {
            using (Form passwordForm = new Form()
            {
                Text = "Şifre Girişi",
                StartPosition = FormStartPosition.CenterParent,
                Width = 300,
                Height = 200,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                Padding = new Padding(10)
            })
            {
                Label label = new Label() { Text = "Silmek için kullanıcının şifresini girin:", Dock = DockStyle.Top, Font = new Font("Microsoft Sans Serif", 12) }; // Açıklama etiketi
                TextBox textBox = new TextBox() { Dock = DockStyle.Top, PasswordChar = '*', Font = new Font("Microsoft Sans Serif", 12) }; // Şifre girişi
                Button confirmation = new Button() { Text = "Tamam", Dock = DockStyle.Bottom, Height = 30, DialogResult = DialogResult.OK, Font = new Font("Microsoft Sans Serif", 12) }; // Tamam butonu
                Button cancel = new Button() { Text = "İptal", Dock = DockStyle.Bottom, Height = 30, DialogResult = DialogResult.Cancel, Font = new Font("Microsoft Sans Serif", 12) }; // İptal butonu

                // Form bileşenlerini ekle
                passwordForm.Controls.Add(textBox);
                passwordForm.Controls.Add(confirmation);
                passwordForm.Controls.Add(cancel);
                passwordForm.Controls.Add(label);
                passwordForm.AcceptButton = confirmation; // Tamam butonunu onayla
                passwordForm.CancelButton = cancel; // İptal butonunu iptal et

                if (passwordForm.ShowDialog() == DialogResult.OK) // Tamam butonuna tıklandıysa
                {
                    return textBox.Text; // Şifreyi döndür
                }
                return string.Empty; // Boş döndür
            }
        }

        // Güncelleme kullanıcı kutusu değiştiğinde
        private void chkUpdateUser_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUpdateUser.Checked) // Güncelleme kutusu işaretlendiyse
            {
                chkNewUser.Checked = false; // Yeni kullanıcı kutusunu kaldır
            }
            UpdateUIState(); // UI durumunu güncelle
        }

        // Yeni kullanıcı kutusu değiştiğinde
        private void chkNewUser_CheckedChanged(object sender, EventArgs e)
        {
            if (chkNewUser.Checked) // Yeni kullanıcı kutusu işaretlendiyse
            {
                chkUpdateUser.Checked = false; // Güncelleme kutusunu kaldır
                dataGridUsers.ClearSelection(); // Veri ızgarasındaki seçimi temizle
                selectedUserID = -1; // Seçilen kullanıcıyı sıfırla
                clearFields(); // Alanları temizle
            }
            UpdateUIState(); // UI durumunu güncelle
        }

        // Temizle butonuna tıklandığında
        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                clearFields(); // Alanları temizle
                chkNewUser.Checked = false; // Yeni kullanıcı kutusunu kaldır
                chkUpdateUser.Checked = false; // Güncelleme kutusunu kaldır
                selectedUserID = -1; // Seçilen kullanıcıyı sıfırla
                UpdateUIState(); // UI durumunu güncelle
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); // Hata mesajını göster 
            }
        }
    }
}
