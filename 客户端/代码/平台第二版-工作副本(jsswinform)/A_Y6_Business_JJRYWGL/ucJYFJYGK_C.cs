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
    public partial class ucJYFJYGK_C : UserControl
    {
            /// <summary>
        /// 查询条件
        /// </summary>
        Hashtable ht_where = new Hashtable();
        public ucJYFJYGK_C()
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
            //string tab_ljsy = "(select (case XZ when '来自买方收益' then c.Y_YSYDDGLJJRYX when '来自卖方收益' then c.T_YSTBDGLJJRYX end) as 关联经纪人,(case XZ when '来自买方收益' then c.Y_YSYDDDLYX when '来自卖方收益' then c.T_YSTBDDLYX end) as 交易方账号,sum(JE) as 收益金额 from AAA_ZKLSMXB as a left join AAA_THDYFHDXXB as b on a.LYDH=b.Number left join AAA_ZBDBXXB as c on c.Number=b.ZBDBXXBBH where (XZ='来自买方收益' or XZ='来自卖方收益') and SJLX='预' group by (case XZ when '来自买方收益' then c.Y_YSYDDDLYX when '来自卖方收益' then c.T_YSTBDDLYX end),(case XZ when '来自买方收益' then c.Y_YSYDDGLJJRYX when '来自卖方收益' then c.T_YSTBDGLJJRYX end))";
            //ht_where["GetCustomersDataPage_NAME"] = "GetCustomersDataPage";  //每页显示的条数(必须设置)
            //ht_where["page_index"] = "0";
            //ht_where["page_size"] = "25";  //每页显示的条数(必须设置)
            //ht_where["serach_Row_str"] = " a.关联经纪人账号,a.交易方账号,a.交易方名称,a.结算账户类型,a.是否当前关联,a.所属区域,a.注册类型,isnull(b.收益金额,0) as 经纪人累计收益,'' as 累计卖出金额,'' as 累计买入金额,'' as 累计竞标次数,'' as 累计中标次数,'' as 已发货待付款金额 "; //检索字段(必须设置)
            //ht_where["search_tbname"] = " AAA_View_JJRGLJYFinfo as a left join "+tab_ljsy+" as b on a.交易方账号=b.交易方账号 and a.关联经纪人账号=b.关联经纪人 ";  //检索的表(必须设置)
            //ht_where["search_mainid"] = " a.交易方账号 ";  //所检索表的主键(必须设置)
            //ht_where["search_str_where"] = " a.关联经纪人账号='" + PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString() + "' ";  //检索条件(必须设置)
            //ht_where["search_paixu"] = " asc ";  //排序方式(必须设置)
            //ht_where["search_paixuZD"] = " isnull(b.收益金额,0) DESC,a.交易方账号";  //用于排序的字段(必须设置)
            //ht_where["method_retreatment"] = "JJRYWGL_C|JYFJYGK"; //用于处理账户状态字段值

            ht_where["page_index"] = "0";
            ht_where["page_size"] = "25";  //每页显示的条数(必须设置)
            ht_where["webmethod"] = "经纪人业务C区";
            Hashtable ht_tiaojian = new Hashtable();
            ht_tiaojian["标签名"] = "交易方交易概况";
            ht_tiaojian["用户邮箱"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString();
            ht_tiaojian["交易方名称"] = txtJYF.Text.Trim();
            string[] ssq = ucCityList1.SelectedItem;
            ht_tiaojian["省份"] =ssq[0].ToString ();
            ht_tiaojian["地市"] = ssq[1].ToString();
            ht_tiaojian["区县"] = ssq[2].ToString();
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
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {


        }
        private void ucJYFJYGK_C_Load(object sender, EventArgs e)
        {
            ucCityList1.initdefault();
            GetData(null);
        }
       
        private void btnSearch_Click_1(object sender, EventArgs e)
        {
            //更改搜索条件
            setDefaultSearch();
            //if (txtJYF.Text.Trim() != "")
            //{
            //    ht_where["search_str_where"] = ht_where["search_str_where"]  + " and a.交易方名称 like '%" + txtJYF.Text.Trim() + "%' ";  //检索条件(必须设置)
            //}
            //string[] ssq = ucCityList1.SelectedItem;
            //string sql_sqq = "";
            //if (ssq[0] != "请选择省份")
            //{
            //    sql_sqq = sql_sqq + " and  所属省='" + ssq[0] + "' ";
            //}
            //if (ssq[1] != "请选择城市")
            //{
            //    sql_sqq = sql_sqq + " and  所属市='" + ssq[1] + "' ";
            //}
            //if (ssq[2] != "请选择区县")
            //{
            //    sql_sqq = sql_sqq + " and  所属区='" + ssq[2] + "' ";
            //}
            //ht_where["search_str_where"] = ht_where["search_str_where"] + " " + sql_sqq + "  "; 
            //执行查询
            GetData(ht_where);
        }
        private void btnExport_Click(object sender, EventArgs e)
        {
            //string sql = "select a.交易方账号,a.交易方名称,a.是否当前关联,a.所属区域,a.注册类型,(case a.结算账户类型 when '经纪人交易账户' then '--' else cast(isnull(d.累计卖出金额,0.00) as varchar(20)) end) as 累计卖出金额,isnull(e.累计买入金额,0.00) as 累计买入金额, isnull(b.收益金额,0) as 经纪人累计收益,f.累计竞标次数,(select count(*) from AAA_ZBDBXXB where (Y_YSYDDDLYX = a.交易方账号 and Y_YSYDDGLJJRYX=a.关联经纪人账号) or (T_YSTBDDLYX = a.交易方账号 and T_YSTBDGLJJRYX=a.关联经纪人账号)) as 累计中标次数,(select isnull(sum(aa.T_DJHKJE),0.00) from  AAA_THDYFHDXXB as aa left join AAA_ZBDBXXB as bb on aa.ZBDBXXBBH = bb.Number where ((bb.T_YSTBDDLYX =a.交易方账号 and bb.T_YSTBDGLJJRYX=a.关联经纪人账号) or (bb.Y_YSYDDDLYX=a.交易方账号 and bb.Y_YSYDDGLJJRYX=a.关联经纪人账号)) and (aa.F_WLXXLRSJ is not null) and (aa.F_BUYWYYSHCZSJ is null)) as 已发货待付款金额 from AAA_View_JJRGLJYFinfo as a left join (select (case XZ when '来自买方收益' then c.Y_YSYDDGLJJRYX when '来自卖方收益' then c.T_YSTBDGLJJRYX end) as 关联经纪人,(case XZ when '来自买方收益' then c.Y_YSYDDDLYX when '来自卖方收益' then c.T_YSTBDDLYX end) as 交易方账号,sum(JE) as 收益金额 from AAA_ZKLSMXB as a left join AAA_THDYFHDXXB as b on a.LYDH=b.Number left join AAA_ZBDBXXB as c on c.Number=b.ZBDBXXBBH where (XZ='来自买方收益' or XZ='来自卖方收益') and SJLX='预' group by  (case XZ when '来自买方收益' then c.Y_YSYDDDLYX when '来自卖方收益' then c.T_YSTBDDLYX end),(case XZ when '来自买方收益' then c.Y_YSYDDGLJJRYX when '来自卖方收益' then c.T_YSTBDGLJJRYX end)) as b on a.交易方账号=b.交易方账号 and a.关联经纪人账号=b.关联经纪人 left join (select b.T_YSTBDDLYX as 交易方账号,b.T_YSTBDGLJJRYX as 关联经济人,sum(a.T_DJHKJE) as 累计卖出金额 from  AAA_THDYFHDXXB as a left join AAA_ZBDBXXB as b on a.ZBDBXXBBH = b.Number where a.F_BUYWYYSHCZSJ is not null group by  b.T_YSTBDDLYX ,b.T_YSTBDGLJJRYX) as d on d.交易方账号= a.交易方账号 and d.关联经济人=a.关联经纪人账号 left join (select b.Y_YSYDDDLYX as 交易方账号,T_YSTBDGLJJRYX as 关联经纪人,sum(a.T_DJHKJE) as 累计买入金额 from  AAA_THDYFHDXXB as a left join AAA_ZBDBXXB as b on a.ZBDBXXBBH = b.Number where a.F_BUYWYYSHCZSJ is not null group by  b.Y_YSYDDDLYX,b.T_YSTBDGLJJRYX ) as e on e.交易方账号=a.交易方账号 and e.关联经纪人=a.关联经纪人账号 left join (select 交易方账号,关联经纪人,count(*) as 累计竞标次数 from (select Number, DLYX as 交易方账号,GLJJRYX as 关联经纪人 from AAA_TBD union all select Number, DLYX as 交易方账号,GLJJRYX as 关联经纪人  from AAA_YDDXXB) as tab group by 交易方账号,关联经纪人) as f on f.交易方账号=a.交易方账号 and f.关联经纪人=a.关联经纪人账号 where " + ht_where["search_str_where"].ToString() + " order by isnull(b.收益金额,0) DESC,a.交易方账号 ASC";
            //string[] HideColumns = new string[] { };
            //cMyXls1.BeginRunFrom_ht_where(sql, HideColumns);

            Hashtable ht_export = new Hashtable();
            ht_export["filename"] = "交易方交易概况";
            ht_export["webmethod"] = "经纪人业务C区导出";
            ht_export["tiaojian"] = (Hashtable)ht_where["tiaojian"]; 
         
            cMyXls1.BeginRunFrom_ht_where(ht_export);
        }

    }
}
