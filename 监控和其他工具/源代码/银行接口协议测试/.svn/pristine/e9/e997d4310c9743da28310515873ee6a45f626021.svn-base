using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace FMBANKpufa
{
    /// <summary>
    /// 银行接口处理类(数据包比特流长度不能过大)
    /// </summary>
    public class ClassSend
    {

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

        public ClassSend()
        {
            //初始化该银行的一些固定默认参数

            EncodingName = "GB2312";
            Dip = "10.112.26.73";
            Ddk = "40012";
            //F＋3位包头长度＋5位包体长度＋8位券商号＋3位密码加密标志＋8位MAC值
            Dbaotou = "F028包体长度60050000加密标志三位MAC值八位";
        }

        /// <summary>
        /// 可以在初始化时指定一些关键参数
        /// </summary>
        /// <param name="temp_EncodingName"></param>
        /// <param name="temp_Dip"></param>
        /// <param name="temp_Ddk"></param>
        /// <param name="temp_Dbaotou"></param>
        public ClassSend(string temp_EncodingName, string temp_Dip, string temp_Ddk, string temp_Dbaotou)
        {
            EncodingName = temp_EncodingName;
            Dip = temp_Dip;
            Ddk = temp_Ddk;
            Dbaotou = temp_Dbaotou;
        }

        /// <summary>
        /// 发送数据包到指定端点，使用短连接，发完等待收到反馈后即立刻关闭连接。
        /// </summary>
        /// <param name="ip">IP地址</param>
        /// <param name="dk">端口</param>
        /// <param name="sendmsg">数据包</param>
        /// <returns>返回值：发包函数执行阶段|返回的数据包|返回的日志</returns>
        public Hashtable BeginSend(string ip, string dk,byte[] sendmsg)
        {
            //初始化返回值
            Hashtable reHT = new Hashtable();
            reHT["发包函数执行阶段"] = "";
            reHT["返回的数据包"] = null;
            reHT["返回的日志"] = "";



            Socket sock = null;
            IPEndPoint serverFullAddr;
            try
            {
                //初始化套接字
                sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                sock.ReceiveTimeout = 20000; //二十秒没有反馈即为超时
                sock.SendTimeout = 20000;//二十秒没有发送出去，即为超时
                //指定本地主机地址和端口号开启连接
                serverFullAddr = new IPEndPoint(IPAddress.Parse(ip), int.Parse(dk));//设置发送目标IP，端口
                sock.Connect(serverFullAddr);
                reHT["发包函数执行阶段"] = "已建立连接";
                reHT["返回的日志"] = reHT["返回的日志"] + "连接建立时间：" + System.DateTime.Now.ToString("yyyy年MM月dd日HH时mm分ss秒fff毫秒") + Environment.NewLine;

                //发送数据
                int f = sock.Send(sendmsg);
                reHT["发包函数执行阶段"] = "已发送指令数据";
                reHT["返回的日志"] = reHT["返回的日志"] + "发出包长度：" + f.ToString() + "字节；" + "发出时间：" + System.DateTime.Now.ToString("yyyy年MM月dd日HH时mm分ss秒fff毫秒") + Environment.NewLine;

                //接收数据
                byte[] ReturnMsg = new byte[9999];//以9999字节为窗口接收数据
                int r = sock.Receive(ReturnMsg);
                reHT["发包函数执行阶段"] = "已接收反馈数据";
                reHT["返回的日志"] = reHT["返回的日志"] + "反馈包长度：" + r.ToString() + "字节；" + "接收时间：" + System.DateTime.Now.ToString("yyyy年MM月dd日HH时mm分ss秒fff毫秒") + Environment.NewLine;
                reHT["返回的数据包"] = ReturnMsg;

            }
            catch (Exception ex)
            {
                reHT["返回的日志"] = reHT["返回的日志"] + "发生错误：" + ex.ToString() + Environment.NewLine;
            }
            sock.Close();
            reHT["发包函数执行阶段"] = "完成本次收发并已关闭连接！";

            return reHT;
        }


        /// <summary>
        /// 发送数据包到默认端点，使用短连接，发完等待收到反馈后即立刻关闭连接。
        /// </summary>
        /// <param name="sendmsg">数据包</param>
        /// <returns>返回值：发包函数执行阶段|返回的数据包|返回的日志</returns>
        public Hashtable BeginSend(byte[] sendmsg)
        {
            return BeginSend(Dip, Ddk, sendmsg);
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


            

            //根据协议规则，把数据集转化成一个字符串

            //处理包头
            string tou = Dbaotou;
            tou = tou.Replace("加密标志三位", "000").Replace("MAC值八位", "00000000");
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

            //得到完整包字符串
            string messagestr = tou + baoti1 + baoti2 + baoti3;

            //把转化好的字符串，用指定编码格式，转化成字节流
            byte[] byteSend = System.Text.Encoding.GetEncoding(EncodingName).GetBytes(messagestr);

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

    }
}
