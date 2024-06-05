using bano_mart_mvc.Models;
using bano_mart_mvc.Service.IService;
using bano_mart_mvc.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices;
using System.Security.Claims;

namespace bano_mart_mvc.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService authService;
        private readonly ITokenProvider tokenProvider;

        public AuthController(IAuthService authService, ITokenProvider tokenProvider)
        {
            this.authService = authService;
            this.tokenProvider = tokenProvider;
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDto loginRequestDto = new();
            return View(loginRequestDto);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto loginRequestDto)
        {

            ResponseDto responseDto = await authService.LoginAsync(loginRequestDto);


            if (responseDto != null && responseDto.IsSuccessful)
            {
                LoginResponseDto loginResponseDto =
                     JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(responseDto.Result));

                tokenProvider.SetToken(loginResponseDto.Token);

                await SignInUser(loginResponseDto);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["error"] = responseDto.Message;
                ModelState.AddModelError("CustomError", responseDto.Message);
                return View(loginRequestDto);
            }

        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            tokenProvider.ClearToken();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            var roleList = new List<SelectListItem>()
            {
                new SelectListItem{Text = Enums.Roles.CUSTOMER.ToString(), Value = Enums.Roles.CUSTOMER.ToString()},
                new SelectListItem{Text = Enums.Roles.ADMIN.ToString(), Value = Enums.Roles.ADMIN.ToString()}
            };

            ViewBag.RoleList = roleList;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationDto registrationDto)
        {
            string errorMessage = string.Empty;

            ResponseDto result = await authService.RegisterAsync(registrationDto);

            ResponseDto assignRoleResult = new ResponseDto();

            AssignRoleDto assignRoleDto = new AssignRoleDto();

            if (result != null && result.IsSuccessful)
            {
                if (string.IsNullOrEmpty(registrationDto.Role))
                {
                    assignRoleDto.RoleName = Enums.Roles.CUSTOMER.ToString();
                    assignRoleDto.UserName = registrationDto.UserName;
                }

                if (registrationDto.Role != null && registrationDto.Role == "ADMIN")
                {
                    assignRoleDto.RoleName = Enums.Roles.ADMIN.ToString();
                    assignRoleDto.UserName = registrationDto.UserName;
                }
                else if (registrationDto.Role != null)
                {
                    assignRoleDto.RoleName = Enums.Roles.CUSTOMER.ToString();
                    assignRoleDto.UserName = registrationDto.UserName;
                }

                assignRoleResult = await authService.AssignRoleAsync(assignRoleDto);

                if (assignRoleResult != null && assignRoleResult.IsSuccessful)
                {
                    TempData["success"] = "User registration successful";

                    return RedirectToAction(nameof(Login));
                }
                else
                {
                    errorMessage = assignRoleResult.Message;
                }
            }
            else
            {
                errorMessage = result.Message;
            }

            var roleList = new List<SelectListItem>()
            {
                new SelectListItem{Text = Enums.Roles.CUSTOMER.ToString(), Value = Enums.Roles.CUSTOMER.ToString()},
                new SelectListItem{Text = Enums.Roles.ADMIN.ToString(), Value = Enums.Roles.ADMIN.ToString()}
            };

            ViewBag.RoleList = roleList;

            TempData["error"] = errorMessage;

            return View(registrationDto);
        }

        private async Task SignInUser(LoginResponseDto loginResponseDto)
        {
            var handler = new JwtSecurityTokenHandler();

            var jwt = handler.ReadJwtToken(loginResponseDto.Token);

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email, jwt.Claims.FirstOrDefault(u => u.Type == "email")?.Value));

            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, jwt.Claims.FirstOrDefault(u => u.Type == "subject")?.Value));

            identity.AddClaim(new Claim(JwtRegisteredClaimNames.FamilyName, jwt.Claims.FirstOrDefault(u => u.Type == "lastName")?.Value));

            identity.AddClaim(new Claim(JwtRegisteredClaimNames.GivenName, jwt.Claims.FirstOrDefault(u => u.Type == "firstName")?.Value));

            identity.AddClaim(new Claim(ClaimTypes.Name, jwt.Claims.FirstOrDefault(u => u.Type == "firstName")?.Value));

            identity.AddClaim(new Claim(ClaimTypes.Role, jwt.Claims.FirstOrDefault(u => u.Type == "role")?.Value));



            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
    }
}
