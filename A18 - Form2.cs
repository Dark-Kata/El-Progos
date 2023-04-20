using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Data.Sql;
using System.Data;

namespace WindowsFormsApp1
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        SqlConnection Kon = new SqlConnection(@"Data Source=DEGENERATION\SQLEXPRESS;Initial Catalog=EIT-A18;Integrated Security=True");
        SqlCommand Kom = new SqlCommand();

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void BtnPrikazi_Click(object sender, EventArgs e)
        {
            if (TxtBoxMax.Text != "" && TxtBoxMin.Text != "")
            {
                try
                {
                    DataGridaViewa.Columns.Clear();
                    Kom.Connection = Kon;
                    Kom.CommandText = "SELECT VoziloID, Registracija, GodinaProizvodnje, PredjenoKM, Cena FROM Vozilo WHERE (Cena BETWEEN @min AND @max)";
                    Kom.Parameters.Clear();
                    Kom.Parameters.AddWithValue("@min", TxtBoxMin.Text);
                    Kom.Parameters.AddWithValue("@max", TxtBoxMax.Text);
                    Kon.Open();
                    SqlDataAdapter Da = new SqlDataAdapter(Kom);
                    DataTable Dt = new DataTable();
                    Da.Fill(Dt);
                    DataGridaViewa.DataSource = Dt;
                    Kon.Close();

                    Kom.CommandText = @"SELECT dbo.Proizvodjac.Naziv, COUNT(dbo.Model.ModelID) AS Broj
                                        FROM dbo.Model INNER JOIN
                                        dbo.Proizvodjac ON dbo.Model.ProizvodjacID = dbo.Proizvodjac.ProizvodjacID INNER JOIN
                                        dbo.Vozilo ON dbo.Model.ModelID = dbo.Vozilo.ModelID
                                        WHERE (Cena BETWEEN @min AND @max)
                                        GROUP BY dbo.Proizvodjac.Naziv, dbo.Vozilo.Cena";
                    Kom.Parameters.Clear();
                    Kom.Parameters.AddWithValue("@min", TxtBoxMin.Text);
                    Kom.Parameters.AddWithValue("@max", TxtBoxMax.Text);
                    Kon.Open();
                    SqlDataAdapter Da1 = new SqlDataAdapter(Kom);
                    DataTable Dt1 = new DataTable();
                    Da1.Fill(Dt1);
                    Kon.Close();
                    Charta.Series["Serija"].Points.Clear();
                    Charta.Series["Serija"].IsValueShownAsLabel = true;
                    int i = 0;
                    foreach (DataRow Dr in Dt1.Rows)
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
                MessageBox.Show("Niste uneli sve vrednosti");
            }
            Kon.Close();
        }

        private void BtnIzadji_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
