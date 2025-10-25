using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Sinav_Takvim;

public partial class BolumWindow
{
    private Bolum bolum;
    DerslikDAO derslikDAO = new();
    OgrenciDAO ogrenciDAO = new();
    DersDAO dersDAO = new();
    ListeYukleStatusDAO listeYukleStatusDAO = new();
    public BolumWindow(Bolum bolum)
    {
        this.bolum = bolum;
        InitializeComponent();
        TitleText.Text = $"{BolumHelper.GetIsim(bolum)} Sınav Yönetim";
        TitleText.Background = new SolidColorBrush(Colors.AliceBlue);
        TitleText.Padding = new Thickness(5);
        TitleText.Margin = new Thickness(10,20,10,10);
        DerslikListesiCiz();
        OgrenciListesiCiz();
        DersListesiCiz();
    }

    private void RefreshButton_Click(object sender, RoutedEventArgs e)
    {
        DersliklerGrid.Children.Clear();
        OgrenciGrid.Children.Clear();
        DersGrid.Children.Clear();
        DerslikListesiCiz();
        OgrenciListesiCiz();
        DersListesiCiz();
    }

    private void DersListesiButton_Click(object sender, RoutedEventArgs e)
    {
        var window = new DersListesiYukle(bolum);
        window.Show();
    }

    private void OgrenciListesiButton_Click(object sender, RoutedEventArgs e)
    {
        if (!listeYukleStatusDAO.GetDersListesi(bolum))
        {
            MessageBox.Show(
                "Öğrenci listesinden önce\nders listesi yüklenmelidir!",
                "Uyarı",
                MessageBoxButton.OK);
            return;
        }
        var window = new OgrenciListesiYukle(bolum);
        window.Show();
    }

    private void DerslikOlusturButton_Click(object sender, RoutedEventArgs e)
    {
        var window = new DerslikOlustur(bolum);
        window.Show();
    }

    private void SinavProgramiOlusturButton_Click(object sender, RoutedEventArgs e)
    {
        if (!listeYukleStatusDAO.GetDersListesi(bolum) || !listeYukleStatusDAO.GetOgrenciListesi(bolum))
        {
            MessageBox.Show(
                "Sınav programı oluşturmadan önce\ngerekli listeler yüklenmelidir!",
                "Uyarı",
                MessageBoxButton.OK);
            return;
        }
        if (derslikDAO.GetDerslikListesi(bolum).Count == 0)
        {
            MessageBox.Show(
                "Sınav programı oluşturmadan önce\nderslik eklemesi yapılmalıdır!",
                "Uyarı",
                MessageBoxButton.OK);
            return;
        }
        var window = new SinavProgramiOlusturWindow(bolum);
        window.Show();
    }

    private void CikisButton_Click(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }

    private void DerslikListesiCiz()
    {
        var derslikListesi = derslikDAO.GetDerslikListesi(bolum);
        
        foreach (var derslik in derslikListesi)
        {
            var derslikKod = new TextBlock
            {
                Text = derslik.GetKod()
            };
            DersliklerGrid.Children.Add(derslikKod);
            
            var derslikAd = new TextBlock
            {
                Text = derslik.GetAd()
            };
            DersliklerGrid.Children.Add(derslikAd);
            
            var derslikKapasite = new TextBlock
            {
                Text = derslik.GetKapasite().ToString()
            };
            DersliklerGrid.Children.Add(derslikKapasite);
            
            var derslikSatir = new TextBlock
            {
                Text = derslik.GetSatir().ToString()
            };
            DersliklerGrid.Children.Add(derslikSatir);
            
            var derslikSutun = new TextBlock
            {
                Text = derslik.GetSutun().ToString()
            };
            DersliklerGrid.Children.Add(derslikSutun);
            
            var derslikSiraYapisi = new TextBlock
            {
                Text = derslik.GetSira_yapisi().ToString()
            };
            DersliklerGrid.Children.Add(derslikSiraYapisi);
            
            var stackPanel = new StackPanel();
            
            var duzenleButton = new Button
            {
                Content = "Düzenle",
                Tag = derslikKod.Text
            };
            duzenleButton.Click += DerslikDuzenleButtonClick;
            stackPanel.Children.Add(duzenleButton);
            
            var silButton = new Button
            {
                Content = "Sil",
                Tag = derslikKod.Text
            };
            silButton.Click += DerslikSilButtonClick;
            stackPanel.Children.Add(silButton);

            stackPanel.Width = 100;
            stackPanel.VerticalAlignment = VerticalAlignment.Center;
            
            DersliklerGrid.Children.Add(stackPanel);

            foreach (var children in DersliklerGrid.Children)
            {
                if (children is TextBlock textBlock)
                {
                    textBlock.Background = new SolidColorBrush(Colors.AliceBlue);
                    textBlock.Padding = new Thickness(5);
                    textBlock.Margin = new Thickness(10);
                    textBlock.Width = 200;
                    textBlock.Height = 50;
                }
            }
        }
    }

    private void DerslikDuzenleButtonClick(object sender, RoutedEventArgs e)
    {
        if (sender is Button button)
        {
            var window = new DerslikDuzenle(derslikDAO.GetDerslik(button.Tag.ToString() ?? throw new InvalidOperationException(), bolum));
            window.Show();
        }
    }

    private void DerslikSilButtonClick(object sender, RoutedEventArgs e)
    {
        if (sender is Button button)
        {
            var result = MessageBox.Show(
                $"{derslikDAO.GetDerslik(button.Tag.ToString() ?? throw new InvalidOperationException(), bolum).GetAd()} " +
                                $"adlı dersliği silmek istediğinize emin misiniz?",
                "Silmeyi onaylayın",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                derslikDAO.DerslikSil(button.Tag.ToString() ?? throw new InvalidOperationException());
                DersliklerGrid.Children.Clear();
                DerslikListesiCiz();
            }
        }
    }
    
    private void DerslikAraButton_Click(object sender, RoutedEventArgs e)
    {
        var derslik = derslikDAO.GetDerslik(DerslikAramaBox.Text, bolum);

        if (derslik.GetBolum() != Bolum.Null)
        {
            var window = new DerslikWindow(derslik);
            window.Show();
        }
        else
        {
            MessageBox.Show(
                "Derslik bulunamadı!",
                "Uyarı",
                MessageBoxButton.OK);
        }
    }
    
    private void OgrenciListesiCiz()
    {
        var ogrenciListesi = ogrenciDAO.GetOgrenciListesi(bolum);
        
        foreach (var ogrenci in ogrenciListesi)
        {
            var ogrenciNumara = new TextBlock
            {
                Text = ogrenci.GetNumara(),
                TextAlignment = TextAlignment.Center,
                Background = new SolidColorBrush(Colors.AliceBlue),
                Padding = new Thickness(5),
                Margin = new Thickness(10)
            };
            OgrenciGrid.Children.Add(ogrenciNumara);
            
            var ogrenciAd = new TextBlock
            {
                Text = ogrenci.GetAd(),
                TextAlignment = TextAlignment.Center,
                Background = new SolidColorBrush(Colors.AliceBlue),
                Padding = new Thickness(5),
                Margin = new Thickness(10)
            };
            OgrenciGrid.Children.Add(ogrenciAd);
            
            var ogrenciSinif = new TextBlock
            {
                Text = ogrenci.GetSinif() + ". Sınıf",
                TextAlignment = TextAlignment.Center,
                Background = new SolidColorBrush(Colors.AliceBlue),
                Padding = new Thickness(5),
                Margin = new Thickness(10)
            };
            OgrenciGrid.Children.Add(ogrenciSinif);
            OgrenciGrid.VerticalAlignment = VerticalAlignment.Top;
        }
    }
    
    private void OgrenciAraButton_Click(object sender, RoutedEventArgs e)
    {
        var ogrenci = ogrenciDAO.GetOgrenci(OgrenciAramaBox.Text, bolum);

        if (!ogrenci.GetAd().Equals(""))
        {
            var window = new OgrenciWindow(ogrenci, bolum);
            window.Show();
        }
        else
        {
            MessageBox.Show(
                "Öğrenci bulunamadı!",
                "Uyarı",
                MessageBoxButton.OK);
        }
    }

    private void DersListesiCiz()
    {
        var dersListesi = dersDAO.GetDersler(bolum);

        foreach (var ders in dersListesi)
        {
            var dersKodu = new TextBlock
            {
                Text = ders.GetKod()
            };
            DersGrid.Children.Add(dersKodu);
            
            var dersAd = new TextBlock
            {
                Text = ders.GetAd()
            };
            DersGrid.Children.Add(dersAd);
            
            var dersOgretmen = new TextBlock
            {
                Text = ders.GetOgretmen()
            };
            DersGrid.Children.Add(dersOgretmen);
            
            var dersSinif = new TextBlock
            {
                Text = ders.GetSinif() + ". Sınıf"
            };
            DersGrid.Children.Add(dersSinif);
            
            var dersSecmeli = new TextBlock
            {
                Text = ders.GetSecmeli() ? "Evet" : "Hayır"
            };
            DersGrid.Children.Add(dersSecmeli);

            var dersButton = new Button()
            {
                Content = "Ders\nBilgisi",
                Tag = dersKodu.Text
            };
            dersButton.Click += DersBilgisiButton_Click;
            
            DersGrid.Children.Add(dersButton);
            DersGrid.VerticalAlignment = VerticalAlignment.Top;
        }

        foreach (var child in DersGrid.Children)
        {
            if (child is TextBlock textBlock)
            {
                textBlock.TextAlignment = TextAlignment.Center;
                textBlock.Background = new SolidColorBrush(Colors.AliceBlue);
                textBlock.Padding = new Thickness(5);
                textBlock.Margin = new Thickness(10);
            }
        }
    }

    private void DersBilgisiButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button)
        {
            var window = new DersBilgisiWindow(dersDAO.GetDers(button.Tag.ToString() ?? throw new InvalidOperationException(), 
                bolum), bolum);
            window.Show();
        }
    }
}
