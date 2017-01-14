using System;
using System.Data;
using FMOP.DB;
using System.Collections;
using System.Configuration;
using System.Text;

/// <summary>
///ERPtongbu 的摘要说明
/// </summary>
public class ERPtongbu
{
    ArrayList al = null;
    public static string Erpcon = ConfigurationManager.ConnectionStrings["fmConnectionWrite"].ToString();
    public ERPtongbu()
    {
        //
        //TODO: 在此处添加构造函数逻辑
        //
    }

    /// <summary>
    /// 客户资料同步
    /// </summary>
    /// <param name="Number"> 客户编号</param>
    /// <param name="type">1.服务商类型客户 2.打款人类型客户</param>
    /// <returns></returns>
    public bool Incustomer(string Number, int type)
    {
        bool ispass = false;
        try
        {
            string TabName = "";
            //string StrSql = "";
            DataSet ds = null;
            string StrErpSql = "";
            string SSBMCode = "";
            string StrMum = "";
            string StrUGrop = "";

            string sql_ZB = "";
            string sql_FGS = "";
            if (Number != "")
            {
                if (type == 1)
                {
                    StrErpSql = " select * from  KHGL_New where Number = '" + Number.Trim() + "'";
                    DataSet dsa = DbHelperSQL.Query(StrErpSql);
                    if (dsa != null && dsa.Tables[0].Rows.Count == 1 && (dsa.Tables[0].Rows[0]["XSQD"].ToString().Trim().IndexOf("推广人员") >= 0 || dsa.Tables[0].Rows[0]["XSQD"].ToString().Trim().IndexOf("直销用户") >= 0 || dsa.Tables[0].Rows[0]["XSQD"].ToString().Trim().IndexOf("服务商") >= 0 || dsa.Tables[0].Rows[0]["XSQD"].ToString().Trim().IndexOf("终端客户") >= 0 || dsa.Tables[0].Rows[0]["XSQD"].ToString().Trim().IndexOf("服务商客户") >= 0))
                    {
                        DataRow dr = dsa.Tables[0].Rows[0];
                        string strSName =ToDBC(dr["KHMC"].ToString().Trim());

                        if (strSName.Length > 10)
                        {
                            strSName = strSName.Substring(0, 9);
                           

                        }

                        dr["LXDH"] = ToDBC(dr["LXDH"].ToString());
                        if (dr["LXDH"].ToString().Length > 20)
                        {
                            dr["LXDH"] = dr["LXDH"].ToString().Substring(0, 19);
                        }
                        string ssbm = dsa.Tables[0].Rows[0]["SSXSQY"].ToString().Trim();
                        StrErpSql = "select * from COPMA where MA001 ='" + Number.Trim() + "'";
                        //判断总部ERP系统中是否存在该客户信息，不存在则可以写入
                        ds = GetErpData.GetDataSet(StrErpSql);
                        //判断分公司ERP系统中是否存在该客户信息，不存在可写入
                        DataSet dsFGS = GetErpData.GetDataSet(StrErpSql, dsa.Tables[0].Rows[0]["SSXSQY"].ToString());
                        if ((ds == null || ds.Tables[0].Rows.Count == 0) && (dsFGS == null || dsFGS.Tables[0].Rows.Count == 0))
                        {
                            if (GetBaseData(ssbm, "总部ERP").Count == 5)
                            {

                                sql_ZB = " insert into COPMA( COMPANY,CREATOR,USR_GROUP,CREATE_DATE,FLAG,MA001,MA002,MA003,MA005,MA006,MA007,MA009,MA014,MA015,MA016,MA018,MA025,MA026,MA027,MA035,MA037,MA038,MA039,MA041,MA042,MA048,MA064,MA066,MA075,MA080,MA084,MA085,MA087,MA089,MA101,UDF01,MA032,MA091,MA092,MA093,MA094,MA088) values ( 'fm','" + al[1].ToString().Trim() + "','" + al[2].ToString().Trim() + "','" + DateTime.Now.ToString("yyyymmddhhmmss").Trim() + "','2','" + dr["Number"].ToString().Trim() + "','" + strSName.Trim() + "','" + dr["KHMC"].ToString().Trim() + "','" + dr["LXRXM"].ToString().Trim() + "','" + dr["LXDH"].ToString().Trim() + "','" + dr["LXSJ"].ToString().Trim() + "','" + dr["DZYX"].ToString().Trim() + "','RMB','" + al[0].ToString().Trim() + "','" + al[3].ToString().Trim() + "','" + al[4].ToString().Trim() + "','" + dr["DZ"].ToString().Trim() + "','" + dr["LXRXM"].ToString().Trim() + "，" + dr["LXSJ"].ToString().Trim() + "','" + dr["DZ"].ToString().Trim() + "','2','A','1','1','2','1','1','" + dr["LXRXM"].ToString().Trim() + "，" + dr["LXSJ"].ToString().Trim() + "','N','142','无','N','" + al[3].ToString().Trim() + "','2','3','0.1700','无','y','1.0000','1.0000','1.0000','1.0000','2')";
                                // int a =  GetErpData.GetDataSet(StrErpSql);
                                // GetErpData.ExecuteSql(StrErpSql1);
                                //  ispass = true;

                            }
                            if (GetBaseData(ssbm, "分公司ERP").Count == 5)
                            {
                                string Strxsqd = "101";
                                if ((dsa.Tables[0].Rows[0]["XSQD"].ToString().Trim().Contains("服务商") || dsa.Tables[0].Rows[0]["XSQD"].ToString().Trim().Contains("门店服务商") || dsa.Tables[0].Rows[0]["XSQD"].ToString().Trim().Contains("打款人")) && !dsa.Tables[0].Rows[0]["XSQD"].ToString().Trim().Contains("服务商客户"))
                                {
                                    Strxsqd = "102";
                                }

                                sql_FGS = " insert into COPMA( COMPANY,CREATOR,USR_GROUP,CREATE_DATE,FLAG,MA001,MA002,MA003,MA005,MA006,MA007,MA009,MA014,MA015,MA016,MA018,MA025,MA026,MA027,MA035,MA037,MA038,MA039,MA041,MA042,MA048,MA064,MA066,MA075,MA080,MA084,MA085,MA087,MA089,MA101,UDF01,MA032,MA091,MA092,MA093,MA094,MA017,MA088) values ( 'fm','" + al[1].ToString().Trim() + "','" + al[2].ToString().Trim() + "','" + DateTime.Now.ToString("yyyymmddhhmmss").Trim() + "','2','" + dr["Number"].ToString().Trim() + "','" + strSName.Trim() + "','" + dr["KHMC"].ToString().Trim() + "','" + dr["LXRXM"].ToString().Trim() + "','" + dr["LXDH"].ToString().Trim() + "','" + dr["LXSJ"].ToString().Trim() + "','" + dr["DZYX"].ToString().Trim() + "','RMB','" + al[0].ToString().Trim() + "','" + al[3].ToString().Trim() + "','" + al[4].ToString().Trim() + "','" + dr["DZ"].ToString().Trim() + "','" + dr["LXRXM"].ToString().Trim() + "，" + dr["LXSJ"].ToString().Trim() + "','" + dr["DZ"].ToString().Trim() + "','2','A','1','1','2','1','1','" + dr["LXRXM"].ToString().Trim() + "，" + dr["LXSJ"].ToString().Trim() + "','N','142','无','N','" + al[3].ToString().Trim() + "','2','3','0.1700','无','y','1.0000','1.0000','1.0000','1.0000','" + Strxsqd + "','2')";
                                // int a =  GetErpData.GetDataSet(StrErpSql);
                                // GetErpData.ExecuteSql(StrErpSql1);

                                //  ispass = true;

                                object objConn = DbHelperSQL.GetSingle("select DYERP from system_city_0 where name='" + ssbm + "' AND DYERP != (select DYERP from system_city_0 where Name='公司总部' )");
                                if (objConn != null && objConn.ToString() != "")
                                {
                                    GetErpData.ExecuteSql(sql_FGS, ssbm);
                                }


                            }
                        }
                        //执行两个ERP数据库的插入语句
                        
                       
                        GetErpData.ExecuteSql(sql_ZB, "公司总部");                       
                        ispass = true;
                    }
                }
                if (type == 2)
                {
                    StrErpSql = " select * from  KHGL_FWSDKRXX where Number = '" + Number.Trim() + "'";
                    DataSet dsa = DbHelperSQL.Query(StrErpSql);
                    if (dsa != null && dsa.Tables[0].Rows.Count == 1 && dsa.Tables[0].Rows[0]["FWSXSQD"].ToString().Trim().IndexOf("服务商") >= 0)
                    {
                        DataRow dr = dsa.Tables[0].Rows[0];
                        string strSName = ToDBC(dr["KHMC"].ToString().Trim()); //dr["DKRXM"].ToString().Trim();
                        if (strSName.Length > 10)
                        {
                            strSName = strSName.Substring(0, 9);
                        }

                        dr["DKRLXDH"] = ToDBC(dr["DKRLXDH"].ToString());
                        if (dr["DKRLXDH"].ToString().Length > 20)
                        {
                            dr["DKRLXDH"] = dr["DKRLXDH"].ToString().Substring(0, 19);
                        }
                        string ssbm = dsa.Tables[0].Rows[0]["SSBSC"].ToString().Trim();
                        StrErpSql = "select * from COPMA where MA001 ='" + Number.Trim() + "'";
                        //判断总部ERP中是否存在数据
                        ds = GetErpData.GetDataSet(StrErpSql);
                        DataSet dsFGS = GetErpData.GetDataSet(StrErpSql, SSBMCode);
                        if ((ds == null || ds.Tables[0].Rows.Count == 0) && (dsFGS == null || dsFGS.Tables[0].Rows.Count == 0))
                        {
                            if (GetBaseData(ssbm, "总部ERP").Count == 5)
                            {
                                sql_ZB = " insert into COPMA( COMPANY,CREATOR,USR_GROUP,CREATE_DATE,FLAG,MA001,MA002,MA003,MA005,MA006,MA007,MA009,MA014,MA015,MA016,MA018,MA025,MA026,MA027,MA035,MA037,MA038,MA039,MA041,MA042,MA048,MA064,MA066,MA075,MA080,MA084,MA085,MA087,MA089,MA101,UDF01,MA032,MA091,MA092,MA093,MA094,MA088) values ( 'fm','" + al[1].ToString().Trim() + "','" + al[2].ToString().Trim() + "','" + DateTime.Now.ToString("yyyymmddhhmmss").Trim() + "','2','" + dr["Number"].ToString().Trim() + "','" + strSName.Trim().Trim() + "','" + dr["DKRXM"].ToString().Trim() + "','" + dr["DKRXM"].ToString().Trim() + "','" + dr["DKRLXDH"].ToString().Trim() + "','" + "无" + "','" + "" + "','RMB','" + al[0].ToString().Trim() + "','" + al[3].ToString().Trim() + "','" + al[4].ToString().Trim() + "','" + "无" + "','" + dr["DKRXM"].ToString().Trim() + "，" + dr["DKRLXDH"].ToString().Trim() + "','" + "" + "','2','A','1','1','2','1','1','" + dr["DKRXM"].ToString().Trim() + "，" + dr["DKRLXDH"].ToString().Trim() + "','N','142','无','N','" + al[3].ToString().Trim() + "','2','3','0.1700','','y','1.0000','1.0000','1.0000','1.0000','2')";


                                // GetErpData.ExecuteSql(StrErpSql1);

                                // ispass = true;

                            }
                            if (GetBaseData(ssbm, "分公司ERP").Count == 5)
                            {
                                sql_FGS = " insert into COPMA( COMPANY,CREATOR,USR_GROUP,CREATE_DATE,FLAG,MA001,MA002,MA003,MA005,MA006,MA007,MA009,MA014,MA015,MA016,MA018,MA025,MA026,MA027,MA035,MA037,MA038,MA039,MA041,MA042,MA048,MA064,MA066,MA075,MA080,MA084,MA085,MA087,MA089,MA101,UDF01,MA032,MA091,MA092,MA093,MA094,MA088) values ( 'fm','" + al[1].ToString().Trim() + "','" + al[2].ToString().Trim() + "','" + DateTime.Now.ToString("yyyymmddhhmmss").Trim() + "','2','" + dr["Number"].ToString().Trim() + "','" + strSName.Trim().Trim() + "','" + dr["DKRXM"].ToString().Trim() + "','" + dr["DKRXM"].ToString().Trim() + "','" + dr["DKRLXDH"].ToString().Trim() + "','" + "无" + "','" + "" + "','RMB','" + al[0].ToString().Trim() + "','" + al[3].ToString().Trim() + "','" + al[4].ToString().Trim() + "','" + "无" + "','" + dr["DKRXM"].ToString().Trim() + "，" + dr["DKRLXDH"].ToString().Trim() + "','" + "" + "','2','A','1','1','2','1','1','" + dr["DKRXM"].ToString().Trim() + "，" + dr["DKRLXDH"].ToString().Trim() + "','N','142','无','N','" + al[3].ToString().Trim() + "','2','3','0.1700','','y','1.0000','1.0000','1.0000','1.0000','2')";


                                // GetErpData.ExecuteSql(StrErpSql1);

                                // ispass = true;

                                object objConna = DbHelperSQL.GetSingle("select DYERP from system_city_0 where name='" + ssbm + "' AND DYERP != (select DYERP from system_city_0 where Name='公司总部' )");
                                if (objConna != null && objConna.ToString() != "")
                                {
                                    GetErpData.ExecuteSql(sql_FGS, ssbm);
                                }
                           
                            }
                            //执行两个ERP数据库的插入语句

                            GetErpData.ExecuteSql(sql_ZB, "公司总部");
                              
                            ispass = true;
                        }
                    }
                }
            }
            return ispass;
        }
        catch
        {
            return ispass;
        }
    }



    /// <summary>
    /// 专半角函数
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>

    public string ToDBC(string input)
    {
        char[] c = input.ToCharArray();
        for (int i = 0; i < c.Length; i++)
        {
            if (c[i] == 12288)
            {
                c[i] = (char)32;
                continue;
            }
            if (c[i] > 65280 && c[i] < 65375)
                c[i] = (char)(c[i] - 65248);
        }
        return new string(c);
    }



    /// <summary>
    /// 获取单据基本信息
    /// 2012-6-16时燕增加type参数，用于分区是从总部ERP取数据还是从分公司ERP取数据
    /// </summary>
    /// <param name="Num"></param>
    /// <returns></returns>
    private ArrayList GetBaseData(string ssbm, string type)
    {
        al = new ArrayList();
        if (type == "总部ERP")
        {
           
            string StrErpSql = " select ME001 from  CMSME where ME002 LIKE '%" + ssbm + "%'";
            string StrCon = "";
            DataSet ds = GetErpData.GetDataSet(StrErpSql,"公司总部");
            if (ds != null && ds.Tables[0].Rows.Count == 1)
            {
                StrCon = ds.Tables[0].Rows[0]["ME001"].ToString();
                al.Add(StrCon);
                StrErpSql = " select MF001 as 账号,MF004 as 用户组  from  ADMMF where MF007='" + StrCon.Trim() + "' and MF001 like '%00' ";
                DataSet dsa = GetErpData.GetDataSet(StrErpSql, "公司总部");
                if (dsa != null && dsa.Tables[0].Rows.Count > 0)
                {
                    /*取的ERP内勤账号跟用户组*/
                    al.Add(dsa.Tables[0].Rows[0]["账号"].ToString());
                    al.Add(dsa.Tables[0].Rows[0]["用户组"].ToString());
                }

                StrErpSql = " select MF001 as 账号,MF004 as 用户组  from  ADMMF where MF007='" + StrCon.Trim() + "' and MF001 like '%00' ";
                DataSet dsaa = GetErpData.GetDataSet(StrErpSql, "公司总部");
                if (dsaa != null && dsaa.Tables[0].Rows.Count > 0)
                {
                    /*取的ERP内勤账号跟用户组*/
                    al.Add(dsaa.Tables[0].Rows[0]["账号"].ToString());

                }
                StrErpSql = " select MR002 from  CMSMR where MR004 LIKE '%" + ssbm.Trim() + "%'";
                DataSet dsaaa = GetErpData.GetDataSet(StrErpSql, "公司总部");
                if (dsaaa != null && dsaaa.Tables[0].Rows.Count > 0)
                {
                    /*取的ERP内勤账号跟用户组*/
                    al.Add(dsaaa.Tables[0].Rows[0]["MR002"].ToString());

                }
            }
        }
        else if (type == "分公司ERP")
        {

            string Strfgs = "";

            object objConn = DbHelperSQL.GetSingle("select DYFGSMC from system_city_0 where name='" + ssbm + "' or dyfgsmc='" + ssbm + "'");
            Strfgs = objConn.ToString();


            string StrErpSql = " select ME001 from  CMSME where ME002 LIKE '%" + Strfgs.Trim() + "%'";
            string StrCon = "";
            DataSet ds = GetErpData.GetDataSet(StrErpSql,ssbm);
            if (ds != null && ds.Tables[0].Rows.Count == 1)
            {
                StrCon = ds.Tables[0].Rows[0]["ME001"].ToString();
                al.Add(StrCon);
                //StrErpSql = " select MF001 as 账号,MF004 as 用户组  from  ADMMF where MF007='" + StrCon.Trim() + "' and MF001 like '%fgszjl%' ";
                //DataSet dsa = GetErpData.GetDataSet(StrErpSql,ssbm);
                //if (dsa != null && dsa.Tables[0].Rows.Count > 0)
                //{
                    /*取的ERP内勤账号跟用户组*/
                    al.Add("fgszjl");
                    al.Add("000");
                //}

                //StrErpSql = " select MF001 as 账号,MF004 as 用户组  from  ADMMF where MF007='" + StrCon.Trim() + "' and MF001 like '%fgszjl%' ";
                //DataSet dsaa = GetErpData.GetDataSet(StrErpSql,ssbm);
                //if (dsaa != null && dsaa.Tables[0].Rows.Count > 0)
                //{
                    /*取的ERP内勤账号跟用户组*/
                    al.Add("fgszjl");

                //}
                //StrErpSql = " select MR002 from  CMSMR where MR004 LIKE '%" + ssbm.Trim() + "%'";
                //DataSet dsaaa = GetErpData.GetDataSet(StrErpSql,ssbm );
                //if (dsaaa != null && dsaaa.Tables[0].Rows.Count > 0)
                //{
                    /*取的ERP内勤账号跟用户组*/
                    al.Add(" ");

                //}
            }
        }
        return al;

    }


    /// <summary>
    /// 将服务机构写入ERP，用于服务站点信息写入ERP
    /// </summary>
    /// /// <param name="SSBSC">所属办事处</param>
    /// /// <param name="Number">服务机构编号</param>
    /// /// <param name="JGMC">服务机构名称</param>
    /// /// <param name="LXR">联系人</param>
    ////// <param name="LXR">联系人</param>
    /// ///<param name="LXDH">联系电话</param>
    /// ///<param name="DZ">地址</param>
    /// <returns></returns>
    public bool Incustomer(string SSBSC, string Number, string JGMC, string LXR, string LXDH, string DZ)
    {
        bool ispass = false;
        try
        {
            //string StrSql = "";
            DataSet ds = null;
            string StrErpSql = "";
            if (Number != "")
            {
                StrErpSql = "select * from COPMA where MA001 ='" + Number.Trim() + "'";
                ds = GetErpData.GetDataSet(StrErpSql);
                if (ds == null || ds.Tables[0].Rows.Count == 0)
                {
                    string strSName = JGMC.Trim();
                    if (strSName.Length > 10)
                    {
                        strSName = strSName.Substring(0, 9);
                    }
                    if (LXDH.Length > 20)
                    {
                        LXDH = LXDH.Substring(0, 19);
                    }

                    if (GetBaseData(SSBSC.Trim(), "总部ERP").Count == 5)
                    {
                        string StrErpSql1 = " insert into COPMA( COMPANY,CREATOR,USR_GROUP,CREATE_DATE,FLAG,MA001,MA002,MA003,MA005,MA006,MA007,MA009,MA014,MA015,MA016,MA018,MA025,MA026,MA027,MA035,MA037,MA038,MA039,MA041,MA042,MA048,MA064,MA066,MA075,MA080,MA084,MA085,MA087,MA089,MA101,UDF01,MA032,MA091,MA092,MA093,MA094) values ( 'fm','" + al[1].ToString().Trim() + "','" + al[2].ToString().Trim() + "','" + DateTime.Now.ToString("yyyymmddhhmmss").Trim() + "','2','" + Number.Trim() + "','" + strSName.Trim() + "','" + JGMC.Trim() + "','" + LXR.Trim() + "','"
                            + LXDH.Trim() + "','','','RMB','" + al[0].ToString().Trim() + "','" + al[3].ToString().Trim() + "','" + al[4].ToString().Trim() + "','"
                            + DZ.Trim() + "','" + LXR.Trim() + "," + LXDH + "','" + DZ.Trim() + "','2','A','1','1','2','1','1','" + LXR.Trim() + "," + LXDH + "','N','142','无','N','" + al[3].ToString().Trim() + "','2','3','0.1700','无','y','1.0000','1.0000','1.0000','1.0000')";
                        // int a =  GetErpData.GetDataSet(StrErpSql);
                        GetErpData.ExecuteSql(StrErpSql1);

                        ispass = true;
                    }
                }

            }
            return ispass;
        }
        catch
        {
            return ispass;
        }
    }
}
