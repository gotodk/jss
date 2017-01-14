﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;

/// <summary>
///DES 的摘要说明
/// </summary>
public class DES
{
    public DES()
    {
        //
        //TODO: 在此处添加构造函数逻辑
        //
    }
    /// <summary>
    /// 使用DES加密指定字符串
    /// </summary>
    /// <param name="encryptStr">待加密的字符串</param>
    /// <param name="key">密钥(最大长度8)</param>
    /// <param name="IV">初始化向量(最大长度8)</param>
    /// <returns>加密后的字符串</returns>
    public static string DESEncrypt(string encryptStr, string key, string IV)
    {
        return Encode(IV + encryptStr, key);

        ////将key和IV处理成8个字符
        //key += "12345678";
        //IV += "12345678";
        //key = key.Substring(0, 8);
        //IV = IV.Substring(0, 8);

        //SymmetricAlgorithm sa;
        //ICryptoTransform ict;
        //MemoryStream ms;
        //CryptoStream cs;
        //byte[] byt;

        //sa = new DESCryptoServiceProvider();
        //sa.Key = Encoding.UTF8.GetBytes(key);
        //sa.IV = Encoding.UTF8.GetBytes(IV);
        //ict = sa.CreateEncryptor();

        //byt = Encoding.UTF8.GetBytes(encryptStr);

        //ms = new MemoryStream();
        //cs = new CryptoStream(ms, ict, CryptoStreamMode.Write);
        //cs.Write(byt, 0, byt.Length);
        //cs.FlushFinalBlock();

        //cs.Close();

        ////加上一些干扰字符
        //string retVal = Convert.ToBase64String(ms.ToArray());
        //System.Random ra = new Random();

        //for (int i = 0; i < 8; i++)
        //{
        //    int radNum = ra.Next(36);
        //    char radChr = Convert.ToChar(radNum + 65);//生成一个随机字符

        //    retVal = retVal.Substring(0, 2 * i + 1) + radChr.ToString() + retVal.Substring(2 * i + 1);
        //}

        //return retVal;
    }

    /// <summary>
    /// 使用DES解密指定字符串
    /// </summary>
    /// <param name="encryptedValue">待解密的字符串</param>
    /// <param name="key">密钥(最大长度8)</param>
    /// <param name="IV">初始化向量(最大长度8)</param>
    /// <returns>解密后的字符串</returns>
    public static string DESDecrypt(string encryptedValue, string key, string IV)
    {
        return Decode(encryptedValue, key).Replace(IV, "");

        ////去掉干扰字符
        //string tmp = encryptedValue;
        //if (tmp.Length < 16)
        //{
        //    return "";
        //}

        //for (int i = 0; i < 8; i++)
        //{
        //    tmp = tmp.Substring(0, i + 1) + tmp.Substring(i + 2);
        //}
        //encryptedValue = tmp;

        ////将key和IV处理成8个字符
        //key += "12345678";
        //IV += "12345678";
        //key = key.Substring(0, 8);
        //IV = IV.Substring(0, 8);

        //SymmetricAlgorithm sa;
        //ICryptoTransform ict;
        //MemoryStream ms;
        //CryptoStream cs;
        //byte[] byt;

        //try
        //{
        //    sa = new DESCryptoServiceProvider();
        //    sa.Key = Encoding.UTF8.GetBytes(key);
        //    sa.IV = Encoding.UTF8.GetBytes(IV);
        //    ict = sa.CreateDecryptor();

        //    byt = Convert.FromBase64String(encryptedValue);

        //    ms = new MemoryStream();
        //    cs = new CryptoStream(ms, ict, CryptoStreamMode.Write);
        //    cs.Write(byt, 0, byt.Length);
        //    cs.FlushFinalBlock();

        //    cs.Close();

        //    return Encoding.UTF8.GetString(ms.ToArray());
        //}
        //catch (System.Exception)
        //{
        //    return "";
        //}

    }






    /// <summary>
    ///Des 加密 GB2312 
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string Encode(string str)
    {
        return Encode(str, "TMDkey22");
    }
    /// <summary>
    /// Des 加密 GB2312 
    /// </summary>
    /// <param name="str"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string Encode(string str, string key)
    {
        DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
        provider.Key = Encoding.ASCII.GetBytes(key.Substring(0, 8));
        provider.IV = Encoding.ASCII.GetBytes(key.Substring(0, 8));
        byte[] bytes = Encoding.GetEncoding("GB2312").GetBytes(str);
        MemoryStream stream = new MemoryStream();
        CryptoStream stream2 = new CryptoStream(stream, provider.CreateEncryptor(), CryptoStreamMode.Write);
        stream2.Write(bytes, 0, bytes.Length);
        stream2.FlushFinalBlock();
        StringBuilder builder = new StringBuilder();
        foreach (byte num in stream.ToArray())
        {
            builder.AppendFormat("{0:X2}", num);
        }
        stream.Close();
        return builder.ToString();
    }

    /// <summary>
    /// Des 解密 GB2312 
    /// </summary>
    /// <param name="str">Desc string </param>
    /// <returns></returns>
    public static string Decode(string str)
    {
        return Decode(str, "TMDkey22");
    }
    /// <summary>
    /// Des 解密 GB2312 
    /// </summary>
    /// <param name="str">Desc string</param>
    /// <param name="key">Key ,必须为8位 </param>
    /// <returns></returns>
    public static string Decode(string str, string key)
    {
        DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
        provider.Key = Encoding.ASCII.GetBytes(key.Substring(0, 8));
        provider.IV = Encoding.ASCII.GetBytes(key.Substring(0, 8));
        byte[] buffer = new byte[str.Length / 2];
        for (int i = 0; i < (str.Length / 2); i++)
        {
            int num2 = Convert.ToInt32(str.Substring(i * 2, 2), 0x10);
            buffer[i] = (byte)num2;
        }
        MemoryStream stream = new MemoryStream();
        CryptoStream stream2 = new CryptoStream(stream, provider.CreateDecryptor(), CryptoStreamMode.Write);
        stream2.Write(buffer, 0, buffer.Length);
        stream2.FlushFinalBlock();
        stream.Close();
        return Encoding.GetEncoding("GB2312").GetString(stream.ToArray());
    }

}
