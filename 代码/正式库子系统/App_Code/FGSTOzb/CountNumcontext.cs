using System;
using System.Collections.Generic;
using System.Web;
using System.Collections;

/// <summary>
///CountNumcontext 的摘要说明
/// </summary>
public class CountNumcontext
{

    CountNum countnum = null;
    string Ssbsc = "";
	public CountNumcontext(string Strtype,string ssfgs)
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
        switch (Strtype)
        {
            case "质量退还":
                countnum = new CountNumzlt();
                break ;
            case "调换货":
                countnum = new CountNumthh();
                break ;
            default :
                break ;
        }

        Ssbsc = ssfgs;
	}


    public Hashtable  GetResul()
    {
        //Hashtable ht = new Hashtable();
        //if (countnum != null)
        //{
           return countnum.GetNum(Ssbsc);

        //}
        //return ht;
    }

     
}