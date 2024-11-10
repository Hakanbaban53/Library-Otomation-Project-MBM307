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
    public partial class RegisterMemberForm : Form
    {
        public RegisterMemberForm()
        {
            InitializeComponent();
        }

        private void btnRegisterMember_Click(object sender, EventArgs e)
        {
            // Collect data from text boxes
            string firstName = txtFirstName.Text;
            string lastName = txtLastName.Text;
            DateTime dateOfBirth = DateTime.Parse(txtDateOfBirth.Text);
            string address = txtAddress.Text;
            string phone = txtPhone.Text;
            string email = txtEmail.Text;

            // Add member to database
            DatabaseHelper db = new DatabaseHelper();
            SqlParameter[] parameters = new SqlParameter[]
            {
           new SqlParameter("@FirstName", firstName),
           new SqlParameter("@LastName", lastName),
           new SqlParameter("@DateOfBirth", dateOfBirth),
           new SqlParameter("@Address", address),
           new SqlParameter("@Phone", phone),
           new SqlParameter("@Email", email)
            };

            db.ExecuteNonQuery("RegisterNewMember", parameters);
            MessageBox.Show("Member registered successfully!");
        }

        private void txtFirstName_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
