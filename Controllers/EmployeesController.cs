using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RedarborWebApiTest.Data;
using RedarborWebApiTest.Models;

namespace RedarborWebApiTest.Controllers
{
    [Route("api/redarbor")]
    [ApiController]
    [Produces("application/json")]
    public class EmployeesController : ControllerBase
    {
        private readonly AppDbContext _context;
        public IConfiguration Configuration { get; }

        public EmployeesController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }

        [HttpGet]
        public async Task<ActionResult<List<Employee>>> GetAllEmployees()
        {
            string sql = "select * from Employees";
            List<Employee> listOfEmployees;
            using (SqlConnection db = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                await db.OpenAsync();
                listOfEmployees = (await db.QueryAsync<Employee>(sql)).ToList();
                await db.CloseAsync();
            }          
            return listOfEmployees;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Employee>> GetEmployeeById(int id)
        {
            try
            {
                string sql = "select * from Employees where Id = @id";
                ActionResult<Employee> employeeById;
                using (SqlConnection db = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
                {
                    await db.OpenAsync();
                    Employee employee = await db.QuerySingleOrDefaultAsync<Employee>(sql, new
                    {
                        Id = id
                    });
                    await db.CloseAsync();
                    if (employee == null)
                    {
                        return NotFound("Any employee found with ID: " + id.ToString());
                    }
                    employeeById = employee;
                }
                return employeeById;
            }
            catch(Exception e)
            {
                return StatusCode(500, e.Message);
            }             
           
        }

        [HttpPost]
        public async Task<ActionResult> CreateNewEmployee(Employee employee)
        {
            try
            {
                await _context.AddAsync(employee);
                await _context.SaveChangesAsync();
                return Ok(employee);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateEmployee(Employee employee, int id)
        {  
            try
            {
                string sql = "select * from Employees where Id = @id";
                using (SqlConnection db = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
                {
                    await db.OpenAsync();
                    Employee currentEmployee = await db.QuerySingleAsync<Employee>(sql, new
                    {
                        Id = id
                    });
                    if (currentEmployee == null)
                    {
                        return NotFound("Any employee found");
                    }                       
                    employee.Id = currentEmployee.Id;
                    employee.UpdatedOn = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                    await db.CloseAsync();
                    return Ok();
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteEmployee(int id)
        {
            try
            {
                string sql = "select * from Employees where Id = @id";
                using (SqlConnection db = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
                {
                    await db.OpenAsync();
                    Employee employee = await db.QuerySingleOrDefaultAsync<Employee>(sql, new
                    {
                        Id = id
                    });
                    if (employee == null)
                    {
                        await db.CloseAsync();                        
                        return NotFound("Any employee found");
                    }
                    _context.Remove(employee);
                    await _context.SaveChangesAsync();
                    await db.CloseAsync();
                    return Ok();
                }             
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
