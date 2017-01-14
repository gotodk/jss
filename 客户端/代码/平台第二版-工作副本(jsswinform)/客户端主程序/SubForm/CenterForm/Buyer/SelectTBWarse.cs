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
using System.Threading;

namespace 客户端主程序.SubForm.CenterForm.Buyer
{
    public partial class SelectTBWarse : BasicForm
    {
        //淡出计时器
        System.Windows.Forms.Timer Timer_DC;

        /// <summary>
        /// 查询条件
        /// </summary>
        Hashtable ht_where = new Hashtable();

        /// <summary>
        /// 对勾选择后的回调指针
        /// </summary>
        delegateForThread dftForParent;
        Hashtable HT;


        

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

        public SelectTBWarse(Hashtable PHT, delegateForThread dftForParent_temp)
        {
    
            InitializeComponent();

            dftForParent = dftForParent_temp;
            HT = PHT;
            
            //初始化分页回调(带分页的页面，先把这个放上)
            ucPager1.DFT = new delegateForThread(ShowThreadResult_BeginBind);

            //图标
            Icon ic = new Icon(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase+ @"\skin\ji.ico");
            this.Icon = ic;
            //处理下拉框间距
            this.CBhhzq.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.CB_DrawItem);
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

            //若执行正常
            if (ds.Tables.Contains("主要数据"))
            {
                dataGridView1.DataSource = ds.Tables[0].DefaultView;
                if (HT["结算账户"].ToString() == "买家")
                {
                    dataGridView1.Columns["投标拟售量"].Visible = false;
                    dataGridView1.Columns["投标价格"].Visible = false;
                    dataGridView1.Columns["集合预订量"].Visible = false;
                    dataGridView1.Columns["所有标的中最大经济批量"].Visible = false;
                    dataGridView1.Columns["投标信息号"].Visible = false;
                }
                if (HT["结算账户"].ToString() == "卖家")
                {
                    dataGridView1.Columns["最低价投标信息隐藏数据"].Visible = false;
                    dataGridView1.Columns["计价单位"].Width = 70;
                    dataGridView1.Columns["最大经经济批量"].Width = 100;
                    dataGridView1.Columns["合同周期"].Width = 50;
                }
                
            }
            else
            {
                dataGridView1.DataSource = null;
            }
            //取消列表的自动排序
            for (int i = 0; i < this.dataGridView1.Columns.Count; i++)
            {
                this.dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                
            }

            
        }

        /// <summary>
        /// 设置默认搜索条件(这个例子中的哈希表键值一个也不能少)
        /// </summary>
        private void setDefaultSearch()
        {
            ht_where["GetCustomersDataPage_NAME"] = "GetCustomersDataPage";  //每页显示的条数(必须设置)
            ht_where["page_index"] = "0";
            ht_where["page_size"] = "25";  //每页显示的条数(必须设置)
           

            if (HT["结算账户"].ToString() == "买家")
            {
                ht_where["serach_Row_str"] = "*"; //检索字段(必须设置)
                #region//作废
                //ht_where["search_tbname"] = "( select a.TBXXH 投标信息号,b.id 子表主键,b.SPBH 商品编号,b.SPMC 商品名称,b.GGXH 规格型号,b.JJDW 计价单位,最大经济批量=(select max(isnull(JJPL,0)) from ZZ_TBXXBZB where SPBH=b.SPBH and HTQX=b.HTQX and ZT='竞标'),isnull(b.TBYSL,0) 投标拟售量, b.HTQX 合同周期,min(isnull(b.TBJG,0)) 投标价格,a.CreateTime 创建时间,集合预订量=(select isnull(sum(isnull(YDZSL,0)-isnull(YZBL,0)),0) 集合预订量 from ZZ_YDDXXB where ZT='竞标' and SPBH=b.SPBH) from ZZ_TBXXB a,ZZ_TBXXBZB b where a.Number=b.parentNumber and b.ZT='竞标' and b.SFJRLJQ='否' and b.SPBH not in (select SPBH from ZZ_YDDXXB where MJDLYX='" + PublicDS.PublisDsUser.Tables[0].Rows[0]["DLYX"].ToString().Trim() + "' and ZT='竞标') group by a.TBXXH,b.id ,b.SPBH ,b.SPMC ,b.GGXH ,b.JJDW,b.TBYSL,b.HTQX,a.CreateTime ) as tab";
                
                ////测试用
                //ht_where["search_tbname"] = "( select a.TBXXH 投标信息号,b.id 子表主键,b.SPBH 商品编号,b.SPMC 商品名称,b.GGXH 规格型号,b.JJDW 计价单位,最大经济批量=(select max(isnull(JJPL,0)) from ZZ_TBXXBZB where SPBH=b.SPBH and HTQX=b.HTQX and ZT='竞标'),isnull(b.TBYSL,0) 投标拟售量, b.HTQX 合同周期,min(isnull(b.TBJG,0)) 投标价格,a.CreateTime 创建时间,集合预订量=(select isnull(sum(isnull(YDZSL,0)-isnull(YZBL,0)),0) 集合预订量 from ZZ_YDDXXB where ZT='竞标' and SPBH=b.SPBH) from ZZ_TBXXB a,ZZ_TBXXBZB b where a.Number=b.parentNumber and b.ZT='竞标' and b.SFJRLJQ='否' group by a.TBXXH,b.id ,b.SPBH ,b.SPMC ,b.GGXH ,b.JJDW,b.TBYSL,b.HTQX,a.CreateTime ) as tab";
                #endregion

                ht_where["search_tbname"] = "( select b.SPBH 商品编号,b.SPMC 商品名称,b.GGXH 规格型号,b.JJDW 计价单位,ZDJJPL 最大经济批量, '投标拟售量' = isnull((select top 1 HHH.TBYSL  from ZZ_TBXXBZB as HHH where HHH.SPBH =a.SPBH and  HHH.HTQX = a.HTQX and HHH.ZT = '竞标' order by HHH.TBJG ASC),0), isnull(a.TBJG,0) 投标价格,集合预订量=convert(varchar(50),isnull((select sum(Y.YDZSL-Y.YZBL) from ZZ_YDDXXB as Y where Y.SPBH =a.SPBH and  Y.HTZQ = a.HTQX and Y.ZT = '竞标' and Y.NMRJG >= isnull((select top 1 R.TBJG from ZZ_TBXXBZB as R  join ZZ_TBXXB as ZHHH on R.parentNumber=ZHHH.Number  where R.SPBH =a.SPBH and  R.HTQX = a.HTQX and R.ZT = '竞标' order by R.TBJG ASC,ZHHH.CreateTime ASC),0)  and charindex(Y.SHQY,(select top 1 R.BGHQY from ZZ_TBXXBZB as R  join ZZ_TBXXB as ZHHH on R.parentNumber=ZHHH.Number  where R.SPBH =a.SPBH and  R.HTQX = a.HTQX and R.ZT = '竞标' order by R.TBJG ASC,ZHHH.CreateTime ASC))<1),0) ),isnull(a.JJPL,0) 所有标的中最大经济批量,a.HTQX 合同周期,是否参与=(select case  when count(*)!= 0 then '是' else '否' end as 是否参与 from ZZ_TBXXB,ZZ_TBXXBZB where ZZ_TBXXB.Number=ZZ_TBXXBZB.ParentNumber and ZZ_TBXXBZB.SPBH=b.SPBH and ZZ_TBXXB.SELDLYX='" + PublicDS.PublisDsUser.Tables[0].Rows[0]["DLYX"].ToString().Trim() + "') from ZZ_PTSPXXB b right join (   select SPBH,SPMC,min(TBJG) TBJG,min(TBYSL) TBYSL,max(JJPL) JJPL,HTQX from ZZ_TBXXBZB  left join ZZ_TBXXB on ZZ_TBXXBZB.ParentNumber=ZZ_TBXXB.Number where ZT='竞标' and SFJRLJQ='否' group by SPBH,SPMC,HTQX) as a on b.SPBH=a.SPBH ) as tab";


                ht_where["search_mainid"] = " 商品编号 ";  //所检索表的主键(必须设置)
                ht_where["search_str_where"] = " 1=1 and 合同周期='" + CBhhzq.SelectedItem.ToString().Trim() + "'";  //检索条件(必须设置)
                ht_where["search_paixu"] = " DESC ";  //排序方式(必须设置)
                ht_where["search_paixuZD"] = " 投标价格";  //用于排序的字段(必须设置)
            }
            else if (HT["结算账户"].ToString() == "卖家")
            {
                string JSNM = PublicDS.PublisDsUser.Tables[0].Rows[0]["卖家角色编号"].ToString().Trim();
                string htzq = CBhhzq.Text.ToString();
                ht_where["serach_Row_str"] = "商品编号,商品名称,合同周期,规格型号,计价单位,最大经济批量,最低价投标信息隐藏数据,合同周期"; //检索字段(必须设置)
                //ht_where["search_tbname"] = "( select AAA.Number,AAA.SFYX,AAA.CreateTime,'商品编号'=AAA.SPBH,'商品名称'=AAA.SPMC ,'规格型号'=AAA.GGXH ,'计价单位'=AAA.JJDW ,'最大经济批量'=AAA.ZDJJPL,'合同周期'='" + htzq + "','最低价投标信息隐藏数据'=(select top 1 '最低价投标信息'=(''+convert(varchar(50),isnull(Z.TBJG,0))+'*'+convert(varchar(50),isnull(Z.TBYSL,0))+'*'+convert(varchar(50),isnull((select sum(Y.YDZSL-Y.YZBL) from ZZ_YDDXXB as Y where Y.SPBH =Z.SPBH and  Y.HTZQ = Z.HTQX and Y.ZT = '竞标' and Y.NMRJG >= isnull((select top 1 R.TBJG from ZZ_TBXXBZB as R where R.SPBH =Z.SPBH and  R.HTQX = Z.HTQX and R.ZT = '竞标' order by R.TBJG ASC),0)  and charindex(Y.SHQY,(select top 1 R.BGHQY from ZZ_TBXXBZB as R where R.SPBH =Z.SPBH and  R.HTQX = Z.HTQX and R.ZT = '竞标' order by R.TBJG ASC))<1),0) ) + '*'+convert(varchar(50),Z.JJPL)+'')  from ZZ_TBXXBZB as Z where Z.SPBH =AAA.SPBH and  Z.HTQX = '" + htzq + "' and Z.ZT = '竞标' order by Z.TBJG ASC) from ZZ_PTSPXXB AS AAA where AAA.SPBH not in ( select distinct BBB.SPBH from ZZ_TBXXBZB as BBB  join ZZ_TBXXB as CCC  on BBB.parentNumber=CCC.Number where BBB.SFJRLJQ = '是' and BBB.HTQX='" + htzq + "' and CCC.SELJSBH = '" + JSNM + "' and BBB.ZT ='竞标') ) as tab ";  //检索的表(必须设置)

                ht_where["search_tbname"] = "( select AAA.Number,AAA.SFYX,AAA.CreateTime,'商品编号'=AAA.SPBH,'商品名称'=AAA.SPMC ,'规格型号'=AAA.GGXH ,'计价单位'=AAA.JJDW ,'最大经济批量'=AAA.ZDJJPL,'合同周期'='" + htzq + "','最低价投标信息隐藏数据'=(select top 1 '最低价投标信息'=(''+convert(varchar(50),isnull(Z.TBJG,0))+'*'+convert(varchar(50),isnull(Z.TBYSL,0))+'*'+convert(varchar(50),isnull((select sum(Y.YDZSL-Y.YZBL) from ZZ_YDDXXB as Y where Y.SPBH =Z.SPBH and  Y.HTZQ = Z.HTQX and Y.ZT = '竞标' and Y.NMRJG >= isnull((select top 1 R.TBJG from ZZ_TBXXBZB  as R join ZZ_TBXXB as PPH on R.parentNumber=PPH.Number where R.SPBH =Z.SPBH and  R.HTQX = Z.HTQX and R.ZT = '竞标' order by R.TBJG ASC,PPH.CreateTime ASC),0)  and charindex(Y.SHQY,(select top 1 R.BGHQY from ZZ_TBXXBZB as R join ZZ_TBXXB as PPH on R.parentNumber=PPH.Number where R.SPBH =Z.SPBH and  R.HTQX = Z.HTQX and R.ZT = '竞标' order by R.TBJG ASC,PPH.CreateTime ASC))<1),0) ) + '*'+convert(varchar(50),Z.JJPL)+'')  from ZZ_TBXXBZB as Z  join ZZ_TBXXB as PPH on Z.parentNumber=PPH.Number where Z.SPBH =AAA.SPBH and  Z.HTQX = '" + htzq + "' and Z.ZT = '竞标' order by Z.TBJG ASC,PPH.CreateTime ASC) from ZZ_PTSPXXB AS AAA where AAA.SPBH not in ( select distinct BBB.SPBH from ZZ_TBXXBZB as BBB  join ZZ_TBXXB as CCC  on BBB.parentNumber=CCC.Number where BBB.SFJRLJQ = '是' and BBB.HTQX='" + htzq + "' and BBB.ZT ='竞标')   ) as tab ";  //检索的表(必须设置)

                ht_where["search_mainid"] = " Number ";  //所检索表的主键(必须设置)
                ht_where["search_str_where"] = " 1=1 and SFYX='是'";  //检索条件(必须设置)
                ht_where["search_paixu"] = " DESC ";  //排序方式(必须设置)
                ht_where["search_paixuZD"] = " CreateTime";  //用于排序的字段(必须设置)
            }

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


        private void SelectTBWarse_Load(object sender, EventArgs e)
        {
            CBhhzq.SelectedIndex = 0;


            GetData(null);
            warseList1.initdefault();
            
        }


        //选择一个数据带入父窗体(仅限单元格控件部分，非任意部分)
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //不处理

        }

        //搞出鼠标小手
        private void dataGridView1_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                dataGridView1.Cursor = Cursors.Hand;
            }
            else
            {
                dataGridView1.Cursor = Cursors.Default;
            }
        }

        //选择一个数据带入父窗体(单元格任意部分)
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex > -1)
            {
                //获取选中值
                int rowindex = e.RowIndex;
                //回调
                Hashtable return_ht = new Hashtable();
                if(HT["结算账户"].ToString() == "买家")
                {
                    return_ht["商品编号"] = dataGridView1.Rows[rowindex].Cells["商品编号"].Value.ToString();
                    return_ht["商品名称"] = dataGridView1.Rows[rowindex].Cells["商品名称"].Value.ToString();
                    return_ht["规格型号"] = dataGridView1.Rows[rowindex].Cells["规格型号"].Value.ToString();
                    return_ht["计价单位"] = dataGridView1.Rows[rowindex].Cells["计价单位"].Value.ToString();
                    return_ht["最大经济批量"] = dataGridView1.Rows[rowindex].Cells["最大经济批量"].Value.ToString();
                    return_ht["投标价格"] = dataGridView1.Rows[rowindex].Cells["投标价格"].Value.ToString();
                    return_ht["投标拟售量"] = dataGridView1.Rows[rowindex].Cells["投标拟售量"].Value.ToString();
                    return_ht["集合预订量"] = dataGridView1.Rows[rowindex].Cells["集合预订量"].Value.ToString();
                    return_ht["合同周期"] = dataGridView1.Rows[rowindex].Cells["合同周期"].Value.ToString();
                    //return_ht["投标信息号"] = dataGridView1.Rows[rowindex].Cells["投标信息号"].Value.ToString();
                    return_ht["所有标的中最大经济批量"] = dataGridView1.Rows[rowindex].Cells["所有标的中最大经济批量"].Value.ToString();
                }
                else if (HT["结算账户"].ToString() == "卖家")
                {
                    return_ht["商品编号"] = dataGridView1.Rows[rowindex].Cells["商品编号"].Value.ToString();
                    return_ht["商品名称"] = dataGridView1.Rows[rowindex].Cells["商品名称"].Value.ToString();
                    return_ht["规格型号"] = dataGridView1.Rows[rowindex].Cells["规格型号"].Value.ToString();
                    return_ht["计价单位"] = dataGridView1.Rows[rowindex].Cells["计价单位"].Value.ToString();
                    return_ht["最大经济批量"] = dataGridView1.Rows[rowindex].Cells["最大经济批量"].Value.ToString();
                    return_ht["合同周期"] = dataGridView1.Rows[rowindex].Cells["合同周期"].Value.ToString();
                    return_ht["最低价投标信息隐藏数据"] = dataGridView1.Rows[rowindex].Cells["最低价投标信息隐藏数据"].Value.ToString();
                }
                           
                
                dftForParent(return_ht);

                //关闭弹窗
                this.Close();

            }
        }

        private void bscSearch_Click(object sender, EventArgs e)
        {            
            ////更改搜索条件
            setDefaultSearch();
            string SPNameOrBH = "";
            if (uctxtSPNameOrBH.Text.Trim() != "商品名称")
            {
                SPNameOrBH = uctxtSPNameOrBH.Text.Trim();
            }
            
            string[] spfl = warseList1.SelectedItem;
            string stratSPFL = spfl[0].ToString() == "一级分类" ? "" : spfl[0].ToString();
            string secondSPFL = spfl[1].ToString() == "二级分类" ? "" : spfl[1].ToString();
            if (HT["结算账户"].ToString() == "买家")
            {
                ht_where["search_str_where"] = ht_where["search_str_where"] + "  and ( 商品名称 like '%" + SPNameOrBH + "%'" + ") ";  //检索条件(必须设置) 

            }
            else if (HT["结算账户"].ToString() == "卖家")
            {
                ht_where["search_str_where"] = ht_where["search_str_where"] + "  and ( 商品名称 like '%" + SPNameOrBH + "%') and 合同周期='" + CBhhzq.SelectedItem.ToString().Trim() + "'";  //检索条件(必须设置) 

    

            }

            if (stratSPFL != "")
            {
                ht_where["search_str_where"] = ht_where["search_str_where"] + " and 商品编号 in ( select SPBH from ZZ_PTSPXXB where SSFLBH in (select SortID from ZZ_tbMenuSPFL where SortParentPath like '%,'+ CONVERT(varchar(50),(select SortID from ZZ_tbMenuSPFL where SortName = '" + stratSPFL + "'))+',%' or SortID=(select SortID from ZZ_tbMenuSPFL where SortName = '" + stratSPFL + "')) ) ";
            }

            if (secondSPFL != "")
            {
                ht_where["search_str_where"] = ht_where["search_str_where"] + " and 商品编号 in ( select SPBH from ZZ_PTSPXXB where SSFLBH in (select SortID from ZZ_tbMenuSPFL where SortParentPath like '%,'+ CONVERT(varchar(50),(select SortID from ZZ_tbMenuSPFL where SortName = '" + secondSPFL + "'))+',%' or SortID=(select SortID from ZZ_tbMenuSPFL where SortName = '" + secondSPFL + "')) ) ";
            }

            
            //执行查询
            GetData(ht_where);

        }

    }
}
