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
using System.Web;


namespace 客户端主程序.SubForm.CenterForm.Buyer
{
    public partial class CXshoukuanzhanghu : UserControl
    {
        /// <summary>
        /// 查询条件
        /// </summary>
        Hashtable ht_where = new Hashtable();
        string JSNM="";
        public CXshoukuanzhanghu()
        {
            InitializeComponent();
            //获取角色类型
            string JSZHLX = PublicDS.PublisDsUser.Tables[0].Rows[0]["JSZHLX"].ToString();

            //根据类型获取编号
            if (JSZHLX == "卖家账户")
            {
                JSNM = PublicDS.PublisDsUser.Tables[0].Rows[0]["卖家角色编号"].ToString().Trim();
            }
            if (JSZHLX == "买家账户")
            {
                JSNM = PublicDS.PublisDsUser.Tables[0].Rows[0]["买家角色编号"].ToString().Trim();
            }
            if (JSZHLX == "经纪人账户")
            {
                JSNM = PublicDS.PublisDsUser.Tables[0].Rows[0]["经纪人角色编号"].ToString().Trim();
            }
            //初始化分页回调(带分页的页面，先把这个放上)
            ucPager1.DFT = new delegateForThread(ShowThreadResult_BeginBind);
            
        }

        //加载窗体后处理数据
        private void CXshoukuanzhanghu_Load(object sender, EventArgs e)
        {
            GetData(null);
            this.ucTextBoxYHZK.OpenZS = true;
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
            ht_where["serach_Row_str"] = " [Number] ,[CreateTime] as 添加时间,[SKZHMC] as 收款人姓名,[KHYHMC] as 开户银行名称,[FilePath] as 文件路径,[KHYHZH] as  银行卡号,case when [SFMRZH]='是' then '默认收款账号' else '设为默认收款账号' end as 默认收款账号 "; //检索字段(必须设置)
            ht_where["search_tbname"] = "  ZZ_SKZHXXB  ";  //检索的表(必须设置)
            ht_where["search_mainid"] = " Number ";  //所检索表的主键(必须设置)
            ht_where["search_str_where"] = " 1=1 ";  //检索条件(必须设置)
            ht_where["search_paixu"] = " DESC ";  //排序方式(必须设置)
            ht_where["search_paixuZD"] = "case when [SFMRZH]='是' then '默认收款账号' else '设为默认收款账号' end, CreateTime, Number ";  //用于排序的字段(必须设置)
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

      
        //保存按钮的后置代码
        private void btnSave_Click(object sender, EventArgs e)
        {            
            //获得表单数据和进行基本验证
            Hashtable ht_skinfo = new Hashtable();
            ht_skinfo["收款人姓名"]=this.ucTextBoxSKR.Text.Trim();
            ht_skinfo["开户银行名称"] = this.ucTextBoxKHYH.Text.Trim();
            ht_skinfo["银行账号"] = this.ucTextBoxYHZK.Text.Trim();
            ht_skinfo["买家角色编号"] =JSNM;
            ht_skinfo["登录邮箱"] = PublicDS.PublisDsUser.Tables[0].Rows[0]["DLYX"].ToString().Trim();
            ht_skinfo["文件路径"] =ucTextBoxSFZ.Text.Trim().Replace(@"\", "/");
            ht_skinfo["是否默认账户"] = this.checkBox1.Checked ? "是" : "否";
            ht_skinfo["IsNum"] = this.lbNumber.Text.Trim();

            if (ht_skinfo["收款人姓名"].ToString() == "")
            {
                ArrayList Almsg3 = new ArrayList();
                Almsg3.Add("");
                Almsg3.Add("收款人姓名必须填写！");
                FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "错误", "提示", Almsg3);
                FRSE3.ShowDialog();
                return;
            }
            else if (ht_skinfo["收款人姓名"].ToString().Length>50)
            {
                ArrayList Almsg3 = new ArrayList();
                Almsg3.Add("");
                Almsg3.Add("收款人姓名不可超过25个汉字！");
                FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "错误", "提示", Almsg3);
                FRSE3.ShowDialog();
                return;
            }
            else if (ht_skinfo["开户银行名称"].ToString().Length > 50)
            {
                ArrayList Almsg3 = new ArrayList();
                Almsg3.Add("");
                Almsg3.Add("开户银行名称不可超过25个汉字！");
                FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "错误", "提示", Almsg3);
                FRSE3.ShowDialog();
                return;
            }
            else if (dataGridView1.DataSource != null && ((DataView)dataGridView1.DataSource).Count >= 5 && lbNumber.Text == "")
            {
                ArrayList Almsg3 = new ArrayList();
                Almsg3.Add("");
                Almsg3.Add("最多设置5个账号！");
                FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "错误", "提示", Almsg3);
                FRSE3.ShowDialog();
            }
            else if (ht_skinfo["开户银行名称"].ToString() == "")
            {
                ArrayList Almsg3 = new ArrayList();
                Almsg3.Add("");
                Almsg3.Add("开户银行名称必须填写！");
                FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "错误", "提示", Almsg3);
                FRSE3.ShowDialog();
                return;
            }
            else if (ht_skinfo["银行账号"].ToString() == "")
            {
                ArrayList Almsg3 = new ArrayList();
                Almsg3.Add("");
                Almsg3.Add("银行账号必须填写！");
                FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "错误", "提示", Almsg3);
                FRSE3.ShowDialog();
                return;
            }
            else if (ht_skinfo["银行账号"].ToString().Length > 19)
            {
                ArrayList Almsg3 = new ArrayList();
                Almsg3.Add("");
                Almsg3.Add("银行账号不可超过19个数字！");
                FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "错误", "提示", Almsg3);
                FRSE3.ShowDialog();
                return;
            }
            else if (ht_skinfo["文件路径"].ToString() == "")
            {
                ArrayList Almsg3 = new ArrayList();
                Almsg3.Add("");
                Almsg3.Add("请上传身份证扫描件！");
                FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "错误", "提示", Almsg3);
                FRSE3.ShowDialog();

               
                return;
            }
           
            else
            {                
                //禁用提交区域并开启进度
                panel1.Enabled = false;
                panel2.Enabled = true;
                resetloadLocation(ResetB, PBload);
                //开启线程提交数据              
                delegateForThread dft = new delegateForThread(ShowThreadResult_save);
                DataControl.RunThreadClassDataUpdate RTCTSOU = lbNumber.Text == "" ? new DataControl.RunThreadClassDataUpdate(ht_skinfo, dft, "skzh_add") : new DataControl.RunThreadClassDataUpdate(ht_skinfo, dft, "skzh_edit");
                Thread trd = new Thread(new ThreadStart(RTCTSOU.BeginRun));
                trd.IsBackground = true;
                trd.Start();

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
            //重新开放提交区域
            panel1.Enabled = true;
            panel2.Enabled = true;
            PBload.Visible = false;
            //显示执行结果
            ResultInvoke(returnHT);

        }
        //处理返回的结果
        private void ResultInvoke( Hashtable returnHT)
        {
            string state = returnHT["执行状态"].ToString();
            string method = returnHT["服务方法"].ToString();
            string str = "";

            switch (method)
            {
                case "skzh_add":
                    str="添加";
                    break;
                case "skzh_edit":
                    str = "修改";
                    break;
                case "skzh_delete":
                    str = "删除";
                    break;
                case "skzh_set":
                    str = "设置";
                    break;
                default:
                    str="处理";
                    break;
            }

            switch (state)
            {
                case "ok":

                    //给出表单提交成功的提示
                    ArrayList Almsg3 = new ArrayList();
                    Almsg3.Add("");
                    Almsg3.Add("您的收款账号已经"+str+"成功！");
                    FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "叹号", "处理成功", Almsg3);
                    FRSE3.ShowDialog();

                    //重置本表单，或转向其他页面
                    CXshoukuanzhanghu UC = new CXshoukuanzhanghu();//这句话是关键，若实例化本身的类，则是重置表单，若实例化其他的类，则是跳转
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
                    Almsg4.Add(returnHT["执行状态"].ToString());// 这里是提交失败的提示，可能是程序出错，可能是条件不足等
                    FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "错误", str+ "账号失败", Almsg4);
                    FRSE4.ShowDialog();

                    break;
            }
        }

        /// <summary>
        /// 重设进度条位置，放到按钮旁边
        /// </summary>
        /// <param name="BB">参考提交按钮</param>
        /// <param name="PB">要移动的进度条</param>
        private void resetloadLocation(BasicButton BB, PictureBox PB)
        {
            PB.Location = new Point(BB.Location.X + BB.Width + 30, BB.Location.Y+ 60);
            PB.Visible = true;
        }

        /// <summary>
        /// 重置表单
        /// </summary>
        /// <param name="UC"></param>
        private void reset(CXshoukuanzhanghu UC)
        {
            UC.Dock = DockStyle.Fill;//铺满
            UC.AutoScroll = true;//出现滚动条
            UC.BackColor = Color.AliceBlue;
            UC.Padding = new Padding(10);
            //清理控件
            Panel P = (Panel)this.Parent;
            P.Controls.Clear();
            P.Controls.Add(UC); //加入到某一个panel中
            UC.Show();//显示出来

            //更改标题(如果重置本表单，不需要执行这句话)
            //this.ParentForm.Text = "xxx提交成功";
        }

        private void ResetB_Click(object sender, EventArgs e)
        {
            //重置本表单，或转向其他页面
            CXshoukuanzhanghu UC = new CXshoukuanzhanghu();//这句话是关键，若实例化本身的类，则是重置表单，若实例化其他的类，则是跳转
            reset(UC);
        }

        //dataGridView1的相关操作，涉及收款账号的变更。
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView view = (DataGridView)sender;
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                //获取主键
                string num = view.Rows[e.RowIndex].Cells["编号"].Value.ToString();
                string fileName = view.Rows[e.RowIndex].Cells["文件路径"].Value.ToString();
                //点击设置默认收款账号链接
                if (view.Columns[e.ColumnIndex].HeaderText == "默认收款账号")
                {
                    if (view.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "默认收款账号")
                    {
                        ArrayList AlmsgMR = new ArrayList();
                        AlmsgMR.Add("");
                        AlmsgMR.Add("该账号已经是默认收款账号！");
                        FormAlertMessage FRSEMR = new FormAlertMessage("仅确定", "叹号", "提示", AlmsgMR);
                        FRSEMR.ShowDialog();
                    }
                    else
                    {
                        SetAcount(num);
                    }
 
                }
                //点击修改链接
                else if(view.Columns[e.ColumnIndex].HeaderText=="修改")
                {
                    lbNumber.Text = num;
                    ucTextBoxSKR.Text = view.Rows[e.RowIndex].Cells["收款人姓名"].Value.ToString();
                    ucTextBoxKHYH.Text = view.Rows[e.RowIndex].Cells["开户银行名称"].Value.ToString();
                    ucTextBoxYHZK.Text = view.Rows[e.RowIndex].Cells["银行卡号"].Value.ToString();
                    ucTextBoxSFZ.Text = view.Rows[e.RowIndex].Cells["文件路径"].Value.ToString();
                    checkBox1.CheckedChanged -= checkBox1_CheckedChanged;
                    checkBox1.Checked = view.Rows[e.RowIndex].Cells["默认收款账号"].Value.ToString().Trim()=="默认收款账号"?true:false;
                          

                }
                //点击删除链接
                else if(view.Columns[e.ColumnIndex].HeaderText=="删除")
                {
                    //是否要删除收款账号的提醒信息
                    ArrayList Almsgxx = new ArrayList();
                    Almsgxx.Add("");
                    Almsgxx.Add("您确定要删除该收款账号吗？");
                    if (view.Rows[e.RowIndex].Cells["默认收款账号"].Value.ToString() == "默认收款账号")
                    {//默认不能删除  
                        Almsgxx.Add("此为默认账号，删除后请重新设置默认收款账号！");
                    }
                    FormAlertMessage FRSExx = new FormAlertMessage("确定取消", "问号", "删除收款账号", Almsgxx);
                    DialogResult dr = FRSExx.ShowDialog();
                    if (dr == DialogResult.Yes)
                    {
                        DelAcount(num,fileName);
                    }
                    else
                    {
                        return;
                    }

                }

            }
        }

        /// <summary>
        /// 删除收款账号
        /// </summary>
        /// <param name="number"></param>
        private void DelAcount(string number,string name)
        {
            Hashtable ht_skinfo = new Hashtable();
            ht_skinfo["编号"] = number;
            ht_skinfo["买家角色编号"] = JSNM;
            ht_skinfo["文件路径"] = name;
            //禁用提交区域并开启进度
            panel1.Enabled = false;
            panel2.Enabled = false;
            resetloadLocation(ResetB, PBload);
            //开启线程提交数据
            delegateForThread dft = new delegateForThread(ShowThreadResult_save);
            DataControl.RunThreadClassDataUpdate RTCTSOU = new DataControl.RunThreadClassDataUpdate(ht_skinfo, dft, "skzh_delete");
            Thread trd = new Thread(new ThreadStart(RTCTSOU.BeginRun));
            trd.IsBackground = true;
            trd.Start();
        }

        /// <summary>
        /// 设置默认收款账户
        /// </summary>
        /// <param name="number"></param>
        private void SetAcount(string number)
        {
            Hashtable ht_skinfo = new Hashtable();
            ht_skinfo["编号"] = number;
            ht_skinfo["买家角色编号"] = JSNM;
            //禁用提交区域并开启进度
            panel1.Enabled = false;
            panel2.Enabled = true;
            resetloadLocation(ResetB, PBload);
            //开启线程提交数据
            delegateForThread dft = new delegateForThread(ShowThreadResult_save);
            DataControl.RunThreadClassDataUpdate RTCTSOU = new DataControl.RunThreadClassDataUpdate(ht_skinfo, dft, "skzh_set");
            Thread trd = new Thread(new ThreadStart(RTCTSOU.BeginRun));
            trd.IsBackground = true;
            trd.Start();
        }        

        /// <summary>
        /// 若全部上传完成，在主窗体进行数据处理，根据情况编写，没有处理也要带着这个方法
        /// </summary>
        /// <param name="LV">上传结果集合</param>
        private void UpLoadSucceed(ListView LV)
        {
            //这里的LV,实际上是打开上传时指定的那个隐藏控件listView1，所以这个方法在多个上传按钮时，只需要写一次就行
            //MessageBox.Show("回调测试" + LV.Name);
            this.ucTextBoxSFZ.Text = LV.Items[LV.Items.Count-1].SubItems[1].Text;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == false)
            {
                if (ucTextBoxKHYH.Text.Trim() != "" && ucTextBoxSFZ.Text.Trim() != "" && ucTextBoxYHZK.Text.Trim() != "" && ucTextBoxSKR.Text.Trim() != "")
                {
                    ArrayList AlmsgMR = new ArrayList();
                    AlmsgMR.Add("");
                    AlmsgMR.Add("请确认已经设置了默认收款账号！");
                    FormAlertMessage FRSEMR = new FormAlertMessage("仅确定", "叹号", "提醒", AlmsgMR);
                    FRSEMR.ShowDialog();
                }

            }
        }

        

        //上传身份证扫描件
        private void B_SC_Click(object sender, EventArgs e)
        {
            //删除已经无效的图片
            if (ucTextBoxSFZ.Text.Trim() != "")
            {
                try
                {
                    DataControl.WebServicesCenter WSC = new DataControl.WebServicesCenter();
                    WSC.DelPicture(ucTextBoxSFZ.Text.Trim().Replace(@"\", "/"));}
                catch 
                {
                    ArrayList AlmsgMR = new ArrayList();
                    AlmsgMR.Add("");
                    AlmsgMR.Add("请检查网络，稍后再试！");
                    FormAlertMessage FRSEMR = new FormAlertMessage("仅确定", "叹号", "提醒", AlmsgMR);
                    FRSEMR.ShowDialog();
                }
                
            }

            //开启上传
            openFileDialog1.ShowDialog();
           
        }

        //浏览身份证扫描件
        private void B_SCCK_Click(object sender, EventArgs e)
        {
            //有地址
            if (ucTextBoxSFZ.Text.Trim() != "")
            {
                string url = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[1] + "/JHJXPT/SaveDir/" + ucTextBoxSFZ.Text.Trim().Replace(@"\", "/");
                //MessageBox.Show(url);
                StringOP.OpenUrl(url);

                //FormWebBrowser fwb = new FormWebBrowser();
                //fwb.webBrowser1.Url = new Uri(url);

                //fwb.ShowDialog();
            }
        }

        //删除身份证扫描件
        private void B_SCSC_Click(object sender, EventArgs e)
        {

            ArrayList Almsgxx = new ArrayList();
            Almsgxx.Add("");
            Almsgxx.Add("您确定要删除已经上传的身份证扫描件吗？");           
            FormAlertMessage FRSExx = new FormAlertMessage("确定取消", "问号", "删除上传文件", Almsgxx);
            DialogResult dr = FRSExx.ShowDialog();
            if (dr == DialogResult.Yes)
            {
                //删除已经无效的图片
                if (ucTextBoxSFZ.Text.Trim() != "")
                {
                    try
                    {
                        DataControl.WebServicesCenter WSC = new DataControl.WebServicesCenter();
                        WSC.DelPicture(ucTextBoxSFZ.Text.Trim().Replace(@"\", "/"));
                    }
                    catch
                    {
                        ArrayList AlmsgMR = new ArrayList();
                        AlmsgMR.Add("");
                        AlmsgMR.Add("请检查网络，稍后再试！");
                        FormAlertMessage FRSEMR = new FormAlertMessage("仅确定", "叹号", "提醒", AlmsgMR);
                        FRSEMR.ShowDialog();
                    }

                    ucTextBoxSFZ.Text = "";
                }
                listView1.Items.Clear();
                
            }
           
        }
        // 根据上传数据处理查看和删除按钮
        private void timer_SCCK_Tick(object sender, EventArgs e)
        {
            if (listView1.Items.Count > 0 || ucTextBoxSFZ.Text.Trim()!="")
            {
                B_SCCK.Visible = true;
                B_SCSC.Visible = true;
            }
            else
            {
                B_SCCK.Visible = false;
                B_SCSC.Visible = false;
            }
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            bool ISOK = true;
            string[] fName = openFileDialog1.FileNames;
            //this.ucTextBoxSFZ.Text = openFileDialog1.FileName;
            //若选择选择对话框允许选择多个，则还要检验不能超过5个
            //多不是对话框选择的，是直接指定数组，则还要检验不能重名

            foreach (var file in fName)
            {
                System.IO.FileInfo fi = new System.IO.FileInfo(file);
                if (fi.Length > 500 * 1024)
                {
                    ArrayList Almsg4 = new ArrayList();
                    Almsg4.Add("");
                    Almsg4.Add("文件大小不可超过500k！");// 这里是提交失败的提示，可能是程序出错，可能是条件不足等
                    FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "错误", "提示", Almsg4);
                    FRSE4.ShowDialog();

                    ISOK = false;
                    break;
                }
            }
            if (ISOK)
            {
                //开始上传,传入文件名数组，传入对应的上传结果控件，传入回调，传入角色编号
                FormSC FSC = new FormSC(fName, listView1, new delegateForSC(UpLoadSucceed), JSNM);
                FSC.ShowDialog();

            }

        }

    }
}
