using CompanyCrudTwo.DatabaseSpecific;
using CompanyCrudTwo.EntityClasses;
using CompanyCrudTwo.HelperClasses;
using Microsoft.Extensions.DependencyInjection;
using SD.LLBLGen.Pro.ORMSupportClasses;
using SD.LLBLGen.Pro.QuerySpec;
using System.Data;
using CrudSamplesTwo.Dtos;
using CompanyCrudTwo.FactoryClasses;
using SD.LLBLGen.Pro.QuerySpec.Adapter;
using CompanyCrudTwo.Linq;
using static System.Net.Mime.MediaTypeNames;
using CompanyCrudTwo;
using System.Transactions;

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
        public void PerformEmployeeOperation(int employeeId, System.Data.IsolationLevel isolationLevel)
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


        public void PerformDepartmentOperation(int departmentId, System.Data.IsolationLevel isolationLevel)
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
        public void UpdateEmployees(List<EmployeeEntity> employees)
        {
            using var _adapter = _serviceProvider.GetService<DataAccessAdapter>();
            var employeeCollection = new EntityCollection<EmployeeEntity>(employees);
            foreach (var employee in employeeCollection)
            {
                employee.Name += "Update";
            }
            _adapter!.SaveEntityCollection(employeeCollection, true, false);
        }
        public void UpdateDepartments(List<DepartmentEntity> departments)
        {
            using var _adapter = _serviceProvider.GetService<DataAccessAdapter>();
            var departmentCollection = new EntityCollection<DepartmentEntity>(departments);
            foreach (var department in departmentCollection)
            {
                department.Name += "Update";
            }
            _adapter!.SaveEntityCollection(departmentCollection, true, false);
        }
        public int BulkUpdateEmployees()
        {
            using var _adapter = _serviceProvider.GetService<DataAccessAdapter>();
            var updatedEmployee = new EmployeeEntity
            {
                Salary = 10000
            };
            var filter = new RelationPredicateBucket(EmployeeFields.Salary > 7000);
            int affectedRows = _adapter!.UpdateEntitiesDirectly(updatedEmployee, filter);
            return affectedRows;
        }
        public int BulkUpdateDepartments()
        {
            using var _adapter = _serviceProvider.GetService<DataAccessAdapter>();
            var updatedDepartment = new DepartmentEntity
            {
                Name = "Updated department"
            };
            var filter = new RelationPredicateBucket(DepartmentFields.DepartmentId < 10);
            int affectedRows = _adapter!.UpdateEntitiesDirectly(updatedDepartment, filter);
            return affectedRows;
        }
        #endregion

        #region Delete entities directly , RelationPredicateBucket
        public int DeleteEmployees()
        {
            using var _adapter = _serviceProvider.GetService<DataAccessAdapter>();
            RelationPredicateBucket filter = new RelationPredicateBucket();
            var deleteFilter = new PredicateExpression(EmployeeFields.Salary == 5500);
            filter.PredicateExpression.Add(deleteFilter);
            int deletedRows = _adapter!.DeleteEntitiesDirectly(typeof(EmployeeEntity), filter);
            return deletedRows;
        }

        public int DeleteDepartments()
        {
            //After I change the deleteRule between emp & dep to be ==> SetNull in database
            using var _adapter = _serviceProvider.GetService<DataAccessAdapter>();
            RelationPredicateBucket filter = new RelationPredicateBucket();
            var deleteFilter = new PredicateExpression(DepartmentFields.Name == "IS");
            filter.PredicateExpression.Add(deleteFilter);
            int deletedRows = _adapter!.DeleteEntitiesDirectly(typeof(DepartmentEntity), filter);
            return deletedRows;
        }

        //RelationPredicateBucket to add relation
        public void ShowEmployeesWithDepartment()
        {
            using var _adapter = _serviceProvider.GetService<DataAccessAdapter>();
            var relationPredicate = new RelationPredicateBucket();
            var employeeDepartmentRelation = EmployeeEntity.Relations.DepartmentEntityUsingDepartmentId;
            relationPredicate.Relations.Add(employeeDepartmentRelation);

            var employeeCollection = new EntityCollection<EmployeeEntity>();
            _adapter!.FetchEntityCollection(employeeCollection, relationPredicate);

            foreach (var employee in employeeCollection)
            {
                Console.WriteLine($"Employee: {employee.Name}, Department: {employee.DepartmentId}");
            }
        }

        #endregion

        #region DTOS , ProjectionParams
        public List<EmployeeDto> GetEmployeeDtos()
        {
            using var _adapter = _serviceProvider.GetService<DataAccessAdapter>();
            var employeesCollection = new EntityCollection<EmployeeEntity>();
            _adapter!.FetchEntityCollection(employeesCollection, new RelationPredicateBucket());

            var employeeDtos = employeesCollection.Select(employee => new EmployeeDto
            {
                EmployeeId = employee.EmployeeId,
                Name = employee.Name,
                Salary = employee.Salary
            }).ToList();

            Console.WriteLine("Fetched Employees and mapped to DTOs.");
            return employeeDtos;
        }
        public void UpdateEmployeeFromDto(EmployeeDto employeeDto)
        {
            using var _adapter = _serviceProvider.GetService<DataAccessAdapter>();
            var employee = new EmployeeEntity(employeeDto.EmployeeId);
            _adapter!.FetchEntity(employee);
            employee.Name = employeeDto.Name;
            employee.Salary = employeeDto.Salary;
            _adapter!.SaveEntity(employee, true, false);
            Console.WriteLine("Employee updated from DTO.");
            Console.WriteLine($"{employee.EmployeeId} :: {employee.Name} :: {employee.Salary}");
        }
        public List<DepartmentDto> GetFilteredDepartments(string departmentName)
        {
            using var _adapter = _serviceProvider.GetService<DataAccessAdapter>();
            var departmentsCollection = new EntityCollection<DepartmentEntity>();
            var filter = new PredicateExpression(DepartmentFields.Name == departmentName);
            var predicateBucket = new RelationPredicateBucket(filter);
            _adapter!.FetchEntityCollection(departmentsCollection, predicateBucket);

            var departmentDtos = departmentsCollection.Select(department => new DepartmentDto
            {
                DepartmentId = department.DepartmentId,
                Name = department.Name
            }).ToList();

            Console.WriteLine("Filtered and mapped departments to DTOs.");
            return departmentDtos;
        }

        //Projection Params
        public List<(string Name, decimal Salary)> FetchEmployeeNamesAndSalaries()
        {
            using var _adapter = _serviceProvider.GetService<DataAccessAdapter>();
            var qf = new QueryFactory();
            var q = qf.Employee
                      .Where(EmployeeFields.Salary > 5000)
                      .Select(() => new
                      {
                          Name = EmployeeFields.Name.ToValue<string>(),
                          Salary = EmployeeFields.Salary.ToValue<decimal>()
                      });

            var results = _adapter?.FetchQuery(q);
            var tuples = results?.Select(e => (e.Name, e.Salary)).ToList();
            Console.WriteLine("Fetched Employee names and salaries using projection.");
            return tuples ?? new List<(string Name, decimal Salary)>();
        }
        public List<(int Id, string Name, int DeptId)> FetchEmployeeWithDepartment(int DepId)
        {
            using var _adapter = _serviceProvider.GetService<DataAccessAdapter>();
            var qf = new QueryFactory();
            var q = qf.Employee
                      .Where(EmployeeFields.DepartmentId == DepId)
                      .Select(() => new
                      {
                          Id = EmployeeFields.EmployeeId.ToValue<int>(),
                          Name = EmployeeFields.Name.ToValue<string>(),
                          DeptId = EmployeeFields.DepartmentId.ToValue<int>()
                      });
            var results = _adapter?.FetchQuery(q);
            var tuples = results?.Select(e => (e.Id, e.Name, e.DeptId)).ToList();
            Console.WriteLine($"Fetched Employees ids and names in department {DepId} using projection.");
            return tuples ?? new List<(int Id, string Name, int DeptId)>();
        }
        public List<(int Id, string Name)> FetchDepartmentsIdsAndNames()
        {
            using var _adapter = _serviceProvider.GetService<DataAccessAdapter>();
            var qf = new QueryFactory();
            var q = qf.Department
                      .Where(DepartmentFields.DepartmentId > 1000)
                      .Select(() => new
                      {
                          Id = DepartmentFields.DepartmentId.ToValue<int>(),
                          Name = DepartmentFields.Name.ToValue<string>()
                      });

            var results = _adapter?.FetchQuery(q);
            var tuples = results?.Select(d => (d.Id, d.Name)).ToList();
            Console.WriteLine("Fetched Departments ids and names using projection.");
            return tuples ?? new List<(int Id, string Name)>();
        }

        public List<(int DepartmentId, string DepartmentName, int EmployeeCount)> GetEmployeeCountPerDepartment()
        {
            List<(int DepartmentId, string DepartmentName, int EmployeeCount)> departmentDetails = new List<(int, string, int)>();
            var _adapter = _serviceProvider.GetService<DataAccessAdapter>();
            var query = new LinqMetaData(_adapter)
                    .Employee
                     .Where(e => e.DepartmentId != null)
                    .GroupBy(e => new { e.DepartmentId, e.Department.Name })
                    .Select(g => new
                    {
                        g.Key.DepartmentId,
                        g.Key.Name,
                        EmployeeCount = g.Count()
                    });
            foreach (var row in query)
                departmentDetails.Add((row.DepartmentId ?? 0, row.Name, row.EmployeeCount));
            Console.WriteLine("Fetched Departments ids and names with employees count using projection.");
            return departmentDetails;
        }



        #endregion

        #region Filter child list in the DTOs
        public List<ParentDto> FilterChildrenByMinAge(List<ParentDto> parents, int minAge)
        {
            foreach (var parent in parents)
            {
                parent.Children = parent.Children.Where(child => child.Age >= minAge).ToList();
            }
            return parents;
        }
        public List<ParentDto> FilterChildrenByNameStartingWith(List<ParentDto> parents, char startingLetter)
        {
            foreach (var parent in parents)
            {
                parent.Children = parent.Children.Where(child => child.Name.StartsWith(startingLetter)).ToList();
            }
            return parents;
        }
        public List<ParentDto> FilterChildrenByAgeAndNameLength(List<ParentDto> parents, int minAge, int maxAge, int minNameLength)
        {
            foreach (var parent in parents)
            {
                parent.Children = parent.Children
                    .Where(child => child.Age >= minAge && child.Age <= maxAge && child.Name.Length > minNameLength)
                    .ToList();
            }
            return parents;
        }
        public Dictionary<string, List<ChildDto>> GroupChildrenByAgeRange(List<ParentDto> parents)
        {
            var groupedChildren = new Dictionary<string, List<ChildDto>>();

            foreach (var parent in parents)
            {
                foreach (var child in parent.Children)
                {
                    string ageRange = GetAgeRange(child.Age);
                    if (!groupedChildren.ContainsKey(ageRange))
                    {
                        groupedChildren[ageRange] = new List<ChildDto>();
                    }
                    groupedChildren[ageRange].Add(child);
                }
            }
            return groupedChildren;
        }

        private static string GetAgeRange(int age)
        {
            if (age >= 10 && age <= 15) return "10 - 15";
            if (age >= 16 && age <= 20) return "16 - 20";
            if (age >= 21 && age <= 40) return "21 - 40";
            return "Other";
        }
        public List<ParentDto> GetParentsHasNumberOfChilds(int minChilds, List<ParentDto> parents)
        {
            return parents
           .Where(parent => parent.Children.Count >= minChilds)
           .ToList();
        }
        public Dictionary<char, List<ChildDto>> GroupChildrenByGender(List<ParentDto> parents)
        {
            return parents
                .SelectMany(parent => parent.Children)
                .GroupBy(child => child.Gender)
                .ToDictionary(group => group.Key, group => group.ToList());
        }

        #endregion

        #region Prefetch
        public void FetchDepartmentWithEmployees(int departmentId)
        {
            using var _adapter = _serviceProvider.GetService<DataAccessAdapter>();
            var department = new DepartmentEntity(departmentId);

            var prefetchPath = new PrefetchPath2(EntityType.DepartmentEntity)
            {
                DepartmentEntity.PrefetchPathEmployees
            };

            if (_adapter!.FetchEntity(department, prefetchPath))
            {
                Console.WriteLine($"Department: {department.Name}");
                foreach (var employee in department.Employees)
                {
                    Console.WriteLine($"  Employee: {employee.Name}, Salary: {employee.Salary}");
                }
            }
            else
            {
                Console.WriteLine("Department not found.");
            }
        }

        public void FetchDepartmentsWithEmployeesFilteredByMinSalary(decimal minSalary)
        {
            using var _adapter = _serviceProvider.GetService<DataAccessAdapter>();
            var bucket = new RelationPredicateBucket();

            var prefetchPath = new PrefetchPath2(EntityType.DepartmentEntity);
            var employeePrefetch = prefetchPath.Add(DepartmentEntity.PrefetchPathEmployees);
            employeePrefetch.Filter.Add(EmployeeFields.Salary.SetObjectAlias("Employee").GreaterThan(minSalary));

            var departments = new EntityCollection<DepartmentEntity>();
            _adapter!.FetchEntityCollection(departments, bucket, prefetchPath);

            foreach (var department in departments)
            {
                Console.WriteLine($"Department: {department.Name}");
                foreach (var employee in department.Employees)
                {
                    Console.WriteLine($"Employee: {employee.Name}, Salary: {employee.Salary:C}");
                }
            }
        }
        #endregion

        #region Advanced Examples
        public void TryAllOperations(System.Data.IsolationLevel isolationLevel)
        {
            using var _adapter = _serviceProvider.GetService<DataAccessAdapter>();
            _adapter!.StartTransaction(isolationLevel, "DepartmentTransaction");
            try
            {
                var newDepartment = new DepartmentEntity { Name = "LR" };
                _adapter.SaveEntity(newDepartment);

                var employees = new EntityCollection<EmployeeEntity>
            {
                new EmployeeEntity { Name = "Marina", Salary = 1000, DepartmentId = newDepartment.DepartmentId },
                new EmployeeEntity { Name = "Mahmoud", Salary = 7000, DepartmentId = newDepartment.DepartmentId }
            };
                _adapter.SaveEntityCollection(employees,true,false);

                newDepartment.Name = "LR & DevOps";
                _adapter.SaveEntity(newDepartment,true,false);

                foreach (var employee in employees)
                {
                    employee.Salary += 1000;
                }
                _adapter.SaveEntityCollection(employees);

                var prefetchPath = new PrefetchPath2(EntityType.DepartmentEntity)
            {
                DepartmentEntity.PrefetchPathEmployees
            };
                var departments = new EntityCollection<DepartmentEntity>();
                _adapter.FetchEntityCollection(departments, null, prefetchPath);

                var resultDto = departments.Select(dept => new DepartmentDto
                {
                    DepartmentId = dept.DepartmentId,
                    Name = dept.Name,
                    Employees = dept.Employees
                        .Where(emp => emp.Salary > 2000)
                        .Select(emp => new EmployeeDto
                        {
                            EmployeeId = emp.EmployeeId,
                            Name = emp.Name,
                            Salary = emp.Salary
                        })
                        .ToList()
                }).ToList();
                foreach(var department in resultDto)
                {
                    Console.WriteLine($"Department : {department.Name}");
                    foreach(var employee in department.Employees)
                        Console.WriteLine($"Employee ==> {employee.Name} , {employee.Salary}");
                }
                var deleteBucket = new RelationPredicateBucket(EmployeeFields.Salary.LesserThan(2000));
                _adapter.DeleteEntitiesDirectly(typeof(EmployeeEntity), deleteBucket);

                _adapter.Commit();
            }
            catch(Exception ex) 
            {
                Console.WriteLine($"{ex.Message}");
                _adapter.Rollback();
            }
        }

       //Have an exception should be edited
        public void ExecuteEmployeeManagementOperations(decimal minSalary, System.Data.IsolationLevel isolationLevel)
        {
            using var _adapter = _serviceProvider.GetService<DataAccessAdapter>();

            _adapter!.StartTransaction(isolationLevel, "EmployeeManagementTransaction");

            try
            {
                var newDepartment = new DepartmentEntity
                {
                    Name = "Sales"
                };
                _adapter.SaveEntity(newDepartment, true, false);

                var employees = new EntityCollection<EmployeeEntity>
        {
            new EmployeeEntity { Name = "Amira", Salary = 5000, DepartmentId = newDepartment.DepartmentId },
            new EmployeeEntity { Name = "Amar", Salary = 55000, DepartmentId = newDepartment.DepartmentId },
            new EmployeeEntity { Name = "Amgad", Salary = 45000, DepartmentId = newDepartment.DepartmentId }
        };
                _adapter.SaveEntityCollection(employees, true, false);

                var softDeleteBucket = new RelationPredicateBucket(EmployeeFields.Salary < 50000);
                var employeesToDelete = new EntityCollection<EmployeeEntity>();  
                _adapter.FetchEntityCollection(employeesToDelete, softDeleteBucket);
                foreach (var employee in employeesToDelete)
                {
                    _adapter.DeleteEntity(employee);
                    _adapter.SaveEntity(employee);
                }

                var prefetchPath = new PrefetchPath2(EntityType.DepartmentEntity)
        {
            DepartmentEntity.PrefetchPathEmployees
        };

                var departmentBucket = new RelationPredicateBucket();
                var departments = new EntityCollection<DepartmentEntity>();
                _adapter.FetchEntityCollection(departments, departmentBucket, prefetchPath);

                var departmentDtos = departments.Select(department => new DepartmentDto
                {
                    Name = department.Name,
                    Employees = department.Employees
                        .Where(emp => emp.Salary >= minSalary) 
                        .Select(emp => new EmployeeDto
                        {
                            Name = emp.Name,
                            Salary = emp.Salary
                        })
                        .ToList()
                }).ToList();

                var bulkUpdateBucket = new RelationPredicateBucket(DepartmentFields.Name == "Sales");
                /*
                 * var employeesToDelete = new EntityCollection<EmployeeEntity>();  
                _adapter.FetchEntityCollection(employeesToDelete, softDeleteBucket);
                 */
                var employeesInSales = new EntityCollection<EmployeeEntity>();
                _adapter.FetchEntityCollection(employeesInSales, bulkUpdateBucket);
                foreach (var employee in employeesInSales)
                {
                    employee.Salary += 500; 
                }
                _adapter.SaveEntityCollection(employeesInSales, true, false);

                var deleteBucket = new RelationPredicateBucket(EmployeeFields.Name == "Amira");
                _adapter.DeleteEntitiesDirectly(typeof(EmployeeEntity), deleteBucket);

                _adapter.Commit();
            }
            catch (Exception ex)
            {
                _adapter.Rollback();
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
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



