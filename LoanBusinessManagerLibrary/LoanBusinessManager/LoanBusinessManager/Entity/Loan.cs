namespace LBMLibrary.Entity
{
    public class Loan : BusinessClass
    {
        public decimal Interest { get; set; } = 10;
        //public InterestType InterestType { get; set; } = InterestType.Simples;

        public static void ClonePropertiesValues(ref Loan loanDestination, Loan loanSource)
        {
            loanDestination.Interest = loanSource.Interest;
            loanDestination.StartDate = loanSource.StartDate;
            //loanDestination.InterestType = loanSource.InterestType;
            loanDestination.Person = loanSource.Person;
            loanDestination.PersonId = loanSource.PersonId;
            loanDestination.ModificationDate = loanSource.ModificationDate;
            loanDestination.HistoryType = loanSource.HistoryType;
            loanDestination.Amount = loanSource.Amount;
        }

        public static Loan ClonePropertiesValues(Loan loanSource)
        {
            Loan loanDestination = new();
            ClonePropertiesValues(ref loanDestination, loanSource);

            return loanDestination;
        }
    }
}
