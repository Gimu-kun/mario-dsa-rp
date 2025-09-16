using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;

namespace mario_dsa_rp.Namespace
{
    public class AuthModel(IHttpClientFactory httpClientFactory) : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

        [BindProperty(SupportsGet = true)]
        public string View { get; set; }

        [BindProperty]
        public string? Username { get; set; }
        [BindProperty]
        public string? Passwords { get; set; }
        [BindProperty]
        public string? FullName { get; set; }

        public string? Message { get; set; } = null;

        public void OnGet()
        {
            if (TempData["Message"] is string tempMessage)
            {
                Message = tempMessage;
            }
        }

        // Handler đăng ký
        public async Task<IActionResult> OnPostRegisterAsync()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Passwords) || string.IsNullOrWhiteSpace(FullName))
            {
                Message = "Vui lòng nhập đầy đủ thông tin đăng ký.";
                View = "register";
                return Page();
            }

            var client = _httpClientFactory.CreateClient();
            var payload = new
            {
                Username,
                FullName,
                Passwords
            };

            var json = new StringContent(
                JsonSerializer.Serialize(payload),
                Encoding.UTF8,
                "application/json"
            );

            var response = await client.PostAsync("http://localhost:5119/api/users/register", json);

            if (response.IsSuccessStatusCode)
            {
                Username = "";
                Passwords = "";
                FullName = "";
                TempData["Message"] = "Đăng ký thành công!";
                return RedirectToPage("Auth", new { view = "login" });
            }
            else
            {
                TempData["Message"] = "Đăng ký thất bại.";
                return RedirectToPage("Auth", new { view = "register" });
            }
            
        }

        // Handler đăng nhập
        public async Task<IActionResult> OnPostLoginAsync()
        {
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Passwords))
            {
                Message = "Tài khoản hoặc mật khẩu không được để trống.";
                View = "login";
                return Page();
            }

            var client = _httpClientFactory.CreateClient();

            var payload = new
            {
                username = Username,
                passwords = Passwords
            };

            var json = new StringContent(
                JsonSerializer.Serialize(payload),
                Encoding.UTF8,
                "application/json"
            );

            var response = await client.PostAsync("http://localhost:5119/api/users/login", json);

            if (response.IsSuccessStatusCode)
            {
                // Đọc token từ response body
                var token = await response.Content.ReadAsStringAsync();

                // Lưu token vào Session hoặc Cookie
                HttpContext.Session.SetString("JwtToken", token);
                return RedirectToPage("/Index"); // Đây là page chính, có dấu /
            }
            else
            {
                TempData["Message"] = "Sai tài khoản hoặc mật khẩu.";
                return RedirectToPage("Auth", new { view = "login" });
            }
        }
    }
}
