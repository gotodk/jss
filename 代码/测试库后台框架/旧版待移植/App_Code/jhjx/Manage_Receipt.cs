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
/// Manage_Receipt 的摘要说明  发票信息管理
/// </summary>
public class Manage_Receipt
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
    /// 添加开票信息
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public string AddReceipt(object obj)
    {
        string result;
        //将参数转换为哈希表
        Hashtable info = ObjToHash(obj);

        List<string> commands = new List<string>();

        //获取角色编号的相关信息       
        Hashtable UserInfo = jhjx_PublicClass.GetUserInfo(info["角色编号"].ToString());

        if (UserInfo["是否审核通过"].ToString() == "否" || UserInfo["是否审核通过"].ToString() == "")
        {

            result = "尚未开通结算账户，无法添加开票信息！";
        }
        else
        {
            //获取新增数据的number值 
            string KeyNumber = jhjx_PublicClass.GetNextNumberZZ("ZZ_KPXXBGSQB", "");

           
            //写入语句
            string sqlstr = " insert into ZZ_KPXXBGSQB ([Number],[DLZH],[YHM],[JSZHLX],[YFPLX],[YKPXX],[XFPLX],[XKPXX],[SHZT],[CheckState],[CreateUser],[CreateTime],[FilePath]) values('" + KeyNumber + "','" + UserInfo["登录邮箱"].ToString() + "','" + UserInfo["用户名"].ToString() + "','" + UserInfo["结算账户类型"].ToString() + "','" + info["原发票类型"].ToString() + "','" + info["原开票信息"].ToString() + "','" + info["新发票类型"].ToString() + "','" + info["新开票信息"].ToString() + "','待审核',0,'" + info["角色编号"].ToString() + "',getdate(),'" + info["文件路径"].ToString() + "')";
            commands.Add(sqlstr);
            int count = DbHelperSQL.ExecuteSqlTran(commands);
            if (count > 0)
            {

                result = "ok";
            }
            else
            {

                result = "数据保存失败！";
            }

        }
        return result;

    }

    public string EditReceipt(object obj)
    {
        string result;
        //将参数转换为哈希表
        Hashtable info = ObjToHash(obj);

        List<string> commands = new List<string>();

        //获取角色编号的相关信息       
        Hashtable UserInfo = jhjx_PublicClass.GetUserInfo(info["角色编号"].ToString());

        if (UserInfo["是否审核通过"].ToString() == "否" || UserInfo["是否审核通过"].ToString() == "")
        {

            result = "尚未开通结算账户，无法添加开票信息！";
        }
        else
        {
            //获取新增数据的number值 
            string KeyNumber = info["IsNum"].ToString().Trim();

            //写入语句
            string sqlstr = "update ZZ_KPXXBGSQB set YFPLX='" + info["原发票类型"].ToString() + "',YKPXX='" + info["原开票信息"].ToString() + "',XFPLX='" + info["新发票类型"].ToString() + "',XKPXX='" + info["新开票信息"].ToString() + "',SHZT='待审核',CheckState=0,CreateTime=GETDATE(),FilePath='" + info["文件路径"].ToString() + "' where Number='" + KeyNumber + "'";
            commands.Add(sqlstr);
            int count = DbHelperSQL.ExecuteSqlTran(commands);
            if (count > 0)
            {
                result = "ok";
            }
            else
            {
                result = "数据保存失败！";
            }
        }
        return result;

    }


}