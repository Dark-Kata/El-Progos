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

        SqlConnection Kon = new SqlConnection(@"Data Source=DEGENERATION\SQLEXPRESS;Initial Catalog=EIT-A02;Integrated Security=True");
        SqlCommand Kom = new SqlCommand();

        private void Ucitaj()
        {
            ListaViewa.Items.Clear();
            Kom.Parameters.Clear();
            Kom.Connection = Kon;
            Kom.CommandText = "SELECT * FROM Autor";
            Kon.Open();
            SqlDataReader Dr = Kom.ExecuteReader();
            while (Dr.Read())
            {
                ListViewItem item = new ListViewItem(Dr[0].ToString());
                item.SubItems.Add(Dr[1].ToString());
                item.SubItems.Add(Dr[2].ToString());
                item.SubItems.Add(((DateTime)Dr[3]).ToString("dd.MM.yyyy"));
                ListaViewa.Items.Add(item);
            }
            Kon.Close();
        }

        private void Resetuj()
        {
            TxtBoxSifra.Clear();
            TxtBoxIme.Clear();
            TxtBoxPrezime.Clear();
            TxtBoxRodjen.Clear();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Ucitaj();
            Resetuj();
        }

        int id = -1;
        private void ListaViewa_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (ListViewItem item in ListaViewa.SelectedItems)
            {
                id = Convert.ToInt32(item.SubItems[0].Text);
                TxtBoxSifra.Text = item.SubItems[0].Text;
                TxtBoxIme.Text = item.SubItems[1].Text;
                TxtBoxPrezime.Text = item.SubItems[2].Text;
                TxtBoxRodjen.Text = item.SubItems[3].Text;
            }
        }

        private void TSBBrisanje_Click(object sender, EventArgs e)
        {
            try
            {
                if (id > 0)
                {
                    Kom.Connection = Kon;
                    Kom.CommandText = "DELETE FROM Autor WHERE AutorID = @a";
                    Kom.Parameters.Clear();
                    Kom.Parameters.AddWithValue("@a", id);
                    Kon.Open();
                    Kom.ExecuteNonQuery();
                    MessageBox.Show("Uspesno");
                    Kon.Close();
                    Ucitaj();
                    Resetuj();
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

        private void TSBIzlaz_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
