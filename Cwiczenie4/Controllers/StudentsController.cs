using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Cwiczenie4.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cwiczenie4.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        private const string ConString = "Data Source=db-mssql;Initial Catalog=s5554;Integrated Security=True";

        [HttpGet]
        public IActionResult GetStudents(string orderBy = "Nazwisko")
        {
            var list = new List<Student>();
            using (SqlConnection con = new SqlConnection(ConString))
            using (SqlCommand com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "select indexnumber, firstname, lastname, birthdate, semester, name from Student inner join Enrollment e on student.idenrollment = e.idenrollment inner join studies s on e.idstudy = s.idstudy";

                con.Open();
                SqlDataReader dr = com.ExecuteReader();
                while (dr.Read())
                {
                    var st = new Student();
                    st.IndexNumber = dr["IndexNumber"].ToString();
                    st.FirstName = dr["FirstName"].ToString();
                    st.LastName = dr["LastName"].ToString();
                    st.BirthDate = dr["BirthDate"].ToString();
                    st.Semester = dr["Semester"].ToString();
                    st.StudiesName = dr["Name"].ToString();

                    list.Add(st);
                }
                con.Close();
            }

            return Ok(list);
        }

        [HttpGet("{id}")]
        public IActionResult GetStudent(string id)
        {
            var list = new List<Student>();
            using (SqlConnection con = new SqlConnection(ConString))
            using (SqlCommand com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "select indexnumber, firstname, lastname, birthdate, semester, name from Student inner join Enrollment e on student.idenrollment = e.idenrollment inner join studies s on e.idstudy = s.idstudy where indexnumber = " + @id;
                com.Parameters.AddWithValue("id", id);

                con.Open();
                SqlDataReader dr = com.ExecuteReader();
                if(dr.Read())
                {
                    var st = new Student();
                    st.IndexNumber = dr["IndexNumber"].ToString();
                    st.FirstName = dr["FirstName"].ToString();
                    st.LastName = dr["LastName"].ToString();
                    st.BirthDate = dr["BirthDate"].ToString();
                    st.Semester = dr["Semester"].ToString();
                    st.StudiesName = dr["Name"].ToString();

                    return Ok(st);
                } else
                {
                    return NotFound("Brak studenta o podanym id.");
                }

            }


        }
    }
}