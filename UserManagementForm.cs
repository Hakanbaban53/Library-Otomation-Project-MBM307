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
        // Seçilen kullanıcı ID'si / Selected user ID
        int selectedUserID = -1;

        // Formun yapıcı metodu / Constructor of the form
        public UserManagementForm()
        {
            InitializeComponent(); // Bileşenleri başlat / Initialize components
            LoadUsers(); // Kullanıcıları yükle / Load users
            UpdateUIState(); // UI durumunu güncelle / Update UI state
            cmbItemAdd(); // Kombinasyon kutusuna öğe ekle / Add items to the combo box
        }

        // Kombinasyon kutusuna rol ve aktiflik durumunu ekler / Adds role and active status to the combo box
        private void cmbItemAdd()
        {
            cmbRole.Items.Add("Admin"); // Admin rolü / Admin role
            cmbRole.Items.Add("Librarian"); // Kütüphaneci rolü / Librarian role
            cmbIsActive.Items.Add("Aktif"); // 1 / Active
            cmbIsActive.Items.Add("Pasif"); // 0 / Inactive
        }

        // Veritabanından kullanıcıları yükler / Loads users from the database
        private void LoadUsers()
        {
            DatabaseHelper db = new DatabaseHelper(); // Veritabanı yardımcı sınıfı / Database helper class
            DataTable dt = db.ExecuteQuery("SELECT * FROM Users", null, false); // Kullanıcıları sorgula / Query users
            dataGridUsers.DataSource = dt; // Kullanıcıları veri ızgarasına ata / Assign users to the data grid
        }

        // Giriş alanlarını temizler / Clears input fields
        private void clearFields()
        {
            txtUsername.Text = ""; // Kullanıcı adı / Username
            txtPassword.Text = ""; // Şifre / Password
            txtEmail.Text = ""; // E-posta / Email
            txtPhone.Text = ""; // Telefon / Phone
            cmbRole.SelectedIndex = -1; // Rolü sıfırla / Reset role
            cmbIsActive.SelectedIndex = -1; // Aktiflik durumunu sıfırla / Reset active status
        }

        // UI durumunu günceller / Updates UI state
        private void UpdateUIState()
        {
            bool enableFields = chkNewUser.Checked || (chkUpdateUser.Checked && selectedUserID != -1); // Alanları etkinleştir / Enable fields
            txtUsername.Enabled = enableFields; // Kullanıcı adı alanı / Username field
            txtPassword.Enabled = enableFields; // Şifre alanı / Password field
            txtEmail.Enabled = enableFields; // E-posta alanı / Email field
            txtPhone.Enabled = enableFields; // Telefon alanı / Phone field
            btnSave.Enabled = enableFields; // Kaydet butonu / Save button
            cmbRole.Enabled = enableFields; // Rol kombinasyon kutusu / Role combo box
            cmbIsActive.Enabled = enableFields; // Aktiflik kombinasyon kutusu / Active combo box
        }

        // Geri butonuna tıklandığında / When the back button is clicked
        private void btnBack_Click(object sender, EventArgs e)
        {
            FormHelper.NavigateBack(); // Geri git / Navigate back
        }

        // Giriş alanlarını kontrol eder / Checks input fields
        private void checkFiels()
        {
            // Tüm alanların dolu olup olmadığını kontrol et / Check if all fields are filled
            if (txtUsername.Text == "" || txtPassword.Text == "" || txtEmail.Text == "" || txtPhone.Text == "" || cmbRole.SelectedIndex == -1 || cmbIsActive.SelectedIndex == -1)
            {
                throw new Exception("Tüm alanlar doldurulmalıdır."); // Hata mesajı / Error message
            }
            // Şifre kurallarını kontrol et / Check password rules
            if (txtPassword.Text.Length < 8 || !txtPassword.Text.Any(char.IsUpper) || !txtPassword.Text.Any(char.IsLower) || !txtPassword.Text.Any(char.IsDigit) || !txtPassword.Text.Any(ch => "!@#$%^&*()_+".Contains(ch)))
            {
                throw new Exception("Şifre en az 8 karakter uzunluğunda olmalı, büyük harf, küçük harf, rakam ve özel karakter içermelidir."); // Hata mesajı / Error message
            }
            // Telefon numarasını kontrol et / Check phone number
            if (!System.Text.RegularExpressions.Regex.IsMatch(txtPhone.Text, @"^\d{10}$"))
            {
                throw new Exception("Geçerli bir telefon numararı giriniz (10 Haneli telefon numaranızı başında '0' olmadan girin.)."); // Hata mesajı / Error message
            }

            // E-posta adresini kontrol et / Check email address
            if (!System.Text.RegularExpressions.Regex.IsMatch(txtEmail.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                throw new Exception("Geçerli bir e-posta adresi giriniz."); // Hata mesajı / Error message
            }
        }

        // Veri ızgarasında bir hücreye tıklandığında / When a cell in the data grid is clicked
        private void dataGridUsers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Geçerli bir satır seçildiyse / If a valid row is selected
            {
                DataGridViewRow row = dataGridUsers.Rows[e.RowIndex]; // Seçilen satırı al / Get the selected row
                selectedUserID = (int)row.Cells["UserID"].Value; // Kullanıcı ID'sini al / Get user ID
                txtUsername.Text = row.Cells["Username"].Value.ToString(); // Kullanıcı adını al / Get username
                txtPassword.Text = row.Cells["PasswordHash"].Value.ToString(); // Şifreyi al / Get password
                txtEmail.Text = row.Cells["Email"].Value.ToString(); // E-postayı al / Get email
                txtPhone.Text = row.Cells["Phone"].Value.ToString(); // Telefonu al / Get phone
                cmbRole.Text = row.Cells["Role"].Value.ToString(); // Rolü al / Get role
                cmbIsActive.Text = row.Cells["IsActive"].Value.ToString() == "1" ? "Aktif" : "Pasif"; // Aktiflik durumunu al / Get active status
            }
            UpdateUIState(); // UI durumunu güncelle / Update UI state
        }

        // Kaydet butonuna tıklandığında / When the save button is clicked
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                checkFiels(); // Alanları kontrol et / Check fields
                string username = txtUsername.Text; // Kullanıcı adı / Username
                string password = txtPassword.Text; // Şifre / Password
                string email = txtEmail.Text; // E-posta / Email
                string phone = txtPhone.Text; // Telefon / Phone
                string role = cmbRole.Text.Trim(); // Rol / Role
                string IsActive = cmbIsActive.Text.Trim() == "Aktif" ? "1" : "0"; // Aktiflik durumunu bit'e çevir / Convert active status to bit

                DatabaseHelper db = new DatabaseHelper(); // Veritabanı yardımcı sınıfı / Database helper class
                SqlParameter[] parameters; // SQL parametreleri / SQL parameters

                password = PasswordHelper.HashPassword(password); // Şifreyi hashle / Hash the password

                // Yeni kullanıcı ekleme / Adding a new user
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
                    db.ExecuteNonQuery("AddNewUser", parameters); // Yeni kullanıcı ekle / Add new user
                }
                // Kullanıcı güncelleme / Updating user
                else if (chkUpdateUser.Checked)
                {
                    int userID = selectedUserID; // Seçilen kullanıcı ID'si / Selected user ID
                    parameters = new SqlParameter[] {
                                new SqlParameter("userID", userID),
                                new SqlParameter("username", username),
                                new SqlParameter("password", password),
                                new SqlParameter("email", email),
                                new SqlParameter("phone", phone),
                                new SqlParameter("role", role),
                                new SqlParameter("IsActive", IsActive)
                            };
                    db.ExecuteNonQuery("UpdateUser", parameters); // Kullanıcıyı güncelle / Update user
                }
                LoadUsers(); // Kullanıcıları yeniden yükle / Reload users
                clearFields(); // Alanları temizle / Clear fields
                UpdateUIState(); // UI durumunu güncelle / Update UI state
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); // Hata mesajını göster / Show error message
            }
        }

        // Kullanıcı silme butonuna tıklandığında / When the delete user button is clicked
        private void btnDeleteUser_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedUserID == -1) // Kullanıcı seçilmediyse / If no user is selected
                {
                    MessageBox.Show("Lütfen silmek istediğiniz kullanıcıyı seçiniz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning); // Uyarı mesajı / Warning message
                    return; // Çık / Exit
                }

                string inputPassword = PromptForPassword(); // Şifreyi isteme / Prompt for password
                inputPassword = PasswordHelper.HashPassword(inputPassword); // Şifreyi hashle / Hash the password
                if (string.IsNullOrEmpty(inputPassword)) // Şifre boşsa / If password is empty
                {
                    MessageBox.Show("Silme işlemi iptal edildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information); // Bilgi mesajı / Information message
                    return; // Çık / Exit
                }

                DatabaseHelper db = new DatabaseHelper(); // Veritabanı yardımcı sınıfı / Database helper class
                SqlParameter[] parameters = new SqlParameter[] // SQL parametreleri / SQL parameters
                {
                            new SqlParameter("@UserID", selectedUserID), // Kullanıcı ID'si / User ID
                            new SqlParameter("@Username", txtUsername.Text), // Kullanıcı adı / Username
                            new SqlParameter("@Password", inputPassword) // Şifre / Password
                };

                db.ExecuteNonQuery("DeleteUser", parameters); // Kullanıcıyı sil / Delete user
                LoadUsers(); // Kullanıcıları yeniden yükle / Reload users
                clearFields(); // Alanları temizle / Clear fields
                UpdateUIState(); // UI durumunu güncelle / Update UI state
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); // Hata mesajını göster / Show error message
            }
        }

        // Şifre girişi için formu açar / Opens a form for password entry
        private string PromptForPassword()
        {
            using (Form passwordForm = new Form()
            {
                Text = "Şifre Girişi", // Password Entry
                StartPosition = FormStartPosition.CenterParent,
                Width = 300,
                Height = 200,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                Padding = new Padding(10)
            })
            {
                Label label = new Label() { Text = "Silmek için kullanıcının şifresini girin:", Dock = DockStyle.Top, Font = new Font("Microsoft Sans Serif", 12) }; // Açıklama etiketi / Description label
                TextBox textBox = new TextBox() { Dock = DockStyle.Top, PasswordChar = '*', Font = new Font("Microsoft Sans Serif", 12) }; // Şifre girişi / Password input
                Button confirmation = new Button() { Text = "Tamam", Dock = DockStyle.Bottom, Height = 30, DialogResult = DialogResult.OK, Font = new Font("Microsoft Sans Serif", 12) }; // Tamam butonu / Confirm button
                Button cancel = new Button() { Text = "İptal", Dock = DockStyle.Bottom, Height = 30, DialogResult = DialogResult.Cancel, Font = new Font("Microsoft Sans Serif", 12) }; // İptal butonu / Cancel button

                // Form bileşenlerini ekle / Add form components
                passwordForm.Controls.Add(textBox);
                passwordForm.Controls.Add(confirmation);
                passwordForm.Controls.Add(cancel);
                passwordForm.Controls.Add(label);
                passwordForm.AcceptButton = confirmation; // Tamam butonunu onayla / Confirm the OK button
                passwordForm.CancelButton = cancel; // İptal butonunu iptal et / Cancel the cancel button

                if (passwordForm.ShowDialog() == DialogResult.OK) // Tamam butonuna tıklandıysa / If OK button is clicked
                {
                    return textBox.Text; // Şifreyi döndür / Return the password
                }
                return string.Empty; // Boş döndür / Return empty
            }
        }

        // Güncelleme kullanıcı kutusu değiştiğinde / When the update user checkbox changes
        private void chkUpdateUser_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUpdateUser.Checked) // Güncelleme kutusu işaretlendiyse / If update checkbox is checked
            {
                chkNewUser.Checked = false; // Yeni kullanıcı kutusunu kaldır / Uncheck new user checkbox
            }
            UpdateUIState(); // UI durumunu güncelle / Update UI state
        }

        // Yeni kullanıcı kutusu değiştiğinde / When the new user checkbox changes
        private void chkNewUser_CheckedChanged(object sender, EventArgs e)
        {
            if (chkNewUser.Checked) // Yeni kullanıcı kutusu işaretlendiyse / If new user checkbox is checked
            {
                chkUpdateUser.Checked = false; // Güncelleme kutusunu kaldır / Uncheck update checkbox
                dataGridUsers.ClearSelection(); // Veri ızgarasındaki seçimi temizle / Clear selection in the data grid
                selectedUserID = -1; // Seçilen kullanıcıyı sıfırla / Reset selected user
                clearFields(); // Alanları temizle / Clear fields
            }
            UpdateUIState(); // UI durumunu güncelle / Update UI state
        }

        // Temizle butonuna tıklandığında / When the clear button is clicked
        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                clearFields(); // Alanları temizle / Clear fields
                chkNewUser.Checked = false; // Yeni kullanıcı kutusunu kaldır / Uncheck new user checkbox
                chkUpdateUser.Checked = false; // Güncelleme kutusunu kaldır / Uncheck update checkbox
                selectedUserID = -1; // Seçilen kullanıcıyı sıfırla / Reset selected user
                UpdateUIState(); // UI durumunu güncelle / Update UI state
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); // Hata mesajını göster / Show error message
            }
        }
    }
}
