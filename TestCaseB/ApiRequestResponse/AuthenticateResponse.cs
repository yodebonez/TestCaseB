using System.ComponentModel.DataAnnotations;
using TestCaseB.Models.ViewModels;

namespace TestCaseB.ApiRequestResponse
{
    public class AuthenticateResponse : UserModel
    {

       

        public TokenData Token { get; set; }
    }


    public class TokenData
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpireFrom { get; set; }
        public DateTime ExpireTo { get; set; }
        public string ExpireTimeTo { get; set; } = string.Empty;

    }

}
