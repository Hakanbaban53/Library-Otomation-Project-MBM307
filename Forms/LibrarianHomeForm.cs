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
        // Geçerli kullanıcı adını saklamak için bir değişken / A variable to store the current username
        private string currentUsername;
        private Form FormMain; // Ana form / Main form

        // Formun yapıcı metodu, kullanıcı adını alır / Constructor method of the form, takes the username
        public LibrarianHomeForm(string username, Form Main)
        {
            InitializeComponent(); // Bileşenleri başlat / Initialize components
            currentUsername = username; // Kullanıcı adını ayarla / Set the username
            FormMain = Main; // Ana formu ayarla / Set the main form
            Greetings_Load(); // Hoşgeldin mesajını yükle / Load the greeting message
        }

        // Hoşgeldin mesajını ayarlayan metod / Method to set the greeting message
        private void Greetings_Load()
        {
            lblGreeting.Text = $"Hoşgeldiniz, {currentUsername}!"; // Mesajı güncelle / Update the message
        }

        // Çıkış butonuna tıklandığında çağrılan metod / Method called when the logout button is clicked
        private void btnLogOut_Click(object sender, EventArgs e)
        {
            FormHelper.Logout(FormMain); // Kullanıcıyı çıkış yaptır / Log out the user
        }

        // Kitap yönetim butonuna tıklandığında çağrılan metod / Method called when the manage books button is clicked
        private void btnManageBooks_Click(object sender, EventArgs e)
        {
            BookManagementForm bookManagementForm = new BookManagementForm(); // Yeni kitap yönetim formu oluştur / Create a new book management form
            FormHelper.Navigate(bookManagementForm); // Formu göster / Show the form
        }

        // Kredi yönetim butonuna tıklandığında çağrılan metod / Method called when the manage loans button is clicked
        private void btnManageLoans_Click(object sender, EventArgs e)
        {
            LoanManagementForm loanManagementForm = new LoanManagementForm(); // Yeni kredi yönetim formu oluştur / Create a new loan management form
            FormHelper.Navigate(loanManagementForm); // Formu göster / Show the form
        }

        // Ceza yönetim butonuna tıklandığında çağrılan metod / Method called when the manage fines button is clicked
        private void btnManageFine_Click(object sender, EventArgs e)
        {
            ManageFinesForm manageFinesForm = new ManageFinesForm(); // Yeni ceza yönetim formu oluştur / Create a new fine management form
            FormHelper.Navigate(manageFinesForm); // Formu göster / Show the form
        }

        // Hesap yönetim butonuna tıklandığında çağrılan metod / Method called when the manage account button is clicked
        private void btnManageAccount_Click(object sender, EventArgs e)
        {
            ManageAccountForm manageAccountForm = new ManageAccountForm(currentUsername); // Yeni hesap yönetim formu oluştur / Create a new account management form
            FormHelper.Navigate(manageAccountForm); // Formu göster / Show the form
        }

        // Sistem günlükleri butonuna tıklandığında çağrılan metod / Method called when the system logs button is clicked
        private void btnSystemLogs_Click(object sender, EventArgs e)
        {
            ReportsForm reportsForm = new ReportsForm(); // Yeni rapor formu oluştur / Create a new reports form
            FormHelper.Navigate(reportsForm); // Formu göster / Show the form
        }
    }
}
