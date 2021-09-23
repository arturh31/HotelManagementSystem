using HotelManagementSystem.Models;
using System.Collections.Generic;

namespace HotelManagementSystem.Services
{
	public interface ITicketService
	{
		SupportTicket Create(SupportTicket newTicket);
		List<SupportTicket> GetAllTickets();
		SupportTicket GetTicketById(int id);
		SupportTicket Update(SupportTicket updatedTicket);
	}
}