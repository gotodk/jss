using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;
using System.Web.Services;

/// <summary>
/// Service 的摘要说明
/// </summary>
[WebService(Namespace = "http://www.fm8844.com")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class Service : System.Web.Services.WebService {

    /// <summary>
    /// Socket目标IP
    /// </summary>
    private string ipAddress = "";//平安银行的IP地址

    /// <summary>
    /// Socket目标端口
    /// </summary>
    private string port = "";//平安银行的端口

    public Service () {

        //如果使用设计的组件，请取消注释以下行 
        //InitializeComponent(); 
    }


    /// <summary>
    /// 获取当前PinKey、MacKey
    /// </summary>
    /// <returns></returns>
    [WebMethod(Description = "获取当前最新的Pin、Mac")]
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
        catch (Exception ex)
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
    /// 7/26 Service会统一通过这个这个入口调用该方法，然后由此方法分发调用这个接口。
    /// 后期可以考虑加入7/26与本Service的密令交换，以防止任何非7/26Service的使用者。
    /// </summary>
    /// <returns>银行返回的结果</returns>
    [WebMethod(Description = "把数据发送给银行服务器")]
    public DataSet Reincarnation(DataSet ds)
    {
        ParamHelper p = new ParamHelper();
        /*方法思路：
         * 1、首先获取从7/26传递过来的数据集（即包体）
         * 2、根据包体，得到相应的包头。
         * 3、将包头包体拼装成字符串，然后转化成byte流
         * 4、发送byte流，接收银行的反馈
         * 5、将结果发回7/26
         */
        if (ds.Tables.Count < 0)
            return null;
        DataTable dtTi = new DataTable();
        dtTi = ds.Tables[0];//1、首先获取从7/26传递过来的数据集（即包体）

        Core c = new Core();
        DataTable dtTou = new DataTable();
        string path = Server.MapPath(@"~/XML/壮哉我大平安报文头.xml");
        dtTou.ReadXml(path);//初始话包头
       
        string FuncId = dtTi.Rows[0]["FunctionId"].ToString();//接口编号
        string SerialNumber = dtTi.Rows[0]["Reserve"].ToString();//用保留域来存单号，替换报头中的流水号
        dtTou.Rows[0]["TranFunc"] = FuncId;
        dtTou.Rows[0]["ThirdLogNo"] = SerialNumber;       
        dtTi.Rows[0]["Reserve"] = "";//赋值完成后将包体保留域的流水号清空
        int baotiLength = c.GetBaotiLength(dtTi);//包体长度
        dtTou.Rows[0]["Length"] = c.GetZero(baotiLength.ToString());//2、根据包体，得到相应的包头。

        DataTable dtTiMain = dtTi.Copy();
        DataTable dtTouMain = dtTou.Copy();
        dtTiMain.TableName = "dtTi";
        dtTouMain.TableName = "dtTou";

        DataSet dsMain = new DataSet();
        dsMain.Tables.Add(dtTouMain);
        dsMain.Tables.Add(dtTiMain);
        Hashtable ht = c.UsToBank(dsMain);//3、将包头包体拼装成字符串，然后转化成byte流

        byte[] b = (byte[])ht["byteAry"];//要发给银行的数据包
        string ipAddr = p.GetIp();
        string portNum = p.GetPort();
        Hashtable htResult = c.Send(ipAddr, portNum, b);//银行反馈结果

        if (htResult["state"].ToString() == "ok")
        {
            //5、将结果发回7/26
            DataSet dsReturn = new DataSet();
            byte[] b_return = (byte[])htResult["DataByte"];//反馈流
            DataTable dtTiReturn = c.GetBaoti(b_return, 0).Copy();
            DataTable dtTouReturn = c.GetBaotou(b_return).Copy();
            dtTiReturn.TableName = "包体";
            dtTouReturn.TableName = "包头";
            dsReturn.Tables.Add(dtTiReturn);
            dsReturn.Tables.Add(dtTouReturn);
            DataTable msg = new DataTable("结果");

            msg.Columns.Add("反馈结果");
            msg.Columns.Add("详情");
            msg.Columns.Add("视觉字符串");
            msg.Rows.Add(new string[] { htResult["state"].ToString(), htResult["msg"].ToString(), htResult["DataStr"].ToString() });
            dsReturn.Tables.Add(msg);

            return dsReturn;
        }
        else
        {
            DataSet dsReturn = new DataSet();
            DataTable msg = new DataTable("结果");
            msg.Columns.Add("反馈结果");
            msg.Columns.Add("详情");
            msg.Columns.Add("视觉字符串");
            msg.Rows.Add(new string[] { htResult["state"].ToString(), htResult["msg"].ToString(), "" });
            dsReturn.Tables.Add(msg);

            return dsReturn;
        }
    }
    
}
