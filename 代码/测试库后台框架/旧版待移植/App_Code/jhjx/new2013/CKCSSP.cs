using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using FMOP.DB;

/// <summary>
/// CKCSSP 的摘要说明
/// </summary>
public class CKCSSP
{
	public CKCSSP()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}
    public DataSet Select_CSSP(DataTable CS, DataSet returnDS)
    { 
        returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
        returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "查询成功！";
        try
        {
            if (CS != null && CS.Rows.Count > 0)
            {
                string DLYX = CS.Rows[0]["DLYX"].ToString();
                string SPBH = CS.Rows[0]["SPBH"].ToString();
                string Number = CS.Rows[0]["Number"].ToString();
                string str = "select a.SPBH 商品编号,c.SPMC 商品名称,c.GG 执行标准,c.JJDW 计价单位,c.JJPL 平台设定最大经济批量,c.SPMS 商品描述,a.DLYX 交易方账号,b.I_JYFMC 交易方名称,b.I_LXRXM 联系人姓名,b.I_LXRSJH 联系人手机号,b.I_JYFLXDH 交易方联系电话,b.I_YBNSRZGZSMJ 一般人纳税人资格证明,a.ZLBZYZM 质量标准与证明,a.CPJCBG 产品检测报告,a.PGZFZRFLCNS 品管总负责人法律承诺书,a.FDDBRCNS 法定代表人承诺书,a.SHFWGDYCN 售后服务规定与承诺,a.CPSJSQS 产品送检授权书,c.SCZZYQ 特殊资质,a.ZZ1,a.ZZ2,a.ZZ3,a.ZZ4,a.ZZ5,a.ZZ6,a.ZZ7,a.ZZ8,a.ZZ9,a.ZZ10,a.SHYJ 审核意见,b.J_SELJSBH 卖家角色编号 from AAA_CSSPB a left join AAA_DLZHXXB b on a.DLYX=b.B_DLYX left join AAA_PTSPXXB c on a.SPBH=c.SPBH where a.DLYX='" + DLYX.Trim() + "' and a.SPBH='" + SPBH.Trim() + "' and a.Number='" + Number.Trim() + "'";
                DataSet ds = DbHelperSQL.Query(str);
                DataTable dt = ds.Tables[0].Copy();
                dt.TableName = "主表";
                returnDS.Tables.Add(dt);
                return returnDS;
            }
            else
            {
                returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "传递的参数不能为空！";  
            }           

        }
        catch(Exception e)
        {
            returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            //returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "数据操作失败，失败原因：" + e.Message;   
            returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "操作失败！";   
        }
        return returnDS;
    }
}