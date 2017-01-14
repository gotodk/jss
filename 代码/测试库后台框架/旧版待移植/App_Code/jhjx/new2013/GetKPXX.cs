using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FMOP.DB;
/// <summary>
/// GetKPXX 的摘要说明
/// </summary>
public class GetKPXX
{
	public GetKPXX()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}
    public Hashtable GetKPXXinfo(string dlyx)
    {
        Hashtable ht = new Hashtable();
        ht["执行结果"] = "ok";
        ht["提示文字"] = "基础数据获取成功！";
        ht["开票信息"] = "";
        ht["注册信息"] = "";
        try
        {
            string sql_kpxx = "select a.*,b.id as BGid,b.parentNumber as BGparentNumber,b.BGSQSMJ,b.YBNSRZGZM,b.SQTJSJ,b.CLZT,b.SLBZ,c.id as SHid,c.parentNumber as SHparentNumbe,c.SHSJ,c.SHJG,c.SHXX,d.I_JYFMC,(case when d.I_YBNSRZGZSMJ='DBL.png' then '' else d.I_YBNSRZGZSMJ end) as I_YBNSRZGZSMJ,I_SWDJZSH,I_XXDZ,I_JYFLXDH,I_KHYH,I_YHZH from AAA_PTKPXXB as a left join (select top 1 AAA_PTKPXX_BGSQ.* from AAA_PTKPXX_BGSQ left join AAA_PTKPXXB on AAA_PTKPXX_BGSQ.parentnumber=AAA_PTKPXXB.number where dlyx='" + dlyx + "'  order by sqtjsj desc)  as b on a.number=b.parentnumber left join (select top 1 AAA_PTKPXXB_TJSHXX.* from AAA_PTKPXXB_TJSHXX left join AAA_PTKPXXB on AAA_PTKPXXB_TJSHXX.parentnumber=AAA_PTKPXXB.number where dlyx='" + dlyx + "' order by tjsj desc) as c on c.parentnumber=a.number left join AAA_DLZHXXB as d on a.dlyx=d.B_DLYX where a.dlyx='" + dlyx + "'";
            DataSet dsKPXX = DbHelperSQL.Query(sql_kpxx);
            ht["开票信息"] = dsKPXX.Tables[0];
            string sql_zcxx = "select B_DLYX,I_ZCLB,I_JYFMC,I_SWDJZSH,I_XXDZ,I_JYFLXDH,I_KHYH,I_YHZH,(case when I_YBNSRZGZSMJ='DBL.png' then '' else I_YBNSRZGZSMJ end) as I_YBNSRZGZSMJ,I_LXRXM,I_LXRSJH,S_SFYBJJRSHTG,S_SFYBFGSSHTG from AAA_DLZHXXB where B_DLYX='" + dlyx + "'";
            DataSet dsZCXX = DbHelperSQL.Query(sql_zcxx);
            ht["注册信息"] = dsZCXX.Tables[0];
        }
        catch(Exception ex)
        {
            ht["执行结果"] = "err";
            ht["提示文字"] = "基础数据获取失败！";
        }
        return ht;
    }

    public Hashtable CommitKPXX(DataTable dt)
    {
        Hashtable ht = new Hashtable();
        ht["执行结果"] = "err";
        ht["提示文字"] = "未执行任何操作！";

        //获取买家卖家交易账户的平台管理机构
        string ptgljg = dt.Rows[0]["平台管理机构"].ToString();
        if (dt.Rows[0]["交易账户类型"].ToString() == "买家卖家交易账户")
        {
            string sql = "select I_PTGLJG from AAA_MJMJJYZHYJJRZHGLB as a left join AAA_DLZHXXB as b on a.GLJJRBH =b.J_JJRJSBH where a.SFDQMRJJR ='是' and a.DLYX='" + dt.Rows[0]["登陆邮箱"].ToString() + "'";
            object objCity = DbHelperSQL.GetSingle(sql);
            if (objCity != null && objCity.ToString() != "")
            {
                ptgljg = objCity.ToString();
            }
        }
        if (ptgljg.Trim() == "")
        {
            ht["执行结果"] = "err";
            ht["提示文字"] = "基础数据获取失败，无法执行提交操作！";
        }

        Hashtable htUserVal = PublicClass2013.GetUserInfo(dt.Rows[0]["买家角色编号"].ToString());//通过买家角色编号获取信息

        if (htUserVal["是否休眠"].ToString().Trim() == "是")
        {
            ht["执行结果"] = "err";
            ht["提示文字"] = "您的交易账户处于休眠状态，请进入账户维护中激活休眠账户！";
            return ht;
        }

        if (dt.Rows[0]["Number"].ToString() == "")
        {//提交新的开票信息
            try
            {
                string KeyNumber = PublicClass2013.GetNextNumberZZ("AAA_PTKPXXB", "");
                string sql_insertZB = "INSERT INTO [AAA_PTKPXXB]([Number],[DLYX],[KHBH],[JYZHLX],[ZCLB],[PTGLJG],[FPLX],[DWMC],[YBNSRSBH],[DWDZ],[LXDH],[KHH],[KHZH],[YBNSRZGZ],[FPJSDWMC],[FPJSDZ],[FPJSLXR],[FPJSLXRDH],[ZT],[ZHGXSJ],[CheckState],[CreateUser],[CreateTime],[CheckLimitTime])  VALUES ('" + KeyNumber + "','" + dt.Rows[0]["登陆邮箱"].ToString() + "','" + dt.Rows[0]["客户编号"].ToString() + "','" + dt.Rows[0]["交易账户类型"].ToString() + "','" + dt.Rows[0]["注册类别"].ToString() + "','" + ptgljg + "','" + dt.Rows[0]["发票类型"].ToString() + "','" + dt.Rows[0]["单位名称"].ToString() + "','" + dt.Rows[0]["纳税人识别号"].ToString() + "','" + dt.Rows[0]["单位地址"].ToString() + "','" + dt.Rows[0]["联系电话"].ToString() + "','" + dt.Rows[0]["开户行"].ToString() + "','" + dt.Rows[0]["开户账号"].ToString() + "','" + dt.Rows[0]["一般纳税人资格证"].ToString() + "','" + dt.Rows[0]["发票接收单位名称"].ToString() + "','" + dt.Rows[0]["发票接收地址"].ToString() + "','" + dt.Rows[0]["发票接收联系人"].ToString() + "','" + dt.Rows[0]["发票接收联系人电话"].ToString() + "','待审核','" + DateTime.Now.ToString() + "',0,'admin','" + DateTime.Now.ToString() + "','" + DateTime.Now.ToString() + "')";
                int count = DbHelperSQL.ExecuteSql(sql_insertZB);
                if (count > 0)
                {
                    ht["执行结果"] = "ok";
                    ht["提示文字"] = "开票信息提交成功！";
                }
                else
                {
                    ht["执行结果"] = "err";
                    ht["提示文字"] = "开票信息提交失败！";
                }
            }
            catch (Exception ex)
            {
                ht["执行结果"] = "err";
                ht["提示文字"] = "开票信息提交失败！";
            }
        }
        else
        { //修改原来的开票信息
            try
            {
                //先判断当前的审核状态，
                object objSHZT = DbHelperSQL.GetSingle("select zt from AAA_PTKPXXB where number='" + dt.Rows[0]["Number"].ToString() + "'");
                string new_zt = "";
                if (objSHZT != null)
                {
                    new_zt = objSHZT.ToString();

                }
                string old_zt = dt.Rows[0]["原状态"].ToString();
                if (new_zt == "已生效" && new_zt != old_zt)
                {
                    ht["执行结果"] = "ok";
                    ht["提示文字"] = "您的开票信息已经审核通过，如需变更，请提交变更申请！";
                }
                else if (new_zt == "驳回" && new_zt != old_zt)
                {
                    ht["执行结果"] = "ok";
                    ht["提示文字"] = "您的开票信息未通过审核，请查看原因，并重新修改！";
                }
                else
                {
                    string sql_update = "UPDATE [AAA_PTKPXXB]  SET [FPLX]='" + dt.Rows[0]["发票类型"].ToString() + "',[DWMC]='" + dt.Rows[0]["单位名称"].ToString() + "',[YBNSRSBH]='" + dt.Rows[0]["纳税人识别号"].ToString() + "', [DWDZ] = '" + dt.Rows[0]["单位地址"].ToString() + "',[LXDH] ='" + dt.Rows[0]["联系电话"].ToString() + "',[KHH] = '" + dt.Rows[0]["开户行"].ToString() + "',[KHZH] ='" + dt.Rows[0]["开户账号"].ToString() + "',[YBNSRZGZ] = '" + dt.Rows[0]["一般纳税人资格证"].ToString() + "',[FPJSDWMC] = '" + dt.Rows[0]["发票接收单位名称"].ToString() + "',[FPJSDZ] ='" + dt.Rows[0]["发票接收地址"].ToString() + "',[FPJSLXR] = '" + dt.Rows[0]["发票接收联系人"].ToString() + "',[FPJSLXRDH] = '" + dt.Rows[0]["发票接收联系人电话"].ToString() + "',[ZT] ='待审核',[ZHGXSJ] = '" + DateTime.Now.ToString() + "' WHERE Number='" + dt.Rows[0]["Number"].ToString() + "'";
                    DbHelperSQL.ExecuteSql(sql_update);
                    ht["执行结果"] = "ok";
                    ht["提示文字"] = "开票信息修改成功！";
                }
            }
            catch (Exception ex)
            {
                ht["执行结果"] = "err";
                ht["提示文字"] = "开票信息修改失败！";
            }
        }

        return ht;
    }

    public Hashtable CommitKPXXChange(DataTable dt)
    {
        Hashtable ht = new Hashtable();
        ht["执行结果"] = "err";
        ht["提示文字"] = "未执行任何操作！";



        Hashtable htUserVal = PublicClass2013.GetUserInfo(dt.Rows[0]["买家角色编号"].ToString());//通过买家角色编号获取信息

        if (htUserVal["是否休眠"].ToString().Trim() == "是")
        {
            ht["执行结果"] = "err";
            ht["提示文字"] = "您的交易账户处于休眠状态，请进入账户维护中激活休眠账户！";
            return ht;
        }

        if (dt.Rows[0]["ID"].ToString() == "")
        {//提交新的变更信息
            try
            {
                string sql_insert = "INSERT INTO [AAA_PTKPXX_BGSQ]([parentNumber],[BGSQSMJ],[YBNSRZGZM],[SQTJSJ],[CLZT]) VALUES ('" + dt.Rows[0]["Number"].ToString() + "','" + dt.Rows[0]["变更信息扫描件"].ToString() + "','"+dt.Rows [0]["一般纳税人资格证明"].ToString ()+"','" + DateTime.Now.ToString() + "','待处理')";
                DbHelperSQL.ExecuteSql(sql_insert);

                ht["执行结果"] = "ok";
                ht["提示文字"] = "变更信息提交成功！";
            }
            catch (Exception ex)
            {
                ht["执行结果"] = "err";
                ht["提示文字"] = "变更信息提交失败！";
            }
        }
        else
        { //修改原来的变更信息
            object objzt = DbHelperSQL.GetSingle("select clzt from AAA_PTKPXX_BGSQ where id='" + dt.Rows[0]["ID"].ToString() + "'");

            string new_zt = "";//当前单据的处理状态
            if (objzt != null)
            {
                new_zt = objzt.ToString();
            }
            string old_zt = dt.Rows[0]["原状态"].ToString();//原始绑定在界面上的时候的处理状态
            //两个状态不一致的时候，需要特殊处理，防止后台审核了前台还可以修改的情况
            if (new_zt == old_zt)
            {                
                if (old_zt == "待处理")
                {
                    //变更信息还没有审核的，直接更新原来的数据即可
                    try
                    {
                        string sql_update = "update AAA_PTKPXX_BGSQ set bgsqsmj='" + dt.Rows[0]["变更信息扫描件"].ToString() + "',ybnsrzgzm='" + dt.Rows[0]["一般纳税人资格证明"].ToString() + "',sqtjsj='" + DateTime.Now.ToString() + "' where parentnumber='" + dt.Rows[0]["Number"].ToString() + "' and id='" + dt.Rows[0]["ID"].ToString() + "'";
                        DbHelperSQL.ExecuteSql(sql_update);
                        ht["执行结果"] = "ok";
                        ht["提示文字"] = "变更信息提交成功！";
                    }
                    catch (Exception ex)
                    {
                        ht["执行结果"] = "err";
                        ht["提示文字"] = "变更信息提交失败！";
                    }
                }
                else if (old_zt == "驳回")
                {
                    //如果原来的变更信息为驳回的，则为保留原来的审核信息，修改的数据新插入一条
                    try
                    {
                        string sql_insert = "INSERT INTO [AAA_PTKPXX_BGSQ]([parentNumber],[BGSQSMJ],[YBNSRZGZM],[SQTJSJ],[CLZT]) VALUES ('" + dt.Rows[0]["Number"].ToString() + "','" + dt.Rows[0]["变更信息扫描件"].ToString() + "','" + dt.Rows[0]["一般纳税人资格证明"].ToString() + "','" + DateTime.Now.ToString() + "','待处理')";
                        DbHelperSQL.ExecuteSql(sql_insert);

                        ht["执行结果"] = "ok";
                        ht["提示文字"] = "变更信息提交成功！";
                    }
                    catch (Exception ex)
                    {
                        ht["执行结果"] = "err";
                        ht["提示文字"] = "变更信息提交失败！";
                    }
                }
            }
            else//新旧处理状态不一致了
            {
                if (new_zt == "已修改")
                {
                    ht["执行结果"] = "ok";
                    ht["提示文字"] = "您的变更申请已经处理完毕，请核对开票信息！";
                }
                else if (new_zt == "驳回")
                {
                    ht["执行结果"] = "ok";
                    ht["提示文字"] = "您的变更申请未通过审核，请查看原因，并重新提交变更资料！";
                }            
            }                
        }
        return ht;
    }
}