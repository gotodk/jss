using FMipcClass;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

/// <summary>
/// hqcs 的摘要说明
/// </summary>
public  class hqcs
{
	

     private Hashtable htya = null;
     public  Hashtable Getht(string HTQX,string MJJSBH, string SPBH) // Getpast()
    {
        #region 用多线程获取验证数据
        //DateTime dtduoxianchen = DateTime.Now;
        //htya = new Hashtable(); //实例化一个公共变量ht来存放多个线程返回结构
        //htya.Clear();
        //ThreadPool.SetMaxThreads(10, 10);
        ////共需要获取N次数据
        ////1.平台动态参数
        //ThreadPool.QueueUserWorkItem(GetPTDTParas, "");
        ////2.获取账户的相关信息
        //ThreadPool.QueueUserWorkItem(GetHQZHXGXXaras, MJJSBH);
        ////3.交易账户开通状态
        //ThreadPool.QueueUserWorkItem(GetJYXHKTZTParas, MJJSBH);
        ////4.商品有效
        //ThreadPool.QueueUserWorkItem(GetSPYXParas, SPBH);
        ////5.检测所有线程是否已运行完毕
        ////Thread.Sleep(5000);
        //while (htya == null || htya.Count != 4)
        //{
        //    ;
        //}
        //DateTime dtduoxianchenend = DateTime.Now;

        //string PTCStime = (dtduoxianchen - dtduoxianchenend).ToString();
        //FMipcClass.Log.WorkLog("C下达预订单", FMipcClass.Log.type.yewu, "多线程运行时间：-" + SPBH + "-执行时间:" + PTCStime, null);

        //return htya;
        #endregion

      
        htya = new Hashtable(); //实例化一个公共变量ht来存放多个线程返回结构
        htya.Clear();  
    
        var task1 = Task.Factory.StartNew(() => GetPTDTParas(""));//获取平台动态参数
        var task2 = Task.Factory.StartNew(() => GetSPYXParas(SPBH));//商品是否有效
        var task3 = Task.Factory.StartNew(() => GetHQZHXGXXaras(MJJSBH));//获取账户的相关信息
        var task4 = Task.Factory.StartNew(() => GetJYXHKTZTParas(MJJSBH));//交易账户开通状态
        GetSPInfo spinfo = new GetSPInfo();
        var task5 = Task.Factory.StartNew(() =>htya["商品信息"]=spinfo.dtSPInfo(SPBH)); //获取商品信息
        var task6 = Task.Factory.StartNew(() => htya["卖家设定经济批量"] = spinfo.Getjjpl(HTQX, SPBH));//获取卖家设定经济批量

        Task.WaitAll(new Task[]{task1, task2, task3, task4, task5, task6});

        if (!(task1.Status == TaskStatus.RanToCompletion && task2.Status == TaskStatus.RanToCompletion && task3.Status == TaskStatus.RanToCompletion && task4.Status == TaskStatus.RanToCompletion && task5.Status == TaskStatus.RanToCompletion && task6.Status == TaskStatus.RanToCompletion))
        {
            return null;
        }    
        
        return htya;
    }
     public  Hashtable GetTBDYZ(string MJJSBH, string SPBH,string HTQX,string DLYX)
     {
         
         htya = new Hashtable(); //实例化一个公共变量ht来存放多个线程返回结构
         htya.Clear();

         var task1 = Task.Factory.StartNew(() => GetPTDTParas(""));
         var task2 = Task.Factory.StartNew(() => GetSPYXParas(SPBH));
         var task3 = Task.Factory.StartNew(() => GetHQZHXGXXaras(MJJSBH));
         var task4 = Task.Factory.StartNew(() => GetJYXHKTZTParas(MJJSBH));
        
         var task5 = Task.Factory.StartNew(() => GetspLJQ(SPBH, HTQX));
         var task6 = Task.Factory.StartNew(() => GetZHKYYE(DLYX));

         Task.WaitAll(task1, task2, task3, task4, task5, task6);
         if (!(task1.Status == TaskStatus.RanToCompletion && task2.Status == TaskStatus.RanToCompletion && task3.Status == TaskStatus.RanToCompletion && task4.Status == TaskStatus.RanToCompletion && task5.Status == TaskStatus.RanToCompletion && task6.Status == TaskStatus.RanToCompletion))
         {
             return null;
         }
       
         
         

       
         return htya;
     
     }

    /// <summary>
     /// 用于capi接口内经纪人审核买卖家信息 驳回操作验证并发取值
    /// </summary>
    /// <returns></returns>
     public Hashtable GetJJRSHBH(string jsbh)
     {
         htya = new Hashtable(); //实例化一个公共变量ht来存放多个线程返回结构
         htya.Clear();

         var task1 = Task.Factory.StartNew(() => GetJYXHKTZTParas(jsbh));
         var task2 = Task.Factory.StartNew(() => GetSfxiumian(jsbh));
         Task.WaitAll(task1, task2);
         if (!(task1.Status == TaskStatus.RanToCompletion && task2.Status == TaskStatus.RanToCompletion ))
         {
             return null;
         }

         return htya;
     }


     /// <summary>
     /// 并行获取开通交易账户的的前置条件，包括是否开通交易账户以及所需关联表主键
     /// </summary>
     /// <param name="dlyx"></param>
     /// <returns></returns>
     public Hashtable GetZhParas(string dlyx)
     {
         htya = new Hashtable();
         htya.Clear();


         var task1 = Task.Factory.StartNew(() => GetFieldJSZH(dlyx));
         var task2 = Task.Factory.StartNew(() => GetNewKey("AAA_MJMJJYZHYJJRZHGLB"));

         Task.WaitAll(task1, task2);

         if (!(task1.Status == TaskStatus.RanToCompletion && task2.Status == TaskStatus.RanToCompletion))
         {
             return null;
         }

         return htya;

     }
   
    #region 为多线程准备的方法 wyH 2014.09.22
    /// <summary>
     /// 平台动态参数
    /// </summary>
    /// <param name="test">""</param>
    private  void GetPTDTParas(object test)
    {
        #region 基础验证
        object[] re = IPC.Call("平台动态参数", new object[] { "" });
        htya["平台动态参数"] = re;


        #endregion
    }

    /// <summary>
    /// 商品有效
    /// </summary>
    /// <param name="test">商品编号</param>
    private  void GetSPYXParas(object test)
    {
        #region 基础验证
        object[] re = IPC.Call("商品有效", new object[] { test.ToString() });
        htya["商品有效"] = re;
        #endregion
    }

    /// <summary>
    /// 获取账户的相关信息
    /// </summary>
    /// <param name="test">买卖家角色编号</param>
    private  void GetHQZHXGXXaras(object test)
    {
        #region 基础验证
        object[] re = IPC.Call("获取账户的相关信息", new object[] { test.ToString() });
        htya["获取账户的相关信息"] = re;


        #endregion
    }

    /// <summary>
    /// 交易账户开通状态
    /// </summary>
    /// <param name="test">买卖家角色编号</param>
    private  void GetJYXHKTZTParas(object test)
    {
        #region 基础验证    
        object[] re = IPC.Call("交易账户开通状态", new object[] { test.ToString() });
        htya["交易账户开通状态"] = re;
        #endregion
    }

    /// <summary>
    /// 商品是否冷静期
    /// </summary>
    /// <param name="test"></param>
    /// <param name="htqx"></param>
    private  void GetspLJQ(string test,string htqx)
    {
        object[] re = IPC.Call("是否冷静期", new object[] { test, htqx });
        htya["是否冷静期"] = re;
    }

    /// <summary>
    /// 账户可用余额
    /// </summary>
    private  void GetZHKYYE(string DLYX)
    {
        object[] re = IPC.Call("账户当前可用余额", new object[] { DLYX });
        htya["账户当前可用余额"] = re;
    }

    /// <summary>
    /// 判断是否休眠
    /// </summary>
    /// <param name="jjrbh"></param>
    private void GetSfxiumian( string jjrbh)
    {
       
        object[] re = IPC.Call("是否休眠", new object[] { jjrbh });
        htya["是否休眠"] = re;
       
    }

    /// <summary>
    /// 获取用户信息中结算账户类型的当前状态
    /// </summary>
    /// <param name="userinfo"></param>
    private void GetFieldJSZH(string userinfo)
    {
        object[] re = IPC.Call("审核状态", new object[] { userinfo });
        htya["结算账户类型状态"] = re;

    }
    /// <summary>
    /// 获取主键
    /// </summary>
    /// <param name="tbname"></param>
    private void GetNewKey(string tbname)
    {
        object[] reynum = IPC.Call("获取主键", new object[] { tbname, "" });
        htya["主键"] = reynum;

    }

    public void SendTX(DataSet dt)
    {

        Task task1 = new Task(() => IPC.Call("发送提醒", new object[] { dt })); //Task.Factory.StartNew(() => xyjf.DealXYJF(dt, "经纪人驳回买卖家"));
        task1.Start();
    }


    #endregion
}