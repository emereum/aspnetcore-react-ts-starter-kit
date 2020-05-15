using Microsoft.AspNetCore.Mvc;

namespace TemplateProductName.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index() => Json(new { HelloWorld = "TemplateProductName" });
    }
}
