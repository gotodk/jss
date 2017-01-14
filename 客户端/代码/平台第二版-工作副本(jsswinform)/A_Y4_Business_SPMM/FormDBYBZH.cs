using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Com.Seezt.Skins;
using System.Collections;
using System.Threading;
using 客户端主程序.NewDataControl;

namespace 客户端主程序.SubForm.NewCenterForm.SUBUC
{
    public partial class FormDBYBZH : BasicForm
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
        public FormDBYBZH(Hashtable ht)
        {

            InitializeComponent();
            ht_where = ht;
            //初始化分页回调(带分页的页面，先把这个放上)
           
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
            
            //将滚动条置顶
            
            Hashtable ht_Number = new Hashtable();
            ht_Number["Number"] = ht_where["Number"].ToString(); ;
            RunThreadClasshwqs g = new RunThreadClasshwqs(ht_Number, new delegateForThread(XG));
            Thread trd = new Thread(new ThreadStart(g.TBDYDDbzh_GetCGinfo));
            trd.IsBackground = true;
            trd.Start();
           

        }


        #region 获取需要显示的数据
        private void XG(Hashtable OutPutHT)
        {
            try { Invoke(new delegateForThreadShow(XG_Invoke), new Hashtable[] { OutPutHT }); }
            catch (Exception ex) { Support.StringOP.WriteLog("委托回调错误：" + ex.ToString()); }
        }
        private void XG_Invoke(Hashtable OutPutHT)
        {
           
            DataSet dsreturn = (DataSet)OutPutHT["结果"];
            if (dsreturn == null)
            {
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add("未查找到任何数据！");
                FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "中国商品批发交易平台", al);
                fm.ShowDialog();
            }
            else
            {
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
                if (zt == "ok")
                {
                    DataTable dt = dsreturn.Tables["SPXX"];
                    DataRow dr = dt.Rows[0];
                    labhtbh.Text = dr["合同编号"].ToString();
                    labjjpl.Text = dr["经济批量"].ToString();
                    if (dr["合同期限"].ToString().Trim().Equals("即时"))
                    {
                        labrjzgfhl.Text = "一次性供货";
                    }
                    else
                    {
                        labrjzgfhl.Text = dr["日均最高发货量"].ToString();
                    }
                    labdbsl.Text = dr["定标数量"].ToString();
                    labmjmc.Text = dr["卖家名称"].ToString();
                    lablxfs.Text = dr["卖家联系方式"].ToString();
                    lablxr.Text = dr["卖家联系人"].ToString();
                    labbuermc.Text = dr["买家家名称"].ToString();
                    labbuyerlxfs.Text = dr["买家联系方式"].ToString();
                    labbuyerlxr.Text = dr["买家联系人"].ToString();

                   
                   
                }
            }
        }
        #endregion


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



       




    }
}
