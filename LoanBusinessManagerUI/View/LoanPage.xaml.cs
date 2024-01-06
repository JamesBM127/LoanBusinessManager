namespace LoanBusinessManagerUI.View;

public partial class LoanPage : ContentPage
{
    public LoanPage(LoanViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private void descriptionHistoryFrame_BindingContextChanged(object sender, EventArgs e)
    {
        ConfigSettings.SetFrameColorsFromTables<Person>(sender);
    }
}