using JbmAwsBucket;
using LoanBusinessManagerUI.JBMException;
using Microsoft.EntityFrameworkCore;

namespace LoanBusinessManagerUI.ViewModel
{
    public partial class OptionsViewModel : BaseViewModel
    {
        private readonly IS3Config _s3Config;
        private readonly ISqliteConfig _sqliteConfig;
        private readonly IS3Manager _s3Manager;
        private readonly LBMContext _fbmContext;

        public OptionsViewModel(IS3Config s3Config,
                                 ISqliteConfig sqliteConfig,
                                 LBMContext fbmContext)
        {
            _s3Config = s3Config;
            _sqliteConfig = sqliteConfig;
            _s3Manager = new S3Manager(s3Config);
            _fbmContext = fbmContext;
        }

        #region Database
        [RelayCommand]
        async Task DownloadDatabaseFromAws()
        {
            bool keepToDownload = await Shell.Current.DisplayAlert(
                 title: "Confirmação",
                 message: "Fazer o download do banco de dados acarreta na perda de dados não salvos, deseja continuar?",
                 accept: "SIM",
                 cancel: "CANCELAR");

            if (keepToDownload)
            {
                DownloadRequest downloadRequest = new()
                {
                    BucketName = _s3Config.BucketName,
                    Key = _sqliteConfig.DbName + _sqliteConfig.ExtensionType,
                    DirectoryPath = ConnectionStringSettings.GetDatabasePath()
                };

                try
                {
                    bool internetConnected = InternetIsConnected();

                    if (internetConnected)
                    {
                        await _fbmContext.Database.EnsureDeletedAsync();
                        await _s3Manager.DownloadAsync(downloadRequest);

                        await Shell.Current.DisplayAlert(title: "Sucesso",
                                                         message: "Download com sucesso, desligamento automático do aplicativo para concluir operação!",
                                                         cancel: "Ok");

                        Environment.Exit(0);
                    }
                }
                catch (NoInternetConnectionException ex)
                {
                    await Shell.Current.DisplayAlert(title: "Falha",
                                                     message: ex.Message,
                                                     cancel: "Ok");
                }
                catch (Exception ex)
                {
                    await Shell.Current.DisplayAlert(title: "Falha",
                                                     message: $"ERRO no download: [{ex.Message}]",
                                                     cancel: "Ok");
                }
            }
        }

        [RelayCommand]
        async Task UploadDatabaseToAws()
        {
            bool keepToUpload = await Shell.Current.DisplayAlert(title: "Confirmação",
                                                                 message: "Deseja continuar?",
                                                                 accept: "SIM",
                                                                 cancel: "CANCELAR");

            if (keepToUpload)
            {
                FileInfo fileInfo = new FileInfo(Path.Combine(ConnectionStringSettings.GetDatabasePath(), (_sqliteConfig.DbName + _sqliteConfig.ExtensionType)));

                UploadRequest uploadRequest = new()
                {
                    FileInfo = fileInfo,
                };

                try
                {
                    bool internetConnected = InternetIsConnected();

                    if (internetConnected)
                    {
                        await EnsureSqliteSaveDataOnMainDb3File();
                        await _s3Manager.UploadAsync(uploadRequest);

                        await Shell.Current.DisplayAlert(title: "Sucesso",
                                                         message: "Salvo com sucesso!",
                                                         cancel: "Ok");
                    }
                }
                catch (NoInternetConnectionException ex)
                {
                    await Shell.Current.DisplayAlert(title: "Falha",
                                                     message: ex.Message,
                                                     cancel: "Ok");
                }
                catch (Exception ex)
                {
                    await Shell.Current.DisplayAlert(title: "Falha",
                                                     message: $"ERRO no salvamento: [{ex.Message}]",
                                                     cancel: "Ok");
                }
            }
        }

        [RelayCommand]
        async Task ShutdownApp()
        {
            bool shutdown = await Shell.Current.DisplayAlert("Confirmação", "DESEJA FECHAR O APP?", "Ok", "Cancelar");

            if (!shutdown)
            {
                return;
            }

            try
            {
                if (DeviceInfo.Platform == DevicePlatform.Android)
                {
#if ANDROID
                    Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
#endif
                }
                else if (DeviceInfo.Platform == DevicePlatform.WinUI)
                {
                    Environment.Exit(0);
                }
                else
                {
                    throw new Exception("Sistema Operacional não reconhecido");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("ERRO!", $"Feche o APP manualmente: {ex.Message}", "OK");
            }
        }

        /// <summary>
        /// Force the SQLite save all no saved data.
        /// </summary>
        /// <returns></returns>
        private async Task EnsureSqliteSaveDataOnMainDb3File()
        {
            await _fbmContext.Database.ExecuteSqlRawAsync("PRAGMA wal_checkpoint(FULL)");
        }
        #endregion
    }
}
