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
        private string role; // Kullanıcı rolü / User role
        private string username; // Kullanıcı adı / Username

        public MainForm(string role, string username) // MainForm constructor / MainForm yapıcı
        {
            InitializeComponent(); // MainForm'u başlat / Initialize MainForm
            this.role = role; // Kullanıcı rolünü al / Get user role
            this.username = username; // Kullanıcı adını al / Get username
            FormHelper.OnFormChange += ShowFormInPanel;  // Form değiştiğinde ShowFormInPanel metodunu çağır / Call ShowFormInPanel method when form changes

            if (role == "Admin") // Eğer kullanıcı rolü Admin ise / If user role is Admin
            {
                AdminHomeForm adminHome = new AdminHomeForm(username, this); // AdminHomeForm'u başlat / Initialize AdminHomeForm
                FormHelper.Navigate(adminHome); // AdminHomeForm'u göster / Show AdminHomeForm
            }
            else if (role == "Librarian") // Eğer kullanıcı rolü Librarian ise / If user role is Librarian
            {
                LibrarianHomeForm librarianHome = new LibrarianHomeForm(username, this); // LibrarianHomeForm'u başlat / Initialize LibrarianHomeForm
                FormHelper.Navigate(librarianHome); // LibrarianHomeForm'u göster / Show LibrarianHomeForm
            }
        }

        // Panel üzerine form gösterimi / Display form on panel
        private void ShowFormInPanel(Form form)
        {
            if (mainPanel.InvokeRequired)
            {
                mainPanel.Invoke(new Action(() => ShowFormInPanel(form)));
                return;
            }

            mainPanel.Controls.Clear();  // Önceki formu temizle / Clear previous form

            form.TopLevel = false;  // Alt form / Child form
            form.FormBorderStyle = FormBorderStyle.None;  // Çerçevesiz form / Borderless form
            form.Dock = DockStyle.Fill;  // Panel üzerine formu tam sığdır / Fit form to panel
            this.Text = form.Text;  // Form başlığını değiştir / Change form title

            mainPanel.Controls.Add(form);  // Panel'e formu ekle / Add form to panel
            form.BringToFront();  // Formu en öne getir / Bring form to front
            form.Show();  // Formu göster / Show form
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit(); // MainForm kapanınca uygulamayı kapat / Close application when MainForm is closed
        }
    }
}
