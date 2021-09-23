using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HotelManagementSystem.Models;
using HotelManagementSystem.Models.ViewModels;
using HotelManagementSystem.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using X.PagedList;

namespace HotelManagementSystem.Controllers
{
    [Authorize]
    public class ReservationController : Controller
    {
        private readonly IReservationService _reservationService;
        private readonly HMSdbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmployeeService _employeeService;

        public ReservationController(IReservationService reservationService, HMSdbContext context, UserManager<ApplicationUser> userManager,
            IEmployeeService employeeService)
        {
            _employeeService = employeeService;
            _userManager = userManager;
            _context = context;
            _reservationService = reservationService;
        }

        public IActionResult Index(int? page)
        {
            List<RoomReservationViewModel> reservation = new List<RoomReservationViewModel>();
            var pageNumber = page ?? 1;
            var pageSize = 10;
            reservation = _reservationService.GetReservations();
          
            return View(reservation.ToPagedList(pageNumber, pageSize));
        }

        public IActionResult Details(int id)
        {
            var result = _reservationService.GetReservationById(id);

            return View(result);
        }

        public IActionResult Create(int id)
        {
            var result = _reservationService.GetReservationsById(id);
            return View();
        }
        public async Task<IActionResult> Created(RoomReservationViewModel viewModel)
        {
            var result = _reservationService.ReservationCreator(viewModel);

            result.FkEmpId = await GetUserAsEmp();

            if (_reservationService.DateValidator(viewModel) == false)
            {
                _context.Reservation.Add(result);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
                return RedirectToAction("Index");
        }
        public IActionResult Edit(int id)
        {
            var result = _reservationService.GetReservationById(id);
            return View(result);
        }
        public async Task<IActionResult> Edited(RoomReservationViewModel viewModel, Reservation reservation)
        {            
            viewModel.EmpId = await GetUserAsEmp();
            var result =  _reservationService.Update(viewModel, reservation);
            return RedirectToAction("Index");
                     
        }
        public IActionResult Delete(int id)
        {
            return View(_reservationService.GetReservationById(id));
        }
        public IActionResult Deleted(RoomReservationViewModel viewModel)
        {
            int x = viewModel.ReservationId;
            _reservationService.Delete(x);
            return RedirectToAction("Index");
        }


        public IActionResult Dates(Reservation reservation)
        {
            return View(reservation);
        }

        public async Task<int> GetUserAsEmp()
        {
            var user = await _userManager.GetUserAsync(User);
            var employees = _employeeService.GetAllEmployes();

            int empId = 0;

            foreach(var emp in employees)
            {                
                if(emp.Email == user.Email)
                {
                    empId = emp.EmpId;
                    break;
                }
               
            }

            return empId;
        }
    }
}