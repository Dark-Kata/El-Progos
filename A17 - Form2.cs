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

        SqlConnection Kon = new SqlConnection(@"Data Source=DEGENERATION\SQLEXPRESS;Initial Catalog=""EIT - A17"";Integrated Security=True");
        SqlCommand Kom = new SqlCommand();

        private void Ucitaj()
        {
            try
            {
                CBXModel2.Items.Clear();
                Kom.Parameters.Clear();
                Kom.Connection = Kon;
                Kom.CommandText = "SELECT DISTINCT Naziv FROM Model";
                Kon.Open();
                SqlDataReader Dr = Kom.ExecuteReader();
                while (Dr.Read())
                {
                    CBXModel2.Items.Add(Dr[0].ToString());
                }
                Kon.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Kon.Close();
            }
            Kon.Close();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            Ucitaj();
        }

        private void BtnPrikazi_Click(object sender, EventArgs e)
        {
            if (CBXModel2.Text != "")
            {
                try
                {
                    DataGridaViewa.Columns.Clear();
                    Kom.Parameters.Clear();
                    Kom.Connection = Kon;
                    Kom.CommandText = @"SELECT dbo.Vozilo.GodinaProizvodnje AS [Godina Proizvodnje], ROUND(AVG(dbo.Vozilo.Cena),2) AS [Prosecna Cena]
                                        FROM dbo.Model INNER JOIN
                                        dbo.Vozilo ON dbo.Model.ModelID = dbo.Vozilo.ModelID
                                        WHERE (dbo.Model.Naziv = @m)
                                        GROUP BY dbo.Vozilo.GodinaProizvodnje, dbo.Model.Naziv";
                    Kom.Parameters.Clear();
                    Kom.Parameters.AddWithValue("@m", CBXModel2.Text);
                    Kon.Open();
                    SqlDataAdapter Da = new SqlDataAdapter(Kom);
                    DataTable Dt = new DataTable();
                    Da.Fill(Dt);
                    DataGridaViewa.DataSource = Dt;
                    Kon.Close();
                    Charta.Series["Serija"].Points.Clear();
                    Charta.Series["Serija"].IsValueShownAsLabel = true;
                    int i = 0;
                    foreach (DataRow Dr in Dt.Rows)
                    {
                        Charta.Series["Serija"].Points.AddXY(Dr[0], Dr[1]);
                        Charta.Series["Serija"].Points[i].LegendText = Dr[0].ToString();
                        i++;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                } finally
                {
                    Kon.Close();
                }
            } else
            {
                Kon.Close();
                MessageBox.Show("Niste nista selektovali");
                Ucitaj();
            }
        }

        private void BtnIzadji_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
