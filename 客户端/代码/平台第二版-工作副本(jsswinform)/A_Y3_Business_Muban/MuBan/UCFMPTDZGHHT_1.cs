using System;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Collections;
using 客户端主程序.NewDataControl;
using System.Threading;
using 客户端主程序.SubForm.NewCenterForm;
namespace 客户端主程序.SubForm.NewCenterForm.MuBan
{
    public partial class UCFMPTDZGHHT_1 : UserControl
    {
        Hashtable hashTable;



        public UCFMPTDZGHHT_1()
        {
         
            InitializeComponent();
         
            flowLayoutPanel1.Enabled = false;
        }

        private void UCdayindemo_Load(object sender, EventArgs e)
        {
            Hashtable hashTabl = (Hashtable)this.Tag;
            hashTable = new Hashtable();
            hashTable["中标定标_Number"] = hashTabl["要传递的参数1"].ToString();
            SRT_GetMMJData_Run();
           
        }


        #region//开启线程获取，界面数据并且绑定界面

        //开启一个线程提交数据
        private void SRT_GetMMJData_Run()
        {
            RunThreadClassKTJYZH OTD = new RunThreadClassKTJYZH(hashTable, new delegateForThread(SRT_GetMMJData));
            Thread trd = new Thread(new ThreadStart(OTD.GetDZGHHTXX));
            trd.IsBackground = true;
            trd.Start();
        }
        //显示线程处理结果的函数(用于处理线程返回数据),调用异步委托
        private void SRT_GetMMJData(Hashtable OutPutHT)
        {
            try { Invoke(new delegateForThreadShow(SRT_GetMMJData_Invoke), new Hashtable[] { OutPutHT }); }
            catch (Exception ex) { Support.StringOP.WriteLog("委托回调错误：" + ex.ToString()); }
        }
        //处理非线程创建的控件
        private void SRT_GetMMJData_Invoke(Hashtable OutPutHT)
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
                  
                    flowLayoutPanel1.Enabled = true;
                    BindDataFace(dsreturn);
                    break;
                default:
                    ArrayList Almsg4 = new ArrayList();
                    Almsg4.Add("");
                    Almsg4.Add(showstr);// 这里是提交失败的提示，可能是程序出错，可能是条件不足等
                    FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "其他", "", Almsg4);
                    FRSE4.ShowDialog();

                    break;
            }
            this.MinimumSize = new Size(100, 100);
        }

        #endregion

        /// <summary>
        /// 绑定界面数据信息
        /// </summary>
        /// <param name="dataSet"></param>
        public void BindDataFace(DataSet dataSet)
        {
            DataTable dataTableSeller = dataSet.Tables["卖家资质"];
            DataTable dataTableBuyer = dataSet.Tables["买家资质"];
            DataTable dataTableTBD = dataSet.Tables["投标单"];
            DataTable dataTableYDD = dataSet.Tables["预订单"];
            //卖家资质
            this.lab_Seller_YYZZ.Tag = dataTableSeller.Rows[0]["营业执照扫描件"].ToString();
            this.lab_Seller_ZZJGDMZ.Tag = dataTableSeller.Rows[0]["组织机构代码证扫描件"].ToString();
            this.lab_Seller_SWDJZ.Tag = dataTableSeller.Rows[0]["税务登记证扫描件"].ToString();
            this.lab_Seller_YBNSRZGZM.Tag = dataTableSeller.Rows[0]["一般纳税人资格证明扫描件"].ToString();
            this.lab_Seller_KHXKZ.Tag = dataTableSeller.Rows[0]["开户许可证扫描件"].ToString();
            this.lab_Seller_YLYJK.Tag = dataTableSeller.Rows[0]["预留印鉴卡扫描件"].ToString();
            this.lab_Seller_FDDBRSFZ.Tag = dataTableSeller.Rows[0]["法定代表人身份证扫描件"].ToString();
            this.lab_Seller_FDDBRSFZ_FM.Tag = dataTableSeller.Rows[0]["法定代表人身份证反面扫描件"].ToString();
            this.lab_Seller_FDDBRSQS.Tag = dataTableSeller.Rows[0]["法定代表人授权书"].ToString();
            this.lab_Seller_SFZ.Tag = dataTableSeller.Rows[0]["身份证扫描件"].ToString();
            this.lab_Seller_SFZ_FM.Tag = dataTableSeller.Rows[0]["身份证反面扫描件"].ToString();
            //买家资质
            this.lab_Buyer_YYZZ.Tag = dataTableBuyer.Rows[0]["营业执照扫描件"].ToString();
            this.lab_Buyer_ZZJGDMZ.Tag = dataTableBuyer.Rows[0]["组织机构代码证扫描件"].ToString();
            this.lab_Buyer_SWDJZ.Tag = dataTableBuyer.Rows[0]["税务登记证扫描件"].ToString();
            this.lab_Buyer_YBNSRZGZM.Tag = dataTableBuyer.Rows[0]["一般纳税人资格证明扫描件"].ToString();
            this.lab_Buyer_KHXKZ.Tag = dataTableBuyer.Rows[0]["开户许可证扫描件"].ToString();
            this.lab_Buyer_YLYJK.Tag = dataTableBuyer.Rows[0]["预留印鉴卡扫描件"].ToString();
            this.lab_Buyer_FDDBRSFZ.Tag = dataTableBuyer.Rows[0]["法定代表人身份证反面扫描件"].ToString();
            this.lab_Buyer_FDDBRSFZ_FM.Tag = dataTableBuyer.Rows[0]["法定代表人身份证扫描件"].ToString();
            this.lab_Buyer_FDDBRSQS.Tag = dataTableBuyer.Rows[0]["法定代表人授权书"].ToString();
            this.lab_Buyer_SFZ.Tag = dataTableBuyer.Rows[0]["身份证扫描件"].ToString();
            this.lab_Buyer_SFZ_FM.Tag = dataTableBuyer.Rows[0]["身份证反面扫描件"].ToString();
            //投标单
            this.lab_TBD_MJMC.Text = "卖方名称：" + dataTableTBD.Rows[0]["卖家名称"].ToString();
            this.lab_TBD_FBSJ.Text = "发布时间：" + dataTableTBD.Rows[0]["发布时间"].ToString();
            this.lab_TBD_TBDH.Text = "投标单号：" + dataTableTBD.Rows[0]["投标单号"].ToString();
            this.dataGridViewTBD.AutoGenerateColumns = false;
            this.dataGridViewTBD.DataSource = dataTableTBD.DefaultView;
            //预订单
            this.lab_YDD_MJMC.Text = "买方名称：" + dataTableYDD.Rows[0]["买家名称"].ToString();
            this.lab_YDD_XDSJ.Text = "下单时间：" + dataTableYDD.Rows[0]["下单时间"].ToString();
            this.lab_YDD_YDDH.Text = "预订单号：" + dataTableYDD.Rows[0]["预订单号"].ToString();
            this.dataGridView2.AutoGenerateColumns = false;
            this.dataGridView2.DataSource = dataTableYDD.DefaultView;


            //隐藏和显示
            if (dataTableSeller.Rows[0]["注册类别"].ToString() == "单位")
            {
                this.panel_Seller_YYZZ.Visible = true;
                this.panel_Seller_ZZJGDMZ.Visible = true;
                this.panel_Seller_SWDJZ.Visible = true;
                this.panel_Seller_YBNSRZGZM.Visible = false;
                this.panel_Seller_KHXKZ.Visible = true;
                this.panel_Seller_YLYJK.Visible = true;
                this.panel_Seller_FDDBRSFZ.Visible = true;
                this.panel_Seller_FDDBRSQS.Visible = true;
                this.panel_Seller_SFZ.Visible = false;
            }
            else
            {
                this.panel_Seller_YYZZ.Visible = false;
                this.panel_Seller_ZZJGDMZ.Visible = false;
                this.panel_Seller_SWDJZ.Visible = false;
                this.panel_Seller_YBNSRZGZM.Visible = false;
                this.panel_Seller_KHXKZ.Visible = false;
                this.panel_Seller_YLYJK.Visible = false;
                this.panel_Seller_FDDBRSFZ.Visible = false;
                this.panel_Seller_FDDBRSQS.Visible = false;
                this.panel_Seller_SFZ.Visible = true;
            }

            if (dataTableBuyer.Rows[0]["注册类别"].ToString() == "单位")
            {
                this.panel_Buyer_YYZZ.Visible = true;
                this.panel_Buyer_ZZJGDMZ.Visible = true;
                this.panel_Buyer_SWDJZ.Visible = true;
                this.panel_Buyer_YBNSRZGZM.Visible = false;
                this.panel_Buyer_KHXKZ.Visible = true;
                this.panel_Buyer_YLYJK.Visible = true;
                this.panel_Buyer_FDDBRSFZ.Visible = true;
                this.panel_Buyer_FDDBRSQS.Visible = true;
                this.panel_Buyer_SFZ.Visible = false;

            }
            else
            {
                this.panel_Buyer_YYZZ.Visible = false;
                this.panel_Buyer_ZZJGDMZ.Visible = false;
                this.panel_Buyer_SWDJZ.Visible = false;
                this.panel_Buyer_YBNSRZGZM.Visible = false;
                this.panel_Buyer_KHXKZ.Visible = false;
                this.panel_Buyer_YLYJK.Visible = false;
                this.panel_Buyer_FDDBRSFZ.Visible = false;
                this.panel_Buyer_FDDBRSQS.Visible = false;
                this.panel_Buyer_SFZ.Visible = true;
            
            }


        }
        ImageShow IS;

        /// <summary>
        /// 资质查看
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lab_Buyer_YYZZ_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
     
          
            LinkLabel linkLable = (LinkLabel)sender;
            string url = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[2] + "/JHJXPT/SaveDir/" + linkLable.Tag.ToString().Replace(@"\", "/").Replace(@"\", "/");
            //StringOP.OpenUrl(url);

            Hashtable htT = new Hashtable();
            htT["类型"] = "网址";
            htT["地址"] = url;
            if (IS == null || IS.IsDisposed == true)
            {
                IS = new ImageShow(htT);
            }
            IS.Show();
            IS.Activate();

        }
        ImageShow ISIS;
        private void lab_Seller_YYZZ_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
 

            LinkLabel linkLable = (LinkLabel)sender;
            string url = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[2] + "/JHJXPT/SaveDir/" + linkLable.Tag.ToString().Replace(@"\", "/").Replace(@"\", "/");
            //StringOP.OpenUrl(url);

            Hashtable htT = new Hashtable();
            htT["类型"] = "网址";
            htT["地址"] = url;
            if (ISIS == null || ISIS.IsDisposed == true)
            {
                ISIS = new ImageShow(htT);
            }
            ISIS.Show();
            ISIS.Activate();
        }

        /// <summary>
        /// 投标单下载页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkTBD_Load_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {


            FormDZGHHT dy = new FormDZGHHT("查看并打印投标单", hashTable["中标定标_Number"].ToString(), new string[] { "Y3_Business_Muban.dll)!客户端主程序.SubForm.NewCenterForm.MuBan.UCFMPTTBD" });
            dy.Show();
        }

        /// <summary>
        /// 预订单下载页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkYDD_Load_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            FormDZGHHT dy = new FormDZGHHT("查看并打印预订单", hashTable["中标定标_Number"].ToString(), new string[] { "Y3_Business_Muban.dll)!客户端主程序.SubForm.NewCenterForm.MuBan.UCFMPTYDD" });
            dy.Show();
        }










    }
}
