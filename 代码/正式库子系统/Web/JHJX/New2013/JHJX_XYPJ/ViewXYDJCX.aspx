<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewXYDJCX.aspx.cs" Inherits="Web_JHJX_New2013_JHJX_XYPJ_ViewXYDJCXaspx" %>

<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<%@ Register Src="../../../pagerdemo/commonpager.ascx" TagName="commonpager" TagPrefix="uc1" %>
<%@ Register Src="../../../pagerdemo/commonpagernew.ascx" TagName="commonpagernew"
    TagPrefix="uc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>交易方评分变化明细</title>
    <script src="../../../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="../../../jquery-1.7.2.min.js" type="text/javascript"></script>
     <link href="../../../../css/standardStyle.css" rel="stylesheet" type="text/css" />
    <script src="../../../../js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/jquery.tablechangecolor.js" type="text/javascript"></script> 
    <script src="../../../../js/standardJSFile/art_confirm.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/jquery.art_confirm.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#theObjTable").tablechangecolor();
        });
    </script>
    <style type="text/css">
        #content_zw
        {
            width: 860px;
        }
        </style>
</head>
<body style="background-color: #f7f7f7;">
    <form id="form1" runat="server">
    <radTS:RadTabStrip ID="RadTabStrip1" runat="server" Height="25px" Skin="Default2006"
        BackColor="#F7F7F7">
        <Tabs>
            <radTS:Tab ID="Tab1" runat="server" NavigateUrl="" ForeColor="red"
                Text="交易方评分变化明细">
            </radTS:Tab>
        </Tabs>
    </radTS:RadTabStrip>
    <div id="new_content">
        <div id="new_zicontent">
            <div id="content_zw">
                 <div class="content_bz" >
                    <%--说明文字：<br />
                     1、该模块用于交易扣罚的查看。<br /> --%>
                </div>
                <div class="content_nr">
                    <table width="100%" cellpadding="0" cellspacing="0" class="content_nr_cx" style="border: solid 1px #ccc">
                        <tr>
                             <td width="70px" align="right">
                                变动时间：
                            </td>
                            <td width="178px">
                                <asp:TextBox class="tj_input Wdate" onclick="WdatePicker({readOnly:true,minDate:'2012-01-01',maxDate:'2022-01-01',dateFmt:'yyyy-MM-dd'});"
                                    ID="txtBeginTime" runat="server" Width="178px"></asp:TextBox>
                            </td>
                            <td align="center">
                                至
                            </td>
                            <td width="178px">
                                <asp:TextBox class="tj_input Wdate" onclick="WdatePicker({readOnly:true,minDate:'2012-01-01',maxDate:'2022-01-01',dateFmt:'yyyy-MM-dd'});"
                                    ID="txtEndTime" runat="server" Width="178px"></asp:TextBox>
                            </td>
                            <td align="left">
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;
                                <asp:Button ID="btnSearch" runat="server" CssClass="tj_bt" Width="80px" 
                                    Text="查询" onclick="btnSearch_Click" />&nbsp;&nbsp;
                                </td>
                        </tr>
                    </table>
                    <table width="100%" cellpadding="0" cellspacing="0" class="content_nr_hj">
                        <tr>
                            <td style="width:80px;text-align:right">交易方账号：</td>
                            <td> <asp:Label ID="lbzh" runat="server" ></asp:Label></td>
                            <td style="width:80px;text-align:right">交易方名称：</td>
                            <td> <asp:Label ID="lbmc" runat="server" ></asp:Label></td>
                            <td style="width:80px;text-align:right">账户类型：</td>
                            <td><asp:Label ID="lblx" runat="server" ></asp:Label></td>
                            <td style="width:80px;text-align:right">当前积分：</td>
                            <td><asp:Label ID="lbjf" runat="server" ></asp:Label></td>
                         
                        </tr>
                    </table>
                    <table cellspacing="0" cellpadding="0" border="1" bordercolor="#99BBE8"
                        style="border-collapse: collapse;table-layout:fixed;width:100%;" class="tab">
                        <tr>
                            <td>
                               <%--<div class="content_nr_lb" style="width:1080px; ">--%>
                                <table id="theObjTable" style="width:860px;" cellspacing="0" cellpadding="0">
                                    <thead>
                                        <tr>
                                            <th class="TheadTh" style="width: 160px; text-align:center;">
                                               产生时间
                                            </th> 
                                            <th class="TheadTh" style="width:500px; text-align:center;">
                                        信用等级评估记录</th>                                   
                                                                     
                                            <th class="TheadTh" style="width: 100px; text-align:center;">
                                                加减分值
                                            </th>                                                                                     
                                            <th class="TheadTh" style="width: 100px; text-align:center;">
                                                信用积分
                                            </th>                                         
                                                                                 
                                             
                                           
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater ID="rpt" runat="server" OnItemDataBound="rpt_ItemDataBound" OnItemCommand="rpt_ItemCommand" >
                                            <ItemTemplate>
                                                <tr class="TbodyTr">
                                                    <td style="width:160px; text-align:center;">
                                                        <%#((System.Data.DataRow)Container.DataItem)["创建时间"].ToString()%>
                                                     
                                                    </td>
                                                    <td   style="width:500px; text-align:center;">    
                                                        <%#((System.Data.DataRow)Container.DataItem)["原因"].ToString()%>
                                                       
                                                    </td> 
                                                    <td style="width:100px; text-align:center;">
                                                      <%#((System.Data.DataRow)Container.DataItem)["分数"].ToString()%>
                                                    </td>
                                                    <td style="width:100px">
                                                        <%#((System.Data.DataRow)Container.DataItem)["信用积分"].ToString()%>
                                                    </td>    
                                                  
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                          <tr id="tdEmpty" runat="server" visible="false"  style="text-align: center;">
                                            <td colspan="4">
                                                您查询的数据为空！
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <%--</div>--%>
                                <%--<uc1:commonpagernew ID="commonpagernew1" runat="server" />--%>
                            </td>
                        </tr>                       
                      
                    </table>
                </div>

                <div style="text-align:center;padding-top:30px"><input type="button" onclick="window.history.go(-1);" value="返回列表"  class="tj_bt_da" style="height:30px"/></div>
            </div>
        </div>
    </div>
    <input runat="server" id="hidID" type="hidden" />
    </form>
</body>
</html>
