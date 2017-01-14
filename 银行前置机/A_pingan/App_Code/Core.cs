using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Text;

/// <summary>
/// Core 的摘要说明
/// </summary>
public class Core
{
    string ipAddress = string.Empty;
    string portNumber = string.Empty;
	public Core()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}
    public Core(string ip, string port)
    {
        ipAddress = ip;
        portNumber = port;
    }
    #region 打包
    /// <summary>
    /// 将我方数据包转换成流（用以发送给银行--仅用于测试）
    /// </summary>
    /// <param name="dt">包体</param>
    public Hashtable UsToBank(DataTable dtSource)
    {
        DataTable dt = dtSource.Copy();
        string INumber = dt.Rows[0]["FunctionId"].ToString().Trim();//接口编号
        dt.Columns.Remove("FunctionId");//移除包头中的FunctionId
        string ImpBaoti = string.Empty;//包体变量
        for (int i = 0; i < dt.Columns.Count; i++)
        {
            ImpBaoti += dt.Rows[0][i].ToString() + "&";//按照银行规则拼接包体
        }
        string ImpBaotou = string.Empty;//包头变量
        string path = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\XML\壮哉我大平安报文头.xml";
        DataTable dtTou = new DataTable();
        dtTou.ReadXml(path);
        dtTou.Rows[0]["TranFunc"] = INumber;//包头中替换接口编号,C4
        dtTou.Rows[0]["ServType"] = "01";//说明这是请求包，C2
        dtTou.Rows[0]["MacCode"] = "1234567890123456";//16为MAC码，C16
        dtTou.Rows[0]["TranDate"] = DateTime.Now.ToString("yyyyMMdd");//C8
        dtTou.Rows[0]["TranTime"] = DateTime.Now.ToString("HHmmss");//C6
        dtTou.Rows[0]["RspCode"] = "999999";//初始包发送999999，C6
        dtTou.Rows[0]["RspMsg"] = GetZero("", 42);//应答码描述，C42
        dtTou.Rows[0]["ConFlag"] = "0";//后续包标志，C1
        dtTou.Rows[0]["Length"] = GetZero(Encoding.GetEncoding("GB2312").GetBytes(ImpBaoti).Length.ToString());//包体的长度，C4
        dtTou.Rows[0]["CounterId"] = "10000";//企业操作员代码，C5
        dtTou.Rows[0]["ThirdLogNo"] = "12345678901234567890";//请求方流水号，C20
        dtTou.Rows[0]["Qydm"] = "8888";//企业代码，C4
        for (int i = 0; i < dtTou.Columns.Count; i++)
        {
            ImpBaotou += dtTou.Rows[0][i].ToString();
        }
        Hashtable ht = new Hashtable();
        ht["DataStr"] = ImpBaotou + ImpBaoti;
        ht["DataByte"] = Encoding.GetEncoding("GB2312").GetBytes(ImpBaotou + ImpBaoti);
        ht["DataTou"] = ImpBaotou;
        ht["DataTi"] = ImpBaoti;
        return ht;
    }
    /// <summary>
    /// 将我方数据包转换成流（用以发送给银行）
    /// </summary>
    /// <param name="ds">Ds包含dtTou和dtTi</param>
    /// <returns></returns>
    public Hashtable UsToBank(DataSet ds)
    {
        if (ds.Tables.Count == 2 && ds.Tables[0] != null && ds.Tables[1] != null)
        {
            DataTable dtTou = ds.Tables["dtTou"];//包头
            DataTable dtTi = ds.Tables["dtTi"];//包体
            string baoti = GetTi(dtTi);//包体字符串
            byte[] baoti_b = Encoding.GetEncoding("GB2312").GetBytes(baoti);//用来计算包体长度
            //再次验证包体长度
            if (dtTou.Rows[0]["Length"].ToString() != GetZero(baoti_b.Length.ToString()))
                dtTou.Rows[0]["Length"] = GetZero(baoti_b.Length.ToString());
            string baotou = GetTou(dtTou);//包头字符串

            string result_str = baotou + baoti;//包头包体字符串
            byte[] result_byteAry = Encoding.GetEncoding("GB2312").GetBytes(result_str);//数据流

            Hashtable ht = new Hashtable();
            ht["baotou"] = baotou;
            ht["baoti"] = baoti;
            ht["bao"] = result_str;
            ht["byteAry"] = result_byteAry;
            return ht;
        }
        else
            return null;
    }
    /// <summary>
    /// 给我包头，给你串~
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    private string GetTou(DataTable dt)
    {
        BaotouFresh(ref dt);
        string str = string.Empty;
        for (int i = 0; i < dt.Columns.Count; i++)
        {
            str += dt.Rows[0][i].ToString();
        }
        return str;
    }
    /// <summary>
    /// 给我包体，给你串~
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    private string GetTi(DataTable dtTi)
    {
        DataTable dt = dtTi.Copy();
        string str = string.Empty;
        if (dt.Columns.Contains("FunctionId"))
            dt.Columns.Remove(dt.Columns["FunctionId"]);
        for (int i = 0; i < dt.Columns.Count; i++)
        {
            str += dt.Rows[0][i].ToString() + "&";
        }
        return str;
    }

    /// <summary>
    /// 处理包头中脏数据、位数
    /// </summary>
    /// <param name="dt"></param>
    public void BaotouFresh(ref DataTable dt)
    {
        if (dt != null)
        {
            if (dt.Rows[0]["MacCode"].ToString().Length != 16)
                dt.Rows[0]["MacCode"] = "1234567890123456";//16为MAC码，C16
            if (dt.Rows[0]["RspMsg"].ToString().Length != 42)
                dt.Rows[0]["RspMsg"] = GetZero(dt.Rows[0]["RspMsg"].ToString(), 42);
            if (dt.Rows[0]["Length"].ToString().Length != 4)
                dt.Rows[0]["Length"] = GetZero(dt.Rows[0]["Length"].ToString());
            if (dt.Rows[0]["ThirdLogNo"].ToString().Length != 20)
                dt.Rows[0]["ThirdLogNo"] = GetZero(dt.Rows[0]["ThirdLogNo"].ToString(), "0", 20);
        }
    }

    /// <summary>
    /// 获取包体长度
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public int GetBaotiLength(DataTable dt)
    {
        DataTable dtbt = new DataTable();
        dtbt = dt.Copy();
        if (dtbt.Columns.Contains("FunctionId"))
            dtbt.Columns.Remove("FunctionId");
        string str = string.Empty;
        for (int i = 0; i < dtbt.Columns.Count; i++)
        {
            str += dtbt.Rows[0][i].ToString() + "&";
        }
        return Encoding.GetEncoding("GB2312").GetBytes(str).Length;
    }

    /// <summary>
    /// 发送数据包到银行
    /// </summary>
    /// <param name="ip"></param>
    /// <param name="port"></param>
    /// <param name="b"></param>
    public Hashtable Send(string ip, string port, byte[] b)
    {
        Hashtable ht = new Hashtable();
        ht["state"] = "error";
        try
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.ReceiveTimeout = 20000;
            socket.SendTimeout = 20000;
            IPEndPoint serverFullAddr = new IPEndPoint(IPAddress.Parse(ip), int.Parse(port));
            socket.Connect(serverFullAddr);
            int f = socket.Send(b);
            byte[] result = new byte[9999];
            int r = socket.Receive(result);
            ht["state"] = "ok";
            ht["msg"] = "";
            ht["DataByte"] = result;
            ht["DataStr"] = Encoding.GetEncoding("GB2312").GetString(result);
        }
        catch (Exception ex)
        {
            ht["state"] = "error";
            ht["msg"] = ex.Message;
            return ht;
        }
        return ht;
    }

    /// <summary>
    /// 补0（前置）
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public string GetZero(string str)
    {
        while (Encoding.GetEncoding("GB2312").GetBytes(str).Length < 4)
        {
            str = "0" + str;
        }
        return str;
    }
    /// <summary>
    /// 补空（后置）
    /// </summary>
    /// <param name="str"></param>
    /// <param name="len"></param>
    /// <returns></returns>
    public string GetZero(string str, int len)
    {
        while (Encoding.GetEncoding("GB2312").GetBytes(str).Length < len)
        {
            str = str + " ";
        }
        return str;
    }
    /// <summary>
    /// 补任意（后置）
    /// </summary>
    /// <param name="str"></param>
    /// <param name="len"></param>
    /// <returns></returns>
    public string GetZero(string str, int len, string rep)
    {
        while (Encoding.GetEncoding("GB2312").GetBytes(str).Length < len)
        {
            str = str + rep;
        }
        return str;
    }
    /// <summary>
    /// 补任意（前置）
    /// </summary>
    /// <param name="str"></param>
    /// <param name="len"></param>
    /// <returns></returns>
    public string GetZero(string str, string rep, int len)
    {
        while (Encoding.GetEncoding("GB2312").GetBytes(str).Length < len)
        {
            str = rep + str;
        }
        return str;
    }
    #endregion

    #region 解包
    /*
         A.包头的长度是固定的   B.截取出包体后，剩下的以'&'为分隔符分割成列
         * 思路：
         * 1、获取银行发来的字符串
         * 2、取出包头数据并转换成DataTable
         * 3、取出包体数据，根据包头信息读取包体XML->DataTable
         * 4、将包体数据填充到DataTable中
         * 5、返回包头、包体数据集
         */
    //包头长度：4+2+16+8+6+6+42+1+4+5+20+4=118
    /// <summary>
    /// 从银行给的数据流中提取包头信息
    /// </summary>
    /// <param name="b"></param>
    /// <returns></returns>
    public string GetBtouStr(byte[] b)
    {
        //截取前118位数组（0-117），这部分是包头的byte流信息
        if (b.Length < 118)
            return null;
        else
        {
            byte[] bTemp = new byte[118];
            for (int i = 0; i < bTemp.Length; i++)
            {
                bTemp[i] = b[i];//获取了包头的byte流
            }
            string str = Encoding.GetEncoding("GB2312").GetString(bTemp);
            return str;
        }
    }
    /// <summary>
    /// 从银行给的数据流中提取包头信息
    /// </summary>
    /// <param name="b"></param>
    /// <returns></returns>
    public string GetBtouStr(byte[] b, ref List<byte> list)
    {
        if (b.Length < 118)
            return null;
        else
        {
            byte[] bTemp = new byte[118];
            for (int i = 0; i < bTemp.Length; i++)
            {
                bTemp[i] = b[i];//获取了包头的byte流
                list.Add(bTemp[i]);
            }
            return Encoding.GetEncoding("GB2312").GetString(bTemp);
        }
    }
    /// <summary>
    /// 获取包头DataTable
    /// </summary>
    /// <param name="b"></param>
    /// <returns></returns>
    public DataTable GetBaotou(byte[] b)
    {
        List<byte> list = new List<byte>();
        string str = GetBtouStr(b, ref list);
        DataTable dt = new DataTable();
        string path = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\XML\壮哉我大平安报文头.xml";
        dt.ReadXml(path);//初始化包头结构
        byte[] b_TranFunc = new byte[4];//交易类型
        list.CopyTo(0, b_TranFunc, 0, 4);
        byte[] b_ServType = new byte[2];//服务类型
        list.CopyTo(4, b_ServType, 0, 2);
        byte[] b_MacCode = new byte[16];//MAC码
        list.CopyTo(6, b_MacCode, 0, 16);
        byte[] b_TranDate = new byte[8];//交易日期
        list.CopyTo(22, b_TranDate, 0, 8);
        byte[] b_TranTime = new byte[6];//交易时间
        list.CopyTo(30, b_TranTime, 0, 6);
        byte[] b_RspCode = new byte[6];//应答码
        list.CopyTo(36, b_RspCode, 0, 6);
        byte[] b_RspMsg = new byte[42];//应答码描述
        list.CopyTo(42, b_RspMsg, 0, 42);
        byte[] b_ConFlag = new byte[1];//后续包标志
        list.CopyTo(84, b_ConFlag, 0, 1);
        byte[] b_Length = new byte[4];//包体长度
        list.CopyTo(85, b_Length, 0, 4);
        byte[] b_CounterId = new byte[5];//操作员号
        list.CopyTo(89, b_CounterId, 0, 5);
        byte[] b_ThirdLogNo = new byte[20];//请求方系统流水号
        list.CopyTo(94, b_ThirdLogNo, 0, 20);
        byte[] b_Qydm = new byte[4];//交易网代码
        list.CopyTo(114, b_Qydm, 0, 4);
        //获取完每一个对应的byte数组后，再对dt进行数据的填充
        dt.Rows[0]["TranFunc"] = GetStr(b_TranFunc);
        dt.Rows[0]["ServType"] = GetStr(b_ServType);
        dt.Rows[0]["MacCode"] = GetStr(b_MacCode);
        dt.Rows[0]["TranDate"] = GetStr(b_TranDate);
        dt.Rows[0]["TranTime"] = GetStr(b_TranTime);
        dt.Rows[0]["RspCode"] = GetStr(b_RspCode);
        dt.Rows[0]["RspMsg"] = GetStr(b_RspMsg);
        dt.Rows[0]["ConFlag"] = GetStr(b_ConFlag);
        dt.Rows[0]["Length"] = GetStr(b_Length);
        dt.Rows[0]["CounterId"] = GetStr(b_CounterId);
        dt.Rows[0]["ThirdLogNo"] = GetStr(b_ThirdLogNo);
        dt.Rows[0]["Qydm"] = GetStr(b_Qydm);
        //获取完包头序列化好的结构，返回到调用位置。
        return dt;
    }
    /// <summary>
    /// 根据数据包包体字符串
    /// </summary>
    /// <param name="b">银行发回来的完整的byte流</param>
    /// <returns></returns>
    public string GetBTiStr(byte[] b)
    {
        List<byte> list = new List<byte>();
        string result = string.Empty;
        if (b.Length < 118)
            return null;
        else
        {
            for (int i = 118; i < b.Length; i++)
            {
                list.Add(b[i]);//取出了包体的byte流
            }
            result = Encoding.GetEncoding("GB2312").GetString(list.ToArray());
            return result;
        }
    }
    /// <summary>
    /// 获取包体结构（仅限银行发送到本地的）
    /// </summary>
    /// <param name="b">完整的byte流</param>
    /// <param name="i">0--代表我方主动发起，银行反馈；1--代表银行主动发起</param>
    /// <returns></returns>
    public DataTable GetBaoti(byte[] b, int i)
    {
        string baotou = GetBtouStr(b);
        if (baotou == null)
            return null;
        if (i != 0 && i != 1)
            return null;
        string FuncId = baotou.Substring(0, 4);//获取接口编号
        if (FuncId == "1010" || FuncId == "1012" || FuncId == "1013" || FuncId == "1015" || FuncId == "1016")
        {
            return null;//这几个接口需要特殊处理
            //return Get1010(baoti);//12月3号给的文档中,1010接口需要特殊处理
        }
        string xmlHead = GetXMLHeadStr(i);
        string xmlPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\XML\" + xmlHead + FuncId + ".xml";//对应的XML路径
        DataTable dt = new DataTable();//将要返回给调用位置的包体数据表
        dt.ReadXml(xmlPath);
        string baoti = GetBTiStr(b);//包体字符串
        string[] strAry = baoti.Split(new string[] { "&" }, StringSplitOptions.None);
        //因为平安银行的包体在最后一定会有一个“&”，C#分解数组的时候，会把这个记作有效元素，应该-1
        if (strAry.Length - 1 != dt.Columns.Count)
            return null;
        for (int j = 0; j < dt.Columns.Count; j++)
        {
            dt.Rows[0][j] = (strAry[j].Trim() == "" ? " " : strAry[j]);
        }
        return dt;
    }
    /// <summary>
    /// 获取XML前缀字符串
    /// </summary>
    /// <param name="i">0代表我方发起指令，银行反馈的包。1代表银行主动发起的包。</param>
    /// <returns></returns>
    public string GetXMLHeadStr(int i)
    {
        switch (i)
        {
            case 0:
                return "Pingan_Reply_";
            case 1:
                return "Pingan_Send_";
            default:
                return null;
        }
    }

    /// <summary>
    /// GB2312模式下把byte[]转换成string
    /// </summary>
    /// <param name="b"></param>
    /// <returns></returns>
    public string GetStr(byte[] b)
    {
        return Encoding.GetEncoding("GB2312").GetString(b);
    }
    #endregion

}