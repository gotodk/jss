<%@ Page Language="C#" AutoEventWireup="true" CodeFile="jqueryUpload.aspx.cs" Inherits="pagerdemo_jqueryUpload" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Uploadify</title>
    <link href="jquery.uploadify-v2.1.4/uploadify.css" type="text/css" rel="stylesheet" />
 
    <script type="text/javascript" src="jquery.uploadify-v2.1.4/jquery-1.6.2.js"></script>
 
    <script type="text/javascript" src="jquery.uploadify-v2.1.4/swfobject.js"></script>
 
    <script type="text/javascript" src="jquery.uploadify-v2.1.4/jquery.uploadify.v2.1.4.js"></script>

 
 
    <script type="text/javascript">
        var myArray = new Array(); //声明此数组来存放文件生成的路径
        $(function () {
            $('#custom_file_upload').uploadify({
                'uploader': 'jquery.uploadify-v2.1.4/uploadify.swf',
                'script': 'jqueryUpload.aspx',
                'cancelImg': 'jquery.uploadify-v2.1.4/cancel.png',
                'queueID': 'fileQueue',
                'folder': 'jqueryUploadFiles',
                'auto': false,
                'multi': true,
                'fileExt': '*.txt;*.rar;*.zip;*.jpg;*.jpeg;*.gif;*.png;*.swf;*.wmv;*.avi;*.wma;*.mp3;*.mid;*.doc;*.xls;*.ppt',
                'fileDesc': 'Files',
                'queueID': 'customqueue',
                'queueSizeLimit': 3,
                'simUploadLimit': 3,
                'removeCompleted': false,
                'onSelectOnce': function (event, data) {
                    $('#status-message').text(data.filesSelected + ' 文件正在等待上传…….');
                },
                'onComplete': function (evt, queueID, fileObj, response, data) {//onComplete表示文件上传成功事件调用函数   
                    $('#files').append('<li>文件:' + (fileObj.name).substring((fileObj.name).lastIndexOf('/')) + '上传成功</li>'); //response 返回的数据 可以返回对应的JSON形式的所有的 已上传的文件的路径（存入SQL）
                    //alert("文件:" + fileObj.name + " 上传成功");    //名字是上传 的当前 的本机的文件名称
                    //alert(queueID);//所在位置

                    //被请求页面直接输出JSON格式字符串。用onComplete里的response获取。再用jQuery.parseJSON(response)格式化成JSON数据就可以啦。 
                    //alert("" + response); //调用传递回来的响应信息  
                    myArray.push(response);
                    $("#customqueue input").each(function (i) {
                        this.val() = i;
                    });
                },
                'onAllComplete': function (event, data) {

                    $('#status-message').text(data.filesUploaded + ' 文件上传完成！' + data.errors + '失误！');
                },
                'onQueueFull': function (event, data) {
                    alert("上传数目已满. 最多上传3个文件！");
                },
                'onCancel': function (event, data) {
                    //alert(data);//所在位置
                    var dd = $('#custom_file_upload' + data).find('.fileName').text();
                    var dd1 = dd.substring(0, (dd).lastIndexOf(' ('))//所要删除的文件名称 这里注意（空格+‘（’）。有个空格字符
                    // var dd2="文件:"+dd1+"上传成功";
                    $("#files li").each(function (i) {
                        if ($(this).text().indexOf(dd1) >= 0) {
                            $(this).remove();
                            for (var i = 0; i < myArray.length; i++) {
                                if (myArray[i].indexOf(dd1) >= 0) {
                                    for (var j = i; j < myArray.length - 1; j++) {
                                        myArray[j] = myArray[j + 1];
                                    }
                                    myArray.pop();
                                }
                            }
                        }

                    });
                    $('#status-message').text('选择上传的文件:');
                }, //清除一个的时候.对应的循序清楚数组中的，后面的向前赋值。
                'onClearQueue': function (event, data) {
                    $('#status-message').text('选择上传的文件:');
                    $("ul").empty();
                    myArray = null;

                } //清楚所有的时候
            });
        });     
    </script>
 
 
    <style type="text/css">
        #custom-demo .uploadifyQueueItem
        {
            background-color: #FFFFFF;
            border: none;
            border-bottom: 1px solid #E5E5E5;
            font: 11px Verdana, Geneva, sans-serif;
            height: 50px;
            margin-top: 0;
            padding: 10px;
            width: 350px;
        }
        #custom-demo .uploadifyError
        {
            background-color: #FDE5DD !important;
            border: none !important;
            border-bottom: 1px solid #FBCBBC !important;
        }
        #custom-demo .uploadifyQueueItem .cancel
        {
            float: right;
        }
        #custom-demo .uploadifyQueue .completed
        {
            color: #C5C5C5;
        }
        #custom-demo .uploadifyProgress
        {
            background-color: #E5E5E5;
            margin-top: 10px;
            width: 100%;
        }
        #custom-demo .uploadifyProgressBar
        {
            background-color: #0099FF;
            height: 3px;
            width: 1px;
        }
        #custom-demo #customqueue
        {
            border: 1px solid #E5E5E5;
            height: 213px;
            margin-bottom: 10px;
            width: 370px;
        }
    </style>
 

 
</head>
<body>
    <form id="form1" runat="server">

                                <table>
                                    <tr>
                                        <td>
                                            <div id="custom-demo" style="margin-left: 60px">
                                                <div id="uploadFile">
                                                    <div id="status-message">
                                                        选择上传的文件:</div>
                                                    <div id="customqueue">
                                                    </div>
                                                    <input id="custom_file_upload" type="file" name="custom_file_upload" />
                                                    <p>
                                                        <a href="javascript:$('#custom_file_upload').uploadifyUpload()">上传</a>| <a href="javascript:$('#custom_file_upload').uploadifyClearQueue()">
                                                            取消上传</a>
                                                    </p>
                                                </div>
                                            </div>
                                        </td>
                                        <td>
                                            <div>
                                                <ul id="files">
                                                </ul>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                </form>
      
</body>
</html>