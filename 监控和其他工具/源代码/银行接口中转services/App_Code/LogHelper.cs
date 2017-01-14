using System;
using System.Collections.Generic;
using System.IO;
using System.Web;

/// <summary>
/// LogHelper 的摘要说明
/// </summary>
public abstract class LogHelper
{
    /// <summary>
    /// 撰写TXT日志（仅试用Web版）
    /// </summary>
    /// <param name="Dir">要放入的文件夹</param>
    /// <param name="Log">日志内容</param>
    /// <param name="PathType">Dir路径类型（true是绝对路径，false是相对路径）</param>
    /// <param name="isEx">是否为异常（是记录的正确日志还是异常日志）</param>
    public static void Log_Wirte(string Dir, string Log, bool PathType, bool isEx)
    {
        if (!PathType)
            Dir = HttpContext.Current.Server.MapPath(Dir);

        if (Dir.Substring(Dir.Length - 1, 1) != "\\")
            Dir = Dir + "\\";//sometime will be wrong if we don't do this.

        if (!IsExistDirectory(Dir))
            Directory.CreateDirectory(Dir);

        string LogFileName = "";

        if (isEx)
            LogFileName = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fff") + "_异常";
        else
            LogFileName = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fff");

        if (!IsExistFile(Dir + "\\" + LogFileName + ".txt"))
        {
            FileStream FI = File.Create(Dir + "\\" + LogFileName + ".txt");
            FI.Close();
            FI.Dispose();
        }

        using (StreamWriter sw = new StreamWriter(Dir + "\\" + LogFileName + ".txt"))
            sw.Write(Log);
    }

    /// <summary>
    /// 撰写TXT日志（仅试用Web版）
    /// </summary>
    /// <param name="Dir">要放入的文件夹（绝对/相对）</param>
    /// <param name="Log">日志内容</param>
    /// <param name="LogFlag">写入日志文件的附加名称（不可用特殊字符用作文件名，否则报FileIO异常）</param>
    /// <param name="PathType">Dir路径类型（true是绝对路径，false是相对路径）</param>
    /// <param name="isEx">是否为异常（是记录的正确日志还是异常日志）</param>
    public static void Log_Wirte(string Dir, string Log,string LogFlag,bool PathType, bool isEx)
    {
        if (!PathType)
            Dir = HttpContext.Current.Server.MapPath(Dir);

        if (Dir.Substring(Dir.Length - 1, 1) != "\\")
            Dir = Dir + "\\";//sometime will be wrong if we don't do this.

        if (!IsExistDirectory(Dir))
            Directory.CreateDirectory(Dir);

        string LogFileName = "";

        if (isEx)
            LogFileName = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fff") + "_" + LogFlag + "_异常";
        else
            LogFileName = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fff") + "_" + LogFlag;

        if (!IsExistFile(Dir + "\\" + LogFileName + ".txt"))
        {
            FileStream FI = File.Create(Dir + "\\" + LogFileName + ".txt");
            FI.Close();
            FI.Dispose();
        }

        using (StreamWriter sw = new StreamWriter(Dir + "\\" + LogFileName + ".txt"))
            sw.Write(Log);
    }



    /// <summary>
    /// 检测指定文件是否存在,如果存在则返回true。
    /// </summary>
    /// <param name="filePath">文件的绝对路径</param>        
    public static bool IsExistFile(string filePath)
    {
        return File.Exists(filePath);
    }

    /// <summary>
    /// 检测指定目录是否存在
    /// </summary>
    /// <param name="directoryPath">目录的绝对路径</param>
    /// <returns></returns>
    public static bool IsExistDirectory(string directoryPath)
    {
        return Directory.Exists(directoryPath);
    }
}