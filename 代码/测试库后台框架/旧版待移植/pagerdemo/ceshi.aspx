<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ceshi.aspx.cs" Inherits="FWPTZS_ceshi" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>

</head>
<body>
    
    <form id="form1" runat="server">
    <asp:GridView ID="GridView2" runat="server" 
        EnableModelValidation="True" onrowcreated="GridView2_RowCreated" 
        Width="980px" BorderWidth="1px" BackColor="White" BorderColor="#CCCCCC" 
        BorderStyle="None" CellPadding="3"  >
        <FooterStyle BackColor="White" ForeColor="#000066" />
        <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
        <RowStyle ForeColor="#000066" />
        <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
    </asp:GridView>
    <asp:TreeView ID="TreeView123456" runat="server" Visible="False">
        <Nodes>
            <asp:TreeNode Text="姓氏" Value="姓氏"></asp:TreeNode>
            <asp:TreeNode Text="人数" Value="人数">
                <asp:TreeNode Text="年龄" Value="年龄">
                    <asp:TreeNode Text="18以下" Value="18以下">
                        <asp:TreeNode Text="男" Value="男"></asp:TreeNode>
                        <asp:TreeNode Text="女" Value="女"></asp:TreeNode>
                    </asp:TreeNode>
                    <asp:TreeNode Text="19到30" Value="19到30">
                        <asp:TreeNode Text="男" Value="男"></asp:TreeNode>
                        <asp:TreeNode Text="女" Value="女"></asp:TreeNode>
                    </asp:TreeNode>
                    <asp:TreeNode Text="30以上" Value="30以上">
                        <asp:TreeNode Text="男" Value="男"></asp:TreeNode>
                        <asp:TreeNode Text="女" Value="女"></asp:TreeNode>
                    </asp:TreeNode>
                </asp:TreeNode>
                <asp:TreeNode Text="地区" Value="地区">
                    <asp:TreeNode Text="北京" Value="北京">
                        <asp:TreeNode Text="北京西" Value="北京西"></asp:TreeNode>
                        <asp:TreeNode Text="北京东" Value="北京东"></asp:TreeNode>
                    </asp:TreeNode>
                    <asp:TreeNode Text="济南" Value="济南">
                    </asp:TreeNode>
                    <asp:TreeNode Text="上海" Value="上海">
                    </asp:TreeNode>
                    <asp:TreeNode Text="广州" Value="广州"></asp:TreeNode>
                    <asp:TreeNode Text="天津" Value="天津"></asp:TreeNode>
                    <asp:TreeNode Text="沈阳" Value="沈阳"></asp:TreeNode>
                </asp:TreeNode>
            </asp:TreeNode>
            <asp:TreeNode Text="平均身高" Value="平均身高"></asp:TreeNode>
            <asp:TreeNode Text="考试成绩平均分" Value="考试成绩平均分">
                <asp:TreeNode Text="男性" Value="男性"></asp:TreeNode>
                <asp:TreeNode Text="女性" Value="女性"></asp:TreeNode>
            </asp:TreeNode>
            <asp:TreeNode Text="班级平均成绩" Value="班级平均成绩">
                <asp:TreeNode Text="大班" Value="大班">
                    <asp:TreeNode Text="1班" Value="1班"></asp:TreeNode>
                    <asp:TreeNode Text="2班" Value="2班"></asp:TreeNode>
                </asp:TreeNode>
                <asp:TreeNode Text="临时班" Value="临时班"></asp:TreeNode>
            </asp:TreeNode>
        </Nodes>
    </asp:TreeView>
    </form>
</body>
</html>
