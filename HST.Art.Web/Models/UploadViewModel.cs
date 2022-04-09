using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HST.Art.Web
{
    /// <summary>
    /// 上传文件的属性类
    /// </summary>
    public class UploadViewModel
    {
        private int _id;
        private string _fileName;
        private string _fileGuidName;
        private string _filePath;
        private bool _isSuccess = false;
        private string _message;
        private string _extension;

        /// <summary>
        /// 文件表储存的ID
        /// </summary>
        public int Id
        {
            get
            {
                return _id;
            }

            set
            {
                _id = value;
            }
        }
        /// <summary>
        /// 文件名字
        /// </summary>
        public string FileName
        {
            get
            {
                return _fileName;
            }

            set
            {
                _fileName = value;
            }
        }
        /// <summary>
        /// 服务器上存储文件的名字
        /// </summary>
        public string FileGuidName
        {
            get
            {
                return _fileGuidName;
            }

            set
            {
                _fileGuidName = value;
            }
        }
        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath
        {
            get
            {
                return _filePath;
            }

            set
            {
                _filePath = value;
            }
        }
        /// <summary>
        /// 是否上传成功
        /// </summary>
        public bool IsSuccess
        {
            get
            {
                return _isSuccess;
            }

            set
            {
                _isSuccess = value;
            }
        }
        /// <summary>
        /// 失败消息
        /// </summary>
        public string Message
        {
            get
            {
                return _message;
            }

            set
            {
                _message = value;
            }
        }


        /// <summary>
        /// 扩展名
        /// </summary>
        public string Extension
        {
            get
            {
                return _extension;
            }

            set
            {
                _extension = value;
            }
        }
    }
}