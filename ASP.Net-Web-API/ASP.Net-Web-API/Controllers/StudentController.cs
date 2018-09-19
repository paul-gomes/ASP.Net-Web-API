﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ASP.Net_Web_API.Models;

namespace ASP.Net_Web_API.Controllers
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
                        Standard = s.StandardId == null? null : new StandardViewModel()
                        {
                            StandardId = s.Standard.StandardId,
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

        //Gets all the students. Not using IHttpActionResult.

        //public IList<StudentViewModel> GetAllStudents(bool includeAddress = false)
        //{
        //    using (var ctx = new SchoolEntities())
        //    {
        //        IList<StudentViewModel> Students = ctx.Students.Include("StudentAddress")
        //            .Select(s => new StudentViewModel()
        //            {
        //                Id = s.StudentID,
        //                StudentName = s.StudentName,
        //                Address = s.StudentAddress == null || includeAddress == false ? null : new AddressViewModel()
        //                {
        //                    Id = s.StudentID,
        //                    Address1 = s.StudentAddress.Address1,
        //                    Address2 = s.StudentAddress.Address2,
        //                    City = s.StudentAddress.City,
        //                    State = s.StudentAddress.State

        //                }
        //            }).ToList<StudentViewModel>();
        //        return Students;
        //    }
        //}



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
        
        public IHttpActionResult PostNewStudent(StudentViewModel student, AddressViewModel studentAddress)
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
                    StudentID = student.Id,
                    StudentName = student.StudentName,
                    StandardId = student.stardardId,
                });

                ctx.StudentAddresses.Add(new StudentAddress()
                {
                    StudentID = student.Id,
                    Address1 = studentAddress.Address1,
                    Address2 = studentAddress.Address2,
                    City = studentAddress.City,
                    State = studentAddress.State,
                });
                ctx.SaveChanges();
            }

            return Ok();


        }



        //Updates the existing data in the database

        public IHttpActionResult PutStudent(StudentViewModel student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data");

            }

            using(var ctx = new SchoolEntities1())
            {
                var existingStudent = ctx.Students.Where(s => s.StudentID == student.Id).FirstOrDefault();
                
                if(existingStudent != null)
                {
                    existingStudent.StudentName = student.StudentName;
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
                var student = ctx.Students
                    .Where(s => s.StudentID == id)
                    .FirstOrDefault();
                if(student != null)
                {
                    ctx.Students.Remove(student);
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
