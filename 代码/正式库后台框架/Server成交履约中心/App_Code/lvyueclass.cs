using FMDBHelperClass;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// lvyueclass 的摘要说明
/// </summary>
public class lvyueclass
{
	public lvyueclass()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}

    /// <summary>
    /// 得到电子购货合同相关的数据信息 wyh 2014.06.04
    /// </summary>
    /// <param name="dataTable">参数集合</param>
    /// <returns>返回符合dsreturn结构的Dataset</returns>
    public DataSet GetDZGHHTXX( DataTable dataTable)
    {
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化", "" });

        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        DataSet dsGXlist = new DataSet();
        Hashtable input = new Hashtable();

        string strSql = "";
        string strNumber = dataTable.Rows[0]["中标定标_Number"].ToString();//中标定标信息表
        input["@Number"] = strNumber;

        strSql = "select Seller.I_ZCLB '注册类别',Seller.I_YYZZSMJ '营业执照扫描件',Seller.I_SFZSMJ '身份证扫描件',Seller.I_SFZFMSMJ '身份证反面扫描件',Seller.I_ZZJGDMZSMJ '组织机构代码证扫描件',Seller.I_SWDJZSMJ '税务登记证扫描件',Seller.I_YBNSRZGZSMJ '一般纳税人资格证明扫描件',Seller.I_KHXKZSMJ '开户许可证扫描件',Seller.I_YLYJK '预留印鉴卡扫描件',Seller.I_FDDBRSFZSMJ '法定代表人身份证扫描件',Seller.I_FDDBRSFZFMSMJ '法定代表人身份证反面扫描件',Seller.I_FDDBRSQS '法定代表人授权书' from AAA_ZBDBXXB as ZZ left join AAA_DLZHXXB as Seller on ZZ.T_YSTBDMJJSBH=Seller.J_SELJSBH where ZZ.Number=@Number";
        Hashtable return_ht = I_DBL.RunParam_SQL(strSql, "htinfo", input);
        if ((bool)(return_ht["return_float"])) //说明执行完成
        {
            //这里就可以用了。
            dsGXlist = (DataSet)(return_ht["return_ds"]);
            if (dsGXlist != null && dsGXlist.Tables["htinfo"].Rows.Count > 0)
            {
                DataTable dt = dsGXlist.Tables["htinfo"].Copy();
                dt.TableName = "卖家资质";
                dsreturn.Tables.Add(dt);   
            }
            

        }
        else
        {
            return null;

        } //?

        strSql = "select Buyer.I_ZCLB '注册类别',Buyer.I_YYZZSMJ '营业执照扫描件',Buyer.I_SFZSMJ '身份证扫描件',Buyer.I_SFZFMSMJ '身份证反面扫描件',Buyer.I_ZZJGDMZSMJ '组织机构代码证扫描件',Buyer.I_SWDJZSMJ '税务登记证扫描件',Buyer.I_YBNSRZGZSMJ '一般纳税人资格证明扫描件',Buyer.I_KHXKZSMJ '开户许可证扫描件',Buyer.I_YLYJK '预留印鉴卡扫描件',Buyer.I_FDDBRSFZSMJ '法定代表人身份证扫描件',Buyer.I_FDDBRSFZFMSMJ '法定代表人身份证反面扫描件' ,Buyer.I_FDDBRSQS '法定代表人授权书' from AAA_ZBDBXXB as ZZ left join AAA_DLZHXXB as Buyer on ZZ.Y_YSYDDMJJSBH=Buyer.J_BUYJSBH where ZZ.Number=@Number";
        return_ht = I_DBL.RunParam_SQL(strSql, "mjinfo", input);
        if ((bool)(return_ht["return_float"])) //说明执行完成
        {
            //这里就可以用了。
            dsGXlist = (DataSet)(return_ht["return_ds"]);
            if (dsGXlist != null && dsGXlist.Tables["mjinfo"].Rows.Count > 0)
            {
                DataTable dt = dsGXlist.Tables["mjinfo"].Copy();
                dt.TableName = "买家资质";
                dsreturn.Tables.Add(dt);
            }
           
        }
        else
        {
            return null;

        } //?


        strSql = "select  DD.I_JYFMC '卖家名称',TT.TJSJ '发布时间',TT.Number '投标单号',TT.GHQY '可供货区域',TT.SPMC '拟卖出商品名称',TT.GG '规格',TT.JJDW '计价单位',TT.TBNSL '投标拟售量',TT.TBJG '投标价格',TT.TBJE '投标金额',TT.DJTBBZJ '冻结的投标保证金',TT.PTSDZDJJPL '平台设定最大经济批量',TT.MJSDJJPL '卖家设定的经济批量',TT.HTQX '合同期限' from AAA_ZBDBXXB as ZZ left join AAA_TBD as TT on ZZ.T_YSTBDBH=TT.Number left join AAA_DLZHXXB as DD on TT.MJJSBH=DD.J_SELJSBH where ZZ.Number=@Number";

        return_ht = I_DBL.RunParam_SQL(strSql, "TBDinfo", input);
        if ((bool)(return_ht["return_float"])) //说明执行完成
        {
            //这里就可以用了。
            dsGXlist = (DataSet)(return_ht["return_ds"]);
            if (dsGXlist != null && dsGXlist.Tables["TBDinfo"].Rows.Count > 0)
            {
                DataTable dt = dsGXlist.Tables["TBDinfo"].Copy();
                dt.TableName = "投标单";
                dsreturn.Tables.Add(dt);
            }
            

        }
        else
        {
            return null;

        } //?


        strSql = "select  DD.I_JYFMC '买家名称',YY.TJSJ '下单时间',YY.Number '预订单号',YY.SHQY '收货区域',YY.SPMC '拟买入商品名称',YY.GG '规格',YY.JJDW '计价单位',YY.NDGSL '拟订购数量',YY.NMRJG '拟买入价格',YY.NDGJE '拟订购金额',YY.DJDJ '冻结的订金',YY.HTQX '合同期限' from AAA_ZBDBXXB as ZZ left join AAA_YDDXXB as YY on ZZ.Y_YSYDDBH=YY.Number left join AAA_DLZHXXB as DD on YY.MJJSBH=DD.J_BUYJSBH where ZZ.Number=@Number";

        return_ht = I_DBL.RunParam_SQL(strSql, "预订单", input);
        if ((bool)(return_ht["return_float"])) //说明执行完成
        {
            //这里就可以用了。
            dsGXlist = (DataSet)(return_ht["return_ds"]);
            if (dsGXlist != null && dsGXlist.Tables["预订单"].Rows.Count > 0)
            {
                DataTable dt = dsGXlist.Tables["预订单"].Copy();
                dt.TableName = "预订单";
                dsreturn.Tables.Add(dt);
            }
            

        }
        else
        {
            return null;

        } //?



        strSql = "select  TT.ZLBZYZM '质量标准与证明',TT.CPJCBG '产品检测报告',TT.PGZFZRFLCNS '品管总负责人法律承诺书',TT.FDDBRCNS '法定代表人承诺书',TT.SHFWGDYCN '售后服务规定与承诺',TT.CPSJSQS '产品送检授权书',TT.SLZM '税率证明',TT.ZZ01 '资质01',TT.ZZ02 '资质02',TT.ZZ03 '资质03',TT.ZZ04 '资质04',TT.ZZ05 '资质05',TT.ZZ06 '资质06',TT.ZZ07 '资质07',TT.ZZ08 '资质08',TT.ZZ09 '资质09',TT.ZZ10 '资质10',SP.SCZZYQ as '上传资质要求' from AAA_ZBDBXXB as ZZ inner join AAA_TBD as TT on ZZ.T_YSTBDBH=TT.Number inner join AAA_PTSPXXB as SP on TT.SPBH=SP.SPBH where ZZ.Number=@Number";

        return_ht = I_DBL.RunParam_SQL(strSql, "投标单资质", input);
        if ((bool)(return_ht["return_float"])) //说明执行完成
        {
            //这里就可以用了。
            dsGXlist = (DataSet)(return_ht["return_ds"]);
            if (dsGXlist != null && dsGXlist.Tables["投标单资质"].Rows.Count > 0)
            {
                DataTable dt = dsGXlist.Tables["投标单资质"].Copy();
                dt.TableName = "投标单资质";
                dsreturn.Tables.Add(dt);
            }
            

        }
        else
        {
            return null;

        } //?


        strSql = "select (select isnull(sum(T_THSL),0) from AAA_THDYFHDXXB where ZBDBXXBBH=ZZ.Number  and F_DQZT<>'撤销') '已提货数量', (select isnull(sum(T_THSL*ZBDJ),0.00)  from AAA_THDYFHDXXB where ZBDBXXBBH=ZZ.Number  and F_DQZT<>'撤销') '已提货金额',(select isnull(sum(T_THSL),0) from AAA_THDYFHDXXB where ZBDBXXBBH=ZZ.Number  and F_DQZT not in('撤销','未生成发货单','已生成发货单')) '已发货数量',(select isnull(sum(T_THSL),0) from AAA_THDYFHDXXB where ZBDBXXBBH=ZZ.Number  and F_DQZT  in('无异议收货','默认无异议收货','补发货物无异议收货','有异议收货后无异议收货')) '无异议收货数量', 'T'+TT.Number '提货单编号',TT.T_THSL '提货数量',isnull(TT.T_THSL*TT.ZBDJ,0.00) '提货金额',TT.F_DQZT '当前状态','F'+TT.Number '发货单编号' from  AAA_THDYFHDXXB as TT left join AAA_ZBDBXXB as ZZ   on ZZ.Number=TT.ZBDBXXBBH where ZZ.Number=@Number";
        return_ht = I_DBL.RunParam_SQL(strSql, "提货单发货单", input);
        if ((bool)(return_ht["return_float"])) //说明执行完成
        {
            //这里就可以用了。
            dsGXlist = (DataSet)(return_ht["return_ds"]);
            if (dsGXlist != null && dsGXlist.Tables["提货单发货单"].Rows.Count > 0)
            {
                DataTable dt = dsGXlist.Tables["提货单发货单"].Copy();
                dt.TableName = "提货单发货单";
                dsreturn.Tables.Add(dt);
            }
            

        }
        else
        {
            return null;

        } //?

        strSql = "select Number '保函编号' ,Q_ZMWJLJ '履约争议证明文件路径' from AAA_ZBDBXXB where Number=@Number";
        //以后处理限制条件，加载这个地方的SQL语句里
        return_ht = I_DBL.RunParam_SQL(strSql, "其他资质", input);
        if ((bool)(return_ht["return_float"])) //说明执行完成
        {
            //这里就可以用了。
            dsGXlist = (DataSet)(return_ht["return_ds"]);
            if (dsGXlist != null && dsGXlist.Tables["其他资质"].Rows.Count > 0)
            {
                DataTable dt = dsGXlist.Tables["其他资质"].Copy();
                dt.TableName = "其他资质";
                dsreturn.Tables.Add(dt);
            }
            

        }
        else
        {
            return null;

        } //?
        
        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取数据成功！";

        return dsreturn;
    }






    /// <summary>
    /// 得到电子购货合同基本信息---用于替换合同模板部分内容
    /// </summary>
    /// <param name="Number">中标定标信息表Number值</param>
    /// <returns>返回符合dsreturn结构Dataset，包含表明为“合同基本资料”的Datatable</returns>
    public DataSet GetHT_jiben(string Number)
    {
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化", "" });

        if (Number.Equals(""))
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "传递的参数不可为空！";
            return dsreturn;
        }

        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        DataSet dsGXlist = new DataSet();
        Hashtable input = new Hashtable();

        string strSql = "";
        string strNumber = Number;//dataTable.Rows[0]["中标定标_Number"].ToString();//中标定标信息表
        input["@Number"] = strNumber;

        strSql = "select  ZZ.Z_HTBH '合同编号','BH'+ZZ.Number '保证函编号',DL_Seller.I_JYFMC '卖家交易方名称',DL_Buyer.I_JYFMC '买家交易方名称',isnull(DL_Seller.B_ZHDQXYFZ,0) '卖家信用分值',isnull(DL_Buyer.B_ZHDQXYFZ,0) '买家信用分值',DL_Seller.B_DLYX '卖家交易方账户',DL_Buyer.B_DLYX '买家交易方账户',DL_Seller.I_YYZZZCH '卖家营业执照注册号',DL_Buyer.I_YYZZZCH '买家营业执照注册号',DL_Buyer.I_SFZH '买家身份账号',DL_Buyer.I_ZCLB '买家注册类别',DL_Seller.I_XXDZ '卖家联系地址',DL_Buyer.I_XXDZ '买家联系地址',DL_Seller.I_LXRXM '卖家联系人',DL_Buyer.I_LXRXM '买家联系人',DL_Seller.I_JYFLXDH '卖家联系电话',DL_Buyer.I_JYFLXDH '买家联系电话',CONVERT(varchar(10),Z_ZBSJ,120) '合同签订时间',ZZ.Z_SPBH '商品编号',ZZ.Z_SPMC '商品名称',ZZ.Z_GG '规格',ZZ.Z_ZBJG '价格',ZZ.Z_ZBSL '数量',ZZ.Z_JJDW '计价单位',ZZ.Z_ZBJE '金额',ZZ.Z_LYBZJJE '履约保证金',ZZ.Z_GG '商品标准',convert(varchar(10),ZZ.Z_DBSJ,120) '合同开始时间',convert(varchar(10),ZZ.Z_HTJSRQ,120) '合同结束时间',ZZ.Z_HTQX '合同期限' from AAA_ZBDBXXB as ZZ left join AAA_DLZHXXB as DL_Seller on ZZ.T_YSTBDMJJSBH=DL_Seller.J_SELJSBH left join AAA_DLZHXXB as DL_Buyer on ZZ.Y_YSYDDMJJSBH=DL_Buyer.J_BUYJSBH left join AAA_PTSPXXB as SP on ZZ.Z_SPBH=SP.SPBH   where ZZ.Number=@Number";
        Hashtable return_ht = I_DBL.RunParam_SQL(strSql, "htinfo", input);
        if ((bool)(return_ht["return_float"])) //说明执行完成
        {
            //这里就可以用了。
            dsGXlist = (DataSet)(return_ht["return_ds"]);
            if (dsGXlist != null && dsGXlist.Tables.Count > 0)
            {
                DataTable dt = dsGXlist.Tables["htinfo"].Copy();
                dt.TableName = "合同基本资料";
                dsreturn.Tables.Add(dt);
            }
            else
            {
                return null;
            }

        }
        else
        {
            return null;

        } //?
        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取数据成功！";

        return dsreturn;
    }





    /// <summary>
    /// 得到电子购货合同卖家资质相关信息---卖家资质 wyh 2014.06.04
    /// </summary>
    /// <param name="Number">中标定标信息表Number值</param>
    /// <returns>返回符合dsreturn结构Dataset，同时包含表明为“卖家资质”的Datatable</returns>
    public DataSet GetDZGHHTXX_Sellzz( string Number)
    {  
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化", "" });

        if (Number.Equals(""))
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "传递的参数不可为空！";
            return dsreturn;
        }

        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        DataSet dsGXlist = new DataSet();
        Hashtable input = new Hashtable();

        string strSql = "";
        string strNumber = Number;//dataTable.Rows[0]["中标定标_Number"].ToString();//中标定标信息表
        input["@Number"] = strNumber;

        strSql = "select Seller.I_ZCLB '注册类别',Seller.I_YYZZSMJ '营业执照扫描件',Seller.I_SFZSMJ '身份证扫描件',Seller.I_SFZFMSMJ '身份证反面扫描件',Seller.I_ZZJGDMZSMJ '组织机构代码证扫描件',Seller.I_SWDJZSMJ '税务登记证扫描件',Seller.I_YBNSRZGZSMJ '一般纳税人资格证明扫描件',Seller.I_KHXKZSMJ '开户许可证扫描件',Seller.I_YLYJK '预留印鉴卡扫描件',Seller.I_FDDBRSFZSMJ '法定代表人身份证扫描件',Seller.I_FDDBRSFZFMSMJ '法定代表人身份证反面扫描件',Seller.I_FDDBRSQS '法定代表人授权书' from AAA_ZBDBXXB as ZZ left join AAA_DLZHXXB as Seller on ZZ.T_YSTBDMJJSBH=Seller.J_SELJSBH where ZZ.Number=@Number";
        Hashtable return_ht = I_DBL.RunParam_SQL(strSql, "htinfo", input);
        if ((bool)(return_ht["return_float"])) //说明执行完成
        {
            //这里就可以用了。
            dsGXlist = (DataSet)(return_ht["return_ds"]);
            if (dsGXlist != null && dsGXlist.Tables["htinfo"].Rows.Count > 0)
            {
                DataTable dt = dsGXlist.Tables["htinfo"].Copy();
                dt.TableName = "卖家资质";
                dsreturn.Tables.Add(dt);
            }
            else
            {
                return null;
            }

        }
        else
        {
            return null;

        } //?
        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取数据成功！";

        return dsreturn;
    }

    /// <summary>
    /// 得到电子购货合同卖家资质相关信息---买家资质 wyh 2014.06.04
    /// </summary>
    /// <param name="Number">中标定标信息表Number值</param>
    /// <returns>返回符合dsreturn结构Dataset，同时包含表明为“买家资质”的Datatable</returns>
    public DataSet GetDZGHHTXX_buyerzz(string Number)
    {
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化", "" });

        if (Number.Equals(""))
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "传递的参数不可为空！";
            return dsreturn;
        }

        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        DataSet dsGXlist = new DataSet();
        Hashtable input = new Hashtable();
        Hashtable return_ht = new Hashtable();
        string strSql = "";
        string strNumber = Number;//dataTable.Rows[0]["中标定标_Number"].ToString();//中标定标信息表
        input["@Number"] = strNumber;
        strSql = "select Buyer.I_ZCLB '注册类别',Buyer.I_YYZZSMJ '营业执照扫描件',Buyer.I_SFZSMJ '身份证扫描件',Buyer.I_SFZFMSMJ '身份证反面扫描件',Buyer.I_ZZJGDMZSMJ '组织机构代码证扫描件',Buyer.I_SWDJZSMJ '税务登记证扫描件',Buyer.I_YBNSRZGZSMJ '一般纳税人资格证明扫描件',Buyer.I_KHXKZSMJ '开户许可证扫描件',Buyer.I_YLYJK '预留印鉴卡扫描件',Buyer.I_FDDBRSFZSMJ '法定代表人身份证扫描件',Buyer.I_FDDBRSFZFMSMJ '法定代表人身份证反面扫描件' ,Buyer.I_FDDBRSQS '法定代表人授权书' from AAA_ZBDBXXB as ZZ left join AAA_DLZHXXB as Buyer on ZZ.Y_YSYDDMJJSBH=Buyer.J_BUYJSBH where ZZ.Number=@Number";
        return_ht = I_DBL.RunParam_SQL(strSql, "mjinfo", input);
        if ((bool)(return_ht["return_float"])) //说明执行完成
        {
            //这里就可以用了。
            dsGXlist = (DataSet)(return_ht["return_ds"]);
            if (dsGXlist != null && dsGXlist.Tables["mjinfo"].Rows.Count > 0)
            {
                DataTable dt = dsGXlist.Tables["mjinfo"].Copy();
                dt.TableName = "买家资质";
                dsreturn.Tables.Add(dt);
            }
            else
            {
                return null;
            }

        }
        else
        {
            return null;

        } //?


        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取数据成功！";

        return dsreturn;


    }


    /// <summary>
    /// 得到电子购货合同卖家资质相关信息---投标单 wyh 2014.06.04
    /// </summary>
    /// <param name="Number">中标定标信息表Number值</param>
    /// <returns>返回符合dsreturn结构Dataset，同时包含表明为“投标单”的Datatable</returns>
    public DataSet GetDZGHHTXX_TBD(string Number)
    {

        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化", "" });
        if (Number.Equals(""))
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "传递的参数不可为空！";
            return dsreturn;
        }
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        DataSet dsGXlist = new DataSet();
        Hashtable input = new Hashtable();
        Hashtable return_ht = new Hashtable();
        string strSql = "";
        string strNumber = Number;//中标定标信息表
        input["@Number"] = strNumber;

        strSql = "select  DD.I_JYFMC '卖家名称',TT.TJSJ '发布时间',TT.Number '投标单号',TT.GHQY '可供货区域',TT.SPMC '拟卖出商品名称',TT.GG '规格',TT.JJDW '计价单位',TT.TBNSL '投标拟售量',TT.TBJG '投标价格',TT.TBJE '投标金额',TT.DJTBBZJ '冻结的投标保证金',TT.PTSDZDJJPL '平台设定最大经济批量',TT.MJSDJJPL '卖家设定的经济批量',TT.HTQX '合同期限' from AAA_ZBDBXXB as ZZ left join AAA_TBD as TT on ZZ.T_YSTBDBH=TT.Number left join AAA_DLZHXXB as DD on TT.MJJSBH=DD.J_SELJSBH where ZZ.Number=@Number";

        return_ht = I_DBL.RunParam_SQL(strSql, "TBDinfo", input);
        if ((bool)(return_ht["return_float"])) //说明执行完成
        {
            //这里就可以用了。
            dsGXlist = (DataSet)(return_ht["return_ds"]);
            if (dsGXlist != null && dsGXlist.Tables["TBDinfo"].Rows.Count > 0)
            {
                DataTable dt = dsGXlist.Tables["TBDinfo"].Copy();
                dt.TableName = "投标单";
                dsreturn.Tables.Add(dt);
            }
            else
            {
                return null;
            }

        }
        else
        {
            return null;

        } //?


        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取数据成功！";

        return dsreturn;
    }

    /// <summary>
    /// 得到电子购货合同卖家资质相关信息---预订单 wyh 2014.06.04
    /// </summary>
    /// <param name="Number">中标定标信息表Number值</param>
    /// <returns>返回符合dsreturn结构Dataset，同时包含表明为“预订单”的Datatable</returns>
    public DataSet GetDZGHHTXX_YDD(string Number)
    {
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化", "" });
        if (Number.Equals(""))
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "传递的参数不可为空！";
            return dsreturn;
        }
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        DataSet dsGXlist = new DataSet();
        Hashtable input = new Hashtable();
        Hashtable return_ht = new Hashtable();
        string strSql = "";
        string strNumber = Number;//中标定标信息表
        input["@Number"] = strNumber;
        strSql = "select  DD.I_JYFMC '买家名称',YY.TJSJ '下单时间',YY.Number '预订单号',YY.SHQY '收货区域',YY.SPMC '拟买入商品名称',YY.GG '规格',YY.JJDW '计价单位',YY.NDGSL '拟订购数量',YY.NMRJG '拟买入价格',YY.NDGJE '拟订购金额',YY.DJDJ '冻结的订金',YY.HTQX '合同期限' from AAA_ZBDBXXB as ZZ left join AAA_YDDXXB as YY on ZZ.Y_YSYDDBH=YY.Number left join AAA_DLZHXXB as DD on YY.MJJSBH=DD.J_BUYJSBH where ZZ.Number=@Number";

        return_ht = I_DBL.RunParam_SQL(strSql, "预订单", input);
        if ((bool)(return_ht["return_float"])) //说明执行完成
        {
            //这里就可以用了。
            dsGXlist = (DataSet)(return_ht["return_ds"]);
            if (dsGXlist != null && dsGXlist.Tables["预订单"].Rows.Count > 0)
            {
                DataTable dt = dsGXlist.Tables["预订单"].Copy();
                dt.TableName = "预订单";
                dsreturn.Tables.Add(dt);
            }
            else
            {
                return null;
            }

        }
        else
        {
            return null;

        } //?

        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取数据成功！";

        return dsreturn;


    }
    //投标单资质

    /// <summary>
    /// 得到电子购货合同卖家资质相关信息---投标单资质 wyh 2014.06.04
    /// </summary>
    /// <param name="Number">中标定标信息表Number值</param>
    /// <returns>返回符合dsreturn结构Dataset，同时包含表明为“投标单资质”的Datatable</returns>
    public DataSet GetDZGHHTXX_TBDZZ(string Number)
    {
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化", "" });
        if (Number.Equals(""))
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "传递的参数不可为空！";
            return dsreturn;
        }
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        DataSet dsGXlist = new DataSet();
        Hashtable input = new Hashtable();
        Hashtable return_ht = new Hashtable();
        input["@Number"] = Number;
       string  strSql = "select  TT.ZLBZYZM '质量标准与证明',TT.CPJCBG '产品检测报告',TT.PGZFZRFLCNS '品管总负责人法律承诺书',TT.FDDBRCNS '法定代表人承诺书',TT.SHFWGDYCN '售后服务规定与承诺',TT.CPSJSQS '产品送检授权书',TT.SLZM '税率证明',TT.ZZ01 '资质01',TT.ZZ02 '资质02',TT.ZZ03 '资质03',TT.ZZ04 '资质04',TT.ZZ05 '资质05',TT.ZZ06 '资质06',TT.ZZ07 '资质07',TT.ZZ08 '资质08',TT.ZZ09 '资质09',TT.ZZ10 '资质10',SP.SCZZYQ as '上传资质要求' from AAA_ZBDBXXB as ZZ inner join AAA_TBD as TT on ZZ.T_YSTBDBH=TT.Number inner join AAA_PTSPXXB as SP on TT.SPBH=SP.SPBH where ZZ.Number=@Number";

        return_ht = I_DBL.RunParam_SQL(strSql, "投标单资质", input);
        if ((bool)(return_ht["return_float"])) //说明执行完成
        {
            //这里就可以用了。
            dsGXlist = (DataSet)(return_ht["return_ds"]);
            if (dsGXlist != null && dsGXlist.Tables["投标单资质"].Rows.Count > 0)
            {
                DataTable dt = dsGXlist.Tables["投标单资质"].Copy();
                dt.TableName = "投标单资质";
                dsreturn.Tables.Add(dt);
            }
            else
            {
                return null;
            }

        }
        else
        {
            return null;

        } //?
        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取数据成功！";

        return dsreturn;
    }


    /// <summary>
    /// 得到电子购货合同卖家资质相关信息---提货单发货单 wyh 2014.06.04
    /// </summary>
    /// <param name="Number">中标定标信息表Number值</param>
    /// <returns>返回符合dsreturn结构Dataset，同时包含表明为“提货单发货单”的Datatable</returns>
    public DataSet GetDZGHHTXX_THDFHD(string Number)
    {
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化", "" });

        if (Number.Equals(""))
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "传递的参数不可为空！";
            return dsreturn;
        }

        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        DataSet dsGXlist = new DataSet();
        Hashtable input = new Hashtable();
        Hashtable return_ht = new Hashtable();
        input["@Number"] = Number;
      string  strSql = "select (select isnull(sum(T_THSL),0) from AAA_THDYFHDXXB where ZBDBXXBBH=ZZ.Number  and F_DQZT<>'撤销') '已提货数量', (select isnull(sum(T_THSL*ZBDJ),0.00)  from AAA_THDYFHDXXB where ZBDBXXBBH=ZZ.Number  and F_DQZT<>'撤销') '已提货金额',(select isnull(sum(T_THSL),0) from AAA_THDYFHDXXB where ZBDBXXBBH=ZZ.Number  and F_DQZT not in('撤销','未生成发货单','已生成发货单')) '已发货数量',(select isnull(sum(T_THSL),0) from AAA_THDYFHDXXB where ZBDBXXBBH=ZZ.Number  and F_DQZT  in('无异议收货','默认无异议收货','补发货物无异议收货','有异议收货后无异议收货')) '无异议收货数量', 'T'+TT.Number '提货单编号',TT.T_THSL '提货数量',isnull(TT.T_THSL*TT.ZBDJ,0.00) '提货金额',TT.F_DQZT '当前状态','F'+TT.Number '发货单编号' from  AAA_THDYFHDXXB as TT left join AAA_ZBDBXXB as ZZ   on ZZ.Number=TT.ZBDBXXBBH where ZZ.Number=@Number";
        return_ht = I_DBL.RunParam_SQL(strSql, "提货单发货单", input);
        if ((bool)(return_ht["return_float"])) //说明执行完成
        {
            //这里就可以用了。
            dsGXlist = (DataSet)(return_ht["return_ds"]);
            if (dsGXlist != null && dsGXlist.Tables["提货单发货单"].Rows.Count > 0)
            {
                DataTable dt = dsGXlist.Tables["提货单发货单"].Copy();
                dt.TableName = "提货单发货单";
                dsreturn.Tables.Add(dt);
            }
            else
            {
                return null;
            }

        }
        else
        {
            return null;

        } //?

        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取数据成功！";

        return dsreturn;

    }

    /// <summary>
    /// 得到电子购货合同卖家资质相关信息---其他资质 wyh 2014.06.04
    /// </summary>
    /// <param name="Number">中标定标信息表Number值</param>
    /// <returns>返回符合dsreturn结构Dataset，同时包含表明为“其他资质”的Datatable</returns>
    public DataSet GetDZGHHTXX_QTZZ(string Number)
    {
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化", "" });

        if (Number.Equals(""))
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "传递的参数不可为空！";
            return dsreturn;
        }

        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        DataSet dsGXlist = new DataSet();
        Hashtable input = new Hashtable();
        Hashtable return_ht = new Hashtable();
        input["@Number"] = Number;
        string  strSql = "select Number '保函编号' ,Q_ZMWJLJ '履约争议证明文件路径' from AAA_ZBDBXXB where Number=@Number";
        //以后处理限制条件，加载这个地方的SQL语句里
        return_ht = I_DBL.RunParam_SQL(strSql, "其他资质", input);
        if ((bool)(return_ht["return_float"])) //说明执行完成
        {
            //这里就可以用了。
            dsGXlist = (DataSet)(return_ht["return_ds"]);
            if (dsGXlist != null && dsGXlist.Tables["其他资质"].Rows.Count > 0)
            {
                DataTable dt = dsGXlist.Tables["其他资质"].Copy();
                dt.TableName = "其他资质";
                dsreturn.Tables.Add(dt);
            }
            else
            {
                return null;
            }

        }
        else
        {
            return null;

        } //?

        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取数据成功！";

        return dsreturn;

    }

    /// <summary>
    /// 验证投标资料审核表对应的投标点信息
    /// </summary>
    /// <param name="Number">AAA_THDYFHDXXB表Number值</param>
    /// <returns>返回Dataset包含对应的表AAA_TBZLSHB，所有信息</returns>
    public DataSet GTTBZLSHinfo(string Number)
    {
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        DataSet ds = null;
        Hashtable input = new Hashtable();
        Hashtable return_ht = new Hashtable();
        input["@Number"] = Number;
        string strTBDZLSH = "select c.* from AAA_THDYFHDXXB a left join AAA_ZBDBXXB b on a.ZBDBXXBBH = b.Number left join AAA_TBZLSHB c on b.T_YSTBDBH = c.TBDH where a.Number=@Number ";

        return_ht = I_DBL.RunParam_SQL(strTBDZLSH,"shinfo",input);
        if ((bool)(return_ht["return_float"])) //说明执行完成
        {
            //这里就可以用了。
            ds = (DataSet)(return_ht["return_ds"]);
        }
        return ds;
       
    }

    /// <summary>
    /// 获取合同状态，以及提货单发货单信息表所有信息
    /// </summary>
    /// <param name="number">AAA_THDYFHDXXB的Number值</param>
    /// <returns>返回Dataset内含 合同状态，以及提货单发货单信息表所有信息</returns>
    public DataSet GetTHDXXAndHTZT(string number)
    {
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        DataSet ds = null;
        Hashtable input = new Hashtable();
        Hashtable return_ht = new Hashtable();
        input["@Number"] = number;

        string strSQLDB = "select d.T_YSTBDMJJSBH 原始投标单卖家角色编号,d.Z_HTZT 合同状态,t.F_DQZT 发货单状态,d.Y_YSYDDDLYX 提醒对象登陆邮箱,'提醒对象用户名'=(select I_JYFMC from AAA_DLZHXXB DL where DL.J_BUYJSBH=d.Y_YSYDDMJJSBH),'提醒对象结算账户类型'=(select B_JSZHLX from AAA_DLZHXXB DL where DL.J_BUYJSBH=d.Y_YSYDDMJJSBH),d.Y_YSYDDMJJSBH '提醒对象角色编号','卖家用户名'=(select I_JYFMC from AAA_DLZHXXB DL where DL.J_SELJSBH=d.T_YSTBDMJJSBH),d.T_YSTBDDLYX 原始投标单登录邮箱,t.* from AAA_THDYFHDXXB t left join AAA_ZBDBXXB d on t.ZBDBXXBBH=d.Number where t.Number=@Number ";//"  select  d.Z_HTZT 合同状态,t.*  from AAA_THDYFHDXXB t,AAA_ZBDBXXB d where t.ZBDBXXBBH=d.Number and t.Number=@Number ";
        return_ht = I_DBL.RunParam_SQL(strSQLDB, "htinfo", input);

        if ((bool)(return_ht["return_float"])) //说明执行完成
        {
            //这里就可以用了。
            ds = (DataSet)(return_ht["return_ds"]);
        }
        return ds;

    }
    /// <summary>
    /// 《电子购货合同》缔约方保密承诺书，获取中标定标表的信息
    /// </summary>
    /// <param name="Number">中标定标信息表编号</param>
    /// <param name="DLYX"></param>
    /// <returns>返回dataset，其中包含表名为"返回值单条","买卖一家","卖家","买家"的Datatable</returns>
    public DataSet BMCNS(string Number,string DLYX)
    {
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化", "" });

        try
        {
            if (string.IsNullOrEmpty(Number) || string.IsNullOrEmpty(DLYX))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "传递的参数不可为空！";
                return dsreturn;
            }

            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");

            Hashtable htCS = new Hashtable();
            htCS["@Number"] = Number;
            htCS["@DLYX"] = DLYX;
            //当前买卖是一家
            string strMMSYJ = "select Number,isnull(T_MJSFQDCNS,'') '卖家是否签订承诺书',T_MJCNSQDSJ '卖家承诺书签订时间',isnull(Y_MJSFQDCNS,'') '买家是否签订承诺书',Y_MJCNSQDSJ '买家承诺书签订时间',T_YSTBDBH '原始投标单号',T_YSTBDDLYX '卖家登录邮箱',T_YSTBDJSZHLX '卖家结算账户类型',T_YSTBDMJJSBH '卖家角色编号',Y_YSYDDBH '原始预订单号',Y_YSYDDDLYX '买家登录邮箱',Y_YSYDDJSZHLX '买家结算账户类型',Y_YSYDDMJJSBH '买家角色编号' from  dbo.AAA_ZBDBXXB where Number=@Number and T_YSTBDDLYX=@DLYX and Y_YSYDDDLYX=@DLYX";
            Hashtable returnHT1 = I_DBL.RunParam_SQL(strMMSYJ,"", htCS);
            if (!(bool)returnHT1["return_float"])
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = returnHT1["return_errmsg"].ToString();
                return dsreturn;
            }
            DataTable dataTableMMSYJ = ((DataSet)returnHT1["return_ds"]).Tables[0].Copy();
            dataTableMMSYJ.TableName = "买卖一家";
            //当前是卖家
                string strDQSSeller = "select Number,isnull(T_MJSFQDCNS,'') '卖家是否签订承诺书',T_MJCNSQDSJ '卖家承诺书签订时间',T_YSTBDBH '原始投标单号',T_YSTBDDLYX '卖家登录邮箱',T_YSTBDJSZHLX '卖家结算账户类型',T_YSTBDMJJSBH '卖家角色编号' from  dbo.AAA_ZBDBXXB where Number=@Number and T_YSTBDDLYX=@DLYX ";
                Hashtable returnHT2 = I_DBL.RunParam_SQL(strDQSSeller, "", htCS);
                if (!(bool)returnHT2["return_float"])
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = returnHT2["return_errmsg"].ToString();
                    return dsreturn;
                }
                DataTable dataTableDQSSeller = ((DataSet)returnHT2["return_ds"]).Tables[0].Copy();
                dataTableDQSSeller.TableName = "卖家";
            //当前是买家
                string strDQSBuyer = "select Number,isnull(Y_MJSFQDCNS,'') '买家是否签订承诺书',Y_MJCNSQDSJ '买家承诺书签订时间',Y_YSYDDBH '原始预订单号',Y_YSYDDDLYX '买家登录邮箱',Y_YSYDDJSZHLX '买家结算账户类型',Y_YSYDDMJJSBH '买家角色编号'  from  dbo.AAA_ZBDBXXB where Number=@Number and Y_YSYDDDLYX=@DLYX ";
                Hashtable returnHT3 = I_DBL.RunParam_SQL(strDQSBuyer, "", htCS);
                if (!(bool)returnHT3["return_float"])
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = returnHT3["return_errmsg"].ToString();
                    return dsreturn;
                }
                DataTable dataTableDQSBuyer = ((DataSet)returnHT3["return_ds"]).Tables[0].Copy();
                dataTableDQSBuyer.TableName = "买家";
                dsreturn.Tables.AddRange(new DataTable[] { dataTableMMSYJ, dataTableDQSSeller, dataTableDQSBuyer });
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "查询成功";
                return dsreturn;
        }
        catch(Exception ex)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = ex.Message;
            return dsreturn;
        }

    }
    /// <summary>
    /// 根据中标定标信息表Number查询卖家名称、买家名称、合同编号、定标时间、保证函金额、保证函编号
    /// </summary>
    /// <param name="number">中标定标信息表Number</param>
    /// <returns>DataSet【包含“返回值单条”、“主表”两个表】</returns>
    public DataSet GetBZHMesg(string number)
    {
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化", "" });
        try
        {
            if (string.IsNullOrEmpty(number))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "传递的参数不可为空！";
                return dsreturn;
            }
            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");

            Hashtable htcs = new Hashtable();
            htcs["@number"] = number;
            string strSQL = " select '卖家名称'=(select I_JYFMC from AAA_DLZHXXB DL where DL.J_SELJSBH=b.T_YSTBDMJJSBH),'买家名称'=(select I_JYFMC from AAA_DLZHXXB DL where DL.J_BUYJSBH=b.Y_YSYDDMJJSBH),Z_HTBH 合同编号,convert(varchar(10),Z_DBSJ,120) 定标时间,Z_BZHJE 保证函金额,'保证函编号'='BH'+b.Number from AAA_ZBDBXXB b where b.Number=@number";
            Hashtable returnHT3 = I_DBL.RunParam_SQL(strSQL, "", htcs);
            if (!(bool)returnHT3["return_float"])
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = returnHT3["return_errmsg"].ToString();
                return dsreturn;
            }
            DataTable dataTableDQSBuyer = ((DataSet)returnHT3["return_ds"]).Tables[0].Copy();
            dataTableDQSBuyer.TableName = "主表";
            dsreturn.Tables.Add(dataTableDQSBuyer);
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "查询成功";
            return dsreturn;
        }
        catch (Exception ex)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = ex.Message;
            return dsreturn;
        }
    }
    /// <summary>
    /// 获取中标定标信息表详细信息 zhouli 2014.07.14 add
    /// </summary>
    /// <param name="Number">中标定标信息表Number</param>
    /// <returns>DataSet[两个DataTable:“返回值单条”、“HTPXX”]</returns>
    public DataSet jhjx_DBZBXXinfoget(string Number)
    {
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "没有查找到任何数据！", "" });
        try
        {
            if (string.IsNullOrEmpty(Number))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "传递的参数不可为空！";
                return dsreturn;
            }
            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
            Hashtable htcs = new Hashtable();
            htcs["@Number"] = Number;
            string Strsql = "   SELECT  distinct  AAA_ZBDBXXB.Number AS 主键 ,Z_QPZT AS 清盘状态,Z_QPKSSJ AS 清盘开始时间,Z_QPJSSJ AS 清盘结束时间, Z_HTBH AS 电子购货合同编号,Z_HTJSRQ AS 合同结束日期,Z_SPMC AS 商品名称,Z_SPBH AS 商品编号,Z_GG AS 规格,Z_ZBSL AS 合同数量,Z_ZBJG AS 单价,Z_ZBJE AS 合同金额,Z_LYBZJJE AS 履约保证金金额,  (select  isnull(sum (T_THSL),0)   from AAA_THDYFHDXXB where ZBDBXXBBH=AAA_ZBDBXXB.Number and  F_DQZT NOT IN ('无异议收货','默认无异议收货','有异议收货后无异议收货','补发货物无异议收货','卖家主动退货','撤销') AND (Z_QPZT='清盘中' or Z_QPZT='清盘结束' )) AS 争议数量 ,(select  ISNULL(sum (T_DJHKJE),0.00)   from AAA_THDYFHDXXB where ZBDBXXBBH=AAA_ZBDBXXB.Number and  F_DQZT NOT IN ('无异议收货','默认无异议收货','有异议收货后无异议收货','补发货物无异议收货','卖家主动退货','撤销') AND (Z_QPZT='清盘中' or Z_QPZT='清盘结束' ) ) AS  争议金额,'人工清盘' AS 清盘类型,CASE WHEN Z_HTZT='未定标废标' THEN '废标' WHEN Z_HTZT='定标合同到期' THEN '《电子购货合同》期满' WHEN Z_HTZT='定标合同终止' THEN '废标' WHEN Z_HTZT='定标执行完成' THEN '合同期内买家中标量全部无异议收货' ELSE '其他' END AS 清盘原因 ,T_YSTBDDLYX AS 卖方邮箱,(select I_JYFMC from AAA_DLZHXXB where B_DLYX=T_YSTBDDLYX) '卖方用户名',(select J_BUYJSBH from AAA_DLZHXXB where B_DLYX=T_YSTBDDLYX)'卖方买家角色编号',Y_YSYDDDLYX AS 买方邮箱,(select I_JYFMC from AAA_DLZHXXB where B_DLYX=Y_YSYDDDLYX) '买方用户名',(select J_BUYJSBH from AAA_DLZHXXB where B_DLYX=Y_YSYDDDLYX)'买方买家角色编号',Q_ZMSCFDLYX AS 证明上传方邮箱,Q_SFYQR AS 是否已确认 ,Q_ZMWJLJ AS 证明文件路径, Q_ZFLYZH AS 转付来源账户, Q_ZFMBZH AS 转付目标账户,Q_ZFJE AS 转付金额,Q_CLYJ AS 处理依据  FROM  AAA_ZBDBXXB JOIN AAA_THDYFHDXXB ON AAA_ZBDBXXB.Number=AAA_THDYFHDXXB.ZBDBXXBBH WHERE F_DQZT NOT IN ('无异议收货','默认无异议收货','有异议收货后无异议收货','补发货物无异议收货','卖家主动退货') AND  (Z_QPZT='清盘中' or Z_QPZT='清盘结束' ) and AAA_ZBDBXXB.Number=@Number and isnull(Z_QPKSSJ,'') != isnull(Z_QPJSSJ,'') ";//-- F_DQZT NOT IN ('无异议收货','默认无异议收货','有异议收货后无异议收货','补发货物无异议收货','卖家主动退货') AND
            Hashtable returnHT = I_DBL.RunParam_SQL(Strsql, "", htcs);
            if (!(bool)returnHT["return_float"])
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "单号为：" + Number + "中标定标信息单据查询失败！";
                return dsreturn;
            }
            DataSet ds = (DataSet)returnHT["return_ds"];
            if (ds != null && ds.Tables.Count>0 && ds.Tables[0].Rows.Count == 1)
            {
                DataTable dt = ds.Tables[0];
                DataTable dtRt = dt.Copy();
                // dtRt.Rows[0]["NEWDJDJ"] = (Convert.ToDouble(dtRt.Rows[0]["NDGJE"].ToString().Trim()) * Convert.ToDouble(htt["买家订金比率"].ToString().Trim())).ToString("#0.00");
                dtRt.TableName = "HTPXX";
                dsreturn.Tables.Add(dtRt);
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "查询成功！";
            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "单号为：" + Number + "中标定标信息单据不存在！";
                return dsreturn;
            }



            return dsreturn;
        }
        catch(Exception ex)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = ex.ToString();
            return dsreturn;
        }
    }
    /// <summary>
    /// 获取定标保证函详细信息 zhouli 2014.07.14 add
    /// </summary>
    /// <param name="Number">中标定标信息表Number</param>
    /// <returns>DataSet[两个DataTable:“返回值单条”、“SPXX”]</returns>
    public DataSet jhjx_DBYbzhinfo(string Number)
    {
         DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "没有查找到任何数据！", "" });
        try
        {
            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
            Hashtable htcs = new Hashtable();
            htcs["@Number"] = Number;
            string Strsql = "   select Z_HTBH as 合同编号, ( select MJSDJJPL from AAA_TBD where Number = AAA_ZBDBXXB.T_YSTBDBH  ) as 经济批量,Z_RJZGFHL as 日均最高发货量,Z_ZBSL as 定标数量,( select I_JYFMC from AAA_DLZHXXB where B_DLYX = AAA_ZBDBXXB.T_YSTBDDLYX ) as 卖家名称,( select I_LXRSJH from AAA_DLZHXXB where B_DLYX = AAA_ZBDBXXB.T_YSTBDDLYX ) as 卖家联系方式,( select I_LXRXM from AAA_DLZHXXB where B_DLYX = AAA_ZBDBXXB.T_YSTBDDLYX ) as 卖家联系人,( select I_JYFMC from AAA_DLZHXXB where B_DLYX = AAA_ZBDBXXB.Y_YSYDDDLYX ) as  买家家名称,( select I_LXRSJH from AAA_DLZHXXB where B_DLYX = AAA_ZBDBXXB.Y_YSYDDDLYX ) as 买家联系方式,( select I_LXRXM from AAA_DLZHXXB where B_DLYX = AAA_ZBDBXXB.Y_YSYDDDLYX ) as 买家联系人,Z_HTQX as 合同期限  from AAA_ZBDBXXB where Number = @Number";
            Hashtable returnHT = I_DBL.RunParam_SQL(Strsql, "", htcs);
            if (!(bool)returnHT["return_float"])
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "单号为：" + Number + "中标定标信息单据查询失败！";
                return dsreturn;
            }
            DataSet ds = (DataSet)returnHT["return_ds"];

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 1)
            {
                DataTable dt = ds.Tables[0];
                DataTable dtRt = dt.Copy();
                // dtRt.Rows[0]["NEWDJDJ"] = (Convert.ToDouble(dtRt.Rows[0]["NDGJE"].ToString().Trim()) * Convert.ToDouble(htt["买家订金比率"].ToString().Trim())).ToString("#0.00");
                dtRt.TableName = "SPXX";
                dsreturn.Tables.Add(dtRt);
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "查询成功！";
            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "单号为：" + Number + "中标定标信息单据不存在！";
                return dsreturn;
            }



            return dsreturn;
        }
        catch (Exception ex)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = ex.ToString();
            return dsreturn;
        }

    }
    /// <summary>
    /// 初始化返回值数据集,执行结果只有两种ok和err
    /// </summary>
    /// <returns></returns>
    public DataSet initReturnDataSet()
    {
        DataSet ds = new DataSet();
        DataTable auto2 = new DataTable();
        auto2.TableName = "返回值单条";
        auto2.Columns.Add("执行结果");
        auto2.Columns.Add("提示文本");
        auto2.Columns.Add("附件信息1");
        auto2.Columns.Add("附件信息2");
        auto2.Columns.Add("附件信息3");
        auto2.Columns.Add("附件信息4");
        auto2.Columns.Add("附件信息5");
        ds.Tables.Add(auto2);
        return ds;
    }

}