using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MyCMS.Models
{
    [Table("Vendors")]
    public class VendorModel
    {

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public virtual int VendorId { get; set; }

        [Required]
        [DisplayName("厂商名称")]
        public virtual string Name { get; set; }

        [Required]
        [DisplayName("联系人")]
        public virtual string ContactPerson { get; set; }


        [Required]
        [DisplayName("联系电话")]
        public virtual string Phone { get; set; }

        [Required]
        [DisplayName("联系地址")]
        public virtual string Address { get; set; }

        [DisplayName("创建时间")]
        public virtual DateTime CreateTime { get; set; }

        [DisplayName("修改时间")]
        public virtual DateTime ModifyTime { get; set; }

    }
}