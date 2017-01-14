using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using 客户端主程序.NewDataControl;
using System.Threading;
using System.Collections;
using 客户端主程序.DataControl;

namespace 客户端主程序.SubForm.NewCenterForm.SUBUC.JJRYWGL
{
    public partial class ucJJRZGZ_B : UserControl
    {

        Hashtable HTuser = new Hashtable();


        public ucJJRZGZ_B()
        {
            InitializeComponent();
            HTuser["DLYX"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString();
            HTuser["JSZHLX"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["结算账户类型"].ToString();
            this.panelTJQ.Enabled = false;
            this.panel1.Enabled = false;
            this.pbJJRZGZS.Enabled = false;
                this.panlInfo.Visible = false;
            this.panlInfo.Location = new Point(this.panelTJQ.Location.X+this.panelTJQ.Width/2-100,50);
        }



        #region//获取经济人资格证书

        //开启一个线程获取基础数据
        private void SRT_JJRZGZSData_Run()
        {
            RunThreadClassKTJYZH OTD = new RunThreadClassKTJYZH(HTuser, new delegateForThread(SRT_JJRZGZSData));
            Thread trd = new Thread(new ThreadStart(OTD.GetJJRZGZSData));
            trd.IsBackground = true;
            trd.Start();
            if (PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["结算账户类型"].ToString().Equals("经纪人交易账户"))
            {
                this.panlInfo.Visible = true;
            }
            else
            {
                this.panlInfo.Visible = false;
            }
        }
        //显示线程处理结果的函数(用于处理线程返回数据),调用异步委托
        private void SRT_JJRZGZSData(Hashtable OutPutHT)
        {
            try { Invoke(new delegateForThreadShow(SRT_JJRZGZSData_Invoke), new Hashtable[] { OutPutHT }); }
            catch (Exception ex) { Support.StringOP.WriteLog("委托回调错误：" + ex.ToString()); }
        }
        //处理非线程创建的控件
        private void SRT_JJRZGZSData_Invoke(Hashtable OutPutHT)
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
                    BindDataFaceData(dsreturn);
                    break;
                default:

                    panlInfo.Visible = false;
                    ArrayList Almsg4 = new ArrayList();
                    Almsg4.Add("");
                    Almsg4.Add(showstr);// 这里是提交失败的提示，可能是程序出错，可能是条件不足等
                    FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "其他", "", Almsg4);
                    FRSE4.ShowDialog();

                    break;
            }
        }

        /// <summary>
        ///获取经纪人资格证书编号路径，并绑定数据
        /// </summary>
        /// <param name="dsreturn"></param>
        public void BindDataFaceData(DataSet dsreturn)
        {
            DataTable dataTable = dsreturn.Tables["经纪人交易账户信息"];
            if (dataTable.Rows[0]["B_JSZHLX"].ToString() == "经纪人交易账户")
            {
                if (!String.IsNullOrEmpty(dataTable.Rows[0]["经纪人资格证书"].ToString()))
                {
                    this.panelTJQ.Enabled = true;
                    this.panel1.Enabled = true;
                    this.pbJJRZGZS.Enabled = true;
                    string url = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[2] + dataTable.Rows[0]["经纪人资格证书"].ToString();
                    this.pbJJRZGZS.ImageLocation = url;
                    this.panlInfo.Visible = false;
                }
                else
                {
                  
                    this.panelTJQ.Enabled = false;
                    this.panel1.Enabled = false;
                    this.pbJJRZGZS.Enabled = false;
                    this.panlInfo.Visible = false;
                }

            }
            else
            {
                this.panelTJQ.Enabled = false;
                this.panel1.Enabled = false;
                this.pbJJRZGZS.Enabled = false;
                this.panlInfo.Visible = false;
            }

        
        
        }
        #endregion

        /// <summary>
        /// 经纪人资格证书页面  加载完毕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ucJJRZGZ_B_Load(object sender, EventArgs e)
        {
            SRT_JJRZGZSData_Run();
        }
        /// <summary>
        /// 下载经纪人资格证书
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
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            //saveFileDialog.Filter = "JPeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif|PnG Image|*.png|Wmf  Image|*.wmf";
            saveFileDialog.Filter = "PNG Image|*.png";
            saveFileDialog.FilterIndex = 0;
            if (pbJJRZGZS.Image == null)
            {
                ArrayList Almsg4 = new ArrayList();
                Almsg4.Add("");
                Almsg4.Add("图片证书获取失败！");// 这里是提交失败的提示，可能是程序出错，可能是条件不足等
                FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "其他", "", Almsg4);
                FRSE4.ShowDialog();
            }
            else if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (pbJJRZGZS.Image != null)
                {
                    pbJJRZGZS.Image.Save(saveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Png);
                }
            }






        }

    }
}
