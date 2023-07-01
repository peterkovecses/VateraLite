using Microsoft.AspNetCore.Identity;
using VateraLite.Application.Interfaces;
using VateraLite.Domain.Entities;
using VateraLite.Infrastructure.Identity.Models;

namespace VateraLite.Infrastructure.Persistence
{
    public class AppDbContextInitialiser
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AppDbContextInitialiser> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public AppDbContextInitialiser(IUnitOfWork unitOfWork, ILogger<AppDbContextInitialiser> logger, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _userManager = userManager;
        }

        public async Task SeedAsync()
        {
            try
            {
                if (!(await _unitOfWork.Customers.GetAsync()).Any())
                {
                    await TrySeedAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }
        }

        private async Task TrySeedAsync()
        {
            var user = new ApplicationUser
            {
                UserName = "test@test.com",
                Email = "test@test.com",
                CustomerId = 1
            };
            var password = "Password1!";
            await _userManager.CreateAsync(user, password);

            var customer = new Customer()
            {
                ApplicationUserId = user.Id,
            };

            await _unitOfWork.Customers.AddAsync(customer);
            await _unitOfWork.CompleteAsync();
        }
    }
}
