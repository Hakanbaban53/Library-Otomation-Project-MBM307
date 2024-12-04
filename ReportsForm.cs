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
    public partial class ReportsForm : Form
    {
        public ReportsForm()
        {
            InitializeComponent();
            cmbReportType.Items.Add("En Çok Ödünç Alınan Kitaplar");
            cmbReportType.Items.Add("Aktif Ödünçler");
            cmbReportType.Items.Add("Süresi Dolan Ödünç Kitaplar");
            cmbReportType.SelectedIndex = 0; // Default selection

        }

        private void btnRefreshLogs_Click(object sender, EventArgs e)
        {
            DatabaseHelper db = new DatabaseHelper();
            DataTable logs = db.ExecuteQuery("SELECT * FROM AuditLog ORDER BY ActionDate DESC", null, false);
            dataGridLogs.DataSource = logs;
        }


        private void btnGenerateReport_Click(object sender, EventArgs e)
        {
            string selectedReport = cmbReportType.SelectedItem.ToString();
            DatabaseHelper db = new DatabaseHelper();
            DataTable reportData;

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
                    MessageBox.Show("Geçersiz Rapor seçimi.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
            }

            dataGridReports.DataSource = reportData;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            FormHelper.NavigateBack();
        }
    }
}
