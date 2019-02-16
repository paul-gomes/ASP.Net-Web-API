using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


//This is teacher API controller

namespace ASP.Net_Web_API.Controllers.api
{
    public class TeacherController : ApiController
    {

        //Gets all the teachers 

        public IHttpActionResult GetAllTeachers()
        {
            IList<Teacher> teachers = null;

            using(var ctx = new SchoolEntities1())
            {
                teachers = ctx.Teachers.Select(t => t.)
            }
        }
    }
}
