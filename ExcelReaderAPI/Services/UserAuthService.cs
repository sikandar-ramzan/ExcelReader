using System.Data.Common;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using ExcelReaderAPI.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ExcelReaderAPI.Services
{
    public class UserAuthService
    {

        private readonly DatabaseHelper _databaseHelper;
        private readonly IConfiguration _configuration;


        public UserAuthService(IConfiguration configuration, DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
            _configuration = configuration;

        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);

            }
        }

        public string CreateToken(User user, bool isAdmin)
        {
            string userRole = isAdmin ? "Admin" : "User";
            List<Claim> claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.Username),
        new Claim(ClaimTypes.Role, userRole),
        new Claim("UserId", user.UserId.ToString())
    };

            var authSecretToken = _configuration.GetSection("AuthSecret:Token").Value;
            int tokenExpirationInMinutes = _configuration.GetValue<int>("TokenExpirationTimeout:Minutes");
            if (authSecretToken == null)
            {
                throw new Exception("AuthSecret:Token configuration value is missing.");
            }
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(authSecretToken));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);


            var tokenExpiration = DateTime.Now.AddMinutes(tokenExpirationInMinutes);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: tokenExpiration,
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        public class UserAuthResponse
        {
            public bool Success { get; set; }
            public string Message { get; set; } = String.Empty;
            public User? UserFromDb { get; set; }
        }

        private User? GetUserFromDb(string username, SqlConnection connection)
        {
            string getUserQuery = "select * from Users where Username = @userName";

            var getUserCmd = _databaseHelper.CreateSqlCommand(getUserQuery, connection);
            getUserCmd.Parameters.AddWithValue("@userName", username);

            using (var reader = getUserCmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new User
                    {
                        UserId = (Guid)reader["UserId"],
                        Username = (string)reader["Username"],
                        PasswordHash = (byte[])reader["PasswordHash"],
                        PasswordSalt = (byte[])reader["PasswordSalt"],
                        IsAdminUser = (bool)reader["IsAdminUser"]
                    };
                }
            }

            return null;
        }


        public UserAuthResponse CreateUser(string username, string password, bool isAdmin)
        {
            var dbConnection = _databaseHelper.CreateDbConnection();
            dbConnection.Open();

            User? userFromDb = GetUserFromDb(username, dbConnection);



            if (userFromDb != null)
            {
                dbConnection.Dispose();
                return new UserAuthResponse { Success = false, Message = $"User with username: {username} already exist!" };
            }

            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            User user = new User
            {
                UserId = Guid.NewGuid(),
                Username = username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                IsAdminUser = isAdmin
            };

            var createUserQuery = "insert into Users values (@userId, @username, @pswrdHash, @pswrdSalt, @isAdmin)";

            var createUserCmd = _databaseHelper.CreateSqlCommand(createUserQuery, dbConnection);
            createUserCmd.Parameters.AddWithValue("@userId", user.UserId);
            createUserCmd.Parameters.AddWithValue("@userName", user.Username);
            createUserCmd.Parameters.AddWithValue("@pswrdHash", user.PasswordHash);
            createUserCmd.Parameters.AddWithValue("@pswrdSalt", user.PasswordSalt);
            createUserCmd.Parameters.AddWithValue("@isAdmin", user.IsAdminUser);

            var userCreationResponse = createUserCmd.ExecuteNonQuery();

            dbConnection.Dispose();

            if (userCreationResponse > 0)
            {
                return new UserAuthResponse { Success = true, Message = $"User: {username} created successfully!" };
            }

            else
            {
                return new UserAuthResponse { Success = false, Message = "Internal Server Error" };
            }
        }


        public UserAuthResponse LoginUser(string username, string password)
        {
            var dbConnection = _databaseHelper.CreateDbConnection();
            dbConnection.Open();

            User? userFromDb = GetUserFromDb(username, dbConnection);


            if (userFromDb == null)
            {
                dbConnection.Dispose();
                return new UserAuthResponse { Success = false, Message = $"User with username: {username} does not exist!" };
            }

            // Verify the entered password against the stored hash and salt
            if (VerifyPasswordHash(password, userFromDb.PasswordHash, userFromDb.PasswordSalt))
            {
                return new UserAuthResponse { Success = true, Message = $"Login successful for user: {username}", UserFromDb = userFromDb };
            }

            dbConnection.Dispose();
            return new UserAuthResponse { Success = false, Message = "Invalid password" };
        }




    }
}
