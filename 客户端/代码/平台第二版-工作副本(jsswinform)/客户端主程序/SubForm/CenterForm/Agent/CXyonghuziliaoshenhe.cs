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
    public partial class CXyonghuziliaoshenhe : UserControl
    {
        /// <summary>
        /// 查询条件
        /// </summary>
        Hashtable ht_where = new Hashtable();

        /// <summary>
        /// 用户资料审核详情页面
        /// </summary>
        FormYHZLXQ formYHZLSH = null;

        /// <summary>
        /// 查询条件
        /// </summary>
        string str_where = null;

        public CXyonghuziliaoshenhe()
        {
            InitializeComponent();

            //初始化分页回调(带分页的页面，先把这个放上)
            ucPager1.DFT = new delegateForThread(ShowThreadResult_BeginBind);
            //处理下拉框间距
            this.cbxZCJS.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.CB_DrawItem);
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
            dataGridView1.AutoGenerateColumns = true;
          dataGridView1.DataSource = ds.Tables[0].DefaultView;
            ////若执行正常
            //if (ds.Tables.Contains("主要数据"))
            //{
                
            //}
            //else
            //{
            //    dataGridView1.DataSource = null;
            //}
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
            ht_where["serach_Row_str"] = " ROWID,结算账户,用户名,登录邮箱,联系人姓名,手机号,注册时间,经纪人审核状态,CAOZUO,角色编号"; //检索字段(必须设置)
            //ht_where["search_tbname"] = " (select '' as 'ROWID',MJ.JSBH,JS.JSLX,MJ.YHM,MJ.DLYX,MJ.LXRXM,MJ.SJH,LG.ZCSJ,JS.KTSHZT,MJJJR.SQGLSJ,'审核' as 'CAOZUO' from ZZ_SELJBZLXXB as MJ left join ZZ_UserLogin as LG on MJ.DLYX=LG.DLYX and LG.YHM=MJ.YHM left join ZZ_YHJSXXB as JS  on MJ.JSBH=JS.JSBH left join ZZ_MMJYDLRZHGLB as MJJJR on MJJJR.JSLX=JS.JSLX and MJJJR.JSBH=JS.JSBH where MJJJR.DLRBH='" + PublicDS.PublisDsUser.Tables[0].Rows[0]["经纪人角色编号"].ToString() + "' and MJJJR.GLZT='有效' and  JS.KTSHZT='待审核' union all select '' as 'ROWID',MJ.JSBH,JS.JSLX,MJ.YHM,MJ.DLYX,MJ.LXRXM,MJ.SJH,LG.ZCSJ,JS.KTSHZT,MJJJR.SQGLSJ, '审核' as 'CAOZUO' from ZZ_BUYJBZLXXB as MJ left join ZZ_UserLogin as LG on MJ.DLYX=LG.DLYX and LG.YHM=MJ.YHM left join ZZ_YHJSXXB as JS  on MJ.JSBH=JS.JSBH left join ZZ_MMJYDLRZHGLB as MJJJR on MJJJR.JSLX=JS.JSLX and MJJJR.JSBH=JS.JSBH where MJJJR.DLRBH='" + PublicDS.PublisDsUser.Tables[0].Rows[0]["经纪人角色编号"].ToString() + "' and MJJJR.GLZT='有效' and  JS.KTSHZT='待审核' ) as tab  ";  //检索的表(必须设置)
            //ht_where["search_tbname"] = " ( select '' as 'ROWID',MJ.JSBH,JS.JSLX,MJ.YHM,MJ.DLYX,MJ.LXRXM,MJ.SJH,LG.ZCSJ,MJJJR.SQGLSJ,'审核' as 'CAOZUO',MJJJR.DLRSHZT 经纪人审核状态 from ZZ_SELJBZLXXB as MJ left join ZZ_UserLogin as LG on MJ.DLYX=LG.DLYX and LG.YHM=MJ.YHM left join ZZ_YHJSXXB as JS  on MJ.JSBH=JS.JSBH left join ZZ_MMJYDLRZHGLB as MJJJR on MJJJR.JSLX=JS.JSLX and MJJJR.JSBH=JS.JSBH where MJJJR.DLRBH='" + PublicDS.PublisDsUser.Tables[0].Rows[0]["经纪人角色编号"].ToString() + "' and MJJJR.GLZT='有效' and  MJJJR.DLRSHZT='待审核' union all  select '' as 'ROWID',MJ.JSBH,JS.JSLX,MJ.YHM,MJ.DLYX,MJ.LXRXM,MJ.SJH,LG.ZCSJ,MJJJR.SQGLSJ, '审核' as 'CAOZUO',MJJJR.DLRSHZT 经纪人审核状态 from ZZ_BUYJBZLXXB as MJ left join ZZ_UserLogin as LG on MJ.DLYX=LG.DLYX and LG.YHM=MJ.YHM left join ZZ_YHJSXXB as JS  on MJ.JSBH=JS.JSBH left join ZZ_MMJYDLRZHGLB as MJJJR on MJJJR.JSLX=JS.JSLX and MJJJR.JSBH=JS.JSBH where MJJJR.DLRBH='" + PublicDS.PublisDsUser.Tables[0].Rows[0]["经纪人角色编号"].ToString() + "' and MJJJR.GLZT='有效' and  MJJJR.DLRSHZT='待审核' ) as tab  ";  //检索的表(必须设置)
            ht_where["search_tbname"] = " (  select '' as 'ROWID',结算账户,SEL.YHM 用户名,登录邮箱,SEL.LXRXM 联系人姓名,SEL.SJH 手机号,申请关联时间,SEL.CreateTime 注册时间,经纪人审核状态,分公司审核状态,L.JSZHLX 结算账户类型,关联经纪人账号,SEL.JSBH 角色编号,'审核' as 'CAOZUO' from (select distinct JSDLZH 登录邮箱,JSZHLX 结算账户,DLRDLZH 关联经纪人账号,DLRSHZT 经纪人审核状态,FGSSHZT 分公司审核状态,SQGLSJ 申请关联时间 from ZZ_MMJYDLRZHGLB where DLRSHZT ='待审核' and JSZHLX='卖家账户' and JSLX='卖家') as MMJ left join ZZ_SELJBZLXXB as SEL on MMJ.登录邮箱=SEL.DLYX left join ZZ_UserLogin as L on MMJ.登录邮箱=L.DLYX union select '' as 'ROWID',结算账户,BUY.YHM 用户名,登录邮箱,BUY.LXRXM 联系人姓名,BUY.SJH 手机号,申请关联时间,BUY.CreateTime 注册时间,经纪人审核状态,分公司审核状态,L.JSZHLX 结算账户类型,关联经纪人账号,BUY.JSBH 角色编号,'审核' as 'CAOZUO' from (select distinct JSDLZH 登录邮箱,JSZHLX 结算账户,DLRDLZH 关联经纪人账号,DLRSHZT 经纪人审核状态,FGSSHZT 分公司审核状态,SQGLSJ 申请关联时间 from ZZ_MMJYDLRZHGLB where DLRSHZT ='待审核' and FGSSHZT='待审核' and JSZHLX='买家账户' and JSLX='买家') as MMJ left join ZZ_BUYJBZLXXB as BUY on MMJ.登录邮箱=BUY.DLYX left join ZZ_UserLogin as L on MMJ.登录邮箱=L.DLYX union select '' as 'ROWID',结算账户,DL.YHM 用户名,登录邮箱,DL.LXRXM 联系人姓名,DL.SJH 手机号,申请关联时间,DL.CreateTime 注册时间,经纪人审核状态,分公司审核状态,L.JSZHLX 结算账户类型,关联经纪人账号,DL.JSBH 角色编号,'审核' as 'CAOZUO' from (select distinct JSDLZH 登录邮箱,JSZHLX 结算账户,DLRDLZH 关联经纪人账号,DLRSHZT 经纪人审核状态,FGSSHZT 分公司审核状态,SQGLSJ 申请关联时间 from ZZ_MMJYDLRZHGLB where DLRSHZT ='待审核' and FGSSHZT='待审核' and JSZHLX='经纪人账户' and JSLX='买家') as MMJ left join dbo.ZZ_DLRJBZLXXB as DL on MMJ.登录邮箱=DL.DLYX left join ZZ_UserLogin as L on MMJ.登录邮箱=L.DLYX  ) as tab  ";  //检索的表(必须设置)
            ht_where["search_mainid"] = " 登录邮箱 ";  //所检索表的主键(必须设置)
            if (!string.IsNullOrEmpty(str_where))
            {
                ht_where["search_str_where"] = " 1=1 and 关联经纪人账号='" + PublicDS.PublisDsUser.Tables[0].Rows[0]["DLYX"].ToString() + "' " + str_where;  //检索条件(必须设置)
            }
            else
            {
                ht_where["search_str_where"] = " 1=1  and 关联经纪人账号='" + PublicDS.PublisDsUser.Tables[0].Rows[0]["DLYX"].ToString() + "'";  //检索条件(必须设置)
            }
            ht_where["search_paixu"] = " ASC ";  //排序方式(必须设置)
            ht_where["search_paixuZD"] = " 申请关联时间 ";  //用于排序的字段(必须设置)
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
        /// 点击查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuery_Click(object sender, EventArgs e)
        {
            str_where = "";
            if (cbxZCJS.Text == "全部")
            {
                str_where += " and 结算账户 like '%%'";
            }
            else
            {
                str_where += " and 结算账户 like '%" + cbxZCJS.Text + "%'";
            }
            str_where += "  and 用户名 like '%" + txtYHM.Text + "%' and  CONVERT(varchar(10),CAST(注册时间 as datetime),120) >='" + dtpTimeStart.Value.ToString("yyyy-MM-dd") + "' and CONVERT(varchar(10),CAST(注册时间 as datetime),120) <='" + dtpTimeEnd.Value.ToString("yyyy-MM-dd") + "' ";


        
            //更改搜索条件
            setDefaultSearch();
            //ht_where["search_str_where"] = " 1=1 and IP like '%" + IP + "%'";  //检索条件(必须设置)
            //执行查询
            GetData(ht_where);
        }
        /// <summary>
        /// dataGrid中 单元格单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;

            if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
                //设置默认收货地址的功能
                if (dgv.Columns[e.ColumnIndex].HeaderText == "操作")
                {
                    //获取当前点击行的number值
                    string strRoleNumber = dgv.Rows[e.RowIndex].Cells["JSBH"].Value.ToString();
                    string strRoleType = dgv.Rows[e.RowIndex].Cells["JSLX"].Value.ToString();
                    string strRoleEmail=dgv.Rows[e.RowIndex].Cells["DLYX"].Value.ToString();
                    if (formYHZLSH == null)
                    {
                        formYHZLSH = new FormYHZLXQ(strRoleType, strRoleNumber, strRoleEmail);
                        formYHZLSH.Show();
                    }
                    else
                    {
                        formYHZLSH.Close();
                        formYHZLSH = new FormYHZLXQ(strRoleType, strRoleNumber, strRoleEmail);
                        formYHZLSH.Show();
                        formYHZLSH.Activate();
                    
                    }
                }
            }
        }
    }
}
