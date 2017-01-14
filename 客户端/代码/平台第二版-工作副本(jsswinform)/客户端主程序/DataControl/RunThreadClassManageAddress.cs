using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Threading;
using System.Data;
using 客户端主程序.Support;

namespace 客户端主程序.DataControl
{
    class RunThreadClassManageAddress
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
        public RunThreadClassManageAddress(Hashtable PHT, delegateForThread DFT)
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
            DForThread = DFT;
            InPutHT = PHT;
        }


        /// <summary>
        /// 开始执行设为默认地址的线程
        /// </summary>
        public void BeginRun_SetMrAddress()
        {
            Thread.Sleep(500);

            //尽量将全套数据处理放到服务器端进行，不要再这里处理业务逻辑。个别情况例外。
            //执行处理前，如果遇到了处理一些东西可能导致网页超时的情况，可能还要直接在这里调用webservices进行运算
            //这种情况可以直接将一些运算逻辑放到这里，而不需要再开新线程。 用于减少服务器端的压力。
            //但由于这里没有事务，所以这里只能进行数据读取和分析操作，绝不能更新数据库

            //用于测试，真正开始更新数据库
            DataControl.WebServicesCenter WSC = new DataControl.WebServicesCenter();
            string returnstr = WSC.Set_MrAddress(InPutHT["number"].ToString(), InPutHT["dlyx"].ToString());


            //填充传入参数哈希表
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["执行状态"] = returnstr;//测试参数          
            DForThread(OutPutHT);


        }

        /// <summary>
        /// 开始执行删除收货地址的线程
        /// </summary>
        public void BeginRun_DelAddress()
        {
            Thread.Sleep(500);

            //尽量将全套数据处理放到服务器端进行，不要再这里处理业务逻辑。个别情况例外。
            //执行处理前，如果遇到了处理一些东西可能导致网页超时的情况，可能还要直接在这里调用webservices进行运算
            //这种情况可以直接将一些运算逻辑放到这里，而不需要再开新线程。 用于减少服务器端的压力。
            //但由于这里没有事务，所以这里只能进行数据读取和分析操作，绝不能更新数据库

            //用于测试，真正开始更新数据库
            DataControl.WebServicesCenter WSC = new DataControl.WebServicesCenter();
            string returnstr = WSC.Del_Address(InPutHT["number"].ToString(), InPutHT["dlyx"].ToString());


            //填充传入参数哈希表
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["执行状态"] = returnstr;//测试参数          
            DForThread(OutPutHT);

        }

        /// <summary>
        /// 开始执行保存收货地址的线程
        /// </summary>
        public void BeginRun_SaveAddress()
        {
            Thread.Sleep(500);

            //尽量将全套数据处理放到服务器端进行，不要再这里处理业务逻辑。个别情况例外。
            //执行处理前，如果遇到了处理一些东西可能导致网页超时的情况，可能还要直接在这里调用webservices进行运算
            //这种情况可以直接将一些运算逻辑放到这里，而不需要再开新线程。 用于减少服务器端的压力。
            //但由于这里没有事务，所以这里只能进行数据读取和分析操作，绝不能更新数据库


            //将哈希表转换成字符串数组，用于参数传递          
            string[][] InputStr = new string[InPutHT.Count][];
            int i = 0;
            foreach (DictionaryEntry item in InPutHT)
            {
                InputStr[i] = new string[2] { item.Key.ToString(), item.Value.ToString() };              
                i++;
            }
            
            //真正开始更新数据库
            DataControl.WebServicesCenter WSC = new DataControl.WebServicesCenter();
            string[] result = WSC.Save_Address(InputStr);


            //填充返回值哈希表
            Hashtable OutPutHT = new Hashtable();
            if (result!=null&&result.Length > 0)
            {
                OutPutHT["执行状态"] = result[0].ToString();
                OutPutHT["执行结果"] = result[1].ToString();
            }
            DForThread(OutPutHT);

        }

        /// <summary>
        /// 开始执行获取待修改的收货地址信息的线程
        /// </summary>
        public void BeginRun_GetEditAddress()
        {
            Thread.Sleep(500);
            //尽量将全套数据处理放到服务器端进行，不要再这里处理业务逻辑。个别情况例外。
            //执行处理前，如果遇到了处理一些东西可能导致网页超时的情况，可能还要直接在这里调用webservices进行运算
            //这种情况可以直接将一些运算逻辑放到这里，而不需要再开新线程。 用于减少服务器端的压力。
            //但由于这里没有事务，所以这里只能进行数据读取和分析操作，绝不能更新数据库

            //用于测试，真正开始更新数据库
            DataControl.WebServicesCenter WSC = new DataControl.WebServicesCenter();
            DataSet resultDs = WSC.Get_EditAddress(InPutHT["number"].ToString(), InPutHT["dlyx"].ToString());

            //填充传入参数哈希表
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["执行状态"] = resultDs;//测试参数            
            DForThread(OutPutHT);

        }
    }
}


