using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data.SqlClient;
using System.Web.Services;
using System.Threading;
using System.Text;
using System.IO;
using FMDBHelperClass;
 
using RedisReLoad.ShareDBcache;

/// <summary>
///HomePage 的摘要说明
/// </summary>
public class HomePage
{
	public HomePage()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}
   
    /// <summary>
    /// 测试获得一个数据集，模拟首页列表显示
    /// </summary>
    /// <returns></returns>
    public DataSet test_ds_Run(string sp, string lujing)
    {
        try
        {
            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
            Hashtable htCS = new Hashtable();
            string sql_zixuan = "";
            string sql_zhidingfenlei = "";
            string topstr = "";
            if (sp.IndexOf("默认") >= 0)
            {
                string shuliang = sp.Replace("默认", "");
                if (shuliang.Trim() != "")
                {
                    topstr = "  top " + shuliang + "  ";
                }
                else
                {
                    topstr = " ";
                }
                sql_zixuan = " where 1=1 and  排序优先级 < 4 ";
            }

            if (sp.IndexOf("完整列表") >= 0)
            {
                sql_zixuan = " where 1=1  ";
            }

            if (sp.IndexOf("指定商品**!!") >= 0)
            {
                string[] arr = sp.Replace("指定商品**!!", "").Split('*');
                //sql_zixuan = "  where 商品编号 = ''" + arr[0] + "'' and  合同周期n = ''" + arr[1] + "'' ";
                sql_zixuan = "  where 商品编号 = @商品编号 and  合同周期n = @合同周期n ";
                htCS["@商品编号"] = arr[0];
                htCS["@合同周期n"] = arr[1];
            }
            if (sp.IndexOf("自选**!!") >= 0)
            {
                //sql_zixuan = "  where 商品编号 COLLATE DATABASE_DEFAULT in (select AAA_ZXSPJLB.SPBH COLLATE DATABASE_DEFAULT from AAA_ZXSPJLB left join AAA_PTSPXXB on AAA_ZXSPJLB.spbh=AAA_PTSPXXB.spbh  where DLYX = ''" + sp.Replace("自选**!!", "") + "'' AND AAA_PTSPXXB.SFYX=''是'')  ";
                sql_zixuan = "  where 商品编号 in (select AAA_ZXSPJLB.SPBH from AAA_ZXSPJLB left join AAA_PTSPXXB on AAA_ZXSPJLB.spbh=AAA_PTSPXXB.spbh  where DLYX = @DLYX AND AAA_PTSPXXB.SFYX='是')  ";
                htCS["@DLYX"] = sp.Replace("自选**!!", "");
            }

            if (sp.IndexOf("二级分类**!!") >= 0)
            {
                string SortID = sp.Replace("二级分类**!!", "");
                //sql_zhidingfenlei = "  where 商品编号 COLLATE DATABASE_DEFAULT in (select SPBH COLLATE DATABASE_DEFAULT from AAA_PTSPXXB where SSFLBH in (select SortID from AAA_tbMenuSPFL where SortParentPath like ''%," + SortID + ",%'' or SortID=" + SortID + ") AND AAA_PTSPXXB.SFYX=''是'') ";
                sql_zixuan = "  where 商品编号  in (select SPBH  from AAA_PTSPXXB where SSFLBH in (select SortID from AAA_tbMenuSPFL where SortParentPath like '%,'+@SortID+',%' or SortID=@SortID) AND AAA_PTSPXXB.SFYX='是') ";
                htCS["@SortID"] = SortID;
            }
            //读取sql,替换变量
            string SQL = File.ReadAllText(lujing);
            SQL = SQL.Replace("[[toptop]]", topstr).Replace("[[shangpin]]", sql_zixuan);

            //DataSet ds = DbHelperSQL.Query(SQL);
            
            Hashtable ht = I_DBL.RunParam_SQL(SQL, "",htCS);
            if (!(bool)ht["return_float"])
            {
                return null;
            }
            return (DataSet)ht["return_ds"];
        }
        catch(Exception)
        {
            return null;
        }


    }


    /// <summary>
    /// 获得一个多维数组，首页下方数据显示
    /// </summary>
    /// <returns></returns>
    public string[][] indexTJSJ_Run(string Lujing)
    {
        /*
        //数据格式需拼接处理为一个多维数组，前台将按照顺序显示出来。例如：
        string[] test1 = new string[] { "商品种类数", "4273", "买家数量", "78780", "卖家数量", "1474", "预订单金额", "78736.98元", "买家区域覆盖率", "74%", "卖家区域覆盖率", "47%" };
        string[] test2 = new string[] { "商品种类数", "273", "买家数量", "4780", "卖家数量", "764", "预订单金额", "2736.98元", "定标商品数量", "784", "定标金额", "15110.70", "下达提货单金额", "44110.25", "买家区域覆盖率", "14%", "卖家区域覆盖率", "22%" };
        string[] test3 = new string[] { "商品种类数", "273", "买家数量", "4780", "卖家数量", "764", "预订单金额", "2736.98元", "定标商品数量", "784", "定标金额", "15110.70", "下达提货单金额", "44110.25", "买家区域覆盖率", "14%", "卖家区域覆盖率", "22%" };
        string[][] returndb = new string[][] { test1, test2, test3 };
        */
        try
        {
            //FileStream fs = new FileStream(Lujing, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            //byte[] data = new byte[fs.Length];
            //fs.Read(data, 0, data.Length);
            //fs.Close();

            //只从缓存获取数据
            byte[] data = HomePageRedis.GetHomePageByte_tongji();
            if (data == null)
            {
                return null;
            }
                 

            DataSet ds = Helper.Byte2DataSet(data);
            //string SQL = File.ReadAllText(Lujing);
            //DataSet ds =  DbHelperSQL.Query(SQL);

            ArrayList al1 = new ArrayList();
            ArrayList al2 = new ArrayList();
            ArrayList al3 = new ArrayList();


            //生成多维数组
            for (int p = 0; p < ds.Tables[0].Columns.Count; p++)
            {
                //不是列分割
                if (ds.Tables[0].Columns[p].ColumnName.IndexOf("_列分割") < 0)
                {
                    string thisLieMing = ds.Tables[0].Columns[p].ColumnName;
                    if (thisLieMing.IndexOf("当前交易统计_") >= 0)
                    {
                        al1.Add(thisLieMing.Replace("当前交易统计_", ""));
                        al1.Add(ds.Tables[0].Rows[0][p].ToString());
                    }
                    if (thisLieMing.IndexOf("今日新增统计_") >= 0)
                    {
                        al2.Add(thisLieMing.Replace("今日新增统计_", ""));
                        al2.Add(ds.Tables[0].Rows[0][p].ToString());
                    }
                    if (thisLieMing.IndexOf("今年累计统计_") >= 0)
                    {
                        al3.Add(thisLieMing.Replace("今年累计统计_", ""));
                        al3.Add(ds.Tables[0].Rows[0][p].ToString());
                    }
                }
            }

            string[] IResult1 = (string[])al1.ToArray(typeof(string));
            string[] IResult2 = (string[])al2.ToArray(typeof(string));
            string[] IResult3 = (string[])al3.ToArray(typeof(string));

            string[] IResult_For_Time = new string[] { "当前服务器时间", System.DateTime.Now.ToString() };

            string[][] returndb = new string[][] { IResult1, IResult2, IResult3, IResult_For_Time };

            return returndb;

        }
        catch (Exception ex)
        {
            return null;
        }

     
    }

    /// <summary>
    /// 根据以及分类获取二级商品分类
    /// </summary>
    /// <param name="SortParentID">父分类id</param>
    /// <returns></returns>
    public DataSet GetSPFLerji_Run(string SortParentID)
    {
        try
        {
            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
            Hashtable htCS = new Hashtable();
            htCS["@SortParentID"] = SortParentID;
            Hashtable ht = I_DBL.RunParam_SQL("select top 10 * from AAA_tbMenuSPFL where SortParentID=@SortParentID order by SortOrder asc", "", htCS);
            if (!(bool)ht["return_float"])
            {
                return null;
            }
            DataSet dsfl = (DataSet)ht["return_ds"];
            return dsfl;
        }
        catch (Exception)
        {
            return null;
        }
    }

    #region//2014.05.26--周丽作废--暂时用不到

    ///// <summary>
    ///// 加入或删除自选商品
    ///// </summary>
    ///// <param name="DLYX"></param>
    ///// <param name="SPBH"></param>
    ///// <returns></returns>
    //public string ZXSPedit_Run(string DLYX, string SPBH,string edit)
    //{
    //    if (edit == "加入")
    //    {
    //        DataSet ds = DbHelperSQL.Query("select count(number) from AAA_ZXSPJLB where DLYX = '" + DLYX + "' and SPBH = '" + SPBH + "'");
    //        if (ds != null && ds.Tables[0].Rows[0][0].ToString() == "0") //不存在
    //        {
    //            string Number = jhjx_PublicClass.GetNextNumberZZ("AAA_ZXSPJLB", "");
    //            string CreateTime = DateTime.Now.ToString();
    //            DbHelperSQL.ExecuteSql("INSERT INTO AAA_ZXSPJLB ([Number],[DLYX],[SPBH],[SCSJ]) VALUES ('" + Number + "','" + DLYX + "','" + SPBH + "','" + CreateTime + "')");
    //        }
    //        return "成功添加到自选商品!";
    //    }
    //    if (edit == "删除")
    //    {
    //        DbHelperSQL.ExecuteSql(" delete AAA_ZXSPJLB where DLYX = '" + DLYX + "' and SPBH = '" + SPBH + "' ");
    //        return "成功从自选商品中删除!";
    //    }
    //    return "";
    //}


  


    ///// <summary>
    ///// 为每组数据处理大盘统计 by 于海滨
    ///// </summary>
    ///// <param name="dsreturn">返回值</param>
    ///// <param name="SQLdapan">大盘sql读取路径</param>
    ///// <param name="SQLdibu">底部统计sql读取路径</param>
    ///// <returns></returns>
    //public DataSet CreatNewDapanList_Group_Run(DataSet dsreturn, string SQLdapan_lujing, string SQLdibu_lujing)
    //{
  
    //    try
    //    {

    //        string showtime = "";
    //        showtime = showtime + DateTime.Now + "_0_\n\r";
    //        //从“大盘数据变化时间对照表”中，获取“最后一次有效数据变更时间”晚于等于“最后一次执行统计时间”的数据
    //        //最先删除临时表和时间对照表中的已撤销的商品，再处理商品表中有，但时间对照表中没有的，插入。   再更新符合条件的预存时间。  最后找符合条件的。 
    //        string SQL_find = "";
    //        SQL_find = SQL_find + "  delete AAA_DaPanTJTime where SPBH in (  select SPBH from   AAA_PTSPXXB where SFYX = '否' )  ; ";
    //        SQL_find = SQL_find + "  delete AAA_DaPanPrd_New where 商品编号 in (  select SPBH from   AAA_PTSPXXB where SFYX = '否' )  ; ";
    //        SQL_find = SQL_find + "  insert into AAA_DaPanTJTime (SPBH) (select S.SPBH from   AAA_PTSPXXB as S left join AAA_DaPanTJTime as D on  S.SPBH = D.SPBH where D.LastTJtime is null and  S.SFYX = '是' )   ;   ";
    //        SQL_find = SQL_find + "    update AAA_DaPanTJTime set LastTJtime_YC = getdate()  where LastYXBGtime>=LastTJtime  ; ";
    //        SQL_find = SQL_find + "  select SPBH,LastTJtime,LastYXBGtime from AAA_DaPanTJTime where LastYXBGtime>=LastTJtime  order by LastYXBGtime,SPBH desc ;  ";

    //        DataSet dsBG = DbHelperSQL.Query(SQL_find);

    //        //为分组好的数据，开启独立线程进行运算
    //        int groupNum = 10; //每组N个商品
    //        int groupMaxThreads = 100; //最大并发线程。
    //        int groupnow = 1; //当前第几组
    //        int spbhNum = dsBG.Tables[0].Rows.Count; //商品编号一共几个
    //        string GroupStr = ""; //某组的字符串
    //        ThreadPool.SetMaxThreads(groupMaxThreads, groupMaxThreads);//设置线程池
    //        htThreadRe = new Hashtable(); //滞空线程标志
    //        for (int p = 0; p < spbhNum; p++)
    //        {
                
    //            //分组

    //            GroupStr = GroupStr + dsBG.Tables[0].Rows[p]["SPBH"].ToString() + "|";
    //            if (spbhNum == p + 1 || p == groupNum * groupnow - 1)
    //            {
    //                GroupStr = GroupStr.Substring(0, GroupStr.Length - 1);
    //                ThreadPool.QueueUserWorkItem(BeginDapanOneGroup, GroupStr);
    //                GroupStr = "";
    //                //更新组数量
    //                groupnow++;
    //            }



    //        }


    //        //确定所有线程都跑完后，执行底部统计和压缩
    //        while (spbhNum != 0 && htThreadRe.Count != (groupnow-1))
    //        {
    //            Thread.Sleep(50);
    //        }


    //        /*
    //        //临时调试
    //        string jieguo  = "";
    //        foreach (DictionaryEntry de in htThreadRe)
    //        {
    //            jieguo = jieguo + de.Key + "<br />";
    //        }
             
    //        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
    //        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = jieguo;
    //        return dsreturn;
    //        */

    //        //检查是否有执行错误的线程(只记录最后一组线程的错误)
    //        string TreadErr = ""; 
    //        foreach (DictionaryEntry de in htThreadRe)
    //        {
    //            if (de.Value.ToString().IndexOf("err_") >= 0)
    //            {
    //                TreadErr = de.Value.ToString();
    //            }
    //        }

 


    //        //生成新的大盘二进制压缩文件(包括底部统计分析)
    //        //得到大盘二进制数据
    //        HomePage HP = new HomePage();

    //        DataSet dslist = HP.test_ds_Run("默认", SQLdapan_lujing);
    //        byte[] b = Helper.DataSet2Byte(dslist);
    //        //生成大盘二进制文件
    //        FileStream fs = new FileStream(SQLdapan_lujing.Replace("HomePageSQLlist.txt", "HomePageByte.txt"), FileMode.OpenOrCreate);
    //        fs.Write(b, 0, b.Length);
    //        fs.Flush();
    //        fs.Close();

    //        showtime = showtime + DateTime.Now + "_1_\n\r";

    //        //得到商品基本数据，用于键盘精灵 
    //        DataSet shangpinS = HP.test_ds_Run("完整列表", SQLdapan_lujing);
    //        DataSet shangpinSnew = new DataSet();
    //        shangpinSnew.Tables.Clear();
    //        shangpinSnew.Tables.Add(shangpinS.Tables[0].DefaultView.ToTable(false, new string[] { "商品编号", "商品名称", "型号规格", "合同周期n" }));
    //        //加载拼音
    //        shangpinSnew.Tables[0].Columns.Add("简拼");
    //        shangpinSnew.Tables[0].Columns.Add("全拼");
    //        for (int p = 0; p < shangpinSnew.Tables[0].Rows.Count; p++)
    //        {
    //            shangpinSnew.Tables[0].Rows[p]["全拼"] = PTHelper.Convert(shangpinSnew.Tables[0].Rows[p]["商品名称"].ToString()).ToLower();
    //            shangpinSnew.Tables[0].Rows[p]["简拼"] = PTHelper.GetHeadLetters(shangpinSnew.Tables[0].Rows[p]["商品名称"].ToString()).ToLower();
    //        }

        

    //        byte[] b_shangpinS = Helper.DataSet2Byte(shangpinSnew);
    //        //生成简化商品二进制文件
    //        fs = new FileStream(SQLdapan_lujing.Replace("HomePageSQLlist.txt", "HomePageByte_jhxp.txt"), FileMode.OpenOrCreate);
    //        fs.Write(b_shangpinS, 0, b_shangpinS.Length);
    //        fs.Flush();
    //        fs.Close();

 
    //        //得到底部统计分析二进制数据
    //        string SQL = File.ReadAllText(SQLdibu_lujing);
    //        DataSet ds = DbHelperSQL.Query(SQL);
    //        byte[] b_tongji = Helper.DataSet2Byte(ds);
    //        //生成底部统计分析二进制文件
    //        fs = new FileStream(SQLdapan_lujing.Replace("HomePageSQLlist.txt", "HomePageByte_tongji.txt"), FileMode.OpenOrCreate);
    //        fs.Write(b_tongji, 0, b_tongji.Length);
    //        fs.Flush();
    //        fs.Close();
    //        showtime = showtime + DateTime.Now + "_h_";
    //        if (TreadErr == "")
    //        {
    //            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
    //            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "大盘共生成" + spbhNum + "个商品。" + groupNum + "个一组，共" + (groupnow - 1).ToString() + "组，最大按组并发" + groupMaxThreads + "个线程。";
    //            //dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "大盘共生成" + spbhNum + "个商品。" + groupNum + "个一组，共" + (groupnow - 1).ToString() + "组，最大按组并发" + groupMaxThreads + "个线程。\n\r" + showtime;
    //        }
    //        else
    //        {
    //            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
    //            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "大盘生成完成但有至少一个错误：" + TreadErr;
    //        }
    //        return dsreturn;
    //    }
    //    catch (Exception ex)
    //    {
    //        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
    //        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = ex.ToString();
    //        return dsreturn;
    //    }



    //}

    //private Hashtable htThreadRe = new Hashtable();

    ///// <summary>
    ///// 开始执行一组的计算 by 于海滨
    ///// </summary>
    ///// <param name="SPBH">有业务变动的商品编号字符串，用竖杠分割</param>
    //private void BeginDapanOneGroup(object SPBH)
    //{
    //    //生成大盘数据，放到临时表
    //    try
    //    {


    //        DbHelperSQL.ExecuteSql(" exec  AAA_DaPanPrd_Get '" + SPBH + "' ");
    //        htThreadRe["组执行结果_" + SPBH + "_" + Guid.NewGuid().ToString()] = "ok_" + DateTime.Now.ToString() + " " + DateTime.Now.Millisecond.ToString();
    //    }
    //    catch (Exception ex)
    //    {
    //        htThreadRe["组执行结果_" + SPBH + "_" + Guid.NewGuid().ToString()] = "err_" + SPBH + "_" + ex.ToString();
    //    }

    //}

    #endregion

}