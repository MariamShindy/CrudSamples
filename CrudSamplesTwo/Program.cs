using CompanyCrudTwo.DatabaseSpecific;
using CompanyCrudTwo.EntityClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;
using SD.LLBLGen.Pro.DQE.SqlServer; 
using System.Diagnostics;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace CrudSamplesTwo
{
    //1] Open llblgen , create project
    //2] Reverse engineering tables to entities
    //3] Generate code
    //4] Add code to class library in my project
    //5] Add project reference to class library
    //6] Add connection string file
    //7] Use DataAccessAdapter to make CRUD Operations 

    //Note : I can make the same using entity framework core , but then i will use database context not DataAccessAdapter
    internal class Program
    {
        static void Main(string[] args)
        {
            #region Config
            //RuntimeConfiguration.ConfigureDQE<SQLServerDQEConfiguration>(
            //    c => c.SetTraceLevel(TraceLevel.Verbose)
            //          .AddDbProviderFactory(typeof(SqlClientFactory)));

            //var config = new ConfigurationBuilder()
            //.AddJsonFile("appsettings.json" , optional: false, reloadOnChange: true)
            //.Build();

            //string connectionString = config.GetConnectionString("DefaultConnectionString")!;
            //var adapter = new DataAccessAdapter(connectionString); 
            #endregion

            #region Add
            //var newDepartment = new DepartmentEntity
            //{
            //    Name = "DepartmentOne",
            //};
            //adapter.SaveEntity(newDepartment, true);

            //var newEmployee = new EmployeeEntity
            //{
            //    Name = "Mariam",
            //    DepartmentId = 4,
            //    Salary = 3000
            //};

            //adapter.SaveEntity(newEmployee, true);
            //Console.WriteLine("Employee added successfully."); 

            //var newEmployeeTwo = new EmployeeEntity
            //{
            //    Name = "Manar",
            //    DepartmentId = 3,
            //    Salary = 4000
            //};

            //adapter.SaveEntity(newEmployeeTwo, true);
            //Console.WriteLine("Employee Two added successfully.");
            #endregion

            #region Update
            //var employee = new EmployeeEntity(4);
            //if (adapter.FetchEntity(employee))
            //{
            //    employee.Salary = 20000;
            //    adapter.SaveEntity(employee, true);
            //    Console.WriteLine("Employee updated successfully.");
            //}
            //else
            //    Console.WriteLine("Employee not found.");
            #endregion

            #region Get
            //var employee = new EmployeeEntity(3);
            //if (adapter.FetchEntity(employee))
            //    Console.WriteLine($"Employee: {employee.Name}, Salary: {employee.Salary}");
            //else
            //    Console.WriteLine("Employee not found.");
            #endregion

            #region Delete
            //var employee = new EmployeeEntity(4);
            //if (adapter.FetchEntity(employee))
            //{
            //    adapter.DeleteEntity(employee);
            //    Console.WriteLine("Employee deleted successfully.");
            //}
            //else
            //    Console.WriteLine("Employee not found.");
            #endregion
        }
    }
}
