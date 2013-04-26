using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyCMS.Models
{
    [Table("Departments")]
    public class DepartmentModel
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Required]
        public virtual int DepartmentId { get; set; }

        [Required]
        [DisplayName("上级部门")]
        public virtual int ParentId { get; set; }

        [Required]
        [DisplayName("部门名称")]
        public virtual string name { get; set; }

        [Required]
        [DisplayName("描　　述")]
        public virtual string Description { get; set; }

        [Required(ErrorMessage="排序内容必须是0-999的数字")]
        //[Range(0, 999)]
        [DisplayName("排　　序")]
        public virtual int SortId { get; set; }

        [Required]
        [DisplayName("路　　径")]
        public virtual string Path { get; set; }
    }
}