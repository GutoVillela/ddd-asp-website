using Flunt.Notifications;
using Flunt.Validations;
using KadoshDomain.Enums;
using KadoshShared.ValueObjects;

namespace KadoshDomain.ValueObjects
{
    public class Document : ValueObject
    {
        #region Constructor
        public Document(string number, EDocumentType type)
        {
            Number = number;
            Type = type;

            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsTrue(ValidateDocument(), nameof(Number), $"{type} inválido.")
            );
        }
        #endregion Constructor

        #region Properties

        public string Number { get; private set; }
        public EDocumentType Type { get; private set; }

        #endregion Properties

        #region Methods

        private bool ValidateDocument()
        {
            if (Type == EDocumentType.CNPJ && ValidateCNPJ(Number))
                return true;

            if (Type == EDocumentType.CPF && ValidateCPF(Number))
                return true;

            return false;
        }

        private bool ValidateCPF(string cpf)
        {
            if (string.IsNullOrEmpty(cpf))
                return false;

            int[] multiplier1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplier2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string hasCPF;
            string digit;
            int sum;
            int rest;

            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");

            if (cpf.Length != 11)
                return false;

            hasCPF = cpf[..9];
            sum = 0;

            for (int i = 0; i < 9; i++)
                sum += int.Parse(hasCPF[i].ToString()) * multiplier1[i];

            rest = sum % 11;

            if (rest < 2)
                rest = 0;
            else
                rest = 11 - rest;

            digit = rest.ToString();
            hasCPF += digit;
            sum = 0;

            for (int i = 0; i < 10; i++)
                sum += int.Parse(hasCPF[i].ToString()) * multiplier2[i];

            rest = sum % 11;

            if (rest < 2)
                rest = 0;
            else
                rest = 11 - rest;

            digit += rest.ToString();
            return cpf.EndsWith(digit);
        }

        private bool ValidateCNPJ(string cnpj)
        {
            if (string.IsNullOrEmpty(cnpj))
                return false;

            int[] multiplier1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplier2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int sum;
            int rest;
            string digit;
            string tempCnpj;
            
            cnpj = cnpj.Trim();
            cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");

            if (cnpj.Length != 14)
                return false;

            tempCnpj = cnpj[..12];
            sum = 0;

            for (int i = 0; i < 12; i++)
                sum += int.Parse(tempCnpj[i].ToString()) * multiplier1[i];

            rest = (sum % 11);

            if (rest < 2)
                rest = 0;
            else
                rest = 11 - rest;

            digit = rest.ToString();
            tempCnpj += digit;
            sum = 0;

            for (int i = 0; i < 13; i++)
                sum += int.Parse(tempCnpj[i].ToString()) * multiplier2[i];

            rest = (sum % 11);

            if (rest < 2)
                rest = 0;
            else
                rest = 11 - rest;

            digit += rest.ToString();

            return cnpj.EndsWith(digit);
        }
        #endregion Methods
    }
}
