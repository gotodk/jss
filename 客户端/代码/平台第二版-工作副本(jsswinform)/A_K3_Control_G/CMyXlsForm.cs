using System;
using System.Collections;
using System.Data;
using System.Threading;
using System.Windows.Forms;
using 客户端主程序.NewDataControl;
using 客户端主程序.Support;

namespace 客户端主程序.SubForm.NewCenterForm
{
    public partial class CMyXlsForm : Form
    {
        //string N_sql; //查询语句
        //string[] N_HideColumns;//隐藏列
        string N_lujing;//保存路径
        string N_webmethod;//要调用的web方法名
        Hashtable N_tiaojian;//查询语句中需要使用的条件
        string N_filename;//导出的文件名

        //public CMyXlsForm(string sql, string[] HideColumns, string lujing)
        //{
        //    InitializeComponent();
        //    N_sql = sql;
        //    N_HideColumns = HideColumns;
        //    N_lujing = lujing;
        //}

        public CMyXlsForm(Hashtable ht_params, string lujing)
        {
            InitializeComponent();
            N_tiaojian = (Hashtable)ht_params["tiaojian"];
            N_webmethod = ht_params["webmethod"].ToString();
            N_lujing = lujing;
            N_filename = ht_params["filename"].ToString();
        }

        private void CMyXlsForm_Load(object sender, EventArgs e)
        {
            //开线程，进行导出
            Hashtable InPutHT = new Hashtable();           
            InPutHT["webmethod"] = N_webmethod;
            InPutHT["tiaojian"] = N_tiaojian;
            OpenThreadMyXls OTMX = new OpenThreadMyXls(InPutHT, new delegateForThread(SRT_demo));
            Thread trd = new Thread(new ThreadStart(OTMX.BeginRun));
            trd.IsBackground = true;
            trd.Start();
        }

        //显示线程处理结果的函数(用于处理线程返回数据),调用异步委托
        private void SRT_demo(Hashtable OutPutHT)
        {
            try { Invoke(new delegateForThreadShow(SRT_demo_Invoke), new Hashtable[] { OutPutHT }); }
            catch (Exception ex) { Support.StringOP.WriteLog("委托回调错误：" + ex.ToString()); }
        }
        //处理非线程创建的控件
        private void SRT_demo_Invoke(Hashtable OutPutHT)
        {

            //返回值后的处理
            DataSet dsreturn = (DataSet)OutPutHT["返回值"];
            if (dsreturn == null)
            {
                //给出表单提交成功的提示
                ArrayList Almsg3 = new ArrayList();
                Almsg3.Add("");
                Almsg3.Add("导出失败！");
                FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "其他", "", Almsg3);
                FRSE3.ShowDialog();
                this.Close();
            }
            else
            {
                DataSet dsreturn_d = dsreturn.Copy();
                dsreturn_d.Tables.Remove("返回值单条");
                string zt = dsreturn.Tables["返回值单条"].Rows[0]["执行结果"].ToString();
                string showstr = dsreturn.Tables["返回值单条"].Rows[0]["提示文本"].ToString();
                //显示执行结果
                switch (zt)
                {
                    case "ok":
                        string na = DateTime.Now.ToString("yyyyMMddHHmmss").ToString()+N_filename;
                        MyXlsClass.goxls(dsreturn_d, na + ".xls", "sheet1",N_filename, 20, N_lujing);

                        //给出表单提交成功的提示
                        ArrayList Almsg3 = new ArrayList();
                        Almsg3.Add("");
                        Almsg3.Add(showstr);
                        FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "对号", "", Almsg3);
                        FRSE3.ShowDialog();

                        this.Close();

                        break;
                    default:
                        ArrayList Almsg4 = new ArrayList();
                        Almsg4.Add("");
                        Almsg4.Add(showstr);// 这里是提交失败的提示，可能是程序出错，可能是条件不足等
                        FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "其他", "", Almsg4);
                        FRSE4.ShowDialog();

                        this.Close();

                        break;
                }
            }

        }
    }
}
