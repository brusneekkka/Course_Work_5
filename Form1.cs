using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace CourseWork_5
{
    public partial class Form1 : Form
    {

        Data_Base dataBase = new Data_Base();
        string filePath = Path.Combine("C:\\Users\\dimbr\\OneDrive\\Рабочий стол", "DB_TEXT.txt");

        int a = int.MinValue;
        int b = int.MinValue;
        int c = int.MinValue;
        double S = 0;
        private StringBuilder logger;
        public Form1()
        {
            InitializeComponent();
            logger = new StringBuilder();
            openFileDialog1.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
        }

        private double find_n_a(int P, int a)
        {
            return (-P + 1 + Math.Sqrt(P*P - 2*P + (1 + 4*a))) / 2;
        }

        private double find_n_b(int P, int b, double n_a)
        {
            return (-(P + 2*n_a - 1) + Math.Sqrt(Math.Pow((P + 2*n_a - 1), 2.0) + 4*b)) / 2;
        }

        private double find_n_c(int P, int c, double n_a, double n_b)
        {
            return (-(P + 2*n_b + 2*n_a - 1)  +  Math.Sqrt(Math.Pow((P + 2*n_b + 2*n_a - 1), 2.0) + 4*c)) / 2;
        }

        private void find_P_Click(object sender, EventArgs e)
        {
            string result = "Given a, b and c, there are no solutions";

            for(int P = 1; P < a + 1; P++)
            {
                double n_a = find_n_a(P, a);
                if (Math.Truncate(n_a) == n_a)          // проверка на целочисленность
                {
                    double n_b = find_n_b(P, b, n_a);
                    if (Math.Truncate(n_b) == n_b)
                    {
                        double n_c = find_n_c(P, c, n_a, n_b);
                        if (Math.Truncate(n_c) == n_c)
                        {
                            if (n_c % 2 ==0)
                            {
                                S = (c / n_c) + 2 * ((n_c / 2) - 1) + 1;
                            }
                            else
                            {
                                S = (c / n_c) + 2 * (n_c / 2 - 0.5);
                            } 

                            label_result.Text = $"Номер дома Пети: {P}, Номер дома школы: {S}";
                            result = $"Number of Peters House = {P}, Number of School =  {S}";

                            dataBase.openConnection();
                            dataBase.add_to_db(a, b, c, result);
                            dataBase.closeConnection();
                            return;
                        }
                    }
                }
            }

            label_result.Text = "Решения для введенных данных нет.";

            dataBase.openConnection();
            dataBase.add_to_db(a, b, c, result);
            dataBase.closeConnection();
        }

        private void textBox_a_TextChanged(object sender, EventArgs e)
        {
            try
            {
                a = int.Parse(textBox_a.Text);
                if (a < 0)
                {
                    label_result.Text = "Номер дома не может быть отрицательным";
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                a = int.MinValue;
                logger.AppendLine($"переменная a некорректна {DateTime.Now}");
            }

            if (a!= int.MinValue && b != int.MinValue && c != int.MinValue)
                find_P.Enabled = true;
            else find_P.Enabled = false;
        }

        private void textBox_b_TextChanged(object sender, EventArgs e)
        {
            try
            {
                b = int.Parse(textBox_b.Text);
                if (b < 0)
                {
                    label_result.Text = "Номер дома не может быть отрицательным";
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                b = int.MinValue;
                logger.AppendLine($"переменная b некорректна {DateTime.Now}");
            }

            if (a != int.MinValue && b != int.MinValue && c != int.MinValue)
                find_P.Enabled = true;
            else find_P.Enabled = false;
        }

        private void textBox_c_TextChanged(object sender, EventArgs e)
        {
            try
            {
                c = int.Parse(textBox_c.Text);
                if (c < 0)
                {
                    label_result.Text = "Номер дома не может быть отрицательным";
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                c = int.MinValue;
                logger.AppendLine($"переменная c некорректна {DateTime.Now}");
            }

            if (a != int.MinValue && b != int.MinValue && c != int.MinValue)
                find_P.Enabled = true;
            else find_P.Enabled = false;
        }

        private void button_open_file_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string filename = openFileDialog1.FileName;
                string fileText = System.IO.File.ReadAllText(filename);
                string[] text_array = fileText.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                int[] abc_array = new int[3] {int.MinValue, int.MinValue, int.MinValue};
                
                for (int i = 0; i < 3; i++)
                {
                    try
                    {
                        abc_array[i] = int.Parse(text_array[i]);
                        if (abc_array[i] < 0)
                        {
                            label_result.Text = "Номер дома не может быть отрицательным";
                            throw new Exception();
                        }

                    }
                    catch (Exception)
                    {
                        logger.AppendLine($"переменная c некорректна {DateTime.Now}");
                        label_result.Text = "Данные в файле некорректны";
                        return;
                    }
                }
                a = abc_array[0]; b = abc_array[1]; c = abc_array[2];
                find_P_Click(sender, e);

            }
        }

        private void print_db_Click(object sender, EventArgs e)
        {
            dataBase.print_db_in_file(filePath);
        }

        private void printer_file_Click(object sender, EventArgs e)
        {
            dataBase.PrintFile(filePath);
        }


        // P - номер дома Пети
        // S - номер дома школы


    }
}
