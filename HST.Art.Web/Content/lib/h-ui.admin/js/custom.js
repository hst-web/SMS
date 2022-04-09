/*自定义脚本*/

//去除字符串最后一个符号
function trimEnd(data) {
    if (data.length > 0) {
        return data.substr(0, data.length - 1);
    }

    return data;
}

//验证url地址
function IsURL(str_url) {
    var strRegex = "^(https?|ftp|file)://[-A-Za-z0-9+&@#/%?=~_|!:,.;]+[-A-Za-z0-9+&@#/%=~_|]$";
    var re = new RegExp(strRegex);
    if (re.test(str_url)) {
        return true;
    } else {
        return false;
    }
}

//验证邮箱
function validEmail(email) {
    return /^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/.test(email);
}

//验证固话，手机号码，400热线合法返回true,反之,返回false
function IsTelephone(value) {
    var isPhone = /^([0-9]{3,4}-)?[0-9]{7,8}$/;
    var isMob = /^((\+?86)|(\(\+86\)))?(13[012356789][0-9]{8}|15[012356789][0-9]{8}|18[02356789][0-9]{8}|147[0-9]{8}|1349[0-9]{7})$/;
    var isServer = /^400[016789]\d{6}$/;
    if (isMob.test(value) || isPhone.test(value) || isServer.test(value)) {
        return true;
    }
    else {
        return false;
    }
}

//初始化check、radio样式
function initUniform() {
    if (!$().uniform) {
        return;
    }
    var uncheck = $("input[type=checkbox]:not(.toggle,.magic-checkbox, .md-check, .md-radiobtn, .make-switch, .icheck), input[type=radio]:not(.toggle, .md-check, .md-radiobtn, .star, .make-switch, .icheck)");
    if (uncheck.size() > 0) {
        uncheck.each(function () {
            if ($(this).parents(".checker").size() === 0) {
                $(this).show();
                $(this).uniform();
            }
        });
    }
}

//字符串处理
function StringBuffer(str) {
    var arr = [];
    str = str || "";
    var size = 0;  // 存放数组大小
    arr.push(str);
    // 追加字符串
    this.append = function (str1) {
        arr.push(str1);
        return this;
    };
    // 返回字符串
    this.toString = function () {
        return arr.join("");
    };
    // 清空
    this.clear = function (key) {
        size = 0;
        arr = [];
    }
    // 返回数组大小
    this.size = function () {
        return size;
    }
    // 返回数组
    this.toArray = function () {
        return buffer;
    }
    // 倒序返回字符串
    this.doReverse = function () {
        var str = buffer.join('');
        str = str.split('');
        return str.reverse().join('');
    }
};

function handleFancybox() {
    if (!jQuery.fancybox) {
        return;
    }

    if ($(".fancybox").size() > 0) {
        $(".fancybox").fancybox({
            buttons: [
                'close',
                'arrowLeft',
                'arrowRight'
            ]
        });
    }
};

//改变数量
function changecount(input, type, min) {
    var ex = /^\d+$/;
    if (type > 1) {
        ex = /^[0-9]+([.]{1}[0-9]{1,2})?$/;
    }

    if ($(input).val() < min || !ex.test($(input).val())) {
        $(input).val("" + min + "");
    }
}


function showLoading() {
    $(".dataTables_processing").attr("style", "display:block")
}


function hideLoading() {
    $(".dataTables_processing").attr("style", "display: none;")
}

function showException() {
    layer.alert('操作数据请求失败，请稍后再试', { icon: 2 });
}

function disSubmit(id, title) {
    title = title || "正在提交...";
    var obj = $("#" + id);
    var hisVal = "";
    if (obj[0].tagName.toLowerCase() == "input") {
        hisVal = obj.val();
        obj.val(title).css("opacity", "0.7").attr("disabled", "disabled");
    } else {
        hisVal = obj.html();
        obj.html(title).css("opacity", "0.7").attr("disabled", "disabled");
    }

    obj.attr("data-bind", hisVal);
}

function enSubmit(id, title) {
    var obj = $("#" + id);
    var hisVal = obj.attr("data-bind");
    title = title || hisVal;
    if (obj[0].tagName.toLowerCase() == "input") {
        obj.val(title).removeAttr("style").removeAttr("disabled");
    } else {
        obj.html(title).removeAttr("style").removeAttr("disabled");
    }
}

/*删除*/
function obj_del(title, action, id) {
    top.layer.confirm('确认要删除该' + title + '吗？', function (e) {
        $.ajax({
            url: action,
            type: "post",
            data: { id: id },
            success: function (data) {
                if (data.isSuccess) {
                    top.layer.alert('操作成功！', {
                        icon: 6,
                        closeBtn: 0,
                        yes: function () {
                            if (parent[1] != null) {
                                parent[pageIndex()].table1.ajax.reload();
                            } else if (parent.table1 != null) {
                                parent.table1.ajax.reload();
                            } else {
                                parent[0].table1.ajax.reload();
                            }

                            parent.layer.closeAll();
                        }
                    });
                } else {
                    top.layer.alert('操作失败！', { icon: 5 });
                }
            },
            error: function (data) { top.layer.alert('操作失败！', { icon: 5 }); }
        })
    });
}


/*上架*/
function obj_publish(action, id) {
    top.layer.confirm('确认要上架吗？', function (e) {
        $.ajax({
            url: action,
            type: "post",
            data: { id: id },
            success: function (data) {
                if (data.isSuccess) {
                    top.layer.alert('操作成功！', {
                        icon: 6,
                        closeBtn: 0,
                        yes: function () {
                            if (parent[1] != null) {
                                parent[pageIndex()].table1.ajax.reload();
                            } else if (parent.table1 != null) {
                                parent.table1.ajax.reload();
                            } else {
                                parent[0].table1.ajax.reload();
                            }
                            parent.layer.closeAll();
                        }
                    });
                } else {
                    top.layer.alert('操作失败！', { icon: 5 });
                }
            },
            error: function (data) { top.layer.alert('操作失败！', { icon: 5 }); }
        })
    });
}

/*下架*/
function obj_shelves(action, id) {
    top.layer.confirm('确认要下架吗？', function (e) {
        $.ajax({
            url: action,
            type: "post",
            data: { id: id },
            success: function (data) {
                if (data.isSuccess) {
                    top.layer.alert('操作成功！', {
                        icon: 6,
                        closeBtn: 0,
                        yes: function () {
                            if (parent[1] != null) {
                                parent[pageIndex()].table1.ajax.reload();
                            } else if (parent.table1 != null) {
                                parent.table1.ajax.reload();
                            } else {
                                parent[0].table1.ajax.reload();
                            }

                            parent.layer.closeAll();
                        }
                    });
                } else {
                    top.layer.alert('操作失败！', { icon: 5 });
                }
            },
            error: function (data) { top.layer.alert('操作失败！', { icon: 5 }); }
        })
    });
}

/*详情*/
function obj_detail(title, url, id) {
    var index = top.layer.open({
        type: 2,
        title: title,
        area: ['800px', '600px'],
        content: url + "?id=" + id
    });
}

function pageIndex() {
    return window.parent.$("#min_title_list").find("li.active").index();
}


function nofind() {
    var img = event.srcElement;
    img.src = "/Content/image/not-img.jpg"; //替换的图片
    img.onerror = null; //控制不要一直触发错误
}

var loading = function () {
    var assetsPath = '/Content/';
    var globalImgPath = 'image/';

    return {
        getGlobalImgPath: function () {
            return assetsPath + globalImgPath;
        },
        // wrMetronicer function to  block element(indicate loading)
        blockUI: function (options) {
            options = $.extend(true, {}, options);
            var html = '';
            if (options.animate) {
                html = '<div class="loading-message ' + (options.boxed ? 'loading-message-boxed' : '') + '">' + '<div class="block-spinner-bar"><div class="bounce1"></div><div class="bounce2"></div><div class="bounce3"></div></div>' + '</div>';
            } else if (options.iconOnly) {
                html = '<div class="loading-message ' + (options.boxed ? 'loading-message-boxed' : '') + '"><img src="' + this.getGlobalImgPath() + 'loading-spinner-grey.gif" align=""></div>';
            } else if (options.textOnly) {
                html = '<div class="loading-message ' + (options.boxed ? 'loading-message-boxed' : '') + '"><span>&nbsp;&nbsp;' + (options.message ? options.message : '加载中...') + '</span></div>';
            } else {
                html = '<div class="loading-message ' + (options.boxed ? 'loading-message-boxed' : '') + '"><img src="' + this.getGlobalImgPath() + 'loading-spinner-grey.gif" align=""><span>&nbsp;&nbsp;' + (options.message ? options.message : '加载中...') + '</span></div>';
            }

            if (options.target) { // element blocking
                var el = $(options.target);
                if (el.height() <= ($(window).height())) {
                    options.cenrerY = true;
                }
                el.block({
                    message: html,
                    baseZ: options.zIndex ? options.zIndex : 1000,
                    centerY: options.cenrerY !== undefined ? options.cenrerY : false,
                    css: {
                        top: '10%',
                        border: '0',
                        padding: '0',
                        backgroundColor: 'none'
                    },
                    overlayCSS: {
                        backgroundColor: options.overlayColor ? options.overlayColor : '#555',
                        opacity: options.boxed ? 0.05 : 0.1,
                        cursor: 'wait'
                    }
                });
            } else { // page blocking
                $.blockUI({
                    message: html,
                    baseZ: options.zIndex ? options.zIndex : 1000,
                    css: {
                        border: '0',
                        padding: '0',
                        backgroundColor: 'none'
                    },
                    overlayCSS: {
                        backgroundColor: options.overlayColor ? options.overlayColor : '#555',
                        opacity: options.boxed ? 0.05 : 0.1,
                        cursor: 'wait'
                    }
                });
            }
        },

        // wrMetronicer function to  un-block element(finish loading)
        unblockUI: function (target) {
            if (target) {
                $(target).unblock({
                    onUnblock: function () {
                        $(target).css('position', '');
                        $(target).css('zoom', '');
                    }
                });
            } else {
                $.unblockUI();
            }
        }
    }
}();

function loadPage(title) {
    var loadingIndex = layer.load(2, { //icon支持传入0-2
        shade: [0.5, 'gray'], //0.5透明度的灰色背景
        content: '<p>' + title + '</p>',
        success: function (layero) {
            layero.find('.layui-layer-content').css({
                'padding-top': '39px',
                'width': '140px',
                'background-color': '#fff',
                'background-position': 'left 15% center',
                'text-align': 'center',
                'height': '20px'
            });
            layero.find('.layui-layer-content p').css({
                'float': 'left',
                'margin-left': '45%',
                'position': 'relative',
                'top': '-18px'
            });
        }
    });
}

function pasteImg(e) {
  
    var a = e.editor.document;
    var b = a.find("img");
    var count = b.count();
    if (count <= 0) {
        return false;
    }

    for (var i = 0; i < count; i++) {
        var src = b.getItem(i).$.src;//获取img的src
        if (src.substring(0, 10) == 'data:image') { //判断是否是二进制图像，是才处理
            var img1 = src.split(',')[1];
            var img2 = window.atob(img1);
            var ia = new Uint8Array(img2.length);
            for (var x = 0; x < img2.length; x++) {
                ia[x] = img2.charCodeAt(x);
            };
            //获得图片的类型
            var w1 = src.indexOf(":");//获得指定字符的第一个下标值
            var w2 = src.indexOf(";");
            var imgType = src.substring(w1 + 1, w2);//返回一个包含从 start 到最后（不包含 end ）的子字符串的字符串

            var blob = new Blob([ia], { type: imgType });
            var formdata = new FormData();
            formdata.append('imgfile', blob);

            $.ajax({
                type: "POST",
                url: "/manage/upload/uploadpasteimg",//服务器url
                async: false,//同步，因为修改编辑器内容的时候会多次调用change方法，所以要同步，否则会多次调用后台@Url.Action("UploadPasteImg", "Upload")
                data: formdata,
                processData: false,
                contentType: false,
                success: function (json) {
                    var imgurl = json.FilePath; //获取回传的图片url

                    //获取并更改到现有的图片标签src的值
                    b.getItem(i).$.src = imgurl;
                    b.getItem(i).$.setAttribute("data-cke-saved-src", imgurl);
                    //console.log("上传图片" + imgurl);
                    //var a = $(this).getElementsByTagName("img")[i]; //content为textarea的id
                    //console.log(a);
                    //a.setAttribute('data-cke-saved-src', imgurl);
                }
            });
        }
    }


}