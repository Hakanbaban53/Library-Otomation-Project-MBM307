﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Library_Otomation
{
    public partial class LoanManagementForm : Form
    { 
        int LoanID = -1;
        public LoanManagementForm()
        {
            InitializeComponent();
            refreshTables();
        }
        private void refreshTables()
        {
            LoadBooks();
            LoadLoans();
            LoadUsers();
        }

        private void LoadUsers()
        {
            DatabaseHelper db = new DatabaseHelper();
            DataTable users = db.ExecuteQuery("SELECT MemberID, FirstName, LastName FROM Members", null, false);
            dataGridUsers.DataSource = users;
        }

        private void LoadBooks()
        {
            DatabaseHelper db = new DatabaseHelper();
            DataTable books = db.ExecuteQuery("SELECT * FROM ViewAvailableBooks", null, false);
            dataGridBooks.DataSource = books;
        }

        private void LoadLoans()
        {
            DatabaseHelper db = new DatabaseHelper();
            DataTable loans = db.ExecuteQuery("SELECT l.LoanID, l.BookID, b.Title, l.MemberID, m.FirstName + ' ' + m.LastName AS MemberName, l.LoanDate, l.DueDate FROM Loans l JOIN Books b ON l.BookID = b.BookID JOIN Members m ON l.MemberID = m.MemberID WHERE l.ReturnDate IS NULL", null, false);
            dataGridLoans.DataSource = loans;
        }

        private bool IsBookAvailable(int bookID)
        {
            DatabaseHelper db = new DatabaseHelper();
            SqlParameter[] parameters = { new SqlParameter("@BookID", bookID) };
            DataTable result = db.ExecuteQuery("SELECT dbo.IsBookAvailable(@BookID) AS IsAvailable", parameters, false);
            return Convert.ToBoolean(result.Rows[0]["IsAvailable"]);
        }

        private bool IsLoanValid(int loanID)
        {
            foreach (DataGridViewRow row in dataGridLoans.Rows)
            {
                if ((int)row.Cells["LoanID"].Value == loanID)
                {
                    return true;
                }
            }
            return false;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            FormHelper.NavigateBack();
        }

        private void dataGridUsers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtMemberID.Text = dataGridUsers.Rows[e.RowIndex].Cells["MemberID"].Value.ToString();
            }
        }

        private void dataGridBooks_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtBookID.Text = dataGridBooks.Rows[e.RowIndex].Cells["BookID"].Value.ToString();
            }
        }

        private void btnRefreshTab1_Click(object sender, EventArgs e)
        {
            refreshTables();
        }

        private void btnIssueLoan_Click(object sender, EventArgs e)
        {
            try
            {
                int bookID = int.Parse(txtBookID.Text);
                int memberID = int.Parse(txtMemberID.Text);
                DateTime loanDate = dtpLoanDate.Value;
                DateTime dueDate = dtpDueDate.Value;

                if (!IsBookAvailable(bookID))
                {
                    MessageBox.Show("Seçilen kitap ödünç vermek için mevcut değil.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DatabaseHelper db = new DatabaseHelper();
                SqlParameter[] parameters = {
                    new SqlParameter("@BookID", bookID),
                    new SqlParameter("@MemberID", memberID),
                    new SqlParameter("@LoanDate", loanDate),
                    new SqlParameter("@DueDate", dueDate)
                };

                db.ExecuteNonQuery("LoanBook", parameters);
                MessageBox.Show("Kitap başarılı bir şeklide ödünç verildi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtBookID.Clear();
                txtMemberID.Clear();
                refreshTables();
            }
            catch (FormatException)
            {
                MessageBox.Show("Lütfen geçerli bir Book ID ve Member ID giriniz", "Format Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void dataGridLoans_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                LoanID = (int)dataGridLoans.Rows[e.RowIndex].Cells["LoanID"].Value;
                txtLoanID.Text = LoanID.ToString();
            }
            else
            {
                LoanID = -1;
                txtLoanID.Clear();
            }

        }
        private void btnReturnBook_Click(object sender, EventArgs e)
        {
            try
            {
                if (LoanID == -1 || !IsLoanValid(LoanID))
                {
                    throw new Exception("Lütfen geçerli bir ödünç alan seçin.");
                }

                DateTime returnDate = dtpReturnDate.Value;

                DatabaseHelper db = new DatabaseHelper();
                SqlParameter[] parameters = {
            new SqlParameter("@LoanID", LoanID),
            new SqlParameter("@ReturnDate", returnDate)
        };

                db.ExecuteNonQuery("ReturnBook", parameters);
                MessageBox.Show("Kitap başarılı bir şekilde iade edildi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                refreshTables();
            }
            catch (FormatException)
            {
                MessageBox.Show("Geçersiz veri formatı. Lütfen ödünç alanı kontrol edin.", "Format Hatası");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata");
            }
        }
    }
}