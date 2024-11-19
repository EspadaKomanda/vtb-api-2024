using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthService.Exceptions.Auth;
using AuthService.Models;
using AuthService.Security;
using AuthService.Services.AccessDataCache;
using AuthService.Services.Models;
using Microsoft.IdentityModel.Tokens;

namespace AuthService.Services.Jwt;

public class JwtService(IConfiguration configuration, IAccessDataCacheService accessDataCacheService, ILogger<JwtService> logger) : IJwtService
{
    private readonly IAccessDataCacheService _adcs = accessDataCacheService;
    private readonly IConfiguration _configuration = configuration;
    private readonly ILogger<JwtService> _logger = logger;

    private string GenerateToken(UserAccessData user, string tokenType, double expireMinutes)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(
            _configuration["Jwt:Key"] ?? throw new MissingConfigurationException("Jwt:Key")
        );

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.Username),
                new(ClaimTypes.Role, user.Role),
                new(JwtClaims.Salt, user.Salt), // Salt пользователя
                new(JwtClaims.TokenType, tokenType), // Тип токена
                new(JwtRegisteredClaimNames.Aud, _configuration["Jwt:Audience"] ?? throw new MissingConfigurationException("Jwt:Audience")),
                new(JwtRegisteredClaimNames.Iss, _configuration["Jwt:Issuer"] ?? throw new MissingConfigurationException("Jwt:Issuer"))
            }),
            Expires = DateTime.UtcNow.AddMinutes(expireMinutes),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private async Task<ValidatedUser?> ValidateToken(string token, string wantedTokenType, bool checkSalt = true)
    {
        try {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? throw new MissingConfigurationException("Jwt:Key"));

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidAudience = _configuration["Jwt:Audience"],
                ValidateLifetime = false,
            }, out SecurityToken validatedToken);

            if (validatedToken == null)
            {
                throw new InvalidTokenException("Invalid token");
            }

            // Validate token type
            string type = tokenHandler.ReadJwtToken(token).Claims.First(x => x.Type == JwtClaims.TokenType).Value;
            if ( type != wantedTokenType) {
                throw new InvalidTokenTypeException("Invalid token type");
            }

            string username = tokenHandler.ReadJwtToken(token).Claims.First(x => x.Type == ClaimTypes.Name).Value;
            string salt = tokenHandler.ReadJwtToken(token).Claims.First(x => x.Type == JwtClaims.Salt).Value;


            if (checkSalt)
            {
                var accessData = await _adcs.Get(username) ?? throw new UserNotFoundException($"User not found: {username}");
                // TODO: If salt does not match the user, throw an exception
                // throw new SessionTerminatedException("Invalid token");
            }

            ValidatedUser user = new() {
                Id = int.Parse(tokenHandler.ReadJwtToken(token).Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value),
                Username = tokenHandler.ReadJwtToken(token).Claims.First(x => x.Type == ClaimTypes.Name).Value,
                Role = tokenHandler.ReadJwtToken(token).Claims.First(x => x.Type == ClaimTypes.Role).Value
            };

            return user;
        }
        catch (Exception e) {
            _logger.LogDebug("Invalidated an {wantedTokenType} token: {e}", wantedTokenType, e.Message);
            return null;
        }
    }


    public string GenerateAccessToken(UserAccessData user)
    {
        return GenerateToken(
            user, 
            "access", 
            int.Parse(_configuration["Jwt:AccessExpires"] ?? throw new MissingConfigurationException("Jwt:AccessExpires"))
        );
    }

    public string GenerateRefreshToken(UserAccessData user)
    {
        return GenerateToken(
            user, 
            "refresh", 
            int.Parse(_configuration["Jwt:RefreshExpires"] ?? throw new MissingConfigurationException("Jwt:RefreshExpires"))
        );
    }

    public async Task<ValidatedUser?> ValidateAccessToken(string token)
    {
        return await ValidateToken(token, "access");
    }

    public async Task<ValidatedUser?> ValidateRefreshToken(string token)
    {
        return await ValidateToken(token, "refresh");
    }
}
