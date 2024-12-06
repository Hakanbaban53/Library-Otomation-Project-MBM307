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
            FormHelper.NavigateBack();
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
            if (!System.Text.RegularExpressions.Regex.IsMatch(txtISBN.Text, @"^(978|979)-\d{1,5}-\d{1,7}-\d{1,7}-\d{1}$") && !System.Text.RegularExpressions.Regex.IsMatch(txtISBN.Text, @"^\d{13}$")
                && !System.Text.RegularExpressions.Regex.IsMatch(txtISBN.Text, @"^(978|979)-\d{10}$"))
            {
                throw new Exception("Geçerli bir ISBN numarası giriniz (978 veya 979 ile başlamalıdır ve 13 haneli olmalıdır. Örnek: 978-3-16-148410-0 veya 9780316769488)");
            }
        }

        private void dataGridBooks_CellClick(object sender, DataGridViewCellEventArgs e)
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
                            new SqlParameter("@Title", title),
                            new SqlParameter("@Author", author),
                            new SqlParameter("@ISBN", isbn),
                            new SqlParameter("@PublishedYear", publishedYear),
                            new SqlParameter("@Publisher", publisher),
                            new SqlParameter("@CategoryID", cmbCategory.SelectedValue)
                        };
                    db.ExecuteNonQuery("AddNewBook", parameters);
                }
                else if (chkUpdateBook.Checked)
                {
                    int bookID = selectedBookID;
                    parameters = new SqlParameter[] {
                            new SqlParameter("@BookID", bookID),
                            new SqlParameter("@Title", title),
                            new SqlParameter("@Author", author),
                            new SqlParameter("@ISBN", isbn),
                            new SqlParameter("@PublishedYear", publishedYear),
                            new SqlParameter("@Publisher", publisher),
                            new SqlParameter("@CategoryID", cmbCategory.SelectedValue)
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
                    db.ExecuteNonQuery("CheckBookBeforeDelete", parameters);
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
                selectedBookID = -1;
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

        private void btnManageCategories_Click(object sender, EventArgs e)
        {
            ManageBookCategories();
            getCategories();
        }

        private void ManageBookCategories()
        {
            using (Form bookCategoryManagementForm = new Form()
            {
                Text = "Kategorileri Yönet",
                StartPosition = FormStartPosition.CenterParent,
                Width = 300,
                Height = 300,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                Padding = new Padding(10)
            })
            {
                ListBox listBox = new ListBox() { Dock = DockStyle.Fill, Font = new Font("Microsoft Sans Serif", 12) };
                TextBox textBox = new TextBox() { Dock = DockStyle.Bottom, Margin = new Padding(10), Font = new Font("Microsoft Sans Serif", 12) };
                Button delete = new Button() { Text = "Sil", Dock = DockStyle.Bottom, Height = 30, Margin = new Padding(10), Font = new Font("Microsoft Sans Serif", 12) };
                Button add = new Button() { Text = "Ekle", Dock = DockStyle.Bottom, Height = 30, Margin = new Padding(10), Font = new Font("Microsoft Sans Serif", 12) };
                Button exit = new Button() { Text = "Çıkış", Dock = DockStyle.Bottom, Height = 30, Padding = new Padding(10), DialogResult = DialogResult.Cancel, Font = new Font("Microsoft Sans Serif", 12) };

                DatabaseHelper db = new DatabaseHelper();
                DataTable categories = db.ExecuteQuery("SELECT * FROM BookCategory", null, false);
                foreach (DataRow row in categories.Rows)
                {
                    listBox.Items.Add(new { Text = row["CategoryName"].ToString(), Value = row["CategoryID"] });
                }

                listBox.DisplayMember = "Text";
                listBox.ValueMember = "Value";

                bookCategoryManagementForm.Controls.Add(listBox);
                bookCategoryManagementForm.Controls.Add(textBox);
                bookCategoryManagementForm.Controls.Add(delete);
                bookCategoryManagementForm.Controls.Add(add);
                bookCategoryManagementForm.Controls.Add(exit);

                delete.Click += (s, e) =>
                {
                    if (listBox.SelectedItem != null)
                    {
                        var selectedCategory = (dynamic)listBox.SelectedItem;
                        int categoryId = selectedCategory.Value;

                        DialogResult dialogResult = MessageBox.Show($"Seçilen kategoriyi silmek istediğinize emin misiniz?", "Kategori Silme", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dialogResult == DialogResult.Yes)
                        {
                            SqlParameter[] parameters = new SqlParameter[] {
                                new SqlParameter("@CategoryID", categoryId)
                            };
                            db.ExecuteQuery("DELETE FROM BookCategory WHERE CategoryID = @CategoryID", parameters, false);
                            listBox.Items.Remove(listBox.SelectedItem);
                            textBox.Text = "";
                        }
                    }
                    else
                    {
                        MessageBox.Show("Lütfen silmek için bir kategori seçin.");
                    }
                };

                add.Click += (s, e) =>
                {
                    try
                    {
                        string newCategoryName = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(textBox.Text.ToLower());
                        if (!string.IsNullOrWhiteSpace(newCategoryName))
                        {
                            SqlParameter[] checkParams = new SqlParameter[] {
                                new SqlParameter("@CategoryName", newCategoryName)
                            };
                            DataTable existingCategory = db.ExecuteQuery("SELECT * FROM BookCategory WHERE CategoryName = @CategoryName", checkParams, false);

                            if (existingCategory.Rows.Count > 0)
                            {
                                throw new Exception("Bu kategori zaten mevcut.");
                            }

                            SqlParameter[] parameters = new SqlParameter[] {
                                new SqlParameter("@CategoryName", newCategoryName)
                            };
                            db.ExecuteQuery("INSERT INTO BookCategory (CategoryName) VALUES (@CategoryName)", parameters, false);
                            MessageBox.Show("Kategori başarıyla eklendi.");
                            listBox.Items.Add(new { Text = newCategoryName, Value = categories.Rows.Count + 1 });
                            textBox.Text = "";
                        }
                        else
                        {
                            throw new Exception("Lütfen bir kategori adı girin.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };

                bookCategoryManagementForm.ShowDialog();
            }
        }
    }
}
