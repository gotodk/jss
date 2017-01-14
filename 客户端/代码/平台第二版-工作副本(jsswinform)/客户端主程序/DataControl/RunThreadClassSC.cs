using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Threading;
using System.Data;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace 客户端主程序.DataControl
{

    /// <summary>
    /// 上传处理线程类
    /// </summary>
    class RunThreadClassSC
    {
        //向线程传递的回调参数
        private delegateForThread DForThread;
        //向线程传递的数据参数
        private Hashtable InPutHT;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="PHT">需要传入线程的参数</param>
        /// <param name="DFT">线程委托</param>
        public RunThreadClassSC(Hashtable PHT, delegateForThread DFT)
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
            DForThread = DFT;
            InPutHT = PHT;
        }

        /// <summary>
        /// 开始执行线程
        /// </summary>
        public void BeginRun()
        {
            Thread.Sleep(500);
            //上传接收处理文件路径
            string url = "http://"+DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[1] + "/JHJXPT/shangchuan.aspx";

            int max = 0;
            string[] files = (string[])InPutHT["待上传文件"];

            
            for (int p = 0; p < files.Length; p++)
            {
                //生成新文件名
                string nfilename = Guid.NewGuid().ToString();
                //生成拓展名
                string extstr = Path.GetExtension(files[p]).ToLower();

                max = max + Upload_Request(url, files[p], nfilename + extstr);
            }

            //检测是否全部上传成功
            if (max == files.Length)
            {
                ////填充传入参数哈希表
                Hashtable OutPutHT = new Hashtable();
                OutPutHT["上传全部完成"] = "ok";
                DForThread(OutPutHT);
            }

                

      
        }



        // <summary> 
        /// 将本地文件上传到指定的服务器(HttpWebRequest方法) 
        /// </summary> 
        /// <param name="address">文件上传到的服务器</param> 
        /// <param name="fileNamePath">要上传的本地文件（全路径）</param> 
        /// <param name="saveName">文件上传后的名称</param> 
        /// <param name="progressBar">上传进度条</param> 
        /// <returns>成功返回1，失败返回0</returns> 
        private int Upload_Request(string address, string fileNamePath, string saveName)
        {
            int returnValue = 0;

            // 要上传的文件 
            FileStream fs = new FileStream(fileNamePath, FileMode.Open, FileAccess.Read);
            BinaryReader r = new BinaryReader(fs);

            //时间戳 
            string strBoundary = "----------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundaryBytes = Encoding.ASCII.GetBytes("\r\n--" + strBoundary + "\r\n");

            //请求头部信息 
            StringBuilder sb = new StringBuilder();
            sb.Append("--");
            sb.Append(strBoundary);
            sb.Append("\r\n");
            sb.Append("Content-Disposition: form-data; name=\"");
            sb.Append("file");
            sb.Append("\"; filename=\"");
            sb.Append(saveName);
            sb.Append("\"");
            sb.Append("\r\n");
            sb.Append("Content-Type: ");
            sb.Append("application/octet-stream");
            sb.Append("\r\n");
            sb.Append("\r\n");
            string strPostHeader = sb.ToString();
            byte[] postHeaderBytes = Encoding.UTF8.GetBytes(strPostHeader);

            // 根据uri创建HttpWebRequest对象 
            HttpWebRequest httpReq = (HttpWebRequest)WebRequest.Create(new Uri(address));
            httpReq.Method = "POST";

            //对发送的数据不使用缓存 
            httpReq.AllowWriteStreamBuffering = false;

            //设置获得响应的超时时间（300秒） 
            httpReq.Timeout = 300000;
            httpReq.ContentType = "multipart/form-data; boundary=" + strBoundary;
            long length = fs.Length + postHeaderBytes.Length + boundaryBytes.Length;
            long fileLength = fs.Length;
            httpReq.ContentLength = length;
            try
            {

                //每次上传4k 
                int bufferLength = 4096;
                byte[] buffer = new byte[bufferLength];

                //已上传的字节数 
                long offset = 0;

                //开始上传时间 
                DateTime startTime = DateTime.Now;
                int size = r.Read(buffer, 0, bufferLength);
                Stream postStream = httpReq.GetRequestStream();

                //发送请求头部消息 
                postStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);
                while (size > 0)
                {
                    postStream.Write(buffer, 0, size);
                    offset += size;
                    TimeSpan span = DateTime.Now - startTime;
                    double second = span.TotalSeconds;
                    string pjsd = "";
                    if (second > 0.001)
                    {
                        pjsd = (offset / 1024 / second).ToString("0.00") + "KB/秒"; //平均速度
                    }
                    else
                    {
                        pjsd = "正在连接…";
                    }

                    //回显进度
                    Hashtable OutPutHT2 = new Hashtable();
                    OutPutHT2["显示进度"] = "正在上传";
                    OutPutHT2["滚动条进度"] = (int)(offset * (int.MaxValue / length));
                    OutPutHT2["已用时"] = second.ToString("F2") + "秒";
                    OutPutHT2["平均速度"] = pjsd;
                    OutPutHT2["已上传"] = (offset * 100.0 / length).ToString("F2") + "%";
                    OutPutHT2["上传进度"] = (offset / 1048576.0).ToString("F2") + "M/" + (fileLength / 1048576.0).ToString("F2") + "M";
                    OutPutHT2["当前文件名"] = Path.GetFileName(fileNamePath);
                    DForThread(OutPutHT2);

                    size = r.Read(buffer, 0, bufferLength);
                }
                //添加尾部的时间戳 
                postStream.Write(boundaryBytes, 0, boundaryBytes.Length);
                postStream.Close();

                //获取服务器端的响应 
                WebResponse webRespon = httpReq.GetResponse();
                Stream s = webRespon.GetResponseStream();
                StreamReader sr = new StreamReader(s);

                //读取服务器端返回的消息 
                String sReturnString = sr.ReadLine();
                s.Close();
                sr.Close();
                string bz = sReturnString.Split('*')[0];
                if (bz == "Success")
                {
                    returnValue = 1;
                    //回显进度
                    Hashtable OutPutHT2 = new Hashtable();
                    OutPutHT2["显示进度"] = "单个完成";
                    OutPutHT2["当前文件名"] = Path.GetFileName(fileNamePath);
                    OutPutHT2["远程保存路径"] = sReturnString.Split('*')[1];
                    DForThread(OutPutHT2);
                }
                else if (bz == "Error")
                {
                    returnValue = 0;
                    //回显进度
                    Hashtable OutPutHT2 = new Hashtable();
                    OutPutHT2["显示进度"] = "单个出错(服务器错误)";
                    OutPutHT2["当前文件名"] = Path.GetFileName(fileNamePath);
                    OutPutHT2["远程保存路径"] = "";
                    DForThread(OutPutHT2);
                }

            }
            catch (Exception ex)
            {
                string aaaa = ex.ToString();
                returnValue = 0;
                //回显进度
                Hashtable OutPutHT2 = new Hashtable();
                OutPutHT2["显示进度"] = "单个出错(程序错误)";
                OutPutHT2["当前文件名"] = Path.GetFileName(fileNamePath);
                DForThread(OutPutHT2);
            }
            finally
            {
                fs.Close();
                r.Close();
            }

            return returnValue;
        }


    }
}
