using Npgsql;

namespace Sinav_Takvim;

public class DerslikDAO
{
    public bool DerslikEkle(Derslik derslik)
    { 
        try 
        {
            using (var conn = DbConnection.Connect())
            {
                conn.Open();
                using (var sqlCMD = new NpgsqlCommand(
                           $"INSERT INTO derslik (kod, isim, satir, sutun, sira_yapisi, kapasite, bolum) " +
                           $"VALUES ('{derslik.GetKod()}', '{derslik.GetAd()}', " +
                           $"'{derslik.GetSatir()}', '{derslik.GetSutun()}'," +
                           $"'{derslik.GetSira_yapisi()}', '{derslik.GetKapasite()}'," +
                           $"'{derslik.GetBolum()}')", conn))
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
    
    public bool DerslikDuzenle(Derslik derslik)
    {
        try
        {
            using (var conn = DbConnection.Connect())
            {
                conn.Open();
                using (var sqlCMD = new NpgsqlCommand($"UPDATE derslik SET isim = '{derslik.GetAd()}', " +
                                                      $"satir = '{derslik.GetSatir()}', sutun = '{derslik.GetSutun()}', " +
                                                      $"sira_yapisi = '{derslik.GetSira_yapisi()}', kapasite = '{derslik.GetKapasite()}' " +
                                                      $"WHERE kod = '{derslik.GetKod()}'", conn))
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

    public void DerslikSil(string kod)
    {
        try
        {
            using (var conn = DbConnection.Connect())
            {
                conn.Open();
                using (var sqlCMD = new NpgsqlCommand($"DELETE FROM derslik WHERE kod = '{kod}'", conn))
                {
                    sqlCMD.ExecuteNonQuery();
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public List<Derslik> GetDerslikListesi(Bolum bolum)
    {
        List<Derslik> list = [];
        try
        {
            using (var conn = DbConnection.Connect())
            {
                conn.Open();
                using(var sqlCMD = new NpgsqlCommand($"SELECT d.* FROM derslik d " +
                                                     $"WHERE d.bolum = '{bolum}'::rol " +
                                                     $"ORDER BY d.kapasite DESC" , conn))
                using (var reader = sqlCMD.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var derslik = new Derslik(reader.GetString(0), reader.GetString(1), 
                                                    reader.GetInt32(2), reader.GetInt32(3),
                                                    reader.GetInt32(4), reader.GetInt32(5),
                                                    Enum.Parse<Bolum>(reader.GetString(6)));
                        list.Add(derslik);
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

    public Derslik GetDerslik(string kod, Bolum bolum)
    {
        var derslik = new Derslik();
        try
        {
            using (var conn = DbConnection.Connect())
            {
                conn.Open();
                using(var sqlCMD = new NpgsqlCommand($"SELECT d.* FROM derslik d " +
                                                     $"WHERE d.kod = '{kod}' AND d.bolum = '{bolum}'::rol", conn))
                using (var reader = sqlCMD.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        derslik = new Derslik(reader.GetString(0), reader.GetString(1), 
                            reader.GetInt32(2), reader.GetInt32(3),
                            reader.GetInt32(4), reader.GetInt32(5),
                            Enum.Parse<Bolum>(reader.GetString(6)));
                    }
                }
                
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        return derslik;
    }
    
}