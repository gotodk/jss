<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LeftContent.aspx.cs" Inherits="Web_LeftContent" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        body
        {
            background-color:#EDF4FD;
            margin-left:10px;
            margin-top:0px;
            }
    .divHead
    {
      width:350px;  
       height:37px;
       float:left;
        }
        .divNewMain
        {
             width:430px;  
              height:33px;
            }
            .divButton
            {
       width:50px;  
       height:37px;
       float:left;
        padding:2px 5px 1px 10px;
                }
        .main
        {
           width:430px;  
            border:1px solid #5E93BB;
           margin-top:3px;
            }
        .divContent
        {
         width:100%;
          margin-top:4px;
      }
      .divMainHead
      {
           width:95%;
           padding:5px 2px 5px 10px;
           background-color:#F5F9FE;
           border-bottom:1px solid #C5DDF9;  
          }
      .divMainHead span
      {
       font-weight:bold;
       font-size:14px;
       font-family:"宋体";
       color:#1A668E;
       }
       .btnhui{border:1px solid #5D94BB;height:30px;  cursor: pointer;min-width:70px; font-family: "\5b8b\4f53","Tamoda","Arial",Sans-serif; font-size: 12px;
               margin:0;padding:0;color:#003267;  background-color:#F3F8FE;}
       .txtM {background-color: #FFFFFF; border:2px solid #2E94DE;  height:14px; width:208px; line-height:14px; padding-top:5px;padding-right:8px;padding-bottom:8px;padding-left:6px; font-size:14px;   margin-top:2px; margin-bottom:3px; color:#8D8582; }
      .txtI { border:2px solid #2E94DE; height:14px; width:102px; line-height:14px; padding-top:5px;padding-right:8px;padding-bottom:8px;padding-left:6px; font-size:14px; color:#8D8582; margin-top:2px; margin-bottom:3px; background:#FFFFFF url(images/welcome/CommonFeatures/trangle.png) 95px 8px no-repeat;}
      .absoluteIco
      {
        width:117px;
        position:absolute;
        left:240px;
        top:59px;
        border:1px solid #4C7B85;
        z-index:5;
        background-color:#FFFFFF;
        display:none;
          }
        .templateClone
          {
             display:none; 
           }
          .templateDiv{ width:117px; height:54px; background-color:#FFFFFF; cursor:pointer;}
          .templateDivBgCol{ background-color:#3D96D0;}
          .templateDivImage{ width:54px; height:52px;  float:left; padding-left:4px; padding-top:2px; }
          .templateDivText{ width:59px; height:54px; padding:auto auto; float:right; }
        .templateDivText span{ display:inline-block; font-size:12px; width:100%; height:14px; line-height:14px; padding-left:14px; margin-top:18px;}
        .promotDiv{   width:430px; height:25px; border:1px solid #EDF4FD;  }
        .promot{  border:1px solid #EB5359; margin-top:2px; display:none; background:#FFF2F2 url(images/welcome/icon.png) left 0px no-repeat;  width:223px; font-size:12px;   height:20px;  color:#000000; line-height:18px; padding-left:18px; }
        .clearBothDiv{ width:0px; height:0px; clear:both;}
    </style>
   <script language="javascript" type="text/javascript" src="Ajax.js"></script>
    <script src="JSON.js" type="text/javascript"></script>
    <script src="jquery-1.7.2.min.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
 
        function onload_tree() {
            getdata("LeftContentTree.aspx?mode=getTree&fatherID=0", "leftContent");
        }
     
 
    //声明图标数组(收集数据)
     var iconTypeArray = new Array();
         iconTypeArray[0] = new Array("/Web/images/welcome/1.png","图标1");
         iconTypeArray[1] = new Array("/Web/images/welcome/2.png","图标2");
         iconTypeArray[2] = new Array("/Web/images/welcome/3.png","图标3");
         iconTypeArray[3] = new Array("/Web/images/welcome/4.png","图标4");
         iconTypeArray[4] = new Array("/Web/images/welcome/5.png","图标5");
         iconTypeArray[5] = new Array("/Web/images/welcome/6.png","图标6");
         iconTypeArray[6] = new Array("/Web/images/welcome/7.png","图标7");
         iconTypeArray[7] = new Array("/Web/images/welcome/8.png","图标8");
         iconTypeArray[8] = new Array("/Web/images/welcome/9.png","图标9");
         iconTypeArray[9] = new Array("/Web/images/welcome/10.png","图标10");
         iconTypeArray[10] = new Array("/Web/images/welcome/11.png","图标11");
         iconTypeArray[11] = new Array("/Web/images/welcome/12.png","图标12");
         iconTypeArray[12] = new Array("/Web/images/welcome/13.png","图标13");
         iconTypeArray[13] = new Array("/Web/images/welcome/14.png","图标14");
         iconTypeArray[14] = new Array("/Web/images/welcome/15.png", "图标15");

         $(document).ready(function (e) {

             /*
             声明控制类对象
             */
             var controllerClass = new ControllerClass();
             /*
             声明功能类对象
             */
             var funIdentInfo = new FunctionsInfo();
             funIdentInfo.FunIdent = '<%=Request.Params["FunIdent"].ToString()%>';

             /*
             为div.templateDiv增加事件
             */
             //鼠标滑过事件
             $("div.templateClone >div.templateDiv").hover(function (e) {
                 $(this).addClass("templateDivBgCol");
             }, function (e) {
                 $(this).removeClass("templateDivBgCol");
             });
             //鼠标点击事件
             $("div.templateClone >div.templateDiv").click(function (e) {
                 var icoTypeText = $(this).find("span").html();
                 var icoImageUrl = $(this).find("img").attr("src");
                 $("#" + '<%=txtIcon.ClientID%>').val(icoTypeText);
                 $("#" + '<%=txtIcon.ClientID%>').css({ "color": "black" });
                 $("#" + '<%=txtIcon.ClientID%>').attr("ImageUrl", icoImageUrl);
                 $("#divAbsoluteIco").css({ "display": "none" });
                 window.location.hash = "firstAnchor";
             });
             /*
             为保存按钮绑定事件
             */
             $("#btnSave").click(function (e) {
                 //① 判断模块名称和图标名称是否符合要求
                 var bool = SubmitFirstStep();
                 //② 符合条件① 收集页面信息
                 if (bool) {

                     funIdentInfo.FunDesc = $("#" + '<%=txtModule.ClientID%>').val();
                     funIdentInfo.FunName = $("#" + '<%=txtModule.ClientID%>').attr("ModuleName");
                     funIdentInfo.FunFullName = $("#" + '<%=txtModule.ClientID%>').attr("FunFullName");
                     funIdentInfo.FunCate = "常用功能";
                     funIdentInfo.FunPicPath = $("#" + '<%=txtIcon.ClientID%>').attr("ImageUrl");
                     //
                     controllerClass.UpdateData(funIdentInfo);
                 }
                 //③ 写入数据库
                 //④ 更新对应的图标 
             });
             controllerClass.LoadData();
         });
         //判断模块名称和图标名称是否符合要求
         function SubmitFirstStep() { 
         var message="";
         if ($("#" + '<%=txtModule.ClientID%>').val() == "请选择模块" || $("#" + '<%=txtModule.ClientID%>').val() == "") {
             message = "请选择模块 ！"
         }
         else if ($("#" + '<%=txtModule.ClientID%>').val().length > 6) {
             message = "模块名称不能超过6个字符！";
         }

         if ($("#" + '<%=txtIcon.ClientID%>').val() == "请选择图标") {
             message = message + "请选择图标！";
         }
         if (message == "") {
             $("#divPromot").css({ "display": "none" });
             return true;
         }
         else {
             $("#divPromot").html(message);
             $("#divPromot").css({ "display": "block" });
             return false;
         }
     }
     function fEvent(sType, oInput) {
         switch (sType) {
             case "focus":
                 if (oInput.getAttribute("id") == "txtIcon") { //图标
                     $("#divAbsoluteIco").css({ "display": "block" });
                 }
                 break;
         }
     }
     //当点击模块的事后触发的事件
     function OnDataClick(moduleName, moduleTitle) {
         $("#" + '<%=txtModule.ClientID%>').val(moduleTitle);
         $("#" + '<%=txtModule.ClientID%>').css({ "color": "black" });
         $("#" + '<%=txtModule.ClientID%>').attr("ModuleName", moduleName);
         $("#" + '<%=txtModule.ClientID%>').attr("FunFullName", moduleTitle);
         $("#" + '<%=txtModule.ClientID%>')[0].focus();
         //回到描点
         window.location.hash = "firstAnchor";
     }
         //创建控制类
         function ControllerClass() {
             /*
             导入数据
             */
             this.LoadData = function () {
                 var $dataDiv = $("#divAbsoluteIco");
                 var $newFun;
                 for (var i = 0; i < iconTypeArray.length; i++) {
                     $newFun = $("div.templateClone >div.templateDiv").clone(true);
                     $newFun.attr("title", iconTypeArray[i][1]);
                     $newFun.find("img").attr("src", iconTypeArray[i][0]);
                     $newFun.find("img").attr("title", iconTypeArray[i][1]);
                     $newFun.find("span").html(iconTypeArray[i][1]);
                     $dataDiv.append($newFun);
                 }
             };
             /*
             更新数据
             */
             this.UpdateData = function (funtionInfoText) {
                 window.parent.UpdateWelcome(funtionInfoText);
             }
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
             功能NAME
             */
             this.FunName = "";
             /*
             功能全称
             */
             this.FunFullName = "";
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
<body onLoad="onload_tree();" >
    <form id="form1" runat="server">
   <span id="firstAnchor" name="firstAnchor"></span>
    <div class="promotDiv"> 
    <div id="divPromot" class="promot"></div></div>
    <div class="divNewMain">
    <div id="leftHead" class="divHead">
   <asp:TextBox runat="server" autocomplete="off" ModuleName="" FunFullName="" ID="txtModule" CssClass="txtM" onkeydown="if(this.value=='请选择模块'){return false;}"   ToolTip="请选择模块" Text="请选择模块"   EnableViewState="true"
                                                                               onpaste="return false;" oncontextmenu = "value=value.replace(/[^\a-\z\A-\Z0-9\u4E00-\u9FA5]/g,'')" 
                                                                               MaxLength="50" Style="
                                                                                font-family: Verdana, Arial, Helvetica, sans-serif"></asp:TextBox>
 <asp:TextBox runat="server" autocomplete="off" ID="txtIcon" CssClass="txtI" Text="请选择图标" onfocus="fEvent('focus',this);" ImageUrl=""     ToolTip="请选择图标"   EnableViewState="true"
                                                                               onkeyup="return false;" onkeydown="return false;" onpaste="value=value.replace(/[^\a-\z\A-\Z0-9\u4E00-\u9FA5]/g,'')" oncontextmenu = "value=value.replace(/[^\a-\z\A-\Z0-9\u4E00-\u9FA5]/g,'')" 
                                                                               MaxLength="50" Style="
                                                                                font-family: Verdana, Arial, Helvetica, sans-serif"></asp:TextBox>   
                                                                                          
    </div> <div class="divButton"><input id="btnSave" type="button" class="btnhui" value="保存"/></div></div>
    <div class="main">
    <div class="divMainHead">
    <span>选择模块</span>
    </div>
    <div id="leftContent" class="divContent">
    
    </div>
    </div>
    <div id="contextmenu" style="text-align:center;border:1px solid #666666;background:#eeeeee;width:100%;padding:5px;display:none;position:absolute;"></div>
    <div id="divAbsoluteIco" class="absoluteIco">

    </div>
    <div class="templateClone">
    <div class="templateDiv" title="">
   <div class="templateDivImage"><img src="/Web/images/welcome/1.png" width="50px" height="48px" alt="" title="图标1" /></div> 
   <div class="templateDivText"><span>图标1</span></div>
    </div></div>
    </form>
</body>
</html>
