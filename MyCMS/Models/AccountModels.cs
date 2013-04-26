using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;
using System.ComponentModel;

namespace MyCMS.Models
{
    public class UsersContext : DbContext
    {
        public UsersContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
    }
     


    

    public class RegisterExternalLoginModel
    {
        [Required]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        public string ExternalLoginData { get; set; }
    }

    public class LocalPasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "当前密码")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Display(Name = "新密码")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认新密码")]
        [Compare("NewPassword", ErrorMessage = "新密码和确认密码不匹配。")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginModel
    {
        [Required]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "验证码")]
        public string ValidateCode { get; set; }


        [Display(Name = "记住我?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterModel
    {
        public RegisterModel()
        {
            User = new UserProfile();
        }

        [Required]
        [Display(Name = "用户名　")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Display(Name = "密码　　")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Compare("Password", ErrorMessage = "密码和确认密码不匹配。")]
        public string ConfirmPassword { get; set; }

        public UserProfile User { get; set; }

        [Required]
        [DisplayName("警员编号")]
        [StringLength(6, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 6)]
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
        public int Sex { get; set; }//add by paul

        [Required]
        [DisplayName("状态　　")]
        public int Enable { get; set; }//add by paul

        //[Required]
        //[DisplayName("部门　　")]
        //public virtual DepartmentModel Dep { get; set; }

        //[Required]
        //[DisplayName("角色　　")]
        //public virtual webpages_Role Role { get; set; }
    }

    public class ExternalLogin
    {
        public string Provider { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderUserId { get; set; }
    }
}
