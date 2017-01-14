using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using FMDBHelperClass;
using FMipcClass;
using System.Collections;
using System.Data;

[WebService(Namespace = "http://analysis.ipc.com/", Description = "V1.00->数据分析中心业务类接口")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]

public class analysis : System.Web.Services.WebService
{
    public analysis()
    {

        //如果使用设计的组件，请取消注释以下行
        //InitializeComponent(); 
    }

    /// <summary>
    /// 测试该接口是否还活着(每个接口必备)
    /// </summary>
    /// <param name="temp">随便传</param>
    /// <returns>返回ok就是接口正常</returns>
    [WebMethod(Description = "测试该接口是否还活着(每个接口必备)")]
    public string onlinetest(string temp)
    {
        //根据不同的传入值，后续可以检查不同的东西，比如这个接口所连接的数据库，比如进程池，服务器空间等等。。。
        return "ok";
    }
    /// <summary>
    ///  获得大盘底部统计分析，直接从压缩文件中读取
    /// </summary>
    /// <param name="cs">此参数无效，只是为了满足格式要求</param>
    /// <returns>多维数组</returns>
    [WebMethod(MessageName = "底部统计", Description = "大盘-底部统计")]
    public string[][] indexTJSJ_SJFX_SJFX(string cs)
    {
        string Lujing = Context.Request.MapPath(@"~/bytefiles/HomePageByte_tongji.txt");
        HomePage HP = new HomePage();
        return HP.indexTJSJ_Run(Lujing);
    }
    /// <summary>
    /// 获得一个数据集，首页列表显示
    /// </summary>
    /// <param name="sp">"默认"||"默认"+行数，“完整列表”，"指定商品**!!"+商品编号+"*"+合同周期，"自选**!!"+登陆邮箱，"二级分类**!!"+AAA_PTSPXXB的SortID【参数形式为固定值+后缀，其中后缀和固定值的排列顺序不限；固定值："默认"，“完整列表”，"指定商品**!!"，"自选**!!"，"二级分类**!!"】</param>
    /// <returns></returns>
    [WebMethod(MessageName = "列表加载返回DataSet", Description = "大盘-列表加载返回DataSet")]
    public DataSet test_ds_Run_SJFX(string sp, string lujing)
    {        
        return new HomePage().test_ds_Run(sp, lujing);
    }
    /// <summary>
    /// 获得一个数据集，首页列表显示
    /// </summary>
    /// <param name="sp">"默认"||"默认"+行数，“完整列表”，"指定商品**!!"+商品编号+"*"+合同周期，"自选**!!"+登陆邮箱，"二级分类**!!"+AAA_PTSPXXB的SortID【参数形式为固定值+后缀，其中后缀和固定值的排列顺序不限；固定值："默认"，“完整列表”，"指定商品**!!"，"自选**!!"，"二级分类**!!"】</param>
    /// <returns></returns>
    [WebMethod(MessageName = "列表加载", Description = "大盘-列表加载")]
    public byte[] test_ds_SJFX(string sp)
    {
        HomePage HP = new HomePage();
        string lujing = Context.Request.MapPath(@"~/bytefiles/HomePageSQLlist.txt");
        return Helper.DataSet2Byte(HP.test_ds_Run(sp, lujing));
    }
    /// <summary>
    /// 获得成交信息数据集，首页列表显示
    /// </summary>
    /// <param name="cs">此参数无效，只是为了满足格式要求</param>
    /// <returns></returns>
    [WebMethod(MessageName = "成交详情", Description = "大盘-成交详情")]
    public byte[] test_cjxx_SJFX(string cs)
    {
        //解锁
        byte[] b = Helper.DataSet2Byte(Index_SJFX.jhjx_cjxx_XTKZ());
        return b;
    }
    /// <summary>
    /// 得到所有商品分类，有卖家、买家区分
    /// </summary>
    ///<param name="cs">此参数无效，只是为了满足格式要求</param>
    /// <returns></returns>
    [WebMethod(MessageName = "商品分类", Description = "大盘-商品分类")]
    public DataSet GetSPFL_SJFX(string cs)
    {
        return Index_SJFX.GetSPFL_XTKZ(cs);
    }
    /// <summary>
    /// 根据以及分类获取二级商品分类
    /// </summary>
    /// <param name="SortParentID">父分类id</param>
    /// <returns></returns>
    [WebMethod(MessageName = "二级分类", Description = "大盘底部-二级分类")]
    public DataSet GetSPFLerji_SJFX(string SortParentID)
    {

        //执行语句    
        HomePage HP = new HomePage();
        DataSet dsfl = HP.GetSPFLerji_Run(SortParentID);
        return dsfl;
    }
    /// <summary>
    /// 根据表明获取商品分类
    /// </summary>
    /// <param name="sTable">”AAA_tbMenuSPFL“：平台商品分类表</param>
    /// <returns></returns>
    [WebMethod(MessageName = "一级分类", Description = "大盘底部-一级分类")]
    public DataSet test_dsfl_SJFX(string sTable)
    {
        return Index_SJFX.test_dsfl(sTable);
    }

        /// <summary>
    /// 商品详情
    /// </summary>
    /// <param name="ds">"商品编号"、“合同期限”</param>
    /// <returns></returns>
     [WebMethod(MessageName = "商品详情页面", Description = "大盘-商品详情")]
    public DataSet SelectSPXQ_SJFX(DataTable ds)
    {
        try
        {
            return  Index_SJFX.SelectSPXQ(ds);
        }
        catch(Exception)
        {
            return null;
        }
    }




     /// <summary>
     /// 商品详情_首次(2104新版)
     /// </summary>
     /// <param name="spbh">商品编号</param>
     /// <param name="htqx">合同期限</param>
     /// <param name="dlyx">登录邮箱</param>
     /// <returns></returns>
     [WebMethod(MessageName = "商品详情首次", Description = "大盘-商品详情_首次(2104新版)")]
     public DataSet SelectSPXQ_sc(string spbh, string htqx, string dlyx)
     {
         try
         {
             return Index_SJFX.SelectSPXQ_sc(spbh, htqx, dlyx);
         }
         catch (Exception)
         {
             return null;
         }
     }

}