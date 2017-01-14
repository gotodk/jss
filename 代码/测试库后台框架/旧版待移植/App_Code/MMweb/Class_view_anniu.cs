using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FMOP.DB;
using System.Data;
using System.Data.SqlClient;
using Hesion.Brick.Core;
using Key;
using System.Collections.Generic;
using Hesion.Brick.Core.WorkFlow;
using System.Collections;
using System.Text.RegularExpressions;

/// <summary>
///Class_view_anniu 的摘要说明
/// </summary>
public class Class_view_anniu
{

    /// <summary>
    /// 验证是否纯数字
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool IsOnlyNumber(string value)
    {
        Regex r = new Regex(@"^[0-9]+$");

        return r.Match(value).Success;
    }
    /// <summary>
    /// 生成角色和内容等各种用户数据对照表
    /// </summary>
    public static DataTable Get_dt()
    {
        //定义表行列
        DataTable dt = new DataTable("对照表");
        DataColumn dc;
        DataRow dr;

        //定义编号列
        dc = new DataColumn();
        dc.DataType = System.Type.GetType("System.String");
        dc.ColumnName = "编号";
        dc.Caption = "编号";
        dc.ReadOnly = true;
        dc.Unique = true;
        //将列添加到表
        dt.Columns.Add(dc);

        //数据中文名
        dc = new DataColumn();
        dc.DataType = System.Type.GetType("System.String");
        dc.ColumnName = "数据中文名";
        dc.Caption = "数据中文名";
        dc.ReadOnly = true;
        dc.Unique = false;
        dc.AutoIncrement = false;
        //将列添加到表
        dt.Columns.Add(dc);

        //权限值
        dc = new DataColumn();
        dc.DataType = System.Type.GetType("System.String");
        dc.ColumnName = "权限值";
        dc.Caption = "权限值";
        dc.ReadOnly = true;
        dc.Unique = false;
        dc.AutoIncrement = false;
        //将列添加到表
        dt.Columns.Add(dc);

        //显示名
        dc = new DataColumn();
        dc.DataType = System.Type.GetType("System.String");
        dc.ColumnName = "显示名";
        dc.Caption = "显示名";
        dc.ReadOnly = true;
        dc.Unique = false;
        dc.AutoIncrement = false;
        //将列添加到表
        dt.Columns.Add(dc);


        //定义主键
        DataColumn[] PrimaryKeyColumns = new DataColumn[1];
        PrimaryKeyColumns[0] = dt.Columns["编号"];
        dt.PrimaryKey = PrimaryKeyColumns;


        //添加数据
        dr = dt.NewRow();
        dr["编号"] = "1";
        dr["数据中文名"] = "卖家超市商品";
        dr["权限值"] = "0";
        dr["显示名"] = "卖家超市商品";
        dt.Rows.Add(dr);
        dr = dt.NewRow();
        dr["编号"] = "2";
        dr["数据中文名"] = "买家超市需求";
        dr["权限值"] = "0";
        dr["显示名"] = "买家超市需求";
        dt.Rows.Add(dr);
        dr = dt.NewRow();
        dr["编号"] = "3";
        dr["数据中文名"] = "买家超市";
        dr["权限值"] = ClassQX.QX()["买家超市"].ToString();
        dr["显示名"] = "买家超市";
        dt.Rows.Add(dr);
        dr = dt.NewRow();
        dr["编号"] = "4";
        dr["数据中文名"] = "卖家超市";
        dr["权限值"] = ClassQX.QX()["卖家超市"].ToString();
        dr["显示名"] = "卖家超市";
        dt.Rows.Add(dr);
        dr = dt.NewRow();
        dr["编号"] = "5";
        dr["数据中文名"] = "景点";
        dr["权限值"] = ClassQX.QX()["景点管理部门"].ToString();
        dr["显示名"] = "景点";
        dt.Rows.Add(dr);
        dr = dt.NewRow();
        dr["编号"] = "6";
        dr["数据中文名"] = "景点内商铺";
        dr["权限值"] = ClassQX.QX()["景点内商铺"].ToString();
        dr["显示名"] = "景点内商铺";
        dt.Rows.Add(dr);
        dr = dt.NewRow();
        dr["编号"] = "7";
        dr["数据中文名"] = "社区";
        dr["权限值"] = ClassQX.QX()["社区管理部门"].ToString();
        dr["显示名"] = "社区";
        dt.Rows.Add(dr);
        dr = dt.NewRow();
        dr["编号"] = "8";
        dr["数据中文名"] = "社区内商铺";
        dr["权限值"] = ClassQX.QX()["社区内商铺"].ToString();
        dr["显示名"] = "社区内商铺";
        dt.Rows.Add(dr);
        dr = dt.NewRow();
        dr["编号"] = "9";
        dr["数据中文名"] = "农产品供应";
        dr["权限值"] = "0";
        dr["显示名"] = "农产品供应";
        dt.Rows.Add(dr);
        dr = dt.NewRow();
        dr["编号"] = "10";
        dr["数据中文名"] = "农产品需求";
        dr["权限值"] = "0";
        dr["显示名"] = "农产品需求";
        dt.Rows.Add(dr);




        return dt;


    }

    /// <summary>
    /// 根据编号获取一行对照数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static DataRow Get_val(string id)
    {
        DataTable dt = Get_dt();
        DataRow dr = dt.Rows.Find(id);
        return dr;
    }

    /// <summary>
    /// 检查角色的可访问性
    /// 可以访问的角色的定义：
    ///1.	被访问用户已开通此角色。
    ///2.	被访问用户此角色是否处于正常状态。这里，不同角色，判定方式不同。
    /// </summary>
    /// <param name="MuBiaoKHBH">对方客户编号</param>
    /// <param name="JSXDMC">角色限定名称</param>
    /// <param name="SPstr">特殊参数</param>
    /// <returns></returns>
    public static bool Check_jszt(string MuBiaoKHBH, string JSXDDM, string SPstr)
    {
        //验证角色是否已开通.从数据库获取用户的权限,获取已开通权限
        Int32 need_qx = Convert.ToInt32(Get_val(JSXDDM)["权限值"]);
        Int32 user_qx = 0;
        object ob = DbHelperSQL.GetSingle("select YKTQX from MM_YHZCDLXXB where KHBH = '" + MuBiaoKHBH + "'");
        if (ob == null || ob.ToString().Trim() == "" || !IsOnlyNumber(ob.ToString()))
        {
            user_qx = 0;
        }
        else
        {
            user_qx = Convert.ToInt32(ob.ToString());
        }
        if ((user_qx & need_qx) != need_qx)
        {
            //没有权限
            return false;
        }

        //进行可访问性的进一步验证
        object obj_mubiao = null;
        switch (JSXDDM)
        {
            case "3":
                //关闭或者冻结等异常   买家超市";
                obj_mubiao = get_GZNRBT("SELECT count(*) FROM MM_BuyCSJBXXB WHERE YHKHBH = '" + MuBiaoKHBH + "' and SFDJ = '否' and  CSKQZT = '开启'");
                if( Convert.ToInt32(obj_mubiao) < 1)
                {
                    return false;
                }

                break;
            case "4":
                //关闭或者冻结等异常   卖家超市";
                obj_mubiao = get_GZNRBT("SELECT count(*) FROM MM_MJCSJBXXB WHERE YHKHBH = '" + MuBiaoKHBH + "' and SFDJ = '否' and  CSKQZT = '开启'");
                if( Convert.ToInt32(obj_mubiao) < 1)
                {
                    return false;
                }
          
                break;
            case "5":
                //关闭或者冻结等异常   景点";
                obj_mubiao = get_GZNRBT("SELECT count(*) FROM MM_JDGLJSYJDJBXXB WHERE YHKHBH = '" + MuBiaoKHBH + "' and SFDJ = '否'");
                if( Convert.ToInt32(obj_mubiao) < 1)
                {
                    return false;
                }
                break;
            case "6":
                //关闭或者冻结等异常   景点内商铺";
                obj_mubiao = get_GZNRBT("SELECT count(*) FROM MM_JDNSPJBXXB WHERE YHKHBH = '" + MuBiaoKHBH + "' and SFDJ = '否'");
                if( Convert.ToInt32(obj_mubiao) < 1)
                {
                    return false;
                }
                break;
            case "7":
                //关闭或者冻结等异常   社区";
                obj_mubiao = get_GZNRBT("SELECT count(*) FROM MM_SQGLJSJBXXB WHERE YHKHBH = '" + MuBiaoKHBH + "' and SFDJ = '否'");
                if( Convert.ToInt32(obj_mubiao) < 1)
                {
                    return false;
                }
                break;
            case "8":
                //关闭或者冻结等异常   社区内商铺";
                obj_mubiao = get_GZNRBT("SELECT count(*) FROM MM_SQNSPJBXXB WHERE YHKHBH = '" + MuBiaoKHBH + "' and SFDJ = '否'");
                if( Convert.ToInt32(obj_mubiao) < 1)
                {
                    return false;
                }
        
                break;
            default:
                return false;
                break;
        }

        return true;
    }

    static object get_GZNRBT(string sql)
    {
        object ob = DbHelperSQL.GetSingle(sql);
        if (ob != null) //存在
        {
            return ob;
        }
        else
        {
            return null;
        }
    }
    public Class_view_anniu()
    {
        //
        //TODO: 在此处添加构造函数逻辑
        //
    }
}