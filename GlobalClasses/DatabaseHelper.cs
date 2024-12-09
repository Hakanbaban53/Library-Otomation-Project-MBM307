using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_Otomation
{

    public class DatabaseHelper
    {
        private string connectionString; // Veritabanı bağlantı dizesi / Database connection string

        public DatabaseHelper()
        {
            // Bağlantı dizesini yapılandırma dosyasından al / Get the connection string from the configuration file
            connectionString = ConfigurationManager.ConnectionStrings["LibraryAutomationDB"].ConnectionString;
        }

        // Veritabanı bağlantısını döndür / Returns the database connection
        public SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        // SQL komutunu çalıştırır ve etkilenen satır sayısını döndürür / Executes a SQL command and returns the number of affected rows
        public int ExecuteNonQuery(string commandText, SqlParameter[] parameters = null, bool isStoredProcedure = true)
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open(); // Bağlantıyı aç / Open the connection
                using (SqlCommand cmd = new SqlCommand(commandText, conn))
                {
                    // Komut türünü belirle / Set CommandType based on the parameter
                    cmd.CommandType = isStoredProcedure ? CommandType.StoredProcedure : CommandType.Text;
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters); // Parametreleri ekle / Add parameters

                    return cmd.ExecuteNonQuery(); // Komutu çalıştır ve etkilenen satır sayısını döndür / Execute the command and return the number of affected rows
                }
            }
        }

        // SQL sorgusunu çalıştırır ve sonuçları DataTable olarak döndürür / Executes a SQL query and returns the results as a DataTable
        public DataTable ExecuteQuery(string query, SqlParameter[] parameters, bool isStoredProcedure = true)
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open(); // Bağlantıyı aç / Open the connection
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.CommandType = isStoredProcedure ? CommandType.StoredProcedure : CommandType.Text; // Komut türünü belirle / Set CommandType based on the parameter
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters); // Parametreleri ekle / Add parameters
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable table = new DataTable(); // Yeni bir DataTable oluştur / Create a new DataTable
                        adapter.Fill(table); // Verileri DataTable'a doldur / Fill the DataTable with data
                        return table; // DataTable'ı döndür / Return the DataTable
                    }
                }
            }
        }
    }
}
