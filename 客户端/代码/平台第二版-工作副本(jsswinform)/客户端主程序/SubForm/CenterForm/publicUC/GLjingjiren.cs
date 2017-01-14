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
using 客户端主程序.DataControl;
using Com.Seezt.Skins;

namespace 客户端主程序.SubForm.CenterForm.publicUC
{
    public partial class GLjingjiren : UserControl
    {
        Hashtable ht_where = new Hashtable();
        DataSet ds = null;
        string JSNM = "";
        string JSZHLX = "";
        string BuyBH = "";
        public GLjingjiren()
        {
            InitializeComponent();
            //获取角色类型
             JSZHLX = PublicDS.PublisDsUser.Tables[0].Rows[0]["JSZHLX"].ToString();

            //根据类型获取编号
            if (JSZHLX == "卖家账户")
            {
                JSNM = PublicDS.PublisDsUser.Tables[0].Rows[0]["卖家角色编号"].ToString().Trim();
                BuyBH = PublicDS.PublisDsUser.Tables[0].Rows[0]["买家角色编号"].ToString().Trim();
            }
            if (JSZHLX == "买家账户")
            {
                JSNM = PublicDS.PublisDsUser.Tables[0].Rows[0]["买家角色编号"].ToString().Trim();
                BuyBH = PublicDS.PublisDsUser.Tables[0].Rows[0]["买家角色编号"].ToString().Trim();
            }
            if (JSZHLX == "经纪人账户")
            {
                JSNM = PublicDS.PublisDsUser.Tables[0].Rows[0]["经纪人角色编号"].ToString().Trim();
                BuyBH = PublicDS.PublisDsUser.Tables[0].Rows[0]["买家角色编号"].ToString().Trim();
            }

            //ucTextBox5.Text = "请输入准确的经纪人邮箱或用户名";
            // this.ucTextBox5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            //this.ucTextBox5.ForeColor = System.Drawing.SystemColors.GrayText;
            
            bgWorker.RunWorkerAsync();//打开异步操作，完成数据的读取
            
            ////最可能的匹配项自动追加到当前数据并产生由一个或多个建议完成字符串组成的下拉列表
            //ucTextBox5.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            ////设置智能提示的源为自定义源
            //ucTextBox5.AutoCompleteSource = AutoCompleteSource.CustomSource;


            //初始化分页回调(带分页的页面，先把这个放上)
            ucPager1.DFT = new delegateForThread(ShowThreadResult_BeginBind);
        }

        //获取智能提示数据的方法
        AutoCompleteStringCollection GetDataFromDB()
        {
            AutoCompleteStringCollection ac = new AutoCompleteStringCollection();
            //
            DataControl.WebServicesCenter WSC = new DataControl.WebServicesCenter();
            string where = "select [DLYX] as text from [ZZ_DLRJBZLXXB]  union  select [YHM] as text from [ZZ_DLRJBZLXXB]";
            try
            {   
                DataSet dstest = WSC.GetDataSet(where);
                if (dstest != null && dstest.Tables[0].Rows.Count > 0)
                { 
                    for (int i = 0; i < dstest.Tables[0].Rows.Count; i++)
                    {
                        ac.Add(dstest.Tables[0].Rows[i]["text"].ToString());
                    } 
                }

            }
            catch
            {
                ArrayList AlmsgMR = new ArrayList();
                AlmsgMR.Add("");
                AlmsgMR.Add("请检查网络，稍后再试！");
                FormAlertMessage FRSEMR = new FormAlertMessage("仅确定", "叹号", "提醒", AlmsgMR);
                FRSEMR.ShowDialog(); 
            }
         
           
            return ac;
        }
        //后台获取智能提示数据
        private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = GetDataFromDB();
        }

        //文本框智能数据源赋值
        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ucTextBox5.AutoCompleteCustomSource = (AutoCompleteStringCollection)e.Result;
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
            ht_where["serach_Row_str"] = "  联系人姓名, 手机号码, 编号 , 角色登陆账号,角色账户类型, 经纪人邮箱, 经纪人用户名, 默认关联经纪人, 申请关联时间, 审核状态, 是否接收新用户 "; //检索字段(必须设置)
            //ht_where["search_tbname"] = " (SELECT  [LXRXM] as 联系人姓名,[SJH] as 手机号码,   [ZZ_MMJYDLRZHGLB].[Number]as 编号 ,[JSDLZH] as 角色登陆账号,[ZZ_MMJYDLRZHGLB].[JSZHLX] as 角色账户类型,[DLRDLZH]as 经纪人邮箱,[DLRYHM] as 经纪人用户名,CASE WHEN [SFMRDLR]='是' THEN '默认经纪人' WHEN [SFMRDLR]='否' THEN '设为默认经纪人' END as 默认关联经纪人,[SQGLSJ] as 申请关联时间,case when [DLRSHZT]='已审核' and FGSSHZT='已审核' then '已审核' when [DLRSHZT] !='已审核' OR FGSSHZT !='已审核' then '待审核' end as 审核状态,CASE WHEN [SFZTXYW]='是' THEN '否' WHEN [SFZTXYW]='否' THEN '是' END as 是否接新业务 FROM  [ZZ_DLRJBZLXXB] JOIN [ZZ_MMJYDLRZHGLB] ON [DLRDLZH]=[DLYX])  tab  ";  //检索的表(必须设置)
            ht_where["search_tbname"] = " ( SELECT  [LXRXM] as 联系人姓名,[SJH] as 手机号码,   [ZZ_MMJYDLRZHGLB].[Number]as 编号 ,[JSDLZH] as 角色登陆账号,[ZZ_MMJYDLRZHGLB].[JSZHLX] as 角色账户类型,ZZ_MMJYDLRZHGLB.JSLX 角色类型,[DLRDLZH]as 经纪人邮箱,[DLRYHM] as 经纪人用户名,DLRBH as 经纪人编号,CASE WHEN [SFMRDLR]='是' THEN '默认经纪人' WHEN [SFMRDLR]='否' THEN '设为默认经纪人' END as 默认关联经纪人,[SQGLSJ] as 申请关联时间,case when [DLRSHZT] ='驳回' then '驳回' when FGSSHZT ='待审核' then '待审核' when [DLRSHZT]='已审核' and FGSSHZT='已审核' then '已审核'  end as 审核状态,CASE WHEN DLRZTJSXYH='是' THEN '否' WHEN DLRZTJSXYH='否' THEN '是' END as 是否接收新用户 FROM  [ZZ_DLRJBZLXXB] JOIN [ZZ_MMJYDLRZHGLB] ON [DLRDLZH]=[DLYX] join ZZ_YHJSXXB on [ZZ_MMJYDLRZHGLB].DLRBH=ZZ_YHJSXXB.JSBH  )  tab  ";  //检索的表(必须设置)
            ht_where["search_mainid"] = " 编号 ";  //所检索表的主键(必须设置)
            ht_where["search_str_where"] = " 1=1 ";  //检索条件(必须设置)
            ht_where["search_paixu"] = " DESC ";  //排序方式(必须设置)
            ht_where["search_paixuZD"] = "默认关联经纪人, 编号 ";  //用于排序的字段(必须设置)
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
            //if (JSZHLX == "经纪人账户")
            //{
            //    ht_where["search_str_where"] = ht_where["search_str_where"] + " and 角色登陆账号='" + PublicDS.PublisDsUser.Tables[0].Rows[0]["DLYX"].ToString() + "'";
            //}
            //else
            //{
            //    ht_where["search_str_where"] = ht_where["search_str_where"] + " and 角色登陆账号='" + PublicDS.PublisDsUser.Tables[0].Rows[0]["DLYX"].ToString() + "'and 角色类型='" + JSZHLX + "'";
            //}        
            ht_where["search_str_where"] = ht_where["search_str_where"] + " and 角色登陆账号='" + PublicDS.PublisDsUser.Tables[0].Rows[0]["DLYX"].ToString() + "'and 角色类型='买家'";
            ucPager1.HT_Where = ht_where;
            ucPager1.BeginBindForUCPager();
        }
        
        private void Lselect_Click(object sender, EventArgs e)
        {
            Hashtable info = new Hashtable();
            //info["where"] = "SELECT [Number],[DLYX],[YHM],[JSBH],[LXRXM],[SJH],[SFYZSJH],[SJHYZM],[SSSF],[SSDS],[SSQX],[SSFGS],[XXDZ],[YZBM],[SFZH],[ZCLX],[SFZSMJ],[GSMC],[GSDH],[GSDZ],[YYZZSMJ],[ZZJGDMZSMJ],[SWDJZSMJ],[KHXKZSMJ],[FDDBRQZDCNSSMJ],[YXKPLX],[YXKPXX],[CreateUser],[CreateTime] FROM [ZZ_DLRJBZLXXB] WHERE [DLYX]='"+ucTextBox5.Text.Trim()+"' OR [YHM]='"+ucTextBox5.Text.Trim()+"' ";
            info["where"] = " SELECT a.[Number],a.[DLYX],a.[YHM],a.[JSBH],a.[LXRXM],a.[SJH],a.[SFYZSJH],a.[SJHYZM],a.[SSSF],a.[SSDS],a.[SSQX],a.[SSFGS],a.[XXDZ],a.[YZBM],a.[SFZH],a.[ZCLX],a.[SFZSMJ],a.[GSMC],a.[GSDH],a.[GSDZ],a.[YYZZSMJ],a.[ZZJGDMZSMJ],a.[SWDJZSMJ],a.[KHXKZSMJ],a.[FDDBRQZDCNSSMJ],a.[YXKPLX],a.[YXKPXX],a.[CreateUser],a.[CreateTime],b.SFYXDL 是否允许登陆,b.SFDJZH 是否冻结账号,b.SFXM 是否休眠,c.DLRZTJSXYH 代理人暂停接受新用户,c.FGSKTSHZT 分公司开通审核状态,c.KTSHZT 经纪人开通审核状态 FROM [ZZ_DLRJBZLXXB] a inner join ZZ_UserLogin b on a.[DLYX]=b.[DLYX] inner join ZZ_YHJSXXB c on a.DLYX=c.DLYX and c.JSBH=a.JSBH  WHERE a.[DLYX]='" + ucTextBox5.Text.Trim() + "' OR a.[YHM]='" + ucTextBox5.Text.Trim() + "'";

            //禁用提交区域并开启进度
            panel1.Enabled = false;
            panel2.Enabled = true;
            //resetloadLocation(ResetB, PBload);
            //开启线程提交数据              
            delegateForThread dft = new delegateForThread(ShowThreadResult_get);
            DataControl.RunThreadClassDataGet RTCTSOU = new DataControl.RunThreadClassDataGet(info,dft);
            Thread trd = new Thread(new ThreadStart(RTCTSOU.BeginRun));
            trd.IsBackground = true;
            trd.Start();

        }

        //显示线程处理结果的函数(用于处理线程返回数据)
        private void ShowThreadResult_get(Hashtable returnHT)
        {
            try
            {
                //调用异步委托
                Invoke(new delegateForThreadShow(ShowThreadResult_get_Invoke), new Hashtable[] { returnHT });
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("委托回调错误：" + ex.ToString());
            }
        }
        //处理非线程创建的控件
        private void ShowThreadResult_get_Invoke(Hashtable returnHT)
        {
            //重新开放提交区域
            panel1.Enabled = true;
            panel2.Enabled = true;
            PBload.Visible = false;
            //显示执行结果
            ds=(DataSet)returnHT["返回数据"];
            if(ds!=null && ds.Tables[0].Rows.Count>1)
            {
                //提示
                ArrayList Almsg1 = new ArrayList();
                Almsg1.Add("");
                Almsg1.Add("检测到多个同名的经纪人，请输入准确的经纪人邮箱或用户名！");
                FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "叹号", "提示", Almsg1);
                FRSE3.ShowDialog();
                ucTextBox5.Text = "请输入准确的经纪人邮箱或用户名";
                //this.ucTextBox5.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                this.ucTextBox5.ForeColor = System.Drawing.SystemColors.GrayText;
            }
            else if (ds != null && ds.Tables[0].Rows.Count ==1)           
            {
                if (ds.Tables[0].Rows[0]["分公司开通审核状态"].ToString().Trim() != "审核通过")
                {
                    ArrayList Almsg1 = new ArrayList();
                    Almsg1.Add("");
                    Almsg1.Add("该经纪人尚未开通结算账户，请输入其他经纪人！");
                    FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "叹号", "提示", Almsg1);
                    FRSE3.ShowDialog();
                    ucTextBox5.Text = "请输入准确的经纪人邮箱或用户名";
                    // this.ucTextBox5.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                    this.ucTextBox5.ForeColor = System.Drawing.SystemColors.GrayText;
                    return;
                }
                if (ds.Tables[0].Rows[0]["是否允许登陆"].ToString().Trim() == "否" || ds.Tables[0].Rows[0]["是否冻结账号"].ToString().Trim() == "是" || ds.Tables[0].Rows[0]["是否休眠"].ToString().Trim() == "是" || ds.Tables[0].Rows[0]["代理人暂停接受新用户"].ToString().Trim() == "是")
                {
                    ArrayList Almsg1 = new ArrayList();
                    Almsg1.Add("");
                    Almsg1.Add("该经纪人不可用，请输入其他经纪人！");
                    FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "叹号", "提示", Almsg1);
                    FRSE3.ShowDialog();
                    ucTextBox5.Text = "请输入准确的经纪人邮箱或用户名";                    
                   // this.ucTextBox5.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                    this.ucTextBox5.ForeColor = System.Drawing.SystemColors.GrayText;
                    return;
                }
                if ( ds.Tables[0].Rows[0]["ZCLX"].ToString().Trim() == "企业注册")
                {
                    lb1.Text = "经纪人用户名：" + ds.Tables[0].Rows[0]["YHM"].ToString().Trim();
                    lb2.Text = "邮箱：" + ds.Tables[0].Rows[0]["DLYX"].ToString().Trim();
                    lb3.Text = "公司名称：" + ds.Tables[0].Rows[0]["GSMC"].ToString().Trim();
                    lb4.Text = "公司电话：" + ds.Tables[0].Rows[0]["GSDH"].ToString().Trim();
                    lb5.Text = "公司地址：" + ds.Tables[0].Rows[0]["GSDZ"].ToString().Trim();
                    lb6.Text = "联系人姓名：" + ds.Tables[0].Rows[0]["LXRXM"].ToString().Trim();
                    lb7.Text = "手机号码：" + ds.Tables[0].Rows[0]["SJH"].ToString().Trim();

                }
                else if (ds.Tables[0].Rows[0]["ZCLX"].ToString().Trim() == "个人注册")
                {
                    lb1.Text = "经纪人用户名：" + ds.Tables[0].Rows[0]["YHM"].ToString().Trim();
                    lb2.Text = "邮箱：" + ds.Tables[0].Rows[0]["DLYX"].ToString().Trim();
                    lb3.Text = "联系人姓名：" + ds.Tables[0].Rows[0]["LXRXM"].ToString().Trim();
                    lb4.Text = "手机号码：" + ds.Tables[0].Rows[0]["SJH"].ToString().Trim();
                    lb5.Text = "";
                    lb6.Text = "";
                    lb7.Text = "";
                }
                
            }
            else if (ds == null || ds.Tables[0].Rows.Count == 0)
            {
                ArrayList Almsg1 = new ArrayList();
                Almsg1.Add("");
                Almsg1.Add("未找到相关的经纪人，请输入准确的经纪人邮箱或用户名！");
                FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "叹号", "提示", Almsg1);
                FRSE3.ShowDialog();
                ucTextBox5.Text = "请输入准确的经纪人邮箱或用户名";
                //this.ucTextBox5.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                this.ucTextBox5.ForeColor = System.Drawing.SystemColors.GrayText;
            }


        }

        private void btnTijiao_Click(object sender, EventArgs e)
        {
            if (ds != null && ds.Tables[0].Rows.Count == 1)
            {
                DataSet dsZB = new DataSet();

                DataTable auto2 = new DataTable();
                auto2.TableName = "子表";
                auto2.Columns.Add("JSBH");
                auto2.Columns.Add("JSDLZH");
                auto2.Columns.Add("DLRDLZH");
                auto2.Columns.Add("DLRBH");
                auto2.Columns.Add("DLRYHM");
                auto2.Columns.Add("SFMRDLR");
                dsZB.Tables.Add(auto2);
                dsZB.Tables[0].Rows.Add(new string[] { BuyBH, PublicDS.PublisDsUser.Tables[0].Rows[0]["DLYX"].ToString(), ds.Tables[0].Rows[0]["DLYX"].ToString().Trim(), ds.Tables[0].Rows[0]["JSBH"].ToString().Trim(), ds.Tables[0].Rows[0]["YHM"].ToString().Trim(), "否" });
                if (JSZHLX == "卖家账户")
                {
                    dsZB.Tables[0].Rows.Add(new string[] { JSNM, PublicDS.PublisDsUser.Tables[0].Rows[0]["DLYX"].ToString(), ds.Tables[0].Rows[0]["DLYX"].ToString().Trim(), ds.Tables[0].Rows[0]["JSBH"].ToString().Trim(), ds.Tables[0].Rows[0]["YHM"].ToString().Trim(), "否" });
                }
                //禁用提交区域并开启进度
                panel1.Enabled = false;
                panel2.Enabled = false;
                resetloadLocation(ResetB, PBload);
                //开启线程提交数据              
                delegateForThread dft = new delegateForThread(ShowThreadResult_save);
                DataControl.RunThreadClassUpdateJJR RTCTSOU = new DataControl.RunThreadClassUpdateJJR(dsZB, dft,"jjr_save");
                Thread trd = new Thread(new ThreadStart(RTCTSOU.BeginRun));
                trd.IsBackground = true;
                trd.Start();
            }
            else
            {
                ArrayList Almsg3 = new ArrayList();
                Almsg3.Add("");
                Almsg3.Add("请先确定一个经纪人！");
                FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "错误", "提示", Almsg3);
                FRSE3.ShowDialog();
                return;
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
        private void ResultInvoke(Hashtable returnHT)
        {
            DataSet state = (DataSet)returnHT["执行状态"];
            switch (state.Tables["返回值单条"].Rows[0]["执行结果"].ToString())
            {
                case "ok":

                    //给出表单提交成功的提示
                    ArrayList Almsg3 = new ArrayList();
                    Almsg3.Add("");
                    Almsg3.Add(state.Tables["返回值单条"].Rows[0]["错误提示"].ToString());
                    FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "叹号", "处理成功", Almsg3);
                    FRSE3.ShowDialog();

                    //重置本表单，或转向其他页面
                    GLjingjiren UC = new GLjingjiren();//这句话是关键，若实例化本身的类，则是重置表单，若实例化其他的类，则是跳转
                    reset(UC);

                    break;
                case "err":
                    ArrayList Almsg7 = new ArrayList();
                    Almsg7.Add("");
                    Almsg7.Add(state.Tables["返回值单条"].Rows[0]["错误提示"].ToString());
                    FormAlertMessage FRSE7 = new FormAlertMessage("仅确定", "错误", "添加或设置未成功", Almsg7);
                    FRSE7.ShowDialog();
                    break;
                case "success":
                    break;
                default:
                    ArrayList Almsg4 = new ArrayList();
                    Almsg4.Add("");
                    Almsg4.Add("抱歉，系统繁忙，请稍后再试……");// 这里是提交失败的提示，可能是程序出错，可能是条件不足等
                    FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "错误", "系统忙", Almsg4);
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
            PB.Location = new Point(BB.Location.X + BB.Width + 30, BB.Location.Y );
            PB.Visible = true;
        }

        /// <summary>
        /// 重置表单
        /// </summary>
        /// <param name="UC"></param>
        private void reset(GLjingjiren UC)
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


        private void GLjingjiren_Load(object sender, EventArgs e)
        {
            GetData(null);
            #region//进入页面的验证
            DataSet InPutdsCS = GetYZXX();
            //禁用提交区域并开启进度
            panel1.Enabled = false;
            panel2.Enabled = false;
            resetloadLocation(ResetB, PBload);
            //开启线程提交数据
            delegateForThread dft = new delegateForThread(ShowThreadResult_save);
            DataControl.RunThreadClassUpdateJJR RTCTSOU = new DataControl.RunThreadClassUpdateJJR(InPutdsCS, dft, "jjr_set");
            Thread trd = new Thread(new ThreadStart(RTCTSOU.BeginRun));
            trd.IsBackground = true;
            trd.Start();
            #endregion

        }
        #region//首次进入页面的验证       
        /// <summary>
        /// 初始化提交参数的数据集
        /// </summary>
        /// <returns></returns>
        private DataSet initListDataSet()
        {
            DataSet ds = new DataSet();

            DataTable auto2 = new DataTable();
            auto2.TableName = "主表";
            auto2.Columns.Add("买家角色编号");
            auto2.Columns.Add("投标保证金金额");
            auto2.Columns.Add("特殊标记");

            ds.Tables.Add(auto2);
            return ds;
        }
        /// <summary>
        /// 获取首次进入的验证信息
        /// </summary>
        /// <returns></returns>
        public DataSet GetYZXX()
        {
            string JSNM = PublicDS.PublisDsUser.Tables[0].Rows[0]["买家角色编号"].ToString().Trim();
            DataSet InPutdsCS = initListDataSet();
            InPutdsCS.Tables["主表"].Rows.Add(new string[] { JSNM, "0", "仅检查基础" });

            return InPutdsCS;
        }

    
        #endregion
        /// <summary>
        /// 设置默认经纪人
        /// </summary>
        /// <param name="number"></param>
        private void SetAgent(string number,string dlrdlzh)
        {
            DataSet ds = new DataSet();

            DataTable auto2 = new DataTable();
            auto2.TableName = "子表";
            auto2.Columns.Add("编号");
            auto2.Columns.Add("JSBH");
            auto2.Columns.Add("JSDLZH");
            auto2.Columns.Add("DLRDLZH");
            ds.Tables.Add(auto2);
            ds.Tables[0].Rows.Add(new string[] { number, JSNM, PublicDS.PublisDsUser.Tables[0].Rows[0]["DLYX"].ToString(), dlrdlzh });

            //禁用提交区域并开启进度
            panel1.Enabled = false;
            panel2.Enabled = true;
            resetloadLocation(ResetB, PBload);
            //开启线程提交数据
            delegateForThread dft = new delegateForThread(ShowThreadResult_save);
            DataControl.RunThreadClassUpdateJJR RTCTSOU = new DataControl.RunThreadClassUpdateJJR(ds, dft, "jjr_set");
            Thread trd = new Thread(new ThreadStart(RTCTSOU.BeginRun));
            trd.IsBackground = true;
            trd.Start();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView view = (DataGridView)sender;
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                //获取主键
                string num = view.Rows[e.RowIndex].Cells["编号"].Value.ToString();
                string dlrdlyx = view.Rows[e.RowIndex].Cells["经纪人邮箱"].Value.ToString();
                //点击设置默认收款账号链接
                if (view.Columns[e.ColumnIndex].HeaderText == "默认关联经纪人")
                {
                    if (view.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "默认经纪人")
                    {
                        ArrayList AlmsgMR = new ArrayList();
                        AlmsgMR.Add("");
                        AlmsgMR.Add("已经是默认经纪人！");
                        FormAlertMessage FRSEMR = new FormAlertMessage("仅确定", "叹号", "提示", AlmsgMR);
                        FRSEMR.ShowDialog();
                        return;
                    }
                    if (view.Rows[e.RowIndex].Cells["审核状态"].Value.ToString() != "已审核")
                    {
                        ArrayList AlmsgMR = new ArrayList();
                        AlmsgMR.Add("");
                        AlmsgMR.Add("不能将未审核经纪人设置为默认！");
                        FormAlertMessage FRSEMR = new FormAlertMessage("仅确定", "叹号", "提示", AlmsgMR);
                        FRSEMR.ShowDialog();
                        return;
                    }
                  SetAgent(num,dlrdlyx);
               

                }
                //点击查看详情
                else if (view.Columns[e.ColumnIndex].HeaderText == "操作")
                {
                    string str = "  SELECT [ZZ_DLRJBZLXXB].[Number] AS[Number] ,[DLYX],[YHM],[ZZ_DLRJBZLXXB].[JSBH] AS [JSBH],[LXRXM],[SJH],[SFYZSJH],[SJHYZM],[SSSF],[SSDS],[SSQX],[SSFGS],[XXDZ],[YZBM],[SFZH],[ZCLX],[SFZSMJ],[GSMC],[GSDH],[GSDZ],[YYZZSMJ],[ZZJGDMZSMJ],[SWDJZSMJ],[KHXKZSMJ],[FDDBRQZDCNSSMJ],[YXKPLX],[YXKPXX],[ZZ_DLRJBZLXXB].[CreateUser] AS [CreateUser],[ZZ_DLRJBZLXXB].[CreateTime] AS [CreateTime], [SFMRDLR],[SQGLSJ],[DLRSHZT],[DLRSHSJ],[DLRSHYJ],[ZZ_MMJYDLRZHGLB].[JSDLZH] AS [DLJSZH],[ZZ_MMJYDLRZHGLB].[JSYHM] AS [DLJSYHM],[ZZ_MMJYDLRZHGLB].[JSBH] AS[DLJSBH],[GLZT] ,[SFZTXYW],[ZZ_MMJYDLRZHGLB].FGSSHZT,[ZZ_MMJYDLRZHGLB].FGSSHYJ,[ZZ_MMJYDLRZHGLB].FGSSHSJ  FROM [ZZ_DLRJBZLXXB] LEFT JOIN [ZZ_MMJYDLRZHGLB] ON [DLYX]=[DLRDLZH] WHERE [DLYX]='" + view.Rows[e.RowIndex].Cells["经纪人邮箱"].Value.ToString().Trim() + "' AND [ZZ_MMJYDLRZHGLB].[JSDLZH]='" + view.Rows[e.RowIndex].Cells["角色登陆账号"].Value.ToString().Trim() + "'";

                    DataControl.WebServicesCenter WSC = new DataControl.WebServicesCenter();
                   
                    try
                    {
                        DataSet dsxq = WSC.GetDataSet(str);
                        if (dsxq != null && dsxq.Tables[0].Rows.Count > 0)
                        {

                            客户端主程序.SubForm.FormJJRDetail fmd = new FormJJRDetail(dsxq);
                            fmd.ShowDialog();
                        }

                    }
                    catch
                    {
                        ArrayList AlmsgMR = new ArrayList();
                        AlmsgMR.Add("");
                        AlmsgMR.Add("请检查网络，稍后再试！");
                        FormAlertMessage FRSEMR = new FormAlertMessage("仅确定", "叹号", "提醒", AlmsgMR);
                        FRSEMR.ShowDialog();
                    }

                }               

            }

        }
       

        private void ResetB_Click(object sender, EventArgs e)
        {
            //重置本表单，或转向其他页面
            GLjingjiren UC = new GLjingjiren();//这句话是关键，若实例化本身的类，则是重置表单，若实例化其他的类，则是跳转
            reset(UC);
        }

      
    }
}
