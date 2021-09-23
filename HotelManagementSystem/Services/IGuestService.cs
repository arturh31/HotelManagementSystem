using HotelManagementSystem.Models;
using System.Collections.Generic;

namespace HotelManagementSystem.Services
{
	public interface IGuestService
	{
		Guest CreateGuest(Guest newGuest);
		void DeleteGuest(int id);
		List<Guest> GetAllGuests();
		Guest GetGuestById(int id);
		Guest UpdateGuest(Guest editedGuest);
		
	}
}