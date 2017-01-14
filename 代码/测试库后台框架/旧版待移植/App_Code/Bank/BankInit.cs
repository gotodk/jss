using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Web;
using System.Web.UI.WebControls;

/// <summary>
/// 提供IBANK类初始化后的常用用户字段信息
/// </summary>
public class BankInit
{
    private string _userEmail = string.Empty;
    /// <summary>
    /// 用户登录邮箱
    /// </summary>
    public string UserEmail
    {
        get { return _userEmail; }
        set { _userEmail = value; }
    }

    private string _userPwd = string.Empty;
    /// <summary>
    /// 用户登录密码
    /// </summary>
    public string UserPwd
    {
        get { return _userPwd; }
        set { _userPwd = value; }
    }

    private string _userPwd_Bank = string.Empty;
    /// <summary>
    /// 结算账户密码
    /// </summary>
    public string UserPwd_Bank
    {
        get { return _userPwd_Bank; }
        set { _userPwd_Bank = value; }
    }

    private string _accountType = string.Empty;
    /// <summary>
    /// 结算账户类型（经纪人交易账户/买家卖家交易账户）
    /// </summary>
    public string AccountType
    {
        get { return _accountType; }
        set { _accountType = value; }
    }

    private double _userMoney_pt = 0.00;
    /// <summary>
    /// 用户在交易平台的可用余额
    /// </summary>
    public double UserMoney_PT
    {
        get { return _userMoney_pt; }
        set { _userMoney_pt = value; }
    }

    private double _userMoney_Bank = 0.00;
    /// <summary>
    /// 第三方存管可用余额
    /// </summary>
    public double UserMoney_Bank
    {
        get { return _userMoney_Bank; }
        set { _userMoney_Bank = value; }
    }

    private string _userType = string.Empty;
    /// <summary>
    /// 用户类型（单位/自然人）
    /// </summary>
    public string UserType
    {
        get { return _userType; }
        set { _userType = value; }
    }

    private string _userDealName = string.Empty;
    /// <summary>
    /// 交易方名称
    /// </summary>
    public string UserDealName
    {
        get { return _userDealName; }
        set { _userDealName = value; }
    }

    private string _idCard = string.Empty;
    /// <summary>
    /// 身份证号
    /// </summary>
    public string IdCard
    {
        get { return _idCard; }
        set { _idCard = value; }
    }

    private string _organizationCode = string.Empty;
    /// <summary>
    /// 组织机构代码证
    /// </summary>
    public string OrganizationCode
    {
        get { return _organizationCode; }
        set { _organizationCode = value; }
    }

    private string _businessCode = string.Empty;
    /// <summary>
    /// 营业执照编码
    /// </summary>
    public string BusinessCode
    {
        get { return _businessCode; }
        set { _businessCode = value; }
    }

    private string _phone_Business = string.Empty;
    /// <summary>
    /// 交易方联系电话（I_JYFLXDH）
    /// </summary>
    public string Phone_Business
    {
        get { return _phone_Business; }
        set { _phone_Business = value; }
    }

    private string _phone = string.Empty;
    /// <summary>
    /// 联系手机
    /// </summary>
    public string Phone
    {
        get { return _phone; }
        set { _phone = value; }
    }

    private string _userName = string.Empty;
    /// <summary>
    /// 联系人姓名（I_LXRXM）
    /// </summary>
    public string UserName
    {
        get { return _userName; }
        set { _userName = value; }
    }

    private string _bank = string.Empty;
    /// <summary>
    /// 用户所属银行（浦发银行/平安银行/...）
    /// </summary>
    public string Bank
    {
        get { return _bank; }
        set { _bank = value; }
    }

    private string _bankCard = string.Empty;
    /// <summary>
    /// 银行卡号
    /// </summary>
    public string BankCard
    {
        get { return _bankCard; }
        set { _bankCard = value; }
    }

    private string _securitiesCode = string.Empty;
    /// <summary>
    /// 证券资金账号
    /// </summary>
    public string SecuritiesCode
    {
        get { return _securitiesCode; }
        set { _securitiesCode = value; }
    }

    private string _bankState = null;
    /// <summary>
    /// 第三方存管开通状态
    /// </summary>
    public string BankState
    {
        get { return _bankState; }
        set { _bankState = value; }
    }

    private string _routes = null;
    /// <summary>
    /// 用户银行实例名
    /// </summary>
    public string Routes
    {
        get { return _routes; }
        set { _routes = value; }
    }
}