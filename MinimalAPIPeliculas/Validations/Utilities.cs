namespace MinimalAPIPeliculas.Validations
{
    public static class Utilities
    {
        public static string requiredFieldMessage = "El campo {PropertyName} es requerido"; 
        public static string requiredFieldMessageMaxLength = "El campo {PropertyName} no debe exceder los {MaxLength} caracteres";
        public static string firstLetterCapitalMessage = "La primera letra del campo {PropertyName} debe ser mayúscula";
        public static string EmailMessage = "El campo {PropertyName} deber ser un email valido";
        public static string GreaterThanOrEqual(DateTime minumumdate)
        {
            return $"El campo {{PropertyName}} no puede ser menor a la fecha {minumumdate.ToString("yyyy-MM-dd")}";
        }
        public static bool capitalLetter(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                return true;
            }
            var firstLetter = nombre[0].ToString();
            return firstLetter == firstLetter.ToUpper();
        }
    }
}
