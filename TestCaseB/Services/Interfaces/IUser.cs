using TestCaseB.ApiRequestResponse;
using TestCaseB.Utility;

namespace TestCaseB.Services.Interfaces
{
    public interface IUser
    {

        Task<Response<CreateUserResponse>> SignUp(CreateUserRequest model);

        Task<Response<Task>> ConfirmEmail(ConfirmEmailRequest model);
        Task<Response<AuthenticateResponse>> LoginAsync(AuthenticateRequest request);
    }
}
