using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using FMDBHelperClass;
using FMipcClass;
using System.Collections;
using System.Data;

[WebService(Namespace = "http://order.ipc.com/", Description = "V1.00->交易指令中心业务接口")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]

public class order : System.Web.Services.WebService
{
    public order()
    {

        //如果使用设计的组件，请取消注释以下行
        //InitializeComponent(); 
    }

    /// <summary>
    /// 测试该接口是否还活着(每个接口必备)
    /// </summary>
    /// <param name="temp">随便传</param>
    /// <returns>返回ok就是接口正常</returns>
    [WebMethod(Description = "（交易指令中心）测试该接口是否还活着(每个接口必备)")]
    public string onlinetest(string temp)
    {
        //根据不同的传入值，后续可以检查不同的东西，比如这个接口所连接的数据库，比如进程池，服务器空间等等。。。
       // return "ok";
       // return (string)(null);
        DataTable dtJK = new DataTable("td");
        dtJK.Columns.Add("商品编号");
        dtJK.Columns.Add("合同期限");
        dtJK.Rows.Add(new string[] { "7L0002", "一年" });

        //需调用成交处理中心方法---------------------------------

       

       object[] re = IPC.Call("成交处理", new object[] { dtJK, "提交" });

       return re[1].ToString();
        
    }

    /// <summary>
    /// 发布投标单--归属交易指令中心 wyh 2014.05.21
    /// </summary>
    /// <param name="ds">参数</param>
    /// <returns>成功：插入成功；非成功：插入失败，失败原因 ；null :程序出现异常</returns>
    [WebMethod(Description = "（交易指令中心）发布投标单--返回值定义：成功：插入成功；非成功：插入失败，失败原因；null :程序出现异常")]
    public string SetTBD_JYZLZX(DataSet ds)
    {
        try
        {
            string strResul = "失败";
            TBD t = new TBD();
            strResul = t.SetTBD(ds);
            return strResul;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 下达投标单草稿--归属交易指令中心 wyh 2014.05.22
    /// </summary>
    /// <param name="ds">参数</param>
    /// <returns>成功：插入成功；非成功：插入失败，失败原因 ；null :程序出现异常</returns>
    [WebMethod(Description = "（交易指令中心）下达投标单草稿--返回值定义：成功：插入成功；非成功：插入失败，失败原因 ；null :程序出现异常")]
    public string SetTBDCG_JYZLZX(DataSet ds)
    {
        try
        {
            string strResul = "失败";
            TBD t = new TBD();
            strResul = t.SetTBDCG(ds);
            return strResul;

        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 投标单修改 wyh 2014.05.24
    /// </summary>
    /// <param name="Number">投标单号</param>
    /// <returns></returns>
     [WebMethod(Description = "（交易指令中心）投标单撤销,返回符合dsreturn结构的Dataset")]
    public DataSet TBD_CX_JYZLZX(string Number)
    {
        DataSet ds = null;
        try
        {
            TBD t = new TBD();
            ds = t.TBD_CX(Number);
            return ds;
        }
        catch
        {
            return null;
        }
       
    }

    /// <summary>
     /// 异常投标单修改
     /// </summary>
     /// <param name="dt">参数列表</param> 
     /// <returns>返回修改成功或失败</returns>
    [WebMethod(Description = "（交易指令中心）异常投标单修改,成功：操作成功；非成功：失败原因；null:程序出错。")]
    public string UpdateTBDYCZZ_JYZLZX(DataSet dt)
     {

         //开始执行 
         try
         {
             TBD bank = new TBD();
             return bank.Update(dt);
         }
         catch
         {
              return  null;
         }

         
     }

    /// <summary>
    /// 查询投标单的异常资质 wyh 2014.05.30 
    /// </summary>
    /// <param name="ds">参数集合</param>
    /// <returns>返回符合dsreturn的Dataset包含标明为”主表“的Datatable</returns>
    [WebMethod(Description = "（交易指令中心）查询投标单的异常资质,返回符合dsreturn的Dataset包含标明为”主表“的Datatable。")]
    public DataSet CXTBDYCZZ_JYZLZX(DataSet ds)
    {
        try
        {
            return (new TBD()).CXTBDYCZZ(ds);
        }
        catch
        {
            return null;
        }
 
    }

    /// <summary>
     /// 投标单审核状态
     /// </summary>
     /// <param name="tbdh">投标单号</param>
     /// <returns>审核状态</returns>
    [WebMethod(Description = "（交易指令中心）交易管理部投标单是否审核,审核状态：审核通过，；null:程序出错。")]
    public string TBDSHZT_JYZLZX(string tbdh)
     {
         try
         {
             TBD bank = new TBD();
             return bank.TBDSHZT(tbdh);
         }
         catch
         {
             return null;
         }
     }

    /// <summary>
     /// 经纪人暂停或恢复买卖家交易 wyh 2014.05.27 add
      /// </summary>
      /// <param name="dataTable">参数集合</param>
     /// <returns>返回符合dsreturn结构Dataset</returns>
    [WebMethod(Description = "（交易指令中心）经纪人暂停或恢复买卖家交易,返回符合dsreturn结构Dataset")]
    public DataSet SetSFZTJY_JYZLZX(DataSet dataTable)
     {
         try
         {
             JJRYEGL jyzt = new JJRYEGL();
             return jyzt.SetSFZTJY(dataTable);
         }
         catch
         {
             return null;
         }
     }

    /// <summary>
    /// 经纪人暂停、恢复新用户审核 wyh 2014.05.27
    /// </summary>
    /// <param name="dt">参数集合</param>
    /// <returns>返回符合dsreturn结构Dataset</returns>
    [WebMethod(Description = "（交易指令中心） 经纪人暂停、恢复新用户审核 ,返回符合dsreturn结构Dataset")]
    public DataSet SetSFZTSH_JYZLZX(DataSet dt)
    {
        try
        {
            JJRYEGL jjryw = new JJRYEGL();
            return jjryw.SetSFZTSH(dt.Tables[0]);
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 投标单修改时返回投标单原始信息
    /// </summary>
    /// <param name="Number">投标单号</param>
    /// <returns>返回符合dsreturn结构Dataset，同时含有表明为：SPXX的Datatable</returns>
    [WebMethod(Description = "（交易指令中心）投标单修改时返回投标单原始信息 ,返回符合dsreturn结构Dataset，同时含有表明为：SPXX的Datatable")]
    public DataSet Tbd_Change_JYZLZX(string Number)
    {
        try
        {
            TBD tbd = new TBD();
            return tbd.Tbd_Change(Number);
        }
        catch
        {
            return null;
        }
 
    }

    /// <summary>
    /// 预订单提交 wyh 2014.05.27 add 
    /// </summary>
    /// <param name="ds">参数集合</param>
    /// <returns>返回字符串：成功：保存成功；非成功字符：失败原因；null:程序出现Bug</returns>
    [WebMethod(Description = "（交易指令中心）预订单提交 ,返回字符串：成功：保存成功；非成功字符：失败原因；null:程序出现Bug")]
    public string SetYDD_JYZLZX(DataSet ds)
    {
        try
        {
            YDD ydd = new YDD();
            return ydd.SetYDD(ds);
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 预订单草稿提交 wyh 2014.05.27 
    /// </summary>
    /// <param name="ds">参数集合</param>
    /// <returns>返回字符串：成功：保存成功；非成功字符：失败原因；null:程序出现Bug</returns>
    [WebMethod(Description = "（交易指令中心）预订单草稿提交 ,返回字符串：成功：保存成功；非成功字符：失败原因；null:程序出现Bug")]
    public string SetYDDCG_JYZLZX(DataSet ds)
    {
        try
        {
            YDD ydd = new YDD();
           return ydd.SetYDDCG(ds);
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 修改预订单（本方法主要用于返回撤销后的订单信息，以及已撤销的订单的商品的最新信息） wyh 2014.05.27 
    /// </summary>
    /// <param name="Number">已经撤销的预订单号</param>
    /// <returns>返回符合dsreturn结构的dataset,包含有表明为SPXX的datatable</returns>
    [WebMethod(Description = "（交易指令中心）本方法主要用于返回撤销后的订单信息，以及已撤销的订单的商品的最新信息 ,返回符合dsreturn结构的dataset,包含有表明为SPXX的datatable")]
    public DataSet Ydd_Change_JYZLZX(string Number)
    {
        try
        {
            YDD ydd = new YDD();
            return ydd.Ydd_Change(Number);
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 撤销预订单
    /// </summary>
    /// <param name="dsreturn">要返回的DataSet</param>
    /// <param name="Number">预订单号</param>
    /// <returns>dsreturn</returns>
    [WebMethod(Description = "（交易指令中心）本方法主要用于预订单撤销，返回符合dsreturn结构的dataset")]
    public DataSet Ydd_CX_JYZLZX(string Number)
    {
        try
        {
            YDD ydd = new YDD();
            return ydd.Ydd_CX(Number);
        }
        catch
        {
            return null;
        }

    }

    /// <summary>
    /// 得到投标单的信息 用户电子购货合同投标单查看与下载
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    [WebMethod(MessageName = "得到投标单的信息HT", Description = "得到投标单的信息电子购货合同使用")]
    public DataSet Get_TBDData_JYZLZX(DataTable dt)
    {
        try
        {
            TBD tbd = new TBD();
            return tbd.Get_TBDData(dt);
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 得到投标单的信息 用户电子购货合同投标单查看与下载
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    [WebMethod(MessageName = "得到预订单的信息HT", Description = "得到预订单的信息电子购货合同使用")]
    public DataSet Get_YDDData_JYZLZX(DataTable dt)
    {
        try
        {
            YDD tbd = new YDD();
            return tbd.Get_YDDData(dt);
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 获取预订单草稿信息 wyh２０１４．０７．１４
    /// </summary>
    /// <param name="Number"></param>
    /// <returns></returns>
    [WebMethod(MessageName = "获取预订单草稿信息", Description = "获取预订单草稿信息-C区草稿")]
    public DataSet GetYDDcaogaoinfo_JYZLZX(string Number)
    {
        try
        {
            YDD tbd = new YDD();
            return tbd.YDD_Getcaogao(Number);
        }
        catch
        {
            return null;
        }
    }


    /// <summary>
    /// 获取投标单草稿信息 wyh２０１４．０７．１４
    /// </summary>
    /// <param name="Number"></param>
    /// <returns></returns>
    [WebMethod(MessageName = "获取投标单草稿信息", Description = "获取投标单草稿信息-C区草稿")]
    public DataSet TBD_GetCGinfo_JYZLZX(string Number)
    {
        try
        {
            TBD tbd = new TBD();
            return tbd.TBD_GetCGinfo(Number);
        }
        catch
        {
            return null;
        }
    }



   
 

}