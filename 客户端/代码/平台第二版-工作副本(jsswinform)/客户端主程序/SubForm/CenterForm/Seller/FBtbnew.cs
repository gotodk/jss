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

namespace 客户端主程序.SubForm.CenterForm.Seller
{
    public partial class FBtbnew : UserControl
    {
        //控件组标记
        int UCnum = 0;

        UCfbtbnewList uCfbtbnewList1;

        public FBtbnew()
        {
            InitializeComponent();

            //加载后，生成一个列表
            uCfbtbnewList1 = new UCfbtbnewList(this);
            uCfbtbnewList1.Location = new Point(25, 40);
            uCfbtbnewList1.Name = "uCfbtbnewList1";
            this.panelUC5.Controls.Add(uCfbtbnewList1);
            uCfbtbnewList1.Visible = false;
            //加载后，新建一个录入区域，重置相关数据
            addUCfbtbnew();
        }

        //初始化参数列表显示集合
        private DataTable initListDataTable()
        {
            DataTable auto = new DataTable();
            auto.TableName = "输入列表";
            auto.Columns.Add("控件名称");
            auto.Columns.Add("商品编号");
            auto.Columns.Add("商品名称");
            auto.Columns.Add("规格型号");
            auto.Columns.Add("投标拟售量");
            auto.Columns.Add("投标价格");
            auto.Columns.Add("提货经济批量");
            auto.Columns.Add("合同期限");
            auto.Columns.Add("不发货区域");
            auto.Columns.Add("质量标准附件");
            auto.Columns.Add("品管总负责人法律承诺书");
            auto.Columns.Add("产品技术与安全鉴定报告");
            auto.Columns.Add("增值税发票税率书面证明");
            auto.Columns.Add("商标与专利证书");
            auto.Columns.Add("危害报告及承诺");
            auto.Columns.Add("法定代表人承诺书");
            
            return auto;
        }

        /// <summary>
        /// 初始化提交参数的数据集
        /// </summary>
        /// <returns></returns>
        private DataSet initListDataSet()
        {
            DataSet ds = new DataSet();

            DataTable auto = initListDataTable();
            auto.PrimaryKey = new DataColumn[] { auto.Columns["控件名称"] };

            DataTable auto2 = new DataTable();
            auto2.TableName = "主表";
            auto2.Columns.Add("卖家角色编号");
            auto2.Columns.Add("投标保证金金额");
            auto2.Columns.Add("特殊标记");
            
            ds.Tables.Add(auto2);
            ds.Tables.Add(auto);
            return ds;
        }

        /// <summary>
        /// 获取表单的内容
        /// </summary>
        /// <returns></returns>
        public DataSet GetTJ()
        {
            string JSNM = PublicDS.PublisDsUser.Tables[0].Rows[0]["卖家角色编号"].ToString().Trim();
            DataSet InPutdsCS = initListDataSet();

            //处理子表
            foreach (Control control in panelUC5.Controls)
            {
                if (control is UCfbtbnew)
                {
                    //隐藏输入区域
                    UCfbtbnew UCfb = (UCfbtbnew)control;
                    string bfhqy = "||";
                    if (UCfb.UCbfhqy.Text == "无" || UCfb.UCbfhqy.Text.Trim() == "")
                    {
                        bfhqy = "||";
                    }
                    else
                    {
                        bfhqy = UCfb.UCbfhqy.Text;
                    }
                    #region//1、上传资质测试时关闭，正式用时再开【共三处需要打开，本页有两处，UCfbtbnew.cs有一处】
                    //string zlbz = UCfb.listView1.Items[0].SubItems[1].Text.Trim().Replace(@"\", "/");//质量标准附件
                    //string flcns = UCfb.listView2.Items[0].SubItems[1].Text.Trim().Replace(@"\", "/");//品管总负责人法律承诺书
                    //string aqjdbg = UCfb.listView3.Items[0].SubItems[1].Text.Trim().Replace(@"\", "/");//产品技术与安全鉴定报告
                    //string fpzm = UCfb.listView4.Items[0].SubItems[1].Text.Trim().Replace(@"\", "/");//增值税发票税率书面证明
                    //string zlzm = UCfb.listView5.Items[0].SubItems[1].Text.Trim().Replace(@"\", "/");//商标与专利证书
                    //string whbg = UCfb.listView5.Items[0].SubItems[1].Text.Trim().Replace(@"\", "/");//危害报告及承诺
                    //string dbrcns = UCfb.listView5.Items[0].SubItems[1].Text.Trim().Replace(@"\", "/");//法定代表人承诺书

                    string zlbz = "";//质量标准附件
                    string flcns = "";//品管总负责人法律承诺书
                    string aqjdbg = "";//产品技术与安全鉴定报告
                    string fpzm = "";//增值税发票税率书面证明
                    string zlzm = "";//商标与专利证书
                    string whbg = "";//危害报告及承诺
                    string dbrcns = "";//法定代表人承诺书
                    #endregion

                    InPutdsCS.Tables["输入列表"].Rows.Add(new string[] { UCfb.Name, UCfb.UCspbh.Text, UCfb.UCspmc.Text, UCfb.UCspgg.Text, UCfb.UCysl.Text, UCfb.UCtbjg.Text, UCfb.UCjjpl.Text, UCfb.CBhhzq.Text.ToString(), bfhqy, zlbz, flcns, aqjdbg, fpzm, zlzm, whbg, dbrcns });
                }

            }

            //处理主表
            double dbbzj = 0;//投标保证金
            //计算投标保证金
            //...............................
            InPutdsCS.Tables["主表"].Rows.Add(new string[] { JSNM, dbbzj.ToString(),"" });

            return InPutdsCS;
        }

        /// <summary>
        /// 显示或隐藏列表区域,true为显示，false为隐藏
        /// </summary>
        /// <param name="f">是否显示</param>
        /// <param name="d">是否重置列表数据</param>
        public void ShowOrHideList(bool f,bool d)
        {
            //用特殊方法把滚动条强制置顶，解决一个bug
            TextBox tb = new TextBox();
            tb.Location = new Point(-1000,-1000);
            this.Controls.Add(tb);
            tb.Focus();
            this.Controls.Remove(tb);
            //若显示
            if(f)
            {
                DataTable newlist = initListDataTable();

                //移动位置
                uCfbtbnewList1.Location = new Point(25, 40);
                foreach (Control control in panelUC5.Controls)
                {
                    if (control is UCfbtbnew)
                    {
                        //隐藏输入区域
                        UCfbtbnew UCfb = (UCfbtbnew)control;
                        UCfb.Visible = false;
                        if (d)
                        {
                            #region//2、上传资质测试时关闭，正式用时再开【共三处需要打开，本页有两处，UCfbtbnew.cs有一处】
                            //string zlbz = UCfb.listView1.Items[0].SubItems[1].Text.Trim().Replace(@"\", "/");//质量标准附件
                            //string flcns = UCfb.listView2.Items[0].SubItems[1].Text.Trim().Replace(@"\", "/");//品管总负责人法律承诺书
                            //string aqjdbg = UCfb.listView3.Items[0].SubItems[1].Text.Trim().Replace(@"\", "/");//产品技术与安全鉴定报告
                            //string fpzm = UCfb.listView4.Items[0].SubItems[1].Text.Trim().Replace(@"\", "/");//增值税发票税率书面证明
                            //string zlzm = UCfb.listView5.Items[0].SubItems[1].Text.Trim().Replace(@"\", "/");//商标与专利证书
                            //string whbg = UCfb.listView5.Items[0].SubItems[1].Text.Trim().Replace(@"\", "/");//危害报告及承诺
                            //string dbrcns = UCfb.listView5.Items[0].SubItems[1].Text.Trim().Replace(@"\", "/");//法定代表人承诺书

                            string zlbz = "";//质量标准附件
                            string flcns = "";//品管总负责人法律承诺书
                            string aqjdbg = "";//产品技术与安全鉴定报告
                            string fpzm = "";//增值税发票税率书面证明
                            string zlzm = "";//商标与专利证书
                            string whbg = "";//危害报告及承诺
                            string dbrcns = "";//法定代表人承诺书
                            #endregion
                            newlist.Rows.Add(new string[] { UCfb.Name, UCfb.UCspbh.Text, UCfb.UCspmc.Text, UCfb.UCspgg.Text, UCfb.UCysl.Text, UCfb.UCtbjg.Text, UCfb.UCjjpl.Text, UCfb.CBhhzq.Text.ToString(), UCfb.UCbfhqy.Text.ToString(), zlbz, flcns, aqjdbg, fpzm, zlzm, whbg, dbrcns });
                        }
                    }

                }
                if (d)
                {
                    //更新列表数据
                    uCfbtbnewList1.tblist = newlist;
                    cktbd.Text = "查看投标单(" + newlist.Rows.Count + ")";
                }
            }
            uCfbtbnewList1.Visible = f;
            
        }

        /// <summary>
        /// 添加一个输入区域
        /// </summary>
        public void addUCfbtbnew()
        {
            UCfbtbnew UCfb = new UCfbtbnew(this);
            UCfb.Location = new System.Drawing.Point(25, 40);
            UCfb.Name = "UCfbtbnewH" + UCnum;
            this.panelUC5.Controls.Add(UCfb);
            UCnum++;
        }

        /// <summary>
        /// 检查是否有重复的信息
        /// </summary>
        /// <param name="SPBH"></param>
        /// <param name="HTZQ"></param>
        public bool CheckChongFu(string SPBH,string HTZQ,string KJMC)
        {
            DataTable dtlist = uCfbtbnewList1.tblist;
            if (dtlist == null)
            {
                return false;
            }
            for (int p = 0; p < dtlist.Rows.Count; p++)
            {
                if (dtlist.Rows[p]["商品编号"].ToString() == SPBH && dtlist.Rows[p]["合同期限"].ToString() == HTZQ && dtlist.Rows[p]["控件名称"].ToString() != KJMC)
                {
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// 显示或移除指定名称的输入区域
        /// </summary>
        /// <param name="name">空间名</param>
        /// <param name="h">是否显示</param>
        /// <param name="del">是否删除</param>
        public void showOrRemoveOneUC(string name,bool h,bool del)
        {
            //这里有个bug
            Control[] Carr = panelUC5.Controls.Find(name, false);
            if (Carr.Length < 1)
            {
                return;
            }
            UCfbtbnew UCfb = (UCfbtbnew)(panelUC5.Controls.Find(name, false).GetValue(0));
            if (h)
            {
                UCfb.Visible = true;
                UCfb.IsEdit = true;
            }
            else
            {
                UCfb.Visible = false;
            }
            if (del)
            {
                this.panelUC5.Controls.Remove(UCfb);
            }
            else
            {
                ;
            }
        }


        /// <summary>
        /// 获取首次进入的验证信息
        /// </summary>
        /// <returns></returns>
        public DataSet GetYZXX()
        {
            string JSNM = PublicDS.PublisDsUser.Tables[0].Rows[0]["卖家角色编号"].ToString().Trim();
            DataSet InPutdsCS = initListDataSet();
            InPutdsCS.Tables["主表"].Rows.Add(new string[] { JSNM,"0", "仅检查基础" });

            return InPutdsCS;
        }


        /// <summary>
        /// 重置表单
        /// </summary>
        /// <param name="UC"></param>
        public void reset()
        {
            FBtbnew UC = new FBtbnew();
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


        bool runonece = true;//只运行一次
        private void panelUC5_Paint(object sender, PaintEventArgs e)
        {
            if (runonece)
            {
                DataSet InPutdsCS = GetYZXX();
                //开启线程提交数据
                delegateForThread dft = new delegateForThread(ShowThreadResult_save);
                DataControl.RunThreadClassFBTHXXSave RTC = new DataControl.RunThreadClassFBTHXXSave(InPutdsCS, dft);
                Thread trd = new Thread(new ThreadStart(RTC.BeginRun));
                trd.IsBackground = true;
                trd.Start();
                runonece = false;
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

            //显示执行结果
            DataSet returnds = (DataSet)returnHT["执行结果"];
            string jieguo = returnds.Tables["返回值单条"].Rows[0]["执行结果"].ToString();
            string showmsg = returnds.Tables["返回值单条"].Rows[0]["错误提示"].ToString();
            switch (jieguo)
            {
                case "ok":
                    //通过检查了，不需要处理
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
                    Almsg4.Add(showmsg);// 这里是提交失败的提示，可能是程序出错，可能是条件不足等
                    FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "错误", "提交xxx失败", Almsg4);
                    FRSE4.ShowDialog();
    
                    break;
            }

        }



        //查看投标单
        private void cktbd_Click(object sender, EventArgs e)
        {
            //删除当前输入区域，作废掉了
            foreach (Control control in panelUC5.Controls)
            {
                if (control is UCfbtbnew)
                {
                    //隐藏输入区域
                    UCfbtbnew UCfb = (UCfbtbnew)control;
                    if (UCfb.Visible == true)
                    {
                        if (!UCfb.IsEdit)
                        {
                            panelUC5.Controls.Remove(UCfb);
                        }
                        else
                        {
                            UCfb.Visible = false;
                        }
                    }
                }

            }
            //显示列表
            ShowOrHideList(true,false);
        }








    }
}
