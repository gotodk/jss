using FMOP.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// jhjx_yhdongjie 的摘要说明
/// </summary>
public class jhjx_yhdongjie
{
	public jhjx_yhdongjie()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}
    /// <summary>
    /// 获取用户冻结信息
    /// </summary>
    /// <param name="YHinfo">用户登录邮箱或者用户角色编号</param>
    /// <param name="DJgn">需验证功能</param>
    /// <returns></returns>
    public static DataSet getYHdjinfo(string YHinfo, string DJgn)
    {

        DataSet dsre = new DataSet();
        DataTable auto2 = new DataTable();
        auto2.TableName = "返回值单条";
        auto2.Columns.Add("执行结果");
        auto2.Columns.Add("提示文本");
        
     
        DataRow dr = auto2.NewRow();
        dr["执行结果"] = "err";
        dr["提示文本"] = "没有查找到任何“" + YHinfo + "”的信息！";
        auto2.Rows.Add(dr);
         dsre.Tables.Add(auto2);
     
        string Strsql = " select B_DLYX as '登陆邮箱' , isnull(B_SFDJ,'否') as '是否冻结',DJGNX as '冻结功能' from    dbo.AAA_DLZHXXB where (B_DLYX = '" + YHinfo + "' or J_SELJSBH='" + YHinfo + "' or J_BUYJSBH='" + YHinfo + "' or J_JJRJSBH='" + YHinfo + "' )   ";//and DJGNX like '%" + DJgn + "%'

        DataSet ds   = DbHelperSQL.Query(Strsql);
        dsre.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
        dsre.Tables["返回值单条"].Rows[0]["提示文本"] = "被冻结功能：" + "没有查找到任何“" + YHinfo + "”的信息！";

        if (ds != null && ds.Tables[0].Rows.Count != 1)
        {
            dsre.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsre.Tables["返回值单条"].Rows[0]["提示文本"] = "被冻结功能：" + "没有查找到任何“" + YHinfo + "”的信息！";
            //return dsreturn;
        }
        else
        {
            if (ds.Tables[0].Rows[0]["是否冻结"].ToString().Equals("是"))
            {
                if (ds.Tables[0].Rows[0]["冻结功能"].ToString().Contains(DJgn))
                {
                    dsre.Tables["返回值单条"].Rows[0]["执行结果"] = "是";//ds.Tables[0].Rows[0]["是否冻结"].ToString();
                    dsre.Tables["返回值单条"].Rows[0]["提示文本"] = "被冻结功能：" +ds.Tables[0].Rows[0]["冻结功能"].ToString();

                }
                else
                {
                    dsre.Tables["返回值单条"].Rows[0]["执行结果"] = "否";//ds.Tables[0].Rows[0]["是否冻结"].ToString();
                    dsre.Tables["返回值单条"].Rows[0]["提示文本"] = "被冻结功能：" + ds.Tables[0].Rows[0]["冻结功能"].ToString();
                }
            }
            else
            {
                dsre.Tables["返回值单条"].Rows[0]["执行结果"] = "否";//ds.Tables[0].Rows[0]["是否冻结"].ToString();
                dsre.Tables["返回值单条"].Rows[0]["提示文本"] = "被冻结功能：" + ds.Tables[0].Rows[0]["冻结功能"].ToString(); 
            }
        }

        return dsre;
    }
}