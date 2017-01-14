<%@ Page Language="C#" AutoEventWireup="true" CodeFile="login.aspx.cs" Inherits="login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
 
    <title>批发交易平台子系统-登陆</title>
    <style type="text/css">
         html{ overflow-y:hidden; overflow-x:hidden; }
    *{ margin:0px; padding:0px;}
#content{ min-width:998px; width:100%;  background-image:url(images/loginNew/bg.jpg);background-repeat:repeat-x; background-color:#3774A0; position:absolute;}
#main{ width:991px; margin:auto auto; height:565px;position:static;}
#main01{ width:991px; height:73px; background-image:url(images/loginNew/bg01.png);}
#main02{ width:991px; height:59px; background-image:url(images/loginNew/bg02.png);}
#main03{ width:991px; height:55px; background-image:url(images/loginNew/bg03.png);}
#main05{ width:991px; height:44px; background-image:url(images/loginNew/bg05.png);}
#main06{ width:991px; height:56px; background-image:url(images/loginNew/bg06.png);}
#main07{ width:991px; height:58px; background-image:url(images/loginNew/bg07.png);}
#main08{ width:991px; height:57px; background-image:url(images/loginNew/bg08.png);}
#main09{ width:991px; height:47px; background-image:url(images/loginNew/bg09.png);}

#main10{ width:991px; height:64px; background-image:url(images/loginNew/bg10.png);} 

#center{width:991px;height:52px; }
#center_center{ width:658px;background-image:url(images/loginNew/bg04_center.png); float:left}
#center01{ width:204px;height:52px;background-image:url(images/loginNew/bg04_01.png); float:left;}
#center02{ width:259px;height:52px;float:left;}
#center03{ width:259px;height:52px;float:left;margin-left:15px;}
#center04{ width:100px;height:52px;float:left;margin-left:15px;}
.btn{ width:101px; height:42px; background-image:url(images/loginNew/btnbg.gif); border:none;}
.txt1{ width:255px; height:35px; line-height:35px; padding-left:10px; position:static; font-size:18px;  border:none; color:#666666;}
.txt {background-color: #FFFFFF; border-color: #B3B3B3 #EBEBEB #EBEBEB #B3B3B3;    border-style: solid;    border-width: 1px;  height:22px; width:255px; line-height:22px; padding-top:5px;padding-right:8px;padding-bottom:8px;padding-left:6px; font-size:14px; font-weight:bold;  margin-top:2px; margin-bottom:3px;}
#center05{ width:129px;height:52px;background-image:url(images/loginNew/bg04_02.png); float:right;}
.plr01{padding-left:15px; padding-right:15px; color:#195484; font-size:12px; }
.plr02 { padding-left:10px; padding-right:10px; color:#FFFFFF;  font-size:12px; }

#main10 a{ font-family:宋体; text-decoration:none; color:#FFFFFF; font-size:12px; line-height:150%;}
.innerSpan{ line-height:14px; padding-bottom:3px; }
#logo{ width:415px; height:195px;  background-image:url(images/loginNew/logoNew.png);  margin:0 284px 0px 285px; position:relative; top:80px; z-index:100; }
.mb11{ width:990px; height:14px; position:relative; }
.promot1{height:15px; border:1px solid #EB5359; background:#FFF2F2 url(images/loginNew/icon.png) left 1px no-repeat; font-size:12px;  line-height:18px;  padding-left:18px; padding-top:3px; padding-bottom:3px; position:static; margin-top:93px; margin-left:-274px;  }
.promot{height:52px; border:1px solid #EB5359; background:#FFF2F2 url(images/loginNew/icon.png) left 1px no-repeat; font-size:12px;  line-height:18px;  padding-left:18px; padding-top:3px; padding-bottom:3px; position: relative; margin-top:70px; margin-left:2px; display:none;  }
.clearfloat { clear:both;} 
.promptingHead{ font-family:宋体;font-size:14px; font-weight:bold; padding-left:8px; padding-top:8px;  color:#FFFFFF;}
.promptingBody{ height:25px; line-height:25px; font-family:宋体;font-size:12px; color:#333; padding-left:5px; }
.span{font-family:宋体;font-size:12px; color:#333; line-height:30px; }
.btn1{background:url(images/loginNew/btn.png) no-repeat; border:0px; color:White; font-weight:bold; line-height:26px; width:61px; height:26px; cursor:pointer;}
.emptyText{ border:1px solid #EB5359; display:none;  background:#FFF2F2 url(images/loginNew/emptyNew.png) 1px 1px no-repeat; width:273px; position:absolute; margin-top:-8px; float:left;   font-size:12px;   color:#000000; line-height:18px;   padding-left:23px; }
.falseText{border:1px solid #EB5359; display:none;  background:#FFF2F2 url(images/loginNew/cancelNew.png) 1px 1px no-repeat; width:273px; position:absolute; margin-top:-8px; float:left;   font-size:12px;   color:#000000; line-height:18px;   padding-left:23px;}
    </style>  
     <script language="JavaScript" type="text/javascript">
<!--
         function fEvent(sType, oInput) {
             switch (sType) {
                 case "focus":
                     if (oInput.getAttribute("id") == "Login1_UserName") { //账号
                         document.getElementById("lblForUserName").style.display = "none";
                         document.getElementById("Login1_UserName").focus();
                     }
                     else if (oInput.getAttribute("id") == "Login1_Password") { //密码
                         document.getElementById("lblForPassword").style.display = "none";
                         document.getElementById("Login1_Password").focus();
                     }
                     else if (oInput.getAttribute("id") == "lblForUserName") {  //账号提示文字

                         document.getElementById("lblForUserName").style.display = "none";
                         document.getElementById("Login1_UserName").focus();
                     }
                     else if (oInput.getAttribute("id") == "lblForPassword") { //密码提示文字

                         document.getElementById("lblForPassword").style.display = "none";
                         document.getElementById("Login1_Password").focus();
                     }
                 case "mouseover":
                     //                     oInput.style.borderColor = '#9ecc00';
                     break;
                 case "blur":
                     oInput.isfocus = false;
                     if (oInput.getAttribute("id") == "Login1_UserName") { //账号
                         if (oInput.value == "") {
                             document.getElementById("lblForUserName").style.display = "inline-block";
                         }
                     }
                     else if (oInput.getAttribute("id") == "Login1_Password") { //密码
                         if (oInput.value == "") {
                             document.getElementById("lblForPassword").style.display = "inline-block";
                         }
                     }
                 case "mouseout":
                     //                     if (!oInput.isfocus) {
                     //                         oInput.style.borderColor = '#84a1bd';
                     //                     }
                     break;
             }
         }

         function showload() {
             document.getElementById("ReStart").style.display = "none";
             if (document.getElementById('Login1_UserName').value.replace(/(^\s*)|(\s*$)/g, "") != "" && document.getElementById('Login1_Password').value.replace(/(^\s*)|(\s*$)/g, "") != "") {

                 document.getElementById('FailureTextDiv').style.display = "none";
                 document.getElementById('logining').style.display = "block";
             }

             if (document.getElementById('Login1_UserName').value.replace(/(^\s*)|(\s*$)/g, "") == "" || document.getElementById('Login1_Password').value.replace(/(^\s*)|(\s*$)/g, "") == "") {
                 document.getElementById('FailureTextDiv').innerHTML = "";
                 document.getElementById('FailureTextDiv').style.display = "none";
                 document.getElementById('logining').style.display = "none";
             }

             if (document.getElementById('Login1_UserName').value.replace(/(^\s*)|(\s*$)/g, "") == "" && document.getElementById('Login1_Password').value.replace(/(^\s*)|(\s*$)/g, "") == "") {
                 document.getElementById('FailureTextDiv').style.display = "block";
                 document.getElementById('FailureTextDiv').innerHTML = "用户名不能为空！密码不能为空！";
                 document.getElementById('FailureTextDiv').className = "emptyText";
             }
             else if (document.getElementById('Login1_UserName').value.replace(/(^\s*)|(\s*$)/g, "") == "") {
                 document.getElementById('FailureTextDiv').style.display = "block";
                 document.getElementById('FailureTextDiv').innerHTML = "用户名不能为空！";
                 document.getElementById('FailureTextDiv').className = "emptyText";
             }
             else if (document.getElementById('Login1_Password').value.replace(/(^\s*)|(\s*$)/g, "") == "") {
                 document.getElementById('FailureTextDiv').style.display = "block";
                 document.getElementById('FailureTextDiv').innerHTML = "密码不能为空！";
                 document.getElementById('FailureTextDiv').className = "emptyText";
             }
         }

         function IsReady() { //判断在页面加载完毕以后，账号的情况，同时对不同情况作出处理
             if (document.getElementById("Login1_UserName").value != "") { //如果账号不为空，则提示信息不显示
                 document.getElementById("lblForUserName").style.display = "none";
             }
             else {  //如果账号为空则提示信息显示
                 //                 document.getElementById("lblForUserName").style.display = "inline-block";
                 document.getElementById("lblForUserName").style.display = "none";
                 document.getElementById("Login1_UserName").focus();
             }
             //如果记住了用户名和密码
             if (document.getElementById('Login1_UserName').value.replace(/(^\s*)|(\s*$)/g, "") == "" && document.getElementById('Login1_Password').value.replace(/(^\s*)|(\s*$)/g, "") == "") {
                 document.getElementById('FailureTextDiv').innerHTML = "";
                 document.getElementById('FailureTextDiv').style.display = "none";
             }
             //             //对于不存在的账号
             //             if (document.getElementById('Login1_UserName').value.replace(/(^\s*)|(\s*$)/g, "") != "" && document.getElementById('Login1_Password').value.replace(/(^\s*)|(\s*$)/g, "") == "") {
             //                 document.getElementById('FailureTextDiv').style.display = "inline-block";
             //             }
         }
        //-->
    </script>
    <script src="js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        function JSbtnOK() {
            if (document.getElementById("txtOldPW").value == "") {
                document.getElementById("span").innerHTML = "原密码不能为空！";
                return false;
            }
            else {
                var oldpw = document.getElementById("lblUser").innerHTML;
                $.ajax({
                    type: "post",
                    url: "login-ajax.aspx",
                    data: { "sl": oldpw },
                    success: function (data) {
                        if (data == null || data == "") {
                            document.getElementById("span").innerHTML = "不存在此用户！";
                            return false;
                        }
                        else if (document.getElementById("txtOldPW").value != data) {
                            document.getElementById("span").innerHTML = "原密码输入错误！";
                            return false;
                        }
                    }
                });
            }
            if (document.getElementById("txtNewPassWord").value != document.getElementById("txtQRNewPW").value) 
            {
                document.getElementById("span").innerHTML = "两次输入密码不相同！";
                return false;            
            }
            else 
            {
                if (document.getElementById("txtNewPassWord").value == "") {
                    document.getElementById("span").innerHTML = "新密码不能为空！";
                    return false;
                }
                else if (!document.getElementById("txtNewPassWord").value.match(/\d+[a-zA-Z]+|[a-zA-Z]+\d+/))
                {
                    document.getElementById("span").innerHTML = "操作失败，请按规则修改密码！";
                    return false;
                }
                else if (document.getElementById("txtNewPassWord").value.length <= 6 || document.getElementById("txtNewPassWord").value.length >20)
                {
                    document.getElementById("span").innerHTML = "操作失败，请按规则修改密码！";
                    return false;
                }
            }
        }
        //为修改密码的确定按钮绑定快捷键Enter键
        $(document).keydown(function (e) {
            var url = window.location.search;
            if (url.indexOf("bl") != -1) {
                if (event.keyCode == 13) {
                    var btnok = document.getElementById("btnOK");
                    if (btnok != null)//判断div显示时，为修改密码的确定按钮绑定快捷键Enter键
                    {
                        document.getElementById("btnOK").click();
                        event.returnValue = false; //取消回车键的默认操作 
                    }
                    
                }
            }
        });

        $(document).ready(function () {
            var url = window.location.search;
            if (url.indexOf("ud=ok") != -1) {
                if (document.getElementById('FailureTextDiv').style.display == "inline-block") {
                    document.getElementById("ReStart").style.display = "none";
                }
                else if (document.getElementById('FailureTextDiv').style.display == "none") {
                    document.getElementById("ReStart").style.display = "block";
                }
            }
        });
    </script>
</head>
<body onload="IsReady()" style=" background-color:#3774A0;">
     <form id="form1" runat="server">
   <div id="content">
   <div style=" width:991px; margin:auto auto; height:195px;position:static;">
   <div id="logo"></div></div>
<div id="main">
  <div id="main01"></div>
  <div id="main02"></div>
  <div id="main03"></div>
  <div id="center">
    <div id="center01"></div>
    <div id="center_center">
       <asp:Login ID="Login1" runat="server" DestinationPageUrl = "Web/Default.aspx"
            onloginerror="Login1_LoginError" onloggedin="Login1_LoggedIn"   >
       <LayoutTemplate>
       <div id="center02">
       <label id="lblForUserName" for="UserName"  onclick="fEvent('focus',this)"  style=" position:absolute;  margin-top:13px;    line-height:16px; padding-left:10px; font-size:16px; color:#999; font-family:宋体,12px/1.8 Tahoma, Geneva; display:inline-block; ">请输入用户名</label>
      <asp:TextBox runat="server" autocomplete="off" ID="UserName" CssClass="txt"  ToolTip="请输入用户名"  EnableViewState="true"
                                                                                onfocus="fEvent('focus',this)" onblur="fEvent('blur',this)" onkeyup="value=value.replace(/[^\a-\z\A-\Z0-9\u4E00-\u9FA5]/g,'')" onpaste="value=value.replace(/[^\a-\z\A-\Z0-9\u4E00-\u9FA5]/g,'')" oncontextmenu = "value=value.replace(/[^\a-\z\A-\Z0-9\u4E00-\u9FA5]/g,'')" 
                                                                               MaxLength="50" Style="font-weight: bold;
                                                                                font-family: Verdana, Arial, Helvetica, sans-serif"></asp:TextBox> 
                                                                             <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName" CssClass="promot"
                                                                                ErrorMessage="用户名不能为空！" ForeColor="#000000"  ToolTip="用户名不能为空！" ValidationGroup="Login1"></asp:RequiredFieldValidator>
      </div>
      <div id="center03">
     
       <label id="lblForPassword" onclick="fEvent('focus',this)" for="Password"   style=" position:absolute;  margin-top:13px;  line-height:16px; padding-left:10px; font-size:16px; color:#999;  font-family:宋体,12px/1.8 Tahoma, Geneva; display:inline-block;  ">请输入密码</label>
         <asp:TextBox ID="Password" autocomplete="off" runat="server" ToolTip="请输入密码" EnableViewState="true"   TextMode="Password" CssClass="txt "  
                                                                                onfocus="fEvent('focus',this)" onblur="fEvent('blur',this)" onkeyup="value=value.replace(/[^\a-\z\A-\Z0-9\u4E00-\u9FA5\_\#]/g,'')" onpaste="value=value.replace(/[^\a-\z\A-\Z0-9\u4E00-\u9FA5]/g,'')" oncontextmenu = "value=value.replace(/[^\a-\z\A-\Z0-9\u4E00-\u9FA5]/g,'')"
                                                                                MaxLength="50" Style="font-weight: bold;
                                                                                font-family: Verdana, Arial, Helvetica, sans-serif"></asp:TextBox>
                                                                                  <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password" CssClass="promot" 
                                                                                ErrorMessage=" 密码不能为空！" ForeColor="#000000"  ToolTip=" 密码不能为空！" ValidationGroup="Login1"></asp:RequiredFieldValidator>
      </div>
      <div id="center04">
           <asp:ImageButton ID="ImageButton1" runat="server" CommandName="Login"  ImageUrl="~/images/loginNew/btnbg.gif"  CssClass="btn" OnClientClick="showload()" ValidationGroup="Login1"  />
              <span style="height:50px; font-size:12px;  line-height:16px;  padding-left:10px; position: relative; margin-top:50px; margin-left:0px; "><%--<a href="#" tamade="header=[<span class='promptingHead'>友情提示</span>] body=[<span class='promptingBody'>&#8226;人力资源部建立员工档案，生成济商所登录账号。</span><br><span class='promptingBody'>&#8226;用户名为“员工工号”，默认密码为“888888”。</span><br><span class='promptingBody'>&#8226;员工应妥善保管工号和密码，不得将账户信息转交他人使用，不得使用他人账户信息登录。</span><br><span class='promptingBody'>&#8226;首次登录后应立即修改默认密码，新设置密码至少7位，并区分大小写。</span><br><span class='promptingBody'>&#8226;必须使用较为复杂的密码，不易被别人冒用，避免因密码保管不善造成信息泄露。</span><br><span class='promptingBody'>&#8226;忘记密码，总部员工携带本人工卡到信息化中心登记备案；办事处员工联系对口行政商务，由</span><br><span class='promptingBody'>&nbsp;&nbsp;对口行政商务到信息化中心进行登记备案；信息化中心安排专人负责恢复默认密码。</span>] fade=[off] " style="text-decoration:none; color:#FFFFFF;  font-family:宋体;">帮助</a>--%><a href="#" tamade="header=[<div class='promptingHead'>友情提示</div>] body=[<span class='promptingBody'>&#8226;人力资源部为员工在济商所建立档案，系统自动生成员工登录账户。</span><br><span class='promptingBody'>&#8226;登录用户名为“员工工号”，密码默认为“888888”。首次登录时，系统将提示立即按规则</span><br><span class='promptingBody'>&nbsp;重新设置密码。</span><br><span class='promptingBody'>&#8226;员工应妥善保管工号和密码，不得将账户信息转交他人使用，不得使用他人账户信息登录，</span><br><span class='promptingBody'>&nbsp;避免因密码保管不善造成信息泄露。</span><br><span class='promptingBody'>&#8226;员工忘记密码或密码锁定时，携本人工卡（分公司员工联系对口行政商务）到信息化中心登</span><br><span class='promptingBody'>&nbsp;记，恢复系统默认密码。</span>] fade=[off] " style="text-decoration:none; color:#FFFFFF;  font-family:宋体;">帮助</a></span>
      </div>
      <div class="clearfloat"></div>
      <div id="FailureTextDiv" class="falseText">
    <asp:Literal ID="FailureText"  runat="server" EnableViewState="False" ></asp:Literal>
      </div>
      <div id="logining" style=" display:none;  border:1px solid #EB5359;     background:#FFF2F2 url(images/loginNew/okNew.png) 1px 1px no-repeat;  width:173px; font-size:12px; position:absolute;float:left; margin-top:-8px;  color:#000000; line-height:18px; padding-left:23px;  ">正在登陆业务平台，请稍后……</div>
    <div id="ReStart" style=" display:none;  border:1px solid #EB5359;     background:#FFF2F2 url(images/loginNew/ok.png) left 0px no-repeat;  width:173px; font-size:12px; position:absolute;float:left; margin-top:-7px;  color:#000000; line-height:21px; padding-left:25px;  ">密码修改成功，请重新登陆！</div>
       </LayoutTemplate>
          </asp:Login>
    </div>
    <div id="center05"></div>
  </div>
    <div class="clearfloat"></div>
   <div id="main05"></div>
  <div id="main06"></div>
  <div id="main07"></div>
  <div id="main08"></div>
  <div id="main09"></div>
  <div id="main10">

  </div>
</div>
</div>
    <div id="divUpdatePassWord" runat="server"  visible="false"  style="display: block; position: absolute; width:100%; height:auto;top: 270px;">
        <div style=" width:991px; height:auto;margin:auto;">                                                                                          <div style="
     margin:auto; width: 600px; background-color:White;
    z-index: 999; height:auto; border:1px solid #0068B6; " align="left">
        <div style="  height:30px; width:100%; background:url(images/loginNew/bg20120829_03.png); overflow:hidden; border:0px;">
            <span style=" color:white; font-white:bold; line-height:30px; padding-left:20px; font-weight:bold; float:left; font-size:14px;">修改密码</span>
            <asp:ImageButton ID="imgbtnClose" runat="server" 
                ImageUrl="~/images/loginNew/close.png" 
                style=" float:right; padding-right:10px; margin-top:8px;cursor:pointer;" 
                onclick="imgbtnClose_Click"/>
        </div>          
        <div style=" width:100%; height:auto; margin:auto;" >  
            <div style=" float:left; width:40px; background-color:White;height:350px;"></div>   
            <div style=" float:left; width:520px; background-color:White;height:350px;">     
            <table style=" border:0px; padding:0px; margin:0px;" cellpadding="0" cellspacing="0">
                <tr style=" height:10px;"><td colspan="2"></td></tr>
                <tr>
                    <td colspan="2">
                        <span class="span">抱歉！<label id="lblUser" runat="server" style="font-size:16px;  font-weight:bold; color:#00416E;"></label><label style="font-size:12px; color:#00416E;">&nbsp;用户</label></span> 
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <span  class="span">
                            您现有密码不符合安全规则，暂时不能使用济商所，请重新设定密码。
                        </span>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <span class="span">
                            1、新密码设定规则：7-20个字符，请使用字母加数字的组合，不能单独使用字母、数字，不能包含特殊符号，字母区分大小写。
                        </span>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <span  class="span">
                            2、请妥善保管密码信息，避免造成信息泄露。
                        </span>
                    </td>
                </tr>
                <tr style=" height:15px;">
                    <td></td>
                    <td>
                        <span id="span" runat="server"  class="span" style="color:Red; line-height:15px;"></span>
                    </td>
                </tr>
                <tr>
                <td style=" width:35%; text-align:right; height:25px;">
                    <span  class="span">原密码：</span>
                </td>
                <td>
                    <asp:TextBox ID="txtOldPW" runat="server" TextMode="Password" onfocus="fEvent('focus',this)" 
                                                                                MaxLength="50" border-color=" #B3B3B3 #EBEBEB #EBEBEB #B3B3B3" border-style="solid" border-width="1px" Height="22px" style=" line-height:22px;"  ></asp:TextBox>
                </td>
            </tr>
            <tr style=" height:10px;"><td colspan="2"></td></tr>
            <tr>
                <td style=" width:35%; text-align:right; height:20px;">
                    <span class="span">新密码：</span>
                </td>
                <td>
                    <asp:TextBox ID="txtNewPassWord" runat="server" TextMode="Password" onfocus="fEvent('focus',this)" onblur="fEvent('blur',this)" onkeyup="value=value.replace(/[^\a-\z\A-\Z0-9\u4E00-\u9FA5\_\#]/g,'')" onpaste="value=value.replace(/[^\a-\z\A-\Z0-9\u4E00-\u9FA5]/g,'')" oncontextmenu = "value=value.replace(/[^\a-\z\A-\Z0-9\u4E00-\u9FA5]/g,'')"
                                                                                MaxLength="50" border-color=" #B3B3B3 #EBEBEB #EBEBEB #B3B3B3" border-style="solid" border-width="1px"  Height="22px" style=" line-height:22px;" ></asp:TextBox>
                </td>
            </tr>
            <tr style=" height:10px;"><td colspan="2"></td></tr>
            <tr>
                <td style=" width:35%; text-align:right; height:20px;">
                    <span class="span">&nbsp;确认新密码：</span>
                </td>
                <td>
                    <asp:TextBox ID="txtQRNewPW" runat="server" TextMode="Password" onfocus="fEvent('focus',this)" onblur="fEvent('blur',this)" onkeyup="value=value.replace(/[^\a-\z\A-\Z0-9\u4E00-\u9FA5\_\#]/g,'')" onpaste="value=value.replace(/[^\a-\z\A-\Z0-9\u4E00-\u9FA5]/g,'')" oncontextmenu = "value=value.replace(/[^\a-\z\A-\Z0-9\u4E00-\u9FA5]/g,'')"
                                                                               MaxLength="50" border-color=" #B3B3B3 #EBEBEB #EBEBEB #B3B3B3" border-style="solid" border-width="1px"  Height="22px" style=" line-height:22px;" ></asp:TextBox>
                </td>
            </tr>
            <tr style=" text-align:left; ">
                <td></td>
                <td style=" height:50px;">                        
                    <asp:Button ID="btnOK" runat="server" CssClass="btn1"
                        Text="确定" OnClientClick=" return JSbtnOK()" onclick="btnOK_Click" />&nbsp;&nbsp;
                    <asp:Button ID="btnCancel" runat="server" CssClass="btn1"  Text="取消" onclick="btnCancel_Click"/>
                </td>
            </tr>
            </table>  
            </div>  
            <div style=" float:left; width:40px; background-color:White;height:350px;"></div>          
        </div>
    </div>
        </div>
    </div>
   </form>
</body>
<script src="js/boxoverNew.js" type="text/javascript"></script>
</html>