<%@ Page Language="C#" AutoEventWireup="true" CodeFile="JHJX_FGSJYZHSH_MMJ_DetailsInfo.aspx.cs" Inherits="Web_JHJX_New2013_JHJX_FGSJYZHSH_MMJ_DetailsInfo" %>
<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<%@ Register src="../../UCCityList.ascx" tagname="UCCityList" tagprefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>经纪人基本资料详情</title>
      <script src="../../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="../../jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../../../js/standardJSFile/art_confirm.js" type="text/javascript"></script>
    <link href="../../../css/standardStyle.css" rel="stylesheet" type="text/css" />
    <script src="../../../js/standardJSFile/jquery.tableresizer.js" type="text/javascript"></script>
    <script src="../../../js/standardJSFile/jquery.art_confirm.js" type="text/javascript"></script>
    <script src="../../../js/standardJSFile/jquery.tablechangecolor.js" type="text/javascript"></script>
    <script src="../../../js/standardJSFile/fcf.js" type="text/javascript"></script>
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
            <radTS:Tab ID="Tab1" runat="server" Text="买方卖方基本资料详情">
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
                    <tr  id="trJYZHLX" runat="server">
                            <td align="right" width="200px;">
                                交易账户类型：
                            </td>
                            <td colspan="3">
                                    <asp:RadioButton ID="radJJR" GroupName="JJR" Enabled="false" runat="server" Text="经纪人交易账户" />&nbsp;&nbsp;<asp:RadioButton
                                    ID="radMMJ" runat="server" GroupName="JJR" Enabled="false" Text="交易方交易账户" />&nbsp;&nbsp;</td>
                        </tr>
                        <tr id="trZCLB" runat="server">
                            <td align="right" width="200px;">
                                注册类别：
                            </td>
                            <td colspan="3">
                                 <asp:RadioButton ID="radDW" GroupName="LB" Enabled="false" runat="server" Text="单位" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:RadioButton
                                    ID="radZRR" runat="server" GroupName="LB" Enabled="false" Text="自然人" />&nbsp;&nbsp;
                            </td>
                        </tr>
                        <tr id="trJYFMC" runat="server">
                            <td align="right">
                                交易方名称：
                            </td>
                            <td colspan="3">
                              <asp:TextBox ID="txtJYFMC" runat="server" CssClass="tj_input" Width="200px" Enabled="false"
                                    TabIndex="1"></asp:TextBox>
                            </td>
                        </tr>
                        <tr id="trYYZZZCH" runat="server">
                            <td align="right">
                                营业执照注册号：
                            </td>
                            <td colspan="3">
                              <asp:TextBox ID="txtYYZZZCH" runat="server" CssClass="tj_input" Width="200px" Enabled="false"
                                    TabIndex="1"></asp:TextBox> &nbsp;&nbsp;<a id="linkYYZZ" class="link" runat="server" href="" target="_blank">查看</a>
                            </td>
                        </tr>
                        <tr id="trSFZH" runat="server">
                            <td align="right">
                                身份证号：
                            </td>
                            <td colspan="3">
                              <asp:TextBox ID="txtSFZH" runat="server" CssClass="tj_input" Width="200px" Enabled="false"
                                    TabIndex="1"></asp:TextBox> &nbsp;&nbsp;<a id="linkSFZH" class="link" runat="server" href="" target="_blank">正面查看</a> &nbsp;&nbsp;<a id="linkSFZH_FM" class="link" runat="server" href="" target="_blank">反面查看</a>
                            </td>
                        </tr>
                        <tr id="trZZJGDMZ" runat="server">
                            <td align="right">
                                组织机构代码证代码：
                            </td>
                            <td colspan="3">
                              <asp:TextBox ID="txtZZJGDMZ" runat="server" CssClass="tj_input" Width="200px" Enabled="false"
                                    TabIndex="1"></asp:TextBox> &nbsp;&nbsp;<a id="linkZZJGDMZ" class="link" runat="server" href="" target="_blank">查看</a>
                            </td>
                        </tr>
                        <tr id="trSWDJZSH" runat="server">
                            <td align="right">
                                税务登记证税号：
                            </td>
                            <td colspan="3">
                               <asp:TextBox ID="txtSWDJZ" runat="server" CssClass="tj_input" Width="200px" Enabled="false"
                                    TabIndex="1"></asp:TextBox> &nbsp;&nbsp;<a id="linkSWDJZ" class="link" runat="server" href="" target="_blank">查看</a>
                            </td>
                        </tr>
                        <tr id="trYBNSRZGZMSMJ" runat="server" visible="false" >
                            <td align="right">
                                一般纳税人资格证明扫描件：
                            </td>
                            <td colspan="3">
                              &nbsp;&nbsp;<a id="linkYBNSRZGZMSMJ" class="link" runat="server" href="" target="_blank">查看</a>
                            </td>
                        </tr>
                        <tr id="trKHXKZH" runat="server">
                            <td align="right">
                                开户许可证号：
                            </td>
                            <td colspan="3">
                              <asp:TextBox ID="txtKHXKZH" runat="server" CssClass="tj_input" Width="200px" Enabled="false"
                                    TabIndex="1"></asp:TextBox> &nbsp;&nbsp;<a id="linkKHXKZH" class="link" runat="server" href="" target="_blank">查看</a>
                                    
                                  
                                 
                            </td>
                        </tr>
                           <tr id="trYLYJK" runat="server">
                            <td align="right">
                                预留印鉴卡：
                            </td>
                            <td colspan="3">
                              &nbsp;&nbsp;<a id="linkYLYJK" class="link" runat="server" href="" target="_blank">查看</a>
                            </td>
                        </tr>
                        <tr id="trFDDBRXM" runat="server">
                            <td align="right">
                                法定代表人姓名：
                            </td>
                            <td colspan="3">
                              <asp:TextBox ID="txtFDDBRXM" runat="server" CssClass="tj_input" Width="200px" Enabled="false"
                                    TabIndex="1"></asp:TextBox>
                            </td>
                        </tr>
                        <tr id="trFDDBRSHZHJSMJ" runat="server">
                            <td align="right">
                                法定代表人身份证号及扫描件：
                            </td>
                            <td colspan="3">
                            <asp:TextBox ID="txtFDDBRSHZHJSMJ" runat="server" CssClass="tj_input" Width="200px" Enabled="false"
                                    TabIndex="1"></asp:TextBox> &nbsp;&nbsp;<a id="linkFDDBRSHZHJSM" class="link" runat="server" href="" target="_blank">正面查看</a>&nbsp;&nbsp;<a id="linkFDDBRSHZHJSM_FM" class="link" runat="server" href="" target="_blank">反面查看</a>
                            </td>
                        </tr>
                        <tr id="trFDDBRSQS" runat="server">
                            <td align="right">
                                法定代表人授权书：
                            </td>
                            <td colspan="3">
                          &nbsp;&nbsp;<a id="linkFDDBRSQS" class="link" runat="server" href="" target="_blank">查看</a>
                            </td>
                        </tr>
                        <tr id="trJYFLXDH" runat="server">
                            <td align="right">
                                交易方联系电话：
                            </td>
                            <td colspan="3">
                              <asp:TextBox ID="txtJYFLXDH" runat="server" CssClass="tj_input" Width="200px" Enabled="false"
                                    TabIndex="1"></asp:TextBox>
                            </td>
                        </tr>
                        <tr id="trSSQY" runat="server">
                            <td align="right" class="style1">
                                所属区域：
                            </td>
                            <td colspan="3" class="style1">
                           <uc1:UCCityList ID="UCCityList1" runat="server" />
                            </td>
                        </tr>
                        <tr id="trXXDZ" runat="server">
                            <td align="right">
                                详细地址：
                            </td>
                            <td colspan="3">
                              <asp:TextBox ID="txtXXDZ" runat="server" CssClass="tj_input" Width="200px" Enabled="false"
                                    TabIndex="1"></asp:TextBox> 
                            </td>
                        </tr>
                        <tr id="trLXRXM" runat="server" >
                            <td align="right">
                                联系人姓名：
                            </td>
                            <td colspan="3">
                              <asp:TextBox ID="txtLXRXM" runat="server" CssClass="tj_input" Width="200px" Enabled="false"
                                    TabIndex="1"></asp:TextBox>
                            </td>
                        </tr>
                        <tr id="trLXRSJH" runat="server">
                            <td align="right">
                                联系人手机号：
                            </td>
                            <td colspan="3">
                              <asp:TextBox ID="txtLXRSJH" runat="server" CssClass="tj_input" Width="200px" Enabled="false"
                                    TabIndex="1"></asp:TextBox> 
                            </td>
                        </tr>
                         <tr id="trKHYH" runat="server">
                            <td align="right">
                                开户银行：
                            </td>
                            <td colspan="3">
                              <asp:TextBox ID="txtKHYH" runat="server" CssClass="tj_input" Width="200px" Enabled="false"
                                    TabIndex="1"></asp:TextBox>
                            </td>
                        </tr>
                           <tr id="trYHZH" runat="server">
                            <td align="right">
                                银行账号：
                            </td>
                            <td colspan="3">
                              <asp:TextBox ID="txtYHZH" runat="server" CssClass="tj_input" Width="200px" Enabled="false"
                                    TabIndex="1"></asp:TextBox>
                            </td>
                        </tr>
                           <tr id="trPTGLJG" runat="server">
                            <td align="right">
                                业务管理部门：
                            </td>
                            <td colspan="3">
                              <asp:DropDownList ID="ddlPTGLJG" runat="server" Width="160px" CssClass="tj_input"
                                    Height="22px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr id="trGLJJRZGZSBH" runat="server">
                            <td align="right">
                                关联经纪人资格证书编号：
                            </td>
                            <td colspan="3">
                            <asp:TextBox ID="txtGLJJRZGZSBH" runat="server" CssClass="tj_input" Width="200px" Enabled="false"
                                    TabIndex="1"></asp:TextBox> &nbsp;&nbsp;
                            </td>
                        </tr>
                          <tr id="trGLJJRMC" runat="server">
                            <td align="right">
                                关联经纪人名称：
                            </td>
                            <td colspan="3">
                            <asp:TextBox ID="txtGLJJRMC" runat="server" CssClass="tj_input" Width="200px" Enabled="false"
                                    TabIndex="1"></asp:TextBox> 
                            </td>
                        </tr>
                         <tr id="trGLJJRLXDH" runat="server">
                            <td align="right">
                                关联经纪人联系电话：
                            </td>
                            <td colspan="3">
                            <asp:TextBox ID="txtGLJJRLXDH" runat="server" CssClass="tj_input" Width="200px" Enabled="false"
                                    TabIndex="1"></asp:TextBox> 
                            </td>
                        </tr>
                        <tr id="trZLTJSJ" runat="server">
                            <td align="right">
                                资料提交时间：
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtZLTJSJ" runat="server" CssClass="tj_input" Width="200px" Enabled="false"
                                    TabIndex="1"></asp:TextBox>
                            </td>
                        </tr>
                          <tr style=" height:10px;">
                            <td colspan="4">
                            <div style=" height: 1px; background-color: #A0A0A0;  margin-top: 10px; margin-bottom: 10px;">
                                   </div>
                            </td>
                        </tr>
                        <tr id="trCS_JL" runat="server" >
                            <td align="right">
                              初审记录：
                            </td>
                            <td colspan="3">
                              <asp:TextBox ID="txtCS_JL" runat="server"  CssClass="tj_input" Width="200px" Enabled="false"
                                    TabIndex="1"></asp:TextBox>
                            </td>
                        </tr>
                        <tr id="trCS_SJ" runat="server" >
                            <td align="right">
                                初审时间：
                            </td>
                            <td colspan="3">
                              <asp:TextBox ID="txtCS_SJ" runat="server" CssClass="tj_input" Width="200px" Enabled="false"
                                    TabIndex="1"></asp:TextBox>
                            </td>
                        </tr>
                        <tr id="trCS_YJ" runat="server" style=" height:110px;">
                            <td id="trCS_SHYJ" align="right">
                               初审意见：
                            </td>
                            <td colspan="3">
                             <asp:TextBox ID="txtCS_SHYJ" runat="server" Enabled="false" class="tj_input" Width="400px" Height="100px"
                                    TextMode="MultiLine" MaxLength="200" Text="" ></asp:TextBox>
                            </td>
                        </tr>

                          <tr id="trFS_JL" runat="server" >
                            <td align="right">
                             复审记录：
                            </td>
                            <td colspan="3">
                              <asp:TextBox ID="txtFS_JL" runat="server"  CssClass="tj_input" Width="200px" Enabled="false"
                                    TabIndex="1"></asp:TextBox>
                            </td>
                        </tr>
                        <tr id="trFS_SJ" runat="server" >
                            <td align="right">
                                复审时间：
                            </td>
                            <td colspan="3">
                              <asp:TextBox ID="txtFS_SJ" runat="server" CssClass="tj_input" Width="200px" Enabled="false"
                                    TabIndex="1"></asp:TextBox>
                            </td>
                        </tr>
                        <tr id="trFS_YJ" runat="server" style=" height:110px;">
                            <td id="Td1" align="right">
                               复审意见：
                            </td>
                            <td colspan="3">
                             <asp:TextBox ID="txtFS_YJ" runat="server" class="tj_input" Enabled="false" Width="400px" Height="100px"
                                    TextMode="MultiLine" MaxLength="200" Text="" ></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                          <td align="right">
                              &nbsp;
                            </td>
                            <td colspan="3" align="left" style="height: 80px;">
                            <div id="djycqymain" style=" margin-top:10px;">
                              <div id="djycqy_show">
                                  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button
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
    
    </form>
</body>
</html>
