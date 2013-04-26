using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyCMS.Models;
using System.Runtime.Caching;

namespace MyCMS.Controllers
{
    [Authorize]
    public class MenuController : Controller
    {

        private string s1 = "　";//分隔符

        private MyCMSDBContent db = new MyCMSDBContent();

        //
        // GET: /Menu/

        public ActionResult Index()
        {
            //return View(db.Menus.ToList());
            return View(GetSortAllList());
        }

        

        public List<MenuModel> GetSortAllList()
        {

            List<MenuModel> SortList = new List<MenuModel>();

            int depth = 1;

            GetAllChildrenList(null, ref SortList, depth);

            return SortList;
        }

        private List<MenuModel> GetAllChildrenList(MenuModel menu, ref List<MenuModel>SortList, int depth)
        {

            if (menu == null)//第一次调用时取第一级，所以parentId = 0
            {
                menu = new MenuModel { ParentId  = 0};
            }

            var list = db.Menus.Where(p => p.ParentId == menu.MenuId).OrderBy(o => o.MenuId).OrderByDescending(o => o.SortId);

            if(depth == 1)
                list = db.Menus.Where(p => p.ParentId == menu.ParentId).OrderBy(o => o.MenuId).OrderByDescending(o => o.SortId);

            if (list.Count() == 0)
                return SortList;

            ++depth;
            string s2 = "";
            for (int j = 2; j < depth; j++)
            {
                s2 += "　";//分隔符 全角空格
            }

            foreach (var child in list)
            {
                if(depth != 2)
                    child.MenuName = s2 + s1 + child.MenuName;

                SortList.Add(child);//添加下级的ParentId

                GetAllChildrenList(child, ref SortList, depth);
            }

            return SortList;
        }

        //根据id及parentid保存path，便于数据的检索
        private bool savePath(MenuModel menu)
        {
            try
            {
                if (menu.ParentId == 0)
                {
                    menu.Path = "0," + menu.MenuId + ",";
                }
                else
                {
                    MenuModel parentData = db.Menus.Single(d => d.MenuId == menu.ParentId);
                    menu.Path = parentData.Path + menu.MenuId + ",";
                }

                db.Entry(menu).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return false;
            }

            return true;
        }

        //
        // GET: /Menu/Details/5

        public ActionResult Details(int id = 0)
        {
            MenuModel menu = db.Menus.Find(id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            return View(menu);
        }

        //
        // GET: /Menu/Create

        public ActionResult Create()
        {
            ViewBag.ParentId = getParentSelectList(null);
            //ViewBag.ParentId = new SelectList(db.Menus,"ParentId","MenuName");
            return View();
        }

        //
        // POST: /Menu/Create

        [HttpPost]
        public ActionResult Create(MenuModel menu)
        {
            if (ModelState.IsValid)
            {
                //新建菜单的创建日期和修改日期为当前日期
                menu.CreateDate = DevTools.GetNowDateTime();
                menu.ModifyDate = menu.CreateDate;
                db.Menus.Add(menu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(menu);
        }

        //获取parentId字段的下拉选择集合
        private List<SelectListItem> getParentSelectList(MenuModel menu)
        {
            int depth = 1;

            var AllList = new List<SelectListItem>();
            AllList.Add(new SelectListItem { Text = "/", Value = "0" });//添加根parentId

            if (menu == null)
            {
                menu = new MenuModel { ParentId = 0 };
            }

            int selectedParentId = menu.ParentId;//传入的记录的上级菜单id，用于设定selectList的默认选中项

            GetChildrenList(menu, AllList, depth, selectedParentId);
            
            return AllList;
        }

        private List<MenuModel> GetChildrenList(MenuModel menu, List<SelectListItem>AllList, int depth, int selectedParentId)
        {
            var list = db.Menus.Where(o => o.ParentId == menu.MenuId).OrderBy(o=>o.MenuId).OrderByDescending(o=>o.SortId);

            if(depth == 1)//如果是第一层，那应该取parentId为0的
                list = db.Menus.Where(p => p.ParentId == 0).OrderBy(o=>o.MenuId).OrderByDescending(o=>o.SortId);

            if (list.Count() == 0)
                return null;

            ++depth;
            string s2 = "";
            for (int j = 1; j < depth; j++)
            {
                s2 += "＿";//分隔符
            }

            foreach (var child in list)
            {
                if (selectedParentId == child.MenuId)//如果此菜单是传入菜单的父级
                    AllList.Add(new SelectListItem { Text = s2  + child.MenuName, Value = child.MenuId.ToString(),Selected = true });//添加下级的ParentId
                else
                    AllList.Add(new SelectListItem { Text = s2  + child.MenuName, Value = child.MenuId.ToString() });//添加下级的ParentId

                GetChildrenList(child, AllList, depth, selectedParentId);
            }

            return list.ToList();   
        }


        private List<MenuModel> GetFirstSonList()
        {
            var list = db.Menus.Where(o => o.ParentId == 0);
            return list.ToList();
        }

        

        //
        // GET: /Menu/Edit/5

        public ActionResult Edit(int id = 0)
        {
            MenuModel menu = db.Menus.Find(id);
            if (menu == null)
            {
                return HttpNotFound();
            }

            ViewBag.ParentId = getParentSelectList(menu);
            return View(menu);
        }

        //
        // POST: /Menu/Edit/5

        [HttpPost]
        public ActionResult Edit(MenuModel menu)
        {
            if (ModelState.IsValid)
            {
                //修改日期为当前日期
                menu.ModifyDate = DevTools.GetNowDateTime();

                db.Entry(menu).State = EntityState.Modified;
                db.SaveChanges();

                if (!savePath(menu))//保存path字段，路径如 0,1,2
                {
                    return View(menu);
                }

                return RedirectToAction("Index");
            }
            ViewBag.ParentId = getParentSelectList(menu);

            //暂时不做缓存 OutputCacheAttribute.ChildActionCache = new MemoryCache("MyMenus");

            return View(menu);
        }

        //
        // GET: /Menu/Delete/5

        public ActionResult Delete(int id = 0)
        {
            MenuModel menu = db.Menus.Find(id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            return View(menu);
        }

        //
        // POST: /Menu/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var child = from menus in db.Menus
                        where menus.ParentId == id
                        select menus;
            if (child.ToList().Count > 0)//有用户属于此角色不能删除
            {
                ModelState.AddModelError("", "此菜单有子菜单存在，无法删除！");
                return View(db.Roles.Find(id));
            }

            MenuModel menu = db.Menus.Find(id);
            db.Menus.Remove(menu);
            db.SaveChanges();

            if (!savePath(menu))//保存path字段，路径如 0,1,2
            {
                return View(menu);
            }

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}