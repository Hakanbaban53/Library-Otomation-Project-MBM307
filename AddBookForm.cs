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
    public partial class AddBookForm : Form
    {
        public AddBookForm()
        {
            InitializeComponent();
        }

        private void btnAddBook_Click(object sender, EventArgs e)
        {
            // Collect data from text boxes
            string title = txtTitle.Text;
            string author = txtAuthor.Text;
            string isbn = txtISBN.Text;
            int publishedYear = int.Parse(txtPublishedYear.Text);
            string publisher = txtPublisher.Text;
            //int categoryID = int.Parse(txtCategoryID.Text);

            // Add book to database
            DatabaseHelper db = new DatabaseHelper();
            SqlParameter[] parameters = new SqlParameter[]
            {
        new SqlParameter("@Title", title),
        new SqlParameter("@Author", author),
        new SqlParameter("@ISBN", isbn),
        new SqlParameter("@PublishedYear", publishedYear),
        new SqlParameter("@Publisher", publisher),
        //new SqlParameter("@CategoryID", categoryID)
            };

            db.ExecuteNonQuery("AddNewBook", parameters);
            MessageBox.Show("Book added successfully!");
        }
    }
}
