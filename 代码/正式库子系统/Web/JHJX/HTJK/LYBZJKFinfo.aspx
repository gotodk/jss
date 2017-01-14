<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LYBZJKFinfo.aspx.cs" Inherits="Web_JHJX_HTJK_LYBZJKFinfo" %>

<%@ Register Src="../../pagerdemo/commonpagernew.ascx" TagName="commonpagernew" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../../../../css/standardStyle.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        /*内容正文div，用于控制内容宽度，取值为500px、919px、1083px;*/
        #content_zw
        {
            width: 800px;
        }
    </style>
    <script src="../../jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../../../js/standardJSFile/jquery.art_confirm.js" type="text/javascript"></script>
    <script src="../../../js/standardJSFile/jquery.tablechangecolor.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            //收集数据
            $("span.spanRightClass").click(function () {
                //使用$.trim()是为了兼容FireFox和Chrome浏览器
                var servicesInfo = new ServiceInfo();
                servicesInfo.rowid = $.trim($(this).attr("spanid"));
                servicesInfo.Number = $.trim($(this).parent().parent().children("td").eq(1).html());
                servicesInfo.dyfgsmc = $.trim($(this).parent().parent().children("td").eq(2).html());
                servicesInfo.dwqc = $.trim($(this).parent().parent().children("td").eq(3).text());
                servicesInfo.khbh = $.trim($(this).parent().parent().children("td").eq(4).html());
                servicesInfo.szsf = $.trim($(this).parent().parent().children("td").eq(5).html());
                window.parent.frames['rightFrame'].eval($.Request("optionName")).IsReady(servicesInfo);
            });
            $("#theObjTable").tablechangecolor();
        });

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="new_content">
        <div id="new_zicontent">
            <div id="content_zw">
                <div class="content_bz">
                    说明：您正在查看合同编号为&nbsp;&nbsp;<span id="spanHTBH" runat="server" style="font-weight: bold"></span>&nbsp;&nbsp;的履约保证金扣罚信息。
                </div>
                <div class="content_nr">
                    <table width="100%" cellpadding="0" cellspacing="0" class="content_nr_hj">
                        <tr>
                            <td height="15px">
                            </td>
                        </tr>
                    </table>
                    <table width="100%" cellspacing="0" cellpadding="0" border="1" bordercolor="#99BBE8"
                        style="border-collapse: collapse;" class="tab">
                        <tr>
                            <td>
                                <table id="theObjTable" style="width: 700px;" cellspacing="0" cellpadding="0">
                                    <thead>
                                        <tr>
                                            <th class="TheadTh" style="width: 100px; text-align: center; line-height: 18px">
                                                扣罚时间
                                            </th>
                                            <th class="TheadTh" style="width: 80px; text-align: center; line-height: 18px">
                                                扣罚金额
                                            </th>
                                            <th class="TheadTh" style="width: 340px; text-align: center; line-height: 18px">
                                                扣罚原因
                                            </th>
                                            <th class="TheadTh" style="width: 140px; text-align: center; line-height: 18px">
                                                赔偿买家<br />
                                                账号
                                            </th>
                                            <th class="TheadTh" style="width: 140px; text-align: center; line-height: 18px">
                                                赔偿买家<br />
                                                名称
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater ID="Repeater1" runat="server">
                                            <ItemTemplate>
                                                <tr class="TbodyTr">
                                                    <td align="center" style="width: 100px; word-wrap: break-word;">
                                                        <div style="width: 100px; word-wrap: break-word; line-height: 18px">
                                                            <%#Eval("扣罚时间")%></div>
                                                    </td>
                                                    <td align="center" style="width: 80px; word-wrap: break-word;">
                                                        <div style="width: 80px; word-wrap: break-word; line-height: 18px">
                                                            <%#Eval("扣罚金额")%></div>
                                                    </td>
                                                    <td align="center" style="width: 340px; word-wrap: break-word;">
                                                        <div style="width: 340px; word-wrap: break-word; line-height: 18px">
                                                            <%#Eval("扣罚原因")%></div>
                                                    </td>
                                                    <td align="center" style="width: 140px; word-wrap: break-word;">
                                                        <div style="width: 140px; word-wrap: break-word; line-height: 18px">
                                                            <%#Eval("赔偿买家账号")%></div>
                                                    </td>
                                                    <td align="center" style="width: 140px; word-wrap: break-word;">
                                                        <div style="width: 140px; word-wrap: break-word; line-height: 18px">
                                                            <%#Eval("赔偿买家名称")%></div>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        <tr id="tdEmpty" runat="server" visible="false" style="text-align: center;">
                                            <td colspan="5">
                                                暂无任何数据！
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
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
