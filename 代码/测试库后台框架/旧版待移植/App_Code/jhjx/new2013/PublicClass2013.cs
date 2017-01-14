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
/// PublicClass2013 的摘要说明
/// </summary>
public class PublicClass2013
{
	public PublicClass2013()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}


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
    /// 根据登录邮箱获取，角色编号
    /// </summary>
    /// <param name="ModuleName"></param>
    /// <param name="strPrefix"></param>
    /// <param name="strDLYX"></param>
    /// <returns></returns>
    public static string GetNumberPrefix(string strDLYX,string strPrefix)
    {
        return strPrefix + DbHelperSQL.GetSingle("select Number from dbo.AAA_DLZHXXB  where B_DLYX='" + strDLYX + "'");
    }
    /// <summary>
    /// 通过最新的配置动态参数信息
    /// </summary>
    /// <returns>带有信息的哈希表</returns>
    public static Hashtable GetParameterInfo()
    {

        //待返回的设备
        Hashtable HTParameterInfo = new Hashtable();
        //单个数据
        HTParameterInfo["卖家投标保证金比率"] = null;
        HTParameterInfo["卖家投标保证金最小值"] = null;
        HTParameterInfo["卖家履约保证金比率"] = null;
        HTParameterInfo["卖家履约保证金冻结期限"] = null;
        HTParameterInfo["履约保证金不足比率"] = null;
        HTParameterInfo["买家订金比率"] = null;
        HTParameterInfo["保证函比率"] = null;
        HTParameterInfo["签收技术服务费比率"] = null;
        HTParameterInfo["保证函外技术服务费比率"] = null;
        HTParameterInfo["经纪人收益卖家比率"] = null;
        HTParameterInfo["经纪人收益买家比率"] = null;
        HTParameterInfo["休眠期限"] = null;
        HTParameterInfo["平台每日开始服务时间"] = null;
        HTParameterInfo["平台每日结束服务时间"] = null;
        HTParameterInfo["即时合同定标下提货单期限"] = null;
        HTParameterInfo["是否在服务时间内"] = null;
        HTParameterInfo["是否在服务时间内(工作时)"] = null;
        HTParameterInfo["是否在服务时间内(假期)"] = null;
        //获取单个数据，取出最后一条更新的数据
        DataSet ds_p = DbHelperSQL.Query("select top 1 * from AAA_PTDTCSSDB order by CreateTime DESC");
        if (ds_p != null && ds_p.Tables[0].Rows.Count > 0)
        {
            HTParameterInfo["卖家投标保证金比率"] = (decimal)ds_p.Tables[0].Rows[0]["MJTBBZJBL"];
            HTParameterInfo["卖家投标保证金最小值"] = (decimal)ds_p.Tables[0].Rows[0]["MJTBBZJZXZ"];
            HTParameterInfo["卖家履约保证金比率"] = (decimal)ds_p.Tables[0].Rows[0]["MJLYBZJBL"];
            HTParameterInfo["卖家履约保证金冻结期限"] = (Int32)ds_p.Tables[0].Rows[0]["MJLYBZJDJQX"];
            HTParameterInfo["履约保证金不足比率"] = (decimal)ds_p.Tables[0].Rows[0]["LYBZJBZBL"];
            HTParameterInfo["买家订金比率"] = (decimal)ds_p.Tables[0].Rows[0]["MJDJBL"];
            HTParameterInfo["保证函比率"] = (decimal)ds_p.Tables[0].Rows[0]["BZHBL"];
            HTParameterInfo["签收技术服务费比率"] = (decimal)ds_p.Tables[0].Rows[0]["QSJSFWFBL"];
            HTParameterInfo["保证函外技术服务费比率"] = (decimal)ds_p.Tables[0].Rows[0]["BZHWJSFWFBL"];
            HTParameterInfo["经纪人收益卖家比率"] = (decimal)ds_p.Tables[0].Rows[0]["JJRSYMJBLsel"];
            HTParameterInfo["经纪人收益买家比率"] = (decimal)ds_p.Tables[0].Rows[0]["JJRSYMJBLbuy"];
            HTParameterInfo["休眠期限"] = (Int32)ds_p.Tables[0].Rows[0]["XMQX"];
            HTParameterInfo["平台每日开始服务时间"] = ds_p.Tables[0].Rows[0]["PTMRKSFWSJ"].ToString();
            HTParameterInfo["平台每日结束服务时间"] = ds_p.Tables[0].Rows[0]["PTMRJSFWSJ"].ToString();
            HTParameterInfo["即时合同定标下提货单期限"] = ds_p.Tables[0].Rows[0]["JSHTDBXTHDQX"].ToString();

            string Strtime1begin = HTParameterInfo["平台每日开始服务时间"].ToString();
            string Strtime1end = HTParameterInfo["平台每日结束服务时间"].ToString();
            DateTime time1begin = DateTime.Parse(Strtime1begin);
            DateTime time1end = DateTime.Parse(Strtime1end);
            if ((DateTime.Now > time1begin) && (DateTime.Now < time1end))
            {
                HTParameterInfo["是否在服务时间内(工作时)"] = "是";
            }
            else
            {
                HTParameterInfo["是否在服务时间内(工作时)"] = "否";
            }

            DataSet ds_pjq = DbHelperSQL.Query("select top 1 * from AAA_PTJRSDB where SFYX='是' and DATEDIFF(day,RQ,GETDATE())=0   order by CreateTime DESC");
            if (ds_pjq != null && ds_pjq.Tables[0].Rows.Count > 0)
            {
                HTParameterInfo["是否在服务时间内(假期)"] = "否";
            }
            else
            {
                HTParameterInfo["是否在服务时间内(假期)"] = "是";
            }

            if (HTParameterInfo["是否在服务时间内(工作时)"].ToString() == "是" && HTParameterInfo["是否在服务时间内(假期)"].ToString() == "是")
            {
                HTParameterInfo["是否在服务时间内"] = "是";
            }
            else
            {
                HTParameterInfo["是否在服务时间内"] = "否";
            }

        }


        return HTParameterInfo;
    }


    /// <summary>
    /// 通过角色编号或证券资金账号获取账户各种频繁使用的相关信息
    /// </summary>
    /// <param name="bh">角色编号</param>
    /// <returns>带有信息的哈希表(包括了主要数据和完整数据集)</returns>
    public static Hashtable GetUserInfo(string bh)
    {
        //待返回的数据
        Hashtable HTUserInfo = new Hashtable();
        HTUserInfo["登录邮箱"] = "";
        HTUserInfo["结算账户类型"] = "";
        HTUserInfo["买家角色编号"] = "";
        HTUserInfo["卖家角色编号"] = "";
        HTUserInfo["经纪人角色编号"] = "";
        HTUserInfo["是否冻结"] = "";
        HTUserInfo["冻结功能项"] = "";
        HTUserInfo["是否休眠"] = "";
        //***********************
        HTUserInfo["当前默认经纪人编号"] = "";
        HTUserInfo["是否被默认经纪人暂停新业务"] = "否";
        HTUserInfo["当前默认经纪人是否被冻结"] = "否";
        //***********************
        HTUserInfo["交易账户是否开通"] = "否";
        //***********************
        HTUserInfo["完整数据集"] = null;
        //***********************
        HTUserInfo["第三方"] = "";//I_DSFCGZT
        HTUserInfo["经纪人分类"] = "";

        //获取单个数据
        DataSet ds_p1 = DbHelperSQL.Query("select top 1 *,'当前默认经纪人是否被冻结'=isnull((select top 1 SubL.B_SFDJ from AAA_DLZHXXB as SubL where SubL.B_DLYX=G.GLJJRDLZH ),'否') from AAA_DLZHXXB as L left join AAA_MJMJJYZHYJJRZHGLB as G on L.B_DLYX = G.DLYX where (L.J_BUYJSBH='" + bh + "' or L.J_SELJSBH='" + bh + "' or L.J_JJRJSBH = '" + bh + "' or L.I_ZQZJZH='"+bh+"') and ''<>'"+ bh + "' and G.SFDQMRJJR = '是'");
        if (ds_p1 != null && ds_p1.Tables[0].Rows.Count > 0)
        {
            HTUserInfo["登录邮箱"] = ds_p1.Tables[0].Rows[0]["B_DLYX"].ToString();
            HTUserInfo["结算账户类型"] = ds_p1.Tables[0].Rows[0]["B_JSZHLX"].ToString();
            HTUserInfo["买家角色编号"] = ds_p1.Tables[0].Rows[0]["J_BUYJSBH"].ToString();
            HTUserInfo["卖家角色编号"] = ds_p1.Tables[0].Rows[0]["J_SELJSBH"].ToString();
            HTUserInfo["经纪人角色编号"] = ds_p1.Tables[0].Rows[0]["J_JJRJSBH"].ToString();
            HTUserInfo["是否冻结"] = ds_p1.Tables[0].Rows[0]["B_SFDJ"].ToString();
            HTUserInfo["冻结功能项"] = ds_p1.Tables[0].Rows[0]["DJGNX"].ToString();
            HTUserInfo["是否休眠"] = ds_p1.Tables[0].Rows[0]["B_SFXM"].ToString();
            HTUserInfo["经纪人分类"] = ds_p1.Tables[0].Rows[0]["I_JJRFL"].ToString();
            //***********************
            HTUserInfo["当前默认经纪人编号"] = ds_p1.Tables[0].Rows[0]["GLJJRBH"].ToString();
            HTUserInfo["第三方"] = ds_p1.Tables[0].Rows[0]["I_DSFCGZT"].ToString();//I_DSFCGZT
            if (ds_p1.Tables[0].Rows[0]["SFZTYHXYW"].ToString() == "是")
            {
                HTUserInfo["是否被默认经纪人暂停新业务"] = "是";
            }
            else
            {
                HTUserInfo["是否被默认经纪人暂停新业务"] = "否";
            }
 
            if (ds_p1.Tables[0].Rows[0]["当前默认经纪人是否被冻结"].ToString() == "是")
            {
                HTUserInfo["当前默认经纪人是否被冻结"] = "是";
            }
            else
            {
                HTUserInfo["当前默认经纪人是否被冻结"] = "否";
            }

            //***********************
            if (ds_p1.Tables[0].Rows[0]["B_JSZHLX"].ToString() == "经纪人交易账户")
            {
                if (ds_p1.Tables[0].Rows[0]["S_SFYBFGSSHTG"].ToString() == "是")
                {
                    HTUserInfo["交易账户是否开通"] = "是";
                }
                else
                {
                    HTUserInfo["交易账户是否开通"] = "否";
                }
            }
            if (ds_p1.Tables[0].Rows[0]["B_JSZHLX"].ToString() == "买家卖家交易账户")
            {
                if (ds_p1.Tables[0].Rows[0]["S_SFYBFGSSHTG"].ToString() == "是" && ds_p1.Tables[0].Rows[0]["S_SFYBJJRSHTG"].ToString() == "是")
                {
                    HTUserInfo["交易账户是否开通"] = "是";
                }
                else
                {
                    HTUserInfo["交易账户是否开通"] = "否";
                }
            }
            //***********************
            HTUserInfo["完整数据集"] = ds_p1.Tables[0].Copy();
        }

        return HTUserInfo;
    }



    /// <summary>
    /// 可以提前获取的一些数据或参数
    /// </summary>
    /// <returns>返回数据集</returns>
    public static DataSet GetPublisDsData()
    {
        DataSet PublisDsData = new DataSet();

        //获取省市区
        DataSet ds_sheng = DbHelperSQL.Query("select p_number as 省编号,p_namestr as 省名,'0' as 省标记 from AAA_CityList_Promary");
        ds_sheng.Tables[0].TableName = "省";
        DataSet ds_shi = DbHelperSQL.Query("select AAA_CityList_City.c_number as 市编号,AAA_CityList_City.c_namestr as 市名,AAA_CityList_City.p_number as 所属省编号,(select AAA_CityList_Promary.p_namestr from AAA_CityList_Promary where AAA_CityList_Promary.p_number=AAA_CityList_City.p_number) as 所属省名 from AAA_CityList_City");
        ds_shi.Tables[0].TableName = "市";
        DataSet ds_qu = DbHelperSQL.Query("select AAA_CityList_qu.q_number as 区编号,AAA_CityList_qu.q_namestr as 区名,AAA_CityList_qu.c_number as 所属市编号,(select AAA_CityList_City.c_namestr from AAA_CityList_City where AAA_CityList_qu.c_number=AAA_CityList_City.c_number) as 所属市名 from AAA_CityList_qu");
        ds_qu.Tables[0].TableName = "区";
        PublisDsData.Tables.Add(ds_sheng.Tables[0].Copy());
        PublisDsData.Tables.Add(ds_shi.Tables[0].Copy());
        PublisDsData.Tables.Add(ds_qu.Tables[0].Copy());

        DataSet ds_fgs = DbHelperSQL.Query("select * from AAA_CityList_FGS  ");
        ds_fgs.Tables[0].TableName = "分公司对照表";
        PublisDsData.Tables.Add(ds_fgs.Tables[0].Copy());
        //新增加的表
        DataSet ds_FGS = DbHelperSQL.Query("select Number '公司Number', GLBMMC '分公司',GLBMZH '办事处' from AAA_PTGLJGB where GLBMFLMC='分公司' and SFYX='是'");
        ds_FGS.Tables[0].TableName = "分公司表";
        PublisDsData.Tables.Add(ds_FGS.Tables[0].Copy());

        DataSet ds_YXB = DbHelperSQL.Query("select Number '院系Number', GXZH '高校账户',YXMC '院系名称' from AAA_PTYXB ");
        ds_YXB.Tables[0].TableName = "院系表";
        PublisDsData.Tables.Add(ds_YXB.Tables[0].Copy());



        //获取最新的配置动态参数信息
        //获取单个数据，取出最后一条更新的数据
        DataSet ds_p = DbHelperSQL.Query("select top 1 * from AAA_PTDTCSSDB order by CreateTime DESC");
        ds_p.Tables[0].TableName = "动态参数";
        PublisDsData.Tables.Add(ds_p.Tables[0].Copy());


        //返回数据
        return PublisDsData;
    }

    /// <summary>
    /// 提醒发送函数  
    /// </summary>
    /// <param name="list1"> HashTable 集合 ,有几种提醒放几个hsahtable，其中type键值区分提醒类别：集合集合经销平台，业务平台，手机短信，邮箱提醒 </param>
    public static void Sendmes(  List<Hashtable> list1 )
    {
        if (list1 != null && list1.Count > 0)
        {
             ArrayList ltsql = new ArrayList();
            DateTime dt = DateTime.Now;
            string Insert = "";
            string KeyNumber = "";
            foreach (Hashtable ht in list1)
            {
                if (ht["type"].ToString().Equals("集合集合经销平台"))
                {
                    KeyNumber = GetNextNumberZZ("AAA_PTTXXXB", "");
                    //集合经销平台平台提醒  
                    Insert = " insert into  AAA_PTTXXXB ( Number, TXDXDLYX,TXDXYHM,TXDXJSZHLX,TXDXJSBH,TXDXJSLX,TXSJ,TXNRWB,SFXSG,CreateUser ) values ( '" + KeyNumber + "','" + ht["提醒对象登陆邮箱"].ToString() + "','  ','" + ht["提醒对象结算账户类型"].ToString() + "','" + ht["提醒对象角色编号"].ToString() + "','" + ht["提醒对象角色类型"].ToString() + "','" + dt + "','" + ht["提醒内容文本"].ToString() + "','否','" + ht["创建人"].ToString() + "' ) ";
                    ltsql.Add(Insert);
                    
                }
                if (ht["type"].ToString().Equals("业务平台"))
                {
                    //业务操作平台提醒
                    //string sql = "insert into User_Warnings(Context,Module_Url,Grade,FromUser,Touser)";
                    //sql = sql + " values('" + ht["提醒内容文本"].ToString() + "','" + ht["提醒页面路径"].ToString()+ "','1','" + ht["提醒发送人"].ToString() + "','" +ht["提醒接收人"] + "')";

                    //ltsql.Add(sql);
                    string Strurl = "commonpage.aspx";
                    if (ht["查看地址"] != null && ht["查看地址"].ToString().Trim() != "" && ht["查看地址"].ToString().Trim() != "#")
                    {
                        Strurl = ht["查看地址"].ToString().Trim();
                    }
                    DataSet ds_touser = DbHelperSQL.Query("select * from AAA_DYWCZPTDTXPZB where txmk='" + ht["模提醒模块名"].ToString() + "'");
                    if (ds_touser.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds_touser.Tables[0].Rows.Count; i++)
                        {
                            //分为两种情况,如果员工编号为空，则向岗位批量发送提醒。
                            if (ds_touser.Tables[0].Rows[i]["YGBH"].ToString() == "")
                            {
                                string ssql = "select number,employee_name,bm,gwmc from HR_employees where  ygzt not like '%离职%' and gwmc='" + ds_touser.Tables[0].Rows[i]["GW"].ToString() + "' and BM='" + ds_touser.Tables[0].Rows[i]["BM"].ToString() + "'";
                                DataSet ds_touser2 = DbHelperSQL.Query(ssql);
                                if (ds_touser2 != null && ds_touser2.Tables[0].Rows.Count > 0)
                                {
                                    for (int t = 0; t < ds_touser2.Tables[0].Rows.Count; t++)
                                    {
                                        string touser = ds_touser2.Tables[0].Rows[t]["number"].ToString();
                                        WorkFlowModule wf = new WorkFlowModule("AAA_ZBDBXXB");
                                        wf.authentication.InsertWarnings(ht["提醒内容文本"].ToString(), Strurl, "1", "admin", touser);
                                    }
                                }

                            }
                            else
                            {
                                string touser = ds_touser.Tables[0].Rows[i]["ygbh"].ToString();
                                WorkFlowModule wf = new WorkFlowModule("AAA_ZBDBXXB");
                                wf.authentication.InsertWarnings(ht["提醒内容文本"].ToString(), Strurl, "1", "admin", touser);
                            }
                        }
                    }


                }
                if (ht["type"].ToString().Equals("手机短信"))
                {
                    //手机提醒

                }
                if (ht["type"].ToString().Equals("邮箱提醒"))
                {
                    //手机提醒

                }

            }
            if(ltsql != null && ltsql.Count>0)
            {

                DbHelperSQL.ExecSqlTran(ltsql);
            }
        }
    }

    /// <summary>
    /// 判断某个商品是否在冷静期内
    /// </summary>
    /// <param name="spbh">商品编号</param>
    /// <param name="htqx">合同期限（三个月/一年）</param>
    /// <returns>true/false</returns>
    public static bool isLJQ(string spbh,string htqx)
    {
        bool b = true;
        string sql = "SELECT * FROM AAA_LJQDQZTXXB WHERE SPBH='" + spbh.Trim() + "' AND SFJRLJQ='是' AND HTQX='"+htqx.Trim()+"'";
        DataTable dt = DbHelperSQL.Query(sql).Tables[0];
        if (dt.Rows.Count == 0)
            b = false;
        return b;
    }
    /// <summary>
    /// 判断某个商品是否在冷静期内
    /// </summary>
    /// <param name="spbh">商品编号</param>
    /// <param name="htqx">合同期限（三个月/一年）</param>
    /// <param name="DateTimes">冷静期结束时间</param>
    /// <returns>true/false</returns>
    public static bool isLJQ(string spbh, string htqx,ref string DateTimes)
    {
        bool b = true;
        string sql = "SELECT * FROM AAA_LJQDQZTXXB WHERE SPBH='" + spbh.Trim() + "' AND SFJRLJQ='是' AND HTQX='" + htqx.Trim() + "'";
        DataTable dt = DbHelperSQL.Query(sql).Tables[0];
        if (dt.Rows.Count == 0)
            b = false;
        else
            DateTimes = Convert.ToDateTime(dt.Rows[0]["LJQJSSJ"]).ToString("yyyy-MM-dd");
        return b;
    }

    /// <summary>
    /// 判断商品是否有效
    /// </summary>
    /// <param name="spbh">商品编号</param>
    /// <returns></returns>
    public static bool isYX(string spbh)
    {
        bool b = false;
        string sql = "SELECT SFYX FROM AAA_PTSPXXB WHERE SPBH='" + spbh + "'";
        DataTable dt = DbHelperSQL.Query(sql).Tables[0];
        if (dt.Rows.Count > 0)
        {
            b = dt.Rows[0][0].ToString() == "是" ? true : false;
        }
        return b;
    }
}