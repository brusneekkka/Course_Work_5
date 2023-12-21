using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Diagnostics;

namespace CourseWork_5
{
    internal class Data_Base
    {
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=MAKBOOK;Initial Catalog=course_work_db;Integrated Security=True");

        public void openConnection()
        {
            if (sqlConnection.State == System.Data.ConnectionState.Closed) 
            {
                sqlConnection.Open();
            }
        }

        public void closeConnection()
        {
            if (sqlConnection.State == System.Data.ConnectionState.Open)
            {
                sqlConnection.Close();
            }
        }

        public SqlConnection getConnection()
        {
            return sqlConnection;
        }

        public void add_to_db(int a, int b, int c, string result)
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();

            string querystring = $"INSERT INTO result_history (a, b, c, result) VALUES ({a}, {b}, {c}, '{result}')";

            SqlCommand command = new SqlCommand(querystring, this.getConnection());

            adapter.SelectCommand = command;
            adapter.Fill(table);

        }

        public void print_db_in_file(string filePath)
        {
            //string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            FileStream fs = File.Open(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            fs.Close();
            File.WriteAllText(filePath, string.Empty);

            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();

            string querystring = "SELECT * FROM [course_work_db].[dbo].[result_history]";

            SqlCommand command = new SqlCommand(querystring, this.getConnection());

            adapter.SelectCommand = command;
            adapter.Fill(table);

            this.openConnection();
            SqlDataReader sqlDataReader = command.ExecuteReader();

            int i = 0;
            while(sqlDataReader.Read())
            {
                string line = (
                    $"{sqlDataReader["id"]} " +
                    $"{sqlDataReader["a"]} " +
                    $"{sqlDataReader["b"]} " +
                    $"{sqlDataReader["c"]} " +
                    $"{sqlDataReader["result"]}"
                    );
                File.AppendAllText(filePath, line + Environment.NewLine);
            }
            this.closeConnection();
        }

        public void PrintFile(string filePath)
        {
            try
            {
                ProcessStartInfo info = new ProcessStartInfo
                {
                    Verb = "print",
                    FileName = filePath,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                };

                Process.Start(info);
                Console.WriteLine("Файл отправлен на печать.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
            }
        }
    }
}
