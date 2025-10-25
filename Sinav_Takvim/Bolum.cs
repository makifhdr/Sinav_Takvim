namespace Sinav_Takvim;

public enum Bolum
{
    Admin,
    Bilgisayar,
    Yazilim,
    Elektrik,
    Elektronik,
    Insaat,
    Null,
}

public static class BolumHelper
{
    public static string GetIsim(Bolum bolum)
    {
        return bolum switch
        {
            Bolum.Admin => "Admin",
            Bolum.Bilgisayar => "Bilgisayar Mühendisliği",
            Bolum.Yazilim => "Yazılım Mühendisliği",
            Bolum.Elektrik => "Elektrik Mühendisliği",
            Bolum.Elektronik => "Elektronik Mühendisliği",
            Bolum.Insaat => "İnşaat Mühendisliği",
            _ => nameof(Bolum.Null)
        };
    }
}