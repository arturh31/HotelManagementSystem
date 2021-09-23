using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Identity;

namespace HotelManagementSystem.Models
{
	public class ApplicationRole : IdentityRole<long>
	{
		public ApplicationRole(string Name)
		{

		}
	}
}
