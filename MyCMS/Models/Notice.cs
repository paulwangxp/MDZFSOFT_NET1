using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyCMS.Models
{
    [Table("Notice")]
    public class NoticeModel
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public virtual int NoticeId { get; set; }

        [Required]
        [DisplayName("公告标题　　")]
        public virtual string Title { get; set; }

        [Required]
        [DisplayName("公告内容　　")]
        public virtual string Content { get; set; }

        [Required]
        [DisplayName("创建时间　　")]
        public virtual DateTime CreateDate { get; set; }

        [Required]
        [DisplayName("创建者　　　")]
        public virtual int CreateUserId { get; set; }

        [Required]
        [DisplayName("公告接收部门")]
        public virtual string DepartmentId { get; set; }

    }
}