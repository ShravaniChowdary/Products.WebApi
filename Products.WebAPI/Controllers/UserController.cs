using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Products.Api.Models;
using Products.WebAPI.Models;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Products.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly AppSettings appSettings;

        public UserController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<AppSettings> options)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.appSettings = options.Value;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Register register)
        {
            try
            {
                var identityUser = new IdentityUser
                {
                    UserName = register.Username,
                    Email = register.Email,
                };
                var response = await userManager.CreateAsync(identityUser, register.Password);
                if (response != null && response.Succeeded)
                {
                    return Ok(new { Message = "Registered successfully, try login!!!" });
                }
                return BadRequest(response?.Errors);
            }
            catch(Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    Message = "An error occured while registering the user",
                    ExceptionDetails = ex.Message
                });
            }
        }

        [HttpPost("add-role/{role}")]
        public async Task<IActionResult> CreateRole(string role)
        {
            try
            {
                if(await roleManager.RoleExistsAsync(role))
                {
                    return BadRequest($"{role} role already exists");
                }
                var response = await roleManager.CreateAsync(new IdentityRole { Name = role });
                if(response != null && response.Succeeded)
                {
                    return Ok(new {Message = $"{role} Role added successfully" });
                }
                return BadRequest(response?.Errors);
            }
            catch(Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    Message = "An error occured while adding the role",
                    ExceptionDetails = ex.Message
                });
            }
        }

        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole([FromBody] UserRole userRole)
        {
            try
            {
                var user = await userManager.FindByNameAsync(userRole.Username);
                if (user != null)
                {
                    var response = await userManager.AddToRoleAsync(user, userRole.Role);
                    if (response.Succeeded)
                    {
                        return Ok($"Successfully {userRole.Role} role assigned to {userRole.Username} user");
                    }
                    return BadRequest(response?.Errors);
                }
                return NotFound($"{userRole.Username} user not exists!");
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    Message = $"An error occured while adding the {userRole.Role} role to {userRole.Username} user",
                    ExceptionDetails = ex.Message
                });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login login)
        {
            try
            {
                var user = await userManager.FindByNameAsync(login.Username);
                if(user != null && await userManager.CheckPasswordAsync(user, login.Password))
                {
                    var userRoles = await userManager.GetRolesAsync(user);
                    var claims = new List<Claim>
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, login.Username),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())   
                    };
                    claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));
                    var token = new JwtSecurityToken(issuer: appSettings.Jwt.Issuer, 
                        claims: claims, 
                        expires: DateTime.Now.AddMinutes(appSettings.Jwt.ExpiryInMinutes),
                        signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.Jwt.Key)), SecurityAlgorithms.HmacSha256));
                    return Ok(new {Token = new JwtSecurityTokenHandler().WriteToken(token)});
                }
                return Unauthorized();
            }
            catch(Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    Message = $"An error occured while logging-in",
                    ExceptionDetails = ex.Message
                });
            }
        }
        
    }
}
