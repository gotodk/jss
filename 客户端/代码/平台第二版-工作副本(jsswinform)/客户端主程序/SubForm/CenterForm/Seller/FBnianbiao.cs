using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Threading;
using 客户端主程序.Support;
using Com.Seezt.Skins;
using 客户端主程序.DataControl;

namespace 客户端主程序.SubForm.CenterForm.Seller
{
    public partial class FBnianbiao : UserControl
    {
        public FBnianbiao()
        {
            InitializeComponent();
            //ZHZT();
            //IsOnOpenDay();
        }





        //模拟提交数据
        private void basicButton2_Click(object sender, EventArgs e)
        {
            //获得表单数据和进行基本验证
            Hashtable ht_form = new Hashtable();
            ht_form["str1"] = tb_spmc.Text.Trim();
            if (ht_form["str1"].ToString() == "")
            {
                ArrayList Almsg3 = new ArrayList();
                Almsg3.Add("");
                Almsg3.Add("测试，投标类型必填！");
                FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "错误", "提示", Almsg3);
                FRSE3.ShowDialog();
                return;
            }

            //禁用提交区域并开启进度
            panelUC5.Enabled = false;
            resetloadLocation(ResetB, PBload);
            

            //开启线程提交数据
            delegateForThread dft = new delegateForThread(ShowThreadResult_save);
            DataControl.RunThreadClassTestSaveOrUpdate RTCTSOU = new DataControl.RunThreadClassTestSaveOrUpdate(ht_form, dft);
            Thread trd = new Thread(new ThreadStart(RTCTSOU.BeginRun));
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
            //重新开放提交区域
            panelUC5.Enabled = true;
            PBload.Visible = false;

            //显示执行结果
            string zt = returnHT["执行状态"].ToString();
            switch (zt)
            {
                case "ok":

                    //给出表单提交成功的提示
                    ArrayList Almsg3 = new ArrayList();
                    Almsg3.Add("");
                    Almsg3.Add("您的xxxxx单提交成功！");
                    FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "叹号", "处理成功", Almsg3);
                    FRSE3.ShowDialog();



                    //重置本表单，或转向其他页面
                    FBnianbiao UC = new FBnianbiao();//这句话是关键，若实例化本身的类，则是重置表单，若实例化其他的类，则是跳转
                    reset(UC);

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
                    Almsg4.Add(returnHT["执行结果"].ToString());// 这里是提交失败的提示，可能是程序出错，可能是条件不足等
                    FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "错误", "提交xxx失败", Almsg4);
                    FRSE4.ShowDialog();

                    break;
            }

        }

        /// <summary>
        /// 重置表单
        /// </summary>
        /// <param name="UC"></param>
        private void reset(FBnianbiao UC)
        {
            UC.Dock = DockStyle.Fill;//铺满
            UC.AutoScroll = true;//出现滚动条
            UC.BackColor = Color.AliceBlue;
            UC.Padding = new Padding(10);
            //清理控件
            Panel P = (Panel)this.Parent;
            P.Controls.Clear();
            P.Controls.Add(UC); //加入到某一个panel中
            UC.Show();//显示出来

            //更改标题(如果重置本表单，不需要执行这句话)
            //this.ParentForm.Text = "xxx提交成功";
        }


       
        /// 重设进度条位置，放到按钮旁边
        /// </summary>
        /// <param name="BB">参考提交按钮</param>
        /// <param name="PB">要移动的进度条</param>
        private void resetloadLocation(BasicButton BB, PictureBox PB)
        {
            PB.Location = new Point(BB.Location.X + BB.Width + 30, BB.Location.Y);
            PB.Visible = true;
        }



        #region 上传控件的具体操作
        //上传
        private void B_SC_Click_1(object sender, EventArgs e)
        {
            //开启上传
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string[] fName = openFileDialog1.FileNames;

                //若选择选择对话框允许选择多个，则还要检验不能超过5个
                //多不是对话框选择的，是直接指定数组，则还要检验不能重名

                //开始上传,传入文件名数组，传入对应的上传结果控件，传入回调，传入角色编号
                FormSC FSC = new FormSC(fName, LVzlbz, new delegateForSC(UpLoadSucceed), PublicDS.PublisDsUser.Tables[0].Rows[0]["卖家角色编号"].ToString());
                FSC.ShowDialog();

            }
        }

        //查看
        private void B_SCCK_Click_1(object sender, EventArgs e)
        {
            //有地址
            if (LVzlbz.Items.Count > 0 && LVzlbz.Items[0].SubItems[1].Text.Trim() != "")
            {
                string url = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[1] + "/JHJXPT/SaveDir/" + LVzlbz.Items[0].SubItems[1].Text.Trim().Replace(@"\", "/");
                StringOP.OpenUrl(url);
            }
        }

        //删除图片
        private void B_SCSC_Click(object sender, EventArgs e)
        {
            LVzlbz.Items.Clear();
        }



        //上传
        private void label18_Click(object sender, EventArgs e)
        {
            //开启上传
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string[] fName = openFileDialog1.FileNames;

                //若选择选择对话框允许选择多个，则还要检验不能超过5个
                //多不是对话框选择的，是直接指定数组，则还要检验不能重名

                //开始上传,传入文件名数组，传入对应的上传结果控件，传入回调，传入角色编号
                FormSC FSC = new FormSC(fName, LVbg, new delegateForSC(UpLoadSucceed), PublicDS.PublisDsUser.Tables[0].Rows[0]["卖家角色编号"].ToString());
                FSC.ShowDialog();

            }
        }
        //查看
        private void label19_Click(object sender, EventArgs e)
        {
            //有地址
            if (LVbg.Items.Count > 0 && LVbg.Items[0].SubItems[1].Text.Trim() != "")
            {
                string url = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[1] + "/JHJXPT/SaveDir/" + LVbg.Items[0].SubItems[1].Text.Trim().Replace(@"\", "/");
                StringOP.OpenUrl(url);
            }
        }
        //删除图片
        private void label6_Click(object sender, EventArgs e)
        {
            LVbg.Items.Clear();
        }




        /// <summary>
        /// 若全部上传完成，在主窗体进行数据处理，根据情况编写，没有处理也要带着这个方法
        /// </summary>
        /// <param name="LV">上传结果集合</param>
        private void UpLoadSucceed(ListView LV)
        {
            //这里的LV,实际上是打开上传时指定的那个隐藏控件listView1，所以这个方法在多个上传按钮时，只需要写一次就行
            //MessageBox.Show("回调测试" + LV.Name);
        }

        //根据上传数据处理查看和删除按钮
        private void timer_SCCK_Tick(object sender, EventArgs e)
        {
            if (LVzlbz.Items.Count > 0)
            {
                B_SCCK.Visible = true;
                B_SCSC.Visible = true;
            }
            else
            {
                B_SCCK.Visible = false;
                B_SCSC.Visible = false;
            }

            if (LVbg.Items.Count > 0)
            {
                label19.Visible = true;
                label6.Visible = true;
            }
            else
            {
                label19.Visible = false;
                label6.Visible = false;
            }
        }

        #endregion


        #region 弹窗选择输入框的处理
        //点击打开弹窗
        private void Lselect_Click(object sender, EventArgs e)
        {
            FormSelectList FSL = new FormSelectList(new delegateForThread(ReSetsomething));
            FSL.ShowDialog();
        }

        /// <summary>
        /// 处理弹窗内选择了对勾后，应该影响这个界面上的哪些东西
        /// </summary>
        /// <param name="return_ht"></param>
        private void ReSetsomething(Hashtable return_ht)
        {
            tb_cn.Text = return_ht["测试值"].ToString();
        }
        #endregion


        private void ResetB_Click(object sender, EventArgs e)
        {
            //重置本表单，或转向其他页面
            FBnianbiao UC = new FBnianbiao();//这句话是关键，若实例化本身的类，则是重置表单，若实例化其他的类，则是跳转
            reset(UC);
        }

        private void FBnianbiao_Load(object sender, EventArgs e)
        {
            //处理初始化各种东西
            //cb_tblx.SelectedIndex = 0;

        }


        /// <summary>
        /// 计算总量合计
        /// </summary>
        private void GetHeJi()
        {
            //不能空也必须是正整数
            if (tb_zxjjpl.Text.Trim() != "" && tb_bs.Text.Trim() != "" && StringOP.IsNumeric(tb_zxjjpl.Text.Trim()) && StringOP.IsNumeric(tb_bs.Text.Trim()))
            {
                tb_zl.Text = (Convert.ToInt64(tb_zxjjpl.Text.Trim()) * Convert.ToInt64(tb_bs.Text.Trim())).ToString();
            }
        }
        private void tb_zxjjpl_TextChanged(object sender, EventArgs e)
        {
            GetHeJi();
        }

        private void tb_bs_TextChanged(object sender, EventArgs e)
        {
            GetHeJi();
        }

        #region//重置表单(跳转至新页面)
        private void B_XZSP_Click(object sender, EventArgs e)
        {
            ZHZT();
            //FBdldjbiao dld = new FBdldjbiao();
            //GoToNewUrl(dld);
        }
        //重置表单(跳转至新页面)
        private void GoToNewUrl(FBdldjbiao UC)
        {
            UC.Dock = DockStyle.Fill;//铺满
            UC.AutoScroll = true;//出现滚动条
            UC.BackColor = Color.AliceBlue;
            UC.Padding = new Padding(10);
            //清理控件
            Panel P = (Panel)this.Parent;
            P.Controls.Clear();
            P.Controls.Add(UC); //加入到某一个panel中
            UC.Show();//显示出来

            //更改标题(如果重置本表单，不需要执行这句话)
            //this.ParentForm.Text = "xxx提交成功";
        }
        #endregion

        #region//判断账号状态
        private void ZHZT()
        {
            string yhm = PublicDS.PublisDsUser.Tables[0].Rows[0]["YHM"].ToString();//用户名
            string SFYXDL = PublicDS.PublisDsUser.Tables[0].Rows[0]["SFYXDL"].ToString();//是否允许登陆
            string SFDJZH = PublicDS.PublisDsUser.Tables[0].Rows[0]["SFDJZH"].ToString();//是否冻结账号
            string SFZTXYW = PublicDS.PublisDsUser.Tables[0].Rows[0]["SFZTXYW"].ToString();//禁止新业务
            string SFXM = PublicDS.PublisDsUser.Tables[0].Rows[0]["SFXM"].ToString();//是否休眠
            switch (SFYXDL)//是否允许登陆
            {
                case "是":
                    switch (SFDJZH)//是否冻结账号
                    {
                        case "否":
                            switch (SFXM)//是否休眠
                            {
                                case "否":
                                    switch (SFZTXYW)//禁止新业务
                                    {
                                        case "否":
                                            IsOnOpenDay();
                                            break;
                                        default:
                                             ArrayList Almsg3 = new ArrayList();
                                            Almsg3.Add("");
                                            Almsg3.Add(yhm + "用户，您的账号已被禁止新业务，如有疑问请联系中国商品批发交易平台！");
                                            FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "错误", "提示", Almsg3);
                                            FRSE3.ShowDialog();
                                            //*关闭我的账户弹窗的操作*//
                                            break;
                                    }
                                    break;
                                default:
                                    ArrayList Almsg2 = new ArrayList();
                                    Almsg2.Add("");
                                    Almsg2.Add(yhm + "用户，您的账号已进入休眠状态，如有疑问请联系中国商品批发交易平台！");
                                    FormAlertMessage FRSE2 = new FormAlertMessage("仅确定", "错误", "提示", Almsg2);
                                    FRSE2.ShowDialog();
                                    //*关闭我的账户弹窗的操作*//

                                    break;
                            }
                            break;
                        default:
                            ArrayList Almsg1 = new ArrayList();
                            Almsg1.Add("");
                            Almsg1.Add(yhm + "用户，您的账号已被冻结，如有疑问请联系中国商品批发交易平台！");
                            FormAlertMessage FRSE1 = new FormAlertMessage("仅确定", "错误", "提示", Almsg1);
                            FRSE1.ShowDialog();
                            //*关闭我的账户弹窗的操作*//

                            break;
                    }
                    break;                
                default:
                    ArrayList Almsg = new ArrayList();
                    Almsg.Add("");
                    Almsg.Add(yhm + "用户，您的账号已被禁止登陆，如有疑问请联系中国商品批发交易平台！");
                    FormAlertMessage FRSE = new FormAlertMessage("仅确定", "错误", "提示", Almsg);
                    FRSE.ShowDialog();
                    //*关闭我的账户弹窗的操作*//

                    break;
            }
                       
        }
        #endregion

        #region//判断是否在营业日期
        private void IsOnOpenDay()
        {
            delegateForThread dft = new delegateForThread(ShowThreadResult_returnDay);
            DataControl.RunThreadClassIsOnOpenDay openTime = new DataControl.RunThreadClassIsOnOpenDay(dft);
            Thread trd = new Thread(new ThreadStart(openTime.BeginRun));
            trd.IsBackground = true;
            trd.Start();
        }
        //显示线程处理结果的函数(用于处理线程返回数据)
        private void ShowThreadResult_returnDay(Hashtable returnHT)
        {
            try
            {
                //调用异步委托
                Invoke(new delegateForThreadShow(ShowThreadResult_returnTime_IsOnOpenDay), new Hashtable[] { returnHT });
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("委托回调错误：" + ex.ToString());
            }
        }
        private void ShowThreadResult_returnTime_IsOnOpenDay(Hashtable returnHT)
        {
            string jg = returnHT["营业日期"].ToString();
            switch (jg)
            {
                case "true":
                    IsOnOpenTime();
                    break;
                case "false":
                    ArrayList Almsg1 = new ArrayList();
                    Almsg1.Add("");
                    Almsg1.Add("不在营业日期内");
                    FormAlertMessage FRSE1 = new FormAlertMessage("仅确定", "错误", "提示", Almsg1);
                    FRSE1.ShowDialog();
                    return;
                default:
                    ArrayList Almsg2 = new ArrayList();
                    Almsg2.Add("");
                    Almsg2.Add("系统繁忙,请稍后重试！");
                    FormAlertMessage FRSE2 = new FormAlertMessage("仅确定", "错误", "提示", Almsg2);
                    FRSE2.ShowDialog();
                    return;
            }


        }
        #endregion

        #region//判断是否在营业时间
        private void IsOnOpenTime()
        {
            delegateForThread dft = new delegateForThread(ShowThreadResult_returnTime);
            DataControl.RunThreadClassIsOnOpenTime openTime = new DataControl.RunThreadClassIsOnOpenTime(dft);
            Thread trd = new Thread(new ThreadStart(openTime.BeginRun));
            trd.IsBackground = true;
            trd.Start();
        }
        //显示线程处理结果的函数(用于处理线程返回数据)
        private void ShowThreadResult_returnTime(Hashtable returnHT)
        {
            try
            {
                //调用异步委托
                Invoke(new delegateForThreadShow(ShowThreadResult_returnTime_IsOnOpenTime), new Hashtable[] { returnHT });
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("委托回调错误：" + ex.ToString());
            }
        }
        private void ShowThreadResult_returnTime_IsOnOpenTime(Hashtable returnHT)
        {
            string jg = returnHT["营业时间"].ToString();
            switch (jg)
            {
                case "true":
                    IsOpenJSZH();
                    
                    break;
                case "false":
                    ArrayList Almsg1 = new ArrayList();
                    Almsg1.Add("");
                    Almsg1.Add("不在营业时间内");
                    FormAlertMessage FRSE1 = new FormAlertMessage("仅确定", "错误", "提示", Almsg1);
                    FRSE1.ShowDialog();
                    return;
                default:
                    ArrayList Almsg2 = new ArrayList();
                    Almsg2.Add("");
                    Almsg2.Add("系统繁忙,请稍后重试！");
                    FormAlertMessage FRSE2 = new FormAlertMessage("仅确定", "错误", "提示", Almsg2);
                    FRSE2.ShowDialog();
                    return;
            }


        }
        #endregion

        #region//判断当前账号是否已开通卖家结算账户
        private void IsOpenJSZH()
        {
            Hashtable HT = new Hashtable();
            HT["登录邮箱"] = PublicDS.PublisDsUser.Tables[0].Rows[0]["DLYX"].ToString();

            delegateForThread tempDFT = new delegateForThread(ShowThreadResult_IsOpenJSZH);
            DataControl.RunThreadClassIsOpenJSZH RTCCL = new DataControl.RunThreadClassIsOpenJSZH(HT, tempDFT);
            Thread trd = new Thread(new ThreadStart(RTCCL.BeginRun));
            trd.IsBackground = true;
            trd.Start();
        }
        //显示线程处理结果的函数(用于处理线程返回数据)
        private void ShowThreadResult_IsOpenJSZH(Hashtable OutPutHT)
        {
            try
            {
                this.SuspendLayout();
                //调用异步委托
                Invoke(new delegateForThreadShow(ShowThreadResult_IsOpenJSZH_Invoke), new Hashtable[] { OutPutHT });
                this.ResumeLayout(false);
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("委托回调错误：" + ex.ToString());
            }
        }
        //处理非线程创建的控件
        private void ShowThreadResult_IsOpenJSZH_Invoke(Hashtable OutPutHT)
        {
            string yhm = PublicDS.PublisDsUser.Tables[0].Rows[0]["YHM"].ToString();//用户名
            //演示用账号密码验证登陆后，显示主窗体并关闭登陆窗口
            DataSet ds = (DataSet)OutPutHT["角色信息"]; //获取登陆结果
            if (ds != null&&ds.Tables[0].Rows.Count>0)
            {
                if (ds.Tables[0].Rows[0]["系统繁忙"].ToString() == "否")
                {
                    if (ds.Tables[0].Rows[0]["KTSHZT"].ToString() == "审核通过")
                    {
                        JJRIsZTXYW();
                    }
                    else
                    {
                        ArrayList Almsg1 = new ArrayList();
                        Almsg1.Add("");
                        Almsg1.Add(yhm + "用户，您当前未开通结算账户，只能操作模拟业务，但不能真正提交业务！");
                        FormAlertMessage FRSE1 = new FormAlertMessage("确定取消", "叹号", "提示", Almsg1);
                        FRSE1.ShowDialog();
                        //*模拟业务*//

                        return;
                    }

                }
                else
                {
                    ArrayList Almsg1 = new ArrayList();
                    Almsg1.Add("");
                    Almsg1.Add(yhm + "用户，当前系统繁忙，请稍后重试！");
                    FormAlertMessage FRSE1 = new FormAlertMessage("仅确定", "错误", "提示", Almsg1);
                    FRSE1.ShowDialog();
                    return;
                    
                }
            }
            else
            {
                ArrayList Almsg1 = new ArrayList();
                Almsg1.Add("");
                Almsg1.Add(yhm + "用户，您当前未开通结算账户，只能操作模拟业务，但不能真正提交业务！");
                FormAlertMessage FRSE1 = new FormAlertMessage("确定取消", "叹号", "提示", Almsg1);
                FRSE1.ShowDialog();
                //*模拟业务*//

                return;
            }
        }

        #endregion

        #region//判断当前账号关联的默认经纪人是否暂停新业务
        private void JJRIsZTXYW()
        {
            Hashtable HT = new Hashtable();
            HT["登录邮箱"] = PublicDS.PublisDsUser.Tables[0].Rows[0]["DLYX"].ToString();

            delegateForThread tempDFT = new delegateForThread(ShowThreadResult_JJRIsZTXYW);
            DataControl.RunThreadClassIsZTXYW RTCCL = new DataControl.RunThreadClassIsZTXYW(HT, tempDFT);
            Thread trd = new Thread(new ThreadStart(RTCCL.BeginRun));
            trd.IsBackground = true;
            trd.Start();
        }
        //显示线程处理结果的函数(用于处理线程返回数据)
        private void ShowThreadResult_JJRIsZTXYW(Hashtable OutPutHT)
        {
            try
            {
                this.SuspendLayout();
                //调用异步委托
                Invoke(new delegateForThreadShow(ShowThreadResult_JJRIsZTXYW_Invoke), new Hashtable[] { OutPutHT });
                this.ResumeLayout(false);
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("委托回调错误：" + ex.ToString());
            }
        }
        //处理非线程创建的控件
        private void ShowThreadResult_JJRIsZTXYW_Invoke(Hashtable OutPutHT)
        {
            string yhm = PublicDS.PublisDsUser.Tables[0].Rows[0]["YHM"].ToString();//用户名
            //演示用账号密码验证登陆后，显示主窗体并关闭登陆窗口
            string ds = (string)OutPutHT["是否暂停新业务"]; //获取登陆结果
             switch (ds)
            {
                case "没有默认的经纪人":
                    ArrayList Almsg = new ArrayList();
                    Almsg.Add("");
                    Almsg.Add(yhm + "用户，您未设定默认关联经纪人，无法直接发布投标！");
                    FormAlertMessage FRSE = new FormAlertMessage("确定取消", "叹号", "提示", Almsg);
                    FRSE.ShowDialog();
                    //*两个按钮：设定关联代理人模拟业务*//

                    return;
                case "系统繁忙":
                     ArrayList Almsg1 = new ArrayList();
                    Almsg1.Add("");
                    Almsg1.Add(yhm + "用户，当前系统繁忙，请稍后重试！");
                    FormAlertMessage FRSE1 = new FormAlertMessage("仅确定", "叹号", "提示", Almsg1);
                    FRSE1.ShowDialog();
                    return;
                default:
                    switch (ds)
                    {
                        case "是":
                            ArrayList Almsg2 = new ArrayList();
                            Almsg2.Add("");
                            Almsg2.Add(yhm + "用户，您设定的默认关联经纪人已暂停新业务，无法直接发布投标，请修改默认经纪人后，在发布投标！");
                            FormAlertMessage FRSE2 = new FormAlertMessage("确定取消", "叹号", "提示", Almsg2);
                            FRSE2.ShowDialog();
                            //*两个按钮：设定关联代理人模拟业务*//

                            return;
                        default:
                             ArrayList Almsg3 = new ArrayList();
                            Almsg3.Add("");
                            Almsg3.Add(yhm + "用户，您设定的默认关联经纪人新业务正常！");
                            FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "叹号", "提示", Almsg3);
                            FRSE3.ShowDialog();
                            break;
                    }
                break;
                
            }
        }

        #endregion

        #region//账户当前可用余额
        private void GetZHDQKYYE()
        {
            Hashtable HT = new Hashtable();
            HT["登录邮箱"] = PublicDS.PublisDsUser.Tables[0].Rows[0]["DLYX"].ToString();

            delegateForThread tempDFT = new delegateForThread(ShowThreadResult_ZHDQKYYE);
            DataControl.RunThreadClassZHDQKYYE RTCCL = new DataControl.RunThreadClassZHDQKYYE(HT, tempDFT);
            Thread trd = new Thread(new ThreadStart(RTCCL.BeginRun));
            trd.IsBackground = true;
            trd.Start();
        }
        //显示线程处理结果的函数(用于处理线程返回数据)
        private void ShowThreadResult_ZHDQKYYE(Hashtable OutPutHT)
        {
            try
            {
                this.SuspendLayout();
                //调用异步委托
                Invoke(new delegateForThreadShow(ShowThreadResult_ZHDQKYYE_Invoke), new Hashtable[] { OutPutHT });
                this.ResumeLayout(false);
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("委托回调错误：" + ex.ToString());
            }
        }
        //处理非线程创建的控件
        private void ShowThreadResult_ZHDQKYYE_Invoke(Hashtable OutPutHT)
        {
            string yhm = PublicDS.PublisDsUser.Tables[0].Rows[0]["YHM"].ToString();//用户名
            //演示用账号密码验证登陆后，显示主窗体并关闭登陆窗口
            string ds = (string)OutPutHT["账户当前可用余额"]; //获取登陆结果
            switch (ds)
            {
                case "无数据":
                    ArrayList Almsg = new ArrayList();
                    Almsg.Add("");
                    Almsg.Add(yhm + "用户，未查到您当前的可用余额！");
                    FormAlertMessage FRSE = new FormAlertMessage("仅确定", "叹号", "提示", Almsg);
                    FRSE.ShowDialog();
                    return;
                case "":
                case "系统繁忙":
                    ArrayList Almsg1 = new ArrayList();
                    Almsg1.Add("");
                    Almsg1.Add(yhm + "用户，当前系统繁忙，请稍后重试！");
                    FormAlertMessage FRSE1 = new FormAlertMessage("仅确定", "叹号", "提示", Almsg1);
                    FRSE1.ShowDialog();
                    return;
                default:
                    //*与投标保证金进行比较，判断当前剩余可用余额是否足够*//
                    break;

            }
        }

        #endregion

        //选择商品投标
        private void bscradSelectSPTB_Click(object sender, EventArgs e)
        {
            bscradSelectSPTB.Checked = true;
            bscradWriteBSH.Checked = false;
            this.tb_bsh.Enabled = false;
            Hashtable HT = new Hashtable();
            HT["投标"] = "年度";
            FBdldjSelectWarse selectWarse = new FBdldjSelectWarse(HT,new delegateForThread(ReSetsomethings));
            selectWarse.ShowDialog();
        }
        private void ReSetsomethings(Hashtable return_ht)
        {
            this.tb_bsh.Text = return_ht["测试值"].ToString();
        }
        //输入标书号
        private void bscradWriteBSH_Click(object sender, EventArgs e)
        {
            bscradSelectSPTB.Checked = false;
            bscradWriteBSH.Checked = true;
            this.tb_bsh.Enabled = true;
        }


    }
}
