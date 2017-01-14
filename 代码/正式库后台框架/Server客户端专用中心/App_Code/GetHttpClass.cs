using System;
//using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Text;
using System.Collections.Specialized;
using System.Collections;
//using GOCE.f_AnalyzeClass;
using Microsoft.Win32;

namespace GOCE.f_HttpClass
{
    /// <summary>
    /// ��ȡԶ����ҳ���ݵķ�������
    /// </summary>
  public  class GetHttpClass
    {

        //private HTMLAnalyzeClass HAC = new HTMLAnalyzeClass();

        public GetHttpClass()
        {
            //
            // TODO: �ڴ˴���ӹ��캯���߼�
            //
            ;
        }


        /// <summary>
        /// ��ȡhttpҳ��ȫ��html����ĺ���(��֧����ͨ���ӣ����ڲɼ���ͨ��ҳ)
        /// </summary>
        /// <param name="a_strUrl">��Ҫ��ȡ��Զ��ҳ���ַ</param>
        /// <param name="encoding">ҳ�����</param>
        /// <param name="myCookieContainer">�Ự״̬</param>
        public Hashtable Get_Http(string a_strUrl, string encoding, CookieContainer myCookieContainer)
        {
            string strResult = "";
            try
            {
                if (myCookieContainer == null)
                {
                    myCookieContainer = new CookieContainer();
                }

                //ʵ����HttpWebRequest��,������������
                HttpWebRequest myReq = (HttpWebRequest)HttpWebRequest.Create(a_strUrl);
                //���Ӳ���
                myReq.Timeout = 99999;
                myReq.AllowAutoRedirect = true;
                myReq.MaximumAutomaticRedirections = 10;
                myReq.Method = "GET";
                myReq.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 1.1.4322)";



                //WebProxy myProxy = new WebProxy();
                //Uri newUri = new Uri("http://91.74.160.18:8080");
                //myProxy.Address = newUri;

                //myReq.Proxy = myProxy;

                //����Ự״̬
                myReq.CookieContainer = myCookieContainer; 
                //myReq.PreAuthenticate = true;
                //������������
                HttpWebResponse HttpWResp = (HttpWebResponse)myReq.GetResponse();
                //��ȡcookies
                HttpWResp.Cookies = myCookieContainer.GetCookies(myReq.RequestUri); 
                //��ȡԶ��������
                Stream myStream = HttpWResp.GetResponseStream();
                StreamReader sr;
                //ʵ������ȡ��
                if (encoding == "")
                {
                    sr = new StreamReader(myStream, Encoding.Default);
                }
                else
                {
                    sr = new StreamReader(myStream, Encoding.GetEncoding(encoding));
                }
                //��ȡ����
                strResult = sr.ReadToEnd();

                //�رն���
                sr.Close();
                myStream.Close();
                HttpWResp.Close();

                //���÷���ֵ
                Hashtable ht = new Hashtable();
                ht.Add("cookies", myCookieContainer);
                ht.Add("html", strResult);
                ht.Add("err", "n");

                return ht;
            }
            catch (Exception exp)
            {
                //���÷���ֵ
                Hashtable ht = new Hashtable();
                ht.Add("cookies", myCookieContainer);
                ht.Add("html", exp.ToString());
                ht.Add("err", "y");
                return ht;
            }

        }




        ///// <summary>
        ///// ����Զ���ļ���ָ��Ŀ¼(ֱ֧�ֱ�׼��ʽ)
        ///// </summary>
        ///// <param name="a_strUrl">Զ���ļ�</param>
        ///// <param name="timeout">��ʱ</param>
        ///// <param name="filepath">������·��,��Ҫ������\</param>
        ///// <param name="keepname">�Ƿ񱣳�Դ�ļ�����</param>
        ///// <param name="timepath">�Ƿ���������Զ������ļ���</param>
        ///// <param name="myCookieContainer">�Ự״̬</param>
        ///// <returns>����ͼƬ·��(������ԭ��Ŀ¼),
        ///// ���紫��filepath="d:\temp\",
        ///// ��ͼƬ��·��Ϊd:\temp\20070303\084534_23.jpg
        ///// ��ô���ص�·����20070303\084534_23.jpg</returns>
        //public string Get_Img(string a_strUrl, int timeout, string filepath, bool keepname, bool timepath, CookieContainer myCookieContainer)
        //{
        //    HttpWebRequest webRequest;
        //    WebResponse craboResponse;

        //    Stream remoteStream;
        //    Stream localStream = null;

        //    int d;
        //    int x;
        //    string filetype; //�ļ�����
        //    string filename; //�ļ���(û�е�)

        //    string newpath_d = ""; //ʱ�����,������Ŀ¼����
        //    string newpath = ""; //���������ɵ�Ŀ¼·��

        //    try
        //    {
        //        //��ȡԶ�̵�ַ��׺
        //        d = a_strUrl.LastIndexOf(".");
        //        x = a_strUrl.LastIndexOf("/");
        //        filetype = a_strUrl.Substring(d + 1);
        //        filename = a_strUrl.Substring(x + 1).Replace("." + filetype, "");


        //        if (timepath)
        //        {
        //            newpath_d = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString();
        //            newpath = newpath_d + @"\";

        //            filepath = filepath + newpath;
        //            // ���Ŀ¼�Ƿ����
        //            if (!Directory.Exists(filepath))
        //            {
        //                Directory.CreateDirectory(filepath);
        //            }
        //        }
        //        if (!keepname)//��Ҫ��������
        //        {
        //            filename = newpath_d + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + GetRandomNum(2, 3, 1, 99).ToString();
        //        }


        //        webRequest = (HttpWebRequest)WebRequest.Create(a_strUrl);
        //        //����Ự״̬
        //        webRequest.CookieContainer = myCookieContainer; 
        //        webRequest.Timeout = timeout;
        //        if (webRequest != null)
        //        {
        //            craboResponse = webRequest.GetResponse();
        //            if (craboResponse != null)
        //            {
        //                remoteStream = craboResponse.GetResponseStream();
        //                localStream = File.Create(filepath + filename + "." + filetype);
        //                byte[] buffer = new byte[1254];
        //                int bytesRead;
        //                do
        //                {
        //                    bytesRead = remoteStream.Read(buffer, 0, buffer.Length);
        //                    localStream.Write(buffer, 0, bytesRead);
        //                } while (bytesRead > 0);
        //            }
        //        }
        //        return newpath + filename + "." + filetype;
        //    }
        //    catch (Exception exp)
        //    {
        //        MessageBox.Show(exp.ToString());
        //        return "err";
        //    }
        //    finally
        //    {
        //        if (localStream != null)
        //        {
        //            localStream.Close();
        //        }
        //    }

        //}

        /// <summary>
        /// ���ɲ��ظ������
        /// </summary>
        /// <param name="i">���������</param>
        /// <param name="length">��������</param>
        /// <param name="up">����</param>
        /// <param name="down">����</param>
        /// <returns></returns>
        private int GetRandomNum(int i, int length, int up, int down)
        {
            int iFirst = 0;
            Random ro = new Random(i * length * unchecked((int)DateTime.Now.Ticks));
            iFirst = ro.Next(up, down);
            return iFirst;
        }


        /// <summary>
        /// ��ȡhttpҳ��ȫ��html����ĺ���
        /// (���أ���֧���ύ��ͨ��,��֧���ύ�ļ�,��֧��.net��)
        /// (������е�¼��Ϣ,��ȻҲ���������ɼ��Ǳ�ҳ��,
        /// ����Щ�Ǳ�ҳ�治֧��д����,��˿����޷��ɼ�,Ӧʹ����ͨ�ɼ�����)
        /// </summary>
        /// <param name="url"></param>
        /// <param name="encoding"></param>
        /// <param name="HtForm"></param>
        /// <param name="myCookieContainer"></param>
        /// <returns>����</returns>
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
            string strResult = ""; //���ص�����
            string postdata = "";//����Ŀ
            try
            {
                if (encoding == null || encoding == "")
                {
                    encoding = "GB2312";
                }


                //ѭ���������Ŀ
                IDictionaryEnumerator myEnumerator1 = HtForm.GetEnumerator();
                myEnumerator1.Reset();
                while (myEnumerator1.MoveNext())
                {
                    postdata += myEnumerator1.Key.ToString() + "=" + myEnumerator1.Value.ToString() + "&";
                }


                //��ʼ��Զ������
                HttpWebRequest webrequest = (HttpWebRequest) WebRequest.Create(url);

                //����Զ����������
                webrequest.Timeout = 99999;
                webrequest.AllowAutoRedirect = true;
                webrequest.MaximumAutomaticRedirections = 10;
                webrequest.Method = "POST";
                //����Ự״̬
                webrequest.CookieContainer = myCookieContainer; 

                string boundary = DateTime.Now.Ticks.ToString("x");
                webrequest.ContentType = "application/x-www-form-urlencoded; boundary=---------------------------" +
                                         boundary;

                //�����ͷ
                //string postHeader = "";
                //postHeader = GETpostHeader(url, boundary, HtForm);
                //byte[] postHeaderBytes = Encoding.GetEncoding("GB2312").GetBytes(postHeader);
                byte[] postHeaderBytes = Encoding.GetEncoding(encoding).GetBytes(postdata);

                //���ñ�ͷ�ĳ���
                long length = postHeaderBytes.Length;
                webrequest.ContentLength = length;

                //��ȡд��������
                Stream requestStream = webrequest.GetRequestStream();
                //д���ͷ
                requestStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);
                requestStream.Close();


                //��ʼ��Զ��������
                HttpWebResponse HttpWResp = (HttpWebResponse) webrequest.GetResponse();

                //��ȡcookies
                HttpWResp.Cookies = myCookieContainer.GetCookies(webrequest.RequestUri); 
                //��ȡԶ��������
                Stream myStream = HttpWResp.GetResponseStream();
               
                StreamReader sr;
                //ʵ������ȡ��
                if (encoding == "")
                {
                    sr = new StreamReader(myStream, Encoding.Default);
                }
                else
                {
                    sr = new StreamReader(myStream, Encoding.GetEncoding(encoding));
                }
                //��ȡ����
             
                strResult = sr.ReadToEnd();

                //�رն���
                sr.Close();
                myStream.Close();
                HttpWResp.Close();

                //���÷���ֵ
                Hashtable ht = new Hashtable();
                ht.Add("cookies", myCookieContainer);
                ht.Add("html", strResult);
                ht.Add("postdata", postdata);
                ht.Add("err", "n");

                return ht;
            }
            catch (Exception exp)
            {
                //���÷���ֵ
                Hashtable ht = new Hashtable();
                ht.Add("cookies", myCookieContainer);
                ht.Add("html", exp.ToString());
                ht.Add("postdata", postdata);
                ht.Add("err", "y");
                return ht;
            }
        }

        ///// <summary>
        ///// ���ɱ�ͷ
        ///// </summary>
        ///// <param name="url"></param>
        ///// <param name="boundary"></param>
        ///// <param name="HtForm"></param>
        ///// <returns></returns>
        //public string GETpostHeader(string url, string boundary, Hashtable HtForm)
        //{
        //    string postHeader = "";
        //    postHeader = postHeader + "Host: " + HAC.My_Cut_Str(url, "http://", "/", 1, false)[0].ToString();
        //    postHeader = postHeader + "\r\n";
        //    postHeader = postHeader + "Connection: Keep-Alive";
        //    postHeader = postHeader + "\r\n";
        //    postHeader = postHeader + "Accept:*/*";
        //    postHeader = postHeader + "\r\n";
        //    postHeader = postHeader + "Cache-Control: ";
        //    postHeader = postHeader + "\r\n";
        //    postHeader = postHeader + "Content-Type: application/x-www-form-urlencoded; boundary=---------------------------" + boundary;
        //    postHeader = postHeader + "\r\n";
        //    postHeader = postHeader + "User-Agent:Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; MyIE2; .NET CLR 1.1.4322; .NET CLR 1.0.3705)";
        //    postHeader = postHeader + "\r\n";
        //    postHeader = postHeader + "\r\n";

        //    //ѭ���������Ŀ
        //    IDictionaryEnumerator myEnumerator = HtForm.GetEnumerator();
        //    myEnumerator.Reset();
        //    while (myEnumerator.MoveNext())
        //    {
        //        postHeader = postHeader + "--" + boundary;
        //        postHeader = postHeader + "\r\n";
        //        postHeader = postHeader + "Content-Disposition: form-data; name=\"" + myEnumerator.Key.ToString() + "\"";
        //        postHeader = postHeader + "\r\n";
        //        postHeader = postHeader + "\r\n";
        //        postHeader = postHeader + myEnumerator.Value.ToString();
        //        postHeader = postHeader + "\r\n";
        //    }

        //    return postHeader;
        //}
    }
}
