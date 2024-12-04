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
        int selectedUserID = -1;
        public UserManagementForm()
        {
            InitializeComponent();
            LoadUsers();
            UpdateUIState();
            cmbItemAdd();

        }

        private void cmbItemAdd()
        {
            cmbRole.Items.Add("Admin");
            cmbRole.Items.Add("Librarian");
            cmbIsActive.Items.Add("Aktif"); // 1
            cmbIsActive.Items.Add("Pasif"); // 0
        }

        private void LoadUsers()
        {
            // Load users from database
            DatabaseHelper db = new DatabaseHelper();
            DataTable dt = db.ExecuteQuery("SELECT * FROM Users", null, false);
            dataGridUsers.DataSource = dt;
        }

        private void clearFields()
        {
            txtUsername.Text = "";
            txtPassword.Text = "";
            txtEmail.Text = "";
            txtPhone.Text = "";
            cmbRole.SelectedIndex = -1;
            cmbIsActive.SelectedIndex = -1;
        }

        private void UpdateUIState()
        {
            bool enableFields = chkNewUser.Checked || (chkUpdateUser.Checked && selectedUserID != -1);
            txtUsername.Enabled = enableFields;
            txtPassword.Enabled = enableFields;
            txtEmail.Enabled = enableFields;
            txtPhone.Enabled = enableFields;
            btnSave.Enabled = enableFields;
            cmbRole.Enabled = enableFields;
            cmbIsActive.Enabled = enableFields;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            FormHelper.NavigateBack();
        }

        private void checkFiels()
        {
            if (txtUsername.Text == "" || txtPassword.Text == "" || txtEmail.Text == "" || txtPhone.Text == "" || cmbRole.SelectedIndex == -1 || cmbIsActive.SelectedIndex == -1)
            {
                throw new Exception("Tüm alanlar doldurulmalıdır.");
            }
            if (txtPassword.Text.Length < 8 || !txtPassword.Text.Any(char.IsUpper) || !txtPassword.Text.Any(char.IsLower) || !txtPassword.Text.Any(char.IsDigit) || !txtPassword.Text.Any(ch => "!@#$%^&*()_+".Contains(ch)))
            {
                throw new Exception("Şifre en az 8 karakter uzunluğunda olmalı, büyük harf, küçük harf, rakam ve özel karakter içermelidir.");
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(txtPhone.Text, @"^\d{10}$"))
            {
                throw new Exception("Geçerli bir telefon numararı giriniz (10 Haneli telefon numaranızı başında '0' olmadan girin.).");
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(txtEmail.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                throw new Exception("Geçerli bir e-posta adresi giriniz.");
            }
        }

        private void dataGridUsers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridUsers.Rows[e.RowIndex];
                selectedUserID = (int)row.Cells["UserID"].Value;
                txtUsername.Text = row.Cells["Username"].Value.ToString();
                txtPassword.Text = row.Cells["PasswordHash"].Value.ToString();
                txtEmail.Text = row.Cells["Email"].Value.ToString();
                txtPhone.Text = row.Cells["Phone"].Value.ToString();
                cmbRole.Text = row.Cells["Role"].Value.ToString();
                cmbIsActive.Text = row.Cells["IsActive"].Value.ToString() == "1" ? "Aktif" : "Pasif" ;
            }
            UpdateUIState();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                checkFiels();
                string username = txtUsername.Text;
                string password = txtPassword.Text;
                string email = txtEmail.Text;
                string phone = txtPhone.Text;
                string role = cmbRole.Text.Trim();
                string IsActive = cmbIsActive.Text.Trim() == "Aktif" ? "1" : "0"; // Convert status to bit

                DatabaseHelper db = new DatabaseHelper();
                SqlParameter[] parameters;

                password = PasswordHelper.HashPassword(password);

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
                    db.ExecuteNonQuery("AddNewUser", parameters);
                }
                else if (chkUpdateUser.Checked)
                {
                    int userID = selectedUserID;
                    parameters = new SqlParameter[] {
                        new SqlParameter("userID", userID),
                        new SqlParameter("username", username),
                        new SqlParameter("password", password),
                        new SqlParameter("email", email),
                        new SqlParameter("phone", phone),
                        new SqlParameter("role", role),
                        new SqlParameter("IsActive", IsActive)
                    };
                    db.ExecuteNonQuery("UpdateUser", parameters);
                }
                LoadUsers();
                clearFields();
                UpdateUIState();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDeleteUser_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedUserID == -1)
                {
                    MessageBox.Show("Lütfen silmek istediğiniz kullanıcıyı seçiniz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string inputPassword = PromptForPassword();
                inputPassword = PasswordHelper.HashPassword(inputPassword);
                if (string.IsNullOrEmpty(inputPassword))
                {
                    MessageBox.Show("Silme işlemi iptal edildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                DatabaseHelper db = new DatabaseHelper();
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@UserID", selectedUserID),
                    new SqlParameter("@Username", txtUsername.Text),
                    new SqlParameter("@Password", inputPassword)
                };

                db.ExecuteNonQuery("DeleteUser", parameters);
                LoadUsers();
                clearFields();
                UpdateUIState();
               
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string PromptForPassword()
        {
            using (Form passwordForm = new Form() { Text = "Şifre Girişi", StartPosition = FormStartPosition.CenterParent, Width = 300, Height = 200,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                Padding = new Padding(10)     
            })
            {
                Label label = new Label() { Text = "Silmek için kullanıcının şifresini girin:", Dock = DockStyle.Top, Font = new Font("Microsoft Sans Serif", 12) };
                TextBox textBox = new TextBox() { Dock = DockStyle.Top, PasswordChar = '*', Font = new Font("Microsoft Sans Serif", 12) };
                Button confirmation = new Button() { Text = "Tamam", Dock = DockStyle.Bottom, Height = 30, DialogResult = DialogResult.OK, Font = new Font("Microsoft Sans Serif", 12) };
                Button cancel = new Button() { Text = "İptal", Dock = DockStyle.Bottom, Height = 30, DialogResult = DialogResult.Cancel, Font = new Font("Microsoft Sans Serif", 12) };

                passwordForm.Controls.Add(textBox);
                passwordForm.Controls.Add(confirmation);
                passwordForm.Controls.Add(cancel);
                passwordForm.Controls.Add(label);
                passwordForm.AcceptButton = confirmation;
                passwordForm.CancelButton = cancel;

                if (passwordForm.ShowDialog() == DialogResult.OK)
                {
                    return textBox.Text;
                }
                return string.Empty;
            }
        }

        private void chkUpdateUser_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUpdateUser.Checked)
            {
                chkNewUser.Checked = false;
            }
            UpdateUIState();
        }

        private void chkNewUser_CheckedChanged(object sender, EventArgs e)
        {
            if (chkNewUser.Checked)
            {
                chkUpdateUser.Checked = false;
                dataGridUsers.ClearSelection();
                selectedUserID = -1;
                clearFields();
            }
            UpdateUIState();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                clearFields();
                chkNewUser.Checked = false;
                chkUpdateUser.Checked = false;
                selectedUserID = -1;
                UpdateUIState();
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }
    }
}
