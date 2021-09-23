using System;
using System.Collections.Generic;

namespace HotelManagementSystem.Models
{
    public partial class RoomStatus
    {
        public RoomStatus()
        {
            Room = new HashSet<Room>();
        }

        public int RoomStatusId { get; set; }
        public int RoomStatusCode { get; set; }

        public virtual ICollection<Room> Room { get; set; }
    }
}
