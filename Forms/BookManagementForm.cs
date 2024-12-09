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
        private int selectedBookID = -1; // Seçilen kitabın ID'si / Selected book's ID

        public BookManagementForm()
        {
            InitializeComponent(); // Form bileşenlerini başlat / Initialize form components
            LoadBooks(); // Kitapları yükle / Load books
            UpdateUIState(); // UI durumunu güncelle / Update UI state
            GetCategories(); // Kategorileri al / Get categories
        }

        // Form alanlarını temizle / Clear form fields
        private void ClearFields()
        {
            txtTitle.Clear();
            txtAuthor.Clear();
            txtISBN.Clear();
            txtPublishedYear.Clear();
            txtPublisher.Clear();
            cmbCategory.SelectedIndex = -1;
        }

        // Kitap kategorilerini al ve combobox'a yükle / Get book categories and load into combobox
        private void GetCategories()
        {
            DatabaseHelper db = new DatabaseHelper();
            DataTable categories = db.ExecuteQuery("SELECT * FROM BookCategory", null, false);
            cmbCategory.DataSource = categories;
            cmbCategory.DisplayMember = "CategoryName";
            cmbCategory.ValueMember = "CategoryID";
        }

        // UI durumunu güncelle / Update UI state
        private void UpdateUIState()
        {
            bool enableFields = chkNewBook.Checked || (chkUpdateBook.Checked && selectedBookID != -1);
            txtTitle.Enabled = enableFields;
            txtAuthor.Enabled = enableFields;
            txtISBN.Enabled = enableFields;
            txtPublishedYear.Enabled = enableFields;
            txtPublisher.Enabled = enableFields;
            btnSave.Enabled = enableFields;
            cmbCategory.Enabled = enableFields;
        }

        // Kitapları yükle / Load books
        private void LoadBooks()
        {
            DatabaseHelper db = new DatabaseHelper();
            DataTable books = db.ExecuteQuery("SELECT b.BookID, b.Title, b.Author, b.ISBN, b.PublishedYear, b.Publisher, c.CategoryName FROM Books b JOIN BookCategory c ON b.CategoryID = c.CategoryID", null, false);
            dataGridBooks.DataSource = books;
            selectedBookID = -1; // Seçilen kitabı sıfırla / Reset selected book
        }

        // Alanları kontrol et / Check fields
        private void CheckFields()
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text) || string.IsNullOrWhiteSpace(txtAuthor.Text) ||
                string.IsNullOrWhiteSpace(txtISBN.Text) || string.IsNullOrWhiteSpace(txtPublishedYear.Text) ||
                string.IsNullOrWhiteSpace(txtPublisher.Text) || cmbCategory.SelectedIndex == -1)
            {
                throw new Exception("Tüm alanlar doldurulmalıdır."); // Hata mesajı / Error message
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(txtPublishedYear.Text, @"^\d{4}$"))
            {
                throw new Exception("Geçerli bir yayın yılı giriniz (4 haneli)."); // Hata mesajı / Error message
            }
            if (!IsValidISBN(txtISBN.Text))
            {
                throw new Exception("Geçerli bir ISBN numarası giriniz (978 veya 979 ile başlamalıdır ve 13 haneli olmalıdır. Örnek: 978-3-16-148410-0 veya 978-0316769488)"); // Hata mesajı / Error message
            }
        }

        // ISBN numarasının geçerliliğini kontrol et / Check validity of ISBN number
        private bool IsValidISBN(string isbn)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(isbn, @"^(978|979)-\d{1,5}-\d{1,7}-\d{1,7}-\d{1}$") ||
                   System.Text.RegularExpressions.Regex.IsMatch(isbn, @"^\d{13}$") ||
                   System.Text.RegularExpressions.Regex.IsMatch(isbn, @"^(978|979)-\d{10}$");
        }

        // DataGridView hücresine tıklandığında / When a cell in DataGridView is clicked
        private void dataGridBooks_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridBooks.Rows[e.RowIndex];
                selectedBookID = (int)row.Cells["BookID"].Value; // Seçilen kitabın ID'sini al / Get selected book's ID
                txtTitle.Text = row.Cells["Title"].Value.ToString();
                txtAuthor.Text = row.Cells["Author"].Value.ToString();
                txtISBN.Text = row.Cells["ISBN"].Value.ToString();
                txtPublishedYear.Text = row.Cells["PublishedYear"].Value.ToString();
                txtPublisher.Text = row.Cells["Publisher"].Value.ToString();
                cmbCategory.Text = row.Cells["CategoryName"].Value.ToString();
            }
            UpdateUIState(); // UI durumunu güncelle / Update UI state
        }

        // Geri butonuna tıklandığında / When back button is clicked
        private void btnBack_Click(object sender, EventArgs e)
        {
            FormHelper.NavigateBack(); // Geri git / Go back
        }

        // Kaydet butonuna tıklandığında / When save button is clicked
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                CheckFields(); // Alanları kontrol et / Check fields
                string title = txtTitle.Text;
                string author = txtAuthor.Text;
                string isbn = txtISBN.Text;
                int publishedYear = int.Parse(txtPublishedYear.Text);
                string publisher = txtPublisher.Text;

                DatabaseHelper db = new DatabaseHelper();
                SqlParameter[] parameters;

                // Yeni kitap ekle / Add new book
                if (chkNewBook.Checked)
                {
                    parameters = new SqlParameter[]
                    {
                                    new SqlParameter("@Title", title),
                                    new SqlParameter("@Author", author),
                                    new SqlParameter("@ISBN", isbn),
                                    new SqlParameter("@PublishedYear", publishedYear),
                                    new SqlParameter("@Publisher", publisher),
                                    new SqlParameter("@CategoryID", cmbCategory.SelectedValue)
                    };
                    db.ExecuteNonQuery("AddNewBook", parameters);
                }
                // Kitap güncelle / Update book
                else if (chkUpdateBook.Checked)
                {
                    parameters = new SqlParameter[]
                    {
                                    new SqlParameter("@BookID", selectedBookID),
                                    new SqlParameter("@Title", title),
                                    new SqlParameter("@Author", author),
                                    new SqlParameter("@ISBN", isbn),
                                    new SqlParameter("@PublishedYear", publishedYear),
                                    new SqlParameter("@Publisher", publisher),
                                    new SqlParameter("@CategoryID", cmbCategory.SelectedValue)
                    };
                    db.ExecuteNonQuery("UpdateBook", parameters);
                }
                LoadBooks(); // Kitapları yükle / Load books
                ClearFields(); // Alanları temizle / Clear fields
                UpdateUIState(); // UI durumunu güncelle / Update UI state
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); // Hata mesajını göster / Show error message
            }
        }

        // Kitap sil butonuna tıklandığında / When delete book button is clicked
        private void btnDeleteBook_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedBookID == -1)
                {
                    throw new Exception("Lütfen silmek istediğiniz kitabı seçiniz."); // Hata mesajı / Error message
                }
                string bookTitle = txtTitle.Text;

                DialogResult dialogResult = MessageBox.Show($"{bookTitle} adlı kitabı gerçekten silmek istiyor musunuz?", "Kitap Silme", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    DatabaseHelper db = new DatabaseHelper();
                    SqlParameter[] parameters = new SqlParameter[] { new SqlParameter("@BookID", selectedBookID) };
                    db.ExecuteNonQuery("CheckBookBeforeDelete", parameters); // Silmeden önce kontrol et / Check before delete
                    LoadBooks(); // Kitapları yükle / Load books
                    ClearFields(); // Alanları temizle / Clear fields
                    UpdateUIState(); // UI durumunu güncelle / Update UI state
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); // Hata mesajını göster / Show error message
            }
        }

        // Yeni kitap ekleme checkbox'ı değiştiğinde / When new book checkbox is changed
        private void chkNewBook_CheckedChanged(object sender, EventArgs e)
        {
            if (chkNewBook.Checked)
            {
                chkUpdateBook.Checked = false; // Güncelleme checkbox'ını kapat / Uncheck update checkbox
                dataGridBooks.ClearSelection(); // Seçimi temizle / Clear selection
                selectedBookID = -1; // Seçilen kitabı sıfırla / Reset selected book
                ClearFields(); // Alanları temizle / Clear fields
            }
            UpdateUIState(); // UI durumunu güncelle / Update UI state
        }

        // Güncelleme checkbox'ı değiştiğinde / When update checkbox is changed
        private void chkUpdateBook_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUpdateBook.Checked)
            {
                chkNewBook.Checked = false; // Yeni kitap checkbox'ını kapat / Uncheck new book checkbox
            }
            UpdateUIState(); // UI durumunu güncelle / Update UI state
        }

        // Temizle butonuna tıklandığında / When clear button is clicked
        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                ClearFields(); // Alanları temizle / Clear fields
                chkNewBook.Checked = false; // Yeni kitap checkbox'ını kapat / Uncheck new book checkbox
                chkUpdateBook.Checked = false; // Güncelleme checkbox'ını kapat / Uncheck update checkbox
                selectedBookID = -1; // Seçilen kitabı sıfırla / Reset selected book
                UpdateUIState(); // UI durumunu güncelle / Update UI state
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); // Hata mesajını göster / Show error message
            }
        }

        // Kategorileri yönet butonuna tıklandığında / When manage categories button is clicked
        private void btnManageCategories_Click(object sender, EventArgs e)
        {
            ManageBookCategories(); // Kategorileri yönet / Manage categories
            GetCategories(); // Kategorileri al / Get categories
        }

        // Kategori yönetim formunu aç / Open category management form
        private void ManageBookCategories()
        {
            using (Form bookCategoryManagementForm = new Form()
            {
                Text = "Kategorileri Yönet", // Manage Categories
                StartPosition = FormStartPosition.CenterParent,
                Width = 400,
                Height = 400,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                Padding = new Padding(10)
            })
            {
                ListBox listBox = new ListBox() { Dock = DockStyle.Fill, Font = new Font("Microsoft Sans Serif", 12) };
                TextBox textBox = new TextBox() { Text = "Yeni Kategori Ekleyin...", ForeColor = Color.Gray, Dock = DockStyle.Bottom, Margin = new Padding(10), Font = new Font("Microsoft Sans Serif", 12) };
                Button delete = new Button() { Text = "Sil", Dock = DockStyle.Bottom, Height = 30, Margin = new Padding(10), Font = new Font("Microsoft Sans Serif", 12) };
                Button add = new Button() { Text = "Ekle", Dock = DockStyle.Bottom, Height = 30, Margin = new Padding(10), Font = new Font("Microsoft Sans Serif", 12) };
                Button exit = new Button() { Text = "Çıkış", Dock = DockStyle.Bottom, Height = 30, Margin = new Padding(10), DialogResult = DialogResult.Cancel, Font = new Font("Microsoft Sans Serif", 12) };

                DatabaseHelper db = new DatabaseHelper();
                DataTable categories = db.ExecuteQuery("SELECT * FROM BookCategory", null, false);
                foreach (DataRow row in categories.Rows)
                {
                    listBox.Items.Add(new { Text = row["CategoryName"].ToString(), Value = row["CategoryID"] }); // Kategorileri listeye ekle / Add categories to list
                }

                listBox.DisplayMember = "Text";
                listBox.ValueMember = "Value";

                bookCategoryManagementForm.Controls.Add(listBox);
                bookCategoryManagementForm.Controls.Add(textBox);
                bookCategoryManagementForm.Controls.Add(delete);
                bookCategoryManagementForm.Controls.Add(add);
                bookCategoryManagementForm.Controls.Add(exit);

                // Yeni kategori metin kutusuna odaklandığında / When focus is on new category textbox
                textBox.GotFocus += (s, e) =>
                {
                    if (textBox.Text == "Yeni Kategori Ekleyin...")
                    {
                        textBox.Text = ""; // Metni temizle / Clear text
                        textBox.ForeColor = Color.Black; // Yazı rengini siyah yap / Change text color to black
                    }
                };

                // Yeni kategori metin kutusundan odak kaybettiğinde / When focus is lost from new category textbox
                textBox.LostFocus += (s, e) =>
                {
                    if (string.IsNullOrWhiteSpace(textBox.Text))
                    {
                        textBox.Text = "Yeni Kategori Ekleyin..."; // Varsayılan metni geri yükle / Restore default text
                        textBox.ForeColor = Color.Gray; // Yazı rengini gri yap / Change text color to gray
                    }
                };

                // Sil butonuna tıklandığında / When delete button is clicked
                delete.Click += (s, e) =>
                {
                    try
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
                                db.ExecuteQuery("DELETE FROM BookCategory WHERE CategoryID = @CategoryID", parameters, false); // Kategoriyi sil / Delete category
                                listBox.Items.Remove(listBox.SelectedItem); // Listeden çıkar / Remove from list
                                textBox.Clear(); // Metni temizle / Clear text
                            }
                        }
                        else
                        {
                            throw new Exception("Lütfen bir kategori seçin."); // Hata mesajı / Error message
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); // Hata mesajını göster / Show error message
                    }
                };

                // Ekle butonuna tıklandığında / When add button is clicked
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

                            if (string.IsNullOrWhiteSpace(newCategoryName) || newCategoryName == "Yeni Kategori Ekleyin...")
                            {
                                throw new Exception("Lütfen bir kategori adı girin."); // Hata mesajı / Error message
                            }

                            if (existingCategory.Rows.Count > 0)
                            {
                                throw new Exception("Bu kategori zaten mevcut."); // Hata mesajı / Error message
                            }

                            SqlParameter[] parameters = new SqlParameter[] {
                                        new SqlParameter("@CategoryName", newCategoryName)
                            };
                            db.ExecuteQuery("INSERT INTO BookCategory (CategoryName) VALUES (@CategoryName)", parameters, false); // Yeni kategori ekle / Add new category
                            MessageBox.Show("Kategori başarıyla eklendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information); // Başarılı mesajı / Success message
                            listBox.Items.Add(new { Text = newCategoryName, Value = categories.Rows.Count + 1 }); // Listeye ekle / Add to list
                            textBox.Clear(); // Metni temizle / Clear text
                        }
                        else
                        {
                            throw new Exception("Lütfen bir kategori adı girin."); // Hata mesajı / Error message
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); // Hata mesajını göster / Show error message
                    }
                };

                bookCategoryManagementForm.Controls.Add(exit); // Çıkış butonunu ekle / Add exit button
                bookCategoryManagementForm.ShowDialog(); // Formu göster / Show form
            }
        }
    }
}
