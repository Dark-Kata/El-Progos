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

        SqlConnection Kon = new SqlConnection(@"Data Source=DEGENERATION\SQLEXPRESS;Initial Catalog=EIT-A02;Integrated Security=True");
        SqlCommand Kom = new SqlCommand();

        private void PopuniCBX()
        {
            CBXAutor.Items.Clear();
            Kom.Connection = Kon;
            Kom.CommandText = "SELECT DISTINCT Ime, Prezime FROM Autor";
            Kon.Open();
            SqlDataReader Dr = Kom.ExecuteReader();
            while (Dr.Read())
            {
                string s = Dr[0].ToString() + " " + Dr[1].ToString();
                CBXAutor.Items.Add(s);
            }
            Kon.Close();
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            PopuniCBX();
        }

        private void BtnPrikazi_Click(object sender, EventArgs e)
        {
            if (CBXAutor.Text != "" && NumUDGodine.Value > 0)
            {
                try
                {
                    Kom.Connection = Kon;
                    Kom.Parameters.Clear();
                    int g = (int)NumUDGodine.Value;
                    int gg = 2023 - g;
                    string s = CBXAutor.Text;
                    var delovi = s.Split(' ');
                    Kom.CommandText = @"SELECT YEAR(dbo.Na_Citanju.DatumUzimanja) AS Godina, COUNT(dbo.Na_Citanju.KnjigaID) AS Broj
                                        FROM dbo.Na_Citanju INNER JOIN 
                                        dbo.Knjiga ON dbo.Na_Citanju.KnjigaID = dbo.Knjiga.KnjigaID INNER JOIN 
                                        dbo.Napisali ON dbo.Knjiga.KnjigaID = dbo.Napisali.KnjigaID INNER JOIN 
                                        dbo.Autor ON dbo.Napisali.AutorID = dbo.Autor.AutorID 
                                        WHERE (dbo.Autor.Prezime = @p) AND (dbo.Autor.Ime =@i) AND (YEAR(dbo.Na_Citanju.DatumUzimanja) BETWEEN 2022-@gg AND 2022)
                                        GROUP BY YEAR(dbo.Na_Citanju.DatumUzimanja)";
                    Kon.Open();
                    Kom.Parameters.AddWithValue("@i", delovi[0]);
                    Kom.Parameters.AddWithValue("@p", delovi[1]);
                    Kom.Parameters.AddWithValue("@gg", gg);
                    SqlDataAdapter Da = new SqlDataAdapter(Kom); 
                    DataTable Dt = new DataTable();
                    Da.Fill(Dt);
                    DataGridaViewa.DataSource = Dt;
                    Kon.Close();
                    Charta.Series.Clear();
                    Charta.Series.Add("Prikaz");
                    foreach (DataRow item in Dt.Rows)
                    {
                        Charta.Series["Prikaz"].Points.AddXY(item[0].ToString(), item[1]);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                } finally
                {
                    Kon.Close();
                }
            }
        }

        private void BtnIzadji_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
