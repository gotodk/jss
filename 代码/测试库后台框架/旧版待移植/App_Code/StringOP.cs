using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
///StringOP 的摘要说明
/// </summary>
public class StringOP
{
	public StringOP()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}
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
}