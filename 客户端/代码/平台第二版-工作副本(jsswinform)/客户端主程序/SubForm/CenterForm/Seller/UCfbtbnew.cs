using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using 客户端主程序.SubForm.CenterForm.Buyer;
using 客户端主程序.DataControl;
using 客户端主程序.Support;

namespace 客户端主程序.SubForm.CenterForm.Seller
{
    public partial class UCfbtbnew : UserControl
    {
        bool isedit = false;
        /// <summary>
        /// 获取或设置是否处于修改状态
        /// </summary>
        [Description("获取或设置是否处于修改状态"), Category("Appearance")]
        public bool IsEdit
        {
            get
            {
                return isedit;
            }
            set
            {
                isedit = value;
            }
        }

        private FBtbnew FB;

        public UCfbtbnew(FBtbnew FBtemp)
        {
            InitializeComponent();
            FB = FBtemp;
        }

        private void UCfbtbnew_Load(object sender, EventArgs e)
        {

            //必须先初始化省市区下拉框
            ucCityList1.initdefault();
            ucCityList1.VisibleItem = new bool[] { true, true, false };
        }

        /// <summary>
        /// 显示错误提示
        /// </summary>
        /// <param name="err"></param>
        private void showAlertY(string err)
        {
            ArrayList Almsg3 = new ArrayList();
            Almsg3.Add("");
            Almsg3.Add(err);
            FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "错误", "投标信息提交", Almsg3);
            FRSE3.ShowDialog();
        }

        //加入投标单
        private void jrtbd_Click(object sender, EventArgs e)
        {
            //进行一些基本的验证
            string ysl = UCysl.Text;
            string jjpl = UCjjpl.Text;
            string jg = UCtbjg.Text;
            if (UCspbh.Text.Trim() == "")
            {
                showAlertY("请选择商品！");
                return;
            }
            if (ysl == "")
            {
                //请填写投标拟售量。
                showAlertY("请填写投标拟售量！");
                return;
            }
            if (Convert.ToInt64(ysl) <= 0)
            {
                //请填写投标拟售量。
                showAlertY("投标拟售量必须大于零！");
                return;
            }
            if (Convert.ToInt64(ysl) < Convert.ToInt64(UCzdjjpl.Text) * 10)
            {
                //投标拟售量须大于等于十倍最大经济批量
                showAlertY("投标拟售量须大于等于十倍最大经济批量！");
                return;
            }
            if (jjpl == "")
            {
                //请填写提货经济批量。
                showAlertY("请填写提货经济批量！");
                return;
            }
            if(Convert.ToInt64(jjpl) <= 0)
            {
                //请填写提货经济批量。
                showAlertY("提货经济批量必须大于零！");
                return;
            }
            if (Convert.ToInt64(jjpl) > Convert.ToInt64(UCzdjjpl.Text))
            {
                //提货经济批量须小于该产品最大经济批量
                showAlertY("提货经济批量须小于该产品最大经济批量！");
                return;
            }
            if (jg == "" || Convert.ToDouble(jg) <= 0)
            {
                //请填写投标价格。
                showAlertY("请填写投标价格！");
                return;
            }
            #region//上传资质测试时关闭，正式用时再开【共三处需要打开，本页有一处，FBtbnew.cs有两处】
            //if (listView1.Items.Count <= 0 || listView1.Items[0].SubItems[1].Text.Trim() == "")
            //{
            //    showAlertY("请上传质量标准与证明！");
            //    return;
            //}
            //if (listView2.Items.Count <= 0 || listView2.Items[0].SubItems[1].Text.Trim() == "")
            //{
            //    showAlertY("请上传品管总负责人法律承诺书！");
            //    return;
            //}
            //if (listView3.Items.Count <= 0 || listView3.Items[0].SubItems[1].Text.Trim() == "")
            //{
            //    showAlertY("请上传产品技术与安全鉴定报告！");
            //    return;
            //}
            //if (listView4.Items.Count <= 0 || listView4.Items[0].SubItems[1].Text.Trim() == "")
            //{
            //    showAlertY("请上传增值税发票税率书面证明！");
            //    return;
            //}
            //if (listView5.Items.Count <= 0 || listView5.Items[0].SubItems[1].Text.Trim() == "")
            //{
            //    showAlertY("请上传商标与专利证书！");
            //    return;
            //}
            //if (listView6.Items.Count <= 0 || listView6.Items[0].SubItems[1].Text.Trim() == "")
            //{
            //    showAlertY("请上传危害报告及承诺！");
            //    return;
            //}
            //if (listView7.Items.Count <= 0 || listView7.Items[0].SubItems[1].Text.Trim() == "")
            //{
            //    showAlertY("请上传法定代表人承诺书！");
            //    return;
            //}
            #endregion

            //检查是否存在重复的商品+投标周期
            bool checkcf = FB.CheckChongFu(UCspbh.Text.Trim(), CBhhzq.Text.Trim(), this.Name);
            if (checkcf)
            {
                //请填写投标价格。
                showAlertY("您已向投标单添加过编号为" + UCspbh.Text.Trim() + "，合同周期为" + CBhhzq.Text.Trim() + "的投标信息，不能重复添加！");
                return;
            }
            //添加后处理按钮文字
            if (FB != null)
            {
                
                //显示投标单
                FB.ShowOrHideList(true,true);
                jrtbd.Texts = "确认修改";
            }
  

        }

        private void Lselect_Click(object sender, EventArgs e)
        {
            Hashtable HT = new Hashtable();
            HT["结算账户"] = "卖家";
            SelectTBWarse selectWarse = new SelectTBWarse(HT, new delegateForThread(Returnsomethings));
            selectWarse.ShowDialog();
        }
        //商品选择回调
        private void Returnsomethings(Hashtable return_ht)
        {
            //this.tb_bsh.Text = return_ht["测试值"].ToString();
            UCspbh.Text = return_ht["商品编号"].ToString();
            UCspmc.Text = return_ht["商品名称"].ToString();
            UCspgg.Text = return_ht["规格型号"].ToString();
            UCspdj.Text = return_ht["计价单位"].ToString();
            UCzdjjpl.Text = return_ht["最大经济批量"].ToString();
            CBhhzq.Text = return_ht["合同周期"].ToString();

            string dbstr = return_ht["最低价投标信息隐藏数据"].ToString();
            string[] dbstr_arr = dbstr.Split('*');
            if (dbstr_arr.Length == 4)
            {
                string showstr = "当前品类商品最低投标价：" + dbstr_arr[0] + "元；\r\n\r\n最低价投标拟售量" + dbstr_arr[1] + "" + UCspdj.Text + "；\r\n\r\n参与竞标集合预订量" + dbstr_arr[2] + "" + UCspdj.Text + "；\r\n\r\n最低价投标提货经济批量" + dbstr_arr[3] + "" + UCspdj.Text + "。";

                label3.Text = showstr;

                label3.Visible = true;
            }
            else
            {
                label3.Text = "";

                label3.Visible = false;
            }
        }

        //显示隐藏设置
        private void Lsd_Click(object sender, EventArgs e)
        {
            if (!GBbfhqysd.Visible)
            {
                GBbfhqysd.Visible = true;
                Lsd.Text = "隐藏设置";
            }
            else
            {
                GBbfhqysd.Visible = false;
                Lsd.Text = "显示设置";
            }
            
        }

        //有没有重复的不发货区域
        private bool ischongfu(string[] selectqy)
        {
            for (int i = 0; i < DGVbfhqy.Rows.Count; i++)
            {
                if (DGVbfhqy.Rows[i].Cells[1].Value.ToString() == selectqy[0] && DGVbfhqy.Rows[i].Cells[2].Value.ToString() == selectqy[1])
                {
                    return true;
                }
            }
            return false;
        }

        //添加
        private void Lbfhqytj_Click(object sender, EventArgs e)
        {
            string[] selectqy = ucCityList1.SelectedItem;
            if (selectqy[0] != "请选择省份" && selectqy[1] != "请选择城市")
            {
                if (!ischongfu(selectqy))
                {
                    DGVbfhqy.Rows.Add(new string[] { "",selectqy[0], selectqy[1] });
                    resetfhqy();
                }
            }
        }

        //删除
        private void DGVbfhqy_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex > -1)
            {
                DGVbfhqy.Rows.RemoveAt(e.RowIndex);
                resetfhqy();
            }
        }

        //重新设置发货区域输入框
        private void resetfhqy()
        {
            string bfhqy = "|";
            for (int i = 0; i < DGVbfhqy.Rows.Count; i++)
            {
                bfhqy = bfhqy + DGVbfhqy.Rows[i].Cells[1].Value.ToString() + DGVbfhqy.Rows[i].Cells[2].Value.ToString() + "|";
            }
            if (bfhqy == "|")
            {
                UCbfhqy.Text = "无";
            }
            else 
            {
                UCbfhqy.Text = bfhqy;
            }
        }

        //private void UCysl_Leave(object sender, EventArgs e)
        //{
        //    L33.Visible = false;
        //    string ysl = UCysl.Text;
        //    if (ysl != "" && UCzdjjpl.Text != "")
        //    {
        //        if (Convert.ToInt64(ysl) < Convert.ToInt64(UCzdjjpl.Text) * 10)
        //        {
        //            //投标拟售量须大于等于十倍最大经济批量
        //            L33.Visible = true;
        //            return;
        //        }
                
        //    }
        //}

        //private void UCjjpl_Leave(object sender, EventArgs e)
        //{
        //    L44.Visible = false;
        //    string jjpl = UCjjpl.Text;
        //    if (jjpl != "" && UCzdjjpl.Text != "")
        //    {
        //        if (Convert.ToInt64(jjpl) > Convert.ToInt64(UCzdjjpl.Text))
        //        {
        //            //提货经济批量须小于该产品最大经济批量
        //            L44.Visible = true;
        //            return;
        //        }

        //    }
        //}
        #region//上传质量标准与证明
        private void B_SC_Click(object sender, EventArgs e)
        {
            //开启上传
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string[] fName = openFileDialog1.FileNames;                
                
                //若选择选择对话框允许选择多个，则还要检验不能超过5个
                //多不是对话框选择的，是直接指定数组，则还要检验不能重名

                //开始上传,传入文件名数组，传入对应的上传结果控件，传入回调，传入角色编号
                FormSC FSC = new FormSC(fName, listView1, new delegateForSC(UpLoadSucceed), PublicDS.PublisDsUser.Tables[0].Rows[0]["卖家角色编号"].ToString().Trim());
                FSC.ShowDialog();
                 //有地址
                if (listView1.Items.Count > 0 && listView1.Items[0].SubItems[1].Text.Trim() != "")
                {
                    this.B_SCCK.Location = new Point(5, 4);
                    this.B_SCSC.Location = new Point(64, 4);
                    this.B_SCCK.Visible = true;
                    this.B_SCSC.Visible = true;
                    this.B_SC.Visible = false;
                }
            }
        }
        /// <summary>
        /// 若全部上传完成，在主窗体进行数据处理，根据情况编写，没有处理也要带着这个方法
        /// </summary>
        /// <param name="LV">上传结果集合</param>
        private void UpLoadSucceed(ListView LV)
        {
            //这里的LV,实际上是打开上传时指定的那个隐藏控件listView1，所以这个方法在多个上传按钮时，只需要写一次就行
            //MessageBox.Show("回调测试" + LV.Name);
        }
        //查看
        private void B_SCCK_Click(object sender, EventArgs e)
        {
            //有地址
            if (listView1.Items.Count > 0 && listView1.Items[0].SubItems[1].Text.Trim() != "")
            {
                string url = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[1] + "/JHJXPT/SaveDir/" + listView1.Items[0].SubItems[1].Text.Trim().Replace(@"\", "/");
                StringOP.OpenUrl(url);
            }
        }
        //删除
        private void B_SCSC_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            //有地址
            if (listView1.Items.Count > 0 && listView1.Items[0].SubItems[1].Text.Trim() != "")
            {
                this.B_SCCK.Visible = true;
                this.B_SCSC.Visible = true;
                this.B_SC.Visible = false;
            }
            else
            {
                this.B_SCCK.Visible = false;
                this.B_SCSC.Visible = false;
                this.B_SC.Visible = true;
            }
        }
        #endregion
        #region//上传增值税发票税率书面证明
        private void label20_Click(object sender, EventArgs e)
        {
            //开启上传
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string[] fName = openFileDialog1.FileNames;

                //若选择选择对话框允许选择多个，则还要检验不能超过5个
                //多不是对话框选择的，是直接指定数组，则还要检验不能重名

                //开始上传,传入文件名数组，传入对应的上传结果控件，传入回调，传入角色编号
                FormSC FSC = new FormSC(fName, listView2, new delegateForSC(UpLoadSucceed), PublicDS.PublisDsUser.Tables[0].Rows[0]["卖家角色编号"].ToString().Trim());
                FSC.ShowDialog();
                //有地址
                if (listView2.Items.Count > 0 && listView2.Items[0].SubItems[1].Text.Trim() != "")
                {
                    this.label22.Location = new Point(5, 4);
                    this.label19.Location = new Point(64, 4);
                    this.label22.Visible = true;
                    this.label19.Visible = true;
                    this.label20.Visible = false;
                }
            }
        }
        private void label22_Click(object sender, EventArgs e)
        {
            //有地址
            if (listView2.Items.Count > 0 && listView2.Items[0].SubItems[1].Text.Trim() != "")
            {
                string url = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[1] + "/JHJXPT/SaveDir/" + listView2.Items[0].SubItems[1].Text.Trim().Replace(@"\", "/");
                StringOP.OpenUrl(url);
            }
        }
        private void label19_Click(object sender, EventArgs e)
        {
            listView2.Items.Clear();
            //有地址
            if (listView2.Items.Count > 0 && listView2.Items[0].SubItems[1].Text.Trim() != "")
            {
                this.label22.Visible = true;
                this.label19.Visible = true;
                this.label20.Visible = false;
            }
            else
            {
                this.label22.Visible = false;
                this.label19.Visible = false;
                this.label20.Visible = true;
            }
        }
        #endregion
        #region//上传品管总负责人法律承诺书
        private void label24_Click(object sender, EventArgs e)
        {
            //开启上传
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string[] fName = openFileDialog1.FileNames;

                //若选择选择对话框允许选择多个，则还要检验不能超过5个
                //多不是对话框选择的，是直接指定数组，则还要检验不能重名

                //开始上传,传入文件名数组，传入对应的上传结果控件，传入回调，传入角色编号
                FormSC FSC = new FormSC(fName, listView3, new delegateForSC(UpLoadSucceed), PublicDS.PublisDsUser.Tables[0].Rows[0]["卖家角色编号"].ToString().Trim());
                FSC.ShowDialog();
                //有地址
                if (listView3.Items.Count > 0 && listView3.Items[0].SubItems[1].Text.Trim() != "")
                {
                    this.label25.Location = new Point(5, 4);
                    this.label23.Location = new Point(64, 4);
                    this.label25.Visible = true;
                    this.label23.Visible = true;
                    this.label24.Visible = false;
                }
            }
        }

        private void label25_Click(object sender, EventArgs e)
        {
            //有地址
            if (listView3.Items.Count > 0 && listView3.Items[0].SubItems[1].Text.Trim() != "")
            {
                string url = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[1] + "/JHJXPT/SaveDir/" + listView3.Items[0].SubItems[1].Text.Trim().Replace(@"\", "/");
                StringOP.OpenUrl(url);
            }
        }

        private void label23_Click(object sender, EventArgs e)
        {
            listView3.Items.Clear();
            //有地址
            if (listView3.Items.Count > 0 && listView3.Items[0].SubItems[1].Text.Trim() != "")
            {
                this.label25.Visible = true;
                this.label23.Visible = true;
                this.label24.Visible = false;
            }
            else
            {
                this.label25.Visible = false;
                this.label23.Visible = false;
                this.label24.Visible = true;
            }
        }
        #endregion
        #region//上传商标与专利证书
        private void label27_Click(object sender, EventArgs e)
        {
            //开启上传
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string[] fName = openFileDialog1.FileNames;

                //若选择选择对话框允许选择多个，则还要检验不能超过5个
                //多不是对话框选择的，是直接指定数组，则还要检验不能重名

                //开始上传,传入文件名数组，传入对应的上传结果控件，传入回调，传入角色编号
                FormSC FSC = new FormSC(fName, listView4, new delegateForSC(UpLoadSucceed), PublicDS.PublisDsUser.Tables[0].Rows[0]["卖家角色编号"].ToString().Trim());
                FSC.ShowDialog();
                //有地址
                if (listView4.Items.Count > 0 && listView4.Items[0].SubItems[1].Text.Trim() != "")
                {
                    this.label28.Location = new Point(5, 4);
                    this.label26.Location = new Point(64, 4);
                    this.label28.Visible = true;
                    this.label26.Visible = true;
                    this.label27.Visible = false;
                }
            }
        }

        private void label28_Click(object sender, EventArgs e)
        {
            //有地址
            if (listView4.Items.Count > 0 && listView4.Items[0].SubItems[1].Text.Trim() != "")
            {
                string url = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[1] + "/JHJXPT/SaveDir/" + listView4.Items[0].SubItems[1].Text.Trim().Replace(@"\", "/");
                StringOP.OpenUrl(url);
            }
        }

        private void label26_Click(object sender, EventArgs e)
        {
            listView4.Items.Clear();
            //有地址
            if (listView4.Items.Count > 0 && listView4.Items[0].SubItems[1].Text.Trim() != "")
            {
                this.label28.Visible = true;
                this.label26.Visible = true;
                this.label27.Visible = false;
            }
            else
            {
                this.label28.Visible = false;
                this.label26.Visible = false;
                this.label27.Visible = true;
            }
        }
        #endregion

        #region//上传产品技术与安全鉴定报告
        private void label31_Click(object sender, EventArgs e)
        {
            //开启上传
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string[] fName = openFileDialog1.FileNames;

                //若选择选择对话框允许选择多个，则还要检验不能超过5个
                //多不是对话框选择的，是直接指定数组，则还要检验不能重名

                //开始上传,传入文件名数组，传入对应的上传结果控件，传入回调，传入角色编号
                FormSC FSC = new FormSC(fName, listView5, new delegateForSC(UpLoadSucceed), PublicDS.PublisDsUser.Tables[0].Rows[0]["卖家角色编号"].ToString().Trim());
                FSC.ShowDialog();
                //有地址
                if (listView5.Items.Count > 0 && listView5.Items[0].SubItems[1].Text.Trim() != "")
                {
                    this.label32.Location = new Point(5, 4);
                    this.label29.Location = new Point(64, 4);
                    this.label32.Visible = true;
                    this.label29.Visible = true;
                    this.label31.Visible = false;
                }
            }
        }

        private void label32_Click(object sender, EventArgs e)
        {
            //有地址
            if (listView5.Items.Count > 0 && listView5.Items[0].SubItems[1].Text.Trim() != "")
            {
                string url = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[1] + "/JHJXPT/SaveDir/" + listView5.Items[0].SubItems[1].Text.Trim().Replace(@"\", "/");
                StringOP.OpenUrl(url);
            }
        }

        private void label29_Click(object sender, EventArgs e)
        {
            listView5.Items.Clear();
            //有地址
            if (listView5.Items.Count > 0 && listView5.Items[0].SubItems[1].Text.Trim() != "")
            {
                this.label32.Visible = true;
                this.label29.Visible = true;
                this.label31.Visible = false;
            }
            else
            {
                this.label32.Visible = false;
                this.label29.Visible = false;
                this.label31.Visible = true;
            }
        }
        #endregion

        #region//上传危害报告及承诺
        private void label34_Click(object sender, EventArgs e)
        {
            //开启上传
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string[] fName = openFileDialog1.FileNames;

                //若选择选择对话框允许选择多个，则还要检验不能超过5个
                //多不是对话框选择的，是直接指定数组，则还要检验不能重名

                //开始上传,传入文件名数组，传入对应的上传结果控件，传入回调，传入角色编号
                FormSC FSC = new FormSC(fName, listView6, new delegateForSC(UpLoadSucceed), PublicDS.PublisDsUser.Tables[0].Rows[0]["卖家角色编号"].ToString().Trim());
                FSC.ShowDialog();
                //有地址
                if (listView6.Items.Count > 0 && listView6.Items[0].SubItems[1].Text.Trim() != "")
                {
                    this.label35.Location = new Point(5, 4);
                    this.label33.Location = new Point(64, 4);
                    this.label35.Visible = true;
                    this.label33.Visible = true;
                    this.label34.Visible = false;
                }
            }
        }

        private void label35_Click(object sender, EventArgs e)
        {
            //有地址
            if (listView6.Items.Count > 0 && listView6.Items[0].SubItems[1].Text.Trim() != "")
            {
                string url = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[1] + "/JHJXPT/SaveDir/" + listView6.Items[0].SubItems[1].Text.Trim().Replace(@"\", "/");
                StringOP.OpenUrl(url);
            }
        }

        private void label33_Click(object sender, EventArgs e)
        {
            listView6.Items.Clear();
            //有地址
            if (listView6.Items.Count > 0 && listView6.Items[0].SubItems[1].Text.Trim() != "")
            {
                this.label35.Visible = true;
                this.label33.Visible = true;
                this.label34.Visible = false;
            }
            else
            {
                this.label35.Visible = false;
                this.label33.Visible = false;
                this.label34.Visible = true;
            }
        }
        #endregion
        #region//上传特殊许可文件
        private void label37_Click(object sender, EventArgs e)
        {
            //开启上传
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string[] fName = openFileDialog1.FileNames;

                //若选择选择对话框允许选择多个，则还要检验不能超过5个
                //多不是对话框选择的，是直接指定数组，则还要检验不能重名

                //开始上传,传入文件名数组，传入对应的上传结果控件，传入回调，传入角色编号
                FormSC FSC = new FormSC(fName, listView7, new delegateForSC(UpLoadSucceed), PublicDS.PublisDsUser.Tables[0].Rows[0]["卖家角色编号"].ToString().Trim());
                FSC.ShowDialog();
                //有地址
                if (listView7.Items.Count > 0 && listView7.Items[0].SubItems[1].Text.Trim() != "")
                {
                    this.label38.Location = new Point(5, 4);
                    this.label36.Location = new Point(64, 4);
                    this.label38.Visible = true;
                    this.label36.Visible = true;
                    this.label37.Visible = false;
                }
            }
        }

        private void label38_Click(object sender, EventArgs e)
        {
            //有地址
            if (listView7.Items.Count > 0 && listView7.Items[0].SubItems[1].Text.Trim() != "")
            {
                string url = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[1] + "/JHJXPT/SaveDir/" + listView7.Items[0].SubItems[1].Text.Trim().Replace(@"\", "/");
                StringOP.OpenUrl(url);
            }
        }

        private void label36_Click(object sender, EventArgs e)
        {
            listView7.Items.Clear();
            //有地址
            if (listView7.Items.Count > 0 && listView7.Items[0].SubItems[1].Text.Trim() != "")
            {
                this.label38.Visible = true;
                this.label36.Visible = true;
                this.label37.Visible = false;
            }
            else
            {
                this.label38.Visible = false;
                this.label36.Visible = false;
                this.label37.Visible = true;
            }
        }
        #endregion

        private void UCysl_KeyUp(object sender, KeyEventArgs e)
        {
            L33.Visible = false;
            string ysl = UCysl.Text;
            if (ysl != "" && UCzdjjpl.Text != "")
            {
                if (Convert.ToInt64(ysl) < Convert.ToInt64(UCzdjjpl.Text) * 10)
                {
                    //投标拟售量须大于等于十倍最大经济批量
                    L33.Visible = true;
                    return;
                }

            }
        }

        private void UCjjpl_KeyUp(object sender, KeyEventArgs e)
        {
            L44.Visible = false;
            string jjpl = UCjjpl.Text;
            if (jjpl != "" && UCzdjjpl.Text != "")
            {
                if (Convert.ToInt64(jjpl) > Convert.ToInt64(UCzdjjpl.Text))
                {
                    //提货经济批量须小于该产品最大经济批量
                    L44.Visible = true;
                    return;
                }

            }
        }
    }
}
