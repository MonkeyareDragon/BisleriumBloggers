using Domain.BisleriumBlog;
using Domain.BisleriumBlog.Model;
using Infrastructure.BisleriumBlog.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.Drawing.Printing;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using static Domain.BisleriumBlog.View_Model.AuthenticationViewModel;

namespace Presentation.BisleriumBlog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly EmailService _emailService;

        public record LoginResponse(bool Flag, string Id, string Name, string Email, string Token, string Role, String Message);
        public record UserSession(string? Id, string? Name, string? Email, string? Role);

        public AccountController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, SignInManager<AppUser> signInManager, EmailService emailService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        //Generate Jwt Token
        private string GenerateToken(UserSession user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var userClaims = new[]
            {
              new Claim(ClaimTypes.NameIdentifier, user.Id),
              new Claim(ClaimTypes.Name, user.Name),
              new Claim(ClaimTypes.Email, user.Email),
              new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(
              issuer: _configuration["Jwt:Issuer"],
              audience: _configuration["Jwt:Audience"],
              claims: userClaims,
              expires: DateTime.Now.AddDays(1),
              signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new AppUser { UserName = model.Username, Email = model.Email, CreatedAt = DateTime.Now, Address = model.Address, Gender = model.Gender };

            // Check if the specified role exists
            var roleExists = await _roleManager.RoleExistsAsync(model.Role);
            if (!roleExists)
            {
                // If the role doesn't exist, return error
                return BadRequest("Invalid role specified.");
            }

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                // Assign the specified role to the user
                await _userManager.AddToRoleAsync(user, model.Role);
                return Ok("User registered successfully.");
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginViewModel loginUser)
        {
            var result = await _signInManager.PasswordSignInAsync(loginUser.Username, loginUser.Password, false, lockoutOnFailure: false);
            Debug.WriteLine(result);
            if (result.Succeeded)
            {
                var getUser = await _userManager.FindByEmailAsync(loginUser.Email);
                var getUserRole = await _userManager.GetRolesAsync(getUser);
                var userSession = new UserSession(getUser.Id, getUser.UserName, getUser.Email, getUserRole.First());
                string token = GenerateToken(userSession);

                return Ok(new LoginResponse(true, getUser.Id, getUser.UserName, getUser.Email, token!, getUserRole.First(), "Login completed"));
            }
            else
            {
                return NotFound(new LoginResponse(false, null, null, null, null!, null, "Invalid email or password"));
            }
        }

        // Base64 URL encode method
        private string Base64UrlEncode(string input)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            return WebEncoders.Base64UrlEncode(bytes);
        }

        // Base64 URL decode method
        private string Base64UrlDecode(string input)
        {
            byte[] bytes = WebEncoders.Base64UrlDecode(input);
            return Encoding.UTF8.GetString(bytes);
        }

        [HttpGet("forgot-password/{email}")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            
            // Generate the password reset token
            string token = await _userManager.GeneratePasswordResetTokenAsync(user);

            // Encode the token
            string encodedToken = Base64UrlEncode(token);

            // Construct the reset password URL
            string resetUrl = $"{_configuration["Url"]}/password-change/{user.Id}/{encodedToken}";
            _emailService.SendEmailAsync(email, "Your Password Change Link.", _emailService.FormatPasswordResetEmail(resetUrl));

            // Send the token to the user's email
            // For demonstration purposes, we'll just return the token and user id
            return Ok(new { resetUrl , encodedToken });
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ResetPassword(string userId, string token, [FromBody] ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            string decodedToken = Base64UrlDecode(token);

            // Reset password
            var result = await _userManager.ResetPasswordAsync(user, decodedToken, model.NewPassword);
            if (result.Succeeded)
            {
                return Ok("Password reset successfully.");
            }

            return BadRequest(result.Errors);
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            return Ok(users);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return Ok("User deleted successfully.");
            }

            return BadRequest(result.Errors);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateUser(string id, UpdateUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            user.Email = model.Email;
            user.UserName = model.Username;

            var roleExists = await _roleManager.RoleExistsAsync(model.Role);
            if (!roleExists)
            {
                return BadRequest("Invalid role specified.");
            }

            // Remove existing roles and assign new role
            var userRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, userRoles.ToArray());
            await _userManager.AddToRoleAsync(user, model.Role);

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return Ok("User updated successfully.");
            }

            return BadRequest(result.Errors);
        }

    }
}
