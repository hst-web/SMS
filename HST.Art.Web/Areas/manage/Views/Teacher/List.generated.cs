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
    
    #line 2 "..\..\Areas\manage\Views\Teacher\List.cshtml"
    using HST.Art.Core;
    
    #line default
    #line hidden
    
    #line 1 "..\..\Areas\manage\Views\Teacher\List.cshtml"
    using HST.Art.Web;
    
    #line default
    #line hidden
    
    #line 3 "..\..\Areas\manage\Views\Teacher\List.cshtml"
    using HST.Utillity;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/manage/Views/Teacher/List.cshtml")]
    public partial class _Areas_manage_Views_Teacher_List_cshtml : System.Web.Mvc.WebViewPage<dynamic>
    {
        public _Areas_manage_Views_Teacher_List_cshtml()
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

WriteLiteral(">&gt;</span>证书管理 <span");

WriteLiteral(" class=\"c-gray en\"");

WriteLiteral(">&gt;</span> 教师证书 <a");

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

WriteLiteral(">\r\n            <option");

WriteLiteral(" value=\"\"");

WriteLiteral(">请选择筛选条件</option>\r\n            <option");

WriteAttribute("value", Tuple.Create(" value=\"", 691), Tuple.Create("\"", 722)
            
            #line 11 "..\..\Areas\manage\Views\Teacher\List.cshtml"
, Tuple.Create(Tuple.Create("", 699), Tuple.Create<System.Object, System.Int32>((int)SearchType.Name
            
            #line default
            #line hidden
, 699), false)
);

WriteLiteral(">教师姓名</option>\r\n            <option");

WriteAttribute("value", Tuple.Create(" value=\"", 758), Tuple.Create("\"", 791)
            
            #line 12 "..\..\Areas\manage\Views\Teacher\List.cshtml"
, Tuple.Create(Tuple.Create("", 766), Tuple.Create<System.Object, System.Int32>((int)SearchType.Number
            
            #line default
            #line hidden
, 766), false)
);

WriteLiteral(">证书编号</option>\r\n            <option");

WriteAttribute("value", Tuple.Create(" value=\"", 827), Tuple.Create("\"", 858)
            
            #line 13 "..\..\Areas\manage\Views\Teacher\List.cshtml"
, Tuple.Create(Tuple.Create("", 835), Tuple.Create<System.Object, System.Int32>((int)SearchType.Area
            
            #line default
            #line hidden
, 835), false)
);

WriteLiteral(">所在地区</option>\r\n            <option");

WriteAttribute("value", Tuple.Create(" value=\"", 894), Tuple.Create("\"", 925)
            
            #line 14 "..\..\Areas\manage\Views\Teacher\List.cshtml"
, Tuple.Create(Tuple.Create("", 902), Tuple.Create<System.Object, System.Int32>((int)SearchType.Type
            
            #line default
            #line hidden
, 902), false)
);

WriteLiteral(">证书类别</option>\r\n        </select>\r\n        <input");

WriteLiteral(" type=\"text\"");

WriteLiteral(" id=\"sel_filVal\"");

WriteLiteral(" placeholder=\"请输入筛选关键字\"");

WriteLiteral(" class=\"input-text lg inline hidden\"");

WriteLiteral(" />\r\n        <select");

WriteLiteral(" name=\"sel_orderstate\"");

WriteLiteral(" id=\"sel_type\"");

WriteLiteral(" class=\"select select-box sm inline hidden\"");

WriteLiteral(" style=\"width:150px\"");

WriteLiteral(">\r\n            <option");

WriteLiteral(" value=\"\"");

WriteLiteral(">请选择证书类别</option>\r\n            <option");

WriteAttribute("value", Tuple.Create(" value=\"", 1250), Tuple.Create("\"", 1287)
            
            #line 19 "..\..\Areas\manage\Views\Teacher\List.cshtml"
, Tuple.Create(Tuple.Create("", 1258), Tuple.Create<System.Object, System.Int32>((int)CertificateType.Prize
            
            #line default
            #line hidden
, 1258), false)
);

WriteLiteral(">获奖证书</option>\r\n            <option");

WriteAttribute("value", Tuple.Create(" value=\"", 1323), Tuple.Create("\"", 1360)
            
            #line 20 "..\..\Areas\manage\Views\Teacher\List.cshtml"
, Tuple.Create(Tuple.Create("", 1331), Tuple.Create<System.Object, System.Int32>((int)CertificateType.Train
            
            #line default
            #line hidden
, 1331), false)
);

WriteLiteral(">师资认证</option>\r\n        </select>\r\n        <select");

WriteLiteral(" name=\"sel_orderstate\"");

WriteLiteral(" id=\"sel_area\"");

WriteLiteral(" class=\"select select-box sm inline hidden\"");

WriteLiteral(" style=\"width:150px\"");

WriteLiteral(">\r\n            <option");

WriteLiteral(" value=\"\"");

WriteLiteral(">请选择所在地区</option>\r\n");

            
            #line 24 "..\..\Areas\manage\Views\Teacher\List.cshtml"
            
            
            #line default
            #line hidden
            
            #line 24 "..\..\Areas\manage\Views\Teacher\List.cshtml"
             foreach (KeyValuePair<int, string> item in ViewBag.AreaCity)
            {

            
            #line default
            #line hidden
WriteLiteral("                <option");

WriteAttribute("value", Tuple.Create(" value=\"", 1673), Tuple.Create("\"", 1690)
            
            #line 26 "..\..\Areas\manage\Views\Teacher\List.cshtml"
, Tuple.Create(Tuple.Create("", 1681), Tuple.Create<System.Object, System.Int32>(item.Key
            
            #line default
            #line hidden
, 1681), false)
);

WriteLiteral(">");

            
            #line 26 "..\..\Areas\manage\Views\Teacher\List.cshtml"
                                     Write(item.Value);

            
            #line default
            #line hidden
WriteLiteral("</option>\r\n");

            
            #line 27 "..\..\Areas\manage\Views\Teacher\List.cshtml"
            }

            
            #line default
            #line hidden
WriteLiteral("        </select>\r\n        <button");

WriteLiteral(" name=\"search\"");

WriteLiteral(" id=\"search\"");

WriteLiteral(" class=\"btn btn-success ml-20\"");

WriteLiteral(" type=\"button\"");

WriteLiteral("><i");

WriteLiteral(" class=\"Hui-iconfont\"");

WriteLiteral(">&#xe665;</i> 查询</button>\r\n        <button");

WriteLiteral(" class=\"btn btn-primary\"");

WriteAttribute("onclick", Tuple.Create(" onclick=\"", 1923), Tuple.Create("\"", 1969)
, Tuple.Create(Tuple.Create("", 1933), Tuple.Create("tea_add(\'新建证书\',\'", 1933), true)
            
            #line 30 "..\..\Areas\manage\Views\Teacher\List.cshtml"
, Tuple.Create(Tuple.Create("", 1949), Tuple.Create<System.Object, System.Int32>(Url.Action("Add")
            
            #line default
            #line hidden
, 1949), false)
, Tuple.Create(Tuple.Create("", 1967), Tuple.Create("\')", 1967), true)
);

WriteLiteral(" type=\"button\"");

WriteLiteral(">新建证书</button>\r\n        <button");

WriteLiteral(" class=\"btn btn-default\"");

WriteAttribute("onclick", Tuple.Create(" onclick=\"", 2039), Tuple.Create("\"", 2091)
, Tuple.Create(Tuple.Create("", 2049), Tuple.Create("tea_import(\'批量导入\',\'", 2049), true)
            
            #line 31 "..\..\Areas\manage\Views\Teacher\List.cshtml"
, Tuple.Create(Tuple.Create("", 2068), Tuple.Create<System.Object, System.Int32>(Url.Action("Import")
            
            #line default
            #line hidden
, 2068), false)
, Tuple.Create(Tuple.Create("", 2089), Tuple.Create("\')", 2089), true)
);

WriteLiteral(" type=\"button\"");

WriteLiteral(">批量导入</button>\r\n    </div>\r\n    <div");

WriteLiteral(" class=\"mt-10\"");

WriteLiteral(">\r\n        <table");

WriteLiteral(" id=\"tbTable\"");

WriteLiteral(" class=\"table table-border table-bordered table-bg table-sort\"");

WriteLiteral(">\r\n            <thead>\r\n                <tr");

WriteLiteral(" class=\"text-c\"");

WriteLiteral(">\r\n                    <th");

WriteLiteral(" width=\"8%\"");

WriteLiteral(">序号</th>\r\n                    <th");

WriteLiteral(" width=\"8%\"");

WriteLiteral(">姓名</th>\r\n                    <th");

WriteLiteral(" width=\"8%\"");

WriteLiteral(">性别</th>\r\n                    <th");

WriteLiteral(" width=\"10%\"");

WriteLiteral(">教师级别</th>\r\n                    <th");

WriteLiteral(" width=\"10%\"");

WriteLiteral(">证书编号</th>\r\n                    <th");

WriteLiteral(" width=\"10%\"");

WriteLiteral(">所在地区</th>\r\n                    <th");

WriteLiteral(" width=\"8%\"");

WriteLiteral(">证书类别</th>\r\n                    <th");

WriteLiteral(" width=\"8%\"");

WriteLiteral(">状态</th>\r\n                    <th");

WriteLiteral(" width=\"8%\"");

WriteLiteral(">创建人</th>\r\n                    <th");

WriteLiteral(" width=\"11%\"");

WriteLiteral(">创建时间</th>\r\n                    <th");

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
                    $(""#sel_area"").addClass(""hidden"");
                    $(""#sel_type"").addClass(""hidden"");
                }
                else if ($(this).val() == '");

            
            #line 77 "..\..\Areas\manage\Views\Teacher\List.cshtml"
                                       Write((int)SearchType.Type);

            
            #line default
            #line hidden
WriteLiteral("\') {\r\n                    $(\"#sel_type\").removeClass(\"hidden\");\r\n                " +
"    $(\"#sel_area\").addClass(\"hidden\");\r\n                    $(\"#sel_filVal\").add" +
"Class(\"hidden\");\r\n                } else if ($(this).val() == \'");

            
            #line 81 "..\..\Areas\manage\Views\Teacher\List.cshtml"
                                         Write((int)SearchType.Area);

            
            #line default
            #line hidden
WriteLiteral(@"') {
                    $(""#sel_filVal"").addClass(""hidden"");
                    $(""#sel_area"").removeClass(""hidden"");
                    $(""#sel_type"").addClass(""hidden"");
                } else {
                    $(""#sel_filVal"").removeClass(""hidden"");
                    $(""#sel_area"").addClass(""hidden"");
                    $(""#sel_type"").addClass(""hidden"");
                }

                $(""#sel_filVal,#sel_area,#sel_type"").val("""");
            })
        });

        /*编辑*/
        function tea_edit(title, url, id) {
            var index = top.layer.open({
                type: 2,
                title: title,
                area: ['750px', '510px'],
                content: url + ""?id="" + id
            });
        }

        /*添加*/
        function tea_add(title, url) {
            var index = top.layer.open({
                type: 2,
                title: title,
                area: ['750px', '510px'],
                content: url
            });
        }

        function tea_import(title, url) {
            var index = top.layer.open({
                type: 2,
                title: title,
                area: ['750px', '500px'],
                content: url
            });
        }

        function getFilterVal() {
            if ($(""#sel_filType"").val() == '");

            
            #line 125 "..\..\Areas\manage\Views\Teacher\List.cshtml"
                                        Write((int)SearchType.Type);

            
            #line default
            #line hidden
WriteLiteral("\') {\r\n                return $(\"#sel_type\").val();\r\n            } else if ($(\"#se" +
"l_filType\").val() == \'");

            
            #line 127 "..\..\Areas\manage\Views\Teacher\List.cshtml"
                                               Write((int)SearchType.Area);

            
            #line default
            #line hidden
WriteLiteral(@"') {
                return $(""#sel_area"").val();
            } else {
                return $(""#sel_filVal"").val();
            }

        }

        function initializeTable() {
            var dataTable = $('#tbTable').DataTable({
                ""serverSide"": true,
                ""ajax"": {
                    ""url"": """);

            
            #line 139 "..\..\Areas\manage\Views\Teacher\List.cshtml"
                       Write(Url.Action("GetJsonData"));

            
            #line default
            #line hidden
WriteLiteral("\",\r\n                    \"type\": \"post\",\r\n                    \"data\": function (da" +
"ta) {\r\n                        data.pageIndex = (data.start / data.length) + 1;\r" +
"\n                        data.filterKey = $(\"#sel_filType\").val();\r\n            " +
"            data.filterVal = getFilterVal();\r\n                    }\r\n           " +
"     },\r\n                \"columns\": [\r\n                      { \"defaultContent\":" +
" \"\" },\r\n                      { \"mDataProp\": \"TeacherName\" },\r\n                 " +
"      { \"mDataProp\": \"GenderName\" },\r\n                      { \"mDataProp\": \"Leve" +
"lName\" },\r\n                      { \"mDataProp\": \"Number\" },\r\n                   " +
"   { \"mDataProp\": \"Area\" },\r\n                      { \"mDataProp\": \"CategoryName\"" +
" },\r\n                      { \"mDataProp\": \"State\" },\r\n                      { \"m" +
"DataProp\": \"UserName\" },\r\n                      { \"mDataProp\": \"CreateTime\" },\r\n" +
"                      { \"defaultContent\": \"\" }\r\n\r\n                ],\r\n          " +
"      \"columnDefs\": [\r\n                 {\r\n                     \"targets\": [7],\r" +
"\n                     \"data\": \"State\",\r\n                     \"render\": function " +
"(data, type, full) {\r\n                         var result = data;\r\n             " +
"            if (data > 0) {\r\n                             result = \"<span class=" +
"\\\"label label-success radius\\\">已上架</span>\";\r\n                         } else {\r\n" +
"                             result = \"<span class=\\\"label label-danger radius\\\"" +
">已下架</span>\";\r\n                         }\r\n\r\n                         return res" +
"ult;\r\n                     }\r\n                 },\r\n                {\r\n          " +
"          \"targets\": [-1],\r\n                    \"data\": \"Id\",\r\n                 " +
"   \"render\": function (data, type, full) {\r\n                        var tmpStrin" +
"g = \"<a  onClick=\\\"tea_edit(\'修改证书\',\'");

            
            #line 180 "..\..\Areas\manage\Views\Teacher\List.cshtml"
                                                                   Write(Url.Action("Edit"));

            
            #line default
            #line hidden
WriteLiteral("\',\" + data + \")\\\" href=\\\"javascript:;\\\" title=\\\"编辑\\\">编辑</a>\";\r\n                  " +
"      if (full.State == 0)\r\n                            tmpString += \"<a href=\\\"" +
"javascript:;\\\" onClick=\\\"obj_publish(\'");

            
            #line 182 "..\..\Areas\manage\Views\Teacher\List.cshtml"
                                                                                     Write(Url.Action("Publish"));

            
            #line default
            #line hidden
WriteLiteral("\',\" + data + \")\\\"  title=\\\"上架\\\">上架</a>\";\r\n                        else\r\n         " +
"                   tmpString += \"<a href=\\\"javascript:;\\\" onClick=\\\"obj_shelves(" +
"\'");

            
            #line 184 "..\..\Areas\manage\Views\Teacher\List.cshtml"
                                                                                     Write(Url.Action("Shelves"));

            
            #line default
            #line hidden
WriteLiteral("\',\" + data + \")\\\"  title=\\\"下架\\\">下架</a>\";\r\n                        tmpString += \"<" +
"a href=\\\"javascript:;\\\" onClick=\\\"obj_del(\'证书\',\'");

            
            #line 185 "..\..\Areas\manage\Views\Teacher\List.cshtml"
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
"en(\'td\').eq(7).attr(\"class\", \"td-status\");\r\n                    $(row).children(" +
"\'td\').eq(10).attr(\"class\", \"td-manage\");\r\n                },\r\n                \"i" +
"nitComplete\": function (settings, json) {\r\n\r\n                },\r\n               " +
" language: {\r\n                    lengthMenu: \'\',\r\n                    loadingRe" +
"cords: \'数据加载中...\',\r\n                    paginate: {\r\n                        pre" +
"vious: \"上一页\",\r\n                        next: \"下一页\",\r\n                        fir" +
"st: \"\",\r\n                        last: \"\"\r\n                    },\r\n             " +
"       zeroRecords: \"暂无数据\",\r\n\r\n                    info: \"<span class=\'pagesStyl" +
"e\'>总共<span class=\'recordsStyle\'> _TOTAL_ 条,计 _PAGES_ </span>页，当前显示 _START_ -- _E" +
"ND_ 条记录 </span>\",\r\n                    infoEmpty: \"0条记录\",\r\n                    i" +
"nfoFiltered: \"\"\r\n                },\r\n                \"searching\": false,\r\n      " +
"          \"ordering\": false,\r\n                \"autoWidth\": false,\r\n             " +
"   \"iDisplayLength\": 10,\r\n                \"processing\": true,\r\n                /" +
"/destroy: true, //Cannot reinitialise DataTable,解决重新加载表格内容问题\r\n\r\n            });\r" +
"\n            return dataTable;\r\n        }\r\n\r\n    </script>\r\n");

});

        }
    }
}
#pragma warning restore 1591
