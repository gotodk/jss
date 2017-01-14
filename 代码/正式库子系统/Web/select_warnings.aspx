<%@ Page Language="C#" AutoEventWireup="true" CodeFile="select_warnings.aspx.cs" Inherits="select_warnings" ValidateRequest="false" EnableEventValidation="false" %>

<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>显示提醒/报警信息</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.js" type="text/javascript"></script>
    <style type="text/css">
    .tabHead{
    background-image: url(images/headerbg.jpg);
    
    }
    </style>
    <script language="javascript" type="text/javascript" src="../js/tigra_tables.js"></script>
    <script language="javascript" type="text/javascript">
//    var zxzx = 1;
//    function flash_z(){ 
//    if(zxzx == 1)
//    {
//    zxzx = 2;
//    var node = document.getElementById("MyDataGrid");//获取第一个表格
//var child = node.getElementsByTagName("tr")[1];//获取行的第一个单元格

//if (child.firstChild.style.backgroundColor!="red") 
//{
//child.firstChild.style.backgroundColor="red" ;
//child.firstChild.style.color="#ffffff";
//}
//else 
//{
//child.firstChild.style.backgroundColor="green" ;
//child.firstChild.style.color="#ffffff";
//}
//zxzx = 1;
//    }


//} 
function showone()
{
if(document.getElementById("lblCurrentIndex").innerHTML.replace(/\s/g,"") == "第1页")
{
    var node = document.getElementById("MyDataGrid");//获取第一个表格
var child = node.getElementsByTagName("tr")[1];//获取行的第一个单元格
child.firstChild.style.color="red";
}


//setInterval('flash_z()',500);
}

        //获得弹出窗口
        function GetRadWindow() {
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow;
            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
            return oWindow;
        }

        function setFrame(url) {
            alert(url);
            rightFrame.location.href = url;
            var oWindow = GetRadWindow(); 
            oWindow.Close();
        }
       

        function refreshLimitTime() {
            var divs = $(".CheckInfoTrue");
            for (var i = 0; i < divs.size(); i++) {
                var secs = parseInt(divs.eq(i).data("limitTime"));
                divs.eq(i).data("limitTime", secs - 1);
                var limitTime = "";
                if (secs < 0) {
                    limitTime = "已经超时";
                }
                else {
                    var days = Math.floor(secs / 24 / 60 / 60);
                    var hours = Math.floor(secs / 3600) - days * 24;
                    var mins = Math.floor(secs / 60) - days * 24 * 60 - hours * 60;
                    var secs = secs - days * 24 * 60 * 60 - hours * 60 * 60 - mins * 60;
                    if (days > 0) limitTime += days + '天';
                    if (hours > 0) limitTime += hours + '小时';
                    else {
                        if (limitTime != "") limitTime += '0小时';
                    }
                    if (mins > 0) limitTime += mins + '分';
                    else {
                        if (limitTime != "") limitTime += '0分';
                    }
                    if (secs > 0) limitTime += secs + '秒';
                    else {
                        limitTime += '0秒';
                    } 
                }
                divs.eq(i).html(limitTime);                
            }
            setTimeout('refreshLimitTime()', 1000) 
        }


        $(document).ready(function() {
            $(".CheckInfoFalse").html("");
            var divs = $(".CheckInfoTrue");
            for (var i = 0; i < divs.size(); i++) {
                divs.eq(i).data("limitTime", divs.eq(i).html());
                divs.eq(i).html("");
            }
            <%
    if (RadioButtonList1.SelectedValue == "0") 
    {
    %>
            refreshLimitTime();<%
    }

%> 
        });
       window.onerror = function() {
            return true;
        } 

    </script>
</head>
<body style="margin:3px">
   <form id="Form1" runat="server">
   <bgsound loop=false autostart=false id="bgss123"> 
<script language="javascript">
    function Select_m123(url) {
        try {
            document.all.bgss123.src = url;
            bgss123.play();
        }
        catch (e) {

        }
    } 
</script>
        <script language="JavaScript" type="text/javascript">
			<!--
				tigra_tables('MyDataGrid', 1, 0, '#F3F3F3', '#ffffff', '#F1F6F9', '#E0EFF2');
			// -->
			</script>
		<table width="100%" align="center">
            <tr>
                <td align="left">
                <table border="0" align="left" cellpadding="0" cellspacing="0" >
  <tr>
    <td  align="left" valign="top" style=" padding-top:8px" ><asp:Label ID="Label1" runat="server"></asp:Label>
        </td>
    <td  align="left" valign="top"><asp:RadioButtonList ID="RadioButtonList1" runat="server" 
                        RepeatDirection="Horizontal" >
                        <asp:ListItem Selected="True" Value="0">未查看</asp:ListItem>
                        <asp:ListItem Value="1">已查看</asp:ListItem></asp:RadioButtonList></td>
                        <td  align="left" valign="top"  style=" padding-top:7px" ><asp:Label ID="Label2" runat="server" ForeColor="#FF3300"></asp:Label></td>
    <td  align="left" valign="middle"> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 来自：</td>
    <td  align="left" valign="middle"><asp:TextBox ID="TextBox1" runat="server" 
            Width="70px"></asp:TextBox></td>
    <td  align="left" valign="middle" style=" padding-left:10px"><asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/Web/images/20070927165819838.gif"
                        OnClick="ImageButton2_Click" /></td>
                        <td  align="left" valign="middle" style=" padding-left:20px">
<asp:ListBox 
                                ID="ListBox_sy" runat="server" AutoPostBack="True" Rows="1" 
                                onselectedindexchanged="ListBox_sy_SelectedIndexChanged">
<asp:ListItem Selected="True" Value="001.wav">默认音</asp:ListItem>
                                <asp:ListItem Value="002.wav">清爽1</asp:ListItem>
                                <asp:ListItem Value="003.wav">清爽2</asp:ListItem>
                                <asp:ListItem Value="004.wav">欢快1</asp:ListItem>
                                <asp:ListItem Value="005.wav">轻声1</asp:ListItem>
                                <asp:ListItem Value="006.wav">轻声2</asp:ListItem>
                            </asp:ListBox>
                        
                            </td>
                            <td>&nbsp;&nbsp;&nbsp; <asp:Button ID="Button1" runat="server" Text="全部转为已查看" 
                                    Width="108px" CssClass="button" ForeColor="#FF3300" 
                                    style=" font-weight:normal " onclick="Button1_Click"></asp:Button></td>
  </tr>
</table>

                    
                    
                    
                    
                </td>
            </tr>
		<tr>
		<td align="right" style="height: 73px;  ">

        <asp:datagrid id="MyDataGrid" runat="server" AutoGenerateColumns="False" name="MyDataGrid"
	        HorizontalAlign="Center" AlternatingItemStyle-BackColor="#eeeeee"
	        CellPadding="3" BorderWidth="1px"
	        BorderColor="#A2B5C6" OnPageIndexChanged="MyDataGrid_Page" PagerStyle-HorizontalAlign="Right"
	        PagerStyle-Mode="NumericPages" PageSize="4" AllowPaging="True" OnItemCommand="MyDataGrid_ItemCommand" OnItemDataBound="MyDataGrid_ItemDataBound" Width="100%" Font-Names="Verdana">
          <AlternatingItemStyle BackColor="#EEEEEE"></AlternatingItemStyle>
          <HeaderStyle Font-Bold="False" HorizontalAlign="Center" ForeColor="Black" CssClass="tabHead" Height="28px"></HeaderStyle>
          <PagerStyle HorizontalAlign="Right" Mode="NumericPages" Visible="False"></PagerStyle>
          <Columns>
          <asp:BoundColumn HeaderText="ID" DataField="id" Visible="False">
          </asp:BoundColumn>
              <asp:BoundColumn DataField="context" HeaderText="内容"></asp:BoundColumn>
               <asp:TemplateColumn HeaderText="来自员工">
                  <ItemTemplate>
                      <%# DataBinder.Eval(Container.DataItem, "Employee_Name").ToString() == "" ? DataBinder.Eval(Container.DataItem, "FromUser") : DataBinder.Eval(Container.DataItem, "Employee_Name")%>
                  </ItemTemplate>
              </asp:TemplateColumn>
               <asp:BoundColumn DataField="toUser" HeaderText="信息接收人" Visible="false">
              </asp:BoundColumn>
              <asp:BoundColumn DataField="Grade" HeaderText="类别" Visible="false">
              </asp:BoundColumn>
              <asp:BoundColumn HeaderText="信息类别"  Visible="false"></asp:BoundColumn>
          <asp:BoundColumn HeaderText="产生日期" DataField="CreateTime" >
          </asp:BoundColumn>
              <asp:TemplateColumn HeaderText="剩余时间">
                <ItemTemplate><div style="color:Red" class="CheckInfo<%#Eval("IsCheckInfo").ToString() %>"><%#Eval("limitSec") %></div></ItemTemplate>
              </asp:TemplateColumn>
              <asp:TemplateColumn HeaderText="查看">
                  <ItemTemplate>
                      <asp:ImageButton ID="ImageButton1" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Id") %>'
                          CommandName="select" ImageUrl="images/a.gif" OnCommand="ImageButton1_Command1"  />
                          
                  </ItemTemplate>
              </asp:TemplateColumn>

              <asp:TemplateColumn HeaderText="转为已查看">
                  <ItemTemplate>
                  <asp:ImageButton ID="Button123" runat="server" Width="13" Height="13"  ImageUrl="images/ok.gif" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Id") %>'   CommandName="select" OnCommand="Button123_Click" Visible='<%#Eval("IsRead").Equals(bool.Parse("False")) %>'  />
                     
                  </ItemTemplate>
              </asp:TemplateColumn>
          </Columns>
            <ItemStyle HorizontalAlign="Center" />
        </asp:datagrid>
		  <asp:label id="lblPageCount" runat="server"></asp:label>&nbsp;
          <asp:label id="lblCurrentIndex" runat="server"></asp:label>
          <asp:linkbutton id="btnFirst" onclick="PagerButtonClick" runat="server" 
  	        Font-size="8pt" ForeColor="navy" CommandArgument="0"></asp:linkbutton>&nbsp;
          <asp:linkbutton id="btnPrev" onclick="PagerButtonClick" runat="server" 
  	        Font-size="8pt" ForeColor="navy" CommandArgument="prev"></asp:linkbutton>&nbsp;
          <asp:linkbutton id="btnNext" onclick="PagerButtonClick" runat="server"
  	        Font-size="8pt" ForeColor="navy" CommandArgument="next"></asp:linkbutton>&nbsp;
          <asp:linkbutton id="btnLast" onclick="PagerButtonClick" runat="server" 
  	        Font-size="8pt" ForeColor="navy" CommandArgument="last"></asp:linkbutton>
		</td>
		</tr>
		</table>
       &nbsp;&nbsp;
   </form>

</body>
</html>
