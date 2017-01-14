using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using 银行监控浦发后台;
using System.Collections;
using System.Threading;
using System.IO;
using System.Globalization;

namespace 银行监控浦发
{
    public partial class FormWindowsServicesTry : Form
    {
        Hashtable htpath = null;
        public FormWindowsServicesTry()
        {
            InitializeComponent();
            rtxYXZT.RightMargin = 1000000000;
            datetimejstime.Value = DateTime.Now;
        }


     


  

      

        private void FormWindowsServicesTry_Load(object sender, EventArgs e)
        {
            //string t1 = "2013-04-26 15:24:36.000";
            //DateTime time1begin = DateTime.Parse(t1);
            //DateTime time2begin = time1begin.AddDays(1);
            //string t2 = time2begin.ToLocalTime().ToString();
            //this.Text = t2;
        }

      

    

        /// <summary>
        /// 开启银行监听
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button12_Click(object sender, EventArgs e)
        {
            button12.Enabled = false;
            //实例化委托,并参数传递给线程执行类开始执行线程
            银行监控浦发后台.delegateForThread tempDFT = new 银行监控浦发后台.delegateForThread(JT);
            ClassAcceptorMsg RTCBM = new ClassAcceptorMsg(null, tempDFT);
            //服务器类库.ClassAcceptorMsg RCT = new 服务器类库.ClassAcceptorMsg(null, tempDFT);
            Thread trd = new Thread(new ThreadStart(RTCBM.AcceptorTest));
            trd.IsBackground = true;
            trd.Start();






            //银行接口协议测试.ClassAcceptorMsg RCT = new 银行接口协议测试.ClassAcceptorMsg(null, tempDFT);
            //Thread trd = new Thread(new ThreadStart(RCT.AcceptorTest));
            //trd.IsBackground = true;
            //trd.Start();

        }

        private void JT(Hashtable OutPutHT)
        {
            try
            {
                //调用异步委托
                Invoke(new delegateForThreadShow(JT_Invoke), new Hashtable[] { OutPutHT });
            }
            catch (Exception ex)
            {
                richTextBox4.SelectionColor = Color.White;
                richTextBox4.AppendText("委托回调错误:=========================" + Environment.NewLine);
                richTextBox4.SelectionColor = Color.Red;
                richTextBox4.AppendText(ex.ToString() + Environment.NewLine);
                richTextBox4.SelectionColor = Color.White;
                richTextBox4.AppendText("============================" + Environment.NewLine);
            }
        }
        private void JT_Invoke(Hashtable OutPutHT)
        {
            richTextBox3.SelectionColor = Color.White;
            richTextBox3.AppendText("=========================" + Environment.NewLine);
            richTextBox3.SelectionColor = Color.Red;
            richTextBox3.AppendText("执行状态：" + OutPutHT["执行状态"].ToString() + Environment.NewLine);
            richTextBox3.AppendText("详细信息：" + OutPutHT["详细信息"].ToString() + Environment.NewLine);
            richTextBox3.AppendText("测试返回值：" + OutPutHT["测试返回值"].ToString() + Environment.NewLine);
            richTextBox3.SelectionColor = Color.White;
            richTextBox3.AppendText("============================" + Environment.NewLine);
            if (OutPutHT["测试返回值"] != null && OutPutHT["测试返回值"].ToString().IndexOf("我方服务器返") > 0)
            {
                richTextBox4.AppendText("=========================" + Environment.NewLine);
                richTextBox4.AppendText("测试返回值：" + OutPutHT["测试返回值"].ToString() + Environment.NewLine);
                richTextBox4.SelectionColor = Color.White;
                richTextBox4.AppendText("============================" + Environment.NewLine);
            }
        }

        /// <summary>
        /// 查看监听异常日志
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button13_Click(object sender, EventArgs e)
        {

        }



        #region  生成.dat文件

        public void creatfile(DataTable dataSetData, Hashtable ht)
        {


            DateTime dt = Convert.ToDateTime(this.datetimejstime.Value);
            string Taname = dataSetData.TableName.ToString();
            richTextBox5.SelectionColor = Color.White;
            richTextBox5.AppendText("=========================" + Environment.NewLine);
            richTextBox5.AppendText("执行状态：开始生成" + dataSetData.TableName.ToString() + "!" + Environment.NewLine);
            string filename = "";
            try
            {
                switch (Taname)
                {
                    case "存管客户交易资金净额清算文件":
                        filename = "sbusi" + dt.Month.ToString("00") + dt.Day.ToString("00");
                        break;
                    case "存管客户资金余额明细文件":
                        filename = "sbala" + dt.Month.ToString("00") + dt.Day.ToString("00");
                        break;
                    case "存管汇总账户资金交收汇总文件":
                        filename = "spay" + dt.Month.ToString("00") + dt.Day.ToString("00");
                        break;
                    case "存管客户批量利息入帐文件":
                        filename = "accr" + dt.Month.ToString("00") + dt.Day.ToString("00");
                        break;
                    default:
                        break;
                }


                string fileName = "FileConfig.xml";
                DataSet dataSet = new DataSet();
                dataSet.ReadXml(fileName);
                string saveFileInfo = dataSet.Tables["基本配置"].Rows[0]["MSS"].ToString();
                string saveFileTime = dataSet.Tables["基本配置"].Rows[0]["Tag"].ToString().Split('|')[0];

                //string saveFileInfo = ht["saveFileInfo"].ToString();//dataSet.Tables["基本配置"].Rows[0]["MSS"].ToString();
                //string saveFileTime = ht["saveFileTime"].ToString();//dataSet.Tables["基本配置"].Rows[0]["Tag"].ToString().Split('|')[0];
                #region//把数据写入到dat文件



                DataTable dataTableCopy = dataSetData;
                FileStream fs = new FileStream(saveFileInfo + filename + ".dat", FileMode.OpenOrCreate, FileAccess.Write);
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    if (dataTableCopy != null && dataTableCopy.Rows.Count > 0)
                    {
                        for (int i = 0; i < dataTableCopy.Rows.Count; i++)
                        {
                            for (int j = 0; j < dataTableCopy.Columns.Count; j++)
                            {
                                sw.Write(dataTableCopy.Rows[i][j].ToString() + "|");
                            }
                            sw.Write("\r\n");
                        }
                    }
                    else
                    {
                        sw.Write("");
                    }
                    sw.Flush();
                }

                richTextBox5.SelectionColor = Color.White;
                richTextBox5.AppendText("=========================" + Environment.NewLine);
                richTextBox5.AppendText("执行状态：生成" + dataSetData.TableName.ToString() + "成功!" + Environment.NewLine);
            }
            catch (Exception ex)
            {
                StringOP.WriteLog("生成" + dataSetData.TableName.ToString() + "文件是出错：" + ex.ToString(),"");
                richTextBox5.SelectionColor = Color.White;
                richTextBox5.AppendText("=========================" + Environment.NewLine);
                richTextBox5.AppendText("执行状态：生成" + dataSetData.TableName.ToString() + "失败!" + Environment.NewLine);
                richTextBox6.SelectionColor = Color.White;
                richTextBox6.AppendText("=========================" + Environment.NewLine);
                richTextBox6.AppendText("生成" + dataSetData.TableName.ToString() + "失败:" + ex.ToString() + Environment.NewLine);
                return;
            }

                #endregion

            //dataSet.Tables["基本配置"].Rows[0]["Tag"] = DateTime.Now.ToString("yyyy-MM-dd") + "|" + "文件生成完毕";
            //dataSet.WriteXml(fileName);
            //richTextBox5.Clear();
            //richTextBox5.SelectionColor = Color.White;
            //richTextBox5.AppendText("=========================" + Environment.NewLine);
            //richTextBox5.SelectionColor = Color.Red;
            //richTextBox5.AppendText("写入文件完毕！" + Environment.NewLine);
        }


        //开启一个测试线程
        private void SRT_demo_Run(Hashtable InPutHT)
        {

            银行监控浦发后台.Runthreadgetbankfile g = new Runthreadgetbankfile(InPutHT, new 银行监控浦发后台.delegateForThread(SRT_demo));
            Thread trd = new Thread(new ThreadStart(g.Getcgkhjyzijefile));
            trd.IsBackground = true;
            trd.Start();
        }
        //显示线程处理结果的函数(用于处理线程返回数据),调用异步委托
        private void SRT_demo(Hashtable OutPutHT)
        {
            try { Invoke(new 银行监控浦发后台.delegateForThreadShow(SRT_demo_Invoke), new Hashtable[] { OutPutHT }); }
            catch (Exception ex)
            {
                StringOP.WriteLog("委托回调错误：" + ex.ToString(),"");
                //richTextBox6.SelectionColor = Color.White;
                //richTextBox6.AppendText("=========================" + Environment.NewLine);
                //richTextBox6.AppendText("委托回调错误:" + ex.ToString() + Environment.NewLine);
            }
        }
        //处理非线程创建的控件
        private void SRT_demo_Invoke(Hashtable OutPutHT)
        {
            //重新开放提交区域,并滚动条强制置顶
            button14.Enabled = true;
            try
            {
                //返回值后的处理
                DataSet dsreturn = (DataSet)OutPutHT["返回值"];


                string zt = dsreturn.Tables["返回值单条"].Rows[0]["执行结果"].ToString();
                string showstr = dsreturn.Tables["返回值单条"].Rows[0]["提示文本"].ToString();
                if (zt == "err")
                {
                    StringOP.WriteLog("数据返回错误：" + showstr,"");
                    richTextBox6.SelectionColor = Color.White;
                    richTextBox6.AppendText("=========================" + Environment.NewLine);
                    richTextBox6.AppendText("数据获取错误:" + showstr + Environment.NewLine);
                }
                else if (zt == "ok")
                {
                    if (dsreturn.Tables.Count == 2)
                    {
                        richTextBox5.SelectionColor = Color.White;
                        richTextBox5.AppendText("=========================" + Environment.NewLine);
                        richTextBox5.AppendText(showstr);
                        Hashtable ht = new Hashtable();
                        foreach (DataTable dt in dsreturn.Tables)
                        {

                            if (!dt.TableName.Equals("返回值单条"))
                            {
                                creatfile(dt, htpath);
                                string Staname = dt.TableName;
                                button14.Enabled = false;
                                this.btnispass.Enabled = true;
                            }
                        }


                    }

                    else
                    {
                        StringOP.WriteLog("获取银行文件数据数据返回错误：","");
                        richTextBox6.SelectionColor = Color.White;
                        richTextBox6.AppendText("=========================" + Environment.NewLine);
                        richTextBox6.AppendText("数据获取错误:返回数据表中数据不完整！" + Environment.NewLine);
                        button14.Enabled = false;
                        this.btnispass.Enabled = false;
                    }
                }
                else if (zt == "have")
                {
                    button14.Enabled = false;
                   
                    DialogResult dialogResult = MessageBox.Show(showstr, "提示对话框", MessageBoxButtons.YesNo, MessageBoxIcon.Warning
);

                    if (dialogResult == DialogResult.Yes)
                    {
                        button14.Enabled = true;
                        btnispass.Enabled = false;
                    }
                    else
                    {
                        button14.Enabled = false;
                        btnispass.Enabled = true;
                        return;
                    }
                }
                else if (zt == "pass")
                {
                    MessageBox.Show(showstr, "提示对话框");
                    button14.Enabled = true;
                    btnispass.Enabled = false;
                }
                else if (zt == "恩，不错取到了")
                {
                    GetFileData(showstr);
                    Creatwheretxt(showstr);

                }
                else if (zt == "木有啊")
                {
                    StringOP.WriteLog("没有查找到需要发送给银行的数据！","");
                    richTextBox6.SelectionColor = Color.White;
                    richTextBox6.AppendText("=========================" + Environment.NewLine);
                    richTextBox6.AppendText("没有查找到需要发送给银行的文件！" + Environment.NewLine);
                    button14.Enabled = false;
                    this.btnispass.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                StringOP.WriteLog("数据返回错误：" + ex.ToString(),"");
                richTextBox6.SelectionColor = Color.White;
                richTextBox6.AppendText("=========================" + Environment.NewLine);
                richTextBox6.AppendText("数据获取错误:" + ex.ToString() + Environment.NewLine);
                button14.Enabled = false;
                btnispass.Enabled = true;
            }
        }


        private void Creatwheretxt(string Strsheretext)
        {
            string sFilePath = "logs\\where"; //DateTime.Now.ToString("yyyyMM");
            string sremarkpath = "logs\\" + "Remarkwhere";
            string sFileName = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + ".txt";
            sFileName = sFilePath + "\\" + sFileName; //文件的绝对路径

            #region 备份文件
            if (!Directory.Exists(sFilePath))//验证路径是否存在
            {
                Directory.CreateDirectory(sFilePath);//不存在则创建
            }
            DirectoryInfo dir = new DirectoryInfo(sFilePath); //path为某个目录，如： “D:\Program Files”
            
            FileInfo[] inf = dir.GetFiles();
            if (inf.Length > 0)
            {
                try
                {
                    DateTime dtremark = Convert.ToDateTime(this.datetimejstime.Value).AddDays(-1);
                    richTextBox5.SelectionColor = Color.White;
                    richTextBox5.AppendText("=========================" + Environment.NewLine);
                    richTextBox5.AppendText("执行状态：开始备份文件夹：" + sFilePath + "内文件......." + Environment.NewLine);
                    foreach (FileInfo finf in inf)
                    {
                        if (finf.Extension.Equals(".txt")) //如果扩展名为“.txt”
                        {
                            if (Directory.Exists(sremarkpath ))
                            {

                                finf.MoveTo(sremarkpath+"\\"+finf.Name);

                            }
                            else
                            {
                                Directory.CreateDirectory(sremarkpath );
                                finf.MoveTo(sremarkpath + "\\" + finf.Name);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    richTextBox5.SelectionColor = Color.White;
                    richTextBox5.AppendText("=========================" + Environment.NewLine);
                    richTextBox5.AppendText("执行状态：备份文件是出错!" + Environment.NewLine);
                    richTextBox6.SelectionColor = Color.White;
                    richTextBox6.AppendText("=========================" + Environment.NewLine);
                    richTextBox6.AppendText("执行状态：备份文件是出错!" + ex.ToString() + Environment.NewLine);
                    button14.Enabled = true;
                    return;
                }

                richTextBox5.SelectionColor = Color.White;
                richTextBox5.AppendText("=========================" + Environment.NewLine);
                richTextBox5.AppendText("执行状态：文件备份完毕......" + Environment.NewLine);
                htpath = new Hashtable();
            }
            #endregion

          
            FileStream fs;
            StreamWriter sw;
            if (File.Exists(sFileName))//验证文件是否存在，有则追加，无则创建
            {
                fs = new FileStream(sFileName, FileMode.Append, FileAccess.Write);
            }
            else
            {
                fs = new FileStream(sFileName, FileMode.Create, FileAccess.Write);
            }
            sw = new StreamWriter(fs);
            sw.WriteLine(Strsheretext);
            sw.WriteLine("");
            sw.Close();
            fs.Close();
        }

        private void button14_Click(object sender, EventArgs e)
        {

            if (comboBox2.Text.Equals("") || !comboBox2.Text.Equals("浦发银行"))
            {

                richTextBox6.SelectionColor = Color.White;
                richTextBox6.AppendText("=========================" + Environment.NewLine);
                richTextBox6.AppendText("当前我只能生成 “浦发银行”文件，请不要虐待我！");
                return;
            }

            button14.Enabled = false;

            htpath = new Hashtable();

            Hashtable hthave = new Hashtable();
            hthave["开户银行"] = comboBox2.Text;
            hthave["结算时间"] = Convert.ToDateTime(this.datetimejstime.Value).ToString("yyyy-MM-dd");
            // hthave["方法名称"] = "GetStrwhere"; 浦发银行-获取结算文件需发送数据
            hthave["方法名称"] = "浦发银行-获取结算文件需发送数据";

            SRT_demo_Run(hthave);
          
        }

        private void GetFileData( string Strwhere)
        {
            htpath = new Hashtable();

            //查看是否已生成过文件
            #region //读取XML配置文件
            string fileName = "FileConfig.xml";
            DataSet dataSet = new DataSet();
            dataSet.ReadXml(fileName);
            string saveFileInfo = dataSet.Tables["基本配置"].Rows[0]["MSS"].ToString();
            string saveFileTime = dataSet.Tables["基本配置"].Rows[0]["Tag"].ToString().Split('|')[0];
            htpath["saveFileInfo"] = dataSet.Tables["基本配置"].Rows[0]["MSS"].ToString();
            htpath["saveFileTime"] = dataSet.Tables["基本配置"].Rows[0]["Tag"].ToString();

            try
            {
                if (!Directory.Exists(saveFileInfo))
                {
                    Directory.CreateDirectory(saveFileInfo);
                }
            }
            catch
            {
                button14.Enabled = true;
                richTextBox6.SelectionColor = Color.White;
                richTextBox6.AppendText("=========================" + Environment.NewLine);
                richTextBox6.AppendText("创建路径:“" + saveFileInfo + "”" + "时出错，请确定此路径确实能创建文件夹！");
                return;
            }



            #endregion
            #region//判断是否已经生成过文件
            if (DateTime.Now.ToString("yyyy-MM-dd").ToString().Equals(saveFileTime))
            {
                button14.Enabled = true;
                richTextBox5.Clear();
                richTextBox5.SelectionColor = Color.White;
                richTextBox5.AppendText("=========================" + Environment.NewLine);
                richTextBox5.SelectionColor = Color.Red;
                richTextBox5.AppendText("您今天已经执行过此操作啦，此操作一天只能执行一次！" + Environment.NewLine);
                return;
            }
            #endregion
            #region 备份文件
            DirectoryInfo dir = new DirectoryInfo(saveFileInfo); //path为某个目录，如： “D:\Program Files”
            FileInfo[] inf = dir.GetFiles();
            if (inf.Length > 0)
            {
                try
                {
                    DateTime dtremark = Convert.ToDateTime(this.datetimejstime.Value).AddDays(-1);
                    richTextBox5.SelectionColor = Color.White;
                    richTextBox5.AppendText("=========================" + Environment.NewLine);
                    richTextBox5.AppendText("执行状态：开始备份文件夹：" + saveFileInfo + "内文件......." + Environment.NewLine);
                    foreach (FileInfo finf in inf)
                    {
                        if (finf.Extension.Equals(".dat")) //如果扩展名为“.xml”
                        {
                            if (Directory.Exists(saveFileInfo + dtremark.ToString("yyyy-MM-dd")))
                            {

                                finf.MoveTo(saveFileInfo + dtremark.ToString("yyyy-MM-dd") + "\\" + finf.Name.Remove(finf.Name.ToString().IndexOf('.')) + DateTime.Now.ToString("yyyymmddhhmmss") + ".dat");

                            }
                            else
                            {
                                Directory.CreateDirectory(saveFileInfo + dtremark.ToString("yyyy-MM-dd"));
                                finf.MoveTo(saveFileInfo + dtremark.ToString("yyyy-MM-dd") + "\\" + finf.Name.Remove(finf.Name.ToString().IndexOf('.')) + DateTime.Now.ToString("yyyymmddhhmmss") + ".dat");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    richTextBox5.SelectionColor = Color.White;
                    richTextBox5.AppendText("=========================" + Environment.NewLine);
                    richTextBox5.AppendText("执行状态：备份文件是出错!" + Environment.NewLine);
                    richTextBox6.SelectionColor = Color.White;
                    richTextBox6.AppendText("=========================" + Environment.NewLine);
                    richTextBox6.AppendText("执行状态：备份文件是出错!" + ex.ToString() + Environment.NewLine);
                    button14.Enabled = true;
                    return;
                }

                richTextBox5.SelectionColor = Color.White;
                richTextBox5.AppendText("=========================" + Environment.NewLine);
                richTextBox5.AppendText("执行状态：文件备份完毕......" + Environment.NewLine);
                htpath = new Hashtable();
            }
            #endregion
            #region  开始生成文件
            richTextBox5.SelectionColor = Color.White;
            richTextBox5.AppendText("=========================" + Environment.NewLine);
            richTextBox5.AppendText("执行状态：开始获取存管客户交易资金净额清算文件数据!" + Environment.NewLine);
            htpath = new Hashtable();



            Hashtable hthave = new Hashtable();
            hthave["开户银行"] = comboBox2.Text;
            hthave["结算时间"] = Convert.ToDateTime(this.datetimejstime.Value).ToString("yyyy-MM-dd");
            hthave["where"] = Strwhere;
            // hthave["方法名称"] = "Getcgkhjyzijefile"; 浦发银行-获取交易资金净额清算数据
            hthave["方法名称"] = "浦发银行-获取交易资金净额清算数据";
            SRT_demo_Run(hthave);


            richTextBox5.SelectionColor = Color.White;
            richTextBox5.AppendText("=========================" + Environment.NewLine);
            richTextBox5.AppendText("执行状态：开始获取存管客户资金余额明细文件数据!" + Environment.NewLine);
            htpath = new Hashtable();


            Hashtable hta = new Hashtable();

            hta["开户银行"] = comboBox2.Text;
            hta["结算时间"] = Convert.ToDateTime(this.datetimejstime.Value).ToString("yyyy-MM-dd");
            hta["where"] = Strwhere;
            //hta["方法名称"] = "Getcgkhzjmc";
            hta["方法名称"] = "浦发银行-获取资金余额明细";
            SRT_demo_Run(hta);


            //creatfile(dt, htpath);
            richTextBox5.SelectionColor = Color.White;
            richTextBox5.AppendText("=========================" + Environment.NewLine);
            richTextBox5.AppendText("执行状态：开始获取存管汇总账户资金交收汇总文件数据!" + Environment.NewLine);
            htpath = new Hashtable();



            Hashtable htt = new Hashtable();
            htt["开户银行"] = comboBox2.Text;
            htt["结算时间"] = Convert.ToDateTime(this.datetimejstime.Value).ToString("yyyy-MM-dd");
            htt["where"] = Strwhere;
           // htt["方法名称"] = "Getcghzzhzjjs";
            htt["方法名称"] = "浦发银行-存管汇总账户资金交收汇总文件格式";
            SRT_demo_Run(htt);


            richTextBox5.SelectionColor = Color.White;
            richTextBox5.AppendText("=========================" + Environment.NewLine);
            richTextBox5.AppendText("执行状态：开始获取存管客户批量利息入帐文件文件数据!" + Environment.NewLine);
            htpath = new Hashtable();



            Hashtable httlx = new Hashtable();
            httlx["开户银行"] = comboBox2.Text;
            httlx["结算时间"] = Convert.ToDateTime(this.datetimejstime.Value).ToString("yyyy-MM-dd");
            httlx["where"] = Strwhere;
           // httlx["方法名称"] = "Getcgkhpllx";
            httlx["方法名称"] = "浦发银行-获取批量利息入帐数据";
            SRT_demo_Run(httlx);

            #endregion
        }

        #endregion



        #region//银证转账对账

        private void tabPage5_Click(object sender, EventArgs e)
        {
            this.dateTimePicker1.Value = DateTime.Now;
        }
        private void btnYZZZDZ_Click(object sender, EventArgs e)
        {
            CRichTestBoxMenu cRichTestBoxMenu = new CRichTestBoxMenu(rtxYXZT);
            CRichTestBoxMenu cRichTestBoxMenu1 = new CRichTestBoxMenu(rtxYCXX);
            //1、获取数据
            if (this.cbxSSYH.Text == "浦发银行")
            {
                #region //读取XML配置文件
                string fileName = "FileYHZZConfig.xml";
                DataSet dataSet = new DataSet();
                dataSet.ReadXml(fileName);
                string saveFilePath = dataSet.Tables["基本配置"].Rows[0]["MSS"].ToString();
                string saveFileTime = dataSet.Tables["基本配置"].Rows[0]["Tag"].ToString().Split('|')[0];
                string saveFileMsg = dataSet.Tables["基本配置"].Rows[0]["Tag"].ToString().Split('|')[1];
                string saveFileBeforName = dataSet.Tables["基本配置"].Rows[0]["FileName"].ToString();
                #endregion

                DateTime dateTime = Convert.ToDateTime(this.dateTimePicker1.Value);
                Hashtable hash = new Hashtable();
                hash["对账日期"] = dateTime.ToString("yyyy-MM-dd");
                hash["开户银行"] = this.cbxSSYH.Text.ToString();
                SRT_YZZZDZ_Run(hash);
                btnYZZZDZ.Enabled = false;

                rtxYXZT.SelectionColor = Color.White;
                rtxYXZT.AppendText("=========================" + Environment.NewLine);
                rtxYXZT.AppendText("对" + dateTime.ToString("yyyy年MM月dd日") + "账目信息进行对账:" + Environment.NewLine);

            }
            else
            {

                rtxYXZT.SelectionColor = Color.White;
                rtxYXZT.AppendText("=========================" + Environment.NewLine);
                rtxYXZT.AppendText(this.cbxSSYH.Text + "的接口的银证转账对账尚未开发！");
            }
        }


        //开启一个测试线程
        private void SRT_YZZZDZ_Run(Hashtable InPutHT)
        {

            银行监控浦发后台.RunThreadClassYZZZDZ yhzz = new 银行监控浦发后台.RunThreadClassYZZZDZ(InPutHT, new 银行监控浦发后台.delegateForThread(HQZZXX_Data));
            Thread trd = new Thread(new ThreadStart(yhzz.GetYHZZXX));
            trd.IsBackground = true;
            trd.Start();

        }
        //显示线程处理结果的函数(用于处理线程返回数据),调用异步委托
        private void HQZZXX_Data(Hashtable OutPutHT)
        {
            try { Invoke(new 银行监控浦发后台.delegateForThreadShow(HQZZXX_Invoke), new Hashtable[] { OutPutHT }); }
            catch (Exception ex)
            {
                StringOP.WriteLog("委托回调错误：" + ex.ToString(),"");
                //richTextBox6.SelectionColor = Color.White;
                //richTextBox6.AppendText("=========================" + Environment.NewLine);
                //richTextBox6.AppendText("委托回调错误:" + ex.ToString() + Environment.NewLine);
            }
        }
        //处理非线程创建的控件
        private void HQZZXX_Invoke(Hashtable OutPutHT)
        {
            //重新开放提交区域,并滚动条强制置顶

            btnYZZZDZ.Enabled = true;
            try
            {
                //返回值后的处理
                DataSet dsreturn = (DataSet)OutPutHT["返回值"];


                string zt = dsreturn.Tables["返回值单条"].Rows[0]["执行结果"].ToString();
                string showstr = dsreturn.Tables["返回值单条"].Rows[0]["提示文本"].ToString();

                switch (zt)
                {
                    case "err":
                        StringOP.WriteLog("数据返回错误：" + showstr,"");
                        rtxYXZT.SelectionColor = Color.White;
                        rtxYXZT.AppendText("=========================" + Environment.NewLine);
                        rtxYXZT.AppendText("数据获取错误:" + showstr + Environment.NewLine);
                        break;
                    case "ok":
                        DataTable dataTableFromBase = dsreturn.Tables["银证转账对账数据"];//原始数据文件

                        #region //读取XML配置文件
                        string fileName = "FileYHZZConfig.xml";
                        DataSet dataSet = new DataSet();
                        dataSet.ReadXml(fileName);
                        string saveFilePath = dataSet.Tables["基本配置"].Rows[0]["MSS"].ToString();
                        string saveFileTime = dataSet.Tables["基本配置"].Rows[0]["Tag"].ToString().Split('|')[0];
                        string saveFileMsg = dataSet.Tables["基本配置"].Rows[0]["Tag"].ToString().Split('|')[1];
                        string saveFileBeforName = dataSet.Tables["基本配置"].Rows[0]["FileName"].ToString();
                        #endregion

                        DateTime dateTime = Convert.ToDateTime(dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"].ToString());
                        string strFilePath = saveFilePath + saveFileBeforName + dateTime.ToString("yyyyMMdd").Substring(4, 4) + ".dat";
                        bool isExits = File.Exists(strFilePath);
                        if (isExits == false)
                        {
                            MessageBox.Show("没有找到" + this.dateTimePicker1.Value.ToString("yyyy年MM月dd日") + "的银行数据文件！");
                            return;
                        }
                        DataTable dataTableFormFile = GetYHSJData(saveFilePath + saveFileBeforName + dateTime.ToString("yyyyMMdd").Substring(4, 4) + ".dat");
                        string strTag = "没有差异";//银行数据和我们的数据是否有差异，“有差异”，“没有差异”

                        #region//开始对账  1、银行有数据我们没有数据  2、我们有数据银行没有数据  3、银行和我们都有数据但是数据不准
                        DataTable dataTableFromFielADD = new DataTable();//银行文件中独自存在的数据
                        DataTable dataTableDifferFromBase = new DataTable();//来自数据库中的不同数据
                        DataTable dataTableDifferFromFile = new DataTable();//来自银行文件中的不同数据
                        DataTable dataTableFromFielDel = new DataTable();//来自银行文件中缺少的数据
                        CompareDt(dataTableFromBase, dataTableFormFile, "证券流水", "银行流水", out dataTableFromFielADD, out dataTableDifferFromBase, out dataTableDifferFromFile, out dataTableFromFielDel);


                        if (dataTableFromFielADD.Rows.Count > 0)//银行有数据我们没有数据
                        {
                            strTag = "有差异";
                        }

                        if (dataTableFromFielDel.Rows.Count > 0)//我们有数据银行没有数据
                        {
                            strTag = "有差异";
                        }

                        if (dataTableDifferFromBase.Rows.Count > 0)//我们和银行不同的数据
                        {
                            strTag = "有差异";
                        }

                        if (dataTableDifferFromFile.Rows.Count > 0)//银行和我们不同的数据
                        {
                            strTag = "有差异";
                        }
                        #endregion

                        if (strTag == "没有差异")
                        {
                            rtxYXZT.SelectionColor = Color.White;
                            rtxYXZT.AppendText("=========================" + Environment.NewLine);
                            rtxYXZT.AppendText("对" + Convert.ToDateTime(this.dateTimePicker1.Value).ToString("yyyy年MM月dd日") + "账目信息和银行没有差异:" + Environment.NewLine);


                            StringOP.WriteLog("对比数据完成：" + "对" + Convert.ToDateTime(this.dateTimePicker1.Value).ToString("yyyy年MM月dd日") + "账目信息和银行没有差异:","");
                        }
                        else
                        {
                            FormBTXXXS formBTXXS = new FormBTXXXS(dataTableFromFielADD, dataTableDifferFromBase, dataTableDifferFromFile, dataTableFromFielDel);
                            formBTXXS.Show();
                            StringOP.WriteLog("对比数据完成:" + "对" + Convert.ToDateTime(this.dateTimePicker1.Value).ToString("yyyy年MM月dd日") + "账目信息和银行有差异:" + Environment.NewLine,"");

                        }
                        break;


                }

            }
            catch (Exception ex)
            {
                StringOP.WriteLog("数据返回错误：" + ex.ToString(),"");
                rtxYCXX.SelectionColor = Color.White;
                rtxYCXX.AppendText("=========================" + Environment.NewLine);
                rtxYCXX.AppendText("数据获取错误:" + ex.ToString() + Environment.NewLine);
            }
        }


        /// <summary>
        /// 得到银行提供的数据信息
        /// </summary>
        public DataTable GetYHSJData(string fileName)
        {
            DataTable dataTable = initReturnDataSet().Clone();
            #region //读取数据文件
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            StreamReader m_streamReader = new StreamReader(fs);
            m_streamReader.BaseStream.Seek(0, SeekOrigin.Begin);
            string strLine = m_streamReader.ReadLine();

            while (strLine != null && strLine != "")
            {
                string[] strSplit = strLine.Split('|');
                dataTable.Rows.Add(new object[] { strSplit[0].ToString().Trim(), strSplit[1].ToString().Trim(), strSplit[2].ToString().Trim(), strSplit[3].ToString().Trim(), strSplit[4].ToString().Trim(), strSplit[5].ToString().Trim(), strSplit[6].ToString().Trim(), strSplit[7].ToString().Trim(), strSplit[8].ToString().Trim(), strSplit[9].ToString().Trim(), strSplit[10].ToString().Trim(), strSplit[11].ToString().Trim(), strSplit[12].ToString().Trim() });


                strLine = m_streamReader.ReadLine();
            }
            m_streamReader.Close();
            m_streamReader.Dispose();
            fs.Close();
            fs.Dispose();
            #endregion
            return dataTable;

        }


        /// <summary>
        /// 设置获取银行数据的表结构
        /// </summary>
        /// <returns></returns>
        public DataTable initReturnDataSet()
        {

            DataTable dt_YH = new DataTable();
            dt_YH.TableName = "银行数据";
            dt_YH.Columns.Add("发起方", typeof(string));
            dt_YH.Columns.Add("交易码", typeof(string));
            dt_YH.Columns.Add("银行结算账号", typeof(string));
            dt_YH.Columns.Add("证券资金账号", typeof(string));
            dt_YH.Columns.Add("发生金额", typeof(string));
            dt_YH.Columns.Add("币种", typeof(string));
            dt_YH.Columns.Add("银行流水", typeof(string));
            dt_YH.Columns.Add("证券流水", typeof(string));
            dt_YH.Columns.Add("交易日期", typeof(string));
            dt_YH.Columns.Add("交易时间", typeof(string));
            dt_YH.Columns.Add("银行类型", typeof(string));
            dt_YH.Columns.Add("券商编号", typeof(string));
            dt_YH.Columns.Add("业务类型", typeof(string));
            return dt_YH;
        }






        /// <summary>
        /// 比较两个DataTable数据（结构相同）
        /// </summary>
        /// <param name="dt1">来自数据库的DataTable</param>
        /// <param name="dt2">来自文件的DataTable</param>
        /// <param name="keyField">关键字段名</param>
        /// <param name="keyFiledYH">关键字段名2</param>
        /// <param name="dtRetAdd">新增数据（dt2中的数据）</param>
        /// <param name="dtRetDif1">不同的数据（数据库中的数据）</param>
        /// <param name="dtRetDif2">不同的数据（图2中的数据）</param>
        /// <param name="dtRetDel">删除的数据（dt2中的数据）</param>
        public static void CompareDt(DataTable dt1, DataTable dt2, string keyField, string keyFiledYH, out DataTable dtRetAdd, out DataTable dtRetDif1, out DataTable dtRetDif2, out DataTable dtRetDel)
        {
            //为三个表拷贝表结构
            dtRetDel = dt1.Clone();
            dtRetAdd = dtRetDel.Clone();
            dtRetDif1 = dtRetDel.Clone();
            dtRetDif2 = dtRetDel.Clone();
            int colCount = dt1.Columns.Count;
            DataView dv1 = dt1.DefaultView;
            DataView dv2 = dt2.DefaultView;
            //先以第一个表为参照，看第二个表是修改了还是删除了
            foreach (DataRowView dr1 in dv1)
            {
                dv2.RowFilter = keyField + " = '" + dr1[keyField].ToString() + "' and " + keyFiledYH + " ='" + dr1[keyFiledYH].ToString() + "'";
                if (dv2.Count > 0)
                {

                    if (!CompareUpdate(dr1, dv2[0]))//比较是否有不同的
                    {

                        dtRetDif1.Rows.Add(dr1.Row.ItemArray);//修改前
                        dtRetDif2.Rows.Add(dv2[0].Row.ItemArray);//修改后
                        continue;
                    }

                }
                else
                {

                    dtRetDel.Rows.Add(dr1.Row.ItemArray);

                }
            }
            //以第一个表为参照，看记录是否是新增的
            dv2.RowFilter = "";//清空条件
            foreach (DataRowView dr2 in dv2)
            {

                dv1.RowFilter = keyField + " = '" + dr2[keyField].ToString() + "' and " + keyFiledYH + " ='" + dr2[keyFiledYH].ToString() + "'";
                if (dv1.Count == 0)
                {
                    dtRetAdd.Rows.Add(dr2.Row.ItemArray);
                }
            }

        }

        /// <summary>
        ///比较是否有不同的
        /// </summary>
        /// <param name="dr1"></param>
        /// <param name="dr2"></param>
        /// <returns></returns>
        private static bool CompareUpdate(DataRowView dr1, DataRowView dr2)
        {
            //行里只要有一项不一样，整个行就不一样,无需比较其它
            object val1;
            object val2;
            for (int i = 1; i < dr1.Row.ItemArray.Length; i++)
            {
                val1 = dr1[i].ToString();
                val2 = dr2[i].ToString();
                if (i == 9)
                {
                    continue;
                }

                if (!val1.Equals(val2))
                {
                    return false;
                }
            }
            return true;
        }


        #endregion

       


        #region//完成清算

        private void btnWCQS_Click(object sender, EventArgs e)
        {
            CRichTestBoxMenu cRichTestBoxMenu = new CRichTestBoxMenu(rTBWCQS_YX);
            CRichTestBoxMenu cRichTestBoxMenu1 = new CRichTestBoxMenu(rTBWCQS_YC);


            //1、获取数据
            if (this.cbxWCQS_SSYH.Text == "浦发银行" || this.cbxWCQS_SSYH.Text == "平安银行")
            {

                #region 读取此次已发送的数据 wyh add 2014.05.19
                string sFilePath = "logs\\where";
                if (!Directory.Exists(sFilePath))//验证路径是否存在
                {
                    richTextBox5.SelectionColor = Color.White;
                    richTextBox5.AppendText("=========================" + Environment.NewLine);
                    richTextBox5.AppendText("未找到“logs\\where”目录，无法完成清算!" + Environment.NewLine);
                    return;
                }

             DirectoryInfo dir = new DirectoryInfo(sFilePath); //path为某个目录，如： “D:\Program Files”
            
             FileInfo[] inf = dir.GetFiles();
             StringBuilder  strb = new StringBuilder("");
             string line = "";
             if (inf.Length == 1)
             {
                 
                 StreamReader sr = new StreamReader(inf[0].FullName);
                 line = sr.ReadLine();
                 while (line != null && !line.Equals(""))
                 {
                     strb.Append(line);
                     line = sr.ReadLine();
                 }
             }
             else
             {
                 rTBWCQS_YX.SelectionColor = Color.White;
                 rTBWCQS_YX.AppendText("=========================" + Environment.NewLine);
                 rTBWCQS_YX.AppendText("读取“logs\\where”目录必要的文件出错，无法完成清算!" + Environment.NewLine);
                 return;
             }


              
                #endregion


                DateTime dateTime = Convert.ToDateTime(this.dtpWCQS.Value);
                Hashtable hash = new Hashtable();
                hash["清算日期"] = dateTime.ToString("yyyy-MM-dd");
                hash["开户银行"] = this.cbxWCQS_SSYH.Text.ToString();
                hash["where"] = strb.ToString();
            
                SRT_WCQS_Run(hash);
                btnYZZZDZ.Enabled = false;

                rTBWCQS_YX.SelectionColor = Color.White;
                rTBWCQS_YX.AppendText("=========================" + Environment.NewLine);
                rTBWCQS_YX.AppendText("对" + dateTime.ToString("yyyy年MM月dd日") + "账目信息进行清算:" + Environment.NewLine);

            }
            else
            {

                rTBWCQS_YX.SelectionColor = Color.White;
                rTBWCQS_YX.AppendText("=========================" + Environment.NewLine);
                rTBWCQS_YX.AppendText(this.cbxWCQS_SSYH.Text + "的账目清算尚未开发！");
            }
        }


        //开启一个测试线程
        private void SRT_WCQS_Run(Hashtable InPutHT)
        {


            银行监控浦发后台.RunThreadClassWCQS wcqs = new 银行监控浦发后台.RunThreadClassWCQS(InPutHT, new 银行监控浦发后台.delegateForThread(WCQS_Data));
            Thread trd = new Thread(new ThreadStart(wcqs.WCQS));
            trd.IsBackground = true;
            trd.Start();


        }
        //显示线程处理结果的函数(用于处理线程返回数据),调用异步委托
        private void WCQS_Data(Hashtable OutPutHT)
        {
            try { Invoke(new 银行监控浦发后台.delegateForThreadShow(WCQS_Invoke), new Hashtable[] { OutPutHT }); }
            catch (Exception ex)
            {
                StringOP.WriteLog("委托回调错误：" + ex.ToString(),"");
                //richTextBox6.SelectionColor = Color.White;
                //richTextBox6.AppendText("=========================" + Environment.NewLine);
                //richTextBox6.AppendText("委托回调错误:" + ex.ToString() + Environment.NewLine);
            }
        }
        //处理非线程创建的控件
        private void WCQS_Invoke(Hashtable OutPutHT)
        {
            //重新开放提交区域,并滚动条强制置顶




            btnYZZZDZ.Enabled = true;
            try
            {
                //返回值后的处理
                DataSet dsreturn = (DataSet)OutPutHT["返回值"];


                string zt = dsreturn.Tables["返回值单条"].Rows[0]["执行结果"].ToString();
                string showstr = dsreturn.Tables["返回值单条"].Rows[0]["提示文本"].ToString();

                switch (zt)
                {
                    case "err":
                        StringOP.WriteLog("数据返回错误：" + showstr,"");
                        rTBWCQS_YX.SelectionColor = Color.White;
                        rTBWCQS_YX.AppendText("=========================" + Environment.NewLine);
                        rTBWCQS_YX.AppendText("数据获取错误:" + showstr + Environment.NewLine);
                        break;
                    case "ok":
                        rTBWCQS_YX.SelectionColor = Color.White;
                        rTBWCQS_YX.AppendText("=========================" + Environment.NewLine);
                        rTBWCQS_YX.AppendText("对" + this.cbxWCQS_SSYH.Text.ToString() + Convert.ToDateTime(this.dtpWCQS.Value).ToString("yyyy年MM月dd日") + "的账目信息清算完成:" + Environment.NewLine);


                        StringOP.WriteLog("清算数据完成：" + "对" + this.cbxWCQS_SSYH.Text.ToString() + Convert.ToDateTime(this.dateTimePicker1.Value).ToString("yyyy年MM月dd日") + "账目信息清算完成","");
                        break;


                }

            }
            catch (Exception ex)
            {
                StringOP.WriteLog("数据返回错误：" + ex.ToString(),"");
                rTBWCQS_YC.SelectionColor = Color.White;
                rTBWCQS_YC.AppendText("=========================" + Environment.NewLine);
                rTBWCQS_YC.AppendText("数据获取错误:" + ex.ToString() + Environment.NewLine);
            }
        }
        /// <summary>
        /// 根据选择的银行改变时确定清算日期
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxWCQS_SSYH_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cbxWCQS_SSYH.Text.ToString() == "浦发银行")
            {
                //比如银行的开始时间为“每天9:00到下午16:30”那么在2013年7月24号银行闭市之后进行清算在16:30以后到25号9:00之前清算完成的同步日期为24号
                DateTime dateTime = DateTime.Now;
                string strBSStartTime = dateTime.ToString("yyyy-MM-dd 16:30:00");//闭市开始时间
                string strBSEndTime = dateTime.ToString("yyyy-MM-dd 09:00:00");//闭市结束时间
                if (DateTime.Compare(dateTime, DateTime.ParseExact(strBSStartTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)) > 0)
                {
                    this.dtpWCQS.Value = dateTime;
                }
                else if (DateTime.Compare(dateTime, DateTime.ParseExact(strBSEndTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)) < 0)
                {
                    this.dtpWCQS.Value = dateTime.AddDays(-1);
                }

            }
        }

        /// <summary>
        /// 处理时间限制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabControl tabb = (TabControl)sender;
            if (tabb.SelectedTab.Text == "完成清算")
            {
                this.dtpWCQS.MinDate = DateTime.Now.AddDays(-1);
            }
        }
        #endregion

        //浏览指定清算文件
        private void button16_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string pa = System.IO.Path.GetFullPath(openFileDialog1.FileName);
                    DataTable dataTableFormFile = GetDataYHB(pa);
                    if (dataTableFormFile != null && dataTableFormFile.Rows.Count >= 0)
                    {
                        FormLookDataTable FLDT = new FormLookDataTable(dataTableFormFile, pa);
                        FLDT.Show();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

        }

        /// <summary>
        /// 得到银行提供的数据信息
        /// </summary>
        public DataTable GetDataYHB(string fileName)
        {
            DataTable dataTable = new DataTable();
            #region //读取数据文件
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            StreamReader m_streamReader = new StreamReader(fs);
            m_streamReader.BaseStream.Seek(0, SeekOrigin.Begin);
            string strLine = m_streamReader.ReadLine();

            int i = 0;
            while (strLine != null && strLine != "")
            {

                string[] strSplit = strLine.Split('|');

                if (i == 0)
                {
                    for (int p = 0; p < strSplit.Length; p++)
                    {
                        dataTable.Columns.Add("列" + p.ToString());
                    }

                }
                i++;

                dataTable.Rows.Add(strSplit);

                strLine = m_streamReader.ReadLine();
            }
            m_streamReader.Close();
            m_streamReader.Dispose();
            fs.Close();
            fs.Dispose();
            #endregion
            return dataTable;

        }

        private void btnispass_Click(object sender, EventArgs e)
        {
            //判断是否存在不在闭式时间内的数据

            if (comboBox2.Text.Equals("") || !comboBox2.Text.Equals("浦发银行"))
            {

                richTextBox6.SelectionColor = Color.White;
                richTextBox6.AppendText("=========================" + Environment.NewLine);
                richTextBox6.AppendText("当前我只能生成 “浦发银行”文件，请不要虐待我！");
                return;
            }

            this.btnispass.Enabled = false;
            Hashtable ht = new Hashtable();
            ht["开户银行"] = comboBox2.Text;
            ht["结算时间"] = Convert.ToDateTime(this.datetimejstime.Value).ToString("yyyy-MM-dd");
            //ht["方法名称"] = "dsishav"; 浦发银行-闭市时间外是否存在数据
            ht["方法名称"] = "浦发银行-闭市时间外是否存在数据";
            SRT_demo_Run(ht);
        }

        #region//RichTextBox处理的文本文档的内容
        private int _selectIndex;
        private int _selectLength;
        private void 剪切ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.richTextBox1.Cut();
        }

        private void 复制ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.richTextBox1.Copy();
        }

        private void 粘贴ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.richTextBox1.Paste();
        }

        private void 删除ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.richTextBox1.SelectedText = "";
        }

        private void 全选ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.richTextBox1.SelectAll();
        }

        private void SetTest(string str)
        {
            this.toolStripTextBox1.Text = str;
        }    //Search();

        private void SearchTest()
        {

            Search();

        }





        private void 工具栏ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            ToolStripMenuItem mm = (ToolStripMenuItem)sender;
            if (mm.Checked == true)
            {
                this.toolStrip1.Visible = true;
            }
            else
            {

                this.toolStrip1.Visible = false;
            }

        }


        private void 自动换行ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.自动换行ToolStripMenuItem.Checked == true)
            {
                this.richTextBox1.WordWrap = true;

            }
            else
            {
                this.richTextBox1.WordWrap = false;
            }

        }

        private void toolStripTextBox1_TextChanged(object sender, EventArgs e)//查找
        {
            FindIndex(0);
        }


        private int FindIndex(int startIndex)//查找方法
        {

            this.richTextBox1.Select(_selectIndex, _selectLength);//选中找到的文本
            this.richTextBox1.SelectionBackColor = this.richTextBox1.BackColor;//改变选中文本的颜色为系统颜色
            int index = this.richTextBox1.Find(this.toolStripTextBox1.Text, startIndex, RichTextBoxFinds.None);//得到符合文本的索引
            if (index != -1)
            {
                this.richTextBox1.Select(index, this.toolStripTextBox1.Text.Length);
                _selectIndex = index;//保存本次得到的文本
                _selectLength = this.toolStripTextBox1.Text.Length;//

                this.richTextBox1.SelectionBackColor = Color.Yellow;
            }

            return index;
        }

        private void Search()
        {
            int index = FindIndex(this.richTextBox1.SelectionStart + this.richTextBox1.SelectionLength);
            if (index == -1)
            {
                FindIndex(0);
            }

        }

        private void toolStripButton1_Click(object sender, EventArgs e)//下一个
        {
            Search();

        }
        private void toolStripComboBox1_Click(object sender, EventArgs e)//设置字体名称
        {

            this.richTextBox1.Font = new Font(this.toolStripComboBox1.SelectedItem.ToString(), this.richTextBox1.Font.Size);



        }

        private void toolStripComboBox2_Click(object sender, EventArgs e)//设置字体大小
        {
            this.richTextBox1.Font = new Font(this.richTextBox1.Font.Name, float.Parse(this.toolStripComboBox2.SelectedItem.ToString()));

        }
        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "副文本(*.txt)|*.txt";
            DialogResult result = ofd.ShowDialog();
            if (result == DialogResult.OK)
            {
                FileStream fs = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read);
                using (StreamReader sr = new StreamReader(fs))
                {
                    this.richTextBox1.Rtf = sr.ReadToEnd();
                }
            }


        }

        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Save();
        }
        private void Save()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "副文本(*.txt)|*.txt";
            sfd.InitialDirectory = "C:\\";
            DialogResult result = sfd.ShowDialog();
            if (result == DialogResult.OK)
            {
                using (StreamWriter sw = new StreamWriter(sfd.OpenFile()))
                {
                    sw.Write(this.richTextBox1.Rtf);
                    sw.Flush();
                }
            }
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void 字体ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FontDialog fd = new FontDialog();
            fd.Font = this.richTextBox1.Font;
            if (fd.ShowDialog() == DialogResult.OK)
            {
                this.richTextBox1.Font = fd.Font;

            }

        }

        private void 颜色ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == DialogResult.OK)
            {
                //this.richTextBox1.BackColor = cd.Color;
                this.richTextBox1.ForeColor = cd.Color;
            }
        }

        private void 新建ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (this.richTextBox1.Text != "")
            //{
            //    保存ToolStripMenuItem_Click(sender, e);
            //}
            if (this.richTextBox1.Text.Length == 0)
                return;
            DialogResult r = MessageBox.Show("是否保存?", "保存", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
            if (r == DialogResult.Yes)
            {
                Save();
                this.richTextBox1.Clear();
            }

        }






        #endregion

        private void button12_Click_1(object sender, EventArgs e)
        {

        }

        private void btnispass_Click_1(object sender, EventArgs e)
        {

        }

        private void toolStripComboBox4_Click(object sender, EventArgs e)
        {

        }

       
    }
}
