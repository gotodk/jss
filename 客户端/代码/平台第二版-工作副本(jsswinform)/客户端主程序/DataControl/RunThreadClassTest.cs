using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Threading;
using System.Data;


namespace 客户端主程序.DataControl
{

    /// <summary>
    /// 测试首页数据表加载线程类
    /// </summary>
    class RunThreadClassTest
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
        public RunThreadClassTest(Hashtable PHT, delegateForThread DFT)
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
            DForThread = DFT;
            InPutHT = PHT;
        }

       static string bdstr = "";

        /// <summary>
        /// 开始绑定
        /// </summary>
        /// <param name="dstest"></param>
        private void BeginBangDing(DataSet dstest)
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
            int jiange = 1;


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
                for (int i = 0; i < hangshu - 1; i++)
                {
                    Thread.Sleep(jiange);
                    OutPutHT = new Hashtable();
                    OutPutHT["可以绑定"] = CanBD;
                    OutPutHT["执行标记"] = "改变列";
                    OutPutHT["i"] = i;
                    OutPutHT["值"] = dstest.Tables[0].Rows[i];
                    DForThread(OutPutHT);

                }
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
            if (cha < 0)//差额负值说明新数据行少
            {
                int oldrowindex = hangshu - 1;
                for (int i = 0; i < dstest.Tables[0].Rows.Count - 1; i++)
                {
                    Thread.Sleep(jiange);
                    OutPutHT = new Hashtable();
                    OutPutHT["可以绑定"] = CanBD;
                    OutPutHT["执行标记"] = "改变列";
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
        /// 开始执行线程，加载默认商品(主区域)
        /// </summary>
        public void BeginRun()
        {
            
            while (true)
            {
                
                Thread.Sleep(500);
                //用于测试
                DataControl.WebServicesCenter WSC = new DataControl.WebServicesCenter();
                DataSet dstest = WSC.test_ds("默认");

                //开始绑定
                BeginBangDing(dstest);

                Thread.Sleep(10000);
            }
        }


        /// <summary>
        /// 开始执行线程,用于自选商品(主区域)
        /// </summary>
        public void BeginRunZXSP()
        {

            while (true)
            {
                string DLYX = PublicDS.PublisDsUser.Tables[0].Rows[0]["DLYX"].ToString().Trim();

                Thread.Sleep(500);
                //用于测试
                DataControl.WebServicesCenter WSC = new DataControl.WebServicesCenter();
                DataSet dstest = WSC.test_ds("自选**!!" + DLYX);

                //开始绑定
                BeginBangDing(dstest);

                Thread.Sleep(20000);
            }
        }



        /// <summary>
        /// 开始执行线程,用于显示二级分类下商品(主区域)
        /// </summary>
        public void BeginRunErJi()
        {
            while (true)
            {
                Thread.Sleep(500);
                //用于测试
                DataControl.WebServicesCenter WSC = new DataControl.WebServicesCenter();
                DataSet dstest = WSC.test_ds("二级分类**!!" + InPutHT["分类编号"].ToString());

                //开始绑定
                BeginBangDing(dstest);

                Thread.Sleep(20000);
            }
       
        }



        /// <summary>
        /// 开始执行线程,用于显示指定商品
        /// </summary>
        public void BeginRunZhiDing()
        {

            Thread.Sleep(500);
            //用于测试
            DataControl.WebServicesCenter WSC = new DataControl.WebServicesCenter();
            DataSet dstest = WSC.test_ds("指定商品**!!" + InPutHT["商品编号"].ToString() + "*" + InPutHT["合同期限"].ToString());


            //填充传入参数哈希表
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["测试列表数据表"] = dstest;//测试参数
            DForThread(OutPutHT);


        }


        /// <summary>
        /// 加入或删除自选商品
        /// </summary>
        public void BeginRunZXSPedit()
        {

                string DLYX = PublicDS.PublisDsUser.Tables[0].Rows[0]["DLYX"].ToString().Trim();

                Thread.Sleep(500);
                //用于测试
                DataControl.WebServicesCenter WSC = new DataControl.WebServicesCenter();
                string rere = WSC.ZXSPedit(DLYX, InPutHT["商品编号"].ToString(), InPutHT["操作"].ToString());


                //填充传入参数哈希表
                Hashtable OutPutHT = new Hashtable();
                OutPutHT["执行结果"] = rere;//测试参数
                DForThread(OutPutHT);

        }



    }
}
