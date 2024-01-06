using JBM.DeserializeJson;
using JBMDatabase;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.IO;

namespace LoanBusinessManagerUI;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        MauiAppBuilder builder = MauiApp.CreateBuilder();

        IS3Config s3Config = null;
        ISqliteConfig sqliteConfig = null;

        try
        {
            string connectionString = string.Empty;

            //Before use, first configure the appsettings of your destination OS.
            //appsettingsWinUi if you gonna use on Windows.
            //appsettingsAndroid if gonna use in Android.
            //Or both if gonna use both.
            //iOS and MacOS not tested.
#if WINDOWS
                        builder.Configuration.AddJsonFile("appsettingsWinUi.json").Build();
#endif

#if ANDROID
                        var assets = Android.App.Application.Context.Assets;
                        using (Stream streamReader = assets.Open("appsettingsAndroid.json"))
                        {
                            builder.Configuration.AddJsonStream(streamReader);
                        }
#endif

            s3Config = builder.Configuration.ToCSharp<S3Config>("S3Config");
            sqliteConfig = builder.Configuration.ToCSharp<SqliteConfig>("SqliteConfig");
            sqliteConfig.Path = ConnectionStringSettings.GetDatabasePath();

            //When InMemoryDatabase is used, the "connectionString" became the name of DB, not a real connection string.
            connectionString = ConnectionStringSettings.GetSqliteConnectionString(sqliteConfig);
            builder.Services.EnsureCreateAsync<LBMContext>(connectionString, DatabaseOptions.InMemoryDatabase);
        }
        catch (Exception ex)
        {
            AppShell.Current.DisplayAlert("Erro", ex.Message, "Ok");
            Environment.Exit(0);
        }

        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        builder.Services.InjectDependency(s3Config, sqliteConfig);

        return builder.Build();
    }

    private static S3Config GetS3Config()
    {
        return new S3Config
        {
            AwsAccessKeyId = "AKIA3BKK7I6BKV4MJJET",
            AwsSecretAccessKey = "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",
            BucketName = "jbm-database-sqlite-bucket",
            BucketUrl = "https://jbm-database-sqlite-bucket.s3.sa-east-1.amazonaws.com/",
            RegionEndpoint = "sa-east-1"
        };
    }
}
