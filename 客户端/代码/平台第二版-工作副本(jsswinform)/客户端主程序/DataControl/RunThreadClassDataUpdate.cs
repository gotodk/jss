using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Threading;
using System.Data;
using System.Xml.Serialization;
using System.IO;
namespace 客户端主程序.DataControl
{
    class RunThreadClassDataUpdate
    {
         //向线程传递的回调参数
        private delegateForThread DForThread;
        //向线程传递的数据参数
        private Hashtable InPutHT;
        //向线程传递的webservice方法名
        private string MethodName;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="PHT">需要传入线程的参数</param>
        /// <param name="DFT">线程委托</param>
        public RunThreadClassDataUpdate(Hashtable PHT, delegateForThread DFT,string methodName)
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
            DForThread = DFT;
            InPutHT = PHT;
            MethodName = methodName;
        }

        /// <summary>
        /// 开始执行线程
        /// </summary>
        public void BeginRun()
        {
            Thread.Sleep(500);
            //尽量将全套数据处理放到服务器端进行，不要再这里处理业务逻辑。个别情况例外。
            //执行处理前，如果遇到了处理一些东西可能导致网页超时的情况，可能还要直接在这里调用webservices进行运算
            //这种情况可以直接将一些运算逻辑放到这里，而不需要再开新线程。 用于减少服务器端的压力。
            //但由于这里没有事务，所以这里只能进行数据读取和分析操作，绝不能更新数据库

            //更新数据库
            DataControl.WebServicesCenter WSC = new DataControl.WebServicesCenter();          
            
            //哈希表XML序列化，以便于webservice传递  
            DictionaryEntry[] array = new DictionaryEntry[InPutHT.Count];
            InPutHT.CopyTo(array, 0);
            String xmltext = Serializer<DictionaryEntry[]>(array);

            //调用服务
            string returnstr = WSC.DataUpdate(xmltext,MethodName) ;          

            //填充传入参数哈希表
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["执行状态"] = returnstr;//测试参数
            OutPutHT["服务方法"] = MethodName;//测试参数
            OutPutHT["执行结果"] = "您的余额不足！";//测试参数
            DForThread(OutPutHT);

        }
        /// <summary>
        /// 序列化数据类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objToXml"></param>
        /// <returns></returns>
        public String Serializer<T>(T objToXml)
        {
            StringWriter writer = new StringWriter();
            XmlSerializer serializer = new XmlSerializer(objToXml.GetType());
            serializer.Serialize(writer, objToXml);
            return writer.GetStringBuilder().ToString();
        }

        
    }
}
