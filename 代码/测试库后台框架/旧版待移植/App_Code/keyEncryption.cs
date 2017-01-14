using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using FMOP.DB;
using Hesion.Brick.Core.WorkFlow;
using System.Net;
using System.IO;
using System.Web.Services.Description;
using System.CodeDom;
using Microsoft.CSharp;
using System.CodeDom.Compiler;

namespace Key
{
    /// <summary>
    ///keyEncryption 的摘要说明
    /// </summary>
    public class keyEncryption
    {
        public keyEncryption()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
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
    }


    public class YHZTC_tixing
    {
        /// <summary>
        /// 用户直通车提醒发送，用户、推广、服务都用这个（字段都不能是null）。如用户端发往推广人员的，则填写两方帐号，同时服务站点留空，并标记发往对象即可。
        /// </summary>
        /// <param name="YHZH">用户帐号</param>
        /// <param name="TGRYZH">推广人员帐号</param>
        /// <param name="FWZDZH">服务站点帐号</param>
        /// <param name="SFXSG">是否显示过(插入时，默认否)</param>
        /// <param name="TXXXNR">提醒详细内容</param>
        /// <param name="FWDX">发往对象(用户,推广人员,服务站点)</param>
        /// <param name="TXLX">提醒类型(下达订单,交付旧硒鼓,出售旧硒鼓,交易中心,合格旧硒鼓调拨,其他)</param>
        public void SendForYHZTC(string YHZH, string TGRYZH, string FWZDZH, string SFXSG, string TXXXNR, string FWDX, string TXLX)
        {
            WorkFlowModule WFM = new WorkFlowModule("FM_YHZTCTX");
            string KeyNumber = WFM.numberFormat.GetNextNumber();
            string sql_insert = "insert into FM_YHZTCTX(number,YHZH,TGRYZH,FWZDZH,SFXSG,TXXXNR,FWDX,TXLX,CheckState,CreateUser) values ('" + KeyNumber + "','" + YHZH + "','" + TGRYZH + "','" + FWZDZH + "','" + SFXSG + "','" + TXXXNR + "','" + FWDX + "','" + TXLX + "',1,'admin' )";
            DbHelperSQL.ExecuteSql(sql_insert);
        }


        public void SendForFMOP(string modname, string msg, string url)
        {
            SendForFMOP("", modname, msg, url);
        }

        /// <summary>
        /// 向业务平台发送提醒。 在[用户直通车]-[提醒人设置] 中增加提醒人。分为两种情况,如果员工编号为空，则向岗位批量发送提醒
        //若接受提醒部门的为办事处，须调用此方法，且bm参数在调用程序中设置
        //若接受提醒部门为非办事处，bm参数在程序中可为空，也可不写。接受提醒部门在[用户直通车]-[提醒人设置]设置。
        /// </summary>
        /// <param name="bm">部门名称</param>
        /// <param name="modname">模块名称</param>
        /// <param name="msg">提醒信息内容</param>
        /// <param name="url">提醒点击查看后跳转到的URL</param>
     
        public void SendForFMOP(string bm,string modname,string msg,string url)
        {
            
            DataSet ds_touser = new DataSet();
            if (bm.IndexOf("办事处") > 0)
            {
                ds_touser = DbHelperSQL.Query("select * from FM_TXRGL where txmk='" + modname + "' and BM='" + bm + "'");
            }
            else
            {
                ds_touser = DbHelperSQL.Query("select * from FM_TXRGL where txmk='" + modname + "'");
            
            }
            if (ds_touser.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds_touser.Tables[0].Rows.Count; i++)
                {
                    //分为两种情况,如果员工编号为空，则向岗位批量发送提醒。
                    if (ds_touser.Tables[0].Rows[i]["ygbh"].ToString() == "")
                    {
                        string ssql = "select number,employee_name,bm,gwmc from HR_employees where  ygzt not like '%离职%' and gwmc='" + ds_touser.Tables[0].Rows[i]["GW"].ToString().Trim() + "' and BM='" + ds_touser.Tables[0].Rows[i]["BM"].ToString().Trim() + "'";
                        DataSet ds_touser2 = DbHelperSQL.Query(ssql);
                        if (ds_touser2 != null && ds_touser2.Tables[0].Rows.Count > 0)
                        {
                            for (int t = 0; t < ds_touser2.Tables[0].Rows.Count; t++)
                            {
                                string touser = ds_touser2.Tables[0].Rows[t]["number"].ToString();
                                WorkFlowModule wf = new WorkFlowModule(modname);
                                wf.authentication.InsertWarnings(msg, url, "1", "", touser);
                            }
                        }

                    }
                    else
                    {
                        string touser = ds_touser.Tables[0].Rows[i]["ygbh"].ToString();
                        WorkFlowModule wf = new WorkFlowModule(modname);
                        wf.authentication.InsertWarnings(msg, url, "1", "", touser);
                    }
                }
            }
        }





        /// <summary>
        /// 插入用户直通车提醒
        /// </summary>
        /// <param name="YHZH">用户账号</param>
        /// <param name="FWSZH">服务商账号</param>
        /// <param name="TXCSRQ">提醒产生日期</param>
        /// <param name="SFXSG">是否显示过</param>
        /// <param name="TXXXNR">详细提醒内容</param>
        /// <param name="TXLX">提醒类型</param>
        public void insertTX(string YHZH, string FWSZH, string TXCSRQ, string SFXSG, string TXXXNR, string TXLX)
        {
            WorkFlowModule WFM = new WorkFlowModule("YHZTC_TIXING");
            string KeyNumber = WFM.numberFormat.GetNextNumber();
            string sql_insert = "insert into YHZTC_TIXING(number,YHZH,FWSZH,TXCSRQ,SFXSG,TXXXNR,TXLX) values ('" + KeyNumber + "','" + YHZH + "','" + FWSZH + "','" + TXCSRQ + "','" + SFXSG + "','" + TXXXNR + "','" + TXLX + "' )";
            DbHelperSQL.ExecuteSql(sql_insert);
        }
        //插入用户直通车提醒（增加提醒类型）
        public void insertTX(string YHZH, string FWSZH, string TXCSRQ, string SFXSG, string TXXXNR, string TXLX, string XXTXLX)
        {
            WorkFlowModule WFM = new WorkFlowModule("YHZTC_TIXING");
            string KeyNumber = WFM.numberFormat.GetNextNumber();
            string sql_insert = "insert into YHZTC_TIXING(number,YHZH,FWSZH,TXCSRQ,SFXSG,TXXXNR,TXLX,XXTXLX) values ('" + KeyNumber + "','" + YHZH + "','" + FWSZH + "','" + TXCSRQ + "','" + SFXSG + "','" + TXXXNR + "','" + TXLX + "','"+XXTXLX+"' )";
            DbHelperSQL.ExecuteSql(sql_insert);
        }
    }


    public class PageViews
    {
        public PageViews()
        { 
            //添加构造函数

        }
        /// <summary>
        /// 将访问情况插入访问明细记录表（仅用于服务商直通车）
        /// </summary>
        /// <param name="page">当前页，一般为this</param>
        /// <param name="email">服务商登录邮箱</param>    
        public void InsertPageView(Page page,string email)
        {

            //临时禁用某些分公司的某些业务开始=====================
            string thispagesp = page.Request.Url.ToString().ToLower();
            object objKHBH = DbHelperSQL.GetSingle("select ssbsc from FWPT_YHXXB where dlyx='" + email + "'");
            if (objKHBH != null)
            {
                string ssbsc = objKHBH.ToString();
                




                //被彻底关闭的分公司,直接出售
                DataSet dsstop = DbHelperSQL.Query("select * from YWZTSD where SSBSC = '" + ssbsc + "'");
                if (dsstop.Tables[0].Rows[0]["ZJCSYWZT"].ToString() == "暂停业务" && thispagesp.IndexOf(("FWPTZS/CT/JXGzhijiechushou.aspx").ToLower()) >= 0)
                {
                    HttpContext.Current.Response.Redirect("/FWPTZS/err_norun.aspx?nowmail=" + Key.keyEncryption.encMe(email, "mimamima") + "&thisurl=" + Key.keyEncryption.encMe

(thispagesp, "mimamima"));
                    HttpContext.Current.Response.End();
                }
                //被彻底关闭的分公司,押金返还
                if (dsstop.Tables[0].Rows[0]["YJFHYW"].ToString() == "暂停业务" && thispagesp.IndexOf(("FWPTZS/CT/YajintuihuanCT.aspx").ToLower()) >= 0)
                {
                    HttpContext.Current.Response.Redirect("/FWPTZS/err_norun.aspx?nowmail=" + Key.keyEncryption.encMe(email, "mimamima") + "&thisurl=" + Key.keyEncryption.encMe

(thispagesp, "mimamima"));
                    HttpContext.Current.Response.End();
                }
                //被彻底关闭的分公司,订单撤销
                if (dsstop.Tables[0].Rows[0]["DDCXYW"].ToString() == "暂停业务" && thispagesp.IndexOf(("fwptzs/FWPT_DHDCX.aspx").ToLower()) >= 0)
                {
                    HttpContext.Current.Response.Redirect("/FWPTZS/err_norun.aspx?nowmail=" + Key.keyEncryption.encMe(email, "mimamima") + "&thisurl=" + Key.keyEncryption.encMe

(thispagesp, "mimamima"));
                    HttpContext.Current.Response.End();
                }
                //被彻底关闭的分公司,其他：下达服务商订单，从草稿进入下达订单，春雨行动订单，分公司自定订单，积分兑换单，积分兑换单，申请提现,交易中心指令，出售明细，买入明细
                if (dsstop.Tables[0].Rows[0]["PTYWZT"].ToString() == "暂停业务")
                {
                    //下达服务商订单，从草稿进入下达订单，春雨行动订单，分公司自定订单，积分兑换单，积分兑换单，押金返还，申请提现,交易中心指令,直接出售，订单撤销，出售明细，买入明细
                    if (
thispagesp.IndexOf (("FWPTZS/FWPTZS_TJJXGXXYM.aspx").ToLower ())>=0||
                        thispagesp.IndexOf (("FWPTZS/2013DHXQD.aspx").ToLower ())>=0||
                        thispagesp.IndexOf(("FWPTZS/DHXQD.aspx").ToLower()) >= 0 ||
                        thispagesp.IndexOf(("FWPTZS/DHXQD_TEMP.aspx").ToLower()) >= 0 ||
                        thispagesp.IndexOf(("FWPTZS/DHXQD_ChunYu.aspx").ToLower()) >= 0 ||
                        thispagesp.IndexOf(("FWPTZS/DHfgszddd.aspx").ToLower()) >= 0 ||
                        thispagesp.IndexOf(("FWPTZS/JFDH.aspx").ToLower()) >= 0 ||
                        thispagesp.IndexOf(("FWPTZS/ApplyCash.aspx").ToLower()) >= 0 ||
                        thispagesp.IndexOf(("FWPTZS/LobbyShow.aspx").ToLower()) >= 0 ||
                        thispagesp.IndexOf(("FWPTZS/HGJXGCS_gl2.aspx").ToLower()) >= 0 ||
                        thispagesp.IndexOf(("FWPTZS/PurchaseHistory.aspx").ToLower()) >= 0||
                        thispagesp.IndexOf (("FWPTZS/FWPTZS_TJJXGXXYM.aspx").ToLower ())>=0)
                    {
                        HttpContext.Current.Response.Redirect("/FWPTZS/err_norun.aspx?nowmail=" + Key.keyEncryption.encMe(email, "mimamima") + "&thisurl=" + 

Key.keyEncryption.encMe(thispagesp, "mimamima"));
                        HttpContext.Current.Response.End();
                    }
                }

            }
            //临时禁用某些分公司的某些业务结束=====================

            try
            {
             
                string url = page.Request.Url.ToString();
                string urlS = "";
                if (url.IndexOf("?") >= 0)
                {
                    urlS = url.Substring(0, url.IndexOf("?"));
                }
                else
                {
                    urlS = url;
                }
                string title = page.Title.ToString();
                string ip = page.Request.UserHostAddress.ToString();
                DataSet ds = DbHelperSQL.Query("select * from fwpt_yhxxb where dlyx='" + email + "'");
                string fwsbh = "";
                string fwsmc = "";
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    fwsbh = ds.Tables[0].Rows[0]["khbh"].ToString();
                    fwsmc = ds.Tables[0].Rows[0]["dwqc"].ToString();
                }
                string fwtime = DateTime.Now.ToString();

                WorkFlowModule WFM = new WorkFlowModule("FWPT_pageview");
                string KeyNumber = WFM.numberFormat.GetNextNumber();

                string sql_insert = "insert into FWPT_pageview(number,dlyx,fwsbh,fwsmc,wz,wym,fwzip,fwsj,createuser) values ('" + KeyNumber + "','" + email + "','" + fwsbh + "','" + fwsmc + "','" + urlS + "','" + title + "','" + ip + "','" + fwtime + "','" + fwsbh + "')";
                DbHelperSQL.ExecuteSql(sql_insert);
            }
            catch
            { 
                
            }            
        }

        /// <summary>
        /// 将访问情况插入访问明细记录表(仅用于用户直通车)
        /// </summary>
        /// <param name="page">当前页，一般为this</param>
        /// <param name="email">用户登录邮箱</param>    
        /// <param name="f_user">是用户直通车的统计</param>    
        public void InsertPageView(Page page, string email,string f_user)
        {
            try
            {
                
                string url = page.Request.Url.ToString();

                string urlS = "";
                if (url.IndexOf("?") >= 0)
                {
                    urlS = url.Substring(0, url.IndexOf("?"));
                }
                else
                {
                    urlS = url;
                }

                string title = page.Title.ToString();
                string ip = page.Request.UserHostAddress.ToString();
                DataSet ds = DbHelperSQL.Query("select * from YHZTC_Users where dlyx='" + email + "'");
                string fwsbh = "";
                string fwsmc = "";
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    fwsbh = ds.Tables[0].Rows[0]["SSFWSBH"].ToString();
                    fwsmc = ds.Tables[0].Rows[0]["SSFWSMC"].ToString();
                }
                string fwtime = DateTime.Now.ToString();

                WorkFlowModule WFM = new WorkFlowModule("YHZTC_pageview");
                string KeyNumber = WFM.numberFormat.GetNextNumber();

                string sql_insert = "insert into YHZTC_pageview(number,dlyx,SSFWSBH,SSFWSMC,wz,wym,fwzip,fwsj,createuser) values ('" + KeyNumber + "','" + email + "','" + fwsbh + "','" + fwsmc + "','" + urlS + "','" + title + "','" + ip + "','" + fwtime + "','admin')";
                DbHelperSQL.ExecuteSql(sql_insert);
            }
            catch
            {

            }     
        }




        /// <summary>
        /// 将访问情况插入访问明细记录表(仅用三方新版用户直通车，三个角色公用)
        /// </summary>
        /// <param name="page">当前页，一般为this</param>
        /// <param name="email">登录邮箱</param>    
        /// <param name="f_user">用户类型(用户,推广人员,服务站点)</param>    
        public void InsertPageView_new(Page page, string email, string f_user)
        {
            try
            {
         
                string url = page.Request.Url.ToString();

                string urlS = "";
                if (url.IndexOf("?") >= 0)
                {
                    urlS = url.Substring(0, url.IndexOf("?"));
                }
                else
                {
                    urlS = url;
                }

                string title = page.Title.ToString();
                string ip = page.Request.UserHostAddress.ToString();
                string yhmc = "";
                if (f_user == "用户")
                {
                    DataSet ds = DbHelperSQL.Query("select * from FM_UsersZXYH where DLZH='" + email + "'");
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        yhmc = ds.Tables[0].Rows[0]["YYYGSJGMCKHMC"].ToString();
                    }
                }
                if (f_user == "推广人员")
                {
                    DataSet ds = DbHelperSQL.Query("select * from FM_UsersTGRY where DLZH='" + email + "'");
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        yhmc = ds.Tables[0].Rows[0]["YYYGSJGMCKHMC"].ToString();
                    }
                }
                if (f_user == "服务站点")
                {
                    DataSet ds = DbHelperSQL.Query("select * from FM_UsersFWZD where DLZH='" + email + "'");
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        yhmc = ds.Tables[0].Rows[0]["ZDMC"].ToString();
                    }
                }
                WorkFlowModule WFM = new WorkFlowModule("FM_pageview");
                string KeyNumber = WFM.numberFormat.GetNextNumber();

                string sql_insert = "insert into FM_pageview(number,dlyx,YHMC,WZ,WYM,FWZIP,YHLX) values ('" + KeyNumber + "','" + email + "','" + yhmc + "','" + urlS + "','" + title + "','" + ip + "','" + f_user + "')";
                DbHelperSQL.ExecuteSql(sql_insert);
            }
            catch
            {

            }
        }
    }


 public static class ChouJjiang
{
        public static int choujiang = 1;//是1的时候表示可以抽奖，0的时候表示不可以抽奖
}


 public static class WebServiceHelper
 {
     #region InvokeWebService
     //动态调用web服务
     public static object InvokeWebService(string url, string methodname, object[] args)
     {
         return WebServiceHelper.InvokeWebService(url, null, methodname, args);
     }

     public static object InvokeWebService(string url, string classname, string methodname, object[] args)
     {
         string @namespace = "EnterpriseServerBase.WebService.DynamicWebCalling";
         if ((classname == null) || (classname == ""))
         {
             classname = WebServiceHelper.GetWsClassName(url);
         }

         try
         {
             //获取WSDL
             WebClient wc = new WebClient();
             Stream stream = wc.OpenRead(url + "?WSDL");
             ServiceDescription sd = ServiceDescription.Read(stream);
             ServiceDescriptionImporter sdi = new ServiceDescriptionImporter();
             sdi.AddServiceDescription(sd, "", "");
             CodeNamespace cn = new CodeNamespace(@namespace);

             //生成客户端代理类代码
             CodeCompileUnit ccu = new CodeCompileUnit();
             ccu.Namespaces.Add(cn);
             sdi.Import(cn, ccu);
             CSharpCodeProvider csc = new CSharpCodeProvider();
             ICodeCompiler icc = csc.CreateCompiler();

             //设定编译参数
             CompilerParameters cplist = new CompilerParameters();
             cplist.GenerateExecutable = false;
             cplist.GenerateInMemory = true;
             cplist.ReferencedAssemblies.Add("System.dll");
             cplist.ReferencedAssemblies.Add("System.XML.dll");
             cplist.ReferencedAssemblies.Add("System.Web.Services.dll");
             cplist.ReferencedAssemblies.Add("System.Data.dll");

             //编译代理类
             CompilerResults cr = icc.CompileAssemblyFromDom(cplist, ccu);
             if (true == cr.Errors.HasErrors)
             {
                 System.Text.StringBuilder sb = new System.Text.StringBuilder();
                 foreach (System.CodeDom.Compiler.CompilerError ce in cr.Errors)
                 {
                     sb.Append(ce.ToString());
                     sb.Append(System.Environment.NewLine);
                 }
                 throw new Exception(sb.ToString());
             }

             //生成代理实例，并调用方法
             System.Reflection.Assembly assembly = cr.CompiledAssembly;
             Type t = assembly.GetType(@namespace + "." + classname, true, true);
             object obj = Activator.CreateInstance(t);
             System.Reflection.MethodInfo mi = t.GetMethod(methodname);

             return mi.Invoke(obj, args);
         }
         catch (Exception ex)
         {
             throw new Exception(ex.InnerException.Message, new Exception(ex.InnerException.StackTrace));
         }
     }

     private static string GetWsClassName(string wsUrl)
     {
         string[] parts = wsUrl.Split('/');
         string[] pps = parts[parts.Length - 1].Split('.');

         return pps[0];
     }
     #endregion
 }

}
