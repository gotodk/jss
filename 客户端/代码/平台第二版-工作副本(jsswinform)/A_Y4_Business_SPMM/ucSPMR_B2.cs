using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using 客户端主程序.NewDataControl;
using System.Collections;
using System.Threading;
using 客户端主程序.DataControl;
using 客户端主程序.SubForm.NewCenterForm.GZZD;
namespace 客户端主程序.SubForm.NewCenterForm.SUBUC.SPMM
{
    public partial class ucSPMR_B2 : UserControl
    {
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

        public ucSPMR_B2()
        {
            InitializeComponent();
            this.ucCityList1.initdefault();
            this.ucCityList1.VisibleItem = new bool[] { true, true, false };
            label19.Visible = false;
            //label20.Visible = false;wyh
            labelzdjjpl.Visible = false;
            label21.Visible = false;
           // label22.Visible = false;wyh
           // label24.Visible = false;wyh
            #region 开启获取经纪人管理机构线程
            string DLYX = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString();//当前用户的登录邮箱
            Hashtable ht = new Hashtable();
            ht["DLYX"] = DLYX;
            G_B g = new G_B(ht, new delegateForThread(GetGLJJRGLJG));
            Thread trd = new Thread(new ThreadStart(g.GetGLJJRGLJG));
            trd.IsBackground = true;
            trd.Start();
            #endregion
            //DataSet dss = PublicDS.PublisDsData;
            //dateTimePicker1.MaxDate = DateTime.Now.AddMonths(3);
            //dateTimePicker1.MinDate = DateTime.Now;PublicDS.PublisTimeServer 
            dateTimePicker1.MaxDate = PublicDS.PublisTimeServer.AddMonths(3);
            dateTimePicker1.MinDate = PublicDS.PublisTimeServer;
            txtNDGJE.Text = "";
            txtDJDJ.Text = "";
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
            //if (dsreturn.Tables["返回值单条"].Rows[0]["执行结果"].ToString() == "err")
            //{
            //    ArrayList al = new ArrayList();
            //    al.Add("");
            //    al.Add(dsreturn.Tables["返回值单条"].Rows[0]["提示文本"].ToString());
            //    FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "", al);
            //    fm.ShowDialog();
            //}
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

        /// <summary>
        ///确认
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValSubInfo())
                return;


            Hashtable ht = new Hashtable();
            ht["FLAG"] = "预执行";
            ht["DLYX"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString();
            ht["JSZHLX"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["结算账户类型"].ToString();
            ht["MJJSBH"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["买家角色编号"].ToString();
            ht["GLJJRYX"] = "";//PublicDS.PublisDsUser.Tables["关联信息"].Rows[0]["关联经纪人登陆账号"].ToString();
            ht["GLJJRYHM"] = ""; //PublicDS.PublisDsUser.Tables["关联信息"].Rows[0]["关联经纪人用户名"].ToString();
            ht["GLJJRJSBH"] = "";// PublicDS.PublisDsUser.Tables["关联信息"].Rows[0]["关联经纪人编号"].ToString();
            ht["GLJJRPTGLJG"] = "";// GLJJRPTGLJG;//关联经纪人平台管理机构

            //ht_Main
            ht["SPBH"] = ht_Main["商品编号"];
            ht["SPMC"] = ht_Main["商品名称"];
            ht["GG"] = ht_Main["规格"];
            ht["JJDW"] = ht_Main["计价单位"];
            ht["PTSDZDJJPL"] = Convert.ToInt32(label24.Tag);
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
            

            panel1.Enabled = false;
            panelTJQ.Enabled = false;
            PBload.Location = new Point(btnSave.Location.X + panel1.Location.X+flowLayoutPanel1.Location.X + btnSave.Width + 7, btnSave.Location.Y + panel15.Location.Y+flowLayoutPanel1.Location.Y );
            PBload.Visible = true;
            //演示新标准
            SRT_demo_Run(ht);

            
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
                if (showstr.IndexOf("预订单一经提交将冻结") > 0)
                {
                    ArrayList al = new ArrayList();
                    al.Add("");
                    al.Add(showstr);
                    FormAlertMessage fm = new FormAlertMessage("确定取消", "问号", "中国商品批发交易平台", al);
                    DialogResult boxresult = fm.ShowDialog();
                    if (boxresult == DialogResult.Yes)
                    {
                        if (!ValSubInfo())
                            return;
                        //真正提交业务单据
                        panel1.Enabled = false;
                        panelTJQ.Enabled = false;
                        PBload.Visible = true;
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
                        ht["PTSDZDJJPL"] = Convert.ToInt32(label24.Tag);
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
                    if (showstr.IndexOf("与该预订单的订金差额为") > 0)
                    {
                        string[] strAy = showstr.Split(new char[] { '。' }, StringSplitOptions.RemoveEmptyEntries);
                        ArrayList al = new ArrayList();
                        al.Add("");
                        al.Add(strAy[0]+"。");
                        al.Add(strAy[1]+"。");
                        FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "中国商品批发交易平台", al);
                        fm.ShowDialog();
                        return;
                    }
                    
                    ArrayList alMain = new ArrayList();
                    alMain.Add("");
                    alMain.Add(showstr);
                    FormAlertMessage fmMain = new FormAlertMessage("仅确定", "其他", "中国商品批发交易平台", alMain);
                    fmMain.ShowDialog();
                    if (showstr.Contains("该商品刚刚下线"))
                    {
                        reset();
                    }

                }
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
        /// <summary>
        /// 重置表单
        /// </summary>
        private void reset()
        {
            ucSPMR_B2 YDD = new ucSPMR_B2();
            YDD.Dock = DockStyle.Fill;//铺满
            YDD.AutoScroll = true;//出现滚动条
            YDD.BackColor = Color.AliceBlue;
            //清理控件
            Panel P = (Panel)this.Parent;
            P.Controls.Clear();
            P.Controls.Add(YDD); //加入到某一个panel中
            YDD.Show();//显示出来
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

            //this.PBload.Visible = true;
            //将滚动条置顶
            UPUP();
            //设置进度条的位置，放到按钮旁边
            PBload.Location = new Point(btnSave.Location.X + btnSave.Width + 25, btnSave.Location.Y + 140);

            label25.Visible = false;
            label23.Visible = false;
            Lshowshowshow.Visible = false;
            label21.Visible = false;
            label19.Visible = false;
            labelzdjjpl.Visible = false;
            panelHTQX.Visible = false;
            panelSPMC.Visible = false;
            panelGG.Visible = false;
            panelJJDW.Visible = false;

            ////有效截止日期
            //lblYXJZRQ.Visible = false;
            //dateTimePicker1.Visible = false;



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

            toolTip2.AutoPopDelay = 30000;
            toolTip2.InitialDelay = 500;
            toolTip2.ReshowDelay = 0;
            toolTip2.ShowAlways = true;
            string Strtext2 = "您的《预订单》包括在一个轮次竞标中出现\n部分中标，未中标部分的《预订单》，若在\n有效期内未中标，则于“所发布信息有效截止\n日期”自动撤销，对应订金同时自动解冻。";

            toolTip2.SetToolTip(this.pictureBox2, Strtext2);

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
            ht_Main = OutPutHT;//会传过来商品的某个信息
            this.txtHTQX.Text = ht_Main["合同期限"].ToString();
            panelHTQX.Visible = true;
            this.txtSPMC.Text = ht_Main["商品名称"].ToString();
            panelSPMC.Visible = true;
            this.txtGG.Text = ht_Main["规格"].ToString();
            panelGG.Visible = true;
            this.txtJJDW.Text = ht_Main["计价单位"].ToString();
            panelJJDW.Visible = true;

            this.txtNMRJG.Text = "";
            this.txtNDGSL.Text = "";

            panelNDGJE.Visible = false;
            panelDJDJ.Visible = false;
            this.txtNDGJE.Text = "";
            this.txtDJDJ.Text = "";

            //顶部提示文字:请再次确认您选择的“商品名称”与“合同期限”；如需更改，请进入“选择商品”界面，重新选定。
            label25.Visible = true;
            label23.Visible = true;

            #region 展示当前最低投标价格
            
            if (ht_Main["最低价格"].ToString() == "0.00")
            {
                label21.Text = "平台设定的该商品最大经济批量："+ht_Main["平台批量"].ToString() + ht_Main["计价单位"].ToString();                
                label21.Width = 300;
                

                //label21.Text = "当前无卖家参与此商品的投标";
                Lshowshowshow.Visible = true;

                label21.Visible = true;
                label21.Location = new Point(363, 21);//重新设置提示文字位置，向下移动，原因：此控件与Lshowshowshow“此商品当前无卖方！”控件在同一个位置
                label19.Visible = false;
                labelzdjjpl.Visible = false;
                ////有效截止日期
                //lblYXJZRQ.Visible = true;
                //dateTimePicker1.Visible = true;
                ////dateTimePicker1.MinDate = DateTime.Now;
                ////dateTimePicker1.Value = DateTime.Now;
                dateTimePicker1.MinDate = PublicDS.PublisTimeServer;
                dateTimePicker1.Value = PublicDS.PublisTimeServer;
            }
            else
            {
                label21.Width = 300;
                Lshowshowshow.Visible = false;
                label21.Text = "当前卖方最低投标价：" + ht_Main["最低价格"].ToString() + "元";
                label21.Visible = true;
                label19.Visible = false;
                label19.Text = ht_Main["最低价格"].ToString() + "元";
                labelzdjjpl.Visible = true;

                label21.Location = new Point(362, 3);//重新设置提示文字位置，向上移动

                ////有效截止日期
                //lblYXJZRQ.Visible = false;
                //dateTimePicker1.Visible = false;
            }
            #endregion

            #region 展示当前最大的经济批量
            if (Convert.ToInt32(ht_Main["卖家批量"]) == 0)
            {
                //label20.Text = "此商品当前无卖方。您的《预订单》若在有";  wyh
               // label24.Text = "效期内未中标，则于“所发布信息有效截止日期”"; wyh
               // label22.Text = "自动撤销，对应订金同时自动解冻。";wyh
                //label20.Text = "当前平台中的最大经济批量：";
                //label24.Text = ht_Main["平台批量"].ToString() + ht_Main["计价单位"].ToString();
                label24.Tag = ht_Main["平台批量"].ToString();
                label20.Visible = true;
              //  labelzdjjpl.Visible  =true;
                label24.Visible = true;
                label22.Visible = true;
            }
            else
            {
                // label20.Text = "当前卖方中的最大经济批量：" + ht_Main["卖家批量"].ToString() + ht_Main["计价单位"].ToString(); wyh                              
                //label20.Text = "当前卖方中的最大经济批量：";
                //label24.Text = ht_Main["卖家批量"].ToString() + ht_Main["计价单位"].ToString();
                labelzdjjpl.Text = "当前卖方中的最大经济批量：" + ht_Main["卖家批量"].ToString() + ht_Main["计价单位"].ToString() + "            ";
                label24.Tag = ht_Main["卖家批量"].ToString();
                label20.Visible = true;
                labelzdjjpl.Visible = true;
                //label24.Visible = false;wyh
               // label22.Visible = false;wyh
            }
            #endregion


            this.Invalidate(true);
        }

        /// <summary>
        /// 输入买入价格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNMRJG_KeyUp(object sender, KeyEventArgs e)
        {
            GetNDGJE();
        }

        /// <summary>
        /// 输入买入数量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNDGSL_KeyUp(object sender, KeyEventArgs e)
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

           // this.txtNDGJE.Text = (money * amount).ToString("#0.00").Length > 10 ? "0.00" : (money * amount).ToString("#0.00");//订购金额
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

        /// <summary>
        /// 重填按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            reset();
        }

        /// <summary>
        /// 存入草稿箱
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCRCGX_Click(object sender, EventArgs e)
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
                return;
            }
            if (txtNDGSL.Text.Trim() == "")
            {
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add("您的拟订购数量不能为空！");
                if (label21.Text.IndexOf("最大经济批量") > -1) //wyh
                {
                    al.Add("拟订购数量需大于等于当前平台设定的最大的经济批量，否则预订单无法下达。");
                }
                else
                {
                    al.Add("拟订购数量需大于等于当前竞标卖方中设定的最大的经济批量，否则预订单无法下达。");
                }
                FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "", al);
                fm.ShowDialog();
                return;
            }
            if (Convert.ToInt64(txtNDGSL.Text) == 0)
            {
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add("您的拟订购数量不能为零！");
                if (label21.Text.IndexOf("最大经济批量") > -1) //wyh
                {
                    al.Add("拟订购数量需大于等于当前平台设定的最大的经济批量，否则预订单无法下达。");
                }
                else
                {
                    al.Add("拟订购数量需大于等于当前竞标卖方中设定的最大的经济批量，否则预订单无法下达。");
                }
                FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "", al);
                fm.ShowDialog();
                return;
            }


            long pl = Convert.ToInt64(label24.Tag);
            if (Convert.ToInt64(txtNDGSL.Text) < pl)
            {
                ArrayList al = new ArrayList();
                al.Add("");
                if (label21.Text.IndexOf("最大经济批量") > -1) //wyh
                {
                    al.Add("拟订购数量需大于等于当前平台设定的最大的经济批量，否则预订单无法下达！");
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
                al.Add("您的收货区域不能为空！");
                FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "", al);
                fm.ShowDialog();
                return ;
            }


            if ((Convert.ToDouble(txtNMRJG.Text) * Convert.ToInt64(txtNDGSL.Text)).ToString("#0.00").Length > 10)
            {
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add("金额过大。");
                FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "", al);
                fm.ShowDialog();
                return ;
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
            panelTJQ.Enabled = false;
            PBload.Location = new Point(btnCRCGX.Location.X + panel1.Location.X+flowLayoutPanel1.Location.X + btnCRCGX.Width + 7, btnCRCGX.Location.Y + panel15.Location.Y+flowLayoutPanel1.Location.Y );

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

        //处理非线程创建的控件
        private void CG_Invoke(Hashtable OutPutHT)
        {
            //重新开放提交区域,并滚动条强制置顶
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
                //reset();
            }
 

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
                if (label21.Text.IndexOf("最大经济批量") > -1) //wyh
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
                if (label21.Text.IndexOf("最大经济批量") > -1) //wyh
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


            long pl = Convert.ToInt64(label24.Tag);
            if (Convert.ToInt64(txtNDGSL.Text) < pl)
            {
                ArrayList al = new ArrayList();
                al.Add("");
                if (label21.Text.IndexOf("最大经济批量") > -1) //wyh
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
        /// 电子购货合同
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LLlishi_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Hashtable htcs = new Hashtable();
            htcs["数据地址"] = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[2] + "/pingtaiservices/mobanrun/FMPTDZGHHT.aspx";
            XSRM xs = new XSRM("电子购货合同样本", htcs);
            xs.ShowDialog();
        }





    }
}
