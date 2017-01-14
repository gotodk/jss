<%@ Page Language="C#" AutoEventWireup="true" CodeFile="demo.aspx.cs" Inherits="pagerdemo_demo" %>

<%@ Register src="commonpager.ascx" tagname="commonpager" tagprefix="uc1" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

            <script type="text/javascript" src="../WebYHZTC/js/jquery-1.6.2.js"></script>
    <link rel="stylesheet" href="css/CRselectBox.css" type="text/css"/> 

<script type="text/javascript">
    $(function () {
        $(".CRselectBox").hover(function () {
            $(this).addClass("CRselectBoxHover");
        }, function () {
            $(this).removeClass("CRselectBoxHover");
        });
        $(".CRselectValue").click(function () {
            $(this).blur();
            $(".CRselectBoxOptions").show();
            return false;
        });
        $(".CRselectBoxItem a").click(function () {
            $(this).blur();
            var value = $(this).attr("rel");
            var txt = $(this).text();
            $("#abc").val(value);
            $("#abc_CRtext").val(txt);
            $(".CRselectValue").text(txt);
            $(".CRselectBoxItem a").removeClass("selected");
            $(this).addClass("selected");
            $(".CRselectBoxOptions").hide();
            return false;
        });
        /*点击任何地方关闭层*/
        $(document).click(function (event) {
            if ($(event.target).attr("class") != "CRselectBox") {
                $(".CRselectBoxOptions").hide();
            }
        });

        /*===================Test========================*/
        $("#test").click(function () {
            var value = $("#abc").val();
            var txt = $("#abc_CRtext").val();
            alert("你本次选择的值和文本分别是：" + value + "  , " + txt);
        });
    })
</script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:GridView ID="GV_show" runat="server" EnableModelValidation="True">
            <Columns>
                <asp:HyperLinkField DataNavigateUrlFields="id" 
                    DataNavigateUrlFormatString="demo_show.aspx?id={0}" HeaderText="测试查看返回" Text="查看" />
            </Columns>
        </asp:GridView>
        <uc1:commonpager ID="commonpager1" runat="server" />
        <asp:TextBox ID="TextBox1" runat="server">1834</asp:TextBox>
        <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="加载测试数据" />
    
        <asp:DropDownList ID="DropDownList1" runat="server">
        </asp:DropDownList>
        <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
        <asp:Image ID="Image1" runat="server"  />




<div class="CRselectBox">
	<input type="hidden" value="1"  name="abc" id="abc"/><!-- hidden 用来代替select的值 -->
	<input type="hidden" value="选项一"  name="abc_CRtext" id="abc_CRtext"/> <!-- hidden 用来代替select的文本-->

    <a class="CRselectValue">选项一</a>
	<ul class="CRselectBoxOptions">
		<li class="CRselectBoxItem"><a href="#" class="selected" rel="1">选项一</a></li>
		<li class="CRselectBoxItem"><a href="#" rel="2">选项二</a></li>
		<li class="CRselectBoxItem"><a href="#" rel="3">选项三</a></li>
		<li class="CRselectBoxItem"><a href="#" rel="4">选项四</a></li>
		<li class="CRselectBoxItem"><a href="#" rel="5">选项五</a></li>
		<li class="CRselectBoxItem"><a href="#" rel="6">选项六</a></li>
	</ul>
</div>

<br/>
<input type="button" id="test" value="输出选中的值和文本内容"/>





    </div>
    </form>
</body>
</html>
