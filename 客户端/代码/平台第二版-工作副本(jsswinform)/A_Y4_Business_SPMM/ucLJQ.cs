using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using 客户端主程序.DataControl;

namespace 客户端主程序.SubForm.NewCenterForm.SUBUC.SPMM
{
    public partial class ucLJQ : UserControl
    {
        /// <summary>
        /// 查询条件
        /// </summary>
        Hashtable ht_where = new Hashtable();
       
        public ucLJQ()
        {
            InitializeComponent();
            //初始化分页回调(带分页的页面，先把这个放上)
            ucPager1.DFT = new delegateForThread(STR_BeginBind);
        }


        /// <summary>
        /// 设置默认搜索条件(这个例子中的哈希表键值一个也不能少)
        /// </summary>
        private void setDefaultSearch()
        {
            ht_where["GetCustomersDataPage_NAME"] = "GetCustomersDataPage";  //每页显示的条数(必须设置)
            ht_where["page_index"] = "0";
            ht_where["page_size"] = "25";  //每页显示的条数(必须设置)
            //ht_where["serach_Row_str"] = " * "; //检索字段(必须设置)
            //ht_where["search_tbname"] = " (" + StrGettbdsql + " union " + Stryddzhengchang + " ) as tab  ";
            #region  2013.05.09 dele wyh
            //ht_where["search_tbname"] = "  ((select ('T'+a.Number) as Number, a.CreateTime as '下单时间',a.SPBH as '商品编号',SPMC as '商品名称',GG as '规格',a.HTQX as '合同期限',TBNSL as '数量',TBJG as '价格',TBJE as '金额', case when (( select MIN(TBJG) from AAA_TBD  where  ZT= '竞标' and SPBH=a.SPBH and HTQX=a.HTQX )  )  is null then '---' else CAST ( (( select MIN(TBJG) from AAA_TBD  where  ZT= '竞标' and SPBH=a.SPBH and HTQX=a.HTQX ) ) as varchar(200)) end  as '当前卖家最低价', case when ( select top 1  MJSDJJPL from AAA_TBD  where TBJG=( select MIN(TBJG) from AAA_TBD  where  ZT= '竞标' and SPBH=a.SPBH and HTQX=a.HTQX )  and ZT= '竞标'  and SPBH=a.SPBH and HTQX=a.HTQX  order by MJSDJJPL, CreateTime ) IS NULL then '---' else CAST ( ( select top 1  MJSDJJPL from AAA_TBD  where TBJG=( select MIN(TBJG) from AAA_TBD  where  ZT= '竞标' and SPBH=a.SPBH and HTQX=a.HTQX )   and ZT= '竞标'  and SPBH=a.SPBH and HTQX=a.HTQX order by MJSDJJPL, CreateTime ) as varchar(200)) end   as '最低价标的经济批量',(CAST(ISNULL(((SELECT SUM(NDGSL) FROM AAA_YDDXXB WHERE AAA_YDDXXB.SPBH=a.SPBH AND AAA_YDDXXB.HTQX=a.HTQX AND AAA_YDDXXB.ZT='竞标' AND AAA_YDDXXB.NMRJG>= ( select MIN(TBJG) from AAA_TBD  where  ZT= '竞标' and SPBH=a.SPBH and HTQX=a.HTQX )  AND CHARINDEX('|'+SHQYsheng+'|',GHQY)>0)/ (SELECT TOP 1 TBNSL FROM AAA_TBD WHERE ZT='竞标' AND AAA_TBD.SPBH=a.SPBH AND AAA_TBD.HTQX = a.HTQX ORDER BY AAA_TBD.TBJG,AAA_TBD.CreateTime ASC)),0)*100 AS numeric(18, 2))) as '最低价标的拟售量达成率','投标单' as '单据类型',a.Number as '单据编号' , ZT as '单据状态','否' as '是否拆单',b.SFJRLJQ as '是否在冷静期',c.Z_HTZT AS '中标定标信息表状态', cast(a.DJTBBZJ as varchar(200) ) as '投标保证金冻结金额','---' as '订金冻结金额'  from AAA_TBD as a left join AAA_LJQDQZTXXB as b on a.SPBH=b.SPBH and a.HTQX = b.HTQX left join AAA_ZBDBXXB as c on a.Number = c.T_YSTBDBH   where MJJSBH = '" + PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["卖家角色编号"].ToString() + "' and ZT= '竞标' and b.SFJRLJQ = '是' and  c.Z_HTZT is null  )  union (select ('Y'+a.Number) as Number, a.CreateTime as '下单时间',a.SPBH as '商品编号',SPMC as '商品名称',GG as '规格',a.HTQX as '合同期限', a.NDGSL as '数量',a.NMRJG as '价格',a.NDGJE as  金额,( select MIN(TBJG) from AAA_TBD  where  ZT= '竞标' and SPBH=a.SPBH and HTQX=a.HTQX ) as '当前卖家最低价',( select top 1  MJSDJJPL from AAA_TBD  where TBJG=( select MIN(TBJG) from AAA_TBD  where  ZT= '竞标' and SPBH=a.SPBH and HTQX=a.HTQX )   and ZT= '竞标' order by CreateTime ) as '最低价标的经济批量',(CAST(ISNULL(((SELECT SUM(NDGSL) FROM AAA_YDDXXB WHERE AAA_YDDXXB.SPBH=a.SPBH AND AAA_YDDXXB.HTQX=a.HTQX AND AAA_YDDXXB.ZT='竞标' AND AAA_YDDXXB.NMRJG>= ( select MIN(TBJG) from AAA_TBD  where  ZT= '竞标' and SPBH=a.SPBH and HTQX=a.HTQX )  AND CHARINDEX('|'+SHQYsheng+'|',(select top 1  GHQY from AAA_TBD  where TBJG=( select MIN(TBJG) from AAA_TBD  where  ZT= '竞标' and SPBH=a.SPBH and HTQX=a.HTQX )   and ZT= '竞标' and SPBH=a.SPBH and HTQX=a.HTQX  order by CreateTime))>0)/ (SELECT TOP 1 TBNSL FROM AAA_TBD WHERE ZT='竞标' AND AAA_TBD.SPBH=a.SPBH AND AAA_TBD.HTQX = a.HTQX ORDER BY AAA_TBD.TBJG,AAA_TBD.CreateTime ASC)),0) AS numeric(18, 2)))*100  as '最低价标的拟售量达成率','预订单' as '单据类型' ,a.Number as '单据编号' , ZT as '单据状态', case  when YZBSL =0 then '否' when YZBSL>0 and YZBSL != NDGSL then '是' else '否'  end  as '是否拆单',b.SFJRLJQ as '是否在冷静期',c.Z_HTZT AS '中标定标信息表状态','---' as '投标保证金冻结金额', cast(a.DJDJ as varchar(200)) as '订金冻结金额'  from AAA_YDDXXB as a left join AAA_LJQDQZTXXB as b on a.SPBH=b.SPBH and a.HTQX = b.HTQX left join AAA_ZBDBXXB as c on a.Number = c.Y_YSYDDBH  where  ZT= '竞标' and   MJJSBH = '" + PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["买家角色编号"].ToString().Trim() + "' and b.SFJRLJQ = '是' and  c.Z_HTZT is null )) as tab  ";  //检索的表(必须设置)
            #endregion
           // ht_where["search_mainid"] = " Number ";  //所检索表的主键(必须设置)
            //ht_where["search_str_where"] = " 1=1 ";  //检索条件(必须设置)
           // ht_where["search_paixu"] = " desc ";  //排序方式(必须设置)
           // ht_where["search_paixuZD"] = " 下单时间 ";  //用于排序的字段(必须设置)

            string dlyx = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString().Trim();
            //ht_where["serach_Row_str"] = " ljq.LJQJSSJ as 冷静期结束时间,tab.*, '--' as 当前卖方最低价,'--' as 最低价标的经济批量,'--' as [最低价标的达成率/中标率]  "; //检索字段(必须设置)
            //ht_where["search_tbname"] = " AAA_View_SPJingBiao as tab left join AAA_LJQDQZTXXB  as ljq on ljq.SPBH=tab.商品编号 and ljq.HTQX=tab.合同期限 ";
            //ht_where["search_mainid"] = " tab.单据编号+tab.单据类型 ";  //所检索表的主键(必须设置)
            //ht_where["search_str_where"] = " tab.交易方账号='"+dlyx+"' and isnull(ljq.SFJRLJQ,'否')='是' ";  //检索条件(必须设置)
            //ht_where["search_paixu"] = " desc ";  //排序方式(必须设置)
            //ht_where["search_paixuZD"] = " tab.下单时间 ";  //用于排序的字段(必须设置)
            //ht_where["method_retreatment"] = "SPMM_C|LengJingQi"; //查询条件

            ht_where["page_index"] = "0";
            ht_where["page_size"] = "25";  //每页显示的条数(必须设置)
            ht_where["webmethod"] = "商品买卖C区";
            Hashtable ht_tiaojian = new Hashtable();
            ht_tiaojian["标签名"] = "冷静期";
            ht_tiaojian["用户邮箱"] = dlyx;
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
        private void GetData()
        {
            setDefaultSearch();
            Hashtable ht_tiaojian = (Hashtable)ht_where["tiaojian"];
            ht_tiaojian["商品名称"] = txtSPMC.Text.Trim();
            ht_tiaojian["单据类型"] = cbxDJLB.SelectedItem.ToString();

            ht_where["tiaojian"] = ht_tiaojian;
            //if (!txtSPMC.Text.Trim().Equals(""))
            //{
            //    ht_where["search_str_where"] += " and 商品名称 like '%" + txtSPMC.Text.Trim() + "%' ";
            //}
            //if (!cbxDJLB.SelectedItem.ToString().Trim().Equals("请选择单据类别"))
            //{
            //    ht_where["search_str_where"] += " and 单据类型='" + cbxDJLB.SelectedItem.ToString().Trim() + "' ";
            //}

            ucPager1.HT_Where = ht_where;
            ucPager1.BeginBindForUCPager();
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

        private void CBXM_DrawItem(object sender, DrawItemEventArgs e)
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
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {

            //更改搜索条件
            setDefaultSearch();
           
            //执行查询
            GetData();
        }
        private void ucLJQ_Load(object sender, EventArgs e)
        {
            //处理下拉框间距
            cbxDJLB.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.CB_DrawItem);
            cbxDJLB.SelectedIndex = 0;
            GetData();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {            
            //string dlyx = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString().Trim();
            //string StrSql = "select ljq.LJQJSSJ as 冷静期结束时间,tab.* from AAA_View_SPJingBiao as tab left join AAA_LJQDQZTXXB  as ljq on ljq.SPBH=tab.商品编号 and ljq.HTQX=tab.合同期限 where " + ht_where["search_str_where"].ToString() + " order by tab.下单时间 desc";
            //string[] HideColumns = new string[] {"交易方账号" };
            //cMyXls1.BeginRunFrom_ht_where(StrSql, HideColumns);

            Hashtable ht_export = new Hashtable();
            ht_export["filename"] = "冷静期";
            ht_export["webmethod"] = "商品买卖C区导出";
            ht_export["tiaojian"] = (Hashtable)ht_where["tiaojian"];
            //string[] HideColumns = new string[] { "交易方账号" };
            //ht_export["HideColumns"] = HideColumns;

            cMyXls1.BeginRunFrom_ht_where(ht_export);
        }
    }
}
