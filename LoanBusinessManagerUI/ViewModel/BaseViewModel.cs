using LBMLibrary.Entity.NotMappedClasses;
using LBMLibrary.Service;
using LoanBusinessManagerUI.JBMException;

namespace LoanBusinessManagerUI.ViewModel;

public partial class BaseViewModel : ObservableObject
{
    //[NotifyPropertyChangedFor(nameof(IsNotBusy))]
    [ObservableProperty]
    bool isBusy;

    [ObservableProperty]
    public string title;

    [ObservableProperty]
    public bool isRefreshing;
    public bool IsNotBusy => !IsBusy;

    public BaseViewModel()
    {
    }

    #region Settings
    protected ObservableCollection<TEnum> GetEnumValuesList<TEnum>() where TEnum : struct
    {
        List<TEnum> tEnums = BaseService.GetEnumValues<TEnum>();
        ObservableCollection<TEnum> tEnumsReturned = new();

        foreach (TEnum item in tEnums)
        {
            tEnumsReturned.Add(item);
        }

        return tEnumsReturned;
    }

    protected void SetBaseHistoryEntity<TEntity>(ref TEntity entity, DateTime modifDate, HistoryType historyType) where TEntity : BaseHistoryEntity
    {
        entity.ModificationDate = modifDate;
        entity.HistoryType = historyType;
    }

    protected TEntity SetBaseHistoryEntity<TEntity>(TEntity entity, DateTime modifDate, HistoryType historyType) where TEntity : BaseHistoryEntity
    {
        entity.ModificationDate = modifDate;
        entity.HistoryType = historyType;

        return entity;
    }

    protected bool InternetIsConnected()
    {
        NetworkAccess currentNetwork = Connectivity.NetworkAccess;
        bool isConnected = false;

        if (currentNetwork == NetworkAccess.Internet || currentNetwork == NetworkAccess.ConstrainedInternet)
            isConnected = true;
        else
            throw new NoInternetConnectionException();

        return isConnected;
    }

    protected bool SameValueUpdate(decimal oldValueAmount, decimal newValueAmount, decimal? oldValueInterest = null, decimal? newValueInterest = null)
    {
        if (oldValueAmount != newValueAmount)
            return false;

        if (oldValueInterest != null && newValueInterest != null)
            if (oldValueInterest != newValueInterest)
                return false;

        return true;
    }

    protected async Task<bool> SaveConfirmation(string name, string nickName, decimal amount, ModificationType modificationType)
    {
        string nameDisplay = name != null ? name : nickName;
        return await Shell.Current.DisplayAlert("CONFIRMAÇÃO", $"{nameDisplay}: {modificationType} {amount.ToString("C")}", "Confirmar", "Cancelar");
    }

    protected List<TEntity> SetCounterTableList<TEntity>(List<TEntity> entities) where TEntity : NotMappedProperties
    {
        int counter = 1;
        foreach (TEntity item in entities)
        {
            item.Counter = counter++;
        }
        return entities;
    }
    #endregion

    #region Navigation
    [RelayCommand]
    protected async Task GoBackAsync()
    {
        if (IsBusy)
            return;

        IsBusy = true;

        try
        {
            Page currentPage = Shell.Current.CurrentPage;
            await Shell.Current.GoToAsync("..", true);
            RemovePage(currentPage);
        }
        finally
        {
            IsBusy = false;
            IsRefreshing = false;
        }
    }
    #endregion

    #region Navigation Settings
    protected void CloseAllFlyoutPlataformButWinUI()
    {
        if (DeviceInfo.Platform != DevicePlatform.WinUI)
        {
            Shell.Current.FlyoutIsPresented = false;
        }
    }

    protected void RemovePage(Page currentPage)
    {
        Shell.Current.Navigation.RemovePage(currentPage);
    }
    #endregion
}

