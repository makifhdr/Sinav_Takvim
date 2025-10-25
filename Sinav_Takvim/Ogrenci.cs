namespace Sinav_Takvim;

public class Ogrenci
{
    private string Numara;
    private string Ad;
    private int Sinif;

    public Ogrenci()
    {
        Numara = "";
        Ad = "";
        Sinif = 0;
    }
    
    public Ogrenci(string numara, string ad, int sinif)
    {
        Numara = numara;
        Ad = ad;
        Sinif = sinif;
    }
    
    public string GetNumara() { return Numara; }
    
    public string GetAd() { return Ad; }
    
    public int GetSinif() { return Sinif; }

    public string GetFormatliAd()
    {
        return Ad.Substring(0, Ad.IndexOf(" ")) + "\n" + Ad.Substring(Ad.IndexOf(" "));
    }
}