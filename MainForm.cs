using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Library_Otomation
{
    public partial class MainForm : Form
    {
        private string role; // Kullanıcı rolü
        private string username; // Kullanıcı adı

        public MainForm(string role, string username) // MainForm constructor
        {
            InitializeComponent(); // MainForm'u başlat
            this.role = role; // Kullanıcı rolünü al
            this.username = username; // Kullanıcı adını al
            FormHelper.OnFormChange += ShowFormInPanel;  // Form değiştiğinde ShowFormInPanel metodunu çağır

            if (role == "Admin") // Eğer kullanıcı rolü Admin ise
            {
                AdminHomeForm adminHome = new AdminHomeForm(username, this); // AdminHomeForm'u başlat
                FormHelper.Navigate(adminHome); // AdminHomeForm'u göster 
            }
            else if (role == "Librarian") // Eğer kullanıcı rolü Librarian ise 
            {
                LibrarianHomeForm librarianHome = new LibrarianHomeForm(username, this); // LibrarianHomeForm'u başlat
                FormHelper.Navigate(librarianHome); // LibrarianHomeForm'u göster
            }
        }

        // Panel üzerine form gösterimi
        private void ShowFormInPanel(Form form)
        {
            if (mainPanel.InvokeRequired)
            {
                mainPanel.Invoke(new Action(() => ShowFormInPanel(form)));
                return;
            }

            mainPanel.Controls.Clear();  // Önceki formu temizle

            form.TopLevel = false;  // Alt form
            form.FormBorderStyle = FormBorderStyle.None;  // Çerçevesiz form
            form.Dock = DockStyle.Fill;  // Panel üzerine formu tam sığdır
            this.Text = form.Text;  // Form başlığını değiştir

            mainPanel.Controls.Add(form);  // Panel'e formu ekle
            form.BringToFront();  // Formu en öne getir
            form.Show();  // Formu göster
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit(); // MainForm kapanınca uygulamayı kapat
        }
    }
}
