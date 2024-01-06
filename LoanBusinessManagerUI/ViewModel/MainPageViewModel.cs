namespace LoanBusinessManagerUI.ViewModel;

public partial class MainPageViewModel : GlobalNavigationViewModel
{
    private readonly DescriptionHistoryService _descriptionHistoryService;

    [ObservableProperty]
    List<DescriptionHistory> descriptions = new();

    [ObservableProperty]
    string searchName = string.Empty;

    public MainPageViewModel(DescriptionHistoryService descriptionHistoryService)
    {
        _descriptionHistoryService = descriptionHistoryService;
    }

    [RelayCommand]
    public async Task GetDescriptionsAsync()
    {
        IsRefreshing = IsBusy = true;

        string pascalCaseSearchName = ConfigSettings.FormatFirstLetterToUpper(SearchName);

        IReadOnlyCollection<DescriptionHistory> descriptionHistories = await _descriptionHistoryService.ListAsync(x => x.Description.Contains(SearchName) ||
                                                                                                                       x.Description.Contains(pascalCaseSearchName));

        if (descriptionHistories != null)
        {
            Descriptions.Clear();
            foreach (DescriptionHistory descriptionHistory in descriptionHistories)
            {
                Descriptions.Add(descriptionHistory);
            }
        }

        SortDescriptionHistory();
        ListDescriptionHistory();
        Descriptions = Descriptions.Take(20).ToList();

        IsRefreshing = IsBusy = false;
    }

    public void ListDescriptionHistory()
    {
        Descriptions = SetCounterTableList(Descriptions);
    }

    public void SortDescriptionHistory()
    {
        Descriptions = Descriptions.OrderByDescending(x => x.ModificationDate).ToList();
    }
}

