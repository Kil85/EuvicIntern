using EuvicIntern.Entities;
using EuvicIntern.IntegrationTests.Fakers;
using EuvicIntern.IntegrationTests.Helpers;
using EuvicIntern.Models;
using EuvicIntern.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using Xunit;

namespace EuvicIntern.IntegrationTests
{
    public class AccountControllerTests
    {
        private HttpClient client;
        private Mock<IAccountService> _accountService = new Mock<IAccountService>();

        public AccountControllerTests()
        {
            var factory = new WebApplicationFactory<Program>();
            client = factory
                .WithWebHostBuilder(builer =>
                {
                    builer.ConfigureServices(services =>
                    {
                        var currentDbContext = services.SingleOrDefault(
                            service =>
                                service.ServiceType == typeof(DbContextOptions<EuvicDbContext>)
                        );
                        services.Remove(currentDbContext);

                        services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                        services.AddSingleton(_accountService.Object);
                        services.AddDbContext<EuvicDbContext>(
                            options => options.UseInMemoryDatabase("DbContext")
                        );
                    });
                })
                .CreateClient();
        }

        [Fact]
        public async Task GetAll_ReturnsOk()
        {
            //set

            var response = await client.GetAsync("/api/account/all");

            //asserts

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task GetById(int id)
        {
            var response = await client.GetAsync($"/api/account/{id}");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task RegisterUser_WithGoodModel_ReturnCreated()
        {
            var model = new RegisterUserDto
            {
                FirstName = "Erling",
                LastName = "Haaland",
                Password = "passworD123!",
                ConfirmPassword = "passworD123!",
                Email = "Haaland@City.en",
                PhoneNumber = "123456789",
            };

            var httpContent = model.ToJsonHttpContent();

            var response = await client.PostAsync("/api/account/register", httpContent);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        }

        [Fact]
        public async Task RegisterUser_WithInvalidModel_ReturnBadRequest()
        {
            var model = new RegisterUserDto
            {
                FirstName = "Erling",
                LastName = "Haaland",
                Password = "passworD123!",
                Email = "Haaland@City.en",
                PhoneNumber = "123456789",
            };

            var httpContent = model.ToJsonHttpContent();

            var response = await client.PostAsync("/api/account/register", httpContent);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task LoginUser_ForRegisterdUser_ReturnOk()
        {
            _accountService.Setup(x => x.Login(It.IsAny<LoginDto>())).Returns("jwt");

            var model = new LoginDto { Email = "test@test.pl", Password = "Abcdefg123!" };

            var httpContent = model.ToJsonHttpContent();

            var response = await client.PostAsync("/api/account/login", httpContent);
        }
    }
}
