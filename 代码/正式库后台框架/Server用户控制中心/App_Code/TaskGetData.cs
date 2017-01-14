using FMDBHelperClass;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

/// <summary>
/// TaskGetData 的摘要说明
/// </summary>
public class TaskGetData
{


    //链接
     static I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
     private Hashtable htya = null;

     public Hashtable GetData(string strSelerJSBH, string strGLJJRBH, string strZCLB, string strMMJDLYX)
    {
        htya = new Hashtable();
        htya.Clear();


        var task1 = Task.Factory.StartNew(() => IsGHZCLB(strSelerJSBH, strGLJJRBH, strZCLB));
        var task2 = Task.Factory.StartNew(() => IsJJRPassMMJ(strMMJDLYX, strGLJJRBH));
        //task1.Start();
        Task.WaitAll(task1, task2);
        if (!(task1.Status == TaskStatus.RanToCompletion && task2.Status == TaskStatus.RanToCompletion))
        {
            return null;
        }



        return htya;
    }

    /// <summary>
    /// 判断当前买卖家交易账户 是否更换了注册类别
    /// </summary>
    /// <param name="strSelerJSBH">卖家角色编号</param>
    /// <param name="strGLJJRBH">关联经纪人角色编号</param>
    /// <param name="strZCLB">当前注册类别</param>
    /// <returns>否 没有更换注册类别，是 已经更换了注册类别</returns>
    public  void IsGHZCLB(string strSelerJSBH, string strGLJJRBH, string strZCLB)
    {
        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start(); //  开始监视代码运行时间
        DataSet ds = new DataSet();
        Hashtable param = new Hashtable();

        param.Add("@mjjsbh", strSelerJSBH);
        param.Add("@jjrjsbh", strGLJJRBH);
        param.Add("@B_JSZHLX", "买家卖家交易账户");
        param.Add("@SFYX", "是");
        param.Add("@SFSCGLJJR", "是");
        param.Add("@SFDQMRJJR", "是");
        
      //  string strSql = "select b.I_ZCLB '注册类别' from  AAA_MJMJJYZHYJJRZHGLB  as a inner join AAA_DLZHXXB as b on a.DLYX=b.B_DLYX where  b.B_JSZHLX='买家卖家交易账户' and a.SFYX='是' and a.GLJJRBH=@jjrjsbh and b.J_SELJSBH=@mjjsbh and a.SFSCGLJJR='是' and a.SFDQMRJJR='是'";
        string strSql = "select b.I_ZCLB '注册类别' from  AAA_MJMJJYZHYJJRZHGLB  as a inner join AAA_DLZHXXB as b on a.DLYX=b.B_DLYX where  b.B_JSZHLX=@B_JSZHLX and a.SFYX=@SFYX and a.GLJJRBH=@jjrjsbh and b.J_SELJSBH=@mjjsbh and a.SFSCGLJJR=@SFSCGLJJR  and a.SFDQMRJJR=@SFDQMRJJR";

        Hashtable return_ht = I_DBL.RunParam_SQL(strSql, "dtResult", param);
        //Hashtable return_ht = I_DBL.RunProc(strSql, "dtResult");
       
        if ((bool)(return_ht["return_float"])) //说明执行完成
        {
            ds = (DataSet)return_ht["return_ds"];
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {

                DataTable dt = ds.Tables[0];
                if (dt.Rows[0]["注册类别"].ToString().Trim() == strZCLB.Trim())
                    htya["是否注册类别"] = "否";
                else
                    htya["是否注册类别"] = "是";

            }

            else
                htya["是否注册类别"] = "";
        }

     
        stopwatch.Stop();
        TimeSpan tszclb = stopwatch.Elapsed;
        FMipcClass.Log.WorkLog("C下达预订单", FMipcClass.Log.type.yewu, "C 判断当前买卖家交易账户 是否更换了注册类别：" + tszclb.TotalMilliseconds.ToString("#.#####"), null);

       


    }


    /// <summary>
    /// 判断当前经纪人是否已经把当前买卖家审核通过
    /// </summary>
    /// <param name="strMMJDLYX">买卖家登录邮箱</param>
    /// <param name="strGLJJRBH">关联经纪人角色编号</param>
    /// <returns>是/否</returns>
    public  void IsJJRPassMMJ(string strMMJDLYX, string strGLJJRBH)
    {

        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start(); //
        DataSet ds = new DataSet();
        Hashtable param = new Hashtable();

        param.Add("@mmjdlyx", strMMJDLYX);
        param.Add("@jjrjsbh", strGLJJRBH);
        param.Add("@SFYX", "是");
        param.Add("@JJRSHZT", "审核通过");
        param.Add("@SFSCGLJJR", "是");
        param.Add("@SFDQMRJJR", "是");

        string strSql = "select Number from AAA_MJMJJYZHYJJRZHGLB where DLYX=@mmjdlyx and GLJJRBH=@jjrjsbh and SFYX=@SFYX and JJRSHZT=@JJRSHZT and SFSCGLJJR=@SFSCGLJJR and SFDQMRJJR=@SFDQMRJJR";
       // string strSql = "select Number from AAA_MJMJJYZHYJJRZHGLB where DLYX=@mmjdlyx and GLJJRBH=@jjrjsbh and SFYX='是' and JJRSHZT='审核通过' and SFSCGLJJR='是' and SFDQMRJJR='是'";
        Hashtable return_ht = I_DBL.RunParam_SQL(strSql, "dtResult", param);
        //Hashtable return_ht = I_DBL.RunProc(strSql, "dtResult");

       
        if ((bool)(return_ht["return_float"])) //说明执行完成
        {
            ds = (DataSet)return_ht["return_ds"];
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                 htya["是否审核通过"] = "是";

            else
                htya["是否审核通过"] = "否";
        }

        stopwatch.Stop();
        TimeSpan tszclb = stopwatch.Elapsed;
        FMipcClass.Log.WorkLog("C下达预订单", FMipcClass.Log.type.yewu, "C 判断当前经纪人是否已经把当前买卖家审核通过 ：" + tszclb.TotalMilliseconds.ToString("#.#####"), null);
    }

    public void CountJifen(DataTable dt)
    {
         XYDJJFCL xyjf = new XYDJJFCL();
           Task  task1 = new Task(()=>xyjf.DealXYJF(dt, "经纪人驳回买卖家")); //Task.Factory.StartNew(() => xyjf.DealXYJF(dt, "经纪人驳回买卖家"));
         task1.Start();
    }

}