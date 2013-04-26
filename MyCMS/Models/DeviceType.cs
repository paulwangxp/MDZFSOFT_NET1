using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MyCMS.Models
{
    public class DeviceType
    {

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public virtual int DeviceTypeId { get; set; }

        [Required]
        [DisplayName("设备名称")]
        public virtual string Name { get; set; }


        [DisplayName("创建时间")]
        public virtual DateTime CreateTime { get; set; }

        [DisplayName("修改时间")]
        public virtual DateTime ModifyTime { get; set; }
    }
}