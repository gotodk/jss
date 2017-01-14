using System;
using System.Collections.Generic;
using System.Web;
using System.Data ;
using FMOP.DB;

/// <summary>
///Yhztcljtj 的摘要说明
/// </summary>
public class Yhztcljtj
{
	public Yhztcljtj()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}

    public YhztceClass Getvalue( string bsc,DateTime dt,DateTime dtStart)
    {
        YhztceClass ytc = new YhztceClass();
        //获取累计用户数值
        string Strzc = "select  count(*) as 累计用户总数,  count(case   when tgrybh  not like '_99999' then Number   end ) as 累计用户非公用注册数 , count(case  sfyyzyx when '否'   then Number end ) as 累计未激活邮箱 from fm_userszxyh where 1=1  ";


        string Strljdd = "    select isnull(sum(tgryzsy),0.0) as 累计收益金额,count(distinct yhbh) as 累计有效订单用户数,count(ddsl) as 累计有效订单数,  sum(ddsl) as 累计有效订单支数,sum(ddje)as 累计订单金额 from fm_yhddb where 1=1  ";
        //获取累计推广人员统计值
        string StrTGRY = "select count(*) as  累计推广代理人总数 ,count( case when ddjlbh not like '_99' then Number end ) as 累计推广人员非公用注册数,count(case sfyyzyx when '否' then Number end ) as 累计未激活邮箱数  from  fm_userstgry where 1=1 ";

        if(bsc.IndexOf("办事处")>0)
        { 
            Strzc += " and  YYYSSBSCDYSSXSQY='"+bsc.Trim()+"'" ;
            Strljdd += " and SSBSC='"+bsc.Trim()+"'";
            StrTGRY += " and  YYYSSBSCDYSSXSQY = '"+bsc.Trim()+"'";
        }

        if (dt != null)
        {
            Strzc += " and createtime <='" + dt+"'";
            Strljdd += " and createtime <='" + dt+"'";
            StrTGRY += " and   createtime <='" + dt+"'";
        }

        if (dtStart != null)
        {
            Strzc += " and createtime >='" + dtStart + "'";
            Strljdd += " and createtime >='" + dtStart + "'";
            StrTGRY += " and   createtime >='" + dtStart + "'";
        }

        DataSet dsdd = DbHelperSQL.Query(Strljdd);
        DataSet ds = DbHelperSQL.Query(Strzc);
        DataSet dstgry = DbHelperSQL.Query(StrTGRY);
        ytc.Labyhzcs = int.Parse(ds.Tables[0].Rows[0]["累计用户总数"].ToString().Trim());
        ytc.Labyhfgyzcs = int.Parse(ds.Tables[0].Rows[0]["累计用户非公用注册数"].ToString().Trim());
        ytc.Labyhgyzcs = int.Parse(ds.Tables[0].Rows[0]["累计用户总数"].ToString().Trim()) - int.Parse(ds.Tables[0].Rows[0]["累计用户非公用注册数"].ToString().Trim());
        ytc.Labyhwjhyx = int.Parse(ds.Tables[0].Rows[0]["累计未激活邮箱"].ToString().Trim());
        ytc.Labsyje = double.Parse(dsdd.Tables[0].Rows[0]["累计收益金额"].ToString().Trim());
        ytc.Labxdyxddyhs = int.Parse(dsdd.Tables[0].Rows[0]["累计有效订单用户数"].ToString().Trim());
        ytc.Labyxdds = int.Parse(dsdd.Tables[0].Rows[0]["累计有效订单数"].ToString().Trim());
        ytc.Labyxddjesl = dsdd.Tables[0].Rows[0]["累计订单金额"].ToString().Trim() + "/" + dsdd.Tables[0].Rows[0]["累计有效订单支数"].ToString().Trim();
        ytc.Labzcs = int.Parse(dstgry.Tables[0].Rows[0]["累计推广代理人总数"].ToString().Trim());
        ytc.Labfgyzcs = int.Parse(dstgry.Tables[0].Rows[0]["累计推广人员非公用注册数"].ToString().Trim());
        ytc.Labgyzcs = int.Parse(dstgry.Tables[0].Rows[0]["累计推广代理人总数"].ToString().Trim()) - int.Parse(dstgry.Tables[0].Rows[0]["累计推广人员非公用注册数"].ToString().Trim());
        ytc.Labwjhyx = int.Parse(dstgry.Tables[0].Rows[0]["累计未激活邮箱数"].ToString().Trim());
    

        //当日累计情况

        //获取累计用户数值
        string StrzcNow = "select  count(*) as 累计用户总数,  count(case   when tgrybh  not like '_99999' then Number   end ) as 累计用户非公用注册数 , count(case  sfyyzyx when '否'   then Number end ) as 累计未激活邮箱 from fm_userszxyh where 1=1  ";


        string StrljddNow = "    select  isnull(sum(tgryzsy),0.0) as 累计收益金额,count(distinct yhbh) as 累计有效订单用户数,count(ddsl) as 累计有效订单数,  sum(ddsl) as 累计有效订单支数,sum(ddje)as 累计订单金额 from fm_yhddb where 1=1  ";
        //获取累计推广人员统计值
        string StrTGRYNow = "select count(*) as  累计推广代理人总数 ,count( case when ddjlbh not like '_99' then Number end ) as 累计推广人员非公用注册数,count(case sfyyzyx when '否' then Number end ) as 累计未激活邮箱数  from  fm_userstgry where 1=1 ";


        if (bsc.IndexOf("办事处") > 0)
        {
            StrzcNow += " and  YYYSSBSCDYSSXSQY='" + bsc.Trim() + "'";
            StrljddNow += " and SSBSC='" + bsc.Trim() + "'";
            StrTGRYNow += " and  YYYSSBSCDYSSXSQY = '" + bsc.Trim() + "'";
        }

        if (dt != null && dt.Year != 3000)
        {
            StrzcNow += " and DateDiff(dd,createtime,'"+dt+"') = 0";
            StrljddNow += " and DateDiff(dd,createtime,'"+dt+"') = 0";
            StrTGRYNow += " and DateDiff(dd,createtime,'"+dt+"') = 0";
        }
        else
        {
            StrzcNow += " and DateDiff(dd,createtime,Getdate()) = 0";
            StrljddNow += " and DateDiff(dd,createtime,Getdate()) = 0";
            StrTGRYNow += " and DateDiff(dd,createtime,Getdate()) = 0";
        }

        DataSet dsddNow = DbHelperSQL.Query(StrljddNow);
        DataSet dsNow = DbHelperSQL.Query(StrzcNow);
        DataSet dstgryNow = DbHelperSQL.Query(StrTGRYNow);

        ytc.LabyhzcsNow = int.Parse(dsNow.Tables[0].Rows[0]["累计用户总数"].ToString().Trim());
        ytc.LabyhfgyzcsNow = int.Parse(dsNow.Tables[0].Rows[0]["累计用户非公用注册数"].ToString().Trim());
        ytc.LabyhgyzcsNow = int.Parse(dsNow.Tables[0].Rows[0]["累计用户总数"].ToString().Trim()) - int.Parse(dsNow.Tables[0].Rows[0]["累计用户非公用注册数"].ToString().Trim());
        ytc.LabyhwjhyxNow = int.Parse(dsNow.Tables[0].Rows[0]["累计未激活邮箱"].ToString().Trim());
        ytc.LabsyjeNow = double.Parse(dsddNow.Tables[0].Rows[0]["累计收益金额"].ToString().Trim());
        ytc.LabyhxdyxddyhsNow = int.Parse(dsddNow.Tables[0].Rows[0]["累计有效订单用户数"].ToString().Trim());
        ytc.LabyhyxddsNow = int.Parse(dsddNow.Tables[0].Rows[0]["累计有效订单数"].ToString().Trim());
        ytc.LabyhyxddjessNow = dsddNow.Tables[0].Rows[0]["累计订单金额"].ToString().Trim() + "/" + dsddNow.Tables[0].Rows[0]["累计有效订单支数"].ToString().Trim();
        ytc.LabzcsNow = int.Parse(dstgryNow.Tables[0].Rows[0]["累计推广代理人总数"].ToString().Trim());
        ytc.LabfgyzcsNow = int.Parse(dstgryNow.Tables[0].Rows[0]["累计推广人员非公用注册数"].ToString().Trim());
        ytc.LabgyzcsNow = int.Parse(dstgryNow.Tables[0].Rows[0]["累计推广代理人总数"].ToString().Trim()) - int.Parse(dstgryNow.Tables[0].Rows[0]["累计推广人员非公用注册数"].ToString().Trim());
          ytc.LabwjhyxNow = int.Parse(dstgryNow.Tables[0].Rows[0]["累计未激活邮箱数"].ToString().Trim());



        return ytc;

        

    }


}