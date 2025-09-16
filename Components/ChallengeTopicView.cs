using Microsoft.AspNetCore.Mvc;

public class ChallengeTopic : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View("ChallengeTopic");
    }
}