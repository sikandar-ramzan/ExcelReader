using ExcelReaderAPI.Services;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using ExcelReaderAPI.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace ExcelReaderAPI.Controllers
{
    [Route("it-request")]
    [ApiController]
    public class ITRequestUpload : ControllerBase
    {
        private readonly FileUploadService _fileUploadService;
        private readonly IConfiguration _configuration;

        public ITRequestUpload(FileUploadService fileUploadService, IConfiguration configuration)
        {
            _fileUploadService = fileUploadService;
            _configuration = configuration;
        }

        private string GetUserIdFromClaims()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var userId = identity?.FindFirst("UserId");

            return userId?.Value;
        }

        private string GetUserRoleFromClaims()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var roleClaim = identity?.FindFirst(ClaimTypes.Role);

            return roleClaim?.Value;
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet]
        public async Task<ActionResult<List<ITRequestWithFile>>> GetAllITRequests()
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("AuthSecret:Token").Value);
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();


            try
            {
                // Validate and parse the token
                var claimsPrincipal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);

                // Check if the token has expired
                var expiration = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp)?.Value;
                if (!string.IsNullOrEmpty(expiration) && long.TryParse(expiration, out long expirationUnixTimestamp))
                {
                    var expirationDateTime = DateTimeOffset.FromUnixTimeSeconds(expirationUnixTimestamp).UtcDateTime;
                    if (expirationDateTime <= DateTime.UtcNow)
                    {
                        return Unauthorized("Token has expired.");
                    }
                }
            }
            catch (Exception ex)
            {
                return Unauthorized("Invalid token.");
            }
            return _fileUploadService.GetAllItRequests();
        }

        [HttpPost]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> UploadExcelFile(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var uploadSuccessful = _fileUploadService.UploadExcelFile(file);

                if (uploadSuccessful != null)
                {
                    return Ok(uploadSuccessful);
                }
            }

            return BadRequest("failure");
        }
    }
}