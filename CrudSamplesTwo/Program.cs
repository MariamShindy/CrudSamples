﻿using CompanyCrudTwo.DatabaseSpecific;
using SD.LLBLGen.Pro.ORMSupportClasses;
using SD.LLBLGen.Pro.DQE.SqlServer;
using System.Diagnostics;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using CrudSamplesTwo.Helpers;
using Microsoft.Extensions.DependencyInjection;
using CrudSamplesTwo.Dtos;

namespace CrudSamplesTwo
{
    #region Notes
    #region Topics
    /*
     * essential CRUD Add,Update,Delete,Get [Done]
       transactions, IsolationLevel [Done]
       add bulk, EntityCollection [Done]
       edit bulk, EntityCollection [Done]
       delete entities directly, RelationPredicateBucket [Done]
       DTOs, ProjectionParams [DTOs Only done]
       filter child list in the DTOs
       prefetch
     https://www.llblgen.com/Documentation/5.9/LLBLGen%20Pro%20RTF/index.htm
     */
    #endregion

    #region CRUD using llblgen
    //1] Open llblgen , create project
    //2] Reverse engineering tables to entities
    //3] Generate code
    //4] Add code to class library in my project
    //5] Add project reference to class library
    //6] Add connection string file
    //7] Use DataAccessAdapter to make CRUD Operations 

    //Note : I can make the same using entity framework core , but then i will use database context not DataAccessAdapter

    #endregion

    #region Isolation levels & Transactions
    /*
     * Transactions: A transaction is a sequence of operations performed as a single logical unit of work.
     * A transaction has four key properties, known as ACID (Atomicity, Consistency, Isolation, Durability)
     */

    /*
     * Isolation Levels: This determines how transaction integrity is visible to other transactions and systems.
     * The common isolation levels are:
       1] Read Uncommitted
       2] Read Committed
       3] Repeatable Read
       4] Serializable
     */

    /*1. ReadUncommitted (Lowest Isolation Level)
       What It Does: Allows reading data that has been modified but not yet committed by another transaction.
       Risk: Dirty reads – You might read data that could later be rolled back, meaning the data might not actually exist or be correct.

       2. ReadCommitted
       What It Does: Allows reading only data that has been committed (saved), so you won’t read "dirty" data.
       Risk: Non-repeatable reads – If you read data, another transaction can modify it before you complete your work, so the data might change.

       3. RepeatableRead
       What It Does: Ensures that once you read data, no one else can change it until your transaction is complete.
       Risk: Phantom reads – While you’re working with data, another transaction can insert new rows that match your query, affecting your results.

       4. Serializable (Highest Isolation Level)
       What It Does: Completely isolates your transaction from others. No other transaction can read, modify, or insert data that your transaction is working with.
       Risk: It is the slowest because it prevents other transactions from interacting with the data, which can lead to lower performance.
      */
    #endregion

    #region Entity collection & add and update bulk
    /*
     * EntityCollection: A class in LLBLGen that represents a collection of entities of the same type
     */
    /*
     * To insert some entities, you put them in a EntityCollection 
     * and save the EntityCollection, they will be inserted one after
     * the other in a transaction
     */
    #endregion

    #region Delete entities directly, RelationPredicateBucket
    /*
     * RelationPredicateBucket : used as a single unit to pass to a data-access
     * adapter for filtering over multi-entities [Based on built in delegate
     * Presicate]
     * 
     * RelationPredicateBucket : is a container to many predicates
     * Usage :-
     * 1] Multiple Conditions
     * 2] Joins and Relationships
     * 3] Complex Queries
     */
    #endregion

    #region DTOS , ProjectionParams
    /*
     * Data Transfer Objects (DTOs) : is used to encapsulate data, 
     * and send it from one subsystem of an application to another.
     */
    #endregion
    #endregion

    internal class Program
    {
        static void Main(string[] args)
        {
            #region Config
            RuntimeConfiguration.ConfigureDQE<SQLServerDQEConfiguration>(
                c => c.SetTraceLevel(TraceLevel.Verbose)
                      .AddDbProviderFactory(typeof(SqlClientFactory)));

            var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

            string connectionString = config.GetConnectionString("DefaultConnectionString")!;
            var adapter = new DataAccessAdapter(connectionString);

            var services = new ServiceCollection();

            services.AddTransient(provider => new DataAccessAdapter(connectionString));

            services.AddSingleton<Helper>();

            var serviceProvider = services.BuildServiceProvider();

            var helper = serviceProvider.GetService<Helper>();
            #endregion

            #region CRUD

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
            #endregion

            #region Isolation level & Transactions
            #region ReadCommitted
            //try
            //{
            //    adapter.StartTransaction(IsolationLevel.ReadCommitted, "MyFirstTrans");

            //    var newDepartment = new DepartmentEntity
            //    {
            //        Name = "DepartmentTest",
            //    };
            //    adapter.SaveEntity(newDepartment, true);

            //    var newEmployee = new EmployeeEntity
            //    {
            //        Name = "Amir",
            //        DepartmentId = newDepartment.DepartmentId,
            //        Salary = 10000
            //    };
            //    adapter.SaveEntity(newEmployee, true);

            //    adapter.Commit();
            //    Console.WriteLine("Transaction committed successfully.");
            //}
            //catch (Exception ex)
            //{
            //    adapter.Rollback();
            //    Console.WriteLine("Transaction rolled back due to an error: " + ex.Message);
            //}

            #endregion

            #region ReadUnCommitted
            //try
            //{
            //    adapter.StartTransaction(IsolationLevel.ReadUncommitted,"MySecondTrans");

            //    var employee = new EmployeeEntity(3);

            //    if (adapter.FetchEntity(employee))
            //    {
            //        employee.Salary = 1000;
            //        //adapter.SaveEntity(employee, false);
            //        adapter.SaveEntity(employee,true);
            //        Console.WriteLine($"Employee Name: {employee.Name}, Salary: {employee.Salary}");
            //    }
            //    else
            //        Console.WriteLine("Employee not found.");

            //    adapter.Commit();
            //}
            //catch (Exception ex)
            //{
            //    adapter.Rollback();
            //    Console.WriteLine("Transaction rolled back: " + ex.Message);
            //}

            //Example with 2 transactions using Threads
            //var adapter1 = new DataAccessAdapter(connectionString);
            //var adapter2 = new DataAccessAdapter(connectionString);

            //var transaction1Thread = new Thread(() =>
            //{
            //    try
            //    {
            //        adapter1.StartTransaction(IsolationLevel.ReadUncommitted, "ReadTransaction");

            //        var employee = new EmployeeEntity(3);
            //        if (adapter1.FetchEntity(employee))
            //        {
            //            Console.WriteLine($"[Transaction 1] Initial Salary: {employee.Salary}");

            //            Thread.Sleep(5000);

            //            if (adapter1.FetchEntity(employee))
            //            {
            //                Console.WriteLine($"[Transaction 1] Salary after operations: {employee.Salary}");
            //            }
            //        }
            //        else
            //        {
            //            Console.WriteLine("[Transaction 1] Employee not found.");
            //        }

            //        adapter1.Commit();
            //    }
            //    catch (Exception ex)
            //    {
            //        adapter1.Rollback();
            //        Console.WriteLine("[Transaction 1] Transaction rolled back: " + ex.Message);
            //    }
            //});

            //var transaction2Thread = new Thread(() =>
            //{
            //    Thread.Sleep(2000);
            //    try
            //    {
            //        adapter2.StartTransaction(IsolationLevel.ReadUncommitted, "UpdateTransaction");

            //        var employee = new EmployeeEntity(3);
            //        if (adapter2.FetchEntity(employee))
            //        {
            //            employee.Salary += 5300; // Update the salary
            //            adapter2.SaveEntity(employee);
            //            Console.WriteLine("[Transaction 2] Salary updated.");
            //        }
            //        else
            //        {
            //            Console.WriteLine("[Transaction 2] Employee not found.");
            //        }

            //        adapter2.Commit();
            //    }
            //    catch (Exception ex)
            //    {
            //        adapter2.Rollback();
            //        Console.WriteLine("[Transaction 2] Transaction rolled back: " + ex.Message);
            //    }
            //});

            //transaction1Thread.Start();
            //transaction2Thread.Start();

            //transaction1Thread.Join();
            //transaction2Thread.Join();
            #endregion

            #region RepeatableRead 
            //try
            //{
            //    adapter.StartTransaction(IsolationLevel.RepeatableRead,"MyThirdTrans");
            //    var employee = new EmployeeEntity(3);
            //    if (adapter.FetchEntity(employee))
            //    {
            //        Console.WriteLine($"Initial Salary: {employee.Salary}");

            //        Thread.Sleep(2000);

            //        if (adapter.FetchEntity(employee))
            //            Console.WriteLine($"Salary after operations: {employee.Salary}");
            //    }
            //    else
            //        Console.WriteLine("Employee not found.");

            //    adapter.Commit();
            //}
            //catch (Exception ex)
            //{
            //    adapter.Rollback();
            //    Console.WriteLine("Transaction rolled back: " + ex.Message);
            //}

            //Example with 2 transactions using Threads
            //var adapter1 = new DataAccessAdapter(connectionString);
            //var adapter2 = new DataAccessAdapter(connectionString);

            //var transaction1Thread = new Thread(() =>
            //{
            //    try
            //    {
            //        adapter1.StartTransaction(IsolationLevel.RepeatableRead, "ReadTransaction");

            //        var employee = new EmployeeEntity(3);
            //        if (adapter1.FetchEntity(employee))
            //        {
            //            Console.WriteLine($"[Transaction 1] Initial Salary: {employee.Salary}");

            //            Thread.Sleep(5000);

            //            if (adapter1.FetchEntity(employee))
            //            {
            //                Console.WriteLine($"[Transaction 1] Salary after operations: {employee.Salary}");
            //            }
            //        }
            //        else
            //        {
            //            Console.WriteLine("[Transaction 1] Employee not found.");
            //        }

            //        adapter1.Commit();
            //    }
            //    catch (Exception ex)
            //    {
            //        adapter1.Rollback();
            //        Console.WriteLine("[Transaction 1] Transaction rolled back: " + ex.Message);
            //    }
            //});

            //var transaction2Thread = new Thread(() =>
            //{
            //    Thread.Sleep(2000);
            //    try
            //    {
            //        adapter2.StartTransaction(IsolationLevel.ReadCommitted, "UpdateTransaction");

            //        var employee = new EmployeeEntity(3);
            //        if (adapter2.FetchEntity(employee))
            //        {
            //            employee.Salary += 5300; // Update the salary
            //            adapter2.SaveEntity(employee);
            //            Console.WriteLine("[Transaction 2] Salary updated.");
            //        }
            //        else
            //        {
            //            Console.WriteLine("[Transaction 2] Employee not found.");
            //        }

            //        adapter2.Commit();
            //    }
            //    catch (Exception ex)
            //    {
            //        adapter2.Rollback();
            //        Console.WriteLine("[Transaction 2] Transaction rolled back: " + ex.Message);
            //    }
            //});

            //transaction1Thread.Start();
            //transaction2Thread.Start();

            //transaction1Thread.Join();
            //transaction2Thread.Join();
            #endregion

            #region Serializable
            //Example with 2 transactions using Threads
            //var adapter1 = new DataAccessAdapter(connectionString);
            //var adapter2 = new DataAccessAdapter(connectionString);

            //var transaction1Thread = new Thread(() =>
            //{
            //    try
            //    {
            //      adapter1.StartTransaction(IsolationLevel.Serializable, "ReadTransaction");

            //        var employee = new EmployeeEntity(3);
            //        if (adapter1.FetchEntity(employee))
            //        {
            //            Console.WriteLine($"[Transaction 1] Initial Salary: {employee.Salary}");

            //            Thread.Sleep(5000);

            //            if (adapter1.FetchEntity(employee))
            //            {
            //                Console.WriteLine($"[Transaction 1] Salary after operations: {employee.Salary}");
            //            }
            //        }
            //        else
            //        {
            //            Console.WriteLine("[Transaction 1] Employee not found.");
            //        }

            //        adapter1.Commit();
            //    }
            //    catch (Exception ex)
            //    {
            //        adapter1.Rollback();
            //        Console.WriteLine("[Transaction 1] Transaction rolled back: " + ex.Message);
            //    }
            //});

            //var transaction2Thread = new Thread(() =>
            //{
            //    Thread.Sleep(2000);
            //    try
            //    {
            //        adapter2.StartTransaction(IsolationLevel.Serializable, "UpdateTransaction");

            //        var employee = new EmployeeEntity(3);
            //        if (adapter2.FetchEntity(employee))
            //        {
            //            employee.Salary += 5300; // Update the salary
            //            adapter2.SaveEntity(employee);
            //            Console.WriteLine("[Transaction 2] Salary updated.");
            //        }
            //        else
            //        {
            //            Console.WriteLine("[Transaction 2] Employee not found.");
            //        }

            //        adapter2.Commit();
            //    }
            //    catch (Exception ex)
            //    {
            //        adapter2.Rollback();
            //        Console.WriteLine("[Transaction 2] Transaction rolled back: " + ex.Message);
            //    }
            //});

            //transaction1Thread.Start();
            //transaction2Thread.Start();

            //transaction1Thread.Join();
            //transaction2Thread.Join();

            #endregion

            #region Advanced example
            //Thread thread1 = new Thread(() => helper!.PerformEmployeeOperation( 3, IsolationLevel.ReadCommitted));
            //Thread thread2 = new Thread(() => helper!.PerformEmployeeOperation( 5, IsolationLevel.RepeatableRead));
            //Thread thread4 = new Thread(() => helper!.PerformEmployeeOperation( 8, IsolationLevel.ReadCommitted));
            //Thread thread5 = new Thread(() => helper!.PerformDepartmentOperation( 4, IsolationLevel.Serializable));
            //Thread thread6 = new Thread(() => helper!.PerformDepartmentOperation( 5, IsolationLevel.RepeatableRead));
            //Thread thread3 = new Thread(() => helper!.PerformEmployeeOperation( 7, IsolationLevel.Serializable));

            //thread1.Start();
            //thread2.Start();
            //thread3.Start();
            //thread4.Start();
            //thread5.Start();
            //thread6.Start();

            //thread1.Join();
            //thread2.Join();
            //thread3.Join();
            //thread4.Join();
            //thread5.Join();
            //thread6.Join();

            //Console.WriteLine("Operations completed.");
            #endregion

            #endregion

            #region Bulk add & update , EntityCollection
            #region Bulk add
            //var allEmployees = helper!.FetchExistingEmployees();
            //foreach (var employee in allEmployees)
            //    Console.WriteLine($"{employee.EmployeeId} :: {employee.Name}");

            //var employees = new List<EmployeeEntity>
            //{
            //    new EmployeeEntity { Name = "Mahmoud", DepartmentId = 4, Salary = 5000 },
            //    new EmployeeEntity { Name = "Karim", DepartmentId = 5, Salary = 6400 },
            //    new EmployeeEntity { Name = "Samar", DepartmentId = 4, Salary = 5500 },
            //    new EmployeeEntity { Name = "Madiha", DepartmentId = 5, Salary = 6200 }
            //};

            //helper!.BulkInsertEmployees(employees);
            //Console.WriteLine("************************************");

            //var updatedEmployees = helper!.FetchExistingEmployees();
            //foreach (var employee in updatedEmployees)
            //    Console.WriteLine($"{employee.EmployeeId} :: {employee.Name}");


            //var allDepartments = helper!.FetchExistingDepartments();
            //foreach(var department in allDepartments)
            //    Console.WriteLine($"{department.DepartmentId} :: {department.Name}");

            //var departments = new List<DepartmentEntity>
            //{
            //    new DepartmentEntity {Name = "IS"},
            //    new DepartmentEntity {Name = "AI"},
            //    new DepartmentEntity {Name = "IT"},
            //    new DepartmentEntity {Name = "SC"},
            //};

            //helper!.BulkInsertDepartments(departments);
            //Console.WriteLine("************************************");

            //var updatedDepartments = helper!.FetchExistingDepartments();
            //foreach (var department in updatedDepartments)
            //    Console.WriteLine($"{department.DepartmentId} :: {department.Name}");
            #endregion

            #region Normal & bulk update
            //var allDepartments = helper!.FetchExistingDepartments();
            //Console.WriteLine("*********************************");
            //Console.WriteLine("All Departments before updating");   
            //Console.WriteLine("*********************************");

            //foreach (var department in allDepartments)
            //    Console.WriteLine($"{department.DepartmentId} :: {department.Name}");

            //helper!.UpdateDepartments(allDepartments);

            //Console.WriteLine("*********************************");
            //Console.WriteLine("All Departments after updating");
            //Console.WriteLine("*********************************");
            //var updatedDepartments = helper!.FetchExistingDepartments();
            //foreach (var department in updatedDepartments)
            //    Console.WriteLine($"{department.DepartmentId} :: {department.Name}");

            //var allEmployees = helper!.FetchExistingEmployees();
            //Console.WriteLine("*********************************");
            //Console.WriteLine("All Employees before updating");   
            //Console.WriteLine("*********************************");
            //foreach(var employee in allEmployees)
            //{
            //    Console.WriteLine($"{employee.EmployeeId} :: {employee.Name}");
            //}

            //helper!.UpdateEmployees(allEmployees);

            //var updatedEmployees = helper!.FetchExistingEmployees();
            //Console.WriteLine("*********************************");
            //Console.WriteLine("All Employees after updating");
            //Console.WriteLine("*********************************");
            //foreach (var employee in updatedEmployees)
            //{
            //    Console.WriteLine($"{employee.EmployeeId} :: {employee.Name}");
            //}


            //**************************Bulk-Update**************************
            //var allDepartments = helper!.FetchExistingDepartments();
            //Console.WriteLine("*********************************");
            //Console.WriteLine("All Departments before bulk updating");
            //Console.WriteLine("*********************************");

            //foreach (var department in allDepartments)
            //    Console.WriteLine($"{department.DepartmentId} :: {department.Name}");

            //int affectedRows = helper!.BulkUpdateDepartments();
            //Console.ForegroundColor = ConsoleColor.Green;
            //Console.WriteLine($"Updates done to {affectedRows} rows");
            //Console.ResetColor();

            //Console.WriteLine("*********************************");
            //Console.WriteLine("All Departments after bulk updating");
            //Console.WriteLine("*********************************");
            //var updatedDepartments = helper!.FetchExistingDepartments();
            //foreach (var department in updatedDepartments)
            //    Console.WriteLine($"{department.DepartmentId} :: {department.Name}");


            //var allEmployees = helper!.FetchExistingEmployees();
            //Console.WriteLine("*********************************");
            //Console.WriteLine("All Employees before bulk updating");
            //Console.WriteLine("*********************************");

            //foreach (var employee in allEmployees)
            //    Console.WriteLine($"{employee.Name} :: {employee.Salary}");

            //int affectedRows = helper!.BulkUpdateEmployees();
            //Console.ForegroundColor = ConsoleColor.Green;
            //Console.WriteLine($"Updates done to {affectedRows} rows");
            //Console.ResetColor();

            //Console.WriteLine("*********************************");
            //Console.WriteLine("All Employees after bulk updating");
            //Console.WriteLine("*********************************");
            //var updatedEmployees = helper!.FetchExistingEmployees();
            //foreach (var employee in updatedEmployees)
            //    Console.WriteLine($"{employee.Name} :: {employee.Salary}");

            #endregion

            #endregion

            #region Delete entities directly , RelationPredicateBucket
            //*******************Delete************************
            //int affectedRows = helper!.DeleteEmployees();
            //Console.WriteLine($"Affected rows of deleting employees whose salary = 5500 : {affectedRows}");

            //int affectedRows = helper!.DeleteDepartments();
            //Console.WriteLine($"Affected rows of deleting departments whose name IS : {affectedRows}");

            //*******************RelationPredicateBucket************************
            //Console.WriteLine("Showing Employees with their Departments started");
            //helper!.ShowEmployeesWithDepartment();
            #endregion

            #region DTOS
            //Convert from employees to employees dto
            //List<EmployeeDto> employees = helper!.GetEmployeeDtos();
            //foreach( EmployeeDto employeeDto in employees )
            //    Console.WriteLine($"{employeeDto.Name} :: {employeeDto.Salary}");

            //Update employee using dto
            //EmployeeDto employeeDto = new EmployeeDto()
            //{
            //    Name = "Mohamaden",
            //    EmployeeId = 1009,
            //    Salary = 10219
            //};
            //helper!.UpdateEmployeeFromDto(employeeDto);

            //Filter and show departments using dto
            //List<DepartmentDto> departmentDtos = helper!.GetFilteredDepartments("AI");
            //foreach(var departmentDto in departmentDtos)
            //    Console.WriteLine($"{departmentDto.DepartmentId} :: {departmentDto.Name}");

            #endregion
        }
    }
}
