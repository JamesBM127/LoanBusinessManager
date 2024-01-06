namespace LoanBusinessManagerUI.View;

public partial class LoansListPage : ContentPage
{
    public LoansListPage(LoanViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private void loanFrame_BindingContextChanged(object sender, EventArgs e)
    {
        ConfigSettings.SetFrameColorsFromTables<Loan>(sender);
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