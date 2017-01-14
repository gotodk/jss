<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FWSList.aspx.cs" Inherits="Web_FWSList" %>
<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radg" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>服务商列表</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
</head>
<script src="../js/jquery.js" type="text/javascript"></script>
<body>
    <form id="form1" runat="server">
    <table border="0" cellpadding="5" cellspacing="0" class="FormView" width="680">
        <tr>
            <td height="25" width="50%">
                服务商编号：<asp:TextBox ID="fwsbh" runat="server" CssClass="input1"></asp:TextBox>
            </td>            <td height="25" width="50%">
                服务商名称：<asp:TextBox ID="fwsmc" runat="server" MaxLength="10" 
                    CssClass="input1" Height="18px" Width="124px"></asp:TextBox>
            </td>
        </tr>
              <tr>
                  <td align="right" colspan="2">
                      <asp:Button ID="EditButton" runat="server" CssClass="Button"
                          Text="搜索" OnClick="EditButton_Click" Width="40px" />
                      </td>
              </tr>
          </table>
     <radg:radgrid id="RadGrid1" runat="server" allowpaging="True" gridlines="None" skin="Monochrome" 
        PageSize="10" OnPageIndexChanged="RadGrid1_PageIndexChanged" DataSourceID="SqlDataSource1">
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
                           <a href='#' onclick="setValue('<%#Eval("number")%>','<%#Eval("KHMC")%>','<%#Eval("DZ")%>','<%#Eval("LXDH")%>','<%#Eval("Sheng_str")%>','<%#Eval("Shi_str")%>','<%#Eval("QuXian_str")%>');"><img  alt=""  src="../images/jpg/Update.gif" /></a>
                        </ItemTemplate>
                        <HeaderTemplate>
                            选择操作
                        </HeaderTemplate>
                             <ItemStyle CssClass="selectButton" />
                       </radg:GridTemplateColumn>
                        <radg:GridBoundColumn DataField="Number" HeaderText="服务商编号" ReadOnly="True" SortExpression="Number"
                            UniqueName="Number">
                        </radg:GridBoundColumn>
                        <radg:GridBoundColumn DataField="KHMC" HeaderText="服务商名称" SortExpression="KHMC" UniqueName="KHMC">
                        </radg:GridBoundColumn>
                        <radg:GridBoundColumn DataField="DZ" HeaderText="服务商地址" SortExpression="DZ" UniqueName="DZ">
                        </radg:GridBoundColumn>
                        <radg:GridBoundColumn DataField="LXDH" HeaderText="联系电话" SortExpression="LXDH" UniqueName="LXDH">
                        </radg:GridBoundColumn>
                        <radg:GridBoundColumn DataField="Sheng_str" HeaderText="所在省" SortExpression="Sheng_str" UniqueName="Sheng_str">
                        </radg:GridBoundColumn>
                        <radg:GridBoundColumn DataField="Shi_str" HeaderText="所在市" SortExpression="Shi_str" UniqueName="Shi_str">
                        </radg:GridBoundColumn>
                        <radg:GridBoundColumn DataField="QuXian_str" HeaderText="所在区县" SortExpression="QuXian_str" UniqueName="QuXian_str">
                        </radg:GridBoundColumn>                      
                    </Columns>
                </MasterTableView>
    </radg:radgrid><asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:FMOPConn %>"
            SelectCommand="SELECT Number,KHMC, DZ,LXDH,Sheng_str,Shi_str,QuXian_str from KHGL_New where XSQD='服务商'"></asp:SqlDataSource>
    </form>
    <script type="text/javascript">
        function setValue(fwsbh,khmc,dz,lxdh,sheng,shi,quxian) {
            if (parent.opener.document.getElementById("FWSBH") != null) {
                parent.opener.document.getElementById("FWSBH").value = fwsbh;
            }
            if (parent.opener.document.getElementById("FWSMC") != null) {
                parent.opener.document.getElementById("FWSMC").value = khmc;
            }
            if (parent.opener.document.getElementById("FWSDZ") != null) {
                parent.opener.document.getElementById("FWSDZ").value =dz;
            }
            if (parent.opener.document.getElementById("LXDH") != null) {
                parent.opener.document.getElementById("LXDH").value = lxdh;
            }
            if (parent.opener.document.getElementById("SZSF") != null) {
                parent.opener.document.getElementById("SZSF").value =sheng;
            }
            if (parent.opener.document.getElementById("SZCS") != null) {
                parent.opener.document.getElementById("SZCS").value = shi;
            }
            if (parent.opener.document.getElementById("SZQX") != null) {
                parent.opener.document.getElementById("SZQX").value =quxian;
            }           
            window.close();
            
        }
    </script>
</body>
</html>
