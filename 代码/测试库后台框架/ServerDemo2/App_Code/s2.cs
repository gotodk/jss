using FMipcClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.Services;

/// <summary>
/// s2 的摘要说明
/// </summary>
[WebService(Namespace = "http://ServerDemo2.ipc.com/", Description = "V1.01->这是这个接口的描述xxx")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class s2 : System.Web.Services.WebService {

    public s2 () {

        //如果使用设计的组件，请取消注释以下行 
        //InitializeComponent(); 
    }

    /// <summary>
    /// 测试该接口是否还活着(每个接口必备)
    /// </summary>
    /// <param name="temp">随便传</param>
    /// <returns>返回ok就是接口正常</returns>
    [WebMethod(Description = "测试该接口是否还活着(每个接口必备)")]
    public string onlinetest(string temp)
    {
        //根据不同的传入值，后续可以检查不同的东西，比如这个接口所连接的数据库，比如进程池，服务器空间等等。。。
        return "ok";
    }


    [WebMethod(Description = "这是说明")] 
    public string test3335(string canshu1,string canshu2) {

        DataSet ds = new DataSet();
        ds.Tables.Add(new DataTable());
        ds.Tables[0].TableName = "sss";
        ds.Tables[0].Columns.Add("L1");

        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start(); //  开始监视代码运行时间

        object[] re = IPC.Call("模拟测试2", new object[] {1, ds });
        stopwatch.Stop(); //  停止监视
        TimeSpan hs = stopwatch.Elapsed;
        using (System.IO.StreamWriter sw = new System.IO.StreamWriter("D://log_temp.txt", true))
        {
            sw.WriteLine("总耗时" + hs.TotalMilliseconds.ToString("#.#####"));
        }
  

        //远程方法执行成功（这里的成功，仅仅是程序上的运行成功。只代表程序运行没有出错，不代表业务成功。业务是否成功，需要根据每个方法自己定义的返回值规则来定。按照预定应该是yewuok:xxx或者yewuerr:xxxx）
        if (re[0].ToString() == "ok")
        {
            //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
            string reFF = (string)(re[1]);
            return reFF;
        }
        else  
        {
            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。
            string reFF = re[1].ToString();
            return reFF;
        }
        
    }
    
}
