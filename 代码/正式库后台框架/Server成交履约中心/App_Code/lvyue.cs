using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using FMDBHelperClass;
using FMipcClass;
using System.Collections;
using System.Data;

[WebService(Namespace = "http://lvyue.ipc.com/", Description = "V1.00->成交履约中心业务接口")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]

public class lvyue : System.Web.Services.WebService
{
    public lvyue()
    {

        //如果使用设计的组件，请取消注释以下行
        //InitializeComponent(); 
    }

    /// <summary>
    /// 测试该接口是否还活着(每个接口必备)
    /// </summary>
    /// <param name="temp">随便传</param>
    /// <returns>返回ok就是接口正常</returns>
    [WebMethod( MessageName="测试接口",Description = "测试该接口是否还活着(每个接口必备)")]
    public string onlinetest(string temp)
    {
        //根据不同的传入值，后续可以检查不同的东西，比如这个接口所连接的数据库，比如进程池，服务器空间等等。。。
       return "ok";

    }



    /// <summary>
    /// 得到电子购货合同基本信息---用于替换合同模板部分内容
    /// </summary>
    /// <param name="Number">中标定标信息表Number值</param>
    /// <returns>返回符合dsreturn结构Dataset，同时包含表明为“合同基本资料”的Datatable</returns>
    [WebMethod(MessageName = "电子购货合同基础资料", Description = "得到电子购货合同基本信息---用于替换合同模板部分内容")]
    public DataSet GetHT_jiben(string Number)
    {
        try
        {
            return new lvyueclass().GetHT_jiben(Number);
        }
        catch
        {
            return null;
        }
    }

     /// <summary>
    /// 得到电子购货合同相关的数据信息 wyh 2014.06.04
    /// </summary>
    /// <param name="dataTable">参数集合</param>
    /// <returns>返回符合dsreturn结构的Dataset</returns>
    [WebMethod(MessageName="电子购货合同信息" , Description = "得到电子购货合同相关的数据信息,包含表名为：卖家资质，买家资质，投标单，预订单，投标单资质，提货单发货单，其他资质等7个datatable，返回值为Null时程序出错。")]
    public DataSet GetDZGHHTXX_CJLYZX(DataTable dataTable)
    {
        try
        {
            return new lvyueclass().GetDZGHHTXX(dataTable);
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 得到电子购货合同卖家资质相关信息---卖家资质 wyh 2014.06.04
    /// </summary>
    /// <param name="Number">中标定标信息表Number值</param>
    /// <returns>返回符合dsreturn结构Dataset，同时包含表明为“卖家资质”的Datatable</returns>
    [WebMethod(MessageName = "电子购货合同信息卖家资质", Description = "得到电子购货合同相关的数据信息,包含表名为“卖家资质”的datatable，返回值为Null时程序出错。")]
    public DataSet GetDZGHHTXX_Sellzz_CJLYZX(string Number)
    {
        try
        {
            return new lvyueclass().GetDZGHHTXX_Sellzz(Number);
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 得到电子购货合同卖家资质相关信息---买家资质 wyh 2014.06.04
    /// </summary>
    /// <param name="Number">中标定标信息表Number值</param>
    /// <returns>返回符合dsreturn结构Dataset，同时包含表明为“买家资质”的Datatable</returns>
    [WebMethod(MessageName = "电子购货合同信息买家资质", Description = "得到电子购货合同相关的数据信息,包含表名为“买家资质”的datatable，返回值为Null时程序出错。")]
    public DataSet GetDZGHHTXX_buyerzz_CJLYZX(string Number)
    {
        try
        {
            return new lvyueclass().GetDZGHHTXX_buyerzz(Number);
        }
        catch
        {
            return null;
        }
    }


    /// <summary>
    /// 得到电子购货合同卖家资质相关信息---投标单 wyh 2014.06.04
    /// </summary>
    /// <param name="Number">中标定标信息表Number值</param>
    /// <returns>返回符合dsreturn结构Dataset，同时包含表明为“投标单”的Datatable</returns>
    [WebMethod(MessageName = "电子购货合同信息投标单", Description = "得到电子购货合同相关的数据信息,包含表名为“投标单”的datatable，返回值为Null时程序出错。")]
    public DataSet GetDZGHHTXX_TBD_CJLYZX(string Number)
    {
        try
        {
            return new lvyueclass().GetDZGHHTXX_TBD(Number);
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 得到电子购货合同卖家资质相关信息---预订单 wyh 2014.06.04
    /// </summary>
    /// <param name="Number">中标定标信息表Number值</param>
    /// <returns>返回符合dsreturn结构Dataset，同时包含表明为“预订单”的Datatable</returns>
    [WebMethod(MessageName = "电子购货合同信息预订单", Description = "得到电子购货合同相关的数据信息,包含表名为“预订单”的datatable，返回值为Null时程序出错。")]
    public DataSet GetDZGHHTXX_YDD_CJLYZX(string Number)
    {
        try
        {
            return new lvyueclass().GetDZGHHTXX_YDD(Number);
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 得到电子购货合同卖家资质相关信息---投标单资质 wyh 2014.06.04
    /// </summary>
    /// <param name="Number">中标定标信息表Number值</param>
    /// <returns>返回符合dsreturn结构Dataset，同时包含表明为“投标单资质”的Datatable</returns>
     [WebMethod(MessageName = "电子购货合同信息投标单资质", Description = "得到电子购货合同相关的数据信息,包含表名为“投标单资质”的datatable，返回值为Null时程序出错。")]
    public DataSet GetDZGHHTXX_TBDZZ_CJLYZX(string Number)
    {
        try
        {
            return new lvyueclass().GetDZGHHTXX_TBDZZ(Number);
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 得到电子购货合同卖家资质相关信息---提货单发货单 wyh 2014.06.04
    /// </summary>
    /// <param name="Number">中标定标信息表Number值</param>
    /// <returns>返回符合dsreturn结构Dataset，同时包含表明为“提货单发货单”的Datatable</returns>
     [WebMethod(MessageName = "电子购货合同信息提货单发货单", Description = "得到电子购货合同相关的数据信息,包含表名为“提货单发货单”的datatable，返回值为Null时程序出错。")]
     public DataSet GetDZGHHTXX_THDFHD_CJLYZX(string Number)
     {
         try
         {
             return new lvyueclass().GetDZGHHTXX_THDFHD(Number);
         }
         catch
         {
             return null;
         }
     }


    /// <summary>
    /// 得到电子购货合同卖家资质相关信息---其他资质 wyh 2014.06.04
    /// </summary>
    /// <param name="Number">中标定标信息表Number值</param>
    /// <returns>返回符合dsreturn结构Dataset，同时包含表明为“其他资质”的Datatable</returns>
     [WebMethod(MessageName = "电子购货合同信息其他资质", Description = "得到电子购货合同相关的数据信息,包含表名为“其他资质”的datatable，返回值为Null时程序出错。")]
     public DataSet GetDZGHHTXX_QTZZ_CJLYZX(string Number)
     {
         try
         {
             return new lvyueclass().GetDZGHHTXX_QTZZ(Number);
         }
         catch
         {
              return null;
         }
     }

     /// <summary>
    /// 保存提货单 wyh 2014.06.06
    /// </summary>
    /// <param name="ds">参数集合</param>
    /// <returns>返回符合dsreturn结构的Dataset</returns>
    [WebMethod(MessageName = "保存提货单", Description = "保存提货单,返回符合dsreturn结构的Dataset")]
    public DataSet SaveTHD_CJLYZX(DataSet ds)
    {
        try
        {
            return new THDInfo().SaveTHD(ds);
        }
        catch
        {
            return null;
        }
    }

      /// <summary>
    /// 提货单信息查看 wyh 2014.06.06
    /// </summary>
    /// <param name="THDBH">提货单号</param>
    /// <returns>返回符合dsreturn结构Dataset,包含名为“主表”的Datatable</returns>
    [WebMethod(MessageName = "提货单信息查看", Description = "返回符合dsreturn结构Dataset,包含名为“主表”的Datatable")]
    public DataSet CKTHD_CJLYZX(string THDBH)
    {
        try
        {
            return new THDInfo().CKTHD(THDBH);
        }
        catch
        {
            return null;
        }
    }

     /// <summary>
    /// 查询发货单详情  wyh 2014.06.06
    /// </summary>
    /// <param name="FHDBH">发货单号</param>
    /// <returns>返回符合dsreturn结构Dataset，并且包含名为主表的datatable</returns>
    [WebMethod(MessageName = "发货单信息查看", Description = "返回符合dsreturn结构Dataset，并且包含名为“主表”的datatable")]
    public DataSet CXFHD_CJLYZX(string FHDBH)
    {
        try
        {
            return new FHDInfo().CXFHD(FHDBH);
        }
        catch
        {
            return null;
        }
    }

     /// <summary>
    /// 货物签收 wyh 2014.06.09 
    /// </summary>
    /// <param name="ht">参数集合</param>
    /// <returns>返回符合dsreturn结构的Dataset</returns>
    [WebMethod(MessageName = "货物签收", Description = "返回符合dsreturn结构Dataset,null:程序出现错误")]
    public DataSet dsjhjx_hwqs_CJLYZX(DataTable ht)
    {
        try
        {
            return new HWQSInfo().dsjhjx_hwqs(ht);
        }
        catch
        {
            //插入日志：货物签收程序错误
            return null;
        }
    }


    /// <summary>
    ///  获取签收--无疑义收货 wyh 2014.06.11 add
    /// </summary>
    /// <param name="ds">参数集合</param>
    /// <returns>返回符合dsreturn结构的Dataset</returns>
    [WebMethod(MessageName = "无异议收货", Description = "返回符合dsreturn结构Dataset,null:程序出现错误。此方法适用：无异议收货，补发货物无异议收货,有异议收货后无异议收货 三种类型无疑义收货。")]
    public DataSet wyysh_strs_CJLYZX(DataTable ds)
    {
        try
        {
            DataSet dsreturn =  new HWQSInfo().wyysh_strs(ds);
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "wyy";
            return dsreturn;
        }
        catch
        {
            return null;
        }
    }


    /// <summary>
    /// 请重新发货 wyh 2014.06.11 
    /// </summary>
    /// <param name="ds">参数集合</param>
    /// <returns>返回符合dsretun结构的Dataset</returns>
    [WebMethod(MessageName = "请重新发货", Description = "返回符合dsreturn结构Dataset,null:程序出现错误。")]
    public DataSet qcxsh_strs_CJLYZX(DataTable ds)
    {
        try
        {
             DataSet dsreturn=  new HWQSInfo().qcxsh_strs(ds);
             dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "nowyy";
             return dsreturn;
        }
        catch
        {
            return null;
        }
 
    }

    /// <summary>
    /// 部分收货 wyh 2014.06.11 
    /// </summary>
    /// <param name="ds">参数集合</param>
    /// <returns>返回符合dsreturn结构的dataset</returns>
    [WebMethod(MessageName = "部分收货", Description = "返回符合dsreturn结构Dataset,null:程序出现错误。")]
    public DataSet bufensh_strs_CJLYZX(DataTable ds)
    {
        try
        {
            DataSet dsreturn= new HWQSInfo().bufensh_strs(ds);
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "nowyy";
            return dsreturn;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 有异议收货 wyh 2014.06.11
    /// </summary>
    /// <param name="ds">参数集合</param>
    /// <returns>返回符合dsretrn结构的Dataset</returns>
    [WebMethod(MessageName = "有异议收货", Description = "返回符合dsreturn结构Dataset,null:程序出现错误。")]
    public DataSet wyyth_strs_CJLYZX(DataTable ds)
    {
        try
        {
            DataSet dsreturn = new HWQSInfo().wyyth_strs(ds);
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "nowyy";
            return dsreturn;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 生成发货单 wyh 2014.06.11 
    /// </summary>
    /// <param name="ds">参数集合</param>
    /// <returns>返回符合dsretrn的Dataset</returns>
    [WebMethod(MessageName = "生成发货单", Description = "返回符合dsreturn结构Dataset,null:程序出现错误。")]
    public DataSet SCFHD_CJLYZX(DataSet ds)
    {
        try
        {
            return new FHDInfo().SCFHD(ds);
        }
        catch
        {
            return null;
        }
    }

     /// <summary>
    /// 问题与处理 wyh 2014.06.10
    /// </summary>
    /// <param name="ds">参数集合</param>
    /// <returns>返回符合dsreturn结构的dataset</returns>
     [WebMethod(MessageName = "问题与处理", Description = "返回符合dsreturn结构Dataset,null:程序出现错误。")]
    public DataSet WTYCL_CJLYZX(DataSet ds)
    {
        try
        {
            return new WenTYuChuli().WTYCL(ds);
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 获取异常信息
    /// </summary>
    /// <param name="ht">参数集合</param>
    /// <returns>返回符合dsreturn结构的Dataset，内含有表明为：info的Datatable ;null:程序出现异常</returns>
     [WebMethod(MessageName = "查询问题与处理", Description = "返回符合dsreturn结构Dataset,null:程序出现错误。")]
     public DataSet Getyiwaiinfo_CJLYZX(DataTable ht)
     {
         try
         {
             return new WenTYuChuli().Getyiwaiinfo(ht);
         }
         catch
         {
             return null;
         }
     }

     /// <summary>
    /// 人工清盘点击列表单条数据清盘信息 -- wyh  2014.06.11 
    /// </summary>
    /// <param name="dt">包含：“dlyx”：登陆邮箱；“num” :AAA_ZBDBXXB,Number值</param>
    /// <returns>返回dataset内含表明为“qpinfo”的Datatable</returns>
     [WebMethod(MessageName = "获取清盘数据", Description = "返回dataset内含表明为“qpinfo”的Datatable，null:程序出错！")]
     public DataSet GetQBInfo_CJLYZX(DataTable dt)
     {
         try
         {
             return new QingPanInfo().GetQBInfo(dt);
         }
         catch
         {
             return null;
         }
     }
   
     /// <summary>
    ///根据中标定标信息表编号获得投标资料审核表中的相关信息  wyh 2014.06.12 
    /// </summary>
    /// <param name="num">中标定标信息表Number</param>
    /// <returns>Dataset</returns>
     [WebMethod(MessageName = "投标资料审核表相关信息", Description = "返回dataset内含表明为“sh”的Datatable，null:程序出错！用于人工清盘提交时。")]
     public DataSet GetTBZLSHinfo_CJLYZX(string num)
     {
         try
         {
             return new QingPanInfo().GetTBZLSHinfo(num);
         }
         catch
         {
             return null;
         }
     }

     /// <summary>
    /// 获取中标定标信息表上传文件的信息，用于验证
    /// </summary>
    /// <param name="num">中标定标信息表Number</param>
    /// <returns>Dataset</returns>
     [WebMethod(MessageName = "获取中标定标信息表上传文件的信息", Description = "返回dataset内含表明为“docinfo”的Datatable，null:程序出错！用于人工清盘提交时。")]
     public DataSet GetSHWJinfo_CJLYZX(string num)
     {
         try
         {
             return new QingPanInfo().GetSHWJinfo(num);
         }
         catch
         {
             return null;
         }
     }

    /// <summary>
    /// 人工清盘时用户上传证明文件写入数据表 wyh 2014.06.12 
    /// </summary>
    /// <param name="dt">参数集合</param>
    /// <returns>返回符合dsreturn结构的Dataset</returns>
      [WebMethod(MessageName = "用户上传证明文件", Description = "返回dataset内含表明为“docinfo”的Datatable，null:程序出错！人工清盘时卖家确认用户上传证明文件写入数据表。")]
     public DataSet DWControversial_CJLYZX(DataTable dt)
     {
         try
         {
             return new QingPanInfo().RunFileData(dt);
         }
         catch
         {
             return null;
         }
     }

    
    /// <summary>
    /// 争议文件确认 wyh  2014.06.12
    /// </summary>
    /// <param name="dt"></param>
    /// <returns>返回符合dareturn结构的Dataset</returns>
     [WebMethod(MessageName = "对方确认", Description = "返回dataset内含表明为“docinfo”的Datatable，null:程序出错！人工清盘时买卖家确认。")]
      public DataSet DWConfirm_CJLYZX(DataTable dt)
      {
          try
          {
              return new QingPanInfo().RunConfirmData(dt);
          }
          catch
          {
              return null;
          }
      }

     /// <summary>
     /// 验证投标资料审核表对应的投标点信息
     /// </summary>
     /// <param name="Number">AAA_THDYFHDXXB表Number值</param>
     /// <returns>返回Dataset包含对应的表AAA_TBZLSHB，所有信息</returns>
     [WebMethod(MessageName = "投标资料审核表", Description = "返回Dataset包含对应的表AAA_TBZLSHB，所有信息.")]
     public DataSet GetTBZLSHB(string number)
     {
         try
         {
             return new lvyueclass().GTTBZLSHinfo(number);
         }
         catch
         {
             return null;
         }
     }

     /// <summary>
    /// 获取合同状态，以及提货单发货单信息表所有信息
    /// </summary>
    /// <param name="number">AAA_THDYFHDXXB的Number值</param>
    /// <returns>返回Dataset内含 合同状态，以及提货单发货单信息表所有信息</returns>
     [WebMethod(MessageName = "获取合同状态", Description = "返回Dataset包含对应的表AAA_TBZLSHB，所有信息.")]
     public DataSet GetTHDXXAndHTZT(string number)
     {
         try
         {
             return new lvyueclass().GetTHDXXAndHTZT(number);
         }
         catch
         {
             return null;
         }
     }

     /// <summary>
     /// 《电子购货合同》缔约方保密承诺书，获取中标定标表的信息
     /// </summary>
     /// <param name="Number">中标定标信息表编号</param>
     /// <param name="DLYX"></param>
     /// <returns>返回dataset，其中包含表名为"返回值单条","买卖一家","卖家","买家"的Datatable</returns>
     [WebMethod(MessageName = "保密承诺书获取中标定标表的信息", Description = "返回dataset，其中包含表名为'返回值单条','买卖一家','卖家','买家'的Datatable")]
     public DataSet BMCNS_CJLYZX(string Number, string DLYX)
     {
         try
         {
             return new lvyueclass().BMCNS(Number, DLYX);
         }
         catch
         {
             return null;
         }
     }
     /// <summary>
     /// 根据中标定标信息表Number查询卖家名称、买家名称、合同编号、定标时间、保证函金额、保证函编号
     /// </summary>
     /// <param name="number">中标定标信息表Number</param>
     /// <returns>DataSet【包含“返回值单条”、“主表”两个表】</returns>
     [WebMethod(MessageName = "保证函获取中标定标表的信息", Description = "后台摸板--保证函 返回dataset，其中包含表名为'返回值单条','主表'的Datatable")]
     public DataSet BZH_CJLYZX(string Number)
     {
         try
         {
             return new lvyueclass().GetBZHMesg(Number);
         }
         catch
         {
             return null;
         }
     }

     #region//2014.07.02 zhouli add 录入发货信息、录入发票信息
     /// <summary>
    /// 录入发货信息
    /// </summary>
     /// <param name="ds">ds：卖家角色编号|发货单编号|物流公司名称|物流单号|物流公司联系人|物流公司电话</param>
     /// <returns>returnDS.Tables["返回值单条"]</returns>
     [WebMethod(MessageName = "录入发货信息", Description = "货物收发--录入发货信息")]
     public DataSet LRFHXX_CJLYZX(DataSet ds)
     {
         try
         {
             return new FHDInfo().LRFHXX(ds);
         }
         catch(Exception)
         {
             return null;
         }
     }
     /// <summary>
     /// 录入发货信息
     /// </summary>
     /// <param name="ds">ds：卖家角色编号|发货单编号|发票邮寄单位名称|发票邮寄单编号</param>
     /// <returns>returnDS.Tables["返回值单条"]</returns>
     [WebMethod(MessageName = "录入发票信息", Description = "货物收发--录入发票信息")]
     public DataSet LRFPXX_CJLYZX(DataSet ds)
     {
         try
         {
             return new FHDInfo().LRFPXX(ds);
         }
         catch (Exception)
         {
             return null;
         }
     }
     /// <summary>
     /// 提请买家签收
     /// </summary>
     /// <param name="ds">ds：卖家角色编号|发货单编号|发票邮寄单位名称|发票邮寄单编号</param>
     /// <returns>returnDS.Tables["返回值单条"]</returns>
     [WebMethod(MessageName = "提请买家签收", Description = "货物收发--提请买家签收")]
     public DataSet InsertSQL_CJLYZX(DataSet ds)
     {
         try
         {
             return new HWQSInfo().InsertSQL(ds);
         }
         catch (Exception)
         {
             return null;
         }
     }

     /// <summary>
     /// 提请买家签收时，输入发货单编号后获取的信息--zhouli add 2019.09.04
     /// </summary>
     /// <param name="number">提货单与发货单表的Number</param>
     /// <returns>returnDS.Tables["返回值单条"]，returnDS.Tables["主表"]</returns>
     [WebMethod(MessageName = "是否已提请买家签收", Description = "货物收发--提请买家签收时，输入发货单编号后获取的信息")]
     public DataSet IsHavedQS_CJLYZX(string number)
     {
         try
         {
             return new HWQSInfo().IsHavedQS(number);
         }
         catch (Exception)
         {
             return null;
         }
     }

     #endregion

    #region//2014.07.14 zhouli add 商品买卖C区-清盘

     /// <summary>
     /// 获取中标定标信息表详细信息 zhouli 2014.07.14 add
     /// </summary>
     /// <param name="number">中标定标信息表Number</param>
     /// <returns>DataSet[两个DataTable:“返回值单条”、“HTPXX”]</returns>
     [WebMethod(MessageName = "清盘C区获取中标定标表的信息", Description = "商品买卖C区--清盘 返回dataset，其中包含表名为'返回值单条','HTPXX'的Datatable")]
     public DataSet DBZBXXinfoget_CJLYZX(string Number)
     {
         try
         {
             return new lvyueclass().jhjx_DBZBXXinfoget(Number);
         }
         catch(Exception)
         {
             return null;
         }
     }
     /// <summary>
     /// 获取定标保证函详细信息 zhouli 2014.07.14 add
     /// </summary>
     /// <param name="Number">中标定标信息表Number</param>
     /// <returns>DataSet[两个DataTable:“返回值单条”、“SPXX”]</returns>
     [WebMethod(MessageName = "清盘C区定标保证函详细信息", Description = "商品买卖C区--清盘 返回dataset，其中包含表名为'返回值单条','SPXX'的Datatable")]
     public DataSet TBDYDDbzh_GetCGinfo(string Number)
     {
         try
         {
             return new lvyueclass().jhjx_DBYbzhinfo(Number);
         }
         catch (Exception)
         {
             return null;
         }
     }

    #endregion
}