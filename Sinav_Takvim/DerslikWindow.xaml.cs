using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Sinav_Takvim;

public partial class DerslikWindow
{
    private Derslik derslik;
    
    public DerslikWindow(Derslik derslik)
    {
        this.derslik = derslik;
        InitializeComponent();
        DerslikBilgisiYaz();
        DerslikGorseliCiz();
    }

    private void DerslikBilgisiYaz()
    {
        KodTextBlock.Text = derslik.GetKod();
        AdTextBlock.Text = derslik.GetAd();
        SatirTextBlock.Text = derslik.GetSatir().ToString();
        SutunTextBlock.Text = derslik.GetSutun().ToString();
        SiraYapisiTextBlock.Text = derslik.GetSira_yapisi().ToString();
        KapasiteTextBlock.Text = derslik.GetKapasite().ToString();
    }

    private void DerslikGorseliCiz()
    {
        var satir = derslik.GetSatir();
        var sutun = derslik.GetSutun();
        var sira_yapisi = derslik.GetSira_yapisi();
        var gercek_sutun = sutun * sira_yapisi + (sutun - 1);
        
        DerslikGorselGrid.Rows = satir;
        DerslikGorselGrid.Columns = gercek_sutun;

        for (var i = 0; i < satir; i++)
        {
            for (var j = 0; j < gercek_sutun; j++)
            {
                if ((j + 1) % (sira_yapisi + 1) != 0)
                {
                    var rectangle = new Rectangle
                    {
                        Stroke = new SolidColorBrush(Colors.Black),
                        StrokeThickness = 5,
                        Width = 1000.0 / gercek_sutun,
                        Height = 50,
                        Fill = new SolidColorBrush(Colors.White)
                    };
                    DerslikGorselGrid.Children.Add(rectangle);
                }
                else
                {
                    DerslikGorselGrid.Children.Add(new Rectangle());
                }
            }
        }
    }
}