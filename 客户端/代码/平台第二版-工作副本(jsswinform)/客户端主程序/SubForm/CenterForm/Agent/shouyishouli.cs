using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using 客户端主程序.DataControl;

namespace 客户端主程序.SubForm.CenterForm.Agent
{
    public partial class shouyishouli : UserControl
    {
        /// <summary>
        /// 查询条件
        /// </summary>
        Hashtable ht_where = new Hashtable();

        public shouyishouli()
        {
            InitializeComponent();

            //初始化分页回调(带分页的页面，先把这个放上)
            ucPager1.DFT = new delegateForThread(ShowThreadResult_BeginBind);
        }

        //加载窗体后处理数据
        private void UserControl1_Load(object sender, EventArgs e)
        {
            CBleixing.SelectedIndex = 0;
            CBddzt.SelectedIndex = 0;
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

            string JJRjsbh = PublicDS.PublisDsUser.Tables[0].Rows[0]["经纪人角色编号"].ToString().Trim();
            ht_where["GetCustomersDataPage_NAME"] = "GetCustomersDataPage";  //每页显示的条数(必须设置)
            ht_where["page_index"] = "0";
            ht_where["page_size"] = "25";  //每页显示的条数(必须设置)
            ht_where["serach_Row_str"] = " * "; //检索字段(必须设置)


            //已生成的收益
            string sql = "select '单号'=Number,'年份'=SYNF,'月份'=SYYF,'收益金额'=BYSYZJE,'买家收益金额'=BUYCSSY,'卖家收益金额'=SELCSSY,'经纪人违规扣减金额'=WGKJJE,'买家违规扣减金额'=YBUYKJJE,'卖家违规扣减金额'=YSELKJJE,'税额'=YJSE from ZZ_DLRSYMXB where DLRJSBH='" + JJRjsbh + "'  union all ";

            //合并已计算过的收益和动态实时计算的收益
            DateTime dtbegin = DateTime.Now;
            if (dtbegin.Day < 5) //5号之前，需要动态计算上月的， 还需要动态计算本月的
            {
                DateTime dtSYtime = DateTime.Now.AddMonths(-1);//收益时间，当前时间上推一个月
                string nian = dtSYtime.Year.ToString();
                string yue = dtSYtime.Month.ToString().PadLeft(2, '0');

                DateTime dtSYtimenow = DateTime.Now;//收益时间，当月
                string nianD = dtSYtimenow.Year.ToString();
                string yueD = dtSYtimenow.Month.ToString().PadLeft(2, '0');

                //上月收益
                sql = sql + "  select '单号'='无','年份'='" + nian + "','月份'='" + yue + "','收益金额'=买家产生收益+卖家产生收益,'买家收益金额'=买家产生收益,'卖家收益金额'=卖家产生收益,'经纪人违规扣减金额'=违规扣减金额,'买家违规扣减金额'=因买家扣减金额,'卖家违规扣减金额'=因卖家扣减金额,'税额'=0.00 from (select '经纪人邮箱'=B.DLYX, '经纪人用户名'=B.YHM, '经纪人角色编号'=B.JSBH, '经纪人注册类型'=B.ZCLX, '收益年份'='" + nian + "','收益月份'='" + yue + "', '买家产生收益'=isnull((select ROUND(convert(float,sum(T.THZJE))*convert(float,0.01)*convert(float,0.35),2) from ZZ_THDXXB as T where T.GLDLRJSBH=B.JSBH and T.THDZT='已签收' and  convert(varchar(7), T.BUYQSSJ , 120) = '" + nian + "-" + yue + "' ),0.00), '卖家产生收益'=isnull((select ROUND(convert(float,sum(T.THZJE))*convert(float,0.01)*convert(float,0.27),2) from ZZ_THDXXB as T where T.SELDLRJSBH=B.JSBH and T.THDZT='已签收' and  convert(varchar(7), T.BUYQSSJ , 120) = '" + nian + "-" + yue + "'),0.00), '违规扣减金额'=0.00, '因买家扣减金额'=0.00, '因卖家扣减金额'=0.00, '受理状态'='待受理' from ZZ_DLRJBZLXXB as B where B.JSBH='" + JJRjsbh + "') as tab22  union all  ";

                //本月收益
                sql = sql + "  select '单号'='无','年份'='" + nianD + "','月份'='" + yueD + "','收益金额'=买家产生收益+卖家产生收益,'买家收益金额'=买家产生收益,'卖家收益金额'=卖家产生收益,'经纪人违规扣减金额'=违规扣减金额,'买家违规扣减金额'=因买家扣减金额,'卖家违规扣减金额'=因卖家扣减金额,'税额'=0.00 from (select '经纪人邮箱'=B.DLYX, '经纪人用户名'=B.YHM, '经纪人角色编号'=B.JSBH, '经纪人注册类型'=B.ZCLX, '收益年份'='" + nianD + "','收益月份'='" + yueD + "', '买家产生收益'=isnull((select ROUND(convert(float,sum(T.THZJE))*convert(float,0.01)*convert(float,0.35),2) from ZZ_THDXXB as T where T.GLDLRJSBH=B.JSBH and T.THDZT='已签收' and  convert(varchar(7), T.BUYQSSJ , 120) = '" + nianD + "-" + yueD + "' ),0.00), '卖家产生收益'=isnull((select ROUND(convert(float,sum(T.THZJE))*convert(float,0.01)*convert(float,0.27),2) from ZZ_THDXXB as T where T.SELDLRJSBH=B.JSBH and T.THDZT='已签收' and  convert(varchar(7), T.BUYQSSJ , 120) = '" + nianD + "-" + yueD + "'),0.00), '违规扣减金额'=0.00, '因买家扣减金额'=0.00, '因卖家扣减金额'=0.00, '受理状态'='待受理' from ZZ_DLRJBZLXXB as B where B.JSBH='" + JJRjsbh + "') as tab33 ";

            }
            else
            {
                //只动态计算合并当月的

                string nianD = dtbegin.Year.ToString();
                string yueD = dtbegin.Month.ToString().PadLeft(2, '0');

                //本月收益
                sql = sql + "  select '单号'='无','年份'='" + nianD + "','月份'='" + yueD + "','收益金额'=买家产生收益+卖家产生收益,'买家收益金额'=买家产生收益,'卖家收益金额'=卖家产生收益,'经纪人违规扣减金额'=违规扣减金额,'买家违规扣减金额'=因买家扣减金额,'卖家违规扣减金额'=因卖家扣减金额,'税额'=0.00 from (select '经纪人邮箱'=B.DLYX, '经纪人用户名'=B.YHM, '经纪人角色编号'=B.JSBH, '经纪人注册类型'=B.ZCLX, '收益年份'='" + nianD + "','收益月份'='" + yueD + "', '买家产生收益'=isnull((select ROUND(convert(float,sum(T.THZJE))*convert(float,0.01)*convert(float,0.35),2) from ZZ_THDXXB as T where T.GLDLRJSBH=B.JSBH and T.THDZT='已签收' and  convert(varchar(7), T.BUYQSSJ , 120) = '" + nianD + "-" + yueD + "' ),0.00), '卖家产生收益'=isnull((select ROUND(convert(float,sum(T.THZJE))*convert(float,0.01)*convert(float,0.27),2) from ZZ_THDXXB as T where T.SELDLRJSBH=B.JSBH and T.THDZT='已签收' and  convert(varchar(7), T.BUYQSSJ , 120) = '" + nianD + "-" + yueD + "'),0.00), '违规扣减金额'=0.00, '因买家扣减金额'=0.00, '因卖家扣减金额'=0.00, '受理状态'='待受理' from ZZ_DLRJBZLXXB as B where B.JSBH='" + JJRjsbh + "') as tab33 ";
            }


            ht_where["search_tbname"] = "  ( " + sql + " ) as tab  ";  //检索的表(必须设置)
            ht_where["search_mainid"] = " 单号+年份+月份 ";  //所检索表的主键(必须设置)
            ht_where["search_str_where"] = " 1=1 ";  //检索条件(必须设置)
            ht_where["search_paixu"] = " DESC ";  //排序方式(必须设置)
            ht_where["search_paixuZD"] = " CONVERT(int,年份+月份) ";  //用于排序的字段(必须设置)
            
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








        private void BBsearch_Click(object sender, EventArgs e)
        {
            //获取搜索条件
            string htzq = CBleixing.Text;
            string zbzt = CBddzt.Text;
            //更改搜索条件
            setDefaultSearch();
            if (htzq != "不限年份")
            {
                ht_where["search_str_where"] = ht_where["search_str_where"] + " and 年份='" + htzq.Replace("年", "") + "' ";
            }
            if (zbzt != "不限月份")
            {
                ht_where["search_str_where"] = ht_where["search_str_where"] + " and 月份='" + zbzt.Replace("月", "") + "' ";
            }
            //执行查询
            GetData(ht_where);
        }
    }
}
