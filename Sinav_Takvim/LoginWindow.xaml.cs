using System.Windows;

namespace Sinav_Takvim;

public partial class LoginWindow
{
    private readonly KullaniciDAO kullaniciDAO  = new();
    
    public LoginWindow()
    {
        InitializeComponent();
    }

    private void Login_Button_Click(object sender, RoutedEventArgs e)
    {
        var bolum = kullaniciDAO.KullaniciGiris(eposta.Text, sifre.Password);

        if (bolum != Bolum.Null)
        {
            if (bolum == Bolum.Admin)
            {
                var window = new AdminWindow();
                window.Show();
            }
            else
            {
                var window = new BolumWindow(bolum);
                window.Show();
            }

            Close();
        }
        else
        {
            UyariBox.Text = "Kullanıcı adı veya şifre hatalı";
        }
    }
}