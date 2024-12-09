namespace Library_Otomation
{
    partial class SystemMaintenanceForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.performanceTimer = new System.Windows.Forms.Timer(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.btnBack = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnEditSettingsKey = new System.Windows.Forms.Button();
            this.btnBackupDatabase = new System.Windows.Forms.Button();
            this.btnRestoreDatabase = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.chartCpu = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartMemory = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartStorage = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lblUptime = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartCpu)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartMemory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartStorage)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer1.Size = new System.Drawing.Size(884, 461);
            this.splitContainer1.SplitterDistance = 46;
            this.splitContainer1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.btnBack);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.panel1.MaximumSize = new System.Drawing.Size(0, 42);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(884, 42);
            this.panel1.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.Location = new System.Drawing.Point(81, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(182, 25);
            this.label1.TabIndex = 1;
            this.label1.Text = "Sistem Yönetimi";
            // 
            // btnBack
            // 
            this.btnBack.AutoSize = true;
            this.btnBack.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnBack.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnBack.Location = new System.Drawing.Point(0, 0);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(75, 42);
            this.btnBack.TabIndex = 0;
            this.btnBack.Text = "Geri";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(884, 411);
            this.tabControl1.TabIndex = 3;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.panel2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.tabPage1.Size = new System.Drawing.Size(876, 385);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Sistem Yönetimi";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnEditSettingsKey);
            this.panel2.Controls.Add(this.btnBackupDatabase);
            this.panel2.Controls.Add(this.btnRestoreDatabase);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 0);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(20);
            this.panel2.Size = new System.Drawing.Size(870, 382);
            this.panel2.TabIndex = 8;
            // 
            // btnEditSettingsKey
            // 
            this.btnEditSettingsKey.AutoSize = true;
            this.btnEditSettingsKey.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnEditSettingsKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.btnEditSettingsKey.Location = new System.Drawing.Point(20, 88);
            this.btnEditSettingsKey.Margin = new System.Windows.Forms.Padding(10);
            this.btnEditSettingsKey.Name = "btnEditSettingsKey";
            this.btnEditSettingsKey.Size = new System.Drawing.Size(830, 34);
            this.btnEditSettingsKey.TabIndex = 2;
            this.btnEditSettingsKey.Text = "Ayar Anahtarlarını Düzenle";
            this.btnEditSettingsKey.UseVisualStyleBackColor = true;
            this.btnEditSettingsKey.Click += new System.EventHandler(this.btnEditSettingsKey_Click);
            // 
            // btnBackupDatabase
            // 
            this.btnBackupDatabase.AutoSize = true;
            this.btnBackupDatabase.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnBackupDatabase.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.btnBackupDatabase.Location = new System.Drawing.Point(20, 54);
            this.btnBackupDatabase.Margin = new System.Windows.Forms.Padding(10);
            this.btnBackupDatabase.Name = "btnBackupDatabase";
            this.btnBackupDatabase.Size = new System.Drawing.Size(830, 34);
            this.btnBackupDatabase.TabIndex = 1;
            this.btnBackupDatabase.Text = "Veri Tabanını Yedekle";
            this.btnBackupDatabase.UseVisualStyleBackColor = true;
            this.btnBackupDatabase.Click += new System.EventHandler(this.btnBackupDatabase_Click);
            // 
            // btnRestoreDatabase
            // 
            this.btnRestoreDatabase.AutoSize = true;
            this.btnRestoreDatabase.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnRestoreDatabase.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.btnRestoreDatabase.Location = new System.Drawing.Point(20, 20);
            this.btnRestoreDatabase.Margin = new System.Windows.Forms.Padding(10);
            this.btnRestoreDatabase.Name = "btnRestoreDatabase";
            this.btnRestoreDatabase.Size = new System.Drawing.Size(830, 34);
            this.btnRestoreDatabase.TabIndex = 0;
            this.btnRestoreDatabase.Text = "Veri Tabanını Yedekten Geri Yükle";
            this.btnRestoreDatabase.UseVisualStyleBackColor = true;
            this.btnRestoreDatabase.Click += new System.EventHandler(this.btnRestoreDatabase_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.tableLayoutPanel1);
            this.tabPage2.Controls.Add(this.lblUptime);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(876, 385);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Sistem Performansı";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Controls.Add(this.chartCpu, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.chartMemory, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.chartStorage, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(870, 379);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // chartCpu
            // 
            chartArea1.Name = "ChartArea1";
            this.chartCpu.ChartAreas.Add(chartArea1);
            this.chartCpu.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.chartCpu.Legends.Add(legend1);
            this.chartCpu.Location = new System.Drawing.Point(583, 3);
            this.chartCpu.Name = "chartCpu";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chartCpu.Series.Add(series1);
            this.chartCpu.Size = new System.Drawing.Size(284, 373);
            this.chartCpu.TabIndex = 7;
            this.chartCpu.Text = "chart1";
            // 
            // chartMemory
            // 
            chartArea2.Name = "ChartArea1";
            this.chartMemory.ChartAreas.Add(chartArea2);
            this.chartMemory.Dock = System.Windows.Forms.DockStyle.Fill;
            legend2.Name = "Legend1";
            this.chartMemory.Legends.Add(legend2);
            this.chartMemory.Location = new System.Drawing.Point(3, 3);
            this.chartMemory.Name = "chartMemory";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.chartMemory.Series.Add(series2);
            this.chartMemory.Size = new System.Drawing.Size(284, 373);
            this.chartMemory.TabIndex = 6;
            this.chartMemory.Text = "chart2";
            // 
            // chartStorage
            // 
            chartArea3.Name = "ChartArea1";
            this.chartStorage.ChartAreas.Add(chartArea3);
            this.chartStorage.Dock = System.Windows.Forms.DockStyle.Fill;
            legend3.Name = "Legend1";
            this.chartStorage.Legends.Add(legend3);
            this.chartStorage.Location = new System.Drawing.Point(293, 3);
            this.chartStorage.Name = "chartStorage";
            series3.ChartArea = "ChartArea1";
            series3.Legend = "Legend1";
            series3.Name = "Series1";
            this.chartStorage.Series.Add(series3);
            this.chartStorage.Size = new System.Drawing.Size(284, 373);
            this.chartStorage.TabIndex = 5;
            this.chartStorage.Text = "chart1";
            // 
            // lblUptime
            // 
            this.lblUptime.AutoSize = true;
            this.lblUptime.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblUptime.Location = new System.Drawing.Point(657, 333);
            this.lblUptime.Name = "lblUptime";
            this.lblUptime.Size = new System.Drawing.Size(0, 20);
            this.lblUptime.TabIndex = 1;
            // 
            // SystemMaintenanceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 461);
            this.Controls.Add(this.splitContainer1);
            this.MinimumSize = new System.Drawing.Size(900, 500);
            this.Name = "SystemMaintenanceForm";
            this.Text = "Sistem Yönetimi";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartCpu)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartMemory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartStorage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer performanceTimer;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button btnBackupDatabase;
        private System.Windows.Forms.Button btnRestoreDatabase;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label lblUptime;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnEditSettingsKey;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartCpu;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartMemory;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartStorage;
    }
}