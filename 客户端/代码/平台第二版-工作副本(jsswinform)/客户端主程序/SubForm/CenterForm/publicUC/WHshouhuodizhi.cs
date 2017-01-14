using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Threading;
using 客户端主程序.Support;
using Com.Seezt.Skins;
using 客户端主程序.DataControl;

namespace 客户端主程序.SubForm.CenterForm.publicUC
{
    public partial class WHshouhuodizhi : UserControl
    {
        /// <summary>
        /// 查询条件
        /// </summary>
        Hashtable ht_where = new Hashtable();

        public WHshouhuodizhi()
        {
            InitializeComponent();

            //初始化分页回调(带分页的页面，先把这个放上)
            ucPager1.DFT = new delegateForThread(ShowThreadResult_BeginBind);
        }

        //加载窗体后处理数据
        private void UserControl1_Load(object sender, EventArgs e)
        {
            //必须先初始化省市区下拉框
            ucCityList1.initdefault();
            //设置默认显示的数据，一般只在编辑的时候才用，默认不需要这句话
            //ucCityList1.SelectedItem = new string[] { "山东省", "济南市", "历下区" };
            //设置是否允许选择，特殊情况才用
            //ucCityList1.EnabledItem = new bool[] { true, true, true };

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
                    dgvcs.Padding = new Padding(10, 0, 0, 0);

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
            ht_where["page_size"] = "10";  //每页显示的条数(必须设置)
            ht_where["serach_Row_str"] = "number,shdwmc as 收货单位名称,shrxm as 收货人姓名,szsf+szds+szqx as 所在区域,xxdz as 详细地址,yzbm as 邮政编码,lxdh as 联系电话,(case sfmrdz when '是' then '默认地址' else '设为默认地址' end) as 默认地址 "; //检索字段(必须设置)
            ht_where["search_tbname"] = "  ZZ_SHDZXXB  ";  //检索的表(必须设置)
            ht_where["search_mainid"] = " number ";  //所检索表的主键(必须设置)
            ht_where["search_str_where"] = " 1=1 ";  //检索条件(必须设置)
            ht_where["search_paixu"] = " DESC ";  //排序方式(必须设置)
            ht_where["search_paixuZD"] = "sfmrdz DESC,createtime ";  //用于排序的字段(必须设置)
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
            ht_where["search_str_where"] = ht_where["search_str_where"] + " and jsdlzh='" + PublicDS.PublisDsUser.Tables[0].Rows[0]["DLYX"].ToString() + "'";
            ucPager1.HT_Where = ht_where;
            ucPager1.BeginBindForUCPager();
        }
     
        /// <summary>
        /// 重设进度条位置，放到按钮旁边
        /// </summary>
        /// <param name="BB">参考提交按钮</param>
        /// <param name="PB">要移动的进度条</param>
        private void resetloadLocation(BasicButton BB, PictureBox PB)
        {
            PB.Location = new Point(BB.Location.X + BB.Width + 30, BB.Location.Y);
            PB.Visible = true;
        }

        //取消按钮的操作
        private void ResetB_Click(object sender, EventArgs e)
        {
            //重置本表单，或转向其他页面
            WHshouhuodizhi UC = new WHshouhuodizhi();//这句话是关键，若实例化本身的类，则是重置表单，若实例化其他的类，则是跳转
            reset(UC);
        }
       
        /// <summary>
        /// 重置表单
        /// </summary>
        /// <param name="UC"></param>
        private void reset(WHshouhuodizhi UC)
        {
            this.SuspendLayout();
            UC.Dock = DockStyle.Fill;//铺满
            UC.AutoScroll = true;//出现滚动条
            UC.BackColor = Color.AliceBlue;
            UC.Padding = new Padding(10);
            //清理控件
            Panel P = (Panel)this.Parent;
            P.Controls.Clear();
            P.Controls.Add(UC); //加入到某一个panel中
            UC.Show();//显示出来
            this.ResumeLayout(false);
            //更改标题(如果重置本表单，不需要执行这句话)
            //this.ParentForm.Text = "xxx提交成功";
        }

        //鼠标点击单元格中的文字动作
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;

            if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
                //获取当前点击行的number值
                string number = dgv.Rows[e.RowIndex].Cells["id"].Value.ToString();
                string dlyx = PublicDS.PublisDsUser.Tables[0].Rows[0]["DLYX"].ToString();

                //设置默认收货地址的功能
                if (dgv.Columns[e.ColumnIndex].HeaderText == "默认地址")
                {
                    if (dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "默认地址")
                    {
                        ArrayList AlmsgMR = new ArrayList();
                        AlmsgMR.Add("");
                        AlmsgMR.Add("该地址已经是默认地址！");
                        FormAlertMessage FRSEMR = new FormAlertMessage("仅确定", "叹号", "处理成功", AlmsgMR);
                        FRSEMR.ShowDialog();
                    }
                    else
                    {//处理“设为默认地址”的操作                   
                        SetMrAddress(number, dlyx);
                    }
                }

                //处理修改操作
                if (dgv.Columns[e.ColumnIndex].HeaderText == "修改")
                {
                    //根据number值获取所选信息的具体内容并给页面中对应的控件赋值                   
                    GetEditAddress(number, dlyx); 
                }

                //删除收货地址的功能
                if (dgv.Columns[e.ColumnIndex].HeaderText == "删除")
                {
                    //是否要删除收货地址的提醒信息
                    ArrayList Almsgxx = new ArrayList();
                    Almsgxx.Add("");
                    Almsgxx.Add("您确定要删除该收货地址吗？");
                    if (dgv.Rows[e.RowIndex].Cells["默认地址"].Value.ToString() == "默认地址")
                    {//默认地址不能删除  
                        Almsgxx.Add("此为默认地址，删除后请重新设置默认地址！");
                    }
                    FormAlertMessage FRSExx = new FormAlertMessage("确定取消", "问号", "删除收货地址", Almsgxx);
                    DialogResult dr = FRSExx.ShowDialog();
                    if (dr == DialogResult.Yes)
                    {
                        DelAddress(number, dlyx);
                    }
                    else
                    {
                        return;
                    }

                }
            }
        }

        #region “保存”按钮的操作。点击后保存收货地址信息
        private void btnSave_Click(object sender, EventArgs e)
        {
            //获得表单数据和进行基本验证
            Hashtable ht_form = new Hashtable();

            //获取所在区域的选择内容
            string[] area = ucCityList1.SelectedItem;
            string sheng = area[0].ToString();
            string shi = area[1].ToString();
            string quxian = area[2].ToString();

            #region 进行完整性验证，判断是否已填写完整信息。
            ArrayList Almsg3 = new ArrayList();
            Almsg3.Add("");           

            if (uctbSHRXM.Text.Trim() == "")
            {
                Almsg3.Add("请填写 收货人姓名！");
            }
            else if (sheng.IndexOf("请选择") >= 0 || shi.IndexOf("请选择") >= 0 || quxian.IndexOf("请选择") >= 0)
            {
                Almsg3.Add("请选择 所在区域！");
            }
            else if (uctbXXDZ.Text.Trim() == "")
            {
                Almsg3.Add("请填写 详细地址！");
            }
            else if (uctbYZBM.Text.Trim() == "")
            {
                Almsg3.Add("请填写 邮政编码！");
            }
            else if (uctbLXDH.Text.Trim() == "")
            {
                Almsg3.Add("请填写 联系电话！");
            }
            if (Almsg3.Count > 1)
            {
                FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "错误", "提示", Almsg3);
                FRSE3.ShowDialog();
                return;
            }
            #endregion

            #region 给要传递的参数赋值
            ht_form["dlyx"] = PublicDS.PublisDsUser.Tables[0].Rows[0]["DLYX"].ToString();
            ht_form["jsbh"] = PublicDS.PublisDsUser.Tables[0].Rows[0]["买家角色编号"].ToString();
            ht_form["Number"] = lblNumber.Text.Trim();
            ht_form["SHDWMC"] = uctbSHDWMC.Text.Trim();
            ht_form["SHRXM"] = uctbSHRXM.Text.Trim();
            ht_form["sheng"] = sheng;
            ht_form["shi"] = shi;
            ht_form["quxian"] = quxian;
            ht_form["XXDZ"] = uctbXXDZ.Text.Trim();
            ht_form["YZBM"] = uctbYZBM.Text.Trim();
            ht_form["LXDH"] = uctbLXDH.Text.Trim();
            if (ckbSetMR.Checked==true)
            {
                ht_form["MRDZ"] = "是";
            }
            else
            {
                ht_form["MRDZ"] = "否";
            }
            #endregion

            //禁用提交区域并开启进度
            panelUC5.Enabled = false;
            panelUC2.Enabled = false;
            resetloadLocation(ResetB, PBload);


            //开启线程提交数据
            delegateForThread dft = new delegateForThread(ShowThreadResult_save);
            DataControl.RunThreadClassManageAddress RTCTSOU = new DataControl.RunThreadClassManageAddress(ht_form, dft);
            Thread trd = new Thread(new ThreadStart(RTCTSOU.BeginRun_SaveAddress));
            trd.IsBackground = true;
            trd.Start();
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
            //重新开放提交区域
            panelUC5.Enabled = true;
            panelUC2.Enabled = true;
            PBload.Visible = false;

            //显示执行结果
            string zt = returnHT["执行状态"].ToString();
            switch (zt)
            {
                case "ok":
                    //给出表单提交成功的提示
                    ArrayList Almsg3 = new ArrayList();
                    Almsg3.Add("");
                    Almsg3.Add(returnHT["执行结果"].ToString().Trim());
                    FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "叹号", "处理成功", Almsg3);
                    FRSE3.ShowDialog();

                    //重置本表单，或转向其他页面
                    WHshouhuodizhi UC = new WHshouhuodizhi();//这句话是关键，若实例化本身的类，则是重置表单，若实例化其他的类，则是跳转
                    reset(UC);

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
                    Almsg4.Add(returnHT["执行结果"].ToString());// 这里是提交失败的提示，可能是程序出错，可能是条件不足等
                    FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "错误", "保存失败", Almsg4);
                    FRSE4.ShowDialog();

                    break;
            }

        }
        #endregion


        #region 设置默认收货地址的操作
        //设置默认地址
        private void SetMrAddress(string number,string dlyx)
        {
            //获得表单数据和进行基本验证
            Hashtable ht_form = new Hashtable();
            ht_form["number"] = number;
            ht_form["dlyx"] = dlyx;
            if (ht_form["number"].ToString() == ""||ht_form ["dlyx"].ToString ()=="")
            {
                ArrayList Almsg3 = new ArrayList();
                Almsg3.Add("");
                Almsg3.Add("无法设置默认收货地址！");
                FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "错误", "提示", Almsg3);
                FRSE3.ShowDialog();
                return;
            }

            //禁用提交区域、列表区域并开启进度
            panelUC5.Enabled = false;            
            panelUC2.Enabled = false;
            resetloadLocation(ResetB, PBload);
           


            //开启线程提交数据
            delegateForThread dft = new delegateForThread(ShowThreadResult_SetMrAddress);
            DataControl.RunThreadClassManageAddress RTCTSOU = new DataControl.RunThreadClassManageAddress(ht_form, dft);
            Thread trd = new Thread(new ThreadStart(RTCTSOU.BeginRun_SetMrAddress));
            trd.IsBackground = true;
            trd.Start();
        }


        //显示线程处理结果的函数(用于处理线程返回数据)
        private void ShowThreadResult_SetMrAddress(Hashtable returnHT)
        {
            try
            {
                //调用异步委托
                Invoke(new delegateForThreadShow(ShowThreadResult_SetMrAddress_Invoke), new Hashtable[] { returnHT });
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("委托回调错误：" + ex.ToString());
            }
        }

        //处理非线程创建的控件
        private void ShowThreadResult_SetMrAddress_Invoke(Hashtable returnHT)
        {
            //重新开放提交区域
            panelUC5.Enabled = true;
            panelUC2.Enabled = true;
            PBload.Visible = false;

            //显示执行结果
            string zt = returnHT["执行状态"].ToString();
            switch (zt)
            {
                case "ok":

                    //给出表单提交成功的提示
                    ArrayList Almsg3 = new ArrayList();
                    Almsg3.Add("");
                    Almsg3.Add("设置默认收货地址成功！");
                    FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "叹号", "处理成功", Almsg3);
                    FRSE3.ShowDialog();

                    //重置本表单，或转向其他页面
                    WHshouhuodizhi UC = new WHshouhuodizhi();//这句话是关键，若实例化本身的类，则是重置表单，若实例化其他的类，则是跳转
                    reset(UC);
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
                    Almsg4.Add("设置默认地址失败！");// 这里是提交失败的提示，可能是程序出错，可能是条件不足等
                    FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "错误", "设置失败", Almsg4);
                    FRSE4.ShowDialog();

                    break;
            }

        }
        #endregion

        #region 删除收货地址的操作
        //删除收货地址
        private void DelAddress(string number, string dlyx)
        {
            //获得表单数据和进行基本验证
            Hashtable ht_form = new Hashtable();
            ht_form["number"] = number;
            ht_form["dlyx"] = dlyx;
            if (ht_form["number"].ToString() == ""||ht_form["dlyx"].ToString () =="")
            {
                ArrayList Almsg3 = new ArrayList();
                Almsg3.Add("");
                Almsg3.Add("无法执行删除操作！");
                FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "错误", "提示", Almsg3);
                FRSE3.ShowDialog();
                return;
            }

            //禁用提交区域、列表区域并开启进度
            panelUC5.Enabled = false;
            panelUC2.Enabled = false;
            resetloadLocation(ResetB, PBload);



            //开启线程提交数据
            delegateForThread dft = new delegateForThread(ShowThreadResult_DelAddress);
            DataControl.RunThreadClassManageAddress RTCTSOU = new DataControl.RunThreadClassManageAddress(ht_form, dft);
            Thread trd = new Thread(new ThreadStart(RTCTSOU.BeginRun_DelAddress));
            trd.IsBackground = true;
            trd.Start();
        }


        //显示线程处理结果的函数(用于处理线程返回数据)
        private void ShowThreadResult_DelAddress(Hashtable returnHT)
        {
            try
            {
                //调用异步委托
                Invoke(new delegateForThreadShow(ShowThreadResult_DelAddress_Invoke), new Hashtable[] { returnHT });
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("委托回调错误：" + ex.ToString());
            }
        }

        //处理非线程创建的控件
        private void ShowThreadResult_DelAddress_Invoke(Hashtable returnHT)
        {
            //重新开放提交区域
            panelUC5.Enabled = true;
            panelUC2.Enabled = true;
            PBload.Visible = false;

            //显示执行结果
            string zt = returnHT["执行状态"].ToString();
            switch (zt)
            {
                case "ok":

                    //给出表单提交成功的提示
                    ArrayList Almsg3 = new ArrayList();
                    Almsg3.Add("");
                    Almsg3.Add("收货地址删除成功！");
                    FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "叹号", "处理成功", Almsg3);
                    FRSE3.ShowDialog();

                    //重置本表单，或转向其他页面
                    WHshouhuodizhi UC = new WHshouhuodizhi();//这句话是关键，若实例化本身的类，则是重置表单，若实例化其他的类，则是跳转
                    reset(UC);
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
                    Almsg4.Add("删除地址信息失败！");// 这里是提交失败的提示，可能是程序出错，可能是条件不足等
                    FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "错误", "删除失败", Almsg4);
                    FRSE4.ShowDialog();

                    break;
            }

        }
        #endregion

        #region 获取要修改的地址信息的具体内容
        //删除收货地址
        private void GetEditAddress(string number, string dlyx)
        {
            //获得表单数据和进行基本验证
            Hashtable ht_form = new Hashtable();
            ht_form["number"] = number;
            ht_form["dlyx"] = dlyx;
            if (ht_form["number"].ToString() == "" || ht_form["dlyx"].ToString() == "")
            {
                ArrayList Almsg3 = new ArrayList();
                Almsg3.Add("");
                Almsg3.Add("无法执行修改操作！");
                FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "错误", "提示", Almsg3);
                FRSE3.ShowDialog();
                return;
            }

            //禁用提交区域、列表区域并开启进度
            panelUC5.Enabled = false;
            panelUC2.Enabled = false;
            resetloadLocation(ResetB, PBload);


            //开启线程提交数据
            delegateForThread dft = new delegateForThread(ShowThreadResult_GetEditAddress);
            DataControl.RunThreadClassManageAddress RTCTSOU = new DataControl.RunThreadClassManageAddress(ht_form, dft);
            Thread trd = new Thread(new ThreadStart(RTCTSOU.BeginRun_GetEditAddress));
            trd.IsBackground = true;
            trd.Start();
        }


        //显示线程处理结果的函数(用于处理线程返回数据)
        private void ShowThreadResult_GetEditAddress(Hashtable returnHT)
        {
            try
            {
                //调用异步委托
                Invoke(new delegateForThreadShow(ShowThreadResult_GetEditAddress_Invoke), new Hashtable[] { returnHT });
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("委托回调错误：" + ex.ToString());
            }
        }

        //处理非线程创建的控件
        private void ShowThreadResult_GetEditAddress_Invoke(Hashtable returnHT)
        {
            //重新开放提交区域
            panelUC5.Enabled = true;
            panelUC2.Enabled = true;
            PBload.Visible = false;

            //显示执行结果
            DataSet resultDs = (DataSet)returnHT["执行状态"];
           
            if (resultDs != null && resultDs.Tables.Count==1&&resultDs .Tables [0].Rows .Count ==1&&resultDs.Tables[0].Rows[0]["执行状态"].ToString()=="ok")
            {
                lblNumber.Text = resultDs.Tables[0].Rows[0]["Number"].ToString();
                uctbSHDWMC.Text = resultDs.Tables[0].Rows[0]["SHDWMC"].ToString();
                uctbSHRXM.Text = resultDs.Tables[0].Rows[0]["SHRXM"].ToString();
                ucCityList1.SelectedItem=new string[]{resultDs.Tables[0].Rows[0]["SZSF"].ToString(), resultDs.Tables[0].Rows[0]["SZDS"].ToString(), resultDs.Tables[0].Rows[0]["SZQX"].ToString() };
                uctbXXDZ.Text = resultDs.Tables[0].Rows[0]["XXDZ"].ToString();
                uctbYZBM.Text = resultDs.Tables[0].Rows[0]["YZBM"].ToString();
                uctbLXDH.Text = resultDs.Tables[0].Rows[0]["LXDH"].ToString();
                if (resultDs.Tables[0].Rows[0]["MRDZ"].ToString() == "是")
                {
                    ckbSetMR.Checked = true;
                }
                else
                {
                    ckbSetMR.Checked = false;
                }
            } 
            else
            {
                if (resultDs == null)
                {
                    ArrayList Almsg7 = new ArrayList();
                    Almsg7.Add("");
                    Almsg7.Add("修改失败！");
                    FormAlertMessage FRSE7 = new FormAlertMessage("仅确定", "错误", "系统忙", Almsg7);
                    FRSE7.ShowDialog();
                }
                else
                {
                    ArrayList Almsg4 = new ArrayList();
                    Almsg4.Add("");
                    Almsg4.Add(resultDs.Tables[0].Rows[0]["执行结果"].ToString());// 这里是提交失败的提示，可能是程序出错，可能是条件不足等
                    FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "错误", "修改失败", Almsg4);
                    FRSE4.ShowDialog();
                }
            }  
        }
        #endregion

    }
}
