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
using 客户端主程序.DataControl;
using 客户端主程序.SubForm.NewCenterForm.GZZD;

namespace 客户端主程序.SubForm
{
    public partial class fmTBDXG : BasicForm
    {
        //淡出计时器
        System.Windows.Forms.Timer Timer_DC;

        /// <summary>
        /// 某一条商品信息
        /// </summary>
        Hashtable ht_Main = null;
        /// <summary>
        /// 卖家选择的区域信息
        /// </summary>
        Hashtable ht_QY = null;
        /// <summary>
        /// 卖家商品的资质信息（路径）
        /// </summary>
        Hashtable ht_ZZ = null;
        /// <summary>
        /// 该商品需要上传的资质信息
        /// </summary>
        string str_ZZ = null;

        /// <summary>
        /// 卖家订金冻结比例
        /// </summary>
        double MJDJDJBL = Convert.ToDouble(PublicDS.PublisDsData.Tables["动态参数"].Rows[0]["MJTBBZJBL"].ToString());
        /// <summary>
        /// 卖家投标保证金最小值
        /// </summary>
        double MJTBBZJZXZ = Convert.ToDouble(PublicDS.PublisDsData.Tables["动态参数"].Rows[0]["MJTBBZJZXZ"].ToString());

        /// <summary>
        /// 当前登录帐号关联经纪人的管理机构
        /// </summary>
        string GLJJRPTGLJG = null;




        /// <summary>
        /// 对勾选择后的回调指针
        /// </summary>
        delegateForThread dftForParent;
     

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

        public fmTBDXG(delegateForThread dftForParent_temp,bool isHasInfo)
        {
            dftForParent = dftForParent_temp;
    
            InitializeComponent();
            //图标
            Icon ic = new Icon(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase+ @"\skin\ji.ico");
            this.Icon = ic;
            //设置窗体显示在屏幕中央
            this.StartPosition = FormStartPosition.CenterScreen;
            //不在任务栏显示此信息
            this.ShowInTaskbar = false;
            #region 获取这个关联经纪人的管理机构
            string DLYX = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString();
            Hashtable ht_GLJG = new Hashtable();
            ht_GLJG["DLYX"] = DLYX;
            G_B gg = new G_B(ht_GLJG, new delegateForThread(GetGLJJRGLJG));
            Thread trdgg = new Thread(new ThreadStart(gg.GetGLJJRGLJG));
            trdgg.IsBackground = true;
            trdgg.Start();
            #endregion
            ucCityList1.initdefault();
            panel4.Visible = false;
            label28.Visible = false;
            label27.Visible = false;
            label29.Visible = false;
            label26.Visible = false;
        }

        public fmTBDXG(string Number)
        {
            InitializeComponent();
            label32.Text = "8、" + Number + "投标单已撤销。";
            label32.Visible = false;
            lbNumber.Text = Number;//隐藏label，是用来传递单号的
            lbNumber.Visible = false;
            ucCityList1.initdefault();
            //图标
            Icon ic = new Icon(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\skin\ji.ico");
            this.Icon = ic;
            //设置窗体显示在屏幕中央
            this.StartPosition = FormStartPosition.CenterScreen;
            //不在任务栏显示此信息
            this.ShowInTaskbar = false;
            panel2.Enabled = false;
            PBload.Visible = true;

            //验证用户是否已冻结账号 

            #region 撤销这个订单
            Hashtable ht = new Hashtable();
            ht["Number"] = Number;
            CX(ht);//开调！
            #endregion

            #region 获取这个关联经纪人的管理机构
            string DLYX = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString();
            Hashtable ht_GLJG = new Hashtable();
            ht_GLJG["DLYX"] = DLYX;
            G_B gg = new G_B(ht_GLJG, new delegateForThread(GetGLJJRGLJG));
            Thread trdgg = new Thread(new ThreadStart(gg.GetGLJJRGLJG));
            trdgg.IsBackground = true;
            trdgg.Start();
            #endregion

            panel4.Visible = false;
            label28.Visible = false;
            label27.Visible = false;
            label29.Visible = false;
            label26.Visible = false;

        }
        #region 获取当前帐号所关联经纪人的管理机构
        private void GetGLJJRGLJG(Hashtable OutPutHT)
        {
            try { Invoke(new delegateForThreadShow(GetGLJJRGLJG_Invoke), new Hashtable[] { OutPutHT }); }
            catch (Exception ex) { Support.StringOP.WriteLog("委托回调错误：" + ex.ToString()); }
        }
        private void GetGLJJRGLJG_Invoke(Hashtable OutPutHT)
        {
            DataSet dsreturn = (DataSet)OutPutHT["结果"];
            //如果当前帐户未成功获得关联经纪人的信息
            if (dsreturn.Tables["返回值单条"].Rows[0]["执行结果"].ToString() == "ok" && dsreturn.Tables.Count == 2)
            {
                GLJJRPTGLJG = dsreturn.Tables["JJR"].Rows[0][0].ToString();
                // MessageBox.Show(GLJJRPTGLJG);
            }
            if (dsreturn.Tables["返回值单条"].Rows[0]["执行结果"].ToString() == "err")
            {
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add(dsreturn.Tables["返回值单条"].Rows[0]["提示文本"].ToString());
                FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "", al);
                fm.ShowDialog();
            }
            if (dsreturn == null)
            {
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add("系统繁忙，请稍后再试！");
                FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "", al);
                fm.ShowDialog();
            }
        }
        #endregion
        #region 投标单撤销
        private void CX(Hashtable InPutHT)
        {
            G_S g = new G_S(InPutHT, new delegateForThread(CX_Begin));
            Thread trd = new Thread(new ThreadStart(g.TBD_CX));
            trd.IsBackground = true;
            trd.Start();
        }
        private void CX_Begin(Hashtable OutPutHT)
        {
            try { Invoke(new delegateForThreadShow(CX_Invoke), new Hashtable[] { OutPutHT }); }
            catch (Exception ex) { Support.StringOP.WriteLog("委托回调错误：" + ex.ToString()); }
        }
        private void CX_Invoke(Hashtable OutPutHT)
        {
            panel2.Enabled = true;
            PBload.Visible = false;
            DataSet dsreturn = (DataSet)OutPutHT["结果"];
            if (dsreturn != null)
            {
                string zt = dsreturn.Tables["返回值单条"].Rows[0]["执行结果"].ToString();
                string showstr = dsreturn.Tables["返回值单条"].Rows[0]["提示文本"].ToString();
                bool showstr2 = Convert.ToBoolean(dsreturn.Tables["返回值单条"].Rows[0]["附件信息2"]);
                if (zt == "err")
                {
                    if (showstr.IndexOf("|||") > 0)
                    {
                        string[] strAry = showstr.Split(new string[] { "|||" }, StringSplitOptions.RemoveEmptyEntries);
                        ArrayList als = new ArrayList();
                        als.Add("");
                        als.Add(strAry[0]);
                        als.Add(strAry[1]);
                        if (strAry.Length > 2)
                            als.Add(strAry[2]);
                        FormAlertMessage fms = new FormAlertMessage("仅确定", "其他", "中国商品批发交易平台", als);
                        fms.ShowDialog();
                        this.Close();
                        this.Dispose();
                        return;
                    }


                    label32.Visible = false;
                    ArrayList al = new ArrayList();
                    al.Add("");
                    al.Add(showstr);
                    FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "中国商品批发交易平台", al);
                    fm.ShowDialog();
                    this.Close();
                    this.Dispose();
                    panel4.Visible = false;
                    label28.Visible = false;
                    label27.Visible = false;
                    label29.Visible = false;
                    label26.Visible = false;
                }
                if (zt == "ok")
                {
                    if (!showstr2)
                    {
                        ArrayList als = new ArrayList();
                        als.Add("");
                        als.Add("您所投标的商品所需资质已发生变更，该投标单无法提交。请重新申请该商品的出售资格或选择其他可出售商品。");
                        this.Hide();
                        panel4.Visible = false;
                        FormAlertMessage fms = new FormAlertMessage("仅确定", "其他", "中国商品批发交易平台", als);
                        fms.ShowDialog();
                        this.Close();
                        this.Dispose();
                        return;
                    }


                    labNsl.Text = dsreturn.Tables["返回值单条"].Rows[0]["附件信息5"].ToString();
                    label32.Visible = true;
                    panel2.Enabled = false;
                    PBload.Visible = true;
                    #region 获取被撤销的投标单信息
                    string Number = lbNumber.Text;//单号
                    Hashtable ht_Number = new Hashtable();
                    ht_Number["Number"] = Number;
                    G_S g = new G_S(ht_Number, new delegateForThread(XG));
                    Thread trd = new Thread(new ThreadStart(g.TBD_XG));
                    trd.IsBackground = true;
                    trd.Start();
                    #endregion

                }
            }
            else
            {
                label32.Visible = false;
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add("系统繁忙，请稍后再试！");
                FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "中国商品批发交易平台", al);
                fm.ShowDialog();
                reset();
            }

        }
        #endregion
        #region 投标单修改，带出原始数据
        private void XG(Hashtable OutPutHT)
        {
            try { Invoke(new delegateForThreadShow(XG_Invoke), new Hashtable[] { OutPutHT }); }
            catch (Exception ex) { Support.StringOP.WriteLog("委托回调错误：" + ex.ToString()); }
        }
        private void XG_Invoke(Hashtable OutPutHT)
        {
            panel2.Enabled = true;
            PBload.Visible = false;
            DataSet dsreturn = (DataSet)OutPutHT["结果"];
            if (dsreturn == null)
            {
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add("系统繁忙！");
                FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "中国商品批发交易平台", al);
                fm.ShowDialog();
            }
            else
            {
                string zt = dsreturn.Tables["返回值单条"].Rows[0]["执行结果"].ToString();
                string showstr = dsreturn.Tables["返回值单条"].Rows[0]["提示文本"].ToString();
                if (zt == "err")
                {
                    ArrayList al = new ArrayList();
                    al.Add("");
                    al.Add(showstr);
                    FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "中国商品批发交易平台", al);
                    fm.ShowDialog();
                }
                if (zt == "ok")
                {
                    panel4.Visible = true;
                    label28.Visible = true;
                    label27.Visible = true;
                    label29.Visible = true;
                    label26.Visible = true;


                    DataTable dt = dsreturn.Tables["SPXX"]; 
                    dt.Rows[0]["TBNSL"]=   this.labNsl.Text;
                    this.txtHTQX.Text = dt.Rows[0]["HTQX"].ToString();//合同期限
                    this.txtSPMC.Text = dt.Rows[0]["SPMC"].ToString();//商品名称
                    this.txtGG.Text = dt.Rows[0]["GG"].ToString();//规格
                    this.txtJJDW.Text = dt.Rows[0]["JJDW"].ToString();//计价单位
                    this.txtPTZDSDJJPL.Text = dt.Rows[0]["JJPL"].ToString();//平台设定最大经济批量
                    this.txtJJPL.Text = dt.Rows[0]["MJSDJJPL"].ToString();//卖家设定的经济批量
                    this.txtTPNGL.Text = dt.Rows[0]["TBNSL"].ToString();//投标拟售量
                    this.txtTBJG.Text = dt.Rows[0]["TBJG"].ToString();//投标价格
                    this.txtTBJE.Text = (Convert.ToDouble(dt.Rows[0]["TBNSL"]) * Convert.ToDouble(dt.Rows[0]["TBJG"])).ToString();//投标金额
                    this.txtDJTBBZJ.Text = (Convert.ToDouble(dt.Rows[0]["TBNSL"]) * Convert.ToDouble(dt.Rows[0]["TBJG"]) * MJDJDJBL) > MJTBBZJZXZ ? (Convert.ToDouble(dt.Rows[0]["TBNSL"]) * Convert.ToDouble(dt.Rows[0]["TBJG"]) * MJDJDJBL).ToString("0.00") : MJTBBZJZXZ.ToString("0.00");//冻结的投标保证金
                    //if (dt.Rows[0]["SLZM"] != DBNull.Value && dt.Rows[0]["SLZM"] != null && dt.Rows[0]["SLZM"].ToString() != "")
                    //{
                    //    ListView lv = uCshangchuan1.UpItem;
                    //    lv.Items.Clear();
                    //    lv.Items.Add(new ListViewItem(new string[]{"", dt.Rows[0]["SLZM"].ToString(), "", ""}));
                    //}

                    this.ucTextBox_slv.Text = dt.Rows[0]["SLZM"].ToString();
                    // wyh 2013.09.06 add SPCD 
                    //string[] strs = dt.Rows[0]["SPCD"].ToString().Split('|');
                    ////ht["省市区"] = ucCityList1.SelectedItem[0] + "|" + ucCityList1.SelectedItem[1] + "|" + ucCityList1.SelectedItem[2];
                    //this.ucCityList1.SelectedItem = new string[] { strs[0].ToString().Trim(), strs[1].ToString().Trim(), strs[2].ToString().Trim() };

                    if (!dt.Rows[0]["SPCD"].ToString().Trim().Equals(""))
                    {
                        string[] strs = dt.Rows[0]["SPCD"].ToString().Split('|');
                        if (strs.Length == 3)
                        {
                            //ht["省市区"] = ucCityList1.SelectedItem[0] + "|" + ucCityList1.SelectedItem[1] + "|" + ucCityList1.SelectedItem[2];
                            this.ucCityList1.SelectedItem = new string[] { strs[0].ToString().Trim(), strs[1].ToString().Trim(), strs[2].ToString().Trim() };
                        }
                    }



                    ht_Main = new Hashtable();
                    ht_Main["商品编号"] = dt.Rows[0]["SPBH"].ToString();//商品编号
                    ht_Main["商品名称"] = dt.Rows[0]["SPMC"].ToString();//商品编号
                    ht_Main["规格"] = dt.Rows[0]["GG"].ToString();//商品编号
                    ht_Main["合同期限"] = dt.Rows[0]["HTQX"].ToString();//商品编号
                    ht_Main["计价单位"] = dt.Rows[0]["JJDW"].ToString();//商品编号
                    ht_Main["平台批量"] = dt.Rows[0]["JJPL"].ToString();//商品编号
                    ht_Main["最低价格"] = dt.Rows[0]["ZDJG"].ToString();//商品编号
                    ht_Main["卖家批量"] = dt.Rows[0]["MJJJPL"].ToString();//商品编号
                    ht_Main["上传资质"] = dt.Rows[0]["SCZZYQ"].ToString();//该商品需要上传的资质
                    str_ZZ = ht_Main["上传资质"].ToString();
                    if (str_ZZ == null || str_ZZ.Trim() == "" || str_ZZ.Trim() == "|")
                    {
                        btnSCZL.Text = "查 看 资 质";
                    }
                    ht_QY = new Hashtable();
                    ht_QY["发货区域"] = dt.Rows[0]["GHQY"].ToString();//绑定发货区域

                    ht_ZZ = new Hashtable();
                    ht_ZZ["质量标准与证明"] = dt.Rows[0]["ZLBZYZM"].ToString();
                    ht_ZZ["产品检测报告"] = dt.Rows[0]["CPJCBG"].ToString();
                    ht_ZZ["品管总负责人法律承诺书"] = dt.Rows[0]["PGZFZRFLCNS"].ToString();
                    ht_ZZ["法定代表人承诺书"] = dt.Rows[0]["FDDBRCNS"].ToString();
                    ht_ZZ["售后服务规定与承诺"] = dt.Rows[0]["SHFWGDYCN"].ToString();
                    ht_ZZ["产品送检授权书"] = dt.Rows[0]["CPSJSQS"].ToString();


                    this.btnSelectQY.Text = "查       看";

                    Hashtable ht_QTZZ = new Hashtable();
                    ht_QTZZ["资质1"] = dt.Rows[0]["ZZ01"].ToString().Trim() == "" ? "|" : dt.Rows[0]["ZZ01"].ToString().Trim();
                    ht_QTZZ["资质2"] = dt.Rows[0]["ZZ02"].ToString().Trim() == "" ? null : dt.Rows[0]["ZZ02"].ToString().Trim();
                    ht_QTZZ["资质3"] = dt.Rows[0]["ZZ03"].ToString().Trim() == "" ? null : dt.Rows[0]["ZZ03"].ToString().Trim();
                    ht_QTZZ["资质4"] = dt.Rows[0]["ZZ04"].ToString().Trim() == "" ? null : dt.Rows[0]["ZZ04"].ToString().Trim();
                    ht_QTZZ["资质5"] = dt.Rows[0]["ZZ05"].ToString().Trim() == "" ? null : dt.Rows[0]["ZZ05"].ToString().Trim();
                    ht_QTZZ["资质6"] = dt.Rows[0]["ZZ06"].ToString().Trim() == "" ? null : dt.Rows[0]["ZZ06"].ToString().Trim();
                    ht_QTZZ["资质7"] = dt.Rows[0]["ZZ07"].ToString().Trim() == "" ? null : dt.Rows[0]["ZZ07"].ToString().Trim();
                    ht_QTZZ["资质8"] = dt.Rows[0]["ZZ08"].ToString().Trim() == "" ? null : dt.Rows[0]["ZZ08"].ToString().Trim();
                    ht_QTZZ["资质9"] = dt.Rows[0]["ZZ09"].ToString().Trim() == "" ? null : dt.Rows[0]["ZZ09"].ToString().Trim();
                    ht_QTZZ["资质10"] = dt.Rows[0]["ZZ10"].ToString().Trim() == "" ? null : dt.Rows[0]["ZZ10"].ToString().Trim();
                    ht_ZZ["其它资质"] = null;

                    #region 展示当前最低投标价格
                    if (ht_Main["最低价格"].ToString() == "0.00")
                    {
                        label28.Width = 170;
                        label28.Text = "当前无卖方参与此商品的投标";
                        label28.Visible = false;
                        label27.Visible = false;
                    }
                    else
                    {
                        label28.Width = 134;
                        label28.Text = "当前卖方最低投标价：";
                        label28.Visible = true;
                        label27.Visible = true;
                        label27.Text = ht_Main["最低价格"].ToString() + "元";
                    }
                    #endregion
                    //label28 label25
                    #region 展示当前最大的经济批量
                    if (Convert.ToInt32(ht_Main["卖家批量"]) == 0)
                    {
                        label29.Text = "平台设定的该商品最大经济批量：" + ht_Main["平台批量"].ToString() + ht_Main["计价单位"].ToString();
                        label29.Width = 300;
                        

                        //label29.Text = "当前平台中的最大经济批量：";
                        //label26.Text = ht_Main["平台批量"].ToString() + ht_Main["计价单位"].ToString();
                        label29.Visible = true;
                        label26.Visible = false;
                    }
                    else
                    {
                        label29.Text = "当前卖方中的最大经济批量：";
                        label26.Text = ht_Main["卖家批量"].ToString() + ht_Main["计价单位"].ToString();
                        label29.Visible = true;
                        label26.Visible = true;
                    }
                    #endregion
                }
            }
        }
        #endregion
        /// <summary>
        ///确认
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {

        }

        //开启一个测试线程
        private void SRT_demo_Run(Hashtable InPutHT)
        {
            G_S g = new G_S(InPutHT, new delegateForThread(SRT_demo));
            Thread trd = new Thread(new ThreadStart(g.SetTBD));
            trd.IsBackground = true;
            trd.Start();
        }
        //显示线程处理结果的函数(用于处理线程返回数据),调用异步委托
        private void SRT_demo(Hashtable OutPutHT)
        {
            try { Invoke(new delegateForThreadShow(SRT_demo_Invoke), new Hashtable[] { OutPutHT }); }
            catch (Exception ex) { Support.StringOP.WriteLog("委托回调错误：" + ex.ToString()); }
        }
        //处理非线程创建的控件
        private void SRT_demo_Invoke(Hashtable OutPutHT)
        {
            //重新开放提交区域,并滚动条强制置顶
            panel1.Enabled = true;
            panelTJQ.Enabled = true;
            PBload.Visible = false;
            UPUP();


            //返回值后的处理
            DataSet dsreturn = (DataSet)OutPutHT["结果"];

            string zt = dsreturn.Tables["返回值单条"].Rows[0]["执行结果"].ToString();
            string showstr = dsreturn.Tables["返回值单条"].Rows[0]["提示文本"].ToString();
            if (zt == "err")
            {
                if (showstr.IndexOf("一经提交") > 0)
                {
                    ArrayList al = new ArrayList();
                    al.Add("");
                    al.Add("本人和本单位再次确认本次所提供、上传的各项资料与信息均真实有效，并为此承担相应法律责任；");
                    al.Add(showstr);
                    FormAlertMessage fm = new FormAlertMessage("确定取消", "问号", "中国商品批发交易平台", al);
                    DialogResult boxresult = fm.ShowDialog();
                    if (boxresult == DialogResult.Yes)
                    {
                        //真正提交业务单据
                        #region 提交预订单
                        Hashtable ht = new Hashtable();
                        panel1.Enabled = false;
                        panelTJQ.Enabled = false;
                        PBload.Visible = true;
                        ht["FLAG"] = "提交";
                        ht["DLYX"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString();
                        ht["JSZHLX"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["结算账户类型"].ToString();
                        ht["MJJSBH"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["卖家角色编号"].ToString();
                        ht["GLJJRYX"] = PublicDS.PublisDsUser.Tables["关联信息"].Rows[0]["关联经纪人登陆账号"].ToString();
                        ht["GLJJRYHM"] = PublicDS.PublisDsUser.Tables["关联信息"].Rows[0]["关联经纪人用户名"].ToString();
                        ht["GLJJRJSBH"] = PublicDS.PublisDsUser.Tables["关联信息"].Rows[0]["关联经纪人编号"].ToString();
                        ht["GLJJRPTGLJG"] = GLJJRPTGLJG;//关联经纪人平台管理机构

                        //ht_Main
                        ht["SPBH"] = ht_Main["商品编号"];
                        ht["SPMC"] = ht_Main["商品名称"];
                        ht["GG"] = ht_Main["规格"];
                        ht["JJDW"] = ht_Main["计价单位"];
                        ht["PTSDZDJJPL"] = ht_Main["平台批量"];
                        ht["HTQX"] = ht_Main["合同期限"];
                        ht["MJSDJJPL"] = this.txtJJPL.Text;//卖家设定的经济批量
                        ht["GHQY"] = ht_QY["发货区域"].ToString();//供货区域
                        ht["TBNSL"] = this.txtTPNGL.Text;//投标拟售量
                        ht["TBJG"] = txtTBJG.Text;//投标价格
                        ht["TBJE"] = txtTBJE.Text;//投标金额
                        ht["MJTBBZJBL"] = MJDJDJBL;//卖家投标保证金比率
                        ht["MJTBBZJZXZ"] = MJTBBZJZXZ;//卖家的那个最小值。。
                        ht["DJTBBZJ"] = txtDJTBBZJ.Text;//冻结的投标保证金
                        ht["ZLBZYZM"] = ht_ZZ["质量标准与证明"];
                        ht["CPJCBG"] = ht_ZZ["产品检测报告"];
                        ht["PGZFZRFLCNS"] = ht_ZZ["品管总负责人法律承诺书"];
                        ht["FDDBRCNS"] = ht_ZZ["法定代表人承诺书"];
                        ht["SHFWGDYCN"] = ht_ZZ["售后服务规定与承诺"];
                        ht["CPSJSQS"] = ht_ZZ["产品送检授权书"];
                        ht["其他资质"] = ht_ZZ["其它资质"];//这个是以哈希表的形式存的
                        ht["省市区"] = ucCityList1.SelectedItem[0] + "|" + ucCityList1.SelectedItem[1] + "|" + ucCityList1.SelectedItem[2];
                        ht["SLZM"] = this.ucTextBox_slv.Text.Trim();//this.uCshangchuan1.UpItem.Items[0].SubItems[1].Text;//税率证明
                        panelTJQ.Enabled = false;
                        PBload.Visible = true;
                        //演示新标准
                        SRT_demo_Run(ht);
                        #endregion
                    }
                }
                else
                {
                    if (showstr.IndexOf("投标的商品正处于冷静期") > 0)
                    {
                        string[] strAy = showstr.Split(new char[] { '。' }, StringSplitOptions.RemoveEmptyEntries);
                        ArrayList al = new ArrayList();
                        al.Add("");
                        al.Add(strAy[0] + "。");
                        al.Add(strAy[1] + "。");
                        FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "中国商品批发交易平台", al);
                        fm.ShowDialog();
                        return;
                    }
                    if (showstr.IndexOf("该投标单的投标保证金差额为") > 0)
                    {
                        string[] strAy = showstr.Split(new char[] { '。' }, StringSplitOptions.RemoveEmptyEntries);
                        ArrayList al = new ArrayList();
                        al.Add("");
                        al.Add(strAy[0] + "。");
                        al.Add(strAy[1] + "。");
                        FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "中国商品批发交易平台", al);
                        fm.ShowDialog();
                        return;
                    }
                    if (showstr.IndexOf("|||") > 0)
                    {
                        string[] strAry = showstr.Split(new string[] { "|||" }, StringSplitOptions.RemoveEmptyEntries);
                        ArrayList al = new ArrayList();
                        al.Add("");
                        al.Add(strAry[0]);
                        al.Add(strAry[1]);
                        if (strAry.Length > 2)
                            al.Add(strAry[2]);
                        FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "中国商品批发交易平台", al);
                        fm.ShowDialog();
                        return;
                    }

                    ArrayList alMain = new ArrayList();
                    alMain.Add("");
                    alMain.Add(showstr);
                    FormAlertMessage fmMain = new FormAlertMessage("仅确定", "其他", "中国商品批发交易平台", alMain);
                    fmMain.ShowDialog();
                }
            }
            else if (zt == "ok")
            {
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add(showstr);
                FormAlertMessage fm = new FormAlertMessage("仅确定", "对号", "中国商品批发交易平台", al);
                fm.ShowDialog();
                this.Close();
                this.Dispose();
            }
 

        }
        /// <summary>
        /// 重置表单
        /// </summary>
        private void reset()
        {
            fmTBDXG f = new fmTBDXG(null,false);
            f.Dock = DockStyle.Fill;//铺满
            f.AutoScroll = true;//出现滚动条
            f.BackColor = Color.AliceBlue;

            //清理控件
            Panel P = (Panel)this.Parent;
            P.Controls.Clear();
            P.Controls.Add(f); //加入到某一个panel中
            f.Show();//显示出来
            //将滚动条置顶
            UPUP();
        }



        /// <summary>
        /// 页面滚动条强制置顶
        /// </summary>
        private void UPUP()
        {
            TextBox tb = new TextBox();
            this.Controls.Add(tb);
            tb.Location = new Point(-5000, -5000);
            tb.TabIndex = 0;
            tb.Focus();
            //this.Controls.Remove(tb);

        }

        private void ucSPMR_B_Load(object sender, EventArgs e)
        {
            //2013.12.06 wyh add 鼠标略过显示文字
            toolTip1.AutoPopDelay = 30000;
            toolTip1.InitialDelay = 500;
            toolTip1.ReshowDelay = 0;
            toolTip1.ShowAlways = true;
           // string Strtext = "1、一个商品一个轮次的竞标中只有一个最低价标的中标，价格相同时投标时间早的优先，中标买方收货区域将在该标的供货区域范围内。\n2、卖方设定的“经济批量”不得高于“平台设定的最大经济批量”；合同期限为“三个月”或“一年”的，“投标拟售量”不得低于“平台设定的最\n 大经济批量”的10倍；平台设定的最大经济批量”的10倍；合同期限为“即时”的，“投标拟售量”不得低于“平台设定的最大经济批量”。\n3、投标单下达的同时系统将自动冻结投标金额的0.5‰（最低不少于1000元/次）作为投标保证金；该投标保证金将在投标单撤销或定标后\n予以解冻；如未履行定标程序，\n后予以解冻；如未履行定标程序，投标保证金将被扣罚，补偿给各中标买方；合同期限为“即时“的投标单，\n可能会一部分中标（不低于卖方设定的一个经济批量），剩余部分将自动转入下一次交易撮合\n低于卖方设定的一个经济批量），剩余部分将\n自动转入下一次交易撮合（若剩余部分数量低于卖方设定的一个经济批量，则自动撤销）。\n4、按有关协议和交易规定，投标单一旦中标，即\n为交易双方正式订立《电子购货合同》。\n5、为保证快速履约，合同期限为“即时”的投标单中标后，卖方须在72小时内自主冻结履约保证金予以定标。\n6、卖方在履行交货时，须按照买方要求开具增值税专用发票或增值税普通发票（非一般纳税人单位须向税务机关申请代开）。\n  ";
            string Strtext = "1、一个商品一个轮次的竞标中只有一个最低价标的中标，价格相同时投标时间早的优先，中标买方收货区域将在该标的供货区域范围内。\n2、卖方设定的“经济批量”不得高于“平台设定的最大经济批量”；合同期限为“三个月”或“一年”的，“投标拟售量”不得低于“平台设定\n    的最大经济批量”的10倍；平台设定的最大经济批量”的10倍；合同期限为“即时”的，“投标拟售量”不得低于“平台设定的最大经济批量”。\n3、投标单下达的同时系统将自动冻结投标金额的0.5‰（最低不少于1000元/次）作为投标保证金；该投标保证金将在投标单撤销或定标后\n予以解冻；如未履行定标程序，投标保证金将被扣罚，补偿给各中标买方；合同期限为“即时“的投标单，可能会一部分中标（不低于卖方\n设定的一个经济批量），剩余部分将自动转入下一次交易撮合低于卖方设定的一个经济批量），剩余部分将自动转入下一次交易撮合\n（若剩余部分数量低于卖方设定的一个经济批量，则自动撤销）。\n4、按有关协议和交易规定，投标单一旦中标，即为交易双方正式订立《电子购货合同》。\n5、为保证快速履约，合同期限为“即时”的投标单中标后，卖方须在72小时内自主冻结履约保证金予以定标。\n6、卖方在履行交货时，须按照买方要求开具增值税专用发票或增值税普通发票（非一般纳税人单位须向税务机关申请代开）。\n  ";
            toolTip1.SetToolTip(this.label34, Strtext);
            toolTip1.SetToolTip(this.pictureBox1, Strtext);
            // this.toolTip1.Show(Strtext, this.label33);
            this.toolTip1.ToolTipTitle = "卖出须知";
        }
        private void Lselect_Click(object sender, EventArgs e)
        {
            fmNMRSP fm = new fmNMRSP(new delegateForThread(TanChuang_demo));
            fm.ShowDialog();
        }

       /// <summary>
       /// 绑定弹窗处理函数
       /// </summary>
       /// <param name="OutPutHT"></param>
        private void TanChuang_demo(Hashtable OutPutHT)
        {
            try { Invoke(new delegateForThreadShow(TanChuang_demo_Invoke), new Hashtable[] { OutPutHT }); }
            catch (Exception ex) { Support.StringOP.WriteLog("委托回调错误：" + ex.ToString()); }
        }
        /// <summary>
        /// 处理非线程创建的控件
        /// </summary>
        /// <param name="OutPutHT"></param>
        private void TanChuang_demo_Invoke(Hashtable OutPutHT)
        {
            ht_Main = OutPutHT;
            this.txtHTQX.Text = ht_Main["合同期限"].ToString();
            this.txtSPMC.Text = ht_Main["商品名称"].ToString();
            this.txtGG.Text = ht_Main["规格"].ToString();
            this.txtJJDW.Text = ht_Main["计价单位"].ToString();
            this.txtPTZDSDJJPL.Text = ht_Main["平台批量"].ToString();
            str_ZZ = ht_Main["上传资质"].ToString();
            btnSelectQY.Text = "选       择";
            ht_ZZ = new Hashtable();//重新初始化资质列表
            ht_ZZ["质量标准与证明"] = ht_Main["ZLBZYZM"];
            ht_ZZ["产品检测报告"] = ht_Main["CPJCBG"];
            ht_ZZ["品管总负责人法律承诺书"] = ht_Main["PGZFZRFLCNS"];
            ht_ZZ["法定代表人承诺书"] = ht_Main["FDDBRCNS"];
            ht_ZZ["售后服务规定与承诺"] = ht_Main["SHFWGDYCN"];
            ht_ZZ["产品送检授权书"] = ht_Main["CPSJSQS"];//6个必填资质
            ht_ZZ["其它资质"] = null;//其他资质项，仅做模拟用。
            ht_QY = null;
            #region 展示当前最低投标价格
            if (ht_Main["最低价格"].ToString() == "0.00")
            {
                label28.Width = 170;
                label28.Text = "当前无卖方参与此商品的投标";
                label28.Visible = false;
                label27.Visible = false;
            }
            else
            {
                label28.Width = 134;
                label28.Text = "当前卖方最低投标价：";
                label28.Visible = true;
                label27.Visible = true;
                label27.Text = ht_Main["最低价格"].ToString() + "元";
            }
            #endregion
            //label28 label25
            #region 展示当前最大的经济批量
            if (Convert.ToInt32(ht_Main["卖家批量"]) == 0)
            {
                label29.Text = "平台设定的该商品最大经济批量：" + ht_Main["平台批量"].ToString() + ht_Main["计价单位"].ToString();
                label29.Width = 300;
                //label29.Text = "当前平台中的最大经济批量：";
                //label26.Text = ht_Main["平台批量"].ToString() + ht_Main["计价单位"].ToString();
                label29.Visible = true;
                label26.Visible = false;
            }
            else
            {
                label29.Text = "当前卖方中的最大经济批量：";
                label26.Text = ht_Main["卖家批量"].ToString() + ht_Main["计价单位"].ToString();
                label29.Visible = true;
                label26.Visible = true;
            }
            #endregion

            panel4.Visible = true;
            //label28.Visible = true;
            //label27.Visible = true;
            //label29.Visible = true;
            //label26.Visible = true;
            if ((str_ZZ == null || str_ZZ.Trim() == "" || str_ZZ.Trim() == "|") && dafwefasdfs(ht_Main["ZLBZYZM"]) && dafwefasdfs(ht_Main["CPJCBG"]) && dafwefasdfs(ht_Main["PGZFZRFLCNS"]) && dafwefasdfs(ht_Main["FDDBRCNS"]) && dafwefasdfs(ht_Main["SHFWGDYCN"]) && dafwefasdfs(ht_Main["CPSJSQS"]))
            {
                btnSCZL.Text = "查 看 资 质";
            }
            else
            {
                btnSCZL.Text = "上 传 资 质";
            }
        }
        private bool dafwefasdfs(object obj)
        {
            if (obj == null || obj.ToString().Trim() == "")
                return false;
            else
                return true;
        }
        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }






       
     

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            fmNMRSP fm = new fmNMRSP(new delegateForThread(TanChuang_demo));
            fm.ShowDialog();
        }


        private void ucSPMC_B_Load(object sender, EventArgs e)
        {
         
        }

        private void btnSCZL_Click(object sender, EventArgs e)
        {
            if (ht_Main == null)
            {
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add("请先选择您要卖出的商品！");
                FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "", al);
                fm.ShowDialog();
                return;
            }
            fmSCZL fms = new fmSCZL(new delegateForThread(Up), ht_ZZ, str_ZZ);
            fms.ShowDialog();
        }
        private void Up(Hashtable OutPutHT)
        {
            try { Invoke(new delegateForThreadShow(Up_Invoke), new Hashtable[] { OutPutHT }); }
            catch (Exception ex) { Support.StringOP.WriteLog("委托回调错误：" + ex.ToString()); }
        }
        private void Up_Invoke(Hashtable OutPutHT)
        {
            ht_ZZ = OutPutHT;
            btnSCZL.Text = "查 看 资 质";
        }

        private void btnSelectQY_Click(object sender, EventArgs e)
        {
            fmXZFHQY fm = new fmXZFHQY((new delegateForThread(QY_Select)), ht_QY,false);
            fm.ShowDialog();
        }
        private void QY_Select(Hashtable OutPutHT)
        {
            try { Invoke(new delegateForThreadShow(QY_Select_Invoke), new Hashtable[] { OutPutHT }); }
            catch (Exception ex) { Support.StringOP.WriteLog("委托回调错误：" + ex.ToString()); }
        }
        private void QY_Select_Invoke(Hashtable OutPutHT)
        {
            ht_QY = OutPutHT;
            this.btnSelectQY.Text = "查       看";
        }


        private void fmTBDXG_Load(object sender, EventArgs e)
        {
            //将滚动条置顶
            UPUP();
            //设置进度条的位置，放到按钮旁边
            PBload.Location = new Point(btnSave.Location.X + btnSave.Width + 25, btnSave.Location.Y + panelTJQ.Height);

            //2013.12.06 wyh add 鼠标略过显示文字
            toolTip1.AutoPopDelay = 30000;
            toolTip1.InitialDelay = 500;
            toolTip1.ReshowDelay = 0;
            toolTip1.ShowAlways = true;
            // string Strtext = "1、一个商品一个轮次的竞标中只有一个最低价标的中标，价格相同时投标时间早的优先，中标买方收货区域将在该标的供货区域范围内。\n2、卖方设定的“经济批量”不得高于“平台设定的最大经济批量”；合同期限为“三个月”或“一年”的，“投标拟售量”不得低于“平台设定的最\n 大经济批量”的10倍；平台设定的最大经济批量”的10倍；合同期限为“即时”的，“投标拟售量”不得低于“平台设定的最大经济批量”。\n3、投标单下达的同时系统将自动冻结投标金额的0.5‰（最低不少于1000元/次）作为投标保证金；该投标保证金将在投标单撤销或定标后\n予以解冻；如未履行定标程序，\n后予以解冻；如未履行定标程序，投标保证金将被扣罚，补偿给各中标买方；合同期限为“即时“的投标单，\n可能会一部分中标（不低于卖方设定的一个经济批量），剩余部分将自动转入下一次交易撮合\n低于卖方设定的一个经济批量），剩余部分将\n自动转入下一次交易撮合（若剩余部分数量低于卖方设定的一个经济批量，则自动撤销）。\n4、按有关协议和交易规定，投标单一旦中标，即\n为交易双方正式订立《电子购货合同》。\n5、为保证快速履约，合同期限为“即时”的投标单中标后，卖方须在72小时内自主冻结履约保证金予以定标。\n6、卖方在履行交货时，须按照买方要求开具增值税专用发票或增值税普通发票（非一般纳税人单位须向税务机关申请代开）。\n  ";
            string Strtext = "1、一个商品一个轮次的竞标中只有一个最低价标的中标，价格相同时投标时间早的优先，中标买方收货区域将在该标的供货区域范围内。\n2、卖方设定的“经济批量”不得高于“平台设定的最大经济批量”；合同期限为“三个月”或“一年”的，“投标拟售量”不得低于“平台设定\n    的最大经济批量”的10倍；平台设定的最大经济批量”的10倍；合同期限为“即时”的，“投标拟售量”不得低于“平台设定的最大经济批量”。\n3、投标单下达的同时系统将自动冻结投标金额的0.5‰（最低不少于1000元/次）作为投标保证金；该投标保证金将在投标单撤销或定标后\n予以解冻；如未履行定标程序，投标保证金将被扣罚，补偿给各中标买方；合同期限为“即时“的投标单，可能会一部分中标（不低于卖方\n设定的一个经济批量），剩余部分将自动转入下一次交易撮合低于卖方设定的一个经济批量），剩余部分将自动转入下一次交易撮合\n（若剩余部分数量低于卖方设定的一个经济批量，则自动撤销）。\n4、按有关协议和交易规定，投标单一旦中标，即为交易双方正式订立《电子购货合同》。\n5、为保证快速履约，合同期限为“即时”的投标单中标后，卖方须在72小时内自主冻结履约保证金予以定标。\n6、卖方在履行交货时，须按照买方要求开具增值税专用发票或增值税普通发票（非一般纳税人单位须向税务机关申请代开）。\n  ";
            toolTip1.SetToolTip(this.label34, Strtext);
            toolTip1.SetToolTip(this.pictureBox1, Strtext);
            // this.toolTip1.Show(Strtext, this.label33);
            this.toolTip1.ToolTipTitle = "卖出须知";

        }

        /// <summary>
        /// 重新下单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click_1(object sender, EventArgs e)
        {
            //禁用提交区域并开启进度
            if (!ValSubInfo())
                return;



            //禁用提交区域并开启进度
            //object nsr = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["一般纳税人资格证扫描件"];
            //if (PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["结算账户类型"] != null && PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["结算账户类型"].ToString() != "买家卖家交易账户")
            //{
            //    ArrayList al = new ArrayList();
            //    al.Add("");
            //    al.Add("您是经纪人交易账户，无法下达预订单！");
            //    FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "中国商品批发交易平台", al);
            //    fm.ShowDialog();
            //    return;
            //}
            //if (nsr == null)
            //{
            //    ArrayList al = new ArrayList();
            //    al.Add("");
            //    al.Add("您未上传一般纳税人资格证扫描件，无法下达投标单！");
            //    FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "中国商品批发交易平台", al);
            //    fm.ShowDialog();
            //    return;
            //}
            //if (GLJJRPTGLJG == null)
            //{
            //    ArrayList al = new ArrayList();
            //    al.Add("");
            //    al.Add("关联经纪人信息异常，无法下达预订单！");
            //    FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "中国商品批发交易平台", al);
            //    fm.ShowDialog();
            //    return;
            //}
            //if (ht_Main == null)
            //{
            //    ArrayList al = new ArrayList();
            //    al.Add("");
            //    al.Add("请选择您要出售的商品！");
            //    FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "", al);
            //    fm.ShowDialog();
            //    return;
            //}
            //if (ht_QY == null)
            //{
            //    ArrayList al = new ArrayList();
            //    al.Add("");
            //    al.Add("请选择发货区域！");
            //    FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "", al);
            //    fm.ShowDialog();
            //    return;
            //}

            //if (ht_ZZ == null)
            //{
            //    ArrayList al = new ArrayList();
            //    al.Add("");
            //    al.Add("请上传必要的资质文件！");
            //    FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "", al);
            //    fm.ShowDialog();
            //    return;
            //}

            //if (str_ZZ.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries).Length > ((Hashtable)ht_ZZ["其它资质"]).Count)
            //{
            //    ArrayList al = new ArrayList();
            //    al.Add("");
            //    al.Add("请上传必要的资质文件！");
            //    FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "", al);
            //    fm.ShowDialog();
            //    return;
            //}

            //if (str_ZZ == null)
            //{
            //    ArrayList al = new ArrayList();
            //    al.Add("");
            //    al.Add("请选择您要出售的商品！");
            //    FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "", al);
            //    fm.ShowDialog();
            //    return;
            //}
            //if (txtJJPL.Text.Trim() == "" || txtTPNGL.Text.Trim() == "" || txtTBJG.Text.Trim() == "")
            //{
            //    ArrayList al = new ArrayList();
            //    al.Add("");
            //    al.Add("请填写正确的经济批量、拟售价及拟售量！");
            //    FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "", al);
            //    fm.ShowDialog();
            //    return;
            //}


            //int MJPL = Convert.ToInt32(this.txtJJPL.Text);//卖家设定的经济批量
            //int PTPL = Convert.ToInt32(ht_Main["平台批量"].ToString());//平台设定的经济批量
            //if (!(MJPL > 0) || (MJPL > PTPL))
            //{
            //    ArrayList al = new ArrayList();
            //    al.Add("");
            //    al.Add("请填写正确的经济批量！");
            //    FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "", al);
            //    fm.ShowDialog();
            //    return;
            //}
            //int TPNSL = Convert.ToInt32(this.txtTPNGL.Text);//卖家投标拟售量
            //if ((PTPL * 10) > TPNSL)
            //{
            //    ArrayList al = new ArrayList();
            //    al.Add("");
            //    al.Add("投标拟售量不得低于平台规定的最大经济批量的10倍！");
            //    FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "", al);
            //    fm.ShowDialog();
            //    return;
            //}


            Hashtable ht = new Hashtable();
            ht["FLAG"] = "预执行";
            ht["DLYX"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString();
            ht["JSZHLX"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["结算账户类型"].ToString();
            ht["MJJSBH"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["卖家角色编号"].ToString();
            ht["GLJJRYX"] = PublicDS.PublisDsUser.Tables["关联信息"].Rows[0]["关联经纪人登陆账号"].ToString();
            ht["GLJJRYHM"] = PublicDS.PublisDsUser.Tables["关联信息"].Rows[0]["关联经纪人用户名"].ToString();
            ht["GLJJRJSBH"] = PublicDS.PublisDsUser.Tables["关联信息"].Rows[0]["关联经纪人编号"].ToString();
            ht["GLJJRPTGLJG"] = GLJJRPTGLJG;//关联经纪人平台管理机构
            ht["SLZM"] = this.ucTextBox_slv.Text.Trim();//this.uCshangchuan1.UpItem.Items[0].SubItems[1].Text;//税率证明
            //ht_Main
            ht["SPBH"] = ht_Main["商品编号"];
            ht["SPMC"] = ht_Main["商品名称"];
            ht["GG"] = ht_Main["规格"];
            ht["JJDW"] = ht_Main["计价单位"];
            ht["PTSDZDJJPL"] = ht_Main["平台批量"];
            ht["HTQX"] = ht_Main["合同期限"];
            ht["MJSDJJPL"] = this.txtJJPL.Text;//卖家设定的经济批量
            ht["GHQY"] = ht_QY["发货区域"].ToString();//供货区域
            ht["TBNSL"] = this.txtTPNGL.Text;//投标拟售量
            ht["TBJG"] = txtTBJG.Text;//投标价格
            ht["TBJE"] = txtTBJE.Text;//投标金额
            ht["MJTBBZJBL"] = MJDJDJBL;//卖家投标保证金比率
            ht["MJTBBZJZXZ"] = MJTBBZJZXZ;//卖家的那个最小值。。
            ht["DJTBBZJ"] = txtDJTBBZJ.Text;//冻结的投标保证金
            ht["ZLBZYZM"] = ht_ZZ["质量标准与证明"];
            ht["CPJCBG"] = ht_ZZ["产品检测报告"];
            ht["PGZFZRFLCNS"] = ht_ZZ["品管总负责人法律承诺书"];
            ht["FDDBRCNS"] = ht_ZZ["法定代表人承诺书"];
            ht["SHFWGDYCN"] = ht_ZZ["售后服务规定与承诺"];
            ht["CPSJSQS"] = ht_ZZ["产品送检授权书"];
            ht["其他资质"] = ht_ZZ["其它资质"];//这个是以哈希表的形式存的
            ht["省市区"] = ucCityList1.SelectedItem[0] + "|" + ucCityList1.SelectedItem[1] + "|" + ucCityList1.SelectedItem[2];
            panel1.Enabled = false;
            panelTJQ.Enabled = false;
            PBload.Visible = true;
            SRT_demo_Run(ht);

        }
        /// <summary>
        /// 重填
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            btnSCZL.Text = "上 传 资 质";
            ucCityList1.initdefault();
            ucTextBox_slv.Text = "";
            
            ht_Main = null;
            ht_QY = null;
            ht_ZZ = null;
            str_ZZ = null;
            txtHTQX.Text = "";
            txtSPMC.Text = "";
            txtGG.Text = "";
            txtJJDW.Text = "";
            txtPTZDSDJJPL.Text = "";
            txtJJPL.Text = "";
            btnSelectQY.Text = "选       择";
            txtTPNGL.Text = "";
            txtTBJG.Text = "";
            txtTBJE.Text = "";
            txtDJTBBZJ.Text = "";
            this.uCshangchuan1.UpItem.Clear();
            label28.Visible = false;
            label27.Visible = false;
            label29.Visible = false;
            label26.Visible = false;
            //str_ZZ = ht_Main["上传资质"].ToString();
            //ht_ZZ = new Hashtable();
        }

        /// <summary>
        /// 存入草稿箱
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCRCGX_Click(object sender, EventArgs e)
        {
            #region 草稿箱的一些必要的验证
            if (ht_Main == null)
            {
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add("请选择您要出售的商品！");
                FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "", al);
                fm.ShowDialog();
                return;
            }
            //wyh 2013.0906 add
            //验证所属区域
            if (ucCityList1.SelectedItem[0].ToString().Contains("请选择") || ucCityList1.SelectedItem[1].ToString().Contains("请选择") || ucCityList1.SelectedItem[2].ToString().Contains("请选择"))
            {
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add("商品产地项请填写完整的省市区信息！");
                FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "中国商品批发交易平台", al);
                fm.ShowDialog();
                return ;
            }
            if (txtJJPL.Text.Trim() == "")
            {
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add("您的经济批量不能为空！");
                al.Add("您的经济批量应不得高于平台设定的最大经济批量，不得为零。");
                FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "", al);
                fm.ShowDialog();
                return;
            }
            //验证供货区域
            if (ht_QY == null)
            {
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add("您的供货区域不能为空！供货区域不得少于两个省份。");
                FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "", al);
                fm.ShowDialog();
                return;
            }
            if (str_ZZ == null)
            {
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add("请选择您要出售的商品！");
                FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "", al);
                fm.ShowDialog();
                return;
            }

            //验证投标你手凉
            if (txtTPNGL.Text.Trim() == "")
            {
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add("您的投标拟售量不能为空！");
                if (txtHTQX.Text.Trim().Equals("即时"))
                {
                    al.Add("投标拟售量不得低于平台规定的最大经济批量。");
                }
                else
                {
                    al.Add("投标拟售量不得低于平台规定的最大经济批量的10倍。");
                }
                FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "", al);
                fm.ShowDialog();
                return;
            }
            //验证投标价格是否填写
            if (txtTBJG.Text.Trim() == "")
            {
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add("您的投标价格不能为空！");
                FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "", al);
                fm.ShowDialog();
                return;
            }
            #endregion

            Hashtable ht = new Hashtable();
            ht["DLYX"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString();
            ht["SPBH"] = ht_Main["商品编号"];
            ht["HTQX"] = ht_Main["合同期限"];
            ht["SLZM"] = ht_Main["SLZM"] == null ? null : ht_Main["SLZM"].ToString();
            ht["GHQY"] = ht_QY["发货区域"].ToString();//供货区域
            ht["MJSDJJPL"] = this.txtJJPL.Text;//卖家设定的经济批量
            ht["TBNSL"] = this.txtTPNGL.Text;//投标拟售量
            ht["TBJG"] = txtTBJG.Text;//投标价格
            ht["TBJE"] = txtTBJE.Text;//投标金额
            ht["ZLBZYZM"] = ht_ZZ["质量标准与证明"] == null ? null : ht_ZZ["质量标准与证明"].ToString();
            ht["CPJCBG"] = ht_ZZ["产品检测报告"] == null ? null : ht_ZZ["产品检测报告"].ToString();
            ht["PGZFZRFLCNS"] = ht_ZZ["品管总负责人法律承诺书"] == null ? null : ht_ZZ["品管总负责人法律承诺书"].ToString();
            ht["SLZM"] = this.ucTextBox_slv.Text.Trim();//this.uCshangchuan1.UpItem.Items.Count == 0 || this.uCshangchuan1.UpItem.Items[0].SubItems[1].Text == "" ? null : this.uCshangchuan1.UpItem.Items[0].SubItems[1].Text;//税率证明
            ht["FDDBRCNS"] = ht_ZZ["法定代表人承诺书"] == null ? null : ht_ZZ["法定代表人承诺书"].ToString();
            ht["SHFWGDYCN"] = ht_ZZ["售后服务规定与承诺"] == null ? null : ht_ZZ["售后服务规定与承诺"].ToString();
            ht["CPSJSQS"] = ht_ZZ["产品送检授权书"] == null ? null : ht_ZZ["产品送检授权书"].ToString();
            ht["其他资质"] = ht_ZZ["其它资质"];//这个是以哈希表的形式存的
            ht["省市区"] = ucCityList1.SelectedItem[0] + "|" + ucCityList1.SelectedItem[1] + "|" + ucCityList1.SelectedItem[2];
            panel1.Enabled = false;
            panelTJQ.Enabled = false;
            PBload.Visible = true;
            //演示新标准
            CG_Run(ht);
        }
        //开启一个测试线程
        private void CG_Run(Hashtable InPutHT)
        {
            G_S g = new G_S(InPutHT, new delegateForThread(CG_Save));
            Thread trd = new Thread(new ThreadStart(g.SetTBDCG));
            trd.IsBackground = true;
            trd.Start();
        }
        //显示线程处理结果的函数(用于处理线程返回数据),调用异步委托
        private void CG_Save(Hashtable OutPutHT)
        {
            try { Invoke(new delegateForThreadShow(CG_Invoke), new Hashtable[] { OutPutHT }); }
            catch (Exception ex) { Support.StringOP.WriteLog("委托回调错误：" + ex.ToString()); }
        }

        //处理非线程创建的控件
        private void CG_Invoke(Hashtable OutPutHT)
        {
            //重新开放提交区域,并滚动条强制置顶
            panel1.Enabled = true;
            panelTJQ.Enabled = true;
            PBload.Visible = false;
            UPUP();


            //返回值后的处理
            DataSet dsreturn = (DataSet)OutPutHT["结果"];

            string zt = dsreturn.Tables["返回值单条"].Rows[0]["执行结果"].ToString();
            string showstr = dsreturn.Tables["返回值单条"].Rows[0]["提示文本"].ToString();
            if (zt == "err")
            {
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add(showstr);
                if (showstr.IndexOf("|||") > 0)
                {
                    al = new ArrayList();
                    al.Add("");
                    string[] action = showstr.Split(new string[] { "|||" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string str in action)
                    {
                        al.Add(str);
                    }
                }

                FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "中国商品批发交易平台", al);
                fm.ShowDialog();
            }
            else if (zt == "ok")
            {
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add(showstr);
                FormAlertMessage fm = new FormAlertMessage("仅确定", "对号", "中国商品批发交易平台", al);
                fm.ShowDialog();
                reset();
            }
 
        }

        private void txtTPNGL_KeyUp(object sender, KeyEventArgs e)
        {
            GetNDGJE();
        }

        private void txtTBJG_KeyUp(object sender, KeyEventArgs e)
        {
            GetNDGJE();
        }
        /// <summary>
        /// 自动计算拟订购金额
        /// </summary>
        private void GetNDGJE()
        {
            double money = 0.00;//拟买入金额
            double result = 0.00;

            if (double.TryParse(this.txtTBJG.Text, out result))
            {
                money = Convert.ToDouble(this.txtTBJG.Text);
            }
            else
            {
                money = 0.00;
            }

            double amount = 0.00;//拟买入数量
            if (double.TryParse(this.txtTPNGL.Text, out result))
            {
                amount = Convert.ToDouble(this.txtTPNGL.Text);//拟买入数量
            }
            else
            {
                amount = 0.00;
            }
            this.txtTBJE.Text = (money * amount).ToString().Length > 10 ? "0.00" : (money * amount).ToString();//订购金额
            if (this.txtTBJE.Text.Trim() != "0.00")
            {
                this.txtDJTBBZJ.Text = Math.Round((money * amount * MJDJDJBL), 2) >= MJTBBZJZXZ ? Math.Round((money * amount * MJDJDJBL), 2).ToString() : MJTBBZJZXZ.ToString();//冻结订金的金额
            }
            else
            {
                this.txtDJTBBZJ.Text = "0.00";
            }
        }






        /// <summary>
        /// 提交时候的前端验证
        /// </summary>
        private bool ValSubInfo()
        {
            #region 验证是否为空
            if (GLJJRPTGLJG == null)
            {
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add("您尚未开通交易账户，不能进行交易操作。");
                FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "中国商品批发交易平台", al);
                fm.ShowDialog();
                return false;
            }
            if (PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["结算账户类型"] != null && PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["结算账户类型"].ToString() != "买家卖家交易账户")
            {
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add("您是经纪人交易账户，无法下达投标单！");
                FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "中国商品批发交易平台", al);
                fm.ShowDialog();
                return false;
            }
            //if (PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["结算账户类型"] != null && PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["结算账户类型"].ToString() != "买家卖家交易帐户" && PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["注册类别"].ToString() == "自然人")
            //{
            //    ArrayList al = new ArrayList();
            //    al.Add("");
            //    al.Add("只有单位才能下达投标单，您目前自然人身份！");
            //    FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "中国商品批发交易平台", al);
            //    fm.ShowDialog();
            //    return false;
            //}

            //验证是否为卖家买家账户
            //object nsr = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["一般纳税人资格证扫描件"];

            //if (nsr == null || nsr.ToString().Trim() == "")
            //{
            //    ArrayList al = new ArrayList();
            //    al.Add("");
            //    al.Add("您未上传一般纳税人资格证扫描件，无法下达投标单！");
            //    FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "中国商品批发交易平台", al);
            //    fm.ShowDialog();
            //    return false;
            //}


            //验证商品是否为空
            if (ht_Main == null || txtHTQX.Text.Trim() == "" || txtSPMC.Text.Trim() == "" || txtGG.Text.Trim() == "" || txtJJDW.Text.Trim() == "" || txtPTZDSDJJPL.Text.Trim() == "")
            {
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add("请选择您要出售的商品！");
                FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "", al);
                fm.ShowDialog();
                return false;
            }
            #region 验证经济批量的填写和正确性
            //验证是否填写经济批量
            if (txtJJPL.Text.Trim() == "")
            {
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add("您的经济批量不能为空！");
                //if (label29.Text.IndexOf("平台设定") >= 0)
                //{
                    al.Add("您的经济批量应不得高于平台设定的最大经济批量，不得为零。");
                //}
                //if (label29.Text.IndexOf("卖方") >= 0)
                //{
                //    al.Add("您的经济批量应不得高于卖方设定的最大经济批量，不得为零。");
                //}
                FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "", al);
                fm.ShowDialog();
                return false;
            }
            if (Convert.ToDouble(txtJJPL.Text) == 0.00)
            {
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add("您的经济批量应不得高于平台设定的最大经济批量，不得为零。");
                FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "", al);
                fm.ShowDialog();
                return false;
            }
            int MJPL = Convert.ToInt32(this.txtJJPL.Text);//卖家设定的经济批量
            int PTPL = Convert.ToInt32(ht_Main["平台批量"].ToString());//平台设定的经济批量
            //验证卖家设定的经济批量
            if (!(MJPL > 0) || (MJPL > PTPL))
            {
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add("您的经济批量应不得高于平台设定的最大经济批量，不得为零。");
                FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "", al);
                fm.ShowDialog();
                return false;
            }
            #endregion
            //验证供货区域
            if (ht_QY == null)
            {
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add("您的供货区域不能为空！供货区域不得少于两个省份。");
                FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "", al);
                fm.ShowDialog();
                return false;
            }

            //wyh 2013.0906 add
            //验证所属区域
            if (ucCityList1.SelectedItem[0].ToString().Contains("请选择") || ucCityList1.SelectedItem[1].ToString().Contains("请选择") || ucCityList1.SelectedItem[2].ToString().Contains("请选择"))
            {
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add("商品产地项请填写完整的省市区信息！");
                FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "中国商品批发交易平台", al);
                fm.ShowDialog();
                return false;
            }
            //验证投标你手凉
            if (txtTPNGL.Text.Trim() == "")
            {
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add("您的投标拟售量不能为空！");
                if (txtHTQX.Text.Trim().Equals("即时"))
                {
                    al.Add("投标拟售量不得低于平台规定的最大经济批量。");
                }
                else
                {
                    al.Add("投标拟售量不得低于平台规定的最大经济批量的10倍。");
                }
                FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "", al);
                fm.ShowDialog();
                return false;
            }
            if (Convert.ToDouble(txtTPNGL.Text) == 0.00)
            {
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add("您的投标拟售量不能为零！");
                if (txtHTQX.Text.Trim().Equals("即时"))
                {

                        al.Add("投标拟售量不得低于平台规定的最大经济批量。");

                }
                else
                {

                    //if (label29.Text.IndexOf("平台设定") >= 0)
                    //{
                        al.Add("投标拟售量不得低于平台规定的最大经济批量的10倍。");
                    //}
                    //if (label29.Text.IndexOf("卖方") >= 0)
                    //{
                    //    al.Add("投标拟售量不得低于卖方设定的最大经济批量的10倍。");
                    //}
                }
                FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "", al);
                fm.ShowDialog();
                return false;
            }
            int TPNSL = Convert.ToInt32(this.txtTPNGL.Text);//卖家投标拟售量
            //验证批量的10倍
            if (txtHTQX.Text.Trim().Equals("即时"))
            {
                if (PTPL > TPNSL)
                {
                    ArrayList al = new ArrayList();
                    al.Add("");
                    //if (label29.Text.IndexOf("平台设定") >= 0)
                    //{
                        al.Add("投标拟售量不得低于平台规定的最大经济批量。");
                    //}
                    //if (label29.Text.IndexOf("卖方") >= 0)
                    //{
                    //    al.Add("投标拟售量不得低于卖方设定的最大经济批量。");
                    //}
                    FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "", al);
                    fm.ShowDialog();
                    return false;
                }
            }
            else
            {
                if ((PTPL * 10) > TPNSL)
                {
                    ArrayList al = new ArrayList();
                    al.Add("");
                    //if (label29.Text.IndexOf("平台设定") >= 0)
                    //{
                        al.Add("投标拟售量不得低于平台规定的最大经济批量的10倍。");
                    //}
                    //if (label29.Text.IndexOf("卖方") >= 0)
                    //{
                    //    al.Add("投标拟售量不得低于卖方设定的最大经济批量的10倍。");
                    //}
                    FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "", al);
                    fm.ShowDialog();
                    return false;
                }
            }
            //if ((PTPL * 10) > TPNSL)
            //{
            //    ArrayList al = new ArrayList();
            //    al.Add("");
            //    al.Add("投标拟售量不得低于平台规定的最大经济批量的10倍！");
            //    FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "", al);
            //    fm.ShowDialog();
            //    return false;
            //}
            //验证投标价格是否填写
            if (txtTBJG.Text.Trim() == "")
            {
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add("您的投标价格不能为空！");
                FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "", al);
                fm.ShowDialog();
                return false;
            }


            if (Convert.ToDouble(txtTBJG.Text) == 0.00)
            {
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add("您的投标价格不能为零！");
                FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "", al);
                fm.ShowDialog();
                return false;
            }

            //if (!(uCshangchuan1.UpItem.Items.Count > 0))
            //{
            //    ArrayList al = new ArrayList();
            //    al.Add("");
            //    al.Add("请上传税率证明！");
            //    FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "", al);
            //    fm.ShowDialog();
            //    return false;
            //}

            if ((this.ucTextBox_slv.Text.Trim().Equals("") || Convert.ToDouble(this.ucTextBox_slv.Text.Trim()) == 0))
            {
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add("税率不能为零，也不能为空！");
                FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "", al);
                fm.ShowDialog();
                return false;
            }
            //验证资质是否上传（完整）
            if (ht_ZZ == null)
            {
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add("请上传必要的资质文件！");
                FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "", al);
                fm.ShowDialog();
                return false;
            }
            if (ht_ZZ["质量标准与证明"] == null || ht_ZZ["产品检测报告"] == null || ht_ZZ["品管总负责人法律承诺书"] == null || ht_ZZ["法定代表人承诺书"] == null || ht_ZZ["售后服务规定与承诺"] == null || ht_ZZ["产品送检授权书"] == null || ht_ZZ["质量标准与证明"].ToString() == "" || ht_ZZ["产品检测报告"].ToString() == "" || ht_ZZ["品管总负责人法律承诺书"].ToString() == "" || ht_ZZ["法定代表人承诺书"].ToString() == "" || ht_ZZ["售后服务规定与承诺"].ToString() == "" || ht_ZZ["产品送检授权书"].ToString() == "")
            {
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add("请上传必要的资质文件！");
                FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "", al);
                fm.ShowDialog();
                return false;
            }
            /*
            if (str_ZZ.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries).Length > ((ht_ZZ["其它资质"] == null || ht_ZZ["其它资质"].ToString() == "") ? 0 : ht_ZZ["其它资质"].ToString().Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries).Length))
            {
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add("请上传必要的资质文件！");
                FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "", al);
                fm.ShowDialog();
                return false;
            }
            */
            #endregion
            #region 验证填写信息的正确性
            double TBJG = Convert.ToDouble(this.txtTBJG.Text);//投标价格
            //验证投标金额是否过大
            if ((TBJG * TPNSL).ToString().Length > 10)
            {
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add("金额过大。");
                FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "", al);
                fm.ShowDialog();
                return false;
            }
            #endregion
            #region 服务器端的验证内容（取一次客户端的）
            //if (nsr == null || nsr.ToString().Trim() == "")
            //{
            //    ArrayList al = new ArrayList();
            //    al.Add("");
            //    al.Add("您未上传一般纳税人资格证扫描件，无法下达投标单！");
            //    FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "中国商品批发交易平台", al);
            //    fm.ShowDialog();
            //    return false;
            //}
            #endregion
            return true;
        }

        private void LLlishi_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Hashtable htcs = new Hashtable();
            htcs["数据地址"] = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[2] + "/pingtaiservices/mobanrun/FMPTDZGHHT.aspx";
            XSRM xs = new XSRM("电子购货合同样本", htcs);
            xs.ShowDialog();
        }

        private void LLlishi_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Hashtable htcs = new Hashtable();
            htcs["数据地址"] = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[2] + "/pingtaiservices/mobanrun/FMPTDZGHHT.aspx";
            XSRM xs = new XSRM("电子购货合同样本", htcs);
            xs.ShowDialog();
        }


    }
}
