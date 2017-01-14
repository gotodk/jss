using System;
using System.Collections;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Text;
using 公用通讯协议类库.共用类库;
using 公用通讯协议类库.消息片类库;
using 客户端主程序通讯类库.客户端类库.UDP通讯;

namespace 客户端主程序通讯类库.客户端类库.UDP通讯
{
    /// <summary>
    /// UDPSlidingWindow udp滑动窗口类,用于接收和发送消息分片。
    /// </summary>
    public class UDPSlidingWindow
    {
        //=============================================接收变量
        /// <summary>
        /// 用于记录接受到的消息片类的哈希表
        /// </summary>
        public Hashtable receiveList = new Hashtable();

        /// <summary>
        /// 用于区分不同实例，记录不同传送要求的哈希表，里面记录的是receiveList哈希表
        /// </summary>
        public Hashtable ht_receiveList = new Hashtable();

        /// <summary>
        /// 初始化滑动窗口
        /// </summary>
        //public SlidingWindow receiveWindow = new SlidingWindow(5);






        //=============================================================发送变量
        /// <summary>
        /// 队列数据发送完毕
        /// </summary>
        public static byte MSG_COMPLETE = 0x01;
        /// <summary>
        /// 片发送状态
        /// </summary>
        public static byte MSG_SLICE_SENDED = 0x03;
        /// <summary>
        /// 片完毕应答
        /// </summary>
        public static byte MSG_SLICE_COMPLETE = 0x04;
        /// <summary>
        /// 窗口最大等待时间
        /// </summary>
        public static long MAX_WAIT_TIME = 300000;//窗口最大等待时间
        /// <summary>
        /// 窗口最大重发次数
        /// </summary>
        public static int MAX_RETRY_TIME = 5;//窗口最大重发次数

        /// <summary>
        /// 队列数据发送状态
        /// </summary>
        public byte ret;
        /// <summary>
        /// 重试次数
        /// </summary>
        public int retryTime = 0;
        /// <summary>
        /// 定义滑动窗口
        /// </summary>
        private SlidingWindow sendWindow = new SlidingWindow(5);
        /// <summary>
        /// 分片消息哈希表
        /// </summary>
        private Hashtable sendList = new Hashtable();
        /// <summary>
        /// 窗口发送队列哈希表
        /// </summary>
        public Hashtable state = new Hashtable();

        /// <summary>
        /// 窗口发送队列哈希表上的哈希表。里面记录的是state.用于区分不同传送要求
        /// </summary>
        public Hashtable ht_state = new Hashtable();

        /// <summary>
        /// 要发送的文件流
        /// </summary>
        public byte[] sendBuffer_file;


        /// <summary>
        /// 发送目标终结点
        /// </summary>
        public System.Net.IPEndPoint target;

        /// <summary>
        /// 接收到的包
        /// </summary>
        public object pack_input;

        /// <summary>
        /// 要发送的消息的唯一GUID
        /// </summary>
        public string sendGUID;

        /// <summary>
        /// 要发送的目标帐号名
        /// </summary>
        public string sendUsernameto;

        /// <summary>
        /// 要发送的消息的类型
        /// </summary>
        public string sendMSGTYPE;

        /// <summary>
        /// 监听数据包线程主类
        /// </summary>
        public Client ClientThread;

        /// <summary>
        /// 多线程同步锁
        /// </summary>
        private Mutex mut = new Mutex();

        public UDPSlidingWindow()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //

        }
        public UDPSlidingWindow(Client CC)
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
            ClientThread = CC;
        }
        public UDPSlidingWindow(Client CC, string sendGUID_temp, string sendMSGTYPE_temp, string sendUsernameto_temp)
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
            ClientThread = CC;
            sendGUID = sendGUID_temp;
            sendMSGTYPE = sendMSGTYPE_temp;
            sendUsernameto = sendUsernameto_temp;
        }
        public UDPSlidingWindow(Client CC, object pack_temp)
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
            ClientThread = CC;
            pack_input = pack_temp;
        }


        /// <summary>
        /// 接收发送来的数据包
        /// </summary>
        public void Revieve_file()
        {
            MsgSlice ms = (MsgSlice)pack_input;
            //receiveList = (Hashtable)ht_receiveList[ms.onlyonefloat];

            ((Hashtable)(ht_receiveList[ms.onlyonefloat])).Remove(ms.getSliceIndex());
            ((Hashtable)(ht_receiveList[ms.onlyonefloat])).Add(ms.getSliceIndex(), ms);

            


            if (!ms.isSimple)
            {
                //show_DForThread("接收到数据片：" + ms.getSliceIndex());
                //一般帧，返回响应信息
                byte[] actBt = new byte[9];
                long index = ms.getSliceIndex();
                byte[] hBt = new byte[1];
                byte[] iBt = BitConverter.GetBytes(index);
                hBt[0] = UDPSlidingWindow.MSG_SLICE_COMPLETE;
                actBt = MsgSlice.byteAdd(hBt, iBt);  //产生应答消息,格式:一字节消息片完成标志 紧跟 八字节消息索引

                //定义接收应答类
                FileRevieveATC frACT = new FileRevieveATC();
                frACT.Byte_ACT = actBt;
                frACT.OnlyOneFloat = ms.onlyonefloat;

                byte[] BtACT = FormatterHelper.Serialize(frACT);
                ClientThread.client.Send(BtACT, BtACT.Length, ClientThread.remotePoint); //发送消息片接收完成应答消息

                //回显
                show_DForThread("(接收)唯一标示:" + ms.onlyonefloat + "\n已接收了" + (index + 1).ToString() + "片，共" + ms.sliceAllnum + "片！");

                ////回显
                //ClientThread.OutPutHT.Clear();
                //ClientThread.OutPutHT["数据片完成(接收)"] = index;
                //ClientThread.OutPutHT["总片数"] = ms.sliceAllnum;
                //ClientThread.OutPutHT["唯一标示"] = ms.onlyonefloat;
                //ClientThread.OutPutHT["调试"] = "";
                //ClientThread.DForThread(ClientThread.OutPutHT);

                if (ms.isEndSlice()) //如果收到最后一片
                {//接收到最后帧
                    //show_DForThread("收到最后一片" + ms.onlyonefloat);
                    if (ms.getSliceIndex() + 1 == (long)(((Hashtable)(ht_receiveList[ms.onlyonefloat])).Count)) //判定是否缺包
                    {//全部接收完毕,则发送“接收完毕”指令
                        //show_DForThread("数据包完整" + ms.onlyonefloat);
                        actBt = new byte[1];
                        actBt[0] = UDPSlidingWindow.MSG_COMPLETE;  //产生应答消息,格式: 一字节消息全部完成标志

                        //定义接收应答类
                        FileRevieveATC frACT_end = new FileRevieveATC();
                        frACT_end.Byte_ACT = actBt;
                        frACT_end.OnlyOneFloat = ms.onlyonefloat;
                        byte[] BtACT_end = FormatterHelper.Serialize(frACT_end);
                        ClientThread.client.Send(BtACT_end, BtACT_end.Length, ClientThread.remotePoint);

                        //回显接收完成
                        show_DForThread("(接收)唯一标示:" + ms.onlyonefloat + "\n已接收完成，共" + ms.sliceAllnum + "片！");

                        //重组数据
                        Hashtable ht_sendlist = (Hashtable)(ht_receiveList[ms.onlyonefloat]);//接受到的数据
                        byte[] msgBt = MsgSlice.uniteMsg(ht_sendlist);//

                        //清理
                        mut.WaitOne();
                        ((Hashtable)(ClientThread.receive_usw.ht_receiveList[ms.onlyonefloat])).Clear();
                        ClientThread.receive_usw.ht_receiveList.Remove(ms.onlyonefloat);
                        mut.ReleaseMutex();
                        
                        //处理接受到的重组后的消息片处理
                        UDPCommunicate UDPC = new UDPCommunicate(ClientThread);
                        UDPC.UDP_Receive(msgBt);



                    }
                    else
                    {//中间有缺包，遍历最后接收窗口，找到缺包，并请求重发。（可有可无？）
                        //Console.WriteLine((ms.getSliceIndex() + 1) + "==" + receiveList.Count + "数据缺包，待处理。。。。。");
                    }
                }
            }
        }







        /// <summary>
        /// 开始发送数据包
        /// </summary>
        public void sendThread()
        {
            string weiyibiaozhi = sendGUID + "|" + sendMSGTYPE + "|" + sendUsernameto;
            //byte[] sendBuffer = System.Text.ASCIIEncoding.UTF8.GetBytes(msg);
            byte[] sendBuffer = ((UDPSlidingWindow)(ClientThread.ht_UDPsw[weiyibiaozhi])).sendBuffer_file;
            //Console.WriteLine("发送开始时间：" + DateTime.Now);
            //Console.WriteLine("待发数据长度：" + sendBuffer.Length / 1024 + "K Byte】");
            

            ((UDPSlidingWindow)(ClientThread.ht_UDPsw[weiyibiaozhi])).sendList = MsgSlice.splitMsg(sendBuffer); //对消息进行分片
            show_DForThread("(发送)唯一标示:" + weiyibiaozhi + "\n准备发送数据,待发数据总长度" + CountSize(sendBuffer.LongLength) + "\n共分为：" + ((UDPSlidingWindow)(ClientThread.ht_UDPsw[weiyibiaozhi])).sendList.Count + "片");

            while (ClientThread.ht_UDPsw.ContainsKey(weiyibiaozhi) && ((UDPSlidingWindow)(ClientThread.ht_UDPsw[weiyibiaozhi])).ret != UDPSlidingWindow.MSG_COMPLETE)
            {
                bool wbzf = ClientThread.ht_UDPsw.ContainsKey(weiyibiaozhi);
                DateTime dtf = ((UDPSlidingWindow)(ClientThread.ht_UDPsw[weiyibiaozhi])).sendWindow.lastActiveTime.AddMilliseconds(UDPSlidingWindow.MAX_WAIT_TIME);
                if (wbzf && ((DateTime.Now) > dtf))
                {//窗口停留超时，则重发落在窗口内未标志为“已接收”的所有包
                    if (ClientThread.ht_UDPsw.ContainsKey(weiyibiaozhi) && ((UDPSlidingWindow)(ClientThread.ht_UDPsw[weiyibiaozhi])).retryTime >= UDPSlidingWindow.MAX_RETRY_TIME)
                    {//重发次数超过设定值，退出。
                        //Console.WriteLine("重发次数超过" + UDPSlidingWindow.MAX_RETRY_TIME + "，发送数据失败！请检查网络是否正常连接。");
                        show_DForThread("(发送)唯一标示:" + weiyibiaozhi + "\n重发次数超过" + UDPSlidingWindow.MAX_RETRY_TIME + "，发送数据失败！");
                        return;
                    }
                    //Console.WriteLine((DateTime.Now).ToString() + "===>" + sendWindow.lastActiveTime.AddMilliseconds(UDPSlidingWindow.MAX_WAIT_TIME));
                    for (long i = ((UDPSlidingWindow)(ClientThread.ht_UDPsw[weiyibiaozhi])).sendWindow.getWindowHead() - ((UDPSlidingWindow)(ClientThread.ht_UDPsw[weiyibiaozhi])).sendWindow.getUsedSize(); i < ((UDPSlidingWindow)(ClientThread.ht_UDPsw[weiyibiaozhi])).sendWindow.getWindowHead(); i++)
                    {
                        MsgSlice tmpMs = (MsgSlice)(((UDPSlidingWindow)(ClientThread.ht_UDPsw[weiyibiaozhi])).sendList[i]);
                        if (tmpMs != null && (((UDPSlidingWindow)(ClientThread.ht_UDPsw[weiyibiaozhi])).state[i] == null || (byte)(((UDPSlidingWindow)(ClientThread.ht_UDPsw[weiyibiaozhi])).state[i]) != UDPSlidingWindow.MSG_SLICE_COMPLETE))
                        {
                            //Console.WriteLine("重新发送数据：" + i);
                            show_DForThread("(发送)唯一标示:" + weiyibiaozhi + "\n重新发送数据：" + i);

                            tmpMs.onlyonefloat = weiyibiaozhi; //定义唯一标志,用于多组数据包同时传送
                            byte[] tmpBt = FormatterHelper.Serialize(tmpMs);
                            ClientThread.client.Send(tmpBt, tmpBt.Length, ((UDPSlidingWindow)(ClientThread.ht_UDPsw[weiyibiaozhi])).target);
                            ((UDPSlidingWindow)(ClientThread.ht_UDPsw[weiyibiaozhi])).state.Remove(i);
                            ((UDPSlidingWindow)(ClientThread.ht_UDPsw[weiyibiaozhi])).state.Add(i, UDPSlidingWindow.MSG_SLICE_SENDED);//将窗口内新发送包加入到等待回应队列
                        }
                    }
                    ((UDPSlidingWindow)(ClientThread.ht_UDPsw[weiyibiaozhi])).retryTime++;
                    ((UDPSlidingWindow)(ClientThread.ht_UDPsw[weiyibiaozhi])).sendWindow.lastActiveTime = DateTime.Now;
                }
                while (ClientThread.ht_UDPsw.ContainsKey(weiyibiaozhi) && (((UDPSlidingWindow)(ClientThread.ht_UDPsw[weiyibiaozhi])).state[(long)(((UDPSlidingWindow)(ClientThread.ht_UDPsw[weiyibiaozhi])).sendWindow.getWindowHead() - ((UDPSlidingWindow)(ClientThread.ht_UDPsw[weiyibiaozhi])).sendWindow.getUsedSize())] != null && (byte)(((UDPSlidingWindow)(ClientThread.ht_UDPsw[weiyibiaozhi])).state[(long)(((UDPSlidingWindow)(ClientThread.ht_UDPsw[weiyibiaozhi])).sendWindow.getWindowHead() - ((UDPSlidingWindow)(ClientThread.ht_UDPsw[weiyibiaozhi])).sendWindow.getUsedSize())]) == MSG_SLICE_COMPLETE || ((UDPSlidingWindow)(ClientThread.ht_UDPsw[weiyibiaozhi])).sendWindow.getUsedSize() < ((UDPSlidingWindow)(ClientThread.ht_UDPsw[weiyibiaozhi])).sendWindow.getMaxSize()) && ((UDPSlidingWindow)(ClientThread.ht_UDPsw[weiyibiaozhi])).sendWindow.getWindowHead() < ((UDPSlidingWindow)(ClientThread.ht_UDPsw[weiyibiaozhi])).sendList.Count)
                {//滑动窗口下标数据已经完成且数据未发完，则发送下一个包，并且窗口前移。
                    ((UDPSlidingWindow)(ClientThread.ht_UDPsw[weiyibiaozhi])).retryTime = 0;
                    MsgSlice tmpMs = (MsgSlice)(((UDPSlidingWindow)(ClientThread.ht_UDPsw[weiyibiaozhi])).sendList[(long)(((UDPSlidingWindow)(ClientThread.ht_UDPsw[weiyibiaozhi])).sendWindow.getWindowHead())]);
                    //Console.WriteLine("准备发送："+sendWindow.getWindowHead()+":"+tmpMs.isEndSlice());

                    tmpMs.onlyonefloat = weiyibiaozhi; //定义唯一标志,用于多组数据包同时传送
                    byte[] tmpBt = FormatterHelper.Serialize(tmpMs);
                    ClientThread.client.Send(tmpBt, tmpBt.Length, ((UDPSlidingWindow)(ClientThread.ht_UDPsw[weiyibiaozhi])).target);
                    ((UDPSlidingWindow)(ClientThread.ht_UDPsw[weiyibiaozhi])).sendWindow.moveAhead();
                    ((UDPSlidingWindow)(ClientThread.ht_UDPsw[weiyibiaozhi])).state.Add(((UDPSlidingWindow)(ClientThread.ht_UDPsw[weiyibiaozhi])).sendWindow.getWindowHead(), UDPSlidingWindow.MSG_SLICE_SENDED);//将窗口内新发送包加入到等待回应队列
                    Thread.Sleep(1);
                }
                Thread.Sleep(1);
                
            }
            //发送线程结束，清理实例
            mut.WaitOne();
            ClientThread.ht_UDPsw.Remove(weiyibiaozhi);
            mut.ReleaseMutex();

        }


        /// <summary>
        /// 接收发送应答消息
        /// </summary>
        /// <param name="act"></param>
        /// <param name="OnlyOneFloat"></param>
        public void recieveThreadACT(byte[] act, string OnlyOneFloat)
        {
            if (act[0] == UDPSlidingWindow.MSG_SLICE_COMPLETE)
            {//接收到片完成信息
                long index = BitConverter.ToInt64(act, 1);
                ((UDPSlidingWindow)(ClientThread.ht_UDPsw[OnlyOneFloat])).state.Remove(index);
                ((UDPSlidingWindow)(ClientThread.ht_UDPsw[OnlyOneFloat])).state.Add(index, UDPSlidingWindow.MSG_SLICE_COMPLETE);

                show_DForThread("(发送)唯一标示:" + OnlyOneFloat + "\n已确认发送了" + (index + 1).ToString() + "片，共" + ((UDPSlidingWindow)(ClientThread.ht_UDPsw[OnlyOneFloat])).sendList.Count.ToString() + "片！");

                showProgress_DForThread("发送","正在传输",OnlyOneFloat, index, (long)(((UDPSlidingWindow)(ClientThread.ht_UDPsw[OnlyOneFloat])).sendList.Count));


                //ClientThread.OutPutHT.Clear();
                //ClientThread.OutPutHT["数据片完成"] = index.ToString();
                //ClientThread.OutPutHT["数据片总量"] = ((UDPSlidingWindow)(ClientThread.ht_UDPsw[OnlyOneFloat])).sendList.Count.ToString();
                //ClientThread.OutPutHT["唯一标示"] = ((UDPSlidingWindow)(ClientThread.ht_UDPsw[OnlyOneFloat])).sendGUID + "|" + ((UDPSlidingWindow)(ClientThread.ht_UDPsw[OnlyOneFloat])).sendMSGTYPE;
                //ClientThread.OutPutHT["调试"] = "";
                //ClientThread.DForThread(ClientThread.OutPutHT);
            }
            else if (act[0] == UDPSlidingWindow.MSG_COMPLETE)
            {//接收到“数据接收完毕”信息
                
                show_DForThread("(发送)唯一标示:" + OnlyOneFloat + "\n已确认发送完毕，共" + ((UDPSlidingWindow)(ClientThread.ht_UDPsw[OnlyOneFloat])).sendList.Count.ToString() + "片！");

                showProgress_DForThread("发送","传输完成", OnlyOneFloat, (long)(((UDPSlidingWindow)(ClientThread.ht_UDPsw[OnlyOneFloat])).sendList.Count), (long)(((UDPSlidingWindow)(ClientThread.ht_UDPsw[OnlyOneFloat])).sendList.Count));

                ((UDPSlidingWindow)(ClientThread.ht_UDPsw[OnlyOneFloat])).ret = UDPSlidingWindow.MSG_COMPLETE;
                //ClientThread.OutPutHT.Clear();
                //ClientThread.OutPutHT["数据片完成"] = ((UDPSlidingWindow)(ClientThread.ht_UDPsw[OnlyOneFloat])).sendList.Count.ToString();
                //ClientThread.OutPutHT["数据片总量"] = ((UDPSlidingWindow)(ClientThread.ht_UDPsw[OnlyOneFloat])).sendList.Count.ToString();
                //ClientThread.OutPutHT["唯一标示"] = ((UDPSlidingWindow)(ClientThread.ht_UDPsw[OnlyOneFloat])).sendGUID + "|" + ((UDPSlidingWindow)(ClientThread.ht_UDPsw[OnlyOneFloat])).sendMSGTYPE;
                //ClientThread.OutPutHT["调试"] = "";
                //ClientThread.DForThread(ClientThread.OutPutHT);

                //ClientThread.ht_UDPsw.Remove(OnlyOneFloat);
            }
        }


        /// <summary>
        /// 调用委托，显示滑动窗口发送过程中的问题
        /// </summary>
        /// <param name="msginfo"></param>
        private void show_DForThread(string msginfo)
        {
            //打洞消息不回显
            if (msginfo.IndexOf("公用通讯协议类库.P2P类库.TrashMessage") > -1 || msginfo.IndexOf("公用通讯协议类库.P2P类库.ACKTrashMessage") > -1)
            {
                return;
            }
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["事件消息"] = msginfo;
            OutPutHT["调试"] = "";
            //调用委托，回显
            ClientThread.DForThread(OutPutHT);
        }

        /// <summary>
        /// 调用委托，显示发送进度条
        /// </summary>
        /// <param name="ForS">接收进度条还是发送进度条，接收/发送</param>
        /// <param name="ZT">进度条状态，准备传输/正在传输/传输完成</param>
        /// <param name="IDFloat">唯一标示</param>
        /// <param name="SliceIndex">进度条进度</param>
        /// <param name="SliceCount">进度条总长度</param>
        private void showProgress_DForThread(string ForS,string ZT,string IDFloat, long SliceIndex, long SliceCount)
        {
            Hashtable ProgressOutHT = new Hashtable();
            ProgressOutHT["进度条类型"] = ForS;
            ProgressOutHT["进度条状态"] = ZT;
            ProgressOutHT["进度条标示"] = IDFloat;
            ProgressOutHT["进度条进度"] = SliceIndex;
            ProgressOutHT["进度条总长度"] = SliceCount;
            ClientThread.DForThread(ProgressOutHT);
        }

        /// <summary>
        /// 计算文件大小函数(保留两位小数),Size为字节大小
        /// </summary>
        /// <param name="Size">初始文件大小</param>
        /// <returns></returns>
        private string CountSize(long Size)
        {
            string m_strSize = "";
            long FactSize = 0;
            FactSize = Size;
            if (FactSize < 1024.00)
                m_strSize = FactSize.ToString("F2") + " Byte";
            else if (FactSize >= 1024.00 && FactSize < 1048576)
                m_strSize = (FactSize / 1024.00).ToString("F2") + " K";
            else if (FactSize >= 1048576 && FactSize < 1073741824)
                m_strSize = (FactSize / 1024.00 / 1024.00).ToString("F2") + " M";
            else if (FactSize >= 1073741824)
                m_strSize = (FactSize / 1024.00 / 1024.00 / 1024.00).ToString("F2") + " G";
            return m_strSize;
        }
    }
}
