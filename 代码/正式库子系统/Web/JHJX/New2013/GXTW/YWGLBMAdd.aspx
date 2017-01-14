<%@ Page Language="C#" AutoEventWireup="true" CodeFile="YWGLBMAdd.aspx.cs" Inherits="Web_JHJX_New2013_GXTW_YWGLBMAdd" %>

<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<%@ Register Src="../../../pagerdemo/commonpager.ascx" TagName="commonpager" TagPrefix="uc1" %>
<%@ Register Src="../../../pagerdemo/commonpagernew.ascx" TagName="commonpagernew"
    TagPrefix="uc1" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link href="../../../../css/standardStyle.css" rel="stylesheet" />
    <script src="../../../../js/jquery-1.7.2.min.js"></script>    
    <script src="../../../../js/standardJSFile/jquery.art_confirm.js"></script>
    <script src="../../../../js/standardJSFile/art_confirm.js"></script>
    <script src="../../../../js/standardJSFile/fcf.js"></script>
      <script type="text/javascript">
         
          function Promot(this_an) {
              return art_confirm_fcf(this_an, "您确定要提交吗！", 'clickyc()');
          }
          function Promot1(this_an) {
              return art_confirm_fcf(this_an, "您确定要保存修改吗！", 'clickyc()');
          }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <radTS:RadTabStrip ID="TabStrip1" runat="server" CausesValidation="False" ReorderTabRows="True"
            Skin="Default2006" Width="99%">
            <Tabs>
                <radTS:Tab ID="Tab2" runat="server" NavigateUrl="YWGLBMAdd.aspx" Text="业务管理部门添加"
                     Font-Size="12px" ForeColor="Red">
                </radTS:Tab>
                
            <radTS:Tab ID="Tab1" runat="server" NavigateUrl="YWGLBMCK.aspx" Text="业务管理部门查看"
                 Font-Size="12px" ></radTS:Tab>
            </Tabs>
        </radTS:RadTabStrip>

    <div id="new_content">
        <div id="new_zicontent">
            <div id="content_zw" style=" width:800px;">
                <div class="content_bz" >
                    <%--说明文字--%>
                </div>
                <div class="content_nr" id="divLB" runat="server">
                    <table width="100%" class="Message">
                        <thead>
                            <tr>
                                <th class="TitleTh">
                                    服务商信息
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                           <tr>
                                <td style=" width:50%;">
                                    <table width="100%">
                                        <tr>
                                            <td style=" width:10px; text-align:left;">
                                                <span class="span">*</span>
                                            </td>
                                            <td style=" width:100px;overflow:hidden; text-align:right;">
                                                管理部门分类：
                                            </td>
                                            <td  style="text-align:left;">
                                                <asp:DropDownList ID="drpGLBMFL" runat="server" Width="162px" CssClass="tj_input">
                                                    <asp:ListItem>高校团委</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style=" width:50%;">
                            
                                </td>                            
                           </tr>
                           <tr>
                                <td style=" width:50%;">
                                    <table width="100%">
                                        <tr>
                                            <td style=" width:10px; text-align:left;">
                                                <span class="span">*</span>
                                            </td>
                                            <td style=" width:100px;overflow:hidden; text-align:right;">
                                                管理部门名称：
                                            </td>
                                            <td  style="text-align:left;">
                                                <asp:TextBox ID="txtGLBMMC" runat="server" CssClass="tj_input" Width="160px" Enabled="True"
                                    TabIndex="1" MaxLength="50"></asp:TextBox>
                                            
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style=" width:50%;">
                                    <table width="100%">
                                        <tr>
                                            <td style=" width:10px; text-align:left;">
                                                <span class="span">*</span>
                                            </td>
                                            <td style=" width:100px;overflow:hidden; text-align:right;">
                                                管理部门帐号：
                                            </td>
                                            <td  style="text-align:left;">
                                            <asp:TextBox ID="txtGLBMZH" runat="server" CssClass="tj_input" Width="160px" Enabled="True"
                                    TabIndex="1" MaxLength="50"></asp:TextBox>
                                            
                                            </td>
                                        </tr>
                                    </table>
                                </td>                            
                           </tr>
                           <tr>
                                <td style=" width:50%;">
                                    <table width="100%">
                                        <tr>
                                            <td style=" width:10px; text-align:left;">
                                                <span class="span">*</span>
                                            </td>
                                            <td style=" width:100px;overflow:hidden; text-align:right;">
                                                管理部门密码：
                                            </td>
                                            <td  style="text-align:left;">
                                                <asp:TextBox ID="txtGLBMMM" runat="server" CssClass="tj_input" Width="160px" Enabled="True"
                                    TabIndex="1" MaxLength="50"></asp:TextBox>
                                            
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style=" width:50%;">
                                    <table width="100%">
                                        <tr>
                                            <td style=" width:10px; text-align:left;">
                                                <span class="span">*</span>
                                            </td>
                                            <td style=" width:100px;overflow:hidden; text-align:right;">
                                                是否有效：
                                            </td>
                                            <td  style="text-align:left;">
                                                <asp:DropDownList ID="drpSFYX" runat="server" Width="162px" CssClass="tj_input">
                                                    <asp:ListItem>是</asp:ListItem>
                                                    <asp:ListItem>否</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>                            
                           </tr>
                           <tr>
                                <td colspan="2" style=" height:30px;">
                                    <table width="100%">
                                        <tr>
                                            <td style=" width:10px; text-align:left;">
                                            </td>
                                            <td style=" width:100px;overflow:hidden; text-align:right;">
                                            </td>
                                            <td  style="text-align:left;">
                                                <div id="djycqymain" style="margin-top: 10px;">
                                                        <div id="djycqy_show">
                                                <asp:Button ID="Button7" UseSubmitBehavior="False" runat="server" CssClass="tj_bt_da"  Text="提交" OnClientClick="return Promot(this);" OnClick="Button7_Click" />
                                                            <asp:Button ID="btnUpdate" UseSubmitBehavior="False" runat="server" CssClass="tj_bt_da"  Text="保存" OnClientClick="return Promot1(this);" OnClick="btnUpdate_Click" />
                                                            &nbsp;&nbsp;&nbsp;&nbsp;
                                                            <asp:Button ID="btnGoBack" UseSubmitBehavior="False" runat="server" CssClass="tj_bt_da"  Text="返回列表"  OnClick="btnGoBack_Click" />
                                                            </div>
                                                    </div>
                                            </td>
                                        </tr>
                                    </table>
                                    
                                </td>                           
                           </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
