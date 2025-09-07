using Microsoft.AspNetCore.Mvc;

public class HeaderViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View("Header");
    }
}