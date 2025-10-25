using Npgsql;

namespace Sinav_Takvim;

public class ListeYukleStatusDAO
{
    public bool SetDersListesi(Bolum bolum, bool status)
    {
        try
        {
            using (var conn = DbConnection.Connect())
            {
                conn.Open();
                using (var sqlCMD = new NpgsqlCommand($"UPDATE listeyuklestatus SET derslistesi = '{status}' " +
                                                      $"WHERE bolum = '{bolum}'::rol", conn))
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
    
    public bool SetOgrenciListesi(Bolum bolum, bool status)
    {
        try
        {
            using (var conn = DbConnection.Connect())
            {
                conn.Open();
                using (var sqlCMD = new NpgsqlCommand($"UPDATE listeyuklestatus SET ogrencilistesi = '{status}' " +
                                                      $"WHERE bolum = '{bolum}'::rol", conn))
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

    public bool GetDersListesi(Bolum bolum)
    {
        try
        {
            using (var conn = DbConnection.Connect())
            {
                conn.Open();
                using(var sqlCMD = new NpgsqlCommand($"SELECT l.derslistesi FROM listeyuklestatus l " + 
                                                     $"WHERE l.bolum = '{bolum}'::rol", conn))
                using (var reader = sqlCMD.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return reader.GetBoolean(0);
                    }
                }
                
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        return false;
    }
    
    public bool GetOgrenciListesi(Bolum bolum)
    {
        try
        {
            using (var conn = DbConnection.Connect())
            {
                conn.Open();
                using(var sqlCMD = new NpgsqlCommand($"SELECT l.ogrencilistesi FROM listeyuklestatus l " +
                                                     $"WHERE l.bolum = '{bolum}'::rol", conn))
                using (var reader = sqlCMD.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return reader.GetBoolean(0);
                    }
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