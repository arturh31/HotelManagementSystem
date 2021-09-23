using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementSystem.Services
{
	public class TicketService : ITicketService
	{
		private readonly HMSdbContext _context;

		public TicketService(HMSdbContext context)
		{
			_context = context;
		}
		public List<SupportTicket> GetAllTickets()
		{
			List<SupportTicket> allTickets = new List<SupportTicket>();

			foreach (var item in _context.SupportTicket)
			{
				allTickets.Add(item);
			}

			return allTickets;
		}

		public SupportTicket GetTicketById(int id)
		{
			var singleTicket = _context.SupportTicket.Find(id);
			return singleTicket;
		}

		public SupportTicket Create(SupportTicket newTicket)
		{
			return newTicket;
		}

		public SupportTicket Update(SupportTicket updatedTicket)
		{
			var entity = _context.SupportTicket.Attach(updatedTicket);
			entity.State = EntityState.Modified;
			_context.SaveChanges();

			return updatedTicket;
		}

	}
}
