using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Threading;
using 客户端主程序.NewDataControl;
using Com.Seezt.Skins;
using 客户端主程序.DataControl;
namespace 客户端主程序.SubForm.NewCenterForm
{
    public partial class UCCtongji : UserControl
    {
       
        public UCCtongji()
        {
            InitializeComponent();
            ////给项目、性质下拉框赋值  
            //Hashtable InputHT = new Hashtable();
            //InputHT["dlyx"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString();
            //InputHT["type"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["结算账户类型"].ToString();
            //SRT_GetXXInfo_Run(InputHT);
        }

        private void UCCtongji_Load(object sender, EventArgs e)
        {
            try
            {                
                Hashtable InputHT = new Hashtable();
                InputHT["dlyx"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString();
                InputHT["type"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["结算账户类型"].ToString();
                SRT_GetXXInfo_Run(InputHT);

            }
            catch (Exception ex)
            {
                ;
            }

        }


        private void SRT_GetXXInfo_Run(Hashtable InPutHT)
        {
            RunThreadClassTongJi OTD = new RunThreadClassTongJi(InPutHT, new delegateForThread(SRT_GetXXInfo));
            Thread trd = new Thread(new ThreadStart(OTD.BeginRun));
            trd.IsBackground = true;
            trd.Start();
        }
        //显示线程处理结果的函数(用于处理线程返回数据),调用异步委托
        private void SRT_GetXXInfo(Hashtable OutPutHT)
        {
            try { Invoke(new delegateForThreadShow(SRT_GetXXInfo_Invoke), new Hashtable[] { OutPutHT }); }
            catch (Exception ex) { Support.StringOP.WriteLog("委托回调错误：" + ex.ToString()); }
        }
        //处理非线程创建的控件
        private void SRT_GetXXInfo_Invoke(Hashtable OutPutHT)
        {
            //返回值后的处理
            DataSet dsreturn = (DataSet)OutPutHT["返回值"];

            string zt = dsreturn.Tables["返回值单条"].Rows[0]["执行结果"].ToString();
            string showstr = dsreturn.Tables["返回值单条"].Rows[0]["提示文本"].ToString();
            //显示执行结果
            if (zt == "ok")
            {
                lblDQDJ.Text = dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"].ToString()+"元";
                lblDQYE.Text = dsreturn.Tables["返回值单条"].Rows[0]["附件信息2"].ToString()+"元";
                lblDQSY.Text = dsreturn.Tables["返回值单条"].Rows[0]["附件信息3"].ToString()+"元";

            }
            else
            {
                lblDQDJ.Text = "...元";
                lblDQYE.Text = "...元";
                lblDQSY.Text = "...元";

                //ArrayList Almsg4 = new ArrayList();
                //Almsg4.Add("");
                //Almsg4.Add(showstr);// 这里是提交失败的提示，可能是程序出错，可能是条件不足等
                //FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "其他", "", Almsg4);
                //FRSE4.ShowDialog();
            }
        }
    }
}
