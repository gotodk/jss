using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Threading;
using System.Data;
using 客户端主程序.Support;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using 客户端主程序.NewDataControl;
using System.Reflection;
using System.Net;

namespace 客户端主程序.DataControl
{

    /// <summary>
    /// 测试首页数据表加载线程类
    /// </summary>
    class RunThreadClassTest
    {
        //刷新间隔时间
        int sxjg = 5000;
        private int NumberPage;
        //向线程传递的回调参数
        private delegateForThread DForThread;
        //向线程传递的数据参数
        private Hashtable InPutHT;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="PHT">需要传入线程的参数</param>
        /// <param name="DFT">线程委托</param>
        public RunThreadClassTest(Hashtable PHT, delegateForThread DFT)
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
            DForThread = DFT;
            InPutHT = PHT;
            NumberPage = 30;
        }

       static string bdstr = "";

        /// <summary>
        /// 开始绑定(逐条绑定，已作废，不要删除)
        /// </summary>
        /// <param name="dstest"></param>
       private void BeginBangDing(DataSet dstest,string duibi)
        {
            //防止绑定冲突
            bdstr = "绑定中";
            Hashtable OutPutHT;

            bool CanBD;
            if (dstest != null && dstest.Tables[0].Rows.Count > 0)
            {
                CanBD = true;
            }
            else
            {
                CanBD = false;
            }

            //绑定间隔,防卡死
            int jiange = 2;


            //保持既定排序和横向滚动条不变
            OutPutHT = new Hashtable();
            OutPutHT["可以绑定"] = CanBD;
            OutPutHT["执行标记"] = "保持既定排序和横向滚动条不变";
            DForThread(OutPutHT);

            int hangshu = Convert.ToInt32(InPutHT["行数"]);

            //重写数值，新行数小的话，删除多余行。新行数大的话，增加新增行。
            //差额正值说明新数据行多
            int cha = dstest.Tables[0].Rows.Count - hangshu;
            if (cha >= 0)
            {
                int oldrowindex = hangshu - 1;
                for (int i = 0; i < hangshu; i++)
                {
                    Thread.Sleep(jiange);
                    OutPutHT = new Hashtable();
                    OutPutHT["可以绑定"] = CanBD;
                    OutPutHT["执行标记"] = "改变列";
                    OutPutHT["是否对比标志"] = duibi;
                    OutPutHT["i"] = i;
                    OutPutHT["值"] = dstest.Tables[0].Rows[i];
                    DForThread(OutPutHT);

                }
                if (hangshu == 0)
                {
                    Thread.Sleep(jiange);
                    OutPutHT = new Hashtable();
                    OutPutHT["可以绑定"] = CanBD;
                    OutPutHT["执行标记"] = "一次性增加所有行";
                    OutPutHT["值"] = dstest.Tables[0];
                    DForThread(OutPutHT);
                }
                else
                {
                    for (int p = 0; p < cha; p++)
                    {


                        Thread.Sleep(jiange);
                        OutPutHT = new Hashtable();
                        OutPutHT["可以绑定"] = CanBD;
                        OutPutHT["执行标记"] = "增加列";
                        OutPutHT["值"] = dstest.Tables[0].Rows[oldrowindex + p + 1];
                        DForThread(OutPutHT);
                    }
                }

            }
            if (cha < 0)//差额负值说明新数据行少
            {
                int oldrowindex = hangshu - 1;
                for (int i = 0; i < dstest.Tables[0].Rows.Count; i++)
                {
                    Thread.Sleep(jiange);
                    OutPutHT = new Hashtable();
                    OutPutHT["可以绑定"] = CanBD;
                    OutPutHT["执行标记"] = "改变列";
                    OutPutHT["是否对比标志"] = duibi;
                    OutPutHT["i"] = i;
                    OutPutHT["值"] = dstest.Tables[0].Rows[i];
                    DForThread(OutPutHT);
                }
                for (int p = 0; p < -cha; p++)
                {
                    Thread.Sleep(jiange);
                    OutPutHT = new Hashtable();
                    OutPutHT["可以绑定"] = CanBD;
                    OutPutHT["执行标记"] = "删除列";
                    OutPutHT["值"] = oldrowindex - p - 1;
                    DForThread(OutPutHT);

                }
            }

            //恢复排序和横向滚动条
            OutPutHT = new Hashtable();
            OutPutHT["可以绑定"] = CanBD;
            OutPutHT["执行标记"] = "恢复排序和横向滚动条";
            DForThread(OutPutHT);


            //重置行数
            if (dstest != null && dstest.Tables[0].Rows.Count > 0)
            {
                InPutHT["行数"] = dstest.Tables[0].Rows.Count;
            }
            else
            {
                InPutHT["行数"] = 0;
            }
           
            bdstr = "绑定结束";
        }


       /// <summary>
       /// 开始绑定(不逐条，一次性写入到界面上)
       /// </summary>
       /// <param name="dstest"></param>
       private void BeginBangDing_oncerun(DataSet dstest, string duibi)
       {
           //防止绑定冲突
           bdstr = "绑定中";
           Hashtable OutPutHT;

           bool CanBD;
           if (dstest != null && dstest.Tables[0].Rows.Count > 0)
           {
               CanBD = true;
           }
           else
           {
               CanBD = false;
           }

           //绑定间隔,防卡死
           //int jiange = 1;


           //保持既定排序和横向滚动条不变
           OutPutHT = new Hashtable();
           OutPutHT["可以绑定"] = CanBD;
           OutPutHT["执行标记"] = "保持既定排序和横向滚动条不变";
           DForThread(OutPutHT);

           int hangshu = Convert.ToInt32(InPutHT["行数"]);

           //重写数值，新行数小的话，删除多余行。新行数大的话，增加新增行。
           //差额正值说明新数据行多
 
           //Thread.Sleep(jiange);
           OutPutHT = new Hashtable();
           OutPutHT["可以绑定"] = CanBD;
           OutPutHT["执行标记"] = "一次性处理所有数据";
           OutPutHT["是否对比标志"] = duibi;
           OutPutHT["值"] = dstest.Tables[0];
           OutPutHT["来源"] = InPutHT["来源"];
           DForThread(OutPutHT);

           //恢复排序和横向滚动条
           OutPutHT = new Hashtable();
           OutPutHT["可以绑定"] = CanBD;
           OutPutHT["执行标记"] = "恢复排序和横向滚动条";
           DForThread(OutPutHT);

           GC.Collect();

           //重置行数
           if (dstest != null && dstest.Tables[0].Rows.Count > 0)
           {
               InPutHT["行数"] = dstest.Tables[0].Rows.Count;
           }
           else
           {
               InPutHT["行数"] = 0;
           }

           bdstr = "绑定结束";
       }


       /// <summary>
       /// 测试滚动
       /// </summary>
       public void GunDong()
       {
           DataSet dstestCOPY = Program.AllData.Clone();
           int BeginIndex = Program.nowIndex; //要过滤的开始索引
           for (int i = BeginIndex; i < BeginIndex + NumberPage; i++)
           {
               if (Program.AllData.Tables[0].Rows.Count > 0 && i < Program.AllData.Tables[0].Rows.Count)
               {
                   dstestCOPY.Tables[0].Rows.Add(Program.AllData.Tables[0].Rows[i].ItemArray);
               }
               else
               {
                   int lie = Program.AllData.Tables[0].Columns.Count;
                   object[] ot_null = new object[lie];
                   for (int s = 0; s < ot_null.Length; s++)
                   {
                       ot_null[s] = null;
                   }
                   dstestCOPY.Tables[0].Rows.Add(ot_null);
               }
           }
           BeginBangDing_oncerun(dstestCOPY, "否");

       }

        /// <summary>
        /// 开始执行线程，加载默认商品(主区域)
        /// </summary>
        public void BeginRun()
        {
            Thread.Sleep(10);
            WebClient client = new WebClient();
            string url = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[2] + "/rediskeygogo.aspx?key=str:HomePageByte";
            string url_jhsp = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[2] + "/rediskeygogo.aspx?key=str:JHXP";


            //读取简化商品，用于键盘精灵
            if (Program.ZhenAllData == null)
            {
                DataSet dstest_jhxp = null;
                try
                {
                    Stream data = client.OpenRead(url_jhsp);
                    StreamReader reader = new StreamReader(data);
                    byte[] mybyte = new byte[3048576];
                    int allbyte = (int)mybyte.Length;
                    int startbyte = 0;
                    while (allbyte > 0)
                    {
                        int n = data.Read(mybyte, startbyte, allbyte);
                        if (n == 0)
                        {
                            break;
                        }
                        allbyte -= n;
                        startbyte += n;
                    }


                    data.Close();

                    Program.ZhenAllData = Helper.Byte2DataSet(mybyte);


                }
                catch (Exception ex)
                {
                    Program.ZhenAllData = null;

                }
       


            }

            while (Thread.CurrentThread.Priority != ThreadPriority.Lowest)
            {


                Thread.Sleep(1);
                //用于测试
                //Support.StringOP.WriteLog("临时调试：" + Thread.CurrentThread.Name);


                //读取大盘
                DataSet dstest = null;
                try
                {
                    Stream data = client.OpenRead(url);
                    StreamReader reader = new StreamReader(data);
                    byte[] mybyte = new byte[3048576];
                    int allbyte = (int)mybyte.Length;
                    int startbyte = 0;
                    while (allbyte > 0)
                    {
                        int n = data.Read(mybyte, startbyte, allbyte);
                        if (n == 0)
                        {
                            break;
                        }
                        allbyte -= n;
                        startbyte += n;
                    }


                    data.Close();

                    dstest = Helper.Byte2DataSet(mybyte);


                }
                catch(Exception ex) 
                {
                    dstest = null;
                    //Support.StringOP.WriteLog("临时调试(大盘默认数据)：" + ex.ToString());
                    continue;
                }






                
 
                //判断和赋值
                if (dstest == null || dstest.Tables.Count < 1)
                {
                    Thread.Sleep(sxjg);
                    continue;
                }
 
                //赋值给公共变量并释放资源
                lock (Program.locker_for_AllData)//锁
                {
                    Program.AllData = dstest.Copy();
                }
                dstest.Clear();
                dstest.Dispose();


                Hashtable OutPutHT = new Hashtable();
                OutPutHT["大盘竖向滚动条最大值"] = Program.AllData.Tables[0].Rows.Count;
                if (Thread.CurrentThread.Priority == ThreadPriority.Lowest) //用低级别线程特殊标记该停止了
                { return; }
                DForThread(OutPutHT);

                DataSet dstestCOPY = Program.AllData.Clone();




                int BeginIndex = Program.nowIndex; //要过滤的开始索引
                for (int i = BeginIndex; i < BeginIndex + NumberPage; i++)
                {
                    if (Program.AllData.Tables[0].Rows.Count > 0 && i < Program.AllData.Tables[0].Rows.Count)
                    {
                        dstestCOPY.Tables[0].Rows.Add(Program.AllData.Tables[0].Rows[i].ItemArray);
                    }
                    else
                    {
                        int lie = Program.AllData.Tables[0].Columns.Count;
                        object[] ot_null = new object[lie];
                        for (int s = 0; s < ot_null.Length; s++)
                        {
                            ot_null[s] = null;
                        }
                        dstestCOPY.Tables[0].Rows.Add(ot_null);
                    }
                }

                if (Thread.CurrentThread.Priority == ThreadPriority.Lowest) //用低级别线程特殊标记该停止了
                { return; }
                    //开始绑定
                BeginBangDing_oncerun(dstestCOPY, "差异底色");


                
                Thread.Sleep(sxjg);
            }
          
        }


        /// <summary>
        /// 开始执行线程,用于自选商品(主区域)
        /// </summary>
        public void BeginRunZXSP()
        {

            while (Thread.CurrentThread.Priority != ThreadPriority.Lowest)
            {
         


                Thread.Sleep(50);
                string DLYX = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString().Trim();
 


                NewDataControl.WebServicesCenter2013 WSC2013 = new NewDataControl.WebServicesCenter2013();
                //byte[] bb = WSC2013.RunAtServices_YS("test_ds", new object[] { "自选**!!" + DLYX });
                //已移植可替换(已替换)
                byte[] bb = WSC2013.RunAtServices_YS("C获取大盘", new object[] { "自选**!!" + DLYX });



                DataSet dstest = Helper.Byte2DataSet(bb);

                if (dstest == null || dstest.Tables.Count < 1)
                {
                    Thread.Sleep(sxjg);
                    continue;
                }
                lock (Program.locker_for_AllData)//锁
                {
                    Program.AllData = dstest;
                }
                Hashtable OutPutHT = new Hashtable();
                OutPutHT["大盘竖向滚动条最大值"] = dstest.Tables[0].Rows.Count;
                if (Thread.CurrentThread.Priority == ThreadPriority.Lowest) //用低级别线程特殊标记该停止了
                { return; }
                DForThread(OutPutHT);

                DataSet dstestCOPY = dstest.Clone();
                int BeginIndex = Program.nowIndex; //要过滤的开始索引
                for (int i = BeginIndex; i < BeginIndex + NumberPage; i++)
                {
                    if (Program.AllData.Tables[0].Rows.Count > 0 && i < Program.AllData.Tables[0].Rows.Count)
                    {
                        dstestCOPY.Tables[0].Rows.Add(Program.AllData.Tables[0].Rows[i].ItemArray);
                    }
                    else
                    {
                        int lie = Program.AllData.Tables[0].Columns.Count;
                        object[] ot_null = new object[lie];
                        for (int s = 0; s < ot_null.Length; s++)
                        {
                            ot_null[s] = null;
                        }
                        dstestCOPY.Tables[0].Rows.Add(ot_null);
                    }
                }

                //开始绑定
                if (Thread.CurrentThread.Priority == ThreadPriority.Lowest) //用低级别线程特殊标记该停止了
                { return; }
                BeginBangDing_oncerun(dstestCOPY, "差异底色");

                Thread.Sleep(sxjg);
            }
        }



        /// <summary>
        /// 开始执行线程,用于显示二级分类下商品(主区域)
        /// </summary>
        public void BeginRunErJi()
        {
            while (Thread.CurrentThread.Priority != ThreadPriority.Lowest)
            {

                Thread.Sleep(50);

    

                NewDataControl.WebServicesCenter2013 WSC2013 = new NewDataControl.WebServicesCenter2013();
                //byte[] bb = WSC2013.RunAtServices_YS("test_ds", new object[] { "二级分类**!!" + InPutHT["分类编号"].ToString() });
                //已移植可替换(已替换)
                byte[] bb = WSC2013.RunAtServices_YS("C获取大盘", new object[] { "二级分类**!!" + InPutHT["分类编号"].ToString() });

                DataSet dstest = Helper.Byte2DataSet(bb);



                if (dstest == null || dstest.Tables.Count < 1)
                {
                    Thread.Sleep(sxjg);
                    continue;
                }
                lock (Program.locker_for_AllData)//锁
                {
                    Program.AllData = dstest;
                }
                Hashtable OutPutHT = new Hashtable();
                OutPutHT["大盘竖向滚动条最大值"] = dstest.Tables[0].Rows.Count;
                if (Thread.CurrentThread.Priority == ThreadPriority.Lowest) //用低级别线程特殊标记该停止了
                { return; }
                DForThread(OutPutHT);

                DataSet dstestCOPY = dstest.Clone();
                int BeginIndex = Program.nowIndex; //要过滤的开始索引
                for (int i = BeginIndex; i < BeginIndex + NumberPage; i++)
                {
                    if (Program.AllData.Tables[0].Rows.Count > 0 && i < Program.AllData.Tables[0].Rows.Count)
                    {
                        dstestCOPY.Tables[0].Rows.Add(Program.AllData.Tables[0].Rows[i].ItemArray);
                    }
                    else
                    {
                        int lie = Program.AllData.Tables[0].Columns.Count;
                        object[] ot_null = new object[lie];
                        for (int s = 0; s < ot_null.Length; s++)
                        {
                            ot_null[s] = null;
                        }
                        dstestCOPY.Tables[0].Rows.Add(ot_null);
                    }
                }
                //开始绑定
                if (Thread.CurrentThread.Priority == ThreadPriority.Lowest) //用低级别线程特殊标记该停止了
                { return; }
                BeginBangDing_oncerun(dstestCOPY, "差异底色");

                Thread.Sleep(sxjg);
            }
       
        }



        /// <summary>
        /// 获得自选商品本地临时表
        /// </summary>
        public void BeginRunInitZXSPtemp()
        {

            string DLYX = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString().Trim();

            Thread.Sleep(10);
            //用于测试
            //访问远程服务
            NewDataControl.WebServicesCenter2013 WSC2013 = new NewDataControl.WebServicesCenter2013();

            DataSet rere = WSC2013.RunAtServices("C临时自选商品表", new object[] { DLYX });

            //初始化
            PublicDS.PublisDsZXSP = rere;

            //填充传入参数哈希表
            //Hashtable OutPutHT = new Hashtable();
            //OutPutHT["执行结果"] = rere;//测试参数
            //DForThread(OutPutHT);

        }


        /// <summary>
        /// 加入或删除自选商品
        /// </summary>
        public void BeginRunZXSPedit()
        {

            string DLYX = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString().Trim();

                Thread.Sleep(50);
                //用于测试
                //访问远程服务
                NewDataControl.WebServicesCenter2013 WSC2013 = new NewDataControl.WebServicesCenter2013();
               // string rere = WSC2013.RunAtServices_ZX("ZXSPedit", new object[] { DLYX, InPutHT["商品编号"].ToString(), InPutHT["操作"].ToString() });
                //已移植可替换(已替换)
                string rere = WSC2013.RunAtServices_ZX("C操作自选商品", new object[] { DLYX, InPutHT["商品编号"].ToString(), InPutHT["操作"].ToString() });
                
            
            //填充传入参数哈希表
                Hashtable OutPutHT = new Hashtable();
                OutPutHT["执行结果"] = rere;//测试参数
                OutPutHT["所操作商品编号"] = InPutHT["商品编号"].ToString();//测试参数
                DForThread(OutPutHT);

        }







        /// <summary>
        /// 开始执行线程，加载成交详情
        /// </summary>
        public void BeginRunCJXQ()
        {


            Thread.Sleep(50);
      
 

            DataTable dt = StringOP.GetDataTableFormHashtable(InPutHT);
 


            NewDataControl.WebServicesCenter2013 WSC2013 = new NewDataControl.WebServicesCenter2013();
            //byte[] bb = WSC2013.RunAtServices_YS("test_cjxx", new object[] { dt });
            //已移植可替换(已替换)
            byte[] bb = WSC2013.RunAtServices_YS("C获得成交信息", new object[] { "" });

            DataSet dstest = Helper.Byte2DataSet(bb);




            if (dstest == null || dstest.Tables.Count < 1)
            {
                Thread.Sleep(sxjg);
                return;
            }

 
            lock (Program.locker_for_AllData)//锁
            {
                Program.AllData = dstest;
            }
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["大盘竖向滚动条最大值"] = dstest.Tables[0].Rows.Count;
            if (Thread.CurrentThread.Priority == ThreadPriority.Lowest) //用低级别线程特殊标记该停止了
            { return; }
            DForThread(OutPutHT);

            DataSet dstestCOPY = dstest.Clone();
            int BeginIndex = Program.nowIndex; //要过滤的开始索引
            for (int i = BeginIndex; i < BeginIndex + NumberPage; i++)
            {
                if (Program.AllData.Tables[0].Rows.Count > 0 && i < Program.AllData.Tables[0].Rows.Count)
                {
                    dstestCOPY.Tables[0].Rows.Add(Program.AllData.Tables[0].Rows[i].ItemArray);
                }
                else
                {
                    int lie = Program.AllData.Tables[0].Columns.Count;
                    object[] ot_null = new object[lie];
                    for (int s = 0; s < ot_null.Length; s++)
                    {
                        ot_null[s] = null;
                    }
                    dstestCOPY.Tables[0].Rows.Add(ot_null);
                }
            }


            //开始绑定
            if (Thread.CurrentThread.Priority == ThreadPriority.Lowest) //用低级别线程特殊标记该停止了
            { return; }
            BeginBangDing_oncerun(dstestCOPY, "否");
             
           
        }

 

                /// <summary>
        /// 开始执行线程，加载键盘精灵搜索
        public void Beginjpjl()
        {
            Thread.Sleep(50);
            string nowstr = InPutHT["新值"].ToString();
            if (nowstr != "")
            {
                Hashtable OutPutHT = new Hashtable();
                DataTable dt = Program.ZhenAllData.Tables[0];
                if (dt == null )
                {
                    Thread.Sleep(sxjg);
                    return;
                }
                //DateTime time1 = DateTime.Now;

                //var query = (from Items in dt.AsEnumerable()
                //             where (Items["商品编号"].ToString().ToLower().IndexOf(nowstr.ToLower()) >= 0
                //             || Items["商品名称"].ToString().ToLower().IndexOf(nowstr.ToLower()) >= 0 || Items["简拼"].ToString().ToLower().IndexOf(nowstr.ToLower()) >= 0 || Items["全拼"].ToString().ToLower().IndexOf(nowstr.ToLower()) >= 0)
                //             select Items).Take(10);

                
                //if (query.Count() > 0)
                //    OutPutHT["查询结果"] = query.CopyToDataTable();
                //else
                //    OutPutHT["查询结果"] = null;
                //DateTime time2 = DateTime.Now;

                //DateTime time3 = DateTime.Now;
                DataTable dtnew = dt.Clone() ;
                for (int p = 0; p < dt.Rows.Count; p++)
                {
                    if (dt.Rows[p]["商品编号"].ToString().ToLower().IndexOf(nowstr.ToLower()) >= 0
                             || dt.Rows[p]["商品名称"].ToString().ToLower().IndexOf(nowstr.ToLower()) >= 0 || dt.Rows[p]["简拼"].ToString().ToLower().IndexOf(nowstr.ToLower()) >= 0 || dt.Rows[p]["全拼"].ToString().ToLower().IndexOf(nowstr.ToLower()) >= 0)
                    {
                        dtnew.Rows.Add(dt.Rows[p].ItemArray);
                        if (dtnew.Rows.Count >= 200)
                        {
                            break;
                        }
                    }
                }

 
                if (dtnew.Rows.Count > 0)
                    OutPutHT["查询结果"] = dtnew;
                else
                    OutPutHT["查询结果"] = null;

                    DForThread(OutPutHT);

            }
          


        }

    }
}
