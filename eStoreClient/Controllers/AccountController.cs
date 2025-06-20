using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Threading.Tasks;
using BusinessObject.Models;
using System.Text.Json;

namespace eStoreClient.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public AccountController(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new System.Uri("https://localhost:7191/api/"); // TODO: move to config
        }

        [HttpGet]
        public IActionResult Login()
        {
            // Nếu đã đăng nhập rồi thì chuyển về Product
            if (HttpContext.Session.GetString("Email") != null)
            {
                return RedirectToAction("Index", "Product");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            string adminEmail = _configuration["AdminAccount:Email"];
            string adminPassword = _configuration["AdminAccount:Password"];

            if (email == adminEmail && password == adminPassword)
            {
                HttpContext.Session.SetString("Email", email);
                HttpContext.Session.SetString("Role", "Admin");
                TempData["LoginMessage"] = "Login successful as Admin";

                return RedirectToAction("Index", "Product");
            }
            else
            {
                var response = await _httpClient.PostAsync($"members/login?email={email}&password={password}", null);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var member = JsonSerializer.Deserialize<Member>(responseContent, options);

                    HttpContext.Session.SetString("Email", member.Email);
                    HttpContext.Session.SetString("MemberId", member.MemberId.ToString());
                    HttpContext.Session.SetString("Role", "Member");
                    TempData["LoginMessage"] = "Login successful";

                    return RedirectToAction("Index", "Product");
                }
                else
                {
                    ViewBag.Error = "Invalid email or password.";
                    return View();
                }
            }
        }

        [HttpPost]
        public IActionResult Logout()
        {
            // Xóa session khi logout
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
