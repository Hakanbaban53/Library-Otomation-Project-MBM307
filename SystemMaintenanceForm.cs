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
using System.Management.Instrumentation;
using System.Management;

namespace Library_Otomation
{
    public partial class SystemMaintenanceForm : Form
    {
        // CPU ve bellek performansını izlemek için sayaçlar
        private PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        private PerformanceCounter memCounter = new PerformanceCounter("Memory", "Available MBytes");

        // Formun yapıcı metodu
        public SystemMaintenanceForm()
        {
            InitializeComponent(); // Bileşenleri başlat
            chartCpu.Series.Clear(); // Mevcut serileri temizle
            chartMemory.Series.Clear();
            chartStorage.Series.Clear();

            chartCpu.Series.Add("CPU Usage"); // CPU kullanımı serisini ekle
            chartCpu.Series["CPU Usage"].ChartType = SeriesChartType.Line; // Çizgi grafik türü
            chartCpu.Series["CPU Usage"].BorderWidth = 3; // Çizgi kalınlığını artır
            chartCpu.ChartAreas[0].AxisY.Maximum = 100; // Y ekseninin maksimum değerini %100 olarak ayarla

            chartMemory.Series.Add("Available Memory"); // Kullanılabilir bellek serisini ekle
            chartMemory.Series["Available Memory"].ChartType = SeriesChartType.Line; // Çizgi grafik türü
            chartMemory.Series["Available Memory"].BorderWidth = 3; // Çizgi kalınlığını artır

            chartStorage.Series.Add("Storage Usage"); // Depolama kullanımı serisini ekle
            chartStorage.Series["Storage Usage"].ChartType = SeriesChartType.Line; // Çizgi grafik türü
            chartStorage.Series["Storage Usage"].BorderWidth = 3; // Çizgi kalınlığını artır

            performanceTimer.Interval = 1000; // 1 saniye aralık
            performanceTimer.Tick += PerformanceTimer_Tick; // Zamanlayıcı tıklama olayını bağla
            performanceTimer.Start(); // Zamanlayıcıyı başlat
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

                    // Modal diyalog oluştur ve göster
                    using (ProgressDialog progressDialog = new ProgressDialog())
                    {
                        progressDialog.Show();
                        progressDialog.UpdateStatus("Veritabanı geri yükleniyor...");

                        try
                        {
                            await Task.Run(() =>
                            {
                                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LibraryAutomationDB"].ConnectionString))
                                {
                                    conn.Open();
                                    using (SqlCommand cmd = new SqlCommand())
                                    {
                                        cmd.Connection = conn;

                                        // Master veritabanına geçiş yap
                                        cmd.CommandText = "USE master;";
                                        cmd.ExecuteNonQuery();

                                        // Veritabanını tek kullanıcı moduna al
                                        cmd.CommandText = @"
                                    ALTER DATABASE [LibraryDB] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;";
                                        cmd.ExecuteNonQuery();

                                        // Veritabanını geri yükle
                                        cmd.CommandText = $@"
                                    RESTORE DATABASE [LibraryDB] FROM DISK = '{backupPath}' WITH REPLACE;";
                                        cmd.ExecuteNonQuery();

                                        // Veritabanını çoklu kullanıcı moduna al
                                        cmd.CommandText = @"
                                    ALTER DATABASE [LibraryDB] SET MULTI_USER;";
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                            });
                            MessageBox.Show("Veritabanı geri yükleme işlemi başarıyla tamamlandı!", "Başarı", MessageBoxButtons.OK, MessageBoxIcon.Information); 
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Geri yükleme başarısız oldu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        finally
                        {
                            progressDialog.Close(); // İşlemden sonra modalı kapat
                        }
                    }
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

                    // Modal diyalog oluştur ve göster
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
                            progressDialog.Close(); // İşlemden sonra modalı kapat
                        }
                    }
                }
            }
        }



        // Veritabanı yedekleme butonuna tıklama olayı
        private void btnBackupDatabase_Click(object sender, EventArgs e)
        {
            BackupDatabaseAsync();
        }

        // Veritabanı geri yükleme butonuna tıklama olayı
        private void btnRestoreDatabase_Click(object sender, EventArgs e)
        {
            RestoreDatabaseAsync();
        }

        // Performans zamanlayıcısının tıklama olayı
        private void PerformanceTimer_Tick(object sender, EventArgs e)
        {
            float cpuUsage = cpuCounter.NextValue(); // CPU kullanımını al
            float availableMemory = memCounter.NextValue(); // Kullanılabilir belleği al

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystem");
            ulong totalMemory = 0;

            foreach (ManagementObject share in searcher.Get())
            {
                totalMemory = (ulong)share["TotalPhysicalMemory"] / (1024 * 1024); // Toplam belleği al (MB cinsinden)
            }

            float usedMemory = totalMemory - availableMemory; // Kullanılan belleği hesapla

            // CPU ve bellek kullanımını grafiğe ekle
            chartCpu.Series["CPU Usage"].Points.AddY(cpuUsage);
            chartMemory.Series["Available Memory"].Points.AddY(usedMemory); // Kullanılan belleği ekle

            // Depolama kullanımını ekle (örnek olarak C: sürücüsünü kullan)
            var drive = new DriveInfo("C");
            float storageUsage = (drive.TotalSize - drive.AvailableFreeSpace) / (1024 * 1024 * 1024) * 100; // Kullanılan depolama oranını % cinsinden hesapla
            chartStorage.Series["Storage Usage"].Points.AddY(storageUsage);

            // Grafik boyutunu yönetilebilir tutmak için eski noktaları kaldır
            const int maxPoints = 60; // Maksimum nokta sayısı
            if (chartCpu.Series["CPU Usage"].Points.Count > maxPoints)
            {
                chartCpu.Series["CPU Usage"].Points.RemoveAt(0);
                chartMemory.Series["Available Memory"].Points.RemoveAt(0);
                chartStorage.Series["Storage Usage"].Points.RemoveAt(0);
            }

            chartCpu.ResetAutoValues(); // Grafik otomatik değerlerini sıfırla
            chartMemory.ResetAutoValues(); // Bellek grafiği otomatik değerlerini sıfırla
            chartStorage.ResetAutoValues(); // Depolama grafiği otomatik değerlerini sıfırla

            // Uptime etiketini güncelle
            TimeSpan uptime = TimeSpan.FromMilliseconds(Environment.TickCount);
            lblUptime.Text = $"Uptime: {uptime:dd\\.hh\\:mm\\:ss}";
        }



        // Geri butonuna tıklama olayı
        private void btnBack_Click(object sender, EventArgs e)
        {
            FormHelper.NavigateBack(); // Geri gitme işlemi
        }

        private void btnEditSettingsKey_Click(object sender, EventArgs e)
        {
            ManageSettingsKeys();
        }


        // Ayarları yönetmek için bir form oluşturur
        private void ManageSettingsKeys()
        {
            // Yeni bir form oluştur ve özelliklerini ayarla
            using (Form manageSettingsKeysForm = new Form
            {
                Text = "Sistem Ayarlarını Yönet", // Form başlığı
                StartPosition = FormStartPosition.CenterParent, // Formun başlangıç konumu
                Width = 500, // Form genişliği
                Height = 500, // Form yüksekliği
                FormBorderStyle = FormBorderStyle.FixedDialog, // Form kenar stili
                MaximizeBox = false, // Maksimize butonunu devre dışı bırak
                MinimizeBox = false, // Minimize butonunu devre dışı bırak
                Padding = new Padding(10) // Formun iç kenar boşlukları
            })
            {
                // Başlık etiketi oluştur
                Label titleLabel = new Label
                {
                    Text = "Mevcut Ayarlar", // Başlık metni
                    Font = new Font("Microsoft Sans Serif", 14, FontStyle.Bold), // Yazı tipi ve boyutu
                    Dock = DockStyle.Top, // Üst kısma yerleştir
                    Height = 40, // Yükseklik
                    TextAlign = ContentAlignment.MiddleCenter // Metni ortala
                };

                // Ayarları listelemek için bir liste kutusu oluştur
                ListBox listBox = new ListBox
                {
                    Dock = DockStyle.Top, // Üst kısma yerleştir
                    Font = new Font("Microsoft Sans Serif", 12), // Yazı tipi ve boyutu
                    Height = 250 // Yükseklik
                };

                // Kullanıcıya talimat vermek için bir etiket oluştur
                Label instructionsLabel = new Label
                {
                    Text = "Bir anahtar seçin veya yeni bir anahtar ekleyin.", // Talimat metni
                    Font = new Font("Microsoft Sans Serif", 10), // Yazı tipi ve boyutu
                    Dock = DockStyle.Top, // Üst kısma yerleştir
                    Height = 30, // Yükseklik
                    TextAlign = ContentAlignment.MiddleLeft // Metni sola yasla
                };

                // Anahtar adı girişi için bir metin kutusu oluştur
                TextBox txtSettingKey = new TextBox
                {
                    Dock = DockStyle.Top, // Üst kısma yerleştir
                    Margin = new Padding(10), // Kenar boşlukları
                    Font = new Font("Microsoft Sans Serif", 12), // Yazı tipi ve boyutu
                    Text = "Anahtar adı girin...", // Varsayılan metin
                    ForeColor = Color.Gray // Varsayılan metin rengi
                };

                // Anahtar değeri girişi için bir metin kutusu oluştur
                TextBox txtSettingValue = new TextBox
                {
                    Dock = DockStyle.Top, // Üst kısma yerleştir
                    Margin = new Padding(10), // Kenar boşlukları
                    Font = new Font("Microsoft Sans Serif", 12), // Yazı tipi ve boyutu
                    Text = "Anahtar değeri girin...", // Varsayılan metin
                    ForeColor = Color.Gray // Varsayılan metin rengi
                };

                // Butonlar için bir akış paneli oluştur
                FlowLayoutPanel buttonPanel = new FlowLayoutPanel
                {
                    Dock = DockStyle.Bottom, // Alt kısma yerleştir
                    FlowDirection = FlowDirection.RightToLeft, // Butonların sağdan sola sıralanması
                    Padding = new Padding(10), // Kenar boşlukları
                    Height = 60 // Yükseklik
                };

                // Sil butonu oluştur
                Button btnDelete = new Button
                {
                    Text = "Sil", // Buton metni
                    AutoSize = true, // Boyutunu otomatik ayarla
                    Font = new Font("Microsoft Sans Serif", 12) // Yazı tipi ve boyutu
                };

                // Ekle butonu oluştur
                Button btnAdd = new Button
                {
                    Text = "Ekle", // Buton metni
                    AutoSize = true, // Boyutunu otomatik ayarla
                    Font = new Font("Microsoft Sans Serif", 12) // Yazı tipi ve boyutu
                };

                // Güncelle butonu oluştur
                Button btnUpdate = new Button
                {
                    Text = "Güncelle", // Buton metni
                    AutoSize = true, // Boyutunu otomatik ayarla
                    Font = new Font("Microsoft Sans Serif", 12) // Yazı tipi ve boyutu
                };

                // Çıkış butonu oluştur
                Button btnExit = new Button
                {
                    Text = "Çıkış", // Buton metni
                    AutoSize = true, // Boyutunu otomatik ayarla
                    Font = new Font("Microsoft Sans Serif", 12), // Yazı tipi ve boyutu
                    DialogResult = DialogResult.Cancel // Çıkış butonu için sonuç
                };

                // Butonları akış paneline ekle
                buttonPanel.Controls.Add(btnExit);
                buttonPanel.Controls.Add(btnDelete);
                buttonPanel.Controls.Add(btnAdd);
                buttonPanel.Controls.Add(btnUpdate);

                // Veritabanı işlemleri için yardımcı sınıf
                DatabaseHelper db = new DatabaseHelper();
                // Ayarları veritabanından al
                DataTable settings = db.ExecuteQuery("SELECT * FROM Settings", null, false);
                foreach (DataRow row in settings.Rows)
                {
                    // Ayarları liste kutusuna ekle
                    listBox.Items.Add(new { Key = row["SettingKey"].ToString(), Value = row["SettingValue"].ToString() });
                }

                // Liste kutusunun gösterim özelliğini ayarla
                listBox.DisplayMember = "Key";

                // Form bileşenlerini ekle
                manageSettingsKeysForm.Controls.Add(buttonPanel);
                manageSettingsKeysForm.Controls.Add(txtSettingValue);
                manageSettingsKeysForm.Controls.Add(txtSettingKey);
                manageSettingsKeysForm.Controls.Add(instructionsLabel);
                manageSettingsKeysForm.Controls.Add(listBox);
                manageSettingsKeysForm.Controls.Add(titleLabel);

                // Anahtar adı metin kutusunun odaklanma olayını yönet
                txtSettingKey.GotFocus += (s, e) =>
                {
                    if (txtSettingKey.Text == "Anahtar adı girin...")
                    {
                        txtSettingKey.Text = ""; // Varsayılan metni temizle
                        txtSettingKey.ForeColor = Color.Black; // Metin rengini siyah yap
                    }
                };
                txtSettingKey.LostFocus += (s, e) =>
                {
                    if (string.IsNullOrWhiteSpace(txtSettingKey.Text))
                    {
                        txtSettingKey.Text = "Anahtar adı girin..."; // Varsayılan metni geri getir
                        txtSettingKey.ForeColor = Color.Gray; // Metin rengini gri yap
                    }
                };

                // Anahtar değeri metin kutusunun odaklanma olayını yönet
                txtSettingValue.GotFocus += (s, e) =>
                {
                    if (txtSettingValue.Text == "Anahtar değeri girin...")
                    {
                        txtSettingValue.Text = ""; // Varsayılan metni temizle
                        txtSettingValue.ForeColor = Color.Black; // Metin rengini siyah yap
                    }
                };
                txtSettingValue.LostFocus += (s, e) =>
                {
                    if (string.IsNullOrWhiteSpace(txtSettingValue.Text))
                    {
                        txtSettingValue.Text = "Anahtar değeri girin..."; // Varsayılan metni geri getir
                        txtSettingValue.ForeColor = Color.Gray; // Metin rengini gri yap
                    }
                };

                // Ekle butonuna tıklama olayı
                btnAdd.Click += (s, e) =>
                {
                    try
                    {
                        string key = txtSettingKey.Text.Trim(); // Anahtar adını al
                        string value = txtSettingValue.Text.Trim(); // Anahtar değerini al

                        // Anahtar adı geçerli mi kontrol et
                        if (string.IsNullOrWhiteSpace(key) || key == "Anahtar adı girin...")
                            throw new Exception("Lütfen geçerli bir anahtar adı girin."); // Hata mesajı

                        // Anahtarın veritabanında mevcut olup olmadığını kontrol et
                        SqlParameter[] checkParams = { new SqlParameter("@SettingKey", key) };
                        DataTable existingKey = db.ExecuteQuery("SELECT * FROM Settings WHERE SettingKey = @SettingKey", checkParams, false);

                        if (existingKey.Rows.Count > 0)
                            throw new Exception("Bu anahtar zaten mevcut."); // Hata mesajı

                        // Yeni anahtarı veritabanına ekle
                        SqlParameter[] parameters = {
                            new SqlParameter("@SettingKey", key),
                            new SqlParameter("@SettingValue", value)
                        };
                        db.ExecuteNonQuery("INSERT INTO Settings (SettingKey, SettingValue) VALUES (@SettingKey, @SettingValue)", parameters, false);
                        listBox.Items.Add(new { Key = key, Value = value }); // Liste kutusuna ekle
                        MessageBox.Show("Anahtar başarıyla eklendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information); // Başarı mesajı
                        txtSettingKey.Clear(); // Anahtar adı kutusunu temizle
                        txtSettingValue.Clear(); // Anahtar değeri kutusunu temizle
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); // Hata mesajı
                    }
                };

                // Sil butonuna tıklama olayı
                btnDelete.Click += (s, e) =>
                {
                    if (listBox.SelectedItem != null) // Seçili bir öğe var mı kontrol et
                    {
                        dynamic selectedItem = listBox.SelectedItem; // Seçili öğeyi al
                        string key = selectedItem.Key; // Anahtar adını al

                        // Anahtarı veritabanından sil
                        SqlParameter[] parameters = { new SqlParameter("@SettingKey", key) };
                        db.ExecuteNonQuery("DELETE FROM Settings WHERE SettingKey = @SettingKey", parameters, false);
                        listBox.Items.Remove(listBox.SelectedItem); // Liste kutusundan kaldır
                        MessageBox.Show("Anahtar başarıyla silindi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information); // Başarı mesajı
                        txtSettingKey.Clear(); // Anahtar adı kutusunu temizle
                        txtSettingValue.Clear(); // Anahtar değeri kutusunu temizle
                    }
                    else
                    {
                        MessageBox.Show("Lütfen silmek için bir anahtar seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning); // Uyarı mesajı
                    }
                };

                // Güncelle butonuna tıklama olayı
                btnUpdate.Click += (s, e) =>
                {
                    if (listBox.SelectedItem != null) // Seçili bir öğe var mı kontrol et
                    {
                        dynamic selectedItem = listBox.SelectedItem; // Seçili öğeyi al
                        string key = selectedItem.Key; // Anahtar adını al
                        string newValue = txtSettingValue.Text.Trim(); // Yeni değeri al

                        // Yeni değer geçerli mi kontrol et
                        if (string.IsNullOrWhiteSpace(newValue) || newValue == "Anahtar değeri girin...")
                        {
                            MessageBox.Show("Lütfen geçerli bir değer girin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning); // Uyarı mesajı
                            return;
                        }

                        // Anahtar değerini güncelle
                        SqlParameter[] parameters = { new SqlParameter("@SettingKey", key), new SqlParameter("@SettingValue", newValue) };
                        db.ExecuteNonQuery("UPDATE Settings SET SettingValue = @SettingValue WHERE SettingKey = @SettingKey", parameters, false);
                        listBox.Items[listBox.SelectedIndex] = new { Key = key, Value = newValue }; // Liste kutusunu güncelle
                        MessageBox.Show("Anahtar başarıyla güncellendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information); // Başarı mesajı
                        txtSettingKey.Clear(); // Anahtar adı kutusunu temizle
                        txtSettingValue.Clear(); // Anahtar değeri kutusunu temizle
                    }
                    else
                    {
                        MessageBox.Show("Lütfen güncellemek için bir anahtar seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning); // Uyarı mesajı
                    }
                };

                // Liste kutusundaki seçili öğe değiştiğinde
                listBox.SelectedIndexChanged += (s, e) =>
                {
                    if (listBox.SelectedItem != null) // Seçili bir öğe var mı kontrol et
                    {
                        dynamic selectedItem = listBox.SelectedItem; // Seçili öğeyi al
                        txtSettingKey.Text = selectedItem.Key; // Anahtar adını metin kutusuna yaz
                        txtSettingValue.Text = selectedItem.Value; // Anahtar değerini metin kutusuna yaz
                    }
                };

                // Formu göster
                manageSettingsKeysForm.ShowDialog();
            }
        }
    }
}

