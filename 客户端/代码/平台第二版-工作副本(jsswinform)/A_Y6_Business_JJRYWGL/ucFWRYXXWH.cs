using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using 客户端主程序.NewDataControl;
using System.Threading;
using 客户端主程序.Support;
using 客户端主程序.DataControl;

namespace 客户端主程序.SubForm.NewCenterForm.SUBUC.JJRYWGL
{
    public partial class ucFWRYXXWH : UserControl
    {
        Hashtable hashTableInfor;
        string strNumber = "";
        Hashtable htUser;
        Support.ManageDgv Dgv;
        public ucFWRYXXWH()
        {
            InitializeComponent();
            this.panelSM.Enabled = false;
            this.panelXX.Enabled = false;
            //初始化分页回调(带分页的页面，先把这个放上)
            ucPager1.DFT = new delegateForThread(STR_BeginBind);
        }
        /// <summary>
        ///确认
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {

            if (PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["结算账户类型"].ToString() != "经纪人交易账户")
            {
                ArrayList Almsg4 = new ArrayList();
                Almsg4.Add("");
                Almsg4.Add("只有经纪人交易账户可以执行此操作！");// 这里是提交失败的提示，可能是程序出错，可能是条件不足等
                FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "其他", "", Almsg4);
                FRSE4.ShowDialog();
                return;
            }
            hashTableInfor = new Hashtable();
            hashTableInfor["经纪人角色编号"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["经纪人角色编号"].ToString();
            hashTableInfor["银行登录账号"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString();
            hashTableInfor["结算账户类型"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["结算账户类型"].ToString();
        

            int tag = 0;
            if (String.IsNullOrEmpty(this.txtYGSSFZJG.Text.Trim()))
            {
                this.labRemYGSSFZJG.Visible = true;
                tag += 1;
            }
            else
            {
                this.labRemYGSSFZJG.Visible = false;
                hashTableInfor["员工隶属机构"] = this.txtYGSSFZJG.Text.Trim().Contains("'") ? this.txtYGSSFZJG.Text.Trim().Replace("'", "‘") : this.txtYGSSFZJG.Text.Trim();
            }
            if (String.IsNullOrEmpty(this.txtYGXM.Text.Trim()))
            {
                this.labRemYGXM.Visible = true;
                tag += 1;
            }
            else
            {
                this.labRemYGXM.Visible = false;
                hashTableInfor["员工姓名"] = this.txtYGXM.Text.Trim().Contains("'") ? this.txtYGXM.Text.Trim().Replace("'", "‘") : this.txtYGXM.Text.Trim();
            }
            if (String.IsNullOrEmpty(this.txtYGGH.Text.Trim()))
            {
                this.labRemYGGH.Visible = true;
                tag += 1;
            }
            else
            {
                this.labRemYGGH.Visible = false;
                hashTableInfor["员工工号"] = this.txtYGGH.Text.Trim();
            }
            hashTableInfor["联系方式"] = this.txtLXFS.Text.Trim().Contains("'") ? this.txtLXFS.Text.Trim().Replace("'", "‘") : this.txtLXFS.Text.Trim();
            hashTableInfor["联系地址"] = this.txtLXDZ.Text.Trim().Contains("'") ? this.txtLXDZ.Text.Trim().Replace("'", "‘") : this.txtLXDZ.Text.Trim();
            if (tag == 0)
            {
              
                //禁用提交区域并开启进度
                panelSM.Enabled = false;
                panelXX.Enabled = false;
                PBload.Visible = true;
                if (this.btnSave.Texts.Trim().Equals("确认"))
                {
                    //演示新标准
                    SRT_SetZTHFYHXYW_Run(hashTableInfor);                    
                }
                else
                {
                    hashTableInfor["银行人员表Number"] = strNumber;
                    SRT_SetZTHFYHXYW_Run_XG(hashTableInfor);
                    
                }
          
            }
        }

        /// <summary>
        /// 恢复
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.txtYGSSFZJG.Text = "";
            this.txtYGXM.Text = "";
            this.txtYGGH.Text = "";
            this.txtLXFS.Text = "";
            this.txtLXDZ.Text = "";
            this.btnSave.Texts = "确认";
            txtYGGH.Enabled = true;
        }
        #region//确认提交数据
        //开启一个测试线程
        private void SRT_SetZTHFYHXYW_Run(Hashtable hashTable)
        {
            RunThreadClassKTJYZH OTD = new RunThreadClassKTJYZH(hashTable, new delegateForThread(SRT_SetZTHFYHXYW));
            Thread trd = new Thread(new ThreadStart(OTD.SubmitYHUserInfor));
            trd.IsBackground = true;
            trd.Start();
        }
        //显示线程处理结果的函数(用于处理线程返回数据),调用异步委托
        private void SRT_SetZTHFYHXYW(Hashtable OutPutHT)
        {
            try { Invoke(new delegateForThreadShow(SRT_SetZTHFYHXYW_Invoke), new Hashtable[] { OutPutHT }); }
            catch (Exception ex) { Support.StringOP.WriteLog("委托回调错误：" + ex.ToString()); }
        }
        //处理非线程创建的控件
        private void SRT_SetZTHFYHXYW_Invoke(Hashtable OutPutHT)
        {
            //重新开放提交区域,并滚动条强制置顶
            panelSM.Enabled = true;
            panelXX.Enabled = true;
            PBload.Visible = false;
            UPUP();


            //返回值后的处理
            DataSet dsreturn = (DataSet)OutPutHT["返回值"];

            string zt = dsreturn.Tables["返回值单条"].Rows[0]["执行结果"].ToString();
            string showstr = dsreturn.Tables["返回值单条"].Rows[0]["提示文本"].ToString();
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
                    GetData(null);
                    Clear();
                    panelXX.Visible = true;
                    panelLB.Visible = false;
                    break;
                default:
                    ArrayList Almsg4 = new ArrayList();
                    Almsg4.Add("");
                    Almsg4.Add(showstr);// 这里是提交失败的提示，可能是程序出错，可能是条件不足等
                    FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "其他", "", Almsg4);
                    FRSE4.ShowDialog();

                    break;
            }
        }
        #endregion

        #region//修改数据
        //开启一个测试线程
        private void SRT_SetZTHFYHXYW_Run_XG(Hashtable hashTable)
        {
            RunThreadClassKTJYZH OTD = new RunThreadClassKTJYZH(hashTable, new delegateForThread(SRT_SetZTHFYHXYW_XG));
            Thread trd = new Thread(new ThreadStart(OTD.ModifyYHUserInfor));
            trd.IsBackground = true;
            trd.Start();
        }
        //显示线程处理结果的函数(用于处理线程返回数据),调用异步委托
        private void SRT_SetZTHFYHXYW_XG(Hashtable OutPutHT)
        {
            try { Invoke(new delegateForThreadShow(SRT_SetZTHFYHXYW_Invoke_XG), new Hashtable[] { OutPutHT }); }
            catch (Exception ex) { Support.StringOP.WriteLog("委托回调错误：" + ex.ToString()); }
        }
        //处理非线程创建的控件
        private void SRT_SetZTHFYHXYW_Invoke_XG(Hashtable OutPutHT)
        {
            //重新开放提交区域,并滚动条强制置顶
            panelSM.Enabled = true;
            panelXX.Enabled = true;
            PBload.Visible = false;
          


            //返回值后的处理
            DataSet dsreturn = (DataSet)OutPutHT["返回值"];

            string zt = dsreturn.Tables["返回值单条"].Rows[0]["执行结果"].ToString();
            string showstr = dsreturn.Tables["返回值单条"].Rows[0]["提示文本"].ToString();
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
                    txtYGGH.Enabled = true;
                    GetData(null);
                    Clear();
                    panelXX.Visible = false;
                    panelLB.Visible = true;
                    panelLB.Location = panelXX.Location;
                    break;
                default:
                    ArrayList Almsg4 = new ArrayList();
                    Almsg4.Add("");
                    Almsg4.Add(showstr);// 这里是提交失败的提示，可能是程序出错，可能是条件不足等
                    FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "其他", "", Almsg4);
                    FRSE4.ShowDialog();

                    break;
            }
        }
        #endregion


        #region//删除数据
        //开启一个测试线程
        private void SRT_SetZTHFYHXYW_Run_DEL(Hashtable hashTable)
        {
            RunThreadClassKTJYZH OTD = new RunThreadClassKTJYZH(hashTable, new delegateForThread(SRT_SetZTHFYHXYW_DEL));
            Thread trd = new Thread(new ThreadStart(OTD.DeleteYHUserInfor));
            trd.IsBackground = true;
            trd.Start();
        }
        //显示线程处理结果的函数(用于处理线程返回数据),调用异步委托
        private void SRT_SetZTHFYHXYW_DEL(Hashtable OutPutHT)
        {
            try { Invoke(new delegateForThreadShow(SRT_SetZTHFYHXYW_Invoke_DEL), new Hashtable[] { OutPutHT }); }
            catch (Exception ex) { Support.StringOP.WriteLog("委托回调错误：" + ex.ToString()); }
        }
        //处理非线程创建的控件
        private void SRT_SetZTHFYHXYW_Invoke_DEL(Hashtable OutPutHT)
        {
            //重新开放提交区域,并滚动条强制置顶
            panelSM.Enabled = true;
            panelXX.Enabled = true;
            PBload.Visible = false;



            //返回值后的处理
            DataSet dsreturn = (DataSet)OutPutHT["返回值"];

            string zt = dsreturn.Tables["返回值单条"].Rows[0]["执行结果"].ToString();
            string showstr = dsreturn.Tables["返回值单条"].Rows[0]["提示文本"].ToString();
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
                    GetData(null);
                    Clear();
                    panelXX.Visible = false;
                    panelLB.Visible = true;
                    panelLB.Location = panelXX.Location;
                    break;
                default:
                    ArrayList Almsg4 = new ArrayList();
                    Almsg4.Add("");
                    Almsg4.Add(showstr);// 这里是提交失败的提示，可能是程序出错，可能是条件不足等
                    FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "其他", "", Almsg4);
                    FRSE4.ShowDialog();

                    break;
            }
        }
        #endregion



        /// <summary>
        /// 重置表单
        /// </summary>
        private void reset()
        {

            ucFWRYXXWH uc = new ucFWRYXXWH();

            uc.Dock = DockStyle.Fill;//铺满
            uc.AutoScroll = true;//出现滚动条
            uc.BackColor = Color.AliceBlue;

            //清理控件
            Panel P = (Panel)this.Parent;
            P.Controls.Clear();
            P.Controls.Add(uc); //加入到某一个panel中
            uc.Show();//显示出来
            //将滚动条置顶
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
      
        private void ucZTYHXYW_B_Load(object sender, EventArgs e)
        {
            this.panelSM.Enabled = true;
            this.panelXX.Enabled = true;
            //将滚动条置顶
            //UPUP();
            //设置进度条的位置，放到按钮旁边
            PBload.Location = new Point(btnSave.Location.X + btnSave.Width + 25, btnSave.Location.Y + panelSM.Height);
            //非经纪人交易账户 此处的两个按钮不可用
            if (PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["结算账户类型"].ToString() != "经纪人交易账户")
            {
                this.panelSM.Enabled = false;
                this.panelXX.Enabled = false;
            }
            else
            {
                htUser = new Hashtable();
                htUser["登录邮箱"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString();
                htUser["经纪人角色编号"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["经纪人角色编号"].ToString();
                htUser["结算账户类型"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["结算账户类型"].ToString();
                GetData(null);
            }

        }

        #region//绑定列表基础信息
        /// <summary>
        /// 查询条件
        /// </summary>
        Hashtable ht_where = new Hashtable();
        /// <summary>
        /// 设置默认搜索条件(这个例子中的哈希表键值一个也不能少)
        /// </summary>
        private void setDefaultSearch()
        {
            //ht_where["GetCustomersDataPage_NAME"] = "GetCustomersDataPage";  //每页显示的条数(必须设置)
            //ht_where["page_index"] = "0";
            //ht_where["page_size"] = "5";  //每页显示的条数(必须设置)
            //ht_where["serach_Row_str"] = " Number '银行人员表Number',YHDLZH '银行登录账号',YGLSJG '员工隶属机构',YGXM '员工姓名',YGGH '员工工号',LXFS '联系方式',LXDZ '联系地址',SFYX '是否有效',TJSJ '提交时间' "; //检索字段(必须设置)
            //ht_where["search_tbname"] = "AAA_YHRYXXB";  //检索的表(必须设置)
            //ht_where["search_mainid"] = " Number ";  //所检索表的主键(必须设置)
            //ht_where["search_str_where"] = " SFYX='是' and YHDLZH='" + PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString() + "' ";  //检索条件(必须设置)
            //ht_where["search_paixu"] = " asc ";  //排序方式(必须设置)
            //ht_where["search_paixuZD"] = " TJSJ ";  //用于排序的字段(必须设置)

            ht_where["page_index"] = "0";
            ht_where["page_size"] = "5";  //每页显示的条数(必须设置)
            ht_where["webmethod"] = "经纪人业务管理B区";
            Hashtable ht_tiaojian = new Hashtable();
            ht_tiaojian["标签名"] = "查看银行员工列表";
            ht_tiaojian["用户邮箱"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString();
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
            Dgv = new Support.ManageDgv(this.dataGridView1);
            Dgv.AddMergeColumns(0, 2, "操作", -1);
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

        #endregion


        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex > -1)
            {
                //获取选中值 
                int rowindex = e.RowIndex;
                //回调
                this.txtYGSSFZJG.Text = dataGridView1.Rows[rowindex].Cells["员工隶属机构"].Value.ToString();
                this.txtYGXM.Text = dataGridView1.Rows[rowindex].Cells["员工姓名"].Value.ToString();
                this.txtYGGH.Text = dataGridView1.Rows[rowindex].Cells["员工工号"].Value.ToString();
                this.txtLXFS.Text = dataGridView1.Rows[rowindex].Cells["联系方式"].Value.ToString();
                this.txtLXDZ.Text = dataGridView1.Rows[rowindex].Cells["联系地址"].Value.ToString();
                strNumber = dataGridView1.Rows[rowindex].Cells["银行人员表Number"].Value.ToString();
                txtYGGH.Enabled = false;
                this.btnSave.Texts = "修改";
                panelXX.Visible = true;
                panelLB.Visible = false;
            }
            if (e.ColumnIndex == 1 && e.RowIndex > -1)
            {
                //获取选中值 
                int rowindex = e.RowIndex;
                ArrayList Almsg3 = new ArrayList();
                Almsg3.Add("");
                Almsg3.Add("您确认要删除此服务人员信息吗？");
                FormAlertMessage FRSE3 = new FormAlertMessage("确定取消", "问号", "", Almsg3);
                if (FRSE3.ShowDialog() == DialogResult.Yes)
                {
                    hashTableInfor = new Hashtable();
                    hashTableInfor["经纪人角色编号"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["经纪人角色编号"].ToString();
                    hashTableInfor["银行登录账号"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString();
                    hashTableInfor["结算账户类型"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["结算账户类型"].ToString();
                    hashTableInfor["银行人员表Number"] = dataGridView1.Rows[rowindex].Cells["银行人员表Number"].Value.ToString();
                    hashTableInfor["员工工号"] = dataGridView1.Rows[rowindex].Cells["员工工号"].Value.ToString();
                    SRT_SetZTHFYHXYW_Run_DEL(hashTableInfor);
                }
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            panelXX.Visible = true;
            panelLB.Visible = false;
            btnSave.Texts = "确认";
            Clear();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            panelXX.Visible = false;
            panelLB.Visible = true;
            panelLB.Location = panelXX.Location;
            Clear();
        }

        private void Clear()
        {
            txtYGSSFZJG.Text = "";
            txtYGXM.Text = "";
            txtYGGH.Text = "";
            txtLXFS.Text = "";
            txtLXDZ.Text = "";
        }


    }
}
