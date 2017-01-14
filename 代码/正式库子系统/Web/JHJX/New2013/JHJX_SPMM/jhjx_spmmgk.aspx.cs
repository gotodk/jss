using FMOP.DB;
using Hesion.Brick.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Web_JHJX_New2013_JHJX_SPMM_jhjx_spmmgk : System.Web.UI.Page
{
        string StrGettbdsql = "";
        string StrGettbdchaifen = "";
        string Stryddzhengchang = "";
        string Stryddchaidan = "";
        Hashtable ht_where = new Hashtable();
    protected void Page_Load(object sender, EventArgs e)
    {
       
        commonpagernew1.OnNeedLoadData += new Web_pagerdemo_commonpagernew.OnNeedDataHandler(MyWebControl_OnNeedLoadData);
        if (!IsPostBack)
        {
 

            //setchengshi();
            this.UCFWJGDetail1.initdefault();
             DisGrid();
        }
    }

    protected void setchengshi()
    {
        /*
        string MainERP = DbHelperSQL.GetSingle("SELECT DYERP FROM SYSTEM_CITY_0 WHERE NAME='公司总部'").ToString();//总部对应的ERP名称
        DataSet ds = DbHelperSQL.Query("SELECT DYFGSMC FROM System_City where DYFGSMC != '' and name<>'其他办事处' ");
        ddlssfgs.DataSource = ds.Tables[0].DefaultView;
        ddlssfgs.DataTextField = "DYFGSMC";
        ddlssfgs.DataValueField = "DYFGSMC";
        ddlssfgs.DataBind();
        ddlssfgs.Items.Insert(0, new ListItem("全部分公司", ""));
        string sql = "select bm from hr_employees where number='" + User.Identity.Name + "'";
        string bm = DbHelperSQL.GetSingle(sql).ToString();
        if (bm.IndexOf("办事处") > 0)
        {
            sql = "select count(name) from system_city where name='" + bm + "'";
            string fgsSql = "select DYFGSMC from system_city where name='" + bm + "'";
            if (Convert.ToInt32(DbHelperSQL.GetSingle(sql)) > 0)
            {
                string strFGSMC = DbHelperSQL.GetSingle(fgsSql).ToString();
                ddlssfgs.DataSource = null;
                ddlssfgs.DataBind();
                ddlssfgs.Items.Clear();
                ddlssfgs.Items.Add(strFGSMC);

            }
        }
         * */
    }

    private void MyWebControl_OnNeedLoadData(DataSet NewDS, string ERRinfo)
    {
        DataTable dt = new DataTable();
        if (ERRinfo.IndexOf("超时") >= 0)
        {
            MessageBox.ShowAlertAndBack(this, "查询超时，请重试或者修改查询条件！");
        }
        if (NewDS != null && NewDS.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < NewDS.Tables[0].Rows.Count; i++)
            {
                #region 获取当前最低价、经济批量、达成率
                //获取当前商品的卖家最低价及相关数据
                DataSet ds_lowprice = DbHelperSQL.Query("select top 1 number as 投标单号, TBJG as 投标价格,TBNSL as 最低价投标拟售量,YZBSL as 已中标数量,MJSDJJPL as 经济批量,GHQY as 供货区域  from AAA_TBD where ZT='竞标' and SPBH='" + NewDS.Tables[0].Rows[i]["商品编号"].ToString() + "' and HTQX='" + NewDS.Tables[0].Rows[i]["合同期限"].ToString() + "'  order by TBJG,TJSJ");
                if (ds_lowprice != null && ds_lowprice.Tables[0].Rows.Count > 0)
                {
                    NewDS.Tables[0].Rows[i]["当前卖家最低价"] = ds_lowprice.Tables[0].Rows[0]["投标价格"].ToString();
                    NewDS.Tables[0].Rows[i]["最低价标的经济批量"] = ds_lowprice.Tables[0].Rows[0]["经济批量"].ToString();

                    //即时合同的达成率和三个月/一年的计算公式不同
                    if (NewDS.Tables[0].Rows[i]["合同期限"].ToString() == "即时")
                    {
                        NewDS.Tables[0].Rows[i]["最低价标的达成率"] = (Convert.ToDouble(ds_lowprice.Tables[0].Rows[0]["已中标数量"].ToString()) / Convert.ToDouble(ds_lowprice.Tables[0].Rows[0]["最低价投标拟售量"].ToString()) * 100.00).ToString("#0.00");
                    }
                    else
                    {
                        //获取最低价标的拟购买数量和
                        string sql = "select isnull(sum(NDGSL-YZBSL),0) from AAA_YDDXXB where SPBH='" + NewDS.Tables[0].Rows[i]["商品编号"].ToString() + "' and  HTQX='" + NewDS.Tables[0].Rows[i]["合同期限"].ToString() + "' and ZT = '竞标' and NMRJG >=convert(decimal(18,2),'" + ds_lowprice.Tables[0].Rows[0]["投标价格"].ToString() + "') and 0<CHARINDEX ('|'+SHQYsheng+'|',isnull('" + ds_lowprice.Tables[0].Rows[0]["供货区域"].ToString() + "',''))";
                        object obj_NDGSL = DbHelperSQL.GetSingle(sql);
                        NewDS.Tables[0].Rows[i]["最低价标的达成率"] = (Convert.ToDouble(obj_NDGSL.ToString()) / Convert.ToDouble(ds_lowprice.Tables[0].Rows[0]["最低价投标拟售量"].ToString()) * 100.00).ToString("#0.00");
                    }
                }
                #endregion
                #region 确定应该显示的当前状态
                if (NewDS.Tables[0].Rows[i]["单据状态"].ToString() == "竞标")
                {
                    if (NewDS.Tables[0].Rows[i]["合同期限"].ToString() == "即时")
                    {
                        NewDS.Tables[0].Rows[i]["当前状态"] = "竞标中";
                    }
                    else
                    {
                        //三个月/一年的，判断是否进入冷静期
                        object objLJQ = DbHelperSQL.GetSingle("select SFJRLJQ from AAA_LJQDQZTXXB where SPBH='" + NewDS.Tables[0].Rows[i]["商品编号"].ToString() + "' and HTQX='" + NewDS.Tables[0].Rows[i]["合同期限"].ToString() + "'");
                        if (objLJQ != null && objLJQ.ToString() != "")
                        {
                            NewDS.Tables[0].Rows[i]["当前状态"] = objLJQ.ToString() == "是" ? "竞标中（冷静期）" : "竞标中";
                        }
                    }
                }
                else
                {
                    //从中标定标信息表中获取的数据的状态对应
                    switch (NewDS.Tables[0].Rows[i]["单据状态"].ToString())
                    {
                        case "定标合同到期":
                            NewDS.Tables[0].Rows[i]["当前状态"] = "定标";
                            break;
                        case "定标执行完成":
                            NewDS.Tables[0].Rows[i]["当前状态"] = "定标";
                            break;
                        case "未定标废标":
                            NewDS.Tables[0].Rows[i]["当前状态"] = "废标";
                            break;
                        case "定标合同终止":
                            NewDS.Tables[0].Rows[i]["当前状态"] = "废标";
                            break;
                        default:
                            NewDS.Tables[0].Rows[i]["当前状态"] = NewDS.Tables[0].Rows[i]["单据状态"].ToString ();
                            break;
                    }
                    //获取是否拆单
                    string sql_cd = "";
                    if (NewDS.Tables[0].Rows[i]["单据类型"].ToString () == "预订单")
                    {
                        sql_cd = "select SFCD from AAA_YDDXXB where Number='" + NewDS.Tables[0].Rows[i]["单据编号"].ToString() + "'";
                    }
                    else
                    {
                        sql_cd = "select SFCD from AAA_TBD where Number='" + NewDS.Tables[0].Rows[i]["单据编号"].ToString() + "'";
                    }
                    object obj_SFCD = DbHelperSQL.GetSingle(sql_cd);
                    if (obj_SFCD != null && obj_SFCD.ToString() != "")
                    {
                        NewDS.Tables[0].Rows[i]["是否拆单"] = obj_SFCD.ToString();
                    }
                }
                #endregion
            }
            tdEmpty.Visible = false;
            dt = NewDS.Tables[0];
            rptSPXX.DataSource = dt.DefaultView;
        }
        else { tdEmpty.Visible = true; }
        rptSPXX.DataBind();
    }

    //设置初始默认值检索
    private Hashtable SetV()
    {
        #region 原始sql语句
        // StrGettbdsql = "(select a.DLYX as '交易方账号',(select I_JYFMC  from  AAA_DLZHXXB where B_DLYX = a.DLYX) as '交易方名称',(select I_PTGLJG  from  AAA_DLZHXXB where B_DLYX =  (select distinct GLJJRDLZH from  AAA_MJMJJYZHYJJRZHGLB where DLYX = a.DLYX and SFDQMRJJR='是')) as 业务管理部门,(select I_YWGLBMFL  from  AAA_DLZHXXB where B_DLYX =  (select distinct GLJJRDLZH from  AAA_MJMJJYZHYJJRZHGLB where DLYX = a.DLYX and SFDQMRJJR='是')) as 业务管理部门分类 , ('T'+a.Number) as Number, CONVERT(varchar(100),a.CreateTime , 20)  as '下单时间',a.SPBH as '商品编号',SPMC as '商品名称',GG as '规格',a.HTQX as '合同期限',(isnull(TBNSL,0)-isnull(YZBSL,0)) as '数量',TBJG as '价格',TBJE as '金额', case when (( select MIN(TBJG) from AAA_TBD  where  ZT= '竞标' and SPBH=a.SPBH and HTQX=a.HTQX )  )  is null then '---' else CAST ( (( select MIN(TBJG) from AAA_TBD  where  ZT= '竞标' and SPBH=a.SPBH and HTQX=a.HTQX ) ) as varchar(200)) end  as '当前卖家最低价', case when ( select top 1  MJSDJJPL from AAA_TBD  where TBJG=( select MIN(TBJG) from AAA_TBD  where  ZT= '竞标' and SPBH=a.SPBH and HTQX=a.HTQX )  and ZT= '竞标'  and SPBH=a.SPBH and HTQX=a.HTQX  order by MJSDJJPL, CreateTime ) IS NULL then '---' else CAST ( ( select top 1  MJSDJJPL from AAA_TBD  where TBJG=( select MIN(TBJG) from AAA_TBD  where  ZT= '竞标' and SPBH=a.SPBH and HTQX=a.HTQX )   and ZT= '竞标'  and SPBH=a.SPBH and HTQX=a.HTQX order by MJSDJJPL, CreateTime ) as varchar(200)) end   as '最低价标的经济批量',(CAST(ISNULL(((SELECT SUM(isnull(NDGSL,0)-isnull(YZBSL,0)) FROM AAA_YDDXXB WHERE AAA_YDDXXB.SPBH=a.SPBH AND AAA_YDDXXB.HTQX=a.HTQX AND AAA_YDDXXB.ZT='竞标' AND AAA_YDDXXB.NMRJG>= ( select MIN(TBJG) from AAA_TBD  where  ZT= '竞标' and SPBH=a.SPBH and HTQX=a.HTQX )  AND CHARINDEX('|'+SHQYsheng+'|',(SELECT TOP 1 GHQY FROM AAA_TBD WHERE ZT='竞标' AND AAA_TBD.SPBH=a.SPBH AND AAA_TBD.HTQX = a.HTQX ORDER BY AAA_TBD.TBJG,AAA_TBD.CreateTime ASC))>0)/ (SELECT TOP 1 (isnull(TBNSL,0)-isnull(YZBSL,0)) FROM AAA_TBD WHERE ZT='竞标' AND AAA_TBD.SPBH=a.SPBH AND AAA_TBD.HTQX = a.HTQX ORDER BY AAA_TBD.TBJG,AAA_TBD.CreateTime ASC)),0)*100  AS numeric(18, 2))) as '最低价标的拟售量达成率','投标单' as '单据类型',a.Number as '单据编号' ,  case  a.ZT when '竞标' then case b.SFJRLJQ  when '是' then '竞标中（冷静期）' else '竞标中'  end end as '单据状态',case  when YZBSL =0 then '否' when YZBSL>0 and YZBSL != TBNSL then '是' when YZBSL>0 and YZBSL = TBNSL and (select Count(Number) from AAA_ZBDBXXB where T_YSTBDBH = a.Number) >1 then '是' else '否'  end  as '是否拆单', (case a.HTQX when '即时' then '---' else   b.SFJRLJQ end) as '是否在冷静期','' AS '中标定标信息表状态'  from AAA_TBD as a left join AAA_LJQDQZTXXB as b on a.SPBH=b.SPBH and a.HTQX = b.HTQX   where  ZT= '竞标' ) ";

        //StrGettbdchaifen = "(select a.DLYX as '交易方账号',(select I_JYFMC  from  AAA_DLZHXXB where B_DLYX = a.DLYX) as '交易方名称',(select I_PTGLJG  from  AAA_DLZHXXB where B_DLYX =  (select distinct GLJJRDLZH from  AAA_MJMJJYZHYJJRZHGLB where DLYX = a.DLYX and SFDQMRJJR='是')) as 业务管理部门,(select I_YWGLBMFL  from  AAA_DLZHXXB where B_DLYX =  (select distinct GLJJRDLZH from  AAA_MJMJJYZHYJJRZHGLB where DLYX = a.DLYX and SFDQMRJJR='是')) as 业务管理部门分类 ,('T'+a.Number+c.Number) as Number, CONVERT(varchar(100),a.CreateTime , 20)  as '下单时间',a.SPBH as '商品编号',SPMC as '商品名称',GG as '规格',a.HTQX as '合同期限', c.Z_ZBSL as '数量',Z_ZBJG  as '价格',c.Z_ZBJE as '金额', case when (( select MIN(TBJG) from AAA_TBD  where  ZT= '竞标' and SPBH=a.SPBH and HTQX=a.HTQX )  )  is null then '---' else CAST ( (( select MIN(TBJG) from AAA_TBD  where  ZT= '竞标' and SPBH=a.SPBH and HTQX=a.HTQX ) ) as varchar(200)) end  as '当前卖家最低价', case when ( select top 1  MJSDJJPL from AAA_TBD  where TBJG=( select MIN(TBJG) from AAA_TBD  where  ZT= '竞标' and SPBH=a.SPBH and HTQX=a.HTQX )  and ZT= '竞标'  and SPBH=a.SPBH and HTQX=a.HTQX  order by MJSDJJPL, CreateTime ) IS NULL then '---' else CAST ( ( select top 1  MJSDJJPL from AAA_TBD  where TBJG=( select MIN(TBJG) from AAA_TBD  where  ZT= '竞标' and SPBH=a.SPBH and HTQX=a.HTQX )   and ZT= '竞标'  and SPBH=a.SPBH and HTQX=a.HTQX order by MJSDJJPL, CreateTime ) as varchar(200)) end   as '最低价标的经济批量',(CAST(ISNULL(((SELECT SUM(isnull(NDGSL,0)-isnull(YZBSL,0)) FROM AAA_YDDXXB WHERE AAA_YDDXXB.SPBH=a.SPBH AND AAA_YDDXXB.HTQX=a.HTQX AND AAA_YDDXXB.ZT='竞标' AND AAA_YDDXXB.NMRJG>= ( select MIN(TBJG) from AAA_TBD  where  ZT= '竞标' and SPBH=a.SPBH and HTQX=a.HTQX )  AND CHARINDEX('|'+SHQYsheng+'|',(SELECT TOP 1 GHQY FROM AAA_TBD WHERE ZT='竞标' AND AAA_TBD.SPBH=a.SPBH AND AAA_TBD.HTQX = a.HTQX ORDER BY AAA_TBD.TBJG,AAA_TBD.CreateTime ASC))>0)/ (SELECT TOP 1 (isnull(TBNSL,0)-isnull(YZBSL,0)) FROM AAA_TBD WHERE ZT='竞标' AND AAA_TBD.SPBH=a.SPBH AND AAA_TBD.HTQX = a.HTQX ORDER BY AAA_TBD.TBJG,AAA_TBD.CreateTime ASC)),0)*100  AS numeric(18, 2))) as '最低价标的拟售量达成率','投标单' as '单据类型',a.Number as '单据编号' , case Z_HTZT when '中标' then '中标' when '未定标废标' then '废标'  when '定标合同终止' then '废标' when '定标' then '定标' when '定标合同到期' then '定标' when '定标执行完成' then '定标'  else '其他' end  as '单据状态' , case  when YZBSL =0 then '否' when YZBSL>0 and YZBSL != TBNSL then '是' when YZBSL>0 and YZBSL = TBNSL and (select Count(Number) from AAA_ZBDBXXB where T_YSTBDBH =a.Number)>1 then '是' else '否'  end  as '是否拆单', (case  a.HTQX when '即时' then '---' else  b.SFJRLJQ end ) as '是否在冷静期',c.Z_HTZT AS '中标定标信息表状态'  from AAA_ZBDBXXB as c  left join AAA_LJQDQZTXXB as b on c.Z_SPBH=b.SPBH and c.Z_HTQX = b.HTQX left join AAA_TBD as a on a.Number = c.T_YSTBDBH  ) ";

        //Stryddzhengchang = "(select a.DLYX as '交易方账号',(select I_JYFMC  from  AAA_DLZHXXB where B_DLYX = a.DLYX) as '交易方名称',(select I_PTGLJG  from  AAA_DLZHXXB where B_DLYX =  (select distinct GLJJRDLZH from  AAA_MJMJJYZHYJJRZHGLB where DLYX = a.DLYX and SFDQMRJJR='是')) as 业务管理部门,(select I_YWGLBMFL  from  AAA_DLZHXXB where B_DLYX =  (select distinct GLJJRDLZH from  AAA_MJMJJYZHYJJRZHGLB where DLYX = a.DLYX and SFDQMRJJR='是')) as 业务管理部门分类 , ('Y'+a.Number) as Number, CONVERT(varchar(100),a.CreateTime , 20) as '下单时间',a.SPBH as '商品编号',SPMC as '商品名称',GG as '规格',a.HTQX as '合同期限', (isnull(NDGSL,0)-isnull(YZBSL,0)) as '数量',NMRJG as '价格',NDGJE as  金额, case when ( select MIN(TBJG) from AAA_TBD  where  ZT= '竞标' and SPBH=a.SPBH and HTQX=a.HTQX ) is null then '---' else CAST( ( select MIN(TBJG) from AAA_TBD  where  ZT= '竞标' and SPBH=a.SPBH and HTQX=a.HTQX ) as varchar(200)) end  as '当前卖家最低价', case when ( select top 1  MJSDJJPL from AAA_TBD  where TBJG=( select MIN(TBJG) from AAA_TBD  where  ZT= '竞标' and SPBH=a.SPBH and HTQX=a.HTQX )   and ZT= '竞标'  and SPBH=a.SPBH and HTQX=a.HTQX order by MJSDJJPL,  CreateTime ) is null then '---' else CAST ( ( select top 1  MJSDJJPL from AAA_TBD  where TBJG=( select MIN(TBJG) from AAA_TBD  where  ZT= '竞标' and SPBH=a.SPBH and HTQX=a.HTQX )   and ZT= '竞标'  and SPBH=a.SPBH and HTQX=a.HTQX order by MJSDJJPL, CreateTime ) as varchar(200) ) end as '最低价标的经济批量',(CAST(ISNULL(((SELECT SUM((isnull(NDGSL,0)-isnull(YZBSL,0))) FROM AAA_YDDXXB WHERE AAA_YDDXXB.SPBH=a.SPBH AND AAA_YDDXXB.HTQX=a.HTQX AND AAA_YDDXXB.ZT='竞标' AND AAA_YDDXXB.NMRJG>= ( select MIN(TBJG) from AAA_TBD  where  ZT= '竞标' and SPBH=a.SPBH and HTQX=a.HTQX )  AND CHARINDEX('|'+SHQYsheng+'|',(select top 1  GHQY from AAA_TBD  where TBJG=( select MIN(TBJG) from AAA_TBD  where  ZT= '竞标' and SPBH=a.SPBH and HTQX=a.HTQX )   and ZT= '竞标' and SPBH=a.SPBH and HTQX=a.HTQX  order by CreateTime))>0)/ (SELECT TOP 1 (isnull(TBNSL,0)-isnull(YZBSL,0)) FROM AAA_TBD WHERE ZT='竞标' AND AAA_TBD.SPBH=a.SPBH AND AAA_TBD.HTQX = a.HTQX ORDER BY AAA_TBD.TBJG,AAA_TBD.CreateTime ASC)),0)*100 AS numeric(18, 2))) as '最低价标的拟售量达成率','预订单' as '单据类型' ,a.Number as '单据编号' ,  case  a.ZT when '竞标' then case b.SFJRLJQ  when '是' then '竞标中（冷静期）' else '竞标中' end   else  case Z_HTZT when '中标' then '中标' when '未定标废标' then '废标'  when '定标合同终止' then '废标' when '定标' then '定标' when '定标合同到期' then '定标' when '定标执行完成' then '定标'  else '其他' end end as '单据状态',  case  when YZBSL =0 then '否' when YZBSL>0 and YZBSL != NDGSL then '是' when YZBSL>0 and YZBSL = NDGSL and (select Count(Number) from AAA_ZBDBXXB where Y_YSYDDBH =a.Number)>1 then '是' else '否'  end  as '是否拆单',  (case a.HTQX when '即时' then '---' else   b.SFJRLJQ end) as '是否在冷静期',c.Z_HTZT AS '中标定标信息表状态'  from AAA_YDDXXB as a left join AAA_LJQDQZTXXB as b on a.SPBH=b.SPBH and a.HTQX = b.HTQX left join AAA_ZBDBXXB as c on a.Number = c.Y_YSYDDBH  where   ZT='竞标'  )";


       // Stryddchaidan = "(select a.DLYX as '交易方账号',(select I_JYFMC  from  AAA_DLZHXXB where B_DLYX = a.DLYX) as '交易方名称',(select I_PTGLJG  from  AAA_DLZHXXB where B_DLYX =  (select distinct GLJJRDLZH from  AAA_MJMJJYZHYJJRZHGLB where DLYX = a.DLYX and SFDQMRJJR='是')) as 业务管理部门,(select I_YWGLBMFL  from  AAA_DLZHXXB where B_DLYX =  (select distinct GLJJRDLZH from  AAA_MJMJJYZHYJJRZHGLB where DLYX = a.DLYX and SFDQMRJJR='是')) as 业务管理部门分类 , ('Y'+a.Number+c.Number) as Number, CONVERT(varchar(100),a.CreateTime , 20) as '下单时间',a.SPBH as '商品编号',SPMC as '商品名称',GG as '规格',a.HTQX as '合同期限', c.Z_ZBSL as '数量',Z_ZBJG as '价格',c.Z_ZBJE as  金额, case when ( select MIN(TBJG) from AAA_TBD  where  ZT= '竞标' and SPBH=a.SPBH and HTQX=a.HTQX ) is null then '---' else CAST( ( select MIN(TBJG) from AAA_TBD  where  ZT= '竞标' and SPBH=a.SPBH and HTQX=a.HTQX ) as varchar(200)) end  as '当前卖家最低价', case when ( select top 1  MJSDJJPL from AAA_TBD  where TBJG=( select MIN(TBJG) from AAA_TBD  where  ZT= '竞标' and SPBH=a.SPBH and HTQX=a.HTQX )   and ZT= '竞标'  and SPBH=a.SPBH and HTQX=a.HTQX order by MJSDJJPL,  CreateTime ) is null then '---' else CAST ( ( select top 1  MJSDJJPL from AAA_TBD  where TBJG=( select MIN(TBJG) from AAA_TBD  where  ZT= '竞标' and SPBH=a.SPBH and HTQX=a.HTQX )   and ZT= '竞标'  and SPBH=a.SPBH and HTQX=a.HTQX order by MJSDJJPL, CreateTime ) as varchar(200) ) end as '最低价标的经济批量',(CAST(ISNULL(((SELECT SUM((isnull(NDGSL,0)-isnull(YZBSL,0))) FROM AAA_YDDXXB WHERE AAA_YDDXXB.SPBH=a.SPBH AND AAA_YDDXXB.HTQX=a.HTQX AND AAA_YDDXXB.ZT='竞标' AND AAA_YDDXXB.NMRJG>= ( select MIN(TBJG) from AAA_TBD  where  ZT= '竞标' and SPBH=a.SPBH and HTQX=a.HTQX )  AND CHARINDEX('|'+SHQYsheng+'|',(select top 1  GHQY from AAA_TBD  where TBJG=( select MIN(TBJG) from AAA_TBD  where  ZT= '竞标' and SPBH=a.SPBH and HTQX=a.HTQX )   and ZT= '竞标' and SPBH=a.SPBH and HTQX=a.HTQX  order by CreateTime))>0)/ (SELECT TOP 1 (isnull(TBNSL,0)-isnull(YZBSL,0)) FROM AAA_TBD WHERE ZT='竞标' AND AAA_TBD.SPBH=a.SPBH AND AAA_TBD.HTQX = a.HTQX ORDER BY AAA_TBD.TBJG,AAA_TBD.CreateTime ASC)),0)*100 AS numeric(18, 2))) as '最低价标的拟售量达成率','预订单' as '单据类型' ,a.Number as '单据编号' ,  case Z_HTZT when '中标' then '中标' when '未定标废标' then '废标'  when '定标合同终止' then '废标' when '定标' then '定标' when '定标合同到期' then '定标' when '定标执行完成' then '定标'  else '其他' end  as '单据状态', case  when YZBSL =0 then '否' when YZBSL>0 and YZBSL != NDGSL then '是' when YZBSL>0 and YZBSL = NDGSL and (select Count(Number) from AAA_ZBDBXXB where Y_YSYDDBH =a.Number)>1 then '是' else '否'  end  as '是否拆单', (case c.Z_HTQX  when '即时' then '---' else b.SFJRLJQ end ) as '是否在冷静期',c.Z_HTZT AS '中标定标信息表状态'  from AAA_ZBDBXXB as c left join AAA_LJQDQZTXXB as b on c.Z_SPBH=b.SPBH and c.Z_HTQX = b.HTQX left join AAA_YDDXXB as a on a.Number = c.Y_YSYDDBH  ) ";
         //this.hidwhereis.Value = " select * from  (" + StrGettbdsql + " union " + StrGettbdchaifen + "  union " + Stryddzhengchang + " union " + Stryddchaidan + ") as tab  ";
        //Hashtable ht_where = new Hashtable();
        //ht_where["page_size"] = " 15 "; //必须设置,每页的数据量。必须是数字。不能是0。     
        //ht_where["serach_Row_str"] = " * ";
        //ht_where["search_tbname"] = " (" + StrGettbdsql + " union " + StrGettbdchaifen + "  union " + Stryddzhengchang + " union " + Stryddchaidan + ") as tab  ";  //检索的表
        //ht_where["search_mainid"] = "tab.Number";  //所检索表的主键
        //ht_where["search_str_where"] = " 1=1 ";  //检索条件  
        //ht_where["search_paixu"] = " DESC ";  //排序方式
        //ht_where["search_paixuZD"] = "tab.下单时间";  //用于排序的字段
        #endregion

        /*---shiyan 2013-12-25 进行数据获取优化。---*/       
        Hashtable ht_where = new Hashtable();
        ht_where["GetCustomersDataPage_NAME"] = "GetCustomersDataPage";//这个可以不设置,默认是  
        ht_where["page_size"] = " 10 "; //必须设置,每页的数据量。必须是数字。不能是0。     
        ht_where["serach_Row_str"] = " tab.*,b.I_JYFMC  as 交易方名称,b.I_PTGLJG as 业务管理部门,b.I_YWGLBMFL as 业务管理部门分类,'' as 当前状态,'--' as 当前卖家最低价,'--' as 最低价标的经济批量,'--' as 最低价标的达成率 ";
        ht_where["search_tbname"] = " AAA_View_SPMMGKinfo as tab  left join AAA_DLZHXXB as b on tab.交易方账号=b.B_DLYX ";  //检索的表
        ht_where["search_mainid"] = " tab.编号 ";  //所检索表的主键
        ht_where["search_str_where"] = " 1=1 ";  //检索条件  
        ht_where["search_paixu"] = " DESC ";  //排序方式
        ht_where["search_paixuZD"] = " tab.下单时间 ";  //用于排序的字段
        return ht_where;
    }
    public void DisGrid()
    {       
        Hashtable HTwhere = SetV();
        if (!txt_spmc.Text.Trim().Equals(""))
        {
            HTwhere["search_str_where"] += " and 商品名称 like '%" + txt_spmc.Text.Trim() + "%' ";
        }
        if (!ddl_djlb.SelectedItem.ToString().Trim().Equals("选择单据类别"))
        {
            HTwhere["search_str_where"] += " and 单据类型='" + ddl_djlb.SelectedItem.ToString().Trim() + "' ";
        }
        //if (this.UCFWJGDetail1 != null && this.UCFWJGDetail1.Value[0].ToString().Trim() != "" && this.UCFWJGDetail1.Value[1].ToString().Trim() != "")
        //{
        //    HTwhere["search_str_where"] += "  and 业务管理部门 = '" + this.UCFWJGDetail1.Value[1].ToString().Trim() + "'";
        //}

        if (this.UCFWJGDetail1 != null)
        {
            if (this.UCFWJGDetail1.Value[0].ToString().Trim() != "")
            {
                HTwhere["search_str_where"] += "  and  b.I_YWGLBMFL ='" + this.UCFWJGDetail1.Value[0].ToString() + "'";
            }
            if (this.UCFWJGDetail1.Value[1].ToString().Trim() != "")
            {
                HTwhere["search_str_where"] += "  and b.I_PTGLJG = '" + this.UCFWJGDetail1.Value[1].ToString().Trim() + "' ";
            }
        }

        if (txtjjrbh.Text.Trim() != "" && txtjjrbh.Text.Trim() != "无")
        {
            HTwhere["search_str_where"] += " and b.I_JYFMC like '%" + txtjjrbh.Text.Trim() + "%'";
        }

        if (txtjjrxm.Text.Trim() != "" && txtjjrxm.Text.Trim() != "无")
        {
            HTwhere["search_str_where"] += " and 交易方账号 like '%" + txtjjrxm.Text.Trim() + "%'";
        }

        //if (txtjjrbh.Text.Trim() != "" && txtjjrbh.Text.Trim() != "无")
        //{
        //    HTwhere["search_str_where"] += " and 交易方账号 like '%" + txtjjrbh.Text.Trim() + "%'";
        //}

        //if (txtjjrxm.Text.Trim() != "" && txtjjrxm.Text.Trim() != "无")
        //{
        //    HTwhere["search_str_where"] += " and 交易方名称 like '%" + txtjjrxm.Text.Trim() + "%'";
        //}
        // HTwhere["search_str_where"] += "  and 所属分公司 = '" + this.ddlssfgs.SelectedValue.ToString() + "' and 经纪人编号 like '%" + txtjjrbh.Text.Trim() + "%' and 经纪人名称 like '%" + txtjjrxm.Text.Trim() + "%'";
        this.hidwhere.Value= HTwhere["search_str_where"].ToString();
      //  hidwhereis.Value = HTwhere["search_str_where"].ToString();
        commonpagernew1.HTwhere = HTwhere;
        commonpagernew1.GetFYdataAndRaiseEvent();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        DisGrid();
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        if (this.rptSPXX != null && this.rptSPXX.Items.Count > 0)
        {
            //StringBuilder stringBuilder = new StringBuilder();
            ////stringBuilder.Append(" select '" + this.hidID.Value + "' as '收益时间',*  from ");
            //stringBuilder.Append(this.hidwhereis.Value + " where " + this.hidwhere.Value.ToString() + " order by 下单时间 desc ");
            //// stringBuilder.Append(" where " + hidwhereis.Value.ToString());
            //DataSet dataSet = DbHelperSQL.Query(stringBuilder.ToString());
            //dataSet.Tables[0].Columns.Remove("Number");
            //dataSet.Tables[0].Columns.Remove("是否在冷静期");
            //dataSet.Tables[0].Columns.Remove("中标定标信息表状态");

            string sql = "select b.I_PTGLJG as 业务管理部门,tab.交易方账号,b.I_JYFMC  as 交易方名称,tab.下单时间,tab.商品编号,tab.商品名称,tab.规格,tab.合同期限,tab.数量,tab.价格,tab.金额,'---' as 当前卖家最低价,'---' as 最低价标的经济批量,'---' as [最低价标的达成率/中标率%],tab.单据类型,tab.单据编号,'' as 当前状态,tab.是否拆单,tab.单据状态,b.I_YWGLBMFL as 业务管理部门分类 from AAA_View_SPMMGKinfo as tab  left join AAA_DLZHXXB as b on tab.交易方账号=b.B_DLYX where " + hidwhere.Value.ToString() + " order by tab.下单时间 DESC";
            DataSet dataSet = DbHelperSQL.Query(sql);

            #region 其他需要计算获取的字段
            for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
            {
                #region 获取当前最低价、经济批量、达成率
                //获取当前商品的卖家最低价及相关数据
                DataSet ds_lowprice = DbHelperSQL.Query("select top 1 number as 投标单号, TBJG as 投标价格,TBNSL as 最低价投标拟售量,YZBSL as 已中标数量,MJSDJJPL as 经济批量,GHQY as 供货区域  from AAA_TBD where ZT='竞标' and SPBH='" + dataSet.Tables[0].Rows[i]["商品编号"].ToString() + "' and HTQX='" + dataSet.Tables[0].Rows[i]["合同期限"].ToString() + "'  order by TBJG,TJSJ");
                if (ds_lowprice != null && ds_lowprice.Tables[0].Rows.Count > 0)
                {
                    dataSet.Tables[0].Rows[i]["当前卖家最低价"] = ds_lowprice.Tables[0].Rows[0]["投标价格"].ToString();
                    dataSet.Tables[0].Rows[i]["最低价标的经济批量"] = ds_lowprice.Tables[0].Rows[0]["经济批量"].ToString();

                    //即时合同的达成率和三个月/一年的计算公式不同
                    if (dataSet.Tables[0].Rows[i]["合同期限"].ToString() == "即时")
                    {
                        dataSet.Tables[0].Rows[i]["最低价标的达成率/中标率%"] = (Convert.ToDouble(ds_lowprice.Tables[0].Rows[0]["已中标数量"].ToString()) / Convert.ToDouble(ds_lowprice.Tables[0].Rows[0]["最低价投标拟售量"].ToString()) * 100.00).ToString("#0.00");
                    }
                    else
                    {
                        //获取最低价标的拟购买数量和
                        string sql_lowprice = "select isnull(sum(NDGSL-YZBSL),0) from AAA_YDDXXB where SPBH='" + dataSet.Tables[0].Rows[i]["商品编号"].ToString() + "' and  HTQX='" + dataSet.Tables[0].Rows[i]["合同期限"].ToString() + "' and ZT = '竞标' and NMRJG >=convert(decimal(18,2),'" + ds_lowprice.Tables[0].Rows[0]["投标价格"].ToString() + "') and 0<CHARINDEX ('|'+SHQYsheng+'|',isnull('" + ds_lowprice.Tables[0].Rows[0]["供货区域"].ToString() + "',''))";
                        object obj_NDGSL = DbHelperSQL.GetSingle(sql_lowprice);
                        dataSet.Tables[0].Rows[i]["最低价标的达成率/中标率%"] = (Convert.ToDouble(obj_NDGSL.ToString()) / Convert.ToDouble(ds_lowprice.Tables[0].Rows[0]["最低价投标拟售量"].ToString()) * 100.00).ToString("#0.00");
                    }
                }
                #endregion
                #region 确定应该显示的当前状态
                if (dataSet.Tables[0].Rows[i]["单据状态"].ToString() == "竞标")
                {
                    if (dataSet.Tables[0].Rows[i]["合同期限"].ToString() == "即时")
                    {
                        dataSet.Tables[0].Rows[i]["当前状态"] = "竞标中";
                    }
                    else
                    {
                        //三个月/一年的，判断是否进入冷静期
                        object objLJQ = DbHelperSQL.GetSingle("select SFJRLJQ from AAA_LJQDQZTXXB where SPBH='" + dataSet.Tables[0].Rows[i]["商品编号"].ToString() + "' and HTQX='" + dataSet.Tables[0].Rows[i]["合同期限"].ToString() + "'");
                        if (objLJQ != null && objLJQ.ToString() != "")
                        {
                            dataSet.Tables[0].Rows[i]["当前状态"] = objLJQ.ToString() == "是" ? "竞标中（冷静期）" : "竞标中";
                        }
                    }
                }
                else
                {
                    //从中标定标信息表中获取的数据的状态对应
                    switch (dataSet.Tables[0].Rows[i]["单据状态"].ToString())
                    {
                        case "定标合同到期":
                            dataSet.Tables[0].Rows[i]["当前状态"] = "定标";
                            break;
                        case "定标执行完成":
                            dataSet.Tables[0].Rows[i]["当前状态"] = "定标";
                            break;
                        case "未定标废标":
                            dataSet.Tables[0].Rows[i]["当前状态"] = "废标";
                            break;
                        case "定标合同终止":
                            dataSet.Tables[0].Rows[i]["当前状态"] = "废标";
                            break;
                        default:
                            dataSet.Tables[0].Rows[i]["当前状态"] = dataSet.Tables[0].Rows[i]["单据状态"].ToString();
                            break;
                    }
                    //获取是否拆单
                    string sql_cd = "";
                    if (dataSet.Tables[0].Rows[i]["单据类型"].ToString () == "预订单")
                    {
                        sql_cd = "select SFCD from AAA_YDDXXB where Number='" + dataSet.Tables[0].Rows[i]["单据编号"].ToString() + "'";
                    }
                    else
                    {
                        sql_cd = "select SFCD from AAA_TBD where Number='" + dataSet.Tables[0].Rows[i]["单据编号"].ToString() + "'";
                    }
                    object obj_SFCD = DbHelperSQL.GetSingle(sql_cd);
                    if (obj_SFCD != null && obj_SFCD.ToString() != "")
                    {
                        dataSet.Tables[0].Rows[i]["是否拆单"] = obj_SFCD.ToString();
                    }

                }
                #endregion               
            }
            #endregion
            dataSet.Tables[0].Columns.Remove("业务管理部门分类");
            dataSet.Tables[0].Columns.Remove("单据状态");
            MyXlsClass MXC = new MyXlsClass();

            MXC.goxls(dataSet, "Report_" + DateTime.Now.ToString("yyyy-MM-dd:hh-mm-sss") + ".xls", "商品买卖概况总表", "商品买卖概况总表", 15);
        }
        else
        {
            MessageBox.ShowAlertAndBack(this, "列表中没有数据可导出！");
        }

    }


}