<%@ Page Language="C#" AutoEventWireup="true" CodeFile="YDJZHGL_info.aspx.cs" Inherits="Web_JHJX_New2013_JHJX_UserDongJie_YDJZHGL_info" %>

<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
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
            <radTS:Tab ID="Tab1" runat="server" NavigateUrl="jhjx_UserDJ.aspx" Text="交易账户冻结"
                ForeColor="Red" Font-Size="12px">
            </radTS:Tab>
        </Tabs>
    </radTS:RadTabStrip>
    <div id="new_content">
        <div id="new_zicontent">
            <div id="content_zw">
                <div class="content_nr">
                    <table width="800px" class="Message">
                        <thead>
                            <tr>
                                <th class="TitleTh" style="padding-left: 15px; width: 100px">
                                    交易方信息 <span runat="server" id="spanNumber" visible="false"></span>
                                </th>
                                <th colspan="3" style="width: 700px; text-align: left; font-size: 9pt; color: Red">
                                    <span runat="server" id="spanFPBZ"></span>
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td style="width: 100px; text-align: right; height: 25px">
                                    交易方账号：
                                </td>
                                <td style="text-align: left; width: 270px">
                                    <asp:Label ID="labzhzh" runat="server" Text=""></asp:Label>
                                </td>
                                <td style="width: 130px; text-align: right;">
                                    当前关联经纪人：
                                </td>
                                <td style="text-align: left; width: 300px">
                                    <asp:Label ID="labgljjr" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; height: 25px">
                                    交易账户类型：
                                </td>
                                <td style="text-align: left;">
                                    <asp:Label ID="labzhlx" runat="server" Text=""></asp:Label>
                                </td>
                                <td style="text-align: right;">
                                    注册类别：
                                </td>
                                <td style="text-align: left;">
                                    <asp:Label ID="labzclb" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; height: 25px">
                                    联系人姓名：
                                </td>
                                <td style="text-align: left;">
                                    <asp:Label ID="lablxr" runat="server" Text=""></asp:Label>
                                </td>
                                <td style="overflow: hidden; text-align: right;">
                                    联系人手机号：
                                </td>
                                <td style="text-align: left;">
                                    <asp:Label ID="lablxdh" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; height: 25px">
                                    交易方名称：
                                </td>
                                <td style="text-align: left;">
                                    <asp:Label ID="labmc" runat="server" Text=""></asp:Label>
                                </td>
                                <td style="text-align: right;">
                                    交易方联系电话：
                                </td>
                                <td style="text-align: left;">
                                    <asp:Label ID="labjyflxdh" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; height: 25px">
                                    所属区域：
                                </td>
                                <td style="text-align: left;">
                                    <asp:Label ID="labzcd" runat="server" Text=""></asp:Label>
                                </td>
                                <td style="text-align: right;">
                                    平台管理机构：
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
                            <th class="TitleTh" style="padding-left: 15px; width: 100px">
                                冻结信息
                            </th>
                            <th colspan ="3" style="text-align: left; font-size: 9pt; color: Red">
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
                                        <%--<asp:ListItem>可投标商品申请</asp:ListItem>--%>
                                        <asp:ListItem>投标单</asp:ListItem>
                                        <asp:ListItem>预订单</asp:ListItem>
                                        <asp:ListItem>下达提货单</asp:ListItem>
                                    </asp:CheckBoxList>
                                </td>                            
                                <td style="text-align: right; width :100px">
                                    <span class="span">*</span>冻结原因：
                                </td>
                                <td style="text-align: left; width :350px">
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
                                <td style="text-align: right; height:30px">
                                    <span class="span">*</span>上传凭证：
                                </td>
                                <td style="text-align: left;" colspan="3">
                                    <asp:FileUpload ID="FileUpload1" runat="server" Width="580px" />
                                    <asp:Button ID="Btnupload" runat="server" Text="上传" CssClass="button" Height="18px"
                                        OnClick="Btnupload_Click" />
                                </td>
                            </tr>
                            <tr runat="server" id="fujian">
                                <td style="text-align: right; height:30px">
                                    附件：
                                </td>
                                <td colspan ="3" align ="left" >
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
                                <td style="width: 100px; height: 50px">
                                </td>
                                <td style="text-align: left;" colspan="3">
                                    <asp:Button ID="btnQRXG" runat="server" CssClass="tj_bt_da" Text="确定修改" Height="30"
                                        Width="80" OnClick="btnQRXG_Click" OnClientClick="javascript:return confirm('您确定要提交修改信息吗？');" />
                                    &nbsp;&nbsp;
                                    <asp:Button ID="btnJieDong" runat="server" CssClass="tj_bt_da" Text="解冻" Height="30"
                                        Width="80" OnClick="btnJieDong_Click" OnClientClick="javascript:return confirm('您确定要对该交易账户执行解冻操作吗？');" />
                                    &nbsp;&nbsp;
                                    <asp:Button ID="btnCancel" runat="server" CssClass="tj_bt_da" Text="返回列表" Height="30"
                                        Width="80" OnClick="btnCancel_Click" OnClientClick="javascript:return confirm('您确定要返回列表页面吗？');" />
                                </td>
                            </tr>
                        </tfoot>
                    </table>
                    <input runat="server" id="hidfilepath" type="hidden" />
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
