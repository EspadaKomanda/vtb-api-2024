using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace UserService.Attributes.Validation;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
sealed public partial class ValidName : ValidationAttribute
{
    // FIXME: test regex
    private const string Pattern = @"^.{1,150}$";

    public override bool IsValid(object? value)
    {
        try
        {
            if (value != null)
            {
                string name = value.ToString() ?? "";

                if (!MyRegex().IsMatch(name))
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
        return "Name must be between 1 and 150 characters";
    }

    [GeneratedRegex(Pattern)]
    private static partial Regex MyRegex();
}