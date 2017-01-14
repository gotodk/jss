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

namespace 客户端主程序.SubForm.CenterForm.publicUC
{
    public partial class WHkaipaoxinxi : UserControl
    {
        string JSNM = "";
        DataSet dstest = null;
        public static List<string> ls=null;
        public WHkaipaoxinxi()
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

            //处理下拉框间距
            this.cboxfapiaolx.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.CB_DrawItem);
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

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        private void WHkaipaoxinxi_Load(object sender, EventArgs e)
        {
            GetKPData();
            lbNumber.Text = "";
            cboxfapiaolx.SelectedItem = "请选择";
        }        
      
        /// <summary>
        /// 搜索字符串,在s中搜索s1、s2之间的字符串，存入list
        /// </summary>
        /// <param name="s">目标字符串</param>
        /// <param name="s1">之前字符串</param>
        /// <param name="s2">之后字符串</param>
        /// <returns></returns>
        public static List<string> Search_string(string s, string s1, string s2)
        {
            if (ls == null)
            {
                ls=new List<string>();
            }            
            int n1, n2;  
            n1 = s.IndexOf(s1, 0) + s1.Length;   //开始位置  
            n2 = s.IndexOf(s2, n1);               //结束位置            
            if ( n2!=-1)
            {
                ls.Add( s.Substring(n1, n2 - n1)); //取搜索的条数，用结束的位置-开始的位置,添加到列表
                
                s = s.Substring(n2+s2.Length);

                Search_string(s, s1, s2);
            } 
            else
            {
                ls.Add(s.Substring(n1));
            }
           
            return ls;

        }  
        
        //获取初始开票信息的方法
        private void GetKPData()
        {
            Hashtable info = new Hashtable();
            info["where"] = "SELECT [Number],[DLZH],[YFPLX],[YKPXX],[XFPLX],[XKPXX],[SHZT],[SHSJ],[SHR],[SHJG],[FilePath] FROM [ZZ_KPXXBGSQB] WHERE [DLZH]='" + PublicDS.PublisDsUser.Tables[0].Rows[0]["DLYX"].ToString() + "'";

            //禁用提交区域并开启进度
            //panel1.Enabled = false;
            //panel2.Enabled = true;
            //resetloadLocation(ResetB, PBload);
            //开启线程提交数据              
            delegateForThread dft = new delegateForThread(ShowThreadResult_get);
            DataControl.RunThreadClassDataGet RTCTSOU = new DataControl.RunThreadClassDataGet(info, dft);
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
            
            PBload.Visible = false;           
            panelUC4.Dock = DockStyle.Top;
            panelUC4.Visible = true;
            //显示执行结果
            dstest =(DataSet)returnHT["返回数据"];
            if(dstest!=null && dstest.Tables[0].Rows.Count>0 )
            {              

                if (dstest.Tables[0].Rows[0]["XKPXX"] != null || dstest.Tables[0].Rows[0]["XKPXX"].ToString() != "")
                {
                    panelUC1.Visible = false;
                    panelUC1.Dock = DockStyle.Bottom;
                                      
                    txtdqfplx.Text = dstest.Tables[0].Rows[0]["XFPLX"].ToString();
                    txtdqkpxx.Text = dstest.Tables[0].Rows[0]["XKPXX"].ToString();
                    label16.Visible = false;
                    label17.Visible = false;
                    panelUC4.Height = panelUC4.Height - (pictureBox1.Location.Y-label16.Location.Y)-5;
                    pictureBox1.Location = new Point(pictureBox1.Location.X, label16.Location.Y);
             
                   
                    panelUC3.Dock = DockStyle.Fill;
                    panelUC3.Visible = true;
                  
                    if (dstest.Tables[0].Rows[0]["SHZT"].ToString() == "待审核")
                    {
                        lbinfo.Text = "您已经提交了开票信息，目前尚未审核，请稍等。";
                    }
                    else if (dstest.Tables[0].Rows[0]["SHZT"].ToString() == "未通过")
                    {
                        lbinfo.Text = "您提交的开票信息因为资料不完整没有通过审核，请点击“变更申请”按钮重新进行提交。";
                    }
                    else
                    {
                        lbinfo.Text = "您提交的开票信息已经通过了审核。如有变更，请提交变更申请。";
                    }
                    
                }
                else
                {
                    panelUC3.Visible = false;
                    panelUC3.Dock = DockStyle.Bottom;
                  
                    panelUC1.Dock = DockStyle.Fill;
                    panelUC1.Visible = true;
                  
                   
                  
                }               
            }
            else
            {
                panelUC3.Visible = false;
                panelUC3.Dock = DockStyle.Bottom;
               
                panelUC1.Dock = DockStyle.Fill;
                panelUC1.Visible = true;
            }
        }
        
        //保存按钮后置代码
        private void btnSave_Click(object sender, EventArgs e)
        {
            Hashtable info = new Hashtable();
            if (dstest != null && dstest.Tables[0].Rows.Count>0 )
            {
                info["原发票类型"] = dstest.Tables[0].Rows[0]["XFPLX"].ToString().Trim();
                info["原开票信息"] = dstest.Tables[0].Rows[0]["XKPXX"].ToString().Trim();
            }
            else
            {
                info["原发票类型"] = "无";
                info["原开票信息"] = "无";
            }
           
            info["新发票类型"] = cboxfapiaolx.SelectedItem.ToString().Trim();
            info["角色编号"] = JSNM;
            info["登录邮箱"] = PublicDS.PublisDsUser.Tables[0].Rows[0]["DLYX"].ToString().Trim();
            info["文件路径"] = txtimage.Text.Trim().Replace(@"\", "/");
            info["IsNum"] = this.lbNumber.Text.Trim();

            if (cboxfapiaolx.SelectedItem.ToString().Trim() == "请选择")
            {
                ArrayList Almsg3 = new ArrayList();
                Almsg3.Add("");
                Almsg3.Add("请先选择发票类型！");
                FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "错误", "提示", Almsg3);
                FRSE3.ShowDialog();
                return;
            }
            else if (cboxfapiaolx.SelectedItem.ToString().Trim() == "增值税专用发票")
            {
                if (txtdwmc.Text == "" || txtdz.Text == "" || txtlxdh.Text == "" || txtsbh.Text == "" || txtyhzh.Text == "" || txtimage.Text=="")
                {
                    ArrayList Almsg3 = new ArrayList();
                    Almsg3.Add("");
                    Almsg3.Add("标注*的字段必须填写！");
                    FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "错误", "提示", Almsg3);
                    FRSE3.ShowDialog();
                    return;
                }

                info["新开票信息"] = "单位名称：" + txtdwmc.Text.Trim() + "'+char(13)+char(10)+'" + "纳税人识别号：" + txtsbh.Text.Trim() + "'+char(13)+char(10)+'" + "地址：" + txtdz.Text.Trim() + "'+char(13)+char(10)+'" + "联系电话：" + txtlxdh.Text.Trim() + "'+char(13)+char(10)+'" + "开户行及账号：" + txtyhzh.Text.Trim();

                if (info["新开票信息"].ToString().Length > 250)
                {
                    ArrayList Almsg3 = new ArrayList();
                    Almsg3.Add("");
                    Almsg3.Add("以上所有信息不可超过250个汉字！");
                    FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "错误", "提示", Almsg3);
                    FRSE3.ShowDialog();
                    return;
                }
                else
                {
                    //禁用提交区域并开启进度                    
                    panelUC1.Enabled = false;
                    panelUC2.Enabled = false;
                    panelUC3.Enabled = false;                  
                    PBload.Visible = true;
                    resetloadLocation(ResetB, PBload);
                    
                    //开启线程提交数据              
                    delegateForThread dft = new delegateForThread(ShowThreadResult_save);
                    DataControl.RunThreadClassDataUpdate RTCTSOU = lbNumber.Text == "" ? new DataControl.RunThreadClassDataUpdate(info, dft, "fp_add") : new DataControl.RunThreadClassDataUpdate(info, dft, "fp_edit");
                    Thread trd = new Thread(new ThreadStart(RTCTSOU.BeginRun));
                    trd.IsBackground = true;
                    trd.Start();
                }


            }
            else if (cboxfapiaolx.SelectedItem.ToString().Trim() == "增值税普通发票")
            {
                if (txtdwmc.Text == "")
                {
                    ArrayList Almsg3 = new ArrayList();
                    Almsg3.Add("");
                    Almsg3.Add("标注*的字段必须填写！");
                    FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "错误", "提示", Almsg3);
                    FRSE3.ShowDialog();
                    return;
                }

                info["新开票信息"] = "单位名称：" + txtdwmc.Text.Trim();

                if (info["新开票信息"].ToString().Length > 250)
                {
                    ArrayList Almsg3 = new ArrayList();
                    Almsg3.Add("");
                    Almsg3.Add("不可超过250个汉字！");
                    FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "错误", "提示", Almsg3);
                    FRSE3.ShowDialog();
                    return;
                }
                else
                {
                    panelUC1.Enabled = false;
                    panelUC2.Enabled = false;
                    panelUC3.Enabled = false;
                    PBload.Visible = true;
                    resetloadLocation(ResetB, PBload);
                    delegateForThread dft = new delegateForThread(ShowThreadResult_save);
                    DataControl.RunThreadClassDataUpdate RTCTSOU = lbNumber.Text == "" ? new DataControl.RunThreadClassDataUpdate(info, dft, "fp_add") : new DataControl.RunThreadClassDataUpdate(info, dft, "fp_edit");
                    Thread trd = new Thread(new ThreadStart(RTCTSOU.BeginRun));
                    trd.IsBackground = true;
                    trd.Start();
                }
            }

        }

        /// <summary>
        /// 重设进度条位置，放到按钮旁边
        /// </summary>
        /// <param name="BB">参考提交按钮</param>
        /// <param name="PB">要移动的进度条</param>
        private void resetloadLocation(BasicButton BB, PictureBox PB)
        {
            PB.Location = new Point(BB.Location.X + BB.Width + 30, BB.Location.Y + 60);
            PB.Visible = true;
        }


        //取消按钮后置代码
        private void ResetB_Click(object sender, EventArgs e)
        {

            //重置本表单，或转向其他页面
            WHkaipaoxinxi UC = new WHkaipaoxinxi();//这句话是关键，若实例化本身的类，则是重置表单，若实例化其他的类，则是跳转
            reset(UC);
          
           

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
            panelUC1.Enabled = true;
            panelUC2.Enabled = true;
            panelUC3.Enabled = true;        
            PBload.Visible = false;
            //显示执行结果
            string state = returnHT["执行状态"].ToString();
            string method = returnHT["服务方法"].ToString();
            string str = "";

            switch (method)
            {
                case "fp_add":
                    str = "添加";
                    break;
                case "fp_edit":
                    str = "修改";
                    break;  
                default:
                    str = "处理";
                    break;
            }

            switch (state)
            {
                case "ok":

                    //给出表单提交成功的提示
                    ArrayList Almsg3 = new ArrayList();
                    Almsg3.Add("");
                    Almsg3.Add("您的开票信息已经" + str + "成功！");
                    FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "叹号", "处理成功", Almsg3);
                    FRSE3.ShowDialog();

                    //重置本表单，或转向其他页面
                    WHkaipaoxinxi UC = new WHkaipaoxinxi();//这句话是关键，若实例化本身的类，则是重置表单，若实例化其他的类，则是跳转
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
                    FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "错误", str + "开票信息失败", Almsg4);
                    FRSE4.ShowDialog();

                    break;
            }

        }
  

        /// <summary>
        /// 重置表单
        /// </summary>
        /// <param name="UC"></param>
        private void reset(WHkaipaoxinxi UC)
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

        //上传图片
        private void B_SC_Click(object sender, EventArgs e)
        {
            //删除已经无效的图片
            if (txtimage.Text.Trim() != "")
            {
                try
                {
                    DataControl.WebServicesCenter WSC = new DataControl.WebServicesCenter();
                    WSC.DelPicture( txtimage.Text.Trim().Replace(@"\", "/"));
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

            //开启上传
            openFileDialog1.ShowDialog();           

        }
        //查看图片
        private void B_SCCK_Click(object sender, EventArgs e)
        {
            //有地址
            if (txtimage.Text.Trim() != "")
            {
                string url = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[1] + "/JHJXPT/SaveDir/" + txtimage.Text.Trim().Replace(@"\", "/");
                //MessageBox.Show(url);
                StringOP.OpenUrl(url);

                //FormWebBrowser fwb = new FormWebBrowser();
                //fwb.webBrowser1.Url = new Uri(url);

                //fwb.ShowDialog();
            }
        }

        //删除图片
        private void B_SCSC_Click(object sender, EventArgs e)
        {
            ArrayList Almsgxx = new ArrayList();
            Almsgxx.Add("");
            Almsgxx.Add("您确定要删除已经上传的图片吗？");
            FormAlertMessage FRSExx = new FormAlertMessage("确定取消", "问号", "删除上传文件", Almsgxx);
            DialogResult dr = FRSExx.ShowDialog();
            if (dr == DialogResult.Yes)
            {
                //删除已经无效的图片
                if (txtimage.Text.Trim() != "")
                {
                    try
                    {
                        DataControl.WebServicesCenter WSC = new DataControl.WebServicesCenter();
                        WSC.DelPicture(txtimage.Text.Trim().Replace(@"\", "/"));
                    }
                    catch
                    {
                        ArrayList AlmsgMR = new ArrayList();
                        AlmsgMR.Add("");
                        AlmsgMR.Add("请检查网络，稍后再试！");
                        FormAlertMessage FRSEMR = new FormAlertMessage("仅确定", "叹号", "提醒", AlmsgMR);
                        FRSEMR.ShowDialog();
                    }

                    txtimage.Text = "";
                }
                listView1.Items.Clear();
            }
        }

        private void timer_SCCK_Tick(object sender, EventArgs e)
        {
            if (listView1.Items.Count > 0 || txtimage.Text.Trim() != "")
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

        /// <summary>
        /// 若全部上传完成，在主窗体进行数据处理，根据情况编写，没有处理也要带着这个方法
        /// </summary>
        /// <param name="LV">上传结果集合</param>
        private void UpLoadSucceed(ListView LV)
        {
            //这里的LV,实际上是打开上传时指定的那个隐藏控件listView1，所以这个方法在多个上传按钮时，只需要写一次就行
            //MessageBox.Show("回调测试" + LV.Name);
            this.txtimage.Text = LV.Items[LV.Items.Count - 1].SubItems[1].Text;
        }
       
        //变更申请按钮后置代码
        private void btnbiangeng_Click(object sender, EventArgs e)
        {
            panelUC3.Dock = DockStyle.Top;  
            panelUC5.Visible = true;
            panelUC5.Dock = DockStyle.Top;

            panelUC1.Dock = DockStyle.Fill;
            panelUC1.Visible = true;

            if (txtdqkpxx.Text.Trim()!="")
            {
                string s=txtdqkpxx.Text;
                //拆分字符串，取出“:”与“\r\n”字符之间的字符串，存入ls
                ls = Search_string(s, "：", "\r\n");

                //赋值
                cboxfapiaolx.SelectedItem = txtdqfplx.Text;
                txtdwmc.Text = ls[0].ToString().Trim();
                if(txtdqfplx.Text.Trim()=="增值税专用发票")
                {
                    txtsbh.Text = ls[1].ToString().Trim();
                    txtdz.Text = ls[2].ToString().Trim();
                    txtlxdh.Text = ls[3].ToString().Trim();
                    txtyhzh.Text = ls[4].ToString().Trim();
                    txtimage.Text = dstest.Tables[0].Rows[0]["FilePath"].ToString().Trim();
                }                
                
                //赋值结束后清空list
                ls = null;
                //读取图片路径
              
                lbNumber.Text = dstest.Tables[0].Rows[0]["Number"].ToString();

            }
          
        }
        //下拉框选择不同的发票类型呈现不同的样式
        private void cboxfapiaolx_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboxfapiaolx.SelectedItem.ToString() == "增值税普通发票")
            {
                label4.Visible = false;
                label5.Visible = false;
                label6.Visible = false;
                label7.Visible = false;
                label9.Visible = false;
                label10.Visible = false;
                label11.Visible = false;
                label12.Visible = false; 
                label13.Visible = false;
                label14.Visible = false;
                //label18.Visible = false;
               // label21.Visible = false;
                panelUC2.Visible = false;
                txtsbh.Visible = false;
                txtdz.Visible = false;
                txtlxdh.Visible = false;
                txtyhzh.Visible = false;

            }
            else
            {
                label4.Visible = true;
                label5.Visible = true;
                label6.Visible = true;
                label7.Visible = true;          
                label9.Visible = true;
                label10.Visible = true;
                label11.Visible = true;
                label12.Visible = true;
                label13.Visible = true;
                label14.Visible = true;
                //label18.Visible = true;
               // label21.Visible = true;
                panelUC2.Visible = true;
                txtsbh.Visible = true;
                txtdz.Visible = true;
                txtlxdh.Visible = true;
                txtyhzh.Visible = true;
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
               if (fi.Length > 1000*1024)
               {
                   ArrayList Almsg4 = new ArrayList();
                   Almsg4.Add("");
                   Almsg4.Add("文件大小不可超过1M！");// 这里是提交失败的提示，可能是程序出错，可能是条件不足等
                   FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "错误",  "提示", Almsg4);
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
