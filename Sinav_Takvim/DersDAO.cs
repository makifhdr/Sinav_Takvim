using Npgsql;

namespace Sinav_Takvim;

public class DersDAO
{
    public bool DersEkle(Ders ders, Bolum bolum)
    {
        try
        {
            using (var conn = DbConnection.Connect())
            {
                conn.Open();
                using (var sqlCMD = new NpgsqlCommand($"INSERT INTO ders (kod, isim, ogretmen, sinif, " +
                                                      $"                          secmeli, sinav, bolum) " +
                                                      $"VALUES ('{ders.GetKod()}', '{ders.GetAd()}', " +
                                                      $"'{ders.GetOgretmen()}', '{ders.GetSinif()}'," +
                                                      $"'{ders.GetSecmeli()}'::bool, '{ders.GetSinav()}'::bool," +
                                                      $"'{bolum.ToString()}'::rol)", conn))
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

    public bool DerslerDuzenle(Bolum bolum, bool sinav)
    {
        try
        {
            using (var conn = DbConnection.Connect())
            {
                conn.Open();
                using (var sqlCMD = new NpgsqlCommand($"UPDATE ders d SET d.sinav = '{sinav}' " +
                                                      $"WHERE d.bolum = '{bolum}'::rol", conn))
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

    public bool DerslerSil(Bolum bolum)
    {
        try
        {
            using (var conn = DbConnection.Connect())
            {
                conn.Open();
                using (var sqlCMD = new NpgsqlCommand($"DELETE FROM ders WHERE bolum = '{bolum}'::rol", conn))
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

    public Ders GetDers(string dersKodu, Bolum bolum)
    {
        var ders = new Ders();
        try
        {
            using (var conn = DbConnection.Connect())
            {
                conn.Open();
                using(var sqlCMD = new NpgsqlCommand($"SELECT d.* FROM ders d " +
                                                     $"WHERE d.kod = '{dersKodu}' AND d.bolum = '{bolum}'::rol", conn))
                using (var reader = sqlCMD.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ders = new Ders(reader.GetString(0), reader.GetString(1),
                            reader.GetString(2), reader.GetInt32(3),reader.GetBoolean(4));
                    }
                }
                
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        return ders;
    }

    public int GetSinifDersSayisi(int sinif, Bolum bolum)
    {
        var sayi = 0;
        try
        {
            using (var conn = DbConnection.Connect())
            {
                conn.Open();
                using(var sqlCMD = new NpgsqlCommand($"SELECT d.* FROM ders d " +
                                                     $"WHERE d.sinif = '{sinif}' AND d.bolum = '{bolum}'::rol", conn))
                using (var reader = sqlCMD.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        sayi++;
                    }
                }
                
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        return sayi;
    }

    public List<Ders> GetDersler(Bolum bolum)
    {
        List<Ders> list = [];
        try
        {
            using (var conn = DbConnection.Connect())
            {
                conn.Open();
                using(var sqlCMD = new NpgsqlCommand($"SELECT d.* FROM ders d " +
                                                     $"WHERE d.bolum = '{bolum}'::rol", conn))
                using (var reader = sqlCMD.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Ders(reader.GetString(0), reader.GetString(1),
                            reader.GetString(2), reader.GetInt32(3),reader.GetBoolean(4)));
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