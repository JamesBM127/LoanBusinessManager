namespace LoanBusinessManagerUI.View;

public partial class PersonPage : ContentPage
{
    private readonly PersonViewModel _viewModel;
    private bool toLoan, toPay, loansSelected, paymentsSelected, save = false;

    public PersonPage(PersonViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
    }

    #region Menu
    private void toLoanButton_Clicked(object sender, EventArgs e)
    {
        SetButtonClicked(ref toLoanButton, ref toLoanBody);
        FooterRightButtonSet("Salvar", "#303030", true);
        toLoan = save = true;
    }

    private void toPayButton_Clicked(object sender, EventArgs e)
    {
        SetButtonClicked(ref toPayButton, ref toPayBody);
        FooterRightButtonSet("Salvar", "#303030", true);
        toPay = true;
    }

    private void loansButton_Clicked(object sender, EventArgs e)
    {
        SetButtonClicked(ref loansButton, ref historicBody);
        FooterRightButtonSet("Pesquisar", "#4300FF", false);
        loansSelected = true;
    }

    private void paymentsButton_Clicked(object sender, EventArgs e)
    {
        SetButtonClicked(ref paymentsButton, ref historicBody);
        FooterRightButtonSet("Pesquisar", "#4300FF", false);
        paymentsSelected = true;
    }

    private void SetAllButtonsUnclicked()
    {
        toLoanButton.BackgroundColor =
        toPayButton.BackgroundColor =
        loansButton.BackgroundColor =
        paymentsButton.BackgroundColor = Color.FromArgb("#2e2e2e");

        toLoanBody.IsVisible =
        toPayBody.IsVisible =
        historicBody.IsVisible = false;

        toPay =
        toLoan =
        loansSelected =
        paymentsSelected = false;
    }

    private void SetButtonClicked(ref Button button, ref StackLayout layout)
    {
        SetAllButtonsUnclicked();
        button.BackgroundColor = Color.FromArgb("#4300FF");
        layout.IsVisible = true;
    }

    private void FooterRightButtonSet(string text, string hexColor, bool saveOption)
    {
        saveFindButton.Text = text;
        saveFindButton.BackgroundColor = Color.FromArgb(hexColor);
        save = saveOption;
    }
    #endregion

    #region ToLoan
    //private void DayOfTheMonth_TextChanged(object sender, TextChangedEventArgs e)
    //{
    //    if (int.TryParse(e.NewTextValue, out int value))
    //    {
    //        if (value == 0)
    //        {
    //            if (DateTime.Today.Day == 31)
    //                dueDate.Text = "1";
    //            else
    //                dueDate.Text = DateTime.Today.Day.ToString();
    //            return;
    //        }

    //        if (value < 1 || value > 30)
    //        {
    //            errorFrame.IsVisible = true;
    //            dueDate.Text = dueDate.Text[0].ToString();
    //            errorLabel.Text = "Apenas números entre 1 e 30 são permitidos";
    //            return;
    //        }
    //    }
    //    else
    //    {
    //        if (e.NewTextValue != "")
    //        {
    //            errorFrame.IsVisible = true;
    //            errorLabel.Text = "Apenas números entre 1 e 30 são permitidos";
    //            dueDate.Text = DateTime.Today.Day.ToString();
    //        }
    //        return;
    //    }
    //}

    private void Interest_TextChanged(object sender, TextChangedEventArgs e)
    {
        interestLoan.Text = ConfigSettings.InterestTextChanged(e);
    }

    private void AmountLoan_TextChanged(object sender, TextChangedEventArgs e)
    {
        amountLoan.Text = MoneyFormat(e);
        amountLoan.CursorPosition = amountLoan.Text.Length;
    }

    #endregion

    #region ToPay
    private void AmountPay_TextChanged(object sender, TextChangedEventArgs e)
    {
        amountPay.Text = MoneyFormat(e);
        amountPay.CursorPosition = amountPay.Text.Length;
    }

    #endregion

    private async void GoToWhatsappButton_ClickedAsync(object sender, EventArgs e)
    {
        if (DeviceInfo.Platform == DevicePlatform.WinUI)
        {
            await Shell.Current.DisplayAlert("Aviso", "Recurso disponível apenas no celular", "Ok");
            return;
        }

        Button buttonClicked = (Button)sender;
        Phone phone = (Phone)buttonClicked.BindingContext;
        _viewModel.GoToWhatsappChat(phone.PhoneNumber).Wait();
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

    private async void SaveFindButton_Clicked(object sender, EventArgs e)
    {
        if (toLoan)
            await _viewModel.CreateLoan();

        else if (toPay)
            await _viewModel.CreatePayment();

        else if (loansSelected)
            await _viewModel.ListLoans();

        else if (paymentsSelected)
            await _viewModel.ListPayments();
    }

    private async void RefreshClicked(object sender, EventArgs e)
    {
        atualizarButton.BackgroundColor = Color.FromArgb("#ffff00");
        await refreshImage.RotateTo(360, 1000);
        refreshImage.Rotation = 0;
        await Task.Delay(180);
        atualizarButton.BackgroundColor = Color.FromArgb("#00bfff");
    }
}