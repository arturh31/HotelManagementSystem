using System;
using System.Collections.Generic;

namespace HotelManagementSystem.Models
{
    public partial class Reservation
    {
        public int ReservationId { get; set; }
        public int FkGuestId { get; set; }
        public int FkEmpId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int FkFoodOptionId { get; set; }
        public int FkRoomId { get; set; }

        public virtual Employee FkEmp { get; set; }
        public virtual FoodOption FkFoodOption { get; set; }
        public virtual Guest FkGuest { get; set; }
        public virtual Room FkRoom { get; set; }
    }
}
