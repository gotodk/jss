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
    public partial class tihuodanPutIn : BasicForm
    {
        //淡出计时器
        System.Windows.Forms.Timer Timer_DC;

        ucXDTHD_B CXb;        

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

        public tihuodanPutIn(ucXDTHD_B CXbtemp)
        {
    
            InitializeComponent();
            CXb = CXbtemp;
            this.TitleYS = new int[] { 0, 0, -50 };

            //图标
            Icon ic = new Icon(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase+ @"\skin\ji.ico");
            this.Icon = ic;
        }
        #region//初始页面设置、获取数据
        //处理下拉框间距
        private void CB_DrawItem(object sender, DrawItemEventArgs e)
        {
            ComboBox CBthis = (ComboBox)sender;
            if (e.Index < 0)
            {
                return;
            }
            e.DrawBackground();
            e.DrawFocusRectangle();
            e.Graphics.DrawString(CBthis.Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds.X + 2, e.Bounds.Y + 3);
        }
        private void CXweizhongbiaoEdit_Load(object sender, EventArgs e)
        {
            //处理下拉框间距
            this.UCkplx1.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.CB_DrawItem);            
            
            LLLNumber.Text = CXb.number; //取得中标定标的编号 
            lblHTBH.Text = CXb.htbh;//合同编号
            Lkthsl.Text = CXb.kthsl.ToString();//可提货数量
            Ldanjia.Text = CXb.dbjg.ToString();//定标价格
            UCjjpl.Text = CXb.jjpl.ToString();//经济批量
            uctxtFPTT.Text = CXb.jyfmc.ToString();//交易方名称
            uctxtSHQU.Text = CXb.yyddshqu.ToString();//原预订单收货区域

            if (CXb.htzt.ToString() == "即时")
            {
                UCthbs.Enabled = false;
                Lbcthsl.Text = CXb.kthsl.ToString();//本次提货数量
                if (Convert.ToInt32(CXb.kthsl.ToString()) % Convert.ToInt32(CXb.jjpl.ToString()) > 0)
                {
                    //本次提货经济批量数
                    UCthbs.Text = (Convert.ToInt32(CXb.kthsl.ToString()) / Convert.ToInt32(CXb.jjpl.ToString()) + 1).ToString();
                }
                else
                {
                    UCthbs.Text = (Convert.ToInt32(CXb.kthsl.ToString()) / Convert.ToInt32(CXb.jjpl.ToString())).ToString();
                }
                UCthje.Text = (Convert.ToInt32(CXb.kthsl.ToString()) * Convert.ToDouble(CXb.dbjg.ToString())).ToString("#0.00");//本次需冻结货款金额
            }
            else
            {
                UCthbs.Enabled = true;
            }

            //开启线程，通过合同编号和买家角色编号，获取一些数据。
            panelUCedit.Enabled = false;
            resetloadLocation(BBedit, PBload);
            Hashtable ht = new Hashtable();
            ht["DLYX"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString();            
            delegateForThread dft = new delegateForThread(ShowThreadResult_GetFPXX);
            NewDataControl.GetTHDFPXX RCthd = new NewDataControl.GetTHDFPXX(ht, dft);
            Thread trd = new Thread(new ThreadStart(RCthd.BeginRun));
            trd.IsBackground = true;
            trd.Start();

            if (PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["注册类别"].ToString() == "自然人")
            {
                UCkplx1.Items.Clear();
                UCkplx1.Items.Add("增值税普通发票");
                UCkplx1.SelectedIndex = 0;
                UCkplx1.Enabled = false;
            }
            else
            {
                UCkplx1.Items.Clear();
                UCkplx1.Items.Add("请选择发票");
                UCkplx1.Items.Add("增值税普通发票");
                UCkplx1.Items.Add("增值税专用发票");
                UCkplx1.SelectedIndex = 0;
                UCkplx1.Enabled = true;
            }
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
            panelUCedit.Enabled = true;
            PBload.Visible = false;
            //显示执行结果
            DataSet returnds = (DataSet)returnHT["返回值"];
            if (returnds == null || returnds.Tables[0].Rows.Count < 1)
            {
                UCkplx1.SelectedIndex = 0;
                return;
            }
            else
            {
                panelUCedit.Enabled = true;
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

                        UCshdz.Text = returnds.Tables["主表"].Rows[0]["收货详细地址"].ToString();
                        uctxtSHRXM.Text = returnds.Tables["主表"].Rows[0]["收货人姓名"].ToString();
                        uctxtLXFS.Text = returnds.Tables["主表"].Rows[0]["收货人联系方式"].ToString();
                        if (PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["注册类别"].ToString() == "单位")
                        {
                            switch (returnds.Tables["主表"].Rows[0]["发票类型"].ToString())
                            {
                                case "增值税普通发票":
                                    UCkplx1.SelectedIndex = 1;
                                    panelKPXX.Visible = false;
                                    break;
                                case "增值税专用发票":
                                    UCkplx1.SelectedIndex = 2;
                                    panelKPXX.Visible = true;
                                    break;
                                default:
                                    UCkplx1.SelectedIndex = 0;
                                    panelKPXX.Visible = false;
                                    break;
                            }
                        }
                        else
                        {
                            UCkplx1.SelectedIndex = 0;
                            panelKPXX.Visible = false;
                        }
                       
                        uctxtFPSH.Text = returnds.Tables["主表"].Rows[0]["税号"].ToString();
                        uctxtKHYH.Text = returnds.Tables["主表"].Rows[0]["开户银行"].ToString();
                        uctxtYHZH.Text = returnds.Tables["主表"].Rows[0]["银行账号"].ToString();
                        if (returnds.Tables["主表"].Rows[0]["单位地址"].ToString() != "")
                        {
                            uctxtDWDZ.Text = returnds.Tables["主表"].Rows[0]["单位地址"].ToString();
                            uctxtDWDZ.Enabled = false;
                        }
                        else
                        {
                            uctxtDWDZ.Enabled = true;
                        }

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
            SaveTHD("只验证钱");           
        }
        private void SaveTHD(string cz)
        {
            #region//基本验证
            if (UCthbs.Text.Trim() == "")
            {
                showAlertY("本次提货经济批量数不能为空！");
                return;
            }
            if (Convert.ToInt32(UCthbs.Text.Trim()) == 0)
            {
                showAlertY("本次提货经济批量数不能为零！");
                return;
            }
            //if (UCthje.Text.Trim() == "")
            //{
            //    showAlertY("提货金额不能为空。");
            //    return;
            //}
            if (UCshdz.Text.Trim() == "")
            {
                showAlertY("收货详细地址不能为空！");
                return;
            }
            if (uctxtSHRXM.Text.Trim() == "")
            {
                showAlertY("收货人姓名不能为空！");
                return;
            }
            if (uctxtLXFS.Text.Trim() == "")
            {
                showAlertY("收货联系方式不能为空！");
                return;
            }
            if (UCkplx1.Text.Trim() == "" || UCkplx1.Text.Trim() == "请选择发票")
            {
                showAlertY("开票类型不能为空！");
                return;
            }
            if (panelKPXX.Visible == true)
            {
                if (uctxtKHYH.Text.Trim() == "")
                {
                    showAlertY("开户银行不能为空！");
                    return;
                }
                if (uctxtYHZH.Text.Trim() == "")
                {
                    showAlertY("银行账号不能为空！");
                    return;
                }
                if (uctxtDWDZ.Text.Trim() == "")
                {
                    showAlertY("单位地址不能为空！");
                    return;
                }
            }
            if (UCthje.Text.Length>10)
            {
                showAlertY("本次需冻结货款金额过大，不能下达提货单！");
                return;            
            }
            #endregion

            #region//生成提交参数
            string BUYjsbh = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["买家角色编号"].ToString();
            string DLYX = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString();
            string ZHLX = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["结算账户类型"].ToString();
            DataSet dsinfosave = new DataSet();
            Hashtable htcs = new Hashtable();
            htcs["操作"] = cz.Trim();            
            htcs["登陆邮箱"] = DLYX;           
            htcs["中标定标信息表编号"] = LLLNumber.Text;           
            htcs["提货的经济批量数"] = UCthbs.Text;
            htcs["提货数量"] = Lbcthsl.Text;
            htcs["收货详细地址"] = UCshdz.Text.Trim();
            htcs["收货人姓名"] = uctxtSHRXM.Text.Trim();
            htcs["收货人联系方式"] = uctxtLXFS.Text.Trim();            
            htcs["发票类型"] = UCkplx1.SelectedItem;            
            htcs["税号"] = UCkplx1.Text.Trim() == "增值税专用发票" ? uctxtFPSH.Text.Trim() : "";
            htcs["开户银行"] = UCkplx1.Text.Trim() == "增值税专用发票" ? uctxtKHYH.Text.Trim():"" ;
            htcs["银行账号"] = UCkplx1.Text.Trim() == "增值税专用发票" ? uctxtYHZH.Text.Trim() : "";
            htcs["单位地址"] = UCkplx1.Text.Trim() == "增值税专用发票" ?uctxtDWDZ.Text.Trim():"";

            #region//2014.07.02 zhouli作废--新框架变更传递参数
            //htcs["买家角色编号"] = BUYjsbh;
            //htcs["结算账户类型"] = ZHLX;
            //htcs["合同编号"] = lblHTBH.Text;
            //htcs["中标单价"] = Ldanjia.Text;
            //htcs["经济批量"] = UCjjpl.Text;
            //htcs["冻结货款金额"] = UCthje.Text.Trim() == "" ? "0.00" : UCthje.Text.Trim();
            //htcs["发票抬头"] = uctxtFPTT.Text.Trim();
            #endregion

            #endregion

            //开启线程，通过合同编号和买家角色编号，获取一些数据。
            panelUCedit.Enabled = false;
            resetloadLocation(BBedit, PBload);
            DataTable dtcs = StringOP.GetDataTableFormHashtable(htcs);
            dsinfosave.Tables.Add(dtcs);
            delegateForThread dft = new delegateForThread(ShowThreadResult_save);
            NewDataControl.SaveTHD RCthd = new SaveTHD(dsinfosave, dft);
            Thread trd = new Thread(new ThreadStart(RCthd.BeginRun));
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

                        panelUCedit.Enabled = true;
                        PBload.Visible = false;

                        //重新绑定数据                    
                        CXb.GetData();
                        this.Close();

                        return;
                    case "err":
                        ArrayList Almsg7 = new ArrayList();
                        Almsg7.Add("");
                        Almsg7.Add(returnds.Tables[0].Rows[0]["提示文本"].ToString());
                        FormAlertMessage FRSE7 = new FormAlertMessage("仅确定", "其他", "系统忙", Almsg7);
                        FRSE7.ShowDialog();
                        panelUCedit.Enabled = true;
                        PBload.Visible = false;
                        return;
                    case "onlymoney":
                        ArrayList Almsg3 = new ArrayList();
                        Almsg3.Add("");
                        Almsg3.Add(returnds.Tables[0].Rows[0]["提示文本"].ToString());
                        FormAlertMessage FRSE3 = new FormAlertMessage("确定取消", "问号", "处理成功", Almsg3);
                        if (FRSE3.ShowDialog() == DialogResult.Yes)
                        {
                            SaveTHD("实际操作");
                        }
                        else
                        {
                            panelUCedit.Enabled = true;
                            PBload.Visible = false;
                        }
                        return;
                    default:
                        ArrayList Almsg4 = new ArrayList();
                        Almsg4.Add("");
                        FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "其他", "提交提货单失败", Almsg4);
                        FRSE4.ShowDialog();
                        panelUCedit.Enabled = true;
                        PBload.Visible = false;
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
                panelUCedit.Enabled = true;
                PBload.Visible = false;
                this.Close();
                return;
            }
            

        }
        #endregion
        //发票类型
        private void UCkplx_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (UCkplx1.SelectedIndex == 1)
            {
                panelKPXX.Visible = false;
            }
            else if (UCkplx1.SelectedIndex ==2)
            {
                panelKPXX.Visible = true;
            }            
        }
        
        //收货地址
        private void UCshdz_Leave(object sender, EventArgs e)
        {
            if (UCshdz.Text != "" && UCshdz.Text == uctxtSHQU.Text)
            {
                UCshdz.Text = "请不要重复输入收货区域";
                UCshdz.ForeColor = System.Drawing.SystemColors.GrayText;
            }
            else if (UCshdz.Text != "")
            {
                UCshdz.ForeColor = System.Drawing.Color.Black;
            }
        }
        //提货经济批量
        private void UCthbs_TextChanged(object sender, EventArgs e)
        {
            //计算本次提货数量
            if (UCjjpl.Text.Trim() != "" && Lkthsl.Text.Trim() != "")
            {
                Int64 kthsl = Convert.ToInt64(Lkthsl.Text);//可提货数量
                Int64 jjpl = Convert.ToInt64(UCjjpl.Text);//经济批量
                Int64 thbs = Convert.ToInt64(UCthbs.Text.ToString().Trim() == "" ? "0" : UCthbs.Text.ToString().Trim());//提货笔数
                if (kthsl < jjpl) //可提货量不足一个经济批量
                {
                    Lbcthsl.Text = kthsl.ToString();
                }
                else if (kthsl < thbs * jjpl) //可提货量不足以支撑录入的提货笔数
                {
                    Lbcthsl.Text = kthsl.ToString();
                }
                else if (kthsl - thbs * jjpl < jjpl)//录入提货笔数后，剩余可提货数量小于经济批量
                {
                    Lbcthsl.Text = kthsl.ToString();
                }
                else
                {
                    Lbcthsl.Text = (thbs * jjpl).ToString();
                }
            }

            //显示本次提货金额
            if (Ldanjia.Text.Trim() != "" && Lbcthsl.Text.Trim() != "")
            {
                UCthje.Text = (Convert.ToDouble(Ldanjia.Text.Trim()) * Convert.ToInt64(Lbcthsl.Text.Trim())).ToString("#0.00");
            }
        }

    }
}
