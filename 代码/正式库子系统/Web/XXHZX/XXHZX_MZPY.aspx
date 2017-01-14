<%@ Page Language="C#" AutoEventWireup="true" CodeFile="XXHZX_MZPY.aspx.cs" Inherits="Web_XXHZX_XXHZX_MZPY" %>
<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>工作能力评议表</title>
    <link type="text/css" rel="Stylesheet" href="/css/style.css" />
    <link href="/web/yhb_BigPage_css/YahooGridView.css" type="text/css" rel="stylesheet" />  
<style type ="text/css"> 
  .Border1   
  {   
  border-right:   #000000   1px   solid;   
  border-top:   #000000     1px   solid;   
  border-left:   #000000   1px   solid;   
  border-bottom:   #000000   1px   solid;   
  }   
  .Border2   
  {   
  border-right:   #000000   1px   solid;   
  border-top:   #000000    1px   solid;   
  border-left:   #000000   0px   solid;   
  border-bottom:  #000000    1px   solid;   
  }   
  .Border3   
  {   
  border-right:   #000000    1px   solid;   
  border-top:   #000000      0px   solid;   
  border-left:   #000000    1px   solid;   
  border-bottom:   #000000    1px   solid;   
  } 
  .Border4   
  {   
  border-right:   #000000   1px   solid;   
  border-top:   #000000     0px   solid;   
  border-left:   #000000    0px   solid;   
  border-bottom:   #000000    1px   solid;   
  }   
  </style>   
  <script type ="text/javascript" language ="javascript">
   function setScore()
        {  
            var gv = document.getElementById("GridView1"); 
            var count=gv.rows.length;
            for(var i=1;i<count;i++)
            {
                var score=0;
               for(var j=2;j<8;j++)
               {
                    if(!isNaN(parseFloat(gv.rows[i].cells[j].childNodes[0].value)))
                    {
                        score=score+parseFloat(gv.rows[i].cells[j].childNodes[0].value);
                    } 
                                        
               }
               gv.rows[i].cells[8].childNodes[0].value=score;
            }
         }
  </script>
</head>
<body onload="setScore()">
    <form id="form1" runat="server">
    <radTS:RadTabStrip ID="RadTabStrip1" runat="server" Height="0px" Skin="Default2006">
        <Tabs>
            <radTS:Tab ID="Tab1" runat="server" Text="工作能力评议表">
            </radTS:Tab>
        </Tabs>
    </radTS:RadTabStrip>  
    <br />      
    <table width="1060px" style="font-size: 9pt" cellpadding ="0" cellspacing="0">
        <tr>
        
            <td colspan="7" align="center" style="font-weight: bold; font-size:10pt">
                工作能力评议指标及说明
            </td>
            </tr>
            <tr style ="font-weight :bold ">
            <td class="Border1" align="center" width="80px" height="25px" >指标项目</td>
           <td align="center" width="160px" class ="Border2">
                计划组织与协调能力
            </td> 
            <td align="center" width="170px" class ="Border2" >
                发现问题和解决问题能力
            </td> 
            <td align="center" width="150px" class ="Border2">
                专业知识和技能
            </td>
            <td align="center" width="150px" class ="Border2">
                创新能力
            </td>
            <td align="center" width="150px" class ="Border2">
                学习能力
            </td>
            <td align="center" width="200px" class ="Border2">
                沟通能力
            </td>
            
        </tr>        
        <tr>
            <td align ="center" width="80px" class ="Border3" style ="font-weight:bold ">详细说明</td>
            <td align ="left" width="160px" class ="Border4">
                制定完善的工作计划，并按计划有序开展工作，积极协调与相关岗位的合作关系，调动现有资源高标准完成工作任务。满分8分。<br />
                组织协调能力较差，工作不能按时完成，或质量较差：0-2分；<br />
                组织协调能力一般，工作较被动：3-5分；<br />
                组织协调能力较好，高标准完成各项工作：6-8分。
            </td>
             <td align ="left" width="170px" class ="Border4">
                对事情的发展有预见性，能够及时发现问题隐患，提出良好的预防措施，保证事情朝预定的目标发展。满分6分。<br />
                预见性较差，问题出现后未采取任何解决措施：0-1分；<br />
                有一定的预见性，能及时解决出现的问题：2-3分；<br />
                有良好的预见性，并主动进行各种预防措施，防患于未然：4-6分。
            </td>
             <td align ="left" width="150px" class ="Border4">
                专业知识扎实过硬，熟练掌握业务工作中所需各项技能。满分8分。<br />
                专业知识与技能较差，不能独立承担本职工作，依赖性较强：0-2分；<br />
                专业知识与技能一般，能独立承担本职工作，技能待提升：3-5分；<br />
                专业知识与技能过硬，能高效率完成本职工作：6-8分。
            </td>
             <td align ="left" width="150px" class ="Border4">
                对所负责的工作经常提出合理的建议，能够运用到实际工作中，取得显著成效。满分6分。<br />
                创新能力较差，不能提出有利于工作改善的建议：0分；<br />
                有一定的创新能力，能提出独到的见解：1-4分；<br />
                有较强创新能力，经常提出合理化建议并应用于实际工作：5-6分。
            </td>
            <td align ="left" width="150px" class ="Border4">
                对新知识、新技能有强烈的渴求，积极利用多种途径学习业务知识。满分6分。<br />
                习能力较差，并缺乏主动性：0-1分；<br />
                有一定的学习能力，能主动学习新知识与技能：2-3分；<br />
                学习能力强，有目的、有计划并积极将学习成果应用于实际工作：4-6分。
            </td>
             <td align ="left" width="200px" class ="Border4">
                重视且乐于沟通，愿意与人建立联系，言简意赅，意思表达清晰， 遇到问题，能够以积极的心态和不懈的努力对待冲突和矛盾。满分6分。<br />
                沟通能力欠缺，主动沟通意识差，工作被动：0-2分；<br />
                具有一定的沟通意识，对待冲突和矛盾的方式方法待改进：3-4分；<br />
                沟通能力较强，遇到问题积极主动沟通，注重沟通方法，成效显著：5-6分。
            </td>
        </tr>
    </table>
    <br />
    <div id="pingfen" runat ="server" >
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False"  CssClass="GridViewStyle">
            <Columns>
                <asp:BoundField DataField="被评人姓名" HeaderText="被评人姓名" />
                <asp:BoundField DataField="评分人姓名" HeaderText="评分人姓名" />
                <asp:TemplateField HeaderText="计划组织与协调能力">
                <ItemTemplate>
                    <asp:TextBox ID="tb_jhzzyxtnl" runat="server" Text='<%# Bind("计划组织与协调能力") %>' 
                        Width="100px" onKeypress="return (/[\d.]/.test(String.fromCharCode(event.keyCode)));" onKeyup="setScore();" onpaste="return false;" style="ime-mode:Disabled"></asp:TextBox>
                </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="发现问题和解决问题能力">
                <ItemTemplate>
                    <asp:TextBox ID="tb_fxwthjjwtnl" runat="server" Text='<%# Bind("发现问题和解决问题能力") %>' 
                        Width="100px" onKeypress="return (/[\d.]/.test(String.fromCharCode(event.keyCode)))" onKeyup="setScore();" onpaste="return false;" style="ime-mode:Disabled"></asp:TextBox>
                </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="专业知识技能">
                <ItemTemplate>
                    <asp:TextBox ID="tb_zyzsjn" runat="server" Text='<%# Bind("专业知识和技能") %>' 
                        Width="100px" onKeypress="return (/[\d.]/.test(String.fromCharCode(event.keyCode)))" onKeyup="setScore();" onpaste="return false;" style="ime-mode:Disabled"></asp:TextBox>
                </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="创新能力">
                <ItemTemplate>
                    <asp:TextBox ID="tb_cxnl" runat="server" Text='<%# Bind("创新能力") %>' 
                        Width="100px" onKeypress="return (/[\d.]/.test(String.fromCharCode(event.keyCode)))" onKeyup="setScore();" onpaste="return false;" style="ime-mode:Disabled"></asp:TextBox>
                </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="学习能力">
                <ItemTemplate>
                    <asp:TextBox ID="tb_xxnl" runat="server" Text='<%# Bind("学习能力") %>' 
                        Width="100px" onKeypress="return (/[\d.]/.test(String.fromCharCode(event.keyCode)))" onKeyup="setScore();" onpaste="return false;" style="ime-mode:Disabled"></asp:TextBox>
                </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="沟通能力">
                <ItemTemplate>
                    <asp:TextBox ID="tb_gtnl" runat="server" Text='<%# Bind("沟通能力") %>' 
                        Width="100px" onKeypress="return (/[\d.]/.test(String.fromCharCode(event.keyCode)))" onKeyup="setScore();" onpaste="return false;" style="ime-mode:Disabled"></asp:TextBox>
                </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="单人总分">
                <ItemTemplate>
                    <asp:TextBox ID="drzf" runat="server" Text='<%# Bind("单人总分") %>' 
                        Width="100px" onKeypress="return (/[\d.]/.test(String.fromCharCode(event.keyCode)))" style="ime-mode:Disabled"></asp:TextBox>
                </ItemTemplate>
                </asp:TemplateField>
            </Columns>
             <HeaderStyle CssClass="HeaderStyle" />
        <RowStyle CssClass="RowStyle" />        
        <AlternatingRowStyle CssClass="AltRowStyle" />
        </asp:GridView>
        </div>
   <table width="1000"><tr><td align ="right"> <asp:Button ID="Button1" runat="server" Width ="50px" Text="提交" OnClick ="button_tj" /></td></tr></table> 
   
    </form>
</body>
</html>
