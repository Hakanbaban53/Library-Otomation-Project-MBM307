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
    public partial class LibrarianHomeForm : Form
    {
        private string currentUsername;
        public LibrarianHomeForm(string username)
        {
            InitializeComponent();
            currentUsername = username;
            Greetings_Load();
        }

        private void Greetings_Load()
        {
            lblGreeting.Text = $"Hoşgeldiniz, {currentUsername}!";
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            FormHelper.Logout();
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

        private void btnManageAccount_Click(object sender, EventArgs e)
        {
            ManageAccountForm manageAccountForm = new ManageAccountForm(currentUsername);
            FormHelper.Navigate(manageAccountForm);
        }

        private void btnSystemLogs_Click(object sender, EventArgs e)
        {
            ReportsForm reportsForm = new ReportsForm();
            FormHelper.Navigate(reportsForm);
    }
    }
}
