using System;
using System.Collections.Generic;

namespace HotelManagementSystem.Models
{
    public partial class FoodOption
    {
        public FoodOption()
        {
            Reservation = new HashSet<Reservation>();
        }

        public int FoodOptionId { get; set; }
        public string FoodOption1 { get; set; }

        public virtual ICollection<Reservation> Reservation { get; set; }
    }
}
