using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OptimusEMS.Models;
using System.ComponentModel;
using System.Windows.Input;
using OptimusEMS.Commands;
//using OptimusEMS.BusinessLogicLayer;
using System.Windows;
using System.Collections.ObjectModel;

namespace OptimusEMS.ViewModel
{
	class EmployeeDetailViewModel : INotifyPropertyChanged
	{
		#region Private Variables		
		private readonly Employee domObject;
		private readonly EmployeeManager employeeManager;
		private readonly ObservableCollection<Employee> _employees;
		private readonly ICommand _addEmployeeCmd;
		private readonly ICommand _deleteEmployeeCmd;
		private readonly ICommand _searchEmployeeCmd;
		#endregion

		#region Constructors

		/// <summary>
		/// Instatiates all the readonly variables
		/// </summary>
		public EmployeeDetailViewModel()
		{
			domObject = new Employee();
			employeeManager = new EmployeeManager();

			_employees = new ObservableCollection<Employee>();
			_addEmployeeCmd = new RelayCommand(Add, CanAdd);
			_deleteEmployeeCmd = new RelayCommand(Delete, CanDelete);
			_searchEmployeeCmd = new RelayCommand(Search, CanSearch);
		}
		#endregion

		#region Properties

		/// <summary>
		/// Gets or Sets Employee Id. Ready to be binded to UI.
		/// Impelments INotifyPropertyChanged which enables the binded element to refresh itself whenever the value changes.
		/// </summary>
		public int Id
		{
			get { return domObject.Id; }
			set
			{
				domObject.Id = value;
				OnPropertyChanged("Id");
			}
		}

		/// <summary>
		/// Gets or Sets Employee Name. Ready to be binded to UI.
		/// Impelments INotifyPropertyChanged which enables the binded element to refresh itself whenever the value changes.
		/// </summary>
		public string Name
		{
			get { return domObject.Name; }
			set
			{
				domObject.Name = value;
				OnPropertyChanged("Name");
			}
		}

		/// <summary>
		/// Gets or Sets Employee MobileNumber. Ready to be binded to UI.
		/// Impelments INotifyPropertyChanged which enables the binded element to refresh itself whenever the value changes.
		/// </summary>
		public Int64 MobileNumber
		{
			get { return domObject.MobileNumber; }
			set
			{
				domObject.MobileNumber = value;
				OnPropertyChanged("MobileNumber");
			}
		}

		/// <summary>
		/// Gets the Employees. Used to maintain the Employee List.
		/// Since this observable collection it makes sure all changes will automatically reflect in UI 
		/// as it implements both CollectionChanged, PropertyChanged;
		/// </summary>
		public ObservableCollection<Employee> Employees { get { return _employees; } }

		/// <summary>
		/// Sets the Selected Employee. Used to identify the selected Employee from the list. 
		/// </summary>
		public Employee SelectedEmployee
		{
			set
			{
				Id = value.Id;
				MobileNumber = value.MobileNumber;
				Name = value.Name;
			}
		}

		#region Commands

		/// <summary>
		/// Gets the AddEmployeeCommand. Used for Add Employee Button Operations
		/// </summary>
		public ICommand AddEmployeeCmd { get { return _addEmployeeCmd; } }

		/// <summary>
		/// Gets the DeleteEmployeeCmd. Used for Delete Employee Button Operations
		/// </summary>
		public ICommand DeleteEmployeeCmd { get { return _deleteEmployeeCmd; } }

		/// <summary>
		/// Gets the SearchEmployeeCmd. Used for Search Employee Button Operations
		/// </summary>
		public ICommand SearchEmployeeCmd { get { return _searchEmployeeCmd; } }
		#endregion

		#endregion

		#region Commands

		#region AddCommand

		/// <summary>
		/// CanAdd operation of the AddEmployeeCmd.
		/// Tells us if the control is to be enabled or disabled.
		/// This method will be fired repeatedly as long as the view is open.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public bool CanAdd(object obj)
		{
			//Enable the Button only if the mandatory fields are filled
			if (Name != string.Empty && MobileNumber != 0)
				return true;
			return false;
		}

		/// <summary>
		/// Add operation of the AddEmployeeCmd.
		/// Operation that will be performormed on the control click.
		/// </summary>
		/// <param name="obj"></param>
		public void Add(object obj)
		{
			//Always create a new instance of Employee before adding. 
			//Otherwise we will endup sending the same instance that is binded, to the logic which will cause complications
			var employee = new Employee { Id = Id, Name = Name, MobileNumber = MobileNumber };
			//Add Employee will be successfull only if the Employee with same ID does not exist.
			if (employeeManager.Add(employee))
			{
				Employees.Add(employee);
				ResetEmployee();
				MessageBox.Show("Employee Add Successful !");
			}
			else
				MessageBox.Show("Employee with this ID already exists !");
		}
		#endregion

		#region DeleteCommand

		/// <summary>
		/// CanDelete operation of the DeleteEmployeeCmd.
		/// Tells us if the control is to be enabled or disabled.
		/// This method will be fired repeatedly as long as the view is open.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		private bool CanDelete(object obj)
		{
			//Enable the Button only if the Employees exist
			if (Employees.Count > 0)
				return true;
			return false;
		}

		/// <summary>
		/// Delete operation of the DeleteEmployeeCmd.
		/// Operation that will be performormed on the control click.
		/// </summary>
		/// <param name="obj"></param>
		private void Delete(object obj)
		{
			//Delete Employee will be successfull only if the Employee with this ID exists.
			if (!employeeManager.Remove(Id))
				MessageBox.Show("Employee with this ID does not exist !");
			else
			{
				//Remove the Employee from our collection as well.
				Employees.RemoveAt(GetIndex(Id));
				ResetEmployee();
				MessageBox.Show("Employee Remove Successful !");
			}
		}

		#endregion

		#region SearchCommand

		/// <summary>
		/// CanSearch operation of the SearchEmployeeCmd.
		/// Tells us if the control is to be enabled or disabled.
		/// This method will be fired repeatedly as long as the view is open.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		private bool CanSearch(object obj)
		{
			//Enable the Button only if the Employees exist
			if (Employees.Count > 0)
				return true;
			return false;
		}

		/// <summary>
		/// Search operation of the SearchEmployeeCmd.
		/// Operation that will be performormed on the control click.
		/// </summary>
		/// <param name="obj"></param>
		private void Search(object obj)
		{
			Employee employee = employeeManager.Search(Id);

			if (employee == null)
				MessageBox.Show("Employee with this ID does not exist !");
			else
			{
				//Change the binded elements so that the searched Employee will reflect in UI
				Id = employee.Id;
				Name = employee.Name;
				MobileNumber = employee.MobileNumber;
			}
		}
		#endregion
		#endregion

		#region Private Methods

		/// <summary>
		/// Resets the Employee properties which will in turn auto reset the UI aswell
		/// </summary>
		private void ResetEmployee()
		{
			Id = 0;
			Name = string.Empty;
			MobileNumber = 0;
		}

		/// <summary>
		/// Finds the Employee in the collection and return the index
		/// </summary>
		/// <param name="Id">Employee ID</param>
		/// <returns></returns>
		private int GetIndex(int Id)
		{
			for (int i = 0; i < Employees.Count; i++)
				if (Employees[i].Id == Id)
					return i;
			return -1;
		}
		#endregion

		#region INotifyPropertyChanged Members

		/// <summary>
		/// Event to which the view's controls will subscribe.
		/// This will enable them to refresh themselves when the binded property changes provided you fire this event.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// When property is changed call this method to fire the PropertyChanged Event
		/// </summary>
		/// <param name="propertyName"></param>
		public void OnPropertyChanged(string propertyName)
		{
			//Fire the PropertyChanged event in case somebody subscribed to it
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
		#endregion
	}
}
