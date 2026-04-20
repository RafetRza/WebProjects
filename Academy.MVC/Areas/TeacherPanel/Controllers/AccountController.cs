using Academy.MVC.Areas.TeacherPanel.Models;
using Academy.MVC.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Academy.MVC.Areas.TeacherPanel.Controllers
{
    [Area("TeacherPanel")]
    public class AccountController : Controller
    {
        private readonly ApiClient _apiClient;

        public AccountController(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true && User.IsInRole("Teacher"))
            {
                return RedirectToAction("Index", "Group", new { area = "TeacherPanel" });
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var tokenResponse = await _apiClient.PostAsync<TokenResponse>("api/auth/login", new
                {
                    UserName = model.UserName,
                    Password = model.Password
                });

                if (tokenResponse == null || string.IsNullOrEmpty(tokenResponse.Token))
                {
                    ModelState.AddModelError(string.Empty, "Invalid username or password.");
                    return View(model);
                }

                // Parse the JWT token to extract claims
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(tokenResponse.Token);

                var roles = jwtToken.Claims
                    .Where(c => c.Type == ClaimTypes.Role || c.Type == "role")
                    .Select(c => c.Value)
                    .ToList();

                // Check if user has Teacher role
                if (!roles.Contains("Teacher"))
                {
                    return RedirectToAction("AccessDenied");
                }

                // Build claims identity for cookie authentication
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.UserName),
                    new Claim("JwtToken", tokenResponse.Token)
                };

                // Add role claims
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                // Add NameIdentifier if present
                var nameIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier || c.Type == "nameid");
                if (nameIdClaim != null)
                {
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, nameIdClaim.Value));
                }

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal,
                    new AuthenticationProperties
                    {
                        IsPersistent = model.RememberMe,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddHours(2)
                    });

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "Group", new { area = "TeacherPanel" });
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Login failed. Please check your credentials and try again.");
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }

    public class TokenResponse
    {
        public string Token { get; set; } = null!;
    }
}
