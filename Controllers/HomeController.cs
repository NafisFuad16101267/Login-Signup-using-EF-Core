using CoreAppPlayGround.Models;
using CoreAppPlayGround.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace CoreAppPlayGround.Controllers
{
    public class HomeController : Controller
    {
        private readonly MyDbContext context;
        private readonly IGenericRepository<User> _genericRepository;

        public HomeController(MyDbContext context, IGenericRepository<User> genericRepository)
        {
            this.context = context;
            _genericRepository = genericRepository;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                return RedirectToAction("Dashboard");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(User u)
        {
            var myUser = await _genericRepository.FindAsync(x => x.Email == u.Email && x.Password == u.Password);
            if (myUser != null)
            {
                HttpContext.Session.SetString("UserSession", myUser.EmpName);
                return RedirectToAction("Dashboard");
            }
            else
            {
                ViewBag.Message = "Bad Credential Email or Password.";
            }
            return View();
        }

        public IActionResult Signup()
        {
            List<SelectListItem> Gender = new()
            {
                new SelectListItem {Value="Male",Text="Male"},
                new SelectListItem {Value="Female",Text="Female"}
            };
            ViewBag.Gender = Gender;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Signup(User u)
        {
            if (ModelState.IsValid)
            {
                await _genericRepository.AddAsync(u);
                await _genericRepository.SaveAsync();
                //await context.Users.AddAsync(u);
                //await context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Registration successful! Please log in.";
                return RedirectToAction("Login");
            }
            return View();
        }

        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                ViewData["MySession"] = HttpContext.Session.GetString("UserSession").ToString();
            }
            else
            {
                return RedirectToAction("Login");
            }
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Logout()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                HttpContext.Session.Remove("UserSession");
                return RedirectToAction("Login");
            }

            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
