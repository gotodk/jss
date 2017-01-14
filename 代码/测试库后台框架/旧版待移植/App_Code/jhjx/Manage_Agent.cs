using System;
using System.Collections.Generic;
using System.Web;
using System.Collections;
using FMOP.DB;
using Hesion.Brick.Core.WorkFlow;
using System.Data;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
/// <summary>
/// Manage_Agent 的摘要说明 用于卖家买家关联经纪人
/// </summary>
public class Manage_Agent
{	

    /// <summary>
    /// 反序列化数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sXml"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    private T DeSerializer<T>(String sXml, Type type)
    {
        XmlReader reader = XmlReader.Create(new StringReader(sXml));
        XmlSerializer serializer = new XmlSerializer(type);
        object obj = serializer.Deserialize(reader);
        return (T)obj;
    }

    /// <summary>
    /// 将参数转化为哈希表
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    private Hashtable ObjToHash(object obj)
    {
        Hashtable info = new Hashtable();
        DictionaryEntry[] array = DeSerializer<DictionaryEntry[]>(obj.ToString(), typeof(DictionaryEntry[]));

        foreach (DictionaryEntry a in array)
        {
            info.Add(a.Key, a.Value);
        }
        return info;

    }
    /// <summary>
    /// 关联新的经纪人
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public DataSet AgentAdd(object obj)
    {
        DataSet dsinput = (DataSet)obj;
        //初始化返回值结构
        DataSet dsreturn = initReturnDataSet();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "未知错误" });
        List<string> commands = new List<string>();
        //数据检查
        string SellerJSBH = dsinput.Tables["子表"].Rows[0]["JSBH"].ToString();
        Hashtable UserInfo = jhjx_PublicClass.GetUserInfo(SellerJSBH);
        if (SellerJSBH.Trim() == "" || UserInfo["是否审核通过"].ToString() != "审核通过")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["错误提示"] = "您尚未开通结算账户，无法添加经纪人！";
            return dsreturn;
        }
        if (UserInfo["是否允许登录"].ToString() != "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["错误提示"] = "您已被禁止登录，无法添加经纪人！";
            return dsreturn;
        }
        if (!jhjx_PublicClass.IsOpenDay() || !jhjx_PublicClass.IsOpenTime())
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["错误提示"] = "当前时间业务停止运行，无法添加经纪人！";
            return dsreturn;
        }
        if (UserInfo["是否休眠"].ToString() == "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["错误提示"] = "您的账号已休眠，无法添加经纪人！";
            return dsreturn;
        }
        //if (UserInfo["是否冻结"].ToString() == "是")
        //{
        //    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
        //    dsreturn.Tables["返回值单条"].Rows[0]["错误提示"] = "您的账号已被禁止业务，无法添加经纪人！";
        //    return dsreturn;
        //}

        string strNewJJR = "select top 1 * from ZZ_UserLogin a left join ZZ_YHJSXXB c on a.DLYX=c.DLYX  where a.DLYX = '" + dsinput.Tables["子表"].Rows[0]["DLRDLZH"].ToString() + "' and a.SFDJZH = '否' order by a.CreateTime DESC ";
        DataSet dsNewJJR = DbHelperSQL.Query(strNewJJR);
        if (dsNewJJR != null && dsNewJJR.Tables[0].Rows.Count > 0)
        {
            if (dsNewJJR.Tables[0].Rows[0]["FGSKTSHZT"].ToString() != "审核通过")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["错误提示"] = "该经纪人尚未开通结算账户，请输入其他经纪人！";
                return dsreturn;
            }
            if (dsNewJJR.Tables[0].Rows[0]["SFDJZH"].ToString() == "是" || dsNewJJR.Tables[0].Rows[0]["SFXM"].ToString() == "是" || dsNewJJR.Tables[0].Rows[0]["SFYXDL"].ToString() == "否" || dsNewJJR.Tables[0].Rows[0]["SFYZYX"].ToString() == "否" || dsNewJJR.Tables[0].Rows[0]["SFZTXYW"].ToString() == "是")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["错误提示"] = "此经纪人被禁止或暂停业务，无法添加经纪人！";
                return dsreturn;
            }
            else
            {
                //获取新增数据的number值 
                string KeyNumber = jhjx_PublicClass.GetNextNumberZZ("ZZ_MMJYDLRZHGLB", "");
                string sql = "SELECT [DLRZTJSXYH]  FROM [ZZ_YHJSXXB] WHERE DLYX='" +dsinput.Tables["子表"].Rows[0]["DLRDLZH"].ToString() + "'";
                string sfztxyw = DbHelperSQL.GetSingle(sql).ToString().Trim();

                //判断经纪人是否已经添加过了
                int c = (int)DbHelperSQL.GetSingle("select count(*) from ZZ_MMJYDLRZHGLB where JSDLZH='" + dsinput.Tables["子表"].Rows[0]["JSDLZH"].ToString() + "' and DLRDLZH='" + dsinput.Tables["子表"].Rows[0]["DLRDLZH"].ToString().Trim() + "'");
                if (c > 0)
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["错误提示"] = "经纪人已经存在于列表之中！";
                }
                else
                {
                    ////判断是否需要更改默认账号
                    //if (dsinput.Tables["子表"].Rows[0]["SFMRDLR"].ToString().Trim() == "是")
                    //{
                    //    string sqlmr = "update ZZ_MMJYDLRZHGLB set SFMRDLR='否' where SFMRDLR='是'  and JSDLZH='" + info["JSDLZH"].ToString().Trim() + "'";
                    //    commands.Add(sqlmr);
                    //}
                    //写入语句

                    //string strJSZHLX = "select JSZHLX from dbo.ZZ_UserLogin where DLYX='" + dsinput.Tables["子表"].Rows[0]["JSDLZH"].ToString().Trim() + "'";
                    //object JSZHLX = DbHelperSQL.GetSingle(strJSZHLX);
                    string sqlstr = "insert into ZZ_MMJYDLRZHGLB([Number],[JSDLZH],[JSYHM],[JSZHLX],[JSBH],[JSLX],[DLRDLZH],[DLRBH],[DLRYHM],[SFMRDLR],[SQGLSJ],[DLRSHZT],[DLRSHSJ],[DLRSHYJ],FGSSHZT,[GLZT],[SFZTXYW],[DLRBGBZ],[CheckState],[CreateUser],[CreateTime]) values ('" + KeyNumber + "','" + dsinput.Tables["子表"].Rows[0]["JSDLZH"].ToString().Trim() + "','" + UserInfo["用户名"].ToString().Trim() + "','" + UserInfo["结算账户类型"].ToString().Trim() + "','" + dsinput.Tables["子表"].Rows[0]["JSBH"].ToString().Trim() + "','" + UserInfo["角色类型"].ToString().Trim() + "','" + dsinput.Tables["子表"].Rows[0]["DLRDLZH"].ToString().Trim() + "','" + dsinput.Tables["子表"].Rows[0]["DLRBH"].ToString().Trim() + "','" + dsinput.Tables["子表"].Rows[0]["DLRYHM"].ToString().Trim() + "','" + dsinput.Tables["子表"].Rows[0]["SFMRDLR"].ToString().Trim() + "',getdate(),'待审核',null,'','待审核','有效','" + sfztxyw + "','" + DateTime.Now.ToString() + "',1,'CUser',getdate())";
                    string sqlstr1 = "";
                    if (dsinput.Tables["子表"].Rows.Count>1)
                    {
                        KeyNumber = jhjx_PublicClass.GetNextNumberZZ("ZZ_MMJYDLRZHGLB", "");
                        //数据检查
                        string SellerJSBH1 = dsinput.Tables["子表"].Rows[1]["JSBH"].ToString();
                        Hashtable UserInfo1 = jhjx_PublicClass.GetUserInfo(SellerJSBH1);
                        sqlstr1 = "insert into ZZ_MMJYDLRZHGLB([Number],[JSDLZH],[JSYHM],[JSZHLX],[JSBH],[JSLX],[DLRDLZH],[DLRBH],[DLRYHM],[SFMRDLR],[SQGLSJ],[DLRSHZT],[DLRSHSJ],[DLRSHYJ],FGSSHZT,[GLZT],[SFZTXYW],[DLRBGBZ],[CheckState],[CreateUser],[CreateTime]) values ('" + KeyNumber + "','" + dsinput.Tables["子表"].Rows[1]["JSDLZH"].ToString().Trim() + "','" + UserInfo1["用户名"].ToString().Trim() + "','" + UserInfo1["结算账户类型"].ToString().Trim() + "','" + dsinput.Tables["子表"].Rows[1]["JSBH"].ToString().Trim() + "','" + UserInfo1["角色类型"].ToString().Trim() + "','" + dsinput.Tables["子表"].Rows[1]["DLRDLZH"].ToString().Trim() + "','" + dsinput.Tables["子表"].Rows[1]["DLRBH"].ToString().Trim() + "','" + dsinput.Tables["子表"].Rows[1]["DLRYHM"].ToString().Trim() + "','" + dsinput.Tables["子表"].Rows[1]["SFMRDLR"].ToString().Trim() + "',getdate(),'待审核',null,'','待审核','有效','" + sfztxyw + "','" + DateTime.Now.ToString() + "',1,'CUser',getdate());";
                    }
                    commands.Add(sqlstr);
                    commands.Add(sqlstr1);
                    int count = DbHelperSQL.ExecuteSqlTran(commands);
                    if (count > 0)
                    {

                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                        dsreturn.Tables["返回值单条"].Rows[0]["错误提示"] = "数据保存成功！";
                    }
                    else
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["错误提示"] = "数据保存失败！";
                    }

                }
            }
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["错误提示"] = "此经纪人被禁止或暂停业务，无法添加经纪人！";
            return dsreturn;
        }
        return dsreturn;
        
    }
    /// <summary>
    /// 初始化返回值数据集
    /// </summary>
    /// <returns></returns>
    public DataSet initReturnDataSet()
    {
        DataSet ds = new DataSet();
        DataTable auto2 = new DataTable();
        auto2.TableName = "返回值单条";
        auto2.Columns.Add("执行结果");
        auto2.Columns.Add("错误提示");
        ds.Tables.Add(auto2);
        return ds;
    }
    /// <summary>
    /// 仅处理基础检查
    /// </summary>
    /// <param name="dsinput"></param>
    /// <returns></returns>
    private DataSet OnlyJC(DataSet dsinput)
    {

        //初始化返回值结构
        DataSet dsreturn = initReturnDataSet();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "未知错误" });

        //数据检查
        string SellerJSBH = dsinput.Tables["主表"].Rows[0]["买家角色编号"].ToString();
        Hashtable UserInfo = jhjx_PublicClass.GetUserInfo(SellerJSBH);


        if (SellerJSBH.Trim() == "" || UserInfo["是否审核通过"].ToString() != "审核通过")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["错误提示"] = "您尚未开通结算账户，将不能维护经纪人！";
            return dsreturn;
        }
        if (UserInfo["是否允许登录"].ToString() != "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["错误提示"] = "您已被禁止登录，将不能维护经纪人！";
            return dsreturn;
        }
        if (!jhjx_PublicClass.IsOpenDay() || !jhjx_PublicClass.IsOpenTime())
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["错误提示"] = "当前时间业务停止运行，将不能维护经纪人！";
            return dsreturn;
        }
        if (UserInfo["是否休眠"].ToString() == "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["错误提示"] = "您的账号已休眠，将不能维护经纪人！";
            return dsreturn;
        }
        //if (UserInfo["是否冻结"].ToString() == "是")
        //{
        //    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
        //    dsreturn.Tables["返回值单条"].Rows[0]["错误提示"] = "您的账号已被禁止业务，将不能维护经纪人！";
        //    return dsreturn;
        //}
        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "success";
        dsreturn.Tables["返回值单条"].Rows[0]["错误提示"] = "检查通过！";
        return dsreturn;

    }
    
    public DataSet AgentSet(object obj)
    {
        DataSet dsinput = (DataSet)obj;
        //首次进入的检查
        if (dsinput.Tables["主表"] != null && dsinput.Tables["主表"].Rows.Count > 0)
        {
            if (dsinput.Tables["主表"].Rows[0]["特殊标记"].ToString() == "仅检查基础")
            {
                return OnlyJC(dsinput);
            }
        }
        //初始化返回值结构
        DataSet dsreturn = initReturnDataSet();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "未知错误" });
        List<string> commands = new List<string>();
        //数据检查
        string SellerJSBH = dsinput.Tables["子表"].Rows[0]["JSBH"].ToString();
        Hashtable UserInfo = jhjx_PublicClass.GetUserInfo(SellerJSBH);
        if (SellerJSBH.Trim() == "" || UserInfo["是否审核通过"].ToString() != "审核通过")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["错误提示"] = "您尚未开通结算账户，将不能维护经纪人！";
            return dsreturn;
        }
        if (UserInfo["是否允许登录"].ToString() != "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["错误提示"] = "您已被禁止登录，将不能维护经纪人！";
            return dsreturn;
        }
        if (!jhjx_PublicClass.IsOpenDay() || !jhjx_PublicClass.IsOpenTime())
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["错误提示"] = "当前时间业务停止运行，将不能维护经纪人！";
            return dsreturn;
        }
        if (UserInfo["是否休眠"].ToString() == "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["错误提示"] = "您的账号已休眠，将不能维护经纪人！";
            return dsreturn;
        }
        //if (UserInfo["是否冻结"].ToString() == "是")
        //{
        //    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
        //    dsreturn.Tables["返回值单条"].Rows[0]["错误提示"] = "您的账号已被禁止业务，将不能维护经纪人！";
        //    return dsreturn;
        //}
        
        if (UserInfo["当前默认关联经纪人角色编号"].ToString() != "" && UserInfo["当前默认关联经纪人是否暂停新业务"].ToString()=="否")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["错误提示"] = "您的默认经纪人未暂停业务，不能更改默认经纪人！";
            return dsreturn;
        }

        if (UserInfo["当前默认关联经纪人角色编号"].ToString() == ""|| UserInfo["当前默认关联经纪人是否冻结"].ToString() == "是" || UserInfo["当前默认关联经纪人是否验证邮箱"].ToString() == "否" || UserInfo["当前默认关联经纪人是否允许登录"].ToString() == "否" || UserInfo["当前默认关联经纪人是否休眠"].ToString() == "是")
        {
            string strNewJJR = "select top 1 * from ZZ_UserLogin  where DLYX = '" + dsinput.Tables["子表"].Rows[0]["DLRDLZH"].ToString() + "' and SFDJZH = '否' order by CreateTime DESC ";
            DataSet dsNewJJR = DbHelperSQL.Query(strNewJJR);
            if (dsNewJJR != null && dsNewJJR.Tables[0].Rows.Count > 0)
            {
                if (dsNewJJR.Tables[0].Rows[0]["SFDJZH"].ToString() == "是" || dsNewJJR.Tables[0].Rows[0]["SFXM"].ToString() == "是" || dsNewJJR.Tables[0].Rows[0]["SFYXDL"].ToString() == "否" || dsNewJJR.Tables[0].Rows[0]["SFYZYX"].ToString() == "否" || dsNewJJR.Tables[0].Rows[0]["SFZTXYW"].ToString() == "是")
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["错误提示"] = "此经纪人被禁止或暂停业务，不能设为默认经纪人！";
                    return dsreturn;
                }
                else
                {
                    string sqlstr1 = "update ZZ_MMJYDLRZHGLB set SFMRDLR='否' where SFMRDLR='是' and JSDLZH='" + dsinput.Tables["子表"].Rows[0]["JSDLZH"].ToString().Trim() + "' and DLRDLZH !='" + dsinput.Tables["子表"].Rows[0]["DLRDLZH"].ToString().Trim() + "' ";
                    string sqlstr2 = "update ZZ_MMJYDLRZHGLB set SFMRDLR='是' where SFMRDLR='否' and JSDLZH ='" + dsinput.Tables["子表"].Rows[0]["JSDLZH"].ToString().Trim() + "' and DLRDLZH ='" + dsinput.Tables["子表"].Rows[0]["DLRDLZH"].ToString().Trim() + "' ";
                    commands.Add(sqlstr1);
                    commands.Add(sqlstr2);
                    int count = DbHelperSQL.ExecuteSqlTran(commands);
                    if (count > 0)
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                        dsreturn.Tables["返回值单条"].Rows[0]["错误提示"] = "更新成功！";
                    }
                    else
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["错误提示"] = "更新失败！";
                    }
                }
            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["错误提示"] = "此经纪人被禁止或暂停业务，不能设为默认经纪人！";
                return dsreturn;
            }
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["错误提示"] = "您的默认经纪人业务正常，不能更改默认经纪人！";
            return dsreturn;
        }

        return dsreturn;
    }

    /// <summary>
    /// 重新提交
    /// </summary>
    /// <param name="DLYX"></param>
    /// <param name="JJRBH"></param>
    /// <returns></returns>
    public DataSet Resubmit(DataSet ds)
    {
        //初始化返回值结构
        DataSet dsreturn = initReturnDataSet();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "未知错误" });
        if (ds!=null&&ds.Tables[0].Rows.Count>0)
        {
            string strSQLUpdate = "update [ZZ_MMJYDLRZHGLB] set DLRSHZT='待审核',FGSSHZT='待审核' where JSDLZH='" + ds.Tables[0].Rows[0]["登录邮箱"].ToString() + "' and DLRBH='" + ds.Tables[0].Rows[0]["经纪人编号"].ToString() + "'";
            int i = DbHelperSQL.ExecuteSql(strSQLUpdate);
            if (i > 0)
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["错误提示"] = "重新提交成功！";
                return dsreturn;
            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["错误提示"] = "重新提交失败！";
                return dsreturn;
            }
        }
        
        
        return dsreturn;
    }

}