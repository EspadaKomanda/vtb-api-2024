using System.ComponentModel.DataAnnotations;

namespace UserService.Attributes.Validation;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
sealed public partial class ValidAge : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        try
        {
            if (value != null)
            {
                DateTime date = (DateTime)value;
                
                // FIXME: Check if this is the actual age we need
                int age = DateTime.Now.Year - date.Year;
                if (age < 18)
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
        return "You must be at least 18 years old";
    }
}