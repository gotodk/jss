<%@ Page Language="C#" AutoEventWireup="true" CodeFile="index.aspx.cs" Inherits="Web_tijiao_index" MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radg" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>提交类</title>
    <link href="../../css/style.css" rel="Stylesheet" type="text/css" />
//    <script src="../../js/adddate.js" type="text/javascript">        function igtbl_reOkBtn_onclick() {

       }

    </script>
    <style type="text/css">
        /*最外层div，宽100%，背景#f7f7f7*/
        #new_content
        {
            width: 100%;
            background-color: #f7f7f7;
        }
        /*内层div*/
        #new_zicontent
        {
            padding: 30px;
        }
        /*内容正文div，用于控制内容宽度，取值为500px、919px、1084px;*/
        #content_zw
        {
            width: 1084px;
        }
        /*页面备注内容*/
        .content_bz
        {
            font-size: 12px;
            line-height: 25px;
            color: Red;
        }
        /*分割线*/
        .content_lx
        {
            height: 1px;
            background-color: #a5cbe2;
            margin-top: 10px;
            margin-bottom: 10px;
        }
        .content_nr
        {
        }
        .tj_input
        {
            height: 20px;
            border-width: 1px;
            border-style: solid;
            border-color: #999999;
            line-height: 20px;
        }
        .tj_input:hover
        {
            border-color: #7e9cb6;
        }
        .tj_input_dj
        {
            height: 20px;
            border-width: 1px;
            border-style: solid;
            border-color: #AAAAAA;
            line-height: 20px;
        }
        .tj_bt
        {
            height: 22px;
            border-width: 1px;
            border-style: solid;
            border-color: #999999;
            background-image: url("huibg.jpg");
            line-height: 20px;
        }
        .tj_bt:hover
        {
            border-color: #7e9cb6;
            cursor: pointer;
            background: url("lanbg.jpg");
        }
        .tj_bt_da
        {
            height: 30px;
            border-width: 1px;
            border-style: solid;
            border-color: #999999;
            background-image: url("huibg2.jpg");
            line-height: 30px;
        }
        .tj_bt_da:hover
        {
            border-color: #7e9cb6;
            cursor: pointer;
            background: url("lanbg2.jpg");
        }
        .tj_input_search
        {
            background-image: url("search.jpg");
            background-position: right;
            background-repeat: no-repeat;
        }
        .tj_input_time
        {
            background-image: url("shijian.jpg");
            background-position: right;
            background-repeat: no-repeat;
        }
        
        .content_tab tr
        {
            height: 30px;
            line-height: 30px;
        }
        
     
        .tab
        {
        }
        
        .tab td
        {
            height: 26px;
            vertical-align: middle;
            line-height: 25px;
        }
        .bg
        {
            background-color: #D6E3F3;
        }
        .bt_bg
        {
            background: url(bg.gif) repeat-x 0 0;
        }
        .tou_table td
        {
            border-right: solid 1px #BEBCBF;
        }
        .nr_table td
        {
            border-bottom: solid 1px #DCDCDC;
            border-right: solid 1px #DCDCDC;
        }
        .nr_table tr:hover
        {
        	 background-color:#DFE8F7;        	       	  
        	}
        .wei_table td
        {
        	border-right: solid 1px #BEBCBF;
        	}
    </style>
    <style type="text/css">
        .TheadTh
        {
            background: url(bg.gif) repeat-x 0 0;
            border-right: solid 1px #BEBCBF;
            border-bottom :solid 1px #99BBE8;
        }
        .TbodyTrAdd td
        {            
            border-bottom: solid 1px #DCDCDC;
            border-right: solid 1px #DCDCDC;
            text-align:center;
        }
        .TbodyTr:hover
        {
            background-color:#DFE8F7;              
        }
         .TbodyTr td
         {
            border-bottom: solid 1px #DCDCDC;
            border-right: solid 1px #DCDCDC;
            text-align:center;
         }   
         .TfootTr
         {
            background-color: #D6E3F3;            
         }
         .TfootTr td
         {
             border-top:solid 1px #99BBE8;
             border-right: solid 1px #BEBCBF;
         }
    </style>
   
</head>
<body>
 <script src="../../js/jquery-1.7.2.min.js" type="text/javascript"></script>
    
    <form id="form1" runat="server">
    <radTS:RadTabStrip ID="RadTabStrip1" runat="server" Height="25px" Skin="Default2006"
        BackColor="#f7f7f7">
        <Tabs>
            <radTS:Tab ID="Tab1" runat="server" Text="所有会议室申请信息管理">
            </radTS:Tab>
        </Tabs>
    </radTS:RadTabStrip>
    <div id="new_content">
        <div id="new_zicontent">
            <div id="content_zw">
                <div class="content_bz">
                    1、该模块用于记录已签约服务商以个人名义打款的打款人信息以及与服务商的对应关系。<br />
                    2、保存时系统根据规则自动生成正式客户编号，打款人编号以"6"开头，用生成的编号录入ERP。<br />
                    3、“所属服务商编号”请填写本办事处销售渠道为“服务商”或“门店服务商”的客户编号。
                </div>
                <div class="content_lx">
                </div>
                <div class="content_nr">
                    <table width="600px" class="content_tab">
                        <tr>
                            <td align="right" width="100px;">
                                输入框:
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="TextBox1" runat="server" CssClass="tj_input" Width="200px" Enabled="True"
                                    TabIndex="1"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                输入框冻结:
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="TextBox9" runat="server" CssClass="tj_input_dj" Width="200px" Text="输入框冻结状态"
                                    Enabled="False"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                文本框:
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="TextBox2" runat="server" class="tj_input" Width="500px" Height="200px"
                                    TextMode="MultiLine"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                内容提交按钮:
                            </td>
                            <td colspan="3">
                                <asp:Button ID="Button4" runat="server" CssClass="tj_bt" Text="按钮" UseSubmitBehavior="False" />&nbsp;&nbsp;<asp:Button
                                    ID="Button3" CssClass="tj_bt" runat="server" Text="提交按钮" UseSubmitBehavior="False" />&nbsp;&nbsp;<asp:Button
                                        ID="Button5" CssClass="tj_bt" runat="server" Text="内容提交按钮" UseSubmitBehavior="False" />
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                列表框:
                            </td>
                            <td colspan="3">
                                <asp:DropDownList ID="DropDownList4" runat="server" Width="162px" CssClass="tj_input"
                                    Height="22px">
                                    <asp:ListItem>办公室</asp:ListItem>
                                    <asp:ListItem>人力资源部</asp:ListItem>
                                    <asp:ListItem>信息化中心</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                查找输入框:
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="TextBox7" runat="server" class="tj_input tj_input_search" Width="158px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" width="100px">
                                开始时间:
                            </td>
                            <td width="200px">
                                <asp:TextBox ID="TextBox4" runat="server" class="tj_input tj_input_time" Width="158px"
                                    onfocus="setday(this,document.all.TextBox4)"></asp:TextBox>
                            </td>
                            <td align="right" width="142px">
                                结束时间:
                            </td>
                            <td width="158px">
                                <asp:TextBox ID="TextBox3" runat="server" class="tj_input tj_input_time" Width="158px"
                                    onfocus="setday(this,document.all.TextBox4)"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                单选按钮:
                            </td>
                            <td colspan="3">
                                <asp:RadioButton ID="RadioButton1" runat="server" Text="单选按钮1" />&nbsp;&nbsp;<asp:RadioButton
                                    ID="RadioButton2" runat="server" Text="单选按钮2" />&nbsp;&nbsp;<asp:RadioButton ID="RadioButton3"
                                        runat="server" Text="单选按钮3" />
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                多选按钮:
                            </td>
                            <td colspan="3">
                                <asp:CheckBox ID="CheckBox1" runat="server" Text="多选按钮1" />&nbsp;&nbsp;<asp:CheckBox
                                    ID="CheckBox2" runat="server" Text="多选按钮1" />&nbsp;&nbsp;<asp:CheckBox ID="CheckBox3"
                                        runat="server" Text="多选按钮1" />
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                提示框:
                            </td>
                            <td colspan="3">
                                <asp:Button ID="Button2" runat="server" CssClass="tj_bt" Text="提醒提示框" UseSubmitBehavior="False" />&nbsp;&nbsp;<asp:Button
                                    ID="Button9" CssClass="tj_bt" runat="server" Text="警告提示框" UseSubmitBehavior="False" />&nbsp;&nbsp;<asp:Button
                                        ID="Button10" CssClass="tj_bt" runat="server" Text="错误提示框" UseSubmitBehavior="False" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="center" style="height: 80px;">
                                <asp:Button ID="Button1" runat="server" CssClass="tj_bt_da" UseSubmitBehavior="False"
                                    OnClick="BtnCheck_Click" Text="确认" Width="50px" />&nbsp;&nbsp;<asp:Button ID="Button6"
                                        UseSubmitBehavior="False" runat="server" CssClass="tj_bt_da" OnClick="BtnCheck_Click"
                                        Text="取消" Width="50px" />&nbsp;&nbsp;<asp:Button ID="Button7" UseSubmitBehavior="False"
                                            runat="server" CssClass="tj_bt_da" OnClick="BtnCheck_Click" Text="确认按钮" />&nbsp;&nbsp;<asp:Button
                                                ID="Button8" UseSubmitBehavior="False" runat="server" CssClass="tj_bt_da" OnClick="BtnCheck_Click"
                                                Text="取消按钮" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <table width="100%" cellspacing="0" cellpadding="0" border="1" bordercolor="#99BBE8"
                        style="border-collapse: collapse;" class="tab">                        
                        <tr>
                            <td colspan="3" class="bt_bg">
                                <table width="100%" cellspacing="0" cellpadding="0" class="tou_table">
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="nr_table">
                                    <tr>
                                        <td>
                                            ddd
                                        </td>
                                        <td>
                                            dd;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="bg">
                                <table width="100%" cellpadding="0" cellspacing="0" class="wei_table"><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr></table>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <table width="100%" cellspacing="0" cellpadding="0" border="1" bordercolor="#99BBE8"
                        style="border-collapse: collapse;" class="tab">
                        <tr>
                            <td class="bg">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" class="bt_bg">
                                <table width="100%" cellspacing="0" cellpadding="0" class="tou_table">
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="nr_table">
                                    <tr>
                                        <td>
                                            ddd
                                        </td>
                                        <td>
                                            dd;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="bg">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                    <br />
                    有"添加"、"删除按钮"
                    <table width="100%" cellspacing="0" cellpadding="0" border="1" bordercolor="#99BBE8"
                        style="border-collapse: collapse;" class="tab">   
                        <tr>
                            <td>
                                <table width="100%" cellspacing="0" cellpadding="0">                                   
                                    <thead>
                                        <tr>
                                            <th class="TheadTh" style=" width:10%">
                                            提货方式
                                            </th>
                                            <th class="TheadTh" style=" width:8%">
                                            硒鼓品类
                                            </th>
                                            <th class="TheadTh" style=" width:20%">
                                            硒鼓型号
                                            </th>
                                            <th class="TheadTh" style=" width:10%">
                                            提货价
                                            </th>
                                            <th class="TheadTh" style=" width:10%">
                                            押金
                                            </th>
                                            <th class="TheadTh" style=" width:7%">
                                            硒鼓数量
                                            </th>
                                            <th class="TheadTh" style=" width:10%">
                                            提货金合计
                                            </th>
                                            <th class="TheadTh" style=" width:10%">
                                            押金合计
                                            </th>
                                            <th class="TheadTh" style=" width:10%">
                                            交易金额
                                            </th>
                                            <th class="TheadTh" style=" width:5%">
                                            操作                                            
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr  class="TbodyTrAdd">
                                            <td>
                                                <asp:DropDownList ID="drpTHFS" runat="server" Width="100px">
                                                    <asp:ListItem>押金提货</asp:ListItem>
                                                    <asp:ListItem>循环提货</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="drpPL" runat="server" Width="80px">
                                                    <asp:ListItem>蓝装</asp:ListItem>
                                                    <asp:ListItem>绿装</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="drpXGXH" runat="server" Width="200px" 
                                                    onselectedindexchanged="drpXGXH_SelectedIndexChanged" AutoPostBack="true">
                                                    <asp:ListItem>请选择</asp:ListItem>
                                                    <asp:ListItem>FM-Q5942AH</asp:ListItem>
                                                    <asp:ListItem>FM-Q2612AH</asp:ListItem>       
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTHJ" runat="server" Width="80px" Enabled="false" style=" text-align:center;"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtYJ" runat="server" Width="80px" Enabled="false"  style=" text-align:center;"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtXGSL" runat="server" Width="50px" 
                                                    style=" text-align:center;" ontextchanged="txtXGSL_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTHJHJ" runat="server" Width="80px" Enabled="false"  style=" text-align:center;"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtYJHJ" runat="server" Width="80px" Enabled="false"  style=" text-align:center;"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtJYHJ" runat="server" Width="80px" Enabled="false"  style=" text-align:center;"></asp:TextBox>
                                            </td>
                                            <td>
                                            <asp:Button ID="btnAdd" runat="server" CssClass="tj_bt" Text="添加" 
                                                    UseSubmitBehavior="False" onclick="btnAdd_Click" />
                                            </td>
                                        </tr>
                                        <asp:Repeater ID="Repeater1" runat="server" onitemcommand="Repeater1_ItemCommand">
                                            <ItemTemplate>                                            
                                               <tr  class="TbodyTr" style=" width:100%;">
                                                    <td>
                                                        <%#Eval("THFS")%>
                                                    </td>
                                                    <td>
                                                        <%#Eval("PL")%>
                                                    </td>
                                                    <td>
                                                        <%#Eval("XH")%>
                                                    </td>
                                                    <td>
                                                        <%#Eval("THJ")%>

                                                    </td>
                                                    <td>
                                                        <%#Eval("YJJ")%>
                                                    </td>
                                                    <td>
                                                        <%#Eval("SL")%>
                                                    </td>
                                                    <td>
                                                        <%#Eval("THJHJ")%>
                                                    </td>
                                                    <td>
                                                        <%#Eval("YJHJ")%>
                                                    </td>
                                                    <td>
                                                        <%#Eval("JEHJ")%>
                                                    </td>
                                                    <td>
                                                    <asp:Button ID="BtnDelete" runat="server" CssClass="tj_bt" Text="删除"  CommandName="Delete" OnClientClick="return confirm('您确认删除该记录吗?');"/>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tbody>
                                    <tfoot>
                                        <tr id="ts" runat="server" class="TfootTr" style=" width:100%;">
                                            <td colspan="10" align="right" style=" width:100%;">
                                            信息填写完整后，请点击添加按钮。
                                            </td>
                                        </tr>
                                        <tr id="hj" runat="server" class="TfootTr" style=" text-align:center;">
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                            &nbsp;
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                            &nbsp;

                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                合计：
                                            </td>
                                            <td>
                                                <%=ViewState["hjthj"]   %>                                              
                                            </td>
                                            <td>
                                            <%=ViewState["hjyj"]%>
                                            </td>
                                            <td>
                                            <%=ViewState["hjje"] %>
                                            </td>
                                            <td>
                                            &nbsp;
                                            </td>
                                        </tr>
                                    </tfoot>
                                </table>
                            </td>
                        </tr>    
                    </table>
                </div>
            </div>
        </div>
        <asp:Panel ID="Panel1" runat="server" Visible="false">
            <div>
                <table>
                    <tr>
                        <td>
                            会议主题:
                        </td>
                        <td>
                            <asp:TextBox ID="tb_hyzt" runat="server" class="input_out" Width="500px"></asp:TextBox>
                        </td>
                        <td>
                            会议主办方:
                        </td>
                        <td>
                            <asp:TextBox ID="tb_hyzbf" runat="server" class="input_out" Width="178px"></asp:TextBox>
                        </td>
                        <td>
                            会议主持人:
                        </td>
                        <td>
                            <asp:TextBox ID="tb_hyzcr" runat="server" class="input_out" Width="178px"></asp:TextBox>
                        </td>
                        <td>
                            会议地点:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_hydd" runat="server" Width="178px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            会议开始时间:
                        </td>
                        <td>
                            <asp:TextBox ID="txtHYStartTime" runat="server" Width="178px" onfocus="setday(this,document.all.txtHYStartTime)"></asp:TextBox>
                        </td>
                        <td>
                            会议结束时间:
                        </td>
                        <td>
                            <asp:TextBox ID="txtHYEndTime" runat="server" Width="178px" onfocus="setday(this,document.all.txtHYEndTime)"></asp:TextBox>
                        </td>
                        <td>
                            会议申请人姓名:
                        </td>
                        <td>
                            <asp:TextBox ID="tb_hysqrxm" runat="server" class="input_out" Width="178px"></asp:TextBox>
                        </td>
                        <td>
                            是否有效:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_sfyx" runat="server" Width="178px">
                                <asp:ListItem>所有</asp:ListItem>
                                <asp:ListItem>有效</asp:ListItem>
                                <asp:ListItem>撤销</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            申请提交时间:
                        </td>
                        <td>
                            <asp:TextBox ID="tb_sqtjsj" runat="server" Width="178px" onfocus="setday(this,document.all.tb_sqtjsj)"></asp:TextBox>
                        </td>
                        <td>
                            会议当前状态:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_hydqzt" runat="server" Width="178px">
                                <asp:ListItem>所有</asp:ListItem>
                                <asp:ListItem>尚未召开</asp:ListItem>
                                <asp:ListItem>正在召开</asp:ListItem>
                                <asp:ListItem>已经结束</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td colspan="4" align="right">
                            <asp:Button ID="BtnCheck" runat="server" CssClass="button" OnClick="BtnCheck_Click"
                                Text="查询" />
                        </td>
                    </tr>
                </table>
            </div>
        </asp:Panel>
        <asp:Panel ID="Panel3" runat="server" Width="100%" ScrollBars="Both" Visible="true">
            <radg:RadGrid ID="RadGrid1" runat="server" AllowPaging="True" Skin="Monochrome" PageSize="10"
                Width="98%" OnPageIndexChanged="RadGrid1_PageIndexChanged">
                <HeaderStyle Height="28px"></HeaderStyle>
                <ExportSettings>
                    <Pdf PageWidth="8.5in" PageRightMargin="" PageFooterMargin="" PageLeftMargin="" PageTopMargin=""
                        PageHeight="11in" PageBottomMargin="" PageHeaderMargin=""></Pdf>
                </ExportSettings>
                <PagerStyle NextPageText="下一页" HorizontalAlign="Right" PagerTextFormat=" {4} &amp;nbsp;|&amp;nbsp; 当前页 {0} / {1}, 当前记录{2} 到 {3} 共 {5}条."
                    PrevPageText="上一页"></PagerStyle>
                <MasterTableView AutoGenerateColumns="false" DataKeyNames="ID">
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
                        <radg:GridBoundColumn DataField="ID" HeaderText="编号" SortExpression="ID" UniqueName="ID"
                            Visible="false">
                        </radg:GridBoundColumn>
                        <radg:GridBoundColumn DataField="HYZT" HeaderText="会议主题" SortExpression="HYZT" UniqueName="HYT">
                        </radg:GridBoundColumn>
                        <radg:GridBoundColumn DataField="HYZBF" HeaderText="会议主办方" SortExpression="HYZBF"
                            UniqueName="HYZBF">
                        </radg:GridBoundColumn>
                        <radg:GridBoundColumn DataField="HYZCR" HeaderText="会议主持人" SortExpression="HYZCR"
                            UniqueName="HYZCR">
                        </radg:GridBoundColumn>
                        <radg:GridBoundColumn DataField="HYDD" HeaderText="会议地点" SortExpression="HYDD" UniqueName="HYDD">
                        </radg:GridBoundColumn>
                        <radg:GridBoundColumn DataField="HYKSSJ" HeaderText="会议开始时间" SortExpression="HYKSSJ"
                            UniqueName="HYKSSJ">
                        </radg:GridBoundColumn>
                        <radg:GridBoundColumn DataField="HYJSSJ" HeaderText="会议结束时间" SortExpression="HYJSSJ"
                            UniqueName="HYJSSJ">
                        </radg:GridBoundColumn>
                        <radg:GridBoundColumn DataField="HYSQRGH" HeaderText="会议申请人工号" SortExpression="HYSQRGH"
                            UniqueName="HYSQRGH" Visible="false">
                        </radg:GridBoundColumn>
                        <radg:GridBoundColumn DataField="HYSQRXM" HeaderText="会议申请人" SortExpression="HYSQRXM"
                            UniqueName="HYSQRXM">
                        </radg:GridBoundColumn>
                        <radg:GridBoundColumn DataField="SFYX" HeaderText="是否有效" SortExpression="SFYX" UniqueName="SFYX">
                        </radg:GridBoundColumn>
                        <radg:GridBoundColumn DataField="SQTJSJ" HeaderText="申请提交时间" SortExpression="SQTJSJ"
                            UniqueName="SQTJSJ">
                        </radg:GridBoundColumn>
                        <radg:GridBoundColumn DataField="HYDQZT" HeaderText="会议当前状态" SortExpression="HYDQZT"
                            UniqueName="HYDQZT">
                        </radg:GridBoundColumn>
                        <radg:GridBoundColumn DataField="HYDJS" HeaderText="会议倒计时(小时)" SortExpression="HYDJS"
                            UniqueName="HYDJS">
                        </radg:GridBoundColumn>
                        <radg:GridBoundColumn DataField="Ischeck" HeaderText="是否审核" SortExpression="Ischeck"
                            UniqueName="Ischeck">
                        </radg:GridBoundColumn>
                        <radg:GridBoundColumn DataField="Remark" HeaderText="备注" SortExpression="Remark"
                            UniqueName="Remark">
                        </radg:GridBoundColumn>
                        <radg:GridTemplateColumn UniqueName="TemplateColumn">
                            <ItemStyle Width="0px" />
                            <HeaderTemplate>
                                查看详情
                            </HeaderTemplate>
                            <ItemTemplate>
                                <a href='HYSSQDJ.aspx?lable=view&ly=qgs&ID=<%# DataBinder.Eval(Container.DataItem, "ID")%>'
                                    style="color: Red">查看详情</a>
                            </ItemTemplate>
                        </radg:GridTemplateColumn>
                        <radg:GridTemplateColumn UniqueName="TemplateColumn">
                            <ItemStyle Width="0px" />
                            <ItemTemplate>
                                <asp:LinkButton Text="撤销会议" OnClientClick="javascript:return confirm('您确实要撤销此会议吗？');"
                                    OnClick="lbtnCX_Click" runat="server" ID="lbtnCX" ForeColor="red"></asp:LinkButton>
                            </ItemTemplate>
                            <HeaderTemplate>
                                撤销会议
                            </HeaderTemplate>
                        </radg:GridTemplateColumn>
                        <radg:GridTemplateColumn UniqueName="TemplateColumn">
                            <ItemStyle Width="0px" />
                            <ItemTemplate>
                                <a href='HYSSQDJ.aspx?lable=Edit&ly=qgs&ID=<%# DataBinder.Eval(Container.DataItem, "ID")%>'
                                    style="color: Red">会议变更</a>
                            </ItemTemplate>
                            <HeaderTemplate>
                                会议变更
                            </HeaderTemplate>
                        </radg:GridTemplateColumn>
                        <radg:GridTemplateColumn UniqueName="TemplateColumn">
                            <ItemStyle Width="0px" />
                            <ItemTemplate>
                                <asp:LinkButton Text="审核" OnClick="lbtnCHeck_Click" runat="server" ID="lbtnCHeck"
                                    ForeColor="red"></asp:LinkButton>
                            </ItemTemplate>
                            <HeaderTemplate>
                                审核
                            </HeaderTemplate>
                        </radg:GridTemplateColumn>
                        <radg:GridTemplateColumn UniqueName="TemplateColumn">
                            <ItemStyle Width="0px" />
                            <ItemTemplate>
                                <asp:LinkButton Text="备注" runat="server" ID="lbtnRemark" ForeColor="red" OnClick="lbtnRemark_Click"></asp:LinkButton>
                            </ItemTemplate>
                            <HeaderTemplate>
                                备注
                            </HeaderTemplate>
                        </radg:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </radg:RadGrid>
        </asp:Panel>
    </div>
    <asp:Panel ID="Panel2" runat="server">
        <table border="0" cellpadding="5" cellspacing="0" class="FormView" width="100%">
            <tr>
                <td align="left">
                    <asp:Button ID="btnTJ" runat="server" Text="显示查询条件" CssClass="button" Width="102px"
                        OnClick="btnTJ_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <div id="divStop" runat="server" style="border: 3px solid #C3D9FF; text-align: left;
        left: 50px; top: 348px; width: 700px; height: 150px; background-color: #F9FBFD;
        position: absolute; z-index: 999;" visible="false">
        <div id="Div2" style="cursor: move; left: 1px; top: 100px; width: 100%; height: 30px;
            background-color: #A5C2E0; text-align: center; vertical-align: center; line-height: 30px;
            font-weight: bold; color: #ffffff">
            您正在为：
            ; color: #ffffff">
            您正在为：
            <asp:Label ID="labsmtitle" runat="server" Text="Label">   
            </asp:Label>
            添加备注。 使用部门:
            <asp:Label ID="labsmid" runat="server" Text="Label"></asp:Label></div>
        <table width="100%" style="height: 122px">
            <tr>
                <td colspan="1" valign="middle" class="style6" align="center">
                    备注：
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtStopidear" runat="server" TextMode="MultiLine" Width="540px"
                        Height="56px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button ID="btnStopSave" runat="server" Text="确定" CssClass="button" OnClick="btnStopSave_Click" />
                    <asp:Button ID="Btnstopcancel" runat="server" Text="取消" CssClass="button" OnClick="Btnstopcancel_Click" />
                </td>
            </tr>
        </table>
    </div>
    <input runat="server" id="hidID" type="hidden" />
    </form>
</body>
</html>
