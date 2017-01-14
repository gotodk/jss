<%@ Page Language="C#" AutoEventWireup="true" CodeFile="JHJX_BZHCX.aspx.cs" Inherits="Web_JHJX_JHJX_BZHCX" %>

<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="../pagerdemo/commonpagernew.ascx" TagName="commonpagernew"
    TagPrefix="uc1" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>保证函查询</title>

    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="../jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/art_confirm.js" type="text/javascript"></script>
    <link href="../../css/standardStyle.css" rel="stylesheet" type="text/css" />
    <script src="../../js/standardJSFile/jquery.tableresizer.js" type="text/javascript"></script>
    <script src="../../js/standardJSFile/jquery.art_confirm.js" type="text/javascript"></script>
    <script src="../../js/standardJSFile/jquery.tablechangecolor.js" type="text/javascript"></script>
    <script type="text/javascript">
    </script>
    <style type="text/css">
                 #content_zw
        {
            width: 930px;
        }      
    </style>

</head><body style="background-color: #f7f7f7;"><form id="form1" runat="server">
    <radTS:RadTabStrip ID="RadTabStrip1" runat="server" Height="25px" Skin="Default2006"
        BackColor="#F7F7F7">
        <Tabs>
            <radTS:Tab ID="Tab1" runat="server" NavigateUrl="JHJX_SHDLR.aspx" ForeColor="red"
                Text="保证函查询">
            </radTS:Tab>
        </Tabs>
    </radTS:RadTabStrip>
    <div id="new_content">
        <div id="new_zicontent">
            <div id="content_zw">
                 <div class="content_bz" >
                    说明文字：<br />
                     1、该模块用于集合交易平台保证函的查询。<br /> 
                </div>
                <div class="content_nr">
                    <table width="100%" cellpadding="0" cellspacing="0" class="content_nr_cx" style="border: solid 1px #ccc">
                        <tr>
                            <td style="width: 90px; overflow: hidden; text-align: right;">
                                合同编号：
                            </td>
                            <td style="text-align: left; width: 120px;">
                               <asp:TextBox ID="txthtbh" runat="server"   class="tj_input" Width="120px" Height="23" > </asp:TextBox></td>
                            <td style="text-align: right; width: 90px;">
                                买方名称：</td>
                            <td width="120px" align="right">
                               <asp:TextBox ID="txtbuyer" runat="server"   class="tj_input" Width="120px"  Height="23"  ></asp:TextBox></td>
                            <td width="90px" align="right">
                                卖方名称：       </td>
                            <td width="120px" align="left">
                                <asp:TextBox ID="txtseller" runat="server"  class="tj_input" Width="120px"  Height="23" ></asp:TextBox>
                            
                            </td>
                            
                            <td width="80px" align="right">
                                是否失效：</td>
                           
                            <td width="100px" align="left">
                                <asp:DropDownList ID="ddlsfyx" runat="server" CssClass="tj_input" Width="70px"  Height="23"   >                                       <asp:ListItem>否</asp:ListItem> 
                                    <asp:ListItem>是</asp:ListItem>                                  
                                    <asp:ListItem Value="">全部</asp:ListItem>                                   
                                </asp:DropDownList>
                            </td>
                            <td width="20px">
                            
                                &nbsp;</td>
                            <td align="left">
                                <asp:Button ID="btnSearch" runat="server" CssClass="tj_bt" Width="80px" 
                                    Text="查询" onclick="btnSearch_Click" />&nbsp;&nbsp;
                            </td>
                        </tr>
                    </table>
                    <table width="100%" cellpadding="0" cellspacing="0" class="content_nr_hj">
                        <tr>
                            <td>
                                保证函信息列表
                            </td>
                        </tr>
                    </table>
                    <table cellspacing="0" cellpadding="0" border="1" bordercolor="#99BBE8"
                        style="border-collapse: collapse;table-layout:fixed;width:100%;" class="tab">
                        <tr>
                            <td >
                                <table id="theObjTable" style="width: 100%;" cellspacing="0" cellpadding="0">
                                    <thead>
                                        <tr>
                                            <th class="TheadTh" style="width: 100px;">
                                                保证函编号
                                            </th>
                                            <th class="TheadTh" style="width: 100px;">
                                                买方名称
                                            </th>
                                            <th class="TheadTh" style="width: 100px;">
                                                卖方名称
                                            </th>
                                            <th class="TheadTh" style="width: 80px;">
                                                开具时间
                                            </th>
                                            <th class="TheadTh" style="width: 80px;">
                                                金额
                                            </th>
                                            <th class="TheadTh" style="width: 100px;">
                                                商品名称
                                            </th>
                                            <th class="TheadTh" style="width: 100px;">
                                                合同编号
                                            </th>                                           
                                            <th class="TheadTh" style="width: 70px;">
                                                是否失效
                                            </th>                                           
                                             <th class="TheadTh" style="width: 80px;">
                                               查看详情
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater ID="rptSPXX" runat="server" OnItemDataBound="rptSPXX_ItemDataBound"  OnItemCommand="rptSPXX_ItemCommand" >
                                            <ItemTemplate>
                                                <tr class="TbodyTr">
                                                    <td>
                                                        <%#Eval("保证函编号")%>
                                                    </td>
                                                    <td title='<%#Eval("买方名称")%>'>
                                                       <asp:Label ID="lbbuyer" runat="server" Text='<%#Eval("买方名称")%>' > </asp:Label>
                                                    </td>
                                                    <td title='<%#Eval("卖方名称")%>'>
                                                       <asp:Label ID="lbseller" runat="server" Text='<%#Eval("卖方名称")%>' > </asp:Label>
                                                    </td>                                                   
                                                   
                                                    <td >
                                                        <%#Eval("开具时间")%>
                                                    </td>                                                   
                                                    <td>
                                                        <%#Eval("金额")%>
                                                    </td>
                                                     <td title='<%#Eval("商品名称")%>'>
                                                       <asp:Label ID="lbspmc" runat="server" Text='<%#Eval("商品名称")%>' > </asp:Label>
                                                    </td>
                                                    <td>
                                                        <%#Eval("合同编号")%>
                                                    </td>
                                                    <td>
                                                        <%#Eval("是否失效")%>
                                                    </td>                                                    
                                                    <td>  <asp:LinkButton ID="ledit" runat="server" CommandName="lck" CommandArgument='<%#Eval("保证函编号")%>' >查看详情</asp:LinkButton>          
                                                                                              
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                          <tr id="tdEmpty" runat="server" visible="false"  style="text-align: center;">
                                            <td colspan="9">
                                                您查询的数据为空！
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <uc1:commonpagernew ID="commonpagernew1" runat="server" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>
    <input runat="server" id="hidID" type="hidden" />
    </form>
</body>
</html>
