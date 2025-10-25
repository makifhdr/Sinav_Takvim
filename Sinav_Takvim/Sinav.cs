namespace Sinav_Takvim;

public class Sinav
{
    private DateTime tarih;
    private TimeSpan saat;
    private int sure;
    private Ders ders;
    private List<Derslik> derslikler;

    public Sinav()
    {
        tarih = DateTime.MinValue;
        saat = TimeSpan.Zero;
        sure = 0;
        ders = new Ders();
        derslikler = [];
    }
    
    public Sinav(DateTime tarih, TimeSpan saat, int sure, Ders ders)
    {
        this.tarih = tarih;
        this.saat = saat;
        this.sure = sure;
        this.ders = ders;
        derslikler = [];
    }
    
    public DateTime GetTarih(){ return tarih; }
    
    public TimeSpan GetSaat(){ return saat; }
    
    public int Getsure(){ return sure; }
    
    public Ders GetDers(){ return ders; }
    
    public List<Derslik> GetDerslikler(){ return derslikler;}

    public void AddDerslikler(List<Derslik> derslikler) { this.derslikler.AddRange(derslikler); }
}