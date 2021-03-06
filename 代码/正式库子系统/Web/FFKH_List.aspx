﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FFKH_List.aspx.cs" Inherits="Web_FFKH_List" %>
<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radg" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>客户列表</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
</head>
<script src="../js/jquery.js" type="text/javascript"></script>
<body>
    <form id="form1" runat="server">
    <table border="0" cellpadding="5" cellspacing="0" class="FormView" width="680">
        <tr>
            <td height="25" width="50%">
                客户编号：<asp:TextBox ID="khbh" runat="server" CssClass="input1"></asp:TextBox>
            </td>            <td height="25" width="50%">
                客户名称：<asp:TextBox ID="khmc" runat="server" MaxLength="10" 
                    CssClass="input1" Height="18px" Width="124px"></asp:TextBox>
            </td>
        </tr>
              <tr>
                  <td align="right" colspan="2">
                      <asp:Button ID="SearchButton" runat="server" CssClass="Button"
                          Text="搜索" OnClick="SearchButton_Click" Width="40px" />
                      </td>
              </tr>
          </table>
     <radg:radgrid id="RadGrid1" runat="server" allowpaging="True" gridlines="None" skin="Monochrome" 
        PageSize="15" OnPageIndexChanged="RadGrid1_PageIndexChanged" DataSourceID="SqlDataSource1">
        <HeaderStyle Height="28px"></HeaderStyle>
                <ExportSettings>
                <Pdf PageWidth="8.5in" PageRightMargin="" PageFooterMargin="" PageLeftMargin="" PageTopMargin="" PageHeight="11in" PageBottomMargin="" PageHeaderMargin=""></Pdf>
                </ExportSettings>
                <PagerStyle NextPageText="下一页" HorizontalAlign="Right" PagerTextFormat=" {4} &amp;nbsp;|&amp;nbsp; 当前页 {0} / {1}, 当前记录{2} 到 {3} 共 {5}条." PrevPageText="上一页"></PagerStyle>
                <MasterTableView AutoGenerateColumns="False" DataKeyNames="Number" DataSourceID="SqlDataSource1">
                <NoRecordsTemplate>
                  没有找到任何数据。             
                </NoRecordsTemplate>
                <ExpandCollapseColumn Visible="False" Resizable="False">
                <HeaderStyle Width="20px"></HeaderStyle>
                </ExpandCollapseColumn>
                <RowIndicatorColumn Visible="False">
                <HeaderStyle Width="20px"></HeaderStyle>
                </RowIndicatorColumn>
                    <Columns>
                         <radg:GridTemplateColumn UniqueName="TemplateColumn" AllowFiltering="False">
                        <ItemTemplate>
                           <a href='#' onclick="setValue('<%#Eval("Number")%>','<%#Eval("KHMC")%>','<%#Eval("SSXSQY")%>');"><img  alt=""  src="../images/jpg/Update.gif" /></a>
                        </ItemTemplate>
                        <HeaderTemplate>
                            选择操作
                        </HeaderTemplate>
                             <ItemStyle CssClass="selectButton" />
                       </radg:GridTemplateColumn>
                        <radg:GridBoundColumn DataField="Number" HeaderText="客户编号" ReadOnly="True" SortExpression="khbh"  UniqueName="khbh"> 
                        </radg:GridBoundColumn>
                        <radg:GridBoundColumn DataField="KHMC" HeaderText="客户名称" SortExpression="khmc" UniqueName="khmc">
                        </radg:GridBoundColumn>
                        <radg:GridBoundColumn DataField="SSXSQY" HeaderText="所属销售区域" SortExpression="ssxsqy" UniqueName="ssxsqy">
                        </radg:GridBoundColumn>

                    </Columns>
                </MasterTableView>
    </radg:radgrid><asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:FMOPConn %>"
            SelectCommand="SELECT Number, KHMC,SSXSQY FROM KHGL_New where 1!=1"></asp:SqlDataSource>
    </form>
    <script type="text/javascript">
        function setValue(khbh,khmc,ssxsqy) {
            if (parent.opener.document.getElementById("KHBH") != null) {
                parent.opener.document.getElementById("KHBH").value =khbh;
            }
            if (parent.opener.document.getElementById("KHMC") != null) {
                parent.opener.document.getElementById("KHMC").value = khmc;
            }
            if (parent.opener.document.getElementById("SSXSQY") != null) {
                parent.opener.document.getElementById("SSXSQY").value =ssxsqy;
            }
            window.close();
            
        }
    </script>
</body>
</html>

