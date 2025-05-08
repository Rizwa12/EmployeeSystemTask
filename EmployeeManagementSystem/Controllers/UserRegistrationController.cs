using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace EmployeeManagementSystem.Controllers
{
    public class UserRegistrationController : Controller
    {
        private readonly ILogger<UserRegistrationController> _logger;
        private readonly ApplicationDbContext _context;
        IWebHostEnvironment _env;

        public UserRegistrationController(ILogger<UserRegistrationController> logger, ApplicationDbContext context,IWebHostEnvironment env)
        {
            _logger = logger;
            _context = context;
            _env = env;
        }


        public IActionResult Login()
        {
            return View();
        }
       

        [HttpPost]
        public IActionResult Login(User userlogin)
        {
            var myUser = _context.Users.Where(x => x.Email == userlogin.Email &&
            x.Password == userlogin.Password).FirstOrDefault();
            if (myUser != null && myUser.Role == "Employer")
            {
                HttpContext.Session.SetString("UserSession", myUser.Email);
                HttpContext.Session.SetString("UserId", myUser.Id.ToString());
                return RedirectToAction("Dashboard", "EmployerDashboard");
            }

            else if(myUser != null && myUser.Role == "Employee")
            {
                HttpContext.Session.SetString("UserSession", myUser.Email);
                HttpContext.Session.SetString("UserId", myUser.Id.ToString());
                return RedirectToAction("Dashboard", "Employee");
            }

            else
            {
                ViewBag.Message = "Password or email incorrect";
            }
            return View();
        }


        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(User request)
        {
            if (ModelState.IsValid)
            {

              var existingUser = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email);

                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "This email is already registered.");
                    return View(request); // Return the form with error
                }
                string fileName = "";
               
                if (request.PictureFile != null)
                {
                    string folder = Path.Combine(_env.WebRootPath, "images");

                    // ✅ Ensure the images folder exists
                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }

                    fileName = Guid.NewGuid().ToString() + "_" + request.PictureFile.FileName;
                    string filepath = Path.Combine(folder, fileName);

                    using (var stream = new FileStream(filepath, FileMode.Create))
                    {
                        request.PictureFile.CopyTo(stream);
                    }

                    // Optional: save the fileName to the user entity
                    request.PicturePath = fileName; // assuming your User entity has a PicturePath field
                }

                await _context.Users.AddAsync(request);
                await _context.SaveChangesAsync();

                TempData["success"] = "User Successfully Registered";
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
