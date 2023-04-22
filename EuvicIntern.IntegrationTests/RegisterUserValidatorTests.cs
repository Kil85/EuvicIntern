using EuvicIntern.Entities;
using EuvicIntern.Models;
using EuvicIntern.Models.Validators;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;
using FluentValidation.TestHelper;
using EuvicIntern.IntegrationTests.Data;

namespace EuvicIntern.IntegrationTests
{
    public class RegisterUserValidatorTests
    {
        private readonly EuvicDbContext _dbContext;

        public RegisterUserValidatorTests()
        {
            var builder = new DbContextOptionsBuilder<EuvicDbContext>();
            builder.UseInMemoryDatabase("validatorDb");

            _dbContext = new EuvicDbContext(builder.Options);
            Seeder();
        }

        [Theory]
        [ClassData(typeof(RegisterUserValidatorValidTestData))]
        public void Validate_ForValidModer_ReturnSucces(RegisterUserDto model)
        {
            //arrange
            var validator = new RegisterUserValidator(_dbContext);

            //act

            var result = validator.TestValidate(model);

            //assert

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [ClassData(typeof(RegisterUserValidatorInvalidTestData))]
        public void Validate_ForInvalidModel_ReturnFailure(RegisterUserDto model)
        {
            //arrange
            var validator = new RegisterUserValidator(_dbContext);

            //act
            var result = validator.TestValidate(model);

            //assert
            result.ShouldHaveAnyValidationError();
        }

        private void Seeder()
        {
            var users = new List<User>()
            {
                new User
                {
                    FirstName = "Jan",
                    LastName = "Kowalski",
                    PhoneNumber = "987654321",
                    Email = "wppl@wp.pl"
                },
                new User
                {
                    FirstName = "Jan",
                    LastName = "Kowalski",
                    PhoneNumber = "987654321",
                    Email = "jankowalski@test.pl"
                },
                new User
                {
                    FirstName = "Jan",
                    LastName = "Kowalski",
                    PhoneNumber = "987654321",
                    Email = "Foden@City.en"
                }
            };

            _dbContext.AddRange(users);
            _dbContext.SaveChanges();
        }
    }
}
