using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using 客户端主程序.NewDataControl;
using System.Threading;
using 客户端主程序.DataControl;

namespace 客户端主程序.SubForm.NewCenterForm.SUBUC.JJRYWGL
{
    public partial class ucZTXYHSH_B : UserControl
    {


        Hashtable htUser;
        public ucZTXYHSH_B()
        {
            InitializeComponent();
            this.panel1.Enabled = false;
        }

        /// <summary>
        ///暂停
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {

            if (PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["结算账户类型"].ToString() != "经纪人交易账户")
            {
                ArrayList Almsg4 = new ArrayList();
                Almsg4.Add("");
                Almsg4.Add("只有经纪人交易账户可以执行此操作！");// 这里是提交失败的提示，可能是程序出错，可能是条件不足等
                FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "其他", "", Almsg4);
                FRSE4.ShowDialog();
                return;
            }
            ArrayList Almsg3 = new ArrayList();
            Almsg3.Add("");
            Almsg3.Add("暂停后您不能再接收新交易方的审核请求！");
            FormAlertMessage FRSE3 = new FormAlertMessage("确定取消", "问号", "", Almsg3);
            if (FRSE3.ShowDialog() == DialogResult.Yes)
            {
                //禁用提交区域并开启进度
                this.panel1.Enabled = false;
                PBload.Visible = true;
                htUser = new Hashtable();
                htUser["登录邮箱"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString();
                htUser["经纪人角色编号"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["经纪人角色编号"].ToString();
                htUser["结算账户类型"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["结算账户类型"].ToString();
                htUser["操作类型"] = "暂停";
                //执行当前操作
                SRT_ZTHFXYHSH_Run();
            }
        }

        #region  //开启线程执行用户当前选择的操作
        //开启一个测试线程
        private void SRT_ZTHFXYHSH_Run()
        {
            RunThreadClassKTJYZH OTD = new RunThreadClassKTJYZH(htUser, new delegateForThread(SRT_ZTHFXYHSH));
            Thread trd = new Thread(new ThreadStart(OTD.ZT_HF_XYHSH));
            trd.IsBackground = true;
            trd.Start();
        }
        //显示线程处理结果的函数(用于处理线程返回数据),调用异步委托
        private void SRT_ZTHFXYHSH(Hashtable OutPutHT)
        {
            try { Invoke(new delegateForThreadShow(SRT_ZTHFXYHSH_Invoke), new Hashtable[] { OutPutHT }); }
            catch (Exception ex) { Support.StringOP.WriteLog("委托回调错误：" + ex.ToString()); }
        }
        //处理非线程创建的控件
        private void SRT_ZTHFXYHSH_Invoke(Hashtable OutPutHT)
        {
            //重新开放提交区域,并滚动条强制置顶
        
            PBload.Visible = false;
            UPUP();


            //返回值后的处理
            DataSet dsreturn = (DataSet)OutPutHT["返回值"];

            string zt = dsreturn.Tables["返回值单条"].Rows[0]["执行结果"].ToString();
            string showstr = dsreturn.Tables["返回值单条"].Rows[0]["提示文本"].ToString();
            //显示执行结果
            switch (zt)
            {
                case "ok":
                    //给出表单提交成功的提示
                    ArrayList Almsg3 = new ArrayList();
                    Almsg3.Add("");
                    Almsg3.Add(showstr);
                    FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "其他", "", Almsg3);
                    FRSE3.ShowDialog();

                    reset();
                    break;
                default:
                    ArrayList Almsg4 = new ArrayList();
                    Almsg4.Add("");
                    Almsg4.Add(showstr);// 这里是提交失败的提示，可能是程序出错，可能是条件不足等
                    FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "其他", "", Almsg4);
                    FRSE4.ShowDialog();
                    reset();
                    break;
            }
        }
        #endregion

        #region  //开启线程获取用户初始状态
        //开启一个测试线程
        private void SRT_ZTHFXYHSHCSZT_Run()
        {
            RunThreadClassKTJYZH OTD = new RunThreadClassKTJYZH(htUser, new delegateForThread(SRT_ZTHFXYHSHCSZT));
            Thread trd = new Thread(new ThreadStart(OTD.ZT_HF_XYHSHCSZT));
            trd.IsBackground = true;
            trd.Start();
        }
        //显示线程处理结果的函数(用于处理线程返回数据),调用异步委托
        private void SRT_ZTHFXYHSHCSZT(Hashtable OutPutHT)
        {
            try { Invoke(new delegateForThreadShow(SRT_ZTHFXYHSHCSZT_Invoke), new Hashtable[] { OutPutHT }); }
            catch (Exception ex) { Support.StringOP.WriteLog("委托回调错误：" + ex.ToString()); }
        }
        //处理非线程创建的控件
        private void SRT_ZTHFXYHSHCSZT_Invoke(Hashtable OutPutHT)
        {
            //重新开放提交区域,并滚动条强制置顶

            PBload.Visible = false;
            UPUP();


            //返回值后的处理
            DataSet dsreturn = (DataSet)OutPutHT["返回值"];

            string zt = dsreturn.Tables["返回值单条"].Rows[0]["执行结果"].ToString();
            string showstr = dsreturn.Tables["返回值单条"].Rows[0]["提示文本"].ToString();
            //显示执行结果
            switch (zt)
            {
                case "ok":
                    //给出表单提交成功的提示
                    ArrayList Almsg3 = new ArrayList();
                    Almsg3.Add("");
                    Almsg3.Add(showstr);
                    FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "其他", "", Almsg3);
                    FRSE3.ShowDialog();
                    reset();
                    break;
                case "okOther":
                    if (dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"].ToString() == "未开通交易账户")
                    {
                        this.btnSave.Enabled = false;
                        this.btnCancel.Enabled = false;
                    }
                    else if (dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"].ToString() == "已开通交易账户")
                    {
                        if (dsreturn.Tables["返回值单条"].Rows[0]["附件信息2"].ToString() == "交易账户当前为暂停状态")
                        {
                            this.btnSave.Enabled = false;
                            this.btnCancel.Enabled = true;
                        }
                        else if (dsreturn.Tables["返回值单条"].Rows[0]["附件信息2"].ToString() == "交易账户当前为恢复状态")
                        {
                            this.btnSave.Enabled = true;
                            this.btnCancel.Enabled = false;
                        }
                    }
                    break;
                default:
                    ArrayList Almsg4 = new ArrayList();
                    Almsg4.Add("");
                    Almsg4.Add(showstr);// 这里是提交失败的提示，可能是程序出错，可能是条件不足等
                    FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "其他", "", Almsg4);
                    FRSE4.ShowDialog();
                    reset();
                    break;
            }
        }
        #endregion







        /// <summary>
        /// 重置表单
        /// </summary>
        private void reset()
        {
            ucZTXYHSH_B s = new ucZTXYHSH_B();
            s.Dock = DockStyle.Fill;//铺满
            s.AutoScroll = true;//出现滚动条
            s.BackColor = Color.AliceBlue;

            //清理控件
            Panel P = (Panel)this.Parent;
            P.Controls.Clear();
            P.Controls.Add(s); //加入到某一个panel中
            s.Show();//显示出来
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
      
        private void ucZTXYHSH_B_Load(object sender, EventArgs e)
        {
            //将滚动条置顶
            UPUP();
            //设置进度条的位置，放到按钮旁边
            PBload.Location = new Point(btnSave.Location.X + btnSave.Width + 25, btnSave.Location.Y );
            this.panel1.Enabled = true;
            this.btnSave.Enabled = false;
            this.btnCancel.Enabled = false;

            if (PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["结算账户类型"].ToString() != "经纪人交易账户")
            {
                this.panel1.Enabled = false;
            }
            else
            {
                htUser = new Hashtable();
                htUser["登录邮箱"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString();
                htUser["经纪人角色编号"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["经纪人角色编号"].ToString();
                htUser["结算账户类型"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["结算账户类型"].ToString();
                htUser["操作类型"] = "暂停";
                SRT_ZTHFXYHSHCSZT_Run();
            
            }
          
        }
        /// <summary>
        /// 恢复
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["结算账户类型"].ToString() != "经纪人交易账户")
            {
                ArrayList Almsg4 = new ArrayList();
                Almsg4.Add("");
                Almsg4.Add("只有经纪人交易账户可以执行此操作！");// 这里是提交失败的提示，可能是程序出错，可能是条件不足等
                FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "其他", "", Almsg4);
                FRSE4.ShowDialog();
                return;
            }
            
            
            
            //禁用提交区域并开启进度
            this.panel1.Enabled = false;
            PBload.Visible = true;
            htUser = new Hashtable();
            htUser["登录邮箱"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString();
            htUser["经纪人角色编号"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["经纪人角色编号"].ToString();
            htUser["结算账户类型"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["结算账户类型"].ToString();
            htUser["操作类型"] = "恢复";
            //执行当前操作
            SRT_ZTHFXYHSH_Run();
        }

       

    
    }
}
