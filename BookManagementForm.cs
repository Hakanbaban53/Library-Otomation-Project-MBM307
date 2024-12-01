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
    public partial class BookManagementForm : Form
    {
        int selectedBookID = -1;
        public BookManagementForm()
        {
            InitializeComponent();
            LoadBooks();
            UpdateUIState();
            getCategories();

        }
        private void clearFields()
        {
            txtTitle.Text = "";
            txtAuthor.Text = "";
            txtISBN.Text = "";
            txtPublishedYear.Text = "";
            txtPublisher.Text = "";
            cmbCategory.SelectedIndex = -1;
        }

        private void getCategories()
        {
            DatabaseHelper db = new DatabaseHelper();
            DataTable categories = db.ExecuteQuery("SELECT * FROM BookCategory", null, false);
            cmbCategory.DataSource = categories;
            cmbCategory.DisplayMember = "CategoryName";
            cmbCategory.ValueMember = "CategoryID";
        }

        private void UpdateUIState()
        {
            bool enableFields = chkNewBook.Checked || (chkUpdateBook.Checked && selectedBookID != -1);
            txtTitle.Enabled = enableFields;
            txtAuthor.Enabled = enableFields;
            txtISBN.Enabled = enableFields;
            txtPublishedYear.Enabled = enableFields;
            txtPublisher.Enabled = enableFields;
            txtAuthor.Enabled = enableFields;
            btnSave.Enabled = enableFields;
            cmbCategory.Enabled = enableFields;
        }

        private void LoadBooks()
        {
            DatabaseHelper db = new DatabaseHelper();
            DataTable books = db.ExecuteQuery("SELECT b.BookID, b.Title, b.Author, b.ISBN, b.PublishedYear, b.Publisher, c.CategoryName FROM Books b JOIN BookCategory c ON b.CategoryID = c.CategoryID", null, false);
            dataGridBooks.DataSource = books;
            selectedBookID = -1;

        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void checkFields()
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text) || string.IsNullOrWhiteSpace(txtAuthor.Text) ||
                string.IsNullOrWhiteSpace(txtISBN.Text) || string.IsNullOrWhiteSpace(txtPublishedYear.Text) ||
                string.IsNullOrWhiteSpace(txtPublisher.Text) || cmbCategory.SelectedIndex == -1)
            {
                throw new Exception("Tüm alanlar doldurulmalıdır.");
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(txtPublishedYear.Text, @"^\d{4}$"))
            {
                throw new Exception("Geçerli bir yayın yılı giriniz (4 haneli).");
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(txtISBN.Text, @"^(978|979)-\d{1,5}-\d{1,7}-\d{1,7}-\d{1}$"))
            {
                throw new Exception("Geçerli bir ISBN numarası giriniz (978 veya 979 ile başlamalıdır ve 13 haneli olmalıdır).");
            }
        }

        private void dataGridBooks_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridBooks.Rows[e.RowIndex];
                selectedBookID = (int)row.Cells["BookID"].Value;
                txtTitle.Text = row.Cells["Title"].Value.ToString();
                txtAuthor.Text = row.Cells["Author"].Value.ToString();
                txtISBN.Text = row.Cells["ISBN"].Value.ToString();
                txtPublishedYear.Text = row.Cells["PublishedYear"].Value.ToString();
                txtPublisher.Text = row.Cells["Publisher"].Value.ToString();
                cmbCategory.Text = row.Cells["CategoryName"].Value.ToString();
            }
            UpdateUIState();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                checkFields();
                string title = txtTitle.Text;
                string author = txtAuthor.Text;
                string isbn = txtISBN.Text;
                int publishedYear = int.Parse(txtPublishedYear.Text);
                string publisher = txtPublisher.Text;
                string category = cmbCategory.Text;

                DatabaseHelper db = new DatabaseHelper();
                SqlParameter[] parameters;

                if (chkNewBook.Checked)
                {
                    parameters = new SqlParameter[] {
                        new SqlParameter(
                            "@Title", title),
                        new SqlParameter(
                            "@Author", author),
                        new SqlParameter(
                            "@ISBN", isbn),
                        new SqlParameter(
                            "@PublishedYear", publishedYear),
                        new SqlParameter(
                            "@Publisher", publisher),
                        new SqlParameter(
                            "@CategoryID", cmbCategory.SelectedValue)
                };
                    db.ExecuteNonQuery("AddNewBook", parameters);
                }
                else if (chkUpdateBook.Checked)
                {
                    int bookID = selectedBookID;
                    parameters = new SqlParameter[] {
                        new SqlParameter(
                            "@BookID", bookID),
                        new SqlParameter(
                            "@Title", title),
                        new SqlParameter(
                            "@Author", author),
                        new SqlParameter(
                            "@ISBN", isbn),
                        new SqlParameter(
                            "@PublishedYear", publishedYear),
                        new SqlParameter(
                            "@Publisher", publisher),
                        new SqlParameter(
                            "@CategoryID", cmbCategory.SelectedValue)
                };
                    db.ExecuteNonQuery("UpdateBook", parameters);
                }
                LoadBooks();
                clearFields();
                UpdateUIState();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDeleteBook_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedBookID == -1)
                {
                    throw new Exception("Lütfen silmek istediğiniz kitabı seçiniz.");
                }
                int bookID = selectedBookID;
                string bookTitle = txtTitle.Text;

                DialogResult dialogResult = MessageBox.Show($"{bookTitle} adlı kitabı gerçekten silmek istiyor musunuz?", "Kitap Silme", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    DatabaseHelper db = new DatabaseHelper();
                    SqlParameter[] parameters = new SqlParameter[]
                    {
                        new SqlParameter("@BookID", bookID)
                    };
                    db.ExecuteNonQuery("DeleteBook", parameters);
                    LoadBooks();
                    clearFields();
                    UpdateUIState();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void chkNewBook_CheckedChanged(object sender, EventArgs e)
        {
            if (chkNewBook.Checked)
            {
                chkUpdateBook.Checked = false;
                dataGridBooks.ClearSelection();
                selectedBookID = 0;
                clearFields();
            }
            UpdateUIState();
        }

        private void chkUpdateBook_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUpdateBook.Checked)
            {
                chkNewBook.Checked = false;
            }
            UpdateUIState();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                clearFields();
                chkNewBook.Checked = false;
                chkUpdateBook.Checked = false;
                selectedBookID = -1;
                UpdateUIState();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
