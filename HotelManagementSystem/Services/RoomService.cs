using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using HotelManagementSystem.Models;
using HotelManagementSystem.Models.ViewModels;


namespace HotelManagementSystem.Services
{
	public class RoomService : IRoomService
	{
		private readonly HMSdbContext _context;

		public RoomService(HMSdbContext context)
		{
			_context = context;
		}

		public List<Room> GetAllRooms()
		{
			List<Room> result = new List<Room>();
			foreach(var row in _context.Room)
			{
				result.Add(row);
			}
			return result;
		}
		public Room GetRoomById(int id)
		{
			var result = _context.Room.Find(id);
			return result;
		}
		public Room Create(Room newRoom)
		{
			var rooms = GetAllRooms();
			bool roomValidation = false;

			foreach(var item in rooms)
			{
				if (item.RoomNumber == newRoom.RoomNumber)
				{
					roomValidation = false;
					break;
				}
				else
					roomValidation = true;
			}
			if (roomValidation == true)
			{
				_context.Add(newRoom);
				_context.SaveChanges();
			}
			return newRoom;
		}
		public Room Update(Room updatedRoom)
		{
			var entity = _context.Room.Attach(updatedRoom);
			entity.State = EntityState.Modified;
			_context.SaveChanges();
			return updatedRoom;
		}
		public void Delete(int id)
		{
			var result = _context.Room.Find(id);

			if(result != null)
			{
				_context.Room.Remove(result);				
			}
			_context.SaveChanges();
		}
		
	}
}
