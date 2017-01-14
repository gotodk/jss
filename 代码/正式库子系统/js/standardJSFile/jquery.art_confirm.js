
var __artConfirmOperner;
(function ($) {
    /*
    Jquery对象级别插件(基于框架的)
    */
    $.fn.art_confirm = function (options) {
        $.fn.art_confirm.defaults =
    {
        name: "diag1",
        width: 463,
        height: 400,
        title: "模块",
        url: "LeftContent.aspx?FunIdent=1",
        showmessagerow: false,
        showbuttonrow: false,
        optionName: "",
        IsReady: function (object, dialog) { }
    };
        // default options used on initialisation
        // and arguments used on later calls
        var opts = $.extend({}, $.fn.art_confirm.defaults, options);
        var args = arguments;
        /*
        * Entry point
        */
         
        return this.each(function () {
          if($(this).get(0).tagName=="INPUT")
          {
           if($(this).get(0).getAttribute("type")=="button")
           {
             $(this).bind("click", function () {
                __artConfirmOperner = new window.top.Dialog(opts.name);
               __artConfirmOperner.Width = opts.width;
               __artConfirmOperner.Height = opts.height;
               __artConfirmOperner.Title = opts.title;
               __artConfirmOperner.URL = $.UrlUpdateParams(opts.url, "optionName", opts.optionName);
               __artConfirmOperner.ShowMessageRow = opts.showmessagerow;
               __artConfirmOperner.ShowButtonRow = opts.showbuttonrow;
               __artConfirmOperner.show();
            });
           }
           else
           {
            $(this).bind("focusin", function () {
                __artConfirmOperner = new window.top.Dialog(opts.name);
               __artConfirmOperner.Width = opts.width;
               __artConfirmOperner.Height = opts.height;
               __artConfirmOperner.Title = opts.title;
               __artConfirmOperner.URL = $.UrlUpdateParams(opts.url, "optionName", opts.optionName);
               __artConfirmOperner.ShowMessageRow = opts.showmessagerow;
               __artConfirmOperner.ShowButtonRow = opts.showbuttonrow;
               __artConfirmOperner.show();
            });
            }
         }
         else
         {
           $(this).bind("click", function () {
                __artConfirmOperner = new window.top.Dialog(opts.name);
               __artConfirmOperner.Width = opts.width;
               __artConfirmOperner.Height = opts.height;
               __artConfirmOperner.Title = opts.title;
               __artConfirmOperner.URL = $.UrlUpdateParams(opts.url, "optionName", opts.optionName);
               __artConfirmOperner.ShowMessageRow = opts.showmessagerow;
               __artConfirmOperner.ShowButtonRow = opts.showbuttonrow;
               __artConfirmOperner.show();
            });
         }
        });
    }
    /*
      Jquery对象级别插件(基于详情的页面)
    */
      $.fn.art_confirmOther = function (options) {
        $.fn.art_confirmOther.defaults =
    {
        name: "diag1",
        width: 463,
        height: 400,
        title: "模块",
        url: "LeftContent.aspx?FunIdent=1",
        showmessagerow: false,
        showbuttonrow: false,
        optionName: "",
        IsReady: function (object, dialog) { }
    };
        // default options used on initialisation
        // and arguments used on later calls
        var opts = $.extend({}, $.fn.art_confirmOther.defaults, options);
        var args = arguments;
        /*
        * Entry point
        */
        return this.each(function () {
          if($(this).get(0).tagName=="INPUT")
          {
           if($(this).get(0).getAttribute("type")=="button")
           {
             $(this).bind("click", function () {
                __artConfirmOperner = new window.top.Dialog(opts.name);
               __artConfirmOperner.Width = opts.width;
               __artConfirmOperner.Height = opts.height;
               __artConfirmOperner.Title = opts.title;
               __artConfirmOperner.URL = $.UrlUpdateParams(opts.url, "optionName", opts.optionName);
               __artConfirmOperner.ShowMessageRow = opts.showmessagerow;
               __artConfirmOperner.ShowButtonRow = opts.showbuttonrow;
               __artConfirmOperner.show();
            });
           }
           else
           {
            $(this).bind("focusin", function () {
                __artConfirmOperner = new window.top.Dialog(opts.name);
               __artConfirmOperner.Width = opts.width;
               __artConfirmOperner.Height = opts.height;
               __artConfirmOperner.Title = opts.title;
               __artConfirmOperner.URL = $.UrlUpdateParams(opts.url, "optionName", opts.optionName);
               __artConfirmOperner.ShowMessageRow = opts.showmessagerow;
               __artConfirmOperner.ShowButtonRow = opts.showbuttonrow;
               __artConfirmOperner.show();
            });
            }
         }
         else
         {
           $(this).bind("click", function () {
                __artConfirmOperner = new window.top.Dialog(opts.name);
               __artConfirmOperner.Width = opts.width;
               __artConfirmOperner.Height = opts.height;
               __artConfirmOperner.Title = opts.title;
               __artConfirmOperner.URL = $.UrlUpdateParams(opts.url, "optionName", opts.optionName);
               __artConfirmOperner.ShowMessageRow = opts.showmessagerow;
               __artConfirmOperner.ShowButtonRow = opts.showbuttonrow;
               __artConfirmOperner.show();
            });
         }

        });
    }
    /*
    类级别的Jquery插件
    */
    $.extend({
        Request: function (m) {
            var sValue = location.search.match(new RegExp("[\?\&]" + m + "=([^\&]*)(\&?)", "i"));
            return sValue ? sValue[1] : sValue;
        },
        UrlUpdateParams: function (url, name, value) {
            var r = url;
            if (r != null && r != 'undefined' && r != "") {
                value = encodeURIComponent(value);
                var reg = new RegExp("(^|)" + name + "=([^&]*)(|$)");
                var tmp = name + "=" + value;
                if (url.match(reg) != null) {
                    r = url.replace(eval(reg), tmp);
                }
                else {
                    if (url.match("[\?]")) {
                        r = url + "&" + tmp;
                    } else {
                        r = url + "?" + tmp;
                    }
                }
            }
            return r;
        }
    });
})(jQuery);   