using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyCodeCamp.Data;
using MyCodeCamp.Data.Entities;
using MyCodeCamp.Filter;
using MyCodeCamp.Models;
using System;
using System.Threading.Tasks;

namespace MyCodeCamp.Controllers
{
    public class AuthController : Controller
    {
        private CampContext _context;
        private ILogger<AuthController> _logger;
        private SignInManager<CampUser> _signInMgr;
        private UserManager<CampUser> _userMgr;
        private IPasswordHasher<CampUser> _hasher;

        public AuthController(CampContext context,
            SignInManager<CampUser> signInMgr,
            UserManager<CampUser> userMgr,
            IPasswordHasher<CampUser> hasher,
            ILogger<AuthController> logger)
        {
            _context = context;
            _signInMgr = signInMgr;
            _logger = logger;
            _userMgr = userMgr;
            _hasher = hasher;
        }

        [HttpPost("api/auth/login")]
        [ValidateModel]        
        public async Task<ActionResult> Login([FromBody]CredentialModel model)
        {
            try
            {
                var result = await _signInMgr.PasswordSignInAsync(model.UserName, model.Password, false, false);
                if (result.Succeeded)
                {
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception thrown while logging in {ex}");
            }

            return BadRequest("Failed to login");
        }

        [ValidateModel]
        [HttpPost("api/auth/login")]
        public async Task<ActionResult> CreateToken([FromBody]CredentialModel model)
        {
            try
            {
                var user = await _userMgr.FindByNameAsync(model.UserName);
                if (user != null)
                {
                    if (_hasher.VerifyHashedPassword(user, user.PasswordHash, model.Password) == PasswordVerificationResult.Success)
                    {

                    }

                }
            }
            catch (Exception ex)
            {

                _logger.LogError($"Exception thown while creating JWT: {ex}");
            }

            return BadRequest("Fail to generate token");
        }
    }
}
