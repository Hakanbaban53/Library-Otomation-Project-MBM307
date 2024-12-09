namespace Library_Otomation
{
    partial class LibrarianHomeForm
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
            this.mainPanel = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnManageAccount = new System.Windows.Forms.Button();
            this.btnManageFine = new System.Windows.Forms.Button();
            this.btnManageLoans = new System.Windows.Forms.Button();
            this.btnManageBooks = new System.Windows.Forms.Button();
            this.btnLogOut = new System.Windows.Forms.Button();
            this.btnSystemLogs = new System.Windows.Forms.Button();
            this.greetingPanel = new System.Windows.Forms.Panel();
            this.lblGreeting = new System.Windows.Forms.Label();
            this.mainPanel.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.greetingPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainPanel
            // 
            this.mainPanel.Controls.Add(this.tableLayoutPanel1);
            this.mainPanel.Controls.Add(this.greetingPanel);
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point(0, 0);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(884, 461);
            this.mainPanel.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.btnManageAccount, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnManageFine, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnManageLoans, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnManageBooks, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnLogOut, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnSystemLogs, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 100);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(884, 361);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // btnManageAccount
            // 
            this.btnManageAccount.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnManageAccount.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnManageAccount.Location = new System.Drawing.Point(462, 140);
            this.btnManageAccount.Margin = new System.Windows.Forms.Padding(20);
            this.btnManageAccount.Name = "btnManageAccount";
            this.btnManageAccount.Size = new System.Drawing.Size(402, 80);
            this.btnManageAccount.TabIndex = 11;
            this.btnManageAccount.Text = "Hesap Yönetimi";
            this.btnManageAccount.UseVisualStyleBackColor = true;
            this.btnManageAccount.Click += new System.EventHandler(this.btnManageAccount_Click);
            // 
            // btnManageFine
            // 
            this.btnManageFine.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnManageFine.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnManageFine.Location = new System.Drawing.Point(20, 140);
            this.btnManageFine.Margin = new System.Windows.Forms.Padding(20);
            this.btnManageFine.Name = "btnManageFine";
            this.btnManageFine.Size = new System.Drawing.Size(402, 80);
            this.btnManageFine.TabIndex = 10;
            this.btnManageFine.Text = "Kitap Gecikme Borçları";
            this.btnManageFine.UseVisualStyleBackColor = true;
            this.btnManageFine.Click += new System.EventHandler(this.btnManageFine_Click);
            // 
            // btnManageLoans
            // 
            this.btnManageLoans.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnManageLoans.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnManageLoans.Location = new System.Drawing.Point(462, 20);
            this.btnManageLoans.Margin = new System.Windows.Forms.Padding(20);
            this.btnManageLoans.Name = "btnManageLoans";
            this.btnManageLoans.Size = new System.Drawing.Size(402, 80);
            this.btnManageLoans.TabIndex = 4;
            this.btnManageLoans.Text = "Ödünç Alınan Kitapları Yönet";
            this.btnManageLoans.UseVisualStyleBackColor = true;
            this.btnManageLoans.Click += new System.EventHandler(this.btnManageLoans_Click);
            // 
            // btnManageBooks
            // 
            this.btnManageBooks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnManageBooks.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnManageBooks.Location = new System.Drawing.Point(20, 20);
            this.btnManageBooks.Margin = new System.Windows.Forms.Padding(20);
            this.btnManageBooks.Name = "btnManageBooks";
            this.btnManageBooks.Size = new System.Drawing.Size(402, 80);
            this.btnManageBooks.TabIndex = 3;
            this.btnManageBooks.Text = "Kitapları Yönet";
            this.btnManageBooks.UseVisualStyleBackColor = true;
            this.btnManageBooks.Click += new System.EventHandler(this.btnManageBooks_Click);
            // 
            // btnLogOut
            // 
            this.btnLogOut.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLogOut.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnLogOut.Location = new System.Drawing.Point(462, 260);
            this.btnLogOut.Margin = new System.Windows.Forms.Padding(20);
            this.btnLogOut.Name = "btnLogOut";
            this.btnLogOut.Size = new System.Drawing.Size(402, 81);
            this.btnLogOut.TabIndex = 8;
            this.btnLogOut.Text = "Çıkış Yap";
            this.btnLogOut.UseVisualStyleBackColor = true;
            this.btnLogOut.Click += new System.EventHandler(this.btnLogOut_Click);
            // 
            // btnSystemLogs
            // 
            this.btnSystemLogs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSystemLogs.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnSystemLogs.Location = new System.Drawing.Point(20, 260);
            this.btnSystemLogs.Margin = new System.Windows.Forms.Padding(20);
            this.btnSystemLogs.Name = "btnSystemLogs";
            this.btnSystemLogs.Size = new System.Drawing.Size(402, 81);
            this.btnSystemLogs.TabIndex = 9;
            this.btnSystemLogs.Text = "Sistem Kayıtları";
            this.btnSystemLogs.UseVisualStyleBackColor = true;
            this.btnSystemLogs.Click += new System.EventHandler(this.btnSystemLogs_Click);
            // 
            // greetingPanel
            // 
            this.greetingPanel.Controls.Add(this.lblGreeting);
            this.greetingPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.greetingPanel.Location = new System.Drawing.Point(0, 0);
            this.greetingPanel.Name = "greetingPanel";
            this.greetingPanel.Size = new System.Drawing.Size(884, 100);
            this.greetingPanel.TabIndex = 0;
            // 
            // lblGreeting
            // 
            this.lblGreeting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblGreeting.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblGreeting.Location = new System.Drawing.Point(0, 0);
            this.lblGreeting.Name = "lblGreeting";
            this.lblGreeting.Size = new System.Drawing.Size(884, 100);
            this.lblGreeting.TabIndex = 0;
            this.lblGreeting.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LibrarianHomeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 461);
            this.Controls.Add(this.mainPanel);
            this.Name = "LibrarianHomeForm";
            this.Text = "Kütüphane Görevlisi Ana Sayfa";
            this.mainPanel.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.greetingPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnManageAccount;
        private System.Windows.Forms.Button btnManageFine;
        private System.Windows.Forms.Button btnManageLoans;
        private System.Windows.Forms.Button btnManageBooks;
        private System.Windows.Forms.Button btnLogOut;
        private System.Windows.Forms.Button btnSystemLogs;
        private System.Windows.Forms.Panel greetingPanel;
        private System.Windows.Forms.Label lblGreeting;
    }
}