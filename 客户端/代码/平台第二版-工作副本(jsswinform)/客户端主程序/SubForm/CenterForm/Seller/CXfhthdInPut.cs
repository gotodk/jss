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
using 客户端主程序.DataControl;
using System.Threading;
using 客户端主程序.Support;


namespace 客户端主程序.SubForm.CenterForm.Seller
{
    public partial class CXfhthdInPut : BasicForm
    {
        //淡出计时器
        System.Windows.Forms.Timer Timer_DC;

        CXfhthd CXb;
        

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

        public CXfhthdInPut(CXfhthd CXbtemp)
        {
    
            InitializeComponent();
            CXb = CXbtemp;
            this.TitleYS = new int[] { 0, 0, -50 };

            //图标
            Icon ic = new Icon(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase+ @"\skin\ji.ico");
            this.Icon = ic;
        }

        private void CXweizhongbiaoEdit_Load(object sender, EventArgs e)
        {

            LLLNumber.Text = CXb.Enumber;
            LLLxdsj.Text = CXb.Exdsj;
            LLLzcfhr.Text = CXb.Ezcfhr;

            string showstr = "";
            showstr = showstr + "收货单位名称：" + CXb.ESHGSMC + "\r\n";
            showstr = showstr + "区域：" + CXb.ESZSF + CXb.ESZDS + CXb.ESZQX + "\r\n";
            showstr = showstr + "详细地址：" + CXb.EXXDZ + "\r\n";
            showstr = showstr + "邮政编码：" + CXb.EYZBM + "\r\n联系电话：" + CXb.ELXDH + "\r\n";
            UCdizhi.Text = showstr;
       
        }

        /// <summary>
        /// 重设进度条位置，放到按钮旁边
        /// </summary>
        /// <param name="BB">参考提交按钮</param>
        /// <param name="PB">要移动的进度条</param>
        private void resetloadLocation(BasicButton BB, PictureBox PB)
        {
            PB.Location = new Point(BB.Location.X + BB.Width + 30, BB.Location.Y +23 );
            PB.Visible = true;
        }

        /// <summary>
        /// 显示错误提示
        /// </summary>
        /// <param name="err"></param>
        private void showAlertY(string err)
        {
            ArrayList Almsg3 = new ArrayList();
            Almsg3.Add("");
            Almsg3.Add(err);
            FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "错误", "投标信息提交", Almsg3);
            FRSE3.ShowDialog();
        }

    



        private void BBedit_Click_1(object sender, EventArgs e)
        {
            ArrayList Almsg3 = new ArrayList();
            Almsg3.Add("");
            Almsg3.Add("您确定要进行发货吗？");
            FormAlertMessage FRSE3 = new FormAlertMessage("确定取消", "问号", "xxx", Almsg3);
            DialogResult dr = FRSE3.ShowDialog();
            if (dr != DialogResult.Yes)
            {
                return;
            }
            //基本验证
            if (UCwlgs.Text.Trim() == "")
            {
                showAlertY("物流公司不能为空。");
                return;
            }
            if (UCwldh.Text.Trim() == "")
            {
                showAlertY("物流单号不能为空。");
                return;
            }
            if (UCfph.Text.Trim() == "")
            {
                showAlertY("发票号码不能为空。");
                return;
            }



            //提交修改
            

            //生成提交参数
            string SELjsbh = PublicDS.PublisDsUser.Tables[0].Rows[0]["卖家角色编号"].ToString();
            DataSet dsinfosave = new DataSet();
            Hashtable htcs = new Hashtable();
            htcs["卖家角色编号"] = SELjsbh;
            htcs["发货单号"] = LLLNumber.Text;
            htcs["物流公司"] = UCwlgs.Text;
            htcs["物流单号"] = UCwldh.Text;
            htcs["发票号码"] = UCfph.Text;

            DataTable dtcs = StringOP.GetDataTableFormHashtable(htcs);
            dsinfosave.Tables.Add(dtcs);
            //开启线程，获取一些数据。
            //禁用提交区域并开启进度
      
            panelUCedit.Enabled = false;
            resetloadLocation(BBedit, PBload);

            delegateForThread dft = new delegateForThread(ShowThreadResult_save);

            DataControl.RunThreadClassTHD RCthd = new DataControl.RunThreadClassTHD(LLLNumber.Text, SELjsbh, dsinfosave, dft);
            Thread trd = new Thread(new ThreadStart(RCthd.BeginRun_FaHuo));
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
          
            panelUCedit.Enabled = true;
            PBload.Visible = false;
            //显示执行结果
            string returnds = returnHT["执行结果"].ToString();
            switch (returnds)
            {
                case "ok":

                    //给出表单提交成功的提示
                    ArrayList Almsg3 = new ArrayList();
                    Almsg3.Add("");
                    Almsg3.Add("发货信息提交成功！");
                    FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "叹号", "处理成功", Almsg3);
                    FRSE3.ShowDialog();

                    //重新绑定数据
                    CXb.GetData(null);
                    this.Close();

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
                    Almsg4.Add(returnds);// 这里是提交失败的提示，可能是程序出错，可能是条件不足等
                    FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "错误", "提交xxx失败", Almsg4);
                    FRSE4.ShowDialog();

                    break;
            }

        }

    }
}
