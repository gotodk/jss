<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DedicatedAccountRegistration.aspx.cs" Inherits="Web_JHJX_New2013_BankDedicated_DedicatedAccountRegistration" %>

<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<%@ Register src="../../../UCCityList.ascx" tagname="UCCityList" tagprefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
      <script src="../../../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="../../../jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/art_confirm.js" type="text/javascript"></script>
    <link href="../../../../css/standardStyle.css" rel="stylesheet" type="text/css" />
    <script src="../../../../js/standardJSFile/jquery.tableresizer.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/jquery.art_confirm.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/jquery.tablechangecolor.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/fcf.js" type="text/javascript"></script>
    <style type="text/css">
        .content_tab tr
        {
            height: 30px;
            line-height: 30px;
        }
        .style1
        {
            height: 30px;
        }
       .link
       {
           cursor:pointer;
           }
    </style>
    <script type="text/javascript">
        //Start--身份证验证
        function isCarId(carid)
        {
            if (carid.match(/^(^\d{15}$|^\d{18}$|^\d{17}(\d|X|x))$/)) {
                return true;
            }
            else {
                return false;
            }
        }
        //End--身份证验证
        //Start--邮箱验证
        function isEmail(email)
        {
            if (email.match(/^([A-Za-z0-9_.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4})(\]?)$/)) {
                return true;
            }
            else {
                return false;
            }
        }

        function isEmailFocusin(email)
        {
            var Email = email;
            if (Email != "") {
                if (Email.length > 40) {
                    $("#lblYXYZ").html('邮箱长度不能超过40个字符');
                    $("#lblYXYZ").css("color", "red");
                    return false;
                }
                else {
                    if (!isEmail(Email)) {
                        $("#lblYXYZ").html('邮箱格式不正确');
                        $("#lblYXYZ").css("color", "red");
                        return false;
                    }
                    else {
                        $("#lblYXYZ").html('');
                        return true;
                    }
                }                
            }
            else {
                $("#lblYXYZ").html('建议您使用常用邮箱作为安全邮箱，方便账号和密码找回。');
                $("#lblYXYZ").css("color", "#646464");
                return false;
            }
        }

        function isEmailFocusout(email)
        {
            var Email = email;
            if (Email != "") {
                if (Email.length > 40) {
                    $("#lblYXYZ").html('邮箱长度不能超过40个字符');
                    $("#lblYXYZ").css("color", "red");
                    return false;
                }
                else {
                    if (!isEmail(Email)) {
                        $("#lblYXYZ").html('邮箱格式不正确');
                        $("#lblYXYZ").css("color", "red");
                    }
                    else {
                        $("#lblYXYZ").html('');
                    }
                }
                
            }
            else {
                $("#lblYXYZ").html('');
            }
        }
        //End--邮箱验证
        //Start--用户名
        function isUserName(userName) {
            if (userName.match(/^\w{2,16}$/)) {
                return true;
            }
            else {
                return false;
            }
        }

        function isUserNameFocusin(userName) {
            var UserName = userName;
            if (UserName != "") {
                if (!isUserName(UserName)) {
                    $("#lblUSerNameYZ").html('2—16位（仅可使用汉字、字母、数字、下划线）');
                    $("#lblUSerNameYZ").css("color", "red");
                    return false;
                }
                else {
                    $("#lblUSerNameYZ").html('');
                    return true;
                }
            }
            else {
                $("#lblUSerNameYZ").html('用户名由2-16位汉字、字母（不区分大小写）、数字、下划线组成。');
                $("#lblUSerNameYZ").css("color", "#646464");
                return false;
            }
        }

        function isUserNameFocusout(userName) {
            var UserName = userName;
            if (UserName != "") {
                if (!isUserName(UserName)) {
                    $("#lblUSerNameYZ").html('2—16位（仅可使用汉字、字母、数字、下划线）');
                    $("#lblUSerNameYZ").css("color", "red");
                }
                else {
                    $("#lblUSerNameYZ").html('');
                }
            }
            else {
                $("#lblUSerNameYZ").html('');
            }
        }
        //End--用户名
        //Start--密码
        function isPassWord(passWord) {
            if (passWord.match(/^\w{2,16}$/)) {
                return true;
            }
            else {
                return false;
            }
        }

        function isPassWordFocusin(passWord) {
            var PassWord = passWord;
            if (PassWord != "") {
                
                if (PassWord.length<6)
                {
                    $("#lblMMYZ").html('密码长度限制在6—16位之间');
                    $("#lblMMYZ").css("color", "red");
                    return false;
                }
                else if (!isPassWord(PassWord) && !(PassWord.length<6)) {
                    $("#lblMMYZ").html('请勿使用数字、字母以外的特殊字符');
                    $("#lblMMYZ").css("color", "red");
                    return false;
                }
                else if (PassWord.length >= 6 && PassWord.length <= 8)
                {
                    if (PassWord.match(/^[a-z]{6,8}$/)||PassWord.match(/^[0-9]{6,8}$/)||PassWord.match(/^[A-Z]{6,8}$/))
                    {
                        $("#lblMMYZ").html('密码强度：弱');
                        $("#lblMMYZ").css("color", "red");
                        return true;
                    }
                    else if (PassWord.match(/^[a-zA-Z]{6,8}$/) || PassWord.match(/^[a-z0-9]{6,8}$/) || PassWord.match(/^[A-Z0-9]{6,8}$/) || PassWord.match(/^[a-zA-Z0-9]{6,8}$/))
                    {
                        $("#lblMMYZ").html('密码强度：中');
                        $("#lblMMYZ").css("color", "red");
                        return true;
                    }                
                }
                else if (PassWord.length >= 9)
                {
                    if (PassWord.match(/^[a-z]{9,16}$/) || PassWord.match(/^[A-Z]{9,16}$/) || PassWord.match(/^[0-9]{9,16}$/))
                    {
                        $("#lblMMYZ").html('密码强度：中');
                        $("#lblMMYZ").css("color", "red");
                        return true;
                    }
                    else if (PassWord.match(/^[a-zA-Z]{9,16}$/) || PassWord.match(/^[a-z0-9]{9,16}$/) || PassWord.match(/^[A-Z0-9]{9,16}$/) || PassWord.match(/^[a-zA-Z0-9]{9,16}$/))
                    {
                        $("#lblMMYZ").html('密码强度：强');
                        $("#lblMMYZ").css("color", "red");
                        return true;
                    }
                }
                else {
                    $("#lblMMYZ").html('');
                    return true;
                }
            }
            else {
                $("#lblMMYZ").html('密码由6-16位字母、数字组成，区分大小写。');
                $("#lblMMYZ").css("color", "#646464");
                return false;
            }
        }

        function isPassWordFocusout(passWord) {
            var PassWord = passWord;
            if (PassWord != "") {
                if (PassWord.length < 6) {
                    $("#lblMMYZ").html('密码长度限制在6—16位之间');
                    $("#lblMMYZ").css("color", "red");
                }
                else if (!isPassWord(PassWord) && !(PassWord.length < 6)) {
                    $("#lblMMYZ").html('请勿使用数字、字母以外的特殊字符');
                    $("#lblMMYZ").css("color", "red");
                }
                else if (PassWord.length >= 6 && PassWord.length <= 8) {
                    if (PassWord.match(/^[a-z]{6,8}$/) || PassWord.match(/^[0-9]{6,8}$/) || PassWord.match(/^[A-Z]{6,8}$/)) {
                        $("#lblMMYZ").html('密码强度：弱');
                        $("#lblMMYZ").css("color", "red");
                    }
                    else if (PassWord.match(/^[a-zA-Z]{6,8}$/) || PassWord.match(/^[a-z0-9]{6,8}$/) || PassWord.match(/^[A-Z0-9]{6,8}$/) || PassWord.match(/^[a-zA-Z0-9]{6,8}$/)) {
                        $("#lblMMYZ").html('密码强度：中');
                        $("#lblMMYZ").css("color", "red");
                    }
                }
                else if (PassWord.length >= 9) {
                    if (PassWord.match(/^[a-z]{9,16}$/) || PassWord.match(/^[A-Z]{9,16}$/) || PassWord.match(/^[0-9]{9,16}$/)) {
                        $("#lblMMYZ").html('密码强度：中');
                        $("#lblMMYZ").css("color", "red");
                    }
                    else if (PassWord.match(/^[a-zA-Z]{9,16}$/) || PassWord.match(/^[a-z0-9]{9,16}$/) || PassWord.match(/^[A-Z0-9]{9,16}$/) || PassWord.match(/^[a-zA-Z0-9]{9,16}$/)) {
                        $("#lblMMYZ").html('密码强度：强');
                        $("#lblMMYZ").css("color", "red");
                    }
                }
                else {
                    $("#lblMMYZ").html('');
                }
            }
            else {
                $("#lblMMYZ").html('');
            }
        }
        //End--密码
        //Start--确认密码
        function isPWDAginFocusin(pwd, pwdagin)
        {
            if (pwdagin != "") {
                if (pwd != pwdagin) {
                    $("#lblQRMMYZ").html('两次密码输入不一致。');
                    $("#lblQRMMYZ").css("color", "red");
                    return false;
                }
                else {
                    $("#lblQRMMYZ").html('');
                    return true;
                }
            }
            else {
                $("#lblQRMMYZ").html('再次输入密码。');
                $("#lblQRMMYZ").css("color", "#646464");
                return false;
            }
        }
        function isPWDAginFocusout(pwd, pwdagin) {
            if (pwdagin != "") {
                if (pwd != pwdagin) {
                    $("#lblQRMMYZ").html('两次密码输入不一致。');
                    $("#lblQRMMYZ").css("color", "red");
                }
                else {
                    $("#lblQRMMYZ").html('');
                }
            }
            else {
                $("#lblQRMMYZ").html('');
            }
        }
        //End--确认密码
    </script>
    <script type="text/javascript">
        function Promot(this_an) {
            var email = $("#txtYX").val();
            var username = $("#txtUserName").val();
            var pwd = $("#txtMM").val();
            var pwdagion = $("#txtQRMM").val();
            var jjfmc = $("#txtJJFMC").val();//交易方名称
            var yyzzzch = $("#txtYYZZZCH").val();//营业执照注册号
            var yyzzsmj = $("#lblYYZZ").attr("title");//营业执照扫描件
            var zzjgdmz = $("#txtZZJGDMZDM").val();//组织机构代码证
            var zzjgdmzsmj = $("#lblZZJGDMZ").attr("title");//组织机构代码证扫描件
            var swdjzsh = $("#txtSWDJZSH").val();//税务登记证税号
            var swdjzsmj = $("#lblSWDJZ").attr("title");//税务登记证扫描件
            var khxkz = $("#txtKHXKZ").val();//开户许可证
            var khxkzsmj = $("#lblKHXKZ").attr("title");//开户许可证扫描件
            var ylyqksmj = $("#lblYLYQK").attr("title");//预留印签卡扫描件
            var fddbrxm = $("#txtFDDBRXM").val();//法定代表人姓名
            var sfzh = $("#txtSFZH").val();//身份证号
            var sfzzmsmj = $("#lblSFZZM").attr("title");//身份证正面扫描件
            var sfzfmsmj = $("#lblSFZFM").attr("title");//身份证反面扫描件
            var fddbrsqssmj = $("#lblFDRSQS").attr("title");//法定代表人授权书扫描件
            var jjflxdh = $("#txtJYFLXDH").val();//交易方联系电话
            //所属区域

            var xxdz = $("#txtXXDZ").val();//详细地址
            var lxr = $("#txtLXRXM").val();//联系人姓名
            var lxrsjh = $("#txtLXRSJH").val();//联系人手机号

            
            var khyh = $("#drpBank").find("option:selected").text();//预指定开户银行
            var jyzjmm = $("#txtJYZJMM").val();//交易资金密码

            var falgs = true;
            if (email=="")
            {
                $("#lblYXYZ").html('请填写您的常用邮箱！');
                $("#lblYXYZ").css("color", "red");
                falgs = false;
            }
            if (username=="")
            {
                $("#lblUSerNameYZ").html('请输入用户名！');
                $("#lblUSerNameYZ").css("color", "red");
                falgs = false;
            }
            if (pwd=="")
            {
                $("#lblMMYZ").html('请输入密码！');
                $("#lblMMYZ").css("color", "red");
                falgs = false;
            }
            if (pwdagion == "") {
                $("#lblQRMMYZ").html('请再次输入密码！');
                $("#lblQRMMYZ").css("color", "red");
                falgs = false;
            }
            //交易方名称
            if (jjfmc == "") {
                $("#lblJYFMCYZ").html('请输入真实的单位名称！');
                falgs = false;
            }
            else {
                $("#lblJYFMCYZ").html('');
            }
            //营业执照
            if (yyzzzch == "" && (typeof (yyzzsmj) == "undefined" || yyzzsmj==""))
            {
                $("#lblYYZZYZ").html('请填写营业执照注册号，并上传扫描件！');
                falgs = false;
            }
            else if (yyzzzch == "" && (typeof (yyzzsmj) != "undefined" && yyzzsmj != ""))
            {
                $("#lblYYZZYZ").html('请填写营业执照注册号！');
                falgs = false;
            }
            else if (yyzzzch != "" && (typeof (yyzzsmj) == "undefined" || yyzzsmj == "")) {
                $("#lblYYZZYZ").html('请上传营业执照扫描件！');
                falgs = false;
            }
            else {
                $("#lblYYZZYZ").html('');
            }
            //组织机构代码证
            if (zzjgdmz == "" && (typeof (zzjgdmzsmj) == "undefined" || zzjgdmzsmj == "")) {
                $("#lblZZJGDMZYZ").html('请填写组织机构代码证代码，并上传扫描件！');
                falgs = false;
            }
            else if (zzjgdmz == "" && (typeof (zzjgdmzsmj) != "undefined" && zzjgdmzsmj != "")) {
                $("#lblZZJGDMZYZ").html('请填写组织机构代码证代码！');
                falgs = false;
            }
            else if (zzjgdmz != "" && (typeof (zzjgdmzsmj) == "undefined" || zzjgdmzsmj == "")) {
                $("#lblZZJGDMZYZ").html('请上传组织机构代码证扫描件！');
                falgs = false;
            }
            else {
                $("#lblZZJGDMZYZ").html('');
            }
            //税务登记证税号
            if (swdjzsh == "" && (typeof (swdjzsmj) == "undefined" || swdjzsmj == "")) {
                $("#lblSWDJZYZ").html('请填写税务登记证税号，并上传扫描件！');
                falgs = false;
            }
            else if (swdjzsh == "" && (typeof (swdjzsmj) != "undefined" && swdjzsmj != "")) {
                $("#lblSWDJZYZ").html('请填写税务登记证税号！');
                falgs = false;
            }
            else if (swdjzsh != "" && (typeof (swdjzsmj) == "undefined" || swdjzsmj == "")) {
                $("#lblSWDJZYZ").html('请上传税务登记证扫描件！');
                falgs = false;
            }
            else {
                $("#lblSWDJZYZ").html('');
            }
            //经纪人单位的开户许可证为非必填项，2014.07改为必填项            
            if (khxkz == "" && (typeof (khxkzsmj) == "undefined" || khxkzsmj == "")) {
                $("#lblKHXKZYZ").html('请填写开户许可证，并上传扫描件！');
                falgs = false;
            }
            else if (khxkz == "" && (typeof (khxkzsmj) != "undefined" && khxkzsmj != "")) {
                $("#lblKHXKZYZ").html('请填写开户许可证！');
                falgs = false;
            }
            else if (khxkz != "" && (typeof (khxkzsmj) == "undefined" || khxkzsmj == "")) {
                $("#lblKHXKZYZ").html('请上传开户许可证扫描件！');
                falgs = false;
            }
            else {
                $("#lblKHXKZYZ").html('');
            }
            
            //预留印签卡
            if (typeof (ylyqksmj) == "undefined" || ylyqksmj == "") {
                $("#lblYLYQKYZ").html('请上传预留印鉴表扫描件！');
                falgs = false;
            }
            else {
                $("#lblYLYQKYZ").html('');
            }
            //经纪人单位的法定代表人姓名为非必填项，2014.07改为必填项
            if (fddbrxm == "") {
                $("#lblFDDBRXMYZ").html("法定代表人姓名！");
                falgs = false;
            }
            else {
                $("#lblFDDBRXMYZ").html("");
            }
            
            //法定代表人身份证号
            if (sfzh == "" && (typeof (sfzzmsmj) == "undefined" || sfzzmsmj == "")) {
                $("#lblSFZZMYZ").html('请填写法定代表人身份证号，并上传正面扫描件！');
                falgs = false;
            }
            else if (sfzh == "" && (typeof (sfzzmsmj) != "undefined" && sfzzmsmj != "")) {
                $("#lblSFZZMYZ").html('请填写法定代表人身份证号！');
                falgs = false;
            }
            else if (sfzh != "" && (typeof (sfzzmsmj) == "undefined" || sfzzmsmj == "")) {
                if (!isCarId(sfzh)) {
                    $("#lblSFZZMYZ").html('请填写正确的法定代表人身份证号！');
                }
                else {
                    $("#lblSFZZMYZ").html('请上传法定代表人身份证正面扫描件！');
                }
                falgs = false;
            }
            else if (sfzh != "" && (typeof (sfzzmsmj) != "undefined" || sfzzmsmj != ""))
            {
                if (!isCarId(sfzh)) {
                    $("#lblSFZZMYZ").html('请填写正确的法定代表人身份证号！');
                    falgs = false;
                }
                else {
                    //需要验证此身份证是否被注册过,在后台验证
                    $("#lblSFZZMYZ").html('');
                }
            }
            else {
                $("#lblSFZZMYZ").html('');
            }

            //法定代表人身份证反面扫描件
            if (typeof (sfzfmsmj) == "undefined" || sfzfmsmj == "") {
                $("#lblSFZFMYZ").html('请上传身份证反面扫描件！');
                falgs = false;
            }
            else {
                $("#lblSFZFMYZ").html('');
            }
            //法定代表人授权书
            if (typeof (fddbrsqssmj) == "undefined" || fddbrsqssmj == "") {
                $("#lblFDDBRYZ").html('请上传法定代表人授权书！');
                falgs = false;
            }
            else {
                $("#lblFDDBRYZ").html('');
            }
            //交易方联系电话
            if(jjflxdh=="")
            {
                $("#lblJYFLXDHYZ").html("请填写交易方联系电话！");
                falgs = false;
            }
            else if (!jjflxdh.match(/^(\-*\d*)?(\d*-*)$/)) {
                $("#lblJYFLXDHYZ").html('交易方联系电话格式不正确！');
                falgs = false;
            }
            else {
                $("#lblJYFLXDHYZ").html('');
            }
            //所属区域
            
            //详细地址
            if (xxdz == "") {
                $("#lblXXDZYZ").html("请填写详细地址！");
                falgs = false;
            }
            else {
                $("#lblXXDZYZ").html("");
            }
            //联系人姓名
            if (lxr=="")
            {
                $("#lblLXRXMYZ").html("请填写联系人姓名！");
                falgs = false;
            }
            else
            {
                $("#lblLXRXMYZ").html("");
            }
            //联系人手机号
            if (lxrsjh == "") {
                $("#lblLXRSJHYZ").html("请填写联系人手机号！");
                falgs = false;
            }
            else {
                if (!lxrsjh.match(/^(13|15|18)[0-9]{9}$/)) {
                    $("#lblLXRSJHYZ").html("联系人手机号格式不正确！");
                    falgs = false;
                }
                else {
                    $("#lblLXRSJHYZ").html("");
                }
            }

            //预指定开户银行
            if ($("#drpBank").find("option:selected").text() == "请选择预指定开户银行") {
                $("#lblBankYZ").html("请选择预指定开户银行！");
                falgs = false;
            }
            else {
                $("#lblBankYZ").html("");
            }
            //交易资金密码
            if (jyzjmm == "") {
                $("#lblJYZJMMYZ").html("请填写交易资金密码！");
                falgs = false;
            }
            else {
                if (!jyzjmm.match(/^\d{6}$/)) {
                    $("#lblJYZJMMYZ").html("交易资金密码必须是6位数字！");
                    falgs = false;
                }
                else {
                    $("#lblJYZJMMYZ").html("");
                }

            }

            if (!falgs)
            {
                return false;
            }
            if (isEmailFocusin(email) && isUserNameFocusin(username) && isPassWordFocusin(pwd) && isPWDAginFocusin(pwd,pwdagion)) {
                return art_confirm_fcf(this_an, "您确定要执行此操作吗！此操作执行的结果将不可恢复！", 'clickyc()');
            }
            else {
                isEmailFocusin(email);
                isUserNameFocusin(username);
                isPassWordFocusin(pwd);
                isPWDAginFocusin(pwd, pwdagion);
                return false;
            }            
        }
        $(document).ready(function () {
            

            //Start--邮箱验证
            $("#txtYX").focusin(function () {
                var Email = this.value;
                isEmailFocusin(Email);
                
            });
            $("#txtYX").focusout(function () {
                var Email = this.value;
                isEmailFocusout(Email);
            });
            //用keyup函数可以获取到全部输入框的值，keydouwn只是获取到按下当前键之前的所有值
            $("#txtYX").keyup(function () {
                var Email = this.value;
                isEmailFocusin(Email);
            });
            //End--邮箱验证
            //Start--用户名
            $("#txtUserName").focusin(function () {
                var Email = this.value;
                isUserNameFocusin(Email);
            });
            $("#txtUserName").focusout(function () {
                var Email = this.value;
                isUserNameFocusout(Email);
            });
            //用keyup函数可以获取到全部输入框的值，keydouwn只是获取到按下当前键之前的所有值
            $("#txtUserName").keyup(function () {
                var Email = this.value;
                isUserNameFocusin(Email);
            });
            //End--用户名
            //Start--密码
            $("#txtMM").focusin(function () {
                var PWD = this.value;
                isPassWordFocusin(PWD);
            });
            $("#txtMM").focusout(function () {
                var PWD = this.value;
                isPassWordFocusout(PWD);
            });
            //用keyup函数可以获取到全部输入框的值，keydouwn只是获取到按下当前键之前的所有值
            $("#txtMM").keyup(function () {
                var PWD = this.value;
                isPassWordFocusin(PWD);
            });
            //End--密码
            //Start--确认密码
            $("#txtQRMM").focusin(function () {
                var PWDAgion = this.value;
                var PWD = $("#txtMM").val();
                isPWDAginFocusin(PWD, PWDAgion);
            });
            $("#txtQRMM").focusout(function () {
                var PWDAgion = this.value;
                var PWD = $("#txtMM").val();
                isPWDAginFocusout(PWD, PWDAgion);
            });
            //用keyup函数可以获取到全部输入框的值，keydouwn只是获取到按下当前键之前的所有值
            $("#txtQRMM").keyup(function () {
                var PWDAgion = this.value;
                var PWD = $("#txtMM").val();
                isPWDAginFocusin(PWD, PWDAgion);
            });
            //End--密确认密码码
        });
    </script>

    <script type="text/javascript">
        function Reset(this_an) {
            return window.top.Dialog.confirm('您确定要重填吗？', function () { var a = window.location.href; var b = a.substring(0, a.lastIndexOf('/JHJX')); var url = b + '/JHJX/New2013/BankDedicated/DedicatedAccountRegistration.aspx'; window.location = url; });
            
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <radTS:RadTabStrip ID="RadTabStrip1" runat="server" Height="25px" Skin="Default2006"
        BackColor="#f7f7f7">
        <Tabs>
            <radTS:Tab ID="Tab1" runat="server" Text="专用账户注册">
            </radTS:Tab>
        </Tabs>
    </radTS:RadTabStrip>
    <div id="new_content">
        <div id="new_zicontent">
            <div id="content_zw">
                <div class="content_lx">
                </div>
                <div class="content_nr">
                    <table width="100%" class="content_tab">
                    <tr>
                            <td align="right" width="150px;">
                                登录邮箱：
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtYX" runat="server" CssClass="tj_input" Width="200px"
                                    TabIndex="1" MaxLength="40"></asp:TextBox>
                                &nbsp;&nbsp;<asp:Label ID="lblYXYZ" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" >
                                用户名：
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtUserName" runat="server" CssClass="tj_input" Width="200px"
                                    TabIndex="1" MaxLength="16"></asp:TextBox>
                                &nbsp;&nbsp;<asp:Label ID="lblUSerNameYZ" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                密码：
                            </td>
                            <td colspan="3">
                              <asp:TextBox ID="txtMM" runat="server" CssClass="tj_input" Width="200px"
                                    TabIndex="1" MaxLength="16" TextMode="Password"></asp:TextBox>
                                &nbsp;&nbsp;<asp:Label ID="lblMMYZ" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                确认密码：
                            </td>
                            <td colspan="3">
                              <asp:TextBox ID="txtQRMM" runat="server" CssClass="tj_input" Width="200px" 
                                    TabIndex="1" MaxLength="16" TextMode="Password" ></asp:TextBox>
                                &nbsp;&nbsp;<asp:Label ID="lblQRMMYZ" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr id="trSFZH" runat="server">
                            <td align="right">
                                
                            </td>
                            <td colspan="3" valign="top">
                                <asp:CheckBox ID="CheckBox1" runat="server" Text="同意　富美平台注册协议" Checked="True" Enabled="False" />
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                交易账户类型：
                            </td>
                            <td colspan="3">
                                <asp:RadioButtonList ID="rdbtnJJFZH" runat="server">
                                    <asp:ListItem Enabled="False" Selected="True">经纪人交易账户</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                注册类别：
                            </td>
                            <td colspan="3">
                              <asp:RadioButtonList ID="rdbtnZCLB" runat="server">
                                    <asp:ListItem Enabled="False" Selected="True">单位</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                交易方名称：
                            </td>
                            <td colspan="3">
                              <asp:TextBox ID="txtJJFMC" runat="server" CssClass="tj_input" Width="400px"
                                    TabIndex="1" ToolTip="请填写单位全称，与您在银行开户时的账户企业名称一致或自然人姓名" MaxLength="20"></asp:TextBox>
                                &nbsp;&nbsp;<asp:Label ID="lblJYFMCYZ" runat="server" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                
                            </td>
                            <td colspan="3">
                              <span style="color:red;">交易方名称须与《组织机构代码证》的单位全称完全一致，否则交易账户将无法与银行的第三方存管账户绑定，不能出、入金！</span>
                            </td>
                        </tr>
                        <tr id="trSFZSMJ" runat="server">
                            <td align="right">
                                营业执照注册号：
                            </td>
                            <td colspan="3">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtYYZZZCH" runat="server" CssClass="tj_input" Width="200px"
                                    TabIndex="1" MaxLength="20"></asp:TextBox>
                                        </td>
                                        <td>
                                            &nbsp;&nbsp;扫描件：
                                        </td>
                                        <td>
                                            <asp:FileUpload ID="FULoadYYZZ" runat="server" EnableTheming="True" CssClass="tj_input" UseSubmitBehavior="False" />
                                &nbsp;&nbsp; <asp:Button ID="btnYYZZUpload" runat="server" Text="上传" CssClass="tj_bt_da" UseSubmitBehavior="False" OnClick="btnYYZZUpload_Click" />
                                &nbsp;&nbsp; <asp:Button ID="btnYYZZCK" runat="server" Text="查看" CssClass="tj_bt_da" UseSubmitBehavior="False" Visible="False"  OnClick="btnYYZZCK_Click" />
                                &nbsp;&nbsp; <asp:Button ID="btnYYZZDelete" runat="server" Text="删除" CssClass="tj_bt_da" UseSubmitBehavior="False" Visible="False" OnClick="btnYYZZDelete_Click" /><asp:Label ID="lblYYZZ" runat="server" Visible="true" Style=" display:none;"></asp:Label>
                                            &nbsp;&nbsp;<asp:Label ID="lblYYZZYZ" runat="server" ForeColor="Red"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                               
                                  
                            </td>
                        </tr>
                        <tr id="trQZCNS" runat="server">
                            <td align="right">
                                组织机构代码证代码：
                            </td>
                            <td colspan="3">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtZZJGDMZDM" runat="server" CssClass="tj_input" Width="200px"
                                    TabIndex="1" MaxLength="18"></asp:TextBox>
                                        </td>
                                        <td>
                                            &nbsp;&nbsp;扫描件：
                                        </td>
                                        <td>
                                            <asp:FileUpload ID="FULoadZZJGDMZ" runat="server" EnableTheming="True" CssClass="tj_input" UseSubmitBehavior="False" />
                                &nbsp;&nbsp; <asp:Button ID="btnZZJGDMZUpload" runat="server" Text="上传" CssClass="tj_bt_da" UseSubmitBehavior="False" OnClick="btnZZJGDMZUpload_Click" />
                                &nbsp;&nbsp; <asp:Button ID="btnZZJGDMZCK" runat="server" Text="查看" CssClass="tj_bt_da" UseSubmitBehavior="False" Visible="False" OnClick="btnZZJGDMZCK_Click" />
                                &nbsp;&nbsp; <asp:Button ID="btnZZJGDMZDelete" runat="server" Text="删除" CssClass="tj_bt_da" UseSubmitBehavior="False" Visible="False" OnClick="btnZZJGDMZDelete_Click" /> <asp:Label ID="lblZZJGDMZ" runat="server" Visible="true" Style=" display:none;"></asp:Label>
                                            &nbsp;&nbsp;<asp:Label ID="lblZZJGDMZYZ" runat="server" ForeColor="Red"></asp:Label>
                                        </td>
                                    </tr>
                                </table>   
                            </td>
                        </tr>
                        <tr id="trGSMC" runat="server">
                            <td align="right">
                                税务登记证税号：
                            </td>
                            <td colspan="3">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtSWDJZSH" runat="server" CssClass="tj_input" Width="200px" 
                                    TabIndex="1" MaxLength="20"></asp:TextBox>
                                        </td>
                                        <td>
                                            &nbsp;&nbsp;扫描件：
                                        </td>
                                        <td>
                                            <asp:FileUpload ID="FULoadSWDJZ" runat="server" EnableTheming="True" CssClass="tj_input" UseSubmitBehavior="False" />
                                &nbsp;&nbsp; <asp:Button ID="btnSWDJZUpload" runat="server" Text="上传" CssClass="tj_bt_da" UseSubmitBehavior="False" OnClick="btnSWDJZUpload_Click" />
                                &nbsp;&nbsp; <asp:Button ID="btnSWDJCK" runat="server" Text="查看" CssClass="tj_bt_da" UseSubmitBehavior="False" Visible="False" OnClick="btnSWDJCK_Click"/>
                                &nbsp;&nbsp; <asp:Button ID="btnSWDJDelete" runat="server" Text="删除" CssClass="tj_bt_da" UseSubmitBehavior="False" Visible="False" OnClick="btnSWDJDelete_Click" /><asp:Label ID="lblSWDJZ" runat="server" Visible="true" Style=" display:none;"></asp:Label>
                                            &nbsp;&nbsp;<asp:Label ID="lblSWDJZYZ" runat="server" ForeColor="Red"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                              
                                 
                            </td>
                        </tr>
                        <tr id="trGSDH" runat="server">
                            <td align="right">
                                开户许可证：
                            </td>
                            <td colspan="3">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtKHXKZ" runat="server" CssClass="tj_input" Width="200px"
                                    TabIndex="1" MaxLength="20"></asp:TextBox>
                                        </td>
                                        <td>
                                            &nbsp;&nbsp;扫描件：
                                        </td>
                                        <td>
                                            <asp:FileUpload ID="FULoadKHXKZ" runat="server" EnableTheming="True" CssClass="tj_input" UseSubmitBehavior="False" />
                                &nbsp;&nbsp; <asp:Button ID="btnKHXKZUpload" runat="server" Text="上传" CssClass="tj_bt_da" UseSubmitBehavior="False" OnClick="btnKHXKZUpload_Click" />
                                &nbsp;&nbsp; <asp:Button ID="btnKHXKZCK" runat="server" Text="查看" CssClass="tj_bt_da" UseSubmitBehavior="False" Visible="False" OnClick="btnKHXKZCK_Click" />
                                &nbsp;&nbsp; <asp:Button ID="btnKHXKZDelete" runat="server" Text="删除" CssClass="tj_bt_da" UseSubmitBehavior="False" Visible="False" OnClick="btnKHXKZDelete_Click" /><asp:Label ID="lblKHXKZ" runat="server" Visible="true" Style=" display:none;"></asp:Label>
                                            &nbsp;&nbsp;<asp:Label ID="lblKHXKZYZ" runat="server" ForeColor="Red"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                              
                               
                            </td>
                        </tr>
                        <tr id="trGSDZ" runat="server">
                            <td align="right">
                                预留印签表：
                            </td>
                            <td colspan="3">
                               <asp:FileUpload ID="FULoadYLYQK" runat="server" EnableTheming="True" CssClass="tj_input" UseSubmitBehavior="False" />
                                &nbsp;&nbsp; <asp:Button ID="btnYLYQKUpload" runat="server" Text="上传" CssClass="tj_bt_da" UseSubmitBehavior="False" OnClick="btnYLYQKUpload_Click" />
                                &nbsp;&nbsp; <asp:Button ID="btnYLYQKCK" runat="server" Text="查看" CssClass="tj_bt_da" UseSubmitBehavior="False" OnClick="btnYLYQKCK_Click" Visible="false" />
                                &nbsp;&nbsp; <asp:Button ID="btnYLYQKDelte" runat="server" Text="删除" CssClass="tj_bt_da" UseSubmitBehavior="False" OnClick="btnYLYQKDelte_Click" Visible="false"  /> <asp:Label ID="lblYLYQK" runat="server" Visible="true" Style=" display:none;"></asp:Label>
                                &nbsp;&nbsp;<asp:Label ID="lblYLYQKYZ" runat="server" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr id="trYYZZ" runat="server">
                            <td align="right" class="style1">
                                法定代表人姓名：
                            </td>
                            <td colspan="3" class="style1">
                              <asp:TextBox ID="txtFDDBRXM" runat="server" CssClass="tj_input" Width="200px"
                                    TabIndex="1" MaxLength="10"></asp:TextBox> 
                               &nbsp;&nbsp;<asp:Label ID="lblFDDBRXMYZ" runat="server" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr id="trZZJGDMZ" runat="server">
                            <td align="right">
                                <table>
                                    <tr>
                                        <td>法定代表人身份证号：</td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                    </tr>
                                </table>
                                
                            </td>
                            <td colspan="3">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtSFZH" runat="server" CssClass="tj_input" Width="200px" 
                                    TabIndex="1" MaxLength="18"></asp:TextBox> 
                                        </td>
                                        <td style="text-align:right;">
                                            &nbsp;&nbsp;身份证正面扫描件：
                                            </td>
                                        <td>
                                            <asp:FileUpload ID="FULoadSFZZM" runat="server" EnableTheming="True" CssClass="tj_input" UseSubmitBehavior="False" />
                                &nbsp;&nbsp; <asp:Button ID="btnSFZZMUpload" runat="server" Text="上传" CssClass="tj_bt_da" UseSubmitBehavior="False" OnClick="btnSFZZMUpload_Click" />
                                &nbsp;&nbsp; <asp:Button ID="btnSFZZMCK" runat="server" Text="查看" CssClass="tj_bt_da" UseSubmitBehavior="False" Visible="False" OnClick="btnSFZZMCK_Click" />
                                &nbsp;&nbsp; <asp:Button ID="btnSFZZMDelete" runat="server" Text="删除" CssClass="tj_bt_da" UseSubmitBehavior="False" Visible="False" OnClick="btnSFZZMDelete_Click" /><asp:Label ID="lblSFZZM" runat="server" Visible="true" Style=" display:none;"></asp:Label>
                                            &nbsp;&nbsp;<asp:Label ID="lblSFZZMYZ" runat="server" ForeColor="Red"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align:right;" colspan="2">
                                            &nbsp;&nbsp;法定代表人身份证反面扫描件：

                                        </td>
                                        <td><asp:FileUpload ID="FULoadSFZFM" runat="server" EnableTheming="True" CssClass="tj_input" UseSubmitBehavior="False" />
                                &nbsp;&nbsp; <asp:Button ID="btnSFZFMUpload" runat="server" Text="上传" CssClass="tj_bt_da" UseSubmitBehavior="False" OnClick="btnSFZFMUpload_Click" />
                                &nbsp;&nbsp; <asp:Button ID="btnSFZFMCK" runat="server" Text="查看" CssClass="tj_bt_da" UseSubmitBehavior="False" Visible="False" OnClick="btnSFZFMCK_Click" />
                                &nbsp;&nbsp; <asp:Button ID="btnSFZFMDelete" runat="server" Text="删除" CssClass="tj_bt_da" UseSubmitBehavior="False" Visible="False" OnClick="btnSFZFMDelete_Click" /><asp:Label ID="lblSFZFM" runat="server" Visible="true" Style=" display:none;"></asp:Label>
                                            &nbsp;&nbsp;<asp:Label ID="lblSFZFMYZ" runat="server" ForeColor="Red"></asp:Label>
                                        </td>
                                    </tr>
                                </table>                              
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                法定代表人授权书：
                            </td>
                            <td colspan="3">
                              <asp:FileUpload ID="FULoadFDRDBSQS" runat="server" EnableTheming="True" CssClass="tj_input" UseSubmitBehavior="False" />
                                &nbsp;&nbsp; <asp:Button ID="btnFDRDBSQSUpload" runat="server" Text="上传" CssClass="tj_bt_da" UseSubmitBehavior="False" OnClick="btnFDRDBSQSUpload_Click" />
                                &nbsp;&nbsp; <asp:Button ID="btnFDRDBSQSCK" runat="server" Text="查看" CssClass="tj_bt_da" UseSubmitBehavior="False" Visible="False" OnClick="btnFDRDBSQSCK_Click" />
                                &nbsp;&nbsp; <asp:Button ID="btnFDRDBSQSDelete" runat="server" Text="删除" CssClass="tj_bt_da" UseSubmitBehavior="False" Visible="False" OnClick="btnFDRDBSQSDelete_Click" /><asp:Label ID="lblFDRSQS" runat="server" Visible="true" Style=" display:none;"></asp:Label>
                                &nbsp;&nbsp;<asp:Label ID="lblFDDBRYZ" runat="server" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                         <tr>
                            <td align="right">
                                交易方联系电话：
                            </td>
                            <td colspan="3">
                              <asp:TextBox ID="txtJYFLXDH" runat="server" CssClass="tj_input" Width="200px"
                                    TabIndex="1" MaxLength="20"></asp:TextBox> 
                                &nbsp;&nbsp;<asp:Label ID="lblJYFLXDHYZ" runat="server" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr >
                            <td align="right">
                                所属区域：
                            </td>
                            <td colspan="3">
                                <table>
                                    <tr>
                                        <td>
                                            <uc1:UCCityList ID="UCCityList1" runat="server" />
                                        </td>
                                        <td>
&nbsp;&nbsp;<asp:Label ID="lblSSQYYZ" runat="server" ForeColor="Red"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                                
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                详细地址：
                            </td>
                            <td colspan="3">
                             <asp:TextBox ID="txtXXDZ" runat="server" class="tj_input" Width="400px" ToolTip="请不要填写省市区信息" MaxLength="40"
                                  ></asp:TextBox>
                                &nbsp;&nbsp;<asp:Label ID="lblXXDZYZ" runat="server" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr id="tr1" runat="server">
                            <td align="right">
                                联系人姓名：
                            </td>
                            <td colspan="3">
                              <asp:TextBox ID="txtLXRXM" runat="server" CssClass="tj_input" Width="200px" 
                                    TabIndex="1" MaxLength="10"></asp:TextBox>
                                &nbsp;&nbsp;<asp:Label ID="lblLXRXMYZ" runat="server" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr id="tr2" runat="server">
                            <td align="right">
                                联系人手机号：
                            </td>
                            <td colspan="3">
                              <asp:TextBox ID="txtLXRSJH" runat="server" CssClass="tj_input" Width="200px"
                                    TabIndex="1" MaxLength="11"></asp:TextBox>
                                &nbsp;&nbsp;<asp:Label ID="lblLXRSJHYZ" runat="server" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr id="tr3" runat="server">
                            <td align="right">
                                预指定开户银行：
                            </td>
                            <td colspan="3">
                              <asp:DropDownList ID="drpBank" runat="server" Width="200px" CssClass="tj_input" >
                                <asp:ListItem Selected="True">请选择预指定开户银行</asp:ListItem>
                                  <asp:ListItem>浦发银行</asp:ListItem>
                                  <asp:ListItem>平安银行</asp:ListItem>
                            </asp:DropDownList>
                                &nbsp;&nbsp;<asp:Label ID="lblBankYZ" runat="server" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr id="tr4" runat="server">
                            <td align="right">
                                交易资金密码：
                            </td>
                            <td colspan="3">
                              <asp:TextBox ID="txtJYZJMM" runat="server" CssClass="tj_input" Width="200px"
                                    TabIndex="1" MaxLength="6"></asp:TextBox>
                                &nbsp;&nbsp;<asp:Label ID="lblJYZJMMYZ" runat="server" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr id="tr5" runat="server">
                            <td align="right">
                                请选择业务管理部门：
                            </td>
                            <td colspan="3">
                                <asp:RadioButtonList ID="RadioButtonList3" runat="server">
                                    <asp:ListItem Enabled="False" Selected="True">平台总部</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr id="tr6" runat="server">
                            <td align="right">
                                请选择经纪人类型：
                            </td>
                            <td colspan="3">
                                <asp:RadioButtonList ID="rdbtnJJRLX" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem>一般经纪人</asp:ListItem>
                                    <asp:ListItem Selected="True">银行</asp:ListItem>
                                    <asp:ListItem>政府</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr id="tr7" runat="server">
                            <td align="right">
                                
                            </td>
                            <td colspan="3">

                                <asp:CheckBox ID="CheckBox2" runat="server" Text="同意　交易账户经纪人开通协议　模拟版测试须知" Checked="True" Enabled="False" />

                            </td>
                        </tr>
                        <tr>
                          <td align="right">
                              &nbsp;
                            </td>
                            <td colspan="3" align="left" style="height: 80px;">
                            <div id="djycqymain" style=" margin-top:10px;">
                              <div id="djycqy_show">
                                <asp:Button ID="btnPass" runat="server" CssClass="tj_bt_da" UseSubmitBehavior="False"
                                    Text="提交" Width="50px" OnClientClick="return Promot(this);" OnClick="btnPass_Click"  />&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="btnReject"
                                        UseSubmitBehavior="False" runat="server" CssClass="tj_bt_da" 
                                        Text="重填" Width="50px"  OnClientClick="return Reset(this);"  />
                                                </div>
                                                </div>
                            </td>
                        </tr>
                    </table>
                  
                </div>
            </div>
        </div>
        
        
    </div>
    
    
    <input runat="server" id="hidID" type="hidden" />
    <script language="javascript" type="text/javascript">
        function OnkeyUp(obj) {
            var len = obj.value.length;
            if (len > 250) {
                obj.value = obj.value.substring(0, 250);
            }
        }
    </script>
    </form>
</body>
</html>
