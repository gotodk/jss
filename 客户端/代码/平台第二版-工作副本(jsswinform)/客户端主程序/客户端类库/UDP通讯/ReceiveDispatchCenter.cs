using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using 公用通讯协议类库.P2P类库;
using 公用通讯协议类库.P2P类库.文件传输消息类;
using 公用通讯协议类库.S2C类库;
using 公用通讯协议类库.共用类库;
using 公用通讯协议类库.消息片类库;
using 客户端主程序通讯类库.客户端类库.UDP通讯;
using 客户端主程序通讯类库.客户端类库.远程控制动作;

namespace 客户端主程序通讯类库.客户端类库.UDP通讯
{
    /// <summary>
    /// 接收消息调度中心
    /// </summary>
    public class ReceiveDispatchCenter
    {
        /// <summary>
        /// 监听数据包线程主类
        /// </summary>
        private Client ClientThread;

        /// <summary>
        /// 多线程同步锁
        /// </summary>
        private Mutex mut = new Mutex();

        /// <summary>
        /// 接收消息调度中心构造函数
        /// </summary>
        public ReceiveDispatchCenter(Client CC)
        {
            ClientThread = CC;
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


                case "公用通讯协议类库.S2C类库.ACKLoginMessage":  //收到登陆应答消息
                    Receive_ACKLoginMessage(msgObj);
                    break;
                case "公用通讯协议类库.S2C类库.GetUsersResponseMessage": //收到用户列表消息
                    Receive_GetUsersResponseMessage(msgObj);
                    break;
                case "公用通讯协议类库.P2P类库.TrashMessage":   //收到请求打洞消息
                    Receive_TrashMessage(msgObj);
                    break;
                case "公用通讯协议类库.P2P类库.ACKTrashMessage":  //收到请求打洞应答消息
                    Receive_ACKTrashMessage(msgObj);
                    break;
                case "公用通讯协议类库.P2P类库.WorkMessage":   //收到普通文本消息
                    Receive_WorkMessage(msgObj);
                    break;
                case "公用通讯协议类库.P2P类库.ACKMessage":  //收到普通文本应答消息
                    Receive_ACKMessage(msgObj);
                    break;
                case "公用通讯协议类库.P2P类库.OpenTscMessage":  //收到请求打开远程桌面消息
                    Receive_OpenTscMessage(msgObj); 
                    break;
                case "公用通讯协议类库.P2P类库.ACKOpenTscMessage":  //收到请求打开远程桌面应答消息
                    Receive_ACKOpenTscMessage(msgObj); 
                    break;
                case "公用通讯协议类库.P2P类库.文件传输消息类.GetDiskInfoMessage":  //收到获取磁盘分区 消息类
                    Receive_GetDiskInfoMessage(msgObj);
                    break;
                case "公用通讯协议类库.P2P类库.文件传输消息类.ACKGetDiskInfoMessage":  //收到获取磁盘分区 应答消息类
                    Receive_ACKGetDiskInfoMessage(msgObj);
                    break;
                case "公用通讯协议类库.P2P类库.文件传输消息类.GetFilesInfoMessage":  // 收到获取文件列表消息类
                    Receive_GetFilesInfoMessage(msgObj);
                    break;
                case "公用通讯协议类库.P2P类库.文件传输消息类.ACKGetFilesInfoMessage":  //收到获取文件列表 应答消息类
                    Receive_ACKGetFilesInfoMessage(msgObj);
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
            if (!ClientThread.receive_usw.ht_receiveList.ContainsKey(ms.onlyonefloat))
            {
                ClientThread.receive_usw.ht_receiveList[ms.onlyonefloat] = new Hashtable();
            }


            ClientThread.receive_usw.pack_input = msgObj;
            ClientThread.receive_usw.Revieve_file();


        }


        /// <summary>
        /// 收到消息片应答消息(发送端调用)
        /// </summary>
        /// <param name="msgObj"></param>
        private void FileRevieveATC(object msgObj)
        {
            FileRevieveATC frACT = (FileRevieveATC)msgObj;
            UDPSlidingWindow usw_fr_ATC;
            usw_fr_ATC = (UDPSlidingWindow)(ClientThread.ht_UDPsw[frACT.OnlyOneFloat]);
            usw_fr_ATC.recieveThreadACT(frACT.Byte_ACT, frACT.OnlyOneFloat);
        }

        #endregion


        #region 登陆注销和普通文本

        /// <summary>
        /// 接受到登陆应答消息
        /// </summary>
        /// <param name="msgObj"></param>
        private void Receive_ACKLoginMessage(object msgObj)
        {
            ACKLoginMessage ACKlogmsg = (ACKLoginMessage)msgObj;
            if (ACKlogmsg.Usernameto == ClientThread.my_SRV_Username)//验证是否正常消息
            {
                if (ACKlogmsg.Reloginmessage == "登陆成功")
                {
                    Hashtable OutPutHT = new Hashtable();
                    OutPutHT["客户端状态"] = "登陆成功";
                    OutPutHT["调试"] = "";
                    //调用委托，回显
                    ClientThread.DForThread(OutPutHT);
                    /*
                    //启动获取列表线程
                    //ClientThread.Send_getUserMsg();
                    Thread SendAllgogo = new Thread(new ThreadStart(ClientThread.Send_getUserMsg));
                    SendAllgogo.Start();
                     */
                }
                else
                {
                    Hashtable OutPutHT = new Hashtable();
                    OutPutHT["客户端状态"] = "登陆失败";
                    OutPutHT["调试"] = "";
                    //调用委托，回显
                    ClientThread.DForThread(OutPutHT);
                    ClientThread.listenThread.Abort();
                    ClientThread.client.Close();
                }

            }


        }


        /// <summary>
        /// 接受到请求用户列表应答消息
        /// </summary>
        /// <param name="msgObj"></param>
        private void Receive_GetUsersResponseMessage(object msgObj)
        {
            // 转换消息
            GetUsersResponseMessage usersMsg = (GetUsersResponseMessage)msgObj;
            if (usersMsg.Usernameto == ClientThread.my_SRV_Username)//验证是否正常消息
            {
                // 更新用户列表
                mut.WaitOne();
                ClientThread.userList.Clear();
                foreach (User user in usersMsg.Userlist)
                {
                    ClientThread.userList.Add(user);
                }
                mut.ReleaseMutex();
                Hashtable OutPutHT = new Hashtable();
                OutPutHT["客户端状态"] = "在线用户已更新,正更新状态...";
                OutPutHT["调试"] = "";
                //调用委托，回显
                ClientThread.DForThread(ClientThread.OutPutHT);

                //启动线程尝试打洞
                if (!ClientThread.ThreadTrans_Begin && ClientThread.userList.Count > 0)
                {
                    Thread SendDD = new Thread(new ThreadStart(ClientThread.Send_TransMsg));
                    SendDD.Start();
                }
            }
        }

        /// <summary>
        /// 收到点对点打洞消息
        /// </summary>
        /// <param name="msgObj"></param>
        private void Receive_TrashMessage(object msgObj)
        {
            TrashMessage trashmes = (TrashMessage)msgObj;
            if (trashmes.Usernameto == ClientThread.my_SRV_Username)//验证是否正常消息
            {
                //更新私有状态
                if (ClientThread.userList.Find(trashmes.Usernameform) != null)
                {
                    mut.WaitOne();
                    ClientThread.userList.Find(trashmes.Usernameform).Online_own = "可连接";
                    ClientThread.userList.Find(trashmes.Usernameform).Type_own = trashmes.Linktype;
                    ClientThread.userList.Find(trashmes.Usernameform).NetPoint_own = ClientThread.remotePoint;
                    mut.ReleaseMutex();
                }

                //回调显示
                //ClientThread.OutPutHT.Clear();
                //ClientThread.OutPutHT["事件消息"] = "接收到打洞消息,来自:" + trashmes.Usernameform + ":" + ClientThread.remotePoint.Address + ":" + ClientThread.remotePoint.Port;
                //ClientThread.OutPutHT["调试"] = "";
                //ClientThread.DForThread(ClientThread.OutPutHT);

                Hashtable InPutHT = new Hashtable();
                InPutHT["打洞消息类"] = trashmes;
                //发送打洞回应消息
                ClientThread.SDC.SendDispatchCenter_Dispatch("Send_ACKTrashMessage", InPutHT);

                

                
            }
        }

        /// <summary>
        /// 收到点对点打洞应答消息
        /// </summary>
        /// <param name="msgObj"></param>
        private void Receive_ACKTrashMessage(object msgObj)
        {
            ACKTrashMessage ACKtrashmsg = (ACKTrashMessage)msgObj;
            if (ACKtrashmsg.Usernameto == ClientThread.my_SRV_Username)//验证是否正常消息
            {
                mut.WaitOne();
                //更新私有状态
                ClientThread.userList.Find(ACKtrashmsg.Usernameform).Online_own = "可连接";
                ClientThread.userList.Find(ACKtrashmsg.Usernameform).Type_own = ACKtrashmsg.Linktype;
                ClientThread.userList.Find(ACKtrashmsg.Usernameform).NetPoint_own = ClientThread.remotePoint;
                mut.ReleaseMutex();
                //回调显示
                //ClientThread.OutPutHT.Clear();
                //ClientThread.OutPutHT["事件消息"] = "接收到打洞应答消息,来自:" + ACKtrashmsg.Usernameform + ":" + ClientThread.remotePoint.Address + ":" + ClientThread.remotePoint.Port;
                //ClientThread.OutPutHT["调试"] = "";
                //ClientThread.DForThread(ClientThread.OutPutHT);
            }


        }

        /// <summary>
        /// 收到普通文本消息
        /// </summary>
        /// <param name="msgObj"></param>
        private void Receive_WorkMessage(object msgObj)
        {
            // 转换消息
            WorkMessage workMsg = (WorkMessage)msgObj;
            if (workMsg.Usernameto == ClientThread.my_SRV_Username)//验证是否正常消息
            {
                Hashtable OutPutHT = new Hashtable();
                OutPutHT["事件消息"] = "接收到一个通用消息,来自:" + workMsg.Usernameform + ":" + ClientThread.remotePoint.Address + ":" + ClientThread.remotePoint.Port;
                OutPutHT["文字消息"] = workMsg.Textmessage;
                OutPutHT["文字消息标识帐号"] = workMsg.Usernameform;
                OutPutHT["调试"] = "";
                //调用委托，回显
                ClientThread.DForThread(OutPutHT);

                //发送普通文本应答消息
                Hashtable InPutHT = new Hashtable();
                InPutHT["普通文本消息类"] = workMsg;
                ClientThread.SDC.SendDispatchCenter_Dispatch("Send_ACKMessage", InPutHT);

            }
        }

        /// <summary>
        /// 收到普通文本应答消息
        /// </summary>
        /// <param name="msgObj"></param>
        private void Receive_ACKMessage(object msgObj)
        {
            ACKMessage RunBack = (ACKMessage)msgObj;
            if (RunBack.Usernameto == ClientThread.my_SRV_Username)//验证是否正常消息
            {
                Hashtable OutPutHT = new Hashtable();
                OutPutHT["事件消息"] = "接收到" + RunBack.Usernameform + "的消息应答,执行结果: " + RunBack.Remessage;
                OutPutHT["调试"] = "";
                //调用委托，回显
                ClientThread.DForThread(OutPutHT);
            }
        }
        #endregion


        /// <summary>
        /// 收到请求打开远程桌面消息
        /// </summary>
        /// <param name="msgObj"></param>
        private void  Receive_OpenTscMessage(object msgObj)
        {
            OpenTscMessage opentsmmessage = (OpenTscMessage)msgObj;
            if (opentsmmessage.Usernameto == ClientThread.my_SRV_Username)//验证是否正常消息
            {

                Hashtable OutPutHT = new Hashtable();
                OutPutHT["事件消息"] = "接收到打开远程桌面请求,来自:" + opentsmmessage.Usernameform + ":" + ClientThread.remotePoint.Address + ":" + ClientThread.remotePoint.Port;
                OutPutHT["调试"] = "";
                //调用委托，回显
                ClientThread.DForThread(OutPutHT);

                //打开远程桌面===========
                OpenMSTSC om = new OpenMSTSC();
                om.Begin_openMSTSC(opentsmmessage.WinUser, opentsmmessage.WinPass);

                //发送应答
                Hashtable InPutHT = new Hashtable();
                InPutHT["打开远程桌面消息类"] = opentsmmessage;
                ClientThread.SDC.SendDispatchCenter_Dispatch("Send_ACKOpenTscMessage", InPutHT);

            }
        }

        
        /// <summary>
        /// 收到请求打开远程桌面应答消息
        /// </summary>
        /// <param name="msgObj"></param>
        private void Receive_ACKOpenTscMessage(object msgObj)
        {
            ACKOpenTscMessage RunBack = (ACKOpenTscMessage)msgObj;
            if (RunBack.Usernameto == ClientThread.my_SRV_Username)//验证是否正常消息
            {
                Hashtable OutPutHT = new Hashtable();
                OutPutHT["事件消息"] = "接收到" + RunBack.Usernameform + "的消息应答,执行结果: " + RunBack.Remessage;
                OutPutHT["调试"] = "";
                //调用委托，回显
                ClientThread.DForThread(OutPutHT);
            }
        }


        //=====================================================================================

        #region 文件管理


        /// <summary>
        /// 收到获取磁盘分区 消息类
        /// </summary>
        /// <param name="msgObj"></param>
        private void Receive_GetDiskInfoMessage(object msgObj)
        {
            GetDiskInfoMessage getdisk = (GetDiskInfoMessage)msgObj;
            if (getdisk.Usernameto == ClientThread.my_SRV_Username)//验证是否正常消息
            {

                Hashtable OutPutHT = new Hashtable();
                OutPutHT["事件消息"] = "接收到请求获取磁盘列表消息,来自:" + getdisk.Usernameform + ":" + ClientThread.remotePoint.Address + ":" + ClientThread.remotePoint.Port;
                OutPutHT["调试"] = "";
                //调用委托，回显
                ClientThread.DForThread(OutPutHT);

                //发送应答
                Hashtable InPutHT = new Hashtable();
                InPutHT["来源消息类"] = getdisk;
                ClientThread.SDC.SendDispatchCenter_Dispatch("Send_ACKGetDiskInfoMessage", InPutHT);

            }
        }

        /// <summary>
        /// 收到 获取磁盘分区 应答消息类
        /// </summary>
        /// <param name="msgObj"></param>
        private void Receive_ACKGetDiskInfoMessage(object msgObj)
        {
            ACKGetDiskInfoMessage ACKdiskinfo = (ACKGetDiskInfoMessage)msgObj;
            if (ACKdiskinfo.Usernameto == ClientThread.my_SRV_Username)//验证是否正常消息
            {
                Hashtable OutPutHT = new Hashtable();
                OutPutHT["事件消息"] = "接收到" + ACKdiskinfo.Usernameform + "的磁盘列表";
                OutPutHT["磁盘列表"] = ACKdiskinfo.FilesInfoList;
                OutPutHT["磁盘列表标识帐号"] = ACKdiskinfo.Usernameform;
                OutPutHT["调试"] = "";
                //调用委托，回显
                ClientThread.DForThread(OutPutHT);
            }
        }


        /// <summary>
        /// 收到获取目录列表 消息类
        /// </summary>
        /// <param name="msgObj"></param>
        private void Receive_GetFilesInfoMessage(object msgObj)
        {
            GetFilesInfoMessage getfilesinfo = (GetFilesInfoMessage)msgObj;
            if (getfilesinfo.Usernameto == ClientThread.my_SRV_Username)//验证是否正常消息
            {

                Hashtable OutPutHT = new Hashtable();
                OutPutHT["事件消息"] = "接收到请求获取磁盘列表消息,来自:" + getfilesinfo.Usernameform + ":" + ClientThread.remotePoint.Address + ":" + ClientThread.remotePoint.Port;
                OutPutHT["调试"] = "";
                //调用委托，回显
                ClientThread.DForThread(OutPutHT);

                //发送应答
                Hashtable InPutHT = new Hashtable();
                InPutHT["来源消息类"] = getfilesinfo;
                ClientThread.SDC.SendDispatchCenter_Dispatch("Send_ACKGetFilesInfoMessage", InPutHT);

            }
        }

        /// <summary>
        /// 收到 获取目录列表 应答消息类
        /// </summary>
        /// <param name="msgObj"></param>
        private void Receive_ACKGetFilesInfoMessage(object msgObj)
        {
            
            ACKGetFilesInfoMessage ACKfilesinfo = (ACKGetFilesInfoMessage)msgObj;
            if (ACKfilesinfo.Usernameto == ClientThread.my_SRV_Username)//验证是否正常消息
            {
                Hashtable OutPutHT = new Hashtable();
                OutPutHT["事件消息"] = "接收到" + ACKfilesinfo.Usernameform + "的目录";
                OutPutHT["目录列表"] = ACKfilesinfo.FilesInfoList;
                OutPutHT["目录列表标识帐号"] = ACKfilesinfo.Usernameform;
                OutPutHT["调试"] = "";
                //调用委托，回显
                ClientThread.DForThread(OutPutHT);
            }
        }

        ///// <summary>
        ///// 收到 要求上传文件 消息类
        ///// </summary>
        ///// <param name="msgObj"></param>
        //private void Receive_ApplyUpLoadMessage(object msgObj)
        //{
        //    msgObj_th = msgObj;
        //    Thread SendDD = new Thread(new ThreadStart(BeiDong_Send_files));
        //    SendDD.Start();

            
        //}

        //public void BeiDong_Send_files()
        //{
        //    ApplyUpLoadMessage UpLoad = (ApplyUpLoadMessage)msgObj_th;
        //    if (UpLoad.Usernameto == ClientThread.my_SRV_Username)//验证是否正常消息
        //    {
        //        ClientThread.OutPutHT.Clear();
        //        ClientThread.OutPutHT["事件消息"] = "收到请求上传消息";
        //        ClientThread.OutPutHT["调试"] = "";
        //        //调用委托，回显
        //        ClientThread.DForThread(ClientThread.OutPutHT);

        //        //开始上传
        //        User toUser = ClientThread.UCown.Find(UpLoad.Usernameform);
        //        if (toUser == null || toUser.Online_own == "")//用户不存在
        //        {
        //            return;
        //        }

        //        string onlyone_f = ClientThread.my_SRV_Username + "|" + UpLoad.OrderFilePath + "|" + UpLoad.SaveFilePath;

        //        if (ClientThread.ht_UDPsw.ContainsKey(onlyone_f))
        //        {
        //            MessageBox.Show("选定文件正在发往[" + ClientThread.my_SRV_Username + "]过程中.无需重复发送");
        //            //ClientThread.OutPutHT.Clear();
        //            //ClientThread.OutPutHT["事件消息"] = "选定文件正在发往[" + ClientThread.my_SRV_Username + "]过程中.无需重复发送";
        //            //ClientThread.OutPutHT["调试"] = "";
        //            //调用委托，回显
        //            //ClientThread.DForThread(ClientThread.OutPutHT);
        //            return;
        //        }

        //        ClientThread.send_usw = new UDPSlidingWindow(ClientThread, UpLoad.OrderFilePath, UpLoad.SaveFilePath);

        //        ClientThread.send_usw.target = toUser.NetPoint_own;
        //        ClientThread.send_usw.username = UpLoad.Usernameform;
        //        //ClientThread.send_usw.msg = InPutHT["附加消息"].ToString();

        //        //创建一个文件对象   
        //        FileInfo EzoneFile = new FileInfo(UpLoad.OrderFilePath);
        //        FileStream EzoneStream = EzoneFile.OpenRead();
        //        byte[] sendBuffer_file = new byte[EzoneFile.Length]; ;
        //        EzoneStream.Read(sendBuffer_file, 0, sendBuffer_file.Length);
        //        ClientThread.send_usw.sendBuffer_file = sendBuffer_file;


        //        ClientThread.ht_UDPsw[onlyone_f] = ClientThread.send_usw;


        //        //启动线程，开始发送文件
        //        Thread SendDD = new Thread(new ThreadStart(((UDPSlidingWindow)(ClientThread.ht_UDPsw[onlyone_f])).sendThread));
        //        SendDD.Start();

        //    }

            
        //}

        #endregion

    }
}
