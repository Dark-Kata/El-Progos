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
using System.Globalization;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SqlConnection Kon = new SqlConnection(@"Data Source=DEGENERATION\SQLEXPRESS;Initial Catalog=""EIT - A17"";Integrated Security=True");
        SqlCommand Kom = new SqlCommand();

        private void Ucitaj()
        {
            try //Ucitavanje u ListView
            {
                ListaBoxa.Items.Clear();
                Kom.Parameters.Clear();
                Kom.Connection = Kon;
                Kom.CommandText = "SELECT * FROM Vozilo";
                Kon.Open();
                SqlDataReader Dr =  Kom.ExecuteReader();
                while (Dr.Read())
                {
                    string s = Dr[0].ToString() + "\t" + Dr[1].ToString() + "\t" + Dr[2].ToString() + "\t" + Dr[3].ToString() + "\t" + Dr[4].ToString() + "\t" + Dr[5].ToString() + "\t" + Dr[6].ToString() + "\t" + Dr[7].ToString();
                    ListaBoxa.Items.Add(s);
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

            try //Ucitavanje u CBXModel
            {
                CBXModel.Items.Clear();
                Kom.Parameters.Clear();
                Kom.Connection = Kon;
                Kom.CommandText = "SELECT DISTINCT Naziv FROM Model";
                Kon.Open();
                SqlDataReader Dr = Kom.ExecuteReader();
                while (Dr.Read())
                {
                    CBXModel.Items.Add(Dr[0].ToString());
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

            try //Ucitavanje u CBXBoja
            {
                CBXBoja.Items.Clear();
                Kom.Parameters.Clear();
                Kom.Connection = Kon;
                Kom.CommandText = "SELECT DISTINCT Naziv FROM Boja";
                Kon.Open();
                SqlDataReader Dr = Kom.ExecuteReader();
                while (Dr.Read())
                {
                    CBXBoja.Items.Add(Dr[0].ToString());
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

            try //Ucitavanje u CBXGorivo
            {
                CBXGorivo.Items.Clear();
                Kom.Parameters.Clear();
                Kom.Connection = Kon;
                Kom.CommandText = "SELECT DISTINCT Naziv FROM Gorivo";
                Kon.Open();
                SqlDataReader Dr = Kom.ExecuteReader();
                while (Dr.Read())
                {
                    CBXGorivo.Items.Add(Dr[0].ToString());
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

            BtnOdustani.Visible = false;
            MSIzmeni.Visible = false;
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
            Kom.Parameters.Clear();
            Kom.Connection = Kon;
            id = ListaBoxa.SelectedIndex;
            string s = ListaBoxa.Items[id].ToString();
            var delovi = s.Split('\t');
            TxtBoxSifra.Text = delovi[0].ToString();
            TxtBoxRegistracija.Text = delovi[1].ToString();
            TxtBoxGodiste.Text = delovi[2].ToString();
            TxtBoxKM.Text = delovi[3].ToString();
            TxtBoxCena.Text = delovi[7].ToString();

            try // Punimo CBXModel
            {
                Kom.CommandText = "SELECT Naziv FROM Model WHERE ModelID=@m";
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
            } finally
            {
                Kon.Close();
            }

            try //Punimo CBXBoja
            {
                Kom.CommandText = "SELECT Naziv FROM Boja WHERE BojaID=@b";
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

            try //Puno CBXGorivo
            {
                Kom.CommandText = "SELECT Naziv FROM Gorivo WHERE GorivoID=@g";
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
            MSIzmeni.Visible = true;
            TxtBoxSifra.Enabled = false;
        }

        private void BtnOdustani_Click(object sender, EventArgs e)
        {
            TxtBoxSifra.Enabled = true;
            Resetuj();
            Ucitaj();
        }

        private void MSIzmeni_Click(object sender, EventArgs e)
        {
            if (TxtBoxSifra.Text != "" && TxtBoxRegistracija.Text != "" && TxtBoxGodiste.Text != "" && TxtBoxKM.Text != "" && TxtBoxCena.Text != "" && CBXModel.Text != "" && CBXBoja.Text != "" && CBXGorivo.Text != "")
            {
                try
                {
                    ListaBoxa.Items.Clear();
                    Kom.Connection = Kon;

                    string m = CBXModel.Text;
                    Kom.CommandText = "SELECT ModelID FROM Model WHERE Naziv=@m";
                    Kom.Parameters.Clear();
                    Kom.Parameters.AddWithValue("@m", m);
                    Kon.Open();
                    SqlDataReader Dr1 = Kom.ExecuteReader();
                    Dr1.Read();
                    int idm = (int)Dr1[0];
                    Kon.Close();

                    string b = CBXBoja.Text;
                    Kom.CommandText = "SELECT BojaID FROM Boja WHERE Naziv=@b";
                    Kom.Parameters.Clear();
                    Kom.Parameters.AddWithValue("@b", b);
                    Kon.Open();
                    SqlDataReader Dr2 = Kom.ExecuteReader();
                    Dr2.Read();
                    int idb = (int)Dr2[0];
                    Kon.Close();

                    string g = CBXGorivo.Text;
                    Kom.CommandText = "SELECT GorivoID FROM Gorivo WHERE Naziv=@g";
                    Kom.Parameters.Clear();
                    Kom.Parameters.AddWithValue("@g", g);
                    Kon.Open();
                    SqlDataReader Dr3 = Kom.ExecuteReader();
                    Dr3.Read();
                    int idg = (int)Dr3[0];
                    Kon.Close();

                    Kom.CommandText = "UPDATE Vozilo SET Registracija=@r, GodinaProizvodnje=@gp, PredjenoKM=@k, Cena=@c, ModelID=@idm, BojaID=@idb, GorivoID=@idg WHERE VoziloID=@v";
                    Kom.Parameters.Clear();
                    Kom.Parameters.AddWithValue("@v", TxtBoxSifra.Text);
                    Kom.Parameters.AddWithValue("@r", TxtBoxRegistracija.Text);
                    Kom.Parameters.AddWithValue("@gp", TxtBoxGodiste.Text);
                    Kom.Parameters.AddWithValue("@k", TxtBoxKM.Text);
                    Kom.Parameters.AddWithValue("@c", TxtBoxCena.Text);
                    Kom.Parameters.AddWithValue("@idm", idm);
                    Kom.Parameters.AddWithValue("@idb", idb);
                    Kom.Parameters.AddWithValue("@idg", idg);
                    Kon.Open();
                    if (Kom.ExecuteNonQuery() > 0)
                    {
                        //Kom.ExecuteNonQuery();
                        MessageBox.Show("Uspesno");
                    } else
                    {
                        MessageBox.Show("Neuspesno");
                    }
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
                    Resetuj();
                }
            } else
            {
                MessageBox.Show("Neuspesno");
                Kon.Close();
                Ucitaj();
                Resetuj();
            }
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
        
        private void textBox1_Leave(object sender, EventArgs e)
      {
          int id = Convert.ToInt32(textBox1.Text);
          SqlCommand kom = new SqlCommand("SELECT * FROM Vozilo WHERE VoziloID = @id", kon);
          kom.Parameters.AddWithValue("@id", textBox1.Text);
          kon.Open();
          SqlDataReader dr = kom.ExecuteReader();
          dr.Read();
          textBox1.Text = dr[0].ToString();
          textBox2.Text = dr[1].ToString();
          textBox3.Text = dr[2].ToString();
          textBox4.Text = dr[3].ToString();
          textBox5.Text = dr[7].ToString();
          int ModelID = Convert.ToInt32(dr[4]);
          int BojaID = Convert.ToInt32(dr[5]);
          int GorivoID = Convert.ToInt32(dr[6]);
          kon.Close();
          //Popunjavanje GorivoCB
          SqlCommand kom1 = new SqlCommand("SELECT Naziv FROM Gorivo Where GorivoID = @g", kon);
          kom1.Parameters.AddWithValue("@g", GorivoID);
          kon.Open();
          SqlDataReader dr1 = kom1.ExecuteReader();
          dr1.Read();
          comboBox3.Text = dr1[0].ToString();
          kon.Close();

          //Popunjavanje BojaCB
          SqlCommand kom12 = new SqlCommand("SELECT Naziv FROM Boja Where BojaID = @b", kon);
          kom12.Parameters.AddWithValue("@b", BojaID);
          kon.Open();
          SqlDataReader dr12 = kom12.ExecuteReader();
          dr12.Read();
          comboBox2.Text = dr12[0].ToString();
          kon.Close();

          //Popunjavanje ModelCB
          SqlCommand kom22 = new SqlCommand("SELECT Naziv FROM Model Where ModelID = @m", kon);
          kom22.Parameters.AddWithValue("@m", ModelID);
          kon.Open();
          SqlDataReader dr22 = kom22.ExecuteReader();
          dr22.Read();
          comboBox1.Text = dr22[0].ToString();
          kon.Close();
      }
        
    }
}
