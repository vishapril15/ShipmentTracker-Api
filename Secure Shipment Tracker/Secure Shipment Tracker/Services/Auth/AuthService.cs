using Microsoft.IdentityModel.Tokens;
using Secure_Shipment_Tracker.DTOs;
using Secure_Shipment_Tracker.Models;
using Secure_Shipment_Tracker.Repositories.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Secure_Shipment_Tracker.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _conf;
        private readonly IAuthRepo _authRepo;
        private readonly ILogger<AuthService> _logger;
        public AuthService(IConfiguration conf,IAuthRepo repo, ILogger<AuthService> logger)
        {
            _conf = conf;
            _authRepo = repo;
            _logger = logger;
        }
        public async Task<LoginResponsedto> Login(LoginDto dto)
        {
                var user = await _authRepo.GetByUsername(dto.Username);
                if (user == null) throw new UnauthorizedAccessException(" Invalid username or passwrod");

                if(!BCrypt.Net.BCrypt.Verify(dto.Password,user.PasswordHash))
                    {
                    throw new UnauthorizedAccessException("Invalid username or password");
                }
                var expiry = DateTime.UtcNow.AddMinutes(40);
                var token = GenerateToken(user);

                return new LoginResponsedto { Token = token,
                Role= user.Role.ToString(),
                Expiry=expiry};
        }

        public async Task Register(RegisterDto dto)
        {
              
                if( await _authRepo.UserExists(dto.Username)) { throw new InvalidOperationException("Username already exists!"); }

                var hashPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);
                var usr = new User
                {
                    UserName = dto.Username,
                    PasswordHash = hashPassword,
                    Role = dto.Role,
                };
                await _authRepo.AddUser(usr);
        }


        private string GenerateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.Role,user.Role.ToString())
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_conf["Jwt:SecretKey"]));
            var cred = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken
            (
                claims : claims,
                signingCredentials :cred,
                expires: DateTime.UtcNow.AddMinutes(40)
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
            
        }
    }
}
