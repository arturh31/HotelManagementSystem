using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManagementSystem.Models.ViewModels
{
	public class EmployeeScheduleViewModel
	{
		public int ShiftId { get; set; }
		public int EmpId { get; set; }
		public string Email { get; set; }
		public string EmpName { get; set; }
		public int ShiftType { get; set; }
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:yyyy-dd-mm}", ApplyFormatInEditMode = true)]
		public DateTime FromDate { get; set; }
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:yyyy-dd-mm}", ApplyFormatInEditMode = true)]
		public DateTime ToDate { get; set; }



	}
}
