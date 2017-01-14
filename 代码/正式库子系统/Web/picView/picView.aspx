<%@ Page Language="C#" AutoEventWireup="true" CodeFile="picView.aspx.cs" Inherits="Web_picView_picView" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=gb2312" />
    <title>ͼƬ�鿴��</title>
    <script src="CJL.0.1.min.js"></script>
    <script src="ImageTrans.js"></script>
    <style>
        #idContainer
        {
            border: 1px solid #CCC;
            width: 800px;
            height: 550px;
            background: #FFF center no-repeat;
        }
        .tj_bt_da
        {
            border: 1px solid #999999;
            padding-left: 5px;
            padding-right: 5px;
            width: 80px;
            height: 25px;
            overflow: visible;
            background: url("../images/standardImageFile/huibg2.jpg");
        }
    </style>
</head>
<body>
    <table align="center" cellpadding="0" cellspacing="0" border="0">
        <tr>
            <td align="center">
                <div id="idContainer">
                </div>
            </td>
        </tr>
        <tr>
            <td align="center" height="35px">
                <input id="idLeft" type="button" value="������ת" class="tj_bt_da" />
                <input id="idRight" type="button" value="������ת" class="tj_bt_da" />
                <input id="idVertical" type="button" value="��ֱ��ת" class="tj_bt_da" />
                <input id="idHorizontal" type="button" value="ˮƽ��ת" class="tj_bt_da" />
                <input id="idReset" type="button" value="����" class="tj_bt_da" />
                <input id="idReal" type="button" value="�鿴ԭͼ" class="tj_bt_da" />
            </td>
        </tr>
    </table>
    <input id="idSrc" type="hidden" runat ="server" />
    <%--<input id="idSrc" type="hidden" runat ="server"  value='<%=Request["path"].ToString()%>' />--%>
    <script> 
        (function () {
            var container = $$("idContainer"), src = $$("idSrc").value,
	options = {
	    onPreLoad: function () { container.style.backgroundImage = "url('loading.gif')"; },
	    onLoad: function () { container.style.backgroundImage = ""; }
	},
	it = new ImageTrans(container, options);

            it.load(src);
            //��ֱ��ת
            $$("idVertical").onclick = function () { it.vertical(); }
            //ˮƽ��ת
            $$("idHorizontal").onclick = function () { it.horizontal(); }
            //����ת
            $$("idLeft").onclick = function () { it.left(); }
            //����ת
            $$("idRight").onclick = function () { it.right(); }
            //����
            $$("idReset").onclick = function () { it.reset(); }
            //            //��ͼ
            //            $$("idLoad").onclick = function () { it.load($$("idSrc").value); }
            //����
            $$("idReal").onclick = function () { window.open(src); }

        })()

    </script>
</body>
</html>
