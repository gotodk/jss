using System;
using System.Collections;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Text;
using 公用通讯协议类库.共用类库;
using 公用通讯协议类库.消息片类库;
using 灵魂裸奔服务器端.服务器类库;

namespace 灵魂裸奔服务器端.服务器类库
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
        /// 要发送的消息的类型
        /// </summary>
        public string sendMSGTYPE;

        /// <summary>
        /// 监听数据包线程主类
        /// </summary>
        public Server ServerThread;

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
        public UDPSlidingWindow(Server SS)
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
            ServerThread = SS;

        }
        public UDPSlidingWindow(Server SS, string sendGUID_temp, string sendMSGTYPE_temp)
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
            ServerThread = SS;
            sendGUID = sendGUID_temp;
            sendMSGTYPE = sendMSGTYPE_temp;
        }
        public UDPSlidingWindow(Server CC, object pack_temp)
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
            ServerThread = CC;
            pack_input = pack_temp;
        }


        /// <summary>
        /// 接收发送来的数据包
        /// </summary>
        public void Revieve_file()
        {
            MsgSlice ms = (MsgSlice)pack_input;
            //receiveList = (Hashtable)ht_receiveList[ms.onlyonefloat];
            //Console.WriteLine("接收到数据："+ms.getSliceIndex());
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
                ServerThread.server.Send(BtACT, BtACT.Length, ServerThread.remotePoint); //发送消息片接收完成应答消息
                //Console.WriteLine("回应接收到片信息:"+index);

                ////回显
                //ServerThread.OutPutHT.Clear();
                //ServerThread.OutPutHT["数据片完成(接收)"] = index;
                //ServerThread.OutPutHT["总片数"] = ms.sliceAllnum;
                //ServerThread.OutPutHT["唯一标示"] = ms.onlyonefloat;
                //ServerThread.OutPutHT["调试"] = "";
                //ServerThread.DForThread(ServerThread.OutPutHT);

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

                        ServerThread.server.Send(BtACT_end, BtACT_end.Length, ServerThread.remotePoint);

                        Hashtable ht_sendlist = (Hashtable)(ht_receiveList[ms.onlyonefloat]);//接受到的数据

                        //重组数据
                        byte[] msgBt = MsgSlice.uniteMsg(ht_sendlist);//
                        mut.WaitOne();
                        ((Hashtable)(ServerThread.receive_usw.ht_receiveList[ms.onlyonefloat])).Clear();
                        ServerThread.receive_usw.ht_receiveList.Remove(ms.onlyonefloat);
                        mut.ReleaseMutex();
                        //处理接受到的消息片
                        UDPCommunicate UDPC = new UDPCommunicate(ServerThread);
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
            string weiyibiaozhi = sendGUID + "|" + sendMSGTYPE;
            //byte[] sendBuffer = System.Text.ASCIIEncoding.UTF8.GetBytes(msg);
            byte[] sendBuffer = ((UDPSlidingWindow)(ServerThread.ht_UDPsw[weiyibiaozhi])).sendBuffer_file;
            //Console.WriteLine("发送开始时间：" + DateTime.Now);
            //Console.WriteLine("待发数据长度：" + sendBuffer.Length / 1024 + "K Byte】");
            

            ((UDPSlidingWindow)(ServerThread.ht_UDPsw[weiyibiaozhi])).sendList = MsgSlice.splitMsg(sendBuffer); //对消息进行分片
            //Console.WriteLine("准备发送数据：" + sendList.Count + "片");
            show_DForThread("准备发送数据,待发数据长度" + sendBuffer.Length / 1024 + "K Byte" + "\n共分为：" + ((UDPSlidingWindow)(ServerThread.ht_UDPsw[weiyibiaozhi])).sendList.Count + "片");
            while (ServerThread.ht_UDPsw.ContainsKey(weiyibiaozhi) && ((UDPSlidingWindow)(ServerThread.ht_UDPsw[weiyibiaozhi])).ret != UDPSlidingWindow.MSG_COMPLETE)
            {
                bool wbzf = ServerThread.ht_UDPsw.ContainsKey(weiyibiaozhi);
                DateTime dtf = ((UDPSlidingWindow)(ServerThread.ht_UDPsw[weiyibiaozhi])).sendWindow.lastActiveTime.AddMilliseconds(UDPSlidingWindow.MAX_WAIT_TIME);
                if (wbzf && ((DateTime.Now) > dtf))
                {//窗口停留超时，则重发落在窗口内未标志为“已接收”的所有包
                    if (ServerThread.ht_UDPsw.ContainsKey(weiyibiaozhi) && ((UDPSlidingWindow)(ServerThread.ht_UDPsw[weiyibiaozhi])).retryTime >= UDPSlidingWindow.MAX_RETRY_TIME)
                    {//重发次数超过设定值，退出。
                        //Console.WriteLine("重发次数超过" + UDPSlidingWindow.MAX_RETRY_TIME + "，发送数据失败！请检查网络是否正常连接。");
                        show_DForThread("重发次数超过" + UDPSlidingWindow.MAX_RETRY_TIME + "，发送数据失败！");
                        return;
                    }
                    //Console.WriteLine((DateTime.Now).ToString() + "===>" + sendWindow.lastActiveTime.AddMilliseconds(UDPSlidingWindow.MAX_WAIT_TIME));
                    for (long i = ((UDPSlidingWindow)(ServerThread.ht_UDPsw[weiyibiaozhi])).sendWindow.getWindowHead() - ((UDPSlidingWindow)(ServerThread.ht_UDPsw[weiyibiaozhi])).sendWindow.getUsedSize(); i < ((UDPSlidingWindow)(ServerThread.ht_UDPsw[weiyibiaozhi])).sendWindow.getWindowHead(); i++)
                    {
                        MsgSlice tmpMs = (MsgSlice)(((UDPSlidingWindow)(ServerThread.ht_UDPsw[weiyibiaozhi])).sendList[i]);
                        if (tmpMs != null && (((UDPSlidingWindow)(ServerThread.ht_UDPsw[weiyibiaozhi])).state[i] == null || (byte)(((UDPSlidingWindow)(ServerThread.ht_UDPsw[weiyibiaozhi])).state[i]) != UDPSlidingWindow.MSG_SLICE_COMPLETE))
                        {
                            //Console.WriteLine("重新发送数据：" + i);
                            show_DForThread("重新发送数据：" + i);

                            tmpMs.onlyonefloat = weiyibiaozhi; //定义唯一标志,用于多组数据包同时传送
                            byte[] tmpBt = FormatterHelper.Serialize(tmpMs);
                            ServerThread.server.Send(tmpBt, tmpBt.Length, ((UDPSlidingWindow)(ServerThread.ht_UDPsw[weiyibiaozhi])).target);
                            ((UDPSlidingWindow)(ServerThread.ht_UDPsw[weiyibiaozhi])).state.Remove(i);
                            ((UDPSlidingWindow)(ServerThread.ht_UDPsw[weiyibiaozhi])).state.Add(i, UDPSlidingWindow.MSG_SLICE_SENDED);//将窗口内新发送包加入到等待回应队列
                        }
                    }
                    ((UDPSlidingWindow)(ServerThread.ht_UDPsw[weiyibiaozhi])).retryTime++;
                    ((UDPSlidingWindow)(ServerThread.ht_UDPsw[weiyibiaozhi])).sendWindow.lastActiveTime = DateTime.Now;
                }
                while (ServerThread.ht_UDPsw.ContainsKey(weiyibiaozhi) && (((UDPSlidingWindow)(ServerThread.ht_UDPsw[weiyibiaozhi])).state[(long)(((UDPSlidingWindow)(ServerThread.ht_UDPsw[weiyibiaozhi])).sendWindow.getWindowHead() - ((UDPSlidingWindow)(ServerThread.ht_UDPsw[weiyibiaozhi])).sendWindow.getUsedSize())] != null && (byte)(((UDPSlidingWindow)(ServerThread.ht_UDPsw[weiyibiaozhi])).state[(long)(((UDPSlidingWindow)(ServerThread.ht_UDPsw[weiyibiaozhi])).sendWindow.getWindowHead() - ((UDPSlidingWindow)(ServerThread.ht_UDPsw[weiyibiaozhi])).sendWindow.getUsedSize())]) == MSG_SLICE_COMPLETE || ((UDPSlidingWindow)(ServerThread.ht_UDPsw[weiyibiaozhi])).sendWindow.getUsedSize() < ((UDPSlidingWindow)(ServerThread.ht_UDPsw[weiyibiaozhi])).sendWindow.getMaxSize()) && ((UDPSlidingWindow)(ServerThread.ht_UDPsw[weiyibiaozhi])).sendWindow.getWindowHead() < ((UDPSlidingWindow)(ServerThread.ht_UDPsw[weiyibiaozhi])).sendList.Count)
                {//滑动窗口下标数据已经完成且数据未发完，则发送下一个包，并且窗口前移。
                    ((UDPSlidingWindow)(ServerThread.ht_UDPsw[weiyibiaozhi])).retryTime = 0;
                    MsgSlice tmpMs = (MsgSlice)(((UDPSlidingWindow)(ServerThread.ht_UDPsw[weiyibiaozhi])).sendList[(long)(((UDPSlidingWindow)(ServerThread.ht_UDPsw[weiyibiaozhi])).sendWindow.getWindowHead())]);
                    //Console.WriteLine("准备发送："+sendWindow.getWindowHead()+":"+tmpMs.isEndSlice());

                    tmpMs.onlyonefloat = weiyibiaozhi; //定义唯一标志,用于多组数据包同时传送
                    byte[] tmpBt = FormatterHelper.Serialize(tmpMs);
                    ServerThread.server.Send(tmpBt, tmpBt.Length, ((UDPSlidingWindow)(ServerThread.ht_UDPsw[weiyibiaozhi])).target);
                    ((UDPSlidingWindow)(ServerThread.ht_UDPsw[weiyibiaozhi])).sendWindow.moveAhead();
                    ((UDPSlidingWindow)(ServerThread.ht_UDPsw[weiyibiaozhi])).state.Add(((UDPSlidingWindow)(ServerThread.ht_UDPsw[weiyibiaozhi])).sendWindow.getWindowHead(), UDPSlidingWindow.MSG_SLICE_SENDED);//将窗口内新发送包加入到等待回应队列
                    Thread.Sleep(1);
                }
                Thread.Sleep(1);
                
            }
            //发送线程结束，清理实例
            mut.WaitOne();
            ServerThread.ht_UDPsw.Remove(weiyibiaozhi);
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
                ((UDPSlidingWindow)(ServerThread.ht_UDPsw[OnlyOneFloat])).state.Remove(index);
                ((UDPSlidingWindow)(ServerThread.ht_UDPsw[OnlyOneFloat])).state.Add(index, UDPSlidingWindow.MSG_SLICE_COMPLETE);
                

                //ServerThread.OutPutHT.Clear();
                //ServerThread.OutPutHT["数据片完成"] = index.ToString();
                //ServerThread.OutPutHT["数据片总量"] = ((UDPSlidingWindow)(ServerThread.ht_UDPsw[OnlyOneFloat])).sendList.Count.ToString();
                //ServerThread.OutPutHT["唯一标示"] = ((UDPSlidingWindow)(ServerThread.ht_UDPsw[OnlyOneFloat])).sendGUID + "|" + ((UDPSlidingWindow)(ServerThread.ht_UDPsw[OnlyOneFloat])).sendMSGTYPE;
                //ServerThread.OutPutHT["调试"] = "";
                //ServerThread.DForThread(ServerThread.OutPutHT);
            }
            else if (act[0] == UDPSlidingWindow.MSG_COMPLETE)
            {//接收到“数据接收完毕”信息
                ((UDPSlidingWindow)(ServerThread.ht_UDPsw[OnlyOneFloat])).ret = UDPSlidingWindow.MSG_COMPLETE;

                //ServerThread.OutPutHT.Clear();
                //ServerThread.OutPutHT["数据片完成"] = ((UDPSlidingWindow)(ServerThread.ht_UDPsw[OnlyOneFloat])).sendList.Count.ToString();
                //ServerThread.OutPutHT["数据片总量"] = ((UDPSlidingWindow)(ServerThread.ht_UDPsw[OnlyOneFloat])).sendList.Count.ToString();
                //ServerThread.OutPutHT["唯一标示"] = ((UDPSlidingWindow)(ServerThread.ht_UDPsw[OnlyOneFloat])).sendGUID + "|" + ((UDPSlidingWindow)(ServerThread.ht_UDPsw[OnlyOneFloat])).sendMSGTYPE;
                //ServerThread.OutPutHT["调试"] = "";
                //ServerThread.DForThread(ServerThread.OutPutHT);

                //ServerThread.ht_UDPsw.Remove(OnlyOneFloat);
            }
        }


        /// <summary>
        /// 调用委托，显示进度
        /// </summary>
        /// <param name="msginfo"></param>
        private void show_DForThread(string msginfo)
        {
            //ServerThread.OutPutHT.Clear();
            //ServerThread.OutPutHT["事件消息"] = msginfo;
            //ServerThread.OutPutHT["调试"] = "";
            ////调用委托，回显
            //ServerThread.DForThread(ServerThread.OutPutHT);
        }
    }
}
