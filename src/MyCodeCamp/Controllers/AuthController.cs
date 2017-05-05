using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyCodeCamp.Data;
using MyCodeCamp.Data.Entities;
using MyCodeCamp.Filter;
using MyCodeCamp.Models;
using System;

namespace MyCodeCamp.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private CampContext _context;
        private ILogger<AuthController> _logger;
        private SignInManager<CampUser> _signInMgr;

        public AuthController(CampContext context, SignInManager<CampUser> signInMgr, ILogger<AuthController> logger)
        {
            _context = context;
            _signInMgr = signInMgr;
            _logger = logger;
        }

        [HttpPost("/login")]
        [ValidateModel]        
        public ActionResult Login([FromBody]CredentialModel model)
        {
            try
            {

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception thrown while logging in {ex}");
            }

            return BadRequest("Failed to login");
        }
    }
}
