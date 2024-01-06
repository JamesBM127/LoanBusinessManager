namespace LoanBusinessManagerUI.View;

public partial class OptionsPage : ContentPage
{
    public OptionsPage(OptionsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}