using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MyCMS.Models
{
    public class ArticleType
    {
        public virtual int ArticleTypeId { get; set; }

        [Required]
        [DisplayName("文章类型")]
        public virtual string Name { get; set; }

        [Required]
        [DisplayName("类型描述")]
        public virtual string Description { get; set; }

        [Required]
        [DisplayName("上级类型")]
        public virtual int ParentId { get; set; }

        [Required]
        [DisplayName("排序")]
        public virtual int SortId { get; set; }
    }
}