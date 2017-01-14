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

namespace 客户端主程序
{
    public partial class FormCenter : BasicForm
    {
        //淡出计时器
        System.Windows.Forms.Timer Timer_DC;

        /// <summary>
        /// 程序集
        /// </summary>
        Assembly asm;
        /// <summary>
        /// 类型
        /// </summary>
        Type type;
        /// <summary>
        /// 激活器
        /// </summary>
        object instance;



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

        public FormCenter()
        {
            //=======================所有窗体都有这个玩意=========
            Init_one_show();
            //====================================================

            InitializeComponent();

            //设置最小化大小
            this.MaximizedBounds = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea;
            this.BackColor = Color.Black;
            this.Width = (int)(System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width*0.8) ;
            this.Height = (int)(System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height * 0.8);

            Icon ic = new Icon(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase+ @"\skin\ji.ico");
            this.Icon = ic;

            
        }



        /// <summary>
        /// 在控件中加载控件
        /// </summary>
        /// <param name="frmTypeName">类型名称</param>
        private void opennewform(string frmTypeName,TreeNode treenode)
        {
            this.SuspendLayout();
            try
            {
                //不能为空值
                if (frmTypeName == "")
                {
                    return;
                }
                //根据数据库动态生成类实例
                asm = Assembly.GetExecutingAssembly();
                type = asm.GetType(frmTypeName, false);
                //若找不到类型，则不再进行操作
                if (type == null)
                {
                    return;
                }
                instance = asm.CreateInstance(frmTypeName, false, BindingFlags.Default, null, new object[] { }, null, null);
                UserControl UC = (UserControl)instance;
                UC.Dock = DockStyle.Fill;//铺满
                UC.AutoScroll = true;//出现滚动条
                UC.BackColor = Color.AliceBlue;
                UC.Padding = new Padding(10);


                //清理控件
                splitContainer1.Panel2.Controls.Clear();
                splitContainer1.Panel2.Controls.Add(UC); //加入到某一个panel中
                UC.Show();//显示出来

                //更改标题
                this.Text = "我的账户 -> " + treenode.FullPath.Replace(">", " -> ");
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("右侧加载控件调用asm.CreateInstance错误：" + ex.ToString());
            }
            this.ResumeLayout(false);
            //若要执行基类中没有的方法
            //MethodInfo method = type.GetMethod("test_str");
            //string aa =  (string)method.Invoke(instance, null);
        }

        private void FormCenter_Load(object sender, EventArgs e)
        {
            //开启线程提交数据
            delegateForThread dft = new delegateForThread(ShowThreadResult_Menu);
            DataControl.RunThreadClassAdminMenu RTCAM = new DataControl.RunThreadClassAdminMenu(null, dft);
            Thread trd = new Thread(new ThreadStart(RTCAM.BeginRun));
            trd.IsBackground = true;
            trd.Start();

            

        }

        //显示线程处理结果的函数(用于处理线程返回数据)
        private void ShowThreadResult_Menu(Hashtable returnHT)
        {
            try
            {
                //调用异步委托
                Invoke(new delegateForThreadShow(ShowThreadResult_Menu_Invoke), new Hashtable[] { returnHT });
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("委托回调错误：" + ex.ToString());
            }
        }
        //处理非线程创建的控件
        private void ShowThreadResult_Menu_Invoke(Hashtable returnHT)
        {
            //取消进度条
            PBloading.Visible = false;

            //绑定菜单
            DataSet dsmenu = (DataSet)returnHT["后台菜单"];
            InitNode(dsmenu.Tables[0]);
            ZZ_tbMenuAdmin.ExpandAll();

            //遍历节点，找到默认值并选中它
            foreach (TreeNode td in ZZ_tbMenuAdmin.Nodes)
            {
                TreeNode targetNode = SearchNode(td, "默认");
                if (targetNode != null)
                {
                    ZZ_tbMenuAdmin.SelectedNode = targetNode;
                    break;
                }
            }

            //显示菜单
            ZZ_tbMenuAdmin.Visible = true;
        }



        /// <summary>
        /// 遍历TreeView节点，查找并设置默认值
        /// </summary>
        /// <param name="td">ParentNode</param>
        /// <param name="selectParentNum">条件</param>
        /// <returns></returns>
        private TreeNode SearchNode(TreeNode td, string defaultstr)
        {
            if (((DataRow)(td.Tag))["GoToUrlParameter"].ToString() == defaultstr)
            {
                return td;
            }
            TreeNode targetNode = null;
            foreach (TreeNode childNodes in td.Nodes)
            {
                targetNode = SearchNode(childNodes, defaultstr);
                if (targetNode != null)
                    break;
            }
            return targetNode;
        }


        /// <summary>
        /// 初始化节点
        /// </summary>
        /// <param name="dt">要加载成树结构的数据源</param>
        private void InitNode(DataTable dt)
        {
            DataRow[] drRoot0 = dt.Select("1=1");
            if (drRoot0 != null && drRoot0.Length > 0)
            {
                //不同结算账户类型使用不同菜单
                string JSZHLX = PublicDS.PublisDsUser.Tables[0].Rows[0]["JSZHLX"].ToString();
                string JSZHLX_SortID = "";
                if (JSZHLX == "卖家账户")
                {
                    JSZHLX_SortID = "2";
                }
                if (JSZHLX == "买家账户")
                {
                    JSZHLX_SortID = "1";
                }
                if (JSZHLX == "经纪人账户")
                {
                    JSZHLX_SortID = "3";
                }
                DataRow[] drRoot_zi1 = dt.Select("SortParentID='" + JSZHLX_SortID + "' ");
                int allzi = drRoot_zi1.Length;
                for (int t = 0; t < allzi; t++)
                {
                    DataRow drRoot_nowzi = drRoot_zi1[t];
                    //检查菜单显示，隐藏禁止查看的菜单。

                    TreeNode root = new TreeNode();
                    root.Text = drRoot_nowzi["SortName"].ToString();
                    root.ToolTipText = drRoot_nowzi["ToolTipText"].ToString();
                    Hashtable htnodeDB = new Hashtable();
                    root.Tag = drRoot_nowzi;
                    root.Name = drRoot_nowzi["SortID"].ToString();
                    //root.ImageKey = "业务查询";
                    //root.SelectedImageKey = "业务查询";
                    this.ZZ_tbMenuAdmin.Nodes.Add(root);
                    this.BuildChild(drRoot_nowzi, root, dt);


                }

            }

        }

        /// <summary>
        /// 加载子节点
        /// </summary>
        /// <param name="dr">父节点对应的行</param>
        /// <param name="root">父节点</param>
        /// <param name="dt">要加载成树结构的数据源</param>
        private void BuildChild(DataRow dr, TreeNode root, DataTable dt)
        {
            if (dr == null || root == null) return;
            DataRow[] drChilds = dt.Select("SortParentID='" + dr["SortID"] + "'");
            if (drChilds != null || drChilds.Length > 0)
            {
                foreach (DataRow drChild in drChilds)
                {

                    //检查菜单显示，隐藏禁止查看的菜单。

                    TreeNode node = new TreeNode();
                    node.Text = drChild["SortName"].ToString();
                    node.ToolTipText = drChild["ToolTipText"].ToString();
                    Hashtable htnodeDB = new Hashtable();
                    node.Tag = drChild;
                    node.Name = drChild["SortID"].ToString();
                    //node.ImageKey = "功能";
                    //node.SelectedImageKey = "功能";
                    root.Nodes.Add(node);
                    this.BuildChild(drChild, node, dt);


                }
            }
        }

        private void ZZ_tbMenuAdmin_AfterSelect(object sender, TreeViewEventArgs e)
        {


            //根据点击的节点，打开功能
            DataRow dr = (DataRow)e.Node.Tag;
            opennewform(dr["GoToUrl"].ToString(), e.Node);
        }



        private void FormCenter_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }






    }
}
