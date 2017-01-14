using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Data.SqlClient;
using System.Threading;
using System.Collections;

namespace StressTesting
{
    public partial class MainForm : Form
    {
        delegate void ShowProgressDelegate(string  currentStep) ;
        delegate void ShowProgressDelegate_ivokl(string currentStep);
        static int marka = 0;
        DateTime dtstar = new DateTime();
       // delegate void RunTaskDelegate();
        //存放预加载的数据，投标单与预订单
        public static DataTable dtydd;
        public static DataTable dttbd;
        public static string strlogall = "";

        //投标单用户
        public static DataTable tbdyh;

        public static string connectionString = "";
        public static string CurrentPath = System.Environment.CurrentDirectory.ToString() + @"\"; //得到当前路径
        public MainForm()
        {
            InitializeComponent();

            if (File.Exists(CurrentPath + "ConSet.xml"))
            {
                TestCon();
                dtydd = DBClass.Query(((string[])GetSQL())[0],connectionString).Tables[0];
                dtydd.TableName = "DTYDD";
                dttbd = DBClass.Query(((string[])GetSQL())[1], connectionString).Tables[0];
                dttbd.TableName = "DTTBD";

                tbdyh = DBClass.Query(((string[])GetSQL())[2], connectionString).Tables[0];
                tbdyh.TableName = "TBDYH";

                DataTableToXml(dtydd);
                DataTableToXml(dttbd);
                DataTableToXml(tbdyh);
            }

            
        }

        //获取数据的sql语句
        protected  string[] GetSQL()
        {
            string[] strs = new string[3];
            //预定单
            strs[0] = " SELECT '' as FLAG ,[DLYX],[JSZHLX],[MJJSBH],[GLJJRYX],[GLJJRYHM],[GLJJRJSBH],[GLJJRPTGLJG],[SPBH],[SPMC],[GG],[JJDW]+'（压力测试）' as [JJDW],[PTSDZDJJPL],[HTQX],[SHQY],[SHQYsheng],[SHQYshi],[NMRJG],[NDGSL], 0 as [YZBSL] ,[NDGJE],[MJDJBL],[DJDJ], '竞标' as [ZT],'' as YXJZRQ   FROM [AAA_YDDXXB]  where Number in(select top 500 Y_YSYDDBH from AAA_ZBDBXXB left join AAA_DLZHXXB a on a.B_DLYX=T_YSTBDDLYX left join AAA_DLZHXXB b on b.B_DLYX=Y_YSYDDDLYX where Z_HTZT='定标执行完成' and Z_HTQX='即时' and a. B_SFYXDL='是' and a.B_SFDJ='否' and a.B_SFXM='否'  and b. B_SFYXDL='是' and b.B_SFDJ='否' and b.B_SFXM='否' order by AAA_ZBDBXXB.CreateTime desc)";
            //投标单
            strs[1] = "SELECT '' as FLAG, [DLYX],[JSZHLX],[MJJSBH],[GLJJRYX],[GLJJRYHM],[GLJJRJSBH],[GLJJRPTGLJG],[SPBH],[SPMC],[GG],[JJDW]+'（压力测试）' as [JJDW]  ,[PTSDZDJJPL] ,[HTQX] ,[MJSDJJPL],[GHQY] ,[TBNSL] ,[TBJG] ,[TBJE] ,[MJTBBZJBL],[MJTBBZJZXZ],[DJTBBZJ] ,[ZLBZYZM] ,[CPJCBG],[PGZFZRFLCNS] ,[FDDBRCNS] ,[SHFWGDYCN] ,[CPSJSQS],[SLZM],'' as '其他资质',[SPCD] as 省市区  FROM [AAA_TBD] where Number in(select top 500 T_YSTBDBH from AAA_ZBDBXXB left join AAA_DLZHXXB a on a.B_DLYX=T_YSTBDDLYX left join AAA_DLZHXXB b on b.B_DLYX=Y_YSYDDDLYX where Z_HTZT='定标执行完成' and Z_HTQX='即时' and a. B_SFYXDL='是' and a.B_SFDJ='否' and a.B_SFXM='否'  and b. B_SFYXDL='是' and b.B_SFDJ='否' and b.B_SFXM='否' order by AAA_ZBDBXXB.CreateTime desc)";

            //投标单用户
            strs[2] = "  select distinct top 10000  B_DLYX,B_JSZHLX, J_SELJSBH, GLJJRDLZH,GLJJRYHM,GLJJRBH ,I_PTGLJG from AAA_DLZHXXB join AAA_MJMJJYZHYJJRZHGLB on B_DLYX=DLYX where    B_SFYXDL='是' and B_SFDJ='否' and B_SFXM='否' and B_JSZHLX='买家卖家交易账户' and SFDQMRJJR='是' and SFYX='是' and B_ZHDQKYYE>5000000 ";

            return strs;
        }

        // DataTable转换成Xml结构的文本
        static void DataTableToXml(DataTable dt)
        {   
            //保存Xml验证架构
            //dt.WriteXmlSchema("E://xmlsample.xsd");

            //dt写成Xml结构
            //System.IO.TextWriter tw = new System.IO.StringWriter();
            //dt.WriteXml(tw);
            //string xml = tw.ToString()

            if (!File.Exists(CurrentPath + dt.TableName+ ".xml"))
            {
                FileStream fw = File.Create(CurrentPath + dt.TableName + ".xml");
                //一定要关闭，否则dt.TableName.xml被占用占用，下面语句不能执行
                fw.Close();
            }
            TextWriter sw = new StreamWriter(CurrentPath + dt.TableName + ".xml");
            dt.WriteXml(sw);
            sw.Close();
        }


        private void btnConnect_Click(object sender, EventArgs e)
        {
            DBSet f = new DBSet();
            f.ShowDialog();
        }

        //测试数据连接，如果连接不上，启动数据连接窗口
        public void TestCon()
        {
            DBClass dc = new DBClass();
            if (dc.isCon() == false)
            {

                DBSet f = new DBSet();
               
                MessageBox.Show("数据连接不上，请重新设置！");
                f.ShowDialog();
               
            }
            else
            {
               connectionString = dc.getConStr();
            }
        }

        private void tablsjl_Enter(object sender, EventArgs e)
        {
            //this.rtxtlsjl.Text = BasicShare.ReadLog(CurrentPath, "logfile");
            System.IO.DirectoryInfo dir = new DirectoryInfo(CurrentPath + "\\logs");
            string result = string.Empty;
            this.rtxtlsjl.Text = BasicShare.ReadLogs(dir, ref result);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
         
        }


        private void button1_Click(object sender, EventArgs e)
        {
            // ThreadPool.SetMaxThreads(groupMaxThreads, groupMaxThreads);//设置线程池排队最大值
            // ThreadPool.QueueUserWorkItem(正常的方法, 方法参数);

            DataSet ds = Creattable();
            string strlog = "";
            string strtime = "";
            DateTime dtgruopbtime = DateTime.Now;
            for (int i = 0; i < 100; i++)
            {
                
                ds.Clear();
                DataRow dr = ds.Tables["ydd"].NewRow();
                dr["FLAG"] = "";
                dr["DLYX"] = "wyhmj3@163.com";
                dr["JSZHLX"] = "买家卖家交易账户";
                dr["MJJSBH"] = "C131023019499";
                dr["GLJJRYX"] = "4301041993@hotmail.com";
                dr["GLJJRYHM"] = "宿月天";
                dr["GLJJRJSBH"] = "J131021017125";
                dr["GLJJRPTGLJG"] = "青岛分公司";
                dr["SPBH"] = "99L5062";
                dr["SPMC"] = "铁屑";
                dr["GG"] = "一级/粒状";
                dr["JJDW"] = "压力测试";
                dr["PTSDZDJJPL"] = 10;
                dr["HTQX"] = "即时";
                dr["SHQY"] = "|北京市北京市辖区|";
                dr["SHQYsheng"] = "北京市";
                dr["SHQYshi"] = "北京市辖区";
                dr["NMRJG"] = 10.00;
                dr["NDGSL"] = 10;
                dr["YZBSL"] = 0;
                dr["NDGJE"] = 100.00;
                dr["MJDJBL"] = 0.7;
                dr["DJDJ"] = 7.00;
                dr["ZT"] = "竞标";
                dr["YXJZRQ"] = "";
                ds.Tables["ydd"].Rows.Add(dr);
                ds.AcceptChanges();
                strlog = dr["DLYX"].ToString();
                DateTime dtbe = DateTime.Now;
                ceshi2013.fm8844.com.ws2013SoapClient aa = new ceshi2013.fm8844.com.ws2013SoapClient();
                DataSet dsa = aa.SetYDD(ds);
                DateTime dtover = DateTime.Now;
                strtime += "登陆邮箱：" + dr["DLYX"].ToString() + "  商品编号：" + dr["SPBH"].ToString() + "商品名称：" + dr["SPMC"].ToString() + "  开始时间：" + dtbe.ToString("yyyy-MM-dd HH：mm：ss：ffff") + "结束时间：" + dtover.ToString("yyyy-MM-dd HH：mm：ss：ffff") + " \r\n";
            }

            DateTime dtgruopbetime = DateTime.Now;

            StringOP.WriteLog(strtime + "本组总开始时间：" + dtgruopbtime.ToString("yyyy-MM-dd HH：mm：ss：ffff") + "总结束时间：" + dtgruopbetime.ToString("yyyy-MM-dd HH：mm：ss：ffff"));
            MessageBox.Show("success");


        }

        private DataSet Creattable()
        {

            DataSet ds = new DataSet();
            DataTable dt = new DataTable("ydd");


          
            DataColumn FLAG = new DataColumn();
            FLAG.ColumnName = "FLAG";
            FLAG.DataType = typeof(string);


            dt.Columns.Add(FLAG);

            DataColumn DLYX = new DataColumn();
            DLYX.ColumnName = "DLYX";
            DLYX.DataType = typeof(string);


            dt.Columns.Add(DLYX);


            DataColumn JSZHLX = new DataColumn();
            JSZHLX.ColumnName = "JSZHLX";
            JSZHLX.DataType = typeof(string);


            dt.Columns.Add(JSZHLX);


            DataColumn MJJSBH = new DataColumn();
            MJJSBH.ColumnName = "MJJSBH";
            MJJSBH.DataType = typeof(string);


            dt.Columns.Add(MJJSBH);

            DataColumn GLJJRYX = new DataColumn();
            GLJJRYX.ColumnName = "GLJJRYX";
            GLJJRYX.DataType = typeof(string);


            dt.Columns.Add(GLJJRYX);

            DataColumn GLJJRYHM = new DataColumn();
            GLJJRYHM.ColumnName = "GLJJRYHM";
            GLJJRYHM.DataType = typeof(string);


            dt.Columns.Add(GLJJRYHM);

            DataColumn GLJJRJSBH = new DataColumn();
            GLJJRJSBH.ColumnName = "GLJJRJSBH";
            GLJJRJSBH.DataType = typeof(string);


            dt.Columns.Add(GLJJRJSBH);

            DataColumn GLJJRPTGLJG = new DataColumn();
            GLJJRPTGLJG.ColumnName = "GLJJRPTGLJG";
            GLJJRPTGLJG.DataType = typeof(string);


            dt.Columns.Add(GLJJRPTGLJG);

            DataColumn SPBH = new DataColumn();
            SPBH.ColumnName = "SPBH";
            SPBH.DataType = typeof(string);


            dt.Columns.Add(SPBH);

            DataColumn SPMC = new DataColumn();
            SPMC.ColumnName = "SPMC";
            SPMC.DataType = typeof(string);
            SPMC.DefaultValue = "铁屑";

            dt.Columns.Add(SPMC);

            DataColumn GG = new DataColumn();
            GG.ColumnName = "GG";
            GG.DataType = typeof(string);
            GG.DefaultValue = "一级/粒状";

            dt.Columns.Add(GG);

            DataColumn JJDW = new DataColumn();
            JJDW.ColumnName = "JJDW";
            JJDW.DataType = typeof(string);
            JJDW.DefaultValue = "压力测试";

            dt.Columns.Add(JJDW);

            DataColumn PTSDZDJJPL = new DataColumn();
            PTSDZDJJPL.ColumnName = "PTSDZDJJPL";
            PTSDZDJJPL.DataType = typeof(int);
            PTSDZDJJPL.DefaultValue = 10;

            dt.Columns.Add(PTSDZDJJPL);

            DataColumn HTQX = new DataColumn();
            HTQX.ColumnName = "HTQX";
            HTQX.DataType = typeof(string);
            HTQX.DefaultValue = "即时";

            dt.Columns.Add(HTQX);

            DataColumn SHQY = new DataColumn();
            SHQY.ColumnName = "SHQY";
            SHQY.DataType = typeof(string);
            SHQY.DefaultValue = "|北京市北京市辖区|";

            dt.Columns.Add(SHQY);

            DataColumn SHQYsheng = new DataColumn();
            SHQYsheng.ColumnName = "SHQYsheng";
            SHQYsheng.DataType = typeof(string);
            SHQYsheng.DefaultValue = "北京市";

            dt.Columns.Add(SHQYsheng);

            DataColumn SHQYshi = new DataColumn();
            SHQYshi.ColumnName = "SHQYshi";
            SHQYshi.DataType = typeof(string);
            SHQYshi.DefaultValue = "北京市辖区";

            dt.Columns.Add(SHQYshi);

            DataColumn NMRJG = new DataColumn();
            NMRJG.ColumnName = "NMRJG";
            NMRJG.DataType = typeof(double);
            NMRJG.DefaultValue = 10.00;

            dt.Columns.Add(NMRJG);

            DataColumn NDGSL = new DataColumn();
            NDGSL.ColumnName = "NDGSL";
            NDGSL.DataType = typeof(int);
            NDGSL.DefaultValue = 10;

            dt.Columns.Add(NDGSL);

            DataColumn YZBSL = new DataColumn();
            YZBSL.ColumnName = "YZBSL";
            YZBSL.DataType = typeof(int);
            YZBSL.DefaultValue = 0;

            dt.Columns.Add(YZBSL);

            DataColumn NDGJE = new DataColumn();
            NDGJE.ColumnName = "NDGJE";
            NDGJE.DataType = typeof(double);
            NDGJE.DefaultValue = 100.00;

            dt.Columns.Add(NDGJE);

            DataColumn MJDJBL = new DataColumn();
            MJDJBL.ColumnName = "MJDJBL";
            MJDJBL.DataType = typeof(double);
            MJDJBL.DefaultValue = 0.0700;

            dt.Columns.Add(MJDJBL);

            DataColumn DJDJ = new DataColumn();
            DJDJ.ColumnName = "DJDJ";
            DJDJ.DataType = typeof(double);
            DJDJ.DefaultValue = 7.00;

            dt.Columns.Add(DJDJ);

            DataColumn ZT = new DataColumn();
            ZT.ColumnName = "ZT";
            ZT.DataType = typeof(string);
            ZT.DefaultValue = "竞标";

            dt.Columns.Add(ZT);



            DataColumn YXJZRQ = new DataColumn();
            YXJZRQ.ColumnName = "YXJZRQ";
            YXJZRQ.DataType = typeof(string);
            YXJZRQ.DefaultValue = "";

            dt.Columns.Add(YXJZRQ);

            ds.Tables.Add(dt);

            return ds;


        }
        /// <summary>
        /// 一个初始化的投标单DataSet
        /// </summary>
        /// <returns></returns>
        private DataSet CreateTBD()
        {
            DataSet ds = new DataSet();
            DataTable dt = dttbd.Clone();
            dt.TableName = "TBD";
            dt.Columns["FLAG"].DefaultValue = "";
            dt.Columns["JJDW"].DefaultValue = "（压力测试）";
           // dt.Columns["ZT"].DefaultValue = "竞标";
            dt.Columns["HTQX"].DefaultValue = "即时";
            ds.Tables.Add(dt);
            return ds;

 
        }

        private void txttotalNum_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && !Char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }  


        }

        private void btnbegin_Click(object sender, EventArgs e)
        {
            this.groupBox1.Enabled = false;
            if (dtydd != null && dtydd.Rows.Count > 0 && dttbd != null && dttbd.Rows.Count > 0 && tbdyh!= null&&tbdyh.Rows.Count>0)
            {
                if (this.txttotalNum.Text == "0" || this.txttotalNum.Text == "" || this.textGRNum.Text == "0" || this.textGRNum.Text == "" || this.texteveNUm.Text == "0" || this.texteveNUm.Text == "")
                {
                    MessageBox.Show("前三个字段不允许为空或0!");
                    this.groupBox1.Enabled = true;
                    return;
                }
                marka = 0;
                insert();
            }
            else
            {
                richTextBox1.SelectionColor = Color.Red;
                richTextBox1.AppendText("获取基础数据出错！" + Environment.NewLine);//MessageBox.Show("获取基础数据出错！");
            }

            this.btnbegin.Text = "开始压力测试";
            this.btnbegin.Enabled = true;
        }

        private  void insertdate(object group)
        {
            //Thread.Sleep(1000);
                    Hashtable ht = group as Hashtable;
                    string strlog = "";
                    string strtime = "";
                    DateTime dtgruopbtime = DateTime.Now;
                    int everNUm = int.Parse(ht["even"].ToString())/2;//int.Parse(texteveNUm.Text.Trim());
                    DataSet ds = new DataSet();
                    int yddNum = dtydd.Rows.Count;
                    int intgroup = int.Parse(ht["G"].ToString());
                    int intStar = (intgroup - 1) * (everNUm);
                    int Over = intgroup * everNUm;
                    ShowProgressDelegate test = ht["dele"] as ShowProgressDelegate;
                try
                {
                    for (int i = (intgroup - 1) * (everNUm); i < intgroup * everNUm; i++)
                    {
                        DataSet dsNew = Creattable();
                        DataSet dsTBDnew = CreateTBD();
                        DataRow drnew = dsNew.Tables["ydd"].NewRow();
                        ds.Clear();
                        DataRow dr = null;
                        if (i < yddNum)
                        {
                            dr = dtydd.Rows[i];
                        }
                        else
                        {
                            //i- (i / yddNum)*yddNum
                            dr = dtydd.Rows[i - (i / yddNum) * yddNum];

                        }
                        drnew.ItemArray = dr.ItemArray;
                        dsNew.Tables["ydd"].Rows.Add(drnew);
                        dsNew.AcceptChanges();

               
                        strlog = dr["DLYX"].ToString();
                        DateTime dtbe = DateTime.Now;
                        ceshi2013.fm8844.com.ws2013SoapClient aa = new ceshi2013.fm8844.com.ws2013SoapClient();
                        DataSet dsa = aa.SetYDD(dsNew);

                        DateTime dtover = DateTime.Now;

                        TimeSpan tsa = dtover - dtbe;
                       strtime += "登陆邮箱：" + dr["DLYX"].ToString() + " , 商品编号：" + dr["SPBH"].ToString() + ",商品名称：" + dr["SPMC"].ToString() + ", 运行状况：" + dsa.Tables["返回值单条"].Rows[0]["执行结果"].ToString() + "  " + dsa.Tables["返回值单条"].Rows[0]["提示文本"].ToString() + " , 开始时间：" + dtbe.ToString("yyyy-MM-dd HH：mm：ss：ffff") + " , 结束时间：" + dtover.ToString("yyyy-MM-dd HH：mm：ss：ffff") +",总耗时："+tsa.TotalSeconds.ToString()+ " \r\n";

                        //获取投标单Dataset
                        DataRow drtdd = dsTBDnew.Tables[0].NewRow();

                        drtdd.ItemArray = dttbd.Select(" SPBH='" + dr["SPBH"].ToString() + "' ")[0].ItemArray;
                        int index = i + (int)ht["Yhindex"];
                        DataRow dryh = tbdyh.Rows[index];
                      
                        drtdd["DLYX"] = dryh["B_DLYX"].ToString();
                        drtdd["JSZHLX"] = dryh["B_JSZHLX"].ToString();
                        drtdd["MJJSBH"] = dryh["J_SELJSBH"].ToString();
                        drtdd["GLJJRYX"] = dryh["GLJJRDLZH"].ToString();
                        drtdd["GLJJRYHM"] = dryh["GLJJRYHM"].ToString();
                        drtdd["GLJJRJSBH"] = dryh["GLJJRBH"].ToString();
                        drtdd["GLJJRPTGLJG"] = dryh["I_PTGLJG"].ToString();
                        dsTBDnew.Tables["TBD"].Rows.Add(drtdd);
                        dsTBDnew.AcceptChanges();
                        DateTime dttbdb = DateTime.Now;
                        ceshi2013.fm8844.com.ws2013SoapClient aatbd = new ceshi2013.fm8844.com.ws2013SoapClient();
                        DataSet dsatbd = aatbd.SetTBD(dsTBDnew);
                        DateTime dttbdover = DateTime.Now;
                        TimeSpan tstbd = dttbdover - dttbdb;
                        strtime += "登陆邮箱：" + drtdd["DLYX"].ToString() + " , 商品编号：" + drtdd["SPBH"].ToString() + ",商品名称：" + drtdd["SPMC"].ToString() + ", 运行状况：" + dsatbd.Tables["返回值单条"].Rows[0]["执行结果"].ToString() + "  " + dsatbd.Tables["返回值单条"].Rows[0]["提示文本"].ToString() + " , 开始时间：" + dttbdb.ToString("yyyy-MM-dd HH：mm：ss：ffff") + " , 结束时间：" + dttbdover.ToString("yyyy-MM-dd HH：mm：ss：ffff") + "总耗时："+tstbd.TotalSeconds.ToString()+" \r\n";

                    }

                    DateTime dtgruopbetime = DateTime.Now;
                    TimeSpan ts = dtgruopbetime - dtgruopbtime;
                    test("\r\n" + strtime + intgroup.ToString() + "组执行成功，开始时间：" + dtgruopbtime.ToString("yyyy-MM-dd HH：mm：ss：ffff") + ",结束时间：" + dtgruopbetime.ToString("yyyy-MM-dd HH：mm：ss：ffff") + ",总耗时：" + ts.TotalSeconds.ToString());

                }
                catch (Exception ex)
                {
                    test(intgroup.ToString()+"组出现异常：" + DateTime.Now.ToString("yyyy-MM-dd HH：mm：ss：ffff") + "异常信息：" + ex.ToString());
                   
                }
                
            
          
        }

        private void insert()
        {
            try
            {
                richTextBox1.SelectionColor = Color.Green;
                richTextBox1.AppendText("新一轮压力测试开始！" + Environment.NewLine);
                richTextBox1.AppendText("======================================================================" + Environment.NewLine);
                StringOP.WriteLog("新一轮压力测试开始!\r\n======================================================================");
                dtstar = DateTime.Now;
                int groupMaxThreads = int.Parse(textGRNum.Text.Trim()); //最大并发线程。
                //int groupnow = 1; //当前第几组
                int Groupmax = 0;
                int enevNum = int.Parse(texteveNUm.Text.Trim());
                int Yhstartindex = 0;
                if (!this.textBox1.Text.Trim().Equals(""))
                {
                    Yhstartindex = int.Parse(this.textBox1.Text.Trim());
                }

                if (int.Parse(txttotalNum.Text.Trim()) % int.Parse(texteveNUm.Text.Trim()) == 0)
                {
                    Groupmax = int.Parse(txttotalNum.Text.Trim()) / int.Parse(texteveNUm.Text.Trim());
                }
                else
                {
                    Groupmax = int.Parse(txttotalNum.Text.Trim()) / int.Parse(texteveNUm.Text.Trim()) + 1;
                }

                ThreadPool.SetMaxThreads(groupMaxThreads, groupMaxThreads);//设置线程池排队最大值
                Hashtable htpars = null;
                // ManualResetEvent[] _ManualEvents = new ManualResetEvent[10];
                this.progressBar1.Maximum = Groupmax;
                this.progressBar1.Value = 0;
                label7.Text = "0%";
                for (int i = 0; i < Groupmax; i++)
                {
                    htpars = new Hashtable();
                    htpars["dele"] = new ShowProgressDelegate(SRT_demo);
                    htpars["G"] = i + 1;
                    htpars["even"] = enevNum;
                    htpars["Yhindex"] = Yhstartindex;
                    ThreadPool.QueueUserWorkItem(new WaitCallback(insertdate), htpars);
                }
            }
            catch (Exception ex)
            {
                richTextBox1.AppendText(ex.ToString() + Environment.NewLine);
                StringOP.WriteLog("\r\n"+ex.ToString()+"\r\n");
            }

           
        }


        //显示线程处理结果的函数(用于处理线程返回数据),调用异步委托
        private void SRT_demo(string OutPutHT)
        {
            try
            { 
                Invoke(new ShowProgressDelegate_ivokl(SRT_demo_Invoke),   OutPutHT );
            }
            catch (Exception ex) 
            { 
                StringOP.WriteLog("委托回调错误：" + ex.ToString());
            }
        }

        //处理非线程创建的控件
        private void SRT_demo_Invoke(string  OutPutHT)
        {

            marka++;
            this.progressBar1.Value = marka;
            label7.Text = (((float)marka / this.progressBar1.Maximum)*100).ToString("0.00")+"%";
            richTextBox1.AppendText(OutPutHT + Environment.NewLine);
            StringOP.WriteLog(OutPutHT);
            if (marka == this.progressBar1.Maximum)
            {
                DateTime dtover = DateTime.Now;
                TimeSpan ts = dtover - dtstar;
                StringOP.WriteLog("本次压力测试  开始时间：" + dtstar.ToString("yyyy-MM-dd HH：mm：ss：ffff") + "结束时间：" + dtover.ToString("yyyy-MM-dd HH：mm：ss：ffff") + " 总耗时：" + ts.TotalSeconds.ToString() + "s。\r\n======================================================================");


                richTextBox1.AppendText("\r\n本次压力测试结束  开始时间：" + dtstar.ToString("yyyy-MM-dd HH：mm：ss：ffff") + " 结束时间：" + dtover.ToString("yyyy-MM-dd HH：mm：ss：ffff") + " 总耗时：" + ts.TotalSeconds.ToString() + "s,详细数据信息请查看日志文件" + Environment.NewLine);
                richTextBox1.AppendText("======================================================================" + Environment.NewLine);
                groupBox1.Enabled = true;
            }

        }

    }
}
