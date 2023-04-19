using EuvicIntern.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;

namespace EuvicIntern.IntegrationTests
{
    public class AccountControllerTests
    {
        private HttpClient client;

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
                        services.AddMvc(option => option.Filters.Add(new FakeUserFilter()));
                        services.AddDbContext<EuvicDbContext>(
                            options => options.UseInMemoryDatabase("DbContext")
                        );
                    });
                })
                .CreateClient();
        }

        [Fact]
        public async Task GetAll_RerurnsOk()
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
    }
}
