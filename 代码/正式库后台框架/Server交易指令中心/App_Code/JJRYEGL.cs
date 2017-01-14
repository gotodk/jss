/*************************************************************************
 *  创建人：wyh 
 *  
 *  创建时间：2014.05.28
 *   
 *  功能：经纪人业务管理
 * 
 *  参考文档《业务分析》
 * **************************************************************************/
using FMDBHelperClass;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// JJRYEGL 的摘要说明
/// </summary>
public class JJRYEGL
{
	public JJRYEGL()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}


    /// <summary>
    /// 经纪人暂停或恢复买卖家交易
    /// </summary>
    /// <param name="dataTable">必要参数</param>
    /// <returns>返回符合dsreturn结构的Dataset</returns>
    public DataSet SetSFZTJY(DataSet dataTablel)
    {
        DataSet dsreturn = PublicClass.initReturnDataSet();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
        DataTable dataTable = dataTablel.Tables[0];
        string strSql = "";
        string JSBH = dataTable.Rows[0]["经纪人角色编号"].ToString();//经纪人角色编号
        string MMJDLYX = dataTable.Rows[0]["买卖家登录邮箱"].ToString();//买卖家登录邮箱
        string DLYX = dataTable.Rows[0]["登录邮箱"].ToString();//登录邮箱
        string JSZHLX = dataTable.Rows[0]["结算账户类型"].ToString();//结算账户类型
        string CZLX = dataTable.Rows[0]["操作类型"].ToString();//操作类型  暂停、恢复
        string strLY = dataTable.Rows[0]["理由"].ToString();//理由信息

        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        DataSet dsGXlist = new DataSet();
        Hashtable input = new Hashtable();

        input["@DLYX"] = MMJDLYX;
        input["@GLJJRBH"] = JSBH;


        DataTable dataTableInfor = null;
        Hashtable return_htzkls = I_DBL.RunParam_SQL("select SFZTYHXYW '是否暂停用户新业务' from AAA_MJMJJYZHYJJRZHGLB where DLYX=@DLYX and GLJJRBH=@GLJJRBH and SFYX='是' and SFDQMRJJR='是' and JSZHLX='买家卖家交易账户' and JJRSHZT='审核通过' and FGSSHZT='审核通过'", "info", input);
        if ((bool)(return_htzkls["return_float"])) //说明执行完成
        {
            //这里就可以用了。
            dsGXlist = (DataSet)(return_htzkls["return_ds"]);
            if (dsGXlist != null && dsGXlist.Tables["info"].Rows.Count > 0)
            {
                dataTableInfor = dsGXlist.Tables["info"];
            }
          
        }
        else
        {
            //读取数据库过程中发生程序错误了。。。
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = return_htzkls["return_errmsg"].ToString();
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "";
            return dsreturn;

        } //?

        if (dataTableInfor!= null&&dataTableInfor.Rows.Count > 0)
        {

            if (dataTableInfor.Rows[0]["是否暂停用户新业务"].ToString() == "是" && CZLX == "暂停")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "该用户的新业务处于暂停状态，请不要再执行暂停操作！";
                return dsreturn;
            }
            else if (dataTableInfor.Rows[0]["是否暂停用户新业务"].ToString() == "否" && CZLX == "恢复")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "该用户的新业务处于恢复状态，请不要再执行恢复操作！";
                return dsreturn;
            }

        }


        string strSourceValue = "";//原始值
        Hashtable htre = new Hashtable();
        htre["@DLYX"] = MMJDLYX;
        htre["@JSBH"] = JSBH;
        Hashtable ht = I_DBL.RunParam_SQL("select ZTYHXYWBZ from AAA_MJMJJYZHYJJRZHGLB where DLYX=@DLYX and GLJJRBH=@JSBH and SFYX='是' and SFDQMRJJR='是' ", "yx", htre);

        if ((bool)(return_htzkls["return_float"])) //说明执行完成
        {
            //这里就可以用了。
            dsGXlist = (DataSet)(return_htzkls["return_ds"]);
            if (dsGXlist != null && dsGXlist.Tables["yx"].Rows.Count > 0)
            {
                strSourceValue = dsGXlist.Tables["yx"].Rows[0][0].ToString();
            }

        }
        else
        {
            //读取数据库过程中发生程序错误了。。。
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = return_htzkls["return_errmsg"].ToString();
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "";
            return dsreturn;

        } //?
   

        ArrayList arrayList = new ArrayList();
        Hashtable htall = new Hashtable();
        if (CZLX == "暂停")
        {
            htall["@ZTXYWSJ"] = DateTime.Now.ToString();
            htall["@DLYX"] = MMJDLYX;
            htall["@JSBH"] = JSBH;
            strSql = "update  AAA_MJMJJYZHYJJRZHGLB set  SFZTYHXYW='是', ZTXYWSJ=@ZTXYWSJ   where DLYX=@DLYX and GLJJRBH=@JSBH and SFYX='是' and SFDQMRJJR='是'  and JJRSHZT='审核通过' and FGSSHZT='审核通过'";
            arrayList.Add(strSql);

            string strCZ = strSourceValue + "时间：" + DateTime.Now.ToString() + "  操作：暂停该用户新业务 操作理由：" + strLY + "◆";
            htall["@ZTYHXYWBZ"] = strSourceValue + "时间：" + DateTime.Now.ToString() + "  操作：暂停该用户新业务 操作理由：" + strLY + "◆";
            htall["@txDLYX"] = MMJDLYX;
            htall["@txGLJJRBH"] = JSBH;
            string strCZValue = "update AAA_MJMJJYZHYJJRZHGLB set ZTYHXYWBZ=@ZTYHXYWBZ  where DLYX=@txDLYX and GLJJRBH=@txGLJJRBH and SFYX='是' and SFDQMRJJR='是' ";
            arrayList.Add(strCZValue);
        }
        else
        {
            htall["@DLYX"] = MMJDLYX;
            htall["@GLJJRBH"] = JSBH;
            strSql = "update  AAA_MJMJJYZHYJJRZHGLB set  SFZTYHXYW='否',ZTXYWSJ=null   where DLYX=@DLYX and GLJJRBH=@GLJJRBH and SFYX='是' and SFDQMRJJR='是'  and JJRSHZT='审核通过' and FGSSHZT='审核通过'";
            arrayList.Add(strSql);


            htall["@ZTYHXYWBZ"] = strSourceValue + "时间：" + DateTime.Now.ToString() + "  操作：暂停该用户新业务 操作理由：" + strLY + "◆";
            htall["@txDLYX"] = MMJDLYX;
            htall["@txGLJJRBH"] = JSBH;

            string strCZ = strSourceValue + "时间：" + DateTime.Now.ToString() + "  操作：恢复该用户新业务  操作理由：" + strLY + "◆";
            string strCZValue = "update AAA_MJMJJYZHYJJRZHGLB set ZTYHXYWBZ=@ZTYHXYWBZ where DLYX=@txDLYX and GLJJRBH=@txGLJJRBH and SFYX='是' and SFDQMRJJR='是' ";
            arrayList.Add(strCZValue);
        }


         Hashtable htreall =   I_DBL.RunParam_SQL(arrayList, htall);
         if ((bool)htreall["return_float"])
         {
             if ((int)htreall["return_other"] > 0)
             {
                 if (CZLX == "暂停")
                 {
                     dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                     dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您已成功暂停该用户的新业务！";

                 }
                 else
                 {
                     dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                     dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您已成功恢复该用户的新业务！";
                 }
             }
             else
             {
                 dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                 dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "数据受影响行数为0";
              
             }
         }
         else
         {
             dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
             dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = htreall["return_errmsg"].ToString();
             dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "";
             return dsreturn;

         }


        return dsreturn;
 
    }

    /// <summary>
    /// 暂停、恢复新用户审核  仅用于经纪人交易账户的时候
    /// </summary>
    /// <param name="dataTable">参数集合</param>
    /// <returns>返回符合dsretrun结构的Dataset</returns>
    public DataSet SetSFZTSH(DataTable dataTable)
    {
        DataSet dsreturn = PublicClass.initReturnDataSet();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
        string strSql = "";
        string JSBH = dataTable.Rows[0]["经纪人角色编号"].ToString();//经纪人角色编号
        string DLYX = dataTable.Rows[0]["登录邮箱"].ToString();//登录邮箱
        string JSZHLX = dataTable.Rows[0]["结算账户类型"].ToString();//结算账户类型
        string CZLX = dataTable.Rows[0]["操作类型"].ToString();//操作类型  暂停、恢复

        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        DataSet dsGXlist = new DataSet();
        Hashtable input = new Hashtable();

        
       
        DataTable dataTableInfor  = null ;
        input["@B_DLYX"] = DLYX;
        input["@B_JSZHLX"] = JSZHLX;
        Hashtable htrereturn = I_DBL.RunParam_SQL("select J_JJRSFZTXYHSH '经纪人是否暂停新用户审核' from AAA_DLZHXXB where B_DLYX=@B_DLYX and  B_JSZHLX=@B_JSZHLX", "ztxyh", input);
        if ((bool)htrereturn["return_float"])
        {
            dataTableInfor = ((DataSet)(htrereturn["return_ds"])).Tables["ztxyh"];   
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = htrereturn["return_errmsg"].ToString(); 
            return dsreturn;
        }

        if (dataTableInfor.Rows.Count > 0)
        {

            if (dataTableInfor.Rows[0]["经纪人是否暂停新用户审核"].ToString() == "是" && CZLX == "暂停")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您当前已经是暂停新用户审核状态，请不要再执行暂停操作！";
                return dsreturn;
            }
            else if (dataTableInfor.Rows[0]["经纪人是否暂停新用户审核"].ToString() == "否" && CZLX == "恢复")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您当前已经是恢复新用户审核状态，请不要再执行恢复操作！";
                return dsreturn;
            }

        }
     
        Hashtable htaa = new Hashtable();
        htaa["@B_DLYX"] = DLYX;
        htaa["@B_JSZHLX"] = JSZHLX;
        string strSourceValue = "";//原始值
        htrereturn = I_DBL.RunParam_SQL("select J_JRZTXYHBZ from AAA_DLZHXXB  where B_DLYX=@B_DLYX and B_JSZHLX=@B_JSZHLX", "J_JRZTXYHBZ", htaa);
        if ((bool)htrereturn["return_float"])
        {
              DataSet dsa = (DataSet)(htrereturn["return_ds"]);
              if (dsa != null && dsa.Tables["J_JRZTXYHBZ"].Rows.Count > 0)
              {
                  strSourceValue = dsa.Tables["J_JRZTXYHBZ"].Rows[0].ToString();
              }

        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = htrereturn["return_errmsg"].ToString();
            return dsreturn;
        }
    
        ArrayList arryList = new ArrayList();
        Hashtable htinputall = new Hashtable();


        if (CZLX == "暂停")
        {
         
            htinputall["@J_JJRZTXYHSHSJ"] = DateTime.Now.ToString();
            htinputall["@B_DLYX"] = DLYX;
            htinputall["@B_JSZHLX"] = JSZHLX;

            strSql = "update AAA_DLZHXXB set J_JJRSFZTXYHSH='是',J_JJRZTXYHSHSJ=@J_JJRZTXYHSHSJ where B_DLYX=@B_DLYX and B_JSZHLX=@B_JSZHLX ";
            arryList.Add(strSql);
  
            string strCZ = strSourceValue + "时间：" + DateTime.Now.ToString() + "  操作：暂停新用户审核" + "◆";
            htinputall["@J_JRZTXYHBZ"] = strCZ;
            htinputall["@B_DLYX_t"] = DLYX;
            htinputall["@B_JSZHLX_t"] = JSZHLX;

            string strCZValue = "update AAA_DLZHXXB set J_JRZTXYHBZ=@J_JRZTXYHBZ where  B_DLYX=@B_DLYX_t and B_JSZHLX=@B_JSZHLX_t ";
            arryList.Add(strCZValue);
        }
        else
        {
            htinputall["@B_DLYX"] = DLYX;
            htinputall["@B_JSZHLX"] = JSZHLX;
            strSql = "update AAA_DLZHXXB set J_JJRSFZTXYHSH='否',J_JJRZTXYHSHSJ=null where B_DLYX=@B_DLYX and B_JSZHLX=@B_JSZHLX";
            arryList.Add(strSql);

            string strCZ = strSourceValue + "时间：" + DateTime.Now.ToString() + "  操作：恢复新用户审核" + "◆";
            htinputall["@J_JRZTXYHBZ"] = strCZ;
            htinputall["@B_DLYX_t"] = DLYX;
            htinputall["@B_JSZHLX_t"] = JSZHLX;

            string strCZValue = "update AAA_DLZHXXB set J_JRZTXYHBZ=@J_JRZTXYHBZ where  B_DLYX=@B_DLYX_t and B_JSZHLX=@B_JSZHLX_t ";
            arryList.Add(strCZValue);
        }

        Hashtable htreall = I_DBL.RunParam_SQL(arryList,htinputall);
        if ((bool)htreall["return_float"])
        {
            if ((int)htreall["return_other"] > 0)
            {
                if (CZLX == "暂停")
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您已成功执行暂停新交易方审核操作！";

                }
                else
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您已成功执行恢复新交易方审核操作！";
                }
            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "数据受影响行数为0";

            }
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = htreall["return_errmsg"].ToString();
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "";
            return dsreturn;

        }

        return dsreturn;
    }
}