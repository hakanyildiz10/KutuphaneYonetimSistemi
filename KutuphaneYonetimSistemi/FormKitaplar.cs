using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KutuphaneYonetimSistemi
{
    public partial class FormKitaplar : Form
    {
        SqlConnection baglanti = new SqlConnection(@"Data Source=HPAVILION\SQLEXPRESS;Initial Catalog=DbYTAKutuphane;Integrated Security=True");
        public FormKitaplar()
        {
            InitializeComponent();
        }

        private void buttonKitapEkle_Click(object sender, EventArgs e)
        {
            try
            {
                baglanti.Open();

                SqlCommand sqlCommand = new SqlCommand("INSERT INTO TableKitaplar" +
                    "(KitapAdi, YazarAdi, YazarSoyadi, ISBN, Durum, KitapTurKodu) VALUES (@p1, @p2, @p3, @p4, @p5, @p6)", baglanti);
                sqlCommand.Parameters.AddWithValue("@p1", textBoxKitapAdi.Text);
                sqlCommand.Parameters.AddWithValue("@p2", textBoxYazarAdi.Text);
                sqlCommand.Parameters.AddWithValue("@p3", textBoxYazarSoyad.Text);
                sqlCommand.Parameters.AddWithValue("@p4", textBoxISBN.Text);
                sqlCommand.Parameters.AddWithValue("@p5", "true");
                sqlCommand.Parameters.AddWithValue("@p6", textBoxKitapTurKodu.Text);

                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kitap eklenirken hata oluştu" + ex.Message);
            }
            finally
            {
                baglanti.Close();
            }

            verileriGoster();
        }

        private void verileriGoster()
        {
            try
            {
                string q = "SELECT * FROM TableKitaplar";
                // * TableKitaplardan her şeyi çek anlamına gelir
                SqlDataAdapter da = new SqlDataAdapter(q, baglanti);
                //bu bizim oluşturduğumuz sanal tablo ve bunu SELECT * FROM yaparak doldurduk
                DataTable dt = new DataTable();
                //datagridview yukarıdaki iki kod aracılığıyla sql den verileri çağırır
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    dataGridViewKitaplar.DataSource = dt;
                    //burada da sanal tablo ile datagridview u bağladık
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FormKitaplar_Load(object sender, EventArgs e)
        //action veya event olarak adlandırılır
        {
            verileriGoster();
        }

        private void dataGridViewKitaplar_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            labelGecikmeBedeli.Text = "0";
            int secilenSatir = dataGridViewKitaplar.SelectedCells[0].RowIndex;
            labelID.Text = dataGridViewKitaplar.Rows[secilenSatir].Cells[0].Value.ToString();
            textBoxKitapAdi.Text = dataGridViewKitaplar.Rows[secilenSatir].Cells[1].Value.ToString();
            textBoxYazarAdi.Text = dataGridViewKitaplar.Rows[secilenSatir].Cells[2].Value.ToString();
            textBoxYazarSoyad.Text = dataGridViewKitaplar.Rows[secilenSatir].Cells[3].Value.ToString();
            textBoxISBN.Text = dataGridViewKitaplar.Rows[secilenSatir].Cells[4].Value.ToString();
            textBoxKitapTurKodu.Text = dataGridViewKitaplar.Rows[secilenSatir].Cells[8].Value.ToString();

            textBoxOduncAlan.Text = dataGridViewKitaplar.Rows[secilenSatir].Cells[6].Value.ToString();
            if (dataGridViewKitaplar.Rows[secilenSatir].Cells[7].Value != DBNull.Value)
                //tarih olan hücre boşsa cast etmekle uğraşma DBNull şeklinde yazılmalı
                dateTimePickerOduncAlmaTarihi.Value = (DateTime)dataGridViewKitaplar.Rows[secilenSatir].Cells[7].Value;
            //(DateTime) cast etmektir 


        }

        private void buttonKitapBilgiGuncelle_Click(object sender, EventArgs e)
        {
            try
            {
                baglanti.Open();
                SqlCommand sqlCommand = new SqlCommand("UPDATE TableKitaplar SET KitapAdi = @p1, YazarAdi=@p2, YazarSoyadi=@p3, ISBN=@P4, KitapTurKodu=@p5" +
                                                      " WHERE ID = @p6", baglanti);
                sqlCommand.Parameters.AddWithValue("@p1", textBoxKitapAdi.Text);
                sqlCommand.Parameters.AddWithValue("@p2", textBoxYazarAdi.Text);
                sqlCommand.Parameters.AddWithValue("@p3", textBoxYazarSoyad.Text);
                sqlCommand.Parameters.AddWithValue("@p4", textBoxISBN.Text);
                sqlCommand.Parameters.AddWithValue("@p5", textBoxKitapTurKodu.Text);
                sqlCommand.Parameters.AddWithValue("@p6", labelID.Text);
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kitap güncellenirken hata oluştu" + ex.Message);
            }
            finally
            {
                baglanti.Close();
            }

            verileriGoster();

        }


        private void buttonKitapOduncVer_Click(object sender, EventArgs e)
        {
            if (labelID.Text != "-")
            {
                try
                {
                    baglanti.Open();
                    SqlCommand sqlCommand = new SqlCommand("UPDATE TableKitaplar SET OduncAlan = @p1, OduncAlmaTarihi=@p2, Durum =@p3 WHERE ID = @p4", baglanti);
                    sqlCommand.Parameters.AddWithValue("@p1", textBoxOduncAlan.Text);
                    sqlCommand.Parameters.AddWithValue("@p2", SqlDbType.Date).Value = dateTimePickerOduncAlmaTarihi.Value.Date;
                    sqlCommand.Parameters.AddWithValue("@p3", "False");
                    sqlCommand.Parameters.AddWithValue("@p4", labelID.Text);
                    sqlCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Kitap ödünç alma işlemi sırasında hata oluştu!" + ex.Message);
                }
                finally
                {
                    baglanti.Close();
                }

                verileriGoster();
            }
            else
            {
                MessageBox.Show("Lütfen listeden bir kitap seçiniz !");
            }
        }
        private void buttonGecikmeBedeliHesapla_Click(object sender, EventArgs e)
        {
            if (labelID.Text != "-")
            {
                DateTime bugununTarihi = DateTime.Now;
                int gunFarki = (int)(bugununTarihi - dateTimePickerOduncAlmaTarihi.Value.Date).TotalDays;
                //ödünç alma ile teslim tarihi arasındaki gün farkını hesaplar
                if (gunFarki > 10)
                {
                    int gecikmeBedeli = (gunFarki - 10) * 1;
                    labelGecikmeBedeli.Text = gecikmeBedeli.ToString();
                    //geciken her gün için fark 1 ile çarpılıyor.
                }

            }
        }

        private void buttonKitabiIadeEt_Click(object sender, EventArgs e)
        {
            if (labelID.Text != "-")
            {
                try
                {
                    baglanti.Open();
                    SqlCommand sqlCommand = new SqlCommand("UPDATE TableKitaplar SET OduncAlan = @p1, OduncAlmaTarihi=@p2, Durum =@p3 WHERE ID = @p4", baglanti);


                    sqlCommand.Parameters.AddWithValue("@p1", "");
                    sqlCommand.Parameters.AddWithValue("@p2", SqlDbType.Date).Value = DBNull.Value;
                    sqlCommand.Parameters.AddWithValue("@p3", "False");
                    sqlCommand.Parameters.AddWithValue("@p4", labelID.Text);
                    sqlCommand.ExecuteNonQuery();
                    textBoxOduncAlan.Text = "";
                    //işlem sonunda textbox ı boşaltır
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Kitap iade alma işlemi sırasında hata oluştu!" + ex.Message);
                }
                finally
                {
                    baglanti.Close();
                }

                verileriGoster();
            }
        }

        private void buttonTemizle_Click(object sender, EventArgs e)
        {
            metinKutulariniTemizle();
            //altta metoda alındı burada çağırıldı
        }

        public void metinKutulariniTemizle()
        {
            labelID.Text = "";
            labelGecikmeBedeli.Text = "" ;
            textBoxKitapAdi.Text = "";
            textBoxYazarAdi.Text = "";
            textBoxYazarSoyad.Text = "";
            textBoxISBN.Text = "";
            textBoxKitapTurKodu.Text = "";
            textBoxOduncAlan.Text = "";
        }

        private void buttonAra_Click(object sender, EventArgs e)
        {
            aramaSonuclariniGoster();
        }


        private void aramaSonuclariniGoster()
        {

            try
            {
                string q = "SELECT * FROM TableKitaplar WHERE KitapAdi LIKE '" + textBoxKitapAdi.Text 
                                                                               + "%' AND YazarSoyadi LIKE '" + textBoxYazarAdi.Text + "%'"
                                                                               + " AND YazarSoyAdi LIKE '" + textBoxYazarSoyad.Text + "%'"
                                                                               + " AND ISBN LIKE '" + textBoxISBN.Text + "%'"
                                                                               + " AND KitapTurKodu LIKE '" + textBoxKitapTurKodu.Text + "%'"
                                                                               + " AND OduncAlan LIKE '" + textBoxISBN.Text + "%'";
                                                                               
                SqlDataAdapter da = new SqlDataAdapter(q, baglanti);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    dataGridViewKitaplar.DataSource = dt;
                    //burada da sanal tablo ile datagridview u bağladık
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonTumKitaplariGoster_Click(object sender, EventArgs e)
        {
            verileriGoster();
        }

        private void buttonSil_Click(object sender, EventArgs e)
        {
            if (labelID.Text == "-" || labelID.Text == "")
            {
                MessageBox.Show("Lütfen listeden silinecek kitabı seçin");
            }
            else
            {

                try
                {
                    baglanti.Open();
                    SqlCommand sqlCommand = new SqlCommand("DELETE FROM TableKitaplar WHERE ID = @p1", baglanti);
                    sqlCommand.Parameters.AddWithValue("@p1", labelID.Text);
                    sqlCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Kitap silinirken hata oluştu" + ex.Message);
                }
                finally
                {
                    baglanti.Close();
                }

                verileriGoster();
                metinKutulariniTemizle();
            }
        }

        private void FormKitaplar_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
            //ilk form kapandıktan sonra kayboldu ikinci formda iş bitince kapatıldı ve bu kod kapatılınca kodun çalılmasını durdurdu
        }
    }

}

           //datagridview e tıklayınca propertylerde autosizecolumnsmode kısmında fill seçeneğini seçersen tablo datagridview u tamamen kapsar
           //controlleri alt a basarak oynatırsan milimetrik hareket eder 