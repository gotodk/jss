using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Threading;
using System.Data;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace 客户端主程序.DataControl
{
    /// <summary>
    /// 分页处理线程类
    /// </summary>
    class RunThreadClassPager
    {


                //向线程传递的回调参数
        private delegateForThread DForThread_pager;//用于控制分页控件
        private delegateForThread DForThread_form;//用于控制调用窗体
        //向线程传递的数据参数
        private Hashtable InPutHT;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="PHT">需要传入线程的参数</param>
        /// <param name="DFT">线程委托</param>
        public RunThreadClassPager(Hashtable PHT, delegateForThread DFT_pager, delegateForThread DFT_form)
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
            DForThread_pager = DFT_pager;
            DForThread_form = DFT_form;
            InPutHT = PHT;
        }

        /// <summary>
        /// 开始执行线程
        /// </summary>
        public void BeginRun()
        {
            Thread.Sleep(500);
            WebServicesCenter WSC = new WebServicesCenter();
            Hashtable returnHT = new Hashtable();
            returnHT["数据"] = WSC.GetPagerDB(InPutHT);
            //对控件和窗体分别回调
            DForThread_form(returnHT);
            DForThread_pager(returnHT);
        }


    }
}
