using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FMOP.DB;

/// <summary>
/// jhjx_greatbankfile 的摘要说明
/// </summary>
public class jhjx_greatbankfile
{
    string Strjssj = "";
    string Strwhere = "";
	public jhjx_greatbankfile()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
        Strjssj = getbssj();
       // Strwhere = GetSqlwhere();
	}



    public DataSet GetSqlwhere(DataSet dsreturn, DataTable ht)
    {

        string Strwhere = "";
        DataSet dswher = DbHelperSQL.Query("select Number from   AAA_PingAnQSZTJLB where SJCSSSSYH='浦发银行'  and QSZT in('未处理','已发送')");
        if (dswher != null && dswher.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in dswher.Tables[0].Rows)
            {
                Strwhere += "'" + dr["Number"].ToString().Trim() + "',";
            }

            Strwhere = Strwhere.Trim().Remove(Strwhere.Trim().Length - 1);
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "恩，不错取到了";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = Strwhere.Trim();
        }
        else
        {
            Strwhere = "'怎么可能存在11'";
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "木有啊";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "！";
        }

      
        return dsreturn;
    }

    private  string  getbssj()
    {
       string  aa= "";
       string Strsql = "select PTMRJSFWSJ from AAA_PTDTCSSDB";
       object obj = DbHelperSQL.GetSingle(Strsql);
       if (obj != null && !obj.ToString().Trim().Equals(""))
       {
           aa = obj.ToString().Trim()+":00.000";
       }
         
        return aa;
    }

    /// <summary>
    /// 存管客户交易资金净额清算文件 
    /// </summary>
    /// <param name="dsreturn"></param>
    /// <param name="ht"></param>
    /// <returns></returns>
	
    public DataSet Getcgkhjyzijefile(DataSet dsreturn, DataTable ht)
    {
        try
        {
            string Strbakdnam = ht.Rows[0]["开户银行"].ToString();

            //string StrSql = " select ROW_NUMBER () over (order by 证券资金账号)  as '顺序流水号', * from (  select '' as '银行类型', I_YHZH as '存管账号/银行结算账号', '60050000' as '证券公司编号', '0000' as '营业部编码','' as '证券资金账号类型',I_ZQZJZH as '证券资金账号', '0' as '币种' ,SUM(je) as 清算金额 ,'' as 'T+N天数','' as '备注','' as '处理标记','' as '证券买入汇总金额', '' as '证券卖出汇总金额' from  (select  I_YHZH , I_ZQZJZH , (case YSLX when '冻结'  then -isnull(JE,0.00) when '扣减' then -isnull(JE,0.00) when '解冻' then isnull(JE,0.00) else isnull(JE,0.00) end) as je  from AAA_ZKLSMXB as a left join AAA_DLZHXXB as b on ( a.JSBH = b.J_BUYJSBH or a.JSBH = b.J_JJRJSBH or a.JSBH = b.J_SELJSBH )   where SJLX = '实' and XM <> '转出资金' and XM<> '转入资金' and b.I_KHYH = '" + Strbakdnam + "' and  (a.LSCSSJ between '" + ht.Rows[0]["结算时间"].ToString().Trim() + " 00:00:00.000' and '" + ht.Rows[0]["结算时间"].ToString().Trim() + " " + Strjssj.Trim() + "' ))  as c group by  I_ZQZJZH,I_YHZH ) as tab "; //按时间查找数据

            string Strwhere = ht.Rows[0]["where"].ToString();
           
            string StrSql = " select ROW_NUMBER () over (order by 证券资金账号)  as '顺序流水号', * from (  select '' as '银行类型', I_YHZH as '存管账号/银行结算账号', '60050000' as '证券公司编号', '0000' as '营业部编码','' as '证券资金账号类型',I_ZQZJZH as '证券资金账号', '0' as '币种' ,SUM(je) as 清算金额 ,'' as 'T+N天数','' as '备注','' as '处理标记','' as '证券买入汇总金额', '' as '证券卖出汇总金额' from  (select  I_YHZH , I_ZQZJZH , (case YSLX when '冻结'  then -isnull(JE,0.00) when '扣减' then -isnull(JE,0.00) when '解冻' then isnull(JE,0.00) else isnull(JE,0.00) end) as je  from AAA_ZKLSMXB as a left join AAA_DLZHXXB as b on ( a.JSBH = b.J_BUYJSBH or a.JSBH = b.J_JJRJSBH or a.JSBH = b.J_SELJSBH )   where SJLX = '实' and XM <> '转出资金' and XM<> '转入资金' and b.I_KHYH = '" + Strbakdnam + "' and  a.Number in (" + Strwhere + ") )  as c group by  I_ZQZJZH,I_YHZH ) as tab "; //按清算状态记录表查找数据 wyh 2014.05.15 add
            DataSet dsget = DbHelperSQL.Query(StrSql);
            int a = 0;
            if (dsget != null && dsget.Tables[0].Rows.Count > 0)
            {
                DataTable dt = dsget.Tables[0].Copy();
                dt.TableName = "存管客户交易资金净额清算文件";
                dsreturn.Tables.Add(dt);
                string strupdate = "update AAA_PingAnQSZTJLB set QSZT='已发送' where Number in (" + Strwhere + ") and QSZT='未处理' and  SJCSSSSYH='浦发银行'";
                 a = DbHelperSQL.ExecuteSql(strupdate);

            }
            else
            {
                DataTable newdtb = new DataTable();

                newdtb.Columns.Add("ProName", typeof(string));

                newdtb.TableName = "存管客户交易资金净额清算文件";
                DataRow newRow = newdtb.NewRow();
                newRow["ProName"] = "";

                newdtb.Rows.Add(newRow);

                dsreturn.Tables.Add(newdtb.Copy());
            }
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "存管客户交易资金净额清算文件数据获取成功！同时"+a.ToString()+"条数据状态被更改为“已发送”。";
            return dsreturn;
        }
        catch (Exception ex)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取存管客户银证转帐对明细帐文件数据时失败，错误提示：！"+ex.ToString();
            return dsreturn;
        }
    }
    //5.2.3	存管客户批量利息入帐文件
    public DataSet Getcgkhpllx(DataSet dsreturn, DataTable ht)
    {
        DataTable newdtb = new DataTable();
     
        newdtb.Columns.Add("ProName", typeof(string));
      
        newdtb.TableName = "存管客户批量利息入帐文件";
        DataRow newRow = newdtb.NewRow();
        newRow["ProName"] = "";
      
        newdtb.Rows.Add(newRow);

        dsreturn.Tables.Add(newdtb.Copy());
        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "存管客户批量利息入帐文件数据获取成功！";
        return dsreturn;

 
    }

    //5.2.4	存管客户资金余额明细文件
    public DataSet Getcgkhzjmc(DataSet dsreturn, DataTable ht)
    {
        try
        {
            string Strbakdnam = ht.Rows[0]["开户银行"].ToString();
            string StrSqlget = " select I_YHZH as '存管账号/银行结算账号',I_ZQZJZH as '证券资金账号','0' as '币种',isnull(B_ZHDQKYYE,0.00) as 证券资金余额,'' as 未回买入资金,'' as 未回卖出资金,'' as 冻结资金, '' as 日期 from AAA_DLZHXXB where I_KHYH = '" + Strbakdnam + "' and I_DSFCGZT = '开通'  ";
            DataSet dsget = DbHelperSQL.Query(StrSqlget);
            if (dsget != null && dsget.Tables[0].Rows.Count > 0)
            {
                DataTable dtable = dsget.Tables[0].Copy();
                dtable.TableName = "存管客户资金余额明细文件";
                dsreturn.Tables.Add(dtable);
            }
            else
            {
                DataTable newdtb = new DataTable();

                newdtb.Columns.Add("ProName", typeof(string));

                newdtb.TableName = "存管客户资金余额明细文件";
                DataRow newRow = newdtb.NewRow();
                newRow["ProName"] = "";

                newdtb.Rows.Add(newRow);

                dsreturn.Tables.Add(newdtb.Copy());
            }
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "存管客户资金余额明细文件数据获取成功！";
            return dsreturn;
        }
        catch (Exception ex)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取存管客户资金余额明细文件数据时失败，错误提示：！" + ex.ToString();
            return dsreturn;
        }

    }

    //5.2.5	存管汇总账户资金交收汇总文件格式
    public DataSet Getcghzzhzjjs(DataSet dsreturn, DataTable ht)
    {
        try
        {
            string Strbakdnam = ht.Rows[0]["开户银行"].ToString();
              string Strwhere = ht.Rows[0]["where"].ToString();
          //  string StrSqlget = " select '60050000' as 证券公司编号,'5' as 业务类型,'O' as 交收业务代码,'' as 交收公司代码,'' as 交收公司名称,'' as 品种代码,'' as 品种名称,'' as 银行账号,'' as 银行账号名称,'0' as 币种,sum((case YSLX when '冻结'  then -isnull(JE,0.00) when '扣减' then -isnull(JE,0.00) when '解冻' then isnull(JE,0.00) else isnull(JE,0.00) end)) as 交收金额,'' as 交易日期,'' as 签约期初金额,'' as 解约金额,'' as 销户利息金额,'' as 季度利息金额,'' as 证券交易金额,'' as 其他金额,'' as 备注 from AAA_ZKLSMXB  as a left join AAA_DLZHXXB as b on ( a.JSBH = b.J_BUYJSBH or a.JSBH = b.J_JJRJSBH or a.JSBH = b.J_SELJSBH )  where (a.LSCSSJ between '" + ht.Rows[0]["结算时间"].ToString().Trim() + " 00:00:00.000' and '" + ht.Rows[0]["结算时间"].ToString().Trim() + " " + Strjssj.Trim() + "' ) and  SJLX = '实' and XM <> '转出资金' and XM<> '转入资金' and b.I_KHYH = '" + Strbakdnam + "'";
              string StrSqlget = " select '60050000' as 证券公司编号,'5' as 业务类型,'O' as 交收业务代码,'' as 交收公司代码,'' as 交收公司名称,'' as 品种代码,'' as 品种名称,'' as 银行账号,'' as 银行账号名称,'0' as 币种,sum((case YSLX when '冻结'  then -isnull(JE,0.00) when '扣减' then -isnull(JE,0.00) when '解冻' then isnull(JE,0.00) else isnull(JE,0.00) end)) as 交收金额,'' as 交易日期,'' as 签约期初金额,'' as 解约金额,'' as 销户利息金额,'' as 季度利息金额,'' as 证券交易金额,'' as 其他金额,'' as 备注 from AAA_ZKLSMXB  as a left join AAA_DLZHXXB as b on ( a.JSBH = b.J_BUYJSBH or a.JSBH = b.J_JJRJSBH or a.JSBH = b.J_SELJSBH )  where a.Number in (" + Strwhere + ") and  SJLX = '实' and XM <> '转出资金' and XM<> '转入资金' and b.I_KHYH = '" + Strbakdnam + "'"; ;
            DataSet dsget = DbHelperSQL.Query(StrSqlget);
            if (dsget != null && dsget.Tables[0].Rows.Count > 0)
            {
                DataTable dtable = dsget.Tables[0].Copy();
                dtable.TableName = "存管汇总账户资金交收汇总文件";
                dsreturn.Tables.Add(dtable);
            }
            else
            {
                DataTable newdtb = new DataTable();

                newdtb.Columns.Add("ProName", typeof(string));

                newdtb.TableName = "存管汇总账户资金交收汇总文件";
                DataRow newRow = newdtb.NewRow();
                newRow["ProName"] = "";

                newdtb.Rows.Add(newRow);

                dsreturn.Tables.Add(newdtb.Copy());
 
            }
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "存管汇总账户资金交收汇总文件文件数据获取成功！";
            return dsreturn;
        }
        catch (Exception ex)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取存管汇总账户资金交收汇总文件时失败，错误提示：！" + ex.ToString();
            return dsreturn;
        }

    }

    //  //5.1.3	存管客户银证转帐对明细帐文件格式
    //public DataSet Getcgkhzjmc(DataSet dsreturn, DataTable ht)
    //{
    //    try
    //    {
    //        string Strbakdnam = ht.Rows[0]["开户银行"].ToString();
    //        string StrSqlget = " select '60050000' as 证券公司编号,'5' as 业务类型,'O' as 交收业务代码,'' as 交收公司代码,'' as 交收公司名称,'' as 品种代码,'' as 品种名称,'' as 银行账号,'' as 银行账号名称,'0' as 币种,sum((case YSLX when '冻结'  then -isnull(JE,0.00) when '扣减' then -isnull(JE,0.00) when '解冻' then isnull(JE,0.00) else isnull(JE,0.00) end)) as 交收金额,'' as 交易日期,'' as 签约期初金额,'' as 解约金额,'' as 销户利息金额,'' as 季度利息金额,'' as 证券交易金额,'' as 其他金额,'' as 备注 from AAA_ZKLSMXB  as a left join AAA_DLZHXXB as b on ( a.JSBH = b.J_BUYJSBH or a.JSBH = b.J_JJRJSBH or a.JSBH = b.J_SELJSBH )  where datediff(day,a.CreateTime,getdate())=0 and  SJLX = '实' and XM <> '转出资金' and XM<> '转入资金' and b.I_KHYH = '" + Strbakdnam + "'";
    //        DataSet dsget = DbHelperSQL.Query(StrSqlget);
    //        DataTable dtable = dsget.Tables[0].Copy();
    //        dtable.TableName = "存管汇总账户资金交收汇总文件";
    //        dsreturn.Tables.Add(dtable);
    //        return dsreturn;
    //    }
    //    catch (Exception ex)
    //    {
    //        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
    //        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取存管汇总账户资金交收汇总文件时失败，错误提示：！" + ex.ToString();
    //        return dsreturn;
    //    }

    //}

    //    //5.1.3	存管客户银证转帐对明细帐文件格式 
    /// <summary>
    /// 得到银行和证券的转账信息
    /// </summary>
    /// <param name="dsreturn"></param>
    /// <param name="ht"></param>
    /// <returns></returns>
    public DataSet GetYZZZXX(DataSet dsreturn, DataTable ht)
    {
        try
        {
            string jssj = ht.Rows[0]["对账日期"].ToString();
            string Strwhere = ht.Rows[0]["where"].ToString();
           // string strGetYZZZXX = " select (case LYYWLX when '银行出金'  then '1' when '银行入金' then '1' when '券商入金' then '0' when '券商出金' then '0' end) '发起方', (case LYYWLX when '银行出金'  then '02' when '银行入金' then '01' when '券商入金' then '01' when '券商出金' then '02' end) '交易码',(select I_YHZH from AAA_DLZHXXB where B_DLYX=DD.B_DLYX ) '银行结算账号',(select I_ZQZJZH from AAA_DLZHXXB where B_DLYX=DD.B_DLYX ) '证券资金账号',JE '发生金额','0' '币种',LYDH '银行流水',ZZ.BDCJRJLSH '证券流水',convert(varchar(10),LSCSSJ,112) '交易日期',substring(convert(varchar(20),LSCSSJ,120),11,10) '交易时间','6' '银行类型','60050000' '券商编号','5' '业务类型' from  AAA_ZKLSMXB as ZZ   inner join AAA_DLZHXXB as DD on ZZ.DLYX=DD.B_DLYX where LYYWLX in ('银行入金','银行出金','券商发起','券商入金','券商出金') and DD.I_KHYH='" + ht.Rows[0]["开户银行"].ToString() + "' and convert(varchar(10),LSCSSJ,120)='" + jssj + "' ";
            string strGetYZZZXX = " select (case LYYWLX when '银行出金'  then '1' when '银行入金' then '1' when '券商入金' then '0' when '券商出金' then '0' end) '发起方', (case LYYWLX when '银行出金'  then '02' when '银行入金' then '01' when '券商入金' then '01' when '券商出金' then '02' end) '交易码',(select I_YHZH from AAA_DLZHXXB where B_DLYX=DD.B_DLYX ) '银行结算账号',(select I_ZQZJZH from AAA_DLZHXXB where B_DLYX=DD.B_DLYX ) '证券资金账号',JE '发生金额','0' '币种',LYDH '银行流水',ZZ.BDCJRJLSH '证券流水',convert(varchar(10),LSCSSJ,112) '交易日期',substring(convert(varchar(20),LSCSSJ,120),11,10) '交易时间','6' '银行类型','60050000' '券商编号','5' '业务类型' from  AAA_ZKLSMXB as ZZ   inner join AAA_DLZHXXB as DD on ZZ.DLYX=DD.B_DLYX where LYYWLX in ('银行入金','银行出金','券商发起','券商入金','券商出金') and DD.I_KHYH='" + ht.Rows[0]["开户银行"].ToString() + "' and ZZ.Number in (" + Strwhere + ") ";
            DataSet dsget = DbHelperSQL.Query(strGetYZZZXX);
            if (dsget != null)
            {
                DataTable dtable = dsget.Tables[0].Copy();
                dtable.TableName = "银证转账对账数据";
                dsreturn.Tables.Add(dtable);
            }
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "银证转账对账数据获取成功！";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = ht.Rows[0]["对账日期"].ToString(); 

            return dsreturn;
        }
        catch (Exception ex)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取银证转账对账数据时失败，错误提示：！" + ex.ToString();
            return dsreturn;
        }

    }


    /// <summary>
    /// 完成清算
    /// </summary>
    /// <param name="dsreturn"></param>
    /// <param name="ht"></param>
    /// <returns></returns>
    public DataSet WCQS(DataSet dsreturn, DataTable ht)
    {
        try
        {
            string qsrq = ht.Rows[0]["清算日期"].ToString();
            //同步登陆表的数据到【监管账户同步表】
            string strwhere = ht.Rows[0]["where"].ToString();
            
            if (strwhere.Equals(""))
            {
                strwhere = "''";          
            }

            string strSqlTBDLBSJ = "insert AAA_JGZHTBB(YHYX,QSBH,TBYE,TBRQ,BYZT,KHYH,YHZH) select B_DLYX '登录邮箱',isnull(I_ZQZJZH,'') '证券资金账号',B_ZHDQKYYE '当前账户可用余额','" + qsrq + "' as '同步日期','' as '备用状态',I_KHYH '开户银行',isnull(I_YHZH,'') '银行账号' from AAA_DLZHXXB  where I_KHYH='" + ht.Rows[0]["开户银行"].ToString() + "'";
            //更新登陆表的【第三方存管可用余额】
            string strSqlGXDLB = "update AAA_DLZHXXB set B_DSFCGKYYE=B_ZHDQKYYE where I_KHYH='" + ht.Rows[0]["开户银行"].ToString() + "'";

            //wyh 2014.05.19 add
            string Strupdate = "update AAA_PingAnQSZTJLB set QSZT='成功' where Number in (" + strwhere + ") and QSZT='已发送' and  SJCSSSSYH='浦发银行'";

            ArrayList arrayList = new ArrayList();
            arrayList.Add(strSqlTBDLBSJ);
            arrayList.Add(strSqlGXDLB);
            arrayList.Add(Strupdate);
            bool bo = DbHelperSQL.ExecSqlTran(arrayList);
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] ="对"+ Convert.ToDateTime(qsrq).ToString("yyyy年MM月dd日") + "的账目信息清算完成！";
            return dsreturn;
        }
        catch (Exception ex)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "执行清算失败，错误提示：！" + ex.ToString();
            return dsreturn;
        }
    
    
    }

 
                                                                                  

}