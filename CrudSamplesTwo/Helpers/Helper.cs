using CompanyCrudTwo.DatabaseSpecific;
using CompanyCrudTwo.EntityClasses;
using System.Data;

namespace CrudSamplesTwo.Helpers
{
    internal class Helper
    {
        public static void PerformEmployeeOperation(DataAccessAdapter adapter,int employeeId, IsolationLevel isolationLevel)
        {
            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} - Starting operation on Employee {employeeId} with Isolation Level {isolationLevel}");
            Console.WriteLine("**********************************************************************************");
            var _adapter = new DataAccessAdapter(adapter.ConnectionString);
            _adapter.StartTransaction(isolationLevel, "MyFirstTrans");
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


        public static void PerformDepartmentOperation(DataAccessAdapter adapter,int departmentId, IsolationLevel isolationLevel)
        {
            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} - Starting operation on Department {departmentId} with Isolation Level {isolationLevel}");
            Console.WriteLine("**********************************************************************************");
            var _adapter = new DataAccessAdapter(adapter.ConnectionString);

            _adapter.StartTransaction(isolationLevel, "MySecondTrans");
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
    }
}



