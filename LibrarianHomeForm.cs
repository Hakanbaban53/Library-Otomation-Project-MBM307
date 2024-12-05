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
        // Geçerli kullanıcı adını saklamak için bir değişken
        private string currentUsername;
        private Form FormMain; // Ana form
        // Formun yapıcı metodu, kullanıcı adını alır
        public LibrarianHomeForm(string username, Form Main)
        {
            InitializeComponent(); // Bileşenleri başlat
            currentUsername = username; // Kullanıcı adını ayarla
            FormMain = Main; // Ana formu ayarla
            Greetings_Load(); // Hoşgeldin mesajını yükle
        }

        // Hoşgeldin mesajını ayarlayan metod
        private void Greetings_Load()
        {
            lblGreeting.Text = $"Hoşgeldiniz, {currentUsername}!"; // Mesajı güncelle
        }

        // Çıkış butonuna tıklandığında çağrılan metod
        private void btnLogOut_Click(object sender, EventArgs e)
        {
            FormHelper.Logout(FormMain); // Kullanıcıyı çıkış yaptır
        }

        // Kitap yönetim butonuna tıklandığında çağrılan metod
        private void btnManageBooks_Click(object sender, EventArgs e)
        {
            BookManagementForm bookManagementForm = new BookManagementForm(); // Yeni kitap yönetim formu oluştur
            FormHelper.Navigate(bookManagementForm); // Formu göster
        }

        // Kredi yönetim butonuna tıklandığında çağrılan metod
        private void btnManageLoans_Click(object sender, EventArgs e)
        {
            LoanManagementForm loanManagementForm = new LoanManagementForm(); // Yeni kredi yönetim formu oluştur
            FormHelper.Navigate(loanManagementForm); // Formu göster
        }

        // Ceza yönetim butonuna tıklandığında çağrılan metod
        private void btnManageFine_Click(object sender, EventArgs e)
        {
            ManageFinesForm manageFinesForm = new ManageFinesForm(); // Yeni ceza yönetim formu oluştur
            FormHelper.Navigate(manageFinesForm); // Formu göster
        }

        // Hesap yönetim butonuna tıklandığında çağrılan metod
        private void btnManageAccount_Click(object sender, EventArgs e)
        {
            ManageAccountForm manageAccountForm = new ManageAccountForm(currentUsername); // Yeni hesap yönetim formu oluştur
            FormHelper.Navigate(manageAccountForm); // Formu göster
        }

        // Sistem günlükleri butonuna tıklandığında çağrılan metod
        private void btnSystemLogs_Click(object sender, EventArgs e)
        {
            ReportsForm reportsForm = new ReportsForm(); // Yeni rapor formu oluştur
            FormHelper.Navigate(reportsForm); // Formu göster
        }
    }
}
