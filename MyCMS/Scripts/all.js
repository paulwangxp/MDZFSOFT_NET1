$(document).ready(function () {

    //保证页面最小高度大于等于屏幕高度
    if ($("#body").height() < $(document).height()) {
        $("body").css("height", $(document).height());
    }

    $(".mod_hd").click(function () {
        $(this).toggleClass("bor").siblings(".inner_bor").toggle();
    });
    //table graybg
    $(".Js_grayBg  tr").each(function () {
        var _this = $(this)
        _this.hover(function () {
            _this.find("td").toggleClass("hoverbg")
        })
    });
    //tab
    $(".Js_tab > li").each(function (index) {
        var _this = $(this)
        _this.click(function () {
            _this.addClass("current").siblings("li").removeClass("current")
            $(".tab_conitem").eq(index).addClass("current").siblings("li").removeClass("current")
        })
    })
    //police list
    $(".Js_ShowPolice > li > .police_hd").each(function () {
        var _this = $(this)
        var _thisSib = $(this).siblings("ul")
        _this.click(function () {
            _thisSib.toggle();
        })
    })

    if ($.datepicker) {
        $.datepicker.regional['zh_CN'] = {
            closeText: '关闭',
            prevText: '&#x3c;上月',
            nextText: '下月&#x3e;',
            currentText: '今天',
            monthNames: ['一月', '二月', '三月', '四月', '五月', '六月',
		'七月', '八月', '九月', '十月', '十一月', '十二月'],
            monthNamesShort: ['01', '02', '03', '04', '05', '06',
		'07', '08', '09', '10', '11', '12'],
            dayNames: ['星期日', '星期一', '星期二', '星期三', '星期四', '星期五', '星期六'],
            dayNamesShort: ['周日', '周一', '周二', '周三', '周四', '周五', '周六'],
            dayNamesMin: ['日', '一', '二', '三', '四', '五', '六'],
            dateFormat: 'yy-mm-dd', firstDay: 1,
            isRTL: false
        };
    }



    //data picker
    //var locale = "<s:property value='#request.locale'/>";//struts2取语言环境
    //var locale = "<%=request.getLocale()%>"; //jsp取浏览器语言环境
    //datePicker('#dateDemo',locale);//根据语言环境切换日期控件语言
    if ($("#begainDate").length > 0) {
        datePicker('#begainDate', 'zh_CN');
    }
    if ($("#endDate").length > 0) {
        datePicker('#endDate', 'zh_CN');
    }
    if ($("#UploadBeginDate").length > 0) {
        datePicker('#UploadBeginDate', 'zh_CN')
    }
    if ($("#UploadendDate").length > 0) {

        datePicker('#UploadendDate', 'zh_CN')
    }
    //datePicker('#dateDemo',''); //''默认的样式在ui.datepicker.js内已定义为英文样式，与附件内的ui.datepicker-en_US.js相同
    $(".dialog-mask").css("opacity", "0.6")

    $(".text-box").addClass("input_130x20");
})

function showLocale(objD) {
    var str, colorhead, colorfoot;
    var yy = objD.getYear();
    if (yy < 1900) yy = yy + 1900;
    var MM = objD.getMonth() + 1;
    if (MM < 10) MM = '0' + MM;
    var dd = objD.getDate();
    if (dd < 10) dd = '0' + dd;
    var hh = objD.getHours();
    if (hh < 10) hh = '0' + hh;
    var mm = objD.getMinutes();
    if (mm < 10) mm = '0' + mm;
    var ss = objD.getSeconds();
    if (ss < 10) ss = '0' + ss;
    var ww = objD.getDay();
    if (ww == 0) colorhead = "";
    if (ww > 0 && ww < 6) colorhead = "";
    if (ww == 6) colorhead = "";
    if (ww == 0) ww = "星期日";
    if (ww == 1) ww = "星期一";
    if (ww == 2) ww = "星期二";
    if (ww == 3) ww = "星期三";
    if (ww == 4) ww = "星期四";
    if (ww == 5) ww = "星期五";
    if (ww == 6) ww = "星期六";
    colorfoot = "</font>"
    str = colorhead + yy + "年" + MM + "月" + dd + "日" + hh + ":" + mm + ":" + ss + "  ";
    return (str);
}
function tick() {
    var today;
    today = new Date();
    if (document.getElementById("localtime") != null) {
        document.getElementById("localtime").innerHTML = showLocale(today);

    }
    window.setTimeout("tick()", 1000);
}
tick();


function datePicker(pickerName, locale) {
    $(pickerName).datepicker($.datepicker.regional[locale]);
    $(pickerName).datepicker('option', 'changeMonth', true); //月份可调
    $(pickerName).datepicker('option', 'changeYear', true); //年份可调
}


/**
* 获取信息div
* @param objId 显示操作的id
* @param msg 提示信息
* @param type 提示类型 0-success 1-error 2-warning 3-information
* @param focusId 焦点停留id名称
*/
function showMsg(msg) {
    jQuery(function ($) {
        $("#fileManagerAddMsg").html(msg);
        $("#fileManagerAddMsg").attr("class", "warning msg");
        $("#fileManagerAddMsg").css("opacity", 4);
        showObj("fileManagerAddMsg");
        $("body").css("height", $(document).height() + $("#fileManagerAddMsg").height()+30);
    });
}

/**
* 显示信息
* @param objId 显示操作的id
* @param msg 提示信息
* @param type 提示类型 0-success 1-error 2-warning 3-information
* @param focusId 焦点停留id名称
*/
function showInformation(msg) {
    jQuery(function ($) {
        $("#fileManagerAddInformation").html(msg);
        $("#fileManagerAddInformation").attr("class", "information msg");
        $("#fileManagerAddInformation").css("opacity", 4);
        showObj("fileManagerAddInformation");
        $("body").css("height", $(document).height() + $("#fileManagerAddMsg").height() + 30);
    });
}