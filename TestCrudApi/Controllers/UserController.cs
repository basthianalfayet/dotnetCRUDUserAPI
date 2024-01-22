using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Security.AccessControl;

namespace TestCrudApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IConfiguration _configuration;

        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("getDataUser")]

        public JsonResult getDataUser(string userId = "all")
        {
            string query = "SELECT * FROM tbl_user";

            // Check if userId is specified and not equal to "all"
            if (!string.IsNullOrEmpty(userId) && userId.ToLower() != "all")
            {
                // Append WHERE clause to the query to filter by userId
                query += $" WHERE UserId = '{userId}'";
            }

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("UserAppCon");
            SqlDataReader myReader;

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();

                using (SqlCommand mycommand = new SqlCommand(query, myCon))
                {
                    myReader = mycommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                }

                myCon.Close();
            }

            return new JsonResult(table);
        }


        [HttpPost]
        [Route("setDataUser")]
        public JsonResult setDataUser([FromForm] string namalengkap, 
            [FromForm] string username, [FromForm] string password, [FromForm] string status)
        {
            string query = "insert into tbl_user values(@namalengkap,@username,@password,@status)";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("UserAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand mycommand = new SqlCommand(query, myCon))
                {
                    mycommand.Parameters.AddWithValue("@namalengkap", namalengkap);
                    mycommand.Parameters.AddWithValue("@username", username);
                    mycommand.Parameters.AddWithValue("@password", password);
                    mycommand.Parameters.AddWithValue("@status", status);
                    myReader = mycommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(table);
        }

        [HttpPost]
        [Route("delDataUser")]
        public JsonResult delDataUser([FromForm] string id)
        {
            string query = "delete from tbl_user where userid=@id";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("UserAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand mycommand = new SqlCommand(query, myCon))
                {
                    mycommand.Parameters.AddWithValue("@id", id);
                    myReader = mycommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(table);
        }
    }
}

