using CSharpFunctionalExtensions;
using FluentValidation;
using FluentValidation.Results;
using TaskManagement.Domain.Errors;

namespace TaskManagement.Application.Extensions;

public static class ValidatorsExtension
{
    public static IRuleBuilderOptionsConditions<T, TElement>
        MustBeValueObject<T, TElement, TValueObject>(
            this IRuleBuilder<T, TElement> ruleBuilder,
            Func<TElement, Result<TValueObject, Error>> factoryMethod)
    {
        return ruleBuilder.Custom((value, context) =>
        {
            var result = factoryMethod(value);

            if (result.IsSuccess)
                return;

            context.AddFailure(result.Error.Serialize());
        });
    }
    
    public static IRuleBuilderOptionsConditions<T, TProperty>
        MustBeEnum<T, TProperty, TEnum>(
            this IRuleBuilder<T, TProperty> ruleBuilder) where TEnum : Enum
    {
        return ruleBuilder.Custom((value, context) =>
        {
            if (!Enum.TryParse(typeof(TEnum), value!.ToString(), out var result))
            {
                context.AddFailure(Errors.General.
                    ValueIsInvalid("value is invalid").Serialize());
            }
        });
    }
    
    public static ErrorList ToErrorList(this ValidationResult validationResult)
    {
        var validationErrors = validationResult.Errors;

        var errors = from validationError in validationErrors
            let errorMessage = validationError.ErrorMessage
            let error = Error.Deserialize(errorMessage)
            select Errors.General.ValueIsInvalid(
                validationError.PropertyName + " is invalid");

        return errors.ToList();
    }
}