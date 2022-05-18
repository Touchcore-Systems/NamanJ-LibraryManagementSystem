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

        LogRecord logRecord = new();

        public AuthenticationController(DataContext context)
        {
            _context = context;
        }
        [HttpPost("login")]
        public async Task<ActionResult<LoginDTO>> Login(LoginDTO loginDto)
        {
            string res = string.Empty;
            var user = await _context.Users.SingleOrDefaultAsync(
                x => x.UName == loginDto.UName
                && x.UPass == HashPass.hashPass(loginDto.UPass)
                && x.URole == loginDto.URole
            );

            if (user == null)
            {
                res = "User unauthenticated";
                logRecord.LogWriter(res);
                return NotFound();
            }

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Secret Key @ 321 #"));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, loginDto.UName),
                        new Claim(ClaimTypes.Role, loginDto.URole)
                    };

            var tokenOptions = new JwtSecurityToken(
                issuer: "http://localhost:7298",
                audience: "http://localhost:7298",
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: signinCredentials
            );

            string tokenString = string.Empty;
            try
            {
                tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
                res = "User logged in and token generated";
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }

            return Ok(new { Token = tokenString });
        }
    }
}
