/*
弹窗方法汇总
*/
/*
confirm弹窗
*/
//<![CDATA[
function art_confirm(this_an, showmsg) {

    if (!this_an.getAttribute("art_sp_s")) {
        this_an.setAttribute('art_sp_s', '0');
    }
    if (this_an.getAttribute("art_sp_s") == '0') {
        window.top.Dialog.confirm(showmsg, function () {
            this_an.setAttribute('art_sp_s', '1');
           this_an.click(); 
        }, function () { this_an.setAttribute('art_sp_s', '0'); });
        return false;
    }
    else {
        this_an.setAttribute('art_sp_s', '0');
        return true;
    }
}
/*防重复提交confirm*/
function art_confirm_fcf(this_an, showmsg,method) {

    if (!this_an.getAttribute("art_sp_s")) {
        this_an.setAttribute('art_sp_s', '0');
    }
    if (this_an.getAttribute("art_sp_s") == '0') {
        window.top.Dialog.confirm(showmsg, function () {
            this_an.setAttribute('art_sp_s', '1');
            eval(method);
            __doPostBack(this_an.id.replace(/_/g, "$"), '');
        }, function () { this_an.setAttribute('art_sp_s', '0'); });
        return false;
    }
    else {
        this_an.setAttribute('art_sp_s', '0');
        return true;
    }
}
function art_WarningConfirm(this_an, showmsg) {

    if (!this_an.getAttribute("art_sp_s")) {
        this_an.setAttribute('art_sp_s', '0');
    }
    if (this_an.getAttribute("art_sp_s") == '0') {
        window.top.Dialog.WarningConfirm(showmsg, function () { this_an.setAttribute('art_sp_s', '1'); this_an.click(); }, function () { this_an.setAttribute('art_sp_s', '0'); });
        return false;
    }
    else {
        this_an.setAttribute('art_sp_s', '0');
        return true;
    }
}
//只能用于LinkButton,id中不能带有下划线。否则出问题
function art_confirm_LinkButton(this_an, showmsg) {

    if (!this_an.getAttribute("art_sp_s")) {
        this_an.setAttribute('art_sp_s', '0');
    }
    if (this_an.getAttribute("art_sp_s") == '0') {
        window.top.Dialog.confirm(showmsg, function () {
            this_an.setAttribute('art_sp_s', '1');
            __doPostBack(this_an.id.replace(/_/g, "$"), '');
        }, function () { this_an.setAttribute('art_sp_s', '0'); });
        return false;
    }
    else {
        this_an.setAttribute('art_sp_s', '0');
        return true;
    }
}
//只能用于LinkButton,id中不能带有下划线。否则出问题
function art_WarningConfirm_LinkButton(this_an, showmsg) {

    if (!this_an.getAttribute("art_sp_s")) {
        this_an.setAttribute('art_sp_s', '0');
    }
    if (this_an.getAttribute("art_sp_s") == '0') {
        window.top.Dialog.WarningConfirm(showmsg, function () {
            this_an.setAttribute('art_sp_s', '1');
            __doPostBack(this_an.id.replace(/_/g, "$"), '');
        }, function () { this_an.setAttribute('art_sp_s', '0'); });
        return false;
    }
    else {
        this_an.setAttribute('art_sp_s', '0');
        return true;
    }
}
//单独弹窗信息
function art_confirm_Message(showmsg) {
    window.top.Dialog.confirm(showmsg);
}
//]]>