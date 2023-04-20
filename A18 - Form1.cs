using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SqlConnection Kon = new SqlConnection(@"Data Source=DEGENERATION\SQLEXPRESS;Initial Catalog=EIT-A18;Integrated Security=True");
        SqlCommand Kom = new SqlCommand();

        private void Ucitaj()
        {
            try //Punimo CBXModel
            {
                CBXModel.Items.Clear();
                Kom.Connection = Kon;
                Kom.CommandText = "SELECT DISTINCT Naziv FROM Model";
                Kon.Open();
                SqlDataReader Dr = Kom.ExecuteReader();
                while (Dr.Read())
                {
                    CBXModel.Items.Add(Dr[0].ToString());
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

            try //Punimo CBXBoja
            {
                CBXBoja.Items.Clear();
                Kom.Connection = Kon;
                Kom.CommandText = "SELECT DISTINCT Naziv FROM Boja";
                Kon.Open();
                SqlDataReader Dr = Kom.ExecuteReader();
                while (Dr.Read())
                {
                    CBXBoja.Items.Add(Dr[0].ToString());
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

            try //Punimo CBXGorivo
            {
                CBXGorivo.Items.Clear();
                Kom.Connection = Kon;
                Kom.CommandText = "SELECT DISTINCT Naziv FROM Gorivo";
                Kon.Open();
                SqlDataReader Dr = Kom.ExecuteReader();
                while (Dr.Read())
                {
                    CBXGorivo.Items.Add(Dr[0].ToString());
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

            try //Punimo ListaBoxa
            {
                ListaBoxa.Items.Clear();
                Kom.Connection = Kon;
                Kom.CommandText = "SELECT * FROM Vozilo";
                Kon.Open();
                SqlDataReader Dr = Kom.ExecuteReader();
                while (Dr.Read())
                {
                    string s = Dr[0].ToString() + "\t" + Dr[1].ToString() + "\t" + Dr[2].ToString() + "\t" + Dr[3].ToString() + "\t" + Dr[4].ToString() + "\t" + Dr[5].ToString() + "\t" + Dr[6].ToString() + "\t" + Dr[7].ToString();
                    ListaBoxa.Items.Add(s);
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
            Kon.Close();
            BtnOdustani.Visible = false;
            MSUnesi.Visible = false;
            TxtBoxSifra.Enabled = true;
        }

        private void Resetuj()
        {
            TxtBoxSifra.Clear();
            TxtBoxRegistracija.Clear();
            TxtBoxGodiste.Clear();
            TxtBoxKM.Clear();
            TxtBoxCena.Clear();
            CBXModel.Text = "";
            CBXBoja.Text = "";
            CBXGorivo.Text = "";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Ucitaj();
            Resetuj();
        }

        int id = -1;
        private void ListaBoxa_SelectedIndexChanged(object sender, EventArgs e)
        {
            Kom.Connection = Kon;
            Kom.Parameters.Clear();
            id = ListaBoxa.SelectedIndex;
            string s = ListaBoxa.Items[id].ToString();
            var delovi = s.Split('\t');
            TxtBoxSifra.Text = delovi[0];
            TxtBoxRegistracija .Text = delovi[1].ToString();
            TxtBoxGodiste .Text = delovi[2].ToString();
            TxtBoxKM .Text = delovi[3].ToString();
            TxtBoxCena .Text = delovi[7].ToString();

            try //Punimo CBXModel
            {
                Kom.CommandText = "SELECT Naziv FROM Model WHERE ModelID=@m";
                Kom.Parameters.Clear();
                Kom.Parameters.AddWithValue("@m", Convert.ToString(delovi[4]));
                Kon.Open();
                SqlDataReader Dr = Kom.ExecuteReader();
                Dr.Read();
                CBXModel.SelectedIndex = CBXModel.FindString(Dr[0].ToString());
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

            try //Punimo CBXBoja
            {
                Kom.CommandText = "SELECT Naziv FROM Boja WHERE BojaID=@b";
                Kom.Parameters.Clear();
                Kom.Parameters.AddWithValue("@b", Convert.ToString(delovi[5]));
                Kon.Open();
                SqlDataReader Dr = Kom.ExecuteReader();
                Dr.Read();
                CBXBoja.SelectedIndex = CBXBoja.FindString(Dr[0].ToString());
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

            try //Punimo CBXGorivo
            {
                Kom.CommandText = "SELECT Naziv FROM Gorivo WHERE GorivoID=@g";
                Kom.Parameters.Clear();
                Kom.Parameters.AddWithValue("@g", Convert.ToString(delovi[6]));
                Kon.Open();
                SqlDataReader Dr = Kom.ExecuteReader();
                Dr.Read();
                CBXGorivo.SelectedIndex = CBXGorivo.FindString(Dr[0].ToString());
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

        private void BtnPriprema_Click(object sender, EventArgs e)
        {
            BtnOdustani.Visible = true;
            MSUnesi.Visible = true;
            TxtBoxSifra.Enabled = false;
        }

        private void BtnOdustani_Click(object sender, EventArgs e)
        {
            Resetuj();
            Ucitaj();
        }

        private void MSUnesi_Click(object sender, EventArgs e)
        {
            if (TxtBoxSifra.Text != "" && TxtBoxRegistracija.Text != "" && TxtBoxGodiste.Text != "" && TxtBoxKM.Text != "" && TxtBoxCena.Text != "" && CBXModel.Text != "" && CBXBoja.Text != "" && CBXGorivo.Text != "")
            {
                try
                {
                    Kom.Parameters.Clear();
                    Kom.Connection = Kon;

                    //CBXModel
                    Kom.CommandText = "SELECT ModelID FROM Model WHERE Naziv=@m";
                    Kom.Parameters.Clear();
                    Kom.Parameters.AddWithValue("@m", CBXModel.Text);
                    Kon.Open();
                    SqlDataReader Dr1 = Kom.ExecuteReader();
                    Dr1.Read();
                    int idm = (int)Dr1[0];
                    Kon.Close();

                    //CBXBoja
                    Kom.CommandText = "SELECT BojaID FROM Boja WHERE Naziv=@b";
                    Kom.Parameters.Clear();
                    Kom.Parameters.AddWithValue("@b", CBXBoja.Text);
                    Kon.Open();
                    SqlDataReader Dr2 = Kom.ExecuteReader();
                    Dr2.Read();
                    int idb = (int)Dr2[0];
                    Kon.Close();

                    //CBXGorivo
                    Kom.CommandText = "SELECT GorivoID FROM Gorivo WHERE Naziv=@g";
                    Kom.Parameters.Clear();
                    Kom.Parameters.AddWithValue("@g", CBXGorivo.Text);
                    Kon.Open();
                    SqlDataReader Dr3 = Kom.ExecuteReader();
                    Dr3.Read();
                    int idg = (int)Dr3[0];
                    Kon.Close();

                    Kom.Connection = Kon;
                    Kom.CommandText = "INSERT INTO Vozilo (Registracija, GodinaProizvodnje, PredjenoKM, ModelID, BojaID, GorivoID, Cena) VALUES (@r, @g, @k, @m, @b, @g1, @c)";
                    Kom.Parameters.Clear();
                    Kom.Parameters.AddWithValue("@r", TxtBoxRegistracija.Text);
                    Kom.Parameters.AddWithValue("@g", TxtBoxGodiste.Text);
                    Kom.Parameters.AddWithValue("@k", TxtBoxKM.Text);
                    Kom.Parameters.AddWithValue("@c", TxtBoxCena.Text);
                    Kom.Parameters.AddWithValue("@m", idm);
                    Kom.Parameters.AddWithValue("@b", idb);
                    Kom.Parameters.AddWithValue("@g1", idg);
                    Kon.Open();
                    Kom.ExecuteNonQuery();
                    MessageBox.Show("Vozilo je ubaceno u bazu. \n Sifra vozila je " /* + Nmp kako da mu napisem vrednost iz tabele*/);
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
            } else
            {
                    MessageBox.Show("Nesto niste uneli");
            }
            Kon.Close();
            Resetuj();
            Ucitaj();
        }

        private void MSAnaliza_Click(object sender, EventArgs e)
        {
            Form2 form = new Form2();
            form.ShowDialog();
        }

        private void MSOapk_Click(object sender, EventArgs e)
        {
            Form3 form = new Form3();
            form.ShowDialog();
        }

        private void MSIzlaz_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
