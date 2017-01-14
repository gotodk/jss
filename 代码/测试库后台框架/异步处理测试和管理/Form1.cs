using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FMipcClass;
using System.Threading;
using ServiceStack.Redis;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using FMDBHelperClass;
using System.Collections;
namespace 异步处理测试和管理
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

     

        bool runing = false;
        //新版
        private void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            runing = true;

            //开启一个线程，执行日志处理 
            ThreadStart savelog = delegate { RunQueue(); };
            Thread thread = new Thread(savelog);
            thread.Name = "RunQueue";
            thread.IsBackground = true;
            thread.Start();
        }


        //处理异步队列
        private void RunQueue()
        {
            RedisClient RC = FMDBHelperClass.RedisClass.GetRedisClient(null);
            while (runing)
            {
                Thread.Sleep(1);
                string LeftOneGUID = "0";
                try
                {
                    //队列中，从左边开始，取出1个，按顺序进行处理
                    //从这里直接连接Redis,写入异步处理队列

                    //取出并删除最左边的元素。没有的话，会阻塞这个线程和此项目的连接。
                    LeftOneGUID = RC.BlockingRemoveStartFromList("L:queIPC:RUN",new TimeSpan(0,5,0));
                    if (LeftOneGUID == null)
                    { }
                    else
                    {
                        //根据这个GUID，获取接口数据进行调用。 调用完成且接口没返回null(不代表业务成功)，就删除。否则保留下来。

                        //调用目标的基本信息
                        Dictionary<string, string> FF = RC.GetAllEntriesFromHash("H:queIPC:FF:" + LeftOneGUID);
                       
                        //参数
                        byte[][] CS = RC.ZRange("Z:queIPC:CS:" + LeftOneGUID, 0, -1);
                        int CScount = CS.Count();
                        object[] objectCS = new object[CScount];
                        for (int i = 0; i < CScount; i++) 
                        {
                            MemoryStream ms = new MemoryStream(CS[i]);
                            IFormatter bf = new BinaryFormatter();
                            objectCS[i] = bf.Deserialize(ms);
                            ms.Close();
                            ms.Dispose();
                        }

                        //开始调用
                        object re = null;
                        string Durls = "http://" + FF["JKhost"] + "/" + FF["JKpath"];
                        FMWScenter wsd = new FMWScenter(Durls + "?wsdl");
                        re = wsd.ExecuteQuery(FF["FFname"], objectCS);
                        if (re != null)
                        {
                            //异步执行完成没有返回null，删除关联数据
                            RC.Del(new string[] { "H:queIPC:FF:" + LeftOneGUID, "Z:queIPC:CS:" + LeftOneGUID });

                        }
                        else
                        {
                            //异步执行返回了null，通常都是没有成功
                            RC.PushItemToList("L:queIPC:RUNERR", LeftOneGUID);
                        }
                        
                    }
                }
                catch (Exception ex)
                {
                    //发生了错误，记录一下。目前只是简单的记录。以后再说
                    RC.PushItemToList("L:queIPC:RUNERR", LeftOneGUID);
                }
               
             
 
            }
            MessageBox.Show("。。。。。停了。。。。。。");
            //
        }



        private void button3_Click(object sender, EventArgs e)
        {
            
            runing = false;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
                RedisClient RC = FMDBHelperClass.RedisClass.GetRedisClient("OnlyOpenOneCheckRedis");
                RC.Set("str:OnlyOpenOneCheck:" + Program.RunAPP, System.Text.Encoding.UTF8.GetBytes("CanRun"));

 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //开启踢人
            RedisClient RC = FMDBHelperClass.RedisClass.GetRedisClient("OnlyOpenOneCheckRedis");
            RC.Set("str:TiRen", System.Text.Encoding.UTF8.GetBytes("T"));
            MessageBox.Show("。。。。。开启踢人完成。。。。。。");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //关闭踢人
            RedisClient RC = FMDBHelperClass.RedisClass.GetRedisClient("OnlyOpenOneCheckRedis");
            RC.Del("str:TiRen");
            MessageBox.Show("。。。。。关闭踢人完成。。。。。。");
        }

        private void btnResetRedisPKid_Click(object sender, EventArgs e)
        {
            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
            Hashtable htInput = new Hashtable();
            htInput.Add("@date", null);
            Hashtable htres = I_DBL.RunProc_CMD("[dbo].[AAA_getTableMaxNum_redis]", "tab", htInput);
            DataSet ds = new DataSet();
            if ((bool)htres["return_float"])
            {
                ds = (DataSet)htres["return_ds"];
            }
            else
            {
                ds = null;
            }

            if (ds != null && ds.Tables.Contains("tab") && ds.Tables["tab"].Rows.Count > 0)
            {
                RedisClient RC = RedisClass.GetRedisClient("OnlyOpenOneCheckRedis");
                string txt = "TableName       MaxNum       MaxId" + Environment.NewLine + "-------------------------------------" + Environment.NewLine;

                //-------计算有效期截止时间---------------
                DateTime dateStart = DateTime.Now;
                //获取当月最后一天的时间
                DateTime dateEnd = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1);
                //获取当前时间与当月最后一天相差的秒数作为键值的生命周期
                TimeSpan ts = dateEnd.Subtract(dateStart).Duration();
                string seconds = ts.TotalSeconds.ToString("0");

                using (IRedisTransaction IRT = RC.CreateTransaction())
                {//使用事务提交所有重置键值的操作语句
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        txt += dr["tname"].ToString() + "       " + dr["maxnum"].ToString() + "       " + dr["maxid"].ToString() + Environment.NewLine;
                        string key = "str:TablePK:" + dr["tname"].ToString() + DateTime.Now.ToString("yyyyMM");
                        IRT.QueueCommand(r => r.Set(key, Convert .ToInt32(dr["maxid"].ToString())));
                        IRT.QueueCommand(r => r.ExpireEntryAt(key, dateEnd));
                    }
                    IRT.Commit(); // 提交事务
                }
                //将待处理的表信息显示在界面上
                richTextBox1.Text = txt;
            }
            else
            {
                richTextBox1.Text = "没有需要处理的信息！";
            }
        }
    }
}
