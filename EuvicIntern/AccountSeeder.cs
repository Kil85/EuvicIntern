using EuvicIntern.Entities;
using EuvicIntern.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EuvicIntern
{
    public class AccountSeeder
    {
        private readonly EuvicDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly ILogger<AccountSeeder> _logger;

        public AccountSeeder(
            EuvicDbContext context,
            IPasswordHasher<User> passwordHasher,
            ILogger<AccountSeeder> logger
        )
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        public void Seeder()
        {
            try
            {
                var migrations = _context.Database.GetPendingMigrations();

                if (migrations.Any())
                {
                    _context.Database.Migrate();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                Environment.Exit(500);
            }

            if (_context.Database.CanConnect())
            {
                if (!_context.Roles.Any())
                {
                    var roles = GetRoles();
                    _context.Roles.AddRange(roles);
                    _context.SaveChanges();
                }
                if (!_context.Users.Any())
                {
                    var users = GetUsers();
                    _context.Users.AddRange(users);
                    _context.SaveChanges();
                }
            }
        }

        private IEnumerable<Role> GetRoles()
        {
            var roles = new List<Role>()
            {
                new Role() { Name = "Admin" },
                new Role() { Name = "User" }
            };
            return roles;
        }

        private IEnumerable<User> GetUsers()
        {
            var users = new List<User>();

            var admin = new User()
            {
                FirstName = "Admin",
                LastName = "Admin",
                Email = "admin",
                PhoneNumber = "1234567890",
                RoleId = 1
            };
            var adminPassword = "admin";
            var adminHassedPassword = _passwordHasher.HashPassword(admin, adminPassword);
            admin.HashedPassword = adminHassedPassword;
            users.Add(admin);

            var user = new User()
            {
                FirstName = "Jan",
                LastName = "Kowalski",
                Email = "user",
                PhoneNumber = "1234567890",
                RoleId = 2
            };
            var userPassword = "admin";
            var userHassedPassword = _passwordHasher.HashPassword(user, userPassword);
            user.HashedPassword = userHassedPassword;
            users.Add(user);

            return users;
        }
    }
}
