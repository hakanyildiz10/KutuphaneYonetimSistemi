using System.Data.SqlClient;

namespace KutuphaneYonetimSistemi
{
    public partial class FormGiris : Form
    {
        FormKitaplar formKitaplar;
        public FormGiris()
        {
            InitializeComponent();
        }

        SqlConnection baglanti = new SqlConnection(@"Data Source=HPAVILION\SQLEXPRESS;Initial Catalog=DbYTAKutuphane;Integrated Security=True");

        private void buttonGiris_Click(object sender, EventArgs e)
        {
            string sifre = "";

            try
            {
               baglanti.Open();
                SqlCommand sqlKomut = new SqlCommand("SELECT Sifre FROM TableKutuphaneYoneticileri WHERE KullaniciAdi = @p1", baglanti);
                sqlKomut.Parameters.AddWithValue("@p1", textBoxKullaniciAdi.Text);
                SqlDataReader sqlDataReader = sqlKomut.ExecuteReader();

                while (sqlDataReader.Read())
                {
                  sifre = sqlDataReader[0].ToString(); 
                  //yukarda sqlkomut nesnesini tanýmlarken sadece Sifre yi çektik numaralandýrmaya sýfýrdan baþlandýðý için köþeli parantezde sýfýr yazýlýr
                }

                if (sifre == textBoxSifre.Text)
                {
                    formKitaplar = new FormKitaplar();
                    this.Hide();
                    //kullanýcý adý ve þifre doðru girilirse formGiriþi kapatýr ama kod çalýþmaya devam eder
                    //kodun çalýþmasýný durdurmak için form a týkla eventlere gel formClosed a týkla ve kodu yaz
                    formKitaplar.Show();
                }
                else
                {
                    MessageBox.Show("Hata!");
                    textBoxKullaniciAdi.Text = "";
                    textBoxSifre.Text = "";

                }

                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                //hatayý yakalýyor ve gösteriyor
            }
            finally
            {
                baglanti.Close();
            }
        }

        // sql ile baðlantý yapmak için; server explorer da data connection a gel, baðlantý ekle de 
        // sonra çýkan pencerede microsoft sql server a týkla
        // sql server uygulamayý aç, object explorer da en üstte isim yazýyýor ( server name), bunu kopyala ve visual studio da server name a yapýþtýr
        // kimlik doðrulama kýsmý windows olarak kalabilir ancak baþka bir sql server hesabýna uzaktan baðlanmak gerekirse sql server kimlik doðrulmayý seç, kullanýcý adý ve parolayý gir
        // veritabaný kütüphane adý DbYTAKütüphane dir. ( bu proje için)
        // baðlantýyý sýna de ve baðlanýr
        // ayný pencerede geliþmiþe týkla data source kýsmýný tamamen kopyala ( home  shift ve ctrl c ) bi boþ sayfaya yapýþtýr. Bu connection string dir
        // buttonGiriþ in üstündeki kodu yaz
        // sqlConnection kodu hata verir, ampule týkla, en alta gel ve son sürümü bul ve yükle yi seç 
        // sonra try ýn içindeki kodlar yazýlýr

      

        
    }
}