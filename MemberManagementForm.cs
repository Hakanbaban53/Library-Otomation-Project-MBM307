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
    public partial class MemberManagementForm : Form
    {
        int selectedMemberID = -1;
        public MemberManagementForm()
        {
            InitializeComponent();
            LoadMembers();
            UpdateUIState();
        }

        private void clearFields()
        {
            txtFirstName.Text = "";
            txtLastName.Text = "";
            dateBirthTime.Value = DateTime.Now;
            txtAddress.Text = "";
            txtPhone.Text = "";
            txtEmail.Text = "";
        }

        private void LoadMembers()
        {
            // Load members from database
            DatabaseHelper db = new DatabaseHelper();
            DataTable dt = db.ExecuteQuery("SELECT * FROM Members", null, false);
            dataGridMembers.DataSource = dt;
            selectedMemberID = -1;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void checkFields()
        {
            if (string.IsNullOrWhiteSpace(txtFirstName.Text) || string.IsNullOrWhiteSpace(txtLastName.Text) ||
                string.IsNullOrWhiteSpace(txtAddress.Text) || string.IsNullOrWhiteSpace(txtPhone.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                throw new Exception("Tüm alanlar doldurulmalıdır.");
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(txtPhone.Text, @"^\d{10}$"))
            {
                throw new Exception("Geçerli bir telefon numararı giriniz (10 Haneli telefon numaranızı başında '0' olmadan girin.).");
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(txtEmail.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                throw new Exception("Geçerli bir e-posta adresi giriniz.");
            }
        }

        private void dataGridMembers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedMemberID = (int)dataGridMembers.Rows[e.RowIndex].Cells[0].Value;
            txtFirstName.Text = dataGridMembers.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtLastName.Text = dataGridMembers.Rows[e.RowIndex].Cells[2].Value.ToString();
            dateBirthTime.Value = (DateTime)dataGridMembers.Rows[e.RowIndex].Cells[3].Value;
            txtAddress.Text = dataGridMembers.Rows[e.RowIndex].Cells[4].Value.ToString();
            txtPhone.Text = dataGridMembers.Rows[e.RowIndex].Cells[5].Value.ToString();
            txtEmail.Text = dataGridMembers.Rows[e.RowIndex].Cells[6].Value.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                checkFields();
                String firstName = txtFirstName.Text;
                String lastName = txtLastName.Text;
                DateTime dateOfBirth = dateBirthTime.Value;
                String address = txtAddress.Text;
                String phone = txtPhone.Text;
                String email = txtEmail.Text;

                DatabaseHelper db = new DatabaseHelper();
                SqlParameter[] parameters;

                if (chkNewMember.Checked)
                {
                    parameters = new SqlParameter[]
                    {
                            new SqlParameter("@FirstName", firstName),
                            new SqlParameter("@LastName", lastName),
                            new SqlParameter("@DateOfBirth", dateOfBirth),
                            new SqlParameter("@Address", address),
                            new SqlParameter("@Phone", phone),
                            new SqlParameter("@Email", email)
                    };
                    db.ExecuteNonQuery("RegisterNewMember", parameters);
                }
                else if (chkUpdateMember.Checked)
                {
                    int memberId = selectedMemberID;
                    parameters = new SqlParameter[]
                    {
                            new SqlParameter("@MemberID", memberId),
                            new SqlParameter("@FirstName", firstName),
                            new SqlParameter("@LastName", lastName),
                            new SqlParameter("@DateOfBirth", dateOfBirth),
                            new SqlParameter("@Address", address),
                            new SqlParameter("@Phone", phone),
                            new SqlParameter("@Email", email)
                    };
                    db.ExecuteNonQuery("UpdateMember", parameters);
                }

                LoadMembers();
                clearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDeleteUser_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedMemberID == 0)
                {
                    throw new Exception("Lütfen silmek istediğiniz üyeyi seçin.");
                }

                int memberId = selectedMemberID;
                string memberName = txtFirstName.Text + " " +
                                    txtLastName.Text;

                DialogResult dialogResult = MessageBox.Show($"{memberName} adlı üyeyi gerçekten silmek istiyor musunuz?", "Üye Silme", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    DatabaseHelper db = new DatabaseHelper();
                    SqlParameter[] parameters = new SqlParameter[]
                    {
                            new SqlParameter("@MemberID", memberId)
                    };

                    db.ExecuteNonQuery("DeleteMember", parameters);
                    LoadMembers();
                    clearFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void chkNewMember_CheckedChanged(object sender, EventArgs e)
        {
            if (chkNewMember.Checked)
            {
                chkUpdateMember.Checked = false;
                dataGridMembers.ClearSelection();
                selectedMemberID = 0;
                clearFields();
            }
            UpdateUIState();
        }

        private void chkUpdateMember_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUpdateMember.Checked)
            {
                chkNewMember.Checked = false;
            }
            UpdateUIState();
        }

        private void UpdateUIState()
        {
            bool enableFields = chkNewMember.Checked || (chkUpdateMember.Checked && selectedMemberID != -1);
            txtFirstName.Enabled = enableFields;
            txtLastName.Enabled = enableFields;
            dateBirthTime.Enabled = enableFields;
            txtAddress.Enabled = enableFields;
            txtPhone.Enabled = enableFields;
            txtEmail.Enabled = enableFields;
            btnSave.Enabled = enableFields;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                clearFields();
                chkNewMember.Checked = false;
                chkUpdateMember.Checked = false;
                selectedMemberID = -1;
                UpdateUIState();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
