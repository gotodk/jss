using System;
using System.Data;
using FMOP.DB;
using System.Collections;
using System.Configuration;

/// <summary>
///ERPtongbu 的摘要说明
/// </summary>
public class ERPtongbu
{
    ArrayList al = null;
    public static string Erpcon = ConfigurationManager.ConnectionStrings["fmConnection"].ToString();
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
            if (Number != "")
            {
                if (type == 1)
                {
                    StrErpSql = "select * from COPMA where MA001 ='" + Number.Trim() + "'";
                    ds = GetErpData.GetDataSet(StrErpSql);
                    if (ds == null || ds.Tables[0].Rows.Count == 0)
                    {
                        StrErpSql = " select * from  KHGL_New where Number = '" + Number.Trim() + "'";
                        DataSet dsa = DbHelperSQL.Query(StrErpSql);
                        if (dsa != null && dsa.Tables[0].Rows.Count == 1)
                        {
                            DataRow dr = dsa.Tables[0].Rows[0];
                            string strSName = dr["KHMC"].ToString().Trim();
                            if (strSName.Length > 10)
                            {
                                strSName = strSName.Substring(0, 8);
                            }
                            if (dr["LXDH"].ToString().Length > 20)
                            {
                                dr["LXDH"] = dr["LXDH"].ToString().Substring(0, 15);
                            }
                            if (dr["DZ"].ToString() == "请填写更详细的地址，如街道、门牌号等")
                            {
                                dr["DZ"] = "";
                            }
                            if (dr["LXRXM"].ToString().Length > 15)
                            {
                                dr["LXRXM"] = dr["LXRXM"].ToString().Substring(0, 10);
                            }

                            if (GetBaseData(dsa.Tables[0].Rows[0]["SSXSQY"].ToString().Trim()).Count == 5)
                            {
                                string StrErpSql1 = " insert into COPMA( COMPANY,CREATOR,USR_GROUP,CREATE_DATE,FLAG,MA001,MA002,MA003,MA005,MA006,MA007,MA009,MA014,MA015,MA016,MA018,MA025,MA026,MA027,MA035,MA037,MA038,MA039,MA041,MA042,MA048,MA064,MA066,MA075,MA080,MA084,MA085,MA087,MA089,MA101,UDF01,MA032,MA091,MA092,MA093,MA094) values ( 'fm','" + al[1].ToString().Trim() + "','" + al[2].ToString().Trim() + "','" + DateTime.Now.ToString("yyyymmddhhmmss").Trim() + "','2','" + dr["Number"].ToString().Trim() + "','" + strSName.Trim() + "','" + dr["KHMC"].ToString().Trim() + "','" + dr["LXRXM"].ToString().Trim() + "','" + dr["LXDH"].ToString().Trim() + "','" + dr["LXSJ"].ToString().Trim() + "','" + dr["DZYX"].ToString().Trim() + "','RMB','" + al[0].ToString().Trim() + "','" + al[3].ToString().Trim() + "','" + al[4].ToString().Trim() + "','" + dr["DZ"].ToString().Trim() + "','" + dr["LXRXM"].ToString().Trim() + "，" + dr["LXSJ"].ToString().Trim() + "','" + dr["DZ"].ToString().Trim() + "','2','A','1','1','2','1','1','" + dr["LXRXM"].ToString().Trim() + "，" + dr["LXSJ"].ToString().Trim() + "','N','142','无','N','" + al[3].ToString().Trim() + "','2','3','0.1700','无','y','1.0000','1.0000','1.0000','1.0000')";
                                // int a =  GetErpData.GetDataSet(StrErpSql);
                                GetErpData.ExecuteSql(StrErpSql1);

                                ispass = true;


                            }
                        }

                    }


                }
                if (type == 2)
                {
                    StrErpSql = "select * from COPMA where MA001 ='" + Number.Trim() + "'";
                    ds = GetErpData.GetDataSet(StrErpSql);
                    if (ds == null || ds.Tables[0].Rows.Count == 0)
                    {
                        StrErpSql = " select * from  KHGL_FWSDKRXX where Number = '" + Number.Trim() + "'";
                        DataSet dsa = DbHelperSQL.Query(StrErpSql);
                        if (dsa != null && dsa.Tables[0].Rows.Count == 1 && dsa.Tables[0].Rows[0]["FWSXSQD"].ToString().Trim().IndexOf("服务商") >= 0)
                        {
                            DataRow dr = dsa.Tables[0].Rows[0];
                            string strSName = dr["DKRXM"].ToString().Trim();
                            if (strSName.Length > 20)
                            {
                                strSName = strSName.Substring(0, 15);
                            }
                            if (dr["DKRLXDH"].ToString().Length > 20)
                            {
                                dr["DKRLXDH"] = dr["DKRLXDH"].ToString().Substring(0, 15);
                            }

                            if (GetBaseData(dsa.Tables[0].Rows[0]["SSBSC"].ToString().Trim()).Count == 5)
                            {
                                string StrErpSql1 = " insert into COPMA( COMPANY,CREATOR,USR_GROUP,CREATE_DATE,FLAG,MA001,MA002,MA003,MA005,MA006,MA007,MA009,MA014,MA015,MA016,MA018,MA025,MA026,MA027,MA035,MA037,MA038,MA039,MA041,MA042,MA048,MA064,MA066,MA075,MA080,MA084,MA085,MA087,MA089,MA101,UDF01,MA032,MA091,MA092,MA093,MA094) values ( 'fm','" + al[1].ToString().Trim() + "','" + al[2].ToString().Trim() + "','" + DateTime.Now.ToString("yyyymmddhhmmss").Trim() + "','2','" + dr["Number"].ToString().Trim() + "','" + strSName.Trim().Trim() + "','" + dr["DKRXM"].ToString().Trim() + "','" + dr["DKRXM"].ToString().Trim() + "','" + dr["DKRLXDH"].ToString().Trim() + "','" + "无" + "','" + "" + "','RMB','" + al[0].ToString().Trim() + "','" + al[3].ToString().Trim() + "','" + al[4].ToString().Trim() + "','" + "无" + "','" + dr["DKRXM"].ToString().Trim() + "，" + dr["DKRLXDH"].ToString().Trim() + "','" + "" + "','2','A','1','1','2','1','1','" + dr["DKRXM"].ToString().Trim() + "，" + dr["DKRLXDH"].ToString().Trim() + "','N','142','无','N','" + al[3].ToString().Trim() + "','2','3','0.1700','无','y','1.0000','1.0000','1.0000','1.0000')";


                                GetErpData.ExecuteSql(StrErpSql1);

                                ispass = true;

                            }
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
    /// 获取单据基本信息
    /// </summary>
    /// <param name="Num"></param>
    /// <returns></returns>
    private ArrayList GetBaseData(string ssbm)
    {
        al = new ArrayList();
        string StrErpSql = " select ME001 from  CMSME where ME002 LIKE '%" + ssbm.Trim() + "%'";
        string StrCon = "";
        DataSet ds = GetErpData.GetDataSet(StrErpSql);
        if (ds != null && ds.Tables[0].Rows.Count == 1)
        {
            StrCon = ds.Tables[0].Rows[0]["ME001"].ToString();
            al.Add(StrCon);
            StrErpSql = " select MF001 as 账号,MF004 as 用户组  from  ADMMF where MF007='" + StrCon.Trim() + "' and MF001 like '%00' ";
            DataSet dsa = GetErpData.GetDataSet(StrErpSql);
            if (dsa != null && dsa.Tables[0].Rows.Count > 0)
            {
                /*取的ERP内勤账号跟用户组*/
                al.Add(dsa.Tables[0].Rows[0]["账号"].ToString());
                al.Add(dsa.Tables[0].Rows[0]["用户组"].ToString());
            }

            StrErpSql = " select MF001 as 账号,MF004 as 用户组  from  ADMMF where MF007='" + StrCon.Trim() + "' and MF001 like '%00' ";
            DataSet dsaa = GetErpData.GetDataSet(StrErpSql);
            if (dsaa != null && dsaa.Tables[0].Rows.Count > 0)
            {
                /*取的ERP内勤账号跟用户组*/
                al.Add(dsaa.Tables[0].Rows[0]["账号"].ToString());

            }
            StrErpSql = " select MR002 from  CMSMR where MR004 LIKE '%" + ssbm.Trim() + "%'";
            DataSet dsaaa = GetErpData.GetDataSet(StrErpSql);
            if (dsaaa != null && dsaaa.Tables[0].Rows.Count > 0)
            {
                /*取的ERP内勤账号跟用户组*/
                al.Add(dsaaa.Tables[0].Rows[0]["MR002"].ToString());

            }

        }

        return al;

    }

}
