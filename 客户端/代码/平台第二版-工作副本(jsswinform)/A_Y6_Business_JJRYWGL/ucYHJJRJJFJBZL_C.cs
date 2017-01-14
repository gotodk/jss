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
    public partial class ucYHJJRJJFJBZL_C : UserControl
    {

        /// <summary>
        /// 查询条件
        /// </summary>
        Hashtable ht_where = new Hashtable();

        public ucYHJJRJJFJBZL_C()
        {
            InitializeComponent();
            //初始化分页回调(带分页的页面，先把这个放上)
            ucPager1.DFT = new delegateForThread(STR_BeginBind);
        }
        string tab = "";

        /// <summary>
        /// 设置默认搜索条件(这个例子中的哈希表键值一个也不能少)
        /// </summary>
        private void setDefaultSearch()
        {
            //string dlyx = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString();
            //ht_where["GetCustomersDataPage_NAME"] = "GetCustomersDataPage";  //每页显示的条数(必须设置)
            //ht_where["page_index"] = "0";
            //ht_where["page_size"] = "25";  //每页显示的条数(必须设置)
            //ht_where["serach_Row_str"] = " tab.*,(case 是否当前关联 when '否' then '--' else isnull(d.YGLSJG,'--') end) as 分支机构,(case 是否当前关联 when '否' then '--' else isnull(c.GLJJRXSYGGH,'--') end) as 员工工号,(case 是否当前关联 when '否' then '--' else isnull(d.YGXM,'--') end) as 员工姓名,'' as 账户状态 "; //检索字段(必须设置)
            //ht_where["search_tbname"] = " AAA_View_JJRGLJYFinfo as tab left join AAA_MJMJJYZHYJJRZHGLB_FZB c on c.Number=tab.Number left join AAA_YHRYXXB  d on c.GLJJRDLZH =d.YHDLZH and c.GLJJRXSYGGH =d.YGGH ";  //检索的表(必须设置)
            //ht_where["search_mainid"] = " tab.Number ";  //所检索表的主键(必须设置)
            //ht_where["search_str_where"] = " 结算账户类型='买家卖家交易账户' and 关联经纪人账号='" + PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString() + "' ";  //检索条件(必须设置)
            //ht_where["search_paixu"] = " DESC ";  //排序方式(必须设置)
            //ht_where["search_paixuZD"] = " tab.Number ";  //用于排序的字段(必须设置)
            //ht_where["method_retreatment"] = "JJRYWGL_C|JYFJBZL"; //用于处理账户状态字段值

            ht_where["page_index"] = "0";
            ht_where["page_size"] = "25";  //每页显示的条数(必须设置)
            ht_where["webmethod"] = "经纪人业务C区";
            Hashtable ht_tiaojian = new Hashtable();
            ht_tiaojian["标签名"] = "银行交易方基本资料";
            ht_tiaojian["用户邮箱"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString();
            ht_tiaojian["交易方名称"] = txtJYFMC.Text.Trim();
            string[] ssq = ucCityList1.SelectedItem;
            ht_tiaojian["省份"] = ssq[0].ToString();
            ht_tiaojian["地市"] = ssq[1].ToString();
            ht_tiaojian["区县"] = ssq[2].ToString();
            ht_tiaojian["分支机构"] = txtFZJG.Text.Trim();
            ht_tiaojian["员工姓名"] = txtYGXM.Text.Trim();
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

            //更改搜索条件
            setDefaultSearch();
            //if (txtJYFMC.Text.Trim() != "")
            //{
            //    ht_where["search_str_where"] = ht_where["search_str_where"] + " and 交易方名称 like '%" + txtJYFMC.Text.Trim() + "%' ";  //检索条件(必须设置)
            //}
            //if (this.txtFZJG.Text.Trim() != "")
            //{
            //    ht_where["search_str_where"] = ht_where["search_str_where"] + " and d.YGLSJG like '%" + txtFZJG.Text.Trim() + "%' ";
            //}
            //if (this.txtYGXM.Text.Trim() != "")
            //{
            //    ht_where["search_str_where"] = ht_where["search_str_where"] + " and d.YGXM like '%" + txtYGXM.Text.Trim() + "%' ";
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
            //    sql_sqq = sql_sqq + " and  所属区县='" + ssq[2] + "' ";
            //}
            //ht_where["search_str_where"] = ht_where["search_str_where"] + " " + sql_sqq + "  ";            
            //执行查询
            GetData(ht_where);
        }
        private void ucJJFJBZL_C_Load(object sender, EventArgs e)
        {
            ucCityList1.initdefault();
            //cbxFSJL.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.CB_DrawItem);
            //cbxFSJL.SelectedIndex = 0;
            //cbxSFDQGL.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.CB_DrawItem);
            //cbxSFDQGL.SelectedIndex = 0;

            GetData(null);

        }
        /// <summary>
        /// 处理单击详情的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;

            if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
                //设置默认收货地址的功能
                if (dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null && dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "资料详情")
                {
                    Hashtable hashTable = new Hashtable();
                    hashTable["JSBH"] = dgv.Rows[e.RowIndex].Cells[15].Value;
                    hashTable["DLYX"] = dgv.Rows[e.RowIndex].Cells[1].Value;
                    hashTable["JYFMC"] = dgv.Rows[e.RowIndex].Cells[16].Value;
                    hashTable["GLJJRBH"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["经纪人角色编号"].ToString();
                    hashTable["关联表Number"] = dgv.Rows[e.RowIndex].Cells[17].Value;
                    hashTable["SHYJ"] = "";
                    hashTable["当前经纪人名称"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["交易方名称"].ToString();
                    FormMMJDetailsInfo fm = new FormMMJDetailsInfo(hashTable);
                    fm.ShowDialog();
                }
            }
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExport_Click(object sender, EventArgs e)
        {
            ////设置要隐藏的列(原分页中，有些列可能是隐藏不显示的，导出时，可以选择滤掉)
            //string[] HideColumns = new string[] { "状态", "卖家角色编号", "省", "市", "区", "是否冻结", "是否休眠", "是否暂停用户新业务", "Number", "CreateTime" };
            ////设置导出语句
            //string sql = " select * from  " + tab + "  where 1=1 ";
            //if (txtJYFMC.Text.Trim() != "")
            //{
            //    sql = sql + " and 交易方名称 like '%" + txtJYFMC.Text.Trim() + "%' ";  //检索条件(必须设置)
            //}

            //string[] ssq = ucCityList1.SelectedItem;
            //string sql_sqq = "  ";
            //if (ssq[0] != "请选择省份")
            //{
            //    sql_sqq = sql_sqq + " and  省='" + ssq[0] + "' ";
            //}
            //if (ssq[1] != "请选择城市")
            //{
            //    sql_sqq = sql_sqq + " and  市='" + ssq[1] + "' ";
            //}
            //if (ssq[2] != "请选择区县")
            //{
            //    sql_sqq = sql_sqq + " and  区='" + ssq[2] + "' ";
            //}
            ////if (this.cbxFSJL.Text.ToString() != "请选择")
            ////{
            ////    sql_sqq = sql_sqq + " and  复审记录='" + this.cbxFSJL.Text.ToString() + "' ";
            ////}
            ////if (this.cbxSFDQGL.Text.ToString() != "请选择")
            ////{
            ////    sql_sqq = sql_sqq + " and  是否当前关联='" + this.cbxSFDQGL.Text.ToString() + "' ";
            ////}
            //sql = sql + " " + sql_sqq + " order by  CreateTime desc";

            //string sql = "select tab.交易方账号,tab.交易方名称,(case 是否当前关联 when '否' then '--' else isnull(d.YGLSJG,'--') end) as 分支机构,(case 是否当前关联 when '否' then '--' else isnull(c.GLJJRXSYGGH,'--') end) as 员工工号,(case 是否当前关联 when '否' then '--' else isnull(d.YGXM,'--') end) as 员工姓名,tab.是否当前关联,tab.所属区域,tab.注册类型,tab.联系人,tab.联系电话,tab.初审记录,tab.复审记录,(case (case tab.是否冻结 when '否' then '' else '冻结、' end)+(case tab.是否休眠 when '否' then '' else '休眠、' end)+(case tab.是否暂停用户新业务 when '否' then '' else '暂停新业务、' end) when '' then '正常' else SUBSTRING((case tab.是否冻结 when '否' then '' else '冻结、' end)+(case tab.是否休眠 when '否' then '' else '休眠、' end)+(case tab.是否暂停用户新业务 when '否' then '' else '暂停新业务、' end),1,LEN((case tab.是否冻结 when '否' then '' else '冻结、' end)+(case tab.是否休眠 when '否' then '' else '休眠、' end)+(case tab.是否暂停用户新业务 when '否' then '' else '暂停新业务、' end))-1) end) as 账户状态 from AAA_View_JJRGLJYFinfo as tab left join AAA_MJMJJYZHYJJRZHGLB_FZB c on c.Number=tab.Number left join AAA_YHRYXXB  d on c.GLJJRDLZH =d.YHDLZH and c.GLJJRXSYGGH =d.YGGH where " + ht_where["search_str_where"].ToString() + " order by tab.Number DESC";
            //string[] HideColumns = new string[] { };
            //cMyXls1.BeginRunFrom_ht_where(sql, HideColumns);

            Hashtable ht_export = new Hashtable();
            ht_export["filename"] = "银行交易方基本资料";
            ht_export["webmethod"] = "经纪人业务C区导出";
            ht_export["tiaojian"] = (Hashtable)ht_where["tiaojian"];

            cMyXls1.BeginRunFrom_ht_where(ht_export);
        }
    }
}                                       
                                        
                                        