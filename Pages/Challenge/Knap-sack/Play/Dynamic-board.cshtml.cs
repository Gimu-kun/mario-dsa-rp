using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;     // Cho JsonSerializer
using System.Text;          // Cho Encoding.UTF8
using System.Net.Http;      // Cho StringContent
using mario_dsa_rp.Models;

namespace mario_dsa_rp.Namespace
{
    public class DynamicBoardModel(IHttpClientFactory httpClientFactory) : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

        public ChallengeDto Challenge { get; set; }
        
        public List<TakenDto> Taken { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Difficult { get; set; }
        [BindProperty(SupportsGet = true)]
        public string Mode { get; set; }
        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }

        [BindProperty]
        public int countDown { get; set; } = 3;

        public int challengeTimer { get; set; } = 0;

        public string userId;
        
        public async Task OnGet()
        {
            userId = HttpContext.Session.GetString("uid");
            await getChallenge();
        }
        public async Task getChallenge()
        {
            var client = _httpClientFactory.CreateClient();
            var userIds = new string[] { userId };
            var jsonContent = JsonSerializer.Serialize(userIds);
            var json = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            try
            {
                var response = await client.PostAsync($"http://localhost:5119/api/challenge/generate?type=dp_board&difficulty={Difficult}&mode={Mode}&id={Id}", json);
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true // <-- đây là chìa khóa
                    };
                    var challengeResponse = JsonSerializer.Deserialize<ChallengeResponseDto>(jsonString, options);
                    Challenge = challengeResponse?.Challenge;
                    Taken =  challengeResponse?.Taken;
                    Console.WriteLine("Calling getChallenge with userId = " + userId);
                    Console.WriteLine("Response status: " + response.StatusCode);
                    Console.WriteLine("Response content: " + jsonString);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed----------------------------");
                Console.WriteLine("lỗi : " + ex);
            }
        }
    }
}