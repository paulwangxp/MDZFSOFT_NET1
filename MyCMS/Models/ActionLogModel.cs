using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Data.Entity;

namespace MyCMS.Models
{
    [Table("ActionLog")]
    public class ActionLogModel
    {
        
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public virtual int ActionLogId { get; set; }

        [DisplayName("访问IP")]
        public virtual string IP { get; set; }

        public virtual int UserId { get; set; }

        public virtual string DepartmentName { get; set; }

        public virtual string RoleName { get; set; }

        [DisplayName("访问时间")]
        public virtual DateTime VisitTime { get; set; }

        [DisplayName("访问控制器")]
        public virtual string Controller { get; set; }

        [DisplayName("访问方法")]
        public virtual string Action { get; set; }

        [DisplayName("访问内容")]
        public virtual string Discription { get; set; }

        [DisplayName("浏览器类型")]
        public virtual string BrowserType { get; set; }

        [DisplayName("浏览器版本")]
        public virtual string BrowserVersion { get; set; }

        //[DisplayName("部门")]
        //public virtual DepartmentModel Dep { get; set; }

        //[DisplayName("角色")]
        //public virtual webpages_Role Role { get; set; }

    }


    public class LogDBContent : DbContext
    {
        public DbSet<ActionLogModel> ActionLogs { get; set; }
    }
}