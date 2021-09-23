using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManagementSystem.Models.ViewModels
{
	public class RoomReservationViewModel
	{
		public int RoomNumber { get; set; }
		public int? MaxGuests { get; set; }
		public int? RoomFloor { get; set; }
		public string RoomType { get; set; }
		public int? RoomStatusId { get; set; }
		public int ReservationId { get; set; }
		public string EmployeeName { get; set; }
		public int GuestId { get; set; }
		public string GuestName { get; set; }
		public int GuestPhoneNumber { get; set; }
		public int FoodOptionId { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public int EmpId { get; set; }

	}
}
