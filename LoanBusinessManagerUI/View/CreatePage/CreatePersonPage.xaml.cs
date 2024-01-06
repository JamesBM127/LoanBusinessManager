namespace LoanBusinessManagerUI.View.CreatePage
{
    public partial class CreatePersonPage : ContentPage
    {
        public CreatePersonPage(PersonViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}