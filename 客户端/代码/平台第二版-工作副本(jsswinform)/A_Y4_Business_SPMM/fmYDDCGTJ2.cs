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
    public partial class fmYDDCGTJ2 : BasicForm
    {
        //淡出计时器
        System.Windows.Forms.Timer Timer_DC;

        /// <summary>
        /// 某一条商品的信息
        /// </summary>
        Hashtable ht_Main = null;

        /// <summary>
        /// 当前登录帐号关联经纪人的管理机构
        /// </summary>
        string GLJJRPTGLJG = null;

        /// <summary>
        /// 买家订金冻结比例
        /// </summary>
        double MJDJDJBL = Convert.ToDouble(PublicDS.PublisDsData.Tables["动态参数"].Rows[0]["MJDJBL"].ToString());

        /// <summary>
        /// 对勾选择后的回调指针
        /// </summary>
        delegateForThread dftForParent;
        
        Hashtable htt = new Hashtable();

        //#region 窗体淡出，窗体最小化最大化等特殊控制，所有窗体都有这个玩意

        ///// <summary>
        ///// 窗体的Load事件中的淡出处理
        ///// </summary>
        //private void Init_one_show()
        //{
        //    //设置双缓冲
        //    this.SetStyle(ControlStyles.DoubleBuffer |
        //    ControlStyles.UserPaint |
        //    ControlStyles.AllPaintingInWmPaint,
        //    true);
        //    this.UpdateStyles();

        //    //加载淡出计时器
        //    Timer_DC = new System.Windows.Forms.Timer();
        //    Timer_DC.Interval = Program.DC_Interval;
        //    this.Timer_DC.Tick += new System.EventHandler(this.Timer_DC_Tick);
        //    //淡出效果
        //    MaxDC();
        //}

        ///// <summary>
        ///// 显示窗体时启动淡出
        ///// </summary>
        //private void MaxDC()
        //{
        //    this.Opacity = 0;
        //    Timer_DC.Enabled = true;
        //}

        ////淡出显示窗体，绕过窗体闪烁问题
        //private void Timer_DC_Tick(object sender, EventArgs e)
        //{
        //    this.Opacity = this.Opacity + Program.DC_step;
        //    if (!Program.DC_open)
        //    {
        //        this.Opacity = 1;
        //    }
        //    if (this.Opacity >= 1)
        //    {
        //        Timer_DC.Enabled = false;
        //    }
        //}

        ////允许任务栏最小化
        //protected override CreateParams CreateParams
        //{
        //    get
        //    {
        //        const int WS_MINIMIZEBOX = 0x00020000;  // Winuser.h中定义
        //        CreateParams cp = base.CreateParams;
        //        cp.Style = cp.Style | WS_MINIMIZEBOX;   // 允许最小化操作
        //        return cp;
        //    }
        //}


        //private int WM_SYSCOMMAND = 0x112;
        //private long SC_MAXIMIZE = 0xF030;
        //private long SC_MINIMIZE = 0xF020;
        //private long SC_CLOSE = 0xF060;
        //private long SC_NORMAL = 0xF120;
        //private FormWindowState SF = FormWindowState.Normal;
        ///// <summary>
        ///// 重绘窗体的状态
        ///// </summary>
        ///// <param name="m"></param>
        //protected override void WndProc(ref   Message m)
        //{
        //    if (this.WindowState != FormWindowState.Minimized)
        //    {
        //        SF = this.WindowState;
        //    }
        //    if (m.Msg == WM_SYSCOMMAND)
        //    {
        //        if (m.WParam.ToInt64() == SC_MAXIMIZE)
        //        {
        //            MaxDC();
        //            this.WindowState = FormWindowState.Maximized;
        //            return;
        //        }
        //        if (m.WParam.ToInt64() == SC_MINIMIZE)
        //        {
        //            this.WindowState = FormWindowState.Minimized;
        //            return;
        //        }
        //        if (m.WParam.ToInt64() == SC_NORMAL)
        //        {
        //            MaxDC();
        //            this.WindowState = SF;
        //            return;
        //        }
        //        if (m.WParam.ToInt64() == SC_CLOSE)
        //        {
        //            this.Close();
        //            return;
        //        }
        //    }
        //    base.WndProc(ref   m);
        //}

        //#endregion
        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="dftForParent_temp">回调指针</param>
        public fmYDDCGTJ2( Hashtable ht)
        {
           // dftForParent = dftForParent_temp;
            htt = ht;
            InitializeComponent();
            this.ucCityList1.initdefault();
            this.ucCityList1.VisibleItem = new bool[] { true, true, false };
            //图标
            Icon ic = new Icon(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase+ @"\skin\ji.ico");
            this.Icon = ic;
            //设置窗体显示在屏幕中央
            this.StartPosition = FormStartPosition.CenterScreen;
           //不在任务栏显示此信息
            this.ShowInTaskbar = false;

        }
        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="dftForParent_temp">回调指针</param>
        public fmYDDCGTJ2(delegateForThread dftForParent_temp,bool b)
        {
            dftForParent = dftForParent_temp;

            InitializeComponent();
            this.ucCityList1.initdefault();
            this.ucCityList1.VisibleItem = new bool[] { true, true, false };
            //图标
            Icon ic = new Icon(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\skin\ji.ico");
            this.Icon = ic;
            //设置窗体显示在屏幕中央
            this.StartPosition = FormStartPosition.CenterScreen;
            //不在任务栏显示此信息
            this.ShowInTaskbar = false;

        }
        /// <summary>
        /// 重载构造函数
        /// </summary>
        /// <param name="Number">原单号</param>
        public fmYDDCGTJ2(string Number)
        {
            InitializeComponent();
            this.ucCityList1.initdefault();
            this.ucCityList1.VisibleItem = new bool[] { true, true, false };
            //label17.Text = "5、" + Number + "预订单已撤销。";
            //图标
            Icon ic = new Icon(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\skin\ji.ico");
            this.Icon = ic;
            //设置窗体显示在屏幕中央
            this.StartPosition = FormStartPosition.CenterScreen;
            //不在任务栏显示此信息
            this.ShowInTaskbar = false;
            #region 自动填充被修改的订单信息
            Hashtable ht_Number = new Hashtable();
            ht_Number["Number"] = Number;
            G_B g = new G_B(ht_Number, new delegateForThread(XG));
            Thread trd = new Thread(new ThreadStart(g.YDD_Caogaoinfo));
            trd.IsBackground = true;
            trd.Start();
            #endregion
            #region 获取当前帐号所关联经纪人的管理机构
            string DLYX = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString();//当前用户的登录邮箱
            Hashtable ht = new Hashtable();
            ht["DLYX"] = DLYX;
            G_B gg = new G_B(ht, new delegateForThread(GetGLJJRGLJG));
            Thread trdgg = new Thread(new ThreadStart(gg.GetGLJJRGLJG));
            trdgg.IsBackground = true;
            trdgg.Start();
            #endregion
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

        #region 自动带出已草稿订单的信息
        private void XG(Hashtable OutPutHT)
        {
            try { Invoke(new delegateForThreadShow(XG_Invoke), new Hashtable[] { OutPutHT }); }
            catch (Exception ex) { Support.StringOP.WriteLog("委托回调错误：" + ex.ToString()); }
        }
        private void XG_Invoke(Hashtable OutPutHT)
        {
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
                    this.panel4.Visible = true;
                    DataTable dt = dsreturn.Tables["SPXX"];
                    this.txtHTQX.Text = dt.Rows[0]["HTQX"].ToString();//合同期限
                    this.txtSPMC.Text = dt.Rows[0]["SPMC"].ToString();//商品名称
                    txtGG.Text = dt.Rows[0]["GG"].ToString();//规格
                    txtJJDW.Text = dt.Rows[0]["JJDW"].ToString();//计价单位
                    ucCityList1.SelectedItem = new string[] { dt.Rows[0]["SHQYsheng"].ToString(), dt.Rows[0]["SHQYshi"].ToString() ,""};
                    //ucCityList1.SelectedItem[0] = dt.Rows[0]["SHQYsheng"].ToString();//省
                    //ucCityList1.SelectedItem[1] = dt.Rows[0]["SHQYshi"].ToString();//市
                    txtNMRJG.Text = dt.Rows[0]["NMRJG"].ToString();//拟买入价格
                    txtNDGSL.Text = dt.Rows[0]["NDGSL"].ToString();//拟订购数量
                    txtNDGJE.Text = Convert.ToDouble(dt.Rows[0]["NDGJE"].ToString()).ToString("#0.00");//你订购金额
                    txtDJDJ.Text = Convert.ToDouble(dt.Rows[0]["NEWDJDJ"].ToString()).ToString("#0.00");//冻结订金
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

                    #region 展示当前最低投标价格
                    if (dt.Rows[0]["ZDJG"].ToString() == "0.00")
                    {
                        label27.Text = "平台设定的该商品最大经济批量：" + ht_Main["平台批量"].ToString() + ht_Main["计价单位"].ToString();
                        label27.Width = 280;

                        //label27.Width = 170;
                        //label27.Text = "当前无卖家参与此商品的投标";
                        label27.Visible = true;
                        label27.Location = new Point(363, 22);//重新设置提示文字位置，向下移动，原因：此控件与Lshowshowshow“此商品当前无卖方！”控件在同一个位置
                        Lshowshowshow.Visible = true;
                        label26.Visible = false;
                        this.labelzdjjpl.Visible = false;
                        //有效截止日期
                        //lblYXJZRQ.Visible = true;
                        //dateTimePicker1.Visible = true;
                        //dateTimePicker1.MinDate = DateTime.Now.Date;
                        if (dt.Rows[0]["YXJZRQ"].ToString() == "")
                        {
                            dateTimePicker1.Value = PublicDS.PublisTimeServer;
                        }
                        else
                        {
                            if (DateTime.Parse(dt.Rows[0]["YXJZRQ"].ToString()) > PublicDS.PublisTimeServer)
                            {
                                dateTimePicker1.Value = DateTime.Parse(dt.Rows[0]["YXJZRQ"].ToString());
                            }
                            else
                            {
                                dateTimePicker1.Value = PublicDS.PublisTimeServer;
                            }
                        }
                    }
                    else
                    {
                        label27.Width = 300;
                        label27.Text = "当前卖方最低投标价：" + ht_Main["最低价格"].ToString() + "元";
                        label27.Visible = true;
                        label27.Location = new Point(363, 2);//重新设置提示文字位置，向下移动，原因：此控件与Lshowshowshow“此商品当前无卖方！”控件在同一个位置
                        label26.Visible = false;
                        label26.Text = ht_Main["最低价格"].ToString() + "元";
                        //有效截止日期
                        //lblYXJZRQ.Visible = false;
                        //dateTimePicker1.Visible = false;
                        this.labelzdjjpl.Visible = true;
                    }
                    #endregion

                    #region 展示当前最大的经济批量
                    if (Convert.ToInt32(ht_Main["卖家批量"]) == 0)
                    {
                       // label28.Text = "此商品当前无卖方。您的《预订单》若在有"; wyh dele
                        // label25.Text = "效期内未中标，则于“所发布信息有效截止日期”";wyh dele
                        // label22.Text = "自动撤销，对应订金同时自动解冻。";wyh dele

                        //label28.Text = "当前平台中的最大经济批量：";
                        //label25.Text = ht_Main["平台批量"].ToString() + ht_Main["计价单位"].ToString();
                        label28.Tag = ht_Main["平台批量"].ToString();
                        label28.Visible = true;
                        label25.Visible = true;
                        label22.Visible = true;
                        labelzdjjpl.Visible = false;
                    }
                    else
                    {
                       // label28.Text = "当前卖方中的最大经济批量：" + ht_Main["卖家批量"].ToString() + ht_Main["计价单位"].ToString(); wyh dele
                        this.labelzdjjpl.Text = "当前卖方中的最大经济批量：" + ht_Main["卖家批量"].ToString() + ht_Main["计价单位"].ToString();
                        //label25.Text = ht_Main["卖家批量"].ToString() + ht_Main["计价单位"].ToString();
                        label28.Tag = ht_Main["卖家批量"].ToString();
                        label28.Visible = true;
                       // label25.Visible = false; wyh dele
                        // label22.Visible = false;wyh dele
                        this.labelzdjjpl.Visible = true;
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
            //禁用提交区域并开启进度
            panelTJQ.Enabled = false;
            PBload.Visible = true;

            //演示新标准
            SRT_demo_Run(null);
        }

        //开启一个测试线程
        private void SRT_demo_Run(Hashtable InPutHT)
        {
            G_B g = new G_B(InPutHT, new delegateForThread(SRT_demo));
            Thread trd = new Thread(new ThreadStart(g.SetYDD));
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
            PBload.Visible = false;
            panel2.Enabled = true;
            UPUP();


            //返回值后的处理
            DataSet dsreturn = (DataSet)OutPutHT["结果"];

            string zt = dsreturn.Tables["返回值单条"].Rows[0]["执行结果"].ToString();
            string showstr = dsreturn.Tables["返回值单条"].Rows[0]["提示文本"].ToString();
            if (zt == "err")
            {
                if (showstr.IndexOf("预订单一经提交将冻结") > 0)
                {
                    ArrayList al = new ArrayList();
                    al.Add("");
                    al.Add(showstr);
                    FormAlertMessage fm = new FormAlertMessage("确定取消", "问号", "中国商品批发交易平台", al);
                    DialogResult boxresult = fm.ShowDialog();
                    if (boxresult == DialogResult.Yes)
                    {
                        //真正提交业务单据
                        #region 提交预订单
                        Hashtable ht = new Hashtable();
                        ht["FLAG"] = "真正执行";
                        ht["DLYX"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString();
                        ht["JSZHLX"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["结算账户类型"].ToString();
                        ht["MJJSBH"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["买家角色编号"].ToString();
                        ht["GLJJRYX"] = PublicDS.PublisDsUser.Tables["关联信息"].Rows[0]["关联经纪人登陆账号"].ToString();
                        ht["GLJJRYHM"] = PublicDS.PublisDsUser.Tables["关联信息"].Rows[0]["关联经纪人用户名"].ToString();
                        ht["GLJJRJSBH"] = PublicDS.PublisDsUser.Tables["关联信息"].Rows[0]["关联经纪人编号"].ToString();
                        ht["GLJJRPTGLJG"] = GLJJRPTGLJG;//关联经纪人平台管理机构

                        //ht_Main
                        ht["SPBH"] = ht_Main["商品编号"];
                        ht["SPMC"] = ht_Main["商品名称"];
                        ht["GG"] = ht_Main["规格"];
                        ht["JJDW"] = ht_Main["计价单位"];
                        ht["PTSDZDJJPL"] = Convert.ToInt32(label28.Tag);
                        ht["HTQX"] = ht_Main["合同期限"];
                        ht["SHQY"] = "|" + ucCityList1.SelectedItem[0] + ucCityList1.SelectedItem[1] + "|";//省市
                        ht["SHQYsheng"] = ucCityList1.SelectedItem[0];//省
                        ht["SHQYshi"] = ucCityList1.SelectedItem[1];//市
                        ht["NMRJG"] = this.txtNMRJG.Text;//拟买入价格
                        ht["NDGSL"] = this.txtNDGSL.Text;//拟订购数量
                        ht["YZBSL"] = "0";
                        ht["NDGJE"] = this.txtNDGJE.Text;
                        ht["MJDJBL"] = MJDJDJBL;//买家保证金冻结比例
                        ht["DJDJ"] = txtDJDJ.Text;//要冻结多少订金
                        ht["ZT"] = "竞标";
                        if (dateTimePicker1.Visible == false)
                        {
                            ht["YXJZRQ"] = "";//有效截止日期
                        }
                        else
                        {
                            ht["YXJZRQ"] = dateTimePicker1.Value.ToString();//有效截止日期
                        }
                        panelTJQ.Enabled = false;
                        PBload.Visible = true;
                        //演示新标准
                        SRT_demo_Run(ht);
                        #endregion
                    }
                }
                else
                {
                    //ArrayList al = new ArrayList();
                    //al.Add("");
                    //al.Add(showstr);
                    //FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "中国商品批发交易平台", al);
                    //fm.ShowDialog();
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
                        this.Close();
                        this.Dispose();
                        return;
                    }
                    if (showstr.IndexOf("与该预订单的订金差额为") > 0)
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

                    ArrayList alMain = new ArrayList();
                    alMain.Add("");
                    alMain.Add(showstr);
                    FormAlertMessage fmMain = new FormAlertMessage("仅确定", "其他", "中国商品批发交易平台", alMain);
                    fmMain.ShowDialog();
                    this.Close();
                    this.Dispose();
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
            fmYDDXG f = new fmYDDXG(null,false);
            f.Dock = DockStyle.Fill;//铺满
            f.AutoScroll = true;//出现滚动条
            f.BackColor = Color.AliceBlue;

            //清理控件
            Panel P = (Panel)this.Parent;
            P.Controls.Clear();
            P.Controls.Add(this); //加入到某一个panel中
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
        }

       

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

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
            this.panel4.Visible = true;
            ht_Main = OutPutHT;//会传过来商品的某个信息
            this.txtHTQX.Text = ht_Main["合同期限"].ToString();
            this.txtSPMC.Text = ht_Main["商品名称"].ToString();
            this.txtGG.Text = ht_Main["规格"].ToString();
            this.txtJJDW.Text = ht_Main["计价单位"].ToString();

            #region 展示当前最低投标价格
            if (ht_Main["最低价格"].ToString() == "0.00")
            {
                label27.Text = "平台设定的该商品最大经济批量：" + ht_Main["平台批量"].ToString() + ht_Main["计价单位"].ToString();
                label27.Width = 280;

                //label27.Width = 170;
                //label27.Text = "当前无卖家参与此商品的投标";
                Lshowshowshow.Visible = true;
                label27.Visible = true;
                label27.Location = new Point(363, 22);//重新设置提示文字位置，向下移动，原因：此控件与Lshowshowshow“此商品当前无卖方！”控件在同一个位置
                label26.Visible = false;
                this.labelzdjjpl.Visible = false;
                //有效截止日期
                //lblYXJZRQ.Visible = true;
                //dateTimePicker1.Visible = true;
                dateTimePicker1.MinDate = PublicDS.PublisTimeServer;
                dateTimePicker1.Value = PublicDS.PublisTimeServer;
            }
            else
            {
                label27.Width = 134;
                Lshowshowshow.Visible = false;
                label27.Text = "当前卖方最低投标价：";
                label27.Visible = true;
                label27.Location = new Point(362, 3);//重新设置提示文字位置，向上移动
                label26.Visible = true;
                label26.Text = ht_Main["最低价格"].ToString() + "元";
                //有效截止日期
                //lblYXJZRQ.Visible = false;
                //dateTimePicker1.Visible = false;
                this.labelzdjjpl.Visible = true;
            }
            #endregion

            #region 展示当前最大的经济批量
            if (Convert.ToInt32(ht_Main["卖家批量"]) == 0)
            {
               // label28.Text = "此商品当前无卖方。您的《预订单》若在有"; wyh dele
                //  label25.Text = "效期内未中标，则于“所发布信息有效截止日期”";wyh dele
                // label22.Text = "自动撤销，对应订金同时自动解冻。";wyh dele

                //label28.Text = "当前平台中的最大经济批量：";
                //label25.Text = ht_Main["平台批量"].ToString() + ht_Main["计价单位"].ToString();
                label28.Tag = ht_Main["平台批量"].ToString();
                label28.Visible = true;
                label25.Visible = true;
                label22.Visible = true;
                this.labelzdjjpl.Visible = false;
            }
            else
            {
                //label28.Text = "当前卖方中的最大经济批量：" + ht_Main["卖家批量"].ToString() + ht_Main["计价单位"].ToString();
                this.labelzdjjpl.Text = "当前卖方中的最大经济批量：" + ht_Main["卖家批量"].ToString() + ht_Main["计价单位"].ToString();
                //label28.Text = "当前卖方中的最大经济批量：";
                //label25.Text = ht_Main["卖家批量"].ToString() + ht_Main["计价单位"].ToString();
                label28.Tag = ht_Main["卖家批量"].ToString();
                label28.Visible = true;
               // label25.Visible = false; wyh dele
                // label22.Visible = false; wyh dele
                this.labelzdjjpl.Visible = true;
            }
            #endregion

            label20.Visible = true;
            label21.Visible = true;
            panelHTQX.Visible = true;
            panelSPMC.Visible = true;
            panelGG.Visible = true;
            panelJJDW.Visible = true;
         
        }

        private void panelUC5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void fmYDDXG_Load(object sender, EventArgs e)
        {
            //将滚动条置顶
            UPUP();
            //设置进度条的位置，放到按钮旁边
            PBload.Location = new Point(btnSave.Location.X + btnSave.Width + 25, btnSave.Location.Y);
            dateTimePicker1.MaxDate = PublicDS.PublisTimeServer.AddMonths(3);
            dateTimePicker1.MinDate = PublicDS.PublisTimeServer;

            //2013.12.06 wyh add 鼠标略过显示文字
            toolTip1.AutoPopDelay = 30000;
            toolTip1.InitialDelay = 500;
            toolTip1.ReshowDelay = 0;
            toolTip1.ShowAlways = true;
            string Strtext = "1、预订单下达的同时系统将自动冻结预订总金额的7%作为订金；\n该订金将在预订单撤销、废标、合同期内提货单全部下达完毕或《电子购货合同》期满时予以解冻 。\n2、预订单在一个轮次的竞标中可能会一部分中标，剩余部分将自动转入下一轮次的竞标。\n3、按有关协议和交易规定，预订单一旦中标，即为交易双方正式签订《电子购货合同》。\n4、为保证快速履约，合同期限为“即时”的预订单定标后，买方须在72小时内下达提货单。";
            toolTip1.SetToolTip(this.label33, Strtext);
            toolTip1.SetToolTip(this.pictureBox1, Strtext);
            // this.toolTip1.Show(Strtext, this.label33);
            this.toolTip1.ToolTipTitle = "买入须知";

            this.toolTip2.AutoPopDelay = 30000;
            this.toolTip2.InitialDelay = 500;
            this.toolTip2.ReshowDelay = 0;
            this.toolTip2.ShowAlways = true;
            string Strtext2 = "您的《预订单》包括在一个轮次竞标中出现\n部分中标，未中标部分的《预订单》，若在\n有效期内未中标，则于“所发布信息有效截止\n日期”自动撤销，对应订金同时自动解冻。";

            this.toolTip2.SetToolTip(this.pictureBox2, Strtext2);
        }

        /// <summary>
        /// 重新保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click_1(object sender, EventArgs e)
        {
            #region  2013.06.13 wyh dele
            /*
            if (GLJJRPTGLJG == null)
            {
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add("关联经纪人信息异常，无法下达预订单！");
                FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "中国商品批发交易平台", al);
                fm.ShowDialog();
                return;
            }
            if (ht_Main == null)
            {
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add("请先选择您要订购的商品！");
                FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "中国商品批发交易平台", al);
                fm.ShowDialog();
                return;
            }
            if (ucCityList1.SelectedItem[0].IndexOf("选择") > 0 || ucCityList1.SelectedItem[1].IndexOf("选择") > 0)
            {
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add("请选择收货区域！");
                FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "中国商品批发交易平台", al);
                fm.ShowDialog();
                return;
            }
            if (txtNMRJG.Text.Trim() == "" || txtNDGSL.Text.Trim() == "" || Convert.ToDouble(txtNDGJE.Text) == 0 || Convert.ToDouble(txtDJDJ.Text) == 0)
            {
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add("请填写正确的拟买入价格和拟订购数量！");
                FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "", al);
                fm.ShowDialog();
                return;
            }
            int pl = Convert.ToInt32(label25.Tag);
            if (Convert.ToInt32(txtNDGSL.Text) < pl)
            {
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add("您的拟订购数量需大于当前平台中此商品的经济批量！");
                FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "", al);
                fm.ShowDialog();
                return;
            }
              */
            #endregion  
            if (!ValSubInfo())
                return;
            Hashtable ht = new Hashtable();
            ht["FLAG"] = "预执行";
            ht["DLYX"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString();
            ht["JSZHLX"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["结算账户类型"].ToString();
            ht["MJJSBH"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["买家角色编号"].ToString();
            ht["GLJJRYX"] = PublicDS.PublisDsUser.Tables["关联信息"].Rows[0]["关联经纪人登陆账号"].ToString();
            ht["GLJJRYHM"] = PublicDS.PublisDsUser.Tables["关联信息"].Rows[0]["关联经纪人用户名"].ToString();
            ht["GLJJRJSBH"] = PublicDS.PublisDsUser.Tables["关联信息"].Rows[0]["关联经纪人编号"].ToString();
            ht["GLJJRPTGLJG"] = GLJJRPTGLJG;//关联经纪人平台管理机构

            //ht_Main
            ht["SPBH"] = ht_Main["商品编号"];
            ht["SPMC"] = ht_Main["商品名称"];
            ht["GG"] = ht_Main["规格"];
            ht["JJDW"] = ht_Main["计价单位"];
            ht["PTSDZDJJPL"] = Convert.ToInt32(label28.Tag);
            ht["HTQX"] = ht_Main["合同期限"];
            ht["SHQY"] = "|" + ucCityList1.SelectedItem[0] + ucCityList1.SelectedItem[1] + "|";//省市
            ht["SHQYsheng"] = ucCityList1.SelectedItem[0];//省
            ht["SHQYshi"] = ucCityList1.SelectedItem[1];//市
            ht["NMRJG"] = this.txtNMRJG.Text;//拟买入价格
            ht["NDGSL"] = this.txtNDGSL.Text;//拟订购数量
            ht["YZBSL"] = "0";
            ht["NDGJE"] = this.txtNDGJE.Text;
            ht["MJDJBL"] = MJDJDJBL;//买家保证金冻结比例
            ht["DJDJ"] = txtDJDJ.Text;//要冻结多少订金
            ht["ZT"] = "竞标";
            if (dateTimePicker1.Visible == false)
            {
                ht["YXJZRQ"] = "";//有效截止日期
            }
            else
            {
                ht["YXJZRQ"] = dateTimePicker1.Value.ToString();//有效截止日期
            }
            panel2.Enabled = false;
            PBload.Location = new Point(btnSave.Location.X + flowLayoutPanel1.Location.X + btnSave.Width + 5, btnSave.Location.Y + panel15.Location.Y + flowLayoutPanel1.Location.Y-2 );

            PBload.Visible = true;
            SRT_demo_Run(ht);
        }




        private bool ValSubInfo()
        {
            //if (GLJJRPTGLJG == null)
            //{
            //    ArrayList al = new ArrayList();
            //    al.Add("");
            //    al.Add("您尚未开通交易账户，不能进行交易操作。");
            //    FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "中国商品批发交易平台", al);
            //    fm.ShowDialog();
            //    return false;
            //}

            if (PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["结算账户类型"].ToString().Trim().Equals(""))
            {
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add("您尚未提交开通交易账户申请，请及时提交！");
                FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "中国商品批发交易平台", al);
                fm.ShowDialog();
                return false;
            }



            if (ht_Main == null || txtHTQX.Text.Trim() == "" || txtSPMC.Text.Trim() == "" || txtGG.Text.Trim() == "" || txtJJDW.Text.Trim() == "")
            {
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add("请先选择您要订购的商品！");
                FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "", al);
                fm.ShowDialog();
                return false;
            }

           
            if (txtNMRJG.Text.Trim() == "")
            {
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add("您的拟买入价格不能为空！");
                al.Add("拟买入价格低于最低价标的投标价格时，不能参与集合计算。");
                FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "", al);
                fm.ShowDialog();
                return false;
            }
            if (Convert.ToDouble(txtNMRJG.Text) == 0.00)
            {
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add("您的拟买入价格不能为零！");
                al.Add("拟买入价格低于最低价标的投标价格时，不能参与集合计算。");
                FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "", al);
                fm.ShowDialog();
                return false;
            }
            if (txtNDGSL.Text.Trim() == "")
            {
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add("您的拟订购数量不能为空！");
                if (label27.Text.IndexOf("最大经济批量") > -1)
                {
                    al.Add("拟订购数量需大于等于当前平台设定的最大的经济批量，否则预订单无法下达。");
                }
                else
                {
                    al.Add("拟订购数量需大于等于当前竞标卖方中设定的最大的经济批量，否则预订单无法下达。");
                }
                FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "", al);
                fm.ShowDialog();
                return false;
            }
            if (Convert.ToInt64(txtNDGSL.Text) == 0)
            {
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add("您的拟订购数量不能为零！");
                if (label27.Text.IndexOf("最大经济批量") > -1)
                {
                    al.Add("拟订购数量需大于等于当前平台设定的最大的经济批量，否则预订单无法下达。");
                }
                else
                {
                    al.Add("拟订购数量需大于等于当前竞标卖方中设定的最大的经济批量，否则预订单无法下达。");
                }
                FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "", al);
                fm.ShowDialog();
                return false;
            }


            long pl = Convert.ToInt64(label28.Tag);
            if (Convert.ToInt64(txtNDGSL.Text) < pl)
            {
                ArrayList al = new ArrayList();
                al.Add("");
                if (label27.Text.IndexOf("最大经济批量") > -1)
                {
                    al.Add("拟订购数量需大于等于当前平台设定的最大的经济批量，否则预订单无法下达！");
                }
                else
                {
                    al.Add("拟订购数量需大于等于当前竞标卖方中设定的最大的经济批量，否则预订单无法下达。");
                }
                FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "", al);
                fm.ShowDialog();
                return false;
            }

            if (ucCityList1.SelectedItem[0].IndexOf("选择") > 0 || ucCityList1.SelectedItem[1].IndexOf("选择") > 0)
            {
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add("您的收货区域不能为空！");
                FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "", al);
                fm.ShowDialog();
                return false;
            }

            if ((Convert.ToDouble(txtNMRJG.Text) * Convert.ToInt64(txtNDGSL.Text)).ToString("#0.00").Length > 10)
            {
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add("金额过大。");
                FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "", al);
                fm.ShowDialog();
                return false;
            }

            return true;
        }



        /// <summary>
        /// 重填
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            ht_Main = null;
            txtHTQX.Text = "";
            txtSPMC.Text = "";
            txtGG.Text = "";
            txtJJDW.Text = "";
            this.ucCityList1.initdefault();
            this.ucCityList1.VisibleItem = new bool[] { true, true, false };
            txtNMRJG.Text = "";
            txtNDGSL.Text = "";
            txtNDGJE.Text = "";
            txtDJDJ.Text = "";
            //this.panel4.Visible = false;
            this.labelzdjjpl.Visible = false;

            //label25.Visible = false;
            label23.Visible = false;
            Lshowshowshow.Visible = false;
            label20.Visible = false;
            label21.Visible = false;
            label19.Visible = false;
            label27.Visible = false;
            label26.Visible = false;
            panelHTQX.Visible = false;
            panelSPMC.Visible = false;
            panelGG.Visible = false;
            panelJJDW.Visible = false;
            panelNDGJE.Visible = false;
            panelDJDJ.Visible = false;
        }

        /// <summary>
        /// 存入草稿箱
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void basicButton3_Click(object sender, EventArgs e)
        {
            if (ht_Main == null)
            {
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add("请先选择您要订购的商品！");
                FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "中国商品批发交易平台", al);
                fm.ShowDialog();
                return;
            }


            if (txtNMRJG.Text.Trim() == "")
            {
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add("您的拟买入价格不能为空！");
                al.Add("拟买入价格低于最低价标的投标价格时，不能参与集合计算。");
                FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "", al);
                fm.ShowDialog();
                return;
                
            }
            if (Convert.ToDouble(txtNMRJG.Text) == 0.00)
            {
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add("您的拟买入价格不能为零！");
                al.Add("拟买入价格低于最低价标的投标价格时，不能参与集合计算。");
                FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "", al);
                fm.ShowDialog();
                return ;
            }
            if (txtNDGSL.Text.Trim() == "")
            {
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add("您的拟订购数量不能为空！");
                if (label27.Text.IndexOf("最大经济批量") > -1)
                {
                    al.Add("拟订购数量需大于等于当前平台设定的最大的经济批量，否则预订单无法下达。");
                }
                else
                {
                    al.Add("拟订购数量需大于等于当前竞标卖方中设定的最大的经济批量，否则预订单无法下达。");
                }
                FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "", al);
                fm.ShowDialog();
                return ;
            }
            if (Convert.ToInt64(txtNDGSL.Text) == 0)
            {
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add("您的拟订购数量不能为零！");
                if (label27.Text.IndexOf("最大经济批量") > -1)
                {
                    al.Add("拟订购数量需大于等于当前平台设定的最大的经济批量，否则预订单无法下达。");
                }
                else
                {
                    al.Add("拟订购数量需大于等于当前竞标卖方中设定的最大的经济批量，否则预订单无法下达。");
                }
                FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "", al);
                fm.ShowDialog();
                return ;
            }

            if (ucCityList1.SelectedItem[0].IndexOf("选择") > 0 || ucCityList1.SelectedItem[1].IndexOf("选择") > 0)
            {
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add("请选择收货区域！");
                FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "中国商品批发交易平台", al);
                fm.ShowDialog();
                return;
            }

            if ((Convert.ToDouble(txtNMRJG.Text) * Convert.ToInt64(txtNDGSL.Text)).ToString("#0.00").Length > 10)
            {
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add("金额过大。");
                FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "", al);
                fm.ShowDialog();
                return;
            }

        
            Hashtable ht = new Hashtable();

            ht["DLYX"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString();
            ht["SPBH"] = ht_Main["商品编号"];
            ht["HTQX"] = ht_Main["合同期限"];
            ht["SHQY"] = "|" + ucCityList1.SelectedItem[0] + ucCityList1.SelectedItem[1] + "|";//省市
            ht["SHQYsheng"] = ucCityList1.SelectedItem[0];//省
            ht["SHQYshi"] = ucCityList1.SelectedItem[1];//市
            ht["NMRJG"] = this.txtNMRJG.Text;//拟买入价格
            ht["NDGSL"] = this.txtNDGSL.Text;//拟订购数量
            ht["NDGJE"] = this.txtNDGJE.Text;
            if (dateTimePicker1.Visible == false)
            {
                ht["YXJZRQ"] = "";//有效截止日期
            }
            else
            {
                ht["YXJZRQ"] = dateTimePicker1.Value.ToString();//有效截止日期
            }

            panel2.Enabled = false;

            PBload.Location = new Point(basicButton3.Location.X + flowLayoutPanel1.Location.X + basicButton3.Width + 5, basicButton3.Location.Y + panel15.Location.Y + flowLayoutPanel1.Location.Y - 2);
            PBload.Visible = true;
            //演示新标准
            CG_Run(ht);
        }
        //开启一个测试线程
        private void CG_Run(Hashtable InPutHT)
        {
            G_B g = new G_B(InPutHT, new delegateForThread(CG_Save));
            Thread trd = new Thread(new ThreadStart(g.SetYDDCG));
            trd.IsBackground = true;
            trd.Start();
        }
        //显示线程处理结果的函数(用于处理线程返回数据),调用异步委托
        private void CG_Save(Hashtable OutPutHT)
        {
            try { Invoke(new delegateForThreadShow(CG_Invoke), new Hashtable[] { OutPutHT }); }
            catch (Exception ex) { Support.StringOP.WriteLog("委托回调错误：" + ex.ToString()); }
        }
        private void CG_Invoke(Hashtable OutPutHT)
        {
            //重新开放提交区域,并滚动条强制置顶
            panel2.Enabled = true;
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
            else if (zt == "是")
            {
                ArrayList alMain = new ArrayList();
                alMain.Add("");
                alMain.Add(showstr);
                FormAlertMessage fmMain = new FormAlertMessage("仅确定", "对号", "中国商品批发交易平台", alMain);
                fmMain.ShowDialog();
                return;
            }
        }

        /// <summary>
        /// 自动计算拟订购金额
        /// </summary>
        private void GetNDGJE()
        {
            double money = 0.00;//拟买入金额
            double result = 0.00;

            if (double.TryParse(this.txtNMRJG.Text, out result))
            {
                money = Convert.ToDouble(this.txtNMRJG.Text);
            }
            else
            {
                money = 0.00;
            }

            double amount = 0.00;//拟买入数量
            if (double.TryParse(this.txtNDGSL.Text, out result))
            {
                amount = Convert.ToDouble(this.txtNDGSL.Text);//拟买入数量
            }
            else
            {
                amount = 0.00;
            }
            //this.txtNDGJE.Text = (money * amount).ToString().Length > 10 ? "0.00" : (money * amount).ToString("#0.00");//订购金额
            this.txtNDGJE.Text =  (money * amount).ToString("#0.00");//订购金额
            if (this.txtNDGJE.Text.Trim() != "0.00")
            {
                this.txtDJDJ.Text = Math.Round((money * amount * MJDJDJBL), 2).ToString("#0.00");//冻结订金的金额
            }
            else
            {
                this.txtDJDJ.Text = "0.00";
            }
            panelNDGJE.Visible = true;
            panelDJDJ.Visible = true;
        }

        private void txtNMRJG_KeyUp(object sender, KeyEventArgs e)
        {
            GetNDGJE();
        }

        private void txtNDGSL_KeyUp(object sender, KeyEventArgs e)
        {
            GetNDGJE();
        }

        private void LLlishi_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Hashtable htcs = new Hashtable();
            htcs["数据地址"] = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[2] + "/pingtaiservices/mobanrun/FMPTDZGHHT.aspx";
            XSRM xs = new XSRM("电子购货合同样本", htcs);
            xs.ShowDialog();
        }





     
      


    }
}
