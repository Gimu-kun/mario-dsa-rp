using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Text.Json;
using mario_dsa_rp.Models;
using Microsoft.AspNetCore.Mvc;

namespace mario_dsa_rp.Pages.Ranking
{
    public class RankingApiResponse
    {
        public string Message { get; set; } = "";
        public RankingResDto[] Data { get; set; } = [];
    }

    public class RankingResDto
    {
        public string? UserId { get; set; }
        public string Username { get; set; } = "";
        public int Exp { get; set; }
        public int? Score { get; set; }
        public int Ranking { get; set; }
        
        public string ChallengeId  { get; set; } = "";
        public int SpendTime { get; set; }
        public string BadgeId { get; set; }
        public DateTime TakenAt { get; set; }
    }
    
    public class RankingModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;
    public string? uid = null;
    public RankingResDto? currentUserRank = null;
    public RankingResDto[] RankingList = [];

    [BindProperty(SupportsGet = true)]
    public required string Mode { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? Diff { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? Playmode { get; set; }

    public RankingModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task OnGetAsync()
    {
        var client = _httpClientFactory.CreateClient();
        getUserId();

        if (!string.IsNullOrEmpty(Mode))
        {
            try
            {
                var url = $"http://localhost:5119/api/ranking/{Mode}";
                if (!string.IsNullOrEmpty(Diff) && !string.IsNullOrEmpty(Playmode))
                {
                    url += $"?difficulty={Diff}&mode={Playmode}";
                }

                var res = await client.GetAsync(url);
                if (res.IsSuccessStatusCode)
                {
                    var jsonString = await res.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var rankingResponse = JsonSerializer.Deserialize<RankingApiResponse>(jsonString, options);

                    if (rankingResponse?.Data != null)
                    {
                        RankingList = rankingResponse.Data;
                        currentUserRank = RankingList.FirstOrDefault(r => r.UserId == uid);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }

    public void getUserId()
    {
        var userId = HttpContext.Session.GetString("uid");
        if (userId != null)
        {
            uid = userId;
        }
    }
}
}
