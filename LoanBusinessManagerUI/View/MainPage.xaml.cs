namespace LoanBusinessManagerUI;

public partial class MainPage : ContentPage
{
    public MainPage(MainPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;

        viewModel.GetDescriptionsAsync().Wait();
        //viewModel.SortDescriptionHistory();
        //viewModel.ListDescriptionHistory();
    }

    private void descriptionHistoryFrame_BindingContextChanged(object sender, EventArgs e)
    {
        ConfigSettings.SetFrameColorsFromTables<DescriptionHistory>(sender);
    }
}

