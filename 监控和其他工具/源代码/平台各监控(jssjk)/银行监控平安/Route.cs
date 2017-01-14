using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using FMipcClass;
using System.Configuration;

namespace 银行监控平安
{
    /// <summary>
    /// 中继器
    /// </summary>
    public class Route
    {
        delegateForThread DForThread;
        Hashtable InPutHT;
        public Route()
        {
            DForThread = null;
            InPutHT = null;
        }
        public Route(Hashtable PHT, delegateForThread DFT)
        {
            DForThread = DFT;
            InPutHT = PHT;
        }
        /// <summary>
        /// 将数据包发送到银行
        /// </summary>
        public void SendToBank()
        {
            DataTable dtTi = new DataTable();
            dtTi = InPutHT["dtTi"] is DataTable ? InPutHT["dtTi"] as DataTable : null;
            DataTable dtTou = new DataTable();
            dtTou = InPutHT["dtTou"] is DataTable ? InPutHT["dtTou"] as DataTable : null;
            dtTi.TableName = "dtTi";
            dtTou.TableName = "dtTou";

            DataSet ds = new DataSet("ds0");

            ds.Tables.Add(dtTi.Copy());
            ds.Tables.Add(dtTou.Copy());
            if (dtTi == null)
                return;
            Core c = new Core();
            Hashtable ht = c.UsToBank(ds);//获取待发送的数据
            DForThread(ht);
        }

        /// <summary>
        /// 开启socket并发送
        /// </summary>
        public void Send()
        {
            Core c = new Core();
            Hashtable ht = c.Send(InPutHT["ip"].ToString().Trim(), InPutHT["port"].ToString().Trim(), (byte[])InPutHT["Data"]);
            DForThread(ht);
        }

        /// <summary>
        /// 开启监听
        /// </summary>
        public void StartListen()
        {           
            string port = InPutHT["port"].ToString();//端口            
            IPEndPoint serverFullAddr = new IPEndPoint(IPAddress.Any, Convert.ToInt32(port.Trim()));
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                //指定本地主机地址和端口号
                socket.Bind(serverFullAddr);
                socket.Listen(50);
                socket.ReceiveTimeout = 20000; //二十秒没有反馈即为超时
                socket.SendTimeout = 20000;//二十秒没有发送出去，即为超时
                //完成接收服务器启动的回调
                //填充传入参数哈希表
                Hashtable OutPutHT = new Hashtable();
                OutPutHT["执行状态"] = "正在开启监听...";//测试参数
                OutPutHT["详细信息"] = "OK，已经开启端口监听";
                DForThread(OutPutHT);
            }
            catch (Exception se)
            {
                Hashtable OutPutHT = new Hashtable();
                OutPutHT["执行状态"] = "err,端口开启监听失败";//测试参数
                OutPutHT["详细信息"] = se.Message;
                DForThread(OutPutHT);
            }
            Core c = new Core();
            //开始循环收包
            while (true)
            {
                Socket newSocket = socket.Accept();
                newSocket.ReceiveTimeout = 60000;//one min
                newSocket.SendTimeout = 60000;
                byte[] message = new byte[999999];
                int bytes = newSocket.Receive(message);

                Hashtable OutPutHT = new Hashtable();                        
                OutPutHT["str"] = c.GetStr(message);//包字符串

                DataTable dtTou = c.GetBaotou(message);//包头
                OutPutHT["baotou"] = dtTou;
                DataTable dtTi = c.GetBaoti(message, 1);//包体
                OutPutHT["baoti"] = dtTi;
               
                string Qydm = dtTou.Rows[0]["Qydm"].ToString();//交易网代码
                if (Qydm != ConfigurationManager.AppSettings["PingAnQydm"].ToString())
                { //交易网代码不符合的话，直接返回异常
                    run_ex_feedback(newSocket, OutPutHT, bytes);                    
                }
                string FuncId = dtTou.Rows[0]["TranFunc"].ToString();//接口编号

                OutPutHT["执行状态"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 收到银行发来的【"+FuncId+"】接口数据包";      
                OutPutHT["详细信息"] = OutPutHT["str"].ToString();
                DForThread(OutPutHT);
               
                switch (FuncId)
                {                  
                    case "1301"://会员开销户确认  
                        run_1301_feedback(newSocket, OutPutHT, bytes);
                        break;
                    case "1315"://出入金账户维护
                        run_1315_feedback(newSocket, OutPutHT, bytes);
                        break;
                    case "1310"://入金（银行发起）
                        run_1310_feedback(newSocket, OutPutHT, bytes);
                        break;
                    case "1312"://出金（银行发起）
                        run_1312_feedback(newSocket, OutPutHT, bytes);
                        break;
                    case "1019"://查交易网端会员管理账户余额
                        run_1019_feedback(newSocket, OutPutHT, bytes);
                        break;
                    case "1005"://银行通知交易网查看清算文件
                        run_1005_feedback(newSocket, OutPutHT, bytes);
                        break;
                    default:
                        run_ex_feedback(newSocket, OutPutHT, bytes);                        
                        break;
                }
            }
        }

        /// <summary>
        ///处理接口 会员开销户确认【1301】---银行发起
        /// </summary>
        /// <param name="newSocket"></param>
        /// <param name="OutPutHT">包括包头、包体的</param>
        /// <param name="bytes">接收到的包的长度</param>
        private void run_1301_feedback(Socket newSocket, Hashtable OutPutHT, int bytes)
        {
            OutPutHT["执行状态"] = "开始处理【1301】接口数据";
            OutPutHT["详细信息"] = "正在准备应答包...";
            DForThread(OutPutHT);

            DataTable dtTou = (DataTable)OutPutHT["baotou"];
            DataTable dtTi = (DataTable)OutPutHT["baoti"];

            Core c = new Core();
            //构建应答包体的结构
            string xmlFile = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\XML\" + c.GetXMLHeadStr(2) + "1301.xml";//XML文件
            DataTable dtReTi = new DataTable();
            dtReTi.ReadXml(xmlFile);
            //设置应答包体的字段值
            dtReTi.Rows[0]["ThirdLogNo"] = "";
            dtReTi.Rows[0]["Reserve"] = "";

            //应答包的包头
            DataTable dtReTou = dtTou.Copy();

            //调用后台接口，对银行发过来的数据进行验证
            DataSet dsTo = new DataSet();
            dtTou.TableName = "dtTou";
            dtTi.TableName = "dtTi";
            dsTo.Tables.Add(dtTou);
            dsTo.Tables.Add(dtTi);
            string khbh = dtTi.Rows[0]["ThirdCustId"].ToString();
            object[] cs = { dsTo, khbh,"发起行*平安银行" };
            //WebServicesCenter wsc = new WebServicesCenter();
            //DataSet dsReturn = wsc.RunAtServices("CompareUInfo", cs);
            DataSet dsReturn=new DataSet ();
            object[] objRe = IPC.Call("多银行框架-银行发起客户签约", cs);
            if (objRe[0].ToString() == "ok")
            {
                dsReturn = (DataSet)objRe[1];
            }
            else
            {
                dsReturn = null;
                OutPutHT["执行状态"] = "调用后台方法出现异常！";
                OutPutHT["详细信息"] = "异常描述：" + objRe[1].ToString();
                DForThread(OutPutHT);
            }

            //根据后台处理结果构建应答包
            dtReTou.Rows[0]["ServType"] = "02";//02表示应答
            if (dsReturn == null)
            {
                //设置包头的应答码为失败，包体为空
                dtReTou.Rows[0]["RspCode"] = "ERR074";
                dtReTou.Rows[0]["RspMsg"] = "交易网处理失败";               
            }
            else if (dsReturn.Tables["返回值单条"].Rows[0]["执行结果"].ToString() == "ok")
            {
                //设置包头的应答码
                dtReTou.Rows[0]["RspCode"] = "000000";
                dtReTou.Rows[0]["RspMsg"] = "交易成功";
                //设置应答包体的字段值
                dtReTi.Rows[0]["ThirdLogNo"] = dsReturn.Tables["返回值单条"].Rows[0]["附件信息2"].ToString();//附件信息2内容是流水号
                dtReTi.Rows[0]["Reserve"] = "";
            }
            else
            {
                //设置包头的应答码为对应的失败码，包体为空。执行失败时，提示文本为失败描述，应答码统一用ERR074
                dtReTou.Rows[0]["RspCode"] = "ERR074";
                dtReTou.Rows[0]["RspMsg"] = dsReturn.Tables["返回值单条"].Rows[0]["提示文本"].ToString();
            }
            //设置包头中包体的长度           
            int baotiLength = c.GetBaotiLength(dtReTi);//包体长度
            dtReTou.Rows[0]["Length"] = baotiLength;
            dtReTou.Rows[0]["TranTime"] = DateTime.Now.ToString("HHmmss");

            DataSet dsRe = new DataSet();
            dtReTou.TableName = "dtTou";
            dtReTi.TableName = "dtTi";
            dsRe.Tables.Add(dtReTou);
            dsRe.Tables.Add(dtReTi);
            Hashtable ht = c.UsToBank(dsRe);//3、将包头包体拼装成字符串，然后转化成byte流
            byte[] reByte = (byte[])ht["byteAry"];//要发给银行的数据包
            try
            {
                //发送应答包
                int r = newSocket.Send(reByte);
                OutPutHT["执行状态"] = "应答成功";
                OutPutHT["详细信息"] = OutPutHT["详细信息"] = "应答码：" + dtReTou.Rows[0]["RspCode"].ToString() + ";应答描述：" + dtReTou.Rows[0]["RspMsg"].ToString();
            }
            catch (Exception ex)
            {
                OutPutHT["执行状态"] = "应答失败";
                OutPutHT["详细信息"] = ex.ToString();
            }
            DForThread(OutPutHT);

            #region 写日志
            // try
            //{
            string serverDir = System.AppDomain.CurrentDomain.BaseDirectory + "\\Log_BankService_PingAn\\" + DateTime.Now.ToString("yyyy-MM-dd") + "\\1301\\";
            if (!Directory.Exists(serverDir))
            {
                Directory.CreateDirectory(serverDir);
            }
            string serverFile = serverDir + DateTime.Now.ToString("HHmmss-fff") + ".txt";
            if (!File.Exists(serverFile))
            {
                FileStream f = File.Create(serverFile);
                f.Close();
                f.Dispose();
            }
            using (StreamWriter sw = new StreamWriter(serverFile, true, System.Text.Encoding.UTF8))
            {
                if (dsReturn == null)
                {
                    sw.WriteLine("接收到银行发来的包：" + OutPutHT["str"].ToString());
                    sw.WriteLine("总长度为：" + bytes);
                    sw.WriteLine("发送到我方服务器进行处理，得到的DataSet为Null。异常描述：" + objRe[1].ToString());
                }
                else
                {
                    sw.WriteLine("接收到银行发来的包：" + OutPutHT["str"].ToString());
                    sw.WriteLine("总长度为：" + bytes);
                    sw.WriteLine("我方服务器的处理结果----------");
                    sw.WriteLine("返回码：" + dtReTou.Rows[0]["RspCode"].ToString());
                    sw.WriteLine("代码信息：" + dtReTou.Rows[0]["RspMsg"].ToString());
                    sw.WriteLine("回馈给银行的数据包：" + ht["bao"].ToString());                   
                }
            }
            //}
            //catch (Exception exexexe) { throw; }
            #endregion

            //返回结果 
            OutPutHT["执行状态"] = "写日志成功";
            OutPutHT["详细信息"] = "【1301】接口处理完毕";
            DForThread(OutPutHT);
        }

        /// <summary>
        ///处理接口 出入金账户维护【1315】---银行发起
        /// </summary>
        /// <param name="newSocket"></param>
        /// <param name="OutPutHT">包括包头、包体的</param>
        /// <param name="bytes">接收到的包的长度</param>
        private void run_1315_feedback(Socket newSocket, Hashtable OutPutHT, int bytes)
        {
            OutPutHT["执行状态"] = "开始处理【1315】接口数据";
            OutPutHT["详细信息"] = "正在准备应答包...";
            DForThread(OutPutHT);

            DataTable dtTou = (DataTable)OutPutHT["baotou"];
            DataTable dtTi = (DataTable)OutPutHT["baoti"];
            Core c = new Core();
            //构建应答包体的结构
            string xmlFile = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\XML\" + c.GetXMLHeadStr(2) + "1315.xml";//XML文件
            DataTable dtReTi = new DataTable();
            dtReTi.ReadXml(xmlFile);
            //设置应答包体的字段值
            dtReTi.Rows[0]["ThirdLogNo"] = "";
            dtReTi.Rows[0]["Reserve"] = "";

            //应答包的包头
            DataTable dtReTou = dtTou.Copy();

            //调用后台接口，对银行发过来的数据进行验证
            DataSet dsTo = new DataSet();
            dtTou.TableName = "dtTou";
            dtTi.TableName = "dtTi";
            dsTo.Tables.Add(dtTou);
            dsTo.Tables.Add(dtTi);
            string khbh = dtTi.Rows[0]["Reserve"].ToString().Length > 32 ? dtTi.Rows[0]["Reserve"].ToString().Substring(0, 31).Trim() : dtTi.Rows[0]["Reserve"].ToString().Trim();//保留域的前32位是交易网会员代码，必输。
            
            object[] cs = { dsTo, khbh, "发起行*平安银行" };
            //调用后台的处理方法。因为浦发中账号绑定和开户是一块的，所有都调用一个方法。
            //WebServicesCenter wsc = new WebServicesCenter();
            DataSet dsReturn = new DataSet();
            object[] objRe = new object[] { };
            if (dtTi.Rows[0]["FuncFlag"].ToString() == "2")
            { //修改操作
                 //dsReturn = wsc.RunAtServices("BankRequest_UpdateBankAccount", cs);
                 objRe = IPC.Call("多银行框架-银行发起变更银行账户", cs);                 
            }
            else
            {//其余类型的操作（指定、删除）
                //dsReturn = wsc.RunAtServices("CompareUInfo", cs);
                objRe = IPC.Call("多银行框架-银行发起客户签约", cs);
            }
            if (objRe[0].ToString() == "ok")
            {
                dsReturn = (DataSet)objRe[1];
            }
            else
            {
                dsReturn = null;
                OutPutHT["执行状态"] = "调用后台方法出现异常！";
                OutPutHT["详细信息"] = "异常描述：" + objRe[1];
                DForThread(OutPutHT);
            }

            //根据后台处理结果构建应答包
            dtReTou.Rows[0]["ServType"] = "02";//02表示应答
            if (dsReturn == null)
            {
                //设置包头的应答码为失败，包体为空
                dtReTou.Rows[0]["RspCode"] = "ERR074";
                dtReTou.Rows[0]["RspMsg"] = "交易网处理失败";
            }
            else if (dsReturn.Tables["返回值单条"].Rows[0]["执行结果"].ToString() == "ok")
            {
                //设置包头的应答码
                dtReTou.Rows[0]["RspCode"] = "000000";
                dtReTou.Rows[0]["RspMsg"] = "交易成功";
                //设置应答包体的字段值
                dtReTi.Rows[0]["ThirdLogNo"] = dsReturn.Tables["返回值单条"].Rows[0]["附件信息2"].ToString();
                dtReTi.Rows[0]["Reserve"] = "";//附件信息2内容是流水号              
            }
            else
            {
                if (dsReturn.Tables["返回值单条"].Rows[0]["提示文本"].ToString() == "该客户不存在")
                {
                    dtReTou.Rows[0]["RspCode"] = "ERR074";
                    dtReTou.Rows[0]["RspMsg"] = "交易网会员代码输入错误";
                }
                else
                {
                    //设置包头的应答码为对应的失败码，包体为空。执行失败时，附件信息1为失败码，附件信息2为失败描述
                    dtReTou.Rows[0]["RspCode"] = dsReturn.Tables["返回值单条"].Rows[0]["附件信息1"].ToString();
                    dtReTou.Rows[0]["RspMsg"] = dsReturn.Tables["返回值单条"].Rows[0]["提示文本"].ToString();
                }
            }
            //设置包头中包体的长度           
            int baotiLength = c.GetBaotiLength(dtReTi);//包体长度
            dtReTou.Rows[0]["Length"] = baotiLength;
            dtReTou.Rows[0]["TranTime"] = DateTime.Now.ToString("HHmmss");

            DataSet dsRe = new DataSet();
            dtReTou.TableName = "dtTou";
            dtReTi.TableName = "dtTi";
            dsRe.Tables.Add(dtReTou);
            dsRe.Tables.Add(dtReTi);
            Hashtable ht = c.UsToBank(dsRe);//3、将包头包体拼装成字符串，然后转化成byte流
            byte[] reByte = (byte[])ht["byteAry"];//要发给银行的数据包
            try
            {
                //发送应答包
                int r = newSocket.Send(reByte);
                OutPutHT["执行状态"] = "应答成功";
                OutPutHT["详细信息"] = OutPutHT["详细信息"] = "应答码：" + dtReTou.Rows[0]["RspCode"].ToString() + ";应答描述：" + dtReTou.Rows[0]["RspMsg"].ToString();
            }
            catch (Exception ex)
            {
                OutPutHT["执行状态"] = "应答失败";
                OutPutHT["详细信息"] = ex.ToString();
            }
            DForThread(OutPutHT);

            #region 写日志
            // try
            //{
            string serverDir = System.AppDomain.CurrentDomain.BaseDirectory + "\\Log_BankService_PingAn\\" + DateTime.Now.ToString("yyyy-MM-dd") + "\\1315\\";
            if (!Directory.Exists(serverDir))
            {
                Directory.CreateDirectory(serverDir);
            }
            string serverFile = serverDir + DateTime.Now.ToString("HHmmss-fff") + ".txt";
            if (!File.Exists(serverFile))
            {
                FileStream f = File.Create(serverFile);
                f.Close();
                f.Dispose();
            }
            using (StreamWriter sw = new StreamWriter(serverFile, true, System.Text.Encoding.UTF8))
            {
                if (dsReturn == null)
                {
                    sw.WriteLine("接收到银行发来的包：" + OutPutHT["str"].ToString());
                    sw.WriteLine("总长度为：" + bytes);
                    sw.WriteLine("发送到我方服务器进行处理，得到的DataSet为Null。异常描述：" + objRe[1].ToString());
                }
                else
                {
                    sw.WriteLine("接收到银行发来的包：" + OutPutHT["str"].ToString());
                    sw.WriteLine("总长度为：" + bytes);
                    sw.WriteLine("我方服务器的处理结果----------");
                    sw.WriteLine("应答码：" + dtReTou.Rows[0]["RspCode"].ToString());
                    sw.WriteLine("代码信息：" + dtReTou.Rows[0]["RspMsg"].ToString());
                    sw.WriteLine("回馈给银行的数据包：" + ht["bao"].ToString());
                }
            }
            //}
            //catch (Exception exexexe) { throw; }
            #endregion

            //返回结果 
            OutPutHT["执行状态"] = "写日志成功";
            OutPutHT["详细信息"] = "【1315】接口处理完毕";
            DForThread(OutPutHT);
        }

        /// <summary>
        ///处理接口 入金【1310】---银行发起
        /// </summary>
        /// <param name="newSocket"></param>
        /// <param name="OutPutHT">包括包头、包体的</param>
        /// <param name="bytes">接收到的包的长度</param>
        private void run_1310_feedback(Socket newSocket, Hashtable OutPutHT, int bytes)
        {
            OutPutHT["执行状态"] = "开始处理【1310】接口数据";
            OutPutHT["详细信息"] = "正在准备应答包...";
            DForThread(OutPutHT);

            DataTable dtTou = (DataTable)OutPutHT["baotou"];
            DataTable dtTi = (DataTable)OutPutHT["baoti"];

            Core c = new Core();
            //构建应答包体的结构
            string xmlFile = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\XML\" + c.GetXMLHeadStr(2) + "1310.xml";//XML文件
            DataTable dtReTi = new DataTable();
            dtReTi.ReadXml(xmlFile);
            //设置包体默认值
            dtReTi.Rows[0]["ThirdLogNo"] = "";
            dtReTi.Rows[0]["Reserve"] = "";

            //应答包的包头
            DataTable dtReTou = dtTou.Copy();

            //调用后台接口，对银行发过来的数据进行验证
            DataSet dsTo = new DataSet();
            dtTou.TableName = "dtTou";
            dtTi.TableName = "dtTi";
            dsTo.Tables.Add(dtTou);
            dsTo.Tables.Add(dtTi);
            string khbh = dtTi.Rows[0]["Reserve"].ToString().Length > 32 ? dtTi.Rows[0]["Reserve"].ToString().Substring(0, 31).Trim() : dtTi.Rows[0]["Reserve"].ToString().Trim();//保留域的前32位是交易网会员代码，必输。
            object[] cs = { dsTo, khbh, "发起行*平安银行" };
            //WebServicesCenter wsc = new WebServicesCenter();
            //DataSet dsReturn = wsc.RunAtServices("BankToS", cs);
            DataSet dsReturn = new DataSet();
            object[] objRe = IPC.Call("多银行框架-银行发起银转商", cs);
            if (objRe[0].ToString() == "ok")
            {
                dsReturn = (DataSet)objRe[1];
            }
            else
            {
                dsReturn = null;
                OutPutHT["执行状态"] = "调用后台方法出现异常！";
                OutPutHT["详细信息"] = "异常描述：" + objRe[1].ToString();
                DForThread(OutPutHT);
            }

            //根据后台处理结果构建应答包
            dtReTou.Rows[0]["ServType"] = "02";//02表示应答
            if (dsReturn == null)
            {
                //设置包头的应答码为失败，包体为空
                dtReTou.Rows[0]["RspCode"] = "ERR074";
                dtReTou.Rows[0]["RspMsg"] = "交易网处理失败";
            }
            else if (dsReturn.Tables["返回值单条"].Rows[0]["执行结果"].ToString() == "ok")
            {
                //设置包头的应答码
                dtReTou.Rows[0]["RspCode"] = "000000";
                dtReTou.Rows[0]["RspMsg"] = "交易成功";
                
                //设置应答包体的字段值
                dtReTi.Rows[0]["ThirdLogNo"] = dsReturn.Tables["返回值单条"].Rows[0]["附件信息2"].ToString();
                dtReTi.Rows[0]["Reserve"] = "";//附件信息2内容是流水号                
            }
            else
            {
                if (dsReturn.Tables["返回值单条"].Rows[0]["提示文本"].ToString() == "该客户不存在")
                {
                    dtReTou.Rows[0]["RspCode"] = "ERR074";
                    dtReTou.Rows[0]["RspMsg"] = "交易网会员代码输入错误";
                }
                else
                {
                    //设置包头的应答码为对应的失败码，包体为空。附件信息1为失败码
                    dtReTou.Rows[0]["RspCode"] = dsReturn.Tables["返回值单条"].Rows[0]["附件信息1"].ToString();
                    dtReTou.Rows[0]["RspMsg"] = dsReturn.Tables["返回值单条"].Rows[0]["提示文本"].ToString();
                }
            }
            //设置包头中包体的长度           
            int baotiLength = c.GetBaotiLength(dtReTi);//包体长度
            dtReTou.Rows[0]["Length"] = baotiLength;
            dtReTou.Rows[0]["TranTime"] = DateTime.Now.ToString("HHmmss");

            DataSet dsRe = new DataSet();
            dtReTou.TableName = "dtTou";
            dtReTi.TableName = "dtTi";
            dsRe.Tables.Add(dtReTou);
            dsRe.Tables.Add(dtReTi);
            Hashtable ht = c.UsToBank(dsRe);//3、将包头包体拼装成字符串，然后转化成byte流
            byte[] reByte = (byte[])ht["byteAry"];//要发给银行的数据包
            try
            {
                //发送应答包
                int r = newSocket.Send(reByte);
                OutPutHT["执行状态"] = "应答成功";
                OutPutHT["详细信息"] = OutPutHT["详细信息"] = "应答码：" + dtReTou.Rows[0]["RspCode"].ToString() + ";应答描述：" + dtReTou.Rows[0]["RspMsg"].ToString();
            }
            catch (Exception ex)
            {
                OutPutHT["执行状态"] = "应答失败";
                OutPutHT["详细信息"] = ex.ToString();
            }
            DForThread(OutPutHT);

            #region 写日志
            // try
            //{
            string serverDir = System.AppDomain.CurrentDomain.BaseDirectory + "\\Log_BankService_PingAn\\" + DateTime.Now.ToString("yyyy-MM-dd") + "\\1310\\";
            if (!Directory.Exists(serverDir))
            {
                Directory.CreateDirectory(serverDir);
            }
            string serverFile = serverDir + DateTime.Now.ToString("HHmmss-fff") + ".txt";
            if (!File.Exists(serverFile))
            {
                FileStream f = File.Create(serverFile);
                f.Close();
                f.Dispose();
            }
            using (StreamWriter sw = new StreamWriter(serverFile, true, System.Text.Encoding.UTF8))
            {
                if (dsReturn == null)
                {
                    sw.WriteLine("接收到银行发来的包：" + OutPutHT["str"].ToString());
                    sw.WriteLine("总长度为：" + bytes);
                    sw.WriteLine("发送到我方服务器进行处理，得到的DataSet为Null。异常描述：" + objRe[1].ToString());
                }
                else
                {
                    sw.WriteLine("接收到银行发来的包：" + OutPutHT["str"].ToString());
                    sw.WriteLine("总长度为：" + bytes);
                    sw.WriteLine("我方服务器的处理结果----------");
                    sw.WriteLine("返回码：" + dtReTou.Rows[0]["RspCode"].ToString());
                    sw.WriteLine("代码信息：" + dtReTou.Rows[0]["RspMsg"].ToString());
                    sw.WriteLine("回馈给银行的数据包：" + ht["bao"].ToString());                   
                }
            }
            //}
            //catch (Exception exexexe) { throw; }
            #endregion

            //返回结果 
            OutPutHT["执行状态"] = "写日志成功";
            OutPutHT["详细信息"] = "【1310】接口处理完毕";
            DForThread(OutPutHT);
        }

        /// <summary>
        ///处理接口 出金【1312】---银行发起
        /// </summary>
        /// <param name="newSocket"></param>
        /// <param name="OutPutHT">包括包头、包体的</param>
        /// <param name="bytes">接收到的包的长度</param>
        private void run_1312_feedback(Socket newSocket, Hashtable OutPutHT, int bytes)
        {
            OutPutHT["执行状态"] = "开始处理【1312】接口数据";
            OutPutHT["详细信息"] = "正在准备应答包...";
            DForThread(OutPutHT);

            DataTable dtTou = (DataTable)OutPutHT["baotou"];
            DataTable dtTi = (DataTable)OutPutHT["baoti"];

            Core c = new Core();
            //构建应答包体的结构
            string xmlFile = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\XML\" + c.GetXMLHeadStr(2) + "1312.xml";//XML文件
            DataTable dtReTi = new DataTable();
            dtReTi.ReadXml(xmlFile);
            //设置包体默认值
            dtReTi.Rows[0]["ThirdLogNo"] = "";
            dtReTi.Rows[0]["Reserve"] = "";

            //应答包的包头
            DataTable dtReTou = dtTou.Copy();

            //调用后台接口，对银行发过来的数据进行验证
            DataSet dsTo = new DataSet();
            dtTou.TableName = "dtTou";
            dtTi.TableName = "dtTi";
            dsTo.Tables.Add(dtTou);
            dsTo.Tables.Add(dtTi);
            string khbh = dtTi.Rows[0]["ThirdCustId"].ToString().Trim();
            object[] cs = { dsTo, khbh, "发起行*平安银行" };
            //WebServicesCenter wsc = new WebServicesCenter();
            //DataSet dsReturn = wsc.RunAtServices("SToBank", cs);
            DataSet dsReturn = new DataSet();
            object[] objRe = IPC.Call("多银行框架-银行发起商转银", cs);
            if (objRe[0].ToString() == "ok")
            {
                dsReturn = (DataSet)objRe[1];
            }
            else
            {
                dsReturn = null;
                OutPutHT["执行状态"] = "调用后台方法出现异常！";
                OutPutHT["详细信息"] = "异常描述：" + objRe[1].ToString();
                DForThread(OutPutHT);
            }

            //根据后台处理结果构建应答包
            dtReTou.Rows[0]["ServType"] = "02";//02表示应答
            if (dsReturn == null)
            {
                //设置包头的应答码为失败，包体为空
                dtReTou.Rows[0]["RspCode"] = "ERR074";
                dtReTou.Rows[0]["RspMsg"] = "交易网处理失败";
            }
            else if (dsReturn.Tables["返回值单条"].Rows[0]["执行结果"].ToString() == "ok")
            {
                //设置包头的应答码
                dtReTou.Rows[0]["RspCode"] = "000000";
                dtReTou.Rows[0]["RspMsg"] = "交易成功";

                //设置应答包体的字段值
                dtReTi.Rows[0]["ThirdLogNo"] = dsReturn.Tables["返回值单条"].Rows[0]["附件信息2"].ToString();
                dtReTi.Rows[0]["Reserve"] = "";//附件信息2内容是流水号               
            }
            else
            {
                if (dsReturn.Tables["返回值单条"].Rows[0]["提示文本"].ToString() == "该客户不存在")
                {
                    dtReTou.Rows[0]["RspCode"] = "ERR074";
                    dtReTou.Rows[0]["RspMsg"] = "交易网会员代码输入错误";
                }
                else
                {
                    //设置包头的应答码为对应的失败码，包体为空。附件信息1为失败码
                    dtReTou.Rows[0]["RspCode"] = dsReturn.Tables["返回值单条"].Rows[0]["附件信息1"].ToString();
                    dtReTou.Rows[0]["RspMsg"] = dsReturn.Tables["返回值单条"].Rows[0]["提示文本"].ToString();
                }
            }
            //设置包头中包体的长度           
            int baotiLength = c.GetBaotiLength(dtReTi);//包体长度
            dtReTou.Rows[0]["Length"] = baotiLength;
            dtReTou.Rows[0]["TranTime"] = DateTime.Now.ToString("HHmmss");

            DataSet dsRe = new DataSet();
            dtReTou.TableName = "dtTou";
            dtReTi.TableName = "dtTi";
            dsRe.Tables.Add(dtReTou);
            dsRe.Tables.Add(dtReTi);
            Hashtable ht = c.UsToBank(dsRe);//3、将包头包体拼装成字符串，然后转化成byte流
            byte[] reByte = (byte[])ht["byteAry"];//要发给银行的数据包
            try
            {
                //发送应答包
                int r = newSocket.Send(reByte);
                OutPutHT["执行状态"] = "应答成功";
                OutPutHT["详细信息"] = OutPutHT["详细信息"] = "应答码：" + dtReTou.Rows[0]["RspCode"].ToString() + ";应答描述：" + dtReTou.Rows[0]["RspMsg"].ToString();
            }
            catch (Exception ex)
            {
                OutPutHT["执行状态"] = "应答失败";
                OutPutHT["详细信息"] = ex.ToString();
            }
            DForThread(OutPutHT);

            #region 写日志
            // try
            //{
            string serverDir = System.AppDomain.CurrentDomain.BaseDirectory + "\\Log_BankService_PingAn\\" + DateTime.Now.ToString("yyyy-MM-dd") + "\\1312\\";
            if (!Directory.Exists(serverDir))
            {
                Directory.CreateDirectory(serverDir);
            }
            string serverFile = serverDir + DateTime.Now.ToString("HHmmss-fff") + ".txt";
            if (!File.Exists(serverFile))
            {
                FileStream f = File.Create(serverFile);
                f.Close();
                f.Dispose();
            }
            using (StreamWriter sw = new StreamWriter(serverFile, true, System.Text.Encoding.UTF8))
            {
                if (dsReturn == null)
                {
                    sw.WriteLine("接收到银行发来的包：" + OutPutHT["str"].ToString());
                    sw.WriteLine("总长度为：" + bytes);
                    sw.WriteLine("发送到我方服务器进行处理，得到的DataSet为Null。异常描述：" + objRe[1].ToString());
                }
                else
                {
                    sw.WriteLine("接收到银行发来的包：" + OutPutHT["str"].ToString());
                    sw.WriteLine("总长度为：" + bytes);
                    sw.WriteLine("我方服务器的处理结果----------");
                    sw.WriteLine("应答码：" + dtReTou.Rows [0]["RspCode"].ToString ());
                    sw.WriteLine("代码信息：" + dtReTou.Rows[0]["RspMsg"].ToString());
                    sw.WriteLine("回馈给银行的数据包：" + ht["bao"].ToString());                    
                }
            }
            //}
            //catch (Exception exexexe) { throw; }
            #endregion

            //返回结果 
            OutPutHT["执行状态"] = "写日志成功";
            OutPutHT["详细信息"] ="【1312】接口处理完毕" ;
            DForThread(OutPutHT);
        }

        /// <summary>
        /// 处理接口 查交易网端会员资金台帐余额【1019】
        /// </summary>
        /// <param name="newSocket"></param>
        /// <param name="OutPutHT"></param>
        /// <param name="bytes"></param>
        private void run_1019_feedback(Socket newSocket, Hashtable OutPutHT, int bytes)
        {
            OutPutHT["执行状态"] = "开始处理【1019】接口数据";
            OutPutHT["详细信息"] = "正在准备应答包...";
            DForThread(OutPutHT);
            DataTable dtTou = (DataTable)OutPutHT["baotou"];
            DataTable dtTi = (DataTable)OutPutHT["baoti"];
            Core c = new Core();
            //构建应答包体的结构
            string xmlFile = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\XML\" + c.GetXMLHeadStr(2) + "1019.xml";//XML文件
            DataTable dtReTi = new DataTable();
            dtReTi.ReadXml(xmlFile);
            //设置包体默认值
            dtReTi.Rows[0]["CustAcctId"] = "";
            dtReTi.Rows[0]["ThirdCustId"] = "";
            dtReTi.Rows[0]["CustName"] = "";
            dtReTi.Rows[0]["TotalAmount"] = "";
            dtReTi.Rows[0]["TotalBalance"] = "";
            dtReTi.Rows[0]["TotalFreezeAmount"] = "";
            dtReTi.Rows[0]["TranDate"] = "";
            dtReTi.Rows[0]["Reserve"] = "";

            //应答包的包头
            DataTable dtReTou = dtTou.Copy();

            //调用后台接口，对银行发过来的数据进行验证
            DataSet dsTo = new DataSet();
            dtTou.TableName = "dtTou";
            dtTi.TableName = "dtTi";
            dsTo.Tables.Add(dtTou);
            dsTo.Tables.Add(dtTi);

            string CustAcctId = dtTi.Rows[0]["CustAcctId"].ToString().Trim();//子账户账号
            string ThirdCustId = dtTi.Rows[0]["ThirdCustId"].ToString().Trim();//交易网会员代码
            string khbh = "";
            //WebServicesCenter wsc = new WebServicesCenter();
            if (ThirdCustId != "")
            {
                khbh = ThirdCustId;
            }
            else
            { //调用后台方法获取客户编号                
                //DataSet dskhbh = wsc.RunAtServices("PingAn_GetCustId", new object[] { CustAcctId });  
                DataSet dskhbh = new DataSet();
                object[] objRekhbh = IPC.Call("平安银行-获取客户编号", new object[] { CustAcctId });
                if (objRekhbh[0].ToString() == "ok")
                {
                    dskhbh = (DataSet)objRekhbh[1];
                }
                else
                {
                    dskhbh = null;
                }
                if (dskhbh != null && dskhbh.Tables[0].Rows.Count > 0)
                {
                    khbh = dskhbh.Tables["返回值单条"].Rows[0]["附件信息1"].ToString();
                    dsTo.Tables["dtTi"].Rows[0]["ThirdCustId"] = khbh;
                }
            }

            //调用后台的处理方法
            object[] cs = { dsTo, khbh, "发起行*平安银行" };
            //DataSet dsReturn = wsc.RunAtServices("GetUMoneyByBANK", cs);
            DataSet dsReturn = new DataSet();
            object[] objRe = IPC.Call("多银行框架-银行查客户余额", cs);
            if (objRe[0].ToString() == "ok")
            {
                dsReturn = (DataSet)objRe[1];
            }
            else
            {
                dsReturn = null;
                OutPutHT["执行状态"] = "调用后台方法出现异常！";
                OutPutHT["详细信息"] = "异常描述:" + objRe[1].ToString();
                DForThread(OutPutHT);
            }

            //根据后台处理结果构建应答包
            dtReTou.Rows[0]["ServType"] = "02";//02表示应答
            if (dsReturn == null)
            {
                //设置包头的应答码为失败，包体为空
                dtReTou.Rows[0]["RspCode"] = "ERR074";
                dtReTou.Rows[0]["RspMsg"] = "交易网处理失败";
            }
            else if (dsReturn.Tables["返回值单条"].Rows[0]["执行结果"].ToString() == "ok")
            {
                //设置包头的应答码
                dtReTou.Rows[0]["RspCode"] = "000000";
                dtReTou.Rows[0]["RspMsg"] = "交易成功";
                //设置应答包体的字段值             
                //执行成功时，“data”表中内容是应答包内容
                dtReTi.Rows[0]["CustAcctId"] = dsReturn.Tables["data"].Rows[0]["CustAcctId"].ToString();
                dtReTi.Rows[0]["ThirdCustId"] = dsReturn.Tables["data"].Rows[0]["ThirdCustId"].ToString();
                dtReTi.Rows[0]["CustName"] = dsReturn.Tables["data"].Rows[0]["CustName"].ToString();
                dtReTi.Rows[0]["TotalAmount"] = dsReturn.Tables["data"].Rows[0]["TotalAmount"].ToString();
                dtReTi.Rows[0]["TotalBalance"] = dsReturn.Tables["data"].Rows[0]["TotalBalance"].ToString();
                dtReTi.Rows[0]["TotalFreezeAmount"] = dsReturn.Tables["data"].Rows[0]["TotalFreezeAmount"].ToString();
                dtReTi.Rows[0]["TranDate"] = dsReturn.Tables["data"].Rows[0]["TranDate"].ToString();
            }
            else
            {
                if (dsReturn.Tables["返回值单条"].Rows[0]["提示文本"].ToString() == "该客户不存在")
                {
                    dtReTou.Rows[0]["RspCode"] = "ERR074";
                    dtReTou.Rows[0]["RspMsg"] = "子账户不存在";
                }
                else
                {
                    //设置包头的应答码为对应的失败码，包体为空。执行失败时，附件信息1为失败码，提示文本为失败描述
                    dtReTou.Rows[0]["RspCode"] = dsReturn.Tables["返回值单条"].Rows[0]["附件信息1"].ToString();
                    dtReTou.Rows[0]["RspMsg"] = dsReturn.Tables["返回值单条"].Rows[0]["提示文本"].ToString();
                }
            }

            //设置包头中包体的长度           
            int baotiLength = c.GetBaotiLength(dtReTi);//包体长度
            dtReTou.Rows[0]["Length"] = baotiLength;
            dtReTou.Rows[0]["TranTime"] = DateTime.Now.ToString("HHmmss");

            DataSet dsRe = new DataSet();
            dtReTou.TableName = "dtTou";
            dtReTi.TableName = "dtTi";
            dsRe.Tables.Add(dtReTou);
            dsRe.Tables.Add(dtReTi);
            Hashtable ht = c.UsToBank(dsRe);//3、将包头包体拼装成字符串，然后转化成byte流
            byte[] reByte = (byte[])ht["byteAry"];//要发给银行的数据包
            try
            {
                //发送应答包
                int r = newSocket.Send(reByte);
                OutPutHT["执行状态"] = "应答成功";
                OutPutHT["详细信息"] = OutPutHT["详细信息"] = "应答码：" + dtReTou.Rows[0]["RspCode"].ToString() + ";应答描述：" + dtReTou.Rows[0]["RspMsg"].ToString();
            }
            catch(Exception ex)
            {
                OutPutHT["执行状态"] = "应答失败";
                OutPutHT["详细信息"] = ex.ToString();
            }
            DForThread(OutPutHT);

            #region 写日志
            // try
            //{
            string serverDir = System.AppDomain.CurrentDomain.BaseDirectory + "\\Log_BankService_PingAn\\" + DateTime.Now.ToString("yyyy-MM-dd") + "\\1019\\";
            if (!Directory.Exists(serverDir))
            {
                Directory.CreateDirectory(serverDir);
            }
            string serverFile = serverDir + DateTime.Now.ToString("HHmmss-fff") + ".txt";
            if (!File.Exists(serverFile))
            {
                FileStream f = File.Create(serverFile);
                f.Close();
                f.Dispose();
            }
            using (StreamWriter sw = new StreamWriter(serverFile, true, System.Text.Encoding.UTF8))
            {
                if (dsReturn == null)
                {
                    sw.WriteLine("接收到银行发来的包：" + OutPutHT["str"].ToString());
                    sw.WriteLine("总长度为：" + bytes);
                    sw.WriteLine("发送到我方服务器进行处理，得到的DataSet为Null。异常描述：" + objRe[1].ToString());
                }
                else
                {
                    sw.WriteLine("接收到银行发来的包：" + OutPutHT["str"].ToString());
                    sw.WriteLine("总长度为：" + bytes);
                    sw.WriteLine("我方服务器的处理结果----------");
                    sw.WriteLine("返回码：" + dtReTou.Rows[0]["RspCode"].ToString());
                    sw.WriteLine("代码信息：" + dtReTou.Rows[0]["RspMsg"].ToString());
                    sw.WriteLine("回馈给银行的数据包：" + ht["bao"].ToString());
                }
            }
            //}
            //catch (Exception exexexe) { throw; }
            #endregion

            //返回结果 
            OutPutHT["执行状态"] = "写日志成功";
            OutPutHT["详细信息"] = "【1019】接口处理完毕";
            DForThread(OutPutHT);
        }

        /// <summary>
        ///处理接口【1005】
        /// </summary>
        /// <param name="newSocket"></param>
        /// <param name="OutPutHT">包括包头、包体的</param>
        /// <param name="bytes">接收到的包的长度</param>
        private void run_1005_feedback(Socket newSocket, Hashtable OutPutHT, int bytes)
        {
            OutPutHT["执行状态"] = "开始处理【1005】接口数据";
            OutPutHT["详细信息"] = "正在准备应答包...";
            DForThread(OutPutHT);

            DataTable dtTou = (DataTable)OutPutHT["baotou"];
            DataTable dtTi = (DataTable)OutPutHT["baoti"];  

            //更新1005状态全局变量的值
            //Program.state1005["状态"] = "1005收到包";
            //Program.state1005["文件名"] = dtTi.Rows[0]["FileName"].ToString();
            //Program.state1005["类型"] = dtTi.Rows[0]["FuncFlag"].ToString();

            Core c = new Core();
            //生成应答包的包体，只有一个Reserve字段
            DataTable dtReTi = new DataTable();
            dtReTi.Columns.Add("Reserve", typeof(string));
            dtReTi.Rows.Add(new object[] { "" });

            //应答包的包头
            DataTable dtReTou = dtTou.Copy();

            //根据后台处理结果构建应答包
            dtReTou.Rows[0]["ServType"] = "02";//02表示应答
            dtReTou.Rows[0]["RspCode"] = "000000";
            dtReTou.Rows[0]["RspMsg"] = "交易成功";

            //设置包头中包体的长度           
            int baotiLength = c.GetBaotiLength(dtReTi);//包体长度
            dtReTou.Rows[0]["Length"] = baotiLength;
            dtReTou.Rows[0]["TranTime"] = DateTime.Now.ToString("HHmmss");

            DataSet dsRe = new DataSet();
            dtReTou.TableName = "dtTou";
            dtReTi.TableName = "dtTi";
            dsRe.Tables.Add(dtReTou);
            dsRe.Tables.Add(dtReTi);
            Hashtable ht = c.UsToBank(dsRe);//3、将包头包体拼装成字符串，然后转化成byte流
            byte[] reByte = (byte[])ht["byteAry"];//要发给银行的数据包
            try
            {
                //发送应答包
                int r = newSocket.Send(reByte);
                OutPutHT["执行状态"] = "【1005】应答成功";
                OutPutHT["详细信息"] = OutPutHT["详细信息"] = "应答码：" + dtReTou.Rows[0]["RspCode"].ToString() + ";应答描述：" + dtReTou.Rows[0]["RspMsg"].ToString();

                OutPutHT["文件名"] = dtTi.Rows[0]["FileName"].ToString();

                //更新1005状态全局变量的值               
                Program.state1005["文件名"] = dtTi.Rows[0]["FileName"].ToString();
                Program.state1005["类型"] = dtTi.Rows[0]["FuncFlag"].ToString();
                //根据不同功能，给状态赋不同的值，方便后面的判断
                switch(dtTi.Rows[0]["FuncFlag"].ToString ())
                {
                    case "1":
                        Program.state1005["状态"] = "清算失败1005完成";
                        Program.state1005["类型"] = "清算失败";
                        OutPutHT["类型"] = "清算失败";
                        break;
                    case "2":
                        Program.state1005["状态"] = "会员余额1005完成";
                        Program.state1005["类型"] = "会员余额";
                        OutPutHT["类型"] = "会员余额";
                        break;
                    case "3":
                        Program.state1005["状态"] = "出入金1005完成";
                        Program.state1005["类型"] = "出入金";
                        OutPutHT["类型"] = "出入金";
                        break;
                    case "4"://开销户匹配流水文件
                        Program.state1005["状态"] = "开销户1005完成";
                        Program.state1005["类型"] = "开销户";
                        OutPutHT["类型"] = "出入金";
                        break;
                    case "5":
                        Program.state1005["状态"] = "对账不平1005完成";
                        Program.state1005["类型"] = "对账不平";
                        OutPutHT["类型"]="对账不平";
                        break;
                    default:
                        break;
                }                
               
            }
            catch (Exception ex)
            {
                OutPutHT["执行状态"] = "应答失败";
                OutPutHT["详细信息"] = ex.ToString();

                //更新1005状态全局变量的值
                Program.state1005["状态"] = "1005应答失败";                
            }
            DForThread(OutPutHT);
           
            
            //写日志
            #region 写日志
            // try
            //{
            string serverDir = System.AppDomain.CurrentDomain.BaseDirectory + "\\Log_BankService_PingAn\\" + DateTime.Now.ToString("yyyy-MM-dd") + "\\1005\\";
            if (!Directory.Exists(serverDir))
            {
                Directory.CreateDirectory(serverDir);
            }
            string serverFile = serverDir + DateTime.Now.ToString("HHmmss-fff") + ".txt";
            if (!File.Exists(serverFile))
            {
                FileStream f = File.Create(serverFile);
                f.Close();
                f.Dispose();
            }
            using (StreamWriter sw = new StreamWriter(serverFile, true, System.Text.Encoding.UTF8))
            {
                sw.WriteLine("接收到银行发来的包：" + OutPutHT["str"].ToString());
                sw.WriteLine("总长度为：" + bytes);
                sw.WriteLine("我方服务器的处理结果----------");
                sw.WriteLine("返回码：" + dtReTou.Rows[0]["RspCode"].ToString());
                sw.WriteLine("代码信息：" + dtReTou.Rows[0]["RspMsg"].ToString());
                sw.WriteLine("回馈给银行的数据包：" + ht["bao"].ToString());
            }
            //}
            //catch (Exception exexexe) { throw; }
            #endregion

            //返回结果 
            OutPutHT["执行状态"] = "写日志成功";
            OutPutHT["详细信息"] = "【1005】接口处理完毕";
            DForThread(OutPutHT);
        }

        /// <summary>
        /// 处理不在我方支持范围内的接口数据，直接给银行反馈失败
        /// </summary>
        /// <param name="newSocket"></param>
        /// <param name="OutPutHT"></param>
        /// <param name="bytes"></param>
        private void run_ex_feedback(Socket newSocket, Hashtable OutPutHT, int bytes)
        {
            OutPutHT["执行状态"] = "接口编号或交易网代码异常";
            OutPutHT["详细信息"] = "正在准备应答包...";
            DForThread(OutPutHT);

            DataTable dtTou = (DataTable)OutPutHT["baotou"];
            DataTable dtTi = (DataTable)OutPutHT["baoti"];

            string FuncId = dtTou.Rows[0]["TranFunc"].ToString();//接口编号

            Core c = new Core();

            //构建应答包体,不处理内容，包体返回空   
            DataTable dtReTi = new DataTable();
          
            //应答包的包头
            DataTable dtReTou = dtTou.Copy();
            dtReTou.Rows[0]["ServType"] = "02";//02表示应答
            dtReTou.Rows[0]["RspCode"] = "ERR074";
            dtReTou.Rows[0]["RspMsg"] = "交易网不支持此接口或交易网代码错误";            
            int baotiLength = c.GetBaotiLength(dtReTi);//包体长度
            dtReTou.Rows[0]["Length"] = baotiLength;
            dtReTou.Rows[0]["TranTime"] = DateTime.Now.ToString("HHmmss");

            DataSet dsRe = new DataSet();
            dtReTou.TableName = "dtTou";
            dtReTi.TableName = "dtTi";
            dsRe.Tables.Add(dtReTou);
            dsRe.Tables.Add(dtReTi);
            Hashtable ht = c.UsToBank(dsRe);//3、将包头包体拼装成字符串，然后转化成byte流
            byte[] reByte = (byte[])ht["byteAry"];//要发给银行的数据包
            try
            {
                //发送应答包
                int r = newSocket.Send(reByte);
                OutPutHT["执行状态"] = "应答成功";
                OutPutHT["详细信息"] = OutPutHT["详细信息"] = "应答码：" + dtReTou.Rows[0]["RspCode"].ToString() + ";应答描述：" + dtReTou.Rows[0]["RspMsg"].ToString();
            }
            catch (Exception ex)
            {
                OutPutHT["执行状态"] = "应答失败";
                OutPutHT["详细信息"] = ex.ToString();
            }
            DForThread(OutPutHT);

            #region 写日志
            // try
            //{
            string serverDir = System.AppDomain.CurrentDomain.BaseDirectory + "\\Log_BankService_PingAn\\" + DateTime.Now.ToString("yyyy-MM-dd") + "\\exption\\";
            if (!Directory.Exists(serverDir))
            {
                Directory.CreateDirectory(serverDir);
            }
            string serverFile = serverDir + DateTime.Now.ToString("HHmmss-fff") + ".txt";
            if (!File.Exists(serverFile))
            {
                FileStream f = File.Create(serverFile);
                f.Close();
                f.Dispose();
            }
            using (StreamWriter sw = new StreamWriter(serverFile, true, System.Text.Encoding.UTF8))
            {
                sw.WriteLine("接收到银行发来的包：" + OutPutHT["str"].ToString());
                sw.WriteLine("总长度为：" + bytes);
                sw.WriteLine("我方不支持此接口业务----------");
                sw.WriteLine("返回码：" + dtReTou.Rows[0]["RspCode"].ToString());
                sw.WriteLine("代码信息：" + dtReTou.Rows[0]["RspMsg"].ToString());
                sw.WriteLine("回馈给银行的数据包：" + ht["bao"].ToString());
            }
            //}
            //catch (Exception exexexe) { throw; }
            #endregion

            //返回结果 
            OutPutHT["执行状态"] = "写日志成功";
            OutPutHT["详细信息"] = "【"+FuncId+"】接口处理完毕";
            DForThread(OutPutHT);
        }
    }
}
