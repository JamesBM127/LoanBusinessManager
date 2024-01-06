using JBM.DeserializeJson;
using Microsoft.Extensions.Configuration;

namespace LoanBusinessManagerUI.ConnectionString
{
    public class ConnectionStringSettings
    {
        public static string GetSqliteConnectionString(string dbName, string dbExtension = ".db3", string dbPath = null)
        {
            if (!string.IsNullOrEmpty(dbName) && !string.IsNullOrEmpty(dbExtension))
            {
                if (string.IsNullOrWhiteSpace(dbPath))
                    dbPath = GetDatabasePath();
            }
            else
                throw new ArgumentException("Invalid Database name or Database extension");

            return $"Data Source={Path.Combine(dbPath, (dbName + dbExtension))};";
        }

        public static string GetSqliteConnectionString(IConfiguration configuration)
        {
            SqliteConfig dbConfig = configuration.ToCSharp<SqliteConfig>("SqliteConfig");
            dbConfig.Path = GetDatabasePath();
            return $"Data Source={Path.Combine(dbConfig.Path, (dbConfig.DbName + dbConfig.ExtensionType))};";
        }

        public static string GetSqliteConnectionString(ISqliteConfig sqliteConfig)
        {
            sqliteConfig.Path = GetDatabasePath();
            return $"Data Source={Path.Combine(sqliteConfig.Path, (sqliteConfig.DbName + sqliteConfig.ExtensionType))};";
        }

        public static string GetDatabasePath()
        {
            return FileSystem.AppDataDirectory;
        }

        public static string GetSqliteConnectionStringRawTest()
        {
            string path = GetDatabasePath();
            string connectionString = $"Data Source={Path.Combine(path, ("jbmdb" + ".db3"))};";
            return connectionString;
        }
    }
}
