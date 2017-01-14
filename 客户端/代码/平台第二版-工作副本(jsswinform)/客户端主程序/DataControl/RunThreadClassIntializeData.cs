using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Threading;
using System.Data;
using 客户端主程序.Support;

namespace 客户端主程序.DataControl
{
    class RunThreadClassIntializeData
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
        public RunThreadClassIntializeData(Hashtable PHT, delegateForThread DFT)
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
            DForThread = DFT;
            InPutHT = PHT;
        }


        /// <summary>
        /// 开始执行获取买家、卖家基础数据的线程
        /// </summary>
        public void BeginRun_IntializeBuyerSellerData()
        {
            Thread.Sleep(500);

            //尽量将全套数据处理放到服务器端进行，不要再这里处理业务逻辑。个别情况例外。
            //执行处理前，如果遇到了处理一些东西可能导致网页超时的情况，可能还要直接在这里调用webservices进行运算
            //这种情况可以直接将一些运算逻辑放到这里，而不需要再开新线程。 用于减少服务器端的压力。
            //但由于这里没有事务，所以这里只能进行数据读取和分析操作，绝不能更新数据库

            //用于测试，真正开始更新数据库
            DataControl.WebServicesCenter WSC = new DataControl.WebServicesCenter();
            DataSet dsSellerBuyerData = WSC.ReturnMMJBascData(InPutHT["角色类型"].ToString(), InPutHT["角色编号"].ToString(), InPutHT["经纪人角色编号"].ToString());
            //填充传入参数哈希表
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["前置数据"] = dsSellerBuyerData;//测试参数          
            DForThread(OutPutHT);


        }

        /// <summary>
        /// 开始执行线程  审核买家数据,买家数据审核通过
        /// </summary>
        public void VerifyBuyerDataPass()
        {
            Thread.Sleep(500);


            //尽量将全套数据处理放到服务器端进行，不要再这里处理业务逻辑。个别情况例外。
            //执行处理前，如果遇到了处理一些东西可能导致网页超时的情况，可能还要直接在这里调用webservices进行运算
            //这种情况可以直接将一些运算逻辑放到这里，而不需要再开新线程。 用于减少服务器端的压力。
            //但由于这里没有事务，所以这里只能进行数据读取和分析操作，绝不能更新数据库

            //更新数据库
            DataControl.WebServicesCenter WSC = new DataControl.WebServicesCenter();
            //string returnstr = WSC.test_save_or_update(InPutHT["str1"].ToString());
            //哈希表转换为DictionaryEntry数组
            DataTable dataTable=StringOP.GetDataTableFormHashtable(InPutHT);
              string[] strInfoData= WSC.VerifyBuyerDataPass(dataTable);
         
            //填充传入参数哈希表
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["执行状态"] = strInfoData[0];//测试参数
            OutPutHT["执行结果"] = strInfoData[1];//测试参数
            DForThread(OutPutHT);
        }

        /// <summary>
        /// 开始执行线程  审核买家数据
        /// </summary>
        public void VerifyBuyerDataReject()
        {
            Thread.Sleep(500); 


            //尽量将全套数据处理放到服务器端进行，不要再这里处理业务逻辑。个别情况例外。
            //执行处理前，如果遇到了处理一些东西可能导致网页超时的情况，可能还要直接在这里调用webservices进行运算
            //这种情况可以直接将一些运算逻辑放到这里，而不需要再开新线程。 用于减少服务器端的压力。
            //但由于这里没有事务，所以这里只能进行数据读取和分析操作，绝不能更新数据库

            //更新数据库
            DataControl.WebServicesCenter WSC = new DataControl.WebServicesCenter();
            //string returnstr = WSC.test_save_or_update(InPutHT["str1"].ToString());
            //哈希表转换为DictionaryEntry数组
            DataTable dataTable = StringOP.GetDataTableFormHashtable(InPutHT);
            string[] strInfoData = WSC.VerifyBuyerDataReject(dataTable);

            //填充传入参数哈希表
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["执行状态"] = strInfoData[0];//测试参数
            OutPutHT["执行结果"] = strInfoData[1];//测试参数
            DForThread(OutPutHT);
        
        
        }

        /// <summary>
        /// 开始执行线程  审核卖家数据，审核通过
        /// </summary>
        public void VerifySellerDataPass()
        {
            Thread.Sleep(500);


            //尽量将全套数据处理放到服务器端进行，不要再这里处理业务逻辑。个别情况例外。
            //执行处理前，如果遇到了处理一些东西可能导致网页超时的情况，可能还要直接在这里调用webservices进行运算
            //这种情况可以直接将一些运算逻辑放到这里，而不需要再开新线程。 用于减少服务器端的压力。
            //但由于这里没有事务，所以这里只能进行数据读取和分析操作，绝不能更新数据库

            //更新数据库
            DataControl.WebServicesCenter WSC = new DataControl.WebServicesCenter();
            //string returnstr = WSC.test_save_or_update(InPutHT["str1"].ToString());
            //哈希表转换为DictionaryEntry数组
            DataTable dataTable = StringOP.GetDataTableFormHashtable(InPutHT);
            string[] strInfoData = WSC.VerifySellerDataPass(dataTable);

            //填充传入参数哈希表
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["执行状态"] = strInfoData[0];//测试参数
            OutPutHT["执行结果"] = strInfoData[1];//测试参数
            DForThread(OutPutHT);
        
        }

        /// <summary>
        /// 开始执行线程  审核卖家数据，驳回
        /// </summary>
        public void VerifySellerDataReject()
        {
            Thread.Sleep(500);


            //尽量将全套数据处理放到服务器端进行，不要再这里处理业务逻辑。个别情况例外。
            //执行处理前，如果遇到了处理一些东西可能导致网页超时的情况，可能还要直接在这里调用webservices进行运算
            //这种情况可以直接将一些运算逻辑放到这里，而不需要再开新线程。 用于减少服务器端的压力。
            //但由于这里没有事务，所以这里只能进行数据读取和分析操作，绝不能更新数据库

            //更新数据库
            DataControl.WebServicesCenter WSC = new DataControl.WebServicesCenter();
            //string returnstr = WSC.test_save_or_update(InPutHT["str1"].ToString());
            //哈希表转换为DictionaryEntry数组
            DataTable dataTable = StringOP.GetDataTableFormHashtable(InPutHT);
            string[] strInfoData = WSC.VerifySellerDataReject(dataTable);

            //填充传入参数哈希表
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["执行状态"] = strInfoData[0];//测试参数
            OutPutHT["执行结果"] = strInfoData[1];//测试参数
            DForThread(OutPutHT);
        
        
        
        }








    }
}
