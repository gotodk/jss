using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using FMipcClass;
using System.IO;
using System.Threading;

/// <summary>
/// 处理来自接口聚合中心的系统级内部指令。与业务逻辑无关。每个接口都应该有，名称固定，名空间也固定。
/// </summary>
[WebService(Namespace = "http://ipc.ipc.com/", Description = "V1.00->处理来自接口聚合中心的系统级内部指令。与业务逻辑无关。每个接口都应该有，名称固定，名空间也固定。")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
// [System.Web.Script.Services.ScriptService]
public class OrderFormIPC : System.Web.Services.WebService {

    public OrderFormIPC () {

        //如果使用设计的组件，请取消注释以下行 
        //InitializeComponent(); 
    }


    /// <summary>
    ///  重置这个域下的服务。先回收本应用程序池进程，再获取最新的关系存到本地，再重新生成代理类。
    /// </summary>
    /// <param name="temp">随便传个""，没用</param>
    /// <returns></returns>
    [WebMethod(Description = "重置这个域下的服务。先回收本应用程序池进程，再获取最新的关系存到本地，再重新生成代理类。")]
    public string ReSetThisWebServices(string temp)
    {

        string a1 = HelpConfig.huishou(); //回收应用程序池
        string a2 = HelpConfig.huoquguanxi();//获取最新关系到本地
        string a3 = HelpConfig.shengchengdaili();//重新生成代理类
        return "回收应用程序池" + a1 + "___获取最新关系到本地" + a2 + "___重新生成代理类" + a3 + "|||";
    }


}
