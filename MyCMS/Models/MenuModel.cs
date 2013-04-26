using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyCMS.Models
{
    [Table("Menus")]
    public class MenuModel
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public virtual int MenuId { get; set; }

        [Required]
        [DisplayName("菜单名　　")]
        public virtual string MenuName { get; set; }

        [Required]
        [DisplayName("菜单URL　 ")]
        public virtual string MenuUrl { get; set; }

        [Required]
        [DisplayName("根菜单　　")]
        public virtual int isRootMenu { get; set; }

        [Required]
        [DisplayName("上一级菜单")]
        public virtual int ParentId { get; set; }

        [Required]
        [DisplayName("是否显示　")]
        public virtual int isShow { get; set; }

        [Required]
        [DisplayName("排　　序　")]
        public virtual int SortId { get; set; }

        [Required]
        [DisplayName("路　　径　")]
        public virtual string Path { get; set; }

        [DisplayName("创建时间　")]
        public virtual DateTime CreateDate { get; set; }

        [DisplayName("修改时间　")]
        public virtual DateTime ModifyDate { get; set; }

    }
}