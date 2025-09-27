using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace mario_dsa_rp.Views.Shared.Components.Header
{
    public class HeaderViewComponent : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HeaderViewComponent(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new HeaderViewModel(_httpClientFactory);
            await model.LoadAsync(HttpContext);
            return View("Header", model);
        }
    }

    public class HeaderViewModel
    {
        public bool IsLogin { get; set; } = false;
        public string? FullName { get; set; } = null;

        private readonly IHttpClientFactory _httpClientFactory;

        public HeaderViewModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task LoadAsync(Microsoft.AspNetCore.Http.HttpContext httpContext)
        {
            string? token = httpContext.Session.GetString("JwtToken");
            if (!string.IsNullOrEmpty(token))
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync($"http://localhost:5119/api/users/token-verify/{token}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<VerifyTokenResult>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (result?.Success == true)
                    {
                        IsLogin = true;
                        FullName = result.FullName;
                        httpContext.Session.SetString("uid", result.UserId);
                    }
                }
            }
        }
    }

    public class VerifyTokenResult
    {
        public bool Success { get; set; }
        public string UserId { get; set; } = null!;
        public string? Username { get; set; }
        public string? FullName { get; set; }
        public string? Message { get; set; }
    }
}
