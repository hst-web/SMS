﻿/*----------------------------------------------------------------

// 文件名：HashHelper.cs
// 功能描述： 字符串Hash操作类
// 创建者：sysmenu
// 创建时间：2019-3-18
//----------------------------------------------------------------*/
using System.Security.Cryptography;
using System.Text;


namespace HST.Utillity
{
    /// <summary>
    /// 字符串Hash操作类
    /// </summary>
    public static class HashHelper
    {
        /// <summary>
        /// 获取字符串的MD5哈希值
        /// </summary>
        public static string GetMd5(string value, Encoding encoding = null)
        {
           
            if (encoding == null)
            {
                encoding = Encoding.ASCII;
            }
            byte[] bytes = encoding.GetBytes(value);
            return GetMd5(bytes);
        }

        /// <summary>
        /// 获取字节数组的MD5哈希值
        /// </summary>
        public static string GetMd5(byte[] bytes)
        {
            if (bytes == null)
                return string.Empty;
            StringBuilder sb = new StringBuilder();
            MD5 hash = new MD5CryptoServiceProvider();
            bytes = hash.ComputeHash(bytes);
            foreach (byte b in bytes)
            {
                sb.AppendFormat("{0:x2}", b);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获取字符串的SHA1哈希值
        /// </summary>
        public static string GetSha1(string value)
        {
            
            StringBuilder sb = new StringBuilder();
            SHA1Managed hash = new SHA1Managed();
            byte[] bytes = hash.ComputeHash(Encoding.ASCII.GetBytes(value));
            foreach (byte b in bytes)
            {
                sb.AppendFormat("{0:x2}", b);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获取字符串的Sha256哈希值
        /// </summary>
        public static string GetSha256(string value)
        {
           
            StringBuilder sb = new StringBuilder();
            SHA256Managed hash = new SHA256Managed();
            byte[] bytes = hash.ComputeHash(Encoding.ASCII.GetBytes(value));
            foreach (byte b in bytes)
            {
                sb.AppendFormat("{0:x2}", b);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获取字符串的Sha512哈希值
        /// </summary>
        public static string GetSha512(string value)
        {
           

            StringBuilder sb = new StringBuilder();
            SHA512Managed hash = new SHA512Managed();
            byte[] bytes = hash.ComputeHash(Encoding.ASCII.GetBytes(value));
            foreach (byte b in bytes)
            {
                sb.AppendFormat("{0:x2}", b);
            }
            return sb.ToString();
        }
    }
}