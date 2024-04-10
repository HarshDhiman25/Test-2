using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Property_Mangement.Data;

using Property_Mangement.Models;
using System.Diagnostics;

namespace Property_Mangement.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<IdentityUser> _signInManager;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, SignInManager<IdentityUser> signInManager)
        {
            _logger = logger;
            _context = context;
            _signInManager = signInManager;
        }


        public async Task<IActionResult> Index(string searchOwnerName)
        {
            if (_signInManager.IsSignedIn(User))
            {
                var properties = await _context.Properties.ToListAsync();
                return View(properties);
            }
            else
            {
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

