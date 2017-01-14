<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WTLBJXQ.aspx.cs" Inherits="Web_JHJX_New2013_JHJX_HWSF_WTLBJXQ" %>
<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
     <link href="../../../../css/standardStyle.css" rel="stylesheet" type="text/css" />
    <script src="../../../../js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/jquery.tablechangecolor.js" type="text/javascript"></script> 
    <style type="text/css">
        .lefttd {
            width: 180px;
            height: 30px;
            text-align:right;
        }
          .righttd {
            width: 320px;
            height: 30px;
        }
      
        .auto-style1 {
            height: 67px;
            text-align:right;
            
        }
      
    </style>
</head>
<body>
    <form id="form1" runat="server">
         <radTS:RadTabStrip ID="TabStrip1" runat="server" CausesValidation="False" ReorderTabRows="True"
        Skin="Default2006" Width="99%">
        <Tabs>            
     
            <radTS:Tab ID="Tab5" runat="server" NavigateUrl=""  Text="问题类别及详情"  ForeColor="Red"
                 Font-Size="12px">
            </radTS:Tab>
        </Tabs>
    </radTS:RadTabStrip>
    <div>
        <div></div>
                            <hr />
                    
    <table style="width:500px; height: 142px;">
        <tr>
            <td class="lefttd">验收时照片：</td>
            <td class="righttd"><a id="ysszp"  runat="server" href="" target="_blank">查看</a></td>
        </tr>
        <tr runat="server" id="tryyj">
            <td class="lefttd">纸质发货单（物流单）影印件：</td>
            <td class="righttd"><a id="fhdyyj"  runat="server" href="" target="_blank">查看</a></td>
        </tr>
        <tr runat="server" id="trsm">
            <td class="auto-style1">情况说明：</td>
            <td >
                <asp:TextBox ID="txtqksm" runat="server" Height="55px"  Width="320px"  CssClass="tj_input"
                        TextMode="MultiLine" ReadOnly="True"></asp:TextBox>
            </td>
        </tr>
        <tr runat="server" id="trsl">
            <td class="lefttd">实际签收数量：</td>
            <td class="righttd">
                <asp:Label ID="lbqssl" runat="server" ></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="lefttd">处理结果：</td>
            <td class="righttd">
                <asp:Label ID="lbcljg" runat="server" ></asp:Label>
            </td>
        </tr>
    </table>
        <div style="align-content:center">
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnGoBack" runat="server" UseSubmitBehavior="False"  CssClass="tj_bt_da" Text="返回列表" OnClick="btnGoBack_Click" Height="30px" Width="80px" />
                    </div>
    </div>
    </form>
</body>
</html>
