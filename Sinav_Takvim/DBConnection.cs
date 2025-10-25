using Npgsql;

namespace Sinav_Takvim;

public static class DbConnection
{
    public static NpgsqlConnection Connect()
    {
        const string cs = "Host=localhost;Username=postgres;Password=Makifhidir61*;Database=sinav_takvim";
        var conn = new NpgsqlConnection(cs);
        return conn;
    }
}