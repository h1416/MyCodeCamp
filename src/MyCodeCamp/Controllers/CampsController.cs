using Microsoft.AspNetCore.Mvc;

namespace MyCodeCamp.Controllers
{
    public class CampsController : Controller
    {
        [HttpGet("api/camps")]
        public IActionResult Get()
        {
            return Ok(new { Name = "HV", FavoriteColor = "Blue" });
        }
        
    }
}
