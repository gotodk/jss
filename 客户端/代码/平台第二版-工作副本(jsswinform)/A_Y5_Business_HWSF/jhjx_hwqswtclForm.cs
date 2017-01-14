using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Com.Seezt.Skins;
using System.Collections;
using 客户端主程序.SubForm.NewCenterForm.SUBUC.HWSF;
using System.Threading;
using 客户端主程序.NewDataControl;
using 客户端主程序.Support;

namespace 客户端主程序.SubForm.NewCenterForm.SUBUC
{
    public partial class jhjx_hwqswtclForm : BasicForm
    {
        //淡出计时器
        System.Windows.Forms.Timer Timer_DC;

        /// <summary>
        /// 查询条件
        /// </summary>
        Hashtable ht_where = new Hashtable();

        /// <summary>
        /// 对勾选择后的回调指针
        /// </summary>
        ucHWQS_B dftForParent;
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

        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="dftForParent_temp"></param>
        public jhjx_hwqswtclForm( Hashtable hwqs)
        {

            InitializeComponent();

            ht_where = hwqs;
            //初始化分页回调(带分页的页面，先把这个放上)
            this.Text = ht_where["type"].ToString();
            //this.Text = ht_where["type"].ToString();
            //图标
            Icon ic = new Icon(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\skin\ji.ico");
            this.Icon = ic;
            //设置窗体显示在屏幕中央
            this.StartPosition = FormStartPosition.CenterScreen;
            //不在任务栏显示此信息
            this.ShowInTaskbar = false;
        }

        private void jhjx_hwqsForm_Load(object sender, EventArgs e)
        {
            controlshow();
            //将滚动条置顶
            UPUP();
            //设置进度条的位置，放到按钮旁边
          

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


        private void controlshow()
        {
           // this.panbfshmes.Visible = false;
            this.payspho.Visible = false;
            this.palzzfhdyyj.Visible = false;
            this.palsjqssl.Visible = false;
            this.palqksm.Visible = false;
            //uCshangchuan2.showB = new bool[] { false, true, false };
           // uCshangchuan1.showB = new bool[] { false, true, false };
            ht_where["cloumn"] = "";
            string Strtype = ht_where["type"].ToString().Trim();
            switch (Strtype)
            {
                case "请重新发货":
                case "撤销":
                    this.payspho.Visible = true;
                    this.palqksm.Visible = true;
                    this.Height = 271 - this.palzzfhdyyj.Height - this.palsjqssl.Height;
                    ht_where["cloumn"] = " F_BUYQZXFHYSZP as 验收时照片,F_BUYQZXFHQKSM as 情况说明 ,F_BUYQZXFHCZSJ as 创建时间 ,'' as 部分收货数量,'' as '发货单(物流单)影印件'   ";
                    break;
                case "部分收货":
                case "已录入补发备注":
                case "补发货物无异议收货":
                    this.payspho.Visible = true;
                    //this.palqksm.Visible = true;
                    this.palsjqssl.Visible = true;
                    this.palzzfhdyyj.Visible = true;
                    this.Height = 271 - this.palqksm.Height;

                    ht_where["cloumn"] = " F_BUYBFSHSJQSSL as 部分收货数量, F_BUYBFSHFHDWLDYYJ as  '发货单(物流单)影印件' , F_BUYBFSHYSZP as 验收时照片,'' as 情况说明 ,F_BUYBFSHCZSJ as 创建时间  ";

                    break;
                case "有异议收货":
                case "有异议收货后无异议收货":
                case "卖方主动退货":

                    this.payspho.Visible = true;
                    this.palqksm.Visible = true;
                    this.palzzfhdyyj.Visible = true;
                    this.Height = 271 - this.palsjqssl.Height;
                    ht_where["cloumn"] = " '' as 部分收货数量, F_BUYYYYSHFHDWLDYYJ as  '发货单(物流单)影印件' , F_BUYYYYYSZP as 验收时照片,F_BUYYYYQKSM as 情况说明 ,F_BUYYYYSHCZSJ as 创建时间  ";
                    break;
                default:
                    break;

            }


            SRT_demo_Run(ht_where);

        }

        private void bbcancel_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        public void SRT_demo_Run(Hashtable InPutHT)
        {
            RunThredgetycinfo OTD = new RunThredgetycinfo(InPutHT, new delegateForThread(SRT_demo));
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
            this.Enabled = true;
            //返回值后的处理
            DataSet dsreturn = (DataSet)OutPutHT["返回值"];

            string zt = dsreturn.Tables["返回值单条"].Rows[0]["执行结果"].ToString();
            string showstr = dsreturn.Tables["返回值单条"].Rows[0]["提示文本"].ToString();
            string Nowuyiyi = dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"].ToString();
            DataTable dtinfo = dsreturn.Tables["info"];
            //显示执行结果
            switch (zt)
            {
                case "ok":
                        
            //取数据

           //ListView lv = null;//uCshangchuan2.UpItem;
            //赋值(一定要先取出来)

            listView1.Items.Clear();
            string str = dtinfo.Rows[0]["验收时照片"].ToString();
            listView1.Items.Add(new ListViewItem(new string[] { "", dtinfo.Rows[0]["验收时照片"].ToString(), "", "", "" }));

            //ListView lv1 = uCshangchuan1.UpItem;
            //赋值(一定要先取出来)
            listView2.Items.Clear();
            listView2.Items.Add(new ListViewItem(new string[] { "", dtinfo.Rows[0]["发货单(物流单)影印件"].ToString(), "", "", "" }));

            ucTxtsl.Text = dtinfo.Rows[0]["部分收货数量"].ToString();
            ucTxtqksm.Text = dtinfo.Rows[0]["情况说明"].ToString();
            labcljg.Text = dtinfo.Rows[0]["F_DQZT"].ToString();
                    //给出表单提交成功的提示
              
             
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
        ImageShow IS;
        private void B_SCCK_Click(object sender, EventArgs e)
        {
            Label lb = sender as Label;
            ListView lv = new ListView();
            lv.Clear();
            if (lb.Name == "B_SCCK")
            {
                lv = this.listView1;
            }
            if (lb.Name == "label3")
            {
                lv = listView2;
            }

            if (lv.Items.Count > 0 && lv.Items[0].SubItems[1].Text.Trim() != "")
            {
                string url = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[2] + "/JHJXPT/SaveDir/" + lv.Items[0].SubItems[1].Text.Trim().Replace(@"\", "/");
                //StringOP.OpenUrl(url);

                Hashtable htT = new Hashtable();
                htT["类型"] = "网址";
                htT["地址"] = url;
                if (IS == null || IS.IsDisposed == true)
                {
                    IS = new ImageShow(htT);
                }
                IS.Show();
                IS.Activate();

            }

        }

       

       


    }
}
