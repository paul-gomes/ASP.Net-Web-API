using System;
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

            using (var ctx = new SchoolEntities())
            {
                Students = ctx.Students.Include("StudentAddress")
                    .Select(s => new StudentViewModel()
                    {
                        Id = s.StudentID,
                        StudentName = s.StudentName,
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
            using(var ctx = new SchoolEntities())
            {
                Student = ctx.Students.Include("StudentAddress")
                    .Where(s => s.StudentID == id)
                    .Select(s => new StudentViewModel()
                    {
                        Id = s.StudentID,
                        StudentName = s.StudentName,
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
            using (var ctx = new SchoolEntities())
            {
                Students = ctx.Students.Include("StudentAddress")
                    .Where(s => s.StudentName == name)
                    .Select(s => new StudentViewModel()
                    {
                        Id = s.StudentID,
                        StudentName = s.StudentName,
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

            using (var ctx = new SchoolEntities())
            {
                Students = ctx.Students.Include("StudentAddress")
                    .Include("StudentStandard")
                    .Where(s => s.StandardId == standardId)
                    .Select(s => new StudentViewModel()
                    {
                        Id = s.StudentID,
                        StudentName = s.StudentName,
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

    
    }
}
