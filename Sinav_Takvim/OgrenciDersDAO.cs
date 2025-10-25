using Npgsql;

namespace Sinav_Takvim;

public class OgrenciDersDAO
{
    private DersDAO dersDAO = new();
    private OgrenciDAO ogrenciDAO = new();
    
    public bool OgrenciDersEkle(string ogrenci_numara, string ders_kodu, Bolum bolum)
    {
        try
        {
            using (var conn = DbConnection.Connect())
            {
                conn.Open();
                using (var sqlCMD = new NpgsqlCommand($"INSERT INTO ogrenciders (ogrenci_numara, ders_kodu, bolum) " +
                                                      $"VALUES ('{ogrenci_numara}', '{ders_kodu}','{bolum}'::rol) ", conn))
                {
                    sqlCMD.ExecuteNonQuery();
                    return true;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return false;
    }

    public bool OgrenciDerslerSil(Bolum bolum)
    {
        try
        {
            using (var conn = DbConnection.Connect())
            {
                conn.Open();
                using (var sqlCMD = new NpgsqlCommand($"DELETE FROM ogrenciders WHERE bolum = '{bolum}'::rol", conn))
                {
                    sqlCMD.ExecuteNonQuery();
                    return true;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return false;
    }

    public List<Ders> GetOgrenciDersler(string ogrenci_numara, Bolum bolum)
    {
        List<Ders> list = [];
        try
        {
            using (var conn = DbConnection.Connect())
            {
                conn.Open();
                using(var sqlCMD = new NpgsqlCommand($"SELECT o.* FROM ogrenciders o " +
                                                     $"WHERE o.ogrenci_numara = '{ogrenci_numara}'", conn))
                using (var reader = sqlCMD.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var dersKodu = reader.GetString(1);
                        
                        list.Add(dersDAO.GetDers(dersKodu, bolum));
                    }
                }
                
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        return list;
    }

    public List<Ogrenci> GetDersOgrenciler(string ders_kodu, Bolum bolum)
    {
        List<Ogrenci> list = [];
        try
        {
            using (var conn = DbConnection.Connect())
            {
                conn.Open();
                using(var sqlCMD = new NpgsqlCommand($"SELECT o.* FROM ogrenciders o " +
                                                     $"WHERE o.ders_kodu = '{ders_kodu}' AND o.bolum = '{bolum}'::rol", conn))
                using (var reader = sqlCMD.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var ogrenci_numara = reader.GetString(0);
                        
                        list.Add(ogrenciDAO.GetOgrenci(ogrenci_numara, bolum));
                    }
                }
                
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        return list;
    }
}