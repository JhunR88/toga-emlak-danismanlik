using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Markup;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TOGA_EMLAK_VE_DANIŞMANLIK
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("İSTANBUL");
            comboBox1.Items.Add("ANKARA");
            comboBox1.Items.Add("ANTALYA");
            comboBox3.Items.Add("müstakil");
            comboBox3.Items.Add("site");
            comboBox3.Items.Add("rezidans");


            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
            // İlçe ComboBox yazma engeli
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;

            if (comboBox1.SelectedItem != null)
            {
                string selectedCity = comboBox1.SelectedItem.ToString().ToLower();

                switch (selectedCity)
                {
                    case "istanbul":
                        comboBox2.Items.AddRange(new string[] { "Kadıköy", "Beykoz", "Beşiktaş" });
                        break;
                    case "ankara":
                        comboBox2.Items.AddRange(new string[] { "Kızılay", "Çankaya", "Ulus" });
                        break;
                    case "antalya":
                        comboBox2.Items.AddRange(new string[] { "Muratpaşa", "Konyaaltı", "Kepez" });
                        break;
                    default:
                        MessageBox.Show("Lütfen geçerli bir şehir seçin.");
                        break;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source = JAYHUN; Initial Catalog = TOGAEMLAK; Integrated Security = True; Encrypt = True; TrustServerCertificate = True";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO EVBİLGİ_TB (IlAdi, IlceAdi, Adres, Kat, Metrekare, OdaSayisi, EvTur, YapimTarih, Yas, Fiyat, Tercih)" +
                               "VALUES (@IlAdi, @IlceAdi, @Adres, @Kat, @Metrekare, @OdaSayisi, @EvTur, @YapimTarih, @Yas, @Fiyat, @Tercih)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@IlAdi", comboBox1.Text);
                cmd.Parameters.AddWithValue("@IlceAdi", comboBox2.Text);
                cmd.Parameters.AddWithValue("@Adres", richTextBox1.Text);
                cmd.Parameters.AddWithValue("@Kat", int.Parse(textBox2.Text));
                cmd.Parameters.AddWithValue("@Metrekare", decimal.Parse(textBox3.Text));
                cmd.Parameters.AddWithValue("@OdaSayisi", decimal.Parse(textBox4.Text));
                cmd.Parameters.AddWithValue("@EvTur", comboBox3.Text);
                cmd.Parameters.AddWithValue("@YapimTarih", dateTimePicker1.Value);
                cmd.Parameters.AddWithValue("@Yas", int.Parse(textBox5.Text));
                cmd.Parameters.AddWithValue("@Fiyat", decimal.Parse(textBox6.Text));
                cmd.Parameters.AddWithValue("@Tercih", radioButton3.Checked ? "Satılık" : "Kiralik");

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Veri başarıyla kaydedildi!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message);
                }

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Resim Dosyaları|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
            openFileDialog.Title = "Resim Seçin";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                string fileName = Path.GetFileName(filePath);

                try
                {
                    byte[] imageBytes = File.ReadAllBytes(filePath);


                    string connectionString = "Data Source=JAYHUN;Initial Catalog=TOGAEMLAK; Integrated Security=True;TrustServerCertificate=True";

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();


                        string query = "INSERT INTO image_TB (FileName, imageData) VALUES (@FileName, @imageData)";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@FileName", fileName);
                            command.Parameters.AddWithValue("@imageData", imageBytes);

                            command.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("Resim başarıyla yüklendi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
    
}
