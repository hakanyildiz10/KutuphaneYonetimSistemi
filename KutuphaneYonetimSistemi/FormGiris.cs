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
                  //yukarda sqlkomut nesnesini tan�mlarken sadece Sifre yi �ektik numaraland�rmaya s�f�rdan ba�land��� i�in k��eli parantezde s�f�r yaz�l�r
                }

                if (sifre == textBoxSifre.Text)
                {
                    formKitaplar = new FormKitaplar();
                    this.Hide();
                    //kullan�c� ad� ve �ifre do�ru girilirse formGiri�i kapat�r ama kod �al��maya devam eder
                    //kodun �al��mas�n� durdurmak i�in form a t�kla eventlere gel formClosed a t�kla ve kodu yaz
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
                //hatay� yakal�yor ve g�steriyor
            }
            finally
            {
                baglanti.Close();
            }
        }

        // sql ile ba�lant� yapmak i�in; server explorer da data connection a gel, ba�lant� ekle de 
        // sonra ��kan pencerede microsoft sql server a t�kla
        // sql server uygulamay� a�, object explorer da en �stte isim yaz�y�or ( server name), bunu kopyala ve visual studio da server name a yap��t�r
        // kimlik do�rulama k�sm� windows olarak kalabilir ancak ba�ka bir sql server hesab�na uzaktan ba�lanmak gerekirse sql server kimlik do�rulmay� se�, kullan�c� ad� ve parolay� gir
        // veritaban� k�t�phane ad� DbYTAK�t�phane dir. ( bu proje i�in)
        // ba�lant�y� s�na de ve ba�lan�r
        // ayn� pencerede geli�mi�e t�kla data source k�sm�n� tamamen kopyala ( home  shift ve ctrl c ) bi bo� sayfaya yap��t�r. Bu connection string dir
        // buttonGiri� in �st�ndeki kodu yaz
        // sqlConnection kodu hata verir, ampule t�kla, en alta gel ve son s�r�m� bul ve y�kle yi se� 
        // sonra try �n i�indeki kodlar yaz�l�r

      

        
    }
}