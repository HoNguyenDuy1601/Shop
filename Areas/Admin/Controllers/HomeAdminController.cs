using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Shop.Data;
using Shop.Areas.Admin.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http.Headers;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Logging;
using NuGet.Protocol;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Principal;

namespace Shop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeAdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IConfiguration _config;
        private IHttpContextAccessor _httpContextAccessor;

        public HomeAdminController(ApplicationDbContext context, IConfiguration config, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _config = config;
            _httpContextAccessor = httpContextAccessor;
        }

        [Authorize]
        public IActionResult Index()
        {
            /*            if (HttpContext.Session.GetString("userName") != null)
                        {
                            return View();
                        }
                        else
                        {
                            return RedirectToAction("Login");
                        }*/
            object Number = new
            {
                NumberOfProduct = _context.Products.Count(),
                NumberOfPost = _context.Posts.Count(),
                NumberOfStaff = _context.Staffs.Count()
            };
            ViewBag.Number = Number;

            return View();
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }


        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
/*            var Account = _context.Staffs
                                .SingleOrDefault(m => m.Username.ToLower() == username.ToLower() && m.Password == password);*/

/*            if (Account != null)
            {
                HttpContext.Session.SetString("userName", Account.Username);
                HttpContext.Session.SetInt32("idAccount", Account.Id);
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = "Username or password is incorrect";
                return View();
            }*/
            IActionResult response = Unauthorized();
            
            var user = AuthenticateUser(username, password);
            if(user != null)
            {
                var tokenStr = GenerateJSONWebToken(user);
                response = Ok(new { token = tokenStr });
                var userPrincipal = this.ValidateToken(tokenStr);
                var authProperties = new AuthenticationProperties
                {
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                    IsPersistent = false
                };
                await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        userPrincipal,
                        authProperties
                        );
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = "Username or password is incorrect";
                return View();
            }
/*            return response;*/
        }

        public async Task<IActionResult> Logout()
        {
            /*            HttpContext.Session.Remove("userName");
            */            // Thoat Form Authen not done
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        private UserLoginModel AuthenticateUser(string username, string password)
        {
            UserLoginModel user = null;
            var Account = _context.Staffs
                               .SingleOrDefault(m => m.Username.ToLower() == username.ToLower() && m.Password == password);

            if (Account != null)
            {
                user = new UserLoginModel(Account.Username, Account.Password, Account.Id);
            }

            return user;
        }

        private string GenerateJSONWebToken(UserLoginModel userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, userInfo.username),
                new Claim(ClaimTypes.Role, userInfo.Id.ToString())
/*              new Claim(JwtRegisteredClaimNames.Name,userInfo.name),
*/            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            var encodetoken = new JwtSecurityTokenHandler().WriteToken(token);
            return encodetoken; 
        }

        private ClaimsPrincipal ValidateToken(string jwtToken)
        {
            IdentityModelEventSource.ShowPII = true;
            SecurityToken validatedToken;
            TokenValidationParameters validationParameters = new TokenValidationParameters();

            validationParameters.ValidateLifetime = true;
            validationParameters.ValidAudience = _config["Jwt:Issuer"];
            validationParameters.ValidIssuer = _config["Jwt:Issuer"];
            validationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

            ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out validatedToken);
            return principal;
        }
    }
}
