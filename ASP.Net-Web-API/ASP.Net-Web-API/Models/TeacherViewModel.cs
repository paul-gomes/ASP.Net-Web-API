using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP.Net_Web_API.Models
{
    public class TeacherViewModel
    {
        public int TeacherId { get; set; }
        public string TeacherName { get; set; }
        public int? room { get; set; }

        

    }
}