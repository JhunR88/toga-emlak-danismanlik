using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using Excel = Microsoft.Office.Interop.Excel;




namespace TOGA_EMLAK_VE_DANIŞMANLIK
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }


        private void VerileriGetir()
        {
            string connectionString = "Data Source = JAYHUN; Initial Catalog = TOGAEMLAK; Integrated Security = True; Encrypt = True; TrustServerCertificate = True";
            string query = "SELECT ID, IlAdi, IlceAdi, Adres, Kat, Metrekare, OdaSayisi, EvTur, YapimTarih, Yas, Fiyat, Tercih FROM EVBİLGİ_TB";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    // DataGridView'e verileri bağla
                    dataGridView1.DataSource = dataTable;

                    // Kolon başlıklarını düzenleme (isteğe bağlı)
                    dataGridView1.Columns["ID"].HeaderText = "Ev ID";
                    dataGridView1.Columns["IlAdi"].HeaderText = "İl";
                    dataGridView1.Columns["IlceAdi"].HeaderText = "İlçe";
                    dataGridView1.Columns["Adres"].HeaderText = "Adres";
                    dataGridView1.Columns["Kat"].HeaderText = "Kat Numarası";
                    dataGridView1.Columns["Metrekare"].HeaderText = "Toplam Alan 1 (m²)";
                    dataGridView1.Columns["OdaSayisi"].HeaderText = "Oda + 1";
                    dataGridView1.Columns["EvTur"].HeaderText = "Ev Türü";
                    dataGridView1.Columns["YapimTarih"].HeaderText = "Yapım Tarihi";
                    dataGridView1.Columns["Yas"].HeaderText = "Ev Yaşı";
                    dataGridView1.Columns["Tercih"].HeaderText = "Ev Tercihi";
                    dataGridView1.Columns["Fiyat"].HeaderText = "Fiyat (₺)";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message);
                }
            }
        }




        private void Form4_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void Form4_Load(object sender, EventArgs e)
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

        private void button1_Click(object sender, EventArgs e)
        {
            // SQL Sorgusu
            string query = "SELECT ID, IlAdi, IlceAdi, Adres, Kat, Metrekare, OdaSayisi, EvTur, YapimTarih, Yas, Fiyat, Tercih FROM EVBİLGİ_TB WHERE IlAdi = @IlAdi AND IlceAdi = @IlceAdi AND Adres = @Adres";
            string connectionString = "Data Source = JAYHUN; Initial Catalog = TOGAEMLAK; Integrated Security = True; Encrypt = True; TrustServerCertificate = True";
            string adres;

            // Verileri DataGridView'e çekmek için DataTable kullanımı
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@IlAdi", comboBox1.SelectedItem);
                    cmd.Parameters.AddWithValue("@IlceAdi", comboBox2.SelectedItem);
                    cmd.Parameters.AddWithValue("@Adres", richTextBox1.Text);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    if (dataTable.Rows.Count > 0)
                    {
                        // DataGridView'e verileri bağla
                        dataGridView1.DataSource = dataTable;

                        // Kolon başlıklarını daha okunabilir yapmak için isteğe bağlı
                        dataGridView1.Columns["ID"].HeaderText = "Ev ID";
                        dataGridView1.Columns["IlAdi"].HeaderText = "İl";
                        dataGridView1.Columns["IlceAdi"].HeaderText = "İlçe";
                        dataGridView1.Columns["Adres"].HeaderText = "Adres";
                        dataGridView1.Columns["Kat"].HeaderText = "Kat Numarası";
                        dataGridView1.Columns["Metrekare"].HeaderText = "Toplam Alan (m²)";
                        dataGridView1.Columns["OdaSayisi"].HeaderText = "Oda + 1";
                        dataGridView1.Columns["EvTur"].HeaderText = "Ev Türü";
                        dataGridView1.Columns["YapimTarih"].HeaderText = "Yapım Tarihi";
                        dataGridView1.Columns["Yas"].HeaderText = "Ev Yaşı";
                        dataGridView1.Columns["Tercih"].HeaderText = "Ev Tercihi";
                        dataGridView1.Columns["Fiyat"].HeaderText = "Fiyat (₺)";
                    }
                    else
                    {
                        MessageBox.Show("Kayıt bulunamadı.");
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message);
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source = JAYHUN; Initial Catalog = TOGAEMLAK; Integrated Security = True; Encrypt = True; TrustServerCertificate = True";

            string Tercih;

            if (radioButton4.Checked)
            {
                Tercih = "SATILIK";
            }
            else if (radioButton3.Checked)
            {
                Tercih = "KİRALİK";
            }
            else
            {
                MessageBox.Show("Lütfen bir tercih seçiniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = "SELECT * FROM EVBİLGİ_TB WHERE Tercih = @Tercih";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Tercih", Tercih);

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();

                    try
                    {
                        connection.Open();
                        adapter.Fill(dataTable);

                        if (dataTable.Rows.Count == 0)
                        {
                            MessageBox.Show("Seçtiğiniz tercihe uygun veri bulunamadı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                        dataGridView1.DataSource = dataTable;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            // SQL Sorgusu
            string query = "SELECT ID, IlAdi, IlceAdi, Adres, Kat, Metrekare, OdaSayisi, EvTur, YapimTarih, Yas, Fiyat, Tercih FROM EVBİLGİ_TB WHERE Kat = @Kat AND Metrekare = @Metrekare AND OdaSayisi = @OdaSayisi AND EvTur = @EvTur";
            string connectionString = "Data Source = JAYHUN; Initial Catalog = TOGAEMLAK; Integrated Security = True; Encrypt = True; TrustServerCertificate = True";

            // Verileri DataGridView'e çekmek için DataTable kullanımı
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Kat", textBox1.Text);
                    cmd.Parameters.AddWithValue("@Metrekare", textBox2.Text);
                    cmd.Parameters.AddWithValue("@OdaSayisi", textBox3.Text);
                    cmd.Parameters.AddWithValue("@EvTur", comboBox3.SelectedItem);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    if (dataTable.Rows.Count > 0)
                    {
                        // DataGridView'e verileri bağla
                        dataGridView1.DataSource = dataTable;

                        // Kolon başlıklarını daha okunabilir yapmak için isteğe bağlı
                        dataGridView1.Columns["ID"].HeaderText = "Ev ID";
                        dataGridView1.Columns["IlAdi"].HeaderText = "İl";
                        dataGridView1.Columns["IlceAdi"].HeaderText = "İlçe";
                        dataGridView1.Columns["Adres"].HeaderText = "Adres";
                        dataGridView1.Columns["Kat"].HeaderText = "Kat Numarası";
                        dataGridView1.Columns["Metrekare"].HeaderText = "Toplam Alan (m²)";
                        dataGridView1.Columns["OdaSayisi"].HeaderText = "Oda + 1";
                        dataGridView1.Columns["EvTur"].HeaderText = "Ev Türü";
                        dataGridView1.Columns["YapimTarih"].HeaderText = "Yapım Tarihi";
                        dataGridView1.Columns["Yas"].HeaderText = "Ev Yaşı";
                        dataGridView1.Columns["Tercih"].HeaderText = "Ev Tercihi";
                        dataGridView1.Columns["Fiyat"].HeaderText = "Fiyat (₺)";
                    }
                    else
                    {
                        MessageBox.Show("Kayıt bulunamadı.");
                    }


                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message);
                }

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string query = "SELECT ID, IlAdi, IlceAdi, Adres, Kat, Metrekare, OdaSayisi, EvTur, YapimTarih, Yas, Fiyat, Tercih FROM EVBİLGİ_TB WHERE YapimTarih = @YapimTarih AND Yas = @Yas AND Fiyat = @Fiyat";
            string connectionString = "Data Source=JAYHUN;Initial Catalog=TOGAEMLAK;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    if (!int.TryParse(textBox5.Text, out int yas))
                    {
                        MessageBox.Show("Lütfen geçerli bir yaş değeri girin.");
                        return;
                    }

                    if (!decimal.TryParse(textBox6.Text, out decimal fiyat))
                    {
                        MessageBox.Show("Lütfen geçerli bir fiyat değeri girin.");
                        return;
                    }

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@YapimTarih", dateTimePicker1.Value.Date); // Örnek olarak DateTimePicker kontrolü
                    cmd.Parameters.AddWithValue("@Yas", yas);
                    cmd.Parameters.AddWithValue("@Fiyat", fiyat);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    if (dataTable.Rows.Count > 0)
                    {
                        dataGridView1.DataSource = dataTable;

                        dataGridView1.Columns["ID"].HeaderText = "Ev ID";
                        dataGridView1.Columns["IlAdi"].HeaderText = "İl";
                        dataGridView1.Columns["IlceAdi"].HeaderText = "İlçe";
                        dataGridView1.Columns["Adres"].HeaderText = "Adres";
                        dataGridView1.Columns["Kat"].HeaderText = "Kat Numarası";
                        dataGridView1.Columns["Metrekare"].HeaderText = "Toplam Alan (m²)";
                        dataGridView1.Columns["OdaSayisi"].HeaderText = "Oda + 1";
                        dataGridView1.Columns["EvTur"].HeaderText = "Ev Türü";
                        dataGridView1.Columns["YapimTarih"].HeaderText = "Yapım Tarihi";
                        dataGridView1.Columns["Yas"].HeaderText = "Ev Yaşı";
                        dataGridView1.Columns["Tercih"].HeaderText = "Ev Tercihi";
                        dataGridView1.Columns["Fiyat"].HeaderText = "Fiyat (₺)";
                    }
                    else
                    {
                        MessageBox.Show("Kayıt bulunamadı.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message);
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            // PrintDocument nesnesi oluştur
            PrintDocument belgeYazdir = new PrintDocument();
            belgeYazdir.PrintPage += new PrintPageEventHandler(yazdirmaMetodu);

            // PrintPreviewDialog ile önizleme göster
            PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog();
            printPreviewDialog.Document = belgeYazdir;
            if (printPreviewDialog.ShowDialog() == DialogResult.OK)
            {
                belgeYazdir.Print();
            }
        }
        private void yazdirmaMetodu(object sender, PrintPageEventArgs e)
        {
            // DataGridView'deki verileri yazdır
            Font font = new Font("Arial", 10);
            Brush brush = Brushes.Black;

            int x = 50; // Sol kenar boşluğu
            int y = 50; // Üst kenar boşluğu
            int rowHeight = 30; // Her satır için yükseklik

            // Başlık satırı yazdır
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                e.Graphics.DrawString(column.HeaderText, font, brush, x, y);
                x += column.Width;
            }

            x = 50;
            y += rowHeight;

            // Satır verilerini yazdır
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (!row.IsNewRow) // Yeni satır değilse yazdır
                {
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        e.Graphics.DrawString(cell.Value?.ToString() ?? "", font, brush, x, y);
                        x += dataGridView1.Columns[cell.ColumnIndex].Width;
                    }

                    x = 50;
                    y += rowHeight;
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("Aktarılacak veri bulunamadı!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                // Excel uygulaması oluştur
                Excel.Application excelApp = new Excel.Application
                {
                    Visible = true
                };

                // Yeni bir çalışma kitabı oluştur
                Excel.Workbook workbook = excelApp.Workbooks.Add(Type.Missing);
                Excel.Worksheet worksheet = (Excel.Worksheet)workbook.ActiveSheet;
                worksheet.Name = "Sonuçlar";

                // Sütun başlıklarını ekle
                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    worksheet.Cells[1, i + 1] = dataGridView1.Columns[i].HeaderText;
                }

                // Hücre verilerini doldur
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    for (int j = 0; j < dataGridView1.Columns.Count; j++)
                    {
                        worksheet.Cells[i + 2, j + 1] = dataGridView1.Rows[i].Cells[j].Value?.ToString() ?? "";
                    }
                }

                // Otomatik sütun genişliği
                worksheet.Columns.AutoFit();

                MessageBox.Show("Veriler Excel'e başarıyla aktarıldı!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            // DataGridView'de seçili satır var mı kontrol et
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen bir satır seçin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            //Seçili satırı al
            DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

            // Güncellenecek değerleri değişkenlere al
            int id = Convert.ToInt32(selectedRow.Cells["ID"].Value); // 'ID' sütunu
            string yeniIl = selectedRow.Cells["IlAdi"].Value?.ToString(); //il sütunu
            string yeniIlce = selectedRow.Cells["IlceAdi"].Value?.ToString(); //ilçe sütunu
            string yeniAdres = selectedRow.Cells["Adres"].Value?.ToString(); // 'Adres' sütunu
            int yeniKat = Convert.ToInt32(selectedRow.Cells["Kat"].Value); // 'Kat' sütunu
            int yenimtrKare = Convert.ToInt32(selectedRow.Cells["Metrekare"].Value); //metrekare sütunu
            int yeniOdaSayisi = Convert.ToInt32(selectedRow.Cells["OdaSayisi"].Value); //Oda sayısı sütunu
            string yeniEvTur = selectedRow.Cells["EvTur"].Value?.ToString(); // EvTur sütunu
            DateTime yeniYapimTarih = Convert.ToDateTime(selectedRow.Cells["YapimTarih"].Value);//yapim tarih sütunu
            int yeniYas = Convert.ToInt32(selectedRow.Cells["Yas"].Value); //yas sütunu
            decimal yeniFiyat = Convert.ToDecimal(selectedRow.Cells["Fiyat"].Value); // 'Fiyat' sütunu
            string yeniTercih = selectedRow.Cells["Tercih"].Value?.ToString(); //tercih sütunu

            // SQL Bağlantı dizesi
            string connectionString = "Data Source = JAYHUN; Initial Catalog = TOGAEMLAK; Integrated Security = True; Encrypt = True; TrustServerCertificate = True";

            // Güncelleme sorgusu
            string query = "UPDATE EVBİLGİ_TB SET IlAdi = @IlAdi, IlceAdi = @IlceAdi, Adres = @Adres, Kat = @Kat, Metrekare = @Metrekare, OdaSayisi = @OdaSayisi, EvTur = @EvTur, YapimTarih = @YapimTarih, Yas = @Yas, Fiyat = @Fiyat, Tercih = @Tercih WHERE ID = @ID";
            // SQL bağlantısı ve komut nesnesi
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.Parameters.AddWithValue("@IlAdi", yeniIl);
                    cmd.Parameters.AddWithValue("@IlceAdi", yeniIlce);
                    cmd.Parameters.AddWithValue("@Adres", yeniAdres);
                    cmd.Parameters.AddWithValue("@Kat", yeniKat);
                    cmd.Parameters.AddWithValue("@Metrekare", yenimtrKare);
                    cmd.Parameters.AddWithValue("@OdaSayisi", yeniOdaSayisi);
                    cmd.Parameters.AddWithValue("@EvTur", yeniEvTur);
                    cmd.Parameters.AddWithValue("@YapimTarih", yeniYapimTarih);
                    cmd.Parameters.AddWithValue("@Yas", yeniYas);
                    cmd.Parameters.AddWithValue("@Fiyat", yeniFiyat);
                    cmd.Parameters.AddWithValue("@Tercih", yeniTercih);

                    // Sorguyu çalıştır
                    int rowsAffected = cmd.ExecuteNonQuery();

                    // Başarı mesajı
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Güncelleme başarılı!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Güncelleme başarısız oldu. Lütfen tekrar deneyin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            // DataGridView'de seçili satır var mı kontrol et
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen bir satır seçin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            //Seçili satırı al
            DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

            // SQL Bağlantı dizesi
            string connectionString = "Data Source = JAYHUN; Initial Catalog = TOGAEMLAK; Integrated Security = True; Encrypt = True; TrustServerCertificate = True";
            // SİLME sorgusu
            string query = "DELETE EVBİLGİ_TB WHERE IlAdi = @IlAdi AND IlceAdi = @IlceAdi AND Adres = @Adres AND Kat = @Kat AND Metrekare = @Metrekare AND OdaSayisi = @OdaSayisi AND EvTur = @EvTur AND YapimTarih = @YapimTarih AND Yas = @Yas AND Fiyat = @Fiyat AND Tercih = @Tercih";
            // SQL bağlantısı ve komut nesnesi
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@IlAdi", selectedRow.Cells["IlAdi"].Value);
                    cmd.Parameters.AddWithValue("@IlceAdi", selectedRow.Cells["IlceAdi"].Value);
                    cmd.Parameters.AddWithValue("@Adres", selectedRow.Cells["Adres"].Value);
                    cmd.Parameters.AddWithValue("@Kat", selectedRow.Cells["Kat"].Value);
                    cmd.Parameters.AddWithValue("@Metrekare", selectedRow.Cells["Metrekare"].Value);
                    cmd.Parameters.AddWithValue("@OdaSayisi", selectedRow.Cells["OdaSayisi"].Value);
                    cmd.Parameters.AddWithValue("@EvTur", selectedRow.Cells["EvTur"].Value);
                    cmd.Parameters.AddWithValue("@YapimTarih", selectedRow.Cells["YapimTarih"].Value);
                    cmd.Parameters.AddWithValue("@Yas", selectedRow.Cells["Yas"].Value);
                    cmd.Parameters.AddWithValue("@Fiyat", selectedRow.Cells["Fiyat"].Value);
                    cmd.Parameters.AddWithValue("@Tercih", selectedRow.Cells["Tercih"].Value);
                    // Sorguyu çalıştır
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Kayıt başarıyla silindi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Kayıt bulunamadı veya silinemedi.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }
    }
}



    
