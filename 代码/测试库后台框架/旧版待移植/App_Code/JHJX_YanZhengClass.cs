using FMOP.DB;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// JHJX_YanZhengClass 的摘要说明
/// </summary>
public  class JHJX_YanZhengClass
{
	public JHJX_YanZhengClass()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}

    /// <summary>
    /// 是否提交开通申请
    /// </summary>
    /// <param name="StrYX"></param>
    /// <returns></returns>
    public static Hashtable SFTJKTSQ(Hashtable ht)
    {
        Hashtable HTUserInfo = new Hashtable();
        HTUserInfo["info"] = "查询用户信息时出错。";
        HTUserInfo["第三方存管状态是否开通"] = "否";
        HTUserInfo["是否提交开通申请"] = "否";
        HTUserInfo["开通交易账户审核状态"] = "审核通过";//驳回或者审核中或者审核通过
        //HTUserInfo["开通交易账户是否驳回"] = "否";
        HTUserInfo["交易资金密码正确"] = "否";
        HTUserInfo["当前账户是否冻结出金"] = "是";
        
        HTUserInfo["是否休眠"] = "是";
        HTUserInfo["是否交易时间内"] = "否";
        HTUserInfo["是否交易日期内"] = "否";
        HTUserInfo["被冻结项"] = "";
        HTUserInfo["是否存在两次出金失败未处理记录"] = "是";

        if (ht == null)
        {
            return HTUserInfo;
        }
        string StrSql = "select JJRSHZT,FGSSHZT,B_SFDJ,B_SFXM, '当前默认经纪人是否被冻结'=isnull((select top 1 SubL.B_SFDJ from AAA_DLZHXXB as SubL where SubL.B_DLYX=G.GLJJRDLZH ),'否'),B_DLYX,B_JSZHLX,I_DSFCGZT,DLYX,B_JSZHMM,SFYX,DJGNX from AAA_DLZHXXB as L left join AAA_MJMJJYZHYJJRZHGLB as G on L.B_DLYX = G.DLYX where B_DLYX = '" + ht["DLYX"].ToString() + "' ";
        DataSet ds = DbHelperSQL.Query(StrSql);
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            HTUserInfo["info"] = "ok";
            //开通
            if (dr["DLYX"] != null && !dr["DLYX"].ToString().Equals("") && dr["SFYX"] != null && dr["SFYX"].ToString().Equals("是"))
            {
                HTUserInfo["是否提交开通申请"] = "是";
            }
            if (dr["I_DSFCGZT"].ToString().Equals("开通"))
            {
                HTUserInfo["第三方存管状态是否开通"] = "是";
            }
            if (dr["JJRSHZT"].ToString().Equals("审核中") || dr["FGSSHZT"].ToString().Equals("审核中"))
            {
                HTUserInfo["开通交易账户审核状态"] = "审核中";
            }
            if (dr["JJRSHZT"].ToString().Equals("驳回") || dr["FGSSHZT"].ToString().Equals("驳回"))
            {
                HTUserInfo["开通交易账户审核状态"] = "驳回";
            }

            if (ht["JYZJMM"] != null && !ht["JYZJMM"].ToString().Equals(""))
            {
                if (ht["JYZJMM"].ToString().Equals(dr["B_JSZHMM"].ToString().Trim()))
                {
                    HTUserInfo["交易资金密码正确"] = "是";
                }
            }

            if (!dr["DJGNX"].ToString().Trim().Contains("出金"))
            {
                HTUserInfo["当前账户是否冻结出金"] = "否";//dr["B_SFDJ"].ToString().Trim();
            }


            HTUserInfo["被冻结项"] = dr["B_SFDJ"].ToString().Trim();
           
            HTUserInfo["是否休眠"] = dr["B_SFXM"].ToString().Trim();

            Hashtable JCYZ = PublicClass2013.GetParameterInfo();
            HTUserInfo["是否交易时间内"] = JCYZ["是否在服务时间内(工作时)"].ToString();
            HTUserInfo["是否交易日期内"] = JCYZ["是否在服务时间内(假期)"].ToString();

            //查看是否存在两次出金失败记录
            string Strchujin = "select COUNT(Number) from AAA_CRJYCSJB where DLYX='" + ht["DLYX"].ToString() + "'"; //未添加有效条件
            object obj = DbHelperSQL.GetSingle(Strchujin);
            if (int.Parse(obj.ToString().Trim()) < 2)
            {
                HTUserInfo["是否存在两次出金失败未处理记录"] = "否";
            }

        }

        return HTUserInfo;
    }

    
}