using Npgsql;

namespace Sinav_Takvim;

public class OgrenciDAO
{
    public bool OgrenciEkle(Ogrenci ogrenci, Bolum bolum)
    {
        if (ogrenci.GetAd().Equals("") || ogrenci.GetNumara().Equals("") || ogrenci.GetSinif() == -1)
        {
            return false;
        }
        
        try
        {
            using (var conn = DbConnection.Connect())
            {
                conn.Open();
                using (var sqlCMD = new NpgsqlCommand($"INSERT INTO ogrenci (numara, isim, sinif, bolum) " +
                                                      $"VALUES ('{ogrenci.GetNumara()}', '{ogrenci.GetAd()}', " +
                                                      $"'{ogrenci.GetSinif()}', '{bolum}'::rol)", conn))
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
    
    public bool OgrencilerSil(Bolum bolum)
    {
        try
        {
            using (var conn = DbConnection.Connect())
            {
                conn.Open();
                using (var sqlCMD = new NpgsqlCommand($"DELETE FROM ogrenci WHERE bolum = '{bolum}'::rol", conn))
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

    public List<Ogrenci> GetOgrenciListesi(Bolum bolum)
    {
        List<Ogrenci> list = [];
        try
        {
            using (var conn = DbConnection.Connect())
            {
                conn.Open();
                using(var sqlCMD = new NpgsqlCommand($"SELECT o.* FROM ogrenci o " +
                                                     $"WHERE o.bolum = '{bolum}'::rol", conn))
                using (var reader = sqlCMD.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var ogrenci = new Ogrenci(reader.GetString(0), reader.GetString(1), 
                            reader.GetInt16(2));
                        list.Add(ogrenci);
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
    
    public Ogrenci GetOgrenci(string numara, Bolum bolum)
    {
        var ogrenci = new Ogrenci();
        
        try
        {
            using (var conn = DbConnection.Connect())
            {
                conn.Open();
                using(var sqlCMD = new NpgsqlCommand($"SELECT o.* FROM ogrenci o " +
                                                     $"WHERE o.numara = '{numara}' AND o.bolum = '{bolum}'::rol", conn))
                using (var reader = sqlCMD.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ogrenci = new Ogrenci(reader.GetString(0), reader.GetString(1), 
                            reader.GetInt16(2));
                    }
                }
                
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        return ogrenci;
    }
}