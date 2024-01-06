namespace LoanBusinessManagerUI.ViewModel;

[QueryProperty(nameof(Loans), "Loans")]
[QueryProperty(nameof(Loan), "Loan")]
public partial class LoanViewModel : BaseViewModel
{
    //[ObservableProperty]
    //public List<Person> persons  = new();
    public ObservableCollection<Person> Persons { get; set; } = new();
    public ObservableCollection<InterestType> InterestsTypes { get; } = new();

    [ObservableProperty]
    List<Loan> loans = new();

    [ObservableProperty]
    Loan loan = new();

    [ObservableProperty]
    Loan loanToUpdate = new();

    [ObservableProperty]
    string searchName = string.Empty;

    [ObservableProperty]
    string totalLoan;

    [ObservableProperty]
    string amountLoan;

    private readonly LoanService _loanService;
    private readonly PersonService _personService;
    private readonly PhoneService _phoneService;
    private readonly DebtService _debtService;
    private readonly DescriptionHistoryService _descriptionHistoryService;

    public LoanViewModel(LoanService loanService,
                         PersonService personService,
                         PhoneService phoneService,
                         DebtService debtService,
                         DescriptionHistoryService descriptionHistoryService)
    {
        _loanService = loanService;
        _personService = personService;
        _phoneService = phoneService;
        _debtService = debtService;
        _descriptionHistoryService = descriptionHistoryService;
        InterestsTypes = GetEnumValuesList<InterestType>();
    }

    public LoanViewModel(PersonService personService)
    {
        _personService = personService;
        InterestsTypes = GetEnumValuesList<InterestType>();
    }

    #region CRUD
    public async Task<bool> CreateLoan(Loan loan, bool commit = true)
    {
        bool added = false;
        try
        {
            if (loan == null)
                throw new ArgumentNullException("Emprestimo inválido");

            added = await _loanService.AddAsync(loan);

            if (commit)
                await _loanService.CommitAsync(added);
        }
        catch (ArgumentNullException ex)
        {
            await Shell.Current.DisplayAlert("ERRO", $"{ex.ParamName}", "Ok");
        }
        finally
        {
            IsBusy = false;
        }

        return added;
    }

    [RelayCommand]
    async Task ListPersons()
    {
        IsRefreshing = IsBusy = true;

        string pascalCaseSearchName = ConfigSettings.FormatFirstLetterToUpper(SearchName);

        IReadOnlyCollection<Person> people = await _personService.ListAsync(x => x.Name.Contains(SearchName) ||
                                                                                 x.Name.Contains(pascalCaseSearchName) ||
                                                                                 x.Nickname.Contains(SearchName) ||
                                                                                 x.Nickname.Contains(pascalCaseSearchName));

        if (people == null)
            return;

        if (Persons.Count != 0)
            Persons.Clear();

        List<Person> peopleList = SetCounterTableList(people.ToList());

        foreach (Person person in peopleList)
            Persons.Add(person);

        IsRefreshing = IsBusy = false;
    }

    public async Task<IReadOnlyCollection<Loan>> ListLoansAsync(Guid personId, DateTime inicialDate, DateTime finalDate)
    {
        if (Loans != null && Loans.Count != 0)
            return Loans;

        else
            return await _loanService.ListAsync(x => x.StartDate.Date >= inicialDate &&
                                                     x.StartDate.Date <= finalDate &&
                                                     x.PersonId == personId);
    }

    [RelayCommand]
    async Task UpdateLoan()
    {
        if (SameValueUpdate(Loan.Amount, LoanToUpdate.Amount, Loan.Interest, LoanToUpdate.Interest))
            return;

        if (!await SaveConfirmation(Loan.Person.Name, Loan.Person.Nickname, LoanToUpdate.Amount, ModificationType.Loan))
            return;

        DateTime modificationDate = DateTime.Now;
        string personName = string.Empty;

        try
        {
            personName = Loan.Person.Name;
        }
        catch (Exception ex)
        {
            throw new Exception("Pessoa não localizada [NULL]");
        }

        DescriptionHistory descriptionHistory = new DescriptionHistory()
        {
            ModificationType = ModificationType.Loan,
            ModificationDate = modificationDate,
            Description = $"{personName}; [Antigo {Loan.Amount.ToString("C")}] [Novo {LoanToUpdate.Amount.ToString("C")}]",
            HistoryType = HistoryType.Update,
            ItemId = Loan.Id
        };

        Debt debt = await _debtService.GetAsync(x => x.PersonId == Loan.PersonId);
        SetBaseHistoryEntity(ref debt, modificationDate, HistoryType.Update);

        debt.AmountRaw -= Loan.Amount * (1 + (Loan.Interest / 100));
        debt.AmountRaw += LoanToUpdate.Amount * (1 + (LoanToUpdate.Interest / 100));

        debt.InterestPerMonth -= (Loan.Amount * (Loan.Interest / 100)); ;
        debt.InterestPerMonth += (LoanToUpdate.Amount * (LoanToUpdate.Interest / 100)); ;

        bool added = await _descriptionHistoryService.AddAsync(descriptionHistory);

        if (added)
        {
            await SetPersonPaymentsStatusAfterUpdate(debt.AmountRaw);
            LoanToUpdate = SetBaseHistoryEntity(LoanToUpdate, modificationDate, HistoryType.Update);

            Loan loanAux = Loan;
            Loan.ClonePropertiesValues(ref loanAux, LoanToUpdate);

            added = _loanService.Update(Loan);
            bool saved = await _loanService.CommitAsync(added);

            if (saved)
            {
                int indexPosition = Loans.FindIndex(x => x.Id == Loan.Id);
                Loans[indexPosition] = Loan;
                await Shell.Current.DisplayAlert("Sucesso", "Editado com sucesso", "Ok");
                await GoBackAsync();
            }
        }
    }

    #endregion

    #region Navegation
    [RelayCommand]
    async Task GoToPersonPage(Person person)
    {
        if (person == null)
            return;
        //else
        //    person.Loans = null;

        if (person.Phones == null)
            person.Phones = await _phoneService.ListAsync(x => x.PersonId == person.Id);

        if (person.Debt == null)
            person.Debt = await _debtService.GetAsync(x => x.PersonId == person.Id);

        try
        {
            await Shell.Current.GoToAsync($"{nameof(PersonPage)}", true, new Dictionary<string, object>
            {
                { "Person", person }
            });
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    [RelayCommand]
    async Task GoToLoanEdit(Loan loanToEdit)
    {
        if (loanToEdit == null)
            return;

        Loan = await _loanService.GetAsync(x => x.Id == loanToEdit.Id);

        LoanToUpdate = Loan.ClonePropertiesValues(Loan);

        decimal totalLoanDecimal = Loan.Amount * (1 + (Loan.Interest / 100));
        TotalLoan = string.Format("{0:N2}", totalLoanDecimal);
        AmountLoan = string.Format("{0:N2}", Loan.Amount);

        await Shell.Current.GoToAsync($"{nameof(EditLoanPage)}", true);
    }

    [RelayCommand]
    async Task GoBackLoanAsync()
    {
        Loans = null;
        await GoBackAsync();
    }
    #endregion

    #region Settings
    private async Task SetPersonPaymentsStatusAfterUpdate(decimal debtAmount)
    {
        if (debtAmount > 0)
        {
            if (Loan.Person == null)
            {
                Person person = await _personService.GetAsync(x => x.Id == Loan.PersonId);
                person.PaymentStatus = PaymentStatus.Pending;
                Loan.Person = person;
            }
            else
            {
                Loan.Person.PaymentStatus = PaymentStatus.Pending;
            }
        }
        else
        {
            if (Loan.Person == null)
            {
                Person person = await _personService.GetAsync(x => x.Id == Loan.PersonId);
                person.PaymentStatus = PaymentStatus.Cleared;
                Loan.Person = person;
            }
            else
            {
                Loan.Person.PaymentStatus = PaymentStatus.Cleared;
            }
        }
    }
    #endregion
}
