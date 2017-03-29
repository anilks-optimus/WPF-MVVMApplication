using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimusEMS.Models
{
	class EmployeeManager
	{
		readonly EmployeeRepository employeeRepository;

		/// <summary>
		/// Initialises all the private variables
		/// </summary>
		public EmployeeManager()
		{
			employeeRepository = new EmployeeRepository();
		}

		/// <summary>
		/// Add Employee
		/// </summary>
		/// <param name="employee">Employee to add</param>
		/// <returns></returns>
		public bool Add(Employee employee)
		{
			//Search if the Employee exists and if not add the Employee.
			if (employeeRepository.Search(employee.Id) == null)
			{
				employeeRepository.Add(employee);
				return true;
			}
			return false;
		}

		/// <summary>
		/// Remove Employee
		/// </summary>
		/// <param name="id">Employee ID</param>
		/// <returns></returns>
		public bool Remove(int id)
		{
			//Search if the Employee exists and if exists remove the Employee.
			Employee employee = employeeRepository.Search(id);
			if (employee != null)
			{
				employeeRepository.Remove(employee);
				return true;
			}
			return false;
		}

		/// <summary>
		/// Search for a Employee
		/// </summary>
		/// <param name="id">Employee ID</param>
		/// <returns></returns>
		public Employee Search(int id)
		{
			return employeeRepository.Search(id);
		}
	}
}
