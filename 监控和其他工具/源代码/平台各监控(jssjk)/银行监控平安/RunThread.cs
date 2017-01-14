using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using FMipcClass;

namespace 银行监控平安
{
    public class RunThread
    {
        delegateForThread DForThread;
        Hashtable InPutHT;
        public RunThread()
        {
            DForThread = null;
            InPutHT = null;
        }
        public RunThread(Hashtable PHT, delegateForThread DFT)
        {
            DForThread = DFT;
            InPutHT = PHT;
        }
        //更新配置文件中的银行接口IP和Port
        public void UpdateConfig(string AppKey, string KeyValue)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(Application.ExecutablePath + ".config");//获取当前配置文件  
            XmlNode node = doc.SelectSingleNode(@"//add[@key='" + AppKey + "']");
            XmlElement ele = (XmlElement)node;
            ele.SetAttribute("value", KeyValue);
            doc.Save(Application.ExecutablePath + ".config");
            ConfigurationManager.RefreshSection("appSettings");
            //上面句很重要,强制程序重新获取配置文件中appSettings节点中所有的值,否则更改后要到程序关闭后才更新,因为程序启动后默认不再重新获取.  
        }

        /// <summary>
        /// 调用后台签到签退接口，通过type传递业务类型
        /// </summary>
        public void RunSign()
        {           
            //访问远程服务
            object[] objParams = { InPutHT["type"] };
            //WebServicesCenter WSC = new WebServicesCenter();           
            //DataSet dsreturn = WSC.RunAtServices("PingAn_Sign", objParams);
            DataSet dsreturn = new DataSet();
            object[] objre = IPC.Call("平安银行-签到签退", objParams);
            if (objre[0].ToString() == "ok")
            {
                dsreturn = (DataSet)objre[1];
            }
            else
            {
                dsreturn = null;               
            }
            //填充传入参数哈希表
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["返回值"] = dsreturn;
            DForThread(OutPutHT);
        }
        //开销户/出入金流水匹配
        public void RunMatch()
        {
            //填充传入参数哈希表
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["执行状态"] = "正在发起匹配请求...";
            OutPutHT["详细描述"] = "调用后台方法执行【1006】接口";
            DForThread(OutPutHT);
            //调用webservices方法，执行1006接口。
            DataTable dtParams = StringOP.GetDataTableFormHashtable(InPutHT);
            object[] objParams1006 = { dtParams };
            //WebServicesCenter WSC = new WebServicesCenter();           
            //DataSet dsReturn1006 = WSC.RunAtServices("PingAn_AcctAndMnyMatch", objParams1006);
            DataSet dsReturn1006 = new DataSet();
            object[] objre1006 = IPC.Call("平安银行-开销户流水匹配", objParams1006);
            string ht_ex = "";//后台调用异常描述
            if (objre1006[0].ToString() == "ok")
            {
                dsReturn1006 = (DataSet)objre1006[1];
            }
            else
            {
                dsReturn1006 = null;
                ht_ex = objre1006[1].ToString();
            }
            if (dsReturn1006 == null)
            {
                OutPutHT["执行状态"] = "OK，系统通讯错误";
                OutPutHT["详细描述"] = "调用后台方法未获得返回值。异常描述：" + ht_ex;
                DForThread(OutPutHT);
            }           
            else if (dsReturn1006.Tables["返回值单条"].Rows[0]["执行结果"].ToString() != "ok")
            {
                OutPutHT["执行状态"] = "ERR，请求失败";
                OutPutHT["详细描述"] = dsReturn1006.Tables["返回值单条"].Rows[0]["提示文本"].ToString();
                DForThread(OutPutHT);
            }
            else
            {
                //如果反馈的ok，则等待1005的反馈结果
                OutPutHT["执行状态"] = "OK，请求成功";
                OutPutHT["详细描述"] = "等待银行生成文件通知...";
                DForThread(OutPutHT);

                string state = Program.state1005["状态"].ToString();
                //每个业务需要执行不同的状态判断
                string endState = "";
                string breakState = "";
                switch (InPutHT["type"].ToString())
                {
                    case "出入金":
                        endState = "出入金1005完成";
                        breakState = "人工中断出入金";
                        break;
                    case "开销户":
                        endState = "开销户1005完成";
                        breakState = "人工中断开销户";
                        break;
                    default:
                        break;
                }
                //开始等待1005接口的成功执行
                while (state != endState)
                {
                    Thread.Sleep(2000);
                    state = Program.state1005["状态"].ToString();
                    if (state == breakState)
                    {
                        break;
                    }
                }
                if (state == breakState)
                {
                    OutPutHT["执行状态"] = "ERR，匹配终止";
                    OutPutHT["详细描述"] = "人为中断等待，程序无法获取银行文件";
                    DForThread(OutPutHT);
                }
                else
                {//如果1005执行成功了，开始获取文件并匹配数据
                    MatchData(OutPutHT);
                }
            }
        }

        //银行和本地数据匹配
        private void MatchData(Hashtable OutPutHT)
        {
            string filename = Program.state1005["文件名"].ToString().Trim();//1005接口收到的文件名
            string bankType = Program.state1005["类型"].ToString();//1005接口收到的文件类型
            bool canMatch = false;//记录是否需要进行数据匹配
            OutPutHT["执行状态"] = "OK，收到银行文件" + filename;
            OutPutHT["详细描述"] = "开始读取文件...";
            DForThread(OutPutHT);

            DataTable dt_file = new DataTable();
            if (filename == "none.txt")
            {//表示银行没有需要匹配的数据。
                dt_file.Columns.Add("数据", typeof(string));
                dt_file.TableName = "file";
                OutPutHT["执行状态"] = "OK，银行无数据";
                OutPutHT["详细描述"] = "正在确认本地数据...";
                DForThread(OutPutHT);
                //银行无数据时，需要确认本地是否有数据
                canMatch = true;
            }
            else
            {
                //调用ftp方法，获取文件内容
                FileFTP ftp = new FileFTP();
                string msg = "";
                dt_file = ftp.readerFtpFile(filename, ref msg);
                if (msg.IndexOf("err") >= 0)
                {
                    OutPutHT["执行状态"] = "ERR，匹配终止";
                    OutPutHT["详细描述"] = "读取银行文件失败。异常描述：" + msg;
                    DForThread(OutPutHT);
                    //文件读取失败，不需要再执行匹配，直接结束
                    canMatch = false;
                }
                else
                {
                    OutPutHT["执行状态"] = "OK，文件读取成功";
                    OutPutHT["详细描述"] = "开始进行数据匹配...";
                    DForThread(OutPutHT);
                    //文件读取成功，开始执行匹配操作
                    canMatch = true;
                }
            }
            if (canMatch)
            {
                object[] objMatch = { dt_file, bankType, filename };
                //DataSet dsResMatch = WSC.RunAtServices("PingAn_DataMatch", objMatch);
                DataSet dsResMatch = new DataSet();
                object[] objResMatch = IPC.Call("平安银行-对账数据处理", objMatch);
                if (objResMatch[0].ToString() == "ok")
                {
                    dsResMatch = (DataSet)objResMatch[1];
                    if (dsResMatch.Tables["返回值单条"].Rows[0]["执行结果"].ToString() == "err")
                    {
                        OutPutHT["执行状态"] = "ERR，匹配失败。";
                        OutPutHT["详细描述"] = dsResMatch.Tables["返回值单条"].Rows[0]["提示文本"].ToString();
                        DForThread(OutPutHT);
                    }
                    else
                    {
                        OutPutHT["执行状态"] = "OK，匹配完毕。";
                        OutPutHT["详细描述"] = "";
                        OutPutHT["异常数据"] = dsResMatch.Tables["异常数据"];
                        OutPutHT["银行数据"] = dsResMatch.Tables["银行数据"];
                        OutPutHT["本地数据"] = dsResMatch.Tables["本地数据"];
                        DForThread(OutPutHT);
                    }
                }
                else
                {
                    dsResMatch = null;
                    OutPutHT["执行状态"] = "ERR，调用后台方法出现异常。";
                    OutPutHT["详细描述"] = "异常描述：" + objResMatch[1].ToString();
                    DForThread(OutPutHT);
                }
            }
        }

        //生成清算文件
        public void CreateQSWJ()
        {
            //填充传入参数哈希表
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["执行状态"] = "正在获取清算数据...";
            OutPutHT["详细描述"] = "调用后台方法获取清算数据";
            DForThread(OutPutHT);

            //WebServicesCenter wsc = new WebServicesCenter();
            //DataSet returnDS = wsc.RunAtServices("GetPingAnQSDsData", null);

            DataSet returnDS = new DataSet();
            object[] objreturnDS = IPC.Call("平安银行-获取清算数据", new object[] { DateTime.Now.ToString("yyyyMMdd") });
            string ht_ex = "";//后台调用异常描述
            if (objreturnDS[0].ToString() == "ok")
            {
                returnDS = (DataSet)objreturnDS[1];
            }
            else
            {
                returnDS = null;
                ht_ex = objreturnDS[1].ToString();
            }

            if (returnDS == null)
            {
                OutPutHT["执行状态"] = "OK，系统通讯错误";
                OutPutHT["详细描述"] = "调用后台方法未获得返回值。异常描述：" + ht_ex;
                DForThread(OutPutHT);
            }
            else if (returnDS.Tables[0].Rows[0]["执行结果"].ToString() != "ok")
            {
                OutPutHT["执行状态"] = "ERR，获取清算数据失败";
                OutPutHT["详细描述"] = returnDS.Tables["返回值单条"].Rows[0]["提示文本"].ToString();
                DForThread(OutPutHT);
            }
            else
            {//清算数据获取成功
                OutPutHT["执行状态"] = "OK，获取清算数据成功";
                OutPutHT["详细描述"] = "正在生成清算文件...";
                DForThread(OutPutHT);
                string name = returnDS.Tables["返回值单条"].Rows[0]["附件信息3"].ToString();//文件名
                if (name == "none.txt")
                {//没有清算数据，只返回文件名，不需要生成和上传实际文件                                   
                    string WJMsg = name + "&" + "0" + "&" + returnDS.Tables["返回值单条"].Rows[0]["附件信息1"].ToString() + "&" + returnDS.Tables["返回值单条"].Rows[0]["附件信息2"].ToString();
                    OutPutHT["执行状态"] = "OK，生成成功,文件名：" + name;
                    OutPutHT["详细描述"] = "无清算数据，无需上传。"+Environment.NewLine+"下一步：执行清算";
                    DForThread(OutPutHT);
                }
                else
                {//存在清算数据，则写txt文件，并上传到ftp 
                    #region //将清算数据写到本地的txt文件
                    FileStream fs = new FileStream(name, FileMode.Create);
                    StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
                    //开始写入
                    string DataSource = returnDS.Tables["返回值单条"].Rows[0]["附件信息1"].ToString() + "&" + returnDS.Tables["返回值单条"].Rows[0]["附件信息2"].ToString() + "&\r\n";
                    for (int i = 0; i < returnDS.Tables["清算数据"].Rows.Count; i++)
                    {
                        for (int j = 0; j < returnDS.Tables["清算数据"].Columns.Count; j++)
                        {
                            DataSource += returnDS.Tables["清算数据"].Rows[i][j].ToString() + "&";
                        }
                        DataSource += "\r\n";
                    }
                    sw.Write(DataSource);

                    //清空缓冲区
                    sw.Flush();
                    long fileL = fs.Length;
                    //关闭流
                    sw.Close();
                    fs.Close();
                    #endregion

                    string WJMsg = name + "&" + fileL.ToString() + "&" + returnDS.Tables["返回值单条"].Rows[0]["附件信息1"].ToString() + "&" + returnDS.Tables["返回值单条"].Rows[0]["附件信息2"].ToString();//文件名&文件长度（b）&浮动盈亏总额（卖-买）&流水号

                    OutPutHT["执行状态"] = "OK，生成成功,文件名：" + name;
                    OutPutHT["详细描述"] = "正在上传到FTP服务器...";
                    DForThread(OutPutHT);

                    #region //FTP上传文件
                    FileFTP ftp = new FileFTP();
                    Hashtable ht = ftp.FTPUploadFile(name);
                    if (ht["状态"].ToString() == "ok")
                    {
                        FileInfo file = new FileInfo(name);
                        file.Delete();
                        OutPutHT["执行状态"] = "OK，上传成功";
                        OutPutHT["详细描述"] = "已上传到FTP服务器，本地文件已删除"+Environment.NewLine+"下一步：执行清算";
                        OutPutHT["文件信息"] = WJMsg;
                        DForThread(OutPutHT);
                    }
                    else
                    {
                        OutPutHT["执行状态"] = "ERR，上传失败";
                        OutPutHT["详细描述"] = ht["详情"].ToString();
                        OutPutHT["文件信息"] = WJMsg;
                        DForThread(OutPutHT);
                    }
                    #endregion
                }
            }
        }

        //执行清算过程
        public void RunQS()
        {
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["执行状态"] = "正在发起清算请求...";
            OutPutHT["详细描述"] = "调用后台方法执行【1003】接口";
            //调用webservices方法，执行1006接口。
            DataTable dtParams = StringOP.GetDataTableFormHashtable(InPutHT);
            object[] objParams1003 = { dtParams };
            //WebServicesCenter WSC = new WebServicesCenter();         
            //DataSet dsReturn1003 = WSC.RunAtServices("PingAn_RunQingSuan", objParams1003);
            DataSet dsReturn1003 = new DataSet();
            object[] objRe1003 = IPC.Call("平安银行-发起清算请求", objParams1003);
            string ht_ex = "";//后台方法调用异常描述
            if (objRe1003[0].ToString() == "ok")
            {
                dsReturn1003 = (DataSet)objRe1003[1];
            }
            else
            {
                dsReturn1003 = null;
                ht_ex = objRe1003[1].ToString();
            }
            if (dsReturn1003 == null)
            {
                OutPutHT["执行状态"] = "OK，系统通讯错误";
                OutPutHT["详细描述"] = "调用后台方法未获得返回值。异常描述：" + ht_ex;
                DForThread(OutPutHT);
            }
            else if (dsReturn1003.Tables["返回值单条"].Rows[0]["执行结果"].ToString() != "ok")
            {
                OutPutHT["执行状态"] = "ERR，请求失败";
                OutPutHT["详细描述"] = dsReturn1003.Tables["返回值单条"].Rows[0]["提示文本"].ToString();
                DForThread(OutPutHT);
            }
            else
            {
                //如果反馈的ok，则等待1005的结果               
                OutPutHT["执行状态"] = "OK，请求成功";
                OutPutHT["详细描述"] = "等待银行处理结果...";
                DForThread(OutPutHT);
               
                #region  //暂时不用
                ////等待1005执行结果或1004查询结果
                //string stateQS = Program.state1005["状态"].ToString();

                ////开始等待1005接口的清算失败文件
                //while (stateQS != "清算失败1005完成")
                //{
                //    Thread.Sleep(2000);
                //    stateQS = Program.state1005["状态"].ToString();
                //    if (stateQS == "清算结束" || stateQS == "人工中断清算")
                //    {                        
                //        break;
                //    }
                //    if (stateQS == "对账不平1005完成")
                //    {
                //        break;
                //    }
                //}
                //if (stateQS == "清算失败1005完成")
                //{
                //    OutPutHT["执行状态"] = "收到银行【1001】清算失败文件：" + Program.state1005["文件名"].ToString();
                //    OutPutHT["详细描述"] = "请核对清算失败数据并进行处理";
                //    OutPutHT["文件名"] = Program.state1005["文件名"].ToString();
                //    DForThread(OutPutHT);
                //}

                ////开始等待1005接口的对账不平文件
                //while (stateQS != "对账不平1005完成")
                //{
                //    Thread.Sleep(2000);
                //    stateQS = Program.state1005["状态"].ToString();
                //    if (stateQS == "清算结束" || stateQS == "人工中断清算")
                //    {                       
                //        break;
                //    }
                //}
                //if (stateQS == "对账不平1005完成")
                //{
                //    OutPutHT["执行状态"] = "收到银行【1007】对账不平文件：" + Program.state1005["文件名"].ToString();
                //    OutPutHT["详细描述"] = "请检查核对不平原因";
                //    OutPutHT["文件名"] = Program.state1005["文件名"].ToString();
                //    DForThread(OutPutHT);
                //}
                //else if (stateQS == "人工中断清算")
                //{
                //    OutPutHT["执行状态"] = "ERR，人工中断等待【1005】";
                //    OutPutHT["详细描述"] = "请人工确认银行反馈结果，然后进行后续处理";
                //    DForThread(OutPutHT);
                //}
                #endregion
            }
        }

        //执行1004接口
        public void BankView()
        {
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["执行状态"] = "发起查询请求...";
            OutPutHT["详细描述"] = "";
            DForThread(OutPutHT);

            //调用1004接口     
            object[] objParams1004 = { InPutHT["type"].ToString(), InPutHT["date"].ToString() };
            //WebServicesCenter WSC = new WebServicesCenter();            
            //DataSet dsReturn1004 = WSC.RunAtServices("PingAn_BankStateQuery", objParams1004);
            DataSet dsReturn1004 = new DataSet();
            object[] objRe1004 = IPC.Call("平安银行-查询银行清算进度", objParams1004);
            string ht_ex = "";//调用后台方法异常描述
            if (objRe1004[0].ToString() == "ok")
            {
                dsReturn1004 = (DataSet)objRe1004[1];
            }
            else
            {
                dsReturn1004 = null;
                ht_ex = objRe1004[1].ToString();
            }

            if (dsReturn1004 == null)
            {
                OutPutHT["执行状态"] = "OK，系统通讯错误";
                OutPutHT["详细描述"] = "调用后台方法未获得返回值。异常描述：" + ht_ex;
                DForThread(OutPutHT);
            }
            else if (dsReturn1004.Tables["返回值单条"].Rows[0]["执行结果"].ToString() != "ok")
            {
                OutPutHT["执行状态"] = "ERR，查询失败";
                OutPutHT["详细描述"] = dsReturn1004.Tables["返回值单条"].Rows[0]["提示文本"].ToString();
                DForThread(OutPutHT);
            }
            else
            {//1004接口执行成功              
                
                OutPutHT["执行状态"] ="银行反馈："+ dsReturn1004.Tables["返回值单条"].Rows[0]["提示文本"].ToString();
                OutPutHT["详细描述"] = "";
                if (dsReturn1004.Tables["返回值单条"].Rows[0]["附件信息1"].ToString() != "")
                {
                    OutPutHT["详细描述"] += "处理失败原因:" + dsReturn1004.Tables["返回值单条"].Rows[0]["附件信息1"].ToString();                     
                }

                if (dsReturn1004.Tables["返回值单条"].Rows[0]["附件信息2"].ToString() != "")
                { 
                    OutPutHT["详细描述"] += Environment.NewLine + "失败结果文件：" + dsReturn1004.Tables["返回值单条"].Rows[0]["附件信息2"].ToString();
                }               
                DForThread(OutPutHT);
            }           
        }
        //更新清算数据状态
        public void UpdState()
        { 
            if (InPutHT["清算状态"].ToString().IndexOf(".txt") >= 0)
            {//存在清算失败文件需要处理 
                FormatData("清算失败", InPutHT["清算状态"].ToString());
            }           
            else if (InPutHT["清算状态"].ToString() == "银行清算成功")
            {//清算成功，需要更新原始清算数据状态 
                FormatData("清算成功", "");
            }
        }

        //保存对账不平文件数据
        public void SaveDZBP()
        {                  
            FormatData("对账不平", InPutHT["对账不平"].ToString());
        }

        //处理清算失败和对账不平文件
        public void  FormatData(string type,string filename)
        {           
            Hashtable OutPutHT = new Hashtable();
            
            DataSet dsreturn = new DataSet();
            //WebServicesCenter WSC = new WebServicesCenter();           
            string ht_ex = "";//记录后台方法调用异常

            DataTable dt_file = new DataTable();
            if (type=="清算成功")
            {//银行清算成功时直接更新原始清算数据状态
                dt_file.Columns.Add("数据");
                dt_file.TableName = "file";
                OutPutHT["执行状态"] =  "开始"+ type+"处理...";
                OutPutHT["详细描述"] = "调用后台方法更新原始清算数据状态";
                DForThread(OutPutHT);
                object[] objParams = { dt_file, type, filename };
                //dsreturn = WSC.RunAtServices("PingAn_DataMatch", objParams);
                object[] objrefile = IPC.Call("平安银行-对账数据处理", objParams);
                if (objrefile[0].ToString() == "ok")
                {
                    dsreturn = (DataSet)objrefile[1];
                }
                else
                {
                    dsreturn = null;
                    ht_ex = objrefile[1].ToString();
                }
            }
            else
            {
                OutPutHT["执行状态"] = "开始"+type+"处理...";
                OutPutHT["详细描述"] = "正在读取" + filename + "文件";
                DForThread(OutPutHT);

                //调用ftp方法，获取文件内容           
                FileFTP ftp = new FileFTP();
                string msg = "";
                dt_file = ftp.readerFtpFile(filename, ref msg);
                if (msg.IndexOf("err") >= 0)
                {
                    OutPutHT["执行状态"] = "ERR，文件读取失败";
                    OutPutHT["详细描述"] = "请重新执行此操作";
                    DForThread(OutPutHT);
                }
                else
                {
                    OutPutHT["执行状态"] = "OK，文件读取成功";
                    OutPutHT["详细描述"] = "正在调用后台方法处理数据...";
                    DForThread(OutPutHT);
                    object[] objParams = { dt_file, type, filename };
                    //dsreturn = WSC.RunAtServices("PingAn_DataMatch", objParams);
                    object[] objrefile = IPC.Call("平安银行-对账数据处理", objParams);
                    if (objrefile[0].ToString() == "ok")
                    {
                        dsreturn = (DataSet)objrefile[1];
                    }
                    else
                    {
                        dsreturn = null;
                        ht_ex = objrefile[1].ToString();
                    }
                }
            }

            if (dsreturn == null)
            {
                OutPutHT["执行状态"] = "OK，系统通讯错误";
                OutPutHT["详细描述"] = "调用后台方法未获得返回值。异常描述："+ht_ex;
                DForThread(OutPutHT);
            }
            else if (dsreturn.Tables["返回值单条"].Rows[0]["执行结果"].ToString() != "ok")
            {
                OutPutHT["执行状态"] = "ERR，"+type+"处理失败";
                OutPutHT["详细描述"] = dsreturn.Tables["返回值单条"].Rows[0]["提示文本"].ToString();
                DForThread(OutPutHT);
            }
            else
            {//后台执行成功
                OutPutHT["执行状态"] = "OK，"+type+"处理成功";
                OutPutHT["详细描述"] = "---清算结束！---";
                OutPutHT["data"] = (DataTable)dsreturn.Tables["data"];
                OutPutHT["type"] = type;
                DForThread(OutPutHT);
            }
        }        
    }
}

