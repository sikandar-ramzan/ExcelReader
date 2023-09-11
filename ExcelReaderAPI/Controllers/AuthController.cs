
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using ExcelReaderAPI.Models;
using ExcelReaderAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ExcelReaderAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly UserAuthService _authServices;
        private readonly IConfiguration _configuration;

        public AuthController(UserAuthService authServices, IConfiguration configuration)
        {
            _authServices = authServices;
            _configuration = configuration;

        }



        [HttpPost("register-user")]
        public async Task<IActionResult> RegisterUser(UserDto request)
        {


            var userCreationResponse = _authServices.CreateUser(request.Username, request.Password, false);


            if (userCreationResponse.Success)
            {
                return Ok($"User: {request.Username} created successfully!");
            }

            else
            {
                return BadRequest(userCreationResponse.Message);
            }
        }

        [HttpPost("register-admin")]
        public async Task<ActionResult> RegisterAdminUser(UserDto request)
        {

            var userCreationResponse = _authServices.CreateUser(request.Username, request.Password, true);


            if (userCreationResponse.Success)
            {
                return Ok(userCreationResponse.Message);
            }

            else
            {
                return BadRequest(userCreationResponse.Message);
            }



        }

        [HttpPost("login")]
        public async Task<ActionResult<object>> Login(UserDto request)
        {

            var userLoginResponse = _authServices.LoginUser(request.Username, request.Password);




            if (!userLoginResponse.Success || userLoginResponse.UserFromDb == null)
            {
                return BadRequest(userLoginResponse.Message);
            }
            var userFromDb = userLoginResponse.UserFromDb;
            var token = _authServices.CreateToken(userFromDb, userFromDb.IsAdminUser);

            return Ok(new { Token = token, UserId = userLoginResponse.UserFromDb.UserId, Username = userLoginResponse.UserFromDb.Username, IsAdminUser = userLoginResponse.UserFromDb.IsAdminUser });


        }

    }


}
