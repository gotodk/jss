<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cd.aspx.cs" Inherits="JHJXPT_set_cd" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            height: 64px;
        }
        .style2
        {
            height: 76px;
        }
        .style3
        {
            height: 81px;
        }
        .style4
        {
            height: 44px;
        }
        .style5
        {
            height: 173px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table width="100%"  border="1">
  <tr>
    <td width="30%" align="left" valign="top">
            <asp:TreeView ID="TV" runat="server" ShowLines="True">
                <NodeStyle Font-Size="Small" NodeSpacing="0px" />
        </asp:TreeView>
    
    </td>
    <td width="70%" valign="top">
    <table width="100%" border="1">
      <tr>
        <td class="style1">加载菜单：</td>
        <td class="style1">数据库表：<asp:DropDownList ID="TBTable" runat="server">
            <asp:ListItem Selected="True">ZZ_tbMenuAdmin</asp:ListItem>
            <asp:ListItem>ZZ_tbMenuSPFL</asp:ListItem>
            </asp:DropDownList>
            <br />
            <asp:Button ID="Button1" runat="server" Text="加载" onclick="Button1_Click" />
            <br />
          </td>
      </tr>
      <tr>
        <td width="186" class="style2">当前选中节点：</td>
        <td width="398" class="style2">节点SortID:<asp:TextBox ID="TBSortID" runat="server" 
                Width="197px"></asp:TextBox>
            <br />
            节点名称：<asp:TextBox ID="TBname" runat="server" Width="200px"></asp:TextBox>
            </td>
        </tr>
      <tr>
        <td class="style3">在当前节点添加子菜单：</td>
        <td class="style3">父级SortID(0是顶级):<asp:TextBox ID="ADD_TBFID" runat="server" 
                Width="126px"></asp:TextBox>
            <br />
            分类名称：<asp:TextBox ID="ADD_name" runat="server" Width="200px"></asp:TextBox>
            <br />
            <asp:Button ID="Button2" runat="server" Text="添加" onclick="Button2_Click" OnClientClick="javascript:return confirm('确定要添加新节点吗？');"  />
            <br />
            </td>
        </tr>
      <tr>
        <td class="style4">删除当前菜单：</td>
        <td class="style4">
            <asp:Button ID="Button3" runat="server" Text="隐藏" onclick="Button3_Click" OnClientClick="javascript:return confirm('确定要隐藏选中的节点吗？');"  />
            <asp:Button ID="Button4" runat="server" Text="删除" onclick="Button4_Click" OnClientClick="javascript:return confirm('确定要删除选中的节点吗？');"  />
            </td>
        </tr>
      <tr>
        <td class="style5">修改当前节点配置：</td>
        <td class="style5">分类名称：<asp:TextBox ID="ADD_name0" runat="server" Width="200px"></asp:TextBox>
            <br />
            绑定URL:<asp:TextBox ID="ADDurl0" runat="server" Width="400px"></asp:TextBox>
            <br />
            绑定URL特殊参数：<asp:TextBox ID="ADDurlsp0" runat="server" Width="138px"></asp:TextBox>
            <br />
            绑定URL开打方式：<asp:DropDownList ID="DDLURLTarget0" runat="server">
                <asp:ListItem Selected="True">_top</asp:ListItem>
                <asp:ListItem>_blank</asp:ListItem>
                <asp:ListItem>_self</asp:ListItem>
                <asp:ListItem>_parent</asp:ListItem>
                <asp:ListItem>其他</asp:ListItem>
            </asp:DropDownList>
            自定义<asp:TextBox ID="DDLURLTarget_d0" runat="server" Width="82px"></asp:TextBox>
            <br />
            显示权限：<asp:DropDownList ID="DDLQX0" runat="server">
                <asp:ListItem Selected="True">无限制</asp:ListItem>
                <asp:ListItem>隐藏</asp:ListItem>
                <asp:ListItem>自定义</asp:ListItem>
            </asp:DropDownList>
            自定义<asp:TextBox ID="DDLQX_d0" runat="server" Width="112px"></asp:TextBox>
            <br />
            ToolTip说明：<asp:TextBox ID="ADD_tip0" runat="server" Width="234px"></asp:TextBox>
            <br />
            <asp:Button ID="Button5" runat="server" Text="修改" onclick="Button5_Click" OnClientClick="javascript:return confirm('确定要修改选中的节点吗？');"  />
            </td>
      </tr>
      <tr>
        <td>调整当前节点排序：</td>
        <td>
            <asp:Button ID="Button7" runat="server" Text="往上排一个" 
                onclick="Button7_Click"   />
            &nbsp;&nbsp;&nbsp;
            <asp:Button ID="Button8" runat="server" Text="往下排一个" 
                onclick="Button8_Click"   />
            </td>
      </tr>
      <tr>
        <td>移动当前节点归属：</td>
        <td>新父级SortID：<asp:TextBox ID="tbydgs" runat="server" Width="112px"></asp:TextBox>
            <asp:Button ID="Button9" runat="server" Text="移动"
                OnClientClick="javascript:return confirm('确定要移动选中的节点吗？');" 
                onclick="Button9_Click"  />
            </td>
      </tr>
      <tr>
        <td>特殊操作：</td>
        <td>
            <asp:Button ID="Button6" runat="server" Text="重新整理纠错，危险" 
                onclick="Button6_Click"   OnClientClick="javascript:return confirm('确定要重新整理菜单吗？');" />
          </td>
      </tr>
    </table>
    
    </td>
  </tr>
</table>

    
    </div>
    </form>
</body>
</html>
