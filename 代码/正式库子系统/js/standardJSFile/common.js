$(document).ready(function () {
    /*全选与非全选
    例：<input id="checkboxAll"  type="checkbox" value="全选" />
    只需将input控件id更改为checkboxAll      */
    $("#checkboxAll,.checkboxAll").click(function () {
        if ($(this).attr("checked") == "checked") {
            $('input:checkbox.checkbox').each(function () {
                $(this).attr("checked", true);
            });
        }
        else {
            $('input:checkbox.checkbox').each(function () {
                $(this).attr("checked", false);
            });
        }
    });
});