namespace AuthService.Security;

public static class JwtClaims
{
    /// <summary>
    /// Salt - случайный идентификатор, позволяющий провести девалидацию сессии.
    /// </summary>
    public const string Salt = "Salt";

    /// <summary>
    /// TokenType - тип токена. МОжеть принимать значения "access" или "refresh".
    /// </summary>
    public const string TokenType = "TokenType";
}