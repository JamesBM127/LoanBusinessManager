using Plugin.Fingerprint.Abstractions;
using System.Text;

namespace LoanBusinessManagerUI.ViewModel;

[QueryProperty(nameof(Person), "Person")]
public partial class PersonViewModel : BaseViewModel
{
    [ObservableProperty]
    string personName, personSurname, personNickname, personPhone1, personPhone2 = string.Empty;

    [ObservableProperty]
    bool phone1Whatsapp, phone2Whatsapp = false;

    [ObservableProperty]
    Person person = new();

    [ObservableProperty]
    Loan loan = new() { StartDate = DateTime.Now };

    [ObservableProperty]
    Payment payment = new() { PaymentDate = DateTime.Now };

    [ObservableProperty]
    DateTime maximumDate = DateTime.Today;

    [ObservableProperty]
    DateTime inicialDate = DateTime.Today.AddDays(-1);

    [ObservableProperty]
    DateTime finalDate = DateTime.Today;

    [ObservableProperty]
    byte? dayOfMonthDueDate;

    public ObservableCollection<InterestType> InterestsTypes { get; } = new();
    public ObservableCollection<Person> Persons { get; set; } = new();

    private readonly PersonService _personService;
    private readonly PhoneService _phoneService;
    private readonly DebtService _debtService;
    private readonly LoanViewModel _loanViewModel;
    private readonly DescriptionHistoryService _descriptionHistoryService;
    private readonly PaymentViewModel _paymentViewModel;
    private readonly IFingerprint _fingerprint;

    public PersonViewModel(PersonService personService,
                           PhoneService phoneService,
                           LoanViewModel loanViewModel,
                           PaymentViewModel paymentViewModel,
                           DebtService debtService,
                           DescriptionHistoryService descriptionHistoryService,
                           IFingerprint fingerprint)
    {
        _personService = personService;
        _phoneService = phoneService;
        _loanViewModel = loanViewModel;
        _paymentViewModel = paymentViewModel;
        _debtService = debtService;
        _descriptionHistoryService = descriptionHistoryService;
        _fingerprint = fingerprint;
        InterestsTypes = GetEnumValuesList<InterestType>();
    }

    public PersonViewModel(PersonService personService,
                           PhoneService phoneService,
                           LoanViewModel loanViewModel,
                           PaymentViewModel paymentViewModel,
                           DebtService debtService,
                           DescriptionHistoryService descriptionHistoryService)
    {
        _personService = personService;
        _phoneService = phoneService;
        _loanViewModel = loanViewModel;
        _paymentViewModel = paymentViewModel;
        _debtService = debtService;
        _descriptionHistoryService = descriptionHistoryService;
        InterestsTypes = GetEnumValuesList<InterestType>();
    }

    #region CRUD
    [RelayCommand]
    async Task CreatePerson()
    {
        bool saved = false;
        IsBusy = true;
        DateTime modificationDate = DateTime.Now;
        StringBuilder historyDescription = new StringBuilder();

        try
        {
            Person personToCreate = new Person()
            {
                Name = ConfigSettings.FormatFirstLetterToUpper(PersonName),
                Surname = ConfigSettings.FormatFirstLetterToUpper(PersonSurname),
                Nickname = ConfigSettings.FormatFirstLetterToUpper(PersonNickname),
                ModificationDate = modificationDate,
                HistoryType = HistoryType.Create
            };
            _personService.IsValid(personToCreate);
            historyDescription.Append($"{personToCreate.Name}");
            personToCreate.PaymentStatus = PaymentStatus.Cleared;
            bool added = await _personService.AddAsync(personToCreate);

            if (added)
            {
                List<Phone> phoneList = new List<Phone>();

                if (!string.IsNullOrWhiteSpace(PersonPhone1))
                {
                    Phone phone = new Phone()
                    {
                        PhoneNumber = ConfigSettings.FormatPhoneNumber(PersonPhone1, "1"),
                        IsWhatsapp = Phone1Whatsapp,
                        PersonId = personToCreate.Id,
                        ModificationDate = modificationDate,
                        HistoryType = HistoryType.Create
                    };
                    historyDescription.Append($", {phone.PhoneNumber}");
                    phoneList.Add(phone);
                }

                if (!string.IsNullOrWhiteSpace(PersonPhone2))
                {
                    Phone phone = new Phone()
                    {
                        PhoneNumber = ConfigSettings.FormatPhoneNumber(PersonPhone2, "2"),
                        IsWhatsapp = Phone2Whatsapp,
                        PersonId = personToCreate.Id,
                        ModificationDate = modificationDate,
                        HistoryType = HistoryType.Create
                    };
                    historyDescription.Append($", {phone.PhoneNumber}");
                    phoneList.Add(phone);
                }

                if (phoneList.Count > 0)
                    await _phoneService.AddAsync(phoneList);

                DescriptionHistory descriptionHistory = new DescriptionHistory()
                {
                    ItemId = personToCreate.Id,
                    Description = historyDescription.ToString(),
                    ModificationType = ModificationType.Data,
                    ModificationDate = modificationDate,
                    HistoryType = HistoryType.Create
                };
                added = await _descriptionHistoryService.AddAsync(descriptionHistory);

                if (added)
                    saved = await _personService.CommitAsync(added);

                PersonName = PersonSurname = PersonNickname = PersonPhone1 = PersonPhone2 = string.Empty;

                if (saved)
                {
                    await Shell.Current.DisplayAlert("Cadastro", "Cadastro feito com sucesso", "Ok");
                    await Shell.Current.GoToAsync("..", true);
                }
            }
        }
        catch (ArgumentNullException ex)
        {
            await Shell.Current.DisplayAlert("Cadastro inválido", $"{ex.ParamName}", "Ok");
        }
        catch (ArgumentException ex)
        {
            await Shell.Current.DisplayAlert(ex.Message, $"{ex.ParamName}", "Ok");
        }
        finally
        {
            IsBusy = false;
        }
    }

    public async Task CreateLoan()
    {
        if (Loan.Amount <= 0)
            return;

        if (!await SaveConfirmation(Person.Name, Person.Nickname, Loan.Amount, ModificationType.Loan))
            return;

        DateTime modificationDate = DateTime.Now;
        Loan.PersonId = Person.Id;
        Loan.ModificationDate = modificationDate;
        Loan.HistoryType = HistoryType.Create;

        bool added = await _loanViewModel.CreateLoan(Loan, false);

        DescriptionHistory descriptionHistory = new DescriptionHistory()
        {
            ModificationType = ModificationType.Loan,
            ModificationDate = modificationDate,
            Description = $"{Person.Name}; {Loan.Amount.ToString("C")}",
            ItemId = Loan.Id,
            HistoryType = HistoryType.Create,
        };

        if (added)
        {
            if (Person.Debt == null)
            {
                decimal interestPerMonth = (Loan.Amount * (Loan.Interest / 100));
                Debt debt = new Debt()
                {
                    //MUDAR ISSO AQUI QUANDO IMPLEMENTAR OS JUROS (Descomentar a linha abaixo)
                    InterestPerMonth = interestPerMonth,
                    AmountRaw = Loan.Amount + interestPerMonth,
                    DayOfMonthDueDate = (byte)modificationDate.Day,
                    PersonId = Person.Id,
                    ModificationDate = modificationDate,
                    HistoryType = HistoryType.Create
                };

                added = await _debtService.AddAsync(debt);

                Person.Debt = debt;
            }
            else
            {
                //MUDAR ISSO AQUI QUANDO IMPLEMENTAR OS JUROS (Descomentar a linha abaixo)
                Person.Debt.InterestPerMonth += (Loan.Amount * (Loan.Interest / 100));
                Person.Debt.AmountRaw += Loan.Amount + (Loan.Amount * (Loan.Interest / 100));
                Person.Debt.ModificationDate = modificationDate;
                Person.Debt.HistoryType = HistoryType.Update;
                added = _debtService.Update(Person.Debt);
            }

            bool updated = false;

            Person.PaymentStatus = PaymentStatus.Pending;

            descriptionHistory.HistoryType = HistoryType.Create;
            updated = _personService.Update(Person);
            await _descriptionHistoryService.AddAsync(descriptionHistory);
            bool saved = await _personService.CommitAsync(updated);

            if (saved)
            {
                await Shell.Current.DisplayAlert("Sucesso", $"Emprestimo {descriptionHistory.Description}", "Ok");
            }

            Loan = new() { StartDate = DateTime.Now };
        }
    }

    public async Task CreatePayment()
    {
        if (Payment.Amount <= 0)
            return;

        if (!await SaveConfirmation(Person.Name, Person.Nickname, Payment.Amount, ModificationType.Payment))
            return;

        DateTime modificationDate = DateTime.Now;
        Payment.ModificationDate = modificationDate;
        Payment.HistoryType = HistoryType.Create;
        Payment.PersonId = Person.Id;

        bool added = await _paymentViewModel.CreatePayment(Payment, false);

        DescriptionHistory descriptionHistory = new DescriptionHistory()
        {
            ModificationType = ModificationType.Payment,
            ModificationDate = modificationDate,
            Description = $"{Person.Name}; {Payment.Amount.ToString("C")}",
            ItemId = Payment.Id,
            HistoryType = HistoryType.Create
        };

        added = await _descriptionHistoryService.AddAsync(descriptionHistory);

        if (added)
        {
            if (Person.Debt == null)
            {
                Person.Debt = await _debtService.GetAsync(x => x.PersonId == Person.Id);
            }

            Person.Debt.ModificationDate = modificationDate;
            Person.Debt.HistoryType = HistoryType.Update;
            Person.Debt.AmountRaw -= Payment.Amount;
            //DESCOMENTAR A LINHA ABAIXO QUANDO JUROS ESTIVEREM IMPLEMENTADOS
            //Person.Debt.InterestPerMonth -= Payment.Amount;

            if (Person.Debt.AmountRaw <= 0)
                Person.PaymentStatus = PaymentStatus.Cleared;
            else
                Person.PaymentStatus = PaymentStatus.Pending;

            bool updated = _personService.Update(Person);

            bool saved = await _personService.CommitAsync(updated);

            if (saved)
            {
                await Shell.Current.DisplayAlert("Sucesso", $"Pagamento {descriptionHistory.Description}", "Ok");
            }

            Payment = new() { PaymentDate = DateTime.Now };
        }
    }

    [RelayCommand]
    async Task GetPerson()
    {
        if (IsBusy)
            return;

        IsBusy = IsRefreshing = true;

        try
        {
            Guid id = Person.Id;
            Person = null;
            Person = await _personService.GetAsync(x => x.Id == id);
            Person.Debt = await _debtService.GetAsync(x => x.PersonId == id);
        }
        finally
        {
            IsBusy = IsRefreshing = false;
        }
    }

    [RelayCommand]
    async Task ListPersons()
    {
        IReadOnlyCollection<Person> people = await _personService.ListAsync();

        if (Persons.Count != 0)
            Persons.Clear();

        foreach (Person person in people)
            Persons.Add(person);
    }

    public async Task ListPayments()
    {
        Person.Payments = await _paymentViewModel.ListPaymentsAsync(Person.Id, InicialDate, FinalDate);
        await GoToPaymentsListPage();
    }

    public async Task ListLoans()
    {
        Person.Loans = await _loanViewModel.ListLoansAsync(Person.Id, InicialDate, FinalDate);
        await GoToLoansListPage();
    }

    [RelayCommand]
    async Task UpdatePerson()
    {
        DateTime modificationDate = DateTime.Now;

        Person.ModificationDate = modificationDate;

        try
        {
            Person.Name = PersonName;
            Person.Surname = PersonSurname;
            Person.Nickname = PersonNickname;

            _personService.IsValid(Person);

            DescriptionHistory descriptionHistory = new DescriptionHistory()
            {
                ModificationType = ModificationType.Data,
                ModificationDate = modificationDate,
                Description = GetDescriptionEditPerson(),
                HistoryType = HistoryType.Update,
                ItemId = Person.Id
            };

            await _descriptionHistoryService.AddAsync(descriptionHistory);
            try
            {
                Person.Phones = SetPhones(modificationDate);
            }
            catch (ArgumentException ex)
            {
                await Shell.Current.DisplayAlert("ERRO!", ex.Message, "OK");
                return;
            }

            bool updated = _personService.Update(Person);

            if (updated)
            {
                bool saved = await _personService.CommitAsync(updated);

                if (saved)
                {
                    await Shell.Current.DisplayAlert("Sucesso", "Perfil editado com sucesso", "Ok");
                    await Shell.Current.GoToAsync("..", true);
                }
            }
        }
        catch (ArgumentNullException ex)
        {
            await Shell.Current.DisplayAlert("Atualização inválido", $"{ex.ParamName}", "Ok");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Erro", ex.Message, "Ok");
        }
        finally
        {
            IsBusy = false;
            PersonName = PersonSurname = PersonNickname = PersonPhone1 = PersonPhone2 = string.Empty;
        }
    }
    #endregion

    #region Navigation
    async Task GoToPersonPage(Person person)
    {
        if (person == null)
            return;

        try
        {
            await Shell.Current.GoToAsync($"{nameof(PersonPage)}", true, new Dictionary<string, object>
            {
                {"Person", person }
            });
        }
        catch (Exception)
        {
            throw;
        }
    }

    [RelayCommand]
    async Task GoToEditPersonPage()
    {
        List<Phone> phonesList = Person.Phones.ToList();
        PersonName = Person.Name;
        PersonSurname = Person.Surname;
        PersonNickname = Person.Nickname;

        try
        {
            PersonPhone1 = phonesList[0].PhoneNumber;
            Phone1Whatsapp = phonesList[0].IsWhatsapp;
        }
        catch (ArgumentOutOfRangeException)
        {
            PersonPhone1 = string.Empty;
            Phone1Whatsapp = false;
        }

        try
        {
            PersonPhone2 = phonesList[1].PhoneNumber;
            Phone2Whatsapp = phonesList[1].IsWhatsapp;
        }
        catch (ArgumentOutOfRangeException)
        {
            PersonPhone2 = string.Empty;
            Phone2Whatsapp = false;
        }

        await Shell.Current.GoToAsync($"{nameof(EditPersonPage)}", true);
    }

    [RelayCommand]
    async Task GoToLoansListPage()
    {
        Person.Loans = await _loanViewModel.ListLoansAsync(Person.Id, InicialDate, FinalDate);
        if (Person.Loans == null || Person.Loans.Count() == 0)
        {
            await Shell.Current.DisplayAlert("ERRO", $"Nenhum emprestimo registrado entre os dias {InicialDate.ToShortDateString()} e {FinalDate.ToShortDateString()}", "Ok");
        }
        else
        {
            List<Loan> loansSorted = Person.Loans.OrderByDescending(x => x.StartDate).ToList();
            loansSorted = SetCounterTableList(loansSorted);
            await Shell.Current.GoToAsync(nameof(LoansListPage), true, new Dictionary<string, object>
            {
                { "Loans", loansSorted }
            });
        }
    }

    [RelayCommand]
    async Task GoToPaymentsListPage()
    {
        Person.Payments = await _paymentViewModel.ListPaymentsAsync(Person.Id, InicialDate, FinalDate);
        if (Person.Payments == null || Person.Payments.Count() == 0)
        {
            await Shell.Current.DisplayAlert("ERRO", $"Nenhum pagamento registrado entre os dias {InicialDate.ToShortDateString()} e {FinalDate.ToShortDateString()}", "Ok");
        }
        else
        {
            List<Payment> paymentsSorted = Person.Payments.OrderByDescending(x => x.PaymentDate).ToList();
            paymentsSorted = SetCounterTableList(paymentsSorted);
            await Shell.Current.GoToAsync(nameof(PaymentsListPage), true, new Dictionary<string, object>
            {
                { "Payments", paymentsSorted }
            });
        }
    }

    public async Task GoToWhatsappChat(string phoneNumber)
    {
        string url = ConfigSettings.FormatWhatsappPhoneUrl(phoneNumber);
        await Browser.OpenAsync(url, BrowserLaunchMode.SystemPreferred);
    }

    [RelayCommand]
    async Task GoBackPersonAsync()
    {
        Person = null;
        _personService.ClearTrack();
        await GoBackAsync();
    }
    #endregion

    #region Settings
    [RelayCommand]
    void SetLast30Days()
    {
        SetFinalDateToday();
        InicialDate = DateTime.Today.AddDays(-30);
    }

    [RelayCommand]
    void SetLast60Days()
    {
        SetFinalDateToday();
        InicialDate = DateTime.Today.AddDays(-60);
    }

    [RelayCommand]
    void SetLast90Days()
    {
        SetFinalDateToday();
        InicialDate = DateTime.Today.AddDays(-90);
    }

    [RelayCommand]
    void SetCurrentYear()
    {
        SetFinalDateToday();
        InicialDate = new DateTime(DateTime.Today.Year, 1, 1);
    }

    private void SetFinalDateToday()
    {
        FinalDate = DateTime.Today;
    }

    private List<DateTime> SortListByDateDecending<TEntity>(List<DateTime> entities)
    {
        if (entities != null || entities.Count != 0)
        {
            entities.Sort();
            entities.Reverse();
        }

        return entities;
    }

    public List<Phone> SetPhones(DateTime modificationDate)
    {
        List<Phone> phonesList = Person.Phones.ToList();

        if (!string.IsNullOrWhiteSpace(PersonPhone1))
        {
            try
            {
                phonesList[0].PhoneNumber = ConfigSettings.FormatPhoneNumber(PersonPhone1, "1");
                phonesList[0].IsWhatsapp = Phone1Whatsapp;
                phonesList[0].ModificationDate = modificationDate;
            }
            catch (ArgumentOutOfRangeException)
            {
                Phone phone = new Phone()
                {
                    PhoneNumber = PersonPhone1,
                    IsWhatsapp = Phone1Whatsapp,
                    PersonId = Person.Id
                };
                phonesList.Add(phone);
            }
            catch (ArgumentException)
            {
                throw;
            }
        }

        if (!string.IsNullOrWhiteSpace(PersonPhone2))
        {
            try
            {
                phonesList[1].PhoneNumber = ConfigSettings.FormatPhoneNumber(PersonPhone2, "2");
                phonesList[1].IsWhatsapp = Phone2Whatsapp;
                phonesList[1].ModificationDate = modificationDate;
            }
            catch (ArgumentOutOfRangeException)
            {
                Phone phone = new Phone()
                {
                    PhoneNumber = PersonPhone2,
                    IsWhatsapp = Phone2Whatsapp,
                    PersonId = Person.Id
                };
                phonesList.Add(phone);
            }
            catch (ArgumentException)
            {
                throw;
            }
        }

        return phonesList;
    }

    private string GetDescriptionEditPerson()
    {
        StringBuilder description = new StringBuilder();
        GetDescriptionName(ref description);
        GetDescriptionPhone(ref description);

        return description.ToString();
    }

    private void GetDescriptionName(ref StringBuilder description)
    {
        if (!string.IsNullOrWhiteSpace(Person.Name))
            description.Append(Person.Name);
        else
            description.Append(Person.Nickname);
    }

    private void GetDescriptionPhone(ref StringBuilder description)
    {
        List<Phone> phones = Person.Phones.ToList();

        try
        {
            if ((phones[0].PhoneNumber != PersonPhone1) || (phones[0].IsWhatsapp != Phone1Whatsapp))
                description.Append($"; {PersonPhone1}");

            if ((phones[1].PhoneNumber != PersonPhone2) || (phones[1].IsWhatsapp != Phone2Whatsapp))
                description.Append($"; {PersonPhone2}");
        }
        catch (ArgumentOutOfRangeException)
        {
            return;
        }
    }
    #endregion

    [RelayCommand]
    async Task PersonDataMobile()
    {
        bool authIsAvailable = await _fingerprint.IsAvailableAsync(true);

        if (authIsAvailable)
        {
            AuthenticationRequestConfiguration request = new AuthenticationRequestConfiguration("Autenticação", null);
            request.AllowAlternativeAuthentication = true;
            FingerprintAuthenticationResult result = await _fingerprint.AuthenticateAsync(request);
            if (result.Authenticated)
            {
                await Shell.Current.DisplayAlert("Autenticação!", "Acesso Garatido", "OK");
            }
            else
            {
                await Shell.Current.DisplayAlert("FALHA", "Acesso PROIBIDO", "OK");
            }
        }
    }

    [RelayCommand]
    async Task PersonDataWinUI()
    {
        await Shell.Current.DisplayAlert("Clicado no Windows!", "WINDOWS", "OK");

    }
}
