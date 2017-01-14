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

namespace 客户端主程序.SubForm.NewCenterForm.SUBUC
{
    public partial class jhjx_hwqsForm : BasicForm
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
        public jhjx_hwqsForm(ucHWQS_B hwqs)
        {

            InitializeComponent();
            dftForParent = hwqs;
            ht_where = hwqs.ht;
            //初始化分页回调(带分页的页面，先把这个放上)
            this.Text = (hwqs.ht)["type"].ToString();
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
            PBload.Location = new Point(BSave.Location.X + BSave.Width + 30, BSave.Location.Y);

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
            this.panbfshmes.Visible = false;
            this.payspho.Visible = false;
            this.palzzfhdyyj.Visible = false;
            this.palsjqssl.Visible = false;
            this.palqksm.Visible = false;

            string Strtype = ht_where["type"].ToString().Trim();
            switch (Strtype)
            {
                case "请重新发货":
                    this.payspho.Visible = true;
                    this.palqksm.Visible = true;
                    this.Height = 336 - this.panbfshmes.Height - this.palzzfhdyyj.Height - this.palsjqssl.Height;
                    break;
                case "部分收货":
                    this.payspho.Visible = true;
                    //this.palqksm.Visible = true;
                    this.palsjqssl.Visible = true;
                    this.panbfshmes.Visible = true;
                    this.palzzfhdyyj.Visible = true;
                    this.Height = 336 - this.palqksm.Height;
                    break;
                case "有异议收货":
                    this.payspho.Visible = true;
                    this.palqksm.Visible = true;
                    this.palzzfhdyyj.Visible = true;
                    this.Height = 336 - this.palsjqssl.Height - this.panbfshmes.Height;
                    break;
                default:
                    break;

            }
        }

        private void bbcancel_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void BSave_Click(object sender, EventArgs e)
        {
            //禁用提交区域并开启进度
            palbtn.Enabled = false;
            PBload.Visible = true;
            ht_where["验收照片路径"] = "";
            ht_where["情况说明"] = "";
            ht_where["部分签收数量"] = "";
            ht_where["发货单（物流单）影印件"] = "";
            ht_where["islegal"] = "no";
            ht_where["from"] = this;

           
            string Strtype = ht_where["type"].ToString().Trim();
            switch (Strtype)
            {
                case "请重新发货":
                    this.payspho.Visible = true;
                    this.palqksm.Visible = true;
                    this.Height = 372 - this.panbfshmes.Height - this.palzzfhdyyj.Height - this.palsjqssl.Height;
                    if (this.uCshangchuan2.UpItem != null && this.uCshangchuan2.UpItem.Items.Count > 0)
                    {
                        if (ucTxtqksm.Text.Trim().Equals(""))
                        {
                            palbtn.Enabled = true;
                            PBload.Visible = false;

                            ArrayList Almsg3 = new ArrayList();
                            Almsg3.Add("");
                            Almsg3.Add("请录入重新发货的情况说明！");
                            FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "其他", "", Almsg3);
                            FRSE3.ShowDialog();
                            return;
                        }

                        ht_where["验收照片路径"] = this.uCshangchuan2.UpItem.Items[0].SubItems[1].Text.ToString().Trim();
                        ht_where["情况说明"] = this.ucTxtqksm.Text;
                        ht_where["islegal"] = "yes";
                    }

                    break;
                case "部分收货":
                    this.payspho.Visible = true;
                    //this.palqksm.Visible = true;
                    this.palsjqssl.Visible = true;
                    this.panbfshmes.Visible = true;
                    this.palzzfhdyyj.Visible = true;
                    this.Height = 372 - this.palqksm.Height;
                    if (this.uCshangchuan2.UpItem != null && this.uCshangchuan2.UpItem.Items.Count > 0 && this.uCshangchuan1.UpItem != null && this.uCshangchuan1.UpItem.Items.Count > 0)
                    {
                        if (ucTxtsl.Text.Trim().Equals(""))
                        {
                            palbtn.Enabled = true;
                            PBload.Visible = false;

                            ArrayList Almsg3 = new ArrayList();
                            Almsg3.Add("");
                            Almsg3.Add("请输入签收数量！");
                            FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "其他", "", Almsg3);
                            FRSE3.ShowDialog();
                            return;
                        }

                        if (Convert.ToInt32(ucTxtsl.Text.Trim()) <= 0)
                        {
                            palbtn.Enabled = true;
                            PBload.Visible = false;
                            ArrayList Almsg3 = new ArrayList();
                            Almsg3.Add("");
                            Almsg3.Add("签收数量必须大于0！");
                            FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "其他", "", Almsg3);
                            FRSE3.ShowDialog();
                            return;
                        }
                        ht_where["验收照片路径"] = this.uCshangchuan2.UpItem.Items[0].SubItems[1].Text.ToString().Trim();
                       // ht_where["情况说明"] = this.ucTxtqksm.Text;
                        ht_where["部分签收数量"] = ucTxtsl.Text;
                        ht_where["发货单（物流单）影印件"] = this.uCshangchuan1.UpItem.Items[0].SubItems[1].Text.ToString().Trim();
                        ht_where["islegal"] = "yes";
                    }
                    break;
                case "有异议收货":
                    this.payspho.Visible = true;
                    this.palqksm.Visible = true;
                    this.palzzfhdyyj.Visible = true;
                    this.Height = 372 - this.palsjqssl.Height - this.panbfshmes.Height;
                    if (this.uCshangchuan2.UpItem != null && this.uCshangchuan2.UpItem.Items.Count > 0 && this.uCshangchuan1.UpItem != null && this.uCshangchuan1.UpItem.Items.Count > 0)
                    {
                        if (ucTxtqksm.Text.Trim().Equals(""))
                        {
                            palbtn.Enabled = true;
                            PBload.Visible = false;

                            ArrayList Almsg3 = new ArrayList();
                            Almsg3.Add("");
                            Almsg3.Add("有异议收货情况说明不能为空！");
                            FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "其他", "", Almsg3);
                            FRSE3.ShowDialog();
                            return;
                        }
                        ht_where["验收照片路径"] = this.uCshangchuan2.UpItem.Items[0].SubItems[1].Text.ToString().Trim();
                        ht_where["情况说明"] = this.ucTxtqksm.Text;
                        //ht_where["部分签收数量"] = ucTxtsl.Text;
                        ht_where["发货单（物流单）影印件"] = this.uCshangchuan1.UpItem.Items[0].SubItems[1].Text.ToString().Trim();
                        ht_where["islegal"] = "yes";
                    }
                    break;
                default:

                    break;

            }
            if (ht_where["islegal"].ToString() == "yes")
            {
                dftForParent.ht = ht_where;
                dftForParent.SRT_demo_Run(ht_where);
                palbtn.Enabled = false;
                PBload.Visible = true;
                
            }
            else
            {
                //Thread.Sleep(5000);
                palbtn.Enabled = true;
                PBload.Visible = false;

                ArrayList Almsg3 = new ArrayList();
                Almsg3.Add("");
                Almsg3.Add("请将附件上传完成后再提交！");
                FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "其他", "", Almsg3);
                FRSE3.ShowDialog();
            }
        }


    }
}
