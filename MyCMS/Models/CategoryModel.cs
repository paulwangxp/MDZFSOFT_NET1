using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyCMS.Models
{
    [Table("Categories")]
    public class CategoryModel
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public virtual int CategoryId { get; set; }

        [Required]
        [DisplayName("上级分类")]
        public virtual int ParentId { get; set; }

        [Required]
        [DisplayName("预排序路径")]
        public virtual int PathId { get; set; }

        [Required]
        [DisplayName("排序")]
        public virtual int SortId { get; set; }

        [Required]
        [DisplayName("文章类型")]
        public virtual int ArticleTypeId { get; set; }

        [Required]
        [DisplayName("分类名称")]
        public virtual string name { get; set; }

        [Required]
        [DisplayName("分类描述")]
        public virtual string Description { get; set; }


        [DisplayName("分类图片")]
        public virtual string imgUrl { get; set; }


    }
}