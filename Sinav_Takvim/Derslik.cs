namespace Sinav_Takvim;

public class Derslik
{
    private string kod;
    private string ad;
    private int satir;
    private int sutun;
    private int sira_yapisi;
    private int kapasite;
    private Bolum bolum;


    public Derslik()
    {
        kod = "";
        ad = "";
        satir = 0;
        sutun = 0;
        sira_yapisi = 0;
        kapasite = 0;
        bolum = Bolum.Null;
    }
    public Derslik(string kod, string ad, int satir, int sutun, int sira_yapisi, int kapasite, Bolum bolum)
    {
        this.kod = kod;
        this.ad = ad;
        this.satir = satir;
        this.sutun = sutun;
        this.sira_yapisi = sira_yapisi;
        this.kapasite = kapasite;
        this.bolum = bolum;
    }
    
    public string GetKod() { return kod; }
    
    public string GetAd() { return ad; }
    
    public int GetSatir() { return satir; }
    
    public int GetSutun() { return sutun; }
    
    public int GetSira_yapisi() { return sira_yapisi; }
    
    public int GetKapasite() { return kapasite; }
    
    public Bolum GetBolum() { return bolum; }
}