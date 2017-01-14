using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Collections;
using FMOP.DB;

/// <summary>
///Manage_Jszhchuli 的摘要说明
/// </summary>
public class Manage_Jszhchuli
{
	public Manage_Jszhchuli()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dt"> 需插入信息</param>
    /// <param name="Type">数据存储类型</param>
    /// <returns></returns>
    public   string  isDOPass( DataTable dt,string Type )
    {
        string  ispass = "注册失败！";

        string DZ = "";
        try
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                //获取所属分公司，以及分公司地址
                
                if (Type.Trim().Equals("insert"))
                {
                    //经纪人通过区域对接分公司
                    if (dt.Rows[0]["结算账户类型"].ToString().Trim().Equals("经纪人账户"))
                    {
                        DataSet dsFGS = DbHelperSQL.Query("select top 1 '分公司名称'=FGS.FGSname,'办事处名称'=FGS.BSCname,'省'=FGS.Pname,'市'=FGS.Cname,'地址'=DZ.LXDZ,'电话'=DZ.LXDH,'邮编'=DZ.YZBM  from ZZ_CityList_FGS as FGS join ZZ_FGSLXFSB as DZ on FGS.FGSname = DZ.FGSMC and FGS.BSCname=DZ.BSCMC   where FGS.Pname='" + dt.Rows[0]["省"].ToString() + "' and FGS.Cname='" + dt.Rows[0]["市"].ToString() + "'");
                        dt.Rows[0]["所属分公司"] = dsFGS.Tables[0].Rows[0]["分公司名称"].ToString();

                        DZ = "〓" + dsFGS.Tables[0].Rows[0]["分公司名称"].ToString() + "〓" + dsFGS.Tables[0].Rows[0]["地址"].ToString() + "〓" + dsFGS.Tables[0].Rows[0]["电话"].ToString() + "〓" + dsFGS.Tables[0].Rows[0]["邮编"].ToString() + "";
                    }
                    else //其他，通过经纪人所在分公司，对接分公司
                    {
                        string jingjirenJSBH = dt.Rows[0]["关联经纪人编号"].ToString();
                        DataSet dsFGS = DbHelperSQL.Query("select top 1 '分公司名称'=FGS.FGSname,'办事处名称'=FGS.BSCname,'省'=FGS.Pname,'市'=FGS.Cname,'地址'=DZ.LXDZ,'电话'=DZ.LXDH,'邮编'=DZ.YZBM  from ZZ_CityList_FGS as FGS join ZZ_FGSLXFSB as DZ on FGS.FGSname = DZ.FGSMC and FGS.BSCname=DZ.BSCMC where FGS.FGSname = (select top 1 SSFGS from ZZ_DLRJBZLXXB where JSBH = '" + jingjirenJSBH + "')");

                        dt.Rows[0]["所属分公司"] = dsFGS.Tables[0].Rows[0]["分公司名称"].ToString();

                        DZ = "〓" + dsFGS.Tables[0].Rows[0]["分公司名称"].ToString() + "〓" + dsFGS.Tables[0].Rows[0]["地址"].ToString() + "〓" + dsFGS.Tables[0].Rows[0]["电话"].ToString() + "〓" + dsFGS.Tables[0].Rows[0]["邮编"].ToString() + "";

                    }
                }
                

                ArrayList al = new ArrayList();
               // string StrSql = "";

                al = Alsql(dt, Type);
                if (al != null && al.Count > 0)
                {
                    
                    ispass = DbHelperSQL.ExecSqlTran(al).ToString();
                }

            }
        }
        catch (Exception ex)
        {
            ispass = ex.ToString();
        }


        return ispass + DZ;
    }


    private ArrayList Alsql(DataTable dt,string Strtype)
    {
        ArrayList al = new ArrayList();
       
        DataRow dr = dt.Rows[0];

        string StrglbNumber = "";
        string Number = "";
      
        //角色编号生成
        string Strjjr = "";
        string Strjsbm = "";
        if (Strtype.Trim().Equals("insert"))
        {



            Hashtable htjjr = jhjx_PublicClass.GetUserInfo(dr["关联经纪人编号"].ToString());
            if (dr["结算账户类型"].ToString().Trim().Equals("经纪人账户"))
            {
                 Number = jhjx_PublicClass.GetNextNumberZZ("ZZ_DLRJBZLXXB", "");
                 string Time = DateTime.Now.ToString();

                //角色编号生成
                 Strjjr = jhjx_PublicClass.GetNextNumberZZ("ZZ_YHJSXXB", "");
                 Strjsbm = "D" + Strjjr.Trim();
                // string Strmj = jhjx_PublicClass.GetNextNumberZZ("ZZ_YHJSXXB", "C");


                //经纪人角色表数据插入  

                 string Strsqlinsertjjrjs = "insert into ZZ_YHJSXXB ( Number, DLYX,YHM,JSZHLX,JSLX,JSBH,SQKTSJ,KTSHZT,KTSHRLX,DLRZTJSXYH,FGSKTSHZT,CreateUser,CreateTime) values ('" + Strjjr + "','" + dr["登陆邮箱"].ToString() + "','" + dr["用户名"].ToString() + "','" + dr["结算账户类型"].ToString() + "','经纪人','" + Strjsbm + "','" + Time + "','待审核','平台管理员','否','待审核','" + dr["登陆邮箱"].ToString() + "','" + Time + "')";
                al.Add(Strsqlinsertjjrjs);

                //插入经纪人基本信息表 
                string StrSqlinsertjjr = " insert into ZZ_DLRJBZLXXB (Number,DLYX,YHM,JSBH,LXRXM,SJH,SFYZSJH,SJHYZM,SSSF,SSDS,SSQX,SSFGS,XXDZ,YZBM,SFZH,ZCLX,SFZSMJ,GSMC,GSDH,GSDZ,YYZZSMJ,ZZJGDMZSMJ,SWDJZSMJ,KHXKZSMJ,FDDBRQZDCNSSMJ,QTZZWJSMJ,CreateUser,CreateTime) values ('" + Number + "','" + dr["登陆邮箱"].ToString() + "','" + dr["用户名"].ToString() + "','" + Strjsbm + "','" + dr["联系人姓名"].ToString() + "','" + dr["手机号"].ToString() + "','" + dr["是否验证手机号"].ToString() + "','" + dr["手机号验证码"].ToString() + "','" + dr["省"].ToString() + "','" + dr["市"].ToString() + "','" + dr["区"].ToString() + "','" + dr["所属分公司"].ToString() + "','" + dr["详细地址"].ToString() + "','" + dr["邮政编码"].ToString() + "','" + dr["身份证号码"].ToString() + "','" + dr["注册类别"].ToString() + "','" + dr["身份证扫描件"].ToString() + "','" + dr["公司名称"].ToString() + "','" + dr["公司电话"].ToString() + "','" + dr["公司地址"].ToString() + "','" + dr["营业执照"].ToString() + "','" + dr["组织机构代码证"].ToString() + "','" + dr["税务登记证"].ToString() + "','" + dr["开户许可证"].ToString() + "','" + dr["签字承诺书"].ToString() + "','" + dr["其他资质文件"].ToString() + "','" + dr["登陆邮箱"].ToString() + "','" + Time + "')  ";
                al.Add(StrSqlinsertjjr);
            }
            else if (dr["结算账户类型"].ToString().Trim().Equals("卖家账户"))
            {
                //买家基本信息Number
                Number = jhjx_PublicClass.GetNextNumberZZ("ZZ_SELJBZLXXB", "");
                string Time = DateTime.Now.ToString();
                //角色编号生成
                Strjjr = jhjx_PublicClass.GetNextNumberZZ("ZZ_YHJSXXB", "");
                Strjsbm = "B" + Strjjr.Trim();

                //关联表
                StrglbNumber = jhjx_PublicClass.GetNextNumberZZ("ZZ_MMJYDLRZHGLB", "");

                //卖家基本信息表插入 手机号验证通过时间
                string StrmjBinfo = "insert into ZZ_SELJBZLXXB(Number,DLYX,YHM,JSBH,LXRXM,SJH,SFYZSJH,SJHYZM,SJYZTGSJ,SSSF,SSDS,SSQX,SSFGS,XXDZ,YZBM,SFZH,GSMC,GSDH,GSDZ,YYZZSMJ,ZZJGDMZSMJ,SWDJZSMJ,KHXKZSMJ,FDDBRQZDCNSSMJ,QTZZWJSMJ,CreateUser,CreateTime) values ('" + Number + "','" + dr["登陆邮箱"].ToString() + "','" + dr["用户名"].ToString() + "','" + Strjsbm + "','" + dr["联系人姓名"].ToString() + "','" + dr["手机号"].ToString() + "','" + dr["是否验证手机号"].ToString() + "','" + dr["手机号验证码"].ToString() + "','" + dr["手机号验证通过时间"].ToString() + "','" + dr["省"].ToString() + "','" + dr["市"].ToString() + "','" + dr["区"].ToString() + "','" + dr["所属分公司"].ToString() + "','" + dr["详细地址"].ToString() + "','" + dr["邮政编码"].ToString() + "','" + dr["身份证号码"].ToString() + "','" + dr["公司名称"].ToString() + "','" + dr["公司电话"].ToString() + "','" + dr["公司地址"].ToString() + "','" + dr["营业执照"].ToString() + "','" + dr["组织机构代码证"].ToString() + "','" + dr["税务登记证"].ToString() + "','" + dr["开户许可证"].ToString() + "','" + dr["签字承诺书"].ToString() + "','" + dr["其他资质文件"].ToString() + "','" + dr["登陆邮箱"].ToString() + "','" + Time + "')";
                al.Add(StrmjBinfo);


              
                //关联表插入
                string StrSqlglb = "insert into ZZ_MMJYDLRZHGLB( Number,JSDLZH,JSYHM,JSZHLX,JSBH,JSLX ,DLRDLZH,DLRBH,DLRYHM,SFMRDLR,SQGLSJ,DLRSHZT,FGSSHZT,GLZT,SFZTXYW,CreateUser,CreateTime) values ('" + Number + "','" + dr["登陆邮箱"].ToString() + "','" + dr["用户名"].ToString() + "','" + dr["结算账户类型"].ToString() + "','" + Strjsbm + "','卖家','" + htjjr["登录邮箱"].ToString() + "','" + dr["关联经纪人编号"].ToString() + "','" + dr["关联代理人用户名"].ToString() + "','是','" + Time + "','待审核','待审核','有效','否','" + dr["登陆邮箱"].ToString() + "','" + Time + "')";

                al.Add(StrSqlglb);

                //角色表插入
                string Strsqljs = "insert into ZZ_YHJSXXB ( Number, DLYX,YHM,JSZHLX,JSLX,JSBH,SQKTSJ,KTSHZT,KTSHRLX,DLRZTJSXYH,FGSKTSHZT) values ('" + Strjjr + "','" + dr["登陆邮箱"].ToString() + "','" + dr["用户名"].ToString() + "','" + dr["结算账户类型"].ToString() + "','卖家','" + Strjsbm + "','" + Time + "','待审核','经纪人','否','待审核') ";

                al.Add(Strsqljs);

            }
            else if (dr["结算账户类型"].ToString().Trim().Equals("买家账户"))
            { 
                Number = jhjx_PublicClass.GetNextNumberZZ("ZZ_BUYJBZLXXB", "");
                string Time = DateTime.Now.ToString();
                Strjjr = jhjx_PublicClass.GetNextNumberZZ("ZZ_YHJSXXB", "");

                Strjsbm = "C" + Strjjr.Trim();

                //买家基本资料插入
                string Strinsertmj = "insert into  ZZ_BUYJBZLXXB ( Number,DLYX,YHM,JSZHLX,JSBH,LXRXM,SJH,SFYZSJH, SJHYZM,SSSF,SSDS,SSQX,SSFGS,XXDZ,YZBM,SFZH,SFZSMJ,CreateUser,CreateTime,SJYZMGQSJ,GSMC,GSDH,GSDZ,YYZZSMJ,ZZJGDMZSMJ,SWDJZSMJ,KHXKZSMJ,QTZZWJSMJ  ) values ('" + Number + "','" + dr["登陆邮箱"].ToString() + "','" + dr["用户名"].ToString() + "','" + dr["结算账户类型"].ToString() + "','" + Strjsbm + "','" + dr["联系人姓名"].ToString() + "','" + dr["手机号"].ToString() + "','" + dr["是否验证手机号"].ToString() + "','" + dr["手机号验证码"].ToString() + "','" + dr["省"].ToString() + "','" + dr["市"].ToString() + "','" + dr["区"].ToString() + "','" + dr["所属分公司"].ToString() + "','" + dr["详细地址"].ToString() + "','" + dr["邮政编码"].ToString() + "','" + dr["身份证号码"].ToString() + "','" + dr["身份证扫描件"].ToString() + "','" + dr["登陆邮箱"].ToString() + "','" + Time + "','" + dr["手机号验证通过时间"].ToString() + "','" + dr["公司名称"].ToString().Trim() + "','" + dr["公司电话"].ToString() + "','" + dr["公司地址"].ToString() + "','" + dr["营业执照"].ToString() + "','" + dr["组织机构代码证"].ToString() + "','" + dr["税务登记证"].ToString() + "','" + dr["开户许可证"].ToString() + "','" + dr["其他资质文件"].ToString() + "') ";
                al.Add(Strinsertmj);



                //关联表插入
                string StrSqlglb = "insert into ZZ_MMJYDLRZHGLB( Number,JSDLZH,JSYHM,JSZHLX,JSBH,JSLX ,DLRDLZH,DLRBH,DLRYHM,SFMRDLR,SQGLSJ,DLRSHZT,FGSSHZT,GLZT,SFZTXYW,CreateUser,CreateTime) values ('" + Number + "','" + dr["登陆邮箱"].ToString() + "','" + dr["用户名"].ToString() + "','" + dr["结算账户类型"].ToString() + "','" + Strjsbm + "','买家','" + htjjr["登录邮箱"].ToString() + "','" + dr["关联经纪人编号"].ToString() + "','" + dr["关联代理人用户名"].ToString() + "','是','" + Time + "','待审核','待审核','有效','否','" + dr["登陆邮箱"].ToString() + "','" + Time + "')";

                al.Add(StrSqlglb);


                //insert into ZZ_YHJSXXB ( Number, DLYX,YHM,JSZHLX,JSLX,JSBH,SQKTSJ,KTSHZT,KTSHRLX,DLRZTJSXYH,FGSKTSHTGSJ,FGSKTSHZT,CreateUser,CreateTime) values ('" + Strjjr + "','" + dr["登陆邮箱"].ToString() + "','" + dr["用户名"].ToString() + "','" + dr["结算账户类型"].ToString() + "','经纪人','" + Strjsbm + "',GETDATE(),'待审核','平台管理员','否',GETDATE(),'待审核','" + dr["登陆邮箱"].ToString() + "',GETDATE())

                 //角色表插入
                string Strsqljs = "insert into ZZ_YHJSXXB ( Number, DLYX,YHM,JSZHLX,JSLX,JSBH,SQKTSJ,KTSHZT,KTSHRLX,DLRZTJSXYH,FGSKTSHZT) values ('" + Strjjr + "','" + dr["登陆邮箱"].ToString() + "','" + dr["用户名"].ToString() + "','" + dr["结算账户类型"].ToString() + "','买家','" + Strjsbm + "','" + Time + "','待审核','经纪人','否','待审核') ";

                al.Add(Strsqljs);




            }

            string Strupdatedlxxinfo = "update ZZ_UserLogin set JSZHLX = '" + dr["结算账户类型"].ToString() + "' where DLYX='" + dr["登陆邮箱"].ToString() + "'  and YHM='" + dr["用户名"].ToString() + "' ";

            al.Add(Strupdatedlxxinfo);
        }
        else if (Strtype.Trim().Equals("update"))
        {
            //修改信息
            if (dr["结算账户类型"].ToString().Trim() == "经纪人账户")
            {
                string Strupdatejjr = "update ZZ_DLRJBZLXXB set LXRXM = '" + dr["联系人姓名"].ToString() + "',SFZH = '" + dr["身份证号码"].ToString() + "',SJH = '" + dr["手机号码"].ToString() + "',SSSF = '" + dr["省"].ToString() + "',SSDS='" + dr["市"].ToString() + "',SSQX='" + dr["区"].ToString() + "',XXDZ='" + dr["详细地址"].ToString() + "',YZBM='" + dr["邮政编码"].ToString() + "',SFZSMJ = '" + dr["身份证扫描件"].ToString() + "',GSMC='" + dr["公司名称"].ToString() + "',GSDH = '" + dr["公司电话"].ToString() + "',GSDZ='" + dr["公司地址"].ToString() + "',YYZZSMJ = '" + dr["营业执照"].ToString() + "',ZZJGDMZSMJ = '" + dr["组织机构代码"].ToString() + "',SWDJZSMJ='" + dr["税务登记扫描件"].ToString() + "',KHXKZSMJ = '" + dr["开户许可证扫描件"].ToString() + "',FDDBRQZDCNSSMJ = '" + dr["承诺书"].ToString() + "',ZCLX='" + dr["注册类型"].ToString() + "' where JSBH='" + dr["角色编号"].ToString() + "'";
                al.Add(Strupdatejjr);
                string Strggshzt = " update ZZ_YHJSXXB set FGSKTSHZT='待审核' ,KTSHZT='待审核' where JSBH='" + dr["角色编号"].ToString() + "' ";
                al.Add(Strggshzt);
            }
            else if(dr["结算账户类型"].ToString().Trim() == "卖家账户")
            {
                string Strupdate = "update ZZ_SELJBZLXXB set  LXRXM='" + dr["联系人姓名"].ToString() + "',SJH='" + dr["手机号码"].ToString() + "',SSSF='" + dr["省"].ToString() + "',SSDS='" + dr["市"].ToString() + "',SSQX='" + dr["区"].ToString() + "',XXDZ='" + dr["详细地址"].ToString() + "',YZBM='" + dr["邮政编码"].ToString() + "',GSMC='" + dr["公司名称"].ToString() + "',GSDH = '" + dr["公司电话"].ToString() + "',GSDZ = '" + dr["公司地址"].ToString() + "',YYZZSMJ = '" + dr["营业执照"].ToString() + "',ZZJGDMZSMJ = '" + dr["组织机构代码"].ToString() + "',SWDJZSMJ='" + dr["税务登记扫描件"].ToString() + "',KHXKZSMJ = '" + dr["开户许可证扫描件"].ToString() + "',FDDBRQZDCNSSMJ = '" + dr["承诺书"].ToString() + "',QTZZWJSMJ = '" + dr["其他资质文件"].ToString() + "' where JSBH='" + dr["角色编号"].ToString() + "'";
                al.Add(Strupdate);
                string Strggshzt = " update ZZ_YHJSXXB set  FGSKTSHZT='待审核' ,KTSHZT='待审核' where JSBH='" + dr["角色编号"].ToString() + "' ";
                al.Add(Strggshzt);
            }
            else if (dr["结算账户类型"].ToString().Trim() == "买家账户")
            {
                string Updatemaijia = "update ZZ_BUYJBZLXXB set LXRXM='" + dr["联系人姓名"].ToString() + "',SFZH='" + dr["身份证号码"].ToString() + "',SJH='" + dr["手机号码"].ToString() + "',SSSF='" + dr["省"].ToString() + "',SSDS='" + dr["市"].ToString() + "',SSQX='" + dr["区"].ToString() + "',XXDZ='" + dr["详细地址"].ToString() + "',YZBM='" + dr["邮政编码"].ToString() + "',SFZSMJ='" + dr["身份证扫描件"].ToString() + "',GSMC='" + dr["公司名称"].ToString() + "',GSDH='" + dr["公司电话"].ToString() + "', GSDZ='" + dr["公司地址"].ToString() + "',YYZZSMJ='" + dr["营业执照"].ToString() + "',ZZJGDMZSMJ='" + dr["组织机构代码"].ToString() + "',SWDJZSMJ='" + dr["税务登记扫描件"].ToString() + "',KHXKZSMJ='" + dr["开户许可证扫描件"].ToString() + "',QTZZWJSMJ='" + dr["其他资质文件"].ToString() + "' where JSBH = '" + dr["角色编号"].ToString() + "'";
              al.Add(Updatemaijia);
              string Strggshzt = " update ZZ_YHJSXXB set FGSKTSHZT='待审核' ,KTSHZT='待审核' where JSBH='" + dr["角色编号"].ToString() + "' ";
              al.Add(Strggshzt);

            }
        }


        return al;
    }

    public  DataSet Getjjrjinfo(string Strjjrjszh)
    {
        DataSet ds = null;
        //string Strgetinfo = " select JSBH as 角色编号, DLYX as 登陆邮箱, YHM as 用户名 ,JSZHLX as 结算账户类型,JSLX as 角色类型, KTSHZT as 开通审核状态 ,FGSKTSHZT as 分公司开通审核状态, DLRZTJSXYH as 代理人暂停接受新用户 from ZZ_YHJSXXB where YHM='" + Strjjrjszh + "' and JSZHLX='经纪人账户' ";
        string Strgetinfo = " select a.JSBH as 角色编号, a.DLYX as 登陆邮箱, a.YHM as 用户名 ,a.JSZHLX as 结算账户类型,a.JSLX as 角色类型, a.KTSHZT as 开通审核状态 ,a.FGSKTSHZT as 分公司开通审核状态, a.DLRZTJSXYH as 代理人暂停接受新用户,b.SFYXDL 是否允许登陆,b.SFDJZH 是否冻结账号,b.SFZTXYW 禁止新业务,b.SFXM 是否休眠 from ZZ_YHJSXXB a  left join ZZ_UserLogin b on a.DLYX =b.DLYX where  a.YHM='" + Strjjrjszh + "' and  a.JSZHLX='经纪人账户' and a.JSLX='经纪人' ";
       ds= DbHelperSQL.Query(Strgetinfo);
       
        return ds;
       
    }


    //获取用户基本资料与资质数据
    public DataSet Get_BasicInfo(string dlyx, string jszhlx)
    {
        string sql_select = "";
        if (jszhlx == "卖家账户")
        {
            sql_select = "select b.KTSHZT as 审核状态,b.KTSHXX as 审核信息,a.Number,a.DLYX as 登陆邮箱,a.YHM as 用户名,a.JSBH as 角色编号,a.LXRXM as 联系人姓名,a.SJH as 手机号,a.SFYZSJH as 是否验证手机号,a.SSSF as 所属省份,a.SSDS as 所属地市,a.SSQX as 所属区县,a.XXDZ as 详细地址,a.YZBM as 邮政编码,a.SFZH as 身份证号,a.GSMC as 公司名称,a.GSDH as 公司电话,a.GSDZ as 公司地址,a.YYZZSMJ as 营业执照,a.ZZJGDMZSMJ as 组织机构代码证,a.SWDJZSMJ as 税务登记证,a.KHXKZSMJ as 开户许可证 ,a.FDDBRQZDCNSSMJ as 签字承诺书,a.QTZZWJSMJ as 其他资质,'' as 注册类别,'' as 身份证扫描件,b.FGSKTSHXX as 分公司审核信息,FGSKTSHZT as 分公司审核状态 from ZZ_SELJBZLXXB as a left join ZZ_YHJSXXB as b on a.JSBH=b.JSBH where a.DLYX='" + dlyx + "'";
        }
        else if (jszhlx == "买家账户")
        {
            //sql_select = "select b.KTSHZT as 审核状态,b.KTSHXX as 审核信息,a.Number,a.DLYX as 登陆邮箱,a.YHM as 用户名,a.JSBH as 角色编号,a.LXRXM as 联系人姓名,a.SJH as 手机号,a.SFYZSJH as 是否验证手机号,a.SSSF as 所属省份,a.SSDS as 所属地市,a.SSQX as 所属区县,a.XXDZ as 详细地址,a.YZBM as 邮政编码,a.SFZH as 身份证号,GSMC as 公司名称,GSDH as 公司电话,GSDZ as 公司地址,YYZZSMJ as 营业执照,ZZJGDMZSMJ as 组织机构代码证,SWDJZSMJ as 税务登记证,KHXKZSMJ as 开户许可证 ,'' as 签字承诺书,QTZZWJSMJ as 其他资质,'' as 注册类别,a.SFZSMJ as 身份证扫描件,b.FGSKTSHXX as 分公司审核信息,FGSKTSHZT as 分公司审核状态  from ZZ_BUYJBZLXXB as a left join ZZ_YHJSXXB as b on a.JSBH=b.JSBH where a.dlyx='" + dlyx + "'";
            sql_select = "select b.KTSHZT as 审核状态,b.KTSHXX as 审核信息,a.Number,a.DLYX as 登陆邮箱,a.YHM as 用户名,a.JSBH as 角色编号,a.LXRXM as 联系人姓名,a.SJH as 手机号,a.SFYZSJH as 是否验证手机号,a.SSSF as 所属省份,a.SSDS as 所属地市,a.SSQX as 所属区县,a.XXDZ as 详细地址,a.YZBM as 邮政编码,a.SFZH as 身份证号,GSMC as 公司名称,GSDH as 公司电话,GSDZ as 公司地址,YYZZSMJ as 营业执照,ZZJGDMZSMJ as 组织机构代码证,SWDJZSMJ as 税务登记证,KHXKZSMJ as 开户许可证 ,'' as 签字承诺书,QTZZWJSMJ as 其他资质,'' as 注册类别,a.SFZSMJ as 身份证扫描件,b.FGSKTSHXX as 分公司审核信息,FGSKTSHZT as 分公司审核状态,c.DLRDLZH as 经纪人登录账号,c.DLRYHM as 经纪人用户名,c.DLRBH as 经纪人编号  from ZZ_BUYJBZLXXB as a left join ZZ_YHJSXXB as b on a.JSBH=b.JSBH left join dbo.ZZ_MMJYDLRZHGLB as c on a.DLYX=c.JSDLZH and b.DLYX=c.JSDLZH where a.dlyx='" + dlyx + "'";
        }
        else if (jszhlx == "经纪人账户")
        {
            sql_select = "select b.KTSHZT as 审核状态,b.KTSHXX as 审核信息,a.Number,a.DLYX as 登陆邮箱,a.YHM as 用户名,a.JSBH as 角色编号,a.LXRXM as 联系人姓名,a.SJH as 手机号,a.SFYZSJH as 是否验证手机号,a.SSSF as 所属省份,a.SSDS as 所属地市,a.SSQX as 所属区县,a.XXDZ as 详细地址,a.YZBM as 邮政编码,a.SFZH as 身份证号,a.GSMC as 公司名称,a.GSDH as 公司电话,a.GSDZ as 公司地址,a.YYZZSMJ as 营业执照,a.ZZJGDMZSMJ as 组织机构代码证,a.SWDJZSMJ as 税务登记证,a.KHXKZSMJ as 开户许可证 ,a.FDDBRQZDCNSSMJ as 签字承诺书,QTZZWJSMJ as 其他资质,a.ZCLX as 注册类别,a.SFZSMJ as 身份证扫描件,b.FGSKTSHXX as 分公司审核信息,FGSKTSHZT as 分公司审核状态  from ZZ_DLRJBZLXXB as a left join ZZ_YHJSXXB as b on a.JSBH=b.JSBH where a.dlyx='" + dlyx + "'";
        }
        
        DataSet ds = DbHelperSQL.Query(sql_select);
        ds.Tables[0].TableName = "数据";

        //无论是否成功，都要加入这个执行结果表，并且要确保这个表中有且只有一条数据
        DataTable objTable = new DataTable("执行情况");
        objTable.Columns.Add("执行状态", typeof(string));
        objTable.Columns.Add("执行结果", typeof(string));      
        ds.Tables.Add(objTable);

        if (ds != null && ds.Tables[0].Rows.Count == 1)
        {
            ds.Tables["执行情况"].Rows.Add(new object[] { "ok", "ok" });          
        }
        else
        {
            ds.Tables["执行情况"].Rows.Add(new object[] { "失败", "未获得任何基本资料和资质信息！" }); 
        }
        return ds;
    }
}