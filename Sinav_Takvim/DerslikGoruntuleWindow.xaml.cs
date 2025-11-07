using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Sinav_Takvim;

public partial class DerslikGoruntuleWindow
{
    private Derslik derslik;
    private List<Ogrenci> ogrenciList;
    
    public DerslikGoruntuleWindow(Derslik derslik, List<Ogrenci> ogrenciList)
    {
        this.derslik = derslik;
        this.ogrenciList = ogrenciList;
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
        OgrenciSayisiTextBlock.Text = ogrenciList.Count.ToString();
    }
    
    private void DerslikGorseliCiz()
    {
        var satir = derslik.GetSatir();
        var sutun = derslik.GetSutun();
        var sira_yapisi = derslik.GetSira_yapisi();
        var gercek_sutun = sutun * sira_yapisi + (sutun - 1);
        
        DerslikGorselGrid.Rows = satir;
        DerslikGorselGrid.Columns = gercek_sutun;

        int siraNumber = sira_yapisi == 2 ? 1 : 0;;

        int a = 0;

        int atlamaRakam = sira_yapisi == 5 ? 2 : sira_yapisi/2 + 1;

        for (var i = 0; i < satir; i++)
        {
            for (var j = 0; j < gercek_sutun; j++)
            {
                if ((j + 1) % (sira_yapisi + 1) != 0)
                {
                    var rectangle = new Rectangle
                    {
                        Stroke = new SolidColorBrush(Colors.Black),
                        StrokeThickness = 2.5,
                        Width = 1820.0 / gercek_sutun,
                        Height = 75,
                        Fill = new SolidColorBrush(Colors.White)
                    };
                    var grid = new Grid
                    {
                        Width = 1820.0 / gercek_sutun,
                        Height = 75
                    };
                    grid.Children.Add(rectangle);
                    
                    if (siraNumber % atlamaRakam == 0 && !(a >= ogrenciList.Count))
                    {
                        grid.Children.Add(new TextBlock
                        {
                            Text = ogrenciList[a].GetFormatliAd(),
                            TextAlignment = TextAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center
                        });
                        a++;
                    }
                    DerslikGorselGrid.Children.Add(grid);
                    
                    siraNumber++;
                }
                else
                {
                    DerslikGorselGrid.Children.Add(new Rectangle());
                    siraNumber = sira_yapisi == 2 ? 1 : 0;
                }
            }
            siraNumber = sira_yapisi == 2 ? 1 : 0;
        }
    }
}