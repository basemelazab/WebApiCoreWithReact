using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using WebApiCoreWithReact.Models;

namespace WebApiCoreWithReact.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public DepartmentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                        select DepartmentID,DepartmentName from dbo.Department
                        ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");

            SqlDataReader myReader;
            using (SqlConnection mycon= new SqlConnection(sqlDataSource))
            {
                mycon.Open();
                using(SqlCommand command= new SqlCommand(query,mycon))
                {
                    myReader = command.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    mycon.Close();
                }
                return new JsonResult(table);
            }
        }

        [HttpPost]
        public JsonResult Post(Department dep)
        {
            string query = @"
                        insert into dbo.Department values (@DepartmentName)
                        ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");

            SqlDataReader myReader;
            using (SqlConnection mycon = new SqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (SqlCommand command = new SqlCommand(query, mycon))
                {
                    command.Parameters.AddWithValue("@DepartmentName", dep.DepartmentName);
                    myReader = command.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    mycon.Close();
                }
                return new JsonResult("Added Successfully.");
            }
        }

        [HttpPut]
        public JsonResult Put(Department dep)
        {
            string query = @"
                        update  dbo.Department
                        set DepartmentName=@DepartmentName
                        where DepartmentID=@DepartmentID
                        ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");

            SqlDataReader myReader;
            using (SqlConnection mycon = new SqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (SqlCommand command = new SqlCommand(query, mycon))
                {
                    command.Parameters.AddWithValue("@DepartmentID", dep.DepartmentID);
                    command.Parameters.AddWithValue("@DepartmentName", dep.DepartmentName);
                    myReader = command.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    mycon.Close();
                }
                return new JsonResult("Updated Successfully.");
            }
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"
                        delete from dbo.Department
                        where DepartmentID=@DepartmentID
                        ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");

            SqlDataReader myReader;
            using (SqlConnection mycon = new SqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (SqlCommand command = new SqlCommand(query, mycon))
                {
                    command.Parameters.AddWithValue("@DepartmentID",id);
                    myReader = command.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    mycon.Close();
                }
                return new JsonResult("Deleted Successfully.");
            }
        }
    }
}
