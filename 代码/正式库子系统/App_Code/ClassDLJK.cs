using System;
using System.Collections;
using System.Data;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml;

public class ClassDLJK
{
    /// <summary>
    /// 获取http页面全部html代码的函数(仅支持普通连接，用于采集普通网页)
    /// </summary>
    /// <param name="a_strUrl">需要获取的远程页面地址</param>
    /// <param name="encoding">页面编码</param>
    /// <param name="myCookieContainer">会话状态</param>
    public Hashtable Get_Http(string a_strUrl, string encoding, CookieContainer myCookieContainer)
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
            myReq.Timeout = 99999;
            myReq.AllowAutoRedirect = true;
            myReq.MaximumAutomaticRedirections = 10;
            myReq.Method = "GET";
            myReq.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 1.1.4322)";



            WebProxy myProxy = new WebProxy();
            Uri newUri = new Uri("http://91.74.160.18:8080");
            myProxy.Address = newUri;

            myReq.Proxy = myProxy;

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
        WebResponse craboResponse;

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


            webRequest = (HttpWebRequest)WebRequest.Create(a_strUrl);
            //定义会话状态
            webRequest.CookieContainer = myCookieContainer;
            webRequest.Timeout = timeout;
            if (webRequest != null)
            {
                craboResponse = webRequest.GetResponse();
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
            //MessageBox.Show(exp.ToString());
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
    public Hashtable Get_Http(string url, string encoding, string Method_lx, Hashtable HtForm, CookieContainer myCookieContainer)
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
                encoding = "utf-8";
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
            string Method_lx_my = "";
            if (Method_lx == null || Method_lx == "")
            {
                Method_lx_my = "POST";//默认类型
            }
            else
            {
                Method_lx_my = Method_lx;
            }

            webrequest.Method = Method_lx_my;
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
        postHeader = postHeader + "Host: " + My_Cut_Str(url, "http://", "/", 1, false)[0].ToString();
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

    /// <summary>
    /// 截取与正则表达式相匹配的字符串动态数组
    /// 如果截取结果只有一个匹配，则数组中只有一个值
    /// 如果截取结果有多个，则数组中有多个值
    /// 当找到匹配后，下一个配置将从已经找到的字符串之后开始配置
    /// </summary>
    /// <param name="inputString">需要截取的字符串</param>
    /// <param name="begin_str">开始截取判定标志</param>
    /// <param name="over_str">停止截取判定标志</param>
    /// <param name="baohan">返回值是否包含判定标志(0为包含，1为不包含)</param>
    /// <param name="mustAa">是否区分大小写(true为区分，false为不区分)</param>
    public ArrayList My_Cut_Str(string inputString, string begin_str, string over_str, int baohan, bool mustAa)
    {
        Regex r;
        Match m;
        ArrayList return_str = new ArrayList();
        if (inputString == "" || begin_str == "" || over_str == "" || inputString == null || begin_str == null || over_str == null)
        {
            return return_str;
        }
        if (mustAa)
        {
            r = new Regex(begin_str + "(.*?)" + over_str, RegexOptions.Compiled | RegexOptions.Singleline);
        }
        else
        {
            r = new Regex(begin_str + "(.*?)" + over_str, RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase);
        }
        for (m = r.Match(inputString); m.Success; m = m.NextMatch())
        {
            return_str.Add(m.Groups[baohan].Value.ToString());
        }
        return return_str;

    }


    /// <summary>
    /// 根据正则表达式过滤字符串。与正则表达式匹配的字符串将被替换
    /// </summary>
    /// <param name="str">需要过滤的字符串</param>
    /// <param name="RegexStr">需要过滤的匹配规则表达式,可用"◆"来分隔多个需要过滤规则</param>
    /// <param name="TH">需要替换的字符串</param>
    /// <returns>过滤结果</returns>
    public string RegexGL(string str, string RegexStr, string TH)
    {
        string[] tempRegexARR = RegexStr.Split('★');
        string[] tempTHARR = TH.Split('★');
        for (int p = 0; p < tempRegexARR.Length; p++)
        {
            if (str == "" || str == null)
            {
                return str;
            }

            str = Regex.Replace(str, tempRegexARR[p].ToString(), tempTHARR[p].ToString());
        }

        string returnstr = str;
        if (returnstr == "" || returnstr == null)
        {
            return "";
        }
        else
        {
            return returnstr;
        }
    }

    /// <summary>
    /// 将单引号变成两个单引号，以便数据可以识别
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public string Encode(string str)
    {
        //str = str.Replace("&", "&amp;");
        //str = str.Replace("<", "&lt;");
        //str = str.Replace(">", "&gt;");
        //str = str.Replace("\"", "&quot;");
        //str = str.Replace(" ", "&nbsp;");
        //str = str.Replace("'", "&apos;");
        str = str.Replace("'", "''");
        return str;
    }


    /// <summary>
    /// 将单引号，大于或小于符号等解码成非HTML语言的字符，与Encode相对．

    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public string Decode(string str)
    {
        str = str.Replace("&amp;", "&");
        str = str.Replace("&lt;", "<");
        str = str.Replace("&gt;", ">");
        str = str.Replace("&quot;", "\"");
        str = str.Replace("&nbsp;", " ");
        str = str.Replace("&apos;", "'");
        return str;
    }

    /// <summary>
    /// 截取指定长度的字符串(从第一位开始,支持汉字)
    /// </summary>
    /// <param name="s"></param>
    /// <param name="l"></param>
    /// <returns></returns>
    public string GetNumStr(string s, int l)
    {
        string temp = s;
        if (Regex.Replace(temp, "[\u4e00-\u9fa5]", "zz", RegexOptions.IgnoreCase).Length <= l)
        {
            return temp;
        }
        for (int i = temp.Length; i >= 0; i--)
        {
            temp = temp.Substring(0, i);
            if (Regex.Replace(temp, "[\u4e00-\u9fa5]", "zz", RegexOptions.IgnoreCase).Length <= l - 3)
            {
                //超长后,加"..."符号表明
                return temp + "...";
            }
        }
        return "";
    }



    /// <summary>
    /// 测试表单提交的处理方法
    /// </summary>
    public Hashtable RunTest(Hashtable InPutHT)
    {
        //分析传入线程的参数
        string Iaction = InPutHT["tb_action"].ToString();//表单提交地址
        string IkeyX = InPutHT["tb_keyX"].ToString();//字段X名称
        string IvalueX = InPutHT["tb_valueX"].ToString();//字段X值
        string Iencode2 = InPutHT["tb_encode2"].ToString();//编码
        string IMethod = InPutHT["Method"].ToString();//表单提交类型
        CookieContainer Icookieheader = (CookieContainer)InPutHT["cookieheader"];//对话

        //获取远程内容
        //生成表单参数

        Hashtable htForm = new Hashtable();
        string[] tempIkeyXARR = IkeyX.Split('★');
        string[] tempIvalueXARR = IvalueX.Split('★');
        if (tempIkeyXARR.Length >= tempIvalueXARR.Length)//参数名称数量必须大于参数值数量
        {
            for (int p = 0; p < tempIkeyXARR.Length; p++)
            {
                htForm[tempIkeyXARR[p].ToString()] = tempIvalueXARR[p].ToString();//写入X参数
            }
        }

        Hashtable ht;
        
        ht = Get_Http(Iaction, Iencode2, IMethod, htForm, Icookieheader);


        //填充传出参数哈希表
        Hashtable OutPutHT = new Hashtable();
        OutPutHT["cookies"] = ht["cookies"];//访问后获得的cookies,次要数据
        OutPutHT["html"] = ht["html"];//访问后获取的html字符串, 主要数据
        OutPutHT["postdata"] = ht["postdata"]; //原始传入的表单项目,如id=1&name=999,参考数据
        OutPutHT["err"] = ht["err"]; //是否存在错误 标记数据

        return OutPutHT;
    }


    //由XML字符串生成DataSet
    public DataSet GetDataSet_fromxml(string xml_str)
    {
        try
        {
            string text = xml_str;
            XmlTextReader reader = new XmlTextReader(new StringReader(text));
            reader.WhitespaceHandling = WhitespaceHandling.None;//
            DataSet ds = new DataSet();
            ds.ReadXml(reader);//加载XML到DS中
            reader.Close();
            ds.Dispose();
            return ds;
        }
        catch (Exception err)
        {
            return null;
            //throw new Exception("GetDataSet方法异常：" + err.Message);

        }
    }

}
