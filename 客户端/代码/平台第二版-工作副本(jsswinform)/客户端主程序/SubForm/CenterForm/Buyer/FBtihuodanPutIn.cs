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
using 客户端主程序.DataControl;
using System.Threading;
using 客户端主程序.Support;


namespace 客户端主程序.SubForm.CenterForm.Buyer
{
    public partial class FBtihuodanPutIn : BasicForm
    {
        //淡出计时器
        System.Windows.Forms.Timer Timer_DC;

        FBtihuodan CXb;
        /// <summary>
        /// 收货地址
        /// </summary>
        Hashtable SHDZ = new Hashtable();

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

        public FBtihuodanPutIn(FBtihuodan CXbtemp)
        {
    
            InitializeComponent();
            CXb = CXbtemp;
            this.TitleYS = new int[] { 0, 0, -50 };

            //图标
            Icon ic = new Icon(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase+ @"\skin\ji.ico");
            this.Icon = ic;
        }

        private void CXweizhongbiaoEdit_Load(object sender, EventArgs e)
        {
            LLLNumber.Text = CXb.Number; //取得合同编号
            string BUYjsbh = PublicDS.PublisDsUser.Tables[0].Rows[0]["买家角色编号"].ToString();

            //开启线程，通过合同编号和买家角色编号，获取一些数据。
            panelUCedit.Enabled = false;
            resetloadLocation(BBedit, PBload);
            delegateForThread dft = new delegateForThread(ShowThreadResult_info);
            DataControl.RunThreadClassTHD RCthd = new DataControl.RunThreadClassTHD(LLLNumber.Text, BUYjsbh, null, dft);
            Thread trd = new Thread(new ThreadStart(RCthd.BeginRun_info));
            trd.IsBackground = true;
            trd.Start();
        }



        //显示线程处理结果的函数(用于处理线程返回数据)
        private void ShowThreadResult_info(Hashtable returnHT)
        {
            try
            {
                //调用异步委托
                Invoke(new delegateForThreadShow(ShowThreadResult_info_Invoke), new Hashtable[] { returnHT });
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("委托回调错误：" + ex.ToString());
            }
        }
        //处理非线程创建的控件
        private void ShowThreadResult_info_Invoke(Hashtable returnHT)
        {
            //显示执行结果
            DataSet returnds = (DataSet)returnHT["执行结果"];
            if (returnds == null || returnds.Tables[0].Rows.Count < 1)
            {
                return;
            }
            else
            {
                panelUCedit.Enabled = true;
                PBload.Visible = false;
            }

            //有返回数据后，进行显示处理
            Ldanjia.Text = returnds.Tables[0].Rows[0]["SELTBJG"].ToString();
            Lkthsl.Text = (Convert.ToInt64(returnds.Tables[0].Rows[0]["ZBSL"]) - Convert.ToInt64(returnds.Tables[0].Rows[0]["BUYYTHSL"])).ToString(); //可提货数量
            UCjjpl.Text = returnds.Tables[0].Rows[0]["JJPL"].ToString();//提货经济批量
            if (returnds.Tables[0].Rows[0]["是否存在默认地址"].ToString() == "是")
            {
                SHDZ["SHDWMC"] = returnds.Tables[0].Rows[0]["默认收货单位名称"].ToString();
                SHDZ["SHRXM"] = returnds.Tables[0].Rows[0]["默认收货人姓名"].ToString();
                SHDZ["SZSF"] = returnds.Tables[0].Rows[0]["默认省"].ToString();
                SHDZ["SZDS"] = returnds.Tables[0].Rows[0]["默认市"].ToString();
                SHDZ["SZQX"] = returnds.Tables[0].Rows[0]["默认区"].ToString();
                SHDZ["XXDZ"] = returnds.Tables[0].Rows[0]["默认详细地址"].ToString();
                SHDZ["YZBM"] = returnds.Tables[0].Rows[0]["默认邮政编码"].ToString();
                SHDZ["LXDH"] = returnds.Tables[0].Rows[0]["默认联系电话"].ToString();

                string showstr = "";
                showstr = showstr + "收货单位名称：" + SHDZ["SHDWMC"].ToString() + "\r\n收货人姓名" + SHDZ["SHRXM"].ToString() + "\r\n";
                showstr = showstr + "区域：" + SHDZ["SZSF"].ToString() + SHDZ["SZDS"].ToString() + SHDZ["SZQX"].ToString() + "\r\n";
                showstr = showstr + "详细地址：" + SHDZ["XXDZ"].ToString() + "\r\n";
                showstr = showstr + "邮政编码：" + SHDZ["YZBM"].ToString() + "\r\n联系电话：" + SHDZ["LXDH"].ToString() + "\r\n";
                UCshdz.Text = showstr;//显示的默认收货地址

                LLLxzqy.Text = returnds.Tables[0].Rows[0]["MJSHQY"].ToString();
            }
            else
            {
                LLLxzqy.Text = returnds.Tables[0].Rows[0]["MJSHQY"].ToString();
                UCshdz.Text = "";//显示的默认收货地址
                SHDZ = null; //用于传递参数的默认收货地址
            }
            UCkplx.Text = returnds.Tables[0].Rows[0]["开票类型"].ToString(); //开票类型
            UCkpxx.Text = returnds.Tables[0].Rows[0]["开票信息"].ToString();//开票信息
        }


        /// <summary>
        /// 重设进度条位置，放到按钮旁边
        /// </summary>
        /// <param name="BB">参考提交按钮</param>
        /// <param name="PB">要移动的进度条</param>
        private void resetloadLocation(BasicButton BB, PictureBox PB)
        {
            PB.Location = new Point(BB.Location.X + BB.Width + 30, BB.Location.Y +23 );
            PB.Visible = true;
        }

        /// <summary>
        /// 显示错误提示
        /// </summary>
        /// <param name="err"></param>
        private void showAlertY(string err)
        {
            ArrayList Almsg3 = new ArrayList();
            Almsg3.Add("");
            Almsg3.Add(err);
            FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "错误", "预订单信息提交", Almsg3);
            FRSE3.ShowDialog();
        }

        private void BBedit_Click(object sender, EventArgs e)
        {
            ArrayList Almsg3 = new ArrayList();
            Almsg3.Add("");
            Almsg3.Add("您确定要提交提货单吗？");
            FormAlertMessage FRSE3 = new FormAlertMessage("确定取消", "问号", "xxx", Almsg3);
            DialogResult dr = FRSE3.ShowDialog();
            if (dr != DialogResult.Yes)
            {
                return;
            }


            //基本验证
            if (UCthbs.Text.Trim() == "")
            {
                showAlertY("本次提货笔数不能为空。");
                return;
            }
            if (Lbcthsl.Text.Trim() == "0" || Lbcthsl.Text.Trim() == "")
            {
                showAlertY("本次提货数量不能为零。");
                return;
            }
            if (UCthje.Text.Trim() == "")
            {
                showAlertY("提货金额不能为空。");
                return;
            }
            if (UCshdz.Text.Trim() == "")
            {
                showAlertY("收货地址不能为空。");
                return;
            }
            if (UCkplx.Text.Trim() == "")
            {
                showAlertY("开票类型不能为空。");
                return;
            }
            if (UCkpxx.Text.Trim() == "")
            {
                showAlertY("开票信息不能为空。");
                return;
            }
            


            //生成提交参数
            string BUYjsbh = PublicDS.PublisDsUser.Tables[0].Rows[0]["买家角色编号"].ToString();
            DataSet dsinfosave = new DataSet();
            Hashtable htcs = new Hashtable();
            htcs["买家角色编号"] = BUYjsbh;
            htcs["合同编号"] = LLLNumber.Text;
            htcs["本次提货量"] = Lbcthsl.Text;
            htcs["SHDWMC"] = SHDZ["SHDWMC"];
            htcs["SHRXM"] = SHDZ["SHRXM"];
            htcs["SZSF"] = SHDZ["SZSF"];
            htcs["SZDS"] = SHDZ["SZDS"];
            htcs["SZQX"] = SHDZ["SZQX"];
            htcs["XXDZ"] = SHDZ["XXDZ"];
            htcs["YZBM"] = SHDZ["YZBM"];
            htcs["LXDH"] = SHDZ["LXDH"];
            htcs["开票类型"] = UCkplx.Text;
            htcs["开票信息"] = UCkpxx.Text;
            htcs["备注"] = UCbz.Text;

            //验证限制区域
            string selectxzqy = "|" + htcs["SZSF"].ToString() + htcs["SZDS"].ToString() + "|";
            if (LLLxzqy.Text != selectxzqy)
            {
                showAlertY("此提货单收货地址，仅限[" + LLLxzqy.Text.Replace("|","") + "]。");
                return;
            }

            DataTable dtcs = StringOP.GetDataTableFormHashtable(htcs);
            dsinfosave.Tables.Add(dtcs);
            //开启线程，通过合同编号和买家角色编号，获取一些数据。
            panelUCedit.Enabled = false;
            resetloadLocation(BBedit, PBload);
            delegateForThread dft = new delegateForThread(ShowThreadResult_save);
            
            DataControl.RunThreadClassTHD RCthd = new DataControl.RunThreadClassTHD(LLLNumber.Text, BUYjsbh, dsinfosave, dft);
            Thread trd = new Thread(new ThreadStart(RCthd.BeginRun_save));
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
            panelUCedit.Enabled = true;
            PBload.Visible = false;
            //显示执行结果
            string returnds = returnHT["执行结果"].ToString();
            switch (returnds)
            {
                case "ok":

                    //给出表单提交成功的提示
                    ArrayList Almsg3 = new ArrayList();
                    Almsg3.Add("");
                    Almsg3.Add("下达提货单成功！");
                    FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "叹号", "处理成功", Almsg3);
                    FRSE3.ShowDialog();

                    //重新绑定数据
                    CXb.GetData(null);
                    this.Close();

                    break;
                case "系统繁忙":
                    ArrayList Almsg7 = new ArrayList();
                    Almsg7.Add("");
                    Almsg7.Add("抱歉，系统繁忙，请稍后再试……");
                    FormAlertMessage FRSE7 = new FormAlertMessage("仅确定", "错误", "系统忙", Almsg7);
                    FRSE7.ShowDialog();
                    break;
                default:
                    ArrayList Almsg4 = new ArrayList();
                    Almsg4.Add("");
                    Almsg4.Add(returnds);// 这里是提交失败的提示，可能是程序出错，可能是条件不足等
                    FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "错误", "提交xxx失败", Almsg4);
                    FRSE4.ShowDialog();

                    break;
            }

        }


        private void lblSelectSHDZ_Click(object sender, EventArgs e)
        {
            Hashtable ht = new Hashtable();
            if (SHDZ["SZSF"] != null && SHDZ["SZDS"]!=null)
            {
                ht["SZSF"] = SHDZ["SZSF"];
                ht["SZDS"] = SHDZ["SZDS"];
            }
            delegateForThread DFT = new delegateForThread(Returnsomethings);
            SelectSHDZ SS = new SelectSHDZ(ht, DFT);
            SS.ShowDialog();
        }
        private void Returnsomethings(Hashtable return_ht)
        {
            //
            string showstr = "";
            showstr = showstr + "收货单位名称：" + return_ht["SHDWMC"].ToString() + "\r\n收货人姓名" + return_ht["SHRXM"].ToString() + "\r\n";
            showstr = showstr + "区域：" + return_ht["SZSF"].ToString() + return_ht["SZDS"].ToString() + return_ht["SZQX"].ToString() + "\r\n";
            showstr = showstr + "详细地址：" + return_ht["XXDZ"].ToString() + "\r\n";
            showstr = showstr + "邮政编码：" + return_ht["YZBM"].ToString() + "\r\n联系电话：" + return_ht["LXDH"].ToString() + "\r\n";
            UCshdz.Text = showstr;

            SHDZ = return_ht;

        }

        private void UCthsl_TextChanged(object sender, EventArgs e)
        {
            //计算本次提货数量
            if (UCjjpl.Text.Trim() != "" && Lkthsl.Text.Trim() != "" && UCthbs.Text.Trim() != "")
            {
               Int64 kthsl = Convert.ToInt64(Lkthsl.Text);//可提货数量
               Int64 jjpl = Convert.ToInt64(UCjjpl.Text);//经济批量
               Int64 thbs = Convert.ToInt64(UCthbs.Text);//提货笔数
               if (kthsl < jjpl) //可提货量不足一个经济批量
               {
                   Lbcthsl.Text = kthsl.ToString();
               }
               else if (kthsl < thbs * jjpl) //可提货量不足以支撑录入的提货笔数
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
                UCthje.Text = (Convert.ToDouble(Ldanjia.Text.Trim()) * Convert.ToInt64(Lbcthsl.Text.Trim())).ToString();
            }
        }

    }
}
