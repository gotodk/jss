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
using 客户端主程序.DataControl;
using 客户端主程序.NewDataControl;
using 客户端主程序.Support;
namespace 客户端主程序.SubForm
{
    public partial class fmSYXQ : BasicForm
    {
        //淡出计时器
        System.Windows.Forms.Timer Timer_DC;

        /// <summary>
        /// 查询条件
        /// </summary>
        Hashtable ht_where = new Hashtable();

        /// <summary>
        /// 是否为从买入窗体实例化来。
        /// </summary>
        bool isFromBuyAuto = true;

        /// <summary>
        /// 传递参数
        /// </summary>
        Hashtable hashTable = new Hashtable();


        /// <summary>
        /// 对勾选择后的回调指针
        /// </summary>
        delegateForThread dftForParent;
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
        public fmSYXQ(delegateForThread dftForParent_temp,Hashtable hastTable )
        {

            InitializeComponent();
            dftForParent = dftForParent_temp;
            //初始化分页回调(带分页的页面，先把这个放上)
            ucPager1.DFT = new delegateForThread(ShowThreadResult_BeginBind);
            hashTable = hastTable;
            //图标
            Icon ic = new Icon(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\skin\ji.ico");
            this.Icon = ic;
            //设置窗体显示在屏幕中央
            this.StartPosition = FormStartPosition.CenterScreen;
            //不在任务栏显示此信息
            this.ShowInTaskbar = false;

        }


        //显示线程处理结果的函数(用于处理线程返回数据)
        private void ShowThreadResult_BeginBind(Hashtable returnHT)
        {
            try
            {
                //调用异步委托
                Invoke(new delegateForThreadShow(ShowThreadResult_BeginBind_Invoke), new Hashtable[] { returnHT });
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("委托回调错误：" + ex.ToString());
            }
        }
        //处理非线程创建的控件(线程获取到分页数据后的具体绑定处理)
        private void ShowThreadResult_BeginBind_Invoke(Hashtable OutPutHT)
        {
            DataSet ds = (DataSet)OutPutHT["数据"];//获取返回的数据集
            int fys = (int)ds.Tables["附加数据"].Rows[0]["分页数"];
            int jls = (int)ds.Tables["附加数据"].Rows[0]["记录数"];

         

            string msg = (string)ds.Tables["附加数据"].Rows[0]["其他描述"];
            string err = (string)ds.Tables["附加数据"].Rows[0]["执行错误"];
            this.dataGridView1.AutoGenerateColumns = false;
            //若执行正常
            if (ds.Tables.Contains("主要数据"))
            {
                #region//作废--按收益产生时间时间排序
                ////访问远程服务
                //NewDataControl.WebServicesCenter2013 WSC2013 = new NewDataControl.WebServicesCenter2013();
                ////InPutHT可以选择灵活使用
                //Hashtable InPutHT = new Hashtable();
                //InPutHT["经纪人角色编号"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["经纪人角色编号"].ToString();;
                //InPutHT["员工工号"] = hashTable["员工工号"].ToString();
                //DataTable tableCS = StringOP.GetDataTableFormHashtable(InPutHT);
                //object[] cs = { tableCS };
                //DataSet dsreturn = WSC2013.RunAtServices("GetSYLJ", cs);

                //foreach(DataRow row in ds.Tables[0].Rows)
                //{
                //    if (dsreturn != null && dsreturn.Tables["返回值单条"].Rows.Count > 0 && dsreturn.Tables["返回值单条"].Rows[0]["执行结果"].ToString().Trim() == "OK")
                //    {
                //        foreach (DataRow rowReturn in dsreturn.Tables["主表"].Rows)
                //        {
                //            if (row["交易方编号"].ToString().Trim() == rowReturn["交易方编号"].ToString().Trim())
                //            {
                //                row["收益累计"] = rowReturn["收益累计"];
                //            }
                //        }
                //    }

                //}       
                #endregion

                dataGridView1.DataSource = ds.Tables[0].DefaultView;
            }
            else
            {
                dataGridView1.DataSource = null;
            }
        }

        /// <summary>
        /// 设置默认搜索条件(这个例子中的哈希表键值一个也不能少)
        /// </summary>
        private void setDefaultSearch()
        {
            //string JJRJSBH = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["经纪人角色编号"].ToString();
            #region //tableName
            //string tableName = " (select *,'收益累计'=( select isnull(sum(收益金额),0.00) from ( select zkmx.Number, zkmx.LSCSSJ 收益产生时间,'交易方名称'=(select I_JYFMC from AAA_DLZHXXB where J_SELJSBH=fzb.YSTBDSelJSBH),'交易方编号'=(select J_BUYJSBH from AAA_DLZHXXB where J_SELJSBH=fzb.YSTBDSelJSBH),zkmx.XZ 交易性质,isnull(zkmx.JE,0.00) 收益金额, isnull( fzb.YSTBDGLJJRXSYGGH ,'--')中标定标信息表辅助表员工工号 from AAA_ZKLSMXB zkmx left join AAA_THDYFHDXXB thd  on thd.number = zkmx.LYDH left join AAA_ZBDBXXB_FZB fzb on fzb.Number=thd.ZBDBXXBBH  where  XM='经纪人收益' and SJLX='预' and LYYWLX='AAA_THDYFHDXXB' and zkmx.XZ='来自卖方收益' and JSBH='" + JJRJSBH.Trim() + "' and fzb.YSTBDGLJJRXSYGGH='" + hashTable["员工工号"].ToString().Trim() + "' union all  select zkmx.Number, zkmx.LSCSSJ 收益产生时间,'交易方名称'=(select I_JYFMC from AAA_DLZHXXB where J_BUYJSBH=fzb.YSYDDBuyJSBH),'交易方编号'=(select J_BUYJSBH from AAA_DLZHXXB where J_BUYJSBH=fzb.YSYDDBuyJSBH),zkmx.XZ 交易性质,isnull(zkmx.JE,0.00) 收益金额, isnull( fzb.YSYDDGLJJRXSYGGH ,'--')中标定标信息表辅助表员工工号 from AAA_ZKLSMXB zkmx left join AAA_THDYFHDXXB thd  on thd.number = zkmx.LYDH left join AAA_ZBDBXXB_FZB fzb on fzb.Number=thd.ZBDBXXBBH  where  XM='经纪人收益' and SJLX='预' and LYYWLX='AAA_THDYFHDXXB' and zkmx.XZ='来自买方收益'and JSBH='" + JJRJSBH.Trim() + "' and fzb.YSYDDGLJJRXSYGGH='" + hashTable["员工工号"].ToString().Trim() + "'  union all   select zkmx.Number, zkmx.LSCSSJ 收益产生时间,'交易方名称'=(select I_JYFMC from AAA_DLZHXXB where J_SELJSBH=fzb.YSTBDSelJSBH),'交易方编号'=(select J_BUYJSBH from AAA_DLZHXXB where J_SELJSBH=fzb.YSTBDSelJSBH),zkmx.XZ 交易性质,isnull(zkmx.JE,0.00) 收益金额, isnull( fzb.YSTBDGLJJRXSYGGH ,'--')中标定标信息表辅助表员工工号 from AAA_ZKLSMXB zkmx left join AAA_ZBDBXXB_FZB fzb on fzb.Number=zkmx.LYDH  where  XM='经纪人收益' and SJLX='预' and LYYWLX='AAA_ZBDBXXB' and zkmx.XZ='来自卖方收益'and JSBH='" + JJRJSBH.Trim() + "' and fzb.YSTBDGLJJRXSYGGH='" + hashTable["员工工号"].ToString().Trim() + "' union all     select zkmx.Number, zkmx.LSCSSJ 收益产生时间,'交易方名称'=(select I_JYFMC from AAA_DLZHXXB where J_BUYJSBH=fzb.YSYDDBuyJSBH),'交易方编号'=(select J_BUYJSBH from AAA_DLZHXXB where J_BUYJSBH=fzb.YSYDDBuyJSBH),zkmx.XZ 交易性质,isnull(zkmx.JE,0.00) 收益金额, isnull( fzb.YSYDDGLJJRXSYGGH ,'--')中标定标信息表辅助表员工工号 from AAA_ZKLSMXB zkmx left join AAA_ZBDBXXB_FZB fzb on fzb.Number=zkmx.LYDH  where  XM='经纪人收益' and SJLX='预' and LYYWLX='AAA_ZBDBXXB' and zkmx.XZ='来自买方收益'and JSBH='" + JJRJSBH.Trim() + "' and fzb.YSYDDGLJJRXSYGGH='" + hashTable["员工工号"].ToString().Trim() + "' 	) as a  where a.交易方编号=tab.交易方编号) " +

            //"from (select zkmx.Number, zkmx.LSCSSJ 收益产生时间,'交易方名称'=(select I_JYFMC from AAA_DLZHXXB where J_SELJSBH=fzb.YSTBDSelJSBH),'交易方编号'=(select J_BUYJSBH from AAA_DLZHXXB where J_SELJSBH=fzb.YSTBDSelJSBH),zkmx.XZ 交易性质,isnull(zkmx.JE,0.00) 收益金额, isnull( fzb.YSTBDGLJJRXSYGGH ,'--')中标定标信息表辅助表员工工号 from AAA_ZKLSMXB zkmx left join AAA_THDYFHDXXB thd  on thd.number = zkmx.LYDH left join AAA_ZBDBXXB_FZB fzb on fzb.Number=thd.ZBDBXXBBH  where  XM='经纪人收益' and SJLX='预' and LYYWLX='AAA_THDYFHDXXB' and zkmx.XZ='来自卖方收益' and JSBH='" + JJRJSBH.Trim() + "' and fzb.YSTBDGLJJRXSYGGH='" + hashTable["员工工号"].ToString().Trim() + "' union all  select zkmx.Number, zkmx.LSCSSJ 收益产生时间,'交易方名称'=(select I_JYFMC from AAA_DLZHXXB where J_BUYJSBH=fzb.YSYDDBuyJSBH),'交易方编号'=(select J_BUYJSBH from AAA_DLZHXXB where J_BUYJSBH=fzb.YSYDDBuyJSBH),zkmx.XZ 交易性质,isnull(zkmx.JE,0.00) 收益金额, isnull( fzb.YSYDDGLJJRXSYGGH ,'--')中标定标信息表辅助表员工工号 from AAA_ZKLSMXB zkmx left join AAA_THDYFHDXXB thd  on thd.number = zkmx.LYDH left join AAA_ZBDBXXB_FZB fzb on fzb.Number=thd.ZBDBXXBBH  where  XM='经纪人收益' and SJLX='预' and LYYWLX='AAA_THDYFHDXXB' and zkmx.XZ='来自买方收益'and JSBH='" + JJRJSBH.Trim() + "' and fzb.YSYDDGLJJRXSYGGH='" + hashTable["员工工号"].ToString().Trim() + "'  union all   select zkmx.Number, zkmx.LSCSSJ 收益产生时间,'交易方名称'=(select I_JYFMC from AAA_DLZHXXB where J_SELJSBH=fzb.YSTBDSelJSBH),'交易方编号'=(select J_BUYJSBH from AAA_DLZHXXB where J_SELJSBH=fzb.YSTBDSelJSBH),zkmx.XZ 交易性质,isnull(zkmx.JE,0.00) 收益金额, isnull( fzb.YSTBDGLJJRXSYGGH ,'--')中标定标信息表辅助表员工工号 from AAA_ZKLSMXB zkmx left join AAA_ZBDBXXB_FZB fzb on fzb.Number=zkmx.LYDH  where  XM='经纪人收益' and SJLX='预' and LYYWLX='AAA_ZBDBXXB' and zkmx.XZ='来自卖方收益'and JSBH='" + JJRJSBH.Trim() + "' and fzb.YSTBDGLJJRXSYGGH='" + hashTable["员工工号"].ToString().Trim() + "' union all     select zkmx.Number, zkmx.LSCSSJ 收益产生时间,'交易方名称'=(select I_JYFMC from AAA_DLZHXXB where J_BUYJSBH=fzb.YSYDDBuyJSBH),'交易方编号'=(select J_BUYJSBH from AAA_DLZHXXB where J_BUYJSBH=fzb.YSYDDBuyJSBH),zkmx.XZ 交易性质,isnull(zkmx.JE,0.00) 收益金额, isnull( fzb.YSYDDGLJJRXSYGGH ,'--')中标定标信息表辅助表员工工号 from AAA_ZKLSMXB zkmx left join AAA_ZBDBXXB_FZB fzb on fzb.Number=zkmx.LYDH  where  XM='经纪人收益' and SJLX='预' and LYYWLX='AAA_ZBDBXXB' and zkmx.XZ='来自买方收益'and JSBH='" + JJRJSBH.Trim() + "' and fzb.YSYDDGLJJRXSYGGH='" + hashTable["员工工号"].ToString().Trim() + "') as tab ) tab1";
            //#region//作废--按收益产生时间时间排序
            ////string tableName = " ( select zkmx.Number, zkmx.LSCSSJ 收益产生时间,'交易方名称'=(select I_JYFMC from AAA_DLZHXXB where J_SELJSBH=fzb.YSTBDSelJSBH),'交易方编号'=(select J_BUYJSBH from AAA_DLZHXXB where J_SELJSBH=fzb.YSTBDSelJSBH),zkmx.XZ 交易性质,isnull(zkmx.JE,0.00) 收益金额, isnull( fzb.YSTBDGLJJRXSYGGH ,'--')中标定标信息表辅助表员工工号 from AAA_ZKLSMXB zkmx left join AAA_THDYFHDXXB thd  on thd.number = zkmx.LYDH left join AAA_ZBDBXXB_FZB fzb on fzb.Number=thd.ZBDBXXBBH  where  XM='经纪人收益' and SJLX='预' and LYYWLX='AAA_THDYFHDXXB' and zkmx.XZ='来自卖方收益' and JSBH='" + JJRJSBH.Trim() + "' and fzb.YSTBDGLJJRXSYGGH='" + hashTable["员工工号"].ToString().Trim() + "' union all  select zkmx.Number, zkmx.LSCSSJ 收益产生时间,'交易方名称'=(select I_JYFMC from AAA_DLZHXXB where J_BUYJSBH=fzb.YSYDDBuyJSBH),'交易方编号'=(select J_BUYJSBH from AAA_DLZHXXB where J_BUYJSBH=fzb.YSYDDBuyJSBH),zkmx.XZ 交易性质,isnull(zkmx.JE,0.00) 收益金额, isnull( fzb.YSYDDGLJJRXSYGGH ,'--')中标定标信息表辅助表员工工号 from AAA_ZKLSMXB zkmx left join AAA_THDYFHDXXB thd  on thd.number = zkmx.LYDH left join AAA_ZBDBXXB_FZB fzb on fzb.Number=thd.ZBDBXXBBH  where  XM='经纪人收益' and SJLX='预' and LYYWLX='AAA_THDYFHDXXB' and zkmx.XZ='来自买方收益'and JSBH='" + JJRJSBH.Trim() + "' and fzb.YSYDDGLJJRXSYGGH='" + hashTable["员工工号"].ToString().Trim() + "'  union all  select zkmx.Number, zkmx.LSCSSJ 收益产生时间,'交易方名称'=(select I_JYFMC from AAA_DLZHXXB where J_SELJSBH=fzb.YSTBDSelJSBH),'交易方编号'=(select J_BUYJSBH from AAA_DLZHXXB where J_SELJSBH=fzb.YSTBDSelJSBH),zkmx.XZ 交易性质,isnull(zkmx.JE,0.00) 收益金额, isnull( fzb.YSTBDGLJJRXSYGGH ,'--')中标定标信息表辅助表员工工号 from AAA_ZKLSMXB zkmx left join AAA_ZBDBXXB_FZB fzb on fzb.Number=zkmx.LYDH  where  XM='经纪人收益' and SJLX='预' and LYYWLX='AAA_ZBDBXXB' and zkmx.XZ='来自卖方收益'and JSBH='" + JJRJSBH.Trim() + "' and fzb.YSTBDGLJJRXSYGGH='" + hashTable["员工工号"].ToString().Trim() + "' union all select zkmx.Number, zkmx.LSCSSJ 收益产生时间,'交易方名称'=(select I_JYFMC from AAA_DLZHXXB where J_BUYJSBH=fzb.YSYDDBuyJSBH),'交易方编号'=(select J_BUYJSBH from AAA_DLZHXXB where J_BUYJSBH=fzb.YSYDDBuyJSBH),zkmx.XZ 交易性质,isnull(zkmx.JE,0.00) 收益金额, isnull( fzb.YSYDDGLJJRXSYGGH ,'--')中标定标信息表辅助表员工工号 from AAA_ZKLSMXB zkmx left join AAA_ZBDBXXB_FZB fzb on fzb.Number=zkmx.LYDH  where  XM='经纪人收益' and SJLX='预' and LYYWLX='AAA_ZBDBXXB' and zkmx.XZ='来自买方收益'and JSBH='" + JJRJSBH.Trim() + "' and fzb.YSYDDGLJJRXSYGGH='" + hashTable["员工工号"].ToString().Trim() + "' ) as tab ";
            //#endregion

            #endregion

            //ht_where["GetCustomersDataPage_NAME"] = "GetCustomersDataPage";
            //ht_where["page_index"] = "0";
            //ht_where["page_size"] = "10";  //每页显示的条数(必须设置)
            //ht_where["serach_Row_str"] = " *  ";


            //#region ht_where["search_tbname"]
            //ht_where["search_tbname"] = tableName;
         
            //#endregion

            //ht_where["search_mainid"] = " Number ";
            //ht_where["search_str_where"] = " 1=1 ";
            //if (txtJYFMC.Text.Trim() != "")
            //{
            //    ht_where["search_str_where"] = " 交易方名称 LIKE '%" + this.txtJYFMC.Text.Trim() + "%' ";
            //}
            //ht_where["search_paixu"] = " ASC ";
            //ht_where["search_paixuZD"] = "收益累计 desc, 收益产生时间  ";


            ht_where["page_index"] = "0";
            ht_where["page_size"] = "10";  //每页显示的条数(必须设置)
            ht_where["webmethod"] = "银行经纪人C区收益管理";
            Hashtable ht_tiaojian = new Hashtable();
            ht_tiaojian["标签名"] = "收益详情";
            ht_tiaojian["经纪人编号"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["经纪人角色编号"].ToString();
            ht_tiaojian["员工工号"] = hashTable["员工工号"].ToString().Trim();
            ht_tiaojian["交易方名称"] = txtJYFMC.Text.Trim();
            ht_where["tiaojian"] = ht_tiaojian;

        }
        
        /// <summary>
        /// 设置条件，获取通过分页控件数据,留空则使用默认条件
        /// </summary>
        /// <param name="HT_Where_temp">留空则使用默认条件</param>
        private void GetData(Hashtable HT_Where_temp)
        {
            if (HT_Where_temp == null)
            {
                setDefaultSearch();
            }
            ucPager1.HT_Where = ht_where;
            ucPager1.BeginBindForUCPager();

        }
        
        private void FormSelectList_Load(object sender, EventArgs e)
        {
            GetData(null);
        }

        //搜索按钮
        private void basicButton2_Click(object sender, EventArgs e)
        {
            GetData(null);
        }
        /// <summary>
        ///导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExport_Click(object sender, EventArgs e)
        {
            ////设置要隐藏的列(原分页中，有些列可能是隐藏不显示的，导出时，可以选择滤掉)
            //string[] HideColumns = new string[] { "Number", "中标定标信息表辅助表员工工号", "交易方编号" };
            //string JJRJSBH = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["经纪人角色编号"].ToString();

            ////设置导出语句
            //string sql = "select *,'收益累计'=( select isnull(sum(收益金额),0.00) from ( select zkmx.Number, zkmx.LSCSSJ 收益产生时间,'交易方名称'=(select I_JYFMC from AAA_DLZHXXB where J_SELJSBH=fzb.YSTBDSelJSBH),'交易方编号'=(select J_BUYJSBH from AAA_DLZHXXB where J_SELJSBH=fzb.YSTBDSelJSBH),zkmx.XZ 交易性质,isnull(zkmx.JE,0.00) 收益金额, isnull( fzb.YSTBDGLJJRXSYGGH ,'--')中标定标信息表辅助表员工工号 from AAA_ZKLSMXB zkmx left join AAA_THDYFHDXXB thd  on thd.number = zkmx.LYDH left join AAA_ZBDBXXB_FZB fzb on fzb.Number=thd.ZBDBXXBBH  where  XM='经纪人收益' and SJLX='预' and LYYWLX='AAA_THDYFHDXXB' and zkmx.XZ='来自卖方收益' and JSBH='" + JJRJSBH.Trim() + "' and fzb.YSTBDGLJJRXSYGGH='" + hashTable["员工工号"].ToString().Trim() + "' union all  select zkmx.Number, zkmx.LSCSSJ 收益产生时间,'交易方名称'=(select I_JYFMC from AAA_DLZHXXB where J_BUYJSBH=fzb.YSYDDBuyJSBH),'交易方编号'=(select J_BUYJSBH from AAA_DLZHXXB where J_BUYJSBH=fzb.YSYDDBuyJSBH),zkmx.XZ 交易性质,isnull(zkmx.JE,0.00) 收益金额, isnull( fzb.YSYDDGLJJRXSYGGH ,'--')中标定标信息表辅助表员工工号 from AAA_ZKLSMXB zkmx left join AAA_THDYFHDXXB thd  on thd.number = zkmx.LYDH left join AAA_ZBDBXXB_FZB fzb on fzb.Number=thd.ZBDBXXBBH  where  XM='经纪人收益' and SJLX='预' and LYYWLX='AAA_THDYFHDXXB' and zkmx.XZ='来自买方收益'and JSBH='" + JJRJSBH.Trim() + "' and fzb.YSYDDGLJJRXSYGGH='" + hashTable["员工工号"].ToString().Trim() + "'  union all   select zkmx.Number, zkmx.LSCSSJ 收益产生时间,'交易方名称'=(select I_JYFMC from AAA_DLZHXXB where J_SELJSBH=fzb.YSTBDSelJSBH),'交易方编号'=(select J_BUYJSBH from AAA_DLZHXXB where J_SELJSBH=fzb.YSTBDSelJSBH),zkmx.XZ 交易性质,isnull(zkmx.JE,0.00) 收益金额, isnull( fzb.YSTBDGLJJRXSYGGH ,'--')中标定标信息表辅助表员工工号 from AAA_ZKLSMXB zkmx left join AAA_ZBDBXXB_FZB fzb on fzb.Number=zkmx.LYDH  where  XM='经纪人收益' and SJLX='预' and LYYWLX='AAA_ZBDBXXB' and zkmx.XZ='来自卖方收益'and JSBH='" + JJRJSBH.Trim() + "' and fzb.YSTBDGLJJRXSYGGH='" + hashTable["员工工号"].ToString().Trim() + "' union all     select zkmx.Number, zkmx.LSCSSJ 收益产生时间,'交易方名称'=(select I_JYFMC from AAA_DLZHXXB where J_BUYJSBH=fzb.YSYDDBuyJSBH),'交易方编号'=(select J_BUYJSBH from AAA_DLZHXXB where J_BUYJSBH=fzb.YSYDDBuyJSBH),zkmx.XZ 交易性质,isnull(zkmx.JE,0.00) 收益金额, isnull( fzb.YSYDDGLJJRXSYGGH ,'--')中标定标信息表辅助表员工工号 from AAA_ZKLSMXB zkmx left join AAA_ZBDBXXB_FZB fzb on fzb.Number=zkmx.LYDH  where  XM='经纪人收益' and SJLX='预' and LYYWLX='AAA_ZBDBXXB' and zkmx.XZ='来自买方收益'and JSBH='" + JJRJSBH.Trim() + "' and fzb.YSYDDGLJJRXSYGGH='" + hashTable["员工工号"].ToString().Trim() + "' 	) as a  where a.交易方编号=tab.交易方编号) " +

            //"from (select zkmx.Number, zkmx.LSCSSJ 收益产生时间,'交易方名称'=(select I_JYFMC from AAA_DLZHXXB where J_SELJSBH=fzb.YSTBDSelJSBH),'交易方编号'=(select J_BUYJSBH from AAA_DLZHXXB where J_SELJSBH=fzb.YSTBDSelJSBH),zkmx.XZ 交易性质,isnull(zkmx.JE,0.00) 收益金额, isnull( fzb.YSTBDGLJJRXSYGGH ,'--')中标定标信息表辅助表员工工号 from AAA_ZKLSMXB zkmx left join AAA_THDYFHDXXB thd  on thd.number = zkmx.LYDH left join AAA_ZBDBXXB_FZB fzb on fzb.Number=thd.ZBDBXXBBH  where  XM='经纪人收益' and SJLX='预' and LYYWLX='AAA_THDYFHDXXB' and zkmx.XZ='来自卖方收益' and JSBH='" + JJRJSBH.Trim() + "' and fzb.YSTBDGLJJRXSYGGH='" + hashTable["员工工号"].ToString().Trim() + "' union all  select zkmx.Number, zkmx.LSCSSJ 收益产生时间,'交易方名称'=(select I_JYFMC from AAA_DLZHXXB where J_BUYJSBH=fzb.YSYDDBuyJSBH),'交易方编号'=(select J_BUYJSBH from AAA_DLZHXXB where J_BUYJSBH=fzb.YSYDDBuyJSBH),zkmx.XZ 交易性质,isnull(zkmx.JE,0.00) 收益金额, isnull( fzb.YSYDDGLJJRXSYGGH ,'--')中标定标信息表辅助表员工工号 from AAA_ZKLSMXB zkmx left join AAA_THDYFHDXXB thd  on thd.number = zkmx.LYDH left join AAA_ZBDBXXB_FZB fzb on fzb.Number=thd.ZBDBXXBBH  where  XM='经纪人收益' and SJLX='预' and LYYWLX='AAA_THDYFHDXXB' and zkmx.XZ='来自买方收益'and JSBH='" + JJRJSBH.Trim() + "' and fzb.YSYDDGLJJRXSYGGH='" + hashTable["员工工号"].ToString().Trim() + "'  union all   select zkmx.Number, zkmx.LSCSSJ 收益产生时间,'交易方名称'=(select I_JYFMC from AAA_DLZHXXB where J_SELJSBH=fzb.YSTBDSelJSBH),'交易方编号'=(select J_BUYJSBH from AAA_DLZHXXB where J_SELJSBH=fzb.YSTBDSelJSBH),zkmx.XZ 交易性质,isnull(zkmx.JE,0.00) 收益金额, isnull( fzb.YSTBDGLJJRXSYGGH ,'--')中标定标信息表辅助表员工工号 from AAA_ZKLSMXB zkmx left join AAA_ZBDBXXB_FZB fzb on fzb.Number=zkmx.LYDH  where  XM='经纪人收益' and SJLX='预' and LYYWLX='AAA_ZBDBXXB' and zkmx.XZ='来自卖方收益'and JSBH='" + JJRJSBH.Trim() + "' and fzb.YSTBDGLJJRXSYGGH='" + hashTable["员工工号"].ToString().Trim() + "' union all     select zkmx.Number, zkmx.LSCSSJ 收益产生时间,'交易方名称'=(select I_JYFMC from AAA_DLZHXXB where J_BUYJSBH=fzb.YSYDDBuyJSBH),'交易方编号'=(select J_BUYJSBH from AAA_DLZHXXB where J_BUYJSBH=fzb.YSYDDBuyJSBH),zkmx.XZ 交易性质,isnull(zkmx.JE,0.00) 收益金额, isnull( fzb.YSYDDGLJJRXSYGGH ,'--')中标定标信息表辅助表员工工号 from AAA_ZKLSMXB zkmx left join AAA_ZBDBXXB_FZB fzb on fzb.Number=zkmx.LYDH  where  XM='经纪人收益' and SJLX='预' and LYYWLX='AAA_ZBDBXXB' and zkmx.XZ='来自买方收益'and JSBH='" + JJRJSBH.Trim() + "' and fzb.YSYDDGLJJRXSYGGH='" + hashTable["员工工号"].ToString().Trim() + "') as tab";
            //if (txtJYFMC.Text.Trim() != "")
            //{
            //    sql += " where 交易方名称 LIKE '%" + this.txtJYFMC.Text.Trim() + "%' ";
            //}
            //sql = sql + " order by 收益累计 desc, 收益产生时间 asc";
            //cMyXls1.BeginRunFrom_ht_where(sql, HideColumns);

            Hashtable ht_export = new Hashtable();
            ht_export["filename"] = "收益详情";
            ht_export["webmethod"] = "银行经纪人C区收益管理导出";
            ht_export["tiaojian"] = (Hashtable)ht_where["tiaojian"];

            cMyXls1.BeginRunFrom_ht_where(ht_export);
        }

    
    }
}
