using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Drawing;

namespace MyCMS.Models
{


    /// <summary>
    /// ImageHelper 的摘要说明
    /// </summary>
    public class ImageHelper
    {
        public ImageHelper()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }
        #region Image水印
        /// <summary>
        /// 写入图像水印
        /// </summary>
        /// <param name="str">水印字符串</param>
        /// <param name="filePath">原图片位置</param>
        /// <param name="savePath">水印加入后的位置</param>
        /// <returns></returns>
        public string CreateBackImage(System.Web.UI.Page pageCurrent, string str, string filePath, string savePath, int x, int y)
        {
            System.Drawing.Image img = System.Drawing.Image.FromFile(pageCurrent.MapPath(filePath));
            //创建图片
            Graphics graphics = Graphics.FromImage(img);
            //指定要绘制的面积
            graphics.DrawImage(img, 0, 0, img.Width, img.Height);
            //定义字段和画笔
            Font font = new Font("黑体", 16);
            Brush brush = new SolidBrush(Color.Yellow);
            graphics.DrawString(str, font, brush, x, y);
            //保存并输出图片
            img.Save(pageCurrent.MapPath(savePath), System.Drawing.Imaging.ImageFormat.Jpeg);
            return savePath;

        }
        #endregion
        #region Image自动缩小
        /// <summary>
        /// 缩小图片到指定的大小
        /// </summary>
        /// <param name="strOldPic">
        /// 原图片的位置
        /// </param>
        /// <param name="strNewPic">
        /// 缩小后的图片位置
        /// </param>
        /// <param name="intWidth">
        /// 宽度
        /// </param>
        /// <param name="intHeight">
        /// 高度
        /// </param>
        public void SmallPic(string strOldPic, string strNewPic, int intWidth, int intHeight)
        {

            System.Drawing.Bitmap objPic, objNewPic;
            try
            {
                objPic = new System.Drawing.Bitmap(strOldPic);
                objNewPic = new System.Drawing.Bitmap(objPic, intWidth, intHeight);
                objNewPic.Save(strNewPic);

            }
            catch (Exception exp) { throw exp; }
            finally
            {
                objPic = null;
                objNewPic = null;
            }
        }


        public void SmallPic(string strOldPic, string strNewPic, string imgSize)
        {

            int width  = int.Parse(imgSize.Split('x')[0]);
            int height = int.Parse(imgSize.Split('x')[1]);
            SmallPic(strOldPic, strNewPic, width, height);
        }

        public void SmallPic(string strOldPic, string strNewPic, int intWidth)
        {

            System.Drawing.Bitmap objPic, objNewPic;
            try
            {
                objPic = new System.Drawing.Bitmap(strOldPic);
                int intHeight = Convert.ToInt32(((intWidth * 1.0) / (objPic.Width * 1.0)) * objPic.Height);
                objNewPic = new System.Drawing.Bitmap(objPic, intWidth, intHeight);
                objNewPic.Save(strNewPic, objPic.RawFormat);

            }
            catch (Exception exp) { throw exp; }
            finally
            {
                objPic = null;
                objNewPic = null;
            }
        }

        //public void SmallPic(string strOldPic, string strNewPic, int intHeight)
        //{

        //    System.Drawing.Bitmap objPic, objNewPic;
        //    try
        //    {
        //        objPic = new System.Drawing.Bitmap(strOldPic);
        //        int intWidth = Convert.ToInt32(((intHeight * 1.0) / objPic.Height) * objPic.Width);
        //        objNewPic = new System.Drawing.Bitmap(objPic, intWidth, intHeight);
        //        objNewPic.Save(strNewPic, objPic.RawFormat);

        //    }
        //    catch (Exception exp) { throw exp; }
        //    finally
        //    {
        //        objPic = null;
        //        objNewPic = null;
        //    }
        //}
        #endregion


    }
}