

//创建控制类
function ControllerClass() {
  
    /*
    判断权限和对权限的处理
    */
    this.Authority = function (operateInfoText, functionInfoText) {
        $.ajax({ "url": "CommonPageAttach.ashx?Methord=JudgeAuthority", "type": "post", "data": { "operateInfoText": operateInfoText, "functionInfoText": functionInfoText },
            "dataType": "json", "cache": false, "async": false, "success": function (responseInfo) {
                if (responseInfo.IsSuccess) {
                    var functionsInfoArray = responseInfo.Data;
                    var strFunParIdColl = functionsInfoArray.FunParIdColl.split(',');
                    //打开左侧的对应目录
                    var p = 0;
                    for (var i = 0; i < strFunParIdColl.length; i++) {
                        if (strFunParIdColl[i] != 0) {
                            //setTimeout("onesub_run(" + strFunParIdColl[i] + ")", p * 2000);
                            onesub_run(strFunParIdColl[i]);
                            p++;
                        }
                    }
                    //标记左侧对应目录
                    window.top.frames['leftFrame'].SignSub(functionsInfoArray.FunName);
                    //打开对应的页面
                    window.top.frames['leftFrame'].menuClick_yhb(functionsInfoArray.FunName, functionsInfoArray.FunAuthrity);

                }
                else {
                    alert("对不起，您没有权限查看此模块！");
                }
            }
        });
    };
  
}

function onesub_run(ii) {

    window.top.frames['leftFrame'].open_onesub(ii);
}

/*
创建操作类对象
*/
function OperateInfo() {
    /*
    用户账号
    */
    this.OperUser = "";
    /*
    用户操作类别 (“默认功能”还是“常用功能”)
    */
    this.OperCate = "";
    /*
    用户操作性质（“增加”还是“删除”还是“查看”）
    */
    this.OperProper = "";
}

/*
创建功能类对象
*/
function FunctionsInfo() {
    /*
    功能ID
    */
    this.FunID = "";
    /*
    功能描述
    */
    this.FunDesc = "";
    /*
    功能全称
    */
    this.FunFullName = "";
    /*
    功能NAME
    */
    this.FunName = "";
    /*
    功能编号
    */
    this.FunIdent = "";
    /*
    功能图片路径
    */
    this.FunPicPath = "";
    /*
    功能类别
    */
    this.FunCate = "";
    /*
    功能类别父类ID集合
    */
    this.FunParIdColl = "";
    /*
    功能权限
    */
    this.FunAuthrity = "";
}    
