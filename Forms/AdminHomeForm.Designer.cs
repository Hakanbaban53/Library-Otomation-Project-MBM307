namespace Library_Otomation
{
    partial class AdminHomeForm
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
            this.btnManageSystem = new System.Windows.Forms.Button();
            this.btnManageFine = new System.Windows.Forms.Button();
            this.btnManageLoans = new System.Windows.Forms.Button();
            this.btnManageBooks = new System.Windows.Forms.Button();
            this.btnManageMembers = new System.Windows.Forms.Button();
            this.btnManageUsers = new System.Windows.Forms.Button();
            this.btnLogOut = new System.Windows.Forms.Button();
            this.btnReportsandSystenLogs = new System.Windows.Forms.Button();
            this.mainPanel.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainPanel
            // 
            this.mainPanel.Controls.Add(this.tableLayoutPanel1);
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
            this.tableLayoutPanel1.Controls.Add(this.btnManageSystem, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnManageFine, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnManageLoans, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnManageBooks, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnManageMembers, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnManageUsers, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnLogOut, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnReportsandSystenLogs, 0, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(884, 461);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // btnManageSystem
            // 
            this.btnManageSystem.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnManageSystem.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnManageSystem.Location = new System.Drawing.Point(462, 250);
            this.btnManageSystem.Margin = new System.Windows.Forms.Padding(20);
            this.btnManageSystem.Name = "btnManageSystem";
            this.btnManageSystem.Size = new System.Drawing.Size(402, 75);
            this.btnManageSystem.TabIndex = 11;
            this.btnManageSystem.Text = "Sistem Yönetimi";
            this.btnManageSystem.UseVisualStyleBackColor = true;
            this.btnManageSystem.Click += new System.EventHandler(this.btnManageSystem_Click);
            // 
            // btnManageFine
            // 
            this.btnManageFine.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnManageFine.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnManageFine.Location = new System.Drawing.Point(20, 250);
            this.btnManageFine.Margin = new System.Windows.Forms.Padding(20);
            this.btnManageFine.Name = "btnManageFine";
            this.btnManageFine.Size = new System.Drawing.Size(402, 75);
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
            this.btnManageLoans.Location = new System.Drawing.Point(462, 135);
            this.btnManageLoans.Margin = new System.Windows.Forms.Padding(20);
            this.btnManageLoans.Name = "btnManageLoans";
            this.btnManageLoans.Size = new System.Drawing.Size(402, 75);
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
            this.btnManageBooks.Location = new System.Drawing.Point(20, 135);
            this.btnManageBooks.Margin = new System.Windows.Forms.Padding(20);
            this.btnManageBooks.Name = "btnManageBooks";
            this.btnManageBooks.Size = new System.Drawing.Size(402, 75);
            this.btnManageBooks.TabIndex = 3;
            this.btnManageBooks.Text = "Kitapları Yönet";
            this.btnManageBooks.UseVisualStyleBackColor = true;
            this.btnManageBooks.Click += new System.EventHandler(this.btnManageBooks_Click);
            // 
            // btnManageMembers
            // 
            this.btnManageMembers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnManageMembers.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnManageMembers.Location = new System.Drawing.Point(462, 20);
            this.btnManageMembers.Margin = new System.Windows.Forms.Padding(20);
            this.btnManageMembers.Name = "btnManageMembers";
            this.btnManageMembers.Size = new System.Drawing.Size(402, 75);
            this.btnManageMembers.TabIndex = 1;
            this.btnManageMembers.Text = "Üyeleri Yönet";
            this.btnManageMembers.UseVisualStyleBackColor = true;
            this.btnManageMembers.Click += new System.EventHandler(this.btnManageMembers_Click);
            // 
            // btnManageUsers
            // 
            this.btnManageUsers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnManageUsers.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnManageUsers.Location = new System.Drawing.Point(20, 20);
            this.btnManageUsers.Margin = new System.Windows.Forms.Padding(20);
            this.btnManageUsers.Name = "btnManageUsers";
            this.btnManageUsers.Size = new System.Drawing.Size(402, 75);
            this.btnManageUsers.TabIndex = 0;
            this.btnManageUsers.Text = "Kullanıcıları Yönet";
            this.btnManageUsers.UseVisualStyleBackColor = true;
            this.btnManageUsers.Click += new System.EventHandler(this.btnManageUsers_Click);
            // 
            // btnLogOut
            // 
            this.btnLogOut.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLogOut.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnLogOut.Location = new System.Drawing.Point(462, 365);
            this.btnLogOut.Margin = new System.Windows.Forms.Padding(20);
            this.btnLogOut.Name = "btnLogOut";
            this.btnLogOut.Size = new System.Drawing.Size(402, 76);
            this.btnLogOut.TabIndex = 8;
            this.btnLogOut.Text = "Çıkış Yap";
            this.btnLogOut.UseVisualStyleBackColor = true;
            this.btnLogOut.Click += new System.EventHandler(this.btnLogOut_Click);
            // 
            // btnReportsandSystenLogs
            // 
            this.btnReportsandSystenLogs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReportsandSystenLogs.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnReportsandSystenLogs.Location = new System.Drawing.Point(20, 365);
            this.btnReportsandSystenLogs.Margin = new System.Windows.Forms.Padding(20);
            this.btnReportsandSystenLogs.Name = "btnReportsandSystenLogs";
            this.btnReportsandSystenLogs.Size = new System.Drawing.Size(402, 76);
            this.btnReportsandSystenLogs.TabIndex = 9;
            this.btnReportsandSystenLogs.Text = "Raporlar ve Sistem Kayıtları";
            this.btnReportsandSystenLogs.UseVisualStyleBackColor = true;
            this.btnReportsandSystenLogs.Click += new System.EventHandler(this.btnReportsandSystenLogs_Click);
            // 
            // AdminHomeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 461);
            this.Controls.Add(this.mainPanel);
            this.MinimumSize = new System.Drawing.Size(900, 500);
            this.Name = "AdminHomeForm";
            this.Text = "Yönetici Paneli Ana Sayfa";
            this.mainPanel.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnManageSystem;
        private System.Windows.Forms.Button btnManageFine;
        private System.Windows.Forms.Button btnManageLoans;
        private System.Windows.Forms.Button btnManageBooks;
        private System.Windows.Forms.Button btnManageMembers;
        private System.Windows.Forms.Button btnManageUsers;
        private System.Windows.Forms.Button btnLogOut;
        private System.Windows.Forms.Button btnReportsandSystenLogs;
    }
}