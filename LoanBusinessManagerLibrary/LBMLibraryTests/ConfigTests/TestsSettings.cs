using FBMLibrary.Data;
using FBMLibrary.Entity;
using FBMLibrary.Enums;
using JBMDatabase;
using JBMDatabase.ConnectionString;
using JBMDatabase.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FBMLibraryTests.ConfigTests
{
    public class TestsSettings
    {
        public static async Task<FBMContext> GetContext(DatabaseOptions databaseOptions = DatabaseOptions.InMemoryDatabase, string jsonSection = "CreateDbTest")
        {
            IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            string connectionString = ConnectionStringSettings.CreateConnectionString(configuration, jsonSection);

            DbContextOptions<FBMContext> options = ConfigDbContextOptions.GetDbContextOptions<FBMContext>(databaseOptions, connectionString);

            return new FBMContext(options);
        }

        public static Person GetPerson(string name = "Pessoa 1", string nickname = "Apelido 1", PaymentStatus status = PaymentStatus.Pendente)
        {
            return new Person()
            {
                Name = name,
                Nickname = nickname,
                PaymentStatus = status
            };
        }
    }
}
