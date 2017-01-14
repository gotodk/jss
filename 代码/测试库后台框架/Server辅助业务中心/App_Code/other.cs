using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using FMDBHelperClass;
using FMipcClass;
using System.Collections;
using System.Data;

[WebService(Namespace = "http://other.ipc.com/", Description = "V1.00->辅助业务中心业务接口")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]

public class other : System.Web.Services.WebService
{
    public other()
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
    /// 添加大盘中的自选商品 zhaoxh 2014.5.22
    /// </summary>
    /// <param name="DLYX">登陆用户邮箱</param>
    /// <param name="SPBH">商品编号</param>
    /// <returns>成功，失败信息，NULL</returns>
    [WebMethod(Description = "添加大盘中的自选商品")]
    public string AddCommodity_FZYWZX(string DLYX, string SPBH)
    {
        string msg = "";
        try
        {
            msg = new OtherService().AddCommodity(DLYX, SPBH);
        }
        catch (Exception)
        {
            msg = null;
        }


        return msg;
    }

    /// <summary>
    /// 删除大盘中的自选商品 zhaoxh 2014.5.22
    /// </summary>
    /// <param name="DLYX">登陆用户邮箱</param>
    /// <param name="SPBH">商品编号</param>
    /// <returns>成功/失败信息/null：报错</returns>
    [WebMethod(Description = "删除大盘中的自选商品")]
    public string DelCommodity_FZYWZX(string DLYX, string SPBH)
    {
        string msg = "";
        try
        {
            msg = new OtherService().DelCommodity(DLYX, SPBH);
        }
        catch (Exception)
        {
            msg = null;
        }


        return msg;
    }

    /// <summary>
    /// 判断商品是否进入冷静期 zhaoxh 2014.5.22
    /// </summary>
    /// <param name="spbh">商品编号</param>
    /// <param name="htqx">合同期限（三个月/一年）</param>
    /// <returns>是：在冷静期/否：不在冷静期/null：报错</returns>
    [WebMethod(Description = "判断商品是否进入冷静期")]
    public string IsLJQ_FZYWZX(string spbh, string htqx)
    {
        string msg = "";
        try
        {
            msg = new OtherService().IsLJQ(spbh, htqx);
        }
        catch (Exception)
        {
            msg = null;
        }
        return msg;
    }

    /// <summary>
    /// 判断商品是否有效 zhaoxh 2014.5.22
    /// </summary>
    /// <param name="spbh">商品编号</param>
    /// <returns>是：有效/否：无效/null：报错</returns>
    [WebMethod(Description = "判断商品是否有效")]
    public string IsSpValid_FZYWZX(string spbh)
    {
        string msg = "";
        try
        {
            msg = new OtherService().IsSpValid(spbh);
        }
        catch (Exception)
        {
            msg = null;
        }
        return msg;
    }

    /// <summary>
    /// 判断履约保证金是否满足比率 zhaoxh 2014.5.23
    /// </summary>
    /// <param name="LYBZJBZBL">履约保证金比率</param>
    /// <param name="HTBH">合同编号</param>
    /// <returns>是：满足比率/否：不足比率/null：报错</returns>
    [WebMethod(Description = "判断履约保证金是否满足比率")]
    public string BZJSFBZBL_FZYWZX(string LYBZJBZBL,string HTBH)
    {
        string msg = "";
        try
        {
            msg = new OtherService().BZJSFBZBL(LYBZJBZBL, HTBH);
        }
        catch (Exception)
        {
            msg = null;
        }
        return msg;
    }

    /// <summary>
    /// 获取买家最近一次发票信息 zhaoxh 2014.5.23
    /// </summary>
    /// <param name="DLYX">登录邮箱</param>
    /// <returns>dataset结果集/null:报错</returns>
    [WebMethod(Description = "获取买家最近一次发票信息,返回：包含表明为：“FPXX”的Datatable的Dataset ")]
    public DataSet GetFPXX_FZYWZX(string DLYX)
    {
        DataSet rtds = new DataSet();
        try
        {
            rtds = new OtherService().GetFPXX(DLYX);
        }
        catch (Exception)
        {
            rtds = null;
        }
        return rtds;
    }


    /// <summary>
    /// 获取省市区信息 wyh 2014.06.03 add 获取是省市区基本信息 
    /// </summary>
    /// <returns>包含省市区信息的Dataset</returns>
    [WebMethod(Description = "获取省市区信息，返回包含省市区信息的Dataset")]
    public DataSet GetSSQXX_FZYWZX(string a)
    {
        try
        {
            string aa = "";
            return new OtherService().dsSSQ();
        }
        catch
        {
           return null ;
        }
    }

    /// <summary>
    /// 获取分公司对照表信息 wyh 2014.06.03 add
    /// </summary>
    /// <returns>包含分公司对照表信息的Dataset</returns>
    [WebMethod(Description = "获取分公司对照表信息,返回包含分公司对照表信息的Dataset")]
    public DataSet GetFGSDZBXX_FZYWZX(string a )
    {
        try
        {
            return new OtherService().dsFGSdzb();
        }
        catch
        {
            return null;
        }
    }


    /// <summary>
    /// 获取分公司信息包含：公司Number', '分公司', '办事处' wyh 2014.06.03 add
    /// </summary>
    /// <returns>返回包含：公司Number', '分公司', '办事处'的Dataset</returns>
     [WebMethod(Description = "获取分公司信息包含：公司Number', '分公司', '办事处' 三个字段")]
    public DataSet GetFGSBXX_FZYWZX(string a )
    {
        try
        {
            return new OtherService().dsFGS();
        }
        catch
        {
            return null;
        }
    }

     /// <summary>
     /// 获取用户自选商品的编号列表
     /// </summary>
     /// 
     /// <returns>返回包含：自选商品列表列</returns>
     [WebMethod(MessageName = "自选商品临时列表", Description = "获取用户自选商品的编号列表")]
     public DataSet GetZXSP_temp(string dlyx)
     {
         try
         {
             return new OtherService().dsZXSPtemp(dlyx);
         }
         catch
         {
             return null;
         }
     }


     /// <summary>
     /// 获取高校院系信息 wyh 2014.06.03 add
    /// </summary>
     /// <returns>获取高校院系信息</returns>
     [WebMethod(Description = "获取高校院系信息Dataset-----作废")]
     public DataSet GetYXBXX_FZYWZX(string a)
     {
         try
         {
             return new OtherService().GetYXBXX();
         }
         catch
         {
             return null;
         }
     }

    /// <summary>
     /// 获取最新动态参数表 wyh 2014.06.03 add
    /// </summary>
    /// <returns>包含最新动态参数表的Dataset</returns>
     [WebMethod(Description = "获取包含最新动态参数表的Dataset")]
     public DataSet GetDTCSBXX_FZYWZX(string a)
     {
         try
         {
             return new OtherService().GetDTCSBXX();
         }
         catch
         {
             return null;
         }
     }
      
    /// <summary>
    /// 根据省市获取平台管理机构 wyh 2014.06.03 
    /// </summary>
    /// <param name="hashTable">包含省市信息的Dataset[省份][地区]</param>
    /// <returns>返回平台管理机构</returns>
     [WebMethod(Description = "根据省市信息获取平台管理机构，用于用户在注册时选择省市以后系统自动带出平台管理机构")]
     public string GetPTGLJG_FZYWZX(DataSet hashTable)
     {
         try
         {
             return new OtherService().GetPTGLJG(hashTable);
         }
         catch
         {
             return null;
         }
     }

    /// <summary>
    /// 2014-06-16 shiyan
    /// 根据登录邮箱获取C区上部的余额、冻结、收益数据
    /// </summary>
    /// <param name="dlyx">登录邮箱</param>
    /// <param name="type">交易方类型</param>
    /// <returns>结果数据集</returns>
     [WebMethod(MessageName = "获取C区统计数据", Description = "获取c区上余额、冻结、收益的数值")]
     public DataSet GetCTongJi(string dlyx, string type)
     {
         OtherService ots = new OtherService();
         DataSet ds = ots.GetCTongJi(dlyx, type);
         return ds;

     }

     [WebMethod(MessageName = "获取C区资金项目性质", Description = "获取资金转账C区中项目、性质下拉框对应的内容")]
     public DataSet GetCZiJinXM(string dlyx, string type)
     {
         OtherService ots = new OtherService();
         DataSet ds = ots.GetCZiJinXM(dlyx, type);
         return ds;

     }
        
     /// <summary>
     /// 提交开票信息
     /// </summary>
     /// <param name="dt">参数列表</param> 
     /// <returns>返回提交开票信息的结果</returns>
     [WebMethod(Description = "返回提交开票信息的结果")]
     public DataSet CommitKPXX_FZYWZX(DataTable dt)
     {
         //初始化返回值,先塞一行数据
         DataSet dsreturn = initReturnDataSet().Clone();
         dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });


         //开始执行     
         OtherService thd = new OtherService();
         Hashtable htres = thd.CommitKPXX(dt);
         dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = htres["执行结果"].ToString();
         dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = htres["提示文字"].ToString();


         return dsreturn;
     }

     /// <summary>
     /// 变更开票信息
     /// </summary>
     /// <param name="dt">参数列表</param> 
     /// <returns>返回提交开票信息变更数据的执行结果</returns>
     [WebMethod(Description = "返回提交开票信息变更数据的执行结果")]
     public DataSet CommitKPXXChange_FZYWZX(DataTable dt)
     {
         //初始化返回值,先塞一行数据
         DataSet dsreturn = initReturnDataSet().Clone();
         dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });


         //开始执行     
         OtherService thd = new OtherService();
         Hashtable htres = thd.CommitKPXXChange(dt);
         dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = htres["执行结果"].ToString();
         dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = htres["提示文字"].ToString();


         return dsreturn;
     }

     /// <summary>
     /// 返回开票信息的数据内容
     /// </summary>
     /// <param name="dlyx">登陆邮箱</param> 
     /// <returns>返回获取的开票信息</returns>
     [WebMethod(Description = "返回获取的开票信息")]
     public DataSet GetKPXX_FZYWZX(string dlyx)
     {
         //初始化返回值,先塞一行数据
         DataSet dsreturn = initReturnDataSet().Clone();
         dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
         
         //开始执行     
         OtherService thd = new OtherService();
         Hashtable htres = thd.GetKPXXinfo(dlyx);
         dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = htres["执行结果"].ToString();
         dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = htres["提示文字"].ToString();
         DataTable dtinfo = (DataTable)htres["开票信息"];
         dtinfo.TableName = "开票信息";
         dsreturn.Tables.Add(dtinfo.Copy());
         DataTable dtzcxx = (DataTable)htres["注册信息"];
         dtzcxx.TableName = "注册信息";
         dsreturn.Tables.Add(dtzcxx.Copy());

         return dsreturn;
     }

     /// <summary>
     /// 判断身份证号是否已经被注册
     /// </summary>
     /// <param name="strZGZS">类型</param>
     /// <param name="strSFZH">身份证号</param>
     /// <returns></returns>
     /// edit by zzf 2014.7.11
     [WebMethod(Description = "判断身份证号是否已经被注册")]
     public DataSet JudgeSFZHXX_FZYWZX(string strZHLX, string strSFZH)
     {
         //初始化返回值,先塞一行数据
         DataSet dsreturn = initReturnDataSet().Clone();
         dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
         //开始执行     
         OtherService dc = new OtherService();
         dsreturn = dc.JudgeSFZHXX(strZHLX, strSFZH);
         return dsreturn;
     }

     /// <summary>
     /// 判断是否签订承诺书
     /// </summary> 
     /// <param name="dataTable"></param>
     /// <returns></returns>
     /// edit by zzf 2014.7.14
     [WebMethod(Description = "判断是否签订承诺书")]
     public DataSet JudgeSFQDCNS_FZYWZX(DataTable dt)
     {
         DataSet ds=null;
         if (dt != null)
         {
             OtherService dc = new OtherService();
             ds = dc.JudgeSFQDCNS(dt);
         }
         return ds;
 
     }

      /// <summary>
    /// 交易账户签订承诺书
    /// </summary>   
    /// <param name="dataTable"></param>
    /// <returns></returns>
    /// edit by zzf 2014.7.14
     [WebMethod(Description = "交易账户签订承诺书")]
     public DataSet JYZHQDCNS_FZYWZX(DataTable dt)
     {
         try
         {
             DataSet ds = null;
             if (dt != null)
             {
                 OtherService dc = new OtherService();
                 ds = dc.JYZHQDCNS(dt);
             }
             return ds;
         }
         catch
         {
             return null;
         }
 
     }


}