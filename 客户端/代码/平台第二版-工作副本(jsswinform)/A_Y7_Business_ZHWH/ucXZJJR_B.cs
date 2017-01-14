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
using 客户端主程序.DataControl;

namespace 客户端主程序.SubForm.NewCenterForm.SUBUC.ZHWH
{
    public partial class ucXZJJR_B : UserControl
    {
        
        /// <summary>
        /// 查询条件
        /// </summary>
        Hashtable ht_where = new Hashtable();
        /// <summary>
        ///用来保存用户数据
        /// </summary>
        Hashtable HTuser = new Hashtable();




        public ucXZJJR_B()
        {
            InitializeComponent();
            this.panel1.Enabled = false;
            this.panJJRSHQK.Enabled = false;
            this.panJJRZGZS.Enabled = false;
            this.dgvZGZS.Enabled = false;
            HTuser["卖家角色编号"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["卖家角色编号"].ToString();
            HTuser["买卖家登录邮箱"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString();
            HTuser["结算账户类型"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["结算账户类型"].ToString();
            HTuser["是否首次进来"] = "是";
            HTuser["搜索经纪人资格证书编号"] = "";
        }

        #region//获取数据
        //开启一个线程获取基础数据
        private void SRT_GetSiftJJRData_Run()
        {
            RunThreadClassKTJYZH OTD = new RunThreadClassKTJYZH(HTuser, new delegateForThread(SRT_GetSiftJJRData));
            Thread trd = new Thread(new ThreadStart(OTD.GetSiftJJRData));
            trd.IsBackground = true;
            trd.Start();
        }
        //显示线程处理结果的函数(用于处理线程返回数据),调用异步委托
        private void SRT_GetSiftJJRData(Hashtable OutPutHT)
        {
            try { Invoke(new delegateForThreadShow(SRT_GetSiftJJRData_Invoke), new Hashtable[] { OutPutHT }); }
            catch (Exception ex) { Support.StringOP.WriteLog("委托回调错误：" + ex.ToString()); }
        }
        //处理非线程创建的控件
        private void SRT_GetSiftJJRData_Invoke(Hashtable OutPutHT)
        {
          
            #region//此处是为了防止页面样式异常问题
            if (PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["结算账户类型"].ToString()=="")
            {
                ArrayList Almsg4 = new ArrayList();
                Almsg4.Add("");
                Almsg4.Add("您尚未提交开通交易账户申请，请及时提交！");// 这里是提交失败的提示，可能是程序出错，可能是条件不足等
                FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "其他", "", Almsg4);
                FRSE4.ShowDialog();
                return;
            }
           else if (PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["结算账户类型"].ToString() != "买家卖家交易账户")
            {
                ArrayList Almsg4 = new ArrayList();
                Almsg4.Add("");
                Almsg4.Add("只有交易方交易账户可以执行此操作！");// 这里是提交失败的提示，可能是程序出错，可能是条件不足等
                FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "其他", "", Almsg4);
                FRSE4.ShowDialog();
                return;
            }
            else if (PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["结算账户类型"].ToString() == "买家卖家交易账户")
            {
               
                    this.panel1.Enabled = true;
                    this.panJJRSHQK.Enabled = true;
                    this.panJJRZGZS.Enabled = true;
                    this.dgvZGZS.Enabled = true;
             
            }
            #endregion

            //重新开放提交区域,并滚动条强制置顶
            //返回值后的处理
            DataSet dsreturn = (DataSet)OutPutHT["返回值"];

            string zt = dsreturn.Tables["返回值单条"].Rows[0]["执行结果"].ToString();
            string showstr = dsreturn.Tables["返回值单条"].Rows[0]["提示文本"].ToString();
            //显示执行结果
            switch (zt)
            {
                case "ok":
                    //绑定界面用户数据
                    BindDataFaceData(dsreturn);
                    break;
                case "okErr":
                       ArrayList Almsg5 = new ArrayList();
                        Almsg5.Add("");
                        Almsg5.Add(showstr);// 这里是提交失败的提示，可能是程序出错，可能是条件不足等
                        FormAlertMessage FRSE5 = new FormAlertMessage("仅确定", "其他", "", Almsg5);
                        FRSE5.ShowDialog();
                    //绑定界面用户数据
                    BindDataFaceData(dsreturn,"");
                    break;
                case "err":
                    ArrayList Almsg6 = new ArrayList();
                    Almsg6.Add("");
                    Almsg6.Add(showstr);// 这里是提交失败的提示，可能是程序出错，可能是条件不足等
                    FormAlertMessage FRSE6 = new FormAlertMessage("仅确定", "其他", "", Almsg6);
                    FRSE6.ShowDialog();
                    //绑定界面用户数据
                    BindDataFaceData(dsreturn, "");
                    break;
                default:
                    lblJJRSHQK.Text = "审核情况：";
                    if (showstr == "您尚未开通交易账户，不能进行交易操作。")
                    {
                        this.panel1.Enabled = false;
                        this.panJJRSHQK.Enabled = false;
                        this.panJJRZGZS.Enabled = false;
                        this.dgvZGZS.Enabled = false;
                        ArrayList Almsg4 = new ArrayList();
                        Almsg4.Add("");
                        Almsg4.Add(showstr);// 这里是提交失败的提示，可能是程序出错，可能是条件不足等
                        FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "其他", "", Almsg4);
                        FRSE4.ShowDialog();
                    }
                    else
                    {
                        ArrayList Almsg4 = new ArrayList();
                        Almsg4.Add("");
                        Almsg4.Add(showstr);// 这里是提交失败的提示，可能是程序出错，可能是条件不足等
                        FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "其他", "", Almsg4);
                        FRSE4.ShowDialog();
                    }
                    break;
            }
        }
        /// <summary>
        /// 绑定界面数据信息
        /// </summary>
        /// <param name="dsReturn"></param>
        public void BindDataFaceData(DataSet dsReturn)
        {
            DataTable dataTableDQJJRZT = dsReturn.Tables["买卖家关联的当前经纪人信息"];
            DataTable dataTableXZJJR = dsReturn.Tables["买卖家可选经纪人信息"];
            DataTable dataTableGLGJJR = dsReturn.Tables["买卖家关联过的经纪人信息"];
            lblJJRSHQK.Text = "审核情况：";
            if (dataTableGLGJJR.Rows.Count > 0)
            {
                if (dataTableGLGJJR.Rows[0]["经纪人_冻结状态"].ToString() == "是" || dataTableGLGJJR.Rows[0]["经纪人_休眠状态"].ToString() == "是")
                {
                    lblJJRSHQK.Text = "审核情况：";
                }
                else
                {
                    lblJJRSHQK.Text = "审核情况：当前 " + dataTableGLGJJR.Rows[0]["经纪人交易方名称"] + " 审核通过";
                    this.labSHLSJL.Location = new Point(this.lblJJRSHQK.Location.X + this.lblJJRSHQK.Width + 10, this.labSHLSJL.Location.Y);
                }
            
            }
            if (dataTableDQJJRZT.Rows[0]["经纪人_冻结状态"].ToString() == "否" && dataTableDQJJRZT.Rows[0]["经纪人_休眠状态"].ToString() == "否")
            {
                BindDataGrid(dataTableXZJJR);
            }
            else if (dataTableDQJJRZT.Rows[0]["经纪人_冻结状态"].ToString() == "是" || dataTableDQJJRZT.Rows[0]["经纪人_休眠状态"].ToString() == "是")
            {
              
                BindDataGrid(dataTableXZJJR);
            }
        }
        /// <summary>
        /// 绑定界面数据信息  为了跟上面的数据重载
        /// </summary>
        /// <param name="dsReturn"></param>
        public void BindDataFaceData(DataSet dsReturn, string str)
        {
            DataTable dataTableDQJJRZT = dsReturn.Tables["买卖家关联的当前经纪人信息"];
            DataTable dataTableXZJJR = dsReturn.Tables["买卖家可选经纪人信息"];
            DataTable dataTableGLGJJR = dsReturn.Tables["买卖家关联过的经纪人信息"];
            lblJJRSHQK.Text = "审核情况：";
            if (dataTableGLGJJR.Rows.Count > 0)
            {
                if (dataTableGLGJJR.Rows[0]["经纪人_冻结状态"].ToString() == "是" || dataTableGLGJJR.Rows[0]["经纪人_休眠状态"].ToString() == "是")
                {
                    lblJJRSHQK.Text = "审核情况：";
                }
                else
                {
                    lblJJRSHQK.Text = "审核情况：当前 " + dataTableGLGJJR.Rows[0]["经纪人交易方名称"] + " 审核通过";
                    this.labSHLSJL.Location = new Point(this.lblJJRSHQK.Location.X + this.lblJJRSHQK.Width + 10, this.labSHLSJL.Location.Y);
                }

            }
        }
        /// <summary>
        /// 绑定gridview中的数据信息
        /// </summary>
        /// <param name="dataTable"></param>
        public void BindDataGrid(DataTable dataTable)
        {
            //是否自动绑定列
            dgvZGZS.AutoGenerateColumns = false;


            if (dataTable.Rows.Count > 0)
            {
                dgvZGZS.DataSource = dataTable.DefaultView;
            }
            else
            {
                dgvZGZS.DataSource = null;
            }
            //取消列表的自动排序
            for (int i = 0; i < this.dgvZGZS.Columns.Count; i++)
            {
                if (i == 0)
                {
                    //让第一列留出一点空白
                    DataGridViewCellStyle dgvcs = new DataGridViewCellStyle();
                    dgvcs.Padding = new Padding(10, 0, 0, 0);
                    this.dgvZGZS.Columns[i].HeaderCell.Style = dgvcs;
                    this.dgvZGZS.Columns[i].DefaultCellStyle = dgvcs;
                }
                this.dgvZGZS.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }
        #endregion


        #region//执行选择经纪人的操作
        //开启一个线程获取基础数据
        private void SRT_GetSelectJudgeJJRData_Run()
        {
            RunThreadClassKTJYZH OTD = new RunThreadClassKTJYZH(HTuser, new delegateForThread(SRT_GetSelectJudgeJJRData));
            Thread trd = new Thread(new ThreadStart(OTD.GetSelectJudgeJJRData));
            trd.IsBackground = true;
            trd.Start();
        }
        //显示线程处理结果的函数(用于处理线程返回数据),调用异步委托
        private void SRT_GetSelectJudgeJJRData(Hashtable OutPutHT)
        {
            try { Invoke(new delegateForThreadShow(SRT_GetSelectJudgeJJRData_Invoke), new Hashtable[] { OutPutHT }); }
            catch (Exception ex) { Support.StringOP.WriteLog("委托回调错误：" + ex.ToString()); }
        }
        //处理非线程创建的控件
        private void SRT_GetSelectJudgeJJRData_Invoke(Hashtable OutPutHT)
        {
            //重新开放提交区域,并滚动条强制置顶
            //返回值后的处理
            DataSet dsreturn = (DataSet)OutPutHT["返回值"];

            string zt = dsreturn.Tables["返回值单条"].Rows[0]["执行结果"].ToString();
            string showstr = dsreturn.Tables["返回值单条"].Rows[0]["提示文本"].ToString();
            //显示执行结果
            switch (zt)
            {
                case "ok":
                    //绑定界面用户数据
                   ArrayList Almsg3 = new ArrayList();
                    Almsg3.Add("");
                    Almsg3.Add(showstr);
                    FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "其他", "", Almsg3);
                    FRSE3.ShowDialog();
                    reset();
                    break;
                default:
                    ArrayList Almsg4 = new ArrayList();
                    Almsg4.Add("");
                    Almsg4.Add(showstr);// 这里是提交失败的提示，可能是程序出错，可能是条件不足等
                    FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "其他", "", Almsg4);
                    FRSE4.ShowDialog();
                    reset();
                    break;
            }
        }

        /// <summary>
        /// 执行选择操作是进行相应的判断操作
        /// </summary>
        /// <param name="dsReturn"></param>
        public void BindGetSelectJudgeJJRData(DataSet dsReturn)
        {
            DataTable dataTableDQJJRZT = dsReturn.Tables["买卖家关联的当前经纪人信息"];
            DataTable dataTableGLGJJR = dsReturn.Tables["买卖家关联过的经纪人信息"];
            if (dataTableGLGJJR.Rows.Count > 0)
            {
                lblJJRSHQK.Text = "审核情况：当前" + dataTableGLGJJR.Rows[0]["经纪人交易方名称"] + "审核通过";

            }
            if (dsReturn.Tables["返回值单条"].Rows[0]["附件信息1"].ToString()!="")
            {
                this.panJJRSHQK.Enabled = true;
                ArrayList Almsg4 = new ArrayList();
                Almsg4.Add("");
                Almsg4.Add(dsReturn.Tables["返回值单条"].Rows[0]["附件信息1"].ToString());
                FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "其他", "", Almsg4);
                FRSE4.ShowDialog();
                this.panJJRZGZS.Enabled = false;
                this.dgvZGZS.Enabled = false;
                return;
            }
            else if (dsReturn.Tables["返回值单条"].Rows[0]["附件信息2"].ToString() != "")
            {
                this.panJJRSHQK.Enabled = true;
                ArrayList Almsg4 = new ArrayList();
                Almsg4.Add("");
                Almsg4.Add(dsReturn.Tables["返回值单条"].Rows[0]["附件信息2"].ToString());
                FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "其他", "", Almsg4);
                FRSE4.ShowDialog();
                this.panJJRZGZS.Enabled = false;
                this.dgvZGZS.Enabled = false;
                    return;
              
            }
            else//真正的执行选择经纪人以后的操作
            {
                ArrayList Almsg4 = new ArrayList();
                Almsg4.Add("");
                Almsg4.Add(dsReturn.Tables["返回值单条"].Rows[0]["提示文本"].ToString());
                FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "其他", "", Almsg4);
                FRSE4.ShowDialog();
                reset();
            }
        }
        #endregion


        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(this.txtJJRZGZSBH.Text.Trim()))
            {
                HTuser["搜索经纪人资格证书编号"] = this.txtJJRZGZSBH.Text.Trim();
                SRT_GetSiftJJRData_Run();
            }
            else
            {
                HTuser["搜索经纪人资格证书编号"] = "";
                SRT_GetSiftJJRData_Run();
            }
       
        }


        /// <summary>
        /// 重置表单
        /// </summary>
        private void reset()
        {
            ucXZJJR_B s = new ucXZJJR_B();
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
        private void ucHWQS_B_Load(object sender, EventArgs e)
        {
            SRT_GetSiftJJRData_Run();
        }

        /// <summary>
        /// 单击操作按钮的时候执行相应的操作（此处是点击 对号按钮时执行相应的操作）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvZGZS_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2 && e.RowIndex > -1)
            {
                //获取选中值
                int rowindex = e.RowIndex;

                //回调

                HTuser["关联经纪人登录邮箱"] = dgvZGZS.Rows[rowindex].Cells[3].Value.ToString();//经纪人登录邮箱
                HTuser["关联经纪人角色编号"] = dgvZGZS.Rows[rowindex].Cells[4].Value.ToString();//经纪人角色编号
                HTuser["关联经纪人用户名"] = dgvZGZS.Rows[rowindex].Cells[5].Value.ToString();//经纪人用户名
                SRT_GetSelectJudgeJJRData_Run();
            }
        }
        /// <summary>
        /// 查询审核通过的历史记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void labSHLSJL_Click(object sender, EventArgs e)
        {
            if (PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["结算账户类型"].ToString() != "买家卖家交易账户")
            {
                ArrayList Almsg4 = new ArrayList();
                Almsg4.Add("");
                Almsg4.Add("只有交易方交易账户可以执行此操作！");// 这里是提交失败的提示，可能是程序出错，可能是条件不足等
                FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "其他", "", Almsg4);
                FRSE4.ShowDialog();
                return;
            }

            ucZHZL_B_lishi uc = new ucZHZL_B_lishi(null);
            uc.ShowDialog();

        }

        private void dgvZGZS_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                dgvZGZS.Cursor = Cursors.Hand;
            }
            else
            {
                dgvZGZS.Cursor = Cursors.Default;
            }
        }
        /// <summary>
        /// 双击列表行 执行相应的操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvZGZS_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex != 2&&e.RowIndex > -1)
            {
                //获取选中值
                int rowindex = e.RowIndex;

                //回调

                HTuser["关联经纪人登录邮箱"] = dgvZGZS.Rows[rowindex].Cells[3].Value.ToString();//经纪人登录邮箱
                HTuser["关联经纪人角色编号"] = dgvZGZS.Rows[rowindex].Cells[4].Value.ToString();//经纪人角色编号
                HTuser["关联经纪人用户名"] = dgvZGZS.Rows[rowindex].Cells[5].Value.ToString();//经纪人用户名
                SRT_GetSelectJudgeJJRData_Run();
            }
        }

      

    }
}
