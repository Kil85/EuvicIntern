using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace EuvicIntern.IntegrationTests
{
    public class ProgramTests
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly List<Type> _controllerTypes;

        public ProgramTests()
        {
            _controllerTypes = typeof(Program).Assembly
                .GetTypes()
                .Where(t => t.IsSubclassOf(typeof(ControllerBase)))
                .ToList();

            var factory = new WebApplicationFactory<Program>();
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    _controllerTypes.ForEach(t => services.AddScoped(t));
                });
            });
        }

        [Fact]
        public async Task ConfigureServices_ForControllers_RegistersAllDependecies()
        {
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();

            _controllerTypes.ForEach(t =>
            {
                var controller = scope.ServiceProvider.GetService(t);
                controller.Should().NotBeNull();
            });
        }
    }
}
