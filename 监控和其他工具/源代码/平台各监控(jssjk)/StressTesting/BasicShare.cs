using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace StressTesting
{
    public class BasicShare
    {
        /// <summary>
        /// 报警
        /// </summary>
        /// <param name="sMesss"></param>
        public static void ShowMessage(string sMesss)
        {
            MessageBox.Show(sMesss, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        /// <summary>
        /// 重启
        /// </summary>
        /// <param name="f"></param>
        public static void AutoRun()
        {
            System.Diagnostics.Process.Start(System.Reflection.Assembly.GetExecutingAssembly().Location);
            Application.Exit();
            // f.Close();

        }
        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="fPath">文件路径</param>
        /// <param name="fFileName">文件名，不包含扩展名</param>
        /// <param name="mess">日志内容</param>
        public static void WriteLog(string fPath, string fFileName, string mess)
        {
            try
            {
                StreamWriter sw = null;
                if (File.Exists(fPath + fFileName + ".txt"))   //存在文件，就直接追加写入
                {
                    sw = File.AppendText(fPath + fFileName + ".txt");

                }
                else
                {
                    sw = new StreamWriter(fPath + fFileName + ".txt");

                }

                sw.Write(mess + "\r\n");              
                sw.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 读取日志
        /// </summary>
        /// <param name="fPath"></param>
        /// <param name="fFileName"></param>
        /// <returns></returns>
        public static string ReadLog(string fPath, string fFileName)
        {
            string txt = "";
            if (File.Exists(fPath + fFileName + ".txt"))
            {
                FileStream fs = new FileStream(fPath + fFileName + ".txt", FileMode.Open, FileAccess.Read);
                StreamReader rw = new StreamReader(fs, Encoding.Default);
                txt = rw.ReadToEnd();
                rw.Close();
               
 
            }
            return txt;
           
        }

        /// <summary>
        /// 从文件夹中读取多个日志文件
        /// </summary>
        /// <param name="fPath"></param>
        /// <param name="fFileName"></param>
        /// <returns></returns>
        public static string ReadLogs(DirectoryInfo dir,ref string result)
        {

            //if (!Directory.Exists(fPath)) 
            //    return null; 
            //string[] fis = Directory.GetFiles(fPath, "*.txt");
            
            //string result = string.Empty;
            //foreach (string s in fis)
            //{
            //    FileInfo fi = new FileInfo(s);
            //    StreamReader sr = new StreamReader(s, Encoding.Default);
            //    string text = sr.ReadToEnd()+"\r\n";
            //    result += text;
            //}
            //return result;
         
            //获取子文件
            FileInfo[] files = dir.GetFiles();
            //获取子文件夹
            DirectoryInfo[] cdirs = dir.GetDirectories();            
            foreach (FileInfo info in files)
            {
                if (info.Extension == ".log")
                {
                    StreamReader sr = new StreamReader(info.FullName, Encoding.UTF8);
                    string text = sr.ReadToEnd() + "\r\n";
                    result += text;
                    sr.Close();
                }
               
            }

            foreach (var item in cdirs)
            {

                ReadLogs(item,ref result);
            }

            return result;
           
            }
     
    }
}
