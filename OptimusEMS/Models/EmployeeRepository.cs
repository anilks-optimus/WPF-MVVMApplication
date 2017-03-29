using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimusEMS.Models
{
	/// <summary>
	/// EmployeeRepository provides mechanism to interact with storage.
	/// Uses a temp collection for storage. Can be extended to store in DB.
	/// </summary>
	class EmployeeRepository
	{
		//Maintains the Employee collection locally
		private static List<Employee> employees = new List<Employee>();

		/// <summary>
		/// Add a Employee 
		/// </summary>
		/// <param name="employee">Employee to Add</param>
		internal void Add(Employee employee)
		{
			employees.Add(employee);
		}

		/// <summary>
		/// Remove a Employee based on 
		/// </summary>
		/// <param name="employee">Employee to remove</param>
		internal void Remove(Employee employee)
		{
			employees.Remove(employee);
		}

		/// <summary>
		/// Search for the Employee with Employee ID
		/// </summary>
		/// <param name="id">Employee ID</param>
		/// <returns></returns>
		internal Employee Search(int id)
		{
			//Get the employees index in the collection
			int index = GetIndex(id);
			//If present then return the element
			if (index > -1)
				return employees[index];
			return null;
		}

		/// <summary>
		/// Search for the Employee ID in the collection and return the Index
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		private int GetIndex(int id)
		{
			int index = -1;
			//If Collection has Items
			if (employees.Count > 0)
			{
				//Loop through the collection
				for (int i = 0; i < employees.Count; i++)
				{
					//If match
					if (employees[i].Id == id)
					{
						index = i;
						break;
					}
				}
			}
			return index;
		}
	}
}
