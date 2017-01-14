using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Collections;
using FMOP.DB;

/// <summary>
///UpdateTBXX 的摘要说明
/// </summary>
public class UpdateTBXX
{
	public UpdateTBXX()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}
    /// <summary>
    /// 投标信息表，中标前的修改、撤销投标信息
    /// </summary>
    ///  <param name="dt">修改的数据集，必须有的字段"修改的数量"、"修改的价格"、"投标信息号"、"子表id"、"商品编号"、"合同周期"</param>
    /// <param name="cz">修改、撤销</param>
    /// <returns></returns>
    public static string BegionUpdateTBXX(DataTable dt, string cz)
    {
        if (dt != null && dt.Rows.Count > 0)
        {
            foreach (DataRow row in dt.Rows)
            {
                string Number = row["投标信息号"].ToString();
                string updateJG = row["修改的价格"].ToString();
                int updateSL = Convert.ToInt32(row["修改的数量"].ToString());
                string id = row["子表id"].ToString();
                #region
                DataSet dsTBXX =jhjx_PublicClass.GetTBXX(Number, "竞标");
                string tbjg = updateJG.Trim() == "" ? "0" : updateJG.Trim();//修改的拟买入价格    
                if (dsTBXX != null && dsTBXX.Tables[0].Rows.Count > 0)
                {
                    switch (cz.Trim())
                    {
                        case "修改":
                            DataRow[] tbrow = dsTBXX.Tables[0].Select("1=1 and id='" + id.Trim() + "'");
                            switch (tbrow[28].ToString().Trim())//是否进入冷静期的字段
                            {
                                case "是":
                                    return "该类商品投标在冷静期内，不能进行修改";
                                default:
                                    int xgcs = Convert.ToInt16(tbrow[29].ToString().Trim() == "" ? "0" : tbrow[29].ToString().Trim());//已修改过的次数
                                    if (xgcs > 0)
                                    {
                                        return "您已修改过，不能再进行修改";
                                    }
                                    else
                                    {
                                        if (updateSL > 0)
                                        {
                                            xgcs += 1;
                                            string strSQL = "update ZZ_TBXXBZB set TBJG='" + tbjg + "',TBYSL=" + updateSL + ",XGCS=" + xgcs + " where id='" + id.Trim() + "' and parentNumber='" + Number.Trim() + "'";
                                            int i = DbHelperSQL.ExecuteSql(strSQL);
                                            if (i > 0)
                                            {
                                                return "修改成功";
                                            }
                                            else
                                            {
                                                return "修改失败";
                                            }
                                        }
                                        else
                                        {
                                            return "修改后的数量必须大于零";
                                        }
                                    }
                            }
                        case "撤销":
                            tbrow = dsTBXX.Tables[0].Select("1=1 and id='" + id.Trim() + "'");
                            switch (tbrow[12].ToString().Trim())//是否进入冷静期的字段
                            {
                                case "是":
                                    return "该类商品投标在冷静期内，不能进行修改";
                                default:
                                    if (dsTBXX.Tables[0].Rows.Count > 1)
                                    {
                                        string strSQL = "update ZZ_TBXXBZB set ZT='撤销' where id='" + id.Trim() + "'";
                                        int i = DbHelperSQL.ExecuteSql(strSQL);
                                        if (i > 0)
                                        {
                                            return "撤销成功";
                                        }
                                        else
                                        {
                                            return "撤销失败";
                                        }
                                    }
                                    else
                                    {
                                        ArrayList list = new ArrayList();
                                        string strSQL = "update ZZ_TBXXBZB set ZT='撤销' where id='" + id.Trim() + "'";
                                        list.Add(strSQL);
                                        strSQL = "update dbo.ZZ_TBXXB set SFCX='是' where number='" + Number.Trim() + "'";
                                        bool bl = DbHelperSQL.ExecSqlTran(list);
                                        if (bl)
                                        {
                                            return "撤销成功";
                                        }
                                        else
                                        {
                                            return "撤销失败";
                                        }
                                    };
                            }
                        default:
                            return "操作不正确";
                    }
                }
                else
                {
                    return "未找到该投标信息";
                }
                #endregion

            }
            return "数据集不能为空";
        }
        else
        {
            return "数据集不能为空";
        }
    }
}