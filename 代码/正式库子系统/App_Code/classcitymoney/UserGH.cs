using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using FMOP.DB;

/// <summary>
///员工工号的自动生成和处理
/// </summary>
public class UserGH
{




	public UserGH()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//

        
	}

    /// <summary>
    /// 得到与现有工号不重复的新的随机工号，不含有3和4.
    /// </summary>
    /// <returns></returns>
    public static string getNewGH()
    {
        int endnumber = checkNumber();
        return endnumber.ToString().PadLeft(7, '0');
    }

    /// <summary>
    /// 产生不跟已有数据重复的数字
    /// </summary>
    public static int checkNumber()
    {
        int endnumber = GetRandomNum(System.DateTime.Now.Millisecond, 7, 99999, 9999999);


        if (checkformoldnumber(endnumber))//如果跟历史数据重复
        {
            endnumber = GetRandomNum(System.DateTime.Now.Millisecond, 7, 99999, 9999999);
            checkNumber();
        }
        return endnumber;
    }



    /// <summary>
    /// 产生不重复随机数
    /// </summary>
    /// <param name="i"></param>
    /// <param name="length"></param>
    /// <param name="up"></param>
    /// <param name="down"></param>
    /// <returns></returns>
    public static int GetRandomNum(int i, int length, int up, int down)
    {
        int iFirst = 0;
        Random ro = new Random(i * length * unchecked((int)DateTime.Now.Ticks));
        iFirst = ro.Next(up, down);

        string endnumber_str = iFirst.ToString();
        endnumber_str = endnumber_str.Replace("3", "7").Replace("4", "9");
        iFirst = Convert.ToInt32(endnumber_str);
        return iFirst;
    }

    public static bool checkformoldnumber(int endnumber)
    {
        DataSet ds = DbHelperSQL.Query("select * from aspnet_Users where UserName = '" + endnumber.ToString() + "'");
        if(ds.Tables[0].Rows.Count < 1)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
