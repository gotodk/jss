using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using FMOP.DB;
using System.Collections;
using Hesion.Brick.Core.WorkFlow;
using System.Text;

/// <summary>
/// 处理只能匹配的各种操作
/// </summary>
public class PPauto
{
	public PPauto()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}
    /// <summary>
    /// 传入关键信息，获取匹配的商品编号字符串信息
    /// </summary>
    /// <param name="FL">商品分类关键信息</param>
    /// <param name="BTGJC">需求标题关键信息</param>
    /// <param name="YZBGJC">精确硬指标关键信息</param>
    /// <param name="RZBGJC">模糊软指标关键信息</param>
    /// <param name="QTMSGJC">其他描述关键信息</param>
    /// <param name="SFBX">是否保修</param>
    /// <param name="SFYFP">是否要求发票</param>
    /// <param name="DQ">所在区域</param>
    /// <param name="FBSJ">需求信息发布时间</param>
    /// <returns></returns>
    static public string GetPPTopNumber_XQ(string FL, string BTGJC, string YZBGJC, string RZBGJC, string QTMSGJC, string SFBX, string DQ)
    {
        /*
select top 60 '最终优先级'=sum(优先级权重),number from  (
select 优先级权重,number,createtime from 
(
select 最终优先级 = 0,优先级权重=0,* from MM_DBdemo2 where 1=2
UNION
select top 60 最终优先级 = 0,优先级权重=90,* from MM_DBdemo2 where FL like '手机,' or FL like '体育用品,' or FL like '生活用品,'
UNION 
select top 60 最终优先级 = 0,优先级权重=80,* from MM_DBdemo2 where BTGJC like '运动,' or BTGJC like 'G10,'
UNION 
select top 60 最终优先级 = 0,优先级权重=70,* from MM_DBdemo2 where SMGJC like '智能,' or SMGJC like '安卓,' or SMGJC like '双核,'
UNION 
select top 60 最终优先级 = 0,优先级权重=60,* from MM_DBdemo2 where SMGJC like '4.3寸,' or SMGJC like '红色,'
UNION 
select top 60 最终优先级 = 0,优先级权重=50,* from MM_DBdemo2 where SMGJC like '本地,' or SMGJC like '供应商,'
UNION 
select top 60 最终优先级 = 0,优先级权重=40,* from MM_DBdemo2 where SFBX = '是' 
UNION 
select top 60 最终优先级 = 0,优先级权重=20,* from MM_DBdemo2 where DQ = '山东济南'
) as Tab1
) as Tab2 group by number,createtime order by sum(优先级权重) desc,createtime desc 

         */

        //找出符合优先级条件的所有数据并计算优先级
        //拼接搜索sql
        StringBuilder sql = new StringBuilder("");
        sql.Append(" select top 60 '最终优先级'=sum(优先级权重),number from  ( select 优先级权重,number,createtime from ( select 最终优先级=0,优先级权重=0,* from MM_DBdemo2 where 1=2 ");
        if (FL != "")
        {
            StringBuilder sql_where = new StringBuilder("");
            string[] w = FL.Split(',');
            for (int r = 0; r < w.Length; r++)
            {
                if (r == 0)
                {
                    sql_where.Append(" FL like '" + w[r] + ",' ");
                }
                else
                {
                    sql_where.Append(" or FL like '" + w[r] + ",' ");
                }
            }
            sql.Append(" UNION select top 60 最终优先级=0,优先级权重=90,* from MM_DBdemo2 where ");
            sql.Append(sql_where);
        }
        if (BTGJC != "")
        {
            StringBuilder sql_where = new StringBuilder("");
            string[] w = BTGJC.Split(',');
            for (int r = 0; r < w.Length; r++)
            {
                if (r == 0)
                {
                    sql_where.Append(" BTGJC like '" + w[r] + ",' ");
                }
                else
                {
                    sql_where.Append(" or BTGJC like '" + w[r] + ",' ");
                }
            }
            sql.Append(" UNION select top 60 最终优先级=0,优先级权重=80,* from MM_DBdemo2 where ");
            sql.Append(sql_where);
        }
        if (YZBGJC != "")
        {
            StringBuilder sql_where = new StringBuilder("");
            string[] w = YZBGJC.Split(',');
            for (int r = 0; r < w.Length; r++)
            {
                if (r == 0)
                {
                    sql_where.Append(" SMGJC like '" + w[r] + ",' ");
                }
                else
                {
                    sql_where.Append(" or SMGJC like '" + w[r] + ",' ");
                }
            }
            sql.Append(" UNION select top 60 最终优先级=0,优先级权重=70,* from MM_DBdemo2 where ");
            sql.Append(sql_where);
        }
        if (RZBGJC != "")
        {
            StringBuilder sql_where = new StringBuilder("");
            string[] w = RZBGJC.Split(',');
            for (int r = 0; r < w.Length; r++)
            {
                if (r == 0)
                {
                    sql_where.Append(" SMGJC like '" + w[r] + ",' ");
                }
                else
                {
                    sql_where.Append(" or SMGJC like '" + w[r] + ",' ");
                }
            }
            sql.Append(" UNION select top 60 最终优先级=0,优先级权重=60,* from MM_DBdemo2 where ");
            sql.Append(sql_where);
        }
        if (QTMSGJC != "")
        {
            StringBuilder sql_where = new StringBuilder("");
            string[] w = QTMSGJC.Split(',');
            for (int r = 0; r < w.Length; r++)
            {
                if (r == 0)
                {
                    sql_where.Append(" SMGJC like '" + w[r] + ",' ");
                }
                else
                {
                    sql_where.Append(" or SMGJC like '" + w[r] + ",' ");
                }
            }
            sql.Append(" UNION select top 60 最终优先级=0,优先级权重=50,* from MM_DBdemo2 where ");
            sql.Append(sql_where);
        }
        if (SFBX == "是")
        {
            sql.Append(" UNION select top 60 最终优先级=0,优先级权重=40,* from MM_DBdemo2 where SFBX = '是'  ");
        }
        if (DQ != "")
        {
            sql.Append(" UNION select top 60 最终优先级=0,优先级权重=20,* from MM_DBdemo2 where DQ = '" + DQ + "' ");
        }
        sql.Append(" ) as Tab1 ) as Tab2 group by number,createtime order by sum(优先级权重) desc,createtime desc  ");
        string aaa = sql.ToString();
        DataSet ds = DbHelperSQL.Query(sql.ToString());


        //开始拼接编号字符串
        DataTable dt_temp = ds.Tables[0];
        string PPBH = "";
        for (int y = 0; y < dt_temp.Rows.Count; y++)
        {
            //只取60个匹配编号
            if (y == 60) 
            {
                break;
            }
            if (y == dt_temp.Rows.Count - 1)
            {
                PPBH = PPBH + dt_temp.Rows[y]["Number"].ToString();
            }
            else
            {
                PPBH = PPBH + dt_temp.Rows[y]["Number"].ToString() + ",";
            }
            
        }
        return PPBH;
    }
}