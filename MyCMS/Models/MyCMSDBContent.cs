using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace MyCMS.Models
{
    public class MyCMSDBContent : DbContext
    {
        
        public DbSet<MenuModel> Menus { get; set; }
        public DbSet<RoleMenu> RoleMenus { get; set; }
        public DbSet<ArticleModel> Articles { get; set; }
        public DbSet<ArticleType> ArticleTypes { get; set; }
        public DbSet<CategoryModel> Categories { get; set; }

        public DbSet<DepartmentModel> Departments { get; set; }

        public DbSet<UserProfile> Users { get; set; }
        public DbSet<webpages_Role> Roles { get; set; }

        public DbSet<NoticeModel> Notices { get; set; }

        public DbSet<FileModel> FileModels { get; set; }

        public DbSet<VendorModel> VendorModels { get; set; }

        public DbSet<DeviceModel> DeviceModels { get; set; }

        public DbSet<DeviceType> DeviceTypes { get; set; }

        
    }
}