namespace UserService.Utils;

public static class BcryptUtils
{
    
    public static string HashPassword(string password)
    {
        
        // Hash the password with the salt and a work factor of 10
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

        return hashedPassword;
    }

    public static bool VerifyPassword(string password, string hashedPassword)
    {
        try {
            // Check if the provided password matches the hashed password
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
        catch (Exception)
        {
            return false;
        }
    }
}