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

namespace 客户端主程序.SubForm.NewCenterForm.SUBUC.JJRYWGL
{
    public partial class ucJYFXYDJCX_B : UserControl
    {
        /// <summary>
        /// 查询条件
        /// </summary>
        Hashtable ht_where = new Hashtable();

        public ucJYFXYDJCX_B()
        {
            InitializeComponent();

            //初始化分页回调(带分页的页面，先把这个放上)
            ucPager1.DFT = new delegateForThread(STR_BeginBind);
        }

        private void ucJYFXYDJCX_B_Load(object sender, EventArgs e)
        {
            //处理下拉框间距
            //CBleixing.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.CB_DrawItem);
            ucSSQY.initdefault();
            ucSSQY.VisibleItem = new bool[]{true, true, false};
            
             if (PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["结算账户类型"].ToString() != "经纪人交易账户")
            {
                ArrayList Almsg4 = new ArrayList();
                Almsg4.Add("");
                Almsg4.Add("只有经纪人交易账户可以执行此操作！");// 这里是提交失败的提示，可能是程序出错，可能是条件不足等
                FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "其他", "", Almsg4);
                FRSE4.ShowDialog();
                ucPager1.Enabled = false;
                return; 
             }

            GetData(null);
        }

        /// <summary>
        /// 设置默认搜索条件(这个例子中的哈希表键值一个也不能少)
        /// </summary>
        private void setDefaultSearch()
        {
            //string dlyx=PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString();
            //ht_where["GetCustomersDataPage_NAME"] = "GetCustomersDataPage";  //每页显示的条数(必须设置)
            //ht_where["page_index"] = "0";
            //ht_where["page_size"] = "25";  //每页显示的条数(必须设置)
            //ht_where["serach_Row_str"] = " DLYX as 交易方账号,I_JYFMC as 交易方名称,I_SSQYS+I_SSQYSHI as 所在区域,'无' as 当前信用等级,isnull(B_ZHDQXYFZ,0.00) as 当前信用积分 "; //检索字段(必须设置)
            //ht_where["search_tbname"] = " AAA_MJMJJYZHYJJRZHGLB as a left join AAA_DLZHXXB as b on a.DLYX=b.B_DLYX ";  //检索的表(必须设置)
            //ht_where["search_mainid"] = " DLYX ";  //所检索表的主键(必须设置)
            //ht_where["search_str_where"] = " 1=1 and SFDQMRJJR='是' and GLJJRDLZH='" + dlyx + "' and DLYX in (select distinct DLYX from AAA_JYFXYMXB) and DLYX<>'" + dlyx + "' "; //检索条件(必须设置)
            //ht_where["search_paixu"] = " DESC ";  //排序方式(必须设置)
            //ht_where["search_paixuZD"] = " isnull(B_ZHDQXYFZ,0) ";  //用于排序的字段(必须设置)

            ht_where["page_index"] = "0";
            ht_where["page_size"] = "25";  //每页显示的条数(必须设置)
            ht_where["webmethod"] = "经纪人业务管理B区";
            Hashtable ht_tiaojian = new Hashtable();
            ht_tiaojian["标签名"] = "交易方信用等级查询";
            ht_tiaojian["用户邮箱"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString();
            ht_tiaojian["交易方名称"] = uctbJYFMC.Text.Trim();
            ht_tiaojian["交易方账号"] = uctbJYFZH.Text.Trim();
            ht_tiaojian["省份"] = ucSSQY.SelectedItem[0].ToString();
            ht_tiaojian["地市"] = ucSSQY.SelectedItem[1].ToString();            
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

        private void BBsearch_Click(object sender, EventArgs e)
        {
            //更改搜索条件
            setDefaultSearch();
           
            ////检索条件(必须设置)           
            //if (uctbJYFMC.Text.Trim() != "")
            //{
            //    ht_where["search_str_where"] =ht_where["search_str_where"]+ " and b.I_JYFMC like '%" + uctbJYFMC.Text.Trim() + "%' ";
            //}
            //if (uctbJYFZH.Text.Trim() != "")
            //{
            //    ht_where["search_str_where"] = ht_where["search_str_where"] + " and a.DLYX like '%" + uctbJYFZH.Text.Trim() + "%' ";
            //}
            //string strSSSF = "";//省份
            //string strSSDS = "";//地市
            //string strSSQX = "";//区县
            //if (ucSSQY.SelectedItem[0].ToString().Contains("请选择"))
            //{
            //    strSSSF = "";
            //}
            //else
            //{
            //    strSSSF = ucSSQY.SelectedItem[0].ToString();
            //}

            //if (ucSSQY.SelectedItem[1].ToString().Contains("请选择"))
            //{
            //    strSSDS = "";
            //}
            //else
            //{
            //    strSSDS = ucSSQY.SelectedItem[1].ToString();
            //}

            //if (ucSSQY.SelectedItem[2].ToString().Contains("请选择"))
            //{
            //    strSSQX = "";
            //}
            //else
            //{
            //    strSSQX = ucSSQY.SelectedItem[2].ToString();
            //}

            //if (strSSSF.Trim() != "")
            //{
            //    ht_where["search_str_where"] =ht_where["search_str_where"]+ " and b.I_SSQYS like '%" + strSSSF.Trim() + "%' ";
            //}
            //if (strSSDS.Trim() != "")
            //{
            //    ht_where["search_str_where"] =ht_where["search_str_where"]+ " and b.I_SSQYSHI like '%" + strSSDS.Trim() + "%' ";
            //}
            
            //执行查询
            GetData(ht_where);
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

            //根据等级分数，确定等级应显示的图片并显示
            for (int p = 0; p < dataGridView1.RowCount; p++)
            {  
                //特殊处理信用等级图片，默认是“无”
                object str_DJ =dataGridView1.Rows[p].Cells["当前信用积分"].Value;
                if (str_DJ != null)
                {
                    if (StringOP.IsNumberXS(str_DJ.ToString().Trim().Replace("-", ""), int.MaxValue, 2))
                    {
                        Image[] imageCollectio = JYFXYMX.GetXYImages(Convert.ToDouble(str_DJ.ToString()));
                        if (imageCollectio != null)
                        {
                            Image newTu = JYFXYMX.MergerImg(imageCollectio);
                            dataGridView1.Rows[p].Cells["当前信用等级"] = new DataGridViewImageCell();
                            dataGridView1.Rows[p].Cells["当前信用等级"].Value = newTu;
                        }
                        else
                        {
                            dataGridView1.Rows[p].Cells["当前信用等级"] = new DataGridViewTextBoxCell();
                            dataGridView1.Rows[p].Cells["当前信用等级"].Value = "无";
                        }
                    }
                }
                else
                {
                    dataGridView1.Rows[p].Cells["当前信用等级"] = new DataGridViewTextBoxCell();
                    dataGridView1.Rows[p].Cells["当前信用等级"].Value = "无";
                }

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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            string columnName = dataGridView1.Columns[e.ColumnIndex].HeaderText;         
            if (columnName == "操作" && e.RowIndex >=0)
            {
                DataGridViewRow dgr = dgv.Rows[e.RowIndex];

                string DLYX = dgr.Cells["交易方账号"].Value.ToString();
                string JYFMC = dgr.Cells["交易方名称"].Value.ToString().Trim();
                string DQJF = dgr.Cells["当前信用积分"].Value.ToString().Trim();
                fmJYFXYDJCX tbdcg = new fmJYFXYDJCX(DLYX,JYFMC,DQJF);
                tbdcg.ShowDialog();
            }
        }

        //处理等级图片的
        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }
    }
}
