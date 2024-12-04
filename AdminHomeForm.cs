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
    public partial class AdminHomeForm : Form
    {
        public AdminHomeForm(string username)
        {
            InitializeComponent();
        }

        private void btnManageUsers_Click(object sender, EventArgs e)
        {
            UserManagementForm manageUsersForm = new UserManagementForm();
            FormHelper.Navigate(manageUsersForm);
        }

        private void btnReportsandSystenLogs_Click(object sender, EventArgs e)
        {
            ReportsForm reportsForm = new ReportsForm();
            FormHelper.Navigate(reportsForm);
        }

        private void btnManageSystem_Click(object sender, EventArgs e)
        {
            SystemMaintenanceForm systemMaintenanceForm = new SystemMaintenanceForm();
            FormHelper.Navigate(systemMaintenanceForm);
        }

        private void btnManageMembers_Click(object sender, EventArgs e)
        {
            MemberManagementForm memberManagementForm = new MemberManagementForm();
            FormHelper.Navigate(memberManagementForm);
        }

        private void btnManageBooks_Click(object sender, EventArgs e)
        {
            BookManagementForm bookManagementForm = new BookManagementForm();
            FormHelper.Navigate(bookManagementForm);
        }

        private void btnManageLoans_Click(object sender, EventArgs e)
        {
            LoanManagementForm loanManagementForm = new LoanManagementForm();
            FormHelper.Navigate(loanManagementForm);
        }

        private void btnManageFine_Click(object sender, EventArgs e)
        {
            ManageFinesForm manageFinesForm = new ManageFinesForm();
            FormHelper.Navigate(manageFinesForm);
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            FormHelper.Logout();
        }
    }
}
