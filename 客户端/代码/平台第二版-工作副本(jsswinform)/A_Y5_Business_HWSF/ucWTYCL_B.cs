using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using 客户端主程序.DataControl;
using 客户端主程序.Support;
using System.Threading;
using 客户端主程序.SubForm.NewCenterForm.MuBan;

namespace 客户端主程序.SubForm.NewCenterForm.SUBUC.HWSF
{
    public partial class ucWTYCL_B : UserControl
    {
              /// <summary>
        /// 查询条件
        /// </summary>
        Hashtable ht_where = new Hashtable();
        public ucWTYCL_B()
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
            //ht_where["serach_Row_str"] = " * "; //检索字段(必须设置)
            //ht_where["search_tbname"] = " ( select '类别'='卖家',t.Number 提货单编号,d.Number 中标定标信息表编号,d.Z_SPMC 商品名称,d.Z_GG 规格,d.Z_SPBH 商品编号,'买家名称'=(select DL.I_JYFMC from AAA_DLZHXXB DL where DL.J_BUYJSBH=d.Y_YSYDDMJJSBH),'卖家名称'=(select DL.I_JYFMC from AAA_DLZHXXB DL where DL.J_SELJSBH=d.T_YSTBDMJJSBH),t.ZBDJ 定标价格,t.T_THSL 发货单数量,d.Z_HTBH 电子购货合同编号,('F'+t.Number) 发货单编号,t.F_DQZT 问题类别,(case when t.F_DQZT in ('部分收货','已录入补发备注') then '部分收货' when t.F_DQZT in ('有异议收货') then '有异议收货' when t.F_DQZT in ('请重新发货') then '请重新发货' end) 问题类别及详情,(case t.F_DQZT when '部分收货' then t.F_BUYBFSHSJQSSL when '已录入补发备注' then t.F_BUYBFSHSJQSSL  else t.T_THSL end) 已签收数量,(case t.F_DQZT when '部分收货' then t.F_BUYBFSHCZSJ when '有异议收货' then t.F_BUYYYYSHCZSJ when '请重新发货' then t.F_BUYQZXFHCZSJ when '已录入补发备注' then t.F_BUYBFSHCZSJ  end) 签收时间,d.T_YSTBDMJJSBH 原始投标单卖家角色编号,d.Y_YSYDDMJJSBH 原始预订单买家角色编号,d.Y_YSYDDDLYX 原始预订单登录邮箱,d.Y_YSYDDJSZHLX 原始预订单结算账户类型,d.T_YSTBDDLYX 原始投标单登录邮箱,d.T_YSTBDJSZHLX 原始投标单结算账户类型,t.F_FPHM 发票编号,case t.F_FPSFSHTH when '是' then '发票随货同行' else '发票单独邮寄' end as '发票发送方式' from AAA_THDYFHDXXB t left join AAA_ZBDBXXB d on t.ZBDBXXBBH=d.Number where d.Z_HTZT='定标' and d.Z_QPZT='未开始清盘' and t.F_DQZT in ('部分收货','已录入补发备注','有异议收货','请重新发货') and d.T_YSTBDMJJSBH='" + PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["卖家角色编号"].ToString() + "' union all  select '类别'='买家',t.Number 提货单编号,d.Number 中标定标信息表编号,d.Z_SPMC 商品名称,d.Z_GG 规格,d.Z_SPBH 商品编号,'买家名称'=(select DL.I_JYFMC from AAA_DLZHXXB DL where DL.J_BUYJSBH=d.Y_YSYDDMJJSBH),'卖家名称'=(select DL.I_JYFMC from AAA_DLZHXXB DL where DL.J_SELJSBH=d.T_YSTBDMJJSBH),t.ZBDJ 定标价格,t.T_THSL 发货单数量,d.Z_HTBH 电子购货合同编号,('F'+t.Number) 发货单编号,t.F_DQZT 问题类别,(case when t.F_DQZT in ('部分收货','已录入补发备注') then '部分收货' when t.F_DQZT in ('有异议收货') then '有异议收货' when t.F_DQZT in ('请重新发货') then '请重新发货' end) 问题类别及详情,(case t.F_DQZT when '部分收货' then t.F_BUYBFSHSJQSSL when '已录入补发备注' then t.F_BUYBFSHSJQSSL  else t.T_THSL end) 已签收数量,(case t.F_DQZT when '部分收货' then t.F_BUYBFSHCZSJ when '有异议收货' then t.F_BUYYYYSHCZSJ when '请重新发货' then t.F_BUYQZXFHCZSJ when '已录入补发备注' then t.F_BUYBFSHCZSJ  end) 签收时间,d.T_YSTBDMJJSBH 原始投标单卖家角色编号,d.Y_YSYDDMJJSBH 原始预订单买家角色编号,d.Y_YSYDDDLYX 原始预订单登录邮箱,d.Y_YSYDDJSZHLX 原始预订单结算账户类型,d.T_YSTBDDLYX 原始投标单登录邮箱,d.T_YSTBDJSZHLX 原始投标单结算账户类型,t.F_FPHM 发票编号,case t.F_FPSFSHTH when '是' then '发票随货同行' else '发票单独邮寄' end as '发票发送方式' from AAA_THDYFHDXXB t left join AAA_ZBDBXXB d on t.ZBDBXXBBH=d.Number where d.Z_HTZT='定标' and d.Z_QPZT='未开始清盘' and t.F_DQZT in ('已录入补发备注','有异议收货') and d.Y_YSYDDMJJSBH='" + PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["买家角色编号"].ToString() + "' ) as tab ";  //检索的表(必须设置)
            //ht_where["search_mainid"] = " 提货单编号 ";  //所检索表的主键(必须设置)
            //ht_where["search_str_where"] = " 1=1 ";  //检索条件(必须设置)
            //ht_where["search_paixu"] = " asc ";  //排序方式(必须设置)
            //ht_where["search_paixuZD"] = " 签收时间 ";  //用于排序的字段(必须设置)

            ht_where["page_index"] = "0";
            ht_where["page_size"] = "25";  //每页显示的条数(必须设置)
            ht_where["webmethod"] = "货物收发B区";
            Hashtable ht_tiaojian = new Hashtable();
            ht_tiaojian["标签名"] = "问题与处理";
            ht_tiaojian["买方编号"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["买家角色编号"].ToString();
            ht_tiaojian["卖方编号"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["卖家角色编号"].ToString();
            ht_tiaojian["发货单号"] = txtFHDH.Text.Trim();
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
        public void GetData(Hashtable HT_Where_temp)
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
            //ht_where["search_str_where"] = " 1=1 and 商品名称 like '%" + txtSPMC.Text.Trim() + "%' and 发货单编号 like '%" + txtFHDH.Text.Trim() + "%'";  //检索条件(必须设置)
            //执行查询
            GetData(ht_where);
        }
      
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string columnName = dataGridView1.Columns[e.ColumnIndex].Name;
            if (columnName == "操作" && e.RowIndex > -1)
            {
                CMCview.Items.Clear();
                //设置菜单最精简
                CMCview.ShowCheckMargin = false;
                CMCview.ShowImageMargin = false;
                //将当前行索引给菜单备用
                //ContextMenuStrip ctm = new ContextMenuStrip();
                ToolStripMenuItem ts = null;
                if (dataGridView1.Rows[e.RowIndex].Cells["类别"].Value.ToString()=="卖家")
                {
                    switch (dataGridView1.Rows[e.RowIndex].Cells["问题类别"].Value.ToString())
                    {
                        case "部分收货":
                             ts = new ToolStripMenuItem();
                            ts.Name = "录入补发备注";
                            ts.Text = "录入补发备注";
                            CMCview.Items.Insert(0, ts);
                            break;
                        case "已录入补发备注":
                            ts = new ToolStripMenuItem();
                            ts.Name = "录入补发备注";
                            ts.Text = "录入补发备注";
                            CMCview.Items.Insert(0, ts);
                            ts = new ToolStripMenuItem();
                            ts.Name = "请买方“无异议收货”";
                            ts.Text = "请买方“无异议收货”";
                            CMCview.Items.Insert(1, ts);
                            break;
                        case "有异议收货":
                            ts = new ToolStripMenuItem();
                            ts.Name = "卖方主动退货";
                            ts.Text = "卖方主动退货";
                            CMCview.Items.Insert(0, ts);
                            break;
                        case "请重新发货":
                            ts = new ToolStripMenuItem();
                            ts.Name = "同意重新发货";
                            ts.Text = "同意重新发货";
                            CMCview.Items.Insert(0, ts);
                            break;
                        default:
                            break;
                    }
                }
                else if (dataGridView1.Rows[e.RowIndex].Cells["类别"].Value.ToString() == "买家")
                {
                    switch (dataGridView1.Rows[e.RowIndex].Cells["问题类别"].Value.ToString())
                    {
                        case "已录入补发备注":
                            ts = new ToolStripMenuItem();
                            ts.Name = "补发货物无异议收货";
                            ts.Text = "无异议收货";
                            CMCview.Items.Insert(0, ts);
                            break;
                        case "有异议收货":
                            ts = new ToolStripMenuItem();
                            ts.Name = "有异议收货后无异议收货";
                            ts.Text = "无异议收货";
                            CMCview.Items.Insert(0, ts);
                            break;
                        default:
                            break;
                    }
                }
                
                

                //将当前行索引给菜单备用
                if (CMCview != null && CMCview.Items.Count > 0)
                {
                    foreach (ToolStripMenuItem items in CMCview.Items)
                    {
                        items.Tag = e.RowIndex;
                        items.Click += new System.EventHandler(this.卖家主动退货_Click);
                    }
                }
                //获得cell的位置
                DataGridViewCell cell = this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                Rectangle rect = this.dataGridView1.GetCellDisplayRectangle(cell.ColumnIndex, cell.RowIndex, true);
                //显示菜单
                CMCview.Show(dataGridView1, new Point(rect.Location.X + 5, rect.Location.Y + rect.Height));
            }
            else if (columnName == "电子购货合同编号" && e.RowIndex > -1)
            {
                int pagenumber = 8;//预估页数，要大于可能的最大页数
                object[] CSarr = new object[pagenumber + 2];
                string[] MKarr = new string[pagenumber + 2];
                for (int i = 0; i < pagenumber; i++)
                {
                    Hashtable htcs = new Hashtable();
                    htcs["纯数字索引"] = (i + 1).ToString(); //这个参数必须存在。
                    htcs["数据地址"] = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[2] + "/pingtaiservices/mobanrun/FMPTDZGHHT.aspx?Number=" + dataGridView1.Rows[e.RowIndex].Cells["中标定标信息表编号"].Value;
                    htcs["要传递的参数1"] = "参数1";
                    CSarr[i] = htcs; //模拟参数
                    MKarr[i] = "Y3_Business_Muban.dll)!客户端主程序.SubForm.NewCenterForm.MuBan.UCFMPTDZGHHT";

                }
                Hashtable htcs1 = new Hashtable();
                htcs1["要传递的参数1"] = dataGridView1.Rows[e.RowIndex].Cells["中标定标信息表编号"].Value;
                CSarr[pagenumber] = htcs1;
                MKarr[pagenumber] = "Y3_Business_Muban.dll)!客户端主程序.SubForm.NewCenterForm.MuBan.UCFMPTDZGHHT_1";

                Hashtable htcs2 = new Hashtable();
                htcs1["要传递的参数1"] = dataGridView1.Rows[e.RowIndex].Cells["中标定标信息表编号"].Value;
                CSarr[pagenumber + 1] = htcs1;
                MKarr[pagenumber + 1] = "Y3_Business_Muban.dll)!客户端主程序.SubForm.NewCenterForm.MuBan.UCFMPTDZGHHT_2";
                FormDZGHHT dy = new FormDZGHHT("电子购货合同", CSarr, MKarr, dataGridView1.Rows[e.RowIndex].Cells["中标定标信息表编号"].Value.ToString());
                dy.Show();
            }
            else if (columnName == "发货单编号" && e.RowIndex > -1)
            {
                string cs = dataGridView1.Rows[e.RowIndex].Cells["发货单编号"].Value.ToString();
                fmDY dy = new fmDY("查看并打印发货单", cs, new string[] { "Y5_Business_HWSF.dll)!客户端主程序.SubForm.NewCenterForm.SUBUC.HWSF.FHDDY" });
                dy.ShowDialog();
            }
            else if (columnName == "问题类别及详情" && e.RowIndex > -1)
            {
                Hashtable ht = new Hashtable();
                ht["Number"] = dataGridView1.Rows[e.RowIndex].Cells["提货单编号"].Value.ToString().TrimStart('T');
                ht["type"] = dataGridView1.Rows[e.RowIndex].Cells["问题类别及详情"].Value.ToString();
                jhjx_hwqswtclForm dia = new jhjx_hwqswtclForm(ht);
                dia.ShowDialog();
            }
        }
        private void ucWTYCL_B_Load(object sender, EventArgs e)
        {
            GetData(null);
        }

        public string fhdh = "";//发货单号
        public string cz = "";//操作
        private void 卖家主动退货_Click(object sender, EventArgs e)
        {
            //具体点击菜单后的操作
            ToolStripMenuItem TSMT = (ToolStripMenuItem)sender;
            int RowIndex = (int)TSMT.Tag;

            switch (TSMT.Text)
            {
                case "录入补发备注":
                    fhdh = dataGridView1.Rows[RowIndex].Cells["发货单编号"].Value.ToString();//发货单号
                    cz = "录入补发备注";//操作
                    LRBFBZ lrbz = new LRBFBZ(this);
                    lrbz.ShowDialog();
                    break;
                case "请买方“无异议收货”":
                    fhdh = dataGridView1.Rows[RowIndex].Cells["发货单编号"].Value.ToString();//发货单号
                    cz = "请买方“无异议收货”";//操作
                    TQBuyWYYSH qs = new TQBuyWYYSH(this);
                    qs.ShowDialog();
                    break;
                case "卖方主动退货":
                    #region//生成提交参数

                    //为什么要传买家角色编号而不传卖家角色编号呢？原因：传角色编号只是为了验证当前账户的状态，卖家和经纪人同时有买家的角色，所以直接用买家角色编号；不然需要判断账户类型，来决定传卖家角色编号还是经纪人角色编号
                    string BUYjsbh = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["买家角色编号"].ToString();
                    Hashtable htcs = new Hashtable();
                    htcs["买家角色编号"] = BUYjsbh;
                    htcs["提货单编号"] = dataGridView1.Rows[RowIndex].Cells["提货单编号"].Value.ToString();
                    htcs["操作"] = TSMT.Text;
                    #region//zhouli 作废 2014.07.08 新框架变更参数
                    //htcs["登录邮箱"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString();
                    //htcs["结算账户类型"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["结算账户类型"].ToString();
                    //htcs["卖家角色编号"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["卖家角色编号"].ToString();
                    //htcs["卖家登录邮箱"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString();
                    //htcs["买家登录邮箱"] = dataGridView1.Rows[RowIndex].Cells["原始预订单登录邮箱"].Value.ToString();
                    //htcs["提醒对象用户名"] = dataGridView1.Rows[RowIndex].Cells["买家名称"].Value.ToString();
                    //htcs["提醒对象结算账户类型"] = dataGridView1.Rows[RowIndex].Cells["原始预订单结算账户类型"].Value.ToString();
                    //htcs["提醒对象角色编号"] = dataGridView1.Rows[RowIndex].Cells["原始预订单买家角色编号"].Value.ToString();
                    //htcs["创建人"] = dataGridView1.Rows[RowIndex].Cells["原始投标单登录邮箱"].Value.ToString();
                    #endregion
                    #endregion

                    Save(htcs,"");
                    break;
                case "同意重新发货":
                    ArrayList Almsg3 = new ArrayList();
                    Almsg3.Add("");
                    Almsg3.Add("您“同意重新发货”后，对应的T" + dataGridView1.Rows[RowIndex].Cells["提货单编号"].Value.ToString() + "提货单与" + dataGridView1.Rows[RowIndex].Cells["发货单编号"].Value.ToString() + "发货单作废，");
                    Almsg3.Add("您需根据新生成的提货单重新发货。");
                    FormAlertMessage FRSE3 = new FormAlertMessage("确定取消", "问号", "同意重新发货提交", Almsg3);
                    if(FRSE3.ShowDialog()==DialogResult.Yes)
                    {
                        #region//生成提交参数

                        //为什么要传买家角色编号而不传卖家角色编号呢？原因：传角色编号只是为了验证当前账户的状态，卖家和经纪人同时有买家的角色，所以直接用买家角色编号；不然需要判断账户类型，来决定传卖家角色编号还是经纪人角色编号
                        BUYjsbh = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["买家角色编号"].ToString();
                        htcs = new Hashtable();
                        htcs["买家角色编号"] = BUYjsbh;
                        htcs["提货单编号"] = dataGridView1.Rows[RowIndex].Cells["提货单编号"].Value.ToString();                        
                        htcs["操作"] = TSMT.Text;
                        #region//zhouli 作废 2014.07.08 新框架变更参数
                        //htcs["Number"] = dataGridView1.Rows[RowIndex].Cells["提货单编号"].Value.ToString();
                        //htcs["登录邮箱"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString();
                        //htcs["结算账户类型"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["结算账户类型"].ToString();
                        //htcs["卖家角色编号"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["卖家角色编号"].ToString();
                        //htcs["提醒对象登陆邮箱"] = dataGridView1.Rows[RowIndex].Cells["原始预订单登录邮箱"].Value.ToString();
                        //htcs["提醒对象用户名"] = dataGridView1.Rows[RowIndex].Cells["买家名称"].Value.ToString();
                        //htcs["提醒对象结算账户类型"] = dataGridView1.Rows[RowIndex].Cells["原始预订单结算账户类型"].Value.ToString();
                        //htcs["提醒对象角色编号"] = dataGridView1.Rows[RowIndex].Cells["原始预订单买家角色编号"].Value.ToString();
                        //htcs["创建人"] = dataGridView1.Rows[RowIndex].Cells["原始投标单登录邮箱"].Value.ToString();
                        #endregion
                        #endregion

                        Save(htcs,"");
                    }
                    
                    break;
                case "无异议收货":
                    #region//生成提交参数

                    //为什么要传买家角色编号而不传卖家角色编号呢？原因：传角色编号只是为了验证当前账户的状态，卖家和经纪人同时有买家的角色，所以直接用买家角色编号；不然需要判断账户类型，来决定传卖家角色编号还是经纪人角色编号
                    BUYjsbh = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["买家角色编号"].ToString();                    
                    htcs = new Hashtable();
                    htcs["买家角色编号"] = BUYjsbh;                    
                    htcs["提货单编号"] = dataGridView1.Rows[RowIndex].Cells["提货单编号"].Value.ToString().TrimStart('T');                   
                    lblTHDBH.Text = dataGridView1.Rows[RowIndex].Cells["提货单编号"].Value.ToString().TrimStart('T');
                    htcs["操作"] = TSMT.Text;
                    lblType.Text = TSMT.Name;
                    lblTBDDLYX.Text = dataGridView1.Rows[RowIndex].Cells["原始投标单登录邮箱"].Value.ToString();
                    lblSelMC.Text = dataGridView1.Rows[RowIndex].Cells["卖家名称"].Value.ToString();
                    lblTBDJSLX.Text = dataGridView1.Rows[RowIndex].Cells["原始投标单结算账户类型"].Value.ToString();
                    lblSelJSBH.Text = dataGridView1.Rows[RowIndex].Cells["原始投标单卖家角色编号"].Value.ToString();
                    htcs["type"] = TSMT.Name;        
                     #region//zhouli 作废 2014.07.08 新框架变更参数
                    //htcs["买家名称"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["交易方名称"].ToString();
                    //htcs["Number"] = dataGridView1.Rows[RowIndex].Cells["提货单编号"].Value.ToString().TrimStart('T');
                    //htcs["登陆邮箱"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString();
                    //htcs["结算账户类型"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["结算账户类型"].ToString();                               
                    //htcs["提醒对象登陆邮箱"] = dataGridView1.Rows[RowIndex].Cells["原始投标单登录邮箱"].Value.ToString();                    
                    //htcs["提醒对象用户名"] = dataGridView1.Rows[RowIndex].Cells["卖家名称"].Value.ToString();                    
                    //htcs["提醒对象结算账户类型"] = dataGridView1.Rows[RowIndex].Cells["原始投标单结算账户类型"].Value.ToString();                    
                    //htcs["提醒对象角色编号"] = dataGridView1.Rows[RowIndex].Cells["原始投标单卖家角色编号"].Value.ToString();                    
                    //htcs["创建人"] = dataGridView1.Rows[RowIndex].Cells["原始预订单登录邮箱"].Value.ToString();
                    #endregion
                    #endregion

                    Save(htcs, "no");
                    break;
                default:
                    showAlertY("非法操作！");
                    break;
            }


        }

        private void Save(Hashtable htcs,string ispass)
        {
            htcs["ispass"] = ispass;
            //开启线程，通过合同编号和买家角色编号，获取一些数据。
            this.Enabled = false;
            //panelUCedit.Enabled = false;
            //resetloadLocation(BBedit, PBload);
            DataSet dsinfosave = new DataSet();
            Hashtable htc = new Hashtable();
            DataTable dtc = StringOP.GetDataTableFormHashtable(htcs);
            dsinfosave.Tables.Add(dtc);
            delegateForThread dft = new delegateForThread(ShowThreadResult_save);
            NewDataControl.WTYCL RCthd = new NewDataControl.WTYCL(dsinfosave, dft);
            Thread trd = new Thread(new ThreadStart(RCthd.BeginRun));
            trd.IsBackground = true;
            trd.Start();
        }
        //显示线程处理结果的函数(用于处理线程返回数据)
        private void ShowThreadResult_save(Hashtable returnHT)
        {
            try
            {
                //调用异步委托
                Invoke(new delegateForThreadShow(ShowThreadResult_save_Invoke), new Hashtable[] { returnHT });
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("委托回调错误：" + ex.ToString());
            }
        }
        //处理非线程创建的控件
        private void ShowThreadResult_save_Invoke(Hashtable returnHT)
        {
            this.Enabled = true;
            //panelUCedit.Enabled = true;
            //PBload.Visible = false;
            //显示执行结果
            DataSet returnds = returnHT["返回值"] as DataSet;
            if (returnds != null && returnds.Tables[0].Rows.Count > 0)
            {
                switch (returnds.Tables[0].Rows[0]["执行结果"].ToString())
                {
                    case "ok":
                        //给出表单提交成功的提示
                        ArrayList Almsg = new ArrayList();
                        Almsg.Add("");
                        Almsg.Add(returnds.Tables[0].Rows[0]["提示文本"].ToString());
                        FormAlertMessage FRSE = new FormAlertMessage("仅确定", "其他", "处理成功", Almsg);
                        FRSE.ShowDialog();

                        //更改搜索条件
                        setDefaultSearch();
                        ht_where["search_str_where"] = " 1=1 and 商品名称 like '%" + txtSPMC.Text.Trim() + "%' and 发货单编号 like '%" + txtFHDH.Text.Trim() + "%'";  //检索条件(必须设置)
                        //执行查询
                        GetData(ht_where);
                        
                        return;
                    case "err":
                        ArrayList Almsg7 = new ArrayList();
                        Almsg7.Add("");
                        Almsg7.Add(returnds.Tables[0].Rows[0]["提示文本"].ToString());
                        FormAlertMessage FRSE7 = new FormAlertMessage("仅确定", "其他", "系统忙", Almsg7);
                        FRSE7.ShowDialog();
                        return;
                    case "pass":
                        ArrayList Almsg3 = new ArrayList();
                        Almsg3.Add("");
                        Almsg3.Add(returnds.Tables[0].Rows[0]["提示文本"].ToString());
                        FormAlertMessage FRSE3 = new FormAlertMessage("确定取消", "问号", "处理成功", Almsg3);
                        if (FRSE3.ShowDialog() == DialogResult.Yes)
                        {
                            #region//生成提交参数
                            //具体点击菜单后的操作
                            
                            //为什么要传买家角色编号而不传卖家角色编号呢？原因：传角色编号只是为了验证当前账户的状态，卖家和经纪人同时有买家的角色，所以直接用买家角色编号；不然需要判断账户类型，来决定传卖家角色编号还是经纪人角色编号
                            string BUYjsbh = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["买家角色编号"].ToString();
                            DataSet dsinfosave = new DataSet();
                            Hashtable htcs = new Hashtable();
                            htcs["买家角色编号"] = BUYjsbh;
                            htcs["提货单编号"] = lblTHDBH.Text;                            
                            htcs["操作"] = "无异议收货";
                            htcs["type"] = lblType.Text;
                            #region//zhouli 作废 2014.07.08 新框架变更参数
                            //htcs["Number"] = lblTHDBH.Text; 
                            //htcs["登陆邮箱"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString();
                            //htcs["结算账户类型"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["结算账户类型"].ToString();                            
                            //htcs["提醒对象登陆邮箱"] = lblTBDDLYX.Text;
                            //htcs["提醒对象用户名"] = lblSelMC.Text;
                            //htcs["提醒对象结算账户类型"] = lblTBDJSLX.Text;
                            //htcs["提醒对象角色编号"] = lblSelJSBH.Text;
                            //htcs["创建人"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString();
                            #endregion
                            #endregion

                            Save(htcs, "ok");
                        }
                        return;
                    case "OpIsInvalid":
                        //给出表单提交成功的提示
                        Almsg = new ArrayList();
                        Almsg.Add("");
                        Almsg.Add(returnds.Tables[0].Rows[0]["提示文本"].ToString());
                        FRSE = new FormAlertMessage("仅确定", "其他", "处理失败", Almsg);
                        FRSE.ShowDialog();

                        //更改搜索条件
                        setDefaultSearch();
                        ht_where["search_str_where"] = " 1=1 and 商品名称 like '%" + txtSPMC.Text.Trim() + "%' and 发货单编号 like '%" + txtFHDH.Text.Trim() + "%'";  //检索条件(必须设置)
                        //执行查询
                        GetData(ht_where);
                        return;
                    default:
                        ArrayList Almsg4 = new ArrayList();
                        Almsg4.Add("");
                        FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "其他", "提交提货单失败", Almsg4);
                        FRSE4.ShowDialog();

                        return;
                }
            }
            else
            {
                ArrayList Almsg4 = new ArrayList();
                Almsg4.Add("");
                Almsg4.Add("系统繁忙，请稍后重试...");
                FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "其他", "提交提货单失败", Almsg4);
                FRSE4.ShowDialog();
                //this.Close();
                return;
            }


        }

        /// <summary>
        /// 显示错误提示
        /// </summary>
        /// <param name="err"></param>
        private void showAlertY(string err)
        {
            ArrayList Almsg3 = new ArrayList();
            Almsg3.Add("");
            Almsg3.Add(err);
            FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "其他", "预订单信息提交", Almsg3);
            FRSE3.ShowDialog();
        }
    }
}
