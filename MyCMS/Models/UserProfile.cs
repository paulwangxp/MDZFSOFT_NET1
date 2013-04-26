using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MyCMS.Models
{
    [Table("UserProfile")]
    public class UserProfile
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Required]
        [DisplayName("用户名　")]
        public string UserName { get; set; }

        [Required]
        [DisplayName("警员编号")]
        [StringLength(100, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 5)]
        public string UserCode { get; set; }

        [Required]
        [DisplayName("部门　　")]
        public int DepartmentId { get; set; }//add by paul 扩展了用户表字段

        [Required]
        [DisplayName("角色　　")]
        public int RoleId { get; set; }//add by paul

        [Required]
        [DisplayName("姓名　　")]
        public string Name { get; set; }//add by paul

        [Required]
        [DisplayName("性别　　")]
        [Range(0, 1)]
        public int Sex { get; set; }//add by paul

        [Required]
        [Range(0, 1)]
        [DisplayName("状态　　")]
        public int Enable { get; set; }//add by paul

        [DisplayName("设备编号")]
        public int DeviceId { get; set; }//add by paul


        [DisplayName("部门　　")]
        public virtual DepartmentModel Dep { get; set; }

        [DisplayName("角色　　")]
        public virtual webpages_Role Role { get; set; }
    }
}