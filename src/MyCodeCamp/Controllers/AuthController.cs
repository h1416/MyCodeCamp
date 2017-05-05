using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyCodeCamp.Data;
using MyCodeCamp.Data.Entities;
using MyCodeCamp.Filter;
using MyCodeCamp.Models;

namespace MyCodeCamp.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private CampContext _context;
        private SignInManager<CampUser> _signInMgr;

        public AuthController(CampContext context, SignInManager<CampUser> signInMgr)
        {
            _context = context;
            _signInMgr = signInMgr;
        }

        [ValidateModel]
        [HttpPost("/login")]
        public ActionResult Login([FromBody]CredentialModel model)
        {
        }
    }
}
