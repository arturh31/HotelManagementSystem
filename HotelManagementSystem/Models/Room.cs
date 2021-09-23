using System;
using System.Collections.Generic;

namespace HotelManagementSystem.Models
{
    public partial class Room
    {
        public Room()
        {
            Reservation = new HashSet<Reservation>();
        }

        public int RoomId { get; set; }
        public int RoomNumber { get; set; }
        public int MaxGuests { get; set; }
        public string RoomType { get; set; }
        public int RoomFloor { get; set; }
        public int? FkRoomStatusId { get; set; }

        public virtual RoomStatus FkRoomStatus { get; set; }
        public virtual ICollection<Reservation> Reservation { get; set; }
    }
}
