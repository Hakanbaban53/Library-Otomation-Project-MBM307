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
            cmbReportType.Items.Add("Most Borrowed Books");
            cmbReportType.Items.Add("Active Members");
            cmbReportType.Items.Add("Overdue Loans");
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
                case "Most Borrowed Books":
                    reportData = db.ExecuteQuery("SELECT * FROM MostBorrowedBooksView ORDER BY BorrowCount DESC", null, false);
                    break;
                case "Active Members":
                    reportData = db.ExecuteQuery("SELECT * FROM ActiveMembersView ORDER BY ActiveLoans DESC", null, false);
                    break;
                case "Overdue Loans":
                    reportData = db.ExecuteQuery("SELECT * FROM ViewOverdueLoans", null, false);
                    break;
                default:
                    MessageBox.Show("Invalid report selection.");
                    return;
            }

            dataGridReports.DataSource = reportData;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
