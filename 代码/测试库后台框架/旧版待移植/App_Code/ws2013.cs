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
using System.IO;
using System.Xml;
using Aspose.Words;
using Aspose.Words.Saving;
using System.Text;
using System.Reflection;

/// <summary>
/// ws2013 的摘要说明
/// </summary>
[WebService(Namespace = "http://www.fm8844.com/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
// [System.Web.Script.Services.ScriptService]
public class ws2013 : System.Web.Services.WebService
{

    //连接工厂接口 
    public Galaxy.ClassLib.DataBaseFactory.I_DBFactory I_DBF;
    //数据库连接接口
    public Galaxy.ClassLib.DataBaseFactory.I_Dblink I_DBL;

    public ws2013 () {

        //如果使用设计的组件，请取消注释以下行 
        //InitializeComponent(); 

        //初始化工厂
        AppSettingsReader hh = new AppSettingsReader();//调用web.Config中的数据库配置
        I_DBF = new Galaxy.ClassLib.DataBaseFactory.DBFactory();
        I_DBL = I_DBF.DbLinkSqlMain(ConfigurationManager.ConnectionStrings["FMOPConn"].ToString());
    }

    /// <summary>
    /// 初始化返回值数据集,执行结果只有两种ok和err
    /// </summary>
    /// <returns></returns>
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


    /// <summary>
    /// 监控异常
    /// </summary>
    /// <returns></returns>
    [WebMethod]
    public DataSet SupervisoryControlAbnormal(DataTable dataTable)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
        string strNumber = "1310000001";
        try
        {
            //执行语句      
            DataSet dstx = DbHelperSQL.Query("select YCFSSJ,RJYHSJ,YCXCBH  from AAA_JKXXSJB where Number='" + strNumber + "'");

            if (dstx != null && dstx.Tables[0].Rows.Count > 0)
            {
                string strSql = "";
                if (dstx.Tables[0].Rows[0]["YCFSSJ"].ToString() == "")//异常发生时间为Null值
                {
                    string[] strArray = dstx.Tables[0].Rows[0]["YCXCBH"].ToString().Split(new char[]{'|'},StringSplitOptions.RemoveEmptyEntries);
                    List<string> listArray = new List<string>();
                    listArray.AddRange(strArray);
                    string strYCXCBH="";
                    if (listArray.Contains(dataTable.Rows[0]["线程编号"].ToString()))
                    {
                        strYCXCBH = String.Join("|", listArray.ToArray());
                    }
                    else
                    {
                        listArray.Add(dataTable.Rows[0]["线程编号"].ToString());
                        strYCXCBH = String.Join("|", listArray.ToArray());
                    }
                    strSql = "update AAA_JKXXSJB set YCFSSJ=GETDATE(),RJYHSJ=GETDATE(),YCXCBH='" + strYCXCBH + "' where Number='" + strNumber + "'";
                }
                else
                {
                    string[] strArray = dstx.Tables[0].Rows[0]["YCXCBH"].ToString().Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                    List<string> listArray = new List<string>();
                    listArray.AddRange(strArray);
                    string strYCXCBH = "";
                    if (listArray.Contains(dataTable.Rows[0]["线程编号"].ToString()))
                    {
                        strYCXCBH = String.Join("|", listArray.ToArray());
                    }
                    else
                    {
                        listArray.Add(dataTable.Rows[0]["线程编号"].ToString());
                        strYCXCBH = String.Join("|", listArray.ToArray());
                    }
                    strSql = "update AAA_JKXXSJB set RJYHSJ=GETDATE(),YCXCBH='" + strYCXCBH + "' where Number='" + strNumber + "'";
                
                }

                DbHelperSQL.ExecuteSql(strSql);
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "监控记录信息更新完毕";
                return dsreturn;
            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未找到监控记录信息";
                return dsreturn;
            }

        }
        catch (Exception ex)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = ex.ToString();
            return dsreturn;
        }
    }



    /// <summary>
    /// 监控正常
    /// </summary>
    /// <returns></returns>
    [WebMethod]
    public DataSet SupervisoryControlNormal(DataTable dataTable)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
        string strNumner = "1310000001";
        try
        {
            //执行语句      
            DataSet dstx = DbHelperSQL.Query("select YCFSSJ,RJYHSJ,YCXCBH  from AAA_JKXXSJB where Number='" + strNumner + "'");
            if (dstx != null && dstx.Tables[0].Rows.Count > 0)
            {
                    string[] strArray = dstx.Tables[0].Rows[0]["YCXCBH"].ToString().Split(new char[]{'|'},StringSplitOptions.RemoveEmptyEntries);
                    List<string> listArray = new List<string>();
                    listArray.AddRange(strArray);
                    if (listArray.Count <= 0)//没有异常监控线程编号
                    {
                        DbHelperSQL.ExecuteSql("update AAA_JKXXSJB set YCFSSJ=null,RJYHSJ=GETDATE() where Number='" + strNumner + "'");
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "监控记录信息更新完毕";
                        return dsreturn;
                    }
                    else 
                    {
                        if (listArray.Contains(dataTable.Rows[0]["线程编号"].ToString()))
                        {
                            listArray.Remove(dataTable.Rows[0]["线程编号"].ToString());
                        }
                        DbHelperSQL.ExecuteSql("update AAA_JKXXSJB set YCXCBH='"+String.Join("|",listArray.ToArray())+"' where Number='" + strNumner + "'");
                        //重新读取数据信息
                        DataSet dstxAgin = DbHelperSQL.Query("select YCFSSJ,RJYHSJ,YCXCBH  from AAA_JKXXSJB where Number='" + strNumner + "'");
                        string[] strArrayAgin = dstxAgin.Tables[0].Rows[0]["YCXCBH"].ToString().Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                        List<string> listArrayAgin = new List<string>();
                        listArray.AddRange(strArrayAgin);
                        if (listArrayAgin.Count <= 0)
                        {
                            DbHelperSQL.ExecuteSql("update AAA_JKXXSJB set YCFSSJ=null,RJYHSJ=GETDATE() where Number='" + strNumner + "'");
                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "监控记录信息更新完毕";
                            return dsreturn;
                        }
                        else
                        {
                            DbHelperSQL.ExecuteSql("update AAA_JKXXSJB set RJYHSJ=GETDATE() where Number='" + strNumner + "'");
                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "监控记录信息更新完毕";
                            return dsreturn;
                        }
                    }

            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未找到监控记录信息";
                return dsreturn;
            }
        }
        catch (Exception ex)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = ex.ToString();
            return dsreturn;
        }
    }


    /// <summary>
    /// 检查右下角提醒
    /// </summary>
    /// <returns></returns>
    [WebMethod]
    public DataSet CheckFormTrayMsg(string dlyx)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        try
        {

            //检查是否要踢人
            //可以登陆时"ok"   要踢人时"提醒内容"
            string T_restr = Helper.GetOpen(dlyx.Trim());
            if (T_restr != "ok")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "要踢人";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = T_restr;
                return dsreturn;
            }

            //执行语句      
            DataSet dstx = DbHelperSQL.Query("select * FROM AAA_PTTXXXB WHERE TXDXDLYX='" + dlyx + "' and SFXSG = '否'");

            if (dstx != null && dstx.Tables[0].Rows.Count > 0)
            {
                DbHelperSQL.ExecuteSql("update AAA_PTTXXXB set SFXSG = '是',XSSJ=getdate() WHERE TXDXDLYX='" + dlyx + "'");
                dstx.Tables[0].TableName = "提醒数据";
                dsreturn.Tables.Add(dstx.Tables[0].Copy());
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "找到了提醒";
                return dsreturn;
            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未找到提醒";
                return dsreturn;
            }

        }
        catch (Exception ex)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = ex.ToString();
            return dsreturn;
        }
    }


    /// <summary>
    /// 可以提前获取的一些数据或参数
    /// </summary>
    /// <returns>返回数据集</returns>
    [WebMethod]
    public byte[] GetPublisDsData_YS()
    {
        byte[] b = Helper.DataSet2Byte(PublicClass2013.GetPublisDsData());
        return b;
    }

    /// <summary>
    /// 可以提前获取的一些数据或参数
    /// </summary>
    /// <returns>返回数据集</returns>
    [WebMethod]
    public DataSet GetPublisDsData()
    {
        return PublicClass2013.GetPublisDsData();
    }

    [WebMethod]
    public DataSet SayThat(DataSet ds)
    {
        ds.Tables[0].Rows[0]["param1"] = "I Will Say :" + ds.Tables[0].Rows[0]["param1"].ToString();
        return ds;
    }

    /// <summary>
    /// 执行导出
    /// </summary>
    /// <param name="SQL">语句</param>
    /// <param name="HideColumns">隐藏列</param>
    /// <returns></returns>
    [WebMethod]
    public DataSet CMyXls(string SQL, string[] HideColumns)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        DataSet ds = DbHelperSQL.Query(SQL);
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < HideColumns.Length; i++)
            {
                if (HideColumns[i].Trim() != "")
                {
                    ds.Tables[0].Columns.Remove(HideColumns[i]);
                }
            }
            ds.Tables[0].TableName = "导出数据集";
            dsreturn.Tables.Add(ds.Tables[0].Copy());
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "导出成功！";
            return dsreturn;
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "暂无数据！";
            return dsreturn;
        }
        
    }

    /// <summary>
    /// 执行标准演示
    /// </summary>
    /// <param name="test">参数</param>
    /// <returns>返回固定格式数据集</returns>
    [WebMethod]
    public DataSet demotest(string test)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err","初始化" });

        //加锁
        if (jhjx_Lock.LockBegin_ByUser("全局锁") != "1")
        {

            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统繁忙";
            return dsreturn;
        }
        //开始执行     
        democlass dc = new democlass();
        dsreturn = dc.RunDemo(dsreturn, test);
        //解锁
        jhjx_Lock.LockEnd_ByUser("全局锁");
        return dsreturn;
    }



    /// <summary>
    /// 执行中标监控
    /// </summary>
    /// <returns></returns>
    [WebMethod]
    public DataSet serverZBJK()
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        try
        {
            //找出超过出冷静期的数据
            DataSet DStbxx = DbHelperSQL.Query("select distinct '商品编号'=SPBH,'合同期限'=HTQX  from  AAA_LJQDQZTXXB where SFJRLJQ='是' and LJQJSSJ < getdate()  order  by SPBH desc");
            if (DStbxx == null || DStbxx.Tables[0].Rows.Count < 1)
             {
                 dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                 dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "没有过冷静期的数据!";
                 return dsreturn;
             }


            //执行语句      
            CJGZ2013 cjgz = new CJGZ2013();
            ArrayList alsql = new ArrayList();

            alsql = cjgz.RunCJGZ(DStbxx.Tables[0], "中标监控");
            bool ff = false;
            if (alsql != null && alsql.Count > 1)
            {
                ff = DbHelperSQL.ExecSqlTran(alsql);
            }


            if (alsql != null && alsql.Count > 1 && ff)
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = alsql[0].ToString().Replace("'", "").Replace("select", "").Trim();
                return dsreturn;
            }
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "无中标或冷静期操作执行！";
            return dsreturn;
        }
        catch (Exception ex)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = ex.ToString();
            return dsreturn;
        }
    }

    /// <summary>
    /// 执行即时交易监控
    /// </summary>
    /// <returns></returns>
    [WebMethod]
    public DataSet serverZBJK_JSJY()
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
        string errSQL = "";
        try
        {
            //执行语句      
            CJGZ2013 cjgz = new CJGZ2013();
            ArrayList alsql = new ArrayList();

            alsql = cjgz.RunCJGZ_JSJY();

           
            for (int i = 0; i < alsql.Count; i++)
            {
                errSQL = errSQL + alsql[i].ToString() + Environment.NewLine;
            }


            bool ff = false;
            if (alsql != null)
            {
                ff = DbHelperSQL.ExecSqlTran(alsql);
            }

            if (ff)
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = alsql[0].ToString().Replace("----", "").Replace(Environment.NewLine,"").Trim();
                return dsreturn;
            }
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "执行出错！";
            return dsreturn;
        }
        catch (Exception ex)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = ex.ToString() + "SQL:" + errSQL;
            return dsreturn;
        }
    }

    /// <summary>
    /// 预订单过期监控
    /// </summary>
    /// <returns></returns>
    [WebMethod]
    public DataSet serverGQJK()
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        try
        {
            //执行语句      
            CJGZ2013 cjgz = new CJGZ2013();
            YDDGQJK GQ = new YDDGQJK();
            ArrayList alsql = new ArrayList();
            Hashtable HT = GQ.RunGQ();
            alsql = (ArrayList)HT["SQL"];
            bool ff = false;
            if (alsql != null && alsql.Count > 0)
            {
                ff = DbHelperSQL.ExecSqlTran(alsql);
            }

            if (alsql != null && alsql.Count > 1 && ff)
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "发现需要撤销的数据，成功执行撤销操作。";
                try
                {
                    List<List<Hashtable>> list = (List<List<Hashtable>>)HT["TX"];//List<List<Hashtable>>
                    foreach (List<Hashtable> msg in list)
                    {
                        PublicClass2013.Sendmes(msg);
                    }
                }
                catch (Exception exMsg)
                {
                    throw;
                }
                return dsreturn;
            }
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "无过期预订单！";
            return dsreturn;
        }
        catch (Exception ex)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = ex.ToString();
            return dsreturn;
        }
    }


    /// <summary>
    /// 执行发货延迟监控
    /// </summary>
    /// <returns></returns>
    [WebMethod]
    public DataSet serverRunFHYC()
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        try
        {
            //执行语句      
            FHyanchi2013 fhyc = new FHyanchi2013();
            dsreturn = fhyc.RunFHYC(dsreturn);
            return dsreturn;

        }
        catch (Exception ex)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = ex.ToString();
            return dsreturn;
        }
    }

    /// <summary>
    /// 执行发票延迟监控
    /// </summary>
    /// <returns></returns>
    [WebMethod]
    public DataSet serverRunFPYC()
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        try
        {
            //执行语句      
            FHyanchi2013 fhyc = new FHyanchi2013();
            dsreturn = fhyc.RunFPYC(dsreturn);
            return dsreturn;

        }
        catch (Exception ex)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = ex.ToString();
            return dsreturn;
        }
    }


    /// <summary>
    /// 发货延迟监控(仅用于即时的)
    /// </summary>
    /// <returns></returns>
    [WebMethod]
    public DataSet serverRunFHYC_onlyJSJY()
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        try
        {
            //执行语句      
            FHyanchi2013 fhyc = new FHyanchi2013();
            dsreturn = fhyc.RunFHYC_onlyJSJY(dsreturn);
            return dsreturn;

        }
        catch (Exception ex)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = ex.ToString();
            return dsreturn;
        }
    }


    /// <summary>
    /// 计算履约保证金不足的合同
    /// </summary>
    /// <returns></returns>
    [WebMethod]
    public DataSet serverRunLYBZJbuzu()
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        try
        {
            //执行语句      
            FHyanchi2013 fhyc = new FHyanchi2013();
            dsreturn = fhyc.RunLYBZJbuzu(dsreturn);
            return dsreturn;

        }
        catch (Exception ex)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = ex.ToString();

            return dsreturn;
        }
    }




    /// <summary>
    /// 为每组数据处理大盘统计
    /// </summary>
    /// <param name="GroupSPBHstr">有业务变动的商品编号字符串，用竖杠分割</param>
    /// <returns></returns>
    [WebMethod]
    public DataSet CreatNewDapanList_Group()
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        //开始执行
        HomePage HP = new HomePage();
        //得到大盘sql读取路径
        string SQLdapan_lujing = Context.Request.MapPath("../bytefiles/HomePageSQLlist.txt");
        //得到底部统计sql读取路径
        string SQLdibu_lujing = Context.Request.MapPath("../bytefiles/HomePageSQL.txt");
        dsreturn = HP.CreatNewDapanList_Group_Run(dsreturn, SQLdapan_lujing, SQLdibu_lujing);

 
        return dsreturn;
    }



 
  

    /// <summary>
    /// 检查当前所属经纪人状态
    /// </summary>
    /// <returns>返回相关数据</returns>
    [WebMethod]
    public DataSet checkJJRzt(string dlyx)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });


        //重新更新用户基础数据
        KTJYZHClass kc = new KTJYZHClass();
        DataSet dsUserNew =  kc.checkLoginIn(dlyx);
        DataTable dataTableYHXX = dsUserNew.Tables["用户信息"];
        DataTable dataTableGLXX = dsUserNew.Tables["关联信息"];
        DataTable dataTableYHXX_Copy = dataTableYHXX.Copy();
        dataTableYHXX_Copy.TableName = "用户信息";
        DataTable dataTableGLXX_Copy = dataTableGLXX.Copy();
        dataTableGLXX_Copy.TableName = "关联信息";

        //更新审核情况
        string str_jjr = "否";
        string str_fgs = "否";
        string str_jyfmc = "";
        DataSet dssh = DbHelperSQL.Query("select '交易方名称'=I_JYFMC,'是否已被经纪人审核通过'=S_SFYBJJRSHTG,'是否已被分公司审核通过'=S_SFYBFGSSHTG from AAA_DLZHXXB where B_DLYX='" + dlyx + "'");
        if (dssh != null && dssh.Tables[0].Rows.Count > 0)
        {
            str_jyfmc = dssh.Tables[0].Rows[0]["交易方名称"].ToString();
            if (dssh.Tables[0].Rows[0]["是否已被经纪人审核通过"].ToString() == "是")
            {
                str_jjr = "是";
            }
            if (dssh.Tables[0].Rows[0]["是否已被分公司审核通过"].ToString() == "是")
            {
                str_fgs = "是";
            }
        }

        DataSet ds = DbHelperSQL.Query("select top 1 '是否冻结'=B_SFDJ,'是否休眠'=B_SFXM from AAA_DLZHXXB where J_JJRJSBH = (isnull((select top 1 '关联经济人编号'=GLJJRBH from AAA_MJMJJYZHYJJRZHGLB  where DLYX='" + dlyx + "' and SFDQMRJJR = '是'),''))");
        if (ds != null && ds.Tables[0].Rows.Count > 0 )
        {
            if (ds.Tables[0].Rows[0]["是否冻结"].ToString() == "是" && ds.Tables[0].Rows[0]["是否休眠"].ToString() == "是")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的经纪人账户已被冻结，若您需要开展新的业务，请重新选择经纪人！\n\r您的经纪人账户已休眠，若您需要开展新的业务，请重新选择经纪人！";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = str_jjr;
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息2"] = str_fgs;
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息3"] = str_jyfmc;


                dsreturn.Tables.Add(dataTableYHXX_Copy);
                dsreturn.Tables.Add(dataTableGLXX_Copy);

                return dsreturn;
            }
            if (ds.Tables[0].Rows[0]["是否冻结"].ToString() == "是")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的经纪人账户已被冻结，若您需要开展新的业务，请重新选择经纪人！";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = str_jjr;
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息2"] = str_fgs;
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息3"] = str_jyfmc;

 
                dsreturn.Tables.Add(dataTableYHXX_Copy);
                dsreturn.Tables.Add(dataTableGLXX_Copy);

                return dsreturn;
            }
            if (ds.Tables[0].Rows[0]["是否休眠"].ToString() == "是")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的经纪人账户已休眠，若您需要开展新的业务，请重新选择经纪人！";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = str_jjr;
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息2"] = str_fgs;
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息3"] = str_jyfmc;

 
                dsreturn.Tables.Add(dataTableYHXX_Copy);
                dsreturn.Tables.Add(dataTableGLXX_Copy);

                return dsreturn;
            }
        }

        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "一切正常";
        dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = str_jjr;
        dsreturn.Tables["返回值单条"].Rows[0]["附件信息2"] = str_fgs;
        dsreturn.Tables["返回值单条"].Rows[0]["附件信息3"] = str_jyfmc;

 
        dsreturn.Tables.Add(dataTableYHXX_Copy);
        dsreturn.Tables.Add(dataTableGLXX_Copy);


        return dsreturn;


        
    }




    /// <summary>
    /// 登陆验证
    /// </summary>
    /// <returns>返回用户相关数据，或登陆失败数据</returns>
    [WebMethod]
    public DataSet checkLoginIn(string user, string pass)
    {
        string s = Helper.GetOpen(user);
        if (s == "ok")
        {
            #region 用户信息查询
            string sql = "SELECT Number,B_DLYX AS 登录邮箱,B_DLMM AS 登录密码,B_JSZHMM AS 结算账户密码,B_YHM AS 用户名,B_JSZHLX AS 结算账户类型,B_SFYZYX AS 是否验证邮箱, B_SFYXDL AS 是否允许登录, B_SFDJ AS 是否冻结, B_SFXM AS 是否休眠, B_ZCSJ AS 注册时间,B_ZHYCDLSJ AS 最后一次登录时间,isnull(B_ZHDQXYFZ,0) '账户当前信用分值',B_ZHDQKYYE AS 账户当前可用余额,B_YZMGQSJ AS 验证码过期时间,B_YXYZM AS 邮箱验证码, ";//用户帐号基础信息部分

            sql += " J_SELJSBH AS 卖家角色编号, J_BUYJSBH AS 买家角色编号, J_JJRJSBH AS 经纪人角色编号, J_JJRSFZTXYHSH AS 经纪人是否暂停新用户审核, J_JJRZGZSBH AS 经纪人资格证书编号, JJRZGZS AS 经纪人资格证书, ";//平台角色部分

            sql += " S_SFYBJJRSHTG AS 是否已被经纪人审核通过, S_SFYBFGSSHTG AS 是否已被分公司审核通过, DJGNX AS 冻结功能项, ";//是否可进行业务flag

            sql += " I_ZCLB AS 注册类别, I_JYFMC AS 交易方名称, I_YYZZZCH AS 营业执照注册号, I_YYZZSMJ AS 营业执照扫描件, I_SFZH AS 身份证号, I_SFZSMJ AS 身份证扫描件, I_SFZFMSMJ AS 身份证反面扫描件, I_ZZJGDMZDM AS 组织机构代码证代码, I_ZZJGDMZSMJ AS 组织机构代码证扫描件, I_SWDJZSH AS 税务登记证税号, I_SWDJZSMJ AS 税务登记证扫描件, I_YBNSRZGZSMJ AS 一般纳税人资格证扫描件, I_KHXKZH AS 开户许可证号, I_KHXKZSMJ AS 开户许可证扫描件, I_FDDBRXM AS 法定代表人姓名, I_FDDBRSFZH AS 法定代表人身份证号, I_FDDBRSFZSMJ AS 法定代表人身份证扫描件, I_FDDBRSFZFMSMJ AS 法定代表人身份证反面扫描件, I_FDDBRSQS AS 法定代表人授权书, I_JYFLXDH AS 交易方联系电话, I_SSQYS AS 省, I_SSQYSHI AS 市, I_SSQYQ AS 区, I_XXDZ AS 详细地址, I_LXRXM AS 联系人姓名, I_LXRSJH AS 联系人手机号, I_KHYH AS 开户银行, I_YHZH AS 银行账号, I_PTGLJG AS 平台管理机构,I_DSFCGZT AS '第三方存管状态',(case charindex('-',I_ZQZJZH) when '0' then I_ZQZJZH else ''  end) AS '证券资金账号',I_JJRFL AS 经纪人分类 FROM AAA_DLZHXXB ";//提交资质详细信息

            sql += " WHERE '|'+B_DLYX+'|'='|'+(@B_DLYX)+'|' AND '|'+B_DLMM+'|'=('|'+(@B_DLMM)+'|')COLLATE  Chinese_PRC_CS_AS  ";//登录帐号&密码
            SqlParameter[] sp = { 
                                new SqlParameter("@B_DLYX",user),
                                new SqlParameter("@B_DLMM",pass)
                            };
            #endregion
            DataSet ds = DbHelperSQL.Query(sql, sp);//查询出来的用户信息

            DataSet dsReturn = initReturnDataSet().Clone();//克隆初始化的数据集形式
            dsReturn.Tables["返回值单条"].Rows.Add(new string[] { "", "", "", "", "", "", "" });//首先插入一条空数据行

            string sql_isHasEmail = "SELECT * FROM AAA_DLZHXXB WHERE B_DLYX='" + user.Trim() + "'";
            if (DbHelperSQL.Query(sql_isHasEmail).Tables[0].Rows.Count == 0)
            {
                dsReturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsReturn.Tables["返回值单条"].Rows[0]["提示文本"] = "不存在该用户！";
                return dsReturn;
            }

            //开始验证
            if (ds != null && ds.Tables[0].Rows.Count == 1)
            {
                if (ds.Tables[0].Rows[0]["是否验证邮箱"].ToString() == "否" || ds.Tables[0].Rows[0]["是否允许登录"].ToString() == "否")//是否验证邮箱
                {
                    dsReturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    if (ds.Tables[0].Rows[0]["是否验证邮箱"].ToString() == "否")
                    {
                        dsReturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的邮箱尚未通过验证！";
                        DataTable dtUInfo = new DataTable();
                        dtUInfo = ds.Tables[0].Copy();//用户信息表
                        dtUInfo.TableName = "用户信息";
                        dsReturn.Tables.Add(dtUInfo);
                    }
                    else
                    {
                        dsReturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的帐号被我公司禁止登录，请与平台服务人员联系。";
                    }
                    return dsReturn;
                }
                else
                {
                    dsReturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                    DateTime UserTime;
                    //若最后一次登录时间为NULL，说明用户没登录过，则按注册时间
                    if (ds.Tables[0].Rows[0]["最后一次登录时间"] != DBNull.Value)
                        UserTime = Convert.ToDateTime(ds.Tables[0].Rows[0]["最后一次登录时间"].ToString());
                    else
                        UserTime = Convert.ToDateTime(ds.Tables[0].Rows[0]["注册时间"].ToString());
                    double seconds = (DateTime.Now.AddMonths(-12) - UserTime).TotalSeconds;//当前时间-12个月-最后一次登录时间如果还大于或者等于0，说明用户超过12个月未登录了。休眠帐户
                    if (seconds >= 0)
                    {
                        UserSleep(ds.Tables[0].Rows[0]["登录邮箱"].ToString());//休眠帐号
                        ds.Tables[0].Rows[0]["是否休眠"] = "是";
                    }
                    DataTable dtUInfo = new DataTable();
                    dtUInfo = ds.Tables[0].Copy();//用户信息表
                    dtUInfo.TableName = "用户信息";
                    DataTable dtUGL = new DataTable();
                    DataTable dtUGL_Copys = DbHelperSQL.Query("SELECT Number,DLYX AS 登录邮箱,JSZHLX AS 结算账号类型,GLJJRDLZH AS 关联经纪人登陆账号,GLJJRBH AS 关联经纪人编号, GLJJRYHM AS 关联经纪人用户名, SQSJ AS 申请时间, JJRSHZT AS 经纪人审核状态, JJRSHSJ AS 经纪人审核时间, JJRSHYJ AS 经纪人审核意见, FGSSHZT AS 分公司审核状态, FGSSHR AS 分公司审核人, FGSSHSJ AS 分公司审核时间, FGSSHYJ AS 分公司审核意见, SFZTYHXYW AS 是否暂停用户新业务 FROM AAA_MJMJJYZHYJJRZHGLB WHERE SFDQMRJJR='是' and DLYX='" + user + "' and SFYX='是' ").Tables[0];//关联信息表
                    dtUGL = dtUGL_Copys.Copy();
                    dtUGL.TableName = "关联信息";
                    dsReturn.Tables.Add(dtUInfo);
                    dsReturn.Tables.Add(dtUGL);
                    //if (ds.Tables[0].Rows[0]["是否休眠"].ToString() != "是")
                    //{
                    UserUpdateLastSignIn(dtUInfo.Rows[0]["登录邮箱"].ToString());//更新最后一次登录时间、登录次数等信息
                    //}
                }
            }
            //如果数据库中查不到匹配的信息
            else
            {
                dsReturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsReturn.Tables["返回值单条"].Rows[0]["提示文本"] = "认证失败：您输入的账号与密码不匹配，请重新输入！";
            }
            return dsReturn;
        }
        else
        {
            DataSet dsReturn = initReturnDataSet().Clone();//克隆初始化的数据集形式
            dsReturn.Tables["返回值单条"].Rows.Add(new string[] { "", "", "", "", "", "", "" });//首先插入一条空数据行
            dsReturn.Tables["返回值单条"].Rows[0]["执行结果"] = "踢下去";
            dsReturn.Tables["返回值单条"].Rows[0]["提示文本"] = s;
            return dsReturn;
        }
    }

    /// <summary>
    /// 新标准测试方法,郭拓测试
    /// </summary>
    /// <param name="str1">参数1</param>
    /// <param name="str2">参数2</param>
    /// <returns>DataSet</returns>
    [WebMethod]
    public DataSet TestMethod(DataTable dtCS)
    {
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "ok", "执行成功" });
        DataTable dt = new DataTable("other");
        dt.Columns.Add("Str1");
        dt.Columns.Add("Str2");
        dt.Rows.Add(new string[] { dtCS.Rows[0][0].ToString(), dtCS.Rows[0][1].ToString() });
        dsreturn.Tables.Add(dt);
        return dsreturn;
    }

    /// <summary>
    /// 返回平台管理机构
    /// </summary>
    /// <returns>返回固定格式数据集</returns>
    [WebMethod]
    public DataSet GetGLJG()
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "平台管理机构数据获取失败！" });
        KTJYZHClass dc = new KTJYZHClass();
        dsreturn = dc.GetGLJG(dsreturn);
        return dsreturn;
    }

    /// <summary>
    /// 提交，开通经纪人、单位的交易账户信息
    /// </summary>
    /// <returns></returns>
    [WebMethod]
    public DataSet SubmitJJRDW(DataTable dtInfor)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });


 
        //开始执行     
        KTJYZHClass dc = new KTJYZHClass();
        dsreturn = dc.SubmitJJRDW(dsreturn, dtInfor);
 

        return dsreturn;
    }


    /// <summary>
    /// 提交，开通经纪人、个人的交易账户信息
    /// </summary>
    /// <returns></returns>
    [WebMethod]
    public DataSet SubmitJJRGR(DataTable dtInfor)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });


 
        //开始执行     
        KTJYZHClass dc = new KTJYZHClass();
        dsreturn = dc.SubmitJJRGR(dsreturn, dtInfor);
 

        return dsreturn;
    }

    /// <summary>
    /// 提交、开通买卖家、单位的交易账户信息
    /// </summary>
    /// <param name="dtInfor"></param>
    /// <returns></returns>
     [WebMethod]
    public DataSet SubmitMMJDW(DataTable dtInfor)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });


 
        //开始执行     
        KTJYZHClass dc = new KTJYZHClass();
        dsreturn = dc.SubmitMMJDW(dsreturn, dtInfor);
 
        return dsreturn;
    }
     /// <summary>
     /// 提交、开通买卖家、个人的交易账户信息
     /// </summary>
     /// <param name="dtInfor"></param>
     /// <returns></returns>
     [WebMethod]
     public DataSet SubmitMMJGR(DataTable dtInfor)
     {
         //初始化返回值,先塞一行数据
         DataSet dsreturn = initReturnDataSet().Clone();
         dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });


 
         //开始执行     
         KTJYZHClass dc = new KTJYZHClass();
         dsreturn = dc.SubmitMMJGR(dsreturn, dtInfor);
 

         return dsreturn;


     }

     /// <summary>
     /// 修改经纪人、单位的交易账户信息
     /// </summary>
     /// <returns></returns>
     [WebMethod]
     public DataSet UpdateJJRDW(DataTable dtInfor)
     {
         //初始化返回值,先塞一行数据
         DataSet dsreturn = initReturnDataSet().Clone();
         dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });


 
         //开始执行     
         KTJYZHClass dc = new KTJYZHClass();
         dsreturn = dc.UpdateJJRDW(dsreturn, dtInfor);
 

         return dsreturn;
     }

     /// <summary>
     /// 修改经纪人、个人的交易账户信息
     /// </summary>
     /// <returns></returns>
     [WebMethod]
     public DataSet UpdateJJRGR(DataTable dtInfor)
     {
         //初始化返回值,先塞一行数据
         DataSet dsreturn = initReturnDataSet().Clone();
         dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });


 
         //开始执行     
         KTJYZHClass dc = new KTJYZHClass();
         dsreturn = dc.UpdateJJRGR(dsreturn, dtInfor);
 

         return dsreturn;
     }

     /// <summary>
     /// 修改买卖家、单位的交易账户信息
     /// </summary>
     /// <param name="dtInfor"></param>
     /// <returns></returns>
     [WebMethod]
     public DataSet UpdateMMJDW(DataTable dtInfor)
     {
         //初始化返回值,先塞一行数据
         DataSet dsreturn = initReturnDataSet().Clone();
         dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });


 
         //开始执行     
         KTJYZHClass dc = new KTJYZHClass();
         dsreturn = dc.UpdateMMJDW(dsreturn, dtInfor);
 
         return dsreturn;
     }

     /// <summary>
     /// 修改买卖家、个人的交易账户信息
     /// </summary>
     /// <param name="dtInfor"></param>
     /// <returns></returns>
     [WebMethod]
     public DataSet UpdateMMJGR(DataTable dtInfor)
     {
         //初始化返回值,先塞一行数据
         DataSet dsreturn = initReturnDataSet().Clone();
         dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });


 
         //开始执行     
         KTJYZHClass dc = new KTJYZHClass();
         dsreturn = dc.UpdateMMJGR(dsreturn, dtInfor);
 

         return dsreturn;


     }
     




     /// <summary>
     /// 经纪人审核 买卖家的数据信息
     /// </summary>
     /// <param name="dsreturn"></param>
     /// <param name="dataTable"></param>
     /// <returns></returns>
    [WebMethod] 
    public DataSet JJRPassMMJ(DataTable dtInfor)
     {
         //初始化返回值,先塞一行数据
         DataSet dsreturn = initReturnDataSet().Clone();
         dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });


 
         //开始执行     
         KTJYZHClass dc = new KTJYZHClass();
         dsreturn = dc.JJRPassMMJ(dsreturn, dtInfor);
 

         return dsreturn;
     
     }

    /// <summary>
    /// 经纪人驳回  买卖家的数据信息
    /// </summary>
    /// <param name="dsreturn"></param>
    /// <param name="dataTable"></param>
    /// <returns></returns>
    [WebMethod] 
    public DataSet JJRRejectMMJ(DataTable dtInfor)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });


 
        //开始执行     
        KTJYZHClass dc = new KTJYZHClass();
        dsreturn = dc.JJRRejectMMJ(dsreturn, dtInfor);
 

        return dsreturn;
    
    
    }

    /// <summary>
    /// 得到买卖家的数据信息
    /// </summary>
    /// <param name="dsreturn"></param>
    /// <param name="dataTable"></param>
    /// <returns></returns>
     [WebMethod] 
    public DataSet GetMMJData(DataTable dtInfor)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });


 
        //开始执行     
        KTJYZHClass dc = new KTJYZHClass();
        dsreturn = dc.GetMMJData(dsreturn, dtInfor);
 

        return dsreturn;
    
    
    }


     /// <summary>
     /// 得到买卖家的数据信息
     /// </summary>
     /// <param name="dsreturn"></param>
     /// <param name="dataTable"></param>
     /// <returns></returns>
     [WebMethod]
     public DataSet GetMMJZHZLData(DataTable dtInfor)
     {
         //初始化返回值,先塞一行数据
         DataSet dsreturn = initReturnDataSet().Clone();
         dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });


 
         //开始执行     
         KTJYZHClass dc = new KTJYZHClass();
         dsreturn = dc.GetMMJZHZLData(dsreturn, dtInfor);
 

         return dsreturn;


     }


     /// <summary>
     /// 得到经纪人的数据信息
     /// </summary>
     /// <param name="dsreturn"></param>
     /// <param name="dataTable"></param>
     /// <returns></returns>
     [WebMethod]
     public DataSet GetJJRData(DataTable dtInfor)
     {
         //初始化返回值,先塞一行数据
         DataSet dsreturn = initReturnDataSet().Clone();
         dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });


 
         //开始执行     
         KTJYZHClass dc = new KTJYZHClass();
         dsreturn = dc.GetJJRData(dsreturn, dtInfor);
 

         return dsreturn;


     }
  



    /// <summary>
    /// 根据资格证书，获取经纪人的用户信息
    /// </summary>
    /// <param name="strZGZS"></param>
    /// <returns></returns>
    [WebMethod]
    public DataSet GetJJRYHXX(string strZGZS)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
        //开始执行     
        KTJYZHClass dc = new KTJYZHClass();
        dsreturn = dc.GetJJRYHXX(dsreturn, strZGZS);
        return dsreturn;     
    }

    /// <summary>
    /// 判断身份证号时候已经被注册
    /// </summary>
    /// <param name="strZGZS"></param>
    /// <returns></returns>
    [WebMethod]
    public DataSet JudgeSFZHXX(string strZHLX, string strSFZH)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
        //开始执行     
        KTJYZHClass dc = new KTJYZHClass();
        dsreturn = dc.JudgeSFZHXX(dsreturn,strZHLX,strSFZH);
        return dsreturn;     
    }


    /// <summary>
    ///根据买卖家交易账户的登录邮箱，获得买卖家交易账户的数据信息
    /// </summary>
    /// <param name="strMMJDLYX"></param>
    /// <returns></returns>
     [WebMethod]
    public DataSet GetMMJJYZHData(DataTable dtInfor)
    { 
      //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
        //开始执行     
        KTJYZHClass dc = new KTJYZHClass();
        dsreturn = dc.GetMMJJYZHData(dsreturn, dtInfor);
        return dsreturn;
    
    }
    /// <summary>
    /// 得到交易账户审核的数据信息
    /// </summary>
    /// <param name="dsreturn"></param>
    /// <param name="dataTable"></param>
    /// <returns></returns>
    [WebMethod]
    public DataSet GetJYZHSHData(DataTable dtInfor)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });


 
        //开始执行     
        KTJYZHClass dc = new KTJYZHClass();
        dsreturn = dc.GetJYZHSHData(dsreturn, dtInfor);
 

        return dsreturn;
    }




    /// <summary>
    /// 生成经纪人资格证书图片
    /// </summary>
    /// <param name="dsreturn"></param>
    /// <param name="dataTable"></param>
    /// <returns></returns>
     [WebMethod]
    public DataSet GetJJRZGZS(DataTable dataTable)
    {

        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });


        Hashtable htUserVal = PublicClass2013.GetUserInfo(dataTable.Rows[0]["买家角色编号"].ToString());//通过买家角色编号获取信息

        if (htUserVal["是否休眠"].ToString().Trim() == "是")
        {

            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于休眠状态，请进入账户维护中激活休眠账户！";
            return dsreturn;
        }



        //开始执行    
        #region//生成经纪人资格证书编号
        DataTable dt_UserInfo = DbHelperSQL.Query("select J_JJRZGZSBH '经纪人资格证书编号',S_SFYBFGSSHTG '是否已经被分公司审核通过',I_JYFMC '交易方名称',JJRZGZS '经纪人资格证书',J_JJRZGZSYXQKSSJ '经纪人资格证书有效期开始时间',J_JJRZGZSYXQJSSJ '经纪人资格证书有效期结束时间',* from AAA_DLZHXXB where B_DLYX='" + dataTable.Rows[0]["DLYX"].ToString() + "'  and B_JSZHLX='" + dataTable.Rows[0]["JSZHLX"].ToString() + "' ").Tables[0];//获取用户信息，
        string strJJRZGZSBH = dt_UserInfo.Rows[0]["经纪人资格证书编号"].ToString();//经纪人资格证书编号
        string strJYFMC = dt_UserInfo.Rows[0]["交易方名称"].ToString();//交易方名称
        object objstrJJRZGZS = dt_UserInfo.Rows[0]["经纪人资格证书"];
           string strJJRZGZS = "";
           if (objstrJJRZGZS is DBNull)
           {
               strJJRZGZS = "";
           }
           else
           {
               strJJRZGZS = objstrJJRZGZS.ToString();
           }
        //如果没有经纪人资格证书而且是审核通过
        if (String.IsNullOrEmpty(strJJRZGZS) && dt_UserInfo.Rows[0]["是否已经被分公司审核通过"].ToString()=="是")
        {
            DateTime ZSYXQ_QS = Convert.ToDateTime(dt_UserInfo.Rows[0]["经纪人资格证书有效期开始时间"].ToString());
            string year_QS = ZSYXQ_QS.Year.ToString();//年份
            string month_QS = ZSYXQ_QS.Month.ToString();//月份
            string day_QS = ZSYXQ_QS.Day.ToString();//日期
            DateTime ZSYXQ_ZZ = Convert.ToDateTime(dt_UserInfo.Rows[0]["经纪人资格证书有效期结束时间"].ToString());//有限期截止时间，在当前有效期在延后两年
            string year_ZZ = ZSYXQ_ZZ.Year.ToString();//年份
            string month_ZZ = ZSYXQ_ZZ.Month.ToString();//月份
            string day_ZZ = ZSYXQ_ZZ.Day.ToString();//日期


            string ResourcePath = Context.Server.MapPath("JHJX_JJRZGZS/JJRZGZS_Path/JJRZGZS_Initial.xml");//经纪人资格证书模板服务器路径
            string FileName = Guid.NewGuid().ToString();
            string Paths = Server.MapPath("JHJX_JJRZGZS/JJRZGZS_NewPath/") + FileName + ".xml";//拷贝后的文件目录、
            string NewPaths = Server.MapPath("JHJX_JJRZGZS/JJRZGZS_NewPath/COPY/") + FileName + ".xml";//保存后的目录
            string SavingPath = "~/JHJXPT/SaveDir/JJRZGZS/" + FileName + ".png";//存入路径
            string SavingPathDataBase = "/JHJXPT/SaveDir/JJRZGZS/" + FileName + ".png";//存入数据库的路径;
            if (File.Exists(ResourcePath))
            {
                File.Copy(ResourcePath, Paths, true);
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(Paths);//载入此XML
                XmlNodeList xnl = xmlDoc.GetElementsByTagName("w:t");//office xml word的Tag
                foreach (XmlNode xn in xnl)
                {
                    if (xn.InnerXml == "PT_BH")
                    {
                        xn.InnerText = strJJRZGZSBH;
                    }
                    if (xn.InnerXml == "JYFMC")
                    {
                        xn.InnerXml = strJYFMC;
                    }
                    if (xn.InnerXml == "YYYY_QS")
                    {
                        xn.InnerXml = year_QS;
                    }
                    if (xn.InnerXml == "MM_QS")
                    {
                        xn.InnerXml = month_QS;
                    }
                    if (xn.InnerXml == "dd_QS")
                    {
                        xn.InnerXml = day_QS;
                    }
                    if (xn.InnerXml == "YYYY_ZZ")
                    {
                        xn.InnerXml = year_ZZ;
                    }
                    if (xn.InnerXml == "MM_ZZ")
                    {
                        xn.InnerXml = month_ZZ;
                    }
                    if (xn.InnerXml == "dd_ZZ")
                    {
                        xn.InnerXml = day_ZZ;
                    }
                    if (xn.InnerXml == "YYYY_PT")
                    {
                        xn.InnerXml = year_QS;
                    }
                    if (xn.InnerXml == "MM_PT")
                    {
                        xn.InnerXml = month_QS;
                    }
                    if (xn.InnerXml == "dd_PT")
                    {
                        xn.InnerXml = day_QS;
                    }
                }
                xmlDoc.Save(NewPaths);
                if (File.Exists(NewPaths))
                {
                    Document doc = new Document(NewPaths);
                    ImageSaveOptions iso = new ImageSaveOptions(SaveFormat.Png);//另存为PNG格式
                  //  iso.Resolution = 128;

                    doc.Save(Server.MapPath(SavingPath), iso);
                }
                File.Delete(NewPaths);
                File.Delete(Paths);
            }

            DbHelperSQL.ExecuteSql("update  AAA_DLZHXXB set JJRZGZS='" + SavingPathDataBase + "' where B_DLYX='" + dataTable.Rows[0]["DLYX"].ToString() + "'");
            DataTable dt_UserInfoTwo = DbHelperSQL.Query("select J_JJRZGZSBH '经纪人资格证书编号',I_JYFMC '交易方名称',JJRZGZS '经纪人资格证书',* from AAA_DLZHXXB where B_DLYX='" + dataTable.Rows[0]["DLYX"].ToString() + "'").Tables[0];//获取用户信息，
            DataTable dt_UserInfoTwoCopy = dt_UserInfoTwo.Copy();
            if (dt_UserInfoTwoCopy.Rows.Count > 0)
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "经纪人交易账户信息获取成功！";
                dsreturn.Tables.Add(dt_UserInfoTwoCopy);
                dsreturn.Tables[1].TableName = "经纪人交易账户信息";
            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "经纪人交易账户信息获取失败！";
            }
     
            return dsreturn;
        }
        else
        {
            if (dt_UserInfo.Rows.Count > 0)
            {
                DataTable dt_UserInfoCopy = dt_UserInfo.Copy();
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "经纪人交易账户信息获取成功！";
                dsreturn.Tables.Add(dt_UserInfoCopy);
                dsreturn.Tables[1].TableName = "经纪人交易账户信息";
            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "经纪人交易账户信息获取失败！";
            
            
            }
            return dsreturn;
           
        }
        #endregion
    }


     /// <summary>
     /// 生成演示图片
     /// </summary>
     /// <param name="dsreturn"></param>
     /// <param name="dataTable"></param>
     /// <returns></returns>
     [WebMethod]
     public DataSet GetYSTP()
     {
         //初始化返回值,先塞一行数据
         DataSet dsreturn = initReturnDataSet().Clone();
         dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
         if (true)
         {
             string FileName = Guid.NewGuid().ToString();

             string ResourcePath = Server.MapPath("JHJX_JJRZGZS/JJRZGZS_Path/FMPTXY_Initial.xml");//经纪人资格证书模板服务器路径
             string Paths = Server.MapPath("JHJX_JJRZGZS/JJRZGZS_NewPath/") + FileName + ".xml";//拷贝后的文件目录、
             string NewPaths = Server.MapPath("JHJX_JJRZGZS/JJRZGZS_NewPath/COPY/") + FileName + ".xml";//保存后的目录
             string SavingPath = "";//保存图片的路径，后面会赋值
             string SavingPathDataBase = "";//返回图片路径;
             DataTable dt = new DataTable();
             dt.TableName = "图片链接";
             if (File.Exists(ResourcePath))
             {
                 File.Copy(ResourcePath, Paths, true);
                 XmlDocument xmlDoc = new XmlDocument();
                 xmlDoc.Load(Paths);//载入此XML
                 XmlNodeList xnl = xmlDoc.GetElementsByTagName("w:t");//office xml word的Tag
                 xmlDoc.Save(NewPaths);
                 if (File.Exists(NewPaths))
                 {
                     Document doc = new Document(NewPaths);

                  
                     dt.Columns.Add("TPLJ", typeof(string));
                     for (int i = 0; i < doc.PageCount; i++)
                     {
                         string FileNamePNG = Guid.NewGuid().ToString();
                         SavingPath = "~/JHJXPT/SaveDir/YSTP/" + FileNamePNG + ".png";//存入路径
                         SavingPathDataBase = "/JHJXPT/SaveDir/YSTP/" + FileNamePNG + ".png";
                         ImageSaveOptions iso = new ImageSaveOptions(SaveFormat.Png);//另存为PNG格式
                         iso.Resolution = 100;
                         iso.PageIndex = i;
                         doc.Save(Server.MapPath(SavingPath), iso);
                         dt.Rows.Add(new object[] { SavingPathDataBase });
                     }
                 }
                 File.Delete(NewPaths);
                 File.Delete(Paths);
             }
             dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
             dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "经纪人交易账户信息获取成功！";
             dsreturn.Tables.Add(dt);
             return dsreturn;
         }
         else
         {

             return null;
         }
     }



       /// <summary>
    /// 暂停、恢复新用户审核  仅用于经纪人交易账户的时候
    /// </summary>
    /// <param name="dsreturn"></param>
    /// <param name="dataTable"></param>
    /// <returns></returns>
      [WebMethod] 
    public DataSet ZT_HF_XYHSH(DataTable dtInfor)
     {
         //初始化返回值,先塞一行数据
         DataSet dsreturn = initReturnDataSet().Clone();
         dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });


 
         //开始执行     
         KTJYZHClass dc = new KTJYZHClass();
         dsreturn = dc.ZT_HF_XYHSH(dsreturn, dtInfor);
 

         return dsreturn;
     
     
     }

      /// <summary>
      /// 暂停、恢复新用户审核  初始状态
      /// </summary>
      /// <param name="dsreturn"></param>
      /// <param name="dataTable"></param>
      /// <returns></returns>
      [WebMethod]
      public DataSet ZT_HF_XYHSHCSZT(DataTable dtInfor)
      {
          //初始化返回值,先塞一行数据
          DataSet dsreturn = initReturnDataSet().Clone();
          dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
          //开始执行     
          KTJYZHClass dc = new KTJYZHClass();
          dsreturn = dc.ZT_HF_XYHSHCSZT(dsreturn, dtInfor);
          return dsreturn;
      }



      /// <summary>
      /// 暂停恢复用户的新业务
      /// </summary>
      /// <param name="dsreturn"></param>
      /// <param name="dataTable"></param>
      /// <returns></returns>
      [WebMethod]
      public DataSet SetZTHFYHXYW(DataTable dtInfor)
      {
          //初始化返回值,先塞一行数据
          DataSet dsreturn = initReturnDataSet().Clone();
          dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
 
          //开始执行     
          KTJYZHClass dc = new KTJYZHClass();
          dsreturn = dc.SetZTHFYHXYW(dsreturn, dtInfor);
 
          return dsreturn;
      }



    /// <summary>
      /// 获取更换经纪人时获取的经纪人交易账户数据信息
    /// </summary>
    /// <param name="dtInfor"></param>
    /// <returns></returns>
    [WebMethod]
    public DataSet GetSiftJJRData(DataTable dtInfor)    
      {
          //初始化返回值,先塞一行数据
          DataSet dsreturn = initReturnDataSet().Clone();
          dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
          //开始执行     
          KTJYZHClass dc = new KTJYZHClass();
          dsreturn = dc.GetSiftJJRData(dsreturn, dtInfor);
          return dsreturn;
      }

    /// <summary>
    /// 当选择经纪人时获取要判断的数据信息
    /// </summary>
    /// <param name="dtInfor"></param>
    /// <returns></returns>
      [WebMethod]
    public DataSet GetSelectJudgeJJRData(DataTable dtInfor)    
      {
          //初始化返回值,先塞一行数据
          DataSet dsreturn = initReturnDataSet().Clone();
          dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
 
          //开始执行     
          KTJYZHClass dc = new KTJYZHClass();
          dsreturn = dc.GetSelectJudgeJJRData(dsreturn, dtInfor);
 
          return dsreturn;
      }
    /// <summary>
    /// 获取交易账户的当前状态
    /// </summary>
    /// <param name="dtInfor"></param>
    /// <returns></returns>
      [WebMethod]
      public DataSet GetJYZHDQZT(DataTable dtInfor)
      {
          //初始化返回值,先塞一行数据
          DataSet dsreturn = initReturnDataSet().Clone();
          dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
          //开始执行     
          KTJYZHClass dc = new KTJYZHClass();
          dsreturn = dc.GetJYZHDQZT(dsreturn, dtInfor);
        
          return dsreturn;
      }
    /// <summary>
    /// 使帐户休眠
    /// </summary>
    /// <param name="email">用户登录邮箱</param>
    protected void UserSleep(string email)
    {

        string sql = "UPDATE AAA_DLZHXXB SET B_SFXM='是' WHERE B_DLYX =@B_DLYX";
        SqlParameter[] sp = {
                               new SqlParameter("@B_DLYX",SqlDbType.VarChar)
                            };
        sp[0].Value = email;
        DbHelperSQL.ExecuteSql(sql, sp);
    }

    /// <summary>
    /// 更新用户最后一次登录时间、IP等信息
    /// </summary>
    /// <param name="email"></param>
    protected void UserUpdateLastSignIn(string email)
    {
        string sql = "UPDATE AAA_DLZHXXB SET B_ZHYCDLSJ=@B_ZHYCDLSJ, B_ZHYCDLIP=@B_ZHYCDLIP, B_DLCS = B_DLCS+1 WHERE B_DLYX=@B_DLYX";
        SqlParameter[] sp = {
                                new SqlParameter("@B_ZHYCDLSJ",DateTime.Now.ToString()),
                                new SqlParameter("@B_ZHYCDLIP",GetIP()),
                                new SqlParameter("@B_DLYX",email)
                            };
        DbHelperSQL.ExecuteSql(sql, sp);
    }

    /// <summary>
    /// 获取用户IP地址,本方法无法放到PulicClass下，需要用到HTTP的东西
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// 定标操作
    /// </summary>
    /// <param name="test">参数</param>
    /// <returns>返回固定格式数据集</returns>
    [WebMethod]
    public DataSet Jhjx_db(DataTable ht)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        //加锁
        if (jhjx_Lock.LockBegin_ByUser("全局锁") != "1")
        {

            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统繁忙";
            return dsreturn;
        }
        //开始执行     
        Jhjxpt_db dc = new Jhjxpt_db();
        dsreturn = dc.jhjx_db(dsreturn, ht);
        //解锁
        jhjx_Lock.LockEnd_ByUser("全局锁");
        return dsreturn;
    }


    /// <summary>
    /// 货物签收
    /// </summary>
    /// <param name="test">参数</param>
    /// <returns>返回固定格式数据集</returns>
    [WebMethod]
    public DataSet Jhjx_hwqs(DataTable ht)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        //加锁
        if (jhjx_Lock.LockBegin_ByUser("全局锁") != "1")
        {

            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统繁忙";
            return dsreturn;
        }
        //开始执行     
        Jhjx_hwqs dc = new Jhjx_hwqs();
        dsreturn = dc.dsjhjx_hwqs(dsreturn, ht);
        //解锁
        jhjx_Lock.LockEnd_ByUser("全局锁");
        return dsreturn;
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

        string isExist = "SELECT * FROM AAA_DLZHXXB WHERE B_DLYX='" + uid.Trim() + "'";//是否有重复帐号
        string isExistUName = "SELECT * FROM AAA_DLZHXXB WHERE B_YHM='" + uname.Trim() + "'";

        string isExist_Checks = "SELECT * FROM AAA_DLZHXXB WHERE B_DLYX='" + uid.Trim() + "' AND B_SFYZYX='是'";//是否有重复帐号
        string isExistUName_Checks = "SELECT * FROM AAA_DLZHXXB WHERE B_YHM='" + uname.Trim() + "' AND B_SFYZYX='是'";

        int isUname = DbHelperSQL.Query(isExistUName).Tables[0].Rows.Count;//是否有重复的用户名
        int isID = DbHelperSQL.Query(isExist).Tables[0].Rows.Count;//是否有重复的ID
        if (isID > 0)
        {
            //DataTable dtNameToID = DbHelperSQL.Query(isExistUName).Tables[0];//获取已经存在的用户名的信息
            DataTable dtResult = dt.Clone();
            dtResult.Rows.Add(new object[] { "0", uid, uname, pwd, valnum, "FALSE", "已存在该帐户" });
            dtResult.TableName = "执行结果";
            ds.Tables.Clear();
            ds.Tables.Add(dtResult);
            return ds;
        }
        //已经存在用户名
        else if (isUname > 0)
        {
            //DataTable dtNameToID = DbHelperSQL.Query(isExistUName).Tables[0];//获取已经存在的用户名的信息
            DataTable dtResult = dt.Clone();
            dtResult.Rows.Add(new object[] { "0", uid, uname, pwd, valnum, "FALSE", "用户名重复" });
            dtResult.TableName = "执行结果";
            ds.Tables.Clear();
            ds.Tables.Add(dtResult);
            return ds;
        }
        if (isID == 0 && isUname == 0)
        {
            #region 新用户，无一点记录。
            string PK = PublicClass2013.GetNextNumberZZ("AAA_DLZHXXB", "");
            string DLYX = uid;
            string YHM = uname;
            string DLMM = pwd;
            string YXYZM = valnum;
            string YZMGQSJ = DateTime.Now.AddDays(1).ToString();//验证码过期时间

            string ZCSJ = DateTime.Now.ToString();
            string ZCIP = GetIP();
            string DLCS = "0";
            string ZHDQKYYE = "0.00";
            string InsertSql = "INSERT INTO AAA_DLZHXXB(Number,B_DLYX,B_YHM,B_DLMM,B_JSZHMM,B_YXYZM,B_YZMGQSJ,B_SFYZYX,B_SFYXDL,B_SFDJ,B_SFXM,B_ZCSJ,B_ZCIP,B_DLCS,B_ZHDQKYYE,S_SFYBJJRSHTG,S_SFYBFGSSHTG,CreateTime,CreateUser,B_DSFCGKYYE) VALUES('" + PK + "','" + DLYX + "','" + YHM + "','" + DLMM + "','" + DLMM + "','" + YXYZM + "','" + YZMGQSJ + "','否','否','否','否','" + ZCSJ + "','" + ZCIP + "','" + DLCS + "','" + ZHDQKYYE + "','否','否','" + ZCSJ + "','" + PK + "',0)";//插入语句，在发送邮箱验证码之后执行。
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
                if (DbHelperSQL.Query(isExist).Tables[0].Rows.Count > 0)
                {
                    //DataTable dtNameToID = DbHelperSQL.Query(isExistUName).Tables[0];//获取已经存在的用户名的信息
                    DataTable dtResult = dt.Clone();
                    dtResult.Rows.Add(new object[] { "0", uid, uname, pwd, valnum, "FALSE", "已存在该帐户" });
                    dtResult.TableName = "执行结果";
                    ds.Tables.Clear();
                    ds.Tables.Add(dtResult);
                    return ds;
                }
                //已经存在用户名
                else if (DbHelperSQL.Query(isExistUName).Tables[0].Rows.Count > 0)
                {
                    //DataTable dtNameToID = DbHelperSQL.Query(isExistUName).Tables[0];//获取已经存在的用户名的信息
                    DataTable dtResult = dt.Clone();
                    dtResult.Rows.Add(new object[] { "0", uid, uname, pwd, valnum, "FALSE", "用户名重复" });
                    dtResult.TableName = "执行结果";
                    ds.Tables.Clear();
                    ds.Tables.Add(dtResult);
                    return ds;
                }


                int i = DbHelperSQL.ExecuteSql(InsertSql);//执行sql
                if (i == 1)
                {
                    DataTable dtResult = dt.Clone();
                    dtResult.Rows.Add(new object[] { PK, DLYX, YHM, DLMM, YXYZM, "SUCCESS", "SUCCESS" });
                    dtResult.TableName = "执行结果";
                    ds.Tables.Clear();
                    ds.Tables.Add(dtResult);
                }
            }
            catch (Exception ex)
            {
                DataTable dtResult = dt.Clone();
                dtResult.Rows.Add(new object[] { PK, DLYX, YHM, DLMM, YXYZM, "FALSE", ex.Message });
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
                //DataTable dtNameToID = DbHelperSQL.Query(isExistUName).Tables[0];//获取已经存在的用户名的信息
                DataTable dtResult = dt.Clone();
                dtResult.Rows.Add(new object[] { "0", uid, uname, pwd, valnum, "FALSE", "已存在该帐户" });
                dtResult.TableName = "执行结果";
                ds.Tables.Clear();
                ds.Tables.Add(dtResult);
                return ds;
            }
            //已经存在用户名
            else if (isUname > 0)
            {
                //DataTable dtNameToID = DbHelperSQL.Query(isExistUName).Tables[0];//获取已经存在的用户名的信息
                DataTable dtResult = dt.Clone();
                dtResult.Rows.Add(new object[] { "0", uid, uname, pwd, valnum, "FALSE", "用户名重复" });
                dtResult.TableName = "执行结果";
                ds.Tables.Clear();
                ds.Tables.Add(dtResult);
                return ds;
            }
            #region 作废
            //else
            //{
            //    #region 判断是否是二次更改
            //    string isExist_Checked = "SELECT * FROM AAA_DLZHXXB WHERE B_DLYX='" + uid.Trim() + "' AND B_SFYZYX='否'";
            //    string isExistUName_Checked = "SELECT * FROM AAA_DLZHXXB WHERE B_YHM='" + uname.Trim() + "' AND B_SFYZYX='否'";
            //    #endregion
            //    DataTable tableDone = DbHelperSQL.Query(isExist_Checked).Tables[0];
            //    if (tableDone.Rows.Count > 0)
            //    {
            //        string pk = tableDone.Rows[0]["Number"].ToString();
            //        if (tableDone.Rows[0]["B_SFYZYX"].ToString() == "否")
            //        {
            //            //如果这个帐号还没有验证邮箱，则更新验证码，允许注册
            //            string UpdateSql = "UPDATE AAA_DLZHXXB SET B_YXYZM='" + valnum + "', B_YZMGQSJ='" + DateTime.Now.AddDays(1).ToString() + "',B_DLMM='" + pwd + "',B_YHM='" + uname + "' WHERE B_DLYX='" + uid + "'";
            //            try
            //            {
            //                ClassEmail CE = new ClassEmail();
            //                string strSubject = "中国商品批发交易平台 - 注册帐号";
            //                string modHtmlFileName = HttpContext.Current.Server.MapPath("~/JHJXPT/EmailTemplate.htm");
            //                Hashtable ht = new Hashtable();
            //                ht["[[Email]]"] = uid;
            //                ht["[[valNumbs]]"] = valnum;
            //                ht["[[DateTime]]"] = DateTime.Now.ToString("yyyy-MM-dd");
            //                CE.SendSPEmail(uid, strSubject, modHtmlFileName, ht);
            //                int i = DbHelperSQL.ExecuteSql(UpdateSql);//更新语句
            //                if (i == 1)
            //                {
            //                    DataTable dtResult = dt.Clone();
            //                    dtResult.Rows.Add(new object[] { pk, uid, uname, pwd, valnum, "SUCCESS", "SUCCESS" });
            //                    dtResult.TableName = "执行结果";
            //                    ds.Tables.Clear();
            //                    ds.Tables.Add(dtResult);
            //                }
            //                else
            //                {
            //                    DataTable dtResult = dt.Clone();
            //                    dtResult.Rows.Add(new object[] { pk, uid, uname, pwd, valnum, "FALSE", "服务器繁忙" });
            //                    dtResult.TableName = "执行结果";
            //                    ds.Tables.Clear();
            //                    ds.Tables.Add(dtResult);
            //                }
            //            }
            //            catch (Exception ex)
            //            {
            //                DataTable dtResult = dt.Clone();
            //                dtResult.Rows.Add(new object[] { pk, uid, uname, pwd, valnum, "FALSE", "服务器繁忙" });
            //                dtResult.TableName = "执行结果";
            //                ds.Tables.Clear();
            //                ds.Tables.Add(dtResult);
            //            }
            //        }
            //    }
            //}
            #endregion
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
        string isExist = "SELECT * FROM AAA_DLZHXXB WHERE B_DLYX='" + uid.Trim() + "' AND B_SFYZYX='是'";//是否有重复帐号
        string isExistUName = "SELECT * FROM AAA_DLZHXXB WHERE B_YHM='" + uname.Trim() + "' AND B_SFYZYX='是'";
        DataTable dtIsEmail = DbHelperSQL.Query(isExist).Tables[0];//是否存在重复帐号
        DataTable dtIsExistUname = DbHelperSQL.Query(isExistUName).Tables[0];//是否存在用户昵称
        if (dtIsEmail.Rows.Count == 0 && dtIsExistUname.Rows.Count == 0)
        {
            string sql_VALEMAIL = "SELECT * FROM AAA_DLZHXXB WHERE B_DLYX='" + uid.Trim() + "' AND Number <> '" + pk + "'";
            string sql2_VALYHM = "SELECT * FROM AAA_DLZHXXB WHERE B_YHM='" + uname.Trim() + "' AND Number <> '" + pk + "'";
            if (DbHelperSQL.Query(sql_VALEMAIL).Tables[0].Rows.Count == 0 && DbHelperSQL.Query(sql2_VALYHM).Tables[0].Rows.Count == 0)
            {
                //如果不存在，执行更新操作
                string UpdateSql = "UPDATE AAA_DLZHXXB SET B_DLYX='" + uid + "', B_YHM='" + uname + "', B_DLMM='" + pwd + "', B_YXYZM='" + valnum + "' ,B_YZMGQSJ='" + DateTime.Now.AddDays(1).ToString() + "' WHERE Number='" + pk + "'";
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
                        dtResult.Rows.Add(new object[] { pk, uid, uname, pwd, valnum, "SUCCESS", "SUCCESS" });
                        dtResult.TableName = "执行结果";
                        ds.Tables.Clear();
                        ds.Tables.Add(dtResult);
                    }
                }
                catch (Exception ex)
                {
                    DataTable dtResult = dt.Clone();
                    dtResult.Rows.Add(new object[] { pk, uid, uname, pwd, valnum, "FALSE", ex.Message });
                    dtResult.TableName = "执行结果";
                    ds.Tables.Clear();
                    ds.Tables.Add(dtResult);
                }
            }
            else
            {
                if (DbHelperSQL.Query(sql_VALEMAIL).Tables[0].Rows.Count > 0)
                {
                    DataTable dtNameToID = DbHelperSQL.Query(isExistUName).Tables[0];//获取已经存在的用户名的信息
                    DataTable dtResult = dt.Clone();
                    dtResult.Rows.Add(new object[] { pk, uid, uname, pwd, valnum, "FALSE", "已存在该帐户" });
                    dtResult.TableName = "执行结果";
                    ds.Tables.Clear();
                    ds.Tables.Add(dtResult);
                    return ds;
                }
                if (DbHelperSQL.Query(sql2_VALYHM).Tables[0].Rows.Count > 0)
                {
                    DataTable dtNameToID = DbHelperSQL.Query(isExistUName).Tables[0];//获取已经存在的用户名的信息
                    DataTable dtResult = dt.Clone();
                    dtResult.Rows.Add(new object[] { pk, uid, uname, pwd, valnum, "FALSE", "用户名重复" });
                    dtResult.TableName = "执行结果";
                    ds.Tables.Clear();
                    ds.Tables.Add(dtResult);
                    return ds;
                }
            }
        }
        else
        {
            if (dtIsEmail.Rows.Count > 0)
            {
                DataTable dtNameToID = DbHelperSQL.Query(isExistUName).Tables[0];//获取已经存在的用户名的信息
                DataTable dtResult = dt.Clone();
                dtResult.Rows.Add(new object[] { pk, uid, uname, pwd, valnum, "FALSE", "已存在该帐户" });
                dtResult.TableName = "执行结果";
                ds.Tables.Clear();
                ds.Tables.Add(dtResult);
                return ds;
            }
            if (dtIsExistUname.Rows.Count > 0)
            {
                DataTable dtNameToID = DbHelperSQL.Query(isExistUName).Tables[0];//获取已经存在的用户名的信息
                DataTable dtResult = dt.Clone();
                dtResult.Rows.Add(new object[] { pk, uid, uname, pwd, valnum, "FALSE", "用户名重复" });
                dtResult.TableName = "执行结果";
                ds.Tables.Clear();
                ds.Tables.Add(dtResult);
                return ds;
            }
        }
        return ds;
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
        string UpdateSql = "UPDATE AAA_DLZHXXB SET B_YXYZM='" + valNum + "', B_YZMGQSJ='" + DateTime.Now.AddDays(1).ToString() + "' WHERE B_DLYX='" + dlyx + "'";//要执行的更新语句
        try
        {
            ClassEmail CE = new ClassEmail();
            string strSuject = "中国商品批发交易平台-注册帐号";
            string modHtmlFileName = HttpContext.Current.Server.MapPath("~/JHJXPT/EmailTemplate.htm");
            Hashtable ht = new Hashtable();
            ht["[[Email]]"] = dlyx;
            ht["[[valNumbs]]"] = valNum;
            ht["[[DateTime]]"] = DateTime.Now.ToString("yyyy-MM-dd");
            CE.SendSPEmail(dlyx.Trim(), strSuject, modHtmlFileName, ht);
        }
        catch (Exception ex)
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
    /// 激活帐户（注册第三步）
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [WebMethod]
    public DataSet AccountID(string id, string valNum)
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
        DataTable dtAccountID = DbHelperSQL.Query("SELECT * FROM AAA_DLZHXXB WHERE B_DLYX='" + id + "'").Tables[0];
        if (dtAccountID.Rows.Count > 0)
        {
            DateTime ValEndTime = Convert.ToDateTime(dtAccountID.Rows[0]["B_YZMGQSJ"].ToString());
            if (ValEndTime < DateTime.Now)
            {
                dt.Rows.Add(new object[] { id, "", "", "FALSE", "验证失败，您的邮箱验证码已失效，已重新发送验证邮件，请重新输入验证码！" });
                ds.Tables.Clear();
                ds.Tables.Add(dt);
                return ds;
            }
        }

        string UpdateSql = "UPDATE AAA_DLZHXXB SET B_SFYZYX='是',B_SFYXDL='是',B_SFDJ='否',B_SFXM='否' WHERE B_DLYX='" + id + "'";
        try
        {
            int i = DbHelperSQL.ExecuteSql(UpdateSql);
            if (i == 1)
            {
                string SelectSql = "SELECT B_DLYX,B_YHM,B_DLMM,'SUCCESS' AS RESULT,'SUCCESS' AS REASON FROM AAA_DLZHXXB WHERE B_DLYX='" + id + "'";
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
    /// 找回密码（验证用户信息、发送验证码）
    /// </summary>
    /// <param name="user">登录邮箱或手机号码</param>
    /// <param name="typeToSet">找回方式</param>
    /// <returns>Dataset（保存发送信息、验证码等）</returns>
    [WebMethod]
    public DataSet SendValNumber(string user, string typeToSet, string RandomNumber)
    {
        DataSet ds = new DataSet();
        if (typeToSet == "email")
        {
            string email = user;
            DataTable dt_old = DbHelperSQL.Query("SELECT *,'' AS 查询信息,'' AS 本次验证码 FROM AAA_DLZHXXB WHERE B_DLYX='" + user + "' AND B_SFYZYX='是'").Tables[0];
            DataTable dt = dt_old.Copy();
            dt.TableName = "帐号信息";
            ds.Tables.Add(dt);
            if (dt.Rows.Count == 1)
            {
                dt.Rows[0]["查询信息"] = "正常";
                string Number = PublicClass2013.GetNextNumberZZ("AAA_WJMMYZMB", "");
                string Randoms = RandomNumber;//验证码
                string ip = GetIP();//Ip地址
                string time = DateTime.Now.ToString("yyyy-MM-dd");//时间
                string types = typeToSet;//方式
                string isUsed = "false";
                string InsertSqls = "INSERT INTO AAA_WJMMYZMB(Number,subType,userEmail,emailValNumber,phoneValNumber,ipAddress,isUsed,CreateTime) VALUES('" + Number + "','" + types + "','" + email + "','" + Randoms + "','" + " " + "','" + ip + "','" + isUsed + "',GETDATE())";//插入验证码表

                try
                {
                    int i = DbHelperSQL.ExecuteSql(InsertSqls);
                    if (i == 1)
                    {
                        string SelectSqls = "SELECT TOP 1 * FROM AAA_WJMMYZMB WHERE userEmail='" + email + "' AND isUsed='false' ORDER BY CreateTime DESC";
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
                        CE.SendSPEmail(email.Trim(), strSubject, modHtmlFileName, ht);
                        dt.Rows[0]["本次验证码"] = Randoms;
                    }
                }
                catch (Exception ex)
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
                al[i - 1] = "无此用户";
                dt.Rows.Add(new object[] { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, "无此用户", null });
            }
        }
        #region 使用手机找回

        //if (typeToSet == "phone")
        //{
        //    string phone = user;//传过来的手机号
        //    string uid = GetEmailByPhone(phone);//根据手机号得到的登录Email
        //    DataTable dt_old = DbHelperSQL.Query("SELECT *,'' AS 查询信息,'' AS 本次验证码 FROM ZZ_UserLogin WHERE DLYX='" + uid + "'").Tables[0];
        //    DataTable dt = dt_old.Copy();
        //    dt.TableName = "帐号信息";
        //    ds.Tables.Add(dt);
        //    //如果存在这个帐号
        //    if (uid.Trim() != "")
        //    {
        //        dt.Rows[0]["查询信息"] = "正常";
        //        string Randoms = RandomNumber;//验证码
        //        string ip = GetIP();
        //        string time = DateTime.Now.ToString("yyyy-MM-dd");
        //        string types = typeToSet;
        //        string isUsed = "false";
        //        string InsertSqls = "INSERT INTO ZZ_UserValiNumbers(subType,userEmail,emailValNumber,phoneValNumber,ipAddress,isUsed,CreateTime) VALUES('" + types + "','" + uid + "','" + " " + "','" + Randoms + "','" + ip + "','" + isUsed + "',GETDATE())";
        //        try
        //        {
        //            int i = DbHelperSQL.ExecuteSql(InsertSqls);
        //            if (i == 1)
        //            {
        //                string SelectSqls = "SELECT TOP 1 * FROM ZZ_UserValiNumbers WHERE userEmail='" + uid + "' AND isUsed='false' ORDER BY CreateTime DESC";
        //                DataTable dtVals_old = DbHelperSQL.Query(SelectSqls).Tables[0];
        //                DataTable dtVals = dtVals_old.Copy();
        //                dtVals.TableName = "验证码信息";
        //                ds.Tables.Add(dtVals);
        //                string Msg = "亲爱的" + uid + "：您在中国商品批发交易平台申请了手机验证，您的验证码是：" + Randoms + "，验证码30分钟内有效！[中国商品批发交易平台]-";
        //                object[] obj = { phone, Msg };
        //                Key.WebServiceHelper.InvokeWebService("http://192.168.0.10:8183/MessageService.asmx", "SendMessage", obj);
        //                dt.Rows[0]["本次验证码"] = Randoms;
        //            }
        //        }
        //        catch
        //        {
        //            ;
        //        }
        //    }
        //    //如果不存在这个帐号
        //    else
        //    {
        //        int i = dt.Columns.Count;
        //        ArrayList al = new ArrayList();
        //        for (int j = 0; j < i; j++)
        //        {
        //            al.Add(" ");
        //        }
        //        al[i - 1] = "无此用户";
        //        dt.Rows.Add(new object[] { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, "无此用户", null });
        //    }
        //}
        #endregion
        return ds;
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
        string SelectReturn = "SELECT '' AS Uid, '' AS Email,'' AS Phone,'' AS NewPwd,'' AS Results FROM AAA_WJMMYZMB WHERE 1!=1";
        DataTable dtReturnTemp = DbHelperSQL.Query(SelectReturn).Tables[0];//初始化要返回的Datatable的表结构
        string SelectValEmail = "SELECT TOP 1 * FROM AAA_WJMMYZMB WHERE subType='email' AND userEmail='" + uid.Trim() + "' ORDER BY Number DESC";
        DataTable dtValEmail = DbHelperSQL.Query(SelectValEmail).Tables[0];//Email验证码信息
        string SelectValPhone = "SELECT TOP 1 * FROM AAA_WJMMYZMB WHERE subType='phone' AND userEmail='" + uid.Trim() + "' ORDER BY Number DESC";
        DataTable dtValPhone = DbHelperSQL.Query(SelectValPhone).Tables[0];//Phone验证码信息
        DataTable dtSuccess = dtReturnTemp.Clone();//复制表结构（若更改成功）
        dtSuccess.TableName = "Success";
        DataTable dtFalse = dtReturnTemp.Clone();//复制表结构（若更改失败）
        dtFalse.TableName = "False";
        string UpdateSql = "UPDATE AAA_DLZHXXB SET B_DLMM='" + pwd + "' WHERE B_DLYX='" + uid + "'";//要被执行的更新语句
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
    /// 返回当前账号最近一次提货使用的发票信息
    /// </summary>
    /// <param name="DLYX">登录邮箱</param>
    /// <returns>返回当前账号最近一次提货使用的发票信息dsreturn.Tables["主表"]</returns>
    [WebMethod(Description = "返回当前账号最近一次提货使用的发票信息")]
    public DataSet GetTHDFP(string cs)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

 
        //开始执行     
        THDClass thd = new THDClass();
        dsreturn = thd.GetFPXX(cs.ToString(), dsreturn);
 
        return dsreturn;
    }

    [WebMethod(Description = "下达提货单")]
    public DataSet SaveTHD(DataSet ds)
    { 
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        //加锁
        if (jhjx_Lock.LockBegin_ByUser("全局锁") != "1")
        {

            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统繁忙";
            return dsreturn;
        }
        //开始执行     
        THDClass thd = new THDClass();
        dsreturn = thd.SaveTHD(ds, dsreturn);
        //解锁
        jhjx_Lock.LockEnd_ByUser("全局锁");
        return dsreturn;
    }

    [WebMethod(Description = "提货单查看")]
    public DataSet CKTHD(string THDBH)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        //加锁
        if (jhjx_Lock.LockBegin_ByUser("全局锁") != "1")
        {

            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统繁忙";
            return dsreturn;
        }
        //开始执行     
        THDClass thd = new THDClass();
        dsreturn = thd.CKTHD(THDBH, dsreturn);
        //解锁
        jhjx_Lock.LockEnd_ByUser("全局锁");
        return dsreturn;
    }

    [WebMethod(Description = "获取用户关联经纪人的管理机构")]
    public DataSet GetGLJJRGLJG(string DLYX)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        string sql = "SELECT I_PTGLJG AS 关联经纪人管理机构 FROM AAA_DLZHXXB WHERE B_DLYX =(SELECT TOP 1 GLJJRDLZH FROM AAA_DLZHXXB LEFT JOIN AAA_MJMJJYZHYJJRZHGLB ON AAA_DLZHXXB.B_DLYX = AAA_MJMJJYZHYJJRZHGLB.DLYX WHERE B_DLYX='" + DLYX + "' AND AAA_MJMJJYZHYJJRZHGLB.SFDQMRJJR='是' ORDER BY AAA_MJMJJYZHYJJRZHGLB.CreateTime DESC)";
        DataTable dt = DbHelperSQL.Query(sql).Tables[0];
        if (dt.Rows.Count > 0)
        {
            DataTable dtJJR = dt.Copy();
            dtJJR.TableName = "JJR";
            dsreturn.Tables.Add(dtJJR);
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "查询成功！";
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未找到关联经纪人信息，您将无法提交预订单！";
        }
        return dsreturn;
    }

    /// <summary>
    /// 返回资金账户C区查询中的项目、性质下拉框的选项内容
    /// </summary>
    /// <param name="Index">查询标签对应的序号</param>
    /// <param name="type">数据类型：实、预</param>
    /// <returns>返回标签对应的项目、性质数据集</returns>
    [WebMethod(Description = "返回资金查询C区个标签对应的项目、性质选项数据集")]
    public DataSet GetZJCXinfo(string index,string type)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

 
        //开始执行     
        ZJCXCqu thd = new ZJCXCqu();
        Hashtable htres = thd.GetZJCXinfo(index,type);
        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = htres["执行结果"].ToString();
        DataTable dt = new DataTable();
        dt = ((DataTable)htres["返回数据集"]).Copy();
        dt.TableName = "ResDT";
        dsreturn.Tables.Add(dt);
 
        return dsreturn;
    }

    
    [WebMethod(Description = "用户修改密码")]
    public DataSet PsdChange(DataTable dt)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

     
        //开始执行     
        PsdManage pdm = new PsdManage();
        dsreturn =pdm.RunPsdChange(dsreturn,dt);
 
        return dsreturn;
    }

      [WebMethod(Description = "用户修改证券资金密码")]
    public DataSet RunZQZJMMChange(DataTable dt)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

   
        //开始执行     
        PsdManage pdm = new PsdManage();
        dsreturn = pdm.RunZQZJMMChange(dsreturn, dt);
 
        return dsreturn;
    }

    /// <summary>
    /// 返回统计部分的数据内容
    /// </summary>
    /// <param name="dlyx">登陆邮箱</param>
    /// <param name="type">结算账户类型</param>
    /// <returns>返回获取的统计数值</returns>
    [WebMethod(Description = "返回统计部分的数据值")]
    public DataSet GetTongJi(string dlyx, string type)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

 
        //开始执行     
        ZJCXCqu thd = new ZJCXCqu();
        Hashtable htres = thd.GetTongJi(dlyx, type);
        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = htres["执行结果"].ToString();
        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = htres["提示文本"].ToString();
        dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = htres["当前冻结"].ToString();
        dsreturn.Tables["返回值单条"].Rows[0]["附件信息2"] = htres["当前余额"].ToString();
        dsreturn.Tables["返回值单条"].Rows[0]["附件信息3"] = htres["当前收益"].ToString();
 
        return dsreturn;
    }

    [WebMethod(Description = "投标单")]
    public DataSet SetTBD(DataSet ds)
    {
        
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        //加锁
        if (jhjx_Lock.LockBegin_ByUser("全局锁") != "1")
        {

            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统繁忙";
            return dsreturn;
        }
        //开始执行     
        TBD t = new TBD();
        dsreturn = t.SetTBD(ds, dsreturn);
        //解锁
        jhjx_Lock.LockEnd_ByUser("全局锁");
        return dsreturn;
    }

    [WebMethod(Description = "投标单撤销")]
    public DataSet TBD_CX(string Number)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化", "" });

        //加锁
        if (jhjx_Lock.LockBegin_ByUser("全局锁") != "1")
        {

            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统繁忙";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "";
            return dsreturn;
        }
        TBD t = new TBD();
        dsreturn = t.TBD_CX(Number, dsreturn);

        jhjx_Lock.LockEnd_ByUser("全局锁");
        return dsreturn;
    }

    [WebMethod(Description = "预订单撤销")]
    public DataSet YDD_CX(string Number)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化","" });

        //加锁
        if (jhjx_Lock.LockBegin_ByUser("全局锁") != "1")
        {

            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统繁忙";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "";
            return dsreturn;
        }
        YDD y = new YDD();
        dsreturn = y.Ydd_CX(dsreturn, Number);

        jhjx_Lock.LockEnd_ByUser("全局锁");
        return dsreturn;
    }

    [WebMethod(Description = "预订单修改")]
    public DataSet YDD_XG(string Number)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化", "" });


        YDD y = new YDD();
        dsreturn = y.Ydd_Change(dsreturn, Number);

        return dsreturn;
    }
    //TBD_XG

    [WebMethod(Description = "投标单修改")]
    public DataSet TBD_XG(string Number)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化", "" });


        TBD t = new TBD();
        dsreturn = t.Tbd_Change(dsreturn, Number);

        return dsreturn;
    }

    /// <summary>
    /// 下达预订单
    /// </summary>
    /// <param name="dt">各种信息</param>
    /// <returns></returns>
    [WebMethod(Description = "下达预定单")]
    public DataSet SetYDD(DataSet ds)
    {
        DataTable dt = ds.Tables["YDD"];
        bool valFlag = false;
        string info = string.Empty;
        dt = InitValidater.ValYDD(dt, ref valFlag, ref info);

        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
        dsreturn.Tables["返回值单条"].Rows[0]["附件信息2"] = "true";
        ClassMoney2013 AboutMoney = new ClassMoney2013();//与钱相关的类
        Hashtable htPTVal = PublicClass2013.GetParameterInfo();//平台信息
        Hashtable htUserVal = PublicClass2013.GetUserInfo(dt.Rows[0]["MJJSBH"].ToString());//通过买家角色编号获取信息
        Hashtable ht_Sql = new Hashtable();
        #region 验证们

        if (htPTVal["是否在服务时间内(假期)"].ToString().Trim() != "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间(假期)，暂停交易！";
            return dsreturn;
        }
        if (htPTVal["是否在服务时间内(工作时)"].ToString().Trim() != "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间（工作时间），暂停交易！";
            return dsreturn;
        }
        if (htPTVal["是否在服务时间内"].ToString().Trim() != "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间，暂停交易！";
            return dsreturn;
        }
        if (htUserVal["交易账户是否开通"].ToString().Trim() != "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您尚未开通交易账户，不能进行交易操作。";
            return dsreturn;
        }



 
 
        if (htUserVal["冻结功能项"].ToString().Trim().IndexOf("预订单") >= 0)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于冻结状态，请与平台服务人员联系。\n\r被冻结功能：" + htUserVal["冻结功能项"].ToString().Trim();
            return dsreturn;
        }
        if (htUserVal["是否休眠"].ToString().Trim() == "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于休眠状态，请进入账户维护中激活休眠账户！";
            return dsreturn;
        }
        if (htUserVal["是否被默认经纪人暂停新业务"].ToString().Trim() == "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "经纪人已暂停您的新业务，请联系您的经纪人或向平台申诉。";
            return dsreturn;
        }
        
        //判断商品是否有效
        if (!PublicClass2013.isYX(dt.Rows[0]["SPBH"].ToString()))
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您拟买入商品刚刚已下线，该申请单无法提交。请选择其他可拟买入商品。";
            return dsreturn;
        }
        //if (htUserVal["第三方"].ToString() != "开通")
        //{
        //    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
        //    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的第三方存管状态为：" + htUserVal["第三方"].ToString() + "，无法下达预订单。";
        //    return dsreturn;
        //}
        TBD tempr = new TBD();
        Hashtable JJRInfo = tempr.GetUserJJRInfo(dt.Rows[0]["DLYX"].ToString());
        if (JJRInfo != null)
        {
            if (JJRInfo["info"].ToString() == "error")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "关联经纪人信息异常。";
                return dsreturn;
            }
            if (JJRInfo["info"].ToString() == "ok")
            {
                //ht[info](ok/error) | ht[content](错误/正确描述) | ht[关联经纪人邮箱] | ht[关联经纪人用户名] | ht[关联经纪人角色编号] | ht[关联经纪人平台管理机构]
                dt.Rows[0]["GLJJRYX"] = JJRInfo["关联经纪人邮箱"];
                dt.Rows[0]["GLJJRYHM"] = JJRInfo["关联经纪人用户名"];
                dt.Rows[0]["GLJJRJSBH"] = JJRInfo["关联经纪人角色编号"];
                dt.Rows[0]["GLJJRPTGLJG"] = JJRInfo["关联经纪人平台管理机构"];
            }
        }
        //验证余额
        double DJ = Convert.ToDouble(dt.Rows[0]["DJDJ"]);//要冻结的订金
        double YE = AboutMoney.GetMoneyT(dt.Rows[0]["DLYX"].ToString(), "");//当前余额
        if (DJ > YE)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您交易账户中的可用资金余额为" + YE.ToString("0.00") + "元，与该预订单的订金差额为" + (Math.Round((DJ - YE), 2)).ToString("0.00") + "元。您可增加账户的可用资金余额或修改预订单。";
            return dsreturn;
        }
        else if (dt.Rows[0]["FLAG"].ToString().Trim() == "预执行")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的预订单一经提交将冻结" + DJ.ToString("0.00") + "元的订金。";
            return dsreturn;
        }

        if (!valFlag)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = info;
            return dsreturn;
        }
        #endregion

        string Time = DateTime.Now.ToString();//本单统一的时间
        #region 预订单SQL
        string Number_YDD = PublicClass2013.GetNextNumberZZ("AAA_YDDXXB", "");
        string sql_YDD = "INSERT INTO AAA_YDDXXB(Number,DLYX,JSZHLX,MJJSBH,GLJJRYX,GLJJRYHM,GLJJRJSBH,GLJJRPTGLJG,SPBH,SPMC,GG,JJDW,PTSDZDJJPL,HTQX,SHQY,SHQYsheng,SHQYshi,NMRJG,NDGSL,YZBSL,NDGJE,MJDJBL,DJDJ,ZT,TJSJ,CreateTime,YXJZRQ) VALUES(@Number,@DLYX,@JSZHLX,@MJJSBH,@GLJJRYX,@GLJJRYHM,@GLJJRJSBH,@GLJJRPTGLJG,@SPBH,@SPMC,@GG,@JJDW,@PTSDZDJJPL,@HTQX,@SHQY,@SHQYsheng,@SHQYshi,@NMRJG,@NDGSL,@YZBSL,@NDGJE,@MJDJBL,@DJDJ,@ZT,@TJSJ,@CreateTime,@YXJZRQ)";
        if (dt.Rows[0]["YXJZRQ"].ToString() == "")
        {
            SqlParameter[] sp_YDD = {
                                    new SqlParameter("@Number",Number_YDD),
                                    new SqlParameter("@DLYX",dt.Rows[0]["DLYX"].ToString()),
                                    new SqlParameter("@JSZHLX",dt.Rows[0]["JSZHLX"].ToString()),
                                    new SqlParameter("@MJJSBH",dt.Rows[0]["MJJSBH"].ToString()),
                                    new SqlParameter("@GLJJRYX",dt.Rows[0]["GLJJRYX"].ToString()),
                                    new SqlParameter("@GLJJRYHM",dt.Rows[0]["GLJJRYHM"].ToString()),
                                    new SqlParameter("@GLJJRJSBH",dt.Rows[0]["GLJJRJSBH"].ToString()),
                                    new SqlParameter("@GLJJRPTGLJG",dt.Rows[0]["GLJJRPTGLJG"].ToString()),
                                    new SqlParameter("@SPBH",dt.Rows[0]["SPBH"].ToString()),
                                    new SqlParameter("@SPMC",dt.Rows[0]["SPMC"].ToString()),
                                    new SqlParameter("@GG",dt.Rows[0]["GG"].ToString()),
                                    new SqlParameter("@JJDW",dt.Rows[0]["JJDW"].ToString()),
                                    new SqlParameter("@PTSDZDJJPL",dt.Rows[0]["PTSDZDJJPL"].ToString()),
                                    new SqlParameter("@HTQX",dt.Rows[0]["HTQX"].ToString()),
                                    new SqlParameter("@SHQY",dt.Rows[0]["SHQY"].ToString()),
                                    new SqlParameter("@SHQYsheng",dt.Rows[0]["SHQYsheng"].ToString()),
                                    new SqlParameter("@SHQYshi",dt.Rows[0]["SHQYshi"].ToString()),
                                    new SqlParameter("@NMRJG",dt.Rows[0]["NMRJG"].ToString()),
                                    new SqlParameter("@NDGSL",dt.Rows[0]["NDGSL"].ToString()),
                                    new SqlParameter("@YZBSL",dt.Rows[0]["YZBSL"].ToString()),
                                    new SqlParameter("@NDGJE",dt.Rows[0]["NDGJE"].ToString()),
                                    new SqlParameter("@MJDJBL",dt.Rows[0]["MJDJBL"].ToString()),
                                    new SqlParameter("@DJDJ",dt.Rows[0]["DJDJ"].ToString()),
                                    new SqlParameter("@ZT","竞标"),
                                    new SqlParameter("@TJSJ",Time),
                                    new SqlParameter("@CreateTime",Time),
                                    new SqlParameter("@YXJZRQ",null)
                                };
            ht_Sql.Add(sql_YDD, sp_YDD);
        }
        else
        {
            SqlParameter[] sp_YDD = {
                                    new SqlParameter("@Number",Number_YDD),
                                    new SqlParameter("@DLYX",dt.Rows[0]["DLYX"].ToString()),
                                    new SqlParameter("@JSZHLX",dt.Rows[0]["JSZHLX"].ToString()),
                                    new SqlParameter("@MJJSBH",dt.Rows[0]["MJJSBH"].ToString()),
                                    new SqlParameter("@GLJJRYX",dt.Rows[0]["GLJJRYX"].ToString()),
                                    new SqlParameter("@GLJJRYHM",dt.Rows[0]["GLJJRYHM"].ToString()),
                                    new SqlParameter("@GLJJRJSBH",dt.Rows[0]["GLJJRJSBH"].ToString()),
                                    new SqlParameter("@GLJJRPTGLJG",dt.Rows[0]["GLJJRPTGLJG"].ToString()),
                                    new SqlParameter("@SPBH",dt.Rows[0]["SPBH"].ToString()),
                                    new SqlParameter("@SPMC",dt.Rows[0]["SPMC"].ToString()),
                                    new SqlParameter("@GG",dt.Rows[0]["GG"].ToString()),
                                    new SqlParameter("@JJDW",dt.Rows[0]["JJDW"].ToString()),
                                    new SqlParameter("@PTSDZDJJPL",dt.Rows[0]["PTSDZDJJPL"].ToString()),
                                    new SqlParameter("@HTQX",dt.Rows[0]["HTQX"].ToString()),
                                    new SqlParameter("@SHQY",dt.Rows[0]["SHQY"].ToString()),
                                    new SqlParameter("@SHQYsheng",dt.Rows[0]["SHQYsheng"].ToString()),
                                    new SqlParameter("@SHQYshi",dt.Rows[0]["SHQYshi"].ToString()),
                                    new SqlParameter("@NMRJG",dt.Rows[0]["NMRJG"].ToString()),
                                    new SqlParameter("@NDGSL",dt.Rows[0]["NDGSL"].ToString()),
                                    new SqlParameter("@YZBSL",dt.Rows[0]["YZBSL"].ToString()),
                                    new SqlParameter("@NDGJE",dt.Rows[0]["NDGJE"].ToString()),
                                    new SqlParameter("@MJDJBL",dt.Rows[0]["MJDJBL"].ToString()),
                                    new SqlParameter("@DJDJ",dt.Rows[0]["DJDJ"].ToString()),
                                    new SqlParameter("@ZT","竞标"),
                                    new SqlParameter("@TJSJ",Time),
                                    new SqlParameter("@CreateTime",Time),
                                    new SqlParameter("@YXJZRQ",dt.Rows[0]["YXJZRQ"].ToString())
                                };
            ht_Sql.Add(sql_YDD, sp_YDD);
        }
        
        #endregion
        #region 账款流水明细表
        DataTable dt_LS = FMOP.DB.DbHelperSQL.Query("SELECT * FROM AAA_moneyDZB WHERE NUMBER='1304000018'").Tables[0];

        string Number_LS = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
        string sql_LS = "INSERT INTO AAA_ZKLSMXB(Number,DLYX,JSZHLX,JSBH,LYYWLX,LYDH,LSCSSJ,YSLX,JE,XM,XZ,ZY,JKBH,SJLX,CreateUser,CreateTime) VALUES(@Number,@DLYX,@JSZHLX,@JSBH,@LYYWLX,@LYDH,@LSCSSJ,@YSLX,@JE,@XM,@XZ,@ZY,@JKBH,@SJLX,@CreateUser,@CreateTime)";
        SqlParameter[] sp_LS = {
                                 new SqlParameter("@Number",Number_LS),
                                 new SqlParameter("@DLYX",htUserVal["登录邮箱"].ToString()),
                                 new SqlParameter("@JSZHLX",htUserVal["结算账户类型"].ToString()),
                                 new SqlParameter("@JSBH",htUserVal["买家角色编号"].ToString()),
                                 new SqlParameter("@LYYWLX","AAA_YDDXXB"),
                                 new SqlParameter("@LYDH",Number_YDD),
                                 new SqlParameter("@LSCSSJ",DateTime.Now),
                                 new SqlParameter("@YSLX",dt_LS.Rows[0]["YSLX"].ToString().Trim()),
                                 new SqlParameter("@JE",DJ),
                                 new SqlParameter("@XM",dt_LS.Rows[0]["XM"].ToString().Trim()),
                                 new SqlParameter("@XZ",dt_LS.Rows[0]["XZ"].ToString().Trim()),
                                 new SqlParameter("@ZY",dt_LS.Rows[0]["ZY"].ToString().Trim().Replace("[x1]",Number_YDD)),
                                 new SqlParameter("@JKBH","接口编号"),
                                 new SqlParameter("@SJLX",dt_LS.Rows[0]["SJLX"].ToString().Trim()),
                                 new SqlParameter("@CreateUser",htUserVal["登录邮箱"].ToString()),
                                 new SqlParameter("@CreateTime",Time)
                              };
        ht_Sql.Add(sql_LS, sp_LS);
        #endregion
        #region 更新用户余额
        string sql_YE = "UPDATE AAA_DLZHXXB SET B_ZHDQKYYE=B_ZHDQKYYE-" + DJ + " WHERE B_DLYX=@B_DLYX";
        SqlParameter[] sp_YE = {
                                   new SqlParameter("@B_DLYX",htUserVal["登录邮箱"].ToString())
                               };
        ht_Sql.Add(sql_YE, sp_YE);
        #endregion
        try
        {
            #region 再次验证余额，然后执行SQL
            //验证余额
            double DJ_Again = Convert.ToDouble(dt.Rows[0]["DJDJ"]);//要冻结的订金
            double YE_Again = AboutMoney.GetMoneyT(dt.Rows[0]["DLYX"].ToString(), "");//当前余额
            if (DJ_Again > YE_Again)
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您交易账户中的可用资金余额为" + YE + "元，与该预订单的订金差额为" + Math.Round((DJ - YE), 2) + "元。您可增加账户的可用资金余额或修改预订单。";
                return dsreturn;
            }
            else if (dt.Rows[0]["FLAG"].ToString().Trim() == "预执行")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的预订单一经提交将冻结" + DJ + "元的订金。";
                return dsreturn;
            }
            #endregion
            if (DbHelperSQL.ExecuteSqlTran(ht_Sql))
            {
                try
                {
                    if (dt.Rows[0]["HTQX"].ToString() == "即时")
                    {
                        //
                        
                    }
                    else
                    {
                        DataTable dtJK = new DataTable();
                        dtJK.Columns.Add("商品编号");
                        dtJK.Columns.Add("合同期限");
                        dtJK.Rows.Add(new string[] { dt.Rows[0]["SPBH"].ToString(), dt.Rows[0]["HTQX"].ToString() });
                        CJGZ2013 jk = new CJGZ2013();
                        ArrayList al = jk.RunCJGZ(dtJK, "提交");
                        DbHelperSQL.ExecSqlTran(al);
                    }                    
                }
                catch (Exception exJK)
                {
                    //监控异常信息开始
                    string jkMsg = exJK.Message;
                    //监控异常信息结束
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "本次预订单提交成功！";
                    return dsreturn;
                }
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "本次预订单提交成功！";
                return dsreturn;
            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统繁忙，请稍后再试！";
                return dsreturn;
            }
        }
        catch
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统繁忙，请稍后再试！";
            return dsreturn;
        }
    }

    /// <summary>
    /// 下达预订单草稿
    /// </summary>
    /// <param name="dt">各种信息</param>
    /// <returns></returns>
    [WebMethod(Description = "下达预定单")]
    public DataSet SetYDDCG(DataSet ds)
    {
        DataTable dt = ds.Tables["YDD"];
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        Hashtable htUserVal = PublicClass2013.GetUserInfo(dt.Rows[0]["买家角色编号"].ToString());//通过买家角色编号获取信息

        if (htUserVal["是否休眠"].ToString().Trim() == "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于休眠状态，请进入账户维护中激活休眠账户！";
            return dsreturn;
        }


        Hashtable ht_Sql = new Hashtable();

        string Time = DateTime.Now.ToString();//本单统一的时间
        #region 预订单SQL
        string Number_YDD = PublicClass2013.GetNextNumberZZ("AAA_YDDCGB", "");
        string sql_YDD = "INSERT INTO AAA_YDDCGB(Number,DLYX,SPBH,HTQX,SHQY,SHQYsheng,SHQYshi,NMRJG,NDGSL,NDGJE,TJSJ,CreateTime,YXJZRQ) VALUES(@Number,@DLYX,@SPBH,@HTQX,@SHQY,@SHQYsheng,@SHQYshi,@NMRJG,@NDGSL,@NDGJE,@TJSJ,@CreateTime,@YXJZRQ)";
        if (dt.Rows[0]["YXJZRQ"].ToString()=="")
        {
            SqlParameter[] sp_YDD = {
                                    new SqlParameter("@Number",Number_YDD),
                                    new SqlParameter("@DLYX",dt.Rows[0]["DLYX"].ToString()),
                                    new SqlParameter("@SPBH",dt.Rows[0]["SPBH"].ToString()),
                                    new SqlParameter("@HTQX",dt.Rows[0]["HTQX"].ToString()),
                                    new SqlParameter("@SHQY",dt.Rows[0]["SHQY"].ToString()),
                                    new SqlParameter("@SHQYsheng",dt.Rows[0]["SHQYsheng"].ToString()),
                                    new SqlParameter("@SHQYshi",dt.Rows[0]["SHQYshi"].ToString()),
                                    new SqlParameter("@NMRJG",dt.Rows[0]["NMRJG"].ToString()),
                                    new SqlParameter("@NDGSL",dt.Rows[0]["NDGSL"].ToString()),
                                    new SqlParameter("@NDGJE",dt.Rows[0]["NDGJE"].ToString()),
                                    new SqlParameter("@TJSJ",Time),
                                    new SqlParameter("@CreateTime",Time),
                                    new SqlParameter("@YXJZRQ",null)
                                };
            ht_Sql.Add(sql_YDD, sp_YDD);
        }
        else
        {
            SqlParameter[] sp_YDD = {
                                    new SqlParameter("@Number",Number_YDD),
                                    new SqlParameter("@DLYX",dt.Rows[0]["DLYX"].ToString()),
                                    new SqlParameter("@SPBH",dt.Rows[0]["SPBH"].ToString()),
                                    new SqlParameter("@HTQX",dt.Rows[0]["HTQX"].ToString()),
                                    new SqlParameter("@SHQY",dt.Rows[0]["SHQY"].ToString()),
                                    new SqlParameter("@SHQYsheng",dt.Rows[0]["SHQYsheng"].ToString()),
                                    new SqlParameter("@SHQYshi",dt.Rows[0]["SHQYshi"].ToString()),
                                    new SqlParameter("@NMRJG",dt.Rows[0]["NMRJG"].ToString()),
                                    new SqlParameter("@NDGSL",dt.Rows[0]["NDGSL"].ToString()),
                                    new SqlParameter("@NDGJE",dt.Rows[0]["NDGJE"].ToString()),
                                    new SqlParameter("@TJSJ",Time),
                                    new SqlParameter("@CreateTime",Time),
                                    new SqlParameter("@YXJZRQ",dt.Rows[0]["YXJZRQ"].ToString())
                                };
            ht_Sql.Add(sql_YDD, sp_YDD);
        }
        
        #endregion
        try
        {
            DbHelperSQL.ExecuteSqlTran(ht_Sql);//事务执行SQL
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "存入预订单草稿成功！";
            return dsreturn;
        }
        catch
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统繁忙，请稍后再试！";
            return dsreturn;
        }
    }

    [WebMethod(Description="下达投标单草稿")]
    public DataSet SetTBDCG(DataSet ds)
    {
        DataTable dt = ds.Tables["TBD"];
        DataTable dtZZ = ds.Tables["ZZ"];//资质表
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        Hashtable htUserVal = PublicClass2013.GetUserInfo(dt.Rows[0]["买家角色编号"].ToString());//通过买家角色编号获取信息

        if (htUserVal["是否休眠"].ToString().Trim() == "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于休眠状态，请进入账户维护中激活休眠账户！";
            return dsreturn;
        }


        Hashtable ht_Sql = new Hashtable();
        #region 投标单SQL
        string Time = DateTime.Now.ToString();
        Hashtable ht_ZZ = new Hashtable(); //dt.Rows[0]["其他资质"] == null ? null : (Hashtable)dt.Rows[0]["其他资质"];
        if (dtZZ != null)
        {
            for (int i = 0; i < dtZZ.Columns.Count; i++)
            {
                ht_ZZ[dtZZ.Columns[i].ColumnName] = dtZZ.Rows[0][dtZZ.Columns[i].ColumnName].ToString();
            }
        }
        string Number_TBD = PublicClass2013.GetNextNumberZZ("AAA_TBDCGB", "");
        string sql_TBD = "INSERT INTO AAA_TBDCGB(Number,DLYX,SPBH,HTQX,MJSDJJPL,GHQY,TBNSL,TBJG,TBJE,TJSJ,ZLBZYZM,CPJCBG,PGZFZRFLCNS,FDDBRCNS,SHFWGDYCN,CPSJSQS,ZZ01,ZZ02,ZZ03,ZZ04,ZZ05,ZZ06,ZZ07,ZZ08,ZZ09,ZZ10,CreateUser,CreateTime,SLZM,SPCD) VALUES(@Number,@DLYX,@SPBH,@HTQX,@MJSDJJPL,@GHQY,@TBNSL,@TBJG,@TBJE,@TJSJ,@ZLBZYZM,@CPJCBG,@PGZFZRFLCNS,@FDDBRCNS,@SHFWGDYCN,@CPSJSQS,@ZZ01,@ZZ02,@ZZ03,@ZZ04,@ZZ05,@ZZ06,@ZZ07,@ZZ08,@ZZ09,@ZZ10,@CreateUser,@CreateTime,@SLZM,@SPCD)";
        SqlParameter[] sp_TBD = { 
                                    new SqlParameter("@Number",Number_TBD),
                                    new SqlParameter("@DLYX",dt.Rows[0]["DLYX"].ToString()),
                                    new SqlParameter("@SPBH",dt.Rows[0]["SPBH"].ToString()),
                                    new SqlParameter("@HTQX",dt.Rows[0]["HTQX"].ToString()),
                                    new SqlParameter("@MJSDJJPL",dt.Rows[0]["MJSDJJPL"].ToString()),
                                    new SqlParameter("@GHQY",dt.Rows[0]["GHQY"].ToString()),
                                    new SqlParameter("@TBNSL",dt.Rows[0]["TBNSL"].ToString()),
                                    new SqlParameter("@TBJG",dt.Rows[0]["TBJG"].ToString()),
                                    new SqlParameter("@TBJE",dt.Rows[0]["TBJE"].ToString()),
                                    new SqlParameter("@TJSJ",Time),
                                    new SqlParameter("@ZLBZYZM",dt.Rows[0]["ZLBZYZM"]==null?null:dt.Rows[0]["ZLBZYZM"].ToString()),
                                    new SqlParameter("@CPJCBG",dt.Rows[0]["CPJCBG"]==null?null:dt.Rows[0]["CPJCBG"].ToString()),
                                    new SqlParameter("@PGZFZRFLCNS",dt.Rows[0]["PGZFZRFLCNS"]==null?null:dt.Rows[0]["PGZFZRFLCNS"].ToString()),
                                    new SqlParameter("@FDDBRCNS",dt.Rows[0]["FDDBRCNS"]==null?null:dt.Rows[0]["FDDBRCNS"].ToString()),
                                    new SqlParameter("@SHFWGDYCN",dt.Rows[0]["SHFWGDYCN"]==null?null:dt.Rows[0]["SHFWGDYCN"].ToString()),
                                    new SqlParameter("@CPSJSQS",dt.Rows[0]["CPSJSQS"]==null?null:dt.Rows[0]["CPSJSQS"].ToString()),
                                    new SqlParameter("@CreateUser",dt.Rows[0]["DLYX"].ToString()),
                                    new SqlParameter("@CreateTime",Time),
                                    new SqlParameter("@ZZ01",ht_ZZ["资质1"] == null?"":ht_ZZ["资质1"].ToString()),
                                    new SqlParameter("@ZZ02",ht_ZZ["资质2"] == null?"":ht_ZZ["资质2"].ToString()),
                                    new SqlParameter("@ZZ03",ht_ZZ["资质3"] == null?"":ht_ZZ["资质3"].ToString()),
                                    new SqlParameter("@ZZ04",ht_ZZ["资质4"] == null?"":ht_ZZ["资质4"].ToString()),
                                    new SqlParameter("@ZZ05",ht_ZZ["资质5"] == null?"":ht_ZZ["资质5"].ToString()),
                                    new SqlParameter("@ZZ06",ht_ZZ["资质6"] == null?"":ht_ZZ["资质6"].ToString()),
                                    new SqlParameter("@ZZ07",ht_ZZ["资质7"] == null?"":ht_ZZ["资质7"].ToString()),
                                    new SqlParameter("@ZZ08",ht_ZZ["资质8"] == null?"":ht_ZZ["资质8"].ToString()),
                                    new SqlParameter("@ZZ09",ht_ZZ["资质9"] == null?"":ht_ZZ["资质9"].ToString()),
                                    new SqlParameter("@ZZ10",ht_ZZ["资质10"] == null?"":ht_ZZ["资质10"].ToString()),
                                    new SqlParameter("@SLZM",dt.Rows[0]["SLZM"] == null?null:dt.Rows[0]["SLZM"].ToString()),
                                     new SqlParameter("@SPCD",dt.Rows[0]["省市区"].ToString())

                                };
        #endregion
        ht_Sql.Add(sql_TBD, sp_TBD);
        try
        {
            DbHelperSQL.ExecuteSqlTran(ht_Sql);//事务执行SQL
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "存入投标单草稿成功！";
            return dsreturn;
        }
        catch
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统繁忙，请稍后再试！";
            return dsreturn;
        }
    }


    /// <summary>
    /// 生成发货单
    /// </summary>
    /// <param name="ds"></param>
    /// <returns></returns>
    [WebMethod(Description = "生成发货单")]
    public DataSet SCFHD(DataSet ds)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        //加锁
        if (jhjx_Lock.LockBegin_ByUser("全局锁") != "1")
        {

            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统繁忙";
            return dsreturn;
        }
        //开始执行     
        FHDClass fhd = new FHDClass();
        dsreturn = fhd.SCFHD(ds, dsreturn);
        //解锁
        jhjx_Lock.LockEnd_ByUser("全局锁");
        return dsreturn;
    }

    [WebMethod(Description = "查看发货单")]
    public DataSet CKFHD(string FHDBH)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

 
        //开始执行   
        FHDClass fhd = new FHDClass();
        dsreturn = fhd.CXFHD(FHDBH, dsreturn);
 
        return dsreturn;
    }

    [WebMethod(Description = "用户账户休眠后自己激活账户")]
    public DataSet AccountActivate(DataTable dt)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        //加锁
        if (jhjx_Lock.LockBegin_ByUser("全局锁") != "1")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统繁忙";
            return dsreturn;
        }
        //开始执行     
        ActManage am = new ActManage();
        dsreturn = am.RunAccountActive(dsreturn, dt);
        //解锁
        jhjx_Lock.LockEnd_ByUser("全局锁");
        return dsreturn;
    }

    [WebMethod(Description = "录入发货信息")]
    public DataSet LRFHXX(DataSet ds)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        //加锁
        if (jhjx_Lock.LockBegin_ByUser("全局锁") != "1")
        {

            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统繁忙";
            return dsreturn;
        }
        //开始执行   
        FHDClass fhd = new FHDClass();
        dsreturn = fhd.LRFHXX(ds, dsreturn);
        //解锁
        jhjx_Lock.LockEnd_ByUser("全局锁");
        return dsreturn;
    }

    [WebMethod(Description = "录入发票信息")]
    public DataSet LRFPXX(DataSet ds)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        //加锁
        if (jhjx_Lock.LockBegin_ByUser("全局锁") != "1")
        {

            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统繁忙";
            return dsreturn;
        }
        //开始执行   
        FHDClass fhd = new FHDClass();
        dsreturn = fhd.LRFPXX(ds, dsreturn);
        //解锁
        jhjx_Lock.LockEnd_ByUser("全局锁");
        return dsreturn;
    }

    [WebMethod(Description = "卖家提请买家签收")]
    public DataSet InsertSelTQBuyQS(DataSet ds)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        //加锁
        if (jhjx_Lock.LockBegin_ByUser("全局锁") != "1")
        {

            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统繁忙";
            return dsreturn;
        }
        //开始执行   
        TQBuyQSClass qs = new TQBuyQSClass();
        dsreturn = qs.InsertSQL(ds, dsreturn);
        //解锁
        jhjx_Lock.LockEnd_ByUser("全局锁");
        return dsreturn;
    }

    /// <summary>
    /// 执行超期未缴纳履约保证金监控
    /// </summary>
    /// <returns></returns>
    [WebMethod(Description="执行超期未缴纳履约保证金监控")]
    public DataSet serverWJNLYBZJJK()
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        try
        {
            CQWJNLYBZJJK ck = new CQWJNLYBZJJK();
            Hashtable ht_result = ck.ServerCQWJNLYBZJJK();

            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = ht_result["执行结果"].ToString();
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = ht_result["提示文本"].ToString();
        }
        catch (Exception ex)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = ex.ToString();
        }
        return dsreturn;
    }
    /// <summary>
    /// 执行合同到期处理监控
    /// </summary>
    /// <returns></returns>
    [WebMethod(Description="执行合同到期处理监控")]
    public DataSet ServerHTQMJK()
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        try
        {
            HTQMJK ck = new HTQMJK();
            Hashtable ht_result = ck.ServerHTQMJK();

            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = ht_result["执行结果"].ToString();
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = ht_result["提示文本"].ToString();
        }
        catch (Exception ex)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = ex.ToString();
        }
        return dsreturn;
    }

    /// <summary>
    /// 执行每月经纪人扣税
    /// </summary>
    /// <returns></returns>
    [WebMethod(Description = "每月经纪人扣税后台监控")]
    public DataSet ServerMYJJRKS()
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        try
        {
            KTJYZHClass ck = new KTJYZHClass();
           dsreturn= ck.ServerMYJJRKS(dsreturn);
           return dsreturn;
        }
        catch (Exception ex)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = ex.ToString();
        }
        return dsreturn;
    }






    /// <summary>
    /// 提醒后自动签收
    /// </summary>
    /// <returns></returns>
    [WebMethod]
    public DataSet serverRunZDQS()
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        try
        {
            //执行语句     
            MRWYYSH2013 zdqs = new MRWYYSH2013();
            dsreturn = zdqs.MRWYYQS(dsreturn);
            return dsreturn;

        }
        catch (Exception ex)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = ex.ToString();
            return dsreturn;
        }
    }

    [WebMethod(Description = "问题与处理")]
    public DataSet WTYCL(DataSet ds)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        //加锁
        if (jhjx_Lock.LockBegin_ByUser("全局锁") != "1")
        {

            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统繁忙";
            return dsreturn;
        }
        //开始执行   
        WTYCLClass wtcl = new WTYCLClass();
        dsreturn = wtcl.WTYCL(ds, dsreturn);
        //解锁
        jhjx_Lock.LockEnd_ByUser("全局锁");
        return dsreturn;
    }

    /// <summary>
    /// 获取用户账户的最新状态，用于验证
    /// </summary>
    /// <param name="dlyx">登陆邮箱</param>
    /// <returns></returns>
    [WebMethod(Description = "获取用户账户的最新状态，用于验证")]
    public DataSet GetNewUserState(string dlyx)
    {
        DataSet ds = DbHelperSQL.Query("SELECT B_YHM AS 用户名,B_JSZHLX AS 结算账户类型,B_SFYXDL AS 是否允许登录,B_SFDJ AS 是否冻结,B_SFXM AS 是否休眠,B_ZHDQKYYE AS 账户当前可用余额,J_BUYJSBH AS 买家角色编号 FROM AAA_DLZHXXB WHERE B_DLYX='" + dlyx + "'");

        return ds;
    }

    /// <summary>
    /// 获取中标定标信息表上传文件的信息，用于验证
    /// </summary>
    /// <param name="num">中标定标信息表Number</param>
    /// <returns></returns>
    [WebMethod(Description = "获取中标定标信息表上传文件的信息，用于验证")]
    public DataSet GetSHWJinfo(string num)
    {
        DataSet ds = DbHelperSQL.Query("SELECT Q_QPZMSCSJ AS 清盘证明上传时间,Q_ZMSCFDLYX AS 证明上传方登录邮箱,Q_ZMSCFJSZHLX 证明上传方结算账户类型,Q_ZMSCFJSBH AS 证明上传方角色编号,Q_ZMWJLJ AS 证明文件路径,Q_ZFLYZH AS 转付来源账户,Q_ZFMBZH AS 转付目标账户,Q_ZFJE AS 转付金额,Q_SFYQR AS 是否已确认,Q_QRSJ AS 确认时间 FROM AAA_ZBDBXXB WHERE Number='" + num + "'");
        return ds;
    }

    /// <summary>
    ///根据中标定标信息表编号获得投标资料审核表中的相关信息
    /// </summary>
    /// <param name="num">中标定标信息表Number</param>
    /// <returns></returns>
    [WebMethod(Description = "根据中标定标信息表编号获得投标资料审核表中的数据信息")]
    public DataSet GetTBZLSHinfo(string num)
    {
        DataSet ds = DbHelperSQL.Query("select ZBDB.T_YSTBDDLYX '原始投标单登录邮箱',TBSH.FWZXSHZT '服务中心审核状态',TBSH.JYGLBSHZT '交易管理部审核状态' from AAA_TBZLSHB as TBSH left join AAA_ZBDBXXB as ZBDB on TBSH.TBDH=ZBDB.T_YSTBDBH where ZBDB.Number='" + num + "'");
        return ds;
    }

    /// <summary>
    /// 删除服务器上已经上传的文件
    /// </summary>
    /// <param name="filepath">路径</param>
    /// <returns></returns>
    [WebMethod(Description = "删除服务器上已经上传的文件")]
    public DataSet DelPicture(string filepath)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

 

        //首先从服务器删除已经上传的图片
        if (filepath != "")
        {
            try
            {
                string path = HttpContext.Current.Server.MapPath("/JHJXPT/SaveDir/" + filepath.ToString().Trim());
                System.IO.File.Delete(path);
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "文件删除成功！";

            }
            catch (Exception ex)
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = ex.Message;
            }

        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未找到文件";

        }
        return dsreturn;

    }

    [WebMethod(Description = "问题与处理")]
    public DataSet HWqs_ycinfo(DataTable ds)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

 
        //开始执行   
        jhjx_getyichanginfo wtcl = new jhjx_getyichanginfo();
        dsreturn = wtcl.Getyiwaiinfo(dsreturn, ds);
 
        return dsreturn;
    }

    /// <summary>
    /// 人工清盘时用户上传证明文件写入数据表
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    [WebMethod(Description = "上传争议证明文件")]
    public DataSet DWControversial(DataTable dt)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });


 
        //开始执行     
        CVManage cv = new CVManage();
        dsreturn = cv.RunFileData(dsreturn, dt);
 

        return dsreturn;

    }

    [WebMethod(Description = "争议证明文件的确认")]
    public DataSet DWConfirm(DataTable dt)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });


 
        //开始执行     
        CVManage cv = new CVManage();
        dsreturn = cv.RunConfirmData(dsreturn, dt);
 

        return dsreturn;
    }

    [WebMethod(Description = "获取预订单草稿信息")]
    public DataSet GetYDDcaogaoinfo(string Number)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });


 
        //开始执行     TBD_GetCGinfo
        YDD cv = new YDD();
        dsreturn = cv.YDD_Getcaogao(dsreturn, Number);
 

        return dsreturn;
    }


    [WebMethod(Description = "获取投标单草稿信息")]
    public DataSet TBD_GetCGinfo(string Number)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

 
        //开始执行     TBD_GetCGinfo
        TBD cv = new TBD();

        dsreturn = cv.TBD_GetCGinfo(dsreturn, Number);
 

        return dsreturn;
    }


    [WebMethod(Description = "获取定标保证函详细信息")]
    public DataSet TBDYDDbzh_GetCGinfo(string Number)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });


 
        //开始执行     TBD_GetCGinfo
        Jhjxpt_db cv = new Jhjxpt_db();

        dsreturn = cv.jhjx_DBYbzhinfo(dsreturn, Number);
 

        return dsreturn;
    }

    [WebMethod(Description = "获取定标中标信息表详细信息")]
    public DataSet DBZBXXinfoget(string Number)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });


 
        //开始执行     TBD_GetCGinfo
        Jhjxpt_db cv = new Jhjxpt_db();

        dsreturn = cv.jhjx_DBZBXXinfoget(dsreturn, Number);
 

        return dsreturn;
    }
    /// <summary>
    /// 更改账户余额的包装方法，方便业务平台调用
    /// </summary>
    /// <param name="dlyx"></param>
    /// <param name="money"></param>
    /// <param name="IsF">+-号</param>
    /// <returns></returns>
    [WebMethod(Description = "更改账户余额的方法")]
    public bool AccontMoney(string dlyx, double money)
    {  
        //加锁
        if (jhjx_Lock.LockBegin_ByUser("全局锁") != "1")
        {
            return false;
        }
        //开始执行     TBD_GetCGinfo
        ClassMoney2013 cs = new ClassMoney2013();
        bool b = true;
        if (money != 0)
        {
            string fh = money < 0 ? "-" : "+";
            b = cs.FreezeMoneyT(dlyx, Math.Abs(money).ToString(), fh);
        }        
       
        //解锁
        jhjx_Lock.LockEnd_ByUser("全局锁");
        return b;
    }
    /// <summary>
    /// 更改账户余额的包装方法，方便业务平台调用（重载）
    /// </summary>
    /// <param name="buydlyx"></param>
    /// <param name="buychange"></param>
    /// <param name="selldlyx"></param>
    /// <param name="sellchange"></param>
    /// <returns></returns>
    [WebMethod(Description = "更改账户余额的方法")]
    public bool AccontsMoneys(string buydlyx, double buychange, string selldlyx, double sellchange)
    {
        //加锁
        if (jhjx_Lock.LockBegin_ByUser("全局锁") != "1")
        {
            return false;
        }
        //开始执行     TBD_GetCGinfo
        bool sbool = true, bbool = true;
        ClassMoney2013 cs = new ClassMoney2013();
        if (buychange != 0)
        {
            string buyfh = buychange < 0 ? "-" : "+";
            bbool= cs.FreezeMoneyT(buydlyx, Math.Abs(buychange).ToString(), buyfh);
        }
        if(sellchange !=0)
        {
            string sellfh = sellchange < 0 ? "-" : "+";
            sbool= cs.FreezeMoneyT(selldlyx, Math.Abs(sellchange).ToString(), sellfh);
        }    

        //解锁
        jhjx_Lock.LockEnd_ByUser("全局锁");

        return bbool && sbool;
     


    }
    /// <summary>
    /// 用户银行存款与用户账户之间的资金转账
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    [WebMethod(Description = "用户多银行框架--资金转账--已移植")]
    public DataSet TransferAccounts(DataTable dt)
    {

        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "操作失败！" });

        if (dt == null || dt.Rows.Count <1)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "传递参数不可为空！";
            return dsreturn;
        }


        //条件验证
        Hashtable ht = new Hashtable();
        ht["DLYX"] = dt.Rows[0]["用户邮箱"].ToString();
        ht["JYZJMM"] = dt.Rows[0]["交易资金密码"].ToString();
        Hashtable htreul = JHJX_YanZhengClass.SFTJKTSQ(ht);
        if (!htreul["info"].ToString().Equals("ok"))
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = htreul["info"].ToString();
            return dsreturn;
        }

       
        if (!htreul["是否提交开通申请"].ToString().Equals("是"))
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您尚未提交开通交易账申请，请及时提交！";
            return dsreturn;
        }

        if (htreul["是否提交开通申请"].ToString().Equals("审核中"))
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的开户申请正在审核中，请耐心等待！";
            return dsreturn;
        }

        if (htreul["是否提交开通申请"].ToString().Equals("驳回"))
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的开户申请审核未通过，请您进入“账户维护”界面查询详情并重新提交申请！";
            return dsreturn;
        }

        if (!htreul["交易资金密码正确"].ToString().Equals("是"))
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "请输入正确交易资金密码！";
            return dsreturn;
        }

        if (!htreul["第三方存管状态是否开通"].ToString().Equals("是"))
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您当前未绑定第三方存管银行，请到相关银行开通！";
            return dsreturn;
        }

        if (!htreul["是否交易时间内"].ToString().Equals("是"))
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间，暂停交易！";
            return dsreturn;
        }
        if (!htreul["是否交易日期内"].ToString().Equals("是"))
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易日期，暂停交易！";
            return dsreturn;
        }

       
        IBank IB  = null;

        if (dt.Rows[0]["转账类别"].ToString().Equals("银转商"))
        {        
              IB = new IBank(dt.Rows[0]["用户邮箱"].ToString(), "入金");
              
        }
        else if (dt.Rows[0]["转账类别"].ToString().Equals("商转银"))
        {
            if (!htreul["是否休眠"].ToString().Equals("否"))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于休眠状态，请进入账户维护中激活休眠账户！";
                return dsreturn;
            }
            if (!htreul["当前账户是否冻结出金"].ToString().Equals("否"))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于冻结状态，请与平台服务人员联系！被冻结功能：" + htreul["被冻结项"].ToString();
                return dsreturn;
            }

            if (!htreul["是否存在两次出金失败未处理记录"].ToString().Equals("否"))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "银行转账系统正在抢修，请耐心等待！";
                return dsreturn;
            }

            IB = new IBank(dt.Rows[0]["用户邮箱"].ToString(), "出金");
            
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "转账类别不正确";
            return dsreturn;
        }

        if (jhjx_Lock.LockBegin_ByUser("全局锁") != "1") //加锁
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统繁忙（锁）";
            return dsreturn;
        }
        DataSet dspara = new DataSet();
        dspara.Tables.Add(dt);
        DataSet dsresult = IB.Invoke(dspara);
        if (dsresult != null && dsresult.Tables[0].Rows.Count > 0)
        {
            dsreturn = dsresult;
        }
        
          
            //解锁
       jhjx_Lock.LockEnd_ByUser("全局锁");
       
        return dsreturn;


    }

    /// <summary>
    /// 用户银行存款与用户账户之间的资金转账
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    [WebMethod(Description = "资金转账查询最大可转金额")]
    public DataSet MaxZZJE(DataTable dt)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });


 
        //开始执行     TBD_GetCGinfo
        TansfMoney tm = new TansfMoney();
        dsreturn = tm.MaxZZJE(dsreturn, dt);

 

        return dsreturn;
    }

    [WebMethod(Description = "获取单条人工清盘记录")]
    public DataSet GetQBInfo(DataTable dt)
    {
        DataSet ds = null;
        if (dt != null && dt.Rows.Count > 0)
        {

            Hashtable htUserVal = PublicClass2013.GetUserInfo(dt.Rows[0]["买家角色编号"].ToString());//通过买家角色编号获取信息

            if (htUserVal["是否休眠"].ToString().Trim() == "是")
            {
                DataSet dsreturn = initReturnDataSet().Clone();
                dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于休眠状态，请进入账户维护中激活休眠账户！";
                return dsreturn;
            }

            string dlyx = dt.Rows[0]["dlyx"].ToString().Trim();
            string num = dt.Rows[0]["num"].ToString().Trim();

            string str = "SELECT  AAA_ZBDBXXB.Number AS 主键 ,Z_QPZT AS 清盘状态,Z_QPKSSJ AS 清盘开始时间,Z_QPJSSJ AS 清盘结束时间, Z_HTBH AS 电子购货合同编号,Z_HTJSRQ AS 合同结束日期,Z_SPMC AS 商品名称,Z_SPBH AS 商品编号,Z_GG AS 规格,Z_ZBSL AS 合同数量,Z_ZBJG AS 单价,Z_ZBJE AS 合同金额,Z_LYBZJJE AS 履约保证金金额,  CAST( SUM( T_DJHKJE/ZBDJ) AS numeric(18,2)) AS 争议数量 ,SUM(T_DJHKJE) AS  争议金额,'人工清盘' AS 清盘类型,CASE WHEN Z_HTZT='未定标废标' THEN '废标' WHEN Z_HTZT='定标合同到期' THEN '《电子购货合同》期满' WHEN Z_HTZT='定标合同终止' THEN '废标' WHEN Z_HTZT='定标执行完成' THEN '合同期内买家中标量全部无异议收货' ELSE '其他' END AS 清盘原因 ,T_YSTBDDLYX AS 卖方邮箱,Y_YSYDDDLYX AS 买方邮箱,Q_ZMSCFDLYX AS 证明上传方邮箱,Q_SFYQR AS 是否已确认 ,Q_ZMWJLJ AS 证明文件路径, Q_ZFLYZH AS 转付来源账户, Q_ZFMBZH AS 转付目标账户,Q_ZFJE AS 转付金额,Q_CLYJ AS 处理依据  FROM AAA_THDYFHDXXB JOIN AAA_ZBDBXXB ON AAA_ZBDBXXB.Number=AAA_THDYFHDXXB.ZBDBXXBBH WHERE (T_YSTBDDLYX='" + dlyx + "' OR Y_YSYDDDLYX='" + dlyx + "') AND AAA_ZBDBXXB.Number='" + num + "' AND Z_QPZT='清盘中' AND F_DQZT NOT IN ('无异议收货','默认无异议收货','有异议收货后无异议收货','补发货物无异议收货','撤销','卖家主动退货') GROUP BY  Z_HTBH,Z_HTJSRQ,Z_SPMC,Z_SPBH,Z_GG,Z_ZBSL,Z_ZBJG,Z_ZBJE,CASE WHEN Z_HTZT='未定标废标' THEN '废标' WHEN Z_HTZT='定标合同到期' THEN '《电子购货合同》期满' WHEN Z_HTZT='定标合同终止' THEN '废标' WHEN Z_HTZT='定标执行完成' THEN '合同期内买家中标量全部无异议收货' ELSE '其他' END,AAA_ZBDBXXB.Number,Z_LYBZJJE,Z_QPZT,T_YSTBDDLYX ,Y_YSYDDDLYX ,Z_QPKSSJ ,Z_QPJSSJ ,Q_ZMSCFDLYX,Q_SFYQR ,Q_ZMWJLJ, Q_ZFLYZH , Q_ZFMBZH ,Q_ZFJE ,Q_CLYJ ";

            ds = DbHelperSQL.Query(str);


        }
        return ds;
    }


    /// <summary>
    /// 得到电子购货合同相关的数据信息
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    [WebMethod]
    public DataSet GetDZGHHTXX(DataTable dt)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
        KTJYZHClass ktjyzh = new KTJYZHClass();
          dsreturn=ktjyzh.GetDZGHHTXX(dsreturn, dt);
        return dsreturn;
    }

    //[WebMethod]
    //public DataSet CompareUInfo(DataSet ds)
    //{
    //    //初始化返回值,先塞一行数据
    //    DataSet dsreturn = initReturnDataSet().Clone();
    //    dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });  
    //    SocketListener s = new SocketListener();
    //    dsreturn = s.CompareUserInfo(ds, dsreturn);
    //    return dsreturn;
    //}
    [WebMethod(Description = "适用于多银行框架--银行发起的客户签约处理--已移植")]
    public DataSet CompareUInfo(DataSet ds, string khbh,string ssbank)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
        
        //根据传过来的客户编号，获取客户邮箱
        DataSet dsuserinfo = DbHelperSQL.Query("select b_dlyx from AAA_DLZHXXB where I_ZQZJZH='" + khbh + "'");
        if (dsuserinfo == null || dsuserinfo.Tables[0].Rows.Count <= 0)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "交易方编号不存在！";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2302";
        }
        else if (dsuserinfo.Tables[0].Rows.Count != 1)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = " 交易方编号重复！";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2302";
        }
        else
        {
            Hashtable ht = new Hashtable();
            ht["DLYX"] = dsuserinfo.Tables[0].Rows[0]["b_dlyx"].ToString();
            Hashtable htre = JHJX_YanZhengClass.SFTJKTSQ(ht);

            if (htre["第三方存管状态是否开通"].ToString().Equals("是"))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "资金账户与银行方账户已建立对应关系";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2014";

                return dsreturn;
            }

            if (jhjx_Lock.LockBegin_ByUser("全局锁") != "1") //加锁
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统繁忙（锁）";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";
                return dsreturn;
            }

            IBank IB = new IBank(dsuserinfo.Tables[0].Rows[0]["b_dlyx"].ToString(), "客户签约确认");
            string strinfo = ssbank;
            dsreturn = IB.Invoke(ds, ref strinfo);
            if (!strinfo.Equals("ok"))
            {
                dsreturn = initReturnDataSet().Clone();
                dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = strinfo;
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";
                return dsreturn;
            }

            //解锁
            jhjx_Lock.LockEnd_ByUser("全局锁");
        }
        return dsreturn;
    }

    //[WebMethod(Description="查询客户余额（银行发起）")]
    //public DataSet GetUMoneyByBANK(DataSet ds)
    //{
    //    //初始化返回值,先塞一行数据
    //    DataSet dsreturn = initReturnDataSet().Clone();
    //    dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
    //    SocketListener s = new SocketListener();
    //    dsreturn = s.GetUserMoney(ds, dsreturn);
    //    return dsreturn;
    //}

    /// <summary>
    /// wyh add 2014.04.16
    /// </summary>
    /// <param name="ds"></param>
    /// <param name="khbh"></param>
    /// <returns></returns>
    [WebMethod(Description = "适用于多银行框架--查询客户余额（银行发起）--已移植")]
    public DataSet GetUMoneyByBANK(DataSet ds,string khbh,string ssbank)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
        //根据传过来的客户编号，获取客户邮箱
        DataSet dsuserinfo = DbHelperSQL.Query("select b_dlyx from AAA_DLZHXXB where I_ZQZJZH='" + khbh + "'");
        if (dsuserinfo == null || dsuserinfo.Tables[0].Rows.Count <= 0)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "该客户不存在";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2302";
            
        }
        else if (dsuserinfo.Tables[0].Rows.Count >1)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "交易方编号重复";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2302";
        }
        else
        {
            IBank IB = new IBank(dsuserinfo.Tables[0].Rows[0]["b_dlyx"].ToString(), "券商余额查询");
            string strinfo = ssbank;
            dsreturn = IB.Invoke(ds, ref strinfo);
            if (!strinfo.Equals("ok"))
            {
                dsreturn = initReturnDataSet().Clone();
                dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = strinfo;
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";
                return dsreturn;
            }
           
        }
        return dsreturn;
    }



    /// <summary>
    /// 获取加密后的密码
    /// </summary>
    /// <param name="pin">PinKey</param>
    /// <param name="word">要加密的明文</param>
    /// <returns></returns>
    [WebMethod]
    public string GetSPassword(string pin, string word)
    {
        return PinMac.NewPassword(pin, word);        
        
    }

    //[WebMethod(Description = "银行端发起-银转商")]
    //public DataSet BankToS(DataSet dt)
    //{
    //    //初始化返回值,先塞一行数据
    //    DataSet dsreturn = initReturnDataSet().Clone();
    //    dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });


    //    //加锁
    //    if (jhjx_Lock.LockBegin_ByUser("全局锁") != "1")
    //    {
    //        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
    //        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统繁忙";
    //        return dsreturn;
    //    }
    //    //开始执行 
    //    BankRequest request = new BankRequest();
    //    dsreturn = request.BankToS(dt, dsreturn);

    //    //解锁
    //    jhjx_Lock.LockEnd_ByUser("全局锁");

    //    return dsreturn;
    //}


    /// <summary>
    /// wyh 2014.04.16 add
    /// </summary>
    /// <param name="dt"></param>
    /// <param name="khbh"></param>
    /// <returns></returns>
    [WebMethod(Description = "适用于多银行框架--银行端发起-银转商--已移植")]
    public DataSet BankToS(DataSet ds, string khbh,string ssbank)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        //根据传过来的客户编号，获取客户邮箱
        DataSet dsuserinfo = DbHelperSQL.Query("select b_dlyx from AAA_DLZHXXB where I_ZQZJZH='" + khbh + "'");
        if (dsuserinfo == null || dsuserinfo.Tables[0].Rows.Count <= 0)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "交易方编号不存在";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "2302";
            return dsreturn;
        }
        else if (dsuserinfo.Tables[0].Rows.Count >1)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "交易方编号重复";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "2302";
            return dsreturn;
        }
        else
        {

            Hashtable ht = new Hashtable();
            ht["DLYX"] = dsuserinfo.Tables[0].Rows[0]["b_dlyx"].ToString();
            Hashtable htreul = JHJX_YanZhengClass.SFTJKTSQ(ht);
            if (!htreul["info"].ToString().Equals("ok"))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = htreul["info"].ToString();
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";
                return dsreturn;
            }
            if (!htreul["是否提交开通申请"].ToString().Equals("是"))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您尚未提交开通交易账申请，请及时提交！";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2011";
                return dsreturn;
            }

            if (htreul["是否提交开通申请"].ToString().Equals("审核中"))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的开户申请正在审核中，请耐心等待！";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2011";
                return dsreturn;
            }

            if (htreul["是否提交开通申请"].ToString().Equals("驳回"))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的开户申请审核未通过，请您进入“账户维护”界面查询详情并重新提交申请！";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2011";
                return dsreturn;
            }

            if (!htreul["第三方存管状态是否开通"].ToString().Equals("是"))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您当前未绑定第三方存管银行，请到相关银行开通！";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2013";
                return dsreturn;
            }

            if (!htreul["是否交易时间内"].ToString().Equals("是"))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间，暂停交易！";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2140";
                return dsreturn;
            }
            if (!htreul["是否交易日期内"].ToString().Equals("是"))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易日期，暂停交易！";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2032";
                return dsreturn;
            }

           
            if (jhjx_Lock.LockBegin_ByUser("全局锁") != "1") //加锁
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统繁忙（锁）";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";
                return dsreturn;
            }

            IBank IB = new IBank(dsuserinfo.Tables[0].Rows[0]["b_dlyx"].ToString(), "银行端入金");
            string strinfo = ssbank;
            dsreturn = IB.Invoke(ds, ref strinfo);
            if (!strinfo.Equals("ok"))
            {
                dsreturn = initReturnDataSet().Clone();
                dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = strinfo;
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";
                return dsreturn;

            }

            //解锁
            jhjx_Lock.LockEnd_ByUser("全局锁");
        }
        return dsreturn;

  
    }

    //[WebMethod(Description = "银行端发起-商转银")] //wyh dele 2014.04.17 
    //public DataSet SToBank(DataSet dt)
    //{
    //    //初始化返回值,先塞一行数据
    //    DataSet dsreturn = initReturnDataSet().Clone();
    //    dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });


    //    //加锁
    //    if (jhjx_Lock.LockBegin_ByUser("全局锁") != "1")
    //    {
    //        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
    //        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统繁忙";
    //        return dsreturn;
    //    }
    //    //开始执行 
    //    BankRequest request = new BankRequest();
    //    dsreturn = request.SToBank(dt, dsreturn);

    //    //解锁
    //    jhjx_Lock.LockEnd_ByUser("全局锁");

    //    return dsreturn;
    //}


    /// <summary>
    /// wyh add 2014.04.17 适用于多银行框架--银行端发起-商转银
    /// </summary>
    /// <param name="ds"></param>
    /// <param name="khbh"></param>
    /// <returns></returns>
    [WebMethod(Description = "适用于多银行框架--银行端发起-商转银--已移植")]
    public DataSet SToBank(DataSet ds, string khbh,string ssbank)
    {

        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        //根据传过来的客户编号，获取客户邮箱
        DataSet dsuserinfo = DbHelperSQL.Query("select b_dlyx from AAA_DLZHXXB where I_ZQZJZH='" + khbh + "'");
        if (dsuserinfo == null || dsuserinfo.Tables[0].Rows.Count <= 0)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "交易方编号不存在";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "2302";
            return dsreturn;
        }
        else if (dsuserinfo.Tables[0].Rows.Count > 1)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "交易方编号重复";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "2302";
            return dsreturn;
        }
        else
        {
        
            Hashtable ht = new Hashtable();
            ht["DLYX"] = dsuserinfo.Tables[0].Rows[0]["b_dlyx"].ToString();
            Hashtable htreul = JHJX_YanZhengClass.SFTJKTSQ(ht);
            if (!htreul["info"].ToString().Equals("ok"))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = htreul["info"].ToString();
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";
                return dsreturn;
            }
            if (!htreul["是否提交开通申请"].ToString().Equals("是"))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您尚未提交开通交易账申请，请及时提交！";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2011";
                return dsreturn;
            }

            if (htreul["是否提交开通申请"].ToString().Equals("审核中"))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的开户申请正在审核中，请耐心等待！";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2011";
                return dsreturn;
            }

            if (htreul["是否提交开通申请"].ToString().Equals("驳回"))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的开户申请审核未通过，请您进入“账户维护”界面查询详情并重新提交申请！";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2011";
                return dsreturn;
            }

            if (!htreul["第三方存管状态是否开通"].ToString().Equals("是"))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您当前未绑定第三方存管银行，请到相关银行开通！";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2013";
                return dsreturn;
            }

            if (!htreul["是否交易时间内"].ToString().Equals("是"))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间，暂停交易！";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2140";
                return dsreturn;
            }
            if (!htreul["是否交易日期内"].ToString().Equals("是"))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易日期，暂停交易！";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2032";
                return dsreturn;
            }
            if (!htreul["当前账户是否冻结出金"].ToString().Equals("否"))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "交易方已冻结出金";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";
                return dsreturn;
            }




            if (jhjx_Lock.LockBegin_ByUser("全局锁") != "1") //加锁
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统繁忙（锁）";
                return dsreturn;
            }

            IBank IB = new IBank(dsuserinfo.Tables[0].Rows[0]["b_dlyx"].ToString(), "银行端出金");
            string strinfo = ssbank;
            dsreturn = IB.Invoke(ds, ref strinfo);
            if (!strinfo.Equals("ok"))
            {
                dsreturn = initReturnDataSet().Clone();
                dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", strinfo });
                return dsreturn;
            }

            //解锁
            jhjx_Lock.LockEnd_ByUser("全局锁");
        }
        return dsreturn;

    }

    /// <summary>
    /// 判断银行闭式时间外是否存在数据
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    [WebMethod(Description = "判断银行闭式时间外是否存在数据--已移植")]
    public DataSet dsishav(DataTable ht)
    {
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        string Strbakdnam = ht.Rows[0]["开户银行"].ToString();;
        string Strjssj = "";
        string Strsql = "select PTMRJSFWSJ from AAA_PTDTCSSDB";
        object objbs = DbHelperSQL.GetSingle(Strsql);
        if (objbs != null && !objbs.ToString().Trim().Equals(""))
        {
            Strjssj = objbs.ToString().Trim() + ":00.000";
        }
        string Sqlhav = "select COUNT(a.Number)  from AAA_ZKLSMXB  as a left join AAA_DLZHXXB as b on ( a.JSBH = b.J_BUYJSBH or a.JSBH = b.J_JJRJSBH or a.JSBH = b.J_SELJSBH )  where (a.LSCSSJ between '" + ht.Rows[0]["结算时间"].ToString().Trim() + " " + Strjssj.Trim() + "'  and  '" + ht.Rows[0]["结算时间"].ToString().Trim() + " 23:59:59.999' ) and b.I_KHYH = '" + Strbakdnam + "'";

        object obj = DbHelperSQL.GetSingle(Sqlhav);
        if (obj != null && !obj.ToString().Trim().Equals("0") && !obj.ToString().Trim().Equals(""))
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "have";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "账户流水明细表中存在 时间段：" + ht.Rows[0]["结算时间"].ToString().Trim() + " " + Strjssj.Trim() + " 至 \n\r" + ht.Rows[0]["结算时间"].ToString().Trim() + " 23:59:59.999'" + "的流水数据" + obj + "条，是否继续统计！";
            return dsreturn;
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "pass";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "不存在不合法数据，你可以正常统计！";
            return dsreturn;
        }

    }
   
    /// <summary>
    /// 存管客户交易资金净额清算文件 
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    [WebMethod(Description = "获取存管客户交易资金净额清算文件数据--已移植")]
    public DataSet Getcgkhjyzijefile(DataTable dt)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });



        //开始执行     TBD_GetCGinfo
        if (1 == 1) //添加工作日条件限制
        {
            jhjx_greatbankfile bankmethed = new jhjx_greatbankfile();
            dsreturn = bankmethed.Getcgkhjyzijefile(dsreturn, dt);

        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "不在文件上传时间段内无法上传文件！";

        }



        return dsreturn;
    }
    /// <summary>
    /// 存管客户批量利息入帐文件 
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    [WebMethod(Description = "存管客户批量利息入帐文件--已移植")]
    public DataSet Getcgkhpllx(DataTable dt)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });



        //开始执行     TBD_GetCGinfo
        if (1 == 1) //添加工作日条件限制
        {
            jhjx_greatbankfile bankmethed = new jhjx_greatbankfile();
            dsreturn = bankmethed.Getcgkhpllx(dsreturn, dt);

        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "不在文件上传时间段内无法上传文件！";

        }



        return dsreturn;
    }
    /// <summary>
    /// 存管客户资金余额明细文件 
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    [WebMethod(Description = "存管客户资金余额明细文件--已移植")]
    public DataSet Getcgkhzjmc(DataTable dt)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });



        //开始执行     TBD_GetCGinfo
        if (1 == 1) //添加工作日条件限制
        {
            jhjx_greatbankfile bankmethed = new jhjx_greatbankfile();
            dsreturn = bankmethed.Getcgkhzjmc(dsreturn, dt);

        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "不在文件上传时间段内无法上传文件！";

        }



        return dsreturn;
    }
    /// <summary>
    /// 存管汇总账户资金交收汇总文件格式 
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    [WebMethod(Description = "存管汇总账户资金交收汇总文件格式--已移植")]
    public DataSet Getcghzzhzjjs(DataTable dt)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });



        //开始执行     TBD_GetCGinfo
        if (1 == 1) //添加工作日条件限制
        {
            jhjx_greatbankfile bankmethed = new jhjx_greatbankfile();
            dsreturn = bankmethed.Getcghzzhzjjs(dsreturn, dt);

        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "不在文件上传时间段内无法上传文件！";

        }

        return dsreturn;
    }

   [WebMethod(Description = "获取此次结算文件需发送数据信息--已移植")]
    public DataSet GetStrwhere(DataTable dt)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });



        //开始执行     TBD_GetCGinfo
        if (1 == 1) //添加工作日条件限制
        {
            jhjx_greatbankfile bankmethed = new jhjx_greatbankfile();
            dsreturn = bankmethed.GetSqlwhere(dsreturn, dt);

        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "不在文件上传时间段内无法上传文件！";

        }

        return dsreturn;
    }



    /// <summary>
    /// 获取银证转账对账数据
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    [WebMethod(Description = "获取银证转账对账数据--已移植")]
    public DataSet GetYHZZXX(DataTable dt)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
     
            jhjx_greatbankfile bankmethed = new jhjx_greatbankfile();
            dsreturn = bankmethed.GetYZZZXX(dsreturn, dt);
        return dsreturn;
    }


    ///// <summary>
    ///// 银行端发起-变更银行账户(25710)  wyh dele  2014.04.17
    ///// </summary>
    ///// <param name="dt"></param>
    ///// <returns></returns>
    //[WebMethod(Description = "银行端发起-变更银行账户(25710)")]
    //public DataSet BankRequest_UpdateBankAccount(DataSet dt)
    //{
    //    //初始化返回值,先塞一行数据
    //    DataSet dsreturn = initReturnDataSet().Clone();
    //    dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });


    //    //加锁
    //    if (jhjx_Lock.LockBegin_ByUser("全局锁") != "1")
    //    {
    //        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
    //        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统繁忙";
    //        return dsreturn;
    //    }
    //    //开始执行 
    //    UpdateBankAccount update = new UpdateBankAccount();
    //    dsreturn = update.BankRequest_UpdateBankAccount(dsreturn, dt);
    //    //解锁
    //    jhjx_Lock.LockEnd_ByUser("全局锁");

    //    return dsreturn;
    //}



    /// <summary>
    /// 银行端发起-变更银行账户(25710)biangengyinhangzhannghu
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    [WebMethod(Description = "适用于多银行框架--银行端发起-变更银行账户(25710)--已移植")]
    public DataSet BankRequest_UpdateBankAccount(DataSet ds,string khbh,string ssbank)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        //根据传过来的客户编号，获取客户邮箱
        DataSet dsuserinfo = DbHelperSQL.Query("select b_dlyx from AAA_DLZHXXB where I_ZQZJZH='" + khbh + "'");
        if (dsuserinfo == null || dsuserinfo.Tables[0].Rows.Count <= 0)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "交易方编号不存在";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2302";
            return dsreturn;
        }
        else if (dsuserinfo.Tables[0].Rows.Count > 1)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "交易方编号重复";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2302";
            return dsreturn;
        }
        else
        {
            //验证是否已开通第三方结算账户
            Hashtable ht = new Hashtable();
            ht["DLYX"] = dsuserinfo.Tables[0].Rows[0]["b_dlyx"].ToString();
            Hashtable htre = JHJX_YanZhengClass.SFTJKTSQ(ht);
           


            if (!htre["第三方存管状态是否开通"].ToString().Equals("是"))
            {
               dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
               dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未绑定第三方存管银行！";
               dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2013";
               return dsreturn;
            }
          
            if (jhjx_Lock.LockBegin_ByUser("全局锁") != "1") //加锁
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统繁忙（锁）";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";
                return dsreturn;
            }

            IBank IB = new IBank(dsuserinfo.Tables[0].Rows[0]["b_dlyx"].ToString(), "银行端换卡");
            string strinfo = ssbank;
            dsreturn = IB.Invoke(ds, ref strinfo);
            if (!strinfo.Equals("ok"))
            {
                dsreturn = initReturnDataSet().Clone();
                dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", strinfo, "8888" });
                return dsreturn;
            }

            //解锁
            jhjx_Lock.LockEnd_ByUser("全局锁");
        }
        return dsreturn;
    }

    /// <summary>
    /// 券商端发起-变更银行账户(26710)
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    [WebMethod(Description = "适用于多银行框架--券商端发起-变更银行账户(26710)--已移植")]
    public DataSet SRequest_UpdateBankAccount(DataTable dt)
    {

        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        //根据传过来的客户编号，获取客户邮箱
     
       
          


            //条件验证
            Hashtable ht = new Hashtable();
            ht["DLYX"] = dt.Rows[0]["用户邮箱"].ToString();

            Hashtable htreul = JHJX_YanZhengClass.SFTJKTSQ(ht);
            if (!htreul["info"].ToString().Equals("ok"))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = htreul["info"].ToString();
                return dsreturn;
            }
            if (!htreul["是否提交开通申请"].ToString().Equals("是"))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您尚未提交开通交易账申请，请及时提交！";
                return dsreturn;
            }

            if (htreul["是否提交开通申请"].ToString().Equals("审核中"))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的开户申请正在审核中，请耐心等待！";
                return dsreturn;
            }

            if (htreul["是否提交开通申请"].ToString().Equals("驳回"))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的开户申请审核未通过，请您进入“账户维护”界面查询详情并重新提交申请！";
                return dsreturn;
            }

            if (!htreul["第三方存管状态是否开通"].ToString().Equals("是"))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您当前未绑定第三方存管银行，请到相关银行开通！";
                return dsreturn;
            }

            if (!htreul["是否交易时间内"].ToString().Equals("是"))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间，暂停交易！";
                return dsreturn;
            }
            if (!htreul["是否交易日期内"].ToString().Equals("是"))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易日期，暂停交易！";
                return dsreturn;
            }

            if (jhjx_Lock.LockBegin_ByUser("全局锁") != "1") //加锁
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统繁忙（锁）";
                return dsreturn;
            }



            IBank IB = new IBank(dt.Rows[0]["用户邮箱"].ToString(), "变更帐户");
            if (jhjx_Lock.LockBegin_ByUser("全局锁") != "1") //加锁
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统繁忙（锁）";
                return dsreturn;
            }

            DataSet dspars = new DataSet();
            dspars.Tables.Add(dt);
            DataSet dsresult = IB.Invoke(dspars);
            if (dsresult != null && dsresult.Tables[0].Rows.Count > 0)
            {
                dsreturn = dsresult;
            }
           
            //解锁
            jhjx_Lock.LockEnd_ByUser("全局锁");
        
        return dsreturn;

    }
    /// <summary>
    /// 客户变更存管银行（26713）
    /// </summary>
    [WebMethod(Description = "券商端发起-客户变更存管银行（26713）")]
    public DataSet SRequest_UpdateCGBank(DataTable dt)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });


        //加锁
        if (jhjx_Lock.LockBegin_ByUser("全局锁") != "1")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统繁忙";
            return dsreturn;
        }
        //开始执行 
        UpdateBankAccount update = new UpdateBankAccount();
        dsreturn = update.SRequest_UpdateCGBank(dsreturn, dt);
        //解锁
        jhjx_Lock.LockEnd_ByUser("全局锁");

        return dsreturn;
    }

    [WebMethod]
    public DataSet GetNewKey()
    {
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
        //获取最新Key 开始
        DataTable dt = BankServicePF.N26700_SecretKey_Init();//初始化的数据集
        if (dt != null && dt.Rows.Count > 0 && dt.Rows[0]["FunctionId"].ToString().Trim() == "26700")
        {
            try
            {
                dt.Rows[0]["ExSerial"] = DateTime.Now.ToString("yyyyMMddHHmmss");
                dt.Rows[0]["Date"] = DateTime.Now.ToString("yyyyMMdd");
                dt.Rows[0]["Time"] = "";
                DataSet ds = BankServicePF.N26700_SecretKey_Push(dt);
                if (ds.Tables["str"].Rows[0]["状态信息"].ToString().Trim() == "ok")
                {
                    //说明发送、接收都成功
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取成功";
                    dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = ds.Tables["result"].Rows[0]["ExSerial"];//流水号
                    dsreturn.Tables["返回值单条"].Rows[0]["附件信息2"] = ds.Tables["result"].Rows[0]["PinKey"];//Pinkey
                    dsreturn.Tables["返回值单条"].Rows[0]["附件信息3"] = ds.Tables["result"].Rows[0]["MacKey"];//Mackey
                }
                else
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = ds.Tables["str"].Rows[0]["状态信息"];
                }
            }
            catch (Exception ex)
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = ex.Message;
            }
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "接口繁忙";
        }
        //获取最新Key 结束
        return dsreturn;
    }

    /// <summary>
    /// 返回开票信息的数据内容
    /// </summary>
    /// <param name="dlyx">登陆邮箱</param> 
    /// <returns>返回获取的开票信息</returns>
    [WebMethod(Description = "返回获取的开票信息")]
    public DataSet GetKPXX(string dlyx)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

 
        //开始执行     
        GetKPXX thd = new GetKPXX();
        Hashtable  htres = thd.GetKPXXinfo(dlyx);
        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = htres["执行结果"].ToString();
        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = htres["提示文字"].ToString();
        DataTable dtinfo = (DataTable)htres["开票信息"];
        dtinfo.TableName = "开票信息";
        dsreturn.Tables.Add(dtinfo.Copy());
        DataTable dtzcxx = (DataTable)htres["注册信息"];
        dtzcxx.TableName = "注册信息";
        dsreturn.Tables.Add(dtzcxx.Copy());
 
        return dsreturn;
    }

    /// <summary>
    /// 提交开票信息
    /// </summary>
    /// <param name="dlyx">登陆邮箱</param> 
    /// <returns>返回提交开票信息的结果</returns>
    [WebMethod(Description = "返回提交开票信息的结果")]
    public DataSet CommitKPXX(DataTable dt)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

 
        //开始执行     
        GetKPXX thd = new GetKPXX();
        Hashtable htres = thd.CommitKPXX(dt);
        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = htres["执行结果"].ToString();
        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = htres["提示文字"].ToString();
        
 
        return dsreturn;
    }

    /// <summary>
    /// 变更开票信息
    /// </summary>
    /// <param name="dt">参数列表</param> 
    /// <returns>返回提交开票信息变更数据的执行结果</returns>
    [WebMethod(Description = "返回提交开票信息变更数据的执行结果")]
    public DataSet CommitKPXXChange(DataTable dt)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

 
        //开始执行     
        GetKPXX thd = new GetKPXX();
        Hashtable htres = thd.CommitKPXXChange(dt);
        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = htres["执行结果"].ToString();
        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = htres["提示文字"].ToString();

 
        return dsreturn;
    }
    /// <summary>
    /// 执行每日清算
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    [WebMethod(Description = "对每天银行闭市后的数据进行清算--已移植")]
    public DataSet WCQS(DataTable dt)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        jhjx_greatbankfile bankmethed = new jhjx_greatbankfile();
        dsreturn = bankmethed.WCQS(dsreturn, dt);
        return dsreturn;
    }
    [WebMethod(Description="申请出售商品")]
    public DataSet SQCSSP(DataSet ds)
    {
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
        TBD t = new TBD();
        dsreturn = t.TBD_SQCSSP(ds, dsreturn);
        return dsreturn;
    }
     [WebMethod(Description = "判断当前用户是否签订承诺书")]
    public DataSet JudgeSFQDCNS(DataTable dt)
    {
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
        KTJYZHClass ktjyzh = new KTJYZHClass();
        dsreturn = ktjyzh.JudgeSFQDCNS(dsreturn, dt);
        return dsreturn;
    } 


  [WebMethod(Description = "交易账户签订承诺书")]
    public DataSet JYZHQDCNS(DataTable dt)
    {
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
        KTJYZHClass ktjyzh = new KTJYZHClass();
        dsreturn = ktjyzh.JYZHQDCNS(dsreturn, dt);
        return dsreturn;
    }
     [WebMethod(Description = "查询当前用户是否有东西能卖")]
  public DataSet isUserCanSellSth(string dlyx)
  {
      DataSet dsreturn = initReturnDataSet().Clone();
      dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
      try
      {
          TBD t = new TBD();
          bool b = t.TBD_IsHasSthToSell(dlyx);
          dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
          dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "查询成功";
          dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = b;
      }
      catch (Exception ex)
      {
          dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
          dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = ex.Message;
      }
      return dsreturn;
  }
     /// <summary>
     /// 查看出售商品基本信息
     /// </summary>
     /// <param name="dt">参数列表</param> 
     /// <returns> </returns>
     [WebMethod(Description = "查看出售商品基本信息")]
     public DataSet Select_CSSP(DataTable dt)
     {
         //初始化返回值,先塞一行数据
         DataSet dsreturn = initReturnDataSet().Clone();
         dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

 
         //开始执行     
         CKCSSP sp = new CKCSSP();
         dsreturn = sp.Select_CSSP(dt, dsreturn);

         return dsreturn;
     }
     
     /// <summary>
     /// 获取用户功能冻结信息
     /// </summary>
     /// <param name="YHinfo">用户登录邮箱或者用户角色编号</param>
     /// <param name="DJgn">需验证功能</param>
     /// <returns>返回冻结信息</returns>
     [WebMethod(Description = "获取用户功能冻结信息")]
     public DataSet getYHdjinfo(string YHinfo, string DJgn)
     {
         //DataSet dsreturn = initReturnDataSet().Clone();
         DataSet dsreturn = null;
         dsreturn = jhjx_yhdongjie.getYHdjinfo(YHinfo, DJgn);
         return dsreturn;
     }



     [WebMethod(Description = "得到投标单的信息")]
     public DataSet Get_TBDData(DataTable dt)
     {
         DataSet dsreturn = initReturnDataSet().Clone();
         dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
         KTJYZHClass ktjyzh = new KTJYZHClass();
         dsreturn = ktjyzh.Get_TBDData(dsreturn, dt);
         return dsreturn;
     }


     [WebMethod(Description = "得到预订单的数据信息")]
     public DataSet Get_YDDData(DataTable dt)
     {
         DataSet dsreturn = initReturnDataSet().Clone();
         dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
         KTJYZHClass ktjyzh = new KTJYZHClass();
         dsreturn = ktjyzh.Get_YDDData(dsreturn, dt);
         return dsreturn;
     }

     ///// <summary>
     ///// 监控，冲证券转银行23704
     ///// </summary>
     ///// <param name="dt">参数列表</param> 
     ///// <returns>监控，冲证券转银行23704</returns>
     //[WebMethod(Description = "冲证券转银行23704")]
     //public DataSet CSToBank(DataSet dt)
     //{
     //    //初始化返回值,先塞一行数据
     //    DataSet dsreturn = initReturnDataSet().Clone();
     //    dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

     //    //加锁
     //    if (jhjx_Lock.LockBegin_ByUser("全局锁") != "1")
     //    {

     //        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
     //        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统繁忙";
     //        return dsreturn;
     //    }
     //    //开始执行  
     //    BankRequest bank = new BankRequest();
     //    dsreturn = bank.CSToBank(dt, dsreturn);

     //    //解锁
     //    jhjx_Lock.LockEnd_ByUser("全局锁");
     //    return dsreturn;
     //}


     /// <summary>
     /// 监控，冲证券转银行23704,wyh add
     /// </summary>
     /// <param name="dt">参数列表</param> 
     /// <returns>监控，冲证券转银行23704</returns>

     [WebMethod(Description = "适用于多银行框架--冲证券转银行23704--已移植")]
     public DataSet CSToBank(DataSet ds, string khbh,string ssbank)
     {

         //初始化返回值,先塞一行数据
         DataSet dsreturn = initReturnDataSet().Clone();
         dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

         //根据传过来的客户编号，获取客户邮箱
         DataSet dsuserinfo = DbHelperSQL.Query("select b_dlyx from AAA_DLZHXXB where I_ZQZJZH='" + khbh + "'");
         if (dsuserinfo == null || dsuserinfo.Tables[0].Rows.Count <= 0)
         {
             dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
             dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "该客户不存在";
             dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";
             return dsreturn;

         }
         else if (dsuserinfo.Tables[0].Rows.Count > 1)
         {
             dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
             dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "客户编号重复";
             dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";
             return dsreturn;
         }
         else
         {
             //验证是否已开通第三方结算账户
             Hashtable ht = new Hashtable();
             ht["DLYX"] = dsuserinfo.Tables[0].Rows[0]["b_dlyx"].ToString();
             Hashtable htre = JHJX_YanZhengClass.SFTJKTSQ(ht);

             if (!htre["info"].ToString().Equals("ok"))
             {
                 dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                 dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = htre["info"].ToString();
                 dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";
                 return dsreturn;
             }

             if (!htre["第三方存管状态是否开通"].ToString().Equals("是"))
             {
                  dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                  dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未绑定第三方存管银行！";
                  dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2013";
                  return dsreturn;
             }

             if (!htre["是否交易时间内"].ToString().Equals("是"))
             {
                 dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                 dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间";
                 dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2140";
                 return dsreturn;
             }

             if (!htre["是否交易日期内"].ToString().Equals("是"))
             {
                 dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                 dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统交易日期不符";
                 dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2302";
                 return dsreturn;
             }
              

             
             if (jhjx_Lock.LockBegin_ByUser("全局锁") != "1") //加锁
             {
                 dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                 dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统繁忙（锁）";
                 return dsreturn;
             }

             IBank IB = new IBank(dsuserinfo.Tables[0].Rows[0]["b_dlyx"].ToString(), "冲银行端出金");
             string strinfo = ssbank;
             dsreturn = IB.Invoke(ds, ref strinfo);
             if (!strinfo.Equals("ok"))
             {
                 dsreturn = initReturnDataSet().Clone();
                 dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", strinfo });
                 return dsreturn;
             }

             //解锁
             jhjx_Lock.LockEnd_ByUser("全局锁");
         }

         return dsreturn;

     }


     ///// <summary>
     ///// 监控，冲银行转证券23703
     ///// </summary>
     ///// <param name="dt">参数列表</param> 
     ///// <returns> </returns>
     //[WebMethod(Description = "冲银行转证券23703")]
     //public DataSet CBankToS(DataSet dt)
     //{
     //    //初始化返回值,先塞一行数据
     //    DataSet dsreturn = initReturnDataSet().Clone();
     //    dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

     //    //加锁
     //    if (jhjx_Lock.LockBegin_ByUser("全局锁") != "1")
     //    {

     //        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
     //        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统繁忙";
     //        return dsreturn;
     //    }
     //    //开始执行  
     //    BankRequest bank = new BankRequest();
     //    dsreturn = bank.CBankToS(dt, dsreturn);

     //    //解锁
     //    jhjx_Lock.LockEnd_ByUser("全局锁");
     //    return dsreturn;
     //}

     
     /// <summary>
     /// 监控，冲银行转证券23703  银行端发起
     /// </summary>
     /// <param name="dt">参数列表</param> 
     /// <returns> </returns>
     [WebMethod(Description = "适用于多银行框架--冲银行转证券23703--已移植")]
     public DataSet CBankToS(DataSet ds,string khbh,string ssbank)
     {

         //初始化返回值,先塞一行数据
         DataSet dsreturn = initReturnDataSet().Clone();
         dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

         //根据传过来的客户编号，获取客户邮箱
         DataSet dsuserinfo = DbHelperSQL.Query("select b_dlyx from AAA_DLZHXXB where I_ZQZJZH='" + khbh + "'");
         if (dsuserinfo == null || dsuserinfo.Tables[0].Rows.Count <= 0)
         {
             dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
             dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "交易方编号不存在";
             dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2302";
         }
         else if (dsuserinfo.Tables[0].Rows.Count > 1)
         {
             dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
             dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "交易方编号重复";
             dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2302";
         }
         else
         {

             //验证是否已开通第三方结算账户
             Hashtable ht = new Hashtable();
             ht["DLYX"] = dsuserinfo.Tables[0].Rows[0]["b_dlyx"].ToString();
             Hashtable htre = JHJX_YanZhengClass.SFTJKTSQ(ht);

             if (!htre["info"].ToString().Equals("ok"))
             {
                 dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                 dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = htre["info"].ToString();
                 dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";
                 return dsreturn;
             }

             if (!htre["第三方存管状态是否开通"].ToString().Equals("是"))
             {
                 dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                 dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未绑定第三方存管银行！";
                 dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2013";
                 return dsreturn;
             }

             if (!htre["是否交易时间内"].ToString().Equals("是"))
             {
                 dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                 dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间";
                 dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2140";
                 return dsreturn;
             }

             if (!htre["是否交易日期内"].ToString().Equals("是"))
             {
                 dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                 dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统交易日期不符";
                 dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2302";
                 return dsreturn;
             }


             if (jhjx_Lock.LockBegin_ByUser("全局锁") != "1") //加锁
             {
                 dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                 dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统繁忙（锁）";
                 dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";
                 return dsreturn;
             }

             IBank IB = new IBank(dsuserinfo.Tables[0].Rows[0]["b_dlyx"].ToString(), "冲银行端入金");
             string strinfo = ssbank;
             dsreturn = IB.Invoke(ds, ref strinfo);
             if (!strinfo.Equals("ok"))
             {
                 dsreturn = initReturnDataSet().Clone();
                 dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
                 dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                 dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = strinfo;
                 dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";
                 return dsreturn;
             }

             //解锁
             jhjx_Lock.LockEnd_ByUser("全局锁");
         }

         return dsreturn;

        
     }

     
     /// <summary>
     /// 异常投标单详情
     /// </summary>
     /// <param name="dt">参数列表</param> 
     /// <returns> </returns>
     [WebMethod(Description = "异常投标单详情")]
     public DataSet CXTBDYCZZ(DataTable dt)
     {
         //初始化返回值,先塞一行数据
         DataSet dsreturn = initReturnDataSet().Clone();
         dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

 
         //开始执行  
         YCTBD bank = new YCTBD();
         dsreturn = bank.CXTBDYCZZ(dt, dsreturn);

    
         return dsreturn;
     }
     /// <summary>
     /// 异常投标单修改
     /// </summary>
     /// <param name="dt">参数列表</param> 
     /// <returns>返回修改成功或失败</returns>
     [WebMethod(Description = "异常投标单修改")]
     public DataSet UpdateTBDYCZZ(DataSet dt)
     {
         //初始化返回值,先塞一行数据
         DataSet dsreturn = initReturnDataSet().Clone();
         dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

 
         //开始执行  
         YCTBD bank = new YCTBD();
         dsreturn = bank.Update(dt, dsreturn);

 
         return dsreturn;
     }
     /// <summary>
     /// 大盘查看商品详情
     /// </summary>
     /// <param name="dt">参数列表</param> 
     /// <returns>返回商品的历史价格数据表、商品详情数据表</returns>
     [WebMethod(Description = "大盘查看商品详情")]
     public byte[] SelectSPXQ(DataTable dt)
     {
         //初始化返回值,先塞一行数据
         DataSet dsreturn = initReturnDataSet().Clone();
         dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

 
         //开始执行  
         DPSPXQ bank = new DPSPXQ();
         dsreturn = bank.SelectSPXQ(dt, dsreturn);
 
         return Helper.DataSet2Byte(dsreturn);
     }


     /// <summary>
     /// SqlServer作业测试项目
     /// </summary>
     /// <param name="dt">参数列表</param> 
     /// <returns>要处理的数值</returns>
     [WebMethod(Description = "大盘查看商品详情")]
     public string DealVithSqlServer(int i)
     {

         return "23423423432";
                    
     }

     [WebMethod]
     public DataSet QueryFor(string sql)
     {
         return DbHelperSQL.Query(sql);
     }

     /// <summary>
     /// 提交银行员工信息
     /// </summary>
     /// <param name="dt">参数列表</param> 
     /// <returns> </returns>
     [WebMethod(Description = "提交银行员工信息")]
     public DataSet SubmitYHUserInfor(DataTable dt)
     {
         //初始化返回值,先塞一行数据
         DataSet dsreturn = initReturnDataSet().Clone();
         dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });


         //开始执行  
         KTJYZHClass bank = new KTJYZHClass();
         dsreturn = bank.SubmitYHYHInfor( dsreturn,dt);


         return dsreturn;
     }

     /// <summary>
     /// 修改银行员工信息 DeleteYHYHInfor
     /// </summary>
     /// <param name="dt">参数列表</param> 
     /// <returns> </returns>
     [WebMethod(Description = "修改银行员工信息")]
     public DataSet ModifyYHYHInfor(DataTable dt)
     {
         //初始化返回值,先塞一行数据
         DataSet dsreturn = initReturnDataSet().Clone();
         dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });


         //开始执行  
         KTJYZHClass bank = new KTJYZHClass();
         dsreturn = bank.ModifyYHYHInfor(dsreturn, dt);


         return dsreturn;
     }


     /// <summary>
     /// 删除银行员工信息 
     /// </summary>
     /// <param name="dt">参数列表</param> 
     /// <returns> </returns>
     [WebMethod(Description = "删除银行员工信息")]
     public DataSet DeleteYHYHInfor(DataTable dt)
     {
         //初始化返回值,先塞一行数据
         DataSet dsreturn = initReturnDataSet().Clone();
         dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });


         //开始执行  
         KTJYZHClass bank = new KTJYZHClass();
         dsreturn = bank.DeleteYHYHInfor(dsreturn, dt);


         return dsreturn;
     }

     [WebMethod(Description = "获取商品分类列表")]
     public DataSet GetSPFL()
     {
         //初始化返回值,先塞一行数据
         DataSet dsreturn = initReturnDataSet().Clone();
         dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });


         //开始执行          
         dsreturn = DbHelperSQL.Query("select SortID,SortName,SortParentID from AAA_tbMenuSPFL order by SortOrder");


         return dsreturn;
     }
     [WebMethod(Description = "经纪人收益-收益详情 根据经纪人角色编号及员工工号获取不同交易方编号的收益累计")]
     public DataSet GetSYLJ(DataTable table)
     {
         //初始化返回值,先塞一行数据
         DataSet dsreturn = initReturnDataSet().Clone();
         dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });


         //开始执行     
         KTJYZHClass ktjyzh = new KTJYZHClass();
         dsreturn = ktjyzh.GetSYLJ(dsreturn,table);
         
         return dsreturn;
     }

    [WebMethod(Description = "客户预制定签约--已移植")]
     public DataSet DsSendQianYue(DataSet ds)
     {
         DataSet dsreturn = initReturnDataSet();
         dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
        
         DataRow Drow = ds.Tables[0].Rows[0];


         Hashtable ht = new Hashtable();
         ht["DLYX"] = Drow["用户邮箱"].ToString();
         Hashtable htre= JHJX_YanZhengClass.SFTJKTSQ(ht);
         if (htre["info"].ToString().Equals("ok"))
         {
             if (!htre["第三方存管状态是否开通"].ToString().Equals("否"))
             {
                 dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                 dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "已绑定第三方存管银行！";
                 return dsreturn;
             }
         }
         else
         {
             dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
             dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = htre["info"].ToString();
             return dsreturn;
         }

         IBank bank = new IBank(Drow["用户邮箱"].ToString(), "签约预指定");
         string info = string.Empty;
         DataSet dsResult = bank.Invoke(ds, ref info);
         if (info.Equals("ok"))
         {
             dsreturn = dsResult;
         }
         else
         {
             dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
             dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = info;
         }

         return dsreturn;
     }

    [WebMethod(Description = "平安银行--签到、签退--已移植")]
    public DataSet PingAn_Sign(string type)
    {
        DataSet dsreturn = initReturnDataSet();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
        BankPingan bkp = new BankPingan();
        try
        {
            dsreturn = bkp.Sign(type, dsreturn);
        }
        catch (Exception ex)
        {
            dsreturn.Tables["返回值单条"].Rows [0]["执行结果"]="err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "调用sign方法失败！" + ex.ToString();
        }
        return dsreturn;
    }

    /// <summary>
    /// 平安银行-通过子账户账号获取交易网会员代码
    /// </summary>
    /// <param name="CustAcctId">子账户账号</param>
    /// <returns></returns>
    [WebMethod(Description = "平安银行-通过子账户账号获取交易网会员代码（即客户编号）--已移植")]
    public DataSet PingAn_GetCustId(string CustAcctId)
    {
        DataSet dsreturn = initReturnDataSet();
       dsreturn .Tables ["返回值单条"].Rows .Add (new object []{"err","初始化"});
       DataSet dsinfo = DbHelperSQL.Query("select * from AAA_PingAnBank where CustAcctId='" + CustAcctId + "'");
        if (dsinfo != null && dsinfo.Tables[0].Rows.Count > 0)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "成功";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = dsinfo.Tables[0].Rows[0]["ThirdCustId"].ToString();
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "子账户不存在";
        }
        return dsreturn; 
    }
    [WebMethod(Description = "平安银行--开销户流水匹配--已移植")]
    public DataSet PingAn_AcctAndMnyMatch(DataTable dtParams)
    {
        DataSet dsreturn = initReturnDataSet();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
        BankPingan bkp = new BankPingan();
        try
        {
            dsreturn = bkp.AcctAndMnyMatch(dtParams, dsreturn);
        }
        catch (Exception ex)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "调用AccountMatch方法失败！" + ex.ToString();
        }
        return dsreturn;
    }
    [WebMethod(Description = "平安银行--对账数据处理--已移植")]
    public DataSet PingAn_DataMatch(DataTable dtParams, string FuncId, string filename)
    {
        DataSet dsreturn = initReturnDataSet();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
        BankPingan bkp = new BankPingan();
        try
        {
            dsreturn = bkp.DataProcess(dtParams, FuncId, filename, dsreturn);
        }
        catch (Exception ex)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "调用DataMatch方法失败！" + ex.ToString();
        }
        return dsreturn;
    }
    /// <summary>
    /// 可以提前获取的一些数据或参数
    /// </summary>
    /// <returns>返回数据集</returns>
    [WebMethod(Description = "平安银行--获取清算数据--已移植")]
    public DataSet GetPingAnQSDsData()
    {
         BankPingan BankPinganQS =new BankPingan();
         return BankPinganQS.GetQSDataSource();
    }
    /// <summary>
    /// 发起清算请求
    /// </summary>
    /// <returns>返回数据集</returns>
    [WebMethod(Description = "平安银行--发起清算请求--已移植")]
    public DataSet PingAn_RunQingSuan(DataTable dsParams)
    {
        DataSet dsreturn = initReturnDataSet();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        BankPingan bkp = new BankPingan();
        try
        {
            //1004接口查询银行处理进度
            dsreturn = bkp.RunQingSuan(dsParams, dsreturn);
        }
        catch (Exception ex)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "调用RunQingSuan方法失败！" + ex.ToString();
        }
        return dsreturn;
    }

    /// <summary>
    /// 获取银行清算进度状态
    /// </summary>
    /// <returns>返回数据集</returns>
    [WebMethod(Description = "平安银行--查询银行清算进度--已移植")]
    public DataSet PingAn_BankStateQuery(string type, string date, string num)
    {
        DataSet dsreturn = initReturnDataSet();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        BankPingan bkp = new BankPingan();
        try
        {
            //1004接口查询银行处理进度
            dsreturn = bkp.BankStateQuery(type, date, dsreturn);
        }
        catch (Exception ex)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "调用QueryBankState方法失败！" + ex.ToString();
        }
        return dsreturn;
    }




    /// <summary>
    /// 加入或删除自选商品
    /// </summary>
    /// <param name="DLYX">登陆邮箱</param>
    /// <returns></returns>
    [WebMethod]
    public string ZXSPedit(string DLYX, string SPBH, string edit)
    {
  
        //执行语句    
        HomePage HP = new HomePage();
        string re = HP.ZXSPedit_Run(DLYX, SPBH, edit);
        return re;
    }


    /// <summary>
    /// 获得大盘底部统计分析，直接从压缩文件中读取
    /// </summary>
    /// <returns></returns>
    [WebMethod]
    public string[][] indexTJSJ()
    {
        string Lujing = LuJing();
        HomePage HP = new HomePage();
        return HP.indexTJSJ_Run(Lujing);
    }
    private string LuJing()
    {
        return Context.Request.MapPath("../bytefiles/HomePageByte_tongji.txt");
    }


    /// <summary>
    /// 获得一个数据集，首页列表显示
    /// </summary>
    /// <returns></returns>
    [WebMethod]
    public byte[] test_ds(string sp)
    {
        HomePage HP = new HomePage();
        string lujing = Context.Request.MapPath("../bytefiles/HomePageSQLlist.txt");
        return Helper.DataSet2Byte(HP.test_ds_Run(sp, lujing));
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
    public DataSet GetPagerDB(string GetCustomersDataPage_NAME, string this_dblink, string page_index, string page_size, string serach_Row_str, string search_tbname, string search_mainid, string search_str_where, string search_paixu, string search_paixuZD, string count_float, string count_zd, string cmd_descript1, string method_retreatment)
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




}
