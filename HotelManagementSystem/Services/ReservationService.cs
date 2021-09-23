using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelManagementSystem.Models.ViewModels;
using HotelManagementSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementSystem.Services
{
	public class ReservationService : IReservationService
	{
		private readonly HMSdbContext _context;
		private readonly IRoomService _roomService;
		private readonly IEmployeeService _employeeService;
		private readonly IGuestService _guestService;
		private UserManager<ApplicationUser> _userManager;

		public ReservationService(HMSdbContext context)
		{
			_context = context;
		}

		public ReservationService(HMSdbContext context, IRoomService roomService, 
			IEmployeeService employeeService, IGuestService guestService, UserManager<ApplicationUser> userManager)
		{
			_context = context;
			_roomService = roomService;
			_employeeService = employeeService;
			_guestService = guestService;
			_userManager = userManager;
		}

		public List<RoomReservationViewModel> GetReservations()
		{
			List<RoomReservationViewModel> a = new List<RoomReservationViewModel>();
			var result = (from p in _context.Room
						  join m in _context.Reservation on p.RoomId equals m.FkRoomId
						  join g in _context.Guest on m.FkGuestId equals g.GuestId
						  join e in _context.Employee on m.FkEmpId equals e.EmpId
						  join f in _context.FoodOption on m.FkFoodOptionId equals f.FoodOptionId
						  select new RoomReservationViewModel
						  {
							  ReservationId = m.ReservationId,
							  RoomNumber = p.RoomNumber,
							  RoomType = p.RoomType,
							  RoomFloor = p.RoomFloor,
							  RoomStatusId = p.FkRoomStatusId,
							  StartDate = m.FromDate,
							  EndDate = m.ToDate,
							  GuestName = g.FirstName + " " + g.LastName,
							  GuestPhoneNumber = g.PhoneNumber,
							  EmployeeName = e.FirstName + " " + e.LastName,
							  MaxGuests = p.MaxGuests,
							  FoodOptionId = f.FoodOptionId,
							  GuestId = g.GuestId,

						  }).ToList();

			return result;
		}

		public List<RoomReservationViewModel> GetReservationsById(int id)
		{
			List<RoomReservationViewModel> a = new List<RoomReservationViewModel>();
			var result = (from p in _context.Room
						  join m in _context.Reservation on p.RoomId equals m.FkRoomId
						  join g in _context.Guest on m.FkGuestId equals g.GuestId
						  join e in _context.Employee on m.FkEmpId equals e.EmpId
						  join f in _context.FoodOption on m.FkFoodOptionId equals f.FoodOptionId
						  where m.FkRoomId == id
						  select new RoomReservationViewModel
						  {
							  ReservationId = m.ReservationId,
							  RoomNumber = p.RoomNumber,
							  RoomType = p.RoomType,
							  RoomFloor = p.RoomFloor,
							  RoomStatusId = p.FkRoomStatusId,
							  StartDate = m.FromDate,
							  EndDate = m.ToDate,
							  GuestName = g.FirstName + " " + g.LastName,
							  GuestPhoneNumber = g.PhoneNumber,
							  EmployeeName = e.FirstName + " " + e.LastName,
							  MaxGuests = p.MaxGuests,
							  FoodOptionId = f.FoodOptionId,
							  GuestId = g.GuestId,

						  }).ToList();

			return result;
		}

		public RoomReservationViewModel GetReservationById(int id)
		{
			var reservationList =  (from p in _context.Room
						  join m in _context.Reservation on p.RoomId equals m.FkRoomId
						  join g in _context.Guest on m.FkGuestId equals g.GuestId
						  join e in _context.Employee on m.FkEmpId equals e.EmpId
						  join f in _context.FoodOption on m.FkFoodOptionId equals f.FoodOptionId
						  where m.ReservationId == id
						  select new RoomReservationViewModel
						  {
							  ReservationId = m.ReservationId,
							  RoomNumber = p.RoomNumber,
							  RoomType = p.RoomType,
							  RoomFloor = p.RoomFloor,
							  RoomStatusId = p.FkRoomStatusId,
							  StartDate = m.FromDate,
							  EndDate = m.ToDate,
							  GuestName = g.FirstName + " " + g.LastName,
							  GuestPhoneNumber = g.PhoneNumber,
							  EmployeeName = e.FirstName + " " + e.LastName,
							  MaxGuests = p.MaxGuests,
							  FoodOptionId = f.FoodOptionId,
							  GuestId = g.GuestId,

						  }).ToList();

			var mapToModel = new RoomReservationViewModel();

			foreach(var item in reservationList)
			{
				mapToModel.RoomNumber = item.RoomNumber;
				mapToModel.RoomType = item.RoomType;
				mapToModel.RoomFloor = item.RoomFloor;
				mapToModel.RoomStatusId = item.RoomStatusId;
				mapToModel.ReservationId = item.ReservationId;
				mapToModel.StartDate = item.StartDate;
				mapToModel.EndDate = item.EndDate;
				mapToModel.ReservationId = item.ReservationId;
				mapToModel.MaxGuests = item.MaxGuests;
				mapToModel.GuestName = item.GuestName;
				mapToModel.GuestPhoneNumber = item.GuestPhoneNumber;
				mapToModel.EmployeeName = item.EmployeeName;
				mapToModel.GuestId = item.GuestId;
			}
					
			return mapToModel;
		}
	

		public Reservation ReservationCreator(RoomReservationViewModel viewModel)
		{
			Reservation reservation = new Reservation();
			var rooms = _roomService.GetAllRooms();
			//var employees = _employeeService.GetAllEmployyes();
			var guests = _guestService.GetAllGuests();
			
			foreach(var r in rooms)
			{
				if(viewModel.RoomNumber == r.RoomNumber)
				{
					reservation.FkRoomId = r.RoomId;
					break;
				}
				else
				{
					//GuestId = 3 will be set as not found Guest. It's set by defualt for this Id on database.
					reservation.FkRoomId = 1;
				}
			}
			foreach (var g in guests)
			{
				if (viewModel.GuestPhoneNumber == g.PhoneNumber)
				{
					reservation.FkGuestId = g.GuestId;
					break;
				}
				else
				{
					//GuestId = 3 will be set as not found Guest. It's set by defualt for this Id on database.
					reservation.FkGuestId = 3;
				}
			}
			reservation.FkEmpId = 0;
			reservation.FkFoodOptionId = viewModel.FoodOptionId;			
			reservation.FromDate = viewModel.StartDate;
			reservation.ToDate = viewModel.EndDate;
							
			return reservation;
		}

		public Reservation Update(RoomReservationViewModel viewModel, Reservation reservation)
		{
			reservation.ReservationId = viewModel.ReservationId;
			reservation.FkGuestId = viewModel.GuestId;
			reservation.FkEmpId = 0;
			var rooms = _roomService.GetAllRooms();
			//var employees = _employeeService.GetAllEmployyes();
			var guests = _guestService.GetAllGuests();

			foreach (var r in rooms)
			{
				if (viewModel.RoomNumber == r.RoomNumber)
				{
					reservation.FkRoomId = r.RoomId;
					break;
				}
				else
				{
					//GuestId = 3 will be set as not found Guest. It's set by defualt for this Id on database.
					reservation.FkRoomId = 1;
				}
			}
			foreach (var g in guests)
			{
				if (viewModel.GuestPhoneNumber == g.PhoneNumber)
				{
					reservation.FkGuestId = g.GuestId;
					break;
				}
				else
				{
					//GuestId = 3 will be set as not found Guest. It's set by defualt for this Id on database.
					reservation.FkGuestId = 3;
				}
			}
			reservation.FkEmpId = viewModel.EmpId;
			reservation.FkFoodOptionId = viewModel.FoodOptionId;
			reservation.FromDate = viewModel.StartDate;
			reservation.ToDate = viewModel.EndDate;

			var entity = _context.Reservation.Attach(reservation);
			entity.State = EntityState.Modified;
			_context.SaveChanges();

			return reservation;
		}

		public void Delete(int id)
		{
			var result = _context.Reservation.Find(id);

			if (result != null)
			{
				_context.Reservation.Remove(result);
			}
			_context.SaveChanges();
		}
		
		public bool DateValidator(RoomReservationViewModel viewModel)
		{
			var reservations = GetReservations();
			bool overlap = true;
			if(reservations.Count == 0)
			{
				return false;
			}

			foreach(var item in reservations)
			{
				
				if (viewModel.StartDate <= item.EndDate && item.StartDate <= viewModel.EndDate && item.RoomNumber == viewModel.RoomNumber)
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
