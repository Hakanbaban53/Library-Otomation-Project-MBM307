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
    public partial class LoanManagementForm : Form
    {
        // Alanlar / Fields
        private int LoanID = -1; // Seçilen ödünç ID'si / Selected loan ID
        private int FinePerDay; // Günlük ceza miktarı / Fine amount per day

        // Yapıcı / Constructor
        public LoanManagementForm()
        {
            InitializeComponent(); // Form bileşenlerini başlat / Initialize form components
            FinePerDay = GetFinePerDay(); // Günlük ceza ayarını al / Get fine per day setting
            refreshTables(); // İlk verileri yükle / Load initial data
        }

        // Veri tablolarını yenileme metodu / Method to refresh data tables
        private void refreshTables()
        {
            try
            {
                LoadUsers(); // Kullanıcıları veri ızgarasına yükle / Load users into data grid
                LoadBooks(); // Mevcut kitapları veri ızgarasına yükle / Load available books into data grid
                LoadLoans(); // Aktif ödünçleri veri ızgarasına yükle / Load active loans into data grid
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex.Message); // Herhangi bir istisna oluşursa hata mesajını göster / Show error message if any exception occurs
            }
        }

        // Günlük ceza ayarını alma metodu / Method to get fine per day setting
        private int GetFinePerDay()
        {
            return GetSettingValue("FinePerDay", "Hata: Gün Başına Ceza Anahtarı Bulunamadı"); // Error: Fine per day key not found
        }

        // Veritabanından belirli bir ayar değerini alma metodu / Method to get a specific setting value from the database
        private int GetSettingValue(string settingKey, string errorMessage)
        {
            try
            {
                DatabaseHelper db = new DatabaseHelper();
                DataTable result = db.ExecuteQuery($"SELECT SettingValue FROM Settings WHERE SettingKey = '{settingKey}'", null, false);
                return int.Parse(result.Rows[0]["SettingValue"].ToString());
            }
            catch (Exception ex)
            {
                throw new Exception(errorMessage, ex); // Özel mesaj ile istisna fırlat / Throw exception with custom message
            }
        }

        // Veri yükleme metodları / Data loading methods
        private void LoadUsers() => LoadData("SELECT MemberID, FirstName, LastName FROM Members", dataGridUsers, "Kullanıcılar Yüklenemedi"); // Users could not be loaded
        private void LoadBooks() => LoadData("SELECT * FROM ViewAvailableBooks", dataGridBooks, "Kitaplar Yüklenemedi"); // Books could not be loaded
        private void LoadLoans() => LoadData("SELECT * FROM ViewActiveLoans", dataGridLoans, "Ödünçler Yüklenemedi"); // Loans could not be loaded

        // DataGridView'e veri yüklemek için genel metot / General method to load data into DataGridView
        private void LoadData(string query, DataGridView dataGrid, string errorMessage)
        {
            try
            {
                DatabaseHelper db = new DatabaseHelper();
                DataTable data = db.ExecuteQuery(query, null, false);
                dataGrid.DataSource = data; // Izgara için veri kaynağını ayarla / Set data source for grid
            }
            catch (Exception ex)
            {
                throw new Exception($"Hata: {errorMessage}", ex); // Özel mesaj ile istisna fırlat / Throw exception with custom message
            }
        }

        // Uygunluk kontrolü metodları / Availability check methods
        private bool IsBookAvailable(int bookID) => CheckAvailability("IsBookAvailable", bookID, "Hata: Kitap Durumu Kontrol Edilemedi"); // Error: Book status could not be checked
        private bool CanBorrowMoreBooks(int memberID) => CheckAvailability("CanBorrowMoreBooks", memberID, "Hata: Ödünç Limiti Kontrol Edilemedi"); // Error: Borrow limit could not be checked

        // Saklı bir fonksiyon kullanarak uygunluğu kontrol etme metodu / Method to check availability using a stored function
        private bool CheckAvailability(string functionName, int id, string errorMessage)
        {
            try
            {
                DatabaseHelper db = new DatabaseHelper();
                SqlParameter[] parameters = { new SqlParameter($"@{functionName}ID", id) };
                DataTable result = db.ExecuteQuery($"SELECT dbo.{functionName}(@{functionName}ID) AS Available", parameters, false);
                return Convert.ToBoolean(result.Rows[0]["Available"]); // Uygunluk durumunu döndür / Return availability status
            }
            catch (Exception ex)
            {
                throw new Exception(errorMessage, ex); // Özel mesaj ile istisna fırlat / Throw exception with custom message
            }
        }

        // Bir ödünçün geçerli olup olmadığını doğrulama metodu / Method to validate if a loan is valid
        private bool IsLoanValid(int loanID)
        {
            foreach (DataGridViewRow row in dataGridLoans.Rows)
            {
                if ((int)row.Cells["LoanID"].Value == loanID)
                {
                    return true; // Ödünç geçerli / Loan is valid
                }
            }
            return false; // Ödünç geçerli değil / Loan is not valid
        }

        // Hata mesajlarını gösterme metodu / Method to show error messages
        private void ShowErrorMessage(string message)
        {
            MessageBox.Show($"Bir hata oluştu: {message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); // An error occurred: 
        }

        // Olay işleyicileri / Event handlers
        private void btnBack_Click(object sender, EventArgs e) => FormHelper.NavigateBack(); // Geri git / Go back

        private void dataGridUsers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtMemberID.Text = dataGridUsers.Rows[e.RowIndex].Cells["MemberID"].Value.ToString(); // Seçilen üye ID'sini ayarla / Set selected member ID
            }
        }

        private void dataGridBooks_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtBookID.Text = dataGridBooks.Rows[e.RowIndex].Cells["BookID"].Value.ToString(); // Seçilen kitap ID'sini ayarla / Set selected book ID
            }
        }

        private void btnRefreshTab1_Click(object sender, EventArgs e) => refreshTables(); // Veri tablolarını yenile / Refresh data tables

        private void btnIssueLoan_Click(object sender, EventArgs e)
        {
            try
            {
                int bookID = int.Parse(txtBookID.Text); // Kitap ID'sini al / Get book ID
                int memberID = int.Parse(txtMemberID.Text); // Üye ID'sini al / Get member ID
                DateTime loanDate = dtpLoanDate.Value; // Ödünç tarihini al / Get loan date
                DateTime dueDate = dtpDueDate.Value; // İade tarihini al / Get due date

                // Kitabın uygun olup olmadığını kontrol et / Check if the book is available
                if (!IsBookAvailable(bookID))
                {
                    throw new Exception("Bu kitap şu anda ödünç alınamaz. Lütfen başka bir kitap seçin."); // This book cannot be borrowed right now. Please select another book.
                }

                // Üyenin daha fazla kitap ödünç alıp alamayacağını kontrol et / Check if the member can borrow more books
                if (!CanBorrowMoreBooks(memberID))
                {
                    throw new Exception("Bu üye zaten maksimum ödünç kitap sayısına ulaştı."); // This member has already reached the maximum number of borrowed books.
                }

                // Kitap ödünç verme prosedürünü çalıştır / Execute the loan book procedure
                DatabaseHelper db = new DatabaseHelper();
                SqlParameter[] parameters = {
                                new SqlParameter("@BookID", bookID),
                                new SqlParameter("@MemberID", memberID),
                                new SqlParameter("@LoanDate", loanDate),
                                new SqlParameter("@DueDate", dueDate)
                            };

                db.ExecuteNonQuery("LoanBook", parameters); // Non-query çalıştır / Execute non-query
                MessageBox.Show("Kitap başarılı bir şekilde ödünç verildi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information); // The book has been successfully loaned!
                txtBookID.Clear(); // Kitap ID'si girişini temizle / Clear book ID input
                txtMemberID.Clear(); // Üye ID'si girişini temizle / Clear member ID input
                refreshTables(); // Veri tablolarını yenile / Refresh data tables
            }
            catch (FormatException)
            {
                MessageBox.Show("Lütfen geçerli bir Book ID ve Member ID giriniz.", "Format Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error); // Please enter a valid Book ID and Member ID.
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex.Message); // Hata mesajını göster / Show error message
            }
        }

        private void dataGridLoans_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                LoanID = (int)dataGridLoans.Rows[e.RowIndex].Cells["LoanID"].Value; // Seçilen ödünç ID'sini ayarla / Set selected loan ID
                txtLoanID.Text = LoanID.ToString(); // Ödünç ID'sini göster / Show loan ID
            }
            else
            {
                LoanID = -1; // Ödünç ID'sini sıfırla / Reset loan ID
                txtLoanID.Clear(); // Ödünç ID'si girişini temizle / Clear loan ID input
            }
        }

        private void btnReturnBook_Click(object sender, EventArgs e)
        {
            try
            {
                // Seçilen ödünçü doğrula / Validate selected loan
                if (LoanID == -1 || !IsLoanValid(LoanID))
                {
                    throw new Exception("Lütfen geçerli bir ödünç alan seçin."); // Please select a valid loan.
                }

                DateTime returnDate = dtpReturnDate.Value; // İade tarihini al / Get return date

                // Kitap iade prosedürünü çalıştır / Execute the return book procedure
                DatabaseHelper db = new DatabaseHelper();
                SqlParameter[] parameters = {
                                new SqlParameter("@LoanID", LoanID),
                                new SqlParameter("@ReturnDate", returnDate),
                                new SqlParameter("@FinePerDay", FinePerDay)
                            };

                db.ExecuteNonQuery("ReturnBook", parameters); // Non-query çalıştır / Execute non-query
                MessageBox.Show("Kitap başarılı bir şekilde iade edildi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information); // The book has been successfully returned!
                refreshTables(); // Veri tablolarını yenile / Refresh data tables
            }
            catch (FormatException)
            {
                MessageBox.Show("Geçersiz veri formatı. Lütfen ödünç alanı kontrol edin.", "Format Hatası"); // Invalid data format. Please check the loan field.
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex.Message); // Hata mesajını göster / Show error message
            }
        }
    }
}

