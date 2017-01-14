<%@ Page Language="C#" AutoEventWireup="true" CodeFile="JHJX_YLRSPXXFH.aspx.cs" Inherits="Web_JHJX_JHJX_YLRSPXXFH" %>
<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>

<!DOCTYPE html>


<%@ Register Src="../pagerdemo/commonpagernew.ascx" TagName="commonpagernew"
    TagPrefix="uc1" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>预录入商品信息复核</title>

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
            <radTS:Tab ID="Tab1" runat="server" NavigateUrl="JHJX_YLRSPXXFH.aspx" ForeColor="red"
                Text="预录入商品信息复核">
            </radTS:Tab>
        </Tabs>
    </radTS:RadTabStrip>
    <div id="new_content">
        <div id="new_zicontent">
            <div id="content_zw">
                 <div class="content_bz" >
                    说明文字：<br />
                    1、该模块用于预录入商品信息的复核。<br /> 
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
                            <td style="text-align: left; width: 150px;">
                                <asp:DropDownList ID="ddlspzt" runat="server" CssClass="tj_input" Width="120px"  Height="23" OnSelectedIndexChanged="ddlspzt_SelectedIndexChanged"  AutoPostBack="true"  >
                                   
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
                            <td style="width:90px">
                                商品信息列表
                            </td>
                            <td>
                                
                                <asp:LinkButton ID="linkqx" runat="server" OnClick="linkqx_Click">全选</asp:LinkButton>
                                &nbsp;/&nbsp; <asp:LinkButton ID="linkfx" runat="server" OnClick="linkfx_Click">反选</asp:LinkButton>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:LinkButton ID="linkfw" runat="server" OnClick="linkfw_Click" >复核未通过</asp:LinkButton>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:LinkButton ID="linkqd" runat="server" OnClick="linkqd_Click" >确定上线</asp:LinkButton>
                                
                            </td>
                        </tr>
                    </table>
                    <table cellspacing="0" cellpadding="0" border="1" bordercolor="#99BBE8"
                        style="border-collapse: collapse;table-layout:fixed;width:100%" class="tab">
                        <tr>
                            <td>
                                <div class="content_nr_lb" style="width:1110px; ">
                                     <table id="theObjTable" style="width: 1410px;" cellspacing="0" cellpadding="0">
                                    <thead>
                                        <tr>
                                            <th class="TheadTh" style="width: 80px;">
                                                选择
                                            </th>
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
                                                  预录入时间
                                            </th>
                                             <th class="TheadTh" style="width: 80px;">
                                                  商品状态
                                            </th>
                                             <th class="TheadTh" style="width: 100px;">
                                                  状态变更时间
                                            </th>
                                            <%--  <th class="TheadTh" style="width: 80px;">
                                                操作管理
                                            </th>--%>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater ID="rptSPXX" runat="server" OnItemDataBound="rptSPXX_ItemDataBound"  OnItemCommand="rptSPXX_ItemCommand"  OnPreRender="rptSPXX_PreRender">
                                            <ItemTemplate>
                                                <tr class="TbodyTr">
                                                    <td>
                                                        <asp:CheckBox ID="cbxz" runat="server"  /> 
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbbh" runat="server" Text='<%#Eval("Number")%>' > </asp:Label>                                                        
                                                    </td>
                                                    <td title='<%#Eval("SPMC")%>' style="width:200px;text-align:center">                                                       
                                                         <asp:Label ID="lbspmc" runat="server" Text='<%#Eval("SPMC")%>' > </asp:Label>
                                                         <asp:Label ID="lbspmc1" runat="server" Text='<%#Eval("SPMC")%>' Visible="false"> </asp:Label>
                                                    </td>
                                                    <td title='<%#Eval("SPGG")%>' style="width:200px;text-align:center">
                                                       <asp:Label ID="lbgg" runat="server" Text='<%#Eval("SPGG")%>' > </asp:Label>
                                                         <asp:Label ID="lbgg1" runat="server" Text='<%#Eval("SPGG")%>'  Visible="false"> </asp:Label>
                                                    </td>
                                                      <td>                                                      
                                                       <asp:Label ID="lbjjdw" runat="server" Text='<%#Eval("JJDW")%>' > </asp:Label>
                                                        
                                                    </td>
                                                    <td style=" word-wrap:break-word;">                                                       
                                                        <asp:Label ID="lbjjpl" runat="server" Text='<%#Eval("JJPL")%>' > </asp:Label>
                                                    </td>
                                                    <td style=" word-wrap:break-word;">
                                                        <%#Eval("YJFL")%>
                                                    </td>
                                                    <td>
                                                        <%#Eval("EJFL")%>
                                                    </td>
                                                    <td title='<%#Eval("SPMS")%>'>
                                                       
                                                        <asp:Label ID="lbspms" runat="server" Text='<%#Eval("SPMS")%>' > </asp:Label>
                                                         <asp:Label ID="lbspms1" runat="server" Text='<%#Eval("SPMS")%>' Visible="false" > </asp:Label>
                                                    </td>
                                                      <td title='<%#Eval("SCZZYQ")%>'>                                                       
                                                        <asp:Label ID="lbzzmc" runat="server" Text='<%#Eval("SCZZYQ")%>' > </asp:Label>
                                                        <asp:Label ID="lbzzmc1" runat="server" Text='<%#Eval("SCZZYQ")%>' Visible="false" > </asp:Label>
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
                                                    <%--<td>  <asp:LinkButton ID="ledit" runat="server" CommandName="linkbj" CommandArgument='<%#Eval("Number")%>'>修改信息</asp:LinkButton></td>--%>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                          <tr id="tdEmpty" runat="server" visible="false"  style="text-align: center;">
                                            <td colspan="13">
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

