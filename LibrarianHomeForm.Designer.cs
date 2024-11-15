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
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnManageAccount = new System.Windows.Forms.Button();
            this.btnManageFine = new System.Windows.Forms.Button();
            this.btnManageLoans = new System.Windows.Forms.Button();
            this.btnManageBooks = new System.Windows.Forms.Button();
            this.btnLogOut = new System.Windows.Forms.Button();
            this.btnSystemLogs = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 450);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(800, 100);
            this.panel2.TabIndex = 0;
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
            this.tableLayoutPanel1.Size = new System.Drawing.Size(800, 350);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // btnManageAccount
            // 
            this.btnManageAccount.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnManageAccount.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnManageAccount.Location = new System.Drawing.Point(420, 136);
            this.btnManageAccount.Margin = new System.Windows.Forms.Padding(20);
            this.btnManageAccount.Name = "btnManageAccount";
            this.btnManageAccount.Size = new System.Drawing.Size(360, 76);
            this.btnManageAccount.TabIndex = 11;
            this.btnManageAccount.Text = "Hesap Yönetimi";
            this.btnManageAccount.UseVisualStyleBackColor = true;
            // 
            // btnManageFine
            // 
            this.btnManageFine.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnManageFine.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnManageFine.Location = new System.Drawing.Point(20, 136);
            this.btnManageFine.Margin = new System.Windows.Forms.Padding(20);
            this.btnManageFine.Name = "btnManageFine";
            this.btnManageFine.Size = new System.Drawing.Size(360, 76);
            this.btnManageFine.TabIndex = 10;
            this.btnManageFine.Text = "Kitap Gecikme Borçları";
            this.btnManageFine.UseVisualStyleBackColor = true;
            // 
            // btnManageLoans
            // 
            this.btnManageLoans.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnManageLoans.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnManageLoans.Location = new System.Drawing.Point(420, 20);
            this.btnManageLoans.Margin = new System.Windows.Forms.Padding(20);
            this.btnManageLoans.Name = "btnManageLoans";
            this.btnManageLoans.Size = new System.Drawing.Size(360, 76);
            this.btnManageLoans.TabIndex = 4;
            this.btnManageLoans.Text = "Ödünç Alınan Kitapları Yönet";
            this.btnManageLoans.UseVisualStyleBackColor = true;
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
            this.btnManageBooks.Size = new System.Drawing.Size(360, 76);
            this.btnManageBooks.TabIndex = 3;
            this.btnManageBooks.Text = "Kitapları Yönet";
            this.btnManageBooks.UseVisualStyleBackColor = true;
            // 
            // btnLogOut
            // 
            this.btnLogOut.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLogOut.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnLogOut.Location = new System.Drawing.Point(420, 252);
            this.btnLogOut.Margin = new System.Windows.Forms.Padding(20);
            this.btnLogOut.Name = "btnLogOut";
            this.btnLogOut.Size = new System.Drawing.Size(360, 78);
            this.btnLogOut.TabIndex = 8;
            this.btnLogOut.Text = "Çıkış Yap";
            this.btnLogOut.UseVisualStyleBackColor = true;
            // 
            // btnSystemLogs
            // 
            this.btnSystemLogs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSystemLogs.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnSystemLogs.Location = new System.Drawing.Point(20, 252);
            this.btnSystemLogs.Margin = new System.Windows.Forms.Padding(20);
            this.btnSystemLogs.Name = "btnSystemLogs";
            this.btnSystemLogs.Size = new System.Drawing.Size(360, 78);
            this.btnSystemLogs.TabIndex = 9;
            this.btnSystemLogs.Text = "Sistem Kayıtları";
            this.btnSystemLogs.UseVisualStyleBackColor = true;
            // 
            // LibrarianHomeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panel1);
            this.Name = "LibrarianHomeForm";
            this.Text = "LibrarianHomeForm";
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnManageAccount;
        private System.Windows.Forms.Button btnManageFine;
        private System.Windows.Forms.Button btnManageLoans;
        private System.Windows.Forms.Button btnManageBooks;
        private System.Windows.Forms.Button btnLogOut;
        private System.Windows.Forms.Button btnSystemLogs;
        private System.Windows.Forms.Panel panel2;
    }
}