using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Library_Otomation
{
    // Raporlar formu sınıfı
    public partial class ReportsForm : Form
    {
        // Yapıcı metod
        public ReportsForm()
        {
            InitializeComponent(); // Form bileşenlerini başlat
                                   // Rapor türlerini combobox'a ekle
            cmbReportType.Items.Add("En Çok Ödünç Alınan Kitaplar");
            cmbReportType.Items.Add("Aktif Ödünçler");
            cmbReportType.Items.Add("Süresi Dolan Ödünç Kitaplar");
            cmbReportType.SelectedIndex = 0; // Varsayılan seçim

            GenerateReport(); // Raporu oluştur
            GetAuditLogs(); // Audit Loglarını Al

        }

        private void GenerateReport()
        {
            string selectedReport = cmbReportType.SelectedItem.ToString(); // Seçilen rapor türünü al
            DatabaseHelper db = new DatabaseHelper(); // Veritabanı yardımcı sınıfı
            DataTable reportData; // Rapor verileri için DataTable

            // Seçilen rapor türüne göre sorgu çalıştır
            switch (selectedReport)
            {
                case "En Çok Ödünç Alınan Kitaplar":
                    reportData = db.ExecuteQuery("SELECT * FROM MostBorrowedBooksView ORDER BY BorrowCount DESC", null, false);
                    break;
                case "Aktif Ödünçler":
                    reportData = db.ExecuteQuery("SELECT * FROM ActiveMembersView ORDER BY ActiveLoans DESC", null, false);
                    break;
                case "Süresi Dolan Ödünç Kitaplar":
                    reportData = db.ExecuteQuery("SELECT * FROM ViewOverdueLoans", null, false);
                    break;
                default:
                    // Geçersiz rapor seçimi durumunda hata mesajı göster
                    MessageBox.Show("Geçersiz Rapor seçimi.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Metodu sonlandır
            }

            dataGridReports.DataSource = reportData; // Rapor verilerini datagrid'e ata
        }

        private void GetAuditLogs()
        {
            DatabaseHelper db = new DatabaseHelper(); // Veritabanı yardımcı sınıfı
            DataTable logs = db.ExecuteQuery("SELECT * FROM AuditLog", null, false); // Tüm logları al
            dataGridLogs.DataSource = logs; // Logları datagrid'e ata
        }

        // Logları yenile butonuna tıklama olayı
        private void btnRefreshLogs_Click(object sender, EventArgs e)
        {
            GetAuditLogs(); // Logları yenile
        }

        // Rapor oluştur butonuna tıklama olayı
        private void btnGenerateReport_Click(object sender, EventArgs e)
        {
            GenerateReport();
        }

        // Geri butonuna tıklama olayı
        private void btnBack_Click(object sender, EventArgs e)
        {
            FormHelper.NavigateBack(); // Geri git
        }
    }
}
