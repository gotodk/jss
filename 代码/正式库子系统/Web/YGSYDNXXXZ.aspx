<%@ Page Language="C#" AutoEventWireup="true" CodeFile="YGSYDNXXXZ.aspx.cs" Inherits="Web_YGSYDNXXXZ" %>

<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../css/style.css" rel="Stylesheet" type="text/css" />
    <link href="yhb_BigPage_css/WhiteChromeGridView20120619.css" rel="Stylesheet" type="text/css" />
    <script src="../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        function IPEditFinished(obj) {
            //IP输入框只能输入数字            
            var len = obj.value.length;
            var IPValue = obj.value.substring(len - 1, len);            
            if (obj.id == "txtIP1") {
                if (IPValue.charCodeAt() == 46 || IPValue.charCodeAt() == 32) {
                        obj.value = obj.value.replace(/[^0-9]/g, '');
                        document.getElementById("txtIP2").focus();
                        document.getElementById("txtIP2").select();
                    }
                    //数字
                    else if (IPValue.charCodeAt() <= 47 || IPValue.charCodeAt() >= 58) {
                        obj.value = obj.value.replace(/[^0-9]/g, '');
                    }
                }
                else if (obj.id == "txtIP2") {
                    if (IPValue.charCodeAt() == 46 || IPValue.charCodeAt() == 32) {
                        obj.value = obj.value.replace(/[^0-9]/g, '');
                        document.getElementById("txtIP3").focus();
                        document.getElementById("txtIP3").select();
                    }
                    //数字
                    else if (IPValue.charCodeAt() <= 47 || IPValue.charCodeAt() >= 58) {
                        obj.value = obj.value.replace(/[^0-9]/g, '');
                    }
                }
                else if (obj.id == "txtIP3") {
                    if (IPValue.charCodeAt() == 46 || IPValue.charCodeAt() == 32) {
                        obj.value = obj.value.replace(/[^0-9]/g, '');
                        document.getElementById("txtIP4").focus();
                        document.getElementById("txtIP4").select();
                    }
                    //数字
                    else if (IPValue.charCodeAt() <= 47 || IPValue.charCodeAt() >= 58) {
                        obj.value = obj.value.replace(/[^0-9]/g, '');
                    }
                }
                else {
                    if (IPValue.charCodeAt() == 46 || IPValue.charCodeAt() == 32) {
                        obj.value = obj.value.replace(/[^0-9]/g, '');
                    }
                    //数字
                    else if (IPValue.charCodeAt() <= 47 || IPValue.charCodeAt() >= 58) {
                        obj.value = obj.value.replace(/[^0-9]/g, '');
                    }
                }
                
            //判断输入字符串的长度，长度如果等于3则下一个IP输入框获得焦点
            if (obj.value.toString().length == 3) {
                if (obj.id == "txtIP1") {
                    document.getElementById("txtIP2").focus();
                    document.getElementById("txtIP2").select();
                }
                else if (obj.id == "txtIP2") {
                    document.getElementById("txtIP3").focus();
                    document.getElementById("txtIP3").select();
                }
                else if (obj.id == "txtIP3") {
                    document.getElementById("txtIP4").focus();
                    document.getElementById("txtIP4").select();
                }
                else {
                    
                }
            }
            
        }
        function MacEditFinished(obj) {            
            var len = obj.value.toString().length;
            var MacValue = "";
            for(var i =1; i<=len;i++)
            {
                var val = obj.value.substring(i-1,i);
                var a = val.charCodeAt(); //转换成十进制的ACSII码值
                //小写字母转换成大写字母,除去字母"o"
                if ((a > 96 && a < 111) || (a > 111 && a < 123)) {
                    MacValue += String.fromCharCode(a - 32);
                }
                //数字
                else if (a > 47 && a < 58) {
                    MacValue += String.fromCharCode(a);
                }
                //大写字母
                else if ((a > 64 && a < 78) || (a > 79 && a < 91)) {
                    MacValue += String.fromCharCode(a);
                }
                //小写或大写的字母"o"转换成数字"0"
                else if(a ==79 || a==111)
                {
                    MacValue += String.fromCharCode(48);
                }
                else {
                    MacValue += "";
                }
            }
            obj.value = MacValue;
            //判断输入字符串的长度，长度如果等于4则下一个MAC输入框获得焦点
            if (obj.value.toString().length == 4) {
                if (obj.id == "txtMAC1") {
                    document.getElementById("txtMAC2").focus();
                    document.getElementById("txtMAC2").select();
                }
                else if (obj.id == "txtMAC2") {
                    document.getElementById("txtMAC3").focus();
                    document.getElementById("txtMAC3").select();
                }
                else {

                }
            }
        }
        //将英文的单引号转换成中文的单引号
        function CharEditFinished(obj) {
            var len = obj.value.toString().length;
            var MacValue = "";
            for (var i = 1; i <= len; i++) {
                var val = obj.value.substring(i - 1, i);
                var a = val.charCodeAt(); //转换成十进制的ACSII码值
                
                if (a == 39) {
                    MacValue += String.fromCharCode(8217);
                }
                else {
                    MacValue += String.fromCharCode(a);
                }
            }
            obj.value = MacValue;
            
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="divXZ" runat="server" visible="true" style="display: block;">
        <radTS:RadTabStrip ID="TabStrip1" runat="server" CausesValidation="False" ReorderTabRows="True"
            Skin="Default2006" Width="970px">
            <Tabs>
                <radTS:Tab ID="Tab3" runat="server" Text="员工使用电脑信息新增" Font-Size="12px" ForeColor="red"
                    NavigateUrl="YGSYDNXXXZ.aspx">
                </radTS:Tab>
                <radTS:Tab ID="Tab1" runat="server" Text="员工使用电脑信息查看及修改" Font-Size="12px" NavigateUrl="YGSYDNXXCK.aspx">
                </radTS:Tab>
            </Tabs>
        </radTS:RadTabStrip>
    </div>
    <table width="800px" border="0" cellspacing="0" cellpadding="0" class="FormView">
        <tr>
            <td colspan="2" height="20px">
            </td>
        </tr>
        <tr>
            <td width="20px">
            </td>
            <td>
                <div id="div3" style="border: 1px solid #C3D9FF; width: 750px">
                    <table width="750" cellpadding="0" cellspacing="0">
                        <tr>
                            <td colspan="6" style="height: 20px;">
                            </td>
                        </tr>
                        <tr style="height: 30px;">
                            <td width="100" align="right" valign="middle">
                                <span style="color: Red">*</span> 所属部门：
                            </td>
                            <td width="200px" align="left" valign="middle">
                                <asp:DropDownList ID="drpSSBM" runat="server" 
                                    AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td width="150" align="right" valign="middle">
                                <span style="color: Red">*</span>使用人：
                            </td>
                            <td width="350px" align="left" valign="middle">
                                <asp:TextBox ID="txtSYR" runat="server" Width="150px" MaxLength="25" CssClass="input1" onkeyup="CharEditFinished(this)"></asp:TextBox>
                            </td>
                        </tr>
                        <tr height="30px">
                            <td align="right" valign="middle">
                                <span style="color: Red">*</span>IP地址：
                            </td>
                            <td align="left" valign="middle">
                                <input id="txtIP1" type="text" runat="server" style="text-align: center; width:40px;"
                                    onkeyup="IPEditFinished(this)" maxLength="3" class="input1" /><span
                                        style="font-size: 14px; font-weight: bold;">.</span>
                                <input id="txtIP2" type="text" runat="server" style="text-align: center; width:40px;"
                                    onkeyup="IPEditFinished(this)" maxLength="3" class="input1" /><span
                                        style="font-size: 14px; font-weight: bold;">.</span>
                                <input id="txtIP3" type="text" runat="server" style="text-align: center; width:40px;"
                                    onkeyup="IPEditFinished(this)" maxLength="3" class="input1" /><span
                                        style="font-size: 14px; font-weight: bold;">.</span>
                                <input id="txtIP4" type="text" runat="server" style="text-align: center; width:40px;"
                                    onkeyup="IPEditFinished(this)" maxLength="3" class="input1" />
                            </td>
                            <td align="right" valign="middle">
                                <span style="color: Red">*</span>MAC地址：
                            </td>
                            <td align="left" valign="middle">
                                <input id="txtMAC1" type="text" runat="server" style="text-align: center; width:40px;"
                                    onkeyup="MacEditFinished(this)" maxLength="4" class="input1" />&nbsp;—
                                <input id="txtMAC2" type="text" runat="server" style="text-align: center; width:40px;"
                                    onkeyup="MacEditFinished(this)" maxLength="4" class="input1" />&nbsp;—
                                <input id="txtMAC3" type="text" runat="server" style="text-align: center; width:40px;"
                                    onkeyup="MacEditFinished(this)" maxLength="4" class="input1" />
                            </td>
                        </tr>
                        <tr height="80px">
                            <td align="right" valign="middle">
                                <span style="color: Red">*</span> 配置信息：
                            </td>
                            <td align="left" valign="middle" colspan="3">
                                <asp:TextBox ID="txtPZ" runat="server" Height="50px" TextMode="MultiLine" Width="500px" onkeyup="CharEditFinished(this)"></asp:TextBox>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="left">
                                <table border="0" cellpadding="0" cellspacing="0" width="750px">
                                    <tr height="30px">
                                        <td width="100px" align="right" valign="middle">
                                            内网：
                                        </td>
                                        <td width="120px" align="left" valign="middle">
                                            <input id="rdnws" type="radio" value="是" name="neiwang" runat="server" />是&nbsp;&nbsp;
                                            <input id="rdnwf" type="radio" value="否" name="neiwang" checked="true" runat="server" />否
                                        </td>
                                        <td width="130px" align="right" valign="middle">
                                            内网使用期限：
                                        </td>
                                        <td width="400px" align="left" valign="middle">
                                            <div style="float: left;">
                                                <asp:DropDownList ID="drpNW" runat="server" OnSelectedIndexChanged="drpNW_SelectedIndexChanged"
                                                    AutoPostBack="true">
                                                    <asp:ListItem Value="请选择">请选择</asp:ListItem>
                                                    <asp:ListItem Value="长期">长期</asp:ListItem>
                                                    <asp:ListItem Value="短期">短期</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div style="width: 300px; padding-left: 5px; display: block;" valign="top" id="divNW"
                                                runat="server" visible="false">
                                                <asp:TextBox class="Wdate" onFocus="WdatePicker({readOnly:true,minDate:'2012-01-01',maxDate:'2022-01-01',dateFmt:'yyyy-MM-dd'});"
                                                    ID="txtNWStart" runat="server" Width="100px"></asp:TextBox>
                                                至
                                                <asp:TextBox class="Wdate" onFocus="WdatePicker({readOnly:true,minDate:'2012-01-01',maxDate:'2022-01-01',dateFmt:'yyyy-MM-dd'});"
                                                    ID="txtNWEnd" runat="server" Width="100px"></asp:TextBox>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr height="30px">
                                        <td align="right" valign="middle">
                                            外网：
                                        </td>
                                        <td align="left" valign="middle">
                                            <input id="rdwws" type="radio" value="是" name="waiwang" runat="server" />是&nbsp;&nbsp;
                                            <input id="rdwwf" type="radio" value="否" name="waiwang" checked="true" runat="server" />否
                                        </td>
                                        <td align="right" valign="middle">
                                            外网使用期限：
                                        </td>
                                        <td align="left" valign="middle">
                                            <div style="float: left;">
                                                <asp:DropDownList ID="drpWW" runat="server" OnSelectedIndexChanged="drpWW_SelectedIndexChanged"
                                                    AutoPostBack="true">
                                                    <asp:ListItem Value="请选择">请选择</asp:ListItem>
                                                    <asp:ListItem Value="长期">长期</asp:ListItem>
                                                    <asp:ListItem Value="短期">短期</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div style="width: 300px; margin-left: 5px; display: block;" valign="top" id="divWW"
                                                runat="server" visible="false">
                                                <asp:TextBox class="Wdate" onFocus="WdatePicker({readOnly:true,minDate:'2012-01-01',maxDate:'2022-01-01',dateFmt:'yyyy-MM-dd'});"
                                                    ID="txtWWStart" runat="server" Width="100px"></asp:TextBox>
                                                至
                                                <asp:TextBox class="Wdate" onFocus="WdatePicker({readOnly:true,minDate:'2012-01-01',maxDate:'2022-01-01',dateFmt:'yyyy-MM-dd'});"
                                                    ID="txtWWEnd" runat="server" Width="100px"></asp:TextBox>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr height="30px">
                                        <td align="right" valign="middle">
                                            USB：
                                        </td>
                                        <td align="left" valign="middle" class="style1">
                                            <input id="rdusbs" type="radio" value="是" name="USB" runat="server" />是&nbsp;&nbsp;
                                            <input id="rdusbf" type="radio" value="否" name="USB" checked="true" runat="server" />否
                                        </td>
                                        <td align="right" valign="middle">
                                            USB使用期限：
                                        </td>
                                        <td align="left" valign="middle">
                                            <div style="float: left;">
                                                <asp:DropDownList ID="drpUSB" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpUSB_SelectedIndexChanged">
                                                    <asp:ListItem Value="请选择">请选择</asp:ListItem>
                                                    <asp:ListItem Value="长期">长期</asp:ListItem>
                                                    <asp:ListItem Value="短期">短期</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div style="width: 300px; margin-left: 5px; display: block;" valign="top" id="divUSB"
                                                runat="server" visible="false">
                                                <asp:TextBox class="Wdate" onFocus="WdatePicker({readOnly:true,minDate:'2012-01-01',maxDate:'2022-01-01',dateFmt:'yyyy-MM-dd'});"
                                                    ID="txtUSBStart" runat="server" Width="100px"></asp:TextBox>
                                                至
                                                <asp:TextBox class="Wdate" onFocus="WdatePicker({readOnly:true,minDate:'2012-01-01',maxDate:'2022-01-01',dateFmt:'yyyy-MM-dd'});"
                                                    ID="txtUSBEnd" runat="server" Width="100px"></asp:TextBox>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr height="30px">
                                        <td align="right" valign="middle">
                                            光驱：
                                        </td>
                                        <td align="left" valign="middle">
                                            <input id="rdgqs" type="radio" value="是" name="guangqu" runat="server" />是&nbsp;&nbsp;
                                            <input id="rdgqf" type="radio" value="否" name="guangqu" checked="true" runat="server" />否
                                        </td>
                                        <td align="right" valign="middle">
                                            光驱使用期限：
                                        </td>
                                        <td align="left" valign="middle">
                                            <div style="float: left;">
                                                <asp:DropDownList ID="drpGQ" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpGQ_SelectedIndexChanged">
                                                    <asp:ListItem Value="请选择">请选择</asp:ListItem>
                                                    <asp:ListItem Value="长期">长期</asp:ListItem>
                                                    <asp:ListItem Value="短期">短期</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div style="width: 300px; margin-left: 5px; display: block;" valign="top" id="divGQ"
                                                runat="server" visible="false">
                                                <asp:TextBox class="Wdate" onFocus="WdatePicker({readOnly:true,minDate:'2012-01-01',maxDate:'2022-01-01',dateFmt:'yyyy-MM-dd'});"
                                                    ID="txtGQStart" runat="server" Width="100px"></asp:TextBox>
                                                至
                                                <asp:TextBox class="Wdate" onFocus="WdatePicker({readOnly:true,minDate:'2012-01-01',maxDate:'2022-01-01',dateFmt:'yyyy-MM-dd'});"
                                                    ID="txtGQEnd" runat="server" Width="100px"></asp:TextBox>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr height="30px">
                                        <td align="right" valign="middle">
                                            电脑管理员：
                                        </td>
                                        <td align="left" valign="middle" class="style1">
                                            <input id="rdglys" type="radio" value="是" name="gly" runat="server" />是&nbsp;&nbsp;
                                            <input id="rdglyf" type="radio" value="否" name="gly" checked="true" runat="server" />否
                                        </td>
                                        <td align="right" valign="middle">
                                            电脑管理员使用期限：
                                        </td>
                                        <td align="left" valign="middle">
                                            <div style="float: left;">
                                                <asp:DropDownList ID="drpGLY" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpGLY_SelectedIndexChanged">
                                                    <asp:ListItem Value="请选择">请选择</asp:ListItem>
                                                    <asp:ListItem Value="长期">长期</asp:ListItem>
                                                    <asp:ListItem Value="短期">短期</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div style="width: 300px; margin-left: 5px; display: block;" valign="top" id="divGLY"
                                                runat="server" visible="false">
                                                <asp:TextBox class="Wdate" onFocus="WdatePicker({readOnly:true,minDate:'2012-01-01',maxDate:'2022-01-01',dateFmt:'yyyy-MM-dd'});"
                                                    ID="txtGLYStart" runat="server" Width="100px"></asp:TextBox>
                                                至
                                                <asp:TextBox class="Wdate" onFocus="WdatePicker({readOnly:true,minDate:'2012-01-01',maxDate:'2022-01-01',dateFmt:'yyyy-MM-dd'});"
                                                    ID="txtGLYEnd" runat="server" Width="100px"></asp:TextBox>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" height="70px">
                                备注：
                            </td>
                            <td colspan="3" align="left">
                                <asp:TextBox ID="txtBZ" runat="server" Height="40px" TextMode="MultiLine" Width="500px" onkeyup="CharEditFinished(this)"></asp:TextBox>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="center" style="height: 50px;">
                                <div id="divTJ" runat="server" visible="true" style="display: block;">
                                    <asp:Button ID="btnTJ" runat="server" Text="提交" Width="100px" Height="30px" OnClientClick="return TJ();"
                                        OnClick="btnTJ_Click" />
                                </div>
                                <div id="divXGBTN" runat="server" visible="false" style="display: block;">
                                    <asp:Button ID="btnSave" runat="server" Text="保存" Width="60px" OnClientClick="return TJ();"
                                        OnClick="btnSave_Click" />
                                    <asp:Button ID="btnGoBack" runat="server" Text="返回" Width="60px" OnClick="btnGoBack_Click" />
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
        <tr>
            <td align="center" style="height: 40px;">
            </td>
        </tr>
    </table>

    </form>
    <script src="../js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        function TJ() {
            if ((document.getElementById("txtIP1").value == "" || document.getElementById("txtIP2").value == "" || document.getElementById("txtIP3").value == "" || document.getElementById("txtIP4").value == "") || (document.getElementById("txtMAC1").value == "" || document.getElementById("txtMAC2").value == "" || document.getElementById("txtMAC3").value == "") || document.getElementById("txtPZ").value == "") {
                alert("请将信息填写完整！");
                return false;
            }
            else {
                return true;
            }
        }
    </script>
</body>
</html>
