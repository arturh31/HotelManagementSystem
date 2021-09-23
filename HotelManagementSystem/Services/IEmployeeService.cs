using HotelManagementSystem.Models;
using System.Collections.Generic;

namespace HotelManagementSystem.Services
{
	public interface IEmployeeService
	{
		Employee Create(Employee newEmployee);
		public Employee GetEmployeeByEmail(string email);
		List<Employee> GetAllEmployes();
		public Employee GetEmpById(int id);

		public Employee Update(Employee updatedEmployee);
	}
}