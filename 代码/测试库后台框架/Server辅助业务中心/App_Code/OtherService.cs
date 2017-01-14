
/************************************************
 * 
 * 创建人：zhaoxh
 * 
 * 创建时间：2014.5.22
 * 
 * 代码功能：辅助业务功能。
 * 
 * 参考文档：无
 * 
 */
using FMDBHelperClass;
using FMipcClass;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// SecondaryService 的摘要说明
/// </summary>
public class OtherService
{
    public OtherService()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}

    /// <summary>
    /// 添加大盘中的自选商品 zhaoxh 2014.5.22
    /// </summary>
    /// <param name="DLYX">登陆用户邮箱</param>
    /// <param name="SPBH">商品编号</param>
    /// <returns>成功/失败信息</returns>
    public string AddCommodity(string DLYX, string SPBH)
    {
        if (DLYX.Equals("") || SPBH.Equals(""))
        {
            return "传递的参数不可为空！";
        }

        string msg = "";
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        DataSet ds = new DataSet();
        Hashtable param = new Hashtable();
        param.Add("@DLYX", DLYX);
        param.Add("@SPBH", SPBH);
        Hashtable return_ht = new Hashtable();
        return_ht = I_DBL.RunParam_SQL("select count(number) from AAA_ZXSPJLB where DLYX = @DLYX and SPBH = @SPBH", "SPSL", param);
        //执行完成
        if ((bool)(return_ht["return_float"])) 
        {
            ds = (DataSet)(return_ht["return_ds"]);
            if (ds != null && ds.Tables["SPSL"].Rows[0][0].ToString() == "0") //不存在
            {
                /*************************************************************************/
                //原调用方法:jhjx_PublicClass.GetNextNumberZZ("AAA_ZXSPJLB", "");
                string Number = "";
                object[] re = IPC.Call("获取主键", new object[] { "AAA_ZXSPJLB", "" });
                if (re[0].ToString() == "ok")
                {
                    string reFF = (string)(re[1]);
                    if ((string)(re[1]) == null || ((string)(re[1])).Equals(""))
                    {
                        return "获取表（AAA_ZXSPJLB）主键失败";
                    }
                    Number = reFF;
                }
                else
                {
                    //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。
                    string reFF = re[1].ToString();
                    //return "IPC.Call(\"获取主键\", new object[] { \"AAA_ZXSPJLB\", \"\" }) 报错";
                    return "IPC调用\"获取主键\"方法时报错";
                }
                /*************************************************************************/
                string CreateTime = DateTime.Now.ToString();
                param = new Hashtable();
                param.Add("@Number", Number);
                param.Add("@DLYX", DLYX);
                param.Add("@SPBH", SPBH);
                param.Add("@CreateTime", CreateTime);
                ds = new DataSet();
                return_ht = I_DBL.RunParam_SQL("INSERT INTO AAA_ZXSPJLB ([Number],[DLYX],[SPBH],[SCSJ]) VALUES (@Number,@DLYX,@SPBH,@CreateTime)", param);
                if ((bool)(return_ht["return_float"]))
                {
                    if ((int)return_ht["return_other"] > 0)
                        msg = "成功";
                    else
                        msg = "插入了0条语句";
                }
                else
                {
                    return return_ht["return_errmsg"].ToString();
                }
            }
            else
            {
                return "请不要插入重复信息！";
            }
        }
        else
        {
            return return_ht["return_errmsg"].ToString();
        }
        return msg;
    }




        /// <summary>
    /// 删除大盘中的自选商品 zhaoxh 2014.5.22
    /// </summary>
    /// <param name="DLYX">登陆用户邮箱</param>
    /// <returns></returns>
    public DataSet dsZXSPtemp(string DLYX)
    {
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        DataSet ds = new DataSet();
        Hashtable param = new Hashtable();
        param.Add("@DLYX", DLYX);
        Hashtable return_ht = new Hashtable();
        return_ht = I_DBL.RunParam_SQL("select SPBH as 商品编号 from AAA_ZXSPJLB where DLYX = @DLYX ", "自选商品临时表", param);
        //执行完成
        if ((bool)(return_ht["return_float"]))
        {
            return  (DataSet)(return_ht["return_ds"]);
             
        }
        else
        {
            return null;
        }
    }


    /// <summary>
    /// 删除大盘中的自选商品 zhaoxh 2014.5.22
    /// </summary>
    /// <param name="DLYX">登陆用户邮箱</param>
    /// <param name="SPBH">商品编号</param>
    /// <returns>成功/失败信息</returns>
    public string DelCommodity(string DLYX, string SPBH)
    {

        if (DLYX.Equals("") || SPBH.Equals(""))
        {
            return "传递的参数不可为空！";
        }


        string msg = "";
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        DataSet ds = new DataSet();
        Hashtable param = new Hashtable();
        param.Add("@DLYX", DLYX);
        param.Add("@SPBH", SPBH);
        Hashtable return_ht = I_DBL.RunParam_SQL("delete AAA_ZXSPJLB where DLYX = @DLYX and SPBH = @SPBH ", param);
        //执行完成
        if ((bool)(return_ht["return_float"]))
        {
            if ((int)return_ht["return_other"]>0)
            {
                msg = "成功";
            }
            else
            {
                return "没找到这条数据，无法删除";
            }
        }
        else
        {
            return return_ht["return_errmsg"].ToString();
        } 
        return msg;
    }

    /// <summary>
    /// 判断商品是否进入冷静期 zhaoxh 2014.5.22
    /// </summary>
    /// <param name="spbh">商品编号</param>
    /// <param name="htqx">合同期限（三个月/一年）</param>
    /// <returns>是：在冷静期/否：不在冷静期</returns>
    public string IsLJQ(string spbh, string htqx)
    {
        if (spbh.Equals("") || htqx.Equals(""))
        {
            return "传递的参数不可为空！";
        }

        string msg = "";
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        DataSet ds = new DataSet();
        Hashtable param = new Hashtable();
        param.Add("@spbh", spbh);
        param.Add("@htqx", htqx);
        Hashtable return_ht = I_DBL.RunParam_SQL("SELECT 1 FROM AAA_LJQDQZTXXB WHERE SPBH=@SPBH AND SFJRLJQ='是' AND HTQX=@HTQX ","ljq" , param);
        //执行完成
        if ((bool)(return_ht["return_float"]))
        {
            ds = (DataSet)return_ht["return_ds"];
            if (ds != null && ds.Tables["ljq"].Rows.Count > 0)
            {
                msg = "是";
            }
            else
            {
                msg = "否";
            }
        }
        else
        {
            return return_ht["return_errmsg"].ToString();
        }
        return msg;
    }
    /// <summary>
    /// 判断商品是否有效 zhaoxh 2014.5.22
    /// </summary>
    /// <param name="spbh">商品编号</param>
    /// <returns>是：有效/否：无效</returns>
    public string IsSpValid(string spbh)
    {

        if (spbh.Equals(""))
        {
            return "商品编号不可为空";
        }

        string msg = "";
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        DataSet ds = new DataSet();
        Hashtable param = new Hashtable();
        param.Add("@spbh", spbh);
        Hashtable return_ht = I_DBL.RunParam_SQL("SELECT SFYX FROM AAA_PTSPXXB WHERE SPBH=@spbh ", "sftx", param);
        //执行完成
        if ((bool)(return_ht["return_float"]))
        {
            ds = (DataSet)return_ht["return_ds"];
            if (ds != null && ds.Tables["sftx"].Rows.Count > 0 && ds.Tables["sftx"].Rows[0][0].ToString() == "是")
            {
                msg = "是";
            }
            else
            {
                msg = "否";
            }
        }
        else
        {
            return return_ht["return_errmsg"].ToString();
        }
        return msg;
    }

    /// <summary>
    /// 判断履约保证金是否满足比率 zhaoxh 2014.5.23
    /// </summary>
    /// <param name="LYBZJBZBL">履约保证金比率</param>
    /// <param name="HTBH">合同编号</param>
    /// <returns>是：满足比率/否：不足比率</returns>
    public string BZJSFBZBL(string LYBZJBZBL, string HTBH)
    {
        if (LYBZJBZBL.Equals("") || HTBH.Equals(""))
        {
            return "传递的参数不可为空";
        }

        string msg = "";
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        DataSet ds = new DataSet();
        Hashtable param = new Hashtable();
        param.Add("@LYBZJBZBL", LYBZJBZBL);
        param.Add("@HTBH", HTBH);
        Hashtable return_ht = I_DBL.RunParam_SQL(
            @"select * from ( 
	            select '履约保证金金额'=Z_LYBZJJE, 
		            '两项延迟总金额'=isnull( (select sum(LS.JE) from  AAA_ZKLSMXB as LS where LS.XM='违约赔偿金' and  LS.XZ in ( '超过最迟发货日后录入发货信息','发货单生成后5日内未录入发票邮寄信息') and LS.SJLX='预' and LS.LYDH in ( select TTT.Number from AAA_THDYFHDXXB as TTT where  TTT.ZBDBXXBBH = ZZZ.Number ) ),0.00),
		            '是否已解冻过订金'=isnull((select top 1 ZK.LYDH from AAA_ZKLSMXB as ZK where ZK.LYDH=ZZZ.Number and ZK.LYYWLX='AAA_ZBDBXXB' and ZK.XM='订金' and ZK.XZ='合同期内下达完全部提货单解冻' and ZK.SJLX='实' ),'否') , 
		            * 
	            from AAA_ZBDBXXB as ZZZ where ZZZ.Z_HTZT = '定标' 
            ) as tab11 
            where convert(float,履约保证金金额-两项延迟总金额)/convert(float,履约保证金金额) >= convert(float,@LYBZJBZBL) 
            and Z_HTBH=@HTBH", "lybzj", param);
        //执行完成
        if ((bool)(return_ht["return_float"]))
        {
            ds = (DataSet)return_ht["return_ds"];
            if (ds != null && ds.Tables["lybzj"].Rows.Count > 0)
            {
                msg = "是";
            }
            else
            {
                msg = "否";
            }
        }
        else
        {
            return return_ht["return_errmsg"].ToString();
        }
        return msg;
    }

    /// <summary>
    /// 获取买家最近一次发票信息 zhaoxh 2014.5.23
    /// </summary>
    /// <param name="DLYX">登录邮箱</param>
    /// <returns>dataset结果集/null:报错</returns>
    public DataSet GetFPXX(string DLYX)
    {

        if (DLYX.Equals(""))
        {
            return null;
        }

        DataSet rtds = new DataSet();

        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        DataSet ds = new DataSet();
        Hashtable param = new Hashtable();
        param.Add("@DLYX1", DLYX);
        param.Add("@DLYX2", DLYX);
        param.Add("@DLYX3", DLYX);
        param.Add("@DLYX4", DLYX);
        param.Add("@DLYX5", DLYX);
        param.Add("@DLYX6", DLYX);
        param.Add("@DLYX7", DLYX);
        param.Add("@DLYX8", DLYX);
        param.Add("@DLYX9", DLYX);
        Hashtable return_ht = I_DBL.RunParam_SQL(
            @"select I_SWDJZSH 税号,
	            '收货详细地址'=(select top 1 T_SHXXDZ from AAA_THDYFHDXXB a left join AAA_ZBDBXXB b on a.ZBDBXXBBH=b.Number where b.Y_YSYDDDLYX=@DLYX1 order by T_THDXDSJ desc),
	            '收货人姓名'=(select top 1 T_SHRXM from AAA_THDYFHDXXB a left join AAA_ZBDBXXB b on a.ZBDBXXBBH=b.Number where b.Y_YSYDDDLYX=@DLYX2 order by T_THDXDSJ desc),
	            '收货人联系方式'=(select top 1 T_SHRLXFS from AAA_THDYFHDXXB a left join AAA_ZBDBXXB b on a.ZBDBXXBBH=b.Number where b.Y_YSYDDDLYX=@DLYX3 order by T_THDXDSJ desc),
	            '发票类型'=(select top 1 T_FPLX from AAA_THDYFHDXXB a left join AAA_ZBDBXXB b on a.ZBDBXXBBH=b.Number where b.Y_YSYDDDLYX=@DLYX4 order by T_THDXDSJ desc),
	            '发票抬头'=(select top 1 T_FPTT from AAA_THDYFHDXXB a left join AAA_ZBDBXXB b on a.ZBDBXXBBH=b.Number where b.Y_YSYDDDLYX=@DLYX5 order by T_THDXDSJ desc),
	            '开户银行'=(select top 1 T_KHYH from AAA_THDYFHDXXB a left join AAA_ZBDBXXB b on a.ZBDBXXBBH=b.Number where a.T_FPLX='增值税专用发票' and b.Y_YSYDDDLYX=@DLYX6 order by T_THDXDSJ desc),
	            '银行账号'=(select top 1 T_YHZH from AAA_THDYFHDXXB a left join AAA_ZBDBXXB b on a.ZBDBXXBBH=b.Number where a.T_FPLX='增值税专用发票' and b.Y_YSYDDDLYX=@DLYX7 order by T_THDXDSJ desc),
	            '单位地址'=(select top 1 T_DWDZ from AAA_THDYFHDXXB a left join AAA_ZBDBXXB b on a.ZBDBXXBBH=b.Number where a.T_FPLX='增值税专用发票' and b.Y_YSYDDDLYX=@DLYX8 order by T_THDXDSJ desc) 
            from AAA_DLZHXXB c where c.B_DLYX=@DLYX9", "FPXX", param);
        //执行完成
        if ((bool)(return_ht["return_float"]))
        {
            ds = (DataSet)return_ht["return_ds"];
            rtds = ds;
        }
        else
        {
            rtds = null;
        }
        return rtds;
    }


    /// <summary>
    /// 获取省市区信息 wyh 2014.06.03 add 获取是省市区基本信息 
    /// </summary>
    /// <returns>包含省市区信息的Dataset</returns>
    public DataSet dsSSQ()
    {
        DataSet dsssq = new DataSet();
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        DataSet ds = new DataSet();
        Hashtable htresult = new Hashtable();
        //获取省市区
        DataTable dt_sheng = new DataTable("省");

        htresult = I_DBL.RunProc("select p_number as 省编号,p_namestr as 省名,'0' as 省标记 from AAA_CityList_Promary", "省");
        dt_sheng = ((DataSet)htresult["return_ds"]).Tables["省"].Copy();


        DataTable dt_shi = new DataTable("市");
        htresult = I_DBL.RunProc("select AAA_CityList_City.c_number as 市编号,AAA_CityList_City.c_namestr as 市名,AAA_CityList_City.p_number as 所属省编号,(select AAA_CityList_Promary.p_namestr from AAA_CityList_Promary where AAA_CityList_Promary.p_number=AAA_CityList_City.p_number) as 所属省名 from AAA_CityList_City", "市");
        dt_shi = ((DataSet)htresult["return_ds"]).Tables["市"].Copy();


        DataTable dt_qu = new DataTable("区");
        htresult = I_DBL.RunProc("select AAA_CityList_qu.q_number as 区编号,AAA_CityList_qu.q_namestr as 区名,AAA_CityList_qu.c_number as 所属市编号,(select AAA_CityList_City.c_namestr from AAA_CityList_City where AAA_CityList_qu.c_number=AAA_CityList_City.c_number) as 所属市名 from AAA_CityList_qu", "区");
        dt_qu = ((DataSet)htresult["return_ds"]).Tables["区"].Copy();

        dsssq.Tables.Add(dt_sheng);
        dsssq.Tables.Add(dt_shi);
        dsssq.Tables.Add(dt_qu);
        return dsssq;
    }


    /// <summary>
    /// 获取分公司对照表信息
    /// </summary>
    /// <returns>包含分公司对照片信息的Dataset</returns>
    public DataSet dsFGSdzb()
    {
        DataSet dsfgsdzb = new DataSet();
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        DataSet ds = new DataSet();
        Hashtable htresult = new Hashtable();

        DataTable dt_fgs = new DataTable("分公司对照表");

        htresult = I_DBL.RunProc("select * from AAA_CityList_FGS  ", "分公司对照表");
        dt_fgs = ((DataSet)htresult["return_ds"]).Tables["分公司对照表"].Copy();
        dsfgsdzb.Tables.Add(dt_fgs);
        return dsfgsdzb;

    }


    /// <summary>
    /// 获取分公司信息包含：公司Number', '分公司', '办事处'
    /// </summary>
    /// <returns>返回包含：公司Number', '分公司', '办事处'的Dataset</returns>
    public DataSet dsFGS()
    {
        DataSet dsFGS = new DataSet();
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        DataSet ds = new DataSet();
        Hashtable htresult = new Hashtable();

        DataTable dt_fgs = new DataTable("分公司表");

        htresult = I_DBL.RunProc("select Number '公司Number', GLBMMC '分公司',GLBMZH '办事处' from AAA_PTGLJGB where GLBMFLMC='分公司' and SFYX='是'", "分公司表");
        dt_fgs = ((DataSet)htresult["return_ds"]).Tables["分公司表"].Copy();
        dsFGS.Tables.Add(dt_fgs);
        return dsFGS;
    }


    /// <summary>
    /// 获取高校院系信息
    /// </summary>
    /// <returns></returns>
    public DataSet GetYXBXX()
    {
        DataSet dsYXX = new DataSet();
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        DataSet ds = new DataSet();
        Hashtable htresult = new Hashtable();

        DataTable dt_YXX = new DataTable("院系表");

        htresult = I_DBL.RunProc("select Number '院系Number', GXZH '高校账户',YXMC '院系名称' from AAA_PTYXB ", "院系表");
        dt_YXX = ((DataSet)htresult["return_ds"]).Tables["院系表"].Copy();
        dsYXX.Tables.Add(dt_YXX);
        return dsYXX;

    }

    /// <summary>
    /// 获取最新动态参数表
    /// </summary>
    /// <returns>包含最新动态参数表的Dataset</returns>
    public DataSet GetDTCSBXX()
    {
        DataSet dsYXX = new DataSet();
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        DataSet ds = new DataSet();
        Hashtable htresult = new Hashtable();

        DataTable dt_YXX = new DataTable("动态参数");

        htresult = I_DBL.RunProc("select top 1 * from AAA_PTDTCSSDB order by CreateTime DESC ", "动态参数");
        dt_YXX = ((DataSet)htresult["return_ds"]).Tables["动态参数"].Copy();
        dsYXX.Tables.Add(dt_YXX);
        return dsYXX;
    }




    /// <summary>
    /// 根据省市获取平台管理机构 wyh 2014.06.03 
    /// </summary>
    /// <param name="hashTable">包含省市信息的Dataset</param>
    /// <returns>返回平台管理机构</returns>
    public string GetPTGLJG(DataSet  hashTable)
    {

            DataSet dsYXX = new DataSet();
            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
            DataSet ds = new DataSet();
            Hashtable return_ht = new Hashtable();
            if (hashTable == null || hashTable.Tables[0].Rows[0]["省份"].ToString().Equals("") || hashTable.Tables[0].Rows[0]["地市"].ToString().Equals(""))
            {
                return "不可传递参数为空的数据";
            }

            return_ht = I_DBL.RunProc("select Pname,Cname,FGSname,BSCname from AAA_CityList_FGS where Pname like '" + hashTable.Tables[0].Rows[0]["省份"].ToString() + "%' and Cname like '" + hashTable.Tables[0].Rows[0]["地市"].ToString() + "%'","GLJG");

     //执行完成
         if ((bool)(return_ht["return_float"]))
         {
             ds = (DataSet)return_ht["return_ds"];
             if (ds != null && ds.Tables["GLJG"].Rows.Count > 0 && ds.Tables["GLJG"].Rows[0][0].ToString()!= "")
             {
                 return ds.Tables["GLJG"].Rows[0]["FGSname"].ToString();
             }
             else
             {
                 return "没有对应的数据";
             }
         }
         else
         {
             return return_ht["return_errmsg"].ToString();
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
    /// <summary>
    /// 2014-06-16 shiyan
    /// 获取C区上部关于钱的数据值
    /// </summary>
    /// <param name="dlyx">登录邮箱</param>
    /// <param name="type">交易方类型</param>
    /// <returns>结果数据集</returns>
    public DataSet GetCTongJi(string dlyx, string type)
    {
        DataSet ds = initReturnDataSet();
        ds.Tables["返回值单条"].Rows.Add(new string[] { "ok", "初始化", "0.00", "0.00", "0.00" });

        try
        {           
            Hashtable input = new Hashtable();
            input["@dlyx"] = dlyx;

            //获取当前冻结金额
            string sql_dj = "select isnull(sum(case yslx when '冻结' then je when '解冻' then -je end),0.00) as 当前冻结 from AAA_ZKLSMXB where dlyx=@dlyx and yslx in ('冻结','解冻') ";
            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
            Hashtable result_dj = I_DBL.RunParam_SQL(sql_dj, "冻结", input);
            if ((bool)result_dj["return_float"])
            {
                DataSet ds_dj=(DataSet)result_dj["return_ds"];
                ds.Tables[0].Rows[0]["附件信息1"] = ds_dj.Tables[0].Rows[0]["当前冻结"].ToString();
            }
            else
            {
                ds.Tables[0].Rows[0]["执行结果"] = "err";
                ds.Tables[0].Rows[0]["提示文本"] = "未获得当前冻结金额";
                ds.Tables[0].Rows[0]["附件信息1"] = "0.00";
            }

            //获取当前可用余额
            object[] re_yue = IPC.Call("获取余额", new object[] { dlyx });
            if (re_yue[0].ToString() == "ok")
            {
                DataSet ds_yue = (DataSet)re_yue[1];
                ds.Tables[0].Rows[0]["附件信息2"] = ds_yue.Tables[0].Rows[0]["账户当前可用余额"].ToString();
            }
            else
            {
                ds.Tables[0].Rows[0]["执行结果"] = "err";
                ds.Tables[0].Rows[0]["提示文本"] = "未获得当前可用余额";
                ds.Tables[0].Rows[0]["附件信息2"] = "0.00";
            }

            //获取当前经纪人收益金额
            if (type.IndexOf("经纪人") >= 0)
            {

                string sql_sy = "select isnull(sum(case yslx when '增加' then je when '扣减' then -je end),0.00) as 当前收益 from AAA_ZKLSMXB where dlyx=@dlyx and yslx in ('增加','扣减') and sjlx='实' and xm='经纪人收益'  ";

                Hashtable result_sy = I_DBL.RunParam_SQL(sql_sy, "收益", input);
                if ((bool)result_sy["return_float"])
                {
                    ds.Tables[0].Rows[0]["附件信息3"] = ((DataSet)result_dj["return_ds"]).Tables[0].Rows[0]["当前收益"].ToString();
                }
                else
                {
                    ds.Tables[0].Rows[0]["执行结果"] = "err";
                    ds.Tables[0].Rows[0]["提示文本"] = "未获得当前收益金额";
                    ds.Tables[0].Rows[0]["附件信息3"] = "0.00";
                }
            }
        }
        catch
        {
            ds.Tables[0].Rows[0]["执行结果"] = "err";
            ds.Tables[0].Rows[0]["提示文本"] = "数据获取失败";
        }
        return ds;
    }

    public DataSet GetCZiJinXM(string index, string type)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        Hashtable input = new Hashtable();
        input["@index"] = index;
        input["@type"] = type;

        string sql = "select XM, XZ as XZvalue,(case when LEN (XZ)>10 then SUBSTRING (xz,0,11)+'...' else XZ end) as XZtext from AAA_moneyDZB where 1=1 ";
        if (index != "" && index != "0")
        {
            sql = sql + " and dymkbh=@index";
        }
        if (type.ToString() != "")
        {
            sql = sql + " and sjlx=@type";
        }
        sql = sql + " order by dymkbh";

        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable result = I_DBL.RunParam_SQL(sql, "项目性质", input);
        if ((bool)result["return_float"])
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取数据成功";
            DataTable dt = new DataTable();
            dt = ((DataSet)result["return_ds"]).Tables[0].Copy();
            dt.TableName = "ResDT";
            dsreturn.Tables.Add(dt);
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取数据失败";
            DataTable dt = new DataTable();
            dt = null;
            dt.TableName = "ResDT";
            dsreturn.Tables.Add(dt);
        }
        return dsreturn;
    }

    /// <summary>
    /// 提交/修改开票信息
    /// </summary>
    /// <param name="dt">参数数据表</param>
    /// <returns></returns>
    public Hashtable CommitKPXX(DataTable dt)
    {
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");      
        Hashtable param = new Hashtable();
        param.Add("@DLYX", dt.Rows[0]["登陆邮箱"].ToString());

        param.Add("@FPLX", dt.Rows[0]["发票类型"].ToString());
        param.Add("@DWMC", dt.Rows[0]["单位名称"].ToString());
        param.Add("@NSRSBH", dt.Rows[0]["纳税人识别号"].ToString());
        param.Add("@DWDZ", dt.Rows[0]["单位地址"].ToString());
        param.Add("@LXDH", dt.Rows[0]["联系电话"].ToString());
        param.Add("@KHH", dt.Rows[0]["开户行"].ToString());
        param.Add("@KHZH", dt.Rows[0]["开户账号"].ToString());
        param.Add("@YBNSRZGZ", dt.Rows[0]["一般纳税人资格证"].ToString());
        param.Add("@FPJSDWMC", dt.Rows[0]["发票接收单位名称"].ToString());
        param.Add("@FPJSDZ", dt.Rows[0]["发票接收地址"].ToString());
        param.Add("@FPJSLXR", dt.Rows[0]["发票接收联系人"].ToString());
        param.Add("@FPJSLXRDH", dt.Rows[0]["发票接收联系人电话"].ToString());
        param.Add("@Number", dt.Rows[0]["Number"].ToString());

        Hashtable ht = new Hashtable();
        ht["执行结果"] = "err";
        ht["提示文字"] = "未执行任何操作！";

        //获取买家卖家交易账户的平台管理机构
        string ptgljg = dt.Rows[0]["平台管理机构"].ToString();
        if (dt.Rows[0]["交易账户类型"].ToString() == "买家卖家交易账户")
        {
            string sql = "select I_PTGLJG from AAA_MJMJJYZHYJJRZHGLB as a left join AAA_DLZHXXB as b on a.GLJJRBH =b.J_JJRJSBH where a.SFDQMRJJR ='是' and a.DLYX=@DLYX ";
            
            Hashtable result = I_DBL.RunParam_SQL(sql, "管理机构", param);
            if ((bool)result["return_float"])
            {
                DataTable dtr = ((DataSet)result["return_ds"]).Tables["管理机构"];
                if (dtr != null && dtr.Rows.Count > 0)
                {
                    ptgljg = dtr.Rows[0]["I_PTGLJG"].ToString(); 
                }
            }
           
        }
        if (ptgljg.Trim() == "")
        {
            ht["执行结果"] = "err";
            ht["提示文字"] = "基础数据获取失败，无法执行提交操作！";
        }
               

        if (dt.Rows[0]["Number"].ToString() == "")
        {//提交新的开票信息
            try
            {              
                string KeyNumber = "";
                object[] reynum = IPC.Call("获取主键", new object[] { "AAA_PTKPXXB", "" });
                if (reynum[0].ToString() == "ok")
                {
                    KeyNumber = (string)(reynum[1]);
                }
                else
                {
                    return null;
                }

                param.Add("@key", KeyNumber);
                param.Add("@KHBH", dt.Rows[0]["客户编号"].ToString());
                param.Add("@JYZHLX", dt.Rows[0]["交易账户类型"].ToString());
                param.Add("@ZCLB", dt.Rows[0]["注册类别"].ToString());
                param.Add("@GLJG", ptgljg);
                 
                string sql_insertZB = "INSERT INTO [AAA_PTKPXXB]([Number],[DLYX],[KHBH],[JYZHLX],[ZCLB],[PTGLJG],[FPLX],[DWMC],[YBNSRSBH],[DWDZ],[LXDH],[KHH],[KHZH],[YBNSRZGZ],[FPJSDWMC],[FPJSDZ],[FPJSLXR],[FPJSLXRDH],[ZT],[ZHGXSJ],[CheckState],[CreateUser],[CreateTime],[CheckLimitTime])  VALUES ( @key,@DLYX,@KHBH,@JYZHLX,@ZCLB,@GLJG,@FPLX,@DWMC,@NSRSBH,@DWDZ,@LXDH,@KHH,@KHZH,@YBNSRZGZ,@FPJSDWMC,@FPJSDZ,@FPJSLXR,@FPJSLXRDH,'待审核','" + DateTime.Now.ToString() + "',0,'admin','" + DateTime.Now.ToString() + "','" + DateTime.Now.ToString() + "')";

                Hashtable htcount = I_DBL.RunParam_SQL(sql_insertZB, param);
                if ((bool)htcount["return_float"] && (int)htcount["return_other"]>0)
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
                string new_zt = "";
                Hashtable result = I_DBL.RunParam_SQL("select zt from AAA_PTKPXXB where number=@Number", "状态", param);
                if ((bool)result["return_float"])
                {
                    DataTable dtr = ((DataSet)result["return_ds"]).Tables["状态"];
                    if (dtr != null && dtr.Rows.Count > 0)
                    {
                        new_zt = dtr.Rows[0]["zt"].ToString();
                    }
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
                    string sql_update = "UPDATE [AAA_PTKPXXB]  SET [FPLX]=@FPLX,[DWMC]=@DWMC,[YBNSRSBH]=@NSRSBH, [DWDZ] = @DWDZ,[LXDH] =@LXDH,[KHH] = @KHH,[KHZH] =@KHZH,[YBNSRZGZ] = @YBNSRZGZ,[FPJSDWMC] = @FPJSDWMC,[FPJSDZ] =@FPJSDZ,[FPJSLXR] = @FPJSLXR,[FPJSLXRDH] = @FPJSLXRDH,[ZT] ='待审核',[ZHGXSJ] = '" + DateTime.Now.ToString() + "' WHERE Number=@Number ";

                    Hashtable htcount = I_DBL.RunParam_SQL(sql_update, param);
                    if ((bool)htcount["return_float"] && (int)htcount["return_other"] > 0)
                    {
                        ht["执行结果"] = "ok";
                        ht["提示文字"] = "开票信息修改成功！"; 
                    }
                  
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

    /// <summary>
    /// 变更开票信息
    /// </summary>
    /// <param name="dt">参数数据表</param>
    /// <returns></returns>
    public Hashtable CommitKPXXChange(DataTable dt)
    {
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable param = new Hashtable();
        
        param.Add("@ID", dt.Rows[0]["ID"].ToString());
        param.Add("@YBNSRZGZM", dt.Rows[0]["一般纳税人资格证明"].ToString());      
        param.Add("@BGXXSMJ", dt.Rows[0]["变更信息扫描件"].ToString());
        param.Add("@Number", dt.Rows[0]["Number"].ToString());

        
        Hashtable ht = new Hashtable();
        ht["执行结果"] = "err";
        ht["提示文字"] = "未执行任何操作！";
        

        if (dt.Rows[0]["ID"].ToString() == "")
        {//提交新的变更信息
            try
            {
                string sql_insert = "INSERT INTO [AAA_PTKPXX_BGSQ]([parentNumber],[BGSQSMJ],[YBNSRZGZM],[SQTJSJ],[CLZT]) VALUES (@Number,@BGXXSMJ,@YBNSRZGZM,'" + DateTime.Now.ToString() + "','待处理')";
                Hashtable htcount = I_DBL.RunParam_SQL(sql_insert, param);
                if ((bool)htcount["return_float"] && (int)htcount["return_other"] > 0)
                {
                    ht["执行结果"] = "ok";
                    ht["提示文字"] = "变更信息提交成功！";
                }
                else
                {
                    ht["执行结果"] = "err";
                    ht["提示文字"] = "变更信息提交失败！";
 
                }
            }
            catch 
            {
                ht["执行结果"] = "err";
                ht["提示文字"] = "变更信息提交失败！";
            }
        }
        else
        { //修改原来的变更信息
           
            string new_zt = "";
            Hashtable result = I_DBL.RunParam_SQL("select clzt from AAA_PTKPXX_BGSQ where id=@ID", "状态", param);
            if ((bool)result["return_float"])
            {
                DataTable dtr = ((DataSet)result["return_ds"]).Tables["状态"];
                if (dtr != null && dtr.Rows.Count > 0)
                {
                    new_zt = dtr.Rows[0]["clzt"].ToString();
                }
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
                        string sql_update = "update AAA_PTKPXX_BGSQ set bgsqsmj=@BGXXSMJ,ybnsrzgzm=@YBNSRZGZM,sqtjsj='" + DateTime.Now.ToString() + "' where parentnumber=@Number and id=@ID ";

                        Hashtable htcount = I_DBL.RunParam_SQL(sql_update, param);
                        if ((bool)htcount["return_float"] && (int)htcount["return_other"] > 0)
                        {
                            ht["执行结果"] = "ok";
                            ht["提示文字"] = "变更信息提交成功！";
                        }

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
                        string sql_insert = "INSERT INTO [AAA_PTKPXX_BGSQ]([parentNumber],[BGSQSMJ],[YBNSRZGZM],[SQTJSJ],[CLZT]) VALUES (@Number,@BGXXSMJ,@YBNSRZGZM,'" + DateTime.Now.ToString() + "','待处理')";

                        Hashtable htcount = I_DBL.RunParam_SQL(sql_insert, param);
                        if ((bool)htcount["return_float"] && (int)htcount["return_other"] > 0)
                        {

                            ht["执行结果"] = "ok";
                            ht["提示文字"] = "变更信息提交成功！";
                        }
                        else
                        {
                            ht["执行结果"] = "ok";
                            ht["提示文字"] = "变更信息提交成功！";
 
                        }
                    }
                    catch 
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
    /// <summary>
    /// 开票信息维护部分的获取开票信息
    /// </summary>
    /// <param name="dlyx"></param>
    /// <returns></returns>
    public Hashtable GetKPXXinfo(string dlyx)
    {
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable param = new Hashtable();

        param.Add("@dlyx", dlyx);

        Hashtable ht = new Hashtable();
        ht["执行结果"] = "ok";
        ht["提示文字"] = "基础数据获取成功！";
        ht["开票信息"] = "";
        ht["注册信息"] = "";
        try
        {
            string sql_kpxx = "select a.*,b.id as BGid,b.parentNumber as BGparentNumber,b.BGSQSMJ,b.YBNSRZGZM,b.SQTJSJ,b.CLZT,b.SLBZ,c.id as SHid,c.parentNumber as SHparentNumbe,c.SHSJ,c.SHJG,c.SHXX,d.I_JYFMC,(case when d.I_YBNSRZGZSMJ='DBL.png' then '' else d.I_YBNSRZGZSMJ end) as I_YBNSRZGZSMJ,I_SWDJZSH,I_XXDZ,I_JYFLXDH,I_KHYH,I_YHZH from AAA_PTKPXXB as a left join (select top 1 AAA_PTKPXX_BGSQ.* from AAA_PTKPXX_BGSQ left join AAA_PTKPXXB on AAA_PTKPXX_BGSQ.parentnumber=AAA_PTKPXXB.number where dlyx=@dlyx order by sqtjsj desc)  as b on a.number=b.parentnumber left join (select top 1 AAA_PTKPXXB_TJSHXX.* from AAA_PTKPXXB_TJSHXX left join AAA_PTKPXXB on AAA_PTKPXXB_TJSHXX.parentnumber=AAA_PTKPXXB.number where dlyx=@dlyx order by tjsj desc) as c on c.parentnumber=a.number left join AAA_DLZHXXB as d on a.dlyx=d.B_DLYX where a.dlyx=@dlyx ";
          
            Hashtable result = I_DBL.RunParam_SQL(sql_kpxx, "kpxx", param);
            if ((bool)result["return_float"])
            {
                ht["开票信息"] = ((DataSet)result["return_ds"]).Tables["kpxx"];

            }
            string sql_zcxx = "select B_DLYX,I_ZCLB,I_JYFMC,I_SWDJZSH,I_XXDZ,I_JYFLXDH,I_KHYH,I_YHZH,(case when I_YBNSRZGZSMJ='DBL.png' then '' else I_YBNSRZGZSMJ end) as I_YBNSRZGZSMJ,I_LXRXM,I_LXRSJH,S_SFYBJJRSHTG,S_SFYBFGSSHTG from AAA_DLZHXXB where B_DLYX=@dlyx";

            Hashtable rezc = I_DBL.RunParam_SQL(sql_zcxx, "zcxx", param);
             if ((bool)rezc["return_float"])
             {
                 ht["注册信息"] = ((DataSet)result["return_ds"]).Tables["zcxx"];
             }         
        }
        catch 
        {
            ht["执行结果"] = "err";
            ht["提示文字"] = "基础数据获取失败！";
        }
        return ht;
    }

    /// <summary>
    /// 判断身份证号是否重复
    /// </summary>
    /// <param name="strJJRZGZ"></param>
    /// <param name="strSFZH"></param>
    /// <returns></returns>
    public DataSet JudgeSFZHXX(string strZHLX, string strSFZH)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable param = new Hashtable();

        //param.Add("@strZHLX", strZHLX);
        param.Add("@strSFZH", strSFZH);

        string strSql = "";
        switch (strZHLX)
        {
            case "个人身份证号":
                strSql = " select * from dbo.AAA_DLZHXXB where I_SFZH=@strSFZH  or I_FDDBRSFZH=@strSFZH ";
                break;
            case "法定代表人身份证号":
                strSql = " select * from dbo.AAA_DLZHXXB where I_FDDBRSFZH=@strSFZH  or I_SFZH=@strSFZH ";
                break;
        }
        DataTable dataTable = new DataTable();
        Hashtable rezc = I_DBL.RunParam_SQL(strSql, "zcxx", param);
        if ((bool)rezc["return_float"])
        {
            dataTable = ((DataSet)rezc["return_ds"]).Tables["zcxx"];
        }     
              
        if (dataTable.Rows.Count == 0)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "此身份证可以注册";
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "此身份证已经被注册";
        }
        return dsreturn;
    }


    /// <summary>
    /// 判断是否签订承诺书
    /// </summary> 
    /// <param name="dataTable"></param>
    /// <returns></returns>
    public DataSet JudgeSFQDCNS( DataTable dataTable)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable param = new Hashtable();

        param.Add("@znum", dataTable.Rows[0]["中标定标_Number"].ToString());
        param.Add("@dlyx", dataTable.Rows[0]["登录邮箱"].ToString());

        try
        {

            //当前买卖是一家
            string strMMSYJ = "select Number,isnull(T_MJSFQDCNS,'') '卖家是否签订承诺书',T_MJCNSQDSJ '卖家承诺书签订时间',isnull(Y_MJSFQDCNS,'') '买家是否签订承诺书',Y_MJCNSQDSJ '买家承诺书签订时间',T_YSTBDBH '原始投标单号',T_YSTBDDLYX '卖家登录邮箱',T_YSTBDJSZHLX '卖家结算账户类型',T_YSTBDMJJSBH '卖家角色编号',Y_YSYDDBH '原始预订单号',Y_YSYDDDLYX '买家登录邮箱',Y_YSYDDJSZHLX '买家结算账户类型',Y_YSYDDMJJSBH '买家角色编号' from  dbo.AAA_ZBDBXXB where Number=@znum and T_YSTBDDLYX=@dlyx and Y_YSYDDDLYX=@dlyx ";

            DataTable dataTableMMSYJ = new DataTable();
            Hashtable rezc = I_DBL.RunParam_SQL(strMMSYJ, "buysell", param);
            if ((bool)rezc["return_float"])
            {
                dataTableMMSYJ = ((DataSet)rezc["return_ds"]).Tables["buysell"];
            }    
                      
            //当前是卖家
            string strDQSSeller = "select Number,isnull(T_MJSFQDCNS,'') '卖家是否签订承诺书',T_MJCNSQDSJ '卖家承诺书签订时间',T_YSTBDBH '原始投标单号',T_YSTBDDLYX '卖家登录邮箱',T_YSTBDJSZHLX '卖家结算账户类型',T_YSTBDMJJSBH '卖家角色编号' from  dbo.AAA_ZBDBXXB where Number=@znum and T_YSTBDDLYX=@dlyx ";

            DataTable dataTableDQSSeller = new DataTable();
            Hashtable resell = I_DBL.RunParam_SQL(strDQSSeller, "sell", param);
            if ((bool)resell["return_float"])
            {
                dataTableDQSSeller = ((DataSet)resell["return_ds"]).Tables["sell"];
            }    
           
            //当前是买家
            string strDQSBuyer = "select Number,isnull(Y_MJSFQDCNS,'') '买家是否签订承诺书',Y_MJCNSQDSJ '买家承诺书签订时间',Y_YSYDDBH '原始预订单号',Y_YSYDDDLYX '买家登录邮箱',Y_YSYDDJSZHLX '买家结算账户类型',Y_YSYDDMJJSBH '买家角色编号'  from  dbo.AAA_ZBDBXXB where Number=@znum and Y_YSYDDDLYX=@dlyx ";

            DataTable dataTableDQSBuyer = new DataTable();
            Hashtable rebuy = I_DBL.RunParam_SQL(strDQSBuyer, "buy", param);
            if ((bool)rebuy["return_float"])
            {
                dataTableDQSBuyer = ((DataSet)rebuy["return_ds"]).Tables["buy"];
            }    

           
            string strSFQDCNS = "";//是否签订承诺书
            DateTime dateQDSJ = DateTime.Now;//承诺书签订时间
            if (dataTableMMSYJ != null && dataTableMMSYJ.Rows.Count > 0)  //当前买卖是一家
            {
                strSFQDCNS = dataTableMMSYJ.Rows[0]["卖家是否签订承诺书"].ToString();
                object obj = dataTableMMSYJ.Rows[0]["卖家承诺书签订时间"];
                if (obj.ToString() == "")
                {
                    dateQDSJ = DateTime.Now;
                }
                else
                {
                    dateQDSJ = Convert.ToDateTime(dataTableMMSYJ.Rows[0]["卖家承诺书签订时间"].ToString());
                }
            }
            else if (dataTableDQSSeller != null && dataTableDQSSeller.Rows.Count > 0)  //当前是卖家
            {
                strSFQDCNS = dataTableDQSSeller.Rows[0]["卖家是否签订承诺书"].ToString();
                object obj = dataTableDQSSeller.Rows[0]["卖家承诺书签订时间"];
                if (obj.ToString() == "")
                {
                    dateQDSJ = DateTime.Now;
                }
                else
                {
                    dateQDSJ = Convert.ToDateTime(dataTableDQSSeller.Rows[0]["卖家承诺书签订时间"].ToString());
                }
            }
            else if (dataTableDQSBuyer != null && dataTableDQSBuyer.Rows.Count > 0)//当前是买家
            {
                strSFQDCNS = dataTableDQSBuyer.Rows[0]["买家是否签订承诺书"].ToString();
                object obj = dataTableDQSBuyer.Rows[0]["买家承诺书签订时间"];
                if (obj.ToString() == "")
                {
                    dateQDSJ = DateTime.Now;
                }
                else
                {
                    dateQDSJ = Convert.ToDateTime(dataTableDQSBuyer.Rows[0]["买家承诺书签订时间"].ToString());
                }

            }

            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = strSFQDCNS;
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取数据成功";

            return dsreturn;
 
        }

        catch
        {
            return null;
        }
                
    }


    /// <summary>
    /// 交易账户签订承诺书
    /// </summary>
    /// <param name="dataTable"></param>
    /// <returns></returns>
    public DataSet JYZHQDCNS( DataTable dataTable)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable param = new Hashtable();

        param.Add("@znum", dataTable.Rows[0]["中标定标_Number"].ToString());
        param.Add("@dlyx", dataTable.Rows[0]["登录邮箱"].ToString());

        //当前买卖是一家
        string strMMSYJ = "select Number,isnull(T_MJSFQDCNS,'') '卖家是否签订承诺书',isnull(T_MJCNSQDSJ,'') '卖家承诺书签订时间',isnull(Y_MJSFQDCNS,'') '买家是否签订承诺书',isnull(Y_MJCNSQDSJ,'') '买家承诺书签订时间',T_YSTBDBH '原始投标单号',T_YSTBDDLYX '卖家登录邮箱',T_YSTBDJSZHLX '卖家结算账户类型',T_YSTBDMJJSBH '卖家角色编号',Y_YSYDDBH '原始预订单号',Y_YSYDDDLYX '买家登录邮箱',Y_YSYDDJSZHLX '买家结算账户类型',Y_YSYDDMJJSBH '买家角色编号' from  dbo.AAA_ZBDBXXB where Number=@znum and T_YSTBDDLYX=@dlyx and Y_YSYDDDLYX=@dlyx ";

        DataTable dataTableMMSYJ = new DataTable();
        Hashtable rezc = I_DBL.RunParam_SQL(strMMSYJ, "buysell", param);
        if ((bool)rezc["return_float"])
        {
            dataTableMMSYJ = ((DataSet)rezc["return_ds"]).Tables["buysell"];
        }            
      
        //当前是卖家
        string strDQSSeller = "select Number,isnull(T_MJSFQDCNS,'') '卖家是否签订承诺书',isnull(T_MJCNSQDSJ,'') '卖家承诺书签订时间',T_YSTBDBH '原始投标单号',T_YSTBDDLYX '卖家登录邮箱',T_YSTBDJSZHLX '卖家结算账户类型',T_YSTBDMJJSBH '卖家角色编号' from  dbo.AAA_ZBDBXXB where Number=@znum and T_YSTBDDLYX=@dlyx ";

        DataTable dataTableDQSSeller = new DataTable();
        Hashtable resell = I_DBL.RunParam_SQL(strDQSSeller, "sell", param);
        if ((bool)resell["return_float"])
        {
            dataTableDQSSeller = ((DataSet)resell["return_ds"]).Tables["sell"];
        }    
                
        //当前是买家
        string strDQSBuyer = "select Number,isnull(Y_MJSFQDCNS,'') '买家是否签订承诺书',isnull(Y_MJCNSQDSJ,'') '买家承诺书签订时间',Y_YSYDDBH '原始预订单号',Y_YSYDDDLYX '买家登录邮箱',Y_YSYDDJSZHLX '买家结算账户类型',Y_YSYDDMJJSBH '买家角色编号'  from  dbo.AAA_ZBDBXXB where Number=@znum and Y_YSYDDDLYX=@dlyx ";

        DataTable dataTableDQSBuyer = new DataTable();
        Hashtable rebuy = I_DBL.RunParam_SQL(strDQSBuyer, "buy", param);
        if ((bool)rebuy["return_float"])
        {
            dataTableDQSBuyer = ((DataSet)rebuy["return_ds"]).Tables["buy"];
        }    
              

        bool b = false;
        if (dataTableMMSYJ != null && dataTableMMSYJ.Rows.Count > 0)  //当前买卖是一家
        {
            string buysell = "update AAA_ZBDBXXB set T_MJSFQDCNS='已签',T_MJCNSQDSJ='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',Y_MJSFQDCNS='已签',Y_MJCNSQDSJ='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where Number='" + dataTable.Rows[0]["中标定标_Number"].ToString() + "' and T_YSTBDDLYX=@dlyx and Y_YSYDDDLYX=@dlyx ";
            
            Hashtable ht= I_DBL.RunParam_SQL(buysell, param);

            b = (int)ht["return_other"] > 0;
        }
        else if (dataTableDQSSeller != null && dataTableDQSSeller.Rows.Count > 0)  //当前是卖家
        {
            string sell = "update AAA_ZBDBXXB set T_MJSFQDCNS='已签',T_MJCNSQDSJ='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where Number=@znum and T_YSTBDDLYX=@dlyx";

            Hashtable ht = I_DBL.RunParam_SQL(sell, param);

            b = (int)ht["return_other"] > 0;
           
        }
        else if (dataTableDQSBuyer != null && dataTableDQSBuyer.Rows.Count > 0)//当前是买家
        {
          
            string buy = "update AAA_ZBDBXXB set Y_MJSFQDCNS='已签',Y_MJCNSQDSJ='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where Number=@znum and Y_YSYDDDLYX=@dlyx";
            Hashtable ht = I_DBL.RunParam_SQL(buy, param);

            b = (int)ht["return_other"] > 0;

        }
        if (b == true)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "成功签订承诺书！";
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "承诺书签订失败！";
        }

        return dsreturn;

    }



}