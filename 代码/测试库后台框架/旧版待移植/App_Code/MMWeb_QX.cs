using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using FMOP.DB;
/// <summary>
///判断用户是否含有某种权限
///某种权限是否正在审核、未填写资料、未曾勾选
///郭拓
///2012-04-25
/// </summary>
public class MMWeb_QX
{
    /// <summary>
    /// 判断用户是否已经开通某种权限
    /// </summary>
    /// <param name="KHBH">用户客户编号</param>
    /// <param name="IntQX">权限值</param>
    /// <returns>True or false</returns>
    public static bool IsHasQX(string KHBH,int IntQX)
    {
        string SelectSql = "SELECT * FROM MM_YHZCDLXXB WHERE KHBH = '"+KHBH+"'";
        DataSet ds = DbHelperSQL.Query(SelectSql);
        DataTable dt = ds.Tables[0];
        bool IsHasQX = false;
        if (dt.Rows.Count > 0)
        {
            string YKTQX = dt.Rows[0]["YKTQX"].ToString(); //获取当前用户的已开通权限
            int IntYKTQX = Convert.ToInt32(YKTQX);
            if ((IntYKTQX & IntQX) != IntQX)
            {
                IsHasQX = false;
            }
            if ((IntYKTQX & IntQX) == IntQX)
            {
                IsHasQX = true;
            }
        }
        return IsHasQX;
    }
    /// <summary>
    /// 判断用户是否已经勾选该权限
    /// </summary>
    /// <param name="KHBH"></param>
    /// <param name="IntQX"></param>
    /// <returns></returns>
    public static bool IsCheckedQX(string KHBH, int IntQX)
    {
        string SelectSql = "SELECT * FROM MM_YHZCDLXXB WHERE KHBH = '" + KHBH + "'";
        DataSet ds = DbHelperSQL.Query(SelectSql);
        DataTable dt = ds.Tables[0];
        bool IsHasQX = false;
        if (dt.Rows.Count > 0)
        {
            string NKTQX = dt.Rows[0]["NKTQX"].ToString(); //获取当前用户的已开通权限
            int IntNKTQX = Convert.ToInt32(NKTQX);
            if ((IntNKTQX & IntQX) != IntQX)
            {
                IsHasQX = false;
            }
            if ((IntNKTQX & IntQX) == IntQX)
            {
                IsHasQX = true;
            }
        }
        return IsHasQX;
    }
    /// <summary>
    /// 判断用户该权限是否已勾选但未填写资料
    /// </summary>
    /// <param name="KHBH"></param>
    /// <param name="IntQX"></param>
    /// <returns></returns>
    public static bool IsCheckedAndSubmitQX(string KHBH, int IntQX)
    {
        
        string SelectSql = "SELECT * FROM MM_YHZCDLXXB WHERE KHBH = '" + KHBH + "'";
        DataSet ds = DbHelperSQL.Query(SelectSql);
        DataTable dt = ds.Tables[0];
        bool IsHasQX = false;
        if (dt.Rows.Count > 0)
        {
            string NKTQX = dt.Rows[0]["NKTQX"].ToString(); //获取当前用户的拟开通权限
            int IntNKTQX = Convert.ToInt32(NKTQX);  //拟开通权限Int
            string YKTQX = dt.Rows[0]["YKTQX"].ToString();//获取当前用户的已开通权限
            int IntYKTQX = Convert.ToInt32(YKTQX);  //已开通权限Int
            string ZZSHZDQX = dt.Rows[0]["ZZSHZDQX"].ToString(); //获取当前用户正在审核中的权限
            int IntZZSHZDQX = Convert.ToInt32(ZZSHZDQX); //正在审核中的权限Int
            if (IntZZSHZDQX == 0)
            {
                if (((IntNKTQX - IntYKTQX) & IntQX) == IntQX)
                {
                    IsHasQX = true; //表示这种权限已经勾选，但没有填写资料
                }
            }
            if (IntZZSHZDQX != 0)
            {
                if (((IntNKTQX - IntYKTQX - IntZZSHZDQX) & IntQX) == IntQX)
                {
                    IsHasQX = true;//表示这种权限已经勾选，但没有填写资料
                }
            }
        }
        return IsHasQX;
    }
    /// <summary>
    /// 判断某种权限是不是正在审核中
    /// </summary>
    /// <param name="KHBH"></param>
    /// <param name="IntQX"></param>
    /// <returns></returns>
    public static bool IsExamQX(string KHBH, int IntQX)
    {
        string SelectSql = "SELECT * FROM MM_YHZCDLXXB WHERE KHBH = '" + KHBH + "'";
        DataSet ds = DbHelperSQL.Query(SelectSql);
        DataTable dt = ds.Tables[0];
        bool IsExamQX = false;
        if (dt.Rows.Count > 0)
        {
            string ZZSHZDQX = dt.Rows[0]["ZZSHZDQX"].ToString(); //获取当前用户的已开通权限
            int IntZZSHZDQX = Convert.ToInt32(ZZSHZDQX);
            if ((IntZZSHZDQX & IntQX) != IntQX)
            {
                IsExamQX = false;
            }
            if ((IntZZSHZDQX & IntQX) == IntQX)
            {
                IsExamQX = true;
            }
        }
        return IsExamQX;
    }
}