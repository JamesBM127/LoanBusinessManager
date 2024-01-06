namespace LoanBusinessManagerUI.JBMException
{
    public class NoInternetConnectionException : Exception
    {
        public NoInternetConnectionException() : base("Sem conexão com a internet!")
        {
        }

        public NoInternetConnectionException(string message) : base(message)
        {
        }

        public NoInternetConnectionException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
