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

namespace 客户端主程序.SubForm.CenterForm.Buyer
{
    public partial class FBYDD : UserControl
    {
        public FBYDD()
        {
            InitializeComponent();
            //必须先初始化省市区下拉框
            ucCityList1.initdefault();
            ucCityList1.VisibleItem = new bool[] { true, true, false };
            
        }

        //模拟提交数据
        private void basicButton2_Click(object sender, EventArgs e)
        {
            //获得表单数据和进行基本验证
            Hashtable ht_form = new Hashtable();
            int ydl =Convert.ToInt32(txtYDL.Text.ToString() == "" ? "0" : txtYDL.Text.ToString());
            int MaxJJPL = Convert.ToInt32(lblMaxJJPL.Text.ToString() == "*" ? "0" : lblMaxJJPL.Text.ToString());
            if (txtNMRJG.Text == "")
            {
                ArrayList Almsg3 = new ArrayList();
                Almsg3.Add("");
                Almsg3.Add("请填写拟买入价格！");
                FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "错误", "提示", Almsg3);
                FRSE3.ShowDialog();
                return;
            }
            if (txtYDL.Text == "")
            {
                ArrayList Almsg3 = new ArrayList();
                Almsg3.Add("");
                Almsg3.Add("请填写预定量！");
                FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "错误", "提示", Almsg3);
                FRSE3.ShowDialog();
                return;
            }
            if (ydl < MaxJJPL)
            {
                ArrayList Almsg3 = new ArrayList();
                Almsg3.Add("");
                Almsg3.Add("预订量必须大于当前最大经济批量！");
                FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "错误", "提示", Almsg3);
                FRSE3.ShowDialog();
                return;
            }            
            if (ucCityList1.SelectedItem[0] == "请选择省份" || ucCityList1.SelectedItem[1] == "请选择城市")
            {
                ArrayList Almsg3 = new ArrayList();
                Almsg3.Add("");
                Almsg3.Add("请选择收货区域！");
                FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "错误", "提示", Almsg3);
                FRSE3.ShowDialog();
                return;
            }
            DataSet dsXDYDD = initListDataSet();
            DataTable tb = initListDataTable();
            dsXDYDD.Tables["预订单"].Rows.Add(new string[] { PublicDS.PublisDsUser.Tables[0].Rows[0]["DLYX"].ToString(), txtGHZQ.Text.ToString(), txtGHZQ.Text.ToString(), label3.Text, txtSPMC.Text, txtXHGG.Text, txtDW.Text, MaxJJPL.ToString(), ydl.ToString(), txtNMRJG.Text, lblTBJ.Text.Substring(0, lblTBJ.Text.IndexOf("元，")), "|" + ucCityList1.SelectedItem[0].ToString().Trim() + ucCityList1.SelectedItem[1].ToString().Trim() + "|" });
            
            //禁用提交区域并开启进度
            panelUC5.Enabled = false;
            resetloadLocation(ResetB, PBload);
            

            //开启线程提交数据
            delegateForThread dft = new delegateForThread(ShowThreadResult_save);
            DataControl.RunThreadClassXDYDDXXSave RTCTSOU = new DataControl.RunThreadClassXDYDDXXSave(dsXDYDD, dft);
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
            DataSet zt = (DataSet)returnHT["执行结果"];
            if (zt != null)
            {
                switch (zt.Tables["返回值单条"].Rows[0]["执行结果"].ToString())
                {
                    case "success":

                        //给出表单提交成功的提示
                        ArrayList Almsg3 = new ArrayList();
                        Almsg3.Add("");
                        Almsg3.Add("本次预订单提交成功！");
                        FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "叹号", "处理成功", Almsg3);
                        FRSE3.ShowDialog();



                        //重置本表单，或转向其他页面
                        FBYDD UC = new FBYDD();//这句话是关键，若实例化本身的类，则是重置表单，若实例化其他的类，则是跳转
                        reset(UC);

                        break;
                    case "err":
                        ArrayList Almsg7 = new ArrayList();
                        Almsg7.Add("");
                        Almsg7.Add(zt.Tables["返回值单条"].Rows[0]["错误提示"].ToString());
                        FormAlertMessage FRSE7 = new FormAlertMessage("仅确定", "错误", "系统忙", Almsg7);
                        FRSE7.ShowDialog();
                        break;
                    case "ok":
                        break;
                    default:
                        ArrayList Almsg4 = new ArrayList();
                        Almsg4.Add("");
                        Almsg4.Add(zt.Tables["返回值单条"].Rows[0]["错误提示"].ToString());// 这里是提交失败的提示，可能是程序出错，可能是条件不足等
                        FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "错误", "提交xxx失败", Almsg4);
                        FRSE4.ShowDialog();

                        break;
                }
            }
            else
            {
                ArrayList Almsg4 = new ArrayList();
                Almsg4.Add("");
                Almsg4.Add("提交失败");// 这里是提交失败的提示，可能是程序出错，可能是条件不足等
                FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "错误", "提交xxx失败", Almsg4);
                FRSE4.ShowDialog();
                return;
            }
            

        }

        /// <summary>
        /// 重置表单
        /// </summary>
        /// <param name="UC"></param>
        private void reset(FBYDD UC)
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

        }

        //查看
        private void B_SCCK_Click_1(object sender, EventArgs e)
        {
            //有地址
            //if (LVzlbz.Items.Count > 0 && LVzlbz.Items[0].SubItems[1].Text.Trim() != "")
            //{
            //    string url = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[1] + "/JHJXPT/SaveDir/" + LVzlbz.Items[0].SubItems[1].Text.Trim().Replace(@"\", "/");
            //    StringOP.OpenUrl(url);
            //}
        }

        //删除图片
        private void B_SCSC_Click(object sender, EventArgs e)
        {
            //LVzlbz.Items.Clear();
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
                //FormSC FSC = new FormSC(fName, LVbg, new delegateForSC(UpLoadSucceed), PublicDS.PublisDsUser.Tables[0].Rows[0]["卖家角色编号"].ToString());
                //FSC.ShowDialog();

            }
        }
        //查看
        private void label19_Click(object sender, EventArgs e)
        {
            //有地址
            //if (LVbg.Items.Count > 0 && LVbg.Items[0].SubItems[1].Text.Trim() != "")
            //{
            //    string url = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[1] + "/JHJXPT/SaveDir/" + LVbg.Items[0].SubItems[1].Text.Trim().Replace(@"\", "/");
            //    StringOP.OpenUrl(url);
            //}
        }
        //删除图片
        private void label6_Click(object sender, EventArgs e)
        {
            //LVbg.Items.Clear();
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
            //if (LVzlbz.Items.Count > 0)
            //{
            //    B_SCCK.Visible = true;
            //    B_SCSC.Visible = true;
            //}
            //else
            //{
            //    B_SCCK.Visible = false;
            //    B_SCSC.Visible = false;
            //}

            //if (LVbg.Items.Count > 0)
            //{
            //    label19.Visible = true;
            //    label6.Visible = true;
            //}
            //else
            //{
            //    label19.Visible = false;
            //    label6.Visible = false;
            //}
        }

        #endregion
        
        #region//选择投标商品
        private void lblSelectWarse_Click(object sender, EventArgs e)
        {
            Hashtable HT = new Hashtable();
            HT["结算账户"] = "买家";
            SelectTBWarse selectWarse = new SelectTBWarse(HT, new delegateForThread(Returnsomethings));
            selectWarse.ShowDialog();
        }
        private void Returnsomethings(Hashtable return_ht)
        {
            txtSPMC.Text = return_ht["商品名称"].ToString();
            txtXHGG.Text = return_ht["规格型号"].ToString();
            txtDW.Text = return_ht["计价单位"].ToString();
            lblMaxJJPL.Text = return_ht["所有标的中最大经济批量"].ToString() ;
            lblTBJ.Text = return_ht["投标价格"].ToString() + "元，";
            txtGHZQ.Text = return_ht["合同周期"].ToString();            
            label3.Text = return_ht["商品编号"].ToString();
            //label7.Text =  return_ht["投标信息号"].ToString();
            string showstr = "当前品类商品最低投标价：" + return_ht["投标价格"].ToString() + "元；\r\n\r\n最低价投标拟售量：" + return_ht["投标拟售量"].ToString() + return_ht["计价单位"].ToString() + "；\r\n\r\n参与竞标集合预订量：" + return_ht["集合预订量"].ToString() + return_ht["计价单位"].ToString() + "；\r\n\r\n所有标的中最大经济批量：" + return_ht["所有标的中最大经济批量"].ToString() + return_ht["计价单位"].ToString() + "。";
            label23.Text = showstr;            
            label23.Visible = true;
        }

        #endregion

        #region//拟买入价格、预定量输入框变化时
        private void txtNMRJG_TextChanged(object sender, EventArgs e)
        {
            double nmrjg = Convert.ToDouble(txtNMRJG.Text.Trim() == "" ? "0" : txtNMRJG.Text.Trim());//拟买入价格
            int ydl = Convert.ToInt32(txtYDL.Text.Trim() == "" ? "0" : txtYDL.Text.Trim());//预定量
            double MJTBBZJBL = Convert.ToDouble(PublicDS.PublisDsData.Tables["动态参数"].Rows[0]["BUYYDDDJ"].ToString().Trim());
            label28.Text = "预订单金额：" + (nmrjg * ydl).ToString() + "元  支付订金：" + (Convert.ToDouble((nmrjg * ydl).ToString()) * MJTBBZJBL).ToString("f2") + "元";
        }

        private void txtYDL_TextChanged(object sender, EventArgs e)
        {
            double nmrjg = Convert.ToDouble(txtNMRJG.Text.Trim() == "" ? "0" : txtNMRJG.Text.Trim());//拟买入价格
            int ydl = Convert.ToInt32(txtYDL.Text.Trim() == "" ? "0" : txtYDL.Text.Trim());//预定量           
            double MJTBBZJBL = Convert.ToDouble(PublicDS.PublisDsData.Tables["动态参数"].Rows[0]["BUYYDDDJ"].ToString().Trim());
            label28.Text = "预订单金额：" + (nmrjg * ydl).ToString() + "元  支付订金：" + (Convert.ToDouble((nmrjg * ydl).ToString()) * MJTBBZJBL).ToString("f2") + "元";
        }

        #endregion
        //重置
        private void ResetB_Click(object sender, EventArgs e)
        {
            //重置本表单，或转向其他页面
            FBYDD UC = new FBYDD();//这句话是关键，若实例化本身的类，则是重置表单，若实例化其他的类，则是跳转
            reset(UC);
        }

        //初始化参数列表显示集合
        private DataTable initListDataTable()
        {
            DataTable tb = new DataTable();
            tb.TableName = "预订单";
            tb.Columns.Add("买家登录邮箱");
            tb.Columns.Add("投标类型");
            tb.Columns.Add("合同周期");
            tb.Columns.Add("商品编号");
            tb.Columns.Add("商品名称");
            tb.Columns.Add("规格型号");
            tb.Columns.Add("计价单位");
            tb.Columns.Add("经济批量");
            tb.Columns.Add("预定总数量");
            tb.Columns.Add("拟买入价格");
            tb.Columns.Add("当前位次投标价格");
            tb.Columns.Add("收货区域");
            return tb;
        }
        /// <summary>
        /// 初始化提交参数的数据集
        /// </summary>
        /// <returns></returns>
        private DataSet initListDataSet()
        {
            DataSet ds = new DataSet();

            DataTable auto = initListDataTable();
            //auto.PrimaryKey = new DataColumn[] { auto.Columns["控件名称"] };

            DataTable auto2 = new DataTable();
            auto2.TableName = "主表";
            auto2.Columns.Add("买家角色编号");
            auto2.Columns.Add("投标保证金金额");
            auto2.Columns.Add("特殊标记");

            ds.Tables.Add(auto2);
            ds.Tables.Add(auto);
            return ds;
        }
        /// <summary>
        /// 获取首次进入的验证信息
        /// </summary>
        /// <returns></returns>
        public DataSet GetYZXX()
        {
            string JSNM = PublicDS.PublisDsUser.Tables[0].Rows[0]["买家角色编号"].ToString().Trim();
            DataSet InPutdsCS = initListDataSet();
            InPutdsCS.Tables["主表"].Rows.Add(new string[] { JSNM, "0", "仅检查基础" });

            return InPutdsCS;
        }

        bool runonece = true;//只运行一次
        private void panelUC5_Paint(object sender, PaintEventArgs e)
        {
            if (runonece)
            {
                DataSet InPutdsCS = GetYZXX();
                //开启线程提交数据
                delegateForThread dft = new delegateForThread(ShowThreadResult_save);
                DataControl.RunThreadClassXDYDDXXSave RTC = new DataControl.RunThreadClassXDYDDXXSave(InPutdsCS, dft);
                Thread trd = new Thread(new ThreadStart(RTC.BeginRun));
                trd.IsBackground = true;
                trd.Start();
                runonece = false;
            }
        }

       


    }
}
