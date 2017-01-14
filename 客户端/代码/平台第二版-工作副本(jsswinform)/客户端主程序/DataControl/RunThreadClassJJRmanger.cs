using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Threading;
using System.Data;

namespace 客户端主程序.DataControl
{
    /// <summary>
    /// 经纪人相关处理
    /// </summary>
    class RunThreadClassJJRmanger
    {
        private delegateForThread threading;
        private Hashtable inPutHT;
        public RunThreadClassJJRmanger(Hashtable ht, delegateForThread dft)
        {
            threading = dft;
            inPutHT = ht;
        }

        //连接远程进行操作
        public void runThreading()
        {
            Thread.Sleep(500);
            DataControl.WebServicesCenter wsc = new WebServicesCenter();
            string[] re = wsc.JJRset(inPutHT["经纪人角色编号"].ToString(), inPutHT["经纪人邮箱"].ToString(), inPutHT["操作类型"].ToString(), inPutHT["被操作人邮箱"].ToString());
            Hashtable outPutHT = new Hashtable();
            outPutHT["执行结果"] = re;
            threading(outPutHT);

        }

    }
}
