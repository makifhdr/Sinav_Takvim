namespace Sinav_Takvim;

public class Ders
{
    private string Kod;
    private string Ad;
    private string Ogretmen;
    private int Sinif;
    private bool Secmeli;
    private bool Sinav;

    public Ders()
    {
        Kod  = "";
        Ad = "";
        Ogretmen = "";
        Sinif = 0;
        Secmeli = false;
        Sinav = false;
    }

    public Ders(string kod, string ad, string ogretmen, int sinif, bool secmeli)
    {
        Kod = kod;
        Ad = ad;
        Ogretmen = ogretmen;
        Sinif = sinif;
        Secmeli = secmeli;
        Sinav = false;
    }

    public string GetKod() { return Kod; }

    public string GetAd() { return Ad; }

    public string GetOgretmen() { return Ogretmen; }

    public int GetSinif() { return Sinif; }
    
    public bool GetSecmeli() { return Secmeli; }
    
    public bool GetSinav() { return Sinav; }
    
    public void SetSinav(bool sinav) { Sinav = sinav; }
}