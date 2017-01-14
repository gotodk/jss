using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web;
using System.Web.Services;

[WebService(Namespace = "http://www.fm8844.com")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]

public class Service : System.Web.Services.WebService
{
    /// <summary>
    /// Socket目标IP
    /// </summary>
    private string ipAddress = "10.112.26.73";

    /// <summary>
    /// Socket目标端口
    /// </summary>
    private string port = "40012";

    public Service () {

        //如果使用设计的组件，请取消注释以下行
        //InitializeComponent(); 
        ipAddress = ConfigurationManager.AppSettings["IP"].ToString();
        port = ConfigurationManager.AppSettings["Port"].ToString();

    }

    /// <summary>
    /// 指定IP、端口号
    /// </summary>
    /// <param name="IP">IP地址</param>
    /// <param name="Port">端口号</param>
    public Service(string IP, string Port)
    {
        ipAddress = IP;
        port = Port;
    }

    /// <summary>
    /// 获取当前PinKey、MacKey
    /// </summary>
    /// <returns></returns>
    [WebMethod(Description="获取当前最新的Pin、Mac")]
    public string GetKey(string temp)
    {
        string KeyStr = "NULL|NULL|NULL";//流水|Pin|Mac
        try
        {
            string serverPath = "C:\\PinKey.txt";
            using (StreamReader sr = new StreamReader(serverPath))
            {
                KeyStr = sr.ReadToEnd();
            }
        }
        catch(Exception ex)
        {
            //LogHelper.Log_Wirte("LogMachine", ex.Message, false, true);
            LogHelper.Log_Wirte("LogMachine", ex.Message, "GetKey", false, true);
        }
        return KeyStr;
    }

    /// <summary>
    /// 初始化数据集结构
    /// </summary>
    /// <returns></returns>
    private DataSet GetInitDataSet()
    {
        DataSet ds = new DataSet();
        DataTable dt = new DataTable("str");
        dt.Columns.Add("视觉检测字符串发");
        dt.Columns.Add("包头视觉检测字符串收");
        dt.Columns.Add("包体视觉检测字符串收");
        dt.Columns.Add("发包函数执行阶段");
        dt.Columns.Add("返回的日志");
        dt.Columns.Add("状态信息");
        dt.Rows.Add(new object[] { "", "", "", "", "" });
        ds.Tables.Add(dt);
        DataTable dt_Result = new DataTable("result");
        ds.Tables.Add(dt_Result);
        return ds;
    }

    /// <summary>
    /// 变更银行卡号_请求
    /// </summary>
    /// <returns>初始化数据表</returns>
    [WebMethod(Description = "变更银行帐户_请求_26710")]
    public DataTable Get_ModifyBankCard()
    {
        DataTable dt = null;
        try
        {
            FileInfo FI = new FileInfo(Server.MapPath("~/XML/券商发起-变更银行账户#26710.xml"));
            dt = new DataTable();
            dt.ReadXml(FI.FullName);
        }
        catch (Exception ex)
        {
            LogHelper.Log_Wirte("LogMachine", ex.Message, "Get_ModifyBankCard", false, true);
            dt = null;
        }
        return dt;
    }

    /// <summary>
    /// 变更银行卡号_发送
    /// </summary>
    /// <param name="dt">获取初始化数据表并修改后的数据</param>
    /// <returns>序列化后的银行反馈结果</returns>
    [WebMethod(Description = "变更银行帐户_发送_26710")]
    public DataSet Set_ModifyBankCard(DataTable dt)
    {
        ClassSend CS = new ClassSend();
        DataSet ds = GetInitDataSet();
        try
        {
            if (dt == null)
            {
                ds.Tables["str"].Rows[0]["状态信息"] = "数据表错误。";
                return ds;
            }

            Hashtable htSend = CS.Get_ByteAndString_From_DataTable(dt, false, "");
            ds.Tables["str"].Rows[0]["视觉检测字符串发"] = htSend["视觉检测字符串"].ToString();//预先放入要返回的数据表中
            byte[] btArr = (byte[])(htSend["字节流"]);
            Hashtable htReTURN = CS.BeginSend(btArr);
            ds.Tables["str"].Rows[0]["发包函数执行阶段"] = htReTURN["发包函数执行阶段"].ToString();//发包执行阶段
            ds.Tables["str"].Rows[0]["返回的日志"] = htReTURN["返回的日志"].ToString();//发包的结果
            byte[] btReturn = (byte[])htReTURN["返回的数据包"];

            Hashtable htResultToPMonkey = CS.Get_BaotouAndDataTableAndString_From_Byte(btReturn, false, "");
            ds.Tables["str"].Rows[0]["包头视觉检测字符串收"] = htResultToPMonkey["包头视觉检测字符串"].ToString();//银行返回的包头
            ds.Tables["str"].Rows[0]["包体视觉检测字符串收"] = htResultToPMonkey["包体视觉检测字符串"].ToString();//银行返回的包体
            DataTable dtResult = (DataTable)htResultToPMonkey["包体数据集"];//需要返回给程序员的数据集
            if (dtResult.Rows[0]["ErrorNo"].ToString() != "0000")
            {
                //如果包含错误信息
                ds.Tables["str"].Rows[0]["状态信息"] = dtResult.Rows[0]["ErrorInfo"];
            }
            else
            {
                //如果包含银行流水号
                ds.Tables["str"].Rows[0]["状态信息"] = "ok";
            }
            dtResult.TableName = "result";
            //ds.Tables["result"] = dtResult;
            ds.Tables.Remove("result");
            ds.Tables.Add(dtResult);


            LogHelper.Log_Wirte("LogMachine", "发送包：" + (htSend["视觉检测字符串"] == null ? "Null" : htSend["视觉检测字符串"].ToString()) + "| 银行返回的包头：" + (htResultToPMonkey["包头视觉检测字符串"] == null ? "Null" : htResultToPMonkey["包头视觉检测字符串"].ToString()) + "| 银行返回的包体：" + (htResultToPMonkey["包体视觉检测字符串"] == null ? "Null" : htResultToPMonkey["包体视觉检测字符串"].ToString()), "Set_ModifyBankCard", false, false);
        }
        catch (Exception ex)
        {
            LogHelper.Log_Wirte("LogMachine", ex.Message, "Set_ModifyBankCard", false, true);
            ds.Tables["str"].Rows[0]["状态信息"] = ex.Message;
        }
        return ds;
    }

    /// <summary>
    /// 获取用户银行资金余额_请求
    /// </summary>
    /// <returns>初始化数据表</returns>
    [WebMethod(Description = "获取用户银行资金余额_请求_24603")]
    public DataTable Get_UMoneyInBank()
    {
        DataTable dt = null;
        try
        {
            FileInfo FI = new FileInfo(Server.MapPath("~/XML/券商发起-查银行资金余额#24603.xml"));
            dt = new DataTable();
            dt.ReadXml(FI.FullName);
        }
        catch(Exception ex)
        {
            LogHelper.Log_Wirte("LogMachine", ex.Message, "Get_UMoneyInBank", false, true);
            dt = null;
        }
        return dt;
    }

    /// <summary>
    /// 获取用户资金余额_发送_24603
    /// </summary>
    /// <param name="dt">获取初始化数据表并修改后的数据</param>
    /// <returns>序列化后的银行反馈结果</returns>
    [WebMethod(Description = "获取用户资金余额_发送_24603")]
    public DataSet Set_UMoneyInBank(DataTable dt)
    {
        ClassSend CS = new ClassSend();
        DataSet ds = GetInitDataSet();
        try
        {
            if (dt == null)
            {
                ds.Tables["str"].Rows[0]["状态信息"] = "数据表错误。";
                return ds;
            }

            Hashtable htSend = CS.Get_ByteAndString_From_DataTable(dt, false, "");
            ds.Tables["str"].Rows[0]["视觉检测字符串发"] = htSend["视觉检测字符串"].ToString();//预先放入要返回的数据表中
            byte[] btArr = (byte[])(htSend["字节流"]);
            Hashtable htReTURN = CS.BeginSend(btArr);
            ds.Tables["str"].Rows[0]["发包函数执行阶段"] = htReTURN["发包函数执行阶段"].ToString();//发包执行阶段
            ds.Tables["str"].Rows[0]["返回的日志"] = htReTURN["返回的日志"].ToString();//发包的结果
            byte[] btReturn = (byte[])htReTURN["返回的数据包"];

            Hashtable htResultToPMonkey = CS.Get_BaotouAndDataTableAndString_From_Byte(btReturn, false, "");
            ds.Tables["str"].Rows[0]["包头视觉检测字符串收"] = htResultToPMonkey["包头视觉检测字符串"].ToString();//银行返回的包头
            ds.Tables["str"].Rows[0]["包体视觉检测字符串收"] = htResultToPMonkey["包体视觉检测字符串"].ToString();//银行返回的包体
            DataTable dtResult = (DataTable)htResultToPMonkey["包体数据集"];//需要返回给程序员的数据集
            if ((dtResult.Rows[0]["ErrorNo"].ToString() != "0000"))
            {
                //如果包含错误信息
                ds.Tables["str"].Rows[0]["状态信息"] = dtResult.Rows[0]["ErrorInfo"];
            }
            else 
            {
                //如果包含银行流水号
                ds.Tables["str"].Rows[0]["状态信息"] = "ok";
            }
            dtResult.TableName = "result";
            //ds.Tables["result"] = dtResult;
            ds.Tables.Remove("result");
            ds.Tables.Add(dtResult);

            LogHelper.Log_Wirte("LogMachine", "发送包：" + (htSend["视觉检测字符串"] == null ? "Null" : htSend["视觉检测字符串"].ToString()) + "| 银行返回的包头：" + (htResultToPMonkey["包头视觉检测字符串"] == null ? "Null" : htResultToPMonkey["包头视觉检测字符串"].ToString()) + "| 银行返回的包体：" + (htResultToPMonkey["包体视觉检测字符串"] == null ? "Null" : htResultToPMonkey["包体视觉检测字符串"].ToString()), "Set_UMoneyInBank", false, false);
        }
        catch (Exception ex)
        {
            LogHelper.Log_Wirte("LogMachine", ex.Message, "Set_UMoneyInBank", false, true);
            ds.Tables["str"].Rows[0]["状态信息"] = ex.Message;
        }
        return ds;
    }

    /// <summary>
    /// 获取冲银行转证券数据表_请求_24703
    /// </summary>
    /// <returns></returns>
    [WebMethod(Description = "冲银行转证券_请求_24703")]
    public DataTable Get_CBankToSecurities()
    {
        DataTable dt = null;
        try
        {
            FileInfo FI = new FileInfo(Server.MapPath("~/XML/券商发起-冲银行转证券#24703.xml"));
            dt = new DataTable();
            dt.ReadXml(FI.FullName);
        }
        catch (Exception ex)
        {
            LogHelper.Log_Wirte("LogMachine", ex.Message, "Get_CBankToSecurities", false, true);
            dt = null;
        }
        return dt;
    }

    /// <summary>
    /// 冲银行转证券指令_发送_24703
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    [WebMethod(Description = "冲银行转证券_发送_24703")]
    public DataSet Set_CBankToSecurities(DataTable dt)
    {
        ClassSend CS = new ClassSend();
        DataSet ds = GetInitDataSet();
        try
        {
            if (dt == null)
            {
                ds.Tables["str"].Rows[0]["状态信息"] = "数据表错误。";
                return ds;
            }

            Hashtable htSend = CS.Get_ByteAndString_From_DataTable(dt, false, "");
            ds.Tables["str"].Rows[0]["视觉检测字符串发"] = htSend["视觉检测字符串"].ToString();//预先放入要返回的数据表中
            byte[] btArr = (byte[])(htSend["字节流"]);
            Hashtable htReTURN = CS.BeginSend(btArr);
            ds.Tables["str"].Rows[0]["发包函数执行阶段"] = htReTURN["发包函数执行阶段"].ToString();//发包执行阶段
            ds.Tables["str"].Rows[0]["返回的日志"] = htReTURN["返回的日志"].ToString();//发包的结果
            byte[] btReturn = (byte[])htReTURN["返回的数据包"];

            Hashtable htResultToPMonkey = CS.Get_BaotouAndDataTableAndString_From_Byte(btReturn, false, "");
            ds.Tables["str"].Rows[0]["包头视觉检测字符串收"] = htResultToPMonkey["包头视觉检测字符串"].ToString();//银行返回的包头
            ds.Tables["str"].Rows[0]["包体视觉检测字符串收"] = htResultToPMonkey["包体视觉检测字符串"].ToString();//银行返回的包体
            DataTable dtResult = (DataTable)htResultToPMonkey["包体数据集"];//需要返回给程序员的数据集
            if ((dtResult.Rows[0]["ErrorNo"].ToString() != "0000"))
            {
                //如果包含错误信息
                ds.Tables["str"].Rows[0]["状态信息"] = dtResult.Rows[0]["ErrorInfo"];
            }
            else 
            {
                //如果包含银行流水号
                ds.Tables["str"].Rows[0]["状态信息"] = "ok";
            }
            dtResult.TableName = "result";
            //ds.Tables["result"] = dtResult;
            ds.Tables.Remove("result");
            ds.Tables.Add(dtResult);

            LogHelper.Log_Wirte("LogMachine", "发送包：" + (htSend["视觉检测字符串"] == null ? "Null" : htSend["视觉检测字符串"].ToString()) + "| 银行返回的包头：" + (htResultToPMonkey["包头视觉检测字符串"] == null ? "Null" : htResultToPMonkey["包头视觉检测字符串"].ToString()) + "| 银行返回的包体：" + (htResultToPMonkey["包体视觉检测字符串"] == null ? "Null" : htResultToPMonkey["包体视觉检测字符串"].ToString()), "Set_CBankToSecurities", false, false);
        }
        catch (Exception ex)
        {
            LogHelper.Log_Wirte("LogMachine", ex.Message, "Set_CBankToSecurities", false, true);
            ds.Tables["str"].Rows[0]["状态信息"] = ex.Message;
        }
        return ds;
    }

    //-----------------------------------------------------------------------------

    /// <summary>
    /// 券商发起-冲证券转银行#24704
    /// </summary>
    /// <returns></returns>
    [WebMethod(Description = "券商发起-冲证券转银行#24704")]
    public DataTable Get_CSecuritiesToBank()
    {
        DataTable dt = null;
        try
        {
            FileInfo FI = new FileInfo(Server.MapPath("~/XML/券商发起-冲证券转银行#24704.xml"));
            dt = new DataTable();
            dt.ReadXml(FI.FullName);
        }
        catch (Exception ex)
        {
            LogHelper.Log_Wirte("LogMachine", ex.Message, "Get_CSecuritiesToBank", false, true);
            dt = null;
        }
        return dt;
    }

    /// <summary>
    /// 券商发起-冲证券转银行#24704
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    [WebMethod(Description = "券商发起-冲证券转银行#24704")]
    public DataSet Set_CSecuritiesToBank(DataTable dt)
    {
        ClassSend CS = new ClassSend();
        DataSet ds = GetInitDataSet();
        try
        {
            if (dt == null)
            {
                ds.Tables["str"].Rows[0]["状态信息"] = "数据表错误。";
                return ds;
            }

            Hashtable htSend = CS.Get_ByteAndString_From_DataTable(dt, false, "");
            ds.Tables["str"].Rows[0]["视觉检测字符串发"] = htSend["视觉检测字符串"].ToString();//预先放入要返回的数据表中
            byte[] btArr = (byte[])(htSend["字节流"]);
            Hashtable htReTURN = CS.BeginSend(btArr);
            ds.Tables["str"].Rows[0]["发包函数执行阶段"] = htReTURN["发包函数执行阶段"].ToString();//发包执行阶段
            ds.Tables["str"].Rows[0]["返回的日志"] = htReTURN["返回的日志"].ToString();//发包的结果
            byte[] btReturn = (byte[])htReTURN["返回的数据包"];

            Hashtable htResultToPMonkey = CS.Get_BaotouAndDataTableAndString_From_Byte(btReturn, false, "");
            ds.Tables["str"].Rows[0]["包头视觉检测字符串收"] = htResultToPMonkey["包头视觉检测字符串"].ToString();//银行返回的包头
            ds.Tables["str"].Rows[0]["包体视觉检测字符串收"] = htResultToPMonkey["包体视觉检测字符串"].ToString();//银行返回的包体
            DataTable dtResult = (DataTable)htResultToPMonkey["包体数据集"];//需要返回给程序员的数据集
            if ((dtResult.Rows[0]["ErrorNo"].ToString() != "0000"))
            {
                //如果包含错误信息
                ds.Tables["str"].Rows[0]["状态信息"] = dtResult.Rows[0]["ErrorInfo"];
            }
            else 
            {
                //如果包含银行流水号
                ds.Tables["str"].Rows[0]["状态信息"] = "ok";
            }
            dtResult.TableName = "result";
            //ds.Tables["result"] = dtResult;
            ds.Tables.Remove("result");
            ds.Tables.Add(dtResult);

            LogHelper.Log_Wirte("LogMachine", "发送包：" + (htSend["视觉检测字符串"] == null ? "Null" : htSend["视觉检测字符串"].ToString()) + "| 银行返回的包头：" + (htResultToPMonkey["包头视觉检测字符串"] == null ? "Null" : htResultToPMonkey["包头视觉检测字符串"].ToString()) + "| 银行返回的包体：" + (htResultToPMonkey["包体视觉检测字符串"] == null ? "Null" : htResultToPMonkey["包体视觉检测字符串"].ToString()), "Set_CSecuritiesToBank", false, false);
        }
        catch (Exception ex)
        {
            LogHelper.Log_Wirte("LogMachine", ex.Message, "Set_CSecuritiesToBank", false, true);
            ds.Tables["str"].Rows[0]["状态信息"] = ex.Message;
        }
        return ds;
    }

    /// <summary>
    /// 券商发起-客户变更存管银行#26713
    /// </summary>
    /// <returns></returns>
    [WebMethod(Description = "券商发起-客户变更存管银行#26713")]
    public DataTable Get_ModifyBank()
    {
        DataTable dt = null;
        try
        {
            FileInfo FI = new FileInfo(Server.MapPath("~/XML/券商发起-客户变更存管银行#26713.xml"));
            dt = new DataTable();
            dt.ReadXml(FI.FullName);
        }
        catch (Exception ex)
        {
            LogHelper.Log_Wirte("LogMachine", ex.Message, "Get_ModifyBank", false, true);
            dt = null;
        }
        return dt;
    }

    /// <summary>
    /// 券商发起-客户变更存管银行#26713
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    [WebMethod(Description = "券商发起-客户变更存管银行#26713")]
    public DataSet Set_ModifyBank(DataTable dt)
    {
        ClassSend CS = new ClassSend();
        DataSet ds = GetInitDataSet();
        try
        {
            if (dt == null)
            {
                ds.Tables["str"].Rows[0]["状态信息"] = "数据表错误。";
                return ds;
            }

            Hashtable htSend = CS.Get_ByteAndString_From_DataTable(dt, false, "");
            ds.Tables["str"].Rows[0]["视觉检测字符串发"] = htSend["视觉检测字符串"].ToString();//预先放入要返回的数据表中
            byte[] btArr = (byte[])(htSend["字节流"]);
            Hashtable htReTURN = CS.BeginSend(btArr);
            ds.Tables["str"].Rows[0]["发包函数执行阶段"] = htReTURN["发包函数执行阶段"].ToString();//发包执行阶段
            ds.Tables["str"].Rows[0]["返回的日志"] = htReTURN["返回的日志"].ToString();//发包的结果
            byte[] btReturn = (byte[])htReTURN["返回的数据包"];

            Hashtable htResultToPMonkey = CS.Get_BaotouAndDataTableAndString_From_Byte(btReturn, false, "");
            ds.Tables["str"].Rows[0]["包头视觉检测字符串收"] = htResultToPMonkey["包头视觉检测字符串"].ToString();//银行返回的包头
            ds.Tables["str"].Rows[0]["包体视觉检测字符串收"] = htResultToPMonkey["包体视觉检测字符串"].ToString();//银行返回的包体
            DataTable dtResult = (DataTable)htResultToPMonkey["包体数据集"];//需要返回给程序员的数据集
            if ((dtResult.Rows[0]["ErrorNo"].ToString() != "0000"))
            {
                //如果包含错误信息
                ds.Tables["str"].Rows[0]["状态信息"] = dtResult.Rows[0]["ErrorInfo"];
            }
            else 
            {
                //如果包含银行流水号
                ds.Tables["str"].Rows[0]["状态信息"] = "ok";
            }
            dtResult.TableName = "result";
            //ds.Tables["result"] = dtResult;
            ds.Tables.Remove("result");
            ds.Tables.Add(dtResult);
            LogHelper.Log_Wirte("LogMachine", "发送包：" + (htSend["视觉检测字符串"] == null ? "Null" : htSend["视觉检测字符串"].ToString()) + "| 银行返回的包头：" + (htResultToPMonkey["包头视觉检测字符串"] == null ? "Null" : htResultToPMonkey["包头视觉检测字符串"].ToString()) + "| 银行返回的包体：" + (htResultToPMonkey["包体视觉检测字符串"] == null ? "Null" : htResultToPMonkey["包体视觉检测字符串"].ToString()), "Set_ModifyBank", false, false);

        }
        catch (Exception ex)
        {
            LogHelper.Log_Wirte("LogMachine", ex.Message, "Set_ModifyBank", false, true);
            ds.Tables["str"].Rows[0]["状态信息"] = ex.Message;
        }
        return ds;
    }

    /// <summary>
    /// 券商发起-客户存管签约#26701
    /// </summary>
    /// <returns></returns>
    [WebMethod(Description = "券商发起-客户存管签约#26701")]
    public DataTable Get_Deal()
    {
        DataTable dt = null;
        try
        {
            FileInfo FI = new FileInfo(Server.MapPath("~/XML/券商发起-客户存管签约#26701.xml"));
            dt = new DataTable();
            dt.ReadXml(FI.FullName);
        }
        catch (Exception ex)
        {
            LogHelper.Log_Wirte("LogMachine", ex.Message, "Get_Deal", false, true);
            dt = null;
        }
        return dt;
    }

    /// <summary>
    /// 券商发起-客户存管签约#26701
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    [WebMethod(Description = "券商发起-客户存管签约#26701")]
    public DataSet Set_Deal(DataTable dt)
    {
        ClassSend CS = new ClassSend();
        DataSet ds = GetInitDataSet();
        try
        {
            if (dt == null)
            {
                ds.Tables["str"].Rows[0]["状态信息"] = "数据表错误。";
                return ds;
            }

            Hashtable htSend = CS.Get_ByteAndString_From_DataTable(dt, false, "");
            ds.Tables["str"].Rows[0]["视觉检测字符串发"] = htSend["视觉检测字符串"].ToString();//预先放入要返回的数据表中
            byte[] btArr = (byte[])(htSend["字节流"]);
            Hashtable htReTURN = CS.BeginSend(btArr);
            ds.Tables["str"].Rows[0]["发包函数执行阶段"] = htReTURN["发包函数执行阶段"].ToString();//发包执行阶段
            ds.Tables["str"].Rows[0]["返回的日志"] = htReTURN["返回的日志"].ToString();//发包的结果
            byte[] btReturn = (byte[])htReTURN["返回的数据包"];

            Hashtable htResultToPMonkey = CS.Get_BaotouAndDataTableAndString_From_Byte(btReturn, false, "");
            ds.Tables["str"].Rows[0]["包头视觉检测字符串收"] = htResultToPMonkey["包头视觉检测字符串"].ToString();//银行返回的包头
            ds.Tables["str"].Rows[0]["包体视觉检测字符串收"] = htResultToPMonkey["包体视觉检测字符串"].ToString();//银行返回的包体
            DataTable dtResult = (DataTable)htResultToPMonkey["包体数据集"];//需要返回给程序员的数据集
            if ((dtResult.Rows[0]["ErrorNo"].ToString() != "0000"))
            {
                //如果包含错误信息
                ds.Tables["str"].Rows[0]["状态信息"] = dtResult.Rows[0]["ErrorInfo"];
            }
            else 
            {
                //如果包含银行流水号
                ds.Tables["str"].Rows[0]["状态信息"] = "ok";
            }
            dtResult.TableName = "result";
            //ds.Tables["result"] = dtResult;
            ds.Tables.Remove("result");
            ds.Tables.Add(dtResult);
            LogHelper.Log_Wirte("LogMachine", "发送包：" + (htSend["视觉检测字符串"] == null ? "Null" : htSend["视觉检测字符串"].ToString()) + "| 银行返回的包头：" + (htResultToPMonkey["包头视觉检测字符串"] == null ? "Null" : htResultToPMonkey["包头视觉检测字符串"].ToString()) + "| 银行返回的包体：" + (htResultToPMonkey["包体视觉检测字符串"] == null ? "Null" : htResultToPMonkey["包体视觉检测字符串"].ToString()), "Set_Deal", false, false);

        }
        catch (Exception ex)
        {
            LogHelper.Log_Wirte("LogMachine", ex.Message, "Set_Deal", false, true);
            ds.Tables["str"].Rows[0]["状态信息"] = ex.Message;
        }
        return ds;
    }

    /// <summary>
    /// 券商发起-客户登记激活#26611
    /// </summary>
    /// <returns></returns>
    [WebMethod(Description = "券商发起-客户登记激活#26611")]
    public DataTable Get_Active()
    {
        DataTable dt = null;
        try
        {
            FileInfo FI = new FileInfo(Server.MapPath("~/XML/券商发起-客户登记激活#26611.xml"));
            dt = new DataTable();
            dt.ReadXml(FI.FullName);
        }
        catch (Exception ex)
        {
            LogHelper.Log_Wirte("LogMachine", ex.Message, "Get_Active", false, true);
            dt = null;
        }
        return dt;
    }

    /// <summary>
    /// 券商发起-客户存管签约#26701
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    [WebMethod(Description = "券商发起-客户登记激活#26611")]
    public DataSet Set_Active(DataTable dt)
    {
        ClassSend CS = new ClassSend();
        DataSet ds = GetInitDataSet();
        try
        {
            if (dt == null)
            {
                ds.Tables["str"].Rows[0]["状态信息"] = "数据表错误。";
                return ds;
            }

            Hashtable htSend = CS.Get_ByteAndString_From_DataTable(dt, false, "");
            ds.Tables["str"].Rows[0]["视觉检测字符串发"] = htSend["视觉检测字符串"].ToString();//预先放入要返回的数据表中
            byte[] btArr = (byte[])(htSend["字节流"]);
            Hashtable htReTURN = CS.BeginSend(btArr);
            ds.Tables["str"].Rows[0]["发包函数执行阶段"] = htReTURN["发包函数执行阶段"].ToString();//发包执行阶段
            ds.Tables["str"].Rows[0]["返回的日志"] = htReTURN["返回的日志"].ToString();//发包的结果
            byte[] btReturn = (byte[])htReTURN["返回的数据包"];

            Hashtable htResultToPMonkey = CS.Get_BaotouAndDataTableAndString_From_Byte(btReturn, false, "");
            ds.Tables["str"].Rows[0]["包头视觉检测字符串收"] = htResultToPMonkey["包头视觉检测字符串"].ToString();//银行返回的包头
            ds.Tables["str"].Rows[0]["包体视觉检测字符串收"] = htResultToPMonkey["包体视觉检测字符串"].ToString();//银行返回的包体
            DataTable dtResult = (DataTable)htResultToPMonkey["包体数据集"];//需要返回给程序员的数据集
            if ((dtResult.Rows[0]["ErrorNo"].ToString() != "0000"))
            {
                //如果包含错误信息
                ds.Tables["str"].Rows[0]["状态信息"] = dtResult.Rows[0]["ErrorInfo"];
            }
            else 
            {
                //如果包含银行流水号
                ds.Tables["str"].Rows[0]["状态信息"] = "ok";
            }
            dtResult.TableName = "result";
            //ds.Tables["result"] = dtResult;
            ds.Tables.Remove("result");
            ds.Tables.Add(dtResult);

            LogHelper.Log_Wirte("LogMachine", "发送包：" + (htSend["视觉检测字符串"] == null ? "Null" : htSend["视觉检测字符串"].ToString()) + "| 银行返回的包头：" + (htResultToPMonkey["包头视觉检测字符串"] == null ? "Null" : htResultToPMonkey["包头视觉检测字符串"].ToString()) + "| 银行返回的包体：" + (htResultToPMonkey["包体视觉检测字符串"] == null ? "Null" : htResultToPMonkey["包体视觉检测字符串"].ToString()), "Set_Active", false, false);

        }
        catch (Exception ex)
        {
            LogHelper.Log_Wirte("LogMachine", ex.Message, "Set_Active", false, true);
            ds.Tables["str"].Rows[0]["状态信息"] = ex.Message;
        }
        return ds;
    }

    /// <summary>
    /// 券商发起-客户结息#26712
    /// </summary>
    /// <returns></returns>
    [WebMethod(Description = "券商发起-客户结息#26712")]
    public DataTable Get_UserInterest()
    {
        DataTable dt = null;
        try
        {
            FileInfo FI = new FileInfo(Server.MapPath("~/XML/券商发起-客户结息#26712.xml"));
            dt = new DataTable();
            dt.ReadXml(FI.FullName);
        }
        catch (Exception ex)
        {
            LogHelper.Log_Wirte("LogMachine", ex.Message, "Get_UserInterest", false, true);
            dt = null;
        }
        return dt;
    }

    /// <summary>
    /// 券商发起-客户结息#26712
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    [WebMethod(Description = "券商发起-客户结息#26712")]
    public DataSet Set_UserInterest(DataTable dt)
    {
        ClassSend CS = new ClassSend();
        DataSet ds = GetInitDataSet();
        try
        {
            if (dt == null)
            {
                ds.Tables["str"].Rows[0]["状态信息"] = "数据表错误。";
                return ds;
            }

            Hashtable htSend = CS.Get_ByteAndString_From_DataTable(dt, false, "");
            ds.Tables["str"].Rows[0]["视觉检测字符串发"] = htSend["视觉检测字符串"].ToString();//预先放入要返回的数据表中
            byte[] btArr = (byte[])(htSend["字节流"]);
            Hashtable htReTURN = CS.BeginSend(btArr);
            ds.Tables["str"].Rows[0]["发包函数执行阶段"] = htReTURN["发包函数执行阶段"].ToString();//发包执行阶段
            ds.Tables["str"].Rows[0]["返回的日志"] = htReTURN["返回的日志"].ToString();//发包的结果
            byte[] btReturn = (byte[])htReTURN["返回的数据包"];

            Hashtable htResultToPMonkey = CS.Get_BaotouAndDataTableAndString_From_Byte(btReturn, false, "");
            ds.Tables["str"].Rows[0]["包头视觉检测字符串收"] = htResultToPMonkey["包头视觉检测字符串"].ToString();//银行返回的包头
            ds.Tables["str"].Rows[0]["包体视觉检测字符串收"] = htResultToPMonkey["包体视觉检测字符串"].ToString();//银行返回的包体
            DataTable dtResult = (DataTable)htResultToPMonkey["包体数据集"];//需要返回给程序员的数据集
            if ((dtResult.Rows[0]["ErrorNo"].ToString() != "0000"))
            {
                //如果包含错误信息
                ds.Tables["str"].Rows[0]["状态信息"] = dtResult.Rows[0]["ErrorInfo"];
            }
            else 
            {
                //如果包含银行流水号
                ds.Tables["str"].Rows[0]["状态信息"] = "ok";
            }
            dtResult.TableName = "result";
            //ds.Tables["result"] = dtResult;
            ds.Tables.Remove("result");
            ds.Tables.Add(dtResult);

            LogHelper.Log_Wirte("LogMachine", "发送包：" + (htSend["视觉检测字符串"] == null ? "Null" : htSend["视觉检测字符串"].ToString()) + "| 银行返回的包头：" + (htResultToPMonkey["包头视觉检测字符串"] == null ? "Null" : htResultToPMonkey["包头视觉检测字符串"].ToString()) + "| 银行返回的包体：" + (htResultToPMonkey["包体视觉检测字符串"] == null ? "Null" : htResultToPMonkey["包体视觉检测字符串"].ToString()), "Set_UserInterest", false, false);
        }
        catch (Exception ex)
        {
            LogHelper.Log_Wirte("LogMachine", ex.Message, "Set_UserInterest", false, true);
            ds.Tables["str"].Rows[0]["状态信息"] = ex.Message;
        }
        return ds;
    }

    /// <summary>
    /// 券商发起-客户解约#26703
    /// </summary>
    /// <returns></returns>
    [WebMethod(Description = "券商发起-客户解约#26703")]
    public DataTable Get_DealBroked()
    {
        DataTable dt = null;
        try
        {
            FileInfo FI = new FileInfo(Server.MapPath("~/XML/券商发起-客户解约#26703.xml"));
            dt = new DataTable();
            dt.ReadXml(FI.FullName);
        }
        catch (Exception ex)
        {
            LogHelper.Log_Wirte("LogMachine", ex.Message, "Get_DealBroked", false, true);
            dt = null;
        }
        return dt;
    }

    /// <summary>
    /// 券商发起-客户解约#26703
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    [WebMethod(Description = "券商发起-客户解约#26703")]
    public DataSet Set_DealBroked(DataTable dt)
    {
        ClassSend CS = new ClassSend();
        DataSet ds = GetInitDataSet();
        try
        {
            if (dt == null)
            {
                ds.Tables["str"].Rows[0]["状态信息"] = "数据表错误。";
                return ds;
            }

            Hashtable htSend = CS.Get_ByteAndString_From_DataTable(dt, false, "");
            ds.Tables["str"].Rows[0]["视觉检测字符串发"] = htSend["视觉检测字符串"].ToString();//预先放入要返回的数据表中
            byte[] btArr = (byte[])(htSend["字节流"]);
            Hashtable htReTURN = CS.BeginSend(btArr);
            ds.Tables["str"].Rows[0]["发包函数执行阶段"] = htReTURN["发包函数执行阶段"].ToString();//发包执行阶段
            ds.Tables["str"].Rows[0]["返回的日志"] = htReTURN["返回的日志"].ToString();//发包的结果
            byte[] btReturn = (byte[])htReTURN["返回的数据包"];

            Hashtable htResultToPMonkey = CS.Get_BaotouAndDataTableAndString_From_Byte(btReturn, false, "");
            ds.Tables["str"].Rows[0]["包头视觉检测字符串收"] = htResultToPMonkey["包头视觉检测字符串"].ToString();//银行返回的包头
            ds.Tables["str"].Rows[0]["包体视觉检测字符串收"] = htResultToPMonkey["包体视觉检测字符串"].ToString();//银行返回的包体
            DataTable dtResult = (DataTable)htResultToPMonkey["包体数据集"];//需要返回给程序员的数据集
            if ((dtResult.Rows[0]["ErrorNo"].ToString() != "0000"))
            {
                //如果包含错误信息
                ds.Tables["str"].Rows[0]["状态信息"] = dtResult.Rows[0]["ErrorInfo"];
            }
            else 
            {
                //如果包含银行流水号
                ds.Tables["str"].Rows[0]["状态信息"] = "ok";
            }
            dtResult.TableName = "result";
            //ds.Tables["result"] = dtResult;
            ds.Tables.Remove("result");
            ds.Tables.Add(dtResult);

            LogHelper.Log_Wirte("LogMachine", "发送包：" + (htSend["视觉检测字符串"] == null ? "Null" : htSend["视觉检测字符串"].ToString()) + "| 银行返回的包头：" + (htResultToPMonkey["包头视觉检测字符串"] == null ? "Null" : htResultToPMonkey["包头视觉检测字符串"].ToString()) + "| 银行返回的包体：" + (htResultToPMonkey["包体视觉检测字符串"] == null ? "Null" : htResultToPMonkey["包体视觉检测字符串"].ToString()), "Set_DealBroked", false, false);
        }
        catch (Exception ex)
        {
            LogHelper.Log_Wirte("LogMachine", ex.Message, "Set_DealBroked", false, true);
            ds.Tables["str"].Rows[0]["状态信息"] = ex.Message;
        }
        return ds;
    }

    /// <summary>
    /// 券商发起-客户预指定存管签约确认#26702
    /// </summary>
    /// <returns></returns>
    [WebMethod(Description = "券商发起-客户预指定存管签约确认#26702")]
    public DataTable Get_DealAerify()
    {
        DataTable dt = null;
        try
        {
            FileInfo FI = new FileInfo(Server.MapPath("~/XML/券商发起-客户预指定存管签约确认#26702.xml"));
            dt = new DataTable();
            dt.ReadXml(FI.FullName);
        }
        catch (Exception ex)
        {
            LogHelper.Log_Wirte("LogMachine", ex.Message, "Get_DealAerify", false, true);
            dt = null;
        }
        return dt;
    }

    /// <summary>
    /// 券商发起-客户解约#26703
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    [WebMethod(Description = "券商发起-客户预指定存管签约确认#26702")]
    public DataSet Set_DealAerify(DataTable dt)
    {
        ClassSend CS = new ClassSend();
        DataSet ds = GetInitDataSet();
        try
        {
            if (dt == null)
            {
                ds.Tables["str"].Rows[0]["状态信息"] = "数据表错误。";
                return ds;
            }

            Hashtable htSend = CS.Get_ByteAndString_From_DataTable(dt, false, "");
            ds.Tables["str"].Rows[0]["视觉检测字符串发"] = htSend["视觉检测字符串"].ToString();//预先放入要返回的数据表中
            byte[] btArr = (byte[])(htSend["字节流"]);
            Hashtable htReTURN = CS.BeginSend(btArr);
            ds.Tables["str"].Rows[0]["发包函数执行阶段"] = htReTURN["发包函数执行阶段"].ToString();//发包执行阶段
            ds.Tables["str"].Rows[0]["返回的日志"] = htReTURN["返回的日志"].ToString();//发包的结果
            byte[] btReturn = (byte[])htReTURN["返回的数据包"];

            Hashtable htResultToPMonkey = CS.Get_BaotouAndDataTableAndString_From_Byte(btReturn, false, "");
            ds.Tables["str"].Rows[0]["包头视觉检测字符串收"] = htResultToPMonkey["包头视觉检测字符串"].ToString();//银行返回的包头
            ds.Tables["str"].Rows[0]["包体视觉检测字符串收"] = htResultToPMonkey["包体视觉检测字符串"].ToString();//银行返回的包体
            DataTable dtResult = (DataTable)htResultToPMonkey["包体数据集"];//需要返回给程序员的数据集
            if ((dtResult.Rows[0]["ErrorNo"].ToString() != "0000"))
            {
                //如果包含错误信息
                ds.Tables["str"].Rows[0]["状态信息"] = dtResult.Rows[0]["ErrorInfo"];
            }
            else 
            {
                //如果包含银行流水号
                ds.Tables["str"].Rows[0]["状态信息"] = "ok";
            }
            dtResult.TableName = "result";
            //ds.Tables["result"] = dtResult;
            ds.Tables.Remove("result");
            ds.Tables.Add(dtResult);

            LogHelper.Log_Wirte("LogMachine", "发送包：" + (htSend["视觉检测字符串"] == null ? "Null" : htSend["视觉检测字符串"].ToString()) + "| 银行返回的包头：" + (htResultToPMonkey["包头视觉检测字符串"] == null ? "Null" : htResultToPMonkey["包头视觉检测字符串"].ToString()) + "| 银行返回的包体：" + (htResultToPMonkey["包体视觉检测字符串"] == null ? "Null" : htResultToPMonkey["包体视觉检测字符串"].ToString()), "Set_DealAerify", false, false);
        }
        catch (Exception ex)
        {
            LogHelper.Log_Wirte("LogMachine", ex.Message, "Set_DealAerify", false, true);
            ds.Tables["str"].Rows[0]["状态信息"] = ex.Message;
        }
        return ds;
    }

    /// <summary>
    /// 券商发起-取银行转帐明细#24710
    /// </summary>
    /// <returns></returns>
    [WebMethod(Description = "券商发起-取银行转帐明细#24710")]
    public DataTable Get_BankMoneyDetails()
    {
        DataTable dt = null;
        try
        {
            FileInfo FI = new FileInfo(Server.MapPath("~/XML/券商发起-取银行转帐明细#24710.xml"));
            dt = new DataTable();
            dt.ReadXml(FI.FullName);
        }
        catch (Exception ex)
        {
            LogHelper.Log_Wirte("LogMachine", ex.Message, "Get_BankMoneyDetails", false, true);
            dt = null;
        }
        return dt;
    }

    /// <summary>
    /// 券商发起-取银行转帐明细#24710
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    [WebMethod(Description = "券商发起-取银行转帐明细#24710")]
    public DataSet Set_BankMoneyDetails(DataTable dt)
    {
        ClassSend CS = new ClassSend();
        DataSet ds = GetInitDataSet();
        try
        {
            if (dt == null)
            {
                ds.Tables["str"].Rows[0]["状态信息"] = "数据表错误。";
                return ds;
            }

            Hashtable htSend = CS.Get_ByteAndString_From_DataTable(dt, false, "");
            ds.Tables["str"].Rows[0]["视觉检测字符串发"] = htSend["视觉检测字符串"].ToString();//预先放入要返回的数据表中
            byte[] btArr = (byte[])(htSend["字节流"]);
            Hashtable htReTURN = CS.BeginSend(btArr);
            ds.Tables["str"].Rows[0]["发包函数执行阶段"] = htReTURN["发包函数执行阶段"].ToString();//发包执行阶段
            ds.Tables["str"].Rows[0]["返回的日志"] = htReTURN["返回的日志"].ToString();//发包的结果
            byte[] btReturn = (byte[])htReTURN["返回的数据包"];

            Hashtable htResultToPMonkey = CS.Get_BaotouAndDataTableAndString_From_Byte(btReturn, false, "");
            ds.Tables["str"].Rows[0]["包头视觉检测字符串收"] = htResultToPMonkey["包头视觉检测字符串"].ToString();//银行返回的包头
            ds.Tables["str"].Rows[0]["包体视觉检测字符串收"] = htResultToPMonkey["包体视觉检测字符串"].ToString();//银行返回的包体
            DataTable dtResult = (DataTable)htResultToPMonkey["包体数据集"];//需要返回给程序员的数据集
            if ((dtResult.Rows[0]["ErrorNo"].ToString() != "0000"))
            {
                //如果包含错误信息
                ds.Tables["str"].Rows[0]["状态信息"] = dtResult.Rows[0]["ErrorInfo"];
            }
            else 
            {
                //如果包含银行流水号
                ds.Tables["str"].Rows[0]["状态信息"] = "ok";
            }
            dtResult.TableName = "result";
            //ds.Tables["result"] = dtResult;
            ds.Tables.Remove("result");
            ds.Tables.Add(dtResult);

            LogHelper.Log_Wirte("LogMachine", "发送包：" + (htSend["视觉检测字符串"] == null ? "Null" : htSend["视觉检测字符串"].ToString()) + "| 银行返回的包头：" + (htResultToPMonkey["包头视觉检测字符串"] == null ? "Null" : htResultToPMonkey["包头视觉检测字符串"].ToString()) + "| 银行返回的包体：" + (htResultToPMonkey["包体视觉检测字符串"] == null ? "Null" : htResultToPMonkey["包体视觉检测字符串"].ToString()), "Set_BankMoneyDetails", false, false);
        }
        catch (Exception ex)
        {
            LogHelper.Log_Wirte("LogMachine", ex.Message, "Set_BankMoneyDetails", false, true);
            ds.Tables["str"].Rows[0]["状态信息"] = ex.Message;
        }
        return ds;
    }

    /// <summary>
    /// 券商发起-修改客户资料#26704
    /// </summary>
    /// <returns></returns>
    [WebMethod(Description = "券商发起-修改客户资料#26704")]
    public DataTable Get_ModifyUserInfo()
    {
        DataTable dt = null;
        try
        {
            FileInfo FI = new FileInfo(Server.MapPath("~/XML/券商发起-修改客户资料#26704.xml"));
            dt = new DataTable();
            dt.ReadXml(FI.FullName);
        }
        catch (Exception ex)
        {
            LogHelper.Log_Wirte("LogMachine", ex.Message, "Get_ModifyUserInfo", false, true);
            dt = null;
        }
        return dt;
    }

    /// <summary>
    /// 券商发起-修改客户资料#26704
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    [WebMethod(Description = "券商发起-修改客户资料#26704")]
    public DataSet Set_ModifyUserInfo(DataTable dt)
    {
        ClassSend CS = new ClassSend();
        DataSet ds = GetInitDataSet();
        try
        {
            if (dt == null)
            {
                ds.Tables["str"].Rows[0]["状态信息"] = "数据表错误。";
                return ds;
            }

            Hashtable htSend = CS.Get_ByteAndString_From_DataTable(dt, false, "");
            ds.Tables["str"].Rows[0]["视觉检测字符串发"] = htSend["视觉检测字符串"].ToString();//预先放入要返回的数据表中
            byte[] btArr = (byte[])(htSend["字节流"]);
            Hashtable htReTURN = CS.BeginSend(btArr);
            ds.Tables["str"].Rows[0]["发包函数执行阶段"] = htReTURN["发包函数执行阶段"].ToString();//发包执行阶段
            ds.Tables["str"].Rows[0]["返回的日志"] = htReTURN["返回的日志"].ToString();//发包的结果
            byte[] btReturn = (byte[])htReTURN["返回的数据包"];

            Hashtable htResultToPMonkey = CS.Get_BaotouAndDataTableAndString_From_Byte(btReturn, false, "");
            ds.Tables["str"].Rows[0]["包头视觉检测字符串收"] = htResultToPMonkey["包头视觉检测字符串"].ToString();//银行返回的包头
            ds.Tables["str"].Rows[0]["包体视觉检测字符串收"] = htResultToPMonkey["包体视觉检测字符串"].ToString();//银行返回的包体
            DataTable dtResult = (DataTable)htResultToPMonkey["包体数据集"];//需要返回给程序员的数据集
            if ((dtResult.Rows[0]["ErrorNo"].ToString() != "0000"))
            {
                //如果包含错误信息
                ds.Tables["str"].Rows[0]["状态信息"] = dtResult.Rows[0]["ErrorInfo"];
            }
            else 
            {
                //如果包含银行流水号
                ds.Tables["str"].Rows[0]["状态信息"] = "ok";
            }
            dtResult.TableName = "result";
            //ds.Tables["result"] = dtResult;
            ds.Tables.Remove("result");
            ds.Tables.Add(dtResult);

            LogHelper.Log_Wirte("LogMachine", "发送包：" + (htSend["视觉检测字符串"] == null ? "Null" : htSend["视觉检测字符串"].ToString()) + "| 银行返回的包头：" + (htResultToPMonkey["包头视觉检测字符串"] == null ? "Null" : htResultToPMonkey["包头视觉检测字符串"].ToString()) + "| 银行返回的包体：" + (htResultToPMonkey["包体视觉检测字符串"] == null ? "Null" : htResultToPMonkey["包体视觉检测字符串"].ToString()), "Set_ModifyUserInfo", false, false);
        }
        catch (Exception ex)
        {
            LogHelper.Log_Wirte("LogMachine", ex.Message, "Set_ModifyUserInfo", false, true);
            ds.Tables["str"].Rows[0]["状态信息"] = ex.Message;
        }
        return ds;
    }

    /// <summary>
    /// 券商发起-银行转证券#24701
    /// </summary>
    /// <returns></returns>
    [WebMethod(Description = "券商发起-银行转证券#24701")]
    public DataTable Get_BankToSecurities()
    {
        DataTable dt = null;
        try
        {
            FileInfo FI = new FileInfo(Server.MapPath("~/XML/券商发起-银行转证券#24701.xml"));
            dt = new DataTable();
            dt.ReadXml(FI.FullName);
        }
        catch (Exception ex)
        {
            LogHelper.Log_Wirte("LogMachine", ex.Message, "Get_BankToSecurities", false, true);
            dt = null;
        }
        return dt;
    }

    /// <summary>
    /// 券商发起-银行转证券#24701
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    [WebMethod(Description = "券商发起-银行转证券#24701")]
    public DataSet Set_BankToSecurities(DataTable dt)
    {
        ClassSend CS = new ClassSend();
        DataSet ds = GetInitDataSet();
        try
        {
            if (dt == null)
            {
                ds.Tables["str"].Rows[0]["状态信息"] = "数据表错误。";
                return ds;
            }

            Hashtable htSend = CS.Get_ByteAndString_From_DataTable(dt, false, "");
            ds.Tables["str"].Rows[0]["视觉检测字符串发"] = htSend["视觉检测字符串"].ToString();//预先放入要返回的数据表中
            byte[] btArr = (byte[])(htSend["字节流"]);
            Hashtable htReTURN = CS.BeginSend(btArr);
            ds.Tables["str"].Rows[0]["发包函数执行阶段"] = htReTURN["发包函数执行阶段"].ToString();//发包执行阶段
            ds.Tables["str"].Rows[0]["返回的日志"] = htReTURN["返回的日志"].ToString();//发包的结果
            byte[] btReturn = (byte[])htReTURN["返回的数据包"];

            Hashtable htResultToPMonkey = CS.Get_BaotouAndDataTableAndString_From_Byte(btReturn, false, "");
            ds.Tables["str"].Rows[0]["包头视觉检测字符串收"] = htResultToPMonkey["包头视觉检测字符串"].ToString();//银行返回的包头
            ds.Tables["str"].Rows[0]["包体视觉检测字符串收"] = htResultToPMonkey["包体视觉检测字符串"].ToString();//银行返回的包体
            DataTable dtResult = (DataTable)htResultToPMonkey["包体数据集"];//需要返回给程序员的数据集
            if ((dtResult.Rows[0]["ErrorNo"].ToString() != "0000"))
            {
                //如果包含错误信息
                ds.Tables["str"].Rows[0]["状态信息"] = dtResult.Rows[0]["ErrorInfo"];
            }
            else 
            {
                //如果包含银行流水号
                ds.Tables["str"].Rows[0]["状态信息"] = "ok";
            }
            dtResult.TableName = "result";
            //ds.Tables["result"] = dtResult;
            ds.Tables.Remove("result");
            ds.Tables.Add(dtResult);

            LogHelper.Log_Wirte("LogMachine", "发送包：" + (htSend["视觉检测字符串"] == null ? "Null" : htSend["视觉检测字符串"].ToString()) + "| 银行返回的包头：" + (htResultToPMonkey["包头视觉检测字符串"] == null ? "Null" : htResultToPMonkey["包头视觉检测字符串"].ToString()) + "| 银行返回的包体：" + (htResultToPMonkey["包体视觉检测字符串"] == null ? "Null" : htResultToPMonkey["包体视觉检测字符串"].ToString()), "Set_BankToSecurities", false, false);
        }
        catch (Exception ex)
        {
            LogHelper.Log_Wirte("LogMachine", ex.Message, "Set_BankToSecurities", false, true);
            ds.Tables["str"].Rows[0]["状态信息"] = ex.Message;
        }
        return ds;
    }

    /// <summary>
    /// 券商发起-银证转帐解约#26715
    /// </summary>
    /// <returns></returns>
    [WebMethod(Description = "券商发起-银证转帐解约#26715")]
    public DataTable Get_BankTraDealBroked()
    {
        DataTable dt = null;
        try
        {
            FileInfo FI = new FileInfo(Server.MapPath("~/XML/券商发起-银证转帐解约#26715.xml"));
            dt = new DataTable();
            dt.ReadXml(FI.FullName);
        }
        catch (Exception ex)
        {
            LogHelper.Log_Wirte("LogMachine", ex.Message, "Get_BankTraDealBroked", false, true);
            dt = null;
        }
        return dt;
    }

    /// <summary>
    /// 券商发起-银证转帐解约#26715
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    [WebMethod(Description = "券商发起-银证转帐解约#26715")]
    public DataSet Set_BankTraDealBroked(DataTable dt)
    {
        ClassSend CS = new ClassSend();
        DataSet ds = GetInitDataSet();
        try
        {
            if (dt == null)
            {
                ds.Tables["str"].Rows[0]["状态信息"] = "数据表错误。";
                return ds;
            }

            Hashtable htSend = CS.Get_ByteAndString_From_DataTable(dt, false, "");
            ds.Tables["str"].Rows[0]["视觉检测字符串发"] = htSend["视觉检测字符串"].ToString();//预先放入要返回的数据表中
            byte[] btArr = (byte[])(htSend["字节流"]);
            Hashtable htReTURN = CS.BeginSend(btArr);
            ds.Tables["str"].Rows[0]["发包函数执行阶段"] = htReTURN["发包函数执行阶段"].ToString();//发包执行阶段
            ds.Tables["str"].Rows[0]["返回的日志"] = htReTURN["返回的日志"].ToString();//发包的结果
            byte[] btReturn = (byte[])htReTURN["返回的数据包"];

            Hashtable htResultToPMonkey = CS.Get_BaotouAndDataTableAndString_From_Byte(btReturn, false, "");
            ds.Tables["str"].Rows[0]["包头视觉检测字符串收"] = htResultToPMonkey["包头视觉检测字符串"].ToString();//银行返回的包头
            ds.Tables["str"].Rows[0]["包体视觉检测字符串收"] = htResultToPMonkey["包体视觉检测字符串"].ToString();//银行返回的包体
            DataTable dtResult = (DataTable)htResultToPMonkey["包体数据集"];//需要返回给程序员的数据集
            if ((dtResult.Rows[0]["ErrorNo"].ToString() != "0000"))
            {
                //如果包含错误信息
                ds.Tables["str"].Rows[0]["状态信息"] = dtResult.Rows[0]["ErrorInfo"];
            }
            else 
            {
                //如果包含银行流水号
                ds.Tables["str"].Rows[0]["状态信息"] = "ok";
            }
            dtResult.TableName = "result";
            //ds.Tables["result"] = dtResult;
            ds.Tables.Remove("result");
            ds.Tables.Add(dtResult);

            LogHelper.Log_Wirte("LogMachine", "发送包：" + (htSend["视觉检测字符串"] == null ? "Null" : htSend["视觉检测字符串"].ToString()) + "| 银行返回的包头：" + (htResultToPMonkey["包头视觉检测字符串"] == null ? "Null" : htResultToPMonkey["包头视觉检测字符串"].ToString()) + "| 银行返回的包体：" + (htResultToPMonkey["包体视觉检测字符串"] == null ? "Null" : htResultToPMonkey["包体视觉检测字符串"].ToString()), "Set_BankTraDealBroked", false, false);
        }
        catch (Exception ex)
        {
            LogHelper.Log_Wirte("LogMachine", ex.Message, "Set_BankTraDealBroked", false, true);
            ds.Tables["str"].Rows[0]["状态信息"] = ex.Message;
        }
        return ds;
    }

    /// <summary>
    /// 券商发起-银证转帐签约#26714
    /// </summary>
    /// <returns></returns>
    [WebMethod(Description = "券商发起-银证转帐签约#26714")]
    public DataTable Get_BankTraDeal()
    {
        DataTable dt = null;
        try
        {
            FileInfo FI = new FileInfo(Server.MapPath("~/XML/券商发起-银证转帐签约#26714.xml"));
            dt = new DataTable();
            dt.ReadXml(FI.FullName);
        }
        catch (Exception ex)
        {
            LogHelper.Log_Wirte("LogMachine", ex.Message, "Get_BankTraDeal", false, true);
            dt = null;
        }
        return dt;
    }

    /// <summary>
    /// 券商发起-银证转帐签约#26714
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    [WebMethod(Description = "券商发起-银证转帐签约#26714")]
    public DataSet Set_BankTraDeal(DataTable dt)
    {
        ClassSend CS = new ClassSend();
        DataSet ds = GetInitDataSet();
        try
        {
            if (dt == null)
            {
                ds.Tables["str"].Rows[0]["状态信息"] = "数据表错误。";
                return ds;
            }

            Hashtable htSend = CS.Get_ByteAndString_From_DataTable(dt, false, "");
            ds.Tables["str"].Rows[0]["视觉检测字符串发"] = htSend["视觉检测字符串"].ToString();//预先放入要返回的数据表中
            byte[] btArr = (byte[])(htSend["字节流"]);
            Hashtable htReTURN = CS.BeginSend(btArr);
            ds.Tables["str"].Rows[0]["发包函数执行阶段"] = htReTURN["发包函数执行阶段"].ToString();//发包执行阶段
            ds.Tables["str"].Rows[0]["返回的日志"] = htReTURN["返回的日志"].ToString();//发包的结果
            byte[] btReturn = (byte[])htReTURN["返回的数据包"];

            Hashtable htResultToPMonkey = CS.Get_BaotouAndDataTableAndString_From_Byte(btReturn, false, "");
            ds.Tables["str"].Rows[0]["包头视觉检测字符串收"] = htResultToPMonkey["包头视觉检测字符串"].ToString();//银行返回的包头
            ds.Tables["str"].Rows[0]["包体视觉检测字符串收"] = htResultToPMonkey["包体视觉检测字符串"].ToString();//银行返回的包体
            DataTable dtResult = (DataTable)htResultToPMonkey["包体数据集"];//需要返回给程序员的数据集
            if ((dtResult.Rows[0]["ErrorNo"].ToString() != "0000"))
            {
                //如果包含错误信息
                ds.Tables["str"].Rows[0]["状态信息"] = dtResult.Rows[0]["ErrorInfo"];
            }
            else 
            {
                //如果包含银行流水号
                ds.Tables["str"].Rows[0]["状态信息"] = "ok";
            }
            dtResult.TableName = "result";
            //ds.Tables["result"] = dtResult;
            ds.Tables.Remove("result");
            ds.Tables.Add(dtResult);

            LogHelper.Log_Wirte("LogMachine", "发送包：" + (htSend["视觉检测字符串"] == null ? "Null" : htSend["视觉检测字符串"].ToString()) + "| 银行返回的包头：" + (htResultToPMonkey["包头视觉检测字符串"] == null ? "Null" : htResultToPMonkey["包头视觉检测字符串"].ToString()) + "| 银行返回的包体：" + (htResultToPMonkey["包体视觉检测字符串"] == null ? "Null" : htResultToPMonkey["包体视觉检测字符串"].ToString()), "Set_BankTraDeal", false, false);
        }
        catch (Exception ex)
        {
            LogHelper.Log_Wirte("LogMachine", ex.Message, "Set_BankTraDeal", false, true);
            ds.Tables["str"].Rows[0]["状态信息"] = ex.Message;
        }
        return ds;
    }

    /// <summary>
    /// 券商发起-银证转账对总账#24708
    /// </summary>
    /// <returns></returns>
    [WebMethod(Description = "券商发起-银证转账对总账#24708")]
    public DataTable Get_BankTraToMain()
    {
        DataTable dt = null;
        try
        {
            FileInfo FI = new FileInfo(Server.MapPath("~/XML/券商发起-银证转账对总账#24708.xml"));
            dt = new DataTable();
            dt.ReadXml(FI.FullName);
        }
        catch (Exception ex)
        {
            LogHelper.Log_Wirte("LogMachine", ex.Message, "Get_BankTraToMain", false, true);
            dt = null;
        }
        return dt;
    }

    /// <summary>
    /// 券商发起-银证转账对总账#24708
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    [WebMethod(Description = "券商发起-银证转账对总账#24708")]
    public DataSet Set_BankTraToMain(DataTable dt)
    {
        ClassSend CS = new ClassSend();
        DataSet ds = GetInitDataSet();
        try
        {
            if (dt == null)
            {
                ds.Tables["str"].Rows[0]["状态信息"] = "数据表错误。";
                return ds;
            }

            Hashtable htSend = CS.Get_ByteAndString_From_DataTable(dt, false, "");
            ds.Tables["str"].Rows[0]["视觉检测字符串发"] = htSend["视觉检测字符串"].ToString();//预先放入要返回的数据表中
            byte[] btArr = (byte[])(htSend["字节流"]);
            Hashtable htReTURN = CS.BeginSend(btArr);
            ds.Tables["str"].Rows[0]["发包函数执行阶段"] = htReTURN["发包函数执行阶段"].ToString();//发包执行阶段
            ds.Tables["str"].Rows[0]["返回的日志"] = htReTURN["返回的日志"].ToString();//发包的结果
            byte[] btReturn = (byte[])htReTURN["返回的数据包"];

            Hashtable htResultToPMonkey = CS.Get_BaotouAndDataTableAndString_From_Byte(btReturn, false, "");
            ds.Tables["str"].Rows[0]["包头视觉检测字符串收"] = htResultToPMonkey["包头视觉检测字符串"].ToString();//银行返回的包头
            ds.Tables["str"].Rows[0]["包体视觉检测字符串收"] = htResultToPMonkey["包体视觉检测字符串"].ToString();//银行返回的包体
            DataTable dtResult = (DataTable)htResultToPMonkey["包体数据集"];//需要返回给程序员的数据集
            if ((dtResult.Rows[0]["ErrorNo"].ToString() != "0000"))
            {
                //如果包含错误信息
                ds.Tables["str"].Rows[0]["状态信息"] = dtResult.Rows[0]["ErrorInfo"];
            }
            else 
            {
                //如果包含银行流水号
                ds.Tables["str"].Rows[0]["状态信息"] = "ok";
            }
            dtResult.TableName = "result";
            //ds.Tables["result"] = dtResult;
            ds.Tables.Remove("result");
            ds.Tables.Add(dtResult);

            LogHelper.Log_Wirte("LogMachine", "发送包：" + (htSend["视觉检测字符串"] == null ? "Null" : htSend["视觉检测字符串"].ToString()) + "| 银行返回的包头：" + (htResultToPMonkey["包头视觉检测字符串"] == null ? "Null" : htResultToPMonkey["包头视觉检测字符串"].ToString()) + "| 银行返回的包体：" + (htResultToPMonkey["包体视觉检测字符串"] == null ? "Null" : htResultToPMonkey["包体视觉检测字符串"].ToString()), "Set_BankTraToMain", false, false);
        }
        catch (Exception ex)
        {
            LogHelper.Log_Wirte("LogMachine", ex.Message, "Set_BankTraToMain", false, true);
            ds.Tables["str"].Rows[0]["状态信息"] = ex.Message;
        }
        return ds;
    }

    /// <summary>
    /// 券商发起-银证转账证券签到#24901
    /// </summary>
    /// <returns></returns>
    [WebMethod(Description = "券商发起-银证转账证券签到#24901")]
    public DataTable Get_BankTraSignIn()
    {
        DataTable dt = null;
        try
        {
            FileInfo FI = new FileInfo(Server.MapPath("~/XML/券商发起-银证转账证券签到#24901.xml"));
            dt = new DataTable();
            dt.ReadXml(FI.FullName);
        }
        catch (Exception ex)
        {
            LogHelper.Log_Wirte("LogMachine", ex.Message, "Get_BankTraSignIn", false, true);
            dt = null;
        }
        return dt;
    }

    /// <summary>
    /// 券商发起-银证转账证券签到#24901
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    [WebMethod(Description = "券商发起-银证转账证券签到#24901")]
    public DataSet Set_BankTraSignIn(DataTable dt)
    {
        ClassSend CS = new ClassSend();
        DataSet ds = GetInitDataSet();
        try
        {
            if (dt == null)
            {
                ds.Tables["str"].Rows[0]["状态信息"] = "数据表错误。";
                return ds;
            }

            Hashtable htSend = CS.Get_ByteAndString_From_DataTable(dt, false, "");
            ds.Tables["str"].Rows[0]["视觉检测字符串发"] = htSend["视觉检测字符串"].ToString();//预先放入要返回的数据表中
            byte[] btArr = (byte[])(htSend["字节流"]);
            Hashtable htReTURN = CS.BeginSend(btArr);
            ds.Tables["str"].Rows[0]["发包函数执行阶段"] = htReTURN["发包函数执行阶段"].ToString();//发包执行阶段
            ds.Tables["str"].Rows[0]["返回的日志"] = htReTURN["返回的日志"].ToString();//发包的结果
            byte[] btReturn = (byte[])htReTURN["返回的数据包"];

            Hashtable htResultToPMonkey = CS.Get_BaotouAndDataTableAndString_From_Byte(btReturn, false, "");
            ds.Tables["str"].Rows[0]["包头视觉检测字符串收"] = htResultToPMonkey["包头视觉检测字符串"].ToString();//银行返回的包头
            ds.Tables["str"].Rows[0]["包体视觉检测字符串收"] = htResultToPMonkey["包体视觉检测字符串"].ToString();//银行返回的包体
            DataTable dtResult = (DataTable)htResultToPMonkey["包体数据集"];//需要返回给程序员的数据集
            if ((dtResult.Rows[0]["ErrorNo"].ToString() != "0000"))
            {
                //如果包含错误信息
                ds.Tables["str"].Rows[0]["状态信息"] = dtResult.Rows[0]["ErrorInfo"];
            }
            else 
            {
                //如果包含银行流水号
                ds.Tables["str"].Rows[0]["状态信息"] = "ok";
            }
            dtResult.TableName = "result";
            //ds.Tables["result"] = dtResult;
            ds.Tables.Remove("result");
            ds.Tables.Add(dtResult);

            LogHelper.Log_Wirte("LogMachine", "发送包：" + (htSend["视觉检测字符串"] == null ? "Null" : htSend["视觉检测字符串"].ToString()) + "| 银行返回的包头：" + (htResultToPMonkey["包头视觉检测字符串"] == null ? "Null" : htResultToPMonkey["包头视觉检测字符串"].ToString()) + "| 银行返回的包体：" + (htResultToPMonkey["包体视觉检测字符串"] == null ? "Null" : htResultToPMonkey["包体视觉检测字符串"].ToString()), "Set_BankTraSignIn", false, false);
        }
        catch (Exception ex)
        {
            LogHelper.Log_Wirte("LogMachine", ex.Message, "Set_BankTraSignIn", false, true);
            ds.Tables["str"].Rows[0]["状态信息"] = ex.Message;
        }
        return ds;
    }

    /// <summary>
    /// 券商发起-银证转账证券签退#24902
    /// </summary>
    /// <returns></returns>
    [WebMethod(Description = "券商发起-银证转账证券签退#24902")]
    public DataTable Get_BankTraSignOut()
    {
        DataTable dt = null;
        try
        {
            FileInfo FI = new FileInfo(Server.MapPath("~/XML/券商发起-银证转账证券签退#24902.xml"));
            dt = new DataTable();
            dt.ReadXml(FI.FullName);
        }
        catch (Exception ex)
        {
            LogHelper.Log_Wirte("LogMachine", ex.Message, "Get_BankTraSignOut", false, true);
            dt = null;
        }
        return dt;
    }

    /// <summary>
    /// 券商发起-银证转账证券签退#24902
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    [WebMethod(Description = "券商发起-银证转账证券签退#24902")]
    public DataSet Set_BankTraSignOut(DataTable dt)
    {
        ClassSend CS = new ClassSend();
        DataSet ds = GetInitDataSet();
        try
        {
            if (dt == null)
            {
                ds.Tables["str"].Rows[0]["状态信息"] = "数据表错误。";
                return ds;
            }

            Hashtable htSend = CS.Get_ByteAndString_From_DataTable(dt, false, "");
            ds.Tables["str"].Rows[0]["视觉检测字符串发"] = htSend["视觉检测字符串"].ToString();//预先放入要返回的数据表中
            byte[] btArr = (byte[])(htSend["字节流"]);
            Hashtable htReTURN = CS.BeginSend(btArr);
            ds.Tables["str"].Rows[0]["发包函数执行阶段"] = htReTURN["发包函数执行阶段"].ToString();//发包执行阶段
            ds.Tables["str"].Rows[0]["返回的日志"] = htReTURN["返回的日志"].ToString();//发包的结果
            byte[] btReturn = (byte[])htReTURN["返回的数据包"];

            Hashtable htResultToPMonkey = CS.Get_BaotouAndDataTableAndString_From_Byte(btReturn, false, "");
            ds.Tables["str"].Rows[0]["包头视觉检测字符串收"] = htResultToPMonkey["包头视觉检测字符串"].ToString();//银行返回的包头
            ds.Tables["str"].Rows[0]["包体视觉检测字符串收"] = htResultToPMonkey["包体视觉检测字符串"].ToString();//银行返回的包体
            DataTable dtResult = (DataTable)htResultToPMonkey["包体数据集"];//需要返回给程序员的数据集
            if ((dtResult.Rows[0]["ErrorNo"].ToString() != "0000"))
            {
                //如果包含错误信息
                ds.Tables["str"].Rows[0]["状态信息"] = dtResult.Rows[0]["ErrorInfo"];
            }
            else 
            {
                //如果包含银行流水号
                ds.Tables["str"].Rows[0]["状态信息"] = "ok";
            }
            dtResult.TableName = "result";
            //ds.Tables["result"] = dtResult;
            ds.Tables.Remove("result");
            ds.Tables.Add(dtResult);

            LogHelper.Log_Wirte("LogMachine", "发送包：" + (htSend["视觉检测字符串"] == null ? "Null" : htSend["视觉检测字符串"].ToString()) + "| 银行返回的包头：" + (htResultToPMonkey["包头视觉检测字符串"] == null ? "Null" : htResultToPMonkey["包头视觉检测字符串"].ToString()) + "| 银行返回的包体：" + (htResultToPMonkey["包体视觉检测字符串"] == null ? "Null" : htResultToPMonkey["包体视觉检测字符串"].ToString()), "Set_BankTraSignOut", false, false);
        }
        catch (Exception ex)
        {
            LogHelper.Log_Wirte("LogMachine", ex.Message, "Set_BankTraSignOut", false, true);
            ds.Tables["str"].Rows[0]["状态信息"] = ex.Message;
        }
        return ds;
    }

    /// <summary>
    /// 券商发起-证券发送文件#24903
    /// </summary>
    /// <returns></returns>
    [WebMethod(Description = "券商发起-证券发送文件#24903")]
    public DataTable Get_SecuritiesSendFile()
    {
        DataTable dt = null;
        try
        {
            FileInfo FI = new FileInfo(Server.MapPath("~/XML/券商发起-证券发送文件#24903.xml"));
            dt = new DataTable();
            dt.ReadXml(FI.FullName);
        }
        catch (Exception ex)
        {
            LogHelper.Log_Wirte("LogMachine", ex.Message, "Get_SecuritiesSendFile", false, true);
            dt = null;
        }
        return dt;
    }

    /// <summary>
    /// 券商发起-证券发送文件#24903
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    [WebMethod(Description = "券商发起-证券发送文件#24903")]
    public DataSet Set_SecuritiesSendFile(DataTable dt)
    {
        ClassSend CS = new ClassSend();
        DataSet ds = GetInitDataSet();
        try
        {
            if (dt == null)
            {
                ds.Tables["str"].Rows[0]["状态信息"] = "数据表错误。";
                return ds;
            }

            Hashtable htSend = CS.Get_ByteAndString_From_DataTable(dt, false, "");
            ds.Tables["str"].Rows[0]["视觉检测字符串发"] = htSend["视觉检测字符串"].ToString();//预先放入要返回的数据表中
            byte[] btArr = (byte[])(htSend["字节流"]);
            Hashtable htReTURN = CS.BeginSend(btArr);
            ds.Tables["str"].Rows[0]["发包函数执行阶段"] = htReTURN["发包函数执行阶段"].ToString();//发包执行阶段
            ds.Tables["str"].Rows[0]["返回的日志"] = htReTURN["返回的日志"].ToString();//发包的结果
            byte[] btReturn = (byte[])htReTURN["返回的数据包"];

            Hashtable htResultToPMonkey = CS.Get_BaotouAndDataTableAndString_From_Byte(btReturn, false, "");
            ds.Tables["str"].Rows[0]["包头视觉检测字符串收"] = htResultToPMonkey["包头视觉检测字符串"].ToString();//银行返回的包头
            ds.Tables["str"].Rows[0]["包体视觉检测字符串收"] = htResultToPMonkey["包体视觉检测字符串"].ToString();//银行返回的包体
            DataTable dtResult = (DataTable)htResultToPMonkey["包体数据集"];//需要返回给程序员的数据集
            if ((dtResult.Rows[0]["ErrorNo"].ToString() != "0000"))
            {
                //如果包含错误信息
                ds.Tables["str"].Rows[0]["状态信息"] = dtResult.Rows[0]["ErrorInfo"];
            }
            else 
            {
                //如果包含银行流水号
                ds.Tables["str"].Rows[0]["状态信息"] = "ok";
            }
            dtResult.TableName = "result";
            //ds.Tables["result"] = dtResult;
            ds.Tables.Remove("result");
            ds.Tables.Add(dtResult);

            LogHelper.Log_Wirte("LogMachine", "发送包：" + (htSend["视觉检测字符串"] == null ? "Null" : htSend["视觉检测字符串"].ToString()) + "| 银行返回的包头：" + (htResultToPMonkey["包头视觉检测字符串"] == null ? "Null" : htResultToPMonkey["包头视觉检测字符串"].ToString()) + "| 银行返回的包体：" + (htResultToPMonkey["包体视觉检测字符串"] == null ? "Null" : htResultToPMonkey["包体视觉检测字符串"].ToString()), "Set_SecuritiesSendFile", false, false);
        }
        catch (Exception ex)
        {
            LogHelper.Log_Wirte("LogMachine", ex.Message, "Set_SecuritiesSendFile", false, true);
            ds.Tables["str"].Rows[0]["状态信息"] = ex.Message;
        }
        return ds;
    }

    /// <summary>
    /// 券商发起-证券发送文件完毕通知#24909
    /// </summary>
    /// <returns></returns>
    [WebMethod(Description = "券商发起-证券发送文件完毕通知#24909")]
    public DataTable Get_SecuritiesSendFileEnd()
    {
        DataTable dt = null;
        try
        {
            FileInfo FI = new FileInfo(Server.MapPath("~/XML/券商发起-证券发送文件完毕通知#24909.xml"));
            dt = new DataTable();
            dt.ReadXml(FI.FullName);
        }
        catch (Exception ex)
        {
            LogHelper.Log_Wirte("LogMachine", ex.Message, "Get_SecuritiesSendFileEnd", false, true);
            dt = null;
        }
        return dt;
    }

    /// <summary>
    /// 券商发起-证券发送文件完毕通知#24909
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    [WebMethod(Description = "券商发起-证券发送文件完毕通知#24909")]
    public DataSet Set_SecuritiesSendFileEnd(DataTable dt)
    {
        ClassSend CS = new ClassSend();
        DataSet ds = GetInitDataSet();
        try
        {
            if (dt == null)
            {
                ds.Tables["str"].Rows[0]["状态信息"] = "数据表错误。";
                return ds;
            }

            Hashtable htSend = CS.Get_ByteAndString_From_DataTable(dt, false, "");
            ds.Tables["str"].Rows[0]["视觉检测字符串发"] = htSend["视觉检测字符串"].ToString();//预先放入要返回的数据表中
            byte[] btArr = (byte[])(htSend["字节流"]);
            Hashtable htReTURN = CS.BeginSend(btArr);
            ds.Tables["str"].Rows[0]["发包函数执行阶段"] = htReTURN["发包函数执行阶段"].ToString();//发包执行阶段
            ds.Tables["str"].Rows[0]["返回的日志"] = htReTURN["返回的日志"].ToString();//发包的结果
            byte[] btReturn = (byte[])htReTURN["返回的数据包"];

            Hashtable htResultToPMonkey = CS.Get_BaotouAndDataTableAndString_From_Byte(btReturn, false, "");
            ds.Tables["str"].Rows[0]["包头视觉检测字符串收"] = htResultToPMonkey["包头视觉检测字符串"].ToString();//银行返回的包头
            ds.Tables["str"].Rows[0]["包体视觉检测字符串收"] = htResultToPMonkey["包体视觉检测字符串"].ToString();//银行返回的包体
            DataTable dtResult = (DataTable)htResultToPMonkey["包体数据集"];//需要返回给程序员的数据集
            if ((dtResult.Rows[0]["ErrorNo"].ToString() != "0000"))
            {
                //如果包含错误信息
                ds.Tables["str"].Rows[0]["状态信息"] = dtResult.Rows[0]["ErrorInfo"];
            }
            else 
            {
                //如果包含银行流水号
                ds.Tables["str"].Rows[0]["状态信息"] = "ok";
            }
            dtResult.TableName = "result";
            //ds.Tables["result"] = dtResult;
            ds.Tables.Remove("result");
            ds.Tables.Add(dtResult);

            LogHelper.Log_Wirte("LogMachine", "发送包：" + (htSend["视觉检测字符串"] == null ? "Null" : htSend["视觉检测字符串"].ToString()) + "| 银行返回的包头：" + (htResultToPMonkey["包头视觉检测字符串"] == null ? "Null" : htResultToPMonkey["包头视觉检测字符串"].ToString()) + "| 银行返回的包体：" + (htResultToPMonkey["包体视觉检测字符串"] == null ? "Null" : htResultToPMonkey["包体视觉检测字符串"].ToString()), "Set_SecuritiesSendFileEnd", false, false);
        }
        catch (Exception ex)
        {
            LogHelper.Log_Wirte("LogMachine", ex.Message, "Set_SecuritiesSendFileEnd", false, true);
            ds.Tables["str"].Rows[0]["状态信息"] = ex.Message;
        }
        return ds;
    }

    /// <summary>
    /// 券商发起-证券接收文件#24904
    /// </summary>
    /// <returns></returns>
    [WebMethod(Description = "券商发起-证券接收文件#24904")]
    public DataTable Get_SecuritiesGetFile()
    {
        DataTable dt = null;
        try
        {
            FileInfo FI = new FileInfo(Server.MapPath("~/XML/券商发起-证券接收文件#24904.xml"));
            dt = new DataTable();
            dt.ReadXml(FI.FullName);
        }
        catch (Exception ex)
        {
            LogHelper.Log_Wirte("LogMachine", ex.Message, "Get_SecuritiesGetFile", false, true);
            dt = null;
        }
        return dt;
    }

    /// <summary>
    /// 券商发起-证券接收文件#24904
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    [WebMethod(Description = "券商发起-证券接收文件#24904")]
    public DataSet Set_SecuritiesGetFile(DataTable dt)
    {
        ClassSend CS = new ClassSend();
        DataSet ds = GetInitDataSet();
        try
        {
            if (dt == null)
            {
                ds.Tables["str"].Rows[0]["状态信息"] = "数据表错误。";
                return ds;
            }

            Hashtable htSend = CS.Get_ByteAndString_From_DataTable(dt, false, "");
            ds.Tables["str"].Rows[0]["视觉检测字符串发"] = htSend["视觉检测字符串"].ToString();//预先放入要返回的数据表中
            byte[] btArr = (byte[])(htSend["字节流"]);
            Hashtable htReTURN = CS.BeginSend(btArr);
            ds.Tables["str"].Rows[0]["发包函数执行阶段"] = htReTURN["发包函数执行阶段"].ToString();//发包执行阶段
            ds.Tables["str"].Rows[0]["返回的日志"] = htReTURN["返回的日志"].ToString();//发包的结果
            byte[] btReturn = (byte[])htReTURN["返回的数据包"];

            Hashtable htResultToPMonkey = CS.Get_BaotouAndDataTableAndString_From_Byte(btReturn, false, "");
            ds.Tables["str"].Rows[0]["包头视觉检测字符串收"] = htResultToPMonkey["包头视觉检测字符串"].ToString();//银行返回的包头
            ds.Tables["str"].Rows[0]["包体视觉检测字符串收"] = htResultToPMonkey["包体视觉检测字符串"].ToString();//银行返回的包体
            DataTable dtResult = (DataTable)htResultToPMonkey["包体数据集"];//需要返回给程序员的数据集
            if ((dtResult.Rows[0]["ErrorNo"].ToString() != "0000"))
            {
                //如果包含错误信息
                ds.Tables["str"].Rows[0]["状态信息"] = dtResult.Rows[0]["ErrorInfo"];
            }
            else 
            {
                //如果包含银行流水号
                ds.Tables["str"].Rows[0]["状态信息"] = "ok";
            }
            dtResult.TableName = "result";
            //ds.Tables["result"] = dtResult;
            ds.Tables.Remove("result");
            ds.Tables.Add(dtResult);

            LogHelper.Log_Wirte("LogMachine", "发送包：" + (htSend["视觉检测字符串"] == null ? "Null" : htSend["视觉检测字符串"].ToString()) + "| 银行返回的包头：" + (htResultToPMonkey["包头视觉检测字符串"] == null ? "Null" : htResultToPMonkey["包头视觉检测字符串"].ToString()) + "| 银行返回的包体：" + (htResultToPMonkey["包体视觉检测字符串"] == null ? "Null" : htResultToPMonkey["包体视觉检测字符串"].ToString()), "Set_SecuritiesGetFile", false, false);
        }
        catch (Exception ex)
        {
            LogHelper.Log_Wirte("LogMachine", ex.Message, "Set_SecuritiesGetFile", false, true);
            ds.Tables["str"].Rows[0]["状态信息"] = ex.Message;
        }
        return ds;
    }

    /// <summary>
    /// 券商发起-证券文件生成完毕通知#24908
    /// </summary>
    /// <returns></returns>
    [WebMethod(Description = "券商发起-证券文件生成完毕通知#24908")]
    public DataTable Get_SecuritiesGetFileEnd()
    {
        DataTable dt = null;
        try
        {
            FileInfo FI = new FileInfo(Server.MapPath("~/XML/券商发起-证券文件生成完毕通知#24908.xml"));
            dt = new DataTable();
            dt.ReadXml(FI.FullName);
        }
        catch (Exception ex)
        {
            LogHelper.Log_Wirte("LogMachine", ex.Message, "Get_SecuritiesGetFileEnd", false, true);
            dt = null;
        }
        return dt;
    }

    /// <summary>
    /// 券商发起-证券文件生成完毕通知#24908
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    [WebMethod(Description = "券商发起-证券文件生成完毕通知#24908")]
    public DataSet Set_SecuritiesGetFileEnd(DataTable dt)
    {
        ClassSend CS = new ClassSend();
        DataSet ds = GetInitDataSet();
        try
        {
            if (dt == null)
            {
                ds.Tables["str"].Rows[0]["状态信息"] = "数据表错误。";
                return ds;
            }

            Hashtable htSend = CS.Get_ByteAndString_From_DataTable(dt, false, "");
            ds.Tables["str"].Rows[0]["视觉检测字符串发"] = htSend["视觉检测字符串"].ToString();//预先放入要返回的数据表中
            byte[] btArr = (byte[])(htSend["字节流"]);
            Hashtable htReTURN = CS.BeginSend(btArr);
            ds.Tables["str"].Rows[0]["发包函数执行阶段"] = htReTURN["发包函数执行阶段"].ToString();//发包执行阶段
            ds.Tables["str"].Rows[0]["返回的日志"] = htReTURN["返回的日志"].ToString();//发包的结果
            byte[] btReturn = (byte[])htReTURN["返回的数据包"];

            Hashtable htResultToPMonkey = CS.Get_BaotouAndDataTableAndString_From_Byte(btReturn, false, "");
            ds.Tables["str"].Rows[0]["包头视觉检测字符串收"] = htResultToPMonkey["包头视觉检测字符串"].ToString();//银行返回的包头
            ds.Tables["str"].Rows[0]["包体视觉检测字符串收"] = htResultToPMonkey["包体视觉检测字符串"].ToString();//银行返回的包体
            DataTable dtResult = (DataTable)htResultToPMonkey["包体数据集"];//需要返回给程序员的数据集
            if ((dtResult.Rows[0]["ErrorNo"].ToString() != "0000"))
            {
                //如果包含错误信息
                ds.Tables["str"].Rows[0]["状态信息"] = dtResult.Rows[0]["ErrorInfo"];
            }
            else 
            {
                //如果包含银行流水号
                ds.Tables["str"].Rows[0]["状态信息"] = "ok";
            }
            dtResult.TableName = "result";
            //ds.Tables["result"] = dtResult;
            ds.Tables.Remove("result");
            ds.Tables.Add(dtResult);

            LogHelper.Log_Wirte("LogMachine", "发送包：" + (htSend["视觉检测字符串"] == null ? "Null" : htSend["视觉检测字符串"].ToString()) + "| 银行返回的包头：" + (htResultToPMonkey["包头视觉检测字符串"] == null ? "Null" : htResultToPMonkey["包头视觉检测字符串"].ToString()) + "| 银行返回的包体：" + (htResultToPMonkey["包体视觉检测字符串"] == null ? "Null" : htResultToPMonkey["包体视觉检测字符串"].ToString()), "Set_SecuritiesGetFileEnd", false, false);
        }
        catch (Exception ex)
        {
            LogHelper.Log_Wirte("LogMachine", ex.Message, "Set_SecuritiesGetFileEnd", false, true);
            ds.Tables["str"].Rows[0]["状态信息"] = ex.Message;
        }
        return ds;
    }

    /// <summary>
    /// 券商发起-证券转银行#24702
    /// </summary>
    /// <returns></returns>
    [WebMethod(Description = "券商发起-证券转银行#24702")]
    public DataTable Get_SecuritiesToBank()
    {
        DataTable dt = null;
        try
        {
            FileInfo FI = new FileInfo(Server.MapPath("~/XML/券商发起-证券转银行#24702.xml"));
            dt = new DataTable();
            dt.ReadXml(FI.FullName);
        }
        catch (Exception ex)
        {
            LogHelper.Log_Wirte("LogMachine", ex.Message, "Get_SecuritiesToBank", false, true);
            dt = null;
        }
        return dt;
    }

    /// <summary>
    /// 券商发起-证券转银行#24702
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    [WebMethod(Description = "券商发起-证券转银行#24702")]
    public DataSet Set_SecuritiesToBank(DataTable dt)
    {
        ClassSend CS = new ClassSend();
        DataSet ds = GetInitDataSet();
        try
        {
            if (dt == null)
            {
                ds.Tables["str"].Rows[0]["状态信息"] = "数据表错误。";
                return ds;
            }

            Hashtable htSend = CS.Get_ByteAndString_From_DataTable(dt, false, "");
            ds.Tables["str"].Rows[0]["视觉检测字符串发"] = htSend["视觉检测字符串"].ToString();//预先放入要返回的数据表中
            byte[] btArr = (byte[])(htSend["字节流"]);
            Hashtable htReTURN = CS.BeginSend(btArr);
            ds.Tables["str"].Rows[0]["发包函数执行阶段"] = htReTURN["发包函数执行阶段"].ToString();//发包执行阶段
            ds.Tables["str"].Rows[0]["返回的日志"] = htReTURN["返回的日志"].ToString();//发包的结果
            byte[] btReturn = (byte[])htReTURN["返回的数据包"];

            Hashtable htResultToPMonkey = CS.Get_BaotouAndDataTableAndString_From_Byte(btReturn, false, "");
            ds.Tables["str"].Rows[0]["包头视觉检测字符串收"] = htResultToPMonkey["包头视觉检测字符串"].ToString();//银行返回的包头
            ds.Tables["str"].Rows[0]["包体视觉检测字符串收"] = htResultToPMonkey["包体视觉检测字符串"].ToString();//银行返回的包体
            DataTable dtResult = (DataTable)htResultToPMonkey["包体数据集"];//需要返回给程序员的数据集
            if ((dtResult.Rows[0]["ErrorNo"].ToString() != "0000"))
            {
                //如果包含错误信息
                ds.Tables["str"].Rows[0]["状态信息"] = dtResult.Rows[0]["ErrorInfo"];
            }
            else 
            {
                //如果包含银行流水号
                ds.Tables["str"].Rows[0]["状态信息"] = "ok";
            }
            dtResult.TableName = "result";
            //ds.Tables["result"] = dtResult;
            ds.Tables.Remove("result");
            ds.Tables.Add(dtResult);

            LogHelper.Log_Wirte("LogMachine", "发送包：" + (htSend["视觉检测字符串"] == null ? "Null" : htSend["视觉检测字符串"].ToString()) + "| 银行返回的包头：" + (htResultToPMonkey["包头视觉检测字符串"] == null ? "Null" : htResultToPMonkey["包头视觉检测字符串"].ToString()) + "| 银行返回的包体：" + (htResultToPMonkey["包体视觉检测字符串"] == null ? "Null" : htResultToPMonkey["包体视觉检测字符串"].ToString()), "Set_SecuritiesToBank", false, false);
        }
        catch (Exception ex)
        {
            LogHelper.Log_Wirte("LogMachine", ex.Message, "Set_SecuritiesToBank", false, true);
            ds.Tables["str"].Rows[0]["状态信息"] = ex.Message;
        }
        return ds;
    }

    /// <summary>
    /// 券商发起-软加密密钥交换#26700
    /// </summary>
    /// <returns></returns>
    [WebMethod(Description = "券商发起-软加密密钥交换#26700")]
    public DataTable Get_SecuritiesKey()
    {
        DataTable dt = null;
        try
        {
            FileInfo FI = new FileInfo(Server.MapPath("~/XML/券商发起-软加密密钥交换#26700.xml"));
            dt = new DataTable();
            dt.ReadXml(FI.FullName);
        }
        catch (Exception ex)
        {
            LogHelper.Log_Wirte("LogMachine", ex.Message, "Get_SecuritiesKey", false, true);
            dt = null;
        }
        return dt;
    }

    /// <summary>
    ///券商发起-软加密密钥交换#26700
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    [WebMethod(Description = "券商发起-软加密密钥交换#26700")]
    public DataSet Set_SecuritiesKey(DataTable dt)
    {
        ClassSend CS = new ClassSend();
        DataSet ds = GetInitDataSet();
        try
        {
            if (dt == null)
            {
                ds.Tables["str"].Rows[0]["状态信息"] = "数据表错误。";
                return ds;
            }

            Hashtable htSend = CS.Get_ByteAndString_From_DataTable(dt, false, "");
            ds.Tables["str"].Rows[0]["视觉检测字符串发"] = htSend["视觉检测字符串"].ToString();//预先放入要返回的数据表中
            byte[] btArr = (byte[])(htSend["字节流"]);
            Hashtable htReTURN = CS.BeginSend(btArr);
            ds.Tables["str"].Rows[0]["发包函数执行阶段"] = htReTURN["发包函数执行阶段"].ToString();//发包执行阶段
            ds.Tables["str"].Rows[0]["返回的日志"] = htReTURN["返回的日志"].ToString();//发包的结果
            byte[] btReturn = (byte[])htReTURN["返回的数据包"];

            Hashtable htResultToPMonkey = CS.Get_BaotouAndDataTableAndString_From_Byte(btReturn, false, "");
            ds.Tables["str"].Rows[0]["包头视觉检测字符串收"] = htResultToPMonkey["包头视觉检测字符串"].ToString();//银行返回的包头
            ds.Tables["str"].Rows[0]["包体视觉检测字符串收"] = htResultToPMonkey["包体视觉检测字符串"].ToString();//银行返回的包体
            DataTable dtResult = (DataTable)htResultToPMonkey["包体数据集"];//需要返回给程序员的数据集
            if ((dtResult.Rows[0]["ErrorNo"].ToString() != "0000"))
            {
                //如果包含错误信息
                ds.Tables["str"].Rows[0]["状态信息"] = dtResult.Rows[0]["ErrorInfo"];
            }
            else
            {
                //如果包含银行流水号
                ds.Tables["str"].Rows[0]["状态信息"] = "ok";
            }
            dtResult.TableName = "result";
            //ds.Tables["result"] = dtResult;
            ds.Tables.Remove("result");
            ds.Tables.Add(dtResult);

            LogHelper.Log_Wirte("LogMachine", "发送包：" + (htSend["视觉检测字符串"] == null ? "Null" : htSend["视觉检测字符串"].ToString()) + "| 银行返回的包头：" + (htResultToPMonkey["包头视觉检测字符串"] == null ? "Null" : htResultToPMonkey["包头视觉检测字符串"].ToString()) + "| 银行返回的包体：" + (htResultToPMonkey["包体视觉检测字符串"] == null ? "Null" : htResultToPMonkey["包体视觉检测字符串"].ToString()), "Set_SecuritiesKey", false, false);
        }
        catch (Exception ex)
        {
            LogHelper.Log_Wirte("LogMachine", ex.Message, "Set_SecuritiesKey", false, true);
            ds.Tables["str"].Rows[0]["状态信息"] = ex.Message;
        }
        return ds;
    }

    [WebMethod(Description = "把数据发送给银行服务器")]
    public DataSet Reincarnation(DataSet dsCome)
    {
        ClassSend CS = new ClassSend();
        DataSet ds = GetInitDataSet();
        try
        {
            DataTable dt = dsCome.Tables[0];
            if (dt == null)
            {
                ds.Tables["str"].Rows[0]["状态信息"] = "数据表错误。";
                return ds;
            }

            Hashtable htSend = CS.Get_ByteAndString_From_DataTable(dt, false, "");
            ds.Tables["str"].Rows[0]["视觉检测字符串发"] = htSend["视觉检测字符串"].ToString();//预先放入要返回的数据表中
            byte[] btArr = (byte[])(htSend["字节流"]);
            Hashtable htReTURN = CS.BeginSend(btArr);
            ds.Tables["str"].Rows[0]["发包函数执行阶段"] = htReTURN["发包函数执行阶段"].ToString();//发包执行阶段
            ds.Tables["str"].Rows[0]["返回的日志"] = htReTURN["返回的日志"].ToString();//发包的结果
            byte[] btReturn = (byte[])htReTURN["返回的数据包"];

            Hashtable htResultToPMonkey = CS.Get_BaotouAndDataTableAndString_From_Byte(btReturn, false, "");
            ds.Tables["str"].Rows[0]["包头视觉检测字符串收"] = htResultToPMonkey["包头视觉检测字符串"].ToString();//银行返回的包头
            ds.Tables["str"].Rows[0]["包体视觉检测字符串收"] = htResultToPMonkey["包体视觉检测字符串"].ToString();//银行返回的包体
            DataTable dtResult = (DataTable)htResultToPMonkey["包体数据集"];//需要返回给程序员的数据集
            if ((dtResult.Rows[0]["ErrorNo"].ToString() != "0000"))
            {
                //如果包含错误信息
                ds.Tables["str"].Rows[0]["状态信息"] = dtResult.Rows[0]["ErrorInfo"];
            }
            else
            {
                //如果包含银行流水号
                ds.Tables["str"].Rows[0]["状态信息"] = "ok";
            }
            dtResult.TableName = "result";
            //ds.Tables["result"] = dtResult;
            ds.Tables.Remove("result");
            ds.Tables.Add(dtResult);

            //LogHelper.Log_Wirte("LogMachine", "发送包：" + (htSend["视觉检测字符串"] == null ? "Null" : htSend["视觉检测字符串"].ToString()) + "| 银行返回的包头：" + (htResultToPMonkey["包头视觉检测字符串"] == null ? "Null" : htResultToPMonkey["包头视觉检测字符串"].ToString()) + "| 银行返回的包体：" + (htResultToPMonkey["包体视觉检测字符串"] == null ? "Null" : htResultToPMonkey["包体视觉检测字符串"].ToString()), "Set_SecuritiesKey", false, false);
        }
        catch (Exception ex)
        {
            //LogHelper.Log_Wirte("LogMachine", ex.Message, "Set_SecuritiesKey", false, true);
            ds.Tables["str"].Rows[0]["状态信息"] = ex.Message;
        }
        return ds;
    }
}