using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace MyCMS.Models
{
    public class FileUserViewModel
    {
        public virtual FileModel files {get;set;}
        public virtual string uploadUserName { get; set; }
        public virtual string recordUserName { get; set; }

        
    }
}