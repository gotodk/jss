using System;
using System.Collections.Generic;
using System.Web;
using System.Collections;
using FMOP.DB;
using System.Data;
/// <summary>
///tgyhClass 的摘要说明
/// </summary>
public class tgyhClass
{
    string dlyx = "";
    public tgyhClass()
    {
        //
        //TODO: 在此处添加构造函数逻辑
        //

    }
    public tgyhClass(string dlyx)
    {
        this.dlyx = dlyx;

    }
    /// <summary>
    /// 返回推广人用户的客户信息
    /// </summary>
    /// <returns></returns>
    public Hashtable GettgyhInfo()
    {
        string sqls = "select TGRBH,YYYGSJGMCKHMC,YYYSSBSCDYSSXSQY,YYYLXSJDYLXRSJ,sheng_str,shi_str,quxian_str,XXDZ,TGRYXM from FM_UsersTGRY where DLZH='" + dlyx.Trim() + "' and SFDJ='否' and TGRBH is not null";
        string yhbh = "空", yhmc = "空", ssbsc = "空", lxrsj = "空", sf = "空", cs = "空", qx = "空", xxdz = "空", lxrxm = "空";
        Hashtable ht = new Hashtable();
        DataSet ds = DbHelperSQL.Query(sqls);
        if (ds.Tables[0].Rows[0][0].ToString().Trim() != "")
        {
            yhbh = ds.Tables[0].Rows[0][0].ToString().Trim();
        }
        if (ds.Tables[0].Rows[0][1].ToString().Trim() != "")
        {
            yhmc = ds.Tables[0].Rows[0][1].ToString().Trim();
        }
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
        //通过《服务站点与推广用户关联表》得到默认分配的服务站点名称 --2012-01-30 刘杰
        string sqls2 = "select top 1 FWZDBH,FWZDMC from FM_FWZDYTGYHGLB where YXX='是'  and GLKHBH='" + yhbh + "' and (GLKHLX='推广人员') order by createTime desc";
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
        
        ht.Add("用户编号", yhbh);    //推广人编号就是推广人用户编号
        ht.Add("用户名称", yhmc);    //YYY公司机构名称是(用户名称)
        ht.Add("所属办事处", ssbsc);
        ht.Add("联系人手机", lxrsj);
        ht.Add("省份", sf);
        ht.Add("城市", cs);
        ht.Add("区县", qx);
        ht.Add("详细地址", xxdz);
        ht.Add("联系人姓名", lxrxm);
        ht.Add("默认服务站点编号", fwzdbh); //2012-01-30 刘杰
        ht.Add("默认服务站点名称", fwzdmc); //2012-01-30 刘杰

        return ht;

    }

}