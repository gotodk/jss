using FMDBHelperClass;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Services;

/// <summary>
/// pagerdemo 的摘要说明
/// </summary>
[WebService(Namespace = "http://other.ipc.com/", Description = "V1.0-->数据分页显示方法 ")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
// [System.Web.Script.Services.ScriptService]
public class pagerdemo : System.Web.Services.WebService {

    public pagerdemo () {

        //如果使用设计的组件，请取消注释以下行 
        //InitializeComponent(); 
    }


    /// <summary>
    /// 测试该接口是否还活着(每个接口必备)
    /// </summary>
    /// <param name="temp">随便传</param>
    /// <returns>返回ok就是接口正常</returns>
    [WebMethod(Description = "测试该接口是否还活着(每个接口必备)")]
    public string onlinetest(string temp)
    {
        //根据不同的传入值，后续可以检查不同的东西，比如这个接口所连接的数据库，比如进程池，服务器空间等等。。。
        return "ok";
    }

    /// <summary>
    /// 根据条件，获得数据集和其他相关数据
    /// 调用名称：分页数据获取
    /// </summary>
    /// <param name="GetCustomersDataPage_NAME">使用的存储过程</param>
    /// <param name="this_dblink">使用的数据库连接名</param>
    /// <param name="page_index">第几页</param>
    /// <param name="page_size">每个条数</param>
    /// <param name="serach_Row_str">列名</param>
    /// <param name="search_tbname">表名</param>
    /// <param name="search_mainid">主键</param>
    /// <param name="search_str_where">查询条件</param>
    /// <param name="search_paixu">排序字段</param>
    /// <param name="search_paixuZD">排序规则</param>
    /// <param name="count_float"></param>
    /// <param name="count_zd"></param>
    /// <param name="cmd_descript1"></param>   
    /// <returns></returns>
    [WebMethod(Description="分页数据获取")]
    public DataSet GetPagerDB(DataSet ds_page)
    {
        DataSet DataSet_Beuse = new DataSet();
        try
        {
            Hashtable HTwhere = new Hashtable();
            HTwhere["GetCustomersDataPage_NAME"] = ds_page.Tables[0].Rows[0]["GetCustomersDataPage_NAME"].ToString();
            HTwhere["this_dblink"] = ds_page.Tables[0].Rows[0]["this_dblink"].ToString();
            HTwhere["page_index"] = ds_page.Tables[0].Rows[0]["page_index"].ToString();
            HTwhere["page_size"] = ds_page.Tables[0].Rows[0]["page_size"].ToString();
            HTwhere["serach_Row_str"] = ds_page.Tables[0].Rows[0]["serach_Row_str"].ToString();
            HTwhere["search_tbname"] = ds_page.Tables[0].Rows[0]["search_tbname"].ToString();
            HTwhere["search_mainid"] = ds_page.Tables[0].Rows[0]["search_mainid"].ToString();
            HTwhere["search_str_where"] = ds_page.Tables[0].Rows[0]["search_str_where"].ToString();
            HTwhere["search_paixu"] = ds_page.Tables[0].Rows[0]["search_paixu"].ToString();
            HTwhere["search_paixuZD"] = ds_page.Tables[0].Rows[0]["search_paixuZD"].ToString();
            HTwhere["count_float"] = ds_page.Tables[0].Rows[0]["count_float"].ToString();
            HTwhere["count_zd"] = ds_page.Tables[0].Rows[0]["count_zd"].ToString();
            HTwhere["cmd_descript"] = ds_page.Tables[0].Rows[0]["cmd_descript"].ToString();           

            //初始化数据工厂  
            if (HTwhere["GetCustomersDataPage_NAME"] == null || HTwhere["GetCustomersDataPage_NAME"].ToString() == "")
            {
                HTwhere["GetCustomersDataPage_NAME"] = "GetCustomersDataPage2";
            }
            if (HTwhere["this_dblink"] == null || HTwhere["this_dblink"].ToString() == "")
            {
                HTwhere["this_dblink"] = "mainsqlserver";
            }
            if (HTwhere["page_index"] == null || HTwhere["page_index"].ToString() == "")
            {
                HTwhere["page_index"] = "0";
            }
            if (HTwhere["page_size"] == null || HTwhere["page_size"].ToString() == "")
            {
                HTwhere["page_size"] = "10";
            }
            if (HTwhere["cmd_descript"] == null || HTwhere["cmd_descript"].ToString() == "")
            {
                HTwhere["cmd_descript"] = "";
            }
            if (HTwhere["record_count"] == null || HTwhere["record_count"].ToString() == "")
            {
                HTwhere["record_count"] = 0;
            }
            if (HTwhere["page_count"] == null || HTwhere["page_count"].ToString() == "")
            {
                HTwhere["page_count"] = 0;
            }
            if (HTwhere["count_float"] == null || HTwhere["count_float"].ToString() == "")
            {
                HTwhere["count_float"] = "普通";
            }
            if (HTwhere["count_float"] == "特殊")
            {
                if (HTwhere["count_zd"] == null || HTwhere["count_zd"].ToString() == "")
                {
                    HTwhere["count_zd"] = "0";
                }
            }

            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain(HTwhere["this_dblink"].ToString());

            Hashtable Hashtable_PutIn = new Hashtable();
            Hashtable_PutIn.Add("@PageIndex", Convert.ToInt32(HTwhere["page_index"]));//页面索引
            Hashtable_PutIn.Add("@PageSize", Convert.ToInt32(HTwhere["page_size"]));//单页数量
            Hashtable_PutIn.Add("@strGetFields", HTwhere["serach_Row_str"]);//要查询的列
            Hashtable_PutIn.Add("@tableName", HTwhere["search_tbname"]);//表名称
            Hashtable_PutIn.Add("@ID", HTwhere["search_mainid"]); //主键
            Hashtable_PutIn.Add("@strWhere", HTwhere["search_str_where"]); //查询条件
            Hashtable_PutIn.Add("@sortName", HTwhere["search_paixu"]); //排序方式,前后空格
            Hashtable_PutIn.Add("@orderName", HTwhere["search_paixuZD"]); //父级查询排序方式,用于排序的字段
            Hashtable_PutIn.Add("@countfloat", HTwhere["count_float"]); //普通/特殊，两种方式，一种默认，一种
            Hashtable_PutIn.Add("@countzd", HTwhere["count_zd"]); //特殊方式获取数据总量的值

            Hashtable Hashtable_PutOut = new Hashtable();
            Hashtable_PutOut.Add("@RecordCount", Convert.ToInt32(HTwhere["record_count"])); //返回记录总数
            Hashtable_PutOut.Add("@PageCount", Convert.ToInt32(HTwhere["page_count"])); //返回分页后页数
            Hashtable_PutOut.Add("@Descript", HTwhere["cmd_descript"].ToString()); //返回错误信息

            //获取数据
            Hashtable return_ht = new Hashtable();
            return_ht = I_DBL.RunProc_CMD(HTwhere["GetCustomersDataPage_NAME"].ToString(),"主要数据", Hashtable_PutIn, ref Hashtable_PutOut);

            if (Hashtable_PutOut["@PageCount"] == null || Hashtable_PutOut["@PageCount"].ToString() == "")
            {
                Hashtable_PutOut["@PageCount"] = "0";
            }
            if (Hashtable_PutOut["@RecordCount"] == null || Hashtable_PutOut["@RecordCount"].ToString() == "")
            {
                Hashtable_PutOut["@RecordCount"] = "0";
            }
            int page_count = 0; //分页数
            page_count = Convert.ToInt32(Hashtable_PutOut["@PageCount"]);

            int record_count = 0;//记录数
            record_count = Convert.ToInt32(Hashtable_PutOut["@RecordCount"]);

            string cmd_descript = ""; //其他描述
            cmd_descript = Hashtable_PutOut["@Descript"].ToString();

            string err_str = "";
            if ((bool)return_ht["return_float"])
            {
                DataSet_Beuse = ((DataSet)return_ht["return_ds"]);
                DataSet_Beuse.Tables[0].TableName = "主要数据";
                err_str = "";
            }
            else
            {
                err_str = return_ht["return_errmsg"].ToString();
            }
            
            DataTable objTable = new DataTable("附加数据");
            objTable.Columns.Add("分页数", typeof(int));
            objTable.Columns.Add("记录数", typeof(int));
            objTable.Columns.Add("其他描述", typeof(string));
            objTable.Columns.Add("执行错误", typeof(string));
            DataSet_Beuse.Tables.Add(objTable);

            DataSet_Beuse.Tables["附加数据"].Rows.Add(new object[] { page_count, record_count, cmd_descript, err_str });

            return DataSet_Beuse;
        }
        catch(Exception ex)
        {
            return null;           
        }
    }

    /// <summary>
    /// 初始化分页功能需要用的条件列表    
    /// </summary>    
    /// <returns>表结构</returns>
    [WebMethod(Description="定义分页参数")]
    public DataSet SetPagerInit(string[] page)
    { 
        try
        {
            DataTable dte = new DataTable();
            dte.Columns.Add("GetCustomersDataPage_NAME", typeof(string));
            dte.Columns.Add("this_dblink", typeof(string));
            dte.Columns.Add("page_index", typeof(string));
            dte.Columns.Add("page_size", typeof(string));
            dte.Columns.Add("serach_Row_str", typeof(string));
            dte.Columns.Add("search_tbname", typeof(string));
            dte.Columns.Add("search_mainid", typeof(string));
            dte.Columns.Add("search_str_where", typeof(string));
            dte.Columns.Add("search_paixu", typeof(string));
            dte.Columns.Add("search_paixuZD", typeof(string));
            dte.Columns.Add("count_float", typeof(string));
            dte.Columns.Add("count_zd", typeof(string));
            dte.Columns.Add("cmd_descript", typeof(string));            

            dte.Rows.Add(new object[] { "GetCustomersDataPage", "", page[0].ToString(), page[1].ToString(), "", "", "", "", "", "", "", "", ""});         

            DataSet ds = new DataSet();
            ds.Tables.Add(dte);
            return ds;
        }
        catch (Exception ex)
        {
            DataSet ds1 = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add("msg", typeof(string));
            dt.Rows.Add(ex.ToString());
            ds1.Tables.Add(dt);
            return ds1;
        }
    }

}
