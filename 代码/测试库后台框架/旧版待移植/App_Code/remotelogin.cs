using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Text;
using System.Collections;
using System.Data;
using System.Net;
using System.IO;
using Galaxy.ClassLib.DataBaseFactory;
using System.Configuration;
using Hesion.Brick.Core.WorkFlow;
using ZTC;
using FMOP.DB;

/// <summary>
///remotelogin 的摘要说明
/// </summary>
[WebService(Namespace = "http://fwpt.fm8844.com/FWPTZS/webservices")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class remotelogin : System.Web.Services.WebService {

    //连接工厂接口
    public Galaxy.ClassLib.DataBaseFactory.I_DBFactory I_DBF;
    //数据库连接接口
    public Galaxy.ClassLib.DataBaseFactory.I_Dblink I_DBL;

    public remotelogin () {

        //如果使用设计的组件，请取消注释以下行 
        //InitializeComponent(); 

        //初始化工厂
        AppSettingsReader hh = new AppSettingsReader();//调用web.Config中的数据库配置
        I_DBF = new Galaxy.ClassLib.DataBaseFactory.DBFactory();
        I_DBL = I_DBF.DbLinkSqlMain(ConfigurationManager.ConnectionStrings["FMOPConn"].ToString());

    }

    /// <summary>
    /// 验证登录账号密码
    /// </summary>
    /// <param name="email"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    [WebMethod]
    public DataSet checklogin(string email, string password)
    {
        DataSet DataSet_this = new DataSet();
        Hashtable return_ht = new Hashtable();


        /*2012.04.27 王永辉添加防止SQl注入  */
        #region
        DeSqlzhuru aa = new DeSqlzhuru();
        Hashtable ht = new Hashtable();
        ht["email"] = email;
        ht["password"] = password;
        if (aa.ValidateQuery(ht))
        {
           
            return null;
        }
        #endregion


        return_ht = I_DBL.RunProc("select *,'上次登录时间'='首次登录','' '是否续签' from FWPT_YHXXB where DLYX = '" + email + "' and DLMM = '" + password + "'   ", DataSet_this);
        if (Convert.ToBoolean(return_ht["return_float"]))
        {
            DataSet_this = (DataSet)return_ht["return_ds"];


            if (DataSet_this != null && DataSet_this.Tables[0].Rows.Count > 0)
            {
                //判定是否解约
                string qyzt = getSFJY(DataSet_this.Tables[0].Rows[0]["KHBH"].ToString());
                if (qyzt == "请选择" || qyzt == "已打款未签约" || qyzt == "解约")
                {
                    return null;
                }

                DataSet_this.Tables[0].Rows[0]["SFXM"] = null;

                //2011.11.16 王永辉用于 休眠用户
                DateTime dt = new DateTime(2011,12,1,0,0,0);
                DateTime dtover = new DateTime(2012,7,1,0,0,0);
                DateTime dtnoew = DateTime.Now ;
                if (dtnoew > dt && dtnoew < dtover)
                {
                    string StrTime = isoverT(DataSet_this.Tables[0].Rows[0]["KHBH"].ToString().Trim()) ;
                    if (StrTime != "") //判断是否注册满3个月
                    {
                        DataSet dschekc = getSFXM(DataSet_this.Tables[0].Rows[0]["KHBH"].ToString().Trim());
                        if (dschekc != null && dschekc.Tables[0].Rows.Count > 0) 
                        {
                            if (dschekc.Tables[0].Rows[0]["SFXM"].ToString().Trim() == "" || dschekc.Tables[0].Rows[0]["SFXM"].ToString().Trim() == "重新激活")//SFXMTime
                            {
                                if (dschekc.Tables[0].Rows[0]["SFXM"].ToString().Trim() == "")
                                {
                                    //判断是否休眠或者直接关闭
                                    DateTime dtaa = new DateTime() ;
                                    string strstre = GetDatechaju(DataSet_this.Tables[0].Rows[0]["KHBH"].ToString().Trim(), StrTime, "", dtaa);
                                     if (strstre != "")
                                     {
                                         DataSet_this.Tables[0].Rows[0]["SFXM"] = strstre;//更改临时表值
                                         I_DBL.RunProc("update FWPT_YHXXB set SFXMTime = getdate(),SFXM ='" + strstre + "' where  DLYX = '" + email + "' ");//更改数据库信息
                                     }
                                    
                                }
                                if (dschekc.Tables[0].Rows[0]["SFXM"].ToString().Trim() == "重新激活")
                                {
                                    string strstre = GetDatechaju(DataSet_this.Tables[0].Rows[0]["KHBH"].ToString().Trim(), StrTime, "重新激活", DateTime.Parse(dschekc.Tables[0].Rows[0]["SFXMTime"].ToString().Trim()));
                                    if (strstre != "")
                                    {
                                        DataSet_this.Tables[0].Rows[0]["SFXM"] = strstre;//更改临时表值
                                        I_DBL.RunProc("update FWPT_YHXXB set SFXMTime = getdate(),SFXM ='" + strstre + "' where  DLYX = '" + email + "' ");//更改数据库信息
                                    }
                                   
                                }
                            }
                        }
                    }
                   
                }
                


                /* 判断条件（如果 状态为“”  或   (DataSet_this.Tables[0].Rows[0]["WYHDJ"] 是“重新激活” ，并且 上次激活时间距离现在超过了三个月)   ） 
                 如果符合条件
                 * {
                 *                    /* 判断条件（直通车超过3个月没有生成A类积分或在合格旧硒鼓交易中心无交易的 ， 并且(状态不为“已冻结”或不为“重新激活”)  ） 
                                  如果符合条件
                 *                      {
                 *              更新数据库，把"WYHDJ"字段，改为“已冻结”
                 *               更新DataSet_this,把DataSet_this.Tables[0].Rows[0]["WYHDJ"] 也改为“已冻结”
                 *                  }
                 *  }
                 *  else
                 *  {
                 *     更新数据库，把"WYHDJ"字段，改为“”
                 *     更新DataSet_this,把DataSet_this.Tables[0].Rows[0]["WYHDJ"] 也改为“”
                 *  }
                 */



                //获取首次登录时间
                DataSet_this.Tables[0].Rows[0]["上次登录时间"] = Getlistlogintime(email);

                //记录登陆信息
                string reply = "";
                reply = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                I_DBL.RunProc("update FWPT_YHXXB set ZHYCDLSJ = getdate(),DLCS = DLCS + 1,ZHYCDLIP='" + reply + "' where DLYX = '" + email + "'");
                //记录续签信息
                string isDealed = "SELECT * FROM FWPT_QYSDB WHERE FWSBH='" + DataSet_this.Tables[0].Rows[0]["KHBH"].ToString().Trim() + "' AND HTYXQ='2013财年'";//是否已经有数据
      DataTable dtIsDealed = DbHelperSQL.Query(isDealed).Tables[0];
        if (dtIsDealed.Rows.Count > 0 && dtIsDealed.Rows[0]["QYWCQK"].ToString() != "已签")
        {//如果存在数据，且签约状态不是已签
            DataSet_this.Tables[0].Rows[0]["是否续签"] = "未签";
        }
        else if (dtIsDealed.Rows.Count > 0 && dtIsDealed.Rows[0]["QYWCQK"].ToString() == "已签")
        {
            DataSet_this.Tables[0].Rows[0]["是否续签"] = "已签";
        }
                return DataSet_this;

            }
            else
            {
                return null;
            }
        }
        else
        {
            //显示错误
            //MessageBox.Show(return_ht["return_errmsg"].ToString(), "数据库操作提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return null;

        }
    }

    /// <summary>
    /// 注册时间是超过3个月还是6个月  wyh 2011.11.17
    /// </summary>
    /// <returns></returns>
    private string  isoverT( string khbh)
    {
       string  ispass = "";
        DataSet ds  = new DataSet();
        Hashtable ht = new Hashtable() ;
        ht = I_DBL.RunProc("select  CreateTime  from FWPT_YHXXB where KHBH = '" + khbh + "' ",ds);
        if (Convert.ToBoolean(ht["return_float"]))
        {
            ds = (DataSet)ht["return_ds"];
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                if (DateTime.Now.AddMonths(-3) > DateTime.Parse(ds.Tables[0].Rows[0]["CreateTime"].ToString().Trim()) && DateTime.Now.AddMonths(-6) <= DateTime.Parse(ds.Tables[0].Rows[0]["CreateTime"].ToString().Trim()))
                {
                    ispass = "3";
                }
                if (DateTime.Now.AddMonths(-6) > DateTime.Parse(ds.Tables[0].Rows[0]["CreateTime"].ToString().Trim()))
                {
                    ispass = "6";
                }
                    //DateTime.Parse(ds.Tables[0].Rows[0]["CreateTime"].ToString().Trim());
               
            }
        }


        return ispass;
    }

    /// <summary>
    /// 获取时间差距值 2011.11.17 wyh 
    /// </summary>
    /// <param name="khbh"></param>
    /// <returns></returns>
    private string GetDatechaju( string khbh,string strtime,string  strisjihuo,DateTime dtcxjh )
    {
        DataSet ds_Get = new DataSet();
        DataSet ds_kgjy = new DataSet();
        Hashtable ht_kgjy = new Hashtable();
        Hashtable ht = new Hashtable();
        DateTime dtstart = new DateTime(1800,1,1);
        
        string a = "";

        if (strisjihuo == "重新激活")
        {
            dtstart = dtcxjh;
        }
        

        //获取最后一次获得A积分时间
        ht = I_DBL.RunProc("select top 1 ChangeTime from  FWPT_FWSJFMXB where Bigclass = 'A' and FWSBH = '" + khbh + "' order by ChangeTime desc", ds_Get);
        if (Convert.ToBoolean(ht["return_float"]))
        {
            ds_Get = (DataSet)ht["return_ds"];
            if (ds_Get != null && ds_Get.Tables[0].Rows.Count > 0)
            {

                if (dtstart < DateTime.Parse(ds_Get.Tables[0].Rows[0]["ChangeTime"].ToString().Trim()))
                {
                    dtstart = DateTime.Parse(ds_Get.Tables[0].Rows[0]["ChangeTime"].ToString().Trim());
 
                }
                   
            }
        }

        //获取最近一次空鼓合格交易
        ht_kgjy = I_DBL.RunProc("select top 1 CreateTime  from SuccedTrade where (MRKHBH = '" + khbh + "' or MCKHBH = '" + khbh + "') order by CreateTime desc",ds_kgjy);
        if (Convert.ToBoolean(ht_kgjy["return_float"]))
        {
            ds_kgjy = (DataSet)ht_kgjy["return_ds"];
            if (ds_kgjy != null && ds_kgjy.Tables[0].Rows.Count > 0)
            {
               
                    if (dtstart < DateTime.Parse(ds_kgjy.Tables[0].Rows[0]["CreateTime"].ToString().Trim()))
                    {
                        dtstart = DateTime.Parse(ds_kgjy.Tables[0].Rows[0]["CreateTime"].ToString().Trim());
                    }
              
            }
        }




        if (dtstart.Year == 1800) // 若没有交易
        {
            //if()
            if (strtime == "3") //注册时间满3个月不满6个月
            {
                a = "休眠";
            }
            else if (strtime == "6")
            {
                a = "关闭账号";
            }
        }
        else
        {
            if (dtstart.AddMonths(3) < DateTime.Now && dtstart.AddMonths(6) > DateTime.Now)
            {
                a = "休眠";
            }
            else if(dtstart.AddMonths(6)< DateTime.Now)
            {
                a = "关闭账号";
            }
        }

        return a;
    }



    /// <summary>
    /// 从打款人获取签约状态
    /// </summary>
    /// <param name="khbh"></param>
    /// <returns></returns>
    [WebMethod]
    public string getSFJY_dkr(string khbh)
    {
        DataSet DataSet_this2 = new DataSet();
        Hashtable return_ht2 = new Hashtable();
        return_ht2 = I_DBL.RunProc("select SSFWSBH from KHGL_FWSDKRXX where number = '" + khbh + "' ", DataSet_this2);
        if (Convert.ToBoolean(return_ht2["return_float"]))
        {
            DataSet_this2 = (DataSet)return_ht2["return_ds"];
            if (DataSet_this2 != null && DataSet_this2.Tables[0].Rows.Count > 0)
            {
                //替换客户编号为档案中的编号
                return DataSet_this2.Tables[0].Rows[0][0].ToString();
            }
            else
            {
                return "";
            }
        }
        else
        {
            return "";
        }
    }


    /// <summary>
    /// 从客户档案获取签约状态
    /// </summary>
    /// <param name="khbh"></param>
    /// <returns></returns>
    [WebMethod]
    public string getSFJY(string khbh)
    {
        string sj_khbh = getSFJY_dkr(khbh);
        if (sj_khbh != "")
        {
            khbh = sj_khbh;
        }

        DataSet DataSet_this = new DataSet();
        Hashtable return_ht = new Hashtable();
        return_ht = I_DBL.RunProc("select HZZT  from KHGL_FXSJBXX where Number = '" + khbh + "'", DataSet_this);
        if (Convert.ToBoolean(return_ht["return_float"]))
        {
            DataSet_this = (DataSet)return_ht["return_ds"];
            if (DataSet_this != null && DataSet_this.Tables[0].Rows.Count > 0)
            {
                return DataSet_this.Tables[0].Rows[0][0].ToString();
            }
            else
            {
                return "";
            }
        }
        else
        {
            //显示错误
            //MessageBox.Show(return_ht["return_errmsg"].ToString(), "数据库操作提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return "";

        }
    }



    /// <summary>
    /// 从客户档案获取休眠状态
    /// </summary>
    /// <param name="khbh"></param>
    /// <returns></returns>
    [WebMethod]
    public DataSet getSFXM(string khbh)
    {
        string sj_khbh = getSFJY_dkr(khbh);
        if (sj_khbh != "")
        {
            khbh = sj_khbh;
        }

        DataSet DataSet_this = new DataSet();
        Hashtable return_ht = new Hashtable();
        return_ht = I_DBL.RunProc("select SFXM,SFXMTime  from FWPT_YHXXB where KHBH = '" + khbh + "'", DataSet_this);
        if (Convert.ToBoolean(return_ht["return_float"]))
        {
            DataSet_this = (DataSet)return_ht["return_ds"];
            if (DataSet_this != null && DataSet_this.Tables[0].Rows.Count > 0)
            {
                return DataSet_this;
            }
            else
            {
                return null ;
            }
        }
        else
        {
            //显示错误
            //MessageBox.Show(return_ht["return_errmsg"].ToString(), "数据库操作提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return null;

        }
    }






    /// <summary>
    /// 获取最后一次登录时间
    /// </summary>
    /// <param name="SFstr"></param>
    /// <returns></returns>
    [WebMethod]
    public string Getlistlogintime(string email)
    {
        DataSet DataSet_this = new DataSet();
        Hashtable return_ht = new Hashtable();
        return_ht = I_DBL.RunProc("select CONVERT(varchar, ZHYCDLSJ, 120 )  from FWPT_YHXXB where DLYX = '" + email + "'", DataSet_this);
        if (Convert.ToBoolean(return_ht["return_float"]))
        {
            DataSet_this = (DataSet)return_ht["return_ds"];
            if (DataSet_this != null && DataSet_this.Tables[0].Rows.Count > 0)
            {
                return DataSet_this.Tables[0].Rows[0][0].ToString();
            }
            else
            {
                return "首次登录";
            }
        }
        else
        {
            //显示错误
            //MessageBox.Show(return_ht["return_errmsg"].ToString(), "数据库操作提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return "首次登录";

        }
    }


    /// <summary>
    /// 检查客户编号是否已拥有可用帐号
    /// </summary>
    /// <param name="KHBH">要检查的客户编号</param>
    /// <param name="XYBH">要检查的客户协议编号</param>
    /// <returns></returns>
    [WebMethod]
    public bool checkeberegonlyone(string KHBH, string XYBH)
    {
        DataSet DataSet_this = new DataSet();
        Hashtable return_ht = new Hashtable();
        return_ht = I_DBL.RunProc("select * from FWPT_YHXXB where KHBH = '" + KHBH + "' and FWSXYBH = '" + XYBH + "' and SFYYZYX = '是' and CheckState = 1", DataSet_this);
        if (Convert.ToBoolean(return_ht["return_float"]))
        {
            DataSet_this = (DataSet)return_ht["return_ds"];
            if (DataSet_this != null && DataSet_this.Tables[0].Rows.Count > 0)
            {
                return true;

            }
            else
            {
                return false;
            }
        }
        else
        {
            //显示错误
            //MessageBox.Show(return_ht["return_errmsg"].ToString(), "数据库操作提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return false;

        }
    }








    /// <summary>
    /// 检查电子邮箱是否已存在
    /// </summary>
    /// <param name="email">要检查的邮箱</param>
    /// <returns></returns>
    [WebMethod]
    public bool checkemail(string email)
    {
        DataSet DataSet_this = new DataSet();
        Hashtable return_ht = new Hashtable();
        return_ht = I_DBL.RunProc("select * from FWPT_YHXXB where DLYX = '" + email + "'", DataSet_this);
        if (Convert.ToBoolean(return_ht["return_float"]))
        {
            DataSet_this = (DataSet)return_ht["return_ds"];
            if (DataSet_this != null && DataSet_this.Tables[0].Rows.Count > 0)
            {
                return true;

            }
            else
            {
                return false;
            }
        }
        else
        {
            //显示错误
            //MessageBox.Show(return_ht["return_errmsg"].ToString(), "数据库操作提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return false;

        }
    }




    /// <summary>
    /// 通过客户编号查找服务商信息(从业务平台客户档案中查找)
    /// </summary>
    /// <param name="email">要检查的客户编号</param>
    /// <param name="email">要检查的客户协议编号</param>
    /// <returns></returns>
    [WebMethod]
    public DataTable checkUserFormKHBH(string KHBH, string xybh)
    {
        ////首先从打款人中进行查找，先找到对应客户档案再查询。
        //if (KHBH.IndexOf('6') == 0 || 1 == 1)
        //{
            //DataSet DataSet_this2 = new DataSet();
            //Hashtable return_ht2 = new Hashtable();
            //return_ht2 = I_DBL.RunProc("select SSFWSBH from KHGL_FWSDKRXX where number = '" + KHBH + "' ", DataSet_this2);
            //if (Convert.ToBoolean(return_ht2["return_float"]))
            //{
                //DataSet_this2 = (DataSet)return_ht2["return_ds"];
                //if (DataSet_this2 != null && DataSet_this2.Tables[0].Rows.Count > 0)
                //{
                    ////替换客户编号为档案中的编号
                    //KHBH = DataSet_this2.Tables[0].Rows[0][0].ToString();
                //}
                //else
                //{
                    //;
                //}
            //}
            //else
            //{
                //;
            //}
        //}

        DataSet DataSet_this = new DataSet();
        Hashtable return_ht = new Hashtable();
        string sql = "select * from KHGL_New as Amain join KHGL_FWSHTXX as Aht on Amain.Number =  Aht.Number join KHGL_FXSJBXX as Ajb on Ajb.Number=Amain.Number where Amain.Number = '" + KHBH + "' and   Amain.XSQD like  '%服务商%' and Amain.Number not like 'DY%' and Amain.SFZSKH = '1' and  Aht.HTBH = '" + xybh + "' and Ajb.HZZT in ('签约已打款','签约未打款')";
        return_ht = I_DBL.RunProc(sql, DataSet_this);
        if (Convert.ToBoolean(return_ht["return_float"]))
        {
            DataSet_this = (DataSet)return_ht["return_ds"];
            if (DataSet_this != null && DataSet_this.Tables[0].Rows.Count > 0)
            {
                return DataSet_this.Tables[0];

            }
            else
            {
                return null;
            }
        }
        else
        {
            //显示错误
            //MessageBox.Show(return_ht["return_errmsg"].ToString(), "数据库操作提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return null;

        }
    }



    /// <summary>
    /// 通过邮箱获取用户信息
    /// </summary>
    /// <param name="email">邮箱账号</param>
    /// <returns></returns>
    [WebMethod]
    public DataTable getuserinfobyemail(string email)
    {
        DataSet DataSet_this = new DataSet();
        Hashtable return_ht = new Hashtable();
        return_ht = I_DBL.RunProc("select * from FWPT_YHXXB where DLYX = '" + email + "'", DataSet_this);
        if (Convert.ToBoolean(return_ht["return_float"]))
        {
            DataSet_this = (DataSet)return_ht["return_ds"];
            if (DataSet_this != null && DataSet_this.Tables[0].Rows.Count > 0)
            {
                return DataSet_this.Tables[0];

            }
            else
            {
                return null;
            }
        }
        else
        {
            //显示错误
            //MessageBox.Show(return_ht["return_errmsg"].ToString(), "数据库操作提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return null;

        }
    }



    /// <summary>
    /// 添加新的注册信息
    /// </summary>
    /// <returns></returns>
    [WebMethod]
    public bool addnewuser(string[] cs)
    {
        DataSet DataSet_this = new DataSet();
        Hashtable return_ht = new Hashtable();
        //获取新编号
        Hashtable ht = new Hashtable();
        ht["客户编号"] = cs[0];
        ht["服务商协议编号"] = cs[1];
        ht["所属办事处"] = cs[2];
        ht["登陆邮箱"] = cs[3];
        ht["传真号码"] = cs[4];
        ht["登陆密码"] = cs[5];
        ht["单位全称"] = cs[6];
        ht["单位地址"] = cs[7];
        ht["用户类型"] = cs[8];
        ht["联系人姓名"] = cs[9];
        ht["联系人性别"] = cs[10];
        ht["联系人固话"] = cs[11];
        ht["联系人手机"] = cs[12];
        ht["所在省份"] = cs[13];
        ht["邮箱验证码"] = cs[14];
        string newnumber = ((DataSet)(I_DBL.RunProc_CMD("FWPT_YHXXB_GetNewNumber", new DataSet())["return_ds"])).Tables[0].Rows[0][0].ToString();

        //return_ht = I_DBL.RunProc("INSERT INTO FWPT_YHXXB (Number, SSBSC, DLYX,CZHM,DLMM, DWQC, DWDZ, YHLX,THYHZC, SSHY, LXRXM, LXRXB, LXRGH, LXRSJ, SZSF, KHBH, ZHYCDLSJ, DLCS, SFYXDL, ZHYCDLIP,SFYYZYX,YXYZM, NextChecker, CheckState, CreateUser) VALUES ('" + newnumber + "', '" + ht["所属办事处"].ToString() + "', '" + ht["登陆邮箱"].ToString() + "',  '" + ht["传真号码"].ToString() + "','" + ht["登陆密码"].ToString() + "', '" + ht["单位全称"].ToString() + "',  '" + ht["单位地址"].ToString() + "', '" + ht["用户类型"].ToString() + "', '" + ht["提货政策"].ToString() + "', '" + ht["所属行业"].ToString() + "', '" + ht["联系人姓名"].ToString() + "', '" + ht["联系人性别"].ToString() + "', '" + ht["联系人固话"].ToString() + "',  '" + ht["联系人手机"].ToString() + "',  '" + ht["所在省份"].ToString() + "', null, null, 0, '是', null, '否', '" + ht["邮箱验证码"].ToString() + "',  null, 0, 'admin')");
        return_ht = I_DBL.RunProc("INSERT INTO FWPT_YHXXB (Number,KHBH,FWSXYBH, SSBSC, DLYX,CZHM,DLMM, DWQC, DWDZ, YHLX, LXRXM, LXRXB, LXRGH, LXRSJ, SZSF, ZHYCDLSJ, DLCS, SFYXDL, ZHYCDLIP,SFYYZYX,YXYZM, NextChecker, CheckState, CreateUser) VALUES ('" + newnumber + "','" + ht["客户编号"].ToString() + "','" + ht["服务商协议编号"].ToString() + "', '" + ht["所属办事处"].ToString() + "', '" + ht["登陆邮箱"].ToString() + "',  '" + ht["传真号码"].ToString() + "','" + ht["登陆密码"].ToString() + "', '" + ht["单位全称"].ToString() + "',  '" + ht["单位地址"].ToString() + "', '" + ht["用户类型"].ToString() + "', '" + ht["联系人姓名"].ToString() + "', '" + ht["联系人性别"].ToString() + "', '" + ht["联系人固话"].ToString() + "',  '" + ht["联系人手机"].ToString() + "',  '" + ht["所在省份"].ToString() + "', null, 0, '是', null, '否', '" + ht["邮箱验证码"].ToString() + "',  null, 0, 'admin')");
        if (Convert.ToBoolean(return_ht["return_float"]))
        {
            return true;
        }
        else
        {
            //显示错误
            //MessageBox.Show(return_ht["return_errmsg"].ToString(), "数据库操作提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return false;

        }
    }


    /// <summary>
    /// 获取所在省份下拉菜单数组
    /// </summary>
    /// <returns></returns>
    [WebMethod]
    public string[] SFlist()
    {
        DataSet DataSet_this = new DataSet();
        Hashtable return_ht = new Hashtable();
        string[] allSF;
        return_ht = I_DBL.RunProc("select Province from System_City_0 where Province <> '无'", DataSet_this);
        if (Convert.ToBoolean(return_ht["return_float"]))
        {
            DataSet_this = (DataSet)return_ht["return_ds"];
            if (DataSet_this != null)
            {
                string tempstr = "";
                for (int i = 0; i < DataSet_this.Tables[0].Rows.Count; i++)
                {
                    tempstr = tempstr + DataSet_this.Tables[0].Rows[i][0].ToString() + ",";
                }
                tempstr = tempstr.Substring(0, tempstr.Length - 1);
                return tempstr.Split(',');

            }
            else
            {
                return null;
            }
        }
        else
        {
            //显示错误
            //MessageBox.Show(return_ht["return_errmsg"].ToString(), "数据库操作提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return null;

        }
    }


    /// <summary>
    /// 获取负责办事处
    /// </summary>
    /// <param name="SFstr"></param>
    /// <returns></returns>
    [WebMethod]
    public string GetSSBSC(string SFstr)
    {
        DataSet DataSet_this = new DataSet();
        Hashtable return_ht = new Hashtable();
        return_ht = I_DBL.RunProc("select Name from System_City_0 where Province like '%" + SFstr + "%'", DataSet_this);
        if (Convert.ToBoolean(return_ht["return_float"]))
        {
            DataSet_this = (DataSet)return_ht["return_ds"];
            if (DataSet_this != null && DataSet_this.Tables[0].Rows.Count > 0)
            {
                return DataSet_this.Tables[0].Rows[0][0].ToString();
            }
            else
            {
                return "未知办事处";
            }
        }
        else
        {
            //显示错误
            //MessageBox.Show(return_ht["return_errmsg"].ToString(), "数据库操作提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return "未知办事处";

        }
    }



    /// <summary>
    /// 获取功能菜单数据集
    /// </summary>
    /// <returns></returns>
    [WebMethod]
    public DataTable GetMenuData()
    {
        DataSet DataSet_this = new DataSet();
        Hashtable return_ht = new Hashtable();
        Hashtable putin_ht = new Hashtable();
        putin_ht["sField"] = "SortID,SortName,SortParentID,SortParentPath,SortOrder,GoToUrl,GoToUrlParameter,ShowWho";
        putin_ht["sTable"] = "FWPT_tbMenuSort";
        putin_ht["iSortID"] = 0;
        putin_ht["iCond"] = 1;
        return_ht = I_DBL.RunProc_CMD("sp_Util_Sort_SELECT", DataSet_this, putin_ht);
        if (Convert.ToBoolean(return_ht["return_float"]))
        {
            DataSet_this = (DataSet)return_ht["return_ds"];
            if (DataSet_this != null)
            {
                return DataSet_this.Tables[0];

            }
            else
            {
                return null;
            }
        }
        else
        {
            //显示错误
            //MessageBox.Show(return_ht["return_errmsg"].ToString(), "数据库操作提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return null;

        }
    }


    /// <summary>
    /// 获取功能菜单数据集-用户端(制定表名)
    /// </summary>
    /// <returns></returns>
    [WebMethod]
    public DataTable GetMenuData_yhtb(string sTable)
    {
        DataSet DataSet_this = new DataSet();
        Hashtable return_ht = new Hashtable();
        Hashtable putin_ht = new Hashtable();
        putin_ht["sField"] = "SortID,SortName,SortParentID,SortParentPath,SortOrder,GoToUrl,GoToUrlParameter,ShowWho,TargetGo,ToolTipText";
        putin_ht["sTable"] = sTable;
        putin_ht["iSortID"] = 0;
        putin_ht["iCond"] = 1;
        return_ht = I_DBL.RunProc_CMD("sp_Util_Sort_SELECT", DataSet_this, putin_ht);
        if (Convert.ToBoolean(return_ht["return_float"]))
        {
            DataSet_this = (DataSet)return_ht["return_ds"];
            if (DataSet_this != null)
            {
                return DataSet_this.Tables[0];

            }
            else
            {
                return null;
            }
        }
        else
        {
            //显示错误
            //MessageBox.Show(return_ht["return_errmsg"].ToString(), "数据库操作提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return null;

        }
    }


    /// <summary>
    /// 获取功能菜单数据集(制定表名)
    /// </summary>
    /// <returns></returns>
    [WebMethod]
    public DataTable GetMenuData_mytb(string sTable)
    {
        DataSet DataSet_this = new DataSet();
        Hashtable return_ht = new Hashtable();
        Hashtable putin_ht = new Hashtable();
        putin_ht["sField"] = "SortID,SortName,SortParentID,SortParentPath,SortOrder,GoToUrl,GoToUrlParameter,ShowWho,TargetGo";
        putin_ht["sTable"] = sTable;
        putin_ht["iSortID"] = 0;
        putin_ht["iCond"] = 1;
        return_ht = I_DBL.RunProc_CMD("sp_Util_Sort_SELECT", DataSet_this, putin_ht);
        if (Convert.ToBoolean(return_ht["return_float"]))
        {
            DataSet_this = (DataSet)return_ht["return_ds"];
            if (DataSet_this != null)
            {
                return DataSet_this.Tables[0];

            }
            else
            {
                return null;
            }
        }
        else
        {
            //显示错误
            //MessageBox.Show(return_ht["return_errmsg"].ToString(), "数据库操作提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return null;

        }
    }



    /// <summary>
    /// 发送新用户注册提醒给相应办事处指定人员
    /// </summary>
    /// <param name="BM">办事处</param>
    [WebMethod]
    public void SendRegWarnings(string BM)
    {
        DataSet DataSet_this = new DataSet();
        Hashtable return_ht = new Hashtable();
        return_ht = I_DBL.RunProc("select * from KHGL_KHXXTXRGL where txmk='FWPT_YHXXB' and BM='" + BM + "'", DataSet_this);
        if (Convert.ToBoolean(return_ht["return_float"]))
        {
            DataSet_this = (DataSet)return_ht["return_ds"];
            if (DataSet_this != null && DataSet_this.Tables[0].Rows.Count > 0)
            {
                string context = "富美直通车有新的注册信息提交，请及时处理审核！";
                string url = "WorkFlow_View.aspx?module=FWPT_YHXXB";
                string grade = "1";
                string fromuser = "admin";
                for (int i = 0; i < DataSet_this.Tables[0].Rows.Count; i++)
                {
                    //分为两种情况,如果员工编号为空，则向岗位批量发送提醒。
                    if (DataSet_this.Tables[0].Rows[i]["ygbh"].ToString() == "")
                    {
                        string ssql = "select number,employee_name,bm,gwmc from HR_employees where  ygzt not like '%离职%' and gwmc='" + DataSet_this.Tables[0].Rows[i]["GW"].ToString() + "' and BM='" + DataSet_this.Tables[0].Rows[i]["BM"].ToString() + "'";
                        DataSet dsall_ds = new DataSet();
                        Hashtable dsall_ht = I_DBL.RunProc(ssql, dsall_ds);
                        dsall_ds = (DataSet)return_ht["return_ds"];
                        if (dsall_ds != null && dsall_ds.Tables[0].Rows.Count > 0)
                        {
                            for (int t = 0; t < dsall_ds.Tables[0].Rows.Count; t++)
                            {
                                string touser = dsall_ds.Tables[0].Rows[t]["number"].ToString();
                                string sql = "insert into User_Warnings(Context,Module_Url,Grade,FromUser,Touser)";
                                sql = sql + " values('" + context.Trim() + "','" + url.Trim() + "','" + grade.Trim() + "','" + fromuser.Trim() + "','" + touser.Trim() + "')";
                                I_DBL.RunProc(sql);
                            }
                        }
                    }
                    else
                    {
                        string touser = DataSet_this.Tables[0].Rows[i]["ygbh"].ToString();
                        string sql = "insert into User_Warnings(Context,Module_Url,Grade,FromUser,Touser)";
                        sql = sql + " values('" + context.Trim() + "','" + url.Trim() + "','" + grade.Trim() + "','" + fromuser.Trim() + "','" + touser.Trim() + "')";
                        I_DBL.RunProc(sql);
                    }

                }

            }
        }


    }







    /// <summary>
    /// 获取服务商等级信息信息
    /// </summary>
    /// <param name="KHBH"></param>
    /// <returns></returns>
    [WebMethod]
    public string GetFWSDJ(string KHBH)
    {
        fwsClass f = new fwsClass();
        fwsInfo ff = f.GetFwsInfo(KHBH);
        return ff.fwsXJ.ToString();    
    }


    
}
