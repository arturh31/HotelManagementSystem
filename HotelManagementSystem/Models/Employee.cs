using System;
using System.Collections.Generic;

namespace HotelManagementSystem.Models
{
    public partial class Employee
    {
        public Employee()
        {
            Reservation = new HashSet<Reservation>();
            ShiftSchedule = new HashSet<ShiftSchedule>();
        }

        public int EmpId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int PhoneNumber { get; set; }
        public DateTime EmpFromDate { get; set; }
        public DateTime? EmpToDate { get; set; }
        public string MobileAppId { get; set; }
        public string Position { get; set; }
        public string Email { get; set; }

        public virtual ICollection<Reservation> Reservation { get; set; }
        public virtual ICollection<ShiftSchedule> ShiftSchedule { get; set; }
    }
}
