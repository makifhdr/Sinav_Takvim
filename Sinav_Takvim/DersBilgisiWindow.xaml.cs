namespace Sinav_Takvim;

public partial class DersBilgisiWindow
{
    private Ders ders;
    private Bolum bolum;
    
    private OgrenciDersDAO ogrenciDersDAO = new();
    
    public DersBilgisiWindow(Ders ders, Bolum bolum)
    {
        this.ders = ders;
        this.bolum = bolum;
        InitializeComponent();
        DersOgrenciYaz();
    }
    
    private void DersOgrenciYaz()
    {
        int sayi = 0;
        var ogrenciList = ogrenciDersDAO.GetDersOgrenciler(ders.GetKod(), bolum);

        AdTextBlock.Text += $"({ogrenciList.Count})";

        foreach (var ogrenci in ogrenciList)
        {
            switch (sayi)
            {
                case < 34:
                    OgrenciListesiBlock1.Text += ogrenci.GetNumara() + " - "+ ogrenci.GetAd() + Environment.NewLine;
                    break;
                case < 68:
                    OgrenciListesiBlock2.Text += ogrenci.GetNumara() + " - "+ ogrenci.GetAd() + Environment.NewLine;
                    break;
                case < 102:
                    OgrenciListesiBlock3.Text += ogrenci.GetNumara() + " - "+ ogrenci.GetAd() + Environment.NewLine;
                    break;
                case > 102:
                    OgrenciListesiBlock4.Text += ogrenci.GetNumara() + " - "+ ogrenci.GetAd() + Environment.NewLine;
                    break;
            }

            sayi++;
        }
        
    }
}