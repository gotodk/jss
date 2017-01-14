using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using 公用通讯协议类库.P2P类库;
using 公用通讯协议类库.P2P类库.文件传输消息类;
using 公用通讯协议类库.S2C类库;
using 公用通讯协议类库.C2S类库;
using 公用通讯协议类库.共用类库;
using 公用通讯协议类库.消息片类库;
using 灵魂裸奔服务器端.服务器类库;

namespace 灵魂裸奔服务器端.服务器类库
{
    /// <summary>
    /// 接收消息调度中心
    /// </summary>
    public class ReceiveDispatchCenter
    {
        /// <summary>
        /// 监听数据包线程主类
        /// </summary>
        private Server ServerThread;

        /// <summary>
        /// 多线程同步锁
        /// </summary>
        private Mutex mut = new Mutex();



        /// <summary>
        /// 接收消息调度中心构造函数
        /// </summary>
        public ReceiveDispatchCenter(Server SS)
        {
            ServerThread = SS;
        }

        /// <summary>
        /// 接收调度中心总调度室
        /// </summary>
        /// <param name="msgType"></param>
        /// <param name="msgObj"></param>
        /// <param name="InPutHT"></param>
        public void ReceiveDispatchCenter_Dispatch(Type msgType, object msgObj, Hashtable InPutHT)
        {
            string msgType_str = "";
            if (msgType != null)
            {
                msgType_str = msgType.ToString();
            }

            switch (msgType_str)
            {
                case "公用通讯协议类库.消息片类库.MsgSlice":  //收到消息片
                    Receive_MsgSlice(msgObj);
                    break;
                case "公用通讯协议类库.消息片类库.FileRevieveATC":  //收到消息片接收应答消息
                    FileRevieveATC(msgObj);
                    break;


                //========================================================================以上为滑动窗口消息特殊接收调度




                case "公用通讯协议类库.C2S类库.LoginMessage":  //收到客户端登陆消息
                    Receive_LoginMessage(msgObj);
                    break;
                case "公用通讯协议类库.C2S类库.LogoutMessage":  //收到客户端登出消息
                    Receive_LogoutMessage(msgObj);
                    break;
                case "公用通讯协议类库.C2S类库.GetUsersMessage":  //收到客户端请求获取用户列表的消息
                    Receive_GetUsersMessage(msgObj);
                    break;
                case "公用通讯协议类库.P2P类库.ACKMessage":  //收到客户端的普通文本应答消息
                    Receive_ACKMessage(msgObj);
                    break;
                default:
                    break;

            }

        }


        #region 滑动窗口数据包接收

        /// <summary>
        /// 收到消息片
        /// </summary>
        /// <param name="msgObj"></param>
        private void Receive_MsgSlice(object msgObj)
        {
            //根据唯一识别标志，判定是否接收实例是否存在。如果存在，使用存在的实例处理，不存在，新建实例处理
            MsgSlice ms = (MsgSlice)msgObj;
            if (!ServerThread.receive_usw.ht_receiveList.ContainsKey(ms.onlyonefloat))
            {
                ServerThread.receive_usw.ht_receiveList[ms.onlyonefloat] = new Hashtable();
            }


            ServerThread.receive_usw.pack_input = msgObj;
            ServerThread.receive_usw.Revieve_file();


        }


        /// <summary>
        /// 收到消息片应答消息(发送端调用)
        /// </summary>
        /// <param name="msgObj"></param>
        private void FileRevieveATC(object msgObj)
        {
            FileRevieveATC frACT = (FileRevieveATC)msgObj;
            UDPSlidingWindow usw_fr_ATC;
            usw_fr_ATC = (UDPSlidingWindow)(ServerThread.ht_UDPsw[frACT.OnlyOneFloat]);
            usw_fr_ATC.recieveThreadACT(frACT.Byte_ACT, frACT.OnlyOneFloat);
        }

        #endregion




        /// <summary>
        /// 收到客户端登陆消息
        /// </summary>
        /// <param name="msgObj"></param>
        private void Receive_LoginMessage(object msgObj)
        {
            // 转换接受的消息
            LoginMessage lginMsg = (LoginMessage)msgObj;
            if (lginMsg.Usernameto == "GALAXY###SERVER")//验证是否正常消息
            {


                // 添加用户到列表
                IPEndPoint userEndPoint = new IPEndPoint(ServerThread.remotePoint.Address, ServerThread.remotePoint.Port);
                User user = new User();
                user.UserName = lginMsg.Usernameform;
                user.NetPoint_G = userEndPoint;
                user.NetPoint_J = lginMsg.Pciplist;
                user.NetPoint_own = ServerThread.remotePoint;
                mut.WaitOne();
                User onlineUser = ServerThread.userList.Find(user.UserName);
                if (onlineUser != null)//相同GUID用户已登录在线
                {
                    ServerThread.userList.Remove(onlineUser);
                }

                ServerThread.userList.Add(user);
                mut.ReleaseMutex();
                //更新在线列表的显示
                Hashtable OutPutHT = new Hashtable();
                OutPutHT["事件消息"] = "有帐号登陆:  " + lginMsg.Usernameform + "";
                OutPutHT["在线列表"] = ServerThread.userList;
                OutPutHT["调试"] = "";
                //调用委托，回显
                ServerThread.DForThread(OutPutHT);

                //发送应答
                Hashtable httemp = new Hashtable();
                httemp["登陆消息类"] = lginMsg;
                ServerThread.SDC.SendDispatchCenter_Dispatch("Send_ACKLoginMessage", httemp);


                //向所有在线用户发送新用户登录消息
            }
        }

         /// <summary>
        /// 收到客户端登出消息
        /// </summary>
        /// <param name="msgObj"></param>
        private void Receive_LogoutMessage(object msgObj)
        {
            // 转换接受的消息
            LogoutMessage lgoutMsg = (LogoutMessage)msgObj;
            if (lgoutMsg.Usernameto == "GALAXY###SERVER")//验证是否正常消息
            {
                // 从列表中删除用户
                User lgoutUser = ServerThread.userList.Find(lgoutMsg.Usernameform);
                if (lgoutUser != null)
                {
                    ServerThread.userList.Remove(lgoutUser);
                }
                Hashtable OutPutHT = new Hashtable();
                OutPutHT["事件消息"] = "有帐号注销:  " + lgoutMsg.Usernameform + "";
                //更新在线列表的显示
                OutPutHT["在线列表"] = ServerThread.userList;
                OutPutHT["调试"] = "";
                //调用委托，回显
                ServerThread.DForThread(OutPutHT);
            }
        }

        /// <summary>
        /// 收到客户端请求获取用户列表的消息
        /// </summary>
        /// <param name="msgObj"></param>
        private void Receive_GetUsersMessage(object msgObj)
        {
            GetUsersMessage getusermsg = (GetUsersMessage)msgObj;
            if (getusermsg.Usernameto == "GALAXY###SERVER")//验证是否正常消息
            {
                //发送应答
                Hashtable httemp = new Hashtable();
                httemp["获取用户列表消息类"] = getusermsg;
                ServerThread.SDC.SendDispatchCenter_Dispatch("Send_GetUsersResponseMessage", httemp);

                Hashtable OutPutHT = new Hashtable();
                OutPutHT["事件消息"] = "客户端请求更新列表,请求帐号: " + getusermsg.Usernameform + "  已发送到:" + ServerThread.remotePoint.Address.ToString() + ":" + ServerThread.remotePoint.Port.ToString();
                OutPutHT["调试"] = "";
                //调用委托，回显
                ServerThread.DForThread(OutPutHT);
            }
        }


        /// <summary>
        /// 收到普通文本应答消息
        /// </summary>
        /// <param name="msgObj"></param>
        private void Receive_ACKMessage(object msgObj)
        {
            ACKMessage RunBack = (ACKMessage)msgObj;
            if (RunBack.Usernameto == "GALAXY###SERVER")//验证是否正常消息
            {
                Hashtable OutPutHT = new Hashtable();
                OutPutHT["事件消息"] = "接收到" + RunBack.Usernameform + "的文本消息应答,执行结果: " + RunBack.Remessage;
                OutPutHT["调试"] = "";
                //调用委托，回显
                ServerThread.DForThread(OutPutHT);
            }
        }
    }
}
