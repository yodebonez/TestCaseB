namespace TestCaseB.Utility
{
    public class CodeGenerator
    {

        public static string GenerateNumber(int lenght, string prefix = "", string appId = "")
        {


            const string chars = "1234567890";
            var random = new Random();

            var code = new string(Enumerable.Repeat(prefix + chars, lenght - prefix.Length)
              .Select(s => s[random.Next(s.Length)]).ToArray());

            return $"{prefix}{code}";

        }

        internal static string Generate(int v, object pREFIX_CUSTOMER_CODE)
        {
            throw new NotImplementedException();
        }
    }
}
