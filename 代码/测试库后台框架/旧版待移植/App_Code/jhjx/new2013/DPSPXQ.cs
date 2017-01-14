using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using FMOP.DB;

/// <summary>
/// DPSPXQ 的摘要说明
/// </summary>
public class DPSPXQ
{
	public DPSPXQ()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}
    public DataSet SelectSPXQ(DataTable ds, DataSet returnDS)
    { 
        returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
        returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "查询成功！";
        try
        {
            if (ds != null && ds.Rows.Count > 0)
            {
                string SPBH = ds.Rows[0]["商品编号"].ToString();
                string HTQX = ds.Rows[0]["合同期限"].ToString();

                #region//商品的历史价格走势
                string strSQL = "select top 100 isnull(isnull(即时竞标时间,三个月竞标时间),一年竞标时间) 时间,即时交易,三个月合同,一年合同 from (select Z_ZBSJ as 即时竞标时间,Z_ZBJG 即时交易 from AAA_ZBDBXXB where Z_SPBH='" + SPBH.Trim() + "' and Z_HTQX = '即时' group by Z_HTQX,Z_SPBH,Z_SPMC,Z_ZBJG,Z_ZBSJ) a full join (select Z_ZBSJ 三个月竞标时间,Z_ZBJG 三个月合同 from AAA_ZBDBXXB where Z_SPBH='" + SPBH.Trim() + "' and Z_HTQX = '三个月' group by Z_HTQX,Z_SPBH,Z_SPMC,Z_ZBJG,Z_ZBSJ) b on a.即时竞标时间=b.三个月竞标时间 full join (select Z_ZBSJ  一年竞标时间,Z_ZBJG 一年合同 from AAA_ZBDBXXB where Z_SPBH='" + SPBH.Trim() + "' and Z_HTQX = '一年' group by Z_HTQX,Z_SPBH,Z_SPMC,Z_ZBJG,Z_ZBSJ) c on a.即时竞标时间=c.一年竞标时间 order by 时间;";
                #endregion

                //商品详情
                string strSPXQ = "select * from AAA_DaPanPrd_New where 商品编号='" + SPBH.Trim() + "' and 合同周期n='" + HTQX.Trim() + "' ;";

                //卖家信息
                string strSelXX = "select top 1 GHQY as 供货区域,DLYX,SLZM as '税率证明','卖出方名称'=(select I_JYFMC from AAA_DLZHXXB b where b.B_DLYX =a.DLYX),'账户当前信用分值'=(select isnull(B_ZHDQXYFZ,0) from AAA_DLZHXXB b where b.B_DLYX =a.DLYX) from AAA_TBD a where ZT='竞标' and SPBH='" + SPBH.Trim() + "' and HTQX='" + HTQX.Trim() + "' order by TBJG,TJSJ;";

                //商品描述
                string strSPMS = "select SPMS 商品描述 from AAA_PTSPXXB where SPBH='" + SPBH.Trim() + "';";

                string str = strSQL + strSPXQ + strSelXX + strSPMS;
                DataSet tble = DbHelperSQL.Query(str);

                DataTable dtJGZS = tble.Tables[0].Copy();
                dtJGZS.TableName = "价格走势";
                returnDS.Tables.Add(dtJGZS);

                DataTable dtSPXQ = tble.Tables[1].Copy();
                dtSPXQ.TableName = "商品详情";
                returnDS.Tables.Add(dtSPXQ);

                DataTable dtSelXX = tble.Tables[2].Copy();
                dtSelXX.TableName = "卖家信息";
                returnDS.Tables.Add(dtSelXX);

                DataTable dsSPMS = tble.Tables[3].Copy();
                dsSPMS.TableName = "商品描述";
                returnDS.Tables.Add(dsSPMS);

                //不供货区域
                string strSQLBGHQY = "";
                if (dtSelXX != null && dtSelXX.Rows.Count > 0)
                {
                    string strGHQY = dtSelXX.Rows[0]["供货区域"].ToString().Replace("|", "','");
                    strGHQY = strGHQY.Substring(2, strGHQY.Length - 4);
                    strSQLBGHQY = "select p_namestr from AAA_CityList_Promary where  p_namestr not in (" + strGHQY + ");";
                }
                else
                {
                    strSQLBGHQY = "select p_namestr from AAA_CityList_Promary;";
                }
                DataTable dsBGHQY = DbHelperSQL.Query(strSQLBGHQY).Tables[0].Copy();
                dsBGHQY.TableName = "不供货区域";
                returnDS.Tables.Add(dsBGHQY);
            }
            else
            {
                returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "传递的参数不能为空！";
                
            }
        }
        catch (Exception e)
        {
            returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "操作失败！";   
            
        }


        return returnDS;
    }

}