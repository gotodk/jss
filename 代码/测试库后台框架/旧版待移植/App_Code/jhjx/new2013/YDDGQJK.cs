using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using FMOP.DB;
using Key;
using Hesion.Brick.Core;
using System.Data.SqlClient;
using Hesion.Brick.Core.WorkFlow;
using Galaxy.ClassLib.DataBaseFactory;
using System.Web.Services;
using System.Threading;
using System.Collections.Generic;

/// <summary>
/// 预订单合同期满监控 2013-06-27 added guotuo
/// </summary>
public class YDDGQJK
{
	public YDDGQJK()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}

    /// <summary>
    /// 检测是否有超期的预订单，有则返回撤销应执行的SQL，没有返回null
    /// </summary>
    /// <returns></returns>
    public Hashtable RunGQ()
    {
        Hashtable ht = new Hashtable();
        ArrayList al = new ArrayList();//要执行的SQL
        List<List<Hashtable>> list = new List<List<Hashtable>>();//要发送的提醒
        /*
         思路描述
         1、本监控每天凌晨执行，如果一张预定单设定到1月23号，那么1月23号凌晨运行监控时这张订单不会被撤销。
         2、1月24号凌晨时运行监控，这张单子就作废了。
         3、所以获取服务器时间为现在，在数据库中查询出小于这个时间的所有预订单
         4、遍历，跑撤销流程。
         */
        string DateOnServer = DateTime.Now.ToString("yyyy-MM-dd");//服务器当前时间
        string sql = "SELECT *,AAA_DLZHXXB.B_YHM AS YHM FROM AAA_YDDXXB LEFT JOIN AAA_DLZHXXB ON AAA_YDDXXB.DLYX=AAA_DLZHXXB.B_DLYX WHERE YXJZRQ< '" + DateOnServer + "' AND ZT='竞标'";
        DataTable dt = DbHelperSQL.Query(sql).Tables[0];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            
            string YZBSL = dt.Rows[i]["YZBSL"].ToString().Trim();//已中标数量
            
            if (YZBSL == "0")
            {
                #region 更改预订单信息
                string sql_NGZB = "UPDATE AAA_YDDXXB SET ZT='撤销' ,NDGJE=((NDGSL-YZBSL)*NMRJG) WHERE Number='" + dt.Rows[i]["Number"].ToString() + "'";
                #endregion
                al.Add(sql_NGZB);

                #region 生成流水信息
                string sql_LSDZ = "SELECT * FROM AAA_moneyDZB WHERE Number='1304000019'";//资金对照表中的撤销预订单

                DataTable dt_LSGZ = DbHelperSQL.Query(sql_LSDZ).Tables[0];
                string sql_LS = "INSERT INTO AAA_ZKLSMXB(Number,DLYX,JSZHLX,JSBH,LYYWLX,LYDH,LSCSSJ,YSLX,JE,XM,XZ,ZY,JKBH,QTBZ,SJLX) VALUES('" + PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "") + "','" + dt.Rows[i]["DLYX"].ToString() + "','" + dt.Rows[i]["JSZHLX"].ToString() + "','" + dt.Rows[i]["MJJSBH"].ToString() + "','" + "AAA_YDDXXB" + "','" + dt.Rows[i]["Number"].ToString() + "','" + DateTime.Now.ToString() + "','" + dt_LSGZ.Rows[0]["YSLX"].ToString() + "','" + dt.Rows[i]["DJDJ"].ToString() + "','" + dt_LSGZ.Rows[0]["XM"].ToString() + "','" + dt_LSGZ.Rows[0]["XZ"].ToString() + "','" + dt_LSGZ.Rows[0]["ZY"].ToString().Replace("[x1]", dt.Rows[i]["Number"].ToString()) + "','" + "接口编号" + "','" + dt.Rows[i]["Number"].ToString() + "预订单撤销，原拟购量：" + dt.Rows[i]["NDGSL"].ToString() + "','" + dt_LSGZ.Rows[0]["SJLX"].ToString() + "')";//插入流水的表
                #endregion
                al.Add(sql_LS);

                #region 更新用户余额
                string sql_YE = "UPDATE AAA_DLZHXXB SET B_ZHDQKYYE=B_ZHDQKYYE+" + dt.Rows[i]["DJDJ"].ToString().Trim() + " WHERE B_DLYX='" + dt.Rows[i]["DLYX"].ToString() + "'";
                #endregion
                al.Add(sql_YE);
                string RQ = Convert.ToDateTime(dt.Rows[i]["YXJZRQ"]).ToString("yyyy年MM月dd日");
                //您**商品的**预订单有效期截止**年**月**日，在有效期内该预订单未能中标，现已自动撤销。

                //string content = "您" + dt.Rows[i]["SPMC"].ToString() + "商品的" + dt.Rows[i]["Number"].ToString() + "预订单有效期截至" + RQ + "，在有效期内该预订单未能中标，现已自动撤销。"; wyh dele 
                string content = "您" + dt.Rows[i]["Number"].ToString() + "编号的《预订单》已于"+RQ+"自动全部撤销，相应订金已解冻。";//wyh add 
                list.Add(GetTXListArray(dt.Rows[i]["DLYX"].ToString(), dt.Rows[i]["YHM"].ToString(), dt.Rows[i]["JSZHLX"].ToString(), dt.Rows[i]["MJJSBH"].ToString(), content));
            }
            else
            {



                string sql_NGZB = "UPDATE AAA_YDDXXB SET NDGSL=YZBSL,ZT='中标' WHERE Number='" + dt.Rows[i]["Number"].ToString() + "'";//把拟订购数量更新为与中标数量相同的量，把状态改为中标
               

                string sql_LSDZ = "SELECT * FROM AAA_moneyDZB WHERE Number='1304000019'";//资金对照表中的撤销预订单

                DataTable dt_LSGZ = DbHelperSQL.Query(sql_LSDZ).Tables[0];
                string sql_LS = "INSERT INTO AAA_ZKLSMXB(Number,DLYX,JSZHLX,JSBH,LYYWLX,LYDH,LSCSSJ,YSLX,JE,XM,XZ,ZY,JKBH,QTBZ,SJLX) VALUES('" + PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "") + "','" + dt.Rows[i]["DLYX"].ToString() + "','" + dt.Rows[i]["JSZHLX"].ToString() + "','" + dt.Rows[i]["MJJSBH"].ToString() + "','" + "AAA_YDDXXB" + "','" + dt.Rows[i]["Number"].ToString() + "','" + DateTime.Now.ToString() + "','" + dt_LSGZ.Rows[0]["YSLX"].ToString() + "','" + dt.Rows[i]["DJDJ"].ToString() + "','" + dt_LSGZ.Rows[0]["XM"].ToString() + "','" + dt_LSGZ.Rows[0]["XZ"].ToString() + "','" + dt_LSGZ.Rows[0]["ZY"].ToString().Replace("[x1]", dt.Rows[i]["Number"].ToString()) + "','" + "接口编号" + "','" + dt.Rows[i]["Number"].ToString() + "预订单撤销（全单）" + "','" + dt_LSGZ.Rows[0]["SJLX"].ToString() + "')";//插入流水的表

                string sql_YE = "UPDATE AAA_DLZHXXB SET B_ZHDQKYYE=B_ZHDQKYYE+" + dt.Rows[i]["DJDJ"].ToString().Trim() + " WHERE B_DLYX='" + dt.Rows[i]["DLYX"].ToString() + "'";
                al.Add(sql_NGZB);
                al.Add(sql_LS);
                al.Add(sql_YE);
                string RQ = Convert.ToDateTime(dt.Rows[i]["YXJZRQ"]).ToString("yyyy年MM月dd日");
                //string content = "您" + dt.Rows[i]["SPMC"].ToString() + "商品的" + dt.Rows[i]["Number"].ToString() + "预订单有效期截至" + RQ + "，在有效期内该预订单已中标" + dt.Rows[i]["YZBSL"].ToString() + dt.Rows[i]["JJDW"].ToString() + "，剩余未中标部分现在已自动撤销。"; wyh dele 
                string content = "您" + dt.Rows[i]["Number"].ToString() + "编号的部分《预订单》已于" + RQ + "自动撤销，相应订金已解冻。"; //wyh add
                //您**商品的**预订单有效期截止**年**月**日，在有效期内该预订单已中标**，剩余未中标部分现已自动撤销。

                list.Add(GetTXListArray(dt.Rows[i]["DLYX"].ToString(), dt.Rows[i]["YHM"].ToString(), dt.Rows[i]["JSZHLX"].ToString(), dt.Rows[i]["MJJSBH"].ToString(), content));
            }
        }
        ht["SQL"] = al;
        ht["TX"] = list;
        return ht;
    }

    /// <summary>
    /// 生成提醒ListHashtable
    /// </summary>
    /// <param name="DLYX">登录邮箱</param>
    /// <param name="YHM">用户名</param>
    /// <param name="JSZHLX">角色类型</param>
    /// <param name="JSBH">角色编号</param>
    /// <param name="txtContent">提醒内容</param>
    /// <returns>ListHashtable</returns>
    private List<Hashtable> GetTXListArray(string DLYX,string YHM,string JSZHLX,string JSBH,string txtContent)
    {
        List<Hashtable> listTX = new List<Hashtable>();
        Hashtable htJYPT = new Hashtable();
        htJYPT["type"] = "集合集合经销平台";
        htJYPT["提醒对象登陆邮箱"] = DLYX;
        htJYPT["提醒对象用户名"] = YHM;
        htJYPT["提醒对象结算账户类型"] = JSZHLX;
        htJYPT["提醒对象角色编号"] = JSBH;
        htJYPT["提醒对象角色类型"] = "买家";
        htJYPT["提醒内容文本"] = txtContent;
        htJYPT["创建人"] = "Admin";
        listTX.Add(htJYPT);
        return listTX;
    }

    
}