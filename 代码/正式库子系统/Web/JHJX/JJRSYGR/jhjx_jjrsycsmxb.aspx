<%@ Page Language="C#" AutoEventWireup="true" CodeFile="jhjx_jjrsycsmxb.aspx.cs" Inherits="Web_JHJX_JJRSYGR_jhjx_jjrsycsmxb" %>

<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="../../pagerdemo/commonpagernew.ascx" TagName="commonpagernew" TagPrefix="uc1" %>
<%@ Register src="../New2013/UCFWJG/UCFWJGDetail.ascx" tagname="UCFWJGDetail" tagprefix="uc2" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>经纪人收益产生明细表</title>
    <script src="../../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="../../jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/art_confirm.js" type="text/javascript"></script>
    <link href="../../../css/standardStyle.css" rel="stylesheet" />
    <script src="../../../js/standardJSFile/jquery.tableresizer.js" type="text/javascript"></script>
    <script src="../../../js/standardJSFile/jquery.art_confirm.js" type="text/javascript"></script>
    <script src="../../../js/standardJSFile/jquery.tablechangecolor.js" type="text/javascript"></script>
    <script type="text/javascript">
    </script>
    <style type="text/css">
        #content_zw
        {
            width: 1024px;
        }
        </style>
</head>
<body style="background-color: #f7f7f7;">
    <form id="form1" runat="server">
    <radTS:RadTabStrip ID="RadTabStrip1" runat="server" Height="25px" Skin="Default2006"
        BackColor="#F7F7F7">
        <Tabs>
            <radTS:Tab ID="Tab2" runat="server" NavigateUrl="jhjx_jjrsyhzbgr.aspx" 
                Text="经纪人（自然人）收益汇总表">
            </radTS:Tab>
            <radTS:Tab ID="Tab1" runat="server" NavigateUrl="jhjx_jjrsycsmxb.aspx" ForeColor="red"
                Text="经纪人收益产生明细表">
            </radTS:Tab>
        </Tabs>
    </radTS:RadTabStrip>
    <div id="new_content">
        <div id="new_zicontent">
            <div id="content_zw">
                
                <div class="content_nr">
                    <table width="100%"  cellpadding="0" cellspacing="0" class="content_nr_cx" style="border: solid 1px #ccc;height:66px;">
                        <tr>
                            
                           
                            <td   style ="padding-left:10px" >
                                <uc2:UCFWJGDetail ID="UCFWJGDetail1" runat="server" />
                            </td> 
                        </tr>
                          <tr>
                             <td  >
                                 <table>
                                     <tr>
                                       <td  style="padding-left:10px;width:90px">
                                收益产生时间：
                            </td>
                            <td width="120px" align="left">
                                 <asp:TextBox ID="Txtkssj" runat="server"  class="tj_input Wdate" onClick="WdatePicker({readOnly:true,minDate:'2012-01-01',maxDate:'2022-01-01',dateFmt:'yyyy-MM-dd'});" ></asp:TextBox>
                            </td>
                            
                            <td width="20px" align="center">
                                至</td>
                            <td style="text-align: left; width: 120px;">
                                 <asp:TextBox ID="Txtjssj" runat="server"  class="tj_input Wdate" onClick="WdatePicker({readOnly:true,minDate:'2012-01-01',maxDate:'2022-01-01',dateFmt:'yyyy-MM-dd'});" ></asp:TextBox>
                            </td>
                           
                            <td style="text-align: right; width: 90px;" align="right" >
                                经纪人类别：</td>
                            <td width="70px" align="left">
                              <asp:DropDownList ID="ddljjrlb" runat="server" CssClass="tj_input" Width="70px" 
                                    Height="23">
                                    <asp:ListItem>无</asp:ListItem>
                                    <asp:ListItem>自然人</asp:ListItem>
                                    <asp:ListItem>单位</asp:ListItem>
                                    
                                    </asp:DropDownList>
                            </td>
                            <td width="85px" align="right">
                                经纪人编号：</td>
                            <td width="95px" align="left">
                                <asp:TextBox ID="txtjjrbh" runat="server" class="tj_input" Width="110px" Height="23"></asp:TextBox>
                              </td>                          
                           
                     
                            <td style="text-align: right; width: 10px; visibility:collapse">
                                <asp:TextBox ID="txtjjrxm" runat="server" class="tj_input" Width="10px" Height="23" Visible="false"></asp:TextBox>
                              </td>
                               
                            <td width="50px" align="right">
                                <asp:Button ID="btnSearch" runat="server" CssClass="tj_bt" Width="50px" Text="查询"
                                    OnClick="btnSearch_Click" />
                              
                              </td>
                                         <td style="padding-left:10px">  <asp:Button ID="Button1" runat="server" CssClass="tj_bt" Width="50px"   Text="导出"
                                    OnClick="Button1_Click" /></td>
                                         <td></td>
                                     </tr>

                                 </table>
                             </td>
                           
                           
                        </tr>
                    </table>
                    <table width="100%" cellpadding="0" cellspacing="0" class="content_nr_hj">
                        <tr>
                            <td>
                                <span>经纪人收益产生明细表</span>
                                （金额单位：元）</td>
                        </tr>
                    </table>

                    <table cellspacing="0" cellpadding="0" border="1" bordercolor="#99BBE8" style="border-collapse: collapse;
                        table-layout: fixed; width: 100%;" class="tab">
                        <tr>
                            <td>
                                <div  style=" overflow-x: scroll; "  >
                                    <table id="theObjTable" style="width: 1640px;" cellspacing="0" cellpadding="0">
                                    <thead>
                                        <tr>
                                            <th class="TheadTh" style="width: 140px;" >
                                                收益产生时间
                                            </th>
                                            <th class="TheadTh" style="width: 100px;">
                                                业务管理部门
                                            </th>
                                            <th class="TheadTh" style="width: 100px;">
                                                经纪人编号
                                            </th>
                                            <th class="TheadTh" style="width: 80px;">
                                                经纪人名称
                                            </th>
                                            <th class="TheadTh" style="width: 80px;">
                                                经纪人类别
                                            </th>
                                            <th class="TheadTh" style="width: 100px;">
                                                关联交易方编号
                                            </th>
                                            <th class="TheadTh" style="width: 100px;">
                                                关联交易方名称
                                            </th>
                                             <th class="TheadTh" style="width: 100px;">
                                                关联交易方身份
                                            </th>
                                              <th class="TheadTh" style="width: 100px;">
                                                来源业务单号
                                            </th>
                                             <th class="TheadTh" style="width: 100px;">
                                                来源业务金额
                                            </th>
                                             <th class="TheadTh" style="width: 100px;">
                                                税前收益金额
                                            </th>
                                             <th class="TheadTh" style="width: 340px;">
                                                业务摘要
                                            </th>
                                              <th class="TheadTh" style="width: 100px;">
                                                收益来源
                                            </th>
                                            
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater ID="rptSPXX" runat="server">
                                            <ItemTemplate>
                                                <tr class="TbodyTr" >
                                                    <td style="width: 140px;">
                                                        <%#Eval("收益产生时间")%>
                                                    </td>
                                                    <td title='<%#Eval("业务管理部门")%>' style="width: 100px;">
                                                        <asp:Label ID="lbbuyer" runat="server" Text='<%#Eval("业务管理部门")%>'> </asp:Label>
                                                    </td>
                                                    <td title='<%#Eval("经纪人编号")%>' style="width: 100px;">
                                                        <asp:Label ID="lbseller" runat="server" Text='<%#Eval("经纪人编号")%>'> </asp:Label>
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <%#Eval("经纪人名称")%>
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <%#Eval("经纪人类别")%>
                                                    </td>
                                                    <td title='<%#Eval("关联交易方编号")%>' style="width: 100px;">
                                                        <asp:Label ID="lbspmc" runat="server" Text='<%#Eval("关联交易方编号")%>'> </asp:Label>
                                                    </td>
                                                    <td style="width: 100px;">
                                                        <%#Eval("关联交易方名称")%>
                                                    </td >
                                                    <td style="width: 100px;">
                                                        <%#Eval("关联交易方身份")%>
                                                    </td>
                                                     <td style="width: 100px;">
                                                        <%#Eval("来源业务单号")%>
                                                    </td>
                                                    <td style="width: 100px;">
                                                        <%#Eval("来源业务金额")%>
                                                    </td>
                                                    <td style="width: 100px;">
                                                        <%#Eval("税前收益金额")%>
                                                    </td>
                                                    <td style="width: 340px;">
                                                        <%#Eval("业务摘要")%>
                                                    </td>
                                                    <td style="width: 100px;">
                                                        <%#Eval("收益来源")%>
                                                    </td>
                                                   
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        <tr id="tdEmpty" runat="server" visible="false" style="text-align: center;">
                                            <td colspan="7">
                                                您查询的数据为空！
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                </div>
                            </td>
                        </tr>
                    </table> <uc1:commonpagernew ID="commonpagernew1" runat="server" />
                </div>
            </div>
        </div>
    </div>
    <input runat="server" id="hidID" type="hidden" />
        <input runat="server" id="hidwhere" type="hidden" />  
         <input runat="server" id="hidwhereis" type="hidden" />  
    </form>
</body>
</html>
