﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace ASP
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Web;
    using System.Web.Helpers;
    using System.Web.Mvc;
    using System.Web.Mvc.Ajax;
    using System.Web.Mvc.Html;
    using System.Web.Optimization;
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    
    #line 1 "..\..\Areas\manage\Views\Home\Index.cshtml"
    using ZT.SMS.Core;
    
    #line default
    #line hidden
    using ZT.SMS.Web;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/manage/Views/Home/Index.cshtml")]
    public partial class _Areas_manage_Views_Home_Index_cshtml : System.Web.Mvc.WebViewPage<Account>
    {
        public _Areas_manage_Views_Home_Index_cshtml()
        {
        }
        public override void Execute()
        {
            
            #line 4 "..\..\Areas\manage\Views\Home\Index.cshtml"
  
    ViewBag.Title = "网站后台管理";

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n<link");

WriteLiteral(" rel=\"stylesheet\"");

WriteLiteral(" type=\"text/css\"");

WriteLiteral(" href=\"/Content/lib/ali-iconfont/iconfont.css\"");

WriteLiteral("/>\r\n<link");

WriteLiteral(" rel=\"stylesheet\"");

WriteLiteral(" type=\"text/css\"");

WriteLiteral(" href=\"/Content/lib/h-ui.admin/skin/default/skin.css\"");

WriteLiteral(" id=\"skin\"");

WriteLiteral(" />\r\n<header");

WriteLiteral(" class=\"navbar-wrapper\"");

WriteLiteral(">\r\n    <div");

WriteLiteral(" class=\"navbar navbar-fixed-top\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" class=\"container-fluid cl\"");

WriteLiteral(">\r\n            <span");

WriteLiteral(" style=\"padding-left:20px;\"");

WriteLiteral("><a");

WriteLiteral(" class=\"logo navbar-logo f-l mr-10\"");

WriteLiteral(" href=\"\"");

WriteLiteral(">网站后台管理</a></span>\r\n            <span");

WriteLiteral(" class=\"logo navbar-slogan f-l mr-10\"");

WriteLiteral("></span>\r\n            <a");

WriteLiteral(" aria-hidden=\"false\"");

WriteLiteral(" class=\"nav-toggle Hui-iconfont visible-xs\"");

WriteLiteral(" href=\"javascript:;\"");

WriteLiteral(">&#xe667;</a>\r\n            <a");

WriteLiteral(" aria-hidden=\"false\"");

WriteLiteral(" class=\"nav-toggle Hui-iconfont visible-xs\"");

WriteLiteral(" href=\"/Account/LoginOut\"");

WriteLiteral(" style=\"right:60px;\"");

WriteLiteral(">&#xe726;</a>\r\n            <nav");

WriteLiteral(" id=\"Hui-userbar\"");

WriteLiteral(" class=\"nav navbar-nav navbar-userbar \"");

WriteLiteral(">\r\n                <ul");

WriteLiteral(" class=\"cl\"");

WriteLiteral(">\r\n                    <li");

WriteLiteral(" class=\"hidden-xs\"");

WriteLiteral(">");

            
            #line 19 "..\..\Areas\manage\Views\Home\Index.cshtml"
                                     Write(Model.UserName);

            
            #line default
            #line hidden
WriteLiteral("&nbsp;&nbsp;");

            
            #line 19 "..\..\Areas\manage\Views\Home\Index.cshtml"
                                                                 Write(Model.IsAdmin?"超级管理员":"管理员");

            
            #line default
            #line hidden
WriteLiteral("</li>\r\n                    <li");

WriteLiteral(" class=\"dropDown right dropDown_hover\"");

WriteLiteral(">\r\n                        <a");

WriteLiteral(" href=\"#\"");

WriteLiteral(" class=\"dropDown_A\"");

WriteLiteral(">管理 <i");

WriteLiteral(" class=\"Hui-iconfont\"");

WriteLiteral(">&#xe6d5;</i></a>\r\n                        <ul");

WriteLiteral(" class=\"dropDown-menu menu radius box-shadow\"");

WriteLiteral(">\r\n                            <li><a");

WriteLiteral(" href=\"javascript:void(0)\"");

WriteLiteral(" onclick=\"user_show()\"");

WriteLiteral(">个人信息</a></li>\r\n                            <li><a");

WriteAttribute("href", Tuple.Create(" href=\"", 1398), Tuple.Create("\"", 1438)
            
            #line 24 "..\..\Areas\manage\Views\Home\Index.cshtml"
, Tuple.Create(Tuple.Create("", 1405), Tuple.Create<System.Object, System.Int32>(Url.Action("LoginOut","Account")
            
            #line default
            #line hidden
, 1405), false)
);

WriteLiteral(" data-title=\"退出\"");

WriteLiteral(">退出</a></li>\r\n                        </ul>\r\n                    </li>\r\n         " +
"       </ul>\r\n            </nav>\r\n        </div>\r\n    </div>\r\n</header>\r\n<aside");

WriteLiteral(" class=\"Hui-aside\"");

WriteLiteral(" id=\"main-aside\"");

WriteLiteral(">\r\n    <div");

WriteLiteral(" class=\"menu_dropdown bk_2\"");

WriteLiteral(">\r\n\r\n        <dl");

WriteLiteral(" class=\"single-dt\"");

WriteLiteral(">\r\n            ");

WriteLiteral("\r\n            <dt");

WriteLiteral(" class=\"selected\"");

WriteLiteral("><a");

WriteLiteral(" data-href=\"");

            
            #line 37 "..\..\Areas\manage\Views\Home\Index.cshtml"
                                          Write(Url.Action("List", "MessageRecord"));

            
            #line default
            #line hidden
WriteLiteral("\"");

WriteLiteral(" data-title=\"短信箱\"");

WriteLiteral(" href=\"javascript:void(0)\"");

WriteLiteral("><i");

WriteLiteral(" class=\"iconfont icon-danwei\"");

WriteLiteral("></i>短信箱</a></dt>           \r\n        </dl>\r\n        <dl");

WriteLiteral(" class=\"list-dt\"");

WriteLiteral(">\r\n            <dt");

WriteLiteral(" class=\"\"");

WriteLiteral("><i");

WriteLiteral(" class=\"iconfont icon-xitong\"");

WriteLiteral("></i>系统设置<i");

WriteLiteral(" class=\"Hui-iconfont menu_dropdown-arrow\"");

WriteLiteral(">&#xe6d5;</i></dt>\r\n            <dd>\r\n                <ul>\r\n                    ");

WriteLiteral("\r\n                    <li><a");

WriteLiteral(" data-href=\"");

            
            #line 44 "..\..\Areas\manage\Views\Home\Index.cshtml"
                                 Write(Url.Action("List", "User"));

            
            #line default
            #line hidden
WriteLiteral("\"");

WriteLiteral(" data-title=\"账号管理\"");

WriteLiteral(" href=\"javascript:void(0)\"");

WriteLiteral(">账号管理</a></li>\r\n                </ul>\r\n            </dd>\r\n        </dl>\r\n\r\n      " +
"  <dl");

WriteLiteral(" class=\"hidden\"");

WriteLiteral(">\r\n            <dd><ul><li");

WriteLiteral(" class=\"hidden\"");

WriteLiteral("><a");

WriteLiteral(" data-href=\"");

            
            #line 50 "..\..\Areas\manage\Views\Home\Index.cshtml"
                                                Write(Url.Action("Index","Account"));

            
            #line default
            #line hidden
WriteLiteral("\"");

WriteLiteral(" href=\"javascript:void(0)\"");

WriteLiteral(" data-title=\"个人信息\"");

WriteLiteral(" id=\"userHid\"");

WriteLiteral(">个人信息</a></li></ul></dd>\r\n        </dl>\r\n    </div>\r\n</aside>\r\n<div");

WriteLiteral(" class=\"dislpayArrow hidden-xs\"");

WriteLiteral("><a");

WriteLiteral(" class=\"pngfix\"");

WriteLiteral(" href=\"javascript:void(0);\"");

WriteLiteral(" onClick=\"displaynavbar(this)\"");

WriteLiteral("></a></div>\r\n<section");

WriteLiteral(" class=\"Hui-article-box\"");

WriteLiteral(">\r\n    <div");

WriteLiteral(" id=\"Hui-tabNav\"");

WriteLiteral(" class=\"Hui-tabNav hidden-xs\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" class=\"Hui-tabNav-wp\"");

WriteLiteral(">\r\n            <ul");

WriteLiteral(" id=\"min_title_list\"");

WriteLiteral(" class=\"acrossTab cl\"");

WriteLiteral(">\r\n                <li");

WriteLiteral(" class=\"active\"");

WriteLiteral(">\r\n                    <span");

WriteLiteral(" title=\"企业信息\"");

WriteLiteral(" data-href=\"");

            
            #line 60 "..\..\Areas\manage\Views\Home\Index.cshtml"
                                             Write(Url.Action("List", "MessageRecord"));

            
            #line default
            #line hidden
WriteLiteral("\"");

WriteLiteral(">短信箱</span>\r\n                    <em></em>\r\n                </li>\r\n            </" +
"ul>\r\n        </div>\r\n        <div");

WriteLiteral(" class=\"Hui-tabNav-more btn-group\"");

WriteLiteral("><a");

WriteLiteral(" id=\"js-tabNav-prev\"");

WriteLiteral(" class=\"btn radius btn-default size-S\"");

WriteLiteral(" href=\"javascript:;\"");

WriteLiteral("><i");

WriteLiteral(" class=\"Hui-iconfont\"");

WriteLiteral(">&#xe6d4;</i></a><a");

WriteLiteral(" id=\"js-tabNav-next\"");

WriteLiteral(" class=\"btn radius btn-default size-S\"");

WriteLiteral(" href=\"javascript:;\"");

WriteLiteral("><i");

WriteLiteral(" class=\"Hui-iconfont\"");

WriteLiteral(">&#xe6d7;</i></a></div>\r\n    </div>\r\n    <div");

WriteLiteral(" id=\"iframe_box\"");

WriteLiteral(" class=\"Hui-article\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" class=\"show_iframe\"");

WriteLiteral(">\r\n            <div");

WriteLiteral(" style=\"display:none\"");

WriteLiteral(" class=\"loading\"");

WriteLiteral("></div>\r\n            <iframe");

WriteLiteral(" scrolling=\"yes\"");

WriteLiteral(" frameborder=\"0\"");

WriteAttribute("src", Tuple.Create(" src=\"", 3920), Tuple.Create("\"", 3962)
            
            #line 70 "..\..\Areas\manage\Views\Home\Index.cshtml"
, Tuple.Create(Tuple.Create("", 3926), Tuple.Create<System.Object, System.Int32>(Url.Action("List", "MessageRecord")
            
            #line default
            #line hidden
, 3926), false)
);

WriteLiteral("></iframe>\r\n        </div>\r\n    </div>\r\n</section>\r\n\r\n<div");

WriteLiteral(" class=\"contextMenu\"");

WriteLiteral(" id=\"Huiadminmenu\"");

WriteLiteral(">\r\n    <ul>\r\n        <li");

WriteLiteral(" id=\"closethis\"");

WriteLiteral(">关闭当前 </li>\r\n        <li");

WriteLiteral(" id=\"closeall\"");

WriteLiteral(">关闭全部 </li>\r\n        <li");

WriteLiteral(" id=\"refreshthis\"");

WriteLiteral(">刷新</li>\r\n    </ul>\r\n</div>\r\n<!--请在下方写此页面业务相关的脚本-->\r\n<script");

WriteLiteral(" type=\"text/javascript\"");

WriteLiteral(" src=\"/Content/lib/jquery.contextmenu/jquery.contextmenu.r2.js\"");

WriteLiteral("></script>\r\n<script");

WriteLiteral(" type=\"text/javascript\"");

WriteLiteral(">\r\n    $(function () {\r\n        $(\"#main-aside ul li\").click(function () {\r\n     " +
"       $(this).addClass(\"current\").siblings().removeClass(\"current\");\r\n         " +
"   $(this).parents(\"dl\").siblings().find(\"li\").removeClass(\"current\");\r\n        " +
"    $(this).parents(\"dl\").siblings(\"dl.single-dt\").find(\"dt\").removeClass(\"selec" +
"ted\");\r\n        })\r\n\r\n        $(\".menu_dropdown dl.single-dt dt\").click(function" +
" () {\r\n            $(this).addClass(\"selected\").siblings().removeClass(\"selected" +
"\");\r\n            $(this).parents(\"dl\").siblings().find(\"dt\").removeClass(\"select" +
"ed\");\r\n            $(this).parents(\"dl\").siblings(\"dl.list-dt\").find(\"dd\").css(\"" +
"display\", \"none\");\r\n            $(this).parents(\"dl\").siblings(\"dl.list-dt\").fin" +
"d(\"li\").removeClass(\"current\");\r\n        })\r\n\r\n    });\r\n\r\n    /*触发用户载入页面*/\r\n    " +
"function user_show() {\r\n        $(\"#userHid\").click();\r\n    }\r\n\r\n    //显示查询框\r\n  " +
"  function showDialog() {\r\n        var index = layer.open({\r\n            type: 1" +
",\r\n            title: \"日志\",\r\n            area: [\'450px\', \'220px\'],\r\n            " +
"anim: 2,\r\n            content: \"<article class=\'page-container form-horizontal\'>" +
" <div class=\'row cl\'> <label class=\'form-label col-xs-3\' style=\'padding-right:0p" +
"x\'>查询日期：</label><div class=\'formControls col-xs-8 \'><input id=\'SearchDate\' type=" +
"\'text\' class=\'input-text Wdate\' placeholder = \'请输入查询日期\' onclick=\'WdatePicker()\' " +
"/> <p style=\'margin:10px 0 0 0;color:#777\' id=\'searchTip\' class=\'hidden\'>已查询到<sp" +
"an style=\'color:red;margin:0 3px\'>0</span>个文件，是否要<a style=\'color:blue\' href=\'jav" +
"ascript:void(0)\'>下载?</a></p></div></div><div class=\'row cl\'><div class=\'col-xs-8" +
"  col-xs-offset-3 mt-20\'><input type=\'button\' class=\'btn btn-primary radius\'  on" +
"click=\'validateForm()\' value=\'确&nbsp;定\' /><input type=\'button\' id=\'close_btn\' cl" +
"ass=\'btn btn-default radius \' value=\'取&nbsp;消\' /></div></div></article>\"\r\n      " +
"  });\r\n\r\n        $(\"#close_btn\").click(function () {\r\n            layer.close(in" +
"dex);\r\n        })\r\n    }\r\n\r\n    //验证查询日期\r\n    function validateForm() {\r\n       " +
" if ($(\"#SearchDate\").val().length <= 0) {\r\n            layer.msg(\"请输入查询日期\");\r\n " +
"           return false;\r\n        }\r\n\r\n        $.ajax({\r\n            url: \"");

            
            #line 129 "..\..\Areas\manage\Views\Home\Index.cshtml"
             Write(Url.Action("SearchLog"));

            
            #line default
            #line hidden
WriteLiteral(@""",
            data: { ""SearchDate"": $(""#SearchDate"").val() },
            type: ""Get"",
            success: function (data) {
                console.log(data);
                if (data) {
                    $(""#searchTip"").removeClass(""hidden"");
                    $(""#searchTip span"").html(data.FileCount);
                    $(""#searchTip a"").attr(""href"", """);

            
            #line 137 "..\..\Areas\manage\Views\Home\Index.cshtml"
                                               Write(Url.Action("DownloadFile"));

            
            #line default
            #line hidden
WriteLiteral(@"?SearchDate="" + $(""#SearchDate"").val() + """");
                } else {
                    $(""#searchTip"").addClass(""hidden"");
                    layer.msg(""没有查询到相关日志"");
                }
            },
            error: function (data) { layer.alert(""操作失败"", { icon: 5 }); }
        });
    }


</script>
");

        }
    }
}
#pragma warning restore 1591
