using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using FMOP.DB;
using System.Collections;
using Hesion.Brick.Core.WorkFlow;


/// <summary>
///Manage_SelerBuyerData 的摘要说明
/// </summary>
public class Manage_SelerBuyerData
{
	public Manage_SelerBuyerData()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}
    /// <summary>
    /// 根据角色编号取出买家或卖家的基础数据
    /// </summary>
    /// <param name="jsbh"></param>
    /// <returns></returns>
    public static DataSet GetSelerAndBuyerData(string strJSLX,string jsbh,string jjrjsbh)
    {
        Hashtable UserInfo = jhjx_PublicClass.GetUserInfo(jsbh);
        string SHZT = "";
        if (jsbh.Trim() != "" && UserInfo["是否已被经纪人审核通过"].ToString() == "审核通过")
        {
            SHZT = "审核通过";
        }
        //买家数据
        //string strSqlBuyer = "select '审核状态'='" + SHZT + "','失败' as 执行状态,'未获得买家数据！' as 执行结果, JS.JSLX,MJ.YHM,MJ.DLYX,MJ.LXRXM,MJ.SFZH,MJ.SJH,MJ.SSSF,MJ.SSDS,MJ.SSQX,MJ.XXDZ,MJ.YZBM,MJ.SFZSMJ,JS.KTSHXX,MJ.GSMC,MJ.GSDH,MJ.GSDZ,MJ.YYZZSMJ,MJ.ZZJGDMZSMJ,MJ.SWDJZSMJ,MJ.KHXKZSMJ,MJ.QTZZWJSMJ from ZZ_BUYJBZLXXB as MJ  left join ZZ_YHJSXXB as JS  on MJ.JSBH=JS.JSBH where MJ.JSBH='" + jsbh + "';";
        string strSqlBuyer = "select '失败' as 执行状态,'未获得买家数据！' as 执行结果, MMJ.JSLX,MJ.YHM,MJ.DLYX,MJ.LXRXM,MJ.SFZH,MJ.SJH,MJ.SSSF,MJ.SSDS,MJ.SSQX,MJ.XXDZ,MJ.YZBM,MJ.SFZSMJ,MMJ.DLRSHZT 审核状态,MJ.GSMC,MJ.GSDH,MJ.GSDZ,MJ.YYZZSMJ,MJ.ZZJGDMZSMJ,MJ.SWDJZSMJ,MJ.KHXKZSMJ,MJ.QTZZWJSMJ,MMJ.DLRSHYJ from ZZ_BUYJBZLXXB as MJ  left join ZZ_MMJYDLRZHGLB as MMJ  on MJ.JSBH=MMJ.JSBH where MJ.JSBH='" + jsbh + "' and MMJ.DLRBH='" + jjrjsbh.Trim() + "';";
        //查询卖家数据
        string strSqlSeler = "select '失败' as 执行状态,'未获得买家数据！' as 执行结果,MMJ.JSLX,MJ.YHM,MJ.DLYX,MJ.LXRXM,MJ.SFZH,MJ.SJH,MJ.SSSF,MJ.SSDS,MJ.SSQX,MJ.XXDZ,MJ.YZBM,MJ.FDDBRQZDCNSSMJ,MJ.GSMC,MJ.GSDH,MJ.GSDZ,MJ.YYZZSMJ,MJ.ZZJGDMZSMJ,MJ.SWDJZSMJ,MJ.KHXKZSMJ,MMJ.DLRSHZT 审核状态,MJ.QTZZWJSMJ,MMJ.DLRSHYJ from ZZ_SELJBZLXXB as MJ  left join ZZ_MMJYDLRZHGLB as MMJ on MJ.JSBH=MMJ.JSBH where MJ.JSBH='" + jsbh + "' and MMJ.DLRBH='" + jjrjsbh.Trim() + "'";
        DataSet dataTableBuyerSeller = DbHelperSQL.Query(strSqlBuyer + strSqlSeler);//得到买家卖家数据
   
     if (dataTableBuyerSeller != null && (dataTableBuyerSeller.Tables[0].Rows.Count > 0||dataTableBuyerSeller.Tables[1].Rows.Count>0))//得到买家数据
     {
         dataTableBuyerSeller.Tables[0].TableName = "买家数据表";
         dataTableBuyerSeller.Tables[1].TableName = "卖家数据表";
         if (strJSLX.Contains("买家"))
         {
             dataTableBuyerSeller.Tables["买家数据表"].Rows[0]["执行状态"] = "ok";
             dataTableBuyerSeller.Tables["买家数据表"].Rows[0]["执行结果"] = "ok";
         }
         else if (strJSLX.Contains("卖家"))
         {
             dataTableBuyerSeller.Tables["卖家数据表"].Rows[0]["执行状态"] = "ok";
             dataTableBuyerSeller.Tables["卖家数据表"].Rows[0]["执行结果"] = "ok";
         }

     }
     return dataTableBuyerSeller;
    }

    /// <summary>
    ///审核买家基本信息  审核通过
    /// </summary>
    /// <param name="strData"></param>
    /// <returns></returns>
    public static string[] VerifyBuyerDataPass(DataTable dataTableInfo)
    {
        string[] result = new string[2];
        string SHTG = DateTime.Now.ToString(); //审核通过时间
        //获取角色编号的相关信息       
        Hashtable jjrUserInfo = jhjx_PublicClass.GetUserInfo(dataTableInfo.Rows[0]["经纪人角色编号"].ToString());
       //查询买家基本信息资料表
        DataTable dataTable = DbHelperSQL.Query("select Number,DLYX,YHM,JSZHLX,JSBH,LXRXM,SJH,SSSF,SSDS,SSQX,XXDZ,YZBM,SFZH from ZZ_BUYJBZLXXB where DLYX='" + dataTableInfo.Rows[0]["买家登录账号"].ToString() + "'").Tables[0];
    //一：更新买卖家与经纪人账户关联表
        string strSql1 = "update ZZ_MMJYDLRZHGLB set DLRSHZT ='已审核',DLRSHSJ='" + SHTG + "',DLRSHYJ='" + dataTableInfo.Rows[0]["经纪人审核意见"].ToString() + "' where JSDLZH='" + dataTableInfo.Rows[0]["买家登录账号"].ToString() + "' and DLRBH='" + dataTableInfo.Rows[0]["经纪人角色编号"].ToString() + "'";
        string strSql2 = "";
        /*
   //二.1：查询角色信息表里面是否已经存在  此角色信息
        string strSqll = "select COUNT(*) from ZZ_YHJSXXB  where  and JSBH='" + dataTable.Rows[0]["JSBH"].ToString() + "'  and JSLX='买家'  ";
        int intCount = (int)DbHelperSQL.GetSingle(strSqll);
       
        if (intCount == 0) //角色表里面不存在及，本次操作是第一次审核操作
        {
            //二.2：向用户角色信息表插入数据
            WorkFlowModule YHJSXXBNumber = new WorkFlowModule("ZZ_YHJSXXB");
            string strYHJSXXBNumber = YHJSXXBNumber.numberFormat.GetNextNumber();//生成主键
            strSql2 = "insert into ZZ_YHJSXXB(Number,DLYX,YHM,JSZHLX,JSLX,JSBH,SQKTSJ,KTSHZT,KTSHRLX,FGSKTSHXX,FGSKTSHRBH,FGSKTSHTGSJ,FGSKTSHZT,KTSHRBH,KTSHTGSJ,KTSHXX,DLRZTJSXYH,CheckState,CreateTime,CheckLimitTime) values('" + YHJSXXBNumber + "','" + dataTable.Rows[0]["DLYX"].ToString() + "','" + dataTable.Rows[0]["YHM"].ToString() + "','" + dataTable.Rows[0]["JSZHLX"].ToString() + "','" + dataTable.Rows[0]["JSLX"].ToString() + "','" + dataTable.Rows[0]["JSBH"].ToString() + "',GETDATE(),'审核通过','经纪人','','','','','" + dataTableInfo.Rows[0]["经纪人角色编号"].ToString() + "',GETDATE(),'" + dataTableInfo.Rows[0]["经纪人审核意见"].ToString() + "','否','1',GETDATE(),GETDATE())";
        }
        else if (intCount > 0)
        {
         * */

        string strSQL = "select KTSHZT from ZZ_YHJSXXB where DLYX='" + dataTable.Rows[0]["DLYX"].ToString() + "' and JSBH='" + dataTable.Rows[0]["JSBH"].ToString() + "'";
        object KTSHZT= DbHelperSQL.GetSingle(strSQL);
        if (KTSHZT.ToString() != "审核通过")
        {
            strSql2 = "update ZZ_YHJSXXB set KTSHXX='" + dataTableInfo.Rows[0]["经纪人审核意见"].ToString() + "',KTSHZT='审核通过',KTSHTGSJ='" + SHTG + "',KTSHRBH='" + dataTableInfo.Rows[0]["经纪人角色编号"].ToString() + "' where DLYX='" + dataTable.Rows[0]["DLYX"].ToString() + "' and JSBH='" + dataTable.Rows[0]["JSBH"].ToString() + "'";
        }
        
        
        //}
   //三：向收获地址信息表中插入数据
        //WorkFlowModule WFPKNumber = new WorkFlowModule("ZZ_SHDZXXB");
        //string JFXXBNumber = WFPKNumber.numberFormat.GetNextNumber();//生成主键
        //string strSql3 = "insert ZZ_SHDZXXB(Number,JSDLZH,JSYHM,JSZHLX,JSBH,JSLX,SHDWMC,SHRXM,SZSF,SZDS,SZQX,XXDZ,YZBM,LXDH,SFMRDZ,CheckState,CreateUser,CreateTime,CheckLimitTime) values('" + JFXXBNumber.ToString() + "','" + dataTable.Rows[0]["DLYX"].ToString() + "','" + dataTable.Rows[0]["YHM"].ToString() + "','" + dataTable.Rows[0]["JSZHLX"].ToString() + "','" + dataTable.Rows[0]["JSBH"].ToString() + "','买家','','" + dataTable.Rows[0]["LXRXM"].ToString() + "','" + dataTable.Rows[0]["SSSF"].ToString() + "','" + dataTable.Rows[0]["SSDS"].ToString() + "','" + dataTable.Rows[0]["SSQX"].ToString() + "','" + dataTable.Rows[0]["XXDZ"].ToString() + "','" + dataTable.Rows[0]["YZBM"].ToString() + "','" + dataTable.Rows[0]["SJH"].ToString() + "','是','1','" + dataTableInfo.Rows[0]["经纪人角色编号"].ToString() + "',GETDATE(),GETDATE())";
               ArrayList arrayList = new ArrayList();
               arrayList.Add(strSql1);
               arrayList.Add(strSql2);
               //arrayList.Add(strSql3);
             bool isSuccess = DbHelperSQL.ExecSqlTran(arrayList);
              if (isSuccess)
               {
                   result[0] = "ok";
                   result[1] = "审核信息保存成功！";
               }
               else
               {
                   result[0] = "失败";
                   result[1] = "审核信息保存失败！";
               }
              return result;
    }
    /// <summary>
    ///审核买家基本信息  驳回
    /// </summary>
    /// <param name="dataTableInfo"></param>
    /// <returns></returns>
    public static string[] VerifyBuyerDataReject(DataTable dataTableInfo)
    {
        string[] result = new string[2];
        string SHTG = DateTime.Now.ToString(); ;//驳回时间
        //获取角色编号的相关信息       
        Hashtable jjrUserInfo = jhjx_PublicClass.GetUserInfo(dataTableInfo.Rows[0]["经纪人角色编号"].ToString());
        //查询买家基本信息资料表
        DataTable dataTable = DbHelperSQL.Query("select Number,DLYX,YHM,JSZHLX,JSBH,LXRXM,SJH,SSSF,SSDS,SSQX,XXDZ,YZBM,SFZH from ZZ_BUYJBZLXXB where DLYX='" + dataTableInfo.Rows[0]["买家登录账号"].ToString() + "'").Tables[0];
        //一：更新买卖家与经纪人账户关联表
        string strSql1 = "update ZZ_MMJYDLRZHGLB set DLRSHZT ='待审核',DLRSHSJ='" + SHTG + "',DLRSHYJ='" + dataTableInfo.Rows[0]["经纪人审核意见"].ToString() + "' where JSDLZH='" + dataTableInfo.Rows[0]["买家登录账号"].ToString() + "' and DLRBH='" + dataTableInfo.Rows[0]["经纪人角色编号"].ToString() + "'";
        string strSql2 = "";
        /*
        //二.1：查询角色信息表里面是否已经存在  此角色信息
        string strSqll = "select COUNT(*) from ZZ_YHJSXXB  where  JSBH='" + dataTable.Rows[0]["JSBH"].ToString() + "'  and JSLX='买家'  ";
        int intCount = (int)DbHelperSQL.GetSingle(strSqll);
      
        if (intCount == 0) //角色表里面不存在及，本次操作是第一次审核操作
        {
            //二.2：向用户角色信息表插入数据
            WorkFlowModule YHJSXXBNumber = new WorkFlowModule("ZZ_YHJSXXB");
            string strYHJSXXBNumber = YHJSXXBNumber.numberFormat.GetNextNumber();//生成主键
            strSql2 = "insert into ZZ_YHJSXXB(Number,DLYX,YHM,JSZHLX,JSLX,JSBH,SQKTSJ,KTSHZT,KTSHRLX,FGSKTSHXX,FGSKTSHRBH,FGSKTSHTGSJ,FGSKTSHZT,KTSHRBH,KTSHTGSJ,KTSHXX,DLRZTJSXYH,CheckState,CreateTime,CheckLimitTime) values('" + YHJSXXBNumber + "','" + dataTable.Rows[0]["DLYX"].ToString() + "','" + dataTable.Rows[0]["YHM"].ToString() + "','" + dataTable.Rows[0]["JSZHLX"].ToString() + "','买家','" + dataTable.Rows[0]["JSBH"].ToString() + "',GETDATE(),'驳回','经纪人','','','','','" + dataTableInfo.Rows[0]["经纪人角色编号"].ToString() + "',GETDATE(),'" + dataTableInfo.Rows[0]["经纪人审核意见"].ToString() + "','否','1',GETDATE(),GETDATE())";
        }
        else if (intCount > 0)
        {
         * */
        string strSQL = "select KTSHZT from ZZ_YHJSXXB where DLYX='" + dataTable.Rows[0]["DLYX"].ToString() + "' and JSBH='" + dataTable.Rows[0]["JSBH"].ToString() + "'";
        object KTSHZT= DbHelperSQL.GetSingle(strSQL);
        if (KTSHZT.ToString() != "审核通过")
        {
            strSql2 = "update ZZ_YHJSXXB set KTSHXX='" + dataTableInfo.Rows[0]["经纪人审核意见"].ToString() + "',KTSHZT='驳回',KTSHTGSJ='" + SHTG + "',KTSHRBH='" + dataTableInfo.Rows[0]["经纪人角色编号"].ToString() + "' where DLYX='" + dataTableInfo.Rows[0]["买家登录账号"].ToString() + "' and JSBH='" + dataTable.Rows[0]["JSBH"].ToString() + "'";
        }
        

        //}
        ArrayList arrayList = new ArrayList();
        arrayList.Add(strSql1);
        arrayList.Add(strSql2);
        bool isSuccess = DbHelperSQL.ExecSqlTran(arrayList);
        if (isSuccess)
        {
            result[0] = "ok";
            result[1] = "审核信息保存成功！";
        }
        else
        {
            result[0] = "失败";
            result[1] = "审核信息保存失败！";
        }
        return result;
    }

    /// <summary>
    /// 审核卖家基本信息  审核通过
    /// </summary>
    /// <param name="dataTableInfo"></param>
    /// <returns></returns>
    public static string[] VerifySellerDataPass(DataTable dataTableInfo)
    {
        string[] result = new string[2];
        string SHTG = DateTime.Now.ToString(); ;//审核通过时间
        //获取角色编号的相关信息       
        Hashtable jjrUserInfo = jhjx_PublicClass.GetUserInfo(dataTableInfo.Rows[0]["经纪人角色编号"].ToString());
        //查询卖家基本信息资料表
        DataTable dataTable = DbHelperSQL.Query("select  Number,DLYX,YHM,JSBH,LXRXM,SJH,SSSF,SSDS,SSQX,SSFGS,XXDZ,YZBM,SFZH,GSMC,GSDH,GSDZ,YYZZSMJ,ZZJGDMZSMJ,SWDJZSMJ,KHXKZSMJ,FDDBRQZDCNSSMJ,QTZZWJSMJ,YXKPLX,YXKPXX from  ZZ_SELJBZLXXB where DLYX='" + dataTableInfo.Rows[0]["卖家登录账号"].ToString() + "' ").Tables[0];

        //一：更新买卖家与经纪人账户关联表
        string strSql1 = "update ZZ_MMJYDLRZHGLB set DLRSHZT ='已审核',DLRSHSJ='" + SHTG + "',DLRSHYJ='" + dataTableInfo.Rows[0]["经纪人审核意见"].ToString() + "' where JSDLZH='" + dataTableInfo.Rows[0]["卖家登录账号"].ToString() + "' and DLRBH='" + dataTableInfo.Rows[0]["经纪人角色编号"].ToString() + "'";
        string strSql2 = "";
        /*
        //二.1：查询角色信息表里面是否已经存在  此角色信息
        string strSqll = "select COUNT(*) from ZZ_YHJSXXB  where  and JSBH='" + dataTable.Rows[0]["JSBH"].ToString() + "'  and JSLX='卖家'  ";
        int intCount = (int)DbHelperSQL.GetSingle(strSqll);
       
        
        if (intCount == 0) //角色表里面不存在及，本次操作是第一次审核操作
        {
            //二.2：向用户角色信息表插入数据
            WorkFlowModule YHJSXXBNumber = new WorkFlowModule("ZZ_YHJSXXB");
            string strYHJSXXBNumber = YHJSXXBNumber.numberFormat.GetNextNumber();//生成主键
            strSql2 = "insert into ZZ_YHJSXXB(Number,DLYX,YHM,JSZHLX,JSLX,JSBH,SQKTSJ,KTSHZT,KTSHRLX,FGSKTSHXX,FGSKTSHRBH,FGSKTSHTGSJ,FGSKTSHZT,KTSHRBH,KTSHTGSJ,KTSHXX,DLRZTJSXYH,CheckState,CreateTime,CheckLimitTime) values('" + YHJSXXBNumber + "','" + dataTable.Rows[0]["DLYX"].ToString() + "','" + dataTable.Rows[0]["YHM"].ToString() + "','卖家账户','" + dataTable.Rows[0]["JSLX"].ToString() + "','" + dataTable.Rows[0]["JSBH"].ToString() + "',GETDATE(),'审核通过','经纪人','','','','','" + dataTableInfo.Rows[0]["经纪人角色编号"].ToString() + "',GETDATE(),'" + dataTableInfo.Rows[0]["经纪人审核意见"].ToString() + "','否','1',GETDATE(),GETDATE())";
        }
        else if (intCount > 0)
        {
         * */
            //二.3：向角色信息表更新数据
        strSql2 = "update ZZ_YHJSXXB set KTSHXX='" + dataTableInfo.Rows[0]["经纪人审核意见"].ToString() + "',KTSHZT='审核通过',KTSHTGSJ='" + SHTG + "',KTSHRBH='" + dataTableInfo.Rows[0]["经纪人角色编号"].ToString() + "' where  JSBH='" + dataTable.Rows[0]["JSBH"].ToString() + "'";

        //}
        /*
        //三：向收获地址信息表中插入数据
        WorkFlowModule WFPKNumber = new WorkFlowModule("ZZ_SHDZXXB");
        string JFXXBNumber = WFPKNumber.numberFormat.GetNextNumber();//生成主键
        string strSql3 = "insert ZZ_SHDZXXB(Number,JSDLZH,JSYHM,JSZHLX,JSBH,JSLX,SHDWMC,SHRXM,SZSF,SZDS,SZQX,XXDZ,YZBM,LXDH,SFMRDZ,CheckState,CreateUser,CreateTime,CheckLimitTime) values('" + JFXXBNumber.ToString() + "','" + dataTable.Rows[0]["DLYX"].ToString() + "','" + dataTable.Rows[0]["YHM"].ToString() + "','卖家账户','" + dataTable.Rows[0]["JSBH"].ToString() + "','卖家','','" + dataTable.Rows[0]["LXRXM"].ToString() + "','" + dataTable.Rows[0]["SSSF"].ToString() + "','" + dataTable.Rows[0]["SSDS"].ToString() + "','" + dataTable.Rows[0]["SSQX"].ToString() + "','" + dataTable.Rows[0]["XXDZ"].ToString() + "','" + dataTable.Rows[0]["YZBM"].ToString() + "','" + dataTable.Rows[0]["SJH"].ToString() + "','是','1','" + dataTableInfo.Rows[0]["经纪人角色编号"].ToString() + "',GETDATE(),GETDATE())";
       //四：买家基本信息表里面插入一条数据
        WorkFlowModule BUYJBZLXXBNumber = new WorkFlowModule("ZZ_BUYJBZLXXB");
        string strBUYJBZLXXBNumber = BUYJBZLXXBNumber.numberFormat.GetNextNumber();//生成主键
        //生成买家角色编号
        string strBuyerRole = jhjx_PublicClass.GetNextNumberZZ("ZZ_YHJSXXB","C");
        string sqlMJJBJSXXB = "insert ZZ_BUYJBZLXXB(Number,DLYX,YHM,JSZHLX,JSBH,LXRXM,SJH,SFYZSJH,SSSF,SSDS,SSQX,SSFGS,XXDZ,YZBM,SFZH,SFZSMJ,CheckState,CreateUser,CreateTime,CheckLimitTime) values('" + strBUYJBZLXXBNumber + "','" + dataTable.Rows[0]["DLYX"].ToString() + "','" + dataTable.Rows[0]["YHM"].ToString() + "','买家账户','" + strBuyerRole + "','" + dataTable.Rows[0]["LXRXM"].ToString() + "','" + dataTable.Rows[0]["SJH"].ToString() + "','否','" + dataTable.Rows[0]["SSSF"].ToString() + "','" + dataTable.Rows[0]["SSDS"].ToString() + "','" + dataTable.Rows[0]["SSQX"].ToString() + "','" + dataTable.Rows[0]["SSFGS"].ToString() + "','" + dataTable.Rows[0]["XXDZ"].ToString() + "','" + dataTable.Rows[0]["YZBM"].ToString() + "','" + dataTable.Rows[0]["SFZH"].ToString() + "','','1','" + dataTableInfo.Rows[0]["经纪人角色编号"].ToString() + "',GETDATE(),GETDATE())";
        //五：向基本角色信息表里面插入一条数据
        WorkFlowModule buyerJSNumber = new WorkFlowModule("ZZ_YHJSXXB");
        string strbuyerJSNumber = buyerJSNumber.numberFormat.GetNextNumber();//生成主键
        //向角色信息表中插入数据
        string strJSXXB = "insert ZZ_YHJSXXB(Number,DLYX,YHM,JSZHLX,JSLX,JSBH,SQKTSJ,KTSHZT,FGSKTSHZT,KTSHRLX,KTSHRBH,FGSKTSHRBH,KTSHTGSJ,FGSKTSHTGSJ,KTSHXX,FGSKTSHXX,DLRZTJSXYH,CheckState,CreateUser,CreateTime,CheckLimitTime)  values('" + strbuyerJSNumber + "','" + dataTable.Rows[0]["DLYX"].ToString() + "','" + dataTable.Rows[0]["YHM"].ToString() + "','买家账户','买家','" + strBuyerRole + "',GETDATE(),'审核通过','','经纪人','" + dataTableInfo.Rows[0]["经纪人角色编号"].ToString() + "','',GETDATE(),'','','','否','1','" + dataTableInfo.Rows[0]["经纪人角色编号"].ToString() + "',GETDATE(),GETDATE())";
        //六：向买、卖家与经纪人关联表中插入一条数据
        WorkFlowModule MMJYDLRZHGLBNumber = new WorkFlowModule("ZZ_MMJYDLRZHGLB");
        string strMMJYDLRZHGLBNumber = MMJYDLRZHGLBNumber.numberFormat.GetNextNumber();//生成主键
        string strGLB = "insert ZZ_MMJYDLRZHGLB(Number,JSDLZH,JSYHM,JSZHLX,JSBH,JSLX,DLRDLZH,DLRBH,DLRYHM,SFMRDLR,SQGLSJ,DLRSHZT,DLRSHSJ,DLRSHYJ,GLZT,SFZTXYW,CheckState,CreateUser,CreateTime,CheckLimitTime)  values('" + strMMJYDLRZHGLBNumber + "','" + dataTable.Rows[0]["DLYX"].ToString() + "','" + dataTable.Rows[0]["YHM"].ToString() + "','买家账户','" + strBuyerRole + "','买家','" + jjrUserInfo["登录邮箱"].ToString() + "','" + dataTableInfo.Rows[0]["经纪人角色编号"].ToString() + "','" + jjrUserInfo["用户名"].ToString() + "','是',GETDATE(),'已审核',GETDATE(),'','有效','否','1','" + dataTableInfo.Rows[0]["经纪人角色编号"].ToString() + "',GETDATE(),GETDATE())";
         */ 

      
     

        ArrayList arrayList = new ArrayList();
        arrayList.Add(strSql1);
        arrayList.Add(strSql2);
        //arrayList.Add(strSql3);
        //arrayList.Add(sqlMJJBJSXXB);
        //arrayList.Add(strJSXXB);
        //arrayList.Add(strGLB);
        bool isSuccess = DbHelperSQL.ExecSqlTran(arrayList);
        if (isSuccess)
        {
            result[0] = "ok";
            result[1] = "审核信息保存成功！";
        }
        else
        {
            result[0] = "失败";
            result[1] = "审核信息保存失败！";
        }
        return result;
    
     
    }

    /// <summary>
    /// 审核卖家基本信息  驳回
    /// </summary>
    /// <param name="dataTableInfo"></param>
    /// <returns></returns>
    public static string[] VerifySellerDataReject(DataTable dataTableInfo)
    {

        string[] result = new string[2];
        string SHTG = DateTime.Now.ToString(); ;//驳回时间
        //获取角色编号的相关信息       
        Hashtable jjrUserInfo = jhjx_PublicClass.GetUserInfo(dataTableInfo.Rows[0]["经纪人角色编号"].ToString());
        //查询卖家基本信息资料表
        DataTable dataTable = DbHelperSQL.Query("select  Number,DLYX,YHM,JSBH,LXRXM,SJH,SSSF,SSDS,SSQX,SSFGS,XXDZ,YZBM,SFZH,GSMC,GSDH,GSDZ,YYZZSMJ,ZZJGDMZSMJ,SWDJZSMJ,KHXKZSMJ,FDDBRQZDCNSSMJ,QTZZWJSMJ,YXKPLX,YXKPXX from  ZZ_SELJBZLXXB where DLYX='" + dataTableInfo.Rows[0]["卖家登录账号"].ToString() + "' ").Tables[0];
        //一：更新买卖家与经纪人账户关联表
        string strSql1 = "update ZZ_MMJYDLRZHGLB set DLRSHZT ='驳回',DLRSHSJ='" + SHTG + "',DLRSHYJ='" + dataTableInfo.Rows[0]["经纪人审核意见"].ToString() + "' where JSDLZH='" + dataTableInfo.Rows[0]["卖家登录账号"].ToString() + "' and DLRBH='" + dataTableInfo.Rows[0]["经纪人角色编号"].ToString() + "'";
        string strSql2 = "";
        /*
        //二.1：查询角色信息表里面是否已经存在  此角色信息
        string strSqll = "select COUNT(*) from ZZ_YHJSXXB  where  JSBH='" + dataTable.Rows[0]["JSBH"].ToString() + "'  and JSLX='卖家'  ";
        int intCount = (int)DbHelperSQL.GetSingle(strSqll);
     
        if (intCount == 0) //角色表里面不存在及，本次操作是第一次审核操作
        {
            //二.2：向用户角色信息表插入数据
            WorkFlowModule YHJSXXBNumber = new WorkFlowModule("ZZ_YHJSXXB");
            string strYHJSXXBNumber = YHJSXXBNumber.numberFormat.GetNextNumber();//生成主键
            strSql2 = "insert into ZZ_YHJSXXB(Number,DLYX,YHM,JSZHLX,JSLX,JSBH,SQKTSJ,KTSHZT,KTSHRLX,FGSKTSHXX,FGSKTSHRBH,FGSKTSHTGSJ,FGSKTSHZT,KTSHRBH,KTSHTGSJ,KTSHXX,DLRZTJSXYH,CheckState,CreateTime,CheckLimitTime) values('" + YHJSXXBNumber + "','" + dataTable.Rows[0]["DLYX"].ToString() + "','" + dataTable.Rows[0]["YHM"].ToString() + "','卖家账户','卖家','" + dataTable.Rows[0]["JSBH"].ToString() + "',GETDATE(),'驳回','经纪人','','','','','" + dataTableInfo.Rows[0]["经纪人角色编号"].ToString() + "',GETDATE(),'" + dataTableInfo.Rows[0]["经纪人审核意见"].ToString() + "','否','1',GETDATE(),GETDATE())";
        }
        else if (intCount > 0)
        {
         * */
        strSql2 = "update ZZ_YHJSXXB set KTSHXX='" + dataTableInfo.Rows[0]["经纪人审核意见"].ToString() + "',KTSHZT='驳回',KTSHTGSJ=GETDATE(),KTSHRBH='" + dataTableInfo.Rows[0]["经纪人角色编号"].ToString() + "' where DLYX='" + dataTableInfo.Rows[0]["卖家登录账号"].ToString() + "' and JSBH='" + dataTable.Rows[0]["JSBH"].ToString() + "'";

        //}
        ArrayList arrayList = new ArrayList();
        arrayList.Add(strSql1);
        arrayList.Add(strSql2);
        bool isSuccess = DbHelperSQL.ExecSqlTran(arrayList);
        if (isSuccess)
        {
            result[0] = "ok";
            result[1] = "审核信息保存成功！";
        }
        else
        {
            result[0] = "失败";
            result[1] = "审核信息保存失败！";
        }
        return result;
    }




}