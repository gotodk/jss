<%@ Page Language="C#" AutoEventWireup="true" CodeFile="showJKinfo.aspx.cs" Inherits="IPCadmin_showJKinfo" %>

<%@ Register Src="wuccaidan.ascx" TagName="wuccaidan" TagPrefix="uc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>IPC管理中心--接口和方法详情</title>
    <link href="css.css" type="text/css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <uc1:wuccaidan ID="wuccaidan1" runat="server" />
            <div>
                <br />
                <strong>接口详情：<a href="editJK.aspx?guid=<%=Request["JK_guid"].ToString() %>" target="_self">修改接口资料</a></strong>
                <table>
                    <tr>
                        <td>

                        </td>
                    </tr>
                </table>
                <div runat="server" id="showjk"></div>
                <br />
                <asp:GridView ID="GridView1" runat="server" BackColor="#CCCCCC" BorderColor="#999999" BorderStyle="Solid" BorderWidth="3px" CellPadding="4" CellSpacing="2" ForeColor="Black" HorizontalAlign="Left" OnRowDataBound="GridView1_RowDataBound" Width="100%" AutoGenerateColumns="False"  DataKeyNames="方法唯一标示">
                    <Columns>
                          <asp:TemplateField HeaderText="操作">
                            <ItemTemplate>
                                  <asp:LinkButton ID="lkbEdit" runat="server" CausesValidation="False" CommandArgument=<%# Eval("方法唯一标示") %>    OnClick="btnEdit_Click" Text="编辑"></asp:LinkButton>
                                  <asp:LinkButton ID="lkbSave" runat="server" CausesValidation="False" CommandArgument=<%# Eval("方法唯一标示") %>    OnClick="btnSave_Click" Text="保存" Visible="false"></asp:LinkButton>
                                  <asp:LinkButton ID="lkbCancel" runat="server" CausesValidation="False" CommandArgument=<%# Eval("方法唯一标示") %>    OnClick="btnCancel_Click" Text="取消" Visible="false"></asp:LinkButton> 
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="业务名称">
                            <ItemTemplate>
                                <asp:Label ID="lblFF_yewuname" runat="server" Text='<%#Eval("业务名称") %>'></asp:Label>
                                <asp:TextBox ID="FF_yewuname" runat="server" Text='<%# Eval("业务名称") %>' Visible="false"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="方法名">
                            <ItemTemplate>
                                <asp:Label ID="lblFF_name" runat="server" Text='<%#Eval("方法名") %>'></asp:Label>
                                <asp:TextBox ID="FF_name" runat="server" Text='<%# Eval("方法名") %>' Visible="false"></asp:TextBox>
                            </ItemTemplate>

                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="返回值类型">
                            <ItemTemplate>
                                <asp:Label ID="lblFF_retype" runat="server" Text='<%#Eval("返回值类型") %>'></asp:Label>
                                <asp:TextBox ID="FF_retype" runat="server" Text='<%# Eval("返回值类型") %>' Visible="false"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="参数类型">
                            <ItemTemplate>
                                <asp:Label ID="lblFF_canshu" runat="server" Text='<%#Eval("参数类型") %>'></asp:Label>
                                <asp:TextBox ID="FF_canshu" runat="server" Text='<%# Eval("参数类型") %>' Visible="false"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="方法注释">
                            <ItemTemplate>
                                <asp:Label ID="lblFF_shuoming" runat="server" Text='<%#Eval("方法注释") %>'></asp:Label>
                                <asp:TextBox ID="FF_shuoming" runat="server" Text='<%# Eval("方法注释") %>' Visible="false"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="方法是否有效">
                            <ItemTemplate>
                                <asp:Label ID="lblFF_open" runat="server" Text='<%#Eval("方法是否有效") %>'></asp:Label>
                                <asp:ListBox ID="FF_open" runat="server" Rows="1" Visible="false">
                                    <asp:ListItem  Value="1">有效</asp:ListItem>
                                    <asp:ListItem Value="0">禁用</asp:ListItem>
                                </asp:ListBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="操作特点">
                            <ItemTemplate>
                                <asp:Label ID="lblFF_CorE" runat="server" Text='<%#Eval("操作特点") %>'></asp:Label>
                                <asp:ListBox ID="FF_CorE" runat="server" Rows="1" Visible="false">
                                    <asp:ListItem Value="0">仅查询操作</asp:ListItem>
                                    <asp:ListItem Value="1">有插入或更新操作</asp:ListItem>
                                </asp:ListBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="日志设置">
                            <ItemTemplate>
                                <asp:Label ID="lblFF_Log" runat="server" Text='<%#Eval("日志设置") %>'></asp:Label>
                                <asp:CheckBoxList ID="FF_Log" runat="server" RepeatDirection="Horizontal" Visible="false">
                                    <asp:ListItem>程序</asp:ListItem>
                                    <asp:ListItem>业务</asp:ListItem>
                                    <asp:ListItem>其他</asp:ListItem>
                                </asp:CheckBoxList>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <FooterStyle BackColor="#CCCCCC" />
                    <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" HorizontalAlign="Left" />
                    <PagerStyle BackColor="#CCCCCC" ForeColor="Black" HorizontalAlign="Left" />
                    <RowStyle BackColor="White" HorizontalAlign="Left" />
                    <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                    <SortedAscendingHeaderStyle BackColor="Gray" />
                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                    <SortedDescendingHeaderStyle BackColor="#383838" />
                </asp:GridView>
            </div>
            <br />
            <br />
            <div>
                ...........................................
           <br />
                ...........................................
                <br />
                <strong>接口所在集群所有IP及目前健康情况：</strong>
                <br />
                <br />
                <div runat="server" id="Div1">后续实现，列出所有此集群内ip，检测该IP本机与IPC中心配置是否存在差异</div>
            </div>
        </div>
    </form>
</body>
</html>
