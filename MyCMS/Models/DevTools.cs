using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.IO;
using System.Web.Mvc;
using System.Web.Security;

namespace MyCMS.Models
{
    public static class DevTools
    {

        public static string GetIP()
        {
            string result = String.Empty;

            result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(result))
            {
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            if (string.IsNullOrEmpty(result))
            {
                result = HttpContext.Current.Request.UserHostAddress;
            }
            if (string.IsNullOrEmpty(result) || result.Equals("::1"))
            {
                return "127.0.0.1";
            }

            return result;
        }

        public static DateTime GetNowDateTime()
        {
            return new DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day,
                    System.DateTime.Now.Hour, System.DateTime.Now.Minute, System.DateTime.Now.Second);
        }

        public static void CatchImg(string fileName, string imgFile, string imgSize/* 240x120 */)
        {
            //获取截图工具路径
            string ffmpeg = @System.Configuration.ConfigurationManager.AppSettings["ffmpeg"];

            //获取截图后保存的路径
            string flv_img = imgFile;

            //获取截取图片的大小
            string FlvImgSize = imgSize;

            //sizeOfImg;

            Process pss = new Process();

            //设置启动程序的路径
            pss.StartInfo.FileName = ffmpeg;

            //后台运行
            pss.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

            pss.StartInfo.Arguments = "   -i   " + fileName + "  -y  -f  image2   -ss 1 -vframes 1  -s   " + FlvImgSize + "   " + flv_img;

            //启动进程
            pss.Start();

            //等待进程结束
            while (!pss.HasExited)
            { }
        }

        //转换电影

        #region //运行FFMpeg的视频解码，(这里是绝对路径)
        /// <summary>
        /// 转换文件并保存在指定文件夹下面(这里是绝对路径)
        /// </summary>
        /// <param name="fileName">上传视频文件的路径（原文件）</param>
        /// <param name="playFile">转换后的文件的路径（网络播放文件）</param>
        /// <param name="imgFile">从视频文件中抓取的图片路径</param>
        /// /// <param name="windowSize">转码后的视频文件尺寸，格式 "640x480"</param>
        /// <returns>成功:返回图片虚拟地址;   失败:返回空字符串</returns>
        public static string ChangeFileType(string fileName, string playFile, string windowSize, string imgFile, string imgSize)
        {


            //取得ffmpeg.exe的路径,路径配置在Web.Config中,如:<add   key="ffmpeg"   value="E:\51aspx\ffmpeg.exe"   />   

            DateTime start = DateTime.Now;

            string ffmpeg = @System.Configuration.ConfigurationManager.AppSettings["ffmpeg"];

            //ConfigurationManager.AppSettings["ffmpeg"];

            string Name = System.Web.HttpContext.Current.Server.MapPath("../")  + "" + fileName;

            if ((!System.IO.File.Exists(ffmpeg)) || (!System.IO.File.Exists(Name)))
            {

                return "";

            }

            //获得图片和(.flv)文件相对路径/最后存储到数据库的路径,如:/Web/User1/00001.jpg   

            string flv_file = System.IO.Path.ChangeExtension(@"D:\Y2项目\AxSchool\AxSchool\AxSchool\Content\upload\Video\" + fileName, ".flv");

            //D:\Y2项目\AxSchool\AxSchool\AxSchool\Content\IAndVImages\baijunjie\

            //截图的尺寸大小,配置在Web.Config中,如:<add   key="CatchFlvImgSize"   value="240x180"   />   

            string FlvImgSize = imgSize;

            System.Diagnostics.Process FilesProcess = new System.Diagnostics.Process();

            FilesProcess.StartInfo.FileName = ffmpeg;

            FilesProcess.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

            FilesProcess.StartInfo.Arguments = " -i " + Name + " -ab 56 -ar 22050 -b 500 -r 30 -s " + windowSize + " " + flv_file;

            try
            {

                FilesProcess.Start();

                while (!FilesProcess.HasExited)
                {

                }

            }

            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);

            }

            return "";

        }
        #endregion

        #region //获得文件的大小，单位：字节
        public static long GetFileSize(string filename)
        {
            FileInfo fo = new FileInfo(filename);
            return fo.Length;
        }
        #endregion
    }
}