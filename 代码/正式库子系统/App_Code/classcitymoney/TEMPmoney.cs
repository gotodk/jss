using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using FMOP.DB;

/// <summary>
///TEMPmoney 的摘要说明
/// </summary>
public class TEMPmoney
{
    public string sp_sql_TC;

    public string sp_sql_BZ;
    public string sp_sql_KH;
	public TEMPmoney()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
        sp_sql_TC = " '时间段'=XM,'身份证号'=SFZH,'姓名'=XM,'有效信息情况'=XM,'有效信息奖励'=XM,'提成总额'=XM,'总销售额'=XM,'总销售量'=XM,'城市'=CS,'性别'=XB,'任职状态'=RZZT,'联系电话'=LXDH,'员工属性'=YGSX ";
        sp_sql_BZ = " '时间段'=XM,'身份证号'=SFZH,'姓名'=XM,'底薪或补助'=DXHBZ,'补发金额'=XM,'应出勤天数'=XM,'实际出勤天数'=XM,'其他扣款'=XM,'基本绩效工资'=JBJXGZ,'管理补贴'=GLBT,'考核得分'=XM,'实发补助'=XM,'城市'=CS,'性别'=XB,'任职状态'=RZZT,'联系电话'=LXDH,'员工属性'=YGSX,'到岗时间'=DGSJ ";
        sp_sql_KH = " '时间段'=XM,'身份证号'=SFZH,'姓名'=XM,'实际工作日'=XM,'每日工作量'=XM,'工作总量'=XM,'推广完成率'=XM,'推广完成率权重'=XM,'实发补助'=XM,'城市'=CS,'性别'=XB,'任职状态'=RZZT,'联系电话'=LXDH,'员工属性'=YGSX,'到岗时间'=DGSJ ";

	}

    /// <summary>
    /// 获取提成数据(sql)
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="time1">开始时间</param>
    /// <param name="time2">结束时间</param>
    /// <returns></returns>
    public DataSet getTC_ds_all(string sql, string time1, string time2)
    {
        DataSet ds_user = DbHelperSQL.Query(sql);
        //GV_show.DataSource = ds_user.Tables[0].DefaultView;
        //GV_show.DataBind();

        //计算提成总额和销售数量,以及有效信息奖励
        for (int i = 0; i < ds_user.Tables[0].Rows.Count; i++)
        {
            string upsql2;
            string timeduan = "";
            upsql2 = "select '提成总额'=sum(LZCPLX.YGTC),'总销售额'=sum(LZCPLX.JEY),'总销售量'=sum(LZCPLX.SLZ) from LZCPLX left join GreenCoverSalesBill on LZCPLX.parentNumber = GreenCoverSalesBill.Number where GreenCoverSalesBill.XSRSFZH = '" + ds_user.Tables[0].Rows[i]["身份证号"] + "' and (CreateTime between '" + time1 + "' and '" + time2 + "')";
            timeduan = time1.Replace(" 00:00:00", "") + "到" + time2.Replace(" 23:59:59", "");

            DataSet ds_buy = DbHelperSQL.Query(upsql2);
            if (ds_buy.Tables[0].Rows[0]["提成总额"].ToString() == "")
            {
                ds_buy.Tables[0].Rows[0]["提成总额"] = "0";
            }
            if (ds_buy.Tables[0].Rows[0]["总销售额"].ToString() == "")
            {
                ds_buy.Tables[0].Rows[0]["总销售额"] = "0";
            }
            if (ds_buy.Tables[0].Rows[0]["总销售量"].ToString() == "")
            {
                ds_buy.Tables[0].Rows[0]["总销售量"] = "0";
            }

            ds_user.Tables[0].Rows[i]["提成总额"] = ds_buy.Tables[0].Rows[0]["提成总额"].ToString();
            ds_user.Tables[0].Rows[i]["总销售额"] = ds_buy.Tables[0].Rows[0]["总销售额"].ToString();
            ds_user.Tables[0].Rows[i]["总销售量"] = ds_buy.Tables[0].Rows[0]["总销售量"].ToString();
            ds_user.Tables[0].Rows[i]["时间段"] = timeduan;




            string upsql9 = "select * from temp_YHBFJLB where SFZH = '" + ds_user.Tables[0].Rows[i]["身份证号"] + "' and (BFSJ between '" + time1 + "' and '" + time2 + "')";
            DataSet ds_bfjl = DbHelperSQL.Query(upsql9);
            int shuliang1 = 0; //一级数量
            int shuliang2 = 0;//二级数量
            int shuliang3 = 0;//三级数量
            int shuliang4 = 0;//四级数量
            int shuliang5 = 0;//无级别数量
            int zongjiangli = 0;
            for (int b = 0; b < ds_bfjl.Tables[0].Rows.Count; b++)
            {
                if(ds_bfjl.Tables[0].Rows[b]["YHJB"].ToString() == "一级")
                {
                    shuliang1++;
                    zongjiangli = zongjiangli + 6;
                }
                if (ds_bfjl.Tables[0].Rows[b]["YHJB"].ToString() == "二级")
                {
                    shuliang2++;
                    zongjiangli = zongjiangli + 4;
                }
                if (ds_bfjl.Tables[0].Rows[b]["YHJB"].ToString() == "三级")
                {
                    shuliang3++;
                    zongjiangli = zongjiangli + 2;
                }
                if (ds_bfjl.Tables[0].Rows[b]["YHJB"].ToString() == "四级")
                {
                    shuliang4++;
                    zongjiangli = zongjiangli + 1;
                }
                if (ds_bfjl.Tables[0].Rows[b]["YHJB"].ToString() == "无级别")
                {
                    shuliang5++;
                    //zongjiangli = zongjiangli + 1;
                }
            }
            ;
            ds_user.Tables[0].Rows[i]["有效信息情况"] = "一级:" + shuliang1 + "|二级:" + shuliang2 + "|三级:" + shuliang3 + "|四级:" + shuliang4 + "|无级别:" + shuliang5;

            //如果是 机动兼职销售推广专员 ，则计算有效信息奖励
            if (ds_user.Tables[0].Rows[0]["员工属性"].ToString() == "机动兼职销售推广专员")
                {
                    ds_user.Tables[0].Rows[i]["有效信息奖励"] = zongjiangli + "";
                }
                else
                {
                    ds_user.Tables[0].Rows[i]["有效信息奖励"] = "0";
                }

        }
        return ds_user;
    }

    /// <summary>
    /// 获取所有员工提成信息列表
    /// </summary>
    /// <param name="time1">开始时间</param>
    /// <param name="time2">结束时间</param>
    /// <returns></returns>
    public DataSet getTC_ds(string time1 , string time2)
    {
        //获取所有临时员工
        string upsql = "select " + sp_sql_TC + " from temp_employee";
        return getTC_ds_all(upsql,time1 , time2);
    }

    


    /// <summary>
    /// 获取所有员工提成信息列表(某城市)
    /// </summary>
    /// <param name="city_str">城市办事处名称(部门名称)</param>
    /// <param name="time1">开始时间</param>
    /// <param name="time2">结束时间</param>
    /// <returns></returns>
    public DataSet getTC_ds(string city_str,string time1 , string time2)
    {
        //获取所有临时员工
        string upsql = "select " + sp_sql_TC + " from temp_employee where  CS = '" + city_str + "'";
        return getTC_ds_all(upsql, time1, time2);
    }

    /// <summary>
    /// 获取所有员工提成信息列表(某身份证)
    /// </summary>
    /// <param name="sfz_str">身份证信息</param>
    /// <param name="ff"></param>
    /// <param name="time1">开始时间</param>
    /// <param name="time2">结束时间</param>
    /// <returns></returns>
    public DataSet getTC_ds(string sfz_str, string ff, string time1, string time2)
    {
        //获取所有临时员工
        string upsql = "select " + sp_sql_TC + " from temp_employee where SFZH = '" + sfz_str + "'";
        return getTC_ds_all(upsql, time1, time2);
    }























    /// <summary>
    /// 获取补助数据(sql)
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="time1">开始时间</param>
    /// <param name="time2">结束时间</param>
    /// <returns></returns>
    public DataSet getBZ_ds_all(string sql, string time1, string time2)
    {
        DataSet ds_user = DbHelperSQL.Query(sql);
        //GV_show.DataSource = ds_user.Tables[0].DefaultView;
        //GV_show.DataBind();

        //计算提成总额和销售数量
        for (int i = 0; i < ds_user.Tables[0].Rows.Count; i++)
        {
            //string upsql2;

            //upsql2 = "select '临时补助额'=LSBZE,'正式补助额'=ZSBZJE from CSTGZYBZJBXXB where CS = '" + ds_user.Tables[0].Rows[i]["城市"].ToString() + "'";
            string timeduan = time1.Replace(" 00:00:00", "") + "到" + time2.Replace(" 23:59:59", "");

            //DataSet ds_buy = DbHelperSQL.Query(upsql2);

            //if (ds_user.Tables[0].Rows[i]["员工属性"].ToString() == "临时推广人员")
            //{
            //    ds_user.Tables[0].Rows[i]["底薪或补助"] = ds_buy.Tables[0].Rows[0]["临时补助额"].ToString();
            //}
            //else
            //{
            //    ds_user.Tables[0].Rows[i]["底薪或补助"] = ds_buy.Tables[0].Rows[0]["正式补助额"].ToString();
            //}
            
            ds_user.Tables[0].Rows[i]["补发金额"] = "0";
            ds_user.Tables[0].Rows[i]["实际出勤天数"] = "0";
            ds_user.Tables[0].Rows[i]["其他扣款"] = "0";
            ds_user.Tables[0].Rows[i]["实发补助"] = "0";
            ds_user.Tables[0].Rows[i]["考核得分"] = "0";
            ds_user.Tables[0].Rows[i]["时间段"] = timeduan;

        }
        return ds_user;
    }

    /// <summary>
    /// 获取所有员工补助信息列表
    /// </summary>
    /// <param name="time1">开始时间</param>
    /// <param name="time2">结束时间</param>
    /// <returns></returns>
    public DataSet getBZ_ds(string time1, string time2)
    {
        //获取所有临时员工
        string upsql = "select " + sp_sql_BZ + " from temp_employee ";
        return getBZ_ds_all(upsql, time1, time2);
    }




    /// <summary>
    /// 获取所有员工补助信息列表(某城市)
    /// </summary>
    /// <param name="city_str">城市办事处名称(部门名称)</param>
    /// <param name="time1">开始时间</param>
    /// <param name="time2">结束时间</param>
    /// <returns></returns>
    public DataSet getBZ_ds(string city_str, string time1, string time2)
    {
        //获取所有临时员工
        string upsql = "select " + sp_sql_BZ + " from temp_employee where  CS = '" + city_str + "'";
        return getBZ_ds_all(upsql, time1, time2);
    }

    /// <summary>
    /// 获取所有员工补助信息列表(某身份证)
    /// </summary>
    /// <param name="sfz_str">身份证信息</param>
    /// <param name="ff"></param>
    /// <param name="time1">开始时间</param>
    /// <param name="time2">结束时间</param>
    /// <returns></returns>
    public DataSet getBZ_ds(string sfz_str, string ff, string time1, string time2)
    {
        //获取所有临时员工
        string upsql = "select " + sp_sql_BZ + " from temp_employee where  SFZH = '" + sfz_str + "'";
        return getBZ_ds_all(upsql, time1, time2);
    }
}
