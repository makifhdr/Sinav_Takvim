using System.Windows;
using System.Windows.Controls;

namespace Sinav_Takvim;

public partial class AdminWindow
{
    public AdminWindow()
    {
        InitializeComponent();
    }

    private void Bolum_Button_Click(object sender, RoutedEventArgs e)
    {
        var button = (Button)sender;
        var window = new BolumWindow(Enum.Parse<Bolum>(button.Tag.ToString() 
                                                             ?? throw new InvalidOperationException()));
        window.Show();
    }

    private void yeniKullanici_Button_Click(object sender, RoutedEventArgs e)
    {
        var window = new KullaniciEkleWindow();
        window.Show();
    }
}