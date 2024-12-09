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
    // ProgressDialog sınıfı / ProgressDialog class
    public partial class ProgressDialog : Form
    {
        // Yapıcı metod / Constructor
        public ProgressDialog()
        {
            InitializeComponent();
            lblStatus.Text = "İşleniyor..."; // Status label text / Durum etiket metni
            progressBar.Style = ProgressBarStyle.Marquee; // Belirsiz ilerleme / Indeterminate progress
            progressBar.MarqueeAnimationSpeed = 30;       // Animasyon hızı / Animation speed
        }

        // Durumu güncelleme metodu / Method to update status
        public void UpdateStatus(string message)
        {
            lblStatus.Text = message; // Durum etiketini güncelle / Update status label
            Application.DoEvents(); // UI'yi hemen yenile / Refresh UI immediately
        }

        // Etiketi güncelleme metodu / Method to update label
        public void UpdateLabel(string message)
        {
            lblStatus.Text = message; // Etiket metnini güncelle / Update label text
        }
    }
}
