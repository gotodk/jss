using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Collections;
using System.Xml.Serialization;
using System.IO;
using System.Configuration;
using System.Threading;
using ServiceStack.Redis;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using FMDBHelperClass;


namespace FMipcClass
{
    /// <summary>
    /// 通过ICP进行调用的类库
    /// </summary>
    static public class IPC
    {

        static DataSet ds_IPClist = null;

        /// <summary>
        /// 呼叫聚合接口中心的webservice，并调用指定的接口获取数据。返回一个数组。记录是否成功，以及具体提示。数组第一个是ok或者err,代表执行结果。 数组第二个返回错误信息或者具体得到数据。
        /// </summary>
        /// <param name="FF_yewuname">业务名称</param>
        /// <param name="objCS">参数数组，数组中的每个项，代表一个参数。</param>
        /// <returns>返回一个数组。记录是否成功，以及具体提示。数组第一个是ok或者err,代表执行结果。 数组第二个返回错误信息或者具体得到数据。</returns>
        static public object[] Call(string FF_yewuname, object[] objCS)
        {



            string IPCurl = ConfigurationManager.ConnectionStrings["IPCurl"].ToString(); //聚合中心地址

            DateTime dtbegin = DateTime.Now;
            //从本地关系配置文件中，找找有没有这个接口调用的配置。若找不到，直接返回错误。
            string dizhi = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "//ForIPC//IPClist.config";
            DataSet dsipc = new DataSet();
            if (ds_IPClist == null)
            {
                //加速，只要有，就从内存中取
                dsipc.ReadXml(dizhi);
                ds_IPClist = dsipc;
            }
            else
            {
                dsipc = ds_IPClist;
            }
          
            string GX_shibie = ConfigurationManager.ConnectionStrings["ThisAppPoolName"].ToString();//用进程池名称作为标识
            DataRow[] drs = dsipc.Tables[0].Select("调用方识别标记='" + GX_shibie + "' and  业务名称='" + FF_yewuname + "'");


            if (drs.Length < 1)
            {
                return new object[] { "err", "找不到匹配关系！" };
            }
            if (drs.Length > 1)
            {
                return new object[] { "err", "找到多个匹配关系，有歧义！" };
            }

            DataTable dtone = dsipc.Tables[0].Clone();
            dtone.Clear();
            dtone.Rows.Add(drs[0].ItemArray);

            //开始调用
            return Call_NEW( dtone, IPCurl, dtbegin, objCS);
            //if (dtone.Rows[0]["操作特点"].ToString() == "1")
            //{
            //    return  Call_E(dsipc, dtone, IPCurl, dtbegin, FF_yewuname, objCS);

            //}
            //else
            //{

            //    return  Call_C(dsipc, dtone, IPCurl, dtbegin, FF_yewuname, objCS);

            //}

 

        }
        /// <summary>
        /// (业务特点是1的用这个)呼叫聚合接口中心的webservice，并调用指定的接口获取数据。这种方式，强制开启了日志，日志记录插入错误，将直接返回错误，业务接口也不会被执行。
        /// </summary>
        /// <param name="FF_yewuname">业务名称</param>
        /// <param name="objCS">参数数组，数组中的每个项，代表一个参数。</param>
        /// <returns>返回一个数组。记录是否成功，以及具体提示。</returns>
        static private object[] Call_E(DataSet dsipc, DataTable dtone, string IPCurl, DateTime dtbegin, string FF_yewuname, object[] objCS)
        {



            //若能找到本地关系，立刻向IPC发送日志指令，IPC会把记录计入“接口调用日志表”（不会检查关系是否正确），并返回消息。
            //下面这一大段，比较不好，后续再修改。

            object re = null;
            try
            {

                FMWScenter wsd = new FMWScenter(IPCurl + "?wsdl");

                //参数方法日志时，转化成多个要保存的日志序列化文本。以解决object[]内有特殊变量不能序列化的问题。
                string[] objCS_str = new string[objCS.Length + 1];
                string typeall = "";
                for (int i = 0; i < objCS.Length; i++)
                {
                    StringWriter sw = new StringWriter();
                    XmlSerializer serializer = new XmlSerializer(objCS[i].GetType());
                    serializer.Serialize(sw, objCS[i]);
                    objCS_str[i + 1] = sw.ToString();
                    if (i == objCS.Length - 1)
                    {
                        typeall = typeall + objCS[i].GetType().AssemblyQualifiedName;
                    }
                    else
                    {
                        typeall = typeall + objCS[i].GetType().AssemblyQualifiedName + "|";
                    }

                    sw.Close();
                }
                objCS_str[0] = typeall;


                re = wsd.ExecuteQuery("SaveCallLog", new object[] { dtone, objCS_str });
                if (re == null)
                {
                    return new object[] { "err", "发送日志失败，原因未知！" };
                }
                else
                {
                    if (re.ToString().IndexOf("okgo:") >= 0)  //日志成功
                    {
                        //日志唯一标示
                        string LOG_guid = re.ToString().Replace("okgo:", "");
                        //返回okgo,并且本次调用的接口是同步接口，可以开始真正调用目标接口(异步不调用)。
                        if (dtone.Rows[0]["调用方式"].ToString() == "1")
                        {
                            object Dre = null;

                            //开始真正调用
                            string Durls = "http://" + dtone.Rows[0]["接口域名"].ToString() + "/" + dtone.Rows[0]["接口地址"].ToString() + "?wsdl";
                            //搞一个异常检查，确定是日志接口对了，但是业务接口调用失败。
                            string trytype = "";
                            try
                            {

                                wsd = new FMWScenter(Durls);
                                Dre = wsd.ExecuteQuery(dtone.Rows[0]["方法名"].ToString(), objCS);
                                if (Dre == null)
                                {
                                    trytype = "同步调用失败，空返回值！";
                                }
                                else
                                {
                                    trytype = "okle";
                                }
                            }
                            catch (Exception exx)
                            {
                                trytype = "同步调用失败，业务接口出现问题：" + exx.ToString() + exx.InnerException.Message;
                            }

                            //无论是否调用成功，都给日志一个反馈。这次反馈不再关注是否成功。
                            DateTime dtendtime = DateTime.Now;
                            try
                            {
                                string catchstring = "";//首次执行调试信息
                                string dqzt = "9";
                                if (trytype == "okle")
                                {
                                    dqzt = "1";
                                }
                                else
                                {
                                    dqzt = "0";
                                    catchstring = trytype;
                                }
                                //数组顺序是： 日志唯一标示、首次执行开始时间 、执行消耗时间、当前状态
                                string[] updatelog = new string[] { LOG_guid, dtbegin.ToString("yyyy-MM-dd HH:mm:ss.fff"), (dtbegin - DateTime.Now).Milliseconds.ToString(), dqzt, catchstring,"" };
                                wsd = new FMWScenter(IPCurl + "?wsdl");
                                object aaa = wsd.ExecuteQuery("SaveCallLog_end", updatelog);
                            }
                            catch { }

                            //返回业务执行结果
                            if (trytype == "okle")
                            {
                                return new object[] { "ok", Dre }; //这里返回的直接就是执行结果
                            }
                            else
                            {
                                return new object[] { "err", trytype };
                            }


                            /////



                        }
                        else
                        {
                            return new object[] { "ok", "异步调用成功！" };
                        }
                    }
                    else
                    {
                        return new object[] { "err", "日志保存失败！" };
                    }
                }

            }
            catch (Exception ex)
            {
                return new object[] { "err", ex.ToString() };
            }





        }

        /// <summary>
        /// (业务特点是0的用这个)呼叫聚合接口中心的webservice，并调用指定的接口获取数据。这种方式，根据配置文件来确定是否开启发送日志。但无论是否开启了日志，无论日志记录成败，都会调用业务接口。 这种方式全部强制成了同步调用，没有必要异步调用。
        /// </summary>
        /// <param name="FF_yewuname">业务名称</param>
        /// <param name="objCS">参数数组，数组中的每个项，代表一个参数。</param>
        /// <returns>返回一个数组。记录是否成功，以及具体提示。</returns>
        static private object[] Call_C(DataSet dsipc, DataTable dtone, string IPCurl, DateTime dtbegin, string FF_yewuname, object[] objCS)
        {

            //如果开启了日志。先调用日志插入，无论日志是否成功。 都接着调用业务接口。  无论业务接口是否发生错误，都会试着更新日志。

            string LOG_guid = ""; //日志唯一标示

            //==========================
            if (dtone.Rows[0]["是否开启日志"].ToString() == "1")
            {
                //尝试调用日志插入接口
                try
                {
                    FMWScenter wsd = new FMWScenter(IPCurl + "?wsdl");



                    //参数方法日志时，转化成多个要保存的日志序列化文本。以解决object[]内有特殊变量不能序列化的问题。
                    string[] objCS_str = new string[objCS.Length + 1];
                    string typeall = "";
                    for (int i = 0; i < objCS.Length; i++)
                    {
                        StringWriter sw = new StringWriter();
                        XmlSerializer serializer = new XmlSerializer(objCS[i].GetType());
                        serializer.Serialize(sw, objCS[i]);
                        objCS_str[i + 1] = sw.ToString();
                        if (i == objCS.Length - 1)
                        {
                            typeall = typeall + objCS[i].GetType().AssemblyQualifiedName;
                        }
                        else
                        {
                            typeall = typeall + objCS[i].GetType().AssemblyQualifiedName + "|";
                        }

                        sw.Close();
                    }
                    objCS_str[0] = typeall;




                    object re = null;
                    re = wsd.ExecuteQuery("SaveCallLog", new object[] { dtone, objCS_str });
                    if (re != null && re.ToString().IndexOf("okgo:") >= 0) //代表日志插入成功了
                    {
                        LOG_guid = re.ToString().Replace("okgo:", ""); //记录新日志标记
                    }
                }
                catch (Exception ex)
                {
                    string aaa = ex.ToString();
                }
            }
            //==========================



            //调用业务接口
            object Dre = null;
            //开始真正调用
            string Durls = "http://" + dtone.Rows[0]["接口域名"].ToString() + "/" + dtone.Rows[0]["接口地址"].ToString() + "?wsdl";
            string trytype = "";
            try
            {


                FMWScenter wsd = new FMWScenter(Durls);
                Dre = wsd.ExecuteQuery(dtone.Rows[0]["方法名"].ToString(), objCS);
                if (Dre == null)
                {
                    trytype = "查询业务调用失败，空返回值！";
                }
                else
                {
                    trytype = "okle";
                }
            }
            catch (Exception exx)
            {
                trytype = "查询业务调用失败，业务接口出现问题：" + exx.ToString() + exx.InnerException.Message;
            }



            //==========================
            if (dtone.Rows[0]["是否开启日志"].ToString() == "1")
            {
                //尝试更新日志
                try
                {
                    string catchstring = "";//首次执行调试信息
                    string dqzt = "9"; //业务接口调用是否成功
                    if (trytype == "okle")
                    {
                        dqzt = "1";
                    }
                    else
                    {
                        dqzt = "0";
                        catchstring = trytype;
                    }
                     
                    //数组顺序是： 日志唯一标示、首次执行开始时间 、执行消耗时间、当前状态
                    string[] updatelog = new string[] { LOG_guid, dtbegin.ToString("yyyy-MM-dd HH:mm:ss.fff"), (dtbegin - DateTime.Now).Milliseconds.ToString(), dqzt, catchstring, "" };
                    FMWScenter wsd = new FMWScenter(IPCurl + "?wsdl");
                    object aaa = wsd.ExecuteQuery("SaveCallLog_end", updatelog);
                }
                catch { }
            }
            //==========================

            //最后返回执行结果
            if (trytype == "okle")
            {
                return new object[] { "ok", Dre };
            }
            else
            {
                return new object[] { "err", trytype };
            }

        }




        /// <summary>
        /// (改版后的)呼叫聚合接口中心的webservice，并调用指定的接口获取数据。
        /// </summary>
        /// <param name="dtone">被调用的接口关系基础数据</param>
        /// <param name="IPCurl">聚合中心地址</param>
        /// <param name="dtbegin">准备调用开始时间</param>
        /// <param name="objCS">参数名称</param>
        /// <returns></returns>
        static private object[] Call_NEW(DataTable dtone, string IPCurl, DateTime dtbegin, object[] objCS)
        {
            //如果是同步调用，直接调用。  如果是异步调用，仅记录。
            //如果开启了日志。先调用日志插入，无论日志是否成功。 都接着调用业务接口。  无论业务接口是否发生错误，都会试着更新日志。


            try
            {


                //如果是同步调用
                if (dtone.Rows[0]["调用方式"].ToString() == "1")
                {
                    string trytype = "";
                    object Dre = null;

                    try
                    {

                        //开始真正调用
                        string Durls = "http://" + dtone.Rows[0]["接口域名"].ToString() + "/" + dtone.Rows[0]["接口地址"].ToString() + "?wsdl";
                        FMWScenter wsd = new FMWScenter(Durls);
                        Dre = wsd.ExecuteQuery(dtone.Rows[0]["方法名"].ToString(), objCS);
                        if (Dre == null)
                        {
                            trytype = "查询业务调用失败，空返回值！";
                        }
                        else
                        {
                            trytype = "okle";
                        }

                    }
                    catch (Exception exx)
                    {
                        trytype = "同步调用失败，业务接口出现问题：" + exx.ToString() + exx.InnerException.Message;
                    }

                    //若开启日志，开线程写日志
                    if (dtone.Rows[0]["是否开启日志"].ToString() == "1")
                    {


                        //开启一个线程，执行日志处理 
                        ThreadStart savelog = delegate { StartSaveLog(dtone, IPCurl, dtbegin.ToString("yyyy-MM-dd HH:mm:ss.fff"), (dtbegin - DateTime.Now).Milliseconds.ToString(), trytype); };
                        Thread thread = new Thread(savelog);
                        thread.Name = "IPCWorkLog_kj";
                        thread.IsBackground = true;
                        thread.Start();
                    }




                    //最后返回执行结果
                    if (trytype == "okle")
                    {
                        return new object[] { "ok", Dre };
                    }
                    else
                    {
                        return new object[] { "err", trytype };
                    }


                }
                else//异步调用
                {
                    string trytype = "";
                    try
                    {
                        //从这里直接连接Redis,写入异步处理队列
                        RedisClient RC = FMDBHelperClass.RedisClass.GetRedisClient(null);

                        //处理值
                        string queGUID = Guid.NewGuid().ToString();
                        string JKhost = dtone.Rows[0]["接口域名"].ToString();
                        string JKpath = dtone.Rows[0]["接口地址"].ToString();
                        string FFname = dtone.Rows[0]["方法名"].ToString();
                        string FFretype = dtone.Rows[0]["返回值类型"].ToString();
                        string CStype = "";
             
                        List<KeyValuePair<byte[], long>> keyValuePairsobjCS = new List<KeyValuePair<byte[], long>>();
                        for (int i = 0; i < objCS.Length; i++)
                        {

                            MemoryStream ms = new MemoryStream();
                            IFormatter bf = new BinaryFormatter();
                            bf.Serialize(ms, objCS[i]);
                            keyValuePairsobjCS.Add(new KeyValuePair<byte[], long>(ms.ToArray(), i));
                            if (i == objCS.Length - 1)
                            {
                                CStype = CStype + objCS[i].GetType().AssemblyQualifiedName;
                            }
                            else
                            {
                                CStype = CStype + objCS[i].GetType().AssemblyQualifiedName + "#";
                            }

                            ms.Close();
                            ms.Dispose();
                        }
                        //开始写入哈希，记录调用目标
                        List<KeyValuePair<string, string>> keyValuePairs = new List<KeyValuePair<string, string>>();
                        keyValuePairs.Add(new KeyValuePair<string, string>("JKhost", JKhost));
                        keyValuePairs.Add(new KeyValuePair<string, string>("JKpath", JKpath));
                        keyValuePairs.Add(new KeyValuePair<string, string>("FFname", FFname));
                        keyValuePairs.Add(new KeyValuePair<string, string>("CStype", CStype));
                        keyValuePairs.Add(new KeyValuePair<string, string>("FFretype", FFretype));

                        lock (RedisClass.LockObj)
                        {
                            RC.SetRangeInHash("H:queIPC:FF:" + queGUID, keyValuePairs);
                            //开始写入有序集合，记录参数
                            RC.ZAdd("Z:queIPC:CS:" + queGUID, keyValuePairsobjCS);

                            //开始写入列表末尾,记录队列GUID
                            RC.PushItemToList("L:queIPC:RUN", queGUID);
                        }

                        return new object[] { "ok", "异步调用成功" };
                    }
                    catch (Exception exx)
                    {
                        trytype = "异步调用写入Redis失败，业务接口出现问题：" + exx.ToString() + exx.InnerException.Message;
                        return new object[] { "err", trytype };
                    }
               
                   
                }

            }
            catch (Exception ex)
            {
                return new object[] { "err", ex.ToString() };
            }

      

        }


        static private void StartSaveLog(DataTable dtone, string IPCurl, string dtbegin, string jg, string catchstring)
        {
            try
            {
                //调用业务接口
                object Dre = null;
                //开始真正调用
                FMWScenter wsd = new FMWScenter(IPCurl + "?wsdl");
                Dre = wsd.ExecuteQuery("SaveCallLogNEW", new object[] { dtone, IPCurl, dtbegin, jg, catchstring });
            }
            catch (Exception exx)
            {
                string trytype = "调用写运行日志方法失败：" + exx.ToString();
            }
        }




    }
}
