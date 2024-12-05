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
        private Form FormMain; // Ana form
        // Yapıcı metod, kullanıcı adını alır
        public AdminHomeForm(string username, Form Main)
        {
            InitializeComponent(); // Bileşenleri başlat
            FormMain = Main; // Ana formu ayarla
        }

        // Kullanıcı yönetim butonuna tıklandığında
        private void btnManageUsers_Click(object sender, EventArgs e)
        {
            UserManagementForm manageUsersForm = new UserManagementForm(); // Kullanıcı yönetim formunu oluştur
            FormHelper.Navigate(manageUsersForm); // Formu göster
        }

        // Raporlar ve sistem günlükleri butonuna tıklandığında
        private void btnReportsandSystenLogs_Click(object sender, EventArgs e)
        {
            ReportsForm reportsForm = new ReportsForm(); // Raporlar formunu oluştur
            FormHelper.Navigate(reportsForm); // Formu göster
        }

        // Sistem yönetimi butonuna tıklandığında
        private void btnManageSystem_Click(object sender, EventArgs e)
        {
            SystemMaintenanceForm systemMaintenanceForm = new SystemMaintenanceForm(); // Sistem bakım formunu oluştur
            FormHelper.Navigate(systemMaintenanceForm); // Formu göster
        }

        // Üye yönetimi butonuna tıklandığında
        private void btnManageMembers_Click(object sender, EventArgs e)
        {
            MemberManagementForm memberManagementForm = new MemberManagementForm(); // Üye yönetim formunu oluştur
            FormHelper.Navigate(memberManagementForm); // Formu göster
        }

        // Kitap yönetimi butonuna tıklandığında
        private void btnManageBooks_Click(object sender, EventArgs e)
        {
            BookManagementForm bookManagementForm = new BookManagementForm(); // Kitap yönetim formunu oluştur
            FormHelper.Navigate(bookManagementForm); // Formu göster
        }

        // Ödünç yönetimi butonuna tıklandığında
        private void btnManageLoans_Click(object sender, EventArgs e)
        {
            LoanManagementForm loanManagementForm = new LoanManagementForm(); // Ödünç yönetim formunu oluştur
            FormHelper.Navigate(loanManagementForm); // Formu göster
        }

        // Ceza yönetimi butonuna tıklandığında
        private void btnManageFine_Click(object sender, EventArgs e)
        {
            ManageFinesForm manageFinesForm = new ManageFinesForm(); // Ceza yönetim formunu oluştur
            FormHelper.Navigate(manageFinesForm); // Formu göster
        }

        // Çıkış butonuna tıklandığında
        private void btnLogOut_Click(object sender, EventArgs e)
        {
            FormHelper.Logout(FormMain); // Kullanıcıyı çıkış yaptır
        }
    }
}
