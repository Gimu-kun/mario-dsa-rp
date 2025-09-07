using Microsoft.AspNetCore.Mvc;

public class HeroBannerViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View("HeroBanner");
    }
}