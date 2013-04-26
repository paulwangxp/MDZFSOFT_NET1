using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyCMS.Models
{
    [Table("Articles")]
    public class ArticleModel
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public virtual int ArticleId { get; set; }

        [Required]
        [DisplayName("类型")]
        public virtual int ArticleTypeId { get; set; }

        [Required]
        [DisplayName("标题")]
        public virtual string Title { get; set; }

        [Required]
        [DisplayName("内容")]
        public virtual string Content { get; set; }

        [Required]
        [DisplayName("介绍")]
        public virtual string Description { get; set; }

        [Required]
        [DisplayName("创建时间")]
        public virtual DateTime CreateTime { get; set; }

        [Required]
        [DisplayName("前台显示创建时间")]
        public virtual DateTime DisplayTime { get; set; }

        [Required]
        [DisplayName("预览图片")]
        public virtual string ImageUrl { get; set; }

        public virtual string SeoTitle { get; set; }
        public virtual string SeoDesription { get; set; }
        public virtual string SeoMeta { get; set; }
        public virtual string SeoKeywords { get; set; }
        public virtual string Tags { get; set; }

        [Required]
        [DisplayName("查看次数")]
        public virtual int ViewCount { get; set; }

        [Required]
        [DisplayName("是否显示")]
        public virtual int isShow { get; set; }

        [Required]
        [DisplayName("是否置顶")]
        public virtual int isTop { get; set; }

        [Required]
        [DisplayName("排序")]
        public virtual int SortId { get; set; }


        public virtual ArticleType ArticleTypes { get; set; }


    }
}