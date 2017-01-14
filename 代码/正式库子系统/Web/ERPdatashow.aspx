<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ERPdatashow.aspx.cs" Inherits="Web_ERPdatashow" %>

<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radg" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>销售单列表</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style2
        {
            height: 215px;
        }
    </style>
</head>
<script src="../js/jquery.js" type="text/javascript"></script>
<body>
    <form id="form1" runat="server">
   <asp:Panel ID="Panal1" runat="server" Width="100%" ScrollBars="Both" Height="100%" >
    <table border="0" cellpadding="5" cellspacing="0" class="FormView" width="780px" style="height:95%">
        <tr>
            <td height="25" width="25%">
                单别：<asp:DropDownList ID="ddldanbie" runat="server" Visible="False">
                </asp:DropDownList>
                <asp:TextBox ID="txtdanbie" runat="server" CssClass="input1"></asp:TextBox>
                </td>
                <td width="25%">
                单号：<asp:DropDownList ID="ddldanhao" runat="server" Visible="False">
                </asp:DropDownList>
                    <asp:TextBox ID="txtdanhao" runat="server" CssClass="input1"></asp:TextBox>
            </td>       </tr>
              <tr>  
               <td height="25" width="25%">
                收货地址：<asp:TextBox ID="txtshouhuoAdress" runat="server" MaxLength="10" 
                    CssClass="input1" Height="18px" Width="124px" AutoPostBack="True"></asp:TextBox>
            </td>
            <td height="25" width="25%">
                所属区域：<asp:TextBox ID="txtArea" runat="server" MaxLength="10" 
                    CssClass="input1" Height="18px" Width="124px" AutoPostBack="True"></asp:TextBox>
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
        PageSize="8" OnPageIndexChanged="RadGrid1_PageIndexChanged">
        <HeaderStyle Height="28px"></HeaderStyle>
                <ExportSettings>
                <Pdf PageWidth="8.5in" PageRightMargin="" PageFooterMargin="" PageLeftMargin="" PageTopMargin="" PageHeight="11in" PageBottomMargin="" PageHeaderMargin=""></Pdf>
                </ExportSettings>
                <PagerStyle NextPageText="下一页" HorizontalAlign="Right" PagerTextFormat=" {4} &amp;nbsp;|&amp;nbsp; 当前页 {0} / {1}, 当前记录{2} 到 {3} 共 {5}条." PrevPageText="上一页"></PagerStyle>
                <MasterTableView AutoGenerateColumns="False" DataKeyNames="TG001">
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
                           <asp:ImageButton ID="btnChildTable" runat="server" 
                                ImageUrl="../images/jpg/Update.gif" onclick="btnChildTable_Click" />
                        </ItemTemplate>
                        <HeaderTemplate>
                            明细显示
                        </HeaderTemplate>
                             <ItemStyle CssClass="selectButton" />
                       </radg:GridTemplateColumn>
                        <radg:GridBoundColumn DataField="TG001" HeaderText="单别" ReadOnly="True" SortExpression="TG001"
                            UniqueName="TG001">
                        </radg:GridBoundColumn>
                        <radg:GridBoundColumn DataField="TG002" HeaderText="单号" SortExpression="KHMC" UniqueName="TG002">
                        </radg:GridBoundColumn>
                          <radg:GridTemplateColumn UniqueName="TemplateColumn" AllowFiltering="False">
                        <ItemTemplate>
                          <%# Convert.ToString(DataBinder.Eval(Container, "DataItem.TGADDRESS")).Length > 10 ? Convert.ToString(DataBinder.Eval(Container, "DataItem.TGADDRESS")).Substring(0, 10) + "....." : Convert.ToString(DataBinder.Eval(Container, "DataItem.TGADDRESS"))%>
                        </ItemTemplate>
                        <HeaderTemplate>
                            收货人地址
                        </HeaderTemplate>
                </radg:GridTemplateColumn>
                         <radg:GridBoundColumn DataField="MA003" HeaderText="所属区域" SortExpression="SZDWXZ"
                            UniqueName="TGADDRESS">
                        </radg:GridBoundColumn>
                        <radg:GridBoundColumn DataField="TG020" HeaderText="备注">
                        </radg:GridBoundColumn>
                        
                    </Columns>
                </MasterTableView>
    </radg:radgrid>
         </asp:Panel>
        
            <div id="DepartmentLine" style="height:30px"></div>
            <div id = "divChildTable" visible="false" runat="server" style="width:100%">
               <table width="100%" class="style2">
                  <tr>
                    <td>
                      <input id="btnSave" onclick="SetParentValue();" type="button" value="确定" /><input id="btnCancel" type="button" onclick="window.close()"value="取消" />
                    </td>
                 </tr>
                 <tr>
                   <td>
                    
                <asp:Panel ID="palChild" ScrollBars="Both" runat="server" Height="320PX" 
                           EnableViewState="False">
               <radg:radgrid id="RadGrid2" runat="server" gridlines="None" skin="Monochrome" 
        PageSize="8" OnPageIndexChanged="RadGrid1_PageIndexChanged" Width="98%">
        <HeaderStyle Height="28px"></HeaderStyle>
                <ExportSettings>
                <Pdf PageWidth="8.5in" PageRightMargin="" PageFooterMargin="" PageLeftMargin="" PageTopMargin="" PageHeight="11in" PageBottomMargin="" PageHeaderMargin=""></Pdf>
                </ExportSettings>
                <PagerStyle NextPageText="下一页" HorizontalAlign="Right" PagerTextFormat=" {4} &amp;nbsp;|&amp;nbsp; 当前页 {0} / {1}, 当前记录{2} 到 {3} 共 {5}条." PrevPageText="上一页"></PagerStyle>
                <MasterTableView AutoGenerateColumns="False">
                <NoRecordsTemplate>
                  没有找到任何数据。</NoRecordsTemplate>
                <ExpandCollapseColumn Visible="False" Resizable="False">
                <HeaderStyle Width="20px"></HeaderStyle>
                </ExpandCollapseColumn>
                <RowIndicatorColumn Visible="False">
                <HeaderStyle Width="20px"></HeaderStyle>
                </RowIndicatorColumn>
                    <Columns>
                       <radg:GridBoundColumn DataField="TH017" HeaderText="批次" >
                       </radg:GridBoundColumn>
                        <radg:GridBoundColumn DataField="XIAOHUODANHAO" HeaderText="销货单号" ReadOnly="True" 
                            UniqueName="TG001">
                        </radg:GridBoundColumn>
                       <%-- <radg:GridBoundColumn DataField="TH002" HeaderText="单号" SortExpression="KHMC">
                        </radg:GridBoundColumn>--%>
                      <%--  <radg:GridBoundColumn DataField="TGADDRESS" HeaderText="收货地址" 
                            UniqueName="TGADDRESS">
                        </radg:GridBoundColumn>--%>
                         <radg:GridBoundColumn DataField="TH004" HeaderText="品号">
                        </radg:GridBoundColumn>
                         <radg:GridBoundColumn DataField="TH005" HeaderText="产品名称" >
                        </radg:GridBoundColumn>
                        
                           <radg:GridBoundColumn DataField="TH006" HeaderText="规格">
                        </radg:GridBoundColumn>
                           <radg:GridBoundColumn DataField="TH009" HeaderText="单位" >
                        </radg:GridBoundColumn>
                        
                              <radg:GridBoundColumn DataField="XHSL" HeaderText="销售数量">
                        </radg:GridBoundColumn>
                        <radg:GridBoundColumn DataField="TGADDRESS" HeaderText="收货地址" SortExpression="SZDWXZ"
                            UniqueName="TGADDRESS">
                        </radg:GridBoundColumn>
                            
                        <radg:GridBoundColumn DataField="MA003" HeaderText="所属区域">
                        </radg:GridBoundColumn>
                         <radg:GridBoundColumn DataField="TH012" HeaderText="单价" >
                        </radg:GridBoundColumn>
                         <radg:GridBoundColumn DataField="TH013" HeaderText="金额" >
                        </radg:GridBoundColumn>
                         <radg:GridBoundColumn DataField="TG020" HeaderText="备注" >
                        </radg:GridBoundColumn>
                       
                        
                    </Columns>
                </MasterTableView>
    </radg:radgrid>
    </asp:Panel>
     <input runat="server" id="hiddanbie" type="hidden" /><input id="hiddanhao" type="hidden" runat="server" /><input id="hidshadress" runat="server" type="hidden" />
            </td>
                 </tr>
                
               </table>
               
            </div>
        
             
         <%--   </asp:Panel>--%>
    </form>
    <script type="text/javascript">
//        function setValue(bh,khmc) {
//            
//            if (parent.opener.document.getElementById("CSZX_CPYXQJHB_JHLBKHBH") != null) {
//                parent.opener.document.getElementById("CSZX_CPYXQJHB_JHLBKHBH").value =bh;
//            }
//            if (parent.opener.document.getElementById("CSZX_CPYXQJHB_JHLBKHMC") != null) {
//                parent.opener.document.getElementById("CSZX_CPYXQJHB_JHLBKHMC").value = khmc;
//            }
//            window.close();
//            
//        }
        
        function SetParentValue()
        {
              
                var tab = document.getElementById('RadGrid2_ctl01'); 
                
                for(var i=1;i < tab.rows.length;i++) 
                { 
                  if(parent.opener.document.getElementById("SHRDZ") != null)
                   {
                      if(parent.opener.document.getElementById("SHRDZ").value == "" || parent.opener.document.getElementById("SHRDZ").value == tab.rows[i].cells[7].innerText)
                      {
                      
                        parent.opener.document.getElementById("SHRDZ").value = tab.rows[i].cells[7].innerText;
                      }
                      else
                      {
                        alert("对不起，收货地址不同不允许添加在同一发货单中") ;
                        return ;
                      }
                   }
                   
                  if(parent.opener.document.getElementById("SSQY") != null)
                  {
                     if(parent.opener.document.getElementById("SSQY").value == "" || parent.opener.document.getElementById("SSQY").value == tab.rows[i].cells[8].innerText)
                      { 
                      
                        parent.opener.document.getElementById("SSQY").value = tab.rows[i].cells[8].innerText;
                      }
                      else
                      {
                       alert("对不起，所属区域不同不允许添加在同一发货单中") ;
                       return ;  
                      }
                  }
                  if (parent.opener.document.getElementById("XHDCPLBCPPH") != null)
                   {     
                     parent.opener.document.getElementById("XHDCPLBCPPH").value = tab.rows[i].cells[0].innerText;
  
                   }
                   if(parent.opener.document.getElementById("DQSRXHDH") != null && parent.opener.document.getElementById("XHDCPLBXHDH") != null)
                   {
                     parent.opener.document.getElementById("DQSRXHDH").value = tab.rows[i].cells[1].innerText;
                     parent.opener.document.getElementById("XHDCPLBXHDH").value = tab.rows[i].cells[1].innerText;
                   }
                   if(parent.opener.document.getElementById("XHDCPLBPH") != null)
                   {
                     parent.opener.document.getElementById("XHDCPLBPH").value = tab.rows[i].cells[2].innerText ;
                   }
                      if(parent.opener.document.getElementById("XHDCPLBCPMC") != null)
                   {
                     parent.opener.document.getElementById("XHDCPLBCPMC").value = tab.rows[i].cells[3].innerText;
                   }
                      if(parent.opener.document.getElementById("XHDCPLBGG") != null)
                   {
                     parent.opener.document.getElementById("XHDCPLBGG").value = tab.rows[i].cells[4].innerText;
                   }
                      if(parent.opener.document.getElementById("XHDCPLBDW") != null)
                   {
                     parent.opener.document.getElementById("XHDCPLBDW").value = tab.rows[i].cells[5].innerText;
                   }
                      if(parent.opener.document.getElementById("XHDCPLBXHSL") != null)
                   {
                     parent.opener.document.getElementById("XHDCPLBXHSL").value = tab.rows[i].cells[6].innerText;
                   }
                     if(parent.opener.document.getElementById("XHDCPLBDJ") != null)
                   {
                     parent.opener.document.getElementById("XHDCPLBDJ").value = tab.rows[i].cells[9].innerText;
                   }
              
                   if(parent.opener.document.getElementById("XHDCPLBJE") != null)
                   {
                     parent.opener.document.getElementById("XHDCPLBJE").value = tab.rows[i].cells[10].innerText;
                   }
                     if(parent.opener.document.getElementById("DYYHSQDH") != null)
                   {
                     parent.opener.document.getElementById("DYYHSQDH").value = tab.rows[i].cells[11].innerText;
                   }
              
                  
                 
                  
                  SaveChildTable();
                  
                }
            
        } 
        
        
        function SaveChildTable()
        {
//          parent.opener.XHDCPLBaddTableRow('XHDCPLBXHDH,XHDCPLBPH,XHDCPLBCPMC,XHDCPLBGG,XHDCPLBDW,XHDCPLBXHSL,XHDCPLBJS','XHDCPLB','XHDCPLBXHDH:TextBox:False:::50.XHDCPLBPH:TextBox:False:::200.XHDCPLBCPMC:TextBox:False:::200.XHDCPLBGG:TextBox:False:::200.XHDCPLBDW:TextBox:False:::50.XHDCPLBXHSL:IntBox:False:::200.XHDCPLBJS:IntBox:False:::50');
               parent.opener.XHDCPLBaddTableRow('XHDCPLBXHDH,XHDCPLBPH,XHDCPLBCPMC,XHDCPLBGG,XHDCPLBDW,XHDCPLBXHSL,XHDCPLBDJ,XHDCPLBJE,XHDCPLBCPPH','XHDCPLB','XHDCPLBXHDH:TextBox:False:::50.XHDCPLBPH:TextBox:True:::200.XHDCPLBCPMC:TextBox:False:::200.XHDCPLBGG:TextBox:False:::200.XHDCPLBDW:TextBox:True:::50.XHDCPLBXHSL:IntBox:False:::200.XHDCPLBDJ:FloatBox:True:::50.XHDCPLBJE:FloatBox:True:::50.XHDCPLBCPPH:TextBox:True:::50');
        }
      
    </script>
</body>
</html>