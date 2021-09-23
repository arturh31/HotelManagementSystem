using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotelManagementSystem.Models
{
    public partial class ShiftSchedule
    {
        public int ShiftId { get; set; }
        public int EmpId { get; set; }
        public int ShiftType { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-dd-mm}", ApplyFormatInEditMode = true)]
        public DateTime FromDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-dd-mm}", ApplyFormatInEditMode = true)]
        public DateTime ToDate { get; set; }
        public virtual Employee Emp { get; set; }
    }
}
