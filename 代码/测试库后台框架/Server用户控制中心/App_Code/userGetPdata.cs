using FMipcClass;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// userGetPdata 的摘要说明
/// </summary>
public class userGetPdata
{
	public userGetPdata()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}

    /// <summary>
    /// 初始化返回值数据集,执行结果只有两种ok和err(大多数情况是这个标准)
    /// </summary>
    /// <returns></returns>
    private DataSet initReturnDataSet()
    {
        DataSet ds = new DataSet();
        DataTable auto2 = new DataTable();
        auto2.TableName = "返回值单条";
        auto2.Columns.Add("执行结果");
        auto2.Columns.Add("提示文本");
        auto2.Columns.Add("附件信息1");
        auto2.Columns.Add("附件信息2");
        auto2.Columns.Add("附件信息3");
        auto2.Columns.Add("附件信息4");
        auto2.Columns.Add("附件信息5");
        ds.Tables.Add(auto2);
        return ds;
    }

    /// <summary>
    /// 经纪人业务管理B区获取分页数据
    /// </summary>
    /// <param name="ds_page">分页使用参数</param>
    /// <param name="ht_tiaojian">查询条件</param>
    /// <returns></returns>
    public DataSet GetBqJJRYWGL(DataSet ds_page, Hashtable ht_tiaojian)
    {
        DataSet dsreturn = new DataSet();
        switch (ht_tiaojian["标签名"].ToString())
        {
            case "审核交易方资料":
                dsreturn = GetBqSHJYFZL(ds_page, ht_tiaojian);
                break;
            case "交易方信用等级查询":
                dsreturn = GetBqJYFXYDJCX(ds_page, ht_tiaojian);
                break;
            case "交易方信用等级详情":
                dsreturn = GetBqJYFXYXQ(ds_page, ht_tiaojian);
                break;
            case "查看银行员工列表":
                dsreturn = GetBqBankYGLB(ds_page, ht_tiaojian);
                break; 
            default:
                dsreturn = null;
                break;
        }
        return dsreturn;
    }

    /// <summary>
    /// 经纪人业务管理B区审核交易方资料获取分页数据
    /// </summary>
    /// <param name="ds_page">大分页参数集</param>
    /// <param name="ht_tiaojian">查询条件</param>
    /// <returns></returns>
    public DataSet GetBqSHJYFZL(DataSet ds_page, Hashtable ht_tiaojian)
    {
        try
        {
            DataSet ds_res = new DataSet();//待返回的结果集  
         
            //设置分页功能需要的参数
            //dt_page.Rows[0]["this_dblink"] = "";//一般不用填，默认webconfig中的mainsqlserver。有特殊情况需连接别的数据库时使用，填写webconfig中添加的数据库链接的“name”值。
            // ds_page.Tables[0].Rows[0]["GetCustomersDataPage_NAME"] = " GetCustomersDataPage2 ";  //使用的存储过程名称,不设置的时候默认为GetCustomersDataPage。
            ds_page.Tables[0].Rows[0]["serach_Row_str"] = " a.Number as '关联表Number',b.B_DLYX '交易方账号',b.I_JYFMC '交易方名称',c.J_JJRZGZSBH '经纪人资格证书编号',b.B_DLYX as '登录邮箱',b.J_SELJSBH as '卖家角色编号',a.GLJJRBH '关联经纪人编号',c.I_JYFMC '经纪人名称',convert(varchar(20),b.I_ZLTJSJ,120) '资料提交时间',a.CreateTime,b.I_ZCLB '注册类型',b.I_SSQYS+b.I_SSQYSHI+b.I_SSQYQ '所属区域',c.I_PTGLJG '平台管理机构',b.I_LXRXM '联系人姓名',b.I_LXRSJH '联系电话',a.JJRSHSJ '初审时间',a.JJRSHZT '初审记录',a.FGSSHZT '复审记录',(case a.JJRSHZT when '审核中' then '--' when '驳回' then a.SFXG else '' end) '驳回后是否修改' "; //检索字段(必须设置)
            ds_page.Tables[0].Rows[0]["search_tbname"] = " AAA_MJMJJYZHYJJRZHGLB as a inner join  AAA_DLZHXXB as b on a.DLYX=b.B_DLYX left join AAA_DLZHXXB as c on a.GLJJRBH=c.J_JJRJSBH ";  //检索的表(必须设置)
            ds_page.Tables[0].Rows[0]["search_mainid"] = " a.Number ";  //所检索表的主键(必须设置)
            ds_page.Tables[0].Rows[0]["search_str_where"] = " a.JSZHLX='买家卖家交易账户' and a.JJRSHZT in('审核中','驳回') and a.SFYX='是' ";  //检索条件(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixu"] = " ASC ";  //排序方式(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixuZD"] = " a.CreateTime ";  //用于排序的字段(必须设置)  


            if (!ht_tiaojian.Contains("经纪人编号"))
            {//如果传入的参数不包括这几项，说明有问题，直接返回null

                return null;
            }
            ds_page.Tables[0].Rows[0]["search_str_where"] += " and a.GLJJRBH='" + ht_tiaojian["经纪人编号"].ToString() + "' ";
            //使用ht_tiaojian 传入的参数拼出需要增加的条件
            if (ht_tiaojian["交易方名称"].ToString().Trim() != "")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += " and b.I_JYFMC like '%" + ht_tiaojian["交易方名称"].ToString() + "%' ";
            }
            if (ht_tiaojian["省份"].ToString().Trim() != "请选择省份")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += " and b.I_SSQYS like '%" + ht_tiaojian["省份"].ToString() + "%' ";
            }
            if (ht_tiaojian["地市"].ToString().Trim() != "请选择城市")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += " and b.I_SSQYSHI like'%" + ht_tiaojian["地市"].ToString() + "%' ";
            }
            if (ht_tiaojian["区县"].ToString().Trim() != "请选择区县")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += " and b.I_SSQYQ like'%" + ht_tiaojian["区县"].ToString() + "%' ";
            }

            //调用执行方法获取数据
            object[] re_ds = IPC.Call("分页数据获取", new object[] { ds_page });
            if (re_ds[0].ToString() == "ok")
            {
                //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                 ds_res = (DataSet)re_ds[1];
            }
            else
            {
                ds_res = null;
            }
            return ds_res;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 经纪人业务管理B区交易方信用等级查询获取分页数据
    /// </summary>
    /// <param name="ds_page">大分页参数集</param>
    /// <param name="ht_tiaojian">查询条件</param>
    /// <returns></returns>
    public DataSet GetBqJYFXYDJCX(DataSet ds_page, Hashtable ht_tiaojian)
    {
        try
        {
            DataSet ds_res = new DataSet();//待返回的结果集  

            //设置分页功能需要的参数
            //dt_page.Rows[0]["this_dblink"] = "";//一般不用填，默认webconfig中的mainsqlserver。有特殊情况需连接别的数据库时使用，填写webconfig中添加的数据库链接的“name”值。
            // ds_page.Tables[0].Rows[0]["GetCustomersDataPage_NAME"] = " GetCustomersDataPage2 ";  //使用的存储过程名称,不设置的时候默认为GetCustomersDataPage。
            ds_page.Tables[0].Rows[0]["serach_Row_str"] = " DLYX as 交易方账号,I_JYFMC as 交易方名称,I_SSQYS+I_SSQYSHI as 所在区域,'无' as 当前信用等级,isnull(B_ZHDQXYFZ,0.00) as 当前信用积分 "; //检索字段(必须设置)
            ds_page.Tables[0].Rows[0]["search_tbname"] = " AAA_MJMJJYZHYJJRZHGLB as a left join AAA_DLZHXXB as b on a.DLYX=b.B_DLYX ";  //检索的表(必须设置)
            ds_page.Tables[0].Rows[0]["search_mainid"] = " DLYX ";  //所检索表的主键(必须设置)
            ds_page.Tables[0].Rows[0]["search_str_where"] = " 1=1 and SFDQMRJJR='是' and DLYX in (select distinct DLYX from AAA_JYFXYMXB) ";  //检索条件(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixu"] = " DESC ";  //排序方式(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixuZD"] = " isnull(B_ZHDQXYFZ,0) ";  //用于排序的字段(必须设置)  


            if (!ht_tiaojian.Contains("用户邮箱"))
            {//如果传入的参数不包括这几项，说明有问题，直接返回null

                return null;
            }
            ds_page.Tables[0].Rows[0]["search_str_where"] += " and GLJJRDLZH='" + ht_tiaojian["用户邮箱"].ToString() + "' and DLYX<>'" + ht_tiaojian["用户邮箱"].ToString() + "' ";
            //使用ht_tiaojian 传入的参数拼出需要增加的条件
            if (ht_tiaojian["交易方名称"].ToString().Trim() != "")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += " and b.I_JYFMC like '%" + ht_tiaojian["交易方名称"].ToString() + "%' ";
            }
            if (ht_tiaojian["交易方账号"].ToString().Trim() != "")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += " and a.DLYX like '%" + ht_tiaojian["交易方账号"].ToString() + "%' ";
            }
            if (ht_tiaojian["省份"].ToString().Trim() != "请选择省份")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += " and b.I_SSQYS like '%" + ht_tiaojian["省份"].ToString() + "%' ";
            }
            if (ht_tiaojian["地市"].ToString().Trim() != "请选择城市")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += " and b.I_SSQYSHI like'%" + ht_tiaojian["地市"].ToString() + "%' ";
            }           

            //调用执行方法获取数据
            object[] re_ds = IPC.Call("分页数据获取", new object[] { ds_page });
            if (re_ds[0].ToString() == "ok")
            {
                //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                ds_res = (DataSet)re_ds[1];
            }
            else
            {
                ds_res = null;
            }
            return ds_res;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 经纪人业务管理B区交易方信用等级查询中信用详情获取分页数据
    /// </summary>
    /// <param name="ds_page">大分页参数集</param>
    /// <param name="ht_tiaojian">查询条件</param>
    /// <returns></returns>
    public DataSet GetBqJYFXYXQ(DataSet ds_page, Hashtable ht_tiaojian)
    {
        try
        {
            if (!ht_tiaojian.Contains("交易方邮箱"))
            {//如果传入的参数不包括这几项，说明有问题，直接返回null

                return null;
            }
            string JYFZH = ht_tiaojian["交易方邮箱"].ToString();
            DataSet ds_res = new DataSet();//待返回的结果集  

            //设置分页功能需要的参数
            //dt_page.Rows[0]["this_dblink"] = "";//一般不用填，默认webconfig中的mainsqlserver。有特殊情况需连接别的数据库时使用，填写webconfig中添加的数据库链接的“name”值。
            // ds_page.Tables[0].Rows[0]["GetCustomersDataPage_NAME"] = " GetCustomersDataPage2 ";  //使用的存储过程名称,不设置的时候默认为GetCustomersDataPage。
            ds_page.Tables[0].Rows[0]["serach_Row_str"] = " a.Number as num,JFSX as ys,convert(numeric(18,2),FS) as fs,(select isnull(sum(convert(numeric(18,2),FS)),0) FROM AAA_JYFXYMXB as b where DLYX='" + JYFZH + "' and b.Number <=a.Number) as xyjf,a.CreateTime as cjsj "; //检索字段(必须设置)
            ds_page.Tables[0].Rows[0]["search_tbname"] = " AAA_JYFXYMXB as a ";  //检索的表(必须设置)
            ds_page.Tables[0].Rows[0]["search_mainid"] = " a.Number";  //所检索表的主键(必须设置)
            ds_page.Tables[0].Rows[0]["search_str_where"] = " DLYX='" + JYFZH + "' ";  //检索条件(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixu"] = " DESC ";  //排序方式(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixuZD"] = " a.Number ";  //用于排序的字段(必须设置)  


            if (ht_tiaojian.Contains("开始时间") && ht_tiaojian.Contains("结束时间"))
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += " and convert(varchar(10),a.CreateTime,120) between '" + ht_tiaojian["开始时间"].ToString() + "' and '" + ht_tiaojian["结束时间"].ToString() + "'";
            }
            //调用执行方法获取数据
            object[] re_ds = IPC.Call("分页数据获取", new object[] { ds_page });
            if (re_ds[0].ToString() == "ok")
            {
                //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                ds_res = (DataSet)re_ds[1];
            }
            else
            {
                ds_res = null;
            }
            return ds_res;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 经纪人业务管理B区服务人员信息维护查看员工列表
    /// </summary>
    /// <param name="ds_page">大分页参数集</param>
    /// <param name="ht_tiaojian">查询条件</param>
    /// <returns></returns>
    public DataSet GetBqBankYGLB(DataSet ds_page, Hashtable ht_tiaojian)
    {
        try
        {
            DataSet ds_res = new DataSet();//待返回的结果集  

            //设置分页功能需要的参数
            //dt_page.Rows[0]["this_dblink"] = "";//一般不用填，默认webconfig中的mainsqlserver。有特殊情况需连接别的数据库时使用，填写webconfig中添加的数据库链接的“name”值。
            // ds_page.Tables[0].Rows[0]["GetCustomersDataPage_NAME"] = " GetCustomersDataPage2 ";  //使用的存储过程名称,不设置的时候默认为GetCustomersDataPage。
            ds_page.Tables[0].Rows[0]["serach_Row_str"] = " Number as 银行人员表Number, YHDLZH as 银行登录账号,YGLSJG as 员工隶属机构,YGXM as 员工姓名,YGGH as 员工工号,LXFS as 联系方式,LXDZ  as 联系地址,SFYX as 是否有效,TJSJ as 提交时间  "; //检索字段(必须设置)
            ds_page.Tables[0].Rows[0]["search_tbname"] = " AAA_YHRYXXB ";  //检索的表(必须设置)
            ds_page.Tables[0].Rows[0]["search_mainid"] = " Number ";  //所检索表的主键(必须设置)
            ds_page.Tables[0].Rows[0]["search_str_where"] = " SFYX='是' ";  //检索条件(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixu"] = " asc ";  //排序方式(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixuZD"] = " TJSJ ";  //用于排序的字段(必须设置)  
            
            //使用ht_tiaojian 传入的参数拼出需要增加的条件
            if (!ht_tiaojian.Contains("用户邮箱"))
            {//如果传入的参数不包括这几项，说明有问题，直接返回null

                return null;
            }
            ds_page.Tables[0].Rows[0]["search_str_where"] += " and YHDLZH='" + ht_tiaojian["用户邮箱"].ToString() + "'";            

            //调用执行方法获取数据
            object[] re_ds = IPC.Call("分页数据获取", new object[] { ds_page });
            if (re_ds[0].ToString() == "ok")
            {
                //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                ds_res = (DataSet)re_ds[1];
            }
            else
            {
                ds_res = null;
            }
            return ds_res;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 开通交易账户功能中部分弹窗内的分页数据获取
    /// </summary>
    /// <param name="ds_page">分页使用参数</param>
    /// <param name="ht_tiaojian">查询条件</param>
    /// <returns></returns>
    public DataSet GetKTJYZH(DataSet ds_page, Hashtable ht_tiaojian)
    {
        DataSet dsreturn = new DataSet();
        switch (ht_tiaojian["标签名"].ToString())
        {
            case "选择高校团委":
                dsreturn = GetGXTW(ds_page, ht_tiaojian);
                break;
            case "选择银行":
                dsreturn = GetBank(ds_page, ht_tiaojian);
                break;
            case "选择银行工号":
                dsreturn = GetBankGH(ds_page, ht_tiaojian);
                break;
            case "选择地方政府":
                dsreturn = GetDFZF(ds_page, ht_tiaojian);
                break;
            case "选择行业协会":
                dsreturn = GetHYXH(ds_page, ht_tiaojian);
                break;
            default:
                dsreturn = null;
                break;
        }
        return dsreturn;
    }

    /// <summary>
    /// 开通交易账户中选择高校团委分页数据获取
    /// </summary>
    /// <param name="ds_page">大分页参数集</param>
    /// <param name="ht_tiaojian">查询条件</param>
    /// <returns></returns>
    public DataSet GetGXTW(DataSet ds_page, Hashtable ht_tiaojian)
    {
        try
        {
            DataSet ds_res = new DataSet();//待返回的结果集  

            //设置分页功能需要的参数
            //dt_page.Rows[0]["this_dblink"] = "";//一般不用填，默认webconfig中的mainsqlserver。有特殊情况需连接别的数据库时使用，填写webconfig中添加的数据库链接的“name”值。
            // ds_page.Tables[0].Rows[0]["GetCustomersDataPage_NAME"] = " GetCustomersDataPage2 ";  //使用的存储过程名称,不设置的时候默认为GetCustomersDataPage。
            ds_page.Tables[0].Rows[0]["serach_Row_str"] = " Number as 高校团委ID,GLBMZH as 高校账号,GLBMMC as 高校团委名称,CreateTime as 创建时间  "; //检索字段(必须设置)
            ds_page.Tables[0].Rows[0]["search_tbname"] = " AAA_PTGLJGB ";  //检索的表(必须设置)
            ds_page.Tables[0].Rows[0]["search_mainid"] = " Number ";  //所检索表的主键(必须设置)
            ds_page.Tables[0].Rows[0]["search_str_where"] = " GLBMFLMC='高校团委' and SFYX='是' ";  //检索条件(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixu"] = " ASC ";  //排序方式(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixuZD"] = " CreateTime ";  //用于排序的字段(必须设置)  

            //使用ht_tiaojian 传入的参数拼出需要增加的条件
            if (ht_tiaojian["高校团委名称"].ToString().Trim() != "")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += " and GLBMMC like '%" + ht_tiaojian["高校团委名称"].ToString() + "%' ";
            }

            //调用执行方法获取数据
            object[] re_ds = IPC.Call("分页数据获取", new object[] { ds_page });
            if (re_ds[0].ToString() == "ok")
            {
                //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                ds_res = (DataSet)re_ds[1];
            }
            else
            {
                ds_res = null;
            }
            return ds_res;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 开通交易账户中选择银行分页数据获取
    /// </summary>
    /// <param name="ds_page">大分页参数集</param>
    /// <param name="ht_tiaojian">查询条件</param>
    /// <returns></returns>
    public DataSet GetBank(DataSet ds_page, Hashtable ht_tiaojian)
    {
        try
        {
            DataSet ds_res = new DataSet();//待返回的结果集  

            //设置分页功能需要的参数
            //dt_page.Rows[0]["this_dblink"] = "";//一般不用填，默认webconfig中的mainsqlserver。有特殊情况需连接别的数据库时使用，填写webconfig中添加的数据库链接的“name”值。
            // ds_page.Tables[0].Rows[0]["GetCustomersDataPage_NAME"] = " GetCustomersDataPage2 ";  //使用的存储过程名称,不设置的时候默认为GetCustomersDataPage。
            ds_page.Tables[0].Rows[0]["serach_Row_str"] = " B_DLYX,I_JYFMC,J_JJRZGZSBH,I_JYFMC,I_LXRSJH,I_JJRFL, createtime "; //检索字段(必须设置)
            ds_page.Tables[0].Rows[0]["search_tbname"] = " AAA_DLZHXXB ";  //检索的表(必须设置)
            ds_page.Tables[0].Rows[0]["search_mainid"] = " number ";  //所检索表的主键(必须设置)
            ds_page.Tables[0].Rows[0]["search_str_where"] = " B_JSZHLX='经纪人交易账户' and I_JJRFL='银行' and S_SFYBFGSSHTG='是' and B_SFYZYX='是' and B_SFYXDL='是' and B_SFDJ='否' and B_SFXM='否' and  J_JJRSFZTXYHSH='否' ";  //检索条件(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixu"] = " ASC ";  //排序方式(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixuZD"] = " createtime ";  //用于排序的字段(必须设置)  
         
            //使用ht_tiaojian 传入的参数拼出需要增加的条件
            if (ht_tiaojian["银行名称"].ToString().Trim() != "")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += " and I_JYFMC like '%" + ht_tiaojian["银行名称"].ToString() + "%' ";
            }          

            //调用执行方法获取数据
            object[] re_ds = IPC.Call("分页数据获取", new object[] { ds_page });
            if (re_ds[0].ToString() == "ok")
            {
                //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                ds_res = (DataSet)re_ds[1];
            }
            else
            {
                ds_res = null;
            }
            return ds_res;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 开通交易账户中选择银行员工工号分页数据获取
    /// </summary>
    /// <param name="ds_page">大分页参数集</param>
    /// <param name="ht_tiaojian">查询条件</param>
    /// <returns></returns>
    public DataSet GetBankGH(DataSet ds_page, Hashtable ht_tiaojian)
    {
        try
        {
            DataSet ds_res = new DataSet();//待返回的结果集  

            //设置分页功能需要的参数
            //dt_page.Rows[0]["this_dblink"] = "";//一般不用填，默认webconfig中的mainsqlserver。有特殊情况需连接别的数据库时使用，填写webconfig中添加的数据库链接的“name”值。
            // ds_page.Tables[0].Rows[0]["GetCustomersDataPage_NAME"] = " GetCustomersDataPage2 ";  //使用的存储过程名称,不设置的时候默认为GetCustomersDataPage。
            ds_page.Tables[0].Rows[0]["serach_Row_str"] = " YGLSJG,YGXM,YGGH  "; //检索字段(必须设置)
            ds_page.Tables[0].Rows[0]["search_tbname"] = " AAA_YHRYXXB ";  //检索的表(必须设置)
            ds_page.Tables[0].Rows[0]["search_mainid"] = " number ";  //所检索表的主键(必须设置)
            ds_page.Tables[0].Rows[0]["search_str_where"] = " SFYX='是' ";  //检索条件(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixu"] = " ASC ";  //排序方式(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixuZD"] = " CreateTime ";  //用于排序的字段(必须设置) 
            
            
            //使用ht_tiaojian 传入的参数拼出需要增加的条件
            if (!ht_tiaojian.Contains("银行登陆账号"))
            {
                return null;
            }
            ds_page.Tables[0].Rows[0]["search_str_where"] += " and YHDLZH='" + ht_tiaojian["银行登陆账号"].ToString() + "' ";
          
            if (ht_tiaojian["分支机构"].ToString().Trim() != "")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += " and YGLSJG like '%" + ht_tiaojian["分支机构"].ToString() + "%' ";
            }
            if (ht_tiaojian["员工工号"].ToString().Trim() != "")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += " and YGGH like '%" + ht_tiaojian["员工工号"].ToString() + "%' ";
            }
            if (ht_tiaojian["员工姓名"].ToString().Trim() != "")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += " and YGXM like '%" + ht_tiaojian["员工姓名"].ToString() + "%' ";
            }

            //调用执行方法获取数据
            object[] re_ds = IPC.Call("分页数据获取", new object[] { ds_page });
            if (re_ds[0].ToString() == "ok")
            {
                //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                ds_res = (DataSet)re_ds[1];
            }
            else
            {
                ds_res = null;
            }
            return ds_res;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 开通交易账户中选择地方政府分页数据获取
    /// </summary>
    /// <param name="ds_page">大分页参数集</param>
    /// <param name="ht_tiaojian">查询条件</param>
    /// <returns></returns>
    public DataSet GetDFZF(DataSet ds_page, Hashtable ht_tiaojian)
    {
        try
        {
            DataSet ds_res = new DataSet();//待返回的结果集  

            //设置分页功能需要的参数
            //dt_page.Rows[0]["this_dblink"] = "";//一般不用填，默认webconfig中的mainsqlserver。有特殊情况需连接别的数据库时使用，填写webconfig中添加的数据库链接的“name”值。
            // ds_page.Tables[0].Rows[0]["GetCustomersDataPage_NAME"] = " GetCustomersDataPage2 ";  //使用的存储过程名称,不设置的时候默认为GetCustomersDataPage。
            ds_page.Tables[0].Rows[0]["serach_Row_str"] = " * "; //检索字段(必须设置)
            ds_page.Tables[0].Rows[0]["search_tbname"] = " (select '' '政府ID','' '政府账号','' '政府名称','' '创建时间' from AAA_PTGLJGB where 1!=1) as Tab";  //检索的表(必须设置)
            ds_page.Tables[0].Rows[0]["search_mainid"] = " 政府ID ";  //所检索表的主键(必须设置)
            ds_page.Tables[0].Rows[0]["search_str_where"] = " 1=1 ";  //检索条件(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixu"] = " ASC ";  //排序方式(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixuZD"] = " 创建时间 ";  //用于排序的字段(必须设置) 


            //使用ht_tiaojian 传入的参数拼出需要增加的条件          
            if (ht_tiaojian["政府名称"].ToString().Trim() != "")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += " and 政府名称 like '%" + ht_tiaojian["政府名称"].ToString() + "%' ";
            }           

            //调用执行方法获取数据
            object[] re_ds = IPC.Call("分页数据获取", new object[] { ds_page });
            if (re_ds[0].ToString() == "ok")
            {
                //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                ds_res = (DataSet)re_ds[1];
            }
            else
            {
                ds_res = null;
            }
            return ds_res;
        }
        catch
        {
            return null;
        }
    }


    /// <summary>
    /// 开通交易账户中选择行业协会分页数据获取
    /// </summary>
    /// <param name="ds_page">大分页参数集</param>
    /// <param name="ht_tiaojian">查询条件</param>
    /// <returns></returns>
    public DataSet GetHYXH(DataSet ds_page, Hashtable ht_tiaojian)
    {
        try
        {
            DataSet ds_res = new DataSet();//待返回的结果集  

            //设置分页功能需要的参数
            //dt_page.Rows[0]["this_dblink"] = "";//一般不用填，默认webconfig中的mainsqlserver。有特殊情况需连接别的数据库时使用，填写webconfig中添加的数据库链接的“name”值。
            // ds_page.Tables[0].Rows[0]["GetCustomersDataPage_NAME"] = " GetCustomersDataPage2 ";  //使用的存储过程名称,不设置的时候默认为GetCustomersDataPage。
            ds_page.Tables[0].Rows[0]["serach_Row_str"] = " * "; //检索字段(必须设置)
            ds_page.Tables[0].Rows[0]["search_tbname"] = " (select '' '行业协会ID','' '行业协会账号','' '行业协会名称','' '创建时间' from AAA_PTGLJGB where GLBMFLMC='高校团委' and 1!=1) as Tab ";  //检索的表(必须设置)
            ds_page.Tables[0].Rows[0]["search_mainid"] = " 行业协会ID ";  //所检索表的主键(必须设置)
            ds_page.Tables[0].Rows[0]["search_str_where"] = " 1=1 ";  //检索条件(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixu"] = " ASC ";  //排序方式(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixuZD"] = " 创建时间 ";  //用于排序的字段(必须设置) 


            //使用ht_tiaojian 传入的参数拼出需要增加的条件          
            if (ht_tiaojian["行业协会名称"].ToString().Trim() != "")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += " and 行业协会名称 like '%" + ht_tiaojian["行业协会名称"].ToString() + "%' ";
            }

            //调用执行方法获取数据
            object[] re_ds = IPC.Call("分页数据获取", new object[] { ds_page });
            if (re_ds[0].ToString() == "ok")
            {
                //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                ds_res = (DataSet)re_ds[1];
            }
            else
            {
                ds_res = null;
            }
            return ds_res;
        }
        catch
        {
            return null;
        }
    }


    /// <summary>
    /// 账户维护B区各页面分页数据
    /// </summary>
    /// <param name="ds_page">分页使用参数</param>
    /// <param name="ht_tiaojian">查询条件</param>
    /// <returns></returns>
    public DataSet GetBqZHWH(DataSet ds_page, Hashtable ht_tiaojian)
    {
        DataSet dsreturn = new DataSet();
        switch (ht_tiaojian["标签名"].ToString())
        {
            case "选择经纪人审核通过记录":
                dsreturn = GetBqXZJJRSHTGJL(ds_page, ht_tiaojian);
                break;         
            default:
                dsreturn = null;
                break;
        }
        return dsreturn;
    }

    /// <summary>
    /// 账户维护B区选择经纪人页面中的查看选择经纪人审核通过记录弹窗内分页数据
    /// </summary>
    /// <param name="ds_page">大分页参数集</param>
    /// <param name="ht_tiaojian">查询条件</param>
    /// <returns></returns>
    public DataSet GetBqXZJJRSHTGJL(DataSet ds_page, Hashtable ht_tiaojian)
    {
        try
        {
            DataSet ds_res = new DataSet();//待返回的结果集  

            //设置分页功能需要的参数
            //dt_page.Rows[0]["this_dblink"] = "";//一般不用填，默认webconfig中的mainsqlserver。有特殊情况需连接别的数据库时使用，填写webconfig中添加的数据库链接的“name”值。
            // ds_page.Tables[0].Rows[0]["GetCustomersDataPage_NAME"] = " GetCustomersDataPage2 ";  //使用的存储过程名称,不设置的时候默认为GetCustomersDataPage。
            ds_page.Tables[0].Rows[0]["serach_Row_str"] = " a.Number,convert(varchar(20),a.SQSJ,120) as 选择经纪人时间,b.I_JYFMC as 经纪人名称,convert(varchar(20),a.JJRSHSJ,120) as 审核时间,a.JJRSHZT as 审核状态,a.JJRSHYJ as 审核意见,b.B_SFDJ as 是否冻结,b.B_SFXM as 是否休眠,b.J_JJRSFZTXYHSH as 经纪人是否暂停新用户审核,'' as 经纪人状态 "; //检索字段(必须设置)
            ds_page.Tables[0].Rows[0]["search_tbname"] = " AAA_MJMJJYZHYJJRZHGLB as a inner join AAA_DLZHXXB as b on a.GLJJRBH=b.J_JJRJSBH ";  //检索的表(必须设置)
            ds_page.Tables[0].Rows[0]["search_mainid"] = " a.Number ";  //所检索表的主键(必须设置)
            ds_page.Tables[0].Rows[0]["search_str_where"] = " a.JSZHLX='买家卖家交易账户'  and a.SFYX='是'  and a.SFSCGLJJR='否' and a.JJRSHZT='审核通过' ";  //检索条件(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixu"] = " DESC ";  //排序方式(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixuZD"] = " a.JJRSHSJ ";  //用于排序的字段(必须设置)  
            
            //使用ht_tiaojian 传入的参数拼出需要增加的条件
            if (!ht_tiaojian.Contains("用户邮箱"))
            {//如果传入的参数不包括这几项，说明有问题，直接返回null

                return null;
            }
            ds_page.Tables[0].Rows[0]["search_str_where"] += " and a.DLYX='" + ht_tiaojian["用户邮箱"].ToString() + "' ";    

            //调用执行方法获取数据
            object[] re_ds = IPC.Call("分页数据获取", new object[] { ds_page });
            if (re_ds[0].ToString() == "ok")
            {
                //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                ds_res = (DataSet)re_ds[1];
            }
            else
            {
                ds_res = null;
            }
            return ds_res;
        }
        catch
        {
            return null;
        }
    }
}