<%@ Page Language="C#" AutoEventWireup="true" CodeFile="XXHZX_CXSCMKLB.aspx.cs" Inherits="Web_XXHZX_XXHZX_CXSCMKLB" %>
<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<%@ Register Assembly="RadAjax.Net2" Namespace="Telerik.WebControls" TagPrefix="radA" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>重新生成模块列表</title>
    <link href="../../css/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" language="javascript">
        function AA()
        {
            document.getElementById("divButton").style.display = "none";
            document.getElementById("divContent").style.display = "block";
            return true;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
     <radTS:RadTabStrip ID="TabStrip1" runat="server" CausesValidation="False" Height="25px"
            ReorderTabRows="True" Skin="Default2006" Width="580px">
            <Tabs>
                <radTS:Tab ID="Tab1" runat="server" ImageUrl="~/RadControls/Grid/Skins/AddRecord.gif"
                    NavigateUrl="XXHZX_CXSCMKLB.aspx" Text="重新生成模块列表">
                </radTS:Tab>
            </Tabs>
        </radTS:RadTabStrip>
        <table border="0" cellpadding="0" cellspacing="0" style="width: 600px">
            <tr>
                <td style="width: 100px">
                </td>
                <td style="width: 100px">
                </td>
            </tr>
            <tr>
                <td style="width: 100px">
                </td>
                <td style="width: 100px">
                </td>
            </tr>
            <tr>
                <td style="width: 100px">
                </td>
                <td style="width: 100px">
                    <div id="divButton" style="display:block;">
                    <asp:Button ID="Button1" runat="server" OnClientClick="return AA()" OnClick="Button1_Click" Text="点击生成" Width="80px" />
                        </div>
                     <div id="divContent" style="display:none;">       
              <asp:Image ID="Image1" runat="server" AlternateText="Loading..." ImageUrl="~/RadControls/Ajax/Skins/Default/Loading.gif" />
          </div>
                 </td>
            </tr>
        </table>
        
        <div id="divError" runat="server" visible="false" style="width:100%; height:200px; overflow-y:scroll;"></div>

    </form>
</body>
</html>
