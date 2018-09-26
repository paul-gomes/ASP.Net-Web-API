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


        //Post a new student 
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


        //Edit existing student 
        //Shows existing student for editing
        public ActionResult Edit(int id)
        {
            StudentViewModel student = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:51541/api/");

                //HTTP Get
                var responseTask = client.GetAsync("student?id=" + id.ToString());
                responseTask.Wait();

                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<StudentViewModel>();
                    readTask.Wait();
                    student = readTask.Result;
                }

                return View(student);
            };

            
        }

        //posting edited data into the database

        [HttpPost]
        public ActionResult Edit(StudentViewModel student)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:51541/api/");
                //var postTask = client.PostAsJsonAsync<StudentViewModel>("student", student);

                var postTask = client.PutAsJsonAsync<StudentViewModel>("student", student);
                postTask.Wait();

                var result = postTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error. Please contact the administrator.");
                }
            }
            return View(student);
        }



        //Delete a student record 

        public ActionResult Delete(int id)
        {
            using(var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:51541/api/");
                var deleteTask = client.DeleteAsync("student/" + id.ToString());
                deleteTask.Wait();

                var result = deleteTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }

            return RedirectToAction("Error.cshtml");

        }
    }



}