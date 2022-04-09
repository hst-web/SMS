/*----------------------------------------------------------------
// 文件名：MailHelper.cs
// 功能描述： 邮件发送帮助类
// 创建者：sysmenu
// 创建时间：2019-3-18
//----------------------------------------------------------------*/
using System;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace HST.Utillity
{
    /// <summary>
    /// 邮件发送帮助类
    /// </summary>
    public  class MailHelper
    {
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="host">邮件服务器</param>
        /// <param name="strFrom">发件地址</param>
        /// <param name="strTo">收件地址</param>
        /// <param name="username">用户名</param>
        /// <param name="passwd">密码</param>
        /// <param name="subject">主题</param>
        /// <param name="body">内容</param>
        /// <param name="isAynce">异步发送</param>
        public static void SendEmail(string host, string strFrom, string strTo, string username, string passwd, string subject, string body,bool isAynce=true)
        {
            if (host == "smtp.qq.com" || host == "smtp.exmail.qq.com")
            {
                SendEmailQQ(host, strFrom, strTo, username, passwd, subject, body, isAynce);
            }
            else if (host == "smtp.gmail.com")
            {
                SendEmailGmail(strFrom, strTo, username, passwd, subject, body, isAynce);
            }
            else
            {
                SendEmailNotSSL(host, strFrom, strTo, username, passwd, subject, body, isAynce);
            }
        }

        /// <summary>
        /// 发送SMTP为非ssl的邮件 
        /// 特别说明：网易、搜狐邮箱不需要到邮箱中设置，新浪邮箱需要到邮箱中设置开启SMTP服务。
        /// </summary>
        /// <param name="host">邮件服务器</param>
        /// <param name="strFrom">发件地址</param>
        /// <param name="strTo">收件地址</param>
        /// <param name="username">用户名</param>
        /// <param name="passwd">密码</param>
        /// <param name="subject">主题</param>
        /// <param name="body">内容</param>
        private static void SendEmailNotSSL(string host, string strFrom, string strTo, string username, string passwd, string subject, string body,bool isAynce=true)
        {
            try
            {
                //确定smtp服务器地址。实例化一个Smtp客户端
                SmtpClient client = new SmtpClient(host);
                //构造一个发件人地址对象
                MailAddress from = new MailAddress(strFrom, strFrom, Encoding.UTF8);
                //构造一个收件人地址对象
                MailAddress to = new MailAddress(strTo, strTo, Encoding.UTF8);
                //构造一个Email的Message对象
                MailMessage message = new MailMessage(from, to);

                //添加邮件主题和内容
                message.Subject = subject;
                message.SubjectEncoding = Encoding.UTF8;
                message.Body = body;
                message.BodyEncoding = Encoding.UTF8;

               

                //设置邮件的信息
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                message.BodyEncoding = Encoding.UTF8;
                message.IsBodyHtml = true;

                client.EnableSsl = false;
                client.UseDefaultCredentials = false;

                //用户登陆信息
                NetworkCredential myCredentials = new NetworkCredential(username, passwd);
                client.Credentials = myCredentials;
                if (isAynce)
                {
                    //发送邮件
                    client.SendMailAsync(message);
                }
                else
                {
                    //发送邮件
                    client.Send(message);
                }
               
            }
            catch (Exception ex)
            {
                Logger.Info("MailHelper:"+ex.Message, ex);
            }
        }

        /// <summary>
        /// 发送qq邮件 
        /// </summary>
        /// <param name="host">邮件服务器：smtp.qq.com或smtp.exmail.qq.com </param>
        /// <param name="strFrom">发件地址</param>
        /// <param name="strTo">收件地址</param>
        /// <param name="username">用户名</param>
        /// <param name="passwd">
        /// 密码
        /// 特别说明：个人邮箱用授权码作为密码，企业邮箱直接用登录密码
        /// </param>
        /// <param name="subject">主题</param>
        /// <param name="body">内容</param>
        private static void SendEmailQQ(string host, string strFrom, string strTo, string username, string passwd, string subject, string body,bool isAynce=true)
        {
           //System.Web.Mail.MailMessage mail = new System.Web.Mail.MailMessage();
           // try
           // {
           //     mail.To = strTo;
           //     mail.From = strFrom;
           //     mail.Subject = subject;
           //     mail.BodyFormat = System.Web.Mail.MailFormat.Html;
           //     mail.Body = body;

           //     mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", "1"); //身份验证  
           //     mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", mail.From); //邮箱登录账号，这里跟前面的发送账号一样就行  
           //     mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", passwd); //这个密码要注意：如果是一般账号，要用授权码；企业账号用登录密码  
           //     mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserverport", 465);//端口  
           //     mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpusessl", "true");//SSL加密  
           //     System.Web.Mail.SmtpMail.SmtpServer = host; //邮件服务器
           //     System.Web.Mail.SmtpMail.Send(mail);
           // }
           // catch (Exception ex)
           // {
           //     Logger.Info("MailHelper:" + ex.Message, ex);
           // }
        }

        /// <summary>
        /// 发送谷歌邮件 
        /// </summary>
        /// <param name="host">邮件服务器</param>
        /// <param name="strFrom">发件地址</param>
        /// <param name="strTo">收件地址</param>
        /// <param name="username">用户名</param>
        /// <param name="passwd">密码</param>
        /// <param name="subject">主题</param>
        /// <param name="body">内容</param>
        private static void SendEmailGmail(string strFrom, string strTo, string username, string passwd, string subject, string body,bool isAsync=true)
        {
            try
            {
                //确定smtp服务器地址。实例化一个Smtp客户端
                SmtpClient client = new SmtpClient("smtp.gmail.com");
                //构造一个发件人地址对象
                MailAddress from = new MailAddress(strFrom, strFrom, Encoding.UTF8);
                //构造一个收件人地址对象
                MailAddress to = new MailAddress(strTo, strTo, Encoding.UTF8);
                //构造一个Email的Message对象
                MailMessage message = new MailMessage(from, to);

                //添加邮件主题和内容
                message.Subject = subject;
                message.SubjectEncoding = Encoding.UTF8;
                message.Body = body;
                message.BodyEncoding = Encoding.UTF8;

                //设置邮件的信息
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                message.BodyEncoding = Encoding.UTF8;
                message.IsBodyHtml = true;

                //SSL是否加密，设置Port端口
                client.UseDefaultCredentials = false;
                client.EnableSsl = true;
                client.Port = 587;

                //用户登陆信息
                NetworkCredential myCredentials = new NetworkCredential(username, passwd);
                client.Credentials = myCredentials;

                if (isAsync)
                {
                    //发送邮件
                    client.SendMailAsync(message);
                }
                else
                {
                    client.Send(message);
                }
               
            }
            catch (Exception ex)
            {
                Logger.Info("MailHelper:" + ex.Message, ex);
            }
        }
    }
}
