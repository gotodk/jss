using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace 银行监控平安
{
    public partial class FormMain : Form
    {
        #region public val
        /// <summary>
        /// 遮罩层
        /// </summary>
        private MyOpaqueLayer m_OpaqueLayer = null;
        /// <summary>
        /// 包体
        /// </summary>
        DataTable dtTi = null;
        /// <summary>
        /// 包头
        /// </summary>
        DataTable dtTou = null;

        #endregion

        public FormMain()
        {
            InitializeComponent();
        }

        private void btnLockIP_Click(object sender, EventArgs e)
        {
            txtIP.Enabled = !txtIP.Enabled;
            txtPort.Enabled = !txtPort.Enabled;
            if (txtIP.Enabled)
                btnLockIP.Text = "锁定IP:Port";
            else
                btnLockIP.Text = "解锁IP:Port";
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            GetXMLList();
            this.txtStateAll.Text = "all info console >> ...";
            this.txtStateThis.Text = "last info console >> ...";
        }

        /// <summary>
        /// 绑定XML Source列表
        /// </summary>
        private void GetXMLList()
        {
            //cBoxXML
            cBoxXML.Items.Clear();
            string xmlPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\XML\";
            List<FileInfo> FI = GetAllFilesInDirectory(xmlPath);
            foreach (FileInfo fi in FI)
            {
                if (fi.FullName.IndexOf("壮哉我大平安报文头") == -1)
                    cBoxXML.Items.Add(fi);
            }
            cBoxXML.DropDownWidth = 260;
            if (cBoxXML.Items.Count > 0)
                cBoxXML.SelectedItem = cBoxXML.Items[0];
            else
                MessageBox.Show("XML文件夹中没有文件");
        }
        /// <summary>
        /// 初始化包头
        /// </summary>
        /// <param name="No"></param>
        private void InitBaotou(string No)
        {
 
        }

        /// <summary>
        /// 返回指定目录下的所有文件信息
        /// </summary>
        /// <param name="strDirectory"></param>
        /// <returns></returns>
        private List<FileInfo> GetAllFilesInDirectory(string strDirectory)
        {
            List<FileInfo> listFiles = new List<FileInfo>(); //保存所有的文件信息  
            DirectoryInfo directory = new DirectoryInfo(strDirectory);
            DirectoryInfo[] directoryArray = directory.GetDirectories();
            FileInfo[] fileInfoArray = directory.GetFiles();
            if (fileInfoArray.Length > 0) listFiles.AddRange(fileInfoArray);
            foreach (DirectoryInfo _directoryInfo in directoryArray)
            {
                DirectoryInfo directoryA = new DirectoryInfo(_directoryInfo.FullName);
                DirectoryInfo[] directoryArrayA = directoryA.GetDirectories();
                FileInfo[] fileInfoArrayA = directoryA.GetFiles();
                if (fileInfoArrayA.Length > 0) listFiles.AddRange(fileInfoArrayA);
                GetAllFilesInDirectory(_directoryInfo.FullName);//递归遍历  
            }
            return listFiles;
        }

        /// <summary>
        /// 读取XML
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReadXML_Click(object sender, EventArgs e)
        {
            #region 处理包体
            if (cBoxXML.SelectedItem.ToString().Trim() != "")
            {
                FileInfo FI = (FileInfo)(cBoxXML.SelectedItem);
                dtTi = new DataTable();
                dtTi.ReadXml(FI.FullName);
                dvBaoti.DataSource = dtTi;
                for (int i = 0; i < dvBaoti.Columns.Count; i++)
                {
                    dvBaoti.Columns[i].ToolTipText = dtTi.Columns[i].Caption;
                }
                GetBaotou();
                dvBaotou.DataSource = dtTou;
                dtTi.AcceptChanges();
                dtTou.AcceptChanges();
            }
            #endregion
        }
        /// <summary>
        /// 处理包头
        /// </summary>
        public void GetBaotou()
        {
            Core c = new Core();
            dtTou = new DataTable();
                string path = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\XML\壮哉我大平安报文头.xml";
            dtTou.ReadXml(path);//初始化数据
            c.BaotouFresh(ref dtTou);
            string No = dtTi.Columns.Contains("FunctionId") ? dtTi.Rows[0]["FunctionId"].ToString() : "";//接口编号
            dtTou.Rows[0]["TranFunc"] = No;//替换接口编号
        }

        //发送到银行
        private void btnSend_Click(object sender, EventArgs e)
        {
            this.txtStateAll.Text += Environment.NewLine + "正在处理数据包……";
            string ip = txtIP.Text.Trim();
            string port = txtPort.Text.Trim();
            this.txtStateAll.Text += Environment.NewLine + "目标IP地址：" + ip + ":" + port;

            this.txtStateThis.Text = "last info console >> ...";
            this.txtStateThis.Text += Environment.NewLine + "正在处理数据包……";
            this.txtStateThis.Text += Environment.NewLine + "目标IP地址：" + ip + ":" + port;
            if (dtTou.Rows[0]["TranFunc"] == null || dtTou.Rows[0]["TranFunc"].ToString().Trim() == "")
            {
                //正常情况下其实不用这个，如果模拟银行端发包，这里就有可能是空的。
                MessageBox.Show("请填写包头接口号");
                return;
            }
            Hashtable InPutHT = new Hashtable();
            delegateForThread tempDFT = new delegateForThread(ShowThreadResult_Log);
            Hashtable ht = new Hashtable();
            ht["dtTi"] = dtTi;
            ht["dtTou"] = dtTou;

            Route r = new Route(ht, tempDFT);
            Thread trd = new Thread(new ThreadStart(r.SendToBank));
            trd.IsBackground = true;
            trd.Start();
        }
        private void ShowThreadResult_Log(Hashtable OutPutHT)
        {
            try
            {
                //调用异步委托
                Invoke(new delegateForThreadShow(StartInvoke), new Hashtable[] { OutPutHT });
            }
            catch (Exception ex)
            {
                //StringOP.WriteLog("委托回调错误：" + ex.ToString());
                string msg = ex.Message;
            }
        }
        private void StartInvoke(Hashtable OutPutHT)
        {
            this.txtStateAll.Text += Environment.NewLine+ "证券to银行数据包：" + OutPutHT["bao"].ToString();
            this.txtStateAll.Text += Environment.NewLine+ "包头：" + OutPutHT["baotou"].ToString();
            this.txtStateAll.Text += Environment.NewLine+ "包体：" + OutPutHT["baoti"].ToString();

            this.txtStateThis.Text += Environment.NewLine + "证券to银行数据包：" + OutPutHT["bao"].ToString();
            this.txtStateThis.Text += Environment.NewLine + "包头：" + OutPutHT["baotou"].ToString();
            this.txtStateThis.Text += Environment.NewLine + "包体：" + OutPutHT["baoti"].ToString();
            //MessageBox.Show(Encoding.GetEncoding("GB2312").GetBytes(OutPutHT["baotou"].ToString()).Length.ToString());
            string ip = txtIP.Text.Trim();
            string port = txtPort.Text.Trim();
            byte[] b = OutPutHT["byteAry"] as byte[];

            Hashtable InPutHT = new Hashtable();
            delegateForThread tempDFT = new delegateForThread(Reback);
            Hashtable ht = new Hashtable();
            ht["ip"] = ip;
            ht["port"] = port;
            ht["Data"] = b;
            Route r = new Route(ht, tempDFT);
            Thread trd = new Thread(new ThreadStart(r.Send));
            trd.IsBackground = true;
            trd.Start();
        }
        private void Reback(Hashtable OutPutHT)
        {
            try
            {
                //调用异步委托
                Invoke(new delegateForThreadShow(Reresult), new Hashtable[] { OutPutHT });
            }
            catch (Exception ex)
            {
                StringOP.WriteLog("委托回调错误：" + ex.ToString());
            }
        }
        private void Reresult(Hashtable OutPutHT)
        {
            this.txtStateAll.Text += Environment.NewLine + "发送状态：" + OutPutHT["state"].ToString();
            this.txtStateThis.Text += Environment.NewLine + "发送状态：" + OutPutHT["state"].ToString();
            this.txtStateThis.Text += Environment.NewLine + "描述：" + OutPutHT["msg"].ToString();
            if (OutPutHT["state"].ToString() == "ok")
            {
                Core c = new Core();
                byte[] bbb = (byte[])OutPutHT["DataByte"];
                dvBankTou.DataSource = c.GetBaotou(bbb);
                dvBank.DataSource = c.GetBaoti(bbb, 0);

                this.txtStateThis.Text += "\n" + "反馈包：" + OutPutHT["DataStr"].ToString();
            }
        }
        private void btnListen_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage2;
        }


        #region 状态同步
        private void dvBaoti_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            Core c = new Core();
            //dtTi = dvBaoti.DataSource as DataTable;
            //dtTi.AcceptChanges();
            dtTou.Rows[0]["Length"] = c.GetZero(c.GetBaotiLength(dtTi).ToString());
            dtTou.Rows[0]["TranFunc"] = dtTi.Rows[0]["FunctionId"].ToString();
            //dtTi.AcceptChanges();
            //dtTou.AcceptChanges();
        }
        #endregion
        #region 遮罩层

        /// <summary>
        /// 显示遮罩层
        /// </summary>
        /// <param name="control"></param>
        /// <param name="alpha"></param>
        /// <param name="showLoadingImage"></param>
        protected void ShowOpaqueLayer(Control control, int alpha, bool showLoadingImage)
        {
            if (this.m_OpaqueLayer == null)
            {
                this.m_OpaqueLayer = new MyOpaqueLayer(alpha, showLoadingImage);
                control.Controls.Add(this.m_OpaqueLayer);
                this.m_OpaqueLayer.Dock = DockStyle.Fill;
                this.m_OpaqueLayer.BringToFront();
            }
            this.m_OpaqueLayer.Enabled = true;
            this.m_OpaqueLayer.Visible = true;


        }
        /// <summary>
        /// 隐藏遮罩层
        /// </summary>
        protected void HideOpaqueLayer()
        {
            if (this.m_OpaqueLayer != null)
            {
                this.m_OpaqueLayer.Enabled = false;
                this.m_OpaqueLayer.Visible = false;
            }
        }
        #endregion
        private void button1_Click(object sender, EventArgs e)
        {
            //Thread t = new Thread(new ThreadStart(this.ShowOpaqueLayer(this, 125, true)));
            test t = new test();
            t.Show();
        }
        /// <summary>
        /// 开启监听
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnListenStart_Click(object sender, EventArgs e)
        {
            string ip = this.txtListenIP.Text.Trim();//监听IP
            string port = this.txtListenPort.Text.Trim();//监听端口
            Hashtable ht = new Hashtable();
            ht["ip"] = ip;
            ht["port"] = port;

            delegateForThread tempDFT = new delegateForThread(ListenInvoke);
            Route r = new Route(ht, tempDFT);
            Thread trd = new Thread(new ThreadStart(r.StartListen));
            trd.IsBackground = true;
            trd.Start();

        }
        private void ListenInvoke(Hashtable OutPutHT)
        {
            try
            {
                //调用异步委托
                Invoke(new delegateForThreadShow(ListenResult), new Hashtable[] { OutPutHT });
            }
            catch (Exception ex)
            {
                StringOP.WriteLog("委托回调错误：" + ex.ToString());
            }
        }
        private void ListenResult(Hashtable OutPutHT)
        {
            //txtListenLast
            if (OutPutHT["执行状态"].ToString().IndexOf("端口") >= 0)
            {
                this.txtListenAll.Text += "执行状态：" + OutPutHT["执行状态"].ToString();
                this.txtListenAll.Text += Environment.NewLine + "描述：" + OutPutHT["详细信息"].ToString();
            }
            else
            {
                this.txtListenAll.Text += Environment.NewLine + "指令描述：" + OutPutHT["执行状态"].ToString();
                this.txtListenAll.Text += Environment.NewLine + "包String：" + OutPutHT["str"].ToString();
                this.txtListenLast.Text = "指令描述：" + OutPutHT["执行状态"].ToString();
                this.txtListenLast.Text += Environment.NewLine + "包String：" + OutPutHT["str"].ToString();
                this.txtListenLast.Text += Environment.NewLine + "正在绑定数据包到界面……";
                this.dataGridViewBaotou.DataSource = (DataTable)OutPutHT["baotou"];
                this.dataGridViewBaoti.DataSource = (DataTable)OutPutHT["baoti"];
                this.txtListenLast.Text += Environment.NewLine + "Done.";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //richTextBox1-》dataGridView1-》dataGridView2


        }



    }
}
