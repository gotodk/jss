using System;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using System.Collections;
using System.Threading;
using 客户端主程序.Support;

namespace 客户端主程序.SubForm
{
    public partial class UCPager : UserControl
    {
        //窗体回调委托
        private delegateForThread dft_form;

        /// <summary>
        /// 窗体回调委托
        /// </summary>
        [Description("设置或读取回调函数"), Category("Appearance")]
        public delegateForThread DFT
        {
            get
            {
                return dft_form;
            }
            set
            {
                dft_form = value;
            }
        }

        private Hashtable ht_where;

        /// <summary>
        /// 查询条件配置
        /// </summary>
        [Description("设置或读取当前查询配置"), Category("Appearance")]
        public Hashtable HT_Where
        {
            get
            {
                return ht_where;
            }
            set
            {
                ht_where = value;
            }
        }

        private bool isOpen = true;

        public bool IsOpen
        {
            set { isOpen = value; }
        }

        public UCPager()
        {
            InitializeComponent();


        }



        /// <summary>
        /// 开始准备绑定数据，从远程根据条件获取数据(开线程)
        /// </summary>
        public void BeginBindForUCPager()
        {

            //显示进度条
            PBloading.Visible = true;
            //禁用分页操作
            sy.Enabled = false;
            syy.Enabled = false;
            xyy.Enabled = false;
            wy.Enabled = false;
            btbgo.Enabled = false;
            LGO.Enabled = false;

            delegateForThread dft_pager = new delegateForThread(ShowThreadResult_pager);
            DataControl.RunThreadClassPager RTCP = new DataControl.RunThreadClassPager(ht_where, dft_pager, dft_form);
            Thread trd = new Thread(new ThreadStart(RTCP.BeginRun));
            trd.IsBackground = true;
            trd.Start();

        }

        //显示线程处理结果的函数(用于处理线程返回数据)
        private void ShowThreadResult_pager(Hashtable returnHT)
        {
            try
            {
                //调用异步委托
                Invoke(new delegateForThreadShow(ShowThreadResult_pager_Invoke), new Hashtable[] { returnHT });
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("委托回调错误：" + ex.ToString());
            }
        }
        //处理非线程创建的控件
        private void ShowThreadResult_pager_Invoke(Hashtable returnHT)
        {
            DataSet ds = (DataSet)returnHT["数据"];//获取返回的数据集
            int fys = (int)ds.Tables["附加数据"].Rows[0]["分页数"];
            int jls = (int)ds.Tables["附加数据"].Rows[0]["记录数"];
            string msg = (string)ds.Tables["附加数据"].Rows[0]["其他描述"];
            string err = (string)ds.Tables["附加数据"].Rows[0]["执行错误"];
            //回写返回值
            ht_where["PageCount"] = fys;
            ht_where["RecordCount"] = jls;
            if (isOpen)
            {
                //线程执行完成，隐藏进度条
                PBloading.Visible = false;
                //开启分页操作
                btbgo.Enabled = true;
                LGO.Enabled = true;
                sy.Enabled = true;
                syy.Enabled = true;
                xyy.Enabled = true;
                wy.Enabled = true;
            }

            int page_index = 0;//当前页索引
            if (ht_where.ContainsKey("page_index") && ht_where["page_index"].ToString().Trim() != "")
            {
                page_index = Convert.ToInt32(ht_where["page_index"]);
            }
            int page_size = 0;//每页数
            if (ht_where.ContainsKey("page_size") && ht_where["page_size"].ToString().Trim() != "")
            {
                page_size = Convert.ToInt32(ht_where["page_size"]);
            }


            btbgo.Texts = (page_index + 1).ToString();
            if (fys < 1)
            {
                label1.Text = "每页" + ht_where["page_size"].ToString() + "条，共" + jls + "条数据。";
            }
            else
            {
                label1.Text = "当前第" + btbgo.Texts + "/" + fys + "页，每页" + ht_where["page_size"].ToString() + "条，共" + jls + "条数据。";
            }


            //没有首页了
            if (page_index - 1 < 0)
            {
                sy.Enabled = false;
                syy.Enabled = false;
            }
            //没有尾页了
            if (page_index + 1 >= fys)
            {
                xyy.Enabled = false;
                wy.Enabled = false;
            }

        }

        private void UCPager_Load(object sender, EventArgs e)
        {

        }

        //转到首页
        private void sy_Click(object sender, EventArgs e)
        {
            if (ht_where != null)
            {
                ht_where["page_index"] = 0;
                BeginBindForUCPager(); 
            }
           
        }
        //上一页
        private void syy_Click(object sender, EventArgs e)
        {
            if (ht_where != null)
            {
                int dqy = Convert.ToInt32(ht_where["page_index"]);
                if (dqy < 1)
                {
                    dqy = 0;
                }
                ht_where["page_index"] = dqy - 1;
                BeginBindForUCPager();
            }
          
        }
        //下一页
        private void xyy_Click(object sender, EventArgs e)
        {
            if (ht_where != null)
            {
                int dqy = Convert.ToInt32(ht_where["page_index"]);
                int fys = Convert.ToInt32(ht_where["PageCount"]);
                if ((dqy + 1) >= (fys - 1))
                {
                    dqy = fys - 2;
                }
                ht_where["page_index"] = dqy + 1;
                BeginBindForUCPager();
            }
           

        }

        //转到尾页
        private void wy_Click(object sender, EventArgs e)
        {
            if (ht_where != null)
            {
                int fys = Convert.ToInt32(ht_where["PageCount"]);
                if (fys < 0)
                {
                    fys = 0;
                }
                ht_where["page_index"] = fys - 1;
                BeginBindForUCPager();
            }
           
        }
        //转到
        private void LGO_Click(object sender, EventArgs e)
        {
            if (ht_where != null)
            {
                string newpageindex = btbgo.Texts;
                if (!StringOP.IsNumeric(newpageindex))
                {
                    btbgo.Texts = (Convert.ToInt32(ht_where["page_index"]) + 1).ToString();
                    return;
                }
                else
                {
                    int fys = Convert.ToInt32(ht_where["PageCount"]);

                    int index = Convert.ToInt32(newpageindex);
                    if (index <= 1)
                    {
                        index = 1;
                    }
                    if (index >= fys)
                    {
                        index = fys;
                    }
                    btbgo.Texts = index.ToString();

                    ht_where["page_index"] = index - 1;
                    BeginBindForUCPager();
                }
 
            }
           
        }


        private void basicTextBox1_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 设置小圈圈的显示与否
        /// </summary>
        /// <param name="b"></param>
        public void SetPicVisible(bool b)
        {
            this.PBloading.Visible = b;
            sy.Enabled = !b;
            syy.Enabled = !b;
            xyy.Enabled = !b;
            wy.Enabled = !b;
            btbgo.Enabled = !b;
            LGO.Enabled = !b;
        }


    }
}
