//修改密码
function showChangePswd() {
jQuery(function($) {
	$("#mainTitle").html("修改密码");
	$("#mainContext").load(contextPath()+"/changePswd.jsp");
});
}

/**
 * 所有公告
 * @param showPage 显示第几页
 */
function showNotice(showPage) {
jQuery(function($) {
	$("#mainTitle").html("所有公告");
	var page = 1;
	if(showPage!=null)
	{
		page = showPage;
	}
	$("#mainContext").load(contextPath()+"/userAction.do?method=mineNotice&pageCute="+page+"&timer="+Math.random());
});
}

/**
 * 我的上传
 * @param showPage 显示第几页
 */
function mineUpload(showPage) {
jQuery(function($) {
	$("#mainTitle").html("我的上传");
	var page = 1;
	if(showPage!=null)
	{
		page = showPage;
	}
	$("#mainContext").load(contextPath()+"/userAction.do?method=uploadFileShow&pageCute="+page+"&timer="+Math.random());
});
}

//修改密码
function changePswdSubmit() {
jQuery(function($) {
	if(getTrimLen($("#oldpassword").val())<6 || getTrimLen($("#oldpassword").val())>20)
	{
		showMsgObj('changePswdMsg', '旧密码长度在[6,20]字节以内！', 2, 'oldpassword');
	}
	else if(getTrimLen($("#newpassword").val())<6 || getTrimLen($("#newpassword").val())>20)
	{
		showMsgObj('changePswdMsg', '新密码长度在[6,20]字节以内！', 2, 'newpassword');
	}
	else if($("#newpassword").val() != $("#repassword").val())
	{
		showMsgObj('changePswdMsg', '两次密码输入不一致！', 2, 'repassword');
	}
	else
	{
		showMsgObj('changePswdMsg', '正在处理，请稍后...', 3);
		$.ajax({
			url:contextPath()+'/servletAction.do?method=userPswdMdf',
			type: 'post',
			dataType: 'json',
			cache: false,
			async: false,
			data: {"loginPswd":$("#newpassword").val(),"oldPswd":$("#oldpassword").val()},
			success:function(res){
				if(res != null)
				{
					if(res.retCode!=0)
					{
						showMsgObj('changePswdMsg', res.msg, 1);
					}
					else
					{
						showMsgObj('changePswdMsg', res.msg, 0);
						$("#oldpassword").val("");
						$("#newpassword").val("");
						$("#repassword").val("");
					}
				}
				else{
					showMsgObj('changePswdMsg', '请求失败，返回结果null', 1);
				}
			},
			error:function(){
				showMsgObj('changePswdMsg', '请求失败 error function', 1);
			}
		});
	}
});
	return false;
}

