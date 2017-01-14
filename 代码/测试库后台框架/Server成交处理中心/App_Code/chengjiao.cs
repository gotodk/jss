using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using FMDBHelperClass;
using FMipcClass;
using System.Collections;
using System.Data;

[WebService(Namespace = "http://chengjiao.ipc.com/", Description = "V1.00->成交履约中心业务接口")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]

public class chengjiao : System.Web.Services.WebService
{
    public chengjiao () {

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
        ArrayList list = new ArrayList();
        list.Add("update AAA_DLZHXXB set I_JYFMC='1' where B_DLYX='jjrzhouli@qq.com'");
        list.Add("select top 1 * from1 AAA_DLZHXXB");
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable ht = I_DBL.RunParam_SQL(list);

        return "ok";
    }


    /// <summary>
    /// 定标 zhaoxh 2014.5.22
    /// </summary>
    /// <param name="DataTable">dt: 登陆邮箱/结算账户类型/卖家角色编号/投标单号/Number/合同期限</param>
    /// <returns>成功/失败信息/null:报错</returns>
    [WebMethod(MessageName = "定标", Description = "定标")]
    public DataSet Jhjx_DB_CJCLZX(DataTable dt)
    {
        DataSet rtds = new DataSet();
        try
        {
            rtds = ChengjiaoService.Jhjx_DB(dt);
        }
        catch (Exception ex)
        {
            //插入日志：定标接口程序错误
            rtds = null;
        }
        return rtds;
    }
    /// <summary>
    /// 处理成交规则业务逻辑 zhaoxh 2014.5.27
    /// 查看是否在冷静期，查看是否能够凑齐，根据调用点进行不同的处理
    /// 传入的数据表，必须包含至少两列“商品编号”“合同期限”，且一定不能有重复数据
    /// 调用点包括： 提交、删除、修改、中标监控
    /// </summary>
    /// <param name="dtRun">包含至少两列“商品编号”“合同期限”的DataTable</param>
    /// <param name="sp">调用点： 提交、删除、修改、中标监控</param>
    /// <returns>数据结果集/null：报错</returns>
    [WebMethod(MessageName = "成交处理", Description = "处理成交规则业务逻辑")]
    public DataSet RunCJGZ_CJCLZX(DataTable dtRun, string sp)
    {
        DataSet rtds = new DataSet();
        try
        {
            rtds = ChengjiaoService.RunCJGZ(dtRun, sp);
        }
        catch (Exception)
        {
            rtds = null;
        }
        return rtds;
    }

    /// <summary>
    /// 执行中标监控
    /// </summary>
    /// <param name="cs">此参数无效，只是为了满足格式要求</param>
    /// <returns></returns>
    [WebMethod(MessageName = "中标监控(三个月一年)", Description = "交易数据监控-中标监控(三个月一年)")]
    public DataSet serverZBJK_CJCLZX(string cs)
    {
        try
        {
            return ChengjiaoService.serverZBJK();
        }
        catch(Exception)
        {
            return null;
        }
    }
    /// <summary>
    /// 执行即时交易监控
    /// </summary>
    /// <param name="cs">此参数无效，只是为了满足格式要求</param>
    /// <returns></returns>
    [WebMethod(MessageName = "即时交易监控", Description = "交易数据监控-即时交易监控")]
    public DataSet serverZBJK_JSJY_CJCLZX(string cs)
    {
        try
        {
            return ChengjiaoService.serverZBJK_JSJY();
        }
        catch(Exception)
        {
            return null;
        }
    }
    /// <summary>
    /// 为每组数据处理大盘统计
    /// </summary>
    /// <param name="cs">此参数无效，只是为了满足格式要求</param>
    /// <returns></returns>
    [WebMethod(MessageName = "重新生成大盘监控", Description = "交易数据监控-重新生成大盘监控")]
    public DataSet CreatNewDapanList_Group_CJCLZX(string cs)
    {
        try
        {
            //得到大盘sql读取路径
            string SQLdapan_lujing = Context.Request.MapPath(@"~/bytefiles/HomePageSQLlist.txt");
            //得到底部统计sql读取路径
            string SQLdibu_lujing = Context.Request.MapPath(@"~/bytefiles/HomePageSQL.txt");
            return ChengjiaoService.CreatNewDapanList_Group_Run(SQLdapan_lujing, SQLdibu_lujing);
        }
        catch(Exception)
        {
            return null;
        }
    }

    /// <summary>
    /// 预订单过期监控
    /// </summary>
    /// <param name="cs">此参数无效，只是为了满足格式要求</param>
    /// <returns></returns>
    [WebMethod(MessageName = "预订单过期", Description = "交易数据监控-预订单过期")]
    public DataSet serverGQJK_CJCLZX(string cs)
    {
        try
        {
            return ChengjiaoService.serverGQJK();
        }
        catch (Exception)
        {
            return null;
        }
    }

    /// <summary>
    /// 执行发货延迟监控
    /// </summary>
    /// <param name="cs">此参数无效，只是为了满足格式要求</param>
    /// <returns></returns>
    [WebMethod(MessageName = "延迟发货监控", Description = "交易数据监控-延迟发货监控")]
    public DataSet serverRunFHYC_CJCLZX(string cs)
    {        
        try
        {
            //执行语句      
            return ChengjiaoService.RunFHYC();
        }
        catch (Exception)
        {            
            return null;
        }
    }

    /// <summary>
    /// 执行发票延迟监控
    /// </summary>
    /// <param name="cs">此参数无效，只是为了满足格式要求</param>
    /// <returns></returns>
    [WebMethod(MessageName = "延迟发票监控", Description = "交易数据监控-延迟发票监控")]
    public DataSet serverRunFPYC_CJCLZX(string cs)
    {        
        try
        {
            return ChengjiaoService.RunFPYC();
        }
        catch (Exception ex)
        {
            return null;
        }
    }
    /// <summary>
    /// 发货延迟监控(仅用于即时的)
    /// </summary>
    /// <param name="cs">此参数无效，只是为了满足格式要求</param>
    /// <returns></returns>
    [WebMethod(MessageName = "即时延迟发货", Description = "交易数据监控-即时延迟发货")]
    public DataSet serverRunFHYC_onlyJSJY_CJCLZX(string cs)
    {
        try
        {
            //执行语句      
            return ChengjiaoService.RunFHYC_onlyJSJY();
        }
        catch (Exception ex)
        {
            return null;
        }
    }
    /// <summary>
    /// 计算履约保证金不足的合同
    /// </summary>
    /// <param name="cs">此参数无效，只是为了满足格式要求</param>
    /// <returns></returns>
    [WebMethod(MessageName = "计算履约保证金不足的合同", Description = "交易数据监控-计算履约保证金不足的合同")]
    public DataSet serverRunLYBZJbuzu_CJCLZX(string cs)
    {
        try
        {
            return ChengjiaoService.RunLYBZJbuzu();
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    /// <summary>
    /// 提醒后自动签收
    /// </summary>
    /// <param name="cs">此参数无效，只是为了满足格式要求</param>
    /// <returns></returns>
    [WebMethod(MessageName = "提醒后自动签收", Description = "交易数据监控-提醒后自动签收")]
    public DataSet serverRunZDQS_CJCLZX(string cs)
    {        
        try
        {
            //执行语句 
            return ChengjiaoService.MRWYYQS();
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    /// <summary>
    /// 执行每月经纪人扣税
    /// </summary>
    /// <param name="cs">此参数无效，只是为了满足格式要求</param>
    /// <returns></returns>
    [WebMethod(MessageName = "计算经纪人收益扣税", Description = "交易数据监控-计算经纪人收益扣税")]
    public DataSet ServerMYJJRKS_CJCLZX(string cs)
    {
        try
        {
            return ChengjiaoService.ServerMYJJRKS();
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    /// <summary>
    /// 执行合同到期处理监控
    /// </summary>
    /// <param name="cs">此参数无效，只是为了满足格式要求</param>
    /// <returns></returns>
    [WebMethod(MessageName = "执行合同到期处理监控", Description = "交易数据监控-执行合同到期处理监控")]
    public DataSet ServerHTQMJK_CJCLZX(string cs)
    {
        try
        {
            return ChengjiaoService.ServerHTQMJK();
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    /// <summary>
    /// 执行超期未缴纳履约保证金监控
    /// </summary>
    /// <param name="cs">此参数无效，只是为了满足格式要求</param>
    /// <returns></returns>
    [WebMethod(MessageName = "执行超期未缴纳履约保证金监控", Description = "交易数据监控-执行超期未缴纳履约保证金监控")]
    public DataSet serverWJNLYBZJJK_CJCLZX(string cs)
    {
        try
        {
            return ChengjiaoService.ServerCQWJNLYBZJJK();
        }
        catch (Exception ex)
        {
            return null;
        }
    }


    /// <summary>
    /// 监控正常 wyh 2014.07.05
    /// </summary>
    /// <param name="dataTable"></param>
    /// <returns></returns>
     [WebMethod(MessageName = "监控正常", Description = "交易数据监控-监控正常")]
    public DataSet SupervisoryControlNormal_CJCLZX(DataTable dataTable)
    {
        try
        {
            return ChengjiaoService.SupervisoryControlNormal(dataTable);
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    /// <summary>
    /// 监控异常 wyh 2014.07.05
    /// </summary>
    /// <returns></returns>
     [WebMethod(MessageName = "监控异常", Description = "交易数据监控-监控异常")]
     public DataSet SupervisoryControlAbnormal_CJCLZX(DataTable dataTable)
     {
         try
         {
             return ChengjiaoService.SupervisoryControlAbnormal(dataTable);
         }
         catch (Exception ex)
         {
             return null;
         }
     }
}