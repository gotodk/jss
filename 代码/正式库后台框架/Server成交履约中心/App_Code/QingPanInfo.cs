/************************************************************************************
 * 
 * 创建人：wyh
 *
 *创建时间：2014.06.12
 *
 *代码功能：实现人工清盘各项功能
 *
 *参考文档：《业务分析》成交履约中心部分
 * 
 * 
 * 
 * ************************************************************************************/
using FMDBHelperClass;
using FMipcClass;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// QingPanInfo 的摘要说明
/// </summary>
public class QingPanInfo
{
	public QingPanInfo()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}


    /// <summary>
    /// 人工清盘点击列表单条数据清盘信息 -- wyh  2014.06.11 
    /// </summary>
    /// <param name="dt">包含：“dlyx”：登陆邮箱；“num” :AAA_ZBDBXXB,Number值</param>
    /// <returns>返回dataset内含表明为“qpinfo”的Datatable</returns>
    public DataSet GetQBInfo(DataTable dt)
    {
        DataSet ds = null;
        if (dt != null && dt.Rows.Count > 0)
        {

           //------判断是否休眠 已拆分

            string dlyx = dt.Rows[0]["dlyx"].ToString().Trim();
            string num = dt.Rows[0]["num"].ToString().Trim();

            if (dlyx.Equals("") || num.Equals(""))
            {
                return null;
            }

            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
            
            Hashtable input = new Hashtable();
            Hashtable htreturn = new Hashtable();

            input["@dlyx"] = dlyx;
            input["@num"] = num;

            string str = "SELECT  AAA_ZBDBXXB.Number AS 主键 ,Z_QPZT AS 清盘状态,Z_QPKSSJ AS 清盘开始时间,Z_QPJSSJ AS 清盘结束时间, Z_HTBH AS 电子购货合同编号,Z_HTJSRQ AS 合同结束日期,Z_SPMC AS 商品名称,Z_SPBH AS 商品编号,Z_GG AS 规格,Z_ZBSL AS 合同数量,Z_ZBJG AS 单价,Z_ZBJE AS 合同金额,Z_LYBZJJE AS 履约保证金金额,  CAST( SUM( T_DJHKJE/ZBDJ) AS numeric(18,2)) AS 争议数量 ,SUM(T_DJHKJE) AS  争议金额,'人工清盘' AS 清盘类型,CASE WHEN Z_HTZT='未定标废标' THEN '废标' WHEN Z_HTZT='定标合同到期' THEN '《电子购货合同》期满' WHEN Z_HTZT='定标合同终止' THEN '废标' WHEN Z_HTZT='定标执行完成' THEN '合同期内买家中标量全部无异议收货' ELSE '其他' END AS 清盘原因 ,T_YSTBDDLYX AS 卖方邮箱,(select I_JYFMC from AAA_DLZHXXB where B_DLYX=T_YSTBDDLYX) '卖方用户名',(select J_BUYJSBH from AAA_DLZHXXB where B_DLYX=T_YSTBDDLYX)'卖方买家角色编号',Y_YSYDDDLYX AS 买方邮箱,(select I_JYFMC from AAA_DLZHXXB where B_DLYX=Y_YSYDDDLYX) '买方用户名',(select J_BUYJSBH from AAA_DLZHXXB where B_DLYX=Y_YSYDDDLYX)'买方买家角色编号',Q_ZMSCFDLYX AS 证明上传方邮箱,Q_SFYQR AS 是否已确认 ,Q_ZMWJLJ AS 证明文件路径, Q_ZFLYZH AS 转付来源账户, Q_ZFMBZH AS 转付目标账户,Q_ZFJE AS 转付金额,Q_CLYJ AS 处理依据  FROM AAA_THDYFHDXXB JOIN AAA_ZBDBXXB ON AAA_ZBDBXXB.Number=AAA_THDYFHDXXB.ZBDBXXBBH WHERE (T_YSTBDDLYX=@dlyx OR Y_YSYDDDLYX=@dlyx) AND AAA_ZBDBXXB.Number=@num AND Z_QPZT='清盘中' AND F_DQZT NOT IN ('无异议收货','默认无异议收货','有异议收货后无异议收货','补发货物无异议收货','撤销','卖家主动退货') GROUP BY  Z_HTBH,Z_HTJSRQ,Z_SPMC,Z_SPBH,Z_GG,Z_ZBSL,Z_ZBJG,Z_ZBJE,CASE WHEN Z_HTZT='未定标废标' THEN '废标' WHEN Z_HTZT='定标合同到期' THEN '《电子购货合同》期满' WHEN Z_HTZT='定标合同终止' THEN '废标' WHEN Z_HTZT='定标执行完成' THEN '合同期内买家中标量全部无异议收货' ELSE '其他' END,AAA_ZBDBXXB.Number,Z_LYBZJJE,Z_QPZT,T_YSTBDDLYX ,Y_YSYDDDLYX ,Z_QPKSSJ ,Z_QPJSSJ ,Q_ZMSCFDLYX,Q_SFYQR ,Q_ZMWJLJ, Q_ZFLYZH , Q_ZFMBZH ,Q_ZFJE ,Q_CLYJ ";

            htreturn = I_DBL.RunParam_SQL(str, "qpinfo", input);
            if ((bool)(htreturn["return_float"]))
            {
                ds = (DataSet)(htreturn["return_ds"]); ;
            }


        }
        return ds;
    }

    /// <summary>
    ///根据中标定标信息表编号获得投标资料审核表中的相关信息 wyh  2014.06.12
    /// </summary>
    /// <param name="num">中标定标信息表Number</param>
    /// <returns></returns>
    public DataSet GetTBZLSHinfo(string num)
    {
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");

        Hashtable input = new Hashtable();
        Hashtable htreturn = new Hashtable();

        if (num.Equals(""))
        {
            return null;
        }

        input["@Number"] = num;
        DataSet ds = null; 
 
        string strsql = "select ZBDB.T_YSTBDDLYX '原始投标单登录邮箱',TBSH.FWZXSHZT '服务中心审核状态',TBSH.JYGLBSHZT '交易管理部审核状态' from AAA_TBZLSHB as TBSH left join AAA_ZBDBXXB as ZBDB on TBSH.TBDH=ZBDB.T_YSTBDBH where ZBDB.Number=@Number";

        htreturn = I_DBL.RunParam_SQL(strsql,"sh",input);
        if ((bool)(htreturn["return_float"])) //说明执行完成
        {
            //这里就可以用了。
            ds = (DataSet)(htreturn["return_ds"]);
        }
    
        return ds;
    }


    /// <summary>
    /// 获取中标定标信息表上传文件的信息，用于验证 wyh  2014.06.12
    /// </summary>
    /// <param name="num">中标定标信息表Number</param>
    /// <returns></returns>
    public DataSet GetSHWJinfo(string num)
    {
        DataSet ds = null; 
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");

        Hashtable input = new Hashtable();
        Hashtable htreturn = new Hashtable();

        input["@Number"] = num;

        string strsql = "SELECT Q_QPZMSCSJ AS 清盘证明上传时间,Q_ZMSCFDLYX AS 证明上传方登录邮箱,Q_ZMSCFJSZHLX 证明上传方结算账户类型,Q_ZMSCFJSBH AS 证明上传方角色编号,Q_ZMWJLJ AS 证明文件路径,Q_ZFLYZH AS 转付来源账户,Q_ZFMBZH AS 转付目标账户,Q_ZFJE AS 转付金额,Q_SFYQR AS 是否已确认,Q_QRSJ AS 确认时间 FROM AAA_ZBDBXXB WHERE Number=@Number ";

        htreturn = I_DBL.RunParam_SQL(strsql,"docinfo",input);
        if ((bool)(htreturn["return_float"])) //说明执行完成
        {
            //这里就可以用了。
            ds = (DataSet)(htreturn["return_ds"]);
        }
        return ds;
    }


    /// <summary>
    /// 人工清盘时用户上传证明文件写入数据表 wyh 2014.06.12 
    /// </summary>
    /// <param name="dt">参数集合</param>
    /// <returns>返回符合dsreturn结构的Dataset</returns>
    public DataSet RunFileData( DataTable dt)
    {
        DataSet dsreturn = new TBD().initReturnDataSet().Copy();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化", "", "true" });
        //进行处理....
        if (dt != null && dt.Rows.Count > 0)
        {

            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");

            Hashtable input = new Hashtable();
            Hashtable htreturn = new Hashtable();

            DataTable dtemp = null;
            
            object[] re = IPC.Call("用户基本信息", new object[] { dt.Rows[0]["dlyx"].ToString().Trim() });//----------------
            if (re[0].ToString() == "ok")
            {
                dtemp = ((DataSet)(re[1])).Tables[0];           
            }

            if (dtemp == null || dt.Rows.Count <= 0)
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取用户信息出错！";
                return dsreturn;
            }

            string jszhlx = dtemp.Rows[0]["结算账户类型"].ToString().Trim();
            string jsbh = dtemp.Rows[0]["买家角色编号"].ToString().Trim();
            string yhm = dtemp.Rows[0]["交易方名称"].ToString().Trim();

            input["@Q_CLYJ"] = dt.Rows[0]["clyj"].ToString().Trim();
            input["@Q_ZMSCFDLYX"] = dt.Rows[0]["dlyx"].ToString().Trim();
            input["@Q_ZMSCFJSZHLX"] = jszhlx;
            input["@Q_ZMSCFJSBH"] = jsbh;
            input["@Q_ZMWJLJ"] = dt.Rows[0]["wjlj"].ToString().Trim();
            input["@Q_ZFLYZH"] = dt.Rows[0]["lyzh"].ToString().Trim();
            input["@Q_ZFMBZH"] = dt.Rows[0]["mbzh"].ToString().Trim();
            input["@Q_ZFJE"] = dt.Rows[0]["zfje"].ToString().Trim();
            input["@Number"] = dt.Rows[0]["num"].ToString().Trim();
            string str = "UPDATE AAA_ZBDBXXB SET Q_QPZMSCSJ=GETDATE(),Q_CLYJ=@Q_CLYJ, Q_ZMSCFDLYX=@Q_ZMSCFDLYX,Q_ZMSCFJSZHLX =@Q_ZMSCFJSZHLX,Q_ZMSCFJSBH=@Q_ZMSCFJSBH,Q_ZMWJLJ=@Q_ZMWJLJ,Q_ZFLYZH =@Q_ZFLYZH,Q_ZFMBZH =@Q_ZFMBZH,Q_ZFJE=@Q_ZFJE WHERE Number=@Number ";

            htreturn = I_DBL.RunParam_SQL(str,input);
            int i = 0;
            if ((bool)(htreturn["return_float"])) //说明执行完成
            {
                //这里就可以用了。
                i = (int)(htreturn["return_other"]);

            }

            if (i > 0)
            {

                //提醒
                string dfyx = dt.Rows[0]["dfyx"].ToString().Trim();
                string dfjslx = dt.Rows[0]["dfjslx"].ToString().Trim();

                DataTable dtmess = new TBD().dtmeww().Clone();
                DataRow ht = dtmess.NewRow();
                ht["提醒对象登陆邮箱"] = dfyx;
                ht["提醒对象结算账户类型"] = jszhlx;//df.Rows[0]["结算账户类型"].ToString().Trim();
                ht["提醒对象角色编号"] = jsbh;//df.Rows[0]["买家角色编号"].ToString().Trim();
                ht["提醒对象角色类型"] = dfjslx;
                ht["提醒内容文本"] = yhm + "上传了关于编号为" + dt.Rows[0]["htbh"].ToString().Trim() + "的电子购货合同的争议的证明文件，请进行确认。";
                ht["创建人"] = dt.Rows[0]["dlyx"].ToString().Trim();
                ht["type"] = "集合经销平台";
               
                 dtmess.Rows.Add(ht);
                 dtmess.AcceptChanges();
                 DataSet dsmess = new DataSet();
                 dsmess.Tables.Add(dtmess);
                 
               //调用系统控制中心发送提醒---------------------- wyh 2014.06.09 以后可能会做成一步，不必判断对错。

                re = IPC.Call("发送提醒", new object[] { dsmess });
                //获得结果
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "确认成功！";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "tj";


            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "提交失败，请稍后重试！";

            }
        }
        else
        {

            //获得结果
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "提交失败，请稍后重试！";

            //附加数据表
        }
        return dsreturn;

    }


    /// <summary>
    /// 争议文件确认 wyh  2014.06.12
    /// </summary>
    /// <param name="dt"></param>
    /// <returns>返回符合dareturn结构的Dataset</returns>
    public DataSet RunConfirmData( DataTable dt)
    {

        DataSet dsreturn = new TBD().initReturnDataSet().Copy();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化", "", "true" });

        //进行处理....
        if (dt != null && dt.Rows.Count > 0)
        {
            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");

            Hashtable input = new Hashtable();
            Hashtable htreturn = new Hashtable();

            DataTable dtemp = null;

            object[] re = IPC.Call("用户基本信息", new object[] { dt.Rows[0]["dlyx"].ToString().Trim()});//----------------
            if (re[0].ToString() == "ok")
            {
                dtemp = ((DataSet)(re[1])).Tables[0];
            }

            if (dtemp == null || dt.Rows.Count <= 0)
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取用户信息出错！";
                return dsreturn;
            }

            //判断是否进行了重复确认
            
            DataSet objqr = null;
            Hashtable htpar = new Hashtable();
            htpar["@Number"] = dt.Rows[0]["num"].ToString().Trim();
            htreturn = I_DBL.RunParam_SQL("select Q_SFYQR from AAA_ZBDBXXB where Number=@Number", "sf", htpar);

            if ((bool)(htreturn["return_float"])) //说明执行完成
            {
                //这里就可以用了。
                objqr = (DataSet)(htreturn["return_ds"]);

            }

            if (objqr != null && objqr.Tables[0].Rows[0][0].ToString().Trim() == "是")
            {
                //获得结果
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "cf";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "检测到已经在其他设备上进行了确认，无需重复操作！";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "qr";
                return dsreturn;

            }

            string yhm = dtemp.Rows[0]["交易方名称"].ToString().Trim();

            input["@Number"] = dt.Rows[0]["num"].ToString().Trim();
            string str = "UPDATE AAA_ZBDBXXB SET Q_QRSJ=GETDATE(),Q_SFYQR='是' WHERE Number=@Number ";
            htreturn = I_DBL.RunParam_SQL(str,input);
            int i = 0;
            if ((bool)(htreturn["return_float"])) //说明执行完成
            {
                //这里就可以用了。
                i = (int)(htreturn["return_other"]);

            }

            if (i > 0)
            {
                //获得结果
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "确认成功！";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "qr";

                //提醒
                string dfyx = dt.Rows[0]["dfyx"].ToString().Trim();
                string dfjslx = dt.Rows[0]["dfjslx"].ToString().Trim();



                DataTable dtmess = new TBD().dtmeww().Clone();
                DataRow ht = dtmess.NewRow();
                ht["提醒对象登陆邮箱"] = dfyx;
                ht["提醒对象结算账户类型"] = dtemp.Rows[0]["结算账户类型"].ToString().Trim();
                ht["提醒对象角色编号"] = dtemp.Rows[0]["买家角色编号"].ToString().Trim();
                ht["提醒对象角色类型"] = dfjslx;
                ht["提醒内容文本"] = yhm + "对您上传的关于编号为" + dt.Rows[0]["htbh"].ToString().Trim() + "的电子购货合同的争议证明文件进行了确认，请进行查看。";
                ht["创建人"] = dt.Rows[0]["dlyx"].ToString().Trim();
                ht["type"] = "集合经销平台";
                dtmess.Rows.Add(ht);

                //操作平台 
                DataRow htyept = dtmess.NewRow();
                htyept["type"] = "业务平台";
                htyept["模提醒模块名"] = "人工清盘";
                htyept["查看地址"] = "";
                htyept["提醒内容文本"] = yhm + "对关于编号为" + dt.Rows[0]["htbh"].ToString().Trim() + "的电子购货合同的争议证明文件进行了确认，请进行人工清盘处理。";
                dtmess.Rows.Add(htyept);
                DataSet dsmess = new DataSet();
                dsmess.Tables.Add(dtmess);
                //调用系统控制中心发送提醒---------------------- wyh 2014.06.09 以后可能会做成一步，不必判断对错。

                 re = IPC.Call("发送提醒", new object[] { dsmess });


            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "确认失败，请稍后重试！";
            }
        }
        else
        {

            //获得结果
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "确认失败，请稍后重试！";

            //附加数据表
        }
        return dsreturn;


    }
}