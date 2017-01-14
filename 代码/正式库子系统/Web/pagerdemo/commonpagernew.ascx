<%@ Control Language="C#" AutoEventWireup="true" CodeFile="commonpagernew.ascx.cs" Inherits="Web_pagerdemo_commonpagernew" %>
<script type="text/javascript" language="javascript">
    function EnterFinished(this_an) {
        var this_an_array = this_an.id.split('_');
        this_an_array[this_an_array.length - 1] = "linkZD";
        var this_an_linkid = this_an_array.join('$');
        __doPostBack(this_an_linkid, '');
        }
</script>
<style type="text/css">
       .content_nr_cx
        {
            border: solid 0px #99BBE8;
        }
        .content_nr_cx tr
        {

        }
         .content_nr_cx span{ overflow:hidden; text-align:center; background:#E0E7F7;}
           .content_nr_cx span.pagebox_first a{display:block; overflow:hidden;  _float:left; width:15px; height:15px;   background: url(/Web/images/commonpagernew/08.png)  0 0;}
              .content_nr_cx span.pagebox_firstUnable a{display:block; overflow:hidden;  _float:left; width:15px; height:15px;   background: url(/Web/images/commonpagernew/06.png)  0 0;
           }
             .content_nr_cx span.pagebox_last a{display:block; overflow:hidden;  _float:left; width:15px; height:15px;   background: url(/Web/images/commonpagernew/03.png)  0 0;
           }
            .content_nr_cx span.pagebox_lastUnable a{display:block; overflow:hidden;  _float:left; width:15px; height:15px;   background:transparent url(/Web/images/commonpagernew/01.png)  0 0;
           }
              .content_nr_cx span.pagebox_next a{display:block; overflow:hidden;  _float:left; width:15px; height:15px;   background: url(/Web/images/commonpagernew/04.png) 0 0;
           }
               .content_nr_cx span.pagebox_nextUnable a{display:block; overflow:hidden;  _float:left; width:15px; height:15px;   background: url(/Web/images/commonpagernew/02.png)  0 0;
           }
                .content_nr_cx span.pagebox_end a{display:block; overflow:hidden;  _float:left; width:15px; height:15px;   background: url(/Web/images/commonpagernew/07.png)  0 0;
           }
              .content_nr_cx span.pagebox_endUnable a{display:block; overflow:hidden;  _float:left; width:15px; height:15px;   background: url(/Web/images/commonpagernew/05.png)  0 0;
          }
            .content_nr_cx span.space
                {
                    display:block; overflow:hidden;   width:6px; height:16px;   background: url(/Web/images/commonpagernew/09.png)  0 0; background-repeat:no-repeat;
                    }
                     .content_nr_cx  a:visited{border:1px #5d9cdf solid; color:#000000; text-decoration:none; text-align:center; width:53px; cursor:pointer; height:20px; }
        </style>
<table width="100%" id="zspagetable" cellpadding="0" cellspacing="0" class="content_nr_cx">
                        <tr>
                           <td width="8px">&nbsp;</td>
                            <td width="40px" >
                             <span id="pagebox_first" runat="server" class="pagebox_firstUnable"> <asp:LinkButton ID="linkFirst" runat="server" Text="" OnClick="Bsy_Click"  ></asp:LinkButton></span>
                            </td>
                            <td width="20px" >
                               <span id="pagebox_last" runat="server" class="pagebox_lastUnable"> <asp:LinkButton ID="linkLast" runat="server" Text="" OnClick="Bsyy_Click"  ></asp:LinkButton></span>
                            </td>
                              <td width="15px" >
                              <span class="space"></span>
                            </td>
                               <td width="35px" >
                               <asp:LinkButton ID="linkZD" runat="server" UseSubmitBehavior="False" OnClick="Bgo_Click"  Text="转到"></asp:LinkButton>
                        
                            </td>
                            <td width="42px" align="right">
                            <asp:TextBox ID="tbgopage" runat="server" Width="42px" 
            onkeypress="var keynum;var keychar;var numcheck;if(window.event) {keynum = event.keyCode;}else if(event.which) {keynum = event.which;}if ( !(((window.event.keyCode >= 48) && (window.event.keyCode <= 57)))){ if (window.event.keyCode == 13) {EnterFinished(this); return false; }return false;}" 
            MaxLength="8"  style="width:40px; height:20px;     line-height:20px; border:1px #ccc solid; font-family:宋体; font-size:12px; color:Black;"></asp:TextBox>
        </td>
                           <td width="50px" id="tdpagecount" runat="server" style=" font: 12px/1.5 tahoma,arial,宋体; color:Black;" >
                           
                           </td>
                               <td width="15px" >
                              <span class="space"></span>
                            </td>
                            <td width="40px" align="right">
                               <span id="pagebox_next" runat="server" class="pagebox_nextUnable"> <asp:LinkButton ID="linkNext" runat="server" Text="" OnClick="Bxyy_Click"  ></asp:LinkButton></span>
                            </td>
                            <td width="40px" align="right">
                               <span id="pagebox_end" runat="server" class="pagebox_endUnable"> <asp:LinkButton ID="linkEnd" runat="server" Text=""  OnClick="Bwy_Click" ></asp:LinkButton></span>
                            </td>
                            <td width="20px" align="center">
                              
                            </td>
                            <td width="120px">
                              
                            </td>
                            <td width="80px" align="right">
                              
                            </td>
                            <td width="120px">
                             
                            </td>
                            <td id="tdpagecountdesc" runat="server" align="right" style=" font: 12px/1.5 tahoma,arial,宋体;color:Black;">
                            
                            </td>
                             <td width="5px">
                            
                            </td>
                        </tr>
                    </table>