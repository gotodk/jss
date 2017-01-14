using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Threading;
using System.Data;
using System.IO;

namespace ClassLibraryBusinessMonitor
{

    /// <summary>
    /// 测试首页数据表加载线程类
    /// </summary>
    public class RunThreadClassTestBusinessMonitor
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
        public RunThreadClassTestBusinessMonitor(Hashtable PHT, delegateForThread DFT)
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
            DForThread = DFT;
            InPutHT = PHT;
        }

        /// <summary>
        /// 开始执行线程(定时器)
        /// </summary>
        public void BeginRun()
        {

            while (true)
            {
                try
                {
                    //一秒跑一次
                    Thread.Sleep(5000);

                    //每天凌晨监控运行的时间，实际上是每天凌晨多一点才执行。
                    DateTime timeRun = DateTime.Parse("23:59");
                    if (DateTime.Now > timeRun)
                    {
                        //暂停五分钟后执行监控，目的是让监控零点多一点跑一次
                        Thread.Sleep(1000 * 60 * 5);
                        //开始跑监控
                        DateTime dtbegin = DateTime.Now;

                        dtbegin = DateTime.Now;
                        Run();



                    }


                    //每秒运行一次的监控
                    RunSS();

                }
                catch (Exception ex)
                {
                    SaveLog("运行错误(BeginRun)：", ex.ToString(), "", DateTime.Now.ToString(), DateTime.Now.ToString());
                }

            }

        }


        /// <summary>
        /// 开始执行线程(立刻执行，只跑一次)
        /// </summary>
        public void BeginRun_now()
        {
            try
            {
                Thread.Sleep(500);

                DateTime dtbegin = DateTime.Now;
                DateTime dtend = DateTime.Now;

                string f = InPutHT["跑啥"].ToString();
                switch (f)
                {
                    case "超期未缴纳履约保证金监控":
                        //超期未缴纳履约保证金监控
                        JK_Chaoqifeibiao2013();
                        break;
                    case "延迟发货、延迟发票扣款的处理":
                        //延迟发货、延迟发票扣款的处理
                        JK_Yanchi2013();
                        break;
                    case "计算经纪人收益扣税":
                        //计算经纪人收益扣税
                        JK_Koushui2013();
                        break;
                    case "合同期满处理":
                        //合同期满处理
                        JK_Hetong2013();
                        break;
                    case "提醒后自动签收":
                        //提醒后自动签收
                        JK_Qianshou2013();
                        break;
                    case "新版中标监控":
                        //新版中标监控
                        JK_ZhongBiao2013();
                        break;
                    case "即时交易":
                        JK_ZhongBiao2013_JSJY();
                        break;
                    case "预订单过期监控":
                        JK_YDDGQ();
                        break;
                    case "PINMAC":
                        Get26700();
                        break;
                    case "DaPan":
                        JK_GetDaPan2013();
                        break;
                    default:
                        //超期未缴纳履约保证金监控
                        JK_Chaoqifeibiao2013();
                        //延迟发货、延迟发票扣款的处理
                        JK_Yanchi2013();
                        //计算经纪人收益扣税
                        JK_Koushui2013();
                        //合同期满处理
                        JK_Hetong2013();
                        //提醒后自动签收
                        JK_Qianshou2013();
                        //新版中标监控
                        JK_ZhongBiao2013();
                        //即时交易
                        JK_ZhongBiao2013_JSJY();
                        //过期
                        JK_YDDGQ();
                        break;
                }











                //回调
                if (DForThread != null)
                {
                    dtend = DateTime.Now;

                    //填充传入参数哈希表
                    Hashtable OutPutHT = new Hashtable();
                    OutPutHT["执行任务名称"] = "监控执行完成";
                    OutPutHT["开始时间"] = dtbegin;
                    OutPutHT["结束时间"] = dtend;
                    OutPutHT["执行结果"] = "监控执行完成";
                    OutPutHT["其他描述"] = "";
                    OutPutHT["执行方式"] = InPutHT["执行方式"];
                    DForThread(OutPutHT);
                }

            }
            catch (Exception ex)
            {
                SaveLog("运行错误(BeginRun_now)：", ex.ToString(), "", DateTime.Now.ToString(), DateTime.Now.ToString());
            }
        }



        /// <summary>
        /// 每秒运行一次的监控，真正的业务运行代码
        /// </summary>
        private void RunSS()
        {
            //新版中标监控
            JK_ZhongBiao2013();
            JK_ZhongBiao2013_JSJY();
            //大盘
            JK_GetDaPan2013();
        }

        /// <summary>
        /// 新版中标监控
        /// </summary>
        private void JK_ZhongBiao2013()
        {
            DateTime dtbegin = DateTime.Now;
            DateTime dtend = DateTime.Now;
            
            //开始业务代码
            WebServicesCenter2013 WSC2013 = new WebServicesCenter2013();
            DataSet DSre = WSC2013.RunAtServices("serverZBJK", null);
            dtend = DateTime.Now;
            SaveLog("中标监控", DSre.Tables["返回值单条"].Rows[0]["执行结果"].ToString() + DSre.Tables["返回值单条"].Rows[0]["提示文本"].ToString(), "", dtbegin.ToString(), dtend.ToString());



        }

        /// <summary>
        /// 即时交易监控
        /// </summary>
        private void JK_ZhongBiao2013_JSJY()
        {
            DateTime dtbegin = DateTime.Now;
            WebServicesCenter2013 wsc2013 = new WebServicesCenter2013();
            DataSet DSre = wsc2013.RunAtServices("serverZBJK_JSJY", null);
            DateTime dtend = DateTime.Now;
            SaveLog("中标监控_即时交易", DSre.Tables["返回值单条"].Rows[0]["执行结果"].ToString() + DSre.Tables["返回值单条"].Rows[0]["提示文本"].ToString(), "", dtbegin.ToString(), dtend.ToString());
        }



        /// <summary>
        /// 预订单过期
        /// </summary>
        private void JK_YDDGQ()
        {
            DateTime dtbegin = DateTime.Now;
            WebServicesCenter2013 wsc2013 = new WebServicesCenter2013();
            DataSet DSre = wsc2013.RunAtServices("serverGQJK", null);
            DateTime dtend = DateTime.Now;
            SaveLog("预订单过期监控", DSre.Tables["返回值单条"].Rows[0]["执行结果"].ToString() + DSre.Tables["返回值单条"].Rows[0]["提示文本"].ToString(), "", dtbegin.ToString(), dtend.ToString());
        }

        /// <summary>
        /// 超期未缴纳履约保证金监控
        /// </summary>
        private void JK_Chaoqifeibiao2013()
        {
            DateTime dtbegin = DateTime.Now;
            DateTime dtend = DateTime.Now;

            //开始业务代码
            WebServicesCenter2013 WSC2013 = new WebServicesCenter2013();
            DataSet DSre = WSC2013.RunAtServices("serverWJNLYBZJJK", null);
            //保存监控执行日志
            dtend = DateTime.Now;
            SaveLog("超期未缴纳履约保证金监控", DSre.Tables["返回值单条"].Rows[0]["执行结果"].ToString() +"提示文本："+ DSre.Tables["返回值单条"].Rows[0]["提示文本"].ToString(), "", dtbegin.ToLocalTime().ToString(), dtend.ToLocalTime().ToString());
        }


        /// <summary>
        /// 重新生成大盘监控
        /// </summary>
        private void JK_GetDaPan2013()
        {
            DateTime dtbegin = DateTime.Now;
            DateTime dtend = DateTime.Now;

            //开始业务代码
            WebServicesCenter2013 WSC2013 = new WebServicesCenter2013();
            DataSet DSre = WSC2013.RunAtServices("CreatNewDapanListTXT", null);
            //保存监控执行日志
            dtend = DateTime.Now;
            SaveLog("重新生成大盘监控", DSre.Tables["返回值单条"].Rows[0]["执行结果"].ToString() + "提示文本：" + DSre.Tables["返回值单条"].Rows[0]["提示文本"].ToString(), "", dtbegin.ToLocalTime().ToString(), dtend.ToLocalTime().ToString());
        }

        /// <summary>
        /// 交换加密妙药
        /// </summary>
        private void Get26700()
        {
            DateTime dtbegin = DateTime.Now;
            WebServicesCenter2013 ws = new WebServicesCenter2013();
            DataSet dsreturn = ws.RunAtServices("GetNewKey", null);
            DateTime dtend = DateTime.Now;
            try
            {
                if (dsreturn.Tables["返回值单条"].Rows[0]["执行结果"].ToString().Trim() == "ok")
                {
                    string path = "C:\\PinKey.txt";
                    string ls = dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"].ToString();
                    string pin = dsreturn.Tables["返回值单条"].Rows[0]["附件信息2"].ToString();
                    string mac = dsreturn.Tables["返回值单条"].Rows[0]["附件信息3"].ToString();
                    string Content = ls + "|" + pin + "|" + mac;
                    File.WriteAllText(path, Content);

                    dtend = DateTime.Now;
                    //填充传入参数哈希表
                    Hashtable OutPutHT = new Hashtable();
                    OutPutHT["执行任务名称"] = "交换密钥成功" + "\r\n";
                    OutPutHT["开始时间"] = dtbegin + "\r\n";
                    OutPutHT["结束时间"] = dtend + "\r\n";
                    OutPutHT["执行结果"] = "交换密钥成功" + "\r\n";
                    OutPutHT["其他描述"] = "交换密钥成功" + "\r\n";
                    OutPutHT["执行方式"] = "强制替换运行机器C盘的PinKey.txt文件，如果不存在，则创建。" + "\r\n";
                    DForThread(OutPutHT);
                }
                else
                {
                    dtend = DateTime.Now;
                    //填充传入参数哈希表
                    Hashtable OutPutHT = new Hashtable();
                    OutPutHT["执行任务名称"] = "交换密钥失败" + "\r\n";
                    OutPutHT["开始时间"] = dtbegin + "\r\n";
                    OutPutHT["结束时间"] = dtend + "\r\n";
                    OutPutHT["执行结果"] = "交换密钥失败" + "\r\n";
                    OutPutHT["其他描述"] = dsreturn.Tables["返回值单条"].Rows[0]["提示文本"].ToString() + "\r\n";
                    OutPutHT["执行方式"] = "强制替换运行机器C盘的PinKey.txt文件，如果不存在，则创建。" + "\r\n";
                    DForThread(OutPutHT);
                }
            }
            catch(Exception ex)
            {
                Hashtable OutPutHT = new Hashtable();
                OutPutHT["执行任务名称"] = "交换密钥成功，文件流操作失败" + "\r\n";
                OutPutHT["开始时间"] = dtbegin + "\r\n";
                OutPutHT["结束时间"] = dtend + "\r\n";
                OutPutHT["执行结果"] = "交换密钥成功，文件流操作失败" + "\r\n";
                OutPutHT["其他描述"] = ex.Message + "\r\n";
                OutPutHT["执行方式"] = "强制替换运行机器C盘的PinKey.txt文件，如果不存在，则创建。" + "\r\n";
                DForThread(OutPutHT);
            }
            finally
            {
                //写日志
                SaveLog("银行接口26700", "流水：" + dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"].ToString() + "，Pin：" + dsreturn.Tables["返回值单条"].Rows[0]["附件信息2"].ToString() + "，Mac：" + dsreturn.Tables["返回值单条"].Rows[0]["附件信息3"].ToString(), "", dtbegin.ToString(), dtend.ToString());
            }
        }


        /// <summary>
        /// 延迟发货、延迟发票扣款的处理
        /// </summary>
        private void JK_Yanchi2013()
        {
            
            DateTime dtend = DateTime.Now;

            //执行业务代码
            WebServicesCenter2013 WSC2013 = new WebServicesCenter2013();

            DateTime dtbegin = DateTime.Now;
            DataSet DSre = WSC2013.RunAtServices("serverRunFHYC", null);
            dtend = DateTime.Now;
            SaveLog("执行发货延迟监控", DSre.Tables["返回值单条"].Rows[0]["执行结果"].ToString() + DSre.Tables["返回值单条"].Rows[0]["提示文本"].ToString(), "", dtbegin.ToString(), dtend.ToString());

            dtbegin = DateTime.Now;
            DataSet DSre1 = WSC2013.RunAtServices("serverRunFPYC", null);
            dtend = DateTime.Now;
            SaveLog("执行发票延迟监控", DSre1.Tables["返回值单条"].Rows[0]["执行结果"].ToString() + DSre1.Tables["返回值单条"].Rows[0]["提示文本"].ToString(), "", dtbegin.ToString(), dtend.ToString());

            dtbegin = DateTime.Now;
            DataSet DSre12 = WSC2013.RunAtServices("serverRunFHYC_onlyJSJY", null);
            dtend = DateTime.Now;
            SaveLog("执行发货延迟监控(仅用于即时交易)", DSre12.Tables["返回值单条"].Rows[0]["执行结果"].ToString() + DSre12.Tables["返回值单条"].Rows[0]["提示文本"].ToString(), "", dtbegin.ToString(), dtend.ToString());


            dtbegin = DateTime.Now;
            DataSet DSre2 = WSC2013.RunAtServices("serverRunLYBZJbuzu", null);
            dtend = DateTime.Now;
            SaveLog("计算履约保证金不足的合同", DSre2.Tables["返回值单条"].Rows[0]["执行结果"].ToString() + DSre2.Tables["返回值单条"].Rows[0]["提示文本"].ToString(), "", dtbegin.ToString(), dtend.ToString());

        }
        /// <summary>
        /// 计算经纪人收益扣税
        /// </summary>
        private void JK_Koushui2013()
        {
            DateTime dtbegin = DateTime.Now;
            DateTime dtend = DateTime.Now;

            //开始业务代码
            //执行业务代码
            WebServicesCenter2013 WSC2013 = new WebServicesCenter2013();
            DataSet DSre = WSC2013.RunAtServices("ServerMYJJRKS", null);
            dtend = DateTime.Now;
            //保存监控执行日志
            dtend = DateTime.Now;
            SaveLog("经纪人收益扣税监控", DSre.Tables["返回值单条"].Rows[0]["执行结果"].ToString() + ";提示文本：" + DSre.Tables["返回值单条"].Rows[0]["提示文本"].ToString(), "", dtbegin.ToString(), dtend.ToString());
        }
        /// <summary>
        /// 合同期满处理
        /// </summary>
        private void JK_Hetong2013()
        {
            DateTime dtbegin = DateTime.Now;
            DateTime dtend = DateTime.Now;

            //开始业务代码
            WebServicesCenter2013 WSC2013 = new WebServicesCenter2013();
            DataSet DSre = WSC2013.RunAtServices("ServerHTQMJK", null);
            //保存监控执行日志
            dtend = DateTime.Now;
            SaveLog("合同期满处理监控", DSre.Tables["返回值单条"].Rows[0]["执行结果"].ToString()+";提示文本：" + DSre.Tables["返回值单条"].Rows[0]["提示文本"].ToString(), "", dtbegin.ToLocalTime().ToString(), dtend.ToLocalTime().ToString());
        }
        /// <summary>
        /// 提醒后自动签收
        /// </summary>
        private void JK_Qianshou2013()
        {
            DateTime dtbegin = DateTime.Now;
            DateTime dtend = DateTime.Now;
            
            //执行业务代码
            WebServicesCenter2013 WSC2013 = new WebServicesCenter2013();
            DataSet DSre = WSC2013.RunAtServices("serverRunZDQS", null);

            //保存监控执行日志
            dtend = DateTime.Now;
            SaveLog("自动签收监控", DSre.Tables["返回值单条"].Rows[0]["执行结果"].ToString() + DSre.Tables["返回值单条"].Rows[0]["提示文本"].ToString(), "", dtbegin.ToString(), dtend.ToString());
        }

        /// <summary>
        /// 每天冷晨运行的监控，真正的业务运行代码
        /// </summary>
        private void Run()
        {
            DateTime dtbegin = DateTime.Now;
            DateTime dtend = DateTime.Now;
            //预订单过期
            JK_YDDGQ();
            //新版监控
            //超期未缴纳履约保证金监控
            JK_Chaoqifeibiao2013();
            //延迟发货、延迟发票扣款的处理
            JK_Yanchi2013();
            //计算经纪人收益扣税
            JK_Koushui2013();
            //合同期满处理
            JK_Hetong2013();
            //提醒后自动签收
            JK_Qianshou2013();
            //获取Pin、Mac
            //Get26700();//迁移到银行专线服务器前应该注释
            /*
            //WebServicesCenter WSC = new WebServicesCenter();
            //(旧版)==========================获取处于冷静期内的商品列表(中标监控)==========================
            JK_ZhongBiao();
            //(旧版)==========================超过期限未缴纳履约保证金的，进行废标==========================
            //先检查有几个，然后执行
            JK_Chaoqifeibiao();
            //(旧版)==========================每月固定日期，计算上月的经纪人收益==========================
            JK_JingJiRenShouYi();
            //==========================一个事情==========================
            */
            //回调
            if (DForThread != null)
            {
                dtend = DateTime.Now;

                //填充传入参数哈希表
                Hashtable OutPutHT = new Hashtable();
                OutPutHT["执行任务名称"] = "监控执行完成";
                OutPutHT["开始时间"] = dtbegin;
                OutPutHT["结束时间"] = dtend;
                OutPutHT["执行结果"] = "监控执行完成";
                OutPutHT["其他描述"] = "";
                OutPutHT["执行方式"] = InPutHT["执行方式"];
                DForThread(OutPutHT);
            }

        }







        /// <summary>
        /// 把要执行的语句集合，转化成数据集，便于服务器执行
        /// </summary>
        /// <param name="alsql"></param>
        /// <returns></returns>
        private DataSet ArrayListToDataSet(ArrayList alsql)
        {
            if(alsql == null || alsql.Count < 1)
            {
                return null;
            }
            DataSet DS = new DataSet();
            DataTable DTsql = new DataTable();
            DTsql.Columns.Add("SQLSTR");
            for (int p = 0; p < alsql.Count; p++)
            {
                DTsql.Rows.Add(new string[] { alsql[p].ToString() });
            }
            DS.Tables.Add(DTsql);
            return DS;
                
        }

        /// <summary>
        /// 回调和保存日志
        /// </summary>
        /// <param name="name">执行任务名称</param>
        /// <param name="restr">执行结果</param>
        /// <param name="otherstr">其他描述</param>
        /// <param name="begintime">开始时间</param>
        /// <param name="endtime">结束时间</param>
        private void SaveLog(string name,string restr,string otherstr,string begintime,string endtime)
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
            StringOP.WriteLog("执行任务名称:" + name + ",开始时间:" + begintime + ",结束时间:" + endtime + ",执行结果:" + restr + ",其他描述:" + otherstr + "");


        }










        /// <summary>
        /// 银行监控
        /// </summary>
        private DataSet BANK_CompareInfo(DataTable dt)
        {
            DateTime dtbegin = DateTime.Now;
            WebServicesCenter2013 wsc2013 = new WebServicesCenter2013();
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            object[] cs = { ds };
            DataSet DSre = wsc2013.RunAtServices("CompareUInfo", cs);
            DateTime dtend = DateTime.Now;
            SaveLog("中标监控_即时交易", DSre.Tables["返回值单条"].Rows[0]["执行结果"].ToString() + DSre.Tables["返回值单条"].Rows[0]["提示文本"].ToString(), "", dtbegin.ToString(), dtend.ToString());
            return DSre;
        }








        /*

        /// <summary>
        /// (旧版)中标监控,获取处于冷静期内的商品列表(中标监控)
        /// </summary>
        private void JK_ZhongBiao()
        {
            DateTime dtbegin = DateTime.Now;
            DateTime dtend = DateTime.Now;

            WebServicesCenter WSC = new WebServicesCenter();
            //==========================获取处于冷静期内的商品列表(中标监控)==========================
            //是否忽略冷静期
            string sqlstrl = " and datediff(day, JRLJQSJ,GETDATE())>=(select top 1 LJQTS from ZZ_PTDTCSSDB  order by CreateTime DESC) ";
            DataSet dstest = WSC.RunSQL("select distinct '商品编号'=ZZ_TBXXBZB.SPBH,'合同期限'=ZZ_TBXXBZB.HTQX  from ZZ_TBXXB join ZZ_TBXXBZB on ZZ_TBXXB.Number = ZZ_TBXXBZB.parentNumber where ZZ_TBXXBZB.SFJRLJQ='是' " + sqlstrl + " order  by ZZ_TBXXBZB.SPBH desc");
            if (dstest == null || dstest.Tables[0].Rows.Count < 1)
            {
                dtend = DateTime.Now;
                SaveLog("中标监控", "成功", "没有要处理的数据", dtbegin.ToString(), dtend.ToString());

            }
            else
            {

                string re = WSC.serverZBJK(dstest.Tables[0]);
                dtend = DateTime.Now;
                SaveLog("中标监控", re, "", dtbegin.ToString(), dtend.ToString());
            }
        }


        /// <summary>
        /// (旧版)超过期限未缴纳履约保证金的，进行废标
        /// </summary>
        private void JK_Chaoqifeibiao()
        {
            DateTime dtbegin = DateTime.Now;
            DateTime dtend = DateTime.Now;

            WebServicesCenter WSC = new WebServicesCenter();


            //要废标的每一条数据，用于向买家赔付和返还买家订金
            DataSet dsfb = WSC.RunSQL("select *,'当前可用投标保证金金额'=isnull((select isnull(PPP.TBBZJJE,0) from  ZZ_TBXXB as PPP  where PPP.Number = B.SELTBXXZBH),0)  from ZZ_ZBDBXXB as B where B.ZBDBZT='中标' and datediff(day, B.ZBSJ,GETDATE())>=(select top 1 CQFBTS from ZZ_PTDTCSSDB order by CreateTime DESC) order by  B.BUYJSBH DESC");
            //要废标的投标信息，用于扣减卖家投标保证金
            DataSet dsfbgroup = WSC.RunSQL("select * from ( select '卖家登陆邮箱'=B.SELDLYX,'卖家角色编号'=B.SELJSBH,'投标信息主表编号'=B.SELTBXXZBH,'投标信息子表编号'=B.MJTBXXChildBH,'商品编号'=B.SPBH,'供货周期'=B.HTQX, '中标集合预订量'=sum(B.ZBSL),'中标价格'=B.SELTBJG,'中标金额'=sum(B.ZBZJE),'中标买家数量'=count(B.BUYJSBH),'投标时间'=B.SELCYJBSJ,'中标时间'=B.ZBSJ,'当前可用投标保证金金额'=isnull((select isnull(PPP.TBBZJJE,0) from  ZZ_TBXXB as PPP  where PPP.Number = B.SELTBXXZBH),0) from ZZ_ZBDBXXB as B where B.ZBDBZT='中标' and datediff(day, B.ZBSJ,GETDATE())>=(select top 1 CQFBTS from ZZ_PTDTCSSDB  order by CreateTime DESC)  group by B.SELTBXXZBH,B.MJTBXXChildBH,B.SPBH,B.SPMC,B.GGXH,B.JJDW,B.HTQX,B.SELTBJG,B.ZBSJ,B.SELCYJBSJ,B.SELJSBH,B.SELDLYX ) as tab order by 中标时间 DESC ");
            int fbsl = 0;
            if (dsfb != null && dsfb.Tables[0].Rows.Count > 0)
            {
                fbsl = Convert.ToInt32(dsfb.Tables[0].Rows.Count);

                ArrayList alsql = new ArrayList();
                string fbtime = DateTime.Now.ToString();
                alsql.Add("update ZZ_ZBDBXXB set ZBDBZT='未定标废标',FBSJ='" + fbtime + "' where ZBDBZT='中标' and datediff(day, ZBSJ,GETDATE())>=(select top 1 CQFBTS from ZZ_PTDTCSSDB  order by CreateTime DESC )");

                //处理资金
                dsfbgroup.DataSetName = "dsfbgroup";
                dsfbgroup.Tables[0].TableName = "fbgroup";
                dsfb.Tables.Add(dsfbgroup.Tables[0].Copy());

                //添加语句
                DataSet dsUpdate = new DataSet();
                dsUpdate = ArrayListToDataSet(alsql);
                dsUpdate.Tables[0].TableName = "update";
                dsfb.Tables.Add(dsUpdate.Tables[0].Copy());

                string re = WSC.GetFeiBiaoSQL(dsfb);
                if (re == "ok")
                {

                    dtend = DateTime.Now;
                    SaveLog("废标监控", "成功", "执行完成，共" + fbsl + "条废标被处理", dtbegin.ToString(), dtend.ToString());

                }
                else
                {
                    dtend = DateTime.Now;
                    SaveLog("废标监控", "失败", "执行错误" + re, dtbegin.ToString(), dtend.ToString());

                }
            }
            else
            {
                dtend = DateTime.Now;
                SaveLog("废标监控", "成功", "没有要处理的数据！", dtbegin.ToString(), dtend.ToString());
            }

        }

        /// <summary>
        /// (旧版)每月固定日期，计算上月的经纪人收益
        /// </summary>
        private void JK_JingJiRenShouYi()
        {
            DateTime dtbegin = DateTime.Now;
            DateTime dtend = DateTime.Now;

            int runday = 5;
            if (dtbegin.Day != runday)
            {
                dtend = DateTime.Now;
                SaveLog("收益监控", "成功", "无需执行，只有每月" + runday + "号才计算上月收益！", dtbegin.ToString(), dtend.ToString());
                return;
            }



            WebServicesCenter WSC = new WebServicesCenter();

            DateTime dtSYtime = DateTime.Now.AddMonths(-1);//收益时间，当前时间上推一个月
            string nian = dtSYtime.Year.ToString();
            string yue = dtSYtime.Month.ToString().PadLeft(2, '0');

            //获取本次月份的基础数据，准备写入
            string sqlSY = "select '经纪人邮箱'=B.DLYX, '经纪人用户名'=B.YHM, '经纪人角色编号'=B.JSBH, '经纪人注册类型'=B.ZCLX, '收益年份'='" + nian + "','收益月份'='" + yue + "', '买家产生收益'=isnull((select ROUND(convert(float,sum(T.THZJE))*convert(float,0.01)*convert(float,0.35),2) from ZZ_THDXXB as T where T.GLDLRJSBH=B.JSBH and T.THDZT='已签收' and  convert(varchar(7), T.BUYQSSJ , 120) = '" + nian + "-" + yue + "' ),0.00), '卖家产生收益'=isnull((select ROUND(convert(float,sum(T.THZJE))*convert(float,0.01)*convert(float,0.27),2) from ZZ_THDXXB as T where T.SELDLRJSBH=B.JSBH and T.THDZT='已签收' and  convert(varchar(7), T.BUYQSSJ , 120) = '" + nian + "-" + yue + "'),0.00), '违规扣减金额'=0.00, '因买家扣减金额'=0.00, '因卖家扣减金额'=0.00, '受理状态'='待受理' from ZZ_DLRJBZLXXB as B order by B.Number ";
            DataSet dsShouYi = WSC.RunSQL(sqlSY);
            DataSet dsShouYi_have = WSC.RunSQL("select top 1 * from ZZ_DLRSYMXB where SYNF='" + nian + "' and SYYF='" + yue + "'");

            if (dsShouYi != null && dsShouYi.Tables[0].Rows.Count > 0 && dsShouYi_have.Tables[0].Rows.Count < 1)
            {
                //生成插入语句
                ArrayList alsqlSY = new ArrayList();
                string fbtime = DateTime.Now.ToString();
                for (int y = 0; y < dsShouYi.Tables[0].Rows.Count; y++)
                {

                    string SYNF = dsShouYi.Tables[0].Rows[y]["收益年份"].ToString();
                    string SYYF = dsShouYi.Tables[0].Rows[y]["收益月份"].ToString();

                    //利用年月生成单号

                    string Number = SYNF + SYYF + y.ToString().PadLeft(10, '0');
                    string DLRDLYX = dsShouYi.Tables[0].Rows[y]["经纪人邮箱"].ToString();
                    string DLRYHM = dsShouYi.Tables[0].Rows[y]["经纪人用户名"].ToString();
                    string DLRJSBH = dsShouYi.Tables[0].Rows[y]["经纪人角色编号"].ToString();
                    string DLRZCLX = dsShouYi.Tables[0].Rows[y]["经纪人注册类型"].ToString();

                    string BUYCSSY = dsShouYi.Tables[0].Rows[y]["买家产生收益"].ToString();
                    string SELCSSY = dsShouYi.Tables[0].Rows[y]["卖家产生收益"].ToString();
                    string BYSYZJE = (Convert.ToDouble(BUYCSSY) + Convert.ToDouble(SELCSSY)).ToString();

                    string WGKJJE = "0.00";
                    string YBUYKJJE = "0.00";
                    string YSELKJJE = "0.00";
                    string YJSE = "0.00";
                    string YDSYJE = "0.00";
                    string SFYJSFP = "否";
                    string PTSFYDK = "否";
                    string SYCLZT = dsShouYi.Tables[0].Rows[y]["受理状态"].ToString();
                    string CreateTime = fbtime;
                    alsqlSY.Add("INSERT INTO ZZ_DLRSYMXB ([Number],[DLRDLYX],[DLRYHM],[DLRJSBH],[DLRZCLX],[SYNF],[SYYF],[BYSYZJE],[BUYCSSY],[SELCSSY],[WGKJJE],[YBUYKJJE],[YSELKJJE],[YJSE],[YDSYJE],[SFYJSFP],[PTSFYDK],[SYCLZT],[CreateTime]) VALUES ('" + Number + "','" + DLRDLYX + "','" + DLRYHM + "','" + DLRJSBH + "','" + DLRZCLX + "','" + SYNF + "','" + SYYF + "','" + BYSYZJE + "','" + BUYCSSY + "','" + SELCSSY + "','" + WGKJJE + "','" + YBUYKJJE + "','" + YSELKJJE + "','" + YJSE + "','" + YDSYJE + "','" + SFYJSFP + "','" + PTSFYDK + "','" + SYCLZT + "','" + CreateTime + "')");
                }

                bool fb = WSC.RunSQLTran(ArrayListToDataSet(alsqlSY));
                if (fb)
                {
                    dtend = DateTime.Now;
                    SaveLog("收益监控", "成功", "执行完成，共" + alsqlSY.Count + "条收益产生！", dtbegin.ToString(), dtend.ToString());

                }
                else
                {
                    dtend = DateTime.Now;
                    SaveLog("收益监控", "失败", "执行错误", dtbegin.ToString(), dtend.ToString());

                }

            }
        }


        */
    }
}
