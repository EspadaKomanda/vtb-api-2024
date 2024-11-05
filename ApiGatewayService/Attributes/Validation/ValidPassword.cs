using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace UserService.Attributes.Validation;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
sealed public partial class ValidPassword : ValidationAttribute
{
    private const string Pattern = @"^.{8,256}$";
    private string? _errorMessage;

    public override bool IsValid(object? value)
    {
        try
        {
            if (value != null)
            {
                string password = value.ToString() ?? "";

                if (!MyRegex().IsMatch(password))
                {
                    _errorMessage = "Password must be between 8 and 256 characters";
                    return false;
                }
            }
            return true;
        }
        catch (Exception e)
        {
            // FIXME: use logger instead
            Console.WriteLine(e);
            _errorMessage = "Invalid password. Contact administrator if you think this is a mistake.";
            return false;
            throw;
        }
    }

    public override string FormatErrorMessage(string name)
    {
        return _errorMessage ?? "Invalid password";
    }

    [GeneratedRegex(Pattern)]
    private static partial Regex MyRegex();
}