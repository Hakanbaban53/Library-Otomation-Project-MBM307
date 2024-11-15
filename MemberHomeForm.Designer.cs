namespace Library_Otomation
{
    partial class MemberHomeForm
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnManageAcoount = new System.Windows.Forms.Button();
            this.btnBookBooking = new System.Windows.Forms.Button();
            this.btnMyFine = new System.Windows.Forms.Button();
            this.btnMyLoans = new System.Windows.Forms.Button();
            this.btnLogOut = new System.Windows.Forms.Button();
            this.btnGetHelp = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(800, 100);
            this.panel2.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 450);
            this.panel1.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.btnManageAcoount, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnBookBooking, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnMyFine, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnMyLoans, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnLogOut, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnGetHelp, 0, 2);
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
            // btnManageAcoount
            // 
            this.btnManageAcoount.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnManageAcoount.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnManageAcoount.Location = new System.Drawing.Point(420, 136);
            this.btnManageAcoount.Margin = new System.Windows.Forms.Padding(20);
            this.btnManageAcoount.Name = "btnManageAcoount";
            this.btnManageAcoount.Size = new System.Drawing.Size(360, 76);
            this.btnManageAcoount.TabIndex = 11;
            this.btnManageAcoount.Text = "Hesap Yönetimi";
            this.btnManageAcoount.UseVisualStyleBackColor = true;
            // 
            // btnBookBooking
            // 
            this.btnBookBooking.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBookBooking.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnBookBooking.Location = new System.Drawing.Point(20, 136);
            this.btnBookBooking.Margin = new System.Windows.Forms.Padding(20);
            this.btnBookBooking.Name = "btnBookBooking";
            this.btnBookBooking.Size = new System.Drawing.Size(360, 76);
            this.btnBookBooking.TabIndex = 10;
            this.btnBookBooking.Text = "Kitap Ayırtma";
            this.btnBookBooking.UseVisualStyleBackColor = true;
            // 
            // btnMyFine
            // 
            this.btnMyFine.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMyFine.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnMyFine.Location = new System.Drawing.Point(420, 20);
            this.btnMyFine.Margin = new System.Windows.Forms.Padding(20);
            this.btnMyFine.Name = "btnMyFine";
            this.btnMyFine.Size = new System.Drawing.Size(360, 76);
            this.btnMyFine.TabIndex = 4;
            this.btnMyFine.Text = "Kitap Geciktirme Borçlarım";
            this.btnMyFine.UseVisualStyleBackColor = true;
            // 
            // btnMyLoans
            // 
            this.btnMyLoans.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMyLoans.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnMyLoans.Location = new System.Drawing.Point(20, 20);
            this.btnMyLoans.Margin = new System.Windows.Forms.Padding(20);
            this.btnMyLoans.Name = "btnMyLoans";
            this.btnMyLoans.Size = new System.Drawing.Size(360, 76);
            this.btnMyLoans.TabIndex = 3;
            this.btnMyLoans.Text = "Ödünç Aldığım Kitaplar";
            this.btnMyLoans.UseVisualStyleBackColor = true;
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
            // btnGetHelp
            // 
            this.btnGetHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGetHelp.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnGetHelp.Location = new System.Drawing.Point(20, 252);
            this.btnGetHelp.Margin = new System.Windows.Forms.Padding(20);
            this.btnGetHelp.Name = "btnGetHelp";
            this.btnGetHelp.Size = new System.Drawing.Size(360, 78);
            this.btnGetHelp.TabIndex = 9;
            this.btnGetHelp.Text = "Destek";
            this.btnGetHelp.UseVisualStyleBackColor = true;
            // 
            // MemberHomeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panel1);
            this.Name = "MemberHomeForm";
            this.Text = "MemberHomeForm";
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnManageAcoount;
        private System.Windows.Forms.Button btnBookBooking;
        private System.Windows.Forms.Button btnMyFine;
        private System.Windows.Forms.Button btnMyLoans;
        private System.Windows.Forms.Button btnLogOut;
        private System.Windows.Forms.Button btnGetHelp;
    }
}