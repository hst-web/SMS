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
    
    #line 5 "..\..\Views\Home\Social.cshtml"
    using HST.Art.Core;
    
    #line default
    #line hidden
    
    #line 4 "..\..\Views\Home\Social.cshtml"
    using HST.Art.Web;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Home/Social.cshtml")]
    public partial class _Views_Home_Social_cshtml : System.Web.Mvc.WebViewPage<WebContentViewModel>
    {
        public _Views_Home_Social_cshtml()
        {
        }
        public override void Execute()
        {
            
            #line 1 "..\..\Views\Home\Social.cshtml"
  
    ViewBag.Title = "社会公益";

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("\r\n");

WriteLiteral("<div");

WriteLiteral(" class=\"list\"");

WriteLiteral(">\r\n    <div");

WriteLiteral(" class=\"position\"");

WriteLiteral(">当前位置：<a");

WriteAttribute("href", Tuple.Create(" href=\"", 160), Tuple.Create("\"", 194)
            
            #line 9 "..\..\Views\Home\Social.cshtml"
, Tuple.Create(Tuple.Create("", 167), Tuple.Create<System.Object, System.Int32>(Url.Action("index","home")
            
            #line default
            #line hidden
, 167), false)
);

WriteLiteral(">首页</a> / <a");

WriteAttribute("href", Tuple.Create(" href=\"", 207), Tuple.Create("\"", 235)
            
            #line 9 "..\..\Views\Home\Social.cshtml"
        , Tuple.Create(Tuple.Create("", 214), Tuple.Create<System.Object, System.Int32>(Url.Action("social")
            
            #line default
            #line hidden
, 214), false)
);

WriteLiteral(" cl");

WriteLiteral(" class=\'red\'");

WriteLiteral(">社会公益</a></div>\r\n    <div");

WriteLiteral(" class=\"list_cont\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" class=\"list_fl fl\"");

WriteLiteral(">\r\n            <ul");

WriteLiteral(" class=\"list_ul list_exam\"");

WriteLiteral(">\r\n                <li");

WriteLiteral(" class=\"title\"");

WriteLiteral(">社会公益</li>\r\n");

            
            #line 14 "..\..\Views\Home\Social.cshtml"
                
            
            #line default
            #line hidden
            
            #line 14 "..\..\Views\Home\Social.cshtml"
                 if (ViewBag.Categorys != null)
                {
                    foreach (CategoryDictionary item in ViewBag.Categorys)
                    {

            
            #line default
            #line hidden
WriteLiteral("                        <li");

WriteAttribute("class", Tuple.Create(" class=\"", 614), Tuple.Create("\"", 696)
            
            #line 18 "..\..\Views\Home\Social.cshtml"
, Tuple.Create(Tuple.Create("", 622), Tuple.Create<System.Object, System.Int32>(Model.PageFilter!=null&&Model.PageFilter.Category==item.Id?"current":""
            
            #line default
            #line hidden
, 622), false)
);

WriteLiteral(">\r\n                            <a");

WriteAttribute("href", Tuple.Create(" href=\"", 730), Tuple.Create("\"", 805)
            
            #line 19 "..\..\Views\Home\Social.cshtml"
, Tuple.Create(Tuple.Create("", 737), Tuple.Create<System.Object, System.Int32>(Url.Action("social", new { qtype = QSType.list, fctype = item.Id })
            
            #line default
            #line hidden
, 737), false)
);

WriteLiteral(">");

            
            #line 19 "..\..\Views\Home\Social.cshtml"
                                                                                                      Write(item.Name);

            
            #line default
            #line hidden
WriteLiteral("</a>\r\n                        </li>\r\n");

            
            #line 21 "..\..\Views\Home\Social.cshtml"
                    }
                }

            
            #line default
            #line hidden
WriteLiteral("            </ul>\r\n");

WriteLiteral("            ");

            
            #line 24 "..\..\Views\Home\Social.cshtml"
       Write(Html.Action("GetRecommend", new { sectionType = CategoryType.Social }));

            
            #line default
            #line hidden
WriteLiteral("\r\n        </div>\r\n");

            
            #line 26 "..\..\Views\Home\Social.cshtml"
        
            
            #line default
            #line hidden
            
            #line 26 "..\..\Views\Home\Social.cshtml"
         if (Model.QType != QSType.detail)
        {

            
            #line default
            #line hidden
WriteLiteral("            <div");

WriteLiteral(" class=\"list_fr fr\"");

WriteLiteral(" id=\"tbTable_wrapper\"");

WriteLiteral("></div>\r\n");

            
            #line 29 "..\..\Views\Home\Social.cshtml"

        }
        else
        {
            
            
            #line default
            #line hidden
            
            #line 33 "..\..\Views\Home\Social.cshtml"
       Write(Html.Partial("GetDetail", Model.DetailModel));

            
            #line default
            #line hidden
            
            #line 33 "..\..\Views\Home\Social.cshtml"
                                                         
        }

            
            #line default
            #line hidden
WriteLiteral("\r\n");

            
            #line 36 "..\..\Views\Home\Social.cshtml"
        
            
            #line default
            #line hidden
            
            #line 36 "..\..\Views\Home\Social.cshtml"
         using (Ajax.BeginForm("GetArticList", new { }, new AjaxOptions { OnFailure = "showException", OnSuccess = "", UpdateTargetId = "tbTable_wrapper", OnBegin = "", OnComplete = "" }, new { id = "searchByRequest" }))
        {
            
            
            #line default
            #line hidden
            
            #line 38 "..\..\Views\Home\Social.cshtml"
       Write(Html.Hidden("PageIndex", Model.PageFilter.PageIndex));

            
            #line default
            #line hidden
            
            #line 38 "..\..\Views\Home\Social.cshtml"
                                                                 
            
            
            #line default
            #line hidden
            
            #line 39 "..\..\Views\Home\Social.cshtml"
       Write(Html.Hidden("PageSize", Model.PageFilter.PageSize));

            
            #line default
            #line hidden
            
            #line 39 "..\..\Views\Home\Social.cshtml"
                                                               
            
            
            #line default
            #line hidden
            
            #line 40 "..\..\Views\Home\Social.cshtml"
       Write(Html.Hidden("Category", Model.PageFilter.Category));

            
            #line default
            #line hidden
            
            #line 40 "..\..\Views\Home\Social.cshtml"
                                                               
            
            
            #line default
            #line hidden
            
            #line 41 "..\..\Views\Home\Social.cshtml"
       Write(Html.Hidden("SectionType", Model.PageFilter.SectionType));

            
            #line default
            #line hidden
            
            #line 41 "..\..\Views\Home\Social.cshtml"
                                                                     
        }

            
            #line default
            #line hidden
WriteLiteral("    </div><!--/list_cont-->\r\n</div><!--/list-->\r\n\r\n");

DefineSection("scripts", () => {

WriteLiteral("\r\n    <script");

WriteLiteral(" type=\"text/javascript\"");

WriteLiteral(">\r\n        $(document).ready(function () {\r\n            if (\'");

            
            #line 49 "..\..\Views\Home\Social.cshtml"
            Write(Model.QType);

            
            #line default
            #line hidden
WriteLiteral("\' != \'");

            
            #line 49 "..\..\Views\Home\Social.cshtml"
                              Write(QSType.detail);

            
            #line default
            #line hidden
WriteLiteral(@"') {
                changeQueryCondition();
            }
        })

        //改变筛选条件
        function changeQueryCondition() {
            $(""#PageIndex"").val(1);//页索引置为1
            $(""#searchByRequest"").submit();
        }

        //改变索引页
        function changePageIndex(index) {
            $(""#PageIndex"").val(index);
            $(""#searchByRequest"").submit();
        }
    </script>
");

});

        }
    }
}
#pragma warning restore 1591
