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
    public partial class ucDJZJ_C : UserControl
    {
        /// <summary>
        /// 查询条件
        /// </summary>
        Hashtable ht_where = new Hashtable();
        DataTable dtXMXZ = new DataTable();
        public ucDJZJ_C()
        {
            InitializeComponent();
            //初始化分页回调(带分页的页面，先把这个放上)
            ucPager1.DFT = new delegateForThread(STR_BeginBind);
        }
        private void ucDJZJ_C_Load(object sender, EventArgs e)
        {
            //处理下拉框间距
            cbxXZ.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.CB_DrawItem);
            cbxXM.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.CBXM_DrawItem);
            //给项目、性质下拉框赋值  
            Hashtable InputHT = new Hashtable();
            InputHT["index"] = "2";
            InputHT["type"] = "实";
            SRT_GetXXInfo_Run(InputHT);
            //显示默认数据
            GetData(null);
        }

        /// <summary>
        /// 设置默认搜索条件(这个例子中的哈希表键值一个也不能少)
        /// </summary>
        private void setDefaultSearch()
        {
            //获取登陆者的登陆邮箱
            string dlyx = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString();

            //ht_where["GetCustomersDataPage_NAME"] = "GetCustomersDataPage";  //每页显示的条数(必须设置)
            //ht_where["page_index"] = "0";
            //ht_where["page_size"] = "25";  //每页显示的条数(必须设置)
            //ht_where["serach_Row_str"] = " a.XM as 项目,a.XZ as 性质,a.ZY as 摘要,a.JE as 金额,a.LSCSSJ as 时间 "; //检索字段(必须设置)
            //ht_where["search_tbname"] = "  AAA_ZKLSMXB as a left join AAA_moneyDZB as b on a.xm=b.xm and a.xz=b.xz  and a.sjlx=b.sjlx ";  //检索的表(必须设置)
            //ht_where["search_mainid"] = " a.number ";  //所检索表的主键(必须设置)
            //ht_where["search_str_where"] = " 1=1 and a.sjlx='实' and isnull(b.dymkbh,'')='2' and a.dlyx='" + dlyx + "' ";  //检索条件(必须设置)
            //// ht_where["search_str_where"] = " 1=1 and b.sjlx='实' and isnull(b.dymkbh,'')='1'";
            //ht_where["search_paixu"] = " DESC ";  //排序方式(必须设置)
            //ht_where["search_paixuZD"] = " a.LSCSSJ DESC,a.number ";  //用于排序的字段(必须设置)

            ht_where["page_index"] = "0";
            ht_where["page_size"] = "25";
            ht_where["webmethod"] = "资金转账C区";

            Hashtable ht_tiaojian = new Hashtable();
            ht_tiaojian["用户邮箱"] = dlyx;
            ht_tiaojian["数据类型"] = "实";
            ht_tiaojian["模块编号"] = "2";

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
        //private void btnSearch_Click(object sender, EventArgs e)
        //{
        //    //获取搜索条件
        //    string begintime = dtTimeStart.Value.ToString("yyyy-MM-dd");
        //    string endtime = dtTImeEnd.Value.ToString("yyyy-MM-dd");
        //    //更改搜索条件
        //    setDefaultSearch();

        //    ArrayList Almsg4 = new ArrayList();
        //    Almsg4.Add("");
        //    if (begintime == "")
        //    {
        //        Almsg4.Add("请查询选择开始时间");// 这里是提交失败的提示，可能是程序出错，可能是条件不足等                
        //    }
        //    if (endtime == "")
        //    {
        //        Almsg4.Add("请查询选择结束时间");// 这里是提交失败的提示，可能是程序出错，可能是条件不足等               
        //    }
        //    if (Convert.ToDateTime(begintime) > Convert.ToDateTime(endtime))
        //    {
        //        Almsg4.Add("查询结束时间不能大于开始时间，请重新选择！");// 这里是提交失败的提示，可能是程序出错，可能是条件不足等  
        //    }
        //    if (Almsg4.Count > 1)
        //    {
        //        FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "其他", "", Almsg4);
        //        FRSE4.ShowDialog();
        //    }

        //    if (begintime != null && endtime != null && Convert.ToDateTime(begintime) <= Convert.ToDateTime(endtime))
        //    {
        //        ht_where["search_str_where"] = ht_where["search_str_where"] + " and convert(varchar(10), a.LSCSSJ,120)>='" + begintime + "' and convert(varchar(10), a.LSCSSJ,120)<='" + endtime + "' ";
        //    }
        //    if (cbxXM.SelectedItem.ToString() != "" && cbxXM.SelectedItem.ToString() != "选择项目")
        //    {
        //        ht_where["search_str_where"] = ht_where["search_str_where"] + " and a.xm='" + cbxXM.SelectedItem.ToString() + "' ";
        //    }
        //    if (cbxXZ.SelectedItem.ToString() != "" && cbxXZ.SelectedItem.ToString() != "选择性质")
        //    {
        //        ht_where["search_str_where"] = ht_where["search_str_where"] + " and a.xz='" + cbxXZ.SelectedItem.ToString() + "' ";
        //    }

        //    ht_where["search_str_where"] = ht_where["search_str_where"];  //检索条件(必须设置)
        //    //执行查询
        //    GetData(ht_where);
        //}

        private void btnSearch_Click(object sender, EventArgs e)
        {
            //获取搜索条件
            string begintime = dtTimeStart.Value.ToString("yyyy-MM-dd");
            string endtime = dtTImeEnd.Value.ToString("yyyy-MM-dd");
            //更改搜索条件
            setDefaultSearch();
            Hashtable ht_tiaojian = (Hashtable)ht_where["tiaojian"];

            ArrayList Almsg4 = new ArrayList();
            Almsg4.Add("");
            if (begintime == "")
            {
                Almsg4.Add("请查询选择开始时间");// 这里是提交失败的提示，可能是程序出错，可能是条件不足等                
            }
            if (endtime == "")
            {
                Almsg4.Add("请查询选择结束时间");// 这里是提交失败的提示，可能是程序出错，可能是条件不足等               
            }
            if (Convert.ToDateTime(begintime) > Convert.ToDateTime(endtime))
            {
                Almsg4.Add("查询结束时间不能大于开始时间，请重新选择！");// 这里是提交失败的提示，可能是程序出错，可能是条件不足等  
            }
            if (Almsg4.Count > 1)
            {
                FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "其他", "", Almsg4);
                FRSE4.ShowDialog();
            }

            if (begintime != null && endtime != null && Convert.ToDateTime(begintime) <= Convert.ToDateTime(endtime))
            {
                ht_tiaojian.Add("开始时间", begintime);
                ht_tiaojian.Add("结束时间", endtime);               
            }
            if (cbxXM.SelectedItem.ToString() != "" && cbxXM.SelectedItem.ToString() != "选择项目")
            {
                ht_tiaojian.Add("项目", cbxXM.SelectedItem.ToString());    
            }
            if (cbxXZ.SelectedItem.ToString() != "" && cbxXZ.SelectedItem.ToString() != "选择性质")
            {
                ht_tiaojian.Add("性质", cbxXZ.SelectedItem.ToString());                
            }            
            ht_where["tiaojian"] = ht_tiaojian;
            //执行查询
            GetData(ht_where);
        }
        //导出操作
        private void btnExport_Click(object sender, EventArgs e)
        {
            ////设置要隐藏的列(原分页中，有些列可能是隐藏不显示的，导出时，可以选择滤掉)
            //string[] HideColumns = new string[] { "" };
           
            ////设置导出语句
            //string sql = "select a.LSCSSJ as 时间,a.ZY as 摘要, a.XM as 项目,a.XZ as 性质,a.JE as 金额 from  AAA_ZKLSMXB as a left join AAA_moneyDZB as b on a.xm=b.xm and a.xz=b.xz  and a.sjlx=b.sjlx  where " + ht_where["search_str_where"].ToString();
            //sql = sql + " order by a.LSCSSJ DESC,a.number DESC";
            //cMyXls1.BeginRunFrom_ht_where(sql, HideColumns);

           
            Hashtable ht = new Hashtable();
            ht["filename"] = "冻结资金";
            ht["webmethod"] = "资金转账C区导出";
            ht["tiaojian"] = (Hashtable)ht_where["tiaojian"];
            //string[] HideColumns = new string[] { "带符号金额" };
            //ht["HideColumns"] = HideColumns;

            cMyXls1.BeginRunFrom_ht_where(ht);
        }
        #region 获取项目、性质下拉框需要绑定的数据
        //开启一个线程
        private void SRT_GetXXInfo_Run(Hashtable InPutHT)
        {
            RunThreadClassZJCXCqu OTD = new RunThreadClassZJCXCqu(InPutHT, new delegateForThread(SRT_GetXXInfo));
            Thread trd = new Thread(new ThreadStart(OTD.BeginRun));
            trd.IsBackground = true;
            trd.Start();
        }
        //显示线程处理结果的函数(用于处理线程返回数据),调用异步委托
        private void SRT_GetXXInfo(Hashtable OutPutHT)
        {
            try { Invoke(new delegateForThreadShow(SRT_GetXXInfo_Invoke), new Hashtable[] { OutPutHT }); }
            catch (Exception ex) { Support.StringOP.WriteLog("委托回调错误：" + ex.ToString()); }
        }
        //处理非线程创建的控件
        private void SRT_GetXXInfo_Invoke(Hashtable OutPutHT)
        {
            //返回值后的处理
            DataSet dsreturn = (DataSet)OutPutHT["返回值"];

            string zt = dsreturn.Tables["返回值单条"].Rows[0]["执行结果"].ToString();
            string showstr = dsreturn.Tables["返回值单条"].Rows[0]["提示文本"].ToString();
            //显示执行结果
            if (zt == "ok")
            {
                dtXMXZ = dsreturn.Tables["ResDT"].Copy();
                SetXM(dtXMXZ);
                SetXZ(dtXMXZ);
            }
            else
            {
                ArrayList Almsg4 = new ArrayList();
                Almsg4.Add("");
                Almsg4.Add(showstr);// 这里是提交失败的提示，可能是程序出错，可能是条件不足等
                FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "其他", "", Almsg4);
                FRSE4.ShowDialog();
            }
        }
        //给查询条件中的项目下拉框添加选项
        private void SetXM(DataTable dt)
        {
            cbxXM.Items.Clear();
            cbxXM.Items.Add("选择项目");
            DataTable dtDistinct = dt.DefaultView.ToTable(true, new string[] { "XM" });//注：其中ToTable（）的第一个参数为是否DISTINCT     
            if (dtDistinct != null && dtDistinct.Rows.Count > 0)
            {
                foreach (DataRow dr in dtDistinct.Rows)
                {
                    cbxXM.Items.Add(dr["XM"].ToString());
                }
            }
            cbxXM.SelectedIndex = 0;
        }
        //给查询条件中的性质下拉框添加选项
        private void SetXZ(DataTable dt)
        {
            cbxXZ.Items.Clear();
            cbxXZ.Items.Add("选择性质");
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    cbxXZ.Items.Add(dr["XZvalue"].ToString());
                }
            }
            cbxXZ.SelectedIndex = 0;
        }
        private void cbxXZ_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cbxXZ.SelectedItem.ToString() == "选择性质")
            {
                SetXM(dtXMXZ);
                SetXZ(dtXMXZ);
            }
            else
            {
                cbxXM.Items.Clear();
                cbxXM.Items.Add("选择项目");
                foreach (DataRow dr in dtXMXZ.Select("XZvalue='" + cbxXZ.SelectedItem.ToString() + "'"))
                {
                    cbxXM.Items.Add(dr["XM"].ToString());
                }
                if (cbxXM.Items.Count == 2)
                {
                    cbxXM.SelectedIndex = 1;
                }
                else
                {
                    cbxXM.SelectedIndex = 0;
                }
            }
        }

        private void cbxXM_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cbxXM.SelectedItem.ToString() == "选择项目")
            {
                SetXM(dtXMXZ);
                SetXZ(dtXMXZ);
            }
            else
            {
                cbxXZ.Items.Clear();
                cbxXZ.Items.Add("选择性质");
                foreach (DataRow dr in dtXMXZ.Select("XM='" + cbxXM.SelectedItem.ToString() + "'"))
                {
                    cbxXZ.Items.Add(dr["XZvalue"].ToString());
                }
                if (cbxXZ.Items.Count == 2)
                {
                    cbxXZ.SelectedIndex = 1;
                }
                else
                {
                    cbxXZ.SelectedIndex = 0;
                }

            }
        }
        #endregion

        #region 处理下拉框间距
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
        #endregion  


        
    }
}
