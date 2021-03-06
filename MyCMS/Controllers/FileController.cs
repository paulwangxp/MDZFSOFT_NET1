﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyCMS.Models;
using PagedList;
using System.IO;

namespace MyCMS.Controllers
{
    [Authorize]
    public class FileController : Controller
    {

        enum FileType
        {
            AVI = 0,
            MP4 = 1,
            WAV = 2,
            JPG = 3
        };

        private MyCMSDBContent db = new MyCMSDBContent();

        //
        // GET: /File/

        public ActionResult Index()
        {
            return View(db.FileModels.ToList());
        }

        public ActionResult UploadView(string recordUserId = "", string recordUser = "", 
                                        string uploadUserId = "", string uploadUser="", 
                                        string DepartmentId = "", int page = 1)
        {

            int maxRecords = 8;//每页4条
            int currentPage = page;

            string sDepid = Request.Cookies["User"]["DepID"];

            var files = db.FileModels.Include(u=>u.UploadUser).Include(p=>p.RecordUser);

            //字段搜索功能
            if (!String.IsNullOrEmpty(recordUserId))
            {
                int id = int.Parse(recordUserId);
                files = files.Where(s => s.RecordUserId == id);
            }
            if (!String.IsNullOrEmpty(uploadUserId))
            {
                int id = int.Parse(uploadUserId);
                files = files.Where(s => s.UploadUserId == id);
            }
            if (!String.IsNullOrEmpty(DepartmentId))
            {
                int depId = int.Parse(DepartmentId);
                files = files.Where(s => s.UploadUserDepartmentId == depId);
            }
            else
            {
                int ownerDepId = int.Parse(sDepid);
                files = files.Where(s => s.UploadUserDepartmentId == ownerDepId);
            }

            //return View(files.ToList());

            //files = from file in db.FileModels select file;
            MyTools myTools = new MyTools();
            ViewBag.DepartmentId = new SelectList(myTools.GetDepList(sDepid), "DepartmentId", "name");
            ViewBag.uploadUser = uploadUser;
            ViewBag.uploadUserId = uploadUserId;
            ViewBag.recordUser = recordUser;
            ViewBag.recordUserId = recordUserId;

            //分页需要排序
            files = files.OrderByDescending(p => p.FileId);


            return View(files.ToPagedList(currentPage,
            maxRecords));
        }

        [HttpPost]
        public ActionResult UploadViewFileUrl(int fileId = 0, int fileType = 0)
        {
            bool result = true;
            string fUlr = "";

            FileModel filemodel = db.FileModels.Find(fileId);
            if (filemodel == null)
            {
               result = false;
            }

            fUlr = filemodel.PlayPath;
            

            JsonResult json = new JsonResult();
            json.Data = new
            {
                bSuccess = result,
                fileUrl = fUlr,
                msg = "要查看的文件不存在！"
            };




            return json;
        }

        [HttpPost]
        public ActionResult GetDiskFreeSpaceAllowUpload()
        {
            bool result = true;

            JsonResult json = new JsonResult();
            json.Data = new
            {
                bUpload = result,
                msg = "服务器磁盘空间不足，无法继续上传"
            };

            


            return json;
        }

        public ActionResult Upload()
        {

            ViewBag.ftpHost = @System.Configuration.ConfigurationManager.AppSettings["FtpHost"];
            ViewBag.ftpPort = @System.Configuration.ConfigurationManager.AppSettings["FtpPort"];
            ViewBag.ftpUser = @System.Configuration.ConfigurationManager.AppSettings["FtpUser"];
            ViewBag.ftpPswd = @System.Configuration.ConfigurationManager.AppSettings["FtpPwd"];
            ViewBag.userId = @System.Web.HttpContext.Current.Request.Cookies["User"]["UserId"];
            ViewBag.userDepId = @System.Web.HttpContext.Current.Request.Cookies["User"]["DepID"];

            return UploadView();
        }

        [HttpPost]
        public ActionResult Upload(int fileType,int uploadUserId, int uploadUserDepId, int recordUserId, int recordUserDepId,
                                    string uploadFileName, string createTime, string recordTime)
        {
            bool result = true;
            JsonResult json = new JsonResult();
            string _msg = "";

            //服务器上的文件保存根目录
            string fileSeverPath = @System.Configuration.ConfigurationManager.AppSettings["FtpRootDir"];

            //文件截图尺寸
            string imgSize = @System.Configuration.ConfigurationManager.AppSettings["CutImageSize"];
            //文件查看尺寸
            string imgViewSize = @System.Configuration.ConfigurationManager.AppSettings["ViewImageSize"];

            //生成的jpg文件名,DB中用,最后生成的文件名类似于/FTP/1/1/123_160x160.jpg
            string ImgShowPath = "/" + @System.Configuration.ConfigurationManager.AppSettings["FtpDirName"] + uploadFileName;               //相对路径地址
            ImgShowPath = Path.ChangeExtension(ImgShowPath, "jpg");
            ImgShowPath = ImgShowPath.Replace(".jpg", "") + "_" + imgSize + ".jpg";
            //生成的jpg文件名，截图用，物理地址
            string jpgTrueFileName = fileSeverPath + uploadFileName;    //物理地址
            jpgTrueFileName = Path.ChangeExtension(jpgTrueFileName, "jpg");
            jpgTrueFileName = jpgTrueFileName.Replace(".jpg", "") + "_" + imgSize + ".jpg";


            //图片的查看地址，比预览图大，比真实图小
            string ImgPlayPath = "/" + @System.Configuration.ConfigurationManager.AppSettings["FtpDirName"] + uploadFileName;
            ImgPlayPath = Path.ChangeExtension(ImgPlayPath, "jpg");
            ImgPlayPath = ImgPlayPath.Replace(".jpg", "") + "_" + imgViewSize + ".jpg";
            //生成的jpg文件名，查看图片用，物理地址
            string ImgPlayTruePath = fileSeverPath + uploadFileName;    //物理地址
            ImgPlayTruePath = Path.ChangeExtension(ImgPlayTruePath, "jpg");
            ImgPlayTruePath = ImgPlayTruePath.Replace(".jpg", "") + "_" + imgViewSize + ".jpg";

            



            if (fileType != (int)FileType.WAV && fileType != (int)FileType.JPG)             //截图
                DevTools.CatchImg(fileSeverPath + uploadFileName, jpgTrueFileName, imgSize);
            else if (fileType == (int)FileType.JPG)                                         
            {
                DevTools.GetSmallImage(fileSeverPath + uploadFileName, jpgTrueFileName, imgSize);//图片缩小
                DevTools.GetSmallImage(fileSeverPath + uploadFileName, ImgPlayTruePath, imgViewSize);//图片查看
            }

            try
            {
                MyCMS.Models.FileModel _uploadFile = new FileModel();

                _uploadFile.UploadFileType = fileType;                  //上传文件的类型，0:avi，详细见DB设计
                _uploadFile.UploadUserId = uploadUserId;                //上传用户的id
                _uploadFile.UploadUserDepartmentId = uploadUserDepId;   //上传用户的部门id
                _uploadFile.RecordUserId = recordUserId;                //录制人id
                _uploadFile.RecordUserDepartmentId = recordUserDepId;   //录制人部门
                _uploadFile.UploadFileName = uploadFileName;            //上传文件名(包括FTP路径名 如/1/1/123.mp4)
                _uploadFile.CreateTime = Convert.ToDateTime(createTime);//文件创建时间
                _uploadFile.RecordTime = Convert.ToDateTime(recordTime);//文件修改时间

                if (fileType != (int)FileType.WAV)//wav文件没有截图
                    _uploadFile.ImageShowPath = ImgShowPath;                //截图文件路径

                _uploadFile.FileImportance = 0;                         //文件重要性
                _uploadFile.FileState = 0;                              //文件状态
                _uploadFile.FileSize = DevTools.GetFileSize(fileSeverPath + uploadFileName);//文件的大小
                if (fileType != (int)FileType.JPG)//如果是MP4，则设置播放路径，一般是 /ftp/1/1/20130202/111.mp4
                {
                    _uploadFile.PlayPath = "/" + @System.Configuration.ConfigurationManager.AppSettings["FtpDirName"] + uploadFileName;
                }
                else
                {
                    _uploadFile.PlayPath = ImgPlayPath;
                }

                _uploadFile.RealPath = @System.Configuration.ConfigurationManager.AppSettings["ServerIP"] + ":" +
                    @System.Configuration.ConfigurationManager.AppSettings["ServerPort"];//服务器ip，分布式访问用

                _uploadFile.UploadTime = DevTools.GetNowDateTime();     //上传的时间
                _uploadFile.UploadUserIP = DevTools.GetClientIP();      //上传人IP


                db.FileModels.Add(_uploadFile);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                result = false;
                _msg = ex.Message;
                
            }


            json.Data = new
            {
                bUpload = result,
                msg = "上传数据入库失败，原因是：" + _msg
            };



            return json;
        }

        //
        // GET: /File/Details/5

        public ActionResult Details(int id = 0)
        {
            FileModel filemodel = db.FileModels.Find(id);
            if (filemodel == null)
            {
                return HttpNotFound();
            }
            return View(filemodel);
        }

        //
        // GET: /File/Create

        public ActionResult Create()
        {
            return UploadView();
        }

        //
        // POST: /File/Create

        [HttpPost]
        public ActionResult Create(FileModel filemodel)
        {
            if (ModelState.IsValid)
            {
                db.FileModels.Add(filemodel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(filemodel);
        }

        //
        // GET: /File/Edit/5

        public ActionResult Edit(int id = 0)
        {
            FileModel filemodel = db.FileModels.Find(id);
            if (filemodel == null)
            {
                return HttpNotFound();
            }
            return View(filemodel);
        }

        //
        // POST: /File/Edit/5

        [HttpPost]
        public ActionResult Edit(FileModel filemodel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(filemodel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(filemodel);
        }

        //
        // GET: /File/Delete/5

        public ActionResult Delete(int id = 0)
        {
            FileModel filemodel = db.FileModels.Find(id);
            if (filemodel == null)
            {
                return HttpNotFound();
            }
            return View(filemodel);
        }

        //
        // POST: /File/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            FileModel filemodel = db.FileModels.Find(id);
            db.FileModels.Remove(filemodel);
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