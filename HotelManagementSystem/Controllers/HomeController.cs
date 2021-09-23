using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HotelManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using HotelManagementSystem.Services;
using HotelManagementSystem.Models.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace HotelManagementSystem.Controllers
{
	[Authorize]
	public class HomeController : Controller
	{
		private readonly HMSdbContext _context;
		private readonly IEmailSender _emailSender;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IEmployeeService _employeeService;
		public HomeController(HMSdbContext context, IEmailSender emailSender, UserManager<ApplicationUser> userManager, IEmployeeService employeeService)
		{
			_emailSender = emailSender;
			_context = context;
			_userManager = userManager;
			_employeeService = employeeService;
		}

		public IActionResult Index()
		{				
			List<RoomReservationViewModel> a = new List<RoomReservationViewModel>();
			var result = (from p in _context.Room
						  join m in _context.Reservation on p.RoomId equals m.FkRoomId						  
						  select new RoomReservationViewModel
						  {
							  RoomNumber = p.RoomNumber,
							  RoomType = p.RoomType,
							  RoomFloor = p.RoomFloor,
							  RoomStatusId = p.FkRoomStatusId,
							  StartDate = m.FromDate,
							  EndDate = m.ToDate,
						  }).ToList();			 	

			return View(result);
		}
		[Authorize(Roles = "Admin")]
		public IActionResult Privacy()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> CreateTicket(SupportTicket newTicket)
		{
			var email = "hms@t.pl";
			var subject = Convert.ToString(Request.Form["Subject"]);
			var message = Convert.ToString(Request.Form["Message"]);

			await _emailSender.SendEmailAsync(email, subject, message);

			newTicket.EmpId = await GetUserAsEmp();
			newTicket.Message = message;
			newTicket.StatusCode = 1;

			_context.SupportTicket.Add(newTicket);
			_context.SaveChanges();

			return RedirectToAction("Index");

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
				}

			}
			return empId;
		}



		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
