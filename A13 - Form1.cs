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

        SqlConnection Kon = new SqlConnection(@"Data Source=degeneration\sqlexpress;Initial Catalog=EIT-А13;Integrated Security=True");
        SqlCommand Kom = new SqlCommand();

        private void Ucitaj()
        {
            try
            {
                ListaBoxa.Items.Clear();
                Kom.Parameters.Clear();
                Kom.Connection = Kon;
                Kom.CommandText = "SELECT * FROM Producent";
                Kon.Open();
                SqlDataReader Dr = Kom.ExecuteReader();
                while (Dr.Read())
                {
                    string s = Dr[0].ToString() + "\t" + Dr[1].ToString() + "\t" + Dr[2].ToString();
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
        }

        private void Resetuj()
        {
            TxtBoxSifra.Clear();
            TxtBoxIme.Clear();
            TxtBoxEmail.Clear();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Ucitaj();
            Resetuj();
        }

        int id = -1;
        private void ListaBoxa_SelectedIndexChanged(object sender, EventArgs e)
        {
            id = ListaBoxa.SelectedIndex;
            string s = ListaBoxa.Items[id].ToString();
            var delovi = s.Split('\t');
            TxtBoxSifra.Text = delovi[0].ToString();
            TxtBoxSifra.Enabled = false;
            TxtBoxIme.Text = delovi[1].ToString();
            TxtBoxEmail.Text = delovi[2].ToString();
        }

        private void TSBIzmena_Click(object sender, EventArgs e)
        {
            if (TxtBoxSifra.Text != "" && TxtBoxIme.Text != "" && TxtBoxEmail.Text != "")
            {
                try
                {
                    ListaBoxa.Items.Clear();
                    Kom.Connection = Kon;
                    Kom.CommandText = "UPDATE Producent SET Ime = @i, Email = @e WHERE ProducentID = " + TxtBoxSifra.Text;
                    Kom.Parameters.Clear();
                    Kom.Parameters.AddWithValue("@i", TxtBoxIme.Text);
                    Kom.Parameters.AddWithValue("@e", TxtBoxEmail.Text);
                    Kon.Open();
                    Kom.ExecuteNonQuery();
                    MessageBox.Show("Uspesno");
                    Kon.Close();
                    Ucitaj();
                    Resetuj();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                Kon.Close();
                Resetuj();
            }
        }

        private void TSBAnaliza_Click(object sender, EventArgs e)
        {
            Form2 form = new Form2();
            form.ShowDialog();
        }

        private void TSBOAPK_Click(object sender, EventArgs e)
        {
            Form3 form = new Form3();
            form.ShowDialog();
        }
    }
}
