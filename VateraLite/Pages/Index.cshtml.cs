using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VateraLite.Application.Interfaces;
using VateraLite.Infrastructure.Identity.Models;

namespace VateraLite.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IStockService _stockService;
        private static readonly SemaphoreSlim _semaphore = new(1);

        [BindProperty]
        public int Quantity { get; set; }

        public string Message { get; set; } = string.Empty;

        public IndexModel(UserManager<ApplicationUser> userManager, IStockService stockService)
        {
            _userManager = userManager;
            _stockService = stockService;
        }

        public async Task OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var customerId = user!.CustomerId!.Value;
            var result = await _stockService.ReserveAsync(Quantity, customerId);
            Message = result.Message;
        }
    }
}
