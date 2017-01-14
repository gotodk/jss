using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using 公用通讯协议类库.C2S类库;
using 公用通讯协议类库.S2C类库;
using 公用通讯协议类库.P2P类库;
using 公用通讯协议类库.共用类库;
using 公用通讯协议类库.P2P类库.文件传输消息类;
using 灵魂裸奔服务器端.服务器类库;

namespace 灵魂裸奔服务器端.服务器类库
{
    /// <summary>
    /// 发送消息调度中心
    /// </summary>
    public class SendDispatchCenter
    {
        /// <summary>
        /// 监听数据包线程主类
        /// </summary>
        /// 
        private Server ServerThread;

        /// <summary>
        /// UDP消息拓展通讯类
        /// </summary>
        private UDPCommunicate UDPC;


        /// <summary>
        /// 发送消息调度中心构造函数
        /// </summary>
        public SendDispatchCenter(Server SS)
        {
            ServerThread = SS;
            UDPC = new UDPCommunicate(ServerThread);
        }

        /// <summary>
        /// 发送调度中心总调度室
        /// </summary>
        /// <param name="cmdstr"></param>
        /// <param name="InPutHT"></param>
        public void SendDispatchCenter_Dispatch(string cmdstr, Hashtable InPutHT)
        {
            switch (cmdstr)
            {

                case "Send_ACKLoginMessage": //发送登陆消息应答消息
                    Send_ACKLoginMessage(InPutHT);
                    break;
                case "Send_GetUsersResponseMessage": //发送获取用户列表消息应答消息
                    Send_GetUsersResponseMessage(InPutHT);
                    break;
                case "Send_WorkMessage": //主动发送的消息
                    Send_WorkMessage(InPutHT);
                    break;
                default:
                    break;

            }

        }

        /// <summary>
        /// 发送登陆消息应答消息
        /// </summary>
        /// <param name="InPutHT"></param>
        private void Send_ACKLoginMessage(Hashtable InPutHT)
        {
            // 发送登陆应答消息
            ACKLoginMessage ACKlogmsg = new ACKLoginMessage();
            ACKlogmsg.Reloginmessage = "登陆成功";
            ACKlogmsg.Usernameform = "GALAXY###SERVER";
            ACKlogmsg.Usernameto = ((LoginMessage)InPutHT["登陆消息类"]).Usernameform;
            byte[] buffer = FormatterHelper.Serialize(ACKlogmsg);
            //ServerThread.server.Send(buffer, buffer.Length, ServerThread.remotePoint);
            UDPC.UDP_Send(buffer, ServerThread.remotePoint, ACKlogmsg.GetType().ToString());
        }

        private void Send_GetUsersResponseMessage(Hashtable InPutHT)
        {
            GetUsersResponseMessage srvResMsg = new GetUsersResponseMessage();
            srvResMsg.Userlist = ServerThread.userList;
            srvResMsg.Usernameform = "GALAXY###SERVER";
            srvResMsg.Usernameto = ((GetUsersMessage)InPutHT["获取用户列表消息类"]).Usernameform;
            byte[] buffer = FormatterHelper.Serialize(srvResMsg);
            //ServerThread.server.Send(buffer, buffer.Length, ServerThread.remotePoint);
            UDPC.UDP_Send(buffer, ServerThread.remotePoint, srvResMsg.GetType().ToString());
        }

        private void Send_WorkMessage(Hashtable InPutHT)
        {
            User toUser = (User)InPutHT["接收人"];
            string message = (string)InPutHT["消息内容"];
            //直接发送消息到客户端
            WorkMessage workMsg = new WorkMessage();
            workMsg.Usernameform = "GALAXY###SERVER";
            workMsg.Usernameto = toUser.UserName;
            workMsg.Textmessage = message;
            byte[] buffer = FormatterHelper.Serialize(workMsg);
            UDPC.UDP_Send(buffer, toUser.NetPoint_own, workMsg.GetType().ToString());
        }
    }
}
