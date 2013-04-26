using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MyCMS.Models
{

    public class AuthorizeDBContent : DbContext
    {       

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<webpages_Role> webpages_Roles { get; set; }
        public DbSet<MenuModel> Menus { get; set; }
        public DbSet<RoleMenu> RoleMenus { get; set; }
    }
}