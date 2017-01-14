using System;
using System.Collections.Generic;
using System.Web;
using System.Collections;
using FMOP.DB;
using System.Data;
/// <summary>
///zdyhClass 的摘要说明
/// </summary>
///刘杰，终端用户类
namespace ZDYH
{
    public class zdyhClass
    {
        private string dlyx = "";
        private string dlzh = "";
        public zdyhClass()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }
        public zdyhClass(string dlyx)
        {
            this.dlyx = dlyx;
        }
        /// <summary>
        /// 使用ERP同步的账号初始化
        /// </summary>
        /// <param name="dlzh">用户账号</param>
        /// <param name="type">ERP</param>
        public zdyhClass(string dlzh, string type)
        {
            this.dlzh = dlzh;
        }
        /// <summary>
        /// 返回终端用户的客户信息
        /// </summary>
        /// <returns></returns>
        public Hashtable GetZdyhInfo()
        {
            string sqls = "select YYYKHBHDYKHBH,YYYGSJGMCKHMC,YYYSSBSCDYSSXSQY,YYYLXSJDYLXRSJ,sheng_str,shi_str,quxian_str,XXDZ,YYYLXRXMDYLXRXM,isNull(SFZQYH,'否') as SFZQYH from FM_UsersZXYH where DLZH='" + dlyx.Trim() + "' and SFDJ='否' and  YYYKHBHDYKHBH is not null";
            string yhbh="空",yhmc="空",ssbsc="空",lxrsj="空",sf="空",cs="空",qx="空",xxdz="空",lxrxm="空",sfzqyh="否";
            Hashtable ht = new Hashtable();
            DataSet ds = DbHelperSQL.Query(sqls);
            if(ds.Tables[0].Rows[0][0].ToString().Trim()!="")
            {
              yhbh=ds.Tables[0].Rows[0][0].ToString().Trim();
            }
            //
            if(ds.Tables[0].Rows[0][1].ToString().Trim()!="")
            {
              yhmc=ds.Tables[0].Rows[0][1].ToString().Trim();
            }
            //
            if(ds.Tables[0].Rows[0][2].ToString().Trim()!="")
            {
                ssbsc=ds.Tables[0].Rows[0][2].ToString().Trim();
            }

            if (ds.Tables[0].Rows[0][3].ToString().Trim() != "")
            {
                lxrsj = ds.Tables[0].Rows[0][3].ToString().Trim();
            }
            if (ds.Tables[0].Rows[0][4].ToString().Trim() != "")
            {
                sf= ds.Tables[0].Rows[0][4].ToString().Trim();
            }
            if (ds.Tables[0].Rows[0][5].ToString().Trim() != "")
            {
                cs = ds.Tables[0].Rows[0][5].ToString().Trim();
            }
            if (ds.Tables[0].Rows[0][6].ToString().Trim() != "")
            {
                qx= ds.Tables[0].Rows[0][6].ToString().Trim();
            }
            if (ds.Tables[0].Rows[0][7].ToString().Trim() != "")
            {
                xxdz = ds.Tables[0].Rows[0][7].ToString().Trim();
            }
            if (ds.Tables[0].Rows[0][8].ToString().Trim() != "")
            {
                lxrxm = ds.Tables[0].Rows[0][8].ToString().Trim();
            }
            if (ds.Tables[0].Rows[0][9].ToString().Trim() != "")
            {
                sfzqyh = ds.Tables[0].Rows[0][9].ToString().Trim();
            }


            //通过《服务站点与推广用户关联表》得到默认分配的服务站点名称 --2012-01-30 刘杰
            string sqls2 = "select top 1 FWZDBH,FWZDMC from FM_FWZDYTGYHGLB where YXX='是'  and GLKHBH='" + yhbh + "' and (GLKHLX='终端用户' or GLKHLX='用户') order by createTime desc";
            string fwzdbh = "", fwzdmc = "";
            DataSet ds2 = null; //// DbHelperSQL.Query(sqls2); 
            if (ds2 != null && ds2.Tables[0].Rows.Count > 0)
            {
                if (ds2.Tables[0].Rows.Count == 1)
                {
                    if (ds2.Tables[0].Rows[0][0].ToString().Trim() != "")
                    {
                        fwzdbh = ds2.Tables[0].Rows[0][0].ToString().Trim();  //得到默认有效的服务站点编号
                    }
                    if (ds2.Tables[0].Rows[0][1].ToString().Trim() != "")
                    {
                        fwzdmc = ds2.Tables[0].Rows[0][1].ToString().Trim();   //得到默认有效的服务站点名称
                    }
                }
            }
            ht.Add("用户编号",yhbh );        //erp中的     
            ht.Add("用户名称", yhmc);        //实际是办事机构名称或者个人
            ht.Add("所属办事处",ssbsc);
            ht.Add("联系人手机", lxrsj);
            ht.Add("省份", sf);
            ht.Add("城市", cs);
            ht.Add("区县", qx);
            ht.Add("详细地址", xxdz);
            ht.Add("联系人姓名", lxrxm);
            ht.Add("是否账期用户", sfzqyh);
            ht.Add("默认服务站点编号", fwzdbh); //2012-01-30 刘杰
            ht.Add("默认服务站点名称", fwzdmc); //2012-01-30 刘杰
            return ht;

        }
        /// <summary>
        /// 返回终端用户的客户信息（使用ERP的同步账号)
        /// </summary>
        /// <returns></returns>
        public Hashtable GetZdyhFromZHInfo()
        {
            string sqls = "select YYYKHBHDYKHBH,YYYGSJGMCKHMC,YYYSSBSCDYSSXSQY,YYYLXSJDYLXRSJ,sheng_str,shi_str,quxian_str,XXDZ,YYYLXRXMDYLXRXM,isNull(SFZQYH,'否') as SFZQYH from FM_UsersZXYH where YYYKHBHDYKHBH='" + dlzh.Trim() + "' and SFDJ='否'";
            string yhbh = "空", yhmc = "空", ssbsc = "空", lxrsj = "空", sf = "空", cs = "空", qx = "空", xxdz = "空", lxrxm = "空", sfzqyh = "否";
            Hashtable ht = new Hashtable();
            DataSet ds = DbHelperSQL.Query(sqls);
            if (ds.Tables[0].Rows[0][0].ToString().Trim() != "")
            {
                yhbh = ds.Tables[0].Rows[0][0].ToString().Trim();
            }
            //
            if (ds.Tables[0].Rows[0][1].ToString().Trim() != "")
            {
                yhmc = ds.Tables[0].Rows[0][1].ToString().Trim();
            }
            //
            if (ds.Tables[0].Rows[0][2].ToString().Trim() != "")
            {
                ssbsc = ds.Tables[0].Rows[0][2].ToString().Trim();
            }

            if (ds.Tables[0].Rows[0][3].ToString().Trim() != "")
            {
                lxrsj = ds.Tables[0].Rows[0][3].ToString().Trim();
            }
            if (ds.Tables[0].Rows[0][4].ToString().Trim() != "")
            {
                sf = ds.Tables[0].Rows[0][4].ToString().Trim();
            }
            if (ds.Tables[0].Rows[0][5].ToString().Trim() != "")
            {
                cs = ds.Tables[0].Rows[0][5].ToString().Trim();
            }
            if (ds.Tables[0].Rows[0][6].ToString().Trim() != "")
            {
                qx = ds.Tables[0].Rows[0][6].ToString().Trim();
            }
            if (ds.Tables[0].Rows[0][7].ToString().Trim() != "")
            {
                xxdz = ds.Tables[0].Rows[0][7].ToString().Trim();
            }
            if (ds.Tables[0].Rows[0][8].ToString().Trim() != "")
            {
                lxrxm = ds.Tables[0].Rows[0][8].ToString().Trim();
            }
            if (ds.Tables[0].Rows[0][9].ToString().Trim() != "")
            {
                sfzqyh = ds.Tables[0].Rows[0][9].ToString().Trim();
            }

            //通过《服务站点与推广用户关联表》得到默认分配的服务站点名称 --2012-01-30 刘杰
            string sqls2 = "select top 1 FWZDBH,FWZDMC from FM_FWZDYTGYHGLB where YXX='是'  and GLKHBH='" + yhbh + "' and (GLKHLX='终端用户' or GLKHLX='用户') order by createTime desc";
            string fwzdbh = "", fwzdmc = "";
            DataSet ds2 = DbHelperSQL.Query(sqls2);
            if (ds2.Tables[0].Rows.Count == 1)
            {
                if (ds2.Tables[0].Rows[0][0].ToString().Trim() != "")
                {
                    fwzdbh = ds2.Tables[0].Rows[0][0].ToString().Trim();  //得到默认有效的服务站点编号
                }
                if (ds2.Tables[0].Rows[0][1].ToString().Trim() != "")
                {
                    fwzdmc = ds2.Tables[0].Rows[0][1].ToString().Trim();   //得到默认有效的服务站点名称
                }
            }
            ht.Add("用户编号", yhbh);        //erp中的     
            ht.Add("用户名称", yhmc);        //实际是办事机构名称或者个人
            ht.Add("所属办事处", ssbsc);
            ht.Add("联系人手机", lxrsj);
            ht.Add("省份", sf);
            ht.Add("城市", cs);
            ht.Add("区县", qx);
            ht.Add("详细地址", xxdz);
            ht.Add("联系人姓名", lxrxm);
            ht.Add("是否账期用户", sfzqyh);
            ht.Add("默认服务站点编号", fwzdbh); //2012-01-30 刘杰
            ht.Add("默认服务站点名称", fwzdmc); //2012-01-30 刘杰
            return ht;

        }
    }
}