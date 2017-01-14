using FMipcClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace 客户端
{
    class Class1
    {
        public void runn()
        {
            while (DateTime.Now.Minute < 10)
            {
                //Thread.Sleep(1);
            }
   
                FMWScenter wsd = new FMWScenter("http://192.168.16.8:8001/s1.asmx?wsdl");
                string suc = (string)wsd.ExecuteQuery("test", null);
   
        }
    }
}
