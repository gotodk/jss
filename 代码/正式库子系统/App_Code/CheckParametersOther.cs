using System;
using System.Collections;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Text;
using FMOP.SD;
using FMOP.DB;
/// <summary>
///galaxy 4-14
/// </summary>
public class CheckParametersOther
{
	public CheckParametersOther()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
     
	}

    /// <summary>
    /// 自定义验证特殊字段,用于主表
    /// </summary>
    /// <param name="paramArray">要验证的字段集合</param>
    /// <param name="modname">模块名</param>
    /// <returns></returns>
    public static ArrayList myCheckSpecial(SaveDepositary[] paramArray, string modname)
    {
        ArrayList AL = new ArrayList();
        AL.Add("");
        AL.Add("");

        //对表单元素进行特殊验证
        //《合同使用登记表》验证“合同编号”唯一性，防止添加重复记录:刘杰 2009-04-14 注:就是现在的<合同领用登记表>
        if (modname == "HTSYDJB")
        {
            string HTBH = paramArray[1].Text;
            //连数据库验证
           DataSet  ds=DbHelperSQL.Query("select * from HTSYDJB where HTBH='"+HTBH+"'");
         if (ds != null && ds.Tables != null)  //存在记录集
         {
             if (ds.Tables[0].Rows.Count>0)   //如果存在编号
            {
             AL[0] = "不通过";
             AL[1] = "合同编号不能重复！";
             return AL;
            }
            else
            {
            AL[0] = "通过";
            AL[1] = "不提示";
            return AL;  
            }
         }          
        }
        //《运单信息管理》运单单号唯一控制 liujie 2010-02-21
        if (modname == "YDXXGL")
        {
            string YDH = paramArray[0].Text; //这个地方很奇怪，为什么调用0正确，而不是1
            //连数据库验证
            DataSet ds = DbHelperSQL.Query("select * from YDXXGL where YDH='" +YDH+ "'");
            if (ds != null && ds.Tables != null)  //存在记录集
            {
                if (ds.Tables[0].Rows.Count > 0)   //如果存在编号
                {
                    AL[0] = "不通过";
                    AL[1] = "运单编号不能重复！";
                    return AL;
                }
                else
                {
                    AL[0] = "通过";
                    AL[1] = "不提示";
                    return AL;
                }
            }
        }

        //对<录入销售单>中的订单单号进行唯一性验证.刘杰 2009-0５-１２ 
        if (modname == "LRXSD")
        {
            string DDDH = paramArray[1].Text;
            //连数据库验证
            DataSet ds = DbHelperSQL.Query("select * from LRXSD where DDDH='" + DDDH + "'");
            if (ds != null && ds.Tables != null)  //存在记录集
            {
                if (ds.Tables[0].Rows.Count > 0)   //如果存在编号
                {
                    AL[0] = "不通过";
                    AL[1] = "该订单已填写过销售单,不能再添加！";
                    return AL;
                }
                else
                {
                    AL[0] = "通过";
                    AL[1] = "不提示";
                    return AL;
                }
            }
        }
        //
        //对表单元素进行特殊验证
        //《城市销售公司月度需求计划》验证“年份”“月份”唯一性，防止添加重复记录
        if (modname == "XSGS_YDXQJHB")
        {
            int year = Convert.ToInt32(paramArray[0].Text);
            int  month = Convert.ToInt32(paramArray[1].Text);
            string city = paramArray[2].Text;
            //连数据库验证
            DataSet ds = DbHelperSQL.Query("select * from XSGS_YDXQJHB where NF=" + year + " and YF = " + month + "  and CSXSGS like '%"+city+"%'");
            if (ds != null && ds.Tables != null)  //存在记录集
            {
                if (ds.Tables[0].Rows.Count > 0)   //如果存在编号
                {
                    AL[0] = "不通过";
                    AL[1] = "该年份月份的需求计划表已经存在，请不要重复输入！";
                    return AL;
                }
                else
                {
                    AL[0] = "通过";
                    AL[1] = "不提示";
                    return AL;
                }
            }
        }
        //2009-5-8《城市销售公司月度销售回款计划》验证“年份”“月份”唯一性，防止添加重复记录
        if (modname == "XSGS_YDXSHKJH")
        {
            int year = Convert.ToInt32(paramArray[0].Text);
            int  month = Convert.ToInt32(paramArray[1].Text);
            string city = paramArray[2].Text;
            //连数据库验证
            DataSet ds = DbHelperSQL.Query("select * from XSGS_YDXSHKJH where NF=" + year + " and YF = " + month + "  and CSXSGS like '%"+city+"%'");
            if (ds != null && ds.Tables != null)  //存在记录集
            {
                if (ds.Tables[0].Rows.Count > 0)   //如果存在编号
                {
                    AL[0] = "不通过";
                    AL[1] = "该年份月份的回款计划已经存在，请不要重复输入！";
                    return AL;
                }
                else
                {
                    AL[0] = "通过";
                    AL[1] = "不提示";
                    return AL;
                }
            }
        }


        //验证 城市销售公司要货申请单 单号不重复 
        if (modname == "CitySaleCenter_SApp")
        {
            string YHSQDH = paramArray[0].Text.ToString();
            DataSet ds = DbHelperSQL.Query("select * from CitySaleCenter_SApp where YHSQDH = '" + YHSQDH + "'");
            if (ds != null && ds.Tables[0] != null)
            {
                if (ds.Tables[0].Rows.Count > 0)   //如果存在编号
                {
                    AL[0] = "不通过";
                    AL[1] = "此要货申请单号已经存在，请不要重复输入！";
                    return AL;
                }
                else
                {
                    AL[0] = "通过";
                    AL[1] = "不提示";
                    return AL;
                }
            }
          
        }



        //验证生成总部发货单发货单号不重复
        if (modname == "CreateZongbuFHD")
        {
            string FHDH = paramArray[0].Text.ToString();
            DataSet ds = DbHelperSQL.Query("select * from CreateZongbuFHD where FHDH = '" + FHDH + "'");

            if (ds != null && ds.Tables[0] != null)
            {
                if (ds.Tables[0].Rows.Count > 0)   //如果存在编号
                {
                    AL[0] = "不通过";
                    AL[1] = "此要货申请单号已经存在，请不要重复输入！";
                    return AL;
                }
                else
                {
                    AL[0] = "通过";
                    AL[1] = "不提示";
                    return AL;
                }
            }

        }

        //2009.06.08 王永辉  添加 服务中心模块 员工重复输入验证
        if (modname == "RightFenpei")
        {
            string YGBH = paramArray[0].Text.ToString();

            DataSet ds = DbHelperSQL.Query("select * from RightFenpei where YGBH = '" + YGBH + "'");

            if (ds != null && ds.Tables[0] != null)
            {
                if (ds.Tables[0].Rows.Count > 0)   //如果存在编号
                {
                    AL[0] = "不通过";
                    AL[1] = "此用户已经存在，请不要重复输入！";
                    return AL;
                }
                else
                {
                    AL[0] = "通过";
                    AL[1] = "不提示";
                    return AL;
                }
            }
        }
       //2010.07-28 liujie 添加 “部门电脑资产”
        //if (modname == "BMDNZCXX")
        //{
        //    string BM = paramArray[0].Text; //这个地方很奇怪，为什么调用0正确，而不是1
        //    //连数据库验证
        //    DataSet ds = DbHelperSQL.Query("select * from BMDNZCXX where BM='" + BM + "'");
        //    if (ds != null && ds.Tables != null)  //存在记录集
        //    {
        //        if (ds.Tables[0].Rows.Count > 0)   //如果存在编号
        //        {
        //            AL[0] = "不通过";
        //            AL[1] = "部门名称不能重复！";
        //            return AL;
        //        }
        //        else
        //        {
        //            AL[0] = "通过";
        //            AL[1] = "不提示";
        //            return AL;
        //        }
        //    }
        //}
        //刘杰 前处理管理
        if (modname == "QCLGL")
        {
            string SCTZD = paramArray[0].Text; //这个地方很奇怪，为什么调用0正确，而不是1
            //连数据库验证
            DataSet ds = DbHelperSQL.Query("select * from QCLGL where SCTZD='" + SCTZD + "'");
            if (ds != null && ds.Tables != null)  //存在记录集
            {
                if (ds.Tables[0].Rows.Count > 0)   //如果存在编号
                {
                    AL[0] = "不通过";
                    AL[1] = "试产通知单不能重复！";
                    return AL;
                }
                else
                {
                    AL[0] = "通过";
                    AL[1] = "不提示";
                    return AL;
                }
            }
        }
        //刘杰 装配管理
        if (modname == "ZPGL")
        {
            string SCTZD = paramArray[0].Text; //这个地方很奇怪，为什么调用0正确，而不是1
            string FGZPJHBH= paramArray[1].Text;
            string GDDH = paramArray[2].Text;
            //连数据库验证
            DataSet ds = DbHelperSQL.Query("select * from ZPGL where SCTZD='" + SCTZD + "' and FGZPJHBH='"+FGZPJHBH+"' and GDDH='"+GDDH+"'");
            if (ds != null && ds.Tables != null)  //存在记录集
            {
                if (ds.Tables[0].Rows.Count > 0)   //如果存在编号
                {
                    AL[0] = "不通过";
                    AL[1] = "试产通知单和返工装配计划单和工单单号不能重复！";
                    return AL;
                }
                else
                {
                    AL[0] = "通过";
                    AL[1] = "不提示";
                    return AL;
                }
            }
        }
        //刘杰 录入试产情况 2010-08-31
        if (modname == "LRSCQK")
        {
            string ZPJHBH = paramArray[0].Text;
            string sql = "select * from " + modname.Trim() + " where ZPJHBH='" + ZPJHBH + "'";
            DataSet ds = DbHelperSQL.Query(sql.Trim());
            if (ds != null && ds.Tables != null)  //存在记录集
            {
                if (ds.Tables[0].Rows.Count > 0)   //如果存在编号
                {
                    AL[0] = "不通过";
                    AL[1] = "装配计划编号不能重复！";
                    return AL;
                }
                else
                {
                    AL[0] = "通过";
                    AL[1] = "不提示";
                    return AL;
                }
            }

        }
        //刘杰 品管检验
        if (modname == "PGJY")
        {
            string SCBH= paramArray[0].Text; //这个地方很奇怪，为什么调用0正确，而不是1
            string sql = "select * from " + modname.Trim() + " where SCBH='" +SCBH+ "'";
            DataSet ds = DbHelperSQL.Query(sql.Trim());
            if (ds != null && ds.Tables != null)  //存在记录集
            {
                if (ds.Tables[0].Rows.Count > 0)   //如果存在编号
                {
                    AL[0] = "不通过";
                    AL[1] = "试产编号不能重复！";
                    return AL;
                }
                else
                {
                    AL[0] = "通过";
                    AL[1] = "不提示";
                    return AL;
                }
            }

        }
        //刘杰 生产技术结论
        if (modname == "SCJSJL")
        {
            string PGJYD= paramArray[0].Text; //这个地方很奇怪，为什么调用0正确，而不是1
            string sql = "select * from " + modname.Trim() + " where PGJYD='" +PGJYD+ "'";
            DataSet ds = DbHelperSQL.Query(sql.Trim());
            if (ds != null && ds.Tables != null)  //存在记录集
            {
                if (ds.Tables[0].Rows.Count > 0)   //如果存在编号
                {
                    AL[0] = "不通过";
                    AL[1] = "品管检验单不能重复！";
                    return AL;
                }
                else
                {
                    AL[0] = "通过";
                    AL[1] = "不提示";
                    return AL;
                }
            }

        }
        //刘杰 生产技术结论
        if (modname == "SCJSJL")
        {
            string PGJYD = paramArray[0].Text; //这个地方很奇怪，为什么调用0正确，而不是1
            string sql = "select * from " + modname.Trim() + " where PGJYD='" + PGJYD + "'";
            DataSet ds = DbHelperSQL.Query(sql.Trim());
            if (ds != null && ds.Tables != null)  //存在记录集
            {
                if (ds.Tables[0].Rows.Count > 0)   //如果存在编号
                {
                    AL[0] = "不通过";
                    AL[1] = "品管检验单不能重复！";
                    return AL;
                }
                else
                {
                    AL[0] = "通过";
                    AL[1] = "不提示";
                    return AL;
                }
            }

        }
        if (modname == "PHGFQJGL")
        {
            string PH= paramArray[0].Text; //这个地方很奇怪，为什么调用0正确，而不是1
            string sql = "select * from " + modname.Trim() + " where PH='" +PH+ "'";
            DataSet ds = DbHelperSQL.Query(sql.Trim());
            if (ds != null && ds.Tables != null)  //存在记录集
            {
                if (ds.Tables[0].Rows.Count > 0)   //如果存在编号
                {
                    AL[0] = "不通过";
                    AL[1] = "品号重复！";
                    return AL;
                }
                else
                {
                    AL[0] = "通过";
                    AL[1] = "不提示";
                    return AL;
                }
            }

        }
        if (modname == "PHCCXXWH") //品号仓储信息维护。
        {
            string PH = paramArray[0].Text; //这个地方很奇怪，为什么调用0正确，而不是1
            string sql = "select * from " + modname.Trim() + " where PH='" + PH + "'";
            DataSet ds = DbHelperSQL.Query(sql.Trim());
            if (ds != null && ds.Tables != null)  //存在记录集
            {
                if (ds.Tables[0].Rows.Count > 0)   //如果存在编号
                {
                    AL[0] = "不通过";
                    AL[1] = "品号重复！";
                    return AL;
                }
                else
                {
                    AL[0] = "通过";
                    AL[1] = "不提示";
                    return AL;
                }
            }

        }
        if (modname == "BSCAQKCCS") //办事处安全库存参数。
        {
            string BSC= paramArray[0].Text; //这个地方很奇怪，为什么调用0正确，而不是1
            string sql = "select * from " + modname.Trim() + " where BSC='" + BSC+ "'";
            DataSet ds = DbHelperSQL.Query(sql.Trim());
            if (ds != null && ds.Tables != null)  //存在记录集
            {
                if (ds.Tables[0].Rows.Count > 0)   //如果存在编号
                {
                    AL[0] = "不通过";
                    AL[1] = "办事处重复！";
                    return AL;
                }
                else
                {
                    AL[0] = "通过";
                    AL[1] = "不提示";
                    return AL;
                }
            }

        }

        AL[0] = "通过";
        AL[1] = "不提示";
        return AL;
    }
      
}
