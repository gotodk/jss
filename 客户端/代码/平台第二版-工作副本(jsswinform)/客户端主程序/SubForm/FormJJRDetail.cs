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
using System.Data;
using System.Threading;
using 客户端主程序.Support;
using 客户端主程序.DataControl;
using Com.Seezt.Skins;
using 客户端主程序.SubForm.CenterForm.publicUC;


namespace 客户端主程序.SubForm
{
    public partial class FormJJRDetail : BasicForm
    {
        //淡出计时器
        DataSet DS = null;
        System.Windows.Forms.Timer Timer_DC;
        public FormJJRDetail(DataSet ds)
        {
            Init_one_show();
            InitializeComponent();
            DS = ds;
            //图标
            Icon ic = new Icon(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase+ @"\skin\ji.ico");
            this.Icon = ic;
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

        private void FormJJRDetail_Load(object sender, EventArgs e)
        {
            //经纪人用户名
            lbjjryhm.Text = DS.Tables[0].Rows[0]["YHM"].ToString();
            //经纪人邮箱
            lbjjryx.Text = DS.Tables[0].Rows[0]["DLYX"].ToString();
            //经纪人地址
            lbjjrdz.Text = DS.Tables[0].Rows[0]["SSSF"].ToString() + DS.Tables[0].Rows[0]["SSDS"].ToString();
            //注册类型
            lbzclx.Text = DS.Tables[0].Rows[0]["ZCLX"].ToString();
            //联系人姓名
            lblxrxm.Text = DS.Tables[0].Rows[0]["LXRXM"].ToString();
            //手机号
            lbsjhm.Text = DS.Tables[0].Rows[0]["SJH"].ToString();
            //是否接新业务
            lbsfjxyw.Text = DS.Tables[0].Rows[0]["SFZTXYW"].ToString().Trim()=="是"?"否":"是";
            //是否默认经纪人
            lbsfmrjjr.Text = DS.Tables[0].Rows[0]["SFMRDLR"].ToString();
            //关联时间
            lbglsj.Text = DS.Tables[0].Rows[0]["SQGLSJ"].ToString();
            //关联状态
            lbglzt.Text = DS.Tables[0].Rows[0]["GLZT"].ToString();
            //代理人审核时间
            lbshsj.Text = DS.Tables[0].Rows[0]["DLRSHSJ"].ToString();
            //代理人审核状态
            lbshzt.Text = DS.Tables[0].Rows[0]["DLRSHZT"].ToString();
            //代理人审核意见
            lbshyj.Text = DS.Tables[0].Rows[0]["DLRSHYJ"].ToString();
            //分公司审核状态
            lblFGSSHZT.Text = DS.Tables[0].Rows[0]["FGSSHZT"].ToString();
            //分公司审核时间
            lblFGSSHSJ.Text = DS.Tables[0].Rows[0]["FGSSHSJ"].ToString();
            //分公司审核意见
            lblFGSSHYJ.Text = DS.Tables[0].Rows[0]["FGSSHYJ"].ToString();
            if (DS.Tables[0].Rows[0]["DLRSHZT"].ToString() == "驳回" || DS.Tables[0].Rows[0]["FGSSHZT"].ToString() == "驳回")
            {
                btnPass.Visible = true;
            }
            else
            {
                btnPass.Visible = false;
            }
        }

        private void btnPass_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();

            DataTable auto2 = new DataTable();
            auto2.TableName = "更新关联表";
            auto2.Columns.Add("登录邮箱");
            auto2.Columns.Add("经纪人编号");
            ds.Tables.Add(auto2);
            ds.Tables[0].Rows.Add(new string[] { PublicDS.PublisDsUser.Tables[0].Rows[0]["DLYX"].ToString(),DS.Tables[0].Rows[0]["JSBH"].ToString() });

            //禁用提交区域并开启进度
            panel1.Enabled = false;
            PBload.Visible = true;
            resetloadLocation(btnPass, PBload);
            //开启线程提交数据
            delegateForThread dft = new delegateForThread(ShowThreadResult_save);
            DataControl.RunThreadClassUpdateJJR RTCTSOU = new DataControl.RunThreadClassUpdateJJR(ds, dft, "jjr_Resubmit");
            Thread trd = new Thread(new ThreadStart(RTCTSOU.BeginRun));
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
            //重新开放提交区域
            panel1.Enabled = true;
            PBload.Visible = false;
            //显示执行结果
            ResultInvoke(returnHT);

        }
        //处理返回的结果
        private void ResultInvoke(Hashtable returnHT)
        {
            
            DataSet state = (DataSet)returnHT["执行状态"];
            switch (state.Tables["返回值单条"].Rows[0]["执行结果"].ToString())
            {
                case "ok":

                    //给出表单提交成功的提示
                    ArrayList Almsg3 = new ArrayList();
                    Almsg3.Add("");
                    Almsg3.Add(state.Tables["返回值单条"].Rows[0]["错误提示"].ToString());
                    FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "叹号", "处理成功", Almsg3);
                    FRSE3.ShowDialog();
                    //this.Close();
                    //if (FRSE3.ShowDialog() == DialogResult.Yes)
                    //{
                    //    btnPass.Visible = false;
                    //    ////重置本表单，或转向其他页面
                    ////GLjingjiren UC = new GLjingjiren();//这句话是关键，若实例化本身的类，则是重置表单，若实例化其他的类，则是跳转
                    ////reset(UC);
                    //    this.Close();
                    //}
                    
                    break;
                case "err":
                    ArrayList Almsg7 = new ArrayList();
                    Almsg7.Add("");
                    Almsg7.Add(state.Tables["返回值单条"].Rows[0]["错误提示"].ToString());
                    FormAlertMessage FRSE7 = new FormAlertMessage("仅确定", "错误", "添加或设置未成功", Almsg7);
                    FRSE7.ShowDialog();
                    break;
                case "success":
                    break;
                default:
                    ArrayList Almsg4 = new ArrayList();
                    Almsg4.Add("");
                    Almsg4.Add("抱歉，系统繁忙，请稍后再试……");// 这里是提交失败的提示，可能是程序出错，可能是条件不足等
                    FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "错误", "系统忙", Almsg4);
                    FRSE4.ShowDialog();

                    break;
            }
            panel1.Enabled = true;
            btnPass.Visible = false;
            PBload.Visible = false;
        }
        /// <summary>
        /// 重设进度条位置，放到按钮旁边
        /// </summary>
        /// <param name="BB">参考提交按钮</param>
        /// <param name="PB">要移动的进度条</param>
        private void resetloadLocation(BasicButton BB, PictureBox PB)
        {
            PB.Location = new Point(BB.Location.X + BB.Width + 30, BB.Location.Y);
            PB.Visible = true;
        }
        /// <summary>
        /// 重置表单
        /// </summary>
        /// <param name="UC"></param>
        private void reset(GLjingjiren UC)
        {
            UC.Dock = DockStyle.Fill;//铺满
            UC.AutoScroll = true;//出现滚动条
            UC.BackColor = Color.AliceBlue;
            UC.Padding = new Padding(10);
            //清理控件
            Panel P = (Panel)this.Parent;
            P.Controls.Clear();
            P.Controls.Add(UC); //加入到某一个panel中
            UC.Show();//显示出来

            //更改标题(如果重置本表单，不需要执行这句话)
            //this.ParentForm.Text = "xxx提交成功";
        }
    }
}
