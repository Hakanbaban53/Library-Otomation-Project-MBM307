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
    public partial class ProgressDialog : Form
    {
        public ProgressDialog()
        {
            InitializeComponent();
            lblStatus.Text = "İşleniyor...";
            progressBar.Style = ProgressBarStyle.Marquee; // Indeterminate progress
            progressBar.MarqueeAnimationSpeed = 30;       // Animation speed
        }

        public void UpdateStatus(string message)
        {
            lblStatus.Text = message;
            Application.DoEvents(); // Refresh UI immediately
        }

        public void UpdateLabel(string message)
        {
            lblStatus.Text = message;
        }

    }
}
