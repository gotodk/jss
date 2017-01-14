<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GXTWDetail.aspx.cs" Inherits="Web_JHJX_New2013_UCFWJG_GXTWDetail" %>

<%@ Register src="../../../pagerdemo/commonpagernew.ascx" tagname="commonpagernew" tagprefix="uc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
  
    <link href="../../../../css/standardStyle.css" rel="stylesheet" type="text/css" />
        <style type="text/css">
        /*内容正文div，用于控制内容宽度，取值为500px、919px、1083px;*/
        #content_zw
        {
            width: 650px;
        }
    </style>
    <script src="../../../jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/jquery.art_confirm.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/jquery.tablechangecolor.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            $("#" + "<%=txtKHBH.ClientID%>").focus();
            //收集数据
            $("span.spanRightClass").click(function () {
                //使用$.trim()是为了兼容FireFox和Chrome浏览器
                var servicesInfo = new ServiceInfo();
                servicesInfo.rowid = $.trim($(this).attr("spanid"));
                servicesInfo.Number = $.trim($(this).parent().parent().children("td").eq(1).html());
                servicesInfo.GXMC = $.trim($(this).parent().parent().children("td").eq(2).html());
                servicesInfo.CreateTime = $.trim($(this).parent().parent().children("td").eq(3).text());
                window.parent.frames['rightFrame'].eval($.Request("optionName")).IsReady(servicesInfo);
            });
            //收集数据
            $("#seleAll").click(function () {
                //使用$.trim()是为了兼容FireFox和Chrome浏览器
                var servicesInfo = new ServiceInfo();
                servicesInfo.rowid = "";
                servicesInfo.Number ="";
                servicesInfo.GXMC ="请选择";
                servicesInfo.CreateTime = "";
                window.parent.frames['rightFrame'].eval($.Request("optionName")).IsReady(servicesInfo);
            });
            $("#theObjTable").tablechangecolor();
        });

        //设定要返回的数据类
        function ServiceInfo() {
            //编号
            this.rowid = "";
            //唯一标识
            this.Number = "";
            //高校名称
            this.GXMC = "";
            //创建时间
            this.CreateTime;
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="new_content">
        <div id="new_zicontent">
            <div id="content_zw">
              <%--  <div class="content_bz">
                </div>--%>
                <div class="content_nr">
                    <table width="100%" cellpadding="0" cellspacing="0" class="content_nr_cx">
                        <tr>
                            <td width="80px" align="right">
                                高校名称：
                            </td>
                            <td width="80px">
                                 <asp:TextBox ID="txtKHBH" runat="server" aa="aa" CssClass="tj_input" Width="120px" Enabled="True"
                                    TabIndex="1"></asp:TextBox>
                            </td>
                            <td width="75px" align="right">
                             
                            </td>
                            <td width="120px">
                              <asp:Button ID="btnSearch" runat="server" CssClass="tj_bt" Text="查询" 
                                    UseSubmitBehavior="False" onclick="btnSearch_Click"  />
                            </td>
                            <td width="20px" align="center">
                               
                            </td>
                            <td width="60px" align="right">
                             
                            </td>
                            <td width="120px">
                                
                            </td>
                            <td align="center">
                                &nbsp;&nbsp;
                            </td>
                        </tr>
                    </table>
                    <table width="100%" cellpadding="0" cellspacing="0" class="content_nr_hj">
                        <tr>
                            <td>
                                高校信息列表&nbsp;&nbsp; <span style=" color:red; cursor:pointer;" id="seleAll"  onclick="">全选</span><br />
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
                                             选择
                                            </th>
                                            <th class="TheadTh" >
                                           编号
                                            </th>
                                            <th   class="TheadTh" >
                                            高校名称
                                            </th>
                                            <th  class="TheadTh" >
                                           创建时间
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater ID="Repeater1" runat="server" 
                                            >
                                            <ItemTemplate>                                            
                                               <tr  class="TbodyTr">
                                               <td  class="xiaoshou">
                                              <span class="spanRightClass"  spanid='<%#Eval("编号")%>'></span>
                                               </td>
                                                    <td>
                                                        <%#Eval("Number")%>
                                                    </td>
                                                    <td>
                                                        <%#Eval("高校名称")%>
                                                    </td>
                                                    <td>
                                                        <%#Eval("创建时间")%>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                          <tr id="tdEmpty" runat="server" visible="false"  style="text-align: center;">
                                            <td colspan="4">
                                                您查询的数据为空！
                                            </td>
                                        </tr>
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
