using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.Sql;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {

        SqlConnection Kon = new SqlConnection(@"Data Source=DEGENERATION\SQLEXPRESS;Initial Catalog=EIT-A01;Integrated Security=True");
        SqlCommand Kom = new SqlCommand();

        public Form1()
        {
            InitializeComponent();
        }

        public void Ucitaj()
        {
            ListaViewa.Items.Clear();
            Kom.Connection = Kon;
            Kom.CommandText = "SELECT * FROM Citalac";
            Kon.Open();
            Kom.ExecuteNonQuery();
            SqlDataReader Dr = Kom.ExecuteReader();
            while (Dr.Read())
            {
                ListViewItem Red = new ListViewItem(Dr[0].ToString());
                Red.SubItems.Add(Dr[1].ToString());
                Red.SubItems.Add(Dr[2].ToString());
                Red.SubItems.Add(Dr[3].ToString());
                Red.SubItems.Add(Dr[4].ToString());
                ListaViewa.Items.Add(Red);
            }
            Kon.Close();
        }

        public void Resetuj()
        {
            TxtBoxBrCk.Clear();
            TxtBoxJMBG.Clear();
            TxtBoxIme.Clear();
            TxtBoxPrezime.Clear();
            TxtBoxAdresa.Clear();
            BtnUpisi.Enabled = false;
        }

        private void ProveriBrCK(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (TxtBoxBrCk.Text != "")
                {
                    try
                    {
                        Kom.Connection = Kon;
                        Kom.CommandText = "SELECT * FROM Citalac WHERE CitalacID =" + Convert.ToInt32(TxtBoxBrCk.Text);
                        Kon.Open();
                        SqlDataReader Dr = Kom.ExecuteReader();
                        if (Dr != null)
                        {
                            while (Dr.Read()) 
                            { 
                                TxtBoxBrCk.Text = Dr[0].ToString();
                                TxtBoxJMBG.Text = Dr[1].ToString();
                                TxtBoxIme.Text = Dr[2].ToString();
                                TxtBoxPrezime.Text = Dr[3].ToString();
                                TxtBoxAdresa.Text = Dr[4].ToString();
                            }
                        }
                        else
                        {
                            Resetuj();
                            MessageBox.Show("Ne postoji");
                        }
                        Kon.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    Resetuj();
                    Kon.Close();
                }
            }
        }

        public void PopuniCBX()
        {
            CBXCitalac.Items.Clear();
            Kom.Connection = Kon;
            Kom.CommandText = "SELECT CitalacID, Ime, Prezime FROM Citalac";
            Kon.Open();
            SqlDataReader Dr = Kom.ExecuteReader();
            while (Dr.Read())
            {
                string s = Dr[0].ToString() + " | " + Dr[1].ToString() + " | " + Dr[2].ToString();
                CBXCitalac.Items.Add(s);
            }
            Kon.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Ucitaj();
            Resetuj();
            PopuniCBX();
        }

        private void BtnUpisi_Click(object sender, EventArgs e)
        {
            if (TxtBoxBrCk.Text != "" && TxtBoxJMBG.Text != "" && TxtBoxIme.Text != "" && TxtBoxPrezime.Text != "" && TxtBoxAdresa.Text != "")
            {
                try
                {
                    Kom.Connection = Kon;
                    Kom.CommandText = "INSERT INTO Citalac (CitalacID, MaticniBroj, Ime, Prezime, Adresa) VALUES (@cid, @jmbg, @im, @pr, @ad)";
                    Kom.Parameters.Clear();
                    Kom.Parameters.AddWithValue("@cid", Convert.ToInt32(TxtBoxBrCk.Text));
                    Kom.Parameters.AddWithValue("@jmbg", TxtBoxJMBG.Text);
                    Kom.Parameters.AddWithValue("@im", TxtBoxIme.Text);
                    Kom.Parameters.AddWithValue("@pr", TxtBoxPrezime.Text);
                    Kom.Parameters.AddWithValue("@ad", TxtBoxAdresa.Text);
                    Kon.Open();
                    Kom.ExecuteNonQuery();
                    MessageBox.Show("Uspesno");
                    Kon.Close();
                    Ucitaj();
                    Resetuj();
                    BtnUpisi.Enabled = false;
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                Kon.Close();
                MessageBox.Show("Nesto ne valja");
            }

        }

        private void BtnIzadji_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void BtnPrikazi_Click(object sender, EventArgs e)
        {
            string s = CBXCitalac.Text;
            var delovi = s.Split(' ');
            string id = delovi[0];
            int g1 = (int)NumUDOd.Value;
            int g2 = (int)NumUDDo.Value;
            Kom.Connection = Kon;
            Kom.CommandText = @"SELECT dbo.Citalac.Ime + ' ' + dbo.Citalac.Prezime AS Citalac, YEAR(dbo.Na_Citanju.DatumUzimanja) AS Godina, COUNT(dbo.Na_Citanju.DatumUzimanja) AS Preuzimanja, COUNT(dbo.Na_Citanju.DatumVracanja) AS Vracanje
                               FROM dbo.Citalac INNER JOIN
                               dbo.Na_Citanju ON dbo.Citalac.CitalacID = dbo.Na_Citanju.CitalacID
                               WHERE(dbo.Citalac.CitalacID = @id) AND(YEAR(dbo.Na_Citanju.DatumUzimanja) BETWEEN @g1 AND @g2)
                               GROUP BY dbo.Citalac.Ime + ' ' + dbo.Citalac.Prezime, YEAR(dbo.Na_Citanju.DatumUzimanja)";
            Kom.Parameters.Clear();
            Kon.Open();
            Kom.Parameters.AddWithValue("@id", id);
            Kom.Parameters.AddWithValue("@g1", g1);
            Kom.Parameters.AddWithValue("@g2", g2);
            SqlDataAdapter Da = new SqlDataAdapter(Kom);
            DataTable Dt = new DataTable();
            Da.Fill(Dt);
            DataGridaViewa.DataSource = Dt;
            Kon.Close();
            foreach (DataRow item in Dt.Rows)
            {
                Charta.Series["Broj_Iznajmljenih"].Points.AddXY(item[1].ToString(), item[2].ToString());
                Charta.Series["Broj_Vracenih"].Points.AddXY(item[1].ToString(), item[3].ToString());
            }
        }

        private void BtnIzadji2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
