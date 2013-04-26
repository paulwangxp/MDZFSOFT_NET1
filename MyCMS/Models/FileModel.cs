using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyCMS.Models
{
    [Table("Files")]
    public class FileModel
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public virtual int FileId { get; set; }


        [Required]
        [DisplayName("上传文件类型")]
        public virtual int UploadFileType { get; set; }

        [Required]
        [DisplayName("上传人")]
        [ForeignKey("UploadUser")]
        public virtual int UploadUserId { get; set; }

        [Required]
        [DisplayName("上传人部门")]
        public virtual int UploadUserDepartmentId { get; set; }

        [Required]
        [DisplayName("上传人IP")]
        public virtual string UploadUserIP { get; set; }

        [Required]
        [DisplayName("采集人")]
        [ForeignKey("RecordUser")]
        public virtual int RecordUserId { get; set; }

        [Required]
        [DisplayName("采集人部门")]
        public virtual int RecordUserDepartmentId { get; set; }

        [Required]
        [DisplayName("上传文件名")]
        public virtual string UploadFileName { get; set; }

        [Required]
        [DisplayName("创建时间")]
        public virtual DateTime CreateTime { get; set; }

        [Required]
        [DisplayName("录制时间")]
        public virtual DateTime RecordTime { get; set; }

        [Required]
        [DisplayName("上传时间")]
        public virtual DateTime UploadTime { get; set; }

        [DisplayName("播放时长")]
        public virtual string FilePlayTime { get; set; }

        [DisplayName("文件大小")]
        public virtual long FileSize { get; set; }

        [Required]
        [DisplayName("文件重要性")]
        public virtual int FileImportance { get; set; }

        [Required]
        [DisplayName("文件状态")]
        public virtual int FileState { get; set; }

        [DisplayName("图片预览地址")]
        public virtual string ImageShowPath { get; set; }

        [DisplayName("播放地址")]
        public virtual string PlayPath { get; set; }

        [Required]
        [DisplayName("播放前缀")]
        public virtual string RealPath { get; set; }

        [DisplayName("flv播放地址")]
        public virtual string FlvPlayPath { get; set; }

        [DisplayName("文件备注")]
        public virtual string Description { get; set; }


        
        public virtual UserProfile UploadUser { get; set; }
        public virtual UserProfile RecordUser { get; set; }

        
    }
}