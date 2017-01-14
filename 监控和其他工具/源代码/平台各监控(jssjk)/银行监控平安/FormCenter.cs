using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Xml;
using System.Collections;
using System.Threading;

namespace 银行监控平安
{
    public partial class FormCenter : Form
    {
        public FormCenter()
        {
            InitializeComponent();
        }
        //加载
        private void FormMain_Load(object sender, EventArgs e)
        {
            //读取配置文件中的端口号           
            txtPort.Text = ConfigurationManager.AppSettings["Port"];
            //设置日期
            dtSign.Value = DateTime.Now;
            dtKXH.Value = DateTime.Now;
            dtCRJ.Value = DateTime.Now;
            dtQS.Value = DateTime.Now;
        }

        #region 开启监听
        private void btnListenStart_Click(object sender, EventArgs e)
        {
            btnListenStart.Enabled = false;
            string port = this.txtPort.Text.Trim();//监听端口
            Hashtable ht = new Hashtable();
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
            if (OutPutHT["执行状态"].ToString().IndexOf("监听") > 0)
            {
                this.txtListenAll.Text = OutPutHT["执行状态"].ToString();
                this.txtListenAll.Text += Environment.NewLine + OutPutHT["详细信息"].ToString();
            }
            else if (OutPutHT["执行状态"].ToString().IndexOf("数据包") > 0)
            {
                this.txtListenAll.Text += Environment.NewLine;
                this.txtListenAll.Text += Environment.NewLine + OutPutHT["执行状态"].ToString();
                this.txtListenAll.Text += Environment.NewLine + "包String：" + OutPutHT["详细信息"].ToString();
                this.txtListenAll.Text += Environment.NewLine + "正在绑定数据包到界面...";
                this.dataGridViewBaotou.DataSource = (DataTable)OutPutHT["baotou"];
                this.dataGridViewBaoti.DataSource = (DataTable)OutPutHT["baoti"];
                this.txtListenAll.Text += Environment.NewLine + "Done.";
            }
            else
            {
                this.txtListenAll.Text += Environment.NewLine + OutPutHT["执行状态"].ToString();
                this.txtListenAll.Text += Environment.NewLine + OutPutHT["详细信息"].ToString();
            }
            //监听到【1005】清算失败、对账不平的情况下反馈给清算功能标签页
            if (OutPutHT["执行状态"].ToString() == "【1005】应答成功" )
            {
                if (OutPutHT["类型"].ToString() == "清算失败")
                {
                    this.txtStateQS.AppendText(Environment.NewLine + DateTime.Now .ToString("yyyy-MM-dd HH:mm:ss")+" 收到银行【1001】清算失败文件");
                    this.txtStateQS.AppendText(Environment.NewLine + "文件名：" + OutPutHT["文件名"].ToString());
                   // this.lblFailName.Text = OutPutHT["文件名"].ToString();
                    this.lblQSZT.Text = OutPutHT["文件名"].ToString();
                }
                else if (OutPutHT["类型"].ToString() == "对账不平")
                {
                    this.txtStateQS.AppendText(Environment.NewLine + DateTime.Now.ToString ("yyyy-MM-dd HH:mm:ss")+ " 收到银行【1007】对账不平文件");
                    this.txtStateQS.AppendText(Environment.NewLine + "文件名：" + OutPutHT["文件名"].ToString());
                    this.lblDZBP.Text = OutPutHT["文件名"].ToString();
                }
            }

        }
        //打开收发包测试功能
        private void btnOpenTest_Click(object sender, EventArgs e)
        {
            FormMain t = new FormMain();
            t.Show();
        }
        #endregion

        #region 签到签退
        /// <summary>
        /// 签到
        /// </summary>
        private void btnSignIn_Click(object sender, EventArgs e)
        {
            string msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  正在签到...";
            this.txtStateSign.Text = this.txtStateSign.Text.Replace("等待执行...", msg);


            Hashtable ht = new Hashtable();
            ht["type"] = "签到";
            delegateForThread tempDFT = new delegateForThread(Sign_Invoke);
            RunThread rt = new RunThread(ht, tempDFT);
            Thread trd = new Thread(new ThreadStart(rt.RunSign));
            trd.IsBackground = true;
            trd.Start();
        }

        // 签退
        private void btnSignOut_Click(object sender, EventArgs e)
        {
            string msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  正在签退...";
            this.txtStateSign.Text = this.txtStateSign.Text.Replace("等待执行...", msg);

            Hashtable ht = new Hashtable();
            ht["type"] = "签退";
            delegateForThread tempDFT = new delegateForThread(Sign_Invoke);
            RunThread rt = new RunThread(ht, tempDFT);
            Thread trd = new Thread(new ThreadStart(rt.RunSign));
            trd.IsBackground = true;
            trd.Start();
        }
        private void Sign_Invoke(Hashtable OutPutHT)
        {
            try
            {
                //调用异步委托
                Invoke(new delegateForThreadShow(Sign_Result), new Hashtable[] { OutPutHT });
            }
            catch (Exception ex)
            {
                StringOP.WriteLog("委托回调错误：" + ex.ToString());
                this.txtStateSign.Text += Environment.NewLine + "委托回调错误：" + ex.ToString();
            }
        }
        private void Sign_Result(Hashtable OutPutHT)
        {
            DataSet dsreturn = (DataSet)OutPutHT["返回值"];
            if (dsreturn == null || dsreturn.Tables["返回值单条"].Rows.Count <= 0)
            {
                this.txtStateSign.Text += Environment.NewLine + "执行结果：err";
                this.txtStateSign.Text += Environment.NewLine + "描述：未接收到返回数据！";
                this.txtStateSign.Text += Environment.NewLine + Environment.NewLine + "等待执行...";

            }
            else
            {
                this.txtStateSign.Text += Environment.NewLine + "执行结果：" + dsreturn.Tables["返回值单条"].Rows[0]["执行结果"].ToString();
                this.txtStateSign.Text += Environment.NewLine + "描述:" + dsreturn.Tables["返回值单条"].Rows[0]["提示文本"].ToString();
                this.txtStateSign.Text += Environment.NewLine + Environment.NewLine + "等待执行...";
            }
        }
        #endregion

        #region //会员开销户流水匹配
        //执行会员开销户匹配
        private void btnRunKXH_Click(object sender, EventArgs e)
        {
            btnRunKXH.Enabled = false;
            this.dgvExpKXH.DataSource = null;
            this.dgvKXHDZ.DataSource = null;

            Program.state1005["状态"] = "开始开销户";
            string msg = Environment.NewLine + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 开始执行会员开销户流水匹配";
            if (txtStateKXH.Text == "等待执行...")
            {
                this.txtStateKXH.Text = this.txtStateKXH.Text.Replace("等待执行...", msg);
            }
            else
            {
                this.txtStateKXH.AppendText(Environment.NewLine + msg);
            }

            //调用后台方法，发送1006接口
            Hashtable ht = new Hashtable();
            ht["type"] = "开销户";
            ht["BeginTime"] = DateTime.Now.ToString("yyyyMMdd") + "000000";
            ht["EndTime"] = DateTime.Now.ToString("yyyyMMdd") + "235959";
            delegateForThread tempDFT = new delegateForThread(KXH_Invoke);
            RunThread rt = new RunThread(ht, tempDFT);
            Thread trd = new Thread(new ThreadStart(rt.RunMatch));
            trd.IsBackground = true;
            trd.Start();
        }
        private void KXH_Invoke(Hashtable OutPutHT)
        {
            try
            {
                //调用异步委托
                Invoke(new delegateForThreadShow(KXH_Result), new Hashtable[] { OutPutHT });
            }
            catch (Exception ex)
            {
                StringOP.WriteLog("委托回调错误：" + ex.ToString());
            }
        }
        private void KXH_Result(Hashtable OutPutHT)
        {
            if (OutPutHT["执行状态"].ToString().IndexOf("ERR") >= 0)
            {
                btnRunKXH.Enabled = true;
            }
            if (OutPutHT["执行状态"].ToString().IndexOf("匹配完毕") >= 0)
            {
                btnRunKXH.Enabled = true;

                //绑定对账数据用于导出
                DataTable dtDZ = ((DataTable)OutPutHT["银行数据"]).Clone();
                dtDZ.Merge((DataTable)OutPutHT["银行数据"], false, MissingSchemaAction.Ignore);
                dtDZ.Merge((DataTable)OutPutHT["本地数据"], false, MissingSchemaAction.Ignore);
                dgvKXHDZ.DataSource = dtDZ;

                //判断是否存在异常数据
                DataTable dtExp = (DataTable)OutPutHT["异常数据"];
                if (dtExp != null && dtExp.Rows.Count > 0)
                {
                    this.txtStateKXH.AppendText(Environment.NewLine + OutPutHT["执行状态"].ToString());
                    this.txtStateKXH.AppendText(Environment.NewLine + "出现异常数据。");
                    this.txtStateKXH.AppendText(Environment.NewLine + "正在绑定异常数据到界面...");
                    dgvExpKXH.DataSource = dtExp;
                    this.txtStateKXH.AppendText(Environment.NewLine + "Done");
                }
                else
                {
                    this.txtStateKXH.AppendText(Environment.NewLine + OutPutHT["执行状态"].ToString());
                    this.txtStateKXH.AppendText(Environment.NewLine + "数据匹配成功。");
                }
            }
            else
            {
                this.txtStateKXH.AppendText(Environment.NewLine + OutPutHT["执行状态"].ToString());
                this.txtStateKXH.AppendText(Environment.NewLine + OutPutHT["详细描述"].ToString());
                if (OutPutHT["详细描述"].ToString() == "等待银行生成文件通知...")
                {
                    btnBrkWatKXH.Enabled = true;
                }
            }
        }
        private void btnBankKXH_Click(object sender, EventArgs e)
        {
            string msg = Environment.NewLine + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            this.txtKXHBank.AppendText(msg);

            Hashtable ht = new Hashtable();
            ht["type"] = "开销户";
            ht["date"] = DateTime.Now.ToString("yyyyMMdd");

            delegateForThread tempDFT = new delegateForThread(btnBankKXH_Invoke);
            RunThread rt = new RunThread(ht, tempDFT);
            Thread trd = new Thread(new ThreadStart(rt.BankView));
            trd.IsBackground = true;
            trd.Start();
        }
        private void btnBankKXH_Invoke(Hashtable OutPutHT)
        {
            try
            {
                //调用异步委托
                Invoke(new delegateForThreadShow(btnBankKXH_Result), new Hashtable[] { OutPutHT });
            }
            catch (Exception ex)
            {
                StringOP.WriteLog("委托回调错误：" + ex.ToString());
            }
        }
        private void btnBankKXH_Result(Hashtable OutPutHT)
        {
            this.txtKXHBank.AppendText(Environment.NewLine + OutPutHT["执行状态"].ToString());
            this.txtKXHBank.AppendText(Environment.NewLine + OutPutHT["详细描述"].ToString());
        }


        //导出开销户匹配异常数据
        private void btnEptKXH_Click(object sender, EventArgs e)
        {
            if (dgvExpKXH.DataSource == null)
            {
                MessageBox.Show("当前无异常数据！");
                return;
            }
            else
            {
                DataTable dt = new DataTable();
                dt = ((DataTable)dgvExpKXH.DataSource).Copy();
                dt.TableName = "异常数据";
                DataSet ds = new DataSet();
                ds.Tables.Add(dt);
                string filename = "开销户匹配异常数据" + DateTime.Now.ToString("yyyyMMddHHmmss"); ;
                cMyXls1.BeginExport(ds, filename);
            }
        }
        //导出开销户匹配使用的原始数据
        private void btnEptKXHDZ_Click(object sender, EventArgs e)
        {
            if (dgvKXHDZ.DataSource == null)
            {
                MessageBox.Show("当前无对账数据！");
                return;
            }
            else
            {
                DataTable dt = new DataTable();
                dt = ((DataTable)dgvKXHDZ.DataSource).Copy();
                dt.TableName = "原始对账数据";
                DataSet ds = new DataSet();
                ds.Tables.Add(dt);
                string filename = "开销户匹配原始数据" + DateTime.Now.ToString("yyyyMMddHHmmss");
                cMyXls1.BeginExport(ds, filename);
            }
        }
        //人工中断开销户等待银行文件
        private void btnBrkWatKXH_Click(object sender, EventArgs e)
        {
            btnBrkWatKXH.Enabled = false;
            Program.state1005["状态"] = "人工中断开销户";
            Program.state1005["详情"] = "";
            Program.state1005["类型"] = "";
        }
        #endregion

        #region //会员出入金流水匹配
        //执行会员出入金流水匹配
        private void btnRunCRJ_Click(object sender, EventArgs e)
        {
            btnRunCRJ.Enabled = false;
            this.dgvExpCRJ.DataSource = null;
            this.dgvCRJDZ.DataSource = null;

            Program.state1005["状态"] = "开始出入金";
            string msg = Environment.NewLine + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 开始执行会员出入金流水匹配";
            if (txtStateCRJ.Text == "等待执行...")
            {
                this.txtStateCRJ.Text = this.txtStateCRJ.Text.Replace("等待执行...", msg);
            }
            else
            {
                this.txtStateCRJ.AppendText(Environment.NewLine + msg);
            }

            //调用后台方法，发送1006接口
            Hashtable ht = new Hashtable();
            ht["type"] = "出入金";
            ht["BeginTime"] = dtKXH.Value.ToString("yyyyMMdd") + "000000";
            ht["EndTime"] = dtKXH.Value.ToString("yyyyMMdd") + "235959";
            delegateForThread tempDFT = new delegateForThread(CRJ_Invoke);
            RunThread rt = new RunThread(ht, tempDFT);
            Thread trd = new Thread(new ThreadStart(rt.RunMatch));
            trd.IsBackground = true;
            trd.Start();
        }
        private void CRJ_Invoke(Hashtable OutPutHT)
        {
            try
            {
                //调用异步委托
                Invoke(new delegateForThreadShow(CRJ_Result), new Hashtable[] { OutPutHT });
            }
            catch (Exception ex)
            {
                StringOP.WriteLog("委托回调错误：" + ex.ToString());
            }
        }
        private void CRJ_Result(Hashtable OutPutHT)
        {
            if (OutPutHT["执行状态"].ToString().IndexOf("ERR") >= 0)
            {
                btnRunCRJ.Enabled = true;
            }
            if (OutPutHT["执行状态"].ToString().IndexOf("匹配完毕") >= 0)
            {
                btnRunCRJ.Enabled = true;

                //绑定对账数据用于导出
                DataTable dtDZ = ((DataTable)OutPutHT["银行数据"]).Clone();
                dtDZ.Merge((DataTable)OutPutHT["银行数据"], false, MissingSchemaAction.Ignore);
                dtDZ.Merge((DataTable)OutPutHT["本地数据"], false, MissingSchemaAction.Ignore);
                dgvCRJDZ.DataSource = dtDZ;

                //判断是否存在异常数据
                DataTable dtExp = (DataTable)OutPutHT["异常数据"];
                if (dtExp != null && dtExp.Rows.Count > 0)
                {
                    this.txtStateCRJ.AppendText(Environment.NewLine + OutPutHT["执行状态"].ToString());
                    this.txtStateCRJ.AppendText(Environment.NewLine + "出现异常数据");
                    this.txtStateCRJ.AppendText(Environment.NewLine + "正在绑定异常数据到界面...");
                    dgvExpCRJ.DataSource = dtExp;
                    this.txtStateCRJ.AppendText(Environment.NewLine + "Done");
                }
                else
                {
                    this.txtStateCRJ.AppendText(Environment.NewLine + OutPutHT["执行状态"].ToString());
                    this.txtStateCRJ.AppendText(Environment.NewLine + "数据匹配成功");
                }
            }
            else
            {
                this.txtStateCRJ.AppendText(Environment.NewLine + OutPutHT["执行状态"].ToString());
                this.txtStateCRJ.AppendText(Environment.NewLine + OutPutHT["详细描述"].ToString());
                if (OutPutHT["详细描述"].ToString() == "等待银行生成文件通知...")
                {
                    btnBrkWatCRJ.Enabled = true;
                }
            }
        }
        //出入金查询银行生成文件进度
        private void btnBankCRJ_Click(object sender, EventArgs e)
        {
            string msg = Environment.NewLine + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            this.txtCRJBank.AppendText(msg);

            Hashtable ht = new Hashtable();
            ht["type"] = "出入金";
            ht["date"] = DateTime.Now.ToString("yyyyMMdd");

            delegateForThread tempDFT = new delegateForThread(btnBankCRJ_Invoke);
            RunThread rt = new RunThread(ht, tempDFT);
            Thread trd = new Thread(new ThreadStart(rt.BankView));
            trd.IsBackground = true;
            trd.Start();
        }
        private void btnBankCRJ_Invoke(Hashtable OutPutHT)
        {
            try
            {
                //调用异步委托
                Invoke(new delegateForThreadShow(btnBankCRJ_Result), new Hashtable[] { OutPutHT });
            }
            catch (Exception ex)
            {
                StringOP.WriteLog("委托回调错误：" + ex.ToString());
            }
        }
        private void btnBankCRJ_Result(Hashtable OutPutHT)
        {
            this.txtCRJBank.AppendText(Environment.NewLine + OutPutHT["执行状态"].ToString());
            this.txtCRJBank.AppendText(Environment.NewLine + OutPutHT["详细描述"].ToString());
        }

        //导出 出入金异常数据
        private void btnEptCRJ_Click(object sender, EventArgs e)
        {
            if (dgvExpCRJ.DataSource == null)
            {
                MessageBox.Show("当前无异常数据！");
                return;
            }
            else
            {
                DataTable dt = new DataTable();
                dt = ((DataTable)dgvExpCRJ.DataSource).Copy();
                dt.TableName = "异常数据";
                DataSet ds = new DataSet();
                ds.Tables.Add(dt);
                string filename = "出入金匹配异常数据" + DateTime.Now.ToString("yyyyMMddHHmmss"); ;
                cMyXls1.BeginExport(ds, filename);
            }
        }
        //导出 出入金原始数据
        private void btnExptCRJDZ_Click(object sender, EventArgs e)
        {
            if (dgvCRJDZ.DataSource == null)
            {
                MessageBox.Show("当前无数据！");
                return;
            }
            else
            {
                DataTable dt = new DataTable();
                dt = ((DataTable)dgvCRJDZ.DataSource).Copy();
                dt.TableName = "原始数据";
                DataSet ds = new DataSet();
                ds.Tables.Add(dt);
                string filename = "出入金匹配原始数据" + DateTime.Now.ToString("yyyyMMddHHmmss");
                cMyXls1.BeginExport(ds, filename);
            }
        }
        //人工中断出入金等待银行文件
        private void btnBrkWatCRJ_Click(object sender, EventArgs e)
        {
            btnBrkWatCRJ.Enabled = false;
            Program.state1005["状态"] = "人工中断出入金";
            Program.state1005["详情"] = "";
            Program.state1005["类型"] = "";
        }
        #endregion

        #region 清算页面功能
        #region //生成清算文件功能
        private void btnCreateQS_Click(object sender, EventArgs e)
        {
            btnCreateQS.Enabled = false;
            btnRunQS.Enabled = false;

            string msg = Environment.NewLine + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 开始生成清算文件";
            if (txtStateQS.Text == "等待执行...")
            {
                this.txtStateQS.Text = this.txtStateCRJ.Text.Replace("等待执行...", msg);
            }
            else
            {
                this.txtStateQS.AppendText(Environment.NewLine + msg);
            }

            delegateForThread tempDFT = new delegateForThread(CreatQS_Invoke);
            RunThread rt = new RunThread(null, tempDFT);
            Thread trd = new Thread(new ThreadStart(rt.CreateQSWJ));
            trd.IsBackground = true;
            trd.Start();
        }
        private void CreatQS_Invoke(Hashtable OutPutHT)
        {
            try
            {
                //调用异步委托
                Invoke(new delegateForThreadShow(CreatQS_Result), new Hashtable[] { OutPutHT });
            }
            catch (Exception ex)
            {
                StringOP.WriteLog("委托回调错误：" + ex.ToString());
            }
        }
        private void CreatQS_Result(Hashtable OutPutHT)
        {
            btnCreateQS.Enabled = true;
            this.txtStateQS.AppendText(Environment.NewLine + OutPutHT["执行状态"].ToString());
            this.txtStateQS.AppendText(Environment.NewLine + OutPutHT["详细描述"].ToString());
            if (OutPutHT["执行状态"].ToString().IndexOf("上传成功") >= 0)
            {
                btnRunQS.Enabled = true;
                lblQSWJXX.Text = OutPutHT["文件信息"].ToString();
            }
        }
        #endregion

        #region  //执行清算
        private void btnRunQS_Click(object sender, EventArgs e)
        {
            if (lblQSWJXX.Text.Trim() == "")
            {
                MessageBox.Show(this, "未获取到清算文件件信息，请先生成清算文件！");
                return;
            }
            Program.state1005["状态"] = "开始清算";
            Program.state1005["类型"] = "";
            Program.state1005["文件名"] = "";

            btnRunQS.Enabled = false;
            //lblFailName.Text = "清算失败文件名";
            lblDZBP.Text = "对账不平文件名";
            //lblQSZT.Text = "清算成功状态";
            lblQSZT.Text = "清算状态";
            string msg = Environment.NewLine + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 开始执行清算";
            if (txtStateQS.Text == "等待执行...")
            {
                this.txtStateQS.Text = this.txtStateCRJ.Text.Replace("等待执行...", msg);
            }
            else
            {
                this.txtStateQS.AppendText(Environment.NewLine + msg);
            }

            string[] fileinfo = lblQSWJXX.Text.Trim().Split('&');//文件名、文件大小、（卖-买）合计
            //发送1003接口参数
            Hashtable ht = new Hashtable();
            ht["FuncFlag"] = "1";//1代表清算
            ht["FileName"] = fileinfo[0].ToString();
            ht["FileSize"] = fileinfo[1].ToString();
            ht["QsZcAmount"] = (-Convert.ToDouble(fileinfo[2].ToString())).ToString("#0.00");//买-卖
            ht["FreezeAmount"] = "0.00";//冻结
            ht["UnfreezeAmount"] = "0.00";//解冻
            ht["SyZcAmount"] = "0.00";//损益

            delegateForThread tempDFT = new delegateForThread(RunQS_Invoke);
            RunThread rt = new RunThread(ht, tempDFT);
            Thread trd = new Thread(new ThreadStart(rt.RunQS));
            trd.IsBackground = true;
            trd.Start();
        }
        private void RunQS_Invoke(Hashtable OutPutHT)
        {
            try
            {
                //调用异步委托
                Invoke(new delegateForThreadShow(RunQS_Result), new Hashtable[] { OutPutHT });
            }
            catch (Exception ex)
            {
                StringOP.WriteLog("委托回调错误：" + ex.ToString());
            }
        }
        private void RunQS_Result(Hashtable OutPutHT)
        {
            this.txtStateQS.AppendText(Environment.NewLine + OutPutHT["执行状态"].ToString());
            this.txtStateQS.AppendText(Environment.NewLine + OutPutHT["详细描述"].ToString());

            if (OutPutHT["执行状态"].ToString().IndexOf("ERR") >= 0)
            {
                btnRunQS.Enabled = true;
            }

            if (OutPutHT["执行状态"].ToString().IndexOf("1001") >= 0)
            {
               // lblFailName.Text = OutPutHT["文件名"].ToString();
                lblQSZT.Text = OutPutHT["文件名"].ToString();
            }
            else if (OutPutHT["执行状态"].ToString().IndexOf("1007") >= 0)
            {
                lblDZBP.Text = OutPutHT["文件名"].ToString();
            }

           // if (OutPutHT["详细描述"].ToString().IndexOf("等待银行处理结果") >= 0)
            //{
                //btnBrkWatQS.Enabled = true;
           // }
        }
        //中断1005等待
        private void btnBrkWatQS_Click(object sender, EventArgs e)
        {
            //btnBrkWatQS.Enabled = false;
            //Program.state1005["状态"] = "人工中断清算";
            //Program.state1005["详情"] = "";
            //Program.state1005["类型"] = "";
        }
        #endregion

        #region //查询银行清算进度
        private void btnBankView_Click(object sender, EventArgs e)
        {
            string msg = Environment.NewLine + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            this.txtQSBank.AppendText(msg);


            Hashtable ht = new Hashtable();
            ht["type"] = "清算";//1代表清算
            ht["date"] = DateTime.Now.ToString("yyyyMMdd");

            delegateForThread tempDFT = new delegateForThread(BankView_Invoke);
            RunThread rt = new RunThread(ht, tempDFT);
            Thread trd = new Thread(new ThreadStart(rt.BankView));
            trd.IsBackground = true;
            trd.Start();
        }
        private void BankView_Invoke(Hashtable OutPutHT)
        {
            try
            {
                //调用异步委托
                Invoke(new delegateForThreadShow(BankView_Result), new Hashtable[] { OutPutHT });
            }
            catch (Exception ex)
            {
                StringOP.WriteLog("委托回调错误：" + ex.ToString());
            }
        }
        private void BankView_Result(Hashtable OutPutHT)
        {
            this.txtQSBank.AppendText(Environment.NewLine + OutPutHT["执行状态"].ToString());
            if (OutPutHT["详细描述"].ToString() != "")
            {
                this.txtQSBank.AppendText(Environment.NewLine + OutPutHT["详细描述"].ToString());
            }

            if (OutPutHT["执行状态"].ToString().IndexOf("取批量文件失败") >= 0 || OutPutHT["执行状态"].ToString().IndexOf("处理全部失败") >= 0)
            { //更新清算的执行状态
                //Program.state1005["状态"] = "清算结束";
                this.txtStateQS.AppendText(Environment.NewLine + "ERR，银行" + OutPutHT["执行状态"].ToString());
                this.txtStateQS.AppendText(Environment.NewLine + "请检查本地数据后重新生成清算文件并执行清算");
                btnRunQS.Enabled = true;
            }
            else if (OutPutHT["执行状态"].ToString().IndexOf("处理完全成功") >= 0)
            {
               // Program.state1005["状态"] = "清算结束";
                lblQSZT.Text = "银行清算成功";
                this.txtStateQS.AppendText(Environment.NewLine + "OK，银行" + OutPutHT["执行状态"].ToString());  
                btnRunQS.Enabled = true;
            }
        }
        #endregion

        #region //更新原始清算数据的状态
        private void btnUpdState_Click(object sender, EventArgs e)
        {
            //if (lblFailName.Text.Trim() == "清算失败文件名" && lblQSZT.Text.Trim() == "清算成功状态")
            //{
            //    MessageBox.Show(this, "暂无清算结果，无法执行此操作！");
            //    return;
            //}
            if (lblQSZT.Text == "清算状态")
            {
                MessageBox.Show(this, "暂无清算结果，无法执行此操作！");
                return;
            }

            string msg = Environment.NewLine + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  开始更新原始清算数据状态";
            if (txtStateQS.Text == "等待执行...")
            {
                this.txtStateQS.Text = this.txtStateCRJ.Text.Replace("等待执行...", msg);
            }
            else
            {
                this.txtStateQS.AppendText(Environment.NewLine + msg);
            }

            //发送1004接口参数
            Hashtable ht = new Hashtable();
            //ht["清算失败"] = lblFailName.Text.Trim();
            //ht["对账不平"] = lblDZBP.Text.Trim();
            //ht["清算成功"] = lblQSZT.Text.Trim();

            ht["清算状态"] = lblQSZT.Text.Trim();
            delegateForThread tempDFT = new delegateForThread(UpdState_Invoke);
            RunThread rt = new RunThread(ht, tempDFT);
            Thread trd = new Thread(new ThreadStart(rt.UpdState));
            trd.IsBackground = true;
            trd.Start();
        }
        private void UpdState_Invoke(Hashtable OutPutHT)
        {
            try
            {
                //调用异步委托
                Invoke(new delegateForThreadShow(UpdState_Result), new Hashtable[] { OutPutHT });
            }
            catch (Exception ex)
            {
                StringOP.WriteLog("委托回调错误：" + ex.ToString());
            }
        }
        private void UpdState_Result(Hashtable OutPutHT)
        {
            this.txtStateQS.AppendText(Environment.NewLine + OutPutHT["执行状态"].ToString());
            this.txtStateQS.AppendText(Environment.NewLine + OutPutHT["详细描述"].ToString());
            DataTable dt = (DataTable)OutPutHT["data"];
            if (dt != null)
            {
                if (OutPutHT["type"].ToString() == "清算失败")
                {
                    dgvQSSB.DataSource = dt;
                }
                else if (OutPutHT["type"].ToString() == "对账不平")
                {
                    dgvDZBP.DataSource = dt;
                }
            }
        }
        #endregion

        #region //保存对账不平数据
        private void btnSaveDZBP_Click(object sender, EventArgs e)
        {
            if (lblDZBP.Text.Trim() == "对账不平文件名")
            {
                MessageBox.Show(this, "暂无对账不平文件，无法执行此操作！");
                return;
            }

            string msg = Environment.NewLine + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  开始保存对账不平数据";
            if (txtStateQS.Text == "等待执行...")
            {
                this.txtStateQS.Text = this.txtStateCRJ.Text.Replace("等待执行...", msg);
            }
            else
            {
                this.txtStateQS.AppendText(Environment.NewLine + msg);
            }

            //发送1004接口参数
            Hashtable ht = new Hashtable();
            ht["对账不平"] = lblDZBP.Text.Trim();

            delegateForThread tempDFT = new delegateForThread(btnSaveDZBP_Invoke);
            RunThread rt = new RunThread(ht, tempDFT);
            Thread trd = new Thread(new ThreadStart(rt.SaveDZBP));
            trd.IsBackground = true;
            trd.Start();
        }
        private void btnSaveDZBP_Invoke(Hashtable OutPutHT)
        {
            try
            {
                //调用异步委托
                Invoke(new delegateForThreadShow(btnSaveDZBP_Result), new Hashtable[] { OutPutHT });
            }
            catch (Exception ex)
            {
                StringOP.WriteLog("委托回调错误：" + ex.ToString());
            }
        }
        private void btnSaveDZBP_Result(Hashtable OutPutHT)
        {
            this.txtStateQS.AppendText(Environment.NewLine + OutPutHT["执行状态"].ToString());
            this.txtStateQS.AppendText(Environment.NewLine + OutPutHT["详细描述"].ToString());
            DataTable dt = (DataTable)OutPutHT["data"];
            if (dt != null)
            {
                dgvDZBP.DataSource = dt;
            }
        }
        #endregion

        #region //导出数据
        private void btnExport_Click(object sender, EventArgs e)
        {
            switch(cbEptType.SelectedItem.ToString())
            {
                case "清算失败数据":
                    ExportQSSB();
                    break;
                case "对账不平数据":
                    ExportDZBP();
                    break;
                default:
                    break;
            }
        }
        //导出清算失败数据
        private void ExportQSSB()
        {
            if (dgvQSSB.DataSource == null)
            {
                MessageBox.Show("当前无清算失败数据！");
                return;
            }
            else
            {
                DataTable dt = new DataTable();
                dt = ((DataTable)dgvQSSB.DataSource).Copy();
                dt.TableName = "清算失败数据";
                DataSet ds = new DataSet();
                ds.Tables.Add(dt);
                string filename = "清算失败数据" + DateTime.Now.ToString("yyyyMMddHHmmss");
                cMyXls1.BeginExport(ds, filename);
            }
        }
        //导出对账不平数据
        private void ExportDZBP()
        {
            if (dgvDZBP.DataSource == null)
            {
                MessageBox.Show("当前无对账不平数据！");
                return;
            }
            else
            {
                DataTable dt = new DataTable();
                dt = ((DataTable)dgvDZBP.DataSource).Copy();
                dt.TableName = "对账不平数据";
                DataSet ds = new DataSet();
                ds.Tables.Add(dt);
                string filename = "对账不平数据" + DateTime.Now.ToString("yyyyMMddHHmmss");
                cMyXls1.BeginExport(ds, filename);
            }
        }
        #endregion

        #endregion

        #region //将文本框中定位在最后一行
        private void txtListenAll_TextChanged(object sender, EventArgs e)
        {
            txtListenAll.Select(txtListenAll.TextLength, txtListenAll.TextLength);
            txtListenAll.ScrollToCaret();
        }

        private void txtStateSign_TextChanged(object sender, EventArgs e)
        {
            txtStateSign.Select(txtStateSign.TextLength, txtStateSign.TextLength);
            txtStateSign.ScrollToCaret();
        }

        private void txtStateKXH_TextChanged(object sender, EventArgs e)
        {
            txtStateKXH.Select(txtStateKXH.TextLength, txtStateKXH.TextLength);
            txtStateKXH.ScrollToCaret();
        }
        private void txtStateCRJ_TextChanged(object sender, EventArgs e)
        {
            txtStateCRJ.Select(txtStateCRJ.TextLength, txtStateCRJ.TextLength);
            txtStateCRJ.ScrollToCaret();
        }
        private void txtStateQS_TextChanged(object sender, EventArgs e)
        {
            txtStateQS.Select(txtStateQS.TextLength, txtStateQS.TextLength);
            txtStateQS.ScrollToCaret();
        }
        private void txtQSBank_TextChanged(object sender, EventArgs e)
        {
            txtQSBank.Select(txtQSBank.TextLength, txtQSBank.TextLength);
            txtQSBank.ScrollToCaret();
        }
        private void txtKXHBank_TextChanged(object sender, EventArgs e)
        {
            txtKXHBank.Select(txtKXHBank.TextLength, txtKXHBank.TextLength);
            txtKXHBank.ScrollToCaret();
        }
        private void txtCRJBank_TextChanged(object sender, EventArgs e)
        {
            txtCRJBank.Select(txtCRJBank.TextLength, txtCRJBank.TextLength);
            txtCRJBank.ScrollToCaret();
        }
        #endregion

        //清空状态区域的内容
        private void btnClear_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            switch (btn.Name)
            {
                case "btnClearSign":
                    this.txtStateSign.Text = "等待执行...";
                    break;
                case "btnClearKXH":
                    this.txtStateKXH.Text = "等待执行...";
                    //this.dgvExpKXH.DataSource = null;
                    break;
                case "btnClearCRJ":
                    this.txtStateCRJ.Text = "等待执行...";
                    break;
                case "btnClearQS":
                    this.txtStateQS.Text = "等待执行...";
                    break;
                case "btnClearBS":
                    this.txtQSBank.Text = "";
                    break;
                case "btnClearBCRJ":
                    this.txtCRJBank.Text = "";
                    break;
                case "btnClearBKXH":
                    this.txtKXHBank.Text = "";
                    break;
                default:
                    break;
            }
        }

    }
}
