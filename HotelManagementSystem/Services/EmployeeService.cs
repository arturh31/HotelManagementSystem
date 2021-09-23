using HotelManagementSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace HotelManagementSystem.Services
{
	public class EmployeeService : IEmployeeService
	{
		private readonly HMSdbContext _context;
		private UserManager<ApplicationUser> _userManager;
		SignInManager<ApplicationUser> _signInManager;

		public EmployeeService(HMSdbContext context)
		{
			_context = context;
		}
		
		
		public List<Employee> GetAllEmployes()
		{
			var result = _context.Employee.ToList();
			return result;
		}

		public Employee GetEmployeeByEmail(string email)
		{
			var result = _context.Employee.Where(c => c.Email == email).FirstOrDefault();
					
			return result;
		}

		public Employee GetEmpById(int id)
		{
			var employee = _context.Employee.Find(id);
			return employee;
		}

		public Employee Create(Employee newEmployee)
		{
			var allEmp = GetAllEmployes();
			bool isUnique = true;

			foreach (var item in allEmp)
			{
				if (item.PhoneNumber == newEmployee.PhoneNumber || item.MobileAppId == newEmployee.MobileAppId)
				{
					isUnique = false;
					break;
				}
				else
					isUnique = true;
			}
		
			if(isUnique == true)
			{
				_context.Add(newEmployee);
				_context.SaveChanges();
				return newEmployee;
			}
			else
			{
				return new Employee();
			}
		}
		public Employee Update(Employee updatedEmployee)
		{
			var allEmp = GetAllEmployes();
			bool isUnique = true;

			foreach (var item in allEmp)
			{
				if (item.PhoneNumber == updatedEmployee.PhoneNumber || item.MobileAppId == updatedEmployee.MobileAppId)
				{
					isUnique = false;
					break;
				}
				else
					isUnique = true;
			}

			if (isUnique == true)
			{
				var entity = _context.Employee.Attach(updatedEmployee);
				entity.State = EntityState.Modified;
				_context.SaveChanges();

				return updatedEmployee;
			}
			else
			{
				return new Employee();
			}
		}
	}
}
