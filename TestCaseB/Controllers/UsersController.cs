using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;
using TestCaseB.ApiRequestResponse;
using TestCaseB.Services.Interfaces;

namespace TestCaseB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUser _service;

        //handling dependency injection
        public UsersController(IUser user) {
        
         _service = user;
        }



        [HttpPost]
        [Route("signup")]
        public async Task<IActionResult> SignUpAsync(CreateUserRequest model)
        {
            Log.Information($"create user request {JsonConvert.SerializeObject(model)}");
            var response = await _service.SignUp(model);
            Log.Information($"Create user response {JsonConvert.SerializeObject(response)}");
            return await Task.FromResult(new JsonResult(response));
        }


        [HttpPost]
        [Route("verifiedaccount")]
        public async Task<IActionResult>  VerifiedAccount(ConfirmEmailRequest model)
        {
            Log.Information($"ConfirmEmailAsync request {JsonConvert.SerializeObject(model)}");
            var response = await _service.ConfirmEmail(model);
            Log.Information($"ConfirmEmailAsync response {JsonConvert.SerializeObject(response)}");
            return await Task.FromResult(new JsonResult(response));
        }



        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(AuthenticateRequest model)
        {
            Log.Information($"LoginAsync request {JsonConvert.SerializeObject(model)}");
            var response = await _service.LoginAsync(model);
            Log.Information($"LoginAsync response {JsonConvert.SerializeObject(response)}");
            return await Task.FromResult(new JsonResult(response));
        }


    }
}
