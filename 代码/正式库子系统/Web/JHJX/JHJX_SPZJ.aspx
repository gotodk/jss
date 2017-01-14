<%@ Page Language="C#" AutoEventWireup="true" CodeFile="JHJX_SPZJ.aspx.cs" Inherits="Web_JHJX_JHJX_SPZJ" %>

<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>    
    <link href="../../css/style.css" rel="Stylesheet" type="text/css" />
    <script src="../../js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../../js/standardJSFile/art_confirm.js" type="text/javascript"></script>
    <script src="../../js/standardJSFile/jquery.art_confirm.js" type="text/javascript"></script>
    <link href="../../css/standardStyle.css" rel="stylesheet" type="text/css" />
    <script src="../../js/standardJSFile/jquery.tablechangecolor.js" type="text/javascript"></script>    
   
    <style type="text/css">
        #theObjTable {
            width: 666px;
        }
    </style>
</head>
<body style=" background-color:#f7f7f7;">

    <form id="form1" runat="server">

        <radTS:RadTabStrip ID="TabStrip1" runat="server" CausesValidation="False" ReorderTabRows="True"
                Skin="Default2006" Width="99%" BackColor="#f7f7f7">
                <Tabs>
                    <radTS:Tab ID="Tab1" runat="server" NavigateUrl="JHJX_SPZJ.aspx" Text="商品增加"
                        ForeColor="Red" Font-Size="12px">
                    </radTS:Tab>
                </Tabs>
            </radTS:RadTabStrip>
        <div id="new_content">
        <div id="new_zicontent">            
            <div id="content_zw">                
                <div class="content_bz">
                    1、该模块用于添加新的商品条目。<br />
                  
                </div>
                <div class="content_lx" style="width:700px">
                </div>
                <div class="content_nr">
                    <table width="700px" class="Message">
                        <thead>
                            <tr>
                                <th class="TitleTh">
                                    基本信息
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                           <tr>
                                <td style=" width:50%;">
                                    <table width="100%">
                                        <tr>
                                            <td style=" width:10px; text-align:right;">
                                                <span class="span">*</span>
                                            </td>
                                            <td style=" width:70px;overflow:hidden; text-align:right;">
                                                所属分类：
                                            </td>
                                            <td  style="text-align:left;">
                                                <asp:DropDownList ID="ddlSSFL" runat="server" Width="199px"  CssClass="tj_input" OnSelectedIndexChanged="ddlSSFL_SelectedIndexChanged" AutoPostBack="true">
                                                    <asp:ListItem Text="所属分类" Value="0"></asp:ListItem>                                                    
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style=" width:50%;">
                            
                                </td>                            
                           </tr>
                           <tr>
                                <td style=" width:50%;">
                                    <table width="100%">
                                        <tr>
                                            <td style=" width:10px; text-align:right;">
                                                <span class="span">*</span>
                                            </td>
                                            <td style=" width:70px;overflow:hidden; text-align:right;">
                                                商品名称：
                                            </td>
                                            <td  style="text-align:left;">
                                                <asp:TextBox ID="txtSPMC" runat="server" CssClass="tj_input" Width="200px" Height="24px" Enabled="True" MaxLength="20" 
                                   ></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style=" width:50%;">
                                    <table width="100%">
                                        <tr>
                                            <td style=" width:20px; text-align:right;">
                                                <span class="span">*</span>
                                            </td>
                                            <td style=" width:90px;overflow:hidden; text-align:right;">
                                                规格：
                                            </td>
                                            <td  style="text-align:left;">
                                                <asp:TextBox ID="txtSPGG" runat="server" CssClass="tj_input" Width="200px" Height="24px" Enabled="True" MaxLength="24" 
                                   ></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>                            
                           </tr>
                           <tr>
                                <td style=" width:50%;">
                                    <table width="100%">
                                        <tr>
                                            <td style=" width:10px; text-align:right;">
                                                <span class="span">*</span>
                                            </td>
                                            <td style=" width:70px;overflow:hidden; text-align:right;">
                                                计价单位：
                                            </td>
                                            <td  style="text-align:left;">
                                                <asp:TextBox ID="txtJJDW" runat="server" CssClass="tj_input" Width="200px" Height="24px" Enabled="True" MaxLength="10"
                                    ></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style=" width:50%;">
                                    <table width="100%">
                                        <tr>
                                            <td style=" width:20px; text-align:right;">
                                                <span class="span">*</span>
                                            </td>
                                            <td style=" width:90px;overflow:hidden; text-align:right;">
                                                最大经济批量：
                                            </td>
                                            <td  style="text-align:left;">
                                                <asp:TextBox ID="txtJJPL" runat="server" CssClass="tj_input" Width="200px" Height="24px" Enabled="True" Style="ime-mode: Disabled;"
                            onKeypress="return (/[\d.]/.test(String.fromCharCode(event.keyCode)))" MaxLength="10" 
                           
                                    ></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>                            
                           </tr>
                           <tr>
                               <td colspan="2">
                                   <table>
                                       <tr>
                                           <td style=" width:10px; text-align:right; vertical-align:top ">
                                                <span class="span">*</span>
                                            </td>
                                            <td style=" width:70px;overflow:hidden; text-align:right; vertical-align:top ">
                                                商品描述：
                                            </td>
                                            <td  style="text-align:left;">
                                                <asp:TextBox ID="txtSPMS" runat="server" CssClass="tj_input" Width="578px" Enabled="True"
                                     Height="66px" MaxLength="300" TextMode="MultiLine" ></asp:TextBox>
                                            </td>
                                       </tr>
                                   </table>
                               </td>            
                          </tr>                            
                        </tbody>
                        <tfoot>
                            <tr>
                                <td colspan="2"  style="line-height:1px;">
                                    </td>
                            </tr>
                            <tr>
                                <td colspan="2" class="TitleTh">
                                    资质要求<span style="color:red;">（资质名称不能带有“|”）</span></td>
                            </tr>
                        </tfoot>
                    </table>        
                    <table width="649px" cellspacing="0" cellpadding="0" border="1" bordercolor="#99BBE8"
                        style="border-collapse: collapse;" class="tab">   
                        <tbody>
                            <tr>
                                <td>                                                                                                                                            <table cellspacing="0" cellpadding="0" id="theObjTable">       
                                        <thead>   
                                                                                                                                                                                        <tr>
                                                       
                                                        <th class="TheadTh" style=" width:10%">
                                                            资质名称</th>
                                                        <th class="TheadTh" style=" width:5%">
                                            操作                                            
                                            </th>
                                            </tr>
                                             <tr  class="TbodyTrAdd" style="height:35px; vertical-align:middle;">   
                             
                                                <td>
                                                    <asp:TextBox ID="txtZZMC" runat="server" Width="232px" Height="24px"  CssClass="tj_input_dj"   style=" text-align:center;" MaxLength="50"></asp:TextBox>
                                                </td>
                                                <td>
                                                <asp:Button ID="btnAdd" runat="server" CssClass="tj_bt" Text="添加" 
                                                        UseSubmitBehavior="False" onclick="btnAdd_Click" />
                                                </td>
                                            </tr>
                                         </thead>     
                                        <tbody>     
                                                                                                                                                                                      <asp:Repeater ID="Repeater1" runat="server" onitemcommand="Repeater1_ItemCommand">
                                            <ItemTemplate>                                            
                                               <tr  class="TbodyTr">   
                                                    
                                                    <td>
                                                        <%#Eval("ZZMC")%>
                                                    </td>
                                                    <td>
                                                    <asp:Button ID="BtnDelete" runat="server" CssClass="tj_bt" Text="删除"  CommandName="Delete" UseSubmitBehavior="False" OnClientClick="if(!confirm('确定执行删除操作？'))return false;"/>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                         </tbody>
                                        <tfoot>
                                            <tr id="ts" runat="server" class="TfootTr">
                                                <td colspan="2" align="right">
                                                信息填写完整后，请点击添加按钮。
                                                </td>                                                                                                                                </tr>                                                                                                                                        <tr id="hj" runat="server" class="TfootTr" style=" text-align:center; " visible="false">
                                            
                                            <td >
                                                合计：总共<span id="hjspan" runat="server">0</span> 项资质
                                            </td>
                                            <td> &nbsp; </td>                                     
                                           
                                           
                                        </tr>
                                         </tfoot>
                                    </table>
                                </td>
                            </tr> 
                        </tbody>                           
                    </table>
                    <br />                  
     
                    <br />
                    <table width="700px" class="Message">
                        <tbody>
                           <tr>
                                <td colspan="2">
                                    <table width="100%">
                                        <tr>
                                            <td style=" width:10px; text-align:left; vertical-align:top;">
                                                
                                            </td>
                                            <td style=" width:180px;overflow:hidden; text-align:right; vertical-align:top;">
                                                
                                            </td>
                                            <td  style="text-align:left; vertical-align:top;">
                                                <asp:Button ID="btnSave" UseSubmitBehavior="False" runat="server" CssClass="tj_bt_da"  Text="提交"  Height="30" Width="70" OnClick="btnSave_Click" />
                                                &nbsp;&nbsp;
                                                <asp:Button ID="btnCancle" UseSubmitBehavior="False" runat="server" CssClass="tj_bt_da" Text="取消" Height="30" Width="70" OnClick="btnCancle_Click" />
                                                &nbsp;&nbsp;
                                                </td>
                                        </tr>
                                    </table>
                                </td>                          
                           </tr>
                        </tbody>
                    </table>

                </div>
            </div>
        </div>        
    </div>
    </form>
</body>
</html>
