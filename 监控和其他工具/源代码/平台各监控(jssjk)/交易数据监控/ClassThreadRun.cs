using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Threading;
using System.Data;
using System.IO;

namespace 交易数据监控
{
    class ClassThreadRun
    {
                //向线程传递的回调参数
        private delegateForThread DForThread;
        //向线程传递的数据参数
        private Hashtable InPutHT;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="PHT">需要传入线程的参数</param>
        /// <param name="DFT">线程委托</param>
        public ClassThreadRun(Hashtable PHT, delegateForThread DFT)
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
            DForThread = DFT;
            InPutHT = PHT;
        }

        /// <summary>
        /// 开始线程(正常)
        /// </summary>
        public void BeginRun()
        {

            DateTime dataTime=DateTime.Parse("1900-01-01");
            while (true)
            {
                //一秒跑一次
                Thread.Sleep(1000);

                try
                {
                    DataSet DSmain = (DataSet)(InPutHT["监控列表集合"]);
                    int allnum = DSmain.Tables["监控列表"].Rows.Count;
                    int errN = 0;
                 
                    //每天凌晨监控运行的时间，实际上是每天凌晨多一点才执行。
                    DateTime timeRun = DateTime.Parse("23:59");
  
                    if (DateTime.Now > timeRun)
                    {
                        //暂停五分钟后执行监控，目的是让监控零点多一点跑一次
                        Thread.Sleep(1000 * 60 * 5);

                        //开始跑监控
                        for (int i = 0; i < allnum; i++)
                        {
                            //只蓝色的
                            if (DSmain.Tables["监控列表"].Rows[i]["监控类型"].ToString() == "蓝色")
                            {
                                bool thisok = RunS(DSmain.Tables["监控列表"].Rows[i]);
                                if (!thisok)
                                {
                                    errN++;
                                    dataTime = DateTime.Now;
                                    break;
                                }
                            }
                        }

                         

                    }

                    //开始跑监控
                    for (int i = 0; i < allnum; i++)
                    {
                        //只蓝色的
                        if (DSmain.Tables["监控列表"].Rows[i]["监控类型"].ToString() == "红色")
                        {
                            bool thisok = RunS(DSmain.Tables["监控列表"].Rows[i]);
                            if (!thisok)
                            {
                                errN++;
                            }
                        }
                    }

                  
                    if (errN > 0 || (!dataTime.Equals(DateTime.Parse("1900-01-01")) && (DateTime.Now - dataTime).Hours < 2))
                    {
                        //代表至少一个监控发生了错误，需要更新短信提醒字段为当前时间(原来是null才更新);

                        WebServicesCenter2013 wsc2013 = new WebServicesCenter2013();
                        Hashtable hastTableParams = new Hashtable();
                        hastTableParams["线程编号"] = DSmain.Tables["监控列表"].Rows[0]["线程编号"].ToString();
                        DataTable dt = StringOP.GetDataTableFormHashtable(hastTableParams);
                        object[] cs = { dt };
                        //已移植可替换(已替换) wyh 2014.07.08
                        DataSet DSre = wsc2013.RunAtServices("监控异常", cs);//SupervisoryControlAbnormal
                    }
                    else
                    {
                        //代表本次执行的所有监控均正常，需要更新短信提醒字段为null;
                        WebServicesCenter2013 wsc2013 = new WebServicesCenter2013();
                        Hashtable hastTableParams = new Hashtable();
                        hastTableParams["线程编号"] = DSmain.Tables["监控列表"].Rows[0]["线程编号"].ToString();
                        DataTable dt = StringOP.GetDataTableFormHashtable(hastTableParams);
                        object[] cs = { dt };
                        //已移植可替换(已替换) wyh 2014.07.08
                        DataSet DSre = wsc2013.RunAtServices("监控正常", cs);
                    
                    }
                }
                catch (Exception ex)
                {
                    //代表至少一个监控发生了错误，需要更新短信提醒字段为当前时间(原来是null才更新);
                    SaveLog("最外层错误", ex.ToString(), "", "", "");
                }
              
            }
        }

        /// <summary>
        /// 开始线程(就一次)
        /// </summary>
        public void BeginRun_now()
        {

            try
            {
                DataSet DSmain = (DataSet)(InPutHT["监控列表集合"]);
                int allnum = DSmain.Tables["监控列表"].Rows.Count;
                int errN = 0;

                for (int i = 0; i < allnum; i++)
                {
                    //只执行选中的
                    if ((bool)(DSmain.Tables["监控列表"].Rows[i]["选中"]))
                    {
                        bool thisok = RunS(DSmain.Tables["监控列表"].Rows[i]);
                        if (!thisok)
                        {
                            errN++;
                        }
                    }
                }
                if (errN > 0)
                {
                    //这里不用处理，不是正常监控，是就一次的
                }
                else
                {
                    //这里不用处理，不是正常监控，是就一次的
                }
            }
            catch (Exception ex)
            {
                SaveLog("最外层错误", ex.ToString(), "", "", "");
            }
            //回调
            if (DForThread != null)
            {
                //填充传入参数哈希表
                Hashtable OutPutHT = new Hashtable();
                OutPutHT["执行任务名称"] = "本次执行完成";
                OutPutHT["开始时间"] = "";
                OutPutHT["结束时间"] = "";
                OutPutHT["执行结果"] = "";
                OutPutHT["其他描述"] = "";
                DForThread(OutPutHT);
            }
        }

        /// <summary>
        /// 开始调用运行某个监控
        /// </summary>
        /// <param name="dr">传入数据集某一行</param>
        /// <returns></returns>
        private bool RunS(DataRow dr)
        {
            DateTime dtbegin = DateTime.Now;
            DateTime dtend = DateTime.Now;
            try
            {
                string ServicesName = dr["远程方法"].ToString();
                switch (ServicesName)
                {
                    case "xxxxxxx":

                        //处理特殊监控标志

                        return true;
                        break;
                    default:
                        dtbegin = DateTime.Now;
                        WebServicesCenter2013 wsc2013 = new WebServicesCenter2013();
                        DataSet DSre = wsc2013.RunAtServices(dr["远程方法"].ToString(), new object[]{""});
                        dtend = DateTime.Now;
                        SaveLog(dr["监控名称"].ToString(), DSre.Tables["返回值单条"].Rows[0]["执行结果"].ToString() + DSre.Tables["返回值单条"].Rows[0]["提示文本"].ToString(), "", dtbegin.ToString(), dtend.ToString());
                        if (DSre.Tables["返回值单条"].Rows[0]["执行结果"].ToString().IndexOf("ok") >= 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }

                        break;

                }

            }
            catch (Exception ex)
            {
                dtend = DateTime.Now;
                SaveLog(dr["监控名称"].ToString(), ex.ToString(), "", dtbegin.ToString(), dtend.ToString());
                return false;
            }
        }

        /// <summary>
        /// 回调和保存日志
        /// </summary>
        /// <param name="name">执行任务名称</param>
        /// <param name="restr">执行结果</param>
        /// <param name="otherstr">其他描述</param>
        /// <param name="begintime">开始时间</param>
        /// <param name="endtime">结束时间</param>
        private void SaveLog(string name, string restr, string otherstr, string begintime, string endtime)
        {
            //回调
            if (DForThread != null)
            {
                //填充传入参数哈希表
                Hashtable OutPutHT = new Hashtable();
                OutPutHT["执行任务名称"] = name;
                OutPutHT["开始时间"] = begintime;
                OutPutHT["结束时间"] = endtime;
                OutPutHT["执行结果"] = restr;
                OutPutHT["其他描述"] = otherstr;
                DForThread(OutPutHT);
            }

            //正常日志
            StringOP.WriteLog("执行任务名称:" + name + ",开始时间:" + begintime + ",结束时间:" + endtime + ",执行结果:" + restr + ",其他描述:" + otherstr + "", name);


        }

    }
}
