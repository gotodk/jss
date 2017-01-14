using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FMPublicClass;
using ServiceStack.Redis;
 
using System.IO;
using FMipcClass;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using FMDBHelperClass;
using System.Threading;
using System.Text.RegularExpressions;


namespace RedisFormTest
{
    public partial class Form1 : Form
    {

      
        public Form1()
        {
            InitializeComponent();

          
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Regex r_g3 = new Regex("\\(" + "(.*?)" + " 最后补位 ", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase);
            string[] temp3 = Regex.Replace(Regex.Replace("    " + r_g3.Match("    " + KEY.Text + " 最后补位 ").Groups[1].Value, " into ", "", RegexOptions.IgnoreCase).Replace(" ", ""), "\\)values\\(", "★", RegexOptions.IgnoreCase).Split('★');
            string[] zd_name = temp3[0].Replace("(", "").Replace(")", "").Split(',');
            string[] zd_value = temp3[1].Replace("(", "").Replace(")", "").Split(',');
            richTextBox1.AppendText("");
            //一次添加多个集合
            //RedisClient RC = FMDBHelperClass.RedisClass.GetRedisClient(null);
            //RC.AddRangeToSet("S:CUN:yhb888@gmail.com", new List<string> { "商品买卖概况", "资金余额变动明细", "竞标中" });

            ////删除一个元素并确认是否存在
            //long delcount = RC.SRem("S:CUN:yhb888@gmail.com", Encoding.UTF8.GetBytes("商品买卖概况"));

            ////写入散列
            //DataSet ds = new DataSet("xxx");
            //byte[] Hvalue =  StringOP.DataToByte(ds);
            //long f = RC.HSet("H:CUC:yhb888@gmail.com", Encoding.UTF8.GetBytes("商品买卖概况"), Hvalue);

            ////读取散列
            //byte[] Hvalue2 = RC.HGet("H:CUC:yhb888@gmail.com", Encoding.UTF8.GetBytes("商品买卖概况"));
            //DataSet ds2 = (Hvalue2 == null) ? null : StringOP.ByteToDataset(Hvalue2);

            ////显示
            //richTextBox1.AppendText(ds2.DataSetName + Environment.NewLine);
          
        }

        private void button2_Click(object sender, EventArgs e)
        {
          
        }

        private void button3_Click(object sender, EventArgs e)
        {
   
        }

     
        private void gogo()
        {
            RedisClient newRC = RedisClass.GetRedisClient(null);
 
            lock (RedisClass.LockObj)
            {
                byte[] Buffer = newRC.Get("cc");
            }
       
          

        }
   
        private void button4_Click(object sender, EventArgs e)
        {
            DateTime dt1 = DateTime.Now;
      
            Task[] tt = new Task[1000];
            for(int i = 0;i < 1000;i++)
            {
                tt[i] = Task.Factory.StartNew(() => gogo());
            }
 
            Task.WaitAll(tt);


            DateTime dt2 = DateTime.Now;
            TimeSpan ts = dt1 - dt2;
            //richTextBox1.AppendText(  ts.TotalMilliseconds.ToString("#.####") + "___"+  Encoding.UTF8.GetString(Buffer) + Environment.NewLine);
            richTextBox1.AppendText(ts.TotalMilliseconds.ToString("#.####")     + Environment.NewLine);
        }

        private void button5_Click(object sender, EventArgs e)
        {

          
            

        }
    }

   
}
