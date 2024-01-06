namespace LoanBusinessManagerUI.View.EditPage;

public partial class EditPersonPage : ContentPage
{
    public EditPersonPage(PersonViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}