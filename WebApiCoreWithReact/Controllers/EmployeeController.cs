using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using WebApiCoreWithReact.Models;

namespace WebApiCoreWithReact.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public EmployeeController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }
        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                        select EmployeeID,EmployeeName,Department,DateOfJoining,PhotoFileName from dbo.Employee
                        ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");

            SqlDataReader myReader;
            using (SqlConnection mycon = new SqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (SqlCommand command = new SqlCommand(query, mycon))
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
        public JsonResult Post(Employee emp)
        {
            string query = @"
                        insert into dbo.Employee (EmployeeName,Department,DateOfJoining,PhotoFileName) 
                        values
                        (@EmployeeName,@Department,@DateOfJoining,@PhotoFileName)
                        ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");

            SqlDataReader myReader;
            using (SqlConnection mycon = new SqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (SqlCommand command = new SqlCommand(query, mycon))
                {
                    command.Parameters.AddWithValue("@EmployeeName", emp.EmployeeName);
                    command.Parameters.AddWithValue("@Department", emp.Department);
                    command.Parameters.AddWithValue("@DateOfJoining", emp.DateOfJoining);
                    command.Parameters.AddWithValue("@PhotoFileName", emp.PhotoFileName);
                    myReader = command.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    mycon.Close();
                }
                return new JsonResult("Added Successfully.");
            }
        }

        [HttpPut]
        public JsonResult Put(Employee emp)
        {
            string query = @"
                        update  dbo.Employee
                        set EmployeeName=@EmployeeName,Department=@Department,DateOfJoining=@DateOfJoining,PhotoFileName=@PhotoFileName
                        where EmployeeID=@EmployeeID
                        ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");

            SqlDataReader myReader;
            using (SqlConnection mycon = new SqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (SqlCommand command = new SqlCommand(query, mycon))
                {
                    command.Parameters.AddWithValue("@EmployeeID", emp.EmployeeID);
                    command.Parameters.AddWithValue("@EmployeeName", emp.EmployeeName);
                    command.Parameters.AddWithValue("@Department", emp.Department);
                    command.Parameters.AddWithValue("@DateOfJoining", emp.DateOfJoining);
                    command.Parameters.AddWithValue("@PhotoFileName", emp.PhotoFileName);
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
                        delete from dbo.Employee
                        where EmployeeID=@EmployeeID
                        ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");

            SqlDataReader myReader;
            using (SqlConnection mycon = new SqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (SqlCommand command = new SqlCommand(query, mycon))
                {
                    command.Parameters.AddWithValue("@EmployeeID", id);
                    myReader = command.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    mycon.Close();
                }
                return new JsonResult("Deleted Successfully.");
            }
        }
        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest= Request.Form;
                var postedFile =httpRequest.Files[0];
                var fileName = postedFile.FileName;
                var physicalPath = _env.ContentRootPath + "/Photos/" + fileName;

                using (var stream = new FileStream(physicalPath,FileMode.Create) )
                {
                    postedFile.CopyTo(stream);
                }
                return new JsonResult(fileName);

            }
            catch (Exception)
            {
                return new JsonResult("anonyous.png");
            }
        }
    }
}
