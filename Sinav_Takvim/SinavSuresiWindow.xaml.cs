using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Sinav_Takvim;

public partial class SinavSuresiWindow
{
    private List<Ders> dersListesi = [];
    private Bolum bolum;

    private static List<(Ders, int)> dersSureListesi = [];
    
    private DersDAO dersDAO = new();
    
    public SinavSuresiWindow(List<Ders> dersListesi, Bolum bolum)
    {
        this.dersListesi = dersListesi;
        this.bolum = bolum;
        InitializeComponent();
        DersListesiCiz();
    }
    
    public static List<(Ders, int)> GetDersSureListesi(){ return dersSureListesi; }
    
    private void DersListesiCiz()
    {
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
                Text = ders.GetOgretmen(),
                TextAlignment = TextAlignment.Center
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

            var sinavTextBox = new TextBox()
            {
                Text = "75",
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Width = 218.5,
                Tag = ders.GetKod()
            };
            
            DersGrid.Children.Add(sinavTextBox);
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
    
    private void OnaylaButton_Click(object sender, RoutedEventArgs e)
    {
        dersSureListesi.Clear();
        foreach (var child in DersGrid.Children)
        {
            if (child is TextBox textBox)
            {
                if(int.TryParse(textBox.Text, out var result))
                    dersSureListesi.Add((dersDAO.GetDers(textBox.Tag.ToString()!, bolum), result));
                else
                {
                    ResultText.Foreground = new SolidColorBrush(Colors.Red);
                    ResultText.Text = "Lütfen girdiğiniz değerleri kontrol ediniz!";
                    return;
                }
            }
                
        }
        Close();
    }
    
    private void TopluSureTextBoxChanged(object sender, TextChangedEventArgs e)
    {
        if (sender is TextBox textBox)
        {
            foreach (var child in DersGrid.Children)
            {
                if (child is TextBox textBox2 && textBox.Text != textBox2.Text)
                {
                    textBox2.Text = textBox.Text;
                }
            }
        }
    }
}