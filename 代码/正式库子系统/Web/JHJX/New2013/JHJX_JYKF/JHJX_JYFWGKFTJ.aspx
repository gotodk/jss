<%@ Page Language="C#" AutoEventWireup="true" CodeFile="JHJX_JYFWGKFTJ.aspx.cs" Inherits="Web_JHJX_JHJX_JYFWGKFTJ" %>

<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
     <script src="../../../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <link href="../../../../css/style.css" rel="Stylesheet" type="text/css" />
    <script src="../../../../js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/art_confirm.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/jquery.art_confirm.js" type="text/javascript"></script>
    <link href="../../../../css/standardStyle.css" rel="stylesheet" type="text/css" />
    <script src="../../../../js/standardJSFile/jquery.tablechangecolor.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/fcf.js" type="text/javascript"></script>
     <script type="text/javascript" language="javascript">
         //浮点数
         function CheckInputIntFloat(oInput) {
             if ('' != oInput.value.replace(/\d{1,}\.{0,1}\d{0,2}/, '')) {
                 oInput.value = oInput.value.match(/\d{1,}\.{0,1}\d{0,2}/) == null ? '' : oInput.value.match(/\d{1,}\.{0,1}\d{0,2}/);
             }
         }
    </script>

    <style type="text/css">
        #theObjTable
        {
            width: 666px;
        }
    </style>
         <script type="text/javascript">
             //js本地图片预览，兼容ie[6-9]、火狐、Chrome17+、Opera11+、Maxthon3
             function PreviewImage(fileObj, imgPreviewId, divPreviewId) {
                 var allowExtention = ".jpg,.bmp,.gif,.png";//允许上传文件的后缀名document.getElementById("hfAllowPicSuffix").value;
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
                                 fileObj.blur();//不加上document.selection.createRange().text在ie9会拒绝访问
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
                     fileObj.value = "";//清空选中文件
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
    <form id="form1" runat="server">
    <radTS:RadTabStrip ID="TabStrip1" runat="server" CausesValidation="False" ReorderTabRows="True"
        Skin="Default2006" Width="99%" BackColor="#f7f7f7">
        <Tabs>
            <radTS:Tab ID="Tab1" runat="server" NavigateUrl="JHJX_JYFWGKFTJ.aspx" ForeColor="red"
                Text="交易方违规扣罚提交">
            </radTS:Tab>           
        </Tabs>
    </radTS:RadTabStrip>
    <div id="new_content">
        <div id="new_zicontent">
            <div id="content_zw">
                <%-- <div class="content_bz">
                    1、该模块用于添加新的商品条目。<br />
                </div>--%>
                <div class="content_nr">
                    <table width="700px" class="Message">
                        <thead>
                            <tr>
                                <th class="TitleTh" colspan="4" style="padding-left: 15px">
                                    交易方基本信息
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td style="width: 120px; text-align: right; height: 25px">
                                    交易方账号：
                                </td>
                                <td style="text-align: left; width: 150px">
                                    
                            <asp:TextBox ID="txtjyfzh" runat="server" Width="150px" CssClass="tj_input"  ></asp:TextBox>  
                                  
                                </td>
                                <td  style="text-align: right;">
                                    交易方名称：</td>
                                <td style="text-align: left; width: 300px">
                                    
                            <asp:TextBox ID="txtjyfmc" runat="server" Width="150px" CssClass="tj_input" ></asp:TextBox>  
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="BtnCheck" runat="server" CssClass="tj_bt" UseSubmitBehavior="False"
                                    Text="查询"  onclick="BtnCheck_Click" Width="80px" />
                                </td>
                            </tr>
                            </tbody>
                        </table>
                        <table width="700px" class="Message" id="tabf" runat="server" visible="false">
                            
                            <tr>
                                <td style="text-align: right;width:120px; height: 25px">
                                    交易方账号：
                                </td>
                                 <td style="text-align: left; width: 150px">
                                     <asp:Label ID="lbjyfzh" runat="server" ></asp:Label>
                                </td>
                                <td  style="text-align: right;">
                                    交易方名称： </td>
                                <td style="text-align: left; width: 300px">
                                   <asp:Label ID="lbjyfmc" runat="server" ></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; height: 25px">
                                    交易账户类型：
                                </td>
                                <td style="text-align: left;">
                                    <asp:Label ID="lbjyzhlx" runat="server" ></asp:Label>
                                </td>
                                <td style="text-align: right;">
                                    注册类别：
                                </td>
                                <td style="text-align: left;">
                                    <asp:Label ID="lbzclb" runat="server" ></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; height: 25px">
                                    联系人姓名：
                                </td>
                                <td style="text-align: left;">
                                   <asp:Label ID="lblxrxm" runat="server" ></asp:Label>
                                </td>
                                <td style="text-align: right;">
                                    联系人手机号：
                                </td>
                                <td style="text-align: left;">
                                    <asp:Label ID="lblxrsjh" runat="server" ></asp:Label>
                                </td>
                            </tr>
                             <tr>
                                <td style="text-align: right; height: 25px">
                                    所属区域：
                                </td>
                                <td style="text-align: left;">
                                    <asp:Label ID="lbssqy" runat="server" ></asp:Label>
                                </td>
                                <td style="text-align: right;">
                                    关联经济人账号：
                                </td>
                                <td style="text-align: left;">
                                    <asp:Label ID="lbgljjrzh" runat="server" ></asp:Label>
                                </td>
                            </tr>
                             <tr>
                                <td style="text-align: right; height: 25px">
                                    所属分公司：
                                </td>
                                <td style="text-align: left;">
                                    <asp:Label ID="lbssfgs" runat="server" ></asp:Label>
                                </td>
                                <td style="text-align: right;">
                                    &nbsp;</td>
                                <td style="text-align: left;">
                                    
                                    &nbsp;</td>
                            </tr>
                        
                    </table>
                    <div class="content_lx" style="width: 700px">
                    </div>
                    <table width="700px" class="Message"  id="tabn" runat="server" visible="false">
                        <thead>
                            <tr>
                                <th class="TitleTh" colspan="2" style="padding-left: 15px">
                                    违规事项
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td style="text-align: right; height: 25px;width:120px">
                                    违规事项：
                                </td>
                                 <td style="text-align: left; width: 150px">
                                    
                            <asp:DropDownList ID="ddlwgsx" runat="server" CssClass="tj_input" Width="150px" >
                                    
                                </asp:DropDownList>
                                    
                                </td>
                                <td  style="text-align: right;">
                                    扣罚账户类别：</td>
                                <td style="text-align: left; width: 300px">     
                                    
                                <asp:DropDownList ID="ddlzhlb" runat="server" CssClass="tj_input" Width="150px" >
                                    
                                </asp:DropDownList>
                                    
                                    
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; height: 25px;width:120px">凭证上传：</td>
                                <td colspan="3">
                                    
                                    <asp:FileUpload ID="FileUpload1" runat="server" Width="428px" />
                                    
                                &nbsp;&nbsp;&nbsp;&nbsp;
                                    
                                <asp:Button ID="btnsc" runat="server" CssClass="button" UseSubmitBehavior="False"
                                    Text="上传"  onclick="BtnSC_Click" Width="45px" /> 
                                    
                                    &nbsp; 
                                   
                                <asp:Button ID="btnqc" runat="server" CssClass="button" UseSubmitBehavior="False"
                                    Text="删除"  onclick="BtnQC_Click" Width="45px" /> 
                                    &nbsp; 
                                                                        
                                </td>
                            </tr>
                            <tr runat="server" id="trpz" visible="false">
                                <td style="text-align:right">附件：</td>
                                <td colspan="3"> 
                                    <asp:Label ID="Lablsit" runat="server"></asp:Label>
                                    
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; height: 25px; width: 120px">
                                    交易方扣罚金额： </td>
                                 <td style="text-align: left;">                                    
                                    
                            <asp:TextBox ID="txtjyfkfje" runat="server" Width="150px" CssClass="tj_input" Style="text-align: left; ime-mode: Disabled;"  onKeypress="return (/[\d.]/.test(String.fromCharCode(event.keyCode)))"  onkeyup="javascript:CheckInputIntFloat(this);" onbeforepaste="clipboardData.setData('text',clipboardData.getData('text').replace(/[^\d]\.?/g,''))" ></asp:TextBox>                                    
                                    
                                </td>
                                <td  style="text-align: right;" runat="server" id="tdjjrkf">
                                    经纪人扣罚金额： </td>
                                <td style="text-align: left; ">                                  
                            <asp:TextBox ID="txtjjrkfje" runat="server" Width="150px" CssClass="tj_input" Style="text-align: left; ime-mode: Disabled;"   onKeypress="return (/[\d.]/.test(String.fromCharCode(event.keyCode)))" onkeyup="javascript:CheckInputIntFloat(this);" onbeforepaste="clipboardData.setData('text',clipboardData.getData('text').replace(/[^\d]\.?/g,''))"></asp:TextBox>  
                                
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <table width="700px" class="Message" runat="server" id="tab2" visible="false" >
                        <tbody>
                            <tr>
                                <td style="width: 120px; text-align: right; height: 100px" >
                                    &nbsp;情况简述：
                                </td>
                                <td style="text-align: left; ">
                                    
                                    <asp:TextBox ID="txtqkjs" runat="server" CssClass="tj_input" Width="541px" Enabled="True"
                                        Height="100px" MaxLength="150" TextMode="MultiLine"></asp:TextBox>
                                  
                                </td>
                            </tr>                           
                        </tbody>
                    </table>
                  
                    
                    <table width="700px" class="Message" id="tab3" runat="server" visible="false">
                        
                        <tbody> 
                            <tr>
                                <td><div class="content_lx" style="width: 700px" >
                    </div></td>
                            </tr>                           
                            <tr>
                                <td colspan="2">
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 200px; height: 30px">
                                            </td>
                                            <td style="text-align: left;">
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                <asp:Button ID="btnSave" runat="server" CssClass="tj_bt_da" Text="提交" Height="30"
                                                    Width="70" OnClick="btnSave_Click" OnClientClick="javascript:return confirm('您确定要进行提交吗？');" />
                                                &nbsp;&nbsp;
                                                &nbsp;&nbsp;
                                                <asp:Button ID="btnCancle" runat="server" CssClass="tj_bt_da" Text="取消" Height="30"
                                                    Width="70" OnClick="btnCancle_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>
    </form>
</body>

</html>
