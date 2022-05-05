﻿using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtWithAngular.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace JwtWithAngular.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        // dependency injection
        private readonly IConfiguration _configuration;
        public AuthenticationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Post api/values
        [HttpPost, Route("login")]
        public IActionResult Login([FromBody] LoginModel user)
        {
            if (user == null)
            {
                return BadRequest("Invalid client request");
            }

            string sqlDataSource = _configuration.GetConnectionString("LmsAuthCon");
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand cmd = new SqlCommand("CheckLogin", myConn))
                {
                    cmd.Parameters.AddWithValue("@Username", user.u_name);
                    cmd.Parameters.AddWithValue("@Password", LmsApi.HashPass.hashPass(user.u_pass));
                    cmd.Parameters.AddWithValue("@Role", user.u_role);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    SqlDataReader sdr = cmd.ExecuteReader();

                    if (sdr.Read())
                    {
                        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
                        var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                        var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Name, user.u_name),
                                new Claim(ClaimTypes.Role, user.u_role)
                            };

                        Console.WriteLine(user.u_role);

                        var tokenOptions = new JwtSecurityToken(
                            issuer: "http://localhost:7248",
                            audience: "http://localhost:7248",
                            claims: claims,
                            expires: DateTime.Now.AddMinutes(60),
                            signingCredentials: signinCredentials
                        );

                        var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
                        Console.WriteLine(Ok(new { Token = tokenString }));
                        return Ok(new { Token = tokenString });
                        //return Ok(tokenString);

                    }
                    myConn.Close();
                    return Unauthorized();
                }
            }
        }
    }
}