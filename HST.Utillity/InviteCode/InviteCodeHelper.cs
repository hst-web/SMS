/*----------------------------------------------------------------

// 文件名：InviteCodeHelper.cs
// 功能描述：邀请码辅助操作类
// 创建者：sysmenu
// 创建时间：2019-3-18
//----------------------------------------------------------------*/
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.VisualBasic.FileIO;
using System;

namespace HST.Utillity
{
    /// <summary>
    /// 邀请码辅助操作类
    /// </summary>
    public class InviteCodeHelper
    {
        //0-9 a-z A-Z 正好62位。将时间MMddHHmmss转为5位邀请码
        private static string[] sourceString = 
            {"A","0","b","C","B","1","c","2","a","D",
             "d","E","4","f","e","F","5","J","3","j",
             "L","M","N","O","m","n","o","P","l","p",
             "6","h","I","8","7","i","K","k","H","9",
             "Q","R","S","T","r","s","t", "V","w","W",
             "U","u","X","Y","q","x","y","Z","v","z" };

        /// <summary>
        /// 生成邀请码
        /// </summary>
        /// <returns></returns>
        public static string Create()
        {
            StringBuilder code = new StringBuilder ();
            string currentDate = DateTime.Now.ToString("MMddHHmmss");

            //将当前时间戳转为5位邀请码
            code.Append(sourceString[Convert.ToInt32(currentDate.Substring(0, 2))]);//第一位
            code.Append(sourceString[Convert.ToInt32(currentDate.Substring(2, 2))]);//第二位
            code.Append(sourceString[Convert.ToInt32(currentDate.Substring(4, 2))]);//第三位
            code.Append(sourceString[Convert.ToInt32(currentDate.Substring(6, 2))]);//第四位
            code.Append(sourceString[Convert.ToInt32(currentDate.Substring(8, 2))]);//第五位

            return code.ToString();
            //DesHelper des = new DesHelper();
            //return des.Encrypt(code.ToString());
        }

        /// <summary>
        /// 邀请码是否合法
        /// </summary>
        /// <param name="code">邀请码</param>
        /// <param name="hours">有效小时数</param>
        /// <returns></returns>
        public static bool IsValid(string code,int hours=24)
        {
            //DesHelper des = new DesHelper();
            //code=des.Decrypt(code);
            if (sourceString.Length != 5)
            {
                return false;
            }
            
            int MM=0 ,dd=0,HH=0,mm=0,ss=0;//月日时分秒
            for (int i = 0; i < sourceString.Length; i++)
            {
                if (sourceString[i] == code.Substring(0, 1))
                {
                    MM = i;//月
                }
                if (sourceString[i] == code.Substring(1, 1))
                {
                    dd = i;//日
                }
                if (sourceString[i] == code.Substring(2, 1))
                {
                    HH = i;//小时
                }
                if (sourceString[i] == code.Substring(3, 1))
                {
                    mm = i;//分
                }
                if (sourceString[i] == code.Substring(4, 1))
                {
                    ss = i;//秒
                }
            }

            //获取邀请码中的时间戳
            DateTime dtTime = DateTime.Parse(string.Format("{0}-{1}-{2} {3}:{4}:{5}", DateTime.Now.ToString("yyyy"), MM.ToString("00"), dd.ToString("00"), HH.ToString("00"), mm.ToString("00"), ss.ToString("00")));

            //时间戳是否在有效时间之内
            return DateTime.Now.Subtract(dtTime).TotalHours < hours;
            
        }

    }
}