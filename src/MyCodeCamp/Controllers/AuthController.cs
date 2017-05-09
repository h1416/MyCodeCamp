using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyCodeCamp.Data;
using MyCodeCamp.Data.Entities;
using MyCodeCamp.Filter;
using MyCodeCamp.Models;
using System;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
                        var claims = new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, user.UserName), // sub is username
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // Jti is a new Guid for uniqueness
                        };

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("VERYLONGKEYVALUETHATISSUCURE"));
                        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                        var token = new JwtSecurityToken(
                            issuer: "http://mycodecamp.org", // the website that generate the token
                            audience: "http://mycodecamp.org", // the website that going to accept the token
                            claims: claims,
                            expires: DateTime.UtcNow.AddMinutes(15),
                            signingCredentials: creds
                            );
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
