<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SPFLGL.aspx.cs" Inherits="Web_JHJX_SPFLGL" %>


<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link href="../../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../../css/standardStyle.css" rel="stylesheet" type="text/css" />
    <script src="../jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../../js/standardJSFile/jquery.art_confirm.js" type="text/javascript"></script>
    <%--<script src="../../js/fcf.js" type="text/javascript"></script>--%>
    <script src="../../js/standardJSFile/jquery.tablechangecolor.js" type="text/javascript"></script>   
     <script type="text/javascript" language="javascript">
         //浮点数
         function CheckInputIntFloat(oInput) {
             if ('' != oInput.value.replace(/\d{1,}\.{0,1}\d{0,2}/, '')) {
                 oInput.value = oInput.value.match(/\d{1,}\.{0,1}\d{0,2}/) == null ? '' : oInput.value.match(/\d{1,}\.{0,1}\d{0,2}/);
             }
         }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <radTS:RadTabStrip ID="TabStrip1" runat="server" CausesValidation="False" ReorderTabRows="True"
        Skin="Default2006" Width="950px" OnTabClick="TabStrip1_TabClick">
        <Tabs>
            <radTS:Tab ID="Tab1" runat="server" NavigateUrl="SPFLGL.aspx" Text="商品分类管理"
                ForeColor="Red">
            </radTS:Tab>
        </Tabs>
    </radTS:RadTabStrip>
    <div id="new_content">
        <div id="new_zicontent">
            <div id="content_zw"> 
                 <div class="content_bz">
                    1、添加分类时，请在分类名称文本框输入添加的名称，选择所属父类，之后点击“添加”按钮。<br />                
                    2、对分类进行排序时，先点击“编辑”按钮，之后输入相应顺序，最后点击“保存”按钮即可。<br />
                    3、注意排序影响的是交易平台大盘中商品分类在商品排序中的权重，而非商品分类自身在下拉框中的显示。<br />
                 

                  
    </div>
    <div class="content_lx" style="width: 700px">
                    </div>
    <div style="font-size: 14px; font-family: 宋体; font-weight: bold; height: 25px; vertical-align: middle;">
            <b><span>添加分类</span></b>

    </div>
    <div>
        <table width="700px" class="Message">                      
                        <tbody>
                            <tr>
                                <td style="width: 100px; text-align: right; height: 25px">
                                    分类名称：
                                </td>
                                <td style="text-align: left; width: 150px">
                                    
                            <asp:TextBox ID="txtflmc" runat="server" Width="150px" CssClass="tj_input"  ></asp:TextBox>  
                                  
                                </td>
                                <td  style="text-align: right; width: 100px">
                                    所属父类：</td>
                                <td style="text-align: left;">
                               
                                    
                            <asp:DropDownList ID="ddlssfl" runat="server" CssClass="tj_input" Width="150px"   >
                                     <asp:ListItem Text="所属分类" Value="0"></asp:ListItem>     
                                </asp:DropDownList>                                   
                           
                             
                                </td>
                                 <td style="text-align:right">
                                    <asp:Button ID="btnqd" runat="server" CssClass="tj_bt" UseSubmitBehavior="False"
                                    Text="添加"   Width="60px" OnClick="btnqd_Click"  Enabled="false"/>
                                     <asp:Button ID="btnxg" runat="server" CssClass="tj_bt" UseSubmitBehavior="False"
                                    Text="修改父类"   Width="60px" OnClick="btnxg_Click"   Visible="false" />
                                </td>
                            </tr>
                            </tbody>
                        </table>
    </div>
    <div class="content_lx" style="width: 700px">
                    </div>
    <div style="font-size: 14px; font-family: 宋体; font-weight: bold; height: 25px; vertical-align: middle;">
            <b><span>分类排序</span></b>

    </div>
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" style="font-size: 12px;
            font-family: 宋体; word-break: break-all; border-collapse: collapse; display:none" bordercolor="#D4D4D4">
            <tr>
                <td style="width:100px"></td>
                <td >
                   <asp:ListBox ID="listsecond" runat="server" Height="114px" Width="192px"></asp:ListBox> 
                </td>
               
            </tr>  
            <tr>
                <td style="width:100px"></td>
                <td>
                    <asp:Button ID="btninitial" CssClass="tj_bt" runat="server"  Text="默认" OnClick="btninitial_Click" />
                &nbsp;<asp:Button ID="btntop" CssClass="tj_bt" runat="server"  Text=" 上移" OnClick="btntop_Click" />
        
                    &nbsp;<asp:Button ID="btnbottom" CssClass="tj_bt" runat="server"  Text="下移" OnClick="btnbottom_Click" />
                </td>
            </tr>        
           
        </table>
        <table  style="width:700px">
            <tr>
                <td style="width:100px"></td>
                <td style="text-align:right">
                    <asp:Button ID="btnSort" runat="server" Text="编辑" CssClass="tj_bt" OnClick="btnSort_Click" Width="60px"/></td>
            </tr>
        </table>
        <asp:Repeater ID="Repeater1" runat="server" OnItemDataBound="Repeater1_ItemDataBound">
            <HeaderTemplate>
                <table border="1px" id="tababc" style="background: url(/Web/images/standardImageFile/bg.gif) repeat-x 0 0;
                    border-style: solid; border-color: #D4D4D4; border-collapse: collapse; text-align: center;
                    font-size: 12px;" rules="all" align="left" cellpadding="0" cellspacing="0"
                    width="700px">
                    <thead>
                        <tr style="height: 27px">
                           
                            <td runat="server" id="xssx">
                                显示顺序
                            </td>
                            <td>
                                分类名称
                            </td>
                            <td >
                                上级分类
                            </td>
                         
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr style="text-align: center; height: 30px;">
                   
                    <td runat="server" id="tbsx">
                        <asp:TextBox runat="server" ID="txtsx" Text='<%# Bind("SortOrder") %>' ReadOnly="True"
                            BorderWidth="0px" Width="50px" Style="text-align: center; ime-mode: Disabled;"
                           onKeypress="return (/[\d]/.test(String.fromCharCode(event.keyCode)))"
                            onkeyup="javascript:CheckInputIntFloat(this);" onbeforepaste="clipboardData.setData('text',clipboardData.getData('text').replace(/[^\d]\.?/g,''))" />
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lbname" Text='<%# Bind("SortName") %>' />
                    </td>
                    <td >
                        <asp:Label runat="server" ID="lbpath" Text='<%# Bind("SortParentPath") %>' />
                    </td>
                    
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </tbody><tfoot>
                    <tr style="height: 27px;" class="TfootTr">
                       
                        <td style="text-align:center" colspan="3">
                             合计：<asp:Label runat="server" ID="slhj" />
                        </td>
                        
                    </tr>
                </tfoot>
                </table>
            </FooterTemplate>
        </asp:Repeater>
            </div>
        </div>
    </div>
   
        
        
    </form>
</body>
</html>
