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
using System.Text.RegularExpressions;

namespace 客户端主程序.SubForm.CenterForm.publicUC
{
    public partial class BasicInfo : UserControl
    {
        public BasicInfo()
        {
            InitializeComponent();
            //根据登陆账号获取已经提交的资料信息及资料的审核状态
            //GetBasicInfo();
        }

        //加载窗体后处理数据
        private void UserControl1_Load(object sender, EventArgs e)
        {
            //必须先初始化省市区下拉框
            ucCityList1.initdefault();

            cmbJSZHLX.SelectedIndex = 0;
            cmbZCLB.SelectedIndex = 0;
            //根据结算账号类型的不同，显示不同的页面内容
            SetPageContent();

            //根据登陆账号获取已经提交的资料信息及资料的审核状态
            GetBasicInfo();

        }
        //根据不同的结算账户类型，分别显示不同的页面内容
        private void SetPageContent()
        {
            string JSZHLX = PublicDS.PublisDsUser.Tables[0].Rows[0]["JSZHLX"].ToString();
            switch (JSZHLX)
            {
                case "买家账户":
                    panelSFZSMJ.Visible = true;
                    this.panelZCLB.Visible = false;
                    this.panelUCGS.Visible = true;
                    this.panelUCGS.Location = new Point(10,293+66);
                    this.panelQTZZ.Visible = true;
                    this.panelQTZZ.Location = new Point(10, 293 + 66 + 267-30);
                    panelSFZSMJ.Location = new System.Drawing.Point(10, 293);
                    panelbutton.Location = new System.Drawing.Point(10, 293 + 66 + 267+33-30);

                    this.label29.Visible = false;
                    this.label31.Visible = false;
                    this.label40.Visible = false;
                    this.label32.Visible = false;
                    this.label33.Visible = false;
                    this.label36.Visible = false;
                    this.label37.Visible = false;
                    break;
                case "卖家账户":

                    panelSFZSMJ.Visible = false;
                    this.panelZCLB.Visible = false;
                    panelUCGS.Visible = true;
                    panelUCGS.Location = new System.Drawing.Point(10, 293);
                    panelQTZZ.Visible = true;
                    panelQTZZ.Location = new System.Drawing.Point(10, 560-30);
                    panelbutton.Location = new System.Drawing.Point(10, 593-30);


                    this.label29.Visible = true;
                    this.label31.Visible = true;
                    this.label40.Visible = true;
                    this.label32.Visible = true;
                    this.label33.Visible = true;
                    this.label36.Visible = true;
                    this.label37.Visible = true;

                    break;
                case "经纪人账户":
                    panelZCLB.Visible = true;

                    this.label29.Visible = true;
                    this.label31.Visible = true;
                    this.label40.Visible = true;
                    this.label32.Visible = true;
                    this.label33.Visible = true;
                    this.label36.Visible = true;
                    this.label37.Visible = true;
                    break;
                default:
                    break;
            }
        }


        #region 提交修改后的数据信息
        //提交数据
        private void btnCommit_Click(object sender, EventArgs e)
        {
            //获得表单数据和进行基本验证
            Hashtable ht_form = new Hashtable();
            ArrayList Almsg3 = new ArrayList();
            ht_form["str1"] = uctbYHM.Text.Trim();

            if (ht_form["str1"].ToString() == "")
            {
               
                Almsg3.Add("");
                Almsg3.Add("用户名不可为空！");
               
            }

            ht_form["角色编号"] = label_jsbh.Text;

            ht_form["结算账户类型"] = cmbJSZHLX.SelectedItem.ToString();

            if (uctbLXRXM.Text.Trim().Equals(""))
            {
                Almsg3.Add("");
                Almsg3.Add("联系人姓名尚未填写！");
            }
            else
            {
                if (this.uctbLXRXM.Text.Length > 60)
                {
                    Almsg3.Add("");
                    Almsg3.Add("联系人姓名长度不可超过60字！");

                }
                else
                {
                    Hashtable htjjryhm = new Hashtable();
                    htjjryhm["tszfcl"] = uctbLXRXM.Text.Trim();
                    if (ValStr.ValidateQuery(htjjryhm))
                    {
                        // "联系人姓名中不可包含['],[,]特殊字符！";
                        Almsg3.Add("");
                        Almsg3.Add("联系人姓名中不可包含['],[,]等特殊字符！");

                    }
                }
            }
            ht_form["联系人姓名"] = uctbLXRXM.Text.Trim();

            if (uctbSJHM.Text.Trim().Equals(""))
            {

                Almsg3.Add("");
                Almsg3.Add("手机号码尚未填写！");
            }
            else
            {
                if (!Regex.IsMatch(this.uctbSJHM.Text.Trim(), @"^(13|15|18)[0-9]{9}$"))
                {
                    Almsg3.Add("");
                    Almsg3.Add("手机号码格式不正确！");
                }
            }

            ht_form["手机号码"] = uctbSJHM.Text.Trim();

            if (ucCityList1.SelectedItem[0].ToString().Contains("请选择") || ucCityList1.SelectedItem[1].ToString().Contains("请选择") || ucCityList1.SelectedItem[2].ToString().Contains("请选择"))
            {
                Almsg3.Add("");
                Almsg3.Add("省市区信息填写不完整！");
            }

            ht_form["省"] = ucCityList1.SelectedItem[0];
            ht_form["市"] = ucCityList1.SelectedItem[1];
            ht_form["区"] = ucCityList1.SelectedItem[2];

            if (uctbXXDZ.Text.Trim().Equals(""))
            {
                Almsg3.Add("");
                Almsg3.Add("详细地址信息填写不完整！");
            }
            else
            {
                if (uctbXXDZ.Text.Length > 100)
                {
                    Almsg3.Add("");
                    Almsg3.Add("详细地址信息内容长度不可超过100字！");

                }
                else
                {
                    Hashtable htjjryhm = new Hashtable();
                    htjjryhm["tszfcl"] = uctbXXDZ.Text.Trim();
                    if (ValStr.ValidateQuery(htjjryhm))
                    {
                        // "联系人姓名中不可包含['],[,]特殊字符！";
                        Almsg3.Add("");
                        Almsg3.Add("详细地址中不可包含['],[,]等特殊字符！");

                    }
                }
            }



            ht_form["详细地址"] = uctbXXDZ.Text.Trim();

            if (uctbYZBM.Text.Trim().Equals(""))
            {
                Almsg3.Add("");
                Almsg3.Add("邮政编码项不可为空！");
            }
            else
            {
                if (!Support.ValStr.isPostalCode(uctbYZBM.Text.Trim()))
                {
                    Almsg3.Add("");
                    Almsg3.Add("邮政编码格式不正确！");
                }

            }
            ht_form["邮政编码"] = uctbYZBM.Text.Trim();

            if (panelZCLB.Visible)
            {
                if (cmbZCLB.SelectedIndex == 0)
                {
                    Almsg3.Add("");
                    Almsg3.Add("尚未选择注册类别！");
                    ht_form["注册类型"] = "";
                }
                else
                {
                    ht_form["注册类型"] = cmbZCLB.SelectedItem.ToString().Trim(); ;
                }
            }
            else
            {
                ht_form["注册类型"] = "";
            }


            if (Almsg3 != null && Almsg3.Count > 0)
            {
                FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "中国商品批发交易平台", "提示", Almsg3);
                FRSE3.ShowDialog();
                return;
            }

            if (panelSFZSMJ.Visible)
            {
                if (uctbSFZHM.Text.Equals(""))
                {
                    Almsg3.Add("");
                    Almsg3.Add("身份证号码尚未填写！");
                }
                else
                {
                    if (!Support.ValStr.isIDCard(uctbSFZHM.Text.Trim()))
                    {
                        Almsg3.Add("");
                        Almsg3.Add("身份证号码格式不正确！");
                    }

                }
                ht_form["身份证号码"] = uctbSFZHM.Text;

                if (listViewSFZH != null && listViewSFZH.Items.Count > 0)
                {
                    ht_form["身份证扫描件"] = listViewSFZH.Items[0].SubItems[1].Text;
                }
                else
                {
                    Almsg3.Add("");
                    Almsg3.Add("没有上传身份证复印件！");
                    ht_form["身份证扫描件"] = "";
                   
                }
            }
            else
            {
                ht_form["身份证号码"] = "";
                ht_form["身份证扫描件"] = "";
            }

            if (Almsg3 != null && Almsg3.Count > 0)
            {
                FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "中国商品批发交易平台", "提示", Almsg3);
                FRSE3.ShowDialog();
                return;
            }

            if (panelUCGS.Visible)
            {
                if (uctbGSMC.Text.Trim().Equals(""))
                {
                    Almsg3.Add("");
                    Almsg3.Add("公司名称尚未提交！");
                }
                else
                {
                    if (this.uctbGSMC.Text.Length > 100)
                    {
                        Almsg3.Add("");
                        Almsg3.Add("公司名称信息长度不可超过100字！");
                    }
                    else
                    {
                        Hashtable htjjryhm = new Hashtable();
                        htjjryhm["tszfcl"] = uctbGSMC.Text.Trim();
                        if (ValStr.ValidateQuery(htjjryhm))
                        {
                            // "联系人姓名中不可包含['],[,]特殊字符！";
                            Almsg3.Add("");
                            Almsg3.Add("公司名称中不可包含['],[,]等特殊字符！");

                        }
                    }
                }
                ht_form["公司名称"] = uctbGSMC.Text.Trim();

                if (uctbGSDH.Text.Trim().Equals(""))
                {
                    Almsg3.Add("");
                    Almsg3.Add("公司电话尚未填写！");
                }
                else
                {
                    if (this.uctbGSDH.Text.Length > 20)
                    {
                        Almsg3.Add("");
                        Almsg3.Add("公司电话长度不可超过20个字！");
                    }
                    else
                    {
                        Hashtable htjjryhm = new Hashtable();
                        htjjryhm["tszfcl"] = uctbGSDH.Text.Trim();
                        if (ValStr.ValidateQuery(htjjryhm))
                        {
                            // "联系人姓名中不可包含['],[,]特殊字符！";
                            Almsg3.Add("");
                            Almsg3.Add("公司名称中不可包含['],[,]等特殊字符！");

                        }
                    }
                }
                ht_form["公司电话"] = uctbGSDH.Text.Trim();

                if (uctbGSDZ.Text.Trim().Equals(""))
                {
                    Almsg3.Add("");
                    Almsg3.Add("公司地址尚未填写！");
                }
                else
                {
                    if (this.uctbGSDZ.Text.Length > 100)
                    {
                        Almsg3.Add("");
                        Almsg3.Add("公司地址信息长度不可超过100字！");
                    }
                    else
                    {
                        Hashtable htjjryhm = new Hashtable();
                        htjjryhm["tszfcl"] = uctbGSDZ.Text.Trim();
                        if (ValStr.ValidateQuery(htjjryhm))
                        {
                            // "联系人姓名中不可包含['],[,]特殊字符！";
                            Almsg3.Add("");
                            Almsg3.Add("公司地址中不可包含['],[,]等特殊字符！");

                        }
                    }
                }
                ht_form["公司地址"] = uctbGSDZ.Text.Trim();

                if (listViewYYZZ != null && listViewYYZZ.Items.Count > 0)
                {
                    ht_form["营业执照"] = listViewYYZZ.Items[0].SubItems[1].Text;
                }
                else
                {
                    Almsg3.Add("");
                    Almsg3.Add("营业执照扫面件尚未上传！");
                    ht_form["营业执照"] = "";
                }

                if (listViewZZJGDMZ != null && listViewZZJGDMZ.Items.Count > 0)
                {
                    ht_form["组织机构代码"] = listViewZZJGDMZ.Items[0].SubItems[1].Text;
                }
                else
                {
                    Almsg3.Add("");
                    Almsg3.Add("组织机构代码扫面件尚未上传！");
                    ht_form["组织机构代码"] = "";
                }

                if (listViewSWDJZ != null && listViewSWDJZ.Items.Count>0)
                {
                    ht_form["税务登记扫描件"] = listViewSWDJZ.Items[0].SubItems[1].Text;
                }
                else
                {
                    Almsg3.Add("");
                    Almsg3.Add("税务登记扫描件尚未上传！");
                    ht_form["税务登记扫描件"] = "";
                }

                if (listViewKHXKZ != null && listViewKHXKZ.Items.Count > 0)
                {
                    ht_form["开户许可证扫描件"] = listViewKHXKZ.Items[0].SubItems[1].Text;
                }
                else
                {
                    Almsg3.Add("");
                    Almsg3.Add("开户许可证扫描件尚未上传！");
                    ht_form["开户许可证扫描件"] = "";
                }

                if (listViewQZCNS != null && listViewQZCNS.Items.Count > 0)
                {
                    ht_form["承诺书"] = listViewQZCNS.Items[0].SubItems[1].Text; ;
                }
                else
                {
                    //Almsg3.Add("");
                    //Almsg3.Add("承诺书扫描件尚未上传！");
                    ht_form["承诺书"] = "";
                }

                if (this.cmbJSZHLX.SelectedIndex != 1)
                {
                    if (Almsg3 != null && Almsg3.Count > 0)
                    {
                        FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "中国商品批发交易平台", "提示", Almsg3);
                        FRSE3.ShowDialog();
                        return;
                    }
                }

            }
            else
            {
                ht_form["公司名称"] = "";
                ht_form["公司电话"] = "";
                ht_form["公司地址"] = "";
                ht_form["营业执照"] = "";
                ht_form["组织机构代码"] = "";
                ht_form["税务登记扫描件"] = "";
                ht_form["开户许可证扫描件"] = "";
                ht_form["承诺书"] = "";
 
            }

            if (panelQTZZ.Visible)
            {
                if (listViewQTZZ != null && listViewQTZZ.Items.Count > 0)
                {
                    ht_form["其他资质文件"] = listViewQTZZ.Items[0].SubItems[1].Text;
                }
                else
                {
                   
                    ht_form["其他资质文件"] = "";
                }

            }
            else
            {
                ht_form["其他资质文件"] = "";
            }

           
            //禁用提交区域并开启进度
            panelUC5.Enabled = false;
            resetloadLocation(ResetB, PBload);


            //开启线程提交数据
            delegateForThread dft = new delegateForThread(ShowThreadResult_save);
            DataControl.RunThreadClassTestSaveOrUpdate RTCTSOU = new DataControl.RunThreadClassTestSaveOrUpdate(ht_form, dft);
            Thread trd = new Thread(new ThreadStart(RTCTSOU.BeginRun));
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
            PBload.Visible = false;

            //显示执行结果
            string zt = returnHT["执行状态"].ToString();
            switch (zt)
            {
                case "True":

                    //给出表单提交成功的提示
                    ArrayList Almsg3 = new ArrayList();
                    Almsg3.Add("");
                    Almsg3.Add("信息修改成功！");
                    FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "叹号", "中国商品批发交易平台", Almsg3);
                    FRSE3.ShowDialog();



                    //重置本表单，或转向其他页面
                    BasicInfo UC = new BasicInfo();//这句话是关键，若实例化本身的类，则是重置表单，若实例化其他的类，则是跳转
                    reset(UC);

                    break;
                case "系统繁忙":
                    ArrayList Almsg7 = new ArrayList();
                    Almsg7.Add("");
                    Almsg7.Add("抱歉，系统繁忙，请稍后再试……");
                    FormAlertMessage FRSE7 = new FormAlertMessage("仅确定", "错误提示", "中国商品批发交易平台", Almsg7);
                    FRSE7.ShowDialog();
                    break;
                default:
                    ArrayList Almsg4 = new ArrayList();
                    Almsg4.Add("");
                    Almsg4.Add(zt);// 这里是提交失败的提示，可能是程序出错，可能是条件不足等
                    FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "错误提示", "中国商品批发交易平台", Almsg4);
                    FRSE4.ShowDialog();

                    break;
            }

        }
        #endregion

        /// <summary>
        /// 重置表单
        /// </summary>
        /// <param name="UC"></param>
        private void reset(BasicInfo UC)
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

        /// <summary>
        /// 重设进度条位置，放到按钮旁边
        /// </summary>
        /// <param name="BB">参考提交按钮</param>
        /// <param name="PB">要移动的进度条</param>
        private void resetloadLocation(BasicButton BB, PictureBox PB)
        {
            PB.Location = new Point(BB.Parent .Location.X +BB.Location.X + BB.Width + 30, BB.Parent.Location.Y+3);
            PB.Visible = true;
        }

        private void ResetB_Click(object sender, EventArgs e)
        {
            //重置本表单，或转向其他页面
            BasicInfo UC = new BasicInfo();//这句话是关键，若实例化本身的类，则是重置表单，若实例化其他的类，则是跳转
            reset(UC);
        }

        #region 上传控件的具体操作
        //上传
        private void B_SC_Click_1(object sender, EventArgs e)
        {
            //开启上传
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string[] fName = openFileDialog1.FileNames;

                //若选择选择对话框允许选择多个，则还要检验不能超过5个
                //多不是对话框选择的，是直接指定数组，则还要检验不能重名

                ListView LV = new ListView();
                Label lab = (Label)sender;
                string lab_name = lab.Name.ToString();
                switch (lab_name)
                {
                    case "labelSC_SFZ":
                        LV = listViewSFZH;
                        break;
                    case "labelSC_YYZZ":
                        LV = listViewYYZZ;
                        break;
                    case "labelSC_ZZJGDMZ":
                        LV = listViewZZJGDMZ;
                        break;
                    case "labelSC_SWDJZ":
                        LV = listViewSWDJZ;
                        break;
                    case "labelSC_KHXKZ":
                        LV = listViewKHXKZ;
                        break;
                    case "labelSC_QZCNS":
                        LV = listViewKHXKZ;
                        break;
                    case "labelSC_QTZZ":
                        LV = listViewQTZZ;
                        break;
                    default:
                        break;
                }

                //开始上传,传入文件名数组，传入对应的上传结果控件，传入回调，传入角色编号
                FormSC FSC = new FormSC(fName, LV, new delegateForSC(UpLoadSucceed), "测试角色编号12345678");
                FSC.ShowDialog();

            }
        }

        //查看
        private void B_SCCK_Click_1(object sender, EventArgs e)
        {
            ListView LV = new ListView();
            string lab_name = ((Label)sender).Name;
            switch (lab_name)
            {
                case "labelCK_SFZ":
                    LV = listViewSFZH;
                    break;
                case "labelCK_YYZZ":
                    LV = listViewYYZZ;
                    break;
                case "labelCK_ZZJGDMZ":
                    LV = listViewZZJGDMZ;
                    break;
                case "labelCK_SWDJZ":
                    LV = listViewSWDJZ;
                    break;
                case "labelCK_KHXKZ":
                    LV = listViewKHXKZ;
                    break;
                case "labelCK_QZCNS":
                    LV = listViewKHXKZ;
                    break;
                case "labelCK_QTZZ":
                    LV = listViewQTZZ;
                    break;
                default:
                    break;
            }
            //有地址
            if (LV.Items.Count > 0 && LV.Items[0].SubItems[1].Text.Trim() != "")
            {
                string url = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[1] + "/JHJXPT/SaveDir/" + LV.Items[0].SubItems[1].Text.Trim().Replace(@"\", "/");
                StringOP.OpenUrl(url);
            }
        }

        //删除图片
        private void B_SCSC_Click(object sender, EventArgs e)
        {
            ListView LV = new ListView();
            string lab_name = ((Label)sender).Name;
            string deletetype = "";
            switch (lab_name)
            {
                case "labelDel_SFZ":
                    LV = listViewSFZH;
                    deletetype = "身份证复印件";
                    break;
                case "labelDel_YYZZ":
                    LV = listViewYYZZ;
                    deletetype = "营业执照文件扫描件";
                    break;
                case "labelDel_ZZJGDMZ":
                    LV = listViewZZJGDMZ;
                    deletetype = "组织机构代码证扫描件";
                    break;
                case "labelDel_SWDJZ":
                    LV = listViewSWDJZ;
                    deletetype = "税务登记证扫描件";
                    break;
                case "labelDel_KHXKZ":
                    LV = listViewKHXKZ;
                    deletetype = "开户许可证扫描件";
                    break;
                case "labelDel_QZCNS":
                    LV = listViewQZCNS;
                    deletetype = "签字承诺书扫描件";
                    break;
                case "labelDel_QTZZ":
                    LV = listViewQTZZ;
                    deletetype = "其他资质文件";
                    break;
                default:
                    break;
            }
            if (LV != null)
            {
                ArrayList Almsg3 = new ArrayList();
                Almsg3.Add("");
                Almsg3.Add("你确定要删除" + deletetype + "吗？");
                FormAlertMessage FRSE3 = new FormAlertMessage("确定取消", "问号", "中国商品批发交易平台", Almsg3);
                DialogResult db = FRSE3.ShowDialog();
                //FRSE3.
                if (db == DialogResult.Yes)
                {
                    LV.Items.Clear();
                }
               // LV.Items.Clear();
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

        //根据上传数据处理查看和删除按钮
        private void timer_SCCK_Tick(object sender, EventArgs e)
        {
            //身份证扫描件的上传
            if (listViewSFZH.Items.Count > 0)
            {
                labelCK_SFZ.Visible = true;
                if (listViewSFZH.Items[0].SubItems[2].Text .ToString() == "可修改")
                {
                    labelDel_SFZ.Visible = true;
                }
            }
            else
            {
                labelCK_SFZ.Visible = false;
                labelDel_SFZ.Visible = false;
            }

            //营业执照
            if (listViewYYZZ.Items.Count > 0 )
            {
                labelCK_YYZZ.Visible = true;
                if (listViewYYZZ.Items[0].SubItems[2].Text.ToString() == "可修改")
                {
                    labelDel_YYZZ.Visible = true;
                }
            }
            else
            {
                labelCK_YYZZ.Visible = false;
                labelDel_YYZZ.Visible = false;
            }

            //组织机构代码证
            if (listViewZZJGDMZ.Items.Count > 0)
            {
                labelCK_ZZJGDMZ.Visible = true;
                if (listViewZZJGDMZ.Items[0].SubItems[2].Text.ToString() == "可修改")
                {
                    labelDel_ZZJGDMZ.Visible = true;
                }
            }
            else
            {
                labelCK_ZZJGDMZ.Visible = false;
                labelDel_ZZJGDMZ.Visible = false;
            }

            //税务登记证
            if (listViewSWDJZ.Items.Count > 0 )
            {
                labelCK_SWDJZ.Visible = true;
                if (listViewSWDJZ.Items[0].SubItems[2].Text.ToString() == "可修改")
                {
                    labelDel_SWDJZ.Visible = true;
                }
            }
            else
            {
                labelCK_SWDJZ.Visible = false;
                labelDel_SWDJZ.Visible = false;
            }

            //开户许可证
            if (listViewKHXKZ.Items.Count > 0)
            {
                labelCK_KHXKZ.Visible = true;
                if (listViewKHXKZ.Items[0].SubItems[2].Text.ToString() == "可修改")
                {
                    labelDel_KHXKZ.Visible = true;
                }
            }
            else
            {
                labelCK_KHXKZ.Visible = false;
                labelDel_KHXKZ.Visible = false;
            }

            //签字承诺书
            if (listViewQZCNS.Items.Count > 0)
            {
                labelCK_QZCNS.Visible = true;
                if (listViewQZCNS.Items[0].SubItems[2].Text.ToString() == "可修改")
                {
                    labelDel_QZCNS.Visible = true;
                }
            }
            else
            {
                labelCK_QZCNS.Visible = false;
                labelDel_QZCNS.Visible = false;
            }

            //其他资质
            if (listViewQTZZ.Items.Count > 0)
            {
                labelCK_QTZZ.Visible = true;
                if (listViewQTZZ.Items[0].SubItems[2].Text.ToString() == "可修改")
                {
                    labelDel_QTZZ.Visible = true;
                }
            }
            else
            {
                labelCK_QTZZ.Visible = false;
                labelDel_QTZZ.Visible = false;
            }
        }
        #endregion

       

        #region 获取登陆者的基本信息及相关审核状态等
        //提交数据
        private void GetBasicInfo()
        {
            //获得表单数据和进行基本验证
            Hashtable ht_form = new Hashtable();
            ht_form["dlyx"] = PublicDS.PublisDsUser.Tables[0].Rows[0]["DLYX"].ToString();
            ht_form["结算账户类型"] = PublicDS.PublisDsUser.Tables[0].Rows[0]["JSZHLX"].ToString();           

            //禁用提交区域并开启进度
            panelUC5.Enabled = false;
            resetloadLocation(ResetB, PBload);

            //开启线程提交数据
            if (ht_form["结算账户类型"].ToString() != "")
            {
                delegateForThread dft = new delegateForThread(ShowThreadResult_GetBasicInfo);
                DataControl.RunThreaClassJszhsq RTCTSOU = new DataControl.RunThreaClassJszhsq(ht_form, dft);
                Thread trd = new Thread(new ThreadStart(RTCTSOU.BeginRun_GetBasicInfo));
                trd.IsBackground = true;
                trd.Start();
            }

        }

        //显示线程处理结果的函数(用于处理线程返回数据)
        private void ShowThreadResult_GetBasicInfo(Hashtable returnHT)
        {
            try
            {
                //调用异步委托
                Invoke(new delegateForThreadShow(ShowThreadResult_GetBasicInfo_Invoke), new Hashtable[] { returnHT });
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("委托回调错误：" + ex.ToString());
            }
        }
        //处理非线程创建的控件
        private void ShowThreadResult_GetBasicInfo_Invoke(Hashtable returnHT)
        {

            this.label29.Visible = true;
            this.label31.Visible = true;
            this.label40.Visible = true;
            this.label32.Visible = true;
            this.label33.Visible = true;
            this.label36.Visible = true;
            this.label37.Visible = true;

            //重新开放提交区域
            panelUC5.Enabled = true;
            PBload.Visible = false;



            UCdizhi.Text = "";
            groupBox1.Visible = false;

            DataSet dsResult = (DataSet)returnHT["执行状态"];
            if (dsResult == null)
            {             
                ArrayList Almsg4 = new ArrayList();
                Almsg4.Add("");
                Almsg4.Add("未获取到任何基本资料与资质信息！");// 这里是提交失败的提示，可能是程序出错，可能是条件不足等
                FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "错误", "中国商品批发交易平台", Almsg4);
                FRSE4.ShowDialog();
            }            
            else if (dsResult != null && dsResult.Tables.Count == 2 && dsResult.Tables["执行情况"].Rows[0]["执行状态"].ToString() == "ok" && dsResult.Tables["数据"].Rows.Count == 1)
            {//如果获取到了基本资料和资质数据

                cmbJSZHLX.Enabled = false;
                uctbYHM.Enabled = false;
                uctbZCYX.Enabled = false;
                cmbZCLB.Enabled = false;
                string JSZHLX = PublicDS.PublisDsUser.Tables[0].Rows[0]["JSZHLX"].ToString();

                string Strdate = "可修改";

                if (JSZHLX.Equals("卖家账户") || JSZHLX.Equals("买家账户"))
                {
                    if (dsResult.Tables["数据"].Rows[0]["审核状态"].ToString() == "审核通过" && dsResult.Tables["数据"].Rows[0]["分公司审核状态"].ToString() == "审核通过")
                    {
                        Strdate = "审核通过";
                    }
                    else if (dsResult.Tables["数据"].Rows[0]["审核状态"].ToString() == "审核通过" && dsResult.Tables["数据"].Rows[0]["分公司审核状态"].ToString() != "审核通过")
                    {
                        Strdate = "经纪人审核通过";
                    }
                }
                if (JSZHLX.Equals("经纪人账户"))
                {
                    if (dsResult.Tables["数据"].Rows[0]["分公司审核状态"].ToString() == "审核通过" )
                    {
                        Strdate = "审核通过";
                    }

                   
                }

                #region 根据审核状态，判断控件的可用及可见状态
                if (Strdate == "审核通过" || Strdate == "经纪人审核通过")
                {
                    //三个账户类型都存在的字段
                    uctbLXRXM.Enabled = false;                  
                    if (dsResult.Tables["数据"].Rows[0]["是否验证手机号"].ToString() == "否")
                    {                       
                        //labelYZSJ.Visible = true;
                        labelYZSJ.Enabled = true;
                    }
                    else
                    {
                        uctbSJHM.Enabled = false; 
                        //labelYZSJ.Visible = false;
                    }
                    uctbSFZHM.Enabled = false;
                    ucCityList1.Enabled = false;
                    uctbXXDZ.Enabled = false;
                    uctbYZBM.Enabled = false;
                    uctbGSMC.Enabled = false;
                    uctbGSDZ.Enabled = false;
                    uctbGSDH.Enabled = false;

                    labelSC_SFZ.Visible = false;                 
                    labelSC_YYZZ.Visible = false;                    
                    labelSC_ZZJGDMZ.Visible = false;                  
                    labelSC_SWDJZ.Visible  = false;                   
                    labelSC_KHXKZ.Visible = false;                  
                    labelSC_QZCNS.Visible = false;                   
                    labelSC_QTZZ.Visible = false;

                    labelCK_SFZ.Left  = 5;
                    labelCK_YYZZ.Left  = 5;
                    labelCK_ZZJGDMZ.Left = 5;
                    labelCK_SWDJZ.Left = 5;
                    labelCK_KHXKZ.Left = 5;
                    labelCK_QZCNS.Left = 5;
                    labelCK_QTZZ.Left = 5;

                    labelDel_SFZ.Visible = false;
                    labelDel_YYZZ.Visible = false;
                    labelDel_ZZJGDMZ.Visible = false;
                    labelDel_SWDJZ.Visible = false;
                    labelDel_KHXKZ.Visible = false;
                    labelDel_QZCNS.Visible = false;
                    labelDel_QTZZ.Visible = false;
                    panelbutton.Visible = false;   

                    SetPageContent();
                    if (Strdate == "经纪人审核通过")
                    {
                        if (dsResult.Tables["数据"].Rows[0]["审核状态"].ToString() == "驳回")
                        {
                            UCdizhi.Text = "经纪人审核失败原因：\r\n" + dsResult.Tables["数据"].Rows[0]["审核信息"].ToString() + "。\r\n";
                            groupBox1.Visible = true;
                        }
                        if (dsResult.Tables["数据"].Rows[0]["审核状态"].ToString() == "审核通过")
                        {
                            UCdizhi.Text = "经纪人审核通过，待分公司审核后，方可正常使用。";
                            groupBox1.Visible = true;
                        }
                        //经纪人和分公司均驳回时候，显示分公司驳回原因
                        if (dsResult.Tables["数据"].Rows[0]["审核状态"].ToString() == "驳回" && dsResult.Tables["数据"].Rows[0]["分公司审核状态"].ToString() == "驳回" && !dsResult.Tables["数据"].Rows[0]["分公司审核信息"].ToString().Trim().Equals(""))
                        {
                            UCdizhi.Text = "分公司审核失败原因：\r\n" + dsResult.Tables["数据"].Rows[0]["分公司审核信息"].ToString() + "。\r\n";
                            groupBox1.Visible = true;
                        }
                    }
                }
                else
                {
                    if (dsResult.Tables["数据"].Rows[0]["审核状态"].ToString() == "驳回")
                    {
                        UCdizhi.Text = "经纪人审核失败原因：\r\n" + dsResult.Tables["数据"].Rows[0]["审核信息"].ToString() + "。\r\n";
                        groupBox1.Visible = true;
                    }
                    if (dsResult.Tables["数据"].Rows[0]["审核状态"].ToString() == "审核通过")
                    {
                        UCdizhi.Text = "经纪人审核通过，待分公司审核后，方可正常使用。";
                        groupBox1.Visible = true;
                    }
                    //经纪人和分公司均驳回时候，显示分公司驳回原因
                    if (dsResult.Tables["数据"].Rows[0]["审核状态"].ToString() == "驳回" && dsResult.Tables["数据"].Rows[0]["分公司审核状态"].ToString() == "驳回" && !dsResult.Tables["数据"].Rows[0]["分公司审核信息"].ToString().Trim().Equals(""))
                    {
                        UCdizhi.Text = "分公司审核失败原因：\r\n" + dsResult.Tables["数据"].Rows[0]["分公司审核信息"].ToString() + "。\r\n";
                        groupBox1.Visible = true;
                    }
              

                    
                    
                 

                }

                if (JSZHLX.Equals("经纪人账户"))
                {
                    if (dsResult.Tables["数据"].Rows[0]["分公司审核状态"].ToString() == "驳回" && !dsResult.Tables["数据"].Rows[0]["分公司审核信息"].ToString().Trim().Equals(""))
                    {
                        UCdizhi.Text += "分公司审核失败原因：\r\n" + dsResult.Tables["数据"].Rows[0]["分公司审核信息"].ToString() + "。";
                        groupBox1.Visible = true;
                    }
                }
                #endregion
             
                #region 给页面中的控件以及图片对应的listview对应赋值
                cmbJSZHLX.SelectedItem = JSZHLX;
                uctbYHM.Text = dsResult.Tables["数据"].Rows[0]["用户名"].ToString();
                uctbZCYX.Text = dsResult.Tables["数据"].Rows[0]["登陆邮箱"].ToString();
              //  uctbZCYX.Width = dsResult.Tables["数据"].Rows[0]["登陆邮箱"].ToString().Length + 50;
                uctbLXRXM.Text = dsResult.Tables["数据"].Rows[0]["联系人姓名"].ToString();
                uctbSJHM.Text = dsResult.Tables["数据"].Rows[0]["手机号"].ToString();
                uctbSFZHM.Text = dsResult.Tables["数据"].Rows[0]["身份证号"].ToString();
                ucCityList1.SelectedItem = new string[] { dsResult.Tables["数据"].Rows[0]["所属省份"].ToString(), dsResult.Tables["数据"].Rows[0]["所属地市"].ToString(), dsResult.Tables["数据"].Rows[0]["所属区县"].ToString() };
                uctbXXDZ.Text = dsResult.Tables["数据"].Rows[0]["详细地址"].ToString();
                uctbYZBM.Text = dsResult.Tables["数据"].Rows[0]["邮政编码"].ToString();
                uctbGSMC.Text = dsResult.Tables["数据"].Rows[0]["公司名称"].ToString();
                uctbGSDH.Text = dsResult.Tables["数据"].Rows[0]["公司电话"].ToString();
                uctbGSDZ.Text = dsResult.Tables["数据"].Rows[0]["公司地址"].ToString();
                label_jsbh.Text = dsResult.Tables["数据"].Rows[0]["角色编号"].ToString();
                if (dsResult.Tables["数据"].Rows[0]["注册类别"].ToString() != "")
                {
                    //string test = dsResult.Tables["数据"].Rows[0]["注册类别"].ToString();
                    cmbZCLB.SelectedItem = dsResult.Tables["数据"].Rows[0]["注册类别"].ToString();
                    if (dsResult.Tables["数据"].Rows[0]["注册类别"].ToString().Equals("个人注册"))
                    {
                        panelSFZSMJ.Visible = true;
                        this.panelZCLB.Visible = true;

                        this.panelUCGS.Visible = false;
                        this.panelQTZZ.Visible = false;

                       // panelSFZSMJ.Location = new System.Drawing.Point(10, 293);
                        panelbutton.Location = new System.Drawing.Point(10, 389);
                    }
                    else if (dsResult.Tables["数据"].Rows[0]["注册类别"].ToString().Equals("企业注册"))
                    {
                        panelSFZSMJ.Visible = false;
                        this.panelZCLB.Visible = true;
                        panelUCGS.Visible = true;

                        panelUCGS.Location = new System.Drawing.Point(10, 323);
                        panelQTZZ.Visible = true;
                        panelQTZZ.Location = new System.Drawing.Point(10, 590-23);
                        panelbutton.Location = new System.Drawing.Point(10, 623-23);
                     
                    }  
                    //panelQTZZ.Visible = false;

                }
                else
                {
                    if (JSZHLX.Trim().Equals("买家账户"))
                    {
                        //panelSFZSMJ.Visible = true;
                        //this.panelZCLB.Visible = false;
                        //this.panelUCGS.Visible = false;
                        //this.panelQTZZ.Visible = false;

                        //panelSFZSMJ.Location = new System.Drawing.Point(10, 293);
                        //panelbutton.Location = new System.Drawing.Point(10, 359);

                        panelSFZSMJ.Visible = true;
                        this.panelZCLB.Visible = false;
                        this.panelUCGS.Visible = true;
                        this.panelUCGS.Location = new Point(10, 293 + 66-10);
                        this.panelQTZZ.Visible = true;
                        this.panelQTZZ.Location = new Point(10, 293 + 66 + 267-30-20);
                        panelSFZSMJ.Location = new System.Drawing.Point(10, 293-13);
                        panelbutton.Location = new System.Drawing.Point(10, 293 + 66 + 267 + 33-30);

                        this.label29.Visible = false;
                        this.label31.Visible = false;
                        this.label40.Visible = false;
                        this.label32.Visible = false;
                        this.label33.Visible = false;
                        this.label36.Visible = false;
                        this.label37.Visible = false;
                    }
                    else
                    {
                        panelSFZSMJ.Visible = false;
                        this.panelZCLB.Visible = false;
                        panelUCGS.Visible = true;
                        panelUCGS.Location = new System.Drawing.Point(10, 283);
                        panelQTZZ.Visible = true;
                        panelQTZZ.Location = new System.Drawing.Point(10, 560-30-15);
                        panelbutton.Location = new System.Drawing.Point(10, 593-30);
                    }
 
                }
                if (dsResult.Tables["数据"].Rows[0]["身份证扫描件"].ToString() != "")
                {
                    listViewSFZH.Items.Add(new ListViewItem(new string[] { "", dsResult.Tables["数据"].Rows[0]["身份证扫描件"].ToString(), Strdate, "", "", "", "" }));
                }
                if (dsResult.Tables["数据"].Rows[0]["营业执照"].ToString() != "")
                {
                    listViewYYZZ.Items.Add(new ListViewItem(new string[] { "", dsResult.Tables["数据"].Rows[0]["营业执照"].ToString(), Strdate, "", "", "", "" }));
                }
                if (dsResult.Tables["数据"].Rows[0]["组织机构代码证"].ToString() != "")
                {
                    listViewZZJGDMZ.Items.Add(new ListViewItem(new string[] { "", dsResult.Tables["数据"].Rows[0]["组织机构代码证"].ToString(), Strdate, "", "", "", "" }));
                }
                if (dsResult.Tables["数据"].Rows[0]["税务登记证"].ToString() != "")
                {
                    listViewSWDJZ.Items.Add(new ListViewItem(new string[] { "", dsResult.Tables["数据"].Rows[0]["税务登记证"].ToString(), Strdate, "", "", "", "" }));
                }
                if (dsResult.Tables["数据"].Rows[0]["开户许可证"].ToString() != "")
                {
                    listViewKHXKZ.Items.Add(new ListViewItem(new string[] { "", dsResult.Tables["数据"].Rows[0]["开户许可证"].ToString(), Strdate, "", "", "", "" }));
                }
                if (dsResult.Tables["数据"].Rows[0]["签字承诺书"].ToString() != "")
                {
                    listViewQZCNS.Items.Add(new ListViewItem(new string[] { "", dsResult.Tables["数据"].Rows[0]["签字承诺书"].ToString(), Strdate, "", "", "", "" }));
                }
                if (dsResult.Tables["数据"].Rows[0]["其他资质"].ToString() != "")
                {
                    listViewQTZZ.Items.Add(new ListViewItem(new string[] { "", dsResult.Tables["数据"].Rows[0]["其他资质"].ToString(), Strdate, "", "", "", "" }));
                }
                #endregion
            }
            else
            {                
                ArrayList Almsg4 = new ArrayList();
                Almsg4.Add("");
                Almsg4.Add(dsResult.Tables["执行情况"].Rows[0]["执行结果"].ToString());// 这里是提交失败的提示，可能是程序出错，可能是条件不足等
                FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "错误", "中国商品批发交易平台", Almsg4);
                FRSE4.ShowDialog();
            }
        }
        #endregion
      
        private void cmbZCLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cmbZCLB.SelectedItem.ToString())
            { 
                case "请选择":
                    panelSFZSMJ.Visible = false;
                    panelUCGS.Visible = false;
                    panelbutton.Location = new System.Drawing.Point(10, 395-30);
                    break;
                case "个人注册":
                    panelSFZSMJ.Visible = true;
                    panelUCGS.Visible = false;
                    panelbutton.Location = new System.Drawing.Point(10, 400-30);
                    break;
                case "公司注册":
                    panelSFZSMJ.Visible = false;
                    panelUCGS.Visible = true;
                    panelUCGS.Location = new System.Drawing.Point(10, 356);
                    panelbutton.Location = new System.Drawing.Point(10, 632-30);
                    break;
                default :
                    break;
            }
            
        }

        private void labelYZSJ_Click(object sender, EventArgs e)
        {
            ArrayList Almsg3 = new ArrayList();
            Almsg3.Add("");
            Almsg3.Add("手机验证暂未开通！");
            FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "叹号", "中国商品批发交易平台", Almsg3);
            FRSE3.ShowDialog();
        }
    }
}
