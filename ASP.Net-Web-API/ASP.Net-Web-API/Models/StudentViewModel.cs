﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP.Net_Web_API.Models
{
    public class StudentViewModel
    {
        public int Id { get; set; }
        public string StudentName { get; set; }
        

        public AddressViewModel Address { get; set; }
        public StandardViewModel Standard { get; set; }
    }
}