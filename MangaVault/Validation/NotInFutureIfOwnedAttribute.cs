using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace MangaVault.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public class NotInFutureIfOwnedAttribute : ValidationAttribute, IClientModelValidator
    {
        private readonly string _ownedPropertyName;

        public NotInFutureIfOwnedAttribute(string ownedPropertyName)
        {
            _ownedPropertyName = ownedPropertyName;
            ErrorMessage = "An owned volume cannot have a release date in the future.";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not DateTime releaseDate)
            {
                return ValidationResult.Success;
            }

            var ownedProperty = validationContext.ObjectType.GetProperty(_ownedPropertyName);
            if (ownedProperty is null || ownedProperty.PropertyType != typeof(bool))
            {
                throw new InvalidOperationException(
                    $"'{_ownedPropertyName}' must be a public bool property on {validationContext.ObjectType.Name}");
            }

            var isOwned = (bool)ownedProperty.GetValue(validationContext.ObjectInstance)!;
            if (isOwned && releaseDate.Date > DateTime.Today)
            {
                return new ValidationResult(ErrorMessage, new[] { validationContext.MemberName! });
            }

            return ValidationResult.Success;
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-notinfutureifowned", ErrorMessage!);
            MergeAttribute(context.Attributes, "data-val-notinfutureifowned-owned", _ownedPropertyName);
        }

        private static void MergeAttribute(IDictionary<string, string> attributes, string key, string value)
        {
            if (!attributes.ContainsKey(key))
            {
                attributes.Add(key, value);
            }
        }
    }
}
