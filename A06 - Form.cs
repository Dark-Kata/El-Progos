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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SqlConnection Kon = new SqlConnection(@"Data Source=DEGENERATION\SQLEXPRESS;Initial Catalog=4EIT-A06;Integrated Security=True");
        SqlCommand Kom = new SqlCommand();

        private void Ucitaj()
        {
            try
            {
                ListaBoxa.Items.Clear();
                Kom.Connection = Kon;
                Kom.CommandText = "SELECT Naziv FROM Proizvodjac";
                Kon.Open();
                SqlDataReader Dr = Kom.ExecuteReader();
                while (Dr.Read())
                {
                    CBXProizvodjac.Items.Add(Dr[0].ToString());
                }
                Kon.Close();

                Kom.CommandText = "SELECT m.ModelID, m.Naziv, p.Naziv FROM Model m JOIN Proizvodjac p ON (p.ProizvodjacID = m.ProizvodjacID)";
                Kon.Open();
                SqlDataReader Drr = Kom.ExecuteReader();
                while (Drr.Read())
                {
                    ListaBoxa.Items.Add(Drr[0].ToString().PadLeft(3,'0') + "\t" + Drr[1].ToString() + ", " + Drr[2].ToString());
                }
                Kon.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            } finally 
            { 
                Kon.Close(); 
            }
        }

        private void Resetuj()
        {
            CBXProizvodjac.Text = "";
            TxtBoxNaziv.Clear();
            TxtBoxSifra.Clear();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Ucitaj();
            Resetuj();
        }

        int i = 0;
        bool valid = false;
        private void BtnLupa_Click(object sender, EventArgs e)
        {
            try
            {
                if (TxtBoxSifra.Text != "")
                {
                    foreach (var item in ListaBoxa.Items)
                    {
                        var delovi = item.ToString().Split('\t');
                        if (TxtBoxSifra.Text.TrimStart('0') == delovi[0].TrimStart('0'))
                        {
                            ListaBoxa.SelectedIndex = i;
                            valid = true;
                            var delovi2 = delovi[1].Split(',');
                            CBXProizvodjac.Text = delovi2[1].Trim();
                            TxtBoxNaziv.Text = delovi2[0];
                            LblModel.Text = TxtBoxNaziv.Text;
                            break;
                        }
                        i++;
                        valid = false;
                    }
                    if (!valid)
                    {
                        MessageBox.Show("Ne postoji");
                        Resetuj();
                    }
                }
                else
                {
                    MessageBox.Show("Ne postoji");
                    ListaBoxa.SelectedIndex = -1;
                    Resetuj();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            } finally
            {
                Kon.Close();
                Ucitaj();
            }
        }

        private void BtnIzmeni_Click(object sender, EventArgs e)
        {
            if (TxtBoxNaziv.Text != "" && CBXProizvodjac.Text != "" && valid)
            {
                try
                {
                    Kom.Connection = Kon;
                    Kom.CommandText = "SELECT ProizvodjacID FROM Proizvodjac WHERE Naziv = @pn";
                    Kom.Parameters.Clear();
                    Kom.Parameters.AddWithValue("@pn", CBXProizvodjac.Text);
                    Kon.Open();
                    int pid = Convert.ToInt32(Kom.ExecuteScalar());
                    Kon.Close();

                    Kom.CommandText = "UPDATE Model SET Naziv = @n, ProizvodjacID = @pid WHERE ModelID = @id";
                    Kom.Parameters.Clear();
                    var delovi = ListaBoxa.Items[i].ToString().Split('\t');
                    Kom.Parameters.AddWithValue("@id", delovi[0].ToString());
                    Kom.Parameters.AddWithValue("@pid", pid);
                    Kom.Parameters.AddWithValue("@n", TxtBoxNaziv.Text);

                    Kon.Open();
                    Kom.ExecuteNonQuery();
                    MessageBox.Show("Uspeno");
                    Kon.Close();
                    Ucitaj();
                    Resetuj();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                Kon.Close();
                Ucitaj();
                }
            } else
            {
                MessageBox.Show("Unesite sifru i izaberite model");
            }
        }

        private void BtnIzadji_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void BtnPrikazi_Click(object sender, EventArgs e)
        {
            if (TxtBoxKM.Text != "")
            {
                try
                {
                    DataGridaViewa.Columns.Clear();
                    Charta.Series.Clear();
                    Kom.Connection = Kon;
                    Kom.CommandText = @"SELECT p.Naziv AS Proizvodjac, COUNT (m.ModelID) AS Broj FROM Proizvodjac p, Model m, Vozilo v
                                        WHERE (m.ProizvodjacID = p.ProizvodjacID) AND (v.ModelID = m.ModelID)
                                        AND v.PredjenoKM < @km AND v.GodinaProizvodnje BETWEEN @od AND @do
                                        GROUP BY p.Naziv ORDER BY p.Naziv";
                    Kom.Parameters.Clear();
                    Kom.Parameters.AddWithValue("@km", TxtBoxKM.Text);
                    Kom.Parameters.AddWithValue("@od", Convert.ToInt32(NumUDOd.Value));
                    Kom.Parameters.AddWithValue("@do", Convert.ToInt32(NumUDDo.Value));
                    Kon.Open();
                    SqlDataAdapter Da = new SqlDataAdapter(Kom);
                    DataTable Dt = new DataTable();
                    Da.Fill(Dt);
                    DataGridaViewa.DataSource = Dt;
                    Kon.Close();
                    Charta.Series.Add("Prikaz");
                    foreach (DataRow Dr in Dt.Rows)
                    {
                        Charta.Series["Prikaz"].Points.AddXY(Dr[0].ToString(), Dr[1].ToString());
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    throw;
                }
            } else
            {
                MessageBox.Show("Pogresna KM");
            }
        }

        private void BtnIzadji2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
