using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace UserService.Attributes.Validation;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
sealed public partial class ValidAvatar : ValidationAttribute
{
    // FIXME: test regex
    private const string Pattern = @"^.{1,150}$";

    public override bool IsValid(object? value)
    {
        try
        {
            // FIXME: Implementation
            return true;
        }
        catch
        {
            return false;
        }
    }

    public override string FormatErrorMessage(string name)
    {
        return "Must be a valid avatar URL in the S3 storage";
    }

    [GeneratedRegex(Pattern)]
    private static partial Regex MyRegex();
}