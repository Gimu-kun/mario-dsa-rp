using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace mario_dsa_rp.Namespace
{
    public class ChooseItemModel(IHttpClientFactory httpClientFactory) : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

        [BindProperty(SupportsGet = true)]
        public string Difficult { get; set; }
        [BindProperty(SupportsGet = true)]
        public string Mode { get; set; }
        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }

        [BindProperty]
        public int countDown { get; set; } = 3;

        public int Timer { get; set; }
        public async Task OnGet()
        {
            letCountDown();
        }

        public async Task letCountDown()
        {
            while (countDown >= 0)
            {
                await Task.Delay(1000);
                countDown--;
            }
        }

    }
}