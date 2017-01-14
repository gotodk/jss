using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using FMDBHelperClass;
using FMipcClass;
using System.Collections;
using System.Data;
using FMPublicClass;
using System.Text.RegularExpressions;

[WebService(Namespace = "http://capi.ipc.com/", Description = "临时用于java，手机端调用")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]


public class forjava : System.Web.Services.WebService
{

    public forjava()
    {

        //仅用于手机版临时处理

    }
    /// <summary>
    /// 初始化返回值数据集,执行结果只有两种ok和err(大多数情况是这个标准)
    /// </summary>
    /// <returns></returns>
    private DataSet initReturnDataSet()
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
    /// 获取用户IP地址,本方法无法放到PulicClass下，需要用到HTTP的东西
    /// </summary>
    /// <returns></returns>
    private string GetIP()
    {
        string result = String.Empty;
        result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
        if (null == result || result == String.Empty)
        { result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]; }
        if (null == result || result == String.Empty) { result = HttpContext.Current.Request.UserHostAddress; }
        if (null == result || result == String.Empty || !IsIP(result)) { return "0.0.0.0"; } return result;
    }
    private bool IsIP(string ip) { return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$"); }




    /// <summary>
    /// 验证账号密码并记录登录IP
    /// </summary>
    /// <param name="user">邮箱</param>
    /// <param name="user">密码</param>
    /// <param name="SPstring">防篡改串</param>
    /// <returns></returns>
    [WebMethod]
    public DataSet checkLoginIn(string user, string pass)
    {
        DataSet dsReturn = initReturnDataSet().Clone();//克隆初始化的数据集形式
        dsReturn.Tables["返回值单条"].Rows.Add(new string[] { "", "", "", "", "", "", "" });//首先插入一条空数据行
        ////开始防篡改串检查
        //if (SPstring != StringOP.GetMD5(new object[] { user, pass }) && IsMD5check())
        //{
        //    return null;
        //}
        ////结束检查      

        #region 1.调用方法 "踢人"， 若不允许(不是"ok"的就踢)，返回特定数据集

        object[] ret = IPC.Call("踢人", new object[] { user });
        if (ret[0].ToString() == "ok")
        {
            string key = (string)(ret[1]);
            if (key != "ok")
            {
                dsReturn.Tables["返回值单条"].Rows[0]["执行结果"] = "踢下去";
                dsReturn.Tables["返回值单条"].Rows[0]["提示文本"] = key;
                return dsReturn;
            }

        }
        else
        {
            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。          
            return null;
        }

        #endregion

        #region 2.调用方法“是否邮箱重复”。如果发现重复，才正确，继续登陆，不重复，代表用户不存在。返回特定数据集。

        object[] reyx = IPC.Call("是否邮箱重复", new object[] { user });
        if (reyx[0].ToString() == "ok")
        {
            //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
            string reFF = (string)(reyx[1]);

            if (reFF != "是")
            {
                DataTable dt = dsReturn.Tables["返回值单条"];
                dsReturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsReturn.Tables["返回值单条"].Rows[0]["提示文本"] = "不存在该用户！";
                return dsReturn;
            }
        }
        else
        {
            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。          
            return null;
        }

        #endregion

        //3.获取客户端ip地址
        string ip = GetIP();

        #region 4.再调用方法“登录信息检查”。将得到的数据返回。
        object[] rezc = IPC.Call("登录信息检查", new object[] { user, pass, ip });
        if (rezc[0].ToString() == "ok")
        {
            DataSet ds = (DataSet)(rezc[1]);
            return ds;
        }
        else
        {
            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。          
            return null;
        }

        #endregion

    }
 

    /// <summary>
    /// 2014-06-16 shiyan
    /// 客户端获取分页数据的统一调用接口
    /// </summary>
    /// <param name="methodname">调用方法业务名</param>
    /// <param name="page">分页参数，包括page_index,page_size</param>
    /// <param name="tiaojian">查询条件字符串数组</param>
    /// <param name="SPstring">md5验证字符串</param>
    /// <returns>查询结果数据集</returns>
    [WebMethod]
    public DataSet GetPageData(string methodname,string page0,string page1,   string  tiaojian)
    {
        //标签名△竞标中★用户邮箱△gccyu@163.com
        string[] page = new string[] { page0, page1 };
        string[] t1 = tiaojian.Split('★');
        int t1_sl = t1.Length;
        string[][] tiaojian0 = new string[t1_sl][];
        for (int i = 0; i < t1_sl; i++)
        {
            tiaojian0[i] = t1[i].Split('△');
        }


       

        //设置分页参数
        DataSet ds_page = new DataSet(); //大分页参数集

        //通过调用web方法，获取大分页功能需要用到的参数列表，返回值为dataset类型
        //此处将page中的page_index、page_size传入，直接在初始化时赋值，不需要自行再次赋值了。
        object[] re_init = IPC.Call("定义分页参数", new object[] { page });
        if (re_init[0].ToString() == "ok")
        {
            ds_page = (DataSet)re_init[1];
        }
        else
        {//分页参数初始化失败返回null
            return null;
        }
        //开始调用实际获取分页数据的方法
        object[] re = IPC.Call(methodname, new object[] { ds_page, tiaojian0 });
        if (re[0].ToString() == "ok")
        {
            DataSet ds_old =  (DataSet)re[1];
            ds_old.AcceptChanges();
            //DataTable dtnews = ds_old.Tables[0].Copy();
            //DataSet ds_new = new DataSet();
            //ds_new.Tables.Add(dtnews);
            return ds_old;
        }
        else
        {
            return null;
        }
    }




    /// <summary>
    /// 获得商品详情页面，用于详情页面
    /// </summary>
    /// <param name="spbh">商品编号</param>
    /// <param name="htqx">合同期限</param>
    /// <param name="dlyx">登陆邮箱</param>
    /// <returns></returns>
    [WebMethod]
    public byte[] SelectSPXQ(string spbh, string htqx, string dlyx)
    {
 
        //正式开始处理
        object[] re = null;
        DataSet reFF = null;
        byte[] reFF_end = null;

        re = IPC.Call("商品详情首次", new object[] { spbh, htqx, dlyx });
        if (re[0].ToString() == "ok")
        {
            reFF = (DataSet)(re[1]);
            reFF_end = Helper.DataSet2Byte(reFF);
        }
        else
        {
            //发生错误，写入日志
            string err = re[1].ToString();
        }
        return reFF_end;
    }



    /// <summary>
    /// 获得一个数据，大盘列表
    /// </summary>
    /// <param name="sp">处理参数，参数不同作用不同</param>
    /// <returns>返回字节流</returns>
    [WebMethod]
    public byte[] GetIndexDB(string sp)
    {
 

        //正式开始处理
        object[] re = null;
        byte[] reFF = null;

        re = IPC.Call("列表加载", new object[] { sp });
        if (re[0].ToString() == "ok")
        {
            reFF = (byte[])(re[1]);
        }
        else
        {
            //发生错误，写入日志
            string err = re[1].ToString();
        }
 
        return reFF;
    }


    /// <summary>
    /// 加入或删除自选商品
    /// </summary>
    /// <param name="DLYX">登陆邮箱</param>
    /// <param name="SPBH">商品编号</param>
    /// <param name="edit">操作类型</param>
    /// <returns>返回处理结果提示文本</returns>
    [WebMethod]
    public string ZXSPedit(string DLYX, string SPBH, string edit )
    {
 
        if (edit == "加入")
        {
            IPC.Call("自选添加", new object[] { DLYX, SPBH });
            return "成功添加到自选商品！";
        }
        if (edit == "删除")
        {
            IPC.Call("自选删除", new object[] { DLYX, SPBH });
            return "成功从自选商品中删除！";
        }
        return null;

    }
    
        

 




    /// <summary>
    /// 提交预订单
    /// </summary>
    /// <param name="YXJZRQ">格式2014-11-27 10:05:04。 也可能是“”</param>
    /// <param name="SPBH">商品编号</param>
    /// <param name="FLAG">预执行/真正执行</param>
    /// <param name="HTQX">合同期限</param>
    /// <param name="NMRJG">拟买入价格 两位小数</param>
    /// <param name="DLYX">登录邮箱</param>
    /// <param name="SHQYsheng">收货区域省份  “天津市”</param>
    /// <param name="MJJSBH"></param>
    /// <param name="SHQY">合并字符串：   |天津市天津市辖区| </param>
    /// <param name="SHQYshi">收货区域市  “天津市辖区”</param>
    /// <param name="NDGSL">拟订购数量 整数</param>
    /// <param name="SPstring">防篡改</param>
    /// <returns></returns>
    [WebMethod]
    public DataSet SetYDD(string YXJZRQ,string SPBH,string FLAG,string HTQX,string NMRJG,string DLYX,string SHQYsheng,string MJJSBH,string SHQY,string SHQYshi,string NDGSL, string SPstring)
    {

        //开始防篡改串检查
        if (SPstring != StringOP.GetMD5forjava(YXJZRQ + SPBH  + FLAG + HTQX + NMRJG  + DLYX + SHQYsheng + MJJSBH + SHQY + SHQYshi + NDGSL))
        {
            return null;
        }

        DataSet dsGO = new DataSet("YDDds");
        DataTable dtGO = new DataTable("YDD");
        //必填
        dtGO.Columns.Add("YXJZRQ");
        dtGO.Columns.Add("SPBH");
        dtGO.Columns.Add("FLAG");
        dtGO.Columns.Add("HTQX");
        dtGO.Columns.Add("NMRJG");
        dtGO.Columns.Add("DLYX");
        dtGO.Columns.Add("SHQYsheng");
        dtGO.Columns.Add("MJJSBH");
        dtGO.Columns.Add("SHQY");
        dtGO.Columns.Add("SHQYshi");
        dtGO.Columns.Add("NDGSL");
        //空
        dtGO.Columns.Add("GLJJRYX");
        dtGO.Columns.Add("GLJJRYHM");
        dtGO.Columns.Add("GLJJRJSBH");
        dtGO.Columns.Add("GLJJRPTGLJG");
        dtGO.Columns.Add("SPMC");
        dtGO.Columns.Add("GG");
        dtGO.Columns.Add("JJDW");
        dtGO.Columns.Add("PTSDZDJJPL");
        dtGO.Columns.Add("YZBSL");
        dtGO.Columns.Add("MJDJBL");
        dtGO.Columns.Add("NDGJE");
        dtGO.Columns.Add("DJDJ");
        dtGO.Columns.Add("ZT");
        dtGO.Columns.Add("JSZHLX");

        dtGO.Rows.Add(new string[] { YXJZRQ,  SPBH,  FLAG, HTQX, NMRJG, DLYX, SHQYsheng, MJJSBH, SHQY, SHQYshi, NDGSL, "", "", "", "", "", "", "", "", "", "", "", "", "", "" });
        dsGO.Tables.Add(dtGO);
        capi c = new capi();
        string SPstring_gogo = StringOP.GetMD5(new object[] { dsGO });
        return c.SetYDD(dsGO, SPstring_gogo);
    }

    [WebMethod]
    public string md5test(string a, string b ,string c,string sp)
    {
        string md5 = StringOP.GetMD5forjava(a + b + c);
        return "参数计算值为：" + md5 + "----传入计算值为："+sp;
    }

    /// <summary>
    /// 定标操作
    /// </summary>
    /// <param name="dlyx">登陆邮箱</param>
    /// <param name="jszhlx">结算账户类型</param>
    /// <param name="seljsbh">卖家角色编号</param>
    /// <param name="tbdh">投标单号(从列表中获取)</param>
    /// <param name="number">中标定标信息表编号(从列表中获取)</param>
    /// <param name="htqx">合同期限(从列表中获取)</param>
    /// <param name="ispass">先传“no”，收到反馈再传“ok”</param>
    /// <param name="SPstring">防篡改</param>
    /// <returns></returns>
    [WebMethod]
    public DataSet Jhjx_db(string dlyx,string jszhlx,string seljsbh,string tbdh, string number, string htqx,string ispass, string SPstring)
    {
        //开始防篡改串检查
        if (SPstring != StringOP.GetMD5forjava(dlyx + jszhlx + seljsbh + tbdh + number + htqx + ispass))
        {
            return null;
        }


        DataTable dsforparameter = new DataTable();
        dsforparameter.TableName = "传递参数";
        ArrayList zhi = new ArrayList();

        dsforparameter.Columns.Add("登陆邮箱", typeof(string));
        zhi.Add(dlyx);
        dsforparameter.Columns.Add("结算账户类型", typeof(string));
        zhi.Add(jszhlx);
        dsforparameter.Columns.Add("卖家角色编号", typeof(string));
        zhi.Add(seljsbh);
        dsforparameter.Columns.Add("投标单号", typeof(string));
        zhi.Add(tbdh);
        dsforparameter.Columns.Add("Number", typeof(string));
        zhi.Add(number);
        dsforparameter.Columns.Add("合同期限", typeof(string));
        zhi.Add(htqx);
        dsforparameter.Columns.Add("ispass", typeof(string));
        zhi.Add(ispass);
       
        dsforparameter.Rows.Add(zhi.ToArray());


        string SPstring_gogo = StringOP.GetMD5(new object[] { dsforparameter });

        capi c = new capi();
        return c.Jhjx_db(dsforparameter, SPstring_gogo);
    }

}
