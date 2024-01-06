namespace LoanBusinessManagerUI;

public partial class AppShell : Shell
{
    public AppShell(AppShellViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
        Routing.RegisterRoute(nameof(LoanPage), typeof(LoanPage));
        Routing.RegisterRoute(nameof(PersonPage), typeof(PersonPage));
        Routing.RegisterRoute(nameof(CreatePersonPage), typeof(CreatePersonPage));
        Routing.RegisterRoute(nameof(LoansListPage), typeof(LoansListPage));
        Routing.RegisterRoute(nameof(EditLoanPage), typeof(EditLoanPage));
        Routing.RegisterRoute(nameof(EditPaymentPage), typeof(EditPaymentPage));
        Routing.RegisterRoute(nameof(EditPersonPage), typeof(EditPersonPage));
        Routing.RegisterRoute(nameof(PaymentsListPage), typeof(PaymentsListPage));
        Routing.RegisterRoute(nameof(OptionsPage), typeof(OptionsPage));
    }
}
