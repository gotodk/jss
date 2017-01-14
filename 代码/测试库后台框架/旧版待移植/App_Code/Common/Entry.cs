using System;
using System.Collections.Generic;
using System.Web;
using System.Security.Cryptography;
using System.Text;

/// <summary>
///Entry 的摘要说明
/// </summary>
public class Entry
{
	public Entry()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}

    /// <summary>
    ///静态字段Md5的实例对象
    /// </summary>
    private static MD5 _md5 = MD5.Create();

    /// <summary>
    ///用来保存密码字符加密后的字节数组
    /// </summary>
    private static byte[] _passwordByte;

    /// <summary>
    /// 经过处理后最终得到的密码字符
    /// </summary>
    private static string _password;

    /// <summary>
    /// 对密码字符进行加密
    /// </summary>
    /// <param name="PasswordString">要加密的字符</param>
    /// <param name="start">对加密后的字符开始截取的位置</param>
    /// <param name="length">截取的长度</param>
    /// <returns>加密后的字符</returns>
    public static string EntryPassword(string passwordString, int start, int length)
    {
        //密码字符加密后是一个字节类型的数组
        _passwordByte = _md5.ComputeHash(Encoding.Unicode.GetBytes(passwordString));
        //通过使用循环，将字节类型的数组转化为字符串，此字符串是常规字符格式化所得
        for (int i = 0; i < _passwordByte.Length; i++)
        {
            //将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符
            _password = _password + _passwordByte[i].ToString("X");
        }
        return _password.Substring(start, length);
    }
}