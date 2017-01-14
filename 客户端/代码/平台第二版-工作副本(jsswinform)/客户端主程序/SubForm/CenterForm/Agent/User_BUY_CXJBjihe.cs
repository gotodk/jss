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

namespace 客户端主程序.SubForm.CenterForm.Agent
{
    public partial class User_BUY_CXJBjihe : UserControl
    {
        /// <summary>
        /// 查询条件
        /// </summary>
        Hashtable ht_where = new Hashtable();

        public User_BUY_CXJBjihe()
        {
            InitializeComponent();

            //初始化分页回调(带分页的页面，先把这个放上)
            ucPager1.DFT = new delegateForThread(ShowThreadResult_BeginBind);
            //处理下拉框间距
            this.CBleixing.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.CB_DrawItem);
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

        //加载窗体后处理数据
        private void UserControl1_Load(object sender, EventArgs e)
        {
            CBleixing.SelectedIndex = 0;
            GetData(null);
        }

        //显示线程处理结果的函数(用于处理线程返回数据)
        private void ShowThreadResult_BeginBind(Hashtable returnHT)
        {
            try
            {
                //调用异步委托
                Invoke(new delegateForThreadShow(ShowThreadResult_BeginBind_Invoke), new Hashtable[] { returnHT });
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("委托回调错误：" + ex.ToString());
            }
        }
        //处理非线程创建的控件(线程获取到分页数据后的具体绑定处理)
        private void ShowThreadResult_BeginBind_Invoke(Hashtable OutPutHT)
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
                    dgvcs.Padding = new Padding(10,0,0,0);

                    this.dataGridView1.Columns[i].HeaderCell.Style = dgvcs;
                    this.dataGridView1.Columns[i].DefaultCellStyle = dgvcs;

                }
                this.dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            

        }

        /// <summary>
        /// 设置默认搜索条件(这个例子中的哈希表键值一个也不能少)
        /// </summary>
        private void setDefaultSearch()
        {
            ht_where["GetCustomersDataPage_NAME"] = "GetCustomersDataPage";  //每页显示的条数(必须设置)
            ht_where["page_index"] = "0";
            ht_where["page_size"] = "25";  //每页显示的条数(必须设置)
            ht_where["serach_Row_str"] = "* "; //检索字段(必须设置)
            ht_where["search_tbname"] = " ( select B.MJDLYX,B.MJYHM,B.CreateTime,'最大经济批量'=(select top 1 P.ZDJJPL from ZZ_PTSPXXB as P where P.SPBH=B.SPBH), '下达时间'=B.CreateTime, '预订单号'=B.Number,'商品编号'=B.SPBH,'商品名称'=B.SPMC,'型号规格'=B.GGXH, '供货周期'=B.HTZQ, '计价单位'=B.JJDW,'竞标状态'=B.ZT,'经济批量'=B.JJPL, '买入价格'=B.NMRJG,'订金金额'=B.ZFDJJE, '预订数量'=B.YDZSL,'已中标量'=B.YZBL,  '是否在冷静期内'=B.SFJRLJQ,'进入冷静期时间'=B.JRLJQSJ,  '最低价标的经济批量'=isnull((select top 1 Z.JJPL from ZZ_TBXXBZB as Z  join ZZ_TBXXB as ZHHH on Z.parentNumber=ZHHH.Number  where Z.SPBH =B.SPBH and  Z.HTQX = B.HTZQ and Z.ZT = '竞标' order by Z.TBJG ASC,ZHHH.CreateTime ASC),0),  '最低价标的投标价格'=isnull((select top 1 Z.TBJG from ZZ_TBXXBZB as Z  join ZZ_TBXXB as ZHHH on Z.parentNumber=ZHHH.Number  where Z.SPBH =B.SPBH and  Z.HTQX = B.HTZQ and Z.ZT = '竞标' order by Z.TBJG ASC,ZHHH.CreateTime ASC ),0.00),  '最低价标的投标拟售量'=isnull((select top 1 Z.TBYSL from ZZ_TBXXBZB as Z  join ZZ_TBXXB as ZHHH on Z.parentNumber=ZHHH.Number  where Z.SPBH =B.SPBH and  Z.HTQX = B.HTZQ and Z.ZT = '竞标' order by Z.TBJG ASC,ZHHH.CreateTime ASC ),0),   '参与竞标集合预订量'=isnull((select sum(Y.YDZSL-Y.YZBL) from ZZ_YDDXXB as Y where Y.SPBH =B.SPBH and  Y.HTZQ = B.HTZQ and Y.ZT = '竞标' and Y.NMRJG >= (select top 1 Z.TBJG from ZZ_TBXXBZB as Z  join ZZ_TBXXB as ZHHH on Z.parentNumber=ZHHH.Number  where Z.SPBH =B.SPBH and  Z.HTQX = B.HTZQ and Z.ZT = '竞标' order by Z.TBJG ASC,ZHHH.CreateTime ASC )  and charindex(Y.SHQY,(select top 1 Z.BGHQY from ZZ_TBXXBZB as Z  join ZZ_TBXXB as ZHHH on Z.parentNumber=ZHHH.Number  where Z.SPBH =B.SPBH and  Z.HTQX = B.HTZQ and Z.ZT = '竞标' order by Z.TBJG ASC,ZHHH.CreateTime ASC ))<1 ),0), '最低价标的达成率'=isnull(convert(varchar(20),ROUND(convert(float,isnull((select sum(Y.YDZSL-Y.YZBL) from ZZ_YDDXXB as Y where Y.SPBH =B.SPBH and  Y.HTZQ = B.HTZQ and Y.ZT = '竞标' and Y.NMRJG >= (select top 1 Z.TBJG from ZZ_TBXXBZB as Z  join ZZ_TBXXB as ZHHH on Z.parentNumber=ZHHH.Number  where Z.SPBH =B.SPBH and  Z.HTQX = B.HTZQ and Z.ZT = '竞标' order by Z.TBJG ASC,ZHHH.CreateTime ASC )  and charindex(Y.SHQY,(select top 1 Z.BGHQY from ZZ_TBXXBZB as Z  join ZZ_TBXXB as ZHHH on Z.parentNumber=ZHHH.Number  where Z.SPBH =B.SPBH and  Z.HTQX = B.HTZQ and Z.ZT = '竞标' order by Z.TBJG ASC,ZHHH.CreateTime ASC ))<1 ),0))/convert(float,(select top 1 Z.TBYSL from ZZ_TBXXBZB as Z  join ZZ_TBXXB as ZHHH on Z.parentNumber=ZHHH.Number  where Z.SPBH =B.SPBH and  Z.HTQX = B.HTZQ and Z.ZT = '竞标' order by Z.TBJG ASC,ZHHH.CreateTime ASC )) * 100,2)),'0.00') + '%',XGCS 修改次数  from ZZ_YDDXXB as B where B.ZT = '竞标' and B.GLDLRDLYX='" + PublicDS.PublisDsUser.Tables[0].Rows[0]["DLYX"].ToString().Trim() + "') as tab";  //检索的表(必须设置)
            ht_where["search_mainid"] = " 预订单号 ";  //所检索表的主键(必须设置)
            string htqx = "";
            if (CBleixing.SelectedItem.ToString().Trim() != "不限周期")
            {
                htqx = CBleixing.SelectedItem.ToString().Trim();
            }

            ht_where["search_str_where"] = " 1=1 and 供货周期 like '%" + htqx + "%'";  //检索条件(必须设置)
            ht_where["search_paixu"] = " DESC ";  //排序方式(必须设置)
            ht_where["search_paixuZD"] = " CreateTime";  //用于排序的字段(必须设置)
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

        private void BBsearch_Click(object sender, EventArgs e)
        {

            //更改搜索条件
            setDefaultSearch();
            DateTime StartTime =dateTimePickerBegin.Value;
            DateTime EndTime =dateTimePickerEnd.Value;
            if (StartTime > EndTime)
            {
                ArrayList AlmsgMR = new ArrayList();
                AlmsgMR.Add("");
                AlmsgMR.Add("开始时间不可大于结束时间，请重新选择！");
                FormAlertMessage FRSEMR = new FormAlertMessage("仅确定", "叹号", "提示", AlmsgMR);
                FRSEMR.ShowDialog();
            }
            //if (StartTime != null && EndTime != null)
            //{
            //    ht_where["search_str_where"] += " and CreateTime between  '" + StartTime.ToString("yyyy-MM-dd") + "' and '" + EndTime.ToString("yyyy-MM-dd") + "' ";
            //}  
            if (StartTime != null && EndTime == null)
            {
                ht_where["search_str_where"] += " and CreateTime >=  '" + StartTime.ToString("yyyy-MM-dd") + "'";
            }
            else if (StartTime == null && EndTime != null)
            {
                ht_where["search_str_where"] += " and CreateTime <=  '" + EndTime.ToString("yyyy-MM-dd") + "'";
            }
            else
            {
                ht_where["search_str_where"] += " and CreateTime between  '" + StartTime.ToString("yyyy-MM-dd") + "' and '" + EndTime.ToString("yyyy-MM-dd") + "' ";
            }

            if (TBkey.Text.Trim() != "" && TBkey.Text.Trim() != "预订单号、商品编号、商品名称")
            {
                ht_where["search_str_where"] += " and (预订单号 like '%" + TBkey.Text.Trim() + "%' or 商品编号 like '%" + TBkey.Text.Trim() + "%' or 商品名称 like '%" + TBkey.Text.Trim() + "%') ";
            }
            if (txtBuyEmailOrName.Text.Trim() != "" && txtBuyEmailOrName.Text.Trim() != "买家登录邮箱、买家用户名")
            {
                ht_where["search_str_where"] += " and (MJDLYX like '%" + txtBuyEmailOrName.Text.Trim() + "%' or MJYHM like '%" + txtBuyEmailOrName.Text.Trim() + "%') ";
            }
            //执行查询
            GetData(ht_where);
        }       
      
    }
}
