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
        public AdminHomeForm()
        {
            InitializeComponent();
        }

        private void OpenChildForm(Form childForm)
        {
            childForm.FormClosed += (s, args) => this.Show();

            this.Hide();
            childForm.Show();
        }

        private void btnManageUsers_Click(object sender, EventArgs e)
        {
            UserManagementForm manageUsersForm = new UserManagementForm();
            OpenChildForm(manageUsersForm);
        }

        private void btnReportsandSystenLogs_Click(object sender, EventArgs e)
        {
            ReportsForm reportsForm = new ReportsForm();
            OpenChildForm(reportsForm);
        }

        private void btnManageSystem_Click(object sender, EventArgs e)
        {
            SystemMaintenanceForm systemMaintenanceForm = new SystemMaintenanceForm();
            OpenChildForm(systemMaintenanceForm);
        }

        private void btnManageMembers_Click(object sender, EventArgs e)
        {
            MemberManagementForm memberManagementForm = new MemberManagementForm();
            OpenChildForm(memberManagementForm);
        }

        private void btnManageBooks_Click(object sender, EventArgs e)
        {
            BookManagementForm bookManagementForm = new BookManagementForm();
            OpenChildForm(bookManagementForm);
        }

        private void btnManageLoans_Click(object sender, EventArgs e)
        {
            LoanManagementForm loanManagementForm = new LoanManagementForm();
            OpenChildForm(loanManagementForm);
        }
    }
}
