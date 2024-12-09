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
        private Form FormMain; // Ana form / Main form

        // Yapıcı metod, kullanıcı adını alır / Constructor method, takes the username
        public AdminHomeForm(string username, Form Main)
        {
            InitializeComponent(); // Bileşenleri başlat / Initialize components
            FormMain = Main; // Ana formu ayarla / Set the main form
        }

        // Kullanıcı yönetim butonuna tıklandığında / When the manage users button is clicked
        private void btnManageUsers_Click(object sender, EventArgs e)
        {
            UserManagementForm manageUsersForm = new UserManagementForm(); // Kullanıcı yönetim formunu oluştur / Create the user management form
            FormHelper.Navigate(manageUsersForm); // Formu göster / Show the form
        }

        // Raporlar ve sistem günlükleri butonuna tıklandığında / When the reports and system logs button is clicked
        private void btnReportsandSystenLogs_Click(object sender, EventArgs e)
        {
            ReportsForm reportsForm = new ReportsForm(); // Raporlar formunu oluştur / Create the reports form
            FormHelper.Navigate(reportsForm); // Formu göster / Show the form
        }

        // Sistem yönetimi butonuna tıklandığında / When the system management button is clicked
        private void btnManageSystem_Click(object sender, EventArgs e)
        {
            using (ProgressDialog progressDialog = new ProgressDialog())
            {
                progressDialog.Show();
                progressDialog.UpdateStatus("Sistem Ayarları Yükleniyor...");

                try
                {
                    SystemMaintenanceForm systemMaintenanceForm = new SystemMaintenanceForm(); // Sistem bakım formunu oluştur / Create the system maintenance form
                    FormHelper.Navigate(systemMaintenanceForm); // Formu göster / Show the form
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Sistem ayarları yüklenirken bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    progressDialog.Close();
                }
            }
        }

        // Üye yönetimi butonuna tıklandığında / When the member management button is clicked
        private void btnManageMembers_Click(object sender, EventArgs e)
        {
            MemberManagementForm memberManagementForm = new MemberManagementForm(); // Üye yönetim formunu oluştur / Create the member management form
            FormHelper.Navigate(memberManagementForm); // Formu göster / Show the form
        }

        // Kitap yönetimi butonuna tıklandığında / When the book management button is clicked
        private void btnManageBooks_Click(object sender, EventArgs e)
        {
            BookManagementForm bookManagementForm = new BookManagementForm(); // Kitap yönetim formunu oluştur / Create the book management form
            FormHelper.Navigate(bookManagementForm); // Formu göster / Show the form
        }

        // Ödünç yönetimi butonuna tıklandığında / When the loan management button is clicked
        private void btnManageLoans_Click(object sender, EventArgs e)
        {
            LoanManagementForm loanManagementForm = new LoanManagementForm(); // Ödünç yönetim formunu oluştur / Create the loan management form
            FormHelper.Navigate(loanManagementForm); // Formu göster / Show the form
        }

        // Ceza yönetimi butonuna tıklandığında / When the fine management button is clicked
        private void btnManageFine_Click(object sender, EventArgs e)
        {
            ManageFinesForm manageFinesForm = new ManageFinesForm(); // Ceza yönetim formunu oluştur / Create the fine management form
            FormHelper.Navigate(manageFinesForm); // Formu göster / Show the form
        }

        // Çıkış butonuna tıklandığında / When the logout button is clicked
        private void btnLogOut_Click(object sender, EventArgs e)
        {
            FormHelper.Logout(FormMain); // Kullanıcıyı çıkış yaptır / Log out the user
        }
    }
}
