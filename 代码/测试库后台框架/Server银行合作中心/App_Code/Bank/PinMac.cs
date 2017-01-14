using System;
using System.Runtime.InteropServices;



/// <summary>
/// 机密解密的基础类，调用HsDes.dll中的函数
/// </summary>
public static class PinMac
{
    [DllImport("HsDes.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern IntPtr AscToHex(ref byte dest, byte[] src, int dest_len);

    [DllImport("HsDes.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern IntPtr HexToAsc(ref byte dest, byte[] src, int src_len);

    [DllImport("HsDes.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int HS_DES(int op, byte[] key, byte[] inblock, ref byte outblock, int len);

    [DllImport("HsDes.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern IntPtr GenerateMAC(int method, byte[] databuf, int datalen, byte[] key, ref byte myMAC);

    /// <summary>
    /// 加密方法
    /// </summary>
    /// <param name="pin"></param>
    /// <param name="word"></param>
    /// <returns></returns>
    public static string NewPassword(string pin, string word)
    {
        byte[] key = System.Text.Encoding.GetEncoding("GB2312").GetBytes(pin);
        byte[] outer = new byte[50];
        byte[] bytewotd = System.Text.Encoding.GetEncoding("GB2312").GetBytes(word);

        HS_DES(0, key, bytewotd, ref outer[0], 8);

        byte[] result = new byte[50];
        IntPtr ptrRet = HexToAsc(ref result[0], outer, 8);

        return System.Text.Encoding.GetEncoding("GB2312").GetString(result).Replace("\0","");

    }

    /// <summary>
    /// 解密方法
    /// </summary>
    /// <param name="pin"></param>
    /// <param name="word"></param>
    /// <returns></returns>
    public static string InitPassword(string pin, string word)
    {
        byte[] key = System.Text.Encoding.GetEncoding("GB2312").GetBytes(pin);
        byte[] byteword = System.Text.Encoding.GetEncoding("GB2312").GetBytes(word);

        byte[] outer = new byte[50];
        AscToHex(ref outer[0], byteword, 8);


        byte[] result = new byte[50];
        HS_DES(1, key, outer, ref result[0], 8);

        return System.Text.Encoding.GetEncoding("GB2312").GetString(result).Replace("\0","");    

    }

    /// <summary>
    /// 生成MAC
    /// </summary>
    /// <param name="str"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static byte[] NewMac(string str, string key)
    {
        byte[] mac = new byte[8];
        byte[] src = System.Text.Encoding.GetEncoding("GB2312").GetBytes(str);
        byte[] skey = System.Text.Encoding.GetEncoding("GB2312").GetBytes(key);

        int len = src.Length;
        while (len % 8 != 0)
        {
            len += 1;
        }
        GenerateMAC(0, src, len, skey, ref mac[0]);
        return mac;
    }

}