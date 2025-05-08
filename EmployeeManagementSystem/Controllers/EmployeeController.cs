using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
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

        public IActionResult EmployeeViewTask()
        {
            int employeeId = Convert.ToInt32(HttpContext.Session.GetString("UserId"));

            if (employeeId == 0)
            {
                return RedirectToAction("UserRegistration", "Login");
            }
            var tasks = _context.AddTasks
           .Where(t => t.AssigneeId == employeeId)
             .ToList();
            return View(tasks);
           
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.AddTasks == null)
            {
                return NotFound();
            }
            var findtask = await _context.AddTasks.FindAsync(id);
            if (findtask == null)
            {
                return NotFound();
            }

            return View(findtask);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int? id, AddTask updatestask)
        {
            var existingTask = await _context.AddTasks.FindAsync(id);
            if (existingTask == null)
            {
                return NotFound();
            }

            // Only allow updating these two fields
            existingTask.Status = updatestask.Status;
            existingTask.EmployeeComments = updatestask.EmployeeComments;

            _context.AddTasks.Update(existingTask);
            await _context.SaveChangesAsync();

            TempData["updatemessage"] = "Task updated successfully!";
            return RedirectToAction("EmployeeViewTask");
        }

    }
}
