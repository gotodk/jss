using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
namespace 银行监控浦发后台
{
    public static class CallPinMac
    {
        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="word">待加密串</param>
        /// <returns>结果</returns>
        public static string EncryptString(string word)
        {
            //DataTable dt = DbHelperSQL.Query("SELECT [ID],[ExSerial],[PinKey],[MacKey] FROM [AAA_PinMac]").Tables[0];
            string serverPath = "C:\\PinKey.txt";
            string[] StrArr = null;
            using (StreamReader sr = new StreamReader(serverPath))
            {
                string StrConfig = sr.ReadToEnd();
                StrArr = StrConfig.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            }
            string Key = "";
            if (StrArr.Length > 2)
            {
                Key = PinMac.InitPassword("handsome", StrArr[1]);
            }
            return PinMac.NewPassword(Key, word);

        }
        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="word">带解密串</param>
        /// <returns>结果</returns>
        public static string DecryptString(string word)
        {
            string serverPath = "C:\\PinKey.txt";
            //string serverPath = System.AppDomain.CurrentDomain.BaseDirectory + "\\PinKey.txt";
            string[] StrArr = null;
            using (StreamReader sr = new StreamReader(serverPath))
            {
                string StrConfig = sr.ReadToEnd();
                StrArr = StrConfig.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            }

            //DataTable dt = DbHelperSQL.Query("SELECT [ID],[ExSerial],[PinKey],[MacKey] FROM [AAA_PinMac]").Tables[0];
            string key = PinMac.InitPassword("handsome", StrArr[1]);

            return PinMac.InitPassword(key, word);

        }

        public static byte[] GetMac(string word)
        {
            //DataTable dt = DbHelperSQL.Query("SELECT [ID],[ExSerial],[PinKey],[MacKey] FROM [AAA_PinMac]").Tables[0];
            //string serverPath = System.AppDomain.CurrentDomain.BaseDirectory + "\\PinKey.txt";
            string serverPath = "C:\\PinKey.txt";
            string[] StrArr = null;
            using (StreamReader sr = new StreamReader(serverPath))
            {
                string StrConfig = sr.ReadToEnd();
                StrArr = StrConfig.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            }
            string key = PinMac.InitPassword("handsome", StrArr[2]);

            return PinMac.NewMac(word, key);
        }
    }
}
