using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Threading;
using 客户端主程序.NewDataControl;
using Com.Seezt.Skins;

namespace 客户端主程序.SubForm.NewCenterForm.SUBUC
{
    public partial class DEMO2 : UserControl
    {
        public DEMO2()
        {
            InitializeComponent();
        }

        //处理下拉框间距
        private void CB_DrawItem(object sender, DrawItemEventArgs e)
        {
            
            ComboBox CBthis = (ComboBox)sender;
            if (e.Index < 0)
            {
                return;
            }
            e.DrawBackground();
            e.DrawFocusRectangle();
            e.Graphics.DrawString(CBthis.Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds.X + 2, e.Bounds.Y + 3);
        }


        private void DEMO2_Load(object sender, EventArgs e)
        {
            //将滚动条置顶
            UPUP();
            //设置进度条的位置，放到按钮旁边
            PBload.Location = new Point(BSave.Location.X + BSave.Width + 30, BSave.Location.Y);

            //处理下拉框间距
            CBleixing.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.CB_DrawItem);

            //给上传控件赋值
            //取数据
            ListView lv = uCshangchuan1.UpItem;
            //赋值(一定要先取出来)
            lv.Items.Clear();
            lv.Items.Add(new ListViewItem(new string[] { "", "数据库记录的远程路径", "", "", "" }));


            //uCshangchuan1.showB = new bool[] { false,true,false};
        }


 
        /// <summary>
        /// 重置表单
        /// </summary>
        private void reset()
        {
            this.Dock = DockStyle.Fill;//铺满
            this.AutoScroll = true;//出现滚动条
            this.BackColor = Color.AliceBlue;
            
            //清理控件
            Panel P = (Panel)this.Parent;
            P.Controls.Clear();
            P.Controls.Add(this); //加入到某一个panel中
            this.Show();//显示出来
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


        private void BSave_Click(object sender, EventArgs e)
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
            OpenThreadDemo OTD = new OpenThreadDemo(null, new delegateForThread(SRT_demo));
            Thread trd = new Thread(new ThreadStart(OTD.BeginRun));
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
            panelTJQ.Enabled = true;
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

                    break;
            }
        }

        private void Breset_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {
            ((Control)sender).Focus();
            fmDY dy = new fmDY("打印内容标题","一个参数",new string[] { "客户端主程序.SubForm.NewCenterForm.UCdayindemo", "客户端主程序.SubForm.NewCenterForm.UCdayindemo2" });
            dy.Show();
        }

        private void ce_Click(object sender, EventArgs e)
        {
            ((Control)sender).Focus();
            fmDY_Demo dy = new fmDY_Demo("打印内容标题", "一个参数", new string[] { "客户端主程序.SubForm.NewCenterForm.UCdayindemo" });
            dy.Show();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            ((Control)sender).Focus();
            
            int pagenumber = 20;//预估页数，要大于可能的最大页数
            object[] CSarr = new object[pagenumber];
            string[] MKarr = new string[pagenumber];
            for (int i = 0; i < pagenumber; i++)
            {
                Hashtable htcs = new Hashtable();
                htcs["纯数字索引"] = (i + 1).ToString(); //这个参数必须存在。
                htcs["数据地址"] = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[2] + "/pingtaiservices/mobanrun/cs1.aspx?moban=ceshi.htm&pagenumber=1&htbh=xxxxx111";
                htcs["要传递的参数1"] = "参数1";
                CSarr[i] = htcs; //模拟参数
                MKarr[i] = "Y3_Business_Muban.dll)!客户端主程序.SubForm.NewCenterForm.PrintUC.UCdzghxy";
            }
            fmDY dy = new fmDY("远程截图打印测试", CSarr, MKarr);
            dy.Show();
           
        }

        private void label6_Click(object sender, EventArgs e)
        {

            //隐藏打开要打包下载的打印页(这里是模式打印同一个东西不同的标题)
            int pagenumber = 20;//预估页数，要大于可能的最大页数
            object[] CSarr = new object[pagenumber];
            string[] MKarr = new string[pagenumber];
            for (int i = 0; i < pagenumber; i++)
            {
                Hashtable htcs = new Hashtable();
                htcs["纯数字索引"] = (i + 1).ToString(); //这个参数必须存在。
                htcs["数据地址"] = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[2] + "/pingtaiservices/mobanrun/cs1.aspx?moban=ceshi.htm&pagenumber=1&htbh=xxxxx111";
                htcs["要传递的参数1"] = "参数1";
                CSarr[i] = htcs; //模拟参数
                MKarr[i] = "Y3_Business_Muban.dll)!客户端主程序.SubForm.NewCenterForm.PrintUC.UCdzghxy";
            }
            fmDY dy = new fmDY("测试打包下载1", CSarr, MKarr);
            dy.Show();
            dy.Hide();
            dy.baocuntupian();

            dy = new fmDY("测试打包下载2", CSarr, MKarr);
            dy.Show();
            dy.Hide();
            dy.baocuntupian();

            dy = new fmDY("测试打包下载3", CSarr, MKarr);
            dy.Show();
            dy.Hide();
            dy.baocuntupian();



 

        }


    }
}
