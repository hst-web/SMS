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
    
    #line 6 "..\..\Areas\manage\Views\Home\AddRotation.cshtml"
    using HST.Art.Core;
    
    #line default
    #line hidden
    
    #line 5 "..\..\Areas\manage\Views\Home\AddRotation.cshtml"
    using HST.Art.Web;
    
    #line default
    #line hidden
    
    #line 7 "..\..\Areas\manage\Views\Home\AddRotation.cshtml"
    using HST.Utillity;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/manage/Views/Home/AddRotation.cshtml")]
    public partial class _Areas_manage_Views_Home_AddRotation_cshtml : System.Web.Mvc.WebViewPage<RotationChartViewModel>
    {
        public _Areas_manage_Views_Home_AddRotation_cshtml()
        {
        }
        public override void Execute()
        {
            
            #line 1 "..\..\Areas\manage\Views\Home\AddRotation.cshtml"
  
    Layout = null;

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n");

            
            #line 10 "..\..\Areas\manage\Views\Home\AddRotation.cshtml"
Write(Scripts.Render("~/bundles/saos"));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

            
            #line 11 "..\..\Areas\manage\Views\Home\AddRotation.cshtml"
Write(Scripts.Render("~/bundles/ajaxAsync"));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

            
            #line 12 "..\..\Areas\manage\Views\Home\AddRotation.cshtml"
Write(Scripts.Render("~/bundles/validate"));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

            
            #line 13 "..\..\Areas\manage\Views\Home\AddRotation.cshtml"
Write(Styles.Render("~/appcss"));

            
            #line default
            #line hidden
WriteLiteral("\r\n<script");

WriteAttribute("src", Tuple.Create(" src=\"", 274), Tuple.Create("\"", 330)
, Tuple.Create(Tuple.Create("", 280), Tuple.Create<System.Object, System.Int32>(Href("~/Content/lib/webuploader/0.1.5/webuploader.min.js")
, 280), false)
);

WriteLiteral(" type=\"text/javascript\"");

WriteLiteral("></script>\r\n<style");

WriteLiteral(" type=\"text/css\"");

WriteLiteral(">\r\n    .form-horizontal .form-label {\r\n        text-align: right;\r\n    }\r\n</style" +
">\r\n<article");

WriteLiteral(" class=\"page-container\"");

WriteLiteral(">\r\n");

            
            #line 21 "..\..\Areas\manage\Views\Home\AddRotation.cshtml"
    
            
            #line default
            #line hidden
            
            #line 21 "..\..\Areas\manage\Views\Home\AddRotation.cshtml"
     using (Ajax.BeginForm("AddRotation", "Home", new { }, new AjaxOptions() { HttpMethod = "Post", OnSuccess = "formSuccess(data)", OnBegin = "disSubmit('sub_btn')", OnComplete = "enSubmit('sub_btn')" }, new { id = "", @class = "form form-horizontal" }))
    {
        
            
            #line default
            #line hidden
            
            #line 23 "..\..\Areas\manage\Views\Home\AddRotation.cshtml"
   Write(Html.AntiForgeryToken());

            
            #line default
            #line hidden
            
            #line 23 "..\..\Areas\manage\Views\Home\AddRotation.cshtml"
                                
        
            
            #line default
            #line hidden
            
            #line 24 "..\..\Areas\manage\Views\Home\AddRotation.cshtml"
   Write(Html.HiddenFor(m => m.Id, new { @Value = 0 }));

            
            #line default
            #line hidden
            
            #line 24 "..\..\Areas\manage\Views\Home\AddRotation.cshtml"
                                                      

            
            #line default
            #line hidden
WriteLiteral("        <input");

WriteLiteral(" type=\"hidden\"");

WriteLiteral(" name=\"RotationType\"");

WriteAttribute("value", Tuple.Create(" value=\"", 908), Tuple.Create("\"", 937)
            
            #line 25 "..\..\Areas\manage\Views\Home\AddRotation.cshtml"
, Tuple.Create(Tuple.Create("", 916), Tuple.Create<System.Object, System.Int32>(ViewBag.RotationType
            
            #line default
            #line hidden
, 916), false)
);

WriteLiteral(" />\r\n");

WriteLiteral("        <div");

WriteLiteral(" class=\"row cl\"");

WriteLiteral(">\r\n            <label");

WriteLiteral(" class=\"form-label col-xs-3 \"");

WriteLiteral("><span");

WriteLiteral(" class=\"c-red\"");

WriteLiteral(">*</span>轮播图片：</label>\r\n            <div");

WriteLiteral(" class=\"formControls col-xs-8 \"");

WriteLiteral(">\r\n                <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n                    <div");

WriteLiteral(" id=\"upfileImg\"");

WriteLiteral(" class=\"up-img-left\"");

WriteLiteral(">\r\n                        上传图片\r\n                    </div>\r\n                    " +
"<span");

WriteLiteral(" class=\"help-block help-tip help-img\"");

WriteLiteral(" style=\"top:8px;position:absolute\"");

WriteLiteral(">请上传jpg、jpeg、gif、bmp、png等图片类型的文件</span>\r\n                    <div");

WriteLiteral(" class=\"formControls\"");

WriteLiteral(">\r\n                        <ul");

WriteLiteral(" id=\"imglist\"");

WriteLiteral(" class=\"\"");

WriteLiteral("></ul>\r\n                        <input");

WriteLiteral(" type=\"hidden\"");

WriteLiteral(" id=\"loc_temp_img\"");

WriteLiteral(" />\r\n                    </div>\r\n                </div>\r\n");

WriteLiteral("                ");

            
            #line 39 "..\..\Areas\manage\Views\Home\AddRotation.cshtml"
           Write(Html.ValidationMessageFor(m => m.HeadImg, null, new { @class = "error-lable" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                ");

            
            #line 40 "..\..\Areas\manage\Views\Home\AddRotation.cshtml"
           Write(Html.TextBoxFor(m => m.HeadImg, new { id = "memberFileImg", @class = "hide-area input-text" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n            </div>\r\n        </div>\r\n");

WriteLiteral("        <div");

WriteLiteral(" class=\"row cl\"");

WriteLiteral(">\r\n            <label");

WriteLiteral(" class=\"form-label col-xs-3 \"");

WriteLiteral("><span");

WriteLiteral(" class=\"c-red\"");

WriteLiteral(">*</span>链接地址：</label>\r\n            <div");

WriteLiteral(" class=\"formControls col-xs-8 \"");

WriteLiteral(">\r\n");

WriteLiteral("                ");

            
            #line 46 "..\..\Areas\manage\Views\Home\AddRotation.cshtml"
           Write(Html.TextAreaFor(m => m.WebLink, new { @class = "textarea", rows = "3", placeholder = "请输入链接地址..." }));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                ");

            
            #line 47 "..\..\Areas\manage\Views\Home\AddRotation.cshtml"
           Write(Html.ValidationMessageFor(m => m.WebLink, null, new { @class = "error-lable" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n            </div>\r\n        </div>\r\n");

WriteLiteral("        <div");

WriteLiteral(" class=\"row cl\"");

WriteLiteral(">\r\n            <div");

WriteLiteral(" id=\"btn_submit\"");

WriteLiteral(" class=\"col-xs-6  col-xs-offset-3 col-sm-offset-2 mt-10\"");

WriteLiteral(">\r\n                <button");

WriteLiteral(" class=\"btn btn-primary radius\"");

WriteLiteral(" type=\"submit\"");

WriteLiteral(" id=\"sub_btn\"");

WriteLiteral(">保存</button>\r\n                <input");

WriteLiteral(" name=\"\"");

WriteLiteral(" type=\"button\"");

WriteLiteral(" onclick=\"layer_close();\"");

WriteLiteral(" class=\"btn btn-default radius \"");

WriteLiteral(" value=\"取消\"");

WriteLiteral(">\r\n            </div>\r\n        </div>\r\n");

            
            #line 56 "..\..\Areas\manage\Views\Home\AddRotation.cshtml"
    }

            
            #line default
            #line hidden
WriteLiteral("</article>\r\n\r\n<script");

WriteLiteral(" type=\"text/javascript\"");

WriteLiteral(@">
    function formSuccess(result) {
        if (result != null) {
            if (result.isSuccess) {
                var index = parent.layer.getFrameIndex(window.name);
                layer.alert('保存成功！', {
                    icon: 6,
                    closeBtn: 0,
                    yes: function () {
                        if (parent[1] != null) {
                            parent[pageIndex()].table1.ajax.reload();
                        } else {
                            parent.table1.ajax.reload();
                        }
                        parent.layer.close(index);
                    }
                });
            } else
                layer.alert(result.message, { icon: 5 });//icon:6为开心图
        } else
            layer.alert('保存失败！', { icon: 5 });//icon:6为开心图
    }
</script>

<script");

WriteLiteral(" type=\"text/javascript\"");

WriteLiteral(@">
    var maxcount = 8;
    //上传参数
    var jq_imgFormData = { BidFileDomain: ' ', uploadQueryData: { Suffix: 1 } };
    if (!WebUploader.Uploader.support()) {
        $("".FJsp2"").html(""说明：当前浏览器不支持附件上传"");
        $(""#upfileImg"").hide();
        //alert( 'Web Uploader 不支持您的浏览器！如果你使用的是IE浏览器，请尝试升级 flash 播放器');
        alert(""当前浏览器不支持附件上传，如果你使用的是IE浏览器，请尝试升级 flash 播放器"");
        throw new Error('WebUploader does not support the browser you are using.');
    }
    var BASE_URL = ""/content/webuploader-0.1.5"";
    var file_ids = []; //已上传文件的路径
    var file_info = []; //记录文件的GUID和存储路径
    var isfilesuping = false;
    // 文件上传
    jQuery(function () {
        var id = ""upfileImg"";
        var $ = jQuery,
    state = 'pending',
    $probar = $(""#"" + id).siblings(""div"").find("".pros""),
    uploader;
        uploader = WebUploader.create({
            // 不压缩image
            resize: false,
            //发送后台时参数
            formData: jq_imgFormData.uploadQueryData,
            fileVal: jq_imgFormData.BidFileDomain,
            method: ""POST"",
            //是否分块（大文件上传）
            chunked: true,
            //设置文件上传域名称
            fileVal: 'file',
            //每块最大限制（默认5M 因为是.net 所以设置2M）
            chunkSize: 2097152,
            // swf文件路径
            swf: BASE_URL + '/Uploader.swf',
            // 文件接收服务端。
            server: jq_imgFormData.BidFileDomain + '");

            
            #line 121 "..\..\Areas\manage\Views\Home\AddRotation.cshtml"
                                               Write(Url.Action("Upload", "Upload"));

            
            #line default
            #line hidden
WriteLiteral("\',\r\n            //创建选择文件按钮\r\n            pick: { id: \"#\" + id, innerHTML: \"\", mult" +
"iple: false },\r\n            //选择文件自动上传\r\n            auto: true,\r\n            //a" +
"ccept:{title:\"只能上传rar,zip文件\",extensions :\'rar,zip\',mimeTypes:\'application/x-zip-" +
"compressed\'},\r\n            fileNumLimit: maxcount\r\n        });\r\n\r\n        //文件大小" +
"限制,超过则不加入队列\r\n        uploader.on(\'beforeFileQueued\', function (file, han, msg) {" +
"\r\n            var xzlist = (\"jpg,jpeg,gif,bmp,png\").split(\",\");\r\n            var" +
" filenamelist = file.name.split(\'.\');\r\n            var filehz = filenamelist[fil" +
"enamelist.length - 1];\r\n\r\n            if (xzlist.indexOf(filehz) < 0) {\r\n       " +
"         layer.alert(\"请上传指定类型格式的文件\", 0);\r\n                return false;\r\n       " +
"     }\r\n\r\n            if (file.size > (1024 * 1024 * 10)) {\r\n                lay" +
"er.alert(\"图片大小不可超过5M\", 0);\r\n                return false;\r\n            }\r\n\r\n    " +
"        var filelen = $(\"#imglist>li\").length;\r\n            if (filelen >= maxco" +
"unt) {\r\n                layer.alert(\"超出最大上传数量\", 0);\r\n                return fals" +
"e;\r\n            }\r\n        });\r\n        //某个文件开始上传前触发，一个文件只会触发一次。\r\n        uploa" +
"der.on(\'uploadStart\', function (file) {\r\n            uploader.options.formData.g" +
"uid = WebUploader.Base.guid();\r\n        });\r\n        // 当有文件添加进来的时候\r\n        upl" +
"oader.on(\'fileQueued\', function (file) {\r\n            $(\".help-img\").hide();\r\n  " +
"          $(\".yangli\").hide()\r\n            $(\".yulan\").show();\r\n            uplo" +
"ader.makeThumb(file, function (error, ret) {\r\n                var file_list = []" +
";\r\n                file_list.push(\"<li id=\'\" + file.id + \"\'>\");\r\n               " +
" file_list.push(\"<img src=\'\" + ret + \"\'/>\");\r\n                file_list.push(\"<d" +
"iv class=\'percent_small\'>\");\r\n                file_list.push(\"<div class=\'per_pi" +
"c2\'>\");\r\n                file_list.push(\"<div class=\'change_per2\' style=\'width:1" +
"%\'></div>\");\r\n                file_list.push(\"</div>\");\r\n                file_li" +
"st.push(\"<span class=\'showtitle\'>正准备上传…</span></div>\");\r\n                file_li" +
"st.push(\"</div></li>\");\r\n                $(\"#imglist\").empty().append(file_list." +
"join(\'\'));\r\n            });\r\n        });\r\n        //重置\r\n        uploader.on(\'res" +
"et\', function (file) {\r\n\r\n        });\r\n        // 文件上传过程中创建进度条实时显示。\r\n        upl" +
"oader.on(\'uploadProgress\', function (file, percentage) {\r\n            var jd = p" +
"arseInt(percentage * 100);\r\n            if (jd > 1) {\r\n                $(\"#\" + f" +
"ile.id).find(\".change_per2\").css(\"width\", jd + \'%\');\r\n                $(\"#\" + fi" +
"le.id).find(\".per_pic2,.showtitle\").attr(\"title\", \'已上传\' + jd + \'%\');\r\n          " +
"      $(\"#\" + file.id).find(\".showtitle\").text(\'已上传\' + jd + \'%\');\r\n            }" +
"\r\n            if (jd >= 100) {\r\n                $(\"#\" + file.id).find(\".per_pic2" +
",.showtitle\").attr(\"title\", \"处理中...\");\r\n            }\r\n            isfilesuping " +
"= uploader.getStats().progressNum != 0;\r\n        });\r\n\r\n        uploader.on(\'upl" +
"oadSuccess\', function (file, obj) {\r\n\r\n            isfilesuping = uploader.getSt" +
"ats().progressNum != 0;\r\n            var jsonresult = obj._raw;\r\n            jso" +
"nresult = eval(\'(\' + jsonresult + \')\');\r\n            if (jsonresult.FilePath) {\r" +
"\n                $(\"#\" + file.id).find(\".showtitle\").text(\"\");\r\n                " +
"$(\"#memberFileImg,#loc_temp_img\").val(jsonresult.FilePath);\r\n                $(\"" +
"#memberFileImg\").blur();\r\n                $(\"#\" + file.id).find(\"span.title\").ht" +
"ml(jsonresult.FileName);\r\n                $(\"#upfileImg\").find(\"div.webuploader-" +
"pick\").html(\"更改图片\");\r\n                $(\"#upfileImg\").parent().attr(\"style\", \"po" +
"sition:relative\");//\r\n                $(\"#upfileImg\").addClass(\"up-imgBtn\");\r\n  " +
"          }\r\n            else {\r\n                layer.msg(jsonresult.Message);\r" +
"\n            }\r\n\r\n            uploader.reset();\r\n        });\r\n\r\n        uploader" +
".on(\'uploadError\', function (file, reason) {\r\n            layer.msg(reason);\r\n  " +
"      });\r\n        uploader.on(\'error\', function (handler) {\r\n            if (ha" +
"ndler == \"Q_EXCEED_NUM_LIMIT\") {\r\n                layer.alert(\"超出最大张数\");\r\n      " +
"      }\r\n            if (handler == \"F_DUPLICATE\") {\r\n                layer.aler" +
"t(\"该文件已在上传列表\", 3);\r\n            }\r\n\r\n            if (handler == \"Q_TYPE_DENIED\")" +
" {\r\n                layer.alert(\"该文件不满足上传要求，可能您上传的文件为0KB\");\r\n            }\r\n    " +
"    });\r\n        uploader.on(\'uploadComplete\', function (file) {\r\n            $(" +
"\'#\' + file.id).find(\'.percent_small\').fadeOut();\r\n        });\r\n\r\n    });\r\n\r\n</sc" +
"ript>");

        }
    }
}
#pragma warning restore 1591
