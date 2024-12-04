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
    public partial class ManageFinesForm : Form
    {

        int MemberID = -1;
        public ManageFinesForm()
        {
            InitializeComponent();
            LoadMembersWithLoansAndFines();
            btnPayFine.Enabled = false;

        }
        private void LoadMembersWithLoansAndFines()
        {
            DatabaseHelper db = new DatabaseHelper();
            DataTable memberLoans = db.ExecuteQuery(
                "SELECT m.MemberID, " +
                "m.FirstName + ' ' + m.LastName AS MemberName, " +
                "COUNT(l.LoanID) AS ActiveLoans, " +
                "STRING_AGG(b.Title, ', ') AS LoanedBooks, " +
                "dbo.GetTotalFinesForMember(m.MemberID) AS TotalFines " +
                "FROM Members m " +
                "LEFT JOIN Loans l ON m.MemberID = l.MemberID AND l.ReturnDate IS NULL " +
                "LEFT JOIN Books b ON l.BookID = b.BookID " +
                "GROUP BY m.MemberID, m.FirstName, m.LastName " +
                "HAVING COUNT(l.LoanID) > 0 OR dbo.GetTotalFinesForMember(m.MemberID) > 0",
                null, false);

            dataGridFines.DataSource = memberLoans;
            dataGridFines.Columns["TotalFines"].DefaultCellStyle.Format = "C2"; // Currency format

            // Highlight rows with unpaid fines
            foreach (DataGridViewRow row in dataGridFines.Rows)
            {
                decimal totalFines = Convert.ToDecimal(row.Cells["TotalFines"].Value ?? 0);
                if (totalFines > 0)
                {
                    row.DefaultCellStyle.BackColor = Color.LightCoral;
                }
            }
        }



        private void dataGridFines_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int memberID = Convert.ToInt32(dataGridFines.Rows[e.RowIndex].Cells["MemberID"].Value);
                MemberID = memberID;
                lblTotalFines.Text = $"Total Fines: {dataGridFines.Rows[e.RowIndex].Cells["TotalFines"].FormattedValue}";
                btnPayFine.Enabled = memberID > 0;
            }
        }


        private void btnPayFine_Click(object sender, EventArgs e)
        {
            if (MemberID == -1)
            {
                MessageBox.Show("Please select a user to pay the fine.", "Error");
                return;
            }

            // Check if the user has active loans
            if (HasActiveLoans(MemberID))
            {
                MessageBox.Show("This user has active loans. Fines can only be paid once all loans are returned.", "Error");
                return;
            }

            DatabaseHelper db = new DatabaseHelper();
            SqlParameter[] parameters = { new SqlParameter("@MemberID", MemberID) };

            // Assuming a procedure to mark all fines as paid for a user
            db.ExecuteNonQuery("EXEC PayAllFinesForMember @MemberID", parameters, false);

            MessageBox.Show("All fines for the selected user have been paid!");
            LoadMembersWithLoansAndFines();
            btnPayFine.Enabled = false;
            MemberID = -1;
        }

        private bool HasActiveLoans(int memberID)
        {
            DatabaseHelper db = new DatabaseHelper();
            SqlParameter[] parameters = { new SqlParameter("@MemberID", memberID) };

            // Check if there are any active loans (loans where ReturnDate is NULL)
            DataTable result = db.ExecuteQuery("SELECT COUNT(*) FROM Loans WHERE MemberID = @MemberID AND ReturnDate IS NULL", parameters, false);

            // If there are active loans, return true, else false
            return Convert.ToInt32(result.Rows[0][0]) > 0;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            FormHelper.NavigateBack();
        }
    }
}
