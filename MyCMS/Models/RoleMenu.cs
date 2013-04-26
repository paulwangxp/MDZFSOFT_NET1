using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyCMS.Models
{
    public class RoleMenu
    {
        public virtual int RoleMenuId { get; set; }

        [Required]
        [DisplayName("菜单")]
        public virtual int MenuId { get; set; }

        [Required]
        [DisplayName("角色")]
        public virtual int UserRoleId { get; set; }

        [Required]
        [DisplayName("启用")]
        public virtual bool Enable { get; set; }

        public virtual MenuModel menus { get; set; }
    }
}