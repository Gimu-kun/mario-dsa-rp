using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyApp.Namespace
{
    public class AuthModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string View { get; set; } = "login"; 
        public void OnGet()
        {
            
        }
    }
}
