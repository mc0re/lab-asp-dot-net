using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace SecureApiLab
{
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly SignInManager<ApiUser> mSignInManager;
        private readonly UserManager<ApiUser> mUserManager;
        private readonly IPasswordHasher<ApiUser> mPwdHasher;
        private readonly IConfiguration mConfig;
        private readonly ILogger<AuthController> mLogger;


        public AuthController(
            SignInManager<ApiUser> signInManager,
            UserManager<ApiUser> userManager,
            IPasswordHasher<ApiUser> pwdHasher,
            IConfiguration config,
            ILogger<AuthController> logger)
        {
            mSignInManager = signInManager;
            mUserManager = userManager;
            mPwdHasher = pwdHasher;
            mConfig = config;
            mLogger = logger;
        }


        /// <summary>
        /// Uses cookie-based authentication.
        /// </summary>
        /// <param name="model">User name and password</param>
        [ValidateModel]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] CredentialModel model)
        {
            try
            {
                var res = await mSignInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);
                if (res.Succeeded)
                    return Ok();
            }
            catch (Exception ex)
            {
                mLogger.LogError(ex.Message);
            }

            return BadRequest("Failed to login");
        }


        [ValidateModel]
        [HttpPost("token")]
        public async Task<IActionResult> CreateToken([FromBody] CredentialModel model)
        {
            try
            {
                var user = await mUserManager.FindByNameAsync(model.UserName);

                if (user != null)
                {
                    if (mPwdHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password) == PasswordVerificationResult.Success)
                    {
                        var userClaims = await mUserManager.GetClaimsAsync(user);

                        var claims = new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim(JwtRegisteredClaimNames.GivenName, user.UserName)
                        }.Union(userClaims);

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(mConfig["Tokens:Key"]));
                        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                        var token = new JwtSecurityToken(
                            issuer: mConfig["Tokens:Site"],
                            audience: mConfig["Tokens:Site"],
                            claims: claims, expires: DateTime.UtcNow.AddMinutes(15),
                            signingCredentials: creds);

                        return Ok(new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                mLogger.LogError(ex.Message);
            }

            return BadRequest("Failed to generate a token");
        }
    }
}
