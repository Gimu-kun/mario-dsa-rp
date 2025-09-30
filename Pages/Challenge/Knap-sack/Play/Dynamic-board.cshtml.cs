using System.Text.Json;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace mario_dsa_rp.Namespace
{
    public class DynamicBoardModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public string? UserId { get; set; }
        public string? FullName { get; set; }
        public string? HostId { get; set; }
        public string? Mode { get; set; }
        public string? Difficult { get; set; }
        public string? Id { get; set; }
        public string? roomId { get; set; } 
        public int challengeTimer { get; set; } = 0;

        public DynamicBoardModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // NOTE: đổi về Task (không dùng async void). Nhận cả id (challenge id) và roomId (phòng) từ querystring
        public async Task OnGetAsync(string? mode, string? difficult, string? id, string? roomId = null)
        {
            Mode = mode;
            Difficult = difficult;
            Id = id;             // đây là challenge id (đề) từ querystring ?id=...
            this.roomId = roomId; // đây là room id (nếu có) từ querystring ?roomId=...

            // session info (nếu có)
            HostId = HttpContext.Session.GetString("hostId");
            UserId = HttpContext.Session.GetString("uid");

            await GetUserInfo(); // lấy thêm từ token nếu có
        }

        private async Task GetUserInfo()
        {
            var token = HttpContext.Session.GetString("JwtToken");
            if (string.IsNullOrEmpty(token)) return;

            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync($"http://localhost:5119/api/users/token-verify/{token}");
                if (!response.IsSuccessStatusCode) return;

                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<VerifyTokenResult>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (result?.Success == true)
                {
                    FullName = result.FullName;
                    UserId = result.UserId;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public class VerifyTokenResult
        {
            public bool Success { get; set; }
            public string UserId { get; set; } = "";
            public string FullName { get; set; } = "";
        }
    }
}
