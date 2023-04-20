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
using System.Deployment.Application;
using System.Windows.Forms.DataVisualization.Charting;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SqlConnection Kon = new SqlConnection(@"Data Source=DEGENERATION\SQLEXPRESS;Initial Catalog=EIT-A16;Integrated Security=True");
        SqlCommand Kom = new SqlCommand();

        private void Ucitaj()
        {
            try //Unos u CBXPas
            {
                CBXPas.Items.Clear();
                Kom.Connection = Kon;
                Kom.CommandText = "SELECT PasID, Ime FROM Pas";
                Kon.Open();
                SqlDataReader Dr1 = Kom.ExecuteReader();
                while (Dr1.Read())
                {
                    string s1 = Dr1[0].ToString() + " | " + Dr1[1].ToString();
                    CBXPas.Items.Add(s1);
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

            try //Unos u CBXIzlozba
            {
                CBXIzlozba.Items.Clear();
                Kom.Connection = Kon;
                Kom.CommandText = "SELECT * FROM Izlozba";
                Kon.Open();
                SqlDataReader Dr2 = Kom.ExecuteReader();
                while (Dr2.Read())
                {
                    string s2 = Dr2[0].ToString() + " | " + Dr2[1].ToString() + " | " + ((DateTime)Dr2[2]).ToString("dd.MM.yyyy");
                    CBXIzlozba.Items.Add(s2);
                    CBXIzlozba2.Items.Add(s2);
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

            try //Unos u CBXKategorija
            {
                CBXKategorija.Items.Clear();
                Kom.Connection = Kon;
                Kom.CommandText = "SELECT * FROM Kategorija";
                Kon.Open();
                SqlDataReader Dr3 = Kom.ExecuteReader();
                while (Dr3.Read())
                {
                    string s3 = Dr3[0].ToString() + " | " + Dr3[1].ToString();
                    CBXKategorija.Items.Add(s3);
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
        }

        private void Resetuj()
        {
            CBXPas.Text = "";
            CBXIzlozba.Text = "";
            CBXKategorija.Text = "";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Ucitaj();
            Resetuj();
        }

        private void BtnPrijava_Click(object sender, EventArgs e)
        {
            if (CBXPas.Text != "" && CBXIzlozba.Text != "" && CBXKategorija.Text != "")
            {
                try
                {
                    string s = CBXPas.SelectedItem.ToString();
                    var delovi = s.Split(' ');
                    int pid = Convert.ToInt32(delovi[0]);

                    s = CBXIzlozba.SelectedItem.ToString();
                    delovi = s.Split(' ');
                    string iid = delovi[0];

                    s = CBXKategorija.SelectedItem.ToString();
                    delovi = s.Split(' ');
                    int kid = Convert.ToInt32(delovi[0]);

                    Kom.Connection = Kon;
                    Kom.CommandText = "INSERT INTO Rezultat (PasID, IzlozbaID, KategorijaID) VALUES (@p, @i, @k)";
                    Kom.Parameters.Clear();
                    Kom.Parameters.AddWithValue("@p", pid);
                    Kom.Parameters.AddWithValue("@i", iid);
                    Kom.Parameters.AddWithValue("@k", kid);
                    Kon.Open();
                    Kom.ExecuteNonQuery();
                    MessageBox.Show("Uspesno");
                    Kon.Close();
                    Ucitaj();
                    Resetuj();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + " \n\n " + "MOGUCE JE DA JE PAS VEC PRIJAVLJEN");
                }
                finally
                {
                    Kon.Close();
                }
            }
        }

        private void BtnZatvori_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void BtnPrikazi_Click(object sender, EventArgs e)
        {
            if (CBXIzlozba2.Text != "")
            {
                try
                {
                    string s = CBXIzlozba2.SelectedItem.ToString();
                    var delovi = s.Split(' ');
                    string iid = delovi[0];

                    DataGridaViewa.Columns.Clear();
                    Kom.Connection = Kon;
                    Kom.CommandText = "SELECT COUNT(PasID) FROM Rezultat WHERE IzlozbaID=@i";
                    Kom.Parameters.Clear();
                    Kom.Parameters.AddWithValue("@i", iid);
                    Kon.Open();
                    LblPrijavio.Text = "Ukupan broj pasa koji je prijavljen je " + Convert.ToInt32(Kom.ExecuteScalar().ToString());
                    Kon.Close();

                    Kom.CommandText = "SELECT COUNT(PasID) FROM Rezultat WHERE (IzlozbaID=@i) AND (Rezultat IS NOT NULL)";
                    Kon.Open();
                    LblTakmicio.Text = "Ukupan broj pasa koji se takmicio je " + Convert.ToInt32(Kom.ExecuteScalar().ToString());
                    Kon.Close();

                    Kom.CommandText = "SELECT r.KategorijaID ,k.Naziv , COUNT(*) AS BrojPasa FROM Rezultat r JOIN Kategorija k ON(k.KategorijaID = r.KategorijaID) WHERE Rezultat IS NOT NULL AND r.IzlozbaID='" + iid.ToString() + "' GROUP BY r.KategorijaID ,k.Naziv";
                    Kon.Open();
                    SqlDataAdapter Da = new SqlDataAdapter(Kom);
                    DataTable Dt = new DataTable();
                    Da.Fill(Dt);
                    DataGridaViewa.DataSource = Dt;
                    Charta.Series["Naziv"].Points.Clear();
                    foreach (DataRow Dr in Dt.Rows)
                    {
                        Charta.Series["Naziv"].Points.AddXY(Dr[0].ToString(), Dr[2].ToString());
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
            }
            else
            {
                Kon.Close();
            }
        }

        private void BtnIzadji_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
