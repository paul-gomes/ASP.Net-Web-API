using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ASP.Net_Web_API.Models;
using System.Net.Http;



// This MVC Student Controller

namespace ASP.Net_Web_API.Controllers
{
    public class StudentController : Controller
    {
        // GET: Student
        public ActionResult Index()
        {
            IEnumerable<StudentViewModel> students = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:51541/api/");

                //HTTP Get
                var reponseTask = client.GetAsync("student?includeAddress=true");
                reponseTask.Wait();

                var result = reponseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<StudentViewModel>>();
                    students = readTask.Result;
                }
                else
                {
                    students = Enumerable.Empty<StudentViewModel>();
                    ModelState.AddModelError(string.Empty, "Server Error. Please contanct the administrator.");
                }
            }

            return View(students);
        }

        public ActionResult create()
        {
            return View();
        }


        [HttpPost]
        public ActionResult create(StudentViewModel student)
        {
            using(var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:51541/api/student");

                string selectStudentStandard = Request.Form["stardardId"].ToString();

                switch (selectStudentStandard)
                {
                    case "Freshman" :
                        student.stardardId = 1;
                        break;
                    case "Sophomore":
                        student.stardardId = 2;
                        break;
                    case "Junior":
                        student.stardardId = 3;
                        break;
                    case "Senior":
                        student.stardardId = 4;
                        break;
                    case "Graduate":
                        student.stardardId = 5;
                        break;
                    default:
                        student.stardardId = null;
                        break;
                }
                
                var postTask = client.PostAsJsonAsync<StudentViewModel>("student", student);
                postTask.Wait();
                

                var result = postTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server Error. Please contact the administrator.");
                }

            }
            return View(student);
        }
    }



}