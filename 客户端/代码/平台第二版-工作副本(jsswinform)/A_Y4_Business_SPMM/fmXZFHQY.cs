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

namespace 客户端主程序.SubForm
{
    public partial class fmXZFHQY : BasicForm
    {
        //淡出计时器
        System.Windows.Forms.Timer Timer_DC;

        /// <summary>
        /// 对勾选择后的回调指针
        /// </summary>
        delegateForThread dftForParent;
     

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
        /// <param name="htQY"></param>
        public fmXZFHQY(delegateForThread dftForParent_temp,Hashtable htQY)
        {
    
            InitializeComponent();
            dftForParent = dftForParent_temp;
            //图标
            Icon ic = new Icon(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase+ @"\skin\ji.ico");
            this.Icon = ic;
            //设置窗体显示在屏幕中央
            this.StartPosition = FormStartPosition.CenterScreen;
            //不在任务栏显示此信息
            this.ShowInTaskbar = false;
            #region 如果传递过来的HashTable不为空，则需要绑定选中状态
            if (htQY != null)
            {
                string QY = htQY["控件名称"].ToString();
                string[] ay = htQY["控件名称"].ToString().Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                
                foreach (Control ctl in panelUC5.Controls)
                {
                    if (QY.Contains(ctl.Name))
                    {
                        BasicCheckBox bchk = ctl as BasicCheckBox;
                        foreach (string str in ay)
                        {
                            if (str == ctl.Name)
                            {
                                bchk.Checked = true;
                                bchk.CheckState = BasicCheckBox.CheckStates.Checked;
                                continue;
                            }
                        }
                        
                    }
                }
            }
            #endregion

            foreach (Control ctl in panelUC5.Controls)
            {
                if (ctl is Com.Seezt.Skins.BasicCheckBox)
                {
                    Com.Seezt.Skins.BasicCheckBox chk = ctl as Com.Seezt.Skins.BasicCheckBox;
                    if (chk.Tag.ToString().IndexOf("_") > 0)
                    {
                        chk.CheckedChanged += onChkChange;
                    }
                    else
                    {
                        chk.CheckedChanged += ged;
                    }
                    //chk.CheckState = BasicCheckBox.CheckStates.Checked;
                    //chk.Checked = true;
                }
            }
        }
        /// <summary>
        /// 重载构造函数，只在修改订单时用，存在小BUG
        /// </summary>
        /// <param name="dftForParent_temp"></param>
        /// <param name="htQY"></param>
        /// <param name="b"></param>
        public fmXZFHQY(delegateForThread dftForParent_temp, Hashtable htQY,bool bFlag)
        {

            InitializeComponent();
            dftForParent = dftForParent_temp;
            //图标
            Icon ic = new Icon(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\skin\ji.ico");
            this.Icon = ic;
            //设置窗体显示在屏幕中央
            this.StartPosition = FormStartPosition.CenterScreen;
            //不在任务栏显示此信息
            this.ShowInTaskbar = false;
            #region 如果传递过来的HashTable不为空，则需要绑定选中状态
            if (htQY != null)
            {
                string QY = htQY["发货区域"].ToString();
                string[] ay = htQY["发货区域"].ToString().Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);

                foreach (Control ctl in panelUC5.Controls)
                {
                    if (ctl is BasicCheckBox)
                    {
                        BasicCheckBox b = ctl as BasicCheckBox;
                        if (QY.Contains(b.Texts))
                        {
                            b.Checked = true;
                            b.CheckState = BasicCheckBox.CheckStates.Checked;
                        }
                    }
                }
            }
            #endregion



            foreach (Control ctl in panelUC5.Controls)
            {
                if (ctl is Com.Seezt.Skins.BasicCheckBox)
                {
                    Com.Seezt.Skins.BasicCheckBox chk = ctl as Com.Seezt.Skins.BasicCheckBox;
                    if (chk.Tag.ToString().IndexOf("_") > 0)
                    {
                        chk.CheckedChanged += onChkChange;
                    }
                    else
                    {
                        chk.CheckedChanged += ged;
                    }
                    //chk.CheckState = BasicCheckBox.CheckStates.Checked;
                    //chk.Checked = true;
                }
            }

        }



        private void btnQueDing_Click(object sender, EventArgs e)
        {
            Hashtable return_ht = new Hashtable();
            
            //回调
            string QY = "";
            string Names = "";
            foreach (Control ctl in panelUC5.Controls)
            {
                if (ctl is BasicCheckBox)
                {
                    BasicCheckBox bchk = ctl as BasicCheckBox;
                    if (bchk.Checked && (!(isQY(bchk))))
                    {
                        QY += bchk.Texts + "|";
                    }
                    if (bchk.Checked)
                    {
                        Names += bchk.Name + "|";
                    }
                }
            }
            QY = "|" + QY;
            if (QY.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries).Length < 2)
            {
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add("您的供货区域不能为空！供货区域不得少于两个省份。");
                FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "", al);
                fm.ShowDialog();
                return;
            }
            return_ht["发货区域"] = QY;
            return_ht["控件名称"] = Names;
            dftForParent(return_ht);

            //关闭弹窗
            this.Close();

        }
        /// <summary>
        /// 全选按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void basicRadioButton1_Click(object sender, EventArgs e)
        {
            foreach (Control ctl in panelUC5.Controls)
            {
                if (ctl is Com.Seezt.Skins.BasicCheckBox)
                {
                    Com.Seezt.Skins.BasicCheckBox chk = ctl as Com.Seezt.Skins.BasicCheckBox;
                    chk.CheckState = BasicCheckBox.CheckStates.Checked;
                    chk.Checked = true;
                }
            }
            
        }
        /// <summary>
        /// 反选按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void basicRadioButton2_Click(object sender, EventArgs e)
        {
            foreach (Control ctl in panelUC5.Controls)
            {
                if (ctl is Com.Seezt.Skins.BasicCheckBox)
                {
                    Com.Seezt.Skins.BasicCheckBox chk = ctl as Com.Seezt.Skins.BasicCheckBox;
                    // && ctl.Tag.ToString().IndexOf("_") > 0)
                    if (chk.Tag.ToString().IndexOf("_") > 0)
                    {
                        if (chk.CheckState == BasicCheckBox.CheckStates.Checked || chk.Checked)
                        {
                            chk.CheckState = BasicCheckBox.CheckStates.Unchecked;
                            chk.Checked = false;
                            continue;
                        }
                        if (chk.CheckState == BasicCheckBox.CheckStates.Unchecked || (!chk.Checked))
                        {
                            chk.CheckState = BasicCheckBox.CheckStates.Checked;
                            chk.Checked = true;
                            continue;
                        }
                    }
                    else
                    {
                         if (chk.CheckState == BasicCheckBox.CheckStates.Checked || chk.Checked)
                        {
                            chk.CheckState = BasicCheckBox.CheckStates.Unchecked;
                            chk.Checked = false;
                            continue;
                        }
                        //if (chk.CheckState == BasicCheckBox.CheckStates.Unchecked || (!chk.Checked))
                        //{
                        //    chk.CheckState = BasicCheckBox.CheckStates.Checked;
                        //    chk.Checked = true;
                        //    continue;
                        //}
                    }
                }
            }
        }

        /// <summary>
        /// 判断此控件是否为大区域而不是精确到省
        /// </summary>
        /// <param name="bchk"></param>
        /// <returns></returns>
        protected bool isQY(BasicCheckBox bchk)
        {
            bool b = true;
            if (bchk.Tag.ToString() == "huabei" || bchk.Tag.ToString() == "huanan" || bchk.Tag.ToString() == "huazhong" || bchk.Tag.ToString() == "huadong" || bchk.Tag.ToString() == "dongbei" || bchk.Tag.ToString() == "xibei" || bchk.Tag.ToString() == "xinan")
                b = true;
            else
                b = false;
            return b;
        }

        /// <summary>
        /// 公用选择方法
        /// </summary>
        /// <param name="CHB">BasicCheckBox实例</param>
        /// <param name="Checked">选择状态</param>
        protected void SetSelectChk(BasicCheckBox CHB,bool Checked)
        {
            string tags = CHB.Tag.ToString();
            foreach (Control ctl in panelUC5.Controls)
            {
                if (ctl is BasicCheckBox)
                {
                    BasicCheckBox bchk = ctl as BasicCheckBox;
                    if (bchk.Tag.ToString().IndexOf(tags) > -1 && bchk.Tag.ToString() != tags)
                    {
                        bchk.Checked = Checked;
                        bchk.CheckState = Checked ? BasicCheckBox.CheckStates.Checked : BasicCheckBox.CheckStates.Unchecked;
                    }
                }
            }
        }
        #region 各种全选
        //private void basicCheckBox1_CheckedChanged(object sender, bool Checked)
        //{
        //    SetSelectChk(basicCheckBox1, Checked);
        //}

        //private void basicCheckBox9_CheckedChanged(object sender, bool Checked)
        //{
        //    SetSelectChk(basicCheckBox9, Checked);
        //}

        //private void basicCheckBox23_CheckedChanged(object sender, bool Checked)
        //{
        //    SetSelectChk(basicCheckBox23, Checked);
        //}

        //private void basicCheckBox15_CheckedChanged(object sender, bool Checked)
        //{
        //    SetSelectChk(basicCheckBox15, Checked);
        //}

        //private void basicCheckBox29_CheckedChanged(object sender, bool Checked)
        //{
        //    SetSelectChk(basicCheckBox29, Checked);
        //}

        //private void basicCheckBox41_CheckedChanged(object sender, bool Checked)
        //{
        //    SetSelectChk(basicCheckBox41, Checked);
        //}

        //private void basicCheckBox35_CheckedChanged(object sender, bool Checked)
        //{
        //    SetSelectChk(basicCheckBox35, Checked);
        //}
        #endregion

        private void onChkChange(object sender, bool Checked)
        {
            BasicCheckBox bcb = (BasicCheckBox)sender;
            string orders = bcb.Tag.ToString();//获取这个控件的Tag属性
            //huabei_beijing  huabei
            if (orders.IndexOf("_") > 0)
            {
                if (!Checked)
                {
                    foreach (Control ctl in panelUC5.Controls)
                    {
                        if (ctl is Com.Seezt.Skins.BasicCheckBox)
                        {
                            Com.Seezt.Skins.BasicCheckBox chk = ctl as Com.Seezt.Skins.BasicCheckBox;
                            if (orders.IndexOf(chk.Tag.ToString()) > -1 && chk.Tag.ToString().IndexOf("_") < 0)
                            {
                                chk.CheckedChanged -= new BasicCheckBox.CheckedChangedEventHandler(ged);
                                chk.CheckState = BasicCheckBox.CheckStates.Unchecked;
                                chk.Checked = false;
                                chk.CheckedChanged += new BasicCheckBox.CheckedChangedEventHandler(ged);
                            }
                            //chk.CheckState = BasicCheckBox.CheckStates.Checked;
                            //chk.Checked = true;
                        }
                    }
                }
            }
            else
            {
 
            }
        }
        private void ged(object sender, bool Checked)
        {
            SetSelectChk((BasicCheckBox)sender, Checked);
        }

    }
}
