using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

public class HeaderViewComponent : ViewComponent
{
    private readonly IHttpClientFactory _httpClientFactory;

    public HeaderViewComponent(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var isLogin = false;
        string? fullName = null;

        var token = HttpContext.Session.GetString("JwtToken");
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
                    isLogin = true;
                    fullName = result.FullName;
                    HttpContext.Session.SetString("uid",result.UserId);
                }
            }
        }

        return View(new HeaderViewModel
        {
            IsLogin = isLogin,
            FullName = fullName
        });
    }
}
public class HeaderViewModel
{
    public bool IsLogin { get; set; }
    public string? FullName { get; set; }
}

public class VerifyTokenResult
{
    public bool Success { get; set; }
    public string UserId { get; set; }
    public string? Username { get; set; }
    public string? FullName { get; set; }
    public string? Message { get; set; }
}