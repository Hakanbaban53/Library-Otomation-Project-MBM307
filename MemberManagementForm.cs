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
        // Seçilen üye ID'si
        int selectedMemberID = -1;

        // Formun yapıcı metodu
        public MemberManagementForm()
        {
            InitializeComponent(); // Bileşenleri başlat
            LoadMembers(); // Üyeleri yükle
            UpdateUIState(); // UI durumunu güncelle
        }

        // Alanları temizleme metodu
        private void clearFields()
        {
            txtFirstName.Text = ""; // Ad alanını temizle
            txtLastName.Text = ""; // Soyad alanını temizle
            dateBirthTime.Value = DateTime.Now; // Doğum tarihini güncel tarihe ayarla
            txtAddress.Text = ""; // Adres alanını temizle
            txtPhone.Text = ""; // Telefon alanını temizle
            txtEmail.Text = ""; // E-posta alanını temizle
        }

        // Üyeleri veritabanından yükleme metodu
        private void LoadMembers()
        {
            DatabaseHelper db = new DatabaseHelper(); // Veritabanı yardımcı nesnesi oluştur
            DataTable dt = db.ExecuteQuery("SELECT * FROM Members", null, false); // Üyeleri sorgula
            dataGridMembers.DataSource = dt; // Üyeleri veri ızgarasına ata
            selectedMemberID = -1; // Seçilen üye ID'sini sıfırla
        }

        // Geri butonuna tıklama olayı
        private void btnBack_Click(object sender, EventArgs e)
        {
            FormHelper.NavigateBack(); // Geri git
        }

        // Alanların kontrolü için metot
        private void checkFields()
        {
            // Alanların boş olup olmadığını kontrol et
            if (string.IsNullOrWhiteSpace(txtFirstName.Text) || string.IsNullOrWhiteSpace(txtLastName.Text) ||
                string.IsNullOrWhiteSpace(txtAddress.Text) || string.IsNullOrWhiteSpace(txtPhone.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                throw new Exception("Tüm alanlar doldurulmalıdır."); // Hata fırlat
            }

            // Telefon numarasının geçerliliğini kontrol et
            if (!System.Text.RegularExpressions.Regex.IsMatch(txtPhone.Text, @"^\d{10}$"))
            {
                throw new Exception("Geçerli bir telefon numararı giriniz (10 Haneli telefon numaranızı başında '0' olmadan girin.)."); // Hata fırlat
            }

            // E-posta adresinin geçerliliğini kontrol et
            if (!System.Text.RegularExpressions.Regex.IsMatch(txtEmail.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                throw new Exception("Geçerli bir e-posta adresi giriniz."); // Hata fırlat
            }
        }

        // Veri ızgarasında hücreye tıklama olayı
        private void dataGridMembers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Geçerli bir satır seçildiyse
            {
                DataGridViewRow row = dataGridMembers.Rows[e.RowIndex]; // Seçilen satırı al
                selectedMemberID = (int)row.Cells["MemberID"].Value; // Üye ID'sini al
                txtFirstName.Text = row.Cells["FirstName"].Value.ToString(); // Adı al
                txtLastName.Text = row.Cells["LastName"].Value.ToString(); // Soyadı al
                dateBirthTime.Value = (DateTime)row.Cells["DateOfBirth"].Value; // Doğum tarihini al
                txtAddress.Text = row.Cells["Address"].Value.ToString(); // Adresi al
                txtPhone.Text = row.Cells["Phone"].Value.ToString(); // Telefonu al
                txtEmail.Text = row.Cells["Email"].Value.ToString(); // E-posta adresini al
            }
            UpdateUIState(); // UI durumunu güncelle
        }

        // Kaydet butonuna tıklama olayı
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                checkFields(); // Alanları kontrol et
                String firstName = txtFirstName.Text; // Adı al
                String lastName = txtLastName.Text; // Soyadı al
                DateTime dateOfBirth = dateBirthTime.Value; // Doğum tarihini al
                String address = txtAddress.Text; // Adresi al
                String phone = txtPhone.Text; // Telefonu al
                String email = txtEmail.Text; // E-posta adresini al

                DatabaseHelper db = new DatabaseHelper(); // Veritabanı yardımcı nesnesi oluştur
                SqlParameter[] parameters; // SQL parametreleri

                // Yeni üye kaydı
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
                    db.ExecuteNonQuery("RegisterNewMember", parameters); // Yeni üye kaydet
                }
                // Üye güncelleme
                else if (chkUpdateMember.Checked)
                {
                    int memberId = selectedMemberID; // Seçilen üye ID'sini al
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
                    db.ExecuteNonQuery("UpdateMember", parameters); // Üyeyi güncelle
                }

                LoadMembers(); // Üyeleri yeniden yükle
                clearFields(); // Alanları temizle
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); // Hata mesajı göster
            }
        }

        // Yeni üye onay kutusunun durumunu değiştirme olayı
        private void chkNewMember_CheckedChanged(object sender, EventArgs e)
        {
            if (chkNewMember.Checked) // Yeni üye onay kutusu işaretlendiyse
            {
                chkUpdateMember.Checked = false; // Güncelleme onay kutusunu temizle
                dataGridMembers.ClearSelection(); // Veri ızgarasındaki seçimi temizle
                selectedMemberID = -1; // Seçilen üye ID'sini sıfırla
                clearFields(); // Alanları temizle
            }
            UpdateUIState(); // UI durumunu güncelle
        }

        // Güncelleme onay kutusunun durumunu değiştirme olayı
        private void chkUpdateMember_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUpdateMember.Checked) // Güncelleme onay kutusu işaretlendiyse
            {
                chkNewMember.Checked = false; // Yeni üye onay kutusunu temizle
            }
            UpdateUIState(); // UI durumunu güncelle
        }

        // UI durumunu güncelleme metodu
        private void UpdateUIState()
        {
            // Alanları etkinleştir veya devre dışı bırak
            bool enableFields = chkNewMember.Checked || (chkUpdateMember.Checked && selectedMemberID != -1);
            txtFirstName.Enabled = enableFields; // Ad alanını etkinleştir
            txtLastName.Enabled = enableFields; // Soyad alanını etkinleştir
            dateBirthTime.Enabled = enableFields; // Doğum tarihini etkinleştir
            txtAddress.Enabled = enableFields; // Adres alanını etkinleştir
            txtPhone.Enabled = enableFields; // Telefon alanını etkinleştir
            txtEmail.Enabled = enableFields; // E-posta alanını etkinleştir
            btnSave.Enabled = enableFields; // Kaydet butonunu etkinleştir
        }

        // Temizle butonuna tıklama olayı
        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                clearFields(); // Alanları temizle
                chkNewMember.Checked = false; // Yeni üye onay kutusunu temizle
                chkUpdateMember.Checked = false; // Güncelleme onay kutusunu temizle
                selectedMemberID = -1; // Seçilen üye ID'sini sıfırla
                UpdateUIState(); // UI durumunu güncelle
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); // Hata mesajı göster
            }
        }

        // Üye silme butonuna tıklama olayı
        private void btnDeleteMember_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedMemberID == -1) // Eğer üye seçilmemişse
                {
                    throw new Exception("Lütfen silmek istediğiniz üyeyi seçin."); // Hata fırlat
                }

                int memberId = selectedMemberID; // Seçilen üye ID'sini al
                string memberName = txtFirstName.Text + " " + txtLastName.Text; // Üye adını al

                // Silme onayı için diyalog kutusu göster
                DialogResult dialogResult = MessageBox.Show($"{memberName} adlı üyeyi gerçekten silmek istiyor musunuz?", "Üye Silme", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes) // Eğer kullanıcı evet derse
                {
                    DatabaseHelper db = new DatabaseHelper(); // Veritabanı yardımcı nesnesi oluştur
                    SqlParameter[] parameters = new SqlParameter[] // SQL parametreleri
                    {
                            new SqlParameter("@MemberID", memberId) // Üye ID'sini parametre olarak ekle
                    };

                    db.ExecuteNonQuery("CheckMemberBeforeDelete", parameters); // Üyeyi sil
                    LoadMembers(); // Üyeleri yeniden yükle
                    clearFields(); // Alanları temizle
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); // Hata mesajı göster
            }
        }
    }
}
