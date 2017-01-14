using System;
using System.Collections.Generic;
using System.Web;
using System.Collections;
using FMOP.DB;

/// <summary>
///JHJX_SendRemindInfor 的摘要说明
/// </summary>
public class JHJX_SendRemindInfor
{
	public JHJX_SendRemindInfor()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}


    /// <summary>
    /// 提醒发送函数  
    /// </summary>
    /// <param name="list1"> HashTable 集合 ,有几种提醒放几个hsahtable，其中type键值区分提醒类别：集合集合经销平台，业务平台，手机短信，邮箱提醒 </param>
    public static void Sendmes(List<Hashtable> list1)
    {
        if (list1 != null && list1.Count > 0)
        {
            ArrayList ltsql = new ArrayList();
            DateTime dt = DateTime.Now;
           // string Insert = "";
            //string KeyNumber = "";
            foreach (Hashtable ht in list1)
            {
              

                if (ht["type"].ToString().Equals("集合集合经销平台"))
                {
                   string KeyNumber = jhjx_PublicClass.GetNextNumberZZ("AAA_PTTXXXB", ""); 
             
                    //集合经销平台平台提醒  
                 string    Insert = " insert into  AAA_PTTXXXB ( Number, TXDXDLYX,TXDXYHM,TXDXJSZHLX,TXDXJSBH,TXDXJSLX,TXSJ,TXNRWB,SFXSG,CreateUser ) values ( '" + KeyNumber + "','" + ht["提醒对象登陆邮箱"].ToString() + "','" + ht["提醒对象用户名"].ToString() + "','" + ht["提醒对象结算账户类型"].ToString() + "','" + ht["提醒对象角色编号"].ToString() + "','" + ht["提醒对象角色类型"].ToString() + "','" + dt + "','" + ht["提醒内容文本"].ToString() + "','否','" + ht["创建人"].ToString() + "' ) ";
                    ltsql.Add(Insert);

                }
                if (ht["type"].ToString().Equals("业务平台"))
                {
                    //业务操作平台提醒
                    string sql = "insert into User_Warnings(Context,Module_Url,Grade,FromUser,Touser)";
                    sql = sql + " values('" + ht["提醒内容文本"].ToString() + "','" + ht["提醒页面路径"].ToString() + "','1','" + ht["提醒发送人"].ToString() + "','" + ht["提醒接收人"] + "')";

                    ltsql.Add(sql);


                }
                if (ht["type"].ToString().Equals("手机短信"))
                {
                    //手机提醒

                }
                if (ht["type"].ToString().Equals("邮箱提醒"))
                {
                    //手机提醒

                }

            }
            if (ltsql != null && ltsql.Count > 0)
            {

                DbHelperSQL.ExecSqlTran(ltsql);
            }
        }
    }


}