using Microsoft.AspNetCore.Mvc;

public class TopicFeatureViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View("TopicFeature");
    }
}