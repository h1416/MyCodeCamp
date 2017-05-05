using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyCodeCamp.Data;
using MyCodeCamp.Data.Entities;

namespace MyCodeCamp.Controllers
{
    public class AuthController : Controller
    {
        private CampContext _context;
        private SignInManager<CampUser> _signInMgr;

        public AuthController(CampContext context, SignInManager<CampUser> signInMgr)
        {
            _context = context;
            _signInMgr = signInMgr;
        }
    }
}
