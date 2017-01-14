using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using 客户端主程序.NewDataControl;
using 客户端主程序.DataControl;

namespace 客户端主程序.SubForm.NewCenterForm.SUBUC.TZJL
{
    public partial class ucXYJL : UserControl
    {
        public static string XYJF = "";
        Hashtable ht_where = new Hashtable();
        public ucXYJL()
        {
            InitializeComponent();
            lbdqxyjf.Text =XYJF = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["账户当前信用分值"].ToString();
            //初始化分页回调(带分页的页面，先把这个放上)
            ucPager1.DFT = new delegateForThread(STR_BeginBind);
        }      

        /// <summary>
        /// 设置默认搜索条件(这个例子中的哈希表键值一个也不能少)
        /// </summary>
        private void setDefaultSearch()
        {
            //string dlyx = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString();
            //string str = "(SELECT a.[Number]as num ,[JFSX] as ys,convert(numeric(18,2), FS) as fs ,  (select  sum(convert(numeric(18,2), FS))  FROM [AAA_JYFXYMXB] as b where [DLYX]='" + dlyx + "' and b.Number <=a.Number )  as xyjf,a.[CreateTime]as cjsj  FROM [AAA_JYFXYMXB] as a    where [DLYX]='" + dlyx + "'  )  tab1";
            
            //ht_where["GetCustomersDataPage_NAME"] = "GetCustomersDataPage";  //每页显示的条数(必须设置)
            //ht_where["page_index"] = "0";
            //ht_where["page_size"] = "25";  //每页显示的条数(必须设置)
            //ht_where["serach_Row_str"] = " num, ys,fs,xyjf, cjsj "; //检索字段(必须设置)
            //ht_where["search_tbname"] = str;  //检索的表(必须设置)           
            //ht_where["search_mainid"] = " num ";  //所检索表的主键(必须设置)
            //ht_where["search_str_where"] = " 1=1 ";  //检索条件(必须设置)
            //ht_where["search_paixu"] = " DESC ";  //排序方式(必须设置)
            //ht_where["search_paixuZD"] = " num ";  //用于排序的字段(必须设置)

            string dlyx = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString();
            ht_where["page_index"] = "0";
            ht_where["page_size"] = "25";  //每页显示的条数(必须设置)
            ht_where["webmethod"] = "通知记录B区";
            Hashtable ht_tiaojian = new Hashtable();
            ht_tiaojian["标签名"] = "信用评级";
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
            dataGridViewfs.AutoGenerateColumns = false;

            //若执行正常
            if (ds.Tables.Contains("主要数据"))
            {
                dataGridViewfs.DataSource = ds.Tables[0].DefaultView;

                if (ds.Tables[0].Rows[0]["xyjf"] != null && ds.Tables[0].Rows[0]["xyjf"].ToString().Trim() != "" && dtTImeEnd.Value.ToString("yyyy-MM-dd") == DateTime.Now.ToString("yyyy-MM-dd"))
                { lbdqxyjf.Text = XYJF = ds.Tables[0].Rows[0]["xyjf"].ToString(); }

            }
            else
            {
                dataGridViewfs.DataSource = null;
       
            }
            //取消列表的自动排序
            for (int i = 0; i < this.dataGridViewfs.Columns.Count; i++)
            {
                if (i == 0)
                {
                    //让第一列留出一点空白
                    DataGridViewCellStyle dgvcs = new DataGridViewCellStyle();
                    dgvcs.Padding = new Padding(10, 0, 0, 0);
                    this.dataGridViewfs.Columns[i].HeaderCell.Style = dgvcs;
                    this.dataGridViewfs.Columns[i].DefaultCellStyle = dgvcs;
                }
                this.dataGridViewfs.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
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
                ht_tiaojian["开始时间"] = begintime;
                ht_tiaojian["结束时间"] = endtime;
                //ht_where["search_str_where"] = ht_where["search_str_where"] + " and convert(varchar(10), cjsj ,120)>='" + begintime + "' and convert(varchar(10), cjsj ,120)<='" + endtime + "' ";
            }
           
            //ht_where["search_str_where"] = ht_where["search_str_where"];  //检索条件(必须设置)
            ht_where["条件"] = ht_tiaojian;
            //执行查询
            GetData(ht_where);
        }      
      

        private void ucXYJL_Load(object sender, EventArgs e)
        { 
            GetData(null);

        }

    }


}
