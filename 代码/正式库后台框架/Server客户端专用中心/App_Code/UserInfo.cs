using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FMDBHelperClass;
using System.IO;
using System.Xml;
using Aspose.Words;
using Aspose.Words.Saving;

/// <summary>
/// 封装一些关于交易账户仅用于一次调用的方法
/// create by zzf 2014.7.13
/// </summary>
public class UserInfo
{
	public UserInfo()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}

    /// <summary>
    ///根据买卖家登录邮箱、获取关联表中有关买卖家的数据信息,用于“暂停交易方新业务”中判断交易方是否真实有效
    /// </summary>
    /// <param name="strJJRZGZ"></param>
    /// <returns></returns>
    public  DataSet GetMMJJYZHData( DataTable dtInfor)
    {
        try
        {
            DataSet ds = null;
            string strSql = "select a.DLYX '登录邮箱',b.I_JYFMC '交易方名称', b.B_JSZHLX '结算账户类型' from AAA_MJMJJYZHYJJRZHGLB as a inner join AAA_DLZHXXB as b on a.DLYX=b.B_DLYX where DLYX='" + dtInfor.Rows[0]["买卖家登录邮箱"].ToString() + "' and SFYX='是' and SFDQMRJJR='是' and GLJJRBH='" + dtInfor.Rows[0]["关联经纪人角色编号"].ToString() + "'   and JJRSHZT='审核通过' and FGSSHZT='审核通过'  and b.B_JSZHLX='买家卖家交易账户'";

            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
            Hashtable param = new Hashtable();
            param.Add("@DLYX", dtInfor.Rows[0]["买卖家登录邮箱"].ToString());
            param.Add("GLJJRBH",dtInfor.Rows[0]["关联经纪人角色编号"].ToString());

            Hashtable return_ht = I_DBL.RunParam_SQL(strSql, "买卖家数据信息", param);
            if ((bool)(return_ht["return_float"])) //说明执行完成
            {
                ds = (DataSet)return_ht["return_ds"];
            }           
            return ds;
                       
        }
        catch
        {
            return null;
        }
                      
    }

    /// <summary>
    /// 得到交易账户审核的数据信息，如审核意见，审核时间等等，用于账户资料页签中的“查看审核详情页面
    /// </summary>    
    /// <param name="dataTable"></param>
    /// <returns></returns>
    public DataSet GetJYZHSHData(DataTable dataTable)
    {
        try
        {
            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
            Hashtable param = new Hashtable();
            param.Add("@DLYX", dataTable.Rows[0]["DLYX"].ToString());
            param.Add("@JSZHLX", dataTable.Rows[0]["JSZHLX"].ToString());

            string strSql = "select AAA_MJMJJYZHYJJRZHGLB.Number,DLYX,JSZHLX,GLJJRDLZH,GLJJRBH,GLJJRYHM,SFDQMRJJR '是否当前默认经纪人',CONVERT(varchar(20),b.I_ZLTJSJ,120) '申请时间',JJRSHZT '经纪人审核状态',CONVERT(varchar(20),JJRSHSJ,120)  '经纪人审核时间',JJRSHYJ '经纪人审核意见',FGSSHZT '分公司审核状态',FGSSHR '分公司审核人',CONVERT(varchar(20),FGSSHSJ,120) '分公司审核时间',FGSSHYJ '分公司审核意见' from AAA_MJMJJYZHYJJRZHGLB inner join AAA_DLZHXXB as b on AAA_MJMJJYZHYJJRZHGLB.DLYX=b.B_DLYX  where DLYX=@DLYX and SFYX='是' and  JSZHLX=@JSZHLX ";
            if (dataTable.Rows[0]["JSZHLX"].ToString() == "买家卖家交易账户")
            {
                strSql += " and SFSCGLJJR='是'  order by SQSJ desc";

            }
            else  //经纪人交易账户
            {
                strSql += " order by SQSJ desc";
            }

            DataSet ds = null;
            Hashtable return_ht = I_DBL.RunParam_SQL(strSql, "交易账户审核数据信息", param);
            if ((bool)(return_ht["return_float"])) //说明执行完成
            {
                ds = (DataSet)return_ht["return_ds"];
            }
            return ds;
                       
        }
        catch
        {
            return null;
 
        }

     
    }

    
    /// <summary>
    /// 获取经纪人资格证书路径，生成经纪人资格证书图片
    /// </summary>   
    /// <param name="dataTable"></param>
    /// <returns></returns>    
    public DataSet GetJJRZGZS(DataTable dataTable)
    {
        DataSet dsreturn = new DataSet();
        //要返回的数据集中的数据表
        DataTable d = new DataTable();
        d.TableName = "返回值单条";
        d.Columns.Add("执行结果", typeof(string));
        d.Columns.Add("提示文本", typeof(string));
        d.Rows.Add(new string[] { "err", "初始化" });

        dsreturn.Tables.Add(d);
        
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable param = new Hashtable();
        param.Add("@JSBH", dataTable.Rows[0]["买家角色编号"].ToString());
        param.Add("@DLYX", dataTable.Rows[0]["DLYX"].ToString());
        param.Add("@JSZHLX", dataTable.Rows[0]["JSZHLX"].ToString());
                
        try
        {
            string str1 = "select J_JJRZGZSBH '经纪人资格证书编号',S_SFYBFGSSHTG '是否已经被分公司审核通过',I_JYFMC '交易方名称',JJRZGZS '经纪人资格证书',J_JJRZGZSYXQKSSJ '经纪人资格证书有效期开始时间',J_JJRZGZSYXQJSSJ '经纪人资格证书有效期结束时间',* from AAA_DLZHXXB where B_DLYX=@DLYX  and B_JSZHLX=@JSZHLX ";

            //开始执行    
            #region//生成经纪人资格证书编号

            Hashtable return_ht = I_DBL.RunParam_SQL(str1, "证书信息", param);
            DataSet ds = (DataSet)return_ht["return_ds"];

            DataTable dt_UserInfo = ds.Tables["证书信息"];//获取用户信息，
            string strJJRZGZSBH = dt_UserInfo.Rows[0]["经纪人资格证书编号"].ToString();//经纪人资格证书编号
            string strJYFMC = dt_UserInfo.Rows[0]["交易方名称"].ToString();//交易方名称
            object objstrJJRZGZS = dt_UserInfo.Rows[0]["经纪人资格证书"];
            string strJJRZGZS = "";
            if (objstrJJRZGZS is DBNull)
            {
                strJJRZGZS = "";
            }
            else
            {
                strJJRZGZS = objstrJJRZGZS.ToString();
            }
            //如果没有经纪人资格证书而且是审核通过
            if (String.IsNullOrEmpty(strJJRZGZS) && dt_UserInfo.Rows[0]["是否已经被分公司审核通过"].ToString() == "是")
            {
                DateTime ZSYXQ_QS = Convert.ToDateTime(dt_UserInfo.Rows[0]["经纪人资格证书有效期开始时间"].ToString());
                string year_QS = ZSYXQ_QS.Year.ToString();//年份
                string month_QS = ZSYXQ_QS.Month.ToString();//月份
                string day_QS = ZSYXQ_QS.Day.ToString();//日期
                DateTime ZSYXQ_ZZ = Convert.ToDateTime(dt_UserInfo.Rows[0]["经纪人资格证书有效期结束时间"].ToString());//有限期截止时间，在当前有效期在延后两年
                string year_ZZ = ZSYXQ_ZZ.Year.ToString();//年份
                string month_ZZ = ZSYXQ_ZZ.Month.ToString();//月份
                string day_ZZ = ZSYXQ_ZZ.Day.ToString();//日期


                string ResourcePath = HttpContext.Current.Server.MapPath("~/pingtaiservices/JHJX_JJRZGZS/JJRZGZS_Path/JJRZGZS_Initial.xml");//经纪人资格证书模板服务器路径
                string FileName = Guid.NewGuid().ToString();
                string Paths = HttpContext.Current.Server.MapPath("~/pingtaiservices/JHJX_JJRZGZS/JJRZGZS_NewPath/") + FileName + ".xml";//拷贝后的文件目录、
                string NewPaths = HttpContext.Current.Server.MapPath("~/pingtaiservices/JHJX_JJRZGZS/JJRZGZS_NewPath/COPY/") + FileName + ".xml";//保存后的目录
                string SavingPath = "~/JHJXPT/SaveDir/JJRZGZS/" + FileName + ".png";//存入路径
                string SavingPathDataBase = "/JHJXPT/SaveDir/JJRZGZS/" + FileName + ".png";//存入数据库的路径;
                if (File.Exists(ResourcePath))
                {
                    File.Copy(ResourcePath, Paths, true);
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(Paths);//载入此XML
                    XmlNodeList xnl = xmlDoc.GetElementsByTagName("w:t");//office xml word的Tag
                    foreach (XmlNode xn in xnl)
                    {
                        if (xn.InnerXml == "PT_BH")
                        {
                            xn.InnerText = strJJRZGZSBH;
                        }
                        if (xn.InnerXml == "JYFMC")
                        {
                            xn.InnerXml = strJYFMC;
                        }
                        if (xn.InnerXml == "YYYY_QS")
                        {
                            xn.InnerXml = year_QS;
                        }
                        if (xn.InnerXml == "MM_QS")
                        {
                            xn.InnerXml = month_QS;
                        }
                        if (xn.InnerXml == "dd_QS")
                        {
                            xn.InnerXml = day_QS;
                        }
                        if (xn.InnerXml == "YYYY_ZZ")
                        {
                            xn.InnerXml = year_ZZ;
                        }
                        if (xn.InnerXml == "MM_ZZ")
                        {
                            xn.InnerXml = month_ZZ;
                        }
                        if (xn.InnerXml == "dd_ZZ")
                        {
                            xn.InnerXml = day_ZZ;
                        }
                        if (xn.InnerXml == "YYYY_PT")
                        {
                            xn.InnerXml = year_QS;
                        }
                        if (xn.InnerXml == "MM_PT")
                        {
                            xn.InnerXml = month_QS;
                        }
                        if (xn.InnerXml == "dd_PT")
                        {
                            xn.InnerXml = day_QS;
                        }
                    }
                    xmlDoc.Save(NewPaths);
                    if (File.Exists(NewPaths))
                    {
                        Document doc = new Document(NewPaths);
                        ImageSaveOptions iso = new ImageSaveOptions(SaveFormat.Png);//另存为PNG格式
                        //  iso.Resolution = 128;

                        doc.Save(HttpContext.Current.Server.MapPath(SavingPath), iso);
                    }
                    File.Delete(NewPaths);
                    File.Delete(Paths);
                }

                string str2="update  AAA_DLZHXXB set JJRZGZS='" + SavingPathDataBase + "' where B_DLYX=@DLYX ";
                
                Hashtable return_ht2 = I_DBL.RunParam_SQL(str2, param);                  
                
                string str3="select J_JJRZGZSBH '经纪人资格证书编号',I_JYFMC '交易方名称',JJRZGZS '经纪人资格证书',* from AAA_DLZHXXB where B_DLYX=@DLYX ";
                
                 Hashtable return_ht3 = I_DBL.RunParam_SQL(str3, "相关信息", param);
                 DataSet ds3 = (DataSet)return_ht3["return_ds"];

                 DataTable dt_UserInfoTwo = ds3.Tables["相关信息"];//获取用户信息，
                DataTable dt_UserInfoTwoCopy = dt_UserInfoTwo.Copy();
                if (dt_UserInfoTwoCopy.Rows.Count > 0)
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "经纪人交易账户信息获取成功！";
                    dsreturn.Tables.Add(dt_UserInfoTwoCopy);
                    dsreturn.Tables[1].TableName = "经纪人交易账户信息";
                }
                else
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "经纪人交易账户信息获取失败！";
                }

                return dsreturn;
            }
            else
            {
                if (dt_UserInfo.Rows.Count > 0)
                {
                    DataTable dt_UserInfoCopy = dt_UserInfo.Copy();
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "经纪人交易账户信息获取成功！";
                    dsreturn.Tables.Add(dt_UserInfoCopy);
                    dsreturn.Tables[1].TableName = "经纪人交易账户信息";
                }
                else
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "经纪人交易账户信息获取失败！";


                }
                return dsreturn;

            }
            #endregion
        }
        catch
        {
            return null;
        }

       
    }
       

}