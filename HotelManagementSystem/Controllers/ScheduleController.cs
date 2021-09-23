using HotelManagementSystem.Models;
using HotelManagementSystem.Models.ViewModels;
using HotelManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManagementSystem.Controllers
{
	[Authorize]
    public class ScheduleController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmployeeService _employeeService;
        private readonly HMSdbContext _context;

        public ScheduleController(IEmployeeService employeeService, UserManager<ApplicationUser> userManager, HMSdbContext context)
        {
            _userManager = userManager;
            _employeeService = employeeService;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "Admin, Manager")]
        public IActionResult FindEmployee()
        {
            return View();
        }

        [HttpPost]        
        public IActionResult FindEmployeeScheduleByEmail()
        {
            var x = Convert.ToString(Request.Form["EmpEmail"]);
            var empList = _employeeService.GetAllEmployes();
            bool isTrue = false;

            foreach (var item in empList)
            {
                if (x == item.Email)
                {
                    isTrue = true;
                    break;
                }              
            }
            if (isTrue == true)
            {
                return Redirect("AllSchedules?email=" + x);
            }
            else
                return RedirectToAction(nameof(FindEmployee));
           
          
        }

        public async Task<IActionResult> EmployeeSchedule()
        {         
           var currentUser = await _userManager.GetUserAsync(User);
           bool roleAdmin = await _userManager.IsInRoleAsync(currentUser, "Admin");
           bool roleManager = await _userManager.IsInRoleAsync(currentUser, "Manager");

            if (roleAdmin == true || roleManager == true)
            {
                return RedirectToAction(nameof(FindEmployee));
            }
            else
                return Redirect("AllSchedules?email=" + currentUser.Email);

           
        }
        public IActionResult AllSchedules(string email)
        {
            
            var employee = _employeeService.GetEmployeeByEmail(email);
            var schedules = _context.ShiftSchedule.Where(e => e.EmpId == employee.EmpId).ToList();

            var result = (from s in _context.ShiftSchedule
                          where s.EmpId == employee.EmpId
                          select new EmployeeScheduleViewModel
                          {
                              ShiftId = s.ShiftId,
                              ShiftType = s.ShiftType,
                              EmpId = employee.EmpId,
                              Email = employee.Email,
                              EmpName = employee.FirstName + " " + employee.LastName,
                              FromDate = s.FromDate,
                              ToDate = s.ToDate

                          }).ToList();            

            if(result.Count == 0)
            {
                result.Add(new EmployeeScheduleViewModel
                {
                    ShiftId = 999999,
                    ShiftType = 0,
                    EmpId = employee.EmpId,
                    Email = employee.Email,
                    EmpName = employee.FirstName + " " + employee.LastName,
                    FromDate = DateTime.Now,
                    ToDate = DateTime.Now.AddHours(10),
                }) ;
            }

            return View(result);
        }
        [Authorize(Roles = "Admin, Manager")]
        public IActionResult Edit(int id)
       {
            var shift = _context.ShiftSchedule.Find(id);
            return View(shift);
       }
        [Authorize(Roles = "Admin, Manager")]
        public IActionResult Edited(ShiftSchedule shift)
       {
            if (DateValidatorForEditor(shift) == false)
            {
                var entity = _context.ShiftSchedule.Attach(shift);
                entity.State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction(nameof(FindEmployee));
            }
            else               
                return RedirectToAction(nameof(FindEmployee));
       }
        [Authorize(Roles = "Admin, Manager")]
        public IActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "Admin, Manager")]
        public IActionResult Created(EmployeeScheduleViewModel viewModel)
        {
            var result = Creates(viewModel);           

            if (DateValidator(viewModel) == false)
            {
                _context.ShiftSchedule.Add(result);
                _context.SaveChanges();
                return Redirect("AllSchedules?email=" + viewModel.Email);
            }
            else
                return RedirectToAction("FindEmployee");
        }

        [Authorize(Roles = "Admin, Manager")]
        public IActionResult Delete(int id)
		{
            ShiftSchedule shiftSchedule = _context.ShiftSchedule.Find(id);
            return View(shiftSchedule);
		}

        public IActionResult Deleted(ShiftSchedule shiftSchedule)
		{
            int x = shiftSchedule.ShiftId;
            var result = _context.ShiftSchedule.Find(shiftSchedule.ShiftId);

          
            _context.SaveChanges();
            if (x != 0)
            {
                _context.ShiftSchedule.Remove(result);
                _context.SaveChanges();
                return RedirectToAction(nameof(FindEmployee));              
            }
            else
                return View();
        }
        

        public async Task<int> GetUserAsEmp()
        {
            var user = await _userManager.GetUserAsync(User);
            var employees = _employeeService.GetAllEmployes();

            int empId = 0;

            foreach (var emp in employees)
            {
                if (emp.Email == user.Email)
                {
                    empId = emp.EmpId;
                    break;
                }

            }

            return empId;
        }
        public ShiftSchedule Creates(EmployeeScheduleViewModel viewModel)
        {
            ShiftSchedule shift = new ShiftSchedule();
            var employee = _employeeService.GetEmployeeByEmail(viewModel.Email);
            shift.EmpId = employee.EmpId;
            shift.ShiftType = viewModel.ShiftType;
            shift.FromDate = viewModel.FromDate;
            shift.ToDate = viewModel.ToDate;


            return shift;

        }
        public bool DateValidator(EmployeeScheduleViewModel viewModel)
        {
            var shifts = _context.ShiftSchedule.ToList();
            var employee = _employeeService.GetEmployeeByEmail(viewModel.Email);
            bool overlap = true;
            if (shifts.Count == 0)
            {
                return false;
            }
            foreach (var item in shifts)
            {

                if (viewModel.FromDate <= item.ToDate && item.FromDate <= viewModel.ToDate && item.EmpId == employee.EmpId)
                {
                    overlap = true;
                    break;
                }
                else
                {
                    overlap = false;
                }
            }
            return overlap;
        }
        public bool DateValidatorForEditor(ShiftSchedule viewModel)
        {
            var shifts = _context.ShiftSchedule.ToList();
            var employee = _employeeService.GetEmpById(viewModel.EmpId);
            bool overlap = true;
            if (shifts.Count == 0)
            {
                return false;
            }
            foreach (var item in shifts)
            {

                if (viewModel.FromDate <= item.ToDate && item.FromDate <= viewModel.ToDate && item.EmpId == employee.EmpId)
                {
                    overlap = true;
                    break;
                }
                else
                {
                    overlap = false;
                }
            }
            return overlap;
        }
    }
   
}