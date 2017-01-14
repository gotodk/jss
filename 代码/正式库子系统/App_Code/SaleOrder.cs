using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using FMOP.SD;
using FMOP.DB;
using FMOP.XParse;
/// <summary>
/// SaleOrder 的摘要说明
/// </summary>
public class SaleOrder
{
	public SaleOrder()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}

    /// <summary>
    /// 查询计划内本月产品数量
    /// </summary>
    /// <returns></returns>
    public DataSet getPlan_bosomSum(string KHBH, string WLBM, string currentlyDate)
    {
        String Month = setNumber(Convert.ToDateTime(currentlyDate).Month.ToString());
        DataSet result = null;

        String cmdText = @" SELECT id,SLDW,YXSL FROM CSZX_YDCPJYJHB_JH INNER JOIN CSZX_YDCPJYJHB ON CSZX_YDCPJYJHB_JH.parentNumber= CSZX_YDCPJYJHB.Number
                            WHERE CSZX_YDCPJYJHB.NF='" + Convert.ToDateTime(currentlyDate).Year + "' AND CSZX_YDCPJYJHB.YF='" + Month + "' AND CSZX_YDCPJYJHB_JH.KHBH='" + KHBH + "' AND CSZX_YDCPJYJHB_JH.WLBM='" + WLBM + "'";
        try
        {
            result = DbHelperSQL.Query(cmdText);
        }
        catch
        {
            result = null;
        }

        return result;
    }

    /// <summary>
    /// 查询计划外本月产品数量
    /// </summary>
    /// <param name="KHBH"></param>
    /// <returns></returns>
    public DataSet getPlan_besidesSum(string KHBH, string WLBM, string currentlyDate)
    {
        DataSet result = null;

        String cmdText = @"SELECT id,ZXYWJHWCPXQJHB.SL,ZXYWJHWCPXQJHB.YXSL 
                           FROM ZXYWJHWCPXQJHB  INNER JOIN SCJHB_ZXYWJHWCPXQJHB ON ZXYWJHWCPXQJHB.parentNumber = SCJHB_ZXYWJHWCPXQJHB.Number
                           WHERE  SCJHB_ZXYWJHWCPXQJHB.KHBH='" + KHBH + "' AND CONVERT(char(7),SCJHB_ZXYWJHWCPXQJHB.createtime,20) = '" + Convert.ToDateTime(currentlyDate).ToString("yyyy-MM") + "' AND ZXYWJHWCPXQJHB.WLBM='" + WLBM + "'";
        try
        {
            result = DbHelperSQL.Query(cmdText);
        }
        catch
        {
            result = null;
        }

        return result;
    }

    /// <summary>
    /// 取客户编号
    /// </summary>
    /// <returns></returns>
    public string getClientNo(SaveDepositary[] MasterNodeArray, string name)
    {
        int index = 0;
        string resultValue = "";

        for (; index < MasterNodeArray.Length; index++)
        {
            if (MasterNodeArray[index].Name == name)
            {
                resultValue = MasterNodeArray[index].Text;
            }
        }

        return resultValue;
    }

    /// <summary>
    /// 计划内更新已销数量
    /// </summary>
    /// <param name="saveToDb">事务泛型</param>
    /// <param name="bosomSum">计划内的数量</param>
    /// <param name="saleOrderSum">销售单的数量</param>
    /// <returns></returns>
    public List<CommandInfo> UpdatePlan_bosomSum(List<CommandInfo> saveToDb, string keys, int sum)
    {

        CommandInfo comInfo = new CommandInfo();
        if (keys != "")
        {
            string cmdText = "update CSZX_YDCPJYJHB_JH set YXSL=@YXSL where id=@Number";
            SqlParameter[] ParamArray = new SqlParameter[2];
            ParamArray[0] = new SqlParameter("@Number", SqlDbType.Int, 6);
            ParamArray[0].Value = Convert.ToInt32(keys);

            ParamArray[1] = new SqlParameter("@YXSL", SqlDbType.Int, 6);
            ParamArray[1].Value = sum;

            comInfo = MakeObj(cmdText, ParamArray, CommandType.Text);
            saveToDb.Add(comInfo);
        }
        return saveToDb;
    }

    /// <summary>
    /// 计划外更新已销数量
    /// </summary>
    /// <param name="saveToDb">事务泛型</param>
    /// <param name="besidesSum">计划外产生的数量</param>
    /// <param name="saleOrderSum">销售单的数量</param>
    /// <returns></returns>
    public List<CommandInfo> UpdatePlan_besidesSum(List<CommandInfo> saveToDb, string keys, int sum)
    {
        CommandInfo comInfo = new CommandInfo();

        if (keys != "")
        {
            string cmdText = "update ZXYWJHWCPXQJHB set YXSL=@YXSL where id=@Number";
            SqlParameter[] ParamArray = new SqlParameter[2];
            ParamArray[0] = new SqlParameter("@Number", SqlDbType.Int, 6);
            ParamArray[0].Value = Convert.ToInt32(keys);

            ParamArray[1] = new SqlParameter("@YXSL", SqlDbType.Int, 6);
            ParamArray[1].Value = sum;

            comInfo = MakeObj(cmdText, ParamArray, CommandType.Text);
            saveToDb.Add(comInfo);
        }
        return saveToDb;
    }

    /// <summary>
    /// 创建泛型对象
    /// </summary>
    /// <param name="strSql"></param>
    /// <param name="parameterArray"></param>
    /// <param name="cmdType"></param>
    /// <returns></returns>
    public static CommandInfo MakeObj(string strSql, SqlParameter[] parameterArray, CommandType cmdType)
    {
        CommandInfo commInfo = new CommandInfo();
        commInfo.CommandText = strSql;
        commInfo.Parameters = parameterArray;
        commInfo.cmdType = cmdType;
        return commInfo;
    }

    /// <summary>
    /// 设置月份为两位数字
    /// </summary>
    /// <param name="number">1-9之间的数字</param>
    /// <returns></returns>
    private string setNumber(string number)
    {
        if (Int32.Parse(number) > 0 && Int32.Parse(number) < 10)
        {
            number = "0" + number;
        }

        return number;
    }
}
