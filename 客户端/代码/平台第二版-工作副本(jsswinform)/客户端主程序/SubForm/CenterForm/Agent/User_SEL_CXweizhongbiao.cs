using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using 客户端主程序.DataControl;
using System.Threading;

namespace 客户端主程序.SubForm.CenterForm.Agent
{
    public partial class User_SEL_CXweizhongbiao : UserControl
    {
        /// <summary>
        /// 查询条件
        /// </summary>
        Hashtable ht_where = new Hashtable();

        public User_SEL_CXweizhongbiao()
        {
            InitializeComponent();

            //初始化分页回调(带分页的页面，先把这个放上)
            ucPager1.DFT = new delegateForThread(ShowThreadResult_BeginBind);
            //处理下拉框间距
            this.CBleixing.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.CB_DrawItem);
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

        //加载窗体后处理数据
        private void UserControl1_Load(object sender, EventArgs e)
        {
            CBleixing.SelectedIndex = 0;
            GetData(null);
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

            //是否自动绑定列
            dataGridView1.AutoGenerateColumns = false;

            //若执行正常
            if (ds.Tables.Contains("主要数据"))
            {
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
                    dgvcs.Padding = new Padding(10,0,0,0);

                    this.dataGridView1.Columns[i].HeaderCell.Style = dgvcs;
                    this.dataGridView1.Columns[i].DefaultCellStyle = dgvcs;

                }
                this.dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            

        }

        /// <summary>
        /// 设置默认搜索条件(这个例子中的哈希表键值一个也不能少)
        /// </summary>
        private void setDefaultSearch()
        {
            string JSNM = PublicDS.PublisDsUser.Tables[0].Rows[0]["DLYX"].ToString().Trim();

            ht_where["GetCustomersDataPage_NAME"] = "GetCustomersDataPage";  //每页显示的条数(必须设置)
            ht_where["page_index"] = "0";
            ht_where["page_size"] = "25";  //每页显示的条数(必须设置)
            ht_where["serach_Row_str"] = " * "; //检索字段(必须设置)
            ht_where["search_tbname"] = "  ( select SELDLYX,SELYHM, '最大经济批量'=(select top 1 P.ZDJJPL from ZZ_PTSPXXB as P where P.SPBH=B.SPBH), '子表编号'=B.ID, '主表编号'=Z.Number,'投标时间'=Z.CreateTime,  '标的号'=convert(varchar(20),Z.Number)+'_'+convert(varchar(20),B.ID),'商品编号'=B.SPBH,'商品名称'=B.SPMC,'型号规格'=B.GGXH, '供货周期'=B.HTQX, '计价单位'=B.JJDW,'竞标状态'=B.ZT,'经济批量'=B.JJPL, '投标拟售量'=B.TBYSL, '投标价格'=B.TBJG,  '是否在冷静期内'=B.SFJRLJQ,'进入冷静期时间'=B.JRLJQSJ, '最低价标的经济批量'=isnull((select top 1 Z.JJPL from ZZ_TBXXBZB as Z  join ZZ_TBXXB as ZHHH on Z.parentNumber=ZHHH.Number  where Z.SPBH =B.SPBH and  Z.HTQX = B.HTQX and Z.ZT = '竞标' order by Z.TBJG ASC,ZHHH.CreateTime ASC),0),  '最低价标的投标价格'=isnull((select top 1 Z.TBJG from ZZ_TBXXBZB as Z join ZZ_TBXXB as ZHHH on Z.parentNumber=ZHHH.Number  where Z.SPBH =B.SPBH and  Z.HTQX = B.HTQX and Z.ZT = '竞标' order by Z.TBJG ASC,ZHHH.CreateTime ASC),0.00),  '最低价标的投标拟售量'=isnull((select top 1 Z.TBYSL from ZZ_TBXXBZB as Z join ZZ_TBXXB as ZHHH on Z.parentNumber=ZHHH.Number  where Z.SPBH =B.SPBH and  Z.HTQX = B.HTQX and Z.ZT = '竞标' order by Z.TBJG ASC,ZHHH.CreateTime ASC),0),  '参与竞标集合预订量'=isnull((select sum(Y.YDZSL-Y.YZBL) from ZZ_YDDXXB as Y where Y.SPBH =B.SPBH and  Y.HTZQ = B.HTQX and Y.ZT = '竞标' and Y.NMRJG >= isnull((select top 1 Z.TBJG from ZZ_TBXXBZB as Z join ZZ_TBXXB as ZHHH on Z.parentNumber=ZHHH.Number  where Z.SPBH =B.SPBH and  Z.HTQX = B.HTQX and Z.ZT = '竞标' order by Z.TBJG ASC,ZHHH.CreateTime ASC),0)  and charindex(Y.SHQY,(select top 1 Z.BGHQY from ZZ_TBXXBZB as Z join ZZ_TBXXB as ZHHH on Z.parentNumber=ZHHH.Number  where Z.SPBH =B.SPBH and  Z.HTQX = B.HTQX and Z.ZT = '竞标' order by Z.TBJG ASC,ZHHH.CreateTime ASC))<1 ),0),'最低价标的达成率'=isnull(convert(varchar(20),ROUND(convert(float,isnull((select sum(Y.YDZSL-Y.YZBL) from ZZ_YDDXXB as Y where Y.SPBH =B.SPBH and  Y.HTZQ = B.HTQX and Y.ZT = '竞标' and Y.NMRJG >= (select top 1 Z.TBJG from ZZ_TBXXBZB as Z join ZZ_TBXXB as ZHHH on Z.parentNumber=ZHHH.Number  where Z.SPBH =B.SPBH and  Z.HTQX = B.HTQX and Z.ZT = '竞标' order by Z.TBJG ASC,ZHHH.CreateTime ASC)  and charindex(Y.SHQY,(select top 1 Z.BGHQY from ZZ_TBXXBZB as Z join ZZ_TBXXB as ZHHH on Z.parentNumber=ZHHH.Number  where Z.SPBH =B.SPBH and  Z.HTQX = B.HTQX and Z.ZT = '竞标' order by Z.TBJG ASC,ZHHH.CreateTime ASC))<1 ),0))/convert(float,(select top 1 Z.TBYSL from ZZ_TBXXBZB as Z join ZZ_TBXXB as ZHHH on Z.parentNumber=ZHHH.Number  where Z.SPBH =B.SPBH and  Z.HTQX = B.HTQX and Z.ZT = '竞标' order by Z.TBJG ASC,ZHHH.CreateTime ASC)) * 100,2)),'0.00') + '%'   from ZZ_TBXXB as Z join ZZ_TBXXBZB as B on Z.Number = B.parentNumber where B.ZT = '竞标' and  Z.GLJJRDLYX='" + JSNM + "' ) as tab ";//检索的表(必须设置)

            ht_where["search_mainid"] = " 子表编号 + 标的号 ";  //所检索表的主键(必须设置)
            ht_where["search_str_where"] = " 1=1 ";  //检索条件(必须设置)
            ht_where["search_paixu"] = " DESC ";  //排序方式(必须设置)
            ht_where["search_paixuZD"] = " 投标时间 ";  //用于排序的字段(必须设置)
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






        private void BBsearch_Click(object sender, EventArgs e)
        {
            //获取搜索条件
            string key = TBkey.Text.Trim();
            string htzq = CBleixing.Text;
            string begintime = dateTimePickerBegin.Value.ToShortDateString() + " 00:00:01";
            string endtime = dateTimePickerEnd.Value.ToShortDateString() + " 23:59:59";
            //更改搜索条件
            setDefaultSearch();
            ht_where["search_str_where"] = ht_where["search_str_where"] + " and 投标时间 > '" + begintime + "' and 投标时间 < '" + endtime + "' ";  //检索条件(必须设置)
            if (htzq != "不限周期")
            {
                ht_where["search_str_where"] = ht_where["search_str_where"] + " and 供货周期='" + htzq + "' ";
            }
            if (key !="" && key!="标的号、商品编号、商品名称")
            {
                ht_where["search_str_where"] = ht_where["search_str_where"] + " and (标的号 like '%" + key + "%' or 商品编号 like '%" + key + "%' or 商品名称 like '%" + key + "%') ";
            }
            if (txtBuyEmailOrName.Text.Trim() != "" && txtBuyEmailOrName.Text.Trim() != "卖家登陆邮箱、卖家用户名")
            {
                ht_where["search_str_where"] += " and (SELDLYX like '%" + txtBuyEmailOrName.Text.Trim() + "%' or SELYHM like '%" + txtBuyEmailOrName.Text.Trim() + "%') ";
            }
            //执行查询
            GetData(ht_where);
        }

    }
}
