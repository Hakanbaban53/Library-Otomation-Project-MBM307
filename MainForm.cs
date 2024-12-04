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
        private readonly string role;
        private readonly string username;

        public MainForm(string role, string username)
        {
            InitializeComponent();
            this.role = role;
            this.username = username;
            FormHelper.OnFormChange += ShowFormInPanel; 

            if (role == "Admin")
            {
                AdminHomeForm adminHome = new AdminHomeForm(username);
                FormHelper.Navigate(adminHome);
            }
            else if (role == "Librarian")
            {
                LibrarianHomeForm librarianHome = new LibrarianHomeForm(username);
                FormHelper.Navigate(librarianHome);
            }
        }

        // Panel üzerine form gösterimi
        private void ShowFormInPanel(Form form)
        {
            mainPanel.Controls.Clear();  // Önceki formu temizle

            form.TopLevel = false;  // Alt form
            form.FormBorderStyle = FormBorderStyle.None;  // Çerçevesiz form
            form.Dock = DockStyle.Fill;  // Panel üzerine formu tam sığdır

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
