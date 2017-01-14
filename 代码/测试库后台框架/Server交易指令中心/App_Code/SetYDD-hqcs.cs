using FMipcClass;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

/// <summary>
/// SetYDD_hqcs 的摘要说明
/// </summary>
public class SetYDD_hqcs
{
    private Hashtable htya = new Hashtable();
    public  Hashtable Get_hqcs(string MoneyDZBNumber,string MJJSBH, string DLYX)
	{
        htya = new Hashtable(); //实例化一个公共变量ht来存放多个线程返回结构
        htya.Clear();

        var task1 = Task.Factory.StartNew(() => YHMessage(MJJSBH));
        var task2 = Task.Factory.StartNew(() => GLJJRMessage(DLYX));
        var task3 = Task.Factory.StartNew(() => DQZHKYYE(DLYX));
        var task4 = Task.Factory.StartNew(() => GetNumber("AAA_YDDXXB", ""));
        var task5 = Task.Factory.StartNew(() => GetNumber("AAA_ZKLSMXB", ""));
        var task6 = Task.Factory.StartNew(() => GetMoneyDZB(MoneyDZBNumber));

        Task.WaitAll(new Task[]{task1, task2, task3, task4, task5, task6});

        if (!(task1.Status == TaskStatus.RanToCompletion && task2.Status == TaskStatus.RanToCompletion && task3.Status == TaskStatus.RanToCompletion && task4.Status == TaskStatus.RanToCompletion && task5.Status == TaskStatus.RanToCompletion && task6.Status == TaskStatus.RanToCompletion))
        {
            return null;
        }
        
        return htya;
    }

    public  Hashtable Get_TBDcs(string MJJSBH, string DLYX)
    {
        


        htya = new Hashtable(); //实例化一个公共变量ht来存放多个线程返回结构
        htya.Clear();

        var task1 = Task.Factory.StartNew(() => YHMessage(MJJSBH));
        var task2 = Task.Factory.StartNew(() => GLJJRMessage(DLYX));

        var task4 = Task.Factory.StartNew(() => GetNumber("AAA_TBD", ""));
        var task5 = Task.Factory.StartNew(() => GetNumber("AAA_ZKLSMXB", ""));
        //var task6 = Task.Factory.StartNew(() => GetMoneyDZB("1304000003"));
        Task.WaitAll(task1, task2, task4,task5);



        if (!(task1.Status == TaskStatus.RanToCompletion && task2.Status == TaskStatus.RanToCompletion && task4.Status == TaskStatus.RanToCompletion && task5.Status == TaskStatus.RanToCompletion ))
        {
            return null;
        }
       
        return htya;
    }

    public  Hashtable Get_LJQcs()
    {
        htya = new Hashtable(); //实例化一个公共变量ht来存放多个线程返回结构
        htya.Clear();

        var task1 = Task.Factory.StartNew(() => GetNumber("AAA_LJQBGLSJLB", ""));
        var task2 = Task.Factory.StartNew(() => GetNumber("AAA_TBZLSHB", ""));
        Task.WaitAll(task1, task2);



        if (!(task1.Status == TaskStatus.RanToCompletion && task2.Status == TaskStatus.RanToCompletion))
        {
            return null;
        }

        return htya;
    }

    #region 为多线程准备的方法 zhouli 2014.09.23
    /// <summary>
    /// 用户基本信息
    /// </summary>
    /// <param name="MJJSBH"></param>
    public  void YHMessage(string MJJSBH)
    {
        object[] re = IPC.Call("用户基本信息", new object[] { MJJSBH });
        htya["用户基本信息"] = re;
    }
    /// <summary>
    /// 关联经纪人信息
    /// </summary>
    /// <param name="DLYX"></param>
    public  void GLJJRMessage(string DLYX)
    {
        object[] re =IPC.Call("关联经纪人信息", new object[] { DLYX });
        htya["关联经纪人信息"] = re;
    }
    /// <summary>
    /// 账户当前可用余额
    /// </summary>
    /// <param name="DLYX"></param>
    public  void DQZHKYYE(string DLYX)
    {
        object[] re = IPC.Call("账户当前可用余额", new object[] { DLYX });
        htya["账户当前可用余额"] = re;
    }

    /// <summary>
    /// 获取主键
    /// </summary>
    /// <param name="tabName">表名</param>
    /// <param name="Prefix">主键前缀</param>
    public  void GetNumber(string tabName, string Prefix)
    {
        object[] re = IPC.Call("获取主键", new object[] { tabName, Prefix });
        htya[tabName+"获取主键"] = re;
    }
    /// <summary>
    /// 平台动态参数
    /// </summary>
    public  void PTDTCS()
    {
        object[] re = IPC.Call("平台动态参数 ", new object[] { "" });
        htya["平台动态参数"] = re;
    }

    private void GetMoneyDZB(string Number)
    {
        TBD tbd = new TBD();
        DataTable dt_LS = tbd.GetMoneydbzDS(Number).Tables["DZB"];
        htya["DZB"] = dt_LS;
    }
    #endregion

}