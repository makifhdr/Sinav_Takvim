using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Sinav_Takvim;

public partial class DersSecimiWindow
{
    private Bolum bolum;
    private static List<Ders> sinavDersler = [];
    
    private DersDAO dersDAO = new();
    
    public DersSecimiWindow(Bolum bolum)
    {
        this.bolum = bolum;
        InitializeComponent();
        DersListesiCiz();
    }
    
    public static List<Ders> GetDersSinav(){ return sinavDersler; }

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

            var sinavCheckBox = new CheckBox()
            {
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Tag = ders.GetKod()
            };
            
            DersGrid.Children.Add(sinavCheckBox);
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
        sinavDersler.Clear();
        foreach (var child in DersGrid.Children)
        {
            if (child is CheckBox { IsChecked: false } checkbox) 
                sinavDersler.Add(dersDAO.GetDers(checkbox.Tag.ToString()!, bolum));
        }
        Close();
    }
}