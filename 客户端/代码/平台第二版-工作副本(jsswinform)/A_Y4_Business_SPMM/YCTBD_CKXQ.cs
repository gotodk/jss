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
using 客户端主程序.NewDataControl;
using System.Threading;
using 客户端主程序.Support;
using 客户端主程序.DataControl;


namespace 客户端主程序.SubForm.NewCenterForm.SUBUC.SPMM
{
    public partial class YCTBD_CKXQ : BasicForm
    {
        //淡出计时器
        System.Windows.Forms.Timer Timer_DC;

        ucYCTBDGL_B CXb;
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

        public YCTBD_CKXQ(ucYCTBDGL_B CXbtemp)
        {
    
            InitializeComponent();
            CXb = CXbtemp;
            this.TitleYS = new int[] { 0, 0, -50 };

            //图标
            Icon ic = new Icon(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase+ @"\skin\ji.ico");
            this.Icon = ic;
        }
        #region//初始页面设置、获取数据
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
        private void CXweizhongbiaoEdit_Load(object sender, EventArgs e)
        {
            //开启线程，通过合同编号和买家角色编号，获取一些数据。
            flowLayoutPanel1.Enabled = false;
            resetloadLocation(PBload);

            #region//设置上传控件显示所有按钮
            //UCshangchuan[] sc = new UCshangchuan[] { uCSC_SLVZM, uCSC_ZLBZ, uCSC_CPJC, uCSC_PGZZR, uCSC_FDDBR, uCSC_SHFW, uCSC_CPSJS, uCSC_ZZ1, uCSC_ZZ2, uCSC_ZZ3, uCSC_ZZ4, uCSC_ZZ5, uCSC_ZZ6, uCSC_ZZ7, uCSC_ZZ8, uCSC_ZZ9, uCSC_ZZ10 };
            //SC(sc);
            #endregion

            Hashtable ht = new Hashtable();
            ht["DLYX"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString();
            ht["TBDH"] = CXb.TBDH.ToString(); 
            delegateForThread dft = new delegateForThread(ShowThreadResult_GetFPXX);
            NewDataControl.YCTBD RCthd = new NewDataControl.YCTBD(ht, dft);
            Thread trd = new Thread(new ThreadStart(RCthd.CXTBDYCZZBeginRun));
            trd.IsBackground = true;
            trd.Start();
        }
        private void SC(UCshangchuan[] sc)
        { 
            foreach(UCshangchuan scz in sc)
            {
                scz.showB = new bool[] { true, true, true };
            }
        }
        //显示线程处理结果的函数(用于处理线程返回数据)
        private void ShowThreadResult_GetFPXX(Hashtable returnHT)
        {
            try
            {
                //调用异步委托
                Invoke(new delegateForThreadShow(ShowThreadResult_GetFPXX_Invoke), new Hashtable[] { returnHT });
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("委托回调错误：" + ex.ToString());
            }
        }
        //处理非线程创建的控件
        string[] zzAy;//记录异常资质
        string TSZZAy;//记录特殊资质
        private void ShowThreadResult_GetFPXX_Invoke(Hashtable returnHT)
        {
            flowLayoutPanel1.Enabled = true;
            PBload.Visible = false;
            //显示执行结果
            DataSet returnds = (DataSet)returnHT["返回值"];
           
            switch (returnds.Tables["返回值单条"].Rows[0]["执行结果"].ToString())
            {
                case "err":
                    ArrayList Almsg3 = new ArrayList();
                    Almsg3.Add("");
                    Almsg3.Add(returnds.Tables["返回值单条"].Rows[0]["提示文本"].ToString());
                    FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "其他", "提示", Almsg3);
                    DialogResult dr = FRSE3.ShowDialog();
                    break;
                case "ok":
                    if (returnds.Tables["主表"].Rows.Count > 0)
                    {
                        #region//资质
                        string zizhi = returnds.Tables["主表"].Rows[0]["YCTBZZ"].ToString();
                        string[] zizhiarr = zizhi.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                        if (zizhiarr.Length > 0)
                        {
                            zzAy = zizhiarr;
                            for (int i = 1; i < zizhiarr.Length + 1; i++)
                            {
                                if (zizhiarr[i - 1].Trim() != "")
                                {
                                    init(i, zizhiarr[i - 1].Trim());
                                    string ZLBZ=returnds.Tables["主表"].Rows[0]["ZLBZYZM"].ToString();
                                    if (zizhiarr[i - 1].Trim() == "质量标准与证明")
                                    {
                                        foreach (Control ctrl in ht_UploadControls)
                                        {
                                            if (ctrl is UCshangchuan && ctrl.Name.Contains("uCshangchuanSP"+i))
                                            {
                                                UCshangchuan u = ctrl as UCshangchuan;
                                                ListView lv = u.UpItem;
                                                lv.Items.Clear();
                                                lv.Items.Add(new ListViewItem(new string[] { "", returnds.Tables["主表"].Rows[0]["ZLBZYZM"].ToString(), "", "" }));
                                            }
                                        }                                                                                
                                    }
                                    else if (zizhiarr[i - 1].Trim() == "产品检测报告")
                                    {
                                        foreach (Control ctrl in ht_UploadControls)
                                        {
                                            if (ctrl is UCshangchuan && ctrl.Name.Contains("uCshangchuanSP" + i))
                                            {
                                                UCshangchuan u = ctrl as UCshangchuan;
                                                ListView lv = u.UpItem;
                                                lv.Items.Clear();
                                                lv.Items.Add(new ListViewItem(new string[] { "", returnds.Tables["主表"].Rows[0]["CPJCBG"].ToString(), "", "" }));
                                            }
                                        }    
                                    }
                                    else if (zizhiarr[i - 1].Trim() == "品管总负责人法律承诺书")
                                    {
                                        foreach (Control ctrl in ht_UploadControls)
                                        {
                                            if (ctrl is UCshangchuan && ctrl.Name.Contains("uCshangchuanSP" + i) )
                                            {
                                                UCshangchuan u = ctrl as UCshangchuan;
                                                ListView lv = u.UpItem;
                                                lv.Items.Clear();
                                                lv.Items.Add(new ListViewItem(new string[] { "", returnds.Tables["主表"].Rows[0]["PGZFZRFLCNS"].ToString(), "", "" }));
                                            }
                                        }
                                    }
                                    else if (zizhiarr[i - 1].Trim() == "法定代表人承诺书")
                                    {
                                        foreach (Control ctrl in ht_UploadControls)
                                        {
                                            if (ctrl is UCshangchuan && ctrl.Name.Contains("uCshangchuanSP" + i))
                                            {
                                                UCshangchuan u = ctrl as UCshangchuan;
                                                ListView lv = u.UpItem;
                                                lv.Items.Clear();
                                                lv.Items.Add(new ListViewItem(new string[] { "", returnds.Tables["主表"].Rows[0]["FDDBRCNS"].ToString(), "", "" }));
                                            }
                                        }
                                    }
                                    else if (zizhiarr[i - 1].Trim() == "售后服务规定与承诺")
                                    {
                                        foreach (Control ctrl in ht_UploadControls)
                                        {
                                            if (ctrl is UCshangchuan && ctrl.Name.Contains("uCshangchuanSP" + i))
                                            {
                                                UCshangchuan u = ctrl as UCshangchuan;
                                                ListView lv = u.UpItem;
                                                lv.Items.Clear();
                                                lv.Items.Add(new ListViewItem(new string[] { "", returnds.Tables["主表"].Rows[0]["SHFWGDYCN"].ToString(), "", "" }));
                                            }
                                        }
                                    }
                                    else if (zizhiarr[i - 1].Trim() == "产品送检授权书")
                                    {
                                        foreach (Control ctrl in ht_UploadControls)
                                        {
                                            if (ctrl is UCshangchuan && ctrl.Name.Contains("uCshangchuanSP" + i) )
                                            {
                                                UCshangchuan u = ctrl as UCshangchuan;
                                                ListView lv = u.UpItem;
                                                lv.Items.Clear();
                                                lv.Items.Add(new ListViewItem(new string[] { "", returnds.Tables["主表"].Rows[0]["CPSJSQS"].ToString(), "", "" }));
                                            }
                                        }
                                    }
                                    else if (zizhiarr[i - 1].Trim() == "税率证明")
                                    {
                                        foreach (Control ctrl in ht_UploadControls)
                                        {
                                            if (ctrl is UCshangchuan && ctrl.Name.Contains("uCshangchuanSP" + i) )
                                            {
                                                UCshangchuan u = ctrl as UCshangchuan;
                                                ListView lv = u.UpItem;
                                                lv.Items.Clear();
                                                lv.Items.Add(new ListViewItem(new string[] { "", returnds.Tables["主表"].Rows[0]["SLZM"].ToString(), "", "" }));
                                            }
                                        }
                                    }
                                    else
                                    {
                                        foreach (Control ctrl in ht_UploadControls)
                                        {
                                            if (ctrl is UCshangchuan && ctrl.Name.Contains("uCshangchuanSP" + i) )
                                            {
                                                string[] TSZZ = returnds.Tables["主表"].Rows[0]["ZZ01"].ToString().Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                                                TSZZAy = returnds.Tables["主表"].Rows[0]["ZZ01"].ToString();
                                                foreach(string TSZZArr in TSZZ)
                                                {
                                                    if (TSZZArr.Contains(zizhiarr[i - 1].Trim()))
                                                    {
                                                        string[] TSZZArrli = TSZZArr.Split('*');
                                                        string TSZZName = TSZZArrli[0].ToString();
                                                        string TSZZUrl = TSZZArrli[1].ToString();
                                                        UCshangchuan u = ctrl as UCshangchuan;
                                                        ListView lv = u.UpItem;
                                                        lv.Items.Clear();
                                                        lv.Items.Add(new ListViewItem(new string[] { "", TSZZUrl, "", "" }));
                                                    }
                                                }

                                                
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        #endregion

                        pnelYCZZ.Height = 30 * zizhiarr.Length;
                        flowLayoutPanel2.Height = 30 * zizhiarr.Length;
                        flowLayoutPanel1.Height = flowLayoutPanel2.Height + 270;
                        
                        if (returnds.Tables["主表"].Rows[0]["JYGLBSHZT"].ToString() == "未审核")
                        {
                            //lblSHR.Text = "服务中心审核";
                            lblSHTime.Text = returnds.Tables["主表"].Rows[0]["FWZXSHSJ"].ToString();
                            txtSHYJ.Text = returnds.Tables["主表"].Rows[0]["FWZXSHYJ"].ToString();
                        }
                        else if (returnds.Tables["主表"].Rows[0]["JYGLBSHZT"].ToString() == "审核未通过")
                        {
                            //lblSHR.Text = "交易管理部审核";
                            lblSHTime.Text = returnds.Tables["主表"].Rows[0]["JYGLBZHYCSHSJ"].ToString();
                            txtSHYJ.Text = returnds.Tables["主表"].Rows[0]["JYGLBSHYJ"].ToString();
                        }
                    }
                    else
                    {
                        ArrayList Almsg1 = new ArrayList();
                        Almsg1.Add("");
                        Almsg1.Add("系统繁忙！请稍后重试...");
                        FormAlertMessage FRSE1 = new FormAlertMessage("仅确定", "其他", "提示", Almsg1);
                        FRSE1.ShowDialog(); 
                    }
                    break;
                default:
                    ArrayList Almsg2 = new ArrayList();
                    Almsg2.Add("");
                    Almsg2.Add("系统繁忙！请稍后重试...");
                    FormAlertMessage FRSE = new FormAlertMessage("仅确定", "其他", "提示", Almsg2);
                    DialogResult dr1 = FRSE.ShowDialog();
                    break;
            }
        }

        /// <summary>
        /// 重设进度条位置，放到按钮旁边
        /// </summary>
        /// <param name="BB">参考提交按钮</param>
        /// <param name="PB">要移动的进度条</param>
        private void resetloadLocation(PictureBox PB)
        {
            PB.Location = new Point(flowLayoutPanel1.Location.X + panelUpdate.Location.X + btnOK.Location.X + btnOK.Width + 10, flowLayoutPanel1.Location.Y + flowLayoutPanel1.Margin.Top * 2 + panelUpdate.Location.Y + panelUpdate.Margin.Top * 2 + btnOK.Location.Y+10);
            PB.Visible = true;
        }
        #endregion
        /// <summary>
        /// 显示错误提示
        /// </summary>
        /// <param name="err"></param>
        private void showAlertY(string err)
        {
            ArrayList Almsg3 = new ArrayList();
            Almsg3.Add("");
            Almsg3.Add(err);
            FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "其他", "预订单信息提交", Almsg3);
            FRSE3.ShowDialog();
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
            labelnew.Location = new System.Drawing.Point(5, 3);
            labelnew.Name = "labelSP" + num.ToString();
            labelnew.Size = new System.Drawing.Size(175, 12);
            labelnew.AutoSize = true;
            labelnew.Text = labelnamestr + "：";
            labelnew.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            UCshangchuan uCshangchuannew = new 客户端主程序.SubForm.NewCenterForm.UCshangchuan();
            uCshangchuannew.BackColor = System.Drawing.Color.AliceBlue;
            uCshangchuannew.ForeColor = System.Drawing.Color.Black;
            uCshangchuannew.JSBH = "xx";
            uCshangchuannew.Location = new System.Drawing.Point(181, 0);
            uCshangchuannew.Name = "uCshangchuanSP" + num.ToString();
            uCshangchuannew.Size = new System.Drawing.Size(256, 20);
            
            Panel panelnew = new System.Windows.Forms.Panel();
            panelnew.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
| System.Windows.Forms.AnchorStyles.Right)));
            panelnew.Location = new System.Drawing.Point(3, 3);
            panelnew.Margin = new System.Windows.Forms.Padding(0);
            panelnew.Name = "panelSP" + num.ToString();
            panelnew.Size = new System.Drawing.Size(689, 30);


            panelnew.Controls.Add(labelnew);
            panelnew.Controls.Add(uCshangchuannew);
            ht_UploadControls.Add(uCshangchuannew);
            this.flowLayoutPanel2.Controls.Add(panelnew);
        }
        /// <summary>
        /// 递归本页面中所有的控件，如果是上传控件，则做判断。
        /// </summary>
        /// <param name="parents">父容器</param>
        private void GetAllControl(Control parents, Hashtable htQTZZ)
        {
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
                        lv.Items.Add(new ListViewItem(new string[] { "", htQTZZ["资质" + num].ToString(), "", "" }));
                    }
                }
                else if (ctrl.Controls != null && ctrl.Controls.Count > 0)
                {
                    GetAllControl(ctrl, htQTZZ);
                }

            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            ArrayList all = new ArrayList();
            all.Add("");
            all.Add("确定要修改资质吗？");
            FormAlertMessage fm1 = new FormAlertMessage("确定取消", "问号", "", all);
            if (fm1.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            #region//验证
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
            #endregion

            DataSet ds = new DataSet();
            DataTable table = new DataTable();
            #region//特殊资质表
            table.TableName = "特殊资质表";
            DataColumn column = new DataColumn("Name",System.Type.GetType("System.String"));
            table.Columns.Add(column);
            column = new DataColumn("Value", System.Type.GetType("System.String"));
            table.Columns.Add(column);
            DataRow row;
            if (TSZZAy!=null && TSZZAy != "")
            {
                string[] TSZZ = TSZZAy.ToString().Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                foreach(string tszzay in TSZZ)
                {
                    if (tszzay!="")
                    {
                        string[] zz = tszzay.Split(new string[] { "*" }, StringSplitOptions.RemoveEmptyEntries);
                        row = table.NewRow();
                        row["Name"] = zz[0].ToString();
                        row["Value"] = zz[1].ToString();
                        table.Rows.Add(row);
                    }
                }                
            }
            ds.Tables.Add(table);
            #endregion

            #region//固定资质表
            DataTable tableGDZZ = new DataTable();
            tableGDZZ.TableName = "固定资质表";
            DataColumn columnGDZZ = new DataColumn("Name", System.Type.GetType("System.String"));
            tableGDZZ.Columns.Add(columnGDZZ);
            columnGDZZ = new DataColumn("Value", System.Type.GetType("System.String"));
            tableGDZZ.Columns.Add(columnGDZZ);
            #endregion

            #region//投标单号表
            table = new DataTable();
            table.TableName = "投标单号表";
            column = new DataColumn("TBDH", System.Type.GetType("System.String"));
            table.Columns.Add(column);
            column = new DataColumn("买家角色编号", System.Type.GetType("System.String"));
            table.Columns.Add(column);

            row = table.NewRow();
            row["TBDH"] = CXb.TBDH.Trim();
            row["买家角色编号"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["买家角色编号"].ToString();
            table.Rows.Add(row);
            ds.Tables.Add(table);
            #endregion

            #region//将修改后的数据存储到ds中
            
            if (ht_UploadControls.Count>0)
            {
                if(zzAy.Length>0)
                {                    
                    for(int i=1;i<zzAy.Length+1;i++)
                    {
                        if (zzAy[i - 1].Trim() == "质量标准与证明")
                        {
                            foreach (UCshangchuan ctrl in ht_UploadControls)
                            {
                                if(ctrl.Name.Contains("uCshangchuanSP"+i))
                                {
                                    row = tableGDZZ.NewRow();
                                    row["Name"] = "ZLBZYZM";
                                    row["Value"] = ctrl.UpItem.Items[0].SubItems[1].Text;
                                    tableGDZZ.Rows.Add(row);
                                    break;
                                }
                            }
                            
                        }
                        else if (zzAy[i - 1].Trim() == "产品检测报告")
                        {
                            foreach (UCshangchuan ctrl in ht_UploadControls)
                            {
                                if (ctrl.Name.Contains("uCshangchuanSP" + i))
                                {
                                    row = tableGDZZ.NewRow();
                                    row["Name"] = "CPJCBG";
                                    row["Value"] = ctrl.UpItem.Items[0].SubItems[1].Text;
                                    tableGDZZ.Rows.Add(row);
                                    break;
                                }
                            }

                        }
                        else if (zzAy[i - 1].Trim() == "品管总负责人法律承诺书")
                        {
                            foreach (UCshangchuan ctrl in ht_UploadControls)
                            {
                                if (ctrl.Name.Contains("uCshangchuanSP" + i))
                                {
                                    row = tableGDZZ.NewRow();
                                    row["Name"] = "PGZFZRFLCNS";
                                    row["Value"] = ctrl.UpItem.Items[0].SubItems[1].Text;
                                    tableGDZZ.Rows.Add(row);
                                    break;
                                }
                            }

                        }
                        else if (zzAy[i - 1].Trim() == "法定代表人承诺书")
                        {
                            foreach (UCshangchuan ctrl in ht_UploadControls)
                            {
                                if (ctrl.Name.Contains("uCshangchuanSP" + i))
                                {
                                    row = tableGDZZ.NewRow();
                                    row["Name"] = "FDDBRCNS";
                                    row["Value"] = ctrl.UpItem.Items[0].SubItems[1].Text;
                                    tableGDZZ.Rows.Add(row);
                                    break;
                                }
                            }

                        }
                        else if (zzAy[i - 1].Trim() == "售后服务规定与承诺")
                        {
                            foreach (UCshangchuan ctrl in ht_UploadControls)
                            {
                                if (ctrl.Name.Contains("uCshangchuanSP" + i))
                                {
                                    row = tableGDZZ.NewRow();
                                    row["Name"] = "SHFWGDYCN";
                                    row["Value"] = ctrl.UpItem.Items[0].SubItems[1].Text;
                                    tableGDZZ.Rows.Add(row);
                                    break;
                                }
                            }

                        }
                        else if (zzAy[i - 1].Trim() == "产品送检授权书")
                        {
                            foreach (UCshangchuan ctrl in ht_UploadControls)
                            {
                                if (ctrl.Name.Contains("uCshangchuanSP" + i))
                                {
                                    row = tableGDZZ.NewRow();
                                    row["Name"] = "CPSJSQS";
                                    row["Value"] = ctrl.UpItem.Items[0].SubItems[1].Text;
                                    tableGDZZ.Rows.Add(row);
                                    break;
                                }
                            }

                        }
                        else if (zzAy[i - 1].Trim() == "税率证明")
                        {
                            foreach (UCshangchuan ctrl in ht_UploadControls)
                            {
                                if (ctrl.Name.Contains("uCshangchuanSP" + i))
                                {
                                    row = tableGDZZ.NewRow();
                                    row["Name"] = "SLZM";
                                    row["Value"] = ctrl.UpItem.Items[0].SubItems[1].Text;
                                    tableGDZZ.Rows.Add(row);
                                    break;
                                }
                            }

                        }
                        else
                        {
                            foreach (DataRow rows in ds.Tables["特殊资质表"].Rows)
                            {
                                if (rows["Name"].ToString().Contains(zzAy[i - 1].Trim()))
                                {
                                    foreach (UCshangchuan ctrl in ht_UploadControls)
                                    {
                                        if (ctrl.Name.Contains("uCshangchuanSP" + i))
                                        {
                                            rows.BeginEdit();
                                            rows["Value"] = ctrl.UpItem.Items[0].SubItems[1].Text;
                                            rows.EndEdit();
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                       
                    }
                    ds.Tables.Add(tableGDZZ);
                }
            }
            #endregion

            //开启线程，通过合同编号和买家角色编号，获取一些数据。
            flowLayoutPanel1.Enabled = false;
            resetloadLocation(PBload);

            delegateForThread dft = new delegateForThread(ShowThreadResult_GetUpdate);
            NewDataControl.YCTBD RCthd = new NewDataControl.YCTBD(ds, dft);
            Thread trd = new Thread(new ThreadStart(RCthd.UpdateTBDYCZZBeginRun));
            trd.IsBackground = true;
            trd.Start();
        }
        //显示线程处理结果的函数(用于处理线程返回数据)
        private void ShowThreadResult_GetUpdate(Hashtable returnHT)
        {
            try
            {
                //调用异步委托
                Invoke(new delegateForThreadShow(ShowThreadResult_GetUpdate_Invoke), new Hashtable[] { returnHT });
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("委托回调错误：" + ex.ToString());
            }
        }
        //处理非线程创建的控件
        private void ShowThreadResult_GetUpdate_Invoke(Hashtable returnHT)
        {
            flowLayoutPanel1.Enabled = true;
            PBload.Visible = false;
            //显示执行结果
            DataSet returnds = (DataSet)returnHT["返回值"];

            switch (returnds.Tables["返回值单条"].Rows[0]["执行结果"].ToString())
            {
                case "err":
                    ArrayList Almsg3 = new ArrayList();
                    Almsg3.Add("");
                    Almsg3.Add(returnds.Tables["返回值单条"].Rows[0]["提示文本"].ToString());
                    FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "其他", "提示", Almsg3);
                    DialogResult dr = FRSE3.ShowDialog();
                    break;
                case "ok":
                    ArrayList Almsg = new ArrayList();
                    Almsg.Add("");
                    Almsg.Add(returnds.Tables["返回值单条"].Rows[0]["提示文本"].ToString());
                    FormAlertMessage FRSE1 = new FormAlertMessage("仅确定", "其他", "提示", Almsg);
                    FRSE1.ShowDialog();
                    this.Close();
                    CXb.GetData(null);
                    break;
                default:
                    ArrayList Almsg2 = new ArrayList();
                    Almsg2.Add("");
                    Almsg2.Add("系统繁忙！请稍后重试...");
                    FormAlertMessage FRSE = new FormAlertMessage("仅确定", "其他", "提示", Almsg2);
                    DialogResult dr1 = FRSE.ShowDialog();
                    break;
            }
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
                if (ctrl is UCshangchuan)
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

    }
}
