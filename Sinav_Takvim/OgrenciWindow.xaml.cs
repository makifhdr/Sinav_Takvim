using System.Windows;

namespace Sinav_Takvim;

public partial class OgrenciWindow
{
    private Ogrenci ogrenci;
    private Bolum bolum;
    
    private OgrenciDersDAO ogrenciDersDAO = new();
    
    public OgrenciWindow(Ogrenci ogrenci, Bolum bolum)
    {
        this.ogrenci = ogrenci;
        this.bolum = bolum;
        InitializeComponent();
        OgrenciDersYaz();
    }

    private void OgrenciDersYaz()
    {
        AdTextBlock.Text += ogrenci.GetAd();
        
        var dersList = ogrenciDersDAO.GetOgrenciDersler(ogrenci.GetNumara(), bolum);

        foreach (var ders in dersList)
        {
            DersListesiBlock.Text += $"- {ders.GetAd()} ({ders.GetKod()})";
            if(ders != dersList.Last())
                DersListesiBlock.Text += Environment.NewLine;
        }
    }
}