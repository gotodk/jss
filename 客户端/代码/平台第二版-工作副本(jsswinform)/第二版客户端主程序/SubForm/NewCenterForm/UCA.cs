using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using 客户端主程序.DataControl;

namespace 客户端主程序.SubForm.NewCenterForm
{
    public partial class UCA : UserControl
    {
        /// <summary>
        /// 父窗体
        /// </summary>
        private Center2013 _Center2013;
        /// <summary>
        /// 父窗体
        /// </summary>
        [Description("设置或读取当前菜单的父窗体"), Category("Appearance")]
        public Center2013 Center2013CC
        {
            get
            {
                return _Center2013;
            }
            set
            {
                _Center2013 = value;

                if (value != null)
                {
                    //设置父窗体同时，让特定按钮变成默认值(用于默认处理)
                    Label L;
                    if (PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["结算账户类型"].ToString() == "经纪人交易账户")
                    {
                        //定位到经纪人业务管理
                        L = label4;
                    }
                    else
                    {
                        //其他定位到商品买卖
                        L = label2;
                    }

                    L.BackColor = Color.SteelBlue;
                    L.ForeColor = Color.White;
                    string[] qu = (string[])L.Tag;
                    _Center2013.uCmenuBBB.Menus = qu[0];
                    _Center2013.uCmenuCCC.Menus = qu[1];
                }

                
            }
        }


        public UCA()
        {
            InitializeComponent();



            if (PublicDS.PublisDsUser != null && PublicDS.PublisDsUser.Tables.Contains("用户信息"))
            {
                //B区和C区菜单配置
                string Bsp1 = "";
                if (PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["结算账户类型"].ToString() == "买家卖家交易账户")
                {
                    Bsp1 = "[B区]资金转账☆客户端主程序.SubForm.NewCenterForm.SUBUC.ucZJZZ_B";//●演示列表☆客户端主程序.SubForm.NewCenterForm.SUBUC.DEMO1●演示提交☆客户端主程序.SubForm.NewCenterForm.SUBUC.DEMO2
                }
                else
                {
                    Bsp1 = "[B区]资金转账☆客户端主程序.SubForm.NewCenterForm.SUBUC.ucZJZZ_B●经纪人收益支取☆客户端主程序.SubForm.NewCenterForm.SUBUC.ucJJRSYZQ_B";//●演示列表☆客户端主程序.SubForm.NewCenterForm.SUBUC.DEMO1●演示提交☆客户端主程序.SubForm.NewCenterForm.SUBUC.DEMO2
                }
                string Bsp2 = "[B区]商品买入☆Y4_Business_SPMM.dll)!客户端主程序.SubForm.NewCenterForm.SUBUC.SPMM.ucSPMR_B2●商品卖出☆Y4_Business_SPMM.dll)!客户端主程序.SubForm.NewCenterForm.SUBUC.SPMM.ucSPMC_B2●预订单管理☆Y4_Business_SPMM.dll)!客户端主程序.SubForm.NewCenterForm.SUBUC.SPMM.ucYDDGL_B●投标单管理☆Y4_Business_SPMM.dll)!客户端主程序.SubForm.NewCenterForm.SUBUC.SPMM.ucTBDGL_B●异常投标单☆Y4_Business_SPMM.dll)!客户端主程序.SubForm.NewCenterForm.SUBUC.SPMM.ucYCTBDGL_B●定标☆Y4_Business_SPMM.dll)!客户端主程序.SubForm.NewCenterForm.SUBUC.SPMM.ucDB_B●清盘☆Y4_Business_SPMM.dll)!客户端主程序.SubForm.NewCenterForm.SUBUC.SPMM.uc_QP_B";
                string Bsp3 = "[B区]下达提货单☆Y5_Business_HWSF.dll)!客户端主程序.SubForm.NewCenterForm.SUBUC.HWSF.ucXDTHD_B●货物签收☆Y5_Business_HWSF.dll)!客户端主程序.SubForm.NewCenterForm.SUBUC.HWSF.ucHWQS_B●生成发货单☆Y5_Business_HWSF.dll)!客户端主程序.SubForm.NewCenterForm.SUBUC.HWSF.ucSCFHD_C●录入发货信息☆Y5_Business_HWSF.dll)!客户端主程序.SubForm.NewCenterForm.SUBUC.HWSF.ucLRFHXX_B●提请买方签收☆Y5_Business_HWSF.dll)!客户端主程序.SubForm.NewCenterForm.SUBUC.HWSF.ucTQMJQS_B●问题与处理☆Y5_Business_HWSF.dll)!客户端主程序.SubForm.NewCenterForm.SUBUC.HWSF.ucWTYCL_B";
                string Bsp4 = "";
                if (PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["经纪人分类"].ToString() == "银行" && PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["结算账户类型"].ToString() == "经纪人交易账户")
                {
                Bsp4 = "[B区]审核交易方资料☆Y6_Business_JJRYWGL.dll)!客户端主程序.SubForm.NewCenterForm.SUBUC.JJRYWGL.ucSHMMJZL_B●暂停新交易方审核☆Y6_Business_JJRYWGL.dll)!客户端主程序.SubForm.NewCenterForm.SUBUC.JJRYWGL.ucZTXYHSH_B●暂停交易方新业务☆Y6_Business_JJRYWGL.dll)!客户端主程序.SubForm.NewCenterForm.SUBUC.JJRYWGL.ucZTYHXYW_B●经纪人资格证书☆Y6_Business_JJRYWGL.dll)!客户端主程序.SubForm.NewCenterForm.SUBUC.JJRYWGL.ucJJRZGZ_B●交易方信用等级查询☆Y6_Business_JJRYWGL.dll)!客户端主程序.SubForm.NewCenterForm.SUBUC.JJRYWGL.ucJYFXYDJCX_B●服务人员信息维护☆Y6_Business_JJRYWGL.dll)!客户端主程序.SubForm.NewCenterForm.SUBUC.JJRYWGL.ucFWRYXXWH";

                }
                else
                {
                    Bsp4 = "[B区]审核交易方资料☆Y6_Business_JJRYWGL.dll)!客户端主程序.SubForm.NewCenterForm.SUBUC.JJRYWGL.ucSHMMJZL_B●暂停新交易方审核☆Y6_Business_JJRYWGL.dll)!客户端主程序.SubForm.NewCenterForm.SUBUC.JJRYWGL.ucZTXYHSH_B●暂停交易方新业务☆Y6_Business_JJRYWGL.dll)!客户端主程序.SubForm.NewCenterForm.SUBUC.JJRYWGL.ucZTYHXYW_B●经纪人资格证书☆Y6_Business_JJRYWGL.dll)!客户端主程序.SubForm.NewCenterForm.SUBUC.JJRYWGL.ucJJRZGZ_B●交易方信用等级查询☆Y6_Business_JJRYWGL.dll)!客户端主程序.SubForm.NewCenterForm.SUBUC.JJRYWGL.ucJYFXYDJCX_B";
                }
            
                string Bsp5 = "[B区]通知记录☆客户端主程序.SubForm.NewCenterForm.SUBUC.TZJL.ucTZJL_B●信用评级记录☆客户端主程序.SubForm.NewCenterForm.SUBUC.TZJL.ucXYJL";
                string Bsp6 = "[B区]账户资料☆客户端主程序.SubForm.NewCenterForm.SUBUC.ZHWH.ucZHZL_B●选择经纪人☆Y7_Business_ZHWH.dll)!客户端主程序.SubForm.NewCenterForm.SUBUC.ZHWH.ucXZJJR_B●休眠账户激活☆Y7_Business_ZHWH.dll)!客户端主程序.SubForm.NewCenterForm.SUBUC.ZHWH.ucXMZHJH_B●密码管理☆Y7_Business_ZHWH.dll)!客户端主程序.SubForm.NewCenterForm.SUBUC.ZHWH.ucMMJH_B●更改银行账户☆Y7_Business_ZHWH.dll)!客户端主程序.SubForm.NewCenterForm.SUBUC.ZHWH.ucUpdateBankAccount_B●开票信息维护☆Y7_Business_ZHWH.dll)!客户端主程序.SubForm.NewCenterForm.SUBUC.ZHWH.ucKPXX_B";
                string Csp1 = "";
                if (PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["结算账户类型"].ToString() == "买家卖家交易账户")
                {
                    Csp1 = "[C区]资金余额变动明细☆客户端主程序.SubForm.NewCenterForm.SUBUC.ucZJYEBDMX_C●资金转账☆客户端主程序.SubForm.NewCenterForm.SUBUC.ucZJZZ_C●冻结资金☆客户端主程序.SubForm.NewCenterForm.SUBUC.ucDJZJ_C●货款收付☆客户端主程序.SubForm.NewCenterForm.SUBUC.ucHKSF_C●违约赔偿☆客户端主程序.SubForm.NewCenterForm.SUBUC.ucWYPC_C●补偿收益☆客户端主程序.SubForm.NewCenterForm.SUBUC.ucBCSY_C●违约赔偿\n\r发生记录☆客户端主程序.SubForm.NewCenterForm.SUBUC.ucWYPCFSJL_C●补偿收益\n\r发生记录☆客户端主程序.SubForm.NewCenterForm.SUBUC.ucBCSYFSJL_C";
                }
                else
                {
                    if (PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["经纪人分类"].ToString() == "银行" && PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["结算账户类型"].ToString() == "经纪人交易账户")
                    {
                        Csp1 = "[C区]资金余额变动明细☆客户端主程序.SubForm.NewCenterForm.SUBUC.ucZJYEBDMX_C●资金转账☆客户端主程序.SubForm.NewCenterForm.SUBUC.ucZJZZ_C●冻结资金☆客户端主程序.SubForm.NewCenterForm.SUBUC.ucDJZJ_C●货款收付☆客户端主程序.SubForm.NewCenterForm.SUBUC.ucHKSF_C●违约赔偿☆客户端主程序.SubForm.NewCenterForm.SUBUC.ucWYPC_C●补偿收益☆客户端主程序.SubForm.NewCenterForm.SUBUC.ucBCSY_C●经纪人收益☆客户端主程序.SubForm.NewCenterForm.SUBUC.ucJJRSY_C●收益管理☆客户端主程序.SubForm.NewCenterForm.SUBUC.ucSYGL●违约赔偿\n\r发生记录☆客户端主程序.SubForm.NewCenterForm.SUBUC.ucWYPCFSJL_C●补偿收益\n\r发生记录☆客户端主程序.SubForm.NewCenterForm.SUBUC.ucBCSYFSJL_C●经纪人收益\n\r发生记录☆客户端主程序.SubForm.NewCenterForm.SUBUC.ucJJRSYFSJL_C";
                    }
                    else
                    {
                        Csp1 = "[C区]资金余额变动明细☆客户端主程序.SubForm.NewCenterForm.SUBUC.ucZJYEBDMX_C●资金转账☆客户端主程序.SubForm.NewCenterForm.SUBUC.ucZJZZ_C●冻结资金☆客户端主程序.SubForm.NewCenterForm.SUBUC.ucDJZJ_C●货款收付☆客户端主程序.SubForm.NewCenterForm.SUBUC.ucHKSF_C●违约赔偿☆客户端主程序.SubForm.NewCenterForm.SUBUC.ucWYPC_C●补偿收益☆客户端主程序.SubForm.NewCenterForm.SUBUC.ucBCSY_C●经纪人收益☆客户端主程序.SubForm.NewCenterForm.SUBUC.ucJJRSY_C●违约赔偿\n\r发生记录☆客户端主程序.SubForm.NewCenterForm.SUBUC.ucWYPCFSJL_C●补偿收益\n\r发生记录☆客户端主程序.SubForm.NewCenterForm.SUBUC.ucBCSYFSJL_C●经纪人收益\n\r发生记录☆客户端主程序.SubForm.NewCenterForm.SUBUC.ucJJRSYFSJL_C";
                    }
                }

                string Csp2 = "[C区]商品买卖概况☆Y4_Business_SPMM.dll)!客户端主程序.SubForm.NewCenterForm.SUBUC.SPMM.ucSPMMQK_C●竞标中☆Y4_Business_SPMM.dll)!客户端主程序.SubForm.NewCenterForm.SUBUC.SPMM.ucJBZ_C●冷静期☆Y4_Business_SPMM.dll)!客户端主程序.SubForm.NewCenterForm.SUBUC.SPMM.ucLJQ●中标☆Y4_Business_SPMM.dll)!客户端主程序.SubForm.NewCenterForm.SUBUC.SPMM.ucZB_C●定标与保证函☆Y4_Business_SPMM.dll)!客户端主程序.SubForm.NewCenterForm.SUBUC.SPMM.ucDBYBZH_C●废标☆Y4_Business_SPMM.dll)!客户端主程序.SubForm.NewCenterForm.SUBUC.SPMM.ucFB_C●清盘☆Y4_Business_SPMM.dll)!客户端主程序.SubForm.NewCenterForm.SUBUC.SPMM.ucQP_C●草稿箱☆Y4_Business_SPMM.dll)!客户端主程序.SubForm.NewCenterForm.SUBUC.SPMM.ucCGX_C";
                string Csp3 = "[C区]货物收发概况☆Y5_Business_HWSF.dll)!客户端主程序.SubForm.NewCenterForm.SUBUC.HWSF.ucHWSFGK_C●已下达提货单☆Y5_Business_HWSF.dll)!客户端主程序.SubForm.NewCenterForm.SUBUC.HWSF.ucYXDTHD_C●货物签收☆Y5_Business_HWSF.dll)!客户端主程序.SubForm.NewCenterForm.SUBUC.HWSF.ucHWQS_C●货物发出☆Y5_Business_HWSF.dll)!客户端主程序.SubForm.NewCenterForm.SUBUC.HWSF.ucHWFC_C●问题与处理☆Y5_Business_HWSF.dll)!客户端主程序.SubForm.NewCenterForm.SUBUC.HWSF.ucWTYCL_C";
                string Csp4 = "";
                if (PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["经纪人分类"].ToString() == "银行" && PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["结算账户类型"].ToString() == "经纪人交易账户")
                {
                    Csp4 = "[C区]交易方交易概况☆Y6_Business_JJRYWGL.dll)!客户端主程序.SubForm.NewCenterForm.SUBUC.JJRYWGL.ucJYFJYGK_C●交易方基本资料☆Y6_Business_JJRYWGL.dll)!客户端主程序.SubForm.NewCenterForm.SUBUC.JJRYWGL.ucYHJJRJJFJBZL_C●交易方违规记录☆Y6_Business_JJRYWGL.dll)!客户端主程序.SubForm.NewCenterForm.SUBUC.JJRYWGL.ucJJFWGJL_C●本账户下交易方交易商品分析☆Y6_Business_JJRYWGL.dll)!客户端主程序.SubForm.NewCenterForm.SUBUC.JJRYWGL.ucBZHXJJFJYSPFX_C";
                }
                else
                {
                    Csp4 = "[C区]交易方交易概况☆Y6_Business_JJRYWGL.dll)!客户端主程序.SubForm.NewCenterForm.SUBUC.JJRYWGL.ucJYFJYGK_C●交易方基本资料☆Y6_Business_JJRYWGL.dll)!客户端主程序.SubForm.NewCenterForm.SUBUC.JJRYWGL.ucJJFJBZL_C●交易方违规记录☆Y6_Business_JJRYWGL.dll)!客户端主程序.SubForm.NewCenterForm.SUBUC.JJRYWGL.ucJJFWGJL_C●本账户下交易方交易商品分析☆Y6_Business_JJRYWGL.dll)!客户端主程序.SubForm.NewCenterForm.SUBUC.JJRYWGL.ucBZHXJJFJYSPFX_C";
                }
         


                //A去菜单对应关系
                label1.Tag = new string[] { Bsp1, Csp1 };
                label2.Tag = new string[] { Bsp2, Csp2 };
                label3.Tag = new string[] { Bsp3, Csp3 };
                label4.Tag = new string[] { Bsp4, Csp4 };
                label5.Tag = new string[] { Bsp5, Csp1 };
                label6.Tag = new string[] { Bsp6, Csp1 };
            }
        }

        /// <summary>
        /// 强制重定向到某个A级菜单上
        /// </summary>
        /// <param name="labeltext"></param>
        public void auto_Click_label(string labeltext)
        {
            bool yixuanzhong = false;
            Label L = null;
            //遍历按钮，都搞成初始的样子
            foreach (Control control in this.flowLayoutPanel1.Controls)
            {
                foreach (Control controlLable in control.Controls)
                {
                    if (controlLable is Label)
                    {
                        Label LL = (Label)controlLable;
                        Color color_old = LL.BackColor;

                        LL.BackColor = Color.AliceBlue;
                        LL.ForeColor = Color.Black;

                        if (LL.Text.Trim() == labeltext.Trim())
                        {
                            //颜色已是选中状态，说明已经被选中了，不需要再次刷新
                            if (color_old == Color.SteelBlue)
                            {
                                yixuanzhong = true;
                            }
                            L = LL;
                        }
                    }
                }
            }
            //把被点击的按钮特殊处理
            
            L.BackColor = Color.SteelBlue;
            L.ForeColor = Color.White;

            if (!yixuanzhong)
            {
                _Center2013.uCmenuCCC.Menus = ((string[])L.Tag)[1];
                _Center2013.uCmenuBBB.Menus = ((string[])L.Tag)[0];
            }
        
        }

        private void label_Click(object sender, EventArgs e)
        {
            //遍历按钮，都搞成初始的样子
            foreach (Control control in this.flowLayoutPanel1.Controls)
            {
                foreach (Control controlLable in control.Controls)
                {

                    if (controlLable is Label)
                    {
                        Label LL = (Label)controlLable;
                        LL.BackColor = Color.AliceBlue;
                        LL.ForeColor = Color.Black;
                    }
                }
            }
            //把被点击的按钮特殊处理
            Label L = (Label)sender;
            L.BackColor = Color.SteelBlue;
            L.ForeColor = Color.White;

          _Center2013.uCmenuCCC.Menus = ((string[])L.Tag)[1];
            _Center2013.uCmenuBBB.Menus = ((string[])L.Tag)[0];
   
        }

        private void label_MouseEnter(object sender, EventArgs e)
        {

            Label L = (Label)sender;
            L.ForeColor = Color.Red;
        }

        private void label_MouseLeave(object sender, EventArgs e)
        {
            
            Label L = (Label)sender;
            if (L.BackColor == Color.SteelBlue)
            {
                L.ForeColor = Color.White;
            }
            else
            {
                L.ForeColor = Color.Black;
            }
        }

        private void UCA_Load(object sender, EventArgs e)
        {
            try
            {
                if (PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["结算账户类型"].ToString() == "买家卖家交易账户")
                {
                    this.panel4.Visible = false;
                }


                //给按钮绑定事件
                foreach (Control control in this.flowLayoutPanel1.Controls)
                {
                    foreach (Control controlLable in control.Controls)
                    {
                        if (controlLable is Label)
                        {
                            Label LL = (Label)controlLable;
                            LL.Click += new System.EventHandler(this.label_Click);
                            LL.MouseEnter += new System.EventHandler(this.label_MouseEnter);
                            LL.MouseLeave += new System.EventHandler(this.label_MouseLeave);
                        }
                    }
                }
            }
            catch { }





        }
    }
}
