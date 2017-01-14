<%@ Page Language="C#" AutoEventWireup="true" CodeFile="JHJX_DLRSHXQ.aspx.cs" Inherits="Web_JHJX_JHJX_DLRSHXQ" %>
<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
      <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="../jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../../js/standardJSFile/art_confirm.js" type="text/javascript"></script>
    <link href="../../css/standardStyle.css" rel="stylesheet" type="text/css" />
    <script src="../../js/standardJSFile/jquery.tableresizer.js" type="text/javascript"></script>
    <script src="../../js/standardJSFile/jquery.art_confirm.js" type="text/javascript"></script>
    <script src="../../js/standardJSFile/jquery.tablechangecolor.js" type="text/javascript"></script>
    <script src="../../js/standardJSFile/fcf.js" type="text/javascript"></script>
    <style type="text/css">
        .content_tab tr
        {
            height: 30px;
            line-height: 30px;
        }
        .style1
        {
            height: 30px;
        }
       .link
       {
           cursor:pointer;
           }
    </style>
    <script type="text/javascript">
        function Promot(this_an) {
            return art_confirm_fcf(this_an, "您确定要执行此操作吗！此操作执行的结果将不可恢复！", 'clickyc()');
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <radTS:RadTabStrip ID="RadTabStrip1" runat="server" Height="25px" Skin="Default2006"
        BackColor="#f7f7f7">
        <Tabs>
            <radTS:Tab ID="Tab1" runat="server" Text="业务平台审核经纪人功能">
            </radTS:Tab>
        </Tabs>
    </radTS:RadTabStrip>
    <div id="new_content">
        <div id="new_zicontent">
            <div id="content_zw">
                <div class="content_lx">
                </div>
                <div class="content_nr">
                    <table width="600px" class="content_tab">
                        <tr>
                            <td align="right" width="100px;">
                                用户名：
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtYHM" runat="server" CssClass="tj_input" Width="200px" Enabled="false"
                                    TabIndex="1"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                注册邮箱：
                            </td>
                            <td colspan="3">
                              <asp:TextBox ID="txtZCYX" runat="server" CssClass="tj_input" Width="200px" Enabled="false"
                                    TabIndex="1"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                联系人姓名：
                            </td>
                            <td colspan="3">
                              <asp:TextBox ID="txtLXRXM" runat="server" CssClass="tj_input" Width="200px" Enabled="false"
                                    TabIndex="1"></asp:TextBox>
                            </td>
                        </tr>
                        <tr id="trSFZH" runat="server">
                            <td align="right">
                                身份证号：
                            </td>
                            <td colspan="3">
                              <asp:TextBox ID="txtSFZH" runat="server" CssClass="tj_input" Width="200px" Enabled="false"
                                    TabIndex="1"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                手机号：
                            </td>
                            <td colspan="3">
                              <asp:TextBox ID="txtSJH" runat="server" CssClass="tj_input" Width="200px" Enabled="false"
                                    TabIndex="1"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                所属区域：
                            </td>
                            <td colspan="3">
                              <asp:TextBox ID="txtSZSF" runat="server" CssClass="tj_input" Width="100px" Enabled="false"
                                    TabIndex="1"></asp:TextBox>
                           &nbsp;&nbsp;  
                             <asp:TextBox ID="txtSZDS" runat="server" CssClass="tj_input" Width="100px" Enabled="false"
                                    TabIndex="1"></asp:TextBox>
                            &nbsp;&nbsp;  
                               <asp:TextBox ID="txtSSQX" runat="server" CssClass="tj_input" Width="100px" Enabled="false"
                                    TabIndex="1"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                详细地址：
                            </td>
                            <td colspan="3">
                              <asp:TextBox ID="txtYXDZ" runat="server" CssClass="tj_input" Width="200px" Enabled="false"
                                    TabIndex="1"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                邮政编码：
                            </td>
                            <td colspan="3">
                              <asp:TextBox ID="txtYZBM" runat="server" CssClass="tj_input" Width="200px" Enabled="false"
                                    TabIndex="1"></asp:TextBox>
                            </td>
                        </tr>
                        <tr id="trSFZSMJ" runat="server">
                            <td align="right">
                                身份证扫描件：
                            </td>
                            <td colspan="3">
                              <asp:TextBox ID="txtSFZSMJ" runat="server" CssClass="tj_input" Width="200px" Enabled="false"
                                    TabIndex="1"></asp:TextBox> &nbsp;&nbsp;<a id="linkSFZSMJ" class="link" runat="server" href="" target="_blank">查看</a>
                                    
                                  
                                 
                            </td>
                        </tr>
                        <%--<tr style=" visibility:hidden;">
                            <td align="right">
                                签字的承诺书：
                            </td>
                            <td colspan="3">
                              <asp:TextBox ID="txtQZCNS" runat="server" CssClass="tj_input" Width="200px" Enabled="false"
                                    TabIndex="1"></asp:TextBox> &nbsp;&nbsp;<a id="linkQZCNS" class="link" runat="server" href="" target="_blank">查看</a>
                            </td>
                        </tr>--%>
                        <tr id="trGSMC" runat="server">
                            <td align="right">
                                公司名称：
                            </td>
                            <td colspan="3">
                              <asp:TextBox ID="txtGSMC" runat="server" CssClass="tj_input" Width="200px" Enabled="false"
                                    TabIndex="1"></asp:TextBox>
                            </td>
                        </tr>
                        <tr id="trGSDH" runat="server">
                            <td align="right">
                                公司电话：
                            </td>
                            <td colspan="3">
                              <asp:TextBox ID="txtGSDH" runat="server" CssClass="tj_input" Width="200px" Enabled="false"
                                    TabIndex="1"></asp:TextBox>
                            </td>
                        </tr>
                        <tr id="trGSDZ" runat="server">
                            <td align="right">
                                公司地址：
                            </td>
                            <td colspan="3">
                              <asp:TextBox ID="txtGSDZ" runat="server" CssClass="tj_input" Width="200px" Enabled="false"
                                    TabIndex="1"></asp:TextBox>
                            </td>
                        </tr>
                        <tr id="trYYZZ" runat="server">
                            <td align="right" class="style1">
                                营业执照：
                            </td>
                            <td colspan="3" class="style1">
                              <asp:TextBox ID="txtYYZZ" runat="server" CssClass="tj_input" Width="200px" Enabled="false"
                                    TabIndex="1"></asp:TextBox> &nbsp;&nbsp;<a id="linkYYZZ" class="link" runat="server" href="" target="_blank">查看</a>
                            </td>
                        </tr>
                        <tr id="trZZJGDMZ" runat="server">
                            <td align="right">
                                组织机构代码证：
                            </td>
                            <td colspan="3">
                              <asp:TextBox ID="txtZZJGDMZ" runat="server" CssClass="tj_input" Width="200px" Enabled="false"
                                    TabIndex="1"></asp:TextBox> &nbsp;&nbsp;<a id="linkZZJGDMZ" class="link" runat="server" href="" target="_blank">查看</a>
                            </td>
                        </tr>
                        <tr id="trSWDJZ" runat="server" >
                            <td align="right">
                                税务登记证：
                            </td>
                            <td colspan="3">
                              <asp:TextBox ID="txtSWDJZ" runat="server" CssClass="tj_input" Width="200px" Enabled="false"
                                    TabIndex="1"></asp:TextBox> &nbsp;&nbsp;<a id="linkSWDJZ" class="link" runat="server" href="" target="_blank">查看</a>
                            </td>
                        </tr>
                        <tr id="trKHXKZ" runat="server">
                            <td align="right">
                                开户许可证：
                            </td>
                            <td colspan="3">
                              <asp:TextBox ID="txtKHXKZ" runat="server" CssClass="tj_input" Width="200px" Enabled="false"
                                    TabIndex="1"></asp:TextBox> &nbsp;&nbsp;<a id="linkKHXKZ" class="link" runat="server" href="" target="_blank">查看</a>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                审核意见：
                            </td>
                            <td colspan="3">
                             <asp:TextBox ID="txtSHYJ" runat="server" class="tj_input" Width="400px" Height="100px"
                                    TextMode="MultiLine" MaxLength="250" Text="" onkeyup="OnkeyUp(this)"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                          <td align="right">
                              &nbsp;
                            </td>
                            <td colspan="3" align="left" style="height: 80px;">
                            <div id="djycqymain" style=" margin-top:10px;">
                              <div id="djycqy_show">
                                <asp:Button ID="btnPass" runat="server" CssClass="tj_bt_da" UseSubmitBehavior="False"
                                    Text="通过" Width="50px" OnClientClick="return Promot(this);" onclick="btnPass_Click" />&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="btnReject"
                                        UseSubmitBehavior="False" runat="server" CssClass="tj_bt_da" 
                                        Text="驳回" Width="50px" OnClientClick="return Promot(this);"   onclick="btnReject_Click" />&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button
                                                ID="btnBack" UseSubmitBehavior="False" runat="server" 
                                    CssClass="tj_bt_da" 
                                                Text="返回列表" onclick="btnBack_Click" />
                                                </div>
                                                </div>
                            </td>
                        </tr>
                    </table>
                  
                </div>
            </div>
        </div>
        
        
    </div>
    
    
    <input runat="server" id="hidID" type="hidden" />
    <script language="javascript" type="text/javascript">
        function OnkeyUp(obj) {
            var len = obj.value.length;
            if (len>250)
            {
                obj.value = obj.value.substring(0, 250);
            }
        }
    </script>

    </form>
</body>
</html>
