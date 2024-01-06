using System.Globalization;

namespace LoanBusinessManagerUI;

public partial class App : Application
{
    public App(AppShellViewModel viewModel)
    {
        InitializeComponent();

        MainPage = new AppShell(viewModel);
    }

    protected override void OnStart()
    {
        CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("pt-BR");
        CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("pt-BR");
    }
}
