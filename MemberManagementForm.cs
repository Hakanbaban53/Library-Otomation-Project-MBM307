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
    public partial class MemberManagementForm : Form
    {
        // Seçilen üye ID'si / Selected member ID
        int selectedMemberID = -1;

        // Formun yapıcı metodu / Constructor of the form
        public MemberManagementForm()
        {
            InitializeComponent(); // Bileşenleri başlat / Initialize components
            LoadMembers(); // Üyeleri yükle / Load members
            UpdateUIState(); // UI durumunu güncelle / Update UI state
        }

        // Alanları temizleme metodu / Method to clear fields
        private void clearFields()
        {
            txtFirstName.Text = ""; // Ad alanını temizle / Clear first name field
            txtLastName.Text = ""; // Soyad alanını temizle / Clear last name field
            dateBirthTime.Value = DateTime.Now; // Doğum tarihini güncel tarihe ayarla / Set birth date to current date
            txtAddress.Text = ""; // Adres alanını temizle / Clear address field
            txtPhone.Text = ""; // Telefon alanını temizle / Clear phone field
            txtEmail.Text = ""; // E-posta alanını temizle / Clear email field
        }

        // Üyeleri veritabanından yükleme metodu / Method to load members from the database
        private void LoadMembers()
        {
            DatabaseHelper db = new DatabaseHelper(); // Veritabanı yardımcı nesnesi oluştur / Create database helper instance
            DataTable dt = db.ExecuteQuery("SELECT * FROM Members", null, false); // Üyeleri sorgula / Query members
            dataGridMembers.DataSource = dt; // Üyeleri veri ızgarasına ata / Assign members to data grid
            selectedMemberID = -1; // Seçilen üye ID'sini sıfırla / Reset selected member ID
        }

        // Geri butonuna tıklama olayı / Back button click event
        private void btnBack_Click(object sender, EventArgs e)
        {
            FormHelper.NavigateBack(); // Geri git / Navigate back
        }

        // Alanların kontrolü için metot / Method to check fields
        private void checkFields()
        {
            // Alanların boş olup olmadığını kontrol et / Check if fields are empty
            if (string.IsNullOrWhiteSpace(txtFirstName.Text) || string.IsNullOrWhiteSpace(txtLastName.Text) ||
                string.IsNullOrWhiteSpace(txtAddress.Text) || string.IsNullOrWhiteSpace(txtPhone.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                throw new Exception("Tüm alanlar doldurulmalıdır."); // Hata fırlat / Throw error
            }

            // Telefon numarasının geçerliliğini kontrol et / Check validity of phone number
            if (!System.Text.RegularExpressions.Regex.IsMatch(txtPhone.Text, @"^\d{10}$"))
            {
                throw new Exception("Geçerli bir telefon numararı giriniz (10 Haneli telefon numaranızı başında '0' olmadan girin.)."); // Hata fırlat / Throw error
            }

            // E-posta adresinin geçerliliğini kontrol et / Check validity of email address
            if (!System.Text.RegularExpressions.Regex.IsMatch(txtEmail.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                throw new Exception("Geçerli bir e-posta adresi giriniz."); // Hata fırlat / Throw error
            }
        }

        // Veri ızgarasında hücreye tıklama olayı / Data grid cell click event
        private void dataGridMembers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Geçerli bir satır seçildiyse / If a valid row is selected
            {
                DataGridViewRow row = dataGridMembers.Rows[e.RowIndex]; // Seçilen satırı al / Get selected row
                selectedMemberID = (int)row.Cells["MemberID"].Value; // Üye ID'sini al / Get member ID
                txtFirstName.Text = row.Cells["FirstName"].Value.ToString(); // Adı al / Get first name
                txtLastName.Text = row.Cells["LastName"].Value.ToString(); // Soyadı al / Get last name
                dateBirthTime.Value = (DateTime)row.Cells["DateOfBirth"].Value; // Doğum tarihini al / Get date of birth
                txtAddress.Text = row.Cells["Address"].Value.ToString(); // Adresi al / Get address
                txtPhone.Text = row.Cells["Phone"].Value.ToString(); // Telefonu al / Get phone
                txtEmail.Text = row.Cells["Email"].Value.ToString(); // E-posta adresini al / Get email
            }
            UpdateUIState(); // UI durumunu güncelle / Update UI state
        }

        // Kaydet butonuna tıklama olayı / Save button click event
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                checkFields(); // Alanları kontrol et / Check fields
                String firstName = txtFirstName.Text; // Adı al / Get first name
                String lastName = txtLastName.Text; // Soyadı al / Get last name
                DateTime dateOfBirth = dateBirthTime.Value; // Doğum tarihini al / Get date of birth
                String address = txtAddress.Text; // Adresi al / Get address
                String phone = txtPhone.Text; // Telefonu al / Get phone
                String email = txtEmail.Text; // E-posta adresini al / Get email

                DatabaseHelper db = new DatabaseHelper(); // Veritabanı yardımcı nesnesi oluştur / Create database helper instance
                SqlParameter[] parameters; // SQL parametreleri / SQL parameters

                // Yeni üye kaydı / New member registration
                if (chkNewMember.Checked)
                {
                    parameters = new SqlParameter[]
                    {
                                new SqlParameter("@FirstName", firstName),
                                new SqlParameter("@LastName", lastName),
                                new SqlParameter("@DateOfBirth", dateOfBirth),
                                new SqlParameter("@Address", address),
                                new SqlParameter("@Phone", phone),
                                new SqlParameter("@Email", email)
                    };
                    db.ExecuteNonQuery("RegisterNewMember", parameters); // Yeni üye kaydet / Save new member
                }
                // Üye güncelleme / Update member
                else if (chkUpdateMember.Checked)
                {
                    int memberId = selectedMemberID; // Seçilen üye ID'sini al / Get selected member ID
                    parameters = new SqlParameter[]
                    {
                                new SqlParameter("@MemberID", memberId),
                                new SqlParameter("@FirstName", firstName),
                                new SqlParameter("@LastName", lastName),
                                new SqlParameter("@DateOfBirth", dateOfBirth),
                                new SqlParameter("@Address", address),
                                new SqlParameter("@Phone", phone),
                                new SqlParameter("@Email", email)
                    };
                    db.ExecuteNonQuery("UpdateMember", parameters); // Üyeyi güncelle / Update member
                }

                LoadMembers(); // Üyeleri yeniden yükle / Reload members
                clearFields(); // Alanları temizle / Clear fields
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); // Hata mesajı göster / Show error message
            }
        }

        // Yeni üye onay kutusunun durumunu değiştirme olayı / New member checkbox checked change event
        private void chkNewMember_CheckedChanged(object sender, EventArgs e)
        {
            if (chkNewMember.Checked) // Yeni üye onay kutusu işaretlendiyse / If new member checkbox is checked
            {
                chkUpdateMember.Checked = false; // Güncelleme onay kutusunu temizle / Uncheck update checkbox
                dataGridMembers.ClearSelection(); // Veri ızgarasındaki seçimi temizle / Clear selection in data grid
                selectedMemberID = -1; // Seçilen üye ID'sini sıfırla / Reset selected member ID
                clearFields(); // Alanları temizle / Clear fields
            }
            UpdateUIState(); // UI durumunu güncelle / Update UI state
        }

        // Güncelleme onay kutusunun durumunu değiştirme olayı / Update checkbox checked change event
        private void chkUpdateMember_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUpdateMember.Checked) // Güncelleme onay kutusu işaretlendiyse / If update checkbox is checked
            {
                chkNewMember.Checked = false; // Yeni üye onay kutusunu temizle / Uncheck new member checkbox
            }
            UpdateUIState(); // UI durumunu güncelle / Update UI state
        }

        // UI durumunu güncelleme metodu / Method to update UI state
        private void UpdateUIState()
        {
            // Alanları etkinleştir veya devre dışı bırak / Enable or disable fields
            bool enableFields = chkNewMember.Checked || (chkUpdateMember.Checked && selectedMemberID != -1);
            txtFirstName.Enabled = enableFields; // Ad alanını etkinleştir / Enable first name field
            txtLastName.Enabled = enableFields; // Soyad alanını etkinleştir / Enable last name field
            dateBirthTime.Enabled = enableFields; // Doğum tarihini etkinleştir / Enable birth date
            txtAddress.Enabled = enableFields; // Adres alanını etkinleştir / Enable address field
            txtPhone.Enabled = enableFields; // Telefon alanını etkinleştir / Enable phone field
            txtEmail.Enabled = enableFields; // E-posta alanını etkinleştir / Enable email field
            btnSave.Enabled = enableFields; // Kaydet butonunu etkinleştir / Enable save button
        }

        // Temizle butonuna tıklama olayı / Clear button click event
        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                clearFields(); // Alanları temizle / Clear fields
                chkNewMember.Checked = false; // Yeni üye onay kutusunu temizle / Uncheck new member checkbox
                chkUpdateMember.Checked = false; // Güncelleme onay kutusunu temizle / Uncheck update checkbox
                selectedMemberID = -1; // Seçilen üye ID'sini sıfırla / Reset selected member ID
                UpdateUIState(); // UI durumunu güncelle / Update UI state
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); // Hata mesajı göster / Show error message
            }
        }

        // Üye silme butonuna tıklama olayı / Delete member button click event
        private void btnDeleteMember_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedMemberID == -1) // Eğer üye seçilmemişse / If no member is selected
                {
                    throw new Exception("Lütfen silmek istediğiniz üyeyi seçin."); // Hata fırlat / Throw error
                }

                int memberId = selectedMemberID; // Seçilen üye ID'sini al / Get selected member ID
                string memberName = txtFirstName.Text + " " + txtLastName.Text; // Üye adını al / Get member name

                // Silme onayı için diyalog kutusu göster / Show dialog for delete confirmation
                DialogResult dialogResult = MessageBox.Show($"{memberName} adlı üyeyi gerçekten silmek istiyor musunuz?", "Üye Silme", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes) // Eğer kullanıcı evet derse / If user confirms
                {
                    DatabaseHelper db = new DatabaseHelper(); // Veritabanı yardımcı nesnesi oluştur / Create database helper instance
                    SqlParameter[] parameters = new SqlParameter[] // SQL parametreleri / SQL parameters
                    {
                                new SqlParameter("@MemberID", memberId) // Üye ID'sini parametre olarak ekle / Add member ID as parameter
                    };

                    db.ExecuteNonQuery("CheckMemberBeforeDelete", parameters); // Üyeyi sil / Delete member
                    LoadMembers(); // Üyeleri yeniden yükle / Reload members
                    clearFields(); // Alanları temizle / Clear fields
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); // Hata mesajı göster / Show error message
            }
        }
    }
}
