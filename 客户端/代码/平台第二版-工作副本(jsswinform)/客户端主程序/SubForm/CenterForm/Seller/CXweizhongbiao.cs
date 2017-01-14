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

namespace 客户端主程序.SubForm.CenterForm.Seller
{
    public partial class CXweizhongbiao : UserControl
    {
        /// <summary>
        /// 查询条件
        /// </summary>
        Hashtable ht_where = new Hashtable();

        public CXweizhongbiao()
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
            e.Graphics.DrawString(CBthis.Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds.X + 2, e.Bounds.Y + 2);
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
            string JSNM = PublicDS.PublisDsUser.Tables[0].Rows[0]["卖家角色编号"].ToString().Trim();

            ht_where["GetCustomersDataPage_NAME"] = "GetCustomersDataPage";  //每页显示的条数(必须设置)
            ht_where["page_index"] = "0";
            ht_where["page_size"] = "25";  //每页显示的条数(必须设置)
            ht_where["serach_Row_str"] = " * "; //检索字段(必须设置)
            ht_where["search_tbname"] = "  ( select  '最大经济批量'=(select top 1 P.ZDJJPL from ZZ_PTSPXXB as P where P.SPBH=B.SPBH), '子表编号'=B.ID, '主表编号'=Z.Number,'投标时间'=Z.CreateTime,  '标的号'=convert(varchar(20),Z.Number)+'_'+convert(varchar(20),B.ID),'商品编号'=B.SPBH,'商品名称'=B.SPMC,'型号规格'=B.GGXH, '供货周期'=B.HTQX, '计价单位'=B.JJDW,'竞标状态'=B.ZT,'经济批量'=B.JJPL, '投标拟售量'=B.TBYSL, '投标价格'=B.TBJG,  '是否在冷静期内'=B.SFJRLJQ,'进入冷静期时间'=B.JRLJQSJ, '最低价标的经济批量'=isnull((select top 1 Z.JJPL from ZZ_TBXXBZB as Z  join ZZ_TBXXB as ZHHH on Z.parentNumber=ZHHH.Number  where Z.SPBH =B.SPBH and  Z.HTQX = B.HTQX and Z.ZT = '竞标' order by Z.TBJG ASC,ZHHH.CreateTime ASC),0),  '最低价标的投标价格'=isnull((select top 1 Z.TBJG from ZZ_TBXXBZB as Z join ZZ_TBXXB as ZHHH on Z.parentNumber=ZHHH.Number  where Z.SPBH =B.SPBH and  Z.HTQX = B.HTQX and Z.ZT = '竞标' order by Z.TBJG ASC,ZHHH.CreateTime ASC),0.00),  '最低价标的投标拟售量'=isnull((select top 1 Z.TBYSL from ZZ_TBXXBZB as Z join ZZ_TBXXB as ZHHH on Z.parentNumber=ZHHH.Number  where Z.SPBH =B.SPBH and  Z.HTQX = B.HTQX and Z.ZT = '竞标' order by Z.TBJG ASC,ZHHH.CreateTime ASC),0),  '参与竞标集合预订量'=isnull((select sum(Y.YDZSL-Y.YZBL) from ZZ_YDDXXB as Y where Y.SPBH =B.SPBH and  Y.HTZQ = B.HTQX and Y.ZT = '竞标' and Y.NMRJG >= isnull((select top 1 Z.TBJG from ZZ_TBXXBZB as Z join ZZ_TBXXB as ZHHH on Z.parentNumber=ZHHH.Number  where Z.SPBH =B.SPBH and  Z.HTQX = B.HTQX and Z.ZT = '竞标' order by Z.TBJG ASC,ZHHH.CreateTime ASC),0)  and charindex(Y.SHQY,(select top 1 Z.BGHQY from ZZ_TBXXBZB as Z join ZZ_TBXXB as ZHHH on Z.parentNumber=ZHHH.Number  where Z.SPBH =B.SPBH and  Z.HTQX = B.HTQX and Z.ZT = '竞标' order by Z.TBJG ASC,ZHHH.CreateTime ASC))<1 ),0),'最低价标的达成率'=isnull(convert(varchar(20),ROUND(convert(float,isnull((select sum(Y.YDZSL-Y.YZBL) from ZZ_YDDXXB as Y where Y.SPBH =B.SPBH and  Y.HTZQ = B.HTQX and Y.ZT = '竞标' and Y.NMRJG >= (select top 1 Z.TBJG from ZZ_TBXXBZB as Z join ZZ_TBXXB as ZHHH on Z.parentNumber=ZHHH.Number  where Z.SPBH =B.SPBH and  Z.HTQX = B.HTQX and Z.ZT = '竞标' order by Z.TBJG ASC,ZHHH.CreateTime ASC)  and charindex(Y.SHQY,(select top 1 Z.BGHQY from ZZ_TBXXBZB as Z join ZZ_TBXXB as ZHHH on Z.parentNumber=ZHHH.Number  where Z.SPBH =B.SPBH and  Z.HTQX = B.HTQX and Z.ZT = '竞标' order by Z.TBJG ASC,ZHHH.CreateTime ASC))<1 ),0))/convert(float,(select top 1 Z.TBYSL from ZZ_TBXXBZB as Z join ZZ_TBXXB as ZHHH on Z.parentNumber=ZHHH.Number  where Z.SPBH =B.SPBH and  Z.HTQX = B.HTQX and Z.ZT = '竞标' order by Z.TBJG ASC,ZHHH.CreateTime ASC)) * 100,2)),'0.00') + '%'   from ZZ_TBXXB as Z join ZZ_TBXXBZB as B on Z.Number = B.parentNumber where B.ZT = '竞标' and  Z.SELJSBH='" + JSNM + "' ) as tab ";//检索的表(必须设置)

            ht_where["search_mainid"] = " 子表编号 + 标的号 ";  //所检索表的主键(必须设置)
            ht_where["search_str_where"] = " 1=1 ";  //检索条件(必须设置)
            ht_where["search_paixu"] = " DESC ";  //排序方式(必须设置)
            ht_where["search_paixuZD"] = " 投标时间 ";  //用于排序的字段(必须设置)
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
            //获取搜索条件
            string key = TBkey.Text.Trim();
            string htzq = CBleixing.Text;
            string begintime = dateTimePickerBegin.Value.ToShortDateString() + " 00:00:01";
            string endtime = dateTimePickerEnd.Value.ToShortDateString() + " 23:59:59";
            //更改搜索条件
            setDefaultSearch();
            ht_where["search_str_where"] = ht_where["search_str_where"] + " and 投标时间 > '" + begintime + "' and 投标时间 < '" + endtime + "' ";  //检索条件(必须设置)
            if (htzq != "不限周期")
            {
                ht_where["search_str_where"] = ht_where["search_str_where"] + " and 供货周期='" + htzq + "' ";
            }
            if (key != "" && key != "标的号、商品编号、商品名称")
            {
                ht_where["search_str_where"] = ht_where["search_str_where"] + " and (标的号 like '%" + key + "%' or 商品编号 like '%" + key + "%' or 商品名称 like '%" + key + "%') ";
            }
           
            //执行查询
            GetData(ht_where);
        }

        public string Enumber = "";//主表编号
        public string Eid = "";//子表编号
        public string Esl = ""; //投标拟售量
        public string Ejg = ""; //投标价格
        public string Espbh = ""; //商品编号
        public string Espmc = ""; //商品名称
        public string Ezdjjpl = ""; //最大经济批量
        public string Ehtzq = "";//合同周期
        public string JJPL = "";

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex > -1)
            {
                //修改
                //获取选中值
                int rowindex = e.RowIndex;

                //显示修改区域
                string bsh = dataGridView1.Rows[rowindex].Cells["Number"].Value.ToString(); //主表编号
                string zibiaobianhao = dataGridView1.Rows[rowindex].Cells["zibiaobianhao"].Value.ToString();//子表编号

                 Enumber = bsh;
                 Eid = zibiaobianhao;
                 Esl = dataGridView1.Rows[rowindex].Cells["zbydl"].Value.ToString(); //投标拟售量
                 Ejg = dataGridView1.Rows[rowindex].Cells["tbpm"].Value.ToString(); //投标价格
                 Espbh = dataGridView1.Rows[rowindex].Cells["Column1"].Value.ToString(); //商品编号
                 Espmc = dataGridView1.Rows[rowindex].Cells["shangpin"].Value.ToString(); //商品名称
                 Ezdjjpl = dataGridView1.Rows[rowindex].Cells["Column9"].Value.ToString(); //最大经济批量
                 Ehtzq = dataGridView1.Rows[rowindex].Cells["leixing"].Value.ToString(); //合同周期
                 JJPL = dataGridView1.Rows[rowindex].Cells["Column8"].Value.ToString();//经济批量
                CXweizhongbiaoEdit CXE = new CXweizhongbiaoEdit(this);
                CXE.ShowDialog();

            }
            if (e.ColumnIndex == 1 && e.RowIndex > -1)
            {
                //撤销
                //获取选中值
                int rowindex = e.RowIndex;
                string bsh = dataGridView1.Rows[rowindex].Cells["Number"].Value.ToString(); //主表编号
                string zibiaobianhao = dataGridView1.Rows[rowindex].Cells["zibiaobianhao"].Value.ToString();//子表编号

                ArrayList Almsg3 = new ArrayList();
                Almsg3.Add("");
                Almsg3.Add("您确定要撤销此投标信息吗？");
                FormAlertMessage FRSE3 = new FormAlertMessage("确定取消", "问号", "xxx", Almsg3);
                DialogResult dr = FRSE3.ShowDialog();
                if (dr == DialogResult.Yes)
                {
                    //开启线程提交数据
                    string Number = bsh;
                    string id = zibiaobianhao;
                    string sp = "撤销";
                    string sl = "";
                    string jg = "";


                    PBload.Visible = true;
                    dataGridView1.Enabled = false;
     

                    delegateForThread dft = new delegateForThread(ShowThreadResult_save);
                    DataControl.RunThreadClassDEltbxx RTC = new DataControl.RunThreadClassDEltbxx(Number, id, sp, sl, jg, PublicDS.PublisDsUser.Tables[0].Rows[0]["卖家角色编号"].ToString().Trim(), dft);
                    Thread trd = new Thread(new ThreadStart(RTC.BeginRun));
                    trd.IsBackground = true;
                    trd.Start();
                }
            }
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

            PBload.Visible = false;
            dataGridView1.Enabled = true;

            //显示执行结果
            string returnds = returnHT["执行结果"].ToString();
            switch (returnds)
            {
                case "ok":

                    //给出表单提交成功的提示
                    ArrayList Almsg3 = new ArrayList();
                    Almsg3.Add("");
                    Almsg3.Add("撤销成功！");
                    FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "叹号", "处理成功", Almsg3);
                    FRSE3.ShowDialog();

                    //重新绑定数据
                    GetData(null);

                    break;
                case "系统繁忙":
                    ArrayList Almsg7 = new ArrayList();
                    Almsg7.Add("");
                    Almsg7.Add("抱歉，系统繁忙，请稍后再试……");
                    FormAlertMessage FRSE7 = new FormAlertMessage("仅确定", "错误", "系统忙", Almsg7);
                    FRSE7.ShowDialog();
                    break;
                default:
                    ArrayList Almsg4 = new ArrayList();
                    Almsg4.Add("");
                    Almsg4.Add(returnds);// 这里是提交失败的提示，可能是程序出错，可能是条件不足等
                    FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "错误", "提交xxx失败", Almsg4);
                    FRSE4.ShowDialog();

                    break;
            }

        }

  




    }
}
