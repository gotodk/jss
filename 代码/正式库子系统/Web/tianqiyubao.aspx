<%@ Page Language="C#" AutoEventWireup="true" CodeFile="tianqiyubao.aspx.cs" Inherits="Web_tianqiyubao" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:GridView ID="GridView1" runat="server" BackColor="White" 
            BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="4" 
            EnableModelValidation="True" ForeColor="Black" GridLines="Vertical">
            <AlternatingRowStyle BackColor="White" />
            <FooterStyle BackColor="#CCCC99" />
            <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
            <RowStyle BackColor="#F7F7DE" Wrap="True" />
            <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
        </asp:GridView>
    
        <br />
        <br />
        图标的含义：<br />
        d00.gif&nbsp;&nbsp; 晴<br />
        d01.gif&nbsp;&nbsp; 多云&nbsp;&nbsp;&nbsp; 
        <br />
        d02.gif&nbsp;&nbsp; 阴<br />
        d03.gif&nbsp;&nbsp; 阵雨<br />
        d04.gif&nbsp;&nbsp; 雷阵雨<br />
        d05.gif&nbsp;&nbsp; 雷阵雨伴有冰雹<br />
        d06.gif&nbsp;&nbsp; 雨夹雪<br />
        d07.gif&nbsp;&nbsp;
        <span style="color: rgb(0, 0, 0); font-family: monospace; font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; line-height: normal; orphans: 2; text-align: -webkit-auto; text-indent: 0px; text-transform: none; white-space: pre-wrap; widows: 2; word-spacing: 0px; -webkit-text-size-adjust: auto; -webkit-text-stroke-width: 0px; font-size: medium; display: inline !important; float: none; ">
        小雨</span><br />
        d08.gif
        <span style="color: rgb(0, 0, 0); font-family: monospace; font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; line-height: normal; orphans: 2; text-align: -webkit-auto; text-indent: 0px; text-transform: none; white-space: pre-wrap; widows: 2; word-spacing: 0px; -webkit-text-size-adjust: auto; -webkit-text-stroke-width: 0px; font-size: medium; display: inline !important; float: none; ">
        中雨</span><br />
        d09.gif
        <span style="color: rgb(0, 0, 0); font-family: monospace; font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; line-height: normal; orphans: 2; text-align: -webkit-auto; text-indent: 0px; text-transform: none; white-space: pre-wrap; widows: 2; word-spacing: 0px; -webkit-text-size-adjust: auto; -webkit-text-stroke-width: 0px; font-size: medium; display: inline !important; float: none; ">
        大雨</span><br />
        d10.gif
        <span style="color: rgb(0, 0, 0); font-family: monospace; font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; line-height: normal; orphans: 2; text-align: -webkit-auto; text-indent: 0px; text-transform: none; white-space: pre-wrap; widows: 2; word-spacing: 0px; -webkit-text-size-adjust: auto; -webkit-text-stroke-width: 0px; font-size: medium; display: inline !important; float: none; ">
        暴雨</span><br />
        d11.gif&nbsp;
        <span style="color: rgb(0, 0, 0); font-family: monospace; font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; line-height: normal; orphans: 2; text-align: -webkit-auto; text-indent: 0px; text-transform: none; white-space: pre-wrap; widows: 2; word-spacing: 0px; -webkit-text-size-adjust: auto; -webkit-text-stroke-width: 0px; font-size: medium; display: inline !important; float: none; ">
        大暴雨</span><br />
        d12.gif
        <span style="color: rgb(0, 0, 0); font-family: monospace; font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; line-height: normal; orphans: 2; text-align: -webkit-auto; text-indent: 0px; text-transform: none; white-space: pre-wrap; widows: 2; word-spacing: 0px; -webkit-text-size-adjust: auto; -webkit-text-stroke-width: 0px; font-size: medium; display: inline !important; float: none; ">
        特大暴雨</span><br />
        d13.gif&nbsp;
        <span style="color: rgb(0, 0, 0); font-family: monospace; font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; line-height: normal; orphans: 2; text-align: -webkit-auto; text-indent: 0px; text-transform: none; white-space: pre-wrap; widows: 2; word-spacing: 0px; -webkit-text-size-adjust: auto; -webkit-text-stroke-width: 0px; font-size: medium; display: inline !important; float: none; ">
        阵雪</span><br />
        d14.gif&nbsp;
        <span style="color: rgb(0, 0, 0); font-family: monospace; font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; line-height: normal; orphans: 2; text-align: -webkit-auto; text-indent: 0px; text-transform: none; white-space: pre-wrap; widows: 2; word-spacing: 0px; -webkit-text-size-adjust: auto; -webkit-text-stroke-width: 0px; font-size: medium; display: inline !important; float: none; ">
        小雪</span><br />
        d15.gif&nbsp;
        <span style="color: rgb(0, 0, 0); font-family: monospace; font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; line-height: normal; orphans: 2; text-align: -webkit-auto; text-indent: 0px; text-transform: none; white-space: pre-wrap; widows: 2; word-spacing: 0px; -webkit-text-size-adjust: auto; -webkit-text-stroke-width: 0px; font-size: medium; display: inline !important; float: none; ">
        中雪</span><br />
        d16.gif
        <span style="color: rgb(0, 0, 0); font-family: monospace; font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; line-height: normal; orphans: 2; text-align: -webkit-auto; text-indent: 0px; text-transform: none; white-space: pre-wrap; widows: 2; word-spacing: 0px; -webkit-text-size-adjust: auto; -webkit-text-stroke-width: 0px; font-size: medium; display: inline !important; float: none; ">
        大雪</span><br />
        d17.gif
        <span style="color: rgb(0, 0, 0); font-family: monospace; font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; line-height: normal; orphans: 2; text-align: -webkit-auto; text-indent: 0px; text-transform: none; white-space: pre-wrap; widows: 2; word-spacing: 0px; -webkit-text-size-adjust: auto; -webkit-text-stroke-width: 0px; font-size: medium; display: inline !important; float: none; ">
        暴雪</span><br />
        d18.gif&nbsp;
        <span style="color: rgb(0, 0, 0); font-family: monospace; font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; line-height: normal; orphans: 2; text-align: -webkit-auto; text-indent: 0px; text-transform: none; white-space: pre-wrap; widows: 2; word-spacing: 0px; -webkit-text-size-adjust: auto; -webkit-text-stroke-width: 0px; font-size: medium; display: inline !important; float: none; ">
        雾</span><br />
        d19.gif&nbsp;
        <span style="color: rgb(0, 0, 0); font-family: monospace; font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; line-height: normal; orphans: 2; text-align: -webkit-auto; text-indent: 0px; text-transform: none; white-space: pre-wrap; widows: 2; word-spacing: 0px; -webkit-text-size-adjust: auto; -webkit-text-stroke-width: 0px; font-size: medium; display: inline !important; float: none; ">
        冻雨</span><br />
        d20.gif&nbsp;
        <span style="color: rgb(0, 0, 0); font-family: monospace; font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; line-height: normal; orphans: 2; text-align: -webkit-auto; text-indent: 0px; text-transform: none; white-space: pre-wrap; widows: 2; word-spacing: 0px; -webkit-text-size-adjust: auto; -webkit-text-stroke-width: 0px; font-size: medium; display: inline !important; float: none; ">
        沙尘暴</span><br />
        d21.gif&nbsp;
        <span style="color: rgb(0, 0, 0); font-family: monospace; font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; line-height: normal; orphans: 2; text-align: -webkit-auto; text-indent: 0px; text-transform: none; white-space: pre-wrap; widows: 2; word-spacing: 0px; -webkit-text-size-adjust: auto; -webkit-text-stroke-width: 0px; font-size: medium; display: inline !important; float: none; ">
        小雨-中雨</span><br />
        d22.gif&nbsp;
        <span style="color: rgb(0, 0, 0); font-family: monospace; font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; line-height: normal; orphans: 2; text-align: -webkit-auto; text-indent: 0px; text-transform: none; white-space: pre-wrap; widows: 2; word-spacing: 0px; -webkit-text-size-adjust: auto; -webkit-text-stroke-width: 0px; font-size: medium; display: inline !important; float: none; ">
        中雨-大雨</span><br />
        d23.gif&nbsp;
        <span style="color: rgb(0, 0, 0); font-family: monospace; font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; line-height: normal; orphans: 2; text-align: -webkit-auto; text-indent: 0px; text-transform: none; white-space: pre-wrap; widows: 2; word-spacing: 0px; -webkit-text-size-adjust: auto; -webkit-text-stroke-width: 0px; font-size: medium; display: inline !important; float: none; ">
        大雨-暴雨</span><br />
        d24.gif&nbsp;
        <span style="color: rgb(0, 0, 0); font-family: monospace; font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; line-height: normal; orphans: 2; text-align: -webkit-auto; text-indent: 0px; text-transform: none; white-space: pre-wrap; widows: 2; word-spacing: 0px; -webkit-text-size-adjust: auto; -webkit-text-stroke-width: 0px; font-size: medium; display: inline !important; float: none; ">
        暴雨-大暴雨</span><br />
        d25.gif&nbsp;
        <span style="color: rgb(0, 0, 0); font-family: monospace; font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; line-height: normal; orphans: 2; text-align: -webkit-auto; text-indent: 0px; text-transform: none; white-space: pre-wrap; widows: 2; word-spacing: 0px; -webkit-text-size-adjust: auto; -webkit-text-stroke-width: 0px; font-size: medium; display: inline !important; float: none; ">
        大暴雨-特大暴雨</span><br />
        d26.gif&nbsp;
        <span style="color: rgb(0, 0, 0); font-family: monospace; font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; line-height: normal; orphans: 2; text-align: -webkit-auto; text-indent: 0px; text-transform: none; white-space: pre-wrap; widows: 2; word-spacing: 0px; -webkit-text-size-adjust: auto; -webkit-text-stroke-width: 0px; font-size: medium; display: inline !important; float: none; ">
        小雪-中雪</span><br />
        d27.gif&nbsp;
        <span style="color: rgb(0, 0, 0); font-family: monospace; font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; line-height: normal; orphans: 2; text-align: -webkit-auto; text-indent: 0px; text-transform: none; white-space: pre-wrap; widows: 2; word-spacing: 0px; -webkit-text-size-adjust: auto; -webkit-text-stroke-width: 0px; font-size: medium; display: inline !important; float: none; ">
        中雪-大雪</span><br />
        d28.gif&nbsp;
        <span style="color: rgb(0, 0, 0); font-family: monospace; font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; line-height: normal; orphans: 2; text-align: -webkit-auto; text-indent: 0px; text-transform: none; white-space: pre-wrap; widows: 2; word-spacing: 0px; -webkit-text-size-adjust: auto; -webkit-text-stroke-width: 0px; font-size: medium; display: inline !important; float: none; ">
        大雪-暴雪</span><br />
        d29.gif&nbsp;
        <span style="color: rgb(0, 0, 0); font-family: monospace; font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; line-height: normal; orphans: 2; text-align: -webkit-auto; text-indent: 0px; text-transform: none; white-space: pre-wrap; widows: 2; word-spacing: 0px; -webkit-text-size-adjust: auto; -webkit-text-stroke-width: 0px; font-size: medium; display: inline !important; float: none; ">
        浮尘</span><br />
        d30.gif&nbsp;
        <span style="color: rgb(0, 0, 0); font-family: monospace; font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; line-height: normal; orphans: 2; text-align: -webkit-auto; text-indent: 0px; text-transform: none; white-space: pre-wrap; widows: 2; word-spacing: 0px; -webkit-text-size-adjust: auto; -webkit-text-stroke-width: 0px; font-size: medium; display: inline !important; float: none; ">
        扬沙</span><br />
        d31.gif&nbsp;
        <span style="color: rgb(0, 0, 0); font-family: monospace; font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; line-height: normal; orphans: 2; text-align: -webkit-auto; text-indent: 0px; text-transform: none; white-space: pre-wrap; widows: 2; word-spacing: 0px; -webkit-text-size-adjust: auto; -webkit-text-stroke-width: 0px; font-size: medium; display: inline !important; float: none; ">
        强沙尘暴</span><br />
        <br />
        <br />
        <br />
        n00.gif<br />
        n01.gif<br />
        n02.gif<br />
        n03.gif<br />
        n04.gif<br />
        n05.gif<br />
        n06.gif<br />
        n07.gif<br />
        n08.gif<br />
        n09.gif<br />
        n10.gif<br />
        n11.gif<br />
        n12.gif<br />
        n13.gif<br />
        n14.gif<br />
        n15.gif<br />
        n16.gif<br />
        n17.gif<br />
        n18.gif<br />
        n19.gif<br />
        n20.gif<br />
        n21.gif<br />
        n22.gif<br />
        n23.gif<br />
        n24.gif<br />
        n25.gif<br />
        n26.gif<br />
        n27.gif<br />
        n28.gif<br />
        n29.gif<br />
        n30.gif<br />
        n31.gif</div>
    </form>
</body>
</html>
