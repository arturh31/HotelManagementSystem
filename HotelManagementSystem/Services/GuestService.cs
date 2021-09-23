using HotelManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManagementSystem.Services
{
	public class GuestService : IGuestService
	{
		private readonly HMSdbContext _context;		
		public GuestService(HMSdbContext context)
		{
			_context = context;
		}
		

		public List<Guest> GetAllGuests()
		{
			List<Guest> allGuests = new List<Guest>();

			foreach (var item in _context.Guest)
			{
				allGuests.Add(item);
			}

			return allGuests;
		}

		public Guest GetGuestById(int id)
		{
			var guest = _context.Guest.Find(id);

			return guest;
		}

		public Guest CreateGuest(Guest newGuest)
		{
			_context.Guest.Add(newGuest);
			_context.SaveChanges();

			return newGuest;
		}

		public Guest UpdateGuest(Guest editedGuest)
		{
			var entity = _context.Guest.Attach(editedGuest);
			entity.State = EntityState.Modified;
			_context.SaveChanges();


			return editedGuest;
		}

		public void DeleteGuest(int id)
		{
			ReservationService service = new ReservationService(_context);
			var reservations = service.GetReservations();
			var guestToDelete = _context.Guest.Find(id);

			foreach (var item in reservations)
			{				
				if (guestToDelete.GuestId == item.GuestId)
				{
					var reservationToDelete = _context.Reservation.Find(item.ReservationId);
					_context.Reservation.Remove(reservationToDelete);
				}
			}

			if (guestToDelete != null)
			{
				_context.Guest.Remove(guestToDelete);
			}
			_context.SaveChanges();

		}


	}
}
