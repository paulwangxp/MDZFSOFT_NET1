using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MyCMS.Models
{
    [Table("Device")]
    public class DeviceModel
    {

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public virtual int DeviceId { get; set; }

        [Required]
        [DisplayName("厂商名称")]
        public virtual int VendorId { get; set; }

        [Required]
        [DisplayName("设备类型")]
        public virtual int DeviceTypeId { get; set; }

        [Required]
        [DisplayName("设备编号")]
        public virtual string DeviceNumber { get; set; }

        [DisplayName("创建时间")]
        public virtual DateTime CreateTime { get; set; }

        [DisplayName("修改时间")]
        public virtual DateTime ModifyTime { get; set; }

        [Required]
        [DisplayName("当前用户")]
        public virtual int UserId { get; set; }

        public virtual VendorModel vendor { get; set; }

        public virtual DeviceType deviceType { get; set; }

        public virtual UserProfile user { get; set; }

    }
}