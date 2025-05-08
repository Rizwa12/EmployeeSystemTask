using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Controllers
{
    public class EmployerDashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        IWebHostEnvironment _env;
        public EmployerDashboardController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
            
        }
       
        public IActionResult DashBoard()
        {

            if (HttpContext.Session.GetString("UserSession") != null)
            {
                ViewBag.MySession = HttpContext.Session.GetString("UserSession").ToString();
            }
            else
            {
                return RedirectToAction("UserRegistration", "Login");
            }
            return View();
        }
        public IActionResult Logout()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                HttpContext.Session.Remove("UserSession");
                return RedirectToAction("Login", "UserRegistration");
            }
            return View();
        }

        public IActionResult DisplayTask()
        {
            int employerId = Convert.ToInt32 (HttpContext.Session.GetString("UserId"));

            if (employerId == 0)
            {
                return RedirectToAction("UserRegistration", "Login");
            }
            var tasks = _context.AddTasks
           .Where(t => t.EmployerId == employerId)
             .ToList();
            return View(tasks);
        
        }
        public IActionResult CreateTask()
        {
            ViewBag.Employees = _context.Users
            .Where(u => u.Role == "Employee")
            .Select(u => new SelectListItem
            {
                Value = u.Id.ToString(),
                Text = u.Name
            }).ToList();

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateTask(AddTask addtask)
        {
            if(ModelState.IsValid)
            {
                if (addtask.PictureFile != null)
                {
                    string folderPath = Path.Combine(_env.WebRootPath, "task-images");

                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    string fileName = Guid.NewGuid().ToString() + "_" + addtask.PictureFile.FileName;
                    string fullPath = Path.Combine(folderPath, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await addtask.PictureFile.CopyToAsync(stream);
                    }

                    addtask.PicturePath = fileName;
                }
                addtask.EmployerId = Convert.ToInt32(HttpContext.Session.GetString("UserId"));
                _context.AddTasks.Add(addtask);
                await _context.SaveChangesAsync();

                TempData["Created"] = "Task created successfully!";
                return RedirectToAction("DisplayTask");
            }

            ViewBag.Employees = _context.Users
           .Where(u => u.Role == "Employee")
           .Select(u => new SelectListItem
           {
               Value = u.Id.ToString(),
               Text = u.Name
           }).ToList();
            return View();
        }
       
        public async Task<IActionResult> EditTask(int?id)
        {
            if (id == null || _context.AddTasks == null)
            {
                return NotFound();
            }
            var findtask = await _context.AddTasks.FindAsync(id);
            if(findtask ==  null)
            {
                return NotFound();
            }

            if (findtask.AssigneeId != null && findtask.AssigneeId != 0)
            {
                TempData["Error"] = "You cannot edit a task that has already been assigned.";
                return RedirectToAction("DisplayTask");
            }
            ViewBag.Employees = _context.Users
                .Where(u => u.Role == "Employee")
                .Select(u => new SelectListItem
                {
                    Value = u.Id.ToString(),
                    Text = u.Name
                }).ToList();
            return View(findtask);
        }
        [HttpPost]
        public async Task<IActionResult> EditTask(int? id,AddTask updatestask)
        {
            if (ModelState.IsValid)
            {
                if (updatestask.PictureFile != null)
                {
                    string folderPath = Path.Combine(_env.WebRootPath, "task-images");

                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    string fileName = Guid.NewGuid().ToString() + "_" + updatestask.PictureFile.FileName;
                    string fullPath = Path.Combine(folderPath, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await updatestask.PictureFile.CopyToAsync(stream);
                    }

                    updatestask.PicturePath = fileName;
                }
                updatestask.EmployerId = Convert.ToInt32(HttpContext.Session.GetString("UserId"));
                _context.AddTasks.Update(updatestask);
                await _context.SaveChangesAsync();

                TempData["Created"] = "Updated successfully!";
                return RedirectToAction("DisplayTask");
            }

            ViewBag.Employees = _context.Users
           .Where(u => u.Role == "Employee")
           .Select(u => new SelectListItem
           {
               Value = u.Id.ToString(),
               Text = u.Name
           }).ToList();

            return View();
        }

        public async Task<IActionResult>  Delete(int? id)
        {
            var deletetask = await _context.AddTasks.FindAsync(id);
            if (deletetask == null)
            {
                return NotFound();
            }

            return View(deletetask);
        }

        [HttpPost,ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var deletetask = await _context.AddTasks.FindAsync(id);
            if (deletetask != null)
            {
                _context.AddTasks.Remove(deletetask);
            }
            await _context.SaveChangesAsync();
            TempData["deleted"] = "successfully deletes!";
            return RedirectToAction("DisplayTask");
        }
    }
}
