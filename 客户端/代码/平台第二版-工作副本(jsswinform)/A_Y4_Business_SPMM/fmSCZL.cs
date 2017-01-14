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
using 客户端主程序.SubForm.NewCenterForm;
using 客户端主程序.Support;

namespace 客户端主程序.SubForm
{
    public partial class fmSCZL : BasicForm
    {
        //淡出计时器
        System.Windows.Forms.Timer Timer_DC;


        /// <summary>
        /// 对勾选择后的回调指针
        /// </summary>
        delegateForThread dftForParent;
        /// <summary>
        ///dym create upload controls colliention
        /// </summary>
        List<UCshangchuan> ht_UploadControls = new List<UCshangchuan>();
        bool isSubed = true;
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
        string[] zzAy;
        public fmSCZL(delegateForThread dftForParent_temp, Hashtable ht_ZZ, string zizhi)
        {
            //issq已经没有用了
            dftForParent = dftForParent_temp;
            InitializeComponent();
            //图标
            Icon ic = new Icon(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\skin\ji.ico");
            this.Icon = ic;
            //设置窗体显示在屏幕中央
            this.StartPosition = FormStartPosition.CenterScreen;
            //不在任务栏显示此信息
            this.ShowInTaskbar = false;

            //生成控件
            string[] zizhiarr = zizhi.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
            if (zizhiarr.Length > 0)
            {
                for (int i = 1; i < zizhiarr.Length + 1; i++)
                {
                    if (zizhiarr[i - 1].Trim() != "")
                    {
                        init(i, zizhiarr[i - 1]);
                    }
                }
                zzAy = zizhiarr;
            }
            #region 六种固定资质的赋值
            if (ht_ZZ["质量标准与证明"] != null && ht_ZZ["质量标准与证明"].ToString() != "")
            {
                ListView lv = uCshangchuan1.UpItem;
                lv.Items.Clear();
                lv.Items.Add(new ListViewItem(new string[] { "", ht_ZZ["质量标准与证明"] == null ? null : ht_ZZ["质量标准与证明"].ToString(), "", "" }));
            }
            if (ht_ZZ["产品检测报告"] != null && ht_ZZ["产品检测报告"].ToString() != "")
            {
                ListView lv = uCshangchuan2.UpItem;
                lv = uCshangchuan2.UpItem;
                lv.Items.Clear();
                lv.Items.Add(new ListViewItem(new string[] { "", ht_ZZ["产品检测报告"] == null ? null : ht_ZZ["产品检测报告"].ToString(), "", "" }));
            }
            if (ht_ZZ["品管总负责人法律承诺书"] != null && ht_ZZ["品管总负责人法律承诺书"].ToString() != "")
            {
                ListView lv = uCshangchuan3.UpItem;
                lv.Clear();
                lv.Items.Add(new ListViewItem(new string[] { "", ht_ZZ["品管总负责人法律承诺书"].ToString(), "", "" }));
            }
            if (ht_ZZ["法定代表人承诺书"] != null && ht_ZZ["法定代表人承诺书"].ToString() != "")
            {
                ListView lv = uCshangchuan4.UpItem;
                lv.Clear();
                lv.Items.Add(new ListViewItem(new string[] { "", ht_ZZ["法定代表人承诺书"].ToString(), "", "" }));
            }
            if (ht_ZZ["售后服务规定与承诺"] != null && ht_ZZ["售后服务规定与承诺"].ToString() != "")
            {
                ListView lv = uCshangchuan5.UpItem;
                lv.Clear();
                lv.Items.Add(new ListViewItem(new string[] { "", ht_ZZ["售后服务规定与承诺"].ToString(), "", "" }));
            }
            if (ht_ZZ["产品送检授权书"] != null && ht_ZZ["产品送检授权书"].ToString() != "")
            {
                ListView lv = uCshangchuan6.UpItem;
                lv.Clear();
                lv.Items.Add(new ListViewItem(new string[] { "", ht_ZZ["产品送检授权书"].ToString(), "", "" }));
            }
            #endregion
            //int j = 0;
            //for (int i = 0; i < this.Controls.Count; i++)
            //{
            //    if (this.Controls[i] is UCshangchuan)
            //    {
            //        j++;
            //    }
            //}
            //MessageBox.Show(j.ToString());
            //Hashtable htQTZZ = (Hashtable)ht_ZZ["其它资质"];

            if (ht_ZZ["其它资质"] != null && ht_ZZ["其它资质"].ToString() != "")
            {
                GetAllControl(this, ht_ZZ["其它资质"].ToString());
            }

        }

        private void btnQueDing_Click(object sender, EventArgs e)
        {
            //if (!(uCshangchuan1.UpItem.Items.Count > 0) || !(uCshangchuan2.UpItem.Items.Count > 0) || !(uCshangchuan3.UpItem.Items.Count > 0) || !(uCshangchuan4.UpItem.Items.Count > 0) || !(uCshangchuan5.UpItem.Items.Count > 0) || !(uCshangchuan6.UpItem.Items.Count > 0))
            //{
            //    ArrayList al = new ArrayList();
            //    al.Add("");
            //    al.Add("请上传必要的资质及证书！");
            //    FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "", al);
            //    fm.ShowDialog();
            //    return;
            //}
            ValUpload(this);
            if (!isSubed)
            {
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add("请上传必要的资质文件！");
                FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "", al);
                fm.ShowDialog();
                isSubed = true;
                return;
            }

            //回调
            Hashtable return_ht = new Hashtable();
            return_ht["质量标准与证明"] = this.uCshangchuan1.UpItem.Items.Count == 0 ? null : this.uCshangchuan1.UpItem.Items[0].SubItems[1].Text;
            return_ht["产品检测报告"] = uCshangchuan2.UpItem.Items.Count == 0 ? null : uCshangchuan2.UpItem.Items[0].SubItems[1].Text;
            return_ht["品管总负责人法律承诺书"] = uCshangchuan3.UpItem.Items.Count == 0 ? null : uCshangchuan3.UpItem.Items[0].SubItems[1].Text;
            return_ht["法定代表人承诺书"] = uCshangchuan4.UpItem.Items.Count == 0 ? null : uCshangchuan4.UpItem.Items[0].SubItems[1].Text;
            return_ht["售后服务规定与承诺"] = uCshangchuan5.UpItem.Items.Count == 0 ? null : uCshangchuan5.UpItem.Items[0].SubItems[1].Text;
            return_ht["产品送检授权书"] = uCshangchuan6.UpItem.Items.Count == 0 ? null : uCshangchuan6.UpItem.Items[0].SubItems[1].Text;
            //Hashtable ht_QTZZ = new Hashtable();
            int i = 0;

            return_ht["其它资质"] = "|";
            if (ht_UploadControls != null)
            {
                #region 2013 08 23 郭拓
                //foreach (UCshangchuan uCshangchuannew in ht_UploadControls)
                //{
                //    if (uCshangchuannew.UpItem.Items.Count > 0)
                //    {
                //        ht_QTZZ["资质" + i.ToString()] = uCshangchuannew.UpItem.Items[0].SubItems[1].Text;
                //        i++;
                //    }
                //    else
                //    {
                //        ArrayList al = new ArrayList();
                //        al.Add("");
                //        al.Add("请上传必要的资质及证书！");
                //        FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "", al);
                //        fm.ShowDialog();
                //        return;
                //    }
                //}
                #endregion

                foreach (UCshangchuan uCshangchuannre in ht_UploadControls)
                {
                    if (uCshangchuannre.UpItem.Items.Count > 0)
                    {
                        return_ht["其它资质"] += (zzAy[i] + "*" + uCshangchuannre.UpItem.Items[0].SubItems[1].Text + "|");
                        i++;
                    }
                    else
                    {
                        return_ht["其它资质"] += (zzAy[i] + "*" + " " + "|");
                        i++;
                    }
                }
            }
            else
                return_ht["其它资质"] = null;

            dftForParent(return_ht);
            //关闭弹窗
            this.Close();
        }

        /// <summary>
        /// 初始化控件
        /// </summary>
        /// <param name="num"></param>
        /// <param name="labelnamestr"></param>
        private void init(int num, string labelnamestr)
        {
            //根据字符串动态生成上传控件
            Label labelnew = new System.Windows.Forms.Label();
            labelnew.ForeColor = System.Drawing.Color.Black;
            labelnew.Location = new System.Drawing.Point(3, 5);
            labelnew.Name = "labelSP" + num.ToString();
            labelnew.Size = new System.Drawing.Size(175, 12);

            labelnew.Text = labelnamestr + "：";
            labelnew.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            UCshangchuan uCshangchuannew = new 客户端主程序.SubForm.NewCenterForm.UCshangchuan();
            uCshangchuannew.BackColor = System.Drawing.Color.AliceBlue;
            uCshangchuannew.ForeColor = System.Drawing.Color.Black;
            uCshangchuannew.JSBH = "xx";
            uCshangchuannew.Location = new System.Drawing.Point(181, 2);
            uCshangchuannew.Name = "uCshangchuanSP" + num.ToString();
            uCshangchuannew.Size = new System.Drawing.Size(256, 20);


            Panel panelnew = new System.Windows.Forms.Panel();
            panelnew.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
| System.Windows.Forms.AnchorStyles.Right)));
            panelnew.Location = new System.Drawing.Point(0, 135);
            panelnew.Margin = new System.Windows.Forms.Padding(0);
            panelnew.Name = "panelSP" + num.ToString();
            panelnew.Size = new System.Drawing.Size(522, 27);


            panelnew.Controls.Add(labelnew);
            panelnew.Controls.Add(uCshangchuannew);
            ht_UploadControls.Add(uCshangchuannew);
            this.flowLayoutPanel1.Controls.Add(panelnew);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }
        /// <summary>
        /// 递归本页面中所有的控件，如果是上传控件，则做判断。
        /// </summary>
        /// <param name="parents">父容器</param>
        private void GetAllControl(Control parents, string str)
        {
            Hashtable htQTZZ = new Hashtable();
            string[] strAry = str.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
            int i = 1;
            foreach (string strZZ in strAry)
            {
                if (strZZ.Split(new string[] { "*" }, StringSplitOptions.RemoveEmptyEntries).Length > 1)
                {
                    htQTZZ["资质" + i] = strZZ.Split(new string[] { "*" }, StringSplitOptions.RemoveEmptyEntries)[1];
                    i++;
                }
            }

            foreach (Control ctrl in parents.Controls)
            {
                if (ctrl is UCshangchuan && ctrl.Name.IndexOf("shangchuanSP") > 0)
                {
                    //uCshangchuanSP
                    string num = ctrl.Name.Replace("uCshangchuanSP", "");
                    if (htQTZZ["资质" + num] != null && htQTZZ["资质" + num].ToString() != "")
                    {
                        UCshangchuan u = ctrl as UCshangchuan;
                        ListView lv = u.UpItem;
                        lv.Items.Clear();
                        if (htQTZZ["资质" + num].ToString().Trim() != "")
                        {
                            lv.Items.Add(new ListViewItem(new string[] { "", htQTZZ["资质" + num].ToString(), "", "" }));
                        }
                    }
                }
                else if (ctrl.Controls != null && ctrl.Controls.Count > 0)
                {
                    GetAllControl(ctrl, str);
                }

            }
        }
        /// <summary>
        /// 下载各种承诺书和送检委托书模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkXZCNSHSJWTS_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[2] + "/pingtaiservices/JHJX_File/承诺书及产品送检委托书模板.zip";
            StringOP.OpenUrl(url);
        }

        /// <summary>
        /// 迭代验证
        /// </summary>
        /// <param name="parents"></param>
        /// <param name="htQTZZ"></param>
        /// <returns></returns>
        private void ValUpload(Control parents)
        {
            foreach (Control ctrl in parents.Controls)
            {
                if (ctrl is UCshangchuan && ctrl.Name.IndexOf("uCshangchuanSP") < 0)
                {
                    UCshangchuan ctrlUp = ctrl as UCshangchuan;
                    if (!(ctrlUp.UpItem.Items.Count > 0))
                    {
                        isSubed = false;
                        return;
                    }
                }
                else
                {
                    if (ctrl.Controls != null && ctrl.Controls.Count > 0)
                        ValUpload(ctrl);
                }
            }

        }

        #region 支持鼠标控制滚动条
        private void fmSCZL_Load(object sender, EventArgs e)
        {
            this.MouseWheel += FormSample_MouseWheel;
        }
          /// <summary>
         /// 滚动方法
         /// </summary>
          /// <param name="sender"></param>
          /// <param name="e"></param>
          void FormSample_MouseWheel(object sender, MouseEventArgs e)
          {
              //获取光标位置
              Point mousePoint = new Point(e.X, e.Y);
             //换算成相对本窗体的位置
             mousePoint.Offset(this.Location.X, this.Location.Y);
             //判断是否在panel内
             if (flowLayoutPanel1.RectangleToScreen(flowLayoutPanel1.DisplayRectangle).Contains(mousePoint))
             {
                 //滚动
                 flowLayoutPanel1.AutoScrollPosition = new Point(0, flowLayoutPanel1.VerticalScroll.Value - e.Delta);
             }
          }
        #endregion
    }
}
