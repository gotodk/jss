using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using 客户端主程序.DataControl;
using 客户端主程序.SubForm.NewCenterForm.MuBan;

namespace 客户端主程序.SubForm.NewCenterForm.SUBUC.SPMM
{
    public partial class ucDBYBZH_C : UserControl
    {

        /// <summary>
        /// 查询条件
        /// </summary>
        Hashtable ht_where = new Hashtable();
        //string StrGettbdsql = "";
        //string Stryddzhengchang = "";
        //string Stryddchaidan = "";
        public ucDBYBZH_C()
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
            //ht_where["GetCustomersDataPage_NAME"] = "GetCustomersDataPage";  //每页显示的条数(必须设置)
            //ht_where["page_index"] = "0";
            //ht_where["page_size"] = "25";  //每页显示的条数(必须设置)
           
            string dlyx = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString().Trim();
            //ht_where["serach_Row_str"] = "  *,'违约扣罚记录' as 违约扣罚记录,(case 单据类型 when '预订单' then '--' else '100.00' end) as 履约保证金冻结剩余比例 ";
            //ht_where["search_tbname"] = " AAA_View_SPDingBiao as tab "; //检索的表
            //ht_where["search_mainid"] = " 中标定标编号+单据类型 ";  //所检索表的主键
            //ht_where["search_str_where"] = " 交易方账号='" + dlyx + "' ";  //检索条件  
            //ht_where["search_paixu"] = " ASC ";  //排序方式
            //ht_where["search_paixuZD"] = " 定标时间 ";  //用于排序的字段
            //ht_where["method_retreatment"] = "SPMM_C|DingBiao";

            ht_where["page_index"] = "0";
            ht_where["page_size"] = "25";  //每页显示的条数(必须设置)
            ht_where["webmethod"] = "商品买卖C区";
            Hashtable ht_tiaojian = new Hashtable();
            ht_tiaojian["标签名"] = "定标与保证函";
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
            //查询条件
            //if (!txtSPMC.Text.Trim().Equals(""))
            //{
            //    ht_where["search_str_where"] += " and 商品名称 like '%" + txtSPMC.Text.Trim() + "%' ";
            //}
            //if (!cbxDJLB.SelectedItem.ToString().Trim().Equals("请选择单据类别"))
            //{
            //    ht_where["search_str_where"] += " and 单据类型='" + cbxDJLB.SelectedItem.ToString().Trim() + "' ";
            //}

            Hashtable ht_tiaojian = (Hashtable)ht_where["tiaojian"];
            ht_tiaojian["商品名称"] = txtSPMC.Text.Trim();
            ht_tiaojian["单据类型"] = cbxDJLB.SelectedItem.ToString();

            ht_where["tiaojian"] = ht_tiaojian;
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

        private void ucDBYBZH_C_Load(object sender, EventArgs e)
        {
            //处理下拉框间距
            cbxDJLB.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.CB_DrawItem);
            cbxDJLB.SelectedIndex = 0;
            GetData();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string columnName = dataGridView1.Columns[e.ColumnIndex].Name;
            Hashtable httt = new Hashtable();
            if (columnName == "查看详情" && e.RowIndex > -1)
            {
                DataGridViewRow dgr = this.dataGridView1.Rows[e.RowIndex];
                string Strtype = dgr.Cells["单据类型"].Value.ToString().Trim();
                string Number = dgr.Cells["中标定标编号"].Value.ToString().Trim();
                Hashtable htNm = new Hashtable();
                htNm["Number"] = Number;
                if (Strtype.Equals("投标单"))
                {
                    FormDBYBZH tbdcg = new FormDBYBZH(htNm);
                    tbdcg.ShowDialog();
                }
                if (Strtype.Equals("预订单"))
                {
                    FormDBYBZH cgtj = new FormDBYBZH(htNm);
                    cgtj.ShowDialog();
                }


            }
            if (columnName == "违约扣罚记录" && e.RowIndex > -1)
            {

                DataGridViewRow dgr = this.dataGridView1.Rows[e.RowIndex];
                string Strtype = dgr.Cells["单据类型"].Value.ToString().Trim();
                string Number = dgr.Cells["中标定标编号"].Value.ToString().Trim();
                if (Strtype.Equals("投标单"))
                {
                    double dbkf = Convert.ToDouble(dgr.Cells["履约保证金冻结剩余比例"].Value.ToString().Trim());
                    if (dbkf != 100)
                    {
                        Hashtable htkf = new Hashtable();
                        htkf["Number"] = Number;
                        Fromwykfjl kfjl = new Fromwykfjl(htkf);
                        kfjl.ShowDialog();
                    }
                    else
                    {
                        ArrayList al = new ArrayList();
                        al.Add("");
                        al.Add("单据暂时没有可查询的违约扣罚记录！");
                        FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "中国商品批发交易平台", al);
                        fm.ShowDialog();
                        return;
                    }
                }
                else
                {
                    ArrayList al = new ArrayList();
                    al.Add("");
                    al.Add("预订单没有违约扣罚记录！");
                    FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "中国商品批发交易平台", al);
                    fm.ShowDialog();
                    return;
                }
               
            }
            if (columnName == "查看保证函" && e.RowIndex > -1)
            {
                DataGridViewRow dgr = this.dataGridView1.Rows[e.RowIndex];
                string Strtype = dgr.Cells["单据类型"].Value.ToString().Trim();
                string Number = dgr.Cells["中标定标编号"].Value.ToString().Trim();
                if (Strtype.Equals("投标单"))
                {
                    //bzhcheck aaa = new bzhcheck(httt);
                    //aaa.ShowDialog();


                    int pagenumber = 1;//预估页数，要大于可能的最大页数
                    object[] CSarr = new object[pagenumber];
                    string[] MKarr = new string[pagenumber];
                    for (int i = 0; i < pagenumber; i++)
                    {
                        Hashtable htcs = new Hashtable();
                        htcs["纯数字索引"] = (i + 1).ToString(); //这个参数必须存在。
                        htcs["数据地址"] = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[2] + "/pingtaiservices/mobanrun/BZH.aspx?moban=BZH.htm&Number=" + Number;
                        htcs["要传递的参数1"] = "参数1";
                        CSarr[i] = htcs; //模拟参数
                        MKarr[i] = "Y3_Business_Muban.dll)!客户端主程序.SubForm.NewCenterForm.PrintUC.UCbzh";
                    }
                    fmDY dy = new fmDY("保证函", CSarr, MKarr);
                    dy.Show();

                }
                else
                {

                    ArrayList al = new ArrayList();
                    al.Add("");
                    al.Add("预订单没有保证函可查看！");
                    FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "中国商品批发交易平台", al);
                    fm.ShowDialog();
                    return;
                }
            
            }
            if (columnName == "合同编号" && e.RowIndex > -1)
            {
                int pagenumber = 8;//预估页数，要大于可能的最大页数
                object[] CSarr = new object[pagenumber + 2];
                string[] MKarr = new string[pagenumber + 2];
                for (int i = 0; i < pagenumber; i++)
                {
                    Hashtable htcs = new Hashtable();
                    htcs["纯数字索引"] = (i + 1).ToString(); //这个参数必须存在。
                    htcs["数据地址"] = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[2] + "/pingtaiservices/mobanrun/FMPTDZGHHT.aspx?Number=" + dataGridView1.Rows[e.RowIndex].Cells["中标定标编号"].Value;
                    htcs["要传递的参数1"] = "参数1";
                    CSarr[i] = htcs; //模拟参数
                    MKarr[i] = "Y3_Business_Muban.dll)!客户端主程序.SubForm.NewCenterForm.MuBan.UCFMPTDZGHHT";

                }
                Hashtable htcs1 = new Hashtable();
                htcs1["要传递的参数1"] = dataGridView1.Rows[e.RowIndex].Cells["中标定标编号"].Value;
                CSarr[pagenumber] = htcs1;
                MKarr[pagenumber] = "Y3_Business_Muban.dll)!客户端主程序.SubForm.NewCenterForm.MuBan.UCFMPTDZGHHT_1";

                Hashtable htcs2 = new Hashtable();
                htcs2["要传递的参数1"] = dataGridView1.Rows[e.RowIndex].Cells["中标定标编号"].Value;
                CSarr[pagenumber + 1] = htcs2;
                MKarr[pagenumber + 1] = "Y3_Business_Muban.dll)!客户端主程序.SubForm.NewCenterForm.MuBan.UCFMPTDZGHHT_2";
                FormDZGHHT dy = new FormDZGHHT("电子购货合同", CSarr, MKarr, dataGridView1.Rows[e.RowIndex].Cells["中标定标编号"].Value.ToString());
                dy.Show();
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {         
            //string dlyx = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString().Trim();
            //string StrSql = "select tab.合同编号,tab.定标时间,tab.下单时间,tab.单据类型,tab.单据编号,tab.商品编号,tab.商品名称,tab.规格,tab.合同期限,tab.定标数量,tab.定标价格,tab.投标保证金解冻金额,tab.订金冻结金额,tab.履约保证金冻结金额,(case tab.单据类型 when '预订单' then '--' else cast(cast((1-(isnull(c.扣履,0)/tab.履约保证金冻结金额))*100.00 as numeric(18,2)) as varchar(20)) end) as [履约保证金冻结剩余比例%],tab.是否拆单 from AAA_View_SPDingBiao as tab left join (select ZBDBXXBBH,'投标单' as 单据类型,sum(a.JE) as 扣履 from AAA_ZKLSMXB as a left join AAA_THDYFHDXXB as b on a.LYDH=b.number where SJLX='预' and (XZ='超过最迟发货日后录入发货信息' or XZ='发货单生成后5日内未录入发票邮寄信息') and LYYWLX='AAA_THDYFHDXXB' group by ZBDBXXBBH) as c on c.ZBDBXXBBH=tab.中标定标编号 and c.单据类型=tab.单据类型 where " + ht_where["search_str_where"].ToString() + " order by 定标时间 asc";
            //string[] HideColumns = new string[] {};
            //cMyXls1.BeginRunFrom_ht_where(StrSql, HideColumns);

            Hashtable ht_export = new Hashtable();
            ht_export["filename"] = "定标与保证函";
            ht_export["webmethod"] = "商品买卖C区导出";
            ht_export["tiaojian"] = (Hashtable)ht_where["tiaojian"];
            //string[] HideColumns = new string[] { "" };
            //ht_export["HideColumns"] = HideColumns;

            cMyXls1.BeginRunFrom_ht_where(ht_export);
        }
    }
}
