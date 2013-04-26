using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using MyCMS.Filters;
using MyCMS.Models;
using System.EnterpriseServices;
using System.Data.Entity.Validation;

namespace MyCMS.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class AccountController : Controller
    {
        private MyCMSDBContent db = new MyCMSDBContent();

        [Description("获取验证码")]
        [AllowAnonymous]
        public ActionResult GetValidateCode()
        {

            ValidateCode vCode = new ValidateCode();

            string code = vCode.CreateValidateCode(4);

            Session["ValidateCode"] = code;

            byte[] bytes = vCode.CreateValidateGraphic(code);

            return File(bytes, @"image/jpeg");

        }


        [Description("获取当前验证码")]
        [AllowAnonymous]
        public ActionResult GetCurrentValidateCode()
        {

            return Content(Session["ValidateCode"].ToString());

        }

        //
        // GET: /Account/Login

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View("~/Views/Account/Login.cshtml", "~/Views/Shared/_LoginLayout.cshtml");
        }

        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (Session["ValidateCode"] == null)
            {
                ModelState.AddModelError("", "系统访问超时，请重新登录");
                return View("~/Views/Account/Login.cshtml", "~/Views/Shared/_LoginLayout.cshtml", model);
            }
            if (Session["ValidateCode"].ToString() != model.ValidateCode)
            {
                ModelState.AddModelError("", "验证码错误");
                return View("~/Views/Account/Login.cshtml", "~/Views/Shared/_LoginLayout.cshtml",model);
            }
            

            if (ModelState.IsValid && WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
            {

                //Session["UserName"] = model.UserName;

                //获得已登录用户相关信息，并写入session

                var userInfo = from role in db.Roles
                               join us in db.Users on role.RoleId equals us.RoleId
                               join dep in db.Departments on us.DepartmentId equals dep.DepartmentId
                               where us.UserName.Equals(model.UserName)
                               select new
                               {
                                   role.Description,
                                   role.RoleId,
                                   us.UserCode,
                                   us.Name,
                                   us.UserId,
                                   dep.name,
                                   dep.DepartmentId

                               };


                //将用户信息写入cookies，取消原来写入session的做法
                Response.Cookies["User"]["UserName"] = HttpUtility.UrlEncode(model.UserName);
                Response.Cookies["User"]["UserId"] = userInfo.ToList()[0].UserId.ToString();
                Response.Cookies["User"]["name"] = HttpUtility.UrlEncode(userInfo.ToList()[0].Name);
                Response.Cookies["User"]["UserCode"] = userInfo.ToList()[0].UserCode;
                Response.Cookies["User"]["UserRoleId"] = userInfo.ToList()[0].RoleId.ToString();
                Response.Cookies["User"]["UserRoleName"] = HttpUtility.UrlEncode(userInfo.ToList()[0].Description);
                Response.Cookies["User"]["DepName"] = HttpUtility.UrlEncode(userInfo.ToList()[0].name);
                Response.Cookies["User"]["DepID"] = userInfo.ToList()[0].DepartmentId.ToString();
                //Response.Cookies["User"].Expires = DateTime.Now.AddDays(1);//有效期1天


                //Session["UserId"] = userInfo.ToList()[0].UserId;
                //Session["name"] = userInfo.ToList()[0].Name;
                //Session["UserCode"] = userInfo.ToList()[0].UserCode;
                //Session["UserRoleId"] = userInfo.ToList()[0].RoleId;
                //Session["UserRoleName"] = userInfo.ToList()[0].Description;
                //Session["DepName"] = userInfo.ToList()[0].name;

                return RedirectToLocal(returnUrl);
            }

            // 如果我们进行到这一步时某个地方出错，则重新显示表单
            ModelState.AddModelError("", "提供的用户名或密码不正确。");
            return View("~/Views/Account/Login.cshtml", "~/Views/Shared/_LoginLayout.cshtml",model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            //退出时删除cookies
            Request.Cookies["User"].Expires = DateTime.Now.AddDays(-1);
            Request.Cookies["User"]["UserId"] = "";
            WebSecurity.Logout();

            return RedirectToAction("Index", "Home");
        }



        [Authorize]
        [HttpPost]
        //重置密码
        public ActionResult ChangePwd223(string userName)
        {
            bool result1 = false;
            string msg1 = "";

            try
            {
                string token = WebSecurity.GeneratePasswordResetToken(userName);
                result1 = WebSecurity.ResetPassword(token, "888888");
            }
            catch (Exception ex)
            {

                msg1 = ex.Message;
            }
            
            

            JsonResult json = new JsonResult();
            json.Data = new
            {
                result = result1,
                msg = msg1
            };
            return json;
        }

        //
        // GET: /Account/Register

        [AllowAnonymous]
        public ActionResult Register()
        {
            ViewBag.DepartmentId = new SelectList(GetOwnerAndChildDep(), "DepartmentId", "name");
            ViewBag.RoleId = new SelectList(GetOwnerAndChildRole(), "RoleId", "RoleName");

            List<SelectListItem> li = new List<SelectListItem>();
            li.Add(new SelectListItem { Text = "男", Value = "1", Selected = true });
            li.Add(new SelectListItem { Text = "女", Value = "0" });
            ViewBag.Sex = li;


            List<SelectListItem> enableLi = new List<SelectListItem>();
            li.Add(new SelectListItem { Text = "启用", Value = "1", Selected = true });
            li.Add(new SelectListItem { Text = "停用", Value = "0" });
            ViewBag.Enable = enableLi;

            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            ViewBag.DepartmentId = new SelectList(GetOwnerAndChildDep(), "DepartmentId", "name");
            ViewBag.RoleId = new SelectList(GetOwnerAndChildRole(), "RoleId", "RoleName");

            if (ModelState.IsValid)
            {

                if (WebSecurity.UserExists(model.UserName))
                {
                    ModelState.AddModelError("UserName", "用户名已经存在");
                    return View(model);
                }



                var UserCodeCount = from user in db.Users
                                    where user.UserCode.Equals(model.UserCode)
                                    select new { user.UserCode };
                if (UserCodeCount.Count() > 0)
                {
                    ModelState.AddModelError("UserCode", "警员编号已经存在");
                    return View(model);
                }

                model.User.UserName = model.UserName;
                model.User.UserCode = model.UserCode;
                model.User.DepartmentId = model.DepartmentId;
                model.User.RoleId = model.RoleId;
                model.User.Name = model.Name;
                model.User.Sex = model.Sex;
                model.User.Enable = model.Enable;
                db.Users.Add(model.User);
                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException dbEx)
                {
                    ModelState.AddModelError("UserName", dbEx.EntityValidationErrors.ToList()[0].ValidationErrors.ToList()[0].ErrorMessage);
                    return View(model);
                }

                // 尝试注册用户
                try
                {
                    //WebSecurity.CreateUserAndAccount(model.UserName, model.Password);
                    WebSecurity.CreateAccount(model.UserName, model.Password);

                    

                    //如果未登录的情况下，就自动登录并回到首页
                    if (!WebSecurity.IsAuthenticated)
                    {
                        WebSecurity.Login(model.UserName, model.Password);
                        return RedirectToAction("Index", "Home");
                    }

                    //提示用户添加成功并清除表单数据
                    Response.Write("<script>alert('添加用户成功')</script>");
                    ModelState.Clear();
                    return View();
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("UserName", ErrorCodeToString(e.StatusCode));
                }
            }

            // 如果我们进行到这一步时某个地方出错，则重新显示表单            
            return View(model);
        }

        private List<webpages_Role> GetOwnerAndChildRole()
        {
            List<webpages_Role> SortList = new List<webpages_Role>();

            //根据用户名获取用户所角色ID
            var user1 = from user in db.Users
                        where user.UserName.Equals(WebSecurity.CurrentUserName)
                        select user;
            int RoleId = user1.ToList()[0].RoleId;


            //获取同等或者更大的角色ID，注意角色表ID越大，对应权限越小！！
            var roleList = from role in db.Roles
                           where role.RoleId >= RoleId
                           select role;

            //将对应部门添加入list
            SortList.AddRange(roleList);

            return SortList;
        }

        //获取自己及下级的部门对象
        private List<DepartmentModel> GetOwnerAndChildDep()
        {
            List<DepartmentModel> SortList = new List<DepartmentModel>();

            //根据用户名获取用户所属部门ID
            var user1 = from user in db.Users
                        where user.UserName.Equals(WebSecurity.CurrentUserName)
                        select user;
            int depId1 = user1.ToList()[0].DepartmentId;


            //获取用户对应的部门对象
            var depOwner = from dep in db.Departments
                       where dep.DepartmentId.Equals(depId1)
                       select dep;

            //将对应部门添加入list
            SortList.Add(depOwner.ToList()[0]);

            int depth = 1;

            //将下属所有部门添加入list
            GetAllChildrenList(depOwner.ToList()[0], ref SortList, depth);

            return SortList;
        }

        private List<DepartmentModel> GetAllChildrenList(DepartmentModel departmentObj, ref List<DepartmentModel> SortList, int depth)
        {
            string s1 = "|__　";//分隔符

            var list = db.Departments.Where(p => p.ParentId == departmentObj.DepartmentId).OrderBy(o => o.DepartmentId).OrderByDescending(o => o.SortId);

            if (list.Count() == 0)
                return SortList;

            ++depth;
            string s2 = "";
            for (int j = 1; j < depth; j++)
            {
                s2 += "　";//分隔符 全角空格
            }

            foreach (var child in list)
            {
                if (depth > 1)
                    child.name = s2 + s1 + child.name;

                SortList.Add(child);//添加下级的ParentId

                GetAllChildrenList(child, ref SortList, depth);
            }

            return SortList;
        }

        

        //
        // POST: /Account/Disassociate

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Disassociate(string provider, string providerUserId)
        {
            string ownerAccount = OAuthWebSecurity.GetUserName(provider, providerUserId);
            ManageMessageId? message = null;

            // 只有在当前登录用户是所有者时才取消关联帐户
            if (ownerAccount == User.Identity.Name)
            {
                // 使用事务来防止用户删除其上次使用的登录凭据
                using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
                    if (hasLocalAccount || OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name).Count > 1)
                    {
                        OAuthWebSecurity.DeleteAccount(provider, providerUserId);
                        scope.Complete();
                        message = ManageMessageId.RemoveLoginSuccess;
                    }
                }
            }

            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage

        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "已更改你的密码。"
                : message == ManageMessageId.SetPasswordSuccess ? "已设置你的密码。"
                : message == ManageMessageId.RemoveLoginSuccess ? "已删除外部登录。"
                : "";
            ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(LocalPasswordModel model)
        {
            bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.HasLocalPassword = hasLocalAccount;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasLocalAccount)
            {
                if (ModelState.IsValid)
                {
                    // 在某些失败方案中，ChangePassword 将引发异常，而不是返回 false。
                    bool changePasswordSucceeded;
                    try
                    {
                        changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                    }
                    catch (Exception)
                    {
                        changePasswordSucceeded = false;
                    }

                    if (changePasswordSucceeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        ModelState.AddModelError("", "当前密码不正确或新密码无效。");
                    }
                }
            }
            else
            {
                // 用户没有本地密码，因此将删除由于缺少
                // OldPassword 字段而导致的所有验证错误
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        WebSecurity.CreateAccount(User.Identity.Name, model.NewPassword);
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError("", e);
                    }
                }
            }

            // 如果我们进行到这一步时某个地方出错，则重新显示表单
            return View(model);
        }

        //
        // POST: /Account/ExternalLogin

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback

        [AllowAnonymous]
        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            AuthenticationResult result = OAuthWebSecurity.VerifyAuthentication(Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
            if (!result.IsSuccessful)
            {
                return RedirectToAction("ExternalLoginFailure");
            }

            if (OAuthWebSecurity.Login(result.Provider, result.ProviderUserId, createPersistentCookie: false))
            {
                return RedirectToLocal(returnUrl);
            }

            if (User.Identity.IsAuthenticated)
            {
                // 如果当前用户已登录，则添加新帐户
                OAuthWebSecurity.CreateOrUpdateAccount(result.Provider, result.ProviderUserId, User.Identity.Name);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // 该用户是新用户，因此将要求该用户提供所需的成员名称
                string loginData = OAuthWebSecurity.SerializeProviderUserId(result.Provider, result.ProviderUserId);
                ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(result.Provider).DisplayName;
                ViewBag.ReturnUrl = returnUrl;
                return View("ExternalLoginConfirmation", new RegisterExternalLoginModel { UserName = result.UserName, ExternalLoginData = loginData });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLoginConfirmation(RegisterExternalLoginModel model, string returnUrl)
        {
            string provider = null;
            string providerUserId = null;

            if (User.Identity.IsAuthenticated || !OAuthWebSecurity.TryDeserializeProviderUserId(model.ExternalLoginData, out provider, out providerUserId))
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // 将新用户插入到数据库
                using (UsersContext db = new UsersContext())
                {
                    UserProfile user = db.UserProfiles.FirstOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower());
                    // 检查用户是否已存在
                    if (user == null)
                    {
                        // 将名称插入到配置文件表
                        db.UserProfiles.Add(new UserProfile { UserName = model.UserName });
                        db.SaveChanges();

                        OAuthWebSecurity.CreateOrUpdateAccount(provider, providerUserId, model.UserName);
                        OAuthWebSecurity.Login(provider, providerUserId, createPersistentCookie: false);

                        return RedirectToLocal(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError("UserName", "用户名已存在。请输入其他用户名。");
                    }
                }
            }

            ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(provider).DisplayName;
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // GET: /Account/ExternalLoginFailure

        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult ExternalLoginsList(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return PartialView("_ExternalLoginsListPartial", OAuthWebSecurity.RegisteredClientData);
        }

        [ChildActionOnly]
        public ActionResult RemoveExternalLogins()
        {
            ICollection<OAuthAccount> accounts = OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name);
            List<ExternalLogin> externalLogins = new List<ExternalLogin>();
            foreach (OAuthAccount account in accounts)
            {
                AuthenticationClientData clientData = OAuthWebSecurity.GetOAuthClientData(account.Provider);

                externalLogins.Add(new ExternalLogin
                {
                    Provider = account.Provider,
                    ProviderDisplayName = clientData.DisplayName,
                    ProviderUserId = account.ProviderUserId,
                });
            }

            ViewBag.ShowRemoveButton = externalLogins.Count > 1 || OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            return PartialView("_RemoveExternalLoginsPartial", externalLogins);
        }

        #region 帮助程序
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // 请参见 http://go.microsoft.com/fwlink/?LinkID=177550 以查看
            // 状态代码的完整列表。
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "用户名已存在。请输入其他用户名。";

                case MembershipCreateStatus.DuplicateEmail:
                    return "该电子邮件地址的用户名已存在。请输入其他电子邮件地址。";

                case MembershipCreateStatus.InvalidPassword:
                    return "提供的密码无效。请输入有效的密码值。";

                case MembershipCreateStatus.InvalidEmail:
                    return "提供的电子邮件地址无效。请检查该值并重试。";

                case MembershipCreateStatus.InvalidAnswer:
                    return "提供的密码取回答案无效。请检查该值并重试。";

                case MembershipCreateStatus.InvalidQuestion:
                    return "提供的密码取回问题无效。请检查该值并重试。";

                case MembershipCreateStatus.InvalidUserName:
                    return "提供的用户名无效。请检查该值并重试。";

                case MembershipCreateStatus.ProviderError:
                    return "身份验证提供程序返回了错误。请验证您的输入并重试。如果问题仍然存在，请与系统管理员联系。";

                case MembershipCreateStatus.UserRejected:
                    return "已取消用户创建请求。请验证您的输入并重试。如果问题仍然存在，请与系统管理员联系。";

                default:
                    return "发生未知错误。请验证您的输入并重试。如果问题仍然存在，请与系统管理员联系。";
            }
        }
        #endregion
    }
}
