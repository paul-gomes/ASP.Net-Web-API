using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ASP.Net_Web_API.Models;
using System.Net.Http;
using ASP.Net_Web_API.Classes;



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
        public ActionResult create(StudentStdAddress student)
        {
            using(var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:51541/api/student");
                var postTask = client.PostAsJsonAsync<StudentStdAddress>("student", student);
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