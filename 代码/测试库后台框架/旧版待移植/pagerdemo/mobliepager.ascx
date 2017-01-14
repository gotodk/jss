<%@ Control Language="C#" AutoEventWireup="true" CodeFile="mobliepager.ascx.cs" Inherits="pagerdemo_mobliepager" %>


<div id="fmdivpage" runat="server" class="fmdivpage">
<asp:Button ID="Bsyy" runat="server"  OnClick="Bsyy_Click" Text="上一页" class="input_fenye" ></asp:Button>
<span class="feiye"><span class="zitihong " runat="server" id="nowpagenum">1</span>/<span runat="server" id="allpagenum">10</span></span>
<asp:Button ID="Bxyy" runat="server" Text="下一页" OnClick="Bxyy_Click"  class="input_fenye" ></asp:Button>
</div>

