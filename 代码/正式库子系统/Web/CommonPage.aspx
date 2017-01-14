<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CommonPage.aspx.cs" Inherits="CommonPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>内容显示页</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <%--<style type="text/css">

body {
	margin-left: 0px;
	margin-top: 0px;
	margin-right: 0px;
	margin-bottom: 0px;
}

</style>--%>
    <style type="text/css">
        .main
        {
            width: 790px;
            height: 800px;
            margin: 0px auto 0px auto;
        }
        .header
        {
            width: 97%;
            height: 30px;
            margin: 0px;
            padding: 10px;
        }
        .header h2
        {
            font-size: 16px;
            font-family: 宋体;
            font-weight: bold;
            color: #343436;
        }
        .content
        {
            width: 97%;
            height: 750px;
            margin: 0px;
            padding: 1px;
        }
        .templateDiv
        {
            width: 120px;
            height: 130px;
            float: left;
            margin: 5px 10px;
            padding: 5px;
            cursor: hand;
            padding: 2px;
            border: 1px solid #DEECEF;
        }
        .addtemplateDiv
        {
             width: 120px;
            height: 130px;
            float: left;
            margin: 5px 10px;
            cursor: hand;
            padding: 2px;
            border: 1px solid #b6b6b6;
            background-color:#ffffff;
            }
        .addtemplateDiv .spanAdd
        {
            display: inline-block;
            width: 24px;
            height: 23px;
            margin:53px 48px;
        }
        .imgageDark
        {
          width:100%;
          height:100%;
            }    
        .addtemplateDivBorder
        {
           border: 1px solid #349aff; 
            }    
        .templateDivBorder
        {
            border: 1px solid #3699FF;
        }
        .templateDiv .error
        {
            display: inline-block;
            width: 12px;
            height: 12px;
            position: relative;
            left: 107px;
            top: 2px;
        }
        .imageError
        {
            width: 100%;
            height: 100%;
        }
        .imageErrorTemplate
        {
            width: 100%;
            height: 100%;
            display: none;
        }
        .imageBox
        {
            width: 78px;
            height: 70px;
            margin: 0px auto;
            padding: 0px;
        }
        .imagecontent
        {
            width: 100%;
            height: 100%;
        }
        .interlining
        {
            width: 100%;
            height: 17px;
        }
        .prompt
        {
            width: 112px;
            height: 25px;
            margin: 0px auto;
        }
        .prompt .con_left
        {
            width: 12px;
            height: 25px;
            background: transparent url(/Web/images/welcome/16.png) no-repeat;
            display: block;
            float: left;
        }
        .prompt .con_content
        {
            width: 88px;
            height: 25px;
            background-color: #268EC5;
            display: block;
            float: left;
            color: White;
            font-size: 12px;
            font-family: 宋体;
            text-align: center;
            line-height: 25px;
        }
        .prompt .con_right
        {
            width: 12px;
            height: 25px;
            background: transparent url(/Web/images/welcome/17.png) no-repeat;
            display: block;
            float: right;
        }
        .templateClone
        {
            display: none;
        }
        .funID
        {
            display: none;
        }
        .funName
        {
            display: none;
        }
        .funIdent
        {
            display: none;
        }
        .funCate
        {
            display: none;
        }
    </style>
    <script src="jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="JSON.js" type="text/javascript"></script>
    <script type="text/javascript">
        //控制类对象
        var controllerInfo;
        //操作类对象
        var operateInfo;
        var operateInfoText = "";
        var functionInfoText = "";

        $(document).ready(function (e) {
            /*
            声明控制类
            和操作类对象
            */

            //实例化控制类对象
            controllerInfo = new ControllerClass();
            //实例化操作类对象
            operateInfo = new OperateInfo();
            operateInfo.OperUser = '<%=User.Identity.Name %>';
             operateInfoText = JSON.stringify(operateInfo);
         


            /*
            为div.templateDiv增加事件
            */
            //鼠标浮上事件
            $("div.templateClone >div.templateDiv").hover(function (e) {
                $(this).addClass("templateDivBorder");
                $(this).find("img[Error='Error']").removeClass("imageErrorTemplate").addClass("imageError");

            }, function (e) {
                $(this).removeClass("templateDivBorder");
                $(this).find("img[Error='Error']").removeClass("imageError").addClass("imageErrorTemplate");

            });
            //绑定单击事件(判断有没有权限)
            $("div.templateClone >div.templateDiv").click(function (e) {
                operateInfo.OperProper = "查看";
                operateInfo.OperCate = $(this).find("span.funCate").attr("FunCate");
                operateInfoText = JSON.stringify(operateInfo);
                var functionsInfo = new FunctionsInfo();
                functionsInfo.FunCate = $(this).find("span.funCate").attr("FunCate");
                functionsInfo.FunFullName = $(this).attr("title");
                functionsInfo.FunID = $(this).find("span.funID").attr("funid");
                functionsInfo.FunName = $(this).find("span.funName").attr("funname");
                functionsInfo.FunDesc = $(this).find("span.con_content").html();
                functionsInfo.FunIdent = $(this).find("span.funIdent").attr("funident");
                functionsInfo.FunPicPath = $(this).find("img[Fun='Fun'").attr("src");
                functionInfoText = JSON.stringify(functionsInfo);
                controllerInfo.Authority(operateInfoText, functionInfoText);
            });
            //绑定删除事件
            $("div.templateClone >div.templateDiv >span.error").click(function (e) {
              
                operateInfo.OperProper = "删除";
                operateInfo.OperCate = $(this).parent("div.templateDiv").find("span.funCate").attr("FunCate");
                operateInfoText = JSON.stringify(operateInfo);
                var functionsInfo = new FunctionsInfo();
                functionsInfo.FunCate = $(this).parent("div.templateDiv").find("span.funCate").attr("FunCate");
                functionsInfo.FunFullName = $(this).attr("title");
                functionsInfo.FunID = $(this).parent("div.templateDiv").find("span.funID").attr("funid");
                functionsInfo.FunName = $(this).parent("div.templateDiv").find("span.funName").attr("funname");
                functionsInfo.FunDesc = $(this).parent("div.templateDiv").find("span.con_content").html();
                functionsInfo.FunIdent = $(this).parent("div.templateDiv").find("span.funIdent").attr("funident");
                functionsInfo.FunPicPath = $(this).parent("div.templateDiv").find("img[Fun='Fun'").attr("src");
                functionInfoText = JSON.stringify(functionsInfo);
//                controllerInfo.DeleteFunctions(operateInfoText, functionInfoText);
                //阻止事件冒泡
                e.stopPropagation();
                //删除弹窗提示
                window.top.DeleteWarning();

            });
            /*
            为div.addtemplateDiv增加事件
            */
            //鼠标滑过事件
            $("div.templateClone >div.addtemplateDiv").hover(function (e) {
                $(this).addClass("addtemplateDivBorder");
                $(this).find("img[Add='Add']").attr("src", "/Web/images/welcome/CommonFeatures/heighLight.png");

            }, function (e) {
                $(this).removeClass("addtemplateDivBorder");
                $(this).find("img[Add='Add']").attr("src", "/Web/images/welcome/CommonFeatures/unheighLight.png");
            });
            //鼠标点击事件
            $("div.templateClone >div.addtemplateDiv").click(function (e) {
                controllerInfo.AddFunctions($(this));
            });
            controllerInfo.LoadData(operateInfoText);
        });
       

        //创建控制类
        function ControllerClass() {
            /*
            导入数据
            */
            this.LoadData = function (operateInfoText) {
                $.ajax({ "url": "CommonPageAttach.ashx?Methord=GetAllCommonFunctions", "type": "post", "data": { "operateInfoText": operateInfoText }, "dataType": "json", "cache": false, "async": true, "success": function (responseInfo) {
                    if (responseInfo.IsSuccess) {
                        var functionsInfoArray = responseInfo.Data;
                        var $dataDiv = $("#divContent");
                        var m = 1;
                        var $newFun;

                        $.each(functionsInfoArray, function (i) {
                            if (parseInt(functionsInfoArray[i].FunIdent) != m) { //增加图标的逻辑
                                for (var k = 0; k < parseInt(functionsInfoArray[i].FunIdent) - m; k++) {
                                    $newFun = $("div.templateClone>div.addtemplateDiv").clone(true);
                                    $newFun.find("span.spanAdd").attr("FunIdent",m+k);
                                    $dataDiv.append($newFun);
                                }
                                m = parseInt(functionsInfoArray[i].FunIdent);
                            }
                            $newFun = $("div.templateClone>div.templateDiv").clone(true);
                            $newFun.attr("title", functionsInfoArray[i].FunFullName); 
                            $newFun.find("img[Fun='Fun']").attr("src", functionsInfoArray[i].FunPicPath);
                            $newFun.find("img[Fun='Fun']").attr("title", functionsInfoArray[i].FunFullName);
                            $newFun.find("span.con_content").html(functionsInfoArray[i].FunDesc);
                            $newFun.find("span.funID").attr("FunID", functionsInfoArray[i].FunID);
                            $newFun.find("span.funName").attr("FunName", functionsInfoArray[i].FunName);
                            $newFun.find("span.funIdent").attr("FunIdent", functionsInfoArray[i].FunIdent);
                            $newFun.find("span.funCate").attr("FunCate", functionsInfoArray[i].FunCate);
                            $dataDiv.append($newFun);
                            m++;
                        });
                        //判断增加多少空图标，并且增加图标
                        var emptyLength = 0;
                        if (functionsInfoArray[functionsInfoArray.length - 1] != undefined) {
                            emptyLength = parseInt(functionsInfoArray[functionsInfoArray.length - 1].FunIdent);
                        }
                        for (var z = 1; z <= 25 - emptyLength; z++) {
                            $newFun = $("div.templateClone>div.addtemplateDiv").clone(true);
                            $newFun.find("span.spanAdd").attr("FunIdent", emptyLength + z);
                            $dataDiv.append($newFun);
                        }
                    }
                    else {
                        alert(responseInfo.Msg);
                    }
                }
                });
            };
            /*
            判断权限和对权限的处理
            */
            this.Authority = function (operateInfoText, functionInfoText) {
                $.ajax({ "url": "CommonPageAttach.ashx?Methord=JudgeAuthority", "type": "post", "data": { "operateInfoText": operateInfoText, "functionInfoText": functionInfoText },
                    "dataType": "json", "cache": false, "async": false, "success": function (responseInfo) {
                        if (responseInfo.IsSuccess) {
                            var functionsInfoArray = responseInfo.Data;
                            var strFunParIdColl = functionsInfoArray.FunParIdColl.split(',');
                            //打开左侧的对应目录
                            var p = 0;
                            for (var i = 0; i < strFunParIdColl.length; i++) {
                                if (strFunParIdColl[i] != 0) {
                                    //setTimeout("onesub_run(" + strFunParIdColl[i] + ")", p * 2000);
                                    onesub_run(strFunParIdColl[i]);
                                    p++;
                                }
                            }
                            //标记左侧对应目录
                            window.top.frames['leftFrame'].SignSub(functionsInfoArray.FunName);
                            //打开对应的页面
                            window.top.frames['leftFrame'].menuClick_yhb(functionsInfoArray.FunName, functionsInfoArray.FunAuthrity);

                        }
                        else {
                            alert("对不起，您没有权限查看此模块！");
                        }
                    }
                });
            };
            /*
            删除功能
            */
            this.DeleteFunctions = function (operateInfoText, functionInfoText) {

                $.ajax({ "url": "CommonPageAttach.ashx?Methord=DeleteFunctions", "type": "post", "data": { "operateInfoText": operateInfoText, "functionInfoText": functionInfoText },
                    "dataType": "json", "cache": false, "async": false, "success": function (responseInfo) {
                        if (responseInfo.IsSuccess) {
                            var functionsInfoArray = responseInfo.Data;
                            //执行删除操作
                            $("div.content >div.templateDiv").each(function () {
                                if ($(this).find("span.funID").attr("FunID") == functionsInfoArray.FunID && $(this).find("span.funName").attr("FunName") == functionsInfoArray.FunName && $(this).find("span.funIdent").attr("FunIdent") == functionsInfoArray.FunIdent) {
                                    var $newFun = $("div.templateClone>div.addtemplateDiv").clone(true);
                                    $newFun.find("span.spanAdd").attr("FunIdent", functionsInfoArray.FunIdent);
                                    $(this).after($newFun);
                                    $(this).remove();
                                }
                            });
                        }
                        else {
                            alert("对不起，删除失败！");
                        }
                    }
                });
            };
            /*
            添加功能弹窗
            */
            this.AddFunctions = function ($object) {
                var funIdent = $object.find("span.spanAdd").attr("FunIdent");
                window.top.OpenWelcome(funIdent);
            };
            /*
            添加功能
            */
            this.AddFunctionsData = function (operateInfoText, functionInfoText) {
                $.ajax({ "url": "CommonPageAttach.ashx?Methord=AddFunData", "type": "post", "data": { "operateInfoText": operateInfoText, "functionInfoText": functionInfoText },
                    "dataType": "json", "cache": false, "async": false, "success": function (responseInfo) {
                        if (responseInfo.IsSuccess) {
                            var functionsInfoArray = responseInfo.Data;
                            //执行删除操作
                            $("div.content >div.addtemplateDiv").each(function () {
                                if ($(this).find("span.spanAdd").attr("FunIdent") == functionsInfoArray.FunIdent) {
                                    $newFun = $("div.templateClone >div.templateDiv").clone(true);
                                    $newFun.attr("title", functionsInfoArray.FunFullName);
                                    $newFun.find("img[Fun='Fun']").attr("src", functionsInfoArray.FunPicPath);
                                    $newFun.find("img[Fun='Fun']").attr("title", functionsInfoArray.FunFullName);
                                    $newFun.find("span.con_content").html(functionsInfoArray.FunDesc);
                                    $newFun.find("span.funID").attr("FunID", functionsInfoArray.FunID);
                                    $newFun.find("span.funName").attr("FunName", functionsInfoArray.FunName);
                                    $newFun.find("span.funIdent").attr("FunIdent", functionsInfoArray.FunIdent);
                                    $newFun.find("span.funCate").attr("FunCate", functionsInfoArray.FunCate);
                                    $(this).after($newFun);
                                    $(this).remove();
                                }
                            });
                        }
                        else {
                            alert("对不起，添加失败！");
                        }
                    }
                });
            };
        }

        function onesub_run(ii) {

            window.top.frames['leftFrame'].open_onesub(ii);
        }

        //更新欢迎页面
        function UpdateOpenVelcome(funInfoText) {
            operateInfo.OperProper = "添加";
            operateInfo.OperCate = "常用功能";
            var operateText = JSON.stringify(operateInfo);
            var funInfoTextText = JSON.stringify(funInfoText);
            controllerInfo.AddFunctionsData(operateText, funInfoTextText);
        }
        /*
        创建操作类对象
        */
        function OperateInfo() {
            /*
            用户账号
            */
            this.OperUser = "";
            /*
            用户操作类别 (“默认功能”还是“常用功能”)
            */
            this.OperCate = "";
            /*
            用户操作性质（“增加”还是“删除”还是“查看”）
            */
            this.OperProper = "";
        }

        /*
        创建功能类对象
        */
        function FunctionsInfo() {
            /*
            功能ID
            */
            this.FunID = "";
            /*
            功能描述
            */
            this.FunDesc = "";
            /*
            功能全称
            */
            this.FunFullName = "";
            /*
            功能NAME
            */
            this.FunName = "";
            /*
            功能编号
            */
            this.FunIdent = "";
            /*
            功能图片路径
            */
            this.FunPicPath = "";
            /*
            功能类别
            */
            this.FunCate = "";
            /*
            功能类别父类ID集合
            */
            this.FunParIdColl = "";
            /*
            功能权限
            */
            this.FunAuthrity = "";
        }    

   
    </script>
</head>
<body style="background-color: #deecef; height: 100%">
    <form id="form1" runat="server">
    <div class="main">
        <div class="header">
            <h2>
                您最常使用的功能</h2>
        </div>
        <div id="divContent" class="content">
         <%--   <div class="templateDiv" title="录入考勤单">
                <span class="error">
                    <img src="/Web/images/welcome/CommonFeatures/error.png" error="Error" alt="" title="删除常用功能"
                        class="imageErrorTemplate" /></span>
                <div class="imageBox">
                    <img src="/Web/images/welcome/CommonFeatures/1.png" fun="Fun" alt="" class="imagecontent" />
                </div>
                <div class="interlining">
                    <span class="funID" funid="1208000001"></span><span class="funName" funname="NewinputChuqindanList">
                    </span><span class="funIdent" funident="1"></span><span class="funCate" funcate="默认功能">
                    </span>
                </div>
                <div class="prompt">
                    <span class="con_left"></span><span class="con_content">录入考勤单</span> <span class="con_right">
                    </span>
                </div>
            </div>
               <div class="addtemplateDiv" title="添加常用功能">
        <span class="spanAdd"><img src="/Web/images/welcome/CommonFeatures/unheighLight.png" Add="Add"  alt="" title="添加常用功能"
                        class="imgageDark" /></span>
        </div>--%>
        </div>
      
    </div>
    <div class="templateClone">
        <div class="templateDiv" title="">
            <span class="error">
                <img src="/Web/images/welcome/CommonFeatures/error.png" error="Error" alt="" title="删除常用功能"
                    class="imageErrorTemplate" /></span>
            <div class="imageBox">
                <img src="/Web/images/welcome/CommonFeatures/1.png" fun="Fun" title="" alt="" class="imagecontent" />
            </div>
            <div class="interlining">
                <span class="funID" funid="1208000001"></span><span class="funName" funname="NewinputChuqindanList">
                </span><span class="funIdent" funident="1"></span><span class="funCate" funcate="默认功能">
                </span>
            </div>
            <div class="prompt">
                <span class="con_left"></span><span class="con_content">录入考勤单</span> <span class="con_right">
                </span>
            </div>
        </div>
        <div class="addtemplateDiv" title="添加常用功能">
        <span class="spanAdd" FunIdent="" ><img src="/Web/images/welcome/CommonFeatures/unheighLight.png" Add="Add"  alt="" title="添加常用功能"
                        class="imgageDark" /></span>
        </div>
    </div>
    </form>
</body>
</html>
