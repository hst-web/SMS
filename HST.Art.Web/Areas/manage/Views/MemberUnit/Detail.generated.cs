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
    
    #line 6 "..\..\Areas\manage\Views\MemberUnit\Detail.cshtml"
    using HST.Art.Core;
    
    #line default
    #line hidden
    
    #line 5 "..\..\Areas\manage\Views\MemberUnit\Detail.cshtml"
    using HST.Art.Web;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/manage/Views/MemberUnit/Detail.cshtml")]
    public partial class _Areas_manage_Views_MemberUnit_Detail_cshtml : System.Web.Mvc.WebViewPage<MemberUnitViewModel>
    {
        public _Areas_manage_Views_MemberUnit_Detail_cshtml()
        {
        }
        public override void Execute()
        {
            
            #line 1 "..\..\Areas\manage\Views\MemberUnit\Detail.cshtml"
  
    Layout = null;

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n");

WriteLiteral("\r\n");

            
            #line 10 "..\..\Areas\manage\Views\MemberUnit\Detail.cshtml"
Write(Scripts.Render("~/bundles/saos"));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

            
            #line 11 "..\..\Areas\manage\Views\MemberUnit\Detail.cshtml"
Write(Styles.Render("~/appcss"));

            
            #line default
            #line hidden
WriteLiteral("\r\n<style");

WriteLiteral(" type=\"text/css\"");

WriteLiteral(">\r\n    p{font-size:14px}\r\n       img{max-width:100% !important}\r\n</style>\r\n<artic" +
"le");

WriteLiteral(" class=\"page-container\"");

WriteLiteral(">\r\n    <div");

WriteLiteral(" class=\"form-horizontal\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" class=\"row cl\"");

WriteLiteral(">\r\n            <div");

WriteLiteral(" class=\"col-xs-12\"");

WriteLiteral(">\r\n");

            
            #line 20 "..\..\Areas\manage\Views\MemberUnit\Detail.cshtml"
                
            
            #line default
            #line hidden
            
            #line 20 "..\..\Areas\manage\Views\MemberUnit\Detail.cshtml"
                 if (string.IsNullOrEmpty(Model.Description))
                {

            
            #line default
            #line hidden
WriteLiteral("                    <p");

WriteLiteral(" style=\"text-align:center;margin-top:20%\"");

WriteLiteral(">暂无详情信息</p>\r\n");

            
            #line 23 "..\..\Areas\manage\Views\MemberUnit\Detail.cshtml"
                }
                else
                {
                    
            
            #line default
            #line hidden
            
            #line 26 "..\..\Areas\manage\Views\MemberUnit\Detail.cshtml"
               Write(Html.Raw(Model.Description));

            
            #line default
            #line hidden
            
            #line 26 "..\..\Areas\manage\Views\MemberUnit\Detail.cshtml"
                                                
                }

            
            #line default
            #line hidden
WriteLiteral("               \r\n            </div>\r\n        </div>\r\n    </div>\r\n</article>\r\n");

        }
    }
}
#pragma warning restore 1591
