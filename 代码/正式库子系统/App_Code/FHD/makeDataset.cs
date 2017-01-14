using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using FMOP.DB;
using System.Text;
using Hesion.Brick.Core;
using Hesion.Brick.Core.WorkFlow;
using System.Collections;
/// <summary>
/// makeDataset 的摘要说明
/// </summary>
public class makeDataset
{
   public DataTable dt;
    public string Number;
	public makeDataset()
	{
        maketable();
	}
    /// <summary>
    /// 临时表结构
    /// </summary>
    private void maketable()
    {
        DataTable mtable = new DataTable();
        DataColumn column_Number = new DataColumn("Number", typeof(string));
        mtable.Columns.Add(column_Number);
        DataColumn column_zNumber=new DataColumn ("zNumber",typeof(string));
        mtable.Columns.Add(column_zNumber);
        DataColumn column_YHSQDH=new DataColumn ("YHSQDH",typeof(string));
        mtable.Columns.Add(column_YHSQDH);
        DataColumn column_XSDH =new DataColumn ("XSDH",typeof(string));
        mtable.Columns.Add(column_XSDH);
        DataColumn column_CPBM=new DataColumn ("CPBM",typeof(string));
        mtable.Columns.Add(column_CPBM);
        DataColumn column_CPPM=new DataColumn ("CPPM",typeof(string));
        mtable.Columns.Add(column_CPPM);
        DataColumn column_CPXH=new DataColumn ("CPXH",typeof(string));
        mtable.Columns.Add(column_CPXH);
        DataColumn column_DW=new DataColumn ("DW",typeof(string ));
        mtable.Columns.Add(column_DW);
        DataColumn column_FHSL =new DataColumn ("FHSL",typeof(double));
        mtable.Columns.Add(column_FHSL);
        DataColumn column_XSSL =new DataColumn ("XSSL",typeof(double));
        mtable.Columns.Add(column_XSSL);
        DataColumn column_BZDW = new DataColumn("BZDW", typeof(string));
        mtable.Columns.Add(column_BZDW);
        DataColumn column_BZSL = new DataColumn("BZSL", typeof(string));
        mtable.Columns.Add(column_BZSL);
        DataColumn column_BZ = new DataColumn("BZ", typeof(string));
        mtable.Columns.Add(column_BZ);
        DataColumn column_type = new DataColumn("TYPE", typeof(string));
        mtable.Columns.Add(column_type);
        
        dt = mtable;
    }
   
    /// <summary>
    /// 添加要货产品
    /// </summary>
    /// <param name="strnumber">要货编号</param>
    /// <param name="type">0:要货单1:销售单</param>
    /// <returns></returns>
    public DataSet addyhRow(string strnumber, string type)
    {
        ArrayList yhid = new ArrayList();
        DataSet dss = new DataSet();
        string sql = "select * from CSKHFWZX_YHSQD_YHLB WHERE FHZT=0 AND  parentNumber in( " + strnumber + ")";
        DataSet yhset = DbHelperSQL.Query(sql);
        if (yhset.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < yhset.Tables[0].Rows.Count; i++)
            {
                dt.Rows.Add("", yhset.Tables[0].Rows[i]["id"], yhset.Tables[0].Rows[i]["parentNumber"], "", yhset.Tables[0].Rows[i]["CPBM"], yhset.Tables[0].Rows[i]["CPPL"], yhset.Tables[0].Rows[i]["CPXH"], yhset.Tables[0].Rows[i]["DW"], yhset.Tables[0].Rows[i]["SLZ"], 0, "", "", yhset.Tables[0].Rows[i]["BZ"], type);
                yhid.Add(yhset.Tables[0].Rows[i]["id"].ToString());
            }
            dss.Tables.Add(dt.Copy());
        }
       
        return dss;
    }
    /// <summary>
    /// 添加要货产品
    /// </summary>
    /// <param name="strnumber">要货编号</param>
    /// <param name="type">0:要货单1:销售单</param>
    /// <param name="delnumber">要删除的ID</param>
    /// <returns></returns>
    public DataSet addyhRow(string strnumber, string type,string delnumber)
    {
        ArrayList yhid = new ArrayList();
        DataSet dss = new DataSet();
        string sql = "select * from CSKHFWZX_YHSQD_YHLB WHERE FHZT=0 AND parentNumber in( " + strnumber + ") AND ID NOT IN (" + delnumber + ")";
        DataSet yhset = DbHelperSQL.Query(sql);
        if (yhset.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < yhset.Tables[0].Rows.Count; i++)
            {
                dt.Rows.Add("", yhset.Tables[0].Rows[i]["id"], yhset.Tables[0].Rows[i]["parentNumber"], "", yhset.Tables[0].Rows[i]["CPBM"], yhset.Tables[0].Rows[i]["CPPL"], yhset.Tables[0].Rows[i]["CPXH"], yhset.Tables[0].Rows[i]["DW"], yhset.Tables[0].Rows[i]["SLZ"], 0, "", "", yhset.Tables[0].Rows[i]["BZ"], type);
                yhid.Add(yhset.Tables[0].Rows[i]["id"].ToString());
            }
            dss.Tables.Add(dt.Copy());
        }
        return dss;
    }
   
    /// <summary>
    /// 添加销售单产品
    /// </summary>
    /// <param name="strnumber">销售单编号</param>
    /// <param name="type">0:要货单1:销售单</param>
    /// <returns></returns>
    public DataSet addxsRow(string strnumber, string type)
    {
        ArrayList xsid = new ArrayList();
        DataSet dss = new DataSet();
        string sql = "select * from YXSYB_CPXSD_dhyd WHERE FHZT=0 AND parentNumber in( " + strnumber + ")";
        DataSet yhset = DbHelperSQL.Query(sql);
        if (yhset.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < yhset.Tables[0].Rows.Count; i++)
            {
                dt.Rows.Add("", yhset.Tables[0].Rows[i]["id"], "", yhset.Tables[0].Rows[i]["parentNumber"], yhset.Tables[0].Rows[i]["CPBM"], yhset.Tables[0].Rows[i]["CPPL"], yhset.Tables[0].Rows[i]["CPXH"], yhset.Tables[0].Rows[i]["DW"], yhset.Tables[0].Rows[i]["SL"], yhset.Tables[0].Rows[i]["SL"], "", "", "", type);
                xsid.Add(yhset.Tables[0].Rows[i]["id"].ToString());
            }
           
        }
        string count = dt.Rows.Count.ToString();
        dss.Tables.Add(dt.Copy());
        return dss;
    }

    /// <summary>
    /// 添加销售单产品
    /// </summary>
    /// <param name="strnumber">销售单编号</param>
    /// <param name="type">0:要货单1:销售单</param>
    /// <param name="delnumber">要删除的销售产品ID</param>
    /// <returns></returns>
    public DataSet addxsRow(string strnumber, string type,string delnumber)
    {
        ArrayList xsid = new ArrayList();
        DataSet dss = new DataSet();
        string sql = "select * from YXSYB_CPXSD_dhyd WHERE FHZT=0 AND parentNumber in( " + strnumber + ") AND ID NOT IN (" + delnumber + ")";
        DataSet yhset = DbHelperSQL.Query(sql);
        if (yhset.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < yhset.Tables[0].Rows.Count; i++)
            {
                dt.Rows.Add("", yhset.Tables[0].Rows[i]["id"], "", yhset.Tables[0].Rows[i]["parentNumber"], yhset.Tables[0].Rows[i]["CPBM"], yhset.Tables[0].Rows[i]["CPPL"], yhset.Tables[0].Rows[i]["CPXH"], yhset.Tables[0].Rows[i]["DW"], yhset.Tables[0].Rows[i]["SL"], yhset.Tables[0].Rows[i]["SL"], "", "", "", type);
                xsid.Add(yhset.Tables[0].Rows[i]["id"].ToString());
            }
          
        }
        string count = dt.Rows.Count.ToString();
        dss.Tables.Add(dt.Copy());
        return dss;
    }
    /// <summary>
    /// 更新要货申请单状态
    /// </summary>
    /// <param name="yhNumber">要货单号</param>
    public static void upyhFHZT(string yhNumber)
    {
        string sql = "update CSKHFWZX_YHSQD set FHZT=1 WHERE Number in (" + yhNumber + ") and ((select count(id) from  CSKHFWZX_YHSQD_YHLB where parentNumber in (" + yhNumber + "))=(select count(id) from  CSKHFWZX_YHSQD_YHLB where parentNumber in (" + yhNumber + ")and fhzt=1)) ";
        DbHelperSQL.ExecuteSql(sql);
    }
    /// <summary>
    /// 更新销售单的发货状态
    /// </summary>
    /// <param name="xsNumber">销售单号</param>
    public static void upxsFHZT(string xsNumber)
    {
        string sql = "update YXSYB_CPXSD set FHZT=1 WHERE Number in (" + xsNumber + ") and ((select count(id) from  YXSYB_CPXSD_dhyd where parentNumber in (" + xsNumber + "))=(select count(id) from  YXSYB_CPXSD_dhyd where parentNumber in (" + xsNumber + ")and fhzt=1)) ";
        DbHelperSQL.ExecuteSql(sql);
    }


    public string getnumber(ArrayList list)
    {
        string a = "";
        if (list.Count > 0)
        {
            if (list.Count > 1)
            {

                for (int i = 0; i < list.Count; i++)
                {
                    if (i == 0)
                        a = list[i].ToString().Trim();
                    else
                        a = a + "," + list[i].ToString().Trim();
                }
            }
            else
            {
                a = list[0].ToString().Trim();
            }
        }
        return a;
    }
}
