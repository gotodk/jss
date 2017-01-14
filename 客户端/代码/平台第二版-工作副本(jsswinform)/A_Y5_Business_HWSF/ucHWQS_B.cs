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
using 客户端主程序.SubForm.NewCenterForm.MuBan;

namespace 客户端主程序.SubForm.NewCenterForm.SUBUC.HWSF
{
    public partial class ucHWQS_B : UserControl
    {
        
             /// <summary>
        /// 查询条件
        /// </summary>
        Hashtable ht_where = new Hashtable();
       public Hashtable ht = new Hashtable();
        public ucHWQS_B()
        {
            InitializeComponent();
                        //初始化分页回调(带分页的页面，先把这个放上)
            ucPager1.DFT = new delegateForThread(STR_BeginBind);
            // PublicDS.PublisDsUser.Tables["用户信息表"].Rows[0]["买家角色编号"].ToString();
        }

      
        /// <summary>
        /// 设置默认搜索条件(这个例子中的哈希表键值一个也不能少)
        /// </summary>
        private void setDefaultSearch()
        {
            //ht_where["GetCustomersDataPage_NAME"] = "GetCustomersDataPage";  //每页显示的条数(必须设置)
            //ht_where["page_index"] = "0";
            //ht_where["page_size"] = "25";  //每页显示的条数(必须设置)
            //ht_where["serach_Row_str"] = " * "; //检索字段(必须设置)
            //ht_where["search_tbname"] = " (select a.Number,('T'+a.Number) as  提货单号,( 'F'+a.Number ) as 发货单号,a.ZBDBXXBBH as 中标定标单号,b.Z_SPBH as 商品编号,b.Z_SPMC as 商品名称,b.Z_GG as 规格,a.ZBDJ as 定标价格,a.T_THSL as 提货数量,T_DJHKJE as 发货金额,c.I_JYFMC as 卖方名称,b.Z_HTBH as 电子购货合同,b.Y_YSYDDMJJSBH as 买方角色编号 ,a.F_DQZT as 当前状态,a.F_FHDSCSJ as 发货单生成时间,b.Z_HTZT as 合同状态  from AAA_THDYFHDXXB as a left join AAA_ZBDBXXB as b on a.ZBDBXXBBH = b.Number left join AAA_DLZHXXB as c on b.T_YSTBDDLYX=c.B_DLYX ) as tab   ";  //检索的表(必须设置)
            //ht_where["search_mainid"] = " Number ";  //所检索表的主键(必须设置)
            //ht_where["search_str_where"] = " 1=1 and (当前状态='已录入发货信息'  ) and 买方角色编号='" + PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["买家角色编号"].ToString() + "' and 合同状态='定标' ";  //检索条件(必须设置)
            //ht_where["search_paixu"] = " asc ";  //排序方式(必须设置)
            //ht_where["search_paixuZD"] = " 发货单生成时间 ";  //用于排序的字段(必须设置)

            ////查询条件
            //if (!txtSPMC.Text.Trim().Equals(""))
            //{
            //    ht_where["search_str_where"] += " and 商品名称 like '%"+txtSPMC.Text.Trim()+"%' ";
            //}
            //if (!ucT_fhdh.Text.Trim().Equals(""))
            //{
            //    ht_where["search_str_where"] += " and 发货单号 like '%" + ucT_fhdh.Text.Trim() + "%' ";
            //}

            ht_where["page_index"] = "0";
            ht_where["page_size"] = "25";  //每页显示的条数(必须设置)
            ht_where["webmethod"] = "货物收发B区";
            Hashtable ht_tiaojian = new Hashtable();
            ht_tiaojian["标签名"] = "货物签收";
            ht_tiaojian["买方编号"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["买家角色编号"].ToString();
            ht_tiaojian["发货单号"] = ucT_fhdh.Text.Trim();
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
            this.Enabled = true;
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
            this.Enabled = false;
            //更改搜索条件
            setDefaultSearch();
           // ht_where["search_str_where"] = " 1=1 ";  //检索条件(必须设置)
            //执行查询
            GetData();
        }
      
     

        private void ucHWQS_B_Load(object sender, EventArgs e)
        {
            GetData();
            
       
            //处理下拉框间距
           // CBleixing.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.CB_DrawItem);

           // GetData(null);
       
        }

        private void btn_export_Click(object sender, EventArgs e)
        {
            ////设置要隐藏的列(原分页中，有些列可能是隐藏不显示的，导出时，可以选择滤掉)
            //string[] HideColumns = new string[] { "Number", "中标定标单号", "当前状态","发货单生成时间","合同状态","买方角色编号"};
            ////设置导出语句
            //string sql = "select * from (select a.Number,('T'+a.Number) as  提货单号,( 'F'+a.Number ) as 发货单号,a.ZBDBXXBBH as 中标定标单号,b.Z_SPBH as 商品编号,b.Z_SPMC as 商品名称,b.Z_GG as 规格,a.ZBDJ as 定标价格,a.T_THSL as 提货数量,T_DJHKJE as 发货金额,c.I_JYFMC as 卖方名称,b.Z_HTBH as 电子购货合同,b.Y_YSYDDMJJSBH as 买方角色编号 ,a.F_DQZT as 当前状态,a.F_FHDSCSJ as 发货单生成时间,b.Z_HTZT as 合同状态  from AAA_THDYFHDXXB as a left join AAA_ZBDBXXB as b on a.ZBDBXXBBH = b.Number left join AAA_DLZHXXB as c on b.T_YSTBDDLYX=c.B_DLYX ) as tab where  " + ht_where["search_str_where"].ToString();
            //cMyXls1.BeginRunFrom_ht_where(sql, HideColumns);

            Hashtable ht_export = new Hashtable();
            ht_export["filename"] = "货物签收";
            ht_export["webmethod"] = "货物收发B区导出";
            ht_export["tiaojian"] = (Hashtable)ht_where["tiaojian"];           
            cMyXls1.BeginRunFrom_ht_where(ht_export);


        }


        public void SRT_demo_Run(Hashtable InPutHT)
        {
            RunThreadClasshwqs OTD = new RunThreadClasshwqs(InPutHT, new delegateForThread(SRT_demo));
            Thread trd = new Thread(new ThreadStart(OTD.BeginRun));
            trd.IsBackground = true;
            trd.Start();
        }

        //显示线程处理结果的函数(用于处理线程返回数据),调用异步委托
        private void SRT_demo(Hashtable OutPutHT)
        {
            try { Invoke(new delegateForThreadShow(SRT_demo_Invoke), new Hashtable[] { OutPutHT }); }
            catch (Exception ex) { Support.StringOP.WriteLog("委托回调错误：" + ex.ToString()); }
        }
        //处理非线程创建的控件
        private void SRT_demo_Invoke(Hashtable OutPutHT)
        {
          
            //返回值后的处理
            DataSet dsreturn = (DataSet)OutPutHT["返回值"];

            string zt = dsreturn.Tables["返回值单条"].Rows[0]["执行结果"].ToString();
            string showstr = dsreturn.Tables["返回值单条"].Rows[0]["提示文本"].ToString();
            string Nowuyiyi = dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"].ToString();
            //显示执行结果
            switch (zt)
            {
                case "ok":
                    //给出表单提交成功的提示
                    ArrayList Almsg3 = new ArrayList();
                    Almsg3.Add("");
                    Almsg3.Add(showstr);
                    FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "其他", "", Almsg3);
                    FRSE3.ShowDialog();
                    if (Nowuyiyi.Trim().Equals("nowyy"))
                    {
                        (ht["from"] as jhjx_hwqsForm).Dispose();
                    }
                    GetData();
                    this.Enabled = true;
                    break;
                case "pass":
                    ArrayList Almsg5 = new ArrayList();
                    Almsg5.Add("");
                    Almsg5.Add(showstr);

                    FormAlertMessage FRSE5 = new FormAlertMessage("确定取消", "问号", "", Almsg5);
                    DialogResult dialogResult = FRSE5.ShowDialog();
                    if (dialogResult == DialogResult.Yes)
                    {
                        ht["ispass"] = "ok";
                        SRT_demo_Run(ht);
                    }
                    else
                    {
                        this.Enabled = true;
                        jhjx_hwqsForm jjj = ht["from"] as jhjx_hwqsForm;
                        jjj.PBload.Visible = false;
                        jjj.palbtn.Enabled = true;
                    }
                    
                    //GetData();
                    break;
                case "err1":
                     ArrayList Almsg6 = new ArrayList();
                    Almsg6.Add("");
                    Almsg6.Add(showstr);// 这里是提交失败的提示，可能是程序出错，可能是条件不足等
                    FormAlertMessage FRSE6 = new FormAlertMessage("仅确定", "其他", "", Almsg6);
                    FRSE6.ShowDialog();
                    this.Enabled = true;
                        jhjx_hwqsForm jjjx1 = ht["from"] as jhjx_hwqsForm;
                        jjjx1.PBload.Visible = false;
                        jjjx1.palbtn.Enabled = true;
                       
                    
                    break;

                default:
                    ArrayList Almsg4 = new ArrayList();
                    Almsg4.Add("");
                    Almsg4.Add(showstr);// 这里是提交失败的提示，可能是程序出错，可能是条件不足等
                    FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "其他", "", Almsg4);
                    FRSE4.ShowDialog();
                    this.Enabled = true;
                        jhjx_hwqsForm jjjx = ht["from"] as jhjx_hwqsForm;
                        jjjx.PBload.Visible = false;
                        jjjx.palbtn.Enabled = true;
                        jjjx.Dispose();
                    
                    break;
            }



        }

        private void 无异议收货ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripItem ts = sender as ToolStripItem;
          
            //Hashtable htpass = new Hashtable();
            
             ht["type"] = ts.Name;
                           
             ht["登陆邮箱"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString();
             #region//zhouli作废--2017.07.03 新框架变更修改参数
             //ht["结算账户类型"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["结算账户类型"].ToString();
             //ht["买家角色编号"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["买家角色编号"].ToString();
             #endregion

             // ht["投标单号"] = dataGridView1.SelectedCells[1].Value.ToString();  
             ht["ispass"] = "no";

             if (ts.Name.ToString().Equals("无异议收货"))
             {
                 this.Enabled = false;
                 SRT_demo_Run(ht);
             }
             else 
             {
                 jhjx_hwqsForm hwsf = new jhjx_hwqsForm(this);
                 hwsf.ShowDialog();

             }
             
             


        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

            string columnName = dataGridView1.Columns[e.ColumnIndex].Name;
            if (columnName == "对应电子购货合同编号" && e.RowIndex > -1)
            {
                int pagenumber = 8;//预估页数，要大于可能的最大页数
                object[] CSarr = new object[pagenumber + 2];
                string[] MKarr = new string[pagenumber + 2];
                for (int i = 0; i < pagenumber; i++)
                {
                    Hashtable htcs = new Hashtable();
                    htcs["纯数字索引"] = (i + 1).ToString(); //这个参数必须存在。
                    htcs["数据地址"] = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[2] + "/pingtaiservices/mobanrun/FMPTDZGHHT.aspx?Number=" + dataGridView1.Rows[e.RowIndex].Cells["中标定标单号"].Value;
                    htcs["要传递的参数1"] = "参数1";
                    CSarr[i] = htcs; //模拟参数
                    MKarr[i] = "Y3_Business_Muban.dll)!客户端主程序.SubForm.NewCenterForm.MuBan.UCFMPTDZGHHT";

                }
                Hashtable htcs1 = new Hashtable();
                htcs1["要传递的参数1"] = dataGridView1.Rows[e.RowIndex].Cells["中标定标单号"].Value;
                CSarr[pagenumber] = htcs1;
                MKarr[pagenumber] = "Y3_Business_Muban.dll)!客户端主程序.SubForm.NewCenterForm.MuBan.UCFMPTDZGHHT_1";

                Hashtable htcs2 = new Hashtable();
                htcs1["要传递的参数1"] = dataGridView1.Rows[e.RowIndex].Cells["中标定标单号"].Value;
                CSarr[pagenumber + 1] = htcs1;
                MKarr[pagenumber + 1] = "Y3_Business_Muban.dll)!客户端主程序.SubForm.NewCenterForm.MuBan.UCFMPTDZGHHT_2";
                FormDZGHHT dy = new FormDZGHHT("电子购货合同", CSarr, MKarr, dataGridView1.Rows[e.RowIndex].Cells["中标定标单号"].Value.ToString());
                dy.Show();
            }
            if (columnName == "发货单号" && e.RowIndex > -1)
            {
                string cs = dataGridView1.Rows[e.RowIndex].Cells["发货单号"].Value.ToString();
                fmDY dy = new fmDY("查看并打印发货单", cs, new string[] { "Y5_Business_HWSF.dll)!客户端主程序.SubForm.NewCenterForm.SUBUC.HWSF.FHDDY" });
                dy.ShowDialog();

            }
            if (columnName == "提货单号" && e.RowIndex > -1)
            {
                string cs = dataGridView1.Rows[e.RowIndex].Cells["提货单号"].Value.ToString();
                fmDY dy = new fmDY("查看并打印提货单", cs, new string[] { "Y5_Business_HWSF.dll)!客户端主程序.SubForm.NewCenterForm.SUBUC.HWSF.THDDY" });
                dy.ShowDialog();

            }
            
            if (columnName == "签收操作" && e.RowIndex > -1)
            {
             
                DataGridViewRow dgr = this.dataGridView1.Rows[e.RowIndex];
                string Number = dgr.Cells["Number"].Value.ToString();
                string dqzt = dgr.Cells["当前状态"].Value.ToString();
                ht["Number"] = Number;
                ContextMenuStrip ctm = new ContextMenuStrip();
                ToolStripMenuItem ts = null;
                switch (dqzt)
                {
                    case "已录入发货信息":
                        ctm.Items.Clear();

                        //加入无疑义发货标签
                        ts = new ToolStripMenuItem();
                        ts.Name = "无异议收货";
                        ts.Text = "无异议收货";
                        ctm.Items.Insert(0, ts);

                        //加入请重新发货标签
                        ts = new ToolStripMenuItem();
                        ts.Name = "请重新发货";
                        ts.Text = "请重新发货";
                        ctm.Items.Insert(1, ts);

                        //加入部分收货标签
                        ts = new ToolStripMenuItem(); 
                        ts.Name = "部分收货";
                        ts.Text = "部分收货";
                        ctm.Items.Insert(2, ts);

                        //加入有异议收货标签
                        ts = new ToolStripMenuItem(); 
                        ts.Name = "有异议收货";
                        ts.Text = "有异议收货";
                        ctm.Items.Insert(3, ts);
                        break;
                    case "已录入补发备注":
                        ctm.Items.Clear();
                        //加入有异议收货标签
                        ts = new ToolStripMenuItem();
                        ts.Name = "补发货物无异议收货";
                        ts.Text = "补发货物无异议收货"; 
                        ctm.Items.Insert(0, ts);
                        break;
                    default:
                        ctm.Items.Clear();
                        break;

                }


                ctm.ShowCheckMargin = false;
                ctm.ShowImageMargin = false;
                //将当前行索引给菜单备用
                if (ctm != null && ctm.Items.Count > 0)
                {
                    foreach (ToolStripMenuItem items in ctm.Items)
                    {
                        items.Tag = e.RowIndex;
                        items.Click += new System.EventHandler(this.无异议收货ToolStripMenuItem_Click);
                    }
                }
                //获得cell的位置
                DataGridViewCell cell = this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                Rectangle rect = this.dataGridView1.GetCellDisplayRectangle(cell.ColumnIndex, cell.RowIndex, true);
                //显示菜单
                ctm.Show(dataGridView1, new Point(rect.Location.X + 5, rect.Location.Y + rect.Height));
            }
        }

    }
}
