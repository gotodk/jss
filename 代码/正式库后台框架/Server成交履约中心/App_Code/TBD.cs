/********************************************************
 * 
 * 创建人：wyh
 *
 *创建时间：2014.06.5
 *
 *代码功能：一些移植过来的公共类....
 *
 *参考文档：
 * 
 * ********************************************************/
using FMDBHelperClass;
using FMipcClass;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// TBD 的摘要说明
/// </summary>
public class TBD
{
	public TBD()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}

    public DataSet initReturnDataSet()
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

    public DataSet GetMoneydbzDS(string Number)
    {
        DataSet ds = null;

        I_Dblink I_DBL_ZKLS = (new DBFactory()).DbLinkSqlMain("");
        DataSet dszkls = new DataSet();
        Hashtable inputzkls = new Hashtable();

        inputzkls["@NUMBER"] = Number;

        Hashtable return_htzkls = I_DBL_ZKLS.RunParam_SQL("SELECT * FROM AAA_moneyDZB WHERE NUMBER=@NUMBER", "DZB", inputzkls);
        if ((bool)(return_htzkls["return_float"])) //说明执行完成
        {
            //这里就可以用了。
            dszkls = (DataSet)(return_htzkls["return_ds"]);
            if (dszkls != null && dszkls.Tables["DZB"].Rows.Count > 0)
            {
                ds = dszkls;
            }

        }

        return ds;

    }

    public DataTable dtmeww()
    {
        DataTable dtmess = new DataTable();
        DataColumn dc = null; //new DataColumn("提醒对象登陆邮箱");

        dc = new DataColumn("type");
        dc.DataType = typeof(string);
        dtmess.Columns.Add(dc);

        dc = new DataColumn("提醒对象登陆邮箱");
        dc.DataType = typeof(string);
        dtmess.Columns.Add(dc);


        dc = new DataColumn("提醒对象结算账户类型");
        dc.DataType = typeof(string);
        dtmess.Columns.Add(dc);

        dc = new DataColumn("提醒对象角色编号");
        dc.DataType = typeof(string);
        dtmess.Columns.Add(dc);

        dc = new DataColumn("提醒对象角色类型");
        dc.DataType = typeof(string);
        dtmess.Columns.Add(dc);

        dc = new DataColumn("提醒内容文本");
        dc.DataType = typeof(string);
        dtmess.Columns.Add(dc);

        dc = new DataColumn("创建人");
        dc.DataType = typeof(string);
        dtmess.Columns.Add(dc);

        dc = new DataColumn("模提醒模块名");
        dc.DataType = typeof(string);
        dtmess.Columns.Add(dc);

        dc = new DataColumn("查看地址");
        dc.DataType = typeof(string);
        dtmess.Columns.Add(dc);

        return dtmess;
    }

    public string GetKey(string Moud,string Expend)
    {
        string zjnumber = "";
        object[] re = IPC.Call("获取主键", new object[] { Moud, Expend });//--------------------
        if (re[0].ToString() == "ok")
        {
            //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
            if (re[1] != null && !re[1].ToString().Trim().Equals(""))
            {
                zjnumber = (string)(re[1]);
            }
            else
            {

                return ""  ;
            }
        }
        else
        {
            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。
            return "";
        }
        return zjnumber;

    }

    public string Getjszhlx(string Num)
    {
        string jszhlx = "";
        object[] re = IPC.Call("用户基本信息", new object[] { Num });//----------------
        if (re[0].ToString() == "ok")
        {
            DataSet reFF = (DataSet)(re[1]);
            if (reFF == null || reFF.Tables.Count <= 0 || reFF.Tables[0].Rows.Count <= 0)
            {
                return "";
            }
            jszhlx = reFF.Tables[0].Rows[0]["结算账户类型"].ToString();
        }
        else
        {
            return "";
        }

        return jszhlx;
    }

    public string GetYHM(string DLYX)
    {
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        //查询用户名
        string YHM = "";
        Hashtable htyhm = new Hashtable();
        htyhm["@B_dlyx"] = DLYX;
        string strsql = "select  I_JYFMC from  AAA_DLZHXXB  where B_DLYX=@B_dlyx";
        Hashtable ht_yhm = I_DBL.RunParam_SQL(strsql, "yhm", htyhm);
        if ((bool)(ht_yhm["return_float"])) //说明执行完成
        {
            //这里就可以用了。
            YHM = ((DataSet)ht_yhm["return_ds"]).Tables[0].Rows[0][0].ToString();

        }

        return YHM;
    }
}