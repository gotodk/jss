using System;
using System.Collections.Generic;
using System.Web;
using System.Collections;
using FMOP.DB;
using Hesion.Brick.Core.WorkFlow;
using System.Data;

/// <summary>
///Manage_Address 的摘要说明
/// </summary>
public class Manage_Address
{
    //public Manage_Address()
    //{
    //    //
    //    //TODO: 在此处添加构造函数逻辑
    //    //
    //}

    //设置默认收货地址
    public string Set_MrAddress(string number,string dlyx)
    {
        ArrayList al = new ArrayList();
        string sql_update1 = "update ZZ_SHDZXXB set SFMRDZ='否' where SFMRDZ='是' and JSDLZH='" + dlyx + "'";
        al.Add(sql_update1);
        string sql_update2 = "update ZZ_SHDZXXB set SFMRDZ='是' where number='" + number + "' and JSDLZH='" + dlyx + "'";
        al.Add(sql_update2);
        try
        {
            DbHelperSQL.ExecSqlTran(al);
            return "ok";
        }
        catch
        {
            return "失败";
        }
    }

    //删除收货地址
    public string Del_Address(string number, string dlyx)
    {
        string sql_del = "Delete ZZ_SHDZXXB where number='" + number + "' and JSDLZH='" + dlyx + "'";

        int count = DbHelperSQL.ExecuteSql(sql_del);
        if (count > 0)
        {
            return "ok";
        }
        else
        {
            return "失败";
        }
    }

    //保存收货地址
    public string[] Save_Address(string[][] InputStr)
    {
        List<string> al = new List<string>();
        string[] result = new string[2];

        //将参数数组转换回哈希表
        Hashtable InputHT = new Hashtable();

        for (int i = 0; i < InputStr.Length; i++)
        {
            InputHT.Add(InputStr[i][0].ToString(), InputStr[i][1].ToString());
        }

        //获取角色编号的相关信息       
        Hashtable UserInfo = jhjx_PublicClass.GetUserInfo(InputHT["jsbh"].ToString());

        if (UserInfo["是否审核通过"].ToString() == "否" || UserInfo["是否审核通过"].ToString() == "")
        {
            result[0] = "失败";
            result[1] = "尚未开通结算账户，无法添加收货地址！";
        }
        else
        {
            //先更新原来的默认地址
            if (InputHT["MRDZ"].ToString() == "是")
            {
                string sql_updateMRDZ = "update ZZ_SHDZXXB set SFMRDZ='否' where SFMRDZ='是' and JSDLZH='" + InputHT["dlyx"].ToString() + "'";
                al.Add(sql_updateMRDZ);
            }

            string number = InputHT["Number"].ToString();
            if (number == "")
            {//新增收货地址            

                //获取新增数据的number支
                string KeyNumber = jhjx_PublicClass.GetNextNumberZZ("ZZ_SHDZXXB", "");

                //生成insert语句
                string sql_insert = "insert into ZZ_SHDZXXB([Number],[JSDLZH],[JSYHM],[JSZHLX],[JSBH],[JSLX],[SHDWMC],[SHRXM],[SZSF],[SZDS],[SZQX],[XXDZ],[YZBM],[LXDH],[SFMRDZ],[CheckState],[CreateUser],[CreateTime]) values ('" + KeyNumber + "','" + InputHT["dlyx"].ToString() + "','" + UserInfo["用户名"].ToString() + "','" + UserInfo["结算账户类型"].ToString() + "','" + InputHT["jsbh"].ToString() + "','" + UserInfo["角色类型"].ToString() + "','" + InputHT["SHDWMC"].ToString() + "','" + InputHT["SHRXM"].ToString() + "','" + InputHT["sheng"].ToString() + "','" + InputHT["shi"].ToString() + "','" + InputHT["quxian"].ToString() + "','" + InputHT["XXDZ"].ToString() + "','" + InputHT["YZBM"].ToString() + "','" + InputHT["LXDH"].ToString() + "','" + InputHT["MRDZ"].ToString() + "',1,'" + InputHT["jsbh"].ToString() + "',getdate())";
                al.Add(sql_insert);
            }
            else
            { //修改收货地址
                //生成update语句
                string sql_UpdateDZ = "update ZZ_SHDZXXB set [SHDWMC]='" + InputHT["SHDWMC"].ToString() + "',[SHRXM]='" + InputHT["SHRXM"].ToString() + "',[SZSF]='" + InputHT["sheng"].ToString() + "',[SZDS]='" + InputHT["shi"].ToString() + "',[SZQX]='" + InputHT["quxian"].ToString() + "',[XXDZ]='" + InputHT["XXDZ"].ToString() + "',[YZBM]='" + InputHT["YZBM"].ToString() + "',[LXDH]='" + InputHT["LXDH"].ToString() + "',[SFMRDZ]='" + InputHT["MRDZ"].ToString() + "',[CreateTime]=getdate() where Number='" + InputHT["Number"].ToString() + "' and JSDLZH='" + InputHT["dlyx"].ToString() + "'";
                al.Add(sql_UpdateDZ);
            }          

            int count = DbHelperSQL.ExecuteSqlTran(al);
            if (count > 0)
            {
                result[0] = "ok";
                result[1] = "收货地址保存成功！";
            }
            else
            {
                result[0] = "失败";
                result[1] = "收货地址保存失败！";
            }          
        }
        return result;
    }

    //设置默认收货地址
    public DataSet  Get_EditAddress(string number, string dlyx)
    {
        DataSet ds = DbHelperSQL.Query("select '' as 执行状态,'' as 执行结果, * from ZZ_SHDZXXB where number='" + number + "' and JSDLZH='" + dlyx + "'");
        if (ds != null && ds.Tables[0].Rows.Count == 1)
        {
            ds.Tables[0].Rows[0]["执行状态"] = "ok";
            ds.Tables[0].Rows[0]["执行结果"] = "ok";
        }
        else
        {
            ds.Tables[0].Rows[0]["执行状态"] = "失败";
            ds.Tables[0].Rows[0]["执行结果"] = "未获得待修改的地址信息！";
        }
        return ds;
    }

}