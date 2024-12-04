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
        private string currentUsername;
        private int userID;
        private string role;
        private bool isActive;


        public ManageAccountForm(string username)
        {
            InitializeComponent();
            currentUsername = username;
        }

        private void ManageAccountForm_Load(object sender, EventArgs e)
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
                    MessageBox.Show("Kullanıcı bilgileri yüklenemedi.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        private void ValidateFields()
        {
            // Check for empty fields
            if (string.IsNullOrWhiteSpace(txtEmail.Text) || string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                throw new Exception("Tüm alanlar doldurulmalıdır.");
            }

            // Validate email format
            if (!System.Text.RegularExpressions.Regex.IsMatch(txtEmail.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                throw new Exception("Geçerli bir e-posta adresi giriniz.");
            }

            // Validate phone number format (10 digits without leading zero)
            if (!System.Text.RegularExpressions.Regex.IsMatch(txtPhone.Text, @"^\d{10}$"))
            {
                throw new Exception("Telefon numarası 10 haneli olmalıdır (başında 0 olmadan).");
            }
            
            if (string.IsNullOrEmpty(txtOldPassword.Text))
            {
                throw new Exception("Bilgilerde güncelleme yapmak için şifrenizi girmelisiniz.");
            }

            // Check password only if it's provided (for update)
            if (!string.IsNullOrEmpty(txtNewPassword.Text))
            {
                if (txtNewPassword.Text.Length < 8)
                    throw new Exception("Şifre en az 8 karakter uzunluğunda olmalıdır.");

                if (!txtNewPassword.Text.Any(char.IsUpper))
                    throw new Exception("Şifre en az bir büyük harf içermelidir.");

                if (!txtNewPassword.Text.Any(char.IsLower))
                    throw new Exception("Şifre en az bir küçük harf içermelidir.");

                if (!txtNewPassword.Text.Any(char.IsDigit))
                    throw new Exception("Şifre en az bir rakam içermelidir.");

                if (!txtNewPassword.Text.Any(ch => "!@#$%^&*()_+".Contains(ch)))
                    throw new Exception("Şifre en az bir özel karakter içermelidir (!@#$%^&*()_+).");

                if (txtNewPassword.Text != txtConfirmPassword.Text)
                    throw new Exception("Yeni şifreler uyuşmuyor.");
            }
        }

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

                DatabaseHelper db = new DatabaseHelper();

                string hashedPassword = PasswordHelper.HashPassword(oldPassword);
                SqlParameter[] authParameters = {
                    new SqlParameter("@Username", username),
                    new SqlParameter("@Password", hashedPassword)
                };

                DataTable result = db.ExecuteQuery("AuthenticateUser", authParameters, true);


                if (result.Rows.Count > 0)
                {
                    SqlParameter[] updateParams = {
                        new SqlParameter("@Username", username),
                        new SqlParameter("@Email", email),
                        new SqlParameter("@Phone", phone)
                    };

                    db.ExecuteNonQuery("UPDATE Users SET Email = @Email, Phone = @Phone WHERE Username = @Username", updateParams, false);

                    if (!string.IsNullOrEmpty(newPassword))
                    {
                        newPassword = PasswordHelper.HashPassword(newPassword);
                        SqlParameter[] parameters = {
                            new SqlParameter("@UserID", userID),
                            new SqlParameter("@Username", currentUsername),
                            new SqlParameter("@PasswordHash", newPassword),
                            new SqlParameter("@Email", email),
                            new SqlParameter("@Phone", phone),
                            new SqlParameter("@Role", role),
                            new SqlParameter("@IsActive", isActive)
                        };

                        db.ExecuteNonQuery("UpdateUser", parameters, true);


                        throw new Exception("Kullanıcı bilgileri başarıyla güncellendi.");
                }
                else
                {
                    throw new Exception("Eski şifrenizi yanlış girdiniz.");
                }
                }
            }


            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            FormHelper.NavigateBack();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            FormHelper.NavigateBack();
        }

    }
}
