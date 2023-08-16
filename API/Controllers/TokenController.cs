using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Azure.Identity;
using JWT.Api.Data;
using JWT.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace JWT.Api.Controllers;

[ApiController]
[Route("api/{controller}")]
public class TokenController : ControllerBase
{
  private readonly DatabaseContext _dbcontext;
  private readonly IConfiguration _config;

  public TokenController(DatabaseContext dbcontext, IConfiguration config)
  {
    _dbcontext = dbcontext;
    _config = config;
  }

  [HttpPost]  
  public async Task<IActionResult> Post(UserInfo userInfo)
  {
    if (await IsValidUser(userInfo))
    {
      var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _config["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                       // new Claim("UserId", userInfo.UserId.ToString()),
                        //new Claim("DisplayName", userInfo.DisplayName),
                        //new Claim("UserName", userInfo.UserName),
                        new Claim("Email", userInfo.Email)
                    };

      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
      var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
      var token = new JwtSecurityToken(
          _config["Jwt:Issuer"],
          _config["Jwt:Audience"],
          claims,
          expires: DateTime.UtcNow.AddMinutes(10),
          signingCredentials: signIn);

      return Ok(new JwtSecurityTokenHandler().WriteToken(token));
    }
    else
    {
      return BadRequest("Invalid credentials");
    }
  }
          
  private async Task<bool> IsValidUser(UserInfo userInfo)
  {
    if (userInfo?.Email != null && userInfo?.Password != null)
    {
      return await IsUserExist(userInfo);
    }
    return false;
  }

    private async Task<bool> IsUserExist(UserInfo user)
    {
        return await _dbcontext.UserInfos.AnyAsync(u => u.Email == user.Email && u.Password == user.Password);
    }
}