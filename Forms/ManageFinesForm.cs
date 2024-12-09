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
    public partial class ManageFinesForm : Form
    {
        // Üye ID'sini tutan değişken / Variable to hold the Member ID
        int MemberID = -1;

        // Formun yapıcı metodu / Constructor method of the form
        public ManageFinesForm()
        {
            InitializeComponent(); // Bileşenleri başlat / Initialize components
            LoadMembersWithLoansAndFines(); // Üyeleri ve cezaları yükle / Load members with loans and fines
            btnPayFine.Enabled = false; // Cezayı ödeme butonunu devre dışı bırak / Disable the pay fine button
        }

        // Üyeleri, aktif ödünçler ve cezalar ile yükleyen metod / Method to load members with active loans and fines
        private void LoadMembersWithLoansAndFines()
        {
            try
            {
                DatabaseHelper db = new DatabaseHelper();
                DataTable memberLoans = db.ExecuteQuery("GetMembersWithLoansAndFines", null, true); // Üyeleri ve cezaları getir / Get members and fines
                dataGridFines.DataSource = memberLoans;
                dataGridFines.Columns["TotalFines"].DefaultCellStyle.Format = "C2"; // Toplam cezaları para birimi olarak göster / Show total fines as currency

                // Sütun başlıklarını güncelle / Update column headers
                if (dataGridFines.Columns.Contains("FineIDs"))
                {
                    dataGridFines.Columns["FineIDs"].HeaderText = "Fine IDs"; // Cezaların ID'leri / Fine IDs
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); // Hata mesajı göster / Show error message
            }
        }

        // Grid'deki bir hücreye tıklandığında çalışan metod / Method that runs when a cell in the grid is clicked
        private void dataGridFines_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Geçerli bir satır seçildiyse / If a valid row is selected
            {
                int memberID = Convert.ToInt32(dataGridFines.Rows[e.RowIndex].Cells["MemberID"].Value); // Üye ID'sini al / Get the Member ID
                MemberID = memberID; // Üye ID'sini güncelle / Update the Member ID
                txtMemberID.Text = memberID.ToString(); // Üye ID'sini göster / Show Member ID
                txtMemberName.Text = dataGridFines.Rows[e.RowIndex].Cells["MemberName"].FormattedValue.ToString(); // Üye adını göster / Show Member Name
                lblTotalFines.Text = $"Toplam Borç: {dataGridFines.Rows[e.RowIndex].Cells["TotalFines"].FormattedValue}"; // Toplam cezayı göster / Show total fines
                btnPayFine.Enabled = memberID > 0; // Butonu etkinleştir / Enable the button
            }
        }

        // Cezayı ödeme butonuna tıklandığında çalışan metod / Method that runs when the pay fine button is clicked
        private void btnPayFine_Click(object sender, EventArgs e)
        {
            try
            {
                if (MemberID == -1) // Eğer üye seçilmemişse / If no member is selected
                {
                    throw new Exception("Lütfen bir üye seçin!"); // Hata mesajı göster / Show error message
                }

                // Kullanıcının aktif ödünçleri olup olmadığını kontrol et / Check if the user has active loans
                if (HasActiveLoans(MemberID))
                {
                    throw new Exception("Seçili üyenin aktif ödünçleri var. Lütfen önce ödünçleri iade edin!"); // Hata mesajı göster / Show error message
                }

                DatabaseHelper db = new DatabaseHelper(); // Veritabanı yardımcı sınıfı / Database helper class
                SqlParameter[] parameters = { new SqlParameter("@MemberID", MemberID) }; // Parametreleri ayarla / Set parameters

                // Üye için tüm cezaları ödeyen prosedürü çalıştır / Execute the procedure to pay all fines for the member
                db.ExecuteNonQuery("EXEC PayAllFinesForMember @MemberID", parameters, false);

                MessageBox.Show("Üyenin tüm cezaları ödendi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information); // Başarılı mesajı göster / Show success message
                LoadMembersWithLoansAndFines(); // Üyeleri ve cezaları yeniden yükle / Reload members and fines
                btnPayFine.Enabled = false; // Butonu devre dışı bırak / Disable the button
                MemberID = -1; // Üye ID'sini sıfırla / Reset the Member ID
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir Hata Oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); // Hata mesajı göster / Show error message
            }
        }

        // Aktif ödünçlerin olup olmadığını kontrol eden metod / Method to check if there are active loans
        private bool HasActiveLoans(int memberID)
        {
            try
            {
                DatabaseHelper db = new DatabaseHelper(); // Veritabanı yardımcı sınıfı / Database helper class
                SqlParameter[] parameters = { new SqlParameter("@MemberID", memberID) }; // Parametreleri ayarla / Set parameters

                // Aktif ödünçlerin sayısını kontrol et / Check the count of active loans
                DataTable result = db.ExecuteQuery("SELECT COUNT(*) FROM Loans WHERE MemberID = @MemberID AND ReturnDate IS NULL", parameters, false);

                // Aktif ödünç varsa true, yoksa false döndür / Return true if there are active loans, otherwise false
                return Convert.ToInt32(result.Rows[0][0]) > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir Hata Oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); // Hata mesajı göster / Show error message
                return false;
            }
        }

        // Geri butonuna tıklandığında çalışan metod / Method that runs when the back button is clicked
        private void btnBack_Click(object sender, EventArgs e)
        {
            FormHelper.NavigateBack(); // Geri git / Navigate back
        }
    }
}
