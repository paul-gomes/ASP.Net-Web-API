using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASP.Net_Web_API.Classes
{
    public class Functionality
    {

        public static IEnumerable<SelectListItem> getStandardList()
        {
            using (var ctx = new SchoolEntities1())
            {
                var standardList = ctx.Standards.
                    Select(s => new SelectListItem()
                    {
                        Value = s.StandardId.ToString(),
                        Text = s.StandardName,
                    }).ToList();

                var standardSelct = new SelectListItem()
                {
                    Value = null,
                    Text = "---Select student standard---",
                };

                standardList.Insert(0, standardSelct);
                return standardList;
            }

        }




    }
}