using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Com.Seezt.Skins;
using System.Collections;
using System.Runtime.InteropServices;
using 客户端主程序.NewDataControl;
using System.Threading;
using 客户端主程序.Support;
using 客户端主程序.DataControl;


namespace 客户端主程序.SubForm.NewCenterForm.SUBUC.HWSF
{
    public partial class SCFHD : BasicForm
    {
        //淡出计时器
        System.Windows.Forms.Timer Timer_DC;

        ucSCFHD_C CXb;        

        #region 窗体淡出，窗体最小化最大化等特殊控制，所有窗体都有这个玩意

        /// <summary>
        /// 窗体的Load事件中的淡出处理
        /// </summary>
        private void Init_one_show()
        {
            //设置双缓冲
            this.SetStyle(ControlStyles.DoubleBuffer |
            ControlStyles.UserPaint |
            ControlStyles.AllPaintingInWmPaint,
            true);
            this.UpdateStyles();

            //加载淡出计时器
            Timer_DC = new System.Windows.Forms.Timer();
            Timer_DC.Interval = Program.DC_Interval;
            this.Timer_DC.Tick += new System.EventHandler(this.Timer_DC_Tick);
            //淡出效果
            MaxDC();
        }

        /// <summary>
        /// 显示窗体时启动淡出
        /// </summary>
        private void MaxDC()
        {
            this.Opacity = 0;
            Timer_DC.Enabled = true;
        }

        //淡出显示窗体，绕过窗体闪烁问题
        private void Timer_DC_Tick(object sender, EventArgs e)
        {
            this.Opacity = this.Opacity + Program.DC_step;
            if (!Program.DC_open)
            {
                this.Opacity = 1;
            }
            if (this.Opacity >= 1)
            {
                Timer_DC.Enabled = false;
            }
        }

        //允许任务栏最小化
        protected override CreateParams CreateParams
        {
            get
            {
                const int WS_MINIMIZEBOX = 0x00020000;  // Winuser.h中定义
                CreateParams cp = base.CreateParams;
                cp.Style = cp.Style | WS_MINIMIZEBOX;   // 允许最小化操作
                return cp;
            }
        }


        private int WM_SYSCOMMAND = 0x112;
        private long SC_MAXIMIZE = 0xF030;
        private long SC_MINIMIZE = 0xF020;
        private long SC_CLOSE = 0xF060;
        private long SC_NORMAL = 0xF120;
        private FormWindowState SF = FormWindowState.Normal;
        /// <summary>
        /// 重绘窗体的状态
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref   Message m)
        {
            if (this.WindowState != FormWindowState.Minimized)
            {
                SF = this.WindowState;
            }
            if (m.Msg == WM_SYSCOMMAND)
            {
                if (m.WParam.ToInt64() == SC_MAXIMIZE)
                {
                    MaxDC();
                    this.WindowState = FormWindowState.Maximized;
                    return;
                }
                if (m.WParam.ToInt64() == SC_MINIMIZE)
                {
                    this.WindowState = FormWindowState.Minimized;
                    return;
                }
                if (m.WParam.ToInt64() == SC_NORMAL)
                {
                    MaxDC();
                    this.WindowState = SF;
                    return;
                }
                if (m.WParam.ToInt64() == SC_CLOSE)
                {
                    this.Close();
                    return;
                }
            }
            base.WndProc(ref   m);
        }

        #endregion

        public SCFHD(ucSCFHD_C fhd)
        {
    
            InitializeComponent();
            CXb = fhd;
            this.TitleYS = new int[] { 0, 0, -50 };

            //图标
            Icon ic = new Icon(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase+ @"\skin\ji.ico");
            this.Icon = ic;
        }
        #region//初始页面设置、获取数据
        //设置readbutton
        private void SetRD()
        {
            //是否有特殊要求
            switch(rdTSYQY.Checked)
            {
                case true:
                    rdTSYQN.Checked = false;
                    panelSM.Visible = true;
                    break;
                default:
                    rdTSYQN.Checked = true;
                    panelSM.Visible = false;
                    break;
            }
            //发票是否随货同行
            switch (rdFPSHTXY.Checked)
            {
                case true:
                    rdFPSHTXN.Checked = false;
                    break;
                default:
                    rdFPSHTXN.Checked = true;
                    break;
            }
            if (CXb.SelCXFHTime.ToString() != "")
            {
                UCfphm.Text = CXb.FPHM.ToString();
                UCfphm.Enabled = false;
                lblFPSFSHTX.Text = CXb.FPSFSHTX.ToString();
                panelSMNR.Visible = true;
                panelFPSHTX1.Visible = false;
                panelFPSHTX2.Visible = true;
                if (CXb.FPSFSHTX.ToString() == "是")
                {
                    rdFPSHTXY.Checked = true;                    
                    rdFPSHTXN.Checked = false;                    
                }
                else
                {
                    rdFPSHTXY.Checked = false;
                    rdFPSHTXN.Checked = true;
                }
            }
            else
            {
                panelSMNR.Visible = false;
                panelFPSHTX1.Visible = true;
                panelFPSHTX2.Visible = false;
            }

        }
        private void CXweizhongbiaoEdit_Load(object sender, EventArgs e)
        {
            SetRD();
            //开启线程，通过合同编号和买家角色编号，获取一些数据。
            flowLayoutPanel1.Enabled = false;
            resetloadLocation(BBedit, PBload);
            Hashtable ht = new Hashtable();
            ht["DLYX"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString();            
            delegateForThread dft = new delegateForThread(ShowThreadResult_GetFPXX);
            NewDataControl.GetTHDFPXX RCthd = new NewDataControl.GetTHDFPXX(ht, dft);
            Thread trd = new Thread(new ThreadStart(RCthd.BeginRun));
            trd.IsBackground = true;
            trd.Start();
        }
        //显示线程处理结果的函数(用于处理线程返回数据)
        private void ShowThreadResult_GetFPXX(Hashtable returnHT)
        {
            try
            {
                //调用异步委托
                Invoke(new delegateForThreadShow(ShowThreadResult_GetFPXX_Invoke), new Hashtable[] { returnHT });
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("委托回调错误：" + ex.ToString());
            }
        }
        //处理非线程创建的控件
        private void ShowThreadResult_GetFPXX_Invoke(Hashtable returnHT)
        {
            flowLayoutPanel1.Enabled = true;
            PBload.Visible = false;
            //显示执行结果
            DataSet returnds = (DataSet)returnHT["返回值"];
            if (returnds == null || returnds.Tables[0].Rows.Count < 1)
            {
                //UCkplx.SelectedIndex = 0;
                return;
            }
            else
            {
                flowLayoutPanel1.Enabled = true;
                PBload.Visible = false;
            }
            switch (returnds.Tables["返回值单条"].Rows[0]["执行结果"].ToString())
            {
                case "err":
                    ArrayList Almsg3 = new ArrayList();
                    Almsg3.Add("");
                    Almsg3.Add(returnds.Tables["返回值单条"].Rows[0]["提示文本"].ToString());
                    FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "其他", "提示", Almsg3);
                    DialogResult dr = FRSE3.ShowDialog();
                    break;
                case "ok":
                    if (returnds.Tables["主表"].Rows.Count > 0)
                    {

                        //UCshdz.Text = returnds.Tables["主表"].Rows[0]["收货详细地址"].ToString();
                        //uctxtSHRXM.Text = returnds.Tables["主表"].Rows[0]["收货人姓名"].ToString();
                        //uctxtLXFS.Text = returnds.Tables["主表"].Rows[0]["收货人联系方式"].ToString();
                        //switch (returnds.Tables["主表"].Rows[0]["发票类型"].ToString())
                        //{
                        //    case "增值税普通发票":
                        //        UCkplx.SelectedIndex = 0;
                        //        panelKPXX.Visible = false;
                        //        break;
                        //    case "增值税专用发票":
                        //        UCkplx.SelectedIndex = 1;
                        //        panelKPXX.Visible = true;
                        //        break;
                        //    default:
                        //        UCkplx.SelectedIndex = 0;
                        //        panelKPXX.Visible = false;
                        //        break;
                        //}
                        //uctxtFPSH.Text = returnds.Tables["主表"].Rows[0]["税号"].ToString();
                        //uctxtKHYH.Text = returnds.Tables["主表"].Rows[0]["开户银行"].ToString();
                        //uctxtYHZH.Text = returnds.Tables["主表"].Rows[0]["银行账号"].ToString();
                        //if (returnds.Tables["主表"].Rows[0]["单位地址"].ToString() != "")
                        //{
                        //    uctxtDWDZ.Text = returnds.Tables["主表"].Rows[0]["单位地址"].ToString();
                        //    uctxtDWDZ.Enabled = false;
                        //}
                        //else
                        //{
                        //    uctxtDWDZ.Enabled = true;
                        //}

                    }
                    break;
                default:
                    ArrayList Almsg = new ArrayList();
                    Almsg.Add("");
                    Almsg.Add("系统繁忙！请稍后重试...");
                    FormAlertMessage FRSE = new FormAlertMessage("仅确定", "其他", "提示", Almsg);
                    DialogResult dr1 = FRSE.ShowDialog();
                    break;
            }
        }
        //获取控件相对于窗体的绝对位置
        private Point LocationOnClient(Control c)
        {
            Point retval = new Point(0, 0);
            for (; c.Parent != null; c = c.Parent)
            { retval.Offset(c.Location); }
            return retval;
        }
        /// <summary>
        /// 重设进度条位置，放到按钮旁边
        /// </summary>
        /// <param name="BB">参考提交按钮</param>
        /// <param name="PB">要移动的进度条</param>
        private void resetloadLocation(BasicButton BB, PictureBox PB)
        {
            Point pt = LocationOnClient(BB);
            PB.BringToFront();//置于顶层
            PB.Location = new Point(pt.X + BB.Width + 1, pt.Y + (BB.Height / 2) - (PB.Height / 2));
            PB.Visible = true;
        }
        #endregion
        /// <summary>
        /// 显示错误提示
        /// </summary>
        /// <param name="err"></param>
        private void showAlertY(string err)
        {
            ArrayList Almsg3 = new ArrayList();
            Almsg3.Add("");
            Almsg3.Add(err);
            FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "其他", "预订单信息提交", Almsg3);
            FRSE3.ShowDialog();
        }
        #region//提交提货单
        private void BBedit_Click(object sender, EventArgs e)
        {
            SaveTHD("只验证");           
        }
        private void SaveTHD(string cz)
        {
            #region//基本验证
            if (panelSM.Visible == true)
            {
                if (string.IsNullOrEmpty(txtYQSM.Text.ToString().Trim()))
                {
                    showAlertY("要求说明不能为空！");
                    return;
                }
            }
            //if (panel3.Visible == true && panel4.Visible == true)
            //{
            //    if (UCfphm.Text.Trim() == "")
            //    {
            //        showAlertY("发票号码不能为空！");
            //        return;
            //    }
            //}
            if (UCfphm.Text.Trim() == "")
            {
                showAlertY("发票号码不能为空！");
                return;
            }
            #endregion

            #region//生成提交参数
            string BUYjsbh = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["卖家角色编号"].ToString();
            DataSet dsinfosave = new DataSet();
            Hashtable htcs = new Hashtable();
            htcs["操作"] = cz.Trim();
            if (CXb.FPYJTime.ToString() != "")
            {
                htcs["状态"] = "重新发货";
            }
            else
            {
                htcs["状态"] = "";
            }
            htcs["卖家角色编号"] = BUYjsbh;
            htcs["是否有物流特殊要求"] = rdTSYQY.Checked==true?"是":"否";
            htcs["物流特殊要求说明"] = txtYQSM.Text.ToString();
            htcs["发票号码"] = UCfphm.Text.ToString();
            htcs["发票是否随货同行"] = rdFPSHTXY.Checked == true ? "是" : "否";
            htcs["提货单编号"] = CXb.thddh.TrimStart('T');
            htcs["中标定标信息表编号"] = CXb.ZBDBXXBBH;
            #endregion

            DataTable dtcs = StringOP.GetDataTableFormHashtable(htcs);
            dsinfosave.Tables.Add(dtcs);
            //开启线程，通过合同编号和买家角色编号，获取一些数据。
            flowLayoutPanel1.Enabled = false;
            resetloadLocation(BBedit, PBload);
            delegateForThread dft = new delegateForThread(ShowThreadResult_save);
            NewDataControl.SCFHD fhd = new NewDataControl.SCFHD(dsinfosave, dft);            
            Thread trd = new Thread(new ThreadStart(fhd.BeginRun));
            trd.IsBackground = true;
            trd.Start();
        }
        //显示线程处理结果的函数(用于处理线程返回数据)
        private void ShowThreadResult_save(Hashtable returnHT)
        {
            try
            {
                //调用异步委托
                Invoke(new delegateForThreadShow(ShowThreadResult_save_Invoke), new Hashtable[] { returnHT });
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("委托回调错误：" + ex.ToString());
            }
        }
        //处理非线程创建的控件
        private void ShowThreadResult_save_Invoke(Hashtable returnHT)
        {
            flowLayoutPanel1.Enabled = true;
            PBload.Visible = false;
            //显示执行结果
            DataSet returnds = returnHT["返回值"] as DataSet;
            if (returnds != null && returnds.Tables[0].Rows.Count > 0)
            {
                switch (returnds.Tables[0].Rows[0]["执行结果"].ToString())
                {
                    case "ok":
                        //给出表单提交成功的提示
                        ArrayList Almsg = new ArrayList();
                        Almsg.Add("");
                        Almsg.Add(returnds.Tables[0].Rows[0]["提示文本"].ToString());
                        FormAlertMessage FRSE = new FormAlertMessage("仅确定", "其他", "处理成功", Almsg);
                        FRSE.ShowDialog();

                        //重新绑定数据                    
                        CXb.GetData(null);
                        this.Close();

                        return;
                    case "err":
                        ArrayList Almsg7 = new ArrayList();
                        Almsg7.Add("");
                        Almsg7.Add(returnds.Tables[0].Rows[0]["提示文本"].ToString());
                        FormAlertMessage FRSE7 = new FormAlertMessage("仅确定", "其他", "系统忙", Almsg7);
                        FRSE7.ShowDialog();
                        return;
                    case "OnlyYZ":
                        ArrayList Almsg3 = new ArrayList();
                        Almsg3.Add("");
                        Almsg3.Add(returnds.Tables[0].Rows[0]["提示文本"].ToString());
                        FormAlertMessage FRSE3 = new FormAlertMessage("确定取消", "问号", "处理成功", Almsg3);
                        if (FRSE3.ShowDialog() == DialogResult.Yes)
                        {
                            SaveTHD("实际操作");
                        }
                        return;
                    case "OpIsInvalid":
                        //给出表单提交成功的提示
                        Almsg = new ArrayList();
                        Almsg.Add("");
                        Almsg.Add(returnds.Tables[0].Rows[0]["提示文本"].ToString());
                        FRSE = new FormAlertMessage("仅确定", "其他", "处理成功", Almsg);
                        FRSE.ShowDialog();

                        //重新绑定数据                    
                        CXb.GetData(null);
                        this.Close();
                        return;

                    default:
                        ArrayList Almsg4 = new ArrayList();
                        Almsg4.Add("");
                        FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "其他", "提交提货单失败", Almsg4);
                        FRSE4.ShowDialog();

                        return;
                }
            }
            else
            {
                ArrayList Almsg4 = new ArrayList();
                Almsg4.Add("");
                Almsg4.Add("系统繁忙，请稍后重试...");
                FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "其他", "提交提货单失败", Almsg4);
                FRSE4.ShowDialog();
                this.Close();
                return;
            }
            

        }
        #endregion
        
        //是否有特殊要求选框"是"
        private void rdTSYQY_CheckedChanged(object sender, bool Checked)
        {
            if (rdTSYQY.Checked == true)
            {
                panelSM.Visible = true;
            }
            else
            {
                panelSM.Visible = false;
            }
        }

    }
}
