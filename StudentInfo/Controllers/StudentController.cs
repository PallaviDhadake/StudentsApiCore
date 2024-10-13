using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using StudentInfo.Models;
using System.Data;

namespace StudentInfo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        iClass c;

        public StudentController(IConfiguration configuration)
        {
            _configuration = configuration;
            c = new iClass(_configuration);
        }


        //Post Method
        [HttpPost]
        [Route("AddNew")]
        public bool CreateNews(int id, string fname, string lname, string email, string password)
        {

            using (SqlConnection conn = new SqlConnection(c.GetConnectionString()))
            {
                conn.Open();
                SqlCommand cmdInsertNews = new SqlCommand("InserDetails", conn);
                cmdInsertNews.CommandType = CommandType.StoredProcedure;
                //Add Parameters
                cmdInsertNews.Parameters.AddWithValue("@Id", id);
                cmdInsertNews.Parameters.AddWithValue("@FirstName", fname);
                cmdInsertNews.Parameters.AddWithValue("@LastName", lname);
                cmdInsertNews.Parameters.AddWithValue("@Email", email);
                cmdInsertNews.Parameters.AddWithValue("@Password", password);
                cmdInsertNews.ExecuteNonQuery();

            }
            return true;
        }


        //Get Method
        [HttpGet]
        [Route("GetAll")]
        public IEnumerable<StudentsData> GetsDataClass()
        {
            List<StudentsData> studlist = new List<StudentsData>();
            using (DataTable dtnews = c.GetDataTable("Select * from Registrations"))
            {
                foreach (DataRow row in dtnews.Rows)
                {
                    StudentsData news = new StudentsData
                    {
                        Id = (int)row["Id"],
                        FName = row["FirstName"].ToString(),
                        LName = row["LastName"].ToString(),
                        Email = row["Email"].ToString(),
                        password = row["Password"].ToString()

                    };

                    studlist.Add(news);
                }
            }

            return studlist;
        }

        //Get with parameter
        [HttpGet]
        [Route("GetAllById/{id}")]
        public IEnumerable<StudentsData> GetsDataClasswithparameter(int id)
        {
            List<StudentsData> studlist = new List<StudentsData>();
            //using (DataTable dtnews = c.GetDataTable("Select * from Registrations where Id=")
                using (DataTable dtstuds = c.GetDataTableWithParameter("SELECT * FROM Registrations WHERE Id = @Id", new SqlParameter("@Id", id)))
            {
                foreach (DataRow row in dtstuds.Rows)
                {
                    StudentsData students = new StudentsData
                    {
                        Id = (int)row["Id"],
                        FName = row["FirstName"].ToString(),
                        LName = row["LastName"].ToString(),
                        Email = row["Email"].ToString(),
                        password = row["Password"].ToString()

                    };

                    studlist.Add(students);
                }
            }

            return studlist;
        }

        //Put Method
        [HttpPut]
        [Route("Update")]
        public bool UpdateNews(int id, string fName, string Lname, string email, string password)
        {

            using (SqlConnection conn = new SqlConnection(c.GetConnectionString()))
            {
                conn.Open();
                SqlCommand cmdUpdatenews = new SqlCommand("spUpdate", conn);
                cmdUpdatenews.CommandType = CommandType.StoredProcedure;

                //Add Parameters
                cmdUpdatenews.Parameters.AddWithValue("@Id", id);
                cmdUpdatenews.Parameters.AddWithValue("@FirstName", fName);
                cmdUpdatenews.Parameters.AddWithValue("@LastName", Lname);
                cmdUpdatenews.Parameters.AddWithValue("@Email", email);
                cmdUpdatenews.Parameters.AddWithValue("@Password", password);

                // Define the output parameter
                SqlParameter messageParam = new SqlParameter("@Message", SqlDbType.NVarChar, 100)
                {
                    Direction = ParameterDirection.Output
                };
                cmdUpdatenews.Parameters.Add(messageParam);

                cmdUpdatenews.ExecuteNonQuery();
            }
            return true;
        }

        //Method Delete
        [HttpDelete]
        [Route("Delete")]
        public bool DeleteNews(int Id)
        {
            using (SqlConnection conn = new SqlConnection(c.GetConnectionString()))
            {
                conn.Open();

                // Delete from Students table using query
                string deleteStudentQuery = "DELETE FROM Registrations WHERE Id=@Id";
                SqlCommand cmdDeleteNews = new SqlCommand(deleteStudentQuery, conn);
                cmdDeleteNews.Parameters.AddWithValue("@Id", Id);
                cmdDeleteNews.ExecuteNonQuery();
            }

            return true;
        }

    }
}
