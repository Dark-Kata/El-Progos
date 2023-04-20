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

        SqlConnection Kon = new SqlConnection(@"Data Source=DEGENERATION\SQLEXPRESS;Initial Catalog=4EIT-A05;Integrated Security=True");
        SqlCommand Kom = new SqlCommand();

        private void Ucitaj()
        {
            ListaViewa.Items.Clear();
            Kom.Parameters.Clear();
            Kom.Connection = Kon;
            Kom.CommandText = "SELECT * FROM Aktivnosti";
            Kon.Open();
            SqlDataReader Dr = Kom.ExecuteReader();
            while (Dr.Read())
            {
                ListViewItem item = new ListViewItem(Dr[0].ToString());
                item.SubItems.Add(Dr[1].ToString());
                item.SubItems.Add(Dr[2].ToString());
                item.SubItems.Add(Dr[3].ToString());
                item.SubItems.Add(Dr[4].ToString());
                ListaViewa.Items.Add(item);
            }
            Kon.Close();
        }

        private void Resetuj()
        {
            TxtBoxSifra.Clear();
            TxtBoxNaziv.Clear();
            TxtBoxPocetak.Clear();
            TxtBoxZavrsetak.Clear();
            CBXDan.Text = "";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Ucitaj();
            Resetuj();
        }

        //int id = -1;
        private void ListaViewa_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (ListViewItem item in ListaViewa.SelectedItems)
            {
                //id = int.Parse(item.SubItems[0].Text);
                TxtBoxSifra.Text = item.SubItems[0].Text;
                TxtBoxNaziv.Text = item.SubItems[1].Text;
                CBXDan.Text = item.SubItems[2].Text;
                TxtBoxPocetak.Text = item.SubItems[3].Text;
                TxtBoxZavrsetak.Text = item.SubItems[4].Text;
            }
        }

        private void BtnUnesi_Click(object sender, EventArgs e)
        {
            if (TxtBoxSifra.Text != "" && TxtBoxNaziv.Text != "" && CBXDan.Text != "" && TxtBoxPocetak.Text != "" && TxtBoxZavrsetak.Text != "")
            {
                try
                {
                    Kom.Connection = Kon;
                    Kom.Parameters.Clear();
                    Kom.CommandText = "INSERT INTO Aktivnosti (AktivnostID, NazivAktivnosti, Dan, Pocetak, Zavrsetak) VALUES (@a, @n, @d, @p, @z)";
                    Kom.Parameters.AddWithValue("@a", Convert.ToInt32(TxtBoxSifra.Text));
                    Kom.Parameters.AddWithValue("@n", TxtBoxNaziv.Text);
                    Kom.Parameters.AddWithValue("@d", CBXDan.Text);
                    Kom.Parameters.AddWithValue("@p", TxtBoxPocetak.Text);
                    Kom.Parameters.AddWithValue("@z", TxtBoxZavrsetak.Text);
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
                } finally 
                { 
                    Kon.Close();
                }
            }
            else
            {
                Kon.Close();
                Resetuj();
                Ucitaj();
            }
        }

        private void BtnIzadji_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void TSButAnaliza_Click(object sender, EventArgs e)
        {
            Form2 form = new Form2();
            form.ShowDialog();
        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {
            Form3 form = new Form3();
            form.ShowDialog();
        }
    }
}
