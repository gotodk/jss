using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using FMOP.DB;

/// <summary>
/// YCTBD 的摘要说明
/// </summary>
public class YCTBD
{
	public YCTBD()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}
    //查询投标单的异常资质
    public DataSet CXTBDYCZZ(DataTable ds, DataSet returnDS)
    { 
        returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
        returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "查询成功！";
        try
        {
            DataTable table = ds;
            if (table != null && table.Rows.Count > 0)
            {
                string DLYX = table.Rows[0]["DLYX"].ToString();
                string TBDH = table.Rows[0]["TBDH"].ToString();
                string str = "select * from AAA_TBZLSHB a left join AAA_TBD b on a.TBDH=b.Number where DLYX='" + DLYX.Trim() + "' and a.TBDH='" + TBDH.Trim() + "' ";
                DataSet dsZB= DbHelperSQL.Query(str);                
                DataTable dtZB = dsZB.Tables[0].Copy();
                dtZB.TableName = "主表";
                returnDS.Tables.Add(dtZB);

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
            returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = e.Message;
        }
        return returnDS;
        
    }

    public DataSet Update(DataSet ds, DataSet returnDS)
    { 
        returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
        returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "查询成功！";


        Hashtable htUserVal = PublicClass2013.GetUserInfo(ds.Tables["投标单号表"].Rows[0]["买家角色编号"].ToString());//通过买家角色编号获取信息

        if (htUserVal["是否休眠"].ToString().Trim() == "是")
        {
 
            returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于休眠状态，请进入账户维护中激活休眠账户！";
            return returnDS;
        }


        try
        {
            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable tblGDZZ = ds.Tables["固定资质表"];
                DataTable tblTSZZ = ds.Tables["特殊资质表"];
                DataTable tblTBDH = ds.Tables["投标单号表"];
                if (tblTBDH != null && tblTBDH.Rows.Count > 0)
                {
                    string TBDH = tblTBDH.Rows[0]["TBDH"].ToString();
                    #region//验证交易管理部审核状态
                    string strTBDZZB = "select JYGLBSHZT from AAA_TBZLSHB where TBDH='" + TBDH.Trim() + "'";
                    object shzt = DbHelperSQL.GetSingle(strTBDZZB);
                    if (shzt != null && shzt.ToString() != "")
                    {
                        if (shzt.ToString() == "审核通过")
                        {
                            returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "该异常投标单已审核通过，不能进行修改操作！";
                            return returnDS;
                        }
                    }
                    else
                    {
                        returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "未查询到该异常投标单的信息！";
                        return returnDS;
                    }
                    #endregion


                    ArrayList list = new ArrayList();
                    string strTBD = "update AAA_TBD set ";
                    #region//固定资质
                    if(tblGDZZ.Rows.Count>0)
                    {
                        foreach(DataRow row in tblGDZZ.Rows)
                        {
                            strTBD += row["Name"].ToString() + "='" + row["Value"].ToString() + "',";
                        }
                    }
                    #endregion
                    #region//特殊资质
                    if(tblTSZZ.Rows.Count>0)
                    {
                        string tszz = "|";
                        foreach(DataRow row in tblTSZZ.Rows)
                        {
                            tszz += row["Name"].ToString() + "*" + row["Value"].ToString() + "|"; 
                        }
                        strTBD += " ZZ01 ='" + tszz+"',";
                    }
                    strTBD = strTBD.TrimEnd(',');
                    strTBD += " where Number='" + TBDH.Trim() + "'";
                    if (tblGDZZ.Rows.Count > 0 || tblTSZZ.Rows.Count > 0)
                    {
                        list.Add(strTBD);
                    }                    
                    #endregion
                    #region//更新投标资料审核表
                    string str = "update AAA_TBZLSHB set FWZXSHYCHSFXG='是',JYGLBSHWTGHSFXG='是',MJZXXGSJ=getdate() where TBDH='" + TBDH.Trim() + "'";
                    list.Add(str);
                    #endregion

                    if (DbHelperSQL.ExecSqlTran(list))
                    {
                        returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                        returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "确认修改成功！";
                    }
                    else
                    {
                        returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "确认修改失败！";
                    }
                }
                else
                {
                    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "传递的参数不能为空";
                }
            }
            else
            {
                returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "传递的参数不能为空";
            }
        }
        catch (Exception e)
        {
            returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = e.Message;
        }
        return returnDS;
    }
}