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

namespace Library_Otomation
{
    public partial class LoanManagementForm : Form
    {
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
            DataTable books = db.ExecuteQuery("SELECT BookID, Title, Author FROM Books WHERE BookID NOT IN (SELECT BookID FROM Loans WHERE ReturnDate IS NULL)", null, false);
            dataGridBooks.DataSource = books;
        }

        private void LoadLoans()
        {
            DatabaseHelper db = new DatabaseHelper();
            DataTable loans = db.ExecuteQuery("SELECT l.LoanID, l.BookID, b.Title, l.MemberID, m.FirstName + ' ' + m.LastName AS MemberName, l.LoanDate, l.DueDate FROM Loans l JOIN Books b ON l.BookID = b.BookID JOIN Members m ON l.MemberID = m.MemberID WHERE l.ReturnDate IS NULL", null, false);
            dataGridLoans.DataSource = loans;
        }




        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridUsers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtMemberID.Text = dataGridUsers.Rows[e.RowIndex].Cells["MemberID"].Value.ToString();
            }
        }

        private void dataGridBooks_CellContentClick(object sender, DataGridViewCellEventArgs e)
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

                DatabaseHelper db = new DatabaseHelper();
                SqlParameter[] parameters = {
                    new SqlParameter("@BookID", bookID),
                    new SqlParameter("@MemberID", memberID),
                    new SqlParameter("@LoanDate", loanDate),
                    new SqlParameter("@DueDate", dueDate)
                };

                db.ExecuteNonQuery("LoanBook", parameters);
                MessageBox.Show("Loan issued successfully!", "Success");
                txtBookID.Clear();
                txtMemberID.Clear();
                refreshTables();
            }
            catch (FormatException)
            {
                MessageBox.Show("Please enter valid numeric values for Book ID and Member ID.", "Format Error");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error");
            }
        }

        private void dataGridLoans_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
                if (e.RowIndex >= 0)
                {
                    txtLoanID.Text = dataGridLoans.Rows[e.RowIndex].Cells["BookID"].Value.ToString();
                }
            
        }
        private void btnReturnBook_Click(object sender, EventArgs e)
        {
            try
            {
                    int loanID = int.Parse(txtLoanID.Text);
                    DateTime returnDate = dtpReturnDate.Value;

                    DatabaseHelper db = new DatabaseHelper();
                    SqlParameter[] parameters = {
                        new SqlParameter("@LoanID", loanID),
                        new SqlParameter("@ReturnDate", returnDate)
                    };

                    db.ExecuteNonQuery("ReturnBook", parameters);
                    MessageBox.Show("Book returned successfully!");
                    refreshTables();
            }
            catch (FormatException)
            {
                MessageBox.Show("Invalid data format. Please check the selected loan.", "Format Error");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while returning the book: {ex.Message}", "Error");
            }
        }
    }
}
