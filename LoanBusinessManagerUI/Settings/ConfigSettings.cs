namespace LoanBusinessManagerUI.Settings
{
    public abstract class ConfigSettings
    {
        public static string FormatMoneyValue(string typedValue)
        {
            decimal formattedValue = 0;
            if (MoneyInputIsValid(typedValue))
            {
                string value = typedValue.Replace(",", "").Replace(".", "");

                if (value.Length <= 2)
                {
                    value = "0" + value;
                }

                string integerPart = value.Substring(0, value.Length - 2);
                string decimalPart = value.Substring(value.Length - 2);

                if (integerPart == string.Empty)
                    integerPart = "0";

                formattedValue = decimal.Parse(integerPart + "," + decimalPart);
            }

            return formattedValue.ToString();
        }

        public static string FormatFirstLetterToUpper(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return input;
            }

            string[] words = input.ToLower().Split(' ');

            for (int i = 0; i < words.Length; i++)
            {
                if (!string.IsNullOrEmpty(words[i]))
                {
                    char firstChar = char.ToUpper(words[i][0]);
                    string restOfWord = words[i].Substring(1);
                    words[i] = firstChar + restOfWord;
                }
            }

            string result = string.Join(" ", words);

            return result;
        }

        public static string FormatPhoneNumber(string phoneNumber, string phone = "")
        {
            phoneNumber = phoneNumber.Replace(" ", null);
            byte phoneNumberLength = (byte)phoneNumber.Length;
            switch (phoneNumberLength)
            {
                case 8:
                    phoneNumber = "55699" + phoneNumber;
                    break;
                case 9:
                    phoneNumber = "5569" + phoneNumber;
                    break;
                case 10:
                    phoneNumber = "55699" + phoneNumber.Substring(2);
                    break;
                case 11:
                    phoneNumber = "55" + phoneNumber;
                    break;
                case 12:
                    phoneNumber = "55" + phoneNumber.Substring(1);
                    break;
                case 13:
                case 14:
                    break;
                default:
                    if (phoneNumberLength > 14)
                        throw new ArgumentException($"TELEFONE {phone} COM NÚMEROS A MAIS");
                    else
                        throw new ArgumentException($"TELEFONE {phone} FALTANDO NÚMEROS");
            }

            return phoneNumber;
        }

        public static string FormatWhatsappPhoneUrl(string phoneNumber)
        {
            string baseUrl = "https://api.whatsapp.com/send?phone=";
            return baseUrl + phoneNumber;
        }

        public static string InterestTextChanged(TextChangedEventArgs e)
        {
            string interestLoanText = string.Empty;

            if (decimal.TryParse(e.NewTextValue, out decimal value))
                interestLoanText = value.ToString();

            else if (e.NewTextValue == string.Empty)
                interestLoanText = "0";

            else
                interestLoanText = e.OldTextValue;

            return interestLoanText;
        }

        //Even = Par
        //Odd = Ímpar
        public static bool IsEven(int counter)
        {
            if (counter % 2 == 0)
                return true;
            else
                return false;
        }

        public static void SetFrameColorsFromTables<TEntity>(object sender, string color1 = "#FFFFFF", string color2 = "D4D4D4") where TEntity : NotMappedProperties
        {
            if (sender is Frame frame && frame.BindingContext is TEntity entity)
            {
                frame.BackgroundColor = IsEven(entity.Counter) == true ? Color.FromArgb(color1) : Color.FromArgb(color2);
            }
        }

        private static bool MoneyInputIsValid(string lastInput)
        {
            if (decimal.TryParse(lastInput, out decimal value))
                return true;

            else
                return false;
        }
    }
}
