<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Import_YGFZJJRXX.aspx.cs" Inherits="Web_JHJX_Import_YGFZJJRXX" %>

<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>业务操作平台-员工发展经纪人信息导入</title>
    <link href="../../css/standardStyle.css" rel="stylesheet" />
    <script src="../../js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../../js/standardJSFile/art_confirm.js" type="text/javascript"></script>
    <script src="../../js/standardJSFile/jquery.tablechangecolor.js" type="text/javascript"></script>
    <script src="../../js/pingbi.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function () {
            $("#theObjTable").tablechangecolor();
        });
    </script>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            $("#btnConvert").click(function () {
                $("#lb1").show();
                $(".btnDisplay").hide();
            })
            var hiddenVal = $("#IsConvert").val();
            if (hiddenVal == "0") {
                $(".btnDisplay").hide();
                $("#lb1").show();
                $("#lb1").val("没有需要处理的数据").text("没有需要处理的数据");
            }
            if (hiddenVal == "3") {
                $(".btnDisplay").hide();
                $("#lb1").show();
                $("#lb1").val("数据验证全部失败，无需执行导入操作。").text("数据验证全部失败，无需执行导入操作");
            }
            $("#btnValidate").click(function () {
                $(".btnDisplay").hide();
                $("#lb2").val("正在验证信息，请稍后……").text("正在验证信息，请稍后……");
                $("#lb2").show();
            })
        })
    </script>
    <style type="text/css">
        #content_zw {
            width: 800px;
        }
    </style>
</head>
<body style="background-color: #f7f7f7;">
    <form id="form1" runat="server">
        <radTS:RadTabStrip ID="RadTabStrip1" runat="server" Height="25px" Skin="Default2006"
            BackColor="#F7F7F7">
            <Tabs>
                <radTS:Tab ID="Tab1" runat="server" NavigateUrl="Import_YGFZJJRXX.aspx" Text="员工发展经纪人信息导入"
                    ForeColor="Red">
                </radTS:Tab>
            </Tabs>
        </radTS:RadTabStrip>
        <div id="new_content">
            <div id="new_zicontent">
                <div id="content_zw">
                    <div class="content_bz">
                        说明：<br />
                        1、该模块用于将员工发展经纪人信息导入到对应表中。<br />
                        2、导入文档需为.xls或.xlsx格式（即excel表格）。<br />
                        3、excel表中必须包括“员工工号、员工姓名、发展经纪人登陆邮箱”三列。<br />
                        4、“验证信息”失败的，鼠标指向“验证结果”，系统会给出失败的具体原因。<br />
                        5、为防止因一次导入数据过多导致的系统异常，建议每次导入数据控制在100条之内。

                    </div>
                    <br />
                    <div class="content_nr">
                        <table>
                            <tr>
                                <td style="padding-left: 10px; height: 30px">
                                    <span style="font-size: 10pt">请选择需导入的Excel文件：</span>
                                </td>
                                <td>
                                    <asp:FileUpload
                                        ID="FileUpload2" runat="server" Width="412px" Height="25px" />
                                </td>
                                <td style="padding-left: 20px">
                                    <asp:Button ID="Button2" runat="server" Text="确定" CssClass="tj_bt" OnClick="Button2_Click" Width="60px" />&nbsp;&nbsp;
                                     <asp:Button ID="btnShuaXin" runat="server" Text="清空数据" CssClass="tj_bt" Width="80px"
                                         OnClick="btnShuaXin_Click" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <div style="width: 800px; text-align: center; vertical-align: middle;">
                            <asp:Label runat="server" ID="lb1" Style="display: none;">正在导入，请稍后……</asp:Label>
                            <asp:Label runat="server" ID="lb2" Style="display: none;">正在验证信息，请稍后……</asp:Label>
                            <div class="btnDisplay">
                                <asp:Button ID="btnValidate" runat="server" Text="验证信息" CssClass="tj_bt_da" Width="120px"
                                    OnClick="btnValidate_Click" Height="30px" />&nbsp;
                                 <asp:Button ID="btnExport" runat="server" Text="导出验证结果" CssClass="tj_bt_da" Width="120px"
                                    OnClick="btnExport_Click" Height="30px" />&nbsp;
                        <asp:Button ID="btnConvert" runat="server" Text="导入" CssClass="tj_bt_da" Width="120px"
                            OnClick="btnConvert_Click" Height="30px" />

                            </div>
                        </div>                     
                        <table width="100%" cellpadding="0" cellspacing="0" class="content_nr_hj">
                            <tr>
                                <td width="25%">导入数据预览
                                </td>
                            </tr>
                        </table>
                        <table cellspacing="0" cellpadding="0" style="border-collapse: collapse; table-layout: fixed; width: 800px; border: 1px,solid,#99BBE8;" class="tab">
                            <tr>
                                <td>
                                    <table id="theObjTable" style="width: 800px;" cellspacing="0" cellpadding="0">
                                        <thead>
                                            <tr>
                                                <th class="TheadTh" style="width: 100px;">员工工号
                                                </th>
                                                <th class="TheadTh" style="width: 100px;">员工姓名
                                                </th>
                                                <th class="TheadTh" style="width: 150px;">发展经纪人登陆邮箱
                                                </th>
                                                <th class="TheadTh" style="width: 200px;">发展经纪人交易方名称
                                                </th>
                                                <th class="TheadTh" style="width: 150px;">发展经纪人注册类别
                                                </th>

                                                <th class="TheadTh" style="width: 100px;">验证结果
                                                </th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:Repeater ID="rpt" runat="server" OnItemDataBound="rpt_ItemDataBound">
                                                <ItemTemplate>
                                                    <tr class="TbodyTr">
                                                        <td>
                                                            <asp:Label ID="lblYGBH" runat="server" Text='  <%#Eval("员工工号") %>'> </asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblYGXM" runat="server" Text='  <%#Eval("员工姓名") %>'> </asp:Label>
                                                        </td>
                                                        <td title='<%#Eval("发展经纪人邮箱")%>'>
                                                            <asp:Label ID="lblJJRYX" runat="server" Text='  <%#Eval("发展经纪人邮箱")%>'> </asp:Label>
                                                        </td>
                                                        <td title='<%#Eval("发展经纪人交易方名称")%>'>
                                                            <asp:Label ID="lblJJRJYFMC" runat="server" Text='  <%#Eval("发展经纪人交易方名称")%>'> </asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblJJRZCLB" runat="server" Text='  <%#Eval("发展经纪人注册类别")%>'> </asp:Label>
                                                        </td>
                                                        <td title='<%#Eval("备注")%>'>
                                                            <asp:Label ID="lblDui" runat="server" Visible='<%#Eval("数据验证").ToString()=="成功" %>' Style="font-weight: bold; font-size: 11pt; color: Green">√</asp:Label>
                                                            <asp:Label ID="lblCha" runat="server" Visible='<%#Eval("数据验证").ToString()=="失败" %>' Style="font-weight: bold; font-size: 11pt; color: Red">×</asp:Label>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                            <tr id="tdEmpty" runat="server" visible="false" style="text-align: center;">
                                                <td colspan="6">
                                                    <label runat="server" id="lblk">请选择需要处理的数据！</label>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <br />
                </div>
            </div>
        </div>
        <asp:HiddenField ID="IsConvert" runat="server" />
        <input runat="server" id="hidFileName" type="hidden" />
    </form>
</body>
</html>

