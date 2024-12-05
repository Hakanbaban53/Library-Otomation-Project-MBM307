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
        // CPU ve bellek performansını izlemek için sayaçlar
        private PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        private PerformanceCounter memCounter = new PerformanceCounter("Memory", "Available MBytes");

        // Formun yapıcı metodu
        public SystemMaintenanceForm()
        {
            InitializeComponent(); // Bileşenleri başlat
            chartPerformance.Series.Clear(); // Mevcut serileri temizle
            chartPerformance.Series.Add("CPU Usage"); // CPU kullanımı serisini ekle
            chartPerformance.Series["CPU Usage"].ChartType = SeriesChartType.Line; // Çizgi grafik türü
            chartPerformance.Series["CPU Usage"].BorderWidth = 3; // Çizgi kalınlığını artır

            chartPerformance.Series.Add("Available Memory"); // Kullanılabilir bellek serisini ekle
            chartPerformance.Series["Available Memory"].ChartType = SeriesChartType.Line; // Çizgi grafik türü
            chartPerformance.Series["Available Memory"].BorderWidth = 3; // Çizgi kalınlığını artır

            performanceTimer.Interval = 1000; // 1 saniye aralık
            performanceTimer.Tick += PerformanceTimer_Tick; // Zamanlayıcı tıklama olayını bağla
            performanceTimer.Start(); // Zamanlayıcıyı başlat
        }

        private bool isMaintenanceMode = false; // Bakım modunun aktif olup olmadığını takip eder

        // Veritabanı yedekleme butonuna tıklama olayı
        private void btnBackupDatabase_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog()) // Yedekleme dosyası için kaydetme diyaloğu
            {
                saveFileDialog.Filter = "Backup Files (*.bak)|*.bak"; // Dosya filtresi
                saveFileDialog.Title = "Select Backup File Location"; // Diyalog başlığı
                saveFileDialog.FileName = "LibraryBackup.bak"; // Varsayılan dosya adı

                if (saveFileDialog.ShowDialog() == DialogResult.OK) // Diyalog onaylandıysa
                {
                    try
                    {
                        string backupPath = saveFileDialog.FileName; // Seçilen dosya yolu
                        DatabaseHelper db = new DatabaseHelper(); // Veritabanı yardımcı sınıfı
                        db.ExecuteNonQuery($"BACKUP DATABASE Library_automation TO DISK = '{backupPath}'", null, false); // Veritabanı yedekleme komutu
                        MessageBox.Show("Database backup completed successfully!"); // Başarılı mesajı
                    }
                    catch (Exception ex) // Hata durumunda
                    {
                        MessageBox.Show("Backup failed: " + ex.Message); // Hata mesajı
                    }
                }
            }
        }

        // Bakım modunu değiştirme butonuna tıklama olayı
        private void btnToggleMaintenance_Click(object sender, EventArgs e)
        {
            if (!isMaintenanceMode) // Eğer bakım modunda değilse
            {
                // Bakım Moduna Geç
                EnterMaintenanceMode(); // Bakım moduna geçiş metodu
                btnToggleMaintenance.Text = "Exit Maintenance Mode"; // Buton metnini değiştir
                isMaintenanceMode = true; // Bakım modunu aktif et
            }
            else // Eğer bakım modundaysa
            {
                // Bakım Modundan Çık
                ExitMaintenanceMode(); // Bakım modundan çıkış metodu
                btnToggleMaintenance.Text = "Enter Maintenance Mode"; // Buton metnini geri değiştir
                isMaintenanceMode = false; // Bakım modunu pasif et
            }
        }

        // Bakım moduna geçiş metodu
        private void EnterMaintenanceMode()
        {
            MessageBox.Show("System is now in Maintenance Mode. Some features may be disabled."); // Bakım moduna geçiş mesajı
                                                                                                  // Opsiyonel: Belirli butonları veya özellikleri devre dışı bırak
            btnBackupDatabase.Enabled = true; // Yedekleme butonunu etkinleştir
            btnRestoreDatabase.Enabled = true; // Geri yükleme butonunu etkinleştir
            btnCleanupOldData.Enabled = true; // Eski verileri temizleme butonunu etkinleştir
        }

        // Bakım modundan çıkış metodu
        private void ExitMaintenanceMode()
        {
            MessageBox.Show("Exited Maintenance Mode. All features are now available."); // Bakım modundan çıkış mesajı
                                                                                         // Opsiyonel: Tüm butonları veya özellikleri tekrar etkinleştir
            btnBackupDatabase.Enabled = false; // Yedekleme butonunu devre dışı bırak
            btnRestoreDatabase.Enabled = false; // Geri yükleme butonunu devre dışı bırak
            btnCleanupOldData.Enabled = false; // Eski verileri temizleme butonunu devre dışı bırak
        }

        // Veritabanı geri yükleme butonuna tıklama olayı
        private void btnRestoreDatabase_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog()) // Geri yükleme dosyası için açma diyaloğu
            {
                openFileDialog.Filter = "Backup Files (*.bak)|*.bak"; // Dosya filtresi
                openFileDialog.Title = "Select Backup File"; // Diyalog başlığı

                if (openFileDialog.ShowDialog() == DialogResult.OK) // Diyalog onaylandıysa
                {
                    try
                    {
                        string backupPath = openFileDialog.FileName; // Seçilen dosya yolu

                        // Master veritabanına geçiş yap ve geri yükle
                        DatabaseHelper db = new DatabaseHelper(); // Veritabanı yardımcı sınıfı
                        db.ExecuteNonQuery("USE master", null, false); // Master veritabanına geçiş
                        db.ExecuteNonQuery($"ALTER DATABASE Library_automation SET SINGLE_USER WITH ROLLBACK IMMEDIATE", null, false); // Tek kullanıcı moduna geçiş
                        db.ExecuteNonQuery($"RESTORE DATABASE Library_automation FROM DISK = '{backupPath}' WITH REPLACE", null, false); // Veritabanı geri yükleme komutu
                        db.ExecuteNonQuery($"ALTER DATABASE Library_automation SET MULTI_USER", null, false); // Çoklu kullanıcı moduna geçiş

                        MessageBox.Show("Database restore completed successfully!"); // Başarılı mesajı
                    }
                    catch (Exception ex) // Hata durumunda
                    {
                        MessageBox.Show("Restore failed: " + ex.Message); // Hata mesajı
                    }
                }
            }
        }

        // Performans zamanlayıcısının tıklama olayı
        private void PerformanceTimer_Tick(object sender, EventArgs e)
        {
            float cpuUsage = cpuCounter.NextValue(); // CPU kullanımını al
            float availableMemory = memCounter.NextValue(); // Kullanılabilir belleği al

            chartPerformance.Series["CPU Usage"].Points.AddY(cpuUsage); // CPU kullanımını grafiğe ekle
            chartPerformance.Series["Available Memory"].Points.AddY(availableMemory); // Kullanılabilir belleği grafiğe ekle

            // Grafik boyutunu yönetilebilir tutmak için eski noktaları kaldır
            if (chartPerformance.Series["CPU Usage"].Points.Count > 60) // Eğer 60'dan fazla nokta varsa
            {
                chartPerformance.Series["CPU Usage"].Points.RemoveAt(0); // İlk noktayı kaldır
                chartPerformance.Series["Available Memory"].Points.RemoveAt(0); // İlk noktayı kaldır
            }

            chartPerformance.ResetAutoValues(); // Grafik otomatik değerlerini sıfırla

            // Uptime etiketini güncelle
            TimeSpan uptime = TimeSpan.FromMilliseconds(Environment.TickCount); // Uptime'ı al
            lblUptime.Text = $"Uptime: {uptime:dd\\.hh\\:mm\\:ss}"; // Uptime etiketini güncelle
        }

        // Geri butonuna tıklama olayı
        private void btnBack_Click(object sender, EventArgs e)
        {
            FormHelper.NavigateBack(); // Geri gitme işlemi
        }
    }
}
