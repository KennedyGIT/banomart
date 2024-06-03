using Banomart.Services.AuthAPI.Data;
using Banomart.Services.AuthAPI.Models;
using Banomart.Services.AuthAPI.Models.DTOs;
using Banomart.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Identity;

namespace Banomart.Services.AuthAPI.Service
{
    public class AuthService : IAuthService
    {
        private readonly DatabaseContext db;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IJwtTokenGenerator tokenGenerator;

        public AuthService(
            DatabaseContext db,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IJwtTokenGenerator tokenGenerator)
        {
            this.db = db;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.tokenGenerator = tokenGenerator;
        }

        public async Task<bool> AssignRole(string userName, string roleName)
        {
            var user = db.ApplicationUsers.FirstOrDefault(x => x.UserName == userName.ToLower());

            if (user != null) 
            {
                if (!roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult()) 
                {
                    roleManager.CreateAsync(new IdentityRole(roleName));
                }

                await userManager.AddToRoleAsync(user, roleName);

                return true;
            }

            return false;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = db.ApplicationUsers.FirstOrDefault( u => u.UserName.ToLower() == loginRequestDto.UserName.ToLower());

            bool isValid = await userManager.CheckPasswordAsync(user, loginRequestDto.Password);

            if (user == null || isValid == false)
            {
                return new LoginResponseDto() { User = null, Token = "" };
            }

            var token = tokenGenerator.GenerateToken(user);

            UserDto userDto = new()
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber
            };

            LoginResponseDto responseDto = new()
            {
                User = userDto,
                Token = token
            };

            return responseDto;
        }

        public async Task<string> RegisterUser(RegistrationDto registrationRequestDto)
        {
            ApplicationUser user = new()
            {
                UserName = registrationRequestDto.UserName.ToLower(),
                Email = registrationRequestDto.Email.ToLower(),
                NormalizedEmail = registrationRequestDto.Email.ToUpper(),
                FirstName = registrationRequestDto.FirstName.ToUpper(),
                LastName = registrationRequestDto.LastName.ToUpper(),
                PhoneNumber = registrationRequestDto.PhoneNumber
            };

            try 
            {
                var result = await userManager.CreateAsync(user, registrationRequestDto.Password);

                if (result.Succeeded)
                {
                    var createdUser = db.ApplicationUsers.First(u => u.Email == registrationRequestDto.Email.ToLower());

                    UserDto userDto = new()
                    {
                        Email = createdUser.Email,
                        Username = createdUser.UserName,
                        Id = createdUser.Id,
                        FirstName = createdUser.FirstName,
                        LastName = createdUser.LastName,
                        PhoneNumber = createdUser.PhoneNumber
                    };

                    return "";
                }
                else 
                {
                    return result.Errors.FirstOrDefault().Description;
                }
            }
            catch(Exception ex) 
            {

            }

            return "Error Encountered";
        }
    }
}
