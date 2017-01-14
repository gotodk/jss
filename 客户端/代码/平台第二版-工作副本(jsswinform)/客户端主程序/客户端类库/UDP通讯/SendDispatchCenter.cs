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
using 公用通讯协议类库.P2P类库;
using 公用通讯协议类库.共用类库;
using 客户端主程序通讯类库.客户端类库.UDP通讯;
using 公用通讯协议类库.P2P类库.文件传输消息类;
using 客户端主程序通讯类库.客户端类库.文件传输;

namespace 客户端主程序通讯类库.客户端类库.UDP通讯
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
        private Client ClientThread;

        /// <summary>
        /// UDP消息拓展通讯类
        /// </summary>
        private UDPCommunicate UDPC;


        /// <summary>
        /// 发送消息调度中心构造函数
        /// </summary>
        public SendDispatchCenter(Client CC)
        {
            ClientThread = CC;
            UDPC = new UDPCommunicate(ClientThread);
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

                case "Send_LoginMessage": //发送登陆消息
                    Send_LoginMessage();
                    break;
                case "Send_GetUserList":  //向服务器发送获取在线用户列表消息
                    Send_GetUserList();
                    break;
                case "Send_TrashMessage":  //向所有在线用户尝试打洞
                    Send_TrashMessage();
                    break;
                case "Send_ACKTrashMessage":  //发送打洞回应消息
                    Send_ACKTrashMessage(InPutHT);
                    break;
                case "Send_LogoutMessage":  //发送退出登录消息
                    Send_LogoutMessage();
                    break;
                case "Send_WorkMessage":  //发送普通文本消息
                    Send_WorkMessage(InPutHT);
                    break;
                case "Send_ACKMessage":  //发送普通文本应答消息
                    Send_ACKMessage(InPutHT);
                    break;
                case "Send_OpenTscMessage":  //发送打开远程桌面命令
                    Send_OpenTscMessage(InPutHT);
                    break;
                case "Send_ACKOpenTscMessage":  //发送打开远程桌面应答消息
                    Send_ACKOpenTscMessage(InPutHT);
                    break;
                case "Send_GetDiskInfoMessage":  //发送获取磁盘分区 消息类
                    Send_GetDiskInfoMessage(InPutHT);
                    break;
                case "Send_ACKGetDiskInfoMessage":  //发送获取磁盘分区 应答消息类
                    Send_ACKGetDiskInfoMessage(InPutHT);
                    break;
                case "Send_GetFilesInfoMessage":  //发送获取文件列表消息类
                    Send_GetFilesInfoMessage(InPutHT);
                    break;
                case "Send_ACKGetFilesInfoMessage":  //发送获取文件列表 应答消息类 
                    Send_ACKGetFilesInfoMessage(InPutHT);
                    break;
                default:
                    break;

            }

        }

        #region 登陆注销和普通文本

        /// <summary>
        /// 发送登陆消息
        /// </summary>
        private void Send_LoginMessage()
        {
            //发送 获取在线用户列表 消息
            LoginMessage logmsg = new LoginMessage();
            logmsg.Usernameform = ClientThread.my_SRV_Username;
            logmsg.Usernameto = "GALAXY###SERVER";
            logmsg.Password = ClientThread.my_SRV_Password;
            logmsg.Pciplist = ClientThread.my_Pciplist;
            byte[] buffer1 = FormatterHelper.Serialize(logmsg);
            //ClientThread.client.Send(buffer1, buffer1.Length, ClientThread.hostPoint);
            UDPC.UDP_Send(buffer1, ClientThread.hostPoint, logmsg.GetType().ToString(), logmsg.Usernameto,null);
            //回调显示
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["事件消息"] = "发送一个登陆消息,目标:服务器:" + ClientThread.hostPoint.Address + ":" + ClientThread.hostPoint.Port;
            OutPutHT["调试"] = "";
            ClientThread.DForThread(OutPutHT);

        }


        /// <summary>
        /// 向服务器发送获取在线用户列表消息
        /// </summary>
        private void Send_GetUserList()
        {
                //发送 获取在线用户列表 消息
                GetUsersMessage getUserMsg = new GetUsersMessage();
                getUserMsg.Usernameform = ClientThread.my_SRV_Username;
                getUserMsg.Usernameto = "GALAXY###SERVER";
                byte[] buffer1 = FormatterHelper.Serialize(getUserMsg);
                //ClientThread.client.Send(buffer1, buffer1.Length, ClientThread.hostPoint);
                UDPC.UDP_Send(buffer1, ClientThread.hostPoint, getUserMsg.GetType().ToString(), getUserMsg.Usernameto,null);
                //回调显示
                Hashtable OutPutHT = new Hashtable();
                OutPutHT["事件消息"] = "获取在线列表,目标:服务器:" + ClientThread.hostPoint.Address + ":" + ClientThread.hostPoint.Port;
                OutPutHT["调试"] = "";
                ClientThread.DForThread(OutPutHT);

        }

        /// <summary>
        /// 向所有在线用户尝试打洞
        /// </summary>
        private void Send_TrashMessage()
        {
            UserCollection UCtemp = ClientThread.userList;
            for (int pp = 0; pp < UCtemp.Count;pp++ )
            {
                User user = (User)UCtemp[pp];
                //向广域网终结点尝试打洞

                TrashMessage TrashMsg = new TrashMessage();
                TrashMsg.Usernameform = ClientThread.my_SRV_Username;
                TrashMsg.Usernameto = user.UserName;
                TrashMsg.Linktype = "广域";
                byte[] buffer1 = FormatterHelper.Serialize(TrashMsg);
                //ClientThread.client.Send(buffer1, buffer1.Length, user.NetPoint_G);
                UDPC.UDP_Send(buffer1, user.NetPoint_G, TrashMsg.GetType().ToString(), TrashMsg.Usernameto,null);
                //Thread.Sleep(1);
                //向局域网终结点列表尝试打洞
                for (int p = user.NetPoint_J.Count - 1; p >= 0; p--)
                {

                    TrashMessage TrashMsg2 = new TrashMessage();
                    TrashMsg2.Usernameform = ClientThread.my_SRV_Username;
                    TrashMsg2.Usernameto = user.UserName;
                    TrashMsg2.Linktype = "局域";
                    byte[] buffer2 = FormatterHelper.Serialize(TrashMsg2);
                    IPEndPoint IPEndPoint_temp = (IPEndPoint)(user.NetPoint_J[p.ToString()]);
                    if (ClientThread.IPCheck(IPEndPoint_temp.Address.ToString()))
                    {
                        //ClientThread.client.Send(buffer2, buffer2.Length, IPEndPoint_temp);
                        UDPC.UDP_Send(buffer2, IPEndPoint_temp, TrashMsg2.GetType().ToString(), TrashMsg2.Usernameto, null);
                        Thread.Sleep(1);
                    }
                }
                Thread.Sleep(1);
            }

            //回调显示
            //ClientThread.OutPutHT.Clear();
            //ClientThread.OutPutHT["事件消息"] = "尝试打洞,目标:所有用户";
            //ClientThread.OutPutHT["调试"] = "";
            //ClientThread.DForThread(ClientThread.OutPutHT);
        }

        /// <summary>
        /// 发送打洞回应消息
        /// </summary>
        private void Send_ACKTrashMessage(Hashtable InPutHT)
        {
            TrashMessage trashmes = (TrashMessage)InPutHT["打洞消息类"];

            ACKTrashMessage ACKtramsg = new ACKTrashMessage();
            ACKtramsg.Linktype = trashmes.Linktype;
            ACKtramsg.Usernameform = ClientThread.my_SRV_Username;
            ACKtramsg.Usernameto = trashmes.Usernameform;
            byte[] buffer = FormatterHelper.Serialize(ACKtramsg);
            //ClientThread.client.Send(buffer, buffer.Length, ClientThread.remotePoint);
            UDPC.UDP_Send(buffer, ClientThread.remotePoint, ACKtramsg.GetType().ToString(), ACKtramsg.Usernameto, null);
            //回调显示
            //ClientThread.OutPutHT.Clear();
            //ClientThread.OutPutHT["事件消息"] = "发送打洞回应,目标:" + trashmes.Usernameform + ":" + ClientThread.remotePoint.Address + ":" + ClientThread.remotePoint.Port;
            //ClientThread.OutPutHT["调试"] = "";
            //ClientThread.DForThread(ClientThread.OutPutHT);
        }

        /// <summary>
        /// 发送退出登录消息
        /// </summary>
        private void Send_LogoutMessage()
        {
            LogoutMessage lgoutMsg = new LogoutMessage();
            lgoutMsg.Usernameform = ClientThread.my_SRV_Username;
            lgoutMsg.Usernameto = "GALAXY###SERVER";
            byte[] buffer = FormatterHelper.Serialize(lgoutMsg);
            //ClientThread.client.Send(buffer, buffer.Length, ClientThread.hostPoint);
            UDPC.UDP_Send(buffer, ClientThread.hostPoint, lgoutMsg.GetType().ToString(), lgoutMsg.Usernameto, null);
            //清理线程和监听实例以及其他可能的东西
            //Thread.Sleep(20000);
            //ClientThread.Dispose();
            //System.Environment.Exit(0);
        }

        /// <summary>
        /// 发送普通文本消息
        /// </summary>
        /// <param name="InPutHT"></param>
        private void Send_WorkMessage(Hashtable InPutHT)
        {
            if (InPutHT["目标帐号"].ToString() != "" && InPutHT["消息内容"].ToString() != "")
            {
                string toUserName = InPutHT["目标帐号"].ToString();
                string message = InPutHT["消息内容"].ToString();

                //判断发送对象是否存在
                User toUser = ClientThread.UCown.Find(toUserName);
                if (toUser == null || toUser.Online_own == "")//用户不存在或者不在线
                {
                    Hashtable OutPutHT = new Hashtable();
                    OutPutHT["事件消息"] = "用户" + toUserName + "不在线";
                    OutPutHT["调试"] = "";
                    //调用委托，回显
                    ClientThread.DForThread(OutPutHT);
                    return;
                }

                //直接发送消息到客户端
                WorkMessage workMsg = new WorkMessage();
                workMsg.Usernameform = ClientThread.my_SRV_Username;
                workMsg.Usernameto = toUser.UserName;
                workMsg.Textmessage = message;
                byte[] buffer = FormatterHelper.Serialize(workMsg);
                
                //ClientThread.client.Send(buffer, buffer.Length, toUser.NetPoint_own);
                UDPC.UDP_Send(buffer, toUser.NetPoint_own, workMsg.GetType().ToString(), workMsg.Usernameto, null);
                Hashtable OutPutHT1 = new Hashtable();
                OutPutHT1["事件消息"] = "发送一个通用消息,目标:" + toUserName + ":" + toUser.NetPoint_own.Address + ":" + toUser.NetPoint_own.Port;
                OutPutHT1["文字消息"] = "发往:" + toUserName + " 消息:" + message;
                OutPutHT1["文字消息标识帐号"] = toUserName;
                OutPutHT1["调试"] = "";
                //调用委托，回显
                ClientThread.DForThread(OutPutHT1);

            }
        }

        /// <summary>
        /// 发送普通文本应答消息
        /// </summary>
        /// <param name="InPutHT"></param>
        private void Send_ACKMessage(Hashtable InPutHT)
        {
            WorkMessage workMsg = (WorkMessage)InPutHT["普通文本消息类"];
            //将消息传入消息处理类，并获得处理结果
            string runreturn_str = "无额外事件发生";

            //如果是服务器发来的消息,进行特殊处理
            if (workMsg.Usernameform == "GALAXY###SERVER")
            {
                // 发送应答消息
                ACKMessage ackMsg = new ACKMessage();
                ackMsg.Usernameform = ClientThread.my_SRV_Username;
                ackMsg.Usernameto = workMsg.Usernameform;
                ackMsg.Remessage = runreturn_str;
                byte[] buffer = FormatterHelper.Serialize(ackMsg);
                //ClientThread.client.Send(buffer, buffer.Length, ClientThread.remotePoint);
                UDPC.UDP_Send(buffer, ClientThread.hostPoint, ackMsg.GetType().ToString(), ackMsg.Usernameto, null);
            }
            else
            {
                //判断发送对象是否存在
                User toUser = ClientThread.UCown.Find(workMsg.Usernameform);
                if (toUser == null || toUser.Online_own == "")//用户不存在或者不在线
                {
                    Hashtable OutPutHT = new Hashtable();
                    OutPutHT["事件消息"] = "用户" + workMsg.Usernameform + "不在线";
                    OutPutHT["调试"] = "";
                    //调用委托，回显
                    ClientThread.DForThread(OutPutHT);
                    return;
                }
                // 发送应答消息
                ACKMessage ackMsg = new ACKMessage();
                ackMsg.Usernameform = ClientThread.my_SRV_Username;
                ackMsg.Usernameto = workMsg.Usernameform;
                ackMsg.Remessage = runreturn_str;
                byte[] buffer = FormatterHelper.Serialize(ackMsg);
                //ClientThread.client.Send(buffer, buffer.Length, ClientThread.remotePoint);
                UDPC.UDP_Send(buffer, toUser.NetPoint_own, ackMsg.GetType().ToString(), ackMsg.Usernameto, null);
            }

            

        }

        #endregion

        //========================================================================================================================================
        #region 远程桌面
        /// <summary>
        /// 发送打开远程桌面命令
        /// </summary>
        /// <param name="InPutHT"></param>
        private void Send_OpenTscMessage(Hashtable InPutHT)
        {
            OpenTscMessage opentsmmessage = new OpenTscMessage();
            opentsmmessage.Usernameform = ClientThread.my_SRV_Username;
            opentsmmessage.Usernameto = InPutHT["目标帐号"].ToString();
            opentsmmessage.WinUser = InPutHT["管理员账号"].ToString();
            opentsmmessage.WinPass = InPutHT["管理员密码"].ToString();
            User toUser = ClientThread.UCown.Find(opentsmmessage.Usernameto);
            if (toUser == null || toUser.Online_own == "")//用户不存在
            {
                return;
            }
            byte[] buffer = FormatterHelper.Serialize(opentsmmessage);
            //ClientThread.client.Send(buffer, buffer.Length, toUser.NetPoint_own);
            UDPC.UDP_Send(buffer, toUser.NetPoint_own, opentsmmessage.GetType().ToString(), opentsmmessage.Usernameto, null);
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["事件消息"] = "请求打开远程桌面,目标:" + opentsmmessage.Usernameto + ":" + toUser.NetPoint_own.Address + ":" + toUser.NetPoint_own.Port;
            OutPutHT["调试"] = "";
            //调用委托，回显
            ClientThread.DForThread(OutPutHT);
        }

        /// <summary>
        /// 发送打开远程桌面应答消息
        /// </summary>
        /// <param name="InPutHT"></param>
        private void Send_ACKOpenTscMessage(Hashtable InPutHT)
        {

            OpenTscMessage opentsmmessage = (OpenTscMessage)InPutHT["打开远程桌面消息类"];

            ACKOpenTscMessage ackMsg = new ACKOpenTscMessage();
            ackMsg.Usernameform = ClientThread.my_SRV_Username;
            ackMsg.Usernameto = opentsmmessage.Usernameform;
            ackMsg.Remessage = "打开远程桌面完成";
            byte[] buffer = FormatterHelper.Serialize(ackMsg);
            //ClientThread.client.Send(buffer, buffer.Length, ClientThread.remotePoint);
            UDPC.UDP_Send(buffer, ClientThread.remotePoint, ackMsg.GetType().ToString(), ackMsg.Usernameto, null);
        }
        #endregion
        //=========================================================================================================================================


        #region 文件管理



        /// <summary>
        /// 发送获取磁盘分区 消息类
        /// </summary>
        /// <param name="InPutHT"></param>
        private void Send_GetDiskInfoMessage(Hashtable InPutHT)
        {
            GetDiskInfoMessage getdisk = new GetDiskInfoMessage();
            getdisk.Usernameform = ClientThread.my_SRV_Username;
            getdisk.Usernameto = InPutHT["目标帐号"].ToString();

            User toUser = ClientThread.UCown.Find(getdisk.Usernameto);
            if (toUser == null || toUser.Online_own == "")//用户不存在
            {
                return;
            }
            byte[] buffer = FormatterHelper.Serialize(getdisk);
            //ClientThread.client.Send(buffer, buffer.Length, toUser.NetPoint_own);
            UDPC.UDP_Send(buffer, toUser.NetPoint_own, getdisk.GetType().ToString(), getdisk.Usernameto, null);
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["事件消息"] = "请求获取磁盘列表,目标:" + getdisk.Usernameto + ":" + toUser.NetPoint_own.Address + ":" + toUser.NetPoint_own.Port;
            OutPutHT["调试"] = "";
            //调用委托，回显
            ClientThread.DForThread(OutPutHT);
        }

        /// <summary>
        /// 发送 获取磁盘分区 应答消息类
        /// </summary>
        /// <param name="InPutHT"></param>
        private void Send_ACKGetDiskInfoMessage(Hashtable InPutHT)
        {
            GetDiskInfoMessage getdisk = (GetDiskInfoMessage)InPutHT["来源消息类"];

            FilesInfoCollection FilesInfoAll_YC;
            FileOperateClassHelp FOCH = new FileOperateClassHelp();
            FilesInfoAll_YC = FOCH.GetPathAllInfo_Disk();
            if(FilesInfoAll_YC == null)
            {
                return;
            }

            ACKGetDiskInfoMessage ackMsg = new ACKGetDiskInfoMessage();
            ackMsg.Usernameform = ClientThread.my_SRV_Username;
            ackMsg.Usernameto = getdisk.Usernameform;
            ackMsg.FilesInfoList = FilesInfoAll_YC;

            User toUser = ClientThread.UCown.Find(getdisk.Usernameform);
            if (toUser == null || toUser.Online_own == "")//用户不存在
            {
                return;
            }
            byte[] buffer = FormatterHelper.Serialize(ackMsg);
            //ClientThread.client.Send(buffer, buffer.Length, ClientThread.remotePoint);
            UDPC.UDP_Send(buffer, toUser.NetPoint_own, ackMsg.GetType().ToString(), ackMsg.Usernameto, null);

            Hashtable OutPutHT = new Hashtable();
            OutPutHT["事件消息"] = "发送磁盘列表,目标:" + getdisk.Usernameform + ":" + toUser.NetPoint_own.Address + ":" + toUser.NetPoint_own.Port;
            OutPutHT["调试"] = "";
            //调用委托，回显
            ClientThread.DForThread(OutPutHT);
        }



        /// <summary>
        /// 发送获取文件列表 消息类
        /// </summary>
        /// <param name="InPutHT"></param>
        private void Send_GetFilesInfoMessage(Hashtable InPutHT)
        {
            GetFilesInfoMessage getfilesinfo = new GetFilesInfoMessage();
            getfilesinfo.Usernameform = ClientThread.my_SRV_Username;
            getfilesinfo.Usernameto = InPutHT["目标帐号"].ToString();
            getfilesinfo.OrderNextPath = InPutHT["目标路径"].ToString();

            User toUser = ClientThread.UCown.Find(getfilesinfo.Usernameto);
            if (toUser == null || toUser.Online_own == "")//用户不存在
            {
                return;
            }
            byte[] buffer = FormatterHelper.Serialize(getfilesinfo);
            //ClientThread.client.Send(buffer, buffer.Length, toUser.NetPoint_own);
            UDPC.UDP_Send(buffer, toUser.NetPoint_own, getfilesinfo.GetType().ToString(), getfilesinfo.Usernameto, null);
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["事件消息"] = "请求目录列表,目标:" + getfilesinfo.Usernameto + ":" + toUser.NetPoint_own.Address + ":" + toUser.NetPoint_own.Port;
            OutPutHT["调试"] = "";
            //调用委托，回显
            ClientThread.DForThread(OutPutHT);
        }


        /// <summary>
        /// 发送 获取目录列表 应答消息类
        /// </summary>
        /// <param name="InPutHT"></param>
        private void Send_ACKGetFilesInfoMessage(Hashtable InPutHT)
        {
            GetFilesInfoMessage getinfo= (GetFilesInfoMessage)InPutHT["来源消息类"];

            FilesInfoCollection FilesInfoAll_YC;
            FileOperateClassHelp FOCH = new FileOperateClassHelp();
            FilesInfoAll_YC = FOCH.GetPathAllInfo_Path(getinfo.OrderNextPath);
            if (FilesInfoAll_YC == null)
            {
                return;
            }
            ACKGetFilesInfoMessage ackMsg = new ACKGetFilesInfoMessage();
            ackMsg.Usernameform = ClientThread.my_SRV_Username;
            ackMsg.Usernameto = getinfo.Usernameform;
            ackMsg.FilesInfoList = FilesInfoAll_YC;

            User toUser = ClientThread.UCown.Find(getinfo.Usernameform);
            if (toUser == null || toUser.Online_own == "")//用户不存在
            {
                return;
            }
            byte[] buffer = FormatterHelper.Serialize(ackMsg);
            //ClientThread.client.Send(buffer, buffer.Length, ClientThread.remotePoint);
            UDPC.UDP_Send(buffer, toUser.NetPoint_own, ackMsg.GetType().ToString(), ackMsg.Usernameto, null);
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["事件消息"] = "发送目录列表,目标:" + getinfo.Usernameform + ":" + toUser.NetPoint_own.Address + ":" + toUser.NetPoint_own.Port;
            OutPutHT["调试"] = "";
            //调用委托，回显
            ClientThread.DForThread(OutPutHT);
        }

        /// <summary>
        /// 发送 要求上传文件 消息类
        /// </summary>
        /// <param name="InPutHT"></param>
        private void Send_ApplyUpLoadMessage(Hashtable InPutHT)
        {
            ApplyUpLoadMessage UpLoad = new ApplyUpLoadMessage();
            UpLoad.Usernameform = ClientThread.my_SRV_Username;
            UpLoad.Usernameto = InPutHT["目标帐号"].ToString();
            UpLoad.SaveFilePath = InPutHT["下载目录"].ToString(); ;
            UpLoad.OrderFilePath = InPutHT["要下载的文件"].ToString(); ;
            string onlyone_f = InPutHT["目标帐号"].ToString() + "|" + InPutHT["要下载的文件"].ToString() + "|" + InPutHT["下载目录"].ToString();
            User toUser = ClientThread.UCown.Find(UpLoad.Usernameto);
            if (toUser == null || toUser.Online_own == "")//用户不存在
            {
                return;
            }
            byte[] buffer = FormatterHelper.Serialize(UpLoad);
            //ClientThread.client.Send(buffer, buffer.Length, toUser.NetPoint_own);
            UDPC.UDP_Send(buffer, toUser.NetPoint_own, UpLoad.GetType().ToString(), UpLoad.Usernameto, null);
            //ClientThread.OutPutHT.Clear();
            //ClientThread.OutPutHT["事件消息"] = "请求下载文件,目标:" + UpLoad.Usernameto + ":" + toUser.NetPoint_own.Address + ":" + toUser.NetPoint_own.Port;
            //ClientThread.OutPutHT["调试"] = "";
            //调用委托，回显
            //ClientThread.DForThread(ClientThread.OutPutHT);
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["开始下载"] = onlyone_f;
            OutPutHT["下载帐号标示"] = InPutHT["目标帐号"].ToString();
            OutPutHT["调试"] = "";
            //调用委托，回显
            ClientThread.DForThread(OutPutHT);
        }


        #endregion
    }
}
