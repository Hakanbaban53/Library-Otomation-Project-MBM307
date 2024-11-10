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
            LoadLoanedBooks();
            LoadOverdueBooks();
        }


        private void btnLoanBook_Click(object sender, EventArgs e)
        {
            int bookID = int.Parse(txtBookID.Text);
            int memberID = int.Parse(txtMemberID.Text);
            DateTime loanDate = DateTime.Parse(txtLoanDate.Text);
            DateTime dueDate = DateTime.Parse(txtDueDate.Text);

            DatabaseHelper db = new DatabaseHelper();
            SqlParameter[] parameters = new SqlParameter[]
            {
           new SqlParameter("@BookID", bookID),
           new SqlParameter("@MemberID", memberID),
           new SqlParameter("@LoanDate", loanDate),
           new SqlParameter("@DueDate", dueDate)
            };

            db.ExecuteNonQuery("LoanBook", parameters);
            MessageBox.Show("Book loaned successfully!");
        }

        private void btnReturnBook_Click(object sender, EventArgs e)
        {
            int loanID = int.Parse(txtLoanID.Text); // You may need an extra field to specify LoanID
            DateTime returnDate = DateTime.Now;

            DatabaseHelper db = new DatabaseHelper();
            SqlParameter[] parameters = new SqlParameter[]
            {
           new SqlParameter("@LoanID", loanID),
           new SqlParameter("@ReturnDate", returnDate)
            };

            db.ExecuteNonQuery("ReturnBook", parameters);
            MessageBox.Show("Book returned successfully!");
        }

        private void LoadLoanedBooks()
        {
            DatabaseHelper db = new DatabaseHelper();
            DataTable loanedBooks = db.ExecuteQuery("SELECT * FROM ViewActiveLoans", null, false);

            dataGridLoanedBooks.DataSource = loanedBooks;

        }

        private void LoadOverdueBooks()
        {
            DatabaseHelper db = new DatabaseHelper();
            DataTable overdueBooks = db.ExecuteQuery("SELECT * FROM ViewOverdueLoans", null, false);
            dataGridOverdueBooks.DataSource = overdueBooks;
        }

        private void dataGridLoanedBooks_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
