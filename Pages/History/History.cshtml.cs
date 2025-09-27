using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace mario_dsa_rp.Pages;

public class HistoryModel : PageModel
{
    public string? UserId { get; set; }
    public ChallengeHistoryItem[] HistoryItems { get; set; } = []; 
    public List<ChallengeHistoryItem> History { get; set; } = new();

    private readonly IHttpClientFactory _httpClientFactory;

    [BindProperty(SupportsGet = true)] 
    public string Difficulty { get; set; }

    [BindProperty(SupportsGet = true)] 
    public string Mode { get; set; }
    
    public HistoryModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    
    public async Task OnGetAsync()
    {
        UserId = HttpContext.Session.GetString("uid");
        Console.WriteLine("------------------"+ UserId);
        Console.WriteLine("------------------"+ Difficulty);
        Console.WriteLine("------------------"+ Mode);
        if (string.IsNullOrEmpty(UserId))
        {
            RedirectToPage("/Index");
            return;
        }
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync($"http://localhost:5119/api/take-challenge/get-all?difficulty={Difficulty}&mode={Mode}&id={UserId}");
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            Console.WriteLine(json);
            var result = JsonSerializer.Deserialize<ApiResponseWrapper<List<ChallengeHistoryItem>>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (result?.Data != null)
            {
                HistoryItems = result.Data.ToArray();
            }
        }
    }
}

public class ChallengeHistoryItem
{
        public string Id { get; set; }
        public string UserId { get; set; }
        public int ChallengeType { get; set; }
        public int Mode { get; set; }
        public string ChallengeId { get; set; }
        public int SpendTime { get; set; }
        public int MaxScore { get; set; }
        public int TakenScore { get; set; }
        public DateTime TakenAt { get; set; }
}

// wrapper cho response API
public class ApiResponseWrapper<T>
{
    public string Message { get; set; }
    public T Data { get; set; }
}