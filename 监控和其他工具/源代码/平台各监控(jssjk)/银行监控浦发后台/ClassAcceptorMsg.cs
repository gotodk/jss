using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace 银行监控浦发后台
{
    public class ClassAcceptorMsg
    {
        //向线程传递的回调参数
        private delegateForThread DForThread;
        //向线程传递的数据参数
        private Hashtable InPutHT;
        /// <summary>
        /// byte编码方式
        /// </summary>
        string EncodingName;
        /// <summary>
        /// IP地址
        /// </summary>
        string Dip;
        /// <summary>
        /// 端口
        /// </summary>
        string Ddk;
        /// <summary>
        /// 包头
        /// </summary>
        string Dbaotou;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="PHT">需要传入线程的参数</param>
        /// <param name="DFT">线程委托</param>
        public ClassAcceptorMsg(Hashtable PHT, delegateForThread DFT)
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
            DForThread = DFT;
            InPutHT = PHT;

            //初始化该银行的一些固定默认参数
            EncodingName = "GB2312";
            Dip = "192.168.16.28";
            Ddk = "40012";
            //F＋3位包头长度＋5位包体长度＋8位券商号＋3位密码加密标志＋8位MAC值
            Dbaotou = "F028包体长度60050000加密标志三位MAC值八位";
        }
        /// <summary>
        /// 检测指定文件是否存在,如果存在则返回true。
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>        
        public bool IsExistFile(string filePath)
        {
            return File.Exists(filePath);
        }
        /// <summary>
        /// 检测指定目录是否存在
        /// </summary>
        /// <param name="directoryPath">目录的绝对路径</param>
        /// <returns></returns>
        public bool IsExistDirectory(string directoryPath)
        {
            return Directory.Exists(directoryPath);
        }
        /// <summary>
        /// 根据该银行协议规则，把配置好的指令，根据固定默认参数，添加包头后，转化成字节流和视觉检测字符串
        /// </summary>
        /// <param name="dt">数据集</param>
        /// <param name="IsJM">是否启用加密</param>
        /// <param name="JMstr">备用的，加密方法或关键字符</param>
        /// <returns>返回值：字节流|视觉检测字符串</returns>
        public Hashtable Get_ByteAndString_From_DataTable(DataTable dt, bool IsJM, string JMstr)
        {
            //初始化返回值
            Hashtable reHT = new Hashtable();
            reHT["字节流"] = null;
            reHT["视觉检测字符串"] = "";

            //数据集为空，直接返回
            if (dt == null || dt.Rows.Count <= 0)
            {
                return reHT;
            }
            string BTStr = "";
            if (dt.Columns.Contains("ErrorNo"))
            {
                //说明这个包是应答包
                BTStr = ((!dt.Columns.Contains("FunctionId")) || dt.Rows[0]["FunctionId"] == null ? "" : dt.Rows[0]["FunctionId"].ToString()) + ((!dt.Columns.Contains("ExSerial")) || dt.Rows[0]["ExSerial"] == null ? "" : dt.Rows[0]["ExSerial"].ToString()) + ((!dt.Columns.Contains("ErrorNo")) || dt.Rows[0]["ErrorNo"] == null ? "" : dt.Rows[0]["ErrorNo"].ToString()) + ((!dt.Columns.Contains("sCustAcct")) || dt.Rows[0]["sCustAcct"] == null ? "" : dt.Rows[0]["sCustAcct"].ToString()) + ((!dt.Columns.Contains("MoneyType")) || dt.Rows[0]["MoneyType"] == null ? "" : dt.Rows[0]["MoneyType"].ToString()) + ((!dt.Columns.Contains("OccurBala")) || dt.Rows[0]["OccurBala"] == null ? "" : dt.Rows[0]["OccurBala"].ToString());
            }
            else
            {
                //说明这个包是请求包
                BTStr = ((!dt.Columns.Contains("FunctionId") || dt.Rows[0]["FunctionId"] == null) ? "" : dt.Rows[0]["FunctionId"].ToString()) + (((!dt.Columns.Contains("ExSerial")) || dt.Rows[0]["ExSerial"] == null) ? "" : dt.Rows[0]["ExSerial"].ToString()) + ((!dt.Columns.Contains("Date") || dt.Rows[0]["Date"] == null) ? "" : dt.Rows[0]["Date"].ToString()) + ((!dt.Columns.Contains("Time")) || dt.Rows[0]["Time"] == null ? "" : dt.Rows[0]["Time"].ToString()) + ((!dt.Columns.Contains("bCustAcct")) || dt.Rows[0]["bCustAcct"] == null ? "" : dt.Rows[0]["bCustAcct"].ToString()) + ((!dt.Columns.Contains("MoneyType")) || dt.Rows[0]["MoneyType"] == null ? "" : dt.Rows[0]["MoneyType"].ToString()) + ((!dt.Columns.Contains("OccurBala")) || dt.Rows[0]["OccurBala"] == null ? "" : dt.Rows[0]["OccurBala"].ToString());
            }

            //根据协议规则，把数据集转化成一个字符串
            byte[] Macs = CallPinMac.GetMac(BTStr);
            //处理包头
            string tou = Dbaotou;
            tou = tou.Replace("加密标志三位", "110").Replace("MAC值八位", "");
            //处理包体
            string baoti1 = dt.Columns.Count.ToString() + Chr(1) + dt.Rows.Count.ToString() + Chr(1);
            string baoti2 = "";
            for (int c = 0; c < dt.Columns.Count; c++)
            {
                baoti2 = baoti2 + dt.Columns[c].ColumnName + Chr(1);
            }
            string baoti3 = "";
            for (int r = 0; r < dt.Rows.Count; r++)
            {
                for (int c = 0; c < dt.Columns.Count; c++)
                {
                    baoti3 = baoti3 + dt.Rows[r][c].ToString() + Chr(1);
                }
            }
            //重新处理包头，替换包体长度(中文算两个字符)
            string baoti_changdu = System.Text.Encoding.GetEncoding(EncodingName).GetBytes(baoti1 + baoti2 + baoti3).Length.ToString();
            tou = tou.Replace("包体长度", baoti_changdu.PadLeft(5, '0'));

            string MacStr = System.Text.Encoding.GetEncoding(EncodingName).GetString(Macs);
            //得到完整包字符串
            string messagestr = tou + MacStr + baoti1 + baoti2 + baoti3;

            //把转化好的字符串，用指定编码格式，转化成字节流
            //byte[] byteSend = System.Text.Encoding.GetEncoding(EncodingName).GetBytes(messagestr);
            byte[] TouB = System.Text.Encoding.GetEncoding(EncodingName).GetBytes(tou);
            List<byte> listB = new List<byte>();
            foreach (byte TouBs in TouB)
            {
                listB.Add(TouBs);
            }
            foreach (byte MacBs in Macs)
            {
                listB.Add(MacBs);
            }
            byte[] tiB = System.Text.Encoding.GetEncoding(EncodingName).GetBytes(baoti1 + baoti2 + baoti3);
            foreach (byte TiBs in tiB)
            {
                listB.Add(TiBs);
            }
            byte[] byteSend = listB.ToArray();

            reHT["字节流"] = byteSend;
            reHT["视觉检测字符串"] = messagestr;

            return reHT;
        }

        /// <summary>
        /// 根据银行协议规则，把收到的字节流，根据固定默认参数，参考包头加密方式后，转化成包头包体视觉检测字符串和包体数据集
        /// </summary>
        /// <param name="remsg">字节流</param>
        /// <param name="IsJM">是否启用加密</param>
        /// <param name="JMstr">备用的，加密方法或关键字符</param>
        /// <returns>返回值：包头视觉检测字符串|包体视觉检测字符串|包体数据集</returns>
        public Hashtable Get_BaotouAndDataTableAndString_From_Byte(byte[] remsg, bool IsJM, string JMstr)
        {
            //初始化返回值
            Hashtable reHT = new Hashtable();
            reHT["包头视觉检测字符串"] = "";
            reHT["包体视觉检测字符串"] = "";
            reHT["包体数据集"] = null;

            //字节流为空，直接返回
            if (remsg == null || remsg.Length <= 0)
            {
                return reHT;
            }

            //得到字节流总长度
            int b = remsg.Length;
            //得到包头字符串，这里暂时是非加密方式，28包头，但最后的8位银行会发的是空格，会被当成结束符。为防止出错，是强制把最后八位替换成了八个零

            /*
                if (Encoding.GetEncoding(EncodingName).GetString(remsg, 28 - 8 - 3, 28 - 8) == "110") 
                {
                     baotou_str = Encoding.GetEncoding(EncodingName).GetString(remsg, 0, 28);
                }
                else
                {
                     baotou_str = Encoding.GetEncoding(EncodingName).GetString(remsg, 0, 28 - 8) + "00000000";
                }
             */


            string baotou_str = Encoding.GetEncoding(EncodingName).GetString(remsg, 0, 28 - 8) + "00000000";
            //得到包体字符串，取28位以后到末位的数据
            string baoti_str = Encoding.GetEncoding(EncodingName).GetString(remsg, 28, b - 28);

            DataTable dt = new DataTable();
            try
            {
                //把包体字符串，转化成数据集
                string[] baoti_Arr = baoti_str.Split(Chr(1).ToCharArray()[0]);
                //得到有多少列
                int Cols = Convert.ToInt32(baoti_Arr[0]);
                //得到有多少行
                int Rows = Convert.ToInt32(baoti_Arr[1]);
                //生成列,从第三个数组元素开始，取到列数结束
                for (int lie = 2; lie < Cols + 2; lie++)
                {
                    dt.Columns.Add(baoti_Arr[lie]);
                }
                //生成行，根据有几行和有几列，跳过前两个数组元素。取到每行的数据
                DataRow dr = dt.NewRow();
                for (int p = 1; p <= Rows; p++)
                {
                    dr = dt.NewRow();
                    for (int i = 0; i < Cols; i++)
                    {
                        //填充值
                        dr[i] = baoti_Arr[2 + p * Cols + i];
                    }
                    dt.Rows.Add(dr);
                }
            }
            catch (Exception ex)
            {
                dt = null;
            }

            reHT["包头视觉检测字符串"] = baotou_str;
            reHT["包体视觉检测字符串"] = baoti_str;
            reHT["包体数据集"] = dt;

            return reHT;
        }

        /// <summary>
        /// asciiCode转字符串
        /// </summary>
        /// <param name="asciiCode"></param>
        /// <returns></returns>
        public string Chr(int asciiCode)
        {
            if (asciiCode >= 0 && asciiCode <= 255)
            {
                System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
                byte[] byteArray = new byte[] { (byte)asciiCode };
                string strCharacter = asciiEncoding.GetString(byteArray);
                return (strCharacter);
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 那个啥。。
        /// </summary>
        public void AcceptorTest()
        {
            IPEndPoint serverFullAddr = new IPEndPoint(IPAddress.Any, Convert.ToInt32(Ddk));//设置接收IP，接收端口
            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                //指定本地主机地址和端口号
                sock.Bind(serverFullAddr);
                sock.Listen(50);
                sock.ReceiveTimeout = 20000; //二十秒没有反馈即为超时
                sock.SendTimeout = 20000;//二十秒没有发送出去，即为超时
                //完成接收服务器启动的回调
                //填充传入参数哈希表
                Hashtable OutPutHT = new Hashtable();
                OutPutHT["执行状态"] = "ok";//测试参数
                OutPutHT["详细信息"] = "已经开启端口监听";
                OutPutHT["测试返回值"] = "";
                DForThread(OutPutHT);
            }
            catch (Exception se)
            {
                Hashtable OutPutHT = new Hashtable();
                OutPutHT["执行状态"] = "端口开启监听失败。";//测试参数
                OutPutHT["详细信息"] = se.Message;
                OutPutHT["测试返回值"] = "";
                DForThread(OutPutHT);
            }
            while (true)
            {
                //定义一个新的套接字接受传递来的消息
                Socket newSocket = sock.Accept();
                newSocket.ReceiveTimeout = 20000; //二十秒没有反馈即为超时
                newSocket.SendTimeout = 20000;//二十秒没有发送出去，即为超时
                byte[] message = new byte[9999];
                int bytes = newSocket.Receive(message);//接收数据

                Hashtable ht_message = Get_BaotouAndDataTableAndString_From_Byte(message, false, "");
                string baotou = ht_message["包头视觉检测字符串"].ToString();
                string baoti = ht_message["包体视觉检测字符串"].ToString();
                DataTable baoDT = (DataTable)(ht_message["包体数据集"]);
                baoDT.TableName = "UINFO";
                //对接收到的数据进行判定处理，这里即模拟银行的反馈，也模拟正常监听到银行发起后的反馈。

                try
                {
                    //若包头以F开头，且包头长度是28，就是来自浦发银行的协议
                    if (baotou.IndexOf("F") == 0 && baotou.Length == 28 && baoDT != null)
                    {
                        string gnh = baoDT.Rows[0]["FunctionId"].ToString();
                        switch (gnh)
                        {
                            case ("25702")://移植客户签约确认(25702)
                                #region 移植客户签约确认(25702)
                                if (!baoDT.Columns.Contains("ErrorNo"))
                                {
                                    //接收银行发来的客户签约确认信息，与本地数据库进行对比，若关键信息一致，则反馈给银行正确的应答，这里为了测试，先跳过数据库验证，直接返回正确的信息
                                    DataTable dteRE;
                                    dteRE = new DataTable("应答");
                                    DataColumn x1 = new DataColumn("FunctionId", typeof(string));//接口也好
                                    DataColumn x2 = new DataColumn("ExSerial", typeof(string));//外部流水号
                                    DataColumn x3 = new DataColumn("ErrorNo", typeof(string));//返回码
                                    DataColumn x4 = new DataColumn("ErrorInfo", typeof(string));//返回说明
                                    DataColumn x5 = new DataColumn("sSerial", typeof(string));//证券流水号
                                    dteRE.Columns.Add(x1);
                                    dteRE.Columns.Add(x2);
                                    dteRE.Columns.Add(x3);
                                    dteRE.Columns.Add(x4);
                                    dteRE.Columns.Add(x5);
                                    DataRow newRow;
                                    newRow = dteRE.NewRow();
                                    /*开始执行服务器验证*/
                                    DataSet dsTo = new DataSet();
                                    dsTo.Tables.Add(baoDT);

                                    // wyh 2014.04.16 add获取客户编号
                                    string Strkhbh = baoDT.Rows[0]["sCustAcct"].ToString().Trim();
                                    object[] cs = { dsTo, Strkhbh, "发起行*浦发银行" };
                                    //wyh end  object[] cs = { dsTo };
                                    
                                    WebServicesCenter2013 wsc = new WebServicesCenter2013();
                                    //DataSet dsReturn = wsc.RunAtServices("CompareUInfo", cs);
                                    DataSet dsReturn = wsc.RunAtServices("多银行框架-银行发起客户签约", cs);
                                    if (dsReturn == null || dsReturn.Tables["返回值单条"].Rows.Count <= 0)
                                    {
                                        newRow["FunctionId"] = baoDT.Rows[0]["FunctionId"].ToString();
                                        newRow["ExSerial"] = baoDT.Rows[0]["ExSerial"].ToString();
                                        newRow["ErrorNo"] = "2405";
                                        newRow["ErrorInfo"] = "连接交易所败！";
                                    }
                                    /*验证完毕*/
                                    //dsReturn.Tables["返回值单条"].Rows[0]["执行结果"];

                                    else if (dsReturn.Tables["返回值单条"].Rows[0]["执行结果"].ToString() == "ok")
                                    {
                                        newRow["FunctionId"] = baoDT.Rows[0]["FunctionId"].ToString();
                                        newRow["ExSerial"] = baoDT.Rows[0]["ExSerial"].ToString();
                                        newRow["ErrorNo"] = dsReturn.Tables["返回值单条"].Rows[0]["附件信息1"]; ;
                                        newRow["ErrorInfo"] = dsReturn.Tables["返回值单条"].Rows[0]["提示文本"]; ;
                                        newRow["sSerial"] = dsReturn.Tables["返回值单条"].Rows[0]["附件信息2"];
                                    }
                                    else
                                    {
                                        newRow["FunctionId"] = baoDT.Rows[0]["FunctionId"].ToString();
                                        newRow["ExSerial"] = baoDT.Rows[0]["ExSerial"].ToString();

                                        if (dsReturn.Tables["返回值单条"].Rows[0]["提示文本"].ToString().Equals("该客户不存在"))
                                        {
                                            newRow["ErrorNo"] = "8888";
                                        }
                                        else
                                        {
                                            newRow["ErrorNo"] = dsReturn.Tables["返回值单条"].Rows[0]["附件信息1"];
                                        }

                                        //newRow["ErrorNo"] = dsReturn.Tables["返回值单条"].Rows[0]["附件信息1"];
                                        newRow["ErrorInfo"] = dsReturn.Tables["返回值单条"].Rows[0]["提示文本"];
                                    }
                                    dteRE.Rows.Add(newRow);

                                    //转换反馈数据
                                    Hashtable reHT = Get_ByteAndString_From_DataTable(dteRE, false, "");
                                    byte[] reByte = (byte[])(reHT["字节流"]);
                                    string restr = reHT["视觉检测字符串"].ToString();

                                    int r = newSocket.Send(reByte);//发送反馈数据

                                    #region 撰写日志
                                    //try
                                    //{
                                    string serverDir = System.AppDomain.CurrentDomain.BaseDirectory + "\\Log_BankService_GT\\" + DateTime.Now.ToString("yyyy-MM-dd") + "\\25702\\";
                                    //string serverDir = HttpContext.Current.Server.MapPath("~/Log_BankService_GT/" + DateTime.Now.ToString("yyyy-MM-dd") + "/25702/");
                                    if (!IsExistDirectory(serverDir))
                                        Directory.CreateDirectory(serverDir);
                                    string serverFile = serverDir + DateTime.Now.ToString("HH：mm：ss-fff") + ".txt";
                                    if (!IsExistFile(serverFile))
                                    {
                                        FileStream f = File.Create(serverFile);
                                        f.Close();
                                        f.Dispose();
                                    }
                                    using (StreamWriter sw = new StreamWriter(serverFile, true, System.Text.Encoding.UTF8))
                                    {
                                        if (dsReturn == null)
                                        {
                                            sw.WriteLine("接收到银行发来的包头：" + baotou);
                                            sw.WriteLine("接收到银行发来的包体：" + baoti);
                                            sw.WriteLine("总长度为：" + bytes);
                                            sw.WriteLine("发送到我方服务器进行处理，得到的DataSet为Null");
                                        }
                                        else
                                        {
                                            sw.WriteLine("接收到银行发来的包头：" + baotou);
                                            sw.WriteLine("接收到银行发来的包体：" + baoti.Replace(Chr(0), ""));
                                            sw.WriteLine("总长度为：" + bytes);
                                            sw.WriteLine("已接收我方服务器的处理结果。----------");
                                            sw.WriteLine("返回码：" + dsReturn.Tables["返回值单条"].Rows[0]["附件信息1"].ToString());
                                            sw.WriteLine("代码信息：" + dsReturn.Tables["返回值单条"].Rows[0]["提示文本"].ToString());
                                            sw.WriteLine("回馈给银行的数据包为：" + restr);
                                            sw.WriteLine("最终回馈长度为：" + r);
                                        }
                                    }
                                    //}
                                    //catch (Exception exexexe) { throw; }
                                    #endregion

                                    //填充传入参数哈希表
                                    Hashtable OutPutHT = new Hashtable();
                                    OutPutHT["执行状态"] = "回馈成功";//测试参数
                                    OutPutHT["详细信息"] = "执行成功";
                                    if (dsReturn != null)
                                        OutPutHT["测试返回值"] = "接收了[" + gnh + "]请求并反馈了结果。执行结果为：" + dsReturn.Tables["返回值单条"].Rows[0]["附件信息1"].ToString() + "；" + dsReturn.Tables["返回值单条"].Rows[0]["提示文本"].ToString();
                                    else
                                        OutPutHT["测试返回值"] = "接收了[" + gnh + "]请求并反馈了结果。我方服务器返回Null DataSet.";
                                    DForThread(OutPutHT);
                                }

                                break;
                                #endregion
                            case ("23603"):     //	查询证券资金余额(银行端发起)
                                #region  查询证券资金余额(银行端发起)
    
                                if (!baoDT.Columns.Contains("ErrorNo"))
                                {
                                    //查询券商资金余额
                                    DataTable dteRE;
                                    dteRE = new DataTable("应答");
                                    DataColumn x1 = new DataColumn("FunctionId", typeof(string));//接口也好
                                    DataColumn x2 = new DataColumn("ExSerial", typeof(string));//外部流水号
                                    DataColumn x3 = new DataColumn("ErrorNo", typeof(string));//返回码
                                    DataColumn x4 = new DataColumn("ErrorInfo", typeof(string));//返回说明
                                    DataColumn x5 = new DataColumn("sSerial", typeof(string));//证券流水号
                                    DataColumn x6 = new DataColumn("sCustAcct", typeof(string));//证券资金账号
                                    DataColumn x7 = new DataColumn("MoneyType", typeof(string));//币种
                                    DataColumn x8 = new DataColumn("OccurBala", typeof(string));//证券资金余额
                                    DataColumn x9 = new DataColumn("sFetchBala", typeof(string));//证券可取金额
                                    dteRE.Columns.Add(x1);
                                    dteRE.Columns.Add(x2);
                                    dteRE.Columns.Add(x3);
                                    dteRE.Columns.Add(x4);
                                    dteRE.Columns.Add(x5);
                                    dteRE.Columns.Add(x6);
                                    dteRE.Columns.Add(x7);
                                    dteRE.Columns.Add(x8);
                                    dteRE.Columns.Add(x9);
                                    DataRow newRow;
                                    newRow = dteRE.NewRow();
                                    /*开始执行服务器验证*/
                                    DataSet dsTo = new DataSet();
                                    dsTo.Tables.Add(baoDT);

                                    // wyh 2014.04.16 add获取客户编号
                                    string Strkhbh = baoDT.Rows[0]["sCustAcct"].ToString().Trim();
                                    object[] cs = { dsTo, Strkhbh, "发起行*浦发银行" };
                                    //wyh end  object[] cs = { dsTo };

                                    //object[] cs = { dsTo };
                                    WebServicesCenter2013 wsc = new WebServicesCenter2013();
                                    //DataSet dsReturn = wsc.RunAtServices("GetUMoneyByBANK", cs); 多银行框架-银行查客户余额
                                    DataSet dsReturn = wsc.RunAtServices("多银行框架-银行查客户余额", cs);
                                    if (dsReturn == null || dsReturn.Tables["返回值单条"].Rows.Count <= 0)
                                    {
                                        newRow["FunctionId"] = baoDT.Rows[0]["FunctionId"].ToString();
                                        newRow["ExSerial"] = baoDT.Rows[0]["ExSerial"].ToString();
                                        newRow["ErrorNo"] = "8888";
                                        newRow["ErrorInfo"] = "证券方服务器繁忙";
                                    }
                                    /*验证完毕*/
                                    //dsReturn.Tables["返回值单条"].Rows[0]["执行结果"];

                                    else if (dsReturn.Tables["返回值单条"].Rows[0]["执行结果"].ToString() == "ok")
                                    {
                                        newRow["FunctionId"] = baoDT.Rows[0]["FunctionId"].ToString();
                                        newRow["ExSerial"] = baoDT.Rows[0]["ExSerial"].ToString();
                                        newRow["ErrorNo"] = dsReturn.Tables["返回值单条"].Rows[0]["附件信息1"];
                                        newRow["ErrorInfo"] = dsReturn.Tables["返回值单条"].Rows[0]["提示文本"]; ;
                                        newRow["sSerial"] = dsReturn.Tables["返回值单条"].Rows[0]["附件信息2"];
                                        newRow["sCustAcct"] = baoDT.Rows[0]["sCustAcct"];
                                        newRow["MoneyType"] = baoDT.Rows[0]["MoneyType"];
                                        newRow["OccurBala"] = dsReturn.Tables["返回值单条"].Rows[0]["附件信息3"];
                                        newRow["sFetchBala"] = dsReturn.Tables["返回值单条"].Rows[0]["附件信息3"];
                                    }
                                    else
                                    {
                                        newRow["FunctionId"] = baoDT.Rows[0]["FunctionId"].ToString();
                                        newRow["ExSerial"] = baoDT.Rows[0]["ExSerial"].ToString();

                                        //2014.04.16 wyh add
                                        if (dsReturn.Tables["返回值单条"].Rows[0]["提示文本"].ToString().Equals("该客户不存在"))
                                        {
                                            newRow["ErrorNo"] = "8888";
                                        }
                                        else
                                        {
                                            newRow["ErrorNo"] = dsReturn.Tables["返回值单条"].Rows[0]["附件信息1"];
                                        }

                                        //newRow["ErrorNo"] = dsReturn.Tables["返回值单条"].Rows[0]["附件信息1"];
                                        newRow["ErrorInfo"] = dsReturn.Tables["返回值单条"].Rows[0]["提示文本"];
                                    }
                                    dteRE.Rows.Add(newRow);

                                    //转换反馈数据
                                    Hashtable reHT = Get_ByteAndString_From_DataTable(dteRE, false, "");
                                    byte[] reByte = (byte[])(reHT["字节流"]);
                                    string restr = reHT["视觉检测字符串"].ToString();

                                    int r = newSocket.Send(reByte);//发送反馈数据


                                    //try
                                    //{
                                    string serverDir = System.AppDomain.CurrentDomain.BaseDirectory + "\\Log_BankService_GT\\" + DateTime.Now.ToString("yyyy-MM-dd") + "\\23603\\";
                                    if (!IsExistDirectory(serverDir))
                                        Directory.CreateDirectory(serverDir);
                                    string serverFile = serverDir + DateTime.Now.ToString("HH：mm：ss-fff") + ".txt";
                                    if (!IsExistFile(serverFile))
                                    {
                                        FileStream f = File.Create(serverFile);
                                        f.Close();
                                        f.Dispose();
                                    }
                                    using (StreamWriter sw = new StreamWriter(serverFile, true, System.Text.Encoding.UTF8))
                                    {
                                        if (dsReturn == null)
                                        {
                                            sw.WriteLine("接收到银行发来的包头：" + baotou);
                                            sw.WriteLine("接收到银行发来的包体：" + baoti);
                                            sw.WriteLine("总长度为：" + bytes);
                                            sw.WriteLine("发送到我方服务器进行处理，得到的DataSet为Null");
                                        }
                                        if (dsReturn != null)
                                        {
                                            //sw.WriteLine("包头：" + baotou + "   |   包体：" + baoti + "   |   dsReturn是否为空：非空" + "    |   返回码：" + dsReturn.Tables["返回值单条"].Rows[0]["附件信息1"].ToString() + "   |   " + dsReturn.Tables["返回值单条"].Rows[0]["提示文本"].ToString());
                                            sw.WriteLine("接收到银行发来的包头：" + baotou);
                                            sw.WriteLine("接收到银行发来的包体：" + baoti.Replace(Chr(0), ""));
                                            sw.WriteLine("总长度为：" + bytes);
                                            sw.WriteLine("已接收我方服务器的处理结果。----------");
                                            sw.WriteLine("返回码：" + dsReturn.Tables["返回值单条"].Rows[0]["附件信息1"].ToString());
                                            sw.WriteLine("代码信息：" + dsReturn.Tables["返回值单条"].Rows[0]["提示文本"].ToString());
                                            sw.WriteLine("回馈给银行的数据包为：" + restr);
                                            sw.WriteLine("最终回馈长度为：" + r);
                                        }
                                    }
                                    //}
                                    //catch (Exception exexexe) { throw; }


                                    //填充传入参数哈希表
                                    Hashtable OutPutHT = new Hashtable();
                                    OutPutHT["执行状态"] = "回馈成功";//测试参数
                                    OutPutHT["详细信息"] = "执行成功";
                                    if (dsReturn != null)
                                        OutPutHT["测试返回值"] = "接收了[" + gnh + "]请求并反馈了结果。执行结果为：" + dsReturn.Tables["返回值单条"].Rows[0]["附件信息1"].ToString() + "；" + dsReturn.Tables["返回值单条"].Rows[0]["提示文本"].ToString();
                                    else
                                        OutPutHT["测试返回值"] = "接收了[" + gnh + "]请求并反馈了结果。我方服务器返回了Null DataSet";
                                    DForThread(OutPutHT);
                                }
                                break;
                                #endregion
                            case ("23701"):  //入金（银行端发起）   
                                #region  入金（银行端发起）
                                //入金
                                //若是指令包
                                if (!baoDT.Columns.Contains("ErrorNo"))
                                {
                                    #region//生成应答DataTable
                                    DataTable dteRE;
                                    dteRE = new DataTable("应答");
                                    DataColumn x1 = new DataColumn("FunctionId", typeof(string));//功能号
                                    DataColumn x2 = new DataColumn("ExSerial", typeof(string));//外部流水号
                                    DataColumn x3 = new DataColumn("ErrorNo", typeof(string));//返回码
                                    DataColumn x4 = new DataColumn("ErrorInfo", typeof(string));//返回说明
                                    DataColumn x5 = new DataColumn("sSerial", typeof(string));//证券流水号
                                    //DataColumn x6 = new DataColumn("MoneyType", typeof(string));//币种
                                    DataColumn x7 = new DataColumn("sFetchBala", typeof(string));//证券可取金额
                                    dteRE.Columns.Add(x1);
                                    dteRE.Columns.Add(x2);
                                    dteRE.Columns.Add(x3);
                                    dteRE.Columns.Add(x4);
                                    dteRE.Columns.Add(x5);
                                    //dteRE.Columns.Add(x6);
                                    dteRE.Columns.Add(x7);
                                    DataRow newRow;
                                    #endregion

                                    #region//执行数据库验证及操作
                                    DataSet dsTo = new DataSet();
                                    dsTo.Tables.Add(baoDT);
                                    string Strkhbh = baoDT.Rows[0]["sCustAcct"].ToString().Trim();
                                    object[] cs = { dsTo, Strkhbh, "发起行*浦发银行" };
                                    //object[] cs = { dsTo };
                                    WebServicesCenter2013 wsc = new WebServicesCenter2013();
                                   // DataSet dsReturn = wsc.RunAtServices("BankToS", cs);
                                    DataSet dsReturn = wsc.RunAtServices("多银行框架-银行发起银转商", cs);
                                    #endregion

                                    #region//生成应答数据内容
                                    newRow = dteRE.NewRow();
                                    if (dsReturn == null || dsReturn.Tables["返回值单条"].Rows.Count <= 0)
                                    {
                                        newRow["FunctionId"] = baoDT.Rows[0]["FunctionId"].ToString();
                                        newRow["ExSerial"] = baoDT.Rows[0]["ExSerial"].ToString();
                                        newRow["ErrorNo"] = "8888";
                                        newRow["ErrorInfo"] = "证券方服务器繁忙";
                                    }


                                    else if (dsReturn.Tables["返回值单条"].Rows[0]["执行结果"].ToString() == "ok")
                                    {
                                        newRow["FunctionId"] = baoDT.Rows[0]["FunctionId"].ToString();
                                        newRow["ExSerial"] = baoDT.Rows[0]["ExSerial"].ToString();
                                        newRow["ErrorNo"] = dsReturn.Tables["返回值单条"].Rows[0]["附件信息1"];
                                        newRow["ErrorInfo"] = dsReturn.Tables["返回值单条"].Rows[0]["提示文本"]; ;
                                        newRow["sSerial"] = dsReturn.Tables["返回值单条"].Rows[0]["附件信息2"];
                                        newRow["sFetchBala"] = dsReturn.Tables["返回值单条"].Rows[0]["附件信息3"];
                                    }
                                    else
                                    {
                                        newRow["FunctionId"] = baoDT.Rows[0]["FunctionId"].ToString();
                                        newRow["ExSerial"] = baoDT.Rows[0]["ExSerial"].ToString();

                                       
                                        newRow["ErrorNo"] = dsReturn.Tables["返回值单条"].Rows[0]["附件信息1"];
                                        newRow["ErrorInfo"] = dsReturn.Tables["返回值单条"].Rows[0]["提示文本"];
                                    }
                                    dteRE.Rows.Add(newRow);
                                    #endregion


                                    //转换反馈数据
                                    Hashtable reHT = Get_ByteAndString_From_DataTable(dteRE, false, "");
                                    byte[] reByte = (byte[])(reHT["字节流"]);
                                    string restr = reHT["视觉检测字符串"].ToString();

                                    int r = newSocket.Send(reByte);//发送反馈数据

                                    //newSocket.Close();

                                    #region//开始写日志
                                    //try
                                    //{
                                    string serverDir = System.AppDomain.CurrentDomain.BaseDirectory + "\\Log_BankService_GT\\" + DateTime.Now.ToString("yyyy-MM-dd") + "\\23701\\";
                                    if (!IsExistDirectory(serverDir))
                                        Directory.CreateDirectory(serverDir);
                                    string serverFile = serverDir + DateTime.Now.ToString("HH：mm：ss-fff") + ".txt";
                                    if (!IsExistFile(serverFile))
                                    {
                                        FileStream f = File.Create(serverFile);
                                        f.Close();
                                        f.Dispose();
                                    }
                                    using (StreamWriter sw = new StreamWriter(serverFile, true, System.Text.Encoding.UTF8))
                                    {
                                        if (dsReturn == null)
                                        {
                                            sw.WriteLine("接收到银行发来的包头：" + baotou);
                                            sw.WriteLine("接收到银行发来的包体：" + baoti);
                                            sw.WriteLine("总长度为：" + bytes);
                                            sw.WriteLine("发送到我方服务器进行处理，得到的DataSet为Null");
                                        }
                                        if (dsReturn != null)
                                        {
                                            //sw.WriteLine("包头：" + baotou + "   |   包体：" + baoti + "   |   dsReturn是否为空：非空" + "    |   返回码：" + dsReturn.Tables["返回值单条"].Rows[0]["附件信息1"].ToString() + "   |   " + dsReturn.Tables["返回值单条"].Rows[0]["提示文本"].ToString());
                                            sw.WriteLine("接收到银行发来的包头：" + baotou);
                                            sw.WriteLine("接收到银行发来的包体：" + baoti.Replace(Chr(0), ""));
                                            sw.WriteLine("总长度为：" + bytes);
                                            sw.WriteLine("已接收我方服务器的处理结果。----------");
                                            sw.WriteLine("返回码：" + dsReturn.Tables["返回值单条"].Rows[0]["附件信息1"].ToString());
                                            sw.WriteLine("代码信息：" + dsReturn.Tables["返回值单条"].Rows[0]["提示文本"].ToString());
                                            sw.WriteLine("回馈给银行的数据包为：" + restr);
                                            sw.WriteLine("最终回馈长度为：" + r);
                                        }
                                    }
                                    //}
                                    //catch (Exception exexexe) { throw; }

                                    #endregion

                                    //填充传入参数哈希表
                                    Hashtable OutPutHT = new Hashtable();
                                    OutPutHT["执行状态"] = "回馈成功";//测试参数
                                    OutPutHT["详细信息"] = "执行成功";
                                    if (dsReturn != null)
                                        OutPutHT["测试返回值"] = "接收了[" + gnh + "]请求并反馈了结果。执行结果为：" + dsReturn.Tables["返回值单条"].Rows[0]["附件信息1"].ToString() + "；" + dsReturn.Tables["返回值单条"].Rows[0]["提示文本"].ToString();
                                    else
                                        OutPutHT["测试返回值"] = "接收了[" + gnh + "]请求并反馈了结果。我方服务器返回了Null DataSet";
                                    DForThread(OutPutHT);

                                }
                                break;
                                #endregion
                            case ("23702"):  //出金（银行端发起） 
                                #region 出金（银行端发起）
                                //出金
                                //若是指令包
                                if (!baoDT.Columns.Contains("ErrorNo"))
                                {
                                    #region//生成应答DataTable
                                    DataTable dteRE;
                                    dteRE = new DataTable("应答");
                                    DataColumn x1 = new DataColumn("FunctionId", typeof(string));//功能号
                                    DataColumn x2 = new DataColumn("ExSerial", typeof(string));//外部流水号
                                    DataColumn x3 = new DataColumn("ErrorNo", typeof(string));//返回码
                                    DataColumn x4 = new DataColumn("ErrorInfo", typeof(string));//返回说明
                                    DataColumn x5 = new DataColumn("sSerial", typeof(string));//证券流水号
                                    //DataColumn x6 = new DataColumn("MoneyType", typeof(string));//币种
                                    DataColumn x7 = new DataColumn("sFetchBala", typeof(string));//证券可取金额
                                    dteRE.Columns.Add(x1);
                                    dteRE.Columns.Add(x2);
                                    dteRE.Columns.Add(x3);
                                    dteRE.Columns.Add(x4);
                                    dteRE.Columns.Add(x5);
                                    //dteRE.Columns.Add(x6);
                                    dteRE.Columns.Add(x7);
                                    DataRow newRow;
                                    #endregion

                                    #region//执行数据库验证及操作
                                    DataSet dsTo = new DataSet();
                                    dsTo.Tables.Add(baoDT);

                                    string Strkhbh = baoDT.Rows[0]["sCustAcct"].ToString().Trim();
                                    object[] cs = { dsTo, Strkhbh, "发起行*浦发银行" };

                                    //  object[] cs = { dsTo };
                                    WebServicesCenter2013 wsc = new WebServicesCenter2013();
                                   // DataSet dsReturn = wsc.RunAtServices("SToBank", cs);
                                    DataSet dsReturn = wsc.RunAtServices("多银行框架-银行发起商转银", cs);
                                    #endregion

                                    #region//生成应答数据内容
                                    newRow = dteRE.NewRow();
                                    if (dsReturn == null || dsReturn.Tables["返回值单条"].Rows.Count <= 0)
                                    {
                                        newRow["FunctionId"] = baoDT.Rows[0]["FunctionId"].ToString();
                                        newRow["ExSerial"] = baoDT.Rows[0]["ExSerial"].ToString();
                                        newRow["ErrorNo"] = "8888";
                                        newRow["ErrorInfo"] = "证券方服务器繁忙";
                                    }


                                    else if (dsReturn.Tables["返回值单条"].Rows[0]["执行结果"].ToString() == "ok")
                                    {
                                        newRow["FunctionId"] = baoDT.Rows[0]["FunctionId"].ToString();
                                        newRow["ExSerial"] = baoDT.Rows[0]["ExSerial"].ToString();
                                        newRow["ErrorNo"] = dsReturn.Tables["返回值单条"].Rows[0]["附件信息1"]; ;
                                        newRow["ErrorInfo"] = dsReturn.Tables["返回值单条"].Rows[0]["提示文本"]; ;
                                        newRow["sSerial"] = dsReturn.Tables["返回值单条"].Rows[0]["附件信息2"];
                                        newRow["sFetchBala"] = dsReturn.Tables["返回值单条"].Rows[0]["附件信息3"];
                                    }
                                    else
                                    {
                                        newRow["FunctionId"] = baoDT.Rows[0]["FunctionId"].ToString();
                                        newRow["ExSerial"] = baoDT.Rows[0]["ExSerial"].ToString();
                                        
                                        newRow["ErrorNo"] = dsReturn.Tables["返回值单条"].Rows[0]["附件信息1"];
                                        newRow["ErrorInfo"] = dsReturn.Tables["返回值单条"].Rows[0]["提示文本"];
                                    }
                                    dteRE.Rows.Add(newRow);
                                    #endregion


                                    //转换反馈数据
                                    Hashtable reHT = Get_ByteAndString_From_DataTable(dteRE, false, "");
                                    byte[] reByte = (byte[])(reHT["字节流"]);
                                    string restr = reHT["视觉检测字符串"].ToString();

                                    int r = newSocket.Send(reByte);//发送反馈数据

                                    //newSocket.Close();

                                    #region//开始写日志
                                    //try
                                    //{
                                    string serverDir = System.AppDomain.CurrentDomain.BaseDirectory + "\\Log_BankService_GT\\" + DateTime.Now.ToString("yyyy-MM-dd") + "\\23702\\";
                                    if (!IsExistDirectory(serverDir))
                                        Directory.CreateDirectory(serverDir);
                                    string serverFile = serverDir + DateTime.Now.ToString("HH：mm：ss-fff") + ".txt";
                                    if (!IsExistFile(serverFile))
                                    {
                                        FileStream f = File.Create(serverFile);
                                        f.Close();
                                        f.Dispose();
                                    }
                                    using (StreamWriter sw = new StreamWriter(serverFile, true, System.Text.Encoding.UTF8))
                                    {
                                        if (dsReturn == null)
                                        {
                                            sw.WriteLine("接收到银行发来的包头：" + baotou);
                                            sw.WriteLine("接收到银行发来的包体：" + baoti);
                                            sw.WriteLine("总长度为：" + bytes);
                                            sw.WriteLine("发送到我方服务器进行处理，得到的DataSet为Null");
                                        }
                                        if (dsReturn != null)
                                        {
                                            //sw.WriteLine("包头：" + baotou + "   |   包体：" + baoti + "   |   dsReturn是否为空：非空" + "    |   返回码：" + dsReturn.Tables["返回值单条"].Rows[0]["附件信息1"].ToString() + "   |   " + dsReturn.Tables["返回值单条"].Rows[0]["提示文本"].ToString());
                                            sw.WriteLine("接收到银行发来的包头：" + baotou);
                                            sw.WriteLine("接收到银行发来的包体：" + baoti.Replace(Chr(0), ""));
                                            sw.WriteLine("总长度为：" + bytes);
                                            sw.WriteLine("已接收我方服务器的处理结果。----------");
                                            sw.WriteLine("返回码：" + dsReturn.Tables["返回值单条"].Rows[0]["附件信息1"].ToString());
                                            sw.WriteLine("代码信息：" + dsReturn.Tables["返回值单条"].Rows[0]["提示文本"].ToString());
                                            sw.WriteLine("回馈给银行的数据包为：" + restr);
                                            sw.WriteLine("最终回馈长度为：" + r);
                                        }
                                    }
                                    //}
                                    //catch (Exception exexexe) { throw; }

                                    #endregion

                                    //填充传入参数哈希表
                                    Hashtable OutPutHT = new Hashtable();
                                    OutPutHT["执行状态"] = "回馈成功";//测试参数
                                    OutPutHT["详细信息"] = "执行成功";
                                    if (dsReturn != null)
                                        OutPutHT["测试返回值"] = "接收了[" + gnh + "]请求并反馈了结果。执行结果为：" + dsReturn.Tables["返回值单条"].Rows[0]["附件信息1"].ToString() + "；" + dsReturn.Tables["返回值单条"].Rows[0]["提示文本"].ToString();
                                    else
                                        OutPutHT["测试返回值"] = "接收了[" + gnh + "]请求并反馈了结果。我方服务器返回了Null DataSet";
                                    DForThread(OutPutHT);

                                }
                                break;
                                #endregion
                            case ("25710"): //变更银行账号（银行发起）
                                #region 变更银行账号（银行发起）
                               
                                //若是指令包
                                if (!baoDT.Columns.Contains("ErrorNo"))
                                {
                                    #region//生成应答DataTable
                                    DataTable dteRE;
                                    dteRE = new DataTable("应答");
                                    DataColumn x1 = new DataColumn("FunctionId", typeof(string));//功能号
                                    DataColumn x2 = new DataColumn("ExSerial", typeof(string));//外部流水号
                                    DataColumn x3 = new DataColumn("ErrorNo", typeof(string));//返回码
                                    DataColumn x4 = new DataColumn("ErrorInfo", typeof(string));//返回说明
                                    DataColumn x5 = new DataColumn("sSerial", typeof(string));//证券流水号
                                    dteRE.Columns.Add(x1);
                                    dteRE.Columns.Add(x2);
                                    dteRE.Columns.Add(x3);
                                    dteRE.Columns.Add(x4);
                                    dteRE.Columns.Add(x5);
                                    DataRow newRow;
                                    #endregion

                                    #region//执行数据库验证及操作
                                    DataSet dsTo = new DataSet();
                                    dsTo.Tables.Add(baoDT);

                                    string Strkhbh = baoDT.Rows[0]["sCustAcct"].ToString().Trim();
                                    object[] cs = { dsTo, Strkhbh, "发起行*浦发银行" };
                                    //object[] cs = { dsTo };

                                    WebServicesCenter2013 wsc = new WebServicesCenter2013();
                                    //DataSet dsReturn = wsc.RunAtServices("BankRequest_UpdateBankAccount", cs);
                                    DataSet dsReturn = wsc.RunAtServices("多银行框架-银行发起变更银行账户", cs);
                                    #endregion

                                    #region//生成应答数据内容
                                    newRow = dteRE.NewRow();
                                    if (dsReturn == null || dsReturn.Tables["返回值单条"].Rows.Count <= 0)
                                    {
                                        newRow["FunctionId"] = baoDT.Rows[0]["FunctionId"].ToString();
                                        newRow["ExSerial"] = baoDT.Rows[0]["ExSerial"].ToString();
                                        newRow["ErrorNo"] = "8888";
                                        newRow["ErrorInfo"] = "证券方服务器繁忙";
                                    }


                                    else if (dsReturn.Tables["返回值单条"].Rows[0]["执行结果"].ToString() == "ok")
                                    {
                                        newRow["FunctionId"] = baoDT.Rows[0]["FunctionId"].ToString();
                                        newRow["ExSerial"] = baoDT.Rows[0]["ExSerial"].ToString();
                                        newRow["ErrorNo"] = dsReturn.Tables["返回值单条"].Rows[0]["附件信息1"]; 
                                        newRow["ErrorInfo"] = dsReturn.Tables["返回值单条"].Rows[0]["提示文本"]; ;
                                        newRow["sSerial"] = dsReturn.Tables["返回值单条"].Rows[0]["附件信息2"];
                                    }
                                    else
                                    {
                                        newRow["FunctionId"] = baoDT.Rows[0]["FunctionId"].ToString();
                                        newRow["ExSerial"] = baoDT.Rows[0]["ExSerial"].ToString();
                                        newRow["ErrorNo"] = dsReturn.Tables["返回值单条"].Rows[0]["附件信息1"];
                                        newRow["ErrorInfo"] = dsReturn.Tables["返回值单条"].Rows[0]["提示文本"];
                                    }
                                    dteRE.Rows.Add(newRow);
                                    #endregion


                                    //转换反馈数据
                                    Hashtable reHT = Get_ByteAndString_From_DataTable(dteRE, false, "");
                                    byte[] reByte = (byte[])(reHT["字节流"]);
                                    string restr = reHT["视觉检测字符串"].ToString();

                                    int r = newSocket.Send(reByte);//发送反馈数据

                                    //newSocket.Close();

                                    #region//开始写日志
                                    //try
                                    //{
                                    string serverDir = System.AppDomain.CurrentDomain.BaseDirectory + "\\Log_BankService_GT\\" + DateTime.Now.ToString("yyyy-MM-dd") + "\\25710\\";
                                    if (!IsExistDirectory(serverDir))
                                        Directory.CreateDirectory(serverDir);
                                    string serverFile = serverDir + DateTime.Now.ToString("HH：mm：ss-fff") + ".txt";
                                    if (!IsExistFile(serverFile))
                                    {
                                        FileStream f = File.Create(serverFile);
                                        f.Close();
                                        f.Dispose();
                                    }
                                    using (StreamWriter sw = new StreamWriter(serverFile, true, System.Text.Encoding.UTF8))
                                    {
                                        if (dsReturn == null)
                                        {
                                            sw.WriteLine("接收到银行发来的包头：" + baotou);
                                            sw.WriteLine("接收到银行发来的包体：" + baoti);
                                            sw.WriteLine("总长度为：" + bytes);
                                            sw.WriteLine("发送到我方服务器进行处理，得到的DataSet为Null");
                                        }
                                        if (dsReturn != null)
                                        {
                                            //sw.WriteLine("包头：" + baotou + "   |   包体：" + baoti + "   |   dsReturn是否为空：非空" + "    |   返回码：" + dsReturn.Tables["返回值单条"].Rows[0]["附件信息1"].ToString() + "   |   " + dsReturn.Tables["返回值单条"].Rows[0]["提示文本"].ToString());
                                            sw.WriteLine("接收到银行发来的包头：" + baotou);
                                            sw.WriteLine("接收到银行发来的包体：" + baoti.Replace(Chr(0), ""));
                                            sw.WriteLine("总长度为：" + bytes);
                                            sw.WriteLine("已接收我方服务器的处理结果。----------");
                                            sw.WriteLine("返回码：" + dsReturn.Tables["返回值单条"].Rows[0]["附件信息1"].ToString());
                                            sw.WriteLine("代码信息：" + dsReturn.Tables["返回值单条"].Rows[0]["提示文本"].ToString());
                                            sw.WriteLine("回馈给银行的数据包为：" + restr);
                                            sw.WriteLine("最终回馈长度为：" + r);
                                        }
                                    }
                                    //}
                                    //catch (Exception exexexe) { throw; }

                                    #endregion

                                    //填充传入参数哈希表
                                    Hashtable OutPutHT = new Hashtable();
                                    OutPutHT["执行状态"] = "回馈成功";//测试参数
                                    OutPutHT["详细信息"] = "执行成功";
                                    if (dsReturn != null)
                                        OutPutHT["测试返回值"] = "接收了[" + gnh + "]请求并反馈了结果。执行结果为：" + dsReturn.Tables["返回值单条"].Rows[0]["附件信息1"].ToString() + "；" + dsReturn.Tables["返回值单条"].Rows[0]["提示文本"].ToString();
                                    else
                                        OutPutHT["测试返回值"] = "接收了[" + gnh + "]请求并反馈了结果。我方服务器返回了Null DataSet";
                                    DForThread(OutPutHT);
                                }
                                break;
                                #endregion
                            case ("23903"): //	银行多文件生成通知（23903）
                                #region 银行多文件生成通知（23903）
                            
                                //若是指令包
                                if (!baoDT.Columns.Contains("ErrorNo"))
                                {
                                    #region//生成应答DataTable
                                    DataTable dteRE;
                                    dteRE = new DataTable("应答");
                                    DataColumn x1 = new DataColumn("FunctionId", typeof(string));//功能号
                                    DataColumn x2 = new DataColumn("ExSerial", typeof(string));//外部流水号
                                    DataColumn x3 = new DataColumn("ErrorNo", typeof(string));//返回码
                                    DataColumn x4 = new DataColumn("ErrorInfo", typeof(string));//返回说明
                                    dteRE.Columns.Add(x1);
                                    dteRE.Columns.Add(x2);
                                    dteRE.Columns.Add(x3);
                                    dteRE.Columns.Add(x4);
                                    DataRow newRow;
                                    #endregion

                                    #region//生成应答数据内容
                                    newRow = dteRE.NewRow();
                                    newRow["FunctionId"] = baoDT.Rows[0]["FunctionId"].ToString();
                                    newRow["ExSerial"] = baoDT.Rows[0]["ExSerial"].ToString();
                                    newRow["ErrorNo"] = "0000";
                                    newRow["ErrorInfo"] = "已正常返回消息";
                                    dteRE.Rows.Add(newRow);
                                    #endregion


                                    //转换反馈数据
                                    Hashtable reHT = Get_ByteAndString_From_DataTable(dteRE, false, "");
                                    byte[] reByte = (byte[])(reHT["字节流"]);
                                    string restr = reHT["视觉检测字符串"].ToString();

                                    int r = newSocket.Send(reByte);//发送反馈数据

                                    //newSocket.Close();

                                    #region//开始写日志
                                    //try
                                    //{
                                    string serverDir = System.AppDomain.CurrentDomain.BaseDirectory + "\\Log_BankService_GT\\" + DateTime.Now.ToString("yyyy-MM-dd") + "\\23903\\";
                                    if (!IsExistDirectory(serverDir))
                                        Directory.CreateDirectory(serverDir);
                                    string serverFile = serverDir + DateTime.Now.ToString("HH：mm：ss-fff") + ".txt";
                                    if (!IsExistFile(serverFile))
                                    {
                                        FileStream f = File.Create(serverFile);
                                        f.Close();
                                        f.Dispose();
                                    }
                                    using (StreamWriter sw = new StreamWriter(serverFile, true, System.Text.Encoding.UTF8))
                                    {
                                        sw.WriteLine("接收到银行发来的包头：" + baotou);
                                        sw.WriteLine("接收到银行发来的包体：" + baoti);
                                        sw.WriteLine("总长度为：" + bytes);
                                    }
                                    //}
                                    //catch (Exception exexexe) { throw; }

                                    #endregion

                                    //填充传入参数哈希表
                                    Hashtable OutPutHT = new Hashtable();
                                    OutPutHT["执行状态"] = "回馈成功";//测试参数
                                    OutPutHT["详细信息"] = "执行成功";
                                    OutPutHT["测试返回值"] = "接收了[" + gnh + "]请求并反馈了结果。这是个银行多文件生成通知。";
                                    DForThread(OutPutHT);
                                }
                                break;
                                #endregion
                            case ("23904"):   //银行单文件生成通知（23904）
                                #region 银行单文件生成通知（23904）
                               
                                //若是指令包
                                if (!baoDT.Columns.Contains("ErrorNo"))
                                {
                                    #region//生成应答DataTable
                                    DataTable dteRE;
                                    dteRE = new DataTable("应答");
                                    DataColumn x1 = new DataColumn("FunctionId", typeof(string));//功能号
                                    DataColumn x2 = new DataColumn("ExSerial", typeof(string));//外部流水号
                                    DataColumn x3 = new DataColumn("ErrorNo", typeof(string));//返回码
                                    DataColumn x4 = new DataColumn("ErrorInfo", typeof(string));//返回说明
                                    dteRE.Columns.Add(x1);
                                    dteRE.Columns.Add(x2);
                                    dteRE.Columns.Add(x3);
                                    dteRE.Columns.Add(x4);
                                    DataRow newRow;
                                    #endregion

                                    #region//生成应答数据内容
                                    newRow = dteRE.NewRow();
                                    newRow["FunctionId"] = baoDT.Rows[0]["FunctionId"].ToString();
                                    newRow["ExSerial"] = baoDT.Rows[0]["ExSerial"].ToString();
                                    newRow["ErrorNo"] = "0000";
                                    newRow["ErrorInfo"] = "已正常返回消息";
                                    dteRE.Rows.Add(newRow);
                                    #endregion


                                    //转换反馈数据
                                    Hashtable reHT = Get_ByteAndString_From_DataTable(dteRE, false, "");
                                    byte[] reByte = (byte[])(reHT["字节流"]);
                                    string restr = reHT["视觉检测字符串"].ToString();

                                    int r = newSocket.Send(reByte);//发送反馈数据

                                    //newSocket.Close();

                                    #region//开始写日志
                                    //try
                                    //{
                                    string serverDir = System.AppDomain.CurrentDomain.BaseDirectory + "\\Log_BankService_GT\\" + DateTime.Now.ToString("yyyy-MM-dd") + "\\23904\\";
                                    if (!IsExistDirectory(serverDir))
                                        Directory.CreateDirectory(serverDir);
                                    string serverFile = serverDir + DateTime.Now.ToString("HH：mm：ss-fff") + ".txt";
                                    if (!IsExistFile(serverFile))
                                    {
                                        FileStream f = File.Create(serverFile);
                                        f.Close();
                                        f.Dispose();
                                    }
                                    using (StreamWriter sw = new StreamWriter(serverFile, true, System.Text.Encoding.UTF8))
                                    {
                                        sw.WriteLine("接收到银行发来的包头：" + baotou);
                                        sw.WriteLine("接收到银行发来的包体：" + baoti);
                                        sw.WriteLine("总长度为：" + bytes);
                                    }
                                    //}
                                    //catch (Exception exexexe) { throw; }

                                    #endregion

                                    //填充传入参数哈希表
                                    Hashtable OutPutHT = new Hashtable();
                                    OutPutHT["执行状态"] = "回馈成功";//测试参数
                                    OutPutHT["详细信息"] = "执行成功";
                                    OutPutHT["测试返回值"] = "接收了[" + gnh + "]请求并反馈了结果。这是个银行单文件生成通知。";
                                    DForThread(OutPutHT);
                                }
                                break;
                                #endregion
                            case ("23704"): //冲证券转银行 
                                #region 冲证券转银行
                              
                                //若是指令包
                                if (!baoDT.Columns.Contains("ErrorNo"))
                                {
                                    #region//生成应答DataTable
                                    DataTable dteRE;
                                    dteRE = new DataTable("应答");
                                    DataColumn x1 = new DataColumn("FunctionId", typeof(string));//功能号
                                    DataColumn x2 = new DataColumn("ExSerial", typeof(string));//外部流水号
                                    DataColumn x3 = new DataColumn("ErrorNo", typeof(string));//返回码
                                    DataColumn x4 = new DataColumn("ErrorInfo", typeof(string));//返回说明
                                    DataColumn x5 = new DataColumn("sSerial", typeof(string));//证券流水号
                                    dteRE.Columns.Add(x1);
                                    dteRE.Columns.Add(x2);
                                    dteRE.Columns.Add(x3);
                                    dteRE.Columns.Add(x4);
                                    dteRE.Columns.Add(x5);
                                    DataRow newRow;
                                    #endregion

                                    #region//执行数据库验证及操作
                                    DataSet dsTo = new DataSet();

                                    dsTo.Tables.Add(baoDT);
                                    // wyh 2014.04.16 add获取客户编号
                                    string Strkhbh = baoDT.Rows[0]["sCustAcct"].ToString().Trim();
                                    object[] cs = { dsTo, Strkhbh, "发起行*浦发银行" };
                                    //wyh end  object[] cs = { dsTo };
                                    
                                    WebServicesCenter2013 wsc = new WebServicesCenter2013();
                                   // DataSet dsReturn = wsc.RunAtServices("CSToBank", cs);
                                    DataSet dsReturn = wsc.RunAtServices("多银行框架-监控冲商转银", cs);
                                    #endregion

                                    #region//生成应答数据内容
                                    newRow = dteRE.NewRow();
                                    if (dsReturn == null || dsReturn.Tables["返回值单条"].Rows.Count<=0)
                                    {
                                        newRow["FunctionId"] = baoDT.Rows[0]["FunctionId"].ToString();
                                        newRow["ExSerial"] = baoDT.Rows[0]["ExSerial"].ToString();
                                        newRow["ErrorNo"] = "8888";
                                        newRow["ErrorInfo"] = "证券方服务器繁忙";
                                    }


                                    else if (dsReturn.Tables["返回值单条"].Rows[0]["执行结果"].ToString() == "ok")
                                    {
                                        newRow["FunctionId"] = baoDT.Rows[0]["FunctionId"].ToString();
                                        newRow["ExSerial"] = baoDT.Rows[0]["ExSerial"].ToString();
                                        newRow["ErrorNo"] = dsReturn.Tables["返回值单条"].Rows[0]["附件信息1"];
                                        newRow["ErrorInfo"] = dsReturn.Tables["返回值单条"].Rows[0]["提示文本"];
                                        newRow["sSerial"] = dsReturn.Tables["返回值单条"].Rows[0]["附件信息2"];
                                    }
                                    else
                                    {
                                        newRow["FunctionId"] = baoDT.Rows[0]["FunctionId"].ToString();
                                        newRow["ExSerial"] = baoDT.Rows[0]["ExSerial"].ToString();
                                        newRow["ErrorNo"] = dsReturn.Tables["返回值单条"].Rows[0]["附件信息1"];
                                        newRow["ErrorInfo"] = dsReturn.Tables["返回值单条"].Rows[0]["提示文本"];
                                    }
                                    dteRE.Rows.Add(newRow);
                                    #endregion


                                    //转换反馈数据
                                    Hashtable reHT = Get_ByteAndString_From_DataTable(dteRE, false, "");
                                    byte[] reByte = (byte[])(reHT["字节流"]);
                                    string restr = reHT["视觉检测字符串"].ToString();

                                    int r = newSocket.Send(reByte);//发送反馈数据

                                    //newSocket.Close();

                                    #region//开始写日志
                                    //try
                                    //{
                                    string serverDir = System.AppDomain.CurrentDomain.BaseDirectory + "\\Log_BankService_GT\\" + DateTime.Now.ToString("yyyy-MM-dd") + "\\25710\\";
                                    if (!IsExistDirectory(serverDir))
                                        Directory.CreateDirectory(serverDir);
                                    string serverFile = serverDir + DateTime.Now.ToString("HH：mm：ss-fff") + ".txt";
                                    if (!IsExistFile(serverFile))
                                    {
                                        FileStream f = File.Create(serverFile);
                                        f.Close();
                                        f.Dispose();
                                    }
                                    using (StreamWriter sw = new StreamWriter(serverFile, true, System.Text.Encoding.UTF8))
                                    {
                                        if (dsReturn == null)
                                        {
                                            sw.WriteLine("接收到银行发来的包头：" + baotou);
                                            sw.WriteLine("接收到银行发来的包体：" + baoti);
                                            sw.WriteLine("总长度为：" + bytes);
                                            sw.WriteLine("发送到我方服务器进行处理，得到的DataSet为Null");
                                        }
                                        if (dsReturn != null)
                                        {
                                            //sw.WriteLine("包头：" + baotou + "   |   包体：" + baoti + "   |   dsReturn是否为空：非空" + "    |   返回码：" + dsReturn.Tables["返回值单条"].Rows[0]["附件信息1"].ToString() + "   |   " + dsReturn.Tables["返回值单条"].Rows[0]["提示文本"].ToString());
                                            sw.WriteLine("接收到银行发来的包头：" + baotou);
                                            sw.WriteLine("接收到银行发来的包体：" + baoti.Replace(Chr(0), ""));
                                            sw.WriteLine("总长度为：" + bytes);
                                            sw.WriteLine("已接收我方服务器的处理结果。----------");
                                            sw.WriteLine("返回码：" + dsReturn.Tables["返回值单条"].Rows[0]["附件信息1"].ToString());
                                            sw.WriteLine("代码信息：" + dsReturn.Tables["返回值单条"].Rows[0]["提示文本"].ToString());
                                            sw.WriteLine("回馈给银行的数据包为：" + restr);
                                            sw.WriteLine("最终回馈长度为：" + r);
                                        }
                                    }
                                    //}
                                    //catch (Exception exexexe) { throw; }

                                    #endregion

                                    //填充传入参数哈希表
                                    Hashtable OutPutHT = new Hashtable();
                                    OutPutHT["执行状态"] = "回馈成功";//测试参数
                                    OutPutHT["详细信息"] = "执行成功";
                                    if (dsReturn != null)
                                        OutPutHT["测试返回值"] = "接收了[" + gnh + "]请求并反馈了结果。执行结果为：" + dsReturn.Tables["返回值单条"].Rows[0]["附件信息1"].ToString() + "；" + dsReturn.Tables["返回值单条"].Rows[0]["提示文本"].ToString();
                                    else
                                        OutPutHT["测试返回值"] = "接收了[" + gnh + "]请求并反馈了结果。我方服务器返回了Null DataSet";
                                    DForThread(OutPutHT);
                                }
                                break;
                                #endregion
                            case ("23703"): //冲银行转证券银行端发起
                                #region 冲银行转证券银行端发起
                               
                                //若是指令包
                                if (!baoDT.Columns.Contains("ErrorNo"))
                                {
                                    #region//生成应答DataTable
                                    DataTable dteRE;
                                    dteRE = new DataTable("应答");
                                    DataColumn x1 = new DataColumn("FunctionId", typeof(string));//功能号
                                    DataColumn x2 = new DataColumn("ExSerial", typeof(string));//外部流水号
                                    DataColumn x3 = new DataColumn("ErrorNo", typeof(string));//返回码
                                    DataColumn x4 = new DataColumn("ErrorInfo", typeof(string));//返回说明
                                    DataColumn x5 = new DataColumn("sSerial", typeof(string));//证券流水号
                                    dteRE.Columns.Add(x1);
                                    dteRE.Columns.Add(x2);
                                    dteRE.Columns.Add(x3);
                                    dteRE.Columns.Add(x4);
                                    dteRE.Columns.Add(x5);
                                    DataRow newRow;
                                    #endregion

                                    #region//执行数据库验证及操作
                                    DataSet dsTo = new DataSet();
                                    dsTo.Tables.Add(baoDT);
                                    string Strkhbh = baoDT.Rows[0]["sCustAcct"].ToString().Trim();
                                    object[] cs = { dsTo, Strkhbh, "发起行*浦发银行" };
                                    WebServicesCenter2013 wsc = new WebServicesCenter2013();
                                   // DataSet dsReturn = wsc.RunAtServices("CBankToS", cs);
                                    DataSet dsReturn = wsc.RunAtServices("多银行框架-监控冲银转商", cs);
                                    #endregion

                                    #region//生成应答数据内容
                                    newRow = dteRE.NewRow();
                                    if (dsReturn == null)
                                    {
                                        newRow["FunctionId"] = baoDT.Rows[0]["FunctionId"].ToString();
                                        newRow["ExSerial"] = baoDT.Rows[0]["ExSerial"].ToString();
                                        newRow["ErrorNo"] = "8888";
                                        newRow["ErrorInfo"] = "证券方服务器繁忙";
                                    }


                                    else if (dsReturn.Tables["返回值单条"].Rows[0]["执行结果"].ToString() == "ok")
                                    {
                                        newRow["FunctionId"] = baoDT.Rows[0]["FunctionId"].ToString();
                                        newRow["ExSerial"] = baoDT.Rows[0]["ExSerial"].ToString();
                                        newRow["ErrorNo"] = dsReturn.Tables["返回值单条"].Rows[0]["附件信息1"];
                                        newRow["ErrorInfo"] = dsReturn.Tables["返回值单条"].Rows[0]["提示文本"];
                                        newRow["sSerial"] = dsReturn.Tables["返回值单条"].Rows[0]["附件信息2"];
                                    }
                                    else
                                    {
                                        newRow["FunctionId"] = baoDT.Rows[0]["FunctionId"].ToString();
                                        newRow["ExSerial"] = baoDT.Rows[0]["ExSerial"].ToString();
                                        newRow["ErrorNo"] = dsReturn.Tables["返回值单条"].Rows[0]["附件信息1"];
                                        newRow["ErrorInfo"] = dsReturn.Tables["返回值单条"].Rows[0]["提示文本"];
                                    }
                                    dteRE.Rows.Add(newRow);
                                    #endregion


                                    //转换反馈数据
                                    Hashtable reHT = Get_ByteAndString_From_DataTable(dteRE, false, "");
                                    byte[] reByte = (byte[])(reHT["字节流"]);
                                    string restr = reHT["视觉检测字符串"].ToString();

                                    int r = newSocket.Send(reByte);//发送反馈数据

                                    //newSocket.Close();

                                    #region//开始写日志
                                    //try
                                    //{
                                    string serverDir = System.AppDomain.CurrentDomain.BaseDirectory + "\\Log_BankService_GT\\" + DateTime.Now.ToString("yyyy-MM-dd") + "\\25710\\";
                                    if (!IsExistDirectory(serverDir))
                                        Directory.CreateDirectory(serverDir);
                                    string serverFile = serverDir + DateTime.Now.ToString("HH：mm：ss-fff") + ".txt";
                                    if (!IsExistFile(serverFile))
                                    {
                                        FileStream f = File.Create(serverFile);
                                        f.Close();
                                        f.Dispose();
                                    }
                                    using (StreamWriter sw = new StreamWriter(serverFile, true, System.Text.Encoding.UTF8))
                                    {
                                        if (dsReturn == null)
                                        {
                                            sw.WriteLine("接收到银行发来的包头：" + baotou);
                                            sw.WriteLine("接收到银行发来的包体：" + baoti);
                                            sw.WriteLine("总长度为：" + bytes);
                                            sw.WriteLine("发送到我方服务器进行处理，得到的DataSet为Null");
                                        }
                                        if (dsReturn != null)
                                        {
                                            //sw.WriteLine("包头：" + baotou + "   |   包体：" + baoti + "   |   dsReturn是否为空：非空" + "    |   返回码：" + dsReturn.Tables["返回值单条"].Rows[0]["附件信息1"].ToString() + "   |   " + dsReturn.Tables["返回值单条"].Rows[0]["提示文本"].ToString());
                                            sw.WriteLine("接收到银行发来的包头：" + baotou);
                                            sw.WriteLine("接收到银行发来的包体：" + baoti.Replace(Chr(0), ""));
                                            sw.WriteLine("总长度为：" + bytes);
                                            sw.WriteLine("已接收我方服务器的处理结果。----------");
                                            sw.WriteLine("返回码：" + dsReturn.Tables["返回值单条"].Rows[0]["附件信息1"].ToString());
                                            sw.WriteLine("代码信息：" + dsReturn.Tables["返回值单条"].Rows[0]["提示文本"].ToString());
                                            sw.WriteLine("回馈给银行的数据包为：" + restr);
                                            sw.WriteLine("最终回馈长度为：" + r);
                                        }
                                    }
                                    //}
                                    //catch (Exception exexexe) { throw; }

                                    #endregion

                                    //填充传入参数哈希表
                                    Hashtable OutPutHT = new Hashtable();
                                    OutPutHT["执行状态"] = "回馈成功";//测试参数
                                    OutPutHT["详细信息"] = "执行成功";
                                    if (dsReturn != null)
                                        OutPutHT["测试返回值"] = "接收了[" + gnh + "]请求并反馈了结果。执行结果为：" + dsReturn.Tables["返回值单条"].Rows[0]["附件信息1"].ToString() + "；" + dsReturn.Tables["返回值单条"].Rows[0]["提示文本"].ToString();
                                    else
                                        OutPutHT["测试返回值"] = "接收了[" + gnh + "]请求并反馈了结果。我方服务器返回了Null DataSet";
                                    DForThread(OutPutHT);
                                }
                                break;
                                #endregion
                            default:
                                #region  
                               
                                if (!baoDT.Columns.Contains("ErrorNo"))
                                {
                                    DataTable dteRE;
                                    dteRE = new DataTable("应答");
                                    DataColumn x1 = new DataColumn("FunctionId", typeof(string));//接口也好
                                    DataColumn x2 = new DataColumn("ExSerial", typeof(string));//外部流水号
                                    DataColumn x3 = new DataColumn("ErrorNo", typeof(string));//返回码
                                    DataColumn x4 = new DataColumn("ErrorInfo", typeof(string));//返回说明
                                    dteRE.Columns.Add(x1);
                                    dteRE.Columns.Add(x2);
                                    dteRE.Columns.Add(x3);
                                    dteRE.Columns.Add(x4);

                                    DataRow newRow;
                                    newRow = dteRE.NewRow();
                                    newRow["FunctionId"] = baoDT.Rows[0]["FunctionId"].ToString();
                                    newRow["ExSerial"] = baoDT.Rows[0]["ExSerial"].ToString();
                                    newRow["ErrorNo"] = "2322";
                                    newRow["ErrorInfo"] = "不支持该业务类别";
                                    //转换反馈数据
                                    Hashtable reHT = Get_ByteAndString_From_DataTable(dteRE, false, "");
                                    byte[] reByte = (byte[])(reHT["字节流"]);
                                    string restr = reHT["视觉检测字符串"].ToString();

                                    int r = newSocket.Send(reByte);//发送反馈数据
                                    //填充传入参数哈希表
                                    Hashtable OutPutHT = new Hashtable();
                                    OutPutHT["执行状态"] = "回馈成功";//测试参数
                                    OutPutHT["详细信息"] = "执行成功";

                                    OutPutHT["测试返回值"] = "接收了[" + gnh + "]请求并反馈了结果。为无效接口号。";
                                    DForThread(OutPutHT);
                                }
                                break;
                                #endregion
                        }
                    }
                }
                catch (Exception e)
                {
                    string serverDir = System.AppDomain.CurrentDomain.BaseDirectory + "\\ERRFile\\" + DateTime.Now.ToString("yyyy-MM-dd") + "\\";
                    if (!IsExistDirectory(serverDir))
                        Directory.CreateDirectory(serverDir);
                    string serverFile = serverDir + DateTime.Now.ToString("HH：mm：ss-fff") + ".txt";
                    if (!IsExistFile(serverFile))
                    {
                        FileStream f = File.Create(serverFile);
                        f.Close();
                        f.Dispose();
                    }
                    using (StreamWriter sw = new StreamWriter(serverFile, true, System.Text.Encoding.GetEncoding("GB2312")))
                    {
                        sw.WriteLine("接收到银行发来的包头：" + baotou);
                        sw.WriteLine("接收到银行发来的包体：" + baoti);
                        sw.WriteLine("总长度为：" + bytes);
                        sw.WriteLine("错误信息：" + e.Message.ToString());
                        sw.Flush();
                    }

                    //填充传入参数哈希表
                    Hashtable OutPutHT = new Hashtable();
                    OutPutHT["执行状态"] = "回馈失败";//测试参数
                    OutPutHT["详细信息"] = "执行失败";
                    OutPutHT["测试返回值"] = e.Message;
                    DForThread(OutPutHT);
                }
            }
        }//结束
    }
}
