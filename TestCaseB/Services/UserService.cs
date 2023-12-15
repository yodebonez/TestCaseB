using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TestCaseB.ApiRequestResponse;
using TestCaseB.Models;
using TestCaseB.Models.ViewModels;
using TestCaseB.Services.Interfaces;
using TestCaseB.Utility;

namespace TestCaseB.Services
{
    public class UserService : IUser
    {

        private readonly DataContext _db;
        private readonly IMapper _mapper;

        private readonly IConfiguration _config;

        public UserService(DataContext context, IConfiguration configuration, IMapper mapper) { 
        
            _db = context;
            _config = configuration;
            _mapper = mapper;
        }

        public async Task<Response<CreateUserResponse>> SignUp(CreateUserRequest model)
        {

            string ActivationCode = "";
            try
            {

                var user = await _db.users.FirstOrDefaultAsync(x => x.Email == model.Email);

                if (user != null)
                    return await Task.FromResult(new Response<CreateUserResponse>
                    {
                        Message = "User already exit"
                    });
                if (user != null && !user.IsEmailConfirmed)
                    return await Task.FromResult(new Response<CreateUserResponse>
                    {
                        Message = "User already exit and account not verified"
                    });

                   if (model.Password != model.ConfirmPassword)
                    return await Task.FromResult(new Response<CreateUserResponse>
                    {
                        Message = "Password and confirm password must be the same"
                    });


                    Random rx = new Random();
                    int rand = rx.Next(1000, 2000);
                   ActivationCode = rand.ToString();
                    string UserCode = "";
                    string CustomerCode = CodeGenerator.GenerateNumber(7, Constants.PREFIX_CUSTOMER_CODE);

                    User details = new User();

                    details.Email = model.Email;

                  // activation code supposed to be sent through email service to user email
                  //but in the absence of email service i default activation code for all user to 0000
                  // details.ActivationCode = EncryptionHelper.Encrypt(ActivationCode);
                    details.ActivationCode = EncryptionHelper.Encrypt("0000");
                    details.Name = model.Name;
                    details.Address  = model.Address;
                     details.CustomerNumber = CustomerCode;
                    details.PhoneNumber = model.PhoneNumber;
                    details.Password = EncryptionHelper.Encrypt(model.Password);


                    await _db.users.AddAsync(details);

                 await _db.SaveChangesAsync();
                //  _emailService.Send(model.Email, activationcode);
                
                CreateUserResponse response = new CreateUserResponse();
                response.Email = model.Email;
                response.Name = model.Name;
                response.CustomerNumber = CustomerCode;

                return await Task.FromResult(new Response<CreateUserResponse>
                {
                    Success = true,
                    Message = "User sign up successfully",
                    Data = response
                });

            }

            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return await Task.FromResult(new Response<CreateUserResponse>
                {
                    Message = ex.Message
                });
            }
        }


        public async Task<Response<Task>> ConfirmEmail(ConfirmEmailRequest model)
        {
            var user = _db.users.Where(x => x.Email == model.Email).FirstOrDefault();
            if (user == null)
            {
                return new Response<Task> { Success = false, Message = "User not available" };
            }
            else
            {
                string PlainOtp = EncryptionHelper.Decrypt(user.ActivationCode);
                if (PlainOtp == model.OTP)
                {
                    user.IsEmailConfirmed = true;


                    await _db.SaveChangesAsync();

                    return new Response<Task> { Success = true, Message = "Email Verified sucessfully" };
                }
                else
                {
                    return new Response<Task> { Success = false, Message = "OTP is incorrect" };
                }
            }
        }

        public async Task<Response<AuthenticateResponse>> LoginAsync(AuthenticateRequest request)
        {
            User user = GetClientByUsername(request.Email);
            if (user == null)
                return await Task.FromResult(new Response<AuthenticateResponse>
                {
                    Message = "Declined - User not available"
                });

            if (!VerifyPassword(request.Password, user.Email))

                return await Task.FromResult(new Response<AuthenticateResponse>
                {
                    Message = "Declined - Invalid email or password"
                });

            var responseModel = _mapper.Map<AuthenticateResponse>(await BuildUserModelAsync(user));
            var token = GenerateJSONWebToken(user);

          
            responseModel.Token = new TokenData
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ExpireFrom = DateTime.Now,
                ExpireTo = token.ValidTo,
                ExpireTimeTo = $"{token.ValidTo.ToString("HH:mm:ss")}"
            };


            return await Task.FromResult(new Response<AuthenticateResponse>
            {

                Success = true,
                Message = "login sucessfully",
                Data = responseModel

            });

        }
        private User GetClientByUsername(string email)
        {
            var user = _db.users.Where(x => x.Email == email).FirstOrDefault();

            return user;


        }


        private JwtSecurityToken GenerateJSONWebToken(User client)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);


            //create claims details based on the client information
            var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _config["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("Email", client.Email.ToString())

                    };


            var token = new JwtSecurityToken(_config["JWT:Issuer"],
              _config["JWT:ValidAudience"],
              claims,
              expires: DateTime.UtcNow.AddMinutes(int.Parse(_config["JWT:ExpireIn"])),
              signingCredentials: credentials);

            return token;

        }

        //verify email method
        public bool VerifyPassword(string password, string email)
        {

            User user = GetClientByUsername(email);
            string PlainPassword = EncryptionHelper.Decrypt(user.Password);
            return PlainPassword == password;

        }




        private async Task<UserModel> BuildUserModelAsync(User user)
        {
            try
            {

                var userModel = new UserModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    Name = user.Name,
                    PhoneNumber = user.PhoneNumber,
                    CustomerNumber = user.CustomerNumber
                    
            
                };

                return userModel;
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                throw;
            }
        }



    }
}
