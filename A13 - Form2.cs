using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Sql;
using System.Data.SqlClient;

namespace WindowsFormsApp1
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        SqlConnection Kon = new SqlConnection(@"Data Source=degeneration\sqlexpress;Initial Catalog=EIT-А13;Integrated Security=True");
        SqlCommand Kom = new SqlCommand();

        private void BtnPrikazi_Click(object sender, EventArgs e)
        {
            try
            {
                Charta.Series.Clear();
                Charta.Series.Add("Filmovi");
                DataGridaViewa.Columns.Clear();
                Kom.Connection = Kon;
                Kom.CommandText = @"SELECT dbo.Producent.Ime AS Producent, COUNT(dbo.Producirao.FilmID) AS [Broj Filmova]
                                    FROM dbo.Producent INNER JOIN
                                    dbo.Producirao ON dbo.Producent.ProducentID = dbo.Producirao.ProducentID
                                    GROUP BY dbo.Producent.Ime";
                Kon.Open();
                SqlDataAdapter Da = new SqlDataAdapter(Kom);
                DataTable Dt = new DataTable();
                Da.Fill(Dt);
                DataGridaViewa.DataSource = Dt;
                Kon.Close();

                int i = 0;
                foreach (DataRow item in Dt.Rows) {
                    Charta.Series[0].Points.AddXY(item[0].ToString(), item[1]);
                    i++;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Kon.Close();
            }
        }

        private void BtnIzadji_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
