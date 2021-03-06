﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Threading;
using System.Data;
using 客户端主程序.Support;

namespace 客户端主程序.DataControl
{

    /// <summary>
    /// 测试提交表单处理线程类
    /// </summary>
    class RunThreadClassTHD
    {
        //向线程传递的回调参数
        private delegateForThread DForThread;
        //向线程传递的数据参数
        string Number;
        string jsbh;
        DataSet dsinfo;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dsCS">需要传入线程的参数</param>
        /// <param name="DFT">线程委托</param>
        public RunThreadClassTHD(string NumberT, string jsbhT, DataSet dsinfoT, delegateForThread DFT)
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
            DForThread = DFT;
            Number = NumberT;
            jsbh = jsbhT;
            dsinfo = dsinfoT;
        }

        /// <summary>
        /// 开始执行线程，获取基础数据
        /// </summary>
        public void BeginRun_info()
        {
            Thread.Sleep(500);


            //尽量将全套数据处理放到服务器端进行，不要再这里处理业务逻辑。个别情况例外。
            //执行处理前，如果遇到了处理一些东西可能导致网页超时的情况，可能还要直接在这里调用webservices进行运算
            //这种情况可以直接将一些运算逻辑放到这里，而不需要再开新线程。 用于减少服务器端的压力。
            //但由于这里没有事务，所以这里只能进行数据读取和分析操作，绝不能更新数据库

            //用于测试，真正开始更新数据库
            DataControl.WebServicesCenter WSC = new DataControl.WebServicesCenter();
            DataSet reds = WSC.GetTHinfo(Number, jsbh);


            //填充传入参数哈希表
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["执行结果"] = reds;//测试参数
            DForThread(OutPutHT);


        }

        /// <summary>
        /// 开始执行线程，保存提货单
        /// </summary>
        public void BeginRun_save()
        {
            Thread.Sleep(500);


            //尽量将全套数据处理放到服务器端进行，不要再这里处理业务逻辑。个别情况例外。
            //执行处理前，如果遇到了处理一些东西可能导致网页超时的情况，可能还要直接在这里调用webservices进行运算
            //这种情况可以直接将一些运算逻辑放到这里，而不需要再开新线程。 用于减少服务器端的压力。
            //但由于这里没有事务，所以这里只能进行数据读取和分析操作，绝不能更新数据库

            //用于测试，真正开始更新数据库
            DataControl.WebServicesCenter WSC = new DataControl.WebServicesCenter();
            string returnstr = WSC.AddTHD(dsinfo);


            //填充传入参数哈希表
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["执行结果"] = returnstr;//测试参数
            DForThread(OutPutHT);


        }


        /// <summary>
        /// 开始执行线程，更新发货信息
        /// </summary>
        public void BeginRun_FaHuo()
        {
            Thread.Sleep(500);


            //尽量将全套数据处理放到服务器端进行，不要再这里处理业务逻辑。个别情况例外。
            //执行处理前，如果遇到了处理一些东西可能导致网页超时的情况，可能还要直接在这里调用webservices进行运算
            //这种情况可以直接将一些运算逻辑放到这里，而不需要再开新线程。 用于减少服务器端的压力。
            //但由于这里没有事务，所以这里只能进行数据读取和分析操作，绝不能更新数据库

            //用于测试，真正开始更新数据库
            DataControl.WebServicesCenter WSC = new DataControl.WebServicesCenter();
            string returnstr = WSC.UpdateFaHuo(dsinfo);


            //填充传入参数哈希表
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["执行结果"] = returnstr;//测试参数
            DForThread(OutPutHT);


        }




        /// <summary>
        /// 开始执行线程，更新发货信息
        /// </summary>
        public void BeginRun_QianShou()
        {
            Thread.Sleep(500);


            //尽量将全套数据处理放到服务器端进行，不要再这里处理业务逻辑。个别情况例外。
            //执行处理前，如果遇到了处理一些东西可能导致网页超时的情况，可能还要直接在这里调用webservices进行运算
            //这种情况可以直接将一些运算逻辑放到这里，而不需要再开新线程。 用于减少服务器端的压力。
            //但由于这里没有事务，所以这里只能进行数据读取和分析操作，绝不能更新数据库

            //用于测试，真正开始更新数据库
            DataControl.WebServicesCenter WSC = new DataControl.WebServicesCenter();
            string returnstr = WSC.UpdateQianShou(Number,jsbh);


            //填充传入参数哈希表
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["执行结果"] = returnstr;//测试参数
            DForThread(OutPutHT);


        }



    }
}
