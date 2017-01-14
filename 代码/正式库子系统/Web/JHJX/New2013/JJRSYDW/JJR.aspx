<%@ Page Language="C#" AutoEventWireup="true" CodeFile="JJR.aspx.cs" Inherits="Web_JHJX_New2013_JJRSYDW_JJR" MaintainScrollPositionOnPostback="false" %>
<%@ Register src="../../../pagerdemo/commonpagernew.ascx" tagname="commonpagernew" tagprefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
  
    <link href="../../../../css/standardStyle.css" rel="stylesheet" type="text/css" />
        <style type="text/css">
        /*内容正文div，用于控制内容宽度，取值为500px、919px、1083px;*/
        #content_zw
        {
            width: 790px;
        }
    </style>
    <script src="../../../jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/jquery.art_confirm.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/jquery.tablechangecolor.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            $("#" + "<%=txtJJRMC.ClientID%>").focus();
            //收集数据
            $("span.spanRightClass").click(function () {
                 //使用$.trim()是为了兼容FireFox和Chrome浏览器
                var servicesInfo = new ServiceInfo();
                servicesInfo.JJRBH = $.trim($(this).attr("spanid"));
                servicesInfo.JJRMC = $.trim($(this).parent().parent().children("td").eq(2).html());
                servicesInfo.FGSMC = $.trim($(this).parent().parent().children("td").eq(3).html());
                servicesInfo.ZSY = $.trim($(this).parent().parent().children("td").eq(0).children("input[type='hidden']").eq(0).attr("Value"));
                servicesInfo.YZQSY = $.trim($(this).parent().parent().children("td").eq(0).children("input[type='hidden']").eq(1).attr("Value"));
                servicesInfo.QPSYJE = $.trim($(this).parent().parent().children("td").eq(4).html());
                servicesInfo.DSFCGZT = $.trim($(this).parent().parent().children("td").eq(5).children("span").html());
                window.parent.frames['rightFrame'].eval($.Request("optionName")).IsReady(servicesInfo);
            });
            $("#theObjTable").tablechangecolor();
        });

        //设定要返回的数据类
        function ServiceInfo() {
            //经纪人编号
            this.JJRBH = "";
            //经纪人名称
            this.JJRMC = "";
            //分公司名称
            this.FGSMC = "";
            //总收益
            this.ZSY = "";
            //已支取收益
            this.YZQSY = "";
            //缺票收益金额
            this.QPSYJE = "";
            //第三方存管状态
            this.DSFCGZT = "";
        }
    
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="new_content">
        <div id="new_zicontent">
            <div id="content_zw">
                <%--<div class="content_bz">
                    说明文字：<br />
                    1、该模块用于记录已签约服务商以个人名义打款的打款人信息以及与服务商的对应关系。<br />
                    2、保存时系统根据规则自动生成正式客户编号，打款人编号以"6"开头，用生成的编号录入ERP。<br />
                    3、“所属服务商编号”请填写本办事处销售渠道为“服务商”或“门店服务商”的客户编号。<br />
                </div>--%>
                <div class="content_nr">
                    <table width="100%" cellpadding="0" cellspacing="0" class="content_nr_cx">
                        <tr>
                            <td width="80px" align="right">
                                经纪人编号：
                            </td>
                            <td width="80px">
                                <asp:TextBox ID="txtJJRBH" runat="server" aa="aa" CssClass="tj_input" Width="120px" Enabled="True"
                                    TabIndex="1"></asp:TextBox>
                            
                            </td>
                            <td width="75px" align="right">
                               经纪人名称：
                            </td>
                            <td width="120px">
                                 <asp:TextBox ID="txtJJRMC" runat="server" aa="aa" CssClass="tj_input" Width="120px" Enabled="True"
                                    TabIndex="1"></asp:TextBox>
                            </td>
                            <td width="20px" align="center">
                               
                            </td>
                            <td width="60px" align="right">
                             
                            </td>
                            <td width="120px">
                                
                            </td>
                            <td align="center">
                                <asp:Button ID="btnSearch" runat="server" CssClass="tj_bt" Text="查询" 
                                    UseSubmitBehavior="False" onclick="btnSearch_Click"  />&nbsp;&nbsp;
                            </td>
                        </tr>
                    </table>
                    <table width="100%" cellpadding="0" cellspacing="0" class="content_nr_hj">
                        <tr>
                            <td>
                               <%-- 当前合计：共24242条数据，数量合计66492支，已交数量6558885。。<br />
                                其中，订单（包括普通订单、特殊支持、春雨行动）共计19744条，数据合计125844只，已提交12578。--%>
                            </td>
                        </tr>
                    </table>
                    <table width="100%" cellspacing="0" cellpadding="0" border="1" bordercolor="#99BBE8"
                        style="border-collapse: collapse;"  class="tab" >   
                        <tr>
                            <td>
                                      <div  id="exprot" runat="server" class="content_nr_lb">
                                          <table id="theObjTable" style=" width:100%;"  cellspacing="0" cellpadding="0">
                                    <thead>
                                        <tr>
                                          <th class="TheadTh"  >
                                             &nbsp;
                                            </th>
                                            <th class="TheadTh" >
                                            经纪人编号
                                            </th>
                                            <th class="TheadTh" >
                                            经纪人名称
                                            </th>
                                            <th class="TheadTh" >
                                            所属分公司
                                            </th>
                                            <th class="TheadTh" >
                                            缺票收益金额
                                            </th>
                                            <th class="TheadTh" >
                                            第三方存管状态
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater ID="Repeater1" runat="server" 
                                            >
                                            <ItemTemplate>                                            
                                               <tr  class="TbodyTr">
                                               <td  class="xiaoshou">
                                              <span class="spanRightClass"  spanid='<%#Eval("经纪人编号")%>'></span>
                                                   <%--<span  hidZSYValue='<%#Eval("总收益")%>' style="visibility:hidden" ></span>
                                                   <span hidYZQSYValue='<%#Eval("已支取收益")%>'  style="visibility:hidden"></span>--%>
                                                   <asp:HiddenField ID="hidZSY" runat="server" Value='<%#Eval("总收益")%>' />
                                                   <asp:HiddenField ID="hidYZQSY" runat="server" Value='<%#Eval("已支取收益")%>' />
                                               </td>
                                                    <td>
                                                        <%#Eval("经纪人编号")%>
                                                    </td>
                                                    <td>
                                                        <%#Eval("经纪人名称")%>
                                                    </td>
                                                    <td>
                                                        <%#Eval("所属分公司")%>
                                                    </td>
                                                    <td>
                                                      <%#Eval("缺票收益金额")%>
                                                    </td>
                                                   <td>
                                                      <span id="spandsf" spanval='<%#Eval("第三方存管状态")%>'><%#Eval("第三方存管状态")%></span>&nbsp;
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                             </tbody>
                                </table>
                                        </div>
                              <uc1:commonpagernew ID="commonpagernew1" runat="server" />
                             
                            </td>
                        </tr>    
                    </table>
                </div>
            </div>
        </div>
        
        
    </div>
    </form>
</body>
</html>
