namespace LoanBusinessManagerUI.ViewModel;

public partial class GlobalNavigationViewModel : BaseViewModel
{
    public GlobalNavigationViewModel()
    {
    }

    [RelayCommand]
    async Task GoToOptionsPage()
    {
        try
        {
            CloseAllFlyoutPlataformButWinUI();
            RemovePage(Shell.Current.CurrentPage);
            await Shell.Current.GoToAsync($"{nameof(OptionsPage)}", true);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    [RelayCommand]
    async Task GoToLoanPage()
    {
        try
        {
            CloseAllFlyoutPlataformButWinUI();
            RemovePage(Shell.Current.CurrentPage);
            await Shell.Current.GoToAsync($"{nameof(LoanPage)}", true);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    [RelayCommand]
    async Task GoToCreatePersonPage()
    {
        try
        {
            CloseAllFlyoutPlataformButWinUI();
            RemovePage(Shell.Current.CurrentPage);
            await Shell.Current.GoToAsync($"{nameof(CreatePersonPage)}", true);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    //[RelayCommand]
    //async Task GoToRentPage()
    //{
    //    try
    //    {
    //        CloseAllFlyoutPlataformButWinUI();
    //        RemovePage(Shell.Current.CurrentPage);
    //        await Shell.Current.GoToAsync($"{nameof(RentPage)}", true);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw;
    //    }
    //}

    //[RelayCommand]
    //async Task GoToCreateHousePage()
    //{
    //    try
    //    {
    //        CloseAllFlyoutPlataformButWinUI();
    //        RemovePage(Shell.Current.CurrentPage);
    //        await Shell.Current.GoToAsync($"{nameof(CreateHousePage)}", true);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw;
    //    }
    //}
}
