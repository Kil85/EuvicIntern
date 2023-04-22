using EuvicIntern.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace EuvicIntern.Models.Validators
{
    public class RegisterUserValidator : AbstractValidator<RegisterUserDto>
    {
        public RegisterUserValidator(EuvicDbContext dbContext)
        {
            RuleFor(r => r.FirstName).NotEmpty().MaximumLength(15);

            RuleFor(r => r.LastName).NotEmpty().MaximumLength(15);

            RuleFor(r => r.Password)
                .NotEmpty()
                .MinimumLength(8)
                .Matches(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$");

            RuleFor(r => r.ConfirmPassword).Equal(p => p.Password);

            RuleFor(r => r.Email).NotEmpty().MaximumLength(35).EmailAddress();

            RuleFor(r => r.PhoneNumber).NotEmpty().Matches(@"^[\s-]?\d{3}[\s-]?\d{3}[\s-]?\d{3}$");

            RuleFor(r => r.Age).GreaterThan(17).WithMessage("Minimum age is 18").LessThan(120);

            RuleFor(r => r.AveragePowerConsumption).PrecisionScale(18, 3, true);

            RuleFor(x => x.Email)
                .Custom(
                    (value, context) =>
                    {
                        var result = dbContext.Users.Any(y => y.Email == value);
                        if (result)
                        {
                            context.AddFailure("Email", "Email taken");
                        }
                    }
                );
        }
    }
}
