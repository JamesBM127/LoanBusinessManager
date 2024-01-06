namespace LoanBusinessManagerUI.View.EditPage;

public partial class EditPaymentPage : ContentPage
{
    public EditPaymentPage(PaymentViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private void AmountPay_TextChanged(object sender, TextChangedEventArgs e)
    {
        amountPay.Text = MoneyFormat(e);
        amountPay.CursorPosition = amountPay.Text.Length;
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
}