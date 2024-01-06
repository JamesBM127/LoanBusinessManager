namespace LoanBusinessManagerUI.View.EditPage;

public partial class EditLoanPage : ContentPage
{
    public EditLoanPage(LoanViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;

    }

    protected override void OnAppearing()
    {

    }

    private void AmountLoan_TextChanged(object sender, TextChangedEventArgs e)
    {
        amountLoan.Text = MoneyFormat(e);
        amountLoan.CursorPosition = amountLoan.Text.Length;
    }

    private string MoneyFormat(TextChangedEventArgs e)
    {
        if (decimal.TryParse(e.NewTextValue, out decimal value))
        {
            return ConfigSettings.FormatMoneyValue(e.NewTextValue);
        }

        else
            return e.OldTextValue;
    }

    private void Interest_TextChanged(object sender, TextChangedEventArgs e)
    {
        interestLoan.Text = ConfigSettings.InterestTextChanged(e);
    }
}