<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChangePassNew.aspx.cs" Inherits="Web_ChangePassNew" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        body
        {
            background-color: White;
        }
        .divMain
        {
            width: 600px;
            height: 100%;
            padding: 17px;
            margin: 50px auto;
        }
        .diivHeadContent
        {
            width: 100%;
            height: 280px;
        }
        .divFootContent
        {
            width: 100%;
            height: 150px;
            margin-top: 20px;
            padding: 10px 10px 10px 26px;
        }
        .divHead
        {
            width: 100%;
            height: 36px;
            font-family: "宋体";
            color: #333333;
            font-size: 20px;
            font-weight: bolder;
            padding: 5px 10px 5px 25px;
        }
        .divPromptingMx
        {
            width: 100%;
            height: 20px;
        }
        .divPromoting
        {
            width: 290px;
            height: 20px;
            margin-left: 100px;
            border: 1px solid #EB5359;
            background: #FFF2F2 url(images/icon.png) left 1px no-repeat;
            font-size: 12px;
            padding-left: 18px;
            padding-top: 4px;
            display: none;
            font-family:宋体;font-size:12px; color:#333;
        }
        .divFormTable
        {
            width: 100%;
            height: 150px;
            margin-top: 10px;
        }
        .FormTablTr
        {
            width: 100%;
            border: 0px;
            padding: 0px;
        }
        .FormTablTd
        {
            width: 32%;
            text-align: right;
            padding: 12px 0px;
        }
        .promptingBody
        {
            font-size: 14px;
            font-family: "宋体";
            color:#333;
        }
        .txt
        {
            background-color: #FFFFFF;
            border-color: #B3B3B3 #EBEBEB #EBEBEB #B3B3B3;
            border-style: solid;
            border-width: 1px;
            height: 22px;
            width: 200px;
            line-height: 22px;
            padding-top: 0px;
            padding-right: 8px;
            padding-bottom: 1px;
            padding-left: 6px;
            font-size: 14px;
            font-weight: bold;
            margin-top: 2px;
            margin-bottom: 3px;
        }
        .btnhui
        {
            border: 1px solid #c4c4c4;
            height: 25px;
            background-color: #F0F0F0;
            cursor: pointer;
            min-width: 70px;
            margin: 0;
            padding: 0;
            color: #333333;
            font-family: "\5b8b\4f53" , "Tamoda" , "Arial" ,Sans-serif;
            font-size: 12px;
        }
        .divSubstance
        {
            height:25px; line-height:25px; font-family:宋体;font-size:12px; color:#333;
        }
    </style>
    <script src="jquery-1.7.2.min.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        function fEvent(sType, oInput) {
            switch (sType) {
                case "focus":
                    break;
                case "mouseover":

                    break;
                case "blur":

                    break;

                case "mouseout":

                    break;
            }
        }

        function IsSucess() {
        
            var userName = "<%= User.Identity.Name.ToString()%>";
          
                        var str = "";
                        var txtOldPWValue = $("#" + "<%=txtOldPW.ClientID %>").val();
                        var txtNewPassWordValue = $("#" + "<%=txtNewPassWord.ClientID %>").val();
                        var txtQRNewPWValue = $("#" + "<%=txtQRNewPW.ClientID %>").val();
                        if (txtOldPWValue == "" && txtNewPassWordValue == "" && txtQRNewPWValue == "") {
                            str = "请输入原始密码、新密码、确认密码！";
                        }
                        else if (txtOldPWValue != "" && txtNewPassWordValue == "" && txtQRNewPWValue == "") {
                            str = "请输入新密码、确认密码！";
                        }
                        else if (txtOldPWValue == "" && txtNewPassWordValue != "" && txtQRNewPWValue == "") {
                            str = "请输入原始密码、确认密码！";
                        }
                        else if (txtOldPWValue == "" && txtNewPassWordValue == "" && txtQRNewPWValue != "") {
                            str = "请输入原始密码、新密码！";
                        }
                        else if (txtOldPWValue == "" && txtNewPassWordValue != "" && txtQRNewPWValue != "") {
                            str = "请输入原始密码！";
                        }
                        else if (txtOldPWValue != "" && txtNewPassWordValue == "" && txtQRNewPWValue != "") {
                            str = "请输入新密码！";
                        }
                        else if (txtOldPWValue != "" && txtNewPassWordValue != "" && txtQRNewPWValue == "") {
                            str = "请输入确认密码！";
                        }
                        else {
                            $.ajax({ "url": "ChangePassNew.ashx?Methord=PassWordCheck", "type": "post", "data": { "PassWordValue": txtOldPWValue, "UserName": userName }, "dataType": "json", "cache": false, "async": false, "success": function (responseInfo) {
                                if (responseInfo == true) {
                                    //                        debugger;
                                    var validate = /\d+[a-zA-Z]+|[a-zA-Z]+\d+/g;
                                    if (txtNewPassWordValue != txtQRNewPWValue) {
                                        str = "新密码与确认密码不相同，请重新设定密码！";
                                    }
                                    else {
                                        if (txtNewPassWordValue.length > 6 && txtNewPassWordValue.length <= 20 && validate.test(txtNewPassWordValue)) {
                                            str = "";
                                        }
                                        else {
                                            str = "请按安全规则修改密码！";
                                        }

                                    }
                                }
                                else {
                                    str = "原始密码输入错误！";
                                }
                            }
                            });
                    }
                    if (str == "") {
                        return true;
                    }
                    else {
                        $("#span").html(str);
                        $("#span").css({ "display": "inline-block" });
                        return false;
                    }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="divMain">
        <div class="diivHeadContent">
            <div class="divHead">
                修改密码</div>
            <div class="divPromptingMx">
                <div id="span" runat="server" class="divPromoting">
                </div>
            </div>
            <div class="divFormTable">
                <table width="100%">
                    <tr class="FormTablTr">
                        <td class="FormTablTd">
                            <span class='promptingBody'>请输入原密码：</span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtOldPW" runat="server" CssClass="txt" TextMode="Password" onkeyup="value=value.replace(/[^\a-\z\A-\Z0-9\u4E00-\u9FA5\_\#]/g,'')"
                                onpaste="value=value.replace(/[^\a-\z\A-\Z0-9\u4E00-\u9FA5]/g,'')" oncontextmenu="value=value.replace(/[^\a-\z\A-\Z0-9\u4E00-\u9FA5]/g,'')"
                                MaxLength="50"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="FormTablTr">
                        <td class="FormTablTd">
                            <span class='promptingBody'>新密码：</span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtNewPassWord" runat="server" CssClass="txt" TextMode="Password"
                                onkeyup="value=value.replace(/[^\a-\z\A-\Z0-9\u4E00-\u9FA5\_\#]/g,'')" onpaste="value=value.replace(/[^\a-\z\A-\Z0-9\u4E00-\u9FA5]/g,'')"
                                oncontextmenu="value=value.replace(/[^\a-\z\A-\Z0-9\u4E00-\u9FA5]/g,'')" MaxLength="50"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="FormTablTr">
                        <td class="FormTablTd">
                            <span class='promptingBody'>&nbsp;确认新密码：</span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtQRNewPW" runat="server" CssClass="txt" TextMode="Password" onkeyup="value=value.replace(/[^\a-\z\A-\Z0-9\u4E00-\u9FA5\_\#]/g,'')"
                                onpaste="value=value.replace(/[^\a-\z\A-\Z0-9\u4E00-\u9FA5]/g,'')" oncontextmenu="value=value.replace(/[^\a-\z\A-\Z0-9\u4E00-\u9FA5]/g,'')"
                                MaxLength="50"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="FormTablTr">
                        <td class="FormTablTd">
                        </td>
                        <td align="left">
                            <asp:Button ID="btnSubmit" CssClass="btnhui" runat="server" Text="提交" 
                                OnClientClick="return  IsSucess()"  onclick="btnSubmit_Click" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <hr />
        <div class="divFootContent">
            <div style="font-size: 14px; font-family: 宋体; font-weight: bold; color: #374451;">
                须知:</div>
            <div>
              <span class="divSubstance">1、新密码的设置规则：7-20个字符，请使用字母加数字的组合密码，不能单独使用字母、数字，</span> </div>
              <div><span class="divSubstance" style=" padding-left:15px;">不能包含特殊符号，字母区分大小写。</span></div>
            <div>
               <span class="divSubstance">  2、请妥善保管密码信息，避免造成信息泄露。</span></div>
        </div>
    </div>
    </form>
</body>
</html>
