using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using 客户端主程序.DataControl;

namespace 客户端主程序.SubForm.NewCenterForm.SUBUC.JJRYWGL
{
    public partial class ucBZHXJJFJYSPFX_C : UserControl
    {
         /// <summary>
        /// 查询条件
        /// </summary>
        Hashtable ht_where = new Hashtable();
        public ucBZHXJJFJYSPFX_C()
        {
            InitializeComponent();
                  //初始化分页回调(带分页的页面，先把这个放上)
            ucPager1.DFT = new delegateForThread(STR_BeginBind);
        }
        string tab = "";
        /// <summary>
        /// </summary>        /// 设置默认搜索条件(这个例子中的哈希表键值一个也不能少)

        private void setDefaultSearch()
        {           
            //string tb_tab = "(select distinct spbh,GLJJRYX from AAA_TBD where ZT<>'撤销' union  select distinct spbh,GLJJRYX from AAA_YDDXXB where ZT<>'撤销' )";//经纪人关联的交易方的交易商品信息
            //string tb_sale = "(select b.T_YSTBDGLJJRYX as 关联经纪人账号,b.Z_SPBH,sum(a.T_DJHKJE) as 卖出金额 from AAA_THDYFHDXXB as a left join AAA_ZBDBXXB as b on a.ZBDBXXBBH=b.Number where a.F_BUYWYYSHCZSJ is not null group by b.T_YSTBDGLJJRYX,b.Z_SPBH)";//累计卖出金额
            //string tb_buy = "(select b.Y_YSYDDGLJJRYX as 关联经纪人账号,b.Z_SPBH,sum(a.T_DJHKJE) as 买入金额 from AAA_THDYFHDXXB as a left join AAA_ZBDBXXB as b on a.ZBDBXXBBH=b.Number where a.F_BUYWYYSHCZSJ is not null group by b.Y_YSYDDGLJJRYX,b.Z_SPBH)";//累计买入金额
            //ht_where["GetCustomersDataPage_NAME"] = "GetCustomersDataPage";  //每页显示的条数(必须设置)
            //ht_where["page_index"] = "0";
            //ht_where["page_size"] = "25";  //每页显示的条数(必须设置)
            //ht_where["serach_Row_str"] = "tab.GLJJRYX as 经纪人账号,a.SPBH as 商品编号,a.SPMC as 商品名称,a.GG as 商品规格,'0' as 累计中标次数,'0' as 累计定标次数,isnull(b.卖出金额,0.00) as 累计卖出金额,'--' as 最大卖方名称,'--' as 最大卖方区域,isnull(c.买入金额,0.00) as 累计买入金额,'--' as 最大买方名称,'--' as 最大买方区域 "; //检索字段(必须设置)
            //ht_where["search_tbname"] = " AAA_PTSPXXB as a left join "+tb_tab+" as tab on tab.SPBH=a.SPBH left join "+tb_sale+" as b on a.SPBH=b.Z_SPBH and b.关联经纪人账号=tab.GLJJRYX left join "+tb_buy+" as c on c.关联经纪人账号=tab.GLJJRYX and c.Z_SPBH=tab.SPBH  and c.关联经纪人账号=tab.GLJJRYX ";  //检索的表(必须设置)
            //ht_where["search_mainid"] = " a.SPBH ";  //所检索表的主键(必须设置)
            //ht_where["search_str_where"] = " tab.GLJJRYX='" + PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString() + "'";  //检索条件(必须设置)
            //ht_where["search_paixu"] = " desc ";  //排序方式(必须设置)
            //ht_where["search_paixuZD"] = "isnull(c.买入金额,0.00) desc,isnull(b.卖出金额,0.00) ";  //用于排序的字段(必须设置)
            //ht_where["method_retreatment"] = "JJRYWGL_C|JYFJYSPTJ"; //用于处理账户状态字段值

            ht_where["page_index"] = "0";
            ht_where["page_size"] = "25";  //每页显示的条数(必须设置)
            ht_where["webmethod"] = "经纪人业务C区";
            Hashtable ht_tiaojian = new Hashtable();
            ht_tiaojian["标签名"] = "交易商品分析";
            ht_tiaojian["用户邮箱"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString();
            ht_tiaojian["商品名称"] = txtSPMC.Text.Trim();           
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
        private void GetData(Hashtable HT_Where_temp)
        {
            if (HT_Where_temp == null)
            {
                setDefaultSearch();
            }
            ucPager1.HT_Where = ht_where;
            ucPager1.BeginBindForUCPager();
        }
      
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            setDefaultSearch();

            //if (txtSPMC.Text.Trim() != "")
            //{
            //    ht_where["search_str_where"] = ht_where["search_str_where"] + " and a.SPMC like '%" + txtSPMC.Text.Trim() + "%' ";  //检索条件(必须设置)
            //}
            //执行查询
            GetData(ht_where);
        }
      
        private void ucBZHXJJFJYSPFX_C_Load(object sender, EventArgs e)
        {
            GetData(null);
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExport_Click(object sender, EventArgs e)
        {
            //设置要隐藏的列(原分页中，有些列可能是隐藏不显示的，导出时，可以选择滤掉)
            //string[] HideColumns = new string[] { "" };
            //设置导出语句
            //string sql = " select * from  " + tab + "  where 1=1 ";
            //if (txtSPMC.Text.Trim() != "")
            //{
            //    sql = sql + " and 商品名称 like '%" + txtSPMC.Text.Trim() + "%' ";  //检索条件(必须设置)
            //}
            //string sql = "select prd.SPBH as 商品编号,SPMC as 商品名称,GG as 商品规格,(select COUNT(*) from AAA_ZBDBXXB where Z_SPBH=prd.SPBH and (T_YSTBDGLJJRYX=tab.GLJJRYX or Y_YSYDDGLJJRYX=tab.GLJJRYX)) as 累计中标次数,(select COUNT(*) from AAA_ZBDBXXB where Z_SPBH=prd.SPBH and (T_YSTBDGLJJRYX=tab.GLJJRYX or Y_YSYDDGLJJRYX=tab.GLJJRYX) and z_HTZT in ('定标','定标合同到期','定标合同终止','定标执行完成')) as 累计定标次数,isnull(b.卖出金额,0.00) as 累计卖出金额,isnull((select top 1 c.I_JYFMC as 交易方名称 from (select b.T_YSTBDDLYX as 交易方账号,sum(a.T_DJHKJE) as 卖出金额 from AAA_THDYFHDXXB as a join AAA_ZBDBXXB as b on a.ZBDBXXBBH=b.Number where a.F_BUYWYYSHCZSJ is not null and b.T_YSTBDGLJJRYX=tab.GLJJRYX and b.Z_SPBH=prd.SPBH group by T_YSTBDDLYX,b.Z_SPBH) as tab left join AAA_DLZHXXB as c on c.B_DLYX=tab.交易方账号 order by 卖出金额 DESC),'--') as 最大卖家名称,isnull((select top 1 c.I_SSQYS+c.I_SSQYSHI+c.I_SSQYQ as 所属区域 from (select b.T_YSTBDDLYX as 交易方账号,sum(a.T_DJHKJE) as 卖出金额 from AAA_THDYFHDXXB as a join AAA_ZBDBXXB as b on a.ZBDBXXBBH=b.Number where a.F_BUYWYYSHCZSJ is not null and b.T_YSTBDGLJJRYX=tab.GLJJRYX and b.Z_SPBH=prd.SPBH group by T_YSTBDDLYX,b.Z_SPBH) as tab left join AAA_DLZHXXB as c on c.B_DLYX=tab.交易方账号 order by 卖出金额 DESC),'--') as 最大卖家区域,isnull(c.买入金额,0.00)as 累计买入金额,isnull((select top 1 c.I_JYFMC as 交易方名称 from (select b.Y_YSYDDDLYX as 交易方账号,sum(a.T_DJHKJE) as 买入金额 from AAA_THDYFHDXXB as a join AAA_ZBDBXXB as b on a.ZBDBXXBBH=b.Number where a.F_BUYWYYSHCZSJ is not null and b.Y_YSYDDGLJJRYX=tab.GLJJRYX and b.Z_SPBH=prd.SPBH  group by b.Y_YSYDDDLYX) as tab left join AAA_DLZHXXB as c on c.B_DLYX=tab.交易方账号  order by 买入金额 DESC),'--') as 最大买家名称,isnull((select top 1 c.I_SSQYS+c.I_SSQYSHI+c.I_SSQYQ as 所属区域 from (select b.Y_YSYDDDLYX as 交易方账号,sum(a.T_DJHKJE) as 买入金额 from AAA_THDYFHDXXB as a join AAA_ZBDBXXB as b on a.ZBDBXXBBH=b.Number where a.F_BUYWYYSHCZSJ is not null and b.Y_YSYDDGLJJRYX=tab.GLJJRYX and b.Z_SPBH=prd.SPBH  group by b.Y_YSYDDDLYX) as tab left join AAA_DLZHXXB as c on c.B_DLYX=tab.交易方账号  order by 买入金额 DESC),'--')  as 最大买家区域 from AAA_PTSPXXB as prd left join (select distinct spbh,GLJJRYX from AAA_TBD where ZT<>'撤销' union  select distinct spbh,GLJJRYX from AAA_YDDXXB where ZT<>'撤销' ) as tab on tab.SPBH=prd.SPBH left join (select b.T_YSTBDGLJJRYX as 关联经纪人账号,b.Z_SPBH,sum(a.T_DJHKJE) as 卖出金额 from AAA_THDYFHDXXB as a left join AAA_ZBDBXXB as b on a.ZBDBXXBBH=b.Number where a.F_BUYWYYSHCZSJ is not null group by b.T_YSTBDGLJJRYX,b.Z_SPBH) as b on prd.SPBH=b.Z_SPBH and b.关联经纪人账号=tab.GLJJRYX left join (select b.Y_YSYDDGLJJRYX as 关联经纪人账号,b.Z_SPBH,sum(a.T_DJHKJE) as 买入金额 from AAA_THDYFHDXXB as a left join AAA_ZBDBXXB as b on a.ZBDBXXBBH=b.Number where a.F_BUYWYYSHCZSJ is not null group by b.Y_YSYDDGLJJRYX,b.Z_SPBH) as c on c.关联经纪人账号=tab.GLJJRYX and c.Z_SPBH=tab.SPBH  and c.关联经纪人账号=tab.GLJJRYX where " + ht_where["search_str_where"].ToString() + "order by isnull(c.买入金额,0.00) desc,isnull(b.卖出金额,0.00) DESC";
            //cMyXls1.BeginRunFrom_ht_where(sql, HideColumns);

            Hashtable ht_export = new Hashtable();
            ht_export["filename"] = "交易商品分析";
            ht_export["webmethod"] = "经纪人业务C区导出";
            ht_export["tiaojian"] = (Hashtable)ht_where["tiaojian"];            

            cMyXls1.BeginRunFrom_ht_where(ht_export);
        }



    }
}
