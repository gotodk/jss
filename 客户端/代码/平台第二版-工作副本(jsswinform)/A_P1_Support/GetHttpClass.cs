using System;
using System.Net;
using System.IO;
using System.Text;
using System.Collections;

namespace 客户端主程序.Support
{
    /// <summary>
    /// 获取远程网页数据的方法集合
    /// </summary>
    public class GetHttpClass
    {

        private HTMLAnalyzeClass HAC = new HTMLAnalyzeClass();

        public GetHttpClass()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
            ;
        }


        /// <summary>
        /// 获取http页面全部html代码的函数(仅支持普通连接，用于采集普通网页)
        /// </summary>
        /// <param name="a_strUrl">需要获取的远程页面地址</param>
        /// <param name="encoding">页面编码</param>
        /// <param name="myCookieContainer">会话状态</param>
        /// <param name="proxyurl">代理服务器地址url</param>
        public Hashtable Get_Http(string a_strUrl, string encoding, CookieContainer myCookieContainer, string proxyurl)
        {
            string strResult = "";
            try
            {
                if (myCookieContainer == null)
                {
                    myCookieContainer = new CookieContainer();
                }

                //实例化HttpWebRequest类,用来建立连接
                HttpWebRequest myReq = (HttpWebRequest)HttpWebRequest.Create(a_strUrl);
                //连接参数
                myReq.Timeout = 30000;
                myReq.AllowAutoRedirect = true;
                myReq.MaximumAutomaticRedirections = 10;
                myReq.Method = "GET";
                myReq.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 1.1.4322)";

                //是否启用代理
                if (proxyurl != "")
                {
                    WebProxy myProxy = new WebProxy();
                    Uri newUri = new Uri("http://91.74.160.18:8080");
                    myProxy.Address = newUri;
                    myReq.Proxy = myProxy;
                }


                //定义会话状态
                myReq.CookieContainer = myCookieContainer;
                //myReq.PreAuthenticate = true;
                //发送连接命令
                HttpWebResponse HttpWResp = (HttpWebResponse)myReq.GetResponse();
                //获取cookies
                HttpWResp.Cookies = myCookieContainer.GetCookies(myReq.RequestUri);
                //获取远程数据流
                Stream myStream = HttpWResp.GetResponseStream();
                StreamReader sr;
                //实例化读取类
                if (encoding == "")
                {
                    sr = new StreamReader(myStream, Encoding.Default);
                }
                else
                {
                    sr = new StreamReader(myStream, Encoding.GetEncoding(encoding));
                }
                //读取数据
                strResult = sr.ReadToEnd();

                //关闭对象
                sr.Close();
                myStream.Close();
                HttpWResp.Close();

                //设置返回值
                Hashtable ht = new Hashtable();
                ht.Add("cookies", myCookieContainer);
                ht.Add("html", strResult);
                ht.Add("err", "n");

                return ht;
            }
            catch (Exception exp)
            {
                //设置返回值
                Hashtable ht = new Hashtable();
                ht.Add("cookies", myCookieContainer);
                ht.Add("html", exp.ToString());
                ht.Add("err", "y");
                return ht;
            }

        }

        WebClient webClient = new WebClient();
        public string Get_file(string a_strUrl, string filepath)
        {
            try
            {
                
                if (webClient.IsBusy)//是否存在正在进行中的Web请求
                {
                    webClient.CancelAsync();
                }
                string filename = Path.GetFileName(a_strUrl);
                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }
                webClient.DownloadFile(new Uri(a_strUrl), filepath + filename);


                return filename;
            }
            catch (Exception ex)
            {
  
                客户端主程序.Support.StringOP.WriteLog("下载文件错误：" + ex.ToString());
                return "err";
            }
            
        }


        /// <summary>
        /// 下载远程文件到指定目录(直支持标准格式)
        /// </summary>
        /// <param name="a_strUrl">远程文件</param>
        /// <param name="timeout">超时</param>
        /// <param name="filepath">本地主路径,需要最后带有\</param>
        /// <param name="keepname">是否保持源文件名称</param>
        /// <param name="timepath">是否根据日期自动生成文件夹</param>
        /// <param name="myCookieContainer">会话状态</param>
        /// <returns>返回图片路径(不包括原主目录),
        /// 比如传入filepath="d:\temp\",
        /// 则图片新路径为d:\temp\20070303\084534_23.jpg
        /// 那么返回的路径是20070303\084534_23.jpg</returns>
        public string Get_Img(string a_strUrl, int timeout, string filepath, bool keepname, bool timepath, CookieContainer myCookieContainer)
        {
            HttpWebRequest webRequest;


            Stream remoteStream;
            Stream localStream = null;

            int d;
            int x;
            string filetype; //文件类型
            string filename; //文件名(没有点)

            string newpath_d = ""; //时间组合,用来给目录命名
            string newpath = ""; //以日期生成的目录路径

            try
            {
                //获取远程地址后缀
                d = a_strUrl.LastIndexOf(".");
                x = a_strUrl.LastIndexOf("/");
                filetype = a_strUrl.Substring(d + 1);
                filename = a_strUrl.Substring(x + 1).Replace("." + filetype, "");

                // 检查目录是否存在
                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }

                if (timepath)
                {
                    newpath_d = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString();
                    newpath = newpath_d + @"\";

                    filepath = filepath + newpath;
                    // 检查目录是否存在
                    if (!Directory.Exists(filepath))
                    {
                        Directory.CreateDirectory(filepath);
                    }
                }
                if (!keepname)//需要重新命名
                {
                    filename = newpath_d + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + GetRandomNum(2, 3, 1, 99).ToString();
                }

                a_strUrl = a_strUrl.Replace("客户端主程序", GB2Unicode("客户端主程序")).Replace("公用通讯协议类库", GB2Unicode("公用通讯协议类库"));
                webRequest = (HttpWebRequest)WebRequest.Create(a_strUrl);
                webRequest.Method = "GET";
                webRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0)";
                webRequest.Headers.Set("Pragma", "no-cache");
                //定义会话状态
                webRequest.CookieContainer = myCookieContainer;
                webRequest.Timeout = timeout;
                if (webRequest != null)
                {


                    HttpWebResponse craboResponse = (HttpWebResponse)webRequest.GetResponse();
                    if (craboResponse != null)
                    {
                        remoteStream = craboResponse.GetResponseStream();
                        localStream = File.Create(filepath + filename + "." + filetype);
                        byte[] buffer = new byte[1254];
                        int bytesRead;
                        do
                        {
                            bytesRead = remoteStream.Read(buffer, 0, buffer.Length);
                            localStream.Write(buffer, 0, bytesRead);
                        } while (bytesRead > 0);
                    }
                }
                return newpath + filename + "." + filetype;
            }
            catch (Exception exp)
            {
                客户端主程序.Support.StringOP.WriteLog("下载文件错误：" + exp.ToString());
                return "err";
            }
            finally
            {
                if (localStream != null)
                {
                    localStream.Close();
                }
            }

        }

        /// <summary>
        /// 转换编码
        /// </summary>
        /// <param name="strSearch"></param>
        /// <returns></returns>
        public string GB2Unicode(string strSearch)
        {
            string Hexs = "";
            string HH;
            Encoding GB = Encoding.GetEncoding("GB2312");
            Encoding unicode = Encoding.Unicode;

            byte[] GBBytes = GB.GetBytes(strSearch);
            for (int i = 0; i < GBBytes.Length; i++)
            {
                HH = "%" + GBBytes[i].ToString("x");
                Hexs += HH;
            }
            return Hexs;
        }


        /// <summary>
        /// 生成不重复随机数
        /// </summary>
        /// <param name="i">随机数种子</param>
        /// <param name="length">种子增量</param>
        /// <param name="up">上限</param>
        /// <param name="down">下线</param>
        /// <returns></returns>
        private int GetRandomNum(int i, int length, int up, int down)
        {
            int iFirst = 0;
            Random ro = new Random(i * length * unchecked((int)DateTime.Now.Ticks));
            iFirst = ro.Next(up, down);
            return iFirst;
        }


        /// <summary>
        /// 获取http页面全部html代码的函数
        /// (重载，仅支持提交普通表单,不支持提交文件,不支持.net表单)
        /// (允许带有登录信息,虽然也可以用来采集非表单页面,
        /// 但有些非表单页面不支持写入流,因此可能无法采集,应使用普通采集方法)
        /// </summary>
        /// <param name="url"></param>
        /// <param name="encoding"></param>
        /// <param name="HtForm"></param>
        /// <param name="myCookieContainer"></param>
        /// <returns>返回</returns>
        public Hashtable Get_Http(string url, string encoding, Hashtable HtForm, CookieContainer myCookieContainer)
        {
            if (myCookieContainer == null)
            {
                myCookieContainer = new CookieContainer();
            }
            if (HtForm == null)
            {
                HtForm = new Hashtable();
            }
            string strResult = ""; //返回的数据
            string postdata = "";//表单项目
            try
            {
                if (encoding == null || encoding == "")
                {
                    encoding = "GB2312";
                }


                //循环加入表单项目
                IDictionaryEnumerator myEnumerator1 = HtForm.GetEnumerator();
                myEnumerator1.Reset();
                while (myEnumerator1.MoveNext())
                {
                    postdata += myEnumerator1.Key.ToString() + "=" + myEnumerator1.Value.ToString() + "&";
                }


                //初始化远程链接
                HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(url);

                //定义远程连接属性
                webrequest.Timeout = 99999;
                webrequest.AllowAutoRedirect = true;
                webrequest.MaximumAutomaticRedirections = 10;
                webrequest.Method = "POST";
                //定义会话状态
                webrequest.CookieContainer = myCookieContainer;

                string boundary = DateTime.Now.Ticks.ToString("x");
                webrequest.ContentType = "application/x-www-form-urlencoded; boundary=---------------------------" +
                                         boundary;

                //编码表单头
                //string postHeader = "";
                //postHeader = GETpostHeader(url, boundary, HtForm);
                //byte[] postHeaderBytes = Encoding.GetEncoding("GB2312").GetBytes(postHeader);
                byte[] postHeaderBytes = Encoding.GetEncoding(encoding).GetBytes(postdata);

                //设置表单头的长度
                long length = postHeaderBytes.Length;
                webrequest.ContentLength = length;

                //获取写入请求流
                Stream requestStream = webrequest.GetRequestStream();
                //写入表单头
                requestStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);
                requestStream.Close();


                //初始化远程链接类
                HttpWebResponse HttpWResp = (HttpWebResponse)webrequest.GetResponse();

                //获取cookies
                HttpWResp.Cookies = myCookieContainer.GetCookies(webrequest.RequestUri);
                //获取远程数据流
                Stream myStream = HttpWResp.GetResponseStream();
                StreamReader sr;
                //实例化读取类
                if (encoding == "")
                {
                    sr = new StreamReader(myStream, Encoding.Default);
                }
                else
                {
                    sr = new StreamReader(myStream, Encoding.GetEncoding(encoding));
                }
                //读取数据

                strResult = sr.ReadToEnd();

                //关闭对象
                sr.Close();
                myStream.Close();
                HttpWResp.Close();

                //设置返回值
                Hashtable ht = new Hashtable();
                ht.Add("cookies", myCookieContainer);
                ht.Add("html", strResult);
                ht.Add("postdata", postdata);
                ht.Add("err", "n");

                return ht;
            }
            catch (Exception exp)
            {
                //设置返回值
                Hashtable ht = new Hashtable();
                ht.Add("cookies", myCookieContainer);
                ht.Add("html", exp.ToString());
                ht.Add("postdata", postdata);
                ht.Add("err", "y");
                return ht;
            }
        }

        /// <summary>
        /// 生成表单头
        /// </summary>
        /// <param name="url"></param>
        /// <param name="boundary"></param>
        /// <param name="HtForm"></param>
        /// <returns></returns>
        public string GETpostHeader(string url, string boundary, Hashtable HtForm)
        {
            string postHeader = "";
            postHeader = postHeader + "Host: " + HAC.My_Cut_Str(url, "http://", "/", 1, false)[0].ToString();
            postHeader = postHeader + "\r\n";
            postHeader = postHeader + "Connection: Keep-Alive";
            postHeader = postHeader + "\r\n";
            postHeader = postHeader + "Accept:*/*";
            postHeader = postHeader + "\r\n";
            postHeader = postHeader + "Cache-Control: ";
            postHeader = postHeader + "\r\n";
            postHeader = postHeader + "Content-Type: application/x-www-form-urlencoded; boundary=---------------------------" + boundary;
            postHeader = postHeader + "\r\n";
            postHeader = postHeader + "User-Agent:Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; MyIE2; .NET CLR 1.1.4322; .NET CLR 1.0.3705)";
            postHeader = postHeader + "\r\n";
            postHeader = postHeader + "\r\n";

            //循环加入表单项目
            IDictionaryEnumerator myEnumerator = HtForm.GetEnumerator();
            myEnumerator.Reset();
            while (myEnumerator.MoveNext())
            {
                postHeader = postHeader + "--" + boundary;
                postHeader = postHeader + "\r\n";
                postHeader = postHeader + "Content-Disposition: form-data; name=\"" + myEnumerator.Key.ToString() + "\"";
                postHeader = postHeader + "\r\n";
                postHeader = postHeader + "\r\n";
                postHeader = postHeader + myEnumerator.Value.ToString();
                postHeader = postHeader + "\r\n";
            }

            return postHeader;
        }
    }
}
