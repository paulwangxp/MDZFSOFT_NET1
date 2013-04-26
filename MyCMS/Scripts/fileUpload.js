

function $BlockNone(objName, displayValue) {
    document.getElementById(objName).style.display = displayValue;
}

function $ALT(alertMsg) {
    //alert(alertMsg);
    showMsg(alertMsg);
}
function $DEBUG(alertMsg) {
    alert(alertMsg);
}

/**
* 获取当前盘符 如果没有安装过插件，跳转至安装插件的界面
*/
function getVersion() {
    var str = "";
    try {
        str = MDOCX.getVersion();
    }
    catch (ex) {
        $ALT("您还没有安装插件，请安装上传插件~");
        window.open(setupUrl);
        return str;
    }
    return str;
}

/**
* ftp连接设置
* @param ftpHost ftpIp地址
* @param ftpUser ftp连接的用户名
* @param ftpPswd ftp用户名对应的密码
* @param ftpPort ftp连接的端口
*/
function setParm(ftpHost, ftpUser, ftpPswd, ftpPort) {
    try {
        var str = MDOCX.setParm(ftpHost, ftpUser, ftpPswd, ftpPort);
    }
    catch (ex) {
        $ALT("ftp连接设置异常： " + ex.description);
    }
}

/**
* ftp登录
*/
function ftpLogin() {
    try {
        var str = MDOCX.ftpLogin();
        if (str != "1") {
            $ALT("ftp服务器登录失败，请与管理员联系");
            alert('login error');
            static_ftpLogin = false;
            ftpLogout();
        }
        else {
            static_ftpLogin = true;
        }
    }
    catch (ex) {
        //$ALT("ftp服务器登录异常 "+ex.description);
        $ALT("ftp服务器登录失败，请与管理员联系~ 失败原因：" + ex.description);
    }
}

/**
* ftp登出
*/
function ftpLogout() {
    try {
        var str = MDOCX.ftpLogout();
        if (str != "1") {
            $ALT("ftp服务器退出失败，请与管理员联系");
        }
    }
    catch (ex) {
        $ALT("ftp服务器退出异常 " + ex.description);
    }
}

/**
* ftp文件缓存设置
* @param valueMB 缓存大小 1即1MB缓存
*/
function ftpSetFileStepSize(valueMB) {
    try {
        var str = MDOCX.ftpSetFileStepSize(1024 * valueMB);
        if (str != "1") {
            $ALT("ftp文件缓存设置失败，请与管理员联系");
        }
    }
    catch (ex) {
        $ALT("ftp文件缓存设置异常 " + ex.description);
    }
}

/**
* 文件夹创建 创建完成后即进入到创建的目录级
* @param remoteDir 文件夹目录名
*/
function ftpCreateRemoteDir(remoteDir) {
    try {
        remoteDir = remoteDir + ',' + getCurrentDateString();//ftp目录格式  depid/userid/date
        ftpSetRemoteRoot();
        static_remoteFileName = "/" + remoteDir.replace(",", "/").replace(",", "/");
        saveDir = remoteDir;
        var dirArr = remoteDir.split(",");
        for (i = 0; i < dirArr.length; i++) {
            if (MDOCX.ftpCreateRemoteDir(dirArr[i]) == "0") {
                ftpSetRemoteDir(dirArr[i]);
            }
        }
        //var str =  MDOCX.ftpCreateRemoteDir(remoteDir);

        /*if(str!="1")
        {
        $ALT("文件夹创建失败，请与管理员联系~ 失败编码："+MDOCX.ftpGetErrorMsg());
        }*/
    }
    catch (ex) {
        $ALT("FTP目录创建失败 " + ex.description);
    }
}

/**
* 文件夹删除 包括文件夹下的所有文件 ***慎用***
* @param remoteDir 文件夹目录名
*/
function ftpDelRemoteDir() {
    try {
        var str = MDOCX.ftpDelRemoteDir(static_fileUrl);
    }
    catch (ex) {
        $ALT("info " + ex.description);
    }
}

/**
* 设置进入文件夹的位置 如果想进入"\test\test\",那么传入的参数为"test\\test"
* @param remoteDir 文件夹目录名
*/
function ftpSetRemoteDir(remoteDir) {
    try {
        var str = MDOCX.ftpSetRemoteDir(remoteDir);
        if (str != "1") {
            $ALT("设置进入文件夹的位置失败，请与管理员联系");
        }
    }
    catch (ex) {
        $ALT("设置进入文件夹的位置异常 " + ex.description);
    }
}

/**
* 设置进入ftp的根目录
* @param remoteDir 文件夹目录名
*/
function ftpSetRemoteRoot() {
    try {
        var str = MDOCX.ftpSetRemoteRoot();
        if (str != "1" && static_ftpLogin) {
            $ALT("设置进入ftp的根目录失败，请与管理员联系：");
        }
    }
    catch (ex) {
        $ALT("设置进入ftp的根目录异常 " + ex.description);
    }
}

/**
* 获取当前进入文件夹的位置 ***基本可以不用这个方法，此方法调试模式下用***
*/
function ftpGetRemoteDir() {
    try {
        var str = MDOCX.ftpGetRemoteDir();
        if (str != "1") {
            $ALT("获取当前进入文件夹的位置失败，请与管理员联系");
        }
    }
    catch (ex) {
        $ALT("info " + ex.description);
    }
}

/**
* ftp开始上传文件
* @param localFile 需要上传的文件全路径 例如"c:\\2223.mp3"
* @param ftpFile 上传在ftp的文件名称（前提，设置进入文件夹的位置）  例如"222.sys"
* @return 1-成功，开始上传； 2-失败
*/
function ftpUploadFile(localFile, ftpFile) {
    //alert("localFile=="+localFile+"  ftpFile=="+ftpFile);
    var str = "";
    try {
        str = MDOCX.ftpUploadFile(localFile, ftpFile);
        static_remoteFileName = static_remoteFileName + "/" + ftpFile;//在这里得到上传文件的FTP全路径文件名
        if (str != "1") {
            $ALT("ftp开始上传文件失败，请与管理员联系");
        }
    }
    catch (ex) {
        $ALT("ftp开始上传文件异常 " + ex.description);
        return str;
    }
    return str;
}

/**
* 获取ftp文件上传进度
*/
function ftpGetUploadFilePercent() {
    var str = 0;
    try {
        str = MDOCX.ftpGetUploadFilePercent();
        //alert(str)
    }
    catch (ex) {
        $ALT("获取ftp文件上传进度异常 " + ex.description);
        return str;
    }
    return str;
}

/**
* 删除本地文件 ***慎用***
* @param localFile 需要删除本地的文件全路径 例如"c:\\2223.mp3"
*/
function DelLocalFile() {
    try {
        var str = MDOCX.DelLocalFile(static_fileUrl);
        if (str != "1") {
            $ALT("删除本地文件失败，请与管理员联系");
        }
        else {
            static_uploaded++;
            //alert(static_uploaded+"/"+static_sum);
            if (getLocalFirstFile(static_selectLetter) != "") {
                setTimeout(uploadFile(), 1000);
            }
            else {
                alert("上传成功");
            }
        }
    }
    catch (ex) {
        //$ALT("删除本地文件异常 "+ex.description);
    }
}

function upload_Timer(file_name) {

    var t = setInterval(function () {
        static_speed = ftpGetUploadSpeed();
        static_per = ftpGetUploadFilePercent();
        $("#speed").html(static_per + ' %    ' + static_speed + ' kb/s');

        if (static_per == 100) {

            clearInterval(t);

            var _recordUserId = $('#upload_editId').val();
            var _recordUserDepId = $('#upload_editId').val();
            var _userId = getUserId();
            var _userDepId = getUserDepartmentId();
            var _createTime = getLocalFileCreateTime();
            var _recordTime = getLocalFileModifyTime();


            //ftp上传成功，DB插入一条上传记录
            $.ajax({
                url: '/File/Upload',
                type: 'post',
                dataType: 'json',
                cache: false,
                async: false,
                data: 'fileType=' + static_fileType + '&uploadUserId=' + _userId + '&uploadUserDepId=' + _userDepId + '&recordUserId=' + _recordUserId + '&recordUserDepId=' + _recordUserDepId + '&uploadFileName=' + static_remoteFileName + '&createTime=' + _createTime + '&recordTime=' + _recordTime,
                success: function (json) {
                    if (json != null) {
                        if (json.bUpload) {
                            //alert('可以上传');
                            clearInterval(t);
                            static_per = 0;
                            if (DelLocalFile2(file_name)) {
                                //如果删除本地文件成功，进度+1，继续下一个上传
                                ProgressBar.setProgress(++static_uploaded);
                                uploadFile();
                            }
                        }
                        else {
                            $ALT(json.msg);
                        }
                    }
                },
                complete: function (XMLHttpRequest, textStatus) {
                    //alert("complete");                    
                },
                error: function () {
                    $ALT("请求失败 error function");
                }
            });



        }
    }, 1000);
}


/**
* 删除本地文件 ***慎用***
* @param localFile 需要删除本地的文件全路径 例如"c:\\2223.mp3"
*/
function DelLocalFile2(file_name) {
    try {
        var str = MDOCX.DelLocalFile(file_name);
        if (str != "1") {
            $ALT("删除本地文件失败，请与管理员联系");
            return false;
        }
    }
    catch (ex) {
        //$ALT("删除本地文件异常 "+ex.description);
    }
    return true;
}

/**
* 取得本地文件夹下的所有文件（含文件夹下的文件） 迭代查询
* @param remoteDir 例如"D:\\wavecut\\"
*/
function getLocalDirFiles(remoteDir) {
    var str = "";
    try {
        str = MDOCX.getLocalDirFiles(remoteDir, 1);
    }
    catch (ex) {
        $ALT("取得本地文件夹下的所有文件（含文件夹下的文件）异常 " + ex.description);
    }
    return str;
}

/**
* 获取ftp文件上传速度
* @param remoteDir 例如"D:\\wavecut\\"
*/
function ftpGetUploadSpeed() {
    var str = 0;
    try {
        str = MDOCX.ftpGetUploadSpeed();
        //alert('文件上传速度:'+str);
    }
    catch (ex) {
        $ALT("获取ftp文件上传速度异常 " + ex.description);
        return str;
    }
    return str;
}

/**
* 获取USB外接设备连接的盘符 并显示在选择列表中
*/
function getUsbDriver() {
    try {
        var str = MDOCX.getUsbDriver();
        if (str != "") {
            var select_letter = str.split("*");
            for (i = 0; i < select_letter.length; i++) {
                document.getElementById("selectLetter").innerHTML += "&nbsp;&nbsp;&nbsp;<input type=radio name=r_letter value=" + select_letter[i] + " onclick=radioSelectLetter('" + select_letter[i] + "')>(" + select_letter[i] + ":)盘<br /><br />";
                //showMsg("请选择需要上传文件的盘符");
            }
        }
        else {
            //$ALT("您还没有插入USB设备，请确认~");
            showMsg("您还没有插入USB设备，请确认~");
            //document.getElementById("uploadTable").style.display = "none";
        }
    }
    catch (ex) {
        $ALT("info " + ex.description);
    }
}

/**
* 选择本地文件
*/
function selectLocalSaveDir() {
    try {
        var str = MDOCX.selectLocalSaveDir();
        if (str != "") {
            setLocalSaveDir(str);
            jQuery(function ($) {
                $("#localSaveDir").val(str);
                $("#localSaveButton").css("display", "block");
            });
        }
    }
    catch (ex) {
        $ALT("选择本地文件异常 " + ex.description);
    }
}




/**
* 选择本地文件（将本地上传至FTP）
*/
function selectLocalUploadDir() {
    try {
        var str = MDOCX.selectLocalSaveDir();
        if (str != "") {
            setLocalSaveUploadDir(str);
            jQuery(function ($) {
                $("#localUploadDir").val(str);
                $("#localUploadSelDiv").css("display", "block");
            });
            var firstFileUrl = getLocalFirstFile(static_selectLetter);
            var firstFileName = firstFileUrl.substring(firstFileUrl.lastIndexOf("\\") + 1);
            $('#uploadNameValue3').val(firstFileName);
            if (firstFileName.length == 34) {
                var codeIndex = firstFileName.indexOf("_") + 1;
                var jingyuanCode = firstFileName.substring(codeIndex, codeIndex + 6);

                
            }
        }
    }
    catch (ex) {
        $ALT("选择本地文件异常 " + ex.description);
    }
}

/**
* 设置本地文件路径（将本地上传至FTP）
*/
function setLocalSaveUploadDir(saveDir) {
    try {
        var str = MDOCX.setLocalSaveDir(saveDir);
        if (str == "1") {
            static_selectLetter = saveDir;
        }
    }
    catch (ex) {
        alert("info " + ex.description);
    }
}

/**
* 设置本地文件路径
*/
function setLocalSaveDir(saveDir) {
    try {
        var str = MDOCX.setLocalSaveDir(saveDir);
        if (str == "1") {
            static_localFilePath = saveDir;
        }
    }
    catch (ex) {
        alert("info " + ex.description);
    }
}

function copyLocalFile() {
    try {

        if (static_FistCallFun) {
            var totalCount = getLocalDirFilesNum(static_selectLetter);
            ProgressBar = new progressBar({ width: 260, height: 20, progress: 0, contentId: "progress", total: totalCount, text: " " });
            ProgressBar.show();
            static_FistCallFun = false;
        }
        var fileF = getLocalFirstFile(static_selectLetter);
        static_fileUrl = fileF;
        if (fileF == "") {
            static_uploaded = 0;
            alert("上传成功");
        }
        else {
            //alert(fileF);
            //alert(static_localFilePath+fileF.substring(fileF.lastIndexOf("\\"),fileF.length));
            //alert(fileF);
            //alert(static_localFilePath+fileF.substring(fileF.lastIndexOf("\\"),fileF.length));
            var str = MDOCX.copyLocalFile(fileF, static_localFilePath + fileF.substring(fileF.lastIndexOf("\\"), fileF.length));
            if (str == 1) {
                if (DelLocalFile2())//删除这个上传完成的文件
                {
                    ProgressBar.setProgress(++static_uploaded);
                    copyLocalFile(); //开始传递下一个文件
                }
                else {
                    alert("删除" + fileF + "时异常~");
                }
            }
        }
    }
    catch (ex) {
        alert("info " + ex.description);
    }
}

/**
* 获得选择需要进行上传盘符下的第一个文件
*/
function getLocalFirstFile(letter) {
    var str = "";
    try {
        if (letter.indexOf(':') > 0) {
            str = MDOCX.getLocalFirstFile(letter, 1);
        }
        else {
            str = MDOCX.getLocalFirstFile(letter + ":\\", 1);
        }
    }
    catch (ex) {
        $ALT("获得选择需要进行上传盘符下的第一个文件异常 " + ex.description);
        return str;
    }
    return str;
}

/**
* 获取上传文件的文件创建时间
* @param localFile 例如"c:\\2223.mp3"
*/
function getLocalFileCreateTime() {
    var str = "";
    try {
        str = MDOCX.getLocalFileCreateTime(static_fileUrl);
    }
    catch (ex) {
        $ALT("获取上传文件的文件创建时间异常 " + ex.description);
        return str;
    }
    return str;
}

/**
* 获取上传文件的文件修改时间
* @param localFile 例如"c:\\2223.mp3"
*/
function getLocalFileModifyTime() {
    var str = "";
    try {
        str = MDOCX.getLocalFileModifyTime(static_fileUrl);
    }
    catch (ex) {
        $ALT("获取上传文件的文件创建时间异常 " + ex.description);
        return str;
    }
    return str;
}

/**
* 获取选择需要进行上传盘符下的文件个数
* @param letter 盘符 例如"C"
*/
function getLocalDirFilesNum(letter) {
    var str = 0;
    try {
        //str =  MDOCX.getLocalDirFilesNum(letter+":\\",1);
        //var str =  MDOCX.getLocalDirFilesNum(letter+"\\",1);
        if (letter.indexOf(":") > 0) {
            //alert(letter+"\\");
            str = MDOCX.getLocalDirFilesNum(letter + "\\", 1);
        }
        else {
            //alert(letter+":\\");
            str = MDOCX.getLocalDirFilesNum(letter + ":\\", 1);
        }
        // alert(fileTotalNums);
    }
    catch (ex) {
        $ALT("获取选择需要进行上传盘符下的文件个数异常 " + ex.description);
        return str;
    }
    return str;
}

/**
* FTP建立文件夹，根据userid和depid
*/
function CreateRemoteDir(remoteDir) {
    if (yewu) {
        ftpCreateRemoteDir(remoteDir);
    }
}

/**
* 我要上传
*/
function uploadTable(remoteDir) {
    if (yewu) {
        ftpCreateRemoteDir(remoteDir);
    }
}

/**
* 我要上传（本地文件上传至FTP）
*/
function uploadTable2(remoteDir) {
    if (yewu) {
        ftpCreateRemoteDir(remoteDir);
    }
}

/**
* 选择盘符
*/
function radioSelectLetter(letterStr) {
    if (getLocalFirstFile(letterStr) != "") {
        static_selectLetter = letterStr;
        jQuery(function ($) {
            if (bUploadType == 2)
                $("#localSaveDiv").css("display", "block");
            else if (bUploadType == 1)
                $("#UsbUploadDiv").css("display", "block");

            var firstFileUrl = getLocalFirstFile(static_selectLetter);
            //alert(firstFileUrl);
            var firstFileName = firstFileUrl.substring(firstFileUrl.lastIndexOf("\\") + 1);
            $('#uploadNameValue1').val(firstFileName);
            if (firstFileName.length == 34) {
                var codeIndex = firstFileName.indexOf("_") + 1;
                var jingyuanCode = firstFileName.substring(codeIndex, codeIndex + 6);

                $.ajax({
                    url: contextPath() + 'servletAction.do?method=userFormByCode',
                    type: 'post',
                    dataType: 'json',
                    cache: false,
                    async: false,
                    data: { "userCode": jingyuanCode },
                    success: function (res) {
                        if (res != null) {
                            try {
                                setDefaultVal(res.retObj.userId, res.retObj.userName);
                                $('#upload_editId').val(res.retObj.userId);
                                $('#editName').val(res.retObj.userName);
                            } catch (e) { }
                        }
                    },
                    error: function () {
                        showMsg( "请求失败 error function");
                    }
                });
            }

        });
    }
    else {
        $ALT("您选择的盘符下没有可以上传的文件，请您确认~");
    }
}

/**
* 取消选择盘符
*/
function cancelRadioSelectLetter() {
    $BlockNone("step3_div", "none");
    uploadTable();
}

/**
* 取消 重新选择上传
*/
function cancelUpload() {
    $BlockNone("step5_div", "none");
    $BlockNone("step4_div", "none");
    $BlockNone("step3_div", "none");
    $BlockNone("step2_div", "none");
    $BlockNone("step1_div", "block");
}

/**
* 开始上传
*/
function startUpload() {

    jQuery(function ($) {

        

        var editValue = "";
        if (bUploadType == 1) {
            editValue = $('#editName1').val();
        }
        else {
            editValue = $('#editName3').val();
        }

        if (editValue == "" || editValue == 'undefine') {
            alert('请选择采集人'); return;
        }

        static_sum = getLocalDirFilesNum(static_selectLetter);
        if (static_sum <= 0) {
            alert('选定的目录下没有要上传的文件!');
            return;
        }

        //上传前判断服务器有没有足够的剩余空间
        $.ajax({
            url: '/File/GetDiskFreeSpaceAllowUpload',
            type: 'post',
            dataType: 'json',
            cache: false,
            async: false,
            data: {},
            success: function (json) {
                if (json != null) {
                    if (json.bUpload) {
                        //alert('可以上传');
                        //static_sum = getLocalDirFilesNum(static_selectLetter);
                        uploadFile();
                    }
                    else {
                        $ALT(json.msg);
                    }
                }
            },
            complete: function (XMLHttpRequest, textStatus) {
                //alert("complete");
            },
            error: function () {
                $ALT("请求失败 error function");
            }
        });
    });
}

/**
* 上传文件
*/
function uploadFile() {
    static_fileUrl = getLocalFirstFile(static_selectLetter);
    if (static_fileUrl != "") {
        var firstFileUrl = static_fileUrl;
        var firstFileName = firstFileUrl.substring(firstFileUrl.lastIndexOf("\\") + 1);

        if (bUploadType == 1) {
            $('#uploadNameValue1').val(firstFileName);
        }
        else {
            $('#uploadNameValue3').val(firstFileName);
        }

        if (static_FistCallFun) {
            var totalCount = getLocalDirFilesNum(static_selectLetter);
            ProgressBar = new progressBar({ width: 260, height: 20, progress: 0, contentId: "progress", total: totalCount, text:" " });
            ProgressBar.show();
            static_FistCallFun = false;
        }


//          自动填入上传用户id，此功能暂时停用，要自己改造
//        if (firstFileName.length == 34) {
//            var codeIndex = firstFileName.indexOf("_") + 1;
//            var jingyuanCode = firstFileName.substring(codeIndex, codeIndex + 6);

//            $.ajax({
//                url: contextPath() + 'servletAction.do?method=userFormByCode',
//                type: 'post',
//                dataType: 'json',
//                cache: false,
//                async: false,
//                data: { "userCode": jingyuanCode },
//                success: function (res) {
//                    if (res != null) {
//                        $('#upload_editId').val(res.retObj.userId);
//                        $('#editName').val(res.retObj.userName);
//                    }
//                },
//                error: function () {
//                    showMsg("请求失败 error function");
//                }
//            });
//        } else {
//            $('#upload_editId').val(defaultEditId);
//            $('#editName').val(defaultEditName);
//        }

        var extension = ""; //扩展名
        if (static_fileUrl.toUpperCase().lastIndexOf("AVI") > 0) {
            extension = "avi";
            static_fileType = 0;
        } else if (static_fileUrl.toUpperCase().lastIndexOf("MP4") > 0) {
            extension = "mp4";
            static_fileType = 1;
        } else if (static_fileUrl.toUpperCase().lastIndexOf("WAV") > 0) {
            extension = "wav";
            static_fileType = 2;
        } else {
            extension = "jpg";
            static_fileType = 3;
        }
        saveAs = getFileNameUseTime() + '.' + extension; //上传到FTP的文件根据当时时间命名
        //saveAs = firstFileName.toLowerCase();//直接转成小写上传
        ftpUploadFile(static_fileUrl, saveAs);
        fileTotalNums = static_sum;
        upload_Timer(static_fileUrl);
        //alert(contextPath()+"/sc.jsp?static_sum="+fileTotalNums+"&static_uploaded="+(static_uploaded+1)+"&speed="+ftpGetUploadSpeed()+"&timer="+Math.random())
        //$('#sc').attr('src', contextPath() + "/sc.jsp?static_sum=" + fileTotalNums + "&static_uploaded=" + (static_uploaded + 1) + "&speed=0&timer=" + Math.random());
        //document.getElementById("sc").src = contextPath()+"/sc.jsp?static_sum="+fileTotalNums+"&static_uploaded="+(static_uploaded+1)+"&speed="+ftpGetUploadSpeed()+"&timer="+Math.random();
    }
    else {
        
        if (static_sum <= 0 )
            $ALT("提醒： 选定的路径下没有要上传的文件!");

        static_sum = 0;
        alert('上传完成!');
    }
}
//获取短文件名
function getShortFileName(filename) { 
    return filename.substring(filename.lastIndexOf("\\") + 1);
}

//得到当前日期的字符串
function getCurrentDateString() {
    var myDate = new Date();
    var yyyy = myDate.getFullYear(); //获取完整的年份(4位,1970-????)
    var MM = myDate.getMonth() + 1; //获取当前月份(0-11,0代表1月)
    var dd = myDate.getDate(); //获取当前日(1-31)
    return yyyy + "" + (MM < 10 ? ("0" + MM) : MM) + "" + (dd < 10 ? ("0" + dd) : dd);
}

//根据时间格式返回文件名
function getFileNameUseTime() {
    var myDate = new Date();
    var yyyy = myDate.getFullYear(); //获取完整的年份(4位,1970-????)
    var MM = myDate.getMonth()+1; //获取当前月份(0-11,0代表1月)
    var dd = myDate.getDate(); //获取当前日(1-31)
    myDate.getDay(); //获取当前星期X(0-6,0代表星期天)
    myDate.getTime(); //获取当前时间(从1970.1.1开始的毫秒数)
    var h24 = myDate.getHours(); //获取当前小时数(0-23)
    var mm = myDate.getMinutes(); //获取当前分钟数(0-59)
    var ss = myDate.getSeconds(); //获取当前秒数(0-59)
    var ms = myDate.getMilliseconds(); //获取当前毫秒数(0-999)
    return yyyy + "" + (MM < 10 ? ("0" + MM) : MM) + "" + (dd < 10 ? ("0" + dd) : dd) + "_" + (h24 < 10 ? ("0" + h24) : h24) + "" + (mm < 10 ? ("0" + mm) : mm) + "" + (ss < 10 ? ("0" + ss) : ss) + "_" + myDate.getMilliseconds();
}

function uploadSuccess() {
    document.getElementById("upload_playCreatetime").value = getLocalFileCreateTime();
    document.getElementById("upload_playPath").value = saveDir + ',' + saveAs;
    if (document.getElementById("uploadNameValue1").value != "") {
        document.getElementById("upload_uploadName").value = document.getElementById("uploadNameValue1").value;
    }
    else {
        document.getElementById("upload_uploadName").value = document.getElementById("uploadNameValue2").value;
    }
    document.getElementById("uploadForm").submit();
}



$(document).ready(function () {


    /**
    * 判断该用户使用的版本是否正确
    */
    if (nowVersion.split(".")[3] > getVersion().split(".")[3]) {
        $ALT("您使用的版本不能正常使用~");
        yewu = false;
    }
    /**
    * 设置ftp参数 并登录
    */
    if (yewu) {
        setParm(ftpHost, ftpUser, ftpPswd, ftpPort);
        ftpLogin();

    }


    if (static_ftpLogin)//ftp登录没有问题
    {
        alert('1');
        ftpSetRemoteRoot();
        ftpSetFileStepSize(10);

        showInformation("请您选择需要上传的方式");
    }
});