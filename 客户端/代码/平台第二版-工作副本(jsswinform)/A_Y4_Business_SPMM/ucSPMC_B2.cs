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
using System.Text.RegularExpressions;

namespace 客户端主程序.SubForm.NewCenterForm.SUBUC.SPMM
{
    public partial class ucSPMC_B2 : UserControl
    {
        /// <summary>
        /// 某一条商品信息
        /// </summary>
        Hashtable ht_Main = null;
        /// <summary>
        /// 卖家选择的区域信息
        /// </summary>
        Hashtable ht_QY =null;
        /// <summary>
        /// 卖家商品的资质信息（路径）
        /// </summary>
        Hashtable ht_ZZ = new Hashtable();
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
        public bool bing = true;
        public ucSPMC_B2()
        {
            InitializeComponent();
            label26.Visible = false;
            label27.Visible = false;
            label25.Visible = false;
            label28.Visible = false;
            ucCityList1.initdefault();
            #region 开启获取经纪人管理机构线程
            string DLYX = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString();//当前用户的登录邮箱
            Hashtable ht = new Hashtable();
            ht["DLYX"] = DLYX;
            G_B g = new G_B(ht, new delegateForThread(GetGLJJRGLJG));
            Thread trd = new Thread(new ThreadStart(g.GetGLJJRGLJG));
            trd.IsBackground = true;
            trd.Start();
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
            //禁用提交区域并开启进度
            if (!ValSubInfo())
                return;




            Hashtable ht = new Hashtable();
            ht["FLAG"] = "预执行";
            ht["DLYX"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString();
            ht["JSZHLX"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["结算账户类型"].ToString();
            ht["MJJSBH"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["卖家角色编号"].ToString();
            ht["GLJJRYX"] = PublicDS.PublisDsUser.Tables["关联信息"].Rows[0]["关联经纪人登陆账号"].ToString();
            ht["GLJJRYHM"] = PublicDS.PublisDsUser.Tables["关联信息"].Rows[0]["关联经纪人用户名"].ToString();
            ht["GLJJRJSBH"] = PublicDS.PublisDsUser.Tables["关联信息"].Rows[0]["关联经纪人编号"].ToString();
            ht["GLJJRPTGLJG"] = GLJJRPTGLJG;//关联经纪人平台管理机构
            ht["SLZM"] = this.ucTextBox_slv.Text.Trim(); //this.uCshangchuan1.UpItem.Items[0].SubItems[1].Text;//税率证明
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
            //wyh 2013.0906 add
            ht["省市区"] = ucCityList1.SelectedItem[0] + "|" + ucCityList1.SelectedItem[1] + "|" + ucCityList1.SelectedItem[2];
            //InPutHT["市"] = ucCityList1.SelectedItem[1];
           // InPutHT["区"] = ucCityList1.SelectedItem[2];
            ht["其他资质"] = ht_ZZ["其它资质"];//这个是以哈希表的形式存的
            panel1.Enabled = false;
            panelTJQ.Enabled = false;
            PBload.Location = new Point(btnSave.Location.X+panel1.Location.X+btnSave.Width+10 ,btnSave.Location.Y+panel18.Location.Y+10);
            PBload.Visible = true;
            //演示新标准
            SRT_demo_Run(ht);
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
                    al.Add(showstr);//请您确认本次所提供的各项资料与信息均真实有效，并愿意为此承担相应法律责任；同时该一经提交***元的投标保证金。
                    FormAlertMessage fm = new FormAlertMessage("确定取消", "问号", "中国商品批发交易平台", al);
                    DialogResult boxresult = fm.ShowDialog();
                    if (boxresult == DialogResult.Yes)
                    {
                        if (!ValSubInfo())
                            return;
                        //真正提交业务单据
                        #region 提交预订单
                        panel1.Enabled = false;
                        panelTJQ.Enabled = false;
                        PBload.Visible = true;
                        Hashtable ht = new Hashtable();
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
                        ht["SLZM"] = this.ucTextBox_slv.Text.Trim();//this.uCshangchuan1.UpItem.Items[0].SubItems[1].Text;//税率证明
                        ht["MJTBBZJBL"] = MJDJDJBL;//卖家投标保证金比率
                        ht["MJTBBZJZXZ"] = MJTBBZJZXZ;//卖家的那个最小值。。
                        ht["DJTBBZJ"] = txtDJTBBZJ.Text;//冻结的投标保证金
                        ht["ZLBZYZM"] = ht_ZZ["质量标准与证明"];
                        ht["CPJCBG"] = ht_ZZ["产品检测报告"];
                        ht["PGZFZRFLCNS"] = ht_ZZ["品管总负责人法律承诺书"];
                        ht["FDDBRCNS"] = ht_ZZ["法定代表人承诺书"];
                        ht["SHFWGDYCN"] = ht_ZZ["售后服务规定与承诺"];
                        ht["CPSJSQS"] = ht_ZZ["产品送检授权书"];
                        //wyh 2013.0906 add
                        ht["省市区"] = ucCityList1.SelectedItem[0] + "|" + ucCityList1.SelectedItem[1] + "|" + ucCityList1.SelectedItem[2];
                        ht["其他资质"] = ht_ZZ["其它资质"];//这个是以哈希表的形式存的
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
                       // al.Add(strAy[1] + "。");
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
            ucSPMC_B2 u = new ucSPMC_B2();
            u.bing = false;
            u.Dock = DockStyle.Fill;//铺满
            u.AutoScroll = true;//出现滚动条
            u.BackColor = Color.AliceBlue;
            
            //清理控件
            Panel P = (Panel)this.Parent;
            P.Controls.Clear();
            P.Controls.Add(u); //加入到某一个panel中
            u.Show();//显示出来
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
     
        /// <summary>
        /// 选择商品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelect_Click(object sender, EventArgs e)
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

        private bool dafwefasdfs(object obj)
        {
            if (obj == null || obj.ToString().Trim() == "")
                return false;
            else
                return true;
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

            this.txtJJPL.Text = "";
            this.txtTPNGL.Text = "";
            this.txtTBJG.Text = "";
            this.txtTBJE.Text = "";
            this.txtDJTBBZJ.Text = "";
            panelTBJE.Visible = false;
            panelDJTBBZJ.Visible = false;

            //this.txtSPMC.
            btnSelectQY.Text = "选       择";
            ht_ZZ = new Hashtable();//重新初始化资质列表
            ht_ZZ["质量标准与证明"] = ht_Main["ZLBZYZM"];
            ht_ZZ["产品检测报告"] = ht_Main["CPJCBG"];
            ht_ZZ["品管总负责人法律承诺书"] = ht_Main["PGZFZRFLCNS"];
            ht_ZZ["法定代表人承诺书"] = ht_Main["FDDBRCNS"];
            ht_ZZ["售后服务规定与承诺"] = ht_Main["SHFWGDYCN"];
            ht_ZZ["产品送检授权书"] = ht_Main["CPSJSQS"];//6个必填资质
            //string[] QTZZStr = ht_Main["QTZZ"].ToString().Split(new char[]{'|'},StringSplitOptions.RemoveEmptyEntries);//其他资质字符串
            ht_ZZ["其它资质"] = null;//其他资质项，仅做模拟用。
            if ((str_ZZ == null || str_ZZ.Trim() == "" || str_ZZ.Trim() == "|") && dafwefasdfs(ht_Main["ZLBZYZM"]) && dafwefasdfs(ht_Main["CPJCBG"]) && dafwefasdfs(ht_Main["PGZFZRFLCNS"]) && dafwefasdfs(ht_Main["FDDBRCNS"]) && dafwefasdfs(ht_Main["SHFWGDYCN"]) && dafwefasdfs(ht_Main["CPSJSQS"]))
            {
                btnSCZL.Text = "查 看 资 质";
            }
            else
            {
                btnSCZL.Text = "上 传 资 质";
            }
            //if (QTZZStr.Length > 0)
            //{
            //    Hashtable htQTZZs = new Hashtable();
            //    for (int i = 0; i < QTZZStr.Length; i++)
            //    {
            //        htQTZZs.Add("资质" + (i + 1), QTZZStr[i]);
            //    }
            //    ht_ZZ["其它资质"] = htQTZZs;
            //}
            if (ht_Main["SLZM"] != null && ht_Main["SLZM"].ToString().Trim() != "")
            {
                ListView lv = uCshangchuan1.UpItem;
                lv.Clear();
                lv.Items.Add(new ListViewItem(new string[] { "", ht_Main["SLZM"].ToString(), "", "" }));
            }

            ht_QY = null;
            #region 展示当前最低投标价格
            if (ht_Main["最低价格"].ToString() == "0.00")
            {
                label27.Width = 170;
                label27.Text = "当前无卖方参与此商品的投标";
                label27.Visible = false ;
                label26.Visible = false;
            }
            else
            {
                label27.Width = 300;
                label27.Text = "当前卖方最低投标价：" + ht_Main["最低价格"].ToString() + "元";
                label27.Visible = true;
                label26.Visible = false;
                label26.Text = ht_Main["最低价格"].ToString() + "元";
            }
            #endregion
            //label28 label25
            #region 展示当前最大的经济批量
            if (Convert.ToInt32(ht_Main["卖家批量"]) == 0)
            {
                label28.Text = "平台设定的该商品最大经济批量：" + ht_Main["平台批量"].ToString() + ht_Main["计价单位"].ToString();
                label28.Width = 340;
                //label28.Text = "当前平台中的最大经济批量：";
                //label25.Text = ht_Main["平台批量"].ToString() + ht_Main["计价单位"].ToString();
                label28.Visible = true;
                label25.Visible = false;
            }
            else
            {
                label28.Text = "当前卖方中的最大经济批量：";
                label25.Text = ht_Main["卖家批量"].ToString() + ht_Main["计价单位"].ToString();
                label28.Visible = true;
                label25.Visible = true;
            }
            #endregion

            label35.Visible = true;
            label34.Visible = true;

            panelSPMC.Visible = true;
            panelGG.Visible = true;
            panelJJDW.Visible = true;
            panelHTQX.Visible = true;
            panelPTSDDZDJJPL.Visible = true;




            this.Invalidate(true);
            
        }

        private void ucSPMC_B2_Load(object sender, EventArgs e)
        {
            

            //将滚动条置顶
            UPUP();
            //设置进度条的位置，放到按钮旁边
            PBload.Location = new Point(panel1.Location.X + flowLayoutPanel1.Location.X + panel18.Location.X + btnSave.Location.X + btnSave.Width + 25, panel1.Location.Y + flowLayoutPanel1.Location.Y + panel18.Location.Y + btnSave.Location.Y + btnSave.Location.Y + panelTJQ.Height);

            string dlyx = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString();
            Hashtable ht = new Hashtable();
            ht["dlyx"] = dlyx;
            //CanSell_Run(ht);

            //2013.12.06 wyh add 鼠标略过显示文字
            toolTip1.AutoPopDelay = 30000;
            toolTip1.InitialDelay = 500;
            toolTip1.ReshowDelay = 0;
            toolTip1.ShowAlways = true;
           // string Strtext = "1、一个商品一个轮次的竞标中只有一个最低价标的中标，价格相同时投标时间早的优先，中标买方收货区域将在该标的供货区域范围内。\n2、卖方设定的“经济批量”不得高于“平台设定的最大经济批量”；合同期限为“三个月”或“一年”的，“投标拟售量”不得低于“平台设定的最\n 大经济批量”的10倍；平台设定的最大经济批量”的10倍；合同期限为“即时”的，“投标拟售量”不得低于“平台设定的最大经济批量”。\n3、投标单下达的同时系统将自动冻结投标金额的0.5‰（最低不少于1000元/次）作为投标保证金；该投标保证金将在投标单撤销或定标后\n予以解冻；如未履行定标程序，\n后予以解冻；如未履行定标程序，投标保证金将被扣罚，补偿给各中标买方；合同期限为“即时“的投标单，\n可能会一部分中标（不低于卖方设定的一个经济批量），剩余部分将自动转入下一次交易撮合\n低于卖方设定的一个经济批量），剩余部分将\n自动转入下一次交易撮合（若剩余部分数量低于卖方设定的一个经济批量，则自动撤销）。\n4、按有关协议和交易规定，投标单一旦中标，即\n为交易双方正式订立《电子购货合同》。\n5、为保证快速履约，合同期限为“即时”的投标单中标后，卖方须在72小时内自主冻结履约保证金予以定标。\n6、卖方在履行交货时，须按照买方要求开具增值税专用发票或增值税普通发票（非一般纳税人单位须向税务机关申请代开）。\n  ";

            string Strtext = "1、一个商品一个轮次的竞标中只有一个最低价标的中标，价格相同时投标时间早的优先，中标买方收货区域将在该标的供货区域范围内。\n2、卖方设定的“经济批量”不得高于“平台设定的最大经济批量”；合同期限为“三个月”或“一年”的，“投标拟售量”不得低于“平台设定\n   的最大经济批量”的10倍；平台设定的最大经济批量”的10倍；合同期限为“即时”的，“投标拟售量”不得低于“平台设定的最大经济批量”。\n3、投标单下达的同时系统将自动冻结投标金额的0.5‰（最低不少于1000元/次）作为投标保证金；该投标保证金将在投标单撤销或定标后\n予以解冻；如未履行定标程序，投标保证金将被扣罚，补偿给各中标买方；合同期限为“即时“的投标单，可能会一部分中标（不低于卖方\n设定的一个经济批量），剩余部分将自动转入下一次交易撮合低于卖方设定的一个经济批量），剩余部分将自动转入下一次交易撮合\n（若剩余部分数量低于卖方设定的一个经济批量，则自动撤销）。\n4、按有关协议和交易规定，投标单一旦中标，即为交易双方正式订立《电子购货合同》。\n5、为保证快速履约，合同期限为“即时”的投标单中标后，卖方须在72小时内自主冻结履约保证金予以定标。\n6、卖方在履行交货时，须按照买方要求开具增值税专用发票或增值税普通发票（非一般纳税人单位须向税务机关申请代开）。";
             toolTip1.SetToolTip(this.label33, Strtext);
             toolTip1.SetToolTip(this.pictureBox1, Strtext);
           // this.toolTip1.Show(Strtext, this.label33);
            this.toolTip1.ToolTipTitle = "卖出须知";
           
           // toolTip1.SetToolTip(this.radYWTZB, "银行经纪人和富美集团总部员工请选择“平台总部” ");
           // toolTip1.SetToolTip(this.radGXTW, "参加“全国大学生课余创业实践活动”的高校师生请选择“高校团委”"); 


            //隐藏
            label35.Visible = false;
            label34.Visible = false;

            panelSPMC.Visible = false;
            panelGG.Visible = false;
            panelJJDW.Visible = false;
            panelHTQX.Visible = false;
            panelPTSDDZDJJPL.Visible = false;
            panelTBJE.Visible = false;
            panelDJTBBZJ.Visible = false;

            if (PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["结算账户类型"].ToString() == "经纪人交易账户" && bing)
            {
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add("您是经纪人交易账户，无法下达投标单！");
                FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "中国商品批发交易平台", al);
                fm.ShowDialog();

            }
        }
 


        /// <summary>
        /// 上传资质
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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


        #region 选择区域的线程和回调
        /// <summary>
        /// 选择区域按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectQY_Click(object sender, EventArgs e)
        {
            fmXZFHQY fm = new fmXZFHQY((new delegateForThread(QY_Select)), ht_QY);
            fm.ShowDialog();
        }
        private void QY_Select(Hashtable OutPutHT)
        {
            try { Invoke(new delegateForThreadShow(QY_Select_Invoke), new Hashtable[] { OutPutHT }); }
            catch (Exception ex) { Support.StringOP.WriteLog("委托回调错误：" + ex.ToString()); }
        }
        /// <summary>
        /// 处理非线程创建的控件
        /// </summary>
        /// <param name="OutPutHT"></param>
        private void QY_Select_Invoke(Hashtable OutPutHT)
        {
            ht_QY = OutPutHT;
            this.btnSelectQY.Text = "查       看";
        }
        #endregion


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
            //this.txtTBJE.Text = (money * amount).ToString("#0.00").Length > 10 ? "0.00" : (money * amount).ToString("#0.00");//订购金额
            this.txtTBJE.Text =  (money * amount).ToString("#0.00");//订购金额
            if (this.txtTBJE.Text.Trim() != "0.00")
            {
                this.txtDJTBBZJ.Text = Math.Round((money * amount * MJDJDJBL), 2) >= MJTBBZJZXZ ? Math.Round((money * amount * MJDJDJBL), 2).ToString("#0.00") : MJTBBZJZXZ.ToString("#0.00");//冻结订金的金额
            }
            else
            {
                this.txtDJTBBZJ.Text = "0.00";
            }

            panelTBJE.Visible = true;
            panelDJTBBZJ.Visible = true;
        }

        private void txtTPNGL_KeyUp(object sender, KeyEventArgs e)
        {
            GetNDGJE();
        }

        private void txtTBJG_KeyUp(object sender, KeyEventArgs e)
        {
            GetNDGJE();
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucSPMC_B2));
            this.panel1 = new System.Windows.Forms.Panel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.linkLabel4 = new System.Windows.Forms.LinkLabel();
            this.label33 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label34 = new System.Windows.Forms.Label();
            this.btnSelect = new System.Windows.Forms.Label();
            this.label35 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panelSPMC = new System.Windows.Forms.Panel();
            this.txtSPMC = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.panelGG = new System.Windows.Forms.Panel();
            this.txtGG = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.panelJJDW = new System.Windows.Forms.Panel();
            this.txtJJDW = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.panelHTQX = new System.Windows.Forms.Panel();
            this.txtHTQX = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.panelPTSDDZDJJPL = new System.Windows.Forms.Panel();
            this.txtPTZDSDJJPL = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.panel9 = new System.Windows.Forms.Panel();
            this.txtJJPL = new 客户端主程序.SubForm.UCTextBox(this.components);
            this.panel19 = new System.Windows.Forms.Panel();
            this.basicButton3 = new Com.Seezt.Skins.BasicButton();
            this.label25 = new System.Windows.Forms.Label();
            this.basicButton4 = new Com.Seezt.Skins.BasicButton();
            this.label26 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.panel10 = new System.Windows.Forms.Panel();
            this.txtTPNGL = new 客户端主程序.SubForm.UCTextBox(this.components);
            this.label14 = new System.Windows.Forms.Label();
            this.panel11 = new System.Windows.Forms.Panel();
            this.TipsInputEmail = new System.Windows.Forms.RichTextBox();
            this.txtTBJG = new 客户端主程序.SubForm.UCTextBox(this.components);
            this.label15 = new System.Windows.Forms.Label();
            this.panel12 = new System.Windows.Forms.Panel();
            this.label31 = new System.Windows.Forms.Label();
            this.ucTextBox_slv = new 客户端主程序.SubForm.UCTextBox(this.components);
            this.uCshangchuan1 = new 客户端主程序.SubForm.NewCenterForm.UCshangchuan();
            this.label32 = new System.Windows.Forms.Label();
            this.panelTBJE = new System.Windows.Forms.Panel();
            this.txtTBJE = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.panelDJTBBZJ = new System.Windows.Forms.Panel();
            this.txtDJTBBZJ = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.panel15 = new System.Windows.Forms.Panel();
            this.ucCityList1 = new 客户端主程序.SubForm.UCCityList();
            this.label30 = new System.Windows.Forms.Label();
            this.panel16 = new System.Windows.Forms.Panel();
            this.btnSelectQY = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.panel17 = new System.Windows.Forms.Panel();
            this.btnSCZL = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.panel18 = new System.Windows.Forms.Panel();
            this.btnCRCGX = new Com.Seezt.Skins.BasicButton();
            this.btnSave = new Com.Seezt.Skins.BasicButton();
            this.btnCancel = new Com.Seezt.Skins.BasicButton();
            this.PBload = new System.Windows.Forms.PictureBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panelTJQ = new System.Windows.Forms.Panel();
            this.label19 = new System.Windows.Forms.Label();
            this.LLlishi = new System.Windows.Forms.LinkLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.BSave = new Com.Seezt.Skins.BasicButton();
            this.Breset = new Com.Seezt.Skins.BasicButton();
            this.panel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel3.SuspendLayout();
            this.panelSPMC.SuspendLayout();
            this.panelGG.SuspendLayout();
            this.panelJJDW.SuspendLayout();
            this.panelHTQX.SuspendLayout();
            this.panelPTSDDZDJJPL.SuspendLayout();
            this.panel9.SuspendLayout();
            this.panel19.SuspendLayout();
            this.panel10.SuspendLayout();
            this.panel11.SuspendLayout();
            this.panel12.SuspendLayout();
            this.panelTBJE.SuspendLayout();
            this.panelDJTBBZJ.SuspendLayout();
            this.panel15.SuspendLayout();
            this.panel16.SuspendLayout();
            this.panel17.SuspendLayout();
            this.panel18.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PBload)).BeginInit();
            this.panelTJQ.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.flowLayoutPanel1);
            this.panel1.Location = new System.Drawing.Point(10, 10);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(769, 527);
            this.panel1.TabIndex = 95;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.panel2);
            this.flowLayoutPanel1.Controls.Add(this.panel3);
            this.flowLayoutPanel1.Controls.Add(this.panelSPMC);
            this.flowLayoutPanel1.Controls.Add(this.panelGG);
            this.flowLayoutPanel1.Controls.Add(this.panelJJDW);
            this.flowLayoutPanel1.Controls.Add(this.panelHTQX);
            this.flowLayoutPanel1.Controls.Add(this.panelPTSDDZDJJPL);
            this.flowLayoutPanel1.Controls.Add(this.panel9);
            this.flowLayoutPanel1.Controls.Add(this.panel10);
            this.flowLayoutPanel1.Controls.Add(this.panel11);
            this.flowLayoutPanel1.Controls.Add(this.panel12);
            this.flowLayoutPanel1.Controls.Add(this.panelTBJE);
            this.flowLayoutPanel1.Controls.Add(this.panelDJTBBZJ);
            this.flowLayoutPanel1.Controls.Add(this.panel15);
            this.flowLayoutPanel1.Controls.Add(this.panel16);
            this.flowLayoutPanel1.Controls.Add(this.panel17);
            this.flowLayoutPanel1.Controls.Add(this.panel18);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(763, 509);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.pictureBox1);
            this.panel2.Controls.Add(this.linkLabel4);
            this.panel2.Controls.Add(this.label33);
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(756, 30);
            this.panel2.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(210, 6);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(22, 19);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 110;
            this.pictureBox1.TabStop = false;
            // 
            // linkLabel4
            // 
            this.linkLabel4.AutoSize = true;
            this.linkLabel4.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabel4.Location = new System.Drawing.Point(316, 10);
            this.linkLabel4.Name = "linkLabel4";
            this.linkLabel4.Size = new System.Drawing.Size(149, 12);
            this.linkLabel4.TabIndex = 112;
            this.linkLabel4.TabStop = true;
            this.linkLabel4.Text = "查看《电子购货合同》样本";
            this.linkLabel4.VisitedLinkColor = System.Drawing.Color.Goldenrod;
            this.linkLabel4.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LLlishi_LinkClicked_1);
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Location = new System.Drawing.Point(236, 10);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(77, 12);
            this.label33.TabIndex = 111;
            this.label33.Text = "查看卖出须知";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label34);
            this.panel3.Controls.Add(this.btnSelect);
            this.panel3.Controls.Add(this.label35);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Location = new System.Drawing.Point(0, 30);
            this.panel3.Margin = new System.Windows.Forms.Padding(0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(756, 35);
            this.panel3.TabIndex = 1;
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.ForeColor = System.Drawing.Color.Red;
            this.label34.Location = new System.Drawing.Point(410, 19);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(269, 12);
            this.label34.TabIndex = 115;
            this.label34.Text = "如需更改，请进入“选择商品”界面，重新选定。";
            // 
            // btnSelect
            // 
            this.btnSelect.BackColor = System.Drawing.Color.PowderBlue;
            this.btnSelect.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSelect.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSelect.ForeColor = System.Drawing.Color.Black;
            this.btnSelect.Location = new System.Drawing.Point(207, 6);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(181, 23);
            this.btnSelect.TabIndex = 101;
            this.btnSelect.Text = "选       择";
            this.btnSelect.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.ForeColor = System.Drawing.Color.Red;
            this.label35.Location = new System.Drawing.Point(409, 4);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(281, 12);
            this.label35.TabIndex = 114;
            this.label35.Text = "请再次确认您选择的“商品名称”与“合同期限”；";
            // 
            // label3
            // 
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(123, 9);
            this.label3.Margin = new System.Windows.Forms.Padding(3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 18);
            this.label3.TabIndex = 100;
            this.label3.Text = "拟卖出商品：";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panelSPMC
            // 
            this.panelSPMC.Controls.Add(this.txtSPMC);
            this.panelSPMC.Controls.Add(this.label5);
            this.panelSPMC.Location = new System.Drawing.Point(0, 65);
            this.panelSPMC.Margin = new System.Windows.Forms.Padding(0);
            this.panelSPMC.Name = "panelSPMC";
            this.panelSPMC.Size = new System.Drawing.Size(756, 23);
            this.panelSPMC.TabIndex = 2;
            // 
            // txtSPMC
            // 
            this.txtSPMC.AutoSize = true;
            this.txtSPMC.Font = new System.Drawing.Font("Tahoma", 9F);
            this.txtSPMC.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.txtSPMC.Location = new System.Drawing.Point(207, 5);
            this.txtSPMC.Name = "txtSPMC";
            this.txtSPMC.Size = new System.Drawing.Size(55, 14);
            this.txtSPMC.TabIndex = 104;
            this.txtSPMC.Text = "自动带出";
            // 
            // label5
            // 
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(123, 4);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 18);
            this.label5.TabIndex = 69;
            this.label5.Text = " 商品名称：";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panelGG
            // 
            this.panelGG.Controls.Add(this.txtGG);
            this.panelGG.Controls.Add(this.label7);
            this.panelGG.Location = new System.Drawing.Point(0, 88);
            this.panelGG.Margin = new System.Windows.Forms.Padding(0);
            this.panelGG.Name = "panelGG";
            this.panelGG.Size = new System.Drawing.Size(756, 23);
            this.panelGG.TabIndex = 3;
            // 
            // txtGG
            // 
            this.txtGG.AutoSize = true;
            this.txtGG.Font = new System.Drawing.Font("Tahoma", 9F);
            this.txtGG.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.txtGG.Location = new System.Drawing.Point(207, 5);
            this.txtGG.Name = "txtGG";
            this.txtGG.Size = new System.Drawing.Size(55, 14);
            this.txtGG.TabIndex = 105;
            this.txtGG.Text = "自动带出";
            // 
            // label7
            // 
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Location = new System.Drawing.Point(123, 4);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(80, 18);
            this.label7.TabIndex = 38;
            this.label7.Text = "规    格：";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panelJJDW
            // 
            this.panelJJDW.Controls.Add(this.txtJJDW);
            this.panelJJDW.Controls.Add(this.label11);
            this.panelJJDW.Location = new System.Drawing.Point(0, 111);
            this.panelJJDW.Margin = new System.Windows.Forms.Padding(0);
            this.panelJJDW.Name = "panelJJDW";
            this.panelJJDW.Size = new System.Drawing.Size(756, 23);
            this.panelJJDW.TabIndex = 3;
            // 
            // txtJJDW
            // 
            this.txtJJDW.AutoSize = true;
            this.txtJJDW.Font = new System.Drawing.Font("Tahoma", 9F);
            this.txtJJDW.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.txtJJDW.Location = new System.Drawing.Point(207, 5);
            this.txtJJDW.Name = "txtJJDW";
            this.txtJJDW.Size = new System.Drawing.Size(55, 14);
            this.txtJJDW.TabIndex = 105;
            this.txtJJDW.Text = "自动带出";
            // 
            // label11
            // 
            this.label11.ForeColor = System.Drawing.Color.Black;
            this.label11.Location = new System.Drawing.Point(125, 4);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(78, 18);
            this.label11.TabIndex = 53;
            this.label11.Text = "计价单位：";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panelHTQX
            // 
            this.panelHTQX.Controls.Add(this.txtHTQX);
            this.panelHTQX.Controls.Add(this.label6);
            this.panelHTQX.Location = new System.Drawing.Point(0, 134);
            this.panelHTQX.Margin = new System.Windows.Forms.Padding(0);
            this.panelHTQX.Name = "panelHTQX";
            this.panelHTQX.Size = new System.Drawing.Size(756, 23);
            this.panelHTQX.TabIndex = 3;
            // 
            // txtHTQX
            // 
            this.txtHTQX.AutoSize = true;
            this.txtHTQX.Font = new System.Drawing.Font("Tahoma", 9F);
            this.txtHTQX.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.txtHTQX.Location = new System.Drawing.Point(207, 5);
            this.txtHTQX.Name = "txtHTQX";
            this.txtHTQX.Size = new System.Drawing.Size(55, 14);
            this.txtHTQX.TabIndex = 105;
            this.txtHTQX.Text = "自动带出";
            // 
            // label6
            // 
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(123, 4);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(80, 18);
            this.label6.TabIndex = 34;
            this.label6.Text = "合同期限：";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panelPTSDDZDJJPL
            // 
            this.panelPTSDDZDJJPL.Controls.Add(this.txtPTZDSDJJPL);
            this.panelPTSDDZDJJPL.Controls.Add(this.label23);
            this.panelPTSDDZDJJPL.Location = new System.Drawing.Point(0, 157);
            this.panelPTSDDZDJJPL.Margin = new System.Windows.Forms.Padding(0);
            this.panelPTSDDZDJJPL.Name = "panelPTSDDZDJJPL";
            this.panelPTSDDZDJJPL.Size = new System.Drawing.Size(756, 23);
            this.panelPTSDDZDJJPL.TabIndex = 3;
            // 
            // txtPTZDSDJJPL
            // 
            this.txtPTZDSDJJPL.AutoSize = true;
            this.txtPTZDSDJJPL.Font = new System.Drawing.Font("Tahoma", 9F);
            this.txtPTZDSDJJPL.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.txtPTZDSDJJPL.Location = new System.Drawing.Point(207, 5);
            this.txtPTZDSDJJPL.Name = "txtPTZDSDJJPL";
            this.txtPTZDSDJJPL.Size = new System.Drawing.Size(55, 14);
            this.txtPTZDSDJJPL.TabIndex = 105;
            this.txtPTZDSDJJPL.Text = "自动带出";
            // 
            // label23
            // 
            this.label23.ForeColor = System.Drawing.Color.Black;
            this.label23.Location = new System.Drawing.Point(51, 4);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(152, 18);
            this.label23.TabIndex = 91;
            this.label23.Text = "平台设定的最大经济批量：";
            this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel9
            // 
            this.panel9.Controls.Add(this.txtJJPL);
            this.panel9.Controls.Add(this.panel19);
            this.panel9.Controls.Add(this.label22);
            this.panel9.Location = new System.Drawing.Point(0, 180);
            this.panel9.Margin = new System.Windows.Forms.Padding(0);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(756, 33);
            this.panel9.TabIndex = 4;
            // 
            // txtJJPL
            // 
            this.txtJJPL.BackColor = System.Drawing.Color.White;
            this.txtJJPL.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.txtJJPL.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtJJPL.CanPASTE = true;
            this.txtJJPL.Counts = 2;
            this.txtJJPL.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtJJPL.ForeColor = System.Drawing.Color.Black;
            this.txtJJPL.HotColor = System.Drawing.Color.CornflowerBlue;
            this.txtJJPL.Location = new System.Drawing.Point(207, 7);
            this.txtJJPL.MaxLength = 10;
            this.txtJJPL.Name = "txtJJPL";
            this.txtJJPL.OpenZS = true;
            this.txtJJPL.Size = new System.Drawing.Size(182, 22);
            this.txtJJPL.TabIndex = 102;
            this.txtJJPL.TextNtip = "";
            this.txtJJPL.WordWrap = false;
            // 
            // panel19
            // 
            this.panel19.Controls.Add(this.basicButton3);
            this.panel19.Controls.Add(this.label25);
            this.panel19.Controls.Add(this.basicButton4);
            this.panel19.Controls.Add(this.label26);
            this.panel19.Controls.Add(this.label27);
            this.panel19.Controls.Add(this.label28);
            this.panel19.Location = new System.Drawing.Point(398, 1);
            this.panel19.Name = "panel19";
            this.panel19.Size = new System.Drawing.Size(351, 30);
            this.panel19.TabIndex = 113;
            // 
            // basicButton3
            // 
            this.basicButton3.BackColor = System.Drawing.Color.LightSkyBlue;
            this.basicButton3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.basicButton3.ForeColor = System.Drawing.Color.DarkBlue;
            this.basicButton3.Location = new System.Drawing.Point(142, 556);
            this.basicButton3.Name = "basicButton3";
            this.basicButton3.Size = new System.Drawing.Size(60, 22);
            this.basicButton3.TabIndex = 49;
            this.basicButton3.Texts = "确认";
            // 
            // label25
            // 
            this.label25.Font = new System.Drawing.Font("Tahoma", 9F);
            this.label25.ForeColor = System.Drawing.Color.Red;
            this.label25.Location = new System.Drawing.Point(166, 14);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(182, 16);
            this.label25.TabIndex = 95;
            this.label25.Text = "88563434支";
            this.label25.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // basicButton4
            // 
            this.basicButton4.BackColor = System.Drawing.Color.LightSkyBlue;
            this.basicButton4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.basicButton4.ForeColor = System.Drawing.Color.DarkBlue;
            this.basicButton4.Location = new System.Drawing.Point(297, 556);
            this.basicButton4.Name = "basicButton4";
            this.basicButton4.Size = new System.Drawing.Size(60, 22);
            this.basicButton4.TabIndex = 50;
            this.basicButton4.Texts = "重填";
            // 
            // label26
            // 
            this.label26.Font = new System.Drawing.Font("Tahoma", 9F);
            this.label26.ForeColor = System.Drawing.Color.Red;
            this.label26.Location = new System.Drawing.Point(142, -2);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(118, 18);
            this.label26.TabIndex = 94;
            this.label26.Text = "12元";
            this.label26.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label27
            // 
            this.label27.Font = new System.Drawing.Font("Tahoma", 9F);
            this.label27.ForeColor = System.Drawing.Color.Red;
            this.label27.Location = new System.Drawing.Point(9, -3);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(127, 18);
            this.label27.TabIndex = 71;
            this.label27.Text = "当前卖方最低投标价：";
            this.label27.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label28
            // 
            this.label28.BackColor = System.Drawing.Color.AliceBlue;
            this.label28.Font = new System.Drawing.Font("Tahoma", 9F);
            this.label28.ForeColor = System.Drawing.Color.Red;
            this.label28.Location = new System.Drawing.Point(9, 12);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(164, 18);
            this.label28.TabIndex = 73;
            this.label28.Text = "当前卖方中的最大经济批量：";
            this.label28.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label22
            // 
            this.label22.ForeColor = System.Drawing.Color.Black;
            this.label22.Location = new System.Drawing.Point(41, 8);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(162, 18);
            this.label22.TabIndex = 89;
            this.label22.Text = " 卖方设定经济批量：";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel10
            // 
            this.panel10.Controls.Add(this.txtTPNGL);
            this.panel10.Controls.Add(this.label14);
            this.panel10.Location = new System.Drawing.Point(0, 213);
            this.panel10.Margin = new System.Windows.Forms.Padding(0);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(756, 30);
            this.panel10.TabIndex = 5;
            // 
            // txtTPNGL
            // 
            this.txtTPNGL.BackColor = System.Drawing.Color.White;
            this.txtTPNGL.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.txtTPNGL.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTPNGL.CanPASTE = true;
            this.txtTPNGL.Counts = 2;
            this.txtTPNGL.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtTPNGL.ForeColor = System.Drawing.Color.Black;
            this.txtTPNGL.HotColor = System.Drawing.Color.CornflowerBlue;
            this.txtTPNGL.Location = new System.Drawing.Point(207, 6);
            this.txtTPNGL.MaxLength = 10;
            this.txtTPNGL.Name = "txtTPNGL";
            this.txtTPNGL.OpenZS = true;
            this.txtTPNGL.Size = new System.Drawing.Size(182, 22);
            this.txtTPNGL.TabIndex = 101;
            this.txtTPNGL.TextNtip = "";
            this.txtTPNGL.WordWrap = false;
            this.txtTPNGL.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtTPNGL_KeyUp);
            // 
            // label14
            // 
            this.label14.ForeColor = System.Drawing.Color.Black;
            this.label14.Location = new System.Drawing.Point(123, 6);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(80, 18);
            this.label14.TabIndex = 59;
            this.label14.Text = "投标拟售量：";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel11
            // 
            this.panel11.Controls.Add(this.TipsInputEmail);
            this.panel11.Controls.Add(this.txtTBJG);
            this.panel11.Controls.Add(this.label15);
            this.panel11.Location = new System.Drawing.Point(0, 243);
            this.panel11.Margin = new System.Windows.Forms.Padding(0);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(756, 30);
            this.panel11.TabIndex = 6;
            // 
            // TipsInputEmail
            // 
            this.TipsInputEmail.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TipsInputEmail.Cursor = System.Windows.Forms.Cursors.Default;
            this.TipsInputEmail.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.TipsInputEmail.Location = new System.Drawing.Point(411, 8);
            this.TipsInputEmail.Name = "TipsInputEmail";
            this.TipsInputEmail.ReadOnly = true;
            this.TipsInputEmail.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.TipsInputEmail.Size = new System.Drawing.Size(173, 20);
            this.TipsInputEmail.TabIndex = 101;
            this.TipsInputEmail.Text = "此价格为含税、物流费价格";
            // 
            // txtTBJG
            // 
            this.txtTBJG.BackColor = System.Drawing.Color.White;
            this.txtTBJG.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.txtTBJG.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTBJG.CanPASTE = true;
            this.txtTBJG.Counts = 2;
            this.txtTBJG.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtTBJG.ForeColor = System.Drawing.Color.Black;
            this.txtTBJG.HotColor = System.Drawing.Color.CornflowerBlue;
            this.txtTBJG.Location = new System.Drawing.Point(207, 6);
            this.txtTBJG.MaxLength = 10;
            this.txtTBJG.Name = "txtTBJG";
            this.txtTBJG.OpenLWXS = true;
            this.txtTBJG.Size = new System.Drawing.Size(182, 22);
            this.txtTBJG.TabIndex = 100;
            this.txtTBJG.TextNtip = "";
            this.txtTBJG.WordWrap = false;
            this.txtTBJG.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtTBJG_KeyUp);
            // 
            // label15
            // 
            this.label15.ForeColor = System.Drawing.Color.Black;
            this.label15.Location = new System.Drawing.Point(125, 8);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(78, 18);
            this.label15.TabIndex = 99;
            this.label15.Text = "投标价格：";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel12
            // 
            this.panel12.Controls.Add(this.label31);
            this.panel12.Controls.Add(this.ucTextBox_slv);
            this.panel12.Controls.Add(this.uCshangchuan1);
            this.panel12.Controls.Add(this.label32);
            this.panel12.Location = new System.Drawing.Point(0, 273);
            this.panel12.Margin = new System.Windows.Forms.Padding(0);
            this.panel12.Name = "panel12";
            this.panel12.Size = new System.Drawing.Size(756, 30);
            this.panel12.TabIndex = 7;
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label31.Location = new System.Drawing.Point(269, 8);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(16, 16);
            this.label31.TabIndex = 109;
            this.label31.Text = "%";
            // 
            // ucTextBox_slv
            // 
            this.ucTextBox_slv.BackColor = System.Drawing.Color.White;
            this.ucTextBox_slv.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.ucTextBox_slv.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ucTextBox_slv.CanPASTE = true;
            this.ucTextBox_slv.Counts = 1;
            this.ucTextBox_slv.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ucTextBox_slv.ForeColor = System.Drawing.Color.Black;
            this.ucTextBox_slv.HotColor = System.Drawing.Color.CornflowerBlue;
            this.ucTextBox_slv.Location = new System.Drawing.Point(207, 5);
            this.ucTextBox_slv.MaxLength = 10;
            this.ucTextBox_slv.Name = "ucTextBox_slv";
            this.ucTextBox_slv.OpenLWXS = true;
            this.ucTextBox_slv.Size = new System.Drawing.Size(59, 22);
            this.ucTextBox_slv.TabIndex = 108;
            this.ucTextBox_slv.TextNtip = "";
            this.ucTextBox_slv.WordWrap = false;
            this.ucTextBox_slv.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ucTextBox_slv_KeyDown);
            this.ucTextBox_slv.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ucTextBox_slv_KeyPress);
            this.ucTextBox_slv.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ucTextBox_slv_KeyUp);
            // 
            // uCshangchuan1
            // 
            this.uCshangchuan1.BackColor = System.Drawing.Color.AliceBlue;
            this.uCshangchuan1.ForeColor = System.Drawing.Color.Black;
            this.uCshangchuan1.JSBH = "xx";
            this.uCshangchuan1.Location = new System.Drawing.Point(497, 6);
            this.uCshangchuan1.ManageData = "";
            this.uCshangchuan1.Margin = new System.Windows.Forms.Padding(0);
            this.uCshangchuan1.Name = "uCshangchuan1";
            this.uCshangchuan1.showB = null;
            this.uCshangchuan1.Size = new System.Drawing.Size(256, 20);
            this.uCshangchuan1.TabIndex = 107;
            this.uCshangchuan1.Visible = false;
            // 
            // label32
            // 
            this.label32.ForeColor = System.Drawing.Color.Black;
            this.label32.Location = new System.Drawing.Point(113, 7);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(90, 18);
            this.label32.TabIndex = 106;
            this.label32.Text = "卖方增值税率：";
            this.label32.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panelTBJE
            // 
            this.panelTBJE.Controls.Add(this.txtTBJE);
            this.panelTBJE.Controls.Add(this.label16);
            this.panelTBJE.Location = new System.Drawing.Point(0, 303);
            this.panelTBJE.Margin = new System.Windows.Forms.Padding(0);
            this.panelTBJE.Name = "panelTBJE";
            this.panelTBJE.Size = new System.Drawing.Size(756, 23);
            this.panelTBJE.TabIndex = 8;
            this.panelTBJE.Visible = false;
            // 
            // txtTBJE
            // 
            this.txtTBJE.AutoSize = true;
            this.txtTBJE.Font = new System.Drawing.Font("Tahoma", 9F);
            this.txtTBJE.ForeColor = System.Drawing.Color.Red;
            this.txtTBJE.Location = new System.Drawing.Point(207, 5);
            this.txtTBJE.Name = "txtTBJE";
            this.txtTBJE.Size = new System.Drawing.Size(55, 14);
            this.txtTBJE.TabIndex = 65;
            this.txtTBJE.Text = "自动带出";
            // 
            // label16
            // 
            this.label16.ForeColor = System.Drawing.Color.Black;
            this.label16.Location = new System.Drawing.Point(123, 4);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(80, 18);
            this.label16.TabIndex = 63;
            this.label16.Text = "投标金额：";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panelDJTBBZJ
            // 
            this.panelDJTBBZJ.Controls.Add(this.txtDJTBBZJ);
            this.panelDJTBBZJ.Controls.Add(this.label17);
            this.panelDJTBBZJ.Location = new System.Drawing.Point(0, 326);
            this.panelDJTBBZJ.Margin = new System.Windows.Forms.Padding(0);
            this.panelDJTBBZJ.Name = "panelDJTBBZJ";
            this.panelDJTBBZJ.Size = new System.Drawing.Size(756, 23);
            this.panelDJTBBZJ.TabIndex = 9;
            this.panelDJTBBZJ.Visible = false;
            // 
            // txtDJTBBZJ
            // 
            this.txtDJTBBZJ.AutoSize = true;
            this.txtDJTBBZJ.Font = new System.Drawing.Font("Tahoma", 9F);
            this.txtDJTBBZJ.ForeColor = System.Drawing.Color.Red;
            this.txtDJTBBZJ.Location = new System.Drawing.Point(207, 5);
            this.txtDJTBBZJ.Name = "txtDJTBBZJ";
            this.txtDJTBBZJ.Size = new System.Drawing.Size(55, 14);
            this.txtDJTBBZJ.TabIndex = 67;
            this.txtDJTBBZJ.Text = "自动带出";
            // 
            // label17
            // 
            this.label17.ForeColor = System.Drawing.Color.Black;
            this.label17.Location = new System.Drawing.Point(61, 4);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(142, 18);
            this.label17.TabIndex = 65;
            this.label17.Text = "冻结投标保证金：";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel15
            // 
            this.panel15.Controls.Add(this.ucCityList1);
            this.panel15.Controls.Add(this.label30);
            this.panel15.Location = new System.Drawing.Point(0, 349);
            this.panel15.Margin = new System.Windows.Forms.Padding(0);
            this.panel15.Name = "panel15";
            this.panel15.Size = new System.Drawing.Size(756, 33);
            this.panel15.TabIndex = 10;
            // 
            // ucCityList1
            // 
            this.ucCityList1.EnabledItem = new bool[] {
        true,
        true,
        true};
            this.ucCityList1.Location = new System.Drawing.Point(207, 5);
            this.ucCityList1.Name = "ucCityList1";
            this.ucCityList1.SelectedItem = new string[] {
        "",
        "",
        ""};
            this.ucCityList1.Size = new System.Drawing.Size(532, 25);
            this.ucCityList1.TabIndex = 105;
            this.ucCityList1.VisibleItem = new bool[] {
        true,
        true,
        true};
            // 
            // label30
            // 
            this.label30.ForeColor = System.Drawing.Color.Black;
            this.label30.Location = new System.Drawing.Point(123, 7);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(80, 18);
            this.label30.TabIndex = 104;
            this.label30.Text = "商品产地：";
            this.label30.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel16
            // 
            this.panel16.Controls.Add(this.btnSelectQY);
            this.panel16.Controls.Add(this.label13);
            this.panel16.Location = new System.Drawing.Point(0, 382);
            this.panel16.Margin = new System.Windows.Forms.Padding(0);
            this.panel16.Name = "panel16";
            this.panel16.Size = new System.Drawing.Size(756, 30);
            this.panel16.TabIndex = 11;
            // 
            // btnSelectQY
            // 
            this.btnSelectQY.BackColor = System.Drawing.Color.PowderBlue;
            this.btnSelectQY.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSelectQY.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSelectQY.ForeColor = System.Drawing.Color.Black;
            this.btnSelectQY.Location = new System.Drawing.Point(206, 5);
            this.btnSelectQY.Name = "btnSelectQY";
            this.btnSelectQY.Size = new System.Drawing.Size(183, 23);
            this.btnSelectQY.TabIndex = 95;
            this.btnSelectQY.Text = "选       择";
            this.btnSelectQY.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnSelectQY.Click += new System.EventHandler(this.btnSelectQY_Click);
            // 
            // label13
            // 
            this.label13.ForeColor = System.Drawing.Color.Black;
            this.label13.Location = new System.Drawing.Point(123, 8);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(80, 18);
            this.label13.TabIndex = 94;
            this.label13.Text = "供货区域：";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel17
            // 
            this.panel17.Controls.Add(this.btnSCZL);
            this.panel17.Controls.Add(this.label18);
            this.panel17.Location = new System.Drawing.Point(0, 415);
            this.panel17.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.panel17.Name = "panel17";
            this.panel17.Size = new System.Drawing.Size(756, 30);
            this.panel17.TabIndex = 12;
            // 
            // btnSCZL
            // 
            this.btnSCZL.BackColor = System.Drawing.Color.PowderBlue;
            this.btnSCZL.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSCZL.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSCZL.ForeColor = System.Drawing.Color.Black;
            this.btnSCZL.Location = new System.Drawing.Point(206, 3);
            this.btnSCZL.Name = "btnSCZL";
            this.btnSCZL.Size = new System.Drawing.Size(183, 23);
            this.btnSCZL.TabIndex = 94;
            this.btnSCZL.Text = "上 传 资 质";
            this.btnSCZL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnSCZL.Click += new System.EventHandler(this.btnSCZL_Click);
            // 
            // label18
            // 
            this.label18.ForeColor = System.Drawing.Color.Black;
            this.label18.Location = new System.Drawing.Point(38, 4);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(165, 18);
            this.label18.TabIndex = 93;
            this.label18.Text = "卖方资质文件：";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel18
            // 
            this.panel18.Controls.Add(this.btnCRCGX);
            this.panel18.Controls.Add(this.btnSave);
            this.panel18.Controls.Add(this.btnCancel);
            this.panel18.Location = new System.Drawing.Point(0, 445);
            this.panel18.Margin = new System.Windows.Forms.Padding(0);
            this.panel18.Name = "panel18";
            this.panel18.Size = new System.Drawing.Size(756, 32);
            this.panel18.TabIndex = 13;
            // 
            // btnCRCGX
            // 
            this.btnCRCGX.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnCRCGX.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCRCGX.ForeColor = System.Drawing.Color.DarkBlue;
            this.btnCRCGX.Location = new System.Drawing.Point(417, 9);
            this.btnCRCGX.Name = "btnCRCGX";
            this.btnCRCGX.Size = new System.Drawing.Size(88, 22);
            this.btnCRCGX.TabIndex = 94;
            this.btnCRCGX.Texts = "存入草稿箱";
            this.btnCRCGX.Click += new System.EventHandler(this.btnCRCGX_Click);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnSave.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSave.ForeColor = System.Drawing.Color.DarkBlue;
            this.btnSave.Location = new System.Drawing.Point(207, 9);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(60, 22);
            this.btnSave.TabIndex = 92;
            this.btnSave.Texts = "确认";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnCancel.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCancel.ForeColor = System.Drawing.Color.DarkBlue;
            this.btnCancel.Location = new System.Drawing.Point(317, 9);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(60, 22);
            this.btnCancel.TabIndex = 93;
            this.btnCancel.Texts = "重填";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // PBload
            // 
            this.PBload.Image = ((System.Drawing.Image)(resources.GetObject("PBload.Image")));
            this.PBload.Location = new System.Drawing.Point(985, 6);
            this.PBload.Name = "PBload";
            this.PBload.Size = new System.Drawing.Size(32, 32);
            this.PBload.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.PBload.TabIndex = 96;
            this.PBload.TabStop = false;
            this.PBload.Visible = false;
            // 
            // panelTJQ
            // 
            this.panelTJQ.Controls.Add(this.label19);
            this.panelTJQ.Controls.Add(this.LLlishi);
            this.panelTJQ.Controls.Add(this.label1);
            this.panelTJQ.Controls.Add(this.label2);
            this.panelTJQ.Controls.Add(this.label4);
            this.panelTJQ.Controls.Add(this.label9);
            this.panelTJQ.Controls.Add(this.label10);
            this.panelTJQ.Controls.Add(this.label12);
            this.panelTJQ.Controls.Add(this.label20);
            this.panelTJQ.Controls.Add(this.label21);
            this.panelTJQ.Controls.Add(this.label24);
            this.panelTJQ.Controls.Add(this.label29);
            this.panelTJQ.Controls.Add(this.label8);
            this.panelTJQ.Controls.Add(this.BSave);
            this.panelTJQ.Controls.Add(this.Breset);
            this.panelTJQ.Location = new System.Drawing.Point(803, 225);
            this.panelTJQ.Name = "panelTJQ";
            this.panelTJQ.Size = new System.Drawing.Size(163, 68);
            this.panelTJQ.TabIndex = 112;
            this.panelTJQ.Visible = false;
            // 
            // label19
            // 
            this.label19.ForeColor = System.Drawing.Color.Black;
            this.label19.Location = new System.Drawing.Point(238, 190);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(15, 12);
            this.label19.TabIndex = 108;
            this.label19.Text = "）";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // LLlishi
            // 
            this.LLlishi.AutoSize = true;
            this.LLlishi.Location = new System.Drawing.Point(163, 190);
            this.LLlishi.Name = "LLlishi";
            this.LLlishi.Size = new System.Drawing.Size(77, 12);
            this.LLlishi.TabIndex = 107;
            this.LLlishi.TabStop = true;
            this.LLlishi.Text = "点击链接查看";
            this.LLlishi.VisitedLinkColor = System.Drawing.Color.Goldenrod;
            // 
            // label1
            // 
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(14, 191);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(730, 12);
            this.label1.TabIndex = 106;
            this.label1.Text = "7、\t《电子购货合同》样本（";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(14, 173);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(730, 12);
            this.label2.TabIndex = 105;
            this.label2.Text = "6、\t卖方在履行交货时，须按照买方要求开具增值税专用发票或增值税普通发票（非一般纳税人单位须向税务机关申请代开）。";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(14, 154);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(730, 12);
            this.label4.TabIndex = 104;
            this.label4.Text = "5、\t为保证快速履约，合同期限为“即时”的投标单中标后，卖方须在72小时内自主冻结履约保证金予以定标。";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label9
            // 
            this.label9.ForeColor = System.Drawing.Color.Black;
            this.label9.Location = new System.Drawing.Point(14, 136);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(730, 12);
            this.label9.TabIndex = 103;
            this.label9.Text = "4、\t按有关协议和交易规定，投标单一旦中标，即为交易双方正式订立《电子购货合同》。";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label10
            // 
            this.label10.ForeColor = System.Drawing.Color.Black;
            this.label10.Location = new System.Drawing.Point(32, 119);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(724, 12);
            this.label10.TabIndex = 102;
            this.label10.Text = "低于卖方设定的一个经济批量），剩余部分将自动转入下一次交易撮合（若剩余部分数量低于卖方设定的一个经济批量，则自动撤销）。";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label12
            // 
            this.label12.ForeColor = System.Drawing.Color.Black;
            this.label12.Location = new System.Drawing.Point(32, 101);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(712, 12);
            this.label12.TabIndex = 101;
            this.label12.Text = "后予以解冻；如未履行定标程序，投标保证金将被扣罚，补偿给各中标买方；合同期限为“即时“的投标单，可能会一部分中标（不低于卖方设定的一个经济批量），剩余部分将自动转" +
    "入下一次交易撮合";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label20
            // 
            this.label20.ForeColor = System.Drawing.Color.Black;
            this.label20.Location = new System.Drawing.Point(14, 83);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(730, 12);
            this.label20.TabIndex = 100;
            this.label20.Text = "3、\t投标单下达的同时系统将自动冻结投标金额的0.5‰（最低不少于1000元/次）作为投标保证金；该投标保证金将在投标单撤销或定标后予以解冻；如未履行定标程序，";
            this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label21
            // 
            this.label21.ForeColor = System.Drawing.Color.Black;
            this.label21.Location = new System.Drawing.Point(33, 64);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(660, 12);
            this.label21.TabIndex = 99;
            this.label21.Text = "平台设定的最大经济批量”的10倍；合同期限为“即时”的，“投标拟售量”不得低于“平台设定的最大经济批量”。";
            this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label24
            // 
            this.label24.ForeColor = System.Drawing.Color.Black;
            this.label24.Location = new System.Drawing.Point(14, 46);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(730, 12);
            this.label24.TabIndex = 98;
            this.label24.Text = "2、\t卖方设定的“经济批量”不得高于“平台设定的最大经济批量”；合同期限为“三个月”或“一年”的，“投标拟售量”不得低于“平台设定的最大经济批量”的10倍；";
            this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label29
            // 
            this.label29.ForeColor = System.Drawing.Color.Black;
            this.label29.Location = new System.Drawing.Point(14, 28);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(731, 12);
            this.label29.TabIndex = 97;
            this.label29.Text = "1、\t一个商品一个轮次的竞标中只有一个最低价标的中标，价格相同时投标时间早的优先，中标买方收货区域将在该标的供货区域范围内。";
            this.label29.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            this.label8.ForeColor = System.Drawing.Color.Black;
            this.label8.Location = new System.Drawing.Point(10, 10);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(68, 12);
            this.label8.TabIndex = 29;
            this.label8.Text = "投标须知：";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // BSave
            // 
            this.BSave.BackColor = System.Drawing.Color.LightSkyBlue;
            this.BSave.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.BSave.ForeColor = System.Drawing.Color.DarkBlue;
            this.BSave.Location = new System.Drawing.Point(142, 556);
            this.BSave.Name = "BSave";
            this.BSave.Size = new System.Drawing.Size(60, 22);
            this.BSave.TabIndex = 49;
            this.BSave.Texts = "确认";
            // 
            // Breset
            // 
            this.Breset.BackColor = System.Drawing.Color.LightSkyBlue;
            this.Breset.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Breset.ForeColor = System.Drawing.Color.DarkBlue;
            this.Breset.Location = new System.Drawing.Point(297, 556);
            this.Breset.Name = "Breset";
            this.Breset.Size = new System.Drawing.Size(60, 22);
            this.Breset.TabIndex = 50;
            this.Breset.Texts = "重填";
            // 
            // ucSPMC_B2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.Controls.Add(this.panelTJQ);
            this.Controls.Add(this.PBload);
            this.Controls.Add(this.panel1);
            this.Name = "ucSPMC_B2";
            this.Size = new System.Drawing.Size(1049, 741);
            this.Load += new System.EventHandler(this.ucSPMC_B2_Load);
            this.panel1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panelSPMC.ResumeLayout(false);
            this.panelSPMC.PerformLayout();
            this.panelGG.ResumeLayout(false);
            this.panelGG.PerformLayout();
            this.panelJJDW.ResumeLayout(false);
            this.panelJJDW.PerformLayout();
            this.panelHTQX.ResumeLayout(false);
            this.panelHTQX.PerformLayout();
            this.panelPTSDDZDJJPL.ResumeLayout(false);
            this.panelPTSDDZDJJPL.PerformLayout();
            this.panel9.ResumeLayout(false);
            this.panel9.PerformLayout();
            this.panel19.ResumeLayout(false);
            this.panel10.ResumeLayout(false);
            this.panel10.PerformLayout();
            this.panel11.ResumeLayout(false);
            this.panel11.PerformLayout();
            this.panel12.ResumeLayout(false);
            this.panel12.PerformLayout();
            this.panelTBJE.ResumeLayout(false);
            this.panelTBJE.PerformLayout();
            this.panelDJTBBZJ.ResumeLayout(false);
            this.panelDJTBBZJ.PerformLayout();
            this.panel15.ResumeLayout(false);
            this.panel16.ResumeLayout(false);
            this.panel17.ResumeLayout(false);
            this.panel18.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PBload)).EndInit();
            this.panelTJQ.ResumeLayout(false);
            this.panelTJQ.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

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
            #region 草稿箱的一些必要的验证

            //if (PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["结算账户类型"] != null && PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["结算账户类型"].ToString() != "买家卖家交易账户")
            //{
            //    ArrayList al = new ArrayList();
            //    al.Add("");
            //    al.Add("您是经纪人交易账户，无法下达投标单！");
            //    FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "中国商品批发交易平台", al);
            //    fm.ShowDialog();
            //    return ;
            //}

            if (ht_Main == null)
            {
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add("请选择您要出售的商品！");
                FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "", al);
                fm.ShowDialog();
                return;
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

            #region 验证经济批量的填写和正确性
            //验证是否填写经济批量
            if (txtJJPL.Text.Trim() == "")
            {
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add("您的经济批量不能为空！");
                //if (label28.Text.IndexOf("平台设定") >= 0)
                //{
                al.Add("您的经济批量应不得高于平台设定的最大经济批量，不得为零。");
                //}
                //if (label28.Text.IndexOf("卖方") >= 0)
                //{
                //    al.Add("您的经济批量应不得高于卖方设定的最大经济批量，不得为零。"); 
                //}
                FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "", al);
                fm.ShowDialog();
                return ;
            }
            if (Convert.ToDouble(txtJJPL.Text) == 0.00)
            {
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add("您的经济批量应不得高于平台设定的最大经济批量，不得为零。");
                FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "", al);
                fm.ShowDialog();
                return ;
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
                return ;
            }
            #endregion

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
                return ;
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
                    al.Add("投标拟售量不得低于平台规定的最大经济批量的10倍。");
                }
                FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "", al);
                fm.ShowDialog();
                return ;
            }
            long TPNSL = Convert.ToInt64(this.txtTPNGL.Text);//卖家投标拟售量
            //验证批量的10倍
            if (txtHTQX.Text.Trim().Equals("即时"))
            {
                if (PTPL > TPNSL)
                {
                    ArrayList al = new ArrayList();
                    al.Add("");

                    al.Add("投标拟售量不得低于平台规定的最大经济批量。");

                    FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "", al);
                    fm.ShowDialog();
                    return ;
                }
            }
            else
            {
                if ((PTPL * 10) > TPNSL)
                {
                    ArrayList al = new ArrayList();
                    al.Add("");

                    al.Add("投标拟售量不得低于平台规定的最大经济批量的10倍。");

                    FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "", al);
                    fm.ShowDialog();
                    return ;
                }
            }

            //验证投标价格是否填写
            if (txtTBJG.Text.Trim() == "")
            {
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add("您的投标价格不能为空！");
                FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "", al);
                fm.ShowDialog();
                return ;
            }
            if (Convert.ToDouble(txtTBJG.Text) == 0.00)
            {
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add("您的投标价格不能为零！");
                FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "", al);
                fm.ShowDialog();
                return ;
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


            double TBJG = Convert.ToDouble(this.txtTBJG.Text);//投标价格
            //验证投标金额是否过大
            if ((TBJG * TPNSL).ToString("#0.00").Length > 10)
            {
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add("金额过大。");
                FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "", al);
                fm.ShowDialog();
                return ;
            }
            



          

            #endregion
            Hashtable ht = new Hashtable();
            ht["DLYX"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString();
            ht["SPBH"] = ht_Main["商品编号"];
            ht["HTQX"] = ht_Main["合同期限"];
            ht["GHQY"] = ht_QY["发货区域"].ToString();//供货区域
            ht["SLZM"] = ht_Main["SLZM"] == null ? null : ht_Main["SLZM"].ToString();
            ht["MJSDJJPL"] = this.txtJJPL.Text;//卖家设定的经济批量
            ht["TBNSL"] = this.txtTPNGL.Text;//投标拟售量
            ht["TBJG"] = txtTBJG.Text;//投标价格
            ht["TBJE"] = txtTBJE.Text;//投标金额
            ht["ZLBZYZM"] = ht_ZZ["质量标准与证明"] == null ? null : ht_ZZ["质量标准与证明"].ToString();
            ht["CPJCBG"] = ht_ZZ["产品检测报告"] == null ? null : ht_ZZ["产品检测报告"].ToString();
            ht["PGZFZRFLCNS"] = ht_ZZ["品管总负责人法律承诺书"] == null ? null : ht_ZZ["品管总负责人法律承诺书"].ToString();

            ht["SLZM"] = this.ucTextBox_slv.Text.Trim(); //this.uCshangchuan1.UpItem.Items.Count == 0 || this.uCshangchuan1.UpItem.Items[0].SubItems[1].Text == "" ? null : this.uCshangchuan1.UpItem.Items[0].SubItems[1].Text;//税率证明
            ht["FDDBRCNS"] = ht_ZZ["法定代表人承诺书"] == null ? null : ht_ZZ["法定代表人承诺书"].ToString();
            ht["SHFWGDYCN"] = ht_ZZ["售后服务规定与承诺"] == null ? null : ht_ZZ["售后服务规定与承诺"].ToString();
            ht["CPSJSQS"] = ht_ZZ["产品送检授权书"] == null ? null : ht_ZZ["产品送检授权书"].ToString();
            ht["其他资质"] = ht_ZZ["其它资质"] == null ? null : ht["其它资质"];//这个是以哈希表的形式存的
            //2013.09.06 wyh add 
            ht["省市区"] = ucCityList1.SelectedItem[0] + "|" + ucCityList1.SelectedItem[1] + "|" + ucCityList1.SelectedItem[2];
            panel1.Enabled = false;
            panelTJQ.Enabled = false;

            PBload.Location = new Point(btnCRCGX.Location.X + panel1.Location.X + btnCRCGX.Width + 10, btnCRCGX.Location.Y + panel18.Location.Y + 10);

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
                //reset();
            }
        }

        /// <summary>
        /// 提交时候的前端验证
        /// </summary>
        private bool ValSubInfo()
        {
            #region 验证是否为空
            //if (GLJJRPTGLJG == null)
            //{
            //    ArrayList al = new ArrayList();
            //    al.Add("");
            //    al.Add("，请及时提交！");
            //    FormAlertMessage fm = new FormAlertMessage("仅确定", "叹号", "中国商品批发交易平台", al);
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



            if (PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["结算账户类型"] == null || PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["结算账户类型"].ToString().Trim() == "")
            {
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add("您尚未开通交易账户，不能进行交易操作！");
                FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "中国商品批发交易平台", al);
                fm.ShowDialog();
                return false;
            }

            if (PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["结算账户类型"].ToString().Trim() != "买家卖家交易账户" && PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["结算账户类型"].ToString().Trim() != "经纪人交易账户")
            {
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add("您的交易账户类型不正确，不能进行交易操作！");
                FormAlertMessage fm = new FormAlertMessage("仅确定", "叹号", "中国商品批发交易平台", al);
                fm.ShowDialog();
                return false;
            }

            if (PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["结算账户类型"].ToString().Trim() == "经纪人交易账户")
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
                //if (label28.Text.IndexOf("平台设定") >= 0)
                //{
                    al.Add("您的经济批量应不得高于平台设定的最大经济批量，不得为零。");
                //}
                //if (label28.Text.IndexOf("卖方") >= 0)
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
                  al.Add("投标拟售量不得低于平台规定的最大经济批量的10倍。");
                }
                FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "", al);
                fm.ShowDialog();
                return false;
            }
            long TPNSL = Convert.ToInt64(this.txtTPNGL.Text);//卖家投标拟售量
            //验证批量的10倍
            if (txtHTQX.Text.Trim().Equals("即时"))
            {
                if(PTPL>TPNSL)
                {
                    ArrayList al = new ArrayList();
                    al.Add("");

                        al.Add("投标拟售量不得低于平台规定的最大经济批量。");

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

                        al.Add("投标拟售量不得低于平台规定的最大经济批量的10倍。");

                    FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "", al);
                    fm.ShowDialog();
                    return false;
                }
            }
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
            if ((this.ucTextBox_slv.Text.Trim().Equals("")||Convert.ToDouble(this.ucTextBox_slv.Text.Trim())==0))
            {
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add("税率不能为零，也不能为空！");
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
            //if (str_ZZ.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries).Length > ((((Hashtable)ht_ZZ["其它资质"]) == null) ? 0 : ((Hashtable)ht_ZZ["其它资质"]).Count))
            //{
            //    ArrayList al = new ArrayList();
            //    al.Add("");
            //    al.Add("请上传必要的资质文件！");
            //    FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "", al);
            //    fm.ShowDialog();
            //    return false;
            //}

            #endregion
            #region 验证填写信息的正确性

            double TBJG = Convert.ToDouble(this.txtTBJG.Text);//投标价格
            //验证投标金额是否过大
            if ((TBJG * TPNSL).ToString("#0.00").Length > 10)
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
            //if (nsr == null||nsr.ToString().Trim() == "")
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
        /// <summary>
        /// 查看电子购货合同
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

   



        private void LLlishi_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Hashtable htcs = new Hashtable();
            htcs["数据地址"] = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[2] + "/pingtaiservices/mobanrun/FMPTDZGHHT.aspx";
            XSRM xs = new XSRM("电子购货合同样本", htcs);
            xs.ShowDialog();
        }


        private void ucTextBox_slv_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (ucTextBox_slv.Text.IndexOf(".") >= 0)
            //{
            //    string ths = ucTextBox_slv.Text;
            //    if (ths.Split(new string[] { "." }, StringSplitOptions.None)[1].Length >= 1)
            //        e.Handled = true;
            //    else
            //        e.Handled = false;
            //}
        }

        private void ucTextBox_slv_KeyDown(object sender, KeyEventArgs e)
        {
            //if (ucTextBox_slv.Text.IndexOf(".") >= 0)
            //{
            //    string ths = ucTextBox_slv.Text;
            //    if (ths.Split(new string[] { "." }, StringSplitOptions.None)[1].Length >= 1 && e.KeyCode != Keys.Back)
            //        e.Handled = true;
            //    else
            //        e.Handled = false;
            //}
        }

        private void ucTextBox_slv_KeyUp(object sender, KeyEventArgs e)
        {
            //if (ucTextBox_slv.Text.IndexOf(".") >= 0)
            //{
            //    string ths = ucTextBox_slv.Text;
            //    if (ths.Split(new string[] { "." }, StringSplitOptions.None)[1].Length >= 1&&e.KeyCode!= Keys.Back)
            //        e.Handled = true;
            //    else
            //        e.Handled = false;
            //}
        }
               
       


    }
}
