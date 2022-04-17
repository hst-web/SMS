using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ZT.SMS.Core;
using ZT.Utillity;

namespace ZT.SMS.Web
{
    public class MsgViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "订单编号不能为空")]
        [RegularExpression(@"^[^\s]+$", ErrorMessage = "订单编号不能包含空字符")]
        [Remote("CheckMsgNumber", "MessageRecord", AdditionalFields = "Id", ErrorMessage = "该订单编号已经存在")]
        public string Number { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "手机号不能为空")]
        [RegularExpression(@"^(13[0-9]|14[01456879]|15[0-35-9]|16[2567]|17[0-8]|18[0-9]|19[0-35-9])\d{8}$", ErrorMessage = "手机号格式不正确")]
        public string Phone { get; set; } // phone
        public MsgSendState State { get; set; }
        public string CreateTime { get; set; }
        public string SendDate { get; set; }
        public string StateName
        {
            get
            {
                switch (State)
                {
                    case MsgSendState.SendFailed:
                        return "发送失败";
                    case MsgSendState.SendSuccess:
                        return "发送成功";
                }

                return "未发送";
            }
        }
        [Required(AllowEmptyStrings = false, ErrorMessage = "订单日期不能为空")]
        public string OrderDate { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "商品名称不能为空")]
        public string OrderName { get; set; }
        public string Remark { get; set; }
    }
}