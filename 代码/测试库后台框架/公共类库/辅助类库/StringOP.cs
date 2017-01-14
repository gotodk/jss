using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Data;
using System.Collections;
using System.Xml.Serialization;
using System.Security.Cryptography;
using System.Xml;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace FMPublicClass
{
    /// <summary>
    /// 字符串相关操作验证支持类
    /// </summary>
    public static class StringOP
    {
        //英文字符
        private static char[] constant =   
      {   
        '0','1','2','3','4','5','6','7','8','9',   
        'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z',   
        'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z'   
      };

        /// <summary>
        /// 产生随即英文字符
        /// </summary>
        /// <param name="Length"></param>
        /// <returns></returns>
        public static string GenerateRandom(int Length)
        {
            System.Text.StringBuilder newRandom = new System.Text.StringBuilder(62);
            Random rd = new Random();
            for (int i = 0; i < Length; i++)
            {
                newRandom.Append(constant[rd.Next(62)]);
            }
            return newRandom.ToString();
        }

        /// <summary>
        /// 验证是否正整数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNumeric(string str)
        {
            System.Text.RegularExpressions.Regex reg1 = new System.Text.RegularExpressions.Regex(@"^[0-9]\d*$");
            return reg1.IsMatch(str);

        }

        /// <summary>   
        /// 判断一个字符串是否为合法数字(指定整数位数和小数位数)   
        /// </summary>   
        /// <param name="s">字符串</param>   
        /// <param name="precision">整数位数</param>   
        /// <param name="scale">小数位数</param>   
        /// <returns></returns>   
        public static bool IsNumberXS(string s, int precision, int scale)
        {
            if ((precision == 0) && (scale == 0))
            {
                return false;
            }
            string pattern = @"(^\d{1," + precision + "}";
            if (scale > 0)
            {
                pattern += @"\.\d{0," + scale + "}$)|" + pattern;
            }
            pattern += "$)";
            return Regex.IsMatch(s, pattern);
        }



        /// <summary>
        /// 验证邮箱字符串
        /// </summary>
        /// <param name="Email"></param>
        /// <returns></returns>
        public static bool GetEmailData(string Email)
        {
            Int32 position = Email.IndexOf("@") + 1;
            Int32 point = Email.IndexOf(".") + 1;
            Int32 length = Email.Length - 1;
            do
            {
                if ((position > 1) && (point > position + 1)
                    && !Email[length].Equals('.')
                    && !Email[length].Equals("@") && !Email[length].Equals("-")
                    && !Email[length].Equals("_") && !Email[length].Equals("+")
                    && !Email[length].Equals("=") && !Email[length].Equals("!")
                    && !Email[length].Equals("~") && !Email[length].Equals("#")
                    && !Email[length].Equals("%") && !Email[length].Equals("$")
                    && !Email[length].Equals("*") && !Email[length].Equals("^")
                    && !Email[length].Equals("(") && !Email[length].Equals(")")
                    && !Email[length].Equals("&") && !Email[length].Equals("<")
                    && !Email[length].Equals(">") && !Email[length].Equals("?")
                    && !Email[length].Equals(",") && !Email[length].Equals(":")
                    && !Email[length].Equals("/") && !Email[length].Equals(";"))
                //判断邮箱的第一位不能为"@ "符号  " ."符号必须在 @ 符号后字符串之后且不能以其他基本符号结束
                //其中判断语句可以简化但为了方便理清思路就这样写，实际运用中并不这样写
                {
                    return true;
                }
                else
                {
                    return false;
                }
            } while (true);
        }


        /// <summary>
        /// url里有key的值，就替换为value,没有的话就追加.
        /// </summary>
        /// <param name="url">原始URL</param>
        /// <param name="ParamText">参数名称</param>
        /// <param name="ParamValue">要替换或增加的参数值</param>
        /// <returns></returns>
        public static string ReBuildUrl(string url, string ParamText, string ParamValue)
        {
            Regex reg = new Regex(string.Format("{0}=[^&]*", ParamText), RegexOptions.IgnoreCase);
            Regex reg1 = new Regex("[&]{2,}", RegexOptions.IgnoreCase);
            string _url = reg.Replace(url, "");
            //_url = reg1.Replace(_url, "");
            if (_url.IndexOf("?") == -1)
                _url += string.Format("?{0}={1}", ParamText, ParamValue);//?
            else
                _url += string.Format("&{0}={1}", ParamText, ParamValue);//&
            _url = reg1.Replace(_url, "&");
            _url = _url.Replace("?&", "?");
            return _url;
        }



        /// <summary>
        /// 对象序列化为byte[]
        /// </summary>
        /// <param name="o">对象object</param>
        /// <returns>byte[]</returns>
        public static byte[] serializeTobyte(object o)
        {
            MemoryStream memStream = new MemoryStream();
            XmlSerializer serializer = new XmlSerializer(o.GetType());
            serializer.Serialize(memStream, o);
            byte[] cs_byte = memStream.ToArray();
            memStream.Close();
            memStream.Dispose();
            return cs_byte;
        }

        /// <summary>
        /// 对象序列化为byte[]
        /// </summary>
        /// <param name="o">对象object[]</param>
        /// <returns>byte[]</returns>
        public static byte[] serializeTobyte(object[] o)
        {
            MemoryStream memStream = new MemoryStream();
            XmlSerializer serializer = new XmlSerializer(o.GetType());
            serializer.Serialize(memStream, o);
            byte[] cs_byte = memStream.ToArray();
            memStream.Close();
            memStream.Dispose();
            return cs_byte;
        }

        /// <summary>
        /// 多个对象，合并后序列化为byte[]
        /// </summary>
        /// <param name="o">对象object[]</param>
        /// <returns></returns>
        public static byte[] serializeTobyteDG(object[] o)
        {
            object[] temp = new object[o.Length];
            for (int i = 0; i < temp.Length; i++)
            {
                temp[i] = serializeTobyte(o[i]);
            }
            return serializeTobyte(temp);
        }

        /// <summary>
        /// java专用md5
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetMD5forjava(string str)
        {
            //将序列化后的参数进行md5加密,不需要复杂加密，因为这是防止修改已抓到的包。
            string cs_md5_string = "";
            MD5CryptoServiceProvider MD5CSP = new MD5CryptoServiceProvider();
            // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
            byte[] cs_md5_byte = MD5CSP.ComputeHash(System.Text.Encoding.UTF8.GetBytes(str));


            return Convert.ToBase64String(cs_md5_byte); ;
        }

        /// <summary>
        /// 根据参数，获的md5值
        /// </summary>
        /// <param name="ccccc"></param>
        /// <returns></returns>
        public static string GetMD5(object[] ccccc)
        {
            //序列化参数
            byte[] cs_byte = serializeTobyteDG(ccccc);


            //将序列化后的参数进行md5加密,不需要复杂加密，因为这是防止修改已抓到的包。
            string cs_md5_string = "";
            MD5 md5 = MD5.Create("System.Security.Cryptography.MD5");//实例化一个md5对像
            // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
            byte[] cs_md5_byte = md5.ComputeHash(cs_byte);
            for (int i = 0; i < cs_md5_byte.Length; i++)
            {
                // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符 
                cs_md5_string = cs_md5_string + cs_md5_byte[i].ToString("X");

            }

            return cs_md5_string;
        }

        #region 配合JS用的C#版DES加解密方法及相关函数

        /// <summary>
        /// 加密测试函数
        /// </summary>
        /// <param name="beinetstr">待加密的字符串</param>
        /// <param name="beinetkey">密钥</param>
        /// <returns></returns>
        public static string encMe(string beinetstr, string beinetkey)
        {
            if (string.IsNullOrEmpty(beinetkey))
                return string.Empty;

            return stringToHex(des(beinetkey, beinetstr, true, false, string.Empty));
        }

        /// <summary>
        /// 解密测试函数
        /// </summary>
        /// <param name="beinetstr">待解密的字符串</param>
        /// <param name="beinetkey">密钥</param>
        /// <returns></returns>
        public static string uncMe(string beinetstr, string beinetkey)
        {
            if (string.IsNullOrEmpty(beinetkey))
                return null;
            string ret = des(beinetkey, HexTostring(beinetstr), false, false, string.Empty);
            return ret;
        }

        /// <summary>
        /// 把字符串转换为16进制字符串
        /// 如：a变成61（即10进制的97）；abc变成616263
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string stringToHex(string s)
        {
            string r = "";
            string[] hexes = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f" };
            for (int i = 0; i < (s.Length); i++)
            {
                r += hexes[RM(s[i], 4)] + hexes[s[i] & 0xf];
            }
            return r;
        }

        /// <summary>
        /// 16进制字符串转换为字符串
        /// 如：61（即10进制的97）变成a；616263变成abc
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string HexTostring(string s)
        {
            string ret = string.Empty;

            for (int i = 0; i < s.Length; i += 2)
            {
                int sxx = Convert.ToInt32(s.Substring(i, 2), 16);
                ret += (char)sxx;
            }
            return ret;
        }

        /// <summary>
        /// 带符号位右移（类似于js的>>>）
        /// </summary>
        /// <param name="a">用于右移的操作数</param>
        /// <param name="bit">右移位数</param>
        /// <returns></returns>
        public static int RM(int a, int bit)
        {
            unchecked
            {
                uint b = (uint)a;
                b = b >> bit;
                return (int)b;
            }
        }

        /// <summary>
        /// 加解密主调方法
        /// </summary>
        /// <param name="beinetkey">密钥</param>
        /// <param name="message">加密时为string，解密时为byte[]</param>
        /// <param name="encrypt">true：加密；false：解密</param>
        /// <param name="mode">true：CBC mode；false：非CBC mode</param>
        /// <param name="iv">初始化向量</param>
        /// <returns></returns>
        public static string des(string beinetkey, string message, bool encrypt, bool mode, string iv)
        {
            //declaring this locally speeds things up a bit
            long[] spfunction1 = { 0x1010400, 0, 0x10000, 0x1010404, 0x1010004, 0x10404, 0x4, 0x10000, 0x400, 0x1010400, 0x1010404, 0x400, 0x1000404, 0x1010004, 0x1000000, 0x4, 0x404, 0x1000400, 0x1000400, 0x10400, 0x10400, 0x1010000, 0x1010000, 0x1000404, 0x10004, 0x1000004, 0x1000004, 0x10004, 0, 0x404, 0x10404, 0x1000000, 0x10000, 0x1010404, 0x4, 0x1010000, 0x1010400, 0x1000000, 0x1000000, 0x400, 0x1010004, 0x10000, 0x10400, 0x1000004, 0x400, 0x4, 0x1000404, 0x10404, 0x1010404, 0x10004, 0x1010000, 0x1000404, 0x1000004, 0x404, 0x10404, 0x1010400, 0x404, 0x1000400, 0x1000400, 0, 0x10004, 0x10400, 0, 0x1010004 };
            long[] spfunction2 = { -0x7fef7fe0, -0x7fff8000, 0x8000, 0x108020, 0x100000, 0x20, -0x7fefffe0, -0x7fff7fe0, -0x7fffffe0, -0x7fef7fe0, -0x7fef8000, -0x80000000, -0x7fff8000, 0x100000, 0x20, -0x7fefffe0, 0x108000, 0x100020, -0x7fff7fe0, 0, -0x80000000, 0x8000, 0x108020, -0x7ff00000, 0x100020, -0x7fffffe0, 0, 0x108000, 0x8020, -0x7fef8000, -0x7ff00000, 0x8020, 0, 0x108020, -0x7fefffe0, 0x100000, -0x7fff7fe0, -0x7ff00000, -0x7fef8000, 0x8000, -0x7ff00000, -0x7fff8000, 0x20, -0x7fef7fe0, 0x108020, 0x20, 0x8000, -0x80000000, 0x8020, -0x7fef8000, 0x100000, -0x7fffffe0, 0x100020, -0x7fff7fe0, -0x7fffffe0, 0x100020, 0x108000, 0, -0x7fff8000, 0x8020, -0x80000000, -0x7fefffe0, -0x7fef7fe0, 0x108000 };
            long[] spfunction3 = { 0x208, 0x8020200, 0, 0x8020008, 0x8000200, 0, 0x20208, 0x8000200, 0x20008, 0x8000008, 0x8000008, 0x20000, 0x8020208, 0x20008, 0x8020000, 0x208, 0x8000000, 0x8, 0x8020200, 0x200, 0x20200, 0x8020000, 0x8020008, 0x20208, 0x8000208, 0x20200, 0x20000, 0x8000208, 0x8, 0x8020208, 0x200, 0x8000000, 0x8020200, 0x8000000, 0x20008, 0x208, 0x20000, 0x8020200, 0x8000200, 0, 0x200, 0x20008, 0x8020208, 0x8000200, 0x8000008, 0x200, 0, 0x8020008, 0x8000208, 0x20000, 0x8000000, 0x8020208, 0x8, 0x20208, 0x20200, 0x8000008, 0x8020000, 0x8000208, 0x208, 0x8020000, 0x20208, 0x8, 0x8020008, 0x20200 };
            long[] spfunction4 = { 0x802001, 0x2081, 0x2081, 0x80, 0x802080, 0x800081, 0x800001, 0x2001, 0, 0x802000, 0x802000, 0x802081, 0x81, 0, 0x800080, 0x800001, 0x1, 0x2000, 0x800000, 0x802001, 0x80, 0x800000, 0x2001, 0x2080, 0x800081, 0x1, 0x2080, 0x800080, 0x2000, 0x802080, 0x802081, 0x81, 0x800080, 0x800001, 0x802000, 0x802081, 0x81, 0, 0, 0x802000, 0x2080, 0x800080, 0x800081, 0x1, 0x802001, 0x2081, 0x2081, 0x80, 0x802081, 0x81, 0x1, 0x2000, 0x800001, 0x2001, 0x802080, 0x800081, 0x2001, 0x2080, 0x800000, 0x802001, 0x80, 0x800000, 0x2000, 0x802080 };
            long[] spfunction5 = { 0x100, 0x2080100, 0x2080000, 0x42000100, 0x80000, 0x100, 0x40000000, 0x2080000, 0x40080100, 0x80000, 0x2000100, 0x40080100, 0x42000100, 0x42080000, 0x80100, 0x40000000, 0x2000000, 0x40080000, 0x40080000, 0, 0x40000100, 0x42080100, 0x42080100, 0x2000100, 0x42080000, 0x40000100, 0, 0x42000000, 0x2080100, 0x2000000, 0x42000000, 0x80100, 0x80000, 0x42000100, 0x100, 0x2000000, 0x40000000, 0x2080000, 0x42000100, 0x40080100, 0x2000100, 0x40000000, 0x42080000, 0x2080100, 0x40080100, 0x100, 0x2000000, 0x42080000, 0x42080100, 0x80100, 0x42000000, 0x42080100, 0x2080000, 0, 0x40080000, 0x42000000, 0x80100, 0x2000100, 0x40000100, 0x80000, 0, 0x40080000, 0x2080100, 0x40000100 };
            long[] spfunction6 = { 0x20000010, 0x20400000, 0x4000, 0x20404010, 0x20400000, 0x10, 0x20404010, 0x400000, 0x20004000, 0x404010, 0x400000, 0x20000010, 0x400010, 0x20004000, 0x20000000, 0x4010, 0, 0x400010, 0x20004010, 0x4000, 0x404000, 0x20004010, 0x10, 0x20400010, 0x20400010, 0, 0x404010, 0x20404000, 0x4010, 0x404000, 0x20404000, 0x20000000, 0x20004000, 0x10, 0x20400010, 0x404000, 0x20404010, 0x400000, 0x4010, 0x20000010, 0x400000, 0x20004000, 0x20000000, 0x4010, 0x20000010, 0x20404010, 0x404000, 0x20400000, 0x404010, 0x20404000, 0, 0x20400010, 0x10, 0x4000, 0x20400000, 0x404010, 0x4000, 0x400010, 0x20004010, 0, 0x20404000, 0x20000000, 0x400010, 0x20004010 };
            long[] spfunction7 = { 0x200000, 0x4200002, 0x4000802, 0, 0x800, 0x4000802, 0x200802, 0x4200800, 0x4200802, 0x200000, 0, 0x4000002, 0x2, 0x4000000, 0x4200002, 0x802, 0x4000800, 0x200802, 0x200002, 0x4000800, 0x4000002, 0x4200000, 0x4200800, 0x200002, 0x4200000, 0x800, 0x802, 0x4200802, 0x200800, 0x2, 0x4000000, 0x200800, 0x4000000, 0x200800, 0x200000, 0x4000802, 0x4000802, 0x4200002, 0x4200002, 0x2, 0x200002, 0x4000000, 0x4000800, 0x200000, 0x4200800, 0x802, 0x200802, 0x4200800, 0x802, 0x4000002, 0x4200802, 0x4200000, 0x200800, 0, 0x2, 0x4200802, 0, 0x200802, 0x4200000, 0x800, 0x4000002, 0x4000800, 0x800, 0x200002 };
            long[] spfunction8 = { 0x10001040, 0x1000, 0x40000, 0x10041040, 0x10000000, 0x10001040, 0x40, 0x10000000, 0x40040, 0x10040000, 0x10041040, 0x41000, 0x10041000, 0x41040, 0x1000, 0x40, 0x10040000, 0x10000040, 0x10001000, 0x1040, 0x41000, 0x40040, 0x10040040, 0x10041000, 0x1040, 0, 0, 0x10040040, 0x10000040, 0x10001000, 0x41040, 0x40000, 0x41040, 0x40000, 0x10041000, 0x1000, 0x40, 0x10040040, 0x1000, 0x41040, 0x10001000, 0x40, 0x10000040, 0x10040000, 0x10040040, 0x10000000, 0x40000, 0x10001040, 0, 0x10041040, 0x40040, 0x10000040, 0x10040000, 0x10001000, 0x10001040, 0, 0x10041040, 0x41000, 0x41000, 0x1040, 0x1040, 0x40040, 0x10000000, 0x10041000 };


            //create the 16 or 48 subkeys we will need
            int[] keys = des_createKeys(beinetkey);
            int m = 0;
            int i, j;
            int temp, right1, right2, left, right;
            int[] looping;
            int cbcleft = 0, cbcleft2 = 0, cbcright = 0, cbcright2 = 0;
            int endloop;
            int loopinc;
            int len = message.Length;
            int chunk = 0;
            //set up the loops for single and triple des
            int iterations = keys.Length == 32 ? 3 : 9;//single or triple des
            if (iterations == 3)
            {
                looping = encrypt ? new int[] { 0, 32, 2 } : new int[] { 30, -2, -2 };
            }
            else { looping = encrypt ? new int[] { 0, 32, 2, 62, 30, -2, 64, 96, 2 } : new int[] { 94, 62, -2, 32, 64, 2, 30, -2, -2 }; }

            if (encrypt)
            {
                message += "\0\0\0\0\0\0\0\0";//pad the message out with null bytes
            }
            //store the result here
            //List<byte> result = new List<byte>();
            //List<byte> tempresult = new List<byte>();
            string result = string.Empty;
            string tempresult = string.Empty;

            if (mode)
            {//CBC mode
                int[] tmp = { 0, 0, 0, 0, 0, 0, 0, 0 };
                int pos = 24;
                int iTmp = 0;
                while (m < iv.Length && iTmp < tmp.Length)
                {
                    if (pos < 0)
                        pos = 24;
                    tmp[iTmp++] = iv[m++] << pos;
                    pos -= 8;
                }
                cbcleft = tmp[0] | tmp[1] | tmp[2] | tmp[3];
                cbcright = tmp[4] | tmp[5] | tmp[6] | tmp[7];

                //cbcleft = (iv[m++] << 24) | (iv[m++] << 16) | (iv[m++] << 8) | iv[m++];
                //cbcright = (iv[m++] << 24) | (iv[m++] << 16) | (iv[m++] << 8) | iv[m++];
                m = 0;
            }

            //loop through each 64 bit chunk of the message
            while (m < len)
            {
                if (encrypt)
                {/*加密时按双字节操作*/
                    left = (message[m++] << 16) | message[m++];
                    right = (message[m++] << 16) | message[m++];
                }
                else
                {
                    left = (message[m++] << 24) | (message[m++] << 16) | (message[m++] << 8) | message[m++];
                    right = (message[m++] << 24) | (message[m++] << 16) | (message[m++] << 8) | message[m++];
                }
                //for Cipher Block Chaining mode,xor the message with the previous result
                if (mode)
                {
                    if (encrypt)
                    {
                        left ^= cbcleft; right ^= cbcright;
                    }
                    else
                    {
                        cbcleft2 = cbcleft; cbcright2 = cbcright; cbcleft = left; cbcright = right;
                    }
                }

                //first each 64 but chunk of the message must be permuted according to IP
                temp = (RM(left, 4) ^ right) & 0x0f0f0f0f; right ^= temp; left ^= (temp << 4);
                temp = (RM(left, 16) ^ right) & 0x0000ffff; right ^= temp; left ^= (temp << 16);
                temp = (RM(right, 2) ^ left) & 0x33333333; left ^= temp; right ^= (temp << 2);
                temp = (RM(right, 8) ^ left) & 0x00ff00ff; left ^= temp; right ^= (temp << 8);
                temp = (RM(left, 1) ^ right) & 0x55555555; right ^= temp; left ^= (temp << 1);

                left = ((left << 1) | RM(left, 31));
                right = ((right << 1) | RM(right, 31));

                //do this either 1 or 3 times for each chunk of the message
                for (j = 0; j < iterations; j += 3)
                {
                    endloop = looping[j + 1];
                    loopinc = looping[j + 2];
                    //now go through and perform the encryption or decryption 
                    for (i = looping[j]; i != endloop; i += loopinc)
                    {//for efficiency
                        right1 = right ^ keys[i];
                        right2 = (RM(right, 4) | (right << 28)) ^ keys[i + 1];
                        //the result is attained by passing these bytes through the S selection functions
                        temp = left;
                        left = right;
                        right = (int)(temp ^ (spfunction2[RM(right1, 24) & 0x3f] | spfunction4[RM(right1, 16) & 0x3f] | spfunction6[RM(right1, 8) & 0x3f] | spfunction8[right1 & 0x3f] | spfunction1[RM(right2, 24) & 0x3f] | spfunction3[RM(right2, 16) & 0x3f] | spfunction5[RM(right2, 8) & 0x3f] | spfunction7[right2 & 0x3f]));
                    }
                    temp = left; left = right; right = temp;//unreverse left and right
                }//for either 1 or 3 iterations

                //move then each one bit to the right
                left = (RM(left, 1) | (left << 31));
                right = (RM(right, 1) | (right << 31));

                //now perform IP-1,which is IP in the opposite direction
                temp = (RM(left, 1) ^ right) & 0x55555555; right ^= temp; left ^= (temp << 1);
                temp = (RM(right, 8) ^ left) & 0x00ff00ff; left ^= temp; right ^= (temp << 8);
                temp = (RM(right, 2) ^ left) & 0x33333333; left ^= temp; right ^= (temp << 2);
                temp = (RM(left, 16) ^ right) & 0x0000ffff; right ^= temp; left ^= (temp << 16);
                temp = (RM(left, 4) ^ right) & 0x0f0f0f0f; right ^= temp; left ^= (temp << 4);

                //for Cipher Block Chaining mode,xor the message with the previous result
                if (mode)
                {
                    if (encrypt)
                    {
                        cbcleft = left; cbcright = right;
                    }
                    else
                    {
                        left ^= cbcleft2; right ^= cbcright2;
                    }
                }
                //int[] arrInt;
                if (encrypt)
                {
                    //arrInt = new int[]{ RM(left, 24), (RM(left, 16) & 0xff), (RM(left, 8) & 0xff), (left & 0xff), RM(right, 24), (RM(right, 16) & 0xff), (RM(right, 8) & 0xff), (right & 0xff) };
                    tempresult += String.Concat((char)RM(left, 24),
                        (char)(RM(left, 16) & 0xff),
                        (char)(RM(left, 8) & 0xff),
                        (char)(left & 0xff),
                        (char)RM(right, 24),
                        (char)(RM(right, 16) & 0xff),
                        (char)(RM(right, 8) & 0xff),
                        (char)(right & 0xff));
                }
                else
                {
                    // 解密时，最后一个字符如果是\0，去除
                    //arrInt = new int[] { (RM(left, 16) & 0xffff), (left & 0xffff), (RM(right, 16) & 0xffff), (right & 0xffff) };
                    int tmpch = (RM(left, 16) & 0xffff);
                    if (tmpch != 0)
                        tempresult += (char)tmpch;
                    tmpch = (left & 0xffff);
                    if (tmpch != 0)
                        tempresult += (char)tmpch;
                    tmpch = (RM(right, 16) & 0xffff);
                    if (tmpch != 0)
                        tempresult += (char)tmpch;
                    tmpch = (right & 0xffff);
                    if (tmpch != 0)
                        tempresult += (char)tmpch;
                    //tempresult += String.Concat((char)(RM(left, 16) & 0xffff),
                    //    (char)(left & 0xffff),
                    //    (char)(RM(right, 16) & 0xffff),
                    //    (char)(right & 0xffff));
                }/*解密时输出双字节*/
                //byte[] arrByte = new byte[arrInt.Length];
                //for (int loop = 0; loop < arrInt.Length; loop++)
                //{
                //    tempresult.Add(byte.Parse(arrInt[loop].ToString()));
                //    //arrByte[loop] = byte.Parse(arrInt[loop].ToString());
                //}
                //tempresult.Add(arrByte;// System.Text.Encoding.Unicode.GetString(arrByte);

                chunk += encrypt ? 16 : 8;
                if (chunk == 512)
                {
                    //result.AddRange(tempresult);tempresult.Clear(); 
                    result += tempresult; tempresult = string.Empty;
                    chunk = 0;
                }
            }//for every 8 characters,or 64 bits in the message

            //return the result as an array

            //result.AddRange(tempresult);
            //return result.ToArray();
            return result + tempresult;
        }//end of des

        /// <summary>
        /// this takes as input a 64 bit beinetkey(even though only 56 bits are used)
        /// as an array of 2 integers,and returns 16 48 bit keys
        /// </summary>
        /// <param name="beinetkey"></param>
        /// <returns></returns>
        static int[] des_createKeys(string beinetkey)
        {
            //declaring this locally speeds things up a bit
            int[] pc2bytes0 = { 0, 0x4, 0x20000000, 0x20000004, 0x10000, 0x10004, 0x20010000, 0x20010004, 0x200, 0x204, 0x20000200, 0x20000204, 0x10200, 0x10204, 0x20010200, 0x20010204 };
            int[] pc2bytes1 = { 0, 0x1, 0x100000, 0x100001, 0x4000000, 0x4000001, 0x4100000, 0x4100001, 0x100, 0x101, 0x100100, 0x100101, 0x4000100, 0x4000101, 0x4100100, 0x4100101 };
            int[] pc2bytes2 = { 0, 0x8, 0x800, 0x808, 0x1000000, 0x1000008, 0x1000800, 0x1000808, 0, 0x8, 0x800, 0x808, 0x1000000, 0x1000008, 0x1000800, 0x1000808 };
            int[] pc2bytes3 = { 0, 0x200000, 0x8000000, 0x8200000, 0x2000, 0x202000, 0x8002000, 0x8202000, 0x20000, 0x220000, 0x8020000, 0x8220000, 0x22000, 0x222000, 0x8022000, 0x8222000 };
            int[] pc2bytes4 = { 0, 0x40000, 0x10, 0x40010, 0, 0x40000, 0x10, 0x40010, 0x1000, 0x41000, 0x1010, 0x41010, 0x1000, 0x41000, 0x1010, 0x41010 };
            int[] pc2bytes5 = { 0, 0x400, 0x20, 0x420, 0, 0x400, 0x20, 0x420, 0x2000000, 0x2000400, 0x2000020, 0x2000420, 0x2000000, 0x2000400, 0x2000020, 0x2000420 };
            int[] pc2bytes6 = { 0, 0x10000000, 0x80000, 0x10080000, 0x2, 0x10000002, 0x80002, 0x10080002, 0, 0x10000000, 0x80000, 0x10080000, 0x2, 0x10000002, 0x80002, 0x10080002 };
            int[] pc2bytes7 = { 0, 0x10000, 0x800, 0x10800, 0x20000000, 0x20010000, 0x20000800, 0x20010800, 0x20000, 0x30000, 0x20800, 0x30800, 0x20020000, 0x20030000, 0x20020800, 0x20030800 };
            int[] pc2bytes8 = { 0, 0x40000, 0, 0x40000, 0x2, 0x40002, 0x2, 0x40002, 0x2000000, 0x2040000, 0x2000000, 0x2040000, 0x2000002, 0x2040002, 0x2000002, 0x2040002 };
            int[] pc2bytes9 = { 0, 0x10000000, 0x8, 0x10000008, 0, 0x10000000, 0x8, 0x10000008, 0x400, 0x10000400, 0x408, 0x10000408, 0x400, 0x10000400, 0x408, 0x10000408 };
            int[] pc2bytes10 = { 0, 0x20, 0, 0x20, 0x100000, 0x100020, 0x100000, 0x100020, 0x2000, 0x2020, 0x2000, 0x2020, 0x102000, 0x102020, 0x102000, 0x102020 };
            int[] pc2bytes11 = { 0, 0x1000000, 0x200, 0x1000200, 0x200000, 0x1200000, 0x200200, 0x1200200, 0x4000000, 0x5000000, 0x4000200, 0x5000200, 0x4200000, 0x5200000, 0x4200200, 0x5200200 };
            int[] pc2bytes12 = { 0, 0x1000, 0x8000000, 0x8001000, 0x80000, 0x81000, 0x8080000, 0x8081000, 0x10, 0x1010, 0x8000010, 0x8001010, 0x80010, 0x81010, 0x8080010, 0x8081010 };
            int[] pc2bytes13 = { 0, 0x4, 0x100, 0x104, 0, 0x4, 0x100, 0x104, 0x1, 0x5, 0x101, 0x105, 0x1, 0x5, 0x101, 0x105 };

            //how many iterations(1 for des,3 for triple des)
            int iterations = beinetkey.Length >= 24 ? 3 : 1;
            //stores the return keys
            int[] keys = new int[32 * iterations];
            //now define the left shifts which need to be done
            int[] shifts = { 0, 0, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 0 };
            //other variables
            int left, right;
            int lefttemp;
            int righttemp;
            int m = 0, n = 0;
            int temp;

            for (int j = 0; j < iterations; j++)
            {//either 1 or 3 iterations
                int[] tmp = { 0, 0, 0, 0, 0, 0, 0, 0 };
                int pos = 24;
                int iTmp = 0;
                while (m < beinetkey.Length && iTmp < tmp.Length)
                {
                    if (pos < 0)
                        pos = 24;
                    tmp[iTmp++] = beinetkey[m++] << pos;
                    pos -= 8;
                }
                left = tmp[0] | tmp[1] | tmp[2] | tmp[3];
                right = tmp[4] | tmp[5] | tmp[6] | tmp[7];

                //left = (beinetkey[m++] << 24) | (beinetkey[m++] << 16) | (beinetkey[m++] << 8) | beinetkey[m++];
                //right = (beinetkey[m++] << 24) | (beinetkey[m++] << 16) | (beinetkey[m++] << 8) | beinetkey[m++];

                temp = (RM(left, 4) ^ right) & 0x0f0f0f0f; right ^= temp; left ^= (temp << 4);
                temp = (RM(right, -16) ^ left) & 0x0000ffff; left ^= temp; right ^= (temp << -16);
                temp = (RM(left, 2) ^ right) & 0x33333333; right ^= temp; left ^= (temp << 2);
                temp = (RM(right, -16) ^ left) & 0x0000ffff; left ^= temp; right ^= (temp << -16);
                temp = (RM(left, 1) ^ right) & 0x55555555; right ^= temp; left ^= (temp << 1);
                temp = (RM(right, 8) ^ left) & 0x00ff00ff; left ^= temp; right ^= (temp << 8);
                temp = (RM(left, 1) ^ right) & 0x55555555; right ^= temp; left ^= (temp << 1);

                //the right side needs to be shifted and to get the last four bits of the left side
                temp = (left << 8) | (RM(right, 20) & 0x000000f0);
                //left needs to be put upside down
                left = (right << 24) | ((right << 8) & 0xff0000) | (RM(right, 8) & 0xff00) | (RM(right, 24) & 0xf0);
                right = temp;

                //now go through and perform these shifts on the left and right keys
                for (int i = 0; i < shifts.Length; i++)
                {
                    //shift the keys either one or two bits to the left
                    if (shifts[i] == 1)
                    {
                        left = (left << 2) | RM(left, 26); right = (right << 2) | RM(right, 26);
                    }
                    else
                    {
                        left = (left << 1) | RM(left, 27); right = (right << 1) | RM(right, 27);
                    }
                    left &= -0xf; right &= -0xf;

                    //now apply PC-2,in such a way that E is easier when encrypting or decrypting
                    //this conversion will look like PC-2 except only the last 6 bits of each byte are used
                    //rather than 48 consecutive bits and the order of lines will be according to 
                    //how the S selection functions will be applied:S2,S4,S6,S8,S1,S3,S5,S7
                    lefttemp = pc2bytes0[RM(left, 28)] | pc2bytes1[RM(left, 24) & 0xf]
                   | pc2bytes2[RM(left, 20) & 0xf] | pc2bytes3[RM(left, 16) & 0xf]
                   | pc2bytes4[RM(left, 12) & 0xf] | pc2bytes5[RM(left, 8) & 0xf]
                   | pc2bytes6[RM(left, 4) & 0xf];
                    righttemp = pc2bytes7[RM(right, 28)] | pc2bytes8[RM(right, 24) & 0xf]
                   | pc2bytes9[RM(right, 20) & 0xf] | pc2bytes10[RM(right, 16) & 0xf]
                   | pc2bytes11[RM(right, 12) & 0xf] | pc2bytes12[RM(right, 8) & 0xf]
                   | pc2bytes13[RM(right, 4) & 0xf];
                    temp = (RM(righttemp, 16) ^ lefttemp) & 0x0000ffff;
                    keys[n++] = lefttemp ^ temp; keys[n++] = righttemp ^ (temp << 16);
                }
            }//for each iterations
            //return the keys we"ve created
            return keys;
        }//end of des_createKeys

        #endregion

        #region 配合C#用的JS版本的加解密方法（需手工复制到客户端，或通过ClientScript添加到客户端）
        /// <summary>
        /// 返回JS版本的Des加解密方法字符串
        /// </summary>
        /// <returns></returns>
        public static string GetJsDesMethod()
        {
            string ret = @"

    /// 加解密主调方法
    /// beinetkey         密钥
    /// message     加密时为待加密的字符串，解密时为待解密的字符串
    /// encrypt     1：加密；0：解密
    /// mode        true：CBC mode；false：非CBC mode
    /// iv          初始化向量
    function des(beinetkey, message, encrypt, mode, iv) {
        //declaring this locally speeds things up a bit
        var spfunction1 = new Array(0x1010400, 0, 0x10000, 0x1010404, 0x1010004, 0x10404, 0x4, 0x10000, 0x400, 0x1010400, 0x1010404, 0x400, 0x1000404, 0x1010004, 0x1000000, 0x4, 0x404, 0x1000400, 0x1000400, 0x10400, 0x10400, 0x1010000, 0x1010000, 0x1000404, 0x10004, 0x1000004, 0x1000004, 0x10004, 0, 0x404, 0x10404, 0x1000000, 0x10000, 0x1010404, 0x4, 0x1010000, 0x1010400, 0x1000000, 0x1000000, 0x400, 0x1010004, 0x10000, 0x10400, 0x1000004, 0x400, 0x4, 0x1000404, 0x10404, 0x1010404, 0x10004, 0x1010000, 0x1000404, 0x1000004, 0x404, 0x10404, 0x1010400, 0x404, 0x1000400, 0x1000400, 0, 0x10004, 0x10400, 0, 0x1010004);
        var spfunction2 = new Array(-0x7fef7fe0, -0x7fff8000, 0x8000, 0x108020, 0x100000, 0x20, -0x7fefffe0, -0x7fff7fe0, -0x7fffffe0, -0x7fef7fe0, -0x7fef8000, -0x80000000, -0x7fff8000, 0x100000, 0x20, -0x7fefffe0, 0x108000, 0x100020, -0x7fff7fe0, 0, -0x80000000, 0x8000, 0x108020, -0x7ff00000, 0x100020, -0x7fffffe0, 0, 0x108000, 0x8020, -0x7fef8000, -0x7ff00000, 0x8020, 0, 0x108020, -0x7fefffe0, 0x100000, -0x7fff7fe0, -0x7ff00000, -0x7fef8000, 0x8000, -0x7ff00000, -0x7fff8000, 0x20, -0x7fef7fe0, 0x108020, 0x20, 0x8000, -0x80000000, 0x8020, -0x7fef8000, 0x100000, -0x7fffffe0, 0x100020, -0x7fff7fe0, -0x7fffffe0, 0x100020, 0x108000, 0, -0x7fff8000, 0x8020, -0x80000000, -0x7fefffe0, -0x7fef7fe0, 0x108000);
        var spfunction3 = new Array(0x208, 0x8020200, 0, 0x8020008, 0x8000200, 0, 0x20208, 0x8000200, 0x20008, 0x8000008, 0x8000008, 0x20000, 0x8020208, 0x20008, 0x8020000, 0x208, 0x8000000, 0x8, 0x8020200, 0x200, 0x20200, 0x8020000, 0x8020008, 0x20208, 0x8000208, 0x20200, 0x20000, 0x8000208, 0x8, 0x8020208, 0x200, 0x8000000, 0x8020200, 0x8000000, 0x20008, 0x208, 0x20000, 0x8020200, 0x8000200, 0, 0x200, 0x20008, 0x8020208, 0x8000200, 0x8000008, 0x200, 0, 0x8020008, 0x8000208, 0x20000, 0x8000000, 0x8020208, 0x8, 0x20208, 0x20200, 0x8000008, 0x8020000, 0x8000208, 0x208, 0x8020000, 0x20208, 0x8, 0x8020008, 0x20200);
        var spfunction4 = new Array(0x802001, 0x2081, 0x2081, 0x80, 0x802080, 0x800081, 0x800001, 0x2001, 0, 0x802000, 0x802000, 0x802081, 0x81, 0, 0x800080, 0x800001, 0x1, 0x2000, 0x800000, 0x802001, 0x80, 0x800000, 0x2001, 0x2080, 0x800081, 0x1, 0x2080, 0x800080, 0x2000, 0x802080, 0x802081, 0x81, 0x800080, 0x800001, 0x802000, 0x802081, 0x81, 0, 0, 0x802000, 0x2080, 0x800080, 0x800081, 0x1, 0x802001, 0x2081, 0x2081, 0x80, 0x802081, 0x81, 0x1, 0x2000, 0x800001, 0x2001, 0x802080, 0x800081, 0x2001, 0x2080, 0x800000, 0x802001, 0x80, 0x800000, 0x2000, 0x802080);
        var spfunction5 = new Array(0x100, 0x2080100, 0x2080000, 0x42000100, 0x80000, 0x100, 0x40000000, 0x2080000, 0x40080100, 0x80000, 0x2000100, 0x40080100, 0x42000100, 0x42080000, 0x80100, 0x40000000, 0x2000000, 0x40080000, 0x40080000, 0, 0x40000100, 0x42080100, 0x42080100, 0x2000100, 0x42080000, 0x40000100, 0, 0x42000000, 0x2080100, 0x2000000, 0x42000000, 0x80100, 0x80000, 0x42000100, 0x100, 0x2000000, 0x40000000, 0x2080000, 0x42000100, 0x40080100, 0x2000100, 0x40000000, 0x42080000, 0x2080100, 0x40080100, 0x100, 0x2000000, 0x42080000, 0x42080100, 0x80100, 0x42000000, 0x42080100, 0x2080000, 0, 0x40080000, 0x42000000, 0x80100, 0x2000100, 0x40000100, 0x80000, 0, 0x40080000, 0x2080100, 0x40000100);
        var spfunction6 = new Array(0x20000010, 0x20400000, 0x4000, 0x20404010, 0x20400000, 0x10, 0x20404010, 0x400000, 0x20004000, 0x404010, 0x400000, 0x20000010, 0x400010, 0x20004000, 0x20000000, 0x4010, 0, 0x400010, 0x20004010, 0x4000, 0x404000, 0x20004010, 0x10, 0x20400010, 0x20400010, 0, 0x404010, 0x20404000, 0x4010, 0x404000, 0x20404000, 0x20000000, 0x20004000, 0x10, 0x20400010, 0x404000, 0x20404010, 0x400000, 0x4010, 0x20000010, 0x400000, 0x20004000, 0x20000000, 0x4010, 0x20000010, 0x20404010, 0x404000, 0x20400000, 0x404010, 0x20404000, 0, 0x20400010, 0x10, 0x4000, 0x20400000, 0x404010, 0x4000, 0x400010, 0x20004010, 0, 0x20404000, 0x20000000, 0x400010, 0x20004010);
        var spfunction7 = new Array(0x200000, 0x4200002, 0x4000802, 0, 0x800, 0x4000802, 0x200802, 0x4200800, 0x4200802, 0x200000, 0, 0x4000002, 0x2, 0x4000000, 0x4200002, 0x802, 0x4000800, 0x200802, 0x200002, 0x4000800, 0x4000002, 0x4200000, 0x4200800, 0x200002, 0x4200000, 0x800, 0x802, 0x4200802, 0x200800, 0x2, 0x4000000, 0x200800, 0x4000000, 0x200800, 0x200000, 0x4000802, 0x4000802, 0x4200002, 0x4200002, 0x2, 0x200002, 0x4000000, 0x4000800, 0x200000, 0x4200800, 0x802, 0x200802, 0x4200800, 0x802, 0x4000002, 0x4200802, 0x4200000, 0x200800, 0, 0x2, 0x4200802, 0, 0x200802, 0x4200000, 0x800, 0x4000002, 0x4000800, 0x800, 0x200002);
        var spfunction8 = new Array(0x10001040, 0x1000, 0x40000, 0x10041040, 0x10000000, 0x10001040, 0x40, 0x10000000, 0x40040, 0x10040000, 0x10041040, 0x41000, 0x10041000, 0x41040, 0x1000, 0x40, 0x10040000, 0x10000040, 0x10001000, 0x1040, 0x41000, 0x40040, 0x10040040, 0x10041000, 0x1040, 0, 0, 0x10040040, 0x10000040, 0x10001000, 0x41040, 0x40000, 0x41040, 0x40000, 0x10041000, 0x1000, 0x40, 0x10040040, 0x1000, 0x41040, 0x10001000, 0x40, 0x10000040, 0x10040000, 0x10040040, 0x10000000, 0x40000, 0x10001040, 0, 0x10041040, 0x40040, 0x10000040, 0x10040000, 0x10001000, 0x10001040, 0, 0x10041040, 0x41000, 0x41000, 0x1040, 0x1040, 0x40040, 0x10000000, 0x10041000);

        //create the 16 or 48 subkeys we will need
        var keys = des_createKeys(beinetkey);
        var m = 0, i, j, temp, temp2, right1, right2, left, right, looping;
        var cbcleft, cbcleft2, cbcright, cbcright2
        var endloop, loopinc;
        var len = message.length;
        var chunk = 0;
        //set up the loops for single and triple des
        var iterations = keys.length == 32 ? 3 : 9; //single or triple des
        if (iterations == 3) { looping = encrypt ? new Array(0, 32, 2) : new Array(30, -2, -2); }
        else { looping = encrypt ? new Array(0, 32, 2, 62, 30, -2, 64, 96, 2) : new Array(94, 62, -2, 32, 64, 2, 30, -2, -2); }

        message += '\0\0\0\0\0\0\0\0'; //pad the message out with null bytes
        //store the result here
        result = '';
        tempresult = '';

        if (mode == 1) {//CBC mode
            cbcleft = (iv.charCodeAt(m++) << 24) | (iv.charCodeAt(m++) << 16) | (iv.charCodeAt(m++) << 8) | iv.charCodeAt(m++);
            cbcright = (iv.charCodeAt(m++) << 24) | (iv.charCodeAt(m++) << 16) | (iv.charCodeAt(m++) << 8) | iv.charCodeAt(m++);
            m = 0;
        }

        //loop through each 64 bit chunk of the message
        while (m < len) {
            if (encrypt) {//加密时按双字节操作
                left = (message.charCodeAt(m++) << 16) | message.charCodeAt(m++);
                right = (message.charCodeAt(m++) << 16) | message.charCodeAt(m++);
            } else {
                left = (message.charCodeAt(m++) << 24) | (message.charCodeAt(m++) << 16) | (message.charCodeAt(m++) << 8) | message.charCodeAt(m++);
                right = (message.charCodeAt(m++) << 24) | (message.charCodeAt(m++) << 16) | (message.charCodeAt(m++) << 8) | message.charCodeAt(m++);
            }
            //for Cipher Block Chaining mode,xor the message with the previous result
            if (mode == 1) { if (encrypt) { left ^= cbcleft; right ^= cbcright; } else { cbcleft2 = cbcleft; cbcright2 = cbcright; cbcleft = left; cbcright = right; } }

            //first each 64 but chunk of the message must be permuted according to IP
            temp = ((left >>> 4) ^ right) & 0x0f0f0f0f; right ^= temp; left ^= (temp << 4);
            temp = ((left >>> 16) ^ right) & 0x0000ffff; right ^= temp; left ^= (temp << 16);
            temp = ((right >>> 2) ^ left) & 0x33333333; left ^= temp; right ^= (temp << 2);
            temp = ((right >>> 8) ^ left) & 0x00ff00ff; left ^= temp; right ^= (temp << 8);
            temp = ((left >>> 1) ^ right) & 0x55555555; right ^= temp; left ^= (temp << 1);

            left = ((left << 1) | (left >>> 31));
            right = ((right << 1) | (right >>> 31));

            //do this either 1 or 3 times for each chunk of the message
            for (j = 0; j < iterations; j += 3) {
                endloop = looping[j + 1];
                loopinc = looping[j + 2];
                //now go through and perform the encryption or decryption 
                for (i = looping[j]; i != endloop; i += loopinc) {//for efficiency
                    right1 = right ^ keys[i];
                    right2 = ((right >>> 4) | (right << 28)) ^ keys[i + 1];
                    //the result is attained by passing these bytes through the S selection functions
                    temp = left;
                    left = right;
                    right = temp ^ (spfunction2[(right1 >>> 24) & 0x3f] | spfunction4[(right1 >>> 16) & 0x3f] | spfunction6[(right1 >>> 8) & 0x3f] | spfunction8[right1 & 0x3f] | spfunction1[(right2 >>> 24) & 0x3f] | spfunction3[(right2 >>> 16) & 0x3f] | spfunction5[(right2 >>> 8) & 0x3f] | spfunction7[right2 & 0x3f]);
                }
                temp = left; left = right; right = temp; //unreverse left and right
            } //for either 1 or 3 iterations

            //move then each one bit to the right
            left = ((left >>> 1) | (left << 31));
            right = ((right >>> 1) | (right << 31));

            //now perform IP-1,which is IP in the opposite direction
            temp = ((left >>> 1) ^ right) & 0x55555555; right ^= temp; left ^= (temp << 1);
            temp = ((right >>> 8) ^ left) & 0x00ff00ff; left ^= temp; right ^= (temp << 8);
            temp = ((right >>> 2) ^ left) & 0x33333333; left ^= temp; right ^= (temp << 2);
            temp = ((left >>> 16) ^ right) & 0x0000ffff; right ^= temp; left ^= (temp << 16);
            temp = ((left >>> 4) ^ right) & 0x0f0f0f0f; right ^= temp; left ^= (temp << 4);

            //for Cipher Block Chaining mode,xor the message with the previous result
            if (mode == 1) { if (encrypt) { cbcleft = left; cbcright = right; } else { left ^= cbcleft2; right ^= cbcright2; } }
            if (encrypt) {
                tempresult += String.fromCharCode((left >>> 24), ((left >>> 16) & 0xff), ((left >>> 8) & 0xff), (left & 0xff), (right >>> 24), ((right >>> 16) & 0xff), ((right >>> 8) & 0xff), (right & 0xff));
            }
            else { 
                tempresult += String.fromCharCode(((left >>> 16) & 0xffff), (left & 0xffff), ((right >>> 16) & 0xffff), (right & 0xffff)); 
            } //解密时输出双字节
            encrypt ? chunk += 16 : chunk += 8;
            if (chunk == 512) { result += tempresult; tempresult = ''; chunk = 0; }
        } //for every 8 characters,or 64 bits in the message

        //return the result as an array
        return result + tempresult;
    } //end of des

//des_createKeys
//this takes as input a 64 bit beinetkey(even though only 56 bits are used)
//as an array of 2 integers,and returns 16 48 bit keys
    function des_createKeys(beinetkey) {
        //declaring this locally speeds things up a bit
        pc2bytes0 = new Array(0, 0x4, 0x20000000, 0x20000004, 0x10000, 0x10004, 0x20010000, 0x20010004, 0x200, 0x204, 0x20000200, 0x20000204, 0x10200, 0x10204, 0x20010200, 0x20010204);
        pc2bytes1 = new Array(0, 0x1, 0x100000, 0x100001, 0x4000000, 0x4000001, 0x4100000, 0x4100001, 0x100, 0x101, 0x100100, 0x100101, 0x4000100, 0x4000101, 0x4100100, 0x4100101);
        pc2bytes2 = new Array(0, 0x8, 0x800, 0x808, 0x1000000, 0x1000008, 0x1000800, 0x1000808, 0, 0x8, 0x800, 0x808, 0x1000000, 0x1000008, 0x1000800, 0x1000808);
        pc2bytes3 = new Array(0, 0x200000, 0x8000000, 0x8200000, 0x2000, 0x202000, 0x8002000, 0x8202000, 0x20000, 0x220000, 0x8020000, 0x8220000, 0x22000, 0x222000, 0x8022000, 0x8222000);
        pc2bytes4 = new Array(0, 0x40000, 0x10, 0x40010, 0, 0x40000, 0x10, 0x40010, 0x1000, 0x41000, 0x1010, 0x41010, 0x1000, 0x41000, 0x1010, 0x41010);
        pc2bytes5 = new Array(0, 0x400, 0x20, 0x420, 0, 0x400, 0x20, 0x420, 0x2000000, 0x2000400, 0x2000020, 0x2000420, 0x2000000, 0x2000400, 0x2000020, 0x2000420);
        pc2bytes6 = new Array(0, 0x10000000, 0x80000, 0x10080000, 0x2, 0x10000002, 0x80002, 0x10080002, 0, 0x10000000, 0x80000, 0x10080000, 0x2, 0x10000002, 0x80002, 0x10080002);
        pc2bytes7 = new Array(0, 0x10000, 0x800, 0x10800, 0x20000000, 0x20010000, 0x20000800, 0x20010800, 0x20000, 0x30000, 0x20800, 0x30800, 0x20020000, 0x20030000, 0x20020800, 0x20030800);
        pc2bytes8 = new Array(0, 0x40000, 0, 0x40000, 0x2, 0x40002, 0x2, 0x40002, 0x2000000, 0x2040000, 0x2000000, 0x2040000, 0x2000002, 0x2040002, 0x2000002, 0x2040002);
        pc2bytes9 = new Array(0, 0x10000000, 0x8, 0x10000008, 0, 0x10000000, 0x8, 0x10000008, 0x400, 0x10000400, 0x408, 0x10000408, 0x400, 0x10000400, 0x408, 0x10000408);
        pc2bytes10 = new Array(0, 0x20, 0, 0x20, 0x100000, 0x100020, 0x100000, 0x100020, 0x2000, 0x2020, 0x2000, 0x2020, 0x102000, 0x102020, 0x102000, 0x102020);
        pc2bytes11 = new Array(0, 0x1000000, 0x200, 0x1000200, 0x200000, 0x1200000, 0x200200, 0x1200200, 0x4000000, 0x5000000, 0x4000200, 0x5000200, 0x4200000, 0x5200000, 0x4200200, 0x5200200);
        pc2bytes12 = new Array(0, 0x1000, 0x8000000, 0x8001000, 0x80000, 0x81000, 0x8080000, 0x8081000, 0x10, 0x1010, 0x8000010, 0x8001010, 0x80010, 0x81010, 0x8080010, 0x8081010);
        pc2bytes13 = new Array(0, 0x4, 0x100, 0x104, 0, 0x4, 0x100, 0x104, 0x1, 0x5, 0x101, 0x105, 0x1, 0x5, 0x101, 0x105);

        //how many iterations(1 for des,3 for triple des)
        var iterations = beinetkey.length >= 24 ? 3 : 1;
        //stores the return keys
        var keys = new Array(32 * iterations);
        //now define the left shifts which need to be done
        var shifts = new Array(0, 0, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 0);
        //other variables
        var lefttemp, righttemp, m = 0, n = 0, temp;

        for (var j = 0; j < iterations; j++) {//either 1 or 3 iterations
            left = (beinetkey.charCodeAt(m++) << 24) | (beinetkey.charCodeAt(m++) << 16) | (beinetkey.charCodeAt(m++) << 8) | beinetkey.charCodeAt(m++);
            right = (beinetkey.charCodeAt(m++) << 24) | (beinetkey.charCodeAt(m++) << 16) | (beinetkey.charCodeAt(m++) << 8) | beinetkey.charCodeAt(m++);

            temp = ((left >>> 4) ^ right) & 0x0f0f0f0f; right ^= temp; left ^= (temp << 4);
            temp = ((right >>> -16) ^ left) & 0x0000ffff; left ^= temp; right ^= (temp << -16);
            temp = ((left >>> 2) ^ right) & 0x33333333; right ^= temp; left ^= (temp << 2);
            temp = ((right >>> -16) ^ left) & 0x0000ffff; left ^= temp; right ^= (temp << -16);
            temp = ((left >>> 1) ^ right) & 0x55555555; right ^= temp; left ^= (temp << 1);
            temp = ((right >>> 8) ^ left) & 0x00ff00ff; left ^= temp; right ^= (temp << 8);
            temp = ((left >>> 1) ^ right) & 0x55555555; right ^= temp; left ^= (temp << 1);

            //the right side needs to be shifted and to get the last four bits of the left side
            temp = (left << 8) | ((right >>> 20) & 0x000000f0);
            //left needs to be put upside down
            left = (right << 24) | ((right << 8) & 0xff0000) | ((right >>> 8) & 0xff00) | ((right >>> 24) & 0xf0);
            right = temp;

            //now go through and perform these shifts on the left and right keys
            for (i = 0; i < shifts.length; i++) {
                //shift the keys either one or two bits to the left
                if (shifts[i]) { left = (left << 2) | (left >>> 26); right = (right << 2) | (right >>> 26); }
                else { left = (left << 1) | (left >>> 27); right = (right << 1) | (right >>> 27); }
                left &= -0xf; right &= -0xf;

                //now apply PC-2,in such a way that E is easier when encrypting or decrypting
                //this conversion will look like PC-2 except only the last 6 bits of each byte are used
                //rather than 48 consecutive bits and the order of lines will be according to 
                //how the S selection functions will be applied:S2,S4,S6,S8,S1,S3,S5,S7
                lefttemp = pc2bytes0[left >>> 28] | pc2bytes1[(left >>> 24) & 0xf]
| pc2bytes2[(left >>> 20) & 0xf] | pc2bytes3[(left >>> 16) & 0xf]
| pc2bytes4[(left >>> 12) & 0xf] | pc2bytes5[(left >>> 8) & 0xf]
| pc2bytes6[(left >>> 4) & 0xf];
                righttemp = pc2bytes7[right >>> 28] | pc2bytes8[(right >>> 24) & 0xf]
| pc2bytes9[(right >>> 20) & 0xf] | pc2bytes10[(right >>> 16) & 0xf]
| pc2bytes11[(right >>> 12) & 0xf] | pc2bytes12[(right >>> 8) & 0xf]
| pc2bytes13[(right >>> 4) & 0xf];
                temp = ((righttemp >>> 16) ^ lefttemp) & 0x0000ffff;
                keys[n++] = lefttemp ^ temp; keys[n++] = righttemp ^ (temp << 16);
            }
        } //for each iterations
        //return the keys we've created
        return keys;
    } //end of des_createKeys


///////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////

// 把字符串转换为16进制字符串
// 如：a变成61（即10进制的97）；abc变成616263
function stringToHex(s) {
 var r='';
 var hexes=new Array('0','1','2','3','4','5','6','7','8','9','a','b','c','d','e','f');
 for(var i=0;i<(s.length);i++){r+=hexes[s.charCodeAt(i)>>4]+hexes[s.charCodeAt(i)&0xf];}
 return r;
}
// 16进制字符串转换为字符串
// 如：61（即10进制的97）变成a；616263变成abc
function HexTostring(s){
 var r='';
 for(var i=0;i<s.length;i+=2){
 var sxx=parseInt(s.substring(i,i+2),16);
 r+=String.fromCharCode(sxx);}
 return r;
}

/// 加密测试函数
/// s     待加密的字符串
/// k     密钥
function encMe(s, k){
    return stringToHex(des(k,s,1,0));
}

/// 解密测试函数
/// s     待解密的字符串
/// k     密钥
function uncMe(s, k){
    return des(k,HexTostring(s),0,0);
}
";
            return ret;
        }
        #endregion

        /// <summary>
        /// 用正则表达式截取字符串
        /// </summary>
        /// <param name="s">要截取的字符串</param>
        /// <param name="r">正则表达式</param>
        /// <returns>截取结果</returns>
        public static string MatchRegexCut(string s, string r)
        {
            string ret = "";
            try
            {
                ret = Regex.Match(s, r, RegexOptions.IgnoreCase).Value;
            }
            catch (Exception ex)
            {
                ;
            }
            return ret;
        }



        /// <summary>
        /// 写入运行日志
        /// </summary>
        /// <param name="strLog"></param>
        public static void WriteLog(string strLog)
        {
            try
            {
                string sFilePath = "logs\\" + DateTime.Now.ToString("yyyyMM");
                string sFileName = DateTime.Now.ToString("dd") + ".log";
                sFileName = sFilePath + "\\" + sFileName; //文件的绝对路径
                if (!Directory.Exists(sFilePath))//验证路径是否存在
                {
                    Directory.CreateDirectory(sFilePath);//不存在则创建
                }
                FileStream fs;
                StreamWriter sw;
                if (File.Exists(sFileName))//验证文件是否存在，有则追加，无则创建
                {
                    fs = new FileStream(sFileName, FileMode.Append, FileAccess.Write);
                }
                else
                {
                    fs = new FileStream(sFileName, FileMode.Create, FileAccess.Write);
                }
                sw = new StreamWriter(fs);
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + "     ---     " + strLog);
                sw.WriteLine("");
                sw.Close();
                fs.Close();
            }
            catch (Exception ex)
            {
                ;
            }


        }


        /// <summary>
        /// 用浏览器打开一个网址，默认IE,找不到IE，用其他默认浏览器打开
        /// </summary>
        /// <param name="urlstr">网址</param>
        public static void OpenUrl(string urlstr)
        {
            if (System.IO.File.Exists("C:\\Program Files\\Internet Explorer\\iexplore.exe"))
            {
                System.Diagnostics.Process.Start("C:\\Program Files\\Internet Explorer\\iexplore.exe", "-new " + urlstr);
            }
            else
            {
                System.Diagnostics.Process.Start(urlstr);
            }

        }




        /*使用方法：
            Hashtable HTforParameter = new Hashtable();
            HTforParameter["用户填写的值1"] = "1234";
            HTforParameter["用户填写的2"] = "32423";
            HTforParameter["用户填写的值3"] = "54767";
            HTforParameter["测试"] = "76867";
            DataTable DSforparameter = StringOP.GetDataTableFormHashtable(HTforParameter);
        */
        /// <summary>
        /// 将哈希表转化成只有一行的数据表，用于webservices参数，只能用字符串值，其他类型参数单独声明
        /// </summary>
        /// <param name="HTforParameter">要转化的哈希表</param>
        /// <returns></returns>
        public static DataTable GetDataTableFormHashtable(Hashtable HTforParameter)
        {
            DataTable dsforparameter = new DataTable();
            dsforparameter.TableName = "传递参数";
            ArrayList zhi = new ArrayList();
            foreach (System.Collections.DictionaryEntry item in HTforParameter)
            {
                dsforparameter.Columns.Add(item.Key.ToString(), typeof(string));
                zhi.Add(item.Value.ToString());
            }
            dsforparameter.Rows.Add(zhi.ToArray());
            return dsforparameter;
        }
       
        /// <summary>
        /// 将哈希表转化成二维交错数组，用于webservices参数，只能用字符串值
        /// </summary>
        /// <param name="HTforParameter">要转化的哈希表</param>
        /// <returns></returns>
        public static string[][] GetstrArryFromHashtable(Hashtable HTforParameter)
        {
            try
            {
                int rown = HTforParameter.Count;
                string[][] strArry = new string[rown][];
                int i = 0;
                foreach (System.Collections.DictionaryEntry item in HTforParameter)
                {
                    strArry[i] = new string[] { item.Key.ToString(), item.Value.ToString() };
                    i++;
                }
                return strArry;
            }
            catch
            {
                return null;
            }
        }
     
        /// <summary>
        /// 将二维交错数组转化成哈希表，用于webservices参数，只能用字符串值
        /// </summary>
        /// <param name="strArray">要转化的二维数组</param>
        /// <returns></returns>
        public static Hashtable GetHashtableFromstrArry(string[][] strArray)
        {
            try
            {
                Hashtable HTresult = new Hashtable();
                int rown = strArray.Length;
                for (int i = 0; i < rown; i++)
                {
                    HTresult.Add(strArray[i][0].ToString(), strArray[i][1].ToString());
                }
                return HTresult;
            }
            catch
            {
                return null;
            }
           
        }

        /// <summary>
        /// 生成随机验证码（数字、字母混合）大小写已失效20130116。
        /// </summary>
        /// <param name="length">验证码长度</param>
        /// <param name="isUperLower">是否区分大小写（不区分都为小写）</param>
        /// <returns></returns>
        public static string GetRandomNumber(int length, bool isUperLower)
        {
            string RandomNumber = string.Empty;
            string[] Letters = {"a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z",   
        "A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z" };
            #region Old
            //int number;
            //char code;
            //Random rd = new Random();
            //for (int i = 0; i < length; i++)
            //{
            //    number = rd.Next();
            //    if (number % 2 == 0)
            //        code = (char)('0' + (char)(number % 10));
            //    else
            //        code = (char)('A' + (char)(number % 26));
            //    RandomNumber += code.ToString();
            //}
            //if (!isUperLower)
            //    RandomNumber.ToLower();
            #endregion
            Random rdNumber = new Random();//随机数字
            Random rdLetter = new Random();//随机字母
            Random NorL = new Random();//决定下一个生成的是字母还是数字
            for (int i = 0; i < length; i++)
            {
                switch (NorL.Next(0, 2))
                {
                    case 0:
                        RandomNumber = RandomNumber + rdNumber.Next(0, 10).ToString();
                        break;
                    case 1:
                        RandomNumber = RandomNumber + Letters[rdLetter.Next(0, Letters.Length)];
                        break;
                }
            }
            return RandomNumber;
        }

        /// <summary>
        /// 交易平台密码规则
        /// </summary>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public static bool ValPwd(string pwd)
        {
            bool b = false;
            if (pwd.Length < 6)
            {
                b = false;
                goto returnTo;
            }
        returnTo:
            return b;
        }



        public static bool ValidateQuery(Hashtable queryConditions)
        {
            //构造SQL的注入关键字符
            #region 字符
            string[] strBadChar = {"and ","and%20"
    ,"exec "
    ,"insert "
    ,"select "
    ,"delete "
    ,"update "
    ,"count "
    ,"or "
    ,"%20"
    //,"*"
    ,"%"
    ," :"
    ,"\'"
    ,"\""
    ,"chr "
    ,"mid "
    ,"master "
    ,"truncate "
    ,"char "
    ,"declare "
    ,"SiteName "
    ,"net user "
    ,"xp_cmdshell "
    ,"/add"
    ,"exec master.dbo.xp_cmdshell "
    ,"net localgroup administrators "};
            #endregion

            //构造正则表达式
            string str_Regex = ".*(";
            for (int i = 0; i < strBadChar.Length - 1; i++)
            {
                str_Regex += strBadChar[i] + "|";
            }
            str_Regex += strBadChar[strBadChar.Length - 1] + ").*";
            //避免查询条件中_list情况
            foreach (string str in queryConditions.Keys)
            {
                if (str.Substring(str.Length - 5) == "_list")
                {
                    //去掉单引号检验
                    str_Regex = str_Regex.Replace("|'|", "|");
                }
                string tempStr = queryConditions[str].ToString();
                if (Regex.Matches(tempStr.ToString(), str_Regex).Count > 0)
                {
                    //有SQL注入字符
                    return true;
                }
            }
            return false;
        }





        /// <summary>
        /// 将Dataset序列化成byte[]
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static byte[] DataToByte(DataSet ds)
        {
            try
            {
                byte[] bArrayResult = null;
                ds.RemotingFormat = SerializationFormat.Binary;
                MemoryStream ms = new MemoryStream();
                IFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, ds);
                bArrayResult = ms.ToArray();
                ms.Close();
                ms.Dispose();
                return bArrayResult;
            }
            catch
            {
                return null;
            }

        }
        /// <summary>
        /// 将byte[]反序列化成Dataset
        /// </summary>
        /// <param name="bArrayResult"></param>
        /// <returns></returns>
        public static DataSet ByteToDataset(byte[] bArrayResult)
        {
            try
            {
                DataSet dsResult = null;
                MemoryStream ms = new MemoryStream(bArrayResult);
                IFormatter bf = new BinaryFormatter();
                object obj = bf.Deserialize(ms);
                dsResult = (DataSet)obj;
                ms.Close();
                ms.Dispose();
                return dsResult;
            }
            catch
            {
                return null;
            }
    
        }

        //public static DataSet ConvertXMLToDataSet(string xmlData)
        //{
        //    if (xmlData == null || xmlData.Trim() == "")
        //    {
        //        return null;
        //    }
        //    StringReader stream = null;
        //    XmlTextReader reader = null;
        //    try
        //    {
        //        stream = new StringReader(xmlData);

        //        DataSet xmlDS = new DataSet();
        //        xmlDS.ReadXml(stream, XmlReadMode.ReadSchema);


        //        return xmlDS;
        //    }
        //    catch (System.Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (reader != null)
        //            reader.Close();
        //    }
        //}


        //public static string ConvertDataSetToXML(DataSet xmlDS)
        //{
        //    MemoryStream stream = null;
        //    XmlTextWriter writer = null;

        //    try
        //    {
        //        stream = new MemoryStream();
        //        //从stream装载到XmlTextReader
        //        writer = new XmlTextWriter(stream, Encoding.UTF8);

        //        //用WriteXml方法写入文件.
        //        xmlDS.WriteXml(writer, XmlWriteMode.WriteSchema);
        //        int count = (int)stream.Length;
        //        byte[] arr = new byte[count];
        //        stream.Seek(0, SeekOrigin.Begin);
        //        stream.Read(arr, 0, count);

        //        UnicodeEncoding utf = new UnicodeEncoding();
        //        return Encoding.UTF8.GetString(arr);
        //    }
        //    catch (System.Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (writer != null)
        //            writer.Close();
        //    }
        //}

        /// <summary>
        /// 将类型序列化为byte数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t">要序列化的类型</param>
        /// <returns>byte数组</returns>
        public static byte[] Serialize<T>(T t)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, t);                
                return stream.ToArray();
            }
        }

        /// <summary>
        /// 将byte数组反序列化为类型
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="s">byte数组</param>
        /// <returns>类型</returns>
        public static TResult Deserialize<TResult>(byte[] bs) where TResult : class
        {
            using (MemoryStream stream = new MemoryStream(bs))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return formatter.Deserialize(stream) as TResult;
            }
        }

    }
}
