using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace 客户端主程序.SubForm.NewCenterForm.SUBUC
{
    public partial class DEMO1 : UserControl
    {
        /// <summary>
        /// 查询条件
        /// </summary>
        Hashtable ht_where = new Hashtable();

        public DEMO1()
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
            ht_where["GetCustomersDataPage_NAME"] = "GetCustomersDataPage";  //每页显示的条数(必须设置)
            ht_where["page_index"] = "0";
            ht_where["page_size"] = "25";  //每页显示的条数(必须设置)
            ht_where["serach_Row_str"] = " id,module,OperateTime,IP as yyyy,'测试'= '测试2','你好' as 你好啊,'测试一个长列名'='测试一个很长的内容测试一个很长的内容测试一个很长的内容' "; //检索字段(必须设置)
            ht_where["search_tbname"] = "  FMEventLog  ";  //检索的表(必须设置)
            ht_where["search_mainid"] = " id ";  //所检索表的主键(必须设置)
            ht_where["search_str_where"] = " 1=1 ";  //检索条件(必须设置)
            ht_where["search_paixu"] = " DESC ";  //排序方式(必须设置)
            ht_where["search_paixuZD"] = " id ";  //用于排序的字段(必须设置)

            //再处理类和方法，竖杠隔开，不需要时留空或注释掉都行。  
            //在处理是指在后台使用指定的类中的指定方法，对本次得到分页进行二次处理，
            //所以一般情况下，分页语句只需要得到能确定唯一行的数据即可，或者处理界面上查询条件和排序必须的字段即可。 
            //其他复杂运算放入再处理类进行循环处理。
            //ht_where["method_retreatment"] = "demoR|chuli_demo";  
        }

        /// <summary>
        ///新系统框架下，大分页功能的参数设置示例(这个例子中的哈希表键值一个也不能少)
        ///仅需要对此处进行修改，其余的都沿用原来的方式
        /// </summary>
        private void setDefaultSearch_new()
        {
            ht_where["page_index"] = "0";//第几页，默认都是0
            ht_where["page_size"] = "25";  //每页显示的条数(必须设置)
            ht_where["webmethod"] = "大分页调用测试";//要调用的后台方法的“业务方法名”  
            
            //拼接查询条件需要用的数据值
            Hashtable ht_tiaojian = new Hashtable();
            ht_tiaojian["时间"] = dateTimePickerBegin.Text.ToString();
            ht_tiaojian["类型"]=CBleixing.SelectedValue.ToString ();
            ht_tiaojian["主键"]=TBkey.Text.ToString(); 
           
            ht_where["tiaojian"] = ht_tiaojian;//作为查询条件要传递的参数
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



            //根据行内容，处理按钮是否可用
            for (int p = 0; p < dataGridView1.RowCount; p++)
            {
                //根据某列的值，更改操作列的按钮 130307000157
                if (dataGridView1["商品编号", p].Value.ToString() == "xxx")
                {
                    dataGridView1["操作", p] = new DataGridViewTextBoxCell();
                    dataGridView1["操作", p].Value = "无需操作x";//换成想要显示的文字
                    dataGridView1["操作", p].Tag = "不让操作哦"; //这五个字固定的，用于点击时的判断
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


        private void DEMO1_Load(object sender, EventArgs e)
        {
            //处理下拉框间距
            CBleixing.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.CB_DrawItem);

            GetData(null);
        }

        private void BBsearch_Click(object sender, EventArgs e)
        {
            //获取搜索条件
            //string begintime = dateTimePickerBegin.Value.ToShortDateString() + " 00:00:01";
            //string endtime = dateTimePickerBegin.Value.ToShortDateString() + " 23:59:59";
            //string key = TBkey.Text.Trim();

            //更改搜索条件
            setDefaultSearch();
            //ht_where["search_str_where"] = " 1=1 and IP like '%" + key + "%' or 1=1";  //检索条件(必须设置)
            //执行查询
            GetData(ht_where);
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            ////设置要隐藏的列(原分页中，有些列可能是隐藏不显示的，导出时，可以选择滤掉)
            //string[] HideColumns = new string[] { "" };
            ////设置导出语句
            //string sql = "select top 30 * from FMEventLog";
            ////cMyXls1.BeginRunFrom_ht_where(sql, HideColumns);

            Hashtable ht_export = new Hashtable();
            ht_export["filename"] = "DEMO1导出";//导出文件的文件名，导出后的文件名为：年月日+此文件名
            ht_export["webmethod"] = "方法名";//实际执行导出功能的方法名。导出功能统一调用客户端专用中心里的导出，然后由客户端专用中心调用实际导出的方法。此处的方法名是指实际执行导出的方法名。
            ht_export["tiaojian"] = (Hashtable)ht_where["tiaojian"];//分页功能中设置的条件

            cMyXls1.BeginRunFrom_ht_where(ht_export);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //点击列表中某列中的控件时处理
            string columnName = dataGridView1.Columns[e.ColumnIndex].Name;
            if (columnName == "操作" && e.RowIndex > -1)
            {
                //特殊处理，让被禁用的行不响应操作
                if (dataGridView1.Rows[e.RowIndex].Cells["操作"].Tag != null && dataGridView1.Rows[e.RowIndex].Cells["操作"].Tag.ToString() == "不让操作哦")
                {
                    return;
                }
                
                //执行点击后应该执行的事件
            }


            //点击某个单元格内控件，在控件下方显示一个菜单
            if (columnName == "带下拉菜单的操作" && e.RowIndex > -1)
            {
                //设置菜单最精简
                CMCview.ShowCheckMargin = false;
                CMCview.ShowImageMargin = false;
                //将当前行索引给菜单备用
                foreach (ToolStripMenuItem items in CMCview.Items)
                {
                    items.Tag = e.RowIndex;
                }
                //获得cell的位置
                DataGridViewCell cell = this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                Rectangle rect = this.dataGridView1.GetCellDisplayRectangle(cell.ColumnIndex, cell.RowIndex, true);
                //显示菜单
                CMCview.Show(dataGridView1, new Point(rect.Location.X + 5, rect.Location.Y + rect.Height));

            }


        }

        private void 选项1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //具体点击菜单后的操作
            ToolStripMenuItem TSMT = (ToolStripMenuItem)sender;
            int RowIndex = (int)TSMT.Tag;
            MessageBox.Show(dataGridView1.Rows[RowIndex].Cells[1].Value.ToString());
        }
    }
}
