using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace UserService.Attributes.Validation;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
sealed public partial class ValidAvatar(ILogger logger) : ValidationAttribute
{
    private readonly ILogger _logger = logger;

    // FIXME: test regex
    private const string Pattern = @"^.{1,150}$";

    public override bool IsValid(object? value)
    {
        try
        {
            // FIXME: Implementation
            _logger.LogWarning("Avatar validation is not implemented, skipping");
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