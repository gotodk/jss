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

/// <summary>
///jhjx_PublicClass 的摘要说明
/// </summary>
public static class jhjx_PublicClass
{
    /// <summary>
    /// 通过模块名和开头标记字母，获得下一个编号
    /// </summary>
    /// <param name="ModuleName"></param>
    /// <param name="HeadStr"></param>
    /// <returns></returns>
    public static string GetNextNumberZZ(string ModuleName, string HeadStr)
    {
        WorkFlowModule WFM = new WorkFlowModule(ModuleName);
        return WFM.numberFormat.GetNextNumberZZ(HeadStr);
    }

    /// <summary>
    /// 通过角色编号获取账户各种频繁使用的相关信息
    /// </summary>
    /// <param name="bh">角色编号</param>
    /// <returns>带有信息的哈希表</returns>
    public static Hashtable GetUserInfo(string bh)
    {
        //旧版，没用了
        return null;
    }


    /// <summary>
    /// 通过卖家或买家角色编号，获取卖家或买家当前所默认关联的经纪人是否可让卖家或买家进行预订单和投标信息的业务
    /// </summary>
    /// <param name="bh"></param>
    /// <returns></returns>
    public static string IsValidDLR(string bh)
    {
        //返回否的条件： 1. 默认经纪人不能被冻结  2. 默认经纪人不能设置了暂停代理新业务

        Hashtable HTUserInfo = GetUserInfo(bh);
        if (HTUserInfo["是否冻结"].ToString() == "是")
        {
            return "默认经纪人被冻结";
        }

        DataSet ds_p3 = DbHelperSQL.Query("select top 1 * from ZZ_MMJYDLRZHGLB where JSBH = '" + bh + "' and SFMRDLR = '是' and DLRBH='" + HTUserInfo["当前默认关联经纪人角色编号"].ToString() + "' order by CreateTime DESC");
        if (ds_p3 == null || ds_p3.Tables[0].Rows.Count < 1)
        {
            return "找不到默认经纪人";
        }
        else
        {
            if (ds_p3.Tables[0].Rows[0]["SFZTXYW"].ToString() == "是")
            {
                return "默认经纪人已暂停新业务";
            }
        }
        return "ok";
    }

    /// <summary>
    /// 通过商品编号获取商品各种频繁使用的相关信息
    /// </summary>
    /// <param name="bh">商品编号</param>
    /// <returns>带有信息的哈希表</returns>
    public static Hashtable GetSPInfo(string bh)
    {
        Hashtable HTSPInfo = new Hashtable();
        HTSPInfo["商品名称"] = "";
        HTSPInfo["规格型号"] = "";
        HTSPInfo["所属分类编号"] = "";
        HTSPInfo["计价单位"] = "";
        HTSPInfo["最小经济批量"] = "";
        HTSPInfo["是否有效"] = "";
        HTSPInfo["发布日期"] = "";
        HTSPInfo["作废日期"] = "";

        DataSet ds_p = DbHelperSQL.Query("select top 1 * from ZZ_PTSPXXB where SPBH = '" + bh + "' order by CreateTime DESC");
        if (ds_p != null && ds_p.Tables[0].Rows.Count > 0)
        {
            HTSPInfo["商品名称"] = ds_p.Tables[0].Rows[0]["SPMC"].ToString();
            HTSPInfo["规格型号"] = ds_p.Tables[0].Rows[0]["GGXH"].ToString();
            HTSPInfo["所属分类编号"] = ds_p.Tables[0].Rows[0]["SSFLBH"].ToString();
            HTSPInfo["计价单位"] = ds_p.Tables[0].Rows[0]["JJDW"].ToString();
            HTSPInfo["最小经济批量"] = ds_p.Tables[0].Rows[0]["ZDJJPL"].ToString();
            HTSPInfo["是否有效"] = ds_p.Tables[0].Rows[0]["SFYX"].ToString();
            HTSPInfo["发布日期"] = ds_p.Tables[0].Rows[0]["FBRQ"].ToString();
            HTSPInfo["作废日期"] = ds_p.Tables[0].Rows[0]["ZFRQ"].ToString();
      
        }
        return HTSPInfo;
    }


    /// <summary>
    /// 通过最新的配置动态参数信息
    /// </summary>
    /// <returns>带有信息的哈希表</returns>
    public static Hashtable GetParameterInfo()
    {
        //旧版，没用了
        return null;
    }

    /// <summary>
    /// 确认当前是否在营业时间内
    /// </summary>
    /// <returns></returns>
    public static bool IsOpenTime()
    {
        Hashtable HTParameterInfo = GetParameterInfo();
        string Strtime1begin = HTParameterInfo["开业时间"].ToString();
        string Strtime1end = HTParameterInfo["结业时间"].ToString();
        DateTime time1begin = DateTime.Parse(Strtime1begin);
        DateTime time1end = DateTime.Parse(Strtime1end);
        if ((DateTime.Now > time1begin) && (DateTime.Now < time1end))
        {
            return true;
        }
        else
        {
            return false;
        } 

        
    }

    /// <summary>
    /// 确认当前是否在营业日期内
    /// </summary>
    /// <returns></returns>
    public static bool IsOpenDay()
    {
        DataSet ds_p = DbHelperSQL.Query("select top 1 * from ZZ_PTJRSDB where SFYX='是' and DATEDIFF(day,RQ,GETDATE())=0   order by CreateTime DESC");
        if (ds_p != null && ds_p.Tables[0].Rows.Count > 0)
        {
            return false;
        }
        else
        {
            return true;
        }

        
    }

    /// <summary>
    /// 确认指定标书号的常用信息
    /// </summary>
    /// <param name="bsh"></param>
    /// <returns></returns>
    public static Hashtable GetBSInfo(string bsh)
    {
        Hashtable HTBSInfo = new Hashtable();
        HTBSInfo["是否正在竞标中"] = "";
        HTBSInfo["标书状态"] = "";
        HTBSInfo["标书类型"] = "";
 


        DataSet ds_p = DbHelperSQL.Query("select top 1 * from ZZ_BSXXB where BSH='" + bsh + "'   order by CreateTime DESC");
        if (ds_p != null && ds_p.Tables[0].Rows.Count > 0)
        {
            if (ds_p.Tables[0].Rows[0]["BSZT"].ToString() == "竞标")
            {
                HTBSInfo["是否正在竞标中"] = "是";
            }

            HTBSInfo["标书状态"] = ds_p.Tables[0].Rows[0]["BSZT"].ToString();
            HTBSInfo["标书类型"] = ds_p.Tables[0].Rows[0]["TBLX"].ToString();
        }


        return HTBSInfo;
    }

    /// <summary>
    /// 判断用户是否要自动休眠（登录时用到）
    /// </summary>
    /// <param name="user_mail">用户邮箱</param>
    /// <returns>T/F</returns>
    public static bool IsSleepingID(string user_mail)
    {
        object zhycdlsj = DbHelperSQL.GetSingle("SELECT ZHYCDLSJ FROM ZZ_UserLogin WHERE DLYX='" + user_mail.Trim() + "'");
        if (zhycdlsj!=null&& zhycdlsj != DBNull.Value&& zhycdlsj.ToString().Trim() != "" && zhycdlsj.ToString() != string.Empty)
        {
            DateTime lastTime = DateTime.Parse(Convert.ToDateTime(DbHelperSQL.GetSingle("SELECT ZHYCDLSJ FROM ZZ_UserLogin WHERE DLYX='" + user_mail.Trim() + "'")).ToString());
            double days = (DateTime.Now - lastTime).TotalDays;
            bool b = days > 365.0d ? true : false;
            if (b)
            {
                string SqlsleepID = "UPDATE ZZ_UserLogin SET SFXM='是',SFJCXM='否' WHERE DLYX='" + user_mail.Trim() + "'";
                try
                {
                    DbHelperSQL.ExecuteSql(SqlsleepID);
                }
                catch
                {
                    ;
                }
                finally
                {
                    ;
                }
            }
            return b;
        }
        else
            return false;
    }

    /// <summary>
    /// 用户角色信息表，根据登录邮箱，判断当前账户是否已开通结算账户
    /// </summary>
    /// <param name="user_mail">用户邮箱</param>
    /// <returns></returns>
    public static DataSet GetSelSHZT(string user_mail)
    {
        string strSQL = "select top 1 '系统繁忙'='否', * from ZZ_YHJSXXB where DLYX='" + user_mail.Trim() + "'";
        DataSet ds_SelSHZT = DbHelperSQL.Query(strSQL);        
        return ds_SelSHZT;
    }
    /// <summary>
    /// 买卖家与经纪人账户关联表，根据登陆账号，判断卖家结算账户默认的经纪人是否暂停新业务
    /// </summary>
    /// <param name="user_mail">用户邮箱</param>
    /// <returns></returns>
    public static string GetIsZTXYW(string user_mail)
    {
        string strSQL = "select SFZTXYW from ZZ_MMJYDLRZHGLB where JSDLZH='" + user_mail + "' and SFMRDLR='是' and DLRSHZT='已审核' and GLZT='有效'";
        object ztxyw = DbHelperSQL.GetSingle(strSQL);
        if (ztxyw != null)
        {
            return ztxyw.ToString();
        }
        else
        {
            return "没有默认的经纪人";
        }        
    }
    /// <summary>
    /// 根据登陆账号，得到当前可用余额
    /// </summary>
    /// <param name="user_mail">用户邮箱</param>
    /// <returns></returns>
    public static string GetZHDQKYYE(string user_mail)
    {
        string strSQL = "select ZHDQKYYE from ZZ_UserLogin where DLYX='" + user_mail + "'";
        object kyye = DbHelperSQL.GetSingle(strSQL);
        if (kyye != null)
        {
            return Convert.ToString(kyye);
        }
        else
        {
            return "无数据";
        }

    }
    /// <summary>
    /// 获取所有一级商品分类
    /// </summary>
    /// <returns></returns>
    public static DataSet GetSatrtSPFL()
    {
        string strSQL = "select '状态'='',* from dbo.ZZ_tbMenuSPFL where SortParentID='0'";
        DataSet ds = DbHelperSQL.Query(strSQL);
        return ds;
    }
    /// <summary>
    /// 根据选中的商品名称，获取子类商品
    /// </summary>
    /// <param name="sortName">商品名称</param>
    /// <returns></returns>
    public static DataSet GetChildSPFL(string sortName)
    {
        string strSQL = "select '状态'='',* from ZZ_tbMenuSPFL where SortParentID =( select SortID from ZZ_tbMenuSPFL where SortName='" + sortName.ToString().Trim() + "')";
        DataSet ds = DbHelperSQL.Query(strSQL);
        return ds;
    }
    /// <summary>
    /// 根据商品分类的名称，得到此商品分类下的SortID
    /// </summary>
    /// <param name="StartSPFL">一级商品名称</param>
    /// <param name="SecondSPFL">二级商品名称</param>
    /// <param name="ThreeSPFL">三级商品名称</param>
    /// <returns></returns>
    public static string GetSPXXSortID(string StartSPFL, string SecondSPFL, string ThreeSPFL)
    {
        string strSQL = "select SortID from ZZ_tbMenuSPFL where SortParentID in( select SortID from ZZ_tbMenuSPFL where SortParentID in( select SortID from ZZ_tbMenuSPFL where SortParentID='0' and SortName like '%" + StartSPFL.Trim() + "%' ) and SortName like '%" + SecondSPFL.Trim() + "%' ) and SortName like '%" + ThreeSPFL.Trim() + "%' ";
        DataSet ds = DbHelperSQL.Query(strSQL);
        string sortID = "";
        if (ds != null && ds.Tables[0].Rows.Count>0)
        {
            foreach(DataRow row in ds.Tables[0].Rows)
            {
                sortID +="'"+ row["SortID"].ToString().Trim() + "',";
            }
            sortID += "''";
        }
        return sortID;
    }
    /// <summary>
    /// 根据登录帐号，获取当前买家的中标定标信息表中的定标执行中的数据
    /// </summary>
    /// <param name="user_mail">登录邮箱</param>
    /// <returns></returns>
    public static DataSet GetYDD(string user_mail)
    {
        string strSQL = "select b.Number 订单号,a.BSH 标书号,a.TBLX 投标类型,a.SPMC 商品名称,a.GGXH 型号规格,a.JJDW 计价单位,a.BUYYDZSL 订单数量,a.BUYYTHSL 已提货数量,isnull(a.BUYYDZSL-a.BUYYTHSL,0) 未提货数量,DBSJ 定标状态,SELYHM 卖家名称,卖家联系方式=(select SJH from ZZ_SELJBZLXXB where DLYX=a.SELDLYX) from ZZ_ZBDBXXB as a,ZZ_YDDXXB as b where a.BSH=b.BSH and a.BUYDLYX=b.MJDLYX and a.ZBDBZT='定标执行中' and a.BUYDLYX='" + user_mail.Trim() + "'";
        DataSet ds = DbHelperSQL.Query(strSQL);
        return ds;
    }
    /// <summary>
    /// 根据买家登录邮箱,标书号,订单号，查询提货单的中标定标状态、定金剩余百分比、履约保证金剩余百分比
    /// </summary>
    /// <param name="user_mail">登录邮箱</param>
    /// <param name="bbh">标书号</param>
    /// <param name="ddh">订单号</param>
    /// <returns></returns>
    public static DataSet GetZBDBZT(string user_mail, string bbh, string ddh)
    {
        string strSQL = "select b.Number 订单号,a.BSH 标书号,a.ZBDBZT 定标中标状态,a.BUYDJSYKYJE 买家定金剩余可用金额,a.LYBZJSYKYJE 履约保证金剩余可用金额,(a.BUYDJSYKYJE/BUYYDDYSDJJE) 定金剩余百分比,(a.LYBZJSYKYJE/DBSYSLYBZJJE) 履约保证金剩余百分比 from ZZ_ZBDBXXB as a,ZZ_YDDXXB as b where a.BSH=b.BSH and a.BUYDLYX=b.MJDLYX and a.BUYDLYX='买家登录邮箱' and a.bsh='标书号' and b.Number='订单号'";
        DataSet ds = DbHelperSQL.Query(strSQL);
        return ds;
    }
    /// <summary>
    /// 根据登陆账号查询收获地址
    /// </summary>
    /// <param name="user_mail"></param>
    /// <returns></returns>
    public static DataSet GetSHDD(string user_mail)
    {
        string strSQL = "select * from ZZ_SHDZXXB where JSDLZH='" + user_mail + "'";
        DataSet ds = DbHelperSQL.Query(strSQL);
        return ds;
    }
    /// <summary>
    /// 根据用户登录邮箱，获取其有效开票信息
    /// </summary>
    /// <param name="user_mail">用户邮箱</param>
    /// <param name="jszh">结算账户类型</param>
    /// <returns></returns>
    public static DataSet GetYXKPXX(string user_mail,string jszh)
    {
        string tb="";
        switch(jszh.Trim())
        {
            case "买家":
                tb="ZZ_BUYJBZLXXB";
                break;
            case "卖家":
                tb="ZZ_SELJBZLXXB";
                break;
            case "经纪人":
                tb="ZZ_DLRJBZLXXB";
                break;
            default:
                tb="ZZ_BUYJBZLXXB";
                break;
        }
        string strSQL = "select * from " + tb.Trim() + " where DLYX='" + user_mail + "'";
        DataSet ds = DbHelperSQL.Query(strSQL);
        return ds;
        
    }

    /// <summary>
    /// 投标信息表,根据登陆账号查询当前账号是否发布过此同期限同类商品的投标(只查询竞标状态的)
    /// 查询结果：如果没有数据，则可以发布，否则不能
    /// </summary>
    /// <param name="user_mail">登陆账号</param>
    /// <param name="htqx">合同期限</param>
    /// <param name="DSspbh">商品编号</param>
    /// <returns></returns>
    public static bool GetIsFBTB(string user_mail,string htqx, DataSet DSspbh)
    {
        string spbh = "";
        foreach(DataRow row in DSspbh.Tables[0].Rows)
        {
            spbh += "'"+row["商品编号"].ToString().Trim()+"'";
        }
        string strSQL = "select count(*) from dbo.ZZ_TBXXB as Ptab,ZZ_TBXXBZB as Ctab where Ptab.SELDLYX='" + user_mail.Trim() + "' and Ptab.HTQX='' and Ctab.SPBH in ('" + spbh.Trim() + "') and Ctab.ZT='竞标'";
        object count = DbHelperSQL.GetSingle(strSQL);
        if (Convert.ToInt32(count.ToString()) > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// 投标信息表,查询同期限同类商品的投标是否进入冷静期(只查询竞标状态的)
    /// 参数:合同期限、商品编号
    /// 查询结果：如果没有数据，则可以发布；如果有数据，则判断是否已在冷静期中，如果在则不能发布
    /// </summary>
    /// <param name="htqx">合同期限</param>
    /// <param name="DSspbh">商品编号</param>
    /// <returns></returns>
    public static bool GetIsInLJQ(string htqx, DataSet DSspbh)
    {
        string strSQL = "";
        if (DSspbh.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow row in DSspbh.Tables[0].Rows)
            {
                strSQL += "select top 1 * from dbo.ZZ_TBXXB as Ptab,ZZ_TBXXBZB as Ctab where Ptab.HTQX='" + htqx.Trim() + "' and Ctab.SPBH='" + row["商品编号"].ToString().Trim() + "' and Ctab.ZT='竞标' union all";
            }
        }
        strSQL.Substring(0, strSQL.LastIndexOf("union all"));
        DataSet ds = DbHelperSQL.Query(strSQL);
        int IsInLJQ = 0;
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            foreach(DataRow row in ds.Tables[0].Rows)
            {
                if (row["是否在冷静期"].ToString() == "是")
                {
                    IsInLJQ +=1;
                }               
            }
            if (IsInLJQ > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 预订单信息表,根据登陆账号查询当前账号是否发布过此同期限同类商品的投标预订单(只查询竞标状态的)    /// 
    /// 查询结果：如果没有数据，则可以发布，否则不能
    /// </summary>
    /// <param name="user_mail">登陆账号</param>
    /// <param name="htqx">合同期限</param>
    /// <param name="DSspbh">必须有"商品编号"的字段</param>
    /// <returns></returns>
    public static bool GetIsXDYDD(string user_mail, string htqx, DataSet DSspbh)
    {
        string spbh = "";
        foreach (DataRow row in DSspbh.Tables[0].Rows)
        {
            spbh += "'" + row["商品编号"].ToString().Trim() + "'";
        }
        string strSQL = "select count(*) from ZZ_YDDXXB where ZZ_YDDXXB.MJDLYX='" + user_mail.Trim() + "' and ZZ_YDDXXB.HTQX='' and ZZ_YDDXXB.SPBH in ('" + spbh.Trim() + "') and ZZ_YDDXXB.ZT='竞标'";
        object count = DbHelperSQL.GetSingle(strSQL);
        if (Convert.ToInt32(count.ToString()) > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// 预订单信息表,根据预订单编号，预订单状态，查询预订单信息
    /// </summary>
    /// <param name="Number">预订单号</param>
    /// <param name="ZT">竞标,中标,定标,废标，撤销</param>
    /// <returns></returns>
    public static DataSet GetYDDXX(string Number,string ZT)
    {
        string strSQL = "select * from ZZ_YDDXXB where Number='" + Number.Trim() + "' and ZT='" + ZT.Trim() + "'";
        DataSet ds = DbHelperSQL.Query(strSQL);
        return ds;
    }
    /// <summary>
    /// 投标信息表，根据投标信息号，标书状态，查询投标信息
    /// </summary>
    /// <param name="Number">投标信息号</param>
    /// <param name="ZT">竞标,中标,定标,废标，撤销</param>
    /// <returns></returns>
    public static DataSet GetTBXX(string Number, string ZT)
    {
        string strSQL = "select * from ZZ_TBXXB as Ptab,ZZ_TBXXBZB as Ctab where Ptab.Number=Ctab.ParentNumber and Ptab.Number='" + Number.Trim() + "' and Ptab.SFCX='否' and Ctab.ZT='" + ZT.Trim() + "'";
        DataSet ds = DbHelperSQL.Query(strSQL);
        return ds;
    }


    /// <summary>
    /// 把要执行的语句集合，转化成数据集，便于服务器执行
    /// </summary>
    /// <param name="alsql"></param>
    /// <returns></returns>
    public static DataSet ArrayListToDataSet(ArrayList alsql)
    {
        if (alsql == null || alsql.Count < 1)
        {
            return null;
        }
        DataSet DS = new DataSet();
        DataTable DTsql = new DataTable();
        DTsql.Columns.Add("SQLSTR");
        for (int p = 0; p < alsql.Count; p++)
        {
            DTsql.Rows.Add(new string[] { alsql[p].ToString() });
        }
        DS.Tables.Add(DTsql);
        return DS;

    }

    /// <summary>
    /// 发送一个提醒
    /// </summary>
    /// <param name="sendtype">提醒的发送方式：弹窗|短信|邮件</param>
    /// <param name="DLYX">接收提醒的登陆账号</param>
    /// <param name="msg">提醒内容</param>
    public static void BeginSendMessage(string sendtype, string DLYX, string msg)
    {
        if (sendtype.IndexOf("弹窗") >= 0)
        {
            string Number = jhjx_PublicClass.GetNextNumberZZ("ZZ_PTTXXXB", "");
            string CreateTime = DateTime.Now.ToString();
            DbHelperSQL.ExecuteSql("INSERT INTO ZZ_PTTXXXB  ([Number],[TXDXDLYX],[TXLX],[TXSJ],[TXNRWB],[SFXSG],[XSSJ],[CreateTime]) VALUES ('" + Number + "','" + DLYX + "','普通','" + CreateTime + "','" + msg + "','否',null,'" + CreateTime + "')");
        }
        if (sendtype.IndexOf("短信") >= 0)
        {
            ;
        }
        if (sendtype.IndexOf("邮件") >= 0)
        {
            ;
        }
    }
    
}