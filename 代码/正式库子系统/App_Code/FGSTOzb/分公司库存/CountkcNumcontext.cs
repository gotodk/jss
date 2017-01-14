using System;
using System.Collections.Generic;
using System.Web;
using System.Collections;

/// <summary>
///CountkcNumcontext 的摘要说明
/// </summary>
public class CountkcNumcontext
{
    Countkcnum cknum = null;
    string Ssfgs;
    string PH;
    bool isGetlist;

    /// <summary>
    /// 获取分公司库存信息
    /// </summary>
    /// <param name="Type">处理类型：分公司库存</param>
    /// <param name="ssbsc">所属办事处</param>
    /// <param name="PH">批号</param>
    /// <param name="isgetlist">是否返回进货单列表(true/false)</param>
    /// <returns>返回HashTable,包含两个键值“库存数量”，“进货单列表”（可选）</returns>
	public CountkcNumcontext(string Type ,string ssfgs,string ph,bool isgetlist)
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
        switch(Type)
        {
            case  "分公司库存":
            cknum = new Countfgskcth();
            break;
        }

        Ssfgs = ssfgs;
        PH = ph;
        isGetlist = isgetlist;
	}

    public Hashtable GetResult()
    {
        return cknum.GetNum(Ssfgs,PH,isGetlist);
    }
}