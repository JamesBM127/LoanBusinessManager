namespace LoanBusinessManagerUI.View;

public partial class PaymentsListPage : ContentPage
{
    public PaymentsListPage(PaymentViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private void paymentFrame_BindingContextChanged(object sender, EventArgs e)
    {
        ConfigSettings.SetFrameColorsFromTables<Payment>(sender);
    }

    //Even = Par
    //Odd = Ímpar
    private bool IsEven(int counter)
    {
        if (counter % 2 == 0)
            return true;
        else
            return false;
    }
}