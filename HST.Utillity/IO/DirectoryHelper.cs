﻿/*----------------------------------------------------------------

// 文件名：DirectoryHelper.cs
// 功能描述：目录操作辅助类
// 创建者：sysmenu
// 创建时间：2019-3-18
//----------------------------------------------------------------*/
using System;
using System.IO;


namespace ZT.Utillity
{
    /// <summary>
    /// 目录操作辅助类
    /// </summary>
    public static class DirectoryHelper
    {
        /// <summary>
        /// 递归复制文件夹及文件夹/文件
        /// </summary>
        /// <param name="sourcePath"> 源文件夹路径 </param>
        /// <param name="targetPath"> 目的文件夹路径 </param>
        /// <param name="searchPatterns"> 要复制的文件扩展名数组 </param>
        public static void Copy(string sourcePath, string targetPath, string[] searchPatterns = null)
        {
            if (string.IsNullOrEmpty(sourcePath) || string.IsNullOrEmpty(targetPath))
                return;

            if (!Directory.Exists(sourcePath))
            {
                throw new DirectoryNotFoundException("递归复制文件夹时源目录不存在。");
            }
            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
            }
            string[] dirs = Directory.GetDirectories(sourcePath);
            if (dirs.Length > 0)
            {
                foreach (string dir in dirs)
                {
                    Copy(dir, targetPath + targetPath + dir.Substring(dir.LastIndexOf("\\", StringComparison.Ordinal)));
                }
            }
            if (searchPatterns != null && searchPatterns.Length > 0)
            {
                foreach (string searchPattern in searchPatterns)
                {
                    string[] files = Directory.GetFiles(sourcePath, searchPattern);
                    if (files.Length <= 0)
                    {
                        continue;
                    }
                    foreach (string file in files)
                    {
                        File.Copy(file, targetPath + file.Substring(file.LastIndexOf("\\", StringComparison.Ordinal)));
                    }
                }
            }
            else
            {
                string[] files = Directory.GetFiles(sourcePath);
                if (files.Length <= 0)
                {
                    return;
                }
                foreach (string file in files)
                {
                    File.Copy(file, targetPath + file.Substring(file.LastIndexOf("\\", StringComparison.Ordinal)));
                }
            }
        }

        /// <summary>
        /// 递归删除目录
        /// </summary>
        /// <param name="directory"> 目录路径 </param>
        /// <param name="isDeleteRoot"> 是否删除根目录 </param>
        /// <returns> 是否成功 </returns>
        public static bool Delete(string directory, bool isDeleteRoot = true)
        {
            if (string.IsNullOrEmpty(directory))
                return false;

            bool flag = false;
            DirectoryInfo dirPathInfo = new DirectoryInfo(directory);
            if (dirPathInfo.Exists)
            {
                //删除目录下所有文件
                foreach (FileInfo fileInfo in dirPathInfo.GetFiles())
                {
                    fileInfo.Delete();
                }
                //递归删除所有子目录
                foreach (DirectoryInfo subDirectory in dirPathInfo.GetDirectories())
                {
                    Delete(subDirectory.FullName);
                }
                //删除目录
                if (isDeleteRoot)
                {
                    dirPathInfo.Attributes = FileAttributes.Normal;
                    dirPathInfo.Delete();
                }
                flag = true;
            }
            return flag;
        }

        /// <summary>
        /// 设置或取消目录的<see cref="FileAttributes"/>属性。
        /// </summary>
        /// <param name="directory">目录路径</param>
        /// <param name="attribute">要设置的目录属性</param>
        /// <param name="isSet">true为设置，false为取消</param>
        public static void SetAttributes(string directory, FileAttributes attribute, bool isSet)
        {
            if (string.IsNullOrEmpty(directory))
                return;
            DirectoryInfo di = new DirectoryInfo(directory);
            if (!di.Exists)
            {
                throw new DirectoryNotFoundException("设置目录属性时指定文件夹不存在");
            }
            if (isSet)
            {
                di.Attributes = di.Attributes | attribute;
            }
            else
            {
                di.Attributes = di.Attributes & ~attribute;
            }
        }
    }
}