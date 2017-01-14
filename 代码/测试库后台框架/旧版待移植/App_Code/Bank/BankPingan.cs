using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;
using FMOP.DB;
using Hesion.Brick.Core.WorkFlow;
using System.Configuration;

/// <summary>
/// 平安银行接口调用类
/// </summary>
public class BankPingan
{
    /// <summary>
    /// 统一调用前置机的方法
    /// </summary>
    /// <param name="dtParams">修改后的Dt</param>
    /// <returns></returns>
    private DataSet Go(DataTable dtParams)
    {
        DataTable dt = BankHelper.GetConfig("平安银行");
        if (dt.Rows.Count > 0)
        {
            string routing = dt.Rows[0]["Imv"].ToString() == "测试库" ? dt.Rows[0]["CStr"].ToString() : dt.Rows[0]["ZStr"].ToString();
            DataSet ds = new DataSet();
            DataTable hl = dtParams.Copy();
            ds.Tables.Add(hl);
            object[] _params = { ds };
            return BankHelper.Go(routing, "Reincarnation", _params);
        }
        else return null;
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
    /// 签到、签退【1330】
    /// </summary>
    ///  <param name="dsParams">业务类型，两个可选值：签到，签退</param>
    /// <returns>是否成功及提示文字</returns>
    public DataSet Sign(string type,DataSet dsreturn)
    {   
        //判断今天是不是已经执行过签到/签退了
        DataSet ds_count = DbHelperSQL.Query("select * from AAA_PingAnSign where ywrq=CONVERT(varchar(8), GETDATE(), 112) and ywlx='" + type + "'");

        if (ds_count != null && ds_count.Tables[0].Rows.Count > 0)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "数据库已存在今天的" + type + "信息！";
            return dsreturn;
        }

       //获取1330的xml项并转换为datatable
        FileInfo FI = new FileInfo(HttpContext.Current.Server.MapPath("BankXML/XMLPingan/Us_Send_1330.xml"));
        DataTable dt = new DataTable();
        dt.ReadXml(FI.FullName);   
         
        //插数据库记录签到、签退信息的流水号，作为发给银行的流水号
          WorkFlowModule WFM = new WorkFlowModule("AAA_PingAnSign");
          string KeyNumber = WFM.numberFormat.GetNextNumber();

        //替换其中的参数
        switch (type)
        {
            case "签到":
                dt.Rows[0]["FuncFlag"] = "1";//请求功能，1表示签到
                dt.Rows[0]["Reserve"] = KeyNumber;//流水号。
                break;
            case "签退":
                dt.Rows[0]["FuncFlag"] = "2";//请求功能，2表示签退
                 dt.Rows[0]["Reserve"] = KeyNumber;//流水号。
                break;
            default:
                dt.Rows[0]["FuncFlag"] = "";
                dt.Rows[0]["Reserve"] = "";
                break;
        }       
        dt.Rows[0]["TxDate"] = DateTime.Now.ToString("yyyyMMdd");//交易日期，8位数字
      
        //执行接口
        DataSet ds = Go(dt);
        if (ds == null)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "银行接口调用失败！";
            return dsreturn;

           // ds = Sing_Test( type,dsreturn);
        }

        //处理返回结果
        if (ds.Tables["结果"].Rows[0]["反馈结果"].ToString() == "ok")
        {
            if (ds.Tables["包头"].Rows[0]["RspCode"].ToString() == "000000")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"]="ok";               

                string sql_insert = "insert into AAA_PingAnSign ([Number],[YHMC],[YWRQ],[YWLX],[CGSJ],[YHLSH],[CheckState],[CreateUser],[CreateTime],[CheckLimitTime]) values ('" + KeyNumber + "','平安银行','" + DateTime.Now.ToString("yyyyMMdd") + "','" + type + "','" + DateTime.Now.ToString() + "','" + ds.Tables["包体"].Rows[0]["FrontLogNo"].ToString().Trim() + "',1,'admin','" + DateTime.Now.ToString() + "','" + DateTime.Now.ToString() + "')";
                int i = 0;
                i = DbHelperSQL.ExecuteSql(sql_insert);
                if (i > 0)
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = type + "成功！写数据库成功！";
                }
                else
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] =type+"成功！写数据库失败！";
                }
            }
            else
            {
                string errmsg= ds.Tables["包头"].Rows[0]["RspCode"].ToString() + "：" + ds.Tables["包头"].Rows[0]["RspMsg"].ToString().Trim();
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = errmsg;
            }
            return dsreturn;
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";           
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = ds.Tables["结果"].Rows[0]["详情"].ToString(); 
            return dsreturn;
        }
       
    }

    /// <summary>
    ///签约预指定
    ///平安没有这个方法，直接返回成功即可 
    /// </summary>
    /// <param name="dsParams"></param>
    /// <param name="BI"></param>
    /// <returns></returns>
    public DataSet qianyueyuzhiding(DataSet dsParams, BankInit BI)
    {
        DataSet dsreturn = initReturnDataSet();
        dsreturn.Tables["返回值单条"].Rows.Add(new object[] { "ok", "平安银行无此业务。" });
        return dsreturn;
    }

    #region 客户签约确认
    /// <summary>
    /// 客户签约确认
    /// 根据头部的业务接口号确定是开户还是出入金账户维护
    /// </summary>
    /// <param name="dsParams"></param>
    /// <param name="BI"></param>
    /// <returns></returns>
    public DataSet kehuqianyuequeren(DataSet dsParams, BankInit BI)
    {
        DataSet dsreturn = initReturnDataSet();
        dsreturn .Tables ["返回值单条"].Rows .Add (new object []{"err","初始化"});

        if (!dsParams.Tables.Contains("dtTou") || !dsParams.Tables.Contains("dtTi"))
        {//不包含这两个表的话，返回参数结构错误
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "参数结构错误";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "ERR074";//返回码              
            return dsreturn;
        }
        if (!dsParams.Tables["dtTou"].Columns.Contains("TranFunc"))
        {
            //表头不包含交易类型字段的话，返回错误
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "参数结构错误";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "ERR074";//返回码              
            return dsreturn;
        }
        switch (dsParams.Tables["dtTou"].Rows[0]["TranFunc"].ToString())
        {
            case "1301":
                dsreturn = SubAcctM(dsParams, BI, dsreturn);
                break;
            case "1315":
                dsreturn = RelatedAcctM(dsParams, BI, dsreturn);
                break;
            default:
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "接口编号错误";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "ERR074";
                break;
        }      
        return dsreturn;
    }

    /// <summary>
    /// 会员开销户确认【1301】
    /// </summary>
    /// <param name="dsParams"></param>
    /// <param name="BI"></param>
    /// <returns></returns>
    private DataSet SubAcctM(DataSet dsParams, BankInit BI,DataSet dsreturn)
    {
        //验证子账户信息是否已存在
        DataSet ds_SubAcct = DbHelperSQL.Query("select number,CustAcctId from AAA_PingAnBank where  ThirdCustId='" + BI.SecuritiesCode + "' and ZT='正常'");
        if (ds_SubAcct != null && ds_SubAcct.Tables[0].Rows.Count > 0)
        {            
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "此交易方已开户！";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "ERR074";//返回码  
            return dsreturn;
        }
        //验证子账户是否已存在
        DataSet ds_subAcct = DbHelperSQL.Query("select * from AAA_PingAnBank where CustAcctId='" + dsParams.Tables["dtTi"].Rows[0]["CustAcctId"].ToString().Trim() + "' and ZT='正常'");
        if (ds_subAcct != null || ds_subAcct.Tables[0].Rows.Count > 0)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "子账户账号已存在";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "ERR074";//返回码  
            return dsreturn;
        }
        if (dsParams.Tables["dtTi"].Rows[0]["FuncFlag"].ToString() == "1")
        {//功能标志，1表示开户。验证客户信息，通过后更新数据库对应表
            string khmc = dsParams.Tables["dtTi"].Rows[0]["CustName"].ToString().Trim();//客户名称
            string zjlx = dsParams.Tables["dtTi"].Rows[0]["IdType"].ToString().Trim();//证件类型
            string zjbh = dsParams.Tables["dtTi"].Rows[0]["IdCode"].ToString().Trim();//证件编号 
            
            if (zjlx != "52" && zjlx != "10")
            { //验证证件类型是否正确。目前只接受组织机构代码证和身份证两种              
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "证件类型无效！";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "ERR074";//返回码              
                return dsreturn;
            }
            if (zjlx == "52" && zjbh != BI.OrganizationCode)
            {//验证组织机构代码证号码
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "组织机构代码不符！";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "ERR074";//返回码            
                return dsreturn;
            }
            else if (zjlx == "10" && zjbh != BI.IdCard)
            {//验证身份证号码
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = " 身体份证号不符！";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "ERR074";//返回码             
                return dsreturn;
            }
            //全部验证通过，执行插入操作,银行账号绑定之后才认为第三方存管开通。
            ArrayList al = new ArrayList();          
            WorkFlowModule WFM = new WorkFlowModule("AAA_PingAnBank");
            string KeyNumber = WFM.numberFormat.GetNextNumber();
            string sql_insert = "insert into AAA_PingAnBank (number,DLYX,ThirdCustId,CustAcctId,CustName,IdType,IdCode,SupAcctId,KHRQ,KHLSH,ZT,CustFlag,UserStatus,createtime,createuser) values ('" + KeyNumber + "','" + BI.UserEmail + "','" + BI.SecuritiesCode + "','" + dsParams.Tables["dtTi"].Rows[0]["CustAcctId"].ToString().Trim() + "','" + dsParams.Tables["dtTi"].Rows[0]["CustName"].ToString().Trim() + "','" + dsParams.Tables["dtTi"].Rows[0]["IdType"].ToString().Trim() + "','" + dsParams.Tables["dtTi"].Rows[0]["IdCode"].ToString().Trim() + "','" + dsParams.Tables["dtTi"].Rows[0]["SupAcctId"].ToString().Trim() + "','" + dsParams.Tables["dtTou"].Rows[0]["TranDate"].ToString() + "','" + dsParams.Tables["dtTou"].Rows[0]["ThirdLogNum"].ToString().Trim() + "','正常','" + dsParams.Tables["dtTi"].Rows[0]["CustFlag"].ToString() + "','" + dsParams.Tables["dtTi"].Rows[0]["FuncFlag"].ToString() + "','" + DateTime.Now.ToString() + "','1301')";
            al.Add(sql_insert);

            if (BI.UserDealName != khmc)
            {//判断银行接口的子账户名称是否跟交易方名称一致,不一致则更新登陆账号信息表
                string sql_up = "update AAA_DLZHXXB set I_JYFMC='" + khmc + "' where B_DLYX='" + BI.UserEmail + "'";
                al.Add(sql_up);
            }
            try
            {
                DbHelperSQL.ExecSqlTran(al);
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "交易成功";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "000000";//返回码  
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息2"] = KeyNumber;//返回包体的流水号
                return dsreturn;
            }
            catch
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "交易网更新数据失败";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "ERR074";//返回码  
                return dsreturn;
            }
        }
        else if (dsParams.Tables["dtTi"].Rows[0]["FuncFlag"].ToString() == "2")
        { //若接收到【1301】的功能标志为“修改”时，交易网端直接返回成功即可
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "交易成功";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "000000";//返回码          
            return dsreturn;
        }
        else if (dsParams.Tables["dtTi"].Rows[0]["FuncFlag"].ToString() == "3")
        {
            //按照业务规则，不允许客户解约。直接返回失败。
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "交易网不支持此业务。";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "ERR074";//返回码          
            return dsreturn;
        }
        else
        {
            //非允许的功能代码
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "错误的功能码";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "ERR074";//返回码           
            return dsreturn;
        }
    }
    /// <summary>
    /// 出入金账户维护【1315】
    /// </summary>
    /// <param name="dsParams"></param>
    /// <param name="BI"></param>
    /// <returns></returns>
    private DataSet RelatedAcctM(DataSet dsParams, BankInit BI,DataSet dsreturn)
    {       
        if (dsParams.Tables["dtTi"].Rows[0]["AcctFlag"].ToString() != "3")
        {//出入金账号为同一账号的，其他情况不受理
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "仅支持出入金账号为同一账号";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "ERR074";//返回码  
            return dsreturn;
        }

        //获取子账户信息
        DataSet ds_SubAcct = DbHelperSQL.Query("select number,CustAcctId from AAA_PingAnBank where  ThirdCustId='" + BI.SecuritiesCode + "'");
        if (ds_SubAcct == null || ds_SubAcct.Tables[0].Rows.Count <= 0)
        {//判断是否已开通第三方存管
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "子账户不存在";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "ERR074";//返回码  
            return dsreturn;
        }
        string subAcct = dsParams.Tables["dtTi"].Rows[0]["CustAcctId"].ToString().Trim();
        if (ds_SubAcct.Tables[0].Rows[0]["CustAcctId"].ToString().Trim() != subAcct)
        {//验证银行发送的接口中的交易网会员编号是否与系统记录一致
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "子账户与交易网会员代码对应错误";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "ERR074";//返回码  
            return dsreturn;
        }
        if (dsParams.Tables["dtTi"].Rows[0]["FuncFlag"].ToString() == "1")
        {//功能标志，1表示指定。          
            if (BI.BankState == "开通" && BI.BankCard.ToString().Trim() != "")
            {//判断原来是不是已经存在了出入金账号
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "已绑定出入金账号";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "ERR074";//返回码  
                return dsreturn;
            }           
           
            //全部验证通过，执行更新和插入操作           
            ArrayList al = new ArrayList();
            string sql_update1 = "update AAA_DLZHXXB set I_DSFCGZT='开通',I_YHZH='" + dsParams.Tables["dtTi"].Rows[0]["RelatedAcctId"].ToString ().Trim () + "' where B_DLYX='" + BI.UserEmail + "' and I_ZQZJZH='" + BI.SecuritiesCode + "'";//更新登陆账号信息表中的第三方存管状态字段值为“开通”            
            al.Add(sql_update1);
            string sql_update2 = "update AAA_PingAnBank set RelatedAcctId='" + dsParams.Tables["dtTi"].Rows[0]["RelatedAcctId"].ToString().Trim() + "',AcctName='" + dsParams.Tables["dtTi"].Rows[0]["AcctName"].ToString().Trim() + "',BankName='" + dsParams.Tables["dtTi"].Rows[0]["BankName"].ToString().Trim() + "' where ThirdCustId='" + BI.SecuritiesCode + "'";
            al.Add(sql_update2);
            try
            {
                DbHelperSQL.ExecSqlTran(al);
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "交易成功";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "000000";//返回码  
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息2"] = ds_SubAcct.Tables[0].Rows[0]["number"].ToString();//用平安专用表的number作为返回包体的流水号
                return dsreturn;
            }
            catch
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "交易网更新数据失败";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "ERR074";//返回码  
                return dsreturn;
            }
        }
        else if (dsParams.Tables["dtTi"].Rows[0]["FuncFlag"].ToString() == "2")
        { //若接收到【1315】的功能标志为“修改”时，由于此交易只能修改交易商出入金账号的联行号、开户行名称、付款人/收款人地址，若交易网不关心这些信息，可直接返回成功即可。因交易网发起的出金操作需要用到开户行名称，故此修改需要记录。

            if (BI.BankState!= "开通")
            {//判断原来是不是已经存在了出入金账号
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "尚未绑定出入金账号无法修改";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "ERR074";//返回码  
                return dsreturn;
            }
            if (dsParams.Tables["dtTi"].Rows[0]["BankName"].ToString().Trim() == ds_SubAcct.Tables[0].Rows[0]["BankName"].ToString().Trim())
            { //如果接口中的开户行名称一致的话，其余可修改字段我们不关心，不需要进行任何操作，直接返回成功即可。
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "交易成功";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "000000";//返回码  
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息2"] = ds_SubAcct.Tables[0].Rows[0]["number"].ToString();//用平安专用表的number作为返回包体的流水号
                return dsreturn;
            }
            //全部验证通过，执行更新操作           
            ArrayList al = new ArrayList();            
            string sql_update = "update AAA_PingAnBank set BankName='" + dsParams.Tables["dtTi"].Rows[0]["BankName"].ToString().Trim() + "' where ThirdCustId='" + BI.SecuritiesCode + "'";
            al.Add(sql_update);
            try
            {
                DbHelperSQL.ExecSqlTran(al);
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "交易成功";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "000000";//返回码  
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息2"] = ds_SubAcct.Tables[0].Rows[0]["number"].ToString();//用平安专用表的number作为返回包体的流水号
                return dsreturn;
            }
            catch
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "交易网更新数据失败";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "ERR074";//返回码  
                return dsreturn;
            }
        }
        else if (dsParams.Tables["dtTi"].Rows[0]["FuncFlag"].ToString() == "3")
        {
            //功能表示，3表示删除
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "交易网不支持此业务。";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "ERR074";//返回码          
            return dsreturn;
        }
        else
        {
            //非允许的功能代码
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "错误的功能码";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "ERR074";//返回码           
            return dsreturn;
        }
    }
    #endregion   
   
    /// <summary>
    /// 入金
    /// 入金（交易网发起）【1316】
    /// </summary>
    /// <param name="dsParams"></param>
    /// <param name="BI"></param>
    /// <returns></returns>
    public DataSet rujin(DataSet dsParams, BankInit BI)
    { //dsparams中的参数包括：用户邮箱、转账类别、金额、银行取款密码、交易资金密码      
        DataSet dsreturn = initReturnDataSet();
        dsreturn.Tables["返回值单条"].Rows.Add(new object[] { "err", "初始化" });

        if (BI.BankState != "开通")
        {//判断第三方存管是否开通
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "第三方存管尚未开通，无法进行操作！";
            return dsreturn;
        }
        //if (BI.UserPwd_Bank != dsParams.Tables[0].Rows[0]["交易资金密码"].ToString())
        //{//判断输入的交易资金密码是否正确，暂时注释掉
        //    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
        //    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "交易资金密码错误";                    
        //    return dsreturn;
        //}       

        //获取1316的xml项并转换为datatable
        FileInfo FI = new FileInfo(HttpContext.Current.Server.MapPath("BankXML/XMLPingan/Us_Send_1316.xml"));
        DataTable dt = new DataTable();
        dt.ReadXml(FI.FullName);

        //获取平安银行专属信息
        DataSet dsinfo = DbHelperSQL.Query("select * from AAA_PingAnBank where ThirdCustId='" + BI.SecuritiesCode + "'");
        if (dsinfo == null || dsinfo.Tables[0].Rows.Count <= 0)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "子账户不存在";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "ERR074";//返回码  
            return dsreturn;
        }
        //替换包体中的参数
        //资金汇总账号	SupAcctId	C(32)	必输	即入金的收款账号
        //子账户账号	CustAcctId	C(32)	必输	
        //交易网会员代码	ThirdCustId	C(32)	必输	
        //会员证件类型	IdType	C(2)	必输	
        //会员证件号码	IdCode	C(20)	必输	
        //入金金额	TranAmount	9(15)	必输	
        //入金账号	InAcctId	C(32)	必输	
        //入金账户名称	InAcctIdName	C(120)	必输	
        //币种	CcyCode	C(3)	必输	
        //保留域	Reserve	C(120)		
        dt.Rows[0]["SupAcctId"] = dsinfo.Tables[0].Rows[0]["SupAcctId"].ToString();//XML文件中固定值
        dt.Rows[0]["CustAcctId"] = dsinfo.Tables[0].Rows[0]["CustAcctId"].ToString();
        dt.Rows[0]["ThirdCustId"] = dsinfo.Tables[0].Rows[0]["ThirdCustId"].ToString();
        dt.Rows[0]["IdType"] = dsinfo.Tables[0].Rows[0]["IdType"].ToString();
        dt.Rows[0]["IdCode"] = dsinfo.Tables[0].Rows[0]["IdCode"].ToString();
        dt.Rows[0]["TranAmount"] = Convert.ToInt32(Convert.ToDouble(dsParams.Tables[0].Rows[0]["金额"].ToString()) * 100);//金额以分为单位
        dt.Rows[0]["InAcctId"] = dsinfo.Tables[0].Rows[0]["RelatedAcctId"].ToString();
        dt.Rows[0]["InAcctIdName"] = dsinfo.Tables[0].Rows[0]["AcctName"].ToString();
       // dt.Rows[0]["CcyCode"] = "RMB";//XML中固定值，无特殊情况不许修改
        
        //生成插入帐款流水明细表的Number，作为该笔业务的流水号
        WorkFlowModule WFM = new WorkFlowModule("AAA_ZKLSMXB");
        string key = WFM.numberFormat.GetNextNumberZZ("");
       //包体的保留域用来传递包头中的流水号
        dt.Rows[0]["Reserve"] = key;
      
        //执行接口
        DataSet dsRe = Go(dt);
        if (dsRe == null)
        {            
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "银行接口调用失败！";
            return dsreturn;
            //dsRe = rujin_Test(dsParams, BI);
        }
        //处理银行的反馈信息     
        if (dsRe.Tables["结果"].Rows[0]["反馈结果"].ToString() == "ok")
        {
            if (dsRe.Tables["包头"].Rows[0]["ServType"].ToString() == "02" && dsRe.Tables["包头"].Rows[0]["RspCode"].ToString() == "000000")
            {              
                ArrayList alist = new ArrayList();
                //资金变动明细对照表
                DataSet dsDZB = DbHelperSQL.Query("select * from AAA_moneyDZB where Number='1304000001'");//1304000001 转入资金 银转商
                
                //银行应答包的流水号
                string BankLSH = dsRe.Tables["包体"].Rows[0]["FrontLogNo"].ToString();
                //获取银行应答包包头里的时间
                string strtime = dsRe.Tables["包头"].Rows[0]["TranDate"].ToString() + dsRe.Tables["包头"].Rows[0]["TranTime"].ToString();
                string time = DateTime.ParseExact(strtime, "yyyyMMddHHmmss", null).ToString("yyyy-MM-dd HH:mm:ss");

                //写入资金流水明细               
                string xm = dsDZB.Tables[0].Rows[0]["XM"].ToString().Trim();//项目
                string xz = dsDZB.Tables[0].Rows[0]["XZ"].ToString().Trim();//性质
                string zy = dsDZB.Tables[0].Rows[0]["ZY"].ToString().Trim().Replace("[x1]", BankLSH);//摘要
                string sjlx = dsDZB.Tables[0].Rows[0]["SJLX"].ToString().Trim();//数据类型
                string yslx = dsDZB.Tables[0].Rows[0]["YSLX"].ToString().Trim();//运算类型   
                
                //插入账款流水明细表
                string str = "INSERT INTO [AAA_ZKLSMXB] ([Number],[DLYX],[JSZHLX],[JSBH],[LYYWLX],[LYDH],[LSCSSJ],[YSLX],[JE],[XM] ,[XZ],[ZY] ,[JKBH],[SJLX],BDCJRJLSH,[CheckState],[CreateUser],[CreateTime]) VALUES ('" + key + "','" + BI.UserEmail + "','" + BI.AccountType + "','" + dsParams.Tables[0].Rows[0]["买家角色编号"].ToString() + "','券商入金','" + BankLSH + "','" + time + "','" + yslx + "'," + dsParams.Tables[0].Rows[0]["金额"].ToString() + ",'" + xm + "','" + xz + "','" + zy + "','平安1316','" + sjlx + "','" + key + "',0,'" + BI.UserEmail + "','" + DateTime.Now.ToString() + "')";
                alist.Add(str);
                //更改登录表可用余额
                str = " update AAA_DLZHXXB set B_ZHDQKYYE = B_ZHDQKYYE +" + dsParams.Tables[0].Rows[0]["金额"].ToString() + ",B_DSFCGKYYE=B_DSFCGKYYE+" + dsParams.Tables[0].Rows[0]["金额"].ToString() + " where B_DLYX='" + BI.UserEmail + "'";
                alist.Add(str);

                bool end = DbHelperSQL.ExecSqlTran(alist);
                if (end == true)
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您成功转入" + dsParams.Tables[0].Rows[0]["金额"].ToString() + "元！";
                }
                else
                {
                    //我们自己的数据库数据更新失败
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "交易平台数据更新失败！";
                }               
            }
            else
            {//银行返回失败提示
                string errmsg = dsRe.Tables["包头"].Rows[0]["RspCode"].ToString() + "：" + dsRe.Tables["包头"].Rows[0]["RspMsg"].ToString().Trim();
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "银行转账失败！";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = errmsg;
            }
            return dsreturn;
        }
        else
        {//调用银行接口的功能返回失败的信息
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = dsRe.Tables["结果"].Rows[0]["msg"].ToString();
            return dsreturn;
        }
    }
    public DataSet rujin_Result(DataSet dsParams, BankInit BI)
    {
        DataSet dsreturn = initReturnDataSet();
        dsreturn.Tables["返回值单条"].Rows.Add(new object[] { "err", "银行接口关闭" });
        return dsreturn;
    }
    

    /// <summary>
    /// 出金（交易网发起）【1318】
    /// </summary>
    /// <param name="dsParams"></param>
    /// <param name="BI"></param>
    /// <returns></returns>

    public DataSet chujin(DataSet dsParams, BankInit BI)
    {
        //dsparams中的参数包括：用户邮箱、开户银行、银行账号、转账类别、金额、银行取款密码、交易资金密码      
        DataSet dsreturn = initReturnDataSet();
        dsreturn.Tables["返回值单条"].Rows.Add(new object[] { "err", "初始化" });

        if (BI.BankState != "开通")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "第三方存管尚未开通，无法进行操作！";
            return dsreturn;
        }
        //if (BI.UserPwd_Bank != dsParams.Tables[0].Rows[0]["交易资金密码"].ToString())
        //{
        //    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
        //    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "交易资金密码错误";
        //    return dsreturn;
        //}     

        // 获取最大可转账金额
        double KZYE = 0.00;//可转余额
        //登录账号可用余额小于银行端存管管理账户余额，可转账额取登录账号可用余额
        if (BI.UserMoney_PT <= BI.UserMoney_Bank)
        {
            KZYE = BI.UserMoney_PT;//可转余额=当前账户可用余额
        }
        else
        {
            KZYE = BI.UserMoney_Bank;//可转余额=第三方存管可用余额
        }
        double je = Convert.ToDouble(dsParams.Tables[0].Rows[0]["金额"]);

        //验证余额是否足够
        if (je > KZYE && je.ToString("#0.00") != KZYE.ToString("#0.00"))
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您当前账户最大可转金额为" + KZYE.ToString("#0.00") + "元，本次转账金额过大，不能进行转账！";
            return dsreturn;
        }
        #region 生成出金需要的数据包
        //获取1318的xml项并转换为datatable
        FileInfo FI = new FileInfo(HttpContext.Current.Server.MapPath("BankXML/XMLPingan/Us_Send_1318.xml"));
        DataTable dt = new DataTable();
        dt.ReadXml(FI.FullName);

        //获取平安银行专属信息
        DataSet dsinfo = DbHelperSQL.Query("select * from AAA_PingAnBank where ThirdCustId='" + BI.SecuritiesCode + "'");
        if (dsinfo == null || dsinfo.Tables[0].Rows.Count <= 0)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "子账户不存在";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "ERR074";//返回码  
            return dsreturn;
        }
        //替换包体中的参数
        //交易网名称	TranWebName	C(120)	必输	
        //交易网会员代码	ThirdCustId	C(32)	必输	
        //会员证件类型	IdType	C(2)	必输	
        //会员证件号码	IdCode	C(20)	必输	
        //出金类型	TranOutType	C（2）	必输	1：会员出金 2：转出交易手续费/利息 3：深发监管资金支付清收
        //子账户账号	CustAcctId	C(32)	必输	
        //子账户名称	CustName	C(120)	必输	
        //资金汇总账号	SupAcctId	C(32)	必输	
        //转账方式	TranType	C(1)	必输	1：行内转账 2：同城转账 3：异地汇款
        //出金账号	OutAcctId	C(32)	必输	即收款账户，必须是在系统中维护的出金账号
        //出金账户名称	OutAcctIdName	C(120)	必输	若会员出金，必须与子账户名称一致
        //出金账号开户行名	OutAcctIdBankName	C(120)	必输	
        //出金账号开户联行号	OutAcctIdBankCode	C(12)	可选	
        //出金账号开户行地址	Address	C(120)	可选	
        //币种	CcyCode	C(3)	可选	默认为RMB
        //申请出金金额	TranAmount	9(15)	必输	不包括转账手续费
        //支付转账手续费的子账号	FeeOutCustId	C(32)	可选	保留字段,暂时不用
        //保留域	Reserve	C(120)		

        //dt.Rows[0]["TranWebName"] = dsinfo.Tables[0].Rows[0]["TranWebName"].ToString();//XML中设置默认值
        dt.Rows[0]["ThirdCustId"] = dsinfo.Tables[0].Rows[0]["ThirdCustId"].ToString();
        dt.Rows[0]["IdType"] = dsinfo.Tables[0].Rows[0]["IdType"].ToString();
        dt.Rows[0]["IdCode"] = dsinfo.Tables[0].Rows[0]["IdCode"].ToString();
        // dt.Rows[0]["TranOutType"] ="01";//XML中有默认值，无特殊情况不许替换
        dt.Rows[0]["CustAcctId"] = dsinfo.Tables[0].Rows[0]["CustAcctId"].ToString();
        dt.Rows[0]["CustName"] = dsinfo.Tables[0].Rows[0]["CustName"].ToString();
        dt.Rows[0]["SupAcctId"] = dsinfo.Tables[0].Rows[0]["SupAcctId"].ToString();//XML中设置默认值
        //dt.Rows[0]["TranType"] = "1";//XML中设置默认值，无特殊情况，不需替换
        dt.Rows[0]["OutAcctId"] = dsinfo.Tables[0].Rows[0]["RelatedAcctId"].ToString();
        dt.Rows[0]["OutAcctIdName"] = dsinfo.Tables[0].Rows[0]["AcctName"].ToString();
        dt.Rows[0]["OutAcctIdBankName"] = dsinfo.Tables[0].Rows[0]["BankName"].ToString();
        //dt.Rows[0]["CcyCode"] = "RMB";//XML设置默认值
        dt.Rows[0]["TranAmount"] = je.ToString ("#0.00");//金额         
        #endregion

        //生成帐款流水明细表的Number，作为该笔业务的流水号
        WorkFlowModule WFM = new WorkFlowModule("AAA_ZKLSMXB");
        string key = WFM.numberFormat.GetNextNumberZZ("");
        
        //包体的保留域用来传递包头中的流水号
        dt.Rows[0]["Reserve"] = key;

        #region 先更新自己的数据库
        //出金验证通过后，先更新自己的数据库。写入资金流水明细、更改登录表可用余额及第三方存管可用余额
        ArrayList alist = new ArrayList();
        //资金变动明细对照表
        DataSet dsDZB = DbHelperSQL.Query("select * from AAA_moneyDZB where Number='1304000002'");//1304000002 转出资金 商转银

        string BankLSH = "暂无";//银行应答包的流水号
        //写入资金流水明细               
        string xm = dsDZB.Tables[0].Rows[0]["XM"].ToString().Trim();//项目
        string xz = dsDZB.Tables[0].Rows[0]["XZ"].ToString().Trim();//性质        
        string zy = dsDZB.Tables[0].Rows[0]["ZY"].ToString().Trim();//暂不替换摘要中的流水号
        string sjlx = dsDZB.Tables[0].Rows[0]["SJLX"].ToString().Trim();//数据类型
        string yslx = dsDZB.Tables[0].Rows[0]["YSLX"].ToString().Trim();//运算类型       
        string datetime = DateTime.Now.ToString();//先用当前时间更新 

        //插入账款流水明细表
        string sql_insert = "INSERT INTO [AAA_ZKLSMXB] ([Number],[DLYX],[JSZHLX],[JSBH],[LYYWLX],[LYDH],[LSCSSJ],[YSLX],[JE],[XM] ,[XZ],[ZY] ,[JKBH],[SJLX],BDCJRJLSH,[CheckState],[CreateUser],[CreateTime]) VALUES ('" + key + "','" + BI.UserEmail + "','" + BI.AccountType + "','" + dsParams.Tables[0].Rows[0]["买家角色编号"].ToString() + "','券商出金','" + BankLSH + "','" + datetime + "','" + yslx + "'," + dsParams.Tables[0].Rows[0]["金额"].ToString() + ",'" + xm + "','" + xz + "','" + zy + "','平安1318','" + sjlx + "','" + key + "',0,'" + BI.UserEmail + "','" + DateTime.Now.ToString() + "')";
        alist.Add(sql_insert);
        //更改登录表可用余额
        string sql_upDL = " update AAA_DLZHXXB set B_ZHDQKYYE = B_ZHDQKYYE-" + dsParams.Tables[0].Rows[0]["金额"].ToString() + ",B_DSFCGKYYE=B_DSFCGKYYE-" + dsParams.Tables[0].Rows[0]["金额"].ToString() + " where B_DLYX='" + BI.UserEmail + "'";
        alist.Add(sql_upDL);

        //更新交易平台数据库
        bool end = DbHelperSQL.ExecSqlTran(alist);

        if (end == false)
        {
            //我们自己的数据库数据更新失败
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "交易平台转账失败！";
            return dsreturn;
        }
        #endregion

        //自己的数据更新成功后，再调用银行接口  
        dt.Rows[0]["Reserve"] = key;//用保留域传递包头的流水号 
        DataSet dsRe = Go(dt);
        if (dsRe == null)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "银行接口错误";
            //系统出金回滚操作
            bool rollback = ChuJinRollBack(BI.UserEmail, key, dsParams.Tables[0].Rows[0]["金额"].ToString());
            if (rollback == true)
            {
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息2"] = "系统回滚成功";              
            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息2"] = "系统回滚失败";              
            }
            return dsreturn;

            //dsRe = chujin_Test(dsParams, BI);
        }

        //处理银行的反馈信息     
        if (dsRe.Tables["结果"].Rows[0]["反馈结果"].ToString() == "ok")
        {
            if (dsRe.Tables["包头"].Rows[0]["ServType"].ToString() == "02" && dsRe.Tables["包头"].Rows[0]["RspCode"].ToString() == "000000")
            {
                //只要银行成功了，就提示转账成功了
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您成功转账" + dsParams.Tables[0].Rows[0]["金额"].ToString() + "元！";

                //银行应答包的流水号 
                BankLSH = dsRe.Tables["包体"].Rows[0]["FrontLogNo"].ToString();
                //获取银行应答包包头里的时间
                string strtime = dsRe.Tables["包头"].Rows[0]["TranDate"].ToString() + dsRe.Tables["包头"].Rows[0]["TranTime"].ToString();
                string time = DateTime.ParseExact(strtime, "yyyyMMddHHmmss", null).ToString("yyyy-MM-dd HH:mm:ss");
                //更新账款流水明细表中的流水号
                string sql_update = "update [AAA_ZKLSMXB] set LSCSSJ='" + time + "', ZY=REPLACE(ZY,'[x1]','" + BankLSH + "'),LYDH='" + BankLSH + "' where Number='" + key + "' and DLYX='" + BI.UserEmail + "'";
                //不管执行成功与否，都返回转账成功
                try
                {
                    int i = DbHelperSQL.ExecuteSql(sql_update);
                }
                catch
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["附件信息2"] = "更新账款流水明细流水号失败！";

                }
                return dsreturn;
            }
            else
            {
                //银行返回码为失败的情况，进行回滚
                string errmsg = dsRe.Tables["包头"].Rows[0]["RspCode"].ToString() + "：" + dsRe.Tables["包头"].Rows[0]["RspMsg"].ToString().Trim();
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "银行转账失败！";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = errmsg;

                bool rollback = ChuJinRollBack(BI.UserEmail, key, dsParams.Tables[0].Rows[0]["金额"].ToString());
                if (rollback == true)
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["附件信息2"] = "系统回滚成功";
                }
                else
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["附件信息2"] = "系统回滚失败";
                }
                return dsreturn;
            }
        }
        else
        {//调用银行接口的功能返回失败，则进行系统回滚
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "银行接口失败！";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = dsRe.Tables["结果"].Rows[0]["详情"].ToString();
            bool rollback = ChuJinRollBack(BI.UserEmail, key, dsParams.Tables[0].Rows[0]["金额"].ToString());
            if (rollback == true)
            {
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息2"] = "系统回滚成功";
            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息2"] = "系统回滚失败";
            }
            return dsreturn;
        }
    }
    public DataSet chujin_Result(DataSet dsParams, BankInit BI)
    {
        DataSet dsreturn = initReturnDataSet();
        dsreturn.Tables["返回值单条"].Rows.Add(new object[] { "err", "银行接口关闭" });
        return dsreturn;
    }   
    
   /// <summary>
   ///交易网发起的出金，银行接口返回失败后，系统回滚处理
   /// </summary>
   /// <param name="UserEmail">用户邮箱</param>
   /// <param name="key">插入账款流水明细表的键值</param>
   /// <param name="je">出金金额</param>
   /// <returns></returns>
    private bool ChuJinRollBack(string UserEmail, string key, string je)
    {
        //系统模拟回滚操作        
        ArrayList al_back = new ArrayList();
        //获取出入金异常数据表的key
        WorkFlowModule WFMCRJ = new WorkFlowModule("AAA_CRJYCSJB");
        string keyCRJ = WFMCRJ.numberFormat.GetNextNumberZZ("");
        string sql_istCRJYC = "insert into AAA_CRJYCSJB select '" + keyCRJ + "', [Number],[DLYX],[JSZHLX],[JSBH],[LYYWLX],[LYDH],[LSCSSJ],[YSLX],[JE],[XM],[XZ],[ZY],[SJLX],[JKBH],'异常',[BDCJRJLSH],[QTBZ],null,0,'admin','" + DateTime.Now.ToString() + "','" + DateTime.Now.ToString() + "' from AAA_ZKLSMXB where number='" + key + "'";
        al_back.Add(sql_istCRJYC);
        //删除之前插入到账款流水明细表中的数据
        string sql_del = "delete from AAA_ZKLSMXB where number='" + key + "'";
        al_back.Add(sql_del);
        //将登陆账号信息表中的余额更新回去
        string sql_up = "update AAA_DLZHXXB set B_ZHDQKYYE = B_ZHDQKYYE+" + je + ",B_DSFCGKYYE=B_DSFCGKYYE+" + je + " where B_DLYX='" +UserEmail + "'";
        al_back.Add(sql_up);        
        try
        {
            DbHelperSQL.ExecuteSqlTran(al_back);
            return true;
        }
        catch
        {
            return false;
        }    
    }

    /// <summary>
    /// 银行端入金
    /// 入金（银行发起）【1310】
    /// </summary>
    /// <param name="dsParams"></param>
    /// <param name="BI"></param>
    /// <returns></returns>
    public DataSet yinhangduanrujin(DataSet dsParams, BankInit BI)
    {
        DataSet dsreturn = initReturnDataSet();
        dsreturn.Tables["返回值单条"].Rows.Add(new object[] { "err", "初始化" });

        if (!dsParams.Tables.Contains("dtTou") || !dsParams.Tables.Contains("dtTi"))
        {//不包含这两个表的话，返回参数结构错误
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "参数结构错误";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "ERR074";//返回码              
            return dsreturn;
        }
        if (!dsParams.Tables["dtTou"].Columns.Contains("TranFunc"))
        {//表头不包含交易类型字段的话，返回错误
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "参数结构错误";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "ERR074";//返回码              
            return dsreturn;
        }
        if (dsParams.Tables["dtTou"].Rows[0]["TranFunc"].ToString().Trim() != "1310")
        {//表头包含交易类型字段的值应该为1316
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "接口编号错误";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "ERR074";//返回码     
        }       
        if (BI.BankState != "开通")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "第三方存管尚未开通，无法进行操作！";
            return dsreturn;
        }
        if (BI.BankCard != dsParams.Tables["dtTi"].Rows[0]["InAcctId"].ToString())
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "入金账号与绑定账号不一致";
            return dsreturn;
        }

        //获取第三方存管子账号信息       
        DataSet ds_SubAcct = DbHelperSQL.Query("select * from AAA_PingAnBank where  ThirdCustId='" + BI.SecuritiesCode + "'");
        if (dsParams.Tables["dtTi"].Rows[0]["SupAcctId"].ToString() != ds_SubAcct.Tables[0].Rows[0]["SupAcctId"].ToString())
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "资金汇总账号错误";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "ERR074";
            return dsreturn;
        }
        if (dsParams.Tables["dtTi"].Rows[0]["CustAcctId"].ToString() != ds_SubAcct.Tables[0].Rows[0]["CustAcctId"].ToString())
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "子账号错误";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "ERR074";
            return dsreturn;
        }
        if (dsParams.Tables["dtTi"].Rows[0]["InAcctIdName"].ToString() != ds_SubAcct.Tables[0].Rows[0]["AcctName"].ToString())
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "入金账号与账户名称不一致";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "ERR074";
            return dsreturn;
        }

        try
        {
            #region 写入资金流水明细、更改登录表可用余额
            ArrayList al = new ArrayList();
            Hashtable htInfo = PublicClass2013.GetUserInfo(BI.SecuritiesCode);//获取用户的基本信息
            string JSBH = htInfo["买家角色编号"].ToString();//买家角色编号  
            //资金变动明细对照表
            DataSet dsDZB = DbHelperSQL.Query("select * from AAA_moneyDZB where Number='1304000001'"); //1304000001 转入资金 银转商 

            //获取银行流水号
            string BankLSH = dsParams.Tables["dtTou"].Rows[0]["ThirdLogNo"].ToString().Trim();
            string strtime = dsParams.Tables["dtTou"].Rows[0]["TranDate"].ToString() + dsParams.Tables["dtTou"].Rows[0]["TranTime"].ToString();
            string time = DateTime.ParseExact(strtime, "yyyyMMddHHmmss", null).ToString("yyyy-MM-dd HH:mm:ss");

            string je = dsParams.Tables["dtTi"].Rows[0]["TranAmount"].ToString();
            //写入资金流水明细     
            string xm = dsDZB.Tables[0].Rows[0]["XM"].ToString().Trim();
            string xz = dsDZB.Tables[0].Rows[0]["XZ"].ToString().Trim();
            string zy = dsDZB.Tables[0].Rows[0]["ZY"].ToString().Trim().Replace("[x1]", BankLSH);
            string sjlx = dsDZB.Tables[0].Rows[0]["SJLX"].ToString().Trim();
            string yslx = dsDZB.Tables[0].Rows[0]["YSLX"].ToString().Trim();

            //生成帐款流水明细表的Number
            WorkFlowModule WFM = new WorkFlowModule("AAA_ZKLSMXB");
            string key = WFM.numberFormat.GetNextNumberZZ("");

            string sql_insert = "INSERT INTO [AAA_ZKLSMXB] ([Number],[DLYX],[JSZHLX],[JSBH],[LYYWLX],[LYDH],[LSCSSJ],[YSLX],[JE],[XM] ,[XZ],[ZY] ,[JKBH],[SJLX],BDCJRJLSH,[CheckState],[CreateUser],[CreateTime]) VALUES ('" + key + "','" + BI.UserEmail + "','" + BI.AccountType + "','" + JSBH + "','银行入金','" + BankLSH + "','" + time + "','" + yslx + "'," + je + ",'" + xm + "','" + xz + "','" + zy + "','平安1310','" + sjlx + "','" + key + "',0,'平安银行','" + time + "')";
            al.Add(sql_insert);
            //更改登录表可用余额
            string sql_update = " update AAA_DLZHXXB set B_ZHDQKYYE = B_ZHDQKYYE +" + je + ",B_DSFCGKYYE=B_DSFCGKYYE+" + je + " where B_DLYX='" + BI.UserEmail + "'";
            al.Add(sql_update);
            #endregion

            DbHelperSQL.ExecuteSqlTran(al);
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "交易成功";//返回说明           
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "000000";//返回码
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息2"] = key;//证券流水号
            return dsreturn;
        }
        catch
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "交易网转账失败!";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "ERR074";
            return dsreturn;
        }
    }
    public DataSet yinhangduanrujin_Result(DataSet dsParams, BankInit BI)
    {
        return null;
    }

    /// <summary>
    /// 银行端出金
    /// 出金（银行发起）【1312】
    /// </summary>
    /// <param name="dsParams"></param>
    /// <param name="BI"></param>
    /// <returns></returns>
    public DataSet yinhangduanchujin(DataSet dsParams, BankInit BI)
    {
        DataSet dsreturn = initReturnDataSet();
        dsreturn.Tables["返回值单条"].Rows.Add(new object[] { "err", "初始化" });

        if (!dsParams.Tables.Contains("dtTou") || !dsParams.Tables.Contains("dtTi"))
        {//不包含这两个表的话，返回参数结构错误
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "参数结构错误";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "ERR074";//返回码              
            return dsreturn;
        }
        if (!dsParams.Tables["dtTou"].Columns.Contains("TranFunc"))
        {//表头不包含交易类型字段的话，返回错误
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "参数结构错误";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "ERR074";//返回码              
            return dsreturn;
        }
        if (dsParams.Tables["dtTou"].Rows[0]["TranFunc"].ToString().Trim() != "1312")
        {//表头包含交易类型字段的值应该为1316
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "接口编号错误";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "ERR074";//返回码     
        }
        if (BI.BankState != "开通")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "第三方存管尚未开通，无法进行操作！";
            return dsreturn;
        }
        if (BI.BankCard != dsParams.Tables["dtTi"].Rows[0]["OutAcctId"].ToString())
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "出金账号与绑定账号不一致";
            return dsreturn;
        }
        //获取第三方存管子账号信息       
        DataSet ds_SubAcct = DbHelperSQL.Query("select * from AAA_PingAnBank where  ThirdCustId='" + BI.SecuritiesCode + "'");
        if (dsParams.Tables["dtTi"].Rows[0]["SupAcctId"].ToString() != ds_SubAcct.Tables[0].Rows[0]["SupAcctId"].ToString())
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "资金汇总账号错误";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "ERR074";
            return dsreturn;
        }
        if (dsParams.Tables["dtTi"].Rows[0]["CustAcctId"].ToString() != ds_SubAcct.Tables[0].Rows[0]["CustAcctId"].ToString())
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "子账号错误";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "ERR074";
            return dsreturn;
        }
        if (dsParams.Tables["dtTi"].Rows[0]["OutAcctIdName"].ToString() != ds_SubAcct.Tables[0].Rows[0]["AcctName"].ToString())
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "出金账号与账户名称不一致";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "ERR074";
            return dsreturn;
        }


        // 获取最大可转账金额
        double KZYE = 0.00;//可转余额
        //登录账号可用余额小于银行端存管管理账户余额，可转账额取登录账号可用余额
        if (BI.UserMoney_PT <= BI.UserMoney_Bank)
        {
            KZYE = BI.UserMoney_PT;//可转余额=当前账户可用余额
        }
        else
        {
            KZYE = BI.UserMoney_Bank;//可转余额=第三方存管可用余额
        }
        double je = Convert.ToDouble(dsParams.Tables["dtTi"].Rows[0]["TranAmount"]) ;

        //验证余额是否足够
        if (je > KZYE && je.ToString("#0.00") != KZYE.ToString("#0.00"))
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "余额不足";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "ERR074";
            return dsreturn;
        }

        try
        {
            #region 写入资金流水明细、更改登录表可用余额
            ArrayList al = new ArrayList();
            Hashtable htInfo = PublicClass2013.GetUserInfo(BI.SecuritiesCode);//获取用户的基本信息
            string JSBH = htInfo["买家角色编号"].ToString();//买家角色编号  
            //资金变动明细对照表
            DataSet dsDZB = DbHelperSQL.Query("select * from AAA_moneyDZB where Number='1304000002'"); //1304000002 转出资金 商转银 

            //获取银行流水号
            string BankLSH = dsParams.Tables["dtTou"].Rows[0]["ThirdLogNo"].ToString().Trim();
            string strtime = dsParams.Tables["dtTou"].Rows[0]["TranDate"].ToString() + dsParams.Tables["dtTou"].Rows[0]["TranTime"].ToString();
            string time = DateTime.ParseExact(strtime, "yyyyMMddHHmmss", null).ToString("yyyy-MM-dd HH:mm:ss");

            //写入资金流水明细     
            string xm = dsDZB.Tables[0].Rows[0]["XM"].ToString().Trim();
            string xz = dsDZB.Tables[0].Rows[0]["XZ"].ToString().Trim();
            string zy = dsDZB.Tables[0].Rows[0]["ZY"].ToString().Trim().Replace("[x1]", BankLSH);
            string sjlx = dsDZB.Tables[0].Rows[0]["SJLX"].ToString().Trim();
            string yslx = dsDZB.Tables[0].Rows[0]["YSLX"].ToString().Trim();

            //生成帐款流水明细表的Number
            WorkFlowModule WFM = new WorkFlowModule("AAA_ZKLSMXB");
            string key = WFM.numberFormat.GetNextNumberZZ("");

            string sql_insert = "INSERT INTO [AAA_ZKLSMXB] ([Number],[DLYX],[JSZHLX],[JSBH],[LYYWLX],[LYDH],[LSCSSJ],[YSLX],[JE],[XM] ,[XZ],[ZY] ,[JKBH],[SJLX],BDCJRJLSH,[CheckState],[CreateUser],[CreateTime]) VALUES ('" + key + "','" + BI.UserEmail + "','" + BI.AccountType + "','" + JSBH + "','银行出金','" + BankLSH + "','" + time + "','" + yslx + "'," + je.ToString("#0.00") + ",'" + xm + "','" + xz + "','" + zy + "','平安1312','" + sjlx + "','" + key + "',0,'平安银行','" + time + "')";
            al.Add(sql_insert);
            //更改登录表可用余额
            string sql_update = " update AAA_DLZHXXB set B_ZHDQKYYE = B_ZHDQKYYE -" + je.ToString("#0.00") + ",B_DSFCGKYYE=B_DSFCGKYYE-" + je.ToString("#0.00") + " where B_DLYX='" + BI.UserEmail + "'";
            al.Add(sql_update);
            #endregion

            DbHelperSQL.ExecuteSqlTran(al);
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "交易成功";//返回说明           
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "000000";//返回码
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息2"] = key;//证券流水号
            return dsreturn;
        }
        catch
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "交易网转账失败!";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "ERR074";
            return dsreturn;
        }
    }
    public DataSet yinhangduanchujin_Result(DataSet dsParams, BankInit BI)
    {
        return null;
    }


    /// <summary>
    /// 券商余额查询
    /// 银行发起--查交易网端会员管理账户余额【1019】
    /// </summary>
    /// <param name="dsParams"></param>
    /// <param name="BI"></param>
    /// <returns></returns>
    public DataSet quanshangyuechaxun(DataSet dsParams, BankInit BI)
    {
        DataSet dsreturn = initReturnDataSet();
        dsreturn.Tables["返回值单条"].Rows.Add(new object[] { "err", "初始化" });
        if (!dsParams.Tables.Contains("dtTou") || !dsParams.Tables.Contains("dtTi"))
        {//不包含这两个表的话，返回参数结构错误
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "参数结构错误";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "ERR074";//返回码              
            return dsreturn;
        }
        if (!dsParams.Tables["dtTou"].Columns.Contains("TranFunc"))
        {//表头不包含交易类型字段的话，返回错误
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "参数结构错误";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "ERR074";//返回码              
            return dsreturn;
        }
        if (dsParams.Tables["dtTou"].Rows[0]["TranFunc"].ToString().Trim() != "1019")
        {//表头包含交易类型字段的值应该为1316
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "接口编号错误";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "ERR074";//返回码     
        }
        //验证子账号信息
        DataSet ds_info = DbHelperSQL.Query("select * from AAA_PingAnBank where ThirdCustId='" + BI.SecuritiesCode + "'");
        if (ds_info == null || ds_info.Tables[0].Rows.Count <= 0)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "子账户不存在";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "ERR074";
            return dsreturn;
        }
        string CustAcctId = dsParams.Tables["dtTi"].Rows[0]["CustAcctId"].ToString().Trim();
        string zzhzh = ds_info.Tables[0].Rows[0]["CustAcctId"].ToString();
        if (CustAcctId != "" && CustAcctId != zzhzh)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "子账户账号与交易网会员代码不对应";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "ERR074";
            return dsreturn;
        }
        string whdate = Convert.ToDateTime(ds_info.Tables[0].Rows[0]["createtime"]).ToString("yyyyMMdd");

        //获取三个账户金额
        double totalMoney = 0;//账户总余额=总冻结+可用,单位为“分”
        double dongjieMoney = 0;//账户总冻结金额，一直为0
        double keyongMoney = 0;//账户可用余额，为当前可进行出金的金额。
        if (BI.UserMoney_PT >= BI.UserMoney_Bank)
        {
            totalMoney = BI.UserMoney_Bank;
            keyongMoney = BI.UserMoney_Bank;           
        }
        else
        {
            totalMoney = BI.UserMoney_PT;
            keyongMoney = BI.UserMoney_PT;            
        }
        //生成返回数据包      
        //子账户	CustAcctId	C(32)	必输	
        //交易网会员代码	ThirdCustId	C(32)	必输	
        //子账户名称	CustName	C(120)	必输	
        //账户总余额	TotalAmount	9(15)	必输	以分为单位，例如100.01元，则填：10001
        //账户可用余额	TotalBalance	9(15)	必输	同上
        //账户总冻结金额	TotalFreezeAmount	9(15)	必输	同上
        //维护日期	TranDate	C(8)	必输	开户日期或修改日期
        //保留域	Reserve	C(120)	可选	

        //获取1019应答包的xml项并转换为datatable
        FileInfo FI = new FileInfo(HttpContext.Current.Server.MapPath("BankXML/XMLPingan/Us_Reply_1019.xml"));
        DataTable dt_data = new DataTable();       
        dt_data.ReadXml(FI.FullName);      
        dt_data.Rows [0]["CustAcctId"]=zzhzh;
        dt_data.Rows [0]["ThirdCustId"]=BI.SecuritiesCode;
        dt_data.Rows [0]["CustName"]=ds_info.Tables[0].Rows[0]["CustName"].ToString();
        dt_data.Rows[0]["TotalAmount"] = Convert.ToInt32(totalMoney * 100);
        dt_data.Rows[0]["TotalBalance"] = Convert.ToInt32(keyongMoney * 100);
        dt_data.Rows[0]["TotalFreezeAmount"] =Convert .ToInt32(dongjieMoney*100);
        dt_data.Rows [0]["TranDate"]=whdate;
        dt_data.Rows [0]["Reserve"]="";

         dt_data.TableName = "data";
        dsreturn.Tables.Add(dt_data);

        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "交易成功";
        dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "000000";
        return dsreturn;
    }
    public DataSet quanshangyuechaxun_Result(DataSet dsParams, BankInit BI)
    {
        return null;
    }

    /// <summary>
    /// 银行端换卡
    /// </summary>
    /// <param name="dsParams"></param>
    /// <param name="BI"></param>
    /// <returns></returns>
    public DataSet yinhangduanhuanka(DataSet dsParams, BankInit BI)
    {
        //平安银行不允许换卡操作。修改之允许修改联行号、开户行名称、付款人/收款人地址
        DataSet dsreturn = initReturnDataSet();
        dsreturn.Tables["返回值单条"].Rows.Add(new object[] { "err", "平安银行不支持此操作！" });
        return dsreturn;
    }
    public DataSet yinhangduanhuanka_Result(DataSet dsParams, BankInit BI)
    {
        return null;
    }

    /// <summary>
    /// 冲银行端出金，平安银行无此业务，直接返回失败
    /// </summary>
    /// <param name="dsParams"></param>
    /// <param name="BI"></param>
    /// <returns></returns>
    public DataSet chongyinhangduanchujin(DataSet dsParams, BankInit BI)
    {
        DataSet dsreturn = initReturnDataSet();
        dsreturn.Tables["返回值单条"].Rows.Add(new object[] { "err", "平安银行不支持此业务" });
        return dsreturn;    
    }
    public DataSet chongyinhangduanchujin_Result(DataSet dsParams, BankInit BI)
    {
        return null;
    }

    /// <summary>
    /// 冲银行端入金，平安无此业务，直接返回失败
    /// </summary>
    /// <param name="dsParams"></param>
    /// <param name="BI"></param>
    /// <returns></returns>
    public DataSet chongyinhangduanrujin(DataSet dsParams, BankInit BI)
    {
        DataSet dsreturn = initReturnDataSet();
        dsreturn.Tables["返回值单条"].Rows.Add(new object[] { "err", "平安银行不支持此业务" });
        return dsreturn;
    }
    public DataSet chongyinhangduanrujin_Result(DataSet dsParams, BankInit BI)
    {
        return null;
    }

    /// <summary>
    /// 会员开销户/出入金流水匹配【1006】接口
    /// </summary>
    /// <param name="type">业务类型</param>
    /// <param name="dsreturn"></param>
    /// <returns></returns>
    public DataSet AcctAndMnyMatch(DataTable dtParams, DataSet dsreturn)
    {
        //获取1006的xml包体信息           
        FileInfo FI = new FileInfo(HttpContext.Current.Server.MapPath("BankXML/XMLPingan/Us_Send_1006.xml"));
        DataTable dt = new DataTable();
        dt.ReadXml(FI.FullName);
        //替换参数
//        功能标志	FuncFlag	C(1)	必输	1:出入金流水对账  2：对会员开销户流水
//资金汇总账号	SupAcctId	C(32)	必输	
//开始时间	BeginDateTime	C(14)	必输	日期+时间：如20100408010101（若输入的只是日期，则从该日的0时0分01秒开始）
//结束时间	EndDateTime	C(14)	必输	同上
//保留域	Reserve	C(120)	可选	
        switch (dtParams.Rows[0]["type"].ToString())
        {
            case "开销户":
                dt.Rows[0]["FuncFlag"] = "2";
                break;
            case "出入金":
                dt.Rows[0]["FuncFlag"] = "1";
                break;
            default:
                break;
        }       
        dt.Rows[0]["BeginDateTime"] = dtParams.Rows[0]["BeginTime"].ToString();
        dt.Rows[0]["EndDateTime"] = dtParams.Rows[0]["EndTime"].ToString();
        dt.Rows[0]["Reserve"] = getLSH();//利用保留域传递包头中的流水号

        //调用银行接口
        DataSet ds = Go(dt);     
        if (ds == null)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "调用银行接口失败！";
            return dsreturn;
            //ds = KXH_Test(dsreturn);
        }
        //处理返回结果
        if (ds.Tables["结果"].Rows[0]["反馈结果"].ToString() == "ok")
        {
            if (ds.Tables["包头"].Rows[0]["RspCode"].ToString() == "000000")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows [0]["提示文本"] = "成功";
                dsreturn.Tables["返回值单条"].Rows [0]["附件信息1"]= ds.Tables["包体"].Rows[0]["FileName"].ToString();//银行生成的对账文件名
                DataTable dtTou = ds.Tables["包头"].Copy();
                DataTable dtTi = ds.Tables["包体"].Copy();
                dtTou.TableName = "包头";
                dtTi.TableName = "包体";
                dsreturn.Tables.Add(dtTou);
                dsreturn.Tables.Add(dtTi);
                return dsreturn;
            }
            else
            {
                string errmsg = ds.Tables["包头"].Rows[0]["RspCode"].ToString() + "：" + ds.Tables["包头"].Rows[0]["RspMsg"].ToString().Trim();
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = errmsg;
            }
            return dsreturn;
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = ds.Tables["结果"].Rows[0]["详情"].ToString();
            return dsreturn;
        }       
       
    }
    /// <summary>
    ///进行数据处理
    /// </summary>
    /// <param name="dsParams"></param>
    /// <param name="FuncId"></param>
    /// <param name="filename"></param>
    /// <param name="dsreturn"></param>
    /// <returns></returns>
    public DataSet DataProcess(DataTable dsParams, string FuncId, string filename, DataSet dsreturn)
    {
        switch (FuncId)
        {
            case "清算失败"://查看清算失败文件
                dsreturn = DealWithQSFail(dsParams, filename, dsreturn);
                break;
            case "2"://会员余额对账文件  
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "暂不支持此类功能";
                break;
            case "出入金"://出入金流水文件
                dsreturn = MatchCRJ(dsParams, filename, dsreturn);
                break;
            case "开销户"://会员开销户文件
                dsreturn = MatchKXH(dsParams, filename, dsreturn);
                break;
            case "对账不平"://对账不平记录文件
                dsreturn = DealWithDZBP(dsParams, filename, dsreturn);
                break;
            case "清算成功":
                dsreturn = DealWithQSCG(dsParams, filename, dsreturn);
                break;
            default:
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "功能标志FuncFlag错误";
                break;
        }
        return dsreturn;
    }
    /// <summary>
    /// 开销户数据匹配
    /// </summary>
    /// <param name="dtParams">未格式化的银行数据</param>
    /// <param name="filename">银行文件名</param>
    /// <param name="dsreturn">返回数据集</param>
    /// <returns>匹配结果和原始对账数据</returns>
    private DataSet MatchKXH(DataTable dtParams, string filename, DataSet dsreturn)
    {
        //拆分数据
        //会员开销户流水文件格式：
        //命名规则： KXH+ ThirdLogNo CustLog+交易网代码（4位）+时间（14位）
        //第一行：总笔数
        //第二行以后为正文：（字段解释同【1016】接口）
        //序号&前置流水号&交易状态&子账户账号&子账户性质&子账户名称&交易网会员代码&交易日期&操作柜员号&
        DataTable dtBank = new DataTable();
        dtBank.Columns.Add("序号");
        dtBank.Columns.Add("前置流水号");
        dtBank.Columns.Add("交易状态");
        dtBank.Columns.Add("子账户账号");
        dtBank.Columns.Add("子账户性质");
        dtBank.Columns.Add("子账户名称");
        dtBank.Columns.Add("交易网会员代码");
        dtBank.Columns.Add("交易日期");
        dtBank.Columns.Add("操作柜员号");
       
        //匹配日期
        //string matchDate = DateTime.Now.ToString("yyyyMMdd");
        string matchDate ="20140510";
        string totalNum = "0";
        if (filename != "none.txt")
        {
            matchDate = filename.Substring(filename.Length - 18, 8);
            //数据表的第一行是总数量
            totalNum = dtParams.Rows[0]["数据"].ToString();
            if (Convert.ToInt32(totalNum) != dtParams.Rows.Count - 1)
            {//判断银行文件中的总笔数与实际笔数是否一致
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "银行文件中的总笔数与实际笔数不符。总笔数：" + totalNum + ";实际笔数：" + (dtParams.Rows.Count - 1).ToString();
                return dsreturn;
            }
        }       

        //拆分每行字符串生成datatable
        string info = "";
        dtBank = SplitString(dtParams, dtBank, ref info);
        if (info.Trim() != "")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "银行数据格式化错误。"+info;          
            return dsreturn;
        }
              
        //将拆分后的数据写数据库
          DataSet ds_file = DbHelperSQL.Query("select * from AAA_PingAnDZKXH where FileName='" + filename + "' and DZRQ='" + matchDate + "'");
          if (ds_file != null && ds_file.Tables[0].Rows.Count > 0)
          {
              dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
              dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "数据库中已存在对应文件数据";             
              return dsreturn;
          }

        ArrayList alBank = new ArrayList();
        //写表头
        WorkFlowModule WFM = new WorkFlowModule("AAA_PingAnDZKXH");
        string KeyNumber = WFM.numberFormat.GetNextNumber();
        string insert_zhu = "insert into AAA_PingAnDZKXH([Number],[DZRQ],[FileName],[TotalCount],[CheckState],[CreateUser],[CreateTime],[CheckLimitTime]) values ('" + KeyNumber + "','" + matchDate + "','" + filename + "','" + totalNum + "',1,'admin','" + DateTime.Now.ToString() + "','" + DateTime.Now.ToString() + "')";
        alBank.Add(insert_zhu);
        for (int i = 0; i < dtBank.Rows.Count; i++)
        {//序号&前置流水号&交易状态&子账户账号&子账户性质&子账户名称&交易网会员代码&交易日期&操作柜员号&
            string insert_zi = "insert into AAA_PingAnDZ_KXHSJ([parentNumber],[XH],[FrontLogNo],[UserStatus],[CustAcctId],[CustFlag],[CustName],[ThirdCustId],[TranDate],[CounterId]) values ('" + KeyNumber + "','" + dtBank.Rows[i]["序号"].ToString() + "','" + dtBank.Rows[i]["前置流水号"].ToString() + "','" + dtBank.Rows[i]["交易状态"].ToString() + "','" + dtBank.Rows[i]["子账户账号"].ToString() + "','" + dtBank.Rows[i]["子账户性质"].ToString() + "','" + dtBank.Rows[i]["子账户名称"].ToString() + "','" + dtBank.Rows[i]["交易网会员代码"].ToString() + "','" + dtBank.Rows[i]["交易日期"].ToString() + "','" + dtBank.Rows[i]["操作柜员号"].ToString() + "')";
            alBank.Add(insert_zi);
        }

        bool succ = DbHelperSQL.ExecSqlTran(alBank);
        if (succ == false)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "开销户流水数据写数据库失败";
            return dsreturn;
        }

       
        //处理源数据表的结构
        DataTable dtSource = dtBank.Copy();
        dtSource.Columns.Remove("操作柜员号");
        DataColumn dc = new DataColumn();
        dc.ColumnName = "来源";
        dc.DefaultValue = "银行";
        dtSource.Columns.Add(dc);
      
        //获取本地数据表
        DataSet dsLocal = DbHelperSQL.Query("select cast(ROW_NUMBER() over(order by KHLSH) as varchar(10)) as 序号, KHLSH as 前置流水号,UserStatus as 交易状态,CustAcctId as 子账户账号,CustFlag as 子账户性质, CustName as 子账户名称,ThirdCustId as 交易网会员代码,KHRQ as 交易日期,'本地' as 来源 from AAA_PingAnBank where KHRQ='"+matchDate+"'");

        if (dtSource.Columns.Count != dsLocal.Tables[0].Columns.Count)
        {//如果列不一致的话，返回失败
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "本地数据与银行数据结构不一致，无法进行匹配";
            return dsreturn;
        }
        
        #region 开始跟当前系统的数据进行对比
        //将银行数据加入返回数据集
        dtSource.TableName = "银行数据";
        dsreturn.Tables.Add(dtSource);
        //将本机数据加入返回数据集
        DataTable dtL = dsLocal.Tables[0].Copy();
        dtL.TableName = "本地数据";
        dsreturn.Tables.Add(dtL);

        DataTable dtRes = dtSource.Clone();//异常数据集
        string[] keyCol = new string[] { "前置流水号", "子账户账号", "交易网会员代码" };
       //进行数据和本地数据匹配
        dtRes = compareDT(dtSource, dsLocal.Tables[0], keyCol);
       
        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "匹配完成";
       
        //将异常数据加入返回数据集
        dtRes.TableName = "异常数据";
        dsreturn.Tables.Add(dtRes);     

        return dsreturn;

        #endregion


    }

    /// <summary>
    /// 出入金数据匹配
    /// </summary>
    /// <param name="dtParams">未格式化的银行数据</param>
    /// <param name="filename">银行文件名</param>
    /// <param name="dsreturn">返回数据集</param>
    /// <returns>匹配结果和原始对账数据</returns>
    private DataSet MatchCRJ(DataTable dtParams, string filename, DataSet dsreturn)
    {
        //格式化数据
        //会员出入金流水文件格式：
        //命名规则：CRJ+ ThirdLogNo TransLog+交易网代码（4位）+时间（14位）
        //第一行：总笔数&出金总金额&入金总金额&挂账总金额（存管模式没有挂账，默认为0）        
        //序号&记账标志(1：出金 2：入金 3：挂账)&处理标志(挂账才有值)&付款人账号&收款人账号&交易网会员代码&子账户&子账户名称&交易金额&手续费&支付手续费子账号&支付子账号名称&交易日期&交易时间&银行前置流水号&
        DataTable dtBank = new DataTable();
        dtBank.Columns.Add("序号");
        dtBank.Columns.Add("记账标志");
        dtBank.Columns.Add("处理标志");
        dtBank.Columns.Add("付款人账号");
        dtBank.Columns.Add("收款人账号");
        dtBank.Columns.Add("交易网会员代码");
        dtBank.Columns.Add("子账户");
        dtBank.Columns.Add("子账户名称");
        dtBank.Columns.Add("交易金额");
        dtBank.Columns.Add("手续费");
        dtBank.Columns.Add("支付手续费子账号");
        dtBank.Columns.Add("支付子账号名称");
        dtBank.Columns.Add("交易日期");
        dtBank.Columns.Add("交易时间");
        dtBank.Columns.Add("银行前置流水号");
      
        //数据表的第一行是总数量
        string matchDate = DateTime.Now.ToString("yyyyMMdd");
        string totalNum = "0";
        string totalChuJin = "0.00";
        string totalRuJin = "0.00";
        if (filename != "none.txt")
        {
            //获取文件名中的交易时间，确定本地取何时的数据
            matchDate = filename.Substring(filename.Length - 18, 8);
            string[] firstLine = dtParams.Rows[0]["数据"].ToString().Split('&');
            totalNum = firstLine[0].ToString();//第一个值为总笔数
            totalChuJin = firstLine[1].ToString();//第二个值是总出金金额
            totalRuJin = firstLine[2].ToString();//第三个值是总入金金额
            if (Convert.ToInt32(totalNum) != dtParams.Rows.Count - 1)
            {//判断总笔数与实际笔数是否一致
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "银行文件中的总笔数与实际笔数不符。总笔数：" + totalNum + ";实际笔数：" + (dtParams.Rows.Count - 1).ToString();
                return dsreturn;
            }
        }       

        //拆分第二行开始的每行字符串生成datatable
        string info = "";
        dtBank = SplitString(dtParams, dtBank, ref info);
        if (info.Trim() != "")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "数据拆分错误。"+info;          
            return dsreturn;
        }

        //将拆分后的数据写数据库
        DataSet ds_file = DbHelperSQL.Query("select * from AAA_PingAnDZCRJ where FileName='" + filename + "' and DZRQ='" + matchDate + "'");
        if (ds_file != null && ds_file.Tables[0].Rows.Count > 0)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "数据库中已存在对应文件数据";
            return dsreturn;
        }

        ArrayList alBank = new ArrayList();
        //写表头
        WorkFlowModule WFM = new WorkFlowModule("AAA_PingAnDZCRJ");
        string KeyNumber = WFM.numberFormat.GetNextNumber();
        string insert_zhu = "insert into AAA_PingAnDZCRJ([Number],[DZRQ],[FileName],[TotalCount],[CheckState],[CreateUser],[CreateTime],[CheckLimitTime]) values ('" + KeyNumber + "','" + matchDate + "','" + filename + "','" + totalNum + "',1,'admin','" + DateTime.Now.ToString() + "','" + DateTime.Now.ToString() + "')";
        alBank.Add(insert_zhu);

        for (int i = 0; i < dtBank.Rows.Count; i++)
        {//序号&记账标志(1：出金 2：入金 3：挂账)&处理标志(挂账才有值)&付款人账号&收款人账号&交易网会员代码&子账户&子账户名称&交易金额&手续费&支付手续费子账号&支付子账号名称&交易日期&交易时间&银行前置流水号&
            string insert_zi = "insert into AAA_PingAnDZCRJSJ([parentNumber],[XH],[JZBZ],[CLBZ],[FKRZH],[SKRZH],[JYWHYDM],[ZZH],[ZZHMC],[JYJE],[SXF],[ZFSXFZZH],[ZFZZHMC],[JYRQ],[JYSJ],[YHQZLSH]) values ('" + KeyNumber + "','" + dtBank.Rows[i]["序号"].ToString() + "','" + dtBank.Rows[i]["记账标志"].ToString() + "','" + dtBank.Rows[i]["处理标志"].ToString() + "','" + dtBank.Rows[i]["付款人账号"].ToString() + "','" + dtBank.Rows[i]["收款人账号"].ToString() + "','" + dtBank.Rows[i]["交易网会员代码"].ToString() + "','" + dtBank.Rows[i]["子账户"].ToString() + "','" + dtBank.Rows[i]["子账户名称"].ToString() + "','" + dtBank.Rows[i]["交易金额"].ToString() + "','" + dtBank.Rows[i]["手续费"].ToString() + "','" + dtBank.Rows[i]["支付手续费子账号"].ToString() + "','" + dtBank.Rows[i]["支付子账号名称"].ToString() + "','" + dtBank.Rows[i]["交易日期"].ToString() + "','" + dtBank.Rows[i]["交易时间"].ToString() + "','" + dtBank.Rows[i]["银行前置流水号"].ToString() + "')";
            alBank.Add(insert_zi);
        }

        bool succ = DbHelperSQL.ExecSqlTran(alBank);
        if (succ == false)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "出入金流水数据写数据库失败";
            return dsreturn;
        }
       
        //处理源数据表的结构
        DataTable dtSource = dtBank.Copy();       
        DataColumn dc = new DataColumn();
        dc.ColumnName = "来源";
        dc.DefaultValue = "银行";
        dtSource.Columns.Add(dc);

        //获取本地数据表
        DataSet dsLocal = DbHelperSQL.Query("select cast(ROW_NUMBER() over(order by 银行前置流水号) as varchar(10)) as 序号, 记账标志,处理标志,付款人账号,收款人账号,交易网会员代码,子账户,子账户名称,交易金额,手续费,支付手续费子账号,支付子账号名称,交易日期,交易时间,银行前置流水号, 来源 from AAA_View_PingAnDZCRJ where 交易日期='" + matchDate + "'");

        if (dtSource.Columns.Count != dsLocal.Tables[0].Columns.Count)
        {//如果列不一致的话，返回失败
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "本地数据与银行数据结构不一致，无法进行匹配";
            return dsreturn;
        }

        //将银行数据加入返回数据集
        dtSource.TableName = "银行数据";
        dsreturn.Tables.Add(dtSource);
        //将本机数据加入返回数据集
        DataTable dtL = dsLocal.Tables[0].Copy();
        dtL.TableName = "本地数据";
        //将匹配使用的原始数据返回
        dsreturn.Tables.Add(dtL);

        #region 开始跟当前系统的数据进行对比
        DataTable dtRes = dtSource.Clone();//异常数据集
        string[] keyCol = new string[] {"交易网会员代码","子账户","银行前置流水号"};//查找数据的主要字段名
        
        //进行数据和本地数据匹配
        dtRes = compareDT(dtSource, dsLocal.Tables[0], keyCol);

        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "匹配完成";

        //将异常数据加入返回数据集
        dtRes.TableName = "异常数据";
        dsreturn.Tables.Add(dtRes);

        return dsreturn;

        #endregion


    }
    /// <summary>
    /// 处理清算失败文件，将数据写数据库并更新成功会员的清算状态
    /// </summary>
    /// <param name="dtParams"></param>
    /// <param name="filename"></param>
    /// <param name="dsreturn"></param>
    /// <returns></returns>
    private DataSet DealWithQSFail(DataTable dtParams, string filename, DataSet dsreturn)
    {
        //获取清算数据表的基本结构
        DataTable dtBank = DbHelperSQL.Query("select * from AAA_PingAnQSFailSJ where 1!=1").Tables[0];
        dtBank.Columns.Remove("parentnumber");
        dtBank.Columns.Remove("id");

        //获取文件名中的交易时间，确定本地取何时的数据
        string matchDate = filename.Substring(filename.Length - 18, 8);

        //数据表的第一行是总数量、浮动盈亏总额
        string[] firstLine = dtParams.Rows[0]["数据"].ToString().Split('&');
        string totalNum = firstLine[0].ToString();//第一个值为总笔数
        string totalFDYK = firstLine[1].ToString();//第二个值是浮动盈亏总额

        if (Convert.ToInt32(totalNum) != dtParams.Rows.Count - 1)
        {//判断总笔数与实际笔数是否一致
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "文件中的总笔数与实际笔数不符。总笔数：" + totalNum + ";实际笔数：" + (dtParams.Rows.Count - 1).ToString();
            return dsreturn;
        }

        //拆分第二行开始的每行字符串生成datatable
        string info = "";
        dtBank = SplitString(dtParams, dtBank, ref info);
        if (info.Trim() != "")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "数据拆分错误。" + info;
            return dsreturn;
        }

        //将拆分后的数据写数据库
        DataSet ds_file = DbHelperSQL.Query("select * from AAA_PingAnQSFail where FileName='" + filename + "' and SBSJ='" + matchDate + "'");
        if (ds_file != null && ds_file.Tables[0].Rows.Count > 0)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "数据库中已存在对应文件数据";
            return dsreturn;
        }

        ArrayList alBank = new ArrayList();
        //写表头
        WorkFlowModule WFM = new WorkFlowModule("AAA_PingAnQSFail");
        string KeyNumber = WFM.numberFormat.GetNextNumber();
        string insert_zhu = "insert into AAA_PingAnQSFail([Number],[SBSJ],[FileName],[TotalCount],[FDYK],[CheckState],[CreateUser],[CreateTime],[CheckLimitTime]) values ('" + KeyNumber + "','" + matchDate + "','" + filename + "','" + totalNum + "','" + totalFDYK + "',1,'admin','" + DateTime.Now.ToString() + "','" + DateTime.Now.ToString() + "')";
        alBank.Add(insert_zhu);

        string allid = "";//所有清算失败的人的客户编号
        for (int i = 0; i < dtBank.Rows.Count; i++)
        {//写子表
            if (i == 0)
            {
                allid = "'" + dtBank.Rows[i]["ThirdCustId"].ToString() + "'";
            }
            else
            {
                allid = allid + ",'" + dtBank.Rows[i]["ThirdCustId"].ToString() + "'";
            }
            string insert_zi = "insert into AAA_PingAnQSFailSJ([parentNumber],[QueryId],[TranDateTime],[CounterId],[SupAcctId],[FuncCode],[CustAcctId],[CustName],[ThirdCustId],[ThirdLogNo],[CcyCode],[FreezeAmount],[UnfreezeAmount],[AddTranAmount],[CutTranAmount],[ProfitAmount],[LossAmount],[TranHandFee],[TranCount],[NewBalance],[NewFreezeAmount],[Note],[Reserve],[RspCode],[RspMsg]) values ('" + KeyNumber + "','" + dtBank.Rows[i]["QueryId"].ToString() + "','" + dtBank.Rows[i]["TranDateTime"].ToString() + "','" + dtBank.Rows[i]["CounterId"].ToString() + "','" + dtBank.Rows[i]["SupAcctId"].ToString() + "','" + dtBank.Rows[i]["FuncCode"].ToString() + "','" + dtBank.Rows[i]["CustAcctId"].ToString() + "','" + dtBank.Rows[i]["CustName"].ToString() + "','" + dtBank.Rows[i]["ThirdCustId"].ToString() + "','" + dtBank.Rows[i]["ThirdLogNo"].ToString() + "','" + dtBank.Rows[i]["CcyCode"].ToString() + "','" + dtBank.Rows[i]["FreezeAmount"].ToString() + "','" + dtBank.Rows[i]["UnfreezeAmount"].ToString() + "','" + dtBank.Rows[i]["AddTranAmount"].ToString() + "','" + dtBank.Rows[i]["CutTranAmount"].ToString() + "','" + dtBank.Rows[i]["ProfitAmount"].ToString() + "','" + dtBank.Rows[i]["LossAmount"].ToString() + "','" + dtBank.Rows[i]["TranHandFee"].ToString() + "','" + dtBank.Rows[i]["TranCount"].ToString() + "','" + dtBank.Rows[i]["NewBalance"].ToString() + "','" + dtBank.Rows[i]["NewFreezeAmount"].ToString() + "','" + dtBank.Rows[i]["Note"].ToString() + "','" + dtBank.Rows[i]["Reserve"].ToString() + "','" + dtBank.Rows[i]["RspCode"].ToString() + "','" + dtBank.Rows[i]["RspMsg"].ToString() + "')";
            alBank.Add(insert_zi);
        }

        //更新清算成功会员的原始清算数据状态
        string sql_upd = "update AAA_PingAnQSZTJLB set QSZT='成功' where QSZT='已发送' and SJCSSSSYH='平安银行' and PTKHBH not in (" + allid + ")";
        alBank.Add(sql_upd);

        try
        {
            DbHelperSQL.ExecSqlTran(alBank);
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "数据处理成功";
            return dsreturn;
        }
        catch (Exception ex)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "数据处理失败。" + ex.ToString();
            return dsreturn;
        }
    }
    /// <summary>
    /// 处理对账不平文件，将数据写数据库
    /// </summary>
    /// <param name="dtParams"></param>
    /// <param name="filename"></param>
    /// <param name="dsreturn"></param>
    /// <returns></returns>
    private DataSet DealWithDZBP(DataTable dtParams, string filename, DataSet dsreturn)
    {
        //格式化数据
        //对账不平文件格式：
        //命名规则：BatCustDzFail+交易网代码（4位）+时间（14位）.txt
        //第一行：总笔数      
        //子账号&子账号名称&会员代码&银行清算后可用余额&银行清算后冻结余额&交易网可用余额&交易网冻结余额&可用余额差额（银行可用余额-交易网可用余额）&冻结余额差额（银行冻结余额-交易网冻结余额）&对账结果说明&
        DataTable dtBank = new DataTable();
        dtBank.Columns.Add("子账号");
        dtBank.Columns.Add("子账号名称");
        dtBank.Columns.Add("会员代码");
        dtBank.Columns.Add("银行可用余额");
        dtBank.Columns.Add("银行冻结余额");
        dtBank.Columns.Add("交易网可用余额");
        dtBank.Columns.Add("交易网冻结余额");
        dtBank.Columns.Add("可用余额差");
        dtBank.Columns.Add("冻结余额差");
        dtBank.Columns.Add("对账结果说明");

        //获取文件名中的交易时间，确定本地取何时的数据
        string matchDate = filename.Substring(filename.Length - 18, 8);
        string totalNum = dtParams.Rows[0]["数据"].ToString();//第一行为总笔数

        if (Convert.ToInt32(totalNum) != dtParams.Rows.Count - 1)
        {//判断总笔数与实际笔数是否一致
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "文件中的总笔数与实际笔数不符。总笔数：" + totalNum + ";实际笔数：" + (dtParams.Rows.Count - 1).ToString();
            return dsreturn;
        }

        //拆分第二行开始的每行字符串生成datatable

        string info = "";
        dtBank = SplitString(dtParams, dtBank, ref info);
        if (info.Trim() != "")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "数据拆分错误。" + info;
            return dsreturn;
        }

        //将拆分后的数据写数据库
        DataSet ds_file = DbHelperSQL.Query("select * from AAA_PingAnDZBP where FileName='" + filename + "' and DZSJ='" + matchDate + "'");
        if (ds_file != null && ds_file.Tables[0].Rows.Count > 0)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "数据库中已存在对应文件数据";
            return dsreturn;
        }

        ArrayList alBank = new ArrayList();
        //写表头
        WorkFlowModule WFM = new WorkFlowModule("AAA_PingAnDZBP");
        string KeyNumber = WFM.numberFormat.GetNextNumber();
        string insert_zhu = "insert into AAA_PingAnDZBP([Number],[DZSJ],[FileName],[TotalCount],[CheckState],[CreateUser],[CreateTime],[CheckLimitTime]) values ('" + KeyNumber + "','" + matchDate + "','" + filename + "','" + totalNum + "',1,'admin','" + DateTime.Now.ToString() + "','" + DateTime.Now.ToString() + "')";
        alBank.Add(insert_zhu);

        for (int i = 0; i < dtBank.Rows.Count; i++)
        {//子账号&子账号名称&会员代码&银行清算后可用余额&银行清算后冻结余额&交易网可用余额&交易网冻结余额&可用余额差额（银行可用余额-交易网可用余额）&冻结余额差额（银行冻结余额-交易网冻结余额）&对账结果说明&
            string insert_zi = "insert into AAA_PingAnDZBPSJ([parentNumber],[ZZH],[ZZHMC],[JYWHYDM],[YHKYYE],[YHDJYE],[JYWKYYE],[JYWDJYE],[KYYEC],[DJYEC],[DZJGSM]) values ('" + KeyNumber + "','" + dtBank.Rows[i]["子账号"].ToString() + "','" + dtBank.Rows[i]["子账号名称"].ToString() + "','" + dtBank.Rows[i]["会员代码"].ToString() + "','" + dtBank.Rows[i]["银行可用余额"].ToString() + "','" + dtBank.Rows[i]["银行冻结余额"].ToString() + "','" + dtBank.Rows[i]["交易网可用余额"].ToString() + "','" + dtBank.Rows[i]["交易网冻结余额"].ToString() + "','" + dtBank.Rows[i]["可用余额差"].ToString() + "','" + dtBank.Rows[i]["冻结余额差"].ToString() + "','" + dtBank.Rows[i]["对账结果说明"].ToString() + "')";
            alBank.Add(insert_zi);
        }
        try
        {
            DbHelperSQL.ExecSqlTran(alBank);
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "对账不平数据已写入数据库";
            dtBank.TableName = "data";
            dsreturn.Tables.Add(dtBank);
            return dsreturn;
        }
        catch (Exception ex)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "数据处理失败。" + ex.ToString();
            return dsreturn;
        }
    }
    /// <summary>
    /// 清算成功后更新原始清算数据中的清算状态
    /// </summary>
    /// <param name="dtParams"></param>
    /// <param name="dsreturn"></param>
    /// <returns></returns>
    private DataSet DealWithQSCG(DataTable dtParams, string filename, DataSet dsreturn)
    {
        //更新清算成功会员的原始清算数据状态
        string sql_upd = "update AAA_PingAnQSZTJLB set QSZT='成功' where QSZT='已发送' and SJCSSSSYH='平安银行'";
        try
        {
            DbHelperSQL.ExecuteSql(sql_upd);
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "原始清算数据状态更新成功";
            return dsreturn;
        }
        catch (Exception ex)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "原始清算数据状态更新失败！" + ex.ToString();
            return dsreturn;
        }
    
    }


    /// <summary>
    /// 拆字符串
    /// </summary>
    /// <param name="dtParams">从文件读取的数据</param>
    /// <param name="dtSource">返回的数据集</param>
    /// <returns></returns>
    private DataTable SplitString(DataTable dtParams,DataTable dtSource,ref string info)
    {
        if (dtParams.Rows.Count <=0)
        {
            return dtSource;
        }

        //返回的数据文件第一行是总数据量，从第二行开始才是正式数据       
        for (int i = 1; i < dtParams.Rows.Count; i++)
        {
            string[] strAry = dtParams.Rows[i]["数据"].ToString().Split(new string[] { "&" }, StringSplitOptions.None);
            //因为原来的数据最后以“&”结尾，故最后一个值为空，需要忽略掉
            if (strAry.Length > 0 && (strAry.Length - 1) == dtSource.Columns.Count)
            {
                //给拆分结果集增加一个新行
                dtSource.Rows.Add(new object[] { "", "", "", "", "", "", "", "", "" });
                
                for (int j = 0; j < strAry.Length-1; j++)
                {//将拆分出的数据保存到dt中
                    dtSource.Rows[i - 1][j] = strAry[j].ToString().Trim();
                }
            }
            else
            {
                info =info+"/第"+i.ToString ()+"行数据拆分错误";
            }
        }
        return dtSource;
    }


    #region 两个结构相同的DataTable比较
    /// <summary>
    /// 两个结构相同的datatable比较
    /// </summary>
    /// <param name="dtSource">待比较数据1</param>
    /// <param name="dtLocal">待比较数据2</param>
    /// <param name="KeyCol">比较的主要字段</param>
    /// <param name="dtRes">返回数据集</param>
    /// <returns>两表中的不同数据</returns>
    private DataTable compareDT(DataTable dtSource, DataTable dtLocal, string[] KeyCol)
    {
        DataTable dtRes = dtLocal.Clone();//比较结果数据集

        if (dtLocal == null || dtLocal.Rows.Count <= 0)
        {//如果本地表是空，则直接返回源数据表
            dtRes = dtSource.Copy();
            return dtRes;
        }
        
        //以源数据表为基础，判断数据的异同
        for (int i = 0; i < dtSource.Rows.Count; i++)
        {
            //通过银行前置流水号、会员代码、子账户、记账类型确定数据
            DataRow drS = dtSource.Rows[i];            
            string tiaojian = "";
            for (int j = 0; j < KeyCol.Length; j++)
            {
                if (j == 0)
                {
                    tiaojian = KeyCol[j] + "='" + dtSource.Rows[i][KeyCol[j].ToString()].ToString() + "'";
                }
                else
                {
                    tiaojian = tiaojian + " and " + KeyCol[j] + "='" + dtSource.Rows[i][KeyCol[j].ToString()].ToString() + "'";
                }
            }
            DataRow[] drL = dtLocal.Select(tiaojian);

            if (drL.Length == 0)
            {//如果在本地表中找不到对应数据，则将原数据添加到结果集中
                DataRow drnew = dtRes.NewRow();
                drnew.ItemArray = drS.ItemArray;
                dtRes.Rows.Add(drnew);
            }
            else
            {   //如果本地表中有对应数据，则判断两表的每个字段是否都相同 
                DataRow drA = dtSource.NewRow();
                drA.ItemArray = drS.ItemArray;
                drA["序号"] = "";
                drA["来源"] = "";

                DataRow drB = dtLocal.NewRow();
               //如果有多行，只考虑第一行，其余的作为异常数据处理。
               drB.ItemArray = drL[0].ItemArray;
               drB["序号"] = "";
               drB["来源"] = "";                

                //比较两条数据是否相等
                bool equals=DataRowComparer<DataRow>.Default.Equals(drA, drB);             

                if (!equals)
                {
                    //如果不相同，则将两个数据都加入结果集
                    DataRow drnewS = dtRes.NewRow();
                    drnewS.ItemArray = drS.ItemArray;
                    dtRes.Rows.Add(drnewS);

                    DataRow drnewL = dtRes.NewRow();
                    drnewL.ItemArray = drL[0].ItemArray;
                    dtRes.Rows.Add(drnewL);
                }

                //相同与否，都需要删除本地表中的数据                
                dtLocal.Rows.Remove(drL[0]);
            }
        }
        //源表处理完成后，目的表中剩余的数据就应该全部是不相同的
        if (dtLocal.Rows.Count > 0)
        {
            foreach (DataRow drL in dtLocal.Rows)
            {
                DataRow dr = dtRes.NewRow();
                dr.ItemArray = drL.ItemArray;
                dtRes.Rows.Add(dr);
            }
        }
        return dtRes;
    }
    #endregion


    /// <summary>
    /// 交易网触发银行进行清算与对账/获取会员余额文件【1003】
    /// </summary>
    /// <param name="dtParams">传入参数集</param>    
    /// <param name="dsreturn">返回数据集</param>
    /// <returns>执行结果</returns>
    public DataSet RunQingSuan(DataTable dtParams, DataSet dsreturn)
    {
        //获取1003的xml包体信息           
        FileInfo FI = new FileInfo(HttpContext.Current.Server.MapPath("BankXML/XMLPingan/Us_Send_1003.xml"));
        DataTable dt = new DataTable();
        dt.ReadXml(FI.FullName);
      
        //批量标识	FuncFlag	C(1)	必输	1：清算； 3：对账（用于清算文件与余账文件分开的情况）
        //批量文件名	FileName	C(32)	可选	只是文件名，ftp的目录固定；文件命名规则见【4001】
        //若清算，则必输
        //文件大小	FileSize	C(10)	可选	若清算，则必输
        //资金汇总账号	SupAcctId	C(32)	必输	
        //清收买卖货款扎差金额	QsZcAmount	9(18)	可选	若清算，则必输（∑买-∑卖）
        //冻结总金额	FreezeAmount	9(18)	可选	若清算，则必输（∑冻结保证金金额）
        //解冻总金额	UnfreezeAmount	9(18)	可选	若清算，则必输（∑释放保证金金额）
        //损益扎差数	SyZcAmount	9(18)	可选	若清算，则必输（∑受益-∑亏损）
        //保留域	Reserve	C(120)	可选	
        dt.Rows[0]["FuncFlag"] = dtParams.Rows[0]["FuncFlag"].ToString();//清算              
        dt.Rows[0]["FileName"] = dtParams.Rows[0]["FileName"].ToString();
        dt.Rows[0]["FileSize"] = dtParams.Rows[0]["FileSize"].ToString();
        //dt.Rows[0]["SupAcctId"] = dtParams.Rows[0]["SupAcctId"].ToString();//XML文件中预存，不需修改
        dt.Rows[0]["QsZcAmount"] = dtParams.Rows[0]["QsZcAmount"].ToString();
        dt.Rows[0]["FreezeAmount"] = dtParams.Rows[0]["FreezeAmount"].ToString();
        dt.Rows[0]["UnfreezeAmount"] = dtParams.Rows[0]["UnfreezeAmount"].ToString();
        dt.Rows[0]["SyZcAmount"] = dtParams.Rows[0]["SyZcAmount"].ToString();
        dt.Rows[0]["Reserve"] = getLSH();//利用保留域传递包头中的流水号

        //调用银行接口
        DataSet ds = Go(dt);
        if (ds == null)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "调用银行接口失败！";
            return dsreturn;
           // ds = RunQS_Test(dsreturn);
        }
        //处理返回结果
        if (ds.Tables["结果"].Rows[0]["反馈结果"].ToString() == "ok")
        {
            if (ds.Tables["包头"].Rows[0]["RspCode"].ToString() == "000000")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "成功";
               
                DataTable dtTou = ds.Tables["包头"].Copy();
                DataTable dtTi = ds.Tables["包体"].Copy();
                dtTou.TableName = "包头";
                dtTi.TableName = "包体";
                dsreturn.Tables.Add(dtTou);
                dsreturn.Tables.Add(dtTi);
                return dsreturn;
            }
            else
            {
                string errmsg = ds.Tables["包头"].Rows[0]["RspCode"].ToString() + "：" + ds.Tables["包头"].Rows[0]["RspMsg"].ToString().Trim();
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = errmsg;
            }
            return dsreturn;
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = ds.Tables["结果"].Rows[0]["详情"].ToString();
            return dsreturn;
        }

    }


    /// <summary>
    /// 交易网查询银行清算与对账/获取会员余额文件的进度【1004】
    /// </summary>
    /// <param name="dtParams">传入参数集</param>    
    /// <param name="dsreturn">返回数据集</param>
    /// <returns>执行结果</returns>
    public DataSet BankStateQuery(string type, string date, DataSet dsreturn)
    {
        //获取1004的xml包体信息           
        FileInfo FI = new FileInfo(HttpContext.Current.Server.MapPath("BankXML/XMLPingan/Us_Send_1004.xml"));
        DataTable dt = new DataTable();
        dt.ReadXml(FI.FullName);

        //批量任务标识	FuncFlag	C(1) 必输	1：清算； 3：对账（用于清算文件与余账文件分开的情况） 4：出入金流水 5：开销户流水
        //起始日期	BeginDate	C(8)	必输	20080428代表2008年4月28日，若查询当天的交易，则起始日期和结束日期均输当天日期
        //结束日期	EndDate	C(8)	必输	同起始日期，时间跨度不能超过一个月
        //资金汇总账号	SupAcctId	C(32)	必输	
        //保留域	Reserve	C(120)	可选	
        switch (type)
        {
            case "清算":
                dt.Rows[0]["FuncFlag"] = "1";//清算     
                break;
            case "出入金":
                dt.Rows[0]["FuncFlag"] = "4";//出入金流水   
                break;
            case "开销户":
                dt.Rows[0]["FuncFlag"] = "5";//开销户流水  
                break;
            default:
                dt.Rows[0]["FuncFlag"] = "";
                break;
        }
                
        dt.Rows[0]["BeginDate"] = date;
        dt.Rows[0]["EndDate"] = date;
        //dt.Rows[0]["SupAcctId"] = "";//XML文件中预存，不需修改       
        dt.Rows[0]["Reserve"] = getLSH();//利用保留域传递包头中的流水号

        //调用银行接口
        DataSet ds = Go(dt);
        if (ds == null)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "调用银行接口失败！";
            return dsreturn;
            //ds = RunQS1004_Test(dsreturn);
        }
        //处理返回结果
        if (ds.Tables["结果"].Rows[0]["反馈结果"].ToString() == "ok")
        {
            if (ds.Tables["包头"].Rows[0]["RspCode"].ToString() == "000000")
            {

                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                switch(ds.Tables["包体"].Rows[0]["ResultFlag"].ToString())
                {
                    case "1":
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] ="正取批量文件";
                        break;
                    case "2":
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "取批量文件失败";
                        break;
                    case "3":
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "正在读取文件";
                        break;
                    case "4":
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "读取文件失败";
                        break;
                    case "5":
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "正在处理中";
                        break;
                    case "6":
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "处理完成,但部分成功";
                        break;
                    case "7":
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "处理全部失败";
                        break;
                    case "8":
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "处理完全成功";
                        break;
                    case "9":
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "处理完成,但生成处理结果文件失败";
                        break;
                    default:
                        break;
                }
                
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = ds.Tables["包体"].Rows[0]["FailMsg"].ToString().Trim();//失败原因，若有部分失败，或全部失败时的原因
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息2"] = ds.Tables["包体"].Rows[0]["FailFilesName"].ToString().Trim();//失败结果文件名,若完全成功则没此文件

                DataTable dtTou = ds.Tables["包头"].Copy();
                DataTable dtTi = ds.Tables["包体"].Copy();
                dtTou.TableName = "包头";
                dtTi.TableName = "包体";
                dsreturn.Tables.Add(dtTou);
                dsreturn.Tables.Add(dtTi);
                return dsreturn;
            }
            else
            {
                string errmsg = ds.Tables["包头"].Rows[0]["RspCode"].ToString() + "：" + ds.Tables["包头"].Rows[0]["RspMsg"].ToString().Trim();
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = errmsg;
            }
            return dsreturn;
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = ds.Tables["结果"].Rows[0]["详情"].ToString();
            return dsreturn;
        }
    }

    //获取流水号。利用了【银行流水号生成表】
    private string getLSH()
    {
        string number = "";
        object newnumber = DbHelperSQL.GetSingle("update AAA_bankLS set LWSXH = CONVERT(varchar(50), convert(decimal(6,0),isnull(LWSXH,'0'))+1, 0)     select top 1 right ('000000'+LWSXH,6)  from AAA_bankLS");
        if (newnumber != null)
        {
            number = DateTime.Now.ToString("yyyyMMdd") + newnumber.ToString();
        }
        return number;
    } 
    /// <summary>
    /// 平安银行--获取清算数据
    /// </summary>
    /// <returns></returns>
    public DataSet GetQSDataSource()
    {
        DataSet returnDS = initReturnDataSet();
        returnDS.Tables["返回值单条"].Rows.Add(new object[] { "err", "初始化" });

        try
        {
            #region //查询清算数据
            string strSelectSQL = "select '序号'='','交易时间'='','操作员号'='测试','资金汇总账号'=SupAcctId,'功能代码'='30','子账户账号'=CustAcctId,'子账户名称'=CustName,'交易网会员代码'=ThirdCustId,'交易网流水号'='','币种'='RMB','当天总冻结金额'='0.00','当天总解冻金额'='0.00', '当天成交的总货款（作为卖方）'=(select ISNULL(sum(JE),0.00) from AAA_ZKLSMXB b left join AAA_PingAnQSZTJLB c on b.Number=c.Number where SJLX='实' and XM not in ('技术服务费','转入资金','转出资金') and b.DLYX=a.DLYX and b.YSLX in ('解冻','增加') and c.QSZT in ('未处理','已发送') and c.SJCSSSSYH='平安银行'),'当天成交的总货款（作为买方）'=(select ISNULL(sum(JE),0.00) from AAA_ZKLSMXB b left join AAA_PingAnQSZTJLB c on b.Number=c.Number  where SJLX='实' and  XM not in ('技术服务费','转入资金','转出资金') and b.DLYX=a.DLYX and b.YSLX in ('冻结','扣减') and c.QSZT in ('未处理','已发送') and c.SJCSSSSYH='平安银行'),'盈利总金额'='0.00','亏损总金额'='0.00','当天的交易手续费总额'=(select ISNULL(sum(JE),0.00) from AAA_ZKLSMXB b left join AAA_PingAnQSZTJLB c on b.Number=c.Number  where SJLX='实' and  XM ='技术服务费' and b.DLYX=a.DLYX and c.QSZT in ('未处理','已发送') and c.SJCSSSSYH='平安银行'),'当天交易总笔数'=(select count(number) from AAA_PingAnQSZTJLB where (QSZT='未处理' or QSZT='已发送') and SJCSSSSYH='平安银行' and PTKHBH=a.ThirdCustId),'交易网端会员最新的可用余额'=(select ISNULL(B_ZHDQKYYE,0.00) from AAA_DLZHXXB where B_DLYX=a.DLYX),'交易网端会员最新的冻结余额'='0.00','说明'='会员日终扎差清算','保留域'='' from AAA_PingAnBank a  where ZT='正常' order by ROW_NUMBER() over(order by DLYX)";
            DataSet ds = DbHelperSQL.Query(strSelectSQL);
            #endregion

            #region //生成插入清算文件的sql语句
            ArrayList list = new ArrayList();

            //生成平安银行清算文件的Number
            WorkFlowModule WFM = new WorkFlowModule("AAA_PingAnBankQS");
            string PingAnBankQSNumber = WFM.numberFormat.GetNextNumberZZ("");

            double SelZHK = 0.00;
            double BuyZHK = 0.00;
            int ZBS = ds.Tables[0].Rows.Count;//总笔数
            string time = DateTime.Now.ToString("yyyyMMddhhmmss");

            string filename = "";//清算文件名
            if (ds == null || ds.Tables[0].Rows.Count <= 0)
            {
                filename = "none.txt";
            }
            else
            {               
                filename = "BatQs" + ConfigurationManager.AppSettings["PingAnID"].ToString() + time + ".txt";//文件名=BatQs+交易网代码+时间（14位）
               
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {//清算数据写入子表
                    ds.Tables[0].Rows[i]["序号"] = i + 1;
                    ds.Tables[0].Rows[i]["交易时间"] = time;
                    //ds.Tables[0].Rows[i]["当天交易总笔数"] = ZBS;
                    SelZHK += Convert.ToDouble(ds.Tables[0].Rows[i]["当天成交的总货款（作为卖方）"].ToString());
                    BuyZHK += Convert.ToDouble(ds.Tables[0].Rows[i]["当天成交的总货款（作为买方）"].ToString());

                    //交易时间+序号，作为交易网流水号
                    ds.Tables[0].Rows[i]["交易网流水号"] = time + (i + 1).ToString();
                    
                    //向平安银行清算文件子表插入数据：AAA_PingAnBankQSZB
                    string strQSZB = "INSERT INTO [dbo].[AAA_PingAnBankQSZB]  ([parentNumber] ,[XH] ,[JYSJ] ,[CZYH] ,[ZJHZZH] ,[GNDM] ,[ZZHZH] ,[ZZHMC],[JYWHYDM] ,[JYWLSH] ,[BZ] ,[DTZDJJE] ,[DTZJDJE] ,[SaleDTCJDZHK] ,[BuyDTCJDZHK] ,[YLZJE] ,[KSZJE] ,[DTDJYSXFZE] ,[DTJYZBS] ,[JYWDHYZXDKYYE],[JYWDHYZXDDJYE] ,[SM] ,[BLY])  VALUES ('" + PingAnBankQSNumber + "'," + Convert.ToInt32(ds.Tables[0].Rows[i]["序号"].ToString()) + " ,'" + ds.Tables[0].Rows[i]["交易时间"].ToString() + "','" + ds.Tables[0].Rows[i]["操作员号"].ToString() + "','" + ds.Tables[0].Rows[i]["资金汇总账号"].ToString() + "','" + ds.Tables[0].Rows[i]["功能代码"].ToString() + "','" + ds.Tables[0].Rows[i]["子账户账号"].ToString() + "','" + ds.Tables[0].Rows[i]["子账户名称"].ToString() + "','" + ds.Tables[0].Rows[i]["交易网会员代码"].ToString() + "','" + ds.Tables[0].Rows[i]["交易网流水号"].ToString() + "','" + ds.Tables[0].Rows[i]["币种"].ToString() + "','" + ds.Tables[0].Rows[i]["当天总冻结金额"].ToString() + "','" + ds.Tables[0].Rows[i]["当天总解冻金额"].ToString() + "','" + ds.Tables[0].Rows[i]["当天成交的总货款（作为卖方）"].ToString() + "','" + ds.Tables[0].Rows[i]["当天成交的总货款（作为买方）"].ToString() + "','" + ds.Tables[0].Rows[i]["盈利总金额"].ToString() + "','" + ds.Tables[0].Rows[i]["亏损总金额"].ToString() + "','" + ds.Tables[0].Rows[i]["当天的交易手续费总额"].ToString() + "'," + Convert.ToInt32(ds.Tables[0].Rows[i]["当天交易总笔数"].ToString()) + ",'" + ds.Tables[0].Rows[i]["交易网端会员最新的可用余额"].ToString() + "','" + ds.Tables[0].Rows[i]["交易网端会员最新的冻结余额"].ToString() + "','" + ds.Tables[0].Rows[i]["说明"].ToString() + "','" + ds.Tables[0].Rows[i]["保留域"].ToString() + "')";
                    list.Add(strQSZB);                   
                }
            }

            double FDYKHZJE = SelZHK - BuyZHK;//浮动盈亏总金额

            //向平安银行清算文件插入数据：AAA_PingAnBankQS
            string strQS = "INSERT INTO [dbo].[AAA_PingAnBankQS]([Number],[QSSJ],[QSWJM],[ZBS] ,[FDYKHZJE] ,[CheckState] ,[CreateUser] ,[CreateTime] ,[CheckLimitTime]) VALUES ('" + PingAnBankQSNumber + "' ,'" + time + "','" + filename + "'," + ZBS + " ,'" + FDYKHZJE.ToString("0.00") + "',1 ,'平安清算',GETDATE(),GETDATE())";
            list.Add(strQS);
            #endregion

            #region//执行SQL
            if (DbHelperSQL.ExecSqlTran(list))
            {
                returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "获取清算数据成功";
                returnDS.Tables["返回值单条"].Rows[0]["附件信息1"] = ZBS;//总笔数
                returnDS.Tables["返回值单条"].Rows[0]["附件信息2"] = FDYKHZJE.ToString("0.00");//浮动盈亏汇总金额
                returnDS.Tables["返回值单条"].Rows[0]["附件信息3"] = filename;//文件名              

                DataTable dt = ds.Tables[0].Copy();
                dt.TableName = "清算数据";
                returnDS.Tables.Add(dt);

            }
            else
            {
                returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "获取清算数据失败";
            }
            #endregion
        }
        catch (Exception ex)
        {
            returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            returnDS.Tables["返回值单条"].Rows[0]["提示文本"] =ex.Message;
        }
        return returnDS;
    }

    #region 本机测试模拟银行应答
    //模拟签到、签退的银行应答
    private DataSet Sing_Test(string type, DataSet dsreturn)
    {
        //入金方法调用模拟银行返回包
        DataSet dsReturn = new DataSet();
        DataTable dtTiReturn = new DataTable();
        DataTable dtTouReturn = new DataTable();
        dtTiReturn.TableName = "包体";
        dtTouReturn.TableName = "包头";

        dtTouReturn.Columns.Add("ServType");
        dtTouReturn.Columns.Add("RspCode");
        dtTouReturn.Columns.Add("TranDate");
        dtTouReturn.Columns.Add("TranTime");
        dtTouReturn.Columns.Add("RspMsg");

        dtTiReturn.Columns.Add("FrontLogNo");
        dtTiReturn.Rows.Add(new object[] { "111111" });
        dtTouReturn.Rows.Add(new object[] { "02", "000000", "20140429", "164000", "交易成功" });

        dsReturn.Tables.Add(dtTiReturn);
        dsReturn.Tables.Add(dtTouReturn);
        DataTable msg = new DataTable("结果");
        msg.Columns.Add("反馈结果");
        msg.Columns.Add("详情");
        msg.Columns.Add("视觉字符串");
        //msg.Rows.Add(new string[] {"err","接口不通",""});
        msg.Rows.Add(new string[] { "ok", "接口不通", "" });
        dsReturn.Tables.Add(msg);

        return dsReturn;
    }

    //模拟交易网发起入金的银行应答
    private DataSet rujin_Test(DataSet dsParams, BankInit BI)
    {
        //入金方法调用模拟银行返回结果
        DataSet dsReturn = new DataSet();
        DataTable dtTiReturn = new DataTable();
        DataTable dtTouReturn = new DataTable();
        dtTiReturn.TableName = "包体";
        dtTouReturn.TableName = "包头";

        dtTouReturn.Columns.Add("ServType");
        dtTouReturn.Columns.Add("RspCode");
        dtTouReturn.Columns.Add("TranDate");
        dtTouReturn.Columns.Add("TranTime");

        dtTiReturn.Columns.Add("FrontLogNo");
        dtTiReturn.Rows.Add(new object[] { "111111" });
        dtTouReturn.Rows.Add(new object[] { "02", "000000", "20140424", "134000" });

        dsReturn.Tables.Add(dtTiReturn);
        dsReturn.Tables.Add(dtTouReturn);
        DataTable msg = new DataTable("结果");
        msg.Columns.Add("反馈结果");
        msg.Columns.Add("详情");
        msg.Columns.Add("视觉字符串");
        //msg.Rows.Add(new string[] {"err","接口不通",""});
        msg.Rows.Add(new string[] { "ok", "接口不通", "" });
        dsReturn.Tables.Add(msg);

        return dsReturn;
    }
    //模拟交易网发起出金的银行应答
    private DataSet chujin_Test(DataSet dsParams, BankInit BI)
    {
        //入金方法调用模拟银行返回包
        DataSet dsReturn = new DataSet();
        DataTable dtTiReturn = new DataTable();
        DataTable dtTouReturn = new DataTable();
        dtTiReturn.TableName = "包体";
        dtTouReturn.TableName = "包头";

        dtTouReturn.Columns.Add("ServType");
        dtTouReturn.Columns.Add("RspCode");
        dtTouReturn.Columns.Add("TranDate");
        dtTouReturn.Columns.Add("TranTime");
        dtTouReturn.Columns.Add("RspMsg");

        dtTiReturn.Columns.Add("FrontLogNo");
        dtTiReturn.Rows.Add(new object[] { "111111" });
        dtTouReturn.Rows.Add(new object[] { "02", "000002", "20140424", "134000", "转账失败" });

        dsReturn.Tables.Add(dtTiReturn);
        dsReturn.Tables.Add(dtTouReturn);
        DataTable msg = new DataTable("结果");
        msg.Columns.Add("反馈结果");
        msg.Columns.Add("详情");
        msg.Columns.Add("视觉字符串");
        //msg.Rows.Add(new string[] {"err","接口不通",""});
        msg.Rows.Add(new string[] { "error", "接口不通", "" });
        dsReturn.Tables.Add(msg);

        return dsReturn;
    }

    //模拟1006银行应答
    private DataSet KXH_Test(DataSet dsreturn)
    {
        //调用模拟银行返回包
        DataSet dsReturn = new DataSet();
        DataTable dtTiReturn = new DataTable();
        DataTable dtTouReturn = new DataTable();
        dtTiReturn.TableName = "包体";
        dtTouReturn.TableName = "包头";

        dtTouReturn.Columns.Add("ServType");
        dtTouReturn.Columns.Add("RspCode");
        dtTouReturn.Columns.Add("TranDate");
        dtTouReturn.Columns.Add("TranTime");
        dtTouReturn.Columns.Add("RspMsg");

        dtTiReturn.Columns.Add("FileName");
        dtTiReturn.Rows.Add(new object[] { "kxh.txt" });
        dtTouReturn.Rows.Add(new object[] { "02", "000000", "20140429", "164000", "交易成功" });

        dsReturn.Tables.Add(dtTiReturn);
        dsReturn.Tables.Add(dtTouReturn);
        DataTable msg = new DataTable("结果");
        msg.Columns.Add("反馈结果");
        msg.Columns.Add("详情");
        msg.Columns.Add("视觉字符串");
        //msg.Rows.Add(new string[] {"err","接口不通",""});
        msg.Rows.Add(new string[] { "ok", "接口不通", "" });
        dsReturn.Tables.Add(msg);

        return dsReturn;
    }

    //模拟1003银行应答
    private DataSet RunQS_Test(DataSet dsreturn)
    {
        //调用模拟银行返回包
        DataSet dsReturn = new DataSet();
        DataTable dtTiReturn = new DataTable();
        DataTable dtTouReturn = new DataTable();
        dtTiReturn.TableName = "包体";
        dtTouReturn.TableName = "包头";

        dtTouReturn.Columns.Add("ServType");
        dtTouReturn.Columns.Add("RspCode");
        dtTouReturn.Columns.Add("TranDate");
        dtTouReturn.Columns.Add("TranTime");
        dtTouReturn.Columns.Add("RspMsg");

        dtTouReturn.Rows.Add(new object[] { "02", "000000", "20140517", "114200", "交易成功" });

        dsReturn.Tables.Add(dtTiReturn);
        dsReturn.Tables.Add(dtTouReturn);
        DataTable msg = new DataTable("结果");
        msg.Columns.Add("反馈结果");
        msg.Columns.Add("详情");
        msg.Columns.Add("视觉字符串");
        //msg.Rows.Add(new string[] {"err","接口不通",""});
        msg.Rows.Add(new string[] { "ok", "接口不通", "" });
        dsReturn.Tables.Add(msg);

        return dsReturn;
    }
    //模拟1004银行应答
    private DataSet RunQS1004_Test(DataSet dsreturn)
    {
        //调用模拟银行返回包
        DataSet dsReturn = new DataSet();
        DataTable dtTiReturn = new DataTable();
        DataTable dtTouReturn = new DataTable();
        dtTiReturn.TableName = "包体";
        dtTouReturn.TableName = "包头";

        dtTouReturn.Columns.Add("ServType");
        dtTouReturn.Columns.Add("RspCode");
        dtTouReturn.Columns.Add("TranDate");
        dtTouReturn.Columns.Add("TranTime");
        dtTouReturn.Columns.Add("RspMsg");
        dtTouReturn.Rows.Add(new object[] { "02", "000000", "20140517", "114200", "交易成功" });

        //dtTiReturn.Columns.Add("RecordCount");
        dtTiReturn.Columns.Add("ResultFlag");
        dtTiReturn.Columns.Add("FailMsg");
        dtTiReturn.Columns.Add("FailFilesName");
        dtTiReturn.Rows.Add(new object[] { "8", "", "" });

        dsReturn.Tables.Add(dtTiReturn);
        dsReturn.Tables.Add(dtTouReturn);
        DataTable msg = new DataTable("结果");
        msg.Columns.Add("反馈结果");
        msg.Columns.Add("详情");
        msg.Columns.Add("视觉字符串");
        //msg.Rows.Add(new string[] {"err","接口不通",""});
        msg.Rows.Add(new string[] { "ok", "接口不通", "" });
        dsReturn.Tables.Add(msg);

        return dsReturn;
    }

    #endregion
}