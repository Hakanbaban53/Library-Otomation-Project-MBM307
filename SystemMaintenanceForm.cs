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
using System.Windows.Forms.DataVisualization.Charting;

using System.Diagnostics;

namespace Library_Otomation
{
    public partial class SystemMaintenanceForm : Form
    {
        private PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        private PerformanceCounter memCounter = new PerformanceCounter("Memory", "Available MBytes");

        public SystemMaintenanceForm()
        {
            InitializeComponent();
            chartPerformance.Series.Clear();
            chartPerformance.Series.Add("CPU Usage");
            chartPerformance.Series["CPU Usage"].ChartType = SeriesChartType.Line;
            chartPerformance.Series["CPU Usage"].BorderWidth = 3; // Increase line width

            chartPerformance.Series.Add("Available Memory");
            chartPerformance.Series["Available Memory"].ChartType = SeriesChartType.Line;
            chartPerformance.Series["Available Memory"].BorderWidth = 3; // Increase line width

            performanceTimer.Interval = 1000; // 1 second interval
            performanceTimer.Tick += PerformanceTimer_Tick;
            performanceTimer.Start();

        }

        private bool isMaintenanceMode = false; // Tracks whether maintenance mode is active

        private void btnBackupDatabase_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Backup Files (*.bak)|*.bak";
                saveFileDialog.Title = "Select Backup File Location";
                saveFileDialog.FileName = "LibraryBackup.bak";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string backupPath = saveFileDialog.FileName;
                        DatabaseHelper db = new DatabaseHelper();
                        db.ExecuteNonQuery($"BACKUP DATABASE Library_automation TO DISK = '{backupPath}'", null, false);
                        MessageBox.Show("Database backup completed successfully!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Backup failed: " + ex.Message);
                    }
                }
            }
        }

        private void btnToggleMaintenance_Click(object sender, EventArgs e)
        {
            if (!isMaintenanceMode)
            {
                // Enter Maintenance Mode
                EnterMaintenanceMode();
                btnToggleMaintenance.Text = "Exit Maintenance Mode"; // Change button text
                isMaintenanceMode = true;
            }
            else
            {
                // Exit Maintenance Mode
                ExitMaintenanceMode();
                btnToggleMaintenance.Text = "Enter Maintenance Mode"; // Change button text back
                isMaintenanceMode = false;
            }
        }

        private void EnterMaintenanceMode()
        {
            MessageBox.Show("System is now in Maintenance Mode. Some features may be disabled.");
            // Optional: Disable certain buttons or features
            btnBackupDatabase.Enabled = true;
            btnRestoreDatabase.Enabled = true;
            btnCleanupOldData.Enabled = true;
        }

        private void ExitMaintenanceMode()
        {
            MessageBox.Show("Exited Maintenance Mode. All features are now available.");
            // Optional: Enable all buttons or features again
            btnBackupDatabase.Enabled = false;
            btnRestoreDatabase.Enabled = false;
            btnCleanupOldData.Enabled = false;
        }

        private void btnRestoreDatabase_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Backup Files (*.bak)|*.bak";
                openFileDialog.Title = "Select Backup File";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string backupPath = openFileDialog.FileName;

                        // Switch to master database and restore
                        DatabaseHelper db = new DatabaseHelper();
                        db.ExecuteNonQuery("USE master", null, false);
                        db.ExecuteNonQuery($"ALTER DATABASE Library_automation SET SINGLE_USER WITH ROLLBACK IMMEDIATE", null, false);
                        db.ExecuteNonQuery($"RESTORE DATABASE Library_automation FROM DISK = '{backupPath}' WITH REPLACE", null, false);
                        db.ExecuteNonQuery($"ALTER DATABASE Library_automation SET MULTI_USER", null, false);

                        MessageBox.Show("Database restore completed successfully!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Restore failed: " + ex.Message);
                    }
                }
            }
        }

        private void PerformanceTimer_Tick(object sender, EventArgs e)
        {
            float cpuUsage = cpuCounter.NextValue();
            float availableMemory = memCounter.NextValue();

            chartPerformance.Series["CPU Usage"].Points.AddY(cpuUsage);
            chartPerformance.Series["Available Memory"].Points.AddY(availableMemory);

            // Keep chart size manageable by removing old points
            if (chartPerformance.Series["CPU Usage"].Points.Count > 60)
            {
                chartPerformance.Series["CPU Usage"].Points.RemoveAt(0);
                chartPerformance.Series["Available Memory"].Points.RemoveAt(0);
            }

            chartPerformance.ResetAutoValues();

            // Update uptime label
            TimeSpan uptime = TimeSpan.FromMilliseconds(Environment.TickCount);
            lblUptime.Text = $"Uptime: {uptime:dd\\.hh\\:mm\\:ss}";
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            FormHelper.NavigateBack();
        }
    }
}
