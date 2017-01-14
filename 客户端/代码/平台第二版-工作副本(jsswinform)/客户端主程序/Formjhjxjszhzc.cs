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
using 客户端主程序.DataControl;
using 客户端主程序.SubForm;
using 客户端主程序.Support;
using System.Text.RegularExpressions;

namespace 客户端主程序
{
    public partial class Formjhjxjszhzc : BasicForm
    {
        /// <summary>
        /// 主显示窗体
        /// </summary>
        FormMainPublic FMP;

        Hashtable HTuser;
        int Formh = 808;
        int Pan_log = 775;
        int SaveH = 687;
        int sfzH = 334;
        int qyinfoH = 405;
        int quziK = 651;
        //淡出计时器
        System.Windows.Forms.Timer Timer_DC;

        /// <summary>
        /// 是否首次打开
        /// </summary>
        string Isreopen;

        #region 窗体淡出，窗体最小化最大化退出等特殊控制，所有窗体都有这个玩意
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
            //MaxDC();

            //tableLayoutPanel1.
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

        /*
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
        */
        #endregion


        public Formjhjxjszhzc( Hashtable htuser)  //FormMainPublic FMPtemp,string isreopen
        {
            InitializeComponent();

            //设置任务栏图标
            Icon ic = new Icon(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase+ @"\skin\ji.ico");
            this.Icon = ic;


            HTuser = htuser;
            //设定详细信息的默认显示信息
            ucTextBox_xxdz.Text = "请不要重复输入省市区信息"; 
            //// 设置文本框字体颜色为灰色         
           ucTextBox_xxdz.ForeColor = Color.Gray;         
            //// 设置文本框字体为斜体          
            //ucTextBox_xxdz.Font = new Font(this.Font, FontStyle.Italic);   
            // 恢复文本框字体颜色黑色          
           // ucTextBox_xxdz.ForeColor = Color.Black;
            // 恢复文本框字体为标准       
       
 ucTextBox_xxdz.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            // 定义文本框click事件          
            this.ucTextBox_xxdz.Click += new System.EventHandler(this.ucTextBox_xxdz_Click);
            //.EventHandler();

            //设定联系人姓名相关提示信息

            ucTextBox_lxrxm.Text = "请输入真实联系人姓名";
           ucTextBox_lxrxm.ForeColor = Color.Gray;
            //ucTextBox_lxrxm.Font = new Font(this.Font, FontStyle.Italic);
            // 恢复文本框字体颜色黑色          
           //ucTextBox_lxrxm.ForeColor = Color.Black;
            // 恢复文本框字体为标准       
            // ucTextBox_lxrxm.Font = new Font(this.Font, FontStyle.Regular);
            ucTextBox_lxrxm.Click += new EventHandler(ucTextBox_lxrxm_Click);


            //初始界面

            //显示身份证录入
            this.panelsfzinfo.Visible = false;
            this.panelqyinfo.Visible = false;
            this.panelqtzz.Visible = false;

            this.paneltijiao.Location = new Point(0, 334);

            this.panel_login.Height = this.panel_login.Height - this.panelsfzinfo.Height - this.panelqyinfo.Height - this.panelqtzz.Height;
            this.Height = this.Height - this.panelsfzinfo.Height - this.panelqyinfo.Height - this.panelqtzz.Height; ;


            //处理下拉框间距
            this.comboBox_xzjs.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.CB_DrawItem);
            this.comboBox_zclb.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.CB_DrawItem);
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

        private void ucTextBox_lxrxm_Click(object sender, EventArgs e)
        {
            // 当点击文本框，清空文本框内容         
            ucTextBox_lxrxm.Text = "";
            // 恢复文本框字体颜色黑色          
            ucTextBox_lxrxm.ForeColor = Color.Black;
            // 恢复文本框字体为标准       
            ucTextBox_lxrxm.Font = new Font(this.Font, FontStyle.Regular);
            // 去掉已注册的单击事件（这里值得注意）     
            this.ucTextBox_lxrxm.Click -= new System.EventHandler(ucTextBox_lxrxm_Click);
        }


        private void ucTextBox_xxdz_Click(object sender, EventArgs e)    
        {    
            // 当点击文本框，清空文本框内容         
            ucTextBox_xxdz.Text = "";            
            // 恢复文本框字体颜色黑色          
            ucTextBox_xxdz.ForeColor = Color.Black;            
            // 恢复文本框字体为标准       
            ucTextBox_xxdz.Font = new Font(this.Font, FontStyle.Regular);     
            // 去掉已注册的单击事件（这里值得注意）     
            this.ucTextBox_xxdz.Click -= new System.EventHandler(ucTextBox_xxdz_Click);   
        }

       

      
        private void FormLogin_Load(object sender, EventArgs e)
        {
            //=======================所有窗体都有这个玩意=========
            Init_one_show();
            //设置任务栏图标
            Icon ic = new Icon(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase+ @"\skin\ji.ico");
            this.Icon = ic;
            Initcobo();
    
            ucCityList1.initdefault();

        }

        private void Initcobo()
        {
            this.comboBox_xzjs.SelectedIndex = 0;
            this.comboBox_zclb.SelectedIndex = 0;
            if (this.comboBox_zclb.SelectedIndex == 1)
            {
                panelUC1.Enabled = false;
            }
        }

        private void FormLogin_FormClosed(object sender, FormClosedEventArgs e)
        {
            //if (Isreopen == "首次打开")
            //{
            //    //关闭登陆窗体时，同时关闭主窗体
            //    FMP.Close();
            //}
        }

       
       
        private void basicButton2_Click(object sender, EventArgs e)
        {
            //回车时，如果未启动这个按钮，不允许提交
            if (!BSave.Enabled)
            {
                return;
            }

            //根据登陆邮箱判断是否注册过结算账户

            //=================================

            #region//各种参数验证---作废
            //Hashtable InPutHT = new Hashtable();
            //ArrayList Almsg3 = new ArrayList();

          

            ////角色选择验证
            //InPutHT["角色编号"] = "";
            //if (this.comboBox_xzjs.SelectedIndex == 0)
            //{
            //    Almsg3.Add("");
            //    Almsg3.Add("结算账户类型尚未填写！");
            //    FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "错误", "中国商品批发交易平台", Almsg3);
            //    FRSE3.ShowDialog();
            //    return;

            //}
            //InPutHT["结算账户类型"] = this.comboBox_xzjs.SelectedItem.ToString();


            //if (!CB_zcxy.Checked)
            //{
            //    string Strmess = "请您仔细阅读" + this.comboBox_xzjs.SelectedItem.ToString() + "注册协议，并同意！";


            //    Almsg3.Add("");
            //    Almsg3.Add(Strmess);
            //    FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "错误", "中国商品批发交易平台", Almsg3);
            //    FRSE3.ShowDialog();
            //    return;

            //}


            ////经纪人选择验证
            //if (!this.comboBox_xzjs.SelectedItem.ToString().Equals("经纪人账户"))
            //{
            //    if (this.ucTextBox_jjrbh.Text.Trim().Equals(""))
            //    {
            //        Almsg3.Add("");
            //        Almsg3.Add("您还没有填写您的经纪人！");
            //    }
            //    else
            //    {
            //        if (label_jjrxm.Text == "" || label_jjrxm.Text.Equals("请输入可用的经纪人用户名"))
            //        {
            //            Almsg3.Add("");
            //            Almsg3.Add("您的经纪人不可用！");
            //        }
                    
            //        //请输入可用的经纪人账号
            //    }

            //    if (Almsg3 != null && Almsg3.Count > 0)
            //    {
            //        FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "错误", "中国商品批发交易平台", Almsg3);
            //        FRSE3.ShowDialog();
            //        return;
            //    }
       

            //}
            //InPutHT["关联经纪人编号"] = this.label_jjrxm.Text.Trim();

            ////联系人姓名验证
            //if (this.ucTextBox_lxrxm.Text.Trim().Equals(""))
            //{
            //    Almsg3.Add("");
            //    Almsg3.Add("您还没有填写您的联系人姓名！");
            //}
            //else if (this.ucTextBox_lxrxm.Text.Trim().Equals("请输入真实联系人姓名"))
            //{
            //    Almsg3.Add("");
            //    Almsg3.Add("您还没有填写您的联系人姓名！");
            //}
            //else
            //{
            //    if (this.ucTextBox_lxrxm.Text.Length > 60)
            //    {
            //        Almsg3.Add("");
            //        Almsg3.Add("联系人姓名长度不可超过60字！");

            //    }
            //    else
            //    {
            //        Hashtable htjjryhm = new Hashtable();
            //        htjjryhm["tszfcl"] = ucTextBox_lxrxm.Text.Trim();
            //        if (ValStr.ValidateQuery(htjjryhm))
            //        {
            //            // "联系人姓名中不可包含['],[,]特殊字符！";
            //            Almsg3.Add("");
            //            Almsg3.Add("联系人姓名中不可包含['],[,]等特殊字符！");
                       
            //        }
            //    }
            //}

            //if (Almsg3 != null && Almsg3.Count > 0)
            //{
            //    FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "错误", "中国商品批发交易平台", Almsg3);
            //    FRSE3.ShowDialog();
            //    return;
            //}
       
            //InPutHT["联系人姓名"] = this.ucTextBox_lxrxm.Text.Trim();

            ////手机号码验证   bool b = Regex.IsMatch(this.ucTextBox_sjh.Text.Trim(), @"^\d{11}$");
            //if (this.ucTextBox_sjh.Text.Trim().Equals(""))
            //{
            //    Almsg3.Add("");
            //    Almsg3.Add("手机号码必须填写！");
            //}
            //else
            //{
            //    if (!Regex.IsMatch(this.ucTextBox_sjh.Text.Trim(), @"^(13|15|18)[0-9]{9}$"))
            //    {
            //        Almsg3.Add("");
            //        Almsg3.Add("手机号码格式不正确！");
            //    }
            //}

            //if (Almsg3 != null && Almsg3.Count > 0)
            //{
            //    FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "错误", "中国商品批发交易平台", Almsg3);
            //    FRSE3.ShowDialog();
            //    return;
            //}
       

            //InPutHT["手机号"] = this.ucTextBox_sjh.Text.Trim();

            ////身份证号码验证//格式验证？？

            ////验证所属区域
            //if (ucCityList1.SelectedItem[0].ToString().Contains("请选择") || ucCityList1.SelectedItem[1].ToString().Contains("请选择")||ucCityList1.SelectedItem[2].ToString().Contains("请选择"))
            //{
            //     Almsg3.Add("");
            //     Almsg3.Add("请填写完整的省市区信息！");
            //     if (Almsg3 != null && Almsg3.Count > 0)
            //     {
            //         FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "错误", "中国商品批发交易平台", Almsg3);
            //         FRSE3.ShowDialog();
            //         return;
            //     }
       
            //}

            //InPutHT["省"] = ucCityList1.SelectedItem[0];
            //InPutHT["市"] = ucCityList1.SelectedItem[1];
            //InPutHT["区"] = ucCityList1.SelectedItem[2];


            ////详细地址验证

            //if (ucTextBox_xxdz.Text.Trim().Equals("") || ucTextBox_xxdz.Text.Trim().Equals("请不要重复输入省市区信息！"))
            //{
            //    Almsg3.Add("");
            //    Almsg3.Add("详细地址信息未填写！");
                

            //}
            //else if (ucTextBox_xxdz.Text.Length > 100)
            //{
            //    Almsg3.Add("");
            //    Almsg3.Add("详细地址信息内容长度不可超过100字！");

            //}
            //else
            //{
            //    Hashtable htjjryhm = new Hashtable();
            //    htjjryhm["tszfcl"] = ucTextBox_xxdz.Text.Trim();
            //    if (ValStr.ValidateQuery(htjjryhm))
            //    {
            //        // "联系人姓名中不可包含['],[,]特殊字符！";
            //        Almsg3.Add("");
            //        Almsg3.Add("详细地址中不可包含['],[,]等特殊字符！");
                    
            //    }
            //}

            //if (Almsg3 != null && Almsg3.Count > 0)
            //{
            //    FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "错误", "中国商品批发交易平台", Almsg3);
            //    FRSE3.ShowDialog();
            //    return;
            //}

            //InPutHT["详细地址"] = ucTextBox_xxdz.Text.Trim();


            ////邮政编码验证
            //if (ucTextBox_yzbm.Text.Trim().Equals(""))
            //{
            //    Almsg3.Add("");
            //    Almsg3.Add("邮政编码未填写！");
            //}
            //else
            //{
            //    if (!Support.ValStr.isPostalCode(ucTextBox_yzbm.Text.Trim()))
            //    {
            //        Almsg3.Add("");
            //        Almsg3.Add("邮政编码格式不正确！");
            //    }

            //}

            //if (Almsg3 != null && Almsg3.Count > 0)
            //{
            //    FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "错误", "中国商品批发交易平台", Almsg3);
            //    FRSE3.ShowDialog();
            //    return;
            //}
       
           
            //InPutHT["邮政编码"] = ucTextBox_yzbm.Text.Trim();


            ////注册类别
            //if (comboBox_zclb.SelectedIndex == 0)
            //{    
            //        Almsg3.Add("");
            //        Almsg3.Add("注册类别尚未选择！");
            //        if (Almsg3 != null && Almsg3.Count > 0)
            //        {
            //            FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "错误", "中国商品批发交易平台", Almsg3);
            //            FRSE3.ShowDialog();
            //            return;
            //        }
       
            //}

            //InPutHT["注册类别"] = comboBox_zclb.SelectedItem.ToString().Trim();


            ////身份证扫描件
            //if (this.panelsfzinfo.Visible)
            //{
            //    if (listvsfzfyj == null || listvsfzfyj.Items.Count <= 0)
            //    {
            //        Almsg3.Add("");
            //        Almsg3.Add("没有上传身份证扫描件！");
            //        InPutHT["身份证扫描件"] = "";
            //        FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "错误", "中国商品批发交易平台", Almsg3);
            //        FRSE3.ShowDialog();
            //        return;
            //    }
            //    else
            //    {

            //        InPutHT["身份证扫描件"] = listvsfzfyj.Items[0].SubItems[1].Text;
            //    }

            //    if (ucTextBox_sfzh.Text.Trim().Equals(""))
            //    {
            //        Almsg3.Add("");
            //        Almsg3.Add("身份证号码尚未填写！");
            //        FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "错误", "中国商品批发交易平台", Almsg3);
            //        FRSE3.ShowDialog();
            //        return;
            //    }
            //    else
            //    {
            //        if (!Support.ValStr.isIDCard(ucTextBox_sfzh.Text.Trim()))
            //        {
            //            Almsg3.Add("");
            //            Almsg3.Add("身份证号码格式不正确！");
            //            FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "错误", "中国商品批发交易平台", Almsg3);
            //            FRSE3.ShowDialog();
            //            return;
            //        }

            //    }

              
            //    //if (Almsg3 != null && Almsg3.Count > 0)
            //    //{
            //    //    FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "错误", "中国商品批发交易平台", Almsg3);
            //    //    FRSE3.ShowDialog();
            //    //    return;
            //    //}  

            //    InPutHT["身份证号码"] = ucTextBox_sfzh.Text.Trim();


            //    InPutHT["公司名称"] = "";
            //    InPutHT["公司电话"] = "";
            //    InPutHT["公司地址"] = "";
            //    InPutHT["营业执照"] = "";
            //    InPutHT["组织机构代码证"] = "";
            //    InPutHT["税务登记证"] = "";
            //    InPutHT["签字承诺书"] = "";
            //    InPutHT["开户许可证"] = "";

            //}

            ////公司信息
            //if (this.panelqyinfo.Visible)
            //{
            //    //if (this.comboBox_xzjs.SelectedIndex != 3)
            //    //{
            //    //    InPutHT["身份证扫描件"] = "";
            //    //    InPutHT["身份证号码"] = "";

            //    //}
            //    //else
            //    //{
            //        //公司名称
            //        if (this.ucTextBox_gsmc.Text.Trim().Equals(""))
            //        {
            //            Almsg3.Add("");
            //            Almsg3.Add("公司名称尚未填写！");
            //            //InPutHT["公司名称"] = "";
            //        }
            //        else
            //        {
            //            if (this.ucTextBox_gsmc.Text.Length > 100)
            //            {
            //                Almsg3.Add("");
            //                Almsg3.Add("公司名称信息长度不可超过100字！");
            //                // InPutHT["公司名称"] = "";
            //            }
            //            else
            //            {
            //                Hashtable htjjryhm = new Hashtable();
            //                htjjryhm["tszfcl"] = ucTextBox_gsmc.Text.Trim();
            //                if (ValStr.ValidateQuery(htjjryhm))
            //                {
            //                    // "联系人姓名中不可包含['],[,]特殊字符！";
            //                    Almsg3.Add("");
            //                    Almsg3.Add("详细地址中不可包含['],[,]等特殊字符！");
                               
            //                }
            //            }
            //        }

            //        //if (Almsg3 != null && Almsg3.Count > 0)
            //        //{
            //        //    FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "错误", "中国商品批发交易平台", Almsg3);
            //        //    FRSE3.ShowDialog();
            //        //    return;
            //        //}
            //        InPutHT["公司名称"] = this.ucTextBox_gsmc.Text.Trim();

            //        //公司电话
            //        if (this.ucTextBox_gsdh.Text.Trim().Equals(""))
            //        {
            //            Almsg3.Add("");
            //            Almsg3.Add("公司电话尚未填写！");
            //        }
            //        else
            //        {
            //            if (this.ucTextBox_gsdh.Text.Length > 20)
            //            {
            //                Almsg3.Add("");
            //                Almsg3.Add("公司电话长度不可超过20个字！");
            //            }
            //            else
            //            {

            //                Hashtable htjjryhm = new Hashtable();
            //                htjjryhm["tszfcl"] = ucTextBox_gsdh.Text.Trim();
            //                if (ValStr.ValidateQuery(htjjryhm))
            //                {
            //                    // "联系人姓名中不可包含['],[,]特殊字符！";
            //                    Almsg3.Add("");
            //                    Almsg3.Add("公司电话中不可包含['],[,]等特殊字符！");
                                
            //                }
            //            }
            //        }

            //        //if (Almsg3 != null && Almsg3.Count > 0)
            //        //{
            //        //    FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "错误", "中国商品批发交易平台", Almsg3);
            //        //    FRSE3.ShowDialog();
            //        //    return;
            //        //}
            //        InPutHT["公司电话"] = this.ucTextBox_gsdh.Text.Trim();


            //        //公司地址
            //        if (this.ucTextBox_gsdz.Text.Trim().Equals(""))
            //        {
            //            Almsg3.Add("");
            //            Almsg3.Add("公司地址尚未填写！");
            //        }
            //        else
            //        {
            //            if (this.ucTextBox_gsdz.Text.Length > 100)
            //            {
            //                Almsg3.Add("");
            //                Almsg3.Add("公司地址信息长度不可超过100字！");
            //            }
            //            else
            //            {
            //                Hashtable htjjryhm = new Hashtable();
            //                htjjryhm["tszfcl"] = ucTextBox_gsdz.Text.Trim();
            //                if (ValStr.ValidateQuery(htjjryhm))
            //                {
            //                    // "联系人姓名中不可包含['],[,]特殊字符！";
            //                    Almsg3.Add("");
            //                    Almsg3.Add("公司地址中不可包含['],[,]等特殊字符！");
                               
            //                }
            //            }
            //        }

            //        //if (Almsg3 != null && Almsg3.Count > 0)
            //        //{
            //        //    FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "错误", "中国商品批发交易平台", Almsg3);
            //        //    FRSE3.ShowDialog();
            //        //    return;
            //        //}
            //        InPutHT["公司地址"] = this.ucTextBox_gsdz.Text.Trim();


            //        //营业执照
            //        if (this.listvyyzz == null || this.listvyyzz.Items.Count == 0)
            //        {
            //            Almsg3.Add("");
            //            Almsg3.Add("营业执照信息尚未上传！");
            //            InPutHT["营业执照"] = "";
            //        }
            //        else
            //        {
            //            InPutHT["营业执照"] = this.listvyyzz.Items[0].SubItems[1].Text.Trim();
            //        }

            //        //if (Almsg3 != null && Almsg3.Count > 0)
            //        //{
            //        //    FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "错误", "中国商品批发交易平台", Almsg3);
            //        //    FRSE3.ShowDialog();
            //        //    return;
            //        //}

            //        //组织机构
            //        if (this.listVzzjgdmz == null || this.listVzzjgdmz.Items.Count == 0)
            //        {
            //            Almsg3.Add("");
            //            Almsg3.Add("组织机构代码证信息尚未上传！");
            //            InPutHT["组织机构代码证"] = "";
            //        }
            //        else
            //        {
            //            InPutHT["组织机构代码证"] = this.listVzzjgdmz.Items[0].SubItems[1].Text.Trim();
            //        }

            //        //if (Almsg3 != null && Almsg3.Count > 0)
            //        //{
            //        //    FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "错误", "中国商品批发交易平台", Almsg3);
            //        //    FRSE3.ShowDialog();
            //        //    return;
            //        //}

            //        //税务登记
            //        if (this.listVswdjz == null || this.listVswdjz.Items.Count == 0)
            //        {
            //            Almsg3.Add("");
            //            Almsg3.Add("税务登记证信息尚未上传！");
            //            InPutHT["税务登记证"] = "";
            //        }
            //        else
            //        {
            //            InPutHT["税务登记证"] = this.listVswdjz.Items[0].SubItems[1].Text.Trim();
            //        }

            //        //if (Almsg3 != null && Almsg3.Count > 0)
            //        //{
            //        //    FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "错误", "中国商品批发交易平台", Almsg3);
            //        //    FRSE3.ShowDialog();
            //        //    return;
            //        //}

            //        //开户许可证
            //        if (this.listVkhxkz == null || this.listVkhxkz.Items.Count == 0)
            //        {
            //            Almsg3.Add("");
            //            Almsg3.Add("开户许可证信息尚未上传！");
            //            InPutHT["开户许可证"] = "";
            //        }
            //        else
            //        {
            //            InPutHT["开户许可证"] = this.listVkhxkz.Items[0].SubItems[1].Text.Trim();
            //        }

            //        //if (Almsg3 != null && Almsg3.Count > 0)
            //        //{
            //        //    FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "错误", "中国商品批发交易平台", Almsg3);
            //        //    FRSE3.ShowDialog();
            //        //    return;
            //        //}

            //        InPutHT["签字承诺书"] = "";

            //        if (this.comboBox_xzjs.SelectedIndex != 3)
            //        {
            //            InPutHT["身份证扫描件"] = "";
            //            InPutHT["身份证号码"] = "";
            //            if (Almsg3 != null && Almsg3.Count > 0)
            //            {
            //                FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "错误", "中国商品批发交易平台", Almsg3);
            //                FRSE3.ShowDialog();
            //                return;
            //            }

            //        }
       
                    


            //   // }


            //}
#endregion


            //各种参数验证
            Hashtable InPutHT = new Hashtable();
            Hashtable htPublicYZ = PublicYZ();
            //角色选择验证
            InPutHT["角色编号"] = "";
            switch (comboBox_xzjs.SelectedIndex)
            {
                case 0://选择结算账户角色
                    ReturnMessage("结算账户类型尚未填写！");
                    return;
                #region//经纪人账户
                case 1://经纪人账户
                    InPutHT["结算账户类型"] = this.comboBox_xzjs.SelectedItem.ToString();
                    InPutHT["关联经纪人编号"] = this.label_jjrxm.Text.Trim();
                    #region//前面公共信息
                    if (htPublicYZ["yz"].ToString() == "Error")
                    {
                        ReturnMessage(htPublicYZ["yzjg"].ToString());
                        return;
                    }

                    InPutHT["联系人姓名"] = htPublicYZ["联系人姓名"].ToString();
                    InPutHT["手机号"] = htPublicYZ["手机号"].ToString();
                    InPutHT["省"] = htPublicYZ["省"].ToString();
                    InPutHT["市"] = htPublicYZ["市"].ToString();
                    InPutHT["区"] = htPublicYZ["区"].ToString();
                    InPutHT["详细地址"] = htPublicYZ["详细地址"].ToString();
                    InPutHT["邮政编码"] = htPublicYZ["邮政编码"].ToString();
                    #endregion

                    //注册类别
                    if (comboBox_zclb.SelectedIndex == 0)
                    {
                        ReturnMessage("注册类别尚未选择！");
                        return;
                    }
                    InPutHT["注册类别"] = comboBox_zclb.SelectedItem.ToString().Trim();

                    #region//身份证扫描件
                    if (this.panelsfzinfo.Visible)
                    {
                        if (ucTextBox_sfzh.Text.Trim().Equals(""))
                        {
                            ReturnMessage("身份证号码尚未填写！");
                            return;
                        }
                        else
                        {
                            if (!Support.ValStr.isIDCard(ucTextBox_sfzh.Text.Trim()))
                            {
                                ReturnMessage("身份证号码格式不正确！");
                                return;
                            }

                        }

                        InPutHT["身份证号码"] = ucTextBox_sfzh.Text.Trim();
                        if (listvsfzfyj == null || listvsfzfyj.Items.Count <= 0)
                        {
                            ReturnMessage("没有上传身份证扫描件！");
                            InPutHT["身份证扫描件"] = "";
                            return;
                        }
                        else
                        {
                            InPutHT["身份证扫描件"] = listvsfzfyj.Items[0].SubItems[1].Text;
                        }

                        

                        InPutHT["公司名称"] = "";
                        InPutHT["公司电话"] = "";
                        InPutHT["公司地址"] = "";
                        InPutHT["营业执照"] = "";
                        InPutHT["组织机构代码证"] = "";
                        InPutHT["税务登记证"] = "";
                        InPutHT["签字承诺书"] = "";
                        InPutHT["开户许可证"] = "";

                    }
                    else
                    {
                        InPutHT["身份证扫描件"] = "";
                        InPutHT["身份证号码"] = "";
                    }
                    #endregion
                    #region//公司信息
                    if (this.panelqyinfo.Visible)
                    {
                        //公司名称
                        if (this.ucTextBox_gsmc.Text.Trim().Equals(""))
                        {
                            ReturnMessage("公司名称尚未填写！");
                            return;
                        }
                        else
                        {
                            if (this.ucTextBox_gsmc.Text.Length > 100)
                            {
                                ReturnMessage("公司名称信息长度不可超过100字！");
                                return;
                            }
                            else
                            {
                                Hashtable htjjryhm = new Hashtable();
                                htjjryhm["tszfcl"] = ucTextBox_gsmc.Text.Trim();
                                if (ValStr.ValidateQuery(htjjryhm))
                                {
                                    ReturnMessage("公司名称信息不可包含['],[,]等特殊字符！");
                                    return;
                                }
                            }
                        }

                        InPutHT["公司名称"] = this.ucTextBox_gsmc.Text.Trim();

                        //公司电话
                        if (this.ucTextBox_gsdh.Text.Trim().Equals(""))
                        {
                            ReturnMessage("公司电话尚未填写！");
                            return;
                        }
                        else
                        {
                            if (this.ucTextBox_gsdh.Text.Length > 20)
                            {
                                ReturnMessage("公司电话长度不可超过20个字！");
                                return;
                            }
                            else
                            {

                                Hashtable htjjryhm = new Hashtable();
                                htjjryhm["tszfcl"] = ucTextBox_gsdh.Text.Trim();
                                if (ValStr.ValidateQuery(htjjryhm))
                                {
                                    // "联系人姓名中不可包含['],[,]特殊字符！";
                                    ReturnMessage("公司电话中不可包含['],[,]等特殊字符！");
                                    return;
                                }
                            }
                        }

                        InPutHT["公司电话"] = this.ucTextBox_gsdh.Text.Trim();

                        //公司地址
                        if (this.ucTextBox_gsdz.Text.Trim().Equals(""))
                        {
                            ReturnMessage("公司地址尚未填写！");
                            return;
                        }
                        else
                        {
                            if (this.ucTextBox_gsdz.Text.Length > 100)
                            {
                                ReturnMessage("公司地址信息长度不可超过100字！");
                                return;
                            }
                            else
                            {
                                Hashtable htjjryhm = new Hashtable();
                                htjjryhm["tszfcl"] = ucTextBox_gsdz.Text.Trim();
                                if (ValStr.ValidateQuery(htjjryhm))
                                {
                                    // "联系人姓名中不可包含['],[,]特殊字符！";
                                    ReturnMessage("公司地址中不可包含['],[,]等特殊字符！");
                                    return;
                                }
                            }
                        }

                        InPutHT["公司地址"] = this.ucTextBox_gsdz.Text.Trim();


                        //营业执照
                        if (this.listvyyzz == null || this.listvyyzz.Items.Count == 0)
                        {
                            ReturnMessage("营业执照信息尚未上传！");
                            InPutHT["营业执照"] = "";
                            return;
                        }
                        else
                        {
                            InPutHT["营业执照"] = this.listvyyzz.Items[0].SubItems[1].Text.Trim();
                        }

                        //组织机构
                        if (this.listVzzjgdmz == null || this.listVzzjgdmz.Items.Count == 0)
                        {
                            ReturnMessage("组织机构代码证信息尚未上传！");
                            InPutHT["组织机构代码证"] = "";
                            return;
                        }
                        else
                        {
                            InPutHT["组织机构代码证"] = this.listVzzjgdmz.Items[0].SubItems[1].Text.Trim();
                        }


                        //税务登记
                        if (this.listVswdjz == null || this.listVswdjz.Items.Count == 0)
                        {
                            ReturnMessage("税务登记证信息尚未上传！");
                            InPutHT["税务登记证"] = "";
                            return;
                        }
                        else
                        {
                            InPutHT["税务登记证"] = this.listVswdjz.Items[0].SubItems[1].Text.Trim();
                        }


                        //开户许可证
                        if (this.listVkhxkz == null || this.listVkhxkz.Items.Count == 0)
                        {
                            ReturnMessage("开户许可证信息尚未上传！");
                            InPutHT["开户许可证"] = "";
                            return;
                        }
                        else
                        {
                            InPutHT["开户许可证"] = this.listVkhxkz.Items[0].SubItems[1].Text.Trim();
                        }


                        InPutHT["签字承诺书"] = "";
                    }
                    #endregion

                    if (!CB_zcxy.Checked)
                    {
                        ReturnMessage("请您仔细阅读" + this.comboBox_xzjs.SelectedItem.ToString() + "注册协议，并同意！");
                        return;
                    }
                    break;
                #endregion
                #region//卖家账户
                case 2://卖家账户
                    InPutHT["结算账户类型"] = this.comboBox_xzjs.SelectedItem.ToString();

                    if (this.ucTextBox_jjrbh.Text.Trim().Equals(""))
                    {
                        ReturnMessage("您还没有填写您的经纪人！");
                        return;
                    }
                    else
                    {
                        if (label_jjrxm.Text == "" || label_jjrxm.Text.Equals("请输入可用的经纪人用户名"))
                        {
                            ReturnMessage("您的经纪人不可用！");
                            return;
                        }

                        //请输入可用的经纪人账号
                    }
                    InPutHT["关联经纪人编号"] = this.label_jjrxm.Text.Trim();
                    #region//前面公共信息
                    if (htPublicYZ["yz"].ToString() == "Error")
                    {
                        ReturnMessage(htPublicYZ["yzjg"].ToString());
                        return;
                    }
                    InPutHT["联系人姓名"] = htPublicYZ["联系人姓名"].ToString();
                    InPutHT["手机号"] = htPublicYZ["手机号"].ToString();
                    InPutHT["省"] = htPublicYZ["省"].ToString();
                    InPutHT["市"] = htPublicYZ["市"].ToString();
                    InPutHT["区"] = htPublicYZ["区"].ToString();
                    InPutHT["详细地址"] = htPublicYZ["详细地址"].ToString();
                    InPutHT["邮政编码"] = htPublicYZ["邮政编码"].ToString();
                    #endregion

                    InPutHT["注册类别"] = comboBox_zclb.SelectedItem.ToString().Trim();
                    InPutHT["身份证扫描件"] = "";
                    InPutHT["身份证号码"] = "";

                    #region//公司信息
                    if (this.panelqyinfo.Visible)
                    {
                        //公司名称
                        if (this.ucTextBox_gsmc.Text.Trim().Equals(""))
                        {
                            ReturnMessage("公司名称尚未填写！");
                            return;
                        }
                        else
                        {
                            if (this.ucTextBox_gsmc.Text.Length > 100)
                            {
                                ReturnMessage("公司名称信息长度不可超过100字！");
                                return;
                            }
                            else
                            {
                                Hashtable htjjryhm = new Hashtable();
                                htjjryhm["tszfcl"] = ucTextBox_gsmc.Text.Trim();
                                if (ValStr.ValidateQuery(htjjryhm))
                                {
                                    ReturnMessage("公司名称信息不可包含['],[,]等特殊字符！");
                                    return;
                                }
                            }
                        }

                        InPutHT["公司名称"] = this.ucTextBox_gsmc.Text.Trim();

                        //公司电话
                        if (this.ucTextBox_gsdh.Text.Trim().Equals(""))
                        {
                            ReturnMessage("公司电话尚未填写！");
                            return;
                        }
                        else
                        {
                            if (this.ucTextBox_gsdh.Text.Length > 20)
                            {
                                ReturnMessage("公司电话长度不可超过20个字！");
                                return;
                            }
                            else
                            {

                                Hashtable htjjryhm = new Hashtable();
                                htjjryhm["tszfcl"] = ucTextBox_gsdh.Text.Trim();
                                if (ValStr.ValidateQuery(htjjryhm))
                                {
                                    // "联系人姓名中不可包含['],[,]特殊字符！";
                                    ReturnMessage("公司电话中不可包含['],[,]等特殊字符！");
                                    return;
                                }
                            }
                        }

                        InPutHT["公司电话"] = this.ucTextBox_gsdh.Text.Trim();

                        //公司地址
                        if (this.ucTextBox_gsdz.Text.Trim().Equals(""))
                        {
                            ReturnMessage("公司地址尚未填写！");
                            return;
                        }
                        else
                        {
                            if (this.ucTextBox_gsdz.Text.Length > 100)
                            {
                                ReturnMessage("公司地址信息长度不可超过100字！");
                                return;
                            }
                            else
                            {
                                Hashtable htjjryhm = new Hashtable();
                                htjjryhm["tszfcl"] = ucTextBox_gsdz.Text.Trim();
                                if (ValStr.ValidateQuery(htjjryhm))
                                {
                                    // "联系人姓名中不可包含['],[,]特殊字符！";
                                    ReturnMessage("公司地址中不可包含['],[,]等特殊字符！");
                                    return;
                                }
                            }
                        }

                        InPutHT["公司地址"] = this.ucTextBox_gsdz.Text.Trim();


                        //营业执照
                        if (this.listvyyzz == null || this.listvyyzz.Items.Count == 0)
                        {
                            ReturnMessage("营业执照信息尚未上传！");
                            InPutHT["营业执照"] = "";
                            return;
                        }
                        else
                        {
                            InPutHT["营业执照"] = this.listvyyzz.Items[0].SubItems[1].Text.Trim();
                        }

                        //组织机构
                        if (this.listVzzjgdmz == null || this.listVzzjgdmz.Items.Count == 0)
                        {
                            ReturnMessage("组织机构代码证信息尚未上传！");
                            InPutHT["组织机构代码证"] = "";
                            return;
                        }
                        else
                        {
                            InPutHT["组织机构代码证"] = this.listVzzjgdmz.Items[0].SubItems[1].Text.Trim();
                        }


                        //税务登记
                        if (this.listVswdjz == null || this.listVswdjz.Items.Count == 0)
                        {
                            ReturnMessage("税务登记证信息尚未上传！");
                            InPutHT["税务登记证"] = "";
                            return;
                        }
                        else
                        {
                            InPutHT["税务登记证"] = this.listVswdjz.Items[0].SubItems[1].Text.Trim();
                        }


                        //开户许可证
                        if (this.listVkhxkz == null || this.listVkhxkz.Items.Count == 0)
                        {
                            ReturnMessage("开户许可证信息尚未上传！");
                            InPutHT["开户许可证"] = "";
                            return;
                        }
                        else
                        {
                            InPutHT["开户许可证"] = this.listVkhxkz.Items[0].SubItems[1].Text.Trim();
                        }


                        InPutHT["签字承诺书"] = "";
                    }
                    #endregion
                    if (!CB_zcxy.Checked)
                    {
                        ReturnMessage("请您仔细阅读" + this.comboBox_xzjs.SelectedItem.ToString() + "注册协议，并同意！");
                        return;
                    }
                    break;
                #endregion
                #region//买家账户
                case 3://买家账户
                    InPutHT["结算账户类型"] = this.comboBox_xzjs.SelectedItem.ToString();

                    if (this.ucTextBox_jjrbh.Text.Trim().Equals(""))
                    {
                        ReturnMessage("您还没有填写您的经纪人！");
                        return;
                    }
                    else
                    {
                        if (label_jjrxm.Text == "" || label_jjrxm.Text.Equals("请输入可用的经纪人用户名"))
                        {
                            ReturnMessage("您的经纪人不可用！");
                            return;
                        }

                        //请输入可用的经纪人账号
                    }
                    InPutHT["关联经纪人编号"] = this.label_jjrxm.Text.Trim();
                    #region//前面公共信息
                    if (htPublicYZ["yz"].ToString() == "Error")
                    {
                        ReturnMessage(htPublicYZ["yzjg"].ToString());
                        return;
                    }
                    InPutHT["联系人姓名"] = htPublicYZ["联系人姓名"].ToString();
                    InPutHT["手机号"] = htPublicYZ["手机号"].ToString();
                    InPutHT["省"] = htPublicYZ["省"].ToString();
                    InPutHT["市"] = htPublicYZ["市"].ToString();
                    InPutHT["区"] = htPublicYZ["区"].ToString();
                    InPutHT["详细地址"] = htPublicYZ["详细地址"].ToString();
                    InPutHT["邮政编码"] = htPublicYZ["邮政编码"].ToString();
                    #endregion
                    InPutHT["注册类别"] = comboBox_zclb.SelectedItem.ToString().Trim();

                    #region//身份证扫描件
                    if (this.panelsfzinfo.Visible)
                    {
                        if (ucTextBox_sfzh.Text.Trim().Equals(""))
                        {
                            ReturnMessage("身份证号码尚未填写！");
                            return;
                        }
                        else
                        {
                            if (!Support.ValStr.isIDCard(ucTextBox_sfzh.Text.Trim()))
                            {
                                ReturnMessage("身份证号码格式不正确！");
                                return;
                            }

                        }

                        InPutHT["身份证号码"] = ucTextBox_sfzh.Text.Trim();
                        if (listvsfzfyj == null || listvsfzfyj.Items.Count <= 0)
                        {
                            ReturnMessage("没有上传身份证扫描件！");
                            InPutHT["身份证扫描件"] = "";
                            return;
                        }
                        else
                        {
                            InPutHT["身份证扫描件"] = listvsfzfyj.Items[0].SubItems[1].Text;
                        }

                        
                    }
                    #endregion

                    #region//公司信息
                    if (this.panelqyinfo.Visible)
                    {
                        //公司名称
                        if (this.ucTextBox_gsmc.Text.Length > 100)
                        {
                            ReturnMessage("公司名称信息长度不可超过100字！");
                            return;
                        }
                        else
                        {
                            Hashtable htjjryhm = new Hashtable();
                            htjjryhm["tszfcl"] = ucTextBox_gsmc.Text.Trim();
                            if (ValStr.ValidateQuery(htjjryhm))
                            {
                                ReturnMessage("公司名称信息不可包含['],[,]等特殊字符！");
                                return;
                            }
                        }

                        InPutHT["公司名称"] = this.ucTextBox_gsmc.Text.Trim();

                        //公司电话
                        if (this.ucTextBox_gsdh.Text.Length > 20)
                        {
                            ReturnMessage("公司电话长度不可超过20个字！");
                            return;
                        }
                        else
                        {

                            Hashtable htjjryhm = new Hashtable();
                            htjjryhm["tszfcl"] = ucTextBox_gsdh.Text.Trim();
                            if (ValStr.ValidateQuery(htjjryhm))
                            {
                                // "联系人姓名中不可包含['],[,]特殊字符！";
                                ReturnMessage("公司电话中不可包含['],[,]等特殊字符！");
                                return;
                            }
                        }

                        InPutHT["公司电话"] = this.ucTextBox_gsdh.Text.Trim();

                        //公司地址
                        if (this.ucTextBox_gsdz.Text.Length > 100)
                        {
                            ReturnMessage("公司地址信息长度不可超过100字！");
                            return;
                        }
                        else
                        {
                            Hashtable htjjryhm = new Hashtable();
                            htjjryhm["tszfcl"] = ucTextBox_gsdz.Text.Trim();
                            if (ValStr.ValidateQuery(htjjryhm))
                            {
                                // "联系人姓名中不可包含['],[,]特殊字符！";
                                ReturnMessage("公司地址中不可包含['],[,]等特殊字符！");
                                return;
                            }
                        }

                        InPutHT["公司地址"] = this.ucTextBox_gsdz.Text.Trim();


                        //营业执照
                        if (this.listvyyzz == null || this.listvyyzz.Items.Count == 0)
                        {
                            InPutHT["营业执照"] = "";
                        }
                        else
                        {
                            InPutHT["营业执照"] = this.listvyyzz.Items[0].SubItems[1].Text.Trim();
                        }

                        //组织机构
                        if (this.listVzzjgdmz == null || this.listVzzjgdmz.Items.Count == 0)
                        {
                            InPutHT["组织机构代码证"] = "";
                        }
                        else
                        {
                            InPutHT["组织机构代码证"] = this.listVzzjgdmz.Items[0].SubItems[1].Text.Trim();
                        }


                        //税务登记
                        if (this.listVswdjz == null || this.listVswdjz.Items.Count == 0)
                        {
                            InPutHT["税务登记证"] = "";
                        }
                        else
                        {
                            InPutHT["税务登记证"] = this.listVswdjz.Items[0].SubItems[1].Text.Trim();
                        }


                        //开户许可证
                        if (this.listVkhxkz == null || this.listVkhxkz.Items.Count == 0)
                        {
                            InPutHT["开户许可证"] = "";
                        }
                        else
                        {
                            InPutHT["开户许可证"] = this.listVkhxkz.Items[0].SubItems[1].Text.Trim();
                        }


                        InPutHT["签字承诺书"] = "";
                    }
                    #endregion

                    if (!CB_zcxy.Checked)
                    {
                        ReturnMessage("请您仔细阅读" + this.comboBox_xzjs.SelectedItem.ToString() + "注册协议，并同意！");
                        return;
                    }
                    break;
                #endregion
                default:
                    break;
            }

            if (this.listVqtzzwj != null && this.listVqtzzwj.Items.Count > 0)
            {
                InPutHT["其他资质文件"] = this.listVqtzzwj.Items[0].SubItems[1].Text; ;
            }
            else
            {
                InPutHT["其他资质文件"] = "";
            }

            InPutHT["登陆邮箱"] = HTuser["dlyx"].ToString();
            InPutHT["用户名"] = HTuser["yhm"].ToString();
            InPutHT["是否验证手机号"] = "否";
            InPutHT["手机号验证码"] = "";
            InPutHT["所属分公司"] = "待分析";
            InPutHT["手机号验证通过时间"] = "";
            InPutHT["关联代理人用户名"] = ucTextBox_jjrbh.Text;//label_jjrxm.Text;


            string ip = "xxx";


            //显示等待
            BSave.Enabled = false;
            basicButton1.Enabled = false;
            PBload.Visible = true;

            //开启线程进行服务器验证
         
            InPutHT["IP地址"] = "";
            delegateForThread tempDFT = new delegateForThread(ShowThreadResult_Login);
            RunThreaClassJszhsq RTCCL = new RunThreaClassJszhsq(InPutHT, tempDFT); //DataControl.RunThreadClassLogin(InPutHT, tempDFT,"insert")
            Thread trd = new Thread(new ThreadStart(RTCCL.BeginRun));
            trd.IsBackground = true;
            trd.Start();

        }
        #region//周丽-验证
        //弹窗提示
        private void ReturnMessage(string message)
        {
            ArrayList Almsg = new ArrayList();
            Almsg.Add("");
            Almsg.Add(message);
            FormAlertMessage FRSE = new FormAlertMessage("仅确定", "错误", "中国商品批发交易平台", Almsg);
            FRSE.ShowDialog();
        }

        private Hashtable PublicYZ()
        {
            Hashtable InPutHT = new Hashtable();
            InPutHT["yz"] = "OK";
            InPutHT["yzjg"] = "";
            //联系人姓名验证
            if (this.ucTextBox_lxrxm.Text.Trim().Equals(""))
            {
                //ReturnMessage("您还没有填写您的联系人姓名！");
                InPutHT["yz"] = "Error";
                InPutHT["yzjg"] = "您还没有填写您的联系人姓名！";
                return InPutHT;
            }
            else if (this.ucTextBox_lxrxm.Text.Trim().Equals("请输入真实联系人姓名"))
            {
                InPutHT["yz"] = "Error";
                InPutHT["yzjg"] = "您还没有填写您的联系人姓名！";
                return InPutHT;
            }
            else
            {
                if (this.ucTextBox_lxrxm.Text.Length > 60)
                {
                    InPutHT["yz"] = "Error";
                    InPutHT["yzjg"] = "您还没有填写您的联系人姓名！";
                    return InPutHT;
                }
                else
                {
                    Hashtable htjjryhm = new Hashtable();
                    htjjryhm["tszfcl"] = ucTextBox_lxrxm.Text.Trim();
                    if (ValStr.ValidateQuery(htjjryhm))
                    {
                        // "联系人姓名中不可包含['],[,]特殊字符！";   
                        InPutHT["yz"] = "Error";
                        InPutHT["yzjg"] = "联系人姓名中不可包含['],[,]等特殊字符！";
                        return InPutHT;
                    }
                }
            }
            InPutHT["联系人姓名"] = this.ucTextBox_lxrxm.Text.Trim();

            if (this.ucTextBox_sjh.Text.Trim().Equals(""))
            {
                InPutHT["yz"] = "Error";
                InPutHT["yzjg"] = "手机号码必须填写！";
                return InPutHT;
            }
            else
            {
                if (!Regex.IsMatch(this.ucTextBox_sjh.Text.Trim(), @"^(13|15|18)[0-9]{9}$"))
                {
                    InPutHT["yz"] = "Error";
                    InPutHT["yzjg"] = "手机号码格式不正确！";
                    return InPutHT;
                }
            }
            InPutHT["手机号"] = this.ucTextBox_sjh.Text.Trim();
            //验证所属区域
            if (ucCityList1.SelectedItem[0].ToString().Contains("请选择") || ucCityList1.SelectedItem[1].ToString().Contains("请选择") || ucCityList1.SelectedItem[2].ToString().Contains("请选择"))
            {
                InPutHT["yz"] = "Error";
                InPutHT["yzjg"] = "请填写完整的省市区信息！";
                return InPutHT;
            }

            InPutHT["省"] = ucCityList1.SelectedItem[0];
            InPutHT["市"] = ucCityList1.SelectedItem[1];
            InPutHT["区"] = ucCityList1.SelectedItem[2];
            //详细地址验证

            if (ucTextBox_xxdz.Text.Trim().Equals("") || ucTextBox_xxdz.Text.Trim().Equals("请不要重复输入省市区信息！"))
            {           
                InPutHT["yz"] = "Error";
                InPutHT["yzjg"] = "详细地址信息未填写！";
                return InPutHT;

            }
            else if (ucTextBox_xxdz.Text.Length > 100)
            {
                InPutHT["yz"] = "Error";
                InPutHT["yzjg"] = "详细地址信息内容长度不可超过100字！";
                return InPutHT;
            }
            else
            {
                Hashtable htjjryhm = new Hashtable();
                htjjryhm["tszfcl"] = ucTextBox_xxdz.Text.Trim();
                if (ValStr.ValidateQuery(htjjryhm))
                {
                    // "联系人姓名中不可包含['],[,]特殊字符！";    
                    InPutHT["yz"] = "Error";
                    InPutHT["yzjg"] = "详细地址中不可包含['],[,]等特殊字符！";
                    return InPutHT;
                }
            }
            InPutHT["详细地址"] = ucTextBox_xxdz.Text.Trim();
            //邮政编码验证
            if (ucTextBox_yzbm.Text.Trim().Equals(""))
            {
                InPutHT["yz"] = "Error";
                InPutHT["yzjg"] = "邮政编码未填写！";
                return InPutHT;
            }
            else
            {
                if (!Support.ValStr.isPostalCode(ucTextBox_yzbm.Text.Trim()))
                {
                    InPutHT["yz"] = "Error";
                    InPutHT["yzjg"] = "邮政编码格式不正确！";
                    return InPutHT;
                }
            }
            InPutHT["邮政编码"] = ucTextBox_yzbm.Text.Trim();
            return InPutHT;

        }
     
        #endregion

        //显示线程处理结果的函数(用于处理线程返回数据)
        private void ShowThreadResult_Login(Hashtable OutPutHT)
        {
            try
            {
                //调用异步委托
                Invoke(new delegateForThreadShow(ShowThreadResult_Login_Invoke), new Hashtable[] { OutPutHT });
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("委托回调错误：" + ex.ToString());
            }
        }


        //处理非线程创建的控件
        private void ShowThreadResult_Login_Invoke(Hashtable OutPutHT)
        {


            //演示用账号密码验证登陆后，显示主窗体并关闭登陆窗口
            string[] arrReturn = OutPutHT["登陆结果"].ToString().Split('〓');
            string dsuser = arrReturn[0]; //获取登陆结果
           // PublicDS.PublisDsUser = dsuser; //告诉主窗体登陆结果
            ArrayList Almsg3 = new ArrayList();

            if (dsuser.Trim().Equals("True"))
            {
                Almsg3.Add("");
                Almsg3.Add("提交成功！");
                if (this.comboBox_xzjs.SelectedItem.ToString().Trim().Equals("经纪人账户"))
                {
                    //提交成功！我们将尽快对您提交的详细资料进行审核，审核通过后您可以使用“经纪人结算账户”和“买家结算账户”。同时请尽快将您的营业执照、组织机构代码证、税务登记证、开户许可证证件影印件邮寄至分公司。

                    Almsg3.Add("我们将在3个工作日内对您提交的详细资料进行审核，审核通过后");

                    Almsg3.Add("您可以使用“经纪人结算账户”和“买家结算账户”。");

                    Almsg3.Add("同时请尽快将您的营业执照、组织机构代码证、税务登记证、开户许可证证件影印件邮寄至分公司。");

             
                    


                }
                if (this.comboBox_xzjs.SelectedItem.ToString().Trim().Equals("卖家账户"))
                {
                    //您的经纪人将尽快对您提交的详细资料进行审核，审核通过后您可以使用“卖家结算账户”和“买家结算账户”。同时请尽快将您的营业执照、组织机构代码证、税务登记证、开户许可证等证件影件邮寄至经纪人。

                    Almsg3.Add("您的经纪人将尽快对您提交的详细资料进行审核，审核通过后");

                    Almsg3.Add("您可以使用“卖家结算账户”和“买家结算账户”。");

                    Almsg3.Add("同时请尽快将您的营业执照、组织机构代码证、税务登记证、开户许可证等证件影件邮寄至经纪人。");

                    //Almsg3.Add("经纪人姓名：** 联系电话：** 邮编***   详细地址：***");

           

                }

                if (this.comboBox_xzjs.SelectedItem.ToString().Trim().Equals("买家账户"))
                {
                    //！您的经纪人将尽快对您提交的详细资料进行审核，审核通过后您可以使用 “买家结算账户”。

                    Almsg3.Add("您的经纪人将尽快对您提交的详细资料进行审核，");

                    Almsg3.Add("审核通过后您可以使用“买家结算账户”。");
                    // Almsg3.Add("同时请尽快将您的营业执照、组织机构代码证、税务登记证、开户许可证证件影印件邮寄至分公司。");
                    //Almsg3.Add(" 详细地址：***。");

                    
                }

                Almsg3.Add("分公司名称：" + arrReturn[1]);
                Almsg3.Add("地址：" + arrReturn[2]);
                Almsg3.Add("电话：" + arrReturn[3]);
                Almsg3.Add("邮编：" + arrReturn[4]);
                PublicDS.PublisDsUser.Tables[0].Rows[0]["JSZHLX"] = this.comboBox_xzjs.SelectedItem.ToString();

                FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "叹号", "中国商品批发交易平台", Almsg3);
                FRSE3.ShowDialog();
                this.Close();

            }
            else
            {
                if (dsuser.Trim().Equals("系统繁忙"))
                {
                    Almsg3.Add("系统繁忙请稍后再试！");
                }
                else
                {
                    Almsg3.Add("信息注册失败！");

                }
                //检查完毕后，恢复按钮
                //释放资源.?需要不需要？
                BSave.Enabled = true;
                basicButton1.Enabled = true;
                PBload.Visible = false;

                FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "叹号", "中国商品批发交易平台", Almsg3);
                FRSE3.ShowDialog();
            }
           
            //Almsg3.Add("");
            //Almsg3.Add(dsuser);
           

            
        }

       /// <summary>
        /// 附件上传按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void B_SC_Click(object sender, EventArgs e)
        {
            //开启上传
            //根据控件ID判断数据存储到那个listView
            Label lb = sender as Label;
            string StrID= lb.Name;
            if (StrID != "")
            {
                OpenFileDialog ofd = null;
                ListView lv = null;
                if (StrID.Trim().Equals("labsc_qtzz"))
                {
                    ofd = openFileDialog2;
                   
                }
                else
                {
                    ofd = openFileDialog1;
                }

                switch (StrID)
                {
                    case "labsc_qtzz":
                        lv = listVqtzzwj;
                        break;
                    case "B_SC":
                        lv = listvsfzfyj;
                        break;
                    case "labelsc_yyzz":
                        lv = listvyyzz;
                        break;
                    case "labelsc_zzjgdm":
                        lv = listVzzjgdmz;
                        break;
                    case "labelsc_swdjz":
                        lv = listVswdjz;
                        break;
                    case "labelsc_khxukez":
                        lv = listVkhxkz;
                        break;
                    case "labelsc_qzcns":
                        lv = listVqzcns;
                        break;
                    default:
                        break;

                }

                if (ofd != null && lv != null)
                {

                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        string[] fName = ofd.FileNames;
                        

                        //若选择选择对话框允许选择多个，则还要检验不能超过5个
                        //多不是对话框选择的，是直接指定数组，则还要检验不能重名

                        // 开始上传,传入文件名数组，传入对应的上传结果控件，传入回调，传入角色编号
                        FormSC FSC = new FormSC(fName, lv, new delegateForSC(UpLoadSucceed), "结算账户开通");
                        FSC.ShowDialog();

                    }
                }



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
    
        }


        //根据上传数据处理查看和删除按钮
        private void timer_Scck_Tick(object sender, EventArgs e)
        {      
                //根据列表中是否已存数据确定显示"其他资质文件查看，删除"按钮与否

                if (listVqtzzwj != null && listVqtzzwj.Items.Count > 0)
                {
                    labsc_ckqtzz.Visible = true;
                    labsc_scqtzz.Visible = true;
                }
                else
                {
                    labsc_ckqtzz.Visible = false;
                    labsc_scqtzz.Visible = false;
                }

                //根据列表中是否已存数据确定显示"营业执照查看·删除"按钮与否
                if (listvyyzz != null && listvyyzz.Items.Count > 0)
                {
                    labelsc_yyzzck.Visible = true;
                    label1sc_yyzzsc.Visible = true;
                }
                else
                {
                    labelsc_yyzzck.Visible = false;
                    label1sc_yyzzsc.Visible = false;
                }

                //根据列表中是否已存数据确定显示"组织机构代码查看·删除"按钮与否

                if (listVzzjgdmz != null && listVzzjgdmz.Items.Count > 0)
                {
                    labelsc_zzjgdmck.Visible = true;
                    labelsc_xxjgdmsc.Visible = true;
                }
                else
                {
                    labelsc_zzjgdmck.Visible = false;
                    labelsc_xxjgdmsc.Visible = false;
                }

                //根据列表中是否已存数据确定显示"税务登记证查看·删除"按钮与否
                if (listVswdjz != null && listVswdjz.Items.Count > 0)
                {
                    label_sedjzck.Visible = true;
                    label_sedjzsc.Visible = true;
                }
                else
                {
                    label_sedjzck.Visible = false;
                    label_sedjzsc.Visible = false;
                }

                //开户许可证
                if (listVkhxkz != null && listVkhxkz.Items.Count > 0)
                {
                    labelsc_khxkzck.Visible = true;
                    label_khxkzsc.Visible = true;
                }
                else
                {
                    labelsc_khxkzck.Visible = false;
                    label_khxkzsc.Visible = false;
                }

                //签字承诺书
                if (listVqzcns != null && listVqzcns.Items.Count > 0)
                {
                    label_qzcnsck.Visible = true;
                    label_qzcnssc.Visible = true;
                }
                else
                {
                    label_qzcnsck.Visible = false;
                    label_qzcnssc.Visible = false;
                }
            //根据列表中是否已存数据确定显示"身份证复印件查看删除"按钮与否
            if (listvsfzfyj.Items.Count > 0)
            {
                B_SCCK.Visible = true;
                B_SCSC.Visible = true;
            }
            else
            {
                B_SCCK.Visible = false;
                B_SCSC.Visible = false;
            }
        }

        /// <summary>
        /// 上传文件查看事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void B_SCCK_Click(object sender, EventArgs e)
        {
            //查看按钮
            Label lb = sender as Label;
            string StrID = lb.Name;
            if (StrID != "")
            {
                ListView lv = null;
                switch (StrID)
                {
                    case "labsc_ckqtzz":
                        lv = listVqtzzwj;
                        break;
                    case "B_SCCK":
                        lv = listvsfzfyj;
                        break;
                    case "labelsc_yyzzck":
                        lv = listvyyzz;
                        break;
                    case "labelsc_zzjgdmck":
                        lv = listVzzjgdmz;
                        break;
                    case "label_sedjzck":
                        lv = listVswdjz;
                        break;
                    case "labelsc_khxkzck":
                        lv = listVkhxkz;
                        break;
                    case "label_qzcnsck":
                        lv = listVqzcns;
                        break;
                    default:
                        break;

                }

                if (lv != null)
                {
                    //有地址
                    if (lv.Items.Count > 0 && lv.Items[0].SubItems[1].Text.Trim() != "")
                    {
                        string url = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[1] + "/JHJXPT/SaveDir/" + lv.Items[0].SubItems[1].Text.Trim().Replace(@"\", "/");
                        StringOP.OpenUrl(url);
                    }

                }
            }
        }
        
        /// <summary>
        /// 删除图片按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void B_SCSC_Click(object sender, EventArgs e)
        {          
            //开启上传
            //根据控件ID判断数据存储到那个listView
            Label lb = sender as Label;
            string StrID = lb.Name;
            string deletetype = "";
            if (StrID != "")
            {
                ListView lv = null;
                switch (StrID)
                {
                    case "labsc_scqtzz":
                        lv = listVqtzzwj;
                        deletetype = "其他资质文件";
                        break;
                    case "B_SCSC":
                        lv = listvsfzfyj;
                        deletetype = "身份证扫描件";
                        break;
                    case "label1sc_yyzzsc":
                        lv = listvyyzz;
                        deletetype = "营业执照文件扫描件";
                        break;
                    case "labelsc_xxjgdmsc":
                        lv = listVzzjgdmz;
                        deletetype = "组织机构代码证扫描件";
                        break;
                    case "label_sedjzsc":
                        lv = listVswdjz;
                        deletetype = "税务登记证扫描件";
                        break;
                    case "label_khxkzsc":
                        lv = listVkhxkz;
                        deletetype = "开户许可证扫描件";
                        break;
                    case "label_qzcnssc":
                        deletetype = "签字承诺书扫描件";
                        lv = listVqzcns;
                        break;
                    default:
                        break;
                }
                if (lv != null)
                {
                    ArrayList Almsg3 = new ArrayList();
                    Almsg3.Add("");
                    Almsg3.Add("您确定要删除" + deletetype+"吗？");
                    FormAlertMessage FRSE3 = new FormAlertMessage("确定取消", "问号", "中国商品批发交易平台", Almsg3);
                    DialogResult db =  FRSE3.ShowDialog();
                    //FRSE3.
                    if (db == DialogResult.Yes)
                    {
                        lv.Items.Clear();
                    }
                }
            }
        }

        /// <summary>
        /// 取消按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void basicButton1_Click_1(object sender, EventArgs e)
        {
            Program.fms = new Formjhjxjszhzc(HTuser);
            Program.fms.Show();
            this.Dispose();
        }


        /// <summary>
        /// 角色选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox_xzjs_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ucTextBox_jjrbh.Text = "";
            this.label_jjrxm.Text = "";
            if (this.comboBox_xzjs.Text.Trim().Equals("经纪人账户"))
            {
                label_l.Visible = false;
                ucTextBox_jjrbh.Enabled = false;
                // 当点击文本框，清空文本框内容         
                ucTextBox_jjrbh.Text = "此栏无需填写";
                // 恢复文本框字体颜色黑色          
                ucTextBox_jjrbh.ForeColor = Color.Gray;
                // 恢复文本框字体为标准       
                //ucTextBox_lxrxm.Font = new Font(this.Font, FontStyle.Regular);

                //恢复买家窗体变化
                label14.Visible = true;
                comboBox_zclb.SelectedIndex = 0;
                comboBox_zclb.Enabled = true;
                label1_zclbts.ForeColor = Color.Red;
                label1_zclbts.Text = "";
                label1_zclbts.Visible = false;

                //显示身份证录入
                this.panelsfzinfo.Visible = false;
                this.panelqyinfo.Visible = false;
                this.panelqtzz.Visible = false;

                this.paneltijiao.Location = new Point(0,334);

                this.panel_login.Height = this.Pan_log - this.panelsfzinfo.Height - this.panelqyinfo.Height - this.panelqtzz.Height;
                this.Height = this.Formh - this.panelsfzinfo.Height - this.panelqyinfo.Height - this.panelqtzz.Height; ;


                //控制是否必填
                this.label11.Visible = true;
                this.label1.Visible = true;
                this.label2.Visible = true;
                this.label12.Visible = true;
                this.label13.Visible = true;
                this.label17.Visible = true;
                this.label18.Visible = true;
               
                

            }
            else if (this.comboBox_xzjs.Text.Trim().Equals("买家账户"))
            {
                comboBox_zclb.SelectedIndex = 1;
                label14.Visible = false;
                comboBox_zclb.Enabled = false;
                label1_zclbts.ForeColor = Color.Black;
                label1_zclbts.Text = "注册买家角色此栏无需填写";
                label1_zclbts.Visible = true;

                //恢复经济人窗体变化
                label_l.Visible = true;
                ucTextBox_jjrbh.Enabled = true;
                // 当点击文本框，清空文本框内容         
                ucTextBox_jjrbh.Text = "";
                // 恢复文本框字体颜色黑色          
                ucTextBox_jjrbh.ForeColor = Color.Black;

                //显示身份证录入
                this.panelsfzinfo.Visible = true;
                this.panelsfzinfo.Location = new Point(0,334);
                this.panelqyinfo.Visible = true;
                this.panelqyinfo.Location = new Point(0,405);
                this.panelqtzz.Visible = true;
                this.panelqtzz.Location = new Point(0,651);
                this.paneltijiao.Location = new Point(0, SaveH);

                this.panel_login.Height = this.Pan_log;
                this.Height = this.Formh ; 

                //控制是否必填
                this.label11.Visible = false;
                this.label1.Visible = false;
                this.label2.Visible = false;
                this.label12.Visible = false;
                this.label13.Visible = false;
                this.label17.Visible = false;
                this.label18.Visible = false;


            }
            else if (this.comboBox_xzjs.Text.Trim().Equals("卖家账户"))
            {
              //必须是企业类型
                comboBox_zclb.SelectedIndex = 2;
                comboBox_zclb.Enabled = false;
                label1_zclbts.ForeColor = Color.Black;
                label1_zclbts.Text = "卖家角色注册类型必须为企业";
                label1_zclbts.Visible = true;

                //恢复经济人窗体变化
                label_l.Visible = true;
                ucTextBox_jjrbh.Enabled = true;
                // 当点击文本框，清空文本框内容         
                ucTextBox_jjrbh.Text = "";
                // 恢复文本框字体颜色黑色          
                ucTextBox_jjrbh.ForeColor = Color.Black;

                //显示企业信息同事隐藏身份信息
               
                this.panelsfzinfo.Visible = false;
               
                this.panelqyinfo.Visible = true;
                this.panelqyinfo.Location = new Point(0, qyinfoH - this.panelsfzinfo.Height);
                this.panelqtzz.Visible = true;
                this.panelqtzz.Location = new Point(0, quziK - this.panelsfzinfo.Height);
                this.paneltijiao.Location = new Point(0, SaveH - this.panelsfzinfo.Height);

                this.panel_login.Height = this.Pan_log - this.panelsfzinfo.Height;
                this.Height = this.Formh - this.panelsfzinfo.Height;


                //控制是否必填
                this.label11.Visible = true;
                this.label1.Visible = true;
                this.label2.Visible = true;
                this.label12.Visible = true;
                this.label13.Visible = true;
                this.label17.Visible = true;
                this.label18.Visible = true;
            }
            else
            {
                //恢复经济人窗体变化
                label_l.Visible = true;
                ucTextBox_jjrbh.Enabled = true;
                // 当点击文本框，清空文本框内容         
                ucTextBox_jjrbh.Text = "";
                // 恢复文本框字体颜色黑色          
                ucTextBox_jjrbh.ForeColor = Color.Black;


                //恢复买家窗体变化
                label14.Visible = true;
                comboBox_zclb.Enabled = true;
                label1_zclbts.ForeColor = Color.Red;
                label1_zclbts.Text = "";
                label1_zclbts.Visible = false;


                this.panelsfzinfo.Visible = false;
                this.panelqyinfo.Visible = false;
                this.panelqtzz.Visible = false;

                this.paneltijiao.Location = new Point(0, 334);

                this.panel_login.Height = this.Pan_log - this.panelsfzinfo.Height - this.panelqyinfo.Height - this.panelqtzz.Height;
                this.Height = this.Formh - this.panelsfzinfo.Height - this.panelqyinfo.Height - this.panelqtzz.Height; ;

                //控制是否必填
                this.label11.Visible = true;
                this.label1.Visible = true;
                this.label2.Visible = true;
                this.label12.Visible = true;
                this.label13.Visible = true;
                this.label17.Visible = true;
                this.label18.Visible = true;

            }

        }


        /// <summary>
        /// 根据类型不同判断上传附件类型不同
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox_zclb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_zclb.SelectedIndex == 1)
            {
                //显示身份证录入
                this.panelsfzinfo.Visible = true;
                this.panelqyinfo.Visible = false;
                this.panelqtzz.Visible = false;
                this.paneltijiao.Location = new Point(0, SaveH - this.panelqyinfo.Height - this.panelqtzz.Height);

                this.panel_login.Height = this.Pan_log - this.panelqyinfo.Height - this.panelqtzz.Height;
                this.Height = this.Formh - this.panelqyinfo.Height - this.panelqtzz.Height; 
                
                //  this.tableLayoutPanel1.GetRow(panelUC2)
            }
            else if (comboBox_zclb.SelectedIndex == 2)
            {
                //显示企业信息同事隐藏身份信息

                this.panelsfzinfo.Visible = false;

                this.panelqyinfo.Visible = true;
                this.panelqyinfo.Location = new Point(0, qyinfoH - this.panelsfzinfo.Height);
                this.panelqtzz.Visible = true;
                this.panelqtzz.Location = new Point(0, quziK - this.panelsfzinfo.Height);
                this.paneltijiao.Location = new Point(0, SaveH - this.panelsfzinfo.Height);

                this.panel_login.Height = this.Pan_log - this.panelsfzinfo.Height;
                this.Height = this.Formh - this.panelsfzinfo.Height; 
            }
            else
            {

                //显示身份证录入
                this.panelsfzinfo.Visible = false;
                this.panelqyinfo.Visible = false;
                this.panelqtzz.Visible = false;

                this.paneltijiao.Location = new Point(0, 334);

                this.panel_login.Height = this.Pan_log - this.panelsfzinfo.Height - this.panelqyinfo.Height - this.panelqtzz.Height;
                this.Height = this.Formh - this.panelsfzinfo.Height - this.panelqyinfo.Height - this.panelqtzz.Height; ;
            }
        }


        //获取手机验证码
        private void label_sjhts_Click(object sender, EventArgs e)
        {
            ArrayList Almsg3 = new ArrayList();
            Almsg3.Add("");
            Almsg3.Add("暂未开通！");
            FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "叹号", "中国商品批发交易平台", Almsg3);
            FRSE3.ShowDialog();
        }

        private void ucTextBox_jjrbh_KeyUp(object sender, KeyEventArgs e)
        {
           // ArrayList lv = new ArrayList();
            if (ucTextBox_jjrbh.Enabled)
            {
                if (!ucTextBox_jjrbh.Text.Trim().Equals(""))
                {
                    Hashtable htjjryhm = new Hashtable();
                    htjjryhm["tszfcl"] = ucTextBox_jjrbh.Text.Trim();
                    if (ValStr.ValidateQuery(htjjryhm))
                    {
                        label_jjrxm.Text = "请输入可用的经纪人用户名";
                        label_jjrxm.ForeColor = System.Drawing.Color.Red;
                        return;
                    }

                    WebServicesCenter ws = new WebServicesCenter();
                    DataSet ds = ws.Getjjrjinfo(ucTextBox_jjrbh.Text.Trim());
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {

                        DataRow dr = ds.Tables[0].Rows[0];

                        //if (!dr["开通审核状态"].ToString().Trim().Equals("审核通过"))
                        //{
                        //    //lv.Add("");
                        //    //lv.Add("经纪人账号尚未通过审核，暂时无法使用！");
                        //    label_jjrxm.Text = "请输入可用的经纪人用户名";
                        //    label_jjrxm.ForeColor = System.Drawing.Color.Red;
                        //    return;
                        //}

                        if (!dr["分公司开通审核状态"].ToString().Trim().Equals("审核通过"))
                        {
                            //lv.Add("");
                            //lv.Add("经纪人账号尚未通过分公司审核，暂时无法使用！");

                            label_jjrxm.Text = "请输入可用的经纪人用户名";
                            label_jjrxm.ForeColor = System.Drawing.Color.Red;
                            return;
                        }
                        if (!dr["角色类型"].ToString().Trim().Equals("经纪人"))
                        {
                            label_jjrxm.Text = "请输入可用的经纪人用户名";
                            label_jjrxm.ForeColor = System.Drawing.Color.Red;
                            return;
                        }

                        if (!dr["代理人暂停接受新用户"].ToString().Trim().Equals("否"))
                        {
                            label_jjrxm.Text = "请输入可用的经纪人用户名";
                            label_jjrxm.ForeColor = System.Drawing.Color.Red;
                            return;
                        }
                        if (!dr["是否允许登陆"].ToString().Trim().Equals("是"))
                        {
                            label_jjrxm.Text = "请输入可用的经纪人用户名";
                            label_jjrxm.ForeColor = System.Drawing.Color.Red;
                            return;
                        }
                        if (dr["是否冻结账号"].ToString().Trim().Equals("是"))
                        {
                            label_jjrxm.Text = "请输入可用的经纪人用户名";
                            label_jjrxm.ForeColor = System.Drawing.Color.Red;
                            return;
                        }
                        if (dr["是否休眠"].ToString().Trim().Equals("是"))
                        {
                            label_jjrxm.Text = "请输入可用的经纪人用户名";
                            label_jjrxm.ForeColor = System.Drawing.Color.Red;
                            return;
                        }
                        label_jjrxm.Text = dr["角色编号"].ToString();
                        label_jjrxm.ForeColor = System.Drawing.Color.Black;
                    }
                    else
                    {
                        label_jjrxm.Text = "请输入可用的经纪人用户名";
                        label_jjrxm.ForeColor = System.Drawing.Color.Red;
                        return;
                        //return;
                    }
                }
                else
                {
                    label_jjrxm.Text = "请输入可用的经纪人用户名";
                    label_jjrxm.ForeColor = System.Drawing.Color.Red;
                    return;
                }
            }

        }

        private void linkLabel_cnsdown_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //C# cs程序 从服务器下载文件

            //string url = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[1] + "/JHJXPT/jjrzzhucexieyi.aspx" ;
            //StringOP.OpenUrl(url);

            ArrayList Almsg3 = new ArrayList();
            Almsg3.Add("");
            Almsg3.Add("暂时没有文件可下载！");
            FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "叹号", "承诺书提示", Almsg3);
            FRSE3.ShowDialog();
        }

        private void linkLabel_xy_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (comboBox_xzjs.SelectedIndex != 0)
            {
                FormZCXY fz = new FormZCXY(comboBox_xzjs.Text);
                fz.Text = comboBox_xzjs.Text.Replace("账户","").Trim()+"注册协议";
                fz.ShowDialog();
            }
            else
            {
                ArrayList Almsg3 = new ArrayList();
                Almsg3.Add("");
                Almsg3.Add("您还没有选择结算账户类型，暂时无法查看协议内容！");
                FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "叹号", "协议提示", Almsg3);
                FRSE3.ShowDialog();
            }
               
        }


    }
}
