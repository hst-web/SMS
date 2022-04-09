using System;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Web.Configuration;
using HST.Utillity;
using System.Web.Helpers;

namespace HST.Art.Web.Areas.manage.Controllers
{
    public class UploadController : ApplicationBase
    {
        // GET: TestUpload
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="Suffix">0:image 1:otherFiles</param>
        /// <param name="guid">文件的GUID</param>
        /// <param name="chunk">当前片</param>
        /// <param name="chunks">总片数</param>
        /// <param name="file">文件</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Upload(int Suffix, string guid, HttpPostedFileBase file, int chunk = 0, int chunks = 0, int userid = 0)
        {
            userid = Convert.ToInt16(GetAccount().Id);
            string locAddr = WebConfigurationManager.AppSettings["FileAddr"].ToString();//图片上传地址

            UploadViewModel um_mod = new UploadViewModel();
            if (file == null || file.ContentLength == 0)
                um_mod.Message = "情选择文件";
            else if (!IsAllowedType((AllowedSuffix)Suffix, Path.GetExtension(file.FileName)))
                um_mod.Message = "该类型不允许上传";
            else
            {
                //图片缩率图
                string thumbFileName = string.Empty;
                //文件后缀
                string Suffixstr = Path.GetExtension(file.FileName);
                um_mod.Extension = Suffixstr;
                string fileNameGuid = Guid.NewGuid().ToString("N");
                //生成新文件名称
                string localFileName = "art_" + userid + "_" + fileNameGuid + Suffixstr;

                if (IsAllowedType(AllowedSuffix.IMG, Suffixstr))
                {
                    thumbFileName = string.Format("small_art_{0}_{1}", userid, fileNameGuid + Suffixstr);
                }

                //相对路径
                string filePath = string.Format("/uploadFiles/{0}/", DateTime.Now.ToString("yyyyMM"));
                //文件需要存储的路径
                string localPath = string.IsNullOrEmpty(locAddr) ? System.Web.Hosting.HostingEnvironment.MapPath(filePath) : locAddr + filePath;
                if (!Directory.Exists(localPath))
                    Directory.CreateDirectory(localPath);
                //文件全路径
                string fullPath = localPath + localFileName;

                #region 分片上传功能
                //临时文件路径
                string tempPath = localPath + "/temp_" + guid + "/";
                //如果文件分片，那么路径就需要转换为临时文件存储的路径
                if (chunks > 1)
                {
                    if (!Directory.Exists(tempPath))
                        Directory.CreateDirectory(tempPath);
                    fullPath = tempPath + GetFileTopStr(chunk) + "_" + file.FileName;
                }
                try
                {
                    um_mod.FilePath = filePath + localFileName;
                    //储存文件
                    file.SaveAs(fullPath);
                    //返回是否上传成功
                    um_mod.IsSuccess = true;
                }
                catch (Exception ex)
                {
                    um_mod.Message = ex.Message;
                }
                //判断是否切分过图片
                if (chunks > 1)
                {
                    // 获取该目录下所有的切片
                    string[] fileList = Directory.GetFileSystemEntries(tempPath);
                    //如果该目录下的文件数量=总片数量，那么就进行合成并且删除无用文件
                    if (fileList.Length == chunks)
                    {
                        try
                        {
                            um_mod.FilePath = filePath + localFileName;
                            //合并文件并生成新的文件
                            CombineFile(fileList, localPath + localFileName);
                            //删除临时文件
                            DeleteFolder(tempPath);

                            //返回是否上传成功
                            um_mod.IsSuccess = true;
                        }
                        catch (Exception ex)
                        {
                            um_mod.Message = ex.Message;
                        }
                    }
                }
                #endregion

                if (um_mod.IsSuccess)
                {
                    //返回源文件名称
                    um_mod.FileName = file.FileName;
                    //返回文件GUID名称
                    um_mod.FileGuidName = localFileName;

                    try
                    {
                        //生成图片缩略图
                        if (!string.IsNullOrEmpty(thumbFileName) && Suffix == (int)AllowedSuffix.IMG)
                        {
                            string thumbFullPath = localPath + thumbFileName;
                            ImagHelper.MakeThumbnail(fullPath, thumbFullPath, 150, 90, ThumbnailModel.Fit);
                        }
                    }
                    catch
                    {

                    }
                }

            }

            // 额外处理逻辑，后续逻辑有变动可删除
            //if (!string.IsNullOrEmpty(um_mod.FilePath))
            //{
            //    um_mod.FilePath = um_mod.FilePath.Replace("/uploadFiles", "");
            //}

            return Json(um_mod);
        }


        [HttpPost]
        public ActionResult UploadEditer(HttpPostedFileBase upload)
        {
            string url = string.Empty;
            string errorMsg = string.Empty;
            string CKEditorFuncNum = System.Web.HttpContext.Current.Request["CKEditorFuncNum"];
            if (upload == null || upload.ContentLength == 0)
                errorMsg = "情选择文件";
            else if (!IsAllowedType(AllowedSuffix.IMG, Path.GetExtension(upload.FileName)))
                errorMsg = "只允许上传图片类型的文件";
            else if (upload.ContentLength > 1024 * 1024 * 5)
                errorMsg = "上传图片大小不可超过5M";

            if (!string.IsNullOrEmpty(errorMsg))
            {
                return Content("<script>window.parent.CKEDITOR.tools.callFunction(" + CKEditorFuncNum + ", \"" + url + "\",\"" + errorMsg + "\");</script>");
            }

            if (upload != null)
            {
                int userid = Convert.ToInt16(GetAccount().Id);
                //文件后缀
                string Suffixstr = Path.GetExtension(upload.FileName);
                string fileNameGuid = Guid.NewGuid().ToString("N");
                string localFileName = "artf_" + userid + "_" + fileNameGuid + Suffixstr;
                string locAddr = WebConfigurationManager.AppSettings["FileAddr"].ToString();//图片上传地址
                //相对路径
                string filePath = string.Format("/uploadFiles/{0}/", DateTime.Now.ToString("yyyyMM"));
                //文件需要存储的路径
                string localPath = string.IsNullOrEmpty(locAddr) ? System.Web.Hosting.HostingEnvironment.MapPath(filePath) : locAddr + filePath;
                if (!Directory.Exists(localPath))
                    Directory.CreateDirectory(localPath);
                //文件全路径
                string fullPath = localPath + localFileName;
                //guid重命名
                string fileName = System.Guid.NewGuid().ToString() + System.IO.Path.GetFileName(upload.FileName);
                //储存文件

                if (upload.ContentLength > 1024 * 500)
                {
                    ImagHelper.CompressImage(upload.InputStream, fullPath);
                }
                else
                {
                    upload.SaveAs(fullPath);
                }

                url = filePath + localFileName;
            }



            //上传成功后，我们还需要通过以下的一个脚本把图片返回到第一个tab选项
            return Content("<script>window.parent.CKEDITOR.tools.callFunction(" + CKEditorFuncNum + ", \"" + url + "\");</script>");
        }

        [HttpPost]
        public ActionResult UploadPasteImg(HttpPostedFileBase imgfile)
        {
            string url = string.Empty;
            string errorMsg = string.Empty;
            UploadViewModel um_mod = new UploadViewModel();

            if (imgfile == null || imgfile.ContentLength == 0)
                errorMsg = "情选择文件";

            if (imgfile != null)
            {
                int userid = Convert.ToInt16(GetAccount().Id);
                //文件后缀
                string Suffixstr = "." + imgfile.ContentType.Split('/')[1];
                string fileNameGuid = Guid.NewGuid().ToString("N");
                string localFileName = "artf_" + userid + "_" + fileNameGuid + Suffixstr;
                string locAddr = WebConfigurationManager.AppSettings["FileAddr"].ToString();//图片上传地址
                //相对路径
                string filePath = string.Format("/uploadFiles/{0}/", DateTime.Now.ToString("yyyyMM"));
                //文件需要存储的路径
                string localPath = string.IsNullOrEmpty(locAddr) ? System.Web.Hosting.HostingEnvironment.MapPath(filePath) : locAddr + filePath;
                if (!Directory.Exists(localPath))
                    Directory.CreateDirectory(localPath);
                //文件全路径
                string fullPath = localPath + localFileName;
                //guid重命名
                string fileName = System.Guid.NewGuid().ToString() + System.IO.Path.GetFileName(imgfile.FileName);
                //储存文件

                if (imgfile.ContentLength > 1024 * 500)
                {
                    ImagHelper.CompressImage(imgfile.InputStream, fullPath);
                }
                else
                {
                    imgfile.SaveAs(fullPath);
                }
           
                url = filePath + localFileName;
            }
            um_mod.Message = errorMsg;
            um_mod.FilePath = url;

            return Json(um_mod);
        }

        #region 方法
        /// <summary>
        /// 是否允许上传类型
        /// </summary>
        /// <param name="Suffix">枚举后缀类型</param>
        /// <param name="SuffixStr">后缀字符串  例如： .txt</param>
        /// <returns></returns>
        private bool IsAllowedType(AllowedSuffix Suffix, string SuffixStr)
        {
            bool result = false;
            string SuffixList = string.Empty;
            switch (Suffix)
            {
                case AllowedSuffix.IMG:
                    SuffixList = ".jpg,.jpeg,.gif,.bmp,.png";
                    break;
                case AllowedSuffix.File:
                    SuffixList = ".chm,.pdf,.zip,.rar,.tar,.gz,.doc,.csv,.docx,.ppt,.pptx,.xlsx,.xls,.mp4,.txt,.mp3";
                    break;
                default:
                    SuffixList = ".chm,.pdf,.zip,.rar,.tar,.gz,.doc,.docx,.csv,.ppt,.pptx,.xlsx,.xls,.mp4,.txt,.mp3,.jpg,.jpeg,.gif,.bmp,.png";
                    break;
            }
            if (!string.IsNullOrEmpty(SuffixStr))
                result = SuffixList.Contains(SuffixStr);
            return result;
        }
        /// <summary>
        /// 允许上传类型
        /// </summary>
        public enum AllowedSuffix
        {
            All,
            /// <summary>
            /// 图片格式
            /// </summary>
            IMG,
            /// <summary>
            /// 文件格式
            /// </summary>
            File
        }
        /// <summary>
        /// 获得文件前文字
        /// </summary>
        /// <param name="currIndex"></param>
        /// <returns></returns>
        private string GetFileTopStr(int currIndex)
        {
            return "art_" + (100000000 + currIndex).ToString();
        }
        /// <summary>
        /// 合并文件
        /// </summary>
        /// <param name="infileName">文件列表</param>
        /// <param name="outfileName"></param>
        private void CombineFile(String[] infileName, String outfileName)
        {
            int b;
            int n = infileName.Length;
            FileStream[] fileIn = new FileStream[infileName.Length];
            using (FileStream fileOut = new FileStream(outfileName, FileMode.Create))
            {
                for (int i = 0; i < n; i++)
                {
                    try
                    {
                        fileIn[i] = new FileStream(infileName[i], FileMode.Open);
                        while ((b = fileIn[i].ReadByte()) != -1)
                            fileOut.WriteByte((byte)b);
                    }
                    catch
                    {

                        //write log……
                    }
                    finally
                    {
                        if (fileIn[i] != null)
                            fileIn[i].Close();
                    }

                }
            }
        }
        /// <summary>
        /// 删除文件夹及其内容
        /// </summary>
        /// <param name="dir"></param>
        private void DeleteFolder(string dir)
        {
            string[] dd = Directory.GetFileSystemEntries(dir);
            foreach (string d in dd)
            {
                if (System.IO.File.Exists(d))
                {
                    FileInfo fi = new FileInfo(d);
                    if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                        fi.Attributes = FileAttributes.Normal;
                    System.IO.File.Delete(d);//直接删除其中的文件  
                }
                else
                    DeleteFolder(d);////递归删除子文件夹
            }
            if (Directory.Exists(dir))
                Directory.Delete(dir);
        }
        #endregion

    }
}