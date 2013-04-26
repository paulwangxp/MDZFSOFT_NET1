using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyCMS.Models;

namespace MyCMS.Controllers
{
    public  class MyTools : Controller
    {
        private MyCMSDBContent db = new MyCMSDBContent();


        public string UserName { get; set; }
        public string UserId { get; set; }
        public string name { get; set; }
        public string UserCode { get; set; }
        public string UserRoleId { get; set; }
        public string UserRoleName { get; set; }
        public string DepName { get; set; }
        public string DepID { get; set; }

        public MyTools()
        {
            
        }

        public List<DepartmentModel> GetDepList(string depid)
        {
            int ownerDepId = int.Parse(depid);
            var path = from dep in db.Departments
                       where dep.DepartmentId == ownerDepId
                       select dep.Path;
            string myPath = path.ToList()[0];
            var depList = from dep in db.Departments
                          where dep.Path.Contains(myPath)
                          orderby dep.Path
                          select dep;

            return depList.ToList();
        }
    }
}