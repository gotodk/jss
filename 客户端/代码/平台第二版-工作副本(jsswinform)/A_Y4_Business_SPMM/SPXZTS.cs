using Com.Seezt.Skins;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using 客户端主程序;
using 客户端主程序.SubForm;


namespace 客户端主程序
{
    public partial class SPXZTS : BasicForm
    {

        //淡出计时器
        System.Windows.Forms.Timer Timer_DC;

        /// <summary>
        /// 查询条件
        /// </summary>
        Hashtable ht_where = new Hashtable();
        fmNMRSP spmr;
        delegateForThread dftForParentts;
        DataGridView dgv;
        int Rowindex;
        /// <summary>
        /// 是否为从买入窗体实例化来。
        /// </summary>
        bool isFromBuyAuto = true;

        /// <summary>
        /// 对勾选择后的回调指针
        /// </summary>
        delegateForThread dftForParent;
        public SPXZTS()
        {
            InitializeComponent();
        }

        #region 窗体淡出，窗体最小化最大化等特殊控制，所有窗体都有这个玩意

        /// <summary>
        /// 窗体的Load事件中的淡出处理
        /// </summary>
        private void Init_one_show()
        {
            //设置双缓冲
            this.SetStyle(ControlStyles.DoubleBuffer |
            ControlStyles.UserPaint |
            ControlStyles.AllPaintingInWmPaint,
            true);
            this.UpdateStyles();

            //加载淡出计时器
            Timer_DC = new System.Windows.Forms.Timer();
            Timer_DC.Interval = Program.DC_Interval;
            this.Timer_DC.Tick += new System.EventHandler(this.Timer_DC_Tick);
            //淡出效果
            MaxDC();
        }

        /// <summary>
        /// 显示窗体时启动淡出
        /// </summary>
        private void MaxDC()
        {
            this.Opacity = 0;
            Timer_DC.Enabled = true;
        }

        //淡出显示窗体，绕过窗体闪烁问题
        private void Timer_DC_Tick(object sender, EventArgs e)
        {
            this.Opacity = this.Opacity + Program.DC_step;
            if (!Program.DC_open)
            {
                this.Opacity = 1;
            }
            if (this.Opacity >= 1)
            {
                Timer_DC.Enabled = false;
            }
        }

        //允许任务栏最小化
        protected override CreateParams CreateParams
        {
            get
            {
                const int WS_MINIMIZEBOX = 0x00020000;  // Winuser.h中定义
                CreateParams cp = base.CreateParams;
                cp.Style = cp.Style | WS_MINIMIZEBOX;   // 允许最小化操作
                return cp;
            }
        }


        private int WM_SYSCOMMAND = 0x112;
        private long SC_MAXIMIZE = 0xF030;
        private long SC_MINIMIZE = 0xF020;
        private long SC_CLOSE = 0xF060;
        private long SC_NORMAL = 0xF120;
        private FormWindowState SF = FormWindowState.Normal;
        /// <summary>
        /// 重绘窗体的状态
        /// </summary>
        /// <param name="m"></param>
      

        #endregion

        public SPXZTS(DataGridView ht, delegateForThread DftForParentts, fmNMRSP form,int rowindex)
        {
            InitializeComponent();
            dgv = ht;
            dftForParentts = DftForParentts;
            spmr = form;
            Rowindex = rowindex;



            //图标
            Icon ic = new Icon(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\skin\ji.ico");
            this.Icon = ic;
            //设置窗体显示在屏幕中央
            this.StartPosition = FormStartPosition.CenterScreen;
            //不在任务栏显示此信息
            this.ShowInTaskbar = false;
        }

        private void SPXZTS_Load(object sender, EventArgs e)
        {

           // return_ht["商品编号"] = dataGridView1.Rows[rowindex].Cells["商品编号"].Value.ToString();//商品编号
            //return_ht["商品名称"] = dataGridView1.Rows[rowindex].Cells["商品名称"].Value.ToString();//商品编号
            string strname = dgv.Rows[Rowindex].Cells["商品名称"].Value.ToString() ;
            if (strname.Length > 20)
            {
                strname = strname.Remove(17).ToString();
                strname += "....";
            }
            label1.Text = "您当前选择的商品名称为“(" + strname + ")”";

        }

        private void basicButton_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
            spmr.Enabled = true;

        }

        private void basicButton_js_Click(object sender, EventArgs e)
        {
            BasicButton bb = sender as BasicButton;
            string name = bb.Name.ToString();
           // int rowindex = e.RowIndex;
            string strQX = string.Empty;
            object ZDJG = 0;
            object MJJJPL = 0;
          
            switch (name)
            {
                case "basicButton_js":
                    ht_where["合同期限"] = "即时";
                    ZDJG = dgv.Rows[Rowindex].Cells["ZDJGJS"].Value;
                    MJJJPL = dgv.Rows[Rowindex].Cells["MJJJPLJS"].Value;
                    break;
                case "basicButton_sgy":
                    ht_where["合同期限"] = "三个月";
                    ZDJG = dgv.Rows[Rowindex].Cells["ZDJGSGY"].Value;
                    MJJJPL = dgv.Rows[Rowindex].Cells["MJJJPLSGY"].Value;
                    break;
                case "basicButton_yn":
                    ht_where["合同期限"] = "一年";
                    ZDJG = dgv.Rows[Rowindex].Cells["ZDJGYN"].Value;
                    MJJJPL = dgv.Rows[Rowindex].Cells["MJJJPLYN"].Value;
                    break;
                default:
                    break;
            }

            ht_where["商品编号"] = dgv.Rows[Rowindex].Cells["商品编号"].Value.ToString();//商品编号
            ht_where["商品名称"] = dgv.Rows[Rowindex].Cells["商品名称"].Value.ToString();//商品编号
            ht_where["规格"] = dgv.Rows[Rowindex].Cells["规格"].Value.ToString();//商品编号
            //ht_where["合同期限"] = dgv.Rows[Rowindex].Cells["合同期限"].Value.ToString();//商品编号
            ht_where["计价单位"] = dgv.Rows[Rowindex].Cells["计价单位"].Value.ToString();//商品编号
            ht_where["平台批量"] = dgv.Rows[Rowindex].Cells["Column2"].Value.ToString();//商品编号
            ht_where["最低价格"] = ZDJG;//商品编号
            ht_where["卖家批量"] = MJJJPL;//商品编号
            ht_where["上传资质"] = dgv.Rows[Rowindex].Cells["SCZZYQ"].Value.ToString();//该商品需要上传的资质
            ht_where["ZLBZYZM"] = dgv.Rows[Rowindex].Cells["ZLBZYZM"].Value;//下面是6个必要的资质，最后一次使用过的
            ht_where["CPJCBG"] = dgv.Rows[Rowindex].Cells["CPJCBG"].Value;
            ht_where["PGZFZRFLCNS"] = dgv.Rows[Rowindex].Cells["PGZFZRFLCNS"].Value;
            ht_where["FDDBRCNS"] = dgv.Rows[Rowindex].Cells["FDDBRCNS"].Value;
            ht_where["SHFWGDYCN"] = dgv.Rows[Rowindex].Cells["SHFWGDYCN"].Value;
            ht_where["CPSJSQS"] = dgv.Rows[Rowindex].Cells["CPSJSQS"].Value;
            //return_ht["QTZZ"] = dataGridView1.Rows[rowindex].Cells[16].Value;
            ht_where["SLZM"] = dgv.Rows[Rowindex].Cells["SLZM"].Value;

            if (ht_where != null && !ht_where["合同期限"].Equals(""))
            {
                dftForParentts(ht_where);
                spmr.Close();
                this.Close();
            }
        }


        private void fhdate()
        {
            dftForParentts(ht_where);
            spmr.Close();
        }

        private void SPXZTS_FormClosed(object sender, FormClosedEventArgs e)
        {
            //this.Close();
            //this.Dispose();
            spmr.Enabled = true;
        }






    }
}
