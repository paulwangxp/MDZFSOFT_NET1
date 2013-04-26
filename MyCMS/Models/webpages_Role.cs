using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyCMS.Models
{
    [Table("webpages_Roles")]
    public class webpages_Role
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int RoleId { get; set; }

        [Required]
        [DisplayName("角色名称")]
        public string RoleName { get; set; }

        [Required]
        [DisplayName("描　　述")]
        public string Description { get; set; }
    }
}