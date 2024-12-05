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
        // Üye ID'sini tutan değişken
        int MemberID = -1;

        // Formun yapıcı metodu
        public ManageFinesForm()
        {
            InitializeComponent(); // Bileşenleri başlat
            LoadMembersWithLoansAndFines(); // Üyeleri ve cezaları yükle
            btnPayFine.Enabled = false; // Cezayı ödeme butonunu devre dışı bırak
        }

        // Üyeleri, aktif ödünçler ve cezalar ile yükleyen metod
        private void LoadMembersWithLoansAndFines()
        {
            try
            {
                DatabaseHelper db = new DatabaseHelper();
                DataTable memberLoans = db.ExecuteQuery("GetMembersWithLoansAndFines", null, true); // Üyeleri ve cezaları getir
                dataGridFines.DataSource = memberLoans;
                dataGridFines.Columns["TotalFines"].DefaultCellStyle.Format = "C2"; // Toplam cezaları para birimi olarak göster

                // Sütun başlıklarını güncelle
                if (dataGridFines.Columns.Contains("FineIDs"))
                {
                    dataGridFines.Columns["FineIDs"].HeaderText = "Fine IDs";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        // Grid'deki bir hücreye tıklandığında çalışan metod
        private void dataGridFines_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Geçerli bir satır seçildiyse
            {
                int memberID = Convert.ToInt32(dataGridFines.Rows[e.RowIndex].Cells["MemberID"].Value); // Üye ID'sini al
                MemberID = memberID; // Üye ID'sini güncelle
                lblTotalFines.Text = $"Total Fines: {dataGridFines.Rows[e.RowIndex].Cells["TotalFines"].FormattedValue}"; // Toplam cezayı göster
                btnPayFine.Enabled = memberID > 0; // Butonu etkinleştir
            }
        }

        // Cezayı ödeme butonuna tıklandığında çalışan metod
        private void btnPayFine_Click(object sender, EventArgs e)
        {
            try
            {
                if (MemberID == -1) // Eğer üye seçilmemişse
                {
                    MessageBox.Show("Please select a user to pay the fine.", "Error"); // Hata mesajı göster
                    return; // Metodu sonlandır
                }

                // Kullanıcının aktif ödünçleri olup olmadığını kontrol et
                if (HasActiveLoans(MemberID))
                {
                    MessageBox.Show("This user has active loans. Fines can only be paid once all loans are returned.", "Error"); // Hata mesajı göster
                    return; // Metodu sonlandır
                }

                DatabaseHelper db = new DatabaseHelper(); // Veritabanı yardımcı sınıfı
                SqlParameter[] parameters = { new SqlParameter("@MemberID", MemberID) }; // Parametreleri ayarla

                // Üye için tüm cezaları ödeyen prosedürü çalıştır
                db.ExecuteNonQuery("EXEC PayAllFinesForMember @MemberID", parameters, false);

                MessageBox.Show("All fines for the selected user have been paid!"); // Başarı mesajı göster
                LoadMembersWithLoansAndFines(); // Üyeleri ve cezaları yeniden yükle
                btnPayFine.Enabled = false; // Butonu devre dışı bırak
                MemberID = -1; // Üye ID'sini sıfırla
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir Hata Oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); // Hata mesajı göster
            }
        }

        // Aktif ödünçlerin olup olmadığını kontrol eden metod
        private bool HasActiveLoans(int memberID)
        {
            try
            {
                DatabaseHelper db = new DatabaseHelper(); // Veritabanı yardımcı sınıfı
                SqlParameter[] parameters = { new SqlParameter("@MemberID", memberID) }; // Parametreleri ayarla

                // Aktif ödünçlerin sayısını kontrol et
                DataTable result = db.ExecuteQuery("SELECT COUNT(*) FROM Loans WHERE MemberID = @MemberID AND ReturnDate IS NULL", parameters, false);

                // Aktif ödünç varsa true, yoksa false döndür
                return Convert.ToInt32(result.Rows[0][0]) > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir Hata Oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); // Hata mesajı göster
                return false;
            }
        }

        // Geri butonuna tıklandığında çalışan metod
        private void btnBack_Click(object sender, EventArgs e)
        {
            FormHelper.NavigateBack(); // Geri git
        }
    }
}
