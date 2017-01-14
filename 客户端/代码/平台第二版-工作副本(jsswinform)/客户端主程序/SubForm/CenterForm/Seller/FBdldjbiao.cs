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

namespace 客户端主程序.SubForm.CenterForm.Seller
{
    public partial class FBdldjbiao : UserControl
    {
        public FBdldjbiao()
        {
            InitializeComponent();
        }





        //模拟提交数据
        private void basicButton2_Click(object sender, EventArgs e)
        {
            //获得表单数据和进行基本验证
            Hashtable ht_form = new Hashtable();
            ht_form["str1"] = ustxtTBLX.Text.Trim();
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
                    FBdldjbiao UC = new FBdldjbiao();//这句话是关键，若实例化本身的类，则是重置表单，若实例化其他的类，则是跳转
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
        private void reset(FBdldjbiao UC)
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


        /// <summary>
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
                FormSC FSC = new FormSC(fName, listView1, new delegateForSC(UpLoadSucceed), "测试角色编号12345678");
                FSC.ShowDialog();

            }
        }

        //查看
        private void B_SCCK_Click_1(object sender, EventArgs e)
        {
            //有地址
            if (listView1.Items.Count > 0 && listView1.Items[0].SubItems[1].Text.Trim() != "")
            {
                string url = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[1] + "/JHJXPT/SaveDir/" + listView1.Items[0].SubItems[1].Text.Trim().Replace(@"\", "/");
                StringOP.OpenUrl(url);
            }
        }

        //删除图片
        private void B_SCSC_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
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
            if (listView1.Items.Count > 0)
            {
                //B_SCCK.Visible = true;
                //B_SCSC.Visible = true;
            }
            else
            {
                //B_SCCK.Visible = false;
                //B_SCSC.Visible = false;
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
            //ucTextBox5.Text = return_ht["测试值"].ToString();
        }
        #endregion


        private void ResetB_Click(object sender, EventArgs e)
        {
            //重置本表单，或转向其他页面
            FBdldjbiao UC = new FBdldjbiao();//这句话是关键，若实例化本身的类，则是重置表单，若实例化其他的类，则是跳转
            reset(UC);
        }
        //选择商品
        private void lblSelectWarse_Click(object sender, EventArgs e)
        {
            Hashtable HT = new Hashtable();
            HT["投标"] = "定量定价";
            FBdldjSelectWarse selectWarse = new FBdldjSelectWarse(HT,new delegateForThread(ReSetsomethings));
            selectWarse.ShowDialog();
        }
        private void ReSetsomethings(Hashtable return_ht)
        {
            uctxtBSH.Text = return_ht["测试值"].ToString();
        }

        private void basicRadioButton3_CheckedChanged(object sender, bool Checked)
        {
            Hashtable HT = new Hashtable();
            HT["投标"] = "定量定价";
            FBdldjSelectWarse selectWarse = new FBdldjSelectWarse(HT, new delegateForThread(ReSetsomethings));
            selectWarse.ShowDialog();
        }

       



    }
}
