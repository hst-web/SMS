/*----------------------------------------------------------------

// 文件名：ServiceBase.cs
// 功能描述： 服务基类
// 创建者：sysmenu
// 创建时间：2015-9-18
//----------------------------------------------------------------*/
using HST.Art.Core;
using HST.Utillity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Web;

namespace HST.Art.Service
{
    /// <summary>
    /// 服务基类
    /// </summary>
    public class ServiceBase : IDisposable
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ServiceBase()
        {
            DisposableObjects = new List<IDisposable>();

        }

        /// <summary>
        /// 包含可释放的实体集合
        /// </summary>
        public IList<IDisposable> DisposableObjects { get; private set; }

        /// <summary>
        /// 添加实体对象到集合
        /// </summary>
        /// <param name="obj"></param>
        protected void AddDisposableObject(object obj)
        {
            IDisposable disposable = obj as IDisposable;
            if (null != disposable)
            {
                this.DisposableObjects.Add(disposable);
            }
        }

        /// <summary>
        /// 销毁时释放包含的实体
        /// </summary>
        public void Dispose()
        {
            foreach (IDisposable obj in this.DisposableObjects)
            {
                if (null != obj)
                {
                    obj.Dispose();
                }
            }
        }
        /// <summary>
        /// 错误信息
        /// </summary>
        public virtual ErrorCode ErrorMsg { get; set; }

        /// <summary>
        /// 处理（简介）
        /// </summary>
        public string DisposeHtmlStr(string description, string synopsis = "")
        {
            if (string.IsNullOrWhiteSpace(description)) return string.Empty;
            if (!string.IsNullOrWhiteSpace(synopsis)) return string.Empty;


            //删除脚本
            description = Regex.Replace(description, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);

            //删除HTML
            description = Regex.Replace(description, "<[^>]+>", "", RegexOptions.Singleline);
            description = Regex.Replace(description, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            description = Regex.Replace(description, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            description = Regex.Replace(description, @"-->", "", RegexOptions.IgnoreCase);
            description = Regex.Replace(description, @"<!--.*", "", RegexOptions.IgnoreCase);
            description = Regex.Replace(description, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            description = Regex.Replace(description, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            description = Regex.Replace(description, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            description = Regex.Replace(description, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            description = Regex.Replace(description, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            description = Regex.Replace(description, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            description = Regex.Replace(description, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            description = Regex.Replace(description, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            description = Regex.Replace(description, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            description = Regex.Replace(description, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            description = Regex.Replace(description, @"<img[^>]*>;", "", RegexOptions.IgnoreCase);
            description.Replace("<", "");
            description.Replace(">", "");
            description.Replace("\r\n", "");
            //System
            // description = HttpServerUtility.HtmlEncode(description).Trim();
            return GetLength(description.Trim(), 50);
        }

        /// <summary>
        /// 获取设定长度的字符串
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="length">截取长度</param>
        /// <returns></returns>
        private string GetLength(string str, int length)
        {
            if (string.IsNullOrWhiteSpace(str)) return string.Empty;
            string strR = str;
            if (str.Length > length)
            {
                strR = str.Substring(0, length).TrimEnd(',')+"...";
            }
            return strR;
        }
    }
}
