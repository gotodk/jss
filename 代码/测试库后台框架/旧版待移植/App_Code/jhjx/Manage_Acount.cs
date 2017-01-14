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
/// Manage_Acount 的摘要说明.用于管理收款账户的类
/// </summary>
public class Manage_Acount
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
    /// 添加收款账户
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public string AddAcount(object obj)
    {
        string result;
        //将参数转换为哈希表
        Hashtable skinfo = ObjToHash(obj);        

        List<string> commands = new List<string>();

         //获取角色编号的相关信息       
        Hashtable UserInfo = jhjx_PublicClass.GetUserInfo(skinfo["买家角色编号"].ToString());

        if (UserInfo["是否审核通过"].ToString() == "否" || UserInfo["是否审核通过"].ToString() == "")
        {

            result = "尚未开通结算账户，无法添加收款账户！";
        }
        else
        {
            //获取新增数据的number值 
            string KeyNumber = jhjx_PublicClass.GetNextNumberZZ("ZZ_SKZHXXB", "");

            //判断是否需要更改默认账号
            if (skinfo["是否默认账户"].ToString() == "是")
            {
                string sqlmr = "update ZZ_SKZHXXB set SFMRZH='否' where SFMRZH='是'  and JSDLZH='" + skinfo["登录邮箱"].ToString().Trim() + "'";
                commands.Add(sqlmr);
            }
            //写入语句
            string sqlstr = "insert into ZZ_SKZHXXB([Number],[JSDLZH],[JSYHM],[JSZHLX],[JSBH],[JSLX],[SKZHMC],[KHYHMC],[KHYHZH],[SFMRZH],[SFYX],[CheckState],[CreateUser],[CreateTime],[FilePath]) values('" + KeyNumber + "','" + UserInfo["登录邮箱"].ToString() + "','" + UserInfo["用户名"].ToString() + "','" + UserInfo["结算账户类型"].ToString() + "','" + skinfo["买家角色编号"].ToString() + "','" + UserInfo["结算账户类型"].ToString() + "','" + skinfo["收款人姓名"].ToString() + "','" + skinfo["开户银行名称"].ToString() + "','" + skinfo["银行账号"].ToString() + "','" + skinfo["是否默认账户"].ToString() + "','是',1,'" + skinfo["买家角色编号"].ToString() + "',GETDATE(),'" + skinfo["文件路径"].ToString() + "')";
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
    /// <summary>
    /// 修改现有收款账户
    /// </summary>
    /// <param name="?"></param>
    /// <returns></returns>
    public string EditAcount(object obj)
    {
        string result;
        //将参数转换为哈希表
        Hashtable skinfo = ObjToHash(obj);        

        List<string> commands = new List<string>();

         //获取角色编号的相关信息       
        Hashtable UserInfo = jhjx_PublicClass.GetUserInfo(skinfo["买家角色编号"].ToString());

        if (UserInfo["是否审核通过"].ToString() == "否" || UserInfo["是否审核通过"].ToString() == "")
        {

            result = "尚未开通结算账户，无法添加收款账户！";
        }
        else
        {
            //获取number
            string KeyNumber = skinfo["IsNum"].ToString().Trim();

            //判断是否需要更改默认账号
            if (skinfo["是否默认账户"].ToString() == "是")
            {
                string sqlmr = "update ZZ_SKZHXXB set SFMRZH='否' where SFMRZH='是' and JSDLZH='" + skinfo["登录邮箱"].ToString().Trim() + "'";
                commands.Add(sqlmr);
            }
            //更新语句           
            string sqlstr = "update ZZ_SKZHXXB set SFMRZH='" + skinfo["是否默认账户"].ToString() + "' ,SKZHMC='" + skinfo["收款人姓名"].ToString() + "',KHYHMC='" + skinfo["开户银行名称"].ToString() + "',KHYHZH='" + skinfo["银行账号"].ToString() + "',CreateTime=GETDATE() ,FilePath='" + skinfo["文件路径"].ToString() + "' where Number='" + KeyNumber + "'";
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

    /// <summary>
    /// 删除收款账户
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public string DeleteAcount(object obj)
    {
        string result;
        //将参数转换为哈希表
        Hashtable skinfo = ObjToHash(obj);

        List<string> commands = new List<string>();

        //获取角色编号的相关信息       
        Hashtable UserInfo = jhjx_PublicClass.GetUserInfo(skinfo["买家角色编号"].ToString());

        if (UserInfo["是否审核通过"].ToString() == "否" || UserInfo["是否审核通过"].ToString() == "")
        {

            result = "尚未开通结算账户，无法删除收款账户！";
        }
        else
        {            
            //首先从服务器删除已经上传的图片
            if(skinfo["文件路径"].ToString().Trim()!="")
            {
                 string path = HttpContext.Current.Server.MapPath("/JHJXPT/SaveDir/" + skinfo["文件路径"].ToString().Trim());
                 System.IO.File.Delete(path);
            }
           

            //获取新增数据的number支
            string KeyNumber = skinfo["编号"].ToString();
            string sqlstr = "delete from ZZ_SKZHXXB where Number='"+KeyNumber+"'";
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

    
    /// <summary>
    /// 设置默认收款账户
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public string SetAcount(object obj)
    {
        string result;
        //将参数转换为哈希表
        Hashtable skinfo = ObjToHash(obj);

        List<string> commands = new List<string>();

        //获取角色编号的相关信息       
        Hashtable UserInfo = jhjx_PublicClass.GetUserInfo(skinfo["买家角色编号"].ToString());

        if (UserInfo["是否审核通过"].ToString() == "否" || UserInfo["是否审核通过"].ToString() == "")
        {

            result = "尚未开通结算账户，无法删除收款账户！";
        }
        else
        {
            //获取新增数据的number支
            string KeyNumber = skinfo["编号"].ToString();
            string sqlstr1 = "update ZZ_SKZHXXB set SFMRZH='否' where SFMRZH='是' and Number!='"+KeyNumber+"' ";
            string sqlstr2 = "update ZZ_SKZHXXB set SFMRZH='是' where SFMRZH='否' and Number='"+KeyNumber+"' ";
            commands.Add(sqlstr1);
            commands.Add(sqlstr2);
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