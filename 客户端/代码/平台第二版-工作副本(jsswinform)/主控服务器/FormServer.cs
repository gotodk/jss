using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using 灵魂裸奔服务器端.服务器类库;
using 公用通讯协议类库.共用类库;

namespace 主控服务器
{
    public partial class FormServer : Form
    {
        /// <summary>
        /// 含有执行线程的服务器动作类
        /// </summary>
        Server server;
        /// <summary>
        /// 用于传入线程的委托
        /// </summary>
        delegateForThread tempDFT;


        /// <summary>
        /// 用户列表
        /// </summary>
        UserCollection userList;

       


        public FormServer()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
        }

        private void B_beginrunserver_Click(object sender, EventArgs e)
        {
            //传入线程的哈希表参数几何
            Hashtable InPutHT = new Hashtable();
            //InPutHT["自定义端口"] = tb_port.Text;//最好验证端口必须为合法数字
            InPutHT["服务器端监听端口"] = tb_port.Text;
            //实例化委托,并参数传递给线程执行类开始执行线程
            tempDFT = new delegateForThread(ShowThreadResult);
            server = new Server(InPutHT, tempDFT);
            try
            {
                L_sservermsginfo.Text = "服务正在尝试启动,请等待...";
                server.Start();
            }
            catch (Exception expp) { RTB_errmsg.Text = "启动服务失败: " + expp.ToString(); }
        }


        //显示线程处理结果的函数(用于处理线程返回数据)
        private void ShowThreadResult(Hashtable OutPutHT)
        {
            //调用异步委托
            try
            {
                Invoke(new delegateForShow(ShowThreadResult_Invoke), new Hashtable[] { OutPutHT });
            }
            catch (Exception expp) { MessageBox.Show("调用异步委托失败: " + expp.ToString()); }
        }

        //处理非线程创建的控件
        private void ShowThreadResult_Invoke(Hashtable OutPutHT)
        {
            //显示错误日志
            RTB_errmsg.Text = OutPutHT["调试"].ToString();

            //更新界面
            if (OutPutHT.ContainsKey("服务器状态"))
            {
                L_sservermsginfo.Text = OutPutHT["服务器状态"].ToString();
            }

            if (OutPutHT.ContainsKey("事件消息"))
            {
                rtb_cmdlist.Text = rtb_cmdlist.Text + OutPutHT["事件消息"].ToString() + "【消息时间："+System.DateTime.Now.ToLongDateString()+ " " +System.DateTime.Now.ToLongTimeString()+"】\n";
            }
            if (OutPutHT.ContainsKey("在线列表"))
            {
                groupBox1.Text = "在线用户列表(" + ((UserCollection)OutPutHT["在线列表"]).Count.ToString() + ")人在线";
                Collection2DataTable C2D = new Collection2DataTable((UserCollection)OutPutHT["在线列表"]);
                DGVuserlist.DataSource = C2D.GetDataTable().DefaultView;
                userList = (UserCollection)OutPutHT["在线列表"];
            }

        }

        private void B_endserver_Click(object sender, EventArgs e)
        {
            //清理线程和监听实例以及其他可能的东西
            Dispose();
            System.Environment.Exit(0);

            //try
            //{
            //    L_sservermsginfo.Text = "P2P服务正在尝试停止,请等待...";
            //    server.Stop();
            //}
            //catch (Exception expp) { MessageBox.Show("停止服务失败: " + expp.ToString()); }
        }

        private void FormServer_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void DGVuserlist_MouseClick(object sender, MouseEventArgs e)
        {
            if (DGVuserlist.SelectedCells.Count > 0)
            {
                infofsmb.Text = DGVuserlist.SelectedCells[0].Value.ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (userList != null)
            {
                User onlineUser = userList.Find(infofsmb.Text);
                if (onlineUser != null)
                {
                    //定时发送登陆消息指令
                    SendDispatchCenter SDC = new SendDispatchCenter(server);
                    if (server != null)
                    {
                        Hashtable InPutHT = new Hashtable();
                        InPutHT["接收人"] = onlineUser;
                        InPutHT["消息内容"] = textBox1.Text;
                        SDC.SendDispatchCenter_Dispatch("Send_WorkMessage", InPutHT);
                    }
                }
                
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FormWindowsServicesTry FWST = new FormWindowsServicesTry();
            FWST.Show();
        }
    }
}
