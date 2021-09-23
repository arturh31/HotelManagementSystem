using System;
using System.Collections.Generic;

namespace HotelManagementSystem.Models
{
    public partial class Guest
    {
        public Guest()
        {
            Reservation = new HashSet<Reservation>();
        }

        public int GuestId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int PhoneNumber { get; set; }
        public string EmailAddress { get; set; }

        public virtual ICollection<Reservation> Reservation { get; set; }
    }
}
