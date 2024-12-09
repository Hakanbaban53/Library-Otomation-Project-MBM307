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
    // Raporlar formu sınıfı / Reports form class
    public partial class ReportsForm : Form
    {
        // Yapıcı metod / Constructor method
        public ReportsForm()
        {
            InitializeComponent(); // Form bileşenlerini başlat / Initialize form components
                                   // Rapor türlerini combobox'a ekle / Add report types to the combobox
            cmbReportType.Items.Add("En Çok Ödünç Alınan Kitaplar");
            cmbReportType.Items.Add("Aktif Ödünçler");
            cmbReportType.Items.Add("Süresi Dolan Ödünç Kitaplar");
            cmbReportType.SelectedIndex = 0; // Varsayılan seçim / Default selection

            GenerateReport(); // Raporu oluştur / Generate report
            GetAuditLogs(); // Audit Loglarını Al / Get Audit Logs
        }

        private void GenerateReport()
        {
            string selectedReport = cmbReportType.SelectedItem.ToString(); // Seçilen rapor türünü al / Get selected report type
            DatabaseHelper db = new DatabaseHelper(); // Veritabanı yardımcı sınıfı / Database helper class
            DataTable reportData; // Rapor verileri için DataTable / DataTable for report data

            // Seçilen rapor türüne göre sorgu çalıştır / Execute query based on selected report type
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
                    // Geçersiz rapor seçimi durumunda hata mesajı göster / Show error message for invalid report selection
                    MessageBox.Show("Geçersiz Rapor seçimi.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Metodu sonlandır / End method
            }

            dataGridReports.DataSource = reportData; // Rapor verilerini datagrid'e ata / Assign report data to the datagrid
        }

        private void GetAuditLogs()
        {
            try
            {
                DatabaseHelper db = new DatabaseHelper(); // Veritabanı yardımcı sınıfı / Database helper class
                DataTable logs = db.ExecuteQuery("SELECT * FROM AuditLogs", null, false); // Tüm logları al / Get all logs
                dataGridLogs.DataSource = logs; // Logları datagrid'e ata / Assign logs to the datagrid
            }
            catch (Exception ex)
            {
                MessageBox.Show("Audit Loglar alınırken bir hata oluştu. Hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Logları yenile butonuna tıklama olayı / Refresh logs button click event
        private void btnRefreshLogs_Click(object sender, EventArgs e)
        {
            GetAuditLogs(); // Logları yenile / Refresh logs
        }

        // Rapor oluştur butonuna tıklama olayı / Generate report button click event
        private void btnGenerateReport_Click(object sender, EventArgs e)
        {
            GenerateReport(); // Raporu oluştur / Generate report
        }

        // Geri butonuna tıklama olayı / Back button click event
        private void btnBack_Click(object sender, EventArgs e)
        {
            FormHelper.NavigateBack(); // Geri git / Navigate back
        }
    }
}
