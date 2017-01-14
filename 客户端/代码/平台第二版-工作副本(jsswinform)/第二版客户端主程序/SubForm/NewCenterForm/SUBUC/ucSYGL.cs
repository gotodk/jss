using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using 客户端主程序.DataControl;
using 客户端主程序.NewDataControl;
using System.Threading;

namespace 客户端主程序.SubForm.NewCenterForm.SUBUC
{
    public partial class ucSYGL : UserControl
    {
        /// <summary>
        /// 查询条件
        /// </summary>
        Hashtable ht_where = new Hashtable();
        DataTable dtXMXZ = new DataTable();
        public ucSYGL()
        {
            InitializeComponent();
            //初始化分页回调(带分页的页面，先把这个放上)
            ucPager1.DFT = new delegateForThread(STR_BeginBind);
        }

        private void ucJJRSY_C_Load(object sender, EventArgs e)
        {
         
            //显示默认数据
            GetData(null);
        }

        /// <summary>
        /// 设置默认搜索条件(这个例子中的哈希表键值一个也不能少)
        /// </summary>
        private void setDefaultSearch()
        {
            ////获取登陆者的登陆邮箱
            //string JJRJSBH = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["经纪人角色编号"].ToString();

            //ht_where["GetCustomersDataPage_NAME"] = "GetCustomersDataPage";  //每页显示的条数(必须设置)
            //ht_where["page_index"] = "0";
            //ht_where["page_size"] = "25";  //每页显示的条数(必须设置)
            //ht_where["serach_Row_str"] = "  银行人员信息表员工隶属机构,中标定标信息表辅助表员工工号,银行人员信息表员工工号,银行人员信息表员工姓名,SUM(金额) 收益累计 "; //检索字段(必须设置)
            //ht_where["search_tbname"] = "  (select isnull(yhryxxb.YGLSJG,'--') 银行人员信息表员工隶属机构,isnull(yhryxxb.YGGH,'--') 银行人员信息表员工工号,isnull(yhryxxb.YGXM,'--') 银行人员信息表员工姓名,isnull(zkmx.JE,0.00) 金额, isnull(fzb.YSTBDGLJJRXSYGGH,'--') 中标定标信息表辅助表员工工号 from AAA_ZKLSMXB zkmx left join AAA_THDYFHDXXB thd  on thd.number = zkmx.LYDH left join AAA_ZBDBXXB_FZB fzb on fzb.Number=thd.ZBDBXXBBH left join AAA_YHRYXXB yhryxxb on yhryxxb.YGGH=fzb.YSTBDGLJJRXSYGGH and yhryxxb.YHDLZH=fzb.YSTBDGLJJRDLYX  where  XM='经纪人收益' and SJLX='预' and LYYWLX='AAA_THDYFHDXXB' and zkmx.XZ='来自卖方收益' and JSBH='" + JJRJSBH.Trim() + "' union all select isnull(yhryxxb.YGLSJG,'--') 银行人员信息表员工隶属机构,isnull(yhryxxb.YGGH,'--') 银行人员信息表员工工号,isnull(yhryxxb.YGXM,'--') 银行人员信息表员工姓名,isnull(zkmx.JE,0.00) 金额, isnull(fzb.YSYDDGLJJRXSYGGH,'--') 中标定标信息表辅助表员工工号 from AAA_ZKLSMXB zkmx left join AAA_THDYFHDXXB thd  on thd.number = zkmx.LYDH left join AAA_ZBDBXXB_FZB fzb on fzb.Number=thd.ZBDBXXBBH left join AAA_YHRYXXB yhryxxb on yhryxxb.YGGH=fzb.YSYDDGLJJRXSYGGH and yhryxxb.YHDLZH=fzb.YSYDDGLJJRDLYX  where  XM='经纪人收益' and SJLX='预' and LYYWLX='AAA_THDYFHDXXB'  and zkmx.XZ='来自买方收益' and JSBH='" + JJRJSBH.Trim() + "'  union all  select isnull(yhryxxb.YGLSJG,'--') 银行人员信息表员工隶属机构,isnull(yhryxxb.YGGH,'--') 银行人员信息表员工工号,isnull(yhryxxb.YGXM,'--') 银行人员信息表员工姓名,isnull(zkmx.JE,0.00) 金额,isnull( fzb.YSTBDGLJJRXSYGGH ,'--')中标定标信息表辅助表员工工号 from AAA_ZKLSMXB zkmx left join AAA_ZBDBXXB_FZB fzb on fzb.Number=zkmx.LYDH left join AAA_YHRYXXB yhryxxb on yhryxxb.YGGH=fzb.YSTBDGLJJRXSYGGH and yhryxxb.YHDLZH=fzb.YSTBDGLJJRDLYX  where  XM='经纪人收益' and SJLX='预' and LYYWLX='AAA_ZBDBXXB' and zkmx.XZ='来自卖方收益' and JSBH='" + JJRJSBH.Trim() + "'  union all  select isnull(yhryxxb.YGLSJG,'--') 银行人员信息表员工隶属机构,isnull(yhryxxb.YGGH,'--') 银行人员信息表员工工号,isnull(yhryxxb.YGXM,'--') 银行人员信息表员工姓名,isnull(zkmx.JE,0.00) 金额, isnull(fzb.YSYDDGLJJRXSYGGH ,'--') 中标定标信息表辅助表员工工号 from AAA_ZKLSMXB zkmx left join AAA_ZBDBXXB_FZB fzb on fzb.Number=zkmx.LYDH left join AAA_YHRYXXB yhryxxb on yhryxxb.YGGH=fzb.YSYDDGLJJRXSYGGH and yhryxxb.YHDLZH=fzb.YSYDDGLJJRDLYX  where  XM='经纪人收益' and SJLX='预' and LYYWLX='AAA_ZBDBXXB' and zkmx.XZ='来自买方收益' and JSBH='" + JJRJSBH.Trim() + "')as tab1   ";  //检索的表(必须设置)
            //ht_where["search_mainid"] = " 中标定标信息表辅助表员工工号 ";  //所检索表的主键(必须设置)
            //ht_where["search_str_where"] = " 1=1  ";  //检索条件(必须设置)
            //ht_where["search_paixu"] = " DESC ";  //排序方式(必须设置)
            //ht_where["search_paixuZD"] = " SUM(金额) ";  //用于排序的字段(必须设置)
            //if (txtFZJG.Text.Trim()!="")
            //{
            //    ht_where["search_str_where"] += " and 银行人员信息表员工隶属机构 like '%" + txtFZJG.Text.Trim() + "%'";
            //}
            //if (txtYGGH.Text.Trim() != "")
            //{
            //    ht_where["search_str_where"] += " and 中标定标信息表辅助表员工工号 like '%" + txtYGGH.Text.Trim() + "%'";
            //}
            //if (txtYGXM.Text.Trim() != "")
            //{
            //    ht_where["search_str_where"] += " and 银行人员信息表员工姓名 like '%" + txtYGXM.Text.Trim() + "%'";
            //}
            //ht_where["search_str_where"] += " group by 中标定标信息表辅助表员工工号,银行人员信息表员工工号,银行人员信息表员工隶属机构,银行人员信息表员工工号,银行人员信息表员工姓名 ";

           
            ht_where["page_index"] = "0";
            ht_where["page_size"] = "25";  //每页显示的条数(必须设置)
            ht_where["webmethod"] = "银行经纪人C区收益管理";
            Hashtable ht_tiaojian = new Hashtable();
            ht_tiaojian["标签名"] = "收益管理";
            ht_tiaojian["经纪人编号"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["经纪人角色编号"].ToString();
            ht_tiaojian["分支机构"] = txtFZJG.Text.Trim();
            ht_tiaojian["员工姓名"] = txtYGXM.Text.Trim();
            ht_tiaojian["员工工号"] = txtYGGH.Text.Trim();
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
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            //执行查询
            GetData(null);
        }
        //导出操作
        private void btnExport_Click(object sender, EventArgs e)
        {
            ////设置要隐藏的列(原分页中，有些列可能是隐藏不显示的，导出时，可以选择滤掉)
            //string[] HideColumns = new string[] { "" };
            //string JJRJSBH = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["经纪人角色编号"].ToString();

            ////设置导出语句
            //string sql = "select 银行人员信息表员工隶属机构 as 分支机构,中标定标信息表辅助表员工工号 as 员工工号,银行人员信息表员工姓名 as 员工姓名,SUM(金额) 收益累计 from ( select isnull(yhryxxb.YGLSJG,'--') 银行人员信息表员工隶属机构,isnull(yhryxxb.YGGH,'--') 银行人员信息表员工工号,isnull(yhryxxb.YGXM,'--') 银行人员信息表员工姓名,isnull(zkmx.JE,0.00) 金额, isnull(fzb.YSTBDGLJJRXSYGGH,'--') 中标定标信息表辅助表员工工号 from AAA_ZKLSMXB zkmx left join AAA_THDYFHDXXB thd  on thd.number = zkmx.LYDH left join AAA_ZBDBXXB_FZB fzb on fzb.Number=thd.ZBDBXXBBH left join AAA_YHRYXXB yhryxxb on yhryxxb.YGGH=fzb.YSTBDGLJJRXSYGGH and yhryxxb.YHDLZH=fzb.YSTBDGLJJRDLYX  where  XM='经纪人收益' and SJLX='预' and LYYWLX='AAA_THDYFHDXXB'  and zkmx.XZ='来自卖方收益' and JSBH='" + JJRJSBH.Trim() + "' union all select isnull(yhryxxb.YGLSJG,'--') 银行人员信息表员工隶属机构,isnull(yhryxxb.YGGH,'--') 银行人员信息表员工工号,isnull(yhryxxb.YGXM,'--') 银行人员信息表员工姓名,isnull(zkmx.JE,0.00) 金额,isnull( fzb.YSYDDGLJJRXSYGGH,'--') 中标定标信息表辅助表员工工号 from AAA_ZKLSMXB zkmx left join AAA_THDYFHDXXB thd  on thd.number = zkmx.LYDH left join AAA_ZBDBXXB_FZB fzb on fzb.Number=thd.ZBDBXXBBH left join AAA_YHRYXXB yhryxxb on yhryxxb.YGGH=fzb.YSYDDGLJJRXSYGGH and yhryxxb.YHDLZH=fzb.YSYDDGLJJRDLYX  where  XM='经纪人收益' and SJLX='预' and LYYWLX='AAA_THDYFHDXXB'  and zkmx.XZ='来自买方收益' and JSBH='" + JJRJSBH.Trim() + "'  union all  select isnull(yhryxxb.YGLSJG,'--') 银行人员信息表员工隶属机构,isnull(yhryxxb.YGGH,'--') 银行人员信息表员工工号,isnull(yhryxxb.YGXM,'--') 银行人员信息表员工姓名,isnull(zkmx.JE,0.00) 金额,isnull( fzb.YSTBDGLJJRXSYGGH ,'--')中标定标信息表辅助表员工工号 from AAA_ZKLSMXB zkmx left join AAA_ZBDBXXB_FZB fzb on fzb.Number=zkmx.LYDH left join AAA_YHRYXXB yhryxxb on yhryxxb.YGGH=fzb.YSTBDGLJJRXSYGGH and yhryxxb.YHDLZH=fzb.YSTBDGLJJRDLYX  where  XM='经纪人收益' and SJLX='预' and LYYWLX='AAA_ZBDBXXB'  and zkmx.XZ='来自卖方收益' and JSBH='" + JJRJSBH.Trim() + "'  union all  select isnull(yhryxxb.YGLSJG,'--') 银行人员信息表员工隶属机构,isnull(yhryxxb.YGGH,'--') 银行人员信息表员工工号,isnull(yhryxxb.YGXM,'--') 银行人员信息表员工姓名,isnull(zkmx.JE,0.00) 金额, isnull(fzb.YSYDDGLJJRXSYGGH ,'--') 中标定标信息表辅助表员工工号 from AAA_ZKLSMXB zkmx left join AAA_ZBDBXXB_FZB fzb on fzb.Number=zkmx.LYDH left join AAA_YHRYXXB yhryxxb on yhryxxb.YGGH=fzb.YSYDDGLJJRXSYGGH and yhryxxb.YHDLZH=fzb.YSYDDGLJJRDLYX  where  XM='经纪人收益' and SJLX='预' and LYYWLX='AAA_ZBDBXXB'  and zkmx.XZ='来自买方收益' and JSBH='" + JJRJSBH.Trim() + "') as tab  ";
            //if (txtFZJG.Text.Trim() != "")
            //{
            //    sql += " and 银行人员信息表员工隶属机构 like '%" + txtFZJG.Text.Trim() + "%'";
            //}
            //if (txtYGGH.Text.Trim() != "")
            //{
            //    sql += " and 中标定标信息表辅助表员工工号 like '%" + txtYGGH.Text.Trim() + "%'";
            //}
            //if (txtYGXM.Text.Trim() != "")
            //{
            //    sql += " and 银行人员信息表员工姓名 like '%" + txtYGXM.Text.Trim() + "%'";
            //}
            //sql = sql + " group by 中标定标信息表辅助表员工工号,银行人员信息表员工工号,银行人员信息表员工隶属机构,银行人员信息表员工工号,银行人员信息表员工姓名  order by SUM(金额) desc ";
            //cMyXls1.BeginRunFrom_ht_where(sql, HideColumns);

            Hashtable ht_export = new Hashtable();
            ht_export["filename"] = "收益管理";
            ht_export["webmethod"] = "银行经纪人C区收益管理导出";
            ht_export["tiaojian"] = (Hashtable)ht_where["tiaojian"];         

            cMyXls1.BeginRunFrom_ht_where(ht_export);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex==4&&e.RowIndex!=-1)
            {
                int rowIndex = e.RowIndex;
                Hashtable hashTable = new Hashtable();
                hashTable["员工工号"] = dataGridView1.Rows[rowIndex].Cells["员工工号"].Value.ToString();
                fmSYXQ fm = new fmSYXQ(null, hashTable);
                fm.ShowDialog();
            }
        }
        

        
    }
}
