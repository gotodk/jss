<%@ Page Language="C#" AutoEventWireup="true" CodeFile="JJRCX.aspx.cs" Inherits="mywork_GXTW_JJRCX" %>

<%@ Register Src="../../pagerdemo/commonpager.ascx" TagName="commonpager" TagPrefix="uc2" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link href="dh.css" rel="stylesheet" />
<link href="Css/fwsite.css" rel="stylesheet" />
    <script src="js/jquery-1.7.2.min.js"></script>
    <script src="js/My97DatePicker/WdatePicker.js"></script>
</head>
<body onload="ShowUpdating()"  style=" text-align:left;" >
    <form id="form1" runat="server">
        
     <table width="100%" border="0" cellspacing="0" cellpadding="0" class="newstop">
        <tr>
            <td width="50" height="29" align="right">
                &nbsp;&nbsp;
            </td>
            <td align="left">
                您现在的位置：  经纪人查询 
            </td>
        </tr>
    </table>
        <table width="100%" border="0" cellspacing="0" cellpadding="0" >
        <tr>
            <td width="50" height="29" align="right">
                &nbsp;&nbsp;
            </td>
            <td align="left">                
            </td>
        </tr>
    </table>
    <div class="content_nr" id="divLB" runat="server" style=" width:800px; margin-left:50px; text-align:left;">
        
        <table width="100%" cellpadding="0" cellspacing="0" border="0">
            <tr>
                <td align="left">
                    <div class="tab1">
                        <ul id="test1_li_now">
                            <li class="now" ><a id="aJJRSYCX" runat="server" href="JJRCX.aspx">经纪人信息查询</a></li>
                            <li><a id="aJJRXXCX" runat="server" href="JJRCX_SY.aspx">经纪人收益查询</a></li>
                        </ul>
                    </div>
                </td>
            </tr>
        </table>
        <br />
        <table width="100%" cellpadding="0" cellspacing="0" class="content_nr_cx" style="border: solid 1px #ccc">                        
                        <tr>  
                            <td width="110px" align="right">
                                注册时间：
                            </td>
                            <td width="178px">
                            <asp:TextBox class="tj_input Wdate" onclick="WdatePicker({readOnly:true,minDate:'2012-01-01',maxDate:'2022-01-01',dateFmt:'yyyy-MM-dd'});"
                                ID="txtBeginTime" runat="server" Width="178px"></asp:TextBox>                            
                            </td>
                            <td width="85px" align="center">
                                至
                            </td>
                            <td width="178px">
                            <asp:TextBox class="tj_input Wdate" onclick="WdatePicker({readOnly:true,minDate:'2012-01-01',maxDate:'2022-01-01',dateFmt:'yyyy-MM-dd'});"
                                ID="txtEndTime" runat="server" Width="178px"></asp:TextBox>                            
                            </td>                             
                            <td  align="center" style=" padding-left:5px; width:149px;">
                                <asp:Label ID="Label1" runat="server" Text="正在查询中，请稍后..." Width="140px" style=" display:none;"></asp:Label>
                                <asp:Button ID="BtnCheck" runat="server" CssClass="tj_bt" UseSubmitBehavior="False"
                                    Text="查询" Width="70px" OnClientClick="Click();" OnClick="BtnCheck_Click"  />
                            </td>
                            <td  align="left" colspan="4" style=" padding-left:10px; width:100px;">
                            <asp:Button ID="btnToExcel" runat="server" CssClass="tj_bt" UseSubmitBehavior="False"
                                    Text="导出" Width="70px" OnClick="btnToExcel_Click"/>
                                </td>
                        </tr>
                        <tr>  
                           <td align="right">
                                经纪人编号：
                            </td>
                            <td>
                            <asp:TextBox ID="txtJJRBH" runat="server" aa="aa" CssClass="tj_input" Width="178px"
                                    Enabled="True" TabIndex="1"></asp:TextBox>                            
                            </td>
                            <td align="center">
                                经纪人姓名：
                            </td>
                            <td>
                                <asp:TextBox ID="txtJJRXM" runat="server" aa="aa" CssClass="tj_input" Width="178px"
                                    Enabled="True" TabIndex="1"></asp:TextBox>                            
                                                        
                            </td> 
                            <td align="left" colspan="2" style=" padding-left:10px;">
                                经纪人院系：
                                <asp:DropDownList ID="ddrJJRYX" runat="server" Width="110px" CssClass="tj_input"
                                    Height="22px" style=" margin-left:5px;">
                                </asp:DropDownList>
                            </td>         
                        </tr>
                    </table>

                    <table width="100%" cellpadding="0" cellspacing="0" class="content_nr_hj" >
                        <tr>
                            <td>
                                <%--说明文字--%>
                            </td>
                        </tr>
                    </table>
                    <table align="left" cellpadding="0" cellspacing="0" class="table_nr" style=" width:100%; margin-left:0px;">
                        <tr>
                            <td align="left" style="height: 25px" valign="bottom" class="title">
                                当前经纪人总数：<asp:Label ID="lblJJRZS" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table cellspacing="0" cellpadding="0" border="1px" bordercolor="#D1CFCF" style="border-style: solid;
                                    border-collapse: collapse; text-align: center; width: 100%;">
                                    <thead>
                                        <tr style="background-color: #D6E3F3; font-weight: bold;">
                                            <td height="28" width="180px">
                                                院（系）
                                            </td>
                                            <td height="28" width="170px">
                                                经纪人编号
                                            </td>
                                            <td height="28" width="150px">
                                                名称
                                            </td>
                                            <td height="28" width="150px">
                                                身份证号
                                            </td>
                                            <td height="28" width="150px">
                                                注册时间
                                            </td>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater ID="rpt" runat="server">
                                            <ItemTemplate>
                                                <tr style="height: 28px;">
                                                    <td width="180px" title='<%#Eval("院系") %>'>
                                                        <%#Eval("院系").ToString().Length>15?Eval("院系").ToString().Substring(0,15):Eval("院系").ToString()%>
                                                    </td>
                                                    <td width="170px" title='<%#Eval("经纪人编号")%>'>
                                                        <%#Eval("经纪人编号")%>
                                                    </td>
                                                    <td width="150px" title='<%#Eval("名称")%>'>
                                                        <%#Eval("名称").ToString().Length>15?Eval("名称").ToString().Substring(0,15):Eval("名称").ToString()%>
                                                    </td>
                                                    <td width="150px">
                                                        <%#Eval("身份证号") %>
                                                    </td>
                                                    <td width="150px">
                                                       <%#Eval("分公司审核时间") %>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tbody>
                                    <tfoot id="tempty" runat="server">
                                        <tr>
                                            <td colspan="5">
                                                您所查询的数据为空！
                                            </td>
                                        </tr>
                                    </tfoot>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <uc2:commonpager ID="commonpager1" runat="server" />
                            </td>
                        </tr>
                    </table>

                    <div id="export" runat="server" style="width:100%; display:none;">
                        <table cellspacing="0" cellpadding="0" border="1px" bordercolor="#D1CFCF" style="border-style: solid;
                                    border-collapse: collapse; text-align: center; width: 100%;">
                                    <thead>
                                        <tr>
                                            <th height="28" width="180px">
                                                院（系）
                                            </th>
                                            <th height="28" width="170px">
                                                经纪人编号
                                            </th>
                                            <th height="28" width="150px">
                                                名称
                                            </th>
                                            <th height="28" width="150px">
                                                身份证号
                                            </th>
                                            <th height="28" width="150px">
                                                注册时间
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater ID="Repeater1" runat="server">
                                            <ItemTemplate>
                                                <tr style="height: 28px;">
                                                    <td width="180px">
                                                        <%#Eval("院系")%>
                                                    </td>
                                                    <td width="170px">
                                                        <%#Eval("经纪人编号")%>
                                                    </td>
                                                    <td width="150px">
                                                        <%#Eval("名称")%>
                                                    </td>
                                                    <td width="150px">
                                                        <%#Eval("身份证号") %>
                                                    </td>
                                                    <td width="150px">
                                                       <%#Eval("分公司审核时间") %>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tbody>
                                    <tfoot id="Tfoot1" runat="server">
                                        <tr>
                                            <td colspan="5" style="text-align:center;">
                                                您所查询的数据为空！
                                            </td>
                                        </tr>
                                    </tfoot>
                                </table>

                    </div>

        
                    
                </div>
    </form>
    <script lang="ja" type="text/javascript">
        function ShowUpdating() {
            if ($("#divLB").length > 0) {
                document.getElementById("<%=BtnCheck.ClientID %>").style.display = 'block';
                document.getElementById("<%=btnToExcel.ClientID %>").style.display = 'block';
                document.getElementById("Label1").style.display = 'none';
            }
        }
        function Click() {
            document.getElementById("<%=BtnCheck.ClientID %>").style.display = 'none';
        document.getElementById("<%=btnToExcel.ClientID %>").style.display = 'none';
        document.getElementById("Label1").style.display = 'block';
    }
</script>
</body>
</html>
