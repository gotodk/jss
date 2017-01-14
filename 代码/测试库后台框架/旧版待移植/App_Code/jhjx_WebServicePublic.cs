using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using FMOP.DB;
using Key;
using Hesion.Brick.Core;
using System.Data.SqlClient;
using Hesion.Brick.Core.WorkFlow;
using Galaxy.ClassLib.DataBaseFactory;
using System.Web.Services;
using System.Threading;
using System.Reflection;

/// <summary>
///WebServicePublic 的摘要说明
/// </summary>
[WebService(Namespace = "http://www.fm8844.com/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
//若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。 
// [System.Web.Script.Services.ScriptService]
public class jhjx_WebServicePublic : System.Web.Services.WebService
{
    //连接工厂接口
    public Galaxy.ClassLib.DataBaseFactory.I_DBFactory I_DBF;
    //数据库连接接口
    public Galaxy.ClassLib.DataBaseFactory.I_Dblink I_DBL;

    public jhjx_WebServicePublic()
    {

        //如果使用设计的组件，请取消注释以下行 
        //InitializeComponent(); 

        //初始化工厂
        AppSettingsReader hh = new AppSettingsReader();//调用web.Config中的数据库配置
        I_DBF = new Galaxy.ClassLib.DataBaseFactory.DBFactory();
        I_DBL = I_DBF.DbLinkSqlMain(ConfigurationManager.ConnectionStrings["FMOPConn"].ToString());
    }


    /// <summary>
    /// 根据条件，获得数据集和其他相关数据
    /// </summary>
    /// <param name="GetCustomersDataPage_NAME"></param>
    /// <param name="this_dblink"></param>
    /// <param name="page_index"></param>
    /// <param name="page_size"></param>
    /// <param name="serach_Row_str"></param>
    /// <param name="search_tbname"></param>
    /// <param name="search_mainid"></param>
    /// <param name="search_str_where"></param>
    /// <param name="search_paixu"></param>
    /// <param name="search_paixuZD"></param>
    /// <param name="count_float"></param>
    /// <param name="count_zd"></param>
    /// <param name="cmd_descript1"></param>
    /// <param name="method_retreatment">再处理对应的方法</param>
    /// <returns></returns>
    [WebMethod]
    public DataSet GetPagerDB(string GetCustomersDataPage_NAME, string this_dblink, string page_index, string page_size, string serach_Row_str, string search_tbname, string search_mainid, string search_str_where, string search_paixu, string search_paixuZD, string count_float, string count_zd, string cmd_descript1,string method_retreatment)
    {
        Hashtable HTwhere = new Hashtable();
        HTwhere["GetCustomersDataPage_NAME"] = GetCustomersDataPage_NAME;
        HTwhere["this_dblink"] = this_dblink;
        HTwhere["page_index"] = page_index;
        HTwhere["page_size"] = page_size;
        HTwhere["serach_Row_str"] = serach_Row_str;
        HTwhere["search_tbname"] = search_tbname;
        HTwhere["search_mainid"] = search_mainid;
        HTwhere["search_str_where"] = search_str_where;
        HTwhere["search_paixu"] = search_paixu;
        HTwhere["search_paixuZD"] = search_paixuZD;
        HTwhere["count_float"] = count_float;
        HTwhere["count_zd"] = count_zd;
        HTwhere["cmd_descript"] = cmd_descript1;
 









        DataSet DataSet_Beuse = new DataSet();

        //DataTable objTable1 = new DataTable("主要数据");
        //DataSet_Beuse.Tables.Add(objTable1);




        AppSettingsReader hh = new AppSettingsReader();//调用web.Config中的数据库配置
        //初始化数据工厂
        string csstr = "";
        if (HTwhere["this_dblink"] == null || HTwhere["this_dblink"].ToString() == "")
        {
            csstr = "FMOPConn";
        }
        else
        {
            if (HTwhere["this_dblink"].ToString() == "ERP")
            {
                csstr = "fmConnection";
            }
            if (HTwhere["this_dblink"].ToString() == "erpFMOPConn")
            {
                csstr = "erpFMOPConn";
            }
            if (HTwhere["this_dblink"].ToString() == "fmConnection2")
            {
                csstr = "fmConnection2";
            }

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
        if (HTwhere["GetCustomersDataPage_NAME"] == null || HTwhere["GetCustomersDataPage_NAME"].ToString() == "")
        {
            HTwhere["GetCustomersDataPage_NAME"] = "GetCustomersDataPage2";
        }
        if (HTwhere["count_float"] == "特殊")
        {
            if (HTwhere["count_zd"] == null || HTwhere["count_zd"].ToString() == "")
            {
                HTwhere["count_zd"] = "0";
            }
        }
        I_DBF = new DBFactory();
        I_DBL = I_DBF.DbLinkSqlMain(ConfigurationManager.ConnectionStrings[csstr].ToString());



        if (HTwhere["page_index"] == null || HTwhere["page_index"].ToString() == "")
        {
            HTwhere["page_index"] = "0";
        }


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
        return_ht = I_DBL.RunProc_CMD(HTwhere["GetCustomersDataPage_NAME"].ToString(), DataSet_Beuse, Hashtable_PutIn, ref Hashtable_PutOut);
        
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
            //若不需要再处理
            if (method_retreatment.Trim() == "")
            {
                DataSet_Beuse = ((DataSet)return_ht["return_ds"]);
                DataSet_Beuse.Tables[0].TableName = "主要数据";
                err_str = "";
            }
            else
            {
                //需要进行再处理，传入分页得到的基础结果
                string Cname = method_retreatment.Split('|')[0];  //类名
                string Mname = method_retreatment.Split('|')[1]; //方法名
                Assembly asm = Assembly.GetExecutingAssembly();
                Type type = asm.GetType(Cname);
                object instance = asm.CreateInstance(Cname);
                MethodInfo method = type.GetMethod(Mname);
                DataSet_Beuse = (DataSet)method.Invoke(instance, new object[] { ((DataSet)return_ht["return_ds"]) });
                DataSet_Beuse.Tables[0].TableName = "主要数据";

                if (DataSet_Beuse.Tables.Contains("二次处理错误"))
                {
                    err_str = DataSet_Beuse.Tables["二次处理错误"].Rows[0]["执行错误"].ToString();
                }
                else
                {
                    err_str = "";
                }
               
            }
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



    /// <summary>
    /// 测试获得一个字符串
    /// </summary>
    /// <returns></returns>
    [WebMethod]
    public string test_str()
    {
        return "标记：测试库";
    }

    /// <summary>
    /// 测试获取新编号
    /// </summary>
    /// <returns></returns>
    [WebMethod]
    public string test_GetNextNumberZZ()
    {
        return jhjx_PublicClass.IsOpenTime().ToString();
        //return jhjx_PublicClass.GetNextNumberZZ("ZZ_PTDTCSSDB", "CE");
    }

    /// <summary>
    /// 获得成交信息数据集，首页列表显示
    /// </summary>
    /// <returns></returns>
    [WebMethod]
    public byte[] test_cjxx(DataTable ht)
    {
        //初始化返回值,先塞一行数据


        DataSet dsreturn = new DataSet();
 
        jhjx_cjxq cjxx = new jhjx_cjxq();
        byte[] b = Helper.DataSet2Byte(cjxx.jhjx_cjxx(dsreturn, ht));
        return b;
    }

    /// <summary>
    /// 获得一个数据集，首页列表显示
    /// </summary>
    /// <returns></returns>
    [WebMethod]
    public byte[] test_ds(string sp)
    {
        HomePage HP = new HomePage();
        string lujing = Context.Request.MapPath("../../bytefiles/HomePageSQLlist.txt");
        return Helper.DataSet2Byte(HP.test_ds_Run(sp, lujing));
    }


    /// <summary>
    /// 直接运行SQL语句，返回数据集(仅限监控，客户端不要用这个)
    /// </summary>
    /// <returns></returns>
    [WebMethod]
    public DataSet RunSQL(string sqlstr)
    {
        DataSet ds = DbHelperSQL.Query(sqlstr);
        return ds;
    }

    /// <summary>
    /// 直接运行SQL语句，返回影响的行数(仅限监控，客户端不要用这个)
    /// </summary>
    /// <returns></returns>
    [WebMethod]
    public int RunSQLOnly(string sqlstr)
    {
        int i = DbHelperSQL.ExecuteSql(sqlstr);
        return i;
    }

    /// <summary>
    /// 直接运行SQL语句组(事务)，返回是否执行成功(仅限监控，客户端不要用这个)
    /// </summary>
    /// <returns></returns>
    [WebMethod]
    public bool RunSQLTran(DataSet DTsql)
    {
        ArrayList SQLstrList = new ArrayList();
        if (DTsql == null || DTsql.Tables[0].Rows.Count < 1)
        {
            return false;
        }
        else
        {
            for (int y = 0; y < DTsql.Tables[0].Rows.Count; y++)
            {
                SQLstrList.Add(DTsql.Tables[0].Rows[y][0].ToString());
            }
        }
        bool i = DbHelperSQL.ExecSqlTran(SQLstrList);
        return i;
    }

    /// <summary>
    /// 获得大盘底部统计分析，直接从压缩文件中读取
    /// </summary>
    /// <returns></returns>
    [WebMethod]
    public string[][]  indexTJSJ()
    {
        string Lujing = LuJing();
        HomePage HP = new HomePage();
        return HP.indexTJSJ_Run(Lujing);
    }
    private string LuJing()
    {
        return Context.Request.MapPath("../../bytefiles/HomePageByte_tongji.txt");
    }

    /// <summary>
    /// 可以提前获取的一些数据或参数
    /// </summary>
    /// <returns>返回数据集</returns>
    [WebMethod]
    public DataSet GetPublisDsData()
    {
        DataSet PublisDsData = new DataSet();

        //获取省市区
        DataSet ds_sheng = DbHelperSQL.Query("select p_number as 省编号,p_namestr as 省名,'0' as 省标记 from ZZ_CityList_Promary");
        ds_sheng.Tables[0].TableName = "省";
        DataSet ds_shi = DbHelperSQL.Query("select ZZ_CityList_City.c_number as 市编号,ZZ_CityList_City.c_namestr as 市名,ZZ_CityList_City.p_number as 所属省编号,(select ZZ_CityList_Promary.p_namestr from ZZ_CityList_Promary where ZZ_CityList_Promary.p_number=ZZ_CityList_City.p_number) as 所属省名 from ZZ_CityList_City");
        ds_shi.Tables[0].TableName = "市";
        DataSet ds_qu = DbHelperSQL.Query("select ZZ_CityList_qu.q_number as 区编号,ZZ_CityList_qu.q_namestr as 区名,ZZ_CityList_qu.c_number as 所属市编号,(select ZZ_CityList_City.c_namestr from ZZ_CityList_City where ZZ_CityList_qu.c_number=ZZ_CityList_City.c_number) as 所属市名 from ZZ_CityList_qu");
        ds_qu.Tables[0].TableName = "区";
        PublisDsData.Tables.Add(ds_sheng.Tables[0].Copy());
        PublisDsData.Tables.Add(ds_shi.Tables[0].Copy());
        PublisDsData.Tables.Add(ds_qu.Tables[0].Copy());


        //获取其他数据
        DataSet dsStartSPFL = DbHelperSQL.Query("select SortID 商品编号,SortName 商品名称,SortParentID 所属商品编号 from dbo.ZZ_tbMenuSPFL where SortParentID='0'");
        dsStartSPFL.Tables[0].TableName = "一级分类";

        DataSet dsSecondSPFL = DbHelperSQL.Query("select a.商品名称,a.商品编号,b.所属商品名称,a.所属商品编号 from (select SortID 商品编号,SortName 商品名称,SortParentID 所属商品编号 from dbo.ZZ_tbMenuSPFL where SortParentID in(select SortID from ZZ_tbMenuSPFL where SortParentID='0')) as a left join (select SortName 所属商品名称,SortID 所属商品编号 from ZZ_tbMenuSPFL where SortID in(select SortParentID from dbo.ZZ_tbMenuSPFL where SortParentID in(select SortID from ZZ_tbMenuSPFL where SortParentID='0'))) as b on a.所属商品编号=b.所属商品编号");
        dsSecondSPFL.Tables[0].TableName = "二级分类";
        PublisDsData.Tables.Add(dsStartSPFL.Tables[0].Copy());
        PublisDsData.Tables.Add(dsSecondSPFL.Tables[0].Copy());

        //获取最新的配置动态参数信息
        //获取单个数据，取出最后一条更新的数据
        DataSet ds_p = DbHelperSQL.Query("select top 1 * from ZZ_PTDTCSSDB order by CreateTime DESC");
        ds_p.Tables[0].TableName = "动态参数";
        PublisDsData.Tables.Add(ds_p.Tables[0].Copy());

        //返回数据
        return PublisDsData;
    }


    /// <summary>
    /// 登陆验证
    /// </summary>
    /// <returns>返回用户相关数据，或登陆失败数据</returns>
    [WebMethod]
    public DataSet checkLoginIn(string user,string pass,string IP)
    {
        //
        //DataSet ds = DbHelperSQL.Query("select *,'卖家角色编号'='xx','买家角色编号'='xx','经纪人角色编号'='xx' from  ZZ_UserLogin where DLYX = '" + user + "' and  DLMM = '" + pass + "' ");
        //string strSql = "SELECT ZZ_UserLogin.Number, ZZ_UserLogin.DLYX, ZZ_UserLogin.YHM, DLMM, ZZ_UserLogin.JSZHLX, YXYZM, YZMGQSJ, SFYZYX, SFYXDL, SFDJZH, SFXM, SFJCXM, JCXMSJ, JCXMJL, ZCSJ, ZCIP, ZHYCDLSJ, ZHYCDLIP, DLCS, ZHDQKYYE, ZZ_UserLogin.NextChecker, ZZ_UserLogin.CheckState, ZZ_UserLogin.CreateUser, ZZ_UserLogin.CreateTime, ZZ_UserLogin.CheckLimitTime,ISNULL(ZZ_BUYJBZLXXB.JSBH,'') AS '买家角色编号',ISNULL(ZZ_SELJBZLXXB.JSBH,'') AS '卖家角色编号',ISNULL(ZZ_DLRJBZLXXB.JSBH,'') AS '经纪人角色编号' FROM ZZ_UserLogin FULL JOIN ZZ_BUYJBZLXXB ON ZZ_BUYJBZLXXB.DLYX=ZZ_UserLogin.DLYX FULL JOIN ZZ_SELJBZLXXB ON ZZ_SELJBZLXXB.DLYX = ZZ_UserLogin.DLYX FULL JOIN ZZ_DLRJBZLXXB ON ZZ_DLRJBZLXXB.DLYX = ZZ_UserLogin.DLYX WHERE ZZ_UserLogin.DLYX='" + user.Trim() + "' AND DLMM='" + pass + "'";//
        string BuyNumber = "";
        string SellNumber = "";
        string DlNumber="";
        DataTable dtNumber = DbHelperSQL.Query("SELECT JSLX,JSBH,DLYX FROM ZZ_YHJSXXB WHERE DLYX='" + user + "'").Tables[0];
        if (dtNumber.Rows.Count > 0)
        {
            for (int i = 0; i < dtNumber.Rows.Count; i++)
            {
                if (dtNumber.Rows[i]["JSLX"].ToString() == "买家")
                    BuyNumber = dtNumber.Rows[i]["JSBH"].ToString().Trim();
                if (dtNumber.Rows[i]["JSLX"].ToString() == "卖家")
                    SellNumber = dtNumber.Rows[i]["JSBH"].ToString().Trim();
                if (dtNumber.Rows[i]["JSLX"].ToString() == "经纪人")
                    DlNumber = dtNumber.Rows[i]["JSBH"].ToString().Trim();
            }
        }

        string strSql = "SELECT *,'" + BuyNumber + "' AS 买家角色编号,'" + SellNumber + "' AS 卖家角色编号, '" + DlNumber + "' AS 经纪人角色编号 FROM ZZ_UserLogin WHERE DLYX='" + user + "' AND DLMM='" + pass + "'";
        DataSet ds = DbHelperSQL.Query(strSql);
        //无论是否登陆成功，都要加入这个错误信息表，并且要确保这个表中有且只有一条数据
        DataTable objTable = new DataTable("登录附加");
        objTable.Columns.Add("是否登录成功", typeof(string));
        objTable.Columns.Add("错误信息1", typeof(string));
        objTable.Columns.Add("错误信息2", typeof(string));
        objTable.Columns.Add("错误信息3", typeof(string));
        objTable.Columns.Add("成功登陆IP地址", typeof(string));
        ds.Tables.Add(objTable);

        //开始验证
        if (ds != null && ds.Tables[0].Rows.Count == 1)
        {
            //说明账号密码正确，这里需要进行其他验证，比如状态，若可以登陆，还要生成各项可能需要的其他信息
            if (ds.Tables[0].Rows[0]["SFYZYX"].ToString() == "否")
            {
                ds.Tables["登录附加"].Rows.Add(new object[] { "失败", "您的邮箱尚未通过验证！", "未激活", "", " " });
                return ds;
            }

            //if (ds.Tables[0].Rows[0]["SFDJZH"].ToString() == "是") //是否冻结账号
            //{
            //    ds.Tables["登录附加"].Rows.Add(new object[] { "失败", "您的账号已被冻结！", "冻结", ""," " });
            //    return ds;
            //}
            if (ds.Tables[0].Rows[0]["SFYXDL"].ToString() == "否")//是否允许登陆
            {
                ds.Tables["登录附加"].Rows.Add(new object[] { "失败", "您的账号已禁止登录！", "禁止登录", ""," " });
                return ds;
            }
            if (ds.Tables[0].Rows[0]["SFXM"].ToString() == "是" && (ds.Tables[0].Rows[0]["SFJCXM"] == DBNull.Value || ds.Tables[0].Rows[0]["SFJCXM"].ToString().Trim() == "否" || ds.Tables[0].Rows[0]["SFJCXM"].ToString().Trim() == ""))//是否休眠
            {
                ds.Tables["登录附加"].Rows.Add(new object[] { "失败", "您的账号因长时间未登录已被休眠！", "休眠", ""," " });
                return ds;
            }
            if (jhjx_PublicClass.IsSleepingID(user))
            {
                ds.Tables["登陆附加"].Rows.Add(new object[] { "失败", "您的账号因长时间未登录已被休眠！", "休眠", "", " " });
                return ds;
            }

            ds.Tables["登录附加"].Rows.Add(new object[] { "成功", "", "","", GetIP(user) });
        }
        else
        {
            ds.Tables["登录附加"].Rows.Add(new object[] { "失败", "认证失败：您输入的账号与密码不匹配，请重新输入！", "", "", " " });//这里的IP做测试用
          
        }


        return ds;
    }

       /// <summary>
    /// 注册用户
    /// </summary>
    /// <param name="uid">登录id</param>
    /// <param name="uname">昵称</param>
    /// <param name="pwd">密码</param>
    /// <param name="valnum">验证码</param>
    /// <returns>执行结果</returns>
    [WebMethod]
    public DataSet UserRegister(string uid, string uname, string pwd, string valnum)
    {
        DataSet ds = new DataSet();//要返回的数据集

        #region 初始化数据表结构
        DataTable dt = new DataTable();//要返回的Datatable
        dt.Columns.Add("pk", typeof(string));//PK
        dt.Columns.Add("DLYX", typeof(string));//登录邮箱
        dt.Columns.Add("YHM", typeof(string));//用户名
        dt.Columns.Add("DLMM", typeof(string));//密码
        dt.Columns.Add("YZM", typeof(string));//验证码
        dt.Columns.Add("RESULT", typeof(string));//执行结果
        dt.Columns.Add("REASON", typeof(string));//原因
        #endregion

        string isExist = "SELECT * FROM ZZ_UserLogin WHERE DLYX='" + uid.Trim() + "'";//是否有重复帐号
        string isExistUName = "SELECT * FROM ZZ_UserLogin WHERE YHM='" + uname.Trim() + "'";

        string isExist_Checks = "SELECT * FROM ZZ_UserLogin WHERE DLYX='" + uid.Trim() + "' AND SFYZYX='是'";//是否有重复帐号
        string isExistUName_Checks = "SELECT * FROM ZZ_UserLogin WHERE YHM='" + uname.Trim() + "' AND SFYZYX='是'";

        int isUname = DbHelperSQL.Query(isExistUName).Tables[0].Rows.Count;//是否有重复的用户名
        int isID = DbHelperSQL.Query(isExist_Checks).Tables[0].Rows.Count;//是否有重复的ID
        if (DbHelperSQL.Query(isExist).Tables[0].Rows.Count == 0 && isUname == 0)
        {
            #region 新用户，无一点记录。
            //WorkFlowModule PKs = new WorkFlowModule("ZZ_UserLogin");
            //string PK = PKs.numberFormat.GetNextNumber();
            string PK = jhjx_PublicClass.GetNextNumberZZ("ZZ_UserLogin", "");
            string DLYX = uid;
            string YHM = uname;
            string DLMM = pwd;
            string YXYZM = valnum;
            string YZMGQSJ = DateTime.Now.AddDays(1).ToString();//验证码过期时间
            string SFYZYX = "否";
            string SFYXDL = "否";
            string SFDJZH = "否";
            string SFZTXYW = "否";
            string SFXM = "否";
            string ZCSJ = DateTime.Now.ToString();
            string ZCIP = GetIP();
            string DLCS = "0";
            string ZHDQKYYE = "500000.00";
            string InsertSql = "INSERT INTO ZZ_UserLogin(Number,DLYX,YHM,DLMM,YXYZM,YZMGQSJ,SFYZYX,SFYXDL,SFDJZH,SFZTXYW,SFXM,ZCSJ,ZCIP,DLCS,ZHDQKYYE) VALUES('" + PK + "','" + DLYX + "','" + YHM + "','" + DLMM + "','" + YXYZM + "','" + YZMGQSJ + "','" + SFYZYX + "','" + SFYXDL + "','" + SFDJZH + "','" + SFZTXYW + "','" + SFXM + "','" + ZCSJ + "','" + ZCIP + "','" + DLCS + "','" + ZHDQKYYE + "')";//插入语句，在发送邮箱验证码之后执行。
            ClassEmail CE = new ClassEmail();
            string strSubject = "中国商品批发交易平台 - 注册账号";
            string modHtmlFileName = HttpContext.Current.Server.MapPath("~/JHJXPT/EmailTemplate.htm");
            Hashtable ht = new Hashtable();
            ht["[[Email]]"] = DLYX;
            ht["[[valNumbs]]"] = YXYZM;
            ht["[[DateTime]]"] = DateTime.Now.ToString("yyyy-MM-dd");
            CE.SendSPEmail(DLYX, strSubject, modHtmlFileName, ht);
            try
            {
                int i = DbHelperSQL.ExecuteSql(InsertSql);//执行sql
                if (i == 1)
                {
                    DataTable dtResult = dt.Clone();
                    dtResult.Rows.Add(new object[] { PK,DLYX, YHM, DLMM, YXYZM, "SUCCESS", "SUCCESS" });
                    dtResult.TableName = "执行结果";
                    ds.Tables.Clear();
                    ds.Tables.Add(dtResult);
                }
            }
            catch (Exception ex)
            {
                DataTable dtResult = dt.Clone();
                dtResult.Rows.Add(new object[] { PK,DLYX, YHM, DLMM, YXYZM, "FALSE", ex.Message });
                dtResult.TableName = "执行结果";
                ds.Tables.Clear();
                ds.Tables.Add(dtResult);
            }
            #endregion
        }
        else
        {
            //已经存在用户ID
            if (isID > 0)
            {
                DataTable dtNameToID = DbHelperSQL.Query(isExistUName).Tables[0];//获取已经存在的用户名的信息
                DataTable dtResult = dt.Clone();
                dtResult.Rows.Add(new object[] {"0", uid, uname, pwd, valnum, "FALSE", "已存在该帐户" });
                dtResult.TableName = "执行结果";
                ds.Tables.Clear();
                ds.Tables.Add(dtResult);
                return ds;
            }
            //已经存在用户名
            else if (isUname > 0)
            {
                DataTable dtNameToID = DbHelperSQL.Query(isExistUName).Tables[0];//获取已经存在的用户名的信息
                DataTable dtResult = dt.Clone();
                dtResult.Rows.Add(new object[] { "0",uid, uname, pwd, valnum, "FALSE", "用户名重复" });
                dtResult.TableName = "执行结果";
                ds.Tables.Clear();
                ds.Tables.Add(dtResult);
                return ds;
            }
            else
            {
                #region 判断是否是二次更改
                string isExist_Checked = "SELECT * FROM ZZ_UserLogin WHERE DLYX='" + uid.Trim() + "' AND SFYZYX='否'";
                string isExistUName_Checked = "SELECT * FROM ZZ_UserLogin WHERE YHM='" + uname.Trim() + "' AND SFYZYX='否'";
                #endregion
                DataTable tableDone = DbHelperSQL.Query(isExist_Checked).Tables[0];
                if (tableDone.Rows.Count > 0)
                {
                    string pk = tableDone.Rows[0]["Number"].ToString();
                    if (tableDone.Rows[0]["SFYZYX"].ToString() == "否")
                    {
                        //如果这个帐号还没有验证邮箱，则更新验证码，允许注册
                        string UpdateSql = "UPDATE ZZ_UserLogin SET YXYZM='" + valnum + "', YZMGQSJ='" + DateTime.Now.AddDays(1).ToString() + "',DLMM='" + pwd + "',YHM='" + uname + "' WHERE DLYX='" + uid + "'";
                        try
                        {
                            ClassEmail CE = new ClassEmail();
                            string strSubject = "中国商品批发交易平台 - 注册帐号";
                            string modHtmlFileName = HttpContext.Current.Server.MapPath("~/JHJXPT/EmailTemplate.htm");
                            Hashtable ht = new Hashtable();
                            ht["[[Email]]"] = uid;
                            ht["[[valNumbs]]"] = valnum;
                            ht["[[DateTime]]"] = DateTime.Now.ToString("yyyy-MM-dd");
                            CE.SendSPEmail(uid, strSubject, modHtmlFileName, ht);
                            int i = DbHelperSQL.ExecuteSql(UpdateSql);//更新语句
                            if (i == 1)
                            {
                                DataTable dtResult = dt.Clone();
                                dtResult.Rows.Add(new object[] {pk, uid, uname, pwd, valnum, "SUCCESS", "SUCCESS" });
                                dtResult.TableName = "执行结果";
                                ds.Tables.Clear();
                                ds.Tables.Add(dtResult);
                            }
                            else
                            {
                                DataTable dtResult = dt.Clone();
                                dtResult.Rows.Add(new object[] {pk, uid, uname, pwd, valnum, "FALSE", "服务器繁忙" });
                                dtResult.TableName = "执行结果";
                                ds.Tables.Clear();
                                ds.Tables.Add(dtResult);
                            }
                        }
                        catch (Exception ex)
                        {
                            DataTable dtResult = dt.Clone();
                            dtResult.Rows.Add(new object[] { pk,uid, uname, pwd, valnum, "FALSE", "服务器繁忙" });
                            dtResult.TableName = "执行结果";
                            ds.Tables.Clear();
                            ds.Tables.Add(dtResult);
                        }
                    }
                }
            }
        }
        return ds;
    }

    [WebMethod]
    public DataSet UserChangeRegister(string pk, string uid, string uname, string pwd, string valnum)
    {
        DataSet ds = new DataSet();
        #region 初始化数据表结构
        DataTable dt = new DataTable();//要返回的Datatable
        dt.Columns.Add("pk", typeof(string));//PK
        dt.Columns.Add("DLYX", typeof(string));//登录邮箱
        dt.Columns.Add("YHM", typeof(string));//用户名
        dt.Columns.Add("DLMM", typeof(string));//密码
        dt.Columns.Add("YZM", typeof(string));//验证码
        dt.Columns.Add("RESULT", typeof(string));//执行结果
        dt.Columns.Add("REASON", typeof(string));//原因
        #endregion
        string isExist = "SELECT * FROM ZZ_UserLogin WHERE DLYX='" + uid.Trim() + "' AND SFYZYX='是'";//是否有重复帐号
        string isExistUName = "SELECT * FROM ZZ_UserLogin WHERE YHM='" + uname.Trim() + "' AND SFYZYX='是'";
        DataTable dtIsEmail = DbHelperSQL.Query(isExist).Tables[0];//是否存在重复帐号
        DataTable dtIsExistUname = DbHelperSQL.Query(isExistUName).Tables[0];//是否存在用户昵称
        if (dtIsEmail.Rows.Count == 0 && dtIsExistUname.Rows.Count == 0)
        {
            //如果不存在，执行更新操作
            string UpdateSql = "UPDATE ZZ_UserLogin SET dlyx='" + uid + "', yhm='" + uname + "', dlmm='" + pwd + "', YXYZM='" + valnum + "' ,YZMGQSJ='" + DateTime.Now.AddDays(1).ToString() + "' WHERE Number='" + pk + "'";
            ClassEmail CE = new ClassEmail();
            string strSubject = "中国商品批发交易平台-邮箱验证";
            string modHtmlFileName = HttpContext.Current.Server.MapPath("~/JHJXPT/EmailTemplate.htm");
            Hashtable ht = new Hashtable();
            ht["[[Email]]"] = uid;
            ht["[[valNumbs]]"] = valnum;
            ht["[[DateTime]]"] = DateTime.Now.ToString("yyyy-MM-dd");
            CE.SendSPEmail(uid, strSubject, modHtmlFileName, ht);
            try
            {
                int i = DbHelperSQL.ExecuteSql(UpdateSql);//执行sql
                if (i == 1)
                {
                    DataTable dtResult = dt.Clone();
                    dtResult.Rows.Add(new object[] {pk, uid, uname, pwd, valnum, "SUCCESS", "SUCCESS" });
                    dtResult.TableName = "执行结果";
                    ds.Tables.Clear();
                    ds.Tables.Add(dtResult);
                }
            }
            catch (Exception ex)
            {
                DataTable dtResult = dt.Clone();
                dtResult.Rows.Add(new object[] {pk, uid, uname, pwd, valnum, "FALSE", ex.Message });
                dtResult.TableName = "执行结果";
                ds.Tables.Clear();
                ds.Tables.Add(dtResult);
            }
        }
        else
        {
            if (dtIsEmail.Rows.Count > 0)
            {
                DataTable dtNameToID = DbHelperSQL.Query(isExistUName).Tables[0];//获取已经存在的用户名的信息
                DataTable dtResult = dt.Clone();
                dtResult.Rows.Add(new object[] {pk, uid, uname, pwd, valnum, "FALSE", "已存在该帐户" });
                dtResult.TableName = "执行结果";
                ds.Tables.Clear();
                ds.Tables.Add(dtResult);
                return ds;
            }
            if (dtIsExistUname.Rows.Count > 0)
            {
                DataTable dtNameToID = DbHelperSQL.Query(isExistUName).Tables[0];//获取已经存在的用户名的信息
                DataTable dtResult = dt.Clone();
                dtResult.Rows.Add(new object[] {pk, uid, uname, pwd, valnum, "FALSE", "用户名重复" });
                dtResult.TableName = "执行结果";
                ds.Tables.Clear();
                ds.Tables.Add(dtResult);
                return ds;
            }
        }
        return ds;
    }
    /// <summary>
    /// 激活帐户（注册第三步）
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [WebMethod]
    public DataSet AccountID(string id,string valNum)
    {
        DataSet ds = new DataSet();
        #region 初始化数据表结构
        DataTable dt = new DataTable();//要返回的Datatable
        dt.Columns.Add("DLYX", typeof(string));//登录邮箱
        dt.Columns.Add("YHM", typeof(string));//验证码
        dt.Columns.Add("DLMM", typeof(string));//验证码
        dt.Columns.Add("RESULT", typeof(string));//执行结果
        dt.Columns.Add("REASON", typeof(string));//原因
        #endregion

        //验证验证码时间是否过期
        DataTable dtAccountID = DbHelperSQL.Query("SELECT * FROM ZZ_UserLogin WHERE DLYX='" + id + "'").Tables[0];
        if (dtAccountID.Rows.Count > 0)
        {
            DateTime ValEndTime = Convert.ToDateTime(dtAccountID.Rows[0]["YZMGQSJ"].ToString());
            if (ValEndTime < DateTime.Now)
            {
                dt.Rows.Add(new object[] { id, "", "", "FALSE", "验证码已过期" });
                ds.Tables.Clear();
                ds.Tables.Add(dt);
                return ds;
            }
        }

        string UpdateSql = "UPDATE ZZ_UserLogin SET SFYZYX='是',SFYXDL='是',SFDJZH='否',SFZTXYW='否',SFXM='否' WHERE DLYX='" + id + "'";
        try
        {
            int i = DbHelperSQL.ExecuteSql(UpdateSql);
            if (i == 1)
            {
                string SelectSql = "SELECT DLYX,YHM,DLMM,'SUCCESS' AS RESULT,'SUCCESS' AS REASON FROM ZZ_UserLogin WHERE DLYX='" + id + "'";
                DataTable dtreturn = DbHelperSQL.Query(SelectSql).Tables[0];
                dt = null;
                dt = dtreturn.Copy();
                ds.Tables.Clear();
                ds.Tables.Add(dt);
                return ds;
            }
            else
            {
                dt.Rows.Add(new object[] { id, "", "", "FALSE", "服务器繁忙(0)" });
                ds.Tables.Clear();
                ds.Tables.Add(dt);
                return ds;
            }
        }
        catch
        {
            dt.Rows.Add(new object[] { id, "", "", "FALSE", "服务器繁忙(1)" });
            ds.Tables.Clear();
            ds.Tables.Add(dt);
            return ds;
        }
    }
    /// <summary>
    /// 再次发送验证码（注册，换验证码，不换邮箱）
    /// </summary>
    /// <param name="dlyx"></param>
    /// <param name="valNum"></param>
    /// <returns></returns>
    [WebMethod]
    public DataSet reSendValNum(string dlyx, string valNum)
    {
        DataSet ds = new DataSet();
        #region 初始化数据表结构
        DataTable dt = new DataTable();//要返回的Datatable
        dt.Columns.Add("DLYX", typeof(string));//登录邮箱
        dt.Columns.Add("YZM", typeof(string));//验证码
        dt.Columns.Add("RESULT", typeof(string));//执行结果
        dt.Columns.Add("REASON", typeof(string));//原因
        #endregion
        string UpdateSql = "UPDATE ZZ_UserLogin SET YXYZM='" + valNum + "', YZMGQSJ='" + DateTime.Now.AddDays(1).ToString() + "' WHERE DLYX='" + dlyx + "'";//要执行的更新语句
        try
        {
            ClassEmail CE = new ClassEmail();
            string strSuject = "中国商品批发交易平台-注册帐号";
            string modHtmlFileName = HttpContext.Current.Server.MapPath("~/JHJXPT/EmailTemplate.htm");
            Hashtable ht = new Hashtable();
            ht["[[Email]]"] = dlyx;
            ht["[[valNumbs]]"] = valNum;
            ht["[[DateTime]]"] = DateTime.Now.ToString("yyyy-MM-dd");
            CE.SendSPEmail(dlyx, strSuject, modHtmlFileName, ht);
        }
        catch(Exception ex)
        {
            dt.Rows.Add(new object[] { dlyx, valNum, "FALSE", "发送邮件失败" });
            ds.Tables.Clear();
            ds.Tables.Add(dt);
            return ds;
        }
        try
        {
            int i = DbHelperSQL.ExecuteSql(UpdateSql);//执行更新语句
            if (i == 1)
            {
                dt.Rows.Add(new object[] { dlyx, valNum, "SUCCESS", "操作成功" });
                ds.Tables.Clear(); ds.Tables.Add(dt);
                return ds;
            }
            else
            {
                dt.Rows.Add(new object[] { dlyx, valNum, "FALSE", "更新验证码失败" });
                ds.Tables.Clear();
                ds.Tables.Add(dt);
                return ds;
            }
        }
        catch (Exception ex)
        {
            dt.Rows.Add(new object[] { dlyx, valNum, "FALSE", "更新验证码失败" });
            ds.Tables.Clear();
            ds.Tables.Add(dt);
            return ds;
        }
    }
    /// <summary>
    /// 修改密码
    /// </summary>
    /// <param name="uid">用户登录Email</param>
    /// <param name="phoneoremail">找回密码的email或者手机号</param>
    /// <param name="pwd">新密码</param>
    /// <param name="valNum">验证码</param>
    /// <param name="typeToSet">找回密码方式</param>
    /// <returns></returns>
    [WebMethod]
    public DataSet ChangePassword(string uid, string phoneoremail, string pwd, string valNum, string typeToSet)
    {
        DataSet ds = new DataSet();//要返回的DataSet
        string SelectReturn = "SELECT '' AS Uid, '' AS Email,'' AS Phone,'' AS NewPwd,'' AS Results FROM ZZ_UserValiNumbers WHERE 1!=1";
        DataTable dtReturnTemp = DbHelperSQL.Query(SelectReturn).Tables[0];//初始化要返回的Datatable的表结构
        string SelectValEmail = "SELECT TOP 1 * FROM ZZ_UserValiNumbers WHERE subType='email' AND userEmail='" + uid.Trim() + "' ORDER BY Number DESC";
        DataTable dtValEmail = DbHelperSQL.Query(SelectValEmail).Tables[0];//Email验证码信息
        string SelectValPhone = "SELECT TOP 1 * FROM ZZ_UserValiNumbers WHERE subType='phone' AND userEmail='" + uid.Trim() + "' ORDER BY Number DESC";
        DataTable dtValPhone = DbHelperSQL.Query(SelectValPhone).Tables[0];//Phone验证码信息
        DataTable dtSuccess = dtReturnTemp.Clone();//复制表结构（若更改成功）
        dtSuccess.TableName = "Success";
        DataTable dtFalse = dtReturnTemp.Clone();//复制表结构（若更改失败）
        dtFalse.TableName = "False";
        string UpdateSql = "UPDATE ZZ_UserLogin SET DLMM='" + pwd + "' WHERE DLYX='" + uid + "'";//要被执行的更新语句
        if (typeToSet == "email")
        {
            if (Convert.ToDateTime(dtValEmail.Rows[0]["CreateTime"]).AddDays(1) > DateTime.Now)
            {
                if (dtValEmail.Rows[0]["emailValNumber"].ToString() != valNum.Trim())
                {
                    dtFalse.Rows.Add(new object[] { uid, phoneoremail, "", "", "验证码不匹配" });
                    ds.Tables.Clear();
                    ds.Tables.Add(dtFalse);
                }
                else
                {
                    int i = DbHelperSQL.ExecuteSql(UpdateSql);
                    if (i == 1)
                    {
                        dtSuccess.Rows.Add(new object[] { uid, phoneoremail, "", pwd, "成功" });
                        ds.Tables.Clear();
                        ds.Tables.Add(dtSuccess);
                    }
                    else
                    {
                        ds.Tables.Clear();
                        dtFalse.Rows.Add(new object[] { uid, phoneoremail, "", "", "服务器繁忙，请稍后再试" });
                        ds.Tables.Add(dtFalse);
                    }
                }
            }
            else
            {
                dtFalse.Rows.Add(new object[] { uid, phoneoremail, "", "", "验证失败，您的邮箱验证码已失效，已重新发送验证邮件，请重新输入验证码！" });
                ds.Tables.Clear();
                ds.Tables.Add(dtFalse);
            }
        }
        if (typeToSet == "phone")
        {
            if (dtValPhone.Rows[0]["phoneValNumber"].ToString() != valNum.Trim())
            {
                dtFalse.Rows.Add(new object[] { uid, "", phoneoremail, "", "验证码不匹配" });
                ds.Tables.Clear();
                ds.Tables.Add(dtFalse);
            }
            else
            {
                int i = DbHelperSQL.ExecuteSql(UpdateSql);
                if (i == 1)
                {
                    dtSuccess.Rows.Add(new object[] { uid, "", phoneoremail, pwd, "成功" });
                    ds.Tables.Clear();
                    ds.Tables.Add(dtSuccess);
                }
                else
                {
                    ds.Tables.Clear();
                    dtFalse.Rows.Add(new object[] { uid, "", phoneoremail, "", "服务器繁忙，请稍后再试" });
                    ds.Tables.Add(dtFalse);
                }
            }
        }
        return ds;
    }

    /// <summary>
    /// 找回密码（验证用户信息、发送验证码）
    /// </summary>
    /// <param name="user">登录邮箱或手机号码</param>
    /// <param name="typeToSet">找回方式</param>
    /// <returns>Dataset（保存发送信息、验证码等）</returns>
    [WebMethod]
    public DataSet SendValNumber(string user, string typeToSet,string RandomNumber)
    {
        DataSet ds = new DataSet();
        if (typeToSet == "email")
        {
            string email = user;
            DataTable dt_old = DbHelperSQL.Query("SELECT *,'' AS 查询信息,'' AS 本次验证码 FROM ZZ_UserLogin WHERE DLYX='" + user + "' AND SFYZYX='是'").Tables[0];
            DataTable dt = dt_old.Copy();
            dt.TableName = "帐号信息";
            ds.Tables.Add(dt);
            if (dt.Rows.Count == 1)
            {
                dt.Rows[0]["查询信息"] = "正常";
                string Randoms = RandomNumber;//验证码
                string ip = GetIP();//Ip地址
                string time = DateTime.Now.ToString("yyyy-MM-dd");//时间
                string types = typeToSet;//方式
                string isUsed = "false";
                string InsertSqls = "INSERT INTO ZZ_UserValiNumbers(subType,userEmail,emailValNumber,phoneValNumber,ipAddress,isUsed,CreateTime) VALUES('" + types + "','" + email + "','" + Randoms + "','" + " " + "','" + ip + "','" + isUsed + "',GETDATE())";//插入验证码表

                try
                {
                    int i = DbHelperSQL.ExecuteSql(InsertSqls);
                    if (i == 1)
                    {
                        string SelectSqls = "SELECT TOP 1 * FROM ZZ_UserValiNumbers WHERE userEmail='" + email + "' AND isUsed='false' ORDER BY CreateTime DESC";
                        DataTable dtVals_old = DbHelperSQL.Query(SelectSqls).Tables[0];
                        DataTable dtVals = dtVals_old.Copy();
                        dtVals.TableName = "验证码信息";
                        ds.Tables.Add(dtVals);
                        ClassEmail CE = new ClassEmail();
                        string strSubject = "中国商品批发交易平台 - 找回密码";
                        string modHtmlFileName = HttpContext.Current.Server.MapPath("~/JHJXPT/EmailTemplate_Repwd.htm");
                        Hashtable ht = new Hashtable();
                        ht["[[Email]]"] = email;
                        ht["[[valNumbs]]"] = Randoms;
                        ht["[[DateTime]]"] = time;
                        CE.SendSPEmail(email, strSubject, modHtmlFileName, ht);
                        dt.Rows[0]["本次验证码"] = Randoms;
                    }
                }
                catch(Exception ex)
                {
                    ;
                }
            }
            else
            {
                int i = dt.Columns.Count;
                ArrayList al = new ArrayList();
                for (int j = 0; j < i; j++)
                {
                    al.Add(" ");  
                }
                al[i-1] = "无此用户";
                dt.Rows.Add(new object[] { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, "无此用户", null });
            }
        }
        if (typeToSet == "phone")
        {
            string phone = user;//传过来的手机号
            string uid = GetEmailByPhone(phone);//根据手机号得到的登录Email
            DataTable dt_old = DbHelperSQL.Query("SELECT *,'' AS 查询信息,'' AS 本次验证码 FROM ZZ_UserLogin WHERE DLYX='" + uid + "'").Tables[0];
            DataTable dt = dt_old.Copy();
            dt.TableName = "帐号信息";
            ds.Tables.Add(dt);
            //如果存在这个帐号
            if (uid.Trim() != "")
            {
                dt.Rows[0]["查询信息"] = "正常";
                string Randoms = RandomNumber;//验证码
                string ip = GetIP();
                string time = DateTime.Now.ToString("yyyy-MM-dd");
                string types = typeToSet;
                string isUsed = "false";
                string InsertSqls = "INSERT INTO ZZ_UserValiNumbers(subType,userEmail,emailValNumber,phoneValNumber,ipAddress,isUsed,CreateTime) VALUES('" + types + "','" + uid + "','" + " " + "','" + Randoms + "','" + ip + "','" + isUsed + "',GETDATE())";
                try
                {
                    int i = DbHelperSQL.ExecuteSql(InsertSqls);
                    if (i == 1)
                    {
                        string SelectSqls = "SELECT TOP 1 * FROM ZZ_UserValiNumbers WHERE userEmail='" + uid + "' AND isUsed='false' ORDER BY CreateTime DESC";
                        DataTable dtVals_old = DbHelperSQL.Query(SelectSqls).Tables[0];
                        DataTable dtVals = dtVals_old.Copy();
                        dtVals.TableName = "验证码信息";
                        ds.Tables.Add(dtVals);
                        string Msg = "亲爱的" + uid + "：您在中国商品批发交易平台申请了手机验证，您的验证码是：" + Randoms + "，验证码30分钟内有效！[中国商品批发交易平台]-";
                        object[] obj = { phone, Msg };
                        Key.WebServiceHelper.InvokeWebService("http://192.168.0.10:8183/MessageService.asmx", "SendMessage", obj);
                        dt.Rows[0]["本次验证码"] = Randoms;
                    }
                }
                catch
                {
                    ;
                }
            }
            //如果不存在这个帐号
            else
            {
                int i = dt.Columns.Count;
                ArrayList al = new ArrayList();
                for (int j = 0; j < i; j++)
                {
                    al.Add(" ");
                }
                al[i - 1] = "无此用户";
                dt.Rows.Add(new object[] { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, "无此用户", null });
            }
        }
        return ds;
    }





    /// <summary>
    /// 获取用户IP地址并更新到登陆信息表
    /// </summary>
    /// <param name="DLYX">用户登陆邮箱</param>
    /// <returns></returns>
    /// 本方法无法放到PulicClass下，需要用到HTTP的东西
    private string GetIP(string DLYX)
    {
        string IpAddress = "";
        if (Context.Request.ServerVariables["HTTP_VIA"] != null)//使用了代理
        {
            IpAddress = Context.Request.ServerVariables["HTTP_X_FORWAREDE_FOR"].ToString();
        }
        //没有使用代理或者得不到客户端IP，则取代理服务器IP地址。
        else
        {
            IpAddress = Context.Request.ServerVariables["REMOTE_ADDR"].ToString();
        }
        try
        {
            string UpdateSql = "UPDATE ZZ_UserLogin SET ZHYCDLIP='" + IpAddress + "',ZHYCDLSJ=GETDATE(),CheckLimitTime=GETDATE(),DLCS=DLCS+1 WHERE DLYX='" + DLYX + "'";
            DbHelperSQL.ExecuteSql(UpdateSql);
        }
        catch
        {
            throw;
        }
        return IpAddress;
    }
    /// <summary>
    /// 获取用户IP地址并更新到登陆信息表（不执行Sql）
    /// </summary>
    /// <returns>IP地址</returns>
    private string GetIP()
    {
        string IpAddress = "";
        if (Context.Request.ServerVariables["HTTP_VIA"] != null)//使用了代理
        {
            IpAddress = Context.Request.ServerVariables["HTTP_X_FORWAREDE_FOR"].ToString();
        }
        //没有使用代理或者得不到客户端IP，则取代理服务器IP地址。
        else
        {
            IpAddress = Context.Request.ServerVariables["REMOTE_ADDR"].ToString();
        }
        return IpAddress;
    }

    private string GetEmailByPhone(string phone)
    {
        string Email = "";
        string TableName="";
        TableName = "ZZ_BUYJBZLXXB";//买家
        string SelectSql1 = "SELECT DLYX FROM " + "ZZ_BUYJBZLXXB" + " WHERE SJH='" + phone.Trim() + "'";
        DataTable dt = DbHelperSQL.Query(SelectSql1).Tables[0];
        TableName = "ZZ_SELJBZLXXB";//卖家
        string SelectSql2 = "SELECT DLYX FROM " + "ZZ_SELJBZLXXB" + " WHERE SJH='" + phone.Trim() + "'";
        DataTable dt2 = DbHelperSQL.Query(SelectSql2).Tables[0];
        TableName = "ZZ_DLRJBZLXXB";//经纪人
        string SelectSql3 = "SELECT DLYX FROM " + "ZZ_DLRJBZLXXB" + " WHERE SJH='" + phone.Trim() + "'";
        DataTable dt3 = DbHelperSQL.Query(SelectSql3).Tables[0];
        DataSet ds = new DataSet();
        DataTable dtNew1 = dt.Copy(); dtNew1.TableName = "1";
        DataTable dtNew2 = dt2.Copy(); dtNew2.TableName = "2";
        DataTable dtNew3 = dt3.Copy(); dtNew3.TableName = "3";
        ds.Tables.Add(dtNew1);
        ds.Tables.Add(dtNew2);
        ds.Tables.Add(dtNew3);
        for (int i = 0; i < ds.Tables.Count; i++)
        {
            if (ds.Tables[i].Rows.Count > 0)
            {
                Email = ds.Tables[i].Rows[0]["DLYX"].ToString();
                break;
            }
        }
        return Email;
    }
    /// <summary>
    /// 根据表明获取商品分类
    /// </summary>
    /// <param name="sTable"></param>
    /// <returns></returns>
    [WebMethod]
    public DataSet test_dsfl(string sTable)
    {
        DataSet DataSet_this = new DataSet();
        Hashtable return_ht = new Hashtable();
        Hashtable putin_ht = new Hashtable();
        putin_ht["sField"] = "SortID,SortName,SortParentID,SortParentPath,SortOrder,GoToUrl,GoToUrlParameter,ShowWho,TargetGo,ToolTipText";
        putin_ht["sTable"] = sTable;
        putin_ht["iSortID"] = 0;
        putin_ht["iCond"] = 1;
        return_ht = I_DBL.RunProc_CMD("sp_Util_Sort_SELECT", DataSet_this, putin_ht);
        if (Convert.ToBoolean(return_ht["return_float"]))
        {
            DataSet_this = (DataSet)return_ht["return_ds"];
            if (DataSet_this != null)
            {
                return DataSet_this;

            }
            else
            {
                return null;
            }
        }
        else
        {
            //显示错误
            //MessageBox.Show(return_ht["return_errmsg"].ToString(), "数据库操作提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return null;

        }
    }

    private int getran(int a, int b)
    {
        System.Random x1 = new Random(DateTime.Now.Millisecond);
       
        return x1.Next(a, b);
    }

    /// <summary>
    /// 测试生成一些模拟数据
    /// </summary>
    /// <returns></returns>
    [WebMethod]
    public DataSet test_moni()
    {
        //标书号
        string str1 = getran(1000001, 9999901).ToString();
        //商品名称
        string[] arrmc = { "打印纸001", "打印纸002", "打印纸003", "打印纸004", "签字笔001", "签字笔002", "文件架001", "文件架002", "文件架003", "文件架004", "重金属合金001", "重金属合金002", "重金属合金003", "重金属合金004", "棉花001", "棉花002", "棉花003", "棉花004", "棉花005", "棉花006", "棉花007", "济南郊区大白菜001", "济南郊区大白菜002", "济南郊区大白菜003", "济南郊区大白菜004", "济南郊区大白菜005", "济南郊区大白菜006" };
        string str2 = arrmc[getran(0, 25)];
        //商品规格
        string str3 = str2;
        //投标类型
        string[] arrmc2 = { "半年度", "半年度", "年度", "定量定价", "定量定价" };
        string str4 = arrmc2[getran(0, 4)];
        //当前价格
        string str5 = getran(20, 9000).ToString() + "." + getran(10, 99).ToString();
        //计价单位
        string[] arrmc3 = { "车", "箱", "个", "支", "瓶", "打", "车" };
        string str6 = arrmc3[getran(0, 6)];
        //达成率
        string str7 = getran(5, 99).ToString() + "." + getran(10, 99).ToString() + "%";
        //中标最低预定量
        string str8 = getran(5000, 999999).ToString();
        //当前集合预定量
        string str9 = getran(5000, 999999).ToString();
        //最小经济批量
        string str10 = getran(200, 999).ToString();
        //集合竞价剩余时间
        string str11 = getran(2, 30).ToString();
        //当前买家数量
        string str12 = getran(200, 9999).ToString();
        //当前卖家数量
        string str13 = getran(200, 9999).ToString();
        //供货起始时间
        string[] arrmc5 = { "定标后", "2012-10-02", "2012-11-02", "2012-01-05", "2013-06-30", "2013-12-01", "定标后", "定标后", "定标后", "定标后" };
        string str14 = arrmc5[getran(0, 9)];
        //供货截止时间
        string[] arrmc6 = { "定标后", "2012-10-02", "2012-11-02", "2012-01-05", "2013-06-30", "2013-12-01", "定标后", "定标后", "定标后", "定标后" };
        string str15 = arrmc6[getran(0, 9)];

        //写入数据库
        DbHelperSQL.ExecuteSql("INSERT INTO [dbo].[ZZZ_test] ([str1],[str2],[str3],[str4] ,[str5],[str6],[str7],[str8],[str9],[str10] ,[str11],[str12],[str13],[str14] ,[str15]) VALUES ('" + str1 + "','" + str2 + "','" + str3 + "','" + str4 + "','" + str5 + "','" + str6 + "','" + str7 + "','" + str8 + "','" + str9 + "','" + str10 + "','" + str11 + "','" + str12 + "','" + str13 + "','" + str14 + "','" + str15 + "')");
        return null;
        
    }


    /// <summary>
    /// 测试处理一个表单
    /// </summary>
    /// <param name="str1">模拟参数</param>
    /// <returns></returns>
    [WebMethod]
    public string test_save_or_update(string str1)
    {
        //加锁
        if (jhjx_Lock.LockBegin_ByUser("全局锁") != "1")
        {
            return "系统繁忙"; //取消业务处理
        }

        //验证表单
        Thread.Sleep(6000);//模拟长时间处理
        //生成SQl语句数组

        //执行语句


        //解锁
        jhjx_Lock.LockEnd_ByUser("全局锁");

        //返回处理结果
        return "ok";
    }

    #region 基础操作，获取数据源返回dataset，删除服务器上面的图片
    /// <summary>
    /// 根据SQL语句获取一个DataSet
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    [WebMethod]
    public DataSet GetDataSet(object obj)
    {
        if (obj.ToString() != null && obj.ToString() != "")
        {
            return DbHelperSQL.Query(obj.ToString());
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// 删除已经上传的图片
    /// </summary>
    /// <param name="path"></param>
    [WebMethod]
    public void DelPicture(string p)
    {
        string path = HttpContext.Current.Server.MapPath("/JHJXPT/SaveDir/" + p.Trim());
        System.IO.File.Delete(path);

    }
    #endregion

    #region 开票信息处理的相关方法
    [WebMethod]
    //添加新的发票信息
    public string fp_add(object obj)
    {
 

        //执行语句  
        Manage_Receipt mr = new Manage_Receipt();
        string result = mr.AddReceipt(obj);
 

        //返回处理结果
        return result;
    }

    [WebMethod]
    //修改发票信息

    public string fp_edit(object obj)
    {
 

        //执行语句  
        Manage_Receipt mr = new Manage_Receipt();
        string result = mr.EditReceipt(obj);

 

        //返回处理结果
        return result;
    }
    #endregion

    #region  买家卖家关联经纪人相关方法
    [WebMethod]
    public DataSet jjr_save(DataSet obj)
    {
        //执行语句  
        Manage_Agent ma = new Manage_Agent();
 
       
        DataSet result = ma.AgentAdd(obj);

 

        //返回处理结果
        return result;
    }

    [WebMethod]
    public DataSet jjr_set(DataSet obj)
    {
        //执行语句
        Manage_Agent ma = new Manage_Agent();
 

       
        DataSet result = ma.AgentSet(obj);

 

        //返回处理结果
        return result;
    }
    [WebMethod]
    public DataSet jjr_Resubmit(DataSet ds)
    {
        //执行语句
        Manage_Agent ma = new Manage_Agent();
 


        DataSet result = ma.Resubmit(ds);

 

        //返回处理结果
        return result;
    } 


    #endregion

    #region  收款账户维护的相关方法
    /// <summary>
    /// 设置默认收款账户
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    [WebMethod]
    public string skzh_set(object obj)
    {
 
        //执行语句
        Manage_Acount ma = new Manage_Acount();
        string result = ma.SetAcount(obj);

 

        //返回处理结果
        return result;
    } 
   
    /// <summary>
    /// 添加收款账户
    /// </summary>
    /// <param name="array"></param>
    /// <returns></returns>
    [WebMethod]
    public string skzh_add(object obj)
    {
 
        
        //执行语句
        Manage_Acount ma = new Manage_Acount();
        string result = ma.AddAcount(obj) ;  

 

        //返回处理结果
        return result;
    }

    /// <summary>
    /// 更新现有收款账号
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    [WebMethod]
    public string skzh_edit(object obj)
    {
 

        //执行语句
        Manage_Acount ma = new Manage_Acount();
        string result = ma.EditAcount(obj);

 

        //返回处理结果
        return result;
    }
    /// <summary>
    /// 删除收款账户
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    [WebMethod]
    public string skzh_delete(object obj)
    {
 

        //执行语句
        Manage_Acount ma = new Manage_Acount();
        string result = ma.DeleteAcount(obj);
 

        //返回处理结果
        return result;
    }
    #endregion

    #region  收货地址维护的相关方法

    /// <summary>
    /// 设置为默认收货地址
    /// </summary>
    /// <param name="number">设为默认的地址信息编号</param>
    /// <param name="dlyx">操作者的登陆邮箱账号</param>
    /// <returns></returns>
    [WebMethod]
    public string Set_MrAddress(string number, string dlyx)
    {
 

        //验证表单
        Thread.Sleep(10000);//模拟长时间处理        

        //执行语句       
        Manage_Address md = new Manage_Address();
        string retrunstr = md.Set_MrAddress(number, dlyx);
 

        return retrunstr;

    }
    /// <summary>
    /// 设置为默认收货地址
    /// </summary>
    /// <param name="number">设为默认的地址信息编号</param>
    /// <param name="dlyx">操作者的登陆邮箱账号</param>
    /// <returns></returns>
    [WebMethod]
    public string Del_Address(string number, string dlyx)
    {
 

        //验证表单
        Thread.Sleep(500);//模拟长时间处理        

        //执行语句        
        Manage_Address md = new Manage_Address();
        string str = md.Del_Address(number, dlyx);
 
        return str;
    }

    /// <summary>
    /// 设置为默认收货地址
    /// </summary>
    /// <param name="number">设为默认的地址信息编号</param>
    /// <param name="dlyx">操作者的登陆邮箱账号</param>
    /// <returns></returns>
    [WebMethod]
    public string[] Save_Address(string[][]  InputStr)
    {
        string[] result = new string[2];

 

        //验证表单
        Thread.Sleep(500);//模拟长时间处理        

        //执行语句        
        Manage_Address md = new Manage_Address();
        result = md.Save_Address(InputStr);      

 
        return result;
        
    }

    /// <summary>
    /// 设置为默认收货地址
    /// </summary>
    /// <param name="number">设为默认的地址信息编号</param>
    /// <param name="dlyx">操作者的登陆邮箱账号</param>
    /// <returns></returns>
    [WebMethod]
    public DataSet Get_EditAddress(string number,string dlyx)
    {
        DataSet ds = DbHelperSQL.Query ("select '' as 执行状态,'' as 执行结果,* from ZZ_SHDZXXB where 1!=1");

 

        //验证表单
        Thread.Sleep(500);//模拟长时间处理        

        //执行语句        
        Manage_Address md = new Manage_Address();
        ds = md.Get_EditAddress(number,dlyx);

 
        return ds;
    }
    #endregion

    /// <summary>
    /// 判断是否在营业时间内
    /// </summary>
    /// <returns></returns>
    [WebMethod]
    public string IsOnOpenTime()
    {
 

        //验证表单
        Thread.Sleep(500);//模拟长时间处理        

        //执行语句      
        string str = jhjx_PublicClass.IsOpenTime()?"true":"false";
 
        return str;
    }
    /// <summary>
    /// 判断是否在营业日期内
    /// </summary>
    /// <returns></returns>
    [WebMethod]
    public string IsOnOpenDay()
    {
 

        //验证表单
        Thread.Sleep(500);//模拟长时间处理        

        //执行语句      
        string str = jhjx_PublicClass.IsOpenDay() ? "true" : "false";
 
        return str;
    }


    /// <summary>
    /// 结算账户注册数据执行(wyh,2013.01.18)
    /// </summary>
    /// <param name="dt">执行参数</param>
    /// <param name="Type">执行类别 insert:插入，update：更新</param>
    /// <returns>返回执行信息</returns>
    [WebMethod]
    public string jszhzx(DataTable dt,string Type)
    {
        //加锁 
        string str = "";
 

            //验证表单
            //执行语句  
            Thread.Sleep(500);
            Manage_Jszhchuli mj = new Manage_Jszhchuli();
            str = mj.isDOPass(dt, Type);
      

        return str;
    }



    /// <summary>
    /// 获取经纪人信息
    /// </summary>
    /// <param name="Strjjrjsbh">经纪人角色编号</param>
    /// <returns></returns>
    [WebMethod]
    public DataSet GetjjrState(string Strjjrjsbh)
    {
        //加锁 
        Manage_Jszhchuli maj = new Manage_Jszhchuli();

        return maj.Getjjrjinfo(Strjjrjsbh);
        
    }



    /// <summary>
    /// 设置为默认收货地址
    /// </summary>
    /// <param name="number">设为默认的地址信息编号</param>
    /// <param name="dlyx">操作者的登陆邮箱账号</param>
    /// <returns></returns>
    [WebMethod]
    public DataSet Get_BasicInfo(string dlyx, string jsbh)
    {
        DataSet ds = new DataSet(); 

        //加锁
        //if (jhjx_Lock.LockBegin_ByUser("全局锁") != "1")
        //{
            //无论是否成功，都要加入这个执行结果表，并且要确保这个表中有且只有一条数据
            //DataTable objTable = new DataTable("执行情况");
            //objTable.Columns.Add("执行状态", typeof(string));
            //objTable.Columns.Add("执行结果", typeof(string));
            //ds.Tables.Add(objTable);      
            //ds.Tables ["执行情况"].Rows .Add (new object []{"系统繁忙","系统繁忙"});           
            //return ds;
        //}

        //验证表单
        Thread.Sleep(500);//模拟长时间处理        

        //执行语句        
        Manage_Jszhchuli md = new Manage_Jszhchuli();
        ds = md.Get_BasicInfo(dlyx, jsbh);

        //解锁
        //jhjx_Lock.LockEnd_ByUser("全局锁");
        return ds;
    }

    /// <summary>
    /// 根据登录邮箱，判断当前账户是否已开通结算账户
    /// </summary>
    /// <returns></returns>
    [WebMethod]
    public DataSet GetSelSHZT(string user_mail)
    {
        DataSet ds = DbHelperSQL.Query("select top 1 '系统繁忙'='', * from ZZ_YHJSXXB where 1!=1");
 

        //验证表单
        Thread.Sleep(500);//模拟长时间处理        

        //执行语句      
        ds = jhjx_PublicClass.GetSelSHZT(user_mail);
 
        return ds;
    }
    /// <summary>
    /// 根据登陆账号，判断卖家结算账户默认的经纪人是否暂停新业务
    /// </summary>
    /// <param name="user_mail">用户邮箱</param>
    /// <returns></returns>
    [WebMethod]
    public string GetIsZTXYW(string user_mail)
    {
 

        //验证表单
        Thread.Sleep(500);//模拟长时间处理        

        //执行语句      
        string str = jhjx_PublicClass.GetIsZTXYW(user_mail);
 
        return str;
    }
    /// <summary>
    /// 根据登陆账号，得到当前可用余额
    /// </summary>
    /// <param name="user_mail">用户邮箱</param>
    /// <returns></returns>
    [WebMethod]
    public string GetZHDQKYYE(string user_mail)
    {
 

        //验证表单
        Thread.Sleep(500);//模拟长时间处理        

        //执行语句      
        string str = jhjx_PublicClass.GetZHDQKYYE(user_mail);
 
        return str;
    }
    /// <summary>
    /// 获取所有一级商品分类
    /// </summary>
    /// <returns></returns>
    [WebMethod]
    public DataSet GetSatrtSPFL()
    {
        DataSet ds = DbHelperSQL.Query("select '状态'='',* from dbo.ZZ_tbMenuSPFL where 1!=1");
 

        //验证表单
        Thread.Sleep(500);//模拟长时间处理        

        //执行语句      
        ds = jhjx_PublicClass.GetSatrtSPFL();
 
        return ds;
    }
    /// <summary>
    /// 根据选中的商品名称，获取子类商品
    /// </summary>
    /// <param name="sortName">商品名称</param>
    /// <returns></returns>
    [WebMethod]
    public DataSet GetChildSPFL(string sortName)
    {
        DataSet ds = DbHelperSQL.Query("select '状态'='',* from dbo.ZZ_tbMenuSPFL where 1!=1");
 

        //验证表单
        Thread.Sleep(500);//模拟长时间处理        

        //执行语句      
        ds = jhjx_PublicClass.GetChildSPFL(sortName);
 
        return ds;
    }
    /// <summary>
    /// 根据商品分类的名称，得到此商品分类下的SortID
    /// </summary>
    /// <param name="StartSPFL">一级商品名称</param>
    /// <param name="SecondSPFL">二级商品名称</param>
    /// <param name="ThreeSPFL">三级商品名称</param>
    /// <returns></returns>
    [WebMethod]
    public string GetSPXXSortID(string StartSPFL, string SecondSPFL, string ThreeSPFL)
    {       
 

        //验证表单
        Thread.Sleep(500);//模拟长时间处理        

        //执行语句      
        string str = jhjx_PublicClass.GetSPXXSortID(StartSPFL, SecondSPFL, ThreeSPFL);
 
        return str;
    }

    #region//更新买、卖家基本信息资料表

    /// <summary>
    /// 根结角色编号、得到买家、卖家的审核信息
    /// </summary>
    /// <param name="jsbh">角色编号</param>
    /// <returns></returns>
    [WebMethod]
    public DataSet ReturnMMJBascData(string jslx,string  jsbh,string jjrjsbh)
    {
        DataSet ds = new DataSet();

 
            //无论是否成功，都要加入这个执行结果表，并且要确保这个表中有且只有一条数据
            DataTable objTableBuyer = new DataTable("买家数据表");
            objTableBuyer.Columns.Add("执行状态", typeof(string));
            objTableBuyer.Columns.Add("执行结果", typeof(string));
            ds.Tables.Add(objTableBuyer);
            ds.Tables["买家数据表"].Rows.Add(new object[] { "系统繁忙", "系统繁忙" });

            DataTable objTableSeller = new DataTable("卖家数据表");
            objTableSeller.Columns.Add("执行状态", typeof(string));
            objTableSeller.Columns.Add("执行结果", typeof(string));
            ds.Tables.Add(objTableBuyer);
            ds.Tables["卖家数据表"].Rows.Add(new object[] { "系统繁忙", "系统繁忙" });
            return ds;
    

        //验证表单
        Thread.Sleep(500);//模拟长时间处理        

        //执行语句        
        ds = Manage_SelerBuyerData.GetSelerAndBuyerData(jslx, jsbh, jjrjsbh);

      
        return ds;
    }
    /// <summary>
    /// 审核买家基本信息 【审核通过】
    /// </summary>
    /// <param name="dataTable"></param>
    /// <returns></returns>
    [WebMethod]
    public string[] VerifyBuyerDataPass(DataTable dataTable)
    {
        string[] result = new string[2];

 

        //验证表单
        Thread.Sleep(500);//模拟长时间处理        

        //执行语句   
        result = Manage_SelerBuyerData.VerifyBuyerDataPass(dataTable);
 
        return result;
    }
    /// <summary>
    /// 审核买家基本信息 【驳回】
    /// </summary>
    /// <param name="dataTable"></param>
    /// <returns></returns>
    [WebMethod]
    public string[] VerifyBuyerDataReject(DataTable dataTable)
    {
        string[] result = new string[2];

 
        //验证表单
        Thread.Sleep(500);//模拟长时间处理        

        //执行语句   
        result = Manage_SelerBuyerData.VerifyBuyerDataReject(dataTable);
  
        return result;
    }

    /// <summary>
    /// 审核卖家基本信息 【审核通过】
    /// </summary>
    /// <param name="dataTable"></param>
    /// <returns></returns>
    [WebMethod]
    public string[] VerifySellerDataPass(DataTable dataTable)
    {
        string[] result = new string[2];

 
        //验证表单
        Thread.Sleep(500);//模拟长时间处理        

        //执行语句   
        result = Manage_SelerBuyerData.VerifySellerDataPass(dataTable);
 
        return result;
    }
    /// <summary>
    /// 审核卖家基本信息【驳回】
    /// </summary>
    /// <param name="dataTable"></param>
    /// <returns></returns>
    [WebMethod]
    public string[] VerifySellerDataReject(DataTable dataTable)
    {
        string[] result = new string[2];

  
        //验证表单
        Thread.Sleep(500);//模拟长时间处理        

        //执行语句   
        result = Manage_SelerBuyerData.VerifySellerDataReject(dataTable);
 
        return result;
    
    
    }


    #endregion

    #region//结算账户 审核、驳回后 发送邮件 发送短信
    /// <summary>
    /// 审核成功后，发送邮件  发送短信
    /// </summary>
    /// <param name="dataTable"></param>
    /// <returns></returns>
    [WebMethod]
    public string[] SendVerifyPassEmailPhone(DataTable dataTable)
    {
        string[] strArray = new string[2];

        try
        {

            //发送电子邮件
            ClassEmail CE = new ClassEmail();
            string strSuject = "中国商品批发交易平台-资料审核";
            string modHtmlFileName = HttpContext.Current.Server.MapPath("~/JHJXPT/JSZHVerifySuccessEmailTemplate.htm");
            Hashtable ht = new Hashtable();
            ht["[[yhm]]"] = dataTable.Rows[0]["用户名"];
            ht["[[jszh]]"] = dataTable.Rows[0]["结算账户"];
            ht["[[DateTime]]"] = dataTable.Rows[0]["发送时间"];
            CE.SendSPEmail(dataTable.Rows[0]["登录邮箱"].ToString(), strSuject, modHtmlFileName, ht);
          
            ////发送短信
            string strMess = "您的资料已经通过审核，可正常使用" + dataTable.Rows[0]["结算账户"].ToString() + "。[中国商品批发交易平台]";
            object[] ob = { dataTable.Rows[0]["手机号"].ToString(), strMess };
            object return_ob = Key.WebServiceHelper.InvokeWebService("http://192.168.0.10:8183/MessageService.asmx", "SendMessage", ob);

            strArray[0] = "ok";
            strArray[1] = "审核信息保存成功！";
            return strArray;
        }
        catch (Exception ex)
        {
            strArray[0] = "失败";
            strArray[1] = "审核信息保存失败！";
            return strArray;
        }
    }
    /// <summary>
    /// 审核失败后，发送的邮件 发送短信
    /// </summary>
    /// <param name="dataTable"></param>
    /// <returns></returns>
    [WebMethod]
    public string[] SendVerifyRejectEmailPhone(DataTable dataTable)
    {
        string[] strArray = new string[2];

        try
        {
            ClassEmail CE = new ClassEmail();
            string strSuject = "中国商品批发交易平台-资料审核";
            string modHtmlFileName = HttpContext.Current.Server.MapPath("~/JHJXPT/JSZHVerifyFailedEmailTemplate.htm");
            Hashtable ht = new Hashtable();
            ht["[[yhm]]"] = dataTable.Rows[0]["用户名"];
            ht["[[reason]]"] = dataTable.Rows[0]["驳回缘由"];
            ht["[[DateTime]]"] = dataTable.Rows[0]["发送时间"];
            CE.SendSPEmail(dataTable.Rows[0]["登录邮箱"].ToString(), strSuject, modHtmlFileName, ht);

            ////发送短信
            string strMess = "[" + dataTable.Rows[0]["用户名"].ToString() + "]的资料经纪人审核失败，请修改后重新提交。[中国商品批发交易平台]";
            object[] ob = { dataTable.Rows[0]["手机号"].ToString(), strMess };
            object return_ob = Key.WebServiceHelper.InvokeWebService("http://192.168.0.10:8183/MessageService.asmx", "SendMessage", ob);

            strArray[0] = "ok";
            strArray[1] = "审核信息保存成功！";
            return strArray;
        }
        catch (Exception ex)
        {
            strArray[0] = "失败";
            strArray[1] = "审核信息保存失败！";
            return strArray;
        }
    }



    #endregion

    /// <summary>
    /// 得到所有商品分类，有卖家、买家区分
    /// </summary>
    /// <param name="jszh">"卖家"、"买家"</param>
    /// <returns></returns>
    [WebMethod(Description = "得到所有商品分类，有卖家、买家区分")]
    public DataSet GetSPFL(string jszh)
    {
        DataSet PublisDsSPFL = new DataSet();
        DataSet dsStartSPFL = DbHelperSQL.Query("select SortID 商品编号,SortName 商品名称,SortParentID 所属商品编号 from dbo.ZZ_tbMenuSPFL where SortParentID='0'");
        dsStartSPFL.Tables[0].TableName = "一级分类";
        
        DataSet dsSecondSPFL = DbHelperSQL.Query("select a.商品名称,a.商品编号,b.所属商品名称,a.所属商品编号 from (select SortID 商品编号,SortName 商品名称,SortParentID 所属商品编号 from dbo.ZZ_tbMenuSPFL where SortParentID in(select SortID from ZZ_tbMenuSPFL where SortParentID='0')) as a left join (select SortName 所属商品名称,SortID 所属商品编号 from ZZ_tbMenuSPFL where SortID in(select SortParentID from dbo.ZZ_tbMenuSPFL where SortParentID in(select SortID from ZZ_tbMenuSPFL where SortParentID='0'))) as b on a.所属商品编号=b.所属商品编号");
        dsSecondSPFL.Tables[0].TableName = "二级分类";
        DataSet dsThreeSPFL;
        if (jszh.Trim() == "卖家")
        {
            dsThreeSPFL = DbHelperSQL.Query("select a.商品名称,a.商品编号,b.所属商品名称,a.所属商品编号 from (select SortID 商品编号,SortName 商品名称,SortParentID 所属商品编号 from ZZ_tbMenuSPFL where SortParentID in( select SortID from dbo.ZZ_tbMenuSPFL where SortParentID in(select SortID from ZZ_tbMenuSPFL where SortParentID='0'))) as a left join (select SortName 所属商品名称,SortID 所属商品编号 from ZZ_tbMenuSPFL where SortID in( select SortParentID from ZZ_tbMenuSPFL where SortParentID in( select SortID from dbo.ZZ_tbMenuSPFL where SortParentID in(select SortID from ZZ_tbMenuSPFL where SortParentID='0')))) as b on a.所属商品编号=b.所属商品编号");
        }
        else
        {
            dsThreeSPFL = DbHelperSQL.Query("select a.商品名称,a.商品编号,b.所属商品名称,a.所属商品编号 from (select SortID 商品编号,SortName 商品名称,SortParentID 所属商品编号 from ZZ_tbMenuSPFL where SortParentID in( select SortID from dbo.ZZ_tbMenuSPFL where SortParentID in(select SortID from ZZ_tbMenuSPFL where SortParentID='0'))) as a left join (select SortName 所属商品名称,SortID 所属商品编号 from ZZ_tbMenuSPFL where SortID in( select SortParentID from ZZ_tbMenuSPFL where SortParentID in( select SortID from dbo.ZZ_tbMenuSPFL where SortParentID in(select SortID from ZZ_tbMenuSPFL where SortParentID='0')))) as b on a.所属商品编号=b.所属商品编号 where a.所属商品编号 in(select distinct SPBH from ZZ_TBXXBZB where ZT='竞标')");
        }
        dsThreeSPFL.Tables[0].TableName = "三级分类";
        PublisDsSPFL.Tables.Add(dsStartSPFL.Tables[0].Copy());
        PublisDsSPFL.Tables.Add(dsSecondSPFL.Tables[0].Copy());
        PublisDsSPFL.Tables.Add(dsThreeSPFL.Tables[0].Copy());
        return PublisDsSPFL;
    }







    /// <summary>
    /// 添加一个新的投标信息
    /// </summary>
    /// <param name="DStbxx">投标信息</param>
    /// <returns></returns>
    [WebMethod]
    public DataSet FBTHXXSaveADD(DataSet DStbxx)
    {
        FBtb fbtb = new FBtb();
        
 

        //验证表单
        Thread.Sleep(500);//模拟长时间处理        

        //执行语句      
        
        DataSet dsreturn3 = fbtb.BeginFBtb(DStbxx);

 
        return dsreturn3;
    }
    /// <summary>
    /// 添加一个新的预订单
    /// </summary>
    /// <param name="DStbxx">投标信息</param>
    /// <returns></returns>
    [WebMethod]
    public DataSet XDYDDSaveADD(DataSet DStbxx)
    {
        
        SaveYDD ydd = new SaveYDD();
        //加锁
        if (jhjx_Lock.LockBegin_ByUser("全局锁") != "1")
        {
            DataSet dsreturn = ydd.initReturnDataSet();
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "系统繁忙";
            dsreturn.Tables["返回值单条"].Rows[0]["错误提示"] = "提交失败，系统繁忙，请稍后再试!";
            return dsreturn;
        }

        //验证表单
        Thread.Sleep(500);//模拟长时间处理        

        //执行语句      

        DataSet dsreturn3 = ydd.BegionSaveYDD(DStbxx);


        //解锁
        jhjx_Lock.LockEnd_ByUser("全局锁");
        return dsreturn3;
    }


    /// <summary>
    /// 执行中标监控
    /// </summary>
    /// <param name="DStbxx">商品</param>
    /// <returns></returns>
    [WebMethod]
    public string serverZBJK(DataTable DStbxx)
    {
        //加锁
        if (jhjx_Lock.LockBegin_ByUser("全局锁") != "1")
        {
            return "系统繁忙";
        }

        //验证表单
        Thread.Sleep(500);//模拟长时间处理        

        //执行语句      
        CJGZ cjgz = new CJGZ();
        ArrayList alsql = new ArrayList();
        try
        {
            alsql = cjgz.RunCJGZ(DStbxx, "中标监控");
            if (alsql != null && alsql.Count > 0)
            {
                DbHelperSQL.ExecuteSqlTran(alsql);
            }
        }
        catch (Exception ex)
        {
            return ex.ToString();
        }


        //解锁
        jhjx_Lock.LockEnd_ByUser("全局锁");
        if (alsql != null && alsql.Count > 0)
        {
            return "有中标数据产生";
        }
        return "无中标数据产生";
    }



    /// <summary>
    /// 撤销或修改一个投标信息
    /// </summary>
    /// <param name="Number">主表编号</param>
    /// <param name="id">子表编号</param>
    /// <param name="sp">撤销/修改</param>
    /// <param name="sl">修改的数量</param>
    /// <param name="jg">修改的单价</param>
    /// <returns></returns>
    [WebMethod]
    public string DEltbxx(string Number, string id,string sp,string sl,string jg,string jsbh)
    {
        //FBtb fbtb = new FBtb();

        //加锁
        if (jhjx_Lock.LockBegin_ByUser("全局锁") != "1")
        {
            return "系统繁忙";
        }

        //验证表单
        Thread.Sleep(500);//模拟长时间处理        
        FBtb fbtb = new FBtb();
        string re = fbtb.delOredit(Number, id, sp, sl, jg, jsbh);

        //解锁
        jhjx_Lock.LockEnd_ByUser("全局锁");
        return re;
    }
    /// <summary>
    /// 预订单信息表,修改、撤销预订单
    /// </summary>
    /// <param name="dt">修改的数据集，必须有的字段"修改的数量"、"修改的价格"、"预订单号"、"商品编号"、"合同周期"</param>
    /// <param name="cz">修改、撤销</param>
    /// <returns>返回修改或撤销的结果</returns>
    [WebMethod]
    public string UpdateOrDeleteYDD(DataSet dt)   
    {             
       
        //加锁
        if (jhjx_Lock.LockBegin_ByUser("全局锁") != "1")
        {
            return "系统繁忙";
        }

        //验证表单
        Thread.Sleep(500);//模拟长时间处理        
        UpdateYDD ydd = new UpdateYDD();
        //执行语句      
        string str = ydd.BegionUpdateYDD(dt);

        //解锁
        jhjx_Lock.LockEnd_ByUser("全局锁");
        return str;
    }








    /// <summary>
    /// 缴纳履约保证金
    /// </summary>
    /// <param name="zhubiao">投标信息主表编号</param>
    /// <param name="zibiao">投标信息子表编号</param>
    /// <param name="zibiao">角色编号</param>
    /// <returns>返回结果</returns>
    [WebMethod]
    public string PayLYHZJ(string zhubiao,string zibiao,string jsbh)
    {

        //加锁
        if (jhjx_Lock.LockBegin_ByUser("全局锁") != "1")
        {
            return "系统繁忙";
        }

        //验证表单
        Thread.Sleep(500);//模拟长时间处理        

        //执行语句    
        FBtb fbtb = new FBtb();
        string str = fbtb.PayLYHZJ_Run(zhubiao, zibiao, jsbh);

        //解锁
        jhjx_Lock.LockEnd_ByUser("全局锁");
        return str;
    }


    /// <summary>
    /// 获取提货单的一些基本信息，用于提交提货单
    /// </summary>
    /// <param name="Number">合同编号</param>
    /// <param name="jsbh">买家角色编号</param>
    /// <returns></returns>
    [WebMethod]
    public DataSet GetTHinfo(string Number, string jsbh)
    {
        DataSet dsreturn = new DataSet();
        THDclass thdclass = new THDclass();
        dsreturn = thdclass.GetTHinfoRun(Number, jsbh);
        return dsreturn;
    }

    /// <summary>
    /// 保存提货单
    /// </summary>
    /// <param name="dsinfo">提货单提交参数</param>
    /// <returns></returns>
    [WebMethod]
    public string AddTHD(DataSet dsinfo)
    {
        //加锁
        if (jhjx_Lock.LockBegin_ByUser("全局锁") != "1")
        {
            return "系统繁忙";
        }

        //验证表单
        Thread.Sleep(500);//模拟长时间处理        

        //执行语句    
        THDclass thdclass = new THDclass();
        string re = thdclass.SaveTHD(dsinfo);

        //解锁
        jhjx_Lock.LockEnd_ByUser("全局锁");
        return re;
    }




    /// <summary>
    /// 发货
    /// </summary>
    /// <param name="dsinfo">发货提交参数</param>
    /// <returns></returns>
    [WebMethod]
    public string UpdateFaHuo(DataSet dsinfo)
    {
        //加锁
        if (jhjx_Lock.LockBegin_ByUser("全局锁") != "1")
        {
            return "系统繁忙";
        }

        //验证表单
        Thread.Sleep(500);//模拟长时间处理        

        //执行语句    
        THDclass thdclass = new THDclass();
        string re = thdclass.FaHuo(dsinfo);

        //解锁
        jhjx_Lock.LockEnd_ByUser("全局锁");
        return re;
    }


    /// <summary>
    /// 签收
    /// </summary>
    /// <param name="Number">提货单号</param>
    /// <param name="jsbh">买家角色编号</param>
    /// <returns></returns>
    [WebMethod]
    public string UpdateQianShou(string Number, string jsbh)
    {
        //加锁
        if (jhjx_Lock.LockBegin_ByUser("全局锁") != "1")
        {
            return "系统繁忙";
        }

        //验证表单
        Thread.Sleep(500);//模拟长时间处理        

        //执行语句    
        THDclass thdclass = new THDclass();
        string re = thdclass.QianShou(Number, jsbh);

        //解锁
        jhjx_Lock.LockEnd_ByUser("全局锁");
        return re;
    }

    /// <summary>
    /// 根据废标数据，生成废标所需的款项处理语句
    /// </summary>
    /// <param name="FeiBiaoDS">废标数据</param>
    /// <returns></returns>
    [WebMethod]
    public string GetFeiBiaoSQL(DataSet FeiBiaoDS)
    {

        try
        {
            Hashtable htinfo = new Hashtable();
            ClassMoney CM = new ClassMoney();
            htinfo["废标数据"] = FeiBiaoDS;
            //要执行的SQL
            ArrayList alRun = new ArrayList();
            //客户端传来的语句
            for (int p = 0; p < FeiBiaoDS.Tables["update"].Rows.Count; p++)
            {
                alRun.Add(FeiBiaoDS.Tables["update"].Rows[p][0].ToString());
            }
            //资金相关其他语句
            ArrayList almoney = CM.InsertMoneyUse("废标", htinfo, "");
            DataTable dtbankmoney = (DataTable)(almoney[almoney.Count - 1]);
            almoney.RemoveAt(almoney.Count - 1);
            alRun.AddRange(almoney);

            bool bb = DbHelperSQL.ExecSqlTran(alRun);
            //bool bb = true;
            if (bb)
            {
                //用dtbankmoney处理银行的资金;
                for (int p = 0; p < dtbankmoney.Rows.Count; p++)
                {
                    //执行成功，调用银行接口，
                    CM.FreezeMoneyT(dtbankmoney.Rows[p]["邮箱"].ToString(), dtbankmoney.Rows[p]["增加金额"].ToString(), "+");
                }
                return "ok";
            }
            else
            {
                return "失败";
            }

        }
        catch (Exception ex)
        {
            return ex.ToString();
        }
 
        
    }




    /// <summary>
    /// 获取未读提醒
    /// </summary>
    /// <param name="DLYX">登陆邮箱</param>
    /// <returns></returns>
    [WebMethod]
    public string GetTrayMsg(string DLYX)
    {

        DataSet dsmsg = DbHelperSQL.Query("select * from ZZ_PTTXXXB where TXDXDLYX = '" + DLYX + "' and SFXSG = '否'");
        if (dsmsg != null && dsmsg.Tables[0].Rows.Count > 0)
        {
            string str = "";
            for (int i = 0; i < dsmsg.Tables[0].Rows.Count; i++)
            {
                str = str + (i + 1).ToString() + ".  " + dsmsg.Tables[0].Rows[i]["TXNRWB"].ToString() + "\r\n";
            }
            string shijian = DateTime.Now.ToString();
            DbHelperSQL.ExecuteSql("update ZZ_PTTXXXB set SFXSG = '是',XSSJ='" + shijian + "'  where TXDXDLYX = '" + DLYX + "' and SFXSG = '否'");
            return str;
        }
        else
        {
            return "";
        }

    }



    /// <summary>
    /// 通过银行接口，获取用户的可用额度
    /// </summary>
    /// <param name="DLYX">登陆邮箱</param>
    /// <returns></returns>
    [WebMethod]
    public string GetMyMoney(string DLYX, string bankinfo)
    {

 

        //验证表单
        Thread.Sleep(500);//模拟长时间处理        

        //执行语句    
        ClassMoney Cm = new ClassMoney();
        string re = "|ok|"+Cm.GetMoneyT(DLYX, "").ToString();

 
        return re;



    }



    /// <summary>
    /// 加入或删除自选商品
    /// </summary>
    /// <param name="DLYX">登陆邮箱</param>
    /// <returns></returns>
    [WebMethod]
    public string ZXSPedit(string DLYX, string SPBH, string edit)
    {

        //验证表单
        Thread.Sleep(500);//模拟长时间处理        

        //执行语句    
        HomePage HP = new HomePage();
        string re = HP.ZXSPedit_Run(DLYX, SPBH, edit);
        return re;
    }


    /// <summary>
    /// 根据以及分类获取二级商品分类
    /// </summary>
    /// <param name="SortParentID">父分类id</param>
    /// <returns></returns>
    [WebMethod]
    public DataSet GetSPFLerji(string SortParentID)
    {

        //执行语句    
        HomePage HP = new HomePage();
        DataSet dsfl = HP.GetSPFLerji_Run(SortParentID);
        return dsfl;
    }

    /// <summary>
    /// 停止或开启审核，或暂停与恢复选中用户的新业务，或仅获取状态而已。
    /// </summary>
    /// <param name="JJRjsbh">经纪人角色编号</param>
    /// <param name="JJRjsbh">经纪人登陆邮箱</param>
    /// <param name="type">操作类型， 新注册、新业务暂停、新业务恢复</param>
    /// <param name="BeSetDLYX">操作对象的登陆邮箱</param>
    /// <returns></returns>
    [WebMethod]
    public string[] JJRset(string JJRjsbh,string JJRDLYX,string type,string BeSetDLYX)
    {
        string[] rearr = { "err", "意外错误","type","zt" };
        //仅获取状态
        if (type == "仅状态")
        {
            DataSet ds = DbHelperSQL.Query("select top 1 DLRZTJSXYH from ZZ_YHJSXXB  where JSBH = '" + JJRjsbh + "'");
            rearr[0] = "ok";
            rearr[1] = ds.Tables[0].Rows[0][0].ToString();
            rearr[2] = type;
            rearr[3] = ds.Tables[0].Rows[0][0].ToString();
            return rearr;
        }
        //停止或开启审核 
        if (type == "新注册")
        {
            DataSet ds = DbHelperSQL.Query("select top 1 DLRZTJSXYH from ZZ_YHJSXXB  where JSBH = '" + JJRjsbh + "'");
            string sqlu = "";
            if (ds.Tables[0].Rows[0][0].ToString() == "否")
            {
                sqlu = "是";
            }
            else
            {
                sqlu = "否";
            }

            int num = DbHelperSQL.ExecuteSql("update ZZ_YHJSXXB set DLRZTJSXYH = '" + sqlu + "' where DLYX = '" + JJRDLYX + "'");
            DataSet dsn = DbHelperSQL.Query("select top 1 DLRZTJSXYH from ZZ_YHJSXXB  where JSBH = '" + JJRjsbh + "'");
       
                rearr[0] = "ok";
                rearr[2] = type;
                rearr[3] = dsn.Tables[0].Rows[0][0].ToString();
                if (ds.Tables[0].Rows[0][0].ToString() == "否")
                {
                    rearr[1] = "暂停新用户的审核成功！";
                }
                else
                {
                    rearr[1] = "恢复新用户的审核成功！";
                }
                

            return rearr;
        }
        //暂停与恢复选中用户的新业务 
        if (type == "新业务暂停")
        {
            int num = DbHelperSQL.ExecuteSql("update ZZ_MMJYDLRZHGLB set SFZTXYW = '是' where JSDLZH in (" + BeSetDLYX + ") and DLRDLZH = '" + JJRDLYX + "'");

                rearr[0] = "ok";
                rearr[1] = "暂停接受选中用户的新业务成功！";
                rearr[2] = type;
                rearr[3] = "";

        }
        if (type == "新业务恢复")
        {
            int num = DbHelperSQL.ExecuteSql("update ZZ_MMJYDLRZHGLB set SFZTXYW = '否' where JSDLZH in (" + BeSetDLYX + ") and DLRDLZH = '" + JJRDLYX + "'");
    
                rearr[0] = "ok";
                rearr[1] = "恢复接受选中用户的新业务成功！";
                rearr[2] = type;
                rearr[3] = "";
 
        }
        return rearr;
    }
    /// <summary>
    /// 新标准测试方法
    /// </summary>
    /// <param name="str1">参数1</param>
    /// <param name="str2">参数2</param>
    /// <returns>DataSet</returns>
    [WebMethod]
    public DataSet TestMethod(string str1, string str2)
    {
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        dt.Columns.Add("Str1");
        dt.Columns.Add("Str2");
        dt.Rows.Add(new object[] { str1, str2 });
        ds.Tables.Add(dt);
        return ds;
    }
}
