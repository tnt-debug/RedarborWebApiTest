using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using RedarborWebApiTest.Controllers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RedarborWebApiTest.Data;
using RedarborWebApiTest.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmployeesTests.UnitTests
{
    [TestClass]
    public class EmployeesTests : TestBase
    {      
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        public EmployeesTests()
        {
           _context = CreateContext();
           _config = InitConfig();
        }

        [TestMethod]    
        ///<summary>
        ///Check if the EmployeesController.GetAllEmployees return the correct number of employees in the DB.
        ///</summary>   
        public async Task GetAllEmployees()
        {
            var sql = "select count(Id) Id from Employees";
            int numberOfEmployeesInDb;
            using (var db = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await db.OpenAsync();
                numberOfEmployeesInDb = await db.QueryFirstAsync<int>(sql);              
            }           
            EmployeesController employeesController = new EmployeesController(_context, _config);
            var employeesList = await employeesController.GetAllEmployees();            
            var employeesCount = employeesList.Value;
            Assert.AreEqual(numberOfEmployeesInDb, employeesCount.Count);
        }

        [TestMethod]
        ///<summary>
        /// Check if the method CreateNewEmployee from EmployeesController returns 200 ( Created ).
        ///</summary>
        public async Task CreateNewEmployee()
        {
            EmployeesController employeesController = new EmployeesController(_context, _config);
            ActionResult result = await employeesController.CreateNewEmployee(new Employee
            {
                CompanyId = 0,
                CreatedOn = DateTime.Now.ToString(("yyyy-MM-dd HH:mm:ss")),
                DeletedOn = null,
                Email = "johndoe@test.com",
                Fax = null,
                Name = "John Doe",
                LastLogin = DateTime.Now.ToString(("yyyy-MM-dd HH:mm:ss")),
                Password  = "password",
                PortalId = 0,
                RoleId = 0,
                StatusId = 0,
                Telephone = null,
                UpdatedOn = DateTime.Now.ToString(("yyyy-MM-dd HH:mm:ss")),
                Username = "Johndoe123"
            });
            OkObjectResult? okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);            
        }

        [TestMethod]
        [DataRow(1)]
        ///<summary>
        /// Check if the user id are retrieved from the method GetEmployeeById of EmployeesController.
        ///</summary>
        public async Task GetEmployeeById(int id)
        {
            EmployeesController employeesController = new EmployeesController(_context, _config);
            ActionResult<Employee> result = await employeesController.GetEmployeeById(id);            
            Assert.IsNotNull(result);
            Assert.AreEqual(id, result.Value.Id);
        }

        [TestMethod]
        ///<summary>
        /// Try to insert null values on the Employees database.
        ///</summary>
        public async Task CheckIfNullsAreNotAllowed()
        {
            EmployeesController employeesController = new EmployeesController(_context, _config);
            ActionResult result = await employeesController.CreateNewEmployee(new Employee
            {
                CompanyId = 0,
                CreatedOn = DateTime.Now.ToString(("yyyy-MM-dd HH:mm:ss")),
                DeletedOn = null,
                Email = null,
                Fax = null,
                Name = "John Doe",
                LastLogin = DateTime.Now.ToString(("yyyy-MM-dd HH:mm:ss")),
                Password = null,
                PortalId = 0,
                RoleId = 0,
                StatusId = 0,
                Telephone = null,
                UpdatedOn = DateTime.Now.ToString(("yyyy-MM-dd HH:mm:ss")),
                Username = null
            });
            BadRequestObjectResult? okResult = result as BadRequestObjectResult;
            Assert.AreEqual(400, okResult.StatusCode);
        } 
    }
}
