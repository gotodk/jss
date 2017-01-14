<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DXTZ.aspx.cs" Inherits="Web_DXTZ" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<title>无标题页</title>
     <link href="../css/style.css" rel="Stylesheet" type="text/css" />
    <script type="text/JavaScript">
        function textCounter(field, counter, maxlimit, linecounter) {
            var fieldWidth = parseInt(field.offsetWidth);
            var charcnt = field.value.length;
            if (charcnt > maxlimit) {
                field.value = field.value.substring(0, maxlimit);
            }
            else {
                var percentage = parseInt(100 - ((maxlimit - charcnt) * 100) / maxlimit);
                //document.getElementById(counter).style.width = parseInt((fieldWidth * percentage) / 100) + "px";
                document.getElementById(counter).innerHTML = "已输入:<span style='color:#F63'>" + charcnt + "</span>字&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;上限<span style='color:#F63'>" + maxlimit + "</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;已使用:<span style='color:#F63'>" + percentage + "%</span>"
                //setcolor(document.getElementById(counter), percentage, "background");
            }
        }
        function setcolor(obj, percentage, prop) {
            obj.style[prop] = "rgb(80%," + (100 - percentage) + "%," + (100 - percentage) + "%)";
        }
        function SetCookie(name,value)//两个参数，一个是cookie的名子，一个是值
{

    var Days = 1; //此 cookie 将被保存 1 天
    var exp  = new Date();    //new Date("December 31, 9998");
    exp.setTime(exp.getTime() + Days*24*60*60*1000);
    document.cookie = name + "="+ escape (value) + ";expires=" + exp.toGMTString();
}
</script>
</head>
<body>
    <form id="form1" runat="server">
       <div>
       <table  border="0"  cellpadding="0" cellspacing="0" align="center">
          <tr>
             <td align="left" valign="middle" height="30px" colspan="5"><asp:Label ID="bt" runat="server" style="font-size:10pt;color:#0066ff"></asp:Label></td>
          </tr>
          <tr>
             <td height="30" align="center" valign="middle">部门：</td>
             <td align="left" valign="middle">
                <asp:DropDownList ID="Ddldeptemp" runat="server" AutoPostBack="True" OnSelectedIndexChanged="Ddldeptemp_SelectedIndexChanged" Width="150px"></asp:DropDownList>
             </td>
             <td align="left" valign="middle"></td>
             <td align="left" valign="middle" colspan="2"></td>
          </tr>
          <tr>
            <td height="30" align="center" valign="middle" >岗位：</td>
            <td align="left" valign="middle">
               <asp:DropDownList ID="Ddlgwemp" runat="server" AutoPostBack="True" OnSelectedIndexChanged="Ddlgwemp_SelectedIndexChanged"  Width="150px"></asp:DropDownList>
            </td>
            <td align="left" valign="middle" ></td>
            <td align="left" valign="middle" colspan="2"></td>
          </tr>
          <tr>
             <td height="200px"  colspan="2" align="left" valign="middle" >
                
                   <asp:ListBox ID="ListBox1" runat="server" Width="280px"    
                       style=" height:200px;border: solid 1px #CCC;"  onselectedindexchanged="ListBox1_SelectedIndexChanged"></asp:ListBox>
               
             </td>
             <td align="center" valign="middle">
                <asp:Button ID="btnadditem" runat="server" CssClass="button"  OnClick="btnadditem_Click" Text=">>" Width="45px" /><br /> <br />
                <asp:Button ID="btnclear" runat="server" CssClass="button"   OnClick="btnclear_Click" Text="<<" Width="45px" /><br /><br />
                <asp:Button ID="btnclearall" runat="server" CssClass="button"  OnClick="btnclearall_Click" Text="重置" Width="45px" />
             </td>
             <td  colspan="2"  align="left" valign="middle">
                <div style="width:300px;border:1px solid #CCC; height:195px; overflow:auto" >
                    <asp:DataList ID="dlist" runat="server"  width="300px" >
                        <ItemTemplate>
                           <table >
                              <tr >   
                                 <td style="width:15">
                                     <asp:CheckBox ID="cbchosse" runat="server" />
                                  </td>                                     
                                  <td style="width:30">
                                      <asp:Label ID="labGonghai" runat="server" Text= '<%# DataBinder.Eval(Container.DataItem, "工号")%>' Font-Size="Small"></asp:Label>
                                  </td>
                                  <td style="width:30" >
                                       <asp:Label ID="labxingming" runat="server" Text= '<%# DataBinder.Eval(Container, "DataItem.姓名") %>' Font-Size="Small"></asp:Label>
                                                      
                                  </td>
                                  <td>
                                        <asp:Label runat="server" ID="sjh" Text="手机号："></asp:Label>
                                  </td>
                                  <td style="width:30" >
                                        <asp:Label ID="labsj" runat="server" Text= '<%# DataBinder.Eval(Container, "DataItem.手机号") %>' Font-Size="Small"></asp:Label>
                                                      
                                  </td>     
                                  <!--<td style="width:15" valign="top" >
                                        <asp:CheckBox ID="cbMesWroning" runat="server" Enabled="false"  Text="接收短信提醒" />
                                  </td>-->            
                              </tr>
                           </table>               
                        </ItemTemplate>
                    </asp:DataList>
                </div>
             </td>
          </tr>
          <tr>
            <td height="30" align="center" valign="middle">短信内容：</td>
            <td colspan="4"><asp:TextBox ID="txtDXNR" runat="server" Width="560px" TextMode="MultiLine" Rows="5"  onKeyUp="textCounter(this,'baifenbi123',3000)"></asp:TextBox></td>
         </tr>  
         <tr>
           <td height="30" align="left" valign="middle">&nbsp;</td>
           <td align="left" valign="middle" >
              <asp:Button ID="Btnsave" runat="server" Text="发送" OnClick="Btnsave_Click" CssClass="button"   OnClientClick="javascript:return confirm('确定发送短信提醒！');" Style="height: 20px" Width="64px" />  
              <asp:Button ID="BtnQX" runat="server" Text="取消"  CssClass="button" Height="20px"  Width="64px" OnClick="BtnQX_Click" />    
           </td>
           <td align="left" valign="middle" colspan="3"><div id="baifenbi123"></div><script> textCounter(document.getElementById("txtSQYJ"), "baifenbi123", 3000)</script>
           </td>

         </tr>
       </table>
       </div>
    </form>
</body>
</html>
