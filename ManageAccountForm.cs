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
        // Kullanıcı bilgilerini saklamak için değişkenler / Variables to store user information
        private string currentUsername;
        private int userID;
        private string role;
        private bool isActive;

        // Yapıcı metod, kullanıcı adını alır / Constructor method that takes the username
        public ManageAccountForm(string username)
        {
            InitializeComponent();
            currentUsername = username;
        }

        // Form yüklendiğinde çağrılan metod / Method called when the form loads
        private void ManageAccountForm_Load(object sender, EventArgs e)
        {
            LoadUserData();
        }

        private void LoadUserData()
        {
            try
            {
                DatabaseHelper db = new DatabaseHelper();
                SqlParameter[] parameters = { new SqlParameter("@Username", currentUsername) };

                DataTable userData = db.ExecuteQuery("SELECT UserID, Role, IsActive, Email, Phone FROM Users WHERE Username = @Username", parameters, false);

                if (userData.Rows.Count > 0)
                {
                    userID = Convert.ToInt32(userData.Rows[0]["UserID"]);
                    role = userData.Rows[0]["Role"].ToString();
                    isActive = Convert.ToBoolean(userData.Rows[0]["IsActive"]);

                    txtUsername.Text = currentUsername;
                    txtEmail.Text = userData.Rows[0]["Email"].ToString();
                    txtPhone.Text = userData.Rows[0]["Phone"].ToString();
                }
                else
                {
                    ShowError("Kullanıcı bilgileri yüklenemedi."); // User information could not be loaded.  
                }
            }
            catch (Exception ex)
            {
                ShowError($"Bir hata oluştu: {ex.Message}"); // An error occurred: {error message}.  
            }
        }

        private void ShowError(string message)
        {
            MessageBox.Show(message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); // Show error message.  
        }

        // Alanları doğrulamak için metod / Method to validate fields
        private void ValidateFields()
        {
            if (string.IsNullOrWhiteSpace(txtEmail.Text) || string.IsNullOrWhiteSpace(txtPhone.Text))
                throw new Exception("Tüm alanlar doldurulmalıdır."); // All fields must be filled.  

            if (!System.Text.RegularExpressions.Regex.IsMatch(txtEmail.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new Exception("Geçerli bir e-posta adresi giriniz."); // Please enter a valid email address.  

            if (!System.Text.RegularExpressions.Regex.IsMatch(txtPhone.Text, @"^\d{10}$"))
                throw new Exception("Telefon numarası 10 haneli olmalıdır (başında 0 olmadan)."); // Phone number must be 10 digits (without leading 0).  

            if (string.IsNullOrEmpty(txtOldPassword.Text))
                throw new Exception("Bilgilerde güncelleme yapmak için şifrenizi girmelisiniz."); // You must enter your password to update information.  

            ValidateNewPassword();
        }

        private void ValidateNewPassword()
        {
            if (!string.IsNullOrEmpty(txtNewPassword.Text))
            {
                if (txtNewPassword.Text.Length < 8)
                    throw new Exception("Şifre en az 8 karakter uzunluğunda olmalıdır."); // Password must be at least 8 characters long.  

                if (!txtNewPassword.Text.Any(char.IsUpper))
                    throw new Exception("Şifre en az bir büyük harf içermelidir."); // Password must contain at least one uppercase letter.  

                if (!txtNewPassword.Text.Any(char.IsLower))
                    throw new Exception("Şifre en az bir küçük harf içermelidir."); // Password must contain at least one lowercase letter.  

                if (!txtNewPassword.Text.Any(char.IsDigit))
                    throw new Exception("Şifre en az bir rakam içermelidir."); // Password must contain at least one digit.  

                if (!txtNewPassword.Text.Any(ch => "!@#$%^&*()_+".Contains(ch)))
                    throw new Exception("Şifre en az bir özel karakter içermelidir (!@#$%^&*()_+)."); // Password must contain at least one special character (!@#$%^&*()_+).  

                if (txtNewPassword.Text != txtConfirmPassword.Text)
                    throw new Exception("Yeni şifreler uyuşmuyor."); // New passwords do not match.  
            }
        }

        // Hesap güncelleme butonuna tıklandığında çağrılan metod / Method called when the update account button is clicked
        private void btnUpdateAccount_Click(object sender, EventArgs e)
        {
            try
            {
                ValidateFields();

                string username = txtUsername.Text.Trim();
                string email = txtEmail.Text.Trim();
                string phone = txtPhone.Text.Trim();
                string oldPassword = txtOldPassword.Text.Trim();
                string newPassword = txtNewPassword.Text.Trim();

                UpdateUserAccount(username, email, phone, oldPassword, newPassword);
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        private void UpdateUserAccount(string username, string email, string phone, string oldPassword, string newPassword)
        {
            DatabaseHelper db = new DatabaseHelper();
            string hashedPassword = PasswordHelper.HashPassword(oldPassword);
            SqlParameter[] authParameters = {
                    new SqlParameter("@Username", username),
                    new SqlParameter("@Password", hashedPassword)
                };

            DataTable result = db.ExecuteQuery("AuthenticateUser", authParameters, true);

            if (result.Rows.Count > 0)
            {
                UpdateUserInfo(username, email, phone);
                if (!string.IsNullOrEmpty(newPassword))
                {
                    UpdateUserPassword(newPassword);
                }
                MessageBox.Show("Hesap bilgileriniz başarıyla güncellendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information); // Your account information has been successfully updated.  
            }
            else
            {
                throw new Exception("Eski şifrenizi yanlış girdiniz."); // You entered your old password incorrectly.  
            }
        }

        private void UpdateUserInfo(string username, string email, string phone)
        {
            SqlParameter[] updateParams = {
                    new SqlParameter("@Username", username),
                    new SqlParameter("@Email", email),
                    new SqlParameter("@Phone", phone)
                };

            DatabaseHelper db = new DatabaseHelper();
            db.ExecuteNonQuery("UPDATE Users SET Email = @Email, Phone = @Phone WHERE Username = @Username", updateParams, false);
        }

        private void UpdateUserPassword(string newPassword)
        {
            newPassword = PasswordHelper.HashPassword(newPassword);
            SqlParameter[] parameters = {
                    new SqlParameter("@UserID", userID),
                    new SqlParameter("@Username", currentUsername),
                    new SqlParameter("@Password", newPassword),
                    new SqlParameter("@Email", txtEmail.Text.Trim()),
                    new SqlParameter("@Phone", txtPhone.Text.Trim()),
                    new SqlParameter("@Role", role),
                    new SqlParameter("@IsActive", isActive)
                };

            DatabaseHelper db = new DatabaseHelper();
            db.ExecuteNonQuery("UpdateUser", parameters, true);
        }

        // İptal butonuna tıklandığında çağrılan metod / Method called when the cancel button is clicked
        private void btnCancel_Click(object sender, EventArgs e)
        {
            FormHelper.NavigateBack();
        }

        // Geri butonuna tıklandığında çağrılan metod / Method called when the back button is clicked
        private void btnBack_Click(object sender, EventArgs e)
        {
            FormHelper.NavigateBack();
        }
    }
}
