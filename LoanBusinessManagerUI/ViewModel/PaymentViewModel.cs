namespace LoanBusinessManagerUI.ViewModel;

[QueryProperty(nameof(Payments), "Payments")]
public partial class PaymentViewModel : BaseViewModel
{
    public ObservableCollection<Person> Persons { get; set; } = new();

    [ObservableProperty]
    List<Payment> payments = new();

    [ObservableProperty]
    Payment payment = new();

    [ObservableProperty]
    Payment paymentToUpdate = new();

    [ObservableProperty]
    string amountPayment;

    private readonly PaymentService _paymentService;
    private readonly DebtService _debtService;
    private readonly PersonService _personService;
    private readonly DescriptionHistoryService _descriptionHistoryService;

    public PaymentViewModel(PaymentService paymentService,
                            PersonService personService,
                            DebtService debtService,
                            DescriptionHistoryService descriptionHistoryService)
    {
        _paymentService = paymentService;
        _debtService = debtService;
        _personService = personService;
        _descriptionHistoryService = descriptionHistoryService;
    }

    #region CRUD
    public async Task<bool> CreatePayment(Payment payment, bool commit = true)
    {
        bool added = false;
        try
        {
            if (payment == null)
                throw new ArgumentNullException("Emprestimo inválido");

            added = await _paymentService.AddAsync(payment);

            if (commit)
                await _paymentService.CommitAsync(added);
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

    public async Task<IReadOnlyCollection<Payment>> ListPaymentsAsync(Guid personId, DateTime inicialDate, DateTime finalDate)
    {
        if (Payments != null && Payments.Count != 0)
            return Payments;

        else
            return await _paymentService.ListAsync(x => x.PaymentDate.Date >= inicialDate &&
                                                        x.PaymentDate.Date <= finalDate &&
                                                        x.PersonId == personId);
    }

    [RelayCommand]
    async Task UpdatePayment()
    {
        if (SameValueUpdate(Payment.Amount, PaymentToUpdate.Amount))
            return;

        if (!await SaveConfirmation(Payment.Person.Name, Payment.Person.Nickname, Payment.Amount, ModificationType.Loan))
            return;

        DateTime modificationDate = DateTime.Now;
        string personName = string.Empty;

        try
        {
            personName = Payment.Person.Name;
        }
        catch (Exception ex)
        {
            throw new Exception("Pessoa não localizada [NULL]");
        }

        DescriptionHistory descriptionHistory = new DescriptionHistory()
        {
            ModificationType = ModificationType.Payment,
            ModificationDate = modificationDate,
            Description = $"{personName}; [Antigo {Payment.Amount.ToString("C")}] [Novo {PaymentToUpdate.Amount.ToString("C")}]",
            HistoryType = HistoryType.Update,
            ItemId = Payment.Id
        };

        Debt debt = await _debtService.GetAsync(x => x.PersonId == Payment.PersonId);
        SetBaseHistoryEntity<Debt>(ref debt, modificationDate, HistoryType.Update);

        debt.AmountRaw += Payment.Amount;
        debt.AmountRaw -= PaymentToUpdate.Amount;
        //DESCOMENTAR QUANDO IMPLEMENTAR OS JUROS
        //debt.InterestPerMonth += Payment.Amount;
        //debt.InterestPerMonth -= PaymentToUpdate.Amount;

        bool added = await _descriptionHistoryService.AddAsync(descriptionHistory);

        if (added)
        {
            await SetPersonPaymentsStatusAfterUpdateAsync(debt.AmountRaw);
            PaymentToUpdate = SetBaseHistoryEntity(PaymentToUpdate, modificationDate, HistoryType.Update);

            Payment paymentAux = Payment;
            Payment.ClonePropertiesValues(ref paymentAux, PaymentToUpdate);

            added = _paymentService.Update(Payment);
            bool saved = await _paymentService.CommitAsync(added);

            if (saved)
            {
                int indexPosition = Payments.FindIndex(x => x.Id == Payment.Id);
                Payments[indexPosition] = Payment;
                await Shell.Current.DisplayAlert("Sucesso", "Editado com sucesso", "Ok");
                await Shell.Current.GoToAsync("..", true);
            }
        }
    }
    #endregion

    #region Navegation
    [RelayCommand]
    async Task GoToPaymentEdit(Payment paymentToEdit)
    {
        if (paymentToEdit == null)
            return;

        Payment = await _paymentService.GetAsync(x => x.Id == paymentToEdit.Id);

        PaymentToUpdate = Payment.ClonePropertiesValues(Payment);

        AmountPayment = string.Format("{0:N2}", Payment.Amount);

        //if (Payment.Id == Guid.Empty)
        //{
        //    decimal amount = paymentToEdit.Amount;
        //    paymentToEdit.Amount = (amount.ToString().EndsWith(",0") || amount.ToString().EndsWith(".0")) ?
        //        decimal.Parse(paymentToEdit.Amount.ToString() + "0") : paymentToEdit.Amount;

        //    Payment = (Payment)paymentToEdit.Clone();
        //    PaymentToUpdate = (Payment)paymentToEdit.Clone();
        //}

        await Shell.Current.GoToAsync($"{nameof(EditPaymentPage)}", true);
    }

    [RelayCommand]
    async Task GoBackPaymentAsync()
    {
        Payments = null;
        await GoBackAsync();
    }
    //[RelayCommand]
    //async Task GoToPersonPage(Person person)
    //{
    //    if (person == null)
    //        return;

    //    if (person.Phones == null)
    //        person.Phones = await _phoneService.ListAsync(x => x.PersonId == person.Id);

    //    try
    //    {
    //        await Shell.Current.GoToAsync($"{nameof(PersonPage)}", true, new Dictionary<string, object>
    //        {
    //            { "Person", person }
    //        });
    //    }
    //    catch (Exception ex)
    //    {
    //        throw;
    //    }
    //}
    #endregion

    #region Settings
    private async Task SetPersonPaymentsStatusAfterUpdateAsync(decimal debtAmount)
    {
        if (debtAmount > 0)
        {
            if (Payment.Person == null)
            {
                Person person = await _personService.GetAsync(x => x.Id == Payment.PersonId);
                person.PaymentStatus = PaymentStatus.Pending;
                Payment.Person = person;
            }
            else
            {
                Payment.Person.PaymentStatus = PaymentStatus.Pending;
            }
        }
        else
        {
            if (Payment.Person == null)
            {
                Person person = await _personService.GetAsync(x => x.Id == Payment.PersonId);
                person.PaymentStatus = PaymentStatus.Cleared;
                Payment.Person = person;
            }
            else
            {
                Payment.Person.PaymentStatus = PaymentStatus.Cleared;
            }
        }
    }
    #endregion
}
