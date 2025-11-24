using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TOGA_EMLAK_VE_DANIŞMANLIK
{
    public partial class Form1 : Form
    {
        Form2 ikinciForm = new Form2();

        public Form1()
        {
            InitializeComponent();
        }
        static string connectionString = "Data Source=JAYHUN;Initial Catalog = TOGAEMLAK; Integrated Security = True; Encrypt=True;TrustServerCertificate=True";


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            
            
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string kullaniciAdi = txtkullanici.Text;
            string sifre = txtsifre.Text;

             
            {using (SqlConnection con = new SqlConnection(connectionString))
                try
                {
                    con.Open();
                    string query = "SELECT COUNT (*) FROM GİRİS_TB WHERE KULLANICI = @kullaniciAdi AND Sifre = @sifre";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@kullaniciAdi", kullaniciAdi);
                    cmd.Parameters.AddWithValue("@sifre", sifre);

                    int result = (int)cmd.ExecuteScalar();
                    if (result > 0)
                    {
                        
                        ikinciForm.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Kullanıcı adı veya şifre hatalı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata" + ex.Message);
                }
            }
        }
    }
}
