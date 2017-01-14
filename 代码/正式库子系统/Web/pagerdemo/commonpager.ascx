<%@ Control Language="C#" AutoEventWireup="true" CodeFile="commonpager.ascx.cs" Inherits="Web_pagerdemo_commonpager" %>
<style type="text/css">
<!--
.yhb_pagebox{overflow:hidden;  font-size:12px; font-family:"宋体",sans-serif;}
.yhb_pagebox span{float:left; margin-right:2px; overflow:hidden; text-align:center; background:#e6f2fe;}
.yhb_pagebox span a{display:block; overflow:hidden;  _float:left;}
.yhb_pagebox span.pagebox_pre_nolink{border:1px #ddd solid; width:53px; height:21px; line-height:21px; text-align:center; color:#999; cursor:default;}
.yhb_pagebox span.pagebox_pre{color:#000000; height:23px;}
.yhb_pagebox span.pagebox_pre a,.yhb_pagebox span.pagebox_pre a:visited,.yhb_pagebox span.pagebox_next a,.yhb_pagebox span.pagebox_next a:visited{border:1px #5d9cdf solid; color:#000000; text-decoration:none; text-align:center; width:53px; cursor:pointer; height:21px; line-height:21px;}
.yhb_pagebox span.pagebox_pre a:hover,.yhb_pagebox span.pagebox_pre a:active,.yhb_pagebox span.pagebox_next a:hover,.yhb_pagebox span.pagebox_next a:active{color:#FF0000; border:1px #9FC4EC solid;background:#ffffff;}
.yhb_pagebox span.pagebox_num_nonce{border:1px #2670C1 solid; padding:0 8px; height:21px; line-height:21px; color:#000000; cursor:default; background:#a9d2ff;}
.yhb_pagebox span.pagebox_num{color:#000000; height:23px;}
.yhb_pagebox span.pagebox_num a,.yhb_pagebox span.pagebox_num a:visited{border:1px #5d9cdf solid; color:#000000; text-decoration:none; padding:0 8px; cursor:pointer; height:21px; line-height:21px;}
.yhb_pagebox span.pagebox_num a:hover,.yhb_pagebox span.pagebox_num a:active{border:1px #9FC4EC solid;color:#FF0000;background:#ffffff;}
.yhb_pagebox span.pagebox_num_ellipsis{color:#393733; width:22px; background:none; line-height:23px;}
.yhb_pagebox span.pagebox_next_nolink{border:1px #ddd solid; width:53px; height:21px; line-height:21px; text-align:center; color:#999; cursor:default;}
-->
</style>
<table border="0"  runat="server" id="zspagetable" visible="false" class="yhb_pagebox" >
  <tr>
    <td><span class="pagebox_pre"><asp:LinkButton ID="Bsy" runat="server" Text="首页" OnClick="Bsy_Click"  ></asp:LinkButton></span></td>
    <td>
    <span class="pagebox_pre"><asp:LinkButton ID="Bsyy" runat="server" Text="上一页" OnClick="Bsyy_Click"  ></asp:LinkButton></span>
      </td>
    <td runat="server" id="Cpageshow">
      </td>
    <td>
        <span class="pagebox_next"><asp:LinkButton ID="Bxyy" runat="server" Text="下一页" OnClick="Bxyy_Click"  ></asp:LinkButton></span>
      </td>
    <td>
            <span class="pagebox_next"><asp:LinkButton ID="Bwy" runat="server" Text="尾页" OnClick="Bwy_Click"  ></asp:LinkButton></span>
      </td>
    <td>转到:</td>
    <td>
        <asp:TextBox ID="tbgopage" runat="server" Width="42px" 
            onkeypress="var keynum;var keychar;var numcheck;if(window.event) {keynum = event.keyCode;}else if(event.which) {keynum = event.which;}if ( !(((window.event.keyCode >= 48) && (window.event.keyCode <= 57)) || (window.event.keyCode == 13))){return false;}" 
            MaxLength="8" style="width:40px; height:20px;border:1px #ccc solid;"></asp:TextBox>
        </td>
            <td>页
      </td>
    <td><span class="pagebox_next">
    <asp:LinkButton ID="Bgo" runat="server" Text="go" OnClick="Bgo_Click" style="width:20px;"></asp:LinkButton></span>
      </td>
    <td runat="server" id="tbpagerinfo"></td>
  </tr>
</table>