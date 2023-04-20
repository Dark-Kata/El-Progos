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

        SqlConnection Kon = new SqlConnection(@"Data Source=DEGENERATION\SQLEXPRESS;Initial Catalog=4EIT-A05;Integrated Security=True");
        SqlCommand Kom = new SqlCommand();

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void BtnPrikazi_Click(object sender, EventArgs e)
        {
            try
            {
                DataGridaViewa.Columns.Clear();
                Charta.Series.Clear();
                Kom.Connection = Kon;
                Kom.CommandText = @"SELECT dbo.Aktivnosti.Dan, COUNT(dbo.Registar_Aktivnosti.DeteID) AS [Broj Dece]
                                    FROM dbo.Aktivnosti INNER JOIN
                                    dbo.Registar_Aktivnosti ON dbo.Aktivnosti.AktivnostID = dbo.Registar_Aktivnosti.AktivnostID
                                    GROUP BY dbo.Aktivnosti.Dan
                                    ORDER BY CASE
                                    WHEN Dan='Ponedeljak' THEN 1
                                    WHEN Dan='Utorak' THEN 2
                                    WHEN Dan='Sreda' THEN 3
                                    WHEN Dan='Cetvrtak' THEN 4
                                    WHEN Dan='Petak' THEN 5
                                    ELSE NULL END";
                Kom.Parameters.Clear();
                Kon.Open();
                Kom.ExecuteNonQuery();
                SqlDataAdapter Da = new SqlDataAdapter(Kom);
                DataTable Dt = new DataTable();
                Da.Fill(Dt);
                DataGridaViewa.DataSource = Dt;
                Kon.Close();
                Charta.Series.Add("Statistika");
                foreach (DataRow Dr in Dt.Rows)
                {
                    Charta.Series["Statistika"].Points.AddXY(Dr[0].ToString(), Dr[1]);
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

        private void BtnIzadji_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
