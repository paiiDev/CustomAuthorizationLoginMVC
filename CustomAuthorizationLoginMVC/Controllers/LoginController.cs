using CustomAuthorizationLoginMVC.Domain.DTOs;
using CustomAuthorizationLoginMVC.Domain.Features.Login;
using Microsoft.AspNetCore.Mvc;

namespace CustomAuthorizationLoginMVC.Controllers
{
    public class LoginController : Controller
    {
        private readonly LoginService _loginService;
        private readonly ILogger<LoginController> _logger;
        public LoginController(LoginService loginService, ILogger<LoginController> logger)
        {
            _loginService = loginService;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginRequestDto request)
        {
            _logger.LogInformation("Login attempt for user: {Username}", request.Username);
            var response = await _loginService.LoginAsync(request);
            if(response is null)
            {
                return Redirect("/Login"); // Redirect to login page on failure
            }

            var options = new CookieOptions
            {
                Expires = response.SessionExpired,
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax
            };

            var token = response.UserId + ":" + response.SessionId; 

            HttpContext.Response.Cookies.Delete("Authorization");
            HttpContext.Response.Cookies.Append("Authorization", token, options);
            return Redirect("/Home"); // Redirect to home page on successful login
        }

    }
}
