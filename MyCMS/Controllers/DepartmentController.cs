using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyCMS.Models;
using Microsoft.Web.WebPages.OAuth;

namespace MyCMS.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
        //private string s1 = "|__　";//分隔符
        private string s1 = "　";//分隔符

        private MyCMSDBContent db = new MyCMSDBContent();

        //
        // GET: /Department/

        public ActionResult Index()
        {
            //return View(db.Departments.ToList());
            return View(GetSortAllList());
        }

        private List<DepartmentModel> GetSortAllList()
        {

            List<DepartmentModel> SortList = new List<DepartmentModel>();

            int depth = 1;

            GetAllChildrenList(null, ref SortList, depth);

            return SortList;
        }

        private List<DepartmentModel> GetAllChildrenList(DepartmentModel department, ref List<DepartmentModel> SortList, int depth)
        {
            int userId = int.Parse(Request.Cookies["User"]["UserId"]);

            if (department == null 
                && userId == 1)//第一次调用时取第一级，所以parentId = 0,必须是系统管理员才能看到所有部门信息
            {
                department = new DepartmentModel { ParentId = 0 };
            }

            var list = db.Departments.Where(p => p.ParentId == department.DepartmentId).OrderBy(o => o.DepartmentId).OrderByDescending(o => o.SortId);

            if (depth == 1)
            {
                
                
                var users = db.Users.Where(u => u.UserId == userId).ToList();
                int id = users[0].DepartmentId;

                if (userId == 1)//系统管理员
                    list = db.Departments.Where(p => p.ParentId == department.DepartmentId).OrderBy(o => o.DepartmentId).OrderByDescending(o => o.SortId);
                else
                    list = db.Departments.Where(p => p.DepartmentId == id).OrderBy(o => o.DepartmentId).OrderByDescending(o => o.SortId);
            }

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
                if (depth != 2)
                    child.name = s2 + s1 + child.name;

                SortList.Add(child);//添加下级的ParentId

                GetAllChildrenList(child, ref SortList, depth);
            }

            return SortList;
        }

        //
        // GET: /Department/Details/5

        public ActionResult Details(int id = 0)
        {
            DepartmentModel department = db.Departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        //
        // GET: /Department/Create

        public ActionResult Create()
        {
            ViewBag.ParentId = getParentSelectList(null);
            return View();
        }

        //
        // POST: /Department/Create

        [HttpPost]
        public ActionResult Create(DepartmentModel department)
        {
            if (ModelState.IsValid)
            {
                db.Departments.Add(department);
                db.SaveChanges();

                if (!savePath(department))//保存path字段，路径如 0,1,2
                {
                    return View(department);
                }

                return RedirectToAction("Index");
            }

            return View(department);
        }

        //获取parentId字段的下拉选择集合
        private List<SelectListItem> getParentSelectList(DepartmentModel menu)
        {
            int depth = 1;

            var AllList = new List<SelectListItem>();
            AllList.Add(new SelectListItem { Text = "/", Value = "0" });//添加根parentId

            if (menu == null)
            {
                menu = new DepartmentModel { ParentId = 0 };
            }

            int selectedParentId = menu.ParentId;//传入的记录的上级菜单id，用于设定selectList的默认选中项

            GetChildrenList(menu, AllList, depth, selectedParentId);

            return AllList;
        }

        private List<DepartmentModel> GetChildrenList(DepartmentModel dep, List<SelectListItem> AllList, int depth, int selectedParentId)
        {
            var list = db.Departments.Where(o => o.ParentId == dep.DepartmentId).OrderBy(o => o.DepartmentId).OrderByDescending(o => o.SortId);

            if (depth == 1)//如果是第一层，那应该取parentId为0的
                list = db.Departments.Where(p => p.ParentId == 0).OrderBy(o => o.DepartmentId).OrderByDescending(o => o.SortId);

            if (list.Count() == 0)
                return null;

            ++depth;
            string s2 = "";
            for (int j = 1; j < depth; j++)
            {
                s2 += "　";//分隔符
            }

            foreach (var child in list)
            {
                if (selectedParentId == child.DepartmentId)//如果此菜单是传入菜单的父级
                    AllList.Add(new SelectListItem { Text = s2  + s1 + child.name, Value = child.DepartmentId.ToString(), Selected = true });//添加下级的ParentId
                else
                    AllList.Add(new SelectListItem { Text = s2  + s1 + child.name, Value = child.DepartmentId.ToString() });//添加下级的ParentId

                GetChildrenList(child, AllList, depth, selectedParentId);
            }

            return list.ToList();
        }


        private List<DepartmentModel> GetFirstSonList()
        {
            var list = db.Departments.Where(o => o.ParentId == 0);
            return list.ToList();
        }

        //
        // GET: /Department/Edit/5

        public ActionResult Edit(int id = 0)
        {
            DepartmentModel department = db.Departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }

            ViewBag.ParentId = getParentSelectList(department);
            return View(department);
        }

        //
        // POST: /Department/Edit/5

        [HttpPost]
        public ActionResult Edit(DepartmentModel department)
        {
            
            if (ModelState.IsValid)
            {
                db.Entry(department).State = EntityState.Modified;
                db.SaveChanges();

                if (!savePath(department))//保存path字段，路径如 0,1,2
                {
                    return View(department);
                }

                return RedirectToAction("Index");
            }
            return View(department);
        }

        //根据id及parentid保存path，便于数据的检索
        private bool savePath(DepartmentModel department)
        {
            try
            {
                if (department.ParentId == 0)
                {
                    department.Path = "0," + department.DepartmentId + ",";
                }
                else
                {
                    DepartmentModel parentData = db.Departments.Single(d => d.DepartmentId == department.ParentId);
                    department.Path = parentData.Path  + department.DepartmentId + ",";
                }

                db.Entry(department).State = EntityState.Modified;
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
        // GET: /Department/Delete/5

        public ActionResult Delete(int id = 0)
        {
            DepartmentModel department = db.Departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        //
        // POST: /Department/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var users = from user in db.Users
                        where user.DepartmentId == id
                        select user;
            if (users.ToList().Count > 0)//部门下有用户不能删除
            {
                ModelState.AddModelError("", "部门下有用户存在，无法删除！");
                return View(db.Departments.Find(id));
            }

            var child = from dep in db.Departments
                        where dep.ParentId == id
                        select dep;
            if (child.ToList().Count > 0 || id == 0)//有子部门，或者自己是根部门的不允许删除
            {
                ModelState.AddModelError("", "部门下有子部门存在，无法删除！");
                return View(db.Departments.Find(id));
            }

            DepartmentModel department = db.Departments.Find(id);
            db.Departments.Remove(department);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}