using System;
using System.Drawing;
using System.Windows.Forms;
using Com.Seezt.Skins;
using System.Collections;
using 客户端主程序.NewDataControl;
using 客户端主程序.DataControl;
using 客户端主程序.SubForm;
using System.Threading;
using System.Data;

namespace 客户端主程序.SubForm.NewCenterForm
{
    public partial class FormGJSS : BasicForm
    {
      
        //用于所属分类下拉框条件
        protected string strwhere = "";
        //用于无限极下拉框数据源
        DataView dv;
        ////层次分隔符
        //const string STR_TREENODE = "├";
        ////const string STR_TREENODE = "┆┄";
        //顶级父节点
        const int INT_TOPID = 0;
        //分类ID,与一下几个字段同数据库中的字段名称相同，以方便取值
        const string SortID = "SortID";
        //父分类ID
        const string SortParentID = "SortParentID";
        //分类名称
        //const string SortName = "SortName";
        const string SortNameTree = "SortNameTree";
        //淡出计时器
        System.Windows.Forms.Timer Timer_DC;

        /// <summary>
        /// 查询条件
        /// </summary>
        Hashtable ht_where = new Hashtable();

        //商品详情
        //SPXQ spxq;

        protected static DataTable dt;

        //存放数据分类列表
        protected static DataTable dfl;

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

        public FormGJSS(string title)
        {
            //=======================所有窗体都有这个玩意=========
            Init_one_show();
            //设置图标
            Icon ic = new Icon(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\skin\ji.ico");
            this.Icon = ic;
            //====================================================

            InitializeComponent();
            this.TitleYS = new int[] { 0, 0, -50 };

            if (PublicDS.PublisDsSPFL != null && PublicDS.PublisDsSPFL.Tables.Contains("分类表"))
            {
                dv = PublicDS.PublisDsSPFL.Tables["分类表"].DefaultView;

                //去掉重复的行,dfl用于构建SQL语句in条件
                dfl = dv.ToTable(true, new string[] { "SortParentID", "SortID" });
                dv.Sort = SortParentID;
                
                if (dv.Table.Rows.Count > 0)
                {                  
                    DataBind(INT_TOPID);
                }
          
            }
            else
            {
                //如果分类表不存在，重新获取
                SRT_demo_Run();
            }

            this.Text = title;

            //初始化分页回调(带分页的页面，先把这个放上)
            ucPager1.DFT = new delegateForThread(STR_BeginBind);
        }
        #region 不再使用
        /// <summary>
        /// 递归绑定DropDownList
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="schar"></param> 
        //protected void RecursBind(int pid, string schar)
        //{
        //    DataRowView[] rows = dv.FindRows(pid);

        //    if (pid != 0)
        //    {
        //        schar += STR_TREENODE;
        //    }

        //    foreach (DataRowView row in rows)
        //    {
        //        this.cbSSFL.Items.Add(new ComboBoxItem<int, string>(Convert.ToInt32(row[SortID]), schar + row[SortName].ToString()));

        //        RecursBind(Convert.ToInt32(row[SortID]), schar);

        //    }

        //}
        #endregion
        
        /// <summary>
        /// 这个方法用于排序，将隶属于同一个父类的子类和父类并在一起,不需要再在数据库中调order
        /// </summary>
        /// <param name="pid">父类SortID</param>
        protected void DataBind(int pid)
        {
            DataRowView[] rows = dv.FindRows(pid);

            foreach (DataRowView row in rows)
            {
                this.cbSSFL.Items.Add(new ComboBoxItem<int, string>(Convert.ToInt32(row[SortID]), row[SortNameTree].ToString()));

                DataBind(Convert.ToInt32(row[SortID]));

            }

        }
        /// <summary>
        /// 获取所选分类的子分类，构成一个SQL中in条件字符串
        /// </summary>
        /// <param name="str"></param>
        protected void FindCSorts(string str)
        {

            DataRow[] dtc = dfl.Select("SortParentID='"+str+"'"); 
            if (dtc != null && dtc.Length > 0)
            {
                for (int i = 0; i < dtc.Length; i++)
                {
                    strwhere += "," + dtc[i]["SortID"].ToString();
                    FindCSorts(dtc[i]["SortID"].ToString());
                }

            }
            else
            {
                //如果本身没有子分类，并且不是其他分类的子分类
                if(!(strwhere+",").Contains(","+str+","))
                strwhere += "," + str;
            }

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



        /// <summary>
        /// 设置默认搜索条件(这个例子中的哈希表键值一个也不能少)
        /// </summary>
        private void setDefaultSearch()
        {
            //ht_where["GetCustomersDataPage_NAME"] = "GetCustomersDataPage";  //每页显示的条数(必须设置)
            //ht_where["page_index"] = "0";
            //ht_where["page_size"] = "25";  //每页显示的条数(必须设置)
            //ht_where["serach_Row_str"] = " * "; //检索字段(必须设置)
            //ht_where["search_tbname"] = "  ( select 商品编号+合同期限 as 主键, SortParentID,a.name as 一级分类,  SSFLBH,SortName as 二级分类, 商品编号,合同期限, 商品名称, 型号规格,计价单位,JJPL as 经济批量  from AAA_DaPanPrd_New left join AAA_PTSPXXB on SPBH=商品编号 left join  AAA_tbMenuSPFL on SortID=SSFLBH left join (select SortID as aid,SortName as name  from AAA_tbMenuSPFL ) a on a.aid= SortParentID ) as tab  ";  //检索的表(必须设置)
            //ht_where["search_mainid"] = " 主键 ";  //所检索表的主键(必须设置)
            //ht_where["search_str_where"] = " 1=1 ";  //检索条件(必须设置)
            //ht_where["search_paixu"] = " ASC ";  //排序方式(必须设置)
            //ht_where["search_paixuZD"] = " 商品编号 ";  //用于排序的字段(必须设置)


            ht_where["page_index"] = "0";
            ht_where["page_size"] = "18";  //每页显示的条数(必须设置)
            ht_where["webmethod"] = "大盘高级搜索分页";
            Hashtable ht_tiaojian = new Hashtable();
            //所属分类
            string strid = "0";
            if (cbSSFL.SelectedItem != null && cbSSFL.SelectedItem.ToString() != "所属分类")
            { strid = ((ComboBoxItem<int, string>)cbSSFL.SelectedItem).Key.ToString(); }

            FindCSorts(strid.Trim());
            if (strwhere != "" && strwhere.StartsWith(","))
            {
                strwhere = strwhere.Remove(0, 1);
            }
            ht_tiaojian["所属分类"] = strwhere;
            ht_tiaojian["合同期限"] = cbHTQX.SelectedItem.ToString();
            ht_tiaojian["商品编号"] = txtSPBH.Text.Trim();
            ht_tiaojian["商品名称"] = txtSPMC.Text.Trim();
            ht_tiaojian["型号规格"] = txtGGXH.Text.Trim();
            ht_where["tiaojian"] = ht_tiaojian;
        }
        //显示线程处理结果的函数(用于处理线程返回数据)
        private void STR_BeginBind(Hashtable returnHT)
        {
            try { Invoke(new delegateForThreadShow(STR_BeginBind_Invoke), new Hashtable[] { returnHT }); }
            catch (Exception ex) { Support.StringOP.WriteLog("委托回调错误：" + ex.ToString()); }
        }
        //处理非线程创建的控件(线程获取到分页数据后的具体绑定处理)
        private void STR_BeginBind_Invoke(Hashtable OutPutHT)
        {
            DataSet ds = (DataSet)OutPutHT["数据"];//获取返回的数据集
            int fys = (int)ds.Tables["附加数据"].Rows[0]["分页数"];
            int jls = (int)ds.Tables["附加数据"].Rows[0]["记录数"];
            string msg = (string)ds.Tables["附加数据"].Rows[0]["其他描述"];
            string err = (string)ds.Tables["附加数据"].Rows[0]["执行错误"];

            //是否自动绑定列
            dataGridView1.AutoGenerateColumns = false;


            //若执行正常
            if (ds.Tables.Contains("主要数据"))
            {
                dt = ds.Tables[0];
                dataGridView1.DataSource = ds.Tables[0].DefaultView;

            }
            else
            {
                dataGridView1.DataSource = null;
            }
            //取消列表的自动排序
            for (int i = 0; i < this.dataGridView1.Columns.Count; i++)
            {
                if (i == 0)
                {
                    //让第一列留出一点空白
                    DataGridViewCellStyle dgvcs = new DataGridViewCellStyle();
                    dgvcs.Padding = new Padding(10, 0, 0, 0);
                    this.dataGridView1.Columns[i].HeaderCell.Style = dgvcs;
                    this.dataGridView1.Columns[i].DefaultCellStyle = dgvcs;
                }
                this.dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }


        }

        /// <summary>
        /// 设置条件，获取通过分页控件数据,留空则使用默认条件
        /// </summary>
        /// <param name="HT_Where_temp">留空则使用默认条件</param>
        public void GetData(Hashtable HT_Where_temp)
        {
            if (HT_Where_temp == null)
            {
                setDefaultSearch();
            }
            ucPager1.HT_Where = ht_where;
            ucPager1.BeginBindForUCPager();
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            //清空in字符串
            strwhere = ""; 
            //更改搜索条件
            setDefaultSearch();


            //获取in条件
            //string strid = "0";
            //if (cbSSFL.SelectedItem != null && cbSSFL.SelectedItem.ToString()!="所属分类")
            //{ strid = ((ComboBoxItem<int, string>)cbSSFL.SelectedItem).Key.ToString(); }

            //FindCSorts(strid.Trim());
            //if (strwhere != "" && strwhere.StartsWith(","))
            //{
            //    strwhere = strwhere.Remove(0, 1);
            //}

            //if (cbHTQX.SelectedItem.ToString() != "全部")
            //{

            //    ht_where["search_str_where"] = ht_where["search_str_where"] + "  and 合同期限 like '%" + cbHTQX.SelectedItem.ToString() + "%' and 商品名称 like '%" + txtSPMC.Text.Trim() + "%' and 型号规格 like '%" + txtGGXH.Text.Trim() + "%' and 商品编号 like '%"+txtSPBH.Text.Trim()+"%' and SSFLBH in (" + strwhere + ") ";
            //}
            //else
            //{

            //    ht_where["search_str_where"] = ht_where["search_str_where"] + "  and 商品名称 like '%" + txtSPMC.Text.Trim() + "%' and 型号规格 like '%" + txtGGXH.Text.Trim() + "%' and 商品编号 like '%" + txtSPBH.Text.Trim() + "%' and SSFLBH in (" + strwhere + ") ";
 
            //}

            //执行查询
            GetData(ht_where);
        }
      

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //点击列表中某列中的控件时处理
            string columnName = dataGridView1.Columns[e.ColumnIndex].Name;
            if (columnName == "详情及操作" && e.RowIndex > -1)
            {
                //特殊处理，让被禁用的行不响应操作
                if (dataGridView1.Rows[e.RowIndex].Cells["详情及操作"].Tag != null && dataGridView1.Rows[e.RowIndex].Cells["详情及操作"].Tag.ToString() == "不让操作哦")
                {
                    return;
                }

                string spbh = dataGridView1.Rows[e.RowIndex].Cells["商品编号"].Value.ToString().Trim();
                string htqx = dataGridView1.Rows[e.RowIndex].Cells["合同期限"].Value.ToString().Trim();


                #region//商品详情页面，如果同种商品的详情已经打开，则让他获取焦点，否则打开新页面
                Form spxqll = null;
                foreach (Form f in Application.OpenForms)
                {
                    if (f.Name.ToString() == "SPXQ2014")
                    {
                        spxqll = f;
                        break;

                    }
                }

                if (spxqll != null && !spxqll.IsDisposed && spxqll.IsHandleCreated)
                {
                    if (spxqll.WindowState == FormWindowState.Minimized)
                    {
                        spxqll.WindowState = FormWindowState.Normal;
                    }//设置窗体状态
                    spxqll.Activate();
                    ((SPXQ2014)spxqll).BeginShowDB(spbh, htqx, "");

                }
                else
                {
                    SPXQ2014 sp2014 = new SPXQ2014();
                    sp2014.Show();
                    sp2014.BeginShowDB(spbh, htqx, "首次");
                }
                #endregion
                          
                

            }
            
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

        private void FormGJSS_Load(object sender, EventArgs e)
        {

            cbSSFL.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.CB_DrawItem);          
            cbSSFL.SelectedItem = "所属分类";
            cbHTQX.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.CB_DrawItem);
            cbHTQX.SelectedItem = "全部";

            //GetData(null);
        }


        //开启一个线程获取商品列表数据
        private void SRT_demo_Run()
        {
            RunTheadGetSPFL OTD = new RunTheadGetSPFL(new delegateForThread(SRT_demo));
            Thread trd = new Thread(new ThreadStart(OTD.BeginRun));

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
            //返回值后的处理
            DataSet dsreturn = (DataSet)OutPutHT["返回值"];

            if (dsreturn != null && dsreturn.Tables[0].Rows.Count > 0)
            {                

                if (dsreturn.Tables.Contains("分类表") && dsreturn.Tables["分类表"].Rows.Count>0)
                {
                    PublicDS.PublisDsSPFL = dsreturn;

                    dv = PublicDS.PublisDsSPFL.Tables["分类表"].DefaultView;

                    //去掉重复的行,dfl用于构建SQL语句in条件
                    dfl = dv.ToTable(true, new string[] { "SortParentID", "SortID" });

                    dv.Sort = SortParentID;
                    //不再使用
                    //string schar = STR_TREENODE;
                    if (dv.Table.Rows.Count > 0)
                    {
                        //RecursBind(INT_TOPID, schar);  
                        DataBind(INT_TOPID);
                    }
                    //如果利用数据库中的排序，可以直接用下面的for语句
                    //for (int i = 0; i < dfl.Rows.Count; i++)
                    //{

                    //    this.cbSSFL.Items.Add(new ComboBoxItem<int, string>(Convert.ToInt32(dfl.Rows[i]["SortID"]), dfl.Rows[i]["SortNameTree"].ToString()));
                    //}
                }


                else  if (dsreturn.Tables.Contains("返回值单条") && dsreturn.Tables["返回值单条"].Rows[0]["执行结果"].ToString() == "err")
                {

                    string showstr = dsreturn.Tables["返回值单条"].Rows[0]["提示文本"].ToString();
                    ArrayList Almsg4 = new ArrayList();
                    Almsg4.Add("");
                    Almsg4.Add(showstr);
                    FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "其他", "", Almsg4);
                    FRSE4.ShowDialog();
                    return;
                }
                                                            

            }
            else
            {
                ArrayList Almsg4 = new ArrayList();
                Almsg4.Add("");
                Almsg4.Add("无法获取数据，请重新登录！");// 这里是提交失败的提示，可能是程序出错，可能是条件不足等
                FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "其他", "", Almsg4);
                FRSE4.ShowDialog();

            }

        }

      

       

    }
}
