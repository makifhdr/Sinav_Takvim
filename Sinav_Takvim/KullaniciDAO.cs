using Npgsql;

namespace Sinav_Takvim;

public class KullaniciDAO
{
    public Bolum KullaniciGiris(string eposta, string sifre)
    {
        try
        {
            using (var conn = DbConnection.Connect())
            {
                conn.Open();
                using(var sqlCMD = new NpgsqlCommand($"SELECT k.* FROM kullanici k", conn))
                using (var reader = sqlCMD.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader.GetString(0) == eposta && reader.GetString(1) == sifre)
                        {
                            return Enum.Parse<Bolum>(reader.GetString(2));
                        }
                    }
                }
                
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        return Bolum.Null;
    }

    public bool KullaniciEkle(string eposta, string sifre, Bolum bolum)
    {
        try
        {
            using (var conn = DbConnection.Connect())
            {
                conn.Open();
                using (var sqlCMD = new NpgsqlCommand($"INSERT INTO kullanici (eposta, sifre, rol) " +
                                                      $"VALUES ('{eposta}', '{sifre}', '{bolum}'::rol)", conn))
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
}