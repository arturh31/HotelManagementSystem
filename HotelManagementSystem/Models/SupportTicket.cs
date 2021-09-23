using System;
using System.Collections.Generic;

namespace HotelManagementSystem.Models
{
    public partial class SupportTicket
    {
        public int TicketId { get; set; }
        public int EmpId { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }
    }
}
