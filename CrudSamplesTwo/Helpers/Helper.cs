using CompanyCrudTwo.DatabaseSpecific;
using CompanyCrudTwo.EntityClasses;
using CompanyCrudTwo.HelperClasses;
using Microsoft.Extensions.DependencyInjection;
using SD.LLBLGen.Pro.ORMSupportClasses;
using System.Data;

namespace CrudSamplesTwo.Helpers
{
    internal class Helper
    {
        private readonly IServiceProvider _serviceProvider;

        public Helper(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        #region Transactions & Isoaltion level
        public void PerformEmployeeOperation( int employeeId, IsolationLevel isolationLevel)
        {
            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} - Starting operation on Employee {employeeId} with Isolation Level {isolationLevel}");
            Console.WriteLine("**********************************************************************************");
            using var _adapter = _serviceProvider.GetService<DataAccessAdapter>();
            _adapter!.StartTransaction(isolationLevel, "MyFirstTrans");
            try
            {
                var employee = new EmployeeEntity(employeeId);
                if (_adapter.FetchEntity(employee))
                {
                    employee.Salary += 1000; // Increase salary by 1000
                    _adapter.SaveEntity(employee, true);

                    Thread.Sleep(500);

                    Console.WriteLine($"Employee {employeeId} updated successfully.");
                }
                else
                {
                    Console.WriteLine($"Employee {employeeId} not found.");
                }

                _adapter.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in thread {Thread.CurrentThread.ManagedThreadId}: {ex.Message}");
                _adapter.Rollback();
            }
        }


        public void PerformDepartmentOperation(int departmentId, IsolationLevel isolationLevel)
        {
            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} - Starting operation on Department {departmentId} with Isolation Level {isolationLevel}");
            Console.WriteLine("**********************************************************************************");
            using var _adapter = _serviceProvider.GetService<DataAccessAdapter>();
            _adapter!.StartTransaction(isolationLevel, "MySecondTrans");
            try
            {
                var department = new DepartmentEntity(departmentId);
                if (_adapter.FetchEntity(department))
                {
                    department.Name += "Updated"; // Add "Updated" to department name
                    _adapter.SaveEntity(department, true);

                    Thread.Sleep(300);

                    Console.WriteLine($"Department {departmentId} updated successfully.");
                }
                else
                {
                    Console.WriteLine($"Department {departmentId} not found.");
                }

                _adapter.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in thread {Thread.CurrentThread.ManagedThreadId}: {ex.Message}");
                _adapter.Rollback();
            }
        }
        #endregion

        #region Bulk Insert , Update Using EntityCollection
        public void BulkInsertEmployees(List<EmployeeEntity> employees)
        {
            using var _adapter = _serviceProvider.GetService<DataAccessAdapter>();
            var employeeCollection = new EntityCollection<EmployeeEntity>(employees);
            _adapter!.SaveEntityCollection(employeeCollection);
            Console.WriteLine("Bulk insert of employees completed successfully.");
        }

        public void BulkInsertDepartments(List<DepartmentEntity> departments)
        {
            using var _adapter = _serviceProvider.GetService<DataAccessAdapter>();
            var departmentCollection = new EntityCollection<DepartmentEntity>(departments);
            _adapter!.SaveEntityCollection(departmentCollection);
            Console.WriteLine("Bulk insert of departments completed successfully.");
        }

        #endregion

        #region Helper methods
        public List<EmployeeEntity> FetchExistingEmployees()
        {
            Console.WriteLine("Fetching Employees started");
            using var _adapter = _serviceProvider.GetService<DataAccessAdapter>();
            var employeesCollection = new EntityCollection<EmployeeEntity>();
            _adapter!.FetchEntityCollection(employeesCollection, new RelationPredicateBucket());
            return new List<EmployeeEntity>(employeesCollection);
        }
        public List<DepartmentEntity> FetchExistingDepartments()
        {
            Console.WriteLine("Fetching Departments started");
            using var _adapter = _serviceProvider.GetService<DataAccessAdapter>();
            var departmentsCollection = new EntityCollection<DepartmentEntity>();
            _adapter!.FetchEntityCollection(departmentsCollection, new RelationPredicateBucket());
            return new List<DepartmentEntity>(departmentsCollection);
        }
        #endregion
    }
}



