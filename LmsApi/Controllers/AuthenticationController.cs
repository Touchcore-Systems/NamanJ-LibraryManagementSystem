using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LmsApi.Data;
using LmsApi.DTO;
using LmsApi.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace LmsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;

        LogRecordHelper logRecord = new();

        public AuthenticationController(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO loginDto)
        {
            string tokenString = string.Empty;
            SymmetricSecurityKey secretKey;
            SigningCredentials signinCredentials;
            List<Claim> claims = new();
            JwtSecurityToken tokenOptions;

            try
            {
                var user = await _context.Users.SingleOrDefaultAsync(
                x => x.UName == loginDto.UName
                && x.UPass == HashPassHelper.hashPass(loginDto.UPass)
                && x.URole == loginDto.URole
                );

                if (user == null)
                {
                    logRecord.LogWriter("Invalid login attempt");
                    return NotFound();
                }

                secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecretKey"]));
                signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, loginDto.UName),
                        new Claim(ClaimTypes.Role, loginDto.URole)
                    };

                tokenOptions = new JwtSecurityToken(
                    issuer: _configuration["TokenUrl"],
                    audience: _configuration["TokenUrl"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["TokenExpiration"])),
                    signingCredentials: signinCredentials
                );

                tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
                logRecord.LogWriter("User logged in and token generated");
                return Ok(new { Token = tokenString });
            }
            catch (Exception ex)
            {
                logRecord.LogWriter(ex.ToString());
                return BadRequest(ex.Message);
            }
        }
    }
}
