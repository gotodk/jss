using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using 客户端主程序.DataControl;

namespace 客户端主程序.SubForm.NewCenterForm.SUBUC.JJRYWGL
{
    public partial class ucSHMMJZL_B : UserControl
    {
         
     



            /// <summary>
        /// 查询条件
        /// </summary>
        Hashtable ht_where = new Hashtable();
        public ucSHMMJZL_B()
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

            //ht_where["GetCustomersDataPage_NAME"] = "GetCustomersDataPage";  //每页显示的条数(必须设置)
            //ht_where["page_index"] = "0";
            //ht_where["page_size"] = "25";  //每页显示的条数(必须设置)
            //ht_where["serach_Row_str"] = " * "; //检索字段(必须设置)
            //ht_where["search_tbname"] = "  (select top 100 a.Number as '关联表Number',b.B_DLYX '交易方账号',b.I_JYFMC '交易方名称',c.J_JJRZGZSBH '经纪人资格证书编号',b.B_DLYX as '登录邮箱',b.J_SELJSBH as '卖家角色编号',a.GLJJRBH '关联经纪人编号',c.I_JYFMC '经纪人名称',convert(varchar(20),b.I_ZLTJSJ,120) '资料提交时间',a.CreateTime,b.I_ZCLB '注册类型',b.I_SSQYS+b.I_SSQYSHI+b.I_SSQYQ '所属区域',c.I_PTGLJG '平台管理机构',b.I_LXRXM '联系人姓名',b.I_LXRSJH '联系电话',a.JJRSHSJ '初审时间',a.JJRSHZT '初审记录',a.FGSSHZT '复审记录',(case a.JJRSHZT when '审核中' then '--' when '驳回' then a.SFXG else '' end) '驳回后是否修改'  from AAA_MJMJJYZHYJJRZHGLB as a inner join  AAA_DLZHXXB as b on a.DLYX=b.B_DLYX left join AAA_DLZHXXB as c on a.GLJJRBH=c.J_JJRJSBH   where   a.JSZHLX='买家卖家交易账户' and a.JJRSHZT in('审核中','驳回') and b.I_JYFMC like '%" + this.txtSPMC.Text.ToString().Trim() + "%'  and a.GLJJRBH='" + PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["经纪人角色编号"].ToString() + "' and b.I_SSQYS like '%" + strSSSF + "%' and b.I_SSQYSHI like'%" + strSSDS + "%' and b.I_SSQYQ like'%" + strSSQX + "%' and a.SFYX='是'  order by a.CreateTime asc ) as tab  ";  //检索的表(必须设置)

            //ht_where["search_mainid"] = " 关联表Number ";  //所检索表的主键(必须设置)
            //ht_where["search_str_where"] = " 1=1  ";  //检索条件(必须设置)
            //ht_where["search_paixu"] = " ASC ";  //排序方式(必须设置)
            //ht_where["search_paixuZD"] = " CreateTime ";  //用于排序的字段(必须设置)

            ht_where["page_index"] = "0";
            ht_where["page_size"] = "25";  //每页显示的条数(必须设置)
            ht_where["webmethod"] = "经纪人业务管理B区";
            Hashtable ht_tiaojian = new Hashtable();
            ht_tiaojian["标签名"] = "审核交易方资料";
            ht_tiaojian["经纪人编号"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["经纪人角色编号"].ToString();
            ht_tiaojian["交易方名称"] = txtSPMC.Text.Trim();
            ht_tiaojian["省份"] = ucSSQY.SelectedItem[0].ToString();
            ht_tiaojian["地市"] = ucSSQY.SelectedItem[1].ToString();
            ht_tiaojian["区县"] = ucSSQY.SelectedItem[2].ToString();
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
        private void ucSHMMJZL_C_Load(object sender, EventArgs e)
        {
            ucSSQY.initdefault();
            GetData(null);
        }

        /// <summary>
        /// 用于重置，重新加载审核页面
        /// </summary>
        /// <param name="FK"></param>
        /// <param name="chongzhi"></param>
        /// <param name="hts"></param>
        public void SPopenDialog(FormMMJDetails FK, bool chongzhi, Hashtable hts)
        {
            if (chongzhi)
            {
                FK = new FormMMJDetails(hts);
                FK.ShowDialog();
                SPopenDialog(FK, FK.chongzhi, hts);
            }
        }



        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;

            if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
                //设置默认收货地址的功能
                if (dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null && dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "进行审核")
                {
                    Hashtable hashTable=new Hashtable();
                    hashTable["JSBH"]=dgv.Rows[e.RowIndex].Cells[11].Value;
                    hashTable["DLYX"]=dgv.Rows[e.RowIndex].Cells[10].Value;
                    hashTable["JYFMC"]=dgv.Rows[e.RowIndex].Cells[1].Value;
                   hashTable["GLJJRBH"]=dgv.Rows[e.RowIndex].Cells[12].Value;
                   hashTable["关联表Number"] = dgv.Rows[e.RowIndex].Cells[13].Value;
                   hashTable["SHYJ"]="";
                   hashTable["注册类别"] = "";
                   hashTable["当前经纪人名称"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["交易方名称"].ToString();
                        FormMMJDetails fm = new FormMMJDetails(hashTable);
                     //DialogResult dialogResult=fm.ShowDialog();

                        SPopenDialog(fm, true, hashTable);
                        GetData(null);
                     //if (dialogResult == DialogResult.OK)
                     //{
                     //    reset();
                     //}
                    }
                }
            }


        /// <summary>
        /// 重置表单
        /// </summary>
        private void reset()
        {
            ucSHMMJZL_B s = new ucSHMMJZL_B();
            s.Dock = DockStyle.Fill;//铺满
            s.AutoScroll = true;//出现滚动条
            s.BackColor = Color.AliceBlue;

            //清理控件
            Panel P = (Panel)this.Parent;
            P.Controls.Clear();
            P.Controls.Add(s); //加入到某一个panel中
            s.Show();//显示出来
            UPUP();
        }



        /// <summary>
        /// 页面滚动条强制置顶
        /// </summary>
        private void UPUP()
        {
            TextBox tb = new TextBox();
            this.Controls.Add(tb);
            tb.Location = new Point(-5000, -5000);
            tb.TabIndex = 0;
            tb.Focus();
            //this.Controls.Remove(tb);
        }
        }
    }

