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
    
    #line 2 "..\..\Areas\manage\Views\Download\List.cshtml"
    using HST.Art.Core;
    
    #line default
    #line hidden
    
    #line 1 "..\..\Areas\manage\Views\Download\List.cshtml"
    using HST.Art.Web;
    
    #line default
    #line hidden
    
    #line 3 "..\..\Areas\manage\Views\Download\List.cshtml"
    using HST.Utillity;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/manage/Views/Download/List.cshtml")]
    public partial class _Areas_manage_Views_Download_List_cshtml : System.Web.Mvc.WebViewPage<dynamic>
    {
        public _Areas_manage_Views_Download_List_cshtml()
        {
        }
        public override void Execute()
        {
WriteLiteral("\r\n<nav");

WriteLiteral(" class=\"breadcrumb\"");

WriteLiteral("><i");

WriteLiteral(" class=\"Hui-iconfont\"");

WriteLiteral(">&#xe67f;</i> 首页 <span");

WriteLiteral(" class=\"c-gray en\"");

WriteLiteral(">&gt;</span>下载管理 <a");

WriteLiteral(" class=\"btn btn-primary radius r\"");

WriteLiteral(" style=\"line-height:1.6em;margin-top:3px\"");

WriteLiteral(" href=\"javascript:location.replace(location.href);\"");

WriteLiteral(" title=\"刷新\"");

WriteLiteral("><i");

WriteLiteral(" class=\"Hui-iconfont\"");

WriteLiteral(">&#xe68f;</i></a></nav>\r\n<div");

WriteLiteral(" class=\"page-container\"");

WriteLiteral(">\r\n    <div");

WriteLiteral(" class=\"cl pd-10 bg-1 bk-gray mt-10 search-wrap\"");

WriteLiteral(">\r\n        <span");

WriteLiteral(" class=\"label-inline\"");

WriteLiteral(">筛选：</span>\r\n        <select");

WriteLiteral(" name=\"sel_orderstate\"");

WriteLiteral(" id=\"sel_filType\"");

WriteLiteral(" class=\"select select-box sm inline\"");

WriteLiteral(" style=\"width:150px\"");

WriteLiteral(">\r\n            <option");

WriteLiteral(" value=\"\"");

WriteLiteral(">请选择筛选条件</option>\r\n            <option");

WriteAttribute("value", Tuple.Create(" value=\"", 670), Tuple.Create("\"", 701)
            
            #line 11 "..\..\Areas\manage\Views\Download\List.cshtml"
, Tuple.Create(Tuple.Create("", 678), Tuple.Create<System.Object, System.Int32>((int)SearchType.Type
            
            #line default
            #line hidden
, 678), false)
);

WriteLiteral(">所属类别</option>\r\n            <option");

WriteAttribute("value", Tuple.Create(" value=\"", 737), Tuple.Create("\"", 769)
            
            #line 12 "..\..\Areas\manage\Views\Download\List.cshtml"
, Tuple.Create(Tuple.Create("", 745), Tuple.Create<System.Object, System.Int32>((int)SearchType.Title
            
            #line default
            #line hidden
, 745), false)
);

WriteLiteral(">标题</option>\r\n            <option");

WriteAttribute("value", Tuple.Create(" value=\"", 803), Tuple.Create("\"", 835)
            
            #line 13 "..\..\Areas\manage\Views\Download\List.cshtml"
, Tuple.Create(Tuple.Create("", 811), Tuple.Create<System.Object, System.Int32>((int)SearchType.State
            
            #line default
            #line hidden
, 811), false)
);

WriteLiteral(">状态</option>\r\n        </select>\r\n        <input");

WriteLiteral(" type=\"text\"");

WriteLiteral(" id=\"sel_filVal\"");

WriteLiteral(" placeholder=\"请输入筛选关键字\"");

WriteLiteral(" class=\"input-text lg inline hidden\"");

WriteLiteral(" />\r\n        <select");

WriteLiteral(" name=\"sel_orderstate\"");

WriteLiteral(" id=\"sel_type\"");

WriteLiteral(" class=\"select select-box sm inline hidden\"");

WriteLiteral(" style=\"min-width:150px\"");

WriteLiteral(">\r\n            <option");

WriteLiteral(" value=\"\"");

WriteLiteral(">请选择所属类别</option>\r\n");

            
            #line 18 "..\..\Areas\manage\Views\Download\List.cshtml"
            
            
            #line default
            #line hidden
            
            #line 18 "..\..\Areas\manage\Views\Download\List.cshtml"
             foreach (CategoryDictionary item in ViewBag.AllCategory)
            {

            
            #line default
            #line hidden
WriteLiteral("                <option");

WriteAttribute("value", Tuple.Create(" value=\"", 1252), Tuple.Create("\"", 1268)
            
            #line 20 "..\..\Areas\manage\Views\Download\List.cshtml"
, Tuple.Create(Tuple.Create("", 1260), Tuple.Create<System.Object, System.Int32>(item.Id
            
            #line default
            #line hidden
, 1260), false)
);

WriteLiteral(">");

            
            #line 20 "..\..\Areas\manage\Views\Download\List.cshtml"
                                    Write(item.Name);

            
            #line default
            #line hidden
WriteLiteral("</option>\r\n");

            
            #line 21 "..\..\Areas\manage\Views\Download\List.cshtml"
            }

            
            #line default
            #line hidden
WriteLiteral("        </select>\r\n        <select");

WriteLiteral(" name=\"sel_orderstate\"");

WriteLiteral(" id=\"sel_state\"");

WriteLiteral(" class=\"select select-box sm inline hidden\"");

WriteLiteral(">\r\n            <option");

WriteLiteral(" value=\"\"");

WriteLiteral(">请选择文件状态</option>\r\n            <option");

WriteAttribute("value", Tuple.Create(" value=\"", 1489), Tuple.Create("\"", 1523)
            
            #line 25 "..\..\Areas\manage\Views\Download\List.cshtml"
, Tuple.Create(Tuple.Create("", 1497), Tuple.Create<System.Object, System.Int32>((int)PublishState.Upper
            
            #line default
            #line hidden
, 1497), false)
);

WriteLiteral(">已上架</option>\r\n            <option");

WriteAttribute("value", Tuple.Create(" value=\"", 1558), Tuple.Create("\"", 1592)
            
            #line 26 "..\..\Areas\manage\Views\Download\List.cshtml"
, Tuple.Create(Tuple.Create("", 1566), Tuple.Create<System.Object, System.Int32>((int)PublishState.Lower
            
            #line default
            #line hidden
, 1566), false)
);

WriteLiteral(">已下架</option>\r\n        </select>\r\n        <button");

WriteLiteral(" name=\"search\"");

WriteLiteral(" id=\"search\"");

WriteLiteral(" class=\"btn btn-success ml-20\"");

WriteLiteral(" type=\"button\"");

WriteLiteral("><i");

WriteLiteral(" class=\"Hui-iconfont\"");

WriteLiteral(">&#xe665;</i> 查询</button>\r\n");

WriteLiteral("        ");

            
            #line 29 "..\..\Areas\manage\Views\Download\List.cshtml"
   Write(Html.ActionLink("新建下载", "Add", null, new { @class = "btn btn-primary" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n    </div>\r\n    <div");

WriteLiteral(" class=\"mt-10\"");

WriteLiteral(">\r\n        <table");

WriteLiteral(" id=\"tbTable\"");

WriteLiteral(" class=\"table table-border table-bordered table-bg table-sort\"");

WriteLiteral(">\r\n            <thead>\r\n                <tr");

WriteLiteral(" class=\"text-c\"");

WriteLiteral(">\r\n                    <th");

WriteLiteral(" width=\"8%\"");

WriteLiteral(">序号</th>\r\n                    <th");

WriteLiteral(" width=\"11%\"");

WriteLiteral(">所属类别</th>\r\n                    <th");

WriteLiteral(" width=\"15%\"");

WriteLiteral(">标题</th>\r\n                    <th");

WriteLiteral(" width=\"20%\"");

WriteLiteral(">下载附件</th>\r\n                    <th");

WriteLiteral(" width=\"8%\"");

WriteLiteral(">状态</th>\r\n                    <th");

WriteLiteral(" width=\"8%\"");

WriteLiteral(">创建人</th>\r\n                    <th");

WriteLiteral(" width=\"11%\"");

WriteLiteral(">创建时间</th>\r\n                    <th");

WriteLiteral(" width=\"8%\"");

WriteLiteral(">详情</th>\r\n                    <th");

WriteLiteral(" width=\"11%\"");

WriteLiteral(">操作</th>\r\n                </tr>\r\n            </thead>\r\n        </table>\r\n    </di" +
"v>\r\n</div>\r\n\r\n");

DefineSection("scripts", () => {

WriteLiteral("\r\n    <script");

WriteLiteral(" type=\"text/javascript\"");

WriteLiteral(@">
        var table1 = null;
        $(function () {
            table1 = initializeTable();
            $(""#search"").click(function () {
                table1.ajax.reload();
            });

            //输入名称回车查询
            $(""#sel_filVal"").keypress(function (e) {
                if (e.which == 13) {
                    table1.ajax.reload();
                    $(""#search"").focus();
                }
            });

            $(""#sel_filType"").change(function () {
                if ($(this).val() == """") {
                    $(""#sel_filVal"").addClass(""hidden"");
                    $(""#sel_state"").addClass(""hidden"");
                    $(""#sel_type"").addClass(""hidden"");
                }
                else if ($(this).val() == '");

            
            #line 73 "..\..\Areas\manage\Views\Download\List.cshtml"
                                       Write((int)SearchType.Type);

            
            #line default
            #line hidden
WriteLiteral("\') {\r\n                    $(\"#sel_type\").removeClass(\"hidden\");\r\n                " +
"    $(\"#sel_state\").addClass(\"hidden\");\r\n                    $(\"#sel_filVal\").ad" +
"dClass(\"hidden\");\r\n                } else if ($(this).val() == \'");

            
            #line 77 "..\..\Areas\manage\Views\Download\List.cshtml"
                                         Write((int)SearchType.State);

            
            #line default
            #line hidden
WriteLiteral(@"') {
                    $(""#sel_filVal"").addClass(""hidden"");
                    $(""#sel_state"").removeClass(""hidden"");
                    $(""#sel_type"").addClass(""hidden"");
                } else {
                    $(""#sel_filVal"").removeClass(""hidden"");
                    $(""#sel_state"").addClass(""hidden"");
                    $(""#sel_type"").addClass(""hidden"");
                }

                $(""#sel_filVal,#sel_state,#sel_type"").val("""");
            })
        });

        function getFilterVal() {
            if ($(""#sel_filType"").val() == '");

            
            #line 92 "..\..\Areas\manage\Views\Download\List.cshtml"
                                        Write((int)SearchType.Type);

            
            #line default
            #line hidden
WriteLiteral("\') {\r\n                return $(\"#sel_type\").val();\r\n            } else if ($(\"#se" +
"l_filType\").val() == \'");

            
            #line 94 "..\..\Areas\manage\Views\Download\List.cshtml"
                                               Write((int)SearchType.State);

            
            #line default
            #line hidden
WriteLiteral(@"') {
                return $(""#sel_state"").val();
            } else {
                return $(""#sel_filVal"").val();
            }

        }

        function initializeTable() {
            var dataTable = $('#tbTable').DataTable({
                ""serverSide"": true,
                ""ajax"": {
                    ""url"": """);

            
            #line 106 "..\..\Areas\manage\Views\Download\List.cshtml"
                       Write(Url.Action("GetJsonData"));

            
            #line default
            #line hidden
WriteLiteral(@""",
                    ""type"": ""post"",
                    ""data"": function (data) {
                        data.pageIndex = (data.start / data.length) + 1;
                        data.filterKey = $(""#sel_filType"").val();
                        data.filterVal = getFilterVal();
                    }
                },
                ""columns"": [
                      { ""defaultContent"": """" },
                      { ""mDataProp"": ""CategoryName"" },
                       { ""mDataProp"": ""FileTitle"" },
                      { ""mDataProp"": ""FileName"" },
                      { ""mDataProp"": ""State"" },
                      { ""mDataProp"": ""UserName"" },
                      { ""mDataProp"": ""CreateTime"" },
                      { ""mDataProp"": ""Id"" },
                      { ""defaultContent"": """" }

                ],
                ""columnDefs"": [
                 {
                     ""targets"": [3],
                     ""data"": ""FileName"",
                     ""render"": function (data, type, full) {
                         //    var result = ""<a class='fancybox' href='"" + full.HeadImg + ""' title='' data-rel='fancybox-button'></a>"";
                         var result = data;
                         if (full.Src) {
                             if (full.FileType == '");

            
            #line 134 "..\..\Areas\manage\Views\Download\List.cshtml"
                                               Write((int)FileFormat.Img);

            
            #line default
            #line hidden
WriteLiteral(@"') {
                                 result = ""<a class='fancybox' data-rel='fancybox-button'  href=\"""" + full.Src + ""\"">"" + data + ""</a>"";
                             } else {
                                 result = ""<a data-bind='""+full.FileType+""' href=\"""" + full.Src + ""\"">"" + data + ""</a>"";
                             }                               
                         }

                         return result;
                     }
                 },
                     {
                         ""targets"": [4],
                         ""data"": ""State"",
                         ""render"": function (data, type, full) {
                             var result = data;
                             if (data > 0) {
                                 result = ""<span class=\""label label-success radius\"">已上架</span>"";
                             } else {
                                 result = ""<span class=\""label label-danger radius\"">已下架</span>"";
                             }

                             return result;
                         }
                     },
                 {

                     ""targets"": [7],
                     ""data"": ""Id"",
                     ""render"": function (data, type, full) {
                         var result = ""<a href=\""javascript:;\"" onClick=\""obj_detail('查看详情','");

            
            #line 163 "..\..\Areas\manage\Views\Download\List.cshtml"
                                                                                        Write(Url.Action("Detail"));

            
            #line default
            #line hidden
WriteLiteral(@"',"" + data + "")\""  title=\""查看详情\"">查看详情</a>"";
                         return result;
                     }
                 },
                {
                    ""targets"": [-1],
                    ""data"": ""Id"",
                    ""render"": function (data, type, full) {
                        var tmpString = '<a href=""");

            
            #line 171 "..\..\Areas\manage\Views\Download\List.cshtml"
                                             Write(Url.Action("Edit"));

            
            #line default
            #line hidden
WriteLiteral("?id=\' + data + \'\" title=\"编辑\">编辑</a>\';\r\n                        if (full.State == " +
"0)\r\n                            tmpString += \"<a href=\\\"javascript:;\\\" onClick=\\" +
"\"obj_publish(\'");

            
            #line 173 "..\..\Areas\manage\Views\Download\List.cshtml"
                                                                                     Write(Url.Action("Publish"));

            
            #line default
            #line hidden
WriteLiteral("\',\" + data + \")\\\"  title=\\\"上架\\\">上架</a>\";\r\n                        else\r\n         " +
"                   tmpString += \"<a href=\\\"javascript:;\\\" onClick=\\\"obj_shelves(" +
"\'");

            
            #line 175 "..\..\Areas\manage\Views\Download\List.cshtml"
                                                                                     Write(Url.Action("Shelves"));

            
            #line default
            #line hidden
WriteLiteral("\',\" + data + \")\\\"  title=\\\"下架\\\">下架</a>\";\r\n                        tmpString += \"<" +
"a href=\\\"javascript:;\\\" onClick=\\\"obj_del(\'文件\',\'");

            
            #line 176 "..\..\Areas\manage\Views\Download\List.cshtml"
                                                                                  Write(Url.Action("Delete"));

            
            #line default
            #line hidden
WriteLiteral("\',\" + data + \")\\\"  title=\\\"删除\\\">删除</a>\";\r\n\r\n                        return tmpStr" +
"ing;\r\n                    }\r\n                }\r\n                ],\r\n            " +
"    \"fnDrawCallback\": function () {\r\n                    var api = this.api();\r\n" +
"                    var startIndex = api.context[0]._iDisplayStart;\r\n           " +
"         api.column(0).nodes().each(function (cell, i) {\r\n                      " +
"  cell.innerHTML = startIndex + i + 1;\r\n                    });\r\n               " +
" },\r\n                \"rowCallback\": function (row, data, displayIndex) {\r\n      " +
"              $(row).attr(\"class\", \"text-c\");\r\n                    $(row).childr" +
"en(\'td\').eq(4).attr(\"class\", \"td-status\");\r\n                    $(row).children(" +
"\'td\').eq(8).attr(\"class\", \"td-manage\");\r\n                },\r\n                \"in" +
"itComplete\": function (settings, json) {\r\n                    handleFancybox();\r" +
"\n                },\r\n                language: {\r\n                    lengthMenu" +
": \'\',\r\n                    loadingRecords: \'数据加载中...\',\r\n                    pagi" +
"nate: {\r\n                        previous: \"上一页\",\r\n                        next:" +
" \"下一页\",\r\n                        first: \"\",\r\n                        last: \"\"\r\n " +
"                   },\r\n                    zeroRecords: \"暂无数据\",\r\n\r\n             " +
"       info: \"<span class=\'pagesStyle\'>总共<span class=\'recordsStyle\'> _TOTAL_ 条,计" +
" _PAGES_ </span>页，当前显示 _START_ -- _END_ 条记录 </span>\",\r\n                    infoE" +
"mpty: \"0条记录\",\r\n                    infoFiltered: \"\"\r\n                },\r\n       " +
"         \"searching\": false,\r\n                \"ordering\": false,\r\n              " +
"  \"autoWidth\": false,\r\n                \"iDisplayLength\": 10,\r\n                \"p" +
"rocessing\": true,\r\n                //destroy: true, //Cannot reinitialise DataTa" +
"ble,解决重新加载表格内容问题\r\n\r\n            });\r\n            return dataTable;\r\n        }\r\n\r" +
"\n    </script>\r\n");

});

        }
    }
}
#pragma warning restore 1591
