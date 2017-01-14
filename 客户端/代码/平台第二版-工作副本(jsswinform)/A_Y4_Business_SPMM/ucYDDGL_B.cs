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
using 客户端主程序.Support;


namespace 客户端主程序.SubForm.NewCenterForm.SUBUC.SPMM
{
    public partial class ucYDDGL_B : UserControl
    {
        Support.ManageDgv Dgv;

        /// <summary>
        /// 查询条件
        /// </summary>
        Hashtable ht_where = new Hashtable();
        public ucYDDGL_B()
        {
            InitializeComponent();
            this.cbxDJLB.SelectedIndex = 0;
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
            //ht_where["serach_Row_str"] = " * ";

            //if (PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["买家角色编号"] == null)
            //{
            //    ht_where["search_tbname"] = "  (SELECT '修改' AS 修改,'撤销' AS 撤销,  Number,SPBH,SPMC,GG,NDGSL,YZBSL,NMRJG,NDGJE,ISNULL(CAST((SELECT TOP 1 TBJG FROM AAA_TBD WHERE ZT='竞标' AND AAA_TBD.SPBH=AAA_YDDXXB.SPBH and AAA_TBD.HTQX=AAA_YDDXXB.HTQX ORDER BY TBJG ASC) AS varchar(50)),'--') AS DQMJZDJ,DJDJ,(CASE WHEN YZBSL= NDGSL THEN '否' WHEN YZBSL=0 THEN '否' ELSE '是' END) AS SFCD,(CASE WHEN(SELECT SFJRLJQ FROM AAA_LJQDQZTXXB WHERE AAA_LJQDQZTXXB.HTQX = AAA_YDDXXB.HTQX AND AAA_LJQDQZTXXB.SPBH=AAA_YDDXXB.SPBH)='是' THEN '竞标中（冷静期）' ELSE '竞标中' END) AS DQZT,isnull(replace(convert(varchar(20),YXJZRQ,111),'/','-'),'--') YXJZRQ,HTQX,SHQY FROM AAA_YDDXXB WHERE AAA_YDDXXB.ZT='竞标' AND 1<>1) AS TABLE1   ";
            //}
            //else
            //{
            //    ht_where["search_tbname"] = "  (SELECT '修改' AS 修改,'撤销' AS 撤销,  Number,SPBH,SPMC,GG,NDGSL,YZBSL,NMRJG,NDGJE,ISNULL(CAST((SELECT TOP 1 TBJG FROM AAA_TBD WHERE ZT='竞标' AND AAA_TBD.SPBH=AAA_YDDXXB.SPBH and AAA_TBD.HTQX=AAA_YDDXXB.HTQX ORDER BY TBJG ASC) AS varchar(50)),'--') AS DQMJZDJ,DJDJ,(CASE WHEN YZBSL= NDGSL THEN '否' WHEN YZBSL=0 THEN '否' ELSE '是' END) AS SFCD,(CASE WHEN(SELECT SFJRLJQ FROM AAA_LJQDQZTXXB WHERE AAA_LJQDQZTXXB.HTQX = AAA_YDDXXB.HTQX AND AAA_LJQDQZTXXB.SPBH=AAA_YDDXXB.SPBH)='是' THEN '竞标中（冷静期）' ELSE '竞标中' END) AS DQZT,isnull(replace(convert(varchar(20),YXJZRQ,111),'/','-'),'--') YXJZRQ,HTQX,SHQY FROM AAA_YDDXXB WHERE AAA_YDDXXB.ZT='竞标' AND AAA_YDDXXB.MJJSBH='" + PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["买家角色编号"].ToString() + "') AS TABLE1   ";  //检索的表(必须设置)
            //}

            //ht_where["search_mainid"] = " Number ";  //所检索表的主键(必须设置)
            //ht_where["search_str_where"] = " 1=1 ";  //检索条件(必须设置)
            //ht_where["search_paixu"] = " ASC ";  //排序方式(必须设置)
            //ht_where["search_paixuZD"] = " Number ";  //用于排序的字段(必须设置)

            //if (txtSPMC.Text.Trim() != "")
            //{
            //    ht_where["search_str_where"] = ht_where["search_str_where"].ToString() + "AND SPMC LIKE '%"+this.txtSPMC.Text.Trim()+"%' ";
            //}
            //if (cbxDJLB.SelectedItem.ToString() != "全部")
            //{
            //    ht_where["search_str_where"] = ht_where["search_str_where"].ToString() + "AND DQZT='" + cbxDJLB.SelectedItem.ToString() + "' ";
            //}


            ht_where["page_index"] = "0";
            ht_where["page_size"] = "25";  //每页显示的条数(必须设置)
            ht_where["webmethod"] = "商品买卖B区";
            Hashtable ht_tiaojian = new Hashtable();
            ht_tiaojian["标签名"] = "预订单管理";
            ht_tiaojian["买方编号"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["买家角色编号"].ToString();
            ht_tiaojian["商品名称"] = txtSPMC.Text.Trim();
            ht_tiaojian["当前状态"] = cbxDJLB.SelectedItem.ToString();
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
            Dgv = new Support.ManageDgv(this.dataGridView1);
            Dgv.AddMergeColumns(0, 2, "操作", -1);

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
            //ht_where["search_str_where"] = " 1=1 ";  //检索条件(必须设置)
            //执行查询
            GetData(ht_where);
        }
        private void ucYDDGL_B_Load(object sender, EventArgs e)
        {
            //处理下拉框间距
            cbxDJLB.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.CB_DrawItem);
            GetData(null);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {


            DataGridView dgv = (DataGridView)sender;

            if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
                // 
                if (dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null && dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "修改")
                {
                    string Number = dgv.Rows[e.RowIndex].Cells[2].Value.ToString();//预订单号

                    int x = Convert.ToInt32(dgv.Rows[e.RowIndex].Cells["Column4"].Value.ToString()) - Convert.ToInt32(dgv.Rows[e.RowIndex].Cells["Column5"].Value.ToString());

                    ArrayList Almsg3 = new ArrayList();
                    Almsg3.Add("");
                    Almsg3.Add("您一旦确认修改，系统将撤销" + Number + "预订单，修改后自动生成新的预订单重新参与竞标！是否确认？");
                    FormAlertMessage FRSE3 = new FormAlertMessage("确定取消", "问号", "", Almsg3);
                    DialogResult dialogResult = FRSE3.ShowDialog();
                    if (dialogResult == DialogResult.Yes)
                    {

                        fmYDDXG2 fm = new fmYDDXG2(Number, x);
                        fm.ShowDialog();
                        reset();
                        //dataGridView1.Enabled = false;
                        //CXOPEN(ht);//开调！
                    }
                }

                if (dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null && dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "撤销")
                {
                    string Number = dgv.Rows[e.RowIndex].Cells[2].Value.ToString();//预订单号
                    ArrayList Almsg3 = new ArrayList();
                    Almsg3.Add("");
                    Almsg3.Add("您一旦确认撤销，系统将撤销" + Number + "预订单的未中标部分，撤销后，相应订金自动解冻。是否确认？");
                    FormAlertMessage FRSE3 = new FormAlertMessage("确定取消", "问号", "", Almsg3);
                    DialogResult dialogResult = FRSE3.ShowDialog();
                    if (dialogResult == DialogResult.Yes)
                    {
                        Hashtable ht = new Hashtable();
                        ht["Number"] = Number;
                        dataGridView1.Enabled = false;
                        CX(ht);//开调！
                    }
                }
            }
        }
        #region 撤销预订单
        private void CX(Hashtable InPutHT)
        {
            G_B g = new G_B(InPutHT, new delegateForThread(CX_Begin));
            Thread trd = new Thread(new ThreadStart(g.YDD_CX));
            trd.IsBackground = true;
            trd.Start();
        }
        private void CX_Begin(Hashtable OutPutHT)
        {
            try { Invoke(new delegateForThreadShow(CX_Invoke), new Hashtable[] { OutPutHT }); }
            catch (Exception ex) { Support.StringOP.WriteLog("委托回调错误：" + ex.ToString()); }
        }
        private void CX_Invoke(Hashtable OutPutHT)
        {
            dataGridView1.Enabled = true;
            DataSet dsreturn = (DataSet)OutPutHT["结果"];
            if (dsreturn != null)
            {
                string zt = dsreturn.Tables["返回值单条"].Rows[0]["执行结果"].ToString();
                string showstr = dsreturn.Tables["返回值单条"].Rows[0]["提示文本"].ToString();
                if (zt == "err")
                {

                    if (showstr.IndexOf("|||") > 0)
                    {
                        string[] strAry = showstr.Split(new string[] { "|||" }, StringSplitOptions.RemoveEmptyEntries);
                        ArrayList als = new ArrayList();
                        als.Add("");
                        als.Add(strAry[0]);
                        als.Add(strAry[1]);
                        if (strAry.Length > 2)
                            als.Add(strAry[2]);
                        FormAlertMessage fms = new FormAlertMessage("仅确定", "其他", "中国商品批发交易平台", als);
                        fms.ShowDialog();
                        reset();
                    }

                    else
                    {
                        ArrayList al = new ArrayList();
                        al.Add("");
                        al.Add(showstr);
                        FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "中国商品批发交易平台", al);
                        fm.ShowDialog();
                        reset();
                    }
                }
                if (zt == "ok")
                {
                    ArrayList al = new ArrayList();
                    al.Add("");
                    al.Add(showstr);
                    FormAlertMessage fm = new FormAlertMessage("仅确定", "对号", "中国商品批发交易平台", al);
                    fm.ShowDialog();
                    reset();
                }
            }
            else
            {
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add("系统繁忙，请稍后再试！");
                FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "中国商品批发交易平台", al);
                fm.ShowDialog();
                reset();
            }

        }
        #endregion

        #region 撤销预订单&&打开新下单窗口
        //private void CXOPEN(Hashtable InPutHT)
        //{
        //    G_B g = new G_B(InPutHT, new delegateForThread(CX_OPEN));
        //    Thread trd = new Thread(new ThreadStart(g.YDD_CX));
        //    trd.IsBackground = true;
        //    trd.Start();
        //}
        //private void CX_OPEN(Hashtable OutPutHT)
        //{
        //    try { Invoke(new delegateForThreadShow(CXOPEN_Invoke), new Hashtable[] { OutPutHT }); }
        //    catch (Exception ex) { Support.StringOP.WriteLog("委托回调错误：" + ex.ToString()); }
        //}
        //private void CXOPEN_Invoke(Hashtable OutPutHT)
        //{
        //    dataGridView1.Enabled = true;
        //    DataSet dsreturn = (DataSet)OutPutHT["结果"];
        //    if (dsreturn != null)
        //    {
        //        string zt = dsreturn.Tables["返回值单条"].Rows[0]["执行结果"].ToString();
        //        string showstr = dsreturn.Tables["返回值单条"].Rows[0]["提示文本"].ToString();
        //        if (zt == "err")
        //        {
        //            ArrayList al = new ArrayList();
        //            al.Add("");
        //            al.Add(showstr);
        //            FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "中国商品批发交易平台", al);
        //            fm.ShowDialog();
        //        }
        //        if (zt == "ok")
        //        {

                    
        //            fmYDDXG fm = new fmYDDXG(dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"].ToString());
        //            fm.ShowDialog();
        //            reset();
        //            //ArrayList al = new ArrayList();
        //            //al.Add("");
        //            //al.Add(showstr);
        //            //FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "中国商品批发交易平台", al);
        //            //fm.ShowDialog();
        //            //reset();
        //        }
        //    }
        //    else
        //    {
        //        ArrayList al = new ArrayList();
        //        al.Add("");
        //        al.Add("系统繁忙，请稍后再试！");
        //        FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "中国商品批发交易平台", al);
        //        fm.ShowDialog();
        //        reset();
        //    }

        //}
        #endregion

        /// <summary>
        /// 查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click_1(object sender, EventArgs e)
        {
            GetData(null);
        }
        /// <summary>
        /// 重置表单
        /// </summary>
        private void reset()
        {
            ucYDDGL_B YDD = new ucYDDGL_B();
            YDD.Dock = DockStyle.Fill;//铺满
            YDD.AutoScroll = true;//出现滚动条
            YDD.BackColor = Color.AliceBlue;
            //清理控件
            Panel P = (Panel)this.Parent;
            P.Controls.Clear();
            P.Controls.Add(YDD); //加入到某一个panel中
            YDD.Show();//显示出来
        }
    }
}
