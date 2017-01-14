using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Com.Seezt.Skins;
using System.Collections;
using 客户端主程序.SubForm.CenterForm;
using 客户端主程序.DataControl;
using System.Reflection;
using System.Threading;
using 客户端主程序.SubForm;
using 客户端主程序.Support;
using 客户端主程序.NewDataControl;



namespace 客户端主程序.SubForm.NewCenterForm.SUBUC.SPMM
{
    public partial class FormQPDetailcheck : BasicForm
    {
       //卖家用户名，买家用户名，登陆邮箱，中标定标信息表主键，角色编号，证明上传方邮箱，是否已确认，卖方邮箱，买方邮箱，对方邮箱，对方角色类型
        string sellerName = "",buyerName="",dlyx="",number="",JSNM = "", zmscfyx="",sfqr="",sellerzh="",buyerzh="",dfyx="",dfjslx, currentName="";

        Panel p, q;
        
        //淡出计时器        
        System.Windows.Forms.Timer Timer_DC;
        Hashtable ht_where = null;
       
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


        public FormQPDetailcheck(Hashtable ht)
        {

            //=======================所有窗体都有这个玩意=========
            Init_one_show();
            //====================================================

            InitializeComponent();
            ht_where = ht;
            //设置最小化大小
            this.MaximizedBounds = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea;
            this.BackColor = Color.Black;

            Icon ic = new Icon(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\skin\ji.ico");
            this.Icon = ic;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ShowInTaskbar = false;

            SRT_demo_Run(ht_where);
            
        }
        //初始页面赋值
        protected void InitialData(DataRow dr)
        {

            lbhtbh.Text = dr["电子购货合同编号"].ToString().Trim();
            lbhtjsrq.Text = dr["合同结束日期"].ToString().Trim();
            lbspbh.Text = dr["商品编号"].ToString().Trim();
            lbspmc.Text = dr["商品名称"].ToString().Trim();
            lbgg.Text = dr["规格"].ToString().Trim();
            lbsl.Text = dr["合同数量"].ToString().Trim();
            lbhtje.Text = dr["合同金额"].ToString().Trim();
            lbdj.Text = dr["单价"].ToString().Trim();
            lblybzj.Text = dr["履约保证金金额"].ToString().Trim();
            lbzysl.Text = dr["争议数量"].ToString().Trim();
            lbzyje.Text = dr["争议金额"].ToString().Trim();
            lbqplx.Text = dr["清盘类型"].ToString().Trim();
            lbqpyy.Text = dr["清盘原因"].ToString().Trim();
            lbhkdjje.Text = dr["争议金额"].ToString().Trim();
            lbqpzt.Text = dr["清盘状态"].ToString().Trim();

            lbqpkssj.Text = dr["清盘开始时间"] == null ? "" : dr["清盘开始时间"].ToString().Trim();
            lbqpjssj.Text = dr["清盘结束时间"] == null ? "" : dr["清盘结束时间"].ToString().Trim();
            number = dr["主键"].ToString().Trim();
            zmscfyx = dr["证明上传方邮箱"] == null ? "" : dr["证明上传方邮箱"].ToString().Trim();
            sfqr = dr["是否已确认"] == null ? "" : dr["是否已确认"].ToString().Trim();
            sellerzh = dr["卖方邮箱"].ToString().Trim();
            buyerzh = dr["买方邮箱"].ToString().Trim();
            dfyx = dlyx == sellerzh ? buyerzh : sellerzh;
            dfjslx = dlyx == sellerzh ? "买家" : "卖家";

            if (dr["处理依据"] != null && dr["处理依据"].ToString().Trim() == "双方协议")
            {
                radxy.Checked = true;
            }
            else  if(dr["处理依据"] != null && dr["处理依据"].ToString().Trim() == "法律文书")
            {
                radws.Checked = true;
            }

            ucTxtpicture.Text=dr["证明文件路径"]==null?"":dr["证明文件路径"].ToString().Trim();
            if (dr["证明文件路径"] == null || dr["证明文件路径"].ToString().Trim() == "")
            {
                //B_SCCK.Visible = false;
                B_SCCK.Text = "无";
                this.B_SCCK.Click -= new System.EventHandler(this.B_SCCK_Click);
               //  B_SCCK_Click-=
            }
            //向ComboBox动态添加项
            ddlzhfk.Items.Add(new ComboBoxItem<int, string>(0, "请选择"));
            ddlzhfk.Items.Add(new ComboBoxItem<int, string>(1, sellerzh));
            ddlzhfk.Items.Add(new ComboBoxItem<int, string>(2, buyerzh));     
            ddlzhfk.SelectedIndex = 0;

            ddlzhsk.Items.Add(new ComboBoxItem<int, string>(0, "请选择"));
            ddlzhsk.Items.Add(new ComboBoxItem<int, string>(1, sellerzh));
            ddlzhsk.Items.Add(new ComboBoxItem<int, string>(2, buyerzh));
            ddlzhsk.SelectedIndex = 0;

            if (dr["转付来源账户"] != null && dr["转付来源账户"].ToString().Trim()==sellerzh)
            {
                ddlzhfk.SelectedIndex = 1;
               
            }
            else if (dr["转付来源账户"] != null && dr["转付来源账户"].ToString().Trim() == buyerzh)
            {
                ddlzhfk.SelectedIndex =2;
            }

            txtje.Text = dr["转付金额"] == null ? "" : dr["转付金额"].ToString().Trim();

            #region//zhouli 作废 2014.07.11--变更新框架，开启线程访问后台数据
            //DataSet dsseller=null, dsbuyer=null;
            
            //try
            //{
            //    NewDataControl.WebServicesCenter2013 WSC2013 = new NewDataControl.WebServicesCenter2013();
            //    //InPutHT可以选择灵活使用               
            //    dsseller = WSC2013.RunAtServices("GetNewUserState", new object[] { sellerzh });
            //    dsbuyer = WSC2013.RunAtServices("GetNewUserState", new object[] { buyerzh });
            //    string buyerjs="", sellerjs = "";
            //     if (dsbuyer != null && dsbuyer.Tables[0].Rows.Count > 0)
            //     {
            //         buyerName = dsbuyer.Tables[0].Rows[0]["用户名"].ToString();
            //         buyerjs = dsbuyer.Tables[0].Rows[0]["买家角色编号"].ToString();
            //     }
            //     if (dsseller != null && dsseller.Tables[0].Rows.Count > 0)
            //     {
            //         sellerName = dsseller.Tables[0].Rows[0]["用户名"].ToString();
            //         sellerjs = dsseller.Tables[0].Rows[0]["买家角色编号"].ToString();
            //     }
            //     currentName = dlyx == buyerzh ? buyerName : sellerName;
            //     JSNM = dlyx == buyerzh ? buyerjs : sellerjs;

            //     //lbcljg.Text = sellerName + "卖家与" + buyerName + "买家双方就" + lbhtbh.Text + "《电子购货合同》存在的所有争议已达成最终处理结果，现由" + currentName + "上传证明文件。";
            //     //lbqrjg.Text = sellerName+"卖家与"+buyerName+"买家双方就" + lbhtbh.Text + "《电子购货合同》存在的所有争议已达成最终处理结果，现对" + currentName + "上传的证明文件确认是真实的，证明文件中所表述的双方处理结果，是双方真实的意思表示。";

            //     dsseller.Dispose();
            //     dsbuyer.Dispose();

            //}
            //catch
            //{
            //    ArrayList AlmsgMR = new ArrayList();
            //    AlmsgMR.Add("");
            //    AlmsgMR.Add("请检查网络，稍后再试！");
            //    FormAlertMessage FRSEMR = new FormAlertMessage("仅确定", "其他", "提醒", AlmsgMR);
            //    FRSEMR.ShowDialog();
            //    this.Close();
            //}   
            #endregion
            //已移植可替换(已替换)

            string buyerjs = "", sellerjs = "";
            buyerName = dr["买方用户名"].ToString();
            buyerjs = dr["买方买家角色编号"].ToString();
            sellerName = dr["卖方用户名"].ToString();
            sellerjs = dr["卖方买家角色编号"].ToString();
            #region//zhouli 作废 2017.07.31 需求要求
            //currentName = dlyx == buyerzh ? buyerName : sellerName;
            #endregion
            #region//zhouli 变更 2014.07.31 需求要求
            if (zmscfyx != "")
            {
                currentName = zmscfyx;
            }
            else
            {
                currentName = "- -";
            }
            #endregion
            JSNM = dlyx == buyerzh ? buyerjs : sellerjs;

        }
        
        /// <summary>
        /// 自定义方便向ComboBox添加项的泛型结构
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        public struct ComboBoxItem<TKey, TValue>
        {
            private TKey key;
            private TValue value;

            public ComboBoxItem(TKey key, TValue value)
            {
                this.key = key;
                this.value = value;
            }

            public TKey Key
            {
                get { return key; }
            }

            public TValue Value
            {
                get { return value; }
            }

            public override string ToString()
            {
                return Value.ToString();
            }
        }

        //页面加载时
        private void FormQPDetail_Load(object sender, EventArgs e)
        {            
            //panXXDZ.Location = new Point(panXXDZ.Location.X, lbcljg.Location.Y + lbcljg.Height - 4);
            //SRT_demo_Run(ht_where);
            //如果双方已经确认，禁用控件，提交、确认按钮改为已确认。
            //获取详细信息

           
        }    



        #region 上传查看删除图片的相关操作

        //删除服务器文件方法，备用
        protected void DelFile()
        {
            if (ucTxtpicture.Text.Trim() != "")
            {
                try
                {
                    NewDataControl.WebServicesCenter2013 WSC2013 = new NewDataControl.WebServicesCenter2013();
                    //InPutHT可以选择灵活使用
                    object[] cs = { ucTxtpicture.Text.Trim().Replace(@"\", "/") };
                    DataSet dsreturn = WSC2013.RunAtServices("DelPicture", cs);

                    //填充传入参数哈希表                   
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
                catch
                {
                    ArrayList AlmsgMR = new ArrayList();
                    AlmsgMR.Add("");
                    AlmsgMR.Add("请检查网络，稍后再试！");
                    FormAlertMessage FRSEMR = new FormAlertMessage("仅确定", "其他", "提醒", AlmsgMR);
                    FRSEMR.ShowDialog();
                }
            }
 
        }

        //上传
        private void B_SC_Click(object sender, EventArgs e)
        {
            //DelFile();
            //ucTxtpicture.Text = "";
            //listView1.Items.Clear();
            //开启上传
            ((Control)sender).Focus();
            openFileDialog1.ShowDialog();
        }
        //查看
        ImageShow IS;
        private void B_SCCK_Click(object sender, EventArgs e)
        {
            ((Control)sender).Focus();
            //有地址
            if (ucTxtpicture.Text.Trim() != "")
            {
                //string url = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[2] + "/JHJXPT/SaveDir/" + lv.Items[0].SubItems[1].Text.Trim().Replace(@"\", "/");
                string url = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[2] + "/JHJXPT/SaveDir/" + ucTxtpicture.Text.Trim().Replace(@"\", "/");
               
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
                //FormWebBrowser fwb = new FormWebBrowser();
                //fwb.webBrowser1.Url = new Uri(url);

                //fwb.ShowDialog();
            }
        }
        //删除
        private void B_SCSC_Click(object sender, EventArgs e)
        {
            ((Control)sender).Focus();
            ArrayList Almsgxx = new ArrayList();
            Almsgxx.Add("");
            Almsgxx.Add("您确定要删除已经上传的扫描件吗？");
            FormAlertMessage FRSExx = new FormAlertMessage("确定取消", "问号", "删除上传文件", Almsgxx);
            DialogResult dr = FRSExx.ShowDialog();
            if (dr == DialogResult.Yes)
            {
                ////删除已经无效的图片
                //DelFile();
                ucTxtpicture.Text = "";
                listView1.Items.Clear();

            }
        }

        private void timer_SCCK_Tick(object sender, EventArgs e)
        {
            //if (listView1.Items.Count > 0 || ucTxtpicture.Text.Trim() != "")
            //{
            //    B_SCCK.Visible = true;

            //    if (sfqr == "是" || zmscfyx != dlyx)
            //    { B_SCSC.Visible = false; }
            //    else
            //    {
            //        B_SCSC.Visible = true;
            //    }

            //}
            //else
            //{
            //    B_SCCK.Visible = false;
            //    B_SCSC.Visible = false;
            //}
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            bool ISOK = true;
            string[] fName = openFileDialog1.FileNames;
            //若选择选择对话框允许选择多个，则还要检验不能超过5个
            //多不是对话框选择的，是直接指定数组，则还要检验不能重名

            foreach (var file in fName)
            {
                System.IO.FileInfo fi = new System.IO.FileInfo(file);
                if (fi.Length > 500 * 1024)
                {
                    ArrayList Almsg4 = new ArrayList();
                    Almsg4.Add("");
                    Almsg4.Add("文件大小不可超过500k！");// 这里是提交失败的提示，可能是程序出错，可能是条件不足等
                    FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "其他", "提示", Almsg4);
                    FRSE4.ShowDialog();

                    ISOK = false;
                    break;
                }
            }
            if (ISOK)
            {
                //开始上传,传入文件名数组，传入对应的上传结果控件，传入回调，传入角色编号
                FormSC FSC = new FormSC(fName, listView1, new delegateForSC(UpLoadSucceed), JSNM);
                FSC.ShowDialog();

            }
        }

        /// <summary>
        /// 若全部上传完成，在主窗体进行数据处理，根据情况编写，没有处理也要带着这个方法
        /// </summary>
        /// <param name="LV">上传结果集合</param>
        private void UpLoadSucceed(ListView LV)
        {
            //这里的LV,实际上是打开上传时指定的那个隐藏控件listView1，所以这个方法在多个上传按钮时，只需要写一次就行
            //MessageBox.Show("回调测试" + LV.Name);
            this.ucTxtpicture.Text = LV.Items[LV.Items.Count - 1].SubItems[1].Text;
        }
        #endregion      

        
        //变换下拉框的值
        private void ddlzhsk_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlzhsk.SelectedIndex == 1)
            {
                ddlzhfk.SelectedIndex = 2;
            }
            else if (ddlzhsk.SelectedIndex == 2)
            {
                ddlzhfk.SelectedIndex =1;
            }
        }

        private void ddlzhfk_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlzhfk.SelectedIndex == 1)
            {
                ddlzhsk.SelectedIndex = 2;
            }
            else if (ddlzhfk.SelectedIndex == 2)
            {
                ddlzhsk.SelectedIndex = 1;
            }
        }
        //提交证明文件
        private void btntj_Click(object sender, EventArgs e)
         {
            if( radxy.Checked==false && radws.Checked==false)
            {
                ArrayList Almsg3 = new ArrayList();
                Almsg3.Add("");
                Almsg3.Add("请选择处理依据！");
                FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "其他", "", Almsg3);
                FRSE3.ShowDialog(); 
            }            
            else  if (ddlzhfk.SelectedIndex == 0)
             {
                 ArrayList Almsg3 = new ArrayList();
                 Almsg3.Add("");
                 Almsg3.Add("请选择付款方的登陆账号！");
                 FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "其他", "", Almsg3);
                 FRSE3.ShowDialog(); 
             }
             else if (ddlzhsk.SelectedIndex == 0)
             {
                 ArrayList Almsg3 = new ArrayList();
                 Almsg3.Add("");
                 Almsg3.Add("请选择收款方的登陆账号！");
                 FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "其他", "", Almsg3);
                 FRSE3.ShowDialog();
             }
             else if(txtje.Text.Trim()=="")
             {
                 ArrayList Almsg3 = new ArrayList();
                 Almsg3.Add("");
                 Almsg3.Add("请填写转付金额！");
                 FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "其他", "", Almsg3);
                 FRSE3.ShowDialog(); 
             }
             else if (ucTxtpicture.Text.Trim() == "")
             {
                 ArrayList Almsg3 = new ArrayList();
                 Almsg3.Add("");
                 Almsg3.Add("请上传证明文件！");
                 FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "其他", "", Almsg3);
                 FRSE3.ShowDialog();
             }          

             else
             {
                 //首先验证并发性，确保对方未在修改时进行了确认
                 #region//zhouli 作废 2014.07.11--变更新框架，开启线程访问后台数据
                 //bool passed = true;
                 //try
                 //{
                 //    NewDataControl.WebServicesCenter2013 WSC2013 = new NewDataControl.WebServicesCenter2013();
                 //    //InPutHT可以选择灵活使用               
                 //    DataSet dy = WSC2013.RunAtServices("GetSHWJinfo", new object[] { number });
                 //    if (dy != null && dy.Tables[0].Rows.Count > 0)
                 //    {
                 //        if (dy.Tables[0].Rows[0]["是否已确认"] != null)
                 //        {
                 //            if (dy.Tables[0].Rows[0]["是否已确认"].ToString().Trim() == "是")
                 //            {
                 //                passed = false;
                 //                ArrayList Almsg3 = new ArrayList();
                 //                Almsg3.Add("");
                 //                Almsg3.Add("对方在"+dy.Tables[0].Rows[0]["确认时间"].ToString().Trim()+"进行过确认，无法重新提交！");
                 //                FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "其他", "", Almsg3);
                 //                FRSE3.ShowDialog();
                 //                if (!btntj.Texts.Contains("已"))
                 //                { btntj.Texts = btntj.Texts.Replace("确认", "已确认"); }
                 //                panelUC1.Enabled = false;
 
                 //            }
                 //        }
                         
 
                 //    }
                 //}
                 //catch
                 //{
                 //    ArrayList AlmsgMR = new ArrayList();
                 //    AlmsgMR.Add("");
                 //    AlmsgMR.Add("请检查网络，稍后再试！");
                 //    FormAlertMessage FRSEMR = new FormAlertMessage("仅确定", "其他", "提醒", AlmsgMR);
                 //    FRSEMR.ShowDialog(); 
                 //}

                 //if (passed)
                 //{
                 //    ArrayList Almsg3 = new ArrayList();
                 //    Almsg3.Add("");
                 //    Almsg3.Add("确定进行此项操作吗？");
                 //    FormAlertMessage FRSE3 = new FormAlertMessage("确定取消", "问号", "", Almsg3);

                 //    DialogResult boxresult = FRSE3.ShowDialog();
                 //    if (boxresult == DialogResult.Yes)
                 //    {
                 //        PBload.Visible = true;
                 //        PBload.Location = new Point(btntj.Location.X + btntj.Width + 10, btntj.Location.Y - 5);
                 //        flowPanel.Enabled = false;

                 //        Hashtable htab = new Hashtable();
                 //        htab["wjlj"] = ucTxtpicture.Text.Trim();
                 //        htab["lyzh"] = ddlzhfk.SelectedItem.ToString();
                 //        htab["mbzh"] = ddlzhsk.SelectedItem.ToString();
                 //        htab["zfje"] = txtje.Text.Trim();
                 //        htab["dlyx"] = dlyx;
                 //        htab["num"] = number;
                 //        htab["clyj"] = radxy.Checked ? "文件协议" : "法律文书";
                 //        htab["dfyx"] = dfyx;
                 //        htab["dfjslx"] = dfjslx;
                 //        htab["htbh"] = lbhtbh.Text.Trim();
                 //        htab["spmc"] = lbspmc.Text.Trim();
                 //        SRT_demo_Run(htab);
                 //    }

                 //}
                 #endregion
                 //已移植可替换(已替换)
                 Hashtable ht =new Hashtable();
                 ht["number"]=number;
                 SRT_GetSHWJinfo_Run(ht);
             }            
           
        }

        #region//zhouli 2014.07.11 add
        //开启一个测试线程
        private void SRT_GetSHWJinfo_Run(Hashtable InPutHT)
        {

            RunThreadClasshwqs OTD = new RunThreadClasshwqs(InPutHT, new delegateForThread(SRT_GetSHWJinfo));
            Thread trd = new Thread(new ThreadStart(OTD.DBZBXXinfoget));

            trd.IsBackground = true;
            trd.Start();
        }
        //显示线程处理结果的函数(用于处理线程返回数据),调用异步委托
        private void SRT_GetSHWJinfo(Hashtable OutPutHT)
        {
            try { Invoke(new delegateForThreadShow(SRT_GetSHWJinfo_Invoke), new Hashtable[] { OutPutHT }); }
            catch (Exception ex) { Support.StringOP.WriteLog("委托回调错误：" + ex.ToString()); }
        }
        //处理非线程创建的控件
        private void SRT_GetSHWJinfo_Invoke(Hashtable OutPutHT)
        {
            //重新开放提交区域,并滚动条强制置顶
            flowPanel.Enabled = true;
            PBload.Visible = false;
            // UPUP();


            //返回值后的处理
            DataSet dy = (DataSet)OutPutHT["返回值"];
            bool passed = true;
            if (dy != null && dy.Tables[0].Rows.Count > 0)
            {
                if (dy.Tables[0].Rows[0]["是否已确认"] != null)
                {
                    if (dy.Tables[0].Rows[0]["是否已确认"].ToString().Trim() == "是")
                    {
                        passed = false;
                        ArrayList Almsg3 = new ArrayList();
                        Almsg3.Add("");
                        Almsg3.Add("对方在" + dy.Tables[0].Rows[0]["确认时间"].ToString().Trim() + "进行过确认，无法重新提交！");
                        FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "其他", "", Almsg3);
                        FRSE3.ShowDialog();
                        if (!btntj.Texts.Contains("已"))
                        { btntj.Texts = btntj.Texts.Replace("确认", "已确认"); }
                        panelUC1.Enabled = false;

                    }
                }


            }
            if (passed)
            {
                ArrayList Almsg3 = new ArrayList();
                Almsg3.Add("");
                Almsg3.Add("确定进行此项操作吗？");
                FormAlertMessage FRSE3 = new FormAlertMessage("确定取消", "问号", "", Almsg3);

                DialogResult boxresult = FRSE3.ShowDialog();
                if (boxresult == DialogResult.Yes)
                {
                    PBload.Visible = true;
                    PBload.Location = new Point(btntj.Location.X + btntj.Width + 10, btntj.Location.Y - 5);
                    flowPanel.Enabled = false;

                    Hashtable htab = new Hashtable();
                    htab["wjlj"] = ucTxtpicture.Text.Trim();
                    htab["lyzh"] = ddlzhfk.SelectedItem.ToString();
                    htab["mbzh"] = ddlzhsk.SelectedItem.ToString();
                    htab["zfje"] = txtje.Text.Trim();
                    htab["dlyx"] = dlyx;
                    htab["num"] = number;
                    htab["clyj"] = radxy.Checked ? "文件协议" : "法律文书";
                    htab["dfyx"] = dfyx;
                    htab["dfjslx"] = dfjslx;
                    htab["htbh"] = lbhtbh.Text.Trim();
                    htab["spmc"] = lbspmc.Text.Trim();
                    SRT_demo_Run(htab);
                }

            }
        }

        #endregion

        //证明上传后对方确认
        private void btnqr_Click(object sender, EventArgs e)
        {
            ArrayList Almsg3 = new ArrayList();
            Almsg3.Add("");
            Almsg3.Add("确定进行此项操作吗？");
            FormAlertMessage FRSE3 = new FormAlertMessage("确定取消", "问号", "", Almsg3);          

            DialogResult boxresult = FRSE3.ShowDialog();
            if (boxresult == DialogResult.Yes)
            {
                PBload.Visible = true;
                PBload.Location = new Point(btnqr.Location.X + btnqr.Width + 10, btnqr.Location.Y - 5);
                flowPanel.Enabled = false;

                Hashtable htab = new Hashtable();
                htab["dlyx"] = dlyx;
                htab["num"] = number;
                htab["dfyx"] = dfyx;
                htab["dfjslx"] = dfjslx;
                htab["htbh"] = lbhtbh.Text.Trim();
                htab["spmc"] = lbspmc.Text.Trim();

                OpenThreadDataUpdate OTD = new OpenThreadDataUpdate(htab, new delegateForThread(SRT_demo), "DWConfirm");
                Thread trd = new Thread(new ThreadStart(OTD.BeginRun));

                trd.IsBackground = true;
                trd.Start(); 
            }           
             
        }

        //开启一个测试线程
        private void SRT_demo_Run(Hashtable InPutHT)
        {

            RunThreadClasshwqs OTD = new RunThreadClasshwqs(InPutHT, new delegateForThread(SRT_demo));
            Thread trd = new Thread(new ThreadStart(OTD.DBZBXXinfoget));

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
            flowPanel.Enabled = true;
            PBload.Visible = false;
           // UPUP();


            //返回值后的处理
            DataSet dsreturn = (DataSet)OutPutHT["返回值"];

            string zt = dsreturn.Tables["返回值单条"].Rows[0]["执行结果"].ToString();
            string showstr = dsreturn.Tables["返回值单条"].Rows[0]["提示文本"].ToString();
            DataTable dt = dsreturn.Tables["HTPXX"];
            //显示执行结果
            switch (zt)
            {
                case "ok":
                    //给出表单提交成功的提示
                    InitialData(dt.Rows[0]);

                    //动态绘制两个panel
                    p = new Panel();
                    p.Name = "p1";
                    p.Font= new Font("宋体", 9.0F);
                    p.Parent=panSSQY;
                    p.Location = new Point(2, 52);
                    p.Paint += panelcl_Paint;
                    p.Size = new Size(790,50);

                    q = new Panel();
                    q.Name = "q1";
                    q.Font = new Font("宋体", 9.0F);
                    q.Parent=panSSQY;
                    q.Location = new Point(2, 184);
                    q.Paint += panelqr_Paint;
                    q.Size = new Size(790,50);

                    
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
            this.Show();
        }

        private void panelcl_Paint(object sender, PaintEventArgs e)
        {
            
            string str1 = "卖方与", str2 = "买方双方就", str3 = "《电子购货合同》存在的所有争议已达成最终处理结果，现由", str4 = "上传证明文件。";

            Point startPoint = new Point(10, 10);

            // Set TextFormatFlags to no padding so strings are drawn together.
            TextFormatFlags flags = TextFormatFlags.NoPadding;

            // Declare a proposed size with dimensions set to the maximum integer value.
            Size proposedSize = new Size(int.MaxValue, int.MaxValue);

            Font myfont = new Font("宋体", 9.0F);
            Font ft = p.Font;

            float h = p.Height;

            Drawing(sellerName, p.Width - 20, myfont, e.Graphics, proposedSize, flags, ref startPoint, Color.Red, out h);
            Drawing(str1, p.Width - 20, ft, e.Graphics, proposedSize, flags, ref startPoint, Color.Black, out h);
            Drawing(buyerName, p.Width - 20, myfont, e.Graphics, proposedSize, flags, ref startPoint, Color.Red, out h);
            Drawing(str2, p.Width - 20, ft, e.Graphics, proposedSize, flags, ref startPoint, Color.Black, out h);
            Drawing(lbhtbh.Text, p.Width - 20, myfont, e.Graphics, proposedSize, flags, ref startPoint, Color.Red, out h);
            Drawing(str3, p.Width - 20, ft, e.Graphics, proposedSize, flags, ref startPoint, Color.Black, out h);
            Drawing(currentName, p.Width - 20, myfont, e.Graphics, proposedSize, flags, ref startPoint, Color.Red, out h);
            Drawing(str4, p.Width - 20, ft, e.Graphics, proposedSize, flags, ref startPoint, Color.Black, out h);
            p.Height = (int)h;
        }

        private void panelqr_Paint(object sender, PaintEventArgs e)
        {
           
            string str1 = "卖方与", str2 = "买方双方就", str3 = "《电子购货合同》存在的所有争议已达成最终处理结果，现对", str4 = "上传的证明文件确认是真实的，证明文件中所表述的双方处理结果，是双方真实的意思表示。";

            Point startPoint = new Point(10, 10);

            // Set TextFormatFlags to no padding so strings are drawn together.
            TextFormatFlags flags = TextFormatFlags.NoPadding;

            // Declare a proposed size with dimensions set to the maximum integer value.
            Size proposedSize = new Size(int.MaxValue, int.MaxValue);

            Font myfont = new Font("宋体", 9.0F);
            Font ft = q.Font;

            float h = q.Height;

            Drawing(sellerName, q.Width - 20, myfont, e.Graphics, proposedSize, flags, ref startPoint, Color.Red, out h);
            Drawing(str1, q.Width - 20, ft, e.Graphics, proposedSize, flags, ref startPoint, Color.Black, out h);
            Drawing(buyerName, q.Width - 20, myfont, e.Graphics, proposedSize, flags, ref startPoint, Color.Red, out h);
            Drawing(str2, q.Width - 20, ft, e.Graphics, proposedSize, flags, ref startPoint, Color.Black, out h);
            Drawing(lbhtbh.Text, q.Width - 20, myfont, e.Graphics, proposedSize, flags, ref startPoint, Color.Red, out h);
            Drawing(str3, q.Width - 20, ft, e.Graphics, proposedSize, flags, ref startPoint, Color.Black, out h);
            Drawing(currentName, q.Width - 20, myfont, e.Graphics, proposedSize, flags, ref startPoint, Color.Red, out h);
            Drawing(str4, q.Width - 20, ft, e.Graphics, proposedSize, flags, ref startPoint, Color.Black, out h);
            q.Height = (int)h;
        }

        /// <summary>
        /// 用不同的颜色绘制字符串
        /// </summary>
        /// <param name="str">要绘制的字符串</param>
        /// <param name="w">panel宽度</param>
        /// <param name="font">字体</param>
        /// <param name="g">绘图图面</param>
        /// <param name="proposedSize">最大值</param>
        /// <param name="flags">关于字符串的显示和布局信息</param>
        /// <param name="startPoint">绘制的起始点</param>
        /// <param name="color">颜色</param>
        /// <param name="height">自动计算的面板宽度</param>
        protected void Drawing(string str, float w, Font font, Graphics g, Size proposedSize, TextFormatFlags flags, ref Point startPoint, Color color, out float height)
        {
            float lineheight = TextRenderer.MeasureText(g, "测试", font, proposedSize, flags).Height;
            for (int i = 0; i < str.Length; i++)
            {
                Size size = TextRenderer.MeasureText(g, str[i].ToString(), font, proposedSize, flags);

                if (startPoint.X < w)
                {
                    TextRenderer.DrawText(g, str[i].ToString(), font, startPoint, color, flags);
                    startPoint.X += size.Width;
                }
                else
                {
                    startPoint.X = 10; startPoint.Y = startPoint.Y + size.Height + 10;
                    TextRenderer.DrawText(g, str[i].ToString(), font,
                    startPoint, color, flags);
                    startPoint.X += size.Width;
                }
            }
            height = startPoint.Y + lineheight + 15;
            //g.Dispose();

        }    

    }
}
