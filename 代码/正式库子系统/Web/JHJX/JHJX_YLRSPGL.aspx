<%@ Page Language="C#" AutoEventWireup="true" CodeFile="JHJX_YLRSPGL.aspx.cs" Inherits="Web_JHJX_JHJX_YLRSPGL" %>
<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<!DOCTYPE html>

<%@ Register Src="../pagerdemo/commonpagernew.ascx" TagName="commonpagernew"
    TagPrefix="uc1" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>预录入商品信息修改</title>

    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="../jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/art_confirm.js" type="text/javascript"></script>
    <link href="../../css/standardStyle.css" rel="stylesheet" type="text/css" />
    <script src="../../js/standardJSFile/jquery.tableresizer.js" type="text/javascript"></script>
    <script src="../../js/standardJSFile/jquery.art_confirm.js" type="text/javascript"></script>
    <script src="../../js/standardJSFile/jquery.tablechangecolor.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#theObjTable").tablechangecolor();
        });
    </script>
    <style type="text/css">
        #content_zw
        {
            width: 1110px;
        }
        </style>
</head>
<body style="background-color: #f7f7f7;">
    <form id="form1" runat="server">
    <radTS:RadTabStrip ID="RadTabStrip1" runat="server" Height="25px" Skin="Default2006"
        BackColor="#F7F7F7">
        <Tabs>
            <radTS:Tab ID="Tab1" runat="server" NavigateUrl="JHJX_YLRSPGL.aspx" ForeColor="red"
                Text="预录入商品信息修改">
            </radTS:Tab>
        </Tabs>
    </radTS:RadTabStrip>
    <div id="new_content">
        <div id="new_zicontent">
            <div id="content_zw">
                 <div class="content_bz" >
                    说明文字：<br />
                    1、该模块用于预录入商品的修改。已经确定上线的商品无法进行编辑。<br /> 
                </div>
                <div class="content_nr">
                    <table width="100%" cellpadding="0" cellspacing="0" class="content_nr_cx" style="border: solid 1px #ccc">
                        <tr>
                            <td style="width: 90px; overflow: hidden; text-align: right;">
                                所属分类：
                            </td>
                            <td style="text-align: left; width: 150px;">
                                <asp:DropDownList ID="ddlssfl" runat="server" CssClass="tj_input" Width="150px"  Height="23"  >
                                    <asp:ListItem Value="0">所属分类</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td width="70px" align="right">
                                &nbsp;商品状态：</td>
                            <td style="text-align: left; width: 100px;">
                                <asp:DropDownList ID="ddlspzt" runat="server" CssClass="tj_input" Width="100px"  Height="23"   >
                                    <asp:ListItem Value="">全部</asp:ListItem>
                                    <asp:ListItem>未复核</asp:ListItem>
                                    <asp:ListItem>复核未通过</asp:ListItem> 
                                    <asp:ListItem>确定上线</asp:ListItem>                                   
                                </asp:DropDownList>
                            </td>
                            <td width="70px" align="right">
                                商品名称：</td>
                            <td width="130px" align="left">
                               <asp:TextBox ID="txtspmc" runat="server"   class="tj_input" Width="120px"  Height="23" ></asp:TextBox>
                            </td>
                            
                            <td width="80px" align="right">
                                商品规格：</td>
                           
                            <td width="80px" align="left">
                                <asp:TextBox ID="txtspgg" runat="server"  class="tj_input" Width="120px"  Height="23" ></asp:TextBox>
                            
                            </td>
                            <td width="50px">
                            
                                &nbsp;</td>
                            <td align="left">
                                <asp:Button ID="btnSearch" runat="server" CssClass="tj_bt" Width="80px" 
                                    Text="查询" onclick="btnSearch_Click" />&nbsp;&nbsp;
                                <asp:Button ID="btnDC" runat="server" CssClass="tj_bt" UseSubmitBehavior="False"
                                    Text="导出" Width="80px" OnClick="btnDC_Click" />
                            </td>
                        </tr>
                    </table>
                    <table width="100%" cellpadding="0" cellspacing="0" class="content_nr_hj">
                        <tr>
                            <td>
                                商品信息列表
                            </td>
                        </tr>
                    </table>
                    <table cellspacing="0" cellpadding="0" border="1" bordercolor="#99BBE8"
                        style="border-collapse: collapse;table-layout:fixed;width:100%;" class="tab">
                        <tr>
                            <td>
                                <div class="content_nr_lb" style="width:1110px; ">
                                     <table id="theObjTable" style="width: 1410px;" cellspacing="0" cellpadding="0">
                                    <thead>
                                        <tr>
                                            <th class="TheadTh" style="width: 80px;">
                                                编号
                                            </th>
                                            <th class="TheadTh" style="width: 200px;">
                                                商品名称
                                            </th>
                                            <th class="TheadTh" style="width: 200px;">
                                                商品规格
                                            </th>
                                            <th class="TheadTh" style="width: 70px;">
                                                   计价单位
                                            </th>
                                            <th class="TheadTh" style="width: 70px;">
                                                经济批量
                                            </th>
                                            <th class="TheadTh" style="width: 80px;">
                                                一级分类
                                            </th>
                                            <th class="TheadTh" style="width: 80px;">
                                                二级分类
                                            </th>                                           
                                            <th class="TheadTh" style="width: 150px;">
                                                商品描述
                                            </th>
                                            <th class="TheadTh" style="width: 120px;">
                                                特殊资质</th>
                                             <th class="TheadTh" style="width: 100px;">
                                                  录入时间
                                            </th>
                                             <th class="TheadTh" style="width: 80px;">
                                                  商品状态
                                            </th>
                                             <th class="TheadTh" style="width: 100px;">
                                                  状态变更时间
                                            </th>
                                            <th class="TheadTh" style="width: 80px;">
                                                操作管理
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater ID="rptSPXX" runat="server" OnItemDataBound="rptSPXX_ItemDataBound"  OnItemCommand="rptSPXX_ItemCommand" >
                                            <ItemTemplate>
                                                <tr class="TbodyTr">
                                                    <td>
                                                        <%#Eval("Number")%>
                                                    </td>
                                                    <td title='<%#Eval("SPMC")%>' style="width:200px;text-align:center;">                                                       
                                                         <asp:Label ID="lbspmc" runat="server" Text='<%#Eval("SPMC")%>' > </asp:Label>
                                                    </td>
                                                    <td title='<%#Eval("SPGG")%>' style="width:200px;text-align:center;">
                                                       <asp:Label ID="lbgg" runat="server" Text='<%#Eval("SPGG")%>' > </asp:Label>
                                                    </td>
                                                      <td>
                                                        <%#Eval("JJDW")%>
                                                        
                                                    </td>
                                                    <td style=" word-wrap:break-word;">
                                                         <%#Eval("JJPL")%>
                                                    </td>
                                                    <td style=" word-wrap:break-word;">
                                                        <%#Eval("YJFL")%>
                                                    </td>
                                                    <td>
                                                        <%#Eval("EJFL")%>
                                                    </td>
                                                    <td title='<%#Eval("SPMS")%>'>
                                                       
                                                        <asp:Label ID="lbspms" runat="server" Text='<%#Eval("SPMS")%>' > </asp:Label>
                                                    </td>
                                                      <td title='<%#Eval("SCZZYQ")%>'>                                                       
                                                        <asp:Label ID="lbzzmc" runat="server" Text='<%#Eval("SCZZYQ")%>' > </asp:Label>
                                                    </td>
                                                    <td>
                                                        <%#Eval("CreateTime")%>
                                                    </td>
                                                     <td>
                                                       <asp:Label ID="lbspzt" runat="server" Text='<%#Eval("SPZT")%>' > </asp:Label>
                                                      
                                                    </td>
                                                    <td>
                                                        <%#Eval("ZTBGSJ")%>
                                                        
                                                    </td>
                                                    <td>  <asp:LinkButton ID="ledit" runat="server" CommandName="linkbj" CommandArgument='<%#Eval("Number")%>'>修改信息</asp:LinkButton></td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                          <tr id="tdEmpty" runat="server" visible="false"  style="text-align: center;">
                                            <td colspan="11">
                                                您查询的数据为空！
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                </div>
                               
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

