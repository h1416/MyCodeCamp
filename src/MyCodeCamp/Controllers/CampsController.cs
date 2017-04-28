using Microsoft.AspNetCore.Mvc;

namespace MyCodeCamp.Controllers
{
    public class CampsController : Controller
    {

        public IActionResult Get()
        {
            return Ok(new { Name = "HV", FavoriteColor = "Blue" });
        }
        
    }
}
