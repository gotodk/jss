<%@ Page Language="C#" AutoEventWireup="true" CodeFile="left.aspx.cs" Inherits="left" %>

<%@ Register Assembly="RadTreeView.Net2" Namespace="Telerik.WebControls" TagPrefix="radT" %>

<html>
<head id="Head1" runat="server">
    <title>无标题页</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />

<script language="javascript" type="text/javascript" src="Ajax.js"></script>
<script language="javascript" type="text/javascript" src="youjian.js"></script>
    <script src="jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="JSON.js" type="text/javascript"></script>
       <script src="comm.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
    <!--
        function onload_tree() {
            var spVariables = getParam("sp");
        if (spVariables == "") {
            getdata("tree.aspx?mode=getTree&fatherID=0", "divAjax");
        }
        else if (spVariables == "assign") {

            getdata("tree.aspx?mode=getTree&fatherID=0&sp=1", "divAjax");
        }
    }
    function onload_tree_1(text) {
        if (text == "显示所有栏目") {
            window.top.frames.leftFrame.location.href = 'left.aspx?sp=assign';
        }
        else {
            window.top.frames.leftFrame.location.href = 'left.aspx';
        }
    }

    //控制类对象
    var controllerInfo;
    //操作类对象
    var operateInfo;
    var operateInfoText = "";
    var functionInfoText = "";

    function signData(textname) {
                /*
                声明控制类
                和操作类对象
                */

                //实例化控制类对象
                controllerInfo = new ControllerClass();
                //实例化操作类对象
                operateInfo = new OperateInfo();
                operateInfo.OperUser = $("#hidzhanghao").val();
                operateInfoText = JSON.stringify(operateInfo);
        switch (textname) {
            case "CYGZ":
                                operateInfo.OperCate = "常用功能";
                                operateInfo.OperProper = "查看";
                                operateInfoText = JSON.stringify(operateInfo);
                                var functionsInfo = new FunctionsInfo();
                                functionsInfo.FunAuthrity = "";
                                functionsInfo.FunCate = "常用功能";
                                functionsInfo.FunDesc = "欢迎页面";
                                functionsInfo.FunFullName = "欢迎页面";
                                functionsInfo.FunID = "1210000001";
                                functionsInfo.FunIdent = "8";
                                functionsInfo.FunName = "HYYM";
                                functionsInfo.FunParIdColl = "";
                                functionInfoText = JSON.stringify(functionsInfo);
                                controllerInfo.Authority(operateInfoText, functionInfoText);
                break;
            case "XXZZGX":
                operateInfo.OperCate = "常用功能";
                operateInfo.OperProper = "查看";
                operateInfoText = JSON.stringify(operateInfo);
                var functionsInfo = new FunctionsInfo();
                functionsInfo.FunAuthrity = "";
                functionsInfo.FunCate = "常用功能";
                functionsInfo.FunDesc = "个人信息更新";
                functionsInfo.FunFullName = "个人信息更新";
                functionsInfo.FunID = "1210000002";
                functionsInfo.FunIdent = "6";
                functionsInfo.FunName = "SJHZZGX";
                functionsInfo.FunParIdColl = "";
                functionInfoText = JSON.stringify(functionsInfo);
                controllerInfo.Authority(operateInfoText, functionInfoText);
    
                break;
        }



    }

  
    //获取父页面框架参数
    function getParam(paramName) {
        paramValue = "";
        isFound = false;
       
        if (window.top.frames.leftFrame.location.href.indexOf("?") > 0 && window.top.frames.leftFrame.location.href.indexOf("=") > 1) {
            arrSource = unescape(window.top.frames.leftFrame.location.href).substring(window.top.frames.leftFrame.location.href.indexOf("?")+1, window.top.frames.leftFrame.location.href.length).split("&");
            i = 0;
            while (i < arrSource.length && !isFound) {
                if (arrSource[i].indexOf("=") > 0) {
                    if (arrSource[i].split("=")[0].toLowerCase() == paramName.toLowerCase()) {
                        paramValue = arrSource[i].split("=")[1];
                        isFound = true;
                    }
                }
                i++;
            }
        }
        return paramValue;
    }

	function aaa()
	{
	   alert("ss");
	}
    -->
    </script>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" /><style type="text/css">
<!--
body {
	margin-left: 0px;
	margin-top: 0px;
	margin-right: 0px;
	margin-bottom: 0px;
	scrollbar-highlight-color: #ACD373; scrollbar-shadow-color: #ACD373; scrollbar-arrow-color: #ACD373; scrollbar-face-color: #FFFFFF; scrollbar-3dlight-color: #FFFFFF; scrollbar-darkshadow-color: #FFFFFF; scrollbar-track-color: #FFFFFF;
}
-->
</style>

</head>
<body onLoad="onload_tree();" oncontextmenu="showMenu('');" style="background-color:#EDF7FA;">
<form name="form1" id="form1" runat="server">
<div id='divAjax' style="width:100%;overflow-x:hidden;overflow-y:hidden;"></div>
<div id="contextmenu" style="text-align:center;border:1px solid #666666;background:#eeeeee;width:100%;padding:5px;display:none;position:absolute;"></div>
<div id="bDiv" style="width:0px; height:0px;position:absolute;display:none; z-index:0; width:100%;"></div>

      <table width="100%" border="0" cellpadding="0" cellspacing="0">
        <tr>
          <td valign="top" style="height: 100%;">
            <radT:RadTreeView ID="RadTreeView1" runat="server" EnableTheming="True">
            </radT:RadTreeView>
          </td>
        </tr>
      </table>
      
          <!-- 这里用来定义需要显示的右键菜单 -->
    <input type = "hidden" name = "itemMenu_id" id="itemMenu_id" value = "">
    <div id="itemMenu" style="display:none">

<table border="0"  style="font-size:9pt; color:#FFF; width:120;" cellpadding="0" cellspacing="1">
  <tr>
    <td height="23" align="center" valign="middle" bgcolor="#333333" style=" color:#FF9" ><strong>===快捷菜单===</strong></td>
  </tr>
    <tr>
    <td height="23" align="center" valign="middle" bgcolor="#333333" onmouseout="this.style.backgroundColor='#333333'"  onMouseOver="this.style.backgroundColor='#666666'"  style=" cursor:hand;"  onclick ="window.top.frames.leftFrame.location.href='left.aspx'" >更新模块列表</td>
  </tr>
  <tr>
    <td height="23" align="center" valign="middle" bgcolor="#333333"  onmouseout="this.style.backgroundColor='#333333'"  onMouseOver="this.style.backgroundColor='#666666'"  style=" cursor:hand;" onclick="window.top.frames['leftFrame'].open_all();">展开所有模块</td>
  </tr>
  <tr  >
    <td height="23"  align="center" valign="middle" bgcolor="#333333"  onmouseout="this.style.backgroundColor='#333333'"   onMouseOver="this.style.backgroundColor='#666666'" onclick="window.top.frames['leftFrame'].signData('CYGZ')";  style=" cursor:hand;">我的常用工作</td>
  </tr>
  <tr>
    <td height="23" align="center" valign="middle" bgcolor="#333333"  onmouseout="this.style.backgroundColor='#333333'"  onMouseOver="this.style.backgroundColor='#666666'"  onclick="window.top.frames['leftFrame'].signData('XXZZGX')";  style=" cursor:hand;">我的个人信息</td>
  </tr>
</table>

    </div>

    <!-- 右键菜单结束-->
    <asp:hiddenfield ID="hidSp" runat="server"  Value='<%=Request.Params["sp"].ToString()%>'></asp:hiddenfield>
    <asp:hiddenfield ID="hidzhanghao" runat="server"  Value='<%=User.Identity.Name%>'></asp:hiddenfield>
    
</form>
</body>
</html>

