using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Collections;
/// <summary>
///ServiceHSSL 的摘要说明
/// </summary>
[WebService(Namespace = "http://fwpt.fm8844.com/FWPTZS/webservices")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class ServiceHSSL : System.Web.Services.WebService {

    public ServiceHSSL () {

        //如果使用设计的组件，请取消注释以下行 
        //InitializeComponent(); 
    }

    /// <summary>
    /// 获取某个型号、类别的空鼓当前剩余可用于计算服务站点编号的数量；适用于交易中心监控程序调用。
    /// </summary>
    /// <param name="UserNumber">用户编号</param>
    /// <param name="FWZDBH">服务站点编号</param>
    /// <param name="lx">类型</param>
    /// <param name="xh">型号</param>
    /// <param name="sl">数量</param>
    /// <param name="pass">密码</param>
    /// <returns></returns>
     [WebMethod]
    public int GetProfitKGCanUseByWhere(string UserNumber, string FWZDBH, string lx, string xh, int sl, string pass)
    {
        KGclassYH ky = new KGclassYH();
        int rCount = 0;

        if (pass == "fmFun")
        {
            try
            {

                rCount = ky.GetProfitKGCanUseByWhere(ky.GetAllProfitKGCanUseByUserNumber(UserNumber.Trim(), FWZDBH.Trim(), null), lx.Trim(), xh.Trim(), sl);
            }
            catch (System.Exception ex)
            {

                rCount = 88888888;
            }

        }
        return rCount;
    }

    /// <summary>
    /// 获取本次业务可以免费的回收箱箱号，用户《交易中心》  liujie 2012-02-21
    /// <param name="UserNumber">用户编号（终端用户)</param>
    /// <param name="kglb">空鼓类别</param>
    /// <param name="kgxh">空鼓型号</param>
    /// <param name="sl">数量</param>
    /// <param name="ywlx">当前业务类型，包括：富美直供订单、出售给富美、旧硒鼓兑换礼品、交易卖出</param>
    /// <param name="ywdh">当前业务的单号</param>
    /// <param name="pass">授权密码</param>
    /// <returns></returns>
   [WebMethod]
    public ArrayList GetFreeOneA(string UserNumber, string kglb, string kgxh,string sl, string ywlx, string ywdh,string pass)
    {          
            ArrayList al = new ArrayList();
            if (String.IsNullOrEmpty(UserNumber) || String.IsNullOrEmpty(kglb) || String.IsNullOrEmpty(kgxh) || String.IsNullOrEmpty(ywdh) || String.IsNullOrEmpty(pass) || String.IsNullOrEmpty(sl))
            {
                return al;
            }
            if (pass == "fmFun")
            {
                try
                {
                    KGclassYH ky = new KGclassYH();
                    al = ky.GetFreeOneA(UserNumber, kglb, kgxh,Convert.ToDouble(sl), ywlx, ywdh);
                    return al;
                }
                catch (Exception ex)
                {
                    return al;
                }

            }
            else
            {
                return al;
            }
      
    }
   [WebMethod]
   public string[] GetFreeOneB(string UserNumber, string kglb, string kgxh, string sl, string ywlx, string ywdh, string pass)
   {
       ArrayList al = new ArrayList();
       if (String.IsNullOrEmpty(UserNumber) || String.IsNullOrEmpty(kglb) || String.IsNullOrEmpty(kgxh) || String.IsNullOrEmpty(ywdh) || String.IsNullOrEmpty(pass) || String.IsNullOrEmpty(sl))
       {
           return null;
       }     
       if (pass == "fmFun")
       {        
           try
           {
               KGclassYH ky = new KGclassYH();
               al = ky.GetFreeOneA(UserNumber, kglb, kgxh, Convert.ToDouble(sl), ywlx, ywdh);
               return (string[])al.ToArray(typeof(string));
           }
           catch (Exception ex)
           {
               return null;
           }
       }
       else
       {
           return null;
       }

   }
    /// <summary>
    /// 测试方法
    /// </summary>
    /// <param name="tstr">传入字符串</param>
    /// <returns></returns>
    [WebMethod]
    public int Test()
    {
        return 10;
    }


    /// <summary>
    /// 交易成功返还授信旧硒鼓 wyh 2013.2.18 add
    /// </summary>
    /// <param name="fwsbh">服务商编号</param>
    /// <param name="kglb">空鼓类别</param>
    /// <param name="kgxh">空鼓型号</param>
    /// <param name="kgsl">空鼓数量</param>
    /// <param name="ywlx">业务类型：交易买入、空鼓调整</param>
    /// <param name="ywdh">业务单号</param>
    /// <returns>需要执行的sql语句数组</returns>
    [WebMethod]
    public string [] GetFHSXsqlweb(string fwsbh, string kglb, string kgxh, int kgsl,string ywlx,string ywdh)
    {
        ArrayList al = new ArrayList();
        KGclass kg = new KGclass();
        al = kg.GetFHSXsql( fwsbh,  kglb,  kgxh,  kgsl, ywlx, ywdh);
        return (string[])al.ToArray(typeof(string));
    }



  

    
}
