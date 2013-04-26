/*****************
contextPath 获取工程根目录路径
loadPage 载入页面
showObj 显示对象
hideObj 隐藏对象
getArray 获取数组
getJqueryArrayStr 获取数组
userLogout 用户退出
selectRole 选择角色
closeSelectRole 关闭选择角色
selectTree 选择部门（单选）
closeSelectTree 关闭选择部门
chooseTree 选择部门（多选）
closeChooseTree 关闭选择部门
selectUser 选择警员
closeSelectUser 关闭选择警员
selectDateTime 选择日期时间
*****************/

/**
 * 获取工程根目录路径
 */
function contextPath()
{
	return "";
}
if(top.location != self.location)
{
	//self.location.href = contextPath()+"/filter.jsp?type=1";
}

/**
 * 获取工程根目录路径
 * @param objId 需要载入id名称
 * @param loadUrl 被载入的url
 * @param ifCache 如果为null 默认请求+时间戳
 */
function loadPage(objId, loadUrl, ifCache)
{
jQuery(function($) {
	if(ifCache==null)
	{
		if(loadUrl.indexOf("?")>=0)
		{
			loadUrl += "&loadtime="+Math.random();
		}
		else
		{
			loadUrl += "?loadtime="+Math.random();
		}
	}
	$("#"+objId).load(loadUrl);
});
}

/**
 * 显示提示信息对象
 * @param objId 对象id名称
 * @param msg 提示信息
 * @param type 提示类型 0-success 1-error 2-warning 3-information
 * @param focusId 焦点停留id名称
 * @param nospeed 关闭速度
 */
function showMsgObj(objId, msgContent, type, focusId, nospeed)
{
jQuery(function($) {
	var speed = 500;
	if(nospeed!=null)
	{
		speed = 0;
	}
	var msgType = "error";
	switch(type)
	{
		case 0 : msgType = "success"; break;
		case 1 : msgType = "error"; break;
		case 2 : msgType = "warning"; break;
		case 3 : msgType = "information"; break;
		default: msgType = "error";
	}
	$('#'+objId).html(msgContent);
	$('#'+objId).attr("class", msgType+" msg");
	$('#'+objId).css("opacity" ,4);
	$('#'+objId).show(speed,function(){
		$(this).css('display', 'block');
	});
	if(focusId != null)
	{
		$("#"+focusId).focus();
	}
});
}

/**
 * 显示对象
 * @param objId 对象id名称
 * @param nospeed 关闭速度
 */
function showObj(objId, nospeed)
{
jQuery(function($) {
	var speed = 500;
	if(nospeed!=null)
	{
		speed = 0;
	}
	$("#"+objId).show(speed,function(){
		$(this).css("display", "block");
	});
});
}

/**
 * 隐藏对象
 * @param objId 对象id名称
 * @param nospeed 关闭速度
 */
function hideObj(objId, nospeed)
{
jQuery(function($) {
	var speed = 1000;
	if(nospeed!=null)
	{
		speed = 0;
	}
	$("#"+objId).hide(speed,function(){
	});
});
}

/**
 * 获取数组
 * @param objClassName
 */
function getArray(objClassName)
{
var array = new Array();
jQuery(function($) {
	var arrayIndex = 0;
	$("."+objClassName).each(function(){
		if($(this).attr("checked"))
		{
			array[arrayIndex] = $(this).val();
			arrayIndex++;
		}
	});
});
return array;
}

/**
 * 获取数组
 * @param objClassName
 */
function getJqueryArrayStr(objClassName)
{
var jqueryArrayStr = "";
jQuery(function($) {
	var forIndex = 0;
	$("."+objClassName).each(function(){
		if($(this).attr("checked"))
		{
			if(forIndex==0)
			{
				jqueryArrayStr += $(this).val();
			}
			else
			{
				jqueryArrayStr += ","+$(this).val();
			}
			forIndex ++;
		}
	});
});
return jqueryArrayStr;
}

/**
 * 用户退出系统
 */
function userLogout()
{
	location.href = contextPath()+"/userAction.do?method=logout";
}

/**
 * 选择角色
 * @param assignmentId 需要赋值角色id的id对象名
 * @param assignmentName 需要赋值角色名称的id对象名
 * @param dialogTitle 弹出层的标题
 */
function selectRole(assignmentId, assignmentName, dialogTitle)
{
jQuery(function($) {
	var dialogUrl = contextPath()+"/userAction.do?method=roleSelect&assignmentId="+assignmentId+"&assignmentName="+assignmentName;
	if(dialogTitle==null)
	{
		dialogTitle = "请选择";
	}
	dialogOpen("selectRoleDiv", dialogUrl, 640, 460, dialogTitle);
});
}
function closeSelectRole()
{
jQuery(function($) {
	dialogClose("selectRoleDiv");
});
}

/**
 * 选择部门 单选
 * @param assignmentId 需要赋值部门id的id对象名
 * @param assignmentName 需要赋值部门名称的id对象名
 * @param dialogTitle 弹出层的标题
 */
function selectTree(assignmentId, assignmentName, dialogTitle, onlyRoot)
{
jQuery(function($) {
	$.weeboxs.open('#selectTreeDiv', {title:dialogTitle, contentType:'selector', width:'100px'});
});
}

function closeSelectTree()
{
jQuery(function($) {
	dialogClose("selectTreeDiv");
});
}

/**
 * 选择部门 多选
 * @param assignmentId 需要赋值部门id的id对象名
 * @param assignmentName 需要赋值部门名称的id对象名
 * @param dialogTitle 弹出层的标题
 */
function chooseTree(assignmentId, assignmentName, dialogTitle)
{
jQuery(function($) {
	if(dialogTitle==null)
	{
		dialogTitle = "请选择";
	}
	var dialogUrl = contextPath()+"/userAction.do?method=treeSelect&typeSelect=choose&assignmentId="+assignmentId+"&assignmentName="+assignmentName;
	dialogOpen("chooseTreeDiv", dialogUrl, 640, 460, dialogTitle);
});
}
function closeChooseTree()
{
jQuery(function($) {
	dialogClose("chooseTreeDiv");
});
}

/**
 * 选择警员
 * @param assignmentId 需要赋值警员id的id对象名
 * @param assignmentName 需要赋值警员名称的id对象名
 * @param dialogTitle 弹出层的标题
 * @param resetOpen 是否每次都是新打开
 */
function selectUser(assignmentId, assignmentName, dialogTitle, resetOpen)
{
jQuery(function($) {
	var dialogUrl = contextPath()+"/userAction.do?method=userSelect&assignmentId="+assignmentId+"&assignmentName="+assignmentName;
	if(dialogTitle==null)
	{
		dialogTitle = "请选择";
	}
	dialogOpen("selectUserDiv", dialogUrl, 640, 500, dialogTitle, resetOpen);
});
}
function closeSelectUser()
{
jQuery(function($) {
	dialogClose("selectUserDiv");
});
}

/**
 * 选择日期时间
 * @param assignmentId 需要赋值的id对象名
 */
function selectDateTime(assignmentId)
{
	var returnVal = window.showModalDialog(contextPath()+"/adddate.html","","dialogwidth:260px;dialogheight:310px; toolbar=no,top=200,left=200, menubar=no, location=no, status=no");
	if(returnVal!=null)
	{
jQuery(function($) {
		$("#"+assignmentId).val(returnVal);
});
	}
}

/**
 * 清空id对象
 * @param valId
 */
function clearVal(valId)
{
jQuery(function($) {
	$("#"+valId).val("");
});
}

/******************************* dialog部分 *********************************/
/**
 * 弹出层设置
 * @param dialogObjId
 * @param dialogWidth
 * @param dialogHeight
 */
function dialogInit(dialogObjId, dialogWidth, dialogHeight, dialogTitle)
{
jQuery(function($) {
	$(function(){
		$('#'+dialogObjId).dialog({
			/*toolbar:[{
				text:'Add',
				iconCls:'icon-add',
				handler:function(){
					alert('add')
				}
			},'-',{
				text:'Save',
				iconCls:'icon-save',
				handler:function(){
					alert('save')
				}
			}],*/
			title: dialogTitle,
			modal: true,
			width: dialogWidth,
			height: dialogHeight/*,
			buttons:[{
				text:'Ok',
				iconCls:'icon-ok',
				handler:function(){
					alert('ok');
				}
			},{
				text:'Cancel',
				handler:function(){
					$('#dd').dialog('close');
				}
			}]*/
		});
	});
});
}

/**
 * 打开弹出层
 * @param dialogObjId
 * @param dialogUrl
 * @param dialogWidth
 * @param dialogHeight
 * @param dialogTitle
 * @param resetOpen 每次都init
 */
function dialogOpen(dialogObjId, dialogUrl, dialogWidth, dialogHeight, dialogTitle, resetOpen)
{
jQuery(function($) {
	if(resetOpen!=null)
	{
		if($('#'+dialogObjId+"Iframe").length>0)
		{
			$('#'+dialogObjId+"Iframe").attr("src", dialogUrl);
			$('#'+dialogObjId+"Iframe").attr("width", dialogWidth-34);
			$('#'+dialogObjId+"Iframe").attr("height", dialogHeight-36);
		}
		else
		{
			$('#'+dialogObjId).load(dialogUrl);
		}
		$('#'+dialogObjId).css('display', 'block');
		dialogInit(dialogObjId, dialogWidth, dialogHeight, dialogTitle);
	}
	else
	{
		if($('#'+dialogObjId).css('display')=='none')
		{
			if($('#'+dialogObjId+"Iframe").length>0)
			{
				$('#'+dialogObjId+"Iframe").attr("src", dialogUrl);
				$('#'+dialogObjId+"Iframe").attr("width", dialogWidth-34);
				$('#'+dialogObjId+"Iframe").attr("height", dialogHeight-36);
			}
			else
			{
				$('#'+dialogObjId).load(dialogUrl);
			}
			$('#'+dialogObjId).css('display', 'block');
			dialogInit(dialogObjId, dialogWidth, dialogHeight, dialogTitle);
		}
		else
		{
			$('#'+dialogObjId).dialog('open');
		}
	}
});
}

/**
 * 关闭弹出层
 * @param dialogObjId
 */
function dialogClose(dialogObjId)
{
jQuery(function($) {
	$('#'+dialogObjId).dialog('close');
});
}

/******************************* 验证部分 *********************************/
/**
 * 验证邮箱格式
 */
function isEmail(strEmail) {
	var regex = /^\w+((-\w+)|(\.\w+))*\@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z0-9]+$/;
	return regex.test(strEmail);
}
/**
 * 返回字节长度 一个汉字长度为2
 */
function getLenB(str)
{
	return str.replace(/[^\x00-\xff]/g,"**").length;
}
/**
 * 返回去除空格的长度
 */
function getTrimLen(str)
{
	return $.trim(str).length;
}
/**
 * 验证验证码
 * @param code
 * @returns
 */
function isCheckCode(code) {
	var regex = /^\d{4}$/;
	return regex.test(code);
}

function toSelected(sltIdStr, sltVal)
{
jQuery(function($) {
	$('#'+sltIdStr+' option').each(function(){
		if($(this).val()==sltVal)
		{
			$(this).attr("selected", true);
		}
	});
});
}

function toChecked(checkClass, sltVal)
{
jQuery(function($) {
	$('.'+checkClass).each(function(){
		if($(this).val()==sltVal)
		{
			$(this).attr("checked",true);
		}
	});
});
}