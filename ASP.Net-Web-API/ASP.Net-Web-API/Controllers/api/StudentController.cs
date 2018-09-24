using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ASP.Net_Web_API.Classes;
using ASP.Net_Web_API.Models;



//This is API web constroller

namespace ASP.Net_Web_API.Controllers.api
{
    public class StudentController : ApiController
    {

        //Gets all the students. Method using IHttpActionResult.


        public IHttpActionResult GetAllStudents(bool includeAddress = false)
        {
            IList<StudentViewModel> Students = null;

            using (var ctx = new SchoolEntities1())
            {
                Students = ctx.Students.Include("StudentAddress").Include("Standard")
                    .Select(s => new StudentViewModel()
                    {
                        Id = s.StudentID,
                        StudentName = s.StudentName,
                        stardardId = s.StandardId ?? default(int),
                        Standard = s.StandardId == null? null : new StandardViewModel()
                        {
                            StandardName = s.Standard.StandardName,
                            Description = s.Standard.Description,
                            

                        },
                        Address = s.StudentAddress == null || includeAddress == false ? null : new AddressViewModel()
                        {
                            Id = s.StudentID,
                            Address1 = s.StudentAddress.Address1,
                            Address2 = s.StudentAddress.Address2,
                            City = s.StudentAddress.City,
                            State = s.StudentAddress.State,

                        }

                    }).ToList<StudentViewModel>();
            }

            if (Students.Count == 0)
            {
                return NotFound();
            }
            return Ok(Students);

        }


        //Get student by Id. Using IHttpActionResult

        public IHttpActionResult GetSudentById(int id)
        {
            StudentViewModel Student = null;
            using(var ctx = new SchoolEntities1())
            {
                Student = ctx.Students.Include("StudentAddress").Include("Standard")
                    .Where(s => s.StudentID == id)
                    .Select(s => new StudentViewModel()
                    {
                        Id = s.StudentID,
                        StudentName = s.StudentName,
                        Standard = s.StandardId == null? null : new StandardViewModel()
                        {
                            StandardId = s.Standard.StandardId,
                            StandardName = s.Standard.StandardName,
                            Description = s.Standard.Description,
                        },
                        Address = s.StudentAddress == null ? null : new AddressViewModel()
                        {
                            Id = s.StudentID,
                            Address1 = s.StudentAddress.Address1,
                            Address2 = s.StudentAddress.Address2,
                            City = s.StudentAddress.City,
                            State = s.StudentAddress.State,
                        }
                    }).FirstOrDefault<StudentViewModel>();

            }

            if(Student == null)
            {
                return NotFound();
            }
            return Ok(Student);
        }



        //Gets Students with specified name. using IHttpActionResult

        public IHttpActionResult GetStudentByName(String name)
        {
            IList<StudentViewModel> Students = null;
            using (var ctx = new SchoolEntities1())
            {
                Students = ctx.Students.Include("StudentAddress").Include("Standard")
                    .Where(s => s.StudentName == name)
                    .Select(s => new StudentViewModel()
                    {
                        Id = s.StudentID,
                        StudentName = s.StudentName,
                        Standard = s.StandardId == null? null: new StandardViewModel()
                        {
                            StandardId = s.Standard.StandardId,
                            StandardName = s.Standard.StandardName,
                            Description = s.Standard.Description,
                        },
                        Address = s.StudentAddress == null ? null : new AddressViewModel()
                        {
                            Id = s.StudentID,
                            Address1 = s.StudentAddress.Address1,
                            Address2 = s.StudentAddress.Address2,
                            City = s.StudentAddress.City,
                            State = s.StudentAddress.State,
                        }
                    }).ToList<StudentViewModel>();

            }

            if(Students.Count == 0)
            {
                return NotFound();
            }

            return Ok(Students);

          
        }

        //Gets all students in same standard.

        public IHttpActionResult GetStudentsInSameStandard(int standardId)
        {
            IList<StudentViewModel> Students = null;

            using (var ctx = new SchoolEntities1())
            {
                Students = ctx.Students.Include("StudentAddress")
                    .Include("Standard")
                    .Where(s => s.StandardId == standardId)
                    .Select(s => new StudentViewModel()
                    {
                        Id = s.StudentID,
                        StudentName = s.StudentName,
                        Standard = s.StandardId == null? null : new StandardViewModel()
                        {
                            StandardId = s.Standard.StandardId,
                            StandardName = s.Standard.StandardName,
                            Description = s.Standard.Description,
                        },
                        Address = s.StudentAddress == null ? null : new AddressViewModel()
                        {
                            Address1 = s.StudentAddress.Address1,
                            Address2 = s.StudentAddress.Address2,
                            City = s.StudentAddress.City,
                            State = s.StudentAddress.State,
                        }
                    }).ToList<StudentViewModel>();
            }

            if(Students.Count == 0)
            {
                return NotFound();
            }
            return Ok(Students);
        }


        //Posts new student in the datbase

        public IHttpActionResult PostNewStudent(StudentStdAddress studentAddress)
        {

            //This will make sure that the student object includes all the necessary information. If it is not valid, it will return BadRequest response.
            if(!ModelState.IsValid)
            {
                return BadRequest("Invalid data");
            }

            using(var ctx = new SchoolEntities1())
            {
                ctx.Students.Add(new Student()
                {
                    StudentID = studentAddress.Student.Id,
                    StudentName = studentAddress.Student.StudentName,
                    StandardId = studentAddress.Student.stardardId,
                });

                ctx.StudentAddresses.Add(new StudentAddress()
                {
                    StudentID = studentAddress.Student.Id,
                    Address1 = studentAddress.Address.Address1,
                    Address2 = studentAddress.Address.Address2,
                    City = studentAddress.Address.City,
                    State = studentAddress.Address.State,
                });
                ctx.SaveChanges();
            }

            return Ok();


        }



        //Updates the existing data in the database

        public IHttpActionResult PutStudent(StudentStdAddress studentAddress)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data");

            }

            using(var ctx = new SchoolEntities1())
            {
                var existingStudent = ctx.Students
                    .Where(s => s.StudentID == studentAddress.Student.Id)
                    .FirstOrDefault();
                
                if(existingStudent != null)
                {
                    existingStudent.StudentName = studentAddress.Student.StudentName;
                    existingStudent.StandardId = studentAddress.Student.stardardId;
                    existingStudent.StudentAddress.Address1 = studentAddress.Address.Address1;
                    existingStudent.StudentAddress.Address2 = studentAddress.Address.Address2;
                    existingStudent.StudentAddress.City = studentAddress.Address.City;
                    existingStudent.StudentAddress.State = studentAddress.Address.State;

                    ctx.SaveChanges();
                }

                else
                {
                    NotFound();
                }

            }

            return Ok();
        }


        //Deletes student data from database

        public IHttpActionResult Delete(int id)
        {
            if(id <= 0)
            {
                return BadRequest("Not a valid student Id");

            }

            using(var ctx = new SchoolEntities1())
            {
                var existingStudent = ctx.Students
                    .Where(s => s.StudentID == id)
                    .FirstOrDefault();
                if(existingStudent != null)
                {
                    ctx.Students.Remove(existingStudent);
                    ctx.SaveChanges();
                }
                else
                {
                    return NotFound();
                }

            }

            return Ok();
        }

    
    }
}
