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

namespace 客户端主程序.SubForm.CenterForm.Agent
{
    public partial class UserListLook : UserControl
    {
        /// <summary>
        /// 查询条件
        /// </summary>
        Hashtable ht_where = new Hashtable();

        /// <summary>
        /// 用户资料审核详情页面
        /// </summary>
        FormYHZLXQ formYHZLSH = null;

        public UserListLook()
        {
            InitializeComponent();

            //初始化分页回调(带分页的页面，先把这个放上)
            ucPager1.DFT = new delegateForThread(ShowThreadResult_BeginBind);
            //处理下拉框间距
            this.CBzcjs.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.CB_DrawItem);
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
            CBzcjs.SelectedIndex = 0;
            GetData(null);

            //开启线程提交数据
            delegateForThread dft = new delegateForThread(ShowThreadResult_RE);
            Hashtable ht = new Hashtable();
            ht["经纪人角色编号"] = PublicDS.PublisDsUser.Tables[0].Rows[0]["经纪人角色编号"].ToString().Trim();
            ht["经纪人邮箱"] = PublicDS.PublisDsUser.Tables[0].Rows[0]["DLYX"].ToString().Trim();
            ht["操作类型"] = "仅状态";
            ht["被操作人邮箱"] = "";
            DataControl.RunThreadClassJJRmanger RTCJJR = new DataControl.RunThreadClassJJRmanger(ht, dft);
            Thread trd = new Thread(new ThreadStart(RTCJJR.runThreading));
            trd.IsBackground = true;
            trd.Start();


            

        }

        //显示线程处理结果的函数(用于处理线程返回数据)
        private void ShowThreadResult_RE(Hashtable returnHT)
        {
            try
            {
                //调用异步委托
                Invoke(new delegateForThreadShow(ShowThreadResult_RE_Invoke), new Hashtable[] { returnHT });
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("委托回调错误：" + ex.ToString());
            }
        }
        //处理非线程创建的控件
        private void ShowThreadResult_RE_Invoke(Hashtable returnHT)
        {

            //显示执行结果
            string[] zt = (string[])returnHT["执行结果"];

            if (zt[2] == "仅状态" && zt[0] == "ok")
            {
                //处理暂停代理新用户
                if (zt[1] == "否")
                {
                    XYHSH.Text = "暂停新用户的审核";
                    XYHSH.Visible = true;
                }
                else
                {
                    XYHSH.Text = "恢复新用户的审核";
                    XYHSH.Visible = true;
                }
            }
            if (zt[2] == "新注册" && zt[0] == "ok")
            {
                XYHSH.Enabled = true;
                PBload.Visible = false;

                if (zt[3] == "否")
                {
                    XYHSH.Text = "暂停新用户的审核";

                }
                else
                {
                    XYHSH.Text = "恢复新用户的审核";

                }

                ArrayList Almsg3 = new ArrayList();
                Almsg3.Add("");
                Almsg3.Add(zt[1]);
                FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "叹号", "xxxx", Almsg3);
                FRSE3.ShowDialog();

                
            }


            if (zt[2] == "新业务暂停" && zt[0] == "ok")
            {
                Lselect.Enabled = true;
                PBload.Visible = false;

                ArrayList Almsg3 = new ArrayList();
                Almsg3.Add("");
                Almsg3.Add(zt[1]);
                FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "叹号", "xxxx", Almsg3);
                FRSE3.ShowDialog();

                GetData(null);

            }
            if (zt[2] == "新业务恢复" && zt[0] == "ok")
            {
                label4.Enabled = true;
                PBload.Visible = false;

                ArrayList Almsg3 = new ArrayList();
                Almsg3.Add("");
                Almsg3.Add(zt[1]);
                FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "叹号", "xxxx", Almsg3);
                FRSE3.ShowDialog();

                GetData(null);
            }
            


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
            string JJRjsbh = PublicDS.PublisDsUser.Tables[0].Rows[0]["经纪人角色编号"].ToString().Trim();
            string dlyx = PublicDS.PublisDsUser.Tables[0].Rows[0]["DLYX"].ToString().Trim();
            ht_where["GetCustomersDataPage_NAME"] = "GetCustomersDataPage";  //每页显示的条数(必须设置)
            ht_where["page_index"] = "0";
            ht_where["page_size"] = "25";  //每页显示的条数(必须设置)
            ht_where["serach_Row_str"] = " * "; //检索字段(必须设置)
            ht_where["search_tbname"] = "  ( select '买家角色编号'=(SELECT JSBB.JSBH FROM ZZ_YHJSXXB as JSBB where JSBB.DLYX = UUU.DLYX and JSBB.JSLX='买家'),'卖家角色编号'=(SELECT JSBB.JSBH FROM ZZ_YHJSXXB as JSBB where JSBB.DLYX = UUU.DLYX and JSBB.JSLX='卖家'),'选中状态'='否','注册角色'=UUU.JSZHLX,'用户名'=YHM,'注册邮箱'=DLYX,'联系人姓名'=(select top 1 XINXI.LXRXM from ZZ_BUYJBZLXXB as XINXI where XINXI.DLYX = UUU.DLYX),'联系人手机号'=(select top 1 XINXI.SJH from ZZ_BUYJBZLXXB as XINXI where XINXI.DLYX = UUU.DLYX),'注册时间'=ZCSJ,'是否休眠'=(select DL.SFXM from ZZ_UserLogin DL where DL.DLYX = UUU.DLYX),'是否暂停新业务'=(select top 1 GLB.SFZTXYW  from ZZ_MMJYDLRZHGLB as GLB where GLB.JSDLZH = UUU.DLYX),* from ZZ_UserLogin as UUU where  UUU.DLYX in (select distinct JSDLZH from ZZ_MMJYDLRZHGLB where  DLRBH='" + JJRjsbh + "' and JSDLZH <> '" + dlyx + "') and isnull((select count(Number) from ZZ_YHJSXXB as SH where SH.DLYX = UUU.DLYX and SH.KTSHZT ='审核通过' and SH.FGSKTSHZT='审核通过'),0)>0 ) as tab  ";  //检索的表(必须设置)
            ht_where["search_mainid"] = " 注册邮箱 ";  //所检索表的主键(必须设置)
            ht_where["search_str_where"] = " 1=1 ";  //检索条件(必须设置)
            ht_where["search_paixu"] = " DESC ";  //排序方式(必须设置)
            ht_where["search_paixuZD"] = " 注册时间 ";  //用于排序的字段(必须设置)
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








        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;

            if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
                //设置默认收货地址的功能
                if (dgv.Columns[e.ColumnIndex].HeaderText == "查看详情")
                {
                    //获取当前点击行的number值
                    string strJS = dgv.Rows[e.RowIndex].Cells["注册角色"].Value.ToString(); //注册角色
                    string strRoleType = ""; //角色类型
                    string strRoleNumber = "";
                    string strRoleEmail = ""; //登陆邮箱
                    if (strJS == "买家账户")
                    {
                        strRoleType = "买家"; //角色类型
                        strRoleNumber = dgv.Rows[e.RowIndex].Cells["买家角色编号"].Value.ToString(); //角色编号
                        strRoleEmail = dgv.Rows[e.RowIndex].Cells["注册邮箱"].Value.ToString(); //登陆邮箱
                    }
                    if (strJS == "卖家账户")
                    {
                        strRoleType = "卖家"; //角色类型
                        strRoleNumber = dgv.Rows[e.RowIndex].Cells["卖家角色编号"].Value.ToString(); //角色编号
                        strRoleEmail = dgv.Rows[e.RowIndex].Cells["注册邮箱"].Value.ToString(); //登陆邮箱
                    }
                    
                    
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
        //停止或开启审核
        private void label5_Click(object sender, EventArgs e)
        {
            ArrayList Almsg3 = new ArrayList();
            Almsg3.Add("");
            Almsg3.Add("您确定要" + XYHSH.Text + "吗？");
            FormAlertMessage FRSE3 = new FormAlertMessage("确定取消", "问号", "xxx", Almsg3);
            DialogResult dr = FRSE3.ShowDialog();
            if (dr != DialogResult.Yes)
            {
                return;
            }

            XYHSH.Enabled = false;
            PBload.Visible = true;
            
            //更新或恢复角色表中的状态即可
            //开启线程提交数据
            delegateForThread dft = new delegateForThread(ShowThreadResult_RE);
            Hashtable ht = new Hashtable();
            ht["经纪人角色编号"] = PublicDS.PublisDsUser.Tables[0].Rows[0]["经纪人角色编号"].ToString().Trim();
            ht["经纪人邮箱"] = PublicDS.PublisDsUser.Tables[0].Rows[0]["DLYX"].ToString().Trim();
            ht["操作类型"] = "新注册";
            ht["被操作人邮箱"] = "";
            DataControl.RunThreadClassJJRmanger RTCJJR = new DataControl.RunThreadClassJJRmanger(ht, dft);
            Thread trd = new Thread(new ThreadStart(RTCJJR.runThreading));
            trd.IsBackground = true;
            trd.Start();
        }

        //暂停选中用户的新业务
        private void Lselect_Click(object sender, EventArgs e)
        {
            //更新状态即可

            //获取要操作的邮箱
            string BeSetDLYX = "'十全大补丸'";
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if ((bool)(dataGridView1.Rows[i].Cells[0].EditedFormattedValue))
                {
                    BeSetDLYX = BeSetDLYX + ",'" + dataGridView1.Rows[i].Cells["注册邮箱"].Value.ToString() + "'";
                }
            }
            if (BeSetDLYX == "'十全大补丸'")
            {
                return;
            }

            ArrayList Almsg3 = new ArrayList();
            Almsg3.Add("");
            Almsg3.Add("暂停后，任何已关联用户将无法在您的名下进行新业务！您确定要" + Lselect.Text + "吗？");
            FormAlertMessage FRSE3 = new FormAlertMessage("确定取消", "问号", "xxx", Almsg3);
            DialogResult dr = FRSE3.ShowDialog();
            if (dr != DialogResult.Yes)
            {
                return;
            }

            Lselect.Enabled = false;
            PBload.Visible = true;

            //开启线程提交数据
            delegateForThread dft = new delegateForThread(ShowThreadResult_RE);
            Hashtable ht = new Hashtable();
            ht["经纪人角色编号"] = PublicDS.PublisDsUser.Tables[0].Rows[0]["经纪人角色编号"].ToString().Trim();
            ht["经纪人邮箱"] = PublicDS.PublisDsUser.Tables[0].Rows[0]["DLYX"].ToString().Trim();
            ht["操作类型"] = "新业务暂停";
            ht["被操作人邮箱"] = BeSetDLYX;
            DataControl.RunThreadClassJJRmanger RTCJJR = new DataControl.RunThreadClassJJRmanger(ht, dft);
            Thread trd = new Thread(new ThreadStart(RTCJJR.runThreading));
            trd.IsBackground = true;
            trd.Start();
        }
        //恢复选中用户的新业务
        private void label4_Click(object sender, EventArgs e)
        {
            //获取要操作的邮箱
            string BeSetDLYX = "'十全大补丸'";
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if ((bool)(dataGridView1.Rows[i].Cells[0].EditedFormattedValue))
                {
                    BeSetDLYX = BeSetDLYX + ",'" + dataGridView1.Rows[i].Cells["注册邮箱"].Value.ToString() + "'";
                }
            }
            if (BeSetDLYX == "'十全大补丸'")
            {
                return;
            }


            //更新状态即可
            ArrayList Almsg3 = new ArrayList();
            Almsg3.Add("");
            Almsg3.Add("恢复后，已关联用户可以继续在您的名下进行新业务！您确定要" + label4.Text + "吗？");
            FormAlertMessage FRSE3 = new FormAlertMessage("确定取消", "问号", "xxx", Almsg3);
            DialogResult dr = FRSE3.ShowDialog();
            if (dr != DialogResult.Yes)
            {
                return;
            }


            label4.Enabled = false;
            PBload.Visible = true;

            //开启线程提交数据
            delegateForThread dft = new delegateForThread(ShowThreadResult_RE);
            Hashtable ht = new Hashtable();
            ht["经纪人角色编号"] = PublicDS.PublisDsUser.Tables[0].Rows[0]["经纪人角色编号"].ToString().Trim();
            ht["经纪人邮箱"] = PublicDS.PublisDsUser.Tables[0].Rows[0]["DLYX"].ToString().Trim();
            ht["操作类型"] = "新业务恢复";
            ht["被操作人邮箱"] = BeSetDLYX;
            DataControl.RunThreadClassJJRmanger RTCJJR = new DataControl.RunThreadClassJJRmanger(ht, dft);
            Thread trd = new Thread(new ThreadStart(RTCJJR.runThreading));
            trd.IsBackground = true;
            trd.Start();
        }

        private void basicButton1_Click(object sender, EventArgs e)
        {
            ////获取搜索条件
            string key = TBkey.Text.Trim();
            string htzq = CBzcjs.Text;
            string begintime = dateTimePickerBegin.Value.ToShortDateString() + " 00:00:01";
            string endtime = dateTimePickerEnd.Value.ToShortDateString() + " 23:59:59";
            //更改搜索条件
            setDefaultSearch();
            ht_where["search_str_where"] = ht_where["search_str_where"] + " and 注册时间 > '" + begintime + "' and 注册时间 < '" + endtime + "' ";  //检索条件(必须设置)
            if (htzq != "不限注册角色")
            {
                ht_where["search_str_where"] = ht_where["search_str_where"] + " and 注册角色='" + htzq + "' ";
            }
            if (key != "用户名、注册邮箱")
            {
                ht_where["search_str_where"] = ht_where["search_str_where"] + " and (注册邮箱 like '%" + key + "%' or 用户名 like '%" + key + "%' ) ";
            }
            
            //执行查询
            GetData(ht_where);
        }

     

    }
}
