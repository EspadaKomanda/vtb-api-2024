using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace UserService.Attributes.Validation;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
sealed public partial class ValidUsername : ValidationAttribute
{
    private const string Pattern = @"^[a-zA-Z0-9_]{3,18}$";

    public override bool IsValid(object? value)
    {
        try
        {
            if (value != null)
            {
                string username = value.ToString() ?? "";

                if (!MyRegex().IsMatch(username))
                {
                    return false;
                }
            }
            return true;
        }
        catch
        {
            return false;
        }
    }

    public override string FormatErrorMessage(string name)
    {
        return "Username must be between 3 and 18 characters and contain only letters, numbers, and underscores";
    }

    [GeneratedRegex(Pattern)]
    private static partial Regex MyRegex();
}