<%@ Page Language="C#" AutoEventWireup="true" CodeFile="jhjx_UserDJ.aspx.cs" Inherits="Web_JHJX_New2013_JHJX_UserDongJie_jhjx_UserDJ" %>

<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<%@ Register Src="../../../pagerdemo/commonpager.ascx" TagName="commonpager" TagPrefix="uc1" %>
<%@ Register Src="../../../pagerdemo/commonpagernew.ascx" TagName="commonpagernew"
    TagPrefix="uc1" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>交易账户冻结</title>
    <script src="../../../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <link href="../../../../css/style.css" rel="Stylesheet" type="text/css" />
    <script src="../../../../js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/art_confirm.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/jquery.art_confirm.js" type="text/javascript"></script>
    <link href="../../../../css/standardStyle.css" rel="stylesheet" type="text/css" />
    <script src="../../../../js/standardJSFile/jquery.tablechangecolor.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/fcf.js" type="text/javascript"></script>
    <script type="text/javascript">
        //js本地图片预览，兼容ie[6-9]、火狐、Chrome17+、Opera11+、Maxthon3
        function PreviewImage(fileObj, imgPreviewId, divPreviewId) {
            var allowExtention = ".jpg,.bmp,.gif,.png"; //允许上传文件的后缀名document.getElementById("hfAllowPicSuffix").value;
            var extention = fileObj.value.substring(fileObj.value.lastIndexOf(".") + 1).toLowerCase();
            var browserVersion = window.navigator.userAgent.toUpperCase();
            if (allowExtention.indexOf(extention) > -1) {
                if (fileObj.files) {//HTML5实现预览，兼容chrome、火狐7+等
                    if (window.FileReader) {
                        var reader = new FileReader();
                        reader.onload = function (e) {
                            document.getElementById(imgPreviewId).setAttribute("src", e.target.result);
                        }
                        reader.readAsDataURL(fileObj.files[0]);
                    } else if (browserVersion.indexOf("SAFARI") > -1) {
                        alert("不支持Safari6.0以下浏览器的图片预览!");
                    }
                } else if (browserVersion.indexOf("MSIE") > -1) {
                    if (browserVersion.indexOf("MSIE 6") > -1) {//ie6
                        document.getElementById(imgPreviewId).setAttribute("src", fileObj.value);
                    } else {//ie[7-9]
                        fileObj.select();
                        if (browserVersion.indexOf("MSIE 9") > -1)
                            fileObj.blur(); //不加上document.selection.createRange().text在ie9会拒绝访问
                        var newPreview = document.getElementById(divPreviewId + "New");
                        if (newPreview == null) {
                            newPreview = document.createElement("div");
                            newPreview.setAttribute("id", divPreviewId + "New");
                            newPreview.style.width = document.getElementById(imgPreviewId).width + "px";
                            newPreview.style.height = document.getElementById(imgPreviewId).height + "px";
                            newPreview.style.border = "solid 1px #d2e2e2";
                        }
                        newPreview.style.filter = "progid:DXImageTransform.Microsoft.AlphaImageLoader(sizingMethod='scale',src='" + document.selection.createRange().text + "')";
                        var tempDivPreview = document.getElementById(divPreviewId);
                        tempDivPreview.parentNode.insertBefore(newPreview, tempDivPreview);
                        tempDivPreview.style.display = "none";
                    }
                } else if (browserVersion.indexOf("FIREFOX") > -1) {//firefox
                    var firefoxVersion = parseFloat(browserVersion.toLowerCase().match(/firefox\/([\d.]+)/)[1]);
                    if (firefoxVersion < 7) {//firefox7以下版本
                        document.getElementById(imgPreviewId).setAttribute("src", fileObj.files[0].getAsDataURL());
                    } else {//firefox7.0+                    
                        document.getElementById(imgPreviewId).setAttribute("src", window.URL.createObjectURL(fileObj.files[0]));
                    }
                } else {
                    document.getElementById(imgPreviewId).setAttribute("src", fileObj.value);
                }
            } else {
                alert("仅支持" + allowExtention + "为后缀名的文件!");
                fileObj.value = ""; //清空选中文件
                if (browserVersion.indexOf("MSIE") > -1) {
                    fileObj.select();
                    document.selection.clear();
                }
                fileObj.outerHTML = fileObj.outerHTML;
            }
        }
        function Promot(this_an) {
            return art_confirm_fcf(this_an, "您确定要执行此操作吗！此操作执行的结果将不可恢复！", 'clickyc()');
        }
    </script>
</head>
<body style="background-color: #f7f7f7;">
    <form id="form1" runat="server" style="margin: 0 0 0 0;">
        <span id="span"></span>
        <asp:HiddenField ID="hidSYZJE" runat="server" />
        <asp:HiddenField ID="hidYZQJE" runat="server" />
        <radTS:RadTabStrip ID="TabStrip1" runat="server" CausesValidation="False" ReorderTabRows="True"
            Skin="Default2006" Width="99%" BackColor="#f7f7f7">
            <Tabs>
                <radTS:Tab ID="Tab1" runat="server" NavigateUrl="jhjx_UserDJ.aspx" Text="交易账户冻结受理"
                    ForeColor="Red" Font-Size="12px">
                </radTS:Tab>
            </Tabs>
        </radTS:RadTabStrip>
        <div id="new_content">
            <div id="new_zicontent">
                <div id="content_zw">
                    <div class="content_nr">
                        <table width="800px" class="Message">
                            <tbody>
                                <tr>
                                    <td style="text-align: right; width: 120px">交易方帐号：
                                    </td>
                                    <td style="text-align: left; width: 200px">
                                        <asp:TextBox ID="txtjyfzh" runat="server" CssClass="tj_input" Width="200px"></asp:TextBox>
                                    </td>
                                    <td style="text-align: right; width: 120px">交易方名称：
                                    </td>
                                    <td width="240px">
                                        <asp:TextBox ID="txtjyfmc" runat="server" CssClass="tj_input" Width="200px"></asp:TextBox>
                                    </td>
                                    <td width="120px" align="left">
                                        <asp:Button ID="btn_check" runat="server" CssClass="tj_bt" Text="查询"
                                            OnClick="btn_check_Click" Width="50px" />
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <div class="content_lx" style="width: 800px">
                        </div>
                        <table width="800px" cellspacing="0" cellpadding="0" border="1" bordercolor="#99BBE8"
                            style="border-collapse: collapse;" class="tab">
                            <tr>
                                <td>
                                    <div class="content_nr_lb" style="width: 1100px;">
                                        <table id="theObjTable" style="width: 1340px;" cellspacing="0" cellpadding="0">
                                            <thead>
                                                <tr>
                                                    <th class="TheadTh" style="width: 150px; line-height: 18px;">交易方账号</th>
                                                    <th class="TheadTh" style="width: 100px;">交易方编号</th>
                                                    <th class="TheadTh" style="width: 120px;">账户类型</th>
                                                    <th class="TheadTh" style="width: 150px;">交易方名称</th>
                                                    <th class="TheadTh" style="width: 80px;">注册类别
                                                    </th>
                                                    <th class="TheadTh" style="width: 120px; line-height: 18px;">分公司<br />
                                                        审核时间
                                                    </th>
                                                    <th class="TheadTh" style="width: 120px; line-height: 18px;">服务中心<br />
                                                        审核时间</th>
                                                    <th class="TheadTh" style="width: 100px;">平台管理机构</th>
                                                    <th class="TheadTh" style="width: 100px;">联系人姓名</th>
                                                    <th class="TheadTh" style="width: 100px;">联系人手机号</th>
                                                    <th class="TheadTh" style="width: 200px;">操作</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <asp:Repeater ID="Repeater1" runat="server" OnItemCommand="Repeater1_ItemCommand">
                                                    <ItemTemplate>
                                                        <tr class="TbodyTr">
                                                            <td style="width: 150px;" title='<%#Eval("交易方账号") %>'>
                                                                <div style="word-wrap: break-word; line-height: 18px; width: 150px;"><%#Eval("交易方账号").ToString().Length > 20 ? Eval("交易方账号").ToString().Substring(0, 20) + "..." : Eval("交易方账号").ToString()%></div>
                                                            </td>
                                                            <td style="width: 100px;" title='<%#Eval("交易方编号") %>'>
                                                                <div style="word-wrap: break-word; line-height: 18px; width: 100px;"><%#Eval("交易方编号").ToString().Length > 14 ? Eval("交易方编号").ToString().Substring(0, 14) + "..." : Eval("交易方编号").ToString()%></div>
                                                            </td>
                                                            <td>
                                                                <%#Eval("账户类型")%>
                                                            </td>
                                                            <td title='<%#Eval("交易方名称") %>'>
                                                                <div style="word-wrap: break-word; line-height: 18px; width: 150px;"><%#Eval("交易方名称").ToString().Length > 10 ? Eval("交易方名称").ToString().Substring(0, 10) + "..." : Eval("交易方名称").ToString()%></div>
                                                            </td>
                                                            <td>
                                                                <%#Eval("注册类别") %>
                                                            </td>
                                                            <td>
                                                                <%#Eval("分公司审核时间")%>
                                                            </td>
                                                            <td style="width: 120px;">
                                                                <div style="word-wrap: break-word; line-height: 18px; width: 120px;"><%#Eval("服务中心审核时间")%></div>
                                                            </td>
                                                            <td>
                                                                <%#Eval("平台管理机构")%>
                                                            </td>
                                                            <td title='<%#Eval("联系人姓名") %>'>
                                                                <div style="word-wrap: break-word; line-height: 18px; width: 100px;"><%#Eval("联系人姓名").ToString().Length > 7 ? Eval("联系人姓名").ToString().Substring(0, 7) + "..." : Eval("联系人姓名").ToString()%></div>
                                                            </td>
                                                            <td>
                                                                <%#Eval("联系人手机号")%>
                                                            </td>
                                                            <td>
                                                                <asp:LinkButton ID="btnCK" runat="server" CommandName='<%#Eval("Number") %>' CommandArgument="ck">查看详情</asp:LinkButton>
                                                                &nbsp;&nbsp;
                                                            <asp:LinkButton ID="LinkButton1" runat="server" CommandName='<%#Eval("Number")+"&"+Eval("交易方账号") %>' CommandArgument="tydj">同意冻结</asp:LinkButton>
                                                                &nbsp;&nbsp;
                                                            <asp:LinkButton ID="LinkButton2" runat="server" CommandName='<%#Eval("Number")+"&"+Eval("交易方账号") %>' CommandArgument="bydj">不予冻结</asp:LinkButton>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </tbody>
                                            <tfoot>
                                                <tr id="ts" runat="server" class="TfootTr">
                                                    <td colspan="11" align="center">当前数据为空！
                                                    </td>
                                                </tr>
                                            </tfoot>
                                        </table>
                                    </div>
                                    <uc1:commonpagernew ID="commonpagernew1" runat="server" />
                                </td>
                            </tr>
                        </table>

                        <div runat="server" id="divinfo" visible="false">
                            <table width="800px" class="Message">
                                <thead>
                                    <tr>
                                        <th class="TitleTh" style="padding-left: 15px; width: 100px">交易方信息 <span runat="server" id="spanNumber" visible="false"></span>
                                        </th>
                                        <th colspan="3" style="width: 700px; text-align: left; font-size: 9pt; color: Red">
                                            <span runat="server" id="spanFPBZ"></span>
                                            <span runat="server" id="spanJYZHZSBNuber" visible="false"></span>
                                        </th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td style="width: 110px; text-align: right; height: 25px">交易方账号：</td>
                                        <td style="text-align: left; width: 270px">
                                            <asp:Label ID="labzhzh" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td style="width: 130px; text-align: right;">当前关联经纪人：
                                        </td>
                                        <td style="text-align: left; width: 300px">
                                            <asp:Label ID="labgljjr" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right; height: 25px">交易账户类型：
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:Label ID="labzhlx" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td style="text-align: right;">注册类别：
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:Label ID="labzclb" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right; height: 25px">联系人姓名：
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:Label ID="lablxr" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td style="overflow: hidden; text-align: right;">联系人手机号：
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:Label ID="lablxdh" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right; height: 25px">交易方名称：
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:Label ID="labmc" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td style="text-align: right;">交易方联系电话：
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:Label ID="labjyflxdh" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right; height: 25px">所属区域：
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:Label ID="labzcd" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td style="text-align: right;">平台管理机构：
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:Label ID="labssfgs" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                            <div class="content_lx" style="width: 800px">
                            </div>
                            <table width="800px" class="Message" id="tableZYFPXX">
                                <tr>
                                    <th class="TitleTh" style="padding-left: 15px; width: 100px">冻结信息
                                    </th>
                                    <th colspan="3" style="text-align: left; font-size: 9pt; color: Red">
                                        <span runat="server" id="spanFPJSBZ"></span>
                                    </th>
                                </tr>
                                <tbody>
                                    <tr>
                                        <td style="width: 100px; text-align: right; vertical-align: central;">
                                            <span class="span">*</span>冻结功能：
                                        </td>
                                        <td style="text-align: left; width: 250px">
                                            <asp:CheckBoxList ID="CheckB_djgn" runat="server">
                                                <asp:ListItem>经纪人暂停代理新业务</asp:ListItem>
                                                <asp:ListItem>经纪人暂停用户新业务</asp:ListItem>
                                                <asp:ListItem>出金</asp:ListItem>
                                                <%--  <asp:ListItem>可投标商品申请</asp:ListItem>--%>
                                                <asp:ListItem>投标单</asp:ListItem>
                                                <asp:ListItem>预订单</asp:ListItem>
                                                <asp:ListItem>下达提货单</asp:ListItem>
                                            </asp:CheckBoxList>
                                        </td>
                                        <td style="text-align: right; width: 100px">
                                            <span class="span">*</span>冻结原因：
                                        </td>
                                        <td style="text-align: left; width: 350px">
                                            <asp:CheckBoxList ID="CheckB_djyy" runat="server">
                                                <asp:ListItem>资料虚假</asp:ListItem>
                                                <asp:ListItem>违规行为</asp:ListItem>
                                                <asp:ListItem>经济纠纷</asp:ListItem>
                                                <asp:ListItem>执法机关要求</asp:ListItem>
                                                <asp:ListItem>其他</asp:ListItem>
                                            </asp:CheckBoxList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right; height: 30px">
                                            <span class="span">*</span>上传凭证：
                                        </td>
                                        <td style="text-align: left;" colspan="3">
                                            <asp:FileUpload ID="FileUpload1" runat="server" Width="580px" Height="20px" />
                                            <asp:Button ID="Btnupload" runat="server" Text="上传" CssClass="button" Height="20px"
                                                OnClick="Btnupload_Click" />
                                            <asp:Button ID="btnyl" runat="server" Text="预览" CssClass="button" Height="20px" OnClick="btnyl_Click"
                                                Visible="false" />
                                        </td>
                                    </tr>
                                    <tr runat="server" id="fujian" visible="false">
                                        <td style="text-align: right; height: 30px">附件：
                                        </td>
                                        <td align="left" colspan="3">
                                            <asp:Label ID="Lablsit" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right; vertical-align: central;">
                                            <span class="span">*</span>原因说明：
                                        </td>
                                        <td style="text-align: left;" colspan="3">
                                            <asp:TextBox ID="txtBZ" runat="server" class="tj_input" Width="518px" Height="80px"
                                                TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                    </tr>
                                </tbody>
                                <tfoot>
                                    <tr>
                                        <td style="width: 100px; height: 50px"></td>
                                        <td style="text-align: left; padding-left: 150px" colspan="3">
                                            <div id="djycqymain" style="margin-top: 10px;">
                                                <div id="djycqy_show">
                                                    <asp:Button ID="Button1" runat="server" CssClass="tj_bt_da" Text="同意冻结" OnClientClick="return Promot(this);"
                                                        Height="30px" Width="80px" OnClick="Button1_Click" Visible="false" />
                                                    <asp:Button ID="btnSave" runat="server" CssClass="tj_bt_da" Text="冻结" OnClientClick="return Promot(this);"
                                                        OnClick="btnSave_Click" Height="30px" Width="80px" />
                                                    <asp:Button ID="btnBYDJ" runat="server" CssClass="tj_bt_da" Text="不予冻结" OnClientClick="return Promot(this);"
                                                        Height="30px" Width="80px" Visible="false" OnClick="btnBYDJ_Click" />
                                                    &nbsp;&nbsp; &nbsp;&nbsp;
                                                    <asp:Button ID="btnReset" runat="server" CssClass="tj_bt_da" Text="取消" OnClientClick="javascript:return confirm('您确定要取消吗？');"
                                                        OnClick="btnReset_Click" Height="30px" Width="80px" />
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </tfoot>
                            </table>
                            <br />
                            <table width="100%" class="Message">
                                <tbody>
                                    <tr>
                                        <td colspan="2">
                                            <table width="100%">
                                                <tr>
                                                    <td style="width: 10px; text-align: left; vertical-align: top;"></td>
                                                    <td style="width: 140px; overflow: hidden; text-align: right; vertical-align: top;"></td>
                                                    <td style="text-align: left; vertical-align: top;"></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        <input runat="server" id="hidfilepath" type="hidden" />
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
