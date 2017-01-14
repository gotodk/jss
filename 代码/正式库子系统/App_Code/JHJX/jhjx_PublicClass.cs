using System;
using System.Collections.Generic;
using System.Web;
using Hesion.Brick.Core.WorkFlow;

/// <summary>
///jhjx_PublicClass 的摘要说明
/// </summary>
public static class jhjx_PublicClass
{
	

     /// <summary>
    /// 通过模块名和开头标记字母，获得下一个编号
    /// </summary>
    /// <param name="ModuleName"></param>
    /// <param name="HeadStr"></param>
    /// <returns></returns>
    public static string GetNextNumberZZ(string ModuleName, string HeadStr)
    {
        WorkFlowModule WFM = new WorkFlowModule(ModuleName);
        return WFM.numberFormat.GetNextNumberZZ(HeadStr);
    }
}