using HotelManagementSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelManagementSystem.Services
{
	public interface IRoomService
	{
		List<Room> GetAllRooms();
		Room GetRoomById(int id);
		Room Create(Room newRoom);
		Room Update(Room updatedRoom);

		void Delete(int id);
	}
}