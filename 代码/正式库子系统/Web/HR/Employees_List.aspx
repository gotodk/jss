<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Employees_List.aspx.cs" Inherits="Web_HR_Employees_List" %>

<%@ Register assembly="RadTabStrip.Net2" namespace="Telerik.WebControls" tagprefix="radTS" %>

<%@ Register assembly="RadGrid.Net2" namespace="Telerik.WebControls" tagprefix="radG" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<script src="/js/setday.js" type="text/javaScript"></script>

    <title>在职员工档案查询</title>
    <link href="../../css/style.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            width: 100%;
            height: 15px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
     <radTS:RadTabStrip ID="RadTabStrip2" runat="server" Skin="Default2006">
     <Tabs>
        <radTS:Tab ID="Tab1" runat="server" Text="在职员工档案查询" NavigateUrl="../HR/Employees_List.aspx">
            </radTS:Tab>
         
       <radTS:Tab ID="Tab2" runat="server" Text="详细信息导出" NavigateUrl="../HR/YGXX_Export.aspx"> </radTS:Tab>
     </Tabs>
     </radTS:RadTabStrip>        
        <table><tr><td>&nbsp;当前结果中共有<b><span style ="color :red"><%=renshu %></span></b>个人</td></tr></table>    
      <table border="0" cellpadding="0" cellspacing="0">
      <tr>
      <td width="40" align ="right">排序:</td>
      <td width="80" align ="left">
         <asp:DropDownList ID="DropDownListPX" runat="server" Width ="80">
               <asp:ListItem Selected="True" Value="Number">员工编号</asp:ListItem>
               <asp:ListItem Value="Employee_Name">姓名</asp:ListItem>
               <asp:ListItem Value="Employee_Sex">性别</asp:ListItem>
               <asp:ListItem Value="LS">隶属</asp:ListItem>
               <asp:ListItem Value="BM">部门</asp:ListItem>
               <asp:ListItem Value="GWMC">岗位名称</asp:ListItem>                    
         </asp:DropDownList>
      </td>
       <td width="40px" align ="right"> 隶属:</td>
      <td width="80px" align ="left"><asp:DropDownList ID="DropDownListLS" runat="server" Width ="80px"></asp:DropDownList></td>
      <td width="40px" align ="right"> 部门:</td>
      <td width="80px" align ="left"><asp:DropDownList ID="DropDownListBM" runat="server" Width="100px"></asp:DropDownList></td>
      <td width="40" align="right">工号:</td>
      <td width="70" align ="left"><asp:TextBox ID="TextBoxGH" runat="server" Width="70px"></asp:TextBox></td>
      <td width="40" align="right">姓名:</td>
      <td width="70" align ="left"><asp:TextBox ID="TextBoxXM" runat="server" Width="70px"></asp:TextBox></td>
      <td width="40" align ="right">学历:</td>
      <td width="90" align ="left">
         <asp:DropDownList ID="DropDownListXL" runat="server" Width ="90">
               <asp:ListItem Selected="True" Value ="全部">全部</asp:ListItem>
               <asp:ListItem Value="初中及以下">初中及以下</asp:ListItem>
               <asp:ListItem Value="高中">高中</asp:ListItem>
               <asp:ListItem Value="技校">技校</asp:ListItem>
               <asp:ListItem Value="中专">中专</asp:ListItem>
               <asp:ListItem Value="大专">大专</asp:ListItem>
               <asp:ListItem Value="本科">本科</asp:ListItem>
               <asp:ListItem Value="硕士研究生">硕士研究生</asp:ListItem>
               <asp:ListItem Value="博士研究生">博士研究生</asp:ListItem>                    
         </asp:DropDownList>
      </td>
      <td width="40" align="right">专业:</td>
      <td width="70" align ="left"><asp:TextBox ID="TextBoxZY" runat="server" Width="80"></asp:TextBox></td>
      <td width="80" align ="right" >出生日期:&nbsp;从</td>
      <td width="45" align="center" ><input type ="text" id="time1" name="time1" maxlength="50" onfocus = "setday(this)" size="7" class="input1"  onblur=" return closeDateWindow();" runat="server" /></td>
      <td width="25" align="center" >到</td>
      <td width="45" align="center"> <input type ="text" id="time2" name="time2" maxlength="50" onfocus = "setday(this)" size="7" class="input1"  onblur=" return closeDateWindow();" runat="server"  /></td>
                    
   <td width="70" align ="right"><asp:ImageButton ID="ImageButton1" runat="server" Height="18px" ImageUrl="~/Web/images/20070927165819838.gif"  Width="59px" OnClick="ImageButton1_Click" /></td>
 </tr>
   </table>           
            
    <table border="0" cellpadding="0" cellspacing="0" style ="width:100%">
            <tr>
                <td align="left" class="style1">
                    <radG:RadGrid ID="RadGrid1" runat="server" GridLines="None" 
                        DataSourceID="SqlDataSource1">
                    <PagerStyle NextPageText="下一页" HorizontalAlign="Right" PagerTextFormat=" {4} &amp;nbsp;|&amp;nbsp; 当前页 {0} / {1}, 当前记录{2} 到 {3} 共 {5}条." PrevPageText="上一页"></PagerStyle>
<ExportSettings>
 
<Pdf PageWidth="8.5in" PageHeight="11in" PageTopMargin="" PageBottomMargin="" PageLeftMargin="" PageRightMargin="" PageHeaderMargin="" PageFooterMargin=""></Pdf>
 
</ExportSettings>

<MasterTableView AutoGenerateColumns="False" DataKeyNames="Number" DataSourceID="SqlDataSource1" OnPageIndexChanged="RadGrid1_PageIndexChanged" >
<RowIndicatorColumn Visible="False">
<HeaderStyle Width="20px"></HeaderStyle>
</RowIndicatorColumn>

<ExpandCollapseColumn Visible="False" Resizable="False">
<HeaderStyle Width="20px"></HeaderStyle>
</ExpandCollapseColumn>
 <NoRecordsTemplate>
                     没有找到任何数据
 </NoRecordsTemplate>
    <Columns>
        <radG:GridBoundColumn DataField="Number" HeaderText="员工编号" UniqueName="Number">
         </radG:GridBoundColumn>
         <radG:GridBoundColumn DataField="Employee_Name" HeaderText="姓名" UniqueName="Employee_Name">
         </radG:GridBoundColumn>
        <radG:GridBoundColumn DataField="Employee_Sex" HeaderText="性别" UniqueName="Employee_Sex">
         </radG:GridBoundColumn>
        <radG:GridBoundColumn DataField="LS" HeaderText="隶属" UniqueName="LS">
        </radG:GridBoundColumn>
        <radG:GridBoundColumn DataField="BM" HeaderText="部门" UniqueName="BM">
        </radG:GridBoundColumn>
        <radG:GridBoundColumn DataField="GWMC" HeaderText="岗位名称"  UniqueName="GWMC">
        </radG:GridBoundColumn>
        <radG:GridBoundColumn DataField="XL1" HeaderText="学历"  UniqueName="XL1">        
        </radG:GridBoundColumn>
        <radG:GridBoundColumn DataField="ZY1" HeaderText="专业"  UniqueName="ZY1">  
        </radG:GridBoundColumn>
        <radG:GridBoundColumn DataField="Employee_BirthDay" HeaderText="出生日期"  UniqueName="Employee_BirthDay">
        </radG:GridBoundColumn>
       
         <radg:GridTemplateColumn UniqueName="TemplateColumn" AllowFiltering="False">
                        <ItemTemplate>
                           <a href='Employees_List_Rusult.aspx?Number=<%# DataBinder.Eval(Container, "DataItem.Number") %>' >  <img src="../../images/a.gif"/></a>
                        </ItemTemplate>
                        <HeaderTemplate>
                            查看详情
                        </HeaderTemplate>
         </radg:GridTemplateColumn>
         
        <%-- <radg:GridTemplateColumn UniqueName="TemplateColumn2" AllowFiltering="False">
                        <ItemTemplate>
                           <a href='YGXX_Print.aspx?Number=<%# DataBinder.Eval(Container, "DataItem.Number") %>' >  <img src="../../images/jpg/Update.gif"/></a>
                        </ItemTemplate>
                        <HeaderTemplate>
                           打印
                        </HeaderTemplate>
         </radg:GridTemplateColumn>--%>
         
    </Columns>
</MasterTableView>
                    </radG:RadGrid>
                </td>
            </tr>
            <tr>
                <td align="left" style="width: 100%">
                    </td>
            </tr>
            </table>
     <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
         ConnectionString="<%$ ConnectionStrings:FMOPConn %>" SelectCommand="SELECT Number , Employee_Name, Employee_Sex , LS , BM, GWMC,convert(varchar(10),Employee_BirthDay,120) as Employee_BirthDay,XL1 FROM HR_Employees WHERE 1!=1"></asp:SqlDataSource>
    </form>
</body>
</html>
