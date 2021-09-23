using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelManagementSystem.Models;
using HotelManagementSystem.Services;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using X.PagedList;

namespace HotelManagementSystem.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ITicketService _ticketService;
        private readonly IServiceProvider _serviceProvider;

        private static IEnumerable<string> roles = new List<string>
        {
            "Admin",
            "Manager",
            "User",
            "Receptionist",
            "CleaningService"
        };
            
        public AdminController(IEmployeeService employeeService, RoleManager<ApplicationRole> roleManager, ITicketService ticketService, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _roleManager = roleManager;
            _employeeService = employeeService;
            _ticketService = ticketService;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public IActionResult EmployeeManager()
        {
            return View(_employeeService.GetAllEmployes());
        }
        [Authorize(Roles = "Admin")]
        public IActionResult FindEmployee()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult SearchEmpByEmail(Employee employee)
        {
            var x = Convert.ToString(Request.Form["EmpEmail"]);

            var empList = _employeeService.GetAllEmployes();
            foreach (var item in empList)
            {
                if (x == item.Email)
                {
                    employee.EmpId = item.EmpId;
                }
            }

            if (employee.EmpId == 0)
            {
                return RedirectToAction("FindEmployee");
            }
            else
            {
                return Redirect("EditEmp/" + employee.EmpId);
            }

        }
        
        [Authorize(Roles="Admin")]
        public IActionResult EditEmp(int id)
        {
            return  View(_employeeService.GetEmpById(id));
        }
        [Authorize(Roles = "Admin")]
        public IActionResult EditedEmp(Employee updatedEmp)
        {
            if (ModelState.IsValid)
            {
                _employeeService.Update(updatedEmp);
                return RedirectToAction(nameof(FindEmployee));
            }
            else
                return View();

        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> RoleManager()
        {
            var RoleManager = _serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var UserManager = _serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var email = Convert.ToString(Request.Form["Employee"]);
            var role = Convert.ToString(Request.Form["Role"]);

            ApplicationUser user = await UserManager.FindByEmailAsync(email);
            var User = new ApplicationUser();
            var currentRoles = await UserManager.GetRolesAsync(user);

            await UserManager.RemoveFromRolesAsync(user, currentRoles);
            await UserManager.AddToRoleAsync(user, role);

            return RedirectToAction(nameof(EmployeeManager));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult EmployeeEditor(Employee employee)
        {
            
            return RedirectToAction(nameof(EmployeeManager));
        }

        [Authorize(Roles = "Admin")]
        public IActionResult ActiveSupportTickets(int? page)
        {
            var tickets = _ticketService.GetAllTickets().Where(t => t.StatusCode == 1);
            var pageNumber = page ?? 1;
            var pageSize = 10;

            return View(tickets.ToPagedList(pageNumber, pageSize));
        }
        [Authorize(Roles = "Admin")]
        public IActionResult ResolvedSupportTickets(int? page)
        {
            var tickets = _ticketService.GetAllTickets().Where(t => t.StatusCode == 2);
            var pageNumber = page ?? 1;
            var pageSize = 10;

            return View(tickets.ToPagedList(pageNumber, pageSize));
        }
        [Authorize(Roles = "Admin")]
        public IActionResult TicketDetails(int id)
        {
            return View(_ticketService.GetTicketById(id));
        }

        [Authorize(Roles = "Admin")]
        public IActionResult TicketUpdate(SupportTicket supportTicket)
        {
            if (ModelState.IsValid)
            {
                _ticketService.Update(supportTicket);
                return RedirectToAction(nameof(ActiveSupportTickets));
            }
            else
                return View();
          
        }

    }
}