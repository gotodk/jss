using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Text;
using System.Collections;
using System.Data;
using System.Net;
using System.IO;
using Galaxy.ClassLib.DataBaseFactory;
using System.Configuration;
using Hesion.Brick.Core.WorkFlow;
using FMOP.DB;

/// <summary>
///WebServicePublic 的摘要说明
/// </summary>
[WebService(Namespace = "http://www.fm8844.com/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
//若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。 
// [System.Web.Script.Services.ScriptService]
public class WebServicePublic : System.Web.Services.WebService {

    public WebServicePublic () {

        //如果使用设计的组件，请取消注释以下行 
        //InitializeComponent(); 
    }

    /// <summary>
    /// 测试获得一个字符串
    /// </summary>
    /// <returns></returns>
    [WebMethod]
    public string test_str() {
        return "标记：正式库";
    }

    /// <summary>
    /// 测试获得一个数据集
    /// </summary>
    /// <returns></returns>
    [WebMethod]
    public DataSet test_ds()
    {
        DataSet ds = DbHelperSQL.Query("select top 200 '标记'='正式库',id,module,operatetype,operatetime,[user],ip from FMEventLog order by operatetime desc");
        return ds;
    }
    
}
