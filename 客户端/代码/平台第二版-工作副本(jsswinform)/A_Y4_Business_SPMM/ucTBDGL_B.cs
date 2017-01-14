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
using 客户端主程序.NewDataControl;
using 客户端主程序.Support;


namespace 客户端主程序.SubForm.NewCenterForm.SUBUC.SPMM
{
    public partial class ucTBDGL_B : UserControl
    {
        Support.ManageDgv Dgv;

            /// <summary>
        /// 查询条件
        /// </summary>
        Hashtable ht_where = new Hashtable();

        public ucTBDGL_B()
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
            #region 2014-01-03 shiyan替换,为了拆分计算当前卖家最低价等
            //ht_where["GetCustomersDataPage_NAME"] = "GetCustomersDataPage";  //每页显示的条数(必须设置)
            //ht_where["page_index"] = "0";
            //ht_where["page_size"] = "25";  //每页显示的条数(必须设置)
            //ht_where["serach_Row_str"] = " * "; //检索字段(必须设置)

            //if (PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["卖家角色编号"] != null)
            //{
            //    ht_where["search_tbname"] = "  (SELECT '撤销' AS 撤销,'修改' AS 修改, TBD.*,(CASE TBD.HTQX WHEN '即时' THEN CAST(ISNULL(((SELECT TOP 1 (YZBSL) FROM AAA_TBD WHERE ZT='竞标' AND AAA_TBD.SPBH=TBD.SPBH AND AAA_TBD.HTQX = '即时' ORDER BY AAA_TBD.TBJG ASC) / (SELECT TOP 1 (TBNSL) FROM AAA_TBD WHERE ZT='竞标' AND AAA_TBD.SPBH=TBD.SPBH AND AAA_TBD.HTQX = '即时' ORDER BY AAA_TBD.TBJG ASC)),0)* 100 AS numeric(18, 2)) WHEN '三个月' THEN CAST(ISNULL(((SELECT SUM(NDGSL)-SUM(YZBSL) FROM AAA_YDDXXB WHERE AAA_YDDXXB.SPBH=TBD.SPBH AND AAA_YDDXXB.HTQX = '三个月' AND AAA_YDDXXB.ZT='竞标' AND AAA_YDDXXB.NMRJG>=(SELECT TOP 1 TBJG FROM AAA_TBD WHERE ZT='竞标' AND AAA_TBD.SPBH=TBD.SPBH AND AAA_TBD.HTQX ='三个月' ORDER BY AAA_TBD.TBJG ASC) AND CHARINDEX('|'+SHQYsheng+'|',(SELECT TOP 1 GHQY FROM AAA_TBD WHERE ZT='竞标' AND AAA_TBD.SPBH=TBD.SPBH AND AAA_TBD.HTQX= '三个月' ORDER BY AAA_TBD.TBJG ASC))>0)/ (SELECT TOP 1 (TBNSL-YZBSL) FROM AAA_TBD WHERE ZT='竞标' AND AAA_TBD.SPBH=TBD.SPBH AND AAA_TBD.HTQX ='三个月' ORDER BY AAA_TBD.TBJG ASC)),0)* 100 AS numeric(18, 2)) WHEN '一年' THEN CAST(ISNULL(((SELECT SUM(NDGSL)-SUM(YZBSL) FROM AAA_YDDXXB WHERE AAA_YDDXXB.SPBH=TBD.SPBH AND AAA_YDDXXB.HTQX='一年' AND AAA_YDDXXB.ZT='竞标' AND AAA_YDDXXB.NMRJG>=(SELECT TOP 1 TBJG FROM AAA_TBD WHERE ZT='竞标' AND AAA_TBD.SPBH=TBD.SPBH AND AAA_TBD.HTQX ='一年' ORDER BY AAA_TBD.TBJG ASC) AND CHARINDEX('|'+SHQYsheng+'|',(SELECT TOP 1 GHQY FROM AAA_TBD WHERE ZT='竞标' AND AAA_TBD.SPBH=TBD.SPBH AND AAA_TBD.HTQX ='一年' ORDER BY AAA_TBD.TBJG ASC))>0)/ (SELECT TOP 1 (TBNSL-YZBSL) FROM AAA_TBD WHERE ZT='竞标' AND AAA_TBD.SPBH=TBD.SPBH AND AAA_TBD.HTQX ='一年' ORDER BY AAA_TBD.TBJG ASC)),0)* 100 AS numeric(18, 2)) END) AS XTDCL,(SELECT TOP 1 TBJG FROM AAA_TBD WHERE ZT='竞标' AND TBD.SPBH=AAA_TBD.SPBH and  AAA_TBD.HTQX = TBD.HTQX  ORDER BY TBJG ASC) AS ZDTBJG,CAST((CASE WHEN TBD.TBJG>(SELECT TOP 1 TBJG FROM AAA_TBD WHERE ZT='竞标' AND TBD.SPBH=AAA_TBD.SPBH AND TBD.HTQX=AAA_TBD.HTQX ORDER BY TBJG ASC) THEN 0 ELSE ISNULL(((SELECT SUM(NDGSL)-SUM(YZBSL) FROM AAA_YDDXXB WHERE AAA_YDDXXB.SPBH=TBD.SPBH AND AAA_YDDXXB.HTQX=TBD.HTQX AND AAA_YDDXXB.ZT='竞标' AND AAA_YDDXXB.NMRJG>=TBD.TBJG AND CHARINDEX('|'+SHQYsheng+'|',(SELECT TOP 1 GHQY FROM AAA_TBD WHERE ZT='竞标' AND AAA_TBD.SPBH=TBD.SPBH AND AAA_TBD.HTQX = TBD.HTQX ORDER BY AAA_TBD.TBJG ASC))>0)/ (SELECT TOP 1 (TBNSL-YZBSL) FROM AAA_TBD WHERE ZT='竞标' AND AAA_TBD.SPBH=TBD.SPBH AND AAA_TBD.HTQX = TBD.HTQX ORDER BY AAA_TBD.TBJG ASC)),0) END)* 100 AS numeric(18, 2))  AS WDDCL FROM (SELECT AAA_TBD.Number,SPMC,GG,TBNSL,YZBSL,TBJG,TBJE,DJTBBZJ,AAA_TBD.SPBH,AAA_TBD.HTQX,GHQY,ZT,MJJSBH FROM AAA_TBD LEFT JOIN AAA_LJQDQZTXXB ON AAA_LJQDQZTXXB.HTQX=AAA_TBD.HTQX AND AAA_LJQDQZTXXB.SPBH=AAA_TBD.SPBH WHERE isnull(AAA_LJQDQZTXXB.SFJRLJQ,'') !='是' AND AAA_TBD.MJJSBH='" + PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["卖家角色编号"].ToString() + "') AS TBD WHERE TBD.ZT='竞标' AND MJJSBH='" + PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["卖家角色编号"].ToString() + "') AS Table1  ";  //检索的表(必须设置)
            //}
            //else
            //{
            //    ht_where["search_tbname"] = "  (SELECT '撤销' AS 撤销,'修改' AS 修改, TBD.*,CAST(ISNULL(((SELECT SUM(NDGSL) FROM AAA_YDDXXB WHERE AAA_YDDXXB.SPBH=TBD.SPBH AND AAA_YDDXXB.HTQX=TBD.HTQX AND AAA_YDDXXB.ZT='竞标' AND AAA_YDDXXB.NMRJG>=TBD.TBJG AND CHARINDEX('|'+SHQYsheng+'|',GHQY)>0)/ (SELECT TOP 1 (TBNSL-YZBSL) FROM AAA_TBD WHERE ZT='竞标' AND AAA_TBD.SPBH=TBD.SPBH AND AAA_TBD.HTQX = TBD.HTQX ORDER BY AAA_TBD.TBJG ASC)),0) AS numeric(18, 2)) AS XTDCL,(SELECT TOP 1 TBJG FROM AAA_TBD WHERE ZT='竞标' AND TBD.SPBH=AAA_TBD.SPBH ORDER BY TBJG ASC) AS ZDTBJG,CAST((CASE WHEN TBD.TBJG>(SELECT TOP 1 TBJG FROM AAA_TBD WHERE ZT='竞标' AND TBD.SPBH=AAA_TBD.SPBH AND TBD.HTQX=AAA_TBD.HTQX ORDER BY TBJG ASC) THEN 0 ELSE ISNULL(((SELECT SUM(NDGSL) FROM AAA_YDDXXB WHERE AAA_YDDXXB.SPBH=TBD.SPBH AND AAA_YDDXXB.HTQX=TBD.HTQX AND AAA_YDDXXB.ZT='竞标' AND AAA_YDDXXB.NMRJG>=TBD.TBJG AND CHARINDEX('|'+SHQYsheng+'|',GHQY)>0)/ (SELECT TOP 1 (TBNSL-YZBSL) FROM AAA_TBD WHERE ZT='竞标' AND AAA_TBD.SPBH=TBD.SPBH AND AAA_TBD.HTQX = TBD.HTQX ORDER BY AAA_TBD.TBJG ASC)),0) END) AS numeric(18, 2)) AS WDDCL FROM (SELECT AAA_TBD.Number,SPMC,GG,TBNSL,YZBSL,TBJG,TBJE,DJTBBZJ,AAA_TBD.SPBH,AAA_TBD.HTQX,GHQY,ZT,MJJSBH FROM AAA_TBD LEFT JOIN AAA_LJQDQZTXXB ON AAA_LJQDQZTXXB.HTQX=AAA_TBD.HTQX AND AAA_LJQDQZTXXB.SPBH=AAA_TBD.SPBH WHERE isnull(AAA_LJQDQZTXXB.SFJRLJQ,'') !='是' AND 1<>1) AS TBD WHERE TBD.ZT='竞标' AND 1<>1) AS Table1  "; 
            //}

            //ht_where["search_mainid"] = " Number ";  //所检索表的主键(必须设置)
            //ht_where["search_str_where"] = " 1=1 ";  //检索条件(必须设置)
            //ht_where["search_paixu"] = " ASC ";  //排序方式(必须设置)
            //ht_where["search_paixuZD"] = " Number ";  //用于排序的字段(必须设置)
            #endregion


            //ht_where["GetCustomersDataPage_NAME"] = "GetCustomersDataPage";  //每页显示的条数(必须设置)
            //ht_where["page_index"] = "0";
            //ht_where["page_size"] = "25";  //每页显示的条数(必须设置)
            //ht_where["serach_Row_str"] = "  '撤销' AS 撤销,'修改' AS 修改,AAA_TBD.Number,SPMC,GG,TBNSL,YZBSL,TBJG,TBJE,DJTBBZJ,AAA_TBD.SPBH,AAA_TBD.HTQX,GHQY,ZT,MJJSBH,''AS XTDCL,'' AS ZDTBJG,'0.00'  AS WDDCL "; //检索字段(必须设置)
            //ht_where["search_tbname"] = " AAA_TBD LEFT JOIN AAA_LJQDQZTXXB ON AAA_LJQDQZTXXB.HTQX=AAA_TBD.HTQX AND AAA_LJQDQZTXXB.SPBH=AAA_TBD.SPBH ";  //检索的表(必须设置)
            //ht_where["search_mainid"] = " AAA_TBD.Number ";  //所检索表的主键(必须设置)
            //ht_where["search_str_where"] = " isnull(AAA_LJQDQZTXXB.SFJRLJQ,'否') ='否' and ZT='竞标' AND AAA_TBD.MJJSBH='" + PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["卖家角色编号"].ToString() + "' ";  //检索条件(必须设置)
            //ht_where["search_paixu"] = " ASC ";  //排序方式(必须设置)
            //ht_where["search_paixuZD"] = " AAA_TBD.Number ";  //用于排序的字段(必须设置)
            //ht_where["method_retreatment"] = "SPMM_C|TBDGL_Bqu";

            //if (txtSPMC.Text.Trim() != "")
            //    ht_where["search_str_where"] = ht_where["search_str_where"].ToString() + " AND SPMC LIKE '%" + this.txtSPMC.Text.Trim() + "%' ";

            ht_where["page_index"] = "0";
            ht_where["page_size"] = "25";  //每页显示的条数(必须设置)
            ht_where["webmethod"] = "商品买卖B区";
            Hashtable ht_tiaojian = new Hashtable();
            ht_tiaojian["标签名"] = "投标单管理";
            ht_tiaojian["卖方编号"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["卖家角色编号"].ToString();
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
      
        private void ucTBDGL_B_Load(object sender, EventArgs e)
        {
           
            GetData(null);
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
                       ArrayList Almsg3 = new ArrayList();
                       Almsg3.Add("");
                       Almsg3.Add("您一旦确认修改，系统将撤销" + Number + "投标单，修改后自动生成新的投标单重新参与竞标！是否确认？");
                       FormAlertMessage FRSE3 = new FormAlertMessage("确定取消", "问号", "", Almsg3);
                       DialogResult dialogResult = FRSE3.ShowDialog();
                       if (dialogResult == DialogResult.Yes)
                       {
                           fmTBDXG2 fm = new fmTBDXG2(Number);
                           fm.ShowDialog();
                           reset();
                       }
                   }

                   if (dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null && dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "撤销")
                   {
                       string Number = dgv.Rows[e.RowIndex].Cells[2].Value.ToString();//预订单号
                       ArrayList Almsg3 = new ArrayList();
                       Almsg3.Add("");
                       Almsg3.Add("您一旦确认撤销，系统将撤销" + Number + "投标单的未中标部分，撤销后，相应订金自动解冻。是否确认？");
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

        #region 投标单撤销
        private void CX(Hashtable InPutHT)
        {
            G_S g = new G_S(InPutHT, new delegateForThread(CX_Begin));
            Thread trd = new Thread(new ThreadStart(g.TBD_CX));
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
                        this.Hide();
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

        private void btnSearch_Click_1(object sender, EventArgs e)
        {
            GetData(null);
        }
        /// <summary>
        /// 重置表单
        /// </summary>
        private void reset()
        {
            ucTBDGL_B YDD = new ucTBDGL_B();
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
