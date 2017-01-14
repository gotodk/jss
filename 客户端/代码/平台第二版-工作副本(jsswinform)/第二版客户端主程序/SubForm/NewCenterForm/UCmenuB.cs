using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace 客户端主程序.SubForm.NewCenterForm
{
    public partial class UCmenuB : UserControl
    {

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


        private Center2013 _Center2013;
        /// <summary>
        /// 父窗体
        /// </summary>
        [Description("设置或读取当前菜单的父窗体"), Category("Appearance")]
        public Center2013 Center2013CC
        {
            get
            {
                return _Center2013;
            }
            set
            {
                _Center2013 = value;
            }
        }





        private string _Menus = "";
        /// <summary>
        /// 菜单参数
        /// </summary>
        [Description("设置或读取当前菜单的配置格式：[B区]文字☆控件●文字☆控件●文字☆控件"), Category("Appearance")]
        public string Menus
        {
            get
            {
                return _Menus;
            }
            set
            {
                if (value != null)
                {
                    
                    //清理菜单
                    this.Controls.Clear();
                    //获取菜单类型，哪个区
                    string caozuoqu = value.Substring(0,4);
                    //加载配置
                    string[] menuARR = value.Replace("[B区]", "").Replace("[C区]", "").Split('●');
                    Graphics g = this.CreateGraphics();
                    int kuangdu = 0;
                    for (int i = menuARR.Length-1; i >= 0; i--)
                    {
                        //添加菜单项目
                        string[] menuoneARR = menuARR[i].Split('☆');
                        Label Lnew = new Label();
                        Lnew.BackColor = System.Drawing.Color.AliceBlue;
                        Lnew.Cursor = System.Windows.Forms.Cursors.Hand;
                        Lnew.Dock = System.Windows.Forms.DockStyle.Left;
                        Lnew.ForeColor = System.Drawing.Color.Black; ;
                        Lnew.Margin = new System.Windows.Forms.Padding(0);
                        Lnew.Name = "menuB_label_" + i.ToString();
                        Lnew.Text = menuoneARR[0];
                        SizeF sizeF = g.MeasureString(menuoneARR[0], new Font("Tahoma", 9));
                        Lnew.Width = Convert.ToInt32(Math.Round(sizeF.Width, 0)) + 4;
                        kuangdu = kuangdu + Lnew.Width;
                        Lnew.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                        Lnew.Tag = caozuoqu + menuoneARR[1];
                        this.Controls.Add(Lnew);

                        //把第一个按钮搞成默认
                        if (i == 0)
                        {
                            Lnew.BackColor = Color.SteelBlue;
                            Lnew.ForeColor = Color.White;

                            ShowGNQ(caozuoqu, Lnew.Tag.ToString());
                        }


                    }

                    if (caozuoqu == "[B区]")
                    {
                        //加载
                        _Center2013.uCmenuBBB.Width = kuangdu + 7;
                    }
                    if (caozuoqu == "[C区]")
                    {
                        //加载
                        _Center2013.uCmenuCCC.Width = kuangdu + 7;
                    }

                    g.Dispose();

                    

                    //给按钮绑定事件
                    foreach (Control control in this.Controls)
                    {
                        if (control is Label)
                        {
                            Label LL = (Label)control;
                            LL.Click += new System.EventHandler(this.label_Click);
                            LL.MouseEnter += new System.EventHandler(this.label_MouseEnter);
                            LL.MouseLeave += new System.EventHandler(this.label_MouseLeave);
                        }
                    }

        

                    
                }
                _Menus = value;
                //重画
                this.Refresh();
            }
        } 


        public UCmenuB()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 在功能区加载具体功能控件
        /// </summary>
        /// <param name="caozuoqu"></param>
        /// <param name="Tagstr"></param>
        private void ShowGNQ(string caozuoqu, string Tagstr)
        {
            string frmTypeName = Tagstr.Replace("[B区]", "").Replace("[C区]", "");
            string dllstr = "";
            if (Tagstr.IndexOf(")!") >= 0)
            {
                dllstr = Tagstr.Replace("[B区]", "").Replace("[C区]", "").Substring(0, Tagstr.Replace("[B区]", "").Replace("[C区]", "").IndexOf(")!"));
                frmTypeName = Tagstr.Substring(Tagstr.IndexOf(")!") + ")!".Length, Tagstr.Length - (Tagstr.IndexOf(")!") + ")!".Length)); 
            }
            //加载配置
            //string[] dllstrAy = Tagstr.Split(new string[] { ")!" }, StringSplitOptions.RemoveEmptyEntries);
            //string frmTypeName="";
            //if (dllstrAy.Length == 1)
            //{
            //    frmTypeName = Tagstr.Replace("[B区]", "").Replace("[C区]", "");
            //    asm = Assembly.GetExecutingAssembly();
            //}
            //else
            //{
            //    asm = Assembly.LoadFile(Application.StartupPath + "\\Business_SPMM.dll");
            //    frmTypeName = dllstrAy[1].Replace("[B区]", "").Replace("[C区]", "");
            //}
            Assembly asms = Assembly.GetExecutingAssembly();
            //不能为空值
            if (frmTypeName == "")
            {
                return;
            }
            //根据数据库动态生成类实例
            if (dllstr == "" || dllstr.Trim() == asms.FullName.Split(',')[0] + ".dll")
            {
                asm = Assembly.GetExecutingAssembly();
            }
            else
            {
                asm = Assembly.LoadFile(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\" + dllstr);
            }
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
            if (caozuoqu == "[B区]")
            {
                //加载
                _Center2013.panelUCBBB.Padding = new System.Windows.Forms.Padding(1);
                _Center2013.panelUCBBB.Controls.Clear();
                _Center2013.panelUCBBB.Controls.Add(UC); //加入到某一个panel中
                UC.Show();//显示出来
            }
            if (caozuoqu == "[C区]")
            {
                //加载
                _Center2013.panelUCCCC.Padding = new System.Windows.Forms.Padding(1);
                _Center2013.panelUCCCC.Controls.Clear();
                _Center2013.panelUCCCC.Controls.Add(UC); //加入到某一个panel中
                UC.Show();//显示出来
            }
        }

        private void label_Click(object sender, EventArgs e)
        {
            //遍历按钮，都搞成初始的样子
            foreach (Control control in this.Controls)
            {
                if (control is Label)
                {
                    Label LL = (Label)control;
                    LL.BackColor = Color.AliceBlue;
                    LL.ForeColor = Color.Black;
                }
            }
            //把被点击的按钮特殊处理
            Label L = (Label)sender;
            L.BackColor = Color.SteelBlue;
            L.ForeColor = Color.White;

            //加载功能区
            //获取菜单类型，哪个区
            string Tagstr = L.Tag.ToString();
            string caozuoqu = Tagstr.Substring(0, 4);

            ShowGNQ(caozuoqu, Tagstr);
            

        }

        private void label_MouseEnter(object sender, EventArgs e)
        {
            Label L = (Label)sender;
            L.ForeColor = Color.Red;
        }

        private void label_MouseLeave(object sender, EventArgs e)
        {
            Label L = (Label)sender;
            if (L.BackColor == Color.SteelBlue)
            {
                L.ForeColor = Color.White;
            }
            else
            {
                L.ForeColor = Color.Black;
            }
        }


        private void UCmenuB_Load(object sender, EventArgs e)
        {


            

        }

        private void UCmenuB_Paint(object sender, PaintEventArgs e)
        {
            //横线
            e.Graphics.DrawLine(Pens.LightSkyBlue, 0, 0, this.Width, 0);
            //左竖线
            e.Graphics.DrawLine(Pens.LightSkyBlue, 0, 0, 0, this.Height);
            //右竖线
            e.Graphics.DrawLine(Pens.LightSkyBlue, this.Width-1, 0, this.Width-1, this.Height);

            //分割线
            foreach (Control control in this.Controls)
            {
                if (control is Label)
                {
                    Label LL = (Label)control;
                    e.Graphics.DrawLine(Pens.LightSkyBlue, LL.Location.X + LL.Width - 1, 0, LL.Location.X + LL.Width - 1, this.Height);
                }
            }
        }
    }
}
