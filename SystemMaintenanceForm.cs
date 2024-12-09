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
using System.Configuration;
using System.IO;
using System.Management;

namespace Library_Otomation
{
    public partial class SystemMaintenanceForm : Form
    {
        // CPU Kullanımını izlemek için performans sayacı oluştur / Create a performance counter to monitor CPU usage
        private PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        // Mevcut bellek miktarını izlemek için performans sayacı oluştur / Create a performance counter to monitor available memory
        private PerformanceCounter memCounter = new PerformanceCounter("Memory", "Available MBytes");

        // Formun yapıcı metodu / Constructor for the form
        public SystemMaintenanceForm()
        {
            InitializeComponent();
            InitializeCharts(); // Grafikleri başlat / Initialize charts  
            InitializeTimer(); // Zamanlayıcıyı başlat / Start the timer  
        }

        // Grafik serilerini başlat / Initialize chart series
        public void InitializeCharts()
        {
            chartCpu.Series.Clear(); // CPU grafiği serilerini temizle / Clear CPU chart series  
            chartMemory.Series.Clear(); // Bellek grafiği serilerini temizle / Clear memory chart series  
            chartStorage.Series.Clear(); // Depolama grafiği serilerini temizle / Clear storage chart series  

            AddChartSeries(chartCpu, "CPU Usage"); // CPU Kullanım serisini ekle / Add CPU Usage series  
            AddChartSeries(chartMemory, "Memory Usage"); // Mevcut Bellek serisini ekle / Add Available Memory series  
            AddChartSeries(chartStorage, "Storage Usage"); // Depolama Kullanım serisini ekle / Add Storage Usage series  
        }

        // Grafik serisini ekle / Add a chart series
        private void AddChartSeries(Chart chart, string seriesName)
        {
            chart.Series.Add(seriesName); // Seriyi ekle / Add the series  
            chart.Series[seriesName].ChartType = SeriesChartType.Line; // Grafik türünü ayarla / Set the chart type  
            chart.Series[seriesName].BorderWidth = 3; // Kenar genişliğini ayarla / Set the border width  
            if (seriesName == "CPU Usage") // Eğer seri adı "CPU Usage" ise / If the series name is "CPU Usage"  
            {
                chart.ChartAreas[0].AxisY.Maximum = 100; // Y ekseninin maksimum değerini ayarla / Set the maximum value for the Y axis  
            }
        }

        public void InitializeTimer()
        {
            performanceTimer.Interval = 1000;
            performanceTimer.Tick += PerformanceTimer_Tick;
            performanceTimer.Start();
        }

        private async void RestoreDatabaseAsync()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Backup Files (*.bak)|*.bak|All Files (*.*)|*.*";
                openFileDialog.Title = "Yedek Dosyası Seçin";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string backupPath = openFileDialog.FileName;
                    await ExecuteDatabaseRestore(backupPath);
                }
            }
        }

        private async Task ExecuteDatabaseRestore(string backupPath)
        {
            using (ProgressDialog progressDialog = new ProgressDialog())
            {
                progressDialog.Show();
                progressDialog.UpdateStatus("Veritabanı geri yükleniyor...");

                bool rollbackRequired = false;

                try
                {
                    await Task.Run(() =>
                    {
                        using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LibraryAutomationDB"].ConnectionString))
                        {
                            conn.Open();
                            using (SqlCommand cmd = new SqlCommand { Connection = conn })
                            {
                                // Switch to master database
                                cmd.CommandText = "USE master;";
                                cmd.ExecuteNonQuery();

                                // Set the database to single-user mode
                                cmd.CommandText = "ALTER DATABASE [LibraryDB] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;";
                                cmd.ExecuteNonQuery();

                                // Indicate rollback is required if something fails beyond this point
                                rollbackRequired = true;

                                // Restore the database
                                cmd.CommandText = $"RESTORE DATABASE [LibraryDB] FROM DISK = '{backupPath}' WITH REPLACE;";
                                cmd.ExecuteNonQuery();

                                // Successfully restored, no rollback needed
                                rollbackRequired = false;

                                // Set the database back to multi-user mode
                                cmd.CommandText = "ALTER DATABASE [LibraryDB] SET MULTI_USER;";
                                cmd.ExecuteNonQuery();
                            }
                        }
                    });

                    MessageBox.Show("Veritabanı geri yükleme işlemi başarıyla tamamlandı!", "Başarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    if (rollbackRequired)
                    {
                        try
                        {
                            await Task.Run(() =>
                            {
                                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LibraryAutomationDB"].ConnectionString))
                                {
                                    conn.Open();
                                    using (SqlCommand cmd = new SqlCommand { Connection = conn })
                                    {
                                        // Ensure the database is back to multi-user mode
                                        cmd.CommandText = "ALTER DATABASE [LibraryDB] SET MULTI_USER;";
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                            });
                        }
                        catch (Exception rollbackEx)
                        {
                            MessageBox.Show($"Rollback işlemi başarısız oldu: {rollbackEx.Message}", "Rollback Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                    MessageBox.Show($"Geri yükleme başarısız oldu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    progressDialog.Close();
                }
            }
        }

        private async void BackupDatabaseAsync()
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Backup Files (*.bak)|*.bak|All Files (*.*)|*.*";
                saveFileDialog.Title = "Yedek Dosyası Konumunu Seçin";
                saveFileDialog.FileName = "LibraryDBBackup.bak";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string backupPath = saveFileDialog.FileName;
                    await ExecuteDatabaseBackup(backupPath);
                }
            }
        }

        private async Task ExecuteDatabaseBackup(string backupPath)
        {
            using (ProgressDialog progressDialog = new ProgressDialog())
            {
                progressDialog.Show();
                progressDialog.UpdateStatus("Veritabanı yedekleniyor...");

                try
                {
                    await Task.Run(() =>
                    {
                        DatabaseHelper db = new DatabaseHelper();
                        SqlParameter[] parameters = { new SqlParameter("@BackupPath", backupPath) };
                        db.ExecuteNonQuery("BackupDatabase", parameters, true);
                    });
                    MessageBox.Show("Veritabanı yedekleme işlemi başarıyla tamamlandı!", "Başarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Yedekleme başarısız oldu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    progressDialog.Close();
                }
            }
        }

        private void btnBackupDatabase_Click(object sender, EventArgs e) => BackupDatabaseAsync();
        private void btnRestoreDatabase_Click(object sender, EventArgs e) => RestoreDatabaseAsync();

        private void PerformanceTimer_Tick(object sender, EventArgs e)
        {
            UpdatePerformanceCharts();
            UpdateUptimeLabel();
        }

        private void UpdatePerformanceCharts()
        {
            float cpuUsage = cpuCounter.NextValue();
            float availableMemory = memCounter.NextValue();
            ulong totalMemory = GetTotalPhysicalMemory();
            float usedMemory = totalMemory - availableMemory; 

            chartCpu.Series["CPU Usage"].Points.AddY(cpuUsage);
            chartMemory.Series["Memory Usage"].Points.AddY(usedMemory);
            UpdateStorageUsageChart();

            const int maxPoints = 60;
            if (chartCpu.Series["CPU Usage"].Points.Count > maxPoints)
            {
                RemoveOldPoints(maxPoints);
            }

            ResetChartAutoValues();
        }

        private ulong GetTotalPhysicalMemory()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystem");
            foreach (ManagementObject share in searcher.Get())
            {
                return (ulong)share["TotalPhysicalMemory"] / (1024 * 1024);
            }
            return 0;
        }


        private void UpdateStorageUsageChart()
        {
            var drive = new DriveInfo("C");
            float storageUsage = (drive.TotalSize - drive.AvailableFreeSpace) / (1024 * 1024 * 1024) * 100;
            chartStorage.Series["Storage Usage"].Points.AddY(storageUsage);
        }

        private void RemoveOldPoints(int maxPoints)
        {
            chartCpu.Series["CPU Usage"].Points.RemoveAt(0);
            chartMemory.Series["Memory Usage"].Points.RemoveAt(0);
            chartStorage.Series["Storage Usage"].Points.RemoveAt(0);
        }

        private void ResetChartAutoValues()
        {
            chartCpu.ResetAutoValues();
            chartMemory.ResetAutoValues();
            chartStorage.ResetAutoValues();
        }

        private void UpdateUptimeLabel()
        {
            TimeSpan uptime = TimeSpan.FromMilliseconds(Environment.TickCount);
            lblUptime.Text = $"Uptime: {uptime:dd\\.hh\\:mm\\:ss}";
        }

        private void btnBack_Click(object sender, EventArgs e){
            performanceTimer.Stop(); // Zamanlayıcıyı durdur / Stop the timer
            performanceTimer.Dispose(); // Zamanlayıcıyı serbest bırak / Dispose the timer
            FormHelper.NavigateBack();
        }
        private void btnEditSettingsKey_Click(object sender, EventArgs e) => ManageSettingsKeys();


        // Sistem ayarlarını yönet / Manage system settings
        private void ManageSettingsKeys()
        {
            using (Form manageSettingsKeysForm = CreateManageSettingsForm())
            {
                PopulateSettingsList(manageSettingsKeysForm);
                manageSettingsKeysForm.ShowDialog();
            }
        }

        private Form CreateManageSettingsForm()
        {
            Form manageSettingsKeysForm = new Form
            {
                Text = "Sistem Ayarlarını Yönet",
                StartPosition = FormStartPosition.CenterParent,
                Width = 500,
                Height = 500,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                Padding = new Padding(10)
            };

            Label titleLabel = new Label
            {
                Text = "Mevcut Ayarlar",
                Font = new Font("Microsoft Sans Serif", 14, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 40,
                TextAlign = ContentAlignment.MiddleCenter
            };

            ListBox listBox = new ListBox
            {
                Dock = DockStyle.Top,
                Font = new Font("Microsoft Sans Serif", 12),
                Height = 250
            };

            Label instructionsLabel = new Label
            {
                Text = "Bir anahtar seçin veya yeni bir anahtar ekleyin.",
                Font = new Font("Microsoft Sans Serif", 10),
                Dock = DockStyle.Top,
                Height = 30,
                TextAlign = ContentAlignment.MiddleLeft
            };

            TextBox txtSettingKey = CreateTextBox("Anahtar adı girin...");
            TextBox txtSettingValue = CreateTextBox("Anahtar değeri girin...");

            FlowLayoutPanel buttonPanel = CreateButtonPanel(txtSettingKey, txtSettingValue, listBox);

            manageSettingsKeysForm.Controls.Add(buttonPanel);
            manageSettingsKeysForm.Controls.Add(txtSettingValue);
            manageSettingsKeysForm.Controls.Add(txtSettingKey);
            manageSettingsKeysForm.Controls.Add(instructionsLabel);
            manageSettingsKeysForm.Controls.Add(listBox);
            manageSettingsKeysForm.Controls.Add(titleLabel);

            return manageSettingsKeysForm;
        }

        private TextBox CreateTextBox(string defaultText)
        {
            TextBox textBox = new TextBox
            {
                Dock = DockStyle.Top,
                Margin = new Padding(10),
                Font = new Font("Microsoft Sans Serif", 12),
                Text = defaultText,
                ForeColor = Color.Gray
            };

            textBox.GotFocus += (s, e) =>
            {
                if (textBox.Text == defaultText)
                {
                    textBox.Text = "";
                    textBox.ForeColor = Color.Black;
                }
            };

            textBox.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    textBox.Text = defaultText;
                    textBox.ForeColor = Color.Gray;
                }
            };

            return textBox;
        }

        private FlowLayoutPanel CreateButtonPanel(TextBox txtSettingKey, TextBox txtSettingValue, ListBox listBox)
        {
            FlowLayoutPanel buttonPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(10),
                Height = 60
            };

            Button btnDelete = CreateButton("Sil");
            Button btnAdd = CreateButton("Ekle");
            Button btnUpdate = CreateButton("Güncelle");
            Button btnExit = CreateButton("Çıkış", DialogResult.Cancel);

            btnAdd.Click += (s, e) => AddSetting(txtSettingKey, txtSettingValue, listBox);
            btnDelete.Click += (s, e) => DeleteSetting(listBox, txtSettingKey, txtSettingValue);
            btnUpdate.Click += (s, e) => UpdateSetting(listBox, txtSettingValue);
            listBox.SelectedIndexChanged += (s, e) => UpdateTextBoxes(listBox, txtSettingKey, txtSettingValue);

            buttonPanel.Controls.Add(btnExit);
            buttonPanel.Controls.Add(btnDelete);
            buttonPanel.Controls.Add(btnAdd);
            buttonPanel.Controls.Add(btnUpdate);

            return buttonPanel;
        }

        private Button CreateButton(string text, DialogResult dialogResult = DialogResult.None)
        {
            return new Button
            {
                Text = text,
                AutoSize = true,
                Font = new Font("Microsoft Sans Serif", 12),
                DialogResult = dialogResult
            };
        }

        private void PopulateSettingsList(Form manageSettingsKeysForm)
        {
            ListBox listBox = (ListBox)manageSettingsKeysForm.Controls.OfType<ListBox>().FirstOrDefault();
            DatabaseHelper db = new DatabaseHelper();
            DataTable settings = db.ExecuteQuery("SELECT * FROM Settings", null, false);
            foreach (DataRow row in settings.Rows)
            {
                listBox.Items.Add(new { Key = row["SettingKey"].ToString(), Value = row["SettingValue"].ToString() });
            }
            listBox.DisplayMember = "Key";
        }

        private void AddSetting(TextBox txtSettingKey, TextBox txtSettingValue, ListBox listBox)
        {
            try
            {
                string key = txtSettingKey.Text.Trim();
                string value = txtSettingValue.Text.Trim();

                if (string.IsNullOrWhiteSpace(key) || key == "Anahtar adı girin...")
                    throw new Exception("Lütfen geçerli bir anahtar adı girin.");

                DatabaseHelper db = new DatabaseHelper();
                SqlParameter[] checkParams = { new SqlParameter("@SettingKey", key) };
                DataTable existingKey = db.ExecuteQuery("SELECT * FROM Settings WHERE SettingKey = @SettingKey", checkParams, false);

                if (existingKey.Rows.Count > 0)
                    throw new Exception("Bu anahtar zaten mevcut.");

                SqlParameter[] parameters = {
                        new SqlParameter("@SettingKey", key),
                        new SqlParameter("@SettingValue", value)
                    };
                db.ExecuteNonQuery("INSERT INTO Settings (SettingKey, SettingValue) VALUES (@SettingKey, @SettingValue)", parameters, false);
                listBox.Items.Add(new { Key = key, Value = value });
                MessageBox.Show("Anahtar başarıyla eklendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtSettingKey.Clear();
                txtSettingValue.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteSetting(ListBox listBox, TextBox txtSettingKey, TextBox txtSettingValue)
        {
            if (listBox.SelectedItem != null)
            {
                dynamic selectedItem = listBox.SelectedItem;
                string key = selectedItem.Key;

                DatabaseHelper db = new DatabaseHelper();
                SqlParameter[] parameters = { new SqlParameter("@SettingKey", key) };
                db.ExecuteNonQuery("DELETE FROM Settings WHERE SettingKey = @SettingKey", parameters, false);
                listBox.Items.Remove(listBox.SelectedItem);
                MessageBox.Show("Anahtar başarıyla silindi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtSettingKey.Clear();
                txtSettingValue.Clear();
            }
            else
            {
                MessageBox.Show("Lütfen silmek için bir anahtar seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void UpdateSetting(ListBox listBox, TextBox txtSettingValue)
        {
            if (listBox.SelectedItem != null)
            {
                dynamic selectedItem = listBox.SelectedItem;
                string key = selectedItem.Key;
                string newValue = txtSettingValue.Text.Trim();

                if (string.IsNullOrWhiteSpace(newValue) || newValue == "Anahtar değeri girin...")
                {
                    MessageBox.Show("Lütfen geçerli bir değer girin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DatabaseHelper db = new DatabaseHelper();
                SqlParameter[] parameters = { new SqlParameter("@SettingKey", key), new SqlParameter("@SettingValue", newValue) };
                db.ExecuteNonQuery("UPDATE Settings SET SettingValue = @SettingValue WHERE SettingKey = @SettingKey", parameters, false);
                listBox.Items[listBox.SelectedIndex] = new { Key = key, Value = newValue };
                MessageBox.Show("Anahtar başarıyla güncellendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtSettingValue.Clear();
            }
            else
            {
                MessageBox.Show("Lütfen güncellemek için bir anahtar seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void UpdateTextBoxes(ListBox listBox, TextBox txtSettingKey, TextBox txtSettingValue)
        {
            if (listBox.SelectedItem != null)
            {
                dynamic selectedItem = listBox.SelectedItem;
                txtSettingKey.Text = selectedItem.Key;
                txtSettingValue.Text = selectedItem.Value;
            }
        }
    }
}

